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
			//can we get away...
			{
				//far away...
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
				System.Windows.Forms.Application.DoEvents();
				return doc;
			}
			catch (Exception ex)
			//say goodnight... to gravity...
			{
				//the passing stars light the way...
				if (!_proxyRequiresAuthentication && ex.Message.Contains("Proxy Authentication Required"))
				{
					_proxyRequiresAuthentication = true;
					return DownloadXml(url);
				}
			}
			return null; //lets go for a ride...
		}

		public static Character GetCharacter(Character.CharacterRegion region, string realm, string name)
		{
			XmlDocument docCharacter = null;
			try
			{
				Log.Write("Getting Character from Armory: " + name + "@" + region.ToString() + "-" + realm);
				//Tell me how he died.
				string armoryDomain = region == Character.CharacterRegion.US ? "www" : "eu";
				string characterSheetPath = string.Format("http://{0}.wowarmory.com/character-sheet.xml?r={1}&n={2}",
					armoryDomain, realm, name);
				docCharacter = DownloadXml(characterSheetPath);

                Character.CharacterRace race = Character.CharacterRace.Orc;
                switch (docCharacter.SelectSingleNode("page/characterInfo/character").Attributes["race"].Value)
                {
                    case "Night Elf":
                        race = Character.CharacterRace.NightElf;
                        break;
                    case "Tauren":
                        race = Character.CharacterRace.Tauren;
                        break;
                    case "Human":
                        race = Character.CharacterRace.Human;
                        break;
                    case "Orc":
                        race = Character.CharacterRace.Orc;
                        break;
                    case "Gnome":
                        race = Character.CharacterRace.Gnome;
                        break;
                    case "Troll":
                        race = Character.CharacterRace.Troll;
                        break;
                    case "Dwarf":
                        race = Character.CharacterRace.Dwarf;
                        break;
                    case "Undead":
                        race = Character.CharacterRace.Undead;
                        break;
                    case "Dranei":
                        race = Character.CharacterRace.Draenei;
                        break;
                    case "Blood Elf":
                        race = Character.CharacterRace.BloodElf;
                        break;
                }
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
				int retry = 0;
				while (retry < 3)
				{
					try
					{
						string id = gemmedId.Split('.')[0];
						Log.Write("Getting Item from Armory: " + id + "   Reason: " + logReason);

						string itemTooltipPath = string.Format("http://www.wowarmory.com/item-tooltip.xml?i={0}", id);
						docItem = DownloadXml(itemTooltipPath);

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
						float speed = 0f;

						foreach (XmlNode node in docItem.SelectNodes("page/itemTooltips/itemTooltip/name")) { name = node.InnerText; }
						foreach (XmlNode node in docItem.SelectNodes("page/itemTooltips/itemTooltip/icon")) { iconPath = node.InnerText; }
						foreach (XmlNode node in docItem.SelectNodes("page/itemTooltips/itemTooltip/overallQualityId")) { quality = (Item.ItemQuality)Enum.Parse(typeof(Item.ItemQuality), node.InnerText); }
						foreach (XmlNode node in docItem.SelectNodes("page/itemTooltips/itemTooltip/classId")) { classId = int.Parse(node.InnerText); }
						foreach (XmlNode node in docItem.SelectNodes("page/itemTooltips/itemTooltip/equipData/inventoryType")) { inventoryType = int.Parse(node.InnerText); }
						foreach (XmlNode node in docItem.SelectNodes("page/itemTooltips/itemTooltip/equipData/subclassName")) { subclassName = node.InnerText; }
						foreach (XmlNode node in docItem.SelectNodes("page/itemTooltips/itemTooltip/damageData/damage/min")) { minDamage = int.Parse(node.InnerText); }
						foreach (XmlNode node in docItem.SelectNodes("page/itemTooltips/itemTooltip/damageData/damage/max")) { maxDamage = int.Parse(node.InnerText); }
						foreach (XmlNode node in docItem.SelectNodes("page/itemTooltips/itemTooltip/damageData/speed")) { speed = float.Parse(node.InnerText); }
						foreach (XmlNode node in docItem.SelectNodes("page/itemTooltips/itemTooltip/setData/name")) { setName = node.InnerText; }

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
							}

							if (isEquip)
							{
								if (spellDesc.StartsWith("Each time you deal melee or ranged damage to an opponent, you gain 6 attack power"))
									stats.AttackPower += 120; //Crusade = 120ap
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
                                    case "Spell Crit Rating":
                                        sockets.Stats.SpellCritRating = socketBonusValue;
                                        break;
                                    case "Spell Haste Rating":
                                        sockets.Stats.SpellHasteRating = socketBonusValue;
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
									int gemBonusValue = int.Parse(gemBonus.Substring(0, gemBonus.IndexOf(' ')).Trim('+').Trim('%'));
									switch (gemBonus.Substring(gemBonus.IndexOf(' ') + 1))
									{
                                        case "Resist All":
                                            stats.AllResist = gemBonusValue;
                                            break;
										case "Increased Critical Damage":
											stats.BonusCritMultiplier = (float)gemBonusValue / 100f;
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
                                        case "Spell Critical Rating":
                                            stats.SpellCritRating = gemBonusValue;
                                            break;
                                        case "Mana every 5 seconds" :
                                        case "Mana ever 5 Sec" :
                                        case "mana per 5 sec" :
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
						Item item = new Item(name, quality, type, int.Parse(id), iconPath, slot, setName, stats, sockets, gem1Id, gem2Id, gem3Id, minDamage, maxDamage, speed);
						retry = 3;
						return item;
					}
					catch
					{
						retry++;
						if (retry == 3) throw;
					}
					//And all will be revealed
				}
				return null;
			}
			catch (Exception ex)
			{
				if (docItem == null || docItem.InnerXml.Length == 0)
				{
					System.Windows.Forms.MessageBox.Show(string.Format("Rawr encountered an error getting Item " +
					"from Armory: {0}. Rawr recieved no response to its query for item" +
					" data, so please check to make sure that no firewall " +
					"or proxy software is blocking Rawr. If you still encounter this error, please copy and" +
					" paste this into an e-mail to cnervig@hotmail.com. Thanks!\r\n\r\nResponse: {1}\r\n\r\n\r\n{2}\r\n\r\n{3}",
					gemmedId, "null", ex.Message, ex.StackTrace));
				}
				else
				{
					System.Windows.Forms.MessageBox.Show(string.Format("Rawr encountered an error getting Item " +
					"from Armory: {0}. If you still encounter this error, please copy and" +
					" paste this into an e-mail to cnervig@hotmail.com. Thanks!\r\n\r\nResponse: {1}\r\n\r\n\r\n{2}\r\n\r\n{3}",
					gemmedId, docItem.OuterXml, ex.Message, ex.StackTrace));
				}
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
					string armoryDomain = character.Region == Character.CharacterRegion.US ? "www" : "eu";
					string upgradeSearchPath = string.Format("http://{0}.wowarmory.com/search.xml?searchType=items&pr={1}&pn={2}&pi={3}",
						armoryDomain, character.Realm, character.Name, itemToUpgrade.Id);
					docUpgradeSearch = DownloadXml(upgradeSearchPath);

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
