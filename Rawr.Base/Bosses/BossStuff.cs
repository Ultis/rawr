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
    #region Subclasses/Enums/Structs
    /// <summary>The role of the player, will allow certain lists to return filtered to things that affect said role</summary>
    public enum PLAYER_ROLES { MainTank = 0, OffTank, TertiaryTank, MeleeDPS, RangedDPS, MainTankHealer, OffAndTertTankHealer, RaidHealer }
    /// <summary>Enumerator for creating a list of possible values for the Level box</summary>
    public enum POSSIBLE_LEVELS { LVLP0 = 85, LVLP1 = 86, LVLP2 = 87, LVLP3 = 88, }
    /// <summary>Enumerator for creating a list of possible values for the Mob Type box</summary>
    public enum MOB_TYPES { BEAST = 0, DEMON, DRAGONKIN, ELEMENTAL, GIANT, HUMANOID, UNDEAD }
    #region Attacks
    /// <summary>Enumerator for attack types, this mostly is for raid members that aren't being directly attacked to know when AoE damage is coming from the boss</summary>
    public enum ATTACK_TYPES { AT_MELEE = 0, AT_RANGED, AT_AOE, }
    /// <summary>A single Attack of various types</summary>
    public partial class Attack {
        /// <summary>The Name of the Attack</summary>
        public string Name = "Unnamed";
        /// <summary>The type of damage done, use the ItemDamageType enumerator to select</summary>
        [DefaultValue(ItemDamageType.Physical)]
        public ItemDamageType DamageType = ItemDamageType.Physical;
        /// <summary>This is so you can pull the Default Melee Attack more easily</summary>
        [DefaultValue(false)]
        public bool IsTheDefaultMelee = false;
        /// <summary>The Unmitigated Damage per Hit for this attack, 5000f is 5,000 Raw Unmitigated Damage. When DamageIsPerc is true DamagePerHit = 0.75f; would be 75% of Player's Health</summary>
        [DefaultValue(100*1000)]
        public virtual float DamagePerHit { get; set; }
        /// <summary>When set to True, DamagePerHit will be seen as a Percentage. DamagePerHit = 0.75f; would be 75% of Player's Health</summary>
        [DefaultValue(false)]
        public bool DamageIsPerc = false;
        /// <summary>The maximum number of party/raid members this attack can hit</summary>
        [DefaultValue(1)]
        public float MaxNumTargets = 1;
        /// <summary>The frequency of this attack (in seconds)</summary>
        [DefaultValue(2)]
        public float AttackSpeed = 2.0f;
        /// <summary>The Attack Type (for AoE vs. single-target Melee/Ranged)</summary>
        [DefaultValue(ATTACK_TYPES.AT_MELEE)]
        public ATTACK_TYPES AttackType = ATTACK_TYPES.AT_MELEE;
        /// <summary>If the attack Parry Haste's, then the attacks that are parried will reset the swing timer.</summary>
        [DefaultValue(false)]
        public bool UseParryHaste = false;
        /// <summary>if the attack is part of a Dual Wield, there is an additional 20% chance to Miss<para>This should flag the Buff... sort of</para></summary>
        [DefaultValue(false)]
        public bool IsDualWielding = false;
        #region Player Avoidance
        /// <summary>Returns True if any of the Avoidance types are true</summary>
        public bool Avoidable { get { return Missable || Dodgable || Parryable || Blockable; } }
        /// <summary>Can this attack Miss the player</summary>
        [DefaultValue(true)]
        public bool Missable = true;
        /// <summary>Can this attack be Dodged by the player</summary>
        [DefaultValue(true)]
        public bool Dodgable = true;
        /// <summary>Can this attack be Parried by the player</summary>
        [DefaultValue(true)]
        public bool Parryable = true;
        /// <summary>Can this attack be Blocked by the player</summary>
        [DefaultValue(true)]
        public bool Blockable = true;
        #endregion
        #region Player Targeting
        private SerializableDictionary<PLAYER_ROLES, bool> _affectsRole = null;
        public SerializableDictionary<PLAYER_ROLES, bool> AffectsRole {
            get {
                return _affectsRole ?? (_affectsRole = new SerializableDictionary<PLAYER_ROLES, bool> {
                    // Tanks
                    { PLAYER_ROLES.MainTank,             false },
                    { PLAYER_ROLES.OffTank,              false },
                    { PLAYER_ROLES.TertiaryTank,         false },
                    // DPS
                    { PLAYER_ROLES.MeleeDPS,             false },
                    { PLAYER_ROLES.RangedDPS,            false },
                    // Heals
                    { PLAYER_ROLES.MainTankHealer,       false },
                    { PLAYER_ROLES.OffAndTertTankHealer, false },
                    { PLAYER_ROLES.RaidHealer,           false },
                });
            }
            set { _affectsRole = value; }
        }
        #endregion
        #region Player Negation
        [DefaultValue(false)]
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
    /// <summary>A single Attack of type Damage over Time</summary>
    public partial class DoT : Attack {
        /// <summary>The initial damage of the dot, separate from the over time portion</summary>
        [DefaultValue(1000)]
        public override float DamagePerHit { get; set; }
        /// <summary>The over time damage of the dot, separate from the initial portion</summary>
        [DefaultValue(20*1000/8)]
        public float DamagePerTick;
        /// <summary>The total number of Ticks</summary>
        [DefaultValue(8)]
        public float NumTicks;
        /// <summary>Interval of the ticks, 1 = 1 sec</summary>
        [DefaultValue(2)]
        public float TickInterval;
        /// <summary>The full duration of the DoT, 2 sec Interval * 5 Ticks = 10 sec duration</summary>
        public float Duration { get { return TickInterval * NumTicks; } }
    }
    #endregion
    #region Impedance
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
        /// <summary>
        /// The amount of time between activations of this Impedance, in sec
        /// <para>Eg- This Impedance occurs every 45 sec</para>
        /// <para>Frequency = 45f</para>
        /// </summary>
        public float Frequency;
        /// <summary>
        /// The amount of time spent in this state, in millisec
        /// <para>Eg- This Impedance lasts 4 seconds:</para>
        /// <para>Duration = 4f * 1000f</para>
        /// </summary>
        public float Duration;
        /// <summary>
        /// A Percentage, value range from 0% to 100% (0.00f to 1.00f)
        /// <para>Eg- An Impedance effects one random raid target:</para>
        /// <para>Chance = 1f / this.Max_Players</para>
        /// <para>Eg- An Impedance effects three random raid targets:</para>
        /// <para>Chance = 3f / this.Max_Players</para>
        /// <para>Eg- An Impedance effects all raid targets:</para>
        /// <para>Chance = 1f</para>
        /// </summary>
        public float Chance;
        /// <summary>
        /// Flag which indicates whether the player can reduce or break the Duration of the Impedance
        /// <para>Eg- The player has to move for 4 seconds, movement can be broken by MovementSpeed bonus and Charge-like abilities:</para>
        /// <para>Breakable = true</para>
        /// <para>Eg- The player is stunned by Smite (Deadmines) for 4 seconds, this stun is scripted and cannot be broken:</para>
        /// <para>Breakable = false</para>
        /// </summary>
        public bool Breakable;
        #endregion
        #region Functions
        /// <summary>
        /// Returns False if
        /// <para>Chance is <= 0</para>
        /// <para>Frequency is <= 0 or > 20 min</para>
        /// <para>Duration <= 0 or > 20 sec</para>
        /// </summary>
        public bool Validate {
            get {
                if (Chance <= 0) { return false; }
                if (Frequency <= 0 || Frequency > 20 * 60) { return false; }
                if (Duration <= 0 || Duration > 20 * 1000) { return false; }
                return true;
            }
        }
        public override string ToString() {
            if (!Validate) { return "None"; }
            return string.Format("F: {0:0}s D: {1:0}ms C: {2:0.0%}{3}",
                Frequency, Duration, Chance, Breakable ? " : B" : "");
        }
        #endregion

        /// <summary>
        /// Generates the time lost to this Impedance type.
        /// </summary>
        /// <param name="bossOpts">Pass character.BossOptions</param>
        /// <param name="type">The Type to check:<para>0: Moves</para><para>1: Fears</para><para>2: Stuns</para><para>3: Roots</para></param>
        /// <param name="breakingMOD">The modifier for this type, eg- Type is Moves, pass stats.MovementSpeed</param>
        /// <param name="react">How much time in milliseconds it takes you to react to the Occurrence of the Impedance</param>
        /// <param name="recovery">How much time in milliseconds the recovery method consumes. eg- Every Man for Himself still consumes a GCD</param>
        /// <returns>Returns the Percentage of time lost to this Impedance type.
        /// <para>This is limited to 0%-100% to prevent wierd calc issues</para></returns>
        public static float GetImpedancePerc(BossOptions bossOpts, int type, float breakingMOD, float react=0, float recovery=0)
        {
            List<Impedance> imps;
            // Which Imps are we looking for?
            if      (type == 0 && bossOpts.MovingTargs  ) { imps = bossOpts.Moves; }
            else if (type == 1 && bossOpts.FearingTargs ) { imps = bossOpts.Fears; }
            else if (type == 2 && bossOpts.StunningTargs) { imps = bossOpts.Stuns; }
            else if (type == 3 && bossOpts.RootingTargs ) { imps = bossOpts.Roots; }
            else if (type == 4 && bossOpts.SilencingTargs) { imps = bossOpts.Silences; }
            else return 0f;
            // Are there any of this type?
            if (imps.Count <= 0) return 0f;
            // Process them individually and add to the total time lost
            float timeIn = 0, newAmt = 0;
            foreach (Impedance i in imps) {
                // Determine how much time we lose
                newAmt  = (i.Duration / 1000f)                    // The length of the Impedance
                        * (1f - (i.Breakable ? breakingMOD : 0f)) // If you can break it, by how much
                        * i.Chance                                // Chance the Occurrence affects you
                        * (bossOpts.BerserkTimer / i.Frequency);  // Number of Occurrences
                // Add back how much time we can recover, up the amount lost - react time and only if it's breakable
                newAmt -= (i.Breakable ? Math.Min(Math.Max(0f, newAmt - (react / 1000f)), newAmt - (recovery/1000f)) : 0f);
                // Add this to the total
                timeIn += newAmt;
            }
            // Convert this to a Percentage
            float timeInPerc = Math.Max(0f, Math.Min(1f, timeIn / bossOpts.BerserkTimer));
            // Return the Percentage
            return timeInPerc;
        }

        /// <summary>
        /// Generates the time lost to all Impedance types.
        /// </summary>
        /// <param name="bossOpts">Pass character.BossOptions</param>
        /// <param name="moveBreakingMOD">The modifier for this type, eg- Type is Moves, pass stats.MovementSpeed</param>
        /// <param name="fearBreakingMOD">The modifier for this type, eg- Type is Fears, pass stats.FearDurReduc</param>
        /// <param name="stunBreakingMOD">The modifier for this type, eg- Type is Stuns, pass stats.StunDurReduc</param>
        /// <param name="rootBreakingMOD">The modifier for this type, eg- Type is Roots, pass stats.SnareRootDurReduc</param>
        /// <param name="silenceBreakingMOD">The modifier for this type, eg- Type is Silences, pass stats.SilenceDurReduc</param>
        /// <param name="moveBreakingRec">How much time in milliseconds the recovery method consumes. eg- Charge still consumes a GCD</param>
        /// <param name="fearBreakingRec">How much time in milliseconds the recovery method consumes. eg- Every Man for Himself still consumes a GCD</param>
        /// <param name="stunBreakingRec">How much time in milliseconds the recovery method consumes. eg- Every Man for Himself still consumes a GCD</param>
        /// <param name="rootBreakingRec">How much time in milliseconds the recovery method consumes. eg- Every Man for Himself still consumes a GCD</param>
        /// <param name="silenceBreakingRec">How much time in milliseconds the recovery method consumes. eg- Every Man for Himself still consumes a GCD</param>
        /// <param name="react">How much time in milliseconds it takes you to react to the Occurrence of the Impedance</param>
        /// <returns>Returns the Percentage of time lost to all Impedance types.
        /// <para>This is limited to 0%-100% to prevent wierd calc issues</para></returns>
        public static float GetTotalImpedancePercs(BossOptions bossOpts, float moveBreakingMOD, float fearBreakingMOD, float stunBreakingMOD, float rootBreakingMOD, float silenceBreakingMOD,
            float moveBreakingRec = 0, float fearBreakingRec = 0, float stunBreakingRec = 0, float rootBreakingRec = 0, float silenceBreakingRec = 0, float react = 0)
        {
            float MoveMOD = 1f - GetImpedancePerc(bossOpts, 0, moveBreakingMOD, react, moveBreakingRec);
            float FearMOD = 1f - GetImpedancePerc(bossOpts, 1, fearBreakingMOD, react, fearBreakingRec);
            float StunMOD = 1f - GetImpedancePerc(bossOpts, 2, stunBreakingMOD, react, stunBreakingRec);
            float RootMOD = 1f - GetImpedancePerc(bossOpts, 3, rootBreakingMOD, react, rootBreakingRec);
            float SilenceMOD = 1f - GetImpedancePerc(bossOpts, 4, silenceBreakingMOD, react, silenceBreakingRec);
            //
            float TotalBossHandlerMOD = MoveMOD * FearMOD * StunMOD * RootMOD * SilenceMOD;
            return TotalBossHandlerMOD;
        }
    }
    public enum ImpedanceTypes { Fear, Root, Stun, Move, /*Silence,*/ Disarm };
    public struct ImpedanceWithType
    {
        public Impedance imp;
        public ImpedanceTypes type;
    }
    #endregion
    public partial class TargetGroup
    {
        #region Constructors
        public TargetGroup() {
            Frequency = -1;
            Duration  = 20 * 1000f;
            Chance    = 1.00f;
            NumTargs  = 2;
            NearBoss  = false;
        }
        public TargetGroup(float f, float d, float c, float t, bool n) {
            Frequency = f;
            Duration  = d;
            Chance    = c;
            NumTargs  = t;
            NearBoss  = n;
        }
        public TargetGroup(TargetGroup i) {
            TargetGroup clone = (TargetGroup)i.MemberwiseClone();
            Frequency = clone.Frequency;
            Duration  = clone.Duration;
            Chance    = clone.Chance;
            NumTargs  = clone.NumTargs;
            NearBoss  = clone.NearBoss;
        }
        #endregion
        #region Variables
        /// <summary>In Seconds<para>Defaults to -1 as an 'invalid' flag</para></summary>
        public float Frequency = -1;
        /// <summary>In MilliSeconds (1/1000 of a second)<para>Defaults to 20 seconds</para></summary>
        public float Duration = 20 * 1000;
        /// <summary>Percentage, 0.50f = 50% Chance that this occurs<para>Defaults to and almost every time in usage this should be 1.00f=100%</para></summary>
        public float Chance = 1.00f;
        /// <summary>The Number of Targets in this Target Group<para>Defaults to 2</para></summary>
        public float NumTargs = 2;
        /// <summary>
        /// If the mobs are near boss then your MultiTargets calculations should also count the boss,
        /// otherwise this is a group that must be DPS'd by itself.
        /// <para>Defaults to false</para>
        /// </summary>
        public bool NearBoss = false;
        #endregion
        #region Functions
        public bool Validated { get { return Frequency != -1 && Duration > 0 && Chance > 0 && NumTargs > 1; } }
        public override string ToString()
        {
            if (!Validated) { return "None"; }
            return string.Format("#T: {0:0.00} F: {1:0.0}s D: {2:0.00}s C: {3:0%}{4}",
                NumTargs, Frequency, Duration / 1000f, Chance, NearBoss ? " : NB" : "");
        }
        #endregion
    }
    public class BuffState
    {
        #region Constructors
        public BuffState() {
            Name = "Unnamed";
            Frequency = 120f;
            Duration  = 5000f;
            Chance    = 1.00f;
            Breakable = true;
            Stats = new Stats();
        }
        public BuffState(string n,float f, float d, float c, bool b, Stats s) {
            Name = n;
            Frequency = f;
            Duration  = d;
            Chance    = c;
            Breakable = b;
            Stats = s;
        }
        public BuffState(BuffState b)
        {
            BuffState clone = b.MemberwiseClone() as BuffState;
            Name = clone.Name;
            Frequency = clone.Frequency;
            Duration  = clone.Duration;
            Chance    = clone.Chance;
            Breakable = clone.Breakable;
            Stats = clone.Stats.Clone();
        }
        #endregion
        #region Variables
        /// <summary>The Name of the BuffState</summary>
        public string Name;
        /// <summary>
        /// The amount of time between activations of this BuffState, in sec
        /// <para>Eg- This BuffState occurs every 45 sec</para>
        /// <para>Frequency = 45f</para>
        /// </summary>
        public float Frequency;
        /// <summary>
        /// The amount of time spent in this state, in millisec
        /// <para>Eg- This BuffState lasts 4 seconds:</para>
        /// <para>Duration = 4f * 1000f</para>
        /// </summary>
        public float Duration;
        /// <summary>
        /// A Percentage, value range from 0% to 100% (0.00f to 1.00f)
        /// <para>Eg- A BuffState effects one random raid target:</para>
        /// <para>Chance = 1f / this.Max_Players</para>
        /// <para>Eg- A BuffState effects three random raid targets:</para>
        /// <para>Chance = 3f / this.Max_Players</para>
        /// <para>Eg- A BuffState effects all raid targets:</para>
        /// <para>Chance = 1f</para>
        /// </summary>
        public float Chance;
        /// <summary>
        /// Flag which indicates whether the player can reduce or break the Duration of the BuffState
        /// <para>ToDo: Examples</para>
        /// </summary>
        public bool Breakable;
        /// <summary>
        /// The Stats of the BuffState
        /// <para>Eg- Icehowl's Staggered Daze: Increases all damage taken by Icehowl by 100% for 15 sec.</para>
        /// <para>Player would get new Stats() { BonusDamageMultiplier = 1.00f, }</para>
        /// </summary>
        public Stats Stats;
        #endregion
        #region Functions
        /// <summary>
        /// Returns False if
        /// <para>Chance is <= 0</para>
        /// <para>Frequency is <= 0 or > 20 min</para>
        /// <para>Duration <= 0 or > 20 sec</para>
        /// </summary>
        public bool Validate {
            get {
                if (Chance <= 0) { return false; }
                if (Frequency <= 0 || Frequency > 20 * 60) { return false; }
                if (Duration <= 0 || Duration > 20 * 1000) { return false; }
                return true;
            }
        }
        public override string ToString() {
            if (!Validate) { return "None"; }
            return string.Format("N: {5} : F: {0:0}s D: {1:0}ms C: {2:0.0%}{3} : {4}",
                Frequency, Duration, Chance, Breakable ? " : B" : "", Stats, Name);
        }
        #endregion
    }
    #endregion
}
