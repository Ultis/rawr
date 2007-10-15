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
		private static bool _proxyRequiresAuthentication = false;
		private static XmlDocument DownloadXml(string url)
		{
			try
			{
				HttpWebRequest request = HttpWebRequest.Create(url) as HttpWebRequest;
				if (_proxyRequiresAuthentication)
				{
					request.Proxy = HttpWebRequest.DefaultWebProxy;
					request.Proxy.Credentials = CredentialCache.DefaultCredentials;
				}
				request.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 6.0; en-US; rv:1.8.1.4) Gecko/20070515 Firefox/2.0.0.4";
				string xml = new StreamReader(request.GetResponse().GetResponseStream()).ReadToEnd();
				XmlDocument doc = new XmlDocument();
				doc.LoadXml(xml);
				return doc;
			}
			catch (Exception ex)
			{
				if (!_proxyRequiresAuthentication && ex.Message.Contains("Proxy Authentication Required"))
				{
					_proxyRequiresAuthentication = true;
					return DownloadXml(url);
				}
			}
			return null;
		}

		public static Character GetCharacter(Character.CharacterRegion region, string realm, string name)
		{
			try
			{
				Log.Write("Getting Character from Armory: " + name + "@" + region.ToString() + "-" + realm);
				//Tell me how he died.
				string armoryDomain = region == Character.CharacterRegion.US ? "www" : "eu";
				string characterSheetPath = string.Format("http://{0}.wowarmory.com/character-sheet.xml?r={1}&n={2}",
					armoryDomain, realm, name);
				XmlDocument docCharacter = DownloadXml(characterSheetPath);

				Character.CharacterRace race = docCharacter.SelectSingleNode("page/characterInfo/character").Attributes["race"].Value == "Night Elf" ?
					Character.CharacterRace.NightElf : Character.CharacterRace.Tauren;
				Dictionary<Character.CharacterSlot, string> items = new Dictionary<Character.CharacterSlot, string>();
				Dictionary<Character.CharacterSlot, int> enchants = new Dictionary<Character.CharacterSlot, int>();

				foreach (XmlNode itemNode in docCharacter.SelectNodes("page/characterInfo/characterTab/items/item"))
				{
					int slot = int.Parse(itemNode.Attributes["slot"].Value);
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
					items.ContainsKey(Character.CharacterSlot.Weapon) ? items[Character.CharacterSlot.Weapon] : null,
					items.ContainsKey(Character.CharacterSlot.Idol) ? items[Character.CharacterSlot.Idol] : null,
					enchants.ContainsKey(Character.CharacterSlot.Head) ? enchants[Character.CharacterSlot.Head] : 0,
					enchants.ContainsKey(Character.CharacterSlot.Shoulders) ? enchants[Character.CharacterSlot.Shoulders] : 0,
					enchants.ContainsKey(Character.CharacterSlot.Back) ? enchants[Character.CharacterSlot.Back] : 0,
					enchants.ContainsKey(Character.CharacterSlot.Chest) ? enchants[Character.CharacterSlot.Chest] : 0,
					enchants.ContainsKey(Character.CharacterSlot.Wrist) ? enchants[Character.CharacterSlot.Wrist] : 0,
					enchants.ContainsKey(Character.CharacterSlot.Hands) ? enchants[Character.CharacterSlot.Hands] : 0,
					enchants.ContainsKey(Character.CharacterSlot.Legs) ? enchants[Character.CharacterSlot.Legs] : 0,
					enchants.ContainsKey(Character.CharacterSlot.Feet) ? enchants[Character.CharacterSlot.Feet] : 0,
					enchants.ContainsKey(Character.CharacterSlot.Weapon) ? enchants[Character.CharacterSlot.Weapon] : 0
					);

				//I will tell you how he lived.
				return character;
			}
			catch (Exception ex)
			{
				System.Windows.Forms.MessageBox.Show("Rawr encountered an error getting Character from Armory: " + name + "@" + region.ToString() + "-" + realm + ". Please check to make sure you've spelled the character name and realm exactly right, and chosen the correct Region. If you still encounter this error, please copy and paste this into an e-mail to cnervig@hotmail.com. Thanks!\r\n\r\n\r\n" + ex.Message + "\r\n\r\n" + ex.StackTrace);
				return null;
			}
		}

		public static Item GetItem(string gemmedId, string logReason)
		{
			//Just close your eyes
			try
			{
				string id = gemmedId.Split('.')[0];
				Log.Write("Getting Item from Armory: " + id + "   Reason: " + logReason);

				string itemTooltipPath = string.Format("http://armory.worldofwarcraft.com/item-tooltip.xml?i={0}", id);
				XmlDocument docItem = DownloadXml(itemTooltipPath);

				string name = string.Empty;
				string iconPath = string.Empty;
				Item.ItemSlot slot = Item.ItemSlot.None;
				Stats stats = new Stats();
				Sockets sockets = new Sockets();

				foreach (XmlNode node in docItem.SelectNodes("page/itemTooltips/itemTooltip/name")) { name = node.InnerText; }
				foreach (XmlNode node in docItem.SelectNodes("page/itemTooltips/itemTooltip/icon")) { iconPath = node.InnerText; }
				foreach (XmlNode node in docItem.SelectNodes("page/itemTooltips/itemTooltip/equipData/inventoryType")) { slot = (Item.ItemSlot)int.Parse(node.InnerText); }

				foreach (XmlNode node in docItem.SelectNodes("page/itemTooltips/itemTooltip/bonusAgility")) { stats.Agility = int.Parse(node.InnerText); }
				foreach (XmlNode node in docItem.SelectNodes("page/itemTooltips/itemTooltip/armor")) { stats.Armor = int.Parse(node.InnerText); }
				foreach (XmlNode node in docItem.SelectNodes("page/itemTooltips/itemTooltip/bonusDefenseSkillRating")) { stats.DefenseRating = int.Parse(node.InnerText); }
				foreach (XmlNode node in docItem.SelectNodes("page/itemTooltips/itemTooltip/bonusDodgeRating")) { stats.DodgeRating = int.Parse(node.InnerText); }
				foreach (XmlNode node in docItem.SelectNodes("page/itemTooltips/itemTooltip/bonusResilienceRating")) { stats.Resilience = int.Parse(node.InnerText); }
				foreach (XmlNode node in docItem.SelectNodes("page/itemTooltips/itemTooltip/bonusStamina")) { stats.Stamina = int.Parse(node.InnerText); }

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
							case "Resilience":
							case "Resilience Rating":
								sockets.Stats.Resilience = socketBonusValue;
								break;
						}
					}
					catch { }
				}
				foreach (XmlNode nodeGemProperties in docItem.SelectNodes("page/itemTooltips/itemTooltip/gemProperties"))
				{
					string[] gemBonuses = nodeGemProperties.InnerText.Split(new string[] { " and ", " & " }, StringSplitOptions.None);
					foreach (string gemBonus in gemBonuses)
					{
						try
						{
							int gemBonusValue = int.Parse(gemBonus.Substring(0, gemBonus.IndexOf(' ')).Trim('+'));
							switch (gemBonus.Substring(gemBonus.IndexOf(' ') + 1))
							{
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
								case "Resilience":
								case "Resilience Rating":
									stats.Resilience = gemBonusValue;
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
				Item item = new Item(name, int.Parse(id), iconPath, slot, stats, sockets, gem1Id, gem2Id, gem3Id);
				//And all will be revealed
				return item;
			}
			catch (Exception ex)
			{
				System.Windows.Forms.MessageBox.Show("Rawr encountered an error getting Item from Armory: " + gemmedId + ". Please copy and paste this into an e-mail to cnervig@hotmail.com. Thanks!\r\n\r\n\r\n" + ex.Message + "\r\n\r\n" + ex.StackTrace);
				return null;
			}
		}

		public static void LoadUpgradesFromArmory(Character character)
		{
			if (!string.IsNullOrEmpty(character.Realm) && !string.IsNullOrEmpty(character.Name))
			{
				List<ItemCalculation> gemCalculations = new List<ItemCalculation>();
				foreach (Item item in ItemCache.GetItemsArray())
				{
					if (item.Slot == Item.ItemSlot.Blue || item.Slot == Item.ItemSlot.Green || item.Slot == Item.ItemSlot.Meta
						 || item.Slot == Item.ItemSlot.Orange || item.Slot == Item.ItemSlot.Prismatic || item.Slot == Item.ItemSlot.Purple
						 || item.Slot == Item.ItemSlot.Red || item.Slot == Item.ItemSlot.Yellow)
					{
						gemCalculations.Add(Calculations.GetItemCalculations(item, character, item.Slot == Item.ItemSlot.Meta ? Character.CharacterSlot.Metas : Character.CharacterSlot.Gems));
					}
				}

				ItemCalculation idealRed = null, idealBlue = null, idealYellow = null, idealMeta = null;
				foreach (ItemCalculation calc in gemCalculations)
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
				LoadUpgradesForSlot(character, Character.CharacterSlot.Wrist, idealGems);
				LoadUpgradesForSlot(character, Character.CharacterSlot.Legs, idealGems);
				LoadUpgradesForSlot(character, Character.CharacterSlot.Feet, idealGems);
				LoadUpgradesForSlot(character, Character.CharacterSlot.Finger1, idealGems);
				LoadUpgradesForSlot(character, Character.CharacterSlot.Finger2, idealGems);
				LoadUpgradesForSlot(character, Character.CharacterSlot.Trinket1, idealGems);
				LoadUpgradesForSlot(character, Character.CharacterSlot.Trinket2, idealGems);
				LoadUpgradesForSlot(character, Character.CharacterSlot.Weapon, idealGems);
			}
			else
			{
				System.Windows.Forms.MessageBox.Show("This feature requires your character name, realm, and region. Please fill these fields out, and try again.");
			}
		}

		private static void LoadUpgradesForSlot(Character character, Character.CharacterSlot slot, Dictionary<Item.ItemSlot, int> idealGems)
		{
			Item itemToUpgrade = character[slot];
			if (itemToUpgrade != null)
			{
				string armoryDomain = character.Region == Character.CharacterRegion.US ? "www" : "eu";
				string upgradeSearchPath = string.Format("http://{0}.wowarmory.com/search.xml?searchType=items&pr={1}&pn={2}&pi={3}",
					armoryDomain, character.Realm, character.Name, itemToUpgrade.Id);
				XmlDocument docUpgradeSearch = DownloadXml(upgradeSearchPath);

				ItemCalculation currentCalculation = Calculations.GetItemCalculations(itemToUpgrade, character, slot);

				foreach (XmlNode node in docUpgradeSearch.SelectNodes("page/armorySearch/searchResults/items/item"))
				{
					string id = node.Attributes["id"].Value + ".0.0.0";
					Item idealItem = GetItem(id, "Loading Upgrades");
					idealItem._gem1Id = idealGems[idealItem.Sockets.Color1];
					idealItem._gem2Id = idealGems[idealItem.Sockets.Color2];
					idealItem._gem3Id = idealGems[idealItem.Sockets.Color3];

					if (!ItemCache.Items.ContainsKey(idealItem.GemmedId))
					{
						ItemCalculation upgradeCalculation = Calculations.GetItemCalculations(idealItem, character, slot);

						if (upgradeCalculation.OverallPoints > (currentCalculation.OverallPoints * .8f))// ||
						//upgradeCalculation.MitigationPoints > (currentCalculation.MitigationPoints * .8f) ||
						//upgradeCalculation.SurvivalPoints > (currentCalculation.SurvivalPoints * .8f) )
						{
							ItemCache.AddItem(idealItem);
						}
					}
				}
				itemToUpgrade.ToString();
			}
		}
	}
}
