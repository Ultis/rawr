using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Windows.Forms;
using System.IO;

namespace Rawr
{
    [Serializable]
    public class Character
    {
        [System.Xml.Serialization.XmlElement("Name")]
        public string _name;
        [System.Xml.Serialization.XmlElement("Realm")]
        public string _realm;
        [System.Xml.Serialization.XmlElement("Region")]
		public Character.CharacterRegion _region = CharacterRegion.US;
        [System.Xml.Serialization.XmlElement("Race")]
        public CharacterRace _race = CharacterRace.NightElf;
        [System.Xml.Serialization.XmlElement("Buffs")]
        public Buffs _buffs = new Buffs();
        [System.Xml.Serialization.XmlElement("Head")]
        public string _head;
        [System.Xml.Serialization.XmlElement("Neck")]
        public string _neck;
        [System.Xml.Serialization.XmlElement("Shoulders")]
        public string _shoulders;
        [System.Xml.Serialization.XmlElement("Back")]
        public string _back;
        [System.Xml.Serialization.XmlElement("Chest")]
        public string _chest;
        [System.Xml.Serialization.XmlElement("Shirt")]
        public string _shirt;
        [System.Xml.Serialization.XmlElement("Tabard")]
        public string _tabard;
        [System.Xml.Serialization.XmlElement("Wrist")]
        public string _wrist;
        [System.Xml.Serialization.XmlElement("Hands")]
        public string _hands;
        [System.Xml.Serialization.XmlElement("Waist")]
        public string _waist;
        [System.Xml.Serialization.XmlElement("Legs")]
        public string _legs;
        [System.Xml.Serialization.XmlElement("Feet")]
        public string _feet;
        [System.Xml.Serialization.XmlElement("Finger1")]
        public string _finger1;
        [System.Xml.Serialization.XmlElement("Finger2")]
        public string _finger2;
        [System.Xml.Serialization.XmlElement("Trinket1")]
        public string _trinket1;
        [System.Xml.Serialization.XmlElement("Trinket2")]
        public string _trinket2;
        [System.Xml.Serialization.XmlElement("Weapon")]
        public string _weapon;
        [System.Xml.Serialization.XmlElement("Idol")]
        public string _idol;
		[System.Xml.Serialization.XmlElement("HeadEnchant")]
		public int _headEnchant = 0;
		[System.Xml.Serialization.XmlElement("ShouldersEnchant")]
		public int _shouldersEnchant = 0;
		[System.Xml.Serialization.XmlElement("BackEnchant")]
		public int _backEnchant = 0;
		[System.Xml.Serialization.XmlElement("ChestEnchant")]
		public int _chestEnchant = 0;
		[System.Xml.Serialization.XmlElement("WristEnchant")]
		public int _wristEnchant = 0;
		[System.Xml.Serialization.XmlElement("HandsEnchant")]
		public int _handsEnchant = 0;
		[System.Xml.Serialization.XmlElement("LegsEnchant")]
		public int _legsEnchant = 0;
		[System.Xml.Serialization.XmlElement("FeetEnchant")]
		public int _feetEnchant = 0;
		[System.Xml.Serialization.XmlElement("WeaponEnchant")]
		public int _weaponEnchant = 0;


        [System.Xml.Serialization.XmlIgnore]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        [System.Xml.Serialization.XmlIgnore]
        public string Realm
        {
            get { return _realm; }
            set { _realm = value; }
        }
        [System.Xml.Serialization.XmlIgnore]
		public Character.CharacterRegion Region
        {
            get { return _region; }
            set { _region = value; }
        }
        [System.Xml.Serialization.XmlIgnore]
        public CharacterRace Race
        {
            get { return _race; }
            set
            {
                if (_race != value)
                {
                    _race = value;
                    if (ItemsChanged != null) ItemsChanged(this, EventArgs.Empty);
                }
            }
        }
        [System.Xml.Serialization.XmlIgnore]
        public Buffs Buffs
        {
            get { return _buffs; }
            set { _buffs = value; }
        }
        [System.Xml.Serialization.XmlIgnore]
        public Item Head
        {
            get { return Item.LoadFromId(_head, "Equipped Head"); }
            set
            {
                if (value == null || _head != value.GemmedId)
                {
                    _head = value != null ? value.GemmedId : null;
                    if (ItemsChanged != null) ItemsChanged(this, EventArgs.Empty);
                }
            }
        }
        [System.Xml.Serialization.XmlIgnore]
        public Item Neck
        {
			get { return Item.LoadFromId(_neck, "Equipped Neck"); }
            set
            {
                if (value == null || _neck != value.GemmedId)
                {
                    _neck = value != null ? value.GemmedId : null;
                    if (ItemsChanged != null) ItemsChanged(this, EventArgs.Empty);
                }
            }
        }
        [System.Xml.Serialization.XmlIgnore]
        public Item Shoulders
        {
			get { return Item.LoadFromId(_shoulders, "Equipped Shoulders"); }
            set
            {
                if (value == null || _shoulders != value.GemmedId)
                {
                    _shoulders = value != null ? value.GemmedId : null;
                    if (ItemsChanged != null) ItemsChanged(this, EventArgs.Empty);
                }
            }
        }
        [System.Xml.Serialization.XmlIgnore]
        public Item Back
        {
			get { return Item.LoadFromId(_back, "Equipped Back"); }
            set
            {
                if (value == null || _back != value.GemmedId)
                {
                    _back = value != null ? value.GemmedId : null;
                    if (ItemsChanged != null) ItemsChanged(this, EventArgs.Empty);
                }
            }
        }
        [System.Xml.Serialization.XmlIgnore]
        public Item Chest
        {
			get { return Item.LoadFromId(_chest, "Equipped Chest"); }
            set
            {
                if (value == null || _chest != value.GemmedId)
                {
                    _chest = value != null ? value.GemmedId : null;
                    if (ItemsChanged != null) ItemsChanged(this, EventArgs.Empty);
                }
            }
        }
        [System.Xml.Serialization.XmlIgnore]
        public Item Shirt
        {
			get { return Item.LoadFromId(_shirt, "Equipped Shirt"); }
            set
            {
                if (value == null || _shirt != value.GemmedId)
                {
                    _shirt = value != null ? value.GemmedId : null;
                    if (ItemsChanged != null) ItemsChanged(this, EventArgs.Empty);
                }
            }
        }
        [System.Xml.Serialization.XmlIgnore]
        public Item Tabard
        {
			get { return Item.LoadFromId(_tabard, "Equipped Tabard"); }
            set
            {
                if (value == null || _tabard != value.GemmedId)
                {
                    _tabard = value != null ? value.GemmedId : null;
                    if (ItemsChanged != null) ItemsChanged(this, EventArgs.Empty);
                }
            }
        }
        [System.Xml.Serialization.XmlIgnore]
        public Item Wrist
        {
			get { return Item.LoadFromId(_wrist, "Equipped Wrist"); }
            set
            {
                if (value == null || _wrist != value.GemmedId)
                {
                    _wrist = value != null ? value.GemmedId : null;
                    if (ItemsChanged != null) ItemsChanged(this, EventArgs.Empty);
                }
            }
        }
        [System.Xml.Serialization.XmlIgnore]
        public Item Hands
        {
			get { return Item.LoadFromId(_hands, "Equipped Hands"); }
            set
            {
                if (value == null || _hands != value.GemmedId)
                {
                    _hands = value != null ? value.GemmedId : null;
                    if (ItemsChanged != null) ItemsChanged(this, EventArgs.Empty);
                }
            }
        }
        [System.Xml.Serialization.XmlIgnore]
        public Item Waist
        {
			get { return Item.LoadFromId(_waist, "Equipped Waist"); }
            set
            {
                if (value == null || _waist != value.GemmedId)
                {
                    _waist = value != null ? value.GemmedId : null;
                    if (ItemsChanged != null) ItemsChanged(this, EventArgs.Empty);
                }
            }
        }
        [System.Xml.Serialization.XmlIgnore]
        public Item Legs
        {
			get { return Item.LoadFromId(_legs, "Equipped Legs"); }
            set
            {
                if (value == null || _legs != value.GemmedId)
                {
                    _legs = value != null ? value.GemmedId : null;
                    if (ItemsChanged != null) ItemsChanged(this, EventArgs.Empty);
                }
            }
        }
        [System.Xml.Serialization.XmlIgnore]
        public Item Feet
        {
			get { return Item.LoadFromId(_feet, "Equipped Feet"); }
            set
            {
                if (value == null || _feet != value.GemmedId)
                {
                    _feet = value != null ? value.GemmedId : null;
                    if (ItemsChanged != null) ItemsChanged(this, EventArgs.Empty);
                }
            }
        }
        [System.Xml.Serialization.XmlIgnore]
        public Item Finger1
        {
			get { return Item.LoadFromId(_finger1, "Equipped Finger1"); }
            set
            {
                if (value == null || _finger1 != value.GemmedId)
                {
                    _finger1 = value != null ? value.GemmedId : null;
                    if (ItemsChanged != null) ItemsChanged(this, EventArgs.Empty);
                }
            }
        }
        [System.Xml.Serialization.XmlIgnore]
        public Item Finger2
        {
			get { return Item.LoadFromId(_finger2, "Equipped Finger2"); }
            set
            {
                if (value == null || _finger2 != value.GemmedId)
                {
                    _finger2 = value != null ? value.GemmedId : null;
                    if (ItemsChanged != null) ItemsChanged(this, EventArgs.Empty);
                }
            }
        }
        [System.Xml.Serialization.XmlIgnore]
        public Item Trinket1
        {
			get { return Item.LoadFromId(_trinket1, "Equipped Trinket1"); }
            set
            {
                if (value == null || _trinket1 != value.GemmedId)
                {
                    _trinket1 = value != null ? value.GemmedId : null;
                    if (ItemsChanged != null) ItemsChanged(this, EventArgs.Empty);
                }
            }
        }
        [System.Xml.Serialization.XmlIgnore]
        public Item Trinket2
        {
			get { return Item.LoadFromId(_trinket2, "Equipped Trinket2"); }
            set
            {
                if (value == null || _trinket2 != value.GemmedId)
                {
                    _trinket2 = value != null ? value.GemmedId : null;
                    if (ItemsChanged != null) ItemsChanged(this, EventArgs.Empty);
                }
            }
        }
        [System.Xml.Serialization.XmlIgnore]
        public Item Weapon
        {
			get { return Item.LoadFromId(_weapon, "Equipped Weapon"); }
            set
            {
                if (value == null || _weapon != value.GemmedId)
                {
                    _weapon = value != null ? value.GemmedId : null;
                    if (ItemsChanged != null) ItemsChanged(this, EventArgs.Empty);
                }
            }
        }
        [System.Xml.Serialization.XmlIgnore]
        public Item Idol
        {
			get { return Item.LoadFromId(_idol, "Equipped Idol"); }
            set
            {
                if (value == null || _idol != value.GemmedId)
                {
                    _idol = value != null ? value.GemmedId : null;
                    if (ItemsChanged != null) ItemsChanged(this, EventArgs.Empty);
                }
            }
		}
		[System.Xml.Serialization.XmlIgnore]
		public Enchant HeadEnchant
		{
			get { return Enchant.FindEnchant(_headEnchant, Item.ItemSlot.Head); }
			set { _headEnchant = value == null ? 0 : value.Id; }
		}
		[System.Xml.Serialization.XmlIgnore]
		public Enchant ShouldersEnchant
		{
			get { return Enchant.FindEnchant(_shouldersEnchant, Item.ItemSlot.Shoulders); }
			set { _shouldersEnchant = value == null ? 0 : value.Id; }
		}
		[System.Xml.Serialization.XmlIgnore]
		public Enchant BackEnchant
		{
			get { return Enchant.FindEnchant(_backEnchant, Item.ItemSlot.Back); }
			set { _backEnchant = value == null ? 0 : value.Id; }
		}
		[System.Xml.Serialization.XmlIgnore]
		public Enchant ChestEnchant
		{
			get { return Enchant.FindEnchant(_chestEnchant, Item.ItemSlot.Chest); }
			set { _chestEnchant = value == null ? 0 : value.Id; }
		}
		[System.Xml.Serialization.XmlIgnore]
		public Enchant WristEnchant
		{
			get { return Enchant.FindEnchant(_wristEnchant, Item.ItemSlot.Wrist); }
			set { _wristEnchant = value == null ? 0 : value.Id; }
		}
		[System.Xml.Serialization.XmlIgnore]
		public Enchant HandsEnchant
		{
			get { return Enchant.FindEnchant(_handsEnchant, Item.ItemSlot.Hands); }
			set { _handsEnchant = value == null ? 0 : value.Id; }
		}
		[System.Xml.Serialization.XmlIgnore]
		public Enchant LegsEnchant
		{
			get { return Enchant.FindEnchant(_legsEnchant, Item.ItemSlot.Legs); }
			set { _legsEnchant = value == null ? 0 : value.Id; }
		}
		[System.Xml.Serialization.XmlIgnore]
		public Enchant FeetEnchant
		{
			get { return Enchant.FindEnchant(_feetEnchant, Item.ItemSlot.Feet); }
			set { _feetEnchant = value == null ? 0 : value.Id; }
		}
		[System.Xml.Serialization.XmlIgnore]
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
					break;
				case Item.ItemSlot.Shoulders:
					return ShouldersEnchant;
					break;
				case Item.ItemSlot.Back:
					return BackEnchant;
					break;
				case Item.ItemSlot.Chest:
					return ChestEnchant;
					break;
				case Item.ItemSlot.Wrist:
					return WristEnchant;
					break;
				case Item.ItemSlot.Hands:
					return HandsEnchant;
					break;
				case Item.ItemSlot.Legs:
					return LegsEnchant;
					break;
				case Item.ItemSlot.Feet:
					return FeetEnchant;
					break;
				case Item.ItemSlot.Weapon:
					return WeaponEnchant;
					break;
				default:
					return null;
					break;
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
				case Item.ItemSlot.Weapon:
					WeaponEnchant = enchant;
					break;
			}
		}

		public event EventHandler ItemsChanged;

        [System.Xml.Serialization.XmlIgnore]
        public Item this[CharacterSlot slot]
        {
            get
            {
                switch (slot)
                {
                    case CharacterSlot.Head:
                        return this.Head;
                        break;
                    case CharacterSlot.Neck:
                        return this.Neck;
                        break;
                    case CharacterSlot.Shoulders:
                        return this.Shoulders;
                        break;
                    case CharacterSlot.Back:
                        return this.Back;
                        break;
                    case CharacterSlot.Chest:
                        return this.Chest;
                        break;
                    case CharacterSlot.Shirt:
                        return this.Shirt;
                        break;
                    case CharacterSlot.Tabard:
                        return this.Tabard;
                        break;
                    case CharacterSlot.Wrist:
                        return this.Wrist;
                        break;
                    case CharacterSlot.Hands:
                        return this.Hands;
                        break;
                    case CharacterSlot.Waist:
                        return this.Waist;
                        break;
                    case CharacterSlot.Legs:
                        return this.Legs;
                        break;
                    case CharacterSlot.Feet:
                        return this.Feet;
                        break;
                    case CharacterSlot.Finger1:
                        return this.Finger1;
                        break;
                    case CharacterSlot.Finger2:
                        return this.Finger2;
                        break;
                    case CharacterSlot.Trinket1:
                        return this.Trinket1;
                        break;
                    case CharacterSlot.Trinket2:
                        return this.Trinket2;
                        break;
                    case CharacterSlot.Weapon:
                        return this.Weapon;
                        break;
                    case CharacterSlot.Idol:
                        return this.Idol;
                        break;
                    default:
                        return null;
                        break;
                }
            }
            set
            {
                switch (slot)
                {
                    case CharacterSlot.Head:
                        this.Head = value;
                        break;
                    case CharacterSlot.Neck:
                        this.Neck = value;
                        break;
                    case CharacterSlot.Shoulders:
                        this.Shoulders = value;
                        break;
                    case CharacterSlot.Back:
                        this.Back = value;
                        break;
                    case CharacterSlot.Chest:
                        this.Chest = value;
                        break;
                    case CharacterSlot.Shirt:
                        this.Shirt = value;
                        break;
                    case CharacterSlot.Tabard:
                        this.Tabard = value;
                        break;
                    case CharacterSlot.Wrist:
                        this.Wrist = value;
                        break;
                    case CharacterSlot.Hands:
                        this.Hands = value;
                        break;
                    case CharacterSlot.Waist:
                        this.Waist = value;
                        break;
                    case CharacterSlot.Legs:
                        this.Legs = value;
                        break;
                    case CharacterSlot.Feet:
                        this.Feet = value;
                        break;
                    case CharacterSlot.Finger1:
                        this.Finger1 = value;
                        break;
                    case CharacterSlot.Finger2:
                        this.Finger2 = value;
                        break;
                    case CharacterSlot.Trinket1:
                        this.Trinket1 = value;
                        break;
                    case CharacterSlot.Trinket2:
                        this.Trinket2 = value;
                        break;
                    case CharacterSlot.Weapon:
                        this.Weapon = value;
                        break;
                    case CharacterSlot.Idol:
                        this.Idol = value;
                        break;
                }
            }
        }

		public enum CharacterRegion { US, EU }
		public enum CharacterRace { NightElf, Tauren }
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
			Metas = 101
        }

        public Character() { }
		public Character(string name, string realm, Character.CharacterRegion region, CharacterRace race, string head, string neck, string shoulders, string back, string chest, string shirt, string tabard,
                string wrist, string hands, string waist, string legs, string feet, string finger1, string finger2, string trinket1, string trinket2, string weapon, string idol) 
        : this(name, realm, region, race, head, neck, shoulders, back, chest, shirt, tabard, wrist, hands, waist, legs, feet, finger1, finger2, trinket1, trinket2, weapon, idol,
			0, 0, 0, 0, 0, 0, 0, 0, 0)
		{ }

		public Character(string name, string realm, Character.CharacterRegion region, CharacterRace race, string head, string neck, string shoulders, string back, string chest, string shirt, string tabard,
                string wrist, string hands, string waist, string legs, string feet, string finger1, string finger2, string trinket1, string trinket2, string weapon, string idol,
			int enchantHead, int enchantShoulders, int enchantBack, int enchantChest, int enchantWrist, int enchantHands, int enchantLegs, int enchantFeet, int enchantWeapon)
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
			_weaponEnchant = enchantWeapon;
		}

		public Character Clone()
		{
			Character clone = new Character(this.Name, this.Realm, this.Region, this.Race,
						this.Head == null ? null : this.Head.GemmedId,
						this.Neck == null ? null : this.Neck.GemmedId,
						this.Shoulders == null ? null : this.Shoulders.GemmedId,
						this.Back == null ? null : this.Back.GemmedId,
						this.Chest == null ? null : this.Chest.GemmedId,
						this.Shirt == null ? null : this.Shirt.GemmedId,
						this.Tabard == null ? null : this.Tabard.GemmedId,
						this.Wrist == null ? null : this.Wrist.GemmedId,
						this.Hands == null ? null : this.Hands.GemmedId,
						this.Waist == null ? null : this.Waist.GemmedId,
						this.Legs == null ? null : this.Legs.GemmedId,
						this.Feet == null ? null : this.Feet.GemmedId,
						this.Finger1 == null ? null : this.Finger1.GemmedId,
						this.Finger2 == null ? null : this.Finger2.GemmedId,
						this.Trinket1 == null ? null : this.Trinket1.GemmedId,
						this.Trinket2 == null ? null : this.Trinket2.GemmedId,
						this.Weapon == null ? null : this.Weapon.GemmedId,
						this.Idol == null ? null : this.Idol.GemmedId,
						this.HeadEnchant.Id,
						this.ShouldersEnchant.Id,
						this.BackEnchant.Id,
						this.ChestEnchant.Id,
						this.WristEnchant.Id,
						this.HandsEnchant.Id,
						this.LegsEnchant.Id,
						this.FeetEnchant.Id,
						this.WeaponEnchant.Id);
			foreach (string buff in Buffs.AllBuffs)
				clone.Buffs[buff] = this.Buffs[buff];
			return clone;
		}
    
        public void Save(string path)
        {
            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(Character));
            StringBuilder sb = new StringBuilder();
            System.IO.StringWriter writer = new System.IO.StringWriter(sb);
            serializer.Serialize(writer, this);
            writer.Close();
			System.IO.File.WriteAllText(path, sb.ToString());//Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "Character.xml"), sb.ToString());
		}

        public static Character Load(string path)
        {
            Character character;
			if (File.Exists(path))//Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "Character.xml")))
            {
				try
				{
					string xml = System.IO.File.ReadAllText(path).Replace("<Region>en", "<Region>US");//Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "Character.xml")).Replace("<Region>en","<Region>US");
					System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(Character));
					System.IO.StringReader reader = new System.IO.StringReader(xml);
					character = (Character)serializer.Deserialize(reader);
					reader.Close();
				}
				catch
				{
					MessageBox.Show("There was an error attempting to open this character. Most likely, it was saved with a previous beta of Rawr, and isn't upgradable to the new format. Sorry. Please load your character from the armory to begin.");
					character = new Character();
				}
            }
            else
                character = new Character();

            return character;
        }

		//public static Character LoadFromArmory(Armory.Region region, string realm, string name)
		//{
		//    WebClient client = new WebClient();
		//    string armoryDomain = region == Armory.Region.US ? "worldofwarcraft" : "wow-europe";
		//    string characterSheetPath = string.Format("http://armory.{0}.com/character-sheet.xml?r={1}&n={2}",
		//        armoryDomain, realm, name);
		//    string armoryHtml = client.DownloadString(characterSheetPath);
			
			
		//    #region Old Armory Html to Item Id code
		//    /* 
		//    int index = armoryHtml.IndexOf("itemsArray[0]  = [");
		//    string headIdString = index < 0 ? " " : armoryHtml.Substring(index + 18, 50);
		//    index = armoryHtml.IndexOf("itemsArray[1]  = [");
		//    string neckIdString = index < 0 ? " " : armoryHtml.Substring(index + 18, 50);
		//    index = armoryHtml.IndexOf("itemsArray[2]  = [");
		//    string shouldersIdString = index < 0 ? " " : armoryHtml.Substring(index + 18, 50);
		//    index = armoryHtml.IndexOf("itemsArray[14]  = [");
		//    string backIdString = index < 0 ? " " : armoryHtml.Substring(index + 19, 50);
		//    index = armoryHtml.IndexOf("itemsArray[4]  = [");
		//    string chestIdString = index < 0 ? " " : armoryHtml.Substring(index + 18, 50);
		//    index = armoryHtml.IndexOf("itemsArray[3]  = [");
		//    string shirtIdString = index < 0 ? " " : armoryHtml.Substring(index + 18, 50);
		//    index = armoryHtml.IndexOf("itemsArray[18]  = [");
		//    string tabardIdString = index < 0 ? " " : armoryHtml.Substring(index + 19, 50);
		//    index = armoryHtml.IndexOf("itemsArray[8]  = [");
		//    string wristIdString = index < 0 ? " " : armoryHtml.Substring(index + 18, 50);
		//    index = armoryHtml.IndexOf("itemsArray[9]  = [");
		//    string handsIdString = index < 0 ? " " : armoryHtml.Substring(index + 18, 50);
		//    index = armoryHtml.IndexOf("itemsArray[5]  = [");
		//    string waistIdString = index < 0 ? " " : armoryHtml.Substring(index + 18, 50);
		//    index = armoryHtml.IndexOf("itemsArray[6]  = [");
		//    string legsIdString = index < 0 ? " " : armoryHtml.Substring(index + 18, 50);
		//    index = armoryHtml.IndexOf("itemsArray[7]  = [");
		//    string feetIdString = index < 0 ? " " : armoryHtml.Substring(index + 18, 50);
		//    index = armoryHtml.IndexOf("itemsArray[10]  = [");
		//    string finger1IdString = index < 0 ? " " : armoryHtml.Substring(index + 19, 50);
		//    index = armoryHtml.IndexOf("itemsArray[11]  = [");
		//    string finger2IdString = index < 0 ? " " : armoryHtml.Substring(index + 19, 50);
		//    index = armoryHtml.IndexOf("itemsArray[12]  = [");
		//    string trinket1IdString = index < 0 ? " " : armoryHtml.Substring(index + 19, 50);
		//    index = armoryHtml.IndexOf("itemsArray[13]  = [");
		//    string trinket2IdString = index < 0 ? " " : armoryHtml.Substring(index + 19, 50);
		//    index = armoryHtml.IndexOf("itemsArray[15]  = [");
		//    string weaponIdString = index < 0 ? " " : armoryHtml.Substring(index + 19, 50);
		//    index = armoryHtml.IndexOf("itemsArray[17]  = [");
		//    string idolIdString = index < 0 ? " " : armoryHtml.Substring(index + 19, 50);
		//    headIdString = headIdString.Substring(0, headIdString.IndexOf(" "));
		//    neckIdString = neckIdString.Substring(0, neckIdString.IndexOf(" "));
		//    shouldersIdString = shouldersIdString.Substring(0, shouldersIdString.IndexOf(" "));
		//    backIdString = backIdString.Substring(0, backIdString.IndexOf(" "));
		//    chestIdString = chestIdString.Substring(0, chestIdString.IndexOf(" "));
		//    shirtIdString = shirtIdString.Substring(0, shirtIdString.IndexOf(" "));
		//    tabardIdString = tabardIdString.Substring(0, tabardIdString.IndexOf(" "));
		//    wristIdString = wristIdString.Substring(0, wristIdString.IndexOf(" "));
		//    handsIdString = handsIdString.Substring(0, handsIdString.IndexOf(" "));
		//    waistIdString = waistIdString.Substring(0, waistIdString.IndexOf(" "));
		//    legsIdString = legsIdString.Substring(0, legsIdString.IndexOf(" "));
		//    feetIdString = feetIdString.Substring(0, feetIdString.IndexOf(" "));
		//    finger1IdString = finger1IdString.Substring(0, finger1IdString.IndexOf(" "));
		//    finger2IdString = finger2IdString.Substring(0, finger2IdString.IndexOf(" "));
		//    trinket1IdString = trinket1IdString.Substring(0, trinket1IdString.IndexOf(" "));
		//    trinket2IdString = trinket2IdString.Substring(0, trinket2IdString.IndexOf(" "));
		//    weaponIdString = weaponIdString.Substring(0, weaponIdString.IndexOf(" "));
		//    idolIdString = idolIdString.Substring(0, idolIdString.IndexOf(" "));
		//    int headId = headIdString.Length == 0 ? 0 : int.Parse(headIdString);
		//    int neckId = neckIdString.Length == 0 ? 0 : int.Parse(neckIdString);
		//    int shouldersId = shouldersIdString.Length == 0 ? 0 : int.Parse(shouldersIdString);
		//    int backId = backIdString.Length == 0 ? 0 : int.Parse(backIdString);
		//    int chestId = chestIdString.Length == 0 ? 0 : int.Parse(chestIdString);
		//    int shirtId = shirtIdString.Length == 0 ? 0 : int.Parse(shirtIdString);
		//    int tabardId = tabardIdString.Length == 0 ? 0 : int.Parse(tabardIdString);
		//    int wristId = wristIdString.Length == 0 ? 0 : int.Parse(wristIdString);
		//    int handsId = handsIdString.Length == 0 ? 0 : int.Parse(handsIdString);
		//    int waistId = waistIdString.Length == 0 ? 0 : int.Parse(waistIdString);
		//    int legsId = legsIdString.Length == 0 ? 0 : int.Parse(legsIdString);
		//    int feetId = feetIdString.Length == 0 ? 0 : int.Parse(feetIdString);
		//    int finger1Id = finger1IdString.Length == 0 ? 0 : int.Parse(finger1IdString);
		//    int finger2Id = finger2IdString.Length == 0 ? 0 : int.Parse(finger2IdString);
		//    int trinket1Id = trinket1IdString.Length == 0 ? 0 : int.Parse(trinket1IdString);
		//    int trinket2Id = trinket2IdString.Length == 0 ? 0 : int.Parse(trinket2IdString);
		//    int weaponId = weaponIdString.Length == 0 ? 0 : int.Parse(weaponIdString);
		//    int idolId = idolIdString.Length == 0 ? 0 : int.Parse(idolIdString);
		//    */
		//    #endregion

		//    object[] headId = GetItemIdFromArmoryHtml(armoryHtml, 0, armoryDomain, realm, name, Item.ItemSlot.Head);
		//    object[] neckId = GetItemIdFromArmoryHtml(armoryHtml, 1, armoryDomain, realm, name, Item.ItemSlot.Neck);
		//    object[] shouldersId = GetItemIdFromArmoryHtml(armoryHtml, 2, armoryDomain, realm, name, Item.ItemSlot.Shoulders);
		//    object[] backId = GetItemIdFromArmoryHtml(armoryHtml, 14, armoryDomain, realm, name, Item.ItemSlot.Back);
		//    object[] chestId = GetItemIdFromArmoryHtml(armoryHtml, 4, armoryDomain, realm, name, Item.ItemSlot.Chest);
		//    object[] shirtId = GetItemIdFromArmoryHtml(armoryHtml, 3, armoryDomain, realm, name, Item.ItemSlot.Shirt);
		//    object[] tabardId = GetItemIdFromArmoryHtml(armoryHtml, 18, armoryDomain, realm, name, Item.ItemSlot.Tabard);
		//    object[] wristId = GetItemIdFromArmoryHtml(armoryHtml, 8, armoryDomain, realm, name, Item.ItemSlot.Wrist);
		//    object[] handsId = GetItemIdFromArmoryHtml(armoryHtml, 9, armoryDomain, realm, name, Item.ItemSlot.Hands);
		//    object[] waistId = GetItemIdFromArmoryHtml(armoryHtml, 5, armoryDomain, realm, name, Item.ItemSlot.Waist);
		//    object[] legsId = GetItemIdFromArmoryHtml(armoryHtml, 6, armoryDomain, realm, name, Item.ItemSlot.Legs);
		//    object[] feetId = GetItemIdFromArmoryHtml(armoryHtml, 7, armoryDomain, realm, name, Item.ItemSlot.Feet);
		//    object[] finger1Id = GetItemIdFromArmoryHtml(armoryHtml, 10, armoryDomain, realm, name, Item.ItemSlot.Finger);
		//    object[] finger2Id = GetItemIdFromArmoryHtml(armoryHtml, 11, armoryDomain, realm, name, Item.ItemSlot.Finger);
		//    object[] trinket1Id = GetItemIdFromArmoryHtml(armoryHtml, 12, armoryDomain, realm, name, Item.ItemSlot.Trinket);
		//    object[] trinket2Id = GetItemIdFromArmoryHtml(armoryHtml, 13, armoryDomain, realm, name, Item.ItemSlot.Trinket);
		//    object[] weaponId = GetItemIdFromArmoryHtml(armoryHtml, 15, armoryDomain, realm, name, Item.ItemSlot.Weapon);
		//    object[] idolId = GetItemIdFromArmoryHtml(armoryHtml, 17, armoryDomain, realm, name, Item.ItemSlot.Idol);

		//    CharacterRace race = armoryHtml.Contains("pinRace = \"Night Elf\"") ? CharacterRace.NightElf : CharacterRace.Tauren;

		//    Character character = new Character(name, realm, region, race, headId[0].ToString(), neckId[0].ToString(), shouldersId[0].ToString(),
		//        backId[0].ToString(), chestId[0].ToString(), shirtId[0].ToString(), tabardId[0].ToString(), wristId[0].ToString(), 
		//        handsId[0].ToString(), waistId[0].ToString(), legsId[0].ToString(), feetId[0].ToString(), finger1Id[0].ToString(),
		//        finger2Id[0].ToString(), trinket1Id[0].ToString(), trinket2Id[0].ToString(), weaponId[0].ToString(), idolId[0].ToString());

		//    character._headEnchant = (int)headId[1];
		//    character._shouldersEnchant = (int)shouldersId[1];
		//    character._backEnchant = (int)backId[1];
		//    character._chestEnchant = (int)chestId[1];
		//    character._wristEnchant = (int)wristId[1];
		//    character._handsEnchant = (int)handsId[1];
		//    character._legsEnchant = (int)legsId[1];
		//    character._feetEnchant = (int)feetId[1];
		//    character._weaponEnchant = (int)weaponId[1];

		//    return character;
		//}


		//private static object[] GetItemIdFromArmoryHtml(string armoryHtml, int itemsArrayIndex, string armoryDomain, string realm, string name, Item.ItemSlot slot)
		//{
		//    string itemsArrayKeyword = "itemsArray[" + itemsArrayIndex.ToString() + "]  = [";
		//    int index = armoryHtml.IndexOf(itemsArrayKeyword);
		//    string idString = index < 0 ? " " : armoryHtml.Substring(index + itemsArrayKeyword.Length, 50);
		//    idString = idString.Substring(0, idString.IndexOf(" "));
		//    int itemId = idString.Length == 0 ? 0 : int.Parse(idString);

		//    //Lookup gems from the armory
		//    WebClient client = new WebClient();
		//    string tooltipHtml = client.DownloadString(string.Format("http://armory.{0}.com/item-tooltip.xml?i={1}&r={2}&n={3}",
		//        armoryDomain, itemId, realm, name));
		//    string[] tooltipParse = tooltipHtml.Split(new string[] { "21x21/" }, StringSplitOptions.None);
		//    List<string> gemIds = new List<string>();
		//    foreach (string tooltipPart in tooltipParse)
		//    {
		//        if (tooltipPart != tooltipParse[0])
		//        {
		//            string gemIconPath = tooltipPart.Substring(0, tooltipPart.IndexOf(".png") + 4);
		//            string gemStatsCombined = tooltipPart.Substring(tooltipPart.IndexOf(".png") + 6);
		//            gemStatsCombined = gemStatsCombined.Substring(0, gemStatsCombined.IndexOf("<br>"));
		//            string[] gemStatsStrings = gemStatsCombined.Split(new string[] { " and ", " &amp; " }, StringSplitOptions.None);
		//            Stats gemStats = new Stats(0, 0, 0, 0, 0, 0, 0);
		//            foreach (string gemStatsString in gemStatsStrings)
		//            {
		//                int statNumber = 0;
		//                try
		//                {
		//                    statNumber = int.Parse(gemStatsString.Split(' ')[0].TrimStart('+'));
		//                } catch { }
		//                string statName = gemStatsString.Split(' ')[1];
		//                switch (statName)
		//                {
		//                    case "Agility":
		//                        gemStats.Agility += statNumber;
		//                        break;
		//                    case "Armor":
		//                        gemStats.Armor += statNumber;
		//                        break;
		//                    case "Defense":
		//                        gemStats.DefenseRating += statNumber;
		//                        break;
		//                    case "Dodge":
		//                        gemStats.DodgeRating += statNumber;
		//                        break;
		//                    case "Health":
		//                        gemStats.Health += statNumber;
		//                        break;
		//                    case "Resilience":
		//                        gemStats.Resilience += statNumber;
		//                        break;
		//                    case "Stamina":
		//                        gemStats.Stamina += statNumber;
		//                        break;
		//                }
		//            }


		//            Item.ItemSlot gemColor = Item.ItemSlot.Meta;
		//            if (gemIconPath.Contains("crimsonspinel") || gemIconPath.Contains("ruby") ||
		//                gemIconPath.Contains("bloodstone") || gemIconPath.Contains("bloodgem"))
		//                gemColor = Item.ItemSlot.Red;
		//            if (gemIconPath.Contains("empyreansapphire") || gemIconPath.Contains("starofelune") ||
		//                gemIconPath.Contains("azuredraenite") || gemIconPath.Contains("crystal_03"))
		//                gemColor = Item.ItemSlot.Blue;
		//            if (gemIconPath.Contains("seasprayemerald") || gemIconPath.Contains("talasite") ||
		//                gemIconPath.Contains("deepperidot"))
		//                gemColor = Item.ItemSlot.Green;
		//            if (gemIconPath.Contains("pyrestone") || gemIconPath.Contains("nobletopaz") ||
		//                gemIconPath.Contains("opal") || gemIconPath.Contains("flamespessarite"))
		//                gemColor = Item.ItemSlot.Orange;
		//            if (gemIconPath.Contains("shadowsongamethyst") || gemIconPath.Contains("nightseye") ||
		//                gemIconPath.Contains("pearl") || gemIconPath.Contains("ebondraenite") || gemIconPath.Contains("sapphire_02"))
		//                gemColor = Item.ItemSlot.Purple;
		//            if (gemIconPath.Contains("lionseye") || gemIconPath.Contains("_topaz") ||
		//                gemIconPath.Contains("dawnstone") || gemIconPath.Contains("goldendraenite"))
		//                gemColor = Item.ItemSlot.Yellow;

		//            Gem gem = ItemCache.FindGemByStats(gemStats, gemColor);
		//            if (gem == null)
		//            {
		//                Random r = new Random();
		//                int id = r.Next(99999);
		//                while (ItemCache.FindItemById(id) != null) id = r.Next(99999);

		//                gem = new Gem(gemStats.ToString(), id, gemIconPath, gemColor, gemStats);
		//                ItemCache.AddGem(gem);
		//            }
		//            gemIds.Add(gem.Id.ToString());
		//        }
		//    }
		//    while (gemIds.Count < 3) gemIds.Add("0");

		//    int enchantId = 0;

		//    if (tooltipHtml.Contains("<span class=\"bonusGreen\">"))
		//    {
		//        string enchantText = tooltipHtml.Substring(tooltipHtml.IndexOf("<span class=\"bonusGreen\">") + 25);
		//        enchantText = enchantText.Substring(0, enchantText.IndexOf("</span>"));

		//        List<Enchant> enchantsForSlot = null;

		//        //switch (slot)
		//        //{
		//        //    case Item.ItemSlot.Head:
		//        //        enchantsForSlot = Enchant.HeadEnchants;
		//        //        break;
		//        //    case Item.ItemSlot.Shoulders:
		//        //        enchantsForSlot = Enchant.ShouldersEnchants;
		//        //        break;
		//        //    case Item.ItemSlot.Back:
		//        //        enchantsForSlot = Enchant.BackEnchants;
		//        //        break;
		//        //    case Item.ItemSlot.Chest:
		//        //        enchantsForSlot = Enchant.ChestEnchants;
		//        //        break;
		//        //    case Item.ItemSlot.Wrist:
		//        //        enchantsForSlot = Enchant.WristEnchants;
		//        //        break;
		//        //    case Item.ItemSlot.Hands:
		//        //        enchantsForSlot = Enchant.HandsEnchants;
		//        //        break;
		//        //    case Item.ItemSlot.Legs:
		//        //        enchantsForSlot = Enchant.LegsEnchants;
		//        //        break;
		//        //    case Item.ItemSlot.Feet:
		//        //        enchantsForSlot = Enchant.FeetEnchants;
		//        //        break;
		//        //    case Item.ItemSlot.Weapon:
		//        //        enchantsForSlot = Enchant.WeaponEnchants;
		//        //        break;
		//        //}

		//        //if (enchantsForSlot != null)
		//        //    foreach (Enchant enchant in enchantsForSlot)
		//        //        if (enchantText == enchant.TextOnItem)
		//        //        {
		//        //            enchantId = enchantsForSlot.IndexOf(enchant);
		//        //            break;
		//        //        }
		//    }

		//    return new object[] { string.Format("{0}.{1}.{2}.{3}", itemId.ToString(), gemIds[0], gemIds[1], gemIds[2]), enchantId };
		//}
    }
}
