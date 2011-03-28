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

namespace Rawr.Server.Controllers
{
    [HandleError]
    public class CharacterController : Controller
    {
        private static string _loadedChars = "";
        private static string STATS_KEY = ConfigurationManager.AppSettings["StatsKey"];
        private static int CACHE_DURATION_MIN = 2;
        private static int QUERY_FREQUENCY_MS = 2000;
        private static DateTime _queueEnd = DateTime.MinValue;
        
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

            string charXml = null;
            if (forceRefresh || (charXml = GetCachedCharacterXml(characterName, region, realm)) == null)
            {
                string html = GetBattleNetHtml(characterName, region, realm);
                try { 
                    if (html.Contains("It’s a busy day for Battle.net!")) {
                        Response.Write(string.Format("{0} Battle.Net Server is {1}", region.ToUpper(), "Overloaded"));
                        return View();
                    /*} else if (html.Contains("Maintenance")) {
                        // TODO: Need a better check for this before implementing. Normal pages do have the word Maintenance on them.
                        Response.Write(string.Format("{0} Battle.Net Server is {1}", region, "down for Maintenance"));
                        return View();*/
                    } else {
                        try {
                            Character character = ConvertBattleNetHtmlToCharacter(html, region);
                            charXml = ConvertCharacterToXml(character);

                            using (RawrDBDataContext context = new RawrDBDataContext()) {
                                var characterXML = context.CharacterXMLs
                                                    .Where(cxml =>
                                                        cxml.CharacterName == characterName &&
                                                        cxml.Region == region &&
                                                        cxml.Realm == realm)
                                                    .FirstOrDefault();
                                if (characterXML == null) {
                                    characterXML = new CharacterXML() {
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
                        } catch {
                            _loadedChars += string.Format("{3}: ERROR PARSING - {0}@{1}-{2}\r\n", characterName, region, realm, DateTime.Now);
                        }
                    }
                } catch {
                    _loadedChars += string.Format("{3}: ERROR PARSING - {0}@{1}-{2}\r\n", characterName, region, realm, DateTime.Now);
                }
            }

            Response.Write(charXml ?? "Error");
            return View();
        }

        private string GetCachedCharacterXml(string characterName, string region, string realm)
        {
            string xml = null;
            using (RawrDBDataContext context = new RawrDBDataContext())
            {
                var cxml = context.CharacterXMLs
                    .Where(chtml =>
                        chtml.CharacterName == characterName &&
                        chtml.Region == region &&
                        chtml.Realm == realm &&
                        chtml.LastRefreshed > DateTime.Now.AddMinutes(-CACHE_DURATION_MIN))
                    .FirstOrDefault();
                if (cxml != null)
                {
                    _loadedChars += string.Format("{3}: Loaded {0}@{1}-{2} (Cached, {4})\r\n", characterName, region, realm, DateTime.Now, cxml.CurrentModel);
                    xml = cxml.XML;
                }
            }
            return xml;
        }

        private string GetBattleNetHtml(string characterName, string region, string realm)
        {
            double secWaited = WaitInQueryQueue();
            _loadedChars += string.Format("{3}: Loading {0}@{1}-{2} ({4:N1}s delay)\r\n", characterName, region, realm, DateTime.Now, secWaited);
            WebClient client = new WebClient();
            client.Encoding = Encoding.UTF8;
            client.Headers["User-Agent"] = "Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US) AppleWebKit/525.13 (KHTML, like Gecko) Chrome/0.A.B.C Safari/525.13";
            client.Headers["Accept-Language"] = "en-US";
            string html = client.DownloadString(string.Format("http://{1}.battle.net/wow/en/character/{2}/{0}/advanced", characterName, region, realm));
            html += "\r\n\r\n|||\r\n\r\n" + client.DownloadString(string.Format("http://{1}.battle.net/wow/en/character/{2}/{0}/talent/", characterName, region, realm));
            return html;
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

        private Character ConvertBattleNetHtmlToCharacter(string html, string region)
        {
            //NOTE: Since this code is going to be completely rewritten when the xml services come out, 
            //I'm just using string manipulation; Regexes would eventually be better, but not worth the dev time right now.

            Character character = new Character();

            //Name/Region/Realm <title>Созерцающий @ Вечная Песня - Game - World of Warcraft</title>
            {
                string name = html.EverythingBetween("<title>", "-");
                string[] def = name.EverythingBetween("/character/", "/\"").Split('/');
                character.Name = name.EverythingBefore("@").Trim();
                character.Realm = name.EverythingBetween("@", " - ").Replace("&#39;", "\'").Trim();
                character.Region = (CharacterRegion)Enum.Parse(typeof(CharacterRegion), region.ToUpperInvariant());
            }
            
            //Race
            {
                character.Race = (CharacterRace)Enum.Parse(typeof(CharacterRace), html.EverythingBetween("/game/race/", "\"").Replace("forsaken", "undead").Replace("-", ""), true);
            }

            //Class
            {
                character.Class = (CharacterClass)Enum.Parse(typeof(CharacterClass), html.EverythingBetween("/game/class/", "\"").Replace("-", ""), true);
            }

            //Talents
            {
                string spec = character.Class.ToString() + "." + html.EverythingBetween("class=\"spec tip\">", "</a>");
                string talents = html.EverythingAfter("new TalentCalculator({ id: \"character\"").EverythingBetween("\"", "\"");
                string glyphsAll = html.EverythingBetween("<h3 class=\"category\">Glyphs</h3>", "<script type=\"text/javascript\">");
                List<string> glyphNames = new List<string>();
                while (glyphsAll.Contains("<span class=\"name\">"))
                {
                    glyphNames.Add(glyphsAll.EverythingBetween("<span class=\"name\">", "</span>").Replace("&#39;", "\'"));
                    glyphsAll = glyphsAll.EverythingAfter("<span class=\"name\">");
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

                Dictionary<string, PropertyInfo> glyphProperty = new Dictionary<string, PropertyInfo>();
                foreach (PropertyInfo pi in character.CurrentTalents.GetType().GetProperties())
                {
                    GlyphDataAttribute[] glyphDatas = pi.GetCustomAttributes(typeof(GlyphDataAttribute), true) as GlyphDataAttribute[];
                    if (glyphDatas.Length > 0)
                    {
                        GlyphDataAttribute glyphData = glyphDatas[0];
                        glyphProperty[glyphData.Name] = pi;
                    }
                }

                foreach (string glyph in glyphNames)
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
                string profession1 = html.EverythingAfter("<span class=\"profession-details\">").EverythingBetween("<span class=\"name\">", "</span>");
                string profession2 = html.EverythingAfter("<span class=\"profession-details\">").EverythingAfter("<span class=\"profession-details\">").EverythingBetween("<span class=\"name\">", "</span>");
                character.PrimaryProfession = (Profession)Enum.Parse(typeof(Profession), profession1);
                character.SecondaryProfession = (Profession)Enum.Parse(typeof(Profession), profession2);
            }
            catch { }
            
            //Items
            {
                string itemsAll = html.EverythingBetween("<div id=\"summary-inventory\"", "<script type=\"text/javascript\">");

                character._head = ParseItemHtml(itemsAll.EverythingBetween("<div data-id=\"0\" data-type=","<div data-id"));
                character._neck = ParseItemHtml(itemsAll.EverythingBetween("<div data-id=\"1\" data-type=","<div data-id"));
                character._shoulders = ParseItemHtml(itemsAll.EverythingBetween("<div data-id=\"2\" data-type=","<div data-id"));
                character._back = ParseItemHtml(itemsAll.EverythingBetween("<div data-id=\"14\" data-type=","<div data-id"));
                character._chest = ParseItemHtml(itemsAll.EverythingBetween("<div data-id=\"4\" data-type=","<div data-id"));
                character._shirt = ParseItemHtml(itemsAll.EverythingBetween("<div data-id=\"3\" data-type=","<div data-id"));
                character._tabard = ParseItemHtml(itemsAll.EverythingBetween("<div data-id=\"18\" data-type=","<div data-id"));
                character._wrist = ParseItemHtml(itemsAll.EverythingBetween("<div data-id=\"8\" data-type=","<div data-id"));
                character._hands = ParseItemHtml(itemsAll.EverythingBetween("<div data-id=\"9\" data-type=","<div data-id"));
                character._waist = ParseItemHtml(itemsAll.EverythingBetween("<div data-id=\"5\" data-type=","<div data-id"));
                character._legs = ParseItemHtml(itemsAll.EverythingBetween("<div data-id=\"6\" data-type=","<div data-id"));
                character._feet = ParseItemHtml(itemsAll.EverythingBetween("<div data-id=\"7\" data-type=","<div data-id"));
                character._finger1 = ParseItemHtml(itemsAll.EverythingBetween("<div data-id=\"10\" data-type=","<div data-id"));
                character._finger2 = ParseItemHtml(itemsAll.EverythingBetween("<div data-id=\"11\" data-type=","<div data-id"));
                character._trinket1 = ParseItemHtml(itemsAll.EverythingBetween("<div data-id=\"12\" data-type=","<div data-id"));
                character._trinket2 = ParseItemHtml(itemsAll.EverythingBetween("<div data-id=\"13\" data-type=","<div data-id"));
                character._mainHand = ParseItemHtml(itemsAll.EverythingBetween("<div data-id=\"15\" data-type=","<div data-id"));
                character._offHand = ParseItemHtml(itemsAll.EverythingBetween("<div data-id=\"16\" data-type=","<div data-id"));
                character._ranged = ParseItemHtml(itemsAll.EverythingBetween("<div data-id=\"17\" data-type=","<div data-id"));

                character.WristBlacksmithingSocketEnabled = itemsAll.EverythingBetween("<div data-id=\"8\" data-type=", "<div data-id").Contains("socket-14");
                character.HandsBlacksmithingSocketEnabled = itemsAll.EverythingBetween("<div data-id=\"9\" data-type=", "<div data-id").Contains("socket-14");
                character.WaistBlacksmithingSocketEnabled = itemsAll.EverythingBetween("<div data-id=\"5\" data-type=", "<div data-id").Contains("socket-14");
                
                /*
                 * 0/1 = Head
                 * 1/2 = Neck
                 * 2/3 = Shoulders
                 * 14/16 = Back
                 * 4/5 = Chest
                 * 3/4 = Shirt
                 * 18/19 = Tabard
                 * 8/9 = Wrist
                 * 9/10 = Hands
                 * 5/6 = Waist
                 * 6/7 = Legs
                 * 7/8 = Feet
                 * 10/11 = Ring1
                 * 11/11 = Ring2
                 * 12/12 = Trinket1
                 * 13/12 = Trinket2
                 * 15/21 = Mainhand
                 * 16/22 = Offhand
                 * 17/28 = Ranged
                 */
            }
    
            return character;
        }

        private string ParseItemHtml(string html)
        {
            if (html.EverythingBefore(" class=\"sockets\"").Contains("class=\"empty\"")) return null;
            string itemId = html.EverythingBetween("data-item=\"i=", "&").EverythingBefore("\"");
            string enchantId = html.Contains("&amp;e=") ? html.EverythingBetween("&amp;e=", "&").EverythingBefore("\"") : null;
            string reforgeId = html.Contains("&amp;re=") ? (int.Parse(html.EverythingBetween("&amp;re=", "&").EverythingBefore("\"")) - 56).ToString() : null;
            string tinkeringId = html.Contains("&amp;ee=") ? (int.Parse(html.EverythingBetween("&amp;ee=", "&").EverythingBefore("\""))).ToString() : null;
            string suffixId = html.Contains("&amp;r=-") ? html.EverythingBetween("&amp;r=-", "&").EverythingBefore("\"") : null;
            string gem1Id = null;
            string gem2Id = null;
            string gem3Id = null;

            if (html.Contains("<span class=\"sockets\">"))
            {
                string sockets = html.EverythingAfter("<span class=\"sockets\">");
                while (sockets.Contains("/wow/en/item/"))
                {
                    sockets = sockets.EverythingAfter("/wow/en/item/");
                    if (gem1Id == null) gem1Id = sockets.EverythingBefore("\"");
                    else if (gem2Id == null) gem2Id = sockets.EverythingBefore("\"");
                    else if (gem3Id == null) gem3Id = sockets.EverythingBefore("\"");
                }
            }

            return string.Format("{0}.{1}.{2}.{3}.{4}.{5}.{6}.{7}",
                itemId,
                suffixId ?? "0",
                gem1Id ?? "0",
                gem2Id ?? "0",
                gem3Id ?? "0",
                enchantId ?? "0",
                reforgeId ?? "0",
                tinkeringId ?? "0");
        }

        private string ConvertCharacterToXml(Character character)
        {
            MemoryStream stream = new MemoryStream();
            character.Save(stream, false);
            StreamReader reader = new StreamReader(stream);

            stream.Position = 0;

            string charXml = reader.ReadToEnd();
            #region Character Weight Loss Program
            charXml = charXml.Replace(@"
  <CustomGemmingTemplates />
  <GemmingTemplateOverrides />
  <ItemFilterEnabledOverride>
    <ItemFilterEnabledOverride>
      <Name>Other</Name>
      <Enabled>true</Enabled>
    </ItemFilterEnabledOverride>
  </ItemFilterEnabledOverride>
  <Boss>
    <Targets />
    <BuffStates />
    <Moves />
    <Stuns />
    <Fears />
    <Roots />
    <Silences />
    <Disarms />
    <Name>Generic</Name>
    <Content>T11_0</Content>
    <Instance>None</Instance>
    <Version>V_10N</Version>
    <Comment>No comments have been written for this Boss.</Comment>
    <Level>88</Level>
    <Armor>11977</Armor>
    <BerserkTimer>600</BerserkTimer>
    <SpeedKillTimer>180</SpeedKillTimer>
    <Health>1000000</Health>
    <InBackPerc_Melee>0</InBackPerc_Melee>
    <InBackPerc_Ranged>0</InBackPerc_Ranged>
    <Max_Players>10</Max_Players>
    <Min_Healers>3</Min_Healers>
    <Min_Tanks>2</Min_Tanks>
    <DoTs />
    <Attacks />
    <Resist_Physical>0</Resist_Physical>
    <Resist_Frost>0</Resist_Frost>
    <Resist_Fire>0</Resist_Fire>
    <Resist_Nature>0</Resist_Nature>
    <Resist_Arcane>0</Resist_Arcane>
    <Resist_Shadow>0</Resist_Shadow>
    <Resist_Holy>0</Resist_Holy>
    <TimeBossIsInvuln>0</TimeBossIsInvuln>
    <InBack>false</InBack>
    <MultiTargs>false</MultiTargs>
    <HasBuffStates>false</HasBuffStates>
    <StunningTargs>false</StunningTargs>
    <MovingTargs>false</MovingTargs>
    <FearingTargs>false</FearingTargs>
    <RootingTargs>false</RootingTargs>
    <SilencingTargs>false</SilencingTargs>
    <DisarmingTargs>false</DisarmingTargs>
    <DamagingTargs>false</DamagingTargs>
    <Under35Perc>0.1</Under35Perc>
    <Under20Perc>0.15</Under20Perc>
    <FilterType>Content</FilterType>
    <Filter />
    <BossName />
  </Boss>", "");
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

