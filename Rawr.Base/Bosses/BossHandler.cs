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
    #region Subclasses
    /// <summary>Enumerator for creating a list of possible values for the Level box</summary>
    public enum POSSIBLE_LEVELS { LVLP0 = 80, LVLP1, LVLP2, LVLP3, }
    /// <summary>
    /// Enumerator for attack types, this mostly is for raid members that aren't
    /// being directly attacked to know when AoE damage is coming from the boss
    /// </summary>
    public enum ATTACK_TYPES { AT_MELEE = 0, AT_RANGED, AT_AOE, }
    /// <summary>A single Attack of various types</summary>
    public partial class Attack {
        /// <summary>The Name of the Attack</summary>
        public string Name;
        /// <summary>The type of damage done, use the ItemDamageType enumerator to select</summary>
        public ItemDamageType DamageType;
        /// <summary>This is so you can pull the Default Melee Attack more easily</summary>
        public bool IsTheDefaultMelee = false;
        /// <summary>The Unmitigated Damage per Hit for this attack, 5000f is 5,000 Raw Unmitigated Damage. When DamageIsPerc is true DamagePerHit = 0.75f; would be 75% of Player's Health</summary>
        public virtual float DamagePerHit { get; set; }
        /// <summary>When set to True, DamagePerHit will be seen as a Percentage. DamagePerHit = 0.75f; would be 75% of Player's Health</summary>
        public bool DamageIsPerc = false;
        /// <summary>The maximum number of party/raid members this attack can hit</summary>
        public float MaxNumTargets;
        /// <summary>The frequency of this attack (in seconds)</summary>
        public float AttackSpeed;
        /// <summary>The Attack Type (for AoE vs single-target Melee/Ranged)</summary>
        public ATTACK_TYPES AttackType;
        public bool UseParryHaste = false;
        #region Player Avoidance
        /// <summary>Returns True if any of the Avoidance types are true</summary>
        public bool Avoidable { get { return Missable || Dodgable || Parryable || Blockable; } }
        /// <summary>Can this attack Miss the player</summary>
        public bool Missable = true;
        /// <summary>Can this attack be Dodged by the player</summary>
        public bool Dodgable = true;
        /// <summary>Can this attack be Parried by the player</summary>
        public bool Parryable = true;
        /// <summary>Can this attack be Blocked by the player</summary>
        public bool Blockable = true;
        #endregion
        #region Player Targeting
        /// <summary>Returns True if anyone gets ignored, serves as a primary flag to run the ignorance system in your model</summary>
        public bool IgnoresSomeone { get { return IgnoresSomeTanks || IgnoresSomeDPS || IgnoresHealers; } }
        // Tanks
        public bool IgnoresAllTanks { get { return IgnoresMTank && IgnoresOTank && IgnoresTTank; } }
        public bool IgnoresSomeTanks { get { return IgnoresMTank || IgnoresOTank || IgnoresTTank; } }
        public bool IgnoresMTank = false;
        public bool IgnoresOTank = false;
        public bool IgnoresTTank = false;
        // Heals
        public bool IgnoresHealers = false;
        // DPS
        public bool IgnoresAllDPS { get { return IgnoresMeleeDPS && IgnoresRangedDPS; } }
        public bool IgnoresSomeDPS { get { return IgnoresMeleeDPS || IgnoresRangedDPS; } }
        public bool IgnoresMeleeDPS = false;
        public bool IgnoresRangedDPS = false;
        #endregion
        #region Player Negation
        public bool Interruptable = false;
        #endregion

        public override string ToString()
        {
            if (AttackSpeed <= 0) { return "None"; }
            return string.Format("Spd: {0:0.0#}sec D: {1:0}{2} #T: {3:0} {4}{5}",
                AttackSpeed,
                DamagePerHit, DamageIsPerc ? "%" : "",
                MaxNumTargets,
                UseParryHaste ? ": PH" : "",
                Name != "Dynamic" ? ": " + Name : "");
        }
    }
    public partial class DoT : Attack {
        /// <summary>The initial damage of the dot, separate from the over time portion</summary>
        public override float DamagePerHit { get; set; }
        /// <summary>The over time damage of the dot, separate from the initial portion</summary>
        public float DamagePerTick;
        /// <summary>The total number of Ticks</summary>
        public float NumTicks;
        /// <summary>Interval of the ticks, 1 = 1 sec</summary>
        public float TickInterval;
        /// <summary>The full duration of the DoT, 2 sec Interval * 5 Ticks = 10 sec duration</summary>
        public float Duration { get { return TickInterval * NumTicks; } }
    }
    public class Impedance {
        #region Constructors
        public Impedance() {
            Frequency = 120f;
            Duration  = 5000f;
            Chance    = 1.00f;
            Breakable = true;
        }
        public Impedance(float f, float d, float c, bool b) {
            Frequency = f;
            Duration  = d;
            Chance    = c;
            Breakable = b;
        }
        public Impedance(Impedance i) {
            Impedance clone = (Impedance)i.MemberwiseClone();
            Frequency = clone.Frequency;
            Duration  = clone.Duration;
            Chance    = clone.Chance;
            Breakable = clone.Breakable;
        }
        #endregion
        #region Variables
        public float Frequency;
        public float Duration;
        public float Chance;
        public bool Breakable;
        #endregion
        #region Functions
        public override string ToString() {
            string retVal = "";
            if (Frequency < 0) return "None";
            retVal += "F: " + Frequency.ToString("0") + "s";
            retVal += " D: " + Duration.ToString("0") + "ms";
            retVal += " C: " + Chance.ToString("0.0%");
            retVal += Breakable ? " : B" : "";
            return retVal;
        }
        #endregion
    }
    /// <summary>
    /// The role of the player, will allow certain lists to return filtered to
    /// things that affect said role
    /// </summary>
    public enum PLAYER_ROLES { ROLE_MainTank=0, ROLE_OffTank, ROLE_TertiaryTank, ROLE_MeleeDPS, ROLE_RangedDPS, ROLE_Healer }
    #endregion

#if !RAWR3 && !SILVERLIGHT
    [Serializable]
#endif
    public partial class BossOptions : BossHandler
    {
        public BossOptions() { }
        public BossOptions Clone() {
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
            this.Version = clone.Version;
            this.Comment = clone.Comment;
            // Basics
            this.Level = clone.Level;
            this.Armor = clone.Armor;
            this.Health = clone.Health;
            this.BerserkTimer = clone.BerserkTimer;
            this.SpeedKillTimer = clone.SpeedKillTimer;
            this.InBackPerc_Melee = clone.InBackPerc_Melee;
            this.InBackPerc_Ranged = clone.InBackPerc_Ranged;
            this.InBack = (this.InBackPerc_Melee + this.InBackPerc_Ranged) > 0f;
            this.Max_Players = clone.Max_Players;
            this.Min_Healers = clone.Min_Healers;
            this.Min_Tanks = clone.Min_Tanks;
            // Offensive
            this.MaxNumTargets = clone.MaxNumTargets;
            this.MultiTargsPerc = clone.MultiTargsPerc;
            this.MultiTargs = (this.MultiTargsPerc > 0f && this.MaxNumTargets > 1f);
            this.DoTs = clone.DoTs;
            this.Attacks = clone.Attacks;
            this.DamagingTargs = (Attacks != null && Attacks.Count > 0);
            // Defensive
            this.Resist_Physical = clone.Resist_Physical;
            this.Resist_Frost = clone.Resist_Physical;
            this.Resist_Fire = clone.Resist_Physical;
            this.Resist_Nature = clone.Resist_Nature;
            this.Resist_Arcane = clone.Resist_Arcane;
            this.Resist_Shadow = clone.Resist_Shadow;
            this.Resist_Holy = clone.Resist_Holy;
            // Impedances
            this.Stuns = clone.Stuns; this.StunningTargs = this.Stuns != null && this.Stuns.Count > 0;
            this.Moves = clone.Moves; this.MovingTargs = this.Moves != null && this.Moves.Count > 0;
            this.Fears = clone.Fears; this.FearingTargs = this.Fears != null && this.Fears.Count > 0;
            this.Roots = clone.Roots; this.RootingTargs = this.Roots != null && this.Roots.Count > 0;
            this.Disarms = clone.Disarms; this.DisarmingTargs = this.Disarms != null && this.Disarms.Count > 0;
            this.TimeBossIsInvuln = clone.TimeBossIsInvuln;
            //
            return this;
        }
        #region Variables
        private bool INBACK = false;
        public bool InBack { get { return INBACK; } set { INBACK = value; OnPropertyChanged("InBack"); } }

        private bool MULTITARGS = false;
        public bool MultiTargs { get { return MULTITARGS; } set { MULTITARGS = value; OnPropertyChanged("MultiTargs"); } }

        private bool STUNNINGTARGS = false;
        public bool StunningTargs { get { return STUNNINGTARGS; } set { STUNNINGTARGS = value; OnPropertyChanged("StunningTargs"); } }
        private bool MOVINGTARGS = false;
        public bool MovingTargs { get { return MOVINGTARGS; } set { MOVINGTARGS = value; OnPropertyChanged("MovingTargs"); } }
        private bool FEARINGTARGS = false;
        public bool FearingTargs { get { return FEARINGTARGS; } set { FEARINGTARGS = value; OnPropertyChanged("FearingTargs"); } }
        private bool ROOTINGTARGS = false;
        public bool RootingTargs { get { return ROOTINGTARGS; } set { ROOTINGTARGS = value; OnPropertyChanged("RootingTargs"); } }
        private bool DISARMINGTARGS = false;
        public bool DisarmingTargs { get { return DISARMINGTARGS; } set { DISARMINGTARGS = value; OnPropertyChanged("DisarmingTargs"); } }
        private bool DAMAGINGTARGS = false;
        public bool DamagingTargs { get { return DAMAGINGTARGS; } set { DAMAGINGTARGS = value; OnPropertyChanged("DamagingTargs"); } }

        private double UNDER35PERC = 0.10d;
        public double Under35Perc { get { return UNDER35PERC; } set { UNDER35PERC = value; OnPropertyChanged("Under35Perc"); } }
        private double UNDER20PERC = 0.15d;
        public double Under20Perc { get { return UNDER20PERC; } set { UNDER20PERC = value; OnPropertyChanged("Under20Perc"); } }

        private BossList.FilterType FILTERTYPE = BossList.FilterType.Content;
        public BossList.FilterType FilterType { get { return FILTERTYPE; } set { FILTERTYPE = value; OnPropertyChanged("FilterType"); } }
        private string FILTER = "";
        public string Filter { get { return FILTER; } set { FILTER = value; OnPropertyChanged("Filter"); } }
        private string BOSSNAME = "";
        public string BossName { get { return BOSSNAME; } set { BOSSNAME = value; OnPropertyChanged("BossName"); } }
        #endregion
    }

#if !RAWR3 && !SILVERLIGHT
    [Serializable]
#endif
    public partial class BossHandler {
        public const int NormCharLevel = 80;
        public BossHandler() { }
        public BossHandler Clone() {
            BossHandler clone = (BossHandler)this.MemberwiseClone();
            //
            //clone.Attacks.Clear(); clone.Attacks.AddRange((Attack[])clone.Attacks.ToArray().Clone());
            //clone.Moves.Clear(); clone.Moves.AddRange((Impedence[])clone.Moves.ToArray().Clone());
            //clone.Stuns.Clear(); clone.Stuns.AddRange((Impedence[])clone.Stuns.ToArray().Clone());
            //clone.Fears.Clear(); clone.Fears.AddRange((Fear[])clone.Fears.ToArray().Clone());
            //clone.Roots.Clear(); clone.Roots.AddRange((Root[])clone.Roots.ToArray().Clone());
            //clone.Disarms.Clear(); clone.Disarms.AddRange((Disarm[])clone.Disarms.ToArray().Clone());
            //
            return clone;
        }

        #region Variables
        #region Enums/Convertors
        protected readonly static string[] BossTierStrings = new string[] {
            "Tier 7",
            "Tier 7.5",
            "Tier 8",
            "Tier 8.5",
            "Tier 9",
            "Tier 9.5",
            "Tier 10",
            "Tier 10.5",
            "Tier 10.9",
        };
        protected readonly static string[] BossVersionStrings = new string[] {
            "10 Man",
            "25 Man",
            "10 Man (H)",
            "25 Man (H)",
        };
        public enum Versions   : int { V_10N = 0, V_25N = 1, V_10H = 2, V_25H = 3, V_10 = 0, V_25 = 1 } // last two are for file compatibility between versions
        public enum TierLevels : int { T7_0 = 0, T7_5, T8_0, T8_5, T9_0, T9_5, T10_0, T10_5, T10_9 }
        public static readonly float[] StandardMeleePerHit = new float[] {
              5000f*2f, //T7_0,
             10000f*2f, //T7_5,
             20000f*2f, //T8_0,
             30000f*2f, //T8_5,
             40000f*2f, //T9_0,
             50000f*2f, //T9_5,
             60000f*2f, //T10_0,
             70000f*2f, //T10_5,
             80000f*2f, //T10_9,
        };
        #endregion
        #region ==== Info ====
        private string NAME = "Generic",
            INSTANCE = "None",
            COMMENT = "No comments have been written for this Boss.";
        private TierLevels CONTENT = TierLevels.T7_0;
        private Versions VERSION = Versions.V_10N;
        #endregion
        #region ==== Basics ====
        private float HEALTH = 1000000f;
        private int BERSERKTIMER = 8 * 60,
                    SPEEDKILLTIMER = 3 * 60,
                    LEVEL = (int)POSSIBLE_LEVELS.LVLP3,
                    ARMOR = (int)StatConversion.NPC_ARMOR[3],
                    MAX_PLAYERS = 10,
                    MIN_HEALERS = 3,
                    MIN_TANKS = 2;
        private double INBACKPERC_MELEE = 0.00d,
                       INBACKPERC_RANGED = 0.00d;
        #endregion
        #region ==== Offensive ====
        private double MAXNUMTARGS = 1d;
        private double MULTITARGSPERC = 0.00d;
        /// <summary>WARNING! This variable is not presently used!</summary>
        private List<DoT> DOTS = new List<DoT>();
        private List<Attack> ATTACKS = new List<Attack>();
        #endregion
        #region ==== Defensive ====
        private double RESISTANCE_PHYSICAL = 0,
                       RESISTANCE_FROST = 0,
                       RESISTANCE_FIRE = 0,
                       RESISTANCE_NATURE = 0,
                       RESISTANCE_ARCANE = 0,
                       RESISTANCE_SHADOW = 0,
                       RESISTANCE_HOLY = 0;
        #endregion
        #region ==== Impedances ====
        public List<Impedance> Moves = new List<Impedance>();
        public List<Impedance> Stuns = new List<Impedance>();
        public List<Impedance> Fears = new List<Impedance>();
        public List<Impedance> Roots = new List<Impedance>();
        public List<Impedance> Disarms = new List<Impedance>();
        private float TIMEBOSSISINVULN = 0;
        #endregion
        #endregion

        #region Get/Set
        #region ==== Info ====
        protected string GetVersionString(Versions v) { return BossVersionStrings[(int)v]; }
        protected string GetContentString(TierLevels c) { return BossTierStrings[(int)c]; }
        public string Name               { get { return NAME;               } set { NAME               = value; OnPropertyChanged("Name"              ); } }
        public TierLevels Content        { get { return CONTENT;            } set { CONTENT            = value; OnPropertyChanged("Content"           ); } }
        public string ContentString      { get { return GetContentString(CONTENT); } }
        public string Instance           { get { return INSTANCE;           } set { INSTANCE           = value; OnPropertyChanged("Instance"          ); } }
        public Versions Version          { get { return VERSION;            } set { VERSION            = value; OnPropertyChanged("Version"           ); } }
        public string VersionString      { get { return GetVersionString(VERSION); } }
        public string Comment            { get { return COMMENT;            } set { COMMENT            = value; OnPropertyChanged("Comment"           ); } }
        #endregion
        #region ==== Basics ====
        public int    Level              { get { return LEVEL;              } set { LEVEL              = value; OnPropertyChanged("Level"             ); } }
        public int    Armor              { get { return ARMOR;              } set { ARMOR              = value; OnPropertyChanged("Armor"             ); } }
        public int    BerserkTimer       { get { return BERSERKTIMER;       } set { BERSERKTIMER       = value; OnPropertyChanged("BerserkTimer"      ); } }
        public int    SpeedKillTimer     { get { return SPEEDKILLTIMER;     } set { SPEEDKILLTIMER     = value; OnPropertyChanged("SpeedKillTimer"    ); } }
        public float  Health             { get { return HEALTH;             } set { HEALTH             = value; OnPropertyChanged("Health"            ); } }
        public double  InBackPerc_Melee  { get { return INBACKPERC_MELEE;   } set { INBACKPERC_MELEE   = value; OnPropertyChanged("InBackPerc_Melee"  ); } }
        public double  InBackPerc_Ranged { get { return INBACKPERC_RANGED;  } set { INBACKPERC_RANGED  = value; OnPropertyChanged("InBackPerc_Ranged" ); } }
        /// <summary>Example values: 5, 10, 25, 40</summary>
        public int Max_Players { get { return MAX_PLAYERS; } set { MAX_PLAYERS = value; OnPropertyChanged("Max_Players"); } }
        public int Min_Healers { get { return MIN_HEALERS; } set { MIN_HEALERS = value; OnPropertyChanged("Min_Healers"); } }
        public int Min_Tanks { get { return MIN_TANKS; } set { MIN_TANKS = value; OnPropertyChanged("Min_Tanks"); } }
        #endregion
        #region ==== Offensive ====
        public double MultiTargsPerc     { get { return MULTITARGSPERC;     } set { MULTITARGSPERC     = value; OnPropertyChanged("MultiTargsPerc"    ); } }
        public double  MaxNumTargets      { get { return MAXNUMTARGS;        } set { MAXNUMTARGS        = value; OnPropertyChanged("MaxNumTargs"       ); } }
        // ==== Attacks ====
        public List<DoT> DoTs { get { return DOTS; } set { DOTS = value; } }// not actually used! Dont even try!
        public List<Attack> Attacks { get { return ATTACKS; } set { ATTACKS = value; } }
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
                    DamagePerHit = 80f * 1000f,
                    DamageIsPerc = false,
                    MaxNumTargets = 1,
                    AttackSpeed = 2.0f,
                    AttackType = ATTACK_TYPES.AT_MELEE,
                    UseParryHaste = true,
                    Interruptable = false,
                    // Player Avoidances
                    Missable = true,
                    Dodgable = true,
                    Parryable = true,
                    Blockable = true,
                    // Targetting Ignores
                    IgnoresMTank = false,
                    IgnoresOTank = false,
                    IgnoresTTank = false,
                    IgnoresHealers = false,
                    IgnoresMeleeDPS = false,
                    IgnoresRangedDPS = false,
                };
                if (Attacks.Count <= 0) { retVal.AttackSpeed = -1; return retVal; }
                // Find the averaged _____
                int numTargs = 0;
                float speeds = 0;
                float dph = 0;
                foreach(Attack a in Attacks){
                    dph += a.DamagePerHit;
                    numTargs += (int)a.MaxNumTargets;
                    speeds += (int)a.AttackSpeed;
                }
                // Mark those into the retVal
                retVal.DamagePerHit = dph / (float)Attacks.Count;
                retVal.MaxNumTargets = (int)((float)numTargs / (float)Attacks.Count);
                retVal.AttackSpeed = (int)(speeds / (float)Attacks.Count);
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
                DamagePerHit = 80f * 1000f,
                DamageIsPerc = false,
                MaxNumTargets = 1,
                AttackSpeed = 2.0f,
                AttackType = ATTACK_TYPES.AT_MELEE,
                UseParryHaste = true,
                Interruptable = false,
                // Player Avoidances
                Missable = true,
                Dodgable = true,
                Parryable = true,
                Blockable = true,
                // Targetting Ignores
                IgnoresMTank = false,
                IgnoresOTank = false,
                IgnoresTTank = false,
                IgnoresHealers = false,
                IgnoresMeleeDPS = false,
                IgnoresRangedDPS = false,
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
        public List<Attack> GetFilteredAttackList(ATTACK_TYPES type) {
            List<Attack> attacks = new List<Attack>();
            if (Attacks.Count <= 0) { return attacks; } // make sure there were some TO put in there
            foreach (Attack a in Attacks) { if (a.AttackType == type) { attacks.Add(a); } }
            return attacks;
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

            foreach (Attack a in attacks) {
                float damage = a.DamagePerHit * (1f + BossDamageBonus) * (1f - BossDamagePenalty),
                      damageOnUse = damage * (1f - p_missPerc - p_dodgePerc - p_parryPerc - p_blockPerc), // takes out the player's defend table
                      swing = a.AttackSpeed;
                      damageOnUse += (damage - p_blockVal) * p_blockPerc; // Adds reduced damage from blocks back in
                float acts = BerserkTimer / swing,
                      avgDmg = damageOnUse * acts,
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
        /// Tanks will recieve this damage.
        /// </summary>
        public float DPS_SingleTarg_Melee { get { float dps = GetDPSByType(ATTACK_TYPES.AT_MELEE, 0, 0); return dps; } }
        /// <summary>
        /// Gets Raw DPS of all attacks that are Ranged type. DPS and Healing characters will use this
        /// to determine incoming damage to Raid, on specific targets. Tanks will recieve this damage in
        /// addition to the Melee single-target under chance methods.
        /// </summary>
        public float DPS_SingleTarg_Ranged { get { float dps = GetDPSByType(ATTACK_TYPES.AT_RANGED, 0, 0, 0); return dps; } }
        /// <summary>
        /// Gets Raw DPS of all attacks that are AoE type. DPS and Healing characters will use this
        /// to determine incoming damage to Raid. Tanks will recieve this damage in addition to the
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
                    float dur = TotalaAoEDmg / attacks.Count;
                    return dur;
                } else {
                    return 1500f;
                }
            }
        }
        #endregion
        #region ==== Defensive ====
        public double Resist_Physical { get { return RESISTANCE_PHYSICAL; } set { RESISTANCE_PHYSICAL = value; OnPropertyChanged("Resist_Physical"); } }
        public double Resist_Frost { get { return RESISTANCE_FROST; } set { RESISTANCE_FROST = value; OnPropertyChanged("Resist_Frost"); } }
        public double Resist_Fire { get { return RESISTANCE_FIRE; } set { RESISTANCE_FIRE = value; OnPropertyChanged("Resist_Fire"); } }
        public double Resist_Nature { get { return RESISTANCE_NATURE; } set { RESISTANCE_NATURE = value; OnPropertyChanged("Resist_Nature"); } }
        public double Resist_Arcane { get { return RESISTANCE_ARCANE; } set { RESISTANCE_ARCANE = value; OnPropertyChanged("Resist_Arcane"); } }
        public double Resist_Shadow { get { return RESISTANCE_SHADOW; } set { RESISTANCE_SHADOW = value; OnPropertyChanged("Resist_Shadow"); } }
        public double Resist_Holy { get { return RESISTANCE_HOLY; } set { RESISTANCE_HOLY = value; OnPropertyChanged("Resist_Holy"); } }
        /// <summary>A handler for Boss Damage Taken Reduction due to Resistance (Physical, Fire, etc). </summary>
        /// <returns>The Percentage of Damage to be removed (0.25 = 25% Damage Reduced, 100 Damage should become 75)</returns>
        public double Resistance(ItemDamageType type) {
            switch (type) {
                case ItemDamageType.Physical: return RESISTANCE_PHYSICAL;
                case ItemDamageType.Nature:   return RESISTANCE_NATURE;
                case ItemDamageType.Arcane:   return RESISTANCE_ARCANE;
                case ItemDamageType.Frost:    return RESISTANCE_FROST;
                case ItemDamageType.Fire:     return RESISTANCE_FIRE;
                case ItemDamageType.Shadow:   return RESISTANCE_SHADOW;
                case ItemDamageType.Holy:     return RESISTANCE_HOLY;
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
        protected float Freq(List<Impedance> imps) {
            // Adds up the total number of impedences
            // and evens them out over the Berserk Timer
            float numImpsOverDur = 0f;
            foreach (Impedance imp in imps) {
                numImpsOverDur += (BerserkTimer / imp.Frequency) * imp.Chance;
            }
            float freq = BerserkTimer / numImpsOverDur;
            return freq;
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
        public float  MovingTargsFreq   {
            get {
                if (Moves.Count > 0) {
                    // Adds up the total number of Moves and evens them out over the Berserk Timer
                    float numMovesOverDur = 0;
                    foreach (Impedance s in Moves) {
                        numMovesOverDur += (BerserkTimer / s.Frequency) * s.Chance;
                    }
                    float freq = BerserkTimer / numMovesOverDur;
                    return freq;
                } else {
                    return 0; // MOVINGTARGS_FREQ;
                }
            }
            //set { MOVINGTARGS_FREQ = value; }
        }
        public float  MovingTargsDur    {
            get {
                if (Moves.Count > 0) {
                    // Averages out the Move Durations
                    float TotalMoveDur = 0;
                    foreach (Impedance s in Moves) { TotalMoveDur += s.Duration; }
                    float dur = TotalMoveDur / Moves.Count;
                    return dur;
                } else {
                    return 0; // MOVINGTARGS_DUR;
                }
            }
            //set { MOVINGTARGS_DUR = value; }
        }
        public float  MovingTargsChance {
            get {
                if (Moves.Count > 0) {
                    // Averages out the Move Chances
                    float TotalChance = 0f;
                    foreach (Impedance s in Moves) { TotalChance += s.Chance; }
                    float chance = TotalChance / (float)Moves.Count;
                    return chance;
                } else {
                    return 0;// MOVINGTARGS_CHANCE;
                }
            }
            //set { MOVINGTARGS_CHANCE = value; }
        }
        public float  MovingTargsTime {
            get {
                float time = 0f;
                if(MovingTargsFreq != 0f && MovingTargsFreq < BerserkTimer) {
                    time = (BerserkTimer / MovingTargsFreq) * (MovingTargsDur / 1000f);
                }
                return time;
            }
        }
        // Stunning Targets
        public Impedance   DynamicCompiler_Stun {
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
        public float  StunningTargsFreq  {
            get {
                if (Stuns.Count > 0) {
                    return Freq(Stuns);
                } else {
                    return 0;// STUNNINGTARGS_FREQ;
                }
            }
            //set { STUNNINGTARGS_FREQ = value; }
        }
        public float  StunningTargsDur   {
            get {
                if (Stuns.Count > 0) {
                    // Averages out the Impedence Durations
                    float TotalStunDur = 0f;
                    foreach (Impedance s in Stuns) { TotalStunDur += s.Duration; }
                    float dur = TotalStunDur / (float)Stuns.Count;
                    return dur;
                } else {
                    return 0;// STUNNINGTARGS_DUR;
                }
            }
            //set { STUNNINGTARGS_DUR  = value; }
        }
        public float  StunningTargsChance {
            get {
                if (Stuns.Count > 0) {
                    // Averages out the Impedence Chances
                    float TotalStunChance = 0f;
                    foreach (Impedance s in Stuns) { TotalStunChance += s.Chance; }
                    float chance = TotalStunChance / (float)Stuns.Count;
                    return chance;
                } else {
                    return 0;// STUNNINGTARGS_CHANCE;
                }
            }
            //set { STUNNINGTARGS_CHANCE = value; }
        }
        public float  StunningTargsTime {
            get {
                float time = 0f;
                if (StunningTargsFreq != 0f && StunningTargsFreq < BerserkTimer)
                {
                    time = (BerserkTimer / StunningTargsFreq) * (StunningTargsDur / 1000f);
                }
                return time;
            }
        }
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
        public float  FearingTargsFreq  {
            get {
                if (Fears.Count > 0) {
                    // Adds up the total number of stuns and evens them out over the Berserk Timer
                    float numFearsOverDur = 0;
                    foreach (Impedance s in Fears) {
                        numFearsOverDur += BerserkTimer / s.Frequency;
                    }
                    float freq = BerserkTimer / numFearsOverDur;
                    return freq;
                } else {
                    return 0; // FEARINGTARGS_FREQ;
                }
            }
            //set { FEARINGTARGS_FREQ = value; }
        }
        public float  FearingTargsDur   {
            get {
                if (Fears.Count > 0) {
                    // Averages out the Fear Durations
                    float TotalFearDur = 0;
                    foreach (Impedance s in Fears) { TotalFearDur += s.Duration; }
                    float dur = TotalFearDur / Fears.Count;
                    return dur;
                } else {
                    return 0; // FEARINGTARGS_DUR;
                }
            }
            //set { FEARINGTARGS_DUR = value; }
        }
        public float  FearingTargsChance {
            get {
                if (Fears.Count > 0) {
                    // Averages out the Fear Chances
                    float TotalChance = 0f;
                    foreach (Impedance s in Fears) { TotalChance += s.Chance; }
                    float chance = TotalChance / (float)Fears.Count;
                    return chance;
                } else {
                    return 0; // FEARINGTARGS_CHANCE;
                }
            }
            //set { FEARINGTARGS_CHANCE = value; }
        }
        public float  FearingTargsTime {
            get {
                float time = 0f;
                if (FearingTargsFreq != 0f && FearingTargsFreq < BerserkTimer)
                {
                    time = (BerserkTimer / FearingTargsFreq) * (FearingTargsDur / 1000f);
                }
                return time;
            }
        }
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
        public float  RootingTargsFreq  {
            get {
                if (Roots.Count > 0) {
                    // Adds up the total number of Roots and evens them out over the Berserk Timer
                    float numRootsOverDur = 0;
                    foreach (Impedance s in Roots)
                    {
                        numRootsOverDur += BerserkTimer / s.Frequency;
                    }
                    float freq = BerserkTimer / numRootsOverDur;
                    return freq;
                } else {
                    return 0; // ROOTINGTARGS_FREQ;
                }
            }
            //set { ROOTINGTARGS_FREQ = value; }
        }
        public float  RootingTargsDur   {
            get {
                if (Roots.Count > 0) {
                    // Averages out the Root Durations
                    float TotalRootDur = 0;
                    foreach (Impedance s in Roots) { TotalRootDur += s.Duration; }
                    float dur = TotalRootDur / Roots.Count;
                    return dur;
                } else {
                    return 0; // ROOTINGTARGS_DUR;
                }
            }
            //set { ROOTINGTARGS_DUR = value; }
        }
        public float  RootingTargsChance {
            get {
                if (Roots.Count > 0) {
                    // Averages out the Impedence Chances
                    float TotalChance = 0f;
                    foreach (Impedance s in Roots) { TotalChance += s.Chance; }
                    float chance = TotalChance / (float)Roots.Count;
                    return chance;
                } else {
                    return 0; // ROOTINGTARGS_CHANCE;
                }
            }
            //set { ROOTINGTARGS_CHANCE = value; }
        }
        public float  RootingTargsTime {
            get {
                float time = 0f;
                if (RootingTargsFreq != 0f && RootingTargsFreq < BerserkTimer)
                {
                    time = (BerserkTimer / RootingTargsFreq) * (RootingTargsDur / 1000f);
                }
                return time;
            }
        }
        // Disarming Targets
        public Impedance DynamicCompiler_Disarm
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
        public float  DisarmingTargsFreq   {
            get {
                if (Disarms.Count > 0) {
                    // Adds up the total number of Disarmes and evens them out over the Berserk Timer
                    float numDisarmsOverDur = 0;
                    foreach (Impedance s in Disarms) {
                        numDisarmsOverDur += BerserkTimer / s.Frequency;
                    }
                    float freq = BerserkTimer / numDisarmsOverDur;
                    return freq;
                } else {
                    return 0; // DISARMINGTARGS_FREQ;
                }
            }
            //set { DISARMINGTARGS_FREQ = value; }
        }
        public float  DisarmingTargsDur    {
            get {
                if (Disarms.Count > 0) {
                    // Averages out the Disarme Durations
                    float TotalDisarmeDur = 0;
                    foreach (Impedance s in Disarms) { TotalDisarmeDur += s.Duration; }
                    float dur = TotalDisarmeDur / Disarms.Count;
                    return dur;
                } else {
                    return 0; // DISARMINGTARGS_DUR;
                }
            }
            //set { DISARMINGTARGS_DUR = value; }
        }
        public float  DisarmingTargsChance {
            get {
                if (Disarms.Count > 0) {
                    // Averages out the Disarm Chances
                    float TotalChance = 0f;
                    foreach (Impedance s in Disarms) { TotalChance += s.Chance; }
                    float chance = TotalChance / (float)Disarms.Count;
                    return chance;
                } else {
                    return 0; // DISARMINGTARGS_CHANCE;
                }
            }
            //set { DISARMINGTARGS_CHANCE = value; }
        }
        public float  DisarmingTargsTime {
            get {
                float time = 0f;
                if (DisarmingTargsFreq != 0f && DisarmingTargsFreq < BerserkTimer)
                {
                    time = (BerserkTimer / DisarmingTargsFreq) * (DisarmingTargsDur / 1000f);
                }
                return time;
            }
        }
        // Other
        public float TimeBossIsInvuln { get { return TIMEBOSSISINVULN; } set { TIMEBOSSISINVULN = value; OnPropertyChanged("TimeBossIsInvuln"); } }
        #endregion
        #endregion

        #region Functions
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
            string retVal = "";
            //
            retVal += "Name: " + Name + "\r\n";
            retVal += "Content: " + ContentString + "\r\n";
            retVal += "Instance: " + Instance + " (" + VersionString + ")\r\n";
            retVal += "Health: " + Health.ToString("#,##0") + "\r\n";
            TimeSpan ts = TimeSpan.FromMinutes(BerserkTimer/60d);
            retVal += "Enrage Timer: " + ts.Minutes.ToString("00") + " Min " + ts.Seconds.ToString("00") + " Sec\r\n";
            ts = TimeSpan.FromMinutes(SpeedKillTimer/60d);
            retVal += "Speed Kill Timer: " + ts.Minutes.ToString("00") + " Min " + ts.Seconds.ToString("00") + " Sec\r\n";
            retVal += "\r\n";
            retVal += "Fight Requirements:" + "\r\n";
            retVal += "Max Players: "  + Max_Players.ToString() + "\r\n";
            retVal += "Min Healers: "  + Min_Healers.ToString() + "\r\n";
            retVal += "Min Tanks: "    + Min_Tanks.ToString() + "\r\n";
            int room = Max_Players - Min_Healers - Min_Tanks;
            retVal += "Room for DPS: " + room.ToString() + "\r\n";
            retVal += "\r\n";
            float TotalDPSNeeded = Health / (BerserkTimer - TimeBossIsInvuln - MovingTargsTime);
            retVal += "To beat the Enrage Timer you need " + TotalDPSNeeded.ToString("#,##0") + " Total DPS\r\n";
            float dpsdps  =  TotalDPSNeeded * ((float)room      / ((float)Min_Tanks / 2f + (float)room)) / (float)room;
            float tankdps = (TotalDPSNeeded * ((float)Min_Tanks / ((float)Min_Tanks / 2f + (float)room)) / (float)Min_Tanks) / 2f;
            retVal += "Tanks Req'd DPS per: " + tankdps.ToString("#,##0") + "\r\n";
            retVal += "DPS Req'd DPS per: " + dpsdps.ToString("#,##0") + "\r\n";
            retVal += "\r\n";
            TotalDPSNeeded = Health / (SpeedKillTimer - TimeBossIsInvuln - SpeedKillTimer * (MovingTargsTime / BerserkTimer));
            retVal += "To beat the Speed Kill Timer you need " + TotalDPSNeeded.ToString("#,##0") + " Total DPS\r\n";
            dpsdps = TotalDPSNeeded * ((float)room / ((float)Min_Tanks / 2f + (float)room)) / (float)room;
            tankdps = (TotalDPSNeeded * ((float)Min_Tanks / ((float)Min_Tanks / 2f + (float)room)) / (float)Min_Tanks) / 2f;
            retVal += "Tanks Req'd DPS per: " + tankdps.ToString("#,##0") + "\r\n";
            retVal += "DPS Req'd DPS per: " + dpsdps.ToString("#,##0") + "\r\n";
            retVal += "\r\n";
            retVal += "This boss does the following Damage Per Second Amounts, factoring Armor and Defend Tables where applicable:" + "\r\n";
            retVal += "Single Target Melee: " + GetDPSByType(ATTACK_TYPES.AT_MELEE,BossDamageBonus, BossDamagePenalty,
                                  p_missPerc, p_dodgePerc, p_parryPerc, p_blockPerc, p_blockVal).ToString("0.0") + "\r\n";
            retVal += "Single Target Ranged: " + GetDPSByType(ATTACK_TYPES.AT_RANGED, BossDamageBonus, BossDamagePenalty,
                                  p_missPerc).ToString("0.0") + "\r\n";
            retVal += "Raid AoE: " + GetDPSByType(ATTACK_TYPES.AT_AOE, BossDamageBonus, 0).ToString("0.0") + "\r\n";
            retVal += "\r\n";
            retVal += "Comment(s):\r\n" + Comment;
            //
            return retVal;
        }
        /// <summary>
        /// Generates a Fight Info description listing the stats of the fight as well as any comments listed for the boss
        /// </summary>
        /// <returns>The generated string</returns>
        public string GenInfoString() { return GenInfoString(0,0,0,0,0,0,0); }
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
            return Content.ToString() + " : " + Instance + " : (" + Version.ToString() + ") : " + Name;
        }
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
            Content = new BossHandler.TierLevels[] { BossHandler.TierLevels.T10_0, BossHandler.TierLevels.T10_5, BossHandler.TierLevels.T10_5, BossHandler.TierLevels.T10_9 };
            Version = new BossHandler.Versions[] { BossHandler.Versions.V_10N, BossHandler.Versions.V_25N, BossHandler.Versions.V_10H, BossHandler.Versions.V_25H };
            // Fight Requirements
            Min_Tanks = new int[] { 2, 2, 2, 2 };
            Min_Healers = new int[] { 2, 5, 2, 5 };
        }
        #region Variable Convenience Overrides
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
        public BossHandler.Versions[] Version
        {
            get
            {
                return new BossHandler.Versions[] {
                    this[0].Version,
                    this[1].Version,
                    this[2].Version,
                    this[3].Version,
                };
            }
            set
            {
                int i = 0;
                this[i].Version = value[i]; i++;
                this[i].Version = value[i]; i++;
                this[i].Version = value[i]; i++;
                this[i].Version = value[i];
            }
        }
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
        public BossHandler BossByVersion(BossHandler.Versions v) { return this[(int)v]; }
        #endregion
    }
}
