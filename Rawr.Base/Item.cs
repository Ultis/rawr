using System;
using System.Xml.Serialization;

namespace Rawr
{
    [Serializable]
	public class Item :IComparable<Item>
	{
		[XmlElement("Name")]
		public string _name;
		[XmlElement("Id")]
		public int _id;
		[XmlElement("IconPath")]
		public string _iconPath;
		[XmlElement("Slot")]
		public ItemSlot _slot;
		[XmlElement("Stats")]
		public Stats _stats = new Stats();
		[XmlElement("Sockets")]
		public Sockets _sockets = new Sockets();
		[System.ComponentModel.DefaultValueAttribute(0)]
		[XmlElement("Gem1Id")]
		public int _gem1Id;
		[System.ComponentModel.DefaultValueAttribute(0)]
		[XmlElement("Gem2Id")]
		public int _gem2Id;
		[System.ComponentModel.DefaultValueAttribute(0)]
		[XmlElement("Gem3Id")]
		public int _gem3Id;
		[XmlElement("Quality")]
		public ItemQuality _quality;
		[System.ComponentModel.DefaultValueAttribute("")]
		[XmlElement("SetName")]
		public string _setName;
		[XmlElement("Type")]
		public ItemType _type = ItemType.None;
		[System.ComponentModel.DefaultValueAttribute(0)]
		[XmlElement("MinDamage")]
		public int _minDamage = 0;
		[System.ComponentModel.DefaultValueAttribute(0)]
		[XmlElement("MaxDamage")]
		public int _maxDamage = 0;
		[System.ComponentModel.DefaultValueAttribute(0)]
		[XmlElement("DamageType")]
        public ItemDamageType _damageType = ItemDamageType.Physical;
		[System.ComponentModel.DefaultValueAttribute(0)]
		[XmlElement("Speed")]
		public float _speed = 0f;
		[System.ComponentModel.DefaultValueAttribute("")]
		[XmlElement("RequiredClasses")]
		public string _requiredClasses;
		[System.ComponentModel.DefaultValueAttribute(false)]
		[XmlElement("Unique")]
		public bool _unique;

        public ItemLocation LocationInfo
        {
            get
            {
                return LocationFactory.Lookup(Id);
            }
        }

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
		/// <summary>
		/// String version of Slot, to facilitate databinding.
		/// </summary>
		[XmlIgnore]
		public string SlotString
		{
			get { return _slot.ToString(); }
			set { _slot = (ItemSlot)Enum.Parse(typeof(ItemSlot), value); }
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
				Item gem = Item.LoadFromId(Gem1Id, "Gem1 in " + GemmedId);
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
				Item gem = Item.LoadFromId(Gem2Id, "Gem2 in " + GemmedId);
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
				Item gem = Item.LoadFromId(Gem3Id, "Gem3 in " + GemmedId);
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
		

		private void OnIdsChanging()
		{
			ItemCache.DeleteItem(this, false);
		}

		public event EventHandler IdsChanged;
		private void OnIdsChanged()
		{
			_gemmedId = string.Empty;
			ItemCache.AddItem(this, false, false);
            InvalidateCachedData();
			if (IdsChanged != null) IdsChanged(this, null);
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
			Legendary
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

        public bool IsGem
        {
            get
            {
                return Slot == ItemSlot.Meta || Slot == ItemSlot.Blue || Slot == ItemSlot.Green || Slot == ItemSlot.Orange || Slot == ItemSlot.Prismatic || Slot == ItemSlot.Purple || Slot == ItemSlot.Red || Slot == ItemSlot.Yellow;
            }
        }

		public Item() { }
		public Item(string name, ItemQuality quality, ItemType type, int id, string iconPath, ItemSlot slot, string setName, bool unique, Stats stats, Sockets sockets, int gem1Id, int gem2Id, int gem3Id, int minDamage, int maxDamage, ItemDamageType damageType, float speed, string requiredClasses)
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
			_setName = setName;
			_quality = quality;
			_type = type;
			_minDamage = minDamage;
			_maxDamage = maxDamage;
            _damageType = damageType;
			_speed = speed;
			_requiredClasses = requiredClasses;
			_unique = unique;
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
				Sockets = this.Sockets.Clone(),
				Gem1Id = this.Gem1Id,
				Gem2Id = this.Gem2Id,
				Gem3Id = this.Gem3Id,
				SetName = this.SetName,
				Type = this.Type,
				MinDamage = this.MinDamage,
				MaxDamage = this.MaxDamage,
                DamageType = this.DamageType,
				Speed = this.Speed,
				RequiredClasses = this.RequiredClasses
			};
		}

		public override string ToString()
		{
			string summary = this.Name + ": ";
			summary += this.GetTotalStats().ToString();
			//summary += Stats.ToString();
			//summary += Sockets.ToString();
			if (summary.EndsWith(", ")) summary = summary.Substring(0, summary.Length - 2);

			if ((Sockets.Color1 != Item.ItemSlot.None && Gem1Id == 0) ||
				(Sockets.Color2 != Item.ItemSlot.None && Gem2Id == 0) ||
				(Sockets.Color3 != Item.ItemSlot.None && Gem3Id == 0))
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
        public void InvalidateCachedData()
        {
            cachedTotalStats = null;
        }

		public Stats GetTotalStats() { return GetTotalStats(null); }
		public Stats GetTotalStats(Character character)
		{
            if (cachedTotalStats != null) return cachedTotalStats;
            bool volatileGem = false, volatileItem = false;
			Stats totalItemStats = new Stats();
			totalItemStats += this.Stats;
			bool eligibleForSocketBonus = GemMatchesSlot(Gem1, Sockets.Color1) &&
				GemMatchesSlot(Gem2, Sockets.Color2) &&
				GemMatchesSlot(Gem3, Sockets.Color3);
			if (Gem1 != null && Gem1.MeetsRequirements(character, out volatileGem)) totalItemStats += Gem1.Stats;
            volatileItem = volatileItem || volatileGem;
            if (Gem2 != null && Gem2.MeetsRequirements(character, out volatileGem)) totalItemStats += Gem2.Stats;
            volatileItem = volatileItem || volatileGem;
            if (Gem3 != null && Gem3.MeetsRequirements(character, out volatileGem)) totalItemStats += Gem3.Stats;
            volatileItem = volatileItem || volatileGem;
            if (eligibleForSocketBonus) totalItemStats += Sockets.Stats;
            if (!volatileItem) cachedTotalStats = totalItemStats;
			return totalItemStats;
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
					return gem == null;
			}
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

        public bool MeetsRequirements(Character character)
        {
            bool temp;
            return MeetsRequirements(character, out temp);
        }

        /// <summary>
        /// Checks whether item meets the requirements to activate its stats.
        /// </summary>
        /// <param name="character">Character for which we are checking the requirements.</param>
        /// <param name="volatileRequirements">This is set to true for items that depend on requirements not local to the item itself.</param>
        /// <returns>True if the item meets the requirements.</returns>
		public bool MeetsRequirements(Character character, out bool volatileRequirements)
		{
            volatileRequirements = false;
            int redGems = 0, yellowGems = 0, blueGems = 0;
            if (character != null)
            {
                redGems = character.GetGemColorCount(ItemSlot.Red);
                yellowGems = character.GetGemColorCount(ItemSlot.Yellow);
                blueGems = character.GetGemColorCount(ItemSlot.Blue);
            }
            bool meetsRequirements = false;

			//TODO: Make this dynamic, by loading the gem requirements from the armory
			switch (this.Id)
			{
				case 25899:
				case 25890:
				case 25901:
				case 32409:
				case 32410:
                    volatileRequirements = true;
					meetsRequirements = redGems >= 2 && yellowGems >= 2 && blueGems >= 2;
                    break;
				case 25897:
                    volatileRequirements = true;
                    meetsRequirements = redGems > blueGems;
                    break;
                case 25895:
                    volatileRequirements = true;
                    meetsRequirements = redGems > yellowGems;
                    break;
                case 25893:
				case 32640:
                    volatileRequirements = true;
                    meetsRequirements = blueGems > yellowGems;
                    break;
                case 34220:
                    volatileRequirements = true;
                    meetsRequirements = blueGems >= 2;
                    break;
                case 25896:
                    volatileRequirements = true;
                    meetsRequirements = blueGems >= 3;
                    break;
                case 25898:
                    volatileRequirements = true;
                    meetsRequirements = blueGems >= 5;
                    break;
                case 32641:
                    volatileRequirements = true;
                    meetsRequirements = yellowGems == 3;
                    break;
                case 25894:
				case 28557:
				case 28556:
                    volatileRequirements = true;
                    meetsRequirements = yellowGems >= 2 && redGems >= 1;
                    break;
                default:
                    meetsRequirements = true;
                    break;
			}
            if (character == null || character.CalculationOptions["EnforceMetagemRequirements"] != "Yes") meetsRequirements = true;

            return meetsRequirements;
		}



		public static Item LoadFromId(int id, string logReason) { return LoadFromId(id, false, logReason, true); }
		public static Item LoadFromId(int id, bool forceRefresh, string logReason, bool raiseEvent) { return LoadFromId(id.ToString() + ".0.0.0", forceRefresh, logReason, raiseEvent); }
		public static Item LoadFromId(string gemmedId, string logReason) { return LoadFromId(gemmedId, false, logReason, true); }	
        public static Item LoadFromId(string gemmedId, bool forceRefresh, string logReason, bool raiseEvent)
		{
			if (string.IsNullOrEmpty(gemmedId))
				return null;
			Item cachedItem = ItemCache.FindItemById(gemmedId, true);
			if (cachedItem != null && !forceRefresh)
				return cachedItem;
			else
			{
				Item newItem = Armory.GetItem(gemmedId, logReason);
                if (newItem != null)
                {
                    ItemCache.AddItem(newItem, true, raiseEvent);
                }
				return newItem;
			}
		}


        #region IComparable<Item> Members

        public int CompareTo(Item other)
        {
            return ToString().CompareTo(other.ToString());
        }

        #endregion
    }

	[Serializable]
	public class Sockets
	{
		[System.ComponentModel.DefaultValueAttribute(0)]
		[XmlElement("Color1")]
		public Item.ItemSlot _color1;
		[System.ComponentModel.DefaultValueAttribute(0)]
		[XmlElement("Color2")]
		public Item.ItemSlot _color2;
		[System.ComponentModel.DefaultValueAttribute(0)]
		[XmlElement("Color3")]
		public Item.ItemSlot _color3;
		[XmlElement("Stats")]
		public Stats _stats = new Stats();

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
			set { _color1 = (Item.ItemSlot)Enum.Parse(typeof(Item.ItemSlot), value); }
		}
		[XmlIgnore]
		public string Color2String
		{
			get { return _color2.ToString(); }
			set { _color2 = (Item.ItemSlot)Enum.Parse(typeof(Item.ItemSlot), value); }
		}
		[XmlIgnore]
		public string Color3String
		{
			get { return _color3.ToString(); }
			set { _color3 = (Item.ItemSlot)Enum.Parse(typeof(Item.ItemSlot), value); }
		}
		[XmlIgnore]
		public Stats Stats
		{
			get { return _stats; }
			set { _stats = value; }
		}

		public Sockets() { }
		public Sockets(Item.ItemSlot color1, Item.ItemSlot color2, Item.ItemSlot color3, Stats stats)
		{
			_color1 = color1;
			_color2 = color2;
			_color3 = color3;
			_stats = stats;
		}

		public Sockets Clone()
		{
			return new Sockets(this.Color1, this.Color2, this.Color3, this.Stats.Clone());
		}

		public override string ToString()
		{
			string summary = Color1.ToString().Substring(0, 1) + Color2.ToString().Substring(0, 1) + Color3.ToString().Substring(0, 1);
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
