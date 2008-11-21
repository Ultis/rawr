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
        [XmlIgnore]
        public List<Buff> _activeBuffs = new List<Buff>();
        [XmlElement("ActiveBuffs")]
        public List<string> _activeBuffsXml = new List<string>();
        [XmlIgnore]
        private string[] _item = new string[21];
        [XmlIgnore]
        private Item[] _itemCached = new Item[21];
        [XmlIgnore]
        private int[] _itemEnchant = new int[21];
        [XmlIgnore]
        private Enchant[] _itemEnchantCached = new Enchant[21];
        [XmlElement("Head")]
        public string _head { get { return _item[(int)CharacterSlot.Head]; } set { _item[(int)CharacterSlot.Head] = value; } }
        [XmlElement("Neck")]
        public string _neck { get { return _item[(int)CharacterSlot.Neck]; } set { _item[(int)CharacterSlot.Neck] = value; } }
        [XmlElement("Shoulders")]
        public string _shoulders { get { return _item[(int)CharacterSlot.Shoulders]; } set { _item[(int)CharacterSlot.Shoulders] = value; } }
        [XmlElement("Back")]
        public string _back { get { return _item[(int)CharacterSlot.Back]; } set { _item[(int)CharacterSlot.Back] = value; } }
        [XmlElement("Chest")]
        public string _chest { get { return _item[(int)CharacterSlot.Chest]; } set { _item[(int)CharacterSlot.Chest] = value; } }
        [XmlElement("Shirt")]
        public string _shirt { get { return _item[(int)CharacterSlot.Shirt]; } set { _item[(int)CharacterSlot.Shirt] = value; } }
        [XmlElement("Tabard")]
        public string _tabard { get { return _item[(int)CharacterSlot.Tabard]; } set { _item[(int)CharacterSlot.Tabard] = value; } }
        [XmlElement("Wrist")]
        public string _wrist { get { return _item[(int)CharacterSlot.Wrist]; } set { _item[(int)CharacterSlot.Wrist] = value; } }
        [XmlElement("Hands")]
        public string _hands { get { return _item[(int)CharacterSlot.Hands]; } set { _item[(int)CharacterSlot.Hands] = value; } }
        [XmlElement("Waist")]
        public string _waist { get { return _item[(int)CharacterSlot.Waist]; } set { _item[(int)CharacterSlot.Waist] = value; } }
        [XmlElement("Legs")]
        public string _legs { get { return _item[(int)CharacterSlot.Legs]; } set { _item[(int)CharacterSlot.Legs] = value; } }
        [XmlElement("Feet")]
        public string _feet { get { return _item[(int)CharacterSlot.Feet]; } set { _item[(int)CharacterSlot.Feet] = value; } }
        [XmlElement("Finger1")]
        public string _finger1 { get { return _item[(int)CharacterSlot.Finger1]; } set { _item[(int)CharacterSlot.Finger1] = value; } }
        [XmlElement("Finger2")]
        public string _finger2 { get { return _item[(int)CharacterSlot.Finger2]; } set { _item[(int)CharacterSlot.Finger2] = value; } }
        [XmlElement("Trinket1")]
        public string _trinket1 { get { return _item[(int)CharacterSlot.Trinket1]; } set { _item[(int)CharacterSlot.Trinket1] = value; } }
        [XmlElement("Trinket2")]
        public string _trinket2 { get { return _item[(int)CharacterSlot.Trinket2]; } set { _item[(int)CharacterSlot.Trinket2] = value; } }
		[XmlElement("MainHand")]
        public string _mainHand { get { return _item[(int)CharacterSlot.MainHand]; } set { _item[(int)CharacterSlot.MainHand] = value; } }
		[XmlElement("OffHand")]
        public string _offHand { get { return _item[(int)CharacterSlot.OffHand]; } set { _item[(int)CharacterSlot.OffHand] = value; } }
		[XmlElement("Ranged")]
        public string _ranged { get { return _item[(int)CharacterSlot.Ranged]; } set { _item[(int)CharacterSlot.Ranged] = value; } }
		[XmlElement("Projectile")]
        public string _projectile { get { return _item[(int)CharacterSlot.Projectile]; } set { _item[(int)CharacterSlot.Projectile] = value; } }
		[XmlElement("ProjectileBag")]
        public string _projectileBag { get { return _item[(int)CharacterSlot.ProjectileBag]; } set { _item[(int)CharacterSlot.ProjectileBag] = value; } }
		[XmlElement("HeadEnchant")]
        public int _headEnchant { get { return _itemEnchant[(int)CharacterSlot.Head]; } set { _itemEnchant[(int)CharacterSlot.Head] = value; } }
		[XmlElement("ShouldersEnchant")]
        public int _shouldersEnchant { get { return _itemEnchant[(int)CharacterSlot.Shoulders]; } set { _itemEnchant[(int)CharacterSlot.Shoulders] = value; } }
		[XmlElement("BackEnchant")]
        public int _backEnchant { get { return _itemEnchant[(int)CharacterSlot.Back]; } set { _itemEnchant[(int)CharacterSlot.Back] = value; } }
		[XmlElement("ChestEnchant")]
        public int _chestEnchant { get { return _itemEnchant[(int)CharacterSlot.Chest]; } set { _itemEnchant[(int)CharacterSlot.Chest] = value; } }
		[XmlElement("WristEnchant")]
        public int _wristEnchant { get { return _itemEnchant[(int)CharacterSlot.Wrist]; } set { _itemEnchant[(int)CharacterSlot.Wrist] = value; } }
		[XmlElement("HandsEnchant")]
        public int _handsEnchant { get { return _itemEnchant[(int)CharacterSlot.Hands]; } set { _itemEnchant[(int)CharacterSlot.Hands] = value; } }
		[XmlElement("LegsEnchant")]
        public int _legsEnchant { get { return _itemEnchant[(int)CharacterSlot.Legs]; } set { _itemEnchant[(int)CharacterSlot.Legs] = value; } }
		[XmlElement("FeetEnchant")]
        public int _feetEnchant { get { return _itemEnchant[(int)CharacterSlot.Feet]; } set { _itemEnchant[(int)CharacterSlot.Feet] = value; } }
		[XmlElement("Finger1Enchant")]
        public int _finger1Enchant { get { return _itemEnchant[(int)CharacterSlot.Finger1]; } set { _itemEnchant[(int)CharacterSlot.Finger1] = value; } }
		[XmlElement("Finger2Enchant")]
        public int _finger2Enchant { get { return _itemEnchant[(int)CharacterSlot.Finger2]; } set { _itemEnchant[(int)CharacterSlot.Finger2] = value; } }
		[XmlElement("MainHandEnchant")]
        public int _mainHandEnchant { get { return _itemEnchant[(int)CharacterSlot.MainHand]; } set { _itemEnchant[(int)CharacterSlot.MainHand] = value; } }
		[XmlElement("OffHandEnchant")]
        public int _offHandEnchant { get { return _itemEnchant[(int)CharacterSlot.OffHand]; } set { _itemEnchant[(int)CharacterSlot.OffHand] = value; } }
		[XmlElement("RangedEnchant")]
        public int _rangedEnchant { get { return _itemEnchant[(int)CharacterSlot.Ranged]; } set { _itemEnchant[(int)CharacterSlot.Ranged] = value; } }
		[XmlElement("CalculationOptions")]
		public SerializableDictionary<string, string> _serializedCalculationOptions = new SerializableDictionary<string, string>();
        //[XmlElement("Talents")]
        //public TalentTree _talents = new TalentTree();
		[XmlElement("AvailableItems")]
		public List<string> _availableItems = new List<string>();
		[XmlElement("CurrentModel")]
		public string _currentModel;
		[XmlElement("EnforceMetagemRequirements")]
		public bool _enforceMetagemRequirements = false;
		public int Level { get { return 80; } }

        public string CalculationToOptimize { get; set; }
        public List<OptimizationRequirement> OptimizationRequirements { get; set; }

		[XmlElement("WarriorTalents")]
		public string SerializableWarriorTalents { get { return WarriorTalents.ToString(); } 
			set { WarriorTalents = new WarriorTalents(value); } }
		[XmlElement("PaladinTalents")]
		public string SerializablePaladinTalents { get { return PaladinTalents.ToString(); } 
			set { PaladinTalents = new PaladinTalents(value); } }
        [XmlElement("HunterTalents")]
		public string SerializableHunterTalents { get { return HunterTalents.ToString(); } 
			set { HunterTalents = new HunterTalents(value); } }
        [XmlElement("RogueTalents")]
		public string SerializableRogueTalents { get { return RogueTalents.ToString(); } 
			set { RogueTalents = new RogueTalents(value); } }
        [XmlElement("PriestTalents")]
		public string SerializablePriestTalents { get { return PriestTalents.ToString(); } 
			set { PriestTalents = new PriestTalents(value); } }
        [XmlElement("ShamanTalents")]
		public string SerializableShamanTalents { get { return ShamanTalents.ToString(); } 
			set { ShamanTalents = new ShamanTalents(value); } }
        [XmlElement("MageTalents")]
		public string SerializableMageTalents { get { return MageTalents.ToString(); } 
			set { MageTalents = new MageTalents(value); } }
        [XmlElement("WarlockTalents")]
		public string SerializableWarlockTalents { get { return WarlockTalents.ToString(); } 
			set { WarlockTalents = new WarlockTalents(value); } }
        [XmlElement("DruidTalents")]
		public string SerializableDruidTalents { get { return DruidTalents.ToString(); } 
			set { DruidTalents = new DruidTalents(value); } }
        [XmlElement("DeathKnightTalents")]
		public string SerializableDeathKnightTalents { get { return DeathKnightTalents.ToString(); } 
			set { DeathKnightTalents = new DeathKnightTalents(value); } }

		[XmlIgnore]
		private WarriorTalents _warriorTalents = null;
		[XmlIgnore]
		private PaladinTalents _paladinTalents = null;
		[XmlIgnore]
		private HunterTalents _hunterTalents = null;
		[XmlIgnore]
		private RogueTalents _rogueTalents = null;
		[XmlIgnore]
		private PriestTalents _priestTalents = null;
		[XmlIgnore]
		private ShamanTalents _shamanTalents = null;
		[XmlIgnore]
		private MageTalents _mageTalents = null;
		[XmlIgnore]
		private WarlockTalents _warlockTalents = null;
		[XmlIgnore]
		private DruidTalents _druidTalents = null;
		[XmlIgnore]
		private DeathKnightTalents _deathKnightTalents = null;

		[XmlIgnore]
		public WarriorTalents WarriorTalents { get { return _warriorTalents = _warriorTalents ?? new WarriorTalents(); } set { _warriorTalents = value; } }
		[XmlIgnore]
		public PaladinTalents PaladinTalents { get { return _paladinTalents = _paladinTalents ?? new PaladinTalents(); } set { _paladinTalents = value; } }
		[XmlIgnore]
		public HunterTalents HunterTalents { get { return _hunterTalents = _hunterTalents ?? new HunterTalents(); } set { _hunterTalents = value; } }
		[XmlIgnore]
		public RogueTalents RogueTalents { get { return _rogueTalents = _rogueTalents ?? new RogueTalents(); } set { _rogueTalents = value; } }
		[XmlIgnore]
		public PriestTalents PriestTalents { get { return _priestTalents = _priestTalents ?? new PriestTalents(); } set { _priestTalents = value; } }
		[XmlIgnore]
		public ShamanTalents ShamanTalents { get { return _shamanTalents = _shamanTalents ?? new ShamanTalents(); } set { _shamanTalents = value; } }
		[XmlIgnore]
		public MageTalents MageTalents { get { return _mageTalents = _mageTalents ?? new MageTalents(); } set { _mageTalents = value; } }
		[XmlIgnore]
		public WarlockTalents WarlockTalents { get { return _warlockTalents = _warlockTalents ?? new WarlockTalents(); } set { _warlockTalents = value; } }
		[XmlIgnore]
		public DruidTalents DruidTalents { get { return _druidTalents = _druidTalents ?? new DruidTalents(); } set { _druidTalents = value; } }
		[XmlIgnore]
		public DeathKnightTalents DeathKnightTalents { get { return _deathKnightTalents = _deathKnightTalents ?? new DeathKnightTalents(); } set { _deathKnightTalents = value; } }

		[XmlIgnore]
		public TalentsBase CurrentTalents
		{
			get
			{
				switch (Class)
				{
					case CharacterClass.Warrior: return WarriorTalents;
					case CharacterClass.Paladin: return PaladinTalents;
					case CharacterClass.Hunter: return HunterTalents;
					case CharacterClass.Rogue: return RogueTalents;
					case CharacterClass.Priest: return PriestTalents;
					case CharacterClass.Shaman: return ShamanTalents;
					case CharacterClass.Mage: return MageTalents;
					case CharacterClass.Warlock: return WarlockTalents;
					case CharacterClass.Druid: return DruidTalents;
					case CharacterClass.DeathKnight: return DeathKnightTalents;
					default: return DruidTalents;
				}
			}
            set
			{
				switch (Class)
				{
                    case CharacterClass.Warrior: WarriorTalents = value as WarriorTalents; break;
                    case CharacterClass.Paladin: PaladinTalents = value as PaladinTalents; break;
                    case CharacterClass.Hunter: HunterTalents = value as HunterTalents; break;
                    case CharacterClass.Rogue: RogueTalents = value as RogueTalents; break;
                    case CharacterClass.Priest: PriestTalents = value as PriestTalents; break;
                    case CharacterClass.Shaman: ShamanTalents = value as ShamanTalents; break;
                    case CharacterClass.Mage: MageTalents = value as MageTalents; break;
                    case CharacterClass.Warlock: WarlockTalents = value as WarlockTalents; break;
                    case CharacterClass.Druid: DruidTalents = value as DruidTalents; break;
                    case CharacterClass.DeathKnight: DeathKnightTalents = value as DeathKnightTalents; break;
                    default: DruidTalents = value as DruidTalents; break;
				}
			}
		}

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
                    OnCalculationsInvalidated();
                }
            }
        }

        [XmlIgnore]
        public Character.CharacterClass Class
        {
            get { return _class; }
			set
			{
				_class = value;
				OnClassChanged();
			}
        }


        [XmlIgnore]
        public List<Buff> ActiveBuffs
        {
            get { return _activeBuffs; }
			set { _activeBuffs = value; }
        }

        public bool ActiveBuffsContains(string buff)
        {
            return _activeBuffs.FindIndex(x => x.Name == buff) >= 0;
        }

        public bool ActiveBuffsConflictingBuffContains(string conflictingBuff)
        {
			return _activeBuffs.FindIndex(x => x.ConflictingBuffs.Contains(conflictingBuff)) >= 0;
        }


        [XmlIgnore]
        public Item Head { get { return this[CharacterSlot.Head]; } set { this[CharacterSlot.Head] = value; } }
		[XmlIgnore]
        public Item Neck { get { return this[CharacterSlot.Neck]; } set { this[CharacterSlot.Neck] = value; } }
		[XmlIgnore]
        public Item Shoulders { get { return this[CharacterSlot.Shoulders]; } set { this[CharacterSlot.Shoulders] = value; } }
		[XmlIgnore]
        public Item Back { get { return this[CharacterSlot.Back]; } set { this[CharacterSlot.Back] = value; } }
		[XmlIgnore]
        public Item Chest { get { return this[CharacterSlot.Chest]; } set { this[CharacterSlot.Chest] = value; } }
		[XmlIgnore]
        public Item Shirt { get { return this[CharacterSlot.Shirt]; } set { this[CharacterSlot.Shirt] = value; } }
		[XmlIgnore]
        public Item Tabard { get { return this[CharacterSlot.Tabard]; } set { this[CharacterSlot.Tabard] = value; } }
		[XmlIgnore]
        public Item Wrist { get { return this[CharacterSlot.Wrist]; } set { this[CharacterSlot.Wrist] = value; } }
		[XmlIgnore]
        public Item Hands { get { return this[CharacterSlot.Hands]; } set { this[CharacterSlot.Hands] = value; } }
		[XmlIgnore]
        public Item Waist { get { return this[CharacterSlot.Waist]; } set { this[CharacterSlot.Waist] = value; } }
		[XmlIgnore]
        public Item Legs { get { return this[CharacterSlot.Legs]; } set { this[CharacterSlot.Legs] = value; } }
		[XmlIgnore]
        public Item Feet { get { return this[CharacterSlot.Feet]; } set { this[CharacterSlot.Feet] = value; } }
		[XmlIgnore]
        public Item Finger1 { get { return this[CharacterSlot.Finger1]; } set { this[CharacterSlot.Finger1] = value; } }
		[XmlIgnore]
        public Item Finger2 { get { return this[CharacterSlot.Finger2]; } set { this[CharacterSlot.Finger2] = value; } }
		[XmlIgnore]
        public Item Trinket1 { get { return this[CharacterSlot.Trinket1]; } set { this[CharacterSlot.Trinket1] = value; } }
		[XmlIgnore]
        public Item Trinket2 { get { return this[CharacterSlot.Trinket2]; } set { this[CharacterSlot.Trinket2] = value; } }
		[XmlIgnore]
        public Item MainHand { get { return this[CharacterSlot.MainHand]; } set { this[CharacterSlot.MainHand] = value; } }
		[XmlIgnore]
		public Item OffHand { get { return this[CharacterSlot.OffHand]; } set { this[CharacterSlot.OffHand] = value; } }
		[XmlIgnore]
        public Item Ranged { get { return this[CharacterSlot.Ranged]; } set { this[CharacterSlot.Ranged] = value; } }
		[XmlIgnore]
		public Item Projectile { get { return this[CharacterSlot.Projectile]; } set { this[CharacterSlot.Projectile] = value; } }
		[XmlIgnore]
		public Item ProjectileBag { get { return this[CharacterSlot.ProjectileBag]; } set { this[CharacterSlot.ProjectileBag] = value; } }

        [XmlIgnore]
		public Enchant HeadEnchant { get { return GetEnchantBySlot(CharacterSlot.Head); } set { SetEnchantBySlot(CharacterSlot.Head, value); } }
        [XmlIgnore]
		public Enchant ShouldersEnchant  { get { return GetEnchantBySlot(CharacterSlot.Shoulders); } set { SetEnchantBySlot(CharacterSlot.Shoulders, value); } }
        [XmlIgnore]
		public Enchant BackEnchant { get { return GetEnchantBySlot(CharacterSlot.Back); } set { SetEnchantBySlot(CharacterSlot.Back, value); } }
        [XmlIgnore]
		public Enchant ChestEnchant { get { return GetEnchantBySlot(CharacterSlot.Chest); } set { SetEnchantBySlot(CharacterSlot.Chest, value); } }
        [XmlIgnore]
		public Enchant WristEnchant { get { return GetEnchantBySlot(CharacterSlot.Wrist); } set { SetEnchantBySlot(CharacterSlot.Wrist, value); } }
        [XmlIgnore]
		public Enchant HandsEnchant { get { return GetEnchantBySlot(CharacterSlot.Hands); } set { SetEnchantBySlot(CharacterSlot.Hands, value); } }
        [XmlIgnore]
		public Enchant LegsEnchant { get { return GetEnchantBySlot(CharacterSlot.Legs); } set { SetEnchantBySlot(CharacterSlot.Legs, value); } }
        [XmlIgnore]
		public Enchant FeetEnchant { get { return GetEnchantBySlot(CharacterSlot.Feet); } set { SetEnchantBySlot(CharacterSlot.Feet, value); } }
        [XmlIgnore]
		public Enchant Finger1Enchant { get { return GetEnchantBySlot(CharacterSlot.Finger1); } set { SetEnchantBySlot(CharacterSlot.Finger1, value); } }
        [XmlIgnore]
		public Enchant Finger2Enchant { get { return GetEnchantBySlot(CharacterSlot.Finger2); } set { SetEnchantBySlot(CharacterSlot.Finger2, value); } }
        [XmlIgnore]
		public Enchant MainHandEnchant { get { return GetEnchantBySlot(CharacterSlot.MainHand); } set { SetEnchantBySlot(CharacterSlot.MainHand, value); } }
        [XmlIgnore]
		public Enchant OffHandEnchant { get { return GetEnchantBySlot(CharacterSlot.OffHand); } set { SetEnchantBySlot(CharacterSlot.OffHand, value); } }
        [XmlIgnore]
		public Enchant RangedEnchant { get { return GetEnchantBySlot(CharacterSlot.Ranged); } set { SetEnchantBySlot(CharacterSlot.Ranged, value); } }

        [XmlIgnore]
        private StatConversion _statConversion = null;
        [XmlIgnore]
        public StatConversion StatConversion
        {
            get
            {
                if (_statConversion == null)
                    _statConversion = new StatConversion(this);
                return _statConversion;
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
                    ret = Calculations.GetModel(CurrentModel).DeserializeDataObject(_serializedCalculationOptions[CurrentModel]);
                    // set parent Character for models that need backward link
                    System.Reflection.PropertyInfo propertyInfo = ret.GetType().GetProperty("Character", typeof(Character));
                    if (propertyInfo != null) propertyInfo.SetValue(ret, this, null);
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

        public void AssignAllTalentsFromCharacter(Character character)
        {
            WarriorTalents = character.WarriorTalents.Clone();
            PaladinTalents = character.PaladinTalents.Clone();
            HunterTalents = character.HunterTalents.Clone();
            RogueTalents = character.RogueTalents.Clone();
            PriestTalents = character.PriestTalents.Clone();
            ShamanTalents = character.ShamanTalents.Clone();
            MageTalents = character.MageTalents.Clone();
            WarlockTalents = character.WarlockTalents.Clone();
            DruidTalents = character.DruidTalents.Clone();
            DeathKnightTalents = character.DeathKnightTalents.Clone();
        }

		//[XmlIgnore]
		//public TalentTree Talents
		//{
		//    get { return _talents; }
		//    set { _talents = value; }
		//}

        // list of 5-tuples itemid.gem1id.gem2id.gem3id.enchantid, itemid is required, others can use * for wildcard
        // for backward compatibility use just itemid instead of itemid.*.*.*.*
        // -id represents enchants
		[XmlIgnore]
		public List<string> AvailableItems
		{
			get { return _availableItems; }
            set
            {
                _availableItems = value;
                OnAvailableItemsChanged();
            }
		}

        public Item[] GetAvailableItems()
        {
            foreach (string gemmedid in _availableItems)
            {
            }
            return null;
        }    

        public bool IsEquipped(Item itemToBeChecked)
        {
            CharacterSlot slot = Character.GetCharacterSlotByItemSlot(itemToBeChecked.Slot);
            if (slot == CharacterSlot.Finger1)
            {
                return IsEquipped(itemToBeChecked, CharacterSlot.Finger1) || IsEquipped(itemToBeChecked, CharacterSlot.Finger2);
			}
			else if (itemToBeChecked.Slot == Rawr.Item.ItemSlot.OneHand)
			{
				return IsEquipped(itemToBeChecked, CharacterSlot.MainHand) || IsEquipped(itemToBeChecked, CharacterSlot.OffHand);
			}
			else if (itemToBeChecked.Slot == Rawr.Item.ItemSlot.Trinket)
			{
				return IsEquipped(itemToBeChecked, CharacterSlot.Trinket1) || IsEquipped(itemToBeChecked, CharacterSlot.Trinket2);
			}
			else
				return IsEquipped(itemToBeChecked, slot);
        }
        public bool IsEquipped(Item itemToBeChecked, CharacterSlot slot)
        { 
			return itemToBeChecked != null && this[slot] != null && itemToBeChecked.GemmedId == this[slot].GemmedId;
        }

        public static Character.CharacterSlot GetCharacterSlotByItemSlot(Item.ItemSlot slot)
        {
            
            //note: When converting ItemSlot.Finger and ItemSlot.Trinket, this will ALWAYS
            //place them in Slot 1 of the 2 possibilities. Items listed as OneHand or TwoHand 
            //in their Itemslot profile, will be parsed into the MainHand CharacterSlot.
            
            switch (slot)
            {
               
                case Rawr.Item.ItemSlot.Projectile: return Character.CharacterSlot.Projectile;
                case Rawr.Item.ItemSlot.Head: return Character.CharacterSlot.Head;
                case Rawr.Item.ItemSlot.Neck: return Character.CharacterSlot.Neck;
                case Rawr.Item.ItemSlot.Shoulders: return Character.CharacterSlot.Shoulders;
                case Rawr.Item.ItemSlot.Chest: return Character.CharacterSlot.Chest;
                case Rawr.Item.ItemSlot.Waist: return Character.CharacterSlot.Waist;
                case Rawr.Item.ItemSlot.Legs: return Character.CharacterSlot.Legs;
                case Rawr.Item.ItemSlot.Feet: return Character.CharacterSlot.Feet;
                case Rawr.Item.ItemSlot.Wrist: return Character.CharacterSlot.Wrist;
                case Rawr.Item.ItemSlot.Hands: return Character.CharacterSlot.Hands;
                case Rawr.Item.ItemSlot.Finger: return Character.CharacterSlot.Finger1;
            //    case Item.ItemSlot.Finger: return Character.CharacterSlot.Finger2;
                case Rawr.Item.ItemSlot.Trinket: return Character.CharacterSlot.Trinket1;
            //    case Item.ItemSlot.Trinket: return Character.CharacterSlot.Trinket2;
                case Rawr.Item.ItemSlot.Back: return Character.CharacterSlot.Back;
                case Rawr.Item.ItemSlot.OneHand: return Character.CharacterSlot.MainHand;
                case Rawr.Item.ItemSlot.TwoHand: return Character.CharacterSlot.MainHand;
                case Rawr.Item.ItemSlot.MainHand: return Character.CharacterSlot.MainHand;
                case Rawr.Item.ItemSlot.OffHand: return Character.CharacterSlot.OffHand;
                case Rawr.Item.ItemSlot.Ranged: return Character.CharacterSlot.Ranged;
                case Rawr.Item.ItemSlot.ProjectileBag: return Character.CharacterSlot.ProjectileBag;
                case Rawr.Item.ItemSlot.Tabard: return Character.CharacterSlot.Tabard;
                case Rawr.Item.ItemSlot.Shirt: return Character.CharacterSlot.Shirt;
                case Rawr.Item.ItemSlot.Prismatic: return Character.CharacterSlot.Gems;
                case Rawr.Item.ItemSlot.Meta: return Character.CharacterSlot.Metas;
                default: return Character.CharacterSlot.None;
            }
        }
        public ItemAvailability GetItemAvailability(Item item)
        {
            string id = item.Id.ToString();
            string anyGem = id + ".*.*.*";
            List<string> list = _availableItems.FindAll(x => x.StartsWith(id));
            if (list.Contains(item.GemmedId + ".*"))
            {
                return ItemAvailability.Available;
            }
            else if (list.FindIndex(x => x.StartsWith(item.GemmedId)) >= 0)
            {
                return ItemAvailability.AvailableWithEnchantRestrictions;
            }
            if (list.Contains(id))
            {
                return ItemAvailability.RegemmingAllowed;
            }
            else if (list.FindIndex(x => x.StartsWith(anyGem)) >= 0)
            {
                return ItemAvailability.RegemmingAllowedWithEnchantRestrictions;
            }
            else
            {
                return ItemAvailability.NotAvailabe;
            }
        }

        public void ToggleItemAvailability(Item item, bool regemmingAllowed)
        {
            string id = item.Id.ToString();
            string anyGem = id + ".*.*.*";

            if (id.StartsWith("-") || regemmingAllowed || item.IsGem)
            {
                // all enabled toggle
                if (_availableItems.Contains(id) || _availableItems.FindIndex(x => x.StartsWith(anyGem)) >= 0)
                {
                    _availableItems.Remove(id);
                    _availableItems.RemoveAll(x => x.StartsWith(anyGem));
                }
                else
                {
                    _availableItems.Add(id);
                }
            }
            else
            {
                // enabled toggle
                if (_availableItems.FindIndex(x => x.StartsWith(item.GemmedId)) >= 0)
                {
                    _availableItems.RemoveAll(x => x.StartsWith(item.GemmedId));
                }
                else
                {
                    _availableItems.Add(item.GemmedId + ".*");
                }
            }
            OnAvailableItemsChanged();
        }

        public void ToggleAvailableItemEnchantRestriction(Item item, Enchant enchant)
        {
            string id = item.Id.ToString();
            string anyGem = id + ".*.*.*";
            ItemAvailability availability = GetItemAvailability(item);
            switch (availability)
            {
                case ItemAvailability.Available:
                    if (enchant != null)
                    {
                        _availableItems.RemoveAll(x => x.StartsWith(item.GemmedId));
                        _availableItems.Add(item.GemmedId + "." + enchant.Id.ToString());
                    }
                    else
                    {
                        // any => all
                        _availableItems.RemoveAll(x => x.StartsWith(item.GemmedId));
                        foreach (Enchant e in Enchant.FindEnchants(item.Slot))
                        {
                            _availableItems.Add(item.GemmedId + "." + e.Id.ToString());
                        }
                    }
                    break;
                case ItemAvailability.AvailableWithEnchantRestrictions:
                    if (enchant != null)
                    {
                        if (_availableItems.Contains(item.GemmedId + "." + enchant.Id.ToString()))
                        {
                            _availableItems.Remove(item.GemmedId + "." + enchant.Id.ToString());
                        }
                        else
                        {
                            _availableItems.Add(item.GemmedId + "." + enchant.Id.ToString());
                        }
                    }
                    else
                    {
                        _availableItems.RemoveAll(x => x.StartsWith(item.GemmedId));
                        _availableItems.Add(item.GemmedId + ".*");
                    }
                    break;
                case ItemAvailability.RegemmingAllowed:
                    if (enchant != null)
                    {
                        _availableItems.RemoveAll(x => x.StartsWith(id));
                        _availableItems.Add(anyGem + "." + enchant.Id.ToString());
                    }
                    else
                    {
                        // any => all
                        _availableItems.RemoveAll(x => x.StartsWith(id));
                        foreach (Enchant e in Enchant.FindEnchants(item.Slot))
                        {
                            _availableItems.Add(anyGem + "." + e.Id.ToString());
                        }
                    }
                    break;
                case ItemAvailability.RegemmingAllowedWithEnchantRestrictions:
                    if (enchant != null)
                    {
                        if (_availableItems.Contains(anyGem + "." + enchant.Id.ToString()))
                        {
                            _availableItems.Remove(anyGem + "." + enchant.Id.ToString());
                        }
                        else
                        {
                            _availableItems.Add(anyGem + "." + enchant.Id.ToString());
                        }
                    }
                    else
                    {
                        _availableItems.RemoveAll(x => x.StartsWith(id));
                        _availableItems.Add(id);
                    }
                    break;
                case ItemAvailability.NotAvailabe:
                    if (enchant != null)
                    {
                        _availableItems.Add(anyGem + "." + enchant.Id.ToString());
                    }
                    else
                    {
                        _availableItems.Add(id);
                    }
                    break;
            }
            OnAvailableItemsChanged();
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
				case Rawr.Item.ItemSlot.Head:
					return HeadEnchant;
				case Rawr.Item.ItemSlot.Shoulders:
					return ShouldersEnchant;
				case Rawr.Item.ItemSlot.Back:
					return BackEnchant;
				case Rawr.Item.ItemSlot.Chest:
					return ChestEnchant;
				case Rawr.Item.ItemSlot.Wrist:
					return WristEnchant;
				case Rawr.Item.ItemSlot.Hands:
					return HandsEnchant;
				case Rawr.Item.ItemSlot.Legs:
					return LegsEnchant;
				case Rawr.Item.ItemSlot.Feet:
					return FeetEnchant;
				case Rawr.Item.ItemSlot.Finger:
					return Finger1Enchant;
				case Rawr.Item.ItemSlot.MainHand:
				case Rawr.Item.ItemSlot.OneHand:
				case Rawr.Item.ItemSlot.TwoHand:
					return MainHandEnchant;
				case Rawr.Item.ItemSlot.OffHand:
					return OffHandEnchant;
				case Rawr.Item.ItemSlot.Ranged:
					return RangedEnchant;
				default:
					return null;
			}
		}

        //private static Item.ItemSlot[] characterSlot2ItemSlot = new Item.ItemSlot[] { Item.ItemSlot.Projectile, Item.ItemSlot.Head, Item.ItemSlot.Neck, Item.ItemSlot.Shoulders, Item.ItemSlot.Chest, Item.ItemSlot.Waist, Item.ItemSlot.Legs, Item.ItemSlot.Feet, Item.ItemSlot.Wrist, Item.ItemSlot.Hands, Item.ItemSlot.Finger, Item.ItemSlot.Finger, Item.ItemSlot.Trinket, Item.ItemSlot.Trinket, Item.ItemSlot.Back, Item.ItemSlot.MainHand, Item.ItemSlot.OffHand, Item.ItemSlot.Ranged, Item.ItemSlot.ProjectileBag, Item.ItemSlot.Tabard, Item.ItemSlot.Shirt };
        public Enchant GetEnchantBySlot(Character.CharacterSlot slot)
        {
            int i = (int)slot;
            if (i < 0 || i > 20) return null;
            Enchant e = _itemEnchantCached[i];
            if (e == null)
            {
				e = _itemEnchantCached[i] = Enchant.FindEnchant(_itemEnchant[i], Rawr.Item.GetItemSlotByCharacterSlot(slot));
            }
            return e;
        }

        public bool IsEnchantable(Character.CharacterSlot slot)
        {
            switch (slot)
            {
                case Character.CharacterSlot.Head:
                case Character.CharacterSlot.Shoulders:
                case Character.CharacterSlot.Back:
                case Character.CharacterSlot.Chest:
                case Character.CharacterSlot.Wrist:
                case Character.CharacterSlot.Hands:
                case Character.CharacterSlot.Legs:
                case Character.CharacterSlot.Feet:
                case Character.CharacterSlot.Finger1:
                case Character.CharacterSlot.Finger2:
                case Character.CharacterSlot.MainHand:
                case Character.CharacterSlot.OffHand:
                case Character.CharacterSlot.Ranged:
                    return true;
                default:
                    return false;
            }
        }

        public bool IsEnchantable(Item.ItemSlot slot)
        {
            switch (slot)
            {
                case Rawr.Item.ItemSlot.Head:
                case Rawr.Item.ItemSlot.Shoulders:
                case Rawr.Item.ItemSlot.Back:
                case Rawr.Item.ItemSlot.Chest:
                case Rawr.Item.ItemSlot.Wrist:
                case Rawr.Item.ItemSlot.Hands:
                case Rawr.Item.ItemSlot.Legs:
                case Rawr.Item.ItemSlot.Feet:
                case Rawr.Item.ItemSlot.Finger:
                case Rawr.Item.ItemSlot.TwoHand:
                case Rawr.Item.ItemSlot.MainHand:
                case Rawr.Item.ItemSlot.OneHand:
                case Rawr.Item.ItemSlot.OffHand:
                case Rawr.Item.ItemSlot.Ranged:
                    return true;
                default:
                    return false;
            }
        }

		public void SetEnchantBySlot(Item.ItemSlot slot, Enchant enchant)
		{
			switch (slot)
			{
				case Rawr.Item.ItemSlot.Head:
					HeadEnchant = enchant;
					break;
				case Rawr.Item.ItemSlot.Shoulders:
					ShouldersEnchant = enchant;
					break;
				case Rawr.Item.ItemSlot.Back:
					BackEnchant = enchant;
					break;
				case Rawr.Item.ItemSlot.Chest:
					ChestEnchant = enchant;
					break;
				case Rawr.Item.ItemSlot.Wrist:
					WristEnchant = enchant;
					break;
				case Rawr.Item.ItemSlot.Hands:
					HandsEnchant = enchant;
					break;
				case Rawr.Item.ItemSlot.Legs:
					LegsEnchant = enchant;
					break;
				case Rawr.Item.ItemSlot.Feet:
					FeetEnchant = enchant;
					break;
				case Rawr.Item.ItemSlot.Finger:
					Finger1Enchant = enchant;
					break;
				case Rawr.Item.ItemSlot.MainHand:
				case Rawr.Item.ItemSlot.OneHand:
				case Rawr.Item.ItemSlot.TwoHand:
					MainHandEnchant = enchant;
					break;
				case Rawr.Item.ItemSlot.OffHand:
					OffHandEnchant = enchant;
					break;
				case Rawr.Item.ItemSlot.Ranged:
					RangedEnchant = enchant;
					break;
			}
		}

        public void SetEnchantBySlot(Character.CharacterSlot slot, Enchant enchant)
        {
            int i = (int)slot;
            if (i < 0 || i > 20) return;
            _itemEnchant[i] = enchant == null ? 0 : enchant.Id;
            _itemEnchantCached[i] = enchant;
			OnCalculationsInvalidated();
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

        #region Cached item tracking and invalidation
        // hook idschanged event on equipped items, only hook this for main character, clones and optimization instances are short lived and don't need to track these changes
        [XmlIgnore]
        private bool _trackEquippedItemChanges = true;
        [XmlIgnore]
        private bool TrackEquippedItemChanges
        {
            get
            {
                return _trackEquippedItemChanges;
            }
            set
            {
                if (_trackEquippedItemChanges != value)
                {
                    _trackEquippedItemChanges = value;
                    if (_trackEquippedItemChanges)
                    {
                        // hook events
                        for (int i = 0; i < _itemCached.Length; i++)
                        {
                            if (_itemCached[i] != null) _itemCached[i].IdsChanged += new EventHandler(_itemCached_IdsChanged);
                        }
                    }
                    else
                    {
                        // unhook events
                        for (int i = 0; i < _itemCached.Length; i++)
                        {
                            if (_itemCached[i] != null) _itemCached[i].IdsChanged -= new EventHandler(_itemCached_IdsChanged);
                        }
                    }
                }
            }
        }

        void _itemCached_IdsChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < _itemCached.Length; i++)
            {
                if (sender == _itemCached[i])
                {
                    _itemCached[i].IdsChanged -= new EventHandler(_itemCached_IdsChanged);
                    _itemCached[i] = null;
                }
            }
        }
        #endregion

        // cache gem counts as this takes the most time of accumulating item stats
        // this becomes invalid when items on character change, invalidate in OnItemsChanged
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
                redGemCount = GetGemColorCount(Rawr.Item.ItemSlot.Red);
                yellowGemCount = GetGemColorCount(Rawr.Item.ItemSlot.Yellow);
                blueGemCount = GetGemColorCount(Rawr.Item.ItemSlot.Blue);
                gemCountValid = true;
            }
        }

        private int GetItemGemColorCount(Rawr.Item item, Rawr.Item.ItemSlot slotColor)
        {
            int count = 0;
            if (item != null)
            {
                if (item.Gem1 != null && Rawr.Item.GemMatchesSlot(item.Gem1, slotColor)) count++;
                if (item.Gem2 != null && Rawr.Item.GemMatchesSlot(item.Gem2, slotColor)) count++;
                if (item.Gem3 != null && Rawr.Item.GemMatchesSlot(item.Gem3, slotColor)) count++;
            }
            return count;
        }

        public int GetGemColorCount(Rawr.Item.ItemSlot slotColor)
        {
            int count = 0;
            /*foreach (CharacterSlot slot in CharacterSlots)
			{
				Item item = this[slot];
				if (item == null) continue;

				if (Item.GemMatchesSlot(item.Gem1, slotColor)) count++;
				if (Item.GemMatchesSlot(item.Gem2, slotColor)) count++;
				if (Item.GemMatchesSlot(item.Gem3, slotColor)) count++;
			}*/
            // unroll loop because the switch in this[slot] is very expensive
            count += GetItemGemColorCount(Head, slotColor);
            count += GetItemGemColorCount(Neck, slotColor);
            count += GetItemGemColorCount(Shoulders, slotColor);
            count += GetItemGemColorCount(Back, slotColor);
            count += GetItemGemColorCount(Chest, slotColor);
            count += GetItemGemColorCount(Shirt, slotColor);
            count += GetItemGemColorCount(Tabard, slotColor);
            count += GetItemGemColorCount(Wrist, slotColor);
            count += GetItemGemColorCount(Hands, slotColor);
            count += GetItemGemColorCount(Waist, slotColor);
            count += GetItemGemColorCount(Legs, slotColor);
            count += GetItemGemColorCount(Feet, slotColor);
            count += GetItemGemColorCount(Finger1, slotColor);
            count += GetItemGemColorCount(Finger2, slotColor);
            count += GetItemGemColorCount(Trinket1, slotColor);
            count += GetItemGemColorCount(Trinket2, slotColor);
            count += GetItemGemColorCount(MainHand, slotColor);
            count += GetItemGemColorCount(OffHand, slotColor);
            count += GetItemGemColorCount(Ranged, slotColor);
            count += GetItemGemColorCount(Projectile, slotColor);
            count += GetItemGemColorCount(ProjectileBag, slotColor);

            return count;
        }
		
		public event EventHandler AvailableItemsChanged;
		public void OnAvailableItemsChanged()
		{
			if (AvailableItemsChanged != null)
				AvailableItemsChanged(this, EventArgs.Empty);
		}
		
		public event EventHandler CalculationsInvalidated;
		public void OnCalculationsInvalidated()
		{
            gemCountValid = false; // invalidate gem counts
            if (IsLoading) return;
			RecalculateSetBonuses();

			if (CalculationsInvalidated != null) CalculationsInvalidated(this, EventArgs.Empty);
		}

		public event EventHandler ClassChanged;
		public void OnClassChanged()
		{
			if (ClassChanged != null)
				ClassChanged(this, EventArgs.Empty);
		}

		private void RecalculateSetBonusesFromCache()
        {
            //Compute Set Bonuses
            Dictionary<string, int> setCounts = new Dictionary<string, int>();
            for (int slot = 0; slot < _itemCached.Length; slot++)
            {
                Item item = _itemCached[slot];
                if (item != null && !string.IsNullOrEmpty(item.SetName))
                {
                    int count;
                    setCounts.TryGetValue(item.SetName, out count);
                    setCounts[item.SetName] = count + 1;
                }
            }

            // eliminate searching in active buffs: first remove all set bonuses, then add active ones
            ActiveBuffs.RemoveAll(buff => !string.IsNullOrEmpty(buff.SetName));
            foreach (KeyValuePair<string, int> pair in setCounts)
            {
                foreach (Buff buff in Buff.GetSetBonuses(pair.Key))
                {
                    if (pair.Value >= buff.SetThreshold)
                    {
                        ActiveBuffs.Add(buff);
                    }
                }
            }
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
                    int count;
                    setCounts.TryGetValue(item.SetName, out count);
					setCounts[item.SetName] = count + 1;
				}
			}

            // eliminate searching in active buffs: first remove all set bonuses, then add active ones
            ActiveBuffs.RemoveAll(buff => !string.IsNullOrEmpty(buff.SetName));
            foreach (KeyValuePair<string, int> pair in setCounts)
            {
                foreach (Buff buff in Buff.GetSetBonuses(pair.Key))
                {
                    if (pair.Value >= buff.SetThreshold)
                    {
                        ActiveBuffs.Add(buff);
                    }
                }
            }
        }

        [XmlIgnore]
        public Item this[CharacterSlot slot]
        {
            get
            {
                int i = (int)slot;
                if (i < 0 || i > 20) return null;
                Item item;
                if ((item = _itemCached[i]) == null)
                {
                    item = _itemCached[i] = Rawr.Item.LoadFromId(_item[i], "Equipped Item");
                    if (item != null && _trackEquippedItemChanges) item.IdsChanged += new EventHandler(_itemCached_IdsChanged);
                }
                return item;
            }
            set
            {
                int i = (int)slot;
                if (i < 0 || i > 20) return;
                if (value == null || _item[i] != value.GemmedId)
                {
                    _item[i] = value != null ? value.GemmedId : null;
                    if (_itemCached[i] != null && _trackEquippedItemChanges) _itemCached[i].IdsChanged -= new EventHandler(_itemCached_IdsChanged);
                    _itemCached[i] = value;
                    if (_itemCached[i] != null && _trackEquippedItemChanges) _itemCached[i].IdsChanged += new EventHandler(_itemCached_IdsChanged);
                    OnCalculationsInvalidated();
                }
            }
        }

        public string[] GetAllEquipedAndAvailableGearIds()
        {
            Dictionary<string, bool> _ids = new Dictionary<string, bool>();
            if (_back != null) _ids[_back] = true;
            if (_chest != null) _ids[_chest] = true;
            if (_feet != null) _ids[_feet] = true;
            if (_finger1 != null) _ids[_finger1] = true;
            if (_finger2 != null) _ids[_finger2] = true;
            if (_hands != null) _ids[_hands] = true;
            if (_head != null) _ids[_head] = true;
            if (_legs != null) _ids[_legs] = true;
            if (_mainHand != null) _ids[_mainHand] = true;
            if (_neck != null) _ids[_neck] = true;
            if (_offHand != null) _ids[_offHand] = true;
            if (_projectile != null) _ids[_projectile] = true;
            if (_projectileBag != null) _ids[_projectileBag] = true;
            if (_ranged != null) _ids[_ranged] = true;
            if (_shirt != null) _ids[_shirt] = true;
            if (_shoulders != null) _ids[_shoulders] = true;
            if (_tabard != null) _ids[_tabard] = true;
            if (_trinket1 != null) _ids[_trinket1] = true;
            if (_trinket2 != null) _ids[_trinket2] = true;
            if (_waist != null) _ids[_waist] = true;
            if (_wrist != null) _ids[_wrist] = true;
            foreach (string xid in AvailableItems)
            {
                if (!xid.StartsWith("-"))
                {
                    int dot = xid.LastIndexOf('.');
                    _ids[(dot >= 0) ? xid.Substring(0, dot).Replace(".*.*.*", "") : xid] = true;
                }
            }
            return new List<string>(_ids.Keys).ToArray();
        }

		public CharacterSlot[] GetEquippedSlots(Item item)
		{
			List<CharacterSlot> listSlots = new List<CharacterSlot>();
            foreach (CharacterSlot slot in CharacterSlots)
				if (this[slot] == item)
					listSlots.Add(slot);
			return listSlots.ToArray();
		}

		public enum CharacterRegion { US, EU, KR, TW, CN }
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
            Projectile = 0,
            Head = 1,
            Neck = 2,
            Shoulders = 3,
            Chest = 4,
            Waist = 5,
            Legs = 6,
            Feet = 7,
            Wrist = 8,
            Hands = 9,
            Finger1 = 10,
            Finger2 = 11,
            Trinket1 = 12,
            Trinket2 = 13,
            Back = 14,
            MainHand = 15,
			OffHand = 16,
            Ranged = 17,
            ProjectileBag = 18,
            Tabard = 19,
            Shirt = 20,
			
			Gems = 100,
			Metas = 101,
            AutoSelect = 1000,
        }

        public static CharacterSlot GetCharacterSlotFromId(int slotId)
        {
            Character.CharacterSlot cslot = CharacterSlot.None;
            switch (slotId)
            {
                case -1:
                    cslot = Character.CharacterSlot.None;
                    break;
                case 1:
                    cslot = Character.CharacterSlot.Head;
                    break;
                case 2:
                    cslot = Character.CharacterSlot.Neck;
                    break;
                case 3:
                    cslot = Character.CharacterSlot.Shoulders;
                    break;
                case 15:
                    cslot = Character.CharacterSlot.Back;
                    break;
                case 5:
                    cslot = Character.CharacterSlot.Chest;
                    break;
                case 4:
                    cslot = Character.CharacterSlot.Shirt;
                    break;
                case 19:
                    cslot = Character.CharacterSlot.Tabard;
                    break;
                case 9:
                    cslot = Character.CharacterSlot.Wrist;
                    break;
                case 10:
                    cslot = Character.CharacterSlot.Hands;
                    break;
                case 6:
                    cslot = Character.CharacterSlot.Waist;
                    break;
                case 7:
                    cslot = Character.CharacterSlot.Legs;
                    break;
                case 8:
                    cslot = Character.CharacterSlot.Feet;
                    break;
                case 11:
                    cslot = Character.CharacterSlot.Finger1;
                    break;
                case 12:
                    cslot = Character.CharacterSlot.Finger2;
                    break;
                case 13:
                    cslot = Character.CharacterSlot.Trinket1;
                    break;
                case 14:
                    cslot = Character.CharacterSlot.Trinket2;
                    break;
                case 16:
                    cslot = Character.CharacterSlot.MainHand;
                    break;
                case 17:
                    cslot = Character.CharacterSlot.OffHand;
                    break;
                case 18:
                    cslot = Character.CharacterSlot.Ranged;
                    break;
                case 0:
                    cslot = Character.CharacterSlot.Projectile;
                    break;
                case 102:
                    cslot = Character.CharacterSlot.ProjectileBag;
                    break;
            }
            return cslot;
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
            Druid = 11,
            DeathKnight = 12
        }

        public enum ItemAvailability
        {
            NotAvailabe,
            Available,
            AvailableWithEnchantRestrictions,
            RegemmingAllowed,
            RegemmingAllowedWithEnchantRestrictions
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
            Enchant enchantHead, Enchant enchantShoulders, Enchant enchantBack, Enchant enchantChest, Enchant enchantWrist, Enchant enchantHands, Enchant enchantLegs, Enchant enchantFeet, Enchant enchantFinger1, Enchant enchantFinger2, Enchant enchantMainHand, Enchant enchantOffHand, Enchant enchantRanged, bool trackEquippedItemChanges)
        {
            _trackEquippedItemChanges = trackEquippedItemChanges;
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

        // the following are special contructors used by optimizer, they assume the cached items/enchant are always used, and the underlying gemmedid/enchantid are never used
        public Character(string name, string realm, Character.CharacterRegion region, CharacterRace race, Item head, Item neck, Item shoulders, Item back, Item chest, Item shirt, Item tabard,
                Item wrist, Item hands, Item waist, Item legs, Item feet, Item finger1, Item finger2, Item trinket1, Item trinket2, Item mainHand, Item offHand, Item ranged, Item projectile, Item projectileBag,
            Enchant enchantHead, Enchant enchantShoulders, Enchant enchantBack, Enchant enchantChest, Enchant enchantWrist, Enchant enchantHands, Enchant enchantLegs, Enchant enchantFeet, Enchant enchantFinger1, Enchant enchantFinger2, Enchant enchantMainHand, Enchant enchantOffHand, Enchant enchantRanged, List<Buff> activeBuffs, bool trackEquippedItemChanges, string model)
        {
            _trackEquippedItemChanges = trackEquippedItemChanges;
            IsLoading = true;
            _name = name;
            _realm = realm;
            _region = region;
            _race = race;
            _itemCached[(int)CharacterSlot.Head] = head;
            _itemCached[(int)CharacterSlot.Neck] = neck;
            _itemCached[(int)CharacterSlot.Shoulders] = shoulders;
            _itemCached[(int)CharacterSlot.Back] = back;
            _itemCached[(int)CharacterSlot.Chest] = chest;
            _itemCached[(int)CharacterSlot.Shirt] = shirt;
            _itemCached[(int)CharacterSlot.Tabard] = tabard;
            _itemCached[(int)CharacterSlot.Wrist] = wrist;
            _itemCached[(int)CharacterSlot.Hands] = hands;
            _itemCached[(int)CharacterSlot.Waist] = waist;
            _itemCached[(int)CharacterSlot.Legs] = legs;
            _itemCached[(int)CharacterSlot.Feet] = feet;
            _itemCached[(int)CharacterSlot.Finger1] = finger1;
            _itemCached[(int)CharacterSlot.Finger2] = finger2;
            _itemCached[(int)CharacterSlot.Trinket1] = trinket1;
            _itemCached[(int)CharacterSlot.Trinket2] = trinket2;
            _itemCached[(int)CharacterSlot.MainHand] = mainHand;
            _itemCached[(int)CharacterSlot.OffHand] = offHand;
            _itemCached[(int)CharacterSlot.Ranged] = ranged;
            _itemCached[(int)CharacterSlot.Projectile] = projectile;
            _itemCached[(int)CharacterSlot.ProjectileBag] = projectileBag;

            _itemEnchantCached[(int)CharacterSlot.Head] = enchantHead;
            _itemEnchantCached[(int)CharacterSlot.Shoulders] = enchantShoulders;
            _itemEnchantCached[(int)CharacterSlot.Back] = enchantBack;
            _itemEnchantCached[(int)CharacterSlot.Chest] = enchantChest;
            _itemEnchantCached[(int)CharacterSlot.Wrist] = enchantWrist;
            _itemEnchantCached[(int)CharacterSlot.Hands] = enchantHands;
            _itemEnchantCached[(int)CharacterSlot.Legs] = enchantLegs;
            _itemEnchantCached[(int)CharacterSlot.Feet] = enchantFeet;
            _itemEnchantCached[(int)CharacterSlot.Finger1] = enchantFinger1;
            _itemEnchantCached[(int)CharacterSlot.Finger2] = enchantFinger2;
            _itemEnchantCached[(int)CharacterSlot.MainHand] = enchantMainHand;
            _itemEnchantCached[(int)CharacterSlot.OffHand] = enchantOffHand;
            _itemEnchantCached[(int)CharacterSlot.Ranged] = enchantRanged;
            IsLoading = false;
            ActiveBuffs.AddRange(activeBuffs);
            CurrentModel = model;
            RecalculateSetBonusesFromCache();
        }

        public Character(string name, string realm, Character.CharacterRegion region, CharacterRace race, Item[] items,
            Enchant enchantHead, Enchant enchantShoulders, Enchant enchantBack, Enchant enchantChest, Enchant enchantWrist, Enchant enchantHands, Enchant enchantLegs, Enchant enchantFeet, Enchant enchantFinger1, Enchant enchantFinger2, Enchant enchantMainHand, Enchant enchantOffHand, Enchant enchantRanged, List<Buff> activeBuffs, bool trackEquippedItemChanges, string model)
        {
            _trackEquippedItemChanges = trackEquippedItemChanges;
            IsLoading = true;
            _name = name;
            _realm = realm;
            _region = region;
            _race = race;
            Array.Copy(items, _itemCached, items.Length);

            _itemEnchantCached[(int)CharacterSlot.Head] = enchantHead;
            _itemEnchantCached[(int)CharacterSlot.Shoulders] = enchantShoulders;
            _itemEnchantCached[(int)CharacterSlot.Back] = enchantBack;
            _itemEnchantCached[(int)CharacterSlot.Chest] = enchantChest;
            _itemEnchantCached[(int)CharacterSlot.Wrist] = enchantWrist;
            _itemEnchantCached[(int)CharacterSlot.Hands] = enchantHands;
            _itemEnchantCached[(int)CharacterSlot.Legs] = enchantLegs;
            _itemEnchantCached[(int)CharacterSlot.Feet] = enchantFeet;
            _itemEnchantCached[(int)CharacterSlot.Finger1] = enchantFinger1;
            _itemEnchantCached[(int)CharacterSlot.Finger2] = enchantFinger2;
            _itemEnchantCached[(int)CharacterSlot.MainHand] = enchantMainHand;
            _itemEnchantCached[(int)CharacterSlot.OffHand] = enchantOffHand;
            _itemEnchantCached[(int)CharacterSlot.Ranged] = enchantRanged;
            IsLoading = false;
            ActiveBuffs.AddRange(activeBuffs);
            CurrentModel = model;
            RecalculateSetBonusesFromCache();
        }

        public Character(string name, string realm, Character.CharacterRegion region, CharacterRace race, Item[] items, Enchant[] enchants, List<Buff> activeBuffs, bool trackEquippedItemChanges, string model)
        {
            _trackEquippedItemChanges = trackEquippedItemChanges;
            IsLoading = true;
            _name = name;
            _realm = realm;
            _region = region;
            _race = race;
            Array.Copy(items, _itemCached, items.Length);
            Array.Copy(enchants, _itemEnchantCached, enchants.Length);
            IsLoading = false;
            ActiveBuffs.AddRange(activeBuffs);
            CurrentModel = model;
            RecalculateSetBonusesFromCache();
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
                        this.RangedEnchant, false);
			foreach (Buff buff in this.ActiveBuffs) 
				if (!clone.ActiveBuffs.Contains(buff))
					clone.ActiveBuffs.Add(buff);
			clone.CalculationOptions = this.CalculationOptions;
            clone.Class = this.Class;
            clone.AssignAllTalentsFromCharacter(this);
			clone.EnforceMetagemRequirements = this.EnforceMetagemRequirements;
            clone.CurrentModel = this.CurrentModel;
			return clone;
		}
    
        public void Save(string path)
        {
			SerializeCalculationOptions();
            _activeBuffsXml = _activeBuffs.ConvertAll(buff => buff.Name);

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
                    character = LoadFromXml(System.IO.File.ReadAllText(path));
                }
                catch (Exception)
                {
                    MessageBox.Show("There was an error attempting to open this character.");
                    character = new Character();
                }
            }
            else
                character = new Character();

            return character;
        }

        public static Character LoadFromXml(string xml)
        {
            Character character;
			if (!string.IsNullOrEmpty(xml))
            {
				try
				{
					xml = xml.Replace("<Region>en", "<Region>US").Replace("<Weapon>", "<MainHand>").Replace("</Weapon>", "</MainHand>").Replace("<Idol>", "<Ranged>").Replace("</Idol>", "</Ranged>").Replace("<WeaponEnchant>", "<MainHandEnchant>").Replace("</WeaponEnchant>", "</MainHandEnchant>");

					if (xml.IndexOf("<CalculationOptions>") != xml.LastIndexOf("<CalculationOptions>"))
					{
						xml = xml.Substring(0, xml.IndexOf("<CalculationOptions>")) +
							xml.Substring(xml.LastIndexOf("</CalculationOptions>") + "</CalculationOptions>".Length);
					}

					System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(Character));
					System.IO.StringReader reader = new System.IO.StringReader(xml);
					character = (Character)serializer.Deserialize(reader);
                    character._activeBuffs = character._activeBuffsXml.ConvertAll(buff => Buff.GetBuffByName(buff));
                    character._activeBuffs.RemoveAll(buff => buff == null);
					reader.Close();
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
