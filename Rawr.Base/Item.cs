using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.ComponentModel;

namespace Rawr
{
	#region Item
	[Serializable]
	public class Item : IComparable<Item>
	{
		[XmlElement("ItemLevel")]
		public int _itemLevel;

		[XmlElement("IconPath")]
		public string _iconPath;

		[XmlElement("Stats")]
		public Stats _stats = new Stats();

		[XmlElement("Quality")]
		public ItemQuality _quality;

		[DefaultValueAttribute("")]
		[XmlElement("SetName")]
		public string _setName;
		
		[XmlElement("Type")]
		public ItemType _type = ItemType.None;
		
		[DefaultValueAttribute(0)]
		[XmlElement("MinDamage")]
		public int _minDamage = 0;

		[DefaultValueAttribute(0)]
		[XmlElement("MaxDamage")]
		public int _maxDamage = 0;

		[DefaultValueAttribute(0)]
		[XmlElement("DamageType")]
		public ItemDamageType _damageType = ItemDamageType.Physical;

		[DefaultValueAttribute(0)]
		[XmlElement("Speed")]
		public float _speed = 0f;

		[DefaultValueAttribute("")]
		[XmlElement("RequiredClasses")]
		public string _requiredClasses;

		[DefaultValueAttribute(false)]
		[XmlElement("Unique")]
		public bool _unique;

		[DefaultValueAttribute(0)]
		[XmlElement("SocketColor1")]
		public ItemSlot _socketColor1;

		[DefaultValueAttribute(0)]
		[XmlElement("SocketColor2")]
		public ItemSlot _socketColor2;

		[DefaultValueAttribute(0)]
		[XmlElement("SocketColor3")]
		public ItemSlot _socketColor3;

		[XmlElement("SocketBonus")]
		public Stats _socketBonus = new Stats();

        [XmlElement("LocalizedName")]
        public string _localizedName;
        
        public ItemLocation LocationInfo
		{
			get
			{
				return LocationFactory.Lookup(Id);
			}
		}

        [XmlIgnore]
        public DateTime LastChange { get; set; }

        public void InvalidateCachedData()
        {
            Stats.InvalidateSparseData();
            SocketBonus.InvalidateSparseData();
            LastChange = DateTime.Now;
        }

        [XmlIgnore]
        public bool Invalid { get; set; }

        public void Delete()
        {
            Invalid = true;
        }

        private string _name;
        [XmlIgnore]
		public string Name
		{
			get 
            {
                if (!Rawr.Properties.GeneralSettings.Default.Locale.Equals("en") && _localizedName != null)
                {
                    return _localizedName;
                }
                else
                {
                    return _name;
                }
            }
			set
            {
                _name = value;
                UpdateGemInformation();
            }
		}

        [XmlElement("Name")]
        public string EnglishName
        {
            get
            { 
                return _name; 
            }
            set
            { 
                _name = value;
                UpdateGemInformation();
            }
        }

        private int _id;
        public int Id
		{
			get 
            {
                return _id; 
            }
			set
			{
				_id = value;
                UpdateGemInformation();
			}
		}

		[XmlIgnore]
		public int ItemLevel
		{
			get { return _itemLevel; }
			set
			{
				_itemLevel = value;
			}
		}
		[XmlIgnore]
		public string IconPath
		{
			get { return _iconPath; }
			set { _iconPath = value; }
		}

        private ItemSlot _slot;
		public ItemSlot Slot
		{
			get 
            { 
                return _slot; 
            }
			set 
            {
                _slot = value;
                UpdateGemInformation();
            }
		}

		/// <summary>
		/// String version of Slot, to facilitate databinding.
		/// </summary>
		[XmlIgnore]
		public string SlotString
		{
			get 
            { 
                return _slot.ToString(); 
            }
			set 
            { 
                _slot = (ItemSlot)Enum.Parse(typeof(ItemSlot), value);
                UpdateGemInformation();
            }
		}

		[XmlIgnore]
		public Stats Stats
		{
			get { return _stats; }
			set { _stats = value; }
		}
		[XmlIgnore]
		public ItemQuality Quality
		{
			get
			{
				return _quality;
			}
			set
			{
				_quality = value;
			}
		}
		[XmlIgnore]
		public string SetName
		{
			get
			{
				return _setName;
			}
			set
			{
				_setName = value;
			}
		}
		[XmlIgnore]
		public ItemType Type
		{
			get
			{
				return _type;
			}
			set
			{
				_type = value;
			}
		}
		/// <summary>
		/// String version of Type, to facilitate databinding
		/// </summary>
		[XmlIgnore]
		public string TypeString
		{
			get { return _type.ToString(); }
			set { _type = (ItemType)Enum.Parse(typeof(ItemType), value); }
		}
		[XmlIgnore]
		public int MinDamage
		{
			get
			{
				return _minDamage;
			}
			set
			{
				_minDamage = value;
			}
		}
		[XmlIgnore]
		public int MaxDamage
		{
			get
			{
				return _maxDamage;
			}
			set
			{
				_maxDamage = value;
			}
		}
		[XmlIgnore]
		public ItemDamageType DamageType
		{
			get
			{
				return _damageType;
			}
			set
			{
				_damageType = value;
			}
		}
		[XmlIgnore]
		public float Speed
		{
			get
			{
				return _speed;
			}
			set
			{
				_speed = value;
			}
		}
		[XmlIgnore]
		public float DPS
		{
			get
			{
				if (Speed == 0f) return 0f;
				else return ((float)(MinDamage + MaxDamage) * 0.5f) / Speed;
			}
		}
		[XmlIgnore]
		public string RequiredClasses
		{
			get
			{
				return _requiredClasses;
			}
			set
			{
				_requiredClasses = value;
			}
		}
		[XmlIgnore]
		public bool Unique
		{
			get
			{
				return _unique;
			}
			set
			{
				_unique = value;
			}
		}
		[XmlIgnore]
		public ItemSlot SocketColor1
		{
			get { return _socketColor1; }
			set { _socketColor1 = value; }
		}
		[XmlIgnore]
		public ItemSlot SocketColor2
		{
			get { return _socketColor2; }
			set { _socketColor2 = value; }
		}
		[XmlIgnore]
		public ItemSlot SocketColor3
		{
			get { return _socketColor3; }
			set { _socketColor3 = value; }
		}
		public ItemSlot GetSocketColor(int index)
		{
			switch (index)
			{
				case 1:
					return SocketColor1;
				case 2:
					return SocketColor2;
				case 3:
					return SocketColor3;
				default:
					return ItemSlot.None;
			}
		}
		[XmlIgnore]
		public string SocketColor1String
		{
			get { return _socketColor1.ToString(); }
			set { _socketColor1 = (ItemSlot)Enum.Parse(typeof(ItemSlot), value); }
		}
		[XmlIgnore]
		public string SocketColor2String
		{
			get { return _socketColor2.ToString(); }
			set { _socketColor2 = (ItemSlot)Enum.Parse(typeof(ItemSlot), value); }
		}
		[XmlIgnore]
		public string SocketColor3String
		{
			get { return _socketColor3.ToString(); }
			set { _socketColor3 = (ItemSlot)Enum.Parse(typeof(ItemSlot), value); }
		}
		[XmlIgnore]
		public Stats SocketBonus
		{
			get { return _socketBonus; }
			set { _socketBonus = value; }
		}
        [XmlIgnore]
        public string LocalizedName
        {
            get { return _localizedName; }
            set { _localizedName = value; }
        }

        public enum ItemDamageType
		{
			Physical = 0,
			Holy,
			Fire,
			Nature,
			Frost,
			Shadow,
			Arcane
		}

		public enum ItemQuality
		{
			Temp = -1,
			Poor = 0,
			Common,
			Uncommon,
			Rare,
			Epic,
			Legendary,
			Artifact,
			Heirloom
		}

		public enum ItemType
		{
			None,

			Cloth,
			Leather,
			Mail,
			Plate,

			Dagger,
			FistWeapon,
			OneHandAxe,
			TwoHandAxe,
			OneHandMace,
			TwoHandMace,
			OneHandSword,
			TwoHandSword,
			Polearm,
			Staff,
			Shield,

			Bow,
			Crossbow,
			Gun,
			Wand,
			Thrown,

			Idol,
			Libram,
			Totem,
			Sigil,

			Arrow,
			Bullet,
			Quiver,
			AmmoPouch
		}

		public enum ItemSlot
		{
			None,

			Head,
			Neck,
			Shoulders,
			Back,
			Chest,
			Shirt,
			Tabard,
			Wrist,
			Hands,
			Waist,
			Legs,
			Feet,
			Finger,
			Trinket,
			OneHand,
			TwoHand,
			MainHand,
			OffHand,
			Ranged,
			Projectile,
			ProjectileBag,

			Meta,
			Red,
			Orange,
			Yellow,
			Green,
			Blue,
			Purple,
			Prismatic

			//None = 0,
			//Head = 1,
			//Neck = 2,
			//Shoulders = 3,
			//Shirt = 4,
			//Chest = 5,
			//Waist = 6,
			//Legs = 7,
			//Feet = 8,
			//Wrist = 9,
			//Hands = 10,
			//Finger = 11,
			//Trinket = 12,
			//OneHand = 13,
			//OffHand = 14,
			//Ranged = 15,
			//Back = 16,
			//TwoHand = 17,

			//Tabard = 19,
			//Robe = 20,
			//MainHand = 21,
			//OffHandB = 22,


			//Thrown = 25,
			//Wand = 26,
			//Relic = 28,

			//Weapon = 97,

			//Meta = 101,
			//Red = 102,
			//Orange = 103,
			//Yellow = 104,
			//Green = 105,
			//Prismatic = 106,
			//Purple = 107,
			//Blue = 108
		}

		public static Item.ItemSlot GetItemSlotByCharacterSlot(Character.CharacterSlot slot)
		{
			switch (slot)
			{
				case Character.CharacterSlot.Projectile: return ItemSlot.Projectile;
				case Character.CharacterSlot.Head: return ItemSlot.Head;
				case Character.CharacterSlot.Neck: return ItemSlot.Neck;
				case Character.CharacterSlot.Shoulders: return ItemSlot.Shoulders;
				case Character.CharacterSlot.Chest: return ItemSlot.Chest;
				case Character.CharacterSlot.Waist: return ItemSlot.Waist;
				case Character.CharacterSlot.Legs: return ItemSlot.Legs;
				case Character.CharacterSlot.Feet: return ItemSlot.Feet;
				case Character.CharacterSlot.Wrist: return ItemSlot.Wrist;
				case Character.CharacterSlot.Hands: return ItemSlot.Hands;
				case Character.CharacterSlot.Finger1: return ItemSlot.Finger;
				case Character.CharacterSlot.Finger2: return ItemSlot.Finger;
				case Character.CharacterSlot.Trinket1: return ItemSlot.Trinket;
				case Character.CharacterSlot.Trinket2: return ItemSlot.Trinket;
				case Character.CharacterSlot.Back: return ItemSlot.Back;
				case Character.CharacterSlot.MainHand: return ItemSlot.MainHand;
				case Character.CharacterSlot.OffHand: return ItemSlot.OffHand;
				case Character.CharacterSlot.Ranged: return ItemSlot.Ranged;
				case Character.CharacterSlot.ProjectileBag: return ItemSlot.ProjectileBag;
                //case Character.CharacterSlot.ExtraWristSocket: return ItemSlot.Prismatic;
                //case Character.CharacterSlot.ExtraHandsSocket: return ItemSlot.Prismatic;
                //case Character.CharacterSlot.ExtraWaistSocket: return ItemSlot.Prismatic;
				case Character.CharacterSlot.Tabard: return ItemSlot.Tabard;
				case Character.CharacterSlot.Shirt: return ItemSlot.Shirt;
				case Character.CharacterSlot.Gems: return ItemSlot.Prismatic;
				case Character.CharacterSlot.Metas: return ItemSlot.Meta;
				default: return ItemSlot.None;
			}
		}

        private bool _isGem;
		public bool IsGem
		{
			get
			{
				return _isGem;
			}
		}

        private bool _isRedGem;
        public bool IsRedGem
        {
            get
            {
                return _isRedGem;
            }
        }

        private bool _isYellowGem;
        public bool IsYellowGem
        {
            get
            {
                return _isYellowGem;
            }
        }

        private bool _isBlueGem;
        public bool IsBlueGem
        {
            get
            {
                return _isBlueGem;
            }
        }

        private bool _isJewelersGem;
		public bool IsJewelersGem
		{
			get
			{
                return _isJewelersGem;
			}
		}

        private bool _isStormjewel;
		public bool IsStormjewel
		{
			get
			{
                return _isStormjewel;
			}
		}

        public bool IsLimitedGem
        {
            get
            {
                return _isGem && (_isStormjewel || _isJewelersGem || Unique);
            }
        }

        private void UpdateGemInformation()
        {
            _isGem = Slot == ItemSlot.Meta || Slot == ItemSlot.Blue || Slot == ItemSlot.Green || Slot == ItemSlot.Orange || Slot == ItemSlot.Prismatic || Slot == ItemSlot.Purple || Slot == ItemSlot.Red || Slot == ItemSlot.Yellow;
            _isStormjewel = _isGem && _name.EndsWith("Stormjewel");
            _isJewelersGem = Slot == ItemSlot.Prismatic && (Id == 42142 || Id == 36766 || Id == 42148 || Id == 42143 || Id == 42152 || Id == 42153 || Id == 42146 || Id == 42158 || Id == 42154 || Id == 42150 || Id == 42156 || Id == 42144 || Id == 42149 || Id == 36767 || Id == 42145 || Id == 42155 || Id == 42151 || Id == 42157);
            _isRedGem = _isGem && Item.GemMatchesSlot(this, Item.ItemSlot.Red);
            _isYellowGem = _isGem && Item.GemMatchesSlot(this, Item.ItemSlot.Yellow);
            _isBlueGem = _isGem && Item.GemMatchesSlot(this, Item.ItemSlot.Blue);
        }

		public Item() { }
		public Item(string name, ItemQuality quality, ItemType type, int id, string iconPath, ItemSlot slot, string setName, bool unique, Stats stats, Stats socketBonus, ItemSlot socketColor1, ItemSlot socketColor2, ItemSlot socketColor3, int minDamage, int maxDamage, ItemDamageType damageType, float speed, string requiredClasses)
		{
			_name = name;
			_id = id;
			_iconPath = iconPath;
			_slot = slot;
			_stats = stats;
			_socketBonus = socketBonus;
			_socketColor1 = socketColor1;
			_socketColor2 = socketColor2;
			_socketColor3 = socketColor3;
			_setName = setName;
			_quality = quality;
			_type = type;
			_minDamage = minDamage;
			_maxDamage = maxDamage;
			_damageType = damageType;
			_speed = speed;
			_requiredClasses = requiredClasses;
			_unique = unique;
            UpdateGemInformation();
		}

		public Item Clone()
		{
			return new Item()
			{
				Name = this.Name,
				Quality = this.Quality,
				Id = this.Id,
				IconPath = this.IconPath,
				Slot = this.Slot,
				Stats = this.Stats.Clone(),
				SocketBonus = this.SocketBonus.Clone(),
				SocketColor1 = this.SocketColor1,
				SocketColor2 = this.SocketColor2,
				SocketColor3 = this.SocketColor3,
				SetName = this.SetName,
				Type = this.Type,
				MinDamage = this.MinDamage,
				MaxDamage = this.MaxDamage,
				DamageType = this.DamageType,
				Speed = this.Speed,
				RequiredClasses = this.RequiredClasses,
				Unique = this.Unique,
                LocalizedName = this.LocalizedName
			};
		}

		public override string ToString()
		{
			return this.Name + ": " + this.Stats.ToString();
		}

		public static bool GemMatchesSlot(Item gem, ItemSlot slotColor)
		{
			switch (slotColor)
			{
				case ItemSlot.Red:
					return gem != null && (gem.Slot == ItemSlot.Red || gem.Slot == ItemSlot.Orange || gem.Slot == ItemSlot.Purple || gem.Slot == ItemSlot.Prismatic);
				case ItemSlot.Yellow:
                    return gem != null && (gem.Slot == ItemSlot.Yellow || gem.Slot == ItemSlot.Orange || gem.Slot == ItemSlot.Green || gem.Slot == ItemSlot.Prismatic);
				case ItemSlot.Blue:
                    return gem != null && (gem.Slot == ItemSlot.Blue || gem.Slot == ItemSlot.Green || gem.Slot == ItemSlot.Purple || gem.Slot == ItemSlot.Prismatic);
				case ItemSlot.Meta:
					return gem != null && (gem.Slot == ItemSlot.Meta);
				default:
                    return gem == null || gem.Slot != ItemSlot.Meta;
			}
		}

		public static SortedList<Item.ItemSlot, Character.CharacterSlot> DefaultSlotMap { get; private set; }
		static Item()
		{
			SortedList<Item.ItemSlot, Character.CharacterSlot> list = new SortedList<Item.ItemSlot, Character.CharacterSlot>();

			foreach (Item.ItemSlot iSlot in Enum.GetValues(typeof(Item.ItemSlot)))
			{
				list[iSlot] = Character.CharacterSlot.None;
			}
			list[Item.ItemSlot.Head] = Character.CharacterSlot.Head;
			list[Item.ItemSlot.Neck] = Character.CharacterSlot.Neck;
			list[Item.ItemSlot.Shoulders] = Character.CharacterSlot.Shoulders;
			list[Item.ItemSlot.Back] = Character.CharacterSlot.Back;
			list[Item.ItemSlot.Chest] = Character.CharacterSlot.Chest;
			list[Item.ItemSlot.Shirt] = Character.CharacterSlot.Shirt;
			list[Item.ItemSlot.Tabard] = Character.CharacterSlot.Tabard;
			list[Item.ItemSlot.Wrist] = Character.CharacterSlot.Wrist;
			list[Item.ItemSlot.Hands] = Character.CharacterSlot.Hands;
			list[Item.ItemSlot.Waist] = Character.CharacterSlot.Waist;
			list[Item.ItemSlot.Legs] = Character.CharacterSlot.Legs;
			list[Item.ItemSlot.Feet] = Character.CharacterSlot.Feet;
			list[Item.ItemSlot.Finger] = Character.CharacterSlot.Finger1;
			list[Item.ItemSlot.Trinket] = Character.CharacterSlot.Trinket1;
			list[Item.ItemSlot.OneHand] = Character.CharacterSlot.MainHand;
			list[Item.ItemSlot.TwoHand] = Character.CharacterSlot.MainHand;
			list[Item.ItemSlot.MainHand] = Character.CharacterSlot.MainHand;
			list[Item.ItemSlot.OffHand] = Character.CharacterSlot.OffHand;
			list[Item.ItemSlot.Ranged] = Character.CharacterSlot.Ranged;
			list[Item.ItemSlot.Projectile] = Character.CharacterSlot.Projectile;
			list[Item.ItemSlot.ProjectileBag] = Character.CharacterSlot.ProjectileBag;
			list.TrimExcess();
			DefaultSlotMap = list;
		}

		public bool FitsInSlot(Character.CharacterSlot charSlot)
		{
			//And if I fell with all the strength I held inside...
			switch (charSlot)
			{
				case Character.CharacterSlot.Head:
					return this.Slot == ItemSlot.Head;
				case Character.CharacterSlot.Neck:
					return this.Slot == ItemSlot.Neck;
				case Character.CharacterSlot.Shoulders:
					return this.Slot == ItemSlot.Shoulders;
				case Character.CharacterSlot.Back:
					return this.Slot == ItemSlot.Back;
				case Character.CharacterSlot.Chest:
					return this.Slot == ItemSlot.Chest;
				case Character.CharacterSlot.Shirt:
					return this.Slot == ItemSlot.Shirt;
				case Character.CharacterSlot.Tabard:
					return this.Slot == ItemSlot.Tabard;
				case Character.CharacterSlot.Wrist:
					return this.Slot == ItemSlot.Wrist;
				case Character.CharacterSlot.Hands:
					return this.Slot == ItemSlot.Hands;
				case Character.CharacterSlot.Waist:
					return this.Slot == ItemSlot.Waist;
				case Character.CharacterSlot.Legs:
					return this.Slot == ItemSlot.Legs;
				case Character.CharacterSlot.Feet:
					return this.Slot == ItemSlot.Feet;
				case Character.CharacterSlot.Finger1:
				case Character.CharacterSlot.Finger2:
					return this.Slot == ItemSlot.Finger;
				case Character.CharacterSlot.Trinket1:
				case Character.CharacterSlot.Trinket2:
					return this.Slot == ItemSlot.Trinket;
				case Character.CharacterSlot.MainHand:
					return this.Slot == ItemSlot.TwoHand || this.Slot == ItemSlot.OneHand || this.Slot == ItemSlot.MainHand;
				case Character.CharacterSlot.OffHand:
					return this.Slot == ItemSlot.OneHand || this.Slot == ItemSlot.OffHand;
				case Character.CharacterSlot.Ranged:
					return this.Slot == ItemSlot.Ranged;
				case Character.CharacterSlot.Projectile:
					return this.Slot == ItemSlot.Projectile;
				case Character.CharacterSlot.ProjectileBag:
					return this.Slot == ItemSlot.ProjectileBag;
                //case Character.CharacterSlot.ExtraWristSocket:
                //case Character.CharacterSlot.ExtraHandsSocket:
                //case Character.CharacterSlot.ExtraWaistSocket:
				case Character.CharacterSlot.Gems:
					return this.Slot == ItemSlot.Red || this.Slot == ItemSlot.Blue || this.Slot == ItemSlot.Yellow
						|| this.Slot == ItemSlot.Purple || this.Slot == ItemSlot.Green || this.Slot == ItemSlot.Orange
						|| this.Slot == ItemSlot.Prismatic;
				case Character.CharacterSlot.Metas:
					return this.Slot == ItemSlot.Meta;
				default:
					return false;
			}
			//I wouldn't be out here... alone tonight
		}

		public bool FitsInSlot(Character.CharacterSlot charSlot, Character character)
		{
			return Calculations.ItemFitsInSlot(this, character, charSlot);
		}

		public bool MeetsRequirements(Character character)
		{
			bool temp;
			return MeetsRequirements(character, out temp);
		}

        public static bool OptimizerManagedVolatiliy { get; set; }

		/// <summary>
		/// Checks whether item meets the requirements to activate its stats.
		/// </summary>
		/// <param name="character">Character for which we are checking the requirements.</param>
		/// <param name="volatileRequirements">This is set to true for items that depend on requirements not local to the item itself.</param>
		/// <returns>True if the item meets the requirements.</returns>
		public bool MeetsRequirements(Character character, out bool volatileRequirements)
		{
			volatileRequirements = false;
			bool meetsRequirements = true;

			if (this.Slot == ItemSlot.Meta)
			{
				#region Metagem Requirements
				volatileRequirements = true;
				if (character == null || !character.EnforceGemRequirements) return true;

				int redGems = 0, yellowGems = 0, blueGems = 0;
				if (character != null)
				{
					/*redGems = character.GetGemColorCount(ItemSlot.Red);
					yellowGems = character.GetGemColorCount(ItemSlot.Yellow);
					blueGems = character.GetGemColorCount(ItemSlot.Blue);*/
					redGems = character.RedGemCount;
					yellowGems = character.YellowGemCount;
					blueGems = character.BlueGemCount;
				}

				//TODO: Make this dynamic, by loading the gem requirements from the armory
				switch (this.Id)
				{
					case 25899:
					case 25890:
					case 25901:
					case 32409:
					case 32410:
						volatileRequirements = true; //2 of each
						meetsRequirements = redGems >= 2 && yellowGems >= 2 && blueGems >= 2;
						break;
					case 41307:
					case 41401:
					case 41400:
					case 41375:
					case 44078:
					case 44089:
					case 41382:
					case 41398:
						volatileRequirements = true; //1 of each
						meetsRequirements = redGems >= 1 && yellowGems >= 1 && blueGems >= 1;
						break;
					case 25897:
						volatileRequirements = true; //More reds than blues
						meetsRequirements = redGems > blueGems;
						break;
					case 25895:
						volatileRequirements = true; //More reds than yellows
						meetsRequirements = redGems > yellowGems;
						break;
					case 25893:
					case 32640:
						volatileRequirements = true; //More blues than yellows
						meetsRequirements = blueGems > yellowGems;
						break;
					case 34220:
					case 41285:
						volatileRequirements = true; //2 blues
						meetsRequirements = blueGems >= 2;
						break;
					case 41376:
						volatileRequirements = true; //2 reds
						meetsRequirements = redGems >= 2;
						break;
					case 25896:
					case 44087:
						volatileRequirements = true; //3 blues
						meetsRequirements = blueGems >= 3;
						break;
					case 25898:
						volatileRequirements = true; //5 blues
						meetsRequirements = blueGems >= 5;
						break;
					case 32641:
						volatileRequirements = true; //exactly 3 yellows
						meetsRequirements = yellowGems == 3;
						break;
					case 41333:
						volatileRequirements = true; //3 red
						meetsRequirements = redGems >= 3;
						break;
					case 25894:
					case 28557:
					case 28556:
					case 41339:
					case 44076:
						volatileRequirements = true; //2 yellows, 1 red
						meetsRequirements = yellowGems >= 2 && redGems >= 1;
						break;
					case 35501:
					case 44088:
						volatileRequirements = true; //1 yellow, 2 blue
						meetsRequirements = yellowGems >= 1 && blueGems >= 2;
						break;
					case 41378:
					case 44084:
					case 41381:
						volatileRequirements = true; //2 yellow, 1 blue
						meetsRequirements = yellowGems >= 2 && blueGems >= 1;
						break;
					case 41380:
					case 41377:
					case 44082:
					case 41385:
						volatileRequirements = true; //2 blue, 1 red
						meetsRequirements = blueGems >= 2 && redGems >= 1;
						break;
					case 41335:
					case 41389:
						volatileRequirements = true; //2 red, 1 yellow
						meetsRequirements = redGems >= 2 && yellowGems >= 1;
						break;
					case 41379:
					case 44081:
					case 41396:
					case 41395:
						volatileRequirements = true; //2 red, 1 blue
						meetsRequirements = redGems >= 2 && blueGems >= 1;
						break;
					default:
						meetsRequirements = true;
						break;
				}
				#endregion
			}
            else if (!OptimizerManagedVolatiliy)
			{
				#region Gem Requirements
				if (IsJewelersGem)
				{
					volatileRequirements = true;
					if (character == null) return true;
					meetsRequirements = character.JewelersGemCount <= 3;
				}
				else if (IsStormjewel)
				{
					volatileRequirements = true;
					if (character == null || !character.EnforceGemRequirements) return true;
					meetsRequirements = character.StormjewelCount <= 1;
				}
				else if (Unique)
				{
					volatileRequirements = true;
					if (character == null || !character.EnforceGemRequirements) return true;
					meetsRequirements = character.GetGemIdCount(Id) <= 1;
				}
				else
				{
					volatileRequirements = false;
					meetsRequirements = true;
				}
				#endregion
			}

			return meetsRequirements;
		}


		public static Item LoadFromId(int id) { return LoadFromId(id, false, true, false); }
        public static Item LoadFromId(int id, bool forceRefresh, bool raiseEvent, bool useWowhead) { return LoadFromId(id, forceRefresh, raiseEvent, useWowhead, "en"); }
        public static Item LoadFromId(int id, bool forceRefresh, bool raiseEvent, bool useWowhead, string locale) { return LoadFromId(id, forceRefresh, raiseEvent, useWowhead, locale, "www"); }
        public static Item LoadFromId(int id, bool forceRefresh, bool raiseEvent, bool useWowhead, string locale, string wowheadSite)
		{
			Item cachedItem = ItemCache.FindItemById(id);
			if (cachedItem != null && !forceRefresh) return cachedItem;
			else
			{
				Item newItem = useWowhead ? Wowhead.GetItem(wowheadSite, id.ToString(), false) : Armory.GetItem(id);
                if (newItem != null)
                {
                    if (!locale.Equals("en"))
                    {
                        Item localItem = Wowhead.GetItem(id, false, locale);
                        if (localItem != null)
                            newItem.LocalizedName = localItem.Name;
                    }
                    ItemCache.AddItem(newItem, raiseEvent);
                }
				return ItemCache.FindItemById(id);
			}
		}

		/// <summary>
		/// Used by optimizer to cache dictionary search result
		/// </summary>
        [XmlIgnore]
        internal Optimizer.ItemAvailabilityInformation AvailabilityInformation;

		#region IComparable<Item> Members

		public int CompareTo(Item other)
		{
			return ToString().CompareTo(other.ToString());
		}

		#endregion
	}
	#endregion

	#region ItemInstance
    // to make our lives more tolerable, ItemInstance is exactly what it implies
    // it is a single instance of an item, it is not to be shared between multiple characters
    // or whatever, at least if you don't know what you are doing
    // if for whatever reason you reuse the same instance treat it as read only
	[Serializable]
	public class ItemInstance : IComparable<ItemInstance>
	{
		[XmlElement("Id")]
		public int _id;
		[DefaultValueAttribute(0)]
		[XmlElement("Gem1Id")]
		public int _gem1Id;
		[DefaultValueAttribute(0)]
		[XmlElement("Gem2Id")]
		public int _gem2Id;
		[DefaultValueAttribute(0)]
		[XmlElement("Gem3Id")]
		public int _gem3Id;
		[DefaultValueAttribute(0)]
		[XmlElement("EnchantId")]
		public int _enchantId;

		[XmlIgnore]
		public int Id
		{
			get { return _id; }
            set
            {
                _id = value; OnIdsChanged();
            }
		}
		[XmlIgnore]
		public int Gem1Id
		{
			get { return _gem1Id; }
            set
            {
                _gem1Id = value; OnIdsChanged();
            }
		}
		[XmlIgnore]
		public int Gem2Id
		{
			get { return _gem2Id; }
            set
            {
                _gem2Id = value; OnIdsChanged();
            }
		}
		[XmlIgnore]
		public int Gem3Id
		{
			get { return _gem3Id; }
            set
            {
                _gem3Id = value; OnIdsChanged();
            }
		}
		[XmlIgnore]
		public int EnchantId
		{
			get { return _enchantId; }
            set
            {
                _enchantId = value; OnIdsChanged();
            }
		}

        private void UpdateJewelerCount()
        {
            int jewelerCount = 0;
            for (int index = 1; index <= 3; index++)
            {
                Item gem = GetGem(index);
                if (gem != null && gem.IsJewelersGem)
                {
                    jewelerCount++;
                }
            }
            JewelerCount = jewelerCount;
        }

        [XmlIgnore]
        public int JewelerCount { get; private set; }

        [DefaultValueAttribute(false)]
        public bool ForceDisplay { get; set; }

        public event EventHandler IdsChanged;
        private void OnIdsChanged()
        {
            _gemmedId = string.Empty;
            InvalidateCachedData();
            UpdateJewelerCount();
            if (IdsChanged != null) IdsChanged(this, null);
        }

        [XmlIgnore]
        private Item _itemCached = null;
        [XmlIgnore]
        public Item Item
        {
            get
            {
                if (Id == 0) return null;
                if (_itemCached == null || _itemCached.Id != Id || _itemCached.Invalid)
                {
                    _itemCached = Item.LoadFromId(Id);
                }
                return _itemCached;
            }
            set
            {
                if (value == null)
                    Id = 0;
                else
                    Id = value.Id;
                _itemCached = value;
            }
        }

		[XmlIgnore]
		private Item _gem1Cached = null;
		[XmlIgnore]
		public Item Gem1
		{
			get
			{
				if (Gem1Id == 0) return null;
				if (_gem1Cached == null || _gem1Cached.Id != Gem1Id || _gem1Cached.Invalid)
				{
					_gem1Cached = Item.LoadFromId(Gem1Id);
				}
				return _gem1Cached;
			}
			set
			{
				if (value == null)
					Gem1Id = 0;
				else
					Gem1Id = value.Id;
				_gem1Cached = value;
            }
		}

		[XmlIgnore]
		private Item _gem2Cached = null;
		[XmlIgnore]
		public Item Gem2
		{
			get
			{
				if (Gem2Id == 0) return null;
				if (_gem2Cached == null || _gem2Cached.Id != Gem2Id || _gem2Cached.Invalid)
				{
					_gem2Cached = Item.LoadFromId(Gem2Id);
				}
				return _gem2Cached;
			}
			set
			{
				if (value == null)
					Gem2Id = 0;
				else
					Gem2Id = value.Id;
				_gem2Cached = value;
            }
		}

		[XmlIgnore]
		private Item _gem3Cached = null;
		[XmlIgnore]
		public Item Gem3
		{
			get
			{
				if (Gem3Id == 0) return null;
				if (_gem3Cached == null || _gem3Cached.Id != Gem3Id || _gem3Cached.Invalid)
				{
					_gem3Cached = Item.LoadFromId(Gem3Id);
				}
				return _gem3Cached;
			}
			set
			{
				if (value == null)
					Gem3Id = 0;
				else
					Gem3Id = value.Id;
				_gem3Cached = value;
            }
		}

		[XmlIgnore]
		private Enchant _enchantCached = null;
		[XmlIgnore]
		public Enchant Enchant
		{
		    get
		    {
		        if (_enchantCached == null || _enchantCached.Id != EnchantId)
		        {
		            _enchantCached = Enchant.FindEnchant(EnchantId, Item != null ? Item.Slot : Item.ItemSlot.None, null);
		        }
		        return _enchantCached;
		    }
		    set
		    {
		        if (value == null)
		            EnchantId = 0;
		        else
                    EnchantId = value.Id;
		        _enchantCached = value;
            }
		}

		// 1-based index
		public Item GetGem(int index)
		{
			switch (index)
			{
				case 1:
					return Gem1;
				case 2:
					return Gem2;
				case 3:
					return Gem3;
				default:
					return null;
			}
		}

		public void SetGem(int index, Item value)
		{
			switch (index)
			{
				case 1:
					Gem1 = value;
					break;
				case 2:
					Gem2 = value;
					break;
				case 3:
					Gem3 = value;
					break;
			}
		}

        [XmlIgnore]
		private string _gemmedId = string.Empty;
		[XmlIgnore]
		public string GemmedId
		{
			get
			{
				if (_gemmedId.Length == 0) // _gemmedId is never null
				{
					_gemmedId = string.Format("{0}.{1}.{2}.{3}.{4}", Id, Gem1Id, Gem2Id, Gem3Id, EnchantId);
				}
				return _gemmedId;
			}
            set
            {
                if (value == null) _gemmedId = string.Empty;
                else _gemmedId = value;
                string[] ids = _gemmedId.Split('.');
                _id = int.Parse(ids[0]);
                _gem1Id = ids.Length > 1 ? int.Parse(ids[1]) : 0;
                _gem2Id = ids.Length > 2 ? int.Parse(ids[2]) : 0;
                _gem3Id = ids.Length > 3 ? int.Parse(ids[3]) : 0;
                _enchantId = ids.Length > 4 ? int.Parse(ids[4]) : 0;
                OnIdsChanged();
            }
		}
		
		public ItemInstance() { }
		public ItemInstance(string gemmedId)
		{
			string[] ids = gemmedId.Split('.');
			_id = int.Parse(ids[0]);
			_gem1Id = ids.Length > 1 ? int.Parse(ids[1]) : 0;
			_gem2Id = ids.Length > 2 ? int.Parse(ids[2]) : 0;
			_gem3Id = ids.Length > 3 ? int.Parse(ids[3]) : 0;
			_enchantId = ids.Length > 4 ? int.Parse(ids[4]) : 0;
            UpdateJewelerCount();
		}
		public ItemInstance(int id, int gem1Id, int gem2Id, int gem3Id, int enchantId)
		{
			_id = id;
			_gem1Id = gem1Id;
			_gem2Id = gem2Id;
			_gem3Id = gem3Id;
			_enchantId = enchantId;
            UpdateJewelerCount();
        }
        public ItemInstance(Item item, Item gem1, Item gem2, Item gem3, Enchant enchant)
        {
            Item = item;
            Gem1 = gem1;
            Gem2 = gem2;
            Gem3 = gem3;
            Enchant = enchant;
        }

		public ItemInstance Clone()
		{
			return new ItemInstance()
			{
				Item = this.Item,
				Gem1 = this.Gem1,
				Gem2 = this.Gem2,
				Gem3 = this.Gem3,
				Enchant = this.Enchant
			};
		}

		public override string ToString()
		{
			string summary = this.Item.Name + ": ";
			summary += this.GetTotalStats().ToString();
			//summary += Stats.ToString();
			//summary += Sockets.ToString();
			if (summary.EndsWith(", ")) summary = summary.Substring(0, summary.Length - 2);

			if ((Item.SocketColor1 != Item.ItemSlot.None && Gem1Id == 0) ||
				(Item.SocketColor2 != Item.ItemSlot.None && Gem2Id == 0) ||
				(Item.SocketColor3 != Item.ItemSlot.None && Gem3Id == 0))
				summary += " [EMPTY SOCKETS]";

			return summary;
		}

		// caching policy: cache total stats only for items that don't have global requirements
		// value should not change if it relies on data other than from this item
		// assume there is no stat editing happening in code other than in item editor
		// invalidate on id changes, invalidate when item is opened for editing
		// invalidate all items when any gem is opened for editing
		// invalidate 
		private Stats cachedTotalStats = null;
        private DateTime cachedTime; 
		public void InvalidateCachedData()
		{
			cachedTotalStats = null;
		}

        public Stats GetTotalStats() { return AccumulateTotalStats(null, null); }
        public Stats GetTotalStats(Character character) { return AccumulateTotalStats(character, null); }
        public unsafe Stats AccumulateTotalStats(Character character, Stats unsafeStatsAccumulator)
		{
            if (cachedTotalStats != null && Item.LastChange <= cachedTime)
            {
                if (unsafeStatsAccumulator != null)
                {
                    unsafeStatsAccumulator.AccumulateUnsafe(cachedTotalStats);
                }
                return cachedTotalStats;
            }
			bool volatileGem = false, volatileItem = false;
            bool gem1 = false;
            bool gem2 = false;
            bool gem3 = false;
            bool eligibleForSocketBonus = Item.GemMatchesSlot(Gem1, Item.SocketColor1) &&
                                            Item.GemMatchesSlot(Gem2, Item.SocketColor2) &&
                                            Item.GemMatchesSlot(Gem3, Item.SocketColor3);
            if (Gem1 != null && Gem1.MeetsRequirements(character, out volatileGem)) gem1 = true;
            volatileItem = volatileItem || volatileGem;
            if (Gem2 != null && Gem2.MeetsRequirements(character, out volatileGem)) gem2 = true;
            volatileItem = volatileItem || volatileGem;
            if (Gem3 != null && Gem3.MeetsRequirements(character, out volatileGem)) gem3 = true;
            volatileItem = volatileItem || volatileGem;
            if (volatileItem && unsafeStatsAccumulator != null)
            {
                unsafeStatsAccumulator.AccumulateUnsafe(Item.Stats, true);
                if (gem1) unsafeStatsAccumulator.AccumulateUnsafe(Gem1.Stats, true);
                if (gem2) unsafeStatsAccumulator.AccumulateUnsafe(Gem2.Stats, true);
                if (gem3) unsafeStatsAccumulator.AccumulateUnsafe(Gem3.Stats, true);
                if (eligibleForSocketBonus) unsafeStatsAccumulator.AccumulateUnsafe(Item.SocketBonus, true);
                bool eligibleForEnchant = false;
                if (Enchant.Slot == Item.ItemSlot.OneHand)
                {
                    eligibleForEnchant = (this.Slot == Item.ItemSlot.OneHand ||
                                        (this.Slot == Item.ItemSlot.OffHand &&
                                            this.Type != Item.ItemType.Shield &&
                                            this.Type != Item.ItemType.None) ||
                                        this.Slot == Item.ItemSlot.MainHand ||
                                        this.Slot == Item.ItemSlot.TwoHand);
                }
                else if (Enchant.Slot == Item.ItemSlot.OffHand)
                {
                    eligibleForEnchant = this.Type == Item.ItemType.Shield;
                }
                else
                {
                    eligibleForEnchant = (Enchant.Slot == this.Slot);
                }
                if (eligibleForEnchant) unsafeStatsAccumulator.AccumulateUnsafe(Enchant.Stats, true);
                return null;
            }
            else
            {
                Stats totalItemStats = new Stats();
                fixed (float* pRawAdditiveData = totalItemStats._rawAdditiveData, pRawMultiplicativeData = totalItemStats._rawMultiplicativeData, pRawNoStackData = totalItemStats._rawNoStackData)
                {
                    totalItemStats.BeginUnsafe(pRawAdditiveData, pRawMultiplicativeData, pRawNoStackData);
                    totalItemStats.AccumulateUnsafe(Item.Stats, true);
                    if (gem1) totalItemStats.AccumulateUnsafe(Gem1.Stats, true);
                    if (gem2) totalItemStats.AccumulateUnsafe(Gem2.Stats, true);
                    if (gem3) totalItemStats.AccumulateUnsafe(Gem3.Stats, true);
                    if (eligibleForSocketBonus) totalItemStats.AccumulateUnsafe(Item.SocketBonus, true);
                    bool eligibleForEnchant = false;
                    if (Enchant.Slot == Item.ItemSlot.OneHand)
                    {
                        eligibleForEnchant = (this.Slot == Item.ItemSlot.OneHand ||
                                            (this.Slot == Item.ItemSlot.OffHand &&
                                                this.Type != Item.ItemType.Shield &&
                                                this.Type != Item.ItemType.None) ||
                                            this.Slot == Item.ItemSlot.MainHand ||
                                            this.Slot == Item.ItemSlot.TwoHand);
                    }
                    else if (Enchant.Slot == Item.ItemSlot.OffHand)
                    {
                        eligibleForEnchant = this.Type == Item.ItemType.Shield;
                    }
                    else
                    {
                        eligibleForEnchant = (Enchant.Slot == this.Slot);
                    }
                    if (eligibleForEnchant) totalItemStats.AccumulateUnsafe(Enchant.Stats, true);
                    if (!volatileItem)
                    {
                        cachedTime = DateTime.Now;
                        cachedTotalStats = totalItemStats;
                        cachedTotalStats.GenerateSparseData();
                        if (unsafeStatsAccumulator != null)
                        {
                            unsafeStatsAccumulator.AccumulateUnsafe(cachedTotalStats);
                        }
                    }
                }
                return totalItemStats;
            }
		}


		public static ItemInstance LoadFromId(string gemmedId)
		{
			if (string.IsNullOrEmpty(gemmedId))
				return null;
			return new ItemInstance(gemmedId);
		}

		#region IComparable<Item> Members

		public int CompareTo(ItemInstance other)
		{
			return GemmedId.CompareTo(other.GemmedId);
		}

		#endregion

        public bool FitsInSlot(Character.CharacterSlot characterSlot)
        {
            return Item.FitsInSlot(characterSlot);
        }

        // helper functions to minimize fixing of models
        [XmlIgnore]
        public Item.ItemSlot Slot
        {
            get
            {
                if (Item == null) return Item.ItemSlot.None;
                return Item.Slot;
            }
        }

        [XmlIgnore]
        public Item.ItemType Type
        {
            get
            {
                if (Item == null) return Item.ItemType.None;
                return Item.Type;
            }
        }

        [XmlIgnore]
        public int MinDamage
        {
            get
            {
                if (Item == null) return 0;
                return Item.MinDamage;
            }
        }

        [XmlIgnore]
        public int MaxDamage
        {
            get
            {
                if (Item == null) return 0;
                return Item.MaxDamage;
            }
        }

        [XmlIgnore]
        public Item.ItemDamageType DamageType
        {
            get
            {
                if (Item == null) return Item.ItemDamageType.Physical;
                return Item.DamageType;
            }
        }

        [XmlIgnore]
        public float Speed
        {
            get
            {
                if (Item == null) return 0;
                return Item.Speed;
            }
        }

        public static bool operator ==(ItemInstance a, ItemInstance b)
        {
            if ((object)b == null || (object)a == null) return (object)a == (object)b;
            return a.GemmedId == b.GemmedId;
        }

        public static bool operator !=(ItemInstance a, ItemInstance b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            ItemInstance o = obj as ItemInstance;
            if ((object)o != null)
            {
                return GemmedId == o.GemmedId;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return GemmedId.GetHashCode();
        }
    }

	#endregion

	public class ItemList : List<Item>
    {
        public ItemList() : base() { }
        public ItemList(IEnumerable<Item> collection) : base(collection) { }
    }
}
