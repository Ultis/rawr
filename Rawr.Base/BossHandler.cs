using System;
using System.Collections.Generic;
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
    public enum ATTACK_TYPES { AT_MELEE, AT_RANGED, AT_AOE, }
    /// <summary>A single Attack of various types</summary>
    public struct Attack {
        /// <summary>The Name of the Attack</summary>
        public string Name;
        /// <summary>The type of damage done, use the ItemDamageType enumerator to select</summary>
        public ItemDamageType DamageType;
        /// <summary>The Unmitigated Damage per Hit for this attack</summary>
        public float DamagePerHit;
        /// <summary>The maximum number of party/raid members this attack can hit</summary>
        public float MaxNumTargets;
        /// <summary>The frequency of this attack (in seconds)</summary>
        public float AttackSpeed;
        /// <summary>The Attack Type (for AoE vs single-target Melee/Ranged)</summary>
        public ATTACK_TYPES AttackType;
    }
    /// <summary>Stores a Stun that the Boss Performs</summary>
    public struct Stun {
        public float Frequency;
        public float Duration;
        public float Chance;
        public bool Breakable;
    }
    /// <summary>Stores a Fear that the Boss Performs</summary>
    public struct Fear {
        public float Frequency;
        public float Duration;
        public float Chance;
        public bool Breakable;
    }
    /// <summary>Stores a Root that the Boss Performs</summary>
    public struct Root {
        public float Frequency;
        public float Duration;
        public float Chance;
        public bool Breakable;
    }
    /// <summary>Stores a Move that the Boss Performs</summary>
    public struct Move {
        public float Frequency;
        public float Duration;
        public float Chance;
        public bool Breakable;
    }
    /// <summary>Stores a Disarm that the Boss Performs</summary>
    public struct Disarm {
        public float Frequency;
        public float Duration;
        public float Chance;
        public bool Breakable;
    }
    #endregion
    public class BossHandler {
        public const int NormCharLevel = 80;
        public BossHandler() {
            // Basics
            Name = "Generic";
            Content = "Generic";
            Instance = "None";
            Version = "10 Man";
            Comment = "No comments have been written for this Boss.";
            BerserkTimer = 8 * 60; // The longest noted Enrage timer is 19 minutes, and seriously, if the fight is taking that long, then fail... just fail.
            SpeedKillTimer = 3 * 60; // Lots of Achievements run on this timer, so using it for generic
            Level = (int)POSSIBLE_LEVELS.LVLP3;
            Health = 1000000f;
            Armor = (float)StatConversion.NPC_ARMOR[Level - NormCharLevel];
            UseParryHaste = false;
            // Resistance
            ItemDamageType[] DamageTypes = new ItemDamageType[] { ItemDamageType.Physical, ItemDamageType.Nature, ItemDamageType.Arcane, ItemDamageType.Frost, ItemDamageType.Fire, ItemDamageType.Shadow, ItemDamageType.Holy, };
            foreach (ItemDamageType t in DamageTypes) { Resistance(t, 0f); }
            // Attacks
            Attacks = new List<Attack>();
            // Situational Changes
            InBackPerc_Melee   = 0.00f; // Default to never in back
            InBackPerc_Ranged  = 0.00f; // Default to never in back
            MultiTargsPerc     = 0.00f; // Default to 0% multiple targets
            MaxNumTargets      =    1f; // Default to max 1 targets (though at 0%, this means nothing)
            StunningTargsFreq  =    0f; // Default to never stunning
            StunningTargsDur   = 5000f; // Default to stun durations of 5 seconds but since it's 0 stuns over dur, this means nothing
            MovingTargsFreq    =    0f; // Default to never moving
            MovingTargsDur     = 5000f; // Default to move durations of 5 seconds but since it's 0 moves over dur, this means nothing
            DisarmingTargsFreq =    0f; // Default to never disarming
            DisarmingTargsDur  = 5000f; // Default to disarm durations of 5 seconds but since it's 0 disarms over dur, this means nothing
            FearingTargsFreq   =    0f; // Default to never fearing
            FearingTargsDur    = 5000f; // Default to fear durations of 5 seconds but since it's 0 fears over dur, this means nothing
            RootingTargsFreq   =    0f; // Default to never rooting
            RootingTargsDur    = 5000f; // Default to root durations of 5 seconds but since it's 0 roots over dur, this means nothing
            TimeBossIsInvuln   =    0f; // Default to never invulnerable (Invuln. like KT in Phase 1)
            // Fight Requirements
            Max_Players = 10;
            Min_Healers =  3;
            Min_Tanks   =  2;
        }

        #region Variables
        public enum TierLevels : int {
            T7_0 = 0,
            T7_5,
            T8_0,
            T8_5,
            T9_0,
            T9_5,
        }
        public readonly float[] StandardMeleePerHit = new float[] {
              5000f*2f, //T7_0,
             10000f*2f, //T7_5,
             20000f*2f, //T8_0,
             30000f*2f, //T8_5,
             40000f*2f, //T9_0,
             50000f*2f, //T9_5,
        };
        // Basics
        private string NAME,CONTENT,INSTANCE,VERSION,COMMENT;
        private float HEALTH,ARMOR;
        private int BERSERKTIMER,SPEEDKILLTIMER,LEVEL;
        private bool USERPARRYHASTE;
        // Resistance
        private float RESISTANCE_PHYSICAL,RESISTANCE_NATURE,RESISTANCE_ARCANE,RESISTANCE_FROST,RESISTANCE_FIRE,RESISTANCE_SHADOW,RESISTANCE_HOLY;
        // Attacks
        private List<Attack> ATTACKS;
        // Situational Changes
        public List<Stun> Stuns = new List<Stun>();
        public List<Fear> Fears = new List<Fear>();
        public List<Root> Roots = new List<Root>();
        public List<Move> Moves = new List<Move>();
        public List<Disarm> Disarms = new List<Disarm>();
        private float INBACKPERC_MELEE, INBACKPERC_RANGED,
                      MULTITARGSPERC, MAXNUMTARGS,
                      STUNNINGTARGS_FREQ, STUNNINGTARGS_DUR, STUNNINGTARGS_CHANCE,
                      MOVINGTARGS_FREQ, MOVINGTARGS_DUR, MOVINGTARGS_CHANCE,
                      DISARMINGTARGS_FREQ, DISARMINGTARGS_DUR, DISARMINGTARGS_CHANCE,
                      FEARINGTARGS_FREQ, FEARINGTARGS_DUR, FEARINGTARGS_CHANCE,
                      ROOTINGTARGS_FREQ, ROOTINGTARGS_DUR, ROOTINGTARGS_CHANCE,
                      TIMEBOSSISINVULN;
        // Fight Requirements
        private int MAX_PLAYERS, MIN_HEALERS, MIN_TANKS;
        #endregion

        #region Get/Set
        // ==== Basics ====
        public string Name               { get { return NAME;               } set { NAME               = value; } }
        public string Content            { get { return CONTENT;            } set { CONTENT            = value; } }
        public string Instance           { get { return INSTANCE;           } set { INSTANCE           = value; } }
        public string Version            { get { return VERSION;            } set { VERSION            = value; } }
        public string Comment            { get { return COMMENT;            } set { COMMENT            = value; } }
        public int    Level              { get { return LEVEL;              } set { LEVEL              = value; } }
        public float  Health             { get { return HEALTH;             } set { HEALTH             = value; } }
        public float  Armor              { get { return ARMOR;              } set { ARMOR              = value; } }
        public int    BerserkTimer       { get { return BERSERKTIMER;       } set { BERSERKTIMER       = value; } }
        public int    SpeedKillTimer     { get { return SPEEDKILLTIMER;     } set { SPEEDKILLTIMER     = value; } }
        public bool   UseParryHaste      { get { return USERPARRYHASTE;     } set { USERPARRYHASTE     = value; } }
        // ==== Situational Changes ====
        // Standing in back
        public float  InBackPerc_Melee   { get { return INBACKPERC_MELEE;   } set { INBACKPERC_MELEE   = value; } }
        public float  InBackPerc_Ranged  { get { return INBACKPERC_RANGED;  } set { INBACKPERC_RANGED  = value; } }
        // Multiple Targets
        public float  MultiTargsPerc     { get { return MULTITARGSPERC;     } set { MULTITARGSPERC     = value; } }
        public float  MaxNumTargets      { get { return MAXNUMTARGS;        } set { MAXNUMTARGS        = value; } }
        // Stunning Targets
        public Stun   DynamicCompiler_Stun {
            get {
                // Make one
                Stun retVal = new Stun() {
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
                    //retVal.Frequency = -1f; // if we are, use this as a flag
                }
                // Return results
                return retVal;
            }
        }
        public float  StunningTargsFreq  {
            get {
                if (Stuns.Count > 0) {
                    // Adds up the total number of stuns and evens them out over the Berserk Timer
                    float numStunsOverDur = 0f;
                    foreach (Stun s in Stuns) {
                        numStunsOverDur += BerserkTimer / s.Frequency;
                    }
                    float freq = BerserkTimer / numStunsOverDur;
                    return freq;
                } else {
                    return STUNNINGTARGS_FREQ;
                }
            }
            set { STUNNINGTARGS_FREQ = value; }
        }
        public float  StunningTargsDur   {
            get {
                if (Stuns.Count > 0) {
                    // Averages out the Stun Durations
                    float TotalStunDur = 0f;
                    foreach (Stun s in Stuns) { TotalStunDur += s.Duration; }
                    float dur = TotalStunDur / (float)Stuns.Count;
                    return dur;
                } else {
                    return STUNNINGTARGS_DUR ;
                }
            }
            set { STUNNINGTARGS_DUR  = value; }
        }
        public float  StunningTargsChance {
            get {
                if (Stuns.Count > 0) {
                    // Averages out the Stun Chances
                    float TotalStunChance = 0f;
                    foreach (Stun s in Stuns) { TotalStunChance += s.Chance; }
                    float chance = TotalStunChance / (float)Stuns.Count;
                    return chance;
                } else {
                    return STUNNINGTARGS_CHANCE ;
                }
            }
            set { STUNNINGTARGS_CHANCE = value; }
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
        // Moving Targets
        public Move   DynamicCompiler_Move {
            get {
                // Make one
                Move retVal = new Move() {
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
                    foreach (Move s in Moves) {
                        numMovesOverDur += (BerserkTimer / s.Frequency) * s.Chance;
                    }
                    float freq = BerserkTimer / numMovesOverDur;
                    return freq;
                } else {
                    return MOVINGTARGS_FREQ;
                }
            }
            set { MOVINGTARGS_FREQ = value; }
        }
        public float  MovingTargsDur    {
            get {
                if (Moves.Count > 0) {
                    // Averages out the Move Durations
                    float TotalMoveDur = 0;
                    foreach (Move s in Moves) { TotalMoveDur += s.Duration; }
                    float dur = TotalMoveDur / Moves.Count;
                    return dur;
                } else {
                    return MOVINGTARGS_DUR;
                }
            }
            set { MOVINGTARGS_DUR = value; }
        }
        public float  MovingTargsChance {
            get {
                if (Moves.Count > 0) {
                    // Averages out the Move Chances
                    float TotalChance = 0f;
                    foreach (Move s in Moves) { TotalChance += s.Chance; }
                    float chance = TotalChance / (float)Moves.Count;
                    return chance;
                } else {
                    return MOVINGTARGS_CHANCE ;
                }
            }
            set { MOVINGTARGS_CHANCE = value; }
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
        // Disarming Targets
        public Disarm DynamicCompiler_Disarm {
            get {
                // Make one
                Disarm retVal = new Disarm() {
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
                    foreach (Disarm s in Disarms) {
                        numDisarmsOverDur += BerserkTimer / s.Frequency;
                    }
                    float freq = BerserkTimer / numDisarmsOverDur;
                    return freq;
                } else {
                    return DISARMINGTARGS_FREQ;
                }
            }
            set { DISARMINGTARGS_FREQ = value; }
        }
        public float  DisarmingTargsDur    {
            get {
                if (Disarms.Count > 0) {
                    // Averages out the Disarme Durations
                    float TotalDisarmeDur = 0;
                    foreach (Disarm s in Disarms) { TotalDisarmeDur += s.Duration; }
                    float dur = TotalDisarmeDur / Disarms.Count;
                    return dur;
                } else {
                    return DISARMINGTARGS_DUR;
                }
            }
            set { DISARMINGTARGS_DUR = value; }
        }
        public float  DisarmingTargsChance {
            get {
                if (Disarms.Count > 0) {
                    // Averages out the Disarm Chances
                    float TotalChance = 0f;
                    foreach (Disarm s in Disarms) { TotalChance += s.Chance; }
                    float chance = TotalChance / (float)Disarms.Count;
                    return chance;
                } else {
                    return DISARMINGTARGS_CHANCE ;
                }
            }
            set { DISARMINGTARGS_CHANCE = value; }
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
        // Fearing Targets
        public Fear   DynamicCompiler_Fear {
            get {
                // Make one
                Fear retVal = new Fear() {
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
                    foreach (Fear s in Fears) {
                        numFearsOverDur += BerserkTimer / s.Frequency;
                    }
                    float freq = BerserkTimer / numFearsOverDur;
                    return freq;
                } else {
                    return FEARINGTARGS_FREQ;
                }
            }
            set { FEARINGTARGS_FREQ = value; }
        }
        public float  FearingTargsDur   {
            get {
                if (Fears.Count > 0) {
                    // Averages out the Fear Durations
                    float TotalFearDur = 0;
                    foreach (Fear s in Fears) { TotalFearDur += s.Duration; }
                    float dur = TotalFearDur / Fears.Count;
                    return dur;
                } else {
                    return FEARINGTARGS_DUR;
                }
            }
            set { FEARINGTARGS_DUR = value; }
        }
        public float  FearingTargsChance {
            get {
                if (Fears.Count > 0) {
                    // Averages out the Fear Chances
                    float TotalChance = 0f;
                    foreach (Fear s in Fears) { TotalChance += s.Chance; }
                    float chance = TotalChance / (float)Fears.Count;
                    return chance;
                } else {
                    return FEARINGTARGS_CHANCE ;
                }
            }
            set { FEARINGTARGS_CHANCE = value; }
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
        public Root   DynamicCompiler_Root {
            get {
                // Make one
                Root retVal = new Root() {
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
                    foreach (Root s in Roots) {
                        numRootsOverDur += BerserkTimer / s.Frequency;
                    }
                    float freq = BerserkTimer / numRootsOverDur;
                    return freq;
                } else {
                    return ROOTINGTARGS_FREQ;
                }
            }
            set { ROOTINGTARGS_FREQ = value; }
        }
        public float  RootingTargsDur   {
            get {
                if (Roots.Count > 0) {
                    // Averages out the Root Durations
                    float TotalRootDur = 0;
                    foreach (Root s in Roots) { TotalRootDur += s.Duration; }
                    float dur = TotalRootDur / Roots.Count;
                    return dur;
                } else {
                    return ROOTINGTARGS_DUR;
                }
            }
            set { ROOTINGTARGS_DUR = value; }
        }
        public float  RootingTargsChance {
            get {
                if (Roots.Count > 0) {
                    // Averages out the Stun Chances
                    float TotalChance = 0f;
                    foreach (Root s in Roots) { TotalChance += s.Chance; }
                    float chance = TotalChance / (float)Roots.Count;
                    return chance;
                } else {
                    return ROOTINGTARGS_CHANCE ;
                }
            }
            set { ROOTINGTARGS_CHANCE = value; }
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
        // Other
        public float  TimeBossIsInvuln   { get { return TIMEBOSSISINVULN;   } set { TIMEBOSSISINVULN   = value; } }
        // ==== Fight Requirements ====
        /// <summary>Example values: 5, 10, 25, 40</summary>
        public int    Max_Players        { get { return MAX_PLAYERS;        } set { MAX_PLAYERS        = value; } }
        public int    Min_Healers        { get { return MIN_HEALERS;        } set { MIN_HEALERS        = value; } }
        public int    Min_Tanks          { get { return MIN_TANKS;          } set { MIN_TANKS          = value; } }
        // ==== Attacks ====
        public List<Attack> Attacks { get { return ATTACKS; } set { ATTACKS = value; } }
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
        // Rooting Targets
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

        #region Functions
        /// <summary>A handler for Boss Damage Taken Reduction due to Resistance (Physical, Fire, etc). </summary>
        /// <returns>The Percentage of Damage to be removed (0.25 = 25% Damage Reduced, 100 Damage should become 75)</returns>
        public float Resistance(ItemDamageType type) {
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
        public float Resistance(ItemDamageType type, float newValue) {
            switch (type) {
                case ItemDamageType.Physical: return RESISTANCE_PHYSICAL = newValue;
                case ItemDamageType.Nature:   return RESISTANCE_NATURE   = newValue;
                case ItemDamageType.Arcane:   return RESISTANCE_ARCANE   = newValue;
                case ItemDamageType.Frost:    return RESISTANCE_FROST    = newValue;
                case ItemDamageType.Fire:     return RESISTANCE_FIRE     = newValue;
                case ItemDamageType.Shadow:   return RESISTANCE_SHADOW   = newValue;
                case ItemDamageType.Holy:     return RESISTANCE_HOLY     = newValue;
                default: break;
            }
            return 0f;
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
            string retVal = "";
            //
            retVal += "Name: " + Name + "\r\n";
            retVal += "Content: " + Content + "\r\n";
            retVal += "Instance: " + Instance + " (" + Version + ")\r\n";
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
    }
}
