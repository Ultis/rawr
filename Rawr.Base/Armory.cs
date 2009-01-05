using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Xml;
using System.Text.RegularExpressions;

namespace Rawr
{
	public static class Armory
	{
        public static Character GetCharacter(Character.CharacterRegion region, string realm, string name)
        {
            string[] ignore;
            return GetCharacter(region, realm, name,out ignore);
        }

		public static Character GetCharacter(Character.CharacterRegion region, string realm, string name, out string[] itemsOnCharacter)
		{
			XmlDocument docCharacter = null;
            //XmlDocument docTalents = null;
            try
			{
				WebRequestWrapper wrw = new WebRequestWrapper();
				docCharacter = wrw.DownloadCharacterSheet(name, region, realm);
                if (docCharacter == null)
                {
                    StatusMessaging.ReportError("Get Character", null, "No character returned from the Armory");
                    itemsOnCharacter = null;
                    return null;
                }
                Character.CharacterRace race = (Character.CharacterRace)Int32.Parse(docCharacter.SelectSingleNode("page/characterInfo/character").Attributes["raceId"].Value);
                Character.CharacterClass charClass = (Character.CharacterClass)Int32.Parse(docCharacter.SelectSingleNode("page/characterInfo/character").Attributes["classId"].Value);

				Dictionary<Character.CharacterSlot, string> items = new Dictionary<Character.CharacterSlot, string>();
				Dictionary<Character.CharacterSlot, int> enchants = new Dictionary<Character.CharacterSlot, int>();

				foreach (XmlNode itemNode in docCharacter.SelectNodes("page/characterInfo/characterTab/items/item"))
				{
					int slot = int.Parse(itemNode.Attributes["slot"].Value) + 1;
                    Character.CharacterSlot cslot = Character.GetCharacterSlotFromId(slot);
					items[cslot] = string.Format("{0}.{1}.{2}.{3}", itemNode.Attributes["id"].Value,
						itemNode.Attributes["gem0Id"].Value, itemNode.Attributes["gem1Id"].Value, itemNode.Attributes["gem2Id"].Value);
					enchants[cslot] = int.Parse(itemNode.Attributes["permanentenchant"].Value);
				}

				if (items.ContainsKey(Character.CharacterSlot.Wrist))
				{
					string[] wristIds = items[Character.CharacterSlot.Wrist].Split('.');
					Item wristItemRaw = null;
					string keyStartsWith = wristIds[0] + ".";
					foreach (string key in ItemCache.Items.Keys)
						if (key.StartsWith(keyStartsWith))
						{
							wristItemRaw = ItemCache.Items[key][0];
							break;
						}
					wristItemRaw = wristItemRaw ?? GetItem(wristIds[0] + ".0.0.0", "Loading Character from Armory");
					if (wristItemRaw.Sockets.Color1 == Item.ItemSlot.None && wristIds[1] != "0")
					{
						items[Character.CharacterSlot.Wrist] = string.Format("{0}.0.0.0", wristIds[0]);
						items[Character.CharacterSlot.ExtraWristSocket] = wristIds[1] + ".0.0.0";
					}
					else if (wristItemRaw.Sockets.Color2 == Item.ItemSlot.None && wristIds[2] != "0")
					{
						items[Character.CharacterSlot.Wrist] = string.Format("{0}.{1}.0.0", wristIds[0], wristIds[1]);
						items[Character.CharacterSlot.ExtraWristSocket] = wristIds[2] + ".0.0.0";
					}
					else if (wristItemRaw.Sockets.Color3 == Item.ItemSlot.None && wristIds[3] != "0")
					{
						items[Character.CharacterSlot.Wrist] = string.Format("{0}.{1}.{2}.0", wristIds[0], wristIds[1], wristIds[2]);
						items[Character.CharacterSlot.ExtraWristSocket] = wristIds[3] + ".0.0.0";
					}
				}
				if (items.ContainsKey(Character.CharacterSlot.Hands))
				{
					string[] handsIds = items[Character.CharacterSlot.Hands].Split('.');
					Item handsItemRaw = null;
					string keyStartsWith = handsIds[0] + ".";
					foreach (string key in ItemCache.Items.Keys)
						if (key.StartsWith(keyStartsWith))
						{
							handsItemRaw = ItemCache.Items[key][0];
							break;
						}
					handsItemRaw = handsItemRaw ?? GetItem(handsIds[0] + ".0.0.0", "Loading Character from Armory");
					if (handsItemRaw.Sockets.Color1 == Item.ItemSlot.None && handsIds[1] != "0")
					{
						items[Character.CharacterSlot.Hands] = string.Format("{0}.0.0.0", handsIds[0]);
						items[Character.CharacterSlot.ExtraHandsSocket] = handsIds[1] + ".0.0.0";
					}
					else if (handsItemRaw.Sockets.Color2 == Item.ItemSlot.None && handsIds[2] != "0")
					{
						items[Character.CharacterSlot.Hands] = string.Format("{0}.{1}.0.0", handsIds[0], handsIds[1]);
						items[Character.CharacterSlot.ExtraHandsSocket] = handsIds[2] + ".0.0.0";
					}
					else if (handsItemRaw.Sockets.Color3 == Item.ItemSlot.None && handsIds[3] != "0")
					{
						items[Character.CharacterSlot.Hands] = string.Format("{0}.{1}.{2}.0", handsIds[0], handsIds[1], handsIds[2]);
						items[Character.CharacterSlot.ExtraHandsSocket] = handsIds[3] + ".0.0.0";
					}
				}
				if (items.ContainsKey(Character.CharacterSlot.Waist))
				{
					string[] waistIds = items[Character.CharacterSlot.Waist].Split('.');
					Item waistItemRaw = null;
					string keyStartsWith = waistIds[0] + ".";
					foreach (string key in ItemCache.Items.Keys)
						if (key.StartsWith(keyStartsWith))
						{
							waistItemRaw = ItemCache.Items[key][0];
							break;
						}
					waistItemRaw = waistItemRaw ?? GetItem(waistIds[0] + ".0.0.0", "Loading Character from Armory");
					if (waistItemRaw.Sockets.Color1 == Item.ItemSlot.None && waistIds[1] != "0")
					{
						items[Character.CharacterSlot.Waist] = string.Format("{0}.0.0.0", waistIds[0]);
						items[Character.CharacterSlot.ExtraWaistSocket] = waistIds[1] + ".0.0.0";
					}
					else if (waistItemRaw.Sockets.Color2 == Item.ItemSlot.None && waistIds[2] != "0")
					{
						items[Character.CharacterSlot.Waist] = string.Format("{0}.{1}.0.0", waistIds[0], waistIds[1]);
						items[Character.CharacterSlot.ExtraWaistSocket] = waistIds[2] + ".0.0.0";
					}
					else if (waistItemRaw.Sockets.Color3 == Item.ItemSlot.None && waistIds[3] != "0")
					{
						items[Character.CharacterSlot.Waist] = string.Format("{0}.{1}.{2}.0", waistIds[0], waistIds[1], waistIds[2]);
						items[Character.CharacterSlot.ExtraWaistSocket] = waistIds[3] + ".0.0.0";
					}
				}

                itemsOnCharacter = new string[items.Values.Count];
                items.Values.CopyTo(itemsOnCharacter, 0);
				Character character = new Character(name, realm, region, race,
					items.ContainsKey(Character.CharacterSlot.Head) ? items[Character.CharacterSlot.Head] : null,
					items.ContainsKey(Character.CharacterSlot.Neck) ? items[Character.CharacterSlot.Neck] : null,
					items.ContainsKey(Character.CharacterSlot.Shoulders) ? items[Character.CharacterSlot.Shoulders] : null,
					items.ContainsKey(Character.CharacterSlot.Back) ? items[Character.CharacterSlot.Back] : null,
					items.ContainsKey(Character.CharacterSlot.Chest) ? items[Character.CharacterSlot.Chest] : null,
					items.ContainsKey(Character.CharacterSlot.Shirt) ? items[Character.CharacterSlot.Shirt] : null,
					items.ContainsKey(Character.CharacterSlot.Tabard) ? items[Character.CharacterSlot.Tabard] : null,
					items.ContainsKey(Character.CharacterSlot.Wrist) ? items[Character.CharacterSlot.Wrist] : null,
					items.ContainsKey(Character.CharacterSlot.Hands) ? items[Character.CharacterSlot.Hands] : null,
					items.ContainsKey(Character.CharacterSlot.Waist) ? items[Character.CharacterSlot.Waist] : null,
					items.ContainsKey(Character.CharacterSlot.Legs) ? items[Character.CharacterSlot.Legs] : null,
					items.ContainsKey(Character.CharacterSlot.Feet) ? items[Character.CharacterSlot.Feet] : null,
					items.ContainsKey(Character.CharacterSlot.Finger1) ? items[Character.CharacterSlot.Finger1] : null,
					items.ContainsKey(Character.CharacterSlot.Finger2) ? items[Character.CharacterSlot.Finger2] : null,
					items.ContainsKey(Character.CharacterSlot.Trinket1) ? items[Character.CharacterSlot.Trinket1] : null,
					items.ContainsKey(Character.CharacterSlot.Trinket2) ? items[Character.CharacterSlot.Trinket2] : null,
					items.ContainsKey(Character.CharacterSlot.MainHand) ? items[Character.CharacterSlot.MainHand] : null,
					items.ContainsKey(Character.CharacterSlot.OffHand) ? items[Character.CharacterSlot.OffHand] : null,
					items.ContainsKey(Character.CharacterSlot.Ranged) ? items[Character.CharacterSlot.Ranged] : null,
					items.ContainsKey(Character.CharacterSlot.Projectile) ? items[Character.CharacterSlot.Projectile] : null,
					null,
					items.ContainsKey(Character.CharacterSlot.ExtraWristSocket) ? items[Character.CharacterSlot.ExtraWristSocket] : null,
					items.ContainsKey(Character.CharacterSlot.ExtraHandsSocket) ? items[Character.CharacterSlot.ExtraHandsSocket] : null,
					items.ContainsKey(Character.CharacterSlot.ExtraWaistSocket) ? items[Character.CharacterSlot.ExtraWaistSocket] : null,
					enchants.ContainsKey(Character.CharacterSlot.Head) ? enchants[Character.CharacterSlot.Head] : 0,
					enchants.ContainsKey(Character.CharacterSlot.Shoulders) ? enchants[Character.CharacterSlot.Shoulders] : 0,
					enchants.ContainsKey(Character.CharacterSlot.Back) ? enchants[Character.CharacterSlot.Back] : 0,
					enchants.ContainsKey(Character.CharacterSlot.Chest) ? enchants[Character.CharacterSlot.Chest] : 0,
					enchants.ContainsKey(Character.CharacterSlot.Wrist) ? enchants[Character.CharacterSlot.Wrist] : 0,
					enchants.ContainsKey(Character.CharacterSlot.Hands) ? enchants[Character.CharacterSlot.Hands] : 0,
					enchants.ContainsKey(Character.CharacterSlot.Legs) ? enchants[Character.CharacterSlot.Legs] : 0,
					enchants.ContainsKey(Character.CharacterSlot.Feet) ? enchants[Character.CharacterSlot.Feet] : 0,
					enchants.ContainsKey(Character.CharacterSlot.Finger1) ? enchants[Character.CharacterSlot.Finger1] : 0,
					enchants.ContainsKey(Character.CharacterSlot.Finger2) ? enchants[Character.CharacterSlot.Finger2] : 0,
					enchants.ContainsKey(Character.CharacterSlot.MainHand) ? enchants[Character.CharacterSlot.MainHand] : 0,
					enchants.ContainsKey(Character.CharacterSlot.OffHand) ? enchants[Character.CharacterSlot.OffHand] : 0,
					enchants.ContainsKey(Character.CharacterSlot.Ranged) ? enchants[Character.CharacterSlot.Ranged] : 0
					);
                
                character.Class = charClass;

				string talentCode = wrw.DownloadCharacterTalentTree(character.Name, character.Region, character.Realm)
					.SelectSingleNode("page/characterInfo/talentTab/talentTree").Attributes["value"].Value;
				switch (charClass)
				{
					case Character.CharacterClass.Warrior:
						character.WarriorTalents = new WarriorTalents(talentCode);
						break;
					case Character.CharacterClass.Paladin:
						character.PaladinTalents = new PaladinTalents(talentCode);
						break;
					case Character.CharacterClass.Hunter:
						character.HunterTalents = new HunterTalents(talentCode);
						break;
					case Character.CharacterClass.Rogue:
						character.RogueTalents = new RogueTalents(talentCode);
						break;
					case Character.CharacterClass.Priest:
						character.PriestTalents = new PriestTalents(talentCode);
						break;
					case Character.CharacterClass.Shaman:
						character.ShamanTalents = new ShamanTalents(talentCode);
						break;
					case Character.CharacterClass.Mage:
						character.MageTalents = new MageTalents(talentCode);
						break;
					case Character.CharacterClass.Warlock:
						character.WarlockTalents = new WarlockTalents(talentCode);
						break;
					case Character.CharacterClass.Druid:
						character.DruidTalents = new DruidTalents(talentCode);
						break;
					case Character.CharacterClass.DeathKnight:
						character.DeathKnightTalents = new DeathKnightTalents(talentCode);
						break;
					default:
						break;
				}

                //I will tell you how he lived.
				return character;
			}
			catch (Exception ex)
			{
                StatusMessaging.ReportError("Get Character", ex, "Rawr encountered an error retrieving the Character - " + name + "@" + region.ToString() + "-" + realm + " from the armory");
                //if (docCharacter == null || docCharacter.InnerXml.Length == 0)
                //{
                //    System.Windows.Forms.MessageBox.Show(string.Format("Rawr encountered an error getting Character " +
                //    "from Armory: {0}@{1}-{2}. Please check to make sure you've spelled the character name and realm" +
                //    " exactly right, and chosen the correct Region. Rawr recieved no response to its query for character" +
                //    " data, so if the character name/region/realm are correct, please check to make sure that no firewall " +
                //    "or proxy software is blocking Rawr. If you still encounter this error, please copy and" +
                //    " paste this into an e-mail to cnervig@hotmail.com. Thanks!\r\n\r\nResponse: {3}\r\n\r\n\r\n{4}\r\n\r\n{5}",
                //    name, region.ToString(), realm, "null", ex.Message, ex.StackTrace));
                //}
                //else
                //{
                //    System.Windows.Forms.MessageBox.Show(string.Format("Rawr encountered an error getting Character " +
                //    "from Armory: {0}@{1}-{2}. Please check to make sure you've spelled the character name and realm" + 
                //    " exactly right, and chosen the correct Region. If you still encounter this error, please copy and" +
                //    " paste this into an e-mail to cnervig@hotmail.com. Thanks!\r\n\r\nResponse: {3}\r\n\r\n\r\n{4}\r\n\r\n{5}",
                //    name, region.ToString(), realm, docCharacter.OuterXml, ex.Message, ex.StackTrace));
                //}
                itemsOnCharacter = null;
				return null;
			}
		}

        public static Int32 GetItemIdByName(string item_name)
        {
            try
            {
                WebRequestWrapper wrw = new WebRequestWrapper();
                XmlDocument docItem = wrw.DownloadItemSearch(item_name);
                if (docItem != null)
                {
                    XmlNodeList items_nodes = docItem.SelectNodes("/page/armorySearch/searchResults/items/item");
                    // we only want a single match, even if its not exact
                    if (items_nodes.Count == 1)
                    {
                        Int32 id = Int32.Parse(items_nodes[0].Attributes["id"].InnerText);
                        return id;
                    }
                }
            }
            catch (Exception ex)
            {
                StatusMessaging.ReportError("Get Item", ex, "Rawr encountered an error searching the Armory for item: " + item_name);
            }

            return -1;
        }

		public static Item GetItem(string gemmedId, string logReason)
		{
			//Just close your eyes
			XmlDocument docItem = null;
			try
			{
				string id = gemmedId.Split('.')[0];
				WebRequestWrapper wrw = new WebRequestWrapper();
                docItem = wrw.DownloadItemToolTipSheet(id);
                XmlDocument docItemInfo = wrw.DownloadItemInformation(int.Parse(id));
                ItemLocation location = LocationFactory.Create(docItem, id);
                if (docItem == null || docItem.SelectSingleNode("/page/itemTooltips/itemTooltip[1]") == null)
                {
                    StatusMessaging.ReportError("Get Item", null, "No item returned from Armory for " + id);
                    return null;
                }
				Item.ItemQuality quality = Item.ItemQuality.Common;
				Item.ItemType type = Item.ItemType.None;
				string name = string.Empty;
				string iconPath = string.Empty;
				string setName = string.Empty;
				Item.ItemSlot slot = Item.ItemSlot.None;
				Stats stats = new Stats();
				Sockets sockets = new Sockets();
				int inventoryType = -1;
				int classId = -1;
				string subclassName = string.Empty;
				int minDamage = 0;
				int maxDamage = 0;
                Item.ItemDamageType damageType = Item.ItemDamageType.Physical;
				float speed = 0f;
				List<string> requiredClasses = new List<string>();
                bool unique = false;
                int itemLevel = 0;
						
				foreach (XmlNode node in docItem.SelectNodes("page/itemTooltips/itemTooltip/name")) { name = node.InnerText; }
				foreach (XmlNode node in docItem.SelectNodes("page/itemTooltips/itemTooltip/icon")) { iconPath = node.InnerText; }
				foreach (XmlNode node in docItem.SelectNodes("page/itemTooltips/itemTooltip/maxCount")) { unique = node.InnerText == "1"; }
				foreach (XmlNode node in docItem.SelectNodes("page/itemTooltips/itemTooltip/overallQualityId")) { quality = (Item.ItemQuality)Enum.Parse(typeof(Item.ItemQuality), node.InnerText); }
				foreach (XmlNode node in docItem.SelectNodes("page/itemTooltips/itemTooltip/classId")) { classId = int.Parse(node.InnerText); }
				foreach (XmlNode node in docItem.SelectNodes("page/itemTooltips/itemTooltip/equipData/inventoryType")) { inventoryType = int.Parse(node.InnerText); }
				foreach (XmlNode node in docItem.SelectNodes("page/itemTooltips/itemTooltip/equipData/subclassName")) { subclassName = node.InnerText; }
				foreach (XmlNode node in docItem.SelectNodes("page/itemTooltips/itemTooltip/damageData/damage/min")) { minDamage = int.Parse(node.InnerText); }
				foreach (XmlNode node in docItem.SelectNodes("page/itemTooltips/itemTooltip/damageData/damage/max")) { maxDamage = int.Parse(node.InnerText); }
                foreach (XmlNode node in docItem.SelectNodes("page/itemTooltips/itemTooltip/damageData/damage/type")) { damageType = (Item.ItemDamageType)Enum.Parse(typeof(Item.ItemDamageType), node.InnerText); }
                foreach (XmlNode node in docItem.SelectNodes("page/itemTooltips/itemTooltip/damageData/speed")) { speed = float.Parse(node.InnerText, System.Globalization.CultureInfo.InvariantCulture); }
				foreach (XmlNode node in docItem.SelectNodes("page/itemTooltips/itemTooltip/setData/name")) { setName = node.InnerText; }
				foreach (XmlNode node in docItem.SelectNodes("page/itemTooltips/itemTooltip/allowableClasses/class")) { requiredClasses.Add(node.InnerText); }

                foreach (XmlNode node in docItemInfo.SelectNodes("page/itemInfo/item/@level")) { itemLevel = int.Parse(node.InnerText); }

				if (inventoryType >= 0)
					slot = GetItemSlot(inventoryType, classId);
				if (!string.IsNullOrEmpty(subclassName))
					type = GetItemType(subclassName, inventoryType, classId);

				/* fix class restrictions on BOP items that can only be made by certain classes */
				switch (id)
				{
					case "35181":
					case "32495":
						requiredClasses.Add("Priest");
						break;
					case "32476":
					case "35184":
					case "32475":
					case "34355":
						requiredClasses.Add("Shaman");
						break;
					case "32474":
					case "34356":
						requiredClasses.Add("Hunter");
						break;
					case "46106":
					case "32479":
					case "32480":
					case "46109":
						requiredClasses.Add("Druid");
						break;
					case "32478":
					case "34353":
						requiredClasses.Add("Druid");
						requiredClasses.Add("Rogue");
						break;
				}

				foreach (XmlNode node in docItem.SelectNodes("page/itemTooltips/itemTooltip/bonusAgility")) { stats.Agility = int.Parse(node.InnerText); }
				foreach (XmlNode node in docItem.SelectNodes("page/itemTooltips/itemTooltip/bonusAttackPower")) { stats.AttackPower = int.Parse(node.InnerText); }
				foreach (XmlNode node in docItem.SelectNodes("page/itemTooltips/itemTooltip/armor")) { stats.Armor = int.Parse(node.InnerText); }
				foreach (XmlNode node in docItem.SelectNodes("page/itemTooltips/itemTooltip/bonusDefenseSkillRating")) { stats.DefenseRating = int.Parse(node.InnerText); }
				foreach (XmlNode node in docItem.SelectNodes("page/itemTooltips/itemTooltip/bonusDodgeRating")) { stats.DodgeRating = int.Parse(node.InnerText); }
                foreach (XmlNode node in docItem.SelectNodes("page/itemTooltips/itemTooltip/bonusParryRating")) { stats.ParryRating = int.Parse(node.InnerText); }
                foreach (XmlNode node in docItem.SelectNodes("page/itemTooltips/itemTooltip/bonusBlockRating")) { stats.BlockRating = int.Parse(node.InnerText); }
                foreach (XmlNode node in docItem.SelectNodes("page/itemTooltips/itemTooltip/bonusBlockValue")) { stats.BlockValue = int.Parse(node.InnerText); }
                foreach (XmlNode node in docItem.SelectNodes("page/itemTooltips/itemTooltip/blockValue")) { stats.BlockValue = int.Parse(node.InnerText); }
				foreach (XmlNode node in docItem.SelectNodes("page/itemTooltips/itemTooltip/bonusResilienceRating")) { stats.Resilience = int.Parse(node.InnerText); }
				foreach (XmlNode node in docItem.SelectNodes("page/itemTooltips/itemTooltip/bonusStamina")) { stats.Stamina = int.Parse(node.InnerText); }
                foreach (XmlNode node in docItem.SelectNodes("page/itemTooltips/itemTooltip/bonusIntellect")) { stats.Intellect = int.Parse(node.InnerText); }

				foreach (XmlNode node in docItem.SelectNodes("page/itemTooltips/itemTooltip/bonusStrength")) { stats.Strength = int.Parse(node.InnerText); }
				foreach (XmlNode node in docItem.SelectNodes("page/itemTooltips/itemTooltip/bonusHitRating")) { stats.HitRating = int.Parse(node.InnerText); }
				foreach (XmlNode node in docItem.SelectNodes("page/itemTooltips/itemTooltip/bonusHasteRating")) { stats.HasteRating = int.Parse(node.InnerText); }
				foreach (XmlNode node in docItem.SelectNodes("page/itemTooltips/itemTooltip/bonusCritRating")) { stats.CritRating = int.Parse(node.InnerText); }
				foreach (XmlNode node in docItem.SelectNodes("page/itemTooltips/itemTooltip/bonusExpertiseRating")) { stats.ExpertiseRating = int.Parse(node.InnerText); }
                foreach (XmlNode node in docItem.SelectNodes("page/itemTooltips/itemTooltip/bonusCritMeleeRating")) { stats.CritMeleeRating = int.Parse(node.InnerText); }

                foreach (XmlNode node in docItem.SelectNodes("page/itemTooltips/itemTooltip/arcaneResist")) { stats.ArcaneResistance = int.Parse(node.InnerText); }
				foreach (XmlNode node in docItem.SelectNodes("page/itemTooltips/itemTooltip/fireResist")) { stats.FireResistance = int.Parse(node.InnerText); }
				foreach (XmlNode node in docItem.SelectNodes("page/itemTooltips/itemTooltip/frostResist")) { stats.FrostResistance = int.Parse(node.InnerText); }
				foreach (XmlNode node in docItem.SelectNodes("page/itemTooltips/itemTooltip/natureResist")) { stats.NatureResistance = int.Parse(node.InnerText); }
				foreach (XmlNode node in docItem.SelectNodes("page/itemTooltips/itemTooltip/shadowResist")) { stats.ShadowResistance = int.Parse(node.InnerText); }

                foreach (XmlNode node in docItem.SelectNodes("page/itemTooltips/itemTooltip/bonusCritSpellRating")) { stats.CritRating = int.Parse(node.InnerText); }
                foreach (XmlNode node in docItem.SelectNodes("page/itemTooltips/itemTooltip/bonusHitSpellRating")) { stats.HitRating = int.Parse(node.InnerText); }
                foreach (XmlNode node in docItem.SelectNodes("page/itemTooltips/itemTooltip/bonusHasteSpellRating")) { stats.HasteRating = int.Parse(node.InnerText); }
                foreach (XmlNode node in docItem.SelectNodes("page/itemTooltips/itemTooltip/bonusSpellPower")) { stats.SpellPower = int.Parse(node.InnerText); }

                foreach (XmlNode node in docItem.SelectNodes("page/itemTooltips/itemTooltip/bonusMana")) { stats.Mana = int.Parse(node.InnerText); }
                foreach (XmlNode node in docItem.SelectNodes("page/itemTooltips/itemTooltip/bonusSpirit")) { stats.Spirit = int.Parse(node.InnerText); }
				foreach (XmlNode node in docItem.SelectNodes("page/itemTooltips/itemTooltip/bonusManaRegen")) { stats.Mp5 = int.Parse(node.InnerText); }

				if (slot == Item.ItemSlot.Finger ||
					slot == Item.ItemSlot.MainHand ||
					slot == Item.ItemSlot.Neck ||
					(slot == Item.ItemSlot.OffHand && type != Item.ItemType.Shield) ||
					slot == Item.ItemSlot.OneHand ||
					slot == Item.ItemSlot.Trinket ||
					slot == Item.ItemSlot.TwoHand)
				{
					stats.BonusArmor += stats.Armor;
					stats.Armor = 0f;
				}

				if (slot == Item.ItemSlot.Back)
				{
					float baseArmor = 0;
					switch (quality)
					{     
						case Item.ItemQuality.Temp:
						case Item.ItemQuality.Poor:
						case Item.ItemQuality.Common:
						case Item.ItemQuality.Uncommon:
							baseArmor = (float)itemLevel * 1.19f + 5.1f;
							break;

						case Item.ItemQuality.Rare:
							baseArmor = ((float)itemLevel + 26.6f) * 16f / 25f;
							break;

						case Item.ItemQuality.Epic:
						case Item.ItemQuality.Legendary:
						case Item.ItemQuality.Artifact:
						case Item.ItemQuality.Heirloom:
							baseArmor = ((float)itemLevel + 358f) * 7f / 26f;
							break;
					}
					baseArmor *= 0.48f;
					baseArmor = (float)Math.Floor(baseArmor);
					stats.BonusArmor = stats.Armor - baseArmor;
					stats.Armor = baseArmor;
				}

                if (name.StartsWith("Ashtongue Talisman"))
				{
					stats.AshtongueTrinketProc = 1;
				}
				
				foreach (XmlNode node in docItem.SelectNodes("page/itemTooltips/itemTooltip/spellData/spell"))
				{
					bool isEquip = false;
					bool isUse = false;
					string spellDesc = null;
					foreach (XmlNode childNode in node.ChildNodes)
					{
						if (childNode.Name == "trigger")
						{
							isEquip = childNode.InnerText == "1";
							isUse = childNode.InnerText == "0";
						}
						if (childNode.Name == "desc")
							spellDesc = childNode.InnerText;
					}

					//parse Use/Equip lines
					if (isUse) SpecialEffects.ProcessUseLine(spellDesc, stats, true, int.Parse(id));
					if (isEquip) SpecialEffects.ProcessEquipLine(spellDesc, stats, true);
				}

				XmlNodeList socketNodes = docItem.SelectNodes("page/itemTooltips/itemTooltip/socketData/socket");
				if (socketNodes.Count > 0) sockets.Color1String = socketNodes[0].Attributes["color"].Value;
				if (socketNodes.Count > 1) sockets.Color2String = socketNodes[1].Attributes["color"].Value;
				if (socketNodes.Count > 2) sockets.Color3String = socketNodes[2].Attributes["color"].Value;
				string socketBonusesString = string.Empty;
				foreach (XmlNode node in docItem.SelectNodes("page/itemTooltips/itemTooltip/socketData/socketMatchEnchant")) { socketBonusesString = node.InnerText.Trim('+'); }
				if (!string.IsNullOrEmpty(socketBonusesString))
				{
					try
					{
						List<string> socketBonuses = new List<string>();
						string[] socketBonusStrings = socketBonusesString.Split(new string[] { " and ", " & ", ", " }, StringSplitOptions.None);
						foreach (string socketBonusString in socketBonusStrings)
						{
							if (socketBonusString.LastIndexOf('+') > 2 && socketBonusString.LastIndexOf('+') < socketBonusString.Length - 3)
							{
								socketBonuses.Add(socketBonusString.Substring(0, socketBonusString.IndexOf(" +")));
								socketBonuses.Add(socketBonusString.Substring(socketBonusString.IndexOf(" +") + 1));
							}
							else
								socketBonuses.Add(socketBonusString);
						}
						foreach (string socketBonus in socketBonuses)
						{
							int socketBonusValue = 0;
							if (socketBonus.IndexOf(' ') > 0) socketBonusValue = int.Parse(socketBonus.Substring(0, socketBonus.IndexOf(' ')));
							switch (socketBonus.Substring(socketBonus.IndexOf(' ') + 1))
							{
								case "Agility":
									sockets.Stats.Agility = socketBonusValue;
									break;
								case "Stamina":
									sockets.Stats.Stamina = socketBonusValue;
									break;
								case "Dodge Rating":
									sockets.Stats.DodgeRating = socketBonusValue;
									break;
								case "Parry Rating":
									sockets.Stats.ParryRating = socketBonusValue;
									break;
								case "Block Rating":
									sockets.Stats.BlockRating = socketBonusValue;
									break;
								case "Block Value":
									sockets.Stats.BlockValue = socketBonusValue;
									break;
								case "Defense Rating":
									sockets.Stats.DefenseRating = socketBonusValue;
									break;
								case "Hit Rating":
									sockets.Stats.HitRating = socketBonusValue;
									break;
								case "Haste Rating":
									sockets.Stats.HasteRating = socketBonusValue;
									break;
								case "Expertise Rating":
									sockets.Stats.ExpertiseRating = socketBonusValue;
									break;
								case "Armor Penetration Rating":
									sockets.Stats.ArmorPenetrationRating = socketBonusValue;
									break;
								case "Strength":
									sockets.Stats.Strength = socketBonusValue;
									break;
								case "Healing":
								//case "Healing +4 Spell Damage":
								//case "Healing +3 Spell Damage":
								//case "Healing +2 Spell Damage":
								//case "Healing +1 Spell Damage":
								//case "Healing and +4 Spell Damage":
								//case "Healing and +3 Spell Damage":
								//case "Healing and +2 Spell Damage":
								//case "Healing and +1 Spell Damage":
									if (socketBonusValue == 0)
										sockets.Stats.SpellPower = (float)Math.Round(int.Parse(socketBonuses[0].Substring(0, socketBonuses[0].IndexOf(' '))) / 1.88f);
									else
										sockets.Stats.SpellPower = (float)Math.Round(socketBonusValue / 1.88f);
									break;
                                case "Spell Damage":
                                    // Only update Spell Damage if its not already set (Incase its an old heal bonus)
                                    if (sockets.Stats.SpellPower == 0)
                                        sockets.Stats.SpellPower = socketBonusValue;
                                    //sockets.Stats.Healing = socketBonusValue;
                                    break;
                                case "Spell Power":
                                    sockets.Stats.SpellPower = socketBonusValue;
                                    break;
                                case "Crit Rating":
								case "Crit Strike Rating":
								case "Critical Rating":
								case "Critical Strike Rating":
									sockets.Stats.CritRating = socketBonusValue;
									break;
								case "Attack Power":
									sockets.Stats.AttackPower = socketBonusValue;
									break;
								case "Weapon Damage":
									sockets.Stats.WeaponDamage = socketBonusValue;
									break;
								case "Resilience":
								case "Resilience Rating":
									sockets.Stats.Resilience = socketBonusValue;
									break;
								//case "Spell Damage and Healing":
								//    sockets.Stats.SpellDamageRating = socketBonusValue;
								//    sockets.Stats.Healing = socketBonusValue;
								//    break;
								case "Spell Hit Rating":
									sockets.Stats.HitRating = socketBonusValue;
									break;
								case "Intellect":
									sockets.Stats.Intellect = socketBonusValue;
									break;
								case "Spell Crit":
								case "Spell Crit Rating":
								case "Spell Critical":
								case "Spell Critical Rating":
								case "Spell Critical Strike Rating":
									sockets.Stats.CritRating = socketBonusValue;
									break;
								case "Spell Haste Rating":
									sockets.Stats.HasteRating = socketBonusValue;
									break;
								case "Spirit":
									sockets.Stats.Spirit = socketBonusValue;
									break;
								case "Mana every 5 seconds":
								case "Mana ever 5 Sec":
								case "mana per 5 sec":
								case "mana per 5 sec.":
								case "Mana per 5 sec.":
								case "Mana per 5 Seconds":
									sockets.Stats.Mp5 = socketBonusValue;
									break;
							}
						}
					}
					catch { }
				}
				foreach (XmlNode nodeGemProperties in docItem.SelectNodes("page/itemTooltips/itemTooltip/gemProperties"))
				{
					List<string> gemBonuses = new List<string>();
					string[] gemBonusStrings = nodeGemProperties.InnerText.Split(new string[] { " and ", " & ", ", " }, StringSplitOptions.None);
					foreach (string gemBonusString in gemBonusStrings)
					{
						if (gemBonusString.IndexOf('+') != gemBonusString.LastIndexOf('+'))
						{
							gemBonuses.Add(gemBonusString.Substring(0, gemBonusString.IndexOf(" +")));
							gemBonuses.Add(gemBonusString.Substring(gemBonusString.IndexOf(" +") + 1));
						}
						else
							gemBonuses.Add(gemBonusString);
					}
					foreach (string gemBonus in gemBonuses)
					{
                        if (gemBonus == "Spell Damage +6")
                        {
                            stats.SpellPower = 6.0f;
                        }
						else if (gemBonus == "2% Increased Armor Value from Items")
						{
							stats.BonusArmorMultiplier = 0.02f;
						}
                        else if (gemBonus == "Stamina +6")
                        {
                            stats.Stamina = 6.0f;
                        }
                        else if (gemBonus == "Chance to restore mana on spellcast")
                        {
                            stats.ManaRestoreOnCast_5_15 = 300; // IED
                        }
                        else if (gemBonus == "Chance on spellcast - next spell cast in half time" || gemBonus == "Chance to Increase Spell Cast Speed")
                        {
                            stats.SpellHasteFor6SecOnCast_15_45 = 320; // MSD changed in 2.4
                        }
                        else if (gemBonus == "+10% Shield Block Value")
                        {
                            stats.BonusBlockValueMultiplier = 0.1f;
                        }
                        else if (gemBonus == "+2% Intellect")
                        {
                            stats.BonusIntellectMultiplier = 0.02f;
                        }
                        else if (gemBonus == "2% Reduced Threat")
                        {
                            stats.ThreatReductionMultiplier = 0.02f;
                        }
                        else if (gemBonus == "3% Increased Critical Healing Effect")
                        {
                            stats.BonusCritHealMultiplier = 0.03f;
                        }
                        else
                        {
                            try
                            {
                                int gemBonusValue = int.Parse(gemBonus.Substring(0, gemBonus.IndexOf(' ')).Trim('+').Trim('%'));
                                switch (gemBonus.Substring(gemBonus.IndexOf(' ') + 1).Trim())
                                {
									case "to All Stats":
									case "All Stats":
										stats.Agility = gemBonusValue;
										stats.Strength = gemBonusValue;
										stats.Stamina = gemBonusValue;
										stats.Intellect = gemBonusValue;
										stats.Spirit = gemBonusValue;
										break;
                                    case "Resist All":
                                        stats.ArcaneResistance = gemBonusValue;
                                        stats.FireResistance = gemBonusValue;
                                        stats.FrostResistance = gemBonusValue;
                                        stats.NatureResistance = gemBonusValue;
                                        stats.ShadowResistance = gemBonusValue;
                                        break;
                                    case "Increased Critical Damage":
                                        stats.BonusCritMultiplier = (float)gemBonusValue / 100f;
                                        stats.BonusSpellCritMultiplier = (float)gemBonusValue / 100f; // both melee and spell crit use the same text, would have to disambiguate based on other stats
                                        break;
                                    case "Agility":
                                        stats.Agility = gemBonusValue;
                                        break;
                                    case "Stamina":
                                        stats.Stamina = gemBonusValue;
                                        break;
                                    case "Dodge Rating":
                                        stats.DodgeRating = gemBonusValue;
                                        break;
                                    case "Parry Rating":
                                        stats.ParryRating = gemBonusValue;
                                        break;
                                    case "Block Rating":
                                        stats.BlockRating = gemBonusValue;
                                        break;
                                    case "Defense Rating":
                                        stats.DefenseRating = gemBonusValue;
                                        break;
                                    case "Hit Rating":
                                        stats.HitRating = gemBonusValue;
                                        break;
                                    case "Haste Rating":
                                        stats.HasteRating = gemBonusValue;
                                        break;
                                    case "Expertise Rating":
                                        stats.ExpertiseRating = gemBonusValue;
                                        break;
                                    case "Armor Penetration Rating":
                                        stats.ArmorPenetrationRating = gemBonusValue;
                                        break;
                                    case "Strength":
                                        stats.Strength = gemBonusValue;
                                        break;
                                    case "Crit Rating":
                                    case "Crit Strike Rating":
                                    case "Critical Rating":
                                    case "Critical Strike Rating":
                                        stats.CritRating = gemBonusValue;
                                        break;
                                    case "Attack Power":
                                        stats.AttackPower = gemBonusValue;
                                        break;
                                    case "Weapon Damage":
                                        stats.WeaponDamage = gemBonusValue;
                                        break;
                                    case "Resilience":
                                    case "Resilience Rating":
                                        stats.Resilience = gemBonusValue;
                                        break;
                                    case "Spell Hit Rating":
                                        stats.HitRating = gemBonusValue;
                                        break;
                                    case "Spell Haste Rating":
                                        stats.HasteRating = gemBonusValue;
                                        break;
                                    case "Spell Damage":
                                        // Ignore spell damage from gem if Healing has already been applied, as it might be a "9 Healing 3 Spell" gem. 
                                        if (stats.SpellPower == 0)
                                            stats.SpellPower = gemBonusValue;
                                        break;
                                    case "Spell Damage and Healing":
                                        stats.SpellPower = gemBonusValue;
                                        break;
                                    case "Healing":
                                        stats.SpellPower = (float)Math.Round(gemBonusValue / 1.88f);
                                        break;
                                    case "Spell Power":
                                        stats.SpellPower = gemBonusValue;
                                        break;
                                    case "Spell Crit":
                                    case "Spell Crit Rating":
                                    case "Spell Critical":
                                    case "Spell Critical Rating":
                                        stats.CritRating = gemBonusValue;
                                        break;
                                    case "Mana every 5 seconds":
                                    case "Mana ever 5 Sec":
                                    case "mana per 5 sec":
                                    case "mana per 5 sec.":
                                    case "Mana per 5 Seconds":
                                        stats.Mp5 = gemBonusValue;
                                        break;
                                    case "Intellect":
                                        stats.Intellect = gemBonusValue;
                                        break;
                                    case "Spirit":
                                        stats.Spirit = gemBonusValue;
                                        break;
                                }
                            }
                            catch { }
                        }
					}
				}
				string desc = string.Empty;
				foreach (XmlNode node in docItem.SelectNodes("page/itemTooltips/itemTooltip/desc")) { desc = node.InnerText; }
                if (desc.Contains("Matches any Socket"))
                {
                    slot = Item.ItemSlot.Prismatic;
                }
                else if (desc.Contains("Matches a "))
				{
					bool red = desc.Contains("Red");
					bool blue = desc.Contains("Blue");
					bool yellow = desc.Contains("Yellow");
					slot = red && blue && yellow ? Item.ItemSlot.Prismatic :
						red && blue ? Item.ItemSlot.Purple :
						blue && yellow ? Item.ItemSlot.Green :
						red && yellow ? Item.ItemSlot.Orange :
						red ? Item.ItemSlot.Red :
						blue ? Item.ItemSlot.Blue :
						yellow ? Item.ItemSlot.Yellow :
						Item.ItemSlot.None;
				}
				else if (desc.Contains("meta gem slot"))
					slot = Item.ItemSlot.Meta;

				string[] ids = gemmedId.Split('.');
				int gem1Id = ids.Length == 4 ? int.Parse(ids[1]) : 0;
				int gem2Id = ids.Length == 4 ? int.Parse(ids[2]) : 0;
				int gem3Id = ids.Length == 4 ? int.Parse(ids[3]) : 0;
                Item item = new Item()
                {
					_id = int.Parse(id),
                    _gem1Id = gem1Id,
                    _gem2Id = gem2Id,
                    _gem3Id = gem3Id, //Set the local fields so as not to call OnIdsChanging/Changed, so it's not added to the ItemCache yet.
                    Name = name,
                    Quality = quality,
                    Type = type,
                    IconPath = iconPath,
                    Slot = slot,
                    SetName = setName,
                    Stats = stats,
                    Sockets = sockets,
                    MinDamage = minDamage,
                    MaxDamage = maxDamage,
                    DamageType = damageType,
                    Speed = speed,
                    RequiredClasses = string.Join("|", requiredClasses.ToArray()),
                    Unique = unique,
                    ItemLevel = itemLevel,
                };

				item.Stats.ConvertStatsToWotLKEquivalents();

                return item;
			}
			catch (Exception ex)
			{
                //This condition is now accounted for elsewhere since this function is usually called in a loop and would display
                //the armory not accessable error many many times.
                //if (docItem == null || docItem.InnerXml.Length == 0)
                //{
                //    System.Windows.Forms.MessageBox.Show(string.Format("Rawr encountered an error getting Item " +
                //    "from Armory: {0}. Rawr recieved no response to its query for item" +
                //    " data, so please check to make sure that no firewall " +
                //    "or proxy software is blocking Rawr. If you still encounter this error, please copy and" +
                //    " paste this into an e-mail to cnervig@hotmail.com. Thanks!\r\n\r\nResponse: {1}\r\n\r\n\r\n{2}\r\n\r\n{3}",
                //    gemmedId, "null", ex.Message, ex.StackTrace));
                //}
                //else
                //{
                StatusMessaging.ReportError("Get Item", ex, "Rawr encountered an error getting Item from Armory: " + gemmedId);
                    //System.Windows.Forms.MessageBox.Show(string.Format("Rawr encountered an error getting Item " +
                    //"from Armory: {0}. If you still encounter this error, please copy and" +
                    //" paste this into an e-mail to cnervig@hotmail.com. Thanks!\r\n\r\nResponse: {1}\r\n\r\n\r\n{2}\r\n\r\n{3}",
                    //gemmedId, docItem.OuterXml, ex.Message, ex.StackTrace));
				//}
				return null;
			}
		}

		private static Item.ItemType GetItemType(string subclassName, int inventoryType, int classId)
		{
			switch (subclassName)
			{
				case "Cloth":
					return Item.ItemType.Cloth;

				case "Leather":
					return Item.ItemType.Leather;

				case "Mail":
					return Item.ItemType.Mail;

				case "Plate":
					return Item.ItemType.Plate;

				case "Dagger":
					return Item.ItemType.Dagger;

				case "Fist Weapon":
					return Item.ItemType.FistWeapon;

				case "Axe":
					if (inventoryType == 17)
						return Item.ItemType.TwoHandAxe;
					else
						return Item.ItemType.OneHandAxe;

				case "Mace":
					if (inventoryType == 17)
						return Item.ItemType.TwoHandMace;
					else
						return Item.ItemType.OneHandMace;

				case "Sword":
					if (inventoryType == 17)
						return Item.ItemType.TwoHandSword;
					else
						return Item.ItemType.OneHandSword;

				case "Polearm":
					return Item.ItemType.Polearm;

				case "Staff":
					return Item.ItemType.Staff;

				case "Shield":
					return Item.ItemType.Shield;

				case "Bow":
					return Item.ItemType.Bow;

				case "Crossbow":
					return Item.ItemType.Crossbow;

				case "Gun":
					return Item.ItemType.Gun;

				case "Wand":
					return Item.ItemType.Wand;

				case "Thrown":
					return Item.ItemType.Thrown;

				case "Idol":
					return Item.ItemType.Idol;

				case "Libram":
					return Item.ItemType.Libram;

				case "Totem":
					return Item.ItemType.Totem;

				case "Arrow":
					return Item.ItemType.Arrow;

				case "Bullet":
					return Item.ItemType.Bullet;

				case "Quiver":
					return Item.ItemType.Quiver;

				case "Ammo Pouch":
					return Item.ItemType.AmmoPouch;

				default:
					return Item.ItemType.None;
			}
		}

		private static Item.ItemSlot GetItemSlot(int inventoryType, int classId)
		{
			switch (classId)
			{
				case 6:
					return Item.ItemSlot.Projectile;

				case 11:
					return Item.ItemSlot.ProjectileBag;
			}
					
			switch (inventoryType)
			{
				case 1:
					return Item.ItemSlot.Head;

				case 2:
					return Item.ItemSlot.Neck;

				case 3:
					return Item.ItemSlot.Shoulders;

				case 16:
					return Item.ItemSlot.Back;

				case 5:
				case 20:
					return Item.ItemSlot.Chest;

				case 4:
					return Item.ItemSlot.Shirt;

				case 19:
					return Item.ItemSlot.Tabard;

				case 9:
					return Item.ItemSlot.Wrist;

				case 10:
					return Item.ItemSlot.Hands;

				case 6:
					return Item.ItemSlot.Waist;

				case 7:
					return Item.ItemSlot.Legs;

				case 8:
					return Item.ItemSlot.Feet;

				case 11:
					return Item.ItemSlot.Finger;

				case 12:
					return Item.ItemSlot.Trinket;

				case 13:
					return Item.ItemSlot.OneHand;

				case 17:
					return Item.ItemSlot.TwoHand;

				case 21:
					return Item.ItemSlot.MainHand;

				case 14:
				case 22:
				case 23:
					return Item.ItemSlot.OffHand;

				case 15:
				case 25:
				case 26:
				case 28:
					return Item.ItemSlot.Ranged;

				case 24:
					return Item.ItemSlot.Projectile;

				case 27:
					return Item.ItemSlot.ProjectileBag;
				
				default:
					return Item.ItemSlot.None;
			}
		}

		public static void LoadUpgradesFromArmory(Character character)
		{
			if (!string.IsNullOrEmpty(character.Realm) && !string.IsNullOrEmpty(character.Name))
			{
				WebRequestWrapper.ResetFatalErrorIndicator();
				List<ComparisonCalculationBase> gemCalculations = new List<ComparisonCalculationBase>();
				foreach (Item item in ItemCache.AllItems)
				{
					if (item.Slot == Item.ItemSlot.Blue || item.Slot == Item.ItemSlot.Green || item.Slot == Item.ItemSlot.Meta
						 || item.Slot == Item.ItemSlot.Orange || item.Slot == Item.ItemSlot.Prismatic || item.Slot == Item.ItemSlot.Purple
						 || item.Slot == Item.ItemSlot.Red || item.Slot == Item.ItemSlot.Yellow)
					{
						gemCalculations.Add(Calculations.GetItemCalculations(item, character, item.Slot == Item.ItemSlot.Meta ? Character.CharacterSlot.Metas : Character.CharacterSlot.Gems));
					}
				}

				ComparisonCalculationBase idealRed = null, idealBlue = null, idealYellow = null, idealMeta = null;
				foreach (ComparisonCalculationBase calc in gemCalculations)
				{
					if (Item.GemMatchesSlot(calc.Item, Item.ItemSlot.Meta) && (idealMeta == null || idealMeta.OverallPoints < calc.OverallPoints))
						idealMeta = calc;
					if (Item.GemMatchesSlot(calc.Item, Item.ItemSlot.Red) && (idealRed == null || idealRed.OverallPoints < calc.OverallPoints))
						idealRed = calc;
					if (Item.GemMatchesSlot(calc.Item, Item.ItemSlot.Blue) && (idealBlue == null || idealBlue.OverallPoints < calc.OverallPoints))
						idealBlue = calc;
					if (Item.GemMatchesSlot(calc.Item, Item.ItemSlot.Yellow) && (idealYellow == null || idealYellow.OverallPoints < calc.OverallPoints))
						idealYellow = calc;
				}
				Dictionary<Item.ItemSlot, int> idealGems = new Dictionary<Item.ItemSlot, int>();
				idealGems.Add(Item.ItemSlot.Meta, idealMeta == null ? 0 : idealMeta.Item.Id);
				idealGems.Add(Item.ItemSlot.Red, idealRed == null ? 0 : idealRed.Item.Id);
				idealGems.Add(Item.ItemSlot.Blue, idealBlue == null ? 0 : idealBlue.Item.Id);
				idealGems.Add(Item.ItemSlot.Yellow, idealYellow == null ? 0 : idealYellow.Item.Id);
				idealGems.Add(Item.ItemSlot.None, 0);

				#region status queuing
				StatusMessaging.UpdateStatus(Character.CharacterSlot.Head.ToString(), "Queued");
				StatusMessaging.UpdateStatus(Character.CharacterSlot.Neck.ToString(), "Queued");
				StatusMessaging.UpdateStatus(Character.CharacterSlot.Shoulders.ToString(), "Queued");
				StatusMessaging.UpdateStatus(Character.CharacterSlot.Back.ToString(), "Queued");
				StatusMessaging.UpdateStatus(Character.CharacterSlot.Chest.ToString(), "Queued");
				StatusMessaging.UpdateStatus(Character.CharacterSlot.Wrist.ToString(), "Queued");
				StatusMessaging.UpdateStatus(Character.CharacterSlot.Hands.ToString(), "Queued");
				StatusMessaging.UpdateStatus(Character.CharacterSlot.Waist.ToString(), "Queued");
				StatusMessaging.UpdateStatus(Character.CharacterSlot.Legs.ToString(), "Queued");
				StatusMessaging.UpdateStatus(Character.CharacterSlot.Feet.ToString(), "Queued");
				StatusMessaging.UpdateStatus(Character.CharacterSlot.Finger1.ToString(), "Queued");
				StatusMessaging.UpdateStatus(Character.CharacterSlot.Finger2.ToString(), "Queued");
				StatusMessaging.UpdateStatus(Character.CharacterSlot.Trinket1.ToString(), "Queued");
				StatusMessaging.UpdateStatus(Character.CharacterSlot.Trinket2.ToString(), "Queued");
				StatusMessaging.UpdateStatus(Character.CharacterSlot.MainHand.ToString(), "Queued");
				StatusMessaging.UpdateStatus(Character.CharacterSlot.OffHand.ToString(), "Queued");
				StatusMessaging.UpdateStatus(Character.CharacterSlot.Ranged.ToString(), "Queued");
				#endregion
				
				LoadUpgradesForSlot(character, Character.CharacterSlot.Head, idealGems);
				LoadUpgradesForSlot(character, Character.CharacterSlot.Neck, idealGems);
				LoadUpgradesForSlot(character, Character.CharacterSlot.Shoulders, idealGems);
				LoadUpgradesForSlot(character, Character.CharacterSlot.Back, idealGems);
				LoadUpgradesForSlot(character, Character.CharacterSlot.Chest, idealGems);
				LoadUpgradesForSlot(character, Character.CharacterSlot.Wrist, idealGems);
				LoadUpgradesForSlot(character, Character.CharacterSlot.Hands, idealGems);
				LoadUpgradesForSlot(character, Character.CharacterSlot.Waist, idealGems);
				LoadUpgradesForSlot(character, Character.CharacterSlot.Legs, idealGems);
				LoadUpgradesForSlot(character, Character.CharacterSlot.Feet, idealGems);
				LoadUpgradesForSlot(character, Character.CharacterSlot.Finger1, idealGems);
				LoadUpgradesForSlot(character, Character.CharacterSlot.Finger2, idealGems);
				LoadUpgradesForSlot(character, Character.CharacterSlot.Trinket1, idealGems);
				LoadUpgradesForSlot(character, Character.CharacterSlot.Trinket2, idealGems);
				LoadUpgradesForSlot(character, Character.CharacterSlot.MainHand, idealGems);
				LoadUpgradesForSlot(character, Character.CharacterSlot.OffHand, idealGems);
				LoadUpgradesForSlot(character, Character.CharacterSlot.Ranged, idealGems);
			}
			else
			{
				System.Windows.Forms.MessageBox.Show("This feature requires your character name, realm, and region. Please fill these fields out, and try again.");
			}
		}

		private static void LoadUpgradesForSlot(Character character, Character.CharacterSlot slot, Dictionary<Item.ItemSlot, int> idealGems)
		{
			XmlDocument docUpgradeSearch = null;
			try
			{
				StatusMessaging.UpdateStatus(slot.ToString(), "Downloading Upgrade List");
				Item itemToUpgrade = character[slot];
				if (itemToUpgrade != null)
				{
					WebRequestWrapper wrw = new WebRequestWrapper();
					docUpgradeSearch = wrw.DownloadUpgrades(character.Name, character.Region,character.Realm,itemToUpgrade.Id);

					ComparisonCalculationBase currentCalculation = Calculations.GetItemCalculations(itemToUpgrade, character, slot);
					if (docUpgradeSearch != null)
					{
						XmlNodeList nodeList = docUpgradeSearch.SelectNodes("page/armorySearch/searchResults/items/item");
						for (int i = 0; i < nodeList.Count; i++)
						{
							StatusMessaging.UpdateStatus(slot.ToString(), string.Format("Downloading definition {0} of {1} possible upgrades", i, nodeList.Count));
							string id = nodeList[i].Attributes["id"].Value;
							List<Item> idealItemGemmings = ItemCache.Instance.FindAllItemsById(int.Parse(id)) as List<Item>;
							if (idealItemGemmings.Count > 0)
							{
								id += string.Format(".{0}.{1}.{2}", idealGems[idealItemGemmings[0].Sockets.Color1],
									idealGems[idealItemGemmings[0].Sockets.Color2], idealGems[idealItemGemmings[0].Sockets.Color3]);
								ItemCache.FindItemById(id);
							}
							else
							{
								Item idealItem = GetItem(id, "Loading Upgrades");
								if (idealItem != null)
								{
									idealItem._gem1Id = idealGems[idealItem.Sockets.Color1];
									idealItem._gem2Id = idealGems[idealItem.Sockets.Color2];
									idealItem._gem3Id = idealGems[idealItem.Sockets.Color3];

									if (!ItemCache.Items.ContainsKey(idealItem.GemmedId))
									{
										Item newItem = ItemCache.Instance.AddItem(idealItem, true, false);

										//This is calling OnItemsChanged and ItemCache.Add further down the call stack so if we add it to the cache first, 
										// then do the compare and remove it if we don't want it, we can avoid that constant event trigger
										ComparisonCalculationBase upgradeCalculation = Calculations.GetItemCalculations(idealItem, character, slot);

										if (upgradeCalculation.OverallPoints < (currentCalculation.OverallPoints * .8f))
										{
											ItemCache.DeleteItem(newItem, false);
										}
									}
								}
							}
						}
					}
					else
					{
                        StatusMessaging.ReportError(slot.ToString(), null, "No response returned from Armory");
					}
				}
				StatusMessaging.UpdateStatusFinished(slot.ToString());
			}
			catch (Exception ex)
			{
                StatusMessaging.ReportError(slot.ToString(), ex, "Error interpreting the data returned from the Armory");
			}
		}
	}
}
