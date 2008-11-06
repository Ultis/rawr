using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Xml;
using System.Text.RegularExpressions;

namespace Rawr
{
	public static class Wowhead
	{
		//32824,Tigole's Trashbringer,6,"id:32824,name:'1Tigole\'s Trashbringer',level:1,reqlevel:1,dps:9.0,speed:3.00,slot:17,classs:2,subclass:8","slotbak:17,displayid:23875,reqlevel:1,maxcount:1,dmgmin1:24,dmgmax1:30,dmgtype1:0,speed:3.00,dps:9.0"
		public static void ProcessFile()
		{
			if (System.IO.File.Exists("C:\\wotlkitems.csv"))
			{
				string[] lines = System.IO.File.ReadAllLines("C:\\wotlkitems.csv");
				List<Item> items = new List<Item>();
				foreach (string line in lines)
					if (line != lines[0])
					{
						Item item = ProcessItem(line);
						if (item != null) items.Add(item);
					}

				"".ToString();
			}
		}

		public static Item ProcessItem(string itemData)
		{
			string data = itemData.Replace("[,{","[{");
			string id = data.Substring(0, data.IndexOf(','));
			data = data.Substring(data.IndexOf(',') + 1);
			string name = string.Empty;
			if (data.StartsWith("\""))
			{
				name = data.Substring(1, data.IndexOf("\",") - 1);
			}
			else
			{
				name = data.Substring(0, data.IndexOf(','));
			}
			data = data.Replace(name, "").TrimStart('"').TrimStart(',');
			string quality = data.Substring(0, data.IndexOf(','));
			data = data.Substring(data.IndexOf(',') + 2);

			string json1 = data.Substring(0, data.IndexOf("\",")).Replace(",subclass:",".");
			data = data.Substring(data.IndexOf("\",") + 2);
			string json2 = data.Trim('"');//string.IsNullOrEmpty(data) ? string.Empty : data.Substring(1, data.IndexOf('"'));

			string source = string.Empty;
			if (json1.Contains("source:["))
			{
				source = json1.Substring(json1.IndexOf("source:[") + "source:[".Length);
				source = source.Substring(0, source.IndexOf("]"));
				json1 = json1.Replace(string.Format("source:[{0}]", source), "source:[SOURCE]");
			}

			string sourcemore = string.Empty;
			if (json1.Contains("sourcemore:[{"))
			{
				sourcemore = json1.Substring(json1.IndexOf("sourcemore:[{") + "sourcemore:[{".Length);
				sourcemore = sourcemore.Substring(0, sourcemore.IndexOf("}]"));
				json1 = json1.Replace(sourcemore, "SOURCEMORE");
			}

			Item item = new Item() { Stats = new Stats(), Sockets = new Sockets(), Name = name };

			foreach (string keyval in (json1 + "," + json2).Split(','))
			{
				if (!string.IsNullOrEmpty(keyval))
				{
					string[] keyvalsplit = keyval.Split(':');
					string key = keyvalsplit[0];
					string val = keyvalsplit[1];
					if (ProcessKeyValue(item, key, val)) return null;
				}
			}
			if (!string.IsNullOrEmpty(source)) ProcessKeyValue(item, "source", source);
			if (!string.IsNullOrEmpty(sourcemore))
			{
				string n = string.Empty;
				if (sourcemore.Contains("n:'"))
				{
					n = sourcemore.Substring(sourcemore.IndexOf("n:'") + "n:'".Length);
					n = n.Substring(0, n.IndexOf("'"));
					if (!string.IsNullOrEmpty(n))
						sourcemore = sourcemore.Replace(n, "N");
				}

				foreach (string keyval in sourcemore.Replace("},", ",").Replace(",{", ",").Split(','))
				{
					if (!string.IsNullOrEmpty(keyval))
					{
						string[] keyvalsplit = keyval.Split(':');
						string key = keyvalsplit[0];
						string val = keyvalsplit[1];
						ProcessKeyValue(item, key, val);
					}
				}
				if (!string.IsNullOrEmpty(n)) ProcessKeyValue(item, "n", n);
			}

			return item;
		}


		private static SortedDictionary<int, List<Item>> _slotItems = new SortedDictionary<int, List<Item>>();
		private static SortedDictionary<string, List<Item>> _classsItems = new SortedDictionary<string, List<Item>>();
		private static SortedDictionary<int, List<Item>> _subclassItems = new SortedDictionary<int, List<Item>>();
		private static List<string> _unhandledKeys = new List<string>();
		private static bool ProcessKeyValue(Item item, string key, string value)
		{
			switch (key)
			{
				case "id":
					item.Id = int.Parse(value);
					break;

				case "name": //Item names are parsed out of the main data, not the json
				case "level": //Rawr doesn't handle item levels
				case "slotbak": //A couple slots actually have two possible slots... ie vests and robes both fit in chest. slotbak distinguishes vests from robes. We don't care for Rawr, so ignored.
				case "subclass": //subclass is combined with class
				case "subsubclass": //Only used for Battle vs Guardian Elixirs
				case "buyprice": //Rawr doesn't care about buy...
				case "sellprice": //...and sell prices
				case "reqlevel": //Rawr assumes that you meet all level requirements
				case "dps": //Rawr calculates weapon dps based on min/max and speed
				case "maxcount": //Rawr doesn't deal with stack sizes
				case "cooldown": //Not handled yet
				case "dura": //durability isn't handled
					break;

				case "slot":
					int slot = int.Parse(value);
					if (slot == 0) return true; //Not Equippable
					item.Slot = GetItemSlot(slot);
					break;

				case "classs":
					if (value.StartsWith("1.") || value.StartsWith("12.")) return true; //Container and Quest
					item.Type = GetItemType(value);
					break;

				case "source":
					break;

				case "sourcemore":
					break;

				case "speed":
					item.Speed = float.Parse(value);
					break;

				case "displayid":
					break;

				case "dmgmin1":
					item.MinDamage += int.Parse(value);
					break;

				case "dmgmax1":
					item.MaxDamage += int.Parse(value);
					break;

				case "dmgtype1":
					item.DamageType = (Item.ItemDamageType)int.Parse(value);
					break;

				case "armor":
				case "armorbonus":
					item.Stats.Armor += int.Parse(value);
					break;

				case "healthrgn":
					item.Stats.Hp5 += float.Parse(value);
					break;

				case "manargn":
					item.Stats.Mp5 += float.Parse(value);
					break;

				case "health":
					item.Stats.Health += float.Parse(value);
					break;

				case "agi":
					item.Stats.Agility += float.Parse(value);
					break;

				case "int":
					item.Stats.Intellect += float.Parse(value);
					break;

				case "spi":
					item.Stats.Spirit += float.Parse(value);
					break;

				case "sta":
					item.Stats.Stamina += float.Parse(value);
					break;

				case "str":
					item.Stats.Strength += float.Parse(value);
					break;

				case "races":
					break;

				case "mlehastertng":
				case "rgdhastertng":
				case "splhastertng":
					item.Stats.HasteRating = float.Parse(value);
					break;

				case "skill":
					break;

				case "reqskill":
					break;

				case "reqskillrank":
					break;

				case "splheal":
				case "spldmg":
					item.Stats.SpellPower = float.Parse(value);
					break;

				case "mlecritstrkrtng":
				case "rgdcritstrkrtng":
				case "splcritstrkrtng":
					item.Stats.CritRating = float.Parse(value);
					break;

				case "holres":
					break;

				case "firres":
					item.Stats.FireResistance = float.Parse(value);
					break;

				case "natres":
					item.Stats.NatureResistance = float.Parse(value);
					break;

				case "frores":
					item.Stats.FrostResistance = float.Parse(value);
					break;

				case "shares":
					item.Stats.ShadowResistance = float.Parse(value);
					break;

				case "arcres":
					item.Stats.ArcaneResistance = float.Parse(value);
					break;

				case "mlehitrtng":
				case "rgdhitrtng":
				case "splhitrtng":
					item.Stats.HitRating = float.Parse(value);
					break;

				case "mleatkpwr":
				case "rgdatkpwr":
				case "feratkpwr":
					item.Stats.AttackPower += float.Parse(value);
					break;

				case "reqrep":
					break;

				case "block":
					break;

				case "exprtng":
					break;

				case "defrtng":
					break;

				case "dodgertng":
					break;

				case "blockrtng":
					break;

				case "socket1":
					break;

				case "socket2":
					break;

				case "socket3":
					break;

				case "socketbonus":
					break;

				case "nsockets":
					break;

				case "parryrtng":
					item.Stats.ParryRating += int.Parse(value);
					break;

				case "classes":
					break;

				case "itemset":
					break;

				case "resirtng":
					item.Stats.Resilience += int.Parse(value);
					break;

				case "armorpen":
					item.Stats.ArmorPenetrationRating = int.Parse(value);
					break;

				case "nslots":
					break;

				case "reqfaction":
					break;

				case "splpen":
					item.Stats.SpellPenetration = int.Parse(value);
					break;

				case "mana":
					item.Stats.Mana = int.Parse(value);
					break;

				case "dmg":
					item.Stats.WeaponDamage = int.Parse(value);
					break;

				case "frospldmg":
					item.Stats.SpellFrostDamageRating = int.Parse(value);
					break;

				case "shaspldmg":
					item.Stats.SpellShadowDamageRating = int.Parse(value);
					break;

				case "firspldmg":
					item.Stats.SpellFireDamageRating = int.Parse(value);
					break;

				case "arcspldmg":
					item.Stats.SpellArcaneDamageRating = int.Parse(value);
					break;

				case "avgmoney":
					break;

				case "glyph":
					break;

				case "reqspell":
					break;



				//sourcemore keys
				case "t":
					break;

				case "ti":
					break;

				case "n":
					break;

				case "z":
					break;

				case "c":
					break;

				case "c2":
					break;

				case "dd":
					break;

				case "s":
					break;

				case "q":
					break;

				case "p":
					break;


				default:
					if (!_unhandledKeys.Contains(key))
						_unhandledKeys.Add(key);
					break;
			}
			return false;
		}




		private static Item.ItemType GetItemType(string classSubclass)
		{
			switch (classSubclass)
			{
				case "4.1":
					return Item.ItemType.Cloth;

				case "4.2":
					return Item.ItemType.Leather;

				case "4.3":
					return Item.ItemType.Mail;

				case "4.4":
					return Item.ItemType.Plate;

				case "2.15":
					return Item.ItemType.Dagger;

				case "2.13":
					return Item.ItemType.FistWeapon;

				case "2.1":
					return Item.ItemType.TwoHandAxe;
				
				case "2.0":
					return Item.ItemType.OneHandAxe;

				case "2.5":
					return Item.ItemType.TwoHandMace;

				case "2.4":
					return Item.ItemType.OneHandMace;

				case "2.8":
					return Item.ItemType.TwoHandSword;
	
				case "2.7":
					return Item.ItemType.OneHandSword;

				case "2.6":
					return Item.ItemType.Polearm;

				case "2.10":
					return Item.ItemType.Staff;

				case "4.6":
					return Item.ItemType.Shield;

				case "2.2":
					return Item.ItemType.Bow;

				case "2.18":
					return Item.ItemType.Crossbow;

				case "2.3":
					return Item.ItemType.Gun;

				case "2.19":
					return Item.ItemType.Wand;

				case "2.16":
					return Item.ItemType.Thrown;

				case "4.8":
					return Item.ItemType.Idol;

				case "4.7":
					return Item.ItemType.Libram;

				case "4.9":
					return Item.ItemType.Totem;

				case "6.2":
					return Item.ItemType.Arrow;

				case "6.3":
					return Item.ItemType.Bullet;

				case "11.2":
					return Item.ItemType.Quiver;

				case "11.3":
					return Item.ItemType.AmmoPouch;

				default:
					return Item.ItemType.None;
			}
		}

		private static Item.ItemSlot GetItemSlot(int slotId)
		{
			switch (slotId)
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

		
	}
}
