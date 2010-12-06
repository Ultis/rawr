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

namespace Rawr.Server.Controllers
{
	[HandleError]
	public class CharacterController : Controller
	{
		public ActionResult Load(string characterRegionServer)
		{
			if (string.IsNullOrWhiteSpace(characterRegionServer)) return View();

			string[] charSplit = characterRegionServer.ToLowerInvariant().Split('@', '-');
			if (charSplit.Length < 3) return View();
			string characterName = charSplit[0];
			string region = charSplit[1];
			string realm = charSplit[2];

			string html = GetBattleNetHtmlFromCharacterDefinition(characterName, region, realm);
			Character character = ConvertBattleNetHtmlToCharacter(html);
			string charXml = ConvertCharacterToXml(character);

			Response.Write(charXml);
			return View();
		}

		private string GetBattleNetHtmlFromCharacterDefinition(string characterName, string region, string realm)
		{
			WebClient client = new WebClient();
			client.Headers["User-Agent"] = "Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US) AppleWebKit/525.13 (KHTML, like Gecko) Chrome/0.A.B.C Safari/525.13";
			string html = client.DownloadString(string.Format("http://{1}.battle.net/wow/en/character/{2}/{0}/advanced", characterName, region, realm));
			html += "\r\n\r\n|||\r\n\r\n" + client.DownloadString(string.Format("http://{1}.battle.net/wow/en/character/{2}/{0}/talent/", characterName, region, realm));
			return html;
		}

		private Character ConvertBattleNetHtmlToCharacter(string html)
		{
			//NOTE: Since this code is going to be completely rewritten when the xml services come out, 
			//I'm just using string manipulation; Regexes 

			Character character = new Character();

			//Name/Region/Realm
			{
				string[] def = html.EverythingBetween("\"character|", "\"").Split('|');
				character.Name = def[1];
				character.Region = (CharacterRegion)Enum.Parse(typeof(CharacterRegion), def[0].ToUpperInvariant());
				character.Realm = def[2][0].ToString().ToUpperInvariant() + def[2].Substring(1);
			}
			
			//Race
			{
				character.Race = (CharacterRace)Enum.Parse(typeof(CharacterRace), html.EverythingBetween("<span class=\"race\">", "</span>").Replace(" ", ""));
			}

			//Class
			{
				character.Class = (CharacterClass)Enum.Parse(typeof(CharacterClass), html.EverythingBetween("<span class=\"class\">", "</span>").Replace(" ", ""));
			}

			//Talents
			{
				string spec = character.Class.ToString() + "." + html.EverythingBetween("class=\"spec tip\">", "</a>");
				string talents = html.EverythingAfter("new TalentCalculator({ id: \"character\"").EverythingBetween("\"", "\"");
				string glyphsAll = html.EverythingBetween("new Talents.Glyphs({", "});");
				List<string> glyphNames = new List<string>();
				while (glyphsAll.Contains("name:"))
				{
					glyphNames.Add(glyphsAll.EverythingBetween("name: \"", "\""));
					glyphsAll = glyphsAll.EverythingAfter(",");
				}

				switch (character.Class)
				{
					case CharacterClass.Warrior:
						character.WarriorTalents = new WarriorTalents(talents);
						break;
					case CharacterClass.Paladin:
						character.PaladinTalents = new PaladinTalents(talents);
						break;
					case CharacterClass.Hunter:
						character.HunterTalents = new HunterTalents(talents);
						break;
					case CharacterClass.Rogue:
						character.RogueTalents = new RogueTalents(talents);
						break;
					case CharacterClass.Priest:
						character.PriestTalents = new PriestTalents(talents);
						break;
					case CharacterClass.DeathKnight:
						character.DeathKnightTalents = new DeathKnightTalents(talents);
						break;
					case CharacterClass.Shaman:
						character.ShamanTalents = new ShamanTalents(talents);
						break;
					case CharacterClass.Mage:
						character.MageTalents = new MageTalents(talents);
						break;
					case CharacterClass.Warlock:
						character.WarlockTalents = new WarlockTalents(talents);
						break;
					case CharacterClass.Druid:
						character.DruidTalents = new DruidTalents(talents);
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
			{
				string profession1 = html.EverythingAfter("<span class=\"profession-details\">").EverythingBetween("<span class=\"name\">", "</span>");
				string profession2 = html.EverythingAfter("<span class=\"profession-details\">").EverythingAfter("<span class=\"profession-details\">").EverythingBetween("<span class=\"name\">", "</span>");
				character.PrimaryProfession = (Profession)Enum.Parse(typeof(Profession), profession1);
				character.SecondaryProfession = (Profession)Enum.Parse(typeof(Profession), profession2);
			}
			
			//Items
			{
				string itemsAll = html.EverythingBetween("<div id=\"summary-inventory\"", "<script type=\"text/javascript\">");

				character._head = ParseItemHtml(itemsAll.EverythingBetween("<div data-id=\"0\" data-type=","\r\n\t</div>"));
				character._neck = ParseItemHtml(itemsAll.EverythingBetween("<div data-id=\"1\" data-type=","\r\n\t</div>"));
				character._shoulders = ParseItemHtml(itemsAll.EverythingBetween("<div data-id=\"2\" data-type=","\r\n\t</div>"));
				character._back = ParseItemHtml(itemsAll.EverythingBetween("<div data-id=\"14\" data-type=","\r\n\t</div>"));
				character._chest = ParseItemHtml(itemsAll.EverythingBetween("<div data-id=\"4\" data-type=","\r\n\t</div>"));
				character._shirt = ParseItemHtml(itemsAll.EverythingBetween("<div data-id=\"3\" data-type=","\r\n\t</div>"));
				character._tabard = ParseItemHtml(itemsAll.EverythingBetween("<div data-id=\"18\" data-type=","\r\n\t</div>"));
				character._wrist = ParseItemHtml(itemsAll.EverythingBetween("<div data-id=\"8\" data-type=","\r\n\t</div>"));
				character._hands = ParseItemHtml(itemsAll.EverythingBetween("<div data-id=\"9\" data-type=","\r\n\t</div>"));
				character._waist = ParseItemHtml(itemsAll.EverythingBetween("<div data-id=\"5\" data-type=","\r\n\t</div>"));
				character._legs = ParseItemHtml(itemsAll.EverythingBetween("<div data-id=\"6\" data-type=","\r\n\t</div>"));
				character._feet = ParseItemHtml(itemsAll.EverythingBetween("<div data-id=\"7\" data-type=","\r\n\t</div>"));
				character._finger1 = ParseItemHtml(itemsAll.EverythingBetween("<div data-id=\"10\" data-type=","\r\n\t</div>"));
				character._finger2 = ParseItemHtml(itemsAll.EverythingBetween("<div data-id=\"11\" data-type=","\r\n\t</div>"));
				character._trinket1 = ParseItemHtml(itemsAll.EverythingBetween("<div data-id=\"12\" data-type=","\r\n\t</div>"));
				character._trinket2 = ParseItemHtml(itemsAll.EverythingBetween("<div data-id=\"13\" data-type=","\r\n\t</div>"));
				character._mainHand = ParseItemHtml(itemsAll.EverythingBetween("<div data-id=\"15\" data-type=","\r\n\t</div>"));
				character._offHand = ParseItemHtml(itemsAll.EverythingBetween("<div data-id=\"16\" data-type=","\r\n\t</div>"));
				character._ranged = ParseItemHtml(itemsAll.EverythingBetween("<div data-id=\"17\" data-type=","\r\n\t</div>"));


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


			//character.HandsBlacksmithingSocketEnabled = //TODO
			//character.WristBlacksmithingSocketEnabled = //TODO
			character.WaistBlacksmithingSocketEnabled = true; //TODO
			
			
			return character;
		}

		private string ParseItemHtml(string html)
		{
			if (html.Contains("class=\"empty\"")) return null;
			string itemId = html.EverythingBetween("data-item=\"i=", "&").EverythingBefore("\"");
			string enchantId = html.Contains("&amp;e=") ? html.EverythingBetween("&amp;e=", "&").EverythingBefore("\"") : null;
			string reforgeId = html.Contains("&amp;re=") ? (int.Parse(html.EverythingBetween("&amp;re=", "&").EverythingBefore("\""))-56).ToString() : null;
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

			return string.Format("{0}.{1}.{2}.{3}.{4}.{5}",
				itemId,
				gem1Id ?? "0",
				gem2Id ?? "0",
				gem3Id ?? "0",
				enchantId ?? "0",
				reforgeId ?? "0");
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
    <Armor>10643</Armor>
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
			#endregion

			return charXml;
		}
	}
}

