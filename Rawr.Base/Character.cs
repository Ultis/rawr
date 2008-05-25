using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Forms;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Rawr //O O . .
{
    [Serializable]
    public class Character
    {
        [XmlElement("Name")]
        public string _name;
        [XmlElement("Realm")]
        public string _realm;
        [XmlElement("Region")]
		public Character.CharacterRegion _region = CharacterRegion.US;
        [XmlElement("Race")]
        public CharacterRace _race = CharacterRace.NightElf;
        [XmlElement("Class")]
        public CharacterClass _class = CharacterClass.Druid;
        [XmlElement("ActiveBuffs")]
		public List<string> _activeBuffs = new List<string>();
        [XmlElement("Head")]
        public string _head;
        [XmlElement("Neck")]
        public string _neck;
        [XmlElement("Shoulders")]
        public string _shoulders;
        [XmlElement("Back")]
        public string _back;
        [XmlElement("Chest")]
        public string _chest;
        [XmlElement("Shirt")]
        public string _shirt;
        [XmlElement("Tabard")]
        public string _tabard;
        [XmlElement("Wrist")]
        public string _wrist;
        [XmlElement("Hands")]
        public string _hands;
        [XmlElement("Waist")]
        public string _waist;
        [XmlElement("Legs")]
        public string _legs;
        [XmlElement("Feet")]
        public string _feet;
        [XmlElement("Finger1")]
        public string _finger1;
        [XmlElement("Finger2")]
        public string _finger2;
        [XmlElement("Trinket1")]
        public string _trinket1;
        [XmlElement("Trinket2")]
        public string _trinket2;
		[XmlElement("MainHand")]
		public string _mainHand;
		[XmlElement("OffHand")]
		public string _offHand;
		[XmlElement("Ranged")]
		public string _ranged;
		[XmlElement("Projectile")]
		public string _projectile;
		[XmlElement("ProjectileBag")]
		public string _projectileBag;
		[XmlElement("HeadEnchant")]
		public int _headEnchant = 0;
		[XmlElement("ShouldersEnchant")]
		public int _shouldersEnchant = 0;
		[XmlElement("BackEnchant")]
		public int _backEnchant = 0;
		[XmlElement("ChestEnchant")]
		public int _chestEnchant = 0;
		[XmlElement("WristEnchant")]
		public int _wristEnchant = 0;
		[XmlElement("HandsEnchant")]
		public int _handsEnchant = 0;
		[XmlElement("LegsEnchant")]
		public int _legsEnchant = 0;
		[XmlElement("FeetEnchant")]
		public int _feetEnchant = 0;
		[XmlElement("Finger1Enchant")]
		public int _finger1Enchant = 0;
		[XmlElement("Finger2Enchant")]
		public int _finger2Enchant = 0;
		[XmlElement("MainHandEnchant")]
		public int _mainHandEnchant = 0;
		[XmlElement("OffHandEnchant")]
		public int _offHandEnchant = 0;
		[XmlElement("RangedEnchant")]
		public int _rangedEnchant = 0;
		[XmlElement("CalculationOptions")]
		public SerializableDictionary<string, string> _serializedCalculationOptions = new SerializableDictionary<string, string>();
        [XmlElement("Talents")]
        public TalentTree _talents = new TalentTree();
		[XmlElement("AvailableItems")]
		public List<int> _availableItems = new List<int>();
		[XmlElement("CurrentModel")]
		public string _currentModel;
		[XmlElement("EnforceMetagemRequirements")]
		public bool _enforceMetagemRequirements = false;



        // set to true to suppress ItemsChanged event
        [XmlIgnore]
        public bool IsLoading { get; set; }
        

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
		public Character.CharacterRegion Region
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
        public Character.CharacterClass Class
        {
            get { return _class; }
            set { _class = value; }
        }


        [XmlIgnore]
        public List<string> ActiveBuffs
        {
            get { return _activeBuffs; }
			set { _activeBuffs = value; }
        }

		[XmlIgnore]
		private Item _headCached = null;
        [XmlIgnore]
        public Item Head
        {
			get
			{
				if (_headCached == null || _headCached.GemmedId != _head)
				{
					_headCached = Item.LoadFromId(_head, "Equipped Head");
				}
				return _headCached;
			}
            set
            {
                if (value == null || _head != value.GemmedId)
                {
                    _head = value != null ? value.GemmedId : null;
                    _headCached = value;
                    OnItemsChanged();
                }
            }
        }
		[XmlIgnore]
		private Item _neckCached = null;
		[XmlIgnore]
        public Item Neck
        {
			get
			{
				if (_neckCached == null || _neckCached.GemmedId != _neck)
				{
					_neckCached = Item.LoadFromId(_neck, "Equipped Neck");
				}
				return _neckCached;
			}
            set
            {
                if (value == null || _neck != value.GemmedId)
                {
                    _neck = value != null ? value.GemmedId : null;
                    _neckCached = value;
                    OnItemsChanged();
                }
            }
        }
		[XmlIgnore]
		private Item _shouldersCached = null;
		[XmlIgnore]
        public Item Shoulders
        {
			get
			{
				if (_shouldersCached == null || _shouldersCached.GemmedId != _shoulders)
				{
					_shouldersCached = Item.LoadFromId(_shoulders, "Equipped Shoulders");
				}
				return _shouldersCached;
			}
            set
            {
                if (value == null || _shoulders != value.GemmedId)
                {
                    _shoulders = value != null ? value.GemmedId : null;
                    _shouldersCached = value;
                    OnItemsChanged();
                }
            }
        }
		[XmlIgnore]
		private Item _backCached = null;
		[XmlIgnore]
        public Item Back
        {
			get
			{
				if (_backCached == null || _backCached.GemmedId != _back)
				{
					_backCached = Item.LoadFromId(_back, "Equipped Back");
				}
				return _backCached;
			}
            set
            {
                if (value == null || _back != value.GemmedId)
                {
                    _back = value != null ? value.GemmedId : null;
                    _backCached = value;
                    OnItemsChanged();
                }
            }
        }
		[XmlIgnore]
		private Item _chestCached = null;
		[XmlIgnore]
        public Item Chest
        {
			get
			{
				if (_chestCached == null || _chestCached.GemmedId != _chest)
				{
					_chestCached = Item.LoadFromId(_chest, "Equipped Chest");
				}
				return _chestCached;
			}
            set
            {
                if (value == null || _chest != value.GemmedId)
                {
                    _chest = value != null ? value.GemmedId : null;
                    _chestCached = value;
                    OnItemsChanged();
                }
            }
        }
		[XmlIgnore]
		private Item _shirtCached = null;
		[XmlIgnore]
        public Item Shirt
        {
			get
			{
				if (_shirtCached == null || _shirtCached.GemmedId != _shirt)
				{
					_shirtCached = Item.LoadFromId(_shirt, "Equipped Shirt");
				}
				return _shirtCached;
			}
            set
            {
                if (value == null || _shirt != value.GemmedId)
                {
                    _shirt = value != null ? value.GemmedId : null;
                    _shirtCached = value;
                    OnItemsChanged();
                }
            }
        }
		[XmlIgnore]
		private Item _tabardCached = null;
		[XmlIgnore]
        public Item Tabard
        {
			get
			{
				if (_tabardCached == null || _tabardCached.GemmedId != _tabard)
				{
					_tabardCached = Item.LoadFromId(_tabard, "Equipped Tabard");
				}
				return _tabardCached;
			}
            set
            {
                if (value == null || _tabard != value.GemmedId)
                {
                    _tabard = value != null ? value.GemmedId : null;
                    _tabardCached = value;
                    OnItemsChanged();
                }
            }
        }
		[XmlIgnore]
		private Item _wristCached = null;
		[XmlIgnore]
        public Item Wrist
        {
			get
			{
				if (_wristCached == null || _wristCached.GemmedId != _wrist)
				{
					_wristCached = Item.LoadFromId(_wrist, "Equipped Wrist");
				}
				return _wristCached;
			} 
            set
            {
                if (value == null || _wrist != value.GemmedId)
                {
                    _wrist = value != null ? value.GemmedId : null;
                    _wristCached = value;
                    OnItemsChanged();
                }
            }
        }
		[XmlIgnore]
		private Item _handsCached = null;
		[XmlIgnore]
        public Item Hands
        {
			get
			{
				if (_handsCached == null || _handsCached.GemmedId != _hands)
				{
					_handsCached = Item.LoadFromId(_hands, "Equipped Hands");
				}
				return _handsCached;
			} 
            set
            {
                if (value == null || _hands != value.GemmedId)
                {
                    _hands = value != null ? value.GemmedId : null;
                    _handsCached = value;
                    OnItemsChanged();
                }
            }
        }
		[XmlIgnore]
		private Item _waistCached = null;
		[XmlIgnore]
        public Item Waist
        {
			get
			{
				if (_waistCached == null || _waistCached.GemmedId != _waist)
				{
					_waistCached = Item.LoadFromId(_waist, "Equipped Waist");
				}
				return _waistCached;
			}
            set
            {
                if (value == null || _waist != value.GemmedId)
                {
                    _waist = value != null ? value.GemmedId : null;
                    _waistCached = value;
                    OnItemsChanged();
                }
            }
        }
		[XmlIgnore]
		private Item _legsCached = null;
		[XmlIgnore]
        public Item Legs
        {
			get
			{
				if (_legsCached == null || _legsCached.GemmedId != _legs)
				{
					_legsCached = Item.LoadFromId(_legs, "Equipped Legs");
				}
				return _legsCached;
			}
            set
            {
                if (value == null || _legs != value.GemmedId)
                {
                    _legs = value != null ? value.GemmedId : null;
                    _legsCached = value;
                    OnItemsChanged();
                }
            }
        }
		[XmlIgnore]
		private Item _feetCached = null;
		[XmlIgnore]
        public Item Feet
        {
			get
			{
				if (_feetCached == null || _feetCached.GemmedId != _feet)
				{
					_feetCached = Item.LoadFromId(_feet, "Equipped Feet");
				}
				return _feetCached;
			} 
            set
            {
                if (value == null || _feet != value.GemmedId)
                {
                    _feet = value != null ? value.GemmedId : null;
                    _feetCached = value;
                    OnItemsChanged();
                }
            }
        }
		[XmlIgnore]
		private Item _finger1Cached = null;
		[XmlIgnore]
        public Item Finger1
        {
			get
			{
				if (_finger1Cached == null || _finger1Cached.GemmedId != _finger1)
				{
					_finger1Cached = Item.LoadFromId(_finger1, "Equipped Finger1");
				}
				return _finger1Cached;
			}
            set
            {
                if (value == null || _finger1 != value.GemmedId)
                {
                    _finger1 = value != null ? value.GemmedId : null;
                    _finger1Cached = value;
                    OnItemsChanged();
                }
            }
        }
		[XmlIgnore]
		private Item _finger2Cached = null;
		[XmlIgnore]
        public Item Finger2
        {
			get
			{
				if (_finger2Cached == null || _finger2Cached.GemmedId != _finger2)
				{
					_finger2Cached = Item.LoadFromId(_finger2, "Equipped Finger2");
				}
				return _finger2Cached;
			}
            set
            {
                if (value == null || _finger2 != value.GemmedId)
                {
                    _finger2 = value != null ? value.GemmedId : null;
                    _finger2Cached = value;
                    OnItemsChanged();
                }
            }
        }
		[XmlIgnore]
		private Item _trinket1Cached = null;
		[XmlIgnore]
        public Item Trinket1
        {
			get
			{
				if (_trinket1Cached == null || _trinket1Cached.GemmedId != _trinket1)
				{
					_trinket1Cached = Item.LoadFromId(_trinket1, "Equipped Trinket1");
				}
				return _trinket1Cached;
			}
            set
            {
                if (value == null || _trinket1 != value.GemmedId)
                {
                    _trinket1 = value != null ? value.GemmedId : null;
                    _trinket1Cached = value;
                    OnItemsChanged();
                }
            }
        }
		[XmlIgnore]
		private Item _trinket2Cached = null;
		[XmlIgnore]
        public Item Trinket2
        {
			get
			{
				if (_trinket2Cached == null || _trinket2Cached.GemmedId != _trinket2)
				{
					_trinket2Cached = Item.LoadFromId(_trinket2, "Equipped Trinket2");
				}
				return _trinket2Cached;
			}
            set
            {
                if (value == null || _trinket2 != value.GemmedId)
                {
                    _trinket2 = value != null ? value.GemmedId : null;
                    _trinket2Cached = value;
                    OnItemsChanged();
                }
            }
        }
		[XmlIgnore]
		private Item _mainHandCached = null;
		[XmlIgnore]
        public Item MainHand
        {
			get
			{
				if (_mainHandCached == null || _mainHandCached.GemmedId != _mainHand)
				{
					_mainHandCached = Item.LoadFromId(_mainHand, "Equipped MainHand");
				}
				return _mainHandCached;
			}
            set
            {
				if (value == null || _mainHand != value.GemmedId)
                {
					_mainHand = value != null ? value.GemmedId : null;
					//if (MainHand != null && MainHand.Slot == Item.ItemSlot.TwoHand)
					//    OffHand = null;
                    _mainHandCached = value;
                    OnItemsChanged();
                }
            }
        }
		[XmlIgnore]
		private Item _offHandCached = null;
		[XmlIgnore]
		public Item OffHand
		{
			get
			{
				if (_offHandCached == null || _offHandCached.GemmedId != _offHand)
				{
					_offHandCached = Item.LoadFromId(_offHand, "Equipped OffHand");
				}
				return _offHandCached;
			}
			set
			{
				if (value == null || _offHand != value.GemmedId)
				{
					_offHand = value != null ? value.GemmedId : null;
                    _offHandCached = value;
					OnItemsChanged();
				}
			}
		}
		[XmlIgnore]
		private Item _rangedCached = null;
		[XmlIgnore]
        public Item Ranged
        {
			get
			{
				if (_rangedCached == null || _rangedCached.GemmedId != _ranged)
				{
					_rangedCached = Item.LoadFromId(_ranged, "Equipped Ranged");
				}
				return _rangedCached;
			} 
            set
            {
				if (value == null || _ranged != value.GemmedId)
                {
					_ranged = value != null ? value.GemmedId : null;
                    _rangedCached = value;
                    OnItemsChanged();
                }
            }
		}
		[XmlIgnore]
		private Item _projectileCached = null;
		[XmlIgnore]
		public Item Projectile
		{
			get
			{
				if (_projectileCached == null || _projectileCached.GemmedId != _projectile)
				{
					_projectileCached = Item.LoadFromId(_projectile, "Equipped Projectile");
				}
				return _projectileCached;
			} 
			set
			{
				if (value == null || _projectile != value.GemmedId)
				{
					_projectile = value != null ? value.GemmedId : null;
                    _projectileCached = value;
					OnItemsChanged();
				}
			}
		}
		[XmlIgnore]
		private Item _projectileBagCached = null;
		[XmlIgnore]
		public Item ProjectileBag
		{
			get
			{
				if (_projectileBagCached == null || _projectileBagCached.GemmedId != _projectileBag)
				{
					_projectileBagCached = Item.LoadFromId(_projectileBag, "Equipped ProjectileBag");
				}
				return _projectileBagCached;
			} 
			set
			{
				if (value == null || _projectileBag != value.GemmedId)
				{
					_projectileBag = value != null ? value.GemmedId : null;
                    _projectileBagCached = value;
					OnItemsChanged();
				}
			}
		}

        [XmlIgnore]
        private Enchant _headEnchantCached = null;
        [XmlIgnore]
		public Enchant HeadEnchant
		{
			get
            {
                if (_headEnchantCached == null || _headEnchantCached.Id != _headEnchant)
                {
                    _headEnchantCached = Enchant.FindEnchant(_headEnchant, Item.ItemSlot.Head);
                }
                return _headEnchantCached;
            }
			set 
            {
                _headEnchant = value == null ? 0 : value.Id;
                _headEnchantCached = value;
            }
		}

        [XmlIgnore]
        private Enchant _shouldersEnchantCached = null;
        [XmlIgnore]
		public Enchant ShouldersEnchant
		{
			get 
            {
                if (_shouldersEnchantCached == null || _shouldersEnchantCached.Id != _shouldersEnchant)
                {
                    _shouldersEnchantCached = Enchant.FindEnchant(_shouldersEnchant, Item.ItemSlot.Shoulders);
                }
                return _shouldersEnchantCached;
            }
			set 
            {
                _shouldersEnchant = value == null ? 0 : value.Id;
                _shouldersEnchantCached = value;
            }
		}

        [XmlIgnore]
        private Enchant _backEnchantCached = null;
        [XmlIgnore]
		public Enchant BackEnchant
		{
			get 
            {
                if (_backEnchantCached == null || _backEnchantCached.Id != _backEnchant)
                {
                    _backEnchantCached = Enchant.FindEnchant(_backEnchant, Item.ItemSlot.Back);
                }
                return _backEnchantCached;
            }
			set 
            {
                _backEnchant = value == null ? 0 : value.Id;
                _backEnchantCached = value;
            }
		}

        [XmlIgnore]
        private Enchant _chestEnchantCached = null;
        [XmlIgnore]
		public Enchant ChestEnchant
		{
			get
            {
                if (_chestEnchantCached == null || _chestEnchantCached.Id != _chestEnchant)
                {
                    _chestEnchantCached = Enchant.FindEnchant(_chestEnchant, Item.ItemSlot.Chest);
                }
                return _chestEnchantCached;
            }
			set
            {
                _chestEnchant = value == null ? 0 : value.Id;
                _chestEnchantCached = value;
            }
		}

        [XmlIgnore]
        private Enchant _wristEnchantCached = null;
        [XmlIgnore]
		public Enchant WristEnchant
		{
			get 
            {
                if (_wristEnchantCached == null || _wristEnchantCached.Id != _wristEnchant)
                {
                    _wristEnchantCached = Enchant.FindEnchant(_wristEnchant, Item.ItemSlot.Wrist);
                }
                return _wristEnchantCached;
            }
			set
            {
                _wristEnchant = value == null ? 0 : value.Id;
                _wristEnchantCached = value;
            }
		}

        [XmlIgnore]
        private Enchant _handsEnchantCached = null;
        [XmlIgnore]
		public Enchant HandsEnchant
		{
			get
            {
                if (_handsEnchantCached == null || _handsEnchantCached.Id != _handsEnchant)
                {
                    _handsEnchantCached = Enchant.FindEnchant(_handsEnchant, Item.ItemSlot.Hands);
                }
                return _handsEnchantCached;
            }
			set
            {
                _handsEnchant = value == null ? 0 : value.Id;
                _handsEnchantCached = value;
            }
		}

        [XmlIgnore]
        private Enchant _legsEnchantCached = null;
        [XmlIgnore]
		public Enchant LegsEnchant
		{
			get 
            {
                if (_legsEnchantCached == null || _legsEnchantCached.Id != _legsEnchant)
                {
                    _legsEnchantCached = Enchant.FindEnchant(_legsEnchant, Item.ItemSlot.Legs);
                }
                return _legsEnchantCached;
            }
			set
            {
                _legsEnchant = value == null ? 0 : value.Id;
                _legsEnchantCached = value;
            }
		}

        [XmlIgnore]
        private Enchant _feetEnchantCached = null;
        [XmlIgnore]
		public Enchant FeetEnchant
		{
			get
            {
                if (_feetEnchantCached == null || _feetEnchantCached.Id != _feetEnchant)
                {
                    _feetEnchantCached = Enchant.FindEnchant(_feetEnchant, Item.ItemSlot.Feet);
                }
                return _feetEnchantCached;
            }
			set 
            {
                _feetEnchant = value == null ? 0 : value.Id;
                _feetEnchantCached = value;
            }
		}

        [XmlIgnore]
        private Enchant _finger1EnchantCached = null;
        [XmlIgnore]
		public Enchant Finger1Enchant
		{
			get 
            {
                if (_finger1EnchantCached == null || _finger1EnchantCached.Id != _finger1Enchant)
                {
                    _finger1EnchantCached = Enchant.FindEnchant(_finger1Enchant, Item.ItemSlot.Finger);
                }
                return _finger1EnchantCached;
            }
			set
            {
                _finger1Enchant = value == null ? 0 : value.Id;
                _finger1EnchantCached = value;
            }
		}

        [XmlIgnore]
        private Enchant _finger2EnchantCached = null;
        [XmlIgnore]
		public Enchant Finger2Enchant
		{
			get 
            {
                if (_finger2EnchantCached == null || _finger2EnchantCached.Id != _finger2Enchant)
                {
                    _finger2EnchantCached = Enchant.FindEnchant(_finger2Enchant, Item.ItemSlot.Finger);
                }
                return _finger2EnchantCached;
            }
			set
            {
                _finger2Enchant = value == null ? 0 : value.Id;
                _finger2EnchantCached = value;
            }
		}

        [XmlIgnore]
        private Enchant _mainHandEnchantCached = null;
        [XmlIgnore]
		public Enchant MainHandEnchant
		{
			get 
            {
                if (_mainHandEnchantCached == null || _mainHandEnchantCached.Id != _mainHandEnchant)
                {
                    _mainHandEnchantCached = Enchant.FindEnchant(_mainHandEnchant, Item.ItemSlot.MainHand);
                }
                return _mainHandEnchantCached;
            }
			set
            {
                _mainHandEnchant = value == null ? 0 : value.Id;
                _mainHandEnchantCached = value;
            }
		}

        [XmlIgnore]
        private Enchant _offHandEnchantCached = null;
        [XmlIgnore]
		public Enchant OffHandEnchant
		{
			get 
            {
                if (_offHandEnchantCached == null || _offHandEnchantCached.Id != _offHandEnchant)
                {
                    _offHandEnchantCached = Enchant.FindEnchant(_offHandEnchant, Item.ItemSlot.OffHand);
                }
                return _offHandEnchantCached;
            }
			set 
            {
                _offHandEnchant = value == null ? 0 : value.Id;
                _offHandEnchantCached = value;
            }
		}

        [XmlIgnore]
        private Enchant _rangedEnchantCached = null;
        [XmlIgnore]
		public Enchant RangedEnchant
		{
			get
            {
                if (_rangedEnchantCached == null || _rangedEnchantCached.Id != _rangedEnchant)
                {
                    _rangedEnchantCached = Enchant.FindEnchant(_rangedEnchant, Item.ItemSlot.Ranged);
                }
                return _rangedEnchantCached;
            }
			set
            {
                _rangedEnchant = value == null ? 0 : value.Id;
                _rangedEnchantCached = value;
            }
		}

		[XmlIgnore]
        private Dictionary<string, ICalculationOptionBase> _calculationOptions = new SerializableDictionary<string, ICalculationOptionBase>();
		[XmlIgnore]
		public ICalculationOptionBase CalculationOptions
		{
			get
			{
                ICalculationOptionBase ret;
                _calculationOptions.TryGetValue(CurrentModel, out ret);
                if (ret == null && _serializedCalculationOptions.ContainsKey(CurrentModel))
                {
                    ret = Calculations.DeserializeDataObject(_serializedCalculationOptions[CurrentModel]);
                    _calculationOptions[CurrentModel] = ret;
                }
                return ret;
			}
			set
			{
				_calculationOptions[CurrentModel] = value;
			}
		}

		[XmlIgnore]
		public string CurrentModel
		{
			get
			{
				if (string.IsNullOrEmpty(_currentModel))
				{
					foreach (KeyValuePair<string, Type> kvp in Calculations.Models)
						if (kvp.Value == Calculations.Instance.GetType())
							_currentModel = kvp.Key;
				}
				return _currentModel;
			}
			set
			{
				_currentModel = value;
			}
		}

		[XmlIgnore]
		public bool EnforceMetagemRequirements
		{
			get { return _enforceMetagemRequirements; }
			set { _enforceMetagemRequirements = value; }
		}

        [XmlIgnore]
        public bool DisableBuffAutoActivation { get; set; }


        [XmlIgnore]
        public TalentTree Talents
        {
            get { return _talents; }
            set { _talents = value; }
        }
		[XmlIgnore]
		public List<int> AvailableItems
		{
			get { return _availableItems; }
			set { _availableItems = value; }
		}

		public void SerializeCalculationOptions()
		{
			if (CalculationOptions != null)
				_serializedCalculationOptions[CurrentModel] = CalculationOptions.GetXml();
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

        private static CharacterSlot[] _characterSlots;
        public static CharacterSlot[] CharacterSlots
        {
            get
            {
                if (_characterSlots == null)
                {
                    _characterSlots = (CharacterSlot[])Enum.GetValues(typeof(CharacterSlot));
                }
                return _characterSlots;
            }
        }

        // cache gem counts as this takes the most time of accumulating item stats
        // this becomes invalid when items on character change or when item id changes by editing gems in the equipped item
        // when item is edited in item editor it listens for id changed event and sets the item to the slot again
        // so it is enough to just invalidate this when items on character are set
        // it would be possible that this becomes out of sync if someone edits the color of gems, for now assumed not to be an issue
        private bool gemCountValid;
        private int redGemCount;
        private int yellowGemCount;
        private int blueGemCount;

        public int RedGemCount
        {
            get
            {
                ComputeGemCount();
                return redGemCount;
            }
        }

        public int YellowGemCount
        {
            get
            {
                ComputeGemCount();
                return yellowGemCount;
            }
        }

        public int BlueGemCount
        {
            get
            {
                ComputeGemCount();
                return blueGemCount;
            }
        }

        private void ComputeGemCount()
        {
            if (!gemCountValid)
            {
                redGemCount = GetGemColorCount(Item.ItemSlot.Red);
                yellowGemCount = GetGemColorCount(Item.ItemSlot.Yellow);
                blueGemCount = GetGemColorCount(Item.ItemSlot.Blue);
                gemCountValid = true;
            }
        }

        public int GetGemColorCount(Item.ItemSlot slotColor)
        {
            int count = 0;
            foreach (CharacterSlot slot in CharacterSlots)
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
            gemCountValid = false; // invalidate gem counts
            if (IsLoading) return;
			RecalculateSetBonuses();

			if (ItemsChanged != null) ItemsChanged(this, EventArgs.Empty);
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

        [XmlIgnore]
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
            foreach (CharacterSlot slot in CharacterSlots)
				if (this[slot] == item)
					listSlots.Add(slot);
			return listSlots.ToArray();
		}

		public enum CharacterRegion { US, EU, KR, TW }
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
			ProjectileBag = 102,
            AutoSelect = 1000,
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

        public Character(string name, string realm, Character.CharacterRegion region, CharacterRace race, Item head, Item neck, Item shoulders, Item back, Item chest, Item shirt, Item tabard,
                Item wrist, Item hands, Item waist, Item legs, Item feet, Item finger1, Item finger2, Item trinket1, Item trinket2, Item mainHand, Item offHand, Item ranged, Item projectile, Item projectileBag,
            Enchant enchantHead, Enchant enchantShoulders, Enchant enchantBack, Enchant enchantChest, Enchant enchantWrist, Enchant enchantHands, Enchant enchantLegs, Enchant enchantFeet, Enchant enchantFinger1, Enchant enchantFinger2, Enchant enchantMainHand, Enchant enchantOffHand, Enchant enchantRanged)
        {
            IsLoading = true;
            _name = name;
            _realm = realm;
            _region = region;
            _race = race;
            Head = head;
            Neck = neck;
            Shoulders = shoulders;
            Back = back;
            Chest = chest;
            Shirt = shirt;
            Tabard = tabard;
            Wrist = wrist;
            Hands = hands;
            Waist = waist;
            Legs = legs;
            Feet = feet;
            Finger1 = finger1;
            Finger2 = finger2;
            Trinket1 = trinket1;
            Trinket2 = trinket2;
            MainHand = mainHand;
            OffHand = offHand;
            Ranged = ranged;
            Projectile = projectile;
            ProjectileBag = projectileBag;

            HeadEnchant = enchantHead;
            ShouldersEnchant = enchantShoulders;
            BackEnchant = enchantBack;
            ChestEnchant = enchantChest;
            WristEnchant = enchantWrist;
            HandsEnchant = enchantHands;
            LegsEnchant = enchantLegs;
            FeetEnchant = enchantFeet;
            Finger1Enchant = enchantFinger1;
            Finger2Enchant = enchantFinger2;
            MainHandEnchant = enchantMainHand;
            OffHandEnchant = enchantOffHand;
            RangedEnchant = enchantRanged;
            IsLoading = false;
            RecalculateSetBonuses();
        }

        public Character(string name, string realm, Character.CharacterRegion region, CharacterRace race, Item head, Item neck, Item shoulders, Item back, Item chest, Item shirt, Item tabard,
                Item wrist, Item hands, Item waist, Item legs, Item feet, Item finger1, Item finger2, Item trinket1, Item trinket2, Item mainHand, Item offHand, Item ranged, Item projectile, Item projectileBag,
            Enchant enchantHead, Enchant enchantShoulders, Enchant enchantBack, Enchant enchantChest, Enchant enchantWrist, Enchant enchantHands, Enchant enchantLegs, Enchant enchantFeet, Enchant enchantFinger1, Enchant enchantFinger2, Enchant enchantMainHand, Enchant enchantOffHand, Enchant enchantRanged, List<string> activeBuffs)
        {
            IsLoading = true;
            _name = name;
            _realm = realm;
            _region = region;
            _race = race;
            Head = head;
            Neck = neck;
            Shoulders = shoulders;
            Back = back;
            Chest = chest;
            Shirt = shirt;
            Tabard = tabard;
            Wrist = wrist;
            Hands = hands;
            Waist = waist;
            Legs = legs;
            Feet = feet;
            Finger1 = finger1;
            Finger2 = finger2;
            Trinket1 = trinket1;
            Trinket2 = trinket2;
            MainHand = mainHand;
            OffHand = offHand;
            Ranged = ranged;
            Projectile = projectile;
            ProjectileBag = projectileBag;

            HeadEnchant = enchantHead;
            ShouldersEnchant = enchantShoulders;
            BackEnchant = enchantBack;
            ChestEnchant = enchantChest;
            WristEnchant = enchantWrist;
            HandsEnchant = enchantHands;
            LegsEnchant = enchantLegs;
            FeetEnchant = enchantFeet;
            Finger1Enchant = enchantFinger1;
            Finger2Enchant = enchantFinger2;
            MainHandEnchant = enchantMainHand;
            OffHandEnchant = enchantOffHand;
            RangedEnchant = enchantRanged;
            IsLoading = false;
            ActiveBuffs.AddRange(activeBuffs);
            RecalculateSetBonuses();
        }

		public Character Clone()
		{
            Character clone = new Character(this.Name, this.Realm, this.Region, this.Race,
                        this.Head, this.Neck, this.Shoulders, this.Back, this.Chest, this.Shirt,
                        this.Tabard, this.Wrist, this.Hands, this.Waist, this.Legs, this.Feet,
                        this.Finger1,
                        this.Finger2,
                        this.Trinket1,
                        this.Trinket2,
                        this.MainHand,
                        this.OffHand,
                        this.Ranged,
                        this.Projectile,
                        this.ProjectileBag,
                        this.HeadEnchant,
                        this.ShouldersEnchant,
                        this.BackEnchant,
                        this.ChestEnchant,
                        this.WristEnchant,
                        this.HandsEnchant,
                        this.LegsEnchant,
                        this.FeetEnchant,
                        this.Finger1Enchant,
                        this.Finger2Enchant,
                        this.MainHandEnchant,
                        this.OffHandEnchant,
                        this.RangedEnchant);
			foreach (string buff in this.ActiveBuffs) 
				if (!clone.ActiveBuffs.Contains(buff))
					clone.ActiveBuffs.Add(buff);
			clone.CalculationOptions = this.CalculationOptions;
            clone.Class = this.Class;
            clone.Talents = this.Talents;
			clone.EnforceMetagemRequirements = this.EnforceMetagemRequirements;
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

					if (xml.IndexOf("<CalculationOptions>") != xml.LastIndexOf("<CalculationOptions>"))
					{
						xml = xml.Substring(0, xml.IndexOf("<CalculationOptions>")) +
							xml.Substring(xml.LastIndexOf("</CalculationOptions>") + "</CalculationOptions>".Length);
					}

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

	public interface ICalculationOptionBase
	{
		string GetXml();
	}
}
