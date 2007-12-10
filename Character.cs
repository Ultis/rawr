using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace Rawr //O O . .
{
    [Serializable]
    public class Character
    {
        [XmlElement("Name")] public string _name;
        [XmlElement("Realm")] public string _realm;
        [XmlElement("Region")] public CharacterRegion _region = CharacterRegion.US;
        [XmlElement("Race")] public CharacterRace _race = CharacterRace.NightElf;
        [XmlElement("ActiveBuffs")] public List<string> _activeBuffs = new List<string>();
        [XmlElement("Head")] public string _head;
        [XmlElement("Neck")] public string _neck;
        [XmlElement("Shoulders")] public string _shoulders;
        [XmlElement("Back")] public string _back;
        [XmlElement("Chest")] public string _chest;
        [XmlElement("Shirt")] public string _shirt;
        [XmlElement("Tabard")] public string _tabard;
        [XmlElement("Wrist")] public string _wrist;
        [XmlElement("Hands")] public string _hands;
        [XmlElement("Waist")] public string _waist;
        [XmlElement("Legs")] public string _legs;
        [XmlElement("Feet")] public string _feet;
        [XmlElement("Finger1")] public string _finger1;
        [XmlElement("Finger2")] public string _finger2;
        [XmlElement("Trinket1")] public string _trinket1;
        [XmlElement("Trinket2")] public string _trinket2;
        [XmlElement("Weapon")] public string _weapon;
        [XmlElement("Idol")] public string _idol;
        [XmlElement("HeadEnchant")] public int _headEnchant = 0;
        [XmlElement("ShouldersEnchant")] public int _shouldersEnchant = 0;
        [XmlElement("BackEnchant")] public int _backEnchant = 0;
        [XmlElement("ChestEnchant")] public int _chestEnchant = 0;
        [XmlElement("WristEnchant")] public int _wristEnchant = 0;
        [XmlElement("HandsEnchant")] public int _handsEnchant = 0;
        [XmlElement("LegsEnchant")] public int _legsEnchant = 0;
        [XmlElement("FeetEnchant")] public int _feetEnchant = 0;
        [XmlElement("Finger1Enchant")] public int _finger1Enchant = 0;
        [XmlElement("Finger2Enchant")] public int _finger2Enchant = 0;
        [XmlElement("WeaponEnchant")] public int _weaponEnchant = 0;


        [XmlIgnore]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        [XmlIgnore]
        public string Realm
        {
            get { return _realm; }
            set { _realm = value; }
        }

        [XmlIgnore]
        public CharacterRegion Region
        {
            get { return _region; }
            set { _region = value; }
        }

        [XmlIgnore]
        public CharacterRace Race
        {
            get { return _race; }
            set
            {
                if (_race != value)
                {
                    _race = value;
                    OnItemsChanged();
                }
            }
        }

        [XmlIgnore]
        public List<string> ActiveBuffs
        {
            get { return _activeBuffs; }
            set { _activeBuffs = value; }
        }

        [XmlIgnore]
        public Item Head
        {
            get { return Item.LoadFromId(_head, "Equipped Head"); }
            set
            {
                if (value == null || _head != value.GemmedId)
                {
                    _head = value != null ? value.GemmedId : null;
                    OnItemsChanged();
                }
            }
        }

        [XmlIgnore]
        public Item Neck
        {
            get { return Item.LoadFromId(_neck, "Equipped Neck"); }
            set
            {
                if (value == null || _neck != value.GemmedId)
                {
                    _neck = value != null ? value.GemmedId : null;
                    OnItemsChanged();
                }
            }
        }

        [XmlIgnore]
        public Item Shoulders
        {
            get { return Item.LoadFromId(_shoulders, "Equipped Shoulders"); }
            set
            {
                if (value == null || _shoulders != value.GemmedId)
                {
                    _shoulders = value != null ? value.GemmedId : null;
                    OnItemsChanged();
                }
            }
        }

        [XmlIgnore]
        public Item Back
        {
            get { return Item.LoadFromId(_back, "Equipped Back"); }
            set
            {
                if (value == null || _back != value.GemmedId)
                {
                    _back = value != null ? value.GemmedId : null;
                    OnItemsChanged();
                }
            }
        }

        [XmlIgnore]
        public Item Chest
        {
            get { return Item.LoadFromId(_chest, "Equipped Chest"); }
            set
            {
                if (value == null || _chest != value.GemmedId)
                {
                    _chest = value != null ? value.GemmedId : null;
                    OnItemsChanged();
                }
            }
        }

        [XmlIgnore]
        public Item Shirt
        {
            get { return Item.LoadFromId(_shirt, "Equipped Shirt"); }
            set
            {
                if (value == null || _shirt != value.GemmedId)
                {
                    _shirt = value != null ? value.GemmedId : null;
                    OnItemsChanged();
                }
            }
        }

        [XmlIgnore]
        public Item Tabard
        {
            get { return Item.LoadFromId(_tabard, "Equipped Tabard"); }
            set
            {
                if (value == null || _tabard != value.GemmedId)
                {
                    _tabard = value != null ? value.GemmedId : null;
                    OnItemsChanged();
                }
            }
        }

        [XmlIgnore]
        public Item Wrist
        {
            get { return Item.LoadFromId(_wrist, "Equipped Wrist"); }
            set
            {
                if (value == null || _wrist != value.GemmedId)
                {
                    _wrist = value != null ? value.GemmedId : null;
                    OnItemsChanged();
                }
            }
        }

        [XmlIgnore]
        public Item Hands
        {
            get { return Item.LoadFromId(_hands, "Equipped Hands"); }
            set
            {
                if (value == null || _hands != value.GemmedId)
                {
                    _hands = value != null ? value.GemmedId : null;
                    OnItemsChanged();
                }
            }
        }

        [XmlIgnore]
        public Item Waist
        {
            get { return Item.LoadFromId(_waist, "Equipped Waist"); }
            set
            {
                if (value == null || _waist != value.GemmedId)
                {
                    _waist = value != null ? value.GemmedId : null;
                    OnItemsChanged();
                }
            }
        }

        [XmlIgnore]
        public Item Legs
        {
            get { return Item.LoadFromId(_legs, "Equipped Legs"); }
            set
            {
                if (value == null || _legs != value.GemmedId)
                {
                    _legs = value != null ? value.GemmedId : null;
                    OnItemsChanged();
                }
            }
        }

        [XmlIgnore]
        public Item Feet
        {
            get { return Item.LoadFromId(_feet, "Equipped Feet"); }
            set
            {
                if (value == null || _feet != value.GemmedId)
                {
                    _feet = value != null ? value.GemmedId : null;
                    OnItemsChanged();
                }
            }
        }

        [XmlIgnore]
        public Item Finger1
        {
            get { return Item.LoadFromId(_finger1, "Equipped Finger1"); }
            set
            {
                if (value == null || _finger1 != value.GemmedId)
                {
                    _finger1 = value != null ? value.GemmedId : null;
                    OnItemsChanged();
                }
            }
        }

        [XmlIgnore]
        public Item Finger2
        {
            get { return Item.LoadFromId(_finger2, "Equipped Finger2"); }
            set
            {
                if (value == null || _finger2 != value.GemmedId)
                {
                    _finger2 = value != null ? value.GemmedId : null;
                    OnItemsChanged();
                }
            }
        }

        [XmlIgnore]
        public Item Trinket1
        {
            get { return Item.LoadFromId(_trinket1, "Equipped Trinket1"); }
            set
            {
                if (value == null || _trinket1 != value.GemmedId)
                {
                    _trinket1 = value != null ? value.GemmedId : null;
                    OnItemsChanged();
                }
            }
        }

        [XmlIgnore]
        public Item Trinket2
        {
            get { return Item.LoadFromId(_trinket2, "Equipped Trinket2"); }
            set
            {
                if (value == null || _trinket2 != value.GemmedId)
                {
                    _trinket2 = value != null ? value.GemmedId : null;
                    OnItemsChanged();
                }
            }
        }

        [XmlIgnore]
        public Item Weapon
        {
            get { return Item.LoadFromId(_weapon, "Equipped Weapon"); }
            set
            {
                if (value == null || _weapon != value.GemmedId)
                {
                    _weapon = value != null ? value.GemmedId : null;
                    OnItemsChanged();
                }
            }
        }

        [XmlIgnore]
        public Item Idol
        {
            get { return Item.LoadFromId(_idol, "Equipped Idol"); }
            set
            {
                if (value == null || _idol != value.GemmedId)
                {
                    _idol = value != null ? value.GemmedId : null;
                    OnItemsChanged();
                }
            }
        }

        [XmlIgnore]
        public Enchant HeadEnchant
        {
            get { return Enchant.FindEnchant(_headEnchant, Item.ItemSlot.Head); }
            set { _headEnchant = value == null ? 0 : value.Id; }
        }

        [XmlIgnore]
        public Enchant ShouldersEnchant
        {
            get { return Enchant.FindEnchant(_shouldersEnchant, Item.ItemSlot.Shoulders); }
            set { _shouldersEnchant = value == null ? 0 : value.Id; }
        }

        [XmlIgnore]
        public Enchant BackEnchant
        {
            get { return Enchant.FindEnchant(_backEnchant, Item.ItemSlot.Back); }
            set { _backEnchant = value == null ? 0 : value.Id; }
        }

        [XmlIgnore]
        public Enchant ChestEnchant
        {
            get { return Enchant.FindEnchant(_chestEnchant, Item.ItemSlot.Chest); }
            set { _chestEnchant = value == null ? 0 : value.Id; }
        }

        [XmlIgnore]
        public Enchant WristEnchant
        {
            get { return Enchant.FindEnchant(_wristEnchant, Item.ItemSlot.Wrist); }
            set { _wristEnchant = value == null ? 0 : value.Id; }
        }

        [XmlIgnore]
        public Enchant HandsEnchant
        {
            get { return Enchant.FindEnchant(_handsEnchant, Item.ItemSlot.Hands); }
            set { _handsEnchant = value == null ? 0 : value.Id; }
        }

        [XmlIgnore]
        public Enchant LegsEnchant
        {
            get { return Enchant.FindEnchant(_legsEnchant, Item.ItemSlot.Legs); }
            set { _legsEnchant = value == null ? 0 : value.Id; }
        }

        [XmlIgnore]
        public Enchant FeetEnchant
        {
            get { return Enchant.FindEnchant(_feetEnchant, Item.ItemSlot.Feet); }
            set { _feetEnchant = value == null ? 0 : value.Id; }
        }

        [XmlIgnore]
        public Enchant Finger1Enchant
        {
            get { return Enchant.FindEnchant(_finger1Enchant, Item.ItemSlot.Finger); }
            set { _finger1Enchant = value == null ? 0 : value.Id; }
        }

        [XmlIgnore]
        public Enchant Finger2Enchant
        {
            get { return Enchant.FindEnchant(_finger2Enchant, Item.ItemSlot.Finger); }
            set { _finger2Enchant = value == null ? 0 : value.Id; }
        }

        [XmlIgnore]
        public Enchant WeaponEnchant
        {
            get { return Enchant.FindEnchant(_weaponEnchant, Item.ItemSlot.Weapon); }
            set { _weaponEnchant = value == null ? 0 : value.Id; }
        }

        public Enchant GetEnchantBySlot(Item.ItemSlot slot)
        {
            switch (slot)
            {
                case Item.ItemSlot.Head:
                    return HeadEnchant;
                   
                case Item.ItemSlot.Shoulders:
                    return ShouldersEnchant;
                    
                case Item.ItemSlot.Back:
                    return BackEnchant;
                    
                case Item.ItemSlot.Chest:
                    return ChestEnchant;
                    
                case Item.ItemSlot.Wrist:
                    return WristEnchant;
                    
                case Item.ItemSlot.Hands:
                    return HandsEnchant;
                    
                case Item.ItemSlot.Legs:
                    return LegsEnchant;
                    
                case Item.ItemSlot.Feet:
                    return FeetEnchant;
                    
                case Item.ItemSlot.Finger:
                    return Finger1Enchant;
                    
                case Item.ItemSlot.Weapon:
                    return WeaponEnchant;
                    
                default:
                    return null;
                    
            }
        }

        public void SetEnchantBySlot(Item.ItemSlot slot, Enchant enchant)
        {
            switch (slot)
            {
                case Item.ItemSlot.Head:
                    HeadEnchant = enchant;
                    break;
                case Item.ItemSlot.Shoulders:
                    ShouldersEnchant = enchant;
                    break;
                case Item.ItemSlot.Back:
                    BackEnchant = enchant;
                    break;
                case Item.ItemSlot.Chest:
                    ChestEnchant = enchant;
                    break;
                case Item.ItemSlot.Wrist:
                    WristEnchant = enchant;
                    break;
                case Item.ItemSlot.Hands:
                    HandsEnchant = enchant;
                    break;
                case Item.ItemSlot.Legs:
                    LegsEnchant = enchant;
                    break;
                case Item.ItemSlot.Feet:
                    FeetEnchant = enchant;
                    break;
                case Item.ItemSlot.Finger:
                    Finger1Enchant = enchant;
                    break;
                case Item.ItemSlot.Weapon:
                    WeaponEnchant = enchant;
                    break;
            }
        }

        public event EventHandler ItemsChanged;

        public void OnItemsChanged()
        {
            if (ItemsChanged != null)
                ItemsChanged(this, EventArgs.Empty);
        }

        [XmlIgnore]
        public Item this[CharacterSlot slot]
        {
            get
            {
                switch (slot)
                {
                    case CharacterSlot.Head:
                        return Head;
                        
                    case CharacterSlot.Neck:
                        return Neck;
                        
                    case CharacterSlot.Shoulders:
                        return Shoulders;
                        
                    case CharacterSlot.Back:
                        return Back;
                        
                    case CharacterSlot.Chest:
                        return Chest;
                        
                    case CharacterSlot.Shirt:
                        return Shirt;
                        
                    case CharacterSlot.Tabard:
                        return Tabard;
                        
                    case CharacterSlot.Wrist:
                        return Wrist;
                        
                    case CharacterSlot.Hands:
                        return Hands;
                        
                    case CharacterSlot.Waist:
                        return Waist;
                        
                    case CharacterSlot.Legs:
                        return Legs;
                        
                    case CharacterSlot.Feet:
                        return Feet;
                        
                    case CharacterSlot.Finger1:
                        return Finger1;
                        
                    case CharacterSlot.Finger2:
                        return Finger2;
                        
                    case CharacterSlot.Trinket1:
                        return Trinket1;
                        
                    case CharacterSlot.Trinket2:
                        return Trinket2;
                        
                    case CharacterSlot.Weapon:
                        return Weapon;
                        
                    case CharacterSlot.Idol:
                        return Idol;
                        
                    default:
                        return null;
                        
                }
            }
            set
            {
                switch (slot)
                {
                    case CharacterSlot.Head:
                        Head = value;
                        break;
                    case CharacterSlot.Neck:
                        Neck = value;
                        break;
                    case CharacterSlot.Shoulders:
                        Shoulders = value;
                        break;
                    case CharacterSlot.Back:
                        Back = value;
                        break;
                    case CharacterSlot.Chest:
                        Chest = value;
                        break;
                    case CharacterSlot.Shirt:
                        Shirt = value;
                        break;
                    case CharacterSlot.Tabard:
                        Tabard = value;
                        break;
                    case CharacterSlot.Wrist:
                        Wrist = value;
                        break;
                    case CharacterSlot.Hands:
                        Hands = value;
                        break;
                    case CharacterSlot.Waist:
                        Waist = value;
                        break;
                    case CharacterSlot.Legs:
                        Legs = value;
                        break;
                    case CharacterSlot.Feet:
                        Feet = value;
                        break;
                    case CharacterSlot.Finger1:
                        Finger1 = value;
                        break;
                    case CharacterSlot.Finger2:
                        Finger2 = value;
                        break;
                    case CharacterSlot.Trinket1:
                        Trinket1 = value;
                        break;
                    case CharacterSlot.Trinket2:
                        Trinket2 = value;
                        break;
                    case CharacterSlot.Weapon:
                        Weapon = value;
                        break;
                    case CharacterSlot.Idol:
                        Idol = value;
                        break;
                }
            }
        }

        public CharacterSlot[] GetEquippedSlots(Item item)
        {
            List<CharacterSlot> listSlots = new List<CharacterSlot>();
            foreach (CharacterSlot slot in Enum.GetValues(typeof (CharacterSlot)))
                if (this[slot] == item)
                    listSlots.Add(slot);
            return listSlots.ToArray();
        }

        public enum CharacterRegion
        {
            US,
            EU
        }

        public enum CharacterRace
        {
            NightElf,
            Tauren
        }

        public enum CharacterSlot
        {
            Head = 0,
            Neck = 1,
            Shoulders = 2,
            Back = 14,
            Chest = 4,
            Shirt = 3,
            Tabard = 18,
            Wrist = 8,
            Hands = 9,
            Waist = 5,
            Legs = 6,
            Feet = 7,
            Finger1 = 10,
            Finger2 = 11,
            Trinket1 = 12,
            Trinket2 = 13,
            Weapon = 15,
            Idol = 17,
            Gems = 100,
            Metas = 101,
            None = -1
        }

        public Character()
        {
        }

        public Character(string name, string realm, CharacterRegion region, CharacterRace race, string head, string neck,
                         string shoulders, string back, string chest, string shirt, string tabard,
                         string wrist, string hands, string waist, string legs, string feet, string finger1,
                         string finger2, string trinket1, string trinket2, string weapon, string idol)
            : this(
                name, realm, region, race, head, neck, shoulders, back, chest, shirt, tabard, wrist, hands, waist, legs,
                feet, finger1, finger2, trinket1, trinket2, weapon, idol,
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0)
        {
        }

        public Character(string name, string realm, CharacterRegion region, CharacterRace race, string head, string neck,
                         string shoulders, string back, string chest, string shirt, string tabard,
                         string wrist, string hands, string waist, string legs, string feet, string finger1,
                         string finger2, string trinket1, string trinket2, string weapon, string idol,
                         int enchantHead, int enchantShoulders, int enchantBack, int enchantChest, int enchantWrist,
                         int enchantHands, int enchantLegs, int enchantFeet, int enchantFinger1, int enchantFinger2,
                         int enchantWeapon)
        {
            _name = name;
            _realm = realm;
            _region = region;
            _race = race;
            _head = head;
            _neck = neck;
            _shoulders = shoulders;
            _back = back;
            _chest = chest;
            _shirt = shirt;
            _tabard = tabard;
            _wrist = wrist;
            _hands = hands;
            _waist = waist;
            _legs = legs;
            _feet = feet;
            _finger1 = finger1;
            _finger2 = finger2;
            _trinket1 = trinket1;
            _trinket2 = trinket2;
            _weapon = weapon;
            _idol = idol;

            _headEnchant = enchantHead;
            _shouldersEnchant = enchantShoulders;
            _backEnchant = enchantBack;
            _chestEnchant = enchantChest;
            _wristEnchant = enchantWrist;
            _handsEnchant = enchantHands;
            _legsEnchant = enchantLegs;
            _feetEnchant = enchantFeet;
            _finger1Enchant = enchantFinger1;
            _finger2Enchant = enchantFinger2;
            _weaponEnchant = enchantWeapon;
        }

        public Character Clone()
        {
            Character clone = new Character(Name, Realm, Region, Race,
                                            Head == null ? null : Head.GemmedId,
                                            Neck == null ? null : Neck.GemmedId,
                                            Shoulders == null ? null : Shoulders.GemmedId,
                                            Back == null ? null : Back.GemmedId,
                                            Chest == null ? null : Chest.GemmedId,
                                            Shirt == null ? null : Shirt.GemmedId,
                                            Tabard == null ? null : Tabard.GemmedId,
                                            Wrist == null ? null : Wrist.GemmedId,
                                            Hands == null ? null : Hands.GemmedId,
                                            Waist == null ? null : Waist.GemmedId,
                                            Legs == null ? null : Legs.GemmedId,
                                            Feet == null ? null : Feet.GemmedId,
                                            Finger1 == null ? null : Finger1.GemmedId,
                                            Finger2 == null ? null : Finger2.GemmedId,
                                            Trinket1 == null ? null : Trinket1.GemmedId,
                                            Trinket2 == null ? null : Trinket2.GemmedId,
                                            Weapon == null ? null : Weapon.GemmedId,
                                            Idol == null ? null : Idol.GemmedId,
                                            HeadEnchant.Id,
                                            ShouldersEnchant.Id,
                                            BackEnchant.Id,
                                            ChestEnchant.Id,
                                            WristEnchant.Id,
                                            HandsEnchant.Id,
                                            LegsEnchant.Id,
                                            FeetEnchant.Id,
                                            Finger1Enchant.Id,
                                            Finger2Enchant.Id,
                                            WeaponEnchant.Id);
            foreach (string buff in ActiveBuffs) clone.ActiveBuffs.Add(buff);
            return clone;
        }

        public void Save(string path)
        {
            XmlSerializer serializer = new XmlSerializer(typeof (Character));
            StringBuilder sb = new StringBuilder();
            StringWriter writer = new StringWriter(sb);
            serializer.Serialize(writer, this);
            writer.Close();
            File.WriteAllText(path, sb.ToString());
            //Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "Character.xml"), sb.ToString());
        }

        public static Character Load(string path)
        {
            Character character;
            if (File.Exists(path)) //Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "Character.xml")))
            {
                try
                {
                    string xml = File.ReadAllText(path).Replace("<Region>en", "<Region>US");
                        //Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "Character.xml")).Replace("<Region>en","<Region>US");
                    XmlSerializer serializer = new XmlSerializer(typeof (Character));
                    StringReader reader = new StringReader(xml);
                    character = (Character) serializer.Deserialize(reader);
                    reader.Close();
                }
                catch
                {
                    MessageBox.Show(
                        "There was an error attempting to open this character. Most likely, it was saved with a previous beta of Rawr, and isn't upgradable to the new format. Sorry. Please load your character from the armory to begin.");
                    character = new Character();
                }
            }
            else
                character = new Character();

            return character;
        }
    }
}