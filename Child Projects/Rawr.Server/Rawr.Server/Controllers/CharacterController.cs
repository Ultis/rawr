using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using System.Net;
using Rawr;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using System.Text;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Security.Cryptography;

namespace Rawr.Server.Controllers
{
    [HandleError]
    public class CharacterController : Controller
    {
        private static string _loadedChars = "";
        private static string STATS_KEY = ConfigurationManager.AppSettings["StatsKey"];
        private static string API_PUBLIC_KEY = ConfigurationManager.AppSettings["APIPublicKey"];
        private static byte[] API_PRIVATE_KEY = System.Text.Encoding.UTF8.GetBytes(ConfigurationManager.AppSettings["APIPrivateKey"]);
        private static int CACHE_DURATION_MIN = 2;
        private static int QUERY_FREQUENCY_MS = 2000;
        private static DateTime _queueEnd = DateTime.MinValue;

        [ValidateInput(false)]
        public ActionResult Index(string request)
        {
            if (string.IsNullOrWhiteSpace(request))				return View();
            if (request.StartsWith(STATS_KEY))					return HandleStatsRequest(request);
            if (Request.HttpMethod == "POST" && request.StartsWith("~")) return HandleServerCharacterRequestPost(request);
            if (request.StartsWith("~"))						return HandleServerCharacterRequest(request);
            if (request.Contains("@") && request.Contains("-")) return HandleBattleNetCharacterRequest(request);
            return View();
        }

        #region Stats Requests
        private ActionResult HandleStatsRequest(string request)
        {
            if (request == STATS_KEY)
            {
                Response.Write(_loadedChars);
                return View();
            }
            else if (request == STATS_KEY + ".cachestats")
            {
                using (RawrDBDataContext context = new RawrDBDataContext())
                {
                    Response.Write(context.CharacterXMLs.Count());
                }
                return View();
            }
            else
            {
                int val = int.Parse(request.Split('.')[1]);
                if (val >= 1000) QUERY_FREQUENCY_MS = val;
                Response.Write(string.Format("QUERY_FREQUENCY_MS = {0}", QUERY_FREQUENCY_MS));
                return View();
            }
        }
        #endregion

        #region Battle.net Character Requests
        private ActionResult HandleBattleNetCharacterRequest(string characterRegionServer)
        {
            string characterName = characterRegionServer.EverythingBefore("@").Trim().ToLowerInvariant();
            string region = characterRegionServer.EverythingBetween("@", "-").Trim().ToLowerInvariant();
            string realm = characterRegionServer.EverythingBetween("-", "!").Trim().ToLowerInvariant();
            bool forceRefresh = characterRegionServer.Contains("!");

            if (string.IsNullOrEmpty(characterName) || string.IsNullOrEmpty(region) || string.IsNullOrEmpty(realm))
                return View();

            DateTime lastRefreshed;
            string charXml = GetCachedCharacterXml(characterName, region, realm, out lastRefreshed);
            if (forceRefresh)
            {
                charXml = null;
                lastRefreshed = DateTime.MinValue;
            }
            if (charXml == null || lastRefreshed < DateTime.Now.AddMinutes(-CACHE_DURATION_MIN))
            {
                try
                {
                    string json = GetBattleNetJson(characterName, region, realm, lastRefreshed);
                    if (json != null)
                    {
                        try
                        {
                            Character character = ConvertBattleNetJsonToCharacter(json, region);
                            charXml = ConvertCharacterToXml(character);

                            using (RawrDBDataContext context = new RawrDBDataContext())
                            {
                                var characterXML = context.CharacterXMLs
                                                    .Where(cxml =>
                                                        cxml.CharacterName == characterName &&
                                                        cxml.Region == region &&
                                                        cxml.Realm == realm)
                                                    .FirstOrDefault();
                                if (characterXML == null)
                                {
                                    characterXML = new CharacterXML()
                                    {
                                        CharacterName = characterName,
                                        Region = region,
                                        Realm = realm
                                    };
                                    context.CharacterXMLs.InsertOnSubmit(characterXML);
                                }
                                characterXML.LastRefreshed = DateTime.Now;
                                characterXML.XML = charXml;
                                characterXML.CurrentModel = character.CurrentModel;
                                context.SubmitChanges();
                            }

                            _loadedChars += string.Format("{3}: Loaded {0}@{1}-{2} ({4})\r\n", characterName, region, realm, DateTime.Now, character.CurrentModel);
                        }
                        catch (Exception ex)
                        {
                            _loadedChars += string.Format("{3}: ERROR PARSING - {0}@{1}-{2}\r\n", characterName, region, realm, DateTime.Now);
                            Response.Write(string.Format("<Error>{0}</Error>", ex));
                            return View();
                        }
                    }
                }
                catch (Exception ex)
                {
                    _loadedChars += string.Format("{3}: ERROR PARSING - {0}@{1}-{2}\r\n", characterName, region, realm, DateTime.Now);

                    WebException wex = ex as WebException;
                    if (wex != null)
                    {
                        Response.Write(string.Format("<Error>{0}</Error>", new StreamReader(wex.Response.GetResponseStream()).ReadToEnd()));
                        return View();
                    }
                    else
                    {
                        Response.Write(string.Format("<Error>{0}</Error>", ex));
                        return View();
                    }
                }
            }

            Response.Write(charXml ?? string.Format("<Error>There was an error resulting in null charXml. {0} could not be loaded</Error>", characterRegionServer.TrimEnd('!')));
            return View();
        }

        private string GetCachedCharacterXml(string characterName, string region, string realm, out DateTime lastRefreshed)
        {
            string xml = null;
            lastRefreshed = DateTime.MinValue;
            using (RawrDBDataContext context = new RawrDBDataContext())
            {
                var cxml = context.CharacterXMLs
                    .Where(chtml =>
                        chtml.CharacterName == characterName &&
                        chtml.Region == region &&
                        chtml.Realm == realm)
                    .FirstOrDefault();
                if (cxml != null)
                {
                    _loadedChars += string.Format("{3}: Loaded {0}@{1}-{2} (Cached, {4})\r\n", characterName, region, realm, DateTime.Now, cxml.CurrentModel);
                    xml = cxml.XML;
                    lastRefreshed = cxml.LastRefreshed;
                }
            }
            return xml;
        }

        private string GetBattleNetJson(string characterName, string region, string realm, DateTime lastRefreshed)
        {
            double secWaited = WaitInQueryQueue();
            _loadedChars += string.Format("{3}: Loading {0}@{1}-{2} ({4:N1}s delay)\r\n", characterName, region, realm, DateTime.Now, secWaited);

            string apiserver;
            switch (region)
            {
                case "eu":
                    apiserver = "eu.battle.net";
                    break;
                case "kr":
                    apiserver = "kr.battle.net";
                    break;
                case "tw":
                    apiserver = "tw.battle.net";
                    break;
                case "cn":
                    apiserver = "battlenet.com.cn";
                    break;
                case "us":
                default:
                    apiserver = "us.battle.net";
                    break;
            }
            string url = string.Format("http://{1}/api/wow/character/{2}/{0}?fields=talents,items,professions,pets", characterName, apiserver, realm);

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);

            if (lastRefreshed != DateTime.MinValue)
            {
                req.IfModifiedSince = lastRefreshed;
            }

            req.Date = DateTime.Now;

            string date = req.Date.ToUniversalTime().ToString("r");
            string stringToSign = req.Method + "\n" + date + "\n" + req.RequestUri.AbsolutePath + "\n";

            byte[] buffer = Encoding.UTF8.GetBytes(stringToSign);
            HMACSHA1 hmac = new HMACSHA1(API_PRIVATE_KEY);
            byte[] hash = hmac.ComputeHash(buffer);
            string sig = Convert.ToBase64String(hash);

            string auth = "BNET " + API_PUBLIC_KEY + ":" + sig;

            req.Headers["Authorization"] = auth;

            try
            {
                using (WebResponse res = req.GetResponse())
                {
                    return new StreamReader(res.GetResponseStream()).ReadToEnd();
                }
            }
            catch (WebException ex)
            {
                HttpWebResponse h = ex.Response as HttpWebResponse;
                if (h.StatusCode != HttpStatusCode.NotModified)
                {
                    throw ex;
                }
                return null;
            }
        }

        private static double WaitInQueryQueue()
        {
            if (_queueEnd < DateTime.Now)
            {
                _queueEnd = DateTime.Now.AddMilliseconds(QUERY_FREQUENCY_MS);
                return 0;
            }
            else
            {
                var end = _queueEnd;
                _queueEnd = end.AddMilliseconds(QUERY_FREQUENCY_MS);
                TimeSpan timeToWait = end - DateTime.Now;
                System.Threading.Thread.Sleep(timeToWait);
                return timeToWait.TotalSeconds;
            }
        }

        private Character ConvertBattleNetJsonToCharacter(string json, string region)
        {
            Character character = new Character();

            // TODO consider replacing with DataContractJsonSerializer
            var dict = JsonParser.Parse(json, false);

            {
                character.Name = (string)dict["name"];
                character.Realm = (string)dict["realm"];
                character.Region = (CharacterRegion)Enum.Parse(typeof(CharacterRegion), region.ToUpperInvariant());
            }
            
            //Race
            {
                character.Race = (CharacterRace)dict["race"];
            }

            //Class
            {
                character.Class = (CharacterClass)dict["class"];
            }

            //Talents
            {
                var talent = (Dictionary<string, object>)((object[])dict["talents"])[0];
                string spec = character.Class.ToString() + "." + talent["name"];
                string talents = (string)talent["build"];
                var glyphs = (Dictionary<string, object>)talent["glyphs"];
                List<int> glyphIds = new List<int>();
                foreach (var group in glyphs)
                {
                    foreach (Dictionary<string, object> glyph in (object[])group.Value)
                    {
                        glyphIds.Add((int)glyph["item"]);
                    }
                }

                switch (character.Class)
                {
                    case CharacterClass.Warrior:
                        character.WarriorTalents = new WarriorTalents(talents);
                        character.CurrentModel = "DPSWarr";
                        break;
                    case CharacterClass.Paladin:
                        character.PaladinTalents = new PaladinTalents(talents);
                        character.CurrentModel = "ProtPaladin";
                        break;
                    case CharacterClass.Hunter:
                        character.HunterTalents = new HunterTalents(talents);
                        character.CurrentModel = "Hunter";
                        break;
                    case CharacterClass.Rogue:
                        character.RogueTalents = new RogueTalents(talents);
                        character.CurrentModel = "Rogue";
                        break;
                    case CharacterClass.Priest:
                        character.PriestTalents = new PriestTalents(talents);
                        character.CurrentModel = "ShadowPriest";
                        break;
                    case CharacterClass.DeathKnight:
                        character.DeathKnightTalents = new DeathKnightTalents(talents);
                        character.CurrentModel = "DPSDK";
                        break;
                    case CharacterClass.Shaman:
                        character.ShamanTalents = new ShamanTalents(talents);
                        character.CurrentModel = "Elemental";
                        break;
                    case CharacterClass.Mage:
                        character.MageTalents = new MageTalents(talents);
                        character.CurrentModel = "Mage";
                        break;
                    case CharacterClass.Warlock:
                        character.WarlockTalents = new WarlockTalents(talents);
                        character.CurrentModel = "Warlock";
                        break;
                    case CharacterClass.Druid:
                        character.DruidTalents = new DruidTalents(talents);
                        character.CurrentModel = "Bear";
                        break;
                }

                switch (spec)
                {
                    case "Warrior.Arms":			character.CurrentModel = "DPSWarr"; break;
                    case "Warrior.Fury":			character.CurrentModel = "DPSWarr"; break;
                    case "Warrior.Protection":		character.CurrentModel = "ProtWarr"; break;
                    case "Paladin.Holy":			character.CurrentModel = "Healadin"; break;
                    case "Paladin.Protection":		character.CurrentModel = "ProtPaladin"; break;
                    case "Paladin.Retribution":		character.CurrentModel = "Retribution"; break;
                    case "Hunter.Beast Mastery":	character.CurrentModel = "Hunter"; break;
                    case "Hunter.Marksmanship":		character.CurrentModel = "Hunter"; break;
                    case "Hunter.Survival":			character.CurrentModel = "Hunter"; break;
                    case "Rogue.Assassination":		character.CurrentModel = "Rogue"; break;
                    case "Rogue.Combat":			character.CurrentModel = "Rogue"; break;
                    case "Rogue.Subtlety":			character.CurrentModel = "Rogue"; break;
                    case "Priest.Discipline":		character.CurrentModel = "HealPriest"; break;
                    case "Priest.Holy":				character.CurrentModel = "HealPriest"; break;
                    case "Priest.Shadow":			character.CurrentModel = "ShadowPriest"; break;
                    case "DeathKnight.Blood":		character.CurrentModel = "TankDK"; break;
                    case "DeathKnight.Frost":		character.CurrentModel = "DPSDK"; break;
                    case "DeathKnight.Unholy":		character.CurrentModel = "DPSDK"; break;
                    case "Shaman.Elemental":		character.CurrentModel = "Elemental"; break;
                    case "Shaman.Enhancement":		character.CurrentModel = "Enhance"; break;
                    case "Shaman.Restoration":		character.CurrentModel = "RestoSham"; break;
                    case "Mage.Arcane":				character.CurrentModel = "Mage"; break;
                    case "Mage.Fire":				character.CurrentModel = "Mage"; break;
                    case "Mage.Frost":				character.CurrentModel = "Mage"; break;
                    case "Warlock.Affliction":		character.CurrentModel = "Warlock"; break;
                    case "Warlock.Demonology":		character.CurrentModel = "Warlock"; break;
                    case "Warlock.Destruction":		character.CurrentModel = "Warlock"; break;
                    case "Druid.Balance":			character.CurrentModel = "Moonkin"; break;
                    case "Druid.Feral Combat":		character.CurrentModel = character.DruidTalents.ThickHide > 0 ? "Bear" : "Cat"; break;
                    case "Druid.Restoration":		character.CurrentModel = "Tree"; break;
                }

                Dictionary<int, PropertyInfo> glyphProperty = new Dictionary<int, PropertyInfo>();
                foreach (PropertyInfo pi in character.CurrentTalents.GetType().GetProperties())
                {
                    GlyphDataAttribute[] glyphDatas = pi.GetCustomAttributes(typeof(GlyphDataAttribute), true) as GlyphDataAttribute[];
                    if (glyphDatas.Length > 0)
                    {
                        GlyphDataAttribute glyphData = glyphDatas[0];
                        glyphProperty[glyphData.SpellID] = pi;
                    }
                }

                foreach (int glyph in glyphIds)
                {
                    PropertyInfo pi;
                    if (glyphProperty.TryGetValue(glyph, out pi))
                    {
                        pi.SetValue(character.CurrentTalents, true, null);
                    }
                }

            }

            //Professions
            try
            {
                var professions = (object[])((Dictionary<string, object>)dict["professions"])["primary"];

                string profession1 = (string)((Dictionary<string, object>)professions[0])["name"];
                string profession2 = (string)((Dictionary<string, object>)professions[1])["name"];
                character.PrimaryProfession = (Profession)Enum.Parse(typeof(Profession), profession1);
                character.SecondaryProfession = (Profession)Enum.Parse(typeof(Profession), profession2);
            }
            catch { }
            
            //Items
            {
                var items = (Dictionary<string, object>)dict["items"];                

                character._head = ParseItemJson(items["head"]);
                character._neck = ParseItemJson(items["neck"]);
                character._shoulders = ParseItemJson(items["shoulder"]);
                character._back = ParseItemJson(items["back"]);
                character._chest = ParseItemJson(items["chest"]);
                character._shirt = ParseItemJson(items["shirt"]);
                character._tabard = ParseItemJson(items["tabard"]);
                character._wrist = ParseItemJson(items["wrist"]);
                character._hands = ParseItemJson(items["hands"]);
                character._waist = ParseItemJson(items["waist"]);
                character._legs = ParseItemJson(items["legs"]);
                character._feet = ParseItemJson(items["feet"]);
                character._finger1 = ParseItemJson(items["finger1"]);
                character._finger2 = ParseItemJson(items["finger2"]);
                character._trinket1 = ParseItemJson(items["trinket1"]);
                character._trinket2 = ParseItemJson(items["trinket2"]);
                character._mainHand = ParseItemJson(items["mainHand"]);
                character._offHand = ParseItemJson(items["offHand"]);
                character._ranged = ParseItemJson(items["ranged"]);

                var i = ((Dictionary<string, object>)((Dictionary<string, object>)items["wrist"])["tooltipParams"]);
                if (i.ContainsKey("extraSocket"))
                {
                    character.WristBlacksmithingSocketEnabled = (bool)i["extraSocket"];
                }
                i = ((Dictionary<string, object>)((Dictionary<string, object>)items["hands"])["tooltipParams"]);
                if (i.ContainsKey("extraSocket"))
                {
                    character.HandsBlacksmithingSocketEnabled = (bool)i["extraSocket"];
                }
                i = ((Dictionary<string, object>)((Dictionary<string, object>)items["waist"])["tooltipParams"]);
                if (i.ContainsKey("extraSocket"))
                {
                    character.WaistBlacksmithingSocketEnabled = (bool)i["extraSocket"];
                }
            }
    
            return character;
        }

        private string ParseItemJson(object json)
        {
            var item = (Dictionary<string, object>)json;

            int itemId = 0;
            int enchantId = 0;
            int reforgeId = 0;
            int tinkeringId = 0;
            int suffixId = 0;
            int gem1Id = 0;
            int gem2Id = 0;
            int gem3Id = 0;

            if (item.ContainsKey("id"))
            {
                itemId = (int)item["id"];
            }
            if (item.ContainsKey("tooltipParams"))
            {
                var param = (Dictionary<string, object>)item["tooltipParams"];
                if (param.ContainsKey("enchant")) enchantId = (int)param["enchant"];
                if (param.ContainsKey("reforge")) reforgeId = (int)param["reforge"];
                if (param.ContainsKey("tinker")) tinkeringId = (int)param["tinker"];
                if (param.ContainsKey("suffix")) suffixId = -(int)param["suffix"];
                if (param.ContainsKey("gem0")) gem1Id = (int)param["gem0"];
                if (param.ContainsKey("gem1")) gem2Id = (int)param["gem1"];
                if (param.ContainsKey("gem2")) gem3Id = (int)param["gem2"];
            }

            string retVal = string.Format("{0}.{1}.{2}.{3}.{4}.{5}.{6}.{7}",
                itemId,
                suffixId,
                gem1Id,
                gem2Id,
                gem3Id,
                enchantId,
                reforgeId,
                tinkeringId);
            return retVal;
        }

        private string ConvertCharacterToXml(Character character)
        {
            MemoryStream stream = new MemoryStream();
            character.Save(stream, false);
            StreamReader reader = new StreamReader(stream);

            stream.Position = 0;

            string charXml = reader.ReadToEnd();
            #region Character Weight Loss Program
            charXml = Regex.Replace(charXml, @"
  <Boss>
    <Targets />
    <BuffStates />
    <Moves />
    <Stuns />
    <Fears />
    <Roots />
    <Silences />
    <Disarms />
    <Attacks>
      <Attack>
        <Name>Generated Default Melee Attack</Name>
        <IsTheDefaultMelee>true</IsTheDefaultMelee>
        <DamagePerHit>\d+</DamagePerHit>
        <PhaseTimes>
          <item>
            <key>
              <int>1</int>
            </key>
            <value>
              <ArrayOfFloat>
                <float>0</float>
                <float>1200</float>
              </ArrayOfFloat>
            </value>
          </item>
        </PhaseTimes>
        <Missable>true</Missable>
        <Dodgable>true</Dodgable>
        <Parryable>true</Parryable>
        <Blockable>true</Blockable>
        <AffectsRole>
          <item>
            <key>
              <PLAYER_ROLES>MainTank</PLAYER_ROLES>
            </key>
            <value>
              <boolean>true</boolean>
            </value>
          </item>
          <item>
            <key>
              <PLAYER_ROLES>OffTank</PLAYER_ROLES>
            </key>
            <value>
              <boolean>true</boolean>
            </value>
          </item>
          <item>
            <key>
              <PLAYER_ROLES>TertiaryTank</PLAYER_ROLES>
            </key>
            <value>
              <boolean>true</boolean>
            </value>
          </item>
          <item>
            <key>
              <PLAYER_ROLES>MeleeDPS</PLAYER_ROLES>
            </key>
            <value>
              <boolean>false</boolean>
            </value>
          </item>
          <item>
            <key>
              <PLAYER_ROLES>RangedDPS</PLAYER_ROLES>
            </key>
            <value>
              <boolean>false</boolean>
            </value>
          </item>
          <item>
            <key>
              <PLAYER_ROLES>MainTankHealer</PLAYER_ROLES>
            </key>
            <value>
              <boolean>false</boolean>
            </value>
          </item>
          <item>
            <key>
              <PLAYER_ROLES>OffAndTertTankHealer</PLAYER_ROLES>
            </key>
            <value>
              <boolean>false</boolean>
            </value>
          </item>
          <item>
            <key>
              <PLAYER_ROLES>RaidHealer</PLAYER_ROLES>
            </key>
            <value>
              <boolean>false</boolean>
            </value>
          </item>
        </AffectsRole>
      </Attack>
    </Attacks>
  </Boss>
  <ItemFilterEnabledOverride>
    <ItemFilterEnabledOverride>
      <Name>Other</Name>
      <Enabled>true</Enabled>
    </ItemFilterEnabledOverride>
  </ItemFilterEnabledOverride>
  <CustomGemmingTemplates />
  <GemmingTemplateOverrides />
  <CustomItemInstances />", "");
            charXml = charXml.Replace(@"
  <ItemFiltersSettings_UseChecks>true</ItemFiltersSettings_UseChecks>
  <ItemFiltersSettings_0>true</ItemFiltersSettings_0>
  <ItemFiltersSettings_1>true</ItemFiltersSettings_1>
  <ItemFiltersSettings_2>true</ItemFiltersSettings_2>
  <ItemFiltersSettings_3>true</ItemFiltersSettings_3>
  <ItemFiltersSettings_4>true</ItemFiltersSettings_4>
  <ItemFiltersSettings_5>true</ItemFiltersSettings_5>
  <ItemFiltersSettings_6>true</ItemFiltersSettings_6>
  <ItemFiltersSettings_7>true</ItemFiltersSettings_7>
  <ItemFiltersSettings_SLMin>285</ItemFiltersSettings_SLMin>
  <ItemFiltersSettings_SLMax>377</ItemFiltersSettings_SLMax>
  <ItemFiltersDropSettings_UseChecks>true</ItemFiltersDropSettings_UseChecks>
  <ItemFiltersDropSettings_01>true</ItemFiltersDropSettings_01>
  <ItemFiltersDropSettings_03>true</ItemFiltersDropSettings_03>
  <ItemFiltersDropSettings_05>true</ItemFiltersDropSettings_05>
  <ItemFiltersDropSettings_10>true</ItemFiltersDropSettings_10>
  <ItemFiltersDropSettings_15>true</ItemFiltersDropSettings_15>
  <ItemFiltersDropSettings_20>true</ItemFiltersDropSettings_20>
  <ItemFiltersDropSettings_25>true</ItemFiltersDropSettings_25>
  <ItemFiltersDropSettings_29>true</ItemFiltersDropSettings_29>
  <ItemFiltersDropSettings_39>true</ItemFiltersDropSettings_39>
  <ItemFiltersDropSettings_49>true</ItemFiltersDropSettings_49>
  <ItemFiltersDropSettings_100>true</ItemFiltersDropSettings_100>
  <ItemFiltersDropSettings_SLMin>0</ItemFiltersDropSettings_SLMin>
  <ItemFiltersDropSettings_SLMax>1.0010000467300415</ItemFiltersDropSettings_SLMax>
  <ItemFiltersBindSettings_0>true</ItemFiltersBindSettings_0>
  <ItemFiltersBindSettings_1>true</ItemFiltersBindSettings_1>
  <ItemFiltersBindSettings_2>true</ItemFiltersBindSettings_2>
  <ItemFiltersBindSettings_3>true</ItemFiltersBindSettings_3>
  <ItemFiltersBindSettings_4>true</ItemFiltersBindSettings_4>
  <ItemFiltersProfSettings_UseChar>true</ItemFiltersProfSettings_UseChar>
  <ItemFiltersProfSettings_00>true</ItemFiltersProfSettings_00>
  <ItemFiltersProfSettings_01>true</ItemFiltersProfSettings_01>
  <ItemFiltersProfSettings_02>true</ItemFiltersProfSettings_02>
  <ItemFiltersProfSettings_03>true</ItemFiltersProfSettings_03>
  <ItemFiltersProfSettings_04>true</ItemFiltersProfSettings_04>
  <ItemFiltersProfSettings_05>true</ItemFiltersProfSettings_05>
  <ItemFiltersProfSettings_06>true</ItemFiltersProfSettings_06>
  <ItemFiltersProfSettings_07>true</ItemFiltersProfSettings_07>
  <ItemFiltersProfSettings_08>true</ItemFiltersProfSettings_08>
  <ItemFiltersProfSettings_09>true</ItemFiltersProfSettings_09>
  <ItemFiltersProfSettings_10>true</ItemFiltersProfSettings_10>", "");
            #endregion

            return charXml;
        }
        #endregion

        #region Server Character Requests
        private ActionResult HandleServerCharacterRequest(string characterName)
        {
            characterName = characterName.TrimStart('~').EverythingBefore("~");

            string charXml = null;
            using (RawrDBDataContext context = new RawrDBDataContext())
            {
                charXml = context.ServerCharacterXMLs
                            .Where(scxml => scxml.CharacterName == characterName)
                            .Select(scxml => scxml.XML)
                            .FirstOrDefault();
            }

            Response.Write(charXml ?? "ERROR: Character Not Found");
            return View();
        }

        private ActionResult HandleServerCharacterRequestPost(string characterNamePassword)
        {
            characterNamePassword = characterNamePassword.TrimStart('~');
            string characterName = characterNamePassword.EverythingBefore("~");
            string savePassword = characterNamePassword.Contains("~") ? characterNamePassword.EverythingAfter("~") : null;
            
            StreamReader reader = new StreamReader(Request.InputStream);
            string xml = reader.ReadToEnd();

            using (RawrDBDataContext context = new RawrDBDataContext())
            {
                var serverCharacterXML = context.ServerCharacterXMLs
                                    .Where(cxml => cxml.CharacterName == characterName)
                                    .FirstOrDefault();
                if (serverCharacterXML == null)
                {
                    serverCharacterXML = new ServerCharacterXML()
                    {
                        CharacterName = characterName,
                        SavePassword = savePassword
                    };
                    context.ServerCharacterXMLs.InsertOnSubmit(serverCharacterXML);
                }
                else if ((serverCharacterXML.SavePassword ?? savePassword) != savePassword)
                {
                    Response.Write("WRONG PASSWORD");
                    return View();
                }
                serverCharacterXML.LastModified = DateTime.Now;
                serverCharacterXML.XML = xml;
                context.SubmitChanges();
            }

            Response.Write("SUCCESS");
            return View();
        }
        #endregion

    }
}

