using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Text;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Rawr.Bosses;

namespace Rawr {
#if !SILVERLIGHT
    [Serializable]
#endif
    public partial class BossOptions : BossHandler
    {
        #region MyModelSupportsThis
        private static readonly Dictionary<string, bool> DefaultSupports = new Dictionary<string, bool>() {
            // Basics
            {"Level", true},
            {"Armor", true},
            {"MobType", true},
            {"Timers", true},
            {"Health", true},
            {"TimeSub35", true},
            {"TimeSub20", true},
            {"InBack_Melee", true},
            {"InBack_Ranged", true},
            {"RaidSize", true},
            // Offensive
            {"TargetGroups", true},
            {"Attacks", true},
            {"BuffStates", true},
            // Defensive
            {"Defensive", true},
            // Impedances
            {"Moves", true},
            {"Stuns", true},
            {"Fears", true},
            {"Roots", true},
            {"Silences", true},
            {"Disarms", true},
            {"Invulnerables", true}, // Not UI'd yet
        };
        protected static Dictionary<string, bool> DuplicateDefaultSupports() {
            Dictionary<string, bool> retVal = new Dictionary<string, bool>();
            foreach (string key in DefaultSupports.Keys)
            {
                retVal.Add(key, DefaultSupports[key]);
            }
            return retVal;
        }
        private static Dictionary<string, Dictionary<string, bool>> _MyModelSupportsThis = null;
        public static Dictionary<string, Dictionary<string, bool>> MyModelSupportsThis {
            get
            {
                if (_MyModelSupportsThis == null)
                {
                    _MyModelSupportsThis = new Dictionary<string, Dictionary<string, bool>>();
                    { // Melee DPS
                        Dictionary<string, bool> custom = DuplicateDefaultSupports();
                        custom["InBack_Ranged"] = false; // You aren't Ranged
                        _MyModelSupportsThis.Add("Cat", custom);
                        _MyModelSupportsThis.Add("DPSDK", custom);
                        _MyModelSupportsThis.Add("Enhance", custom);
                        _MyModelSupportsThis.Add("Rogue", custom);
                        _MyModelSupportsThis.Add("Retribution", custom);
                    }
                    { // Ranged DPS
                        Dictionary<string, bool> custom = DuplicateDefaultSupports();
                        custom["InBack_Melee"] = false; // You aren't melee
                        _MyModelSupportsThis.Add("Hunter", custom);
                        _MyModelSupportsThis.Add("Elemental", custom);
                        _MyModelSupportsThis.Add("Mage", custom);
                        _MyModelSupportsThis.Add("ShadowPriest", custom);
                        _MyModelSupportsThis.Add("Warlock", custom);
                    }
                    { // Tanks
                        Dictionary<string, bool> custom = DuplicateDefaultSupports();
                        custom["InBack_Melee"] = custom["InBack_Ranged"] = false; // The boss is focused on you
                        _MyModelSupportsThis.Add("ProtWarr", custom);
                        _MyModelSupportsThis.Add("TankDK", custom);
                    }
                    { // Heals
                        Dictionary<string, bool> custom = DuplicateDefaultSupports();
                        custom["InBack_Melee"] = custom["InBack_Ranged"] = false; // Doesn't matter
                        custom["Level"] = false; // Your target isn't the boss
                        custom["Armor"] = false; // You don't damage anything, so there's nothing for the armor to mitigate
                        custom["MobType"] = false; // Your target isn't the boss
                        custom["Defensive"] = false; // Your target isn't the boss
                        custom["Invulnerables"] = false; // Your target isn't the boss
                        custom["TimeSub35"] = false; // No abilities tied to this
                        custom["TimeSub20"] = false; // No abilities tied to this
                        _MyModelSupportsThis.Add("HealPriest", custom);
                        _MyModelSupportsThis.Add("RestoSham", custom);
                        _MyModelSupportsThis.Add("Tree", custom);
                    }
                    #region Bear
                    {
                        Dictionary<string, bool> custom = DuplicateDefaultSupports();
#if !DEBUG
                        custom["InBack_Melee"] = custom["InBack_Ranged"] = false; // Not Ranged
                        custom["MobType"] = false; // Your target isn't the boss
                        custom["TargetGroups"] = false; // Your target isn't these groups
                        custom["BuffStates"] = false;
                        custom["Defensive"] = false;
                        custom["Moves"] = false;
                        custom["Stuns"] = false;
                        custom["Fears"] = false;
                        custom["Roots"] = false;
                        custom["Silences"] = false;
                        custom["Disarms"] = false;
                        custom["Invulnerables"] = false; // Not UI'd yet
                        //custom["TimeSub35"] = false; // No abilities tied to this // we are going to show this now for the 35% target trinkets
#else
                        custom["InBack_Ranged"] = false; // Not Ranged
#endif
                        _MyModelSupportsThis.Add("Bear", custom);
                    }
                    #endregion
                    #region DPSWarr
                    {
                        Dictionary<string, bool> custom = DuplicateDefaultSupports();
                        custom["InBack_Ranged"] = false; // Not Ranged
                        //custom["TimeSub35"] = false; // No abilities tied to this // we are going to show this now for the 35% target trinkets
                        _MyModelSupportsThis.Add("DPSWarr", custom);
                    }
                    #endregion
                    #region ProtPaladin
                    {
                        // ProtPaladin needs a lot of work before it can support most of BossHandler
                        Dictionary<string, bool> custom = DuplicateDefaultSupports();
                        custom["Timers"] = false; // NYI
                        custom["Health"] = false; // NYI
                        custom["TimeSub35"] = false; // No abilities tied to this
                        custom["TimeSub20"] = false; // NYI
                        custom["InBack_Melee"] = custom["InBack_Ranged"] = false; // The boss is focused on you
                        custom["RaidSize"] = false; // NYI
                        custom["TargetGroups"] = false; // NYI
                        custom["Attacks"] = false; // NYI
                        custom["Defensive"] = false; // NYI
                        custom["Moves"] = false; // NYI
                        custom["Stuns"] = false; // NYI
                        custom["Fears"] = false; // NYI
                        custom["Roots"] = false; // NYI
                        custom["Disarms"] = false; // NYI
                        custom["Invulnerables"] = false; // NYI
                        _MyModelSupportsThis.Add("ProtPaladin", custom);
                    }
                    #endregion
                    #region Healadin
                    {
                        Dictionary<string, bool> custom = DuplicateDefaultSupports();
                        custom["Level"] = false; // Your target isn't the boss
                        custom["Armor"] = false; // You don't damage anything, so there's nothing for the armor to mitigate
                        custom["Health"] = false; // Your target isn't the boss
                        custom["MobType"] = false; // Your target isn't the boss
                        custom["TimeSub35"] = false; // No abilities tied to this
                        custom["TimeSub20"] = false; // No abilities tied to this
                        custom["InBack_Melee"] = false; // NYI
                        custom["InBack_Ranged"] = false; // You're not ranged
                        custom["RaidSize"] = false; // No abilities tied to this
                        custom["TargetGroups"] = false; // Your target isn't these groups
                        custom["Attacks"] = false; // We don't model damage taken by the player
                        custom["Defensive"] = false; // Your target isn't the boss
                        custom["Moves"] = false; // NYI
                        custom["Stuns"] = false; // NYI
                        custom["Fears"] = false; // NYI
                        custom["Roots"] = false; // NYI
                        custom["Disarms"] = false; // NYI
                        custom["Invulnerables"] = false; // Your target isn't the boss
                        _MyModelSupportsThis.Add("Healadin", custom);
                    }
                    #endregion
                    #region Moonkin
                    {
                        // Moonkin only needs to support a few features - we don't have any fancy executes or other cool stuff
                        Dictionary<string, bool> custom = DuplicateDefaultSupports();
                        custom["Health"] = false; // NYI
                        custom["TimeSub35"] = false; // Not applicable
                        custom["TimeSub20"] = false; // Not applicable
                        custom["InBack_Melee"] = custom["InBack_Ranged"] = false; // We're a caster, we don't care
                        custom["RaidSize"] = false; // NYI
                        custom["MobType"] = false; // Your target isn't the boss
                        custom["TargetGroups"] = false; // NYI
                        custom["Attacks"] = false; // NYI
                        custom["Defensive"] = false; // NYI
                        custom["Roots"] = false; // Not applicable
                        custom["Disarms"] = false; // Not applicable
                        _MyModelSupportsThis.Add("Moonkin", custom);
                    }
                    #endregion
                }
                return _MyModelSupportsThis;
            }
        }
        #endregion

        public BossOptions() { }
        public new BossOptions Clone() {
            BossOptions clone = (BossOptions)this.MemberwiseClone();
            return clone;
        }
        public BossOptions CloneThis(BossHandler toClone)
        {
            BossHandler clone = (BossHandler)toClone.Clone();
            // Info
            this.Name = clone.Name;
            this.Content = clone.Content;
            this.Instance = clone.Instance;
            this.Comment = clone.Comment;
            // Basics
            this.Level = clone.Level;
            this.Armor = clone.Armor;
            this.Health = clone.Health;
            this.MobType = clone.MobType;
            this.BerserkTimer = clone.BerserkTimer;
            this.SpeedKillTimer = clone.SpeedKillTimer;
            this.InBackPerc_Melee = clone.InBackPerc_Melee;
            this.InBackPerc_Ranged = clone.InBackPerc_Ranged;
            this.InBack = (this.InBackPerc_Melee + this.InBackPerc_Ranged) > 0f;
            this.Max_Players = clone.Max_Players;
            this.Min_Healers = clone.Min_Healers;
            this.Min_Tanks = clone.Min_Tanks;
            // Offensive
            this.Targets = clone.Targets; this.MultiTargs = this.Targets != null && this.Targets.Count > 0;
            this.BuffStates = clone.BuffStates; this.HasBuffStates = this.BuffStates != null && this.BuffStates.Count > 0;
            this.Attacks = clone.Attacks;
            this.DamagingTargs = (Attacks != null && Attacks.Count > 0);
            // Defensive
            this.Resist_Physical = clone.Resist_Physical;
            this.Resist_Frost = clone.Resist_Frost;
            this.Resist_Fire = clone.Resist_Fire;
            this.Resist_Nature = clone.Resist_Nature;
            this.Resist_Arcane = clone.Resist_Arcane;
            this.Resist_Shadow = clone.Resist_Shadow;
            this.Resist_Holy = clone.Resist_Holy;
            // Impedances
            this.Moves = clone.Moves; this.MovingTargs = this.Moves != null && this.Moves.Count > 0;
            this.Stuns = clone.Stuns; this.StunningTargs = this.Stuns != null && this.Stuns.Count > 0;
            this.Fears = clone.Fears; this.FearingTargs = this.Fears != null && this.Fears.Count > 0;
            this.Roots = clone.Roots; this.RootingTargs = this.Roots != null && this.Roots.Count > 0;
            this.Silences = clone.Silences; this.SilencingTargs = this.Silences != null && this.Silences.Count > 0;
            this.Disarms = clone.Disarms; this.DisarmingTargs = this.Disarms != null && this.Disarms.Count > 0;
            this.TimeBossIsInvuln = clone.TimeBossIsInvuln;
            //
            return this;
        }
        #region Variables
        private bool INBACK = false;
        [DefaultValue(false)]
        public bool InBack { get { return INBACK; } set { INBACK = value; OnPropertyChanged("InBack"); } }

        private bool MULTITARGS = false;
        private bool DAMAGINGTARGS = false;
        private bool HASBUFFSTATES = false;
        [DefaultValue(false)]
        public bool MultiTargs { get { return MULTITARGS; } set { MULTITARGS = value; OnPropertyChanged("MultiTargs"); } }
        [DefaultValue(false)]
        public bool DamagingTargs { get { return DAMAGINGTARGS; } set { DAMAGINGTARGS = value; OnPropertyChanged("DamagingTargs"); } }
        [DefaultValue(false)]
        public bool HasBuffStates { get { return HASBUFFSTATES; } set { HASBUFFSTATES = value; OnPropertyChanged("HasBuffStates"); } }

        private bool STUNNINGTARGS = false;
        [DefaultValue(false)]
        public bool StunningTargs { get { return STUNNINGTARGS; } set { STUNNINGTARGS = value; OnPropertyChanged("StunningTargs"); } }
        private bool MOVINGTARGS = false;
        [DefaultValue(false)]
        public bool MovingTargs { get { return MOVINGTARGS; } set { MOVINGTARGS = value; OnPropertyChanged("MovingTargs"); } }
        private bool FEARINGTARGS = false;
        [DefaultValue(false)]
        public bool FearingTargs { get { return FEARINGTARGS; } set { FEARINGTARGS = value; OnPropertyChanged("FearingTargs"); } }
        private bool ROOTINGTARGS = false;
        [DefaultValue(false)]
        public bool RootingTargs { get { return ROOTINGTARGS; } set { ROOTINGTARGS = value; OnPropertyChanged("RootingTargs"); } }
        private bool SILENCINGTARGS = false;
        [DefaultValue(false)]
        public bool SilencingTargs { get { return SILENCINGTARGS; } set { SILENCINGTARGS = value; OnPropertyChanged("SilencingTargs"); } }
        private bool DISARMINGTARGS = false;
        [DefaultValue(false)]
        public bool DisarmingTargs { get { return DISARMINGTARGS; } set { DISARMINGTARGS = value; OnPropertyChanged("DisarmingTargs"); } }

        private double UNDER35PERC = 0.10d;
        [DefaultValue(0.10d)]
        public double Under35Perc { get { return UNDER35PERC; } set { UNDER35PERC = value; OnPropertyChanged("Under35Perc"); } }
        private double UNDER20PERC = 0.15d;
        [DefaultValue(0.15d)]
        public double Under20Perc { get { return UNDER20PERC; } set { UNDER20PERC = value; OnPropertyChanged("Under20Perc"); } }

        private BossList.FilterType FILTERTYPE = BossList.FilterType.Content;
        [DefaultValue(BossList.FilterType.Content)]
        public BossList.FilterType FilterType { get { return FILTERTYPE; } set { FILTERTYPE = value; OnPropertyChanged("FilterType"); } }
        private string FILTER = "";
        [DefaultValue("")]
        public string Filter { get { return FILTER; } set { FILTER = value; OnPropertyChanged("Filter"); } }
        private string BOSSNAME = "";
        [DefaultValue("")]
        public string BossName { get { return BOSSNAME; } set { BOSSNAME = value; OnPropertyChanged("BossName"); } }
        #endregion
    }

#if !SILVERLIGHT
    [Serializable]
#endif
    public partial class BossHandler {
        public const int NormCharLevel = (int)POSSIBLE_LEVELS.LVLP0;
        public BossHandler() { }
        public BossHandler Clone() {
            BossHandler clone = (BossHandler)this.MemberwiseClone();
            //
            clone.Targets = new List<TargetGroup>(this.Targets);
            clone.Attacks = new List<Attack>(this.Attacks);
            clone.BuffStates = new List<BuffState>(this.BuffStates);
            clone.Moves = new List<Impedance>(this.Moves);
            clone.Stuns = new List<Impedance>(this.Stuns);
            clone.Fears = new List<Impedance>(this.Fears);
            clone.Roots = new List<Impedance>(this.Roots);
            clone.Silences = new List<Impedance>(this.Silences);
            clone.Disarms = new List<Impedance>(this.Disarms);
            //
            return clone;
        }

        #region Variables
        #region Enums/Convertors
        public readonly static string[] BossTierStrings = new string[] {
          /*"Tier  7.0",
            "Tier  7.5",
            "Tier  8.0",
            "Tier  8.5",
            "Tier  9.0",
            "Tier  9.5",
            "Tier 10.0",
            "Tier 10.5",
            "Tier 10.9",*/
            "Tier 11 10N",
            "Tier 11 25N",
            "Tier 11 10H",
            "Tier 11 25H",
        };
        public enum TierLevels : int { T11_10 = 0, T11_25, T11_10H, T11_25H, } /*T7_0 = 0, T7_5, T8_0, T8_5, T9_0, T9_5, T10_0, T10_5, T10_9,*/
        public static readonly float[] StandardMeleePerHit = new float[] {
               /*5000f*2f, //T7_0,
                10000f*2f, //T7_5,
                20000f*2f, //T8_0,
                30000f*2f, //T8_5,
                40000f*2f, //T9_0,
                50000f*2f, //T9_5,
                60000f*2f, //T10_0,
                70000f*2f, //T10_5,
                80000f*2f, //T10_9,*/
               100000f*1.25f, //T11_10,
               120000f*1.25f, //T11_25,
               135000f*1.25f, //T11_10H,
               150000f*1.25f, //T11_25H,
        };
        #endregion
        #region ==== Info ====
        // Name
        /// <summary>The Boss's Name, like "Patchwerk"</summary>
        [DefaultValue("Generic")]
        public string Name { get { return _name; } set { _name = value; OnPropertyChanged("Name"); } }
        private string _name = "Generic";
        // Content
        /// <summary>The Boss's Tier Content Level, T10, T11, etc</summary>
        [DefaultValue(TierLevels.T11_10)]
        public TierLevels Content { get { return _content; } set { _content = value; OnPropertyChanged("Content"); } }
        private TierLevels _content = TierLevels.T11_10;
        public string ContentString { get { return GetContentString(_content); } }
        protected string GetContentString(TierLevels c) { return BossTierStrings[(int)c]; }
        // Instance
        /// <summary>The Instance of this Boss, like "Naxxramas"</summary>
        [DefaultValue("None")]
        public string Instance { get { return _instance; } set { _instance = value; OnPropertyChanged("Instance"); } }
        private string _instance = "None";
        // Comment
        [DefaultValue("No comments have been written for this Boss.")]
        public string Comment { get { return _comment; } set { _comment = value; OnPropertyChanged("Comment"); } }
        private string _comment = "No comments have been written for this Boss.";
        #endregion
        #region ==== Basics ====
        [DefaultValue(1000000f)]
        public float Health { get { return _health; } set { _health = value; OnPropertyChanged("Health"); } }
        private float _health = 1000000f;
        [DefaultValue(10 * 60)]
        public int BerserkTimer { get { return _berserkTimer; } set { _berserkTimer = value; OnPropertyChanged("BerserkTimer"); } }
        private int _berserkTimer = 10 * 60;
        [DefaultValue(3 * 60)]
        public int SpeedKillTimer { get { return _speedKillTimer; } set { _speedKillTimer = value; OnPropertyChanged("SpeedKillTimer"); } }
        private int _speedKillTimer = 3 * 60;
        [DefaultValue(88)]
        public int Level { get { return _level; } set { _level = value; OnPropertyChanged("Level"); } }
        private int _level = (int)POSSIBLE_LEVELS.LVLP3;
        [DefaultValue(11977f)]//(int)StatConversion.NPC_ARMOR[3])]
        public int Armor { get { return _armor; } set { _armor = value; OnPropertyChanged("Armor"); } }
        private int _armor = (int)StatConversion.NPC_ARMOR[3];
        [DefaultValue((int)MOB_TYPES.HUMANOID)]
        public int MobType { get { return _mobtype; } set { _mobtype = value; OnPropertyChanged("MobType"); } }
        private int _mobtype = (int)MOB_TYPES.HUMANOID;
        /// <summary>Example values: 5, 10, 25, 40</summary>
        [DefaultValue(10)]
        public int Max_Players { get { return _maxPlayers; } set { _maxPlayers = value; OnPropertyChanged("Max_Players"); } }
        private int _maxPlayers = 10;
        [DefaultValue(3)]
        public int Min_Healers { get { return _minHealers; } set { _minHealers = value; OnPropertyChanged("Min_Healers"); } }
        private int _minHealers = 3;
        [DefaultValue(2)]
        public int Min_Tanks { get { return _minTanks; } set { _minTanks = value; OnPropertyChanged("Min_Tanks"); } }
        private int _minTanks = 2;
        [DefaultValue(0.00d)]
        public double InBackPerc_Melee { get { return _inBackPerc_Melee; } set { _inBackPerc_Melee = CPd(value); OnPropertyChanged("InBackPerc_Melee"); } }
        private double _inBackPerc_Melee = 0.00d;
        [DefaultValue(0.00d)]
        public double InBackPerc_Ranged { get { return _inBackPerc_Ranged; } set { _inBackPerc_Ranged = CPd(value); OnPropertyChanged("InBackPerc_Ranged"); } }
        private double _inBackPerc_Ranged = 0.00d;
        #endregion
        #region ==== Offensive ====
        public List<TargetGroup> Targets = new List<TargetGroup>();
        private List<Attack> ATTACKS = new List<Attack>();
        public List<BuffState> BuffStates = new List<BuffState>();
        #endregion
        #region ==== Defensive ====
        [DefaultValue(0)]
        public double Resist_Physical { get { return _resistance_physical; } set { _resistance_physical = value; OnPropertyChanged("Resist_Physical"); } }
        private double _resistance_physical = 0;
        [DefaultValue(0)]
        public double Resist_Frost { get { return _resistance_frost; } set { _resistance_frost = value; OnPropertyChanged("Resist_Frost"); } }
        private double _resistance_frost = 0;
        [DefaultValue(0)]
        public double Resist_Fire { get { return _resistance_fire; } set { _resistance_fire = value; OnPropertyChanged("Resist_Fire"); } }
        private double _resistance_fire = 0;
        [DefaultValue(0)]
        public double Resist_Nature { get { return _resistance_nature; } set { _resistance_nature = value; OnPropertyChanged("Resist_Nature"); } }
        private double _resistance_nature = 0;
        [DefaultValue(0)]
        public double Resist_Arcane { get { return _resistance_arcane; } set { _resistance_arcane = value; OnPropertyChanged("Resist_Arcane"); } }
        private double _resistance_arcane = 0;
        [DefaultValue(0)]
        public double Resist_Shadow { get { return _resistance_shadow; } set { _resistance_shadow = value; OnPropertyChanged("Resist_Shadow"); } }
        private double _resistance_shadow = 0;
        [DefaultValue(0)]
        public double Resist_Holy { get { return _resistance_holy; } set { _resistance_holy = value; OnPropertyChanged("Resist_Holy"); } }
        private double _resistance_holy = 0;
        /// <summary>A handler for Boss Damage Taken Reduction due to Resistance (Physical, Fire, etc). </summary>
        /// <returns>The Percentage of Damage to be removed (0.25 = 25% Damage Reduced, 100 Damage should become 75)</returns>
        public double Resistance(ItemDamageType type) {
            switch (type) {
                case ItemDamageType.Physical: return _resistance_physical;
                case ItemDamageType.Nature:   return _resistance_nature;
                case ItemDamageType.Arcane:   return _resistance_arcane;
                case ItemDamageType.Frost:    return _resistance_frost;
                case ItemDamageType.Fire:     return _resistance_fire;
                case ItemDamageType.Shadow:   return _resistance_shadow;
                case ItemDamageType.Holy:     return _resistance_holy;
                default: break;
            }
            return 0f;
        }
        /// <summary>A handler for Boss Damage Taken Reduction due to Resistance (Physical, Fire, etc). This is the Set function</summary>
        /// <returns>The Percentage of Damage to be removed (0.25 = 25% Damage Reduced, 100 Damage should become 75)</returns>
        public double Resistance(ItemDamageType type, float newValue) {
            switch (type) {
                case ItemDamageType.Physical: return Resist_Physical = newValue;
                case ItemDamageType.Frost:    return Resist_Frost    = newValue;
                case ItemDamageType.Fire:     return Resist_Fire     = newValue;
                case ItemDamageType.Nature:   return Resist_Nature   = newValue;
                case ItemDamageType.Arcane:   return Resist_Arcane   = newValue;
                case ItemDamageType.Shadow:   return Resist_Shadow   = newValue;
                case ItemDamageType.Holy:     return Resist_Holy     = newValue;
                default: break;
            }
            return 0f;
        }
        #endregion
        #region ==== Impedances ====
        public List<Impedance> Moves = new List<Impedance>();
        public List<Impedance> Stuns = new List<Impedance>();
        public List<Impedance> Fears = new List<Impedance>();
        public List<Impedance> Roots = new List<Impedance>();
        public List<Impedance> Silences = new List<Impedance>();
        public List<Impedance> Disarms = new List<Impedance>();
        // Other
        [DefaultValue(0)]
        public float TimeBossIsInvuln { get { return _timeBossIsInvulnerable; } set { _timeBossIsInvulnerable = value; OnPropertyChanged("TimeBossIsInvuln"); } }
        private float _timeBossIsInvulnerable = 0;
        #endregion
        #endregion

        #region Get/Set
        #region ==== Offensive ====
        #region Multiple Targets
        public TargetGroup DynamicCompiler_MultiTargs {
            get {
                // Make one
                TargetGroup retVal = new TargetGroup();
                // Find the averaged _____
                float time = MultiTargsTime;
                float dur = MultiTargsDur;
                float acts = time / (dur / 1000f);
                float freq = BerserkTimer / acts;
                float chance = MultiTargsChance;
                double num = MultiTargsNum;
                bool near = MultiTargsNear;
                // Mark those into the retVal
                retVal.Frequency = freq;
                retVal.Duration = dur;
                retVal.Chance = chance;
                retVal.NearBoss = near;
                retVal.NumTargs = (float)num;
                // Double-check we aren't sending a bad one
                if (retVal.Frequency <= 0f || retVal.Chance <= 0f) {
                    retVal.Frequency = -1f; // if we are, use this as a flag
                }
                // Return results
                return retVal;
            }
        }
        public float MultiTargsFreq {
            get {
                if (Targets.Count > 0) {
                    // Adds up the total number of Moves and evens them out over the Berserk Timer
                    float numMultiTargsOverDur = 0;
                    foreach (TargetGroup s in Targets) {
                        numMultiTargsOverDur += (BerserkTimer / s.Frequency) * s.Chance;
                    }
                    float freq = BerserkTimer / numMultiTargsOverDur;
                    return freq;
                } else { return 0; }
            }
        }
        public float MultiTargsDur {
            get {
                if (Targets.Count > 0) {
                    // Averages out the MultiTarg Durations
                    float TotalMultiTargDur = 0;
                    foreach (TargetGroup s in Targets) { TotalMultiTargDur += s.Duration; }
                    float dur = TotalMultiTargDur / Targets.Count;
                    return dur;
                } else { return 0; }
            }
        }
        public double MultiTargsNum {
            get {
                if (Targets.Count > 0) {
                    // Averages out the MultiTarg Amounts
                    float TotalMultiTargAmt = 0;
                    foreach (TargetGroup s in Targets) { TotalMultiTargAmt += s.NumTargs; }
                    float dur = TotalMultiTargAmt / Targets.Count;
                    return dur;
                } else { return 0; }
            }
        }
        public float MultiTargsChance {
            get {
                if (Targets.Count > 0) {
                    // Averages out the MultiTarg Chances
                    float TotalChance = 0f;
                    foreach (TargetGroup s in Targets) { TotalChance += s.Chance; }
                    float chance = TotalChance / (float)Targets.Count;
                    return chance;
                } else { return 0; }
            }
        }
        public float MultiTargsTime {
            get {
                float time = 0f;
                float freq = MultiTargsFreq;
                float dur = MultiTargsDur;
                if (freq > 0f && freq < BerserkTimer) {
                    time = (BerserkTimer / freq) * (dur / 1000f);
                }
                return time;
            }
        }
        public bool MultiTargsNear {
            get {
                if (Targets.Count > 0) {
                    // Averages out the Move Durations
                    int countYes = 0;
                    int countNo = 0;
                    foreach (TargetGroup s in Targets) {
                        if (s.NearBoss) { countYes++; } else { countNo++; };
                    }
                    return countYes >= countNo;
                } else { return false; }
            }
        }
        public string DynamicString_MultiTargs {
            get {
                string retVal = "";
                //
                if (Targets.Count > 0) {
                    foreach (TargetGroup i in Targets) {
                        retVal += i.ToString() + "\n";
                    }
                } else {
                    retVal = "No Target Groups other than Boss.";
                }
                //
                return retVal.Trim('\n');
            }
        }
        #endregion
        #region Attacks
        // ==== Attacks ====
        public List<Attack> Attacks { get { return ATTACKS; } set { ATTACKS = value; } }
        public string DynamicString_Attacks {
            get {
                string retVal = "";
                //
                if (Attacks.Count > 0) {
                    foreach (Attack i in Attacks) {
                        retVal += i.ToString() + "\n";
                    }
                } else {
                    retVal = "No Attacks.";
                }
                //
                return retVal.Trim('\n');
            }
        }
        public Attack DynamicCompiler_Attacks
        {
            get
            {
                // Make one
                Attack retVal = new Attack()
                {
                    // Basics
                    Name = "Dynamic",
                    DamageType = ItemDamageType.Physical,
                    DamagePerHit = StandardMeleePerHit[(int)TierLevels.T11_10],
                    DamageIsPerc = false,
                    MaxNumTargets = 1,
                    AttackSpeed = 2.0f,
                    AttackType = ATTACK_TYPES.AT_MELEE,
                    Interruptable = false,
                    // Player Avoidances
                    Missable = true,
                    Dodgable = true,
                    Parryable = true,
                    Blockable = true,
                };
                if (Attacks.Count <= 0) { retVal.AttackSpeed = -1; return retVal; }
                // Find the averaged _____
                int numTargs = 0;
                float speeds = 0;
                float dph = 0;
                float SumSpeedInverse = 0;
                foreach(Attack a in Attacks){
                    dph += a.DamagePerHit;
                    numTargs += (int)a.MaxNumTargets;
                    speeds += (int)a.AttackSpeed;
                    SumSpeedInverse += 1/a.AttackSpeed;
                }
                // Mark those into the retVal
                retVal.DamagePerHit = dph / (float)Attacks.Count;
                retVal.MaxNumTargets = (int)((float)numTargs / (float)Attacks.Count);
                retVal.AttackSpeed = 1 / (SumSpeedInverse);
                // Double-check we aren't sending a bad one
                if (retVal.AttackSpeed <= 0f)
                {
                    retVal.AttackSpeed = -1f; // if we are, use this as a flag
                }
                // Return results
                return retVal;
            }
        }
        public Attack DynamicCompiler_FilteredAttacks(List<Attack> atks)
        {
            // Make one
            Attack retVal = new Attack()
            {
                // Basics
                Name = "DynamicFiltered",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = StandardMeleePerHit[(int)TierLevels.T11_10],
                DamageIsPerc = false,
                MaxNumTargets = 1,
                AttackSpeed = 2.0f,
                AttackType = ATTACK_TYPES.AT_MELEE,
                Interruptable = false,
                // Player Avoidances
                Missable = true,
                Dodgable = true,
                Parryable = true,
                Blockable = true,
            };
            if (atks.Count <= 0) { retVal.AttackSpeed = -1; return retVal; }
            // Find the averaged _____
            int numTargs = 0;
            float speeds = 0;
            float dph = 0;
            foreach (Attack a in atks)
            {
                dph += a.DamagePerHit;
                numTargs += (int)a.MaxNumTargets;
                speeds += (int)a.AttackSpeed;
            }
            // Mark those into the retVal
            retVal.DamagePerHit = dph / (float)atks.Count;
            retVal.MaxNumTargets = (int)((float)numTargs / (float)atks.Count);
            retVal.AttackSpeed = (int)(speeds / (float)atks.Count);
            // Double-check we aren't sending a bad one
            if (retVal.AttackSpeed <= 0f)
            {
                retVal.AttackSpeed = -1f; // if we are, use this as a flag
            }
            // Return results
            return retVal;
        }
        // ==== Methods for Pulling Boss DPS ==========
        public List<Attack> GetFilteredAttackList(ATTACK_TYPES type, bool includeDoTs = false) {
            //List<Attack> attacks = new List<Attack>();
            //if (Attacks.Count <= 0) { return attacks; } // make sure there were some TO put in there
            //foreach (Attack a in Attacks) { if (a.AttackType == type) { attacks.Add(a); } }
            //return attacks;
            return Attacks.FindAll(attack => attack.AttackType == type);
        }
        public List<Attack> GetFilteredAttackList(ItemDamageType type) {
            //List<Attack> attacks = new List<Attack>();
            //if (Attacks.Count <= 0) { return attacks; } // make sure there were some TO put in there
            //foreach (Attack a in Attacks) { if (a.DamageType == type) { attacks.Add(a); } }
            //return attacks;
            return Attacks.FindAll(attack => attack.DamageType == type);
        }
        public List<Attack> GetFilteredAttackList(Type type) {
            //List<Attack> attacks = new List<Attack>();
            //if (Attacks.Count <= 0) { return attacks; } // make sure there were some TO put in there
            //foreach (Attack a in Attacks) { if (a.DamageType == type) { attacks.Add(a); } }
            //return attacks;
            return Attacks.FindAll(attack => (attack.GetType() == type));
        }
        /// <summary>Public function for the DPS Gets so we can re-use code. Includes a full player defend table.</summary>
        /// <param name="type">The type of attack to check: AT_MELEE, AT_RANGED, AT_AOE</param>
        /// <param name="BossDamageBonus">Perc value (0.10f = 110% Base damage)</param>
        /// <param name="BossDamagePenalty">Perc value (0.10f = 90% Base damage)</param>
        /// <param name="p_missPerc">Perc value (0.08f = 8% Chance for Boss to Miss Player)</param>
        /// <param name="p_dodgePerc">Perc value (0.201f = 20.10% Chance for Player to Dodge Boss Attack)</param>
        /// <param name="p_parryPerc">Perc value (0.1375f = 13.75% Chance for Player to Parry Boss Attack)</param>
        /// <param name="p_blockPerc">Perc value (0.065f = 6.5% Chance for Player to Block Boss Attack)</param>
        /// <param name="p_blockVal">How much Damage is absorbed by player's Shield in Block Value</param>
        /// <returns>The DPS value requested, returns zero if no Attacks have been created for the Boss or there are no Attacks of that Type.</returns>
        public float GetDPSByType(ATTACK_TYPES type, float BossDamageBonus, float BossDamagePenalty,
                                  float p_missPerc, float p_dodgePerc, float p_parryPerc, float p_blockPerc, float p_blockVal)
        {
            List<Attack> attacks = GetFilteredAttackList(type);
            if (attacks.Count <= 0) { return 0f; } // make sure there were some put in there

            float retDPS = 0f;
            // Coded as template for dealing w/ different attack types.
#if false
            foreach (Attack a in attacks)
            {
                float damage, damageOnUse, swing, acts, avgDmg, dps;
                if (true/*Melee*/)
                {
                    damage = a.DamagePerHit * (1f + BossDamageBonus) * (1f - BossDamagePenalty);
                    damageOnUse = damage * (1f - p_missPerc - p_dodgePerc - p_parryPerc - p_blockPerc); // takes out the player's defend table
                    swing = a.AttackSpeed;
                    damageOnUse += (damage - p_blockVal) * p_blockPerc; // Adds reduced damage from blocks back in
                    acts = BerserkTimer / swing;
                    avgDmg = damageOnUse * acts;
                    dps = avgDmg / BerserkTimer;
                }
                else if (true/*Spell*/)
                {
                    // This needs to be rewritten for spell resist tables
                    damage = a.DamagePerHit * (1f + BossDamageBonus) * (1f - BossDamagePenalty);
                    damageOnUse = damage * (1f - p_missPerc - p_dodgePerc - p_parryPerc - p_blockPerc); // takes out the player's defend table
                    swing = a.AttackSpeed;
                    damageOnUse += (damage - p_blockVal) * p_blockPerc; // Adds reduced damage from blocks back in
                    acts = BerserkTimer / swing;
                    avgDmg = damageOnUse * acts;
                    dps = avgDmg / BerserkTimer;
                }
                else if (true/*Bleed*/)
                {
                    // This needs to be rewritten that you cant avoid DoT ticks
                    damage = a.DamagePerHit * (1f + BossDamageBonus) * (1f - BossDamagePenalty);
                    damageOnUse = damage * (1f - p_missPerc - p_dodgePerc - p_parryPerc - p_blockPerc); // takes out the player's defend table
                    swing = a.AttackSpeed;
                    damageOnUse += (damage - p_blockVal) * p_blockPerc; // Adds reduced damage from blocks back in
                    acts = BerserkTimer / swing;
                    avgDmg = damageOnUse * acts;
                    dps = avgDmg / BerserkTimer;
                }
                retDPS += dps;
            }
#endif
            foreach (Attack a in attacks) 
            {
                float damage, damageOnUse, swing, acts, avgDmg, dps;
                if (true/*Melee*/) // Currently all decision paths are using this.  
                {
                    // This works for physical attacks that actually trigger the attack/defend table.
                    damage = a.DamagePerHit * (1f + BossDamageBonus) * (1f - BossDamagePenalty);
                    swing = a.AttackSpeed;
                    damageOnUse = damage;
                    if (a.Dodgable) damageOnUse *= (1f - p_dodgePerc); // takes out the player's defend table
                    if (a.Parryable) damageOnUse *= (1f - p_parryPerc); // takes out the player's defend table
                    if (a.Missable) damageOnUse *= (1f - p_missPerc); // takes out the player's defend table
                    if (a.Blockable) damageOnUse += (damage - p_blockVal) * p_blockPerc; // Adds reduced damage from blocks back in
                    acts = BerserkTimer / swing;
                    avgDmg = damageOnUse * acts;
                    dps = avgDmg / BerserkTimer;
                    retDPS += dps;
                }
                //else if (true/*Spell*/) {  }
                //else if (true/*Bleed*/) {  }
            }
            return retDPS;
        }

        /// <summary>Public function for the DPS Gets so we can re-use code. Includes a full player defend table.</summary>
        /// <param name="type">The type of attack to check: AT_MELEE, AT_RANGED, AT_AOE</param>
        /// <param name="BossDamageBonus">Perc value (0.10f = 110% Base damage)</param>
        /// <param name="BossDamagePenalty">Perc value (0.10f = 90% Base damage)</param>
        /// <param name="BossAttackSpeedAcceleration">Perc value (0.10f = 110% Base attack speed)</param>
        /// <param name="BossAttackSpeedReduction">Perc value (0.10f = 90% Base attack speed)</param>
        /// <param name="p_missPerc">Perc value (0.08f = 8% Chance for Boss to Miss Player)</param>
        /// <param name="p_dodgePerc">Perc value (0.201f = 20.10% Chance for Player to Dodge Boss Attack)</param>
        /// <param name="p_parryPerc">Perc value (0.1375f = 13.75% Chance for Player to Parry Boss Attack)</param>
        /// <param name="p_blockPerc">Perc value (0.065f = 6.5% Chance for Player to Block Boss Attack)</param>
        /// <param name="p_blockVal">How much Damage is absorbed by player's Shield in Block Value</param>
        /// <param name="p_ArcaneResist">Perc value (0.08f = 8% Avg Arcane Resistance)</param>
        /// <param name="p_FireResist">Perc value (0.201f = 20.10% Avg Fire Resistance)</param>
        /// <param name="p_FrostResist">Perc value (0.1375f = 13.75% Avg Frost Resistance)</param>
        /// <param name="p_NatureResist">Perc value (0.065f = 6.5% Avg Nature Resistance)</param>
        /// <param name="p_ShadowResist">Perc value (0.065f = 6.5% Avg Shadow Resistance)</param>
        /// <returns>The DPS value requested, returns zero if no Attacks have been created for the Boss or there are no Attacks of that Type.</returns>
        public float GetDPSByType(ATTACK_TYPES type, float BossDamageBonus, float BossDamagePenalty, float BossAttackSpeedAcceleration, float BossAttackSpeedReduction,
                                  float p_missPerc, float p_dodgePerc, float p_parryPerc, float p_blockPerc, float p_blockVal,
                                  float p_ArcaneResist, float p_FireResist, float p_FrostResist, float p_NatureResist, float p_ShadowResist)
        {
            List<Attack> attacks = GetFilteredAttackList(type);
            if (attacks.Count <= 0) { return 0f; } // make sure there were some put in there

            float retDPS = 0f;
            foreach (Attack a in attacks)
            {
                float damage, damageOnUse, swing, acts, avgDmg, dps;
                damage = a.DamagePerHit * (1f + BossDamageBonus) * (1f - BossDamagePenalty);
                swing = a.AttackSpeed * (1f - BossAttackSpeedAcceleration) * (1f + BossAttackSpeedReduction);
                damageOnUse = damage;
                switch (a.DamageType)
                {
                    case ItemDamageType.Physical:
                        {
                            // This works for physical attacks that actually trigger the attack/defend table.
                            if (a.Dodgable) damageOnUse *= (1f - p_dodgePerc); // takes out the player's defend table
                            if (a.Parryable) damageOnUse *= (1f - p_parryPerc); // takes out the player's defend table
                            if (a.Missable) damageOnUse *= (1f - p_missPerc); // takes out the player's defend table
                            if (a.Blockable) damageOnUse += (damage - p_blockVal) * p_blockPerc; // Adds reduced damage from blocks back in
                            break;
                        }
                    case ItemDamageType.Arcane:
                        {
                            damageOnUse *= (1f - p_ArcaneResist); break;
                        }
                    case ItemDamageType.Fire:
                        {
                            damageOnUse *= (1f - p_FireResist); break;
                        }
                    case ItemDamageType.Frost:
                        {
                            damageOnUse *= (1f - p_FrostResist); break;
                        }
                    case ItemDamageType.Nature:
                        {
                            damageOnUse *= (1f - p_NatureResist); break;
                        }
                    case ItemDamageType.Shadow:
                        {
                            damageOnUse *= (1f - p_ShadowResist); break;
                        }
                }
                acts = BerserkTimer / swing;
                avgDmg = damageOnUse * acts;
                dps = avgDmg / BerserkTimer;
                retDPS += dps;

            }
            return retDPS;
        }

        /// <summary>Public function for the DPS Gets so we can re-use code.</summary>
        /// <param name="type">The type of attack to check: AT_MELEE, AT_RANGED, AT_AOE</param>
        /// <param name="BossDamageBonus">Perc value (0.10f = 110% Base damage)</param>
        /// <param name="BossDamagePenalty">Perc value (0.10f = 90% Base damage)</param>
        /// <param name="p_missPerc">Perc value (0.08f = 8% Chance for Boss to Miss Player)</param>
        /// <returns>The DPS value requested, returns zero if no Attacks have been created for the Boss or there are no Attacks of that Type.</returns>
        public float GetDPSByType(ATTACK_TYPES type, float BossDamageBonus, float BossDamagePenalty, float p_missPerc) {
            return GetDPSByType(type, BossDamageBonus, BossDamagePenalty, p_missPerc, 0, 0, 0, 0);
        }
        /// <summary>Public function for the DPS Gets so we can re-use code.</summary>
        /// <param name="type">The type of attack to check: AT_MELEE, AT_RANGED, AT_AOE</param>
        /// <param name="BossDamageBonus">Perc value (0.10f = 110% Base damage)</param>
        /// <param name="BossDamagePenalty">Perc value (0.10f = 90% Base damage)</param>
        /// <returns>The DPS value requested, returns zero if no Attacks have been created for the Boss or there are no Attacks of that Type.</returns>
        public float GetDPSByType(ATTACK_TYPES type, float BossDamageBonus, float BossDamagePenalty) {
            return GetDPSByType(type, BossDamageBonus, BossDamagePenalty, 0, 0, 0, 0, 0);
        }
        /// <summary>
        /// Gets Raw DPS of all attacks that are Melee type. DPS and Healing characters should not normally see this damage.
        /// Tanks will receive this damage.
        /// </summary>
        public float DPS_SingleTarg_Melee { get { float dps = GetDPSByType(ATTACK_TYPES.AT_MELEE, 0, 0); return dps; } }
        /// <summary>
        /// Gets Raw DPS of all attacks that are Ranged type. DPS and Healing characters will use this
        /// to determine incoming damage to Raid, on specific targets. Tanks will receive this damage in
        /// addition to the Melee single-target under chance methods.
        /// </summary>
        public float DPS_SingleTarg_Ranged { get { float dps = GetDPSByType(ATTACK_TYPES.AT_RANGED, 0, 0, 0); return dps; } }
        /// <summary>
        /// Gets Raw DPS of all attacks that are AoE type. DPS and Healing characters will use this
        /// to determine incoming damage to Raid. Tanks will receive this damage in addition to the
        /// Melee single-target.
        /// </summary>
        public float DPS_AoE { get { float dps = GetDPSByType(ATTACK_TYPES.AT_AOE, 0, 0); return dps; } }
        /// <summary>
        /// Iterates though the attack list to find the Default Melee Attack.
        /// <para>If the Attack list is empty, this will return null</para>
        /// <para>If the Attack list doesn't have an attack listed as the Default Melee Attack, this will return null.</para>
        /// </summary>
        public Attack DefaultMeleeAttack {
            get
            {
                // There are no attacks
                if (Attacks.Count <= 0) { return null; }
                // Iterating the list to find it
                foreach (Attack a in Attacks) {
                    if (a.IsTheDefaultMelee) {
                        return a;
                    }
                }
                // We reached the end without finding it
                return null;
                
            }
        }
        // AoE Targets
        public float  AoETargsFreq  {
            get {
                List<Attack> attacks = GetFilteredAttackList(ATTACK_TYPES.AT_AOE);
                if (attacks.Count > 0) {
                    // Adds up the total number of AoEs and evens them out over the Berserk Timer
                    float numAoEsOverDur = 0;
                    foreach (Attack s in attacks) {
                        numAoEsOverDur += BerserkTimer / s.AttackSpeed;
                    }
                    float freq = BerserkTimer / numAoEsOverDur;
                    return freq;
                } else {
                    return BerserkTimer;
                }
            }
        }
        public float  AoETargsDMG   {
            get {
                List<Attack> attacks = GetFilteredAttackList(ATTACK_TYPES.AT_AOE);
                if (attacks.Count > 0) {
                    // Averages out the Root Durations
                    float TotalaAoEDmg = 0;
                    foreach (Attack s in attacks) { TotalaAoEDmg += s.DamagePerHit; }
                    float dmg = TotalaAoEDmg / attacks.Count;
                    return dmg;
                } else {
                    return 1500f;
                }
            }
        }
        #endregion
        #region Buff States
        public BuffState DynamicCompiler_BuffStates {
            get {
                // Make one
                BuffState retVal = new BuffState();
                // Find the averaged _____
                float time = BuffStatesTime;
                float dur = BuffStatesDur;
                float acts = time / (dur / 1000f);
                float freq = BerserkTimer / acts;
                float chance = BuffStatesChance;
                Stats stats = BuffStatesStats;
                // Mark those into the retVal
                retVal.Name = "Dynamic";
                retVal.Frequency = freq;
                retVal.Duration = dur;
                retVal.Chance = chance;
                retVal.Stats = stats; // having no stats to come back is not a reason to invalidate.
                // Double-check we aren't sending a bad one
                if (retVal.Frequency <= 0f || retVal.Chance <= 0f) {
                    retVal.Frequency = -1f; // if we are, use this as a flag
                }
                // Return results
                return retVal;
            }
        }
        public string DynamicString_BuffStates {
            get {
                string retVal = "";
                //
                if (BuffStates.Count > 0) {
                    foreach (BuffState i in BuffStates) {
                        retVal += i.ToString() + "\n";
                    }
                } else {
                    retVal = "No Buff States.";
                }
                //
                return retVal.Trim('\n');
            }
        }
        public float BuffStatesFreq {
            get {
                if (BuffStates.Count > 0) {
                    // Adds up the total number of Moves and evens them out over the Berserk Timer
                    float numBuffStatesOverDur = 0;
                    foreach (BuffState s in BuffStates) {
                        numBuffStatesOverDur += (BerserkTimer / s.Frequency) * s.Chance;
                    }
                    float freq = BerserkTimer / numBuffStatesOverDur;
                    return freq;
                } else { return 0; }
            }
        }
        public float BuffStatesDur {
            get {
                if (BuffStates.Count > 0) {
                    // Averages out the MultiTarg Durations
                    float TotalMultiTargDur = 0;
                    foreach (BuffState s in BuffStates) { TotalMultiTargDur += s.Duration; }
                    float dur = TotalMultiTargDur / BuffStates.Count;
                    return dur;
                } else { return 0; }
            }
        }
        public float BuffStatesChance {
            get {
                if (BuffStates.Count > 0) {
                    // Averages out the MultiTarg Chances
                    float TotalChance = 0f;
                    foreach (BuffState s in BuffStates) { TotalChance += s.Chance; }
                    float chance = TotalChance / (float)BuffStates.Count;
                    return chance;
                } else { return 0; }
            }
        }
        public float BuffStatesTime {
            get {
                float time = 0f;
                float freq = BuffStatesFreq;
                float dur = BuffStatesDur;
                if (freq > 0f && freq < BerserkTimer) {
                    time = (BerserkTimer / freq) * (dur / 1000f);
                }
                return time;
            }
        }
        public Stats BuffStatesStats {
            get {
                if (BuffStates.Count > 0) {
                    // having no stats to come back is not a reason to invalidate.
                    Stats stats = new Stats();
                    foreach (BuffState s in BuffStates) { if (s.Stats != null) { stats.Accumulate(s.Stats, s.Chance); } }
                    return stats;
                } else { return new Stats(); }
            }
        }
        #endregion
        #endregion
        #region ==== Impedances ====
        protected Impedance DynamicCompiler(List<Impedance> imps) {
            //if (imps == null || imps.Count <= 0) return null;
            // Make one
            Impedance retVal = new Impedance() {
                Frequency = 20f,
                Duration = 1f * 1000f,
                Chance = 1.00f,
                Breakable = true,
            };
            // Find the averaged _____
            float time = Time(imps);
            float dur = Dur(imps);
            float acts = time / (dur / 1000f);
            float freq = BerserkTimer / acts;
            float chance = Chance(imps);
            // Mark those into the retVal
            retVal.Frequency = freq;
            retVal.Duration = dur;
            retVal.Chance = chance;
            // Double-check we aren't sending a bad one
            if (retVal.Frequency <= 0f || retVal.Chance <= 0f)
            {
                retVal.Frequency = -1f; // if we are, use this as a flag
            }
            // Return results
            return retVal;
        }
        protected float Freq(List<Impedance> imps) {
            if (imps == null || imps.Count < 1) return -1;
            // Adds up the total number of impedences
            // and evens them out over the Berserk Timer
            float numImpsOverDur = 0f;
            foreach (Impedance imp in imps) {
                numImpsOverDur += (BerserkTimer / imp.Frequency) * imp.Chance;
            }
            return BerserkTimer / numImpsOverDur;
        }
        protected float Time(List<Impedance> imps) {
            if (imps == null || imps.Count < 1) return 0;
            float time = 0f;
            float freq = Freq(imps);
            float dur = Dur(imps) / 1000f;
            if (freq > 0f && freq < BerserkTimer) {
                time = (BerserkTimer / freq) * dur;
            } else if (freq >= BerserkTimer) {
                time = dur;
            }
            return time;
        }
        protected float Dur(List<Impedance> imps) {
            if (imps == null || imps.Count < 1) return 0;
            // Averages out the Move Durations
            float TotalDur = 0;
            foreach (Impedance s in imps) { TotalDur += s.Duration; }
            return TotalDur / (float)imps.Count;
        }
        protected float Chance(List<Impedance> imps) {
            if (imps == null || imps.Count < 1) return 0;
            // Averages out the Chances
            float TotalChance = 0f;
            foreach (Impedance s in imps) { TotalChance += s.Chance; }
            return TotalChance / (float)imps.Count;
        }
        // Moving Targets
        public Impedance DynamicCompiler_Move
        {
            get {
                // Make one
                Impedance retVal = new Impedance() {
                    Frequency = 20f,
                    Duration = 1f * 1000f,
                    Chance = 1.00f,
                    Breakable = true,
                };
                // Find the averaged _____
                float time = MovingTargsTime;
                float dur = MovingTargsDur;
                float acts = time / (dur / 1000f);
                float freq = BerserkTimer / acts;
                float chance = MovingTargsChance;
                // Mark those into the retVal
                retVal.Frequency = freq;
                retVal.Duration = dur;
                retVal.Chance = chance;
                // Double-check we aren't sending a bad one
                if (retVal.Frequency <= 0f || retVal.Chance <= 0f) {
                    retVal.Frequency = -1f; // if we are, use this as a flag
                }
                // Return results
                return retVal;
            }
        }
        public string DynamicString_Move {
            get {
                string retVal = "";
                //
                if (Moves.Count > 0) {
                    foreach (Impedance i in Moves) {
                        retVal += i.ToString() + "\n";
                    }
                } else {
                    retVal = "No Movement.";
                }
                //
                return retVal.Trim('\n');
            }
        }
        public float MovingTargsFreq      { get { return Moves.Count > 0 ? Freq(Moves) : -1; } }
        public float MovingTargsDur       { get { return Moves.Count > 0 ? Dur(Moves) : 0; } }
        public float MovingTargsChance    { get { return Moves.Count > 0 ? Chance(Moves) : 0; } }
        public float MovingTargsTime      { get { return Moves.Count > 0 ? Time(Moves) : 0; } }
        // Stunning Targets
        public Impedance DynamicCompiler_Stun {
            get {
                // Make one
                Impedance retVal = new Impedance() {
                    Frequency = 20f,
                    Duration = 1f * 1000f,
                    Chance = 1.00f,
                    Breakable = true,
                };
                // Find the averaged _____
                float time = StunningTargsTime;
                float dur = StunningTargsDur;
                float acts = time / (dur / 1000f);
                float freq = BerserkTimer / acts;
                float chance = StunningTargsChance;
                // Mark those into the retVal
                retVal.Frequency = freq;
                retVal.Duration = dur;
                retVal.Chance = chance;
                // Double-check we aren't sending a bad one
                if (retVal.Frequency <= 0f || retVal.Chance <= 0f) {
                    retVal.Frequency = -1f; // if we are, use this as a flag
                }
                // Return results
                return retVal;
            }
        }
        public string DynamicString_Stun {
            get {
                string retVal = "";
                //
                if (Stuns.Count > 0) {
                    foreach (Impedance i in Stuns) {
                        retVal += i.ToString() + "\n";
                    }
                } else {
                    retVal = "No Stuns.";
                }
                //
                return retVal.Trim('\n');
            }
        }
        public float StunningTargsFreq    { get { return Stuns.Count > 0 ? Freq(Stuns) : -1; } }
        public float StunningTargsDur     { get { return Stuns.Count > 0 ? Dur(Stuns) : 0; } }
        public float StunningTargsChance  { get { return Stuns.Count > 0 ? Chance(Stuns) : 0; } }
        public float StunningTargsTime    { get { return Stuns.Count > 0 ? Time(Stuns) : 0; } }
        // Fearing Targets
        public Impedance DynamicCompiler_Fear
        {
            get {
                // Make one
                Impedance retVal = new Impedance() {
                    Frequency = 20f,
                    Duration = 1f * 1000f,
                    Chance = 1.00f,
                    Breakable = true,
                };
                // Find the averaged _____
                float time = FearingTargsTime;
                float dur = FearingTargsDur;
                float acts = time / (dur / 1000f);
                float freq = BerserkTimer / acts;
                float chance = FearingTargsChance;
                // Mark those into the retVal
                retVal.Frequency = freq;
                retVal.Duration = dur;
                retVal.Chance = chance;
                // Double-check we aren't sending a bad one
                if (retVal.Frequency <= 0f || retVal.Chance <= 0f) {
                    retVal.Frequency = -1f; // if we are, use this as a flag
                }
                // Return results
                return retVal;
            }
        }
        public string DynamicString_Fear {
            get {
                string retVal = "";
                //
                if (Fears.Count > 0) {
                    foreach (Impedance i in Fears) {
                        retVal += i.ToString() + "\n";
                    }
                } else {
                    retVal = "No Fears.";
                }
                //
                return retVal.Trim('\n');
            }
        }
        public float FearingTargsFreq     { get { return Fears.Count > 0 ? Freq(Fears) : -1; } }
        public float FearingTargsDur      { get { return Fears.Count > 0 ? Dur(Fears) : 0; } }
        public float FearingTargsChance   { get { return Fears.Count > 0 ? Chance(Fears) : 0; } }
        public float FearingTargsTime     { get { return Fears.Count > 0 ? Time(Fears) : 0; } }
        // Rooting Targets
        public Impedance DynamicCompiler_Root
        {
            get {
                // Make one
                Impedance retVal = new Impedance()
                {
                    Frequency = 20f,
                    Duration = 1f * 1000f,
                    Chance = 1.00f,
                    Breakable = true,
                };
                // Find the averaged _____
                float time = RootingTargsTime;
                float dur = RootingTargsDur;
                float acts = time / (dur / 1000f);
                float freq = BerserkTimer / acts;
                float chance = RootingTargsChance;
                // Mark those into the retVal
                retVal.Frequency = freq;
                retVal.Duration = dur;
                retVal.Chance = chance;
                // Double-check we aren't sending a bad one
                if (retVal.Frequency <= 0f || retVal.Chance <= 0f) {
                    retVal.Frequency = -1f; // if we are, use this as a flag
                }
                // Return results
                return retVal;
            }
        }
        public string DynamicString_Root {
            get {
                string retVal = "";
                //
                if (Roots.Count > 0) {
                    foreach (Impedance i in Roots) {
                        retVal += i.ToString() + "\n";
                    }
                } else {
                    retVal = "No Roots.";
                }
                //
                return retVal.Trim('\n');
            }
        }
        public float RootingTargsFreq     { get { return Roots.Count > 0 ? Freq(Roots) : -1; } }
        public float RootingTargsDur      { get { return Roots.Count > 0 ? Dur(Roots) : 0; } }
        public float RootingTargsChance   { get { return Roots.Count > 0 ? Chance(Roots) : 0; } }
        public float RootingTargsTime     { get { return Roots.Count > 0 ? Time(Roots) : 0; } }
        // Silencing Targets
        public Impedance DynamicCompiler_Slnc
        {
            get {
                // Make one
                Impedance retVal = new Impedance()
                {
                    Frequency = 20f,
                    Duration = 1f * 1000f,
                    Chance = 1.00f,
                    Breakable = true,
                };
                // Find the averaged _____
                float time = SilencingTargsTime;
                float dur = SilencingTargsDur;
                float acts = time / (dur / 1000f);
                float freq = BerserkTimer / acts;
                float chance = SilencingTargsChance;
                // Mark those into the retVal
                retVal.Frequency = freq;
                retVal.Duration = dur;
                retVal.Chance = chance;
                // Double-check we aren't sending a bad one
                if (retVal.Frequency <= 0f || retVal.Chance <= 0f) {
                    retVal.Frequency = -1f; // if we are, use this as a flag
                }
                // Return results
                return retVal;
            }
        }
        public string DynamicString_Slnc {
            get {
                string retVal = "";
                //
                if (Silences.Count > 0) {
                    foreach (Impedance i in Silences) {
                        retVal += i.ToString() + "\n";
                    }
                } else {
                    retVal = "No Silences.";
                }
                //
                return retVal.Trim('\n');
            }
        }
        public float SilencingTargsFreq   { get { return Silences.Count > 0 ? Freq(Silences) : -1; } }
        public float SilencingTargsDur    { get { return Silences.Count > 0 ? Dur(Silences) : 0; } }
        public float SilencingTargsChance { get { return Silences.Count > 0 ? Chance(Silences) : 0; } }
        public float SilencingTargsTime   { get { return Silences.Count > 0 ? Time(Silences) : 0; } }
        // Disarming Targets
        public Impedance DynamicCompiler_Dsrm
        {
            get {
                // Make one
                Impedance retVal = new Impedance() {
                    Frequency = 20f,
                    Duration = 1f * 1000f,
                    Chance = 1.00f,
                    Breakable = true,
                };
                // Find the averaged _____
                float time = DisarmingTargsTime;
                float dur = DisarmingTargsDur;
                float acts = time / (dur / 1000f);
                float freq = BerserkTimer / acts;
                float chance = DisarmingTargsChance;
                // Mark those into the retVal
                retVal.Frequency = freq;
                retVal.Duration = dur;
                retVal.Chance = chance;
                // Double-check we aren't sending a bad one
                if (retVal.Frequency <= 0f || retVal.Chance <= 0f) {
                    retVal.Frequency = -1f; // if we are, use this as a flag
                }
                // Return results
                return retVal;
            }
        }
        public string DynamicString_Dsrm {
            get {
                string retVal = "";
                //
                if (Disarms.Count > 0) {
                    foreach (Impedance i in Disarms) {
                        retVal += i.ToString() + "\n";
                    }
                } else {
                    retVal = "No Disarms.";
                }
                //
                return retVal.Trim('\n');
            }
        }
        public float DisarmingTargsFreq   { get { return Disarms.Count > 0 ? Freq(Disarms) : -1; } }
        public float DisarmingTargsDur    { get { return Disarms.Count > 0 ? Dur(Disarms) : 0; } }
        public float DisarmingTargsChance { get { return Disarms.Count > 0 ? Chance(Disarms) : 0; } }
        public float DisarmingTargsTime   { get { return Disarms.Count > 0 ? Time(Disarms) : 0; } }
        #endregion
        #endregion

        #region Functions
        /// <summary>Constrain Percent (float)</summary>
        /// <param name="value">Value to be constrained</param>
        /// <returns>value if it is between 0% and 100% or those limits</returns>
        public static float CPf(float value) { return Math.Max(0f, Math.Min(1f, value)); }
        /// <summary>Constrain Percent (double)</summary>
        /// <param name="value">Value to be constrained</param>
        /// <returns>value if it is between 0% and 100% or those limits</returns>
        public static double CPd(double value) { return Math.Max(0d, Math.Min(1d, value)); }

        public static float CalcADotTick(float mindmg, float maxdmg, float time, float interval) {
            return ((mindmg + maxdmg) / 2.0f) * time / interval;
        }
        public static float CalcADotTick(float mindmg, float maxdmg, float time) {
            return ((mindmg + maxdmg) / 2.0f) * time;
        }

        /// <summary>
        /// Generates a Fight Info description listing the stats of the fight as well as any comments listed for the boss
        /// </summary>
        /// <param name="BossDamageBonus">Perc value (0.10f = 110% Base damage)</param>
        /// <param name="BossDamagePenalty">Perc value (0.10f = 90% Base damage)</param>
        /// <param name="p_missPerc">Perc value (0.08f = 8% Chance for Boss to Miss Player)</param>
        /// <param name="p_dodgePerc">Perc value (0.201f = 20.10% Chance for Player to Dodge Boss Attack)</param>
        /// <param name="p_parryPerc">Perc value (0.1375f = 13.75% Chance for Player to Parry Boss Attack)</param>
        /// <param name="p_blockPerc">Perc value (0.065f = 6.5% Chance for Player to Block Boss Attack)</param>
        /// <param name="p_blockVal">How much Damage is absorbed by player's Shield in Block Value</param>
        /// <returns>The generated string</returns>
        public string GenInfoString(float BossDamageBonus, float BossDamagePenalty,
                                  float p_missPerc, float p_dodgePerc, float p_parryPerc, float p_blockPerc, float p_blockVal)
        {
            int room = Max_Players - Min_Healers - Min_Tanks;
            float TotalDPSNeeded = Health / (BerserkTimer - TimeBossIsInvuln - MovingTargsTime);
            float dpsdps = TotalDPSNeeded * ((float)room / ((float)Min_Tanks / 2f + (float)room)) / (float)room;
            float tankdps = (TotalDPSNeeded * ((float)Min_Tanks / ((float)Min_Tanks / 2f + (float)room)) / (float)Min_Tanks) / 2f;
            string retVal = "";
            //
            /*retVal += string.Format("Name: {0}\r\nContent: {1}\r\nInstance: {2}\r\nHealth: {3:#,##0}\r\n",
                Name, ContentString.Replace("  "," "), Instance, Health);*/
            //
            /*retVal += string.Format("Enrage Timer: {0:mm} min {0:ss} sec\r\nSpeed Kill Timer: {1:mm} min {1:ss} sec\r\n\r\n",
                TimeSpan.FromSeconds(BerserkTimer), TimeSpan.FromSeconds(SpeedKillTimer));*/
            //
            /*retVal += string.Format("Raid Setup: {0:0} man ({1:0} Tanks {2:0} Healers {3:0} DPS)\r\n\r\n",
                Max_Players, Min_Tanks, Min_Healers, room);*/
            //
            retVal += string.Format("To beat the Enrage Timer you need {0:#,##0} Total DPS\r\n"
                + "{1:#,##0} from each Tank {2:#,##0} from each DPS\r\n\r\n",
                TotalDPSNeeded, tankdps, dpsdps);
            //
            TotalDPSNeeded = Health / (SpeedKillTimer - TimeBossIsInvuln - SpeedKillTimer * (MovingTargsTime / BerserkTimer));
            dpsdps = TotalDPSNeeded * ((float)room / ((float)Min_Tanks / 2f + (float)room)) / (float)room;
            tankdps = (TotalDPSNeeded * ((float)Min_Tanks / ((float)Min_Tanks / 2f + (float)room)) / (float)Min_Tanks) / 2f;
            //
            retVal += string.Format("To beat the Speed Kill Timer you need {0:#,##0} Total DPS\r\n"
                + "{1:#,##0} from each Tank {2:#,##0} from each DPS\r\n\r\n",
                TotalDPSNeeded, tankdps, dpsdps);
            //
            retVal += string.Format("This boss does the following Damage Per Second Amounts, factoring Armor, Resistance and Defend Tables where applicable:\r\n"
                + "Single Target Melee: {0:0.0}\r\n"
                + "Single Target Ranged: {1:0.0}\r\n"
                + "Raid AoE: {2:0.0}\r\n\r\n",
                GetDPSByType(ATTACK_TYPES.AT_MELEE, BossDamageBonus, BossDamagePenalty, p_missPerc, p_dodgePerc, p_parryPerc, p_blockPerc, p_blockVal),
                GetDPSByType(ATTACK_TYPES.AT_RANGED, BossDamageBonus, BossDamagePenalty, p_missPerc),
                GetDPSByType(ATTACK_TYPES.AT_AOE, BossDamageBonus, 0));
            //
            retVal += string.Format("Comment(s):\r\n{0}", Comment);
            //
            return retVal;
        }
        /// <summary>
        /// Generates a Fight Info description listing the stats of the fight as well as any comments listed for the boss
        /// </summary>
        /// <returns>The generated string</returns>
        public string GenInfoString() { return GenInfoString(0,0,0,0,0,0,0); }
        public string GenInfoString(Character character) {
            if (character == null || character.CurrentCalculations == null) return GenInfoString();
            Stats stats = character.CurrentCalculations.GetCharacterStats(character);
            return GenInfoString(stats.DamageTakenMultiplier, 0f,
                0f/*stats.Defense * StatConversion.DEFENSE_RATING_AVOIDANCE_MULTIPLIER*/,
                stats.Dodge, stats.Parry, stats.Block, 0f/*stats.BlockValue*/);
        }
        #endregion

        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(property));
        }
        #endregion

        public override string ToString()
        {
            return Content.ToString() + " : " + Instance + " : " + Name;
        }

        #region Statics
        private static Attack _ADefaultMeleeAttack = null;
        public static Attack ADefaultMeleeAttack {
            get {
                if (_ADefaultMeleeAttack == null)
                {
                    _ADefaultMeleeAttack = new Attack()
                    {
                        // Enforced
                        IsTheDefaultMelee = true,
                        // Basics
                        Name = "Generated Default Melee Attack",
                        DamageType = ItemDamageType.Physical,
                        DamagePerHit = StandardMeleePerHit[(int)TierLevels.T11_10],
                        DamageIsPerc = false,
                        MaxNumTargets = 1f,
                        AttackSpeed = 2.0f,
                        AttackType = ATTACK_TYPES.AT_MELEE,
                        Interruptable = false,
                        // Player Avoidances
                        Missable = true,
                        Dodgable = true,
                        Parryable = true,
                        Blockable = true,
                    };
                    _ADefaultMeleeAttack.AffectsRole[PLAYER_ROLES.MainTank]
                        = _ADefaultMeleeAttack.AffectsRole[PLAYER_ROLES.OffTank]
                        = _ADefaultMeleeAttack.AffectsRole[PLAYER_ROLES.TertiaryTank]
                        = true;
                }
                return _ADefaultMeleeAttack;
            }
        }
        #endregion
    }

    public class MultiDiffBoss : List<BossHandler>
    {
        public MultiDiffBoss()
        {
            // Initialize
            //this = new List<BossHandler>() { };
            this.Add(new BossHandler());
            this.Add(new BossHandler());
            this.Add(new BossHandler());
            this.Add(new BossHandler());
            // Basic Setups we don't want to repeat over and over again
            Content = new BossHandler.TierLevels[] { BossHandler.TierLevels.T11_10, BossHandler.TierLevels.T11_25, BossHandler.TierLevels.T11_10H, BossHandler.TierLevels.T11_25H };
            // Fight Requirements
            Min_Tanks = new int[] { 2, 2, 2, 2 };
            Min_Healers = new int[] { 2, 5, 2, 5 };
        }
        #region Variable Convenience Overrides
        // Info
        public string Name
        {
            get { return this[0].Name; }
            set
            {
                this[0].Name = value;
                this[1].Name = value;
                this[2].Name = value;
                this[3].Name = value;
            }
        }
        public string Instance
        {
            get { return this[0].Instance; }
            set
            {
                this[0].Instance = value;
                this[1].Instance = value;
                this[2].Instance = value;
                this[3].Instance = value;
            }
        }
        public BossHandler.TierLevels[] Content
        {
            get
            {
                return new BossHandler.TierLevels[] {
                    this[0].Content,
                    this[1].Content,
                    this[2].Content,
                    this[3].Content,
                };
            }
            set
            {
                int i = 0;
                this[i].Content = value[i]; i++;
                this[i].Content = value[i]; i++;
                this[i].Content = value[i]; i++;
                this[i].Content = value[i];
            }
        }
        public string Comment
        {
            get { return this[0].Comment; }
            set
            {
                this[0].Comment = value;
                this[1].Comment = value;
                this[2].Comment = value;
                this[3].Comment = value;
            }
        }
        // Basics
        public float[] Health
        {
            get
            {
                return new float[] {
                    this[0].Health,
                    this[1].Health,
                    this[2].Health,
                    this[3].Health,
                };
            }
            set
            {
                int i = 0;
                this[i].Health = value[i]; i++;
                this[i].Health = value[i]; i++;
                this[i].Health = value[i]; i++;
                this[i].Health = value[i];
            }
        }
        public int[] BerserkTimer
        {
            get
            {
                return new int[] {
                    this[0].BerserkTimer,
                    this[1].BerserkTimer,
                    this[2].BerserkTimer,
                    this[3].BerserkTimer,
                };
            }
            set
            {
                int i = 0;
                this[i].BerserkTimer = value[i]; i++;
                this[i].BerserkTimer = value[i]; i++;
                this[i].BerserkTimer = value[i]; i++;
                this[i].BerserkTimer = value[i];
            }
        }
        public int[] SpeedKillTimer
        {
            get
            {
                return new int[] {
                    this[0].SpeedKillTimer,
                    this[1].SpeedKillTimer,
                    this[2].SpeedKillTimer,
                    this[3].SpeedKillTimer,
                };
            }
            set
            {
                int i = 0;
                this[i].SpeedKillTimer = value[i]; i++;
                this[i].SpeedKillTimer = value[i]; i++;
                this[i].SpeedKillTimer = value[i]; i++;
                this[i].SpeedKillTimer = value[i];
            }
        }
        public double[] InBackPerc_Melee
        {
            get
            {
                return new double[] {
                    this[0].InBackPerc_Melee,
                    this[1].InBackPerc_Melee,
                    this[2].InBackPerc_Melee,
                    this[3].InBackPerc_Melee,
                };
            }
            set
            {
                int i = 0;
                this[i].InBackPerc_Melee = value[i]; i++;
                this[i].InBackPerc_Melee = value[i]; i++;
                this[i].InBackPerc_Melee = value[i]; i++;
                this[i].InBackPerc_Melee = value[i];
            }
        }
        public double[] InBackPerc_Ranged
        {
            get
            {
                return new double[] {
                    this[0].InBackPerc_Ranged,
                    this[1].InBackPerc_Ranged,
                    this[2].InBackPerc_Ranged,
                    this[3].InBackPerc_Ranged,
                };
            }
            set
            {
                int i = 0;
                this[i].InBackPerc_Ranged = value[i]; i++;
                this[i].InBackPerc_Ranged = value[i]; i++;
                this[i].InBackPerc_Ranged = value[i]; i++;
                this[i].InBackPerc_Ranged = value[i];
            }
        }
        public int[] Max_Players
        {
            get
            {
                return new int[] {
                    this[0].Max_Players,
                    this[1].Max_Players,
                    this[2].Max_Players,
                    this[3].Max_Players,
                };
            }
            set
            {
                int i = 0;
                this[i].Max_Players = value[i]; i++;
                this[i].Max_Players = value[i]; i++;
                this[i].Max_Players = value[i]; i++;
                this[i].Max_Players = value[i];
            }
        }
        public int[] Min_Tanks
        {
            get
            {
                return new int[] {
                    this[0].Min_Tanks,
                    this[1].Min_Tanks,
                    this[2].Min_Tanks,
                    this[3].Min_Tanks,
                };
            }
            set
            {
                int i = 0;
                this[i].Min_Tanks = value[i]; i++;
                this[i].Min_Tanks = value[i]; i++;
                this[i].Min_Tanks = value[i]; i++;
                this[i].Min_Tanks = value[i];
            }
        }
        public int[] Min_Healers
        {
            get
            {
                return new int[] {
                    this[0].Min_Healers,
                    this[1].Min_Healers,
                    this[2].Min_Healers,
                    this[3].Min_Healers,
                };
            }
            set
            {
                int i = 0;
                this[i].Min_Healers = value[i]; i++;
                this[i].Min_Healers = value[i]; i++;
                this[i].Min_Healers = value[i]; i++;
                this[i].Min_Healers = value[i];
            }
        }
        // Offensive
        // Defensive
        public double[] Resist_Physical
        {
            get
            {
                return new double[] {
                    this[0].Resist_Physical,
                    this[1].Resist_Physical,
                    this[2].Resist_Physical,
                    this[3].Resist_Physical,
                };
            }
            set
            {
                int i = 0;
                this[i].Resist_Physical = value[i]; i++;
                this[i].Resist_Physical = value[i]; i++;
                this[i].Resist_Physical = value[i]; i++;
                this[i].Resist_Physical = value[i];
            }
        }
        public double[] Resist_Frost
        {
            get
            {
                return new double[] {
                    this[0].Resist_Frost,
                    this[1].Resist_Frost,
                    this[2].Resist_Frost,
                    this[3].Resist_Frost,
                };
            }
            set
            {
                int i = 0;
                this[i].Resist_Frost = value[i]; i++;
                this[i].Resist_Frost = value[i]; i++;
                this[i].Resist_Frost = value[i]; i++;
                this[i].Resist_Frost = value[i];
            }
        }
        public double[] Resist_Fire
        {
            get
            {
                return new double[] {
                    this[0].Resist_Fire,
                    this[1].Resist_Fire,
                    this[2].Resist_Fire,
                    this[3].Resist_Fire,
                };
            }
            set
            {
                int i = 0;
                this[i].Resist_Fire = value[i]; i++;
                this[i].Resist_Fire = value[i]; i++;
                this[i].Resist_Fire = value[i]; i++;
                this[i].Resist_Fire = value[i];
            }
        }
        public double[] Resist_Nature
        {
            get
            {
                return new double[] {
                    this[0].Resist_Nature,
                    this[1].Resist_Nature,
                    this[2].Resist_Nature,
                    this[3].Resist_Nature,
                };
            }
            set
            {
                int i = 0;
                this[i].Resist_Nature = value[i]; i++;
                this[i].Resist_Nature = value[i]; i++;
                this[i].Resist_Nature = value[i]; i++;
                this[i].Resist_Nature = value[i];
            }
        }
        public double[] Resist_Arcane
        {
            get
            {
                return new double[] {
                    this[0].Resist_Arcane,
                    this[1].Resist_Arcane,
                    this[2].Resist_Arcane,
                    this[3].Resist_Arcane,
                };
            }
            set
            {
                int i = 0;
                this[i].Resist_Arcane = value[i]; i++;
                this[i].Resist_Arcane = value[i]; i++;
                this[i].Resist_Arcane = value[i]; i++;
                this[i].Resist_Arcane = value[i];
            }
        }
        public double[] Resist_Shadow
        {
            get
            {
                return new double[] {
                    this[0].Resist_Shadow,
                    this[1].Resist_Shadow,
                    this[2].Resist_Shadow,
                    this[3].Resist_Shadow,
                };
            }
            set
            {
                int i = 0;
                this[i].Resist_Shadow = value[i]; i++;
                this[i].Resist_Shadow = value[i]; i++;
                this[i].Resist_Shadow = value[i]; i++;
                this[i].Resist_Shadow = value[i];
            }
        }
        public double[] Resist_Holy
        {
            get
            {
                return new double[] {
                    this[0].Resist_Holy,
                    this[1].Resist_Holy,
                    this[2].Resist_Holy,
                    this[3].Resist_Holy,
                };
            }
            set
            {
                int i = 0;
                this[i].Resist_Holy = value[i]; i++;
                this[i].Resist_Holy = value[i]; i++;
                this[i].Resist_Holy = value[i]; i++;
                this[i].Resist_Holy = value[i];
            }
        }
        // Impedances
        public float[] TimeBossIsInvuln
        {
            get
            {
                return new float[] {
                    this[0].TimeBossIsInvuln,
                    this[1].TimeBossIsInvuln,
                    this[2].TimeBossIsInvuln,
                    this[3].TimeBossIsInvuln,
                };
            }
            set
            {
                int i = 0;
                this[i].TimeBossIsInvuln = value[i]; i++;
                this[i].TimeBossIsInvuln = value[i]; i++;
                this[i].TimeBossIsInvuln = value[i]; i++;
                this[i].TimeBossIsInvuln = value[i];
            }
        }
        // Methods
        public Attack GenAStandardMelee(BossHandler.TierLevels content) {
            Attack retVal = new Attack {
                Name = "Melee",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = BossHandler.StandardMeleePerHit[(int)content],
                MaxNumTargets = 1f,
                AttackSpeed = 2.0f,
                AttackType = ATTACK_TYPES.AT_MELEE,
                IsTheDefaultMelee = true,
            };
            retVal.AffectsRole[PLAYER_ROLES.MainTank]
                = retVal.AffectsRole[PLAYER_ROLES.OffTank]
                = retVal.AffectsRole[PLAYER_ROLES.TertiaryTank]
                = true;
            return retVal;
        }
        #endregion
    }
}
