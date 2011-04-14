using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;

namespace Rawr
{
    [GenerateSerializer]
    public class Character
    {
        #region Constants
        /// <summary>The number of actual slots where items could go</summary>
        public const int SlotCount = 21;
        /// <summary>
        /// The number of slots where items could go, but that we would
        /// actually optimize against
        /// </summary>
        public const int OptimizableSlotCount = 19;
        private static readonly List<int> zeroSuffixList = new List<int>(new int[] { 0 });
        private static ItemSlot[] _itemSlots;
        public static ItemSlot[] ItemSlots
        {
            get
            {
                if (_itemSlots == null)
                {
#if SILVERLIGHT
                    _itemSlots = EnumHelper.GetValues<ItemSlot>();
#else
                    _itemSlots = (ItemSlot[])Enum.GetValues(typeof(ItemSlot));
#endif
                }
                return _itemSlots;
            }
        }
        private static CharacterSlot[] _characterSlots;
        public static CharacterSlot[] CharacterSlots
        {
            get
            {
                if (_characterSlots == null)
                {
#if SILVERLIGHT
                    _characterSlots = EnumHelper.GetValues<CharacterSlot>();
#else
                    _characterSlots = (CharacterSlot[])Enum.GetValues(typeof(CharacterSlot));
#endif
                }
                return _characterSlots;
            }
        }
        public static CharacterSlot[] EquippableCharacterSlots = {
            CharacterSlot.Projectile,
            CharacterSlot.Head,
            CharacterSlot.Neck,
            CharacterSlot.Shoulders,
            CharacterSlot.Chest,
            CharacterSlot.Waist,
            CharacterSlot.Legs,
            CharacterSlot.Feet,
            CharacterSlot.Wrist,
            CharacterSlot.Hands,
            CharacterSlot.Finger1,
            CharacterSlot.Finger2,
            CharacterSlot.Trinket1,
            CharacterSlot.Trinket2,
            CharacterSlot.Back,
            CharacterSlot.MainHand,
            CharacterSlot.OffHand,
            CharacterSlot.Ranged,
            CharacterSlot.ProjectileBag,
            CharacterSlot.Tabard,
            CharacterSlot.Shirt,
        };
        #endregion

        #region Character Information: Basics (Name, Realm, Race, etc)
        #region Name
        /// <summary>Character Name: Bob</summary>
        [DefaultValue("")]
        public string Name { get { return _name; } set { _name = value; } }
        private string _name;
        #endregion
        #region Realm
        /// <summary>Realm: Stormrage, etc.</summary>
        [DefaultValue("")]
        public string Realm { get { return _realm; } set { _realm = value; } }
        private string _realm;
        #endregion
        #region Region
        /// <summary>Region: US, EU, KR, TW, or CN</summary>
        [DefaultValue(CharacterRegion.US)]
        public CharacterRegion Region { get { return _region; } set { _region = value; } }
        private CharacterRegion _region = CharacterRegion.US;
        /// <summary>This is a Helper variable for use with the Stats Pane UI</summary>
        [XmlIgnore]
        public int RegionIndex { get { return (int)Region; } set { Region = (CharacterRegion)value; } }
        #endregion
        #region Race
        /// <summary>Race, such as Night Elf</summary>
        [XmlElement("Race")][DefaultValue(CharacterRace.NightElf)]
        public CharacterRace _race = CharacterRace.NightElf;
        [XmlIgnore]
        public CharacterRace Race { get { return _race; }
            set {
                if (_race != value) {
                    _race = value;
                    SetFaction();
                    OnRaceChanged();
                    OnCalculationsInvalidated();
                }
            }
        }
        /// <summary>This is a Helper variable for use with the Stats Pane UI</summary>
        [XmlIgnore]
        public int RaceIndex { get { return (int)Race; } set { Race = (CharacterRace)value; } }
        /// <summary>An event to attach to, allows other reactions in the program to a Race change</summary>
        public static event EventHandler RaceChanged;
        protected static void OnRaceChanged() { if (RaceChanged != null) RaceChanged(null, EventArgs.Empty); }
        #endregion
        #region Faction (Automatically determined by Race)
        /// <summary>Faction: Alliance, Horde</summary>
        [XmlElement("Faction")][DefaultValue(CharacterFaction.Alliance)]
        private CharacterFaction _faction = CharacterFaction.Alliance;
        [XmlIgnore]
        public CharacterFaction Faction { get { return _faction; } }
        private void SetFaction()
        {
            if (_race == CharacterRace.Draenei || _race == CharacterRace.Dwarf || _race == CharacterRace.Gnome || _race == CharacterRace.Human || _race == CharacterRace.NightElf || _race == CharacterRace.Worgen)
                _faction = CharacterFaction.Alliance;
            else
            {
                _faction = CharacterFaction.Horde;
            }
        }
        #endregion
        #region Class
        /// <summary>Class: Druid, Warrior, etc.</summary>
        [DefaultValue(CharacterClass.Druid)]
        public CharacterClass Class { get { return _class; } set { _class = value; OnClassChanged(); } }
        private CharacterClass _class = CharacterClass.Druid;
        /// <summary>This is a Helper variable for use with the Stats Pane UI</summary>
        [XmlIgnore]
        public int ClassIndex { get { return (int)Class; } set { Class = (CharacterClass)value; } }
        public event EventHandler ClassChanged;
        public void OnClassChanged() { if (ClassChanged != null) { ClassChanged(this, EventArgs.Empty); } }
        #endregion
        #region Model
        /// <summary>The Current Model for this Character: DPSWarr vs ProtWarr, etc.</summary>
        [XmlElement("CurrentModel")] // this should not have a default value, otherwise loading a bear character while in cat will load it as cat
        public string _currentModel;
        [XmlIgnore]
        public string CurrentModel {
            get {
                if (string.IsNullOrEmpty(_currentModel)) {
                    foreach (KeyValuePair<string, Type> kvp in Calculations.Models) {
                        if (kvp.Value == Calculations.Instance.GetType()) {
                            _currentModel = kvp.Key;
                            Class = Calculations.ModelClasses[_currentModel];
                        }
                    }
                }
                return _currentModel;
            }
            set { _currentModel = value; }
        }
        #endregion
        #region Character Level (Always 85)
        /// <summary>The level of the Character. In Cataclysm, we support level 85.<br />
        /// Note that some models (Mage, Warlock) allow for alternate Character levels.
        /// They implement their own level set function rather than utilizing this variable.</summary>
        public int Level { get { return 85; } }
        #endregion
        #region Professions
        /// <summary>The Character's Primary Profession</summary>
        [DefaultValue(Profession.None)]
        public Profession PrimaryProfession { 
            get { return _primaryProfession; }
            set
            {
                if (_primaryProfession != value)
                {
                    _primaryProfession = value;
                    Calculations.UpdateProfessions(this);
                    ValidateActiveBuffs();
                }
            }
        }
        private Profession _primaryProfession = Profession.None;
        /// <summary>This is a Helper variable for use with the Stats Pane UI</summary>
        [XmlIgnore]
        /// <summary>The Character's Secondary Profession (NOT Cooking, First Aid, Archeology or Fishing)</summary>
        public int PriProfIndex
        {
            get { return Profs.ProfessionToIndex(PrimaryProfession); }
            set { PrimaryProfession = Profs.IndexToProfession(value); if (ProfessionChanged != null) { ProfessionChanged(this, new EventArgs()); } }
        }
        [DefaultValue(Profession.None)]
        public Profession SecondaryProfession
        {
            get { return _secondaryProfession; }
            set
            {
                if (_secondaryProfession != value)
                {
                    _secondaryProfession = value;
                    Calculations.UpdateProfessions(this);
                    ValidateActiveBuffs();
                }
            }
        }
        private Profession _secondaryProfession = Profession.None;
        /// <summary>This is a Helper variable for use with the Stats Pane UI</summary>
        [XmlIgnore]
        public int SecProfIndex
        {
            get { return Profs.ProfessionToIndex(SecondaryProfession); }
            set { SecondaryProfession = Profs.IndexToProfession(value); if (ProfessionChanged != null) { ProfessionChanged(this, new EventArgs()); } }
        }
        /// <summary>An event to attach to, allows other reactions in the program to a Professions' change</summary>
        public event EventHandler ProfessionChanged;
        /// <summary>
        /// Convenience function, checks both PrimaryProfession and SecondaryProfession for a match to provided profession check
        /// </summary>
        /// <param name="p">The Profession to match</param>
        /// <returns>True if one of the two professions matches, false if no match on either</returns>
        public bool HasProfession(Profession p)
        {
            if (PrimaryProfession == p) { return true; }
            if (SecondaryProfession == p) { return true; }
            return false;
        }
        /// <summary>
        /// Convenience function, checks both PrimaryProfession and SecondaryProfession for a match to provided professions check
        /// </summary>
        /// <param name="list">The Professions to match</param>
        /// <returns>True if one of the two professions matches anything in the list, false if no match on either</returns>
        public bool HasProfession(List<Profession> list)
        {
            foreach (Profession p in list)
            {
                if (HasProfession(p))
                    return true;
            }
            return false;
        }
        #endregion
        #endregion
        #region Character Information: Intermediates (Buffs, Talents, Armory Pets, BS Sockets)
        #region Buffs
        /// <summary>Buffs: This variable stores the actual active buffs list in their Buff Class form.</summary>
        [XmlIgnore]
        public List<Buff> _activeBuffs;
        /// <summary>Buffs: This variable stores the active buffs list in their string names form,
        /// this allows it to store to the character xml file correctly.</summary>
        [XmlElement("ActiveBuffs")]
        public List<string> _activeBuffsXml;
        [XmlIgnore]
        public List<Buff> ActiveBuffs { get { return _activeBuffs; } set { _activeBuffs = value; ValidateActiveBuffs(); } }
        public void ActiveBuffsAdd(Buff buff) { if (buff != null) { ActiveBuffs.Add(buff); } }
        public void ActiveBuffsAdd(string buffName) {
            Buff buff = Buff.GetBuffByName(buffName);
            if (buff != null && !ActiveBuffs.Contains(buff)) {
                ActiveBuffs.Add(buff);
            }
        }
        public bool ActiveBuffsContains(string buff) {
            if (_activeBuffs == null) { return false; }
            return _activeBuffs.FindIndex(x => x.Name == buff) >= 0;
        }
        public bool ActiveBuffsConflictingBuffContains(string conflictingBuff) {
            return _activeBuffs.FindIndex(x => x.ConflictingBuffs.Contains(conflictingBuff)) >= 0;
        }
        public bool ActiveBuffsConflictingBuffContains(string name, string conflictingBuff) {
            return _activeBuffs.FindIndex(x => (x.ConflictingBuffs.Contains(conflictingBuff) && x.Name != name)) >= 0;
        }
        /// <summary>
        /// This function forces any duplicate buffs off the current buff list
        /// and enforces buffs that should be in there due to race/profession
        /// </summary>
        public void ValidateActiveBuffs() {
            // First let's check for Duplicate Buffs and remove them
            Buff cur = null;
            for (int i = 0; i < ActiveBuffs.Count;/*no default iter*/)
            {
                cur = ActiveBuffs[i];
                if (cur == null) { ActiveBuffs.RemoveAt(i); continue; } // don't iterate
                int count = 0;
                foreach (Buff iter in ActiveBuffs) {
                    if (iter.Name == cur.Name) count++;
                }
                if (count > 1) { ActiveBuffs.RemoveAt(i); continue; } // remove this first one, we'll check the other one(s) again later, don't iterate
                // At this point, we didn't fail so we can move on to the next one
                i++;
            }

            // Second let's check for Conflicting Buffs and remove them
            cur = null;
            for (int i = 0; i < ActiveBuffs.Count;/*no default iter*/)
            {
                cur = ActiveBuffs[i];
                if (cur == null) { ActiveBuffs.RemoveAt(i); continue; } // don't iterate
                int count = 0;
                foreach (Buff iter in ActiveBuffs) {
                    if (iter.Name == cur.Name) { continue; } // its the same buff, we dont need to compare against it
                    foreach (string conf in iter.ConflictingBuffs)
                    {
                        if (!string.IsNullOrEmpty(conf) && cur.ConflictingBuffs.Contains(conf))
                        {
                            count++;
                        }
                    }
                }
                if (count > 0) { ActiveBuffs.RemoveAt(i); continue; } // remove this first one, we'll check the other one(s) again later, don't iterate
                // At this point, we didn't fail so we can move on to the next one
                i++;
            }

            // Finally, let's check Profession Buffs that should be automatically applied
            // Toughness buff from Mining
            if (HasProfession(Profession.Mining) && !ActiveBuffsContains("Toughness")) { ActiveBuffsAdd("Toughness"); }
            else if (!HasProfession(Profession.Mining) && ActiveBuffsContains("Toughness")) { ActiveBuffs.Remove(Buff.GetBuffByName("Toughness")); }
            // Master of Anatomy from Skinning
            if (HasProfession(Profession.Skinning) && !ActiveBuffsContains("Master of Anatomy")) { ActiveBuffsAdd("Master of Anatomy"); }
            else if (!HasProfession(Profession.Skinning) && ActiveBuffsContains("Master of Anatomy")) { ActiveBuffs.Remove(Buff.GetBuffByName("Master of Anatomy")); }

            // Force a recalc, this will also update the Buffs tab since it's designed to react to that
            OnCalculationsInvalidated();
        }
        #endregion
        #region ArmoryPets (for Hunters)
        /// <summary>Hunter Pets: This variable stores the actual armory pets list in their ArmoryPet Class form.</summary>
        [XmlIgnore]
        public List<ArmoryPet> ArmoryPets;
        /// <summary>Hunter Pets: This variable stores the armory pets list in their string infos form,
        /// this allows it to store to the character xml file correctly.</summary>
        [XmlElement("ArmoryPets")]
        public List<string> ArmoryPetsXml;
        #endregion
        #region Talents
        [DefaultValue("")]
        [XmlElement("WarriorTalents")]
        public string SerializableWarriorTalents { get { return (string.IsNullOrWhiteSpace(WarriorTalents.ToString().Replace("0", "").Replace(".", ""))) ? "" : WarriorTalents.ToString(); } set { WarriorTalents = new WarriorTalents(value); } }
        [DefaultValue("")]
        [XmlElement("PaladinTalents")]
        public string SerializablePaladinTalents { get { return (string.IsNullOrWhiteSpace(PaladinTalents.ToString().Replace("0", "").Replace(".", ""))) ? "" : PaladinTalents.ToString(); } set { PaladinTalents = new PaladinTalents(value); } }
        [DefaultValue("")]
        [XmlElement("HunterTalents")]
        public string SerializableHunterTalents { get { return (string.IsNullOrWhiteSpace(HunterTalents.ToString().Replace("0", "").Replace(".", ""))) ? "" : HunterTalents.ToString(); } set { HunterTalents = new HunterTalents(value); } }
        [DefaultValue("")]
        [XmlElement("RogueTalents")]
        public string SerializableRogueTalents { get { return (string.IsNullOrWhiteSpace(RogueTalents.ToString().Replace("0", "").Replace(".", ""))) ? "" : RogueTalents.ToString(); } set { RogueTalents = new RogueTalents(value); } }
        [DefaultValue("")]
        [XmlElement("PriestTalents")]
        public string SerializablePriestTalents { get { return (string.IsNullOrWhiteSpace(PriestTalents.ToString().Replace("0", "").Replace(".", ""))) ? "" : PriestTalents.ToString(); } set { PriestTalents = new PriestTalents(value); } }
        [DefaultValue("")]
        [XmlElement("ShamanTalents")]
        public string SerializableShamanTalents { get { return (string.IsNullOrWhiteSpace(ShamanTalents.ToString().Replace("0", "").Replace(".", ""))) ? "" : ShamanTalents.ToString(); } set { ShamanTalents = new ShamanTalents(value); } }
        [DefaultValue("")]
        [XmlElement("MageTalents")]
        public string SerializableMageTalents { get { return (string.IsNullOrWhiteSpace(MageTalents.ToString().Replace("0", "").Replace(".", ""))) ? "" : MageTalents.ToString(); } set { MageTalents = new MageTalents(value); } }
        [DefaultValue("")]
        [XmlElement("WarlockTalents")]
        public string SerializableWarlockTalents { get { return (string.IsNullOrWhiteSpace(WarlockTalents.ToString().Replace("0", "").Replace(".", ""))) ? "" : WarlockTalents.ToString(); } set { WarlockTalents = new WarlockTalents(value); } }
        [DefaultValue("")]
        [XmlElement("DruidTalents")]
        public string SerializableDruidTalents { get { return (string.IsNullOrWhiteSpace(DruidTalents.ToString().Replace("0", "").Replace(".", ""))) ? "" : DruidTalents.ToString(); } set { DruidTalents = new DruidTalents(value); } }
        [DefaultValue("")]
        [XmlElement("DeathKnightTalents")]
        public string SerializableDeathKnightTalents { get { return (string.IsNullOrWhiteSpace(DeathKnightTalents.ToString().Replace("0", "").Replace(".", ""))) ? "" : DeathKnightTalents.ToString(); } set { DeathKnightTalents = new DeathKnightTalents(value); } }

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
        public WarriorTalents WarriorTalents { get { return _warriorTalents ?? (_warriorTalents = new WarriorTalents()); } set { _warriorTalents = value; } }
        [XmlIgnore]
        public PaladinTalents PaladinTalents { get { return _paladinTalents ?? (_paladinTalents = new PaladinTalents()); } set { _paladinTalents = value; } }
        [XmlIgnore]
        public HunterTalents HunterTalents { get { return _hunterTalents ?? (_hunterTalents = new HunterTalents()); } set { _hunterTalents = value; } }
        [XmlIgnore]
        public RogueTalents RogueTalents { get { return _rogueTalents ?? (_rogueTalents = new RogueTalents()); } set { _rogueTalents = value; } }
        [XmlIgnore]
        public PriestTalents PriestTalents { get { return _priestTalents ?? (_priestTalents = new PriestTalents()); } set { _priestTalents = value; } }
        [XmlIgnore]
        public ShamanTalents ShamanTalents { get { return _shamanTalents ?? (_shamanTalents = new ShamanTalents()); } set { _shamanTalents = value; } }
        [XmlIgnore]
        public MageTalents MageTalents { get { return _mageTalents ?? (_mageTalents = new MageTalents()); } set { _mageTalents = value; } }
        [XmlIgnore]
        public WarlockTalents WarlockTalents { get { return _warlockTalents ?? (_warlockTalents = new WarlockTalents()); } set { _warlockTalents = value; } }
        [XmlIgnore]
        public DruidTalents DruidTalents { get { return _druidTalents ?? (_druidTalents = new DruidTalents()); } set { _druidTalents = value; } }
        [XmlIgnore]
        public DeathKnightTalents DeathKnightTalents { get { return _deathKnightTalents ?? (_deathKnightTalents = new DeathKnightTalents()); } set { _deathKnightTalents = value; } }

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

        public event EventHandler TalentChangedEvent;
        public void OnTalentChange()
        {
            if (TalentChangedEvent != null)
                TalentChangedEvent(this, EventArgs.Empty);
        }
        #endregion
        #region Blacksmithing Sockets
        [DefaultValue(false)]
        public bool WaistBlacksmithingSocketEnabled { get { return waistBSSocket; } set { waistBSSocket = value; OnCalculationsInvalidated(); } }
        private bool waistBSSocket = false;
        [DefaultValue(false)]
        public bool HandsBlacksmithingSocketEnabled { get { return handsBSSocket; } set { handsBSSocket = value; OnCalculationsInvalidated(); } }
        private bool handsBSSocket = false;
        [DefaultValue(false)]
        public bool WristBlacksmithingSocketEnabled { get { return wristBSSocket; } set { wristBSSocket = value; OnCalculationsInvalidated(); } }
        private bool wristBSSocket = false;
        #endregion
        #endregion
        #region Character Information: Equipped Item Slots
        #region As Gemmed ID's (How they are stored to the XML)
        /// <summary>
        /// A Function to convert the item from a specific slot into it's string form with gemming info.
        /// If the specified slot is empty this function will return an empty string.
        /// </summary>
        /// <param name="slot">The Slot to pull from.</param>
        /// <returns>ItemId.RandomSuffixId.Gem1.Gem2.Gem3.EnchantId.ReforgeId.TinkeringId
        /// <br />56278.0.52291.52219.0.4208.89.0</returns>
        private string GetGemmedId(CharacterSlot slot)
        {
            ItemInstance item = this[slot];
            if ((object)item == null) return null;
            return item.GemmedId;
        }
        /// <summary>
        /// A Function to take an item in it's Gemmed string form and set character's slot to that.
        /// </summary>
        /// <param name="slot">The Slot to set</param>
        /// <param name="gemmedId">The string gemmed id to put into the slot</param>
        private void SetGemmedId(CharacterSlot slot, string gemmedId)
        {
            if (string.IsNullOrEmpty(gemmedId)) _item[(int)slot] = null;
            else _item[(int)slot] = new ItemInstance(gemmedId); // don't call invalidations all the time while loading character
        }
        [XmlElement("Head")]
        public string _head { get { return GetGemmedId(CharacterSlot.Head); } set { SetGemmedId(CharacterSlot.Head, value); } }
        [XmlElement("Neck")]
        public string _neck { get { return GetGemmedId(CharacterSlot.Neck); } set { SetGemmedId(CharacterSlot.Neck, value); } }
        [XmlElement("Shoulders")]
        public string _shoulders { get { return GetGemmedId(CharacterSlot.Shoulders); } set { SetGemmedId(CharacterSlot.Shoulders, value); } }
        [XmlElement("Back")]
        public string _back { get { return GetGemmedId(CharacterSlot.Back); } set { SetGemmedId(CharacterSlot.Back, value); } }
        [XmlElement("Chest")]
        public string _chest { get { return GetGemmedId(CharacterSlot.Chest); } set { SetGemmedId(CharacterSlot.Chest, value); } }
        [XmlElement("Shirt")]
        public string _shirt { get { return GetGemmedId(CharacterSlot.Shirt); } set { SetGemmedId(CharacterSlot.Shirt, value); } }
        [XmlElement("Tabard")]
        public string _tabard { get { return GetGemmedId(CharacterSlot.Tabard); } set { SetGemmedId(CharacterSlot.Tabard, value); } }
        [XmlElement("Wrist")]
        public string _wrist { get { return GetGemmedId(CharacterSlot.Wrist); } set { SetGemmedId(CharacterSlot.Wrist, value); } }
        [XmlElement("Hands")]
        public string _hands { get { return GetGemmedId(CharacterSlot.Hands); } set { SetGemmedId(CharacterSlot.Hands, value); } }
        [XmlElement("Waist")]
        public string _waist { get { return GetGemmedId(CharacterSlot.Waist); } set { SetGemmedId(CharacterSlot.Waist, value); } }
        [XmlElement("Legs")]
        public string _legs { get { return GetGemmedId(CharacterSlot.Legs); } set { SetGemmedId(CharacterSlot.Legs, value); } }
        [XmlElement("Feet")]
        public string _feet { get { return GetGemmedId(CharacterSlot.Feet); } set { SetGemmedId(CharacterSlot.Feet, value); } }
        [XmlElement("Finger1")]
        public string _finger1 { get { return GetGemmedId(CharacterSlot.Finger1); } set { SetGemmedId(CharacterSlot.Finger1, value); } }
        [XmlElement("Finger2")]
        public string _finger2 { get { return GetGemmedId(CharacterSlot.Finger2); } set { SetGemmedId(CharacterSlot.Finger2, value); } }
        [XmlElement("Trinket1")]
        public string _trinket1 { get { return GetGemmedId(CharacterSlot.Trinket1); } set { SetGemmedId(CharacterSlot.Trinket1, value); } }
        [XmlElement("Trinket2")]
        public string _trinket2 { get { return GetGemmedId(CharacterSlot.Trinket2); } set { SetGemmedId(CharacterSlot.Trinket2, value); } }
        [XmlElement("MainHand")]
        public string _mainHand { get { return GetGemmedId(CharacterSlot.MainHand); } set { SetGemmedId(CharacterSlot.MainHand, value); } }
        [XmlElement("OffHand")]
        public string _offHand { get { return GetGemmedId(CharacterSlot.OffHand); } set { SetGemmedId(CharacterSlot.OffHand, value); } }
        [XmlElement("Ranged")]
        public string _ranged { get { return GetGemmedId(CharacterSlot.Ranged); } set { SetGemmedId(CharacterSlot.Ranged, value); } }
        [XmlElement("Projectile")]
        public string _projectile { get { return GetGemmedId(CharacterSlot.Projectile); } set { SetGemmedId(CharacterSlot.Projectile, value); } }
        [XmlElement("ProjectileBag")]
        public string _projectileBag { get { return GetGemmedId(CharacterSlot.ProjectileBag); } set { SetGemmedId(CharacterSlot.ProjectileBag, value); } }
        #endregion
        #endregion
        #region Character Information: Saved Item Sets Lists for Comparing Sets
        [XmlElement("ItemSetList")]
        public List<string> _itemSetListXML = null;
        [XmlIgnore]
        private ItemSetList itemSetList = new ItemSetList() { /*new ItemSet() { Name = "Naked", }*/ };
        public ItemSetList GetItemSetList() { return itemSetList; }
        /// <summary>
        /// Adds an ItemSet to the ItemSetList for this Character.
        /// ItemSets are for the Comparison chart.
        /// It will automatically check to see if the ItemSet is already in the list.
        /// </summary>
        /// <param name="newset"></param>
        public void AddToItemSetList(ItemSet newset)
        {
            if (itemSetList == null) return;
            bool didsomething = false;
            if (!ItemSetListContainsItemSetByName(newset.Name)) {
                itemSetList.Add(newset);
                didsomething |= true;
            } else {
                // Remove the original and replace it
                RemoveFromItemSetList(newset);
                itemSetList.Add(newset);
                didsomething |= true;
            }
            if (didsomething) { OnCalculationsInvalidated(); }
        }
        /// <summary>
        /// Remove an ItemSet to the ItemSetList for this Character.
        /// ItemSets are for the Comparison chart.
        /// It will run the Remove command until it stops finding the set in the ItemSetList, in case its there multiple times
        /// </summary>
        /// <param name="newset"></param>
        public void RemoveFromItemSetList(ItemSet newset)
        {
            if (itemSetList == null) return;
            // Using a While in case it's been added multiple times by accident
            bool didsomething = false;
            while (ItemSetListContainsItemSetByName(newset.Name))
            {
                itemSetList.RemoveAll(set => (set.Name == newset.Name));
                didsomething |= true;
            }
            if (didsomething) { OnCalculationsInvalidated(); }
        }
        public void RemoveFromItemSetListByName(String name) {
            if (itemSetList == null || itemSetList.Count <= 0) { return; }
            bool didsomething = false;
            for (int i = 0; i < itemSetList.Count; ) {
                if (itemSetList[i].Name == name) {
                    itemSetList.RemoveAt(i);
                    didsomething |= true;
                } else { i++; }
            }
            if (didsomething) { OnCalculationsInvalidated(); }
        }
        public void ClearItemSetList() {
            if (itemSetList == null) return;
            itemSetList.Clear();
            //itemSetList.Add(new ItemSet() { Name = "Naked", });
            OnCalculationsInvalidated();
        }
        public bool ItemSetListContainsItemSet(ItemSet IS)
        {
            if (itemSetList == null || itemSetList.Count <= 0) return false;
            bool contains = false;
            foreach (ItemSet ISs in itemSetList)
            {
                if (IS.Equals(ISs)) {
                    contains = true;
                    break;
                }
            }
            return contains;
            //return itemSetList.Contains(IS);
        }
        public bool ItemSetListContainsItemSetByName(String IS)
        {
            if (itemSetList == null || itemSetList.Count <= 0) return false;
            bool contains = false;
            foreach (ItemSet ISs in itemSetList)
            {
                if (ISs.Name.Equals(IS))
                {
                    contains = true;
                    break;
                }
            }
            return contains;
            //return itemSetList.Contains(IS);
        }
        public void EquipItemSetByName(String name)
        {
            if (itemSetList == null || itemSetList.Count <= 0) { return; }
            foreach (ItemSet IS in itemSetList) {
                if (name == IS.Name) {
                    foreach (CharacterSlot cs in Character.EquippableCharacterSlots) {
                        this[cs] = IS[cs];
                    }
                    break;
                }
            }
        }
        public int GetNumItemSetsFromOptimizer() {
            if (itemSetList == null || itemSetList.Count <= 0) { return 0; }
            int count = 0;
            foreach (ItemSet IS in itemSetList) {
                if (IS.Name.Contains("Optimized GearSet")) { count++; }
            }
            return count;
        }
        /// <summary>Warning! Retuns NULL when it can't find the set</summary>
        public ItemSet GetItemSetByName(String name) {
            if (name == "Current") {
                ItemSet current = new ItemSet();
                foreach (CharacterSlot cs in Character.EquippableCharacterSlots) {
                    current.Add(this[cs]);
                }
                current.Name = "Current";
                return current;
            }
            if (itemSetList == null || itemSetList.Count <= 0) { return null; }
            if (ItemSetListContainsItemSetByName(name)) {
                foreach (ItemSet ISs in itemSetList) {
                    if (ISs.Name.Equals(name)) { return ISs; }
                }
            }
            return null;
        }
        #endregion
        #region Character Information: Calculation Options Panes Settings (Stored as a serialized xml string rather than just XML)
        [XmlElement("CalculationOptions")]
        public SerializableDictionary<string, string> _serializedCalculationOptions;
        public void SerializeCalculationOptions()
        {
            if (CalculationOptions != null)
            {
                if (_serializedCalculationOptions == null)
                {
                    _serializedCalculationOptions = new SerializableDictionary<string, string>();
                }
                _serializedCalculationOptions[CurrentModel] = CalculationOptions.GetXml();
            }
        }
        private Dictionary<string, ICalculationOptionBase> _calculationOptions;
        [XmlIgnore]
        public ICalculationOptionBase CalculationOptions {
            get 
            {
                ICalculationOptionBase ret;
                if (_calculationOptions.TryGetValue(CurrentModel, out ret)) {
                    return ret;
                } else {
                    return LoadCalculationOptions();
                }
            }
            set { _calculationOptions[CurrentModel] = value; }
        }
        private ICalculationOptionBase LoadCalculationOptions()
        {
            if (_serializedCalculationOptions != null && _serializedCalculationOptions.ContainsKey(CurrentModel))
            {
                ICalculationOptionBase ret = Calculations.GetModel(CurrentModel)
                    .DeserializeDataObject(_serializedCalculationOptions[CurrentModel]);

                // set parent Character for models that need backward link
                ICharacterCalculationOptions characterCalculationOptions =
                    ret as ICharacterCalculationOptions;
                if (characterCalculationOptions != null)
                    characterCalculationOptions.Character = this;

                _calculationOptions[CurrentModel] = ret;
                return ret;
            }
            return null;
        }
        #endregion
        #region Character Information: Boss Options Pane Settings (Stored as XML)
        [XmlElement("Boss")]
        public BossOptions SerializableBoss {
            get { return BossOptions; }
            set { BossOptions = value.Clone(); }
        }
        [XmlIgnore]
        private BossOptions _bossOptions = null;
        [XmlIgnore]
        public BossOptions BossOptions
        {
            get {
                if (_bossOptions == null) {
                    _bossOptions = new BossOptions();
                    _bossOptions.Attacks.Add(BossOptions.ADefaultMeleeAttack);
                }
                return _bossOptions;
            }
            set { _bossOptions = value; }
        }
        #endregion
        #region Settings: Item Filters Enables/Disables
        #region Constants for Filters
        private struct RangeValue { public int Min; public int Max; }
        private struct PercRangeValue { public float Min; public float Max; }
        private readonly static RangeValue[] RangeValues = new RangeValue[] {
            new RangeValue { Min = 000, Max = 001 },
            new RangeValue { Min = 002, Max = 199 },
            new RangeValue { Min = 200, Max = 284 },
            new RangeValue { Min = 285, Max = 333 },
            new RangeValue { Min = 334, Max = 358 },
            new RangeValue { Min = 359, Max = 371 },
            new RangeValue { Min = 372, Max = 378 },
            new RangeValue { Min = 379, Max = 500 },
        };
        private readonly static PercRangeValue[] DropRangeValues = new PercRangeValue[] {
            new PercRangeValue { Min = 0.00f, Max = 0.01f },
            new PercRangeValue { Min = 0.01f, Max = 0.03f },
            new PercRangeValue { Min = 0.03f, Max = 0.05f },
            new PercRangeValue { Min = 0.05f, Max = 0.10f },
            new PercRangeValue { Min = 0.10f, Max = 0.15f },
            new PercRangeValue { Min = 0.15f, Max = 0.20f },
            new PercRangeValue { Min = 0.20f, Max = 0.25f },
            new PercRangeValue { Min = 0.25f, Max = 0.29f },
            new PercRangeValue { Min = 0.29f, Max = 0.39f },
            new PercRangeValue { Min = 0.39f, Max = 0.49f },
            new PercRangeValue { Min = 0.50f, Max = 1.001f },
        };
        #endregion

        /// <summary>This modified call allows it to speed up character loading</summary>
        private void OnFiltersChanged()
        {
            if (!IsLoading)
            {
                ItemCache.OnItemsChanged();
            }
        }

        #region ItemFilters by iLevel
        public bool ItemMatchesiLvlCheck(Item item)
        {
            if (item.Type == ItemType.None
                && (item.Slot == ItemSlot.Cogwheel || item.Slot == ItemSlot.Hydraulic || item.Slot == ItemSlot.Meta
                || item.Slot == ItemSlot.Purple || item.Slot == ItemSlot.Red || item.Slot == ItemSlot.Orange
                || item.Slot == ItemSlot.Yellow || item.Slot == ItemSlot.Green || item.Slot == ItemSlot.Blue))
            { return true; } // Don't filter gems
            //
            bool retVal = false;
            //
            if (iLvl_UseChecks) {
                // We only need 1 match to make it true
                for (int i = 0; i < _iLvl.Length; i++)
                {
                    if (iLvl[i] && (item.ItemLevel >= RangeValues[i].Min && item.ItemLevel <= RangeValues[i].Max))
                    {
                        retVal = true;
                        break;
                    }
                }
            } else {
                if (item.ItemLevel >= ilvlF_SLMin && item.ItemLevel <= ilvlF_SLMax) {
                    retVal = true;
                }
            }
            //
            return retVal;
        }

        [XmlIgnore]
        private bool _iLvl_UseChecks = true;
        [XmlElement("ItemFiltersSettings_UseChecks")][DefaultValue(true)]
        public bool iLvl_UseChecks { get { return _iLvl_UseChecks; } set { _iLvl_UseChecks = value; OnFiltersChanged(); } }
        [XmlIgnore]
        private bool[] _iLvl = new bool[] {
            true, // 0 000-001 (Heirloom)
            true, // 1 002-199 (Tier 01-06)
            true, // 2 200-284 (Tier 07-10)
            true, // 3 285-333 (Cata Dungeons)
            true, // 4 334-358 (Cata Heroics)
            true, // 5 359-371 (Tier 11.0)
            true, // 6 372-378 (Tier 11.5)
            true, // 7 379+    (Tier 12.0)
        };
        [XmlIgnore]
        public bool[] iLvl {
            get {
                if (_iLvl == null) {
                    _iLvl = new bool[] {
                        true, // 0 000-001 (Heirloom)
                        true, // 1 002-199 (Tier 01-06)
                        true, // 2 200-284 (Tier 07-10)
                        true, // 3 285-333 (Cata Dungeons)
                        true, // 4 334-358 (Cata Heroics)
                        true, // 5 359-371 (Tier 11.0)
                        true, // 6 372-378 (Tier 11.5)
                        true, // 7 379+    (Tier 12.0)
                    };
                }
                return _iLvl;
            }
            set {
                if (value == null) {
                    _iLvl = new bool[] {
                        true, // 0 000-001 (Heirloom)
                        true, // 1 002-199 (Tier 01-06)
                        true, // 2 200-284 (Tier 07-10)
                        true, // 3 285-333 (Cata Dungeons)
                        true, // 4 334-358 (Cata Heroics)
                        true, // 5 359-371 (Tier 11.0)
                        true, // 6 372-378 (Tier 11.5)
                        true, // 7 379+    (Tier 12.0)
                    };
                } else {
                    _iLvl = value;
                }
                OnFiltersChanged();
            }
        }
        [XmlElement("ItemFiltersSettings_0")][DefaultValue(true)]
        public bool ilvlF_0 { get { return _iLvl[0]; } set { _iLvl[0] = value; OnFiltersChanged(); } }
        [XmlElement("ItemFiltersSettings_1")][DefaultValue(true)]
        public bool ilvlF_1 { get { return _iLvl[1]; } set { _iLvl[1] = value; OnFiltersChanged(); } }
        [XmlElement("ItemFiltersSettings_2")][DefaultValue(true)]
        public bool ilvlF_2 { get { return _iLvl[2]; } set { _iLvl[2] = value; OnFiltersChanged(); } }
        [XmlElement("ItemFiltersSettings_3")][DefaultValue(true)]
        public bool ilvlF_3 { get { return _iLvl[3]; } set { _iLvl[3] = value; OnFiltersChanged(); } }
        [XmlElement("ItemFiltersSettings_4")][DefaultValue(true)]
        public bool ilvlF_4 { get { return _iLvl[4]; } set { _iLvl[4] = value; OnFiltersChanged(); } }
        [XmlElement("ItemFiltersSettings_5")][DefaultValue(true)]
        public bool ilvlF_5 { get { return _iLvl[5]; } set { _iLvl[5] = value; OnFiltersChanged(); } }
        [XmlElement("ItemFiltersSettings_6")][DefaultValue(true)]
        public bool ilvlF_6 { get { return _iLvl[6]; } set { _iLvl[6] = value; OnFiltersChanged(); } }
        [XmlElement("ItemFiltersSettings_7")][DefaultValue(true)]
        public bool ilvlF_7 { get { return _iLvl[7]; } set { _iLvl[7] = value; OnFiltersChanged(); } }

        [XmlIgnore]
        private double _ilvlF_SLMin = 285;
        [XmlElement("ItemFiltersSettings_SLMin")][DefaultValue(285)]
        public double ilvlF_SLMin { get { return _ilvlF_SLMin; } set { _ilvlF_SLMin = value; OnFiltersChanged(); } }
        [XmlIgnore]
        private double _ilvlF_SLMax = 377;
        [XmlElement("ItemFiltersSettings_SLMax")][DefaultValue(377)]
        public double ilvlF_SLMax { get { return _ilvlF_SLMax; } set { _ilvlF_SLMax = value; OnFiltersChanged(); } }
        #endregion

        #region ItemFilters by Drop Rate
        public bool ItemMatchesDropCheck(Item item)
        {
            if (item.Type == ItemType.None
                && (item.Slot == ItemSlot.Cogwheel || item.Slot == ItemSlot.Hydraulic || item.Slot == ItemSlot.Meta
                || item.Slot == ItemSlot.Purple || item.Slot == ItemSlot.Red || item.Slot == ItemSlot.Orange
                || item.Slot == ItemSlot.Yellow || item.Slot == ItemSlot.Green || item.Slot == ItemSlot.Blue))
            { return true; } // Don't filter gems
            //
            bool retVal = false;
            // First, check to see if any of the sources is based on Drop
            int index = -1;
            int type = 0;
            for (int i = 0; i < item.LocationInfo.Count; )
            {
                /*if (item.LocationInfo[i] == null) { item.LocationInfo.RemoveAt(i); }
                else*/ if (item.LocationInfo[i].GetType() == typeof(StaticDrop))    { index = i; type = 1; break; }
                //else if (item.LocationInfo[i].GetType() == typeof(WorldDrop))     { index = i; type = 2; break; }
                //else if (item.LocationInfo[i].GetType() == typeof(ContainerItem)) { index = i; type = 3; break; }
                else { i++; }
            }
            if (index == -1) { return true; } // ignoring the concept of drop filtering because it's not tied to a drop
            //
            float dropPerc = (type == 1 ? (item.LocationInfo[index] as StaticDrop).DropPerc/*
                            : type == 2 ? (item.LocationInfo[index] as WorldDrop).DropPerc
                            : type == 3 ? (item.LocationInfo[index] as ContainerItem).DropPerc*/
                            : 0f);
            if (float.IsNaN(dropPerc))
            {
                return true; // bad data, can happen, don't filter out
            }
            //
            if (Drop_UseChecks) {
                // We only need 1 match to make it true
                for (int i = 0; i < _Drop.Length; i++) {
                    if (Drop[i] && (dropPerc >= DropRangeValues[i].Min && dropPerc < DropRangeValues[i].Max)) {
                        retVal = true;
                        break;
                    }
                }
            } else {
                if (dropPerc >= DropF_SLMin && dropPerc <= DropF_SLMax) {
                    retVal = true;
                }
            }
            //
            return retVal;
        }

        [XmlIgnore]
        private bool _Drop_UseChecks = true;
        [XmlElement("ItemFiltersDropSettings_UseChecks")][DefaultValue(true)]
        public bool Drop_UseChecks { get { return _Drop_UseChecks; } set { _Drop_UseChecks = value; OnFiltersChanged(); } }
        [XmlIgnore]
        private bool[] _Drop = new bool[] {
            true, //  0   1%
            true, //  1   3%
            true, //  2   5%
            true, //  3  10%
            true, //  4  15%
            true, //  5  20%
            true, //  6  25%
            true, //  7  29%
            true, //  8  39%
            true, //  9  49%
            true, // 10 100%
        };
        [XmlIgnore]
        public bool[] Drop
        {
            get
            {
                if (_Drop == null)
                {
                    _Drop = new bool[] {
                        true, //  0   1%
                        true, //  1   3%
                        true, //  2   5%
                        true, //  3  10%
                        true, //  4  15%
                        true, //  5  20%
                        true, //  6  25%
                        true, //  7  29%
                        true, //  8  39%
                        true, //  9  49%
                        true, // 10 100%
                    };
                }
                return _Drop;
            }
            set
            {
                if (value == null)
                {
                    _Drop = new bool[] {
                        true, //  0   1%
                        true, //  1   3%
                        true, //  2   5%
                        true, //  3  10%
                        true, //  4  15%
                        true, //  5  20%
                        true, //  6  25%
                        true, //  7  29%
                        true, //  8  39%
                        true, //  9  49%
                        true, // 10 100%
                    };
                }
                else
                {
                    _Drop = value;
                }
                OnFiltersChanged();
            }
        }
        [XmlElement("ItemFiltersDropSettings_01")][DefaultValue(true)]
        public bool DropF_00 { get { return _Drop[0]; } set { _Drop[0] = value; OnFiltersChanged(); } }
        [XmlElement("ItemFiltersDropSettings_03")][DefaultValue(true)]
        public bool DropF_01 { get { return _Drop[1]; } set { _Drop[1] = value; OnFiltersChanged(); } }
        [XmlElement("ItemFiltersDropSettings_05")][DefaultValue(true)]
        public bool DropF_02 { get { return _Drop[2]; } set { _Drop[2] = value; OnFiltersChanged(); } }
        [XmlElement("ItemFiltersDropSettings_10")][DefaultValue(true)]
        public bool DropF_03 { get { return _Drop[3]; } set { _Drop[3] = value; OnFiltersChanged(); } }
        [XmlElement("ItemFiltersDropSettings_15")][DefaultValue(true)]
        public bool DropF_04 { get { return _Drop[4]; } set { _Drop[4] = value; OnFiltersChanged(); } }
        [XmlElement("ItemFiltersDropSettings_20")][DefaultValue(true)]
        public bool DropF_05 { get { return _Drop[5]; } set { _Drop[5] = value; OnFiltersChanged(); } }
        [XmlElement("ItemFiltersDropSettings_25")][DefaultValue(true)]
        public bool DropF_06 { get { return _Drop[6]; } set { _Drop[6] = value; OnFiltersChanged(); } }
        [XmlElement("ItemFiltersDropSettings_29")][DefaultValue(true)]
        public bool DropF_07 { get { return _Drop[7]; } set { _Drop[7] = value; OnFiltersChanged(); } }
        [XmlElement("ItemFiltersDropSettings_39")][DefaultValue(true)]
        public bool DropF_08 { get { return _Drop[8]; } set { _Drop[8] = value; OnFiltersChanged(); } }
        [XmlElement("ItemFiltersDropSettings_49")][DefaultValue(true)]
        public bool DropF_09 { get { return _Drop[9]; } set { _Drop[9] = value; OnFiltersChanged(); } }
        [XmlElement("ItemFiltersDropSettings_100")][DefaultValue(true)]
        public bool DropF_10 { get { return _Drop[10]; } set { _Drop[10] = value; OnFiltersChanged(); } }

        [XmlIgnore]
        private double _DropF_SLMin = 0.00f;
        [XmlElement("ItemFiltersDropSettings_SLMin")][DefaultValue(0.00f)]
        public double DropF_SLMin { get { return _DropF_SLMin; } set { _DropF_SLMin = value; OnFiltersChanged(); } }
        [XmlIgnore]
        private double _DropF_SLMax = 1.001f;
        [XmlElement("ItemFiltersDropSettings_SLMax")][DefaultValue(1.001f)]
        public double DropF_SLMax { get { return _DropF_SLMax; } set { _DropF_SLMax = value; OnFiltersChanged(); } }
        #endregion

        #region ItemFilters by Bind Type
        public bool ItemMatchesBindCheck(Item item)
        {
            if (item.Type == ItemType.None
                && (item.Slot == ItemSlot.Cogwheel || item.Slot == ItemSlot.Hydraulic || item.Slot == ItemSlot.Meta
                || item.Slot == ItemSlot.Purple || item.Slot == ItemSlot.Red || item.Slot == ItemSlot.Orange
                || item.Slot == ItemSlot.Yellow || item.Slot == ItemSlot.Green || item.Slot == ItemSlot.Blue))
            { return true; } // Don't filter gems
            //
            bool retVal = false;
            //
            // We only need 1 match to make it true
            if (bindF_0 && item.Bind == BindsOn.None) { retVal = true; }
            if (bindF_1 && item.Bind == BindsOn.BoA ) { retVal = true; }
            if (bindF_2 && item.Bind == BindsOn.BoU ) { retVal = true; }
            if (bindF_3 && item.Bind == BindsOn.BoE ) { retVal = true; }
            if (bindF_4 && item.Bind == BindsOn.BoP ) { retVal = true; }
            //
            return retVal;
        }

        [XmlIgnore]
        private bool[] _bind = new bool[] {
            true, // 0 Doesn't Bind
            true, // 1 Binds To Account
            true, // 2 Binds on Use
            true, // 3 Binds on Equip
            true, // 4 Binds on Pickup
        };
        [XmlIgnore]
        public bool[] bind {
            get {
                if (_bind == null) {
                    _bind = new bool[] {
                        true, // 0 Doesn't Bind
                        true, // 1 Binds To Account
                        true, // 2 Binds on Use
                        true, // 3 Binds on Equip
                        true, // 4 Binds on Pickup
                    };
                }
                return _bind;
            }
            set {
                if (value == null) {
                    _bind = new bool[] {
                        true, // 0 Doesn't Bind
                        true, // 1 Binds To Account
                        true, // 2 Binds on Use
                        true, // 3 Binds on Equip
                        true, // 4 Binds on Pickup
                    };
                } else { _bind = value; }
                OnFiltersChanged();
            }
        }
        [XmlElement("ItemFiltersBindSettings_0")][DefaultValue(true)]
        public bool bindF_0 { get { return _bind[0]; } set { _bind[0] = value; OnFiltersChanged(); } }
        [XmlElement("ItemFiltersBindSettings_1")][DefaultValue(true)]
        public bool bindF_1 { get { return _bind[1]; } set { _bind[1] = value; OnFiltersChanged(); } }
        [XmlElement("ItemFiltersBindSettings_2")][DefaultValue(true)]
        public bool bindF_2 { get { return _bind[2]; } set { _bind[2] = value; OnFiltersChanged(); } }
        [XmlElement("ItemFiltersBindSettings_3")][DefaultValue(true)]
        public bool bindF_3 { get { return _bind[3]; } set { _bind[3] = value; OnFiltersChanged(); } }
        [XmlElement("ItemFiltersBindSettings_4")][DefaultValue(true)]
        public bool bindF_4 { get { return _bind[4]; } set { _bind[4] = value; OnFiltersChanged(); } }
        #endregion

        #region ItemFilters by Profession
        public bool ItemMatchesProfCheck(Item item)
        {
            if (item.Type == ItemType.None
                && (item.Slot == ItemSlot.Cogwheel || item.Slot == ItemSlot.Hydraulic || item.Slot == ItemSlot.Meta
                || item.Slot == ItemSlot.Purple || item.Slot == ItemSlot.Red || item.Slot == ItemSlot.Orange
                || item.Slot == ItemSlot.Yellow || item.Slot == ItemSlot.Green || item.Slot == ItemSlot.Blue))
            { return true; } // Don't filter gems
            //
            bool retVal = false;
            // Check to see if its BoP, if it's not, we don't want this filter working against the item
            if (item.Bind != BindsOn.BoP) { return true; }
            // Check to see if it's a Shirt or Tabard, we don't care about filtering these out
            if (item.Slot == ItemSlot.Tabard || item.Slot == ItemSlot.Shirt) { return true; }
            // Check to see if any of the sources is based on Profession
            int index = -1;
            for(int i=0; i < item.LocationInfo.Count;)
            {
                /*if (item.LocationInfo[i] == null) { item.LocationInfo.RemoveAt(i); }
                else*/ if (item.LocationInfo[i].GetType() == typeof(CraftedItem)) { index = i; break; }
                else { i++; }
            }
            if (index == -1) { return true; } // ignoring the concept of profession filtering because it's not tied to a profession
            //
            if (prof_UseChar) {
                // We only need 1 match to make it true
                if (HasProfession(Profession.Alchemy) && item.LocationInfo[index].Description.Contains(Profession.Alchemy.ToString())) { retVal = true; }
                if (HasProfession(Profession.Blacksmithing) && item.LocationInfo[index].Description.Contains(Profession.Blacksmithing.ToString())) { retVal = true; }
                if (HasProfession(Profession.Enchanting) && item.LocationInfo[index].Description.Contains(Profession.Enchanting.ToString())) { retVal = true; }
                if (HasProfession(Profession.Engineering) && item.LocationInfo[index].Description.Contains(Profession.Engineering.ToString())) { retVal = true; }
                if (HasProfession(Profession.Herbalism) && item.LocationInfo[index].Description.Contains(Profession.Herbalism.ToString())) { retVal = true; }
                if (HasProfession(Profession.Inscription) && item.LocationInfo[index].Description.Contains(Profession.Inscription.ToString())) { retVal = true; }
                if (HasProfession(Profession.Jewelcrafting) && item.LocationInfo[index].Description.Contains(Profession.Jewelcrafting.ToString())) { retVal = true; }
                if (HasProfession(Profession.Leatherworking) && item.LocationInfo[index].Description.Contains(Profession.Leatherworking.ToString())) { retVal = true; }
                if (HasProfession(Profession.Mining) && item.LocationInfo[index].Description.Contains(Profession.Mining.ToString())) { retVal = true; }
                if (HasProfession(Profession.Skinning) && item.LocationInfo[index].Description.Contains(Profession.Skinning.ToString())) { retVal = true; }
                if (HasProfession(Profession.Tailoring) && item.LocationInfo[index].Description.Contains(Profession.Tailoring.ToString())) { retVal = true; }
            } else {
                // We only need 1 match to make it true
                if (profF_00 && item.LocationInfo[index].Description.Contains(Profession.Alchemy.ToString())) { retVal = true; }
                if (profF_01 && item.LocationInfo[index].Description.Contains(Profession.Blacksmithing.ToString())) { retVal = true; }
                if (profF_02 && item.LocationInfo[index].Description.Contains(Profession.Enchanting.ToString())) { retVal = true; }
                if (profF_03 && item.LocationInfo[index].Description.Contains(Profession.Engineering.ToString())) { retVal = true; }
                if (profF_04 && item.LocationInfo[index].Description.Contains(Profession.Herbalism.ToString())) { retVal = true; }
                if (profF_05 && item.LocationInfo[index].Description.Contains(Profession.Inscription.ToString())) { retVal = true; }
                if (profF_06 && item.LocationInfo[index].Description.Contains(Profession.Jewelcrafting.ToString())) { retVal = true; }
                if (profF_07 && item.LocationInfo[index].Description.Contains(Profession.Leatherworking.ToString())) { retVal = true; }
                if (profF_08 && item.LocationInfo[index].Description.Contains(Profession.Mining.ToString())) { retVal = true; }
                if (profF_09 && item.LocationInfo[index].Description.Contains(Profession.Skinning.ToString())) { retVal = true; }
                if (profF_10 && item.LocationInfo[index].Description.Contains(Profession.Tailoring.ToString())) { retVal = true; }
            }
            //
            return retVal;
        }

        [XmlIgnore]
        private bool _prof_UseChar = true;
        [XmlElement("ItemFiltersProfSettings_UseChar")][DefaultValue(true)]
        public bool prof_UseChar { get { return _prof_UseChar; } set { _prof_UseChar = value; OnFiltersChanged(); } }
        [XmlIgnore]
        private bool[] _prof = new bool[] {
            true,
            true,
            true,
            true,
            true,
            true,
            true,
            true,
            true,
            true,
            true,
        };
        [XmlIgnore]
        public bool[] prof
        {
            get
            {
                if (_prof == null)
                {
                    _prof = new bool[] {
                        true,
                        true,
                        true,
                        true,
                        true,
                        true,
                        true,
                        true,
                        true,
                        true,
                        true,
                    };
                }
                return _prof;
            }
            set
            {
                if (value == null)
                {
                    _prof = new bool[] {
                        true,
                        true,
                        true,
                        true,
                        true,
                        true,
                        true,
                        true,
                        true,
                        true,
                        true,
                    };
                }
                else
                {
                    _prof = value;
                }
                OnFiltersChanged();
            }
        }
        [XmlElement("ItemFiltersProfSettings_00")][DefaultValue(true)]
        public bool profF_00 { get { return _prof[0]; } set { _prof[0] = value; OnFiltersChanged(); } }
        [XmlElement("ItemFiltersProfSettings_01")][DefaultValue(true)]
        public bool profF_01 { get { return _prof[1]; } set { _prof[1] = value; OnFiltersChanged(); } }
        [XmlElement("ItemFiltersProfSettings_02")][DefaultValue(true)]
        public bool profF_02 { get { return _prof[2]; } set { _prof[2] = value; OnFiltersChanged(); } }
        [XmlElement("ItemFiltersProfSettings_03")][DefaultValue(true)]
        public bool profF_03 { get { return _prof[3]; } set { _prof[3] = value; OnFiltersChanged(); } }
        [XmlElement("ItemFiltersProfSettings_04")][DefaultValue(true)]
        public bool profF_04 { get { return _prof[4]; } set { _prof[4] = value; OnFiltersChanged(); } }
        [XmlElement("ItemFiltersProfSettings_05")][DefaultValue(true)]
        public bool profF_05 { get { return _prof[5]; } set { _prof[5] = value; OnFiltersChanged(); } }
        [XmlElement("ItemFiltersProfSettings_06")][DefaultValue(true)]
        public bool profF_06 { get { return _prof[6]; } set { _prof[6] = value; OnFiltersChanged(); } }
        [XmlElement("ItemFiltersProfSettings_07")][DefaultValue(true)]
        public bool profF_07 { get { return _prof[7]; } set { _prof[7] = value; OnFiltersChanged(); } }
        [XmlElement("ItemFiltersProfSettings_08")][DefaultValue(true)]
        public bool profF_08 { get { return _prof[8]; } set { _prof[8] = value; OnFiltersChanged(); } }
        [XmlElement("ItemFiltersProfSettings_09")][DefaultValue(true)]
        public bool profF_09 { get { return _prof[9]; } set { _prof[9] = value; OnFiltersChanged(); } }
        [XmlElement("ItemFiltersProfSettings_10")][DefaultValue(true)]
        public bool profF_10 { get { return _prof[10]; } set { _prof[10] = value; OnFiltersChanged(); } }
        #endregion

        #region ItemFilters by Source
        public List<ItemFilterEnabledOverride> ItemFilterEnabledOverride { get; set; }

        private void SaveItemFilterEnabledOverride()
        {
            ItemFilterEnabledOverride = new List<ItemFilterEnabledOverride>();
            foreach (var itemFilter in ItemFilter.FilterList)
            {
                SaveItemFilterEnabledOverride(itemFilter, ItemFilterEnabledOverride);
            }
            ItemFilterEnabledOverride.Add(new ItemFilterEnabledOverride() { Name = "Other", Enabled = ItemFilter.OtherEnabled });
        }

        private void SaveItemFilterEnabledOverride(ItemFilterRegex itemFilter, List<ItemFilterEnabledOverride> list)
        {
            ItemFilterEnabledOverride filterOverride = new ItemFilterEnabledOverride();
            filterOverride.Name = itemFilter.Name;
            filterOverride.Enabled = itemFilter.Enabled;
            if (itemFilter.RegexList.Count > 0)
            {
                filterOverride.SubFilterOverride = new List<ItemFilterEnabledOverride>();
                foreach (var subFilter in itemFilter.RegexList)
                {
                    SaveItemFilterEnabledOverride(subFilter, filterOverride.SubFilterOverride);
                }
                filterOverride.SubFilterOverride.Add(new ItemFilterEnabledOverride() { Name = "Other", Enabled = itemFilter.OtherRegexEnabled });                
            }
            list.Add(filterOverride);
        }

        public bool LoadItemFilterEnabledOverride()
        {
            bool triggerEvent = false;
            if (ItemFilterEnabledOverride == null || ItemFilterEnabledOverride.Count == 0) return false;
            ItemFilter.IsLoading = true;
            foreach (var filterOverride in ItemFilterEnabledOverride)
            {
                if (filterOverride.Name != "Other")
                {
                    LoadItemFilterEnabledOverride(filterOverride, ItemFilter.FilterList, ref triggerEvent);
                }
                else
                {
                    if (ItemFilter.OtherEnabled != filterOverride.Enabled)
                    {
                        ItemFilter.OtherEnabled = (bool)filterOverride.Enabled;
                        triggerEvent = true;
                    }
                }
            }
            ItemFilter.IsLoading = false;
            return triggerEvent;
        }

        private void LoadItemFilterEnabledOverride(ItemFilterEnabledOverride filterOverride, ItemFilterRegexList list, ref bool triggerEvent)
        {
            foreach (ItemFilterRegex itemFilter in list)
            {
                if (itemFilter.Name == filterOverride.Name)
                {
                    if (itemFilter.Enabled != filterOverride.Enabled)
                    {
                        itemFilter.Enabled = filterOverride.Enabled;
                        triggerEvent = true;
                    }
                    if (filterOverride.SubFilterOverride != null && filterOverride.SubFilterOverride.Count > 0)
                    {
                        foreach (var subOverride in filterOverride.SubFilterOverride)
                        {
                            if (subOverride.Name != "Other")
                            {
                                LoadItemFilterEnabledOverride(subOverride, itemFilter.RegexList, ref triggerEvent);
                            }
                            else
                            {
                                if (itemFilter.OtherRegexEnabled != subOverride.Enabled)
                                {
                                    itemFilter.OtherRegexEnabled = (bool)subOverride.Enabled;
                                    triggerEvent = true;
                                }
                            }
                        }
                    }
                    return;
                }
            }
        }
        #endregion
        #endregion
        #region Settings: Custom Gemming Templates
        public List<GemmingTemplate> CustomGemmingTemplates { get; set; }
        public List<GemmingTemplate> GemmingTemplateOverrides { get; set; }

        private string gemmingTemplateModel;
        private List<GemmingTemplate> currentGemmingTemplates;


        [XmlIgnore]
        public List<GemmingTemplate> CurrentGemmingTemplates
        {
            get
            {
                if (currentGemmingTemplates == null || CurrentModel != gemmingTemplateModel)
                {
                    SaveGemmingTemplateOverrides();
                    GenerateGemmingTemplates();
                }
                return currentGemmingTemplates;
            }
        }

        private void SaveGemmingTemplateOverrides()
        {
            if (currentGemmingTemplates == null) return;
            List<GemmingTemplate> defaults = GemmingTemplate.AllTemplates[gemmingTemplateModel];
            GemmingTemplateOverrides.RemoveAll(template => template.Model == gemmingTemplateModel);
            foreach (GemmingTemplate template in defaults)
            {
                foreach (GemmingTemplate overrideTemplate in currentGemmingTemplates)
                {
                    if (template.Group == overrideTemplate.Group && template.BlueId == overrideTemplate.BlueId && template.MetaId == overrideTemplate.MetaId && template.Model == overrideTemplate.Model && template.PrismaticId == overrideTemplate.PrismaticId && template.RedId == overrideTemplate.RedId && template.YellowId == overrideTemplate.YellowId)
                    {
                        if (template.Enabled != overrideTemplate.Enabled)
                        {
                            GemmingTemplateOverrides.Add(overrideTemplate);
                            break;
                        }
                    }
                }
            }
        }

        private void GenerateGemmingTemplates()
        {
            List<GemmingTemplate> defaults = GemmingTemplate.CurrentTemplates;
            currentGemmingTemplates = new List<GemmingTemplate>();
            foreach (GemmingTemplate template in defaults)
            {
                GemmingTemplate toCopy = template;
                foreach (GemmingTemplate overrideTemplate in GemmingTemplateOverrides)
                {
                    if (template.Group == overrideTemplate.Group && template.BlueId == overrideTemplate.BlueId && template.MetaId == overrideTemplate.MetaId && template.Model == overrideTemplate.Model && template.PrismaticId == overrideTemplate.PrismaticId && template.RedId == overrideTemplate.RedId && template.YellowId == overrideTemplate.YellowId)
                    {
                        toCopy = overrideTemplate;
                        break;
                    }
                }
                currentGemmingTemplates.Add(new GemmingTemplate()
                {
                    BlueId = toCopy.BlueId,
                    Enabled = toCopy.Enabled,
                    Group = toCopy.Group,
                    MetaId = toCopy.MetaId,
                    Model = toCopy.Model,
                    PrismaticId = toCopy.PrismaticId,
                    RedId = toCopy.RedId,
                    YellowId = toCopy.YellowId,
                    CogwheelId = toCopy.CogwheelId,
                    Cogwheel2Id = toCopy.Cogwheel2Id,
                    HydraulicId = toCopy.HydraulicId,
                });
            }
            gemmingTemplateModel = CurrentModel;
        }
        #endregion
        #region Settings: Optimizer
        #region Available Items, Green/Blue Diamonds
        /// <summary>The list of items marked as avaiable to the optimizer (Green/Blue Diamonds)</summary>
        [XmlElement("AvailableItems")]
        public List<string> _availableItems;
        /// <summary>list of 5-tuples itemid.gem1id.gem2id.gem3id.enchantid,
        /// itemid is required, others can use * for wildcard
        /// for backward compatibility use just itemid instead of itemid.*.*.*.*
        /// -id represents enchants
        /// </summary>
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
        #region Getting/Setting Item Availability
        public ItemAvailability GetItemAvailability(Item item)
        {
            return GetItemAvailability(item.Id.ToString(), item.Id.ToString() + ".0", item.Id.ToString() + ".0.0.0.0.0.0.0");
        }
        public ItemAvailability GetItemAvailability(ItemInstance itemInstance)
        {
            return GetItemAvailability(itemInstance.Id.ToString(), string.Format("{0}.{1}", itemInstance.Id, itemInstance.RandomSuffixId), itemInstance.GemmedId);
        }
        public ItemAvailability GetItemAvailability(Enchant enchant)
        {
            string id = (-1 * (enchant.Id + ((int)AvailableItemIDModifiers.Enchants * (int)enchant.Slot))).ToString();
            return GetItemAvailability(id, string.Format("{0}.0", enchant.Id), id);
        }
        public ItemAvailability GetItemAvailability(Tinkering tinkering)
        {
            string id = (-1 * (tinkering.Id + ((int)AvailableItemIDModifiers.Tinkerings * (int)tinkering.Slot))).ToString();
            return GetItemAvailability(id, string.Format("{0}.0", tinkering.Id), id);
        }
        private ItemAvailability GetItemAvailability(string id, string suffixId, string fullId)
        {
            List<string> list = _availableItems.FindAll(x => x.StartsWith(id, StringComparison.Ordinal));
            if (list.Contains(fullId))
            {
                return ItemAvailability.Available;
            }
            if (list.Contains(id) || list.Contains(suffixId))
            {
                return ItemAvailability.RegemmingAllowed;
            }
            else
            {
                return ItemAvailability.NotAvailable;
            }
        }
        public void ToggleItemAvailability(int itemId, bool regemmingAllowed)
        {
            string id = itemId.ToString();
            string anyGem = id + ".*.*.*";

            if (id.StartsWith("-", StringComparison.Ordinal) || regemmingAllowed)
            {
                // all enabled toggle
                if (_availableItems.Contains(id) || _availableItems.FindIndex(x => x.StartsWith(anyGem, StringComparison.Ordinal)) >= 0)
                {
                    _availableItems.Remove(id);
                    _availableItems.RemoveAll(x => x.StartsWith(anyGem, StringComparison.Ordinal));
                }
                else
                {
                    _availableItems.Add(id);
                }
            }
            OnAvailableItemsChanged();
        }
        public void ToggleItemAvailability(Item item, bool regemmingAllowed)
        {
            string id = item.Id.ToString();
            string anyGem = id + ".*.*.*";

            if (id.StartsWith("-", StringComparison.Ordinal) || regemmingAllowed || item.IsGem)
            {
                // all enabled toggle
                if (_availableItems.Contains(id) || _availableItems.FindIndex(x => x.StartsWith(anyGem, StringComparison.Ordinal)) >= 0)
                {
                    _availableItems.Remove(id);
                    _availableItems.RemoveAll(x => x.StartsWith(anyGem, StringComparison.Ordinal));
                }
                else
                {
                    _availableItems.Add(id);
                }
            }
            OnAvailableItemsChanged();
        }
        public void ToggleItemAvailability(ItemInstance item, bool regemmingAllowed)
        {
            string id = item.Id.ToString();
            if (item.RandomSuffixId != 0)
            {
                id += "." + item.RandomSuffixId;
            }

            if (id.StartsWith("-", StringComparison.Ordinal) || regemmingAllowed)
            {
                // all enabled toggle
                if (_availableItems.Contains(id))
                {
                    _availableItems.Remove(id);
                }
                else
                {
                    _availableItems.Add(id);
                }
            }
            else
            {
                Predicate<string> p = (x =>
                {
                    return x == item.GemmedId;
                });
                // enabled toggle
                if (_availableItems.FindIndex(p) >= 0)
                {
                    _availableItems.RemoveAll(p);
                }
                else
                {
                    _availableItems.Add(item.GemmedId);
                }
            }
            OnAvailableItemsChanged();
        }
        public void ToggleItemAvailability(Enchant enchant)
        {
            string id = (-1 * (enchant.Id + ((int)AvailableItemIDModifiers.Enchants * (int)enchant.Slot))).ToString();
            // all enabled toggle
            if (_availableItems.Contains(id)) {
                while (_availableItems.Contains(id)) { _availableItems.Remove(id); }
            } else {
                _availableItems.Add(id);
            }
            OnAvailableItemsChanged();
        }
        public void ToggleItemAvailability(Tinkering tinkering)
        {
            string id = (-1 * (tinkering.Id + ((int)AvailableItemIDModifiers.Tinkerings * (int)tinkering.Slot))).ToString();
            // all enabled toggle
            if (_availableItems.Contains(id)) {
                while (_availableItems.Contains(id)) { _availableItems.Remove(id); }
            } else {
                _availableItems.Add(id);
            }
            OnAvailableItemsChanged();
        }
        // deprecated
        public void ToggleAvailableItemEnchantRestriction(ItemInstance item, Enchant enchant)
        {
            string id = item.Id.ToString();
            string anyGem = id + ".*.*.*";
            string gemId = string.Format("{0}.{1}.{2}.{3}", item.Id, item.Gem1Id, item.Gem2Id, item.Gem3Id);
            ItemAvailability availability = GetItemAvailability(item);
            switch (availability)
            {
                case ItemAvailability.Available:
                    if (enchant != null)
                    {
                        _availableItems.RemoveAll(x => x.StartsWith(gemId, StringComparison.Ordinal));
                        _availableItems.Add(gemId + "." + enchant.Id.ToString());
                    }
                    else
                    {
                        // any => all
                        _availableItems.RemoveAll(x => x.StartsWith(gemId, StringComparison.Ordinal));
                        foreach (Enchant e in Enchant.FindEnchants(item.Slot, this))
                        {
                            _availableItems.Add(gemId + "." + e.Id.ToString());
                        }
                    }
                    break;
                case ItemAvailability.AvailableWithEnchantRestrictions:
                    if (enchant != null)
                    {
                        if (_availableItems.Contains(gemId + "." + enchant.Id.ToString()))
                        {
                            _availableItems.Remove(gemId + "." + enchant.Id.ToString());
                        }
                        else
                        {
                            _availableItems.Add(gemId + "." + enchant.Id.ToString());
                        }
                    }
                    else
                    {
                        _availableItems.RemoveAll(x => x.StartsWith(gemId, StringComparison.Ordinal));
                        _availableItems.Add(gemId + ".*");
                    }
                    break;
                case ItemAvailability.RegemmingAllowed:
                    if (enchant != null)
                    {
                        _availableItems.RemoveAll(x => x.StartsWith(id, StringComparison.Ordinal));
                        _availableItems.Add(anyGem + "." + enchant.Id.ToString());
                    }
                    else
                    {
                        // any => all
                        _availableItems.RemoveAll(x => x.StartsWith(id, StringComparison.Ordinal));
                        foreach (Enchant e in Enchant.FindEnchants(item.Slot, this))
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
                        _availableItems.RemoveAll(x => x.StartsWith(id, StringComparison.Ordinal));
                        _availableItems.Add(id);
                    }
                    break;
                case ItemAvailability.NotAvailable:
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
        /*public void ToggleAvailableItemTinkeringRestriction(ItemInstance item, Tinkering tinkering)
        {
            string id = item.Id.ToString();
            string anyGem = id + ".*.*.*";
            string gemId = string.Format("{0}.{1}.{2}.{3}", item.Id, item.Gem1Id, item.Gem2Id, item.Gem3Id);
            ItemAvailability availability = GetItemAvailability(item);
            switch (availability)
            {
                case ItemAvailability.Available:
                    if (tinkering != null)
                    {
                        _availableItems.RemoveAll(x => x.StartsWith(gemId, StringComparison.Ordinal));
                        _availableItems.Add(gemId + "." + tinkering.Id.ToString());
                    }
                    else
                    {
                        // any => all
                        _availableItems.RemoveAll(x => x.StartsWith(gemId, StringComparison.Ordinal));
                        foreach (Tinkering e in Tinkering.FindTinkerings(item.Slot, this))
                        {
                            _availableItems.Add(gemId + "." + e.Id.ToString());
                        }
                    }
                    break;
                case ItemAvailability.AvailableWithTinkeringRestrictions:
                    if (tinkering != null)
                    {
                        if (_availableItems.Contains(gemId + "." + tinkering.Id.ToString()))
                        {
                            _availableItems.Remove(gemId + "." + tinkering.Id.ToString());
                        }
                        else
                        {
                            _availableItems.Add(gemId + "." + tinkering.Id.ToString());
                        }
                    }
                    else
                    {
                        _availableItems.RemoveAll(x => x.StartsWith(gemId, StringComparison.Ordinal));
                        _availableItems.Add(gemId + ".*");
                    }
                    break;
                case ItemAvailability.RegemmingAllowed:
                    if (tinkering != null)
                    {
                        _availableItems.RemoveAll(x => x.StartsWith(id, StringComparison.Ordinal));
                        _availableItems.Add(anyGem + "." + tinkering.Id.ToString());
                    }
                    else
                    {
                        // any => all
                        _availableItems.RemoveAll(x => x.StartsWith(id, StringComparison.Ordinal));
                        foreach (Tinkering e in Tinkering.FindTinkerings(item.Slot, this))
                        {
                            _availableItems.Add(anyGem + "." + e.Id.ToString());
                        }
                    }
                    break;
                case ItemAvailability.RegemmingAllowedWithTinkeringRestrictions:
                    if (tinkering != null)
                    {
                        if (_availableItems.Contains(anyGem + "." + tinkering.Id.ToString()))
                        {
                            _availableItems.Remove(anyGem + "." + tinkering.Id.ToString());
                        }
                        else
                        {
                            _availableItems.Add(anyGem + "." + tinkering.Id.ToString());
                        }
                    }
                    else
                    {
                        _availableItems.RemoveAll(x => x.StartsWith(id, StringComparison.Ordinal));
                        _availableItems.Add(id);
                    }
                    break;
                case ItemAvailability.NotAvailable:
                    if (tinkering != null)
                    {
                        _availableItems.Add(anyGem + "." + tinkering.Id.ToString());
                    }
                    else
                    {
                        _availableItems.Add(id);
                    }
                    break;
            }
            OnAvailableItemsChanged();
        }*/
        #endregion
        public event EventHandler AvailableItemsChanged;
        public void OnAvailableItemsChanged()
        {
            if (AvailableItemsChanged != null)
                AvailableItemsChanged(this, EventArgs.Empty);
        }
        #endregion
        [XmlIgnore]
        private List<ItemInstance> _customItemInstances;
        public List<ItemInstance> CustomItemInstances
        {
            get { return _customItemInstances; }
            set {
                _customItemInstances = value;
                InvalidateItemInstances();
            }
        }
        /// <summary>In the optimizer, you can choose to optimize a subpoint rather than Overall</summary>
        public string CalculationToOptimize { get; set; }
        /// <summary>Any requirements you have set on the optimizer</summary>
        public List<OptimizationRequirement> OptimizationRequirements { get; set; }
        #endregion

        #region Functions: Get/Set All Items on the character, primarily used by the Optimizer
        public ItemInstance[] GetItems()
        {
            return (ItemInstance[])_item.Clone();
        }
        public void SetItems(ItemInstance[] items) { SetItems(items, true); }
        public void SetItems(ItemInstance[] items, bool invalidate)
        {
            int max = Math.Min(OptimizableSlotCount, items.Length);
            for (int slot = 0; slot < max; slot++)
            {
                _item[slot] = items[slot] == null ? null : items[slot].Clone();
            }
            // when called from optimizer we never want to invalidate since that causes creation of new item instances
            // and causes us to lose stats cache
            if (invalidate) { OnCalculationsInvalidated(); }
        }
        public void SetItems(Character character) { SetItems(character, false, true); }
        public void SetItems(Character character, bool allSlots, bool invalidate)
        {
            int max = allSlots ? SlotCount : OptimizableSlotCount;
            for (int slot = 0; slot < max; slot++)
            {
                _item[slot] = character._item[slot] == null ? null : character._item[slot].Clone();
            }
            // when called from optimizer we never want to invalidate since that causes creation of new item instances
            // and causes us to lose stats cache
            if (invalidate) { OnCalculationsInvalidated(); }
        }
        #endregion

        #region Static Functions, used for Validations or Conversions
        public static bool ValidateArmorSpecialization(Character source, ItemType i)
        {
            // Null Check
            if (source == null) { return false; }
            // Item Type Fails
            if (source.Head == null || source.Head.Type != i) { return false; }
            if (source.Shoulders == null || source.Shoulders.Type != i) { return false; }
            if (source.Chest == null || source.Chest.Type != i) { return false; }
            if (source.Wrist == null || source.Wrist.Type != i) { return false; }
            if (source.Hands == null || source.Hands.Type != i) { return false; }
            if (source.Waist == null || source.Waist.Type != i) { return false; }
            if (source.Legs == null || source.Legs.Type != i) { return false; }
            if (source.Feet == null || source.Feet.Type != i) { return false; }
            // If it hasn't failed by now, it must be good
            return true;
        }
        public static CharacterSlot GetCharacterSlotByItemSlot(ItemSlot slot)
        {
            
            //note: When converting ItemSlot.Finger and ItemSlot.Trinket, this will ALWAYS
            //place them in Slot 1 of the 2 possibilities. Items listed as OneHand or TwoHand 
            //in their Itemslot profile, will be parsed into the MainHand CharacterSlot.
            
            switch (slot)
            {
               
                case Rawr.ItemSlot.Projectile: return CharacterSlot.Projectile;
                case Rawr.ItemSlot.Head: return CharacterSlot.Head;
                case Rawr.ItemSlot.Neck: return CharacterSlot.Neck;
                case Rawr.ItemSlot.Shoulders: return CharacterSlot.Shoulders;
                case Rawr.ItemSlot.Chest: return CharacterSlot.Chest;
                case Rawr.ItemSlot.Waist: return CharacterSlot.Waist;
                case Rawr.ItemSlot.Legs: return CharacterSlot.Legs;
                case Rawr.ItemSlot.Feet: return CharacterSlot.Feet;
                case Rawr.ItemSlot.Wrist: return CharacterSlot.Wrist;
                case Rawr.ItemSlot.Hands: return CharacterSlot.Hands;
                case Rawr.ItemSlot.Finger: return CharacterSlot.Finger1;
                //case Rawr.ItemSlot.Finger: return CharacterSlot.Finger2;
                case Rawr.ItemSlot.Trinket: return CharacterSlot.Trinket1;
                //case Rawr.ItemSlot.Trinket: return CharacterSlot.Trinket2;
                case Rawr.ItemSlot.Back: return CharacterSlot.Back;
                case Rawr.ItemSlot.OneHand: return CharacterSlot.MainHand;
                case Rawr.ItemSlot.TwoHand: return CharacterSlot.MainHand;
                case Rawr.ItemSlot.MainHand: return CharacterSlot.MainHand;
                case Rawr.ItemSlot.OffHand: return CharacterSlot.OffHand;
                case Rawr.ItemSlot.Ranged: return CharacterSlot.Ranged;
                case Rawr.ItemSlot.ProjectileBag: return CharacterSlot.ProjectileBag;
                case Rawr.ItemSlot.Tabard: return CharacterSlot.Tabard;
                case Rawr.ItemSlot.Shirt: return CharacterSlot.Shirt;
                case Rawr.ItemSlot.Red: return CharacterSlot.Gems;
                case Rawr.ItemSlot.Orange: return CharacterSlot.Gems;
                case Rawr.ItemSlot.Yellow: return CharacterSlot.Gems;
                case Rawr.ItemSlot.Green: return CharacterSlot.Gems;
                case Rawr.ItemSlot.Blue: return CharacterSlot.Gems;
                case Rawr.ItemSlot.Purple: return CharacterSlot.Gems;
                case Rawr.ItemSlot.Prismatic: return CharacterSlot.Gems;
                case Rawr.ItemSlot.Meta: return CharacterSlot.Metas;
                case Rawr.ItemSlot.Cogwheel: return CharacterSlot.Cogwheels;
                case Rawr.ItemSlot.Hydraulic: return CharacterSlot.Hydraulics;
                default: return CharacterSlot.None;
            }
        }
        #endregion

        #region Internal Variables used in calculations, These should not be saved to XML
        [XmlIgnore]
        internal ItemInstance[] _item;
        // Set to true to suppress ItemsChanged event
        [XmlIgnore]
        public bool IsLoading { get; set; }
        [XmlIgnore]
        public CalculationsBase CurrentCalculations { get { return Calculations.GetModel(CurrentModel); } }
        [XmlIgnore]
        public bool DisableBuffAutoActivation { get; set; }
        #endregion

        #region Items in Slots
        public int MaxWornItemLevel {
            get {
                int retVal = 0;
                foreach (CharacterSlot slot in EquippableCharacterSlots) {
                    if (slot == CharacterSlot.Tabard || slot == CharacterSlot.Shirt
                        || slot == CharacterSlot.ProjectileBag || slot == CharacterSlot.Projectile) { continue; }
                    if (this[slot] != null && this[slot].Item.ItemLevel > retVal) {
                        retVal = this[slot].Item.ItemLevel;
                    }
                }
                return retVal;
            }
        }
        public int MinWornItemLevel {
            get {
                int retVal = 10000;
                foreach (CharacterSlot slot in EquippableCharacterSlots) {
                    if (slot == CharacterSlot.Tabard || slot == CharacterSlot.Shirt
                        || slot == CharacterSlot.ProjectileBag || slot == CharacterSlot.Projectile) { continue; }
                    if (this[slot] != null && this[slot].Item.ItemLevel < retVal)
                    {
                        retVal = this[slot].Item.ItemLevel;
                    }
                }
                return retVal;
            }
        }
        public int AvgWornItemLevel {
            get {
                int retVal = 0;
                int count = 0;
                foreach (CharacterSlot slot in EquippableCharacterSlots) {
                    if (slot == CharacterSlot.Tabard || slot == CharacterSlot.Shirt
                        || slot == CharacterSlot.ProjectileBag || slot == CharacterSlot.Projectile) { continue; }
                    if (this[slot] != null)
                    {
                        retVal += this[slot].Item.ItemLevel;
                        count++;
                    }
                }
                //return count != 0 ? (int)Math.Round(((double)retVal / (double)count), MidpointRounding.AwayFromZero) : 0;  //MidpointRounding does not exist in Silverlight
                return count != 0 ? (int)((double)retVal / (double)count) : 0;
            }
        }

        [XmlIgnore]
        public ItemInstance Head { get { return this[CharacterSlot.Head]; } set { this[CharacterSlot.Head] = value; } }
        [XmlIgnore]
        public ItemInstance Neck { get { return this[CharacterSlot.Neck]; } set { this[CharacterSlot.Neck] = value; } }
        [XmlIgnore]
        public ItemInstance Shoulders { get { return this[CharacterSlot.Shoulders]; } set { this[CharacterSlot.Shoulders] = value; } }
        [XmlIgnore]
        public ItemInstance Back { get { return this[CharacterSlot.Back]; } set { this[CharacterSlot.Back] = value; } }
        [XmlIgnore]
        public ItemInstance Chest { get { return this[CharacterSlot.Chest]; } set { this[CharacterSlot.Chest] = value; } }
        [XmlIgnore]
        public ItemInstance Shirt { get { return this[CharacterSlot.Shirt]; } set { this[CharacterSlot.Shirt] = value; } }
        [XmlIgnore]
        public ItemInstance Tabard { get { return this[CharacterSlot.Tabard]; } set { this[CharacterSlot.Tabard] = value; } }
        [XmlIgnore]
        public ItemInstance Wrist { get { return this[CharacterSlot.Wrist]; } set { this[CharacterSlot.Wrist] = value; } }
        [XmlIgnore]
        public ItemInstance Hands { get { return this[CharacterSlot.Hands]; } set { this[CharacterSlot.Hands] = value; } }
        [XmlIgnore]
        public ItemInstance Waist { get { return this[CharacterSlot.Waist]; } set { this[CharacterSlot.Waist] = value; } }
        [XmlIgnore]
        public ItemInstance Legs { get { return this[CharacterSlot.Legs]; } set { this[CharacterSlot.Legs] = value; } }
        [XmlIgnore]
        public ItemInstance Feet { get { return this[CharacterSlot.Feet]; } set { this[CharacterSlot.Feet] = value; } }
        [XmlIgnore]
        public ItemInstance Finger1 { get { return this[CharacterSlot.Finger1]; } set { this[CharacterSlot.Finger1] = value; } }
        [XmlIgnore]
        public ItemInstance Finger2 { get { return this[CharacterSlot.Finger2]; } set { this[CharacterSlot.Finger2] = value; } }
        [XmlIgnore]
        public ItemInstance Trinket1 { get { return this[CharacterSlot.Trinket1]; } set { this[CharacterSlot.Trinket1] = value; } }
        [XmlIgnore]
        public ItemInstance Trinket2 { get { return this[CharacterSlot.Trinket2]; } set { this[CharacterSlot.Trinket2] = value; } }
        [XmlIgnore]
        public ItemInstance MainHand { get { return this[CharacterSlot.MainHand]; } set { this[CharacterSlot.MainHand] = value; } }
        [XmlIgnore]
        public ItemInstance OffHand { get { return this[CharacterSlot.OffHand]; } set { this[CharacterSlot.OffHand] = value; } }
        [XmlIgnore]
        public ItemInstance Ranged { get { return this[CharacterSlot.Ranged]; } set { this[CharacterSlot.Ranged] = value; } }
        [XmlIgnore]
        public ItemInstance Projectile { get { return this[CharacterSlot.Projectile]; } set { this[CharacterSlot.Projectile] = value; } }
        [XmlIgnore]
        public ItemInstance ProjectileBag { get { return this[CharacterSlot.ProjectileBag]; } set { this[CharacterSlot.ProjectileBag] = value; } }
        //[XmlIgnore]
        //public Item ExtraWristSocket { get { return this[CharacterSlot.ExtraWristSocket]; } set { this[CharacterSlot.ExtraWristSocket] = value; } }
        //[XmlIgnore]
        //public Item ExtraHandsSocket { get { return this[CharacterSlot.ExtraHandsSocket]; } set { this[CharacterSlot.ExtraHandsSocket] = value; } }
        //[XmlIgnore]
        //public Item ExtraWaistSocket { get { return this[CharacterSlot.ExtraWaistSocket]; } set { this[CharacterSlot.ExtraWaistSocket] = value; } }

        // leave in for now to reduce rebinding needed
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
        public Tinkering BackTinkering { get { return GetTinkeringBySlot(CharacterSlot.Back); } set { SetTinkeringBySlot(CharacterSlot.Back, value); } }
        [XmlIgnore]
        public Tinkering HandsTinkering { get { return GetTinkeringBySlot(CharacterSlot.Hands); } set { SetTinkeringBySlot(CharacterSlot.Hands, value); } }
        [XmlIgnore]
        public Tinkering WaistTinkering { get { return GetTinkeringBySlot(CharacterSlot.Waist); } set { SetTinkeringBySlot(CharacterSlot.Waist, value); } }

        [XmlIgnore]
        public Reforging HeadReforging { get { return GetReforgingBySlot(CharacterSlot.Head); } set { SetReforgingBySlot(CharacterSlot.Head, value); } }
        [XmlIgnore]
        public Reforging ShouldersReforging { get { return GetReforgingBySlot(CharacterSlot.Shoulders); } set { SetReforgingBySlot(CharacterSlot.Shoulders, value); } }
        [XmlIgnore]
        public Reforging BackReforging { get { return GetReforgingBySlot(CharacterSlot.Back); } set { SetReforgingBySlot(CharacterSlot.Back, value); } }
        [XmlIgnore]
        public Reforging ChestReforging { get { return GetReforgingBySlot(CharacterSlot.Chest); } set { SetReforgingBySlot(CharacterSlot.Chest, value); } }
        [XmlIgnore]
        public Reforging WristReforging { get { return GetReforgingBySlot(CharacterSlot.Wrist); } set { SetReforgingBySlot(CharacterSlot.Wrist, value); } }
        [XmlIgnore]
        public Reforging HandsReforging { get { return GetReforgingBySlot(CharacterSlot.Hands); } set { SetReforgingBySlot(CharacterSlot.Hands, value); } }
        [XmlIgnore]
        public Reforging LegsReforging { get { return GetReforgingBySlot(CharacterSlot.Legs); } set { SetReforgingBySlot(CharacterSlot.Legs, value); } }
        [XmlIgnore]
        public Reforging FeetReforging { get { return GetReforgingBySlot(CharacterSlot.Feet); } set { SetReforgingBySlot(CharacterSlot.Feet, value); } }
        [XmlIgnore]
        public Reforging Finger1Reforging { get { return GetReforgingBySlot(CharacterSlot.Finger1); } set { SetReforgingBySlot(CharacterSlot.Finger1, value); } }
        [XmlIgnore]
        public Reforging Finger2Reforging { get { return GetReforgingBySlot(CharacterSlot.Finger2); } set { SetReforgingBySlot(CharacterSlot.Finger2, value); } }
        [XmlIgnore]
        public Reforging MainHandReforging { get { return GetReforgingBySlot(CharacterSlot.MainHand); } set { SetReforgingBySlot(CharacterSlot.MainHand, value); } }
        [XmlIgnore]
        public Reforging OffHandReforging { get { return GetReforgingBySlot(CharacterSlot.OffHand); } set { SetReforgingBySlot(CharacterSlot.OffHand, value); } }
        [XmlIgnore]
        public Reforging RangedReforging { get { return GetReforgingBySlot(CharacterSlot.Ranged); } set { SetReforgingBySlot(CharacterSlot.Ranged, value); } }
        #endregion

        public void InvalidateItemInstances()
        {
            if (_relevantItems != null)
            {
                _relevantItems.Clear();
                _relevantItemInstances.Clear();
            }
        }

        public void InvalidateItemInstances(CharacterSlot slot)
        {
            _relevantItemInstances.Remove(slot);
        }

        [XmlIgnore]
        private Dictionary<CharacterSlot, List<ItemInstance>> _relevantItemInstances;

        [XmlIgnore]
        private Dictionary<CharacterSlot, List<Item>> _relevantItems;

        public List<ItemInstance> GetRelevantItemInstances(CharacterSlot slot, bool forceAll=false)
        {
            bool blacksmithingSocket = false;
            if ((slot == CharacterSlot.Waist && WaistBlacksmithingSocketEnabled)
                || (slot == CharacterSlot.Hands && HandsBlacksmithingSocketEnabled)
                || (slot == CharacterSlot.Wrist && WristBlacksmithingSocketEnabled))
            {
                blacksmithingSocket = true;
            }
            List<ItemInstance> items;
            if (!_relevantItemInstances.TryGetValue(slot, out items))
            {
                Dictionary<int, bool> itemChecked = new Dictionary<int, bool>();
                items = new List<ItemInstance>();
                foreach (Item item in (forceAll ? ItemCache.AllItems : ItemCache.RelevantItems))
                {
                    if (item.FitsInSlot(slot, this) && item.FitsFaction(Race))
                    {
                        itemChecked[item.Id] = true;
                        List<int> suffixList;
                        if (item.AllowedRandomSuffixes == null || item.AllowedRandomSuffixes.Count == 0)
                        {
                            suffixList = zeroSuffixList;
                        }
                        else
                        {
                            suffixList = item.AllowedRandomSuffixes;
                        }
                        foreach (int randomSuffix in suffixList)
                        {
                            foreach (Reforging reforging in CurrentCalculations.GetReforgingOptions(item, randomSuffix))
                            {
                                List<ItemInstance> itemInstances = new List<ItemInstance>();
                                foreach (GemmingTemplate template in CurrentGemmingTemplates)
                                {
                                    if (template.Enabled)
                                    {
                                        ItemInstance instance = template.GetItemInstance(item, randomSuffix, GetEnchantBySlot(slot), reforging, GetTinkeringBySlot(slot), blacksmithingSocket);
                                        if (!itemInstances.Contains(instance)) itemInstances.Add(instance);
                                    }
                                }
                                foreach (GemmingTemplate template in CustomGemmingTemplates)
                                {
                                    if (template.Enabled && template.Model == CurrentModel)
                                    {
                                        ItemInstance instance = template.GetItemInstance(item, randomSuffix, GetEnchantBySlot(slot), reforging, GetTinkeringBySlot(slot), blacksmithingSocket);
                                        if (!itemInstances.Contains(instance)) itemInstances.Add(instance);
                                    }
                                }
                                items.AddRange(itemInstances);
                            }
                        }
                    }
                }
                // add custom instances
                foreach (ItemInstance item in CustomItemInstances)
                {
                    if (item.Item != null && item.Item.FitsInSlot(slot, this)) // item.Item can be null if you're loading character with custom items that are not present on this install
                    {
                        // if it's already in make sure to set force visible to true
                        int index = items.IndexOf(item);
                        if (index >= 0)
                        {
                            items[index] = item;
                        }
                        else
                        {
                            items.Add(item);
                        }
                    }
                }
                // add available instances (green diamonds)
                foreach (string availableItem in AvailableItems)
                {
                    string[] ids = availableItem.Split('.');
                    if (ids.Length <= 2)
                    {
                        // we have an available item that might be filtered out
                        Item item = ItemCache.FindItemById(int.Parse(ids[0]));
                        if (item != null)
                        {
                            if (item.FitsInSlot(slot, this))
                            {
                                if (itemChecked.ContainsKey(item.Id))
                                {
                                    // we've already processed this one
                                    continue;
                                }
                                Enchant enchant = GetEnchantBySlot(slot);
                                Tinkering tinkering = GetTinkeringBySlot(slot);
                                List<ItemInstance> itemInstances = new List<ItemInstance>();
                                int randomSuffix = ids.Length < 2 ? 0 : int.Parse(ids[1]);
                                foreach (Reforging reforging in CurrentCalculations.GetReforgingOptions(item, randomSuffix))
                                {
                                    foreach (GemmingTemplate template in CurrentGemmingTemplates)
                                    {
                                        if (template.Enabled)
                                        {
                                            ItemInstance instance = template.GetItemInstance(item, randomSuffix, GetEnchantBySlot(slot), reforging, GetTinkeringBySlot(slot), blacksmithingSocket);
                                            if (!itemInstances.Contains(instance)) itemInstances.Add(instance);
                                        }
                                    }
                                    foreach (GemmingTemplate template in CustomGemmingTemplates)
                                    {
                                        if (template.Enabled && template.Model == CurrentModel)
                                        {
                                            ItemInstance instance = template.GetItemInstance(item, randomSuffix, GetEnchantBySlot(slot), reforging, GetTinkeringBySlot(slot), blacksmithingSocket);
                                            if (!itemInstances.Contains(instance)) itemInstances.Add(instance);
                                        }
                                    }
                                }
                                items.AddRange(itemInstances);
                            }
                            itemChecked[item.Id] = true;
                        }
                    }
                }
                // add available instances (blue diamonds)
                foreach (string availableItem in AvailableItems)
                {
                    string[] ids = availableItem.Split('.');
                    if (ids.Length == 8) // only support new format with random suffixes
                    {
                        Item item = ItemCache.FindItemById(int.Parse(ids[0]));
                        if (item.FitsInSlot(slot, this))
                        {
                            ItemInstance instance = new ItemInstance(availableItem);
                            instance.ForceDisplay = true;
                            // we want to force display even if it's already present (might be lower than top N)
                            int index = items.IndexOf(instance);
                            if (index < 0)
                            {
                                items.Add(instance);
                            }
                            else
                            {
                                items[index] = instance;
                            }
                        }
                    }
                } 
                _relevantItemInstances[slot] = items;
            }
            return items;
        }

        public void ClearRelevantGems() { _relevantItems.Remove(CharacterSlot.Gems); }
        public List<Item> GetRelevantItems(CharacterSlot slot) { return GetRelevantItems(slot, ItemSlot.None); }
        public List<Item> GetRelevantItems(CharacterSlot slot, ItemSlot gemColour)
        {
            List<Item> items;
            if (!_relevantItems.TryGetValue(slot, out items))
            {
                items = new List<Item>();
                foreach (Item item in ItemCache.RelevantItems)
                {
                    if (item.FitsInSlot(slot, this))
                    {
                        if(!item.IsJewelersGem || !Rawr.Properties.GeneralSettings.Default.HideProfEnchants ||
                           (item.IsJewelersGem && this.HasProfession(Profession.Jewelcrafting)))
                        {
                            if ((gemColour == ItemSlot.None) ||
                                (gemColour == ItemSlot.Red && item.IsRedGem) ||
                                (gemColour == ItemSlot.Yellow && item.IsYellowGem) ||
                                (gemColour == ItemSlot.Blue && item.IsBlueGem) ||
                                (gemColour == ItemSlot.Cogwheel && item.IsCogwheel) ||
                                (gemColour == ItemSlot.Hydraulic && item.IsHydraulic))
                            {
                                items.Add(item);
                            }
                        }
                    }
                }
                _relevantItems[slot] = items;
            }
            return items;
        }

        public void AssignAllTalentsFromCharacter(Character character, bool clone)
        {
            if (clone)
            {
                WarriorTalents = (WarriorTalents)character.WarriorTalents.Clone();
                PaladinTalents = (PaladinTalents)character.PaladinTalents.Clone();
                HunterTalents = (HunterTalents)character.HunterTalents.Clone();
                RogueTalents = (RogueTalents)character.RogueTalents.Clone();
                PriestTalents = (PriestTalents)character.PriestTalents.Clone();
                ShamanTalents = (ShamanTalents)character.ShamanTalents.Clone();
                MageTalents = (MageTalents)character.MageTalents.Clone();
                WarlockTalents = (WarlockTalents)character.WarlockTalents.Clone();
                DruidTalents = (DruidTalents)character.DruidTalents.Clone();
                DeathKnightTalents = (DeathKnightTalents)character.DeathKnightTalents.Clone();
            }
            else
            {
                _warriorTalents = character._warriorTalents;
                _paladinTalents = character._paladinTalents;
                _hunterTalents = character._hunterTalents;
                _rogueTalents = character._rogueTalents;
                _priestTalents = character._priestTalents;
                _shamanTalents = character._shamanTalents;
                _mageTalents = character._mageTalents;
                _warlockTalents = character._warlockTalents;
                _druidTalents = character._druidTalents;
                _deathKnightTalents = character._deathKnightTalents;
            }
        }

        public bool IsEquipped(ItemInstance itemToBeChecked)
        {
            CharacterSlot slot = Character.GetCharacterSlotByItemSlot(itemToBeChecked.Slot);
            if (slot == CharacterSlot.Finger1)
                return IsEquipped(itemToBeChecked, CharacterSlot.Finger1) || IsEquipped(itemToBeChecked, CharacterSlot.Finger2);
            else if (itemToBeChecked.Slot == Rawr.ItemSlot.OneHand)
                return IsEquipped(itemToBeChecked, CharacterSlot.MainHand) || IsEquipped(itemToBeChecked, CharacterSlot.OffHand);
            else if (itemToBeChecked.Slot == Rawr.ItemSlot.Trinket)
                return IsEquipped(itemToBeChecked, CharacterSlot.Trinket1) || IsEquipped(itemToBeChecked, CharacterSlot.Trinket2);
            else
                return IsEquipped(itemToBeChecked, slot);
        }
        public bool IsEquipped(ItemInstance itemToBeChecked, CharacterSlot slot)
        {
            return itemToBeChecked == this[slot];
        }
        public bool IsEquipped(Item itemToBeChecked)
        {
            CharacterSlot slot = Character.GetCharacterSlotByItemSlot(itemToBeChecked.Slot);
            if (slot == CharacterSlot.Finger1)
                return IsEquipped(itemToBeChecked, CharacterSlot.Finger1) || IsEquipped(itemToBeChecked, CharacterSlot.Finger2);
            else if (itemToBeChecked.Slot == Rawr.ItemSlot.OneHand)
                return IsEquipped(itemToBeChecked, CharacterSlot.MainHand) || IsEquipped(itemToBeChecked, CharacterSlot.OffHand);
            else if (itemToBeChecked.Slot == Rawr.ItemSlot.Trinket)
                return IsEquipped(itemToBeChecked, CharacterSlot.Trinket1) || IsEquipped(itemToBeChecked, CharacterSlot.Trinket2);
            else
                return IsEquipped(itemToBeChecked, slot);
        }
        public bool IsEquipped(Item itemToBeChecked, CharacterSlot slot)
        {
            return (object)this[slot] != null && itemToBeChecked.Id == this[slot].Id;
        }

        public Enchant GetEnchantBySlot(ItemSlot slot)
        {
            switch (slot)
            {
                case Rawr.ItemSlot.Head:
                    return HeadEnchant;
                case Rawr.ItemSlot.Shoulders:
                    return ShouldersEnchant;
                case Rawr.ItemSlot.Back:
                    return BackEnchant;
                case Rawr.ItemSlot.Chest:
                    return ChestEnchant;
                case Rawr.ItemSlot.Wrist:
                    return WristEnchant;
                case Rawr.ItemSlot.Hands:
                    return HandsEnchant;
                case Rawr.ItemSlot.Legs:
                    return LegsEnchant;
                case Rawr.ItemSlot.Feet:
                    return FeetEnchant;
                case Rawr.ItemSlot.Finger:
                    return Finger1Enchant;
                case Rawr.ItemSlot.MainHand:
                case Rawr.ItemSlot.OneHand:
                case Rawr.ItemSlot.TwoHand:
                    return MainHandEnchant;
                case Rawr.ItemSlot.OffHand:
                    return OffHandEnchant;
                case Rawr.ItemSlot.Ranged:
                    return RangedEnchant;
                default:
                    return null;
            }
        }

        public Tinkering GetTinkeringBySlot(ItemSlot slot)
        {
            switch (slot)
            {
                case Rawr.ItemSlot.Back:
                    return BackTinkering;
                case Rawr.ItemSlot.Hands:
                    return HandsTinkering;
                case Rawr.ItemSlot.Waist:
                    return WaistTinkering;
                default:
                    return null;
            }
        }

        public Enchant GetEnchantBySlot(CharacterSlot slot)
        {
            ItemInstance item = this[slot];
            if ((object)item == null) return null;
            return item.Enchant;
        }

        public Tinkering GetTinkeringBySlot(CharacterSlot slot)
        {
            ItemInstance item = this[slot];
            if ((object)item == null) return null;
            return item.Tinkering;
        }

        public Reforging GetReforgingBySlot(CharacterSlot slot)
        {
            ItemInstance item = this[slot];
            if ((object)item == null) return null;
            return item.Reforging;
        }

        public bool IsEnchantable(CharacterSlot slot)
        {
            switch (slot)
            {
                case CharacterSlot.Head:
                case CharacterSlot.Shoulders:
                case CharacterSlot.Back:
                case CharacterSlot.Chest:
                case CharacterSlot.Wrist:
                case CharacterSlot.Hands:
                case CharacterSlot.Legs:
                case CharacterSlot.Feet:
                case CharacterSlot.Finger1:
                case CharacterSlot.Finger2:
                case CharacterSlot.MainHand:
                case CharacterSlot.OffHand:
                case CharacterSlot.Ranged:
                    return true;
                default:
                    return false;
            }
        }
        public bool IsEnchantable(ItemSlot slot)
        {
            switch (slot)
            {
                case Rawr.ItemSlot.Head:
                case Rawr.ItemSlot.Shoulders:
                case Rawr.ItemSlot.Back:
                case Rawr.ItemSlot.Chest:
                case Rawr.ItemSlot.Wrist:
                case Rawr.ItemSlot.Hands:
                case Rawr.ItemSlot.Legs:
                case Rawr.ItemSlot.Feet:
                case Rawr.ItemSlot.Finger:
                case Rawr.ItemSlot.TwoHand:
                case Rawr.ItemSlot.MainHand:
                case Rawr.ItemSlot.OneHand:
                case Rawr.ItemSlot.OffHand:
                case Rawr.ItemSlot.Ranged:
                    return true;
                default:
                    return false;
            }
        }

        public bool IsTinkeringable(CharacterSlot slot)
        {
            switch (slot)
            {
                case CharacterSlot.Back:
                case CharacterSlot.Hands:
                case CharacterSlot.Waist:
                    return true;
                default:
                    return false;
            }
        }
        public bool IsTinkeringable(ItemSlot slot)
        {
            switch (slot)
            {
                case Rawr.ItemSlot.Back:
                case Rawr.ItemSlot.Hands:
                case Rawr.ItemSlot.Waist:
                    return true;
                default:
                    return false;
            }
        }

        public void SetReforgingBySlot(ItemSlot slot, Reforging reforge)
        {
            switch (slot)
            {
                case Rawr.ItemSlot.Head: HeadReforging = reforge; break;
                case Rawr.ItemSlot.Shoulders: ShouldersReforging = reforge; break;
                case Rawr.ItemSlot.Back: BackReforging = reforge; break;
                case Rawr.ItemSlot.Chest: ChestReforging = reforge; break;
                case Rawr.ItemSlot.Wrist: WristReforging = reforge; break;
                case Rawr.ItemSlot.Hands: HandsReforging = reforge; break;
                case Rawr.ItemSlot.Legs: LegsReforging = reforge; break;
                case Rawr.ItemSlot.Feet: FeetReforging = reforge; break;
                case Rawr.ItemSlot.Finger: Finger1Reforging = reforge; break;
                case Rawr.ItemSlot.MainHand:
                case Rawr.ItemSlot.OneHand:
                case Rawr.ItemSlot.TwoHand: MainHandReforging = reforge; break;
                case Rawr.ItemSlot.OffHand: OffHandReforging = reforge; break;
                case Rawr.ItemSlot.Ranged: RangedReforging = reforge; break;
            }
        }
        public void SetReforgingBySlot(CharacterSlot slot, Reforging reforge)
        {
            int i = (int)slot;
            if (i < 0 || i >= SlotCount) return;
            ItemInstance item = this[slot];
            if ((object)item != null) item.Reforging = reforge;
            OnCalculationsInvalidated();
        }

        public void SetEnchantBySlot(ItemSlot slot, Enchant enchant)
        {
            switch (slot)
            {
                case Rawr.ItemSlot.Head:
                    HeadEnchant = enchant;
                    break;
                case Rawr.ItemSlot.Shoulders:
                    ShouldersEnchant = enchant;
                    break;
                case Rawr.ItemSlot.Back:
                    BackEnchant = enchant;
                    break;
                case Rawr.ItemSlot.Chest:
                    ChestEnchant = enchant;
                    break;
                case Rawr.ItemSlot.Wrist:
                    WristEnchant = enchant;
                    break;
                case Rawr.ItemSlot.Hands:
                    HandsEnchant = enchant;
                    break;
                case Rawr.ItemSlot.Legs:
                    LegsEnchant = enchant;
                    break;
                case Rawr.ItemSlot.Feet:
                    FeetEnchant = enchant;
                    break;
                case Rawr.ItemSlot.Finger:
                    Finger1Enchant = enchant;
                    break;
                case Rawr.ItemSlot.MainHand:
                case Rawr.ItemSlot.OneHand:
                case Rawr.ItemSlot.TwoHand:
                    MainHandEnchant = enchant;
                    break;
                case Rawr.ItemSlot.OffHand:
                    OffHandEnchant = enchant;
                    break;
                case Rawr.ItemSlot.Ranged:
                    RangedEnchant = enchant;
                    break;
            }
        }
        public void SetEnchantBySlot(CharacterSlot slot, Enchant enchant)
        {
            int i = (int)slot;
            if (i < 0 || i >= SlotCount) return;
            ItemInstance item = this[slot];
            if ((object)item != null) item.Enchant = enchant;
            OnCalculationsInvalidated();
        }

        public void SetTinkeringBySlot(ItemSlot slot, Tinkering tinkering)
        {
            switch (slot)
            {
                case Rawr.ItemSlot.Back: BackTinkering = tinkering; break;
                case Rawr.ItemSlot.Hands: HandsTinkering = tinkering; break;
                case Rawr.ItemSlot.Waist: WaistTinkering = tinkering; break;
                default: break;
            }
        }
        public void SetTinkeringBySlot(CharacterSlot slot, Tinkering tinkering)
        {
            int i = (int)slot;
            if (i < 0 || i >= SlotCount) return;
            ItemInstance item = this[slot];
            if ((object)item != null) item.Tinkering = tinkering;
            OnCalculationsInvalidated();
        }

        #region Gem Counting Functions, Used for managing Gem Requirements
        // cache gem counts as this takes the most time of accumulating item stats
        // this becomes invalid when items on character change, invalidate in OnItemsChanged
        private bool gemCountValid;
        private int redGemCount;
        private int yellowGemCount;
        private int blueGemCount;
        private int jewelersGemCount;
        private int gemRequirementsInvalid;
        private int nonjewelerGemRequirementsInvalid;

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

        public int JewelersGemCount
        {
            get
            {
                ComputeGemCount();
                return jewelersGemCount;
            }
        }

        public int GemRequirementsInvalid
        {
            get
            {
                ComputeGemCount();
                return gemRequirementsInvalid;
            }
        }

        public int NonjewelerGemRequirementsInvalid
        {
            get
            {
                ComputeGemCount();
                return nonjewelerGemRequirementsInvalid;
            }
        }

        public bool IsMetaGemActive
        {
            get
            {
                ItemInstance head = _item[1];
                if (head == null) return true;
                Item metagem = head.Gem1;
                if (metagem == null) return true;
                return metagem.MeetsRequirements(this);
            }
        }

        private void ComputeGemCount()
        {
            if (!gemCountValid)
            {
                redGemCount = 0;
                yellowGemCount = 0;
                blueGemCount = 0;
                jewelersGemCount = 0;
                Dictionary<int, bool> uniqueMap = null;
                gemRequirementsInvalid = 0;
                nonjewelerGemRequirementsInvalid = 0;
                for (int slot = 0; slot < OptimizableSlotCount; slot++)
                {
                    if (slot != (int)CharacterSlot.OffHand || CurrentCalculations.IncludeOffHandInCalculations(this))
                    {
                        ItemInstance item = _item[slot];
                        if (item == null) continue;
                        for (int gemIndex = 1; gemIndex <= 3; gemIndex++)
                        {
                            Item gem = item.GetGem(gemIndex);
                            if (gem != null)
                            {
                                if (gem.IsRedGem) redGemCount++;
                                if (gem.IsYellowGem) yellowGemCount++;
                                if (gem.IsBlueGem) blueGemCount++;
                                if (gem.IsJewelersGem) jewelersGemCount++;
                                else if (gem.Unique) // needs else, it seems jewelers gems are marked as unique
                                {
                                    if (uniqueMap == null)
                                    {
                                        uniqueMap = new Dictionary<int, bool>(); // this is a rare case, only create dictionary when really needed
                                    }
                                    if (uniqueMap.ContainsKey(gem.Id))
                                    {
                                        gemRequirementsInvalid++;
                                        nonjewelerGemRequirementsInvalid++;
                                    }
                                    else
                                    {
                                        uniqueMap[gem.Id] = true;
                                    }
                                }
                            }
                        }
                    }
                }
                if (jewelersGemCount > 3)
                {
                    gemRequirementsInvalid += jewelersGemCount - 3;
                }

                gemCountValid = true;
            }
        }

        public bool IsUniqueGemEquipped(Item testGem)
        {
            for (int slot = 0; slot < OptimizableSlotCount; slot++)
            {
                if (slot != (int)CharacterSlot.OffHand || CurrentCalculations.IncludeOffHandInCalculations(this))
                {
                    ItemInstance item = _item[slot];
                    if (item == null) continue;
                    for (int gemIndex = 1; gemIndex <= 3; gemIndex++)
                    {
                        Item gem = item.GetGem(gemIndex);
                        if (gem != null && !gem.IsJewelersGem)
                        {
                            if (gem.Unique && gem == testGem) 
                                return true;
                        }
                    }
                }
            }
            return false;
        }
        
        private int GetItemGemIdCount(ItemInstance item, int id)
        {
            int count = 0;
            if ((object)item != null)
            {
                if (item.Gem1 != null && item.Gem1.Id == id) count++;
                if (item.Gem2 != null && item.Gem2.Id == id) count++;
                if (item.Gem3 != null && item.Gem3.Id == id) count++;
            }
            return count;
        }

        public int GetGemIdCount(int id)
        {
            int count = 0;
            for (int slot = 0; slot < SlotCount; slot++)
            {
                count += GetItemGemIdCount(_item[slot], id);
            }
            return count;
        }
        #endregion

        public event EventHandler CalculationsInvalidated;
        public void OnCalculationsInvalidated()
        {
#if DEBUG
            if (CalculationsInvalidated != null) System.Diagnostics.Debug.WriteLine("Starting CalculationsInvalidated: " + this.Name);
            DateTime start = DateTime.Now;
#endif
            gemCountValid = false; // invalidate gem counts
            InvalidateItemInstances();
            if (IsLoading) {
#if DEBUG
                System.Diagnostics.Debug.WriteLine("CalculationsInvalidated: " + this.Name +" Skipped: Character Is Loading");
#endif
                return;
            }
            RecalculateSetBonuses();

            if (CalculationsInvalidated != null)
            {
                CalculationsInvalidated(this, EventArgs.Empty);
#if DEBUG
                System.Diagnostics.Debug.WriteLine("Finished CalculationsInvalidated: Total " + DateTime.Now.Subtract(start).TotalMilliseconds.ToString() + "ms");
#endif
            }
        }

        private Dictionary<string, int> _setBonusCount = null;
        [XmlIgnore]
        public Dictionary<string, int> SetBonusCount {
            get {
                if (_setBonusCount == null)
                {
                    RecalculateSetBonuses();
                }
                return _setBonusCount;
            }
            private set { _setBonusCount = value; }
        }

        public void RecalculateSetBonuses()
        {
            if (_setBonusCount == null) {
                SetBonusCount = new Dictionary<string, int>();
            } else {
                SetBonusCount.Clear();
            }
            //Compute Set Bonuses
            for (int slot = 0; slot < _item.Length; slot++)
            {
                ItemInstance item = _item[slot];
                if ((object)item != null)
                {
                    Item i = item.Item;
                    if (i != null && !string.IsNullOrEmpty(i.SetName))
                    {
                        int count;
                        SetBonusCount.TryGetValue(i.SetName, out count);
                        SetBonusCount[i.SetName] = count + 1;
                    }
                }
            }
        }

        [XmlIgnore]
        public ItemInstance this[CharacterSlot slot]
        {
            get
            {
                int i = (int)slot;
                if (i < 0 || i >= SlotCount) return null;
                return _item[i];
            }
            set
            {
                int i = (int)slot;
                if (i < 0 || i >= SlotCount) return;
                // should we track id changes? for now assume assume we don't have to
                _item[i] = value;
                OnCalculationsInvalidated();
                //if (value == null || _item[i] != value.GemmedId) 
                //{
                //    _item[i] = value != null ? value.GemmedId : null;
                //    if (_itemCached[i] != null && _trackEquippedItemChanges) _itemCached[i].IdsChanged -= new EventHandler(_itemCached_IdsChanged);
                //    _itemCached[i] = value;
                //    if (_itemCached[i] != null && _trackEquippedItemChanges) _itemCached[i].IdsChanged += new EventHandler(_itemCached_IdsChanged);
                //    OnCalculationsInvalidated();
                //}
            }
        }

        public string[] GetAllEquippedGearIds()
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
            return new List<string>(_ids.Keys).ToArray();
        }
        public string[] GetAllEquippedAndAvailableGearIds()
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
                if (!xid.StartsWith("-", StringComparison.Ordinal))
                {
                    int dot = xid.LastIndexOf('.');
                    _ids[(dot >= 0) ? xid.Substring(0, dot).Replace(".*.*.*", "") : xid] = true;
                }
            }
            return new List<string>(_ids.Keys).ToArray();
        }

        public CharacterSlot[] GetEquippedSlots(ItemInstance item)
        {
            List<CharacterSlot> listSlots = new List<CharacterSlot>();
            foreach (CharacterSlot slot in CharacterSlots)
                if (this[slot] == item)
                    listSlots.Add(slot);
            return listSlots.ToArray();
        }

        public static CharacterSlot GetCharacterSlotFromId(int slotId)
        {
            CharacterSlot cslot = CharacterSlot.None;
            switch (slotId)
            {
                case -1:
                    cslot = CharacterSlot.None;
                    break;
                case 1:
                    cslot = CharacterSlot.Head;
                    break;
                case 2:
                    cslot = CharacterSlot.Neck;
                    break;
                case 3:
                    cslot = CharacterSlot.Shoulders;
                    break;
                case 15:
                    cslot = CharacterSlot.Back;
                    break;
                case 5:
                    cslot = CharacterSlot.Chest;
                    break;
                case 4:
                    cslot = CharacterSlot.Shirt;
                    break;
                case 19:
                    cslot = CharacterSlot.Tabard;
                    break;
                case 9:
                    cslot = CharacterSlot.Wrist;
                    break;
                case 10:
                    cslot = CharacterSlot.Hands;
                    break;
                case 6:
                    cslot = CharacterSlot.Waist;
                    break;
                case 7:
                    cslot = CharacterSlot.Legs;
                    break;
                case 8:
                    cslot = CharacterSlot.Feet;
                    break;
                case 11:
                    cslot = CharacterSlot.Finger1;
                    break;
                case 12:
                    cslot = CharacterSlot.Finger2;
                    break;
                case 13:
                    cslot = CharacterSlot.Trinket1;
                    break;
                case 14:
                    cslot = CharacterSlot.Trinket2;
                    break;
                case 16:
                    cslot = CharacterSlot.MainHand;
                    break;
                case 17:
                    cslot = CharacterSlot.OffHand;
                    break;
                case 18:
                    cslot = CharacterSlot.Ranged;
                    break;
                case 0:
                    cslot = CharacterSlot.Projectile;
                    break;
                case 102:
                    cslot = CharacterSlot.ProjectileBag;
                    break;
            }
            return cslot;
        }

        #region Character Contructors and Initializers
        private void Initialize()
        {
            // common initialization used by constructors
            // avoid inline instantiation of fields as not all constructors want/need the overhead
            _item = new ItemInstance[SlotCount];
            _availableItems = new List<string>();
            _calculationOptions = new SerializableDictionary<string, ICalculationOptionBase>();
            _customItemInstances = new List<ItemInstance>();
            CustomGemmingTemplates = new List<GemmingTemplate>();
            GemmingTemplateOverrides = new List<GemmingTemplate>();
            _relevantItemInstances = new Dictionary<CharacterSlot, List<ItemInstance>>();
            _relevantItems = new Dictionary<CharacterSlot, List<Item>>();
        }

        public Character() 
        {
            // this constructor is used be deserialization
            // deserialization also sets all kinds of filter bools, make sure they don't trigger a million item cache changed events
            Initialize();
            _activeBuffs = new List<Buff>();
            IsLoading = true;
        }

        public Character(string name, string realm, CharacterRegion region, CharacterRace race, BossOptions boss,
            string head, string neck, string shoulders, string back, string chest, string shirt, string tabard,
                string wrist, string hands, string waist, string legs, string feet, string finger1, string finger2, 
            string trinket1, string trinket2, string mainHand, string offHand, string ranged, string projectile, 
            string projectileBag)
        {
            Initialize();
            IsLoading = true;
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

            WaistBlacksmithingSocketEnabled = true;
            _activeBuffs = new List<Buff>();
            SetFaction();
            IsLoading = false;
            RecalculateSetBonuses();

            BossOptions = boss.Clone();
        }

        public Character(string name, string realm, CharacterRegion region, CharacterRace race, BossOptions boss,
            ItemInstance head, ItemInstance neck, ItemInstance shoulders, ItemInstance back, ItemInstance chest, ItemInstance shirt, ItemInstance tabard,
                ItemInstance wrist, ItemInstance hands, ItemInstance waist, ItemInstance legs, ItemInstance feet, ItemInstance finger1, ItemInstance finger2,
            ItemInstance trinket1, ItemInstance trinket2, ItemInstance mainHand, ItemInstance offHand, ItemInstance ranged, ItemInstance projectile,
            ItemInstance projectileBag)
        {
            Initialize();
            //_trackEquippedItemChanges = trackEquippedItemChanges;
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
            _activeBuffs = new List<Buff>();
            SetFaction();
            IsLoading = false;
            RecalculateSetBonuses();
            BossOptions = boss.Clone();
        }

        // The following are special contructors used by optimizer, they assume the cached items/enchant are always used, and the underlying gemmedid/enchantid are never used
        public Character(string name, string realm, CharacterRegion region, CharacterRace race, BossOptions boss,
            ItemInstance head, ItemInstance neck, ItemInstance shoulders, ItemInstance back, ItemInstance chest, ItemInstance shirt, ItemInstance tabard,
                ItemInstance wrist, ItemInstance hands, ItemInstance waist, ItemInstance legs, ItemInstance feet, ItemInstance finger1, ItemInstance finger2, 
            ItemInstance trinket1, ItemInstance trinket2, ItemInstance mainHand, ItemInstance offHand, ItemInstance ranged, ItemInstance projectile,
            ItemInstance projectileBag, List<Buff> activeBuffs, string model)
        {
            Initialize();
            IsLoading = true;
            _name = name;
            _realm = realm;
            _region = region;
            _race = race;
            _item[(int)CharacterSlot.Head] = head;
            _item[(int)CharacterSlot.Neck] = neck;
            _item[(int)CharacterSlot.Shoulders] = shoulders;
            _item[(int)CharacterSlot.Back] = back;
            _item[(int)CharacterSlot.Chest] = chest;
            _item[(int)CharacterSlot.Shirt] = shirt;
            _item[(int)CharacterSlot.Tabard] = tabard;
            _item[(int)CharacterSlot.Wrist] = wrist;
            _item[(int)CharacterSlot.Hands] = hands;
            _item[(int)CharacterSlot.Waist] = waist;
            _item[(int)CharacterSlot.Legs] = legs;
            _item[(int)CharacterSlot.Feet] = feet;
            _item[(int)CharacterSlot.Finger1] = finger1;
            _item[(int)CharacterSlot.Finger2] = finger2;
            _item[(int)CharacterSlot.Trinket1] = trinket1;
            _item[(int)CharacterSlot.Trinket2] = trinket2;
            _item[(int)CharacterSlot.MainHand] = mainHand;
            _item[(int)CharacterSlot.OffHand] = offHand;
            _item[(int)CharacterSlot.Ranged] = ranged;
            _item[(int)CharacterSlot.Projectile] = projectile;
            _item[(int)CharacterSlot.ProjectileBag] = projectileBag;
            IsLoading = false;
            ActiveBuffs = new List<Buff>(activeBuffs);
            SetFaction();
            CurrentModel = model;
            RecalculateSetBonuses();

            BossOptions = boss.Clone();
        }

        /// <summary>This overload is used from optimizer and is optimized for performance, do not modify</summary>
        public Character(Character baseCharacter, object[] items, int count)
        {
            IsLoading = true;
            _name = baseCharacter._name;
            _realm = baseCharacter._realm;
            _region = baseCharacter._region;
            _race = baseCharacter._race;
            _currentModel = baseCharacter._currentModel;
            _calculationOptions = baseCharacter._calculationOptions;
            _primaryProfession = baseCharacter._primaryProfession;
            _secondaryProfession = baseCharacter._secondaryProfession;
            _class = baseCharacter._class;
            AssignAllTalentsFromCharacter(baseCharacter, false);
            CalculationToOptimize = baseCharacter.CalculationToOptimize;
            OptimizationRequirements = baseCharacter.OptimizationRequirements;
            _bossOptions = baseCharacter._bossOptions;
            _faction = baseCharacter._faction;

            _item = new ItemInstance[SlotCount];
            Array.Copy(items, _item, count);

            IsLoading = false;
            ActiveBuffs = new List<Buff>(baseCharacter.ActiveBuffs);
            RecalculateSetBonuses();
        }

        /// <summary>
        /// This is a variant of the above constructor used when recycling Character, assuming
        /// it was first created with the above constructor and same baseCharacter.
        /// </summary>
        internal void InitializeCharacter(object[] items, int count)
        {
            gemCountValid = false;
            Array.Copy(items, _item, count);
            RecalculateSetBonuses();
        }

        public Character(string name, string realm, CharacterRegion region, CharacterRace race, BossOptions boss,
            ItemInstance[] items, List<Buff> activeBuffs, string model)
        {
            Initialize();
            IsLoading = true;
            _name = name;
            _realm = realm;
            _region = region;
            _race = race;
            Array.Copy(items, _item, items.Length);

            IsLoading = false;
            ActiveBuffs = new List<Buff>(activeBuffs);
            SetFaction();
            CurrentModel = model;
            RecalculateSetBonuses();

            BossOptions = boss.Clone();
        }

        public Character Clone()
        {
            ItemInstance[] clonedItemInstances = new ItemInstance[SlotCount];
            for (int i = 0; i < clonedItemInstances.Length; i++)
            {
                ItemInstance itemInstance = _item[i];
                if (itemInstance != null) clonedItemInstances[i] = itemInstance.Clone();
            }
            Character clone = new Character(this.Name, this.Realm, this.Region, this.Race, this.BossOptions.Clone(),
                clonedItemInstances, ActiveBuffs, CurrentModel);
            clone.CalculationOptions = this.CalculationOptions;
            clone.itemSetList = this.itemSetList;
            clone.Class = this.Class;
            clone.AssignAllTalentsFromCharacter(this, true);
            clone.PrimaryProfession = this.PrimaryProfession;
            clone.SecondaryProfession = this.SecondaryProfession;
            clone.WaistBlacksmithingSocketEnabled = this.WaistBlacksmithingSocketEnabled;
            clone.WristBlacksmithingSocketEnabled = this.WristBlacksmithingSocketEnabled;
            clone.HandsBlacksmithingSocketEnabled = this.HandsBlacksmithingSocketEnabled;
            clone.OptimizationRequirements = this.OptimizationRequirements;
            clone.CalculationToOptimize = this.CalculationToOptimize;
            clone.BossOptions = this.BossOptions.Clone();
            return clone;
        }
    
        public void Save(Stream stream, bool closeStream = false)
        {
            SerializeCalculationOptions();
            SaveGemmingTemplateOverrides();
            SaveItemFilterEnabledOverride();
            _activeBuffsXml = new List<string>(_activeBuffs.ConvertAll(buff => buff.Name));
            _itemSetListXML = new List<string>(itemSetList.ConvertAll(ItemSet => ItemSet.ToGemmedIDList()));
            //if(ArmoryPets!=null) ArmoryPetsXml = new List<string>(ArmoryPets.ConvertAll(ArmoryPet => ArmoryPet.ToString()));

            XmlSerializer serializer = new XmlSerializer(typeof(Character));
            serializer.Serialize(stream, this);
            if (closeStream) stream.Close();
        }

        public void SaveBuffs(Stream writer)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Buff>));
            serializer.Serialize(writer, _activeBuffs);
            writer.Close();
        }

        public static Character LoadFromXml(string xml)
        {
            Character character;
            if (!string.IsNullOrEmpty(xml))
            {
                try
                {
                    xml = xml.Replace("<Region>en", "<Region>US").Replace("<Weapon>", "<MainHand>").Replace("</Weapon>", "</MainHand>")
                        .Replace("<Idol>", "<Ranged>").Replace("</Idol>", "</Ranged>").Replace("<WeaponEnchant>", "<MainHandEnchant>").Replace("</WeaponEnchant>", "</MainHandEnchant>")
                        .Replace("HolyPriest", "HealPriest")
                        .Replace("T7_0", "T11_10").Replace("T8_0", "T11_10").Replace("T9_0", "T11_10").Replace("T10_0", "T11_10")
                        .Replace("T7_5", "T11_10").Replace("T8_5", "T11_10").Replace("T9_5", "T11_10").Replace("T10_5", "T11_10")
                        .Replace("T7_9", "T11_10").Replace("T8_9", "T11_10").Replace("T9_9", "T11_10").Replace("T10_9", "T11_10")
                        .Replace("T11_0", "T11_10").Replace("T11_5", "T11_10").Replace("T11_9", "T11_10")
                        .Replace("<Attack xsi:type=\"DoT\">", "<Attack>");

                    if (xml.IndexOf("<CalculationOptions>") != xml.LastIndexOf("<CalculationOptions>"))
                    {
                        xml = xml.Substring(0, xml.IndexOf("<CalculationOptions>")) +
                            xml.Substring(xml.LastIndexOf("</CalculationOptions>") + "</CalculationOptions>".Length);
                    }

                    System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(Character));
                    System.IO.StringReader reader = new System.IO.StringReader(xml);
                    character = (Character)serializer.Deserialize(reader);
                    character.IsLoading = false;

                    // decode non-english characters for name and realm
                    if (!String.IsNullOrEmpty(character.Name))
                    {
                        character.Name = Uri.UnescapeDataString(character.Name);
                    }

                    // Realm is now not correctly encoded by rawr4 character-loading proxy, but it will work fine in future, I think.
                    if (!String.IsNullOrEmpty(character.Realm))
                    {
                        //character.Realm = Encoding. UnicodeEncoding.Convert(Encoding.UTF8, Encoding.Unicode, character.Realm.ToCharArray());
                        character.Realm = Uri.UnescapeDataString(character.Realm);
                    }

                    character._activeBuffs = new List<Buff>(character._activeBuffsXml.ConvertAll(buff => Buff.GetBuffByName(buff)));
                    character._activeBuffs.RemoveAll(buff => buff == null);
                    // remove all set bonuses, they should no longer be in active buffs
                    character._activeBuffs.RemoveAll(buff => !string.IsNullOrEmpty(buff.SetName));
                    character.itemSetList = new ItemSetList(character._itemSetListXML.ConvertAll(itemset => ItemSet.GenerateItemSetFromSavedString(itemset)));
                    character.itemSetList.RemoveAll(ItemSet => ItemSet == null);
                    character.ArmoryPets = new List<ArmoryPet>(character.ArmoryPetsXml.ConvertAll(armoryPet => ArmoryPet.GetPetByString(armoryPet)));
                    character.RecalculateSetBonuses(); // now you can call it
                    foreach (ItemInstance item in character.CustomItemInstances)
                    {
                        item.ForceDisplay = true;
                    }
                    for (int i = 0; i < character.AvailableItems.Count; i++)
                    {
                        // breaking compatibility with 2.0 characters with version 4.0.20
                        // assume all available items at this point are single id green diamonds
                        // or full 7 id gemmed id without random suffix
                        // convert gemmed ids to include random suffix
                        bool dirty = false;
                        string item = character.AvailableItems[i];
                        string[] ids = item.Split('.');
                        if (ids.Length == 7)
                        {
                            string[] nids = new string[] { ids[0], "0", ids[1], ids[2], ids[3], ids[4], ids[5], ids[6] };
                            item = string.Join(".", nids);
                            dirty = true;
                        }
                        if (dirty)
                        {
                            character.AvailableItems[i] = item;
                        }
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
#if !RAWR3 && !RAWR4
                    Log.Show("There was an error attempting to open this character. Most likely, it was saved with a previous version of Rawr, and isn't upgradable to the new format. Sorry. Please load your character from the armory to begin.");
#endif
                    new Base.ErrorBox() {
                        Title = "Error Generating Character from XML",
                        Function = "Character.LoadFromXML(...)",
                        TheException = ex,
                    }.Show();
                    character = new Character() { IsLoading = false };
                }
            }
            else
                character = new Character() { IsLoading = false };

            return character;
        }

        public void LoadBuffsFromXml(string path)
        {
            string xml = null;
#if !RAWR3 && !RAWR4
            if (File.Exists(path))
            {
                try
                {
                    xml = System.IO.File.ReadAllText(path);
                }
                catch (Exception)
                {
                    Log.Show("There was an error attempting to open this buff file.");
                }
            }
#endif
            if (!string.IsNullOrEmpty(xml))
            {
                try
                {
                    System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(List<string>));
                    System.IO.StringReader reader = new System.IO.StringReader(xml);
                    List<string> buffs = (List<string>)serializer.Deserialize(reader);
                    _activeBuffs = new List<Buff>(buffs.ConvertAll(buff => Buff.GetBuffByName(buff))); ;
                    _activeBuffs.RemoveAll(buff => buff == null);
                    OnCalculationsInvalidated();
                    reader.Close();
                }
                catch (Exception)
                {
#if !RAWR3 && !RAWR4
                    Log.Show("There was an error attempting to open this buffs file. Most likely, it was saved with a previous beta of Rawr, and isn't upgradable to the new format. Sorry. No buff changes have been applied.");
#endif
                }
            }
        }

        //public string ToCompressedString()
        //{
        //    //TODO: Just messing around with potential ways to serialize a character down to a string short enough to fit in a URL (<2000 characters)

        //    //List<object> objectsToSerialize = new List<object>();
        //    //objectsToSerialize.Add(Name);
        //    //objectsToSerialize.Add(Race);
        //    //objectsToSerialize.Add(Region);
        //    //objectsToSerialize.Add(Realm);
        //    //objectsToSerialize.Add(Class);
        //    //objectsToSerialize.Add(string.Join("|", _item.ConvertAll(itemInstance => itemInstance == null ? string.Empty : itemInstance.GemmedId).ToArray()));
        //    //objectsToSerialize.Add(string.Join("|", _activeBuffs.ConvertAll(buff=>buff.Name).ToArray()));
        //    //objectsToSerialize.Add(CurrentModel);
        //    //objectsToSerialize.Add(CurrentTalents.Data);
        //    //objectsToSerialize.Add(CurrentTalents.GlyphData);
        //    //objectsToSerialize.Add(EnforceGemRequirements);
        //    //objectsToSerialize.Add(WristBlacksmithingSocketEnabled);
        //    //objectsToSerialize.Add(WaistBlacksmithingSocketEnabled);
        //    //objectsToSerialize.Add(HandsBlacksmithingSocketEnabled);
        //    //objectsToSerialize.Add(CalculationOptions.GetXml());
        //    //objectsToSerialize.Add(string.Join("|", AvailableItems.ToArray()));

        //    //MemoryStream stream = new MemoryStream();
        //    //StreamWriter writer = new StreamWriter(stream);
        //    //writer.Write(objectsToSerialize[6].ToString());
        //    //string base64 = System.Convert.ToBase64String(stream.ToArray());


        //    //_serializedCalculationOptions.Clear();
        //    //SerializeCalculationOptions();
        //    //_activeBuffsXml = new List<string>(_activeBuffs.ConvertAll(buff => buff.Name));
        //    //if (this.Class != CharacterClass.DeathKnight) this.DeathKnightTalents = null;
        //    //if (this.Class != CharacterClass.Druid) this.DruidTalents = null;
        //    //if (this.Class != CharacterClass.Hunter) this.HunterTalents = null;
        //    //if (this.Class != CharacterClass.Mage) this.MageTalents = null;
        //    //if (this.Class != CharacterClass.Paladin) this.PaladinTalents = null;
        //    //if (this.Class != CharacterClass.Priest) this.PriestTalents = null;
        //    //if (this.Class != CharacterClass.Rogue) this.RogueTalents = null;
        //    //if (this.Class != CharacterClass.Shaman) this.ShamanTalents = null;
        //    //if (this.Class != CharacterClass.Warlock) this.WarlockTalents = null;
        //    //if (this.Class != CharacterClass.Warrior) this.WarriorTalents = null;

            
        //    ////MemoryStream stream = new MemoryStream();
        //    ////XmlSerializer serializer = new XmlSerializer(typeof(Character));
        //    ////serializer.Serialize(stream, this);
        //    ////StreamReader reader = new StreamReader(stream);
        //    ////string serializedCharacter = reader.ReadToEnd();
        //    ////reader.Close();
        //    ////stream.Close();
        //    ////stream.Dispose();

            

        //    //return "";
        //}

        public static Character FromCompressedString(string characterString)
        {
            return null;
        }
        #endregion
    }

    public interface ICalculationOptionBase { string GetXml(); }

    public class ArmoryPet
    {
        public ArmoryPet(string family, string name, string speckey, string spec)
        {
            Family = PetFamilyIdToPetFamilyName(family);
            Name = name;
            Spec = spec;
            SpecKey = speckey;
        }
        public string Family;
        public string Name;
        private string _SpecKey = "";
        public string SpecKey {
            get {
                if (_SpecKey == "") { _SpecKey = PetFamilyToPetFamilyTree(Family); }
                return _SpecKey;
            }
            set {
                if (value == "") { _SpecKey = PetFamilyToPetFamilyTree(Family); }
                else { _SpecKey = value; }
            }
        }
        public string Spec;

        public override string ToString()
        {
            return Family + ": [" + Name + "] Spec: " + SpecKey + " '" + Spec + "'";
        }
        public static ArmoryPet GetPetByString(string input) {
            string family = "";
            string name = "";
            string specKey = "";
            string spec = "";
            try {
                int start = 0, end = input.IndexOf(':');
                family = input.Substring(start, end);
                start = input.IndexOf('[') + 1; end = input.IndexOf(']', start) - start;
                name = input.Substring(start, end);
                start = input.IndexOf("Spec:") + "Spec: ".Length; end = input.IndexOf(" '", start) - start;
                specKey = input.Substring(start, end);
                start = input.IndexOf("Spec:") + ("Spec: " + specKey + " '").Length; end = input.IndexOf("\'", start) - start;
                spec = input.Substring(start, end);
            } catch (Exception ex) {
                new Base.ErrorBox()
                {
                    Title = "Error converting character saved Armory Pets to class form",
                    Function = "GetPetByString(string input)",
                    TheException = ex,
                }.Show();
            }

            return new ArmoryPet(family, name, specKey, spec);
        }

        public static string PetFamilyToPetFamilyTree(string family)
        {
            switch (family)
            {
                case "Bat": case "24":
                case "BirdOfPrey": case "26":
                case "Chimaera": case "38":
                case "Dragonhawk": case "30":
                case "NetherRay": case "34":
                case "Ravager": case "31":
                case "Serpent": case "35":
                case "Silithid": case "41":
                case "Spider": case "3":
                case "SporeBat": case "33":
                case "WindSerpent": case "27":
                    return "Cunning";

                case "Bear": case "4":
                case "Boar": case "5":
                case "Crab": case "8":
                case "Crocolisk": case "6":
                case "Gorilla": case "9":
                case "Rhino": case "43":
                case "Scorpid": case "20":
                case "Turtle": case "21":
                case "WarpStalker": case "32":
                case "Worm": case "42":
                    return "Tenacity";

                case "CarrionBird": case "7":
                case "Cat": case "2":
                case "CoreHound": case "45":
                case "Devilsaur": case "39":
                case "Hyena": case "25":
                case "Moth": case "37":
                case "Raptor": case "11":
                case "SpiritBeast": case "46":
                case "Tallstrider": case "12":
                case "Wasp": case "44":
                case "Wolf": case "1":
                    return "Ferocity";
            }

            // hmmm!
            return "None";
        }
        public static string PetFamilyIdToPetFamilyName(string familyid)
        {
            switch (familyid)
            {
                case "Bat": case "24": return "Bat";
                case "BirdOfPrey": case "26": return "BirdOfPrey";
                case "Chimaera": case "38": return "Chimaera";
                case "Dragonhawk": case "30": return "Dragonhawk";
                case "NetherRay": case "34": return "NetherRay";
                case "Ravager": case "31": return "Ravager";
                case "Serpent": case "35": return "Serpent";
                case "Silithid": case "41": return "Silithid";
                case "Spider": case "3": return "Spider";
                case "SporeBat": case "33": return "SporeBat";
                case "WindSerpent": case "27": return "WindSerpent";

                case "Bear": case "4": return "Bear";
                case "Boar": case "5": return "Boar";
                case "Crab": case "8": return "Crab";
                case "Crocolisk": case "6": return "Crocolisk";
                case "Gorilla": case "9": return "Gorilla";
                case "Rhino": case "43": return "Rhino";
                case "Scorpid": case "20": return "Scorpid";
                case "Turtle": case "21": return "Turtle";
                case "WarpStalker": case "32": return "WarpStalker";
                case "Worm": case "42": return "Worm";

                case "CarrionBird": case "7": return "CarrionBird";
                case "Cat": case "2": return "Cat";
                case "CoreHound": case "45": return "CoreHound";
                case "Devilsaur": case "39": return "Devilsaur";
                case "Hyena": case "25": return "Hyena";
                case "Moth": case "37": return "Moth";
                case "Raptor": case "11": return "Raptor";
                case "SpiritBeast": case "46": return "SpiritBeast";
                case "Tallstrider": case "12": return "Tallstrider";
                case "Wasp": case "44": return "Wasp";
                case "Wolf": case "1": return "Wolf";
            }

            return familyid; // it's already a name
        }
        public static string PetFamilyNameToPetFamilyId(string familyname)
        {
            switch (familyname)
            {
                case "Bat": case "24": return "24";
                case "BirdOfPrey": case "26": return "26";
                case "Chimaera": case "38": return "38";
                case "Dragonhawk": case "30": return "30";
                case "NetherRay": case "34": return "34";
                case "Ravager": case "31": return "31";
                case "Serpent": case "35": return "35";
                case "Silithid": case "41": return "41";
                case "Spider": case "3": return "3";
                case "SporeBat": case "33": return "33";
                case "WindSerpent": case "27": return "27";

                case "Bear": case "4": return "4";
                case "Boar": case "5": return "5";
                case "Crab": case "8": return "8";
                case "Crocolisk": case "6": return "6";
                case "Gorilla": case "9": return "9";
                case "Rhino": case "43": return "43";
                case "Scorpid": case "20": return "20";
                case "Turtle": case "21": return "21";
                case "WarpStalker": case "32": return "32";
                case "Worm": case "42": return "42";

                case "CarrionBird": case "7": return "7";
                case "Cat": case "2": return "2";
                case "CoreHound": case "45": return "45";
                case "Devilsaur": case "39": return "39";
                case "Hyena": case "25": return "25";
                case "Moth": case "37": return "37";
                case "Raptor": case "11": return "11";
                case "SpiritBeast": case "46": return "46";
                case "Tallstrider": case "12": return "12";
                case "Wasp": case "44": return "44";
                case "Wolf": case "1": return "1";
            }

            return familyname; // it's already an id
        }
    }
}
