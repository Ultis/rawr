using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Xml;
using System.Text.RegularExpressions;
using System.Reflection;

namespace Rawr
{
	public static class Armory
	{
        public static Character GetCharacter(CharacterRegion region, string realm, string name)
        {
            string[] ignore;
            return GetCharacter(region, realm, name,out ignore);
        }

		public static Character GetCharacter(CharacterRegion region, string realm, string name, out string[] itemsOnCharacter)
		{
			XmlDocument docCharacter = null;
            //XmlDocument docTalents = null;
            try
			{
				WebRequestWrapper wrw = new WebRequestWrapper();
				docCharacter = wrw.DownloadCharacterSheet(name, region, realm);
                if (docCharacter == null)
                {
                    StatusMessaging.ReportError("Get Character", null, "No character returned from the Armory. Is the Armory down?");
                    itemsOnCharacter = null;
                    return null;
                }
                CharacterRace race = (CharacterRace)Int32.Parse(docCharacter.SelectSingleNode("page/characterInfo/character").Attributes["raceId"].Value);
                CharacterClass charClass = (CharacterClass)Int32.Parse(docCharacter.SelectSingleNode("page/characterInfo/character").Attributes["classId"].Value);

                XmlNodeList nodes = docCharacter.SelectNodes("page/characterInfo/characterTab/professions/skill");
                Profession prof1 = Profession.None, prof2 = Profession.None;
                if(nodes.Count > 0){
                    prof1 = (Profession)Int32.Parse(nodes[0].Attributes["id"].Value);
                    if (nodes.Count > 1) { // If there's only one profession, it doesn't have a second node so we don't have to worry about setting a NONE enum
                        prof2 = (Profession)Int32.Parse(nodes[1].Attributes["id"].Value);
                    }
                }

				Dictionary<CharacterSlot, string> items = new Dictionary<CharacterSlot, string>();
				//Dictionary<CharacterSlot, int> enchants = new Dictionary<CharacterSlot, int>();

				foreach (XmlNode itemNode in docCharacter.SelectNodes("page/characterInfo/characterTab/items/item"))
				{
					int slot = int.Parse(itemNode.Attributes["slot"].Value) + 1;
                    CharacterSlot cslot = Character.GetCharacterSlotFromId(slot);
					items[cslot] = string.Format("{0}.{1}.{2}.{3}.{4}", itemNode.Attributes["id"].Value,
						itemNode.Attributes["gem0Id"].Value, itemNode.Attributes["gem1Id"].Value, itemNode.Attributes["gem2Id"].Value, itemNode.Attributes["permanentenchant"].Value);
					//enchants[cslot] = int.Parse(itemNode.Attributes["permanentenchant"].Value);
				}

				/*if (items.ContainsKey(CharacterSlot.Wrist))
				{
					string[] wristIds = items[CharacterSlot.Wrist].Split('.');
					Item wristItemRaw = ItemCache.FindItemById(int.Parse(wristIds[0]));
					wristItemRaw = wristItemRaw ?? GetItem(wristIds[0] + ".0.0.0", "Loading Character from Armory");
					if (wristItemRaw.Sockets.Color1 == ItemSlot.None && wristIds[1] != "0")
					{
						items[CharacterSlot.Wrist] = string.Format("{0}.0.0.0", wristIds[0]);
						items[CharacterSlot.ExtraWristSocket] = wristIds[1] + ".0.0.0";
					}
					else if (wristItemRaw.Sockets.Color2 == ItemSlot.None && wristIds[2] != "0")
					{
						items[CharacterSlot.Wrist] = string.Format("{0}.{1}.0.0", wristIds[0], wristIds[1]);
						items[CharacterSlot.ExtraWristSocket] = wristIds[2] + ".0.0.0";
					}
					else if (wristItemRaw.Sockets.Color3 == ItemSlot.None && wristIds[3] != "0")
					{
						items[CharacterSlot.Wrist] = string.Format("{0}.{1}.{2}.0", wristIds[0], wristIds[1], wristIds[2]);
						items[CharacterSlot.ExtraWristSocket] = wristIds[3] + ".0.0.0";
					}
				}
				if (items.ContainsKey(CharacterSlot.Hands))
				{
					string[] handsIds = items[CharacterSlot.Hands].Split('.');
					Item handsItemRaw = null;
					string keyStartsWith = handsIds[0] + ".";
					foreach (string key in ItemCache.Items.Keys)
						if (key.StartsWith(keyStartsWith))
						{
							handsItemRaw = ItemCache.Items[key][0];
							break;
						}
					handsItemRaw = handsItemRaw ?? GetItem(handsIds[0] + ".0.0.0", "Loading Character from Armory");
					if (handsItemRaw.Sockets.Color1 == ItemSlot.None && handsIds[1] != "0")
					{
						items[CharacterSlot.Hands] = string.Format("{0}.0.0.0", handsIds[0]);
						items[CharacterSlot.ExtraHandsSocket] = handsIds[1] + ".0.0.0";
					}
					else if (handsItemRaw.Sockets.Color2 == ItemSlot.None && handsIds[2] != "0")
					{
						items[CharacterSlot.Hands] = string.Format("{0}.{1}.0.0", handsIds[0], handsIds[1]);
						items[CharacterSlot.ExtraHandsSocket] = handsIds[2] + ".0.0.0";
					}
					else if (handsItemRaw.Sockets.Color3 == ItemSlot.None && handsIds[3] != "0")
					{
						items[CharacterSlot.Hands] = string.Format("{0}.{1}.{2}.0", handsIds[0], handsIds[1], handsIds[2]);
						items[CharacterSlot.ExtraHandsSocket] = handsIds[3] + ".0.0.0";
					}
				}
				if (items.ContainsKey(CharacterSlot.Waist))
				{
					string[] waistIds = items[CharacterSlot.Waist].Split('.');
					Item waistItemRaw = null;
					string keyStartsWith = waistIds[0] + ".";
					foreach (string key in ItemCache.Items.Keys)
						if (key.StartsWith(keyStartsWith))
						{
							waistItemRaw = ItemCache.Items[key][0];
							break;
						}
					waistItemRaw = waistItemRaw ?? GetItem(waistIds[0] + ".0.0.0", "Loading Character from Armory");
					if (waistItemRaw.Sockets.Color1 == ItemSlot.None && waistIds[1] != "0")
					{
						items[CharacterSlot.Waist] = string.Format("{0}.0.0.0", waistIds[0]);
						items[CharacterSlot.ExtraWaistSocket] = waistIds[1] + ".0.0.0";
					}
					else if (waistItemRaw.Sockets.Color2 == ItemSlot.None && waistIds[2] != "0")
					{
						items[CharacterSlot.Waist] = string.Format("{0}.{1}.0.0", waistIds[0], waistIds[1]);
						items[CharacterSlot.ExtraWaistSocket] = waistIds[2] + ".0.0.0";
					}
					else if (waistItemRaw.Sockets.Color3 == ItemSlot.None && waistIds[3] != "0")
					{
						items[CharacterSlot.Waist] = string.Format("{0}.{1}.{2}.0", waistIds[0], waistIds[1], waistIds[2]);
						items[CharacterSlot.ExtraWaistSocket] = waistIds[3] + ".0.0.0";
					}
				}*/

                itemsOnCharacter = new string[items.Values.Count];
                items.Values.CopyTo(itemsOnCharacter, 0);
				Character character = new Character(name, realm, region, race, new BossHandler(),
					items.ContainsKey(CharacterSlot.Head) ? items[CharacterSlot.Head] : null,
					items.ContainsKey(CharacterSlot.Neck) ? items[CharacterSlot.Neck] : null,
					items.ContainsKey(CharacterSlot.Shoulders) ? items[CharacterSlot.Shoulders] : null,
					items.ContainsKey(CharacterSlot.Back) ? items[CharacterSlot.Back] : null,
					items.ContainsKey(CharacterSlot.Chest) ? items[CharacterSlot.Chest] : null,
					items.ContainsKey(CharacterSlot.Shirt) ? items[CharacterSlot.Shirt] : null,
					items.ContainsKey(CharacterSlot.Tabard) ? items[CharacterSlot.Tabard] : null,
					items.ContainsKey(CharacterSlot.Wrist) ? items[CharacterSlot.Wrist] : null,
					items.ContainsKey(CharacterSlot.Hands) ? items[CharacterSlot.Hands] : null,
					items.ContainsKey(CharacterSlot.Waist) ? items[CharacterSlot.Waist] : null,
					items.ContainsKey(CharacterSlot.Legs) ? items[CharacterSlot.Legs] : null,
					items.ContainsKey(CharacterSlot.Feet) ? items[CharacterSlot.Feet] : null,
					items.ContainsKey(CharacterSlot.Finger1) ? items[CharacterSlot.Finger1] : null,
					items.ContainsKey(CharacterSlot.Finger2) ? items[CharacterSlot.Finger2] : null,
					items.ContainsKey(CharacterSlot.Trinket1) ? items[CharacterSlot.Trinket1] : null,
					items.ContainsKey(CharacterSlot.Trinket2) ? items[CharacterSlot.Trinket2] : null,
					items.ContainsKey(CharacterSlot.MainHand) ? items[CharacterSlot.MainHand] : null,
					items.ContainsKey(CharacterSlot.OffHand) ? items[CharacterSlot.OffHand] : null,
					items.ContainsKey(CharacterSlot.Ranged) ? items[CharacterSlot.Ranged] : null,
					items.ContainsKey(CharacterSlot.Projectile) ? items[CharacterSlot.Projectile] : null,
					null
                    //items.ContainsKey(CharacterSlot.ExtraWristSocket) ? items[CharacterSlot.ExtraWristSocket] : null,
                    //items.ContainsKey(CharacterSlot.ExtraHandsSocket) ? items[CharacterSlot.ExtraHandsSocket] : null,
                    //items.ContainsKey(CharacterSlot.ExtraWaistSocket) ? items[CharacterSlot.ExtraWaistSocket] : null,
                    //enchants.ContainsKey(CharacterSlot.Head) ? enchants[CharacterSlot.Head] : 0,
                    //enchants.ContainsKey(CharacterSlot.Shoulders) ? enchants[CharacterSlot.Shoulders] : 0,
                    //enchants.ContainsKey(CharacterSlot.Back) ? enchants[CharacterSlot.Back] : 0,
                    //enchants.ContainsKey(CharacterSlot.Chest) ? enchants[CharacterSlot.Chest] : 0,
                    //enchants.ContainsKey(CharacterSlot.Wrist) ? enchants[CharacterSlot.Wrist] : 0,
                    //enchants.ContainsKey(CharacterSlot.Hands) ? enchants[CharacterSlot.Hands] : 0,
                    //enchants.ContainsKey(CharacterSlot.Legs) ? enchants[CharacterSlot.Legs] : 0,
                    //enchants.ContainsKey(CharacterSlot.Feet) ? enchants[CharacterSlot.Feet] : 0,
                    //enchants.ContainsKey(CharacterSlot.Finger1) ? enchants[CharacterSlot.Finger1] : 0,
                    //enchants.ContainsKey(CharacterSlot.Finger2) ? enchants[CharacterSlot.Finger2] : 0,
                    //enchants.ContainsKey(CharacterSlot.MainHand) ? enchants[CharacterSlot.MainHand] : 0,
                    //enchants.ContainsKey(CharacterSlot.OffHand) ? enchants[CharacterSlot.OffHand] : 0,
                    //enchants.ContainsKey(CharacterSlot.Ranged) ? enchants[CharacterSlot.Ranged] : 0
					);
                
                character.Class = charClass;

                character.PrimaryProfession = prof1;
                character.SecondaryProfession = prof2;

				XmlNode activeTalentGroup = wrw.DownloadCharacterTalentTree(character.Name, character.Region, character.Realm)
					.SelectSingleNode("page/characterInfo/talents/talentGroup[@active='1']");
                string talentCode = activeTalentGroup.SelectSingleNode("talentSpec").Attributes["value"].Value;
				switch (charClass)
				{
					case CharacterClass.Warrior:
						character.WarriorTalents = new WarriorTalents(talentCode);
						if (character.WarriorTalents.Devastate > 0) character.CurrentModel = "ProtWarr";
						else character.CurrentModel = "DPSWarr";
						break;
					case CharacterClass.Paladin:
						character.PaladinTalents = new PaladinTalents(talentCode);
						if (character.PaladinTalents.HolyShield > 0) character.CurrentModel = "ProtPaladin";
						else if (character.PaladinTalents.CrusaderStrike > 0) character.CurrentModel = "Retribution";
						else character.CurrentModel = "Healadin";
						break;
					case CharacterClass.Hunter:
						character.HunterTalents = new HunterTalents(talentCode);
						character.CurrentModel = "Hunter";
						break;
					case CharacterClass.Rogue:
						character.RogueTalents = new RogueTalents(talentCode);
				        character.CurrentModel = "Rogue";
						break;
					case CharacterClass.Priest:
						character.PriestTalents = new PriestTalents(talentCode);
						if (character.PriestTalents.Shadowform > 0) character.CurrentModel = "ShadowPriest";
						else character.CurrentModel = "HolyPriest";
						break;
					case CharacterClass.Shaman:
						character.ShamanTalents = new ShamanTalents(talentCode);
						if (character.ShamanTalents.ElementalMastery > 0) character.CurrentModel = "Elemental";
						else if (character.ShamanTalents.Stormstrike > 0) character.CurrentModel = "Enhance";
						else character.CurrentModel = "RestoSham";
						break;
					case CharacterClass.Mage:
						character.MageTalents = new MageTalents(talentCode);
						character.CurrentModel = "Mage";
						break;
					case CharacterClass.Warlock:
						character.WarlockTalents = new WarlockTalents(talentCode);
						character.CurrentModel = "Warlock";
						break;
					case CharacterClass.Druid:
						character.DruidTalents = new DruidTalents(talentCode);
						if (character.DruidTalents.ProtectorOfThePack > 0) character.CurrentModel = "Bear";
						else if (character.DruidTalents.LeaderOfThePack > 0) character.CurrentModel = "Cat";
						else if (character.DruidTalents.MoonkinForm > 0) character.CurrentModel = "Moonkin";
						else character.CurrentModel = "Tree";
						break;
					case CharacterClass.DeathKnight:
						character.DeathKnightTalents = new DeathKnightTalents(talentCode);
						if (character.DeathKnightTalents.Anticipation > 0) character.CurrentModel = "TankDK";
						else character.CurrentModel = "DPSDK";
						break;
					default:
						break;
				}
                TalentsBase talents = character.CurrentTalents;
                Dictionary<string, PropertyInfo> glyphProperty = new Dictionary<string, PropertyInfo>();
                foreach (PropertyInfo pi in talents.GetType().GetProperties())
                {
                    GlyphDataAttribute[] glyphDatas = pi.GetCustomAttributes(typeof(GlyphDataAttribute), true) as GlyphDataAttribute[];
                    if (glyphDatas.Length > 0)
                    {
                        GlyphDataAttribute glyphData = glyphDatas[0];
                        glyphProperty[glyphData.Name] = pi;
                    }
                }

                foreach (XmlNode glyph in activeTalentGroup.SelectNodes("glyphs/glyph/@name"))
                {
                    PropertyInfo pi;
                    if (glyphProperty.TryGetValue(glyph.Value, out pi))
                    {
                        pi.SetValue(talents, true, null);
                    }
                }

				InitializeAvailableItemList(character);
                ApplyRacialandProfessionBuffs(docCharacter, character);
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

		/// <summary>
		/// 09.01.01 - TankConcrete
		/// Sets up the initial list of gear that the toon has available. Method looks the
		/// gear that is currently equipped and adds it to the "AvailableItems" list if it
		/// is not already there.
		/// 
		/// If a new item is added to the "AvailableItems" list, the "unsaved" changes flag
		/// will be updated so the user knows to save.
		/// </summary>
		/// <param name="currentChar"></param>
		private static void InitializeAvailableItemList(Character currentChar)
		{
			// Get the current list of items that we know about.
			/*string[] equippedItems = currentChar.GetAllEquippedAndAvailableGearIds();
			string itemId = string.Empty;

			// Loop through the list of known and equipped items
			foreach (string item in equippedItems)
			{
				itemId = item;
				// Check to see if this is a compound item id.
				if (item.IndexOf('.') > 0)
				{
					// We're only concerned with the base item ID, so take out the .*.*.*
					itemId = item.Substring(0, item.IndexOf('.'));
				}

				// Check the list of available items to see if this item is in the list
				if (!currentChar.AvailableItems.Contains(itemId))
				{
					// Add this item to our list
					currentChar.AvailableItems.Add(itemId);
				}
			}*/
            for (CharacterSlot slot = 0; slot < (CharacterSlot)19; slot++)
            {
                ItemInstance item = currentChar[slot];
                if ((object)item != null && item.Id != 0)
                {
                    if (!currentChar.AvailableItems.Contains(item.Id.ToString())) currentChar.AvailableItems.Add(item.Id.ToString());
                    Enchant enchant = item.Enchant;
                    if (enchant != null && enchant.Id != 0)
                    {
                        string enchantString = (-1 * (enchant.Id + (10000 * (int)enchant.Slot))).ToString();
                        if (!currentChar.AvailableItems.Contains(enchantString)) currentChar.AvailableItems.Add(enchantString);
                    }
                }
            }
		}

        private static void ApplyRacialandProfessionBuffs(XmlDocument doc, Character character)
        {
            if (character.Race == CharacterRace.Draenei && !character.ActiveBuffs.Contains(Buff.GetBuffByName("Heroic Presence")))
                character.ActiveBuffsAdd(("Heroic Presence"));

            foreach (XmlNode profession in doc.SelectNodes("page/characterInfo/characterTab/professions/skill"))
            {   // apply profession buffs if max skill
                if (profession.Attributes["name"].Value == "Mining" && profession.Attributes["value"].Value == "450")
                    character.ActiveBuffsAdd(("Toughness"));
                if (profession.Attributes["name"].Value == "Skinning" && profession.Attributes["value"].Value == "450")
                    character.ActiveBuffsAdd(("Master of Anatomy"));
                if (profession.Attributes["name"].Value == "Blacksmithing" && int.Parse(profession.Attributes["value"].Value) >= 400)
                {
                    character.WristBlacksmithingSocketEnabled = true;
                    character.HandsBlacksmithingSocketEnabled = true;
                }
            }

            Calculations.GetModel(character.CurrentModel).SetDefaults(character);

        }

        public static int GetItemIdByName(string item_name)
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
						int id = Int32.Parse(items_nodes[0].Attributes["id"].InnerText);
                        return id;
                    }
                    else
                    {
                        // choose an exact match if it exists
                        foreach (XmlNode node in items_nodes)
                        {
                            if (node.Attributes["name"].InnerText == item_name)
                            {
                                int id = Int32.Parse(items_nodes[0].Attributes["id"].InnerText);
                                return id;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                StatusMessaging.ReportError("Get Item", ex, "Rawr encountered an error searching the Armory for item: " + item_name);
            }

            return -1;
        }

        public static Item GetItem(int id) { return GetItem(id, "Unknown reason"); }
		public static Item GetItem(int id, string logReason)
		{
			//Just close your eyes
			XmlDocument docItem = null;
			try
			{
				WebRequestWrapper wrw = new WebRequestWrapper();
                docItem = wrw.DownloadItemToolTipSheet(id.ToString());
                XmlDocument docItemInfo = wrw.DownloadItemInformation(id);
                ItemLocation location = LocationFactory.Create(docItem, id.ToString());
                if (docItem == null || docItem.SelectSingleNode("/page/itemTooltips/itemTooltip[1]") == null)
                {
                    Item wowhead = null;
                    wowhead = Wowhead.GetItem(id);
                    if (wowhead != null) { return wowhead; }// else we throw the error as we didn't get it from Wowhead either
                    StatusMessaging.ReportError("Get Item", null, "No item returned from Armory for " + id);
                    return null;
                }
				ItemQuality quality = ItemQuality.Common;
				ItemType type = ItemType.None;
				ItemFaction faction = ItemFaction.Neutral;
                ItemSlot socketColor1 = ItemSlot.None;
                ItemSlot socketColor2 = ItemSlot.None;
                ItemSlot socketColor3 = ItemSlot.None;
                Stats socketStats = new Stats();
                string name = string.Empty;
				string iconPath = string.Empty;
				string setName = string.Empty;
				ItemSlot slot = ItemSlot.None;
				Stats stats = new Stats();
				int inventoryType = -1;
				int classId = -1;
				string subclassName = string.Empty;
				int minDamage = 0;
				int maxDamage = 0;
                ItemDamageType damageType = ItemDamageType.Physical;
				float speed = 0f;
				List<string> requiredClasses = new List<string>();
                bool unique = false;
                int itemLevel = 0;
						
				foreach (XmlNode node in docItem.SelectNodes("page/itemTooltips/itemTooltip/name")) { name = node.InnerText; }
				foreach (XmlNode node in docItem.SelectNodes("page/itemTooltips/itemTooltip/icon")) { iconPath = node.InnerText; }
				foreach (XmlNode node in docItem.SelectNodes("page/itemTooltips/itemTooltip/maxCount")) { unique = node.InnerText == "1"; }
				foreach (XmlNode node in docItem.SelectNodes("page/itemTooltips/itemTooltip/overallQualityId")) { quality = (ItemQuality)Enum.Parse(typeof(ItemQuality), node.InnerText); }
				foreach (XmlNode node in docItem.SelectNodes("page/itemTooltips/itemTooltip/classId")) { classId = int.Parse(node.InnerText); }
				foreach (XmlNode node in docItem.SelectNodes("page/itemTooltips/itemTooltip/equipData/inventoryType")) { inventoryType = int.Parse(node.InnerText); }
				foreach (XmlNode node in docItem.SelectNodes("page/itemTooltips/itemTooltip/equipData/subclassName")) { subclassName = node.InnerText; }
				foreach (XmlNode node in docItem.SelectNodes("page/itemTooltips/itemTooltip/damageData/damage/min")) { minDamage = int.Parse(node.InnerText); }
				foreach (XmlNode node in docItem.SelectNodes("page/itemTooltips/itemTooltip/damageData/damage/max")) { maxDamage = int.Parse(node.InnerText); }
                foreach (XmlNode node in docItem.SelectNodes("page/itemTooltips/itemTooltip/damageData/damage/type")) { damageType = (ItemDamageType)Enum.Parse(typeof(ItemDamageType), node.InnerText); }
                foreach (XmlNode node in docItem.SelectNodes("page/itemTooltips/itemTooltip/damageData/speed")) { speed = float.Parse(node.InnerText, System.Globalization.CultureInfo.InvariantCulture); }
				foreach (XmlNode node in docItem.SelectNodes("page/itemTooltips/itemTooltip/setData/name")) { setName = node.InnerText.Trim(); }
				foreach (XmlNode node in docItem.SelectNodes("page/itemTooltips/itemTooltip/allowableClasses/class")) { requiredClasses.Add(node.InnerText); }

				foreach (XmlNode node in docItemInfo.SelectNodes("page/itemInfo/item/@level")) { itemLevel = int.Parse(node.InnerText); }
				foreach (XmlNode node in docItemInfo.SelectNodes("page/itemInfo/item/translationFor")) { faction = node.Attributes["factionEquiv"].Value == "1" ? ItemFaction.Alliance : ItemFaction.Horde; }

				if (inventoryType >= 0)
					slot = GetItemSlot(inventoryType, classId);
				if (!string.IsNullOrEmpty(subclassName))
					type = GetItemType(subclassName, inventoryType, classId);

				/* fix class restrictions on BOP items that can only be made by certain classes */
				switch (id)
				{
					case 35181:
					case 32495:
						requiredClasses.Add("Priest");
						break;
					case 32476:
					case 35184:
					case 32475:
					case 34355:
						requiredClasses.Add("Shaman");
						break;
					case 32474:
					case 34356:
						requiredClasses.Add("Hunter");
						break;
					case 46106:
					case 32479:
					case 32480:
					case 46109:
						requiredClasses.Add("Druid");
						break;
					case 32478:
					case 34353:
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
				foreach (XmlNode node in docItem.SelectNodes("page/itemTooltips/itemTooltip/bonusArmorPenetration")) { stats.ArmorPenetrationRating = int.Parse(node.InnerText); }
				
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

				DetermineBaseBonusArmor(stats, slot, type, id);

				foreach (XmlNode node in docItem.SelectNodes("page/itemTooltips/itemTooltip/spellData/spell"))
				{
					bool isEquip = false;
					bool isUse = false;
					string spellDesc = null;
					foreach (XmlNode childNode in node.ChildNodes)
					{
						if (childNode.Name == "trigger")
						{
							isEquip = (childNode.InnerText == "1" || childNode.InnerText == "2");
							isUse = childNode.InnerText == "0";
						}
						if (childNode.Name == "desc")
							spellDesc = childNode.InnerText;
					}

					//parse Use/Equip lines
					if (isUse) SpecialEffects.ProcessUseLine(spellDesc, stats, true, id);
					if (isEquip) SpecialEffects.ProcessEquipLine(spellDesc, stats, true);
				}

				XmlNodeList socketNodes = docItem.SelectNodes("page/itemTooltips/itemTooltip/socketData/socket");
                if (socketNodes.Count > 0) socketColor1 = (ItemSlot)Enum.Parse(typeof(ItemSlot), socketNodes[0].Attributes["color"].Value);
                if (socketNodes.Count > 1) socketColor2 = (ItemSlot)Enum.Parse(typeof(ItemSlot), socketNodes[1].Attributes["color"].Value);
                if (socketNodes.Count > 2) socketColor3 = (ItemSlot)Enum.Parse(typeof(ItemSlot), socketNodes[2].Attributes["color"].Value);
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
									socketStats.Agility = socketBonusValue;
									break;
								case "Stamina":
									socketStats.Stamina = socketBonusValue;
									break;
								case "Dodge Rating":
									socketStats.DodgeRating = socketBonusValue;
									break;
								case "Parry Rating":
									socketStats.ParryRating = socketBonusValue;
									break;
								case "Block Rating":
									socketStats.BlockRating = socketBonusValue;
									break;
								case "Block Value":
									socketStats.BlockValue = socketBonusValue;
									break;
								case "Defense Rating":
									socketStats.DefenseRating = socketBonusValue;
									break;
								case "Hit Rating":
									socketStats.HitRating = socketBonusValue;
									break;
								case "Haste Rating":
									socketStats.HasteRating = socketBonusValue;
									break;
								case "Expertise Rating":
									socketStats.ExpertiseRating = socketBonusValue;
									break;
								case "Armor Penetration Rating":
									socketStats.ArmorPenetrationRating = socketBonusValue;
									break;
								case "Strength":
									socketStats.Strength = socketBonusValue;
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
										socketStats.SpellPower = (float)Math.Round(int.Parse(socketBonuses[0].Substring(0, socketBonuses[0].IndexOf(' '))) / 1.88f);
									else
										socketStats.SpellPower = (float)Math.Round(socketBonusValue / 1.88f);
									break;
                                case "Spell Damage":
                                    // Only update Spell Damage if its not already set (Incase its an old heal bonus)
                                    if (socketStats.SpellPower == 0)
                                        socketStats.SpellPower = socketBonusValue;
                                    //sockets.Stats.Healing = socketBonusValue;
                                    break;
                                case "Spell Power":
                                    socketStats.SpellPower = socketBonusValue;
                                    break;
                                case "Crit Rating":
								case "Crit Strike Rating":
								case "Critical Rating":
								case "Critical Strike Rating":
									socketStats.CritRating = socketBonusValue;
									break;
								case "Attack Power":
									socketStats.AttackPower = socketBonusValue;
									break;
								case "Weapon Damage":
									socketStats.WeaponDamage = socketBonusValue;
									break;
								case "Resilience":
								case "Resilience Rating":
									socketStats.Resilience = socketBonusValue;
									break;
								//case "Spell Damage and Healing":
								//    sockets.Stats.SpellDamageRating = socketBonusValue;
								//    sockets.Stats.Healing = socketBonusValue;
								//    break;
								case "Spell Hit Rating":
									socketStats.HitRating = socketBonusValue;
									break;
								case "Intellect":
									socketStats.Intellect = socketBonusValue;
									break;
								case "Spell Crit":
								case "Spell Crit Rating":
								case "Spell Critical":
								case "Spell Critical Rating":
								case "Spell Critical Strike Rating":
									socketStats.CritRating = socketBonusValue;
									break;
								case "Spell Haste Rating":
									socketStats.HasteRating = socketBonusValue;
									break;
								case "Spirit":
									socketStats.Spirit = socketBonusValue;
									break;
								case "Mana every 5 seconds":
								case "Mana ever 5 Sec":
								case "mana per 5 sec":
								case "mana per 5 sec.":
								case "Mana per 5 sec.":
								case "Mana per 5 Seconds":
									socketStats.Mp5 = socketBonusValue;
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
							stats.BaseArmorMultiplier = 0.02f;
						}
                        else if (gemBonus == "Stamina +6")
                        {
                            stats.Stamina = 6.0f;
                        }
                        else if (gemBonus == "Chance to restore mana on spellcast")
                        {
							stats.AddSpecialEffect(new SpecialEffect(Trigger.SpellCast, new Stats() { ManaRestore = 600 }, 0f, 15f, .05f));
							stats.ManaRestoreOnCast_5_15 = 600; // IED
						}
                        else if (gemBonus == "Chance on spellcast - next spell cast in half time" || gemBonus == "Chance to Increase Spell Cast Speed")
                        {
                            //stats.SpellHasteFor6SecOnCast_15_45 = 320; // MSD changed in 2.4
                            stats.AddSpecialEffect(new SpecialEffect(Trigger.SpellCast, new Stats() { HasteRating = 320 }, 6, 45, 0.15f));
                        }
						else if (gemBonus == "+10% Shield Block Value")
						{
							stats.BonusBlockValueMultiplier = 0.1f;
						}
						else if (gemBonus == "+5% Shield Block Value")
						{
							stats.BonusBlockValueMultiplier = 0.05f;
						}
                        else if (gemBonus == "Minor Run Speed Increase")
                        {
                            stats.MovementSpeed = 0.08f;
                        }
                        else if (gemBonus.Contains("Stun Duration Reduced by "))
                        {
                            int bonus = int.Parse(gemBonus.Substring(gemBonus.Length - 3, 2));
                            stats.StunDurReduc = (float)bonus / 100f;
                        }
                        else if (gemBonus.Contains("Reduces Snare/Root Duration by "))
                        {
                            int bonus = int.Parse(gemBonus.Substring(gemBonus.Length - 3, 2));
                            stats.SnareRootDurReduc = (float)bonus / 100f;
                        }
                        else if (gemBonus.Contains("Fear Duration Reduced by "))
                        {
                            int bonus = int.Parse(gemBonus.Substring(gemBonus.Length - 3, 2));
                            stats.FearDurReduc = (float)bonus / 100f;
                        }
                        else if (gemBonus.Contains("Chance to Increase Melee/Ranged Attack Speed"))
                        {
                            // 480 Haste Rating on 100% Chance to proc every 60 seconds for 6 seconds
                            stats.AddSpecialEffect(new SpecialEffect(Trigger.DamageDone, new Stats() { HasteRating = 480f, }, 6f, 60f));
                        }
                        else if (gemBonus.Contains("% Spell Reflect"))
                        {
                            int bonus = int.Parse(gemBonus.Substring(0, 2).Trim('%'));
                            stats.SpellReflectChance = (float)bonus / 100f;
                        }
                        else if (gemBonus == "Sometimes Heal on Your Crits")
                        {
                            // this is supposed to be 2% of your total health healed when it procs, 50% chance to proc on crit
                            stats.AddSpecialEffect(new SpecialEffect(Trigger.PhysicalCrit, new Stats() { Health = 400f }, 0f, 0f, 0.50f));
                            stats.AddSpecialEffect(new SpecialEffect(Trigger.SpellCrit   , new Stats() { Health = 400f }, 0f, 0f, 0.50f));
                        }
                        else if (gemBonus.Contains("Reduce Spell Damage Taken by "))
                        {
                            int bonus = int.Parse(gemBonus.Substring(gemBonus.Length - 3, 2));
                            stats.SpellDamageTakenMultiplier = (float)bonus / -100f;
                        }
                        else if (gemBonus == "+2% Intellect")
                        {
                            stats.BonusIntellectMultiplier = 0.02f;
                        }
						else if (gemBonus == "+2% Mana")
                        {
                            stats.BonusManaMultiplier = 0.02f;
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
									case "Spell Penetration":
										stats.SpellPenetration = gemBonusValue;
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
				foreach (XmlNode node in docItem.SelectNodes("page/itemTooltips/itemTooltip/desc")) { desc = node.InnerText.ToLower(); }
                if (desc.Contains("matches any socket"))
                {
                    slot = ItemSlot.Prismatic;
                }
                else if (desc.Contains("matches a "))
				{
					bool red = desc.Contains("red");
					bool blue = desc.Contains("blue");
					bool yellow = desc.Contains("yellow");
					slot = red && blue && yellow ? ItemSlot.Prismatic :
						red && blue ? ItemSlot.Purple :
						blue && yellow ? ItemSlot.Green :
						red && yellow ? ItemSlot.Orange :
						red ? ItemSlot.Red :
						blue ? ItemSlot.Blue :
						yellow ? ItemSlot.Yellow :
						ItemSlot.None;
				}
				else if (desc.Contains("meta gem slot"))
					slot = ItemSlot.Meta;

                // normalize alliance/horde set names
				setName = setName.Replace("Sunstrider's", "Khadgar's")   // Mage T9
								 .Replace("Zabra's", "Velen's") // Priest T9
								 .Replace("Gul'dan's", "Kel'Thuzad's") // Warlock T9
								 .Replace("Garona's", "VanCleef's") // Rogue T9
								 .Replace("Runetotem's", "Malfurion's") // Druid T9
								 .Replace("Windrunner's Pursuit", "Windrunner's Battlegear") // Hunter T9
								 .Replace("Thrall's", "Nobundo's") // Shaman T9
								 .Replace("Liadrin's", "Turalyon's") // Paladin T9
								 .Replace("Hellscream's", "Wrynn's") // Warrior T9
                                 .Replace("Kolitra's", "Koltira's") // Fix for Death Knight T9 set name being misspelled
                                 .Replace("Koltira's", "Thassarian's") // Death Knight T9
								 .Replace("Regaila", "Regalia"); // Fix for Moonkin set name being misspelled
                Item item = new Item()
                {
					Id = id,
                    Name = name,
                    Quality = quality,
                    Type = type,
					Faction = faction,
                    IconPath = iconPath,
                    Slot = slot,
                    SetName = setName,
                    Stats = stats,
                    SocketColor1 = socketColor1,
                    SocketColor2 = socketColor2,
                    SocketColor3 = socketColor3,
                    SocketBonus = socketStats,
                    MinDamage = minDamage,
                    MaxDamage = maxDamage,
                    DamageType = damageType,
                    Speed = speed,
                    RequiredClasses = string.Join("|", requiredClasses.ToArray()),
                    Unique = unique,
                    ItemLevel = itemLevel,
                };

				//item.Stats.ConvertStatsToWotLKEquivalents();

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
                StatusMessaging.ReportError("Get Item", ex, "Rawr encountered an error getting Item from Armory: " + id);
                    //System.Windows.Forms.MessageBox.Show(string.Format("Rawr encountered an error getting Item " +
                    //"from Armory: {0}. If you still encounter this error, please copy and" +
                    //" paste this into an e-mail to cnervig@hotmail.com. Thanks!\r\n\r\nResponse: {1}\r\n\r\n\r\n{2}\r\n\r\n{3}",
                    //gemmedId, docItem.OuterXml, ex.Message, ex.StackTrace));
				//}
				return null;
			}
		}

		private static void DetermineBaseBonusArmor(Stats stats, ItemSlot slot, ItemType type, int id)
		{
			float totalArmor = stats.Armor;
			float bonusArmor = 0f;

			if (slot == ItemSlot.Finger || slot == ItemSlot.MainHand || slot == ItemSlot.Neck ||
				(slot == ItemSlot.OffHand && type != ItemType.Shield) || slot == ItemSlot.OneHand ||
				slot == ItemSlot.Trinket || slot == ItemSlot.TwoHand)
				bonusArmor = totalArmor;
			else if (id == 37084) //Flowing Cloak of Command
				bonusArmor = 364;
			else if (id == 39225) //Cloak of Armed Strife
				bonusArmor = 336;
			else if (id == 40252) //Cloak of the Shadowed Sun
				bonusArmor = 336;
			else if (id == 45267) //Saronite Plated Legguards
				bonusArmor = 826;

			stats.BonusArmor = bonusArmor;
			stats.Armor = totalArmor - bonusArmor;
		}

		private static ItemType GetItemType(string subclassName, int inventoryType, int classId)
		{
			switch (subclassName)
			{
				case "Cloth":
					return ItemType.Cloth;

				case "Leather":
					return ItemType.Leather;

				case "Mail":
					return ItemType.Mail;

				case "Plate":
					return ItemType.Plate;

				case "Dagger":
					return ItemType.Dagger;

				case "Fist Weapon":
					return ItemType.FistWeapon;

				case "Axe":
					if (inventoryType == 17)
						return ItemType.TwoHandAxe;
					else
						return ItemType.OneHandAxe;

				case "Mace":
					if (inventoryType == 17)
						return ItemType.TwoHandMace;
					else
						return ItemType.OneHandMace;

				case "Sword":
					if (inventoryType == 17)
						return ItemType.TwoHandSword;
					else
						return ItemType.OneHandSword;

				case "Polearm":
					return ItemType.Polearm;

				case "Staff":
					return ItemType.Staff;

				case "Shield":
					return ItemType.Shield;

				case "Bow":
					return ItemType.Bow;

				case "Crossbow":
					return ItemType.Crossbow;

				case "Gun":
					return ItemType.Gun;

				case "Wand":
					return ItemType.Wand;

				case "Thrown":
					return ItemType.Thrown;

				case "Idol":
					return ItemType.Idol;

				case "Libram":
					return ItemType.Libram;

				case "Totem":
					return ItemType.Totem;

				case "Arrow":
					return ItemType.Arrow;

				case "Bullet":
					return ItemType.Bullet;

				case "Quiver":
					return ItemType.Quiver;

				case "Ammo Pouch":
					return ItemType.AmmoPouch;

				case "Sigil":
					return ItemType.Sigil;

				default:
					return ItemType.None;
			}
		}

		private static ItemSlot GetItemSlot(int inventoryType, int classId)
		{
			switch (classId)
			{
				case 6:
					return ItemSlot.Projectile;

				case 11:
					return ItemSlot.ProjectileBag;
			}
					
			switch (inventoryType)
			{
				case 1:
					return ItemSlot.Head;

				case 2:
					return ItemSlot.Neck;

				case 3:
					return ItemSlot.Shoulders;

				case 16:
					return ItemSlot.Back;

				case 5:
				case 20:
					return ItemSlot.Chest;

				case 4:
					return ItemSlot.Shirt;

				case 19:
					return ItemSlot.Tabard;

				case 9:
					return ItemSlot.Wrist;

				case 10:
					return ItemSlot.Hands;

				case 6:
					return ItemSlot.Waist;

				case 7:
					return ItemSlot.Legs;

				case 8:
					return ItemSlot.Feet;

				case 11:
					return ItemSlot.Finger;

				case 12:
					return ItemSlot.Trinket;

				case 13:
					return ItemSlot.OneHand;

				case 17:
					return ItemSlot.TwoHand;

				case 21:
					return ItemSlot.MainHand;

				case 14:
				case 22:
				case 23:
					return ItemSlot.OffHand;

				case 15:
				case 25:
				case 26:
				case 28:
					return ItemSlot.Ranged;

				case 24:
					return ItemSlot.Projectile;

				case 27:
					return ItemSlot.ProjectileBag;
				
				default:
					return ItemSlot.None;
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
					if (item.Slot == ItemSlot.Blue || item.Slot == ItemSlot.Green || item.Slot == ItemSlot.Meta
						 || item.Slot == ItemSlot.Orange || item.Slot == ItemSlot.Prismatic || item.Slot == ItemSlot.Purple
						 || item.Slot == ItemSlot.Red || item.Slot == ItemSlot.Yellow)
					{
						gemCalculations.Add(Calculations.GetItemCalculations(item, character, item.Slot == ItemSlot.Meta ? CharacterSlot.Metas : CharacterSlot.Gems));
					}
				}

				ComparisonCalculationBase idealRed = null, idealBlue = null, idealYellow = null, idealMeta = null;
				foreach (ComparisonCalculationBase calc in gemCalculations)
				{
					if (Item.GemMatchesSlot(calc.Item, ItemSlot.Meta) && (idealMeta == null || idealMeta.OverallPoints < calc.OverallPoints))
						idealMeta = calc;
					if (Item.GemMatchesSlot(calc.Item, ItemSlot.Red) && (idealRed == null || idealRed.OverallPoints < calc.OverallPoints))
						idealRed = calc;
					if (Item.GemMatchesSlot(calc.Item, ItemSlot.Blue) && (idealBlue == null || idealBlue.OverallPoints < calc.OverallPoints))
						idealBlue = calc;
					if (Item.GemMatchesSlot(calc.Item, ItemSlot.Yellow) && (idealYellow == null || idealYellow.OverallPoints < calc.OverallPoints))
						idealYellow = calc;
				}
				Dictionary<ItemSlot, int> idealGems = new Dictionary<ItemSlot, int>();
				idealGems.Add(ItemSlot.Meta, idealMeta == null ? 0 : idealMeta.Item.Id);
				idealGems.Add(ItemSlot.Red, idealRed == null ? 0 : idealRed.Item.Id);
				idealGems.Add(ItemSlot.Blue, idealBlue == null ? 0 : idealBlue.Item.Id);
				idealGems.Add(ItemSlot.Yellow, idealYellow == null ? 0 : idealYellow.Item.Id);
				idealGems.Add(ItemSlot.None, 0);

				#region status queuing
				StatusMessaging.UpdateStatus(CharacterSlot.Head.ToString(), "Queued");
				StatusMessaging.UpdateStatus(CharacterSlot.Neck.ToString(), "Queued");
				StatusMessaging.UpdateStatus(CharacterSlot.Shoulders.ToString(), "Queued");
				StatusMessaging.UpdateStatus(CharacterSlot.Back.ToString(), "Queued");
				StatusMessaging.UpdateStatus(CharacterSlot.Chest.ToString(), "Queued");
				StatusMessaging.UpdateStatus(CharacterSlot.Wrist.ToString(), "Queued");
				StatusMessaging.UpdateStatus(CharacterSlot.Hands.ToString(), "Queued");
				StatusMessaging.UpdateStatus(CharacterSlot.Waist.ToString(), "Queued");
				StatusMessaging.UpdateStatus(CharacterSlot.Legs.ToString(), "Queued");
				StatusMessaging.UpdateStatus(CharacterSlot.Feet.ToString(), "Queued");
				StatusMessaging.UpdateStatus(CharacterSlot.Finger1.ToString(), "Queued");
				StatusMessaging.UpdateStatus(CharacterSlot.Finger2.ToString(), "Queued");
				StatusMessaging.UpdateStatus(CharacterSlot.Trinket1.ToString(), "Queued");
				StatusMessaging.UpdateStatus(CharacterSlot.Trinket2.ToString(), "Queued");
				StatusMessaging.UpdateStatus(CharacterSlot.MainHand.ToString(), "Queued");
				StatusMessaging.UpdateStatus(CharacterSlot.OffHand.ToString(), "Queued");
				StatusMessaging.UpdateStatus(CharacterSlot.Ranged.ToString(), "Queued");
				#endregion
				
				LoadUpgradesForSlot(character, CharacterSlot.Head, idealGems);
				LoadUpgradesForSlot(character, CharacterSlot.Neck, idealGems);
				LoadUpgradesForSlot(character, CharacterSlot.Shoulders, idealGems);
				LoadUpgradesForSlot(character, CharacterSlot.Back, idealGems);
				LoadUpgradesForSlot(character, CharacterSlot.Chest, idealGems);
				LoadUpgradesForSlot(character, CharacterSlot.Wrist, idealGems);
				LoadUpgradesForSlot(character, CharacterSlot.Hands, idealGems);
				LoadUpgradesForSlot(character, CharacterSlot.Waist, idealGems);
				LoadUpgradesForSlot(character, CharacterSlot.Legs, idealGems);
				LoadUpgradesForSlot(character, CharacterSlot.Feet, idealGems);
				LoadUpgradesForSlot(character, CharacterSlot.Finger1, idealGems);
				LoadUpgradesForSlot(character, CharacterSlot.Finger2, idealGems);
				LoadUpgradesForSlot(character, CharacterSlot.Trinket1, idealGems);
				LoadUpgradesForSlot(character, CharacterSlot.Trinket2, idealGems);
				LoadUpgradesForSlot(character, CharacterSlot.MainHand, idealGems);
				LoadUpgradesForSlot(character, CharacterSlot.OffHand, idealGems);
				LoadUpgradesForSlot(character, CharacterSlot.Ranged, idealGems);
			}
			else
			{
				System.Windows.Forms.MessageBox.Show("This feature requires your character name, realm, and region. Please fill these fields out, and try again.");
			}
		}

		private static void LoadUpgradesForSlot(Character character, CharacterSlot slot, Dictionary<ItemSlot, int> idealGems)
		{
			XmlDocument docUpgradeSearch = null;
			try
			{
				StatusMessaging.UpdateStatus(slot.ToString(), "Downloading Upgrade List");
				ItemInstance itemToUpgrade = character[slot];
                if ((object)itemToUpgrade != null)
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
                            if (!ItemCache.Instance.ContainsItemId(int.Parse(id)))
							{
								Item idealItem = GetItem(int.Parse(id), "Loading Upgrades");
								if (idealItem != null)
								{
                                    ItemInstance idealGemmedItem = new ItemInstance(int.Parse(id), idealGems[idealItem.SocketColor1], idealGems[idealItem.SocketColor2], idealGems[idealItem.SocketColor3], itemToUpgrade.EnchantId);

									Item newItem = ItemCache.AddItem(idealItem, false);

									//This is calling OnItemsChanged and ItemCache.Add further down the call stack so if we add it to the cache first, 
									// then do the compare and remove it if we don't want it, we can avoid that constant event trigger
                                    ComparisonCalculationBase upgradeCalculation = Calculations.GetItemCalculations(idealGemmedItem, character, slot);

									if (upgradeCalculation.OverallPoints < (currentCalculation.OverallPoints * .8f))
									{
										ItemCache.DeleteItem(newItem, false);
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
/*

A Tip: <?xml version="1.0" encoding="UTF-8"?><?xml-stylesheet type="text/xsl" href="/_layout/items/tooltip.xsl"?><page globalSearch="1" lang="en_us" requestUrl="/item-tooltip.xml"><itemTooltips><itemTooltip><id>48211</id><name>Malfurion's Headguard of Triumph</name><icon>inv_helmet_139`</icon><overallQualityId>4</overallQualityId><bonding>1</bonding><classId>4</classId><equipData><inventoryType>1</inventoryType><subclassName>Leather</subclassName></equipData><damageData /><bonusAgility>120</bonusAgility><bonusStamina>136</bonusStamina><armor armorBonus="0">506</armor><socketData><socket color="Meta" /><socket color="Yellow" /></socketData><durability current="70" max="70" /><allowableClasses><class>Druid</class></allowableClasses><requiredLevel>80</requiredLevel><itemLevel>245</itemLevel><bonusAttackPower>149</bonusAttackPower><bonusCritRating>90</bonusCritRating><bonusExpertiseRating>74</bonusExpertiseRating><setData><name>Malfurion's Battlegear</name><item name="Malfurion's Handgrips " /><item name="Malfurion's Headguard " /><item name="Malfurion's Legguards " /><item name="Malfurion's Raiments " /><item name="Malfurion's Shoulderpads " /><setBonus desc="Decreases the cooldown on your Growl ability by 2 sec, increases the periodic damage done by your Lacerate ability by 5%, and increases the duration of your Rake ability by 3 sec." threshold="2" /><setBonus desc="Reduces the cooldown on Barkskin by 12 sec and increases the critical strike chance of Rip and Ferocious Bite by 5%." threshold="4" /></setData><itemSource value="sourceType.vendor" /></itemTooltip></itemTooltips></page>
H Tip: <?xml version="1.0" encoding="UTF-8"?><?xml-stylesheet type="text/xsl" href="/_layout/items/tooltip.xsl"?><page globalSearch="1" lang="en_us" requestUrl="/item-tooltip.xml"><itemTooltips><itemTooltip><id>48194</id><name>Runetotem's Headguard of Triumph</name><icon>inv_helmet_145b</icon><overallQualityId>4</overallQualityId><bonding>1</bonding><classId>4</classId><equipData><inventoryType>1</inventoryType><subclassName>Leather</subclassName></equipData><damageData /><bonusAgility>120</bonusAgility><bonusStamina>136</bonusStamina><armor armorBonus="0">506</armor><socketData><socket color="Meta" /><socket color="Yellow" /></socketData><durability current="70" max="70" /><allowableClasses><class>Druid</class></allowableClasses><requiredLevel>80</requiredLevel><itemLevel>245</itemLevel><bonusAttackPower>149</bonusAttackPower><bonusCritRating>90</bonusCritRating><bonusExpertiseRating>74</bonusExpertiseRating><setData><name>Runetotem's Battlegear</name><item name="Runetotem's Handgrips " /><item name="Runetotem's Headguard " /><item name="Runetotem's Legguards " /><item name="Runetotem's Raiments " /><item name="Runetotem's Shoulderpads " /><setBonus desc="Decreases the cooldown on your Growl ability by 2 sec, increases the periodic damage done by your Lacerate ability by 5%, and increases the duration of your Rake ability by 3 sec." threshold="2" /><setBonus desc="Reduces the cooldown on Barkskin by 12 sec and increases the critical strike chance of Rip and Ferocious Bite by 5%." threshold="4" /></setData><itemSource value="sourceType.vendor" /></itemTooltip></itemTooltips></page>
A Info: <?xml version="1.0" encoding="UTF-8"?><?xml-stylesheet type="text/xsl" href="/_layout/items/info.xsl"?><page globalSearch="1" lang="en_us" requestQuery="i=48211" requestUrl="/item-info.xml"><itemInfo><item icon="inv_helmet_139`" id="48211" level="245" name="Malfurion's Headguard of Triumph" quality="4" type="Leather"><cost><token count="75" icon="spell_holy_summonchampion" id="47241" /><token count="1" icon="inv_misc_trophy_argent" id="47242" /></cost><disenchantLoot requiredSkillRank="375"><item dropRate="6" icon="inv_enchant_abysscrystal" id="34057" level="80" maxCount="1" minCount="1" name="Abyss Crystal" quality="4" type="Enchanting" /></disenchantLoot><vendors><creature area="Icecrown" classification="0" heroic="1" id="35577" maxLevel="79" minLevel="79" name="Valiant Laradia" title="Triumphant Armor Vendor" type="Humanoid" /></vendors><translationFor factionEquiv="1"><item icon="inv_helmet_145b" id="48194" level="245" name="Runetotem's Headguard of Triumph" quality="4" /></translationFor></item></itemInfo></page>
H Info: <?xml version="1.0" encoding="UTF-8"?><?xml-stylesheet type="text/xsl" href="/_layout/items/info.xsl"?><page globalSearch="1" lang="en_us" requestQuery="i=48194" requestUrl="/item-info.xml"><itemInfo><item icon="inv_helmet_145b" id="48194" level="245" name="Runetotem's Headguard of Triumph" quality="4" type="Leather"><cost><token count="75" icon="spell_holy_summonchampion" id="47241" /><token count="1" icon="inv_misc_trophy_argent" id="47242" /></cost><disenchantLoot requiredSkillRank="375"><item dropRate="6" icon="inv_enchant_abysscrystal" id="34057" level="80" maxCount="1" minCount="1" name="Abyss Crystal" quality="4" type="Enchanting" /></disenchantLoot><vendors><creature area="Icecrown" classification="0" heroic="1" id="35578" maxLevel="79" minLevel="79" name="Valiant Bressia" title="Triumphant Armor Vendor" type="Humanoid" /></vendors><translationFor factionEquiv="0"><item icon="inv_helmet_139`" id="48211" level="245" name="Malfurion's Headguard of Triumph" quality="4" /></translationFor></item></itemInfo></page>



*/