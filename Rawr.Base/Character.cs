using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Windows.Forms;
using System.IO;

namespace Rawr //O O . .
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
        [System.Xml.Serialization.XmlElement("Class")]
        public CharacterClass _class = CharacterClass.Druid;
        [System.Xml.Serialization.XmlElement("ActiveBuffs")]
		public List<string> _activeBuffs = new List<string>();
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
		[System.Xml.Serialization.XmlElement("MainHand")]
		public string _mainHand;
		[System.Xml.Serialization.XmlElement("OffHand")]
		public string _offHand;
		[System.Xml.Serialization.XmlElement("Ranged")]
		public string _ranged;
		[System.Xml.Serialization.XmlElement("Projectile")]
		public string _projectile;
		[System.Xml.Serialization.XmlElement("ProjectileBag")]
		public string _projectileBag;
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
		[System.Xml.Serialization.XmlElement("Finger1Enchant")]
		public int _finger1Enchant = 0;
		[System.Xml.Serialization.XmlElement("Finger2Enchant")]
		public int _finger2Enchant = 0;
		[System.Xml.Serialization.XmlElement("MainHandEnchant")]
		public int _mainHandEnchant = 0;
		[System.Xml.Serialization.XmlElement("OffHandEnchant")]
		public int _offHandEnchant = 0;
		[System.Xml.Serialization.XmlElement("RangedEnchant")]
		public int _rangedEnchant = 0;
		[System.Xml.Serialization.XmlElement("CalculationOptions")]
		public string[] _calculationOptionsStrings = new string[0];
        [System.Xml.Serialization.XmlElement("Talents")]
        public TalentTree _talents = new TalentTree();
		[System.Xml.Serialization.XmlElement("AvailableItems")]
		public List<int> _availableItems = new List<int>();



        // set to true to suppress ItemsChanged event
        [System.Xml.Serialization.XmlIgnore]
        public bool IsLoading { get; set; }
        

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
                    OnItemsChanged();
                }
            }
        }

        [System.Xml.Serialization.XmlIgnore]
        public Character.CharacterClass Class
        {
            get { return _class; }
            set { _class = value; }
        }


        [System.Xml.Serialization.XmlIgnore]
        public List<string> ActiveBuffs
        {
            get { return _activeBuffs; }
			set { _activeBuffs = value; }
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
                    OnItemsChanged();
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
                    OnItemsChanged();
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
                    OnItemsChanged();
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
                    OnItemsChanged();
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
                    OnItemsChanged();
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
                    OnItemsChanged();
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
                    OnItemsChanged();
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
                    OnItemsChanged();
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
                    OnItemsChanged();
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
                    OnItemsChanged();
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
                    OnItemsChanged();
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
                    OnItemsChanged();
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
                    OnItemsChanged();
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
                    OnItemsChanged();
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
                    OnItemsChanged();
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
                    OnItemsChanged();
                }
            }
        }
        [System.Xml.Serialization.XmlIgnore]
        public Item MainHand
        {
			get { return Item.LoadFromId(_mainHand, "Equipped MainHand"); }
            set
            {
				if (value == null || _mainHand != value.GemmedId)
                {
					_mainHand = value != null ? value.GemmedId : null;
					if (MainHand != null && MainHand.Slot == Item.ItemSlot.TwoHand)
						OffHand = null;
                    OnItemsChanged();
                }
            }
        }
		[System.Xml.Serialization.XmlIgnore]
		public Item OffHand
		{
			get { return Item.LoadFromId(_offHand, "Equipped OffHand"); }
			set
			{
				if (value == null || _offHand != value.GemmedId)
				{
					_offHand = value != null ? value.GemmedId : null;
					OnItemsChanged();
				}
			}
		}
		[System.Xml.Serialization.XmlIgnore]
        public Item Ranged
        {
			get { return Item.LoadFromId(_ranged, "Equipped Ranged"); }
            set
            {
				if (value == null || _ranged != value.GemmedId)
                {
					_ranged = value != null ? value.GemmedId : null;
                    OnItemsChanged();
                }
            }
		}
		[System.Xml.Serialization.XmlIgnore]
		public Item Projectile
		{
			get { return Item.LoadFromId(_projectile, "Equipped Projectile"); }
			set
			{
				if (value == null || _projectile != value.GemmedId)
				{
					_projectile = value != null ? value.GemmedId : null;
					OnItemsChanged();
				}
			}
		}
		[System.Xml.Serialization.XmlIgnore]
		public Item ProjectileBag
		{
			get { return Item.LoadFromId(_projectileBag, "Equipped ProjectileBag"); }
			set
			{
				if (value == null || _projectileBag != value.GemmedId)
				{
					_projectileBag = value != null ? value.GemmedId : null;
					OnItemsChanged();
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
		public Enchant Finger1Enchant
		{
			get { return Enchant.FindEnchant(_finger1Enchant, Item.ItemSlot.Finger); }
			set { _finger1Enchant = value == null ? 0 : value.Id; }
		}
		[System.Xml.Serialization.XmlIgnore]
		public Enchant Finger2Enchant
		{
			get { return Enchant.FindEnchant(_finger2Enchant, Item.ItemSlot.Finger); }
			set { _finger2Enchant = value == null ? 0 : value.Id; }
		}
		[System.Xml.Serialization.XmlIgnore]
		public Enchant MainHandEnchant
		{
			get { return Enchant.FindEnchant(_mainHandEnchant, Item.ItemSlot.MainHand); }
			set { _mainHandEnchant = value == null ? 0 : value.Id; }
		}
		[System.Xml.Serialization.XmlIgnore]
		public Enchant OffHandEnchant
		{
			get { return Enchant.FindEnchant(_offHandEnchant, Item.ItemSlot.MainHand); }
			set { _offHandEnchant = value == null ? 0 : value.Id; }
		}
		[System.Xml.Serialization.XmlIgnore]
		public Enchant RangedEnchant
		{
			get { return Enchant.FindEnchant(_rangedEnchant, Item.ItemSlot.Ranged); }
			set { _rangedEnchant = value == null ? 0 : value.Id; }
		}
		[System.Xml.Serialization.XmlIgnore]
		public string[] CalculationOptionsStrings
		{
			get
			{
				SerializeCalculationOptions();
				return _calculationOptionsStrings; 
			}
			set { _calculationOptionsStrings = value; }
		}
		[System.Xml.Serialization.XmlIgnore]
		private Dictionary<string, string> _calculationOptions = null;
		[System.Xml.Serialization.XmlIgnore]
		public Dictionary<string, string> CalculationOptions
		{
			get
			{
				if (_calculationOptions == null)
				{
					_calculationOptions = new Dictionary<string, string>();
					if (_calculationOptionsStrings != null)
					{
						foreach (string calcOpt in _calculationOptionsStrings)
						{
							string[] calcOptSplit = calcOpt.Split('=');
							_calculationOptions.Add(calcOptSplit[0], calcOptSplit[1]);
						}
					}
				}
				return _calculationOptions;
			}
			set { _calculationOptions = value; }
		}

        [System.Xml.Serialization.XmlIgnore]
        public TalentTree Talents
        {
            get { return _talents; }
            set { _talents = value; }
        }
		[System.Xml.Serialization.XmlIgnore]
		public List<int> AvailableItems
		{
			get { return _availableItems; }
			set { _availableItems = value; }
		}



		private void SerializeCalculationOptions()
		{
			List<string> listCalcOpts = new List<string>();
			foreach (KeyValuePair<string, string> kvp in CalculationOptions)
				listCalcOpts.Add(string.Format("{0}={1}", kvp.Key, kvp.Value));
			_calculationOptionsStrings = listCalcOpts.ToArray();
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
				case Item.ItemSlot.MainHand:
				case Item.ItemSlot.OneHand:
				case Item.ItemSlot.TwoHand:
					return MainHandEnchant;
				case Item.ItemSlot.OffHand:
					return OffHandEnchant;
				case Item.ItemSlot.Ranged:
					return RangedEnchant;
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
				case Item.ItemSlot.MainHand:
				case Item.ItemSlot.OneHand:
				case Item.ItemSlot.TwoHand:
					MainHandEnchant = enchant;
					break;
				case Item.ItemSlot.OffHand:
					OffHandEnchant = enchant;
					break;
				case Item.ItemSlot.Ranged:
					RangedEnchant = enchant;
					break;
			}
		}

        public int GetGemColorCount(Item.ItemSlot slotColor)
        {
            int count = 0;
			foreach (CharacterSlot slot in Enum.GetValues(typeof(CharacterSlot)))
			{
				Item item = this[slot];
				if (item == null) continue;

				if (Item.GemMatchesSlot(item.Gem1, slotColor)) count++;
				if (Item.GemMatchesSlot(item.Gem2, slotColor)) count++;
				if (Item.GemMatchesSlot(item.Gem3, slotColor)) count++;
			}
            return count;
        }
		
		public event EventHandler AvailableItemsChanged;
		public void OnAvailableItemsChanged()
		{
			if (AvailableItemsChanged != null)
				AvailableItemsChanged(this, EventArgs.Empty);
		}
		
		public event EventHandler ItemsChanged;
		public void OnItemsChanged()
		{
			RecalculateSetBonuses();

			if (!IsLoading && ItemsChanged != null) // alternatively enclose the whole function inside !IsLoading
				ItemsChanged(this, EventArgs.Empty);
		}

		public void RecalculateSetBonuses()
		{
			//Compute Set Bonuses
			Dictionary<string, int> setCounts = new Dictionary<string, int>();
			foreach (Item item in new Item[] {Back, Chest, Feet, Finger1, Finger2, Hands, Head, Legs, Neck,
                Shirt, Shoulders, Tabard, Trinket1, Trinket2, Waist, MainHand, OffHand, Ranged, Wrist})
			{
				if (item != null && !string.IsNullOrEmpty(item.SetName))
				{
					if (setCounts.ContainsKey(item.SetName))
						setCounts[item.SetName] = setCounts[item.SetName] + 1;
					else
						setCounts[item.SetName] = 1;
				}
			}

			foreach (Buff buff in Buff.GetBuffsByType(Buff.BuffType.All))
			{
				if (!string.IsNullOrEmpty(buff.SetName))
				{
					if (setCounts.ContainsKey(buff.SetName) && setCounts[buff.SetName] >= buff.SetThreshold)
					{
						if (!ActiveBuffs.Contains(buff.Name))
							ActiveBuffs.Add(buff.Name);
					}
					else
					{
						if (ActiveBuffs.Contains(buff.Name))
							ActiveBuffs.Remove(buff.Name);
					}
				}
			}
		}

        [System.Xml.Serialization.XmlIgnore]
        public Item this[CharacterSlot slot]
        {
            get
            {
                switch (slot)
                {
                    case CharacterSlot.Head:
                        return this.Head;

                    case CharacterSlot.Neck:
                        return this.Neck;

                    case CharacterSlot.Shoulders:
                        return this.Shoulders;

                    case CharacterSlot.Back:
                        return this.Back;

                    case CharacterSlot.Chest:
                        return this.Chest;

                    case CharacterSlot.Shirt:
                        return this.Shirt;

                    case CharacterSlot.Tabard:
                        return this.Tabard;

                    case CharacterSlot.Wrist:
                        return this.Wrist;

                    case CharacterSlot.Hands:
                        return this.Hands;

                    case CharacterSlot.Waist:
                        return this.Waist;

                    case CharacterSlot.Legs:
                        return this.Legs;

                    case CharacterSlot.Feet:
                        return this.Feet;

                    case CharacterSlot.Finger1:
                        return this.Finger1;

                    case CharacterSlot.Finger2:
                        return this.Finger2;

                    case CharacterSlot.Trinket1:
                        return this.Trinket1;

                    case CharacterSlot.Trinket2:
                        return this.Trinket2;

					case CharacterSlot.MainHand:
						return this.MainHand;

					case CharacterSlot.OffHand:
						return this.OffHand;

					case CharacterSlot.Ranged:
						return this.Ranged;

					case CharacterSlot.Projectile:
						return this.Projectile;

					case CharacterSlot.ProjectileBag:
						return this.ProjectileBag;

					default:
                        return null;
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
					case CharacterSlot.MainHand:
						this.MainHand = value;
						break;
					case CharacterSlot.OffHand:
						this.OffHand = value;
						break;
					case CharacterSlot.Ranged:
						this.Ranged = value;
						break;
					case CharacterSlot.Projectile:
						this.Projectile = value;
						break;
					case CharacterSlot.ProjectileBag:
						this.ProjectileBag = value;
                        break;
                }
            }
        }

        public string[] GetAllEquipedGearIds()
        {
            List<string> _ids = new List<string>();
            _ids.Add(_back);
            _ids.Add(_chest);
            _ids.Add(_feet);
            _ids.Add(_finger1);
            _ids.Add(_finger2);
            _ids.Add(_hands);
            _ids.Add(_head);
            _ids.Add(_legs);
            _ids.Add(_mainHand);
            _ids.Add(_neck);
            _ids.Add(_offHand);
            _ids.Add(_projectile);
            _ids.Add(_projectileBag);
            _ids.Add(_ranged);
            _ids.Add(_shirt);
            _ids.Add(_shoulders);
            _ids.Add(_tabard);
            _ids.Add(_trinket1);
            _ids.Add(_trinket2);
            _ids.Add(_waist);
            _ids.Add(_wrist);
            return _ids.ToArray();
        }

		public CharacterSlot[] GetEquippedSlots(Item item)
		{
			List<CharacterSlot> listSlots = new List<CharacterSlot>();
			foreach (CharacterSlot slot in Enum.GetValues(typeof(CharacterSlot)))
				if (this[slot] == item)
					listSlots.Add(slot);
			return listSlots.ToArray();
		}

		public enum CharacterRegion { US, EU }
		public enum CharacterRace
        {
            Human = 1,
            Orc = 2,
            Dwarf = 3,
            NightElf = 4,
            Undead = 5,
            Tauren = 6,
            Gnome = 7,
            Troll = 8,
            BloodElf = 10,
            Draenei = 11
        }
        public enum CharacterSlot
        {
			None = -1,
			Head = 1,
            Neck = 2,
            Shoulders = 3,
            Back = 15,
            Chest = 5,
            Shirt = 4,
            Tabard = 19,
            Wrist = 9,
            Hands = 10,
            Waist = 6,
            Legs = 7,
            Feet = 8,
            Finger1 = 11,
            Finger2 = 12,
            Trinket1 = 13,
            Trinket2 = 14,
            MainHand = 16,
			OffHand = 17,
            Ranged = 18,
			Projectile = 0,
			
			Gems = 100,
			Metas = 101,
			ProjectileBag = 102
        }

        public enum CharacterClass
        {
            Warrior = 1,
            Paladin = 2,
            Hunter = 3,
            Rogue = 4,
            Priest = 5,
            Shaman = 7,
            Mage = 8,
            Warlock = 9,
            Druid = 11
        }

        public Character() { }
		public Character(string name, string realm, Character.CharacterRegion region, CharacterRace race, string head, string neck, string shoulders, string back, string chest, string shirt, string tabard,
                string wrist, string hands, string waist, string legs, string feet, string finger1, string finger2, string trinket1, string trinket2, string mainHand, string offHand, string ranged, string projectile, string projectileBag) 
        : this(name, realm, region, race, head, neck, shoulders, back, chest, shirt, tabard, wrist, hands, waist, legs, feet, finger1, finger2, trinket1, trinket2, mainHand, offHand, ranged, projectile, projectileBag,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0)
		{ }

		public Character(string name, string realm, Character.CharacterRegion region, CharacterRace race, string head, string neck, string shoulders, string back, string chest, string shirt, string tabard,
				string wrist, string hands, string waist, string legs, string feet, string finger1, string finger2, string trinket1, string trinket2, string mainHand, string offHand, string ranged, string projectile, string projectileBag,
			int enchantHead, int enchantShoulders, int enchantBack, int enchantChest, int enchantWrist, int enchantHands, int enchantLegs, int enchantFeet, int enchantFinger1, int enchantFinger2, int enchantMainHand, int enchantOffHand, int enchantRanged)
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
			_mainHand = mainHand;
			_offHand = offHand;
			_ranged = ranged;
			_projectile = projectile;
			_projectileBag = projectileBag;

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
			_mainHandEnchant = enchantMainHand;
			_offHandEnchant = enchantOffHand;
			_rangedEnchant = enchantRanged;
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
						this.MainHand == null ? null : this.MainHand.GemmedId,
						this.OffHand == null ? null : this.OffHand.GemmedId,
						this.Ranged == null ? null : this.Ranged.GemmedId,
						this.Projectile == null ? null : this.Projectile.GemmedId,
						this.ProjectileBag == null ? null : this.ProjectileBag.GemmedId,
						this.HeadEnchant.Id,
						this.ShouldersEnchant.Id,
						this.BackEnchant.Id,
						this.ChestEnchant.Id,
						this.WristEnchant.Id,
						this.HandsEnchant.Id,
						this.LegsEnchant.Id,
						this.FeetEnchant.Id,
						this.Finger1Enchant.Id,
						this.Finger2Enchant.Id,
						this.MainHandEnchant.Id,
						this.OffHandEnchant.Id,
						this.RangedEnchant.Id);
			foreach (string buff in this.ActiveBuffs) clone.ActiveBuffs.Add(buff);
			clone.CalculationOptionsStrings = this.CalculationOptionsStrings;
            clone.Class = this.Class;
            clone.Talents = this.Talents;
			return clone;
		}
    
        public void Save(string path)
        {
			SerializeCalculationOptions();
            using (StreamWriter writer = new StreamWriter(path, false, Encoding.UTF8))
            {
                System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(Character));
                serializer.Serialize(writer, this);
                writer.Close();
            }
		}

        public static Character Load(string path)
        {
            Character character;
			if (File.Exists(path))
            {
				try
				{
					string xml = System.IO.File.ReadAllText(path).Replace("<Region>en", "<Region>US").Replace("<Weapon>", "<MainHand>").Replace("</Weapon>", "</MainHand>").Replace("<Idol>", "<Ranged>").Replace("</Idol>", "</Ranged>").Replace("<WeaponEnchant>", "<MainHandEnchant>").Replace("</WeaponEnchant>", "</MainHandEnchant>");
					System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(Character));
					System.IO.StringReader reader = new System.IO.StringReader(xml);
					character = (Character)serializer.Deserialize(reader);
					reader.Close();
                    if (character != null)
                    {
                        for (int i = 0; i < character._activeBuffs.Count; i++)
                        {
                            Buff buff = Buff.GetBuffByName(character._activeBuffs[i]);
                            if (buff == null)
                            {
                                character._activeBuffs.RemoveAt(i);
                                i--;
                            }
                        }
                    }
				}
				catch (Exception)
				{
					MessageBox.Show("There was an error attempting to open this character. Most likely, it was saved with a previous beta of Rawr, and isn't upgradable to the new format. Sorry. Please load your character from the armory to begin.");
					character = new Character();
				}
            }
            else
                character = new Character();

            return character;
		}
	}
}
