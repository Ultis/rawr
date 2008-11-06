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
            item.IconPath = string.Empty;
			return item;
		}


		private static SortedDictionary<int, List<Item>> _slotItems = new SortedDictionary<int, List<Item>>();
		private static SortedDictionary<string, List<Item>> _classsItems = new SortedDictionary<string, List<Item>>();
		private static SortedDictionary<int, List<Item>> _subclassItems = new SortedDictionary<int, List<Item>>();
		private static List<string> _unhandledKeys = new List<string>();
        private static List<string> _unhandledSocketBonus = new List<string>();
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

                case "reqrep": // reqrep=6: heroic 5 man, reqrep=1: heroic raid, reqrep=5: Arena, reqrep=4: Faction Friendly, reqrep=5: Faction Honored, etc
                case "reqfaction":
                    // Currently faction & reputation is not handled.
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
					item.Speed = float.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
					break;

				case "displayid":
                    // Possibly link to the model/icon.
					break;

				case "dmgmin1":
					item.MinDamage += (int)float.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
					break;

				case "dmgmax1":
					item.MaxDamage += (int)float.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
					break;

				case "dmgtype1":
					item.DamageType = (Item.ItemDamageType)int.Parse(value);
					break;

				case "armor":
				case "armorbonus":
					item.Stats.Armor += int.Parse(value);
					break;

				case "healthrgn":
					item.Stats.Hp5 += float.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
					break;

				case "manargn":
					item.Stats.Mp5 += float.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
					break;

				case "health":
					item.Stats.Health += float.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
					break;

				case "agi":
					item.Stats.Agility += float.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
					break;

				case "int":
					item.Stats.Intellect += float.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
					break;

				case "spi":
					item.Stats.Spirit += float.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
					break;

				case "sta":
					item.Stats.Stamina += float.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
					break;

				case "str":
					item.Stats.Strength += float.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
					break;

				case "races":
					break;

				case "mlehastertng":
				case "rgdhastertng":
				case "splhastertng":
					item.Stats.HasteRating = float.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
					break;

				case "skill":
					break;

				case "reqskill":
					break;

				case "reqskillrank":
					break;

				case "splheal":
				case "spldmg":
					item.Stats.SpellPower = float.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
					break;

				case "mlecritstrkrtng":
				case "rgdcritstrkrtng":
				case "splcritstrkrtng":
					item.Stats.CritRating = float.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
					break;

				case "holres":
					break;

				case "firres":
					item.Stats.FireResistance = float.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
					break;

				case "natres":
					item.Stats.NatureResistance = float.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
					break;

				case "frores":
					item.Stats.FrostResistance = float.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
					break;

				case "shares":
					item.Stats.ShadowResistance = float.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
					break;

				case "arcres":
					item.Stats.ArcaneResistance = float.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
					break;

				case "mlehitrtng":
				case "rgdhitrtng":
				case "splhitrtng":
					item.Stats.HitRating = float.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
					break;

				case "mleatkpwr":
				case "rgdatkpwr":
				case "feratkpwr":
					item.Stats.AttackPower += float.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
					break;


				case "block":
                    item.Stats.BlockValue += int.Parse(value);
					break;

				case "exprtng":
                    item.Stats.ExpertiseRating += int.Parse(value);
					break;

				case "defrtng":
                    item.Stats.DefenseRating += int.Parse(value);
					break;

				case "dodgertng":
                    item.Stats.DodgeRating += int.Parse(value);
					break;

				case "blockrtng":
                    item.Stats.BlockRating += int.Parse(value);
					break;

				case "socket1":
                    item.Sockets.Color1 = GetSocketType(value);
					break;

				case "socket2":
                    item.Sockets.Color2 = GetSocketType(value);
					break;

				case "socket3":
                    item.Sockets.Color3 = GetSocketType(value);
					break;

				case "socketbonus":
                    item.Sockets.Stats = GetSocketBonus(value);
                    break;

				case "nsockets":
                    // Rawr figures this out itself, Smart program.
					break;

				case "parryrtng":
					item.Stats.ParryRating += int.Parse(value);
					break;

				case "classes":
                    List<string> requiredClasses = new List<string>();
                    int classbitfield = int.Parse(value);
                    if ((classbitfield & 1) > 0)
                        requiredClasses.Add("Warrior");
                    if ((classbitfield & 2) > 0)
                        requiredClasses.Add("Paladin");
                    if ((classbitfield & 4) > 0)
                        requiredClasses.Add("Hunter");
                    if ((classbitfield & 8) > 0)
                        requiredClasses.Add("Rogue");
                    if ((classbitfield & 16) > 0)
                        requiredClasses.Add("Priest");
                    if ((classbitfield & 32) > 0)
                        requiredClasses.Add("Death Knight");
                    if ((classbitfield & 64) > 0)
                        requiredClasses.Add("Shaman");
                    if ((classbitfield & 128) > 0)
                        requiredClasses.Add("Mage");
                    if ((classbitfield & 256) > 0)
                        requiredClasses.Add("Warlock");
                    //if ((classbitfield & 512) > 0) ; // Only seems to occur in PvP gear, along with another huge value
                    //    requiredClasses.Add("");
                    if ((classbitfield & 1024) > 0)
                        requiredClasses.Add("Druid");
                    item.RequiredClasses = string.Join("|", requiredClasses.ToArray());
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
				case "t":       // Type (1 = drop)
                    break;

				case "ti":      // NPC that drops/gives
                    break;

				case "n":       // NPC 'Name'
                    break;

				case "z":       // Zone
                    string zonename = "Unknown";
                    switch (value)
                    {
                        #region Hugeass switch for all Zones.
                        case "65":
                            zonename = "Dragonblight";
                            break;
                        case "66":
                            zonename = "Zul'Drak";
                            break;
                        case "67":
                            zonename = "The Storm Peaks";
                            break;
                        case "206":
                            zonename = "Utgarde Keep";
                            break;
                        case "210":
                            zonename = "Icecrown";
                            break;
                        case "394":
                            zonename = "Grizzly Hills";
                            break;
                        case "495":
                            zonename = "Howling Fjord";
                            break;
                        case "1196":
                            zonename = "Utgarde Pinnacle";
                            break;
                        case "2817":
                            zonename = "Crystalsong Forest";
                            break;
                        case "2917":
                            zonename = "Hall of Legends";
                            break;
                        case "2918":
                            zonename = "Champion's Hall";
                            break;
                        case "3477":
                            zonename = "Azjol-Nerub";
                            break;
                        case "3537":
                            zonename = "Borean Tundra";
                            break;
                        case "3711":
                            zonename = "Sholazar Basin";
                            break;
                        case "4100":
                            zonename = "CoT: Stratholme";
                            break;
                        case "4120":
                            zonename = "The Nexus";
                            break;
                        case "4196":
                            zonename = "Drak'Tharon Keep";
                            break;
                        case "4197":
                            zonename = "Wintergrasp";
                            break;
                        case "4228":
                            zonename = "The Oculus";
                            break;
                        case "4264":
                            zonename = "Halls of Stone";
                            break;
                        case "4272":
                            zonename = "Halls of Lightning";
                            break;
                        case "4298":
                            zonename = "The Scarlet Enclave";
                            break;
                        case "4375":
                            zonename = "Gundrak";
                            break;
                        case "4395":
                            zonename = "Dalaran";
                            break;
                        case "4415":
                            zonename = "The Violet Hold";
                            break;
                        case "4493":
                            zonename = "The Obsidian Sanctum";
                            break;
                        case "4494":
                            zonename = "Ahn'kahet";
                            break;
                        case "4500":
                            zonename = "The Eye of Eternity";
                            break;
                        case "4603":
                            zonename = "Vault of Archavon";
                            break;
                        #endregion
                        default:
                            throw (new Exception("Unknown Area id :" + value));
                    }
                    break;

				case "c":
                    "".ToString();
                    break;

				case "c2":
                    "".ToString();
                    break;

				case "dd":      // ??? Dungeon Difficulty? (1 = Normal, 2 = Heroic)
                    break;

				case "s":
                    "".ToString();
                    break;

				case "q":
                    "".ToString();
                    break;

				case "p":
                    "".ToString();
                    break;


				default:
					if (!_unhandledKeys.Contains(key))
						_unhandledKeys.Add(key);
					break;
			}
			return false;
		}

        private static Item.ItemSlot GetSocketType(string socket)
        {
            switch (socket)
            {
                case "1":
                    return Item.ItemSlot.Meta;
                case "2":
                    return Item.ItemSlot.Red;
                case "4":
                    return Item.ItemSlot.Yellow;
                case "6":
                    return Item.ItemSlot.Orange;
                case "8":
                    return Item.ItemSlot.Blue;
                case "10":
                    return Item.ItemSlot.Purple;
                case "12":
                    return Item.ItemSlot.Green;
                case "14":
                    return Item.ItemSlot.Prismatic;
                default:
                    throw( new Exception("Unknown Slot Type :" + socket));
            }
        }

        private static Stats GetSocketBonus(string socketbonus)
        {
            Stats stats = new Stats();
            switch (socketbonus)
            {
                #region Hugeass switch to deal with all the socket bonuses. You dont want to see this. Really!
                case "2770":
                    stats.SpellPower += 7;
                    break;
                case "2771":
                    stats.CritRating += 8;
                    break;
                case "2787":
                    stats.CritRating += 8;
                    break;
                case "2842":
                    stats.Spirit += 8;
                    break;
                case "2843":
                    stats.CritRating += 8;
                    break;
                case "2854":
                    stats.Mp5 += 3;
                    break;
                case "2864":
                    stats.CritRating += 4;
                    break;
                case "2865":
                    stats.Mp5 += 2;
                    break;
                case "2868":
                    stats.Stamina += 6;
                    break;
                case "2869":
                    stats.Intellect += 4;
                    break;
                case "2872":
                    stats.SpellPower += 5;
                    break;
                case "2873":
                    stats.HitRating += 4;
                    break;
                case "2874":
                    stats.CritRating += 4;
                    break;
                case "2877":
                    stats.Agility += 4;
                    break;
                case "2878":
                    stats.Resilience += 4;
                    break;
                case "2882":
                    stats.Stamina += 6;
                    break;
                case "2888":
                    stats.BlockValue += 6;
                    break;
                case "2889":
                    stats.SpellPower += 5;
                    break;
                case "2890":
                    stats.Spirit += 4;
                    break;
                case "2892":
                    stats.Strength += 4;
                    break;
                case "2895":
                    stats.Stamina += 4;
                    break;
                case "2900":
                    stats.SpellPower += 4;
                    break;
                case "2908":
                    stats.CritRating += 6;
                    break;
                case "2927":
                    stats.Strength += 4;
                    break;
                case "2932":
                    stats.DefenseRating += 4;
                    break;
                case "2936":
                    stats.AttackPower += 8;
                    break;
                case "2951":
                    stats.CritRating += 4;
                    break;
                case "2952":
                    stats.CritRating += 4;
                    break;
                case "2963":
                    stats.HasteRating += 8;
                    break;
                case "2972":
                    stats.BlockRating += 4;
                    break;
                case "3094":
                    stats.ExpertiseRating += 4;
                    break;
                case "3198":
                    stats.SpellPower += 5;
                    break;
                case "3204":
                    stats.CritRating += 3;
                    break;
                case "3263":
                    stats.CritRating += 4;
                    break;
                case "3267":
                    stats.HasteRating += 4;
                    break;
                case "3301":
                    stats.CritRating += 6;
                    break;
                case "3302":
                    stats.DefenseRating += 8;
                    break;
                case "3303":
                    stats.HasteRating += 8;
                    break;
                case "3305":
                    stats.Stamina += 12;
                    break;
                case "3306":
                    stats.Mp5 += 2;
                    break;
                case "3307":
                    stats.Stamina += 9;
                    break;
                case "3308":
                    stats.HasteRating += 4;
                    break;
                case "3309":
                    stats.HasteRating += 6;
                    break;
                case "3310":
                    stats.Intellect += 6;
                    break;
                case "3311":
                    stats.Spirit += 6;
                    break;
                case "3312":
                    stats.Strength += 8;
                    break;
                case "3313":
                    stats.Agility += 8;
                    break;
                case "3314":
                    stats.CritRating += 8;
                    break;
                case "3316":
                    stats.CritRating += 6;
                    break;
                case "3351":
                    stats.HitRating += 6;
                    break;
                case "3352":
                    stats.Spirit += 8;
                    break;
                case "3353":
                    stats.Intellect += 8;
                    break;
                case "3354":
                    stats.Stamina += 12;
                    break;
                case "3355":
                    stats.Agility += 6;
                    break;
                case "3356":
                    stats.AttackPower += 12;
                    break;
                case "3357":
                    stats.Strength += 6;
                    break;
                case "3358":
                    stats.DodgeRating += 6;
                    break;
                case "3359":
                    stats.ParryRating += 4;
                    break;
                case "3360":
                    stats.ParryRating += 8;
                    break;
                case "3361":
                    stats.BlockRating += 6;
                    break;
                case "3362":
                    stats.ExpertiseRating += 6;
                    break;
                case "3363":
                    stats.BlockValue += 9;
                    break;
                case "3596":
                    stats.SpellPower += 5;
                    break;
                case "3600":
                    stats.Resilience += 6;
                    break;
                case "3602":
                    stats.SpellPower += 7;
                    break;
                case "3751":
                    stats.DefenseRating += 6;
                    break;
                case "3752":
                    stats.SpellPower += 5;
                    break;
                case "3753":
                    stats.SpellPower += 9;
                    break;
                case "3764":
                    stats.AttackPower += 12;
                    break;
                case "3765":
                    stats.ArmorPenetrationRating += 4;
                    break;
                case "3766":
                    stats.Stamina += 12;
                    break;
                case "3778":
                    stats.ExpertiseRating += 8;
                    break;
                case "3821":
                    stats.Resilience += 8;
                    break;
                default:
                    if (!_unhandledSocketBonus.Contains(socketbonus))
                        _unhandledSocketBonus.Add(socketbonus);
                    break;
                #endregion
            }
            return stats;
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
