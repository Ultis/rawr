using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Xml;

namespace Rawr
{
	public static class Armory
	{
		public static Character GetCharacter(Character.CharacterRegion region, string realm, string name)
		{
			XmlDocument docCharacter = null;
            XmlDocument docTalents = null;
            try
			{
				WebRequestWrapper wrw = new WebRequestWrapper();
				docCharacter = wrw.DownloadCharacterSheet(name, region, realm);

                Character.CharacterRace race = (Character.CharacterRace)Int32.Parse(docCharacter.SelectSingleNode("page/characterInfo/character").Attributes["raceId"].Value);
                Character.CharacterClass charClass = (Character.CharacterClass)Int32.Parse(docCharacter.SelectSingleNode("page/characterInfo/character").Attributes["classId"].Value);

				Dictionary<Character.CharacterSlot, string> items = new Dictionary<Character.CharacterSlot, string>();
				Dictionary<Character.CharacterSlot, int> enchants = new Dictionary<Character.CharacterSlot, int>();

				foreach (XmlNode itemNode in docCharacter.SelectNodes("page/characterInfo/characterTab/items/item"))
				{
					int slot = int.Parse(itemNode.Attributes["slot"].Value) + 1;
					items[(Character.CharacterSlot)slot] = string.Format("{0}.{1}.{2}.{3}", itemNode.Attributes["id"].Value,
						itemNode.Attributes["gem0Id"].Value, itemNode.Attributes["gem1Id"].Value, itemNode.Attributes["gem2Id"].Value);
					enchants[(Character.CharacterSlot)slot] = int.Parse(itemNode.Attributes["permanentenchant"].Value);
				}

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

                #region Mage Talents Import
                if (docCharacter.SelectSingleNode("page/characterInfo/character").Attributes["class"].Value == "Mage")
                {
					docTalents = wrw.DownloadCharacterTalentTree(name, region, realm);

                    //<talentTab>
                    //  <talentTree value="2550050300230151333125100000000000000000000002030302010000000000000"/>
                    //</talentTab>
                    string talentCode = docTalents.SelectSingleNode("page/characterInfo/talentTab/talentTree").Attributes["value"].Value;
                    character.CalculationOptions["ArcaneSubtlety"] = talentCode.Substring(0, 1);
                    character.CalculationOptions["ArcaneFocus"] = talentCode.Substring(1, 1);
                    character.CalculationOptions["ImprovedArcaneMissiles"] = talentCode.Substring(2, 1);
                    character.CalculationOptions["WandSpecialization"] = talentCode.Substring(3, 1);
                    character.CalculationOptions["MagicAbsorption"] = talentCode.Substring(4, 1);
                    character.CalculationOptions["ArcaneConcentration"] = talentCode.Substring(5, 1);
                    character.CalculationOptions["MagicAttunement"] = talentCode.Substring(6, 1);
                    character.CalculationOptions["ArcaneImpact"] = talentCode.Substring(7, 1);
                    character.CalculationOptions["ArcaneFortitude"] = talentCode.Substring(8, 1);
                    character.CalculationOptions["ImprovedManaShield"] = talentCode.Substring(9, 1);
                    character.CalculationOptions["ImprovedCounterspell"] = talentCode.Substring(10, 1);
                    character.CalculationOptions["ArcaneMeditation"] = talentCode.Substring(11, 1);
                    character.CalculationOptions["ImprovedBlink"] = talentCode.Substring(12, 1);
                    character.CalculationOptions["PresenceOfMind"] = talentCode.Substring(13, 1);
                    character.CalculationOptions["ArcaneMind"] = talentCode.Substring(14, 1);
                    character.CalculationOptions["PrismaticCloak"] = talentCode.Substring(15, 1);
                    character.CalculationOptions["ArcaneInstability"] = talentCode.Substring(16, 1);
                    character.CalculationOptions["ArcanePotency"] = talentCode.Substring(17, 1);
                    character.CalculationOptions["EmpoweredArcaneMissiles"] = talentCode.Substring(18, 1);
                    character.CalculationOptions["ArcanePower"] = talentCode.Substring(19, 1);
                    character.CalculationOptions["SpellPower"] = talentCode.Substring(20, 1);
                    character.CalculationOptions["MindMastery"] = talentCode.Substring(21, 1);
                    character.CalculationOptions["Slow"] = talentCode.Substring(22, 1);
                    character.CalculationOptions["ImprovedFireball"] = talentCode.Substring(23, 1);
                    character.CalculationOptions["Impact"] = talentCode.Substring(24, 1);
                    character.CalculationOptions["Ignite"] = talentCode.Substring(25, 1);
                    character.CalculationOptions["FlameThrowing"] = talentCode.Substring(26, 1);
                    character.CalculationOptions["ImprovedFireBlast"] = talentCode.Substring(27, 1);
                    character.CalculationOptions["Incinerate"] = talentCode.Substring(28, 1);
                    character.CalculationOptions["ImprovedFlamestrike"] = talentCode.Substring(29, 1);
                    character.CalculationOptions["Pyroblast"] = talentCode.Substring(30, 1);
                    character.CalculationOptions["BurningSoul"] = talentCode.Substring(31, 1);
                    character.CalculationOptions["ImprovedScorch"] = talentCode.Substring(32, 1);
                    character.CalculationOptions["ImprovedFireWard"] = talentCode.Substring(33, 1);
                    character.CalculationOptions["MasterOfElements"] = talentCode.Substring(34, 1);
                    character.CalculationOptions["PlayingWithFire"] = talentCode.Substring(35, 1);
                    character.CalculationOptions["CriticalMass"] = talentCode.Substring(36, 1);
                    character.CalculationOptions["BlastWave"] = talentCode.Substring(37, 1);
                    character.CalculationOptions["BlazingSpeed"] = talentCode.Substring(38, 1);
                    character.CalculationOptions["FirePower"] = talentCode.Substring(39, 1);
                    character.CalculationOptions["Pyromaniac"] = talentCode.Substring(40, 1);
                    character.CalculationOptions["Combustion"] = talentCode.Substring(41, 1);
                    character.CalculationOptions["MoltenFury"] = talentCode.Substring(42, 1);
                    character.CalculationOptions["EmpoweredFireball"] = talentCode.Substring(43, 1);
                    character.CalculationOptions["DragonsBreath"] = talentCode.Substring(44, 1);
                    character.CalculationOptions["FrostWarding"] = talentCode.Substring(45, 1);
                    character.CalculationOptions["ImprovedFrostbolt"] = talentCode.Substring(46, 1);
                    character.CalculationOptions["ElementalPrecision"] = talentCode.Substring(47, 1);
                    character.CalculationOptions["IceShards"] = talentCode.Substring(48, 1);
                    character.CalculationOptions["Frostbite"] = talentCode.Substring(49, 1);
                    character.CalculationOptions["ImprovedFrostNova"] = talentCode.Substring(50, 1);
                    character.CalculationOptions["Permafrost"] = talentCode.Substring(51, 1);
                    character.CalculationOptions["PiercingIce"] = talentCode.Substring(52, 1);
                    character.CalculationOptions["IcyVeins"] = talentCode.Substring(53, 1);
                    character.CalculationOptions["ImprovedBlizzard"] = talentCode.Substring(54, 1);
                    character.CalculationOptions["ArcticReach"] = talentCode.Substring(55, 1);
                    character.CalculationOptions["FrostChanneling"] = talentCode.Substring(56, 1);
                    character.CalculationOptions["Shatter"] = talentCode.Substring(57, 1);
                    character.CalculationOptions["FrozenCore"] = talentCode.Substring(58, 1);
                    character.CalculationOptions["ColdSnap"] = talentCode.Substring(59, 1);
                    character.CalculationOptions["ImprovedConeOfCold"] = talentCode.Substring(60, 1);
                    character.CalculationOptions["IceFloes"] = talentCode.Substring(61, 1);
                    character.CalculationOptions["WintersChill"] = talentCode.Substring(62, 1);
                    character.CalculationOptions["IceBarrier"] = talentCode.Substring(63, 1);
                    character.CalculationOptions["ArcticWinds"] = talentCode.Substring(64, 1);
                    character.CalculationOptions["EmpoweredFrostbolt"] = talentCode.Substring(65, 1);
                    character.CalculationOptions["SummonWaterElemental"] = talentCode.Substring(66, 1);
                }
                #endregion

                #region Druid Talents Import
                if (docCharacter.SelectSingleNode("page/characterInfo/character").Attributes["class"].Value == "Druid")
                {
					docTalents = wrw.DownloadCharacterTalentTree(name, region, realm);

                    //<talentTab>
                    //  <talentTree value="50002201050313523105100000000000000530000000000300001000030300"/>
                    //</talentTab>
                    string talentCode = docTalents.SelectSingleNode("page/characterInfo/talentTab/talentTree").Attributes["value"].Value;
                    character.CalculationOptions["StarlightWrath"] = talentCode.Substring(0, 1);
                    character.CalculationOptions["NaturesGrasp"] = talentCode.Substring(1, 1);
                    character.CalculationOptions["ImpNaturesGrasp"] = talentCode.Substring(2, 1);
                    character.CalculationOptions["ControlofNature"] = talentCode.Substring(3, 1);
                    character.CalculationOptions["FocusedStarlight"] = talentCode.Substring(4, 1);
                    character.CalculationOptions["ImpMoonfire"] = talentCode.Substring(5, 1);
                    character.CalculationOptions["Brambles"] = talentCode.Substring(6, 1);
                    character.CalculationOptions["InsectSwarm"] = talentCode.Substring(7, 1);
                    character.CalculationOptions["NaturesReach"] = talentCode.Substring(8, 1);
                    character.CalculationOptions["Vengeance"] = talentCode.Substring(9, 1);
                    character.CalculationOptions["CelestialFocus"] = talentCode.Substring(10, 1);
                    character.CalculationOptions["LunarGuidance"] = talentCode.Substring(11, 1);
                    character.CalculationOptions["NaturesGrace"] = talentCode.Substring(12, 1);
                    character.CalculationOptions["Moonglow"] = talentCode.Substring(13, 1);
                    character.CalculationOptions["Moonfury"] = talentCode.Substring(14, 1);
                    character.CalculationOptions["BalanceofPower"] = talentCode.Substring(15, 1);
                    character.CalculationOptions["Dreamstate"] = talentCode.Substring(16, 1);
                    character.CalculationOptions["MoonkinForm"] = talentCode.Substring(17, 1);
                    character.CalculationOptions["ImprovedFF"] = talentCode.Substring(18, 1);
                    character.CalculationOptions["WrathofCenarius"] = talentCode.Substring(19, 1);
                    character.CalculationOptions["ForceofNature"] = talentCode.Substring(20, 1);
                    character.CalculationOptions["Ferocity"] = talentCode.Substring(21, 1);
                    character.CalculationOptions["FeralAggression"] = talentCode.Substring(22, 1);
                    character.CalculationOptions["FeralInstinct"] = talentCode.Substring(23, 1);
                    character.CalculationOptions["Brutal Impact"] = talentCode.Substring(24, 1);
                    character.CalculationOptions["ThickHide"] = talentCode.Substring(25, 1);
                    character.CalculationOptions["FeralSwiftness"] = talentCode.Substring(26, 1);
                    character.CalculationOptions["FeralCharge"] = talentCode.Substring(27, 1);
                    character.CalculationOptions["SharpenedClaws"] = talentCode.Substring(28, 1);
                    character.CalculationOptions["ShreddingAttacks"] = talentCode.Substring(29, 1);
                    character.CalculationOptions["PredatoryStrikes"] = talentCode.Substring(30, 1);
                    character.CalculationOptions["PrimalFury"] = talentCode.Substring(31, 1);
                    character.CalculationOptions["SavageFury"] = talentCode.Substring(32, 1);
                    character.CalculationOptions["FeralFaerieFire"] = talentCode.Substring(33, 1);
                    character.CalculationOptions["NurturingInstinct"] = talentCode.Substring(34, 1);
                    character.CalculationOptions["HotW"] = talentCode.Substring(35, 1);
                    character.CalculationOptions["SotF"] = talentCode.Substring(36, 1);
                    character.CalculationOptions["PrimalTenacity"] = talentCode.Substring(37, 1);
                    character.CalculationOptions["LotP"] = talentCode.Substring(38, 1);
                    character.CalculationOptions["ImprovedLotP"] = talentCode.Substring(39, 1);
                    character.CalculationOptions["PredatoryInstincts"] = talentCode.Substring(40, 1);
                    character.CalculationOptions["Mangle"] = talentCode.Substring(41, 1);
                    character.CalculationOptions["ImprovedMotW"] = talentCode.Substring(42, 1);
                    character.CalculationOptions["Furor"] = talentCode.Substring(43, 1);
                    character.CalculationOptions["Naturalist"] = talentCode.Substring(44, 1);
                    character.CalculationOptions["NaturesFocus"] = talentCode.Substring(45, 1);
                    character.CalculationOptions["NaturalShapeshifter"] = talentCode.Substring(46, 1);
                    character.CalculationOptions["Intensity"] = talentCode.Substring(47, 1);
                    character.CalculationOptions["Subtlety"] = talentCode.Substring(48, 1);
                    character.CalculationOptions["OmenofClarity"] = talentCode.Substring(49, 1);
                    character.CalculationOptions["TranquilSpirit"] = talentCode.Substring(50, 1);
                    character.CalculationOptions["ImprovedRejuv"] = talentCode.Substring(51, 1);
                    character.CalculationOptions["NaturesSwiftness"] = talentCode.Substring(52, 1);
                    character.CalculationOptions["GiftofNature"] = talentCode.Substring(53, 1);
                    character.CalculationOptions["ImpTranquility"] = talentCode.Substring(54, 1);
                    character.CalculationOptions["EmpoweredTouch"] = talentCode.Substring(55, 1);
                    character.CalculationOptions["ImprovedRegrowth"] = talentCode.Substring(56, 1);
                    character.CalculationOptions["LivingSpirit"] = talentCode.Substring(57, 1);
                    character.CalculationOptions["Swiftmend"] = talentCode.Substring(58, 1);
                    character.CalculationOptions["NaturalPerfection"] = talentCode.Substring(59, 1);
                    character.CalculationOptions["EmpoweredRejuv"] = talentCode.Substring(60, 1);
                    character.CalculationOptions["TreeofLife"] = talentCode.Substring(61, 1);;
                }
                #endregion


                
                character.Class = charClass;
                
                //build the talent tree 
                character.Talents.SetCharacter(character);

                //I will tell you how he lived.
				return character;
			}
			catch (Exception ex)
			{
				if (docCharacter == null || docCharacter.InnerXml.Length == 0)
				{
					System.Windows.Forms.MessageBox.Show(string.Format("Rawr encountered an error getting Character " +
					"from Armory: {0}@{1}-{2}. Please check to make sure you've spelled the character name and realm" +
					" exactly right, and chosen the correct Region. Rawr recieved no response to its query for character" +
					" data, so if the character name/region/realm are correct, please check to make sure that no firewall " +
					"or proxy software is blocking Rawr. If you still encounter this error, please copy and" +
					" paste this into an e-mail to cnervig@hotmail.com. Thanks!\r\n\r\nResponse: {3}\r\n\r\n\r\n{4}\r\n\r\n{5}",
					name, region.ToString(), realm, "null", ex.Message, ex.StackTrace));
				}
				else
				{
					System.Windows.Forms.MessageBox.Show(string.Format("Rawr encountered an error getting Character " +
					"from Armory: {0}@{1}-{2}. Please check to make sure you've spelled the character name and realm" + 
					" exactly right, and chosen the correct Region. If you still encounter this error, please copy and" +
					" paste this into an e-mail to cnervig@hotmail.com. Thanks!\r\n\r\nResponse: {3}\r\n\r\n\r\n{4}\r\n\r\n{5}",
					name, region.ToString(), realm, docCharacter.OuterXml, ex.Message, ex.StackTrace));
				}
				return null;
			}
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
                if (docItem == null)
                {
                    //No such item exists.
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

				foreach (XmlNode node in docItem.SelectNodes("page/itemTooltips/itemTooltip/name")) { name = node.InnerText; }
				foreach (XmlNode node in docItem.SelectNodes("page/itemTooltips/itemTooltip/icon")) { iconPath = node.InnerText; }
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

				if (inventoryType >= 0)
					slot = GetItemSlot(inventoryType, classId);
				if (!string.IsNullOrEmpty(subclassName))
					type = GetItemType(subclassName, inventoryType, classId);

				foreach (XmlNode node in docItem.SelectNodes("page/itemTooltips/itemTooltip/bonusAgility")) { stats.Agility = int.Parse(node.InnerText); }
				foreach (XmlNode node in docItem.SelectNodes("page/itemTooltips/itemTooltip/armor")) { stats.Armor = int.Parse(node.InnerText); }
				foreach (XmlNode node in docItem.SelectNodes("page/itemTooltips/itemTooltip/bonusDefenseSkillRating")) { stats.DefenseRating = int.Parse(node.InnerText); }
				foreach (XmlNode node in docItem.SelectNodes("page/itemTooltips/itemTooltip/bonusDodgeRating")) { stats.DodgeRating = int.Parse(node.InnerText); }
				foreach (XmlNode node in docItem.SelectNodes("page/itemTooltips/itemTooltip/bonusResilienceRating")) { stats.Resilience = int.Parse(node.InnerText); }
				foreach (XmlNode node in docItem.SelectNodes("page/itemTooltips/itemTooltip/bonusStamina")) { stats.Stamina = int.Parse(node.InnerText); }
                foreach (XmlNode node in docItem.SelectNodes("page/itemTooltips/itemTooltip/bonusIntellect")) { stats.Intellect = int.Parse(node.InnerText); }

				foreach (XmlNode node in docItem.SelectNodes("page/itemTooltips/itemTooltip/bonusStrength")) { stats.Strength = int.Parse(node.InnerText); }
				foreach (XmlNode node in docItem.SelectNodes("page/itemTooltips/itemTooltip/bonusHitRating")) { stats.HitRating = int.Parse(node.InnerText); }
				foreach (XmlNode node in docItem.SelectNodes("page/itemTooltips/itemTooltip/bonusHasteRating")) { stats.HasteRating = int.Parse(node.InnerText); }
				foreach (XmlNode node in docItem.SelectNodes("page/itemTooltips/itemTooltip/bonusCritRating")) { stats.CritRating = int.Parse(node.InnerText); }
				foreach (XmlNode node in docItem.SelectNodes("page/itemTooltips/itemTooltip/bonusExpertiseRating")) { stats.ExpertiseRating = int.Parse(node.InnerText); }

                
				foreach (XmlNode node in docItem.SelectNodes("page/itemTooltips/itemTooltip/arcaneResist")) { stats.ArcaneResistance = int.Parse(node.InnerText); }
				foreach (XmlNode node in docItem.SelectNodes("page/itemTooltips/itemTooltip/fireResist")) { stats.FireResistance = int.Parse(node.InnerText); }
				foreach (XmlNode node in docItem.SelectNodes("page/itemTooltips/itemTooltip/frostResist")) { stats.FrostResistance = int.Parse(node.InnerText); }
				foreach (XmlNode node in docItem.SelectNodes("page/itemTooltips/itemTooltip/natureResist")) { stats.NatureResistance = int.Parse(node.InnerText); }
				foreach (XmlNode node in docItem.SelectNodes("page/itemTooltips/itemTooltip/shadowResist")) { stats.ShadowResistance = int.Parse(node.InnerText); }

                foreach (XmlNode node in docItem.SelectNodes("page/itemTooltips/itemTooltip/bonusCritSpellRating")) { stats.SpellCritRating = int.Parse(node.InnerText); }
                foreach (XmlNode node in docItem.SelectNodes("page/itemTooltips/itemTooltip/bonusHitSpellRating")) { stats.SpellHitRating = int.Parse(node.InnerText); }
                foreach (XmlNode node in docItem.SelectNodes("page/itemTooltips/itemTooltip/bonusHasteSpellRating")) { stats.SpellHasteRating = int.Parse(node.InnerText); }

                foreach (XmlNode node in docItem.SelectNodes("page/itemTooltips/itemTooltip/bonusMana")) { stats.Mana = int.Parse(node.InnerText); }
                foreach (XmlNode node in docItem.SelectNodes("page/itemTooltips/itemTooltip/bonusSpirit")) { stats.Spirit = int.Parse(node.InnerText); }
                

                
				
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
					if (isUse)
					{
						if (spellDesc.StartsWith("Increases attack power by 320 for 12 sec."))
							stats.AttackPower += 21f; //Nightseye Panther
						else if (spellDesc.StartsWith("Increases attack power by 185 for 15 sec."))
							stats.AttackPower += 23f; //Uniting Charm + Ogre Mauler's Badge
						else if (spellDesc.StartsWith("Increases attack power by "))
						{
							spellDesc = spellDesc.Substring("Increases attack power by ".Length);
							if (spellDesc.Contains(".")) spellDesc = spellDesc.Substring(0, spellDesc.IndexOf("."));
							if (spellDesc.Contains(" ")) spellDesc = spellDesc.Substring(0, spellDesc.IndexOf(" "));
							stats.AttackPower += ((float)int.Parse(spellDesc)) / 6f;
						}
						else if (spellDesc.StartsWith("Increases your melee and ranged attack power by "))
						{
							spellDesc = spellDesc.Substring("Increases your melee and ranged attack power by ".Length);
							if (spellDesc.Contains(".")) spellDesc = spellDesc.Substring(0, spellDesc.IndexOf("."));
							if (spellDesc.Contains(" ")) spellDesc = spellDesc.Substring(0, spellDesc.IndexOf(" "));
							stats.AttackPower += ((float)int.Parse(spellDesc)) / 6f;
						}
						else if (spellDesc.StartsWith("Increases haste rating by "))
						{
							spellDesc = spellDesc.Substring("Increases haste rating by ".Length);
							if (spellDesc.Contains(".")) spellDesc = spellDesc.Substring(0, spellDesc.IndexOf("."));
							if (spellDesc.Contains(" ")) spellDesc = spellDesc.Substring(0, spellDesc.IndexOf(" "));
							stats.HasteRating += ((float)int.Parse(spellDesc)) / 12f;
						}
						else if (spellDesc.StartsWith("Your attacks ignore "))
						{
							spellDesc = spellDesc.Substring("Your attacks ignore ".Length);
							if (spellDesc.Contains(".")) spellDesc = spellDesc.Substring(0, spellDesc.IndexOf("."));
							if (spellDesc.Contains(" ")) spellDesc = spellDesc.Substring(0, spellDesc.IndexOf(" "));
							stats.ArmorPenetration += ((float)int.Parse(spellDesc)) / 6f;
						}
						else if (spellDesc.StartsWith("Increases agility by "))
						{ //Special case: So that we don't increase bear stats by the average value, translate the agi to crit and ap
							spellDesc = spellDesc.Substring("Increases agility by ".Length);
							if (spellDesc.Contains(".")) spellDesc = spellDesc.Substring(0, spellDesc.IndexOf("."));
							if (spellDesc.Contains(" ")) spellDesc = spellDesc.Substring(0, spellDesc.IndexOf(" "));
							stats.CritRating += ((((float)int.Parse(spellDesc)) / 6f) / 25f) * 22.08f;
							stats.AttackPower += (((float)int.Parse(spellDesc)) / 6f) * 1.03f;
						}
                        // Increases damage and healing done by magical spells and effects by up to 211 for 20 sec.
                        else if (spellDesc.StartsWith("Increases damage and healing done by magical spells and effects by up to "))
                        {
                            spellDesc = spellDesc.Substring("Increases damage and healing done by magical spells and effects by up to ".Length);
                            string[] tokens = spellDesc.Split(' ');
                            int damageIncrease = int.Parse(tokens[0]);
                            int duration = int.Parse(tokens[2]);
                            switch (duration)
                            {
                                case 20:
                                    stats.SpellDamageFor20SecOnUse2Min += damageIncrease;
                                    break;
                            }
                        }
                        else if (spellDesc.StartsWith("Increases spell damage by up to 150 and healing by up to 280 for 15 sec."))
                        {
                            stats.SpellDamageFor15SecOnUse90Sec += 150;
                            // TODO healing effect
                        }
                        else if (spellDesc.StartsWith("Tap into the power of the skull, increasing spell haste rating by 175 for 20 sec."))
                        {
                            stats.SpellHasteFor20SecOnUse2Min += 175;
                        }
                        else if (spellDesc.StartsWith("Each spell cast within 20 seconds will grant a stacking bonus of 21 mana regen per 5 sec. Expires after 20 seconds.  Abilities with no mana cost will not trigger this trinket."))
                        {
                            stats.Mp5OnCastFor20SecOnUse2Min += 21;
                        }
                        // Figurine - Talasite Owl, 5 min cooldown
                        else if (spellDesc.StartsWith("Restores 900 mana over 12 sec."))
                        {
                            stats.Mp5 += 5f * 900f / 300f;
                        }
                    }

					if (isEquip)
					{
                        if (spellDesc.StartsWith("Each time you deal melee or ranged damage to an opponent, you gain 6 attack power for the next 10 sec., stacking up to 20 times.  Each time you land a harmful spell on an opponent, you gain 8 spell damage for the next 10 sec., stacking up to 10 times."))
                        {
                            stats.AttackPower += 120; //Crusade = 120ap
                            stats.SpellDamageRating += 80;
                        }
                        else if (spellDesc.StartsWith("Your melee and ranged attacks have a chance to inject poison"))
                            stats.WeaponDamage += 2f; //Romulo's = 4dmg
                        else if (spellDesc.StartsWith("Mangle has a 40% chance to grant 140 Strength for 8sec"))
                            stats.Strength += 37f; //Ashtongue = 37str
                        else if (spellDesc.StartsWith("Your spells and attacks in each form have a chance to grant you a blessing for 15 sec."))
                            stats.Strength += 32f; //LivingRoot = 32str
                        else if (spellDesc.StartsWith("Chance on critical hit to increase your attack power by "))
                        {
                            spellDesc = spellDesc.Substring("Chance on critical hit to increase your attack power by ".Length);
                            if (spellDesc.Contains(".")) spellDesc = spellDesc.Substring(0, spellDesc.IndexOf("."));
                            if (spellDesc.Contains(" ")) spellDesc = spellDesc.Substring(0, spellDesc.IndexOf(" "));
                            stats.AttackPower += ((float)int.Parse(spellDesc)) / 6f;
                        }
                        else if (spellDesc.StartsWith("Chance on hit to increase your attack power by "))
                        {
                            spellDesc = spellDesc.Substring("Chance on hit to increase your attack power by ".Length);
                            if (spellDesc.Contains(".")) spellDesc = spellDesc.Substring(0, spellDesc.IndexOf("."));
                            if (spellDesc.Contains(" ")) spellDesc = spellDesc.Substring(0, spellDesc.IndexOf(" "));
                            stats.AttackPower += ((float)int.Parse(spellDesc)) / 6f;
                        }
                        else if (spellDesc.Contains(" critical strike rating to the Leader of the Pack aura"))
                        {
                            spellDesc = spellDesc.Substring(0, spellDesc.IndexOf(" critical strike rating to the Leader of the Pack aura"));
                            spellDesc = spellDesc.Substring(spellDesc.LastIndexOf(' ') + 1);
                            if (spellDesc.Contains(".")) spellDesc = spellDesc.Substring(0, spellDesc.IndexOf("."));
                            if (spellDesc.Contains(" ")) spellDesc = spellDesc.Substring(0, spellDesc.IndexOf(" "));
                            stats.CritRating += (float)int.Parse(spellDesc);
                        }
                        else if (spellDesc.StartsWith("Your Mangle ability also increases your attack power by "))
                        {
                            spellDesc = spellDesc.Substring("Your Mangle ability also increases your attack power by ".Length);
                            if (spellDesc.Contains(".")) spellDesc = spellDesc.Substring(0, spellDesc.IndexOf("."));
                            if (spellDesc.Contains(" ")) spellDesc = spellDesc.Substring(0, spellDesc.IndexOf(" "));
                            stats.AttackPower += (float)int.Parse(spellDesc);
                        }
                        else if (spellDesc.StartsWith("Increases periodic damage done by Rip by "))
                        {
                            spellDesc = spellDesc.Substring("Increases periodic damage done by Rip by ".Length);
                            if (spellDesc.Contains(".")) spellDesc = spellDesc.Substring(0, spellDesc.IndexOf("."));
                            if (spellDesc.Contains(" ")) spellDesc = spellDesc.Substring(0, spellDesc.IndexOf(" "));
                            stats.BonusRipDamagePerCPPerTick += (float)int.Parse(spellDesc);
                        }
                        else if (spellDesc.StartsWith("Your melee and ranged attacks have a chance to increase your haste rating by "))
                        {
                            spellDesc = spellDesc.Substring("Your melee and ranged attacks have a chance to increase your haste rating by ".Length);
                            if (spellDesc.Contains(".")) spellDesc = spellDesc.Substring(0, spellDesc.IndexOf("."));
                            if (spellDesc.Contains(" ")) spellDesc = spellDesc.Substring(0, spellDesc.IndexOf(" "));
                            stats.HasteRating += ((float)int.Parse(spellDesc)) / 4f;
                        }
                        else if (spellDesc.StartsWith("Your melee and ranged attacks have a chance allow you to ignore "))
                        {
                            spellDesc = spellDesc.Substring("Your melee and ranged attacks have a chance allow you to ignore ".Length);
                            if (spellDesc.Contains(".")) spellDesc = spellDesc.Substring(0, spellDesc.IndexOf("."));
                            if (spellDesc.Contains(" ")) spellDesc = spellDesc.Substring(0, spellDesc.IndexOf(" "));
                            stats.ArmorPenetration += ((float)int.Parse(spellDesc)) / 3f;
                        }
                        else if (spellDesc.StartsWith("Increases attack power by "))
                        {
                            spellDesc = spellDesc.Substring("Increases attack power by ".Length);
                            if (spellDesc.Contains(".")) spellDesc = spellDesc.Substring(0, spellDesc.IndexOf("."));
                            if (spellDesc.Contains(" ")) spellDesc = spellDesc.Substring(0, spellDesc.IndexOf(" "));
                            stats.AttackPower += int.Parse(spellDesc);
                        }
                        else if (spellDesc.StartsWith("Increases your dodge rating by "))
                        {
                            spellDesc = spellDesc.Substring("Increases your dodge rating by ".Length);
                            if (spellDesc.Contains(".")) spellDesc = spellDesc.Substring(0, spellDesc.IndexOf("."));
                            if (spellDesc.Contains(" ")) spellDesc = spellDesc.Substring(0, spellDesc.IndexOf(" "));
                            stats.DodgeRating += int.Parse(spellDesc);
                        }
                        else if (spellDesc.StartsWith("Increases your hit rating by "))
                        {
                            spellDesc = spellDesc.Substring("Increases your hit rating by ".Length);
                            if (spellDesc.Contains(".")) spellDesc = spellDesc.Substring(0, spellDesc.IndexOf("."));
                            if (spellDesc.Contains(" ")) spellDesc = spellDesc.Substring(0, spellDesc.IndexOf(" "));
                            stats.HitRating += int.Parse(spellDesc);
                        }
                        else if (spellDesc.StartsWith("Your attacks ignore "))
                        {
                            spellDesc = spellDesc.Substring("Your attacks ignore ".Length);
                            if (spellDesc.Contains(".")) spellDesc = spellDesc.Substring(0, spellDesc.IndexOf("."));
                            if (spellDesc.Contains(" ")) spellDesc = spellDesc.Substring(0, spellDesc.IndexOf(" "));
                            stats.ArmorPenetration += int.Parse(spellDesc);
                        }
                        else if (spellDesc.StartsWith("Increases the damage dealt by Shred by "))
                        {
                            spellDesc = spellDesc.Substring("Increases the damage dealt by Shred by ".Length);
                            if (spellDesc.Contains(".")) spellDesc = spellDesc.Substring(0, spellDesc.IndexOf("."));
                            if (spellDesc.Contains(" ")) spellDesc = spellDesc.Substring(0, spellDesc.IndexOf(" "));
                            stats.BonusShredDamage += int.Parse(spellDesc);
                        }
                        else if (spellDesc.StartsWith("Increases the damage dealt by Mangle (Cat) by "))
                        {
                            spellDesc = spellDesc.Substring("Increases the damage dealt by Mangle (Cat) by ".Length);
                            if (spellDesc.Contains(".")) spellDesc = spellDesc.Substring(0, spellDesc.IndexOf("."));
                            if (spellDesc.Contains(" ")) spellDesc = spellDesc.Substring(0, spellDesc.IndexOf(" "));
                            stats.BonusMangleDamage += int.Parse(spellDesc);
                        }
                        else if (spellDesc.EndsWith(" Weapon Damage."))
                        {
                            spellDesc = spellDesc.Trim('+').Substring(0, spellDesc.IndexOf(" "));
                            stats.WeaponDamage += int.Parse(spellDesc);
                        }
                        else if (spellDesc.StartsWith("Your Mangle ability has a chance to grant "))
                        {
                            spellDesc = spellDesc.Substring("Your Mangle ability has a chance to grant ".Length);
                            if (spellDesc.Contains(".")) spellDesc = spellDesc.Substring(0, spellDesc.IndexOf("."));
                            if (spellDesc.Contains(" ")) spellDesc = spellDesc.Substring(0, spellDesc.IndexOf(" "));
                            stats.TerrorProc += int.Parse(spellDesc);
                        }
                        else if (spellDesc.StartsWith("Increases damage and healing done by magical spells and effects by up to"))
                        {
                            spellDesc = spellDesc.Substring("Increases damage and healing done by magical spells and effects by up to".Length);
                            spellDesc = spellDesc.Replace(".", "").Replace(" ", "");
                            stats.SpellDamageRating += int.Parse(spellDesc);
                        }
                        // Increases healing done by up to 375 and damage done by up to 125 for all magical spells and effects.
                        else if (spellDesc.StartsWith("Increases healing done by up to "))
                        {
                            stats.Healing += int.Parse(spellDesc.Split(' ')[6]);
                            spellDesc = spellDesc.Substring(spellDesc.IndexOf("damage done by up to "));
                            spellDesc = spellDesc.Substring("damage done by up to ".Length);
                            spellDesc = spellDesc.Substring(0, spellDesc.IndexOf(" for all magical spells and effects."));
                            stats.SpellDamageRating += int.Parse(spellDesc);
                        }
                        else if (spellDesc.StartsWith("Increases damage done by Shadow spells and effects by up to"))
                        {
                            spellDesc = spellDesc.Substring("Increases damage done by Shadow spells and effects by up to".Length);
                            spellDesc = spellDesc.Replace(".", "").Replace(" ", "");
                            stats.SpellShadowDamageRating += int.Parse(spellDesc);
                        }
                        else if (spellDesc.StartsWith("Increases damage done by Fire spells and effects by up to"))
                        {
                            spellDesc = spellDesc.Substring("Increases damage done by Fire spells and effects by up to".Length);
                            spellDesc = spellDesc.Replace(".", "").Replace(" ", "");
                            stats.SpellFireDamageRating += int.Parse(spellDesc);
                        }
                        else if (spellDesc.StartsWith("Increases damage done by Frost spells and effects by up to"))
                        {
                            spellDesc = spellDesc.Substring("Increases damage done by Frost spells and effects by up to".Length);
                            spellDesc = spellDesc.Replace(".", "").Replace(" ", "");
                            stats.SpellFrostDamageRating += int.Parse(spellDesc);
                        }
                        else if (spellDesc.StartsWith("Increases damage done by Arcane spells and effects by up to"))
                        {
                            spellDesc = spellDesc.Substring("Increases damage done by Arcane spells and effects by up to".Length);
                            spellDesc = spellDesc.Replace(".", "").Replace(" ", "");
                            stats.SpellArcaneDamageRating += int.Parse(spellDesc);
                        }
                        else if (spellDesc.StartsWith("Increases damage done by Nature spells and effects by up to"))
                        {
                            spellDesc = spellDesc.Substring("Increases damage done by Nature spells and effects by up to".Length);
                            spellDesc = spellDesc.Replace(".", "").Replace(" ", "");
                            stats.SpellNatureDamageRating += int.Parse(spellDesc);
                        }
                        else if (spellDesc.StartsWith("Improves spell haste rating by"))
                        {
                            spellDesc = spellDesc.Substring("Improves spell haste rating by".Length);
                            spellDesc = spellDesc.Replace(".", "").Replace(" ", "");
                            stats.SpellHasteRating += int.Parse(spellDesc);
                        }
                        else if (spellDesc.StartsWith("Improves spell critical strike rating by"))
                        {
                            spellDesc = spellDesc.Substring("Improves spell critical strike rating by".Length);
                            spellDesc = spellDesc.Replace(".", "").Replace(" ", "");
                            stats.SpellCritRating += int.Parse(spellDesc);
                        }
                        else if (spellDesc.StartsWith("Increases your spell penetration by"))
                        {
                            spellDesc = spellDesc.Substring("Increases your spell penetration by".Length);
                            spellDesc = spellDesc.Replace(".", "").Replace(" ", "");
                            stats.SpellPenetration += int.Parse(spellDesc);
                        }
                        else if (spellDesc.StartsWith("Increases your spell hit rating by "))
                        {
                            spellDesc = spellDesc.Substring("Increases your spell hit rating by ".Length);
                            spellDesc = spellDesc.Replace(".", "").Replace(" ", "");
                            stats.SpellHitRating += int.Parse(spellDesc);
                        }
                        // Restores 7 mana per 5 sec.
                        else if (spellDesc.StartsWith("Restores "))
                        {
                            spellDesc = spellDesc.Substring("Restores ".Length);
                            spellDesc = spellDesc.Substring(0, spellDesc.IndexOf(" mana"));
                            stats.Mp5 += int.Parse(spellDesc);
                        }
                        else if (spellDesc.StartsWith("You gain an Electrical Charge each time you cause a damaging spell critical strike.  When you reach 3 Electrical Charges, they will release, firing a Lightning Bolt for 694 to 806 damage.  Electrical Charge cannot be gained more often than once every 2.5 sec."))
                        {
                            stats.LightningCapacitorProc = 1;
                        }
                        else if (spellDesc.StartsWith("You gain 25% more mana when you use a mana gem.  In addition, using a mana gem grants you 225 spell damage for 15 sec."))
                        {
                            stats.BonusManaGem += 0.25f;
                            stats.SpellDamageFor15SecOnManaGem += 225;
                        }
                        else if (spellDesc.StartsWith("Grants 170 increased spell damage for 10 sec when one of your spells is resisted."))
                        {
                            stats.SpellDamageFor10SecOnResist += 170;
                        }
                        else if (spellDesc.StartsWith("Your spell critical strikes have a 50% chance to grant you 145 spell haste rating for 5 sec."))
                        {
                            stats.SpellHasteFor5SecOnCrit_50 += 145;
                        }
                        else if (spellDesc.StartsWith("Your harmful spells have a chance to increase your spell haste rating by 320 for 6 secs."))
                        {
                            stats.SpellHasteFor6SecOnHit_10_45 += 320;
                        }
                        else if (spellDesc.StartsWith("Chance on spell critical hit to increase your spell damage and healing by 225 for 10 secs."))
                        {
                            stats.SpellDamageFor10SecOnCrit_20_45 += 225;
                        }
                        else if (spellDesc.StartsWith("Increases the effect that healing and mana potions have on the wearer by "))
                        {
                            spellDesc = spellDesc.Substring("Increases the effect that healing and mana potions have on the wearer by ".Length);
                            spellDesc = spellDesc.Replace(".", "").Replace(" ", "").Replace("%", "");
                            stats.BonusManaPotion += int.Parse(spellDesc) / 100f;
                            // TODO health potion effect
                        }
                        //Your spell critical strikes have a chance to increase your spell damage and healing by 190 for 15 sec.
                        else if (spellDesc.StartsWith("Your spell critical strikes have a chance to increase your spell damage and healing by "))
                        {
                            spellDesc = spellDesc.Substring("Your spell critical strikes have a chance to increase your spell damage and healing by ".Length);
                            float value = int.Parse(spellDesc.Substring(0, spellDesc.IndexOf(" for")));
                            spellDesc = spellDesc.Substring(spellDesc.IndexOf(" for") + " for ".Length);
                            int duration = int.Parse(spellDesc.Substring(0, spellDesc.IndexOf(" ")));

                            switch (duration)
                            {
                                case 15:
                                    if (name == "Sextant of Unstable Currents")
                                    {
                                        stats.SpellDamageFor15SecOnCrit_20_45 += value;
                                    }
                                    break;
                            }
                        }
                        else if (spellDesc.StartsWith("Gives a chance when your harmful spells land to increase the damage of your spells and effects by up to "))
                        {
                            // Gives a chance when your harmful spells land to increase the damage of your spells and effects by up to 130 for 10 sec.
                            spellDesc = spellDesc.Substring("Gives a chance when your harmful spells land to increase the damage of your spells and effects by up to ".Length);
                            float value = int.Parse(spellDesc.Substring(0, spellDesc.IndexOf(" for")));
                            spellDesc = spellDesc.Substring(spellDesc.IndexOf(" for") + " for ".Length);
                            int duration = int.Parse(spellDesc.Substring(0, spellDesc.IndexOf(" ")));

                            switch (duration)
                            {
                                case 10:
                                    if (name == "Robe of the Elder Scribes")
                                    {
                                        stats.SpellDamageFor10SecOnHit_10_45 += value;
                                    }
                                    break;
                            }
                        }
                        else if (spellDesc.StartsWith("Your offensive spells have a chance on hit to increase your spell damage by "))
                        {
                            // Your offensive spells have a chance on hit to increase your spell damage by 95 for 10 secs.
                            spellDesc = spellDesc.Substring("Your offensive spells have a chance on hit to increase your spell damage by ".Length);
                            float value = int.Parse(spellDesc.Substring(0, spellDesc.IndexOf(" for")));
                            spellDesc = spellDesc.Substring(spellDesc.IndexOf(" for") + " for ".Length);
                            int duration = int.Parse(spellDesc.Substring(0, spellDesc.IndexOf(" ")));

                            switch (duration)
                            {
                                case 10:
                                    if (name == "Band of the Eternal Sage")
                                    {
                                        // Fixed in 2.4 to be 10 sec instead of 15
                                        stats.SpellDamageFor10SecOnHit_10_45 += value;
                                    }
                                    break;
                            }
                        }
                        else if (spellDesc.StartsWith("Increases the damage of your Starfire spell by up to "))
                        {
                            spellDesc = spellDesc.Substring("Increases the damage of your Starfire spell by up to ".Length);
                            spellDesc = spellDesc.Replace(".", "");
                            stats.StarfireDmg += float.Parse(spellDesc, System.Globalization.CultureInfo.InvariantCulture);
                        }
                        else if (spellDesc.StartsWith("Increases the damage of your Moonfire spell by up to "))
                        {
                            spellDesc = spellDesc.Substring("Increases the damage of your Moonfire spell by up to ".Length);
                            spellDesc = spellDesc.Replace(".", "");
                            stats.MoonfireDmg += float.Parse(spellDesc, System.Globalization.CultureInfo.InvariantCulture);
                        }
                        else if (spellDesc.StartsWith("Increases the damage dealt by Wrath by "))
                        {
                            spellDesc = spellDesc.Substring("Increases the damage dealt by Wrath by ".Length);
                            spellDesc = spellDesc.Replace(".", "");
                            stats.WrathDmg += float.Parse(spellDesc, System.Globalization.CultureInfo.InvariantCulture);
                        }
                        else if (spellDesc.StartsWith("Increases the healing granted by the Tree of Life form"))
                        {
                            spellDesc = spellDesc.Substring(spellDesc.IndexOf("and adds "));
                            spellDesc = spellDesc.Substring(0, spellDesc.IndexOf(" spell critical strike rating"));
                            stats.IdolCritRating += float.Parse(spellDesc, System.Globalization.CultureInfo.InvariantCulture);
                        }
                        else if (spellDesc.StartsWith("Your Judgement of Command ability has a chance to grant "))
                        {
                            spellDesc = spellDesc.Substring("Your Judgement of Command ability has a chance to grant ".Length, 3);
                            stats.AttackPower += (10f / (9f / 0.4f)) * float.Parse(spellDesc, System.Globalization.CultureInfo.InvariantCulture);

                        }
                        else if (spellDesc.StartsWith("Causes your Judgement of Command, Judgement of Righteousness, Judgement of Blood, and Judgement of Vengeance to increase your Critical Strike rating by"))
                        {
                            spellDesc = spellDesc.Substring("Causes your Judgement of Command, Judgement of Righteousness, Judgement of Blood, and Judgement of Vengeance to increase your Critical Strike rating by ".Length, 2);
                            spellDesc = spellDesc.Replace(".", "");
                            stats.CritRating += float.Parse(spellDesc, System.Globalization.CultureInfo.InvariantCulture);
                        }
                        else if (spellDesc.StartsWith("Increases the damage dealt by your Crusader Strike ability by "))
                        {
                            spellDesc = spellDesc.Substring("Increases the damage dealt by your Cruasder Strike ability by ".Length);
                            spellDesc = spellDesc.Replace("%", "");
                            spellDesc = spellDesc.Replace(".", "");
                            stats.BonusCrusaderStrikeDamageMultiplier += float.Parse(spellDesc, System.Globalization.CultureInfo.InvariantCulture) / 100f;

                        }
						//else if (spellDesc.StartsWith("Increases the benefit your Flash of Light"))
						//{
						//    stats.HLBoL = 120;
						//    stats.FoLBoL = 60;
						//}
						//else if (spellDesc.StartsWith("Reduces the mana cost of Holy Light by"))
						//{
						//    stats.HLCost = 34;
						//}
						//else if (spellDesc.StartsWith("Increases healing done by Flash of Light by up to"))
						//{
						//    spellDesc = spellDesc.Substring("Increases healing done by Flash of Light by up to ".Length);
						//    spellDesc = spellDesc.Replace(".", "");
						//    stats.FoLHeal = float.Parse(spellDesc, System.Globalization.CultureInfo.InvariantCulture);
						//}
						//else if (spellDesc.StartsWith("Increases healing done by Holy Light by up to"))
						//{
						//    spellDesc = spellDesc.Substring("Increases healing done by Holy Light by up to ".Length);
						//    spellDesc = spellDesc.Replace(".", "");
						//    stats.HLHeal = float.Parse(spellDesc, System.Globalization.CultureInfo.InvariantCulture);
						//}
                    }
				}

				XmlNodeList socketNodes = docItem.SelectNodes("page/itemTooltips/itemTooltip/socketData/socket");
				if (socketNodes.Count > 0) sockets.Color1String = socketNodes[0].Attributes["color"].Value;
				if (socketNodes.Count > 1) sockets.Color2String = socketNodes[1].Attributes["color"].Value;
				if (socketNodes.Count > 2) sockets.Color3String = socketNodes[2].Attributes["color"].Value;
				string socketBonus = string.Empty;
				foreach (XmlNode node in docItem.SelectNodes("page/itemTooltips/itemTooltip/socketData/socketMatchEnchant")) { socketBonus = node.InnerText.Trim('+'); }
				if (!string.IsNullOrEmpty(socketBonus))
				{
					try
					{
						int socketBonusValue = int.Parse(socketBonus.Substring(0, socketBonus.IndexOf(' ')));
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
							case "Armor Penetration":
								sockets.Stats.ArmorPenetration = socketBonusValue;
								break;
							case "Strength":
								sockets.Stats.Strength = socketBonusValue;
								break;
                            case "Healing":
                                sockets.Stats.Healing = socketBonusValue;
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
                            case "Spell Damage":
                                sockets.Stats.SpellDamageRating = socketBonusValue;
                                break;
                            case "Spell Hit Rating":
                                sockets.Stats.SpellHitRating = socketBonusValue;
                                break;
                            case "Intellect":
                                sockets.Stats.Intellect = socketBonusValue;
                                break;
							case "Spell Crit":
							case "Spell Crit Rating":
							case "Spell Critical":
							case "Spell Critical Rating":
                            case "Spell Critical Strike Rating":
								sockets.Stats.SpellCritRating = socketBonusValue;
                                break;
                            case "Spell Haste Rating":
                                sockets.Stats.SpellHasteRating = socketBonusValue;
                                break;
                            case "Spirit":
                                sockets.Stats.Spirit = socketBonusValue;
                                break;
                            case "Mana every 5 seconds":
                            case "Mana ever 5 Sec":
                            case "mana per 5 sec":
                            case "mana per 5 sec.":
                            case "Mana per 5 Seconds":
                                sockets.Stats.Mp5 = socketBonusValue;
                                break;
						}
					}
					catch { }
				}
				foreach (XmlNode nodeGemProperties in docItem.SelectNodes("page/itemTooltips/itemTooltip/gemProperties"))
				{
					string[] gemBonuses = nodeGemProperties.InnerText.Split(new string[] { " and ", " & ", ", " }, StringSplitOptions.None);
					foreach (string gemBonus in gemBonuses)
					{
                        if (gemBonus == "Spell Damage +6")
                        {
                            stats.SpellDamageRating = 6.0f;
                        }
                        else if (gemBonus == "Stamina +6")
                        {
                            stats.Stamina = 6.0f;
                        }
                        else if (gemBonus == "Chance to restore mana on spellcast")
                        {
                            stats.ManaRestorePerCast = 15; // IED
                        }
                        else if (gemBonus == "Chance on spellcast - next spell cast in half time" || gemBonus == "Chance to Increase Spell Cast Speed")
                        {
                            stats.SpellHasteFor6SecOnCast_15_45 = 320; // MSD changed in 2.4
                        }
                        try
						{
							int gemBonusValue = int.Parse(gemBonus.Substring(0, gemBonus.IndexOf(' ')).Trim('+').Trim('%'));
							switch (gemBonus.Substring(gemBonus.IndexOf(' ') + 1))
							{
                                case "Resist All":
                                    stats.AllResist = gemBonusValue;
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
								case "Defense Rating":
									stats.DefenseRating = gemBonusValue;
                                    break;
                                case "Healing":
                                case "Healing +4 Spell Damage":
                                case "Healing +3 Spell Damage":
                                case "Healing +2 Spell Damage":
                                    stats.Healing = gemBonusValue;
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
								case "Armor Penetration":
									stats.ArmorPenetration = gemBonusValue;
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
                                    stats.SpellHitRating = gemBonusValue;
                                    break;
                                case "Spell Damage":
                                    stats.SpellDamageRating = gemBonusValue;
                                    break;
								case "Spell Crit":
								case "Spell Crit Rating":
								case "Spell Critical":
                                case "Spell Critical Rating":
                                    stats.SpellCritRating = gemBonusValue;
                                    break;
                                case "Mana every 5 seconds" :
                                case "Mana ever 5 Sec" :
                                case "mana per 5 sec":
                                case "mana per 5 sec.":
                                case "Mana per 5 Seconds" :
                                    stats.Mp5 = gemBonusValue;
                                    break;
                                case "Intellect" :
                                    stats.Intellect = gemBonusValue;
                                    break; 
                                case "Spirit" :
                                    stats.Spirit = gemBonusValue;
                                    break;
                            }
						}
						catch { }
					}
				}
				string desc = string.Empty;
				foreach (XmlNode node in docItem.SelectNodes("page/itemTooltips/itemTooltip/desc")) { desc = node.InnerText; }
				if (desc.Contains("Matches a "))
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
				Item item = new Item(name, quality, type, int.Parse(id), iconPath, slot, setName, stats, sockets, gem1Id, gem2Id, gem3Id, minDamage, maxDamage, damageType, speed, string.Join("|", requiredClasses.ToArray()));
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
					System.Windows.Forms.MessageBox.Show(string.Format("Rawr encountered an error getting Item " +
					"from Armory: {0}. If you still encounter this error, please copy and" +
					" paste this into an e-mail to cnervig@hotmail.com. Thanks!\r\n\r\nResponse: {1}\r\n\r\n\r\n{2}\r\n\r\n{3}",
					gemmedId, docItem.OuterXml, ex.Message, ex.StackTrace));
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

				case "Crowssbow":
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
				Item itemToUpgrade = character[slot];
				if (itemToUpgrade != null)
				{
					WebRequestWrapper wrw = new WebRequestWrapper();
					docUpgradeSearch = wrw.DownloadUpgrades(character.Name, character.Region,character.Realm,itemToUpgrade.Id);

					ComparisonCalculationBase currentCalculation = Calculations.GetItemCalculations(itemToUpgrade, character, slot);

					foreach (XmlNode node in docUpgradeSearch.SelectNodes("page/armorySearch/searchResults/items/item"))
					{
						string id = node.Attributes["id"].Value + ".0.0.0";
						Item idealItem = GetItem(id, "Loading Upgrades");
						idealItem._gem1Id = idealGems[idealItem.Sockets.Color1];
						idealItem._gem2Id = idealGems[idealItem.Sockets.Color2];
						idealItem._gem3Id = idealGems[idealItem.Sockets.Color3];

						if (!ItemCache.Items.ContainsKey(idealItem.GemmedId))
						{
							ComparisonCalculationBase upgradeCalculation = Calculations.GetItemCalculations(idealItem, character, slot);

							if (upgradeCalculation.OverallPoints > (currentCalculation.OverallPoints * .8f))
							{
								ItemCache.AddItem(idealItem);
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				if (docUpgradeSearch == null || docUpgradeSearch.InnerXml.Length == 0)
				{
					System.Windows.Forms.MessageBox.Show(string.Format("Rawr encountered an error getting Upgrades " +
					"from Armory: {0}. Rawr recieved no response to its query for upgrade" +
					" data, so please check to make sure that no firewall " +
					"or proxy software is blocking Rawr. If you still encounter this error, please copy and" +
					" paste this into an e-mail to cnervig@hotmail.com. Thanks!\r\n\r\nResponse: {1}\r\n\r\n\r\n{2}\r\n\r\n{3}",
					slot.ToString(), "null", ex.Message, ex.StackTrace));
				}
				else
				{
					System.Windows.Forms.MessageBox.Show(string.Format("Rawr encountered an error getting Upgrades " +
					"from Armory: {0}. If you still encounter this error, please copy and" +
					" paste this into an e-mail to cnervig@hotmail.com. Thanks!\r\n\r\nResponse: {1}\r\n\r\n\r\n{2}\r\n\r\n{3}",
					slot.ToString(), docUpgradeSearch.OuterXml, ex.Message, ex.StackTrace));
				}
			}
		}
	}
}
