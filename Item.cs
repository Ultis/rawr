using System;
using System.Xml.Serialization;

namespace Rawr
{
    public enum Quality
    {
        Poor = 0,
        Common,
        Uncommon,
        Rare,
        Epic,
        Legendary,
    }

    [Serializable]
    public class Item
    {
        [XmlElement("Name")] public string _name;
        [XmlElement("Id")] public int _id;
        [XmlElement("IconPath")] public string _iconPath;
        [XmlElement("Slot")] public ItemSlot _slot;
        [XmlElement("Stats")] public Stats _stats = new Stats();
        [XmlElement("Sockets")] public Sockets _sockets = new Sockets();
        [XmlElement("Gem1Id")] public int _gem1Id;
        [XmlElement("Gem2Id")] public int _gem2Id;
        [XmlElement("Gem3Id")] public int _gem3Id;
        [XmlElement("Quality")] public Quality _quality;


        [XmlIgnore]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        [XmlIgnore]
        public int Id
        {
            get { return _id; }
            set
            {
                OnIdsChanging();
                _id = value;
                OnIdsChanged();
            }
        }

        [XmlIgnore]
        public string IconPath
        {
            get { return _iconPath; }
            set { _iconPath = value; }
        }

        [XmlIgnore]
        public ItemSlot Slot
        {
            get { return _slot; }
            set { _slot = value; }
        }

        [XmlIgnore]
        public string SlotString
        {
            get { return _slot.ToString(); }
            set { _slot = (ItemSlot) Enum.Parse(typeof (ItemSlot), value); }
        }

        [XmlIgnore]
        public Stats Stats
        {
            get { return _stats; }
            set { _stats = value; }
        }

        [XmlIgnore]
        public Sockets Sockets
        {
            get { return _sockets; }
            set { _sockets = value; }
        }

        [XmlIgnore]
        public int Gem1Id
        {
            get { return _gem1Id; }
            set
            {
                OnIdsChanging();
                _gem1Id = value;
                OnIdsChanged();
            }
        }

        [XmlIgnore]
        public int Gem2Id
        {
            get { return _gem2Id; }
            set
            {
                OnIdsChanging();
                _gem2Id = value;
                OnIdsChanged();
            }
        }

        [XmlIgnore]
        public int Gem3Id
        {
            get { return _gem3Id; }
            set
            {
                OnIdsChanging();
                _gem3Id = value;
                OnIdsChanged();
            }
        }

        [XmlIgnore]
        public Item Gem1
        {
            get
            {
                if (Gem1Id == 0) return null;
                Item gem = LoadFromId(Gem1Id, "Gem1 in " + GemmedId);
                if (gem == null) Gem1Id = 0;
                return gem;
            }
            set
            {
                if (value == null)
                    Gem1Id = 0;
                else
                    Gem1Id = value.Id;
            }
        }

        [XmlIgnore]
        public Item Gem2
        {
            get
            {
                if (Gem2Id == 0) return null;
                Item gem = LoadFromId(Gem2Id, "Gem2 in " + GemmedId);
                if (gem == null) Gem2Id = 0;
                return gem;
            }
            set
            {
                if (value == null)
                    Gem2Id = 0;
                else
                    Gem2Id = value.Id;
            }
        }

        [XmlIgnore]
        public Item Gem3
        {
            get
            {
                if (Gem3Id == 0) return null;
                Item gem = LoadFromId(Gem3Id, "Gem3 in " + GemmedId);
                if (gem == null) Gem3Id = 0;
                return gem;
            }
            set
            {
                if (value == null)
                    Gem3Id = 0;
                else
                    Gem3Id = value.Id;
            }
        }

        private string _gemmedId = string.Empty;

        [XmlIgnore]
        public string GemmedId
        {
            get
            {
                if (_gemmedId == string.Empty)
                {
                    _gemmedId = string.Format("{0}.{1}.{2}.{3}", Id, Gem1Id, Gem2Id, Gem3Id);
                }
                return _gemmedId;
            }
        }

        [XmlIgnore]
        public Quality Quality
        {
            get { return _quality; }
        }

        private void OnIdsChanging()
        {
            ItemCache.DeleteItem(this, false);
        }

        public event EventHandler IdsChanged;

        private void OnIdsChanged()
        {
            _gemmedId = string.Empty;
            ItemCache.AddItem(this, false, false);
            if (IdsChanged != null) IdsChanged(this, null);
        }

        public enum ItemSlot
        {
            None = 0,
            Head = 1,
            Neck = 2,
            Shoulders = 3,
            Back = 16,
            Chest = 5,
            Shirt = 4,
            Tabard = 19,
            Wrist = 9,
            Hands = 10,
            Waist = 6,
            Legs = 7,
            Feet = 8,
            Finger = 11,
            Trinket = 12,
            Weapon = 17,
            Robe = 20,
            OneHand = 21,
            Wand = 26,
            Idol = 28,

            Meta = 101,
            Red = 102,
            Orange = 103,
            Yellow = 104,
            Green = 105,
            Prismatic = 106,
            Purple = 107,
            Blue = 108
        }

        public Item()
        {
        }

        public Item(string name, Quality quality, int id, string iconPath, ItemSlot slot, Stats stats, Sockets sockets,
                    int gem1Id, int gem2Id, int gem3Id)
        {
            _name = name;
            _id = id;
            _iconPath = iconPath;
            _slot = slot;
            _stats = stats;
            _sockets = sockets;
            _gem1Id = gem1Id;
            _gem2Id = gem2Id;
            _gem3Id = gem3Id;

            _quality = quality;
        }

        public override string ToString()
        {
            string summary = Name + ": ";
            summary += GetTotalStats().ToString();
            //summary += Stats.ToString();
            //summary += Sockets.ToString();
            if (summary.EndsWith(", ")) summary = summary.Substring(0, summary.Length - 2);

            if ((Sockets.Color1 != ItemSlot.None && Gem1Id == 0) ||
                (Sockets.Color2 != ItemSlot.None && Gem2Id == 0) ||
                (Sockets.Color3 != ItemSlot.None && Gem3Id == 0))
                summary += " [EMPTY SOCKETS]";

            return summary;
        }

        public Stats GetTotalStats()
        {
            Stats totalItemStats = new Stats();
            totalItemStats += Stats;
            bool eligibleForSocketBonus = GemMatchesSlot(Gem1, Sockets.Color1) &&
                                          GemMatchesSlot(Gem2, Sockets.Color2) &&
                                          GemMatchesSlot(Gem3, Sockets.Color3);
            if (Gem1 != null) totalItemStats += Gem1.Stats;
            if (Gem2 != null) totalItemStats += Gem2.Stats;
            if (Gem3 != null) totalItemStats += Gem3.Stats;
            if (eligibleForSocketBonus) totalItemStats += Sockets.Stats;
            return totalItemStats;
        }

        public static bool GemMatchesSlot(Item gem, ItemSlot slotColor)
        {
            switch (slotColor)
            {
                case ItemSlot.Red:
                    return
                        gem != null &&
                        (gem.Slot == ItemSlot.Red || gem.Slot == ItemSlot.Orange || gem.Slot == ItemSlot.Purple ||
                         gem.Slot == ItemSlot.Prismatic);
                    
                case ItemSlot.Yellow:
                    return
                        gem != null &&
                        (gem.Slot == ItemSlot.Yellow || gem.Slot == ItemSlot.Orange || gem.Slot == ItemSlot.Green ||
                         gem.Slot == ItemSlot.Prismatic);
                    
                case ItemSlot.Blue:
                    return
                        gem != null &&
                        (gem.Slot == ItemSlot.Blue || gem.Slot == ItemSlot.Green || gem.Slot == ItemSlot.Purple ||
                         gem.Slot == ItemSlot.Prismatic);
                    
                case ItemSlot.Meta:
                    return gem != null && (gem.Slot == ItemSlot.Meta);
                    
                default:
                    return gem == null;
                    
            }
        }

        public bool FitsInSlot(Character.CharacterSlot charSlot)
        {
            //And if I fell with all the strength I held inside...
            switch (charSlot)
            {
                case Character.CharacterSlot.Head:
                    return Slot == ItemSlot.Head;
                case Character.CharacterSlot.Neck:
                    return Slot == ItemSlot.Neck;
                case Character.CharacterSlot.Shoulders:
                    return Slot == ItemSlot.Shoulders;
                case Character.CharacterSlot.Back:
                    return Slot == ItemSlot.Back;
                case Character.CharacterSlot.Chest:
                    return Slot == ItemSlot.Chest || Slot == ItemSlot.Robe;
                case Character.CharacterSlot.Shirt:
                    return Slot == ItemSlot.Shirt;
                case Character.CharacterSlot.Tabard:
                    return Slot == ItemSlot.Tabard;
                case Character.CharacterSlot.Wrist:
                    return Slot == ItemSlot.Wrist;
                case Character.CharacterSlot.Hands:
                    return Slot == ItemSlot.Hands;
                case Character.CharacterSlot.Waist:
                    return Slot == ItemSlot.Waist;
                case Character.CharacterSlot.Legs:
                    return Slot == ItemSlot.Legs;
                case Character.CharacterSlot.Feet:
                    return Slot == ItemSlot.Feet;
                case Character.CharacterSlot.Finger1:
                case Character.CharacterSlot.Finger2:
                    return Slot == ItemSlot.Finger;
                case Character.CharacterSlot.Trinket1:
                case Character.CharacterSlot.Trinket2:
                    return Slot == ItemSlot.Trinket;
                case Character.CharacterSlot.Weapon:
                    return Slot == ItemSlot.Weapon || Slot == ItemSlot.OneHand;
                case Character.CharacterSlot.Idol:
                    return Slot == ItemSlot.Idol;
                case Character.CharacterSlot.Gems:
                    return Slot == ItemSlot.Red || Slot == ItemSlot.Blue || Slot == ItemSlot.Yellow
                           || Slot == ItemSlot.Purple || Slot == ItemSlot.Green || Slot == ItemSlot.Orange
                           || Slot == ItemSlot.Prismatic;
                case Character.CharacterSlot.Metas:
                    return Slot == ItemSlot.Meta;
                default:
                    return false;
            }
            //I wouldn't be out here... alone tonight
        }

        public static Item LoadFromId(int id, string logReason)
        {
            return LoadFromId(id, false, logReason);
        }

        public static Item LoadFromId(int id, bool forceRefresh, string logReason)
        {
            return LoadFromId(id.ToString() + ".0.0.0", forceRefresh, logReason);
        }

        public static Item LoadFromId(string gemmedId, string logReason)
        {
            return LoadFromId(gemmedId, false, logReason);
        }

        public static Item LoadFromId(string gemmedId, bool forceRefresh, string logReason)
        {
            if (string.IsNullOrEmpty(gemmedId))
                return null;
            Item cachedItem = ItemCache.FindItemById(gemmedId, false);
            if (cachedItem != null && !forceRefresh)
                return cachedItem;
            else
            {
                Item newItem = Armory.Instance.GetItem(gemmedId, logReason);
                if (newItem != null) ItemCache.AddItem(newItem);
                return newItem;
            }
            //{
            //int id = int.Parse(gemmedId.Split('.')[0]);
            //if (id == 0) return null;
            //WebClient client = new WebClient();
            //string itemXml = client.DownloadString("http://wow.allakhazam.com/cluster/item-xml.pl?witem=" + id.ToString());
            //XmlDocument doc = new XmlDocument();
            //doc.LoadXml(itemXml);
            //string name = doc.SelectSingleNode("wowitem/name1").InnerText;
            //string iconPath = doc.SelectSingleNode("wowitem/icon").InnerText;
            //if (iconPath.Contains("/images/icons/")) iconPath = iconPath.Substring(14);
            //ItemSlot slot = (ItemSlot)int.Parse(doc.SelectSingleNode("wowitem/slot").InnerText);
            //int armor = int.Parse(doc.SelectSingleNode("wowitem/armor").InnerText);
            //int agility = int.Parse(doc.SelectSingleNode("wowitem/stats/agility").InnerText);
            //int stamina = int.Parse(doc.SelectSingleNode("wowitem/stats/stamina").InnerText);
            //int dodgeRating = int.Parse(doc.SelectSingleNode("wowitem/stats/dodgerating").InnerText);
            //int defenseRating = int.Parse(doc.SelectSingleNode("wowitem/stats/defenserating").InnerText);
            //int resilience = int.Parse(doc.SelectSingleNode("wowitem/stats/resiliencerating").InnerText);

            //string displayHtml = doc.SelectSingleNode("wowitem/display_html").InnerText;
            //int socketBonusStartIndex = displayHtml.IndexOf("Socket Bonus: +");
            //Item.ItemSlot color1 = (Item.ItemSlot)int.Parse(doc.SelectSingleNode("wowitem/socket_1").InnerText);
            //Item.ItemSlot color2 = (Item.ItemSlot)int.Parse(doc.SelectSingleNode("wowitem/socket_2").InnerText);
            //Item.ItemSlot color3 = (Item.ItemSlot)int.Parse(doc.SelectSingleNode("wowitem/socket_3").InnerText);
            //int socketAgility = 0;
            //int socketStamina = 0;
            //int socketDodgeRating = 0;
            //int socketDefenseRating = 0;
            //int socketResilience = 0;
            //if (socketBonusStartIndex > 0)
            //{
            //    string socketBonus = displayHtml.Substring(socketBonusStartIndex + 15);
            //    socketBonus = socketBonus.Substring(0, socketBonus.IndexOf("</span>"));
            //    //Should be something like "4 Agility" now
            //    int socketBonusValue = int.Parse(socketBonus.Substring(0, socketBonus.IndexOf(' ')));
            //    string socketBonusStat = socketBonus.Substring(socketBonus.IndexOf(' ') + 1);

            //    switch (socketBonusStat)
            //    {
            //        case "Agility":
            //            socketAgility = socketBonusValue;
            //            break;

            //        case "Stamina":
            //            socketStamina = socketBonusValue;
            //            break;

            //        case "Dodge Rating":
            //            socketDodgeRating = socketBonusValue;
            //            break;

            //        case "Defense Rating":
            //            socketDefenseRating = socketBonusValue;
            //            break;

            //        case "Resilience":
            //            socketResilience = socketBonusValue;
            //            break;
            //    }
            //}

            //string[] ids = gemmedId.Split('.');
            //int id1 = ids.Length == 4 ? int.Parse(ids[1]) : 0;
            //int id2 = ids.Length == 4 ? int.Parse(ids[2]) : 0;
            //int id3 = ids.Length == 4 ? int.Parse(ids[3]) : 0;

            //return ItemCache.AddItem(new Item(name, id, iconPath, slot, new Stats(armor, 0, agility, stamina, dodgeRating, defenseRating, resilience),
            //    new Sockets(color1, color2, color3, new Stats(0, 0, socketAgility, socketStamina, socketDodgeRating, socketDefenseRating, socketResilience)),
            //    id1, id2, id3));
            //}
        }
    }

    [Serializable]
    public class Sockets
    {
        [XmlElement("Color1")] public Item.ItemSlot _color1;
        [XmlElement("Color2")] public Item.ItemSlot _color2;
        [XmlElement("Color3")] public Item.ItemSlot _color3;
        [XmlElement("Stats")] public Stats _stats = new Stats();

        [XmlIgnore]
        public Item.ItemSlot Color1
        {
            get { return _color1; }
            set { _color1 = value; }
        }

        [XmlIgnore]
        public Item.ItemSlot Color2
        {
            get { return _color2; }
            set { _color2 = value; }
        }

        [XmlIgnore]
        public Item.ItemSlot Color3
        {
            get { return _color3; }
            set { _color3 = value; }
        }

        [XmlIgnore]
        public string Color1String
        {
            get { return _color1.ToString(); }
            set { _color1 = (Item.ItemSlot) Enum.Parse(typeof (Item.ItemSlot), value); }
        }

        [XmlIgnore]
        public string Color2String
        {
            get { return _color2.ToString(); }
            set { _color2 = (Item.ItemSlot) Enum.Parse(typeof (Item.ItemSlot), value); }
        }

        [XmlIgnore]
        public string Color3String
        {
            get { return _color3.ToString(); }
            set { _color3 = (Item.ItemSlot) Enum.Parse(typeof (Item.ItemSlot), value); }
        }

        [XmlIgnore]
        public Stats Stats
        {
            get { return _stats; }
            set { _stats = value; }
        }

        public Sockets()
        {
        }

        public Sockets(Item.ItemSlot color1, Item.ItemSlot color2, Item.ItemSlot color3, Stats stats)
        {
            _color1 = color1;
            _color2 = color2;
            _color3 = color3;
            _stats = stats;
        }

        public override string ToString()
        {
            string summary = Color1.ToString().Substring(0, 1) + Color2.ToString().Substring(0, 1) +
                             Color3.ToString().Substring(0, 1);
            summary = summary.Replace("N", "") + "+";
            if (Stats.Agility > 0) summary += Stats.Agility.ToString() + "Agi";
            if (Stats.Stamina > 0) summary += Stats.Stamina.ToString() + "Sta";
            if (Stats.DodgeRating > 0) summary += Stats.DodgeRating.ToString() + "Dodge";
            if (Stats.DefenseRating > 0) summary += Stats.DefenseRating.ToString() + "Def";
            if (Stats.Resilience > 0) summary += Stats.Resilience.ToString() + "Res";
            if (summary.EndsWith("+")) summary = summary.Substring(0, summary.Length - 1);
            return summary;
        }
    }
}