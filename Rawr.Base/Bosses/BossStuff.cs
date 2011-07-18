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
    /// <summary>The role of the player, will allow certain lists to return filtered to things that affect said role</summary>
    public enum PLAYER_ROLES { MainTank = 0, OffTank, TertiaryTank, MeleeDPS, RangedDPS, MainTankHealer, OffAndTertTankHealer, RaidHealer }
    /// <summary>Enumerator for creating a list of possible values for the Level box</summary>
    public enum POSSIBLE_LEVELS { LVLP0 = 85, LVLP1 = 86, LVLP2 = 87, LVLP3 = 88, }
    /// <summary>Enumerator for creating a list of possible values for the Mob Type box</summary>
    public enum MOB_TYPES { BEAST = 0, DEMON, DRAGONKIN, ELEMENTAL, GIANT, HUMANOID, MECHANICAL, UNDEAD }
    public enum ImpedanceTypes { Fear, Root, Stun, Move, Silence, Disarm };
    /// <summary>Enumerator for attack types, this mostly is for raid members that aren't being directly attacked to know when AoE damage is coming from the boss</summary>
    public enum ATTACK_TYPES { AT_MELEE = 0, AT_RANGED, AT_AOE, AT_DOT }
    [Flags]
    public enum AVOIDANCE_TYPES
    {
        None = 0,
        Miss = 1,
        Dodge = 2,
        Parry = 4,
        Block = 8,
    }
    /// <summary>A single Attack of various types</summary>
    public class Attack {
        public Attack Clone() {
            Attack clone = (Attack)this.MemberwiseClone();
            return clone;
        }
        /// <summary>The Name of the Attack</summary>
        public string Name = "Unnamed";
        /// <summary>The Spell ID of the attack</summary>
        [DefaultValue(0)]
        public float SpellID = 0;
        /// <summary>The type of damage done, use the ItemDamageType enumerator to select</summary>
        [DefaultValue(ItemDamageType.Physical)]
        public ItemDamageType DamageType = ItemDamageType.Physical;
        /// <summary>This is so you can pull the Default Melee Attack more easily</summary>
        [DefaultValue(false)]
        public bool IsTheDefaultMelee = false;
        /// <summary>
        /// The Unmitigated Damage per Hit for this attack, 5000f is 5,000 Raw Unmitigated Damage.<br/>
        /// When DamageIsPerc is true DamagePerHit = 0.75f; would be 75% of Player's Health<br/>
        /// When this is a DoT is true DamagePerHit is just the Initial Damage. The Tick contains the remaining Damage<br/>
        /// </summary>
        [DefaultValue(0)]
        public float DamagePerHit = 0;
        /// <summary>When set to True, DamagePerHit will be seen as a Percentage. DamagePerHit = 0.75f; would be 75% of Player's Health</summary>
        [DefaultValue(false)]
        public bool DamageIsPerc = false;
        /// <summary>The maximum number of party/raid members this attack can hit</summary>
        [DefaultValue(1f)]
        public float MaxNumTargets = 1f;
        /// <summary>The frequency of this attack (in seconds)</summary>
        [DefaultValue(2.0f)]
        public float AttackSpeed = 2.0f;
        /// <summary>The Attack Type (for AoE vs. single-target Melee/Ranged)</summary>
        [DefaultValue(ATTACK_TYPES.AT_MELEE)]
        public ATTACK_TYPES AttackType = ATTACK_TYPES.AT_MELEE;
        /// <summary>If the attack is part of a Dual Wield, there is an additional 20% chance to Miss<para>This should flag the Buff... sort of</para></summary>
        [DefaultValue(false)]
        public bool IsDualWielding = false;
        /// <summary>If the attack is coming from Adds, that affects the Special Boss calcs</summary>
        [DefaultValue(false)]
        public bool IsFromAnAdd = false;

        #region Phase Info
        public SerializableDictionary<int, float[]> PhaseTimes {
            get {
                if (_phaseTimes == null) {
                    _phaseTimes = new SerializableDictionary<int,float[]>() {
                        { 1, new float[] { 0f, 20f*60f }}, // Default of only one phase in the fight which lasts the maximumum BerserkTimer value
                    };
                }
                return _phaseTimes;
            }
            set { _phaseTimes = value; }
        }
        private SerializableDictionary<int, float[]> _phaseTimes = null;
        public void RemovePhase1Values() { PhaseTimes.Remove(1); }
        public void SetPhaseValue(int phaseNumber, float phaseStartTime, float phaseDuration, float fightDuration)
        {
            FightDuration = fightDuration;
            PhaseTimes[phaseNumber] = new float[] { Math.Min(phaseStartTime, FightDuration), Math.Min(FightDuration, phaseStartTime + phaseDuration) };
            _fightUptimePercent = -1f;
        }
        /// <summary>Pass in the BerserkTimer, this is for % of Fight Uptime calcs</summary>
        [DefaultValue(20 * 60)]
        public float FightDuration { get { return _fightDuration; } set { _fightDuration = value; if (_fightDuration > 20 * 60) { _fightDuration = 20 * 60; } _fightUptimePercent = -1f; } }
        private float _fightDuration = 20 * 60;
        public float FightUptimePercent
        {
            get
            {
                if (FightDuration <= 0f) { return 0f; }
                if (_fightUptimePercent == -1f)
                {
                    float uptime = 0f;
                    foreach (float[] i in PhaseTimes.Values)
                    {
                        uptime += i[1] - i[0];
                    }
                    _fightUptimePercent = Math.Max(0f, Math.Min(uptime, FightDuration) / FightDuration);
                }
                return _fightUptimePercent;
            }
        }
        private float _fightUptimePercent = -1f;
        #endregion

        #region DoT Stats
        /// <summary>Whether this attack is a DoT</summary>
        [DefaultValue(false)]
        public bool IsDoT = false;
        /// <summary>
        /// The over time damage of the dot, separate from the initial portion<br/>
        /// Calculated as Damage Divided by Number of Ticks<br/>
        /// Example: 20000f/8f
        /// </summary>
        [DefaultValue(0)]
        public float DamagePerTick { get { return IsDoT ? _damagePerTick : 0; } set { _damagePerTick = value; } }
        private float _damagePerTick = 0f;//20f*1000f/8f;
        /// <summary>Interval of the ticks, 1 = 1 sec. 0 if not a DoT</summary>
        [DefaultValue(0)]
        public float TickInterval { get { return IsDoT ? _tickInterval : 0; } set { _tickInterval = value; } }
        private float _tickInterval = 0;
        /// <summary>The full duration of the DoT, 2 sec Interval * 5 Ticks = 10 sec duration</summary>
        [DefaultValue(0)]
        public float Duration { get { return IsDoT ? _duration : 0; } set { _duration = value; } }
        private float _duration = 0f;

        /// <summary>The total number of Ticks, 0 if not a DoT</summary>
        public float NumTicks { get { return IsDoT ? Duration / TickInterval : 0; } }
        /// <summary>Total of the Intitial Damage and the Damage over Time</summary>
        public float TotalDoTDamage { get { return IsDoT ? DamagePerHit + NumTicks * DamagePerTick : 0; } }
        #endregion

        #region Player Avoidance
        /// <summary>Missable = Dodgable = Parryable = Blockable = false</summary>
        public void SetUnavoidable() { _AvoidanceFlags = AVOIDANCE_TYPES.None; }
        /// <summary>Returns True if any of the Avoidance types are true</summary>
        public bool Avoidable { get { return Missable || Dodgable || Parryable || Blockable; } }
        /// <summary>
        /// Returns True if ALL of the Specified avoidance types are true.
        /// But not limited to just the specified avoidance types. 
        /// EG: if you want all Blockable then pass in AVOIDANCE_TYPE.Block.
        /// It will return true for Blockable no matter the value of Miss,Parry,Dodge.
        /// </summary>
        /// <param name="AvoidanceFlags">Mask for ALL Types you want to be valid.</param>
        /// <returns></returns>
        public bool AvoidableBy(AVOIDANCE_TYPES AvoidanceFlags)
        {
            if (AvoidanceFlags == AVOIDANCE_TYPES.None && _AvoidanceFlags != AVOIDANCE_TYPES.None) return false;
            else return (_AvoidanceFlags & AvoidanceFlags) == AvoidanceFlags;
        }
        [DefaultValue(AVOIDANCE_TYPES.Block | AVOIDANCE_TYPES.Dodge | AVOIDANCE_TYPES.Miss | AVOIDANCE_TYPES.Parry)]
        private AVOIDANCE_TYPES _AvoidanceFlags = AVOIDANCE_TYPES.Block | AVOIDANCE_TYPES.Dodge | AVOIDANCE_TYPES.Miss | AVOIDANCE_TYPES.Parry;
        /// <summary>Can this attack Miss the player</summary>
        public bool Missable
        {
            get { return (_AvoidanceFlags & AVOIDANCE_TYPES.Miss) == AVOIDANCE_TYPES.Miss; }
            set
            {
                if (value == true) { _AvoidanceFlags |= AVOIDANCE_TYPES.Miss; }
                else { _AvoidanceFlags ^= AVOIDANCE_TYPES.Miss; }
            }
        }
        /// <summary>Can this attack be Dodged by the player</summary>
        public bool Dodgable
        {
            get { return (_AvoidanceFlags & AVOIDANCE_TYPES.Dodge) == AVOIDANCE_TYPES.Dodge; }
            set
            {
                if (value == true) { _AvoidanceFlags |= AVOIDANCE_TYPES.Dodge; }
                else { _AvoidanceFlags ^= AVOIDANCE_TYPES.Dodge; }
            }
        }
        /// <summary>Can this attack be Parried by the player</summary>
        public bool Parryable
        {
            get { return (_AvoidanceFlags & AVOIDANCE_TYPES.Parry) == AVOIDANCE_TYPES.Parry; }
            set
            {
                if (value == true) { _AvoidanceFlags |= AVOIDANCE_TYPES.Parry; }
                else { _AvoidanceFlags ^= AVOIDANCE_TYPES.Parry; }
            }
        }
        /// <summary>Can this attack be Blocked by the player</summary>
        public bool Blockable
        {
            get { return (_AvoidanceFlags & AVOIDANCE_TYPES.Block) == AVOIDANCE_TYPES.Block; }
            set
            {
                if (value == true) { _AvoidanceFlags |= AVOIDANCE_TYPES.Block; }
                else { _AvoidanceFlags ^= AVOIDANCE_TYPES.Block; }
            }
        }
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
        public void SetAffectsRoles_All() {
            AffectsRole[PLAYER_ROLES.MainTank]
                = AffectsRole[PLAYER_ROLES.OffTank]
                = AffectsRole[PLAYER_ROLES.TertiaryTank]
                = AffectsRole[PLAYER_ROLES.MeleeDPS]
                = AffectsRole[PLAYER_ROLES.RangedDPS]
                = AffectsRole[PLAYER_ROLES.MainTankHealer]
                = AffectsRole[PLAYER_ROLES.OffAndTertTankHealer]
                = AffectsRole[PLAYER_ROLES.RaidHealer]
                = true;
        }
        public void SetAffectsRoles_Tanks() {
            AffectsRole[PLAYER_ROLES.MainTank]
                = AffectsRole[PLAYER_ROLES.OffTank]
                = AffectsRole[PLAYER_ROLES.TertiaryTank]
                = true;
        }
        public void SetAffectsRoles_DPS() {
            AffectsRole[PLAYER_ROLES.MeleeDPS]
                = AffectsRole[PLAYER_ROLES.RangedDPS]
                = true;
        }
        public void SetAffectsRoles_Healers() {
            AffectsRole[PLAYER_ROLES.MainTankHealer]
                = AffectsRole[PLAYER_ROLES.OffAndTertTankHealer]
                = AffectsRole[PLAYER_ROLES.RaidHealer]
                = true;
        }
        #endregion
        #region Player Negation
        [DefaultValue(false)]
        public bool Interruptable = false;
        #endregion

        /// <summary>
        /// Returns False if
        /// <para>Chance is <= 0</para>
        /// <para>Frequency is <= 0 or > 20 min</para>
        /// <para>Duration <= 0 or > 20 sec</para>
        /// </summary>
        public bool Validate {
            get {
                // If it's Named invalid... it's for a reason
                if (Name == "Invalid") { return false; }
                // Can't have a negative attack speed or an attack speed greater than berserk timer
                if (AttackSpeed <= 0 || AttackSpeed > 20 * 60) { return false; }
                // Damage Per Hit (and dot damage) in total are negative
                if ((DamagePerHit + (DamagePerTick*NumTicks)) <= 0) { return false; }
                // Has to affect someone
                bool anyroles = false;
                foreach (bool b in AffectsRole.Values) { if (b) { anyroles = true; break; } }
                if (!anyroles) { return false; }
                // Didn't fail, must be valid
                return true;
            }
        }
        /// <summary>Returns the reason it is invalid</summary>
        public string ValidateString
        {
            get
            {
                // If it's Named invalid... it's for a reason
                if (Name == "Invalid") { return "This attack was specifically marked invalid"; }
                // Can't have a negative frequency and can't be more than the zerk timer
                if (AttackSpeed <= 0) { return "Negative Attack Speed value"; }
                if (AttackSpeed > 20 * 60) { return "Attack Speed is above Berserk timer"; }
                // Damage Per Hit (and dot damage) in total are negative
                if ((DamagePerHit + (DamagePerTick * NumTicks)) <= 0) { return "Total Damage is negative"; }
                // Has to affect someone
                bool anyroles = false;
                foreach (bool b in AffectsRole.Values) { if (b) { anyroles = true; break; } }
                if (!anyroles) { return "It is not affecting any Player Roles"; }
                // Didn't fail, must be valid
                return "";
            }
        }
        public override string ToString()
        {
            if (!Validate) { return Name + " is Invalid: " + ValidateString; }
            string damage = "";
            if (DamagePerHit != 0 &&  DamageIsPerc) { damage += string.Format("{0:0.##%}", DamagePerHit); }
            if (DamagePerHit != 0 && !DamageIsPerc) { damage += string.Format("{0:#,##0}", DamagePerHit); }
            if (DamagePerTick != 0 && NumTicks != 0) { damage += string.Format("{3}{0:#,##0}/{1:0.#}s(x{2:0.#})", DamagePerTick, TickInterval, NumTicks, damage != "" ? "+" : ""); }
            return string.Format("{0} every {1:0.#}s to {2:0} targets{3}{4}",
                damage, AttackSpeed, MaxNumTargets,
                Name != "Dynamic" ? " [" + Name + "]" : "",
                string.Format(" {0:0.#%} phase uptime", FightUptimePercent));
        }
    }
    /// <summary>A single Impedance of various types</summary>
    public class Impedance {
        #region Constructors
        public Impedance() {
            Frequency = 120f;
            Duration  = 5 * 1000f;
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
        public Impedance Clone()
        {
            Impedance clone = (Impedance)this.MemberwiseClone();
            return clone;
        }
        #endregion
        #region Variables
        /// <summary>The Name of the Attack</summary>
        public string Name = "Unnamed";
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
        #region Phase Info
       public SerializableDictionary<int, float[]> PhaseTimes
        {
            get
            {
                if (_phaseTimes == null)
                {
                    _phaseTimes = new SerializableDictionary<int, float[]>() {
                        { 1, new float[] { 0f, 20f*60f }}, // Default of only one phase in the fight which lasts the maximumum BerserkTimer value
                    };
                }
                return _phaseTimes;
            }
            set { _phaseTimes = value; _fightUptimePercent = -1f; }
        }
        private SerializableDictionary<int, float[]> _phaseTimes = null;
        public void RemovePhase1Values() { PhaseTimes.Remove(1); }
        public void SetPhaseValue(int phaseNumber, float phaseStartTime, float phaseDuration, float fightDuration) {
            FightDuration = fightDuration;
            PhaseTimes[phaseNumber] = new float[] { Math.Min(phaseStartTime, FightDuration), Math.Min(FightDuration, phaseStartTime + phaseDuration) };
            _fightUptimePercent = -1f;
        }
        /// <summary>Pass in the BerserkTimer, this is for % of Fight Uptime calcs</summary>
        [DefaultValue(20*60)]
        public float FightDuration { get { return _fightDuration; } set { _fightDuration = value; if (_fightDuration > 20 * 60) { _fightDuration = 20 * 60; } _fightUptimePercent = -1f; } }
        private float _fightDuration = 20 * 60;
        public float FightUptimePercent {
            get {
                if (FightDuration <= 0f) { return 0f; }
                if (_fightUptimePercent == -1f) {
                    float uptime = 0f;
                    foreach (float[] i in PhaseTimes.Values)
                    {
                        uptime += i[1] - i[0];
                    }
                    _fightUptimePercent = Math.Max(0f, Math.Min(uptime, FightDuration) / FightDuration);
                }
                return _fightUptimePercent;
            }
        }
        private float _fightUptimePercent = -1f;
        #endregion
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
                // Has to at least have some chance of happening
                if (Chance <= 0) { return false; }
                // Can't have a negative frequency and can't be more than the zerk timer
                if (Frequency <= 0 || Frequency > 20 * 60) { return false; }
                // Can't have a negative duration and can't be greater than zerk timer, modifying for the phase uptime
                if (Duration <= 0 || (Duration * FightUptimePercent) > 20 * 60 * 1000) { return false; }
                // Has to affect someone
                bool anyroles = false;
                foreach (bool b in AffectsRole.Values) { if (b) { anyroles = true; break; } }
                if (!anyroles) { return false; }
                // Didn't fail, must be valid
                return true;
            }
        }
        /// <summary>Returns the reason it is invalid</summary>
        public string ValidateString
        {
            get {
                // Has to at least have some chance of happening
                if (Chance <= 0) { return "Negative Chance value"; }
                // Can't have a negative frequency and can't be more than the zerk timer
                if (Frequency <= 0) { return "Negative Frequency value"; }
                if (Frequency > 20 * 60) { return "Frequency is above Berserk timer"; }
                // Can't have a negative duration and can't be greater than zerk timer, modifying for the phase uptime
                if (Duration <= 0) { return "Negative Duration value"; }
                if ((Duration * FightUptimePercent) > 20 * 60 * 1000) { return "Duration is above Berserk Timer"; }
                // Has to affect someone
                bool anyroles = false;
                foreach (bool b in AffectsRole.Values) { if (b) { anyroles = true; break; } }
                if (!anyroles) { return "It is not affecting any Player Roles"; }
                // Didn't fail, must be valid
                return "";
            }
        }
        public override string ToString() {
            if (!Validate) { return Name + " is Invalid: " + ValidateString; }
            return string.Format("{0:0.#%} chance every {1:0.#}s for {2:0.##}s{3}{4}{5}",
                Chance, Frequency, Duration / 1000f, Breakable ? ", Breakable" : "",
                Name != "Dynamic" ? " [" + Name + "]" : "",
                string.Format(" {0:0.#%} phase uptime", FightUptimePercent));
        }
        #endregion
        #region Player Targeting
        private SerializableDictionary<PLAYER_ROLES, bool> _affectsRole = null;
        public SerializableDictionary<PLAYER_ROLES, bool> AffectsRole
        {
            get
            {
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
        public void SetAffectsRoles_All() {
            AffectsRole[PLAYER_ROLES.MainTank]
                = AffectsRole[PLAYER_ROLES.OffTank]
                = AffectsRole[PLAYER_ROLES.TertiaryTank]
                = AffectsRole[PLAYER_ROLES.MeleeDPS]
                = AffectsRole[PLAYER_ROLES.RangedDPS]
                = AffectsRole[PLAYER_ROLES.MainTankHealer]
                = AffectsRole[PLAYER_ROLES.OffAndTertTankHealer]
                = AffectsRole[PLAYER_ROLES.RaidHealer]
                = true;
        }
        public void SetAffectsRoles_Tanks() {
            AffectsRole[PLAYER_ROLES.MainTank]
                = AffectsRole[PLAYER_ROLES.OffTank]
                = AffectsRole[PLAYER_ROLES.TertiaryTank]
                = true;
        }
        public void SetAffectsRoles_DPS() {
            AffectsRole[PLAYER_ROLES.MeleeDPS]
                = AffectsRole[PLAYER_ROLES.RangedDPS]
                = true;
        }
        public void SetAffectsRoles_Healers() {
            AffectsRole[PLAYER_ROLES.MainTankHealer]
                = AffectsRole[PLAYER_ROLES.OffAndTertTankHealer]
                = AffectsRole[PLAYER_ROLES.RaidHealer]
                = true;
        }
        #endregion

        /// <summary>
        /// Generates the time lost to this Impedance type.
        /// </summary>
        /// <param name="bossOpts">Pass character.BossOptions</param>
        /// <param name="role">The role of the character</param>
        /// <param name="type">The Type to check:<para>0: Moves</para><para>1: Fears</para><para>2: Stuns</para><para>3: Roots</para><para>4: Silence</para></param>
        /// <param name="breakingMOD">% reduction of duration</param>
        /// <param name="breakingCD">CD of the breaking ability</param>
        /// <returns>Returns the Percentage of time lost to this Impedance type.
        /// <para>This is limited to 0%-100% to prevent wierd calc issues</para></returns>
        public static float GetImpedancePerc(BossOptions bossOpts, PLAYER_ROLES role, int type, float breakingMOD, float breakingCD=0)
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
            float timeIn = 0;
            foreach (Impedance i in imps) {
                if (i.AffectsRole[role])
                {
                    float acts = i.Chance * i.FightUptimePercent * bossOpts.BerserkTimer / i.Frequency;
                    float breakedActs = acts * (i.Breakable && breakingCD > 0 ? Math.Min(i.Frequency / breakingCD, 1f) : 0f);
                    acts -= breakedActs;
                    timeIn += acts * i.Duration / 1000f * (i.Breakable ? (1f - breakingMOD) : 1f); // full duration
                    timeIn += breakedActs * 3f; // breaked ones: 1.5 to react, 1.5 GCD 
                }
            }
            // Convert this to a Percentage
            return Math.Max(0f, Math.Min(1f, timeIn / bossOpts.BerserkTimer));
        }

        /// <summary>
        /// Generates the time lost to all Impedance types.
        /// </summary>
        /// <param name="bossOpts">Pass character.BossOptions</param>
        /// <param name="role">The role of the character</param>
        /// <param name="moveBreakingMOD">The modifier for this type, eg- Type is Moves, pass stats.MovementSpeed</param>
        /// <param name="fearBreakingMOD">The modifier for this type, eg- Type is Fears, pass stats.FearDurReduc</param>
        /// <param name="stunBreakingMOD">The modifier for this type, eg- Type is Stuns, pass stats.StunDurReduc</param>
        /// <param name="rootBreakingMOD">The modifier for this type, eg- Type is Roots, pass stats.SnareRootDurReduc</param>
        /// <param name="silenceBreakingMOD">The modifier for this type, eg- Type is Silences, pass stats.SilenceDurReduc</param>
        /// <param name="moveBreakingCD">CD of the move breaking ability in seconds</param>
        /// <param name="fearBreakingCD">CD of the fear breaking ability in seconds</param>
        /// <param name="stunBreakingCD">CD of the stun breaking ability in seconds</param>
        /// <param name="rootBreakingCD">CD of the root breaking ability in seconds</param>
        /// <param name="silenceBreakingCD">CD of the silence breaking ability in seconds</param>
        /// <returns>Returns the Percentage of time lost to all Impedance types.
        /// <para>This is limited to 0%-100% to prevent wierd calc issues</para></returns>
        public static float GetTotalImpedancePercs(BossOptions bossOpts, PLAYER_ROLES role, float moveBreakingMOD, float fearBreakingMOD, float stunBreakingMOD, float rootBreakingMOD, float silenceBreakingMOD,
            float moveBreakingCD = 0, float fearBreakingCD = 0, float stunBreakingCD = 0, float rootBreakingCD = 0, float silenceBreakingCD = 0)
        {
            return GetImpedancePerc(bossOpts, role, 0, moveBreakingMOD, moveBreakingCD) +
                   GetImpedancePerc(bossOpts, role, 1, fearBreakingMOD, fearBreakingCD) +
                   GetImpedancePerc(bossOpts, role, 2, stunBreakingMOD, stunBreakingCD) +
                   GetImpedancePerc(bossOpts, role, 3, rootBreakingMOD, rootBreakingCD) +
                   GetImpedancePerc(bossOpts, role, 4, silenceBreakingMOD, silenceBreakingCD);
        }
    }
    public struct ImpedanceWithType
    {
        public Impedance imp;
        public ImpedanceTypes type;
    }
    /// <summary>A single Target Group</summary>
    public class TargetGroup
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
        public TargetGroup Clone()
        {
            TargetGroup clone = (TargetGroup)this.MemberwiseClone();
            return clone;
        }
        #endregion
        #region Variables
        /// <summary>The Name of the Target Group</summary>
        public string Name = "Unnamed";
        /// <summary>Target mobs IDs</summary>
        [DefaultValue(0)]
        public float TargetID = 0;
        /// <summary>In Seconds<para>Defaults to -1 as an 'invalid' flag</para></summary>
        public float Frequency = -1;
        /// <summary>In MilliSeconds (1/1000 of a second)<para>Defaults to 20 seconds</para></summary>
        public float Duration = 20 * 1000;
        /// <summary>Percentage, 0.50f = 50% Chance that this occurs<para>Defaults to and almost every time in usage this should be 1.00f=100%</para></summary>
        public float Chance = 1.00f;
        #region Phase Info
        public SerializableDictionary<int, float[]> PhaseTimes
        {
            get
            {
                if (_phaseTimes == null)
                {
                    _phaseTimes = new SerializableDictionary<int, float[]>() {
                        { 1, new float[] { 0f, 20f*60f }}, // Default of only one phase in the fight which lasts the maximumum BerserkTimer value
                    };
                }
                return _phaseTimes;
            }
            set { _phaseTimes = value; }
        }
        private SerializableDictionary<int, float[]> _phaseTimes = null;
        public void RemovePhase1Values() { PhaseTimes.Remove(1); }
        public void SetPhaseValue(int phaseNumber, float phaseStartTime, float phaseDuration, float fightDuration)
        {
            FightDuration = fightDuration;
            PhaseTimes[phaseNumber] = new float[] { Math.Min(phaseStartTime, FightDuration), Math.Min(FightDuration, phaseStartTime + phaseDuration) };
            _fightUptimePercent = -1f;
        }
        /// <summary>Pass in the BerserkTimer, this is for % of Fight Uptime calcs</summary>
        [DefaultValue(20 * 60)]
        public float FightDuration { get { return _fightDuration; } set { _fightDuration = value; if (_fightDuration > 20 * 60) { _fightDuration = 20 * 60; } _fightUptimePercent = -1f; } }
        private float _fightDuration = 20 * 60;
        public float FightUptimePercent
        {
            get
            {
                if (FightDuration <= 0f) { return 0f; }
                if (_fightUptimePercent == -1f)
                {
                    float uptime = 0f;
                    foreach (float[] i in PhaseTimes.Values)
                    {
                        uptime += i[1] - i[0];
                    }
                    _fightUptimePercent = Math.Max(0f, Math.Min(uptime, FightDuration) / FightDuration);
                }
                return _fightUptimePercent;
            }
        }
        private float _fightUptimePercent = -1f;
        #endregion
        /// <summary>The Number of Targets in this Target Group<br/>Defaults to 2</summary>
        public float NumTargs = 2;
        /// <summary>
        /// If the mobs are near boss then your MultiTargets calculations should also count the boss,
        /// otherwise this is a group that must be DPS'd by itself.
        /// <para>Defaults to false</para>
        /// </summary>
        public bool NearBoss = false;
        /// <summary>Level of the additional Targets, 85-88</summary>
        public int LevelOfTargets = (int)POSSIBLE_LEVELS.LVLP2;
        #endregion
        #region Player Targeting
        private SerializableDictionary<PLAYER_ROLES, bool> _affectsRole = null;
        public SerializableDictionary<PLAYER_ROLES, bool> AffectsRole
        {
            get
            {
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
        public void SetAffectsRoles_All() {
            AffectsRole[PLAYER_ROLES.MainTank]
                = AffectsRole[PLAYER_ROLES.OffTank]
                = AffectsRole[PLAYER_ROLES.TertiaryTank]
                = AffectsRole[PLAYER_ROLES.MeleeDPS]
                = AffectsRole[PLAYER_ROLES.RangedDPS]
                = AffectsRole[PLAYER_ROLES.MainTankHealer]
                = AffectsRole[PLAYER_ROLES.OffAndTertTankHealer]
                = AffectsRole[PLAYER_ROLES.RaidHealer]
                = true;
        }
        public void SetAffectsRoles_Tanks() {
            AffectsRole[PLAYER_ROLES.MainTank]
                = AffectsRole[PLAYER_ROLES.OffTank]
                = AffectsRole[PLAYER_ROLES.TertiaryTank]
                = true;
        }
        public void SetAffectsRoles_DPS() {
            AffectsRole[PLAYER_ROLES.MeleeDPS]
                = AffectsRole[PLAYER_ROLES.RangedDPS]
                = true;
        }
        public void SetAffectsRoles_Healers() {
            AffectsRole[PLAYER_ROLES.MainTankHealer]
                = AffectsRole[PLAYER_ROLES.OffAndTertTankHealer]
                = AffectsRole[PLAYER_ROLES.RaidHealer]
                = true;
        }
        #endregion
        #region Functions
        public float GetAverageTargetGroupSize(float fightDuration) {
            if (!Validate) { return 0f; }
            return new SpecialEffect(Trigger.Use, null, Duration / 1000f, Frequency, Chance, (int)NumTargs).GetAverageStackSize(0, 1f, 3, fightDuration);
        }
        /// <summary>
        /// Returns False if
        /// <para>Chance is <= 0</para>
        /// <para>Frequency is <= 0 or > 20 min</para>
        /// <para>Duration <= 0 or > 20 sec</para>
        /// </summary>
        public bool Validate {
            get {
                // Has to at least have some chance of happening
                if (Chance <= 0) { return false; }
                // Can't have a negative frequency and can't be more than the zerk timer
                if (Frequency <= 0 || Frequency > 20 * 60) { return false; }
                // Can't have a negative duration and can't be greater than zerk timer, modifying for the phase uptime
                if (Duration <= 0 || (Duration * FightUptimePercent) > 20 * 60 * 1000) { return false; }
                // Has to affect someone
                bool anyroles = false;
                foreach (bool b in AffectsRole.Values) { if (b) { anyroles = true; break; } }
                if (!anyroles) { return false; }
                // Didn't fail, must be valid
                return true;
            }
        }
        /// <summary>Returns the reason it is invalid</summary>
        public string ValidateString {
            get {
                // Has to at least have some chance of happening
                if (Chance <= 0) { return "Negative Chance value"; }
                // Can't have a negative frequency and can't be more than the zerk timer
                if (Frequency <= 0) { return "Negative Frequency value"; }
                if (Frequency > 20 * 60) { return "Frequency is above Berserk timer"; }
                // Can't have a negative duration and can't be greater than zerk timer, modifying for the phase uptime
                if (Duration <= 0) { return "Negative Duration value"; }
                if ((Duration * FightUptimePercent) > 20 * 60 * 1000) { return "Duration is above Berserk Timer"; }
                // Has to affect someone
                bool anyroles = false;
                foreach (bool b in AffectsRole.Values) { if (b) { anyroles = true; break; } }
                if (!anyroles) { return "It is not affecting any Player Roles"; }
                // Didn't fail, must be valid
                return "";
            }
        }
        public override string ToString()
        {
            if (!Validate) { return Name + " is Invalid: " + ValidateString; }
            return string.Format("{0:0.#%} chance of {1:0.##} Lvl {2} targs{3} every {4:0.#}s for {5:0.##}s{6}{7}",
                Chance, NumTargs, LevelOfTargets, NearBoss ? " Near Boss" : "", Frequency, Duration / 1000f,
                Name != "Dynamic" ? " [" + Name + "]" : "",
                string.Format(" {0:0.#%} phase uptime", FightUptimePercent));
        }
        #endregion
    }
    /// <summary>A single Buff the Boss places on the raid</summary>
    public class BuffState
    {
        #region Constructors
        public BuffState() {
            Name = "Unnamed";
            Frequency = 120f;
            Duration  = 5f * 1000f;
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
        public BuffState Clone()
        {
            BuffState clone = (BuffState)this.MemberwiseClone();
            return clone;
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
        public float Frequency = 45f;
        /// <summary>
        /// The amount of time spent in this state, in millisec
        /// <para>Eg- This BuffState lasts 4 seconds:</para>
        /// <para>Duration = 4f * 1000f</para>
        /// </summary>
        public float Duration = 5f * 1000f;

        #region Phase Info
        public SerializableDictionary<int, float[]> PhaseTimes
        {
            get
            {
                if (_phaseTimes == null)
                {
                    _phaseTimes = new SerializableDictionary<int, float[]>() {
                        { 1, new float[] { 0f, 20f*60f }}, // Default of only one phase in the fight which lasts the maximumum BerserkTimer value
                    };
                }
                return _phaseTimes;
            }
            set { _phaseTimes = value; }
        }
        private SerializableDictionary<int, float[]> _phaseTimes = null;
        public void RemovePhase1Values() { PhaseTimes.Remove(1); }
        public void SetPhaseValue(int phaseNumber, float phaseStartTime, float phaseDuration, float fightDuration)
        {
            FightDuration = fightDuration;
            PhaseTimes[phaseNumber] = new float[] { Math.Min(phaseStartTime, FightDuration), Math.Min(FightDuration, phaseStartTime + phaseDuration) };
            _fightUptimePercent = -1f;
        }
        /// <summary>Pass in the BerserkTimer, this is for % of Fight Uptime calcs</summary>
        [DefaultValue(20 * 60)]
        public float FightDuration { get { return _fightDuration; } set { _fightDuration = value; if (_fightDuration > 20 * 60) { _fightDuration = 20 * 60; } _fightUptimePercent = -1f; } }
        private float _fightDuration = 20 * 60;
        public float FightUptimePercent
        {
            get
            {
                if (FightDuration <= 0f) { return 0f; }
                if (_fightUptimePercent == -1f)
                {
                    float uptime = 0f;
                    foreach (float[] i in PhaseTimes.Values)
                    {
                        uptime += i[1] - i[0];
                    }
                    _fightUptimePercent = Math.Max(0f, Math.Min(uptime, FightDuration) / FightDuration);
                }
                return _fightUptimePercent;
            }
        }
        private float _fightUptimePercent = -1f;
        #endregion

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
        #region Player Targeting
        private SerializableDictionary<PLAYER_ROLES, bool> _affectsRole = null;
        public SerializableDictionary<PLAYER_ROLES, bool> AffectsRole
        {
            get
            {
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
        public void SetAffectsRoles_All() {
            AffectsRole[PLAYER_ROLES.MainTank]
                = AffectsRole[PLAYER_ROLES.OffTank]
                = AffectsRole[PLAYER_ROLES.TertiaryTank]
                = AffectsRole[PLAYER_ROLES.MeleeDPS]
                = AffectsRole[PLAYER_ROLES.RangedDPS]
                = AffectsRole[PLAYER_ROLES.MainTankHealer]
                = AffectsRole[PLAYER_ROLES.OffAndTertTankHealer]
                = AffectsRole[PLAYER_ROLES.RaidHealer]
                = true;
        }
        public void SetAffectsRoles_Tanks() {
            AffectsRole[PLAYER_ROLES.MainTank]
                = AffectsRole[PLAYER_ROLES.OffTank]
                = AffectsRole[PLAYER_ROLES.TertiaryTank]
                = true;
        }
        public void SetAffectsRoles_DPS() {
            AffectsRole[PLAYER_ROLES.MeleeDPS]
                = AffectsRole[PLAYER_ROLES.RangedDPS]
                = true;
        }
        public void SetAffectsRoles_Healers() {
            AffectsRole[PLAYER_ROLES.MainTankHealer]
                = AffectsRole[PLAYER_ROLES.OffAndTertTankHealer]
                = AffectsRole[PLAYER_ROLES.RaidHealer]
                = true;
        }
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
                // Has to at least have some chance of happening
                if (Chance <= 0) { return false; }
                // Can't have a negative frequency and can't be more than the zerk timer
                if (Frequency <= 0 || Frequency > 20 * 60) { return false; }
                // Can't have a negative duration and can't be greater than zerk timer, modifying for the phase uptime
                if (Duration <= 0 || (Duration * FightUptimePercent) > 20 * 60 * 1000) { return false; }
                // Has to affect someone
                bool anyroles = false;
                foreach (bool b in AffectsRole.Values) { if (b) { anyroles = true; break; } }
                if (!anyroles) { return false; }
                // Didn't fail, must be valid
                return true;
            }
        }
        /// <summary>Returns the reason it is invalid</summary>
        public string ValidateString
        {
            get {
                // Has to at least have some chance of happening
                if (Chance <= 0) { return "Negative Chance value"; }
                // Can't have a negative frequency and can't be more than the zerk timer
                if (Frequency <= 0) { return "Negative Frequency value"; }
                if (Frequency > 20 * 60) { return "Frequency is above Berserk timer"; }
                // Can't have a negative duration and can't be greater than zerk timer, modifying for the phase uptime
                if (Duration <= 0) { return "Negative Duration value"; }
                if ((Duration * FightUptimePercent) > 20 * 60 * 1000) { return "Duration is above Berserk Timer"; }
                // Has to affect someone
                bool anyroles = false;
                foreach (bool b in AffectsRole.Values) { if (b) { anyroles = true; break; } }
                if (!anyroles) { return "It is not affecting any Player Roles"; }
                // Didn't fail, must be valid
                return "";
            }
        }
        public override string ToString() {
            if (!Validate) { return Name + " is Invalid: " + ValidateString; }
            return string.Format("{0:0.#%} chance of {1} every {2:0.#}s for {3:0.##}s{4}{5}{6}",
                Chance, Stats, Frequency, Duration / 1000f, Breakable ? ", Breakable" : "",
                Name != "Dynamic" ? " [" + Name + "]" : "",
                string.Format(" {0:0.#%} phase uptime", FightUptimePercent));
        }
        #endregion
    }

    public class Phase
    {
        public string Name = "Phase";
        //
        public Phase Clone() {
            Phase clone = (Phase)this.MemberwiseClone();
            //
            clone.Attacks    = new List<Attack>(); foreach (Attack i in this.Attacks) { clone.Attacks.Add(i.Clone()); }
            clone.Targets    = new List<TargetGroup>(); foreach (TargetGroup i in this.Targets) { clone.Targets.Add(i.Clone()); }
            clone.BuffStates = new List<BuffState>(); foreach (BuffState i in this.BuffStates) { clone.BuffStates.Add(i.Clone()); }
            clone.Moves      = new List<Impedance>(); foreach (Impedance i in this.Moves) { clone.Moves.Add(i.Clone()); }
            clone.Stuns      = new List<Impedance>(); foreach (Impedance i in this.Stuns) { clone.Stuns.Add(i.Clone()); }
            clone.Fears      = new List<Impedance>(); foreach (Impedance i in this.Fears) { clone.Fears.Add(i.Clone()); }
            clone.Roots      = new List<Impedance>(); foreach (Impedance i in this.Roots) { clone.Roots.Add(i.Clone()); }
            clone.Silences   = new List<Impedance>(); foreach (Impedance i in this.Silences) { clone.Silences.Add(i.Clone()); }
            clone.Disarms    = new List<Impedance>(); foreach (Impedance i in this.Disarms) { clone.Disarms.Add(i.Clone()); }
            //
            return clone;
        }
        //
        public List<Attack> Attacks = new List<Attack>();
        public List<TargetGroup> Targets = new List<TargetGroup>();
        public List<BuffState> BuffStates = new List<BuffState>();
        public List<Impedance> Fears = new List<Impedance>();
        public List<Impedance> Roots = new List<Impedance>();
        public List<Impedance> Stuns = new List<Impedance>();
        public List<Impedance> Moves = new List<Impedance>();
        public List<Impedance> Silences = new List<Impedance>();
        public List<Impedance> Disarms = new List<Impedance>();
        //
        public Attack LastAttack { get { return Attacks[Attacks.Count - 1]; } }
        public TargetGroup LastTarget { get { return Targets[Targets.Count - 1]; } }
        public BuffState LastBuffState { get { return BuffStates[BuffStates.Count - 1]; } }
        public Impedance LastFear { get { return Fears[Fears.Count - 1]; } }
        public Impedance LastRoot { get { return Roots[Roots.Count - 1]; } }
        public Impedance LastStun { get { return Stuns[Stuns.Count - 1]; } }
        public Impedance LastMove { get { return Moves[Moves.Count - 1]; } }
        public Impedance LastSilence { get { return Silences[Silences.Count - 1]; } }
        public Impedance LastDisarm { get { return Disarms[Disarms.Count - 1]; } }
    };
}
