using System;

namespace Rawr.DPSWarr {
    public class Skills {
        public Skills(Character character,WarriorTalents talents, Stats stats, CombatFactors combatFactors,WhiteAttacks whiteStats) {
            _character = character;
            _talents = talents;
            _stats = stats;
            _combatFactors = combatFactors;
            _whiteStats = whiteStats;
            _calcOpts = character.CalculationOptions as CalculationOptionsDPSWarr;
        }
        #region Global Variables
        private Character _character;
        private WarriorTalents _talents;
        private Stats _stats;
        private CombatFactors _combatFactors;
        private WhiteAttacks _whiteStats;
        private CalculationOptionsDPSWarr _calcOpts;
        /*public const float ROTATION_LENGTH_FURY = 8.0f;
        public const float ROTATION_LENGTH_ARMS_GLYPH = 42.0f;
        public const float ROTATION_LENGTH_ARMS_NOGLYPH = 30.0f;*/
        #endregion
        #region Base
        // White Damage + White Rage Generated
        public class WhiteAttacks {
            // Constructors
            public WhiteAttacks(WarriorTalents talents, Stats stats, CombatFactors combatFactors, Character character) {
                _talents = talents;
                _stats = stats;
                _combatFactors = combatFactors;
                _character = character;
                OVD_FREQ = 0.0f;
            }
            // Variables
            private readonly WarriorTalents _talents;
            private readonly Stats _stats;
            private readonly CombatFactors _combatFactors;
            private readonly Character _character;
            private float OVD_FREQ;
            // Get/Set
            public float Ovd_Freq { get { return OVD_FREQ; } set { OVD_FREQ = value; } }
            // Functions
            public float MhWhiteDPS
            {
                get
                {
                    float wepSpeed = _combatFactors.MainHandSpeed;
                    if (_combatFactors.MainHand.Slot == Item.ItemSlot.TwoHand &&
                        (_character.OffHand != null && _combatFactors.OffHand.Slot == Item.ItemSlot.TwoHand) &&
                        _talents.TitansGrip != 1f)
                    {
                        wepSpeed += (1.5f - (0.5f * _talents.ImprovedSlam)) / 5f;
                    }
                    float mhWhiteDPS = MhAvgSwingDmg;
                    mhWhiteDPS /= wepSpeed;
                    mhWhiteDPS *= (1f - Ovd_Freq);
                    return (float)Math.Max(0f, mhWhiteDPS);
                }
            }
            public float OhWhiteDPS
            {
                get
                {
                    float ohWhiteDPS = OhAvgSwingDmg;
                    ohWhiteDPS /= _combatFactors.OffHandSpeed;
                    if (_combatFactors.OffHand != null && _combatFactors.OffHand.DPS > 0 &&
                        (_combatFactors.MainHand.Slot != Item.ItemSlot.TwoHand || _talents.TitansGrip == 1))
                    {
                        return (float)Math.Max(0f, ohWhiteDPS);
                    }
                    else { return 0f; }
                }
            }
            public float MhAvgSwingDmg
            {
                get
                {
                    float mhWhiteSwing = _combatFactors.AvgMhWeaponDmg * _combatFactors.ProbMhWhiteHit;
                    mhWhiteSwing += _combatFactors.AvgMhWeaponDmg * _combatFactors.MhCrit * (1 + _combatFactors.BonusWhiteCritDmg);
                    mhWhiteSwing += _combatFactors.AvgMhWeaponDmg * _combatFactors.GlanceChance * 0.7f;

                    mhWhiteSwing *= _combatFactors.DamageBonus;
                    mhWhiteSwing *= _combatFactors.DamageReduction;

                    return mhWhiteSwing;
                }
            }
            public float OhAvgSwingDmg
            {
                get
                {
                    float ohWhiteSwing = _combatFactors.AvgOhWeaponDmg * _combatFactors.ProbOhWhiteHit;
                    ohWhiteSwing += _combatFactors.AvgOhWeaponDmg * _combatFactors.OhCrit * (1f + _combatFactors.BonusWhiteCritDmg);
                    ohWhiteSwing += _combatFactors.AvgOhWeaponDmg * _combatFactors.GlanceChance * 0.7f;

                    ohWhiteSwing *= _combatFactors.DamageBonus;
                    ohWhiteSwing *= _combatFactors.DamageReduction;

                    return ohWhiteSwing;
                }
            }
            // Rage Calcs
            public float GetSwingRage(Item i, bool isMH) {
                // d = damage amt
                // s = weapon speed
                // f = hit factor

                float d, s, f, rage;

                rage = 0.0f;
                s = i.Speed;

                // regular hit
                d = _combatFactors.AvgWeaponDmg(i,isMH) * _combatFactors.DamageReduction;
                f = (isMH ? 3.5f : 1.75f);
                rage += RageFormula(d, s, f) * _combatFactors.ProbWhiteHit(i);

                // crit
                d *= (1f + _combatFactors.BonusWhiteCritDmg);
                f = (isMH ? 7.0f : 3.5f);
                rage += RageFormula(d, s, f) * _combatFactors.CalcCrit(i);

                // glance
                d = d / (1f + _combatFactors.BonusWhiteCritDmg) * 0.75f;
                f = (isMH ? 3.5f : 1.75f);
                rage += RageFormula(d, s, f) * _combatFactors.GlanceChance;

                // UW rage per swing
                rage += (_combatFactors.MainHand.Speed * (3f * _talents.UnbridledWrath) / 60.0f) * (1.0f - _combatFactors.WhiteMissChance);
                return rage;
            }
            // Rage generated per second
            public float whiteRageGenPerSec
            {
                get
                {
                    bool useOH = _character.OffHand != null;
                    float MHRage = (_character.MainHand != null && _character.MainHand.MaxDamage > 0 ? GetSwingRage(_character.MainHand.Item, true) : 0f);
                    float OHRage = 0f;
                    OHRage = (useOH && _character.OffHand != null && _character.OffHand.MaxDamage > 0 ? GetSwingRage(_character.OffHand.Item, false) : 0f);

                    // Rage per Second
                    MHRage /= _combatFactors.MainHandSpeed;
                    if (useOH) { OHRage /= _combatFactors.OffHandSpeed; }

                    float rage = MHRage + (useOH ? OHRage : 0f);

                    return rage;
                }
            }
            public float RageFormula(float d, float s, float f) {
                /* R = Rage Generated
                 * d = damage amount
                 * c = rage conversion value
                 * s = weapon speed
                 * f = hit factor */
                float c = 453.3f;
                if (_character.Level != 80) c = 0.0091107836f * _character.Level * _character.Level + 3.225598133f * _character.Level + 4.2652911f; // = ~320.6;
                float dmgRage = 7.5f * d / c;
                float rps = f * s; // 3.5rage/sec baseline
                float R = System.Math.Min((dmgRage + rps) / 2.0f, dmgRage*2.0f);
                
                //R = 3.75f * d / c + f * s / 2.0f;
                R *= (1.0f + 0.25f * _talents.EndlessRage);
                return R;
            }
        }
        // Ability class base
        public class Ability {
            // Constructors
            public Ability() {
                // Character related
                Char = null;
                Talents = null;
                StatS = null;
                combatFactors = null;
                Whiteattacks = null;
                CalcOpts = null;
                // Ability Related
                Name = "Invalid";
                ReqTalent = false;
                CanBeDodged = true;
                CanBeParried = true;
                Talent2ChksValue = 0;
                ReqMeleeWeap = false;
                ReqMeleeRange = false;
                ReqMultiTargs = false;
                Targets = 1f;
                MaxRange = 5f; // In Yards 
                Cd = -1f; // In Seconds
                RageCost = -1f;
                CastTime = -1f; // In Seconds
                StanceOkFury = false;
                StanceOkArms = false;
                StanceOkDef = false;
                DamageBase = 0f;
                DamageBonus = 0f;
                CritPercBonus = 0.00f;
            }
            #region Variables
            private string NAME;
            private float DAMAGEBASE;
            private float DAMAGEBONUS;
            private float CRITPERCBONUS;
            private bool CANBEDODGED;
            private bool CANBEPARRIED;
            private bool REQTALENT;
            private int TALENT2CHKSVALUE;
            private bool REQMELEEWEAP;
            private bool REQMELEERRANGE;
            private bool REQMULTITARGS;
            private float TARGETS;
            private float MAXRANGE; // In Yards 
            private float CD; // In Seconds
            private float RAGECOST;
            private float CASTTIME; // In Seconds
            private bool STANCEOKARMS; // The ability can be used in Battle Stance
            private bool STANCEOKFURY; // The ability can be used in Berserker Stance
            private bool STANCEOKDEF;  // The ability can be used in Defensive Stance
            public float HSorCLVPerSecond;
            public float HSorCLVPercent;
            private Character CHARACTER;
            private WarriorTalents TALENTS;
            private Stats STATS;
            private CombatFactors COMBATFACTORS;
            private WhiteAttacks WHITEATTACKS;
            private CalculationOptionsDPSWarr CALCOPTS;
            #endregion
            #region Get/Set
            public float bloodsurgeRPS { get; set; }
            public string Name { get { return NAME; } set { NAME = value; } }
            public bool ReqTalent { get { return REQTALENT; } set { REQTALENT = value; } }
            public int Talent2ChksValue { get { return TALENT2CHKSVALUE; } set { TALENT2CHKSVALUE = value; } }
            public bool ReqMeleeWeap { get { return REQMELEEWEAP; } set { REQMELEEWEAP = value; } }
            public bool ReqMeleeRange { get { return REQMELEERRANGE; } set { REQMELEERRANGE = value; } }
            public bool ReqMultiTargs { get { return REQMULTITARGS; } set { REQMULTITARGS = value; } }
            public float Targets { get { return TARGETS; } set { TARGETS = value; } }
            public bool CanBeDodged { get { return CANBEDODGED; } set { CANBEDODGED = value; } }
            public bool CanBeParried { get { return CANBEPARRIED; } set { CANBEPARRIED = value; } }
            public float MaxRange { get { return MAXRANGE; } set { MAXRANGE = value; } } // In Yards 
            public float Cd { get { return CD; } set { CD = value; } } // In Seconds
            public float RageCost { get { return RAGECOST; } set { RAGECOST = value; } }
            public float CastTime { get { return CASTTIME; } set { CASTTIME = value; } } // In Seconds
            public float DamageBase { get { return DAMAGEBASE; } set { DAMAGEBASE = value; } }
            public float DamageBonus { get { return CRITPERCBONUS; } set { CRITPERCBONUS = value; } }
            public float CritPercBonus { get { return DAMAGEBONUS; } set { DAMAGEBONUS = value; } }
            public bool StanceOkFury { get { return STANCEOKFURY; } set { STANCEOKFURY = value; } }
            public bool StanceOkArms { get { return STANCEOKARMS; } set { STANCEOKARMS = value; } }
            public bool StanceOkDef { get { return STANCEOKDEF; } set { STANCEOKDEF = value; } }
            public Character Char {
                get { return CHARACTER; }
                set {
                    CHARACTER = value;
                    if(CHARACTER != null){
                        Talents = CHARACTER.WarriorTalents;
                        //StatS = CalculationsDPSWarr.GetCharacterStats(CHARACTER, null);
                        combatFactors = new CombatFactors(CHARACTER, StatS);
                        //Whiteattacks = Whiteattacks;
                        CalcOpts = CHARACTER.CalculationOptions as CalculationOptionsDPSWarr;
                    } else {
                        Talents = null;
                        StatS = null;
                        combatFactors = null;
                        Whiteattacks = null;
                        CalcOpts = null;
                    }
                }
            }
            public WarriorTalents Talents { get { return TALENTS; } set { TALENTS = value; } }
            public Stats StatS { get { return STATS; } set { STATS = value; } }
            public CombatFactors combatFactors { get { return COMBATFACTORS; } set { COMBATFACTORS = value; } }
            public WhiteAttacks Whiteattacks { get { return WHITEATTACKS; } set { WHITEATTACKS = value; } }
            public CalculationOptionsDPSWarr CalcOpts { get { return CALCOPTS; } set { CALCOPTS = value; } }
            public virtual float RageUsePerSecond { get { return (!GetValided() ? 0f : Activates * RageCost / RotationLength); } }
            public virtual float RotationLength { 
                get 
                { 
                    float LatencyMOD = 1f + CalcOpts.GetLatency(); 
                    return (CalcOpts.FuryStance ? Rotation.ROTATION_LENGTH_FURY * LatencyMOD : 
                        (Talents.GlyphOfRending ? Rotation.ROTATION_LENGTH_ARMS_GLYPH * LatencyMOD : Rotation.ROTATION_LENGTH_ARMS_NOGLYPH * LatencyMOD));
                }
            }
            public virtual float Activates { 
                get 
                { 
                    if (!GetValided()) return 0f; 
                    return RotationLength / Cd; 
                } 
            } // Number of times used in rotation
            public virtual float ActivatesOverride
            {
                get
                { // Number of times used in rotation
                    if (!GetValided()) { return 0f; }
                    return RotationLength / Cd;
                }
            }

            public virtual float Damage { 
                get
                { 
                    return (!GetValided() ? 0f : (float)Math.Max(0f, DamageBase * (1f + DamageBonus) * Targets)); 
                } 
            }
            public virtual float DamageOverride { 
                get { 
                    return (float)Math.Max(0f, DamageBase * (1f + DamageBonus) * Targets); 
                } 
            }
            public virtual float AvgDamage { get { return Damage * Activates; } }
            public virtual float DamageOnUse
            {
                get
                {
                    float dmg = Damage; // Base Damage
                    dmg *= combatFactors.DamageBonus; // Global Damage Bonuses
                    dmg *= combatFactors.DamageReduction; // Global Damage Penalties

                    float Crit = 0f;
                    // Following keeps the crit perc between 0f and 1f (0%-100%)
                    Crit = (float)Math.Max(0f, (float)Math.Min(1f, combatFactors.MhYellowCrit + CritPercBonus));

                    dmg *= (1f - combatFactors.YellowMissChance - (CanBeDodged ? combatFactors.MhDodgeChance : 0f)
                        + Crit * combatFactors.BonusYellowCritDmg); // Attack Table

                    return (float)Math.Max(0f, dmg);
                }
            }
            public virtual float AvgDamageOnUse{ get{ return DamageOnUse * Activates; } }
            public virtual float DPS { get { return AvgDamageOnUse / RotationLength; } }
            #endregion
            #region Functions
            /*public virtual float GetRageUsePerSecond() {
                if (!GetValided()) { return 0f; }
                return Activates * RageCost / RotationLength;
            }*/
            public virtual float GetRageUsePerSecond(float acts) {
                if (!GetValided()) { return 0f; }
                return acts * RageCost / RotationLength;
            }
            /*public virtual float RotationLength {
                float latencyMOD = 1f + CalcOpts.GetLatency();
                if (CalcOpts.FuryStance) {
                    return Rotation.ROTATION_LENGTH_FURY * latencyMOD;
                }else{
                    if (Talents.GlyphOfRending) {
                        return Rotation.ROTATION_LENGTH_ARMS_GLYPH * latencyMOD;
                    } else {
                        return Rotation.ROTATION_LENGTH_ARMS_NOGLYPH * latencyMOD;
                    }
                }
            }*/
            public virtual bool GetValided() {
                // Null crap is bad
                if (Char == null || CalcOpts == null || Talents == null || Char.MainHand == null) { return false; }
                // Talent Requirements
                if (ReqTalent && Talent2ChksValue == 0) { return false; }
                // Need a weapon
                if (ReqMeleeWeap && Char.MainHand.MaxDamage <= 0) { return false; }
                // Need Multiple Targets or it's useless
                if (ReqMultiTargs && !CalcOpts.MultipleTargets) { return false; }
                // Proper Stance
                if ((CalcOpts.FuryStance && !StanceOkFury)
                    || (!CalcOpts.FuryStance && !StanceOkArms)
                    /*||( CalcOpts.DefStance  && !StanceOkDef )*/ ) { return false; }
                return true;
            }
            /*public virtual float GetActivates() { return GetActivates(true); } // Number of times used in rotation
            public virtual float GetActivates(bool Override) { // Number of times used in rotation
                if (!GetValided()) { return 0f; }
                return RotationLength / Cd;
            }*/
            public virtual float GetHealing() { return 0f; }
            /*public virtual float GetDamage() { return GetDamage(false); }
            public virtual float GetDamage(bool Override) {
                if (!GetValided()) { return 0f; }
                return (float)Math.Max(0f,DamageBase * (1f + DamageBonus) * Targets);
            }
            public virtual float GetAvgDamage() { return GetDamage() * Activates; }
            public virtual float GetDamageOnUse() {
                float Damage = GetDamage(); // Base Damage
                Damage *= combatFactors.DamageBonus; // Global Damage Bonuses
                Damage *= combatFactors.DamageReduction; // Global Damage Penalties

                float Crit = 0f;
                // Following keeps the crit perc between 0f and 1f (0%-100%)
                Crit = (float)Math.Max(0f,(float)Math.Min(1f,combatFactors.MhYellowCrit + CritPercBonus));

                Damage *= (1f - combatFactors.YellowMissChance
                              - (CanBeDodged  ? combatFactors.MhDodgeChance : 0f)
                              - (CanBeParried ? combatFactors.MhParryChance : 0f)
                        + Crit * combatFactors.BonusYellowCritDmg); // Attack Table

                return (float)Math.Max(0f,Damage);
            }
            public virtual float GetAvgDamageOnUse() { return GetDamageOnUse() * Activates; }
            public virtual float GetDPS() { return GetAvgDamageOnUse() / GetRotation(); }*/
            public virtual float GetAvgDamageOnUse(float acts) {
                float dou = DamageOnUse;
                return dou * acts;
            }
            public virtual float GetDPS(float acts) {
                float adou = GetAvgDamageOnUse(acts);
                float rot = RotationLength;
                return adou / rot;
            }
            #endregion
        }
        #endregion

        #region Direct Damage Abilities
        // Fury Abilities
        public class BloodThirst : Ability {
            // Constructors
            /// <summary>
            /// Instantly attack the target causing [AP*50/100] damage. In addition, the next 3 successful melee
            /// attacks will restore 1% health. This effect lasts 8 sec. Damage is based on your attack power.
            /// </summary>
            /// <TalentsAffecting>Bloodthirst (Requires talent), Unending Fury [+(2*Pts)% Damage]</TalentsAffecting>
            /// <GlyphsAffecting>Glyph of Bloodthirst [+100% from healing effect]</GlyphsAffecting>
            public BloodThirst(Character c, Stats s, CombatFactors cf, WhiteAttacks wa) {
                Char = c;Talents = c.WarriorTalents;StatS = s;combatFactors = cf;Whiteattacks = wa;CalcOpts = Char.CalculationOptions as CalculationOptionsDPSWarr;
                //
                Name = "Bloodthirst";
                ReqTalent = true;
                Talent2ChksValue = Talents.Bloodthirst;
                ReqMeleeWeap = true;
                ReqMeleeRange = true;
                Targets += StatS.BonusTargets;
                Cd = 4f; // In Seconds
                Duration = 8f;
                RageCost = 20f - (Talents.FocusedRage * 1f);
                StanceOkFury = true;
                DamageBase = StatS.AttackPower * 50f / 100f;
                DamageBonus = Talents.UnendingFury * 0.02f;
                CritPercBonus = StatS.MortalstrikeBloodthirstCritIncrease;
            }
            // Variables
            private float DURATION;
            // Get/Set
            public float Duration { get { return DURATION; } set { DURATION = value; } }
            // Functions
            public override float Activates
            {
                get
                {
                    if (!GetValided()) { return 0f; }
                    return 2.0f; // Only have time for 3 in rotation due to clashes in BT and WW cooldown timers
                }
            }
            public override float GetHealing() {
                // ToDo: Bloodthirst healing effect, also adding in GlyphOfBloodthirst (+100% healing)
                return StatS.Health / 100.0f * (Talents.GlyphOfBloodthirst?2f:1f);
            }
        }
        public class WhirlWind : Ability {
            // Constructors
            /// <summary>
            /// In a whirlwind of steel you attack up to 4 enemies in 8 yards,    
            /// causing weapon damage from both melee weapons to each enemy.
            /// </summary>
            /// <TalentsAffecting>Improved Whirlwind [+(10*Pts)% Damage], Unending Fury [+(2*Pts)% Damage]</TalentsAffecting>
            /// <GlyphsAffecting>Glyph of Whirlwind [-2 sec Cooldown]</GlyphsAffecting>
            public WhirlWind(Character c, Stats s, CombatFactors cf,WhiteAttacks wa) {
                Char = c;Talents = c.WarriorTalents;StatS = s;combatFactors = cf;Whiteattacks = wa;CalcOpts = Char.CalculationOptions as CalculationOptionsDPSWarr;
                //
                Name = "WhirlWind";
                ReqMeleeWeap = true;
                ReqMeleeRange = true;
                MaxRange = 8f; // In Yards
                Cd = 10f - (Talents.GlyphOfWhirlwind ? 2f : 0f); // In Seconds
                Targets *= (1f + StatS.BonusTargets);
                Targets *= (CalcOpts.MultipleTargets?4f:1f);
                RageCost = 25f - (Talents.FocusedRage * 1f);
                StanceOkFury = true;
                DamageBonus = Talents.ImprovedWhirlwind * 0.10f + Talents.UnendingFury * 0.02f;
            }
            // Variables
            // Get/Set
            // Functions
            public override float Activates
            {
                get
                {
                    if (!GetValided()) { return 0f; }
                    //return RotationLength / (Cd - (Talents.GlyphOfWhirlwind ? 2f : 0f));
                    return 1f;
                }
            }
            // Whirlwind while dual wielding executes two separate attacks; assume no offhand in base case
            public override float Damage
            {
                get
                {
                    return GetDamage(false, false) + GetDamage(false,true);
                }
            }
            public override float DamageOverride { get { return GetDamage(true, false); } }
            /// <summary></summary>
            /// <param name="Override">When true, do not check for Bers Stance</param>
            /// <param name="isOffHand">When true, do calculations for off-hand damage instead of main-hand</param>
            /// <returns>Unmitigated damage of a single hit</returns>
            private float GetDamage(bool Override, bool isOffHand) {
                if (!GetValided() && !Override) { return 0f; }

                float Damage;
                if (isOffHand) {
                    if (this.Char.OffHand != null && this.Char.OffHand.Item != null) {
                        Damage = combatFactors.NormalizedOhWeaponDmg * (0.50f + Talents.DualWieldSpecialization * 0.025f);
                    }else{ Damage = 0f; }
                }else{ Damage = combatFactors.NormalizedMhWeaponDmg; }

                return (float)Math.Max(0f, Damage * (1f + DamageBonus) * Targets);
            }
            public override float DamageOnUse
            {
                get
                {
                    float DamageMH = GetDamage(false,false); // Base Damage
                    DamageMH *= combatFactors.DamageBonus; // Global Damage Bonuses
                    DamageMH *= combatFactors.DamageReduction; // Global Damage Penalties
                    DamageMH *= (1f - combatFactors.YellowMissChance - combatFactors.MhDodgeChance - combatFactors.MhParryChance
                        + combatFactors.MhYellowCrit * combatFactors.BonusYellowCritDmg); // Attack Table
                    float DamageOH = GetDamage(false, true);
                    DamageOH *= combatFactors.DamageBonus;
                    DamageOH *= combatFactors.DamageReduction;
                    DamageOH *= (1f - combatFactors.YellowMissChance - combatFactors.OhDodgeChance - combatFactors.OhParryChance
                        + combatFactors.OhYellowCrit * combatFactors.BonusYellowCritDmg);

                    float Damage = DamageMH + DamageOH;
                    return (float)Math.Max(0f, Damage * Targets);
                }
            }
        }
        public class BloodSurge : Ability {
            // Constructors
            /// <summary>
            /// Your Heroic Strike, Bloodthirst and Whirlwind hits have a (7%/13%/20%) chance of making
            /// your next Slam instant for 5 sec.
            /// </summary>
            /// <TalentsAffecting>Bloodsurge (Requires Talent) [(7%/13%/20%) chance]</TalentsAffecting>
            /// <GlyphsAffecting></GlyphsAffecting>
            public BloodSurge(Character c, Stats s, CombatFactors cf,WhiteAttacks wa) {
                Char = c;Talents = c.WarriorTalents;StatS = s;combatFactors = cf;Whiteattacks = wa;CalcOpts = Char.CalculationOptions as CalculationOptionsDPSWarr;
                //
                Name = "Bloodsurge";
                ReqTalent = true;
                Talent2ChksValue = Talents.Bloodsurge;
                ReqMeleeWeap = true;
                ReqMeleeRange = true;
                Cd = 5f; // In Seconds
                RageCost = 15f - (Talents.FocusedRage * 1f);
                StanceOkFury = true;
                hsActivates = 0.0f;
            }
            // Variables
            public float hsActivates { get; set; }
            private Slam SL;
            private WhirlWind WW;
            private BloodThirst BT;
            // Get/Set
            public Slam Slam { get { return SL; } set { SL = value; } }
            public WhirlWind Whirlwind { get { return WW; } set { WW = value; } }
            public BloodThirst Bloodthirst { get { return BT; } set { BT = value; } }
            // Functions

            // Assumes one slot to slam every 8 seconds: WW/BT/Slam/BT repeat. Not optimal, but easy to do
            private float BasicFuryRotation(float chanceMHhit, float chanceOHhit, float hsActivates, float procChance){
                float chanceWeDontProc = 1f;
                chanceWeDontProc *= (1f - hsActivates * procChance * chanceMHhit);
                chanceWeDontProc *= (1f - Whirlwind.Activates * procChance * chanceMHhit) * (1f - Whirlwind.Activates * procChance * chanceOHhit);
                chanceWeDontProc *= (1f - Bloodthirst.Activates * procChance * chanceMHhit);
                return 1f - chanceWeDontProc;
            }
            
            private float CalcSlamProcs(float chanceMHhit, float chanceOHhit, float hsActivates, float procChance) {
                float hsPercent = (hsActivates) / (RotationLength / combatFactors.MainHandSpeed);
                float numProcs = 0.0f;
                int whiteTimer = 0;
                int WWtimer = 0;
                int BTtimer = 0;
                const int GCD = 15;
                float chanceWeDontProc = 1f; // temp value that keeps track of what the odds are we got a proc by SLAM time
                int numWW = 0;
                int numBT = 0;
                for (int timeStamp = 0; timeStamp < RotationLength*10; timeStamp++) {
                    if (whiteTimer <= 0) {
                        chanceWeDontProc *= (1f - hsPercent * procChance * chanceMHhit);
                        whiteTimer = (int)Math.Ceiling(combatFactors.MainHandSpeed*10);
                    }
                    if (timeStamp % GCD == 0) {
                        if (WWtimer <= 0) {
                            chanceWeDontProc *= (1f - procChance*chanceMHhit) * (1f - procChance*chanceOHhit);
                            WWtimer = 80;
                            numWW++;
                        } else if (BTtimer <= 0) {
                            chanceWeDontProc *= (1f - procChance * chanceMHhit);
                            BTtimer = 40;
                            numBT++;
                        } else {
                            // We slam
                            numProcs += (1f - chanceWeDontProc);
                            chanceWeDontProc = 1f;
                        }
                    }
                    whiteTimer--;
                    WWtimer--;
                    BTtimer--;
                }
                return numProcs;
            }
            public override float Activates
            {
                get
                {
                    // Invalidators
                    if (!GetValided()) { return 0f; }

                    // Actual Calcs
                    //Ability HS = new HeroicStrike(Char, StatS, combatFactors, Whiteattacks);

                    float chance = Talents.Bloodsurge * 0.20f / 3f;
                    float chanceMhHitLands = (1f - combatFactors.YellowMissChance - combatFactors.MhDodgeChance);
                    float chanceOhHitLands = (1f - combatFactors.YellowMissChance - combatFactors.OhDodgeChance);

                    float procs2 = CalcSlamProcs(chanceMhHitLands, chanceOhHitLands, hsActivates, chance);
                    float procs3 = BasicFuryRotation(chanceMhHitLands, chanceOhHitLands, hsActivates, chance);

                    float procs = BT.Activates * chanceMhHitLands + Whirlwind.Activates * chanceMhHitLands + Whirlwind.Activates * chanceOhHitLands
                        + hsActivates * chanceMhHitLands;// HS.Activates;
                    procs *= chance;
                    //procs /= RotationLength;
                    // procs = (procs / RotationLength) - (chance * chance + 0.01f); // WTF is with squaring chance?
                    if (procs2 < 0) { procs2 = 0; }
                    if (procs2 > 1) { procs2 = 1; } // Only have 1 free GCD in the default rotation
                    return procs3;

                    // ORIGINAL LINES
                    //float chance = _talents.Bloodsurge * 0.0666666666f;
                    //float procs = 3 + 4 + ((16 / _combatFactors.MainHandSpeed) * heroicStrikePercent);
                    //procs *= chance;
                    //procs = (procs / 16) - (chance * chance + 0.01f);
                    //if (procs < 0) { procs = 0; }
                    //return procs;
                }
            }
            public override float Damage
            {
                get
                {
                    if (!GetValided()) { return 0f; }
                    return (float)Math.Max(0f, Slam.DamageOverride);
                }
            }
        }
        // Arms Abilities
        public class MortalStrike : Ability {
            // Constructors
            /// <summary>
            /// A vicious strike that deals weapon damage plus 380 and wounds the target, reducing
            /// the effectiveness of any healing by 50% for 10 sec.
            /// </summary>
            /// <TalentsAffecting>
            /// Mortal Strike (Requires Talent),
            /// Improved Mortal Strike [+(10-ROUNDUP(10/3*Pts))% damage, -(1/3*Pts) sec cooldown]
            /// </TalentsAffecting>
            /// <GlyphsAffecting>Glyph of Mortal Strike [+10% damage]</GlyphsAffecting>
            public MortalStrike(Character c, Stats s, CombatFactors cf,WhiteAttacks wa) {
                Char = c;Talents = c.WarriorTalents;StatS = s;combatFactors = cf;Whiteattacks = wa;CalcOpts = Char.CalculationOptions as CalculationOptionsDPSWarr;
                //
                Name = "Mortal Strike";
                ReqTalent = true;
                Talent2ChksValue = Talents.MortalStrike;
                ReqMeleeWeap = true;
                ReqMeleeRange = true;
                Targets += StatS.BonusTargets;
                Cd = 6f; // In Seconds
                RageCost = 30f;
                StanceOkFury = StanceOkArms = StanceOkDef = true;
                DamageBase = combatFactors.NormalizedMhWeaponDmg + 380f;
                DamageBonus = Talents.ImprovedMortalStrike / 3f * 0.1f +
                              (Talents.GlyphOfMortalStrike ? 0.1f : 0f);
                CritPercBonus = StatS.MortalstrikeBloodthirstCritIncrease;
            }
            // Variables
            // Get/Set
            // Functions
            public override float Activates
            {
                get
                {
                    if (!GetValided()) { return 0f; }
                    float latencyMOD = 1f + CalcOpts.GetLatency();
                    return (float)Math.Max(0f, RotationLength / ((Cd - (Talents.ImprovedMortalStrike / 3.0f)) * latencyMOD));
                }
            }
        }
        public class Suddendeath : Ability {
            // Constructors
            /// <summary>
            /// Your melee hits have a (3*Pts)% chance of allowing the use of Execute regardless of
            /// the target's Health state. This Execute only uses up to 30 total rage. In addition,
            /// you keep at least (3/7/10) rage after using Execute.
            /// </summary>
            /// <TalentsAffecting>Sudden Death (Requires Talent) [(3*Pts)% chance to proc and (3/7/10) rage kept after],
            /// Improved Execute [-(2.5*Pts) rage cost]</TalentsAffecting>
            /// <GlyphsAffecting>Glyph of Execute [Execute acts as if it had 10 additional rage]</GlyphsAffecting>
            public Suddendeath(Character c, Stats s, CombatFactors cf,WhiteAttacks wa) {
                Char = c;Talents = c.WarriorTalents;StatS = s;combatFactors = cf;Whiteattacks = wa;CalcOpts = Char.CalculationOptions as CalculationOptionsDPSWarr;
                //
                Name = "SuddenDeath";
                Execute = new Execute(c, s, cf, wa);
                RageCost = Execute.RageCost;
                ReqTalent = true;
                Talent2ChksValue = Talents.SuddenDeath;
                ReqMeleeWeap = Execute.ReqMeleeWeap;
                ReqMeleeRange = Execute.ReqMeleeRange;
                Cd = Execute.Cd;
                StanceOkArms = true;
            }
            // Variables
            private Execute EXECUTE;
            private float FREERAGE;
            // Get/Set
            public Execute Execute { get { return EXECUTE; } set { EXECUTE = value; } }
            public float LandedAtksPerSec { get; set; }
            public float FreeRage { get { return FREERAGE; } set { EXECUTE.FreeRage = value; FREERAGE = value; } }
            // Functions
            public override float Activates { get { return GetActivates(true); } }
            public float GetActivates(bool Override) {
                // Invalidators
                if (!GetValided()) { return 0f; }

                // ACTUAL CALCS
                float talent = 3f * Talents.SuddenDeath / 100f;
                float hitspersec = (Override ? 1f : LandedAtksPerSec);
                float latency = 1.5f * (1f + CalcOpts.GetLatency());
                //float mod = 100f;
                float SD_GCDS = talent * hitspersec * latency /* * mod*/;
                // END ACTUAL CALCS */

                /*float fightDuration = CalcOpts.Duration;
                float hasteBonus = StatConversion.GetPhysicalHasteFromRating(combatFactors.TotalHaste, Character.CharacterClass.Warrior);
                hasteBonus = (1f + hasteBonus) * (1f + combatFactors.TotalHaste) * (1f + StatS.Bloodlust * 40f / fightDuration) - 1f;
                float meleeHitsPerSecond = 1f / 1.5f;
                if (Char.MainHand != null && Char.MainHand.Speed > 0f){ meleeHitsPerSecond += (1f / Char.MainHand.Speed) * (1f + hasteBonus); }
                if (Char.OffHand  != null && Char.OffHand.Speed  > 0f){ meleeHitsPerSecond += (1f / Char.OffHand.Speed ) * (1f + hasteBonus); }
                float meleeHitInterval = 1f / meleeHitsPerSecond;
                float baseWeaponSpeed = Char.MainHand != null ? Char.MainHand.Speed : 3f;

                SpecialEffect SuddenDeath = new SpecialEffect(Trigger.MeleeHit,
                    new Stats() { BonusCritChance = 0f, },
                    1f, 1.5f, Talents.SuddenDeath * (3f / 100f));
                float procs = SuddenDeath.GetAverageProcsPerSecond(meleeHitInterval, 1f, baseWeaponSpeed, CalcOpts.Duration);

                return procs * 1.5f;//*/
                return SD_GCDS;
            }
            public override float Damage {
                get {
                    if (!GetValided()) { return 0f; }
                    float Damage = Execute.DamageOverride;
                    return (float)Math.Max(0f, Damage);
                }
            }
        }
        public class OverPower : Ability
        {
            // Constructors
            /// <summary>
            /// Instantly overpower the enemy, causing weapon damage plus 125. Only usable after the target dodges.
            /// The Overpower cannot be blocked, dodged or parried.
            /// </summary>
            /// <TalentsAffecting>Improved Overpower [+(25*Pts)% Crit Chance],
            /// Unrelenting Assault [-(2*Pts) sec cooldown, +(10*Pts)% Damage.]</TalentsAffecting>
            /// <GlyphsAffecting>Glyph of Overpower [Can proc when parried]</GlyphsAffecting>
            public OverPower(Character c, Stats s, CombatFactors cf, WhiteAttacks wa)
            {
                Char = c; Talents = c.WarriorTalents; StatS = s; combatFactors = cf; Whiteattacks = wa; CalcOpts = Char.CalculationOptions as CalculationOptionsDPSWarr;
                //
                Name = "Overpower";
                ReqMeleeWeap = true;
                ReqMeleeRange = true;
                CanBeDodged = false;
                CanBeParried = false;
                Cd = 5f - (2f * Talents.UnrelentingAssault); // In Seconds
                RageCost = 5f - (Talents.FocusedRage * 1f);
                Targets += StatS.BonusTargets;
                StanceOkArms = true;
                DamageBase = combatFactors.NormalizedMhWeaponDmg;
                DamageBonus = (0.1f * Talents.UnrelentingAssault);
                CritPercBonus = 0.25f * Talents.ImprovedOverpower;
            }
            // Variables
            // Get/Set
            // Functions
            public override float Activates
            {
                get
                {
                    if (!GetValided()) { return 0f; }

                    float latencyMOD = 1f + CalcOpts.GetLatency();
                    //float GCDPerc = (Talents.TasteForBlood == 0 ? 0f : (1.5f - 0.5f * Talents.ImprovedSlam / 1000f) / ((Talents.TasteForBlood > 1f) ? 6f : 9f));

                    float cd = 1f, result = 0;

                    if (combatFactors.MhDodgeChance + (Talents.GlyphOfOverpower ? combatFactors.MhParryChance : 0f) <= 0f && Talents.TasteForBlood == 0f)
                    {
                        // No TasteForBlood talent and no chance to activate otherwise
                        cd = 0f;
                    }
                    else if (Talents.TasteForBlood == 0f)
                    {
                        // No TasteForBlood talent and but chance to activate via parry or dodge
                        cd = 1f / (
                            (combatFactors.MhDodgeChance + (Talents.GlyphOfOverpower ? combatFactors.MhParryChance : 0f)) * (1f / combatFactors.MainHandSpeed) +
                            /*0.01f * GetLandedAtksPerSecNoSS() * combatFactors.MhExpertise * Talents.SwordSpecialization * 54f / 60f +
                            0.03f * GCDPerc * GetLandedAtksPerSec() +*/
                            // Removed this because it's causing stack overflow (try a fury spec in arms stance)
                            1f / (5f / 1000f)//+
                            //1f / /*AB49 Slam Proc GCD % 0.071227f*/ SL.GetActivates()
                         );
                    }// TODO: TasteForBlood talent AND chance to activate otherwise
                    else if (Talents.TasteForBlood > 0f)
                    {
                        // TasteForBlood talent and NO chance to activate otherwise
                        cd = 6f / (1f / 3f * Talents.TasteForBlood);
                    }

                    result = RotationLength / (cd * latencyMOD);

                    return result;
                }
            }
        }
        
        public class Bladestorm : Ability {
            // Constructors
            /// <summary>
            /// Instantly Whirlwind up to 4 nearby targets and for the next 6 sec you will
            /// perform a whirlwind attack every 1 sec. While under the effects of Bladestorm, you can move but cannot
            /// perform any other abilities but you do not feel pity or remorse or fear and you cannot be stopped
            /// unless killed.
            /// </summary>
            /// <TalentsAffecting>Bladestorm [Requires Talent]</TalentsAffecting>
            /// <GlyphsAffecting>Glyph of Bladestorm [-15 sec Cd]</GlyphsAffecting>
            public Bladestorm(Character c, Stats s, CombatFactors cf,WhiteAttacks wa) {
                Char = c;Talents = c.WarriorTalents;StatS = s;combatFactors = cf;Whiteattacks = wa;CalcOpts = Char.CalculationOptions as CalculationOptionsDPSWarr;
                //
                WW = new WhirlWind(c, s, cf, wa);
                Name = "Bladestorm";
                ReqTalent = true;
                Talent2ChksValue = Talents.Bladestorm;
                ReqMeleeWeap = true;
                ReqMeleeRange = true;
                MaxRange = WW.MaxRange; // In Yards 
                Cd = 90f - (Talents.GlyphOfBladestorm ? 15f : 0f); // In Seconds
                RageCost = 25f - (Talents.FocusedRage * 1f);
                CastTime = 6f; // In Seconds // Channeled
                StanceOkFury = StanceOkArms = StanceOkDef = true;
            }
            // Variables
            private WhirlWind WHIRLWIND;
            // Get/Set
            public WhirlWind WW { get { return WHIRLWIND; } set { WHIRLWIND = value; } }
            // Functions
            public override float Damage
            {
                get
                {
                    if (!GetValided()) { return 0f; }

                    // Base Damage
                    float Damage = WW.DamageOverride;

                    return (float)Math.Max(0f, Damage * 6f); // it WW's 6 times
                }
            }
        }
        public class Swordspec : Ability {
            // Constructors
            /// <summary>
            /// Gives a (1*Pts)% chance to get an extra attack on the same target after hitting
            /// your target with your Sword. This effect cannot occur more than once every 6 seconds.
            /// </summary>
            /// <TalentsAffecting>Sword Specialization (Requires Talent)</TalentsAffecting>
            /// <GlyphsAffecting></GlyphsAffecting>
            public Swordspec(Character c, Stats s, CombatFactors cf,WhiteAttacks wa) {
                Char = c;Talents = c.WarriorTalents;StatS = s;combatFactors = cf;Whiteattacks = wa;CalcOpts = Char.CalculationOptions as CalculationOptionsDPSWarr;
                //
                Name = "Sword Specialization";
                ReqTalent = true;
                Talent2ChksValue = Talents.SwordSpecialization;
                Cd = 6f; // In Seconds
                StanceOkFury = StanceOkArms = StanceOkDef = true;
            }
            // Variables
            private Ability SL, MS, OP, SD;
            // Get/Set
            public Ability Slam { get { return SL; } set { SL = value; } }
            public Ability MortalStrike { get { return MS; } set { MS = value; } }
            public Ability Overpower { get { return OP; } set { OP = value; } }
            public Ability SuddenDeath { get { return SD; } set { SD = value; } }
            // Functions
            public override float Activates
            {
                get
                {
                    if (!GetValided()) { return 0f; }

                    if (combatFactors.MainHand.Type != Item.ItemType.TwoHandSword) { return 0.0f; }
                    float wepSpeed = combatFactors.MainHandSpeed;
                    if (combatFactors.MainHand.Slot == Item.ItemSlot.TwoHand && Talents.TitansGrip != 1)
                    {
                        wepSpeed += (1.5f - (0.5f * Talents.ImprovedSlam)) / 5;
                    }
                    float whiteHits = (1 / wepSpeed);
                    float attacks = 0.01f * Talents.SwordSpecialization;
                    attacks *= (MS.Activates + SL.Activates + OP.Activates + SD.Activates + whiteHits);
                    return attacks;
                }
            }
            public override float Damage
            {
                get
                {
                    if (!GetValided()) { return 0f; }
                    float Damage = combatFactors.AvgMhWeaponDmg;
                    return (float)Math.Max(0f,Damage);
                    // ORIGINAL LINES
                    //float damage = SwordSpecHits() * _combatFactors.AvgMhWeaponDmg;
                    //damage *= (1 + _combatFactors.MhCrit * _combatFactors.BonusWhiteCritDmg);
                    //return damage * _combatFactors.DamageReduction;
                }
            }
            public override float DamageOnUse
            {
                get
                {
                    float _Damage = Damage; // Base Damage
                    _Damage *= combatFactors.DamageBonus; // Global Damage Bonuses
                    _Damage *= combatFactors.DamageReduction; // Global Damage Penalties
                    _Damage *= (1f - combatFactors.YellowMissChance - combatFactors.MhDodgeChance - combatFactors.MhParryChance
                        + combatFactors.MhYellowCrit * combatFactors.BonusYellowCritDmg); // Attack Table

                return (float)Math.Max(0f, _Damage);
                }
            }
            }
        public class ThunderClap : Ability {
            // Constructors
            /// <summary>
            /// Blasts nearby enemies increasing the time between their attacks by 10% for 30 sec
            /// and doing [300+AP*0.12] damage to them. Damage increased by attack power.
            /// This ability causes additional threat.
            /// </summary>
            /// <TalentsAffecting>
            /// Improved Thunder Clap [-(1/2/4) rage cost, +(10*Pts)% Damage, +(ROUNDUP(10-10/3*Pts))% Slowing Effect]
            /// Incite [+(5*Pts)% Critical Strike chance]
            /// </TalentsAffecting>
            /// <GlyphsAffecting>
            /// Glyph of Thunder Clap [+2 yds MaxRange]
            /// Glyph of Resonating Power [-5 RageCost]
            /// </GlyphsAffecting>
            public ThunderClap(Character c, Stats s, CombatFactors cf, WhiteAttacks wa) {
                Char = c;
                Talents = c.WarriorTalents;
                StatS = s;
                combatFactors = cf;
                Whiteattacks = wa;
                CalcOpts = Char.CalculationOptions as CalculationOptionsDPSWarr;
                Name = "Thunder Clap";
                ReqMeleeWeap = true;
                ReqMeleeRange = true;
                Targets = (CalcOpts.MultipleTargets ? 4f : 1f) + StatS.BonusTargets;
                MaxRange = 5f + (Talents.GlyphOfThunderClap ? 2f : 0f); // In Yards 
                Cd = 6f; // In Seconds
                Duration = 30f; // In Seconds
                float cost = 0f;
                switch (Talents.ImprovedThunderClap) {
                    case 1: { cost = 1f; break; }
                    case 2: { cost = 2f; break; }
                    case 3: { cost = 4f; break; }
                    default:{ cost = 0f; break; }
                }
                RageCost = 20f - cost - (Talents.GlyphOfResonatingPower?5f:0f) - (Talents.FocusedRage*1f);
                StanceOkArms = StanceOkDef = true;
                DamageBase = 300f + StatS.AttackPower * 0.12f;
                DamageBonus = Talents.ImprovedThunderClap * 0.10f;
                CritPercBonus = Talents.Incite * 0.05f;
            }
            // Variables
            // Get/Set
            public float Duration { get; set; }
            // Functions
            public override float Activates
            {
                get
                {
                    return base.Activates * Cd / Duration;
                }
            }
        }
        public class Execute : Ability {
            // Constructors
            /// <summary>
            /// Attempt to finish off a wounded foe, causing (1456+AP*0.2) damage and converting each
            /// extra point of rage into 38 additional damage. Only usable on enemies that have less
            /// than 20% health.
            /// </summary>
            /// <TalentsAffecting>Improved Execute [Reduces the rage cost of your Execute ability by (2.5/5).]</TalentsAffecting>
            /// <GlyphsAffecting>Glyph of Execute [Your Execute ability acts as if it has 10 additional rage.]</GlyphsAffecting>
            public Execute(Character c, Stats s, CombatFactors cf,WhiteAttacks wa) {
                Char = c;Talents = c.WarriorTalents;StatS = s;combatFactors = cf;Whiteattacks = wa;CalcOpts = Char.CalculationOptions as CalculationOptionsDPSWarr;
                //
                Name = "Execute";
                ReqMeleeWeap = true;
                ReqMeleeRange = true;
                Targets += StatS.BonusTargets;
                RageCost = 15f - (Talents.ImprovedExecute * 2.5f) - (Talents.FocusedRage * 1f);
                StanceOkFury = StanceOkArms = true;
            }
            // Variables
            // Get/Set
            public float FreeRage { get; set; }
            // Functions
            public override float Activates { get { if (!GetValided()) { return 0f; } return 0f; } }
            public override float Damage { get { return GetDamage(false); } }
            public override float DamageOverride { get { return GetDamage(true); } }
            private float GetDamage(bool Override) {
                if (!Override && !GetValided()) { return 0f; }

                float freerage = (float)System.Math.Max(0f,FreeRage);
                if (Override && freerage <= (RageCost - (Talents.ImprovedExecute * 2.5f))) {
                    freerage = RageCost - (Talents.ImprovedExecute * 2.5f);
                }else if (freerage <= 0f) {
                    return 0.0f; // No Free Rage = 0 damage
                }
                float executeRage = freerage * RotationLength;
                if (Override && executeRage > 30f) { executeRage = 30f; }
                executeRage += (Talents.GlyphOfExecution ? 10.00f : 0.00f);
                executeRage -= RageCost;

                float Damage = 1456f + StatS.AttackPower * 0.2f + executeRage * 38f;

                return (float)Math.Max(0f,Damage * Targets);
            }
        }
        public class Slam : Ability {
            // Constructors
            /// <summary>Slams the opponent, causing weapon damage plus 250.</summary>
            /// <TalentsAffecting>Improved Slam [Reduces cast time of your Slam ability by (0.5/1) sec.]</TalentsAffecting>
            /// <SetsAffecting>T7 Deadnaught Battlegear 2 Pc [+10% Damage]</SetsAffecting>
            public Slam(Character c, Stats s, CombatFactors cf,WhiteAttacks wa) {
                Char = c;Talents = c.WarriorTalents;StatS = s;combatFactors = cf;Whiteattacks = wa;CalcOpts = Char.CalculationOptions as CalculationOptionsDPSWarr;
                //
                Name = "Slam";
                ReqMeleeWeap = true;
                ReqMeleeRange = true;
                Targets += StatS.BonusTargets;
                RageCost = 15f - (Talents.FocusedRage * 1f);
                float latencyMOD = 1f + CalcOpts.GetLatency();
                CastTime = (1.5f - (Talents.ImprovedSlam * 0.5f)) /** latencyMOD*/; // In Seconds
                StanceOkArms = StanceOkDef = true;
                DamageBase = combatFactors.AvgMhWeaponDmg + 250f;
                DamageBonus = (Talents.UnendingFury * 0.02f) + (StatS.BonusSlamDamage);
            }
            // Variables
            // Get/Set
            // Functions
            public override float Activates { get { if (!GetValided()) { return 0f; } return 0f; } }
            public virtual float GetActivates(float RemActs) {
                if (!GetValided()) { return 0f; }
                return 0f;
                //return RemActs / CastTime;
            }
            public override float Damage { get { return GetDamage(false); } }
            public override float DamageOverride
            {
                get
                {
                    return GetDamage(true);
                }
            }
            private float GetDamage(bool Override) {
                if (!Override && !GetValided()) { return 0f; }
                return DamageBase * (1f + DamageBonus) * Targets;
            }
        }
        // Prot Abilities
        public class ShieldSlam : Ability {
            /// <summary>
            /// Instant, 6 sec cd, 20 Rage, Melee Range, Shields (Any)
            /// Slam the target with your shield, causing 990 to 1040 damage, modified by you shield block
            /// value, and dispels 1 magic effect on the target. Also causes a high amount of threat.
            /// </summary>
            /// <TalentsAffecting>
            /// Focused Rage [-(Talents.FocusedRage * 1f) RageCost],
            /// Gag Order [+(5*Pts)% Damage],
            /// OneHandedWeaponSpecialization [+(2*Pts)% Damage]
            /// </TalentsAffecting>
            /// <GlyphsAffecting></GlyphsAffecting>
        }
        public class Revenge : Ability {
            /// <summary>
            /// Instant, 1 sec cd, 5 Rage, Melee Range, Melee Weapon (Def)
            /// Instantly counterattack the enemy for 2399 to 2787 damage. Revenge is only usable after the
            /// warrior blocks, dodges or parries an attack.
            /// </summary>
            /// <TalentsAffecting></TalentsAffecting>
            /// <GlyphsAffecting></GlyphsAffecting>
            ///  -(Talents.FocusedRage * 1f) RageCost
            ///  +(10*Pts)% Damage
        }
        public class ConcussionBlow : Ability {
            /// <summary>
            /// Instant, 30 sec cd, 12 Rage, Melee Range, Melee Weapon (Any)
            /// Stuns the opponent for 5 sec and deals 2419 damage (based upon attack power).
            /// </summary>
            /// <TalentsAffecting>Concussion Blow [Requires Talent], Focused Rage [-(Talents.FocusedRage * 1f ) Ragecost]</TalentsAffecting>
            /// <GlyphsAffecting></GlyphsAffecting>
        }
        public class Devastate : Ability {
            /// <summary>
            /// Instant, No Cd, 12 Rage, Melee Range, 1h Melee Weapon (Any)
            /// Sunder the target's armor causing the Sunder Armor effect. In addition, causes 50% of weapon
            /// damage plus 101 for each application of Sunder Armor on the target. The Sunder Armor effect
            /// can stack up to 5 times.
            /// </summary>
            /// <TalentsAffecting>
            /// Devastate [Requires Talent]
            /// Focused Rage [-(Talents.FocusedRage * 1f) RageCost]
            /// Puncture [-(Talents.Puncture * 1f) RageCost]
            /// Sword and Board [+(5*Pts)% Crit Chance]
            /// </TalentsAffecting>
            /// <GlyphsAffecting>Glyph of Devastate [+1 stack of Sunder Armor]</GlyphsAffecting>
        }
        public class Shockwave : Ability {
            /// <summary>
            /// Instant, 20 sec Cd, 12 Rage, (Any)
            /// Sends a wave of force in front of the warrior, causing 2419 damage (based upon attack power)
            /// and stunning all enemy targets within 10 yards in a frontal cone for 4 sec.
            /// </summary>
            /// <TalentsAffecting>Shockwave [Requires Talent], Focused Rage [-(Talents.FocusedRage*1f) RageCost]</TalentsAffecting>
            /// <GlyphsAffecting>Glyph of Shockwave [-3 sec Cd]</GlyphsAffecting>
        }
        public class MockingBlow : Ability {
            /// <summary>
            /// Instant, 1 min Cooldown, 10 Rage, Melee Range, Melee Weapon, (Battle/Zerker)
            /// A mocking attack that causes weapon damage, a moderate amount of threat and forces the
            /// target to focus attacks on you for 6 sec.
            /// </summary>
            /// <TalentsAffecting>
            /// Focused Rage [-(Talents.FocusedRage*1f) RageCost]
            /// </TalentsAffecting>
            /// <GlyphsAffecting>
            /// Glyph of Barbaric Insults [+100% Threat]
            /// Glyph of Mocking Blow [+25% Damage]
            /// </GlyphsAffecting>
        }
        // PvP Abilities
        public class Pummel : Ability {
            /// <summary>
            /// Instant, 10 sec Cd, 10 Rage, Melee Range, (Zerker)
            /// Pummel the target, interupting spellcasting and preventing any spell in that school
            /// from being cast for 4 sec.
            /// </summary>
            /// <TalentsAffecting></TalentsAffecting>
            /// <GlyphsAffecting></GlyphsAffecting>
            ///  - (Talents.FocusedRage * 1f)
        }
        // Mixed Abilities
        public class VictoryRush : Ability{
            /// <summary>
            /// Instant, No Cd, No Rage, Melee Range, (Battle, Zerker)
            /// Instant attack the target causing 1424 damage. Can only be used within 25 sec after you
            /// kill an enemy that yields experience or honor. Damage is based on your attack power.
            /// </summary>
            /// <TalentsAffecting></TalentsAffecting>
            /// <GlyphsAffecting>
            /// Glyph of Victory Rush [+30% Crit Chance @ targs >70% HP]
            /// Glyph of Enduring Victory [+5 sec to length before ability wears off]
            /// </GlyphsAffecting>
        }
        public class HeroicThrow : Ability {
            /// <summary>
            /// Instant, 1 min Cd, 30 yd, Melee Weapon (Any)
            /// Throws your weapon at the enemy causing 1595 dmg (based upon attack power). This ability
            /// causes high threat.
            /// </summary>
            /// <TalentsAffecting></TalentsAffecting>
            /// <GlyphsAffecting></GlyphsAffecting>
            ///  - (Talents.FocusedRage * 1f)
        }
        #endregion
        #region OnAttack Abilities
        public class OnAttack : Ability {
            // Constructors
            public OnAttack() { OverridesPerSec = 0f; }
            // Variables
            private float OVERRIDESPERSEC;
            // Get/Set
            public float OverridesPerSec { get { return OVERRIDESPERSEC; } set { OVERRIDESPERSEC = value; } }
            public virtual float FullRageCost {
                get {
                    return RageCost
                        + Whiteattacks.GetSwingRage(combatFactors.MainHand, true);
                }
            }
            // Functions
            public override float Activates
            {
                get
                {
                    if (!GetValided()) { return 0f; }

                    float Hits = (float)Math.Max(0f, OverridesPerSec);

                    HSorCLVPerSecond = Hits;

                    return Hits * RotationLength;
                }
            }
        };
        public class HeroicStrike : OnAttack {
            // Constructors
            /// <summary>
            /// A strong attack that increases melee damage by 495 and causes a high amount of
            /// threat. Causes 173.25 additional damage against Dazed targets.
            /// </summary>
            /// <TalentsAffecting>Improved Heroic Strike [-(1*Pts) rage cost], Incite [+(5*Pts)% crit chance]</TalentsAffecting>
            /// <GlyphsAffecting>Glyph of Heroic Strike [+10 rage on crits]</GlyphsAffecting>
            public HeroicStrike(Character c, Stats s, CombatFactors cf, WhiteAttacks wa) {
                Char = c;Talents = c.WarriorTalents;StatS = s;combatFactors = cf;Whiteattacks = wa;CalcOpts = Char.CalculationOptions as CalculationOptionsDPSWarr;
                //
                Name = "Heroic Strike";
                ReqMeleeWeap = true;
                ReqMeleeRange = true;
                MaxRange = 5f; // In Yards 
                Cd = 0f/*(MHWeapon!=null?MHWeaponSpeed:0)*/; // In Seconds
                Targets += StatS.BonusTargets;
                RageCost = 15f - (Talents.ImprovedHeroicStrike * 1f) - (Talents.FocusedRage * 1f);
                CastTime = 0f; // In Seconds // Replaces a white hit
                StanceOkFury = StanceOkArms = StanceOkDef = true;
                bloodsurgeRPS = 0.0f;
                DamageBase = combatFactors.AvgMhWeaponDmg + 495f;
                CritPercBonus = Talents.Incite * 0.05f;
            }
            // Variables
            // Get/Set
            // Functions
            public override float FullRageCost {
                get {
                    return RageCost
                        - (Talents.GlyphOfHeroicStrike ? 10.0f * combatFactors.MhCrit : 0f)
                        + Whiteattacks.GetSwingRage(combatFactors.MainHand, true);
                }
            }
        }
        public class Cleave : OnAttack {
            // Constructors
            /// <summary>
            /// A sweeping attack that does your weapon damage plus 222 to the target and his nearest ally.
            /// </summary>
            /// <TalentsAffecting>Improved Cleave [+(40*Pts)% Damage], Incite [+(5*Pts)% Crit Perc]</TalentsAffecting>
            /// <GlyphsAffecting>Glyph of Cleaving [+1 targets hit]</GlyphsAffecting>
            public Cleave(Character c, Stats s, CombatFactors cf, WhiteAttacks wa) {
                Char = c;Talents = c.WarriorTalents;StatS = s;combatFactors = cf;Whiteattacks = wa;CalcOpts = Char.CalculationOptions as CalculationOptionsDPSWarr;
                //
                Name = "Cleave";
                ReqMeleeWeap = true;
                ReqMeleeRange = true;
                RageCost = 20f - (Talents.FocusedRage * 1f);
                Targets += (Talents.GlyphOfCleaving?1f:0f);
                Targets *= StatS.BonusTargets;
                CastTime = 0f; // In Seconds // Replaces a white hit
                StanceOkFury = StanceOkArms = StanceOkDef = true;
                bloodsurgeRPS = 0.0f;
                DamageBase = combatFactors.AvgMhWeaponDmg + 222f;
                DamageBonus = Talents.ImprovedCleave * 0.40f;
                CritPercBonus = Talents.Incite * 0.05f;
            }
            // Variables
            // Get/Set
            // Functions
        }
        #endregion
        #region DoT Abilities
        public class DoT : Ability {
            // Constructors
            public DoT(){}
            // Variables
            private float DURATION; // In Seconds
            private float TIMEBTWNTICKS; // In Seconds
            // Get/Set
            public float Duration      { get { return DURATION;      } set { DURATION      = value; } } // In Seconds
            public float TimeBtwnTicks { get { return TIMEBTWNTICKS; } set { TIMEBTWNTICKS = value; } } // In Seconds
            // Functions
            public virtual float TickSize { get { return 0f; } }
            public virtual float TTLTickingTime { get { return Duration; } }
            public virtual float TickLength { get { return TimeBtwnTicks; } }
            public virtual float NumTicks { get { return TTLTickingTime / TickLength; } }
            public virtual float DmgOverTickingTime { get { return TickSize * NumTicks; } }
            public virtual float GetDmgOverTickingTime(float acts) { return TickSize * (NumTicks * acts); }
            public override float GetDPS(float acts) {
                float dmgonuse = TickSize;
                float numticks = NumTicks*acts;
                float rot = RotationLength;
                float result = GetDmgOverTickingTime(acts) / rot;
                return result;
            }
            public override float DPS { get { return TickSize / TickLength; } }
        }
        public class Rend : DoT {
            // Constructors
            /// <summary>
            /// Wounds the target causing them to bleed for 380 damage plus an additional
            /// (0.2*5*MWB+mwb/2+AP/14*MWS) (based on weapon damage) over 15 sec. If used while your
            /// target is above 75% health, Rend does 35% more damage.
            /// </summary>
            /// <TalentsAffecting>Improved Rend [+(10*Pts)% Bleed Damage], Trauma [+(15*Pts)% Bleed Damage]</TalentsAffecting>
            /// <GlyphsAffecting>Glyph of Rending [+2 damage ticks]</GlyphsAffecting>
            public Rend(Character c, Stats s, CombatFactors cf,WhiteAttacks wa) {
                Char = c;Talents = c.WarriorTalents;StatS = s;combatFactors = cf;Whiteattacks = wa;CalcOpts = Char.CalculationOptions as CalculationOptionsDPSWarr;
                //
                Name = "Rend";
                ReqMeleeWeap = true;
                ReqMeleeRange = true;
                Duration = 15f + (Talents.GlyphOfRending ? 6f : 0f); // In Seconds
                TimeBtwnTicks = 3f; // In Seconds
                RageCost = 10f - (Talents.FocusedRage * 1f);
                StanceOkArms = StanceOkDef = true;
                DamageBase = 380f;
                DamageBonus = 0.10f * Talents.ImprovedRend + 0.15f * Talents.Trauma;
            }
            // Variables
            // Get/Set
            // Functions
            public override float Activates
            {
                get
                {
                    if (!GetValided()) { return 0f; }
                    float RendGCDs = RotationLength / TTLTickingTime;
                    return RendGCDs;
                }
            }
            public override float TickSize
            {
                get
                {
                    if (!GetValided()) { return 0f; }

                    float DmgBonusBase = ((StatS.AttackPower * combatFactors.MainHand.Speed) / 14f + (combatFactors.MainHand.MaxDamage + combatFactors.MainHand.MinDamage) / 2f) * (743f / 300000f);
                    float DmgBonusO75 = 0.25f * 1.35f * DmgBonusBase;
                    float DmgBonusU75 = 0.75f * 1.00f * DmgBonusBase;
                    float DmgMod = (1f + StatS.BonusBleedDamageMultiplier + DamageBonus);

                    float TickSize = (DamageBase + DmgBonusO75 + DmgBonusU75) * DmgMod;
                    return TickSize;
                }
            }
        }
        public class DeepWounds : DoT {
            // Constructors
            /// <summary>
            /// Your critical strikes cause the opponent to bleed, dealing (16*Pts)% of your melee weapon's
            /// average damage over 6 sec.
            /// </summary>
            /// <TalentsAffecting>Deep Wounds (Requires Talent) [(16*Pts)% damage dealt]</TalentsAffecting>
            /// <GlyphsAffecting></GlyphsAffecting>
            public DeepWounds(Character c, Stats s, CombatFactors cf,WhiteAttacks wa) {
                Char = c;Talents = c.WarriorTalents;StatS = s;combatFactors = cf;Whiteattacks = wa;CalcOpts = Char.CalculationOptions as CalculationOptionsDPSWarr;
                //
                Name = "Deep Wounds";
                ReqTalent = true;
                Talent2ChksValue = Talents.DeepWounds;
                ReqMeleeWeap = true;
                ReqMeleeRange = true;
                Duration = 6f; // In Seconds
                TimeBtwnTicks = 1f; // In Seconds
                StanceOkFury = StanceOkArms = StanceOkDef  = true;
                mhActivates = ohActivates = 0;
            }
            // Variables
            private float mhActivates, ohActivates;
            public void SetAllAbilityActivates(float mh, float oh) { 
                mhActivates = mh * combatFactors.MhYellowCrit + RotationLength / combatFactors.MainHandSpeed * combatFactors.MhCrit;
                if (Char.OffHand == null) {
                    ohActivates = 0f;
                }else{
                    ohActivates = oh * combatFactors.OhYellowCrit + RotationLength / combatFactors.OffHandSpeed * combatFactors.OhCrit; 
                }
            }
            // Get/Set
            // Functions
            public override float Activates
            {
                get
                {
                    if (!GetValided()) { return 0f; }
                    //SetAllAbilityActivates(0,0);
                    //return mhActivates + ohActivates;
                    float DWsacts = RotationLength / TTLTickingTime;
                    return DWsacts;
                }
            }
            public override float TickSize
            {
                get
                {
                    if (!GetValided()) { return 0f; }

                    // doing it this way because Deep Wounds triggering off of a MH crit and Deep Wounds triggering off of an OH crit do diff damage.
                    // Damage stores the average damage of single deep wounds trigger
                    float Damage = combatFactors.AvgMhWeaponDmg * (0.16f * Talents.DeepWounds) * mhActivates / (mhActivates + ohActivates) +
                                   combatFactors.AvgOhWeaponDmg * (0.16f * Talents.DeepWounds) * ohActivates / (mhActivates + ohActivates);

                    Damage *= (1f + StatS.BonusBleedDamageMultiplier);
                    Damage *= combatFactors.DamageBonus;

                    // Removed, titan's grip is included in DamageBonus
                    //if (Talents.TitansGrip == 1 && Char.OffHand != null && Char.OffHand.Slot == Item.ItemSlot.TwoHand) { Damage *= 0.9f; } // Titan's Grip penalty, since we're not modifying by combatFactors.DamageReduction

                    // Because Deep Wounds is rolling, each tick is compounded by total number of times it's activated over its duration
                    Damage = Damage * (mhActivates+ohActivates) * Duration / RotationLength;

                    // Tick size
                    Damage = Damage / Duration * TimeBtwnTicks;

                    // Ensure we're not doing negative damage
                    return Math.Max(0, Damage);
                }
            }
        }
        #endregion
        #region BuffEffect Abilities
        public class BuffEffect : Ability {
            // Constructors
            public BuffEffect(){}
            // Variables
            private SpecialEffect EFFECT;
            private float DURATION; // In Seconds
            // Get/Set
            public SpecialEffect Effect { get { return EFFECT; } set { EFFECT = value; } }
            public float Duration { get { return DURATION; } set { DURATION = value; } } // In Seconds
            // Functions
            public virtual Stats AverageStats
            {
                get
                {
                    Stats bonus = Effect.GetAverageStats(0f, 1f, combatFactors.MainHand.Speed, CalcOpts.Duration);
                    return bonus;
                }
            }
        }
        public class Trauma : BuffEffect {
            // Constructors
            /// <summary>
            /// Your melee critical strikes increase the effectiveness of Bleed Effects on the
            /// target by (15*Pts)% for 15 sec.
            /// </summary>
            /// <TalentsAffecting>Trauma (Requires Talent)</TalentsAffecting>
            /// <GlyphsAffecting></GlyphsAffecting>
            public Trauma(Character c, Stats s, CombatFactors cf,WhiteAttacks wa) {
                Char = c;
                Talents = c.WarriorTalents;
                StatS = s;
                combatFactors = cf;
                Whiteattacks = wa;
                CalcOpts = Char.CalculationOptions as CalculationOptionsDPSWarr;
                Name = "Trauma";
                ReqMeleeWeap = false;
                ReqMeleeRange = true;
                MaxRange = 5f; // In Yards 
                Cd = -1f; // In Seconds
                Duration = 15f; // In Seconds
                RageCost = -1f;
                CastTime = -1f; // In Seconds
                StanceOkFury = true;
                StanceOkArms = true;
                StanceOkDef = true;
                Effect = new SpecialEffect(Trigger.MeleeCrit, StatS, Duration, 0f);
            }
            // Variables
            // Get/Set
            // Functions
            public override float Activates
            {
                get
                {
                    if (!GetValided() || Talents.Trauma == 0) { return 0f; }
                    // Chance to activate on every GCD
                    return (RotationLength / 1.5f) * combatFactors.MhCrit;
                }
            }
        }
        public class BerserkerRage : BuffEffect {
            /// <summary>
            /// Instant, 30 sec Cd, Self, (Any)
            /// The warrior enters a berserker rage, becoming immune to Fear, Sap and Incapacitate effects
            /// and generating extra tage when taking damage. Lasts 10 sec.
            /// </summary>
            /// <TalentsAffecting>Improved Berserker Rage [+(10*Pts) Rage Generated]</TalentsAffecting>
            /// <GlyphsAffecting></GlyphsAffecting>
            public BerserkerRage(Character c, Stats s, CombatFactors cf, WhiteAttacks wa) {
                Char = c; Talents = c.WarriorTalents; StatS = s; combatFactors = cf; Whiteattacks = wa; CalcOpts = Char.CalculationOptions as CalculationOptionsDPSWarr;
                //
                Name = "Berserker Rage";
                Cd = 30f * (1f - 1f/9f*Talents.IntensifyRage); // In Seconds
                RageCost = 0f + (Talents.ImprovedBerserkerRage * 10f); // This is actually reversed in the rotation
                StanceOkArms = StanceOkDef = StanceOkFury = true;
            }
        }
        public class EnragedRegeneration : BuffEffect {
            /// <summary>
            /// Instant, 3 min Cd, 15 Rage, Self, (Any)
            /// You regenerate 30% of your total health over 10 sec. This ability requires an Enrage effect,
            /// consumes all Enrage effects and prevents any from affecting you for the full duration.
            /// </summary>
            /// <TalentsAffecting></TalentsAffecting>
            /// <GlyphsAffecting>Glyph of Enraged Regeneration [+10% to effect]</GlyphsAffecting>
        }
        public class LastStand : BuffEffect {
            /// <summary>
            /// Instant, 5 min Cd, Self, (Def)
            /// 
            /// </summary>
            /// <TalentsAffecting>Last Stand [Requires Talent]</TalentsAffecting>
            /// <GlyphsAffecting>Glyph of Last Stand [-1 min Cd]</GlyphsAffecting>
        }
        public class Bloodrage : BuffEffect {
            /// <summary>
            /// Instant, 1 min cd, Self (Any)
            /// Generates 10 rage at the cost of health and then generates an additional 10 rage over 10 sec.
            /// </summary>
            /// <TalentsAffecting>Improved Bloodrge [+(25*Pts)% Rage Generated], Intensify Rage [-(1/9*Pts]% Cooldown]</TalentsAffecting>
            /// <GlyphsAffecting>Glyph of Bloodrage [-100% Health Cost]</GlyphsAffecting>
            public Bloodrage(Character c, Stats s, CombatFactors cf, WhiteAttacks wa) {
                Char = c; Talents = c.WarriorTalents; StatS = s; combatFactors = cf; Whiteattacks = wa; CalcOpts = Char.CalculationOptions as CalculationOptionsDPSWarr;
                //
                Name = "Bloodrage";
                Cd = 60f * (1f - 1f/9f*Talents.IntensifyRage); // In Seconds
                Duration = 10f; // In Seconds
                RageCost = 10f * (1f + Talents.ImprovedBloodrage * 0.25f); // This is actually reversed in the rotation
                StanceOkArms = StanceOkDef = StanceOkFury = true;
                Effect = new SpecialEffect(Trigger.Use,
                    new Stats() { BonusRageGen = 1f * (1f + Talents.ImprovedBloodrage * 0.25f), },
                    Duration, Cd);
            }
        }
        public class BattleShout : BuffEffect {
            // Constructors
            /// <summary>
            /// The warrior shouts, increasing attack power of all raid and party members within 20 yards by 548. Lasts 2 min.
            /// </summary>
            /// <TalentsAffecting>
            /// Booming Voice [+(25*Pts)% AoE and Duration],
            /// Commanding Presence [+(5*Pts)% to the AP Bonus]
            /// </TalentsAffecting>
            /// <GlyphsAffecting>Glyph of Battle [+1 min duration]</GlyphsAffecting>
            public BattleShout(Character c, Stats s, CombatFactors cf,WhiteAttacks wa) {
                Char = c;Talents = c.WarriorTalents;StatS = s;combatFactors = cf;Whiteattacks = wa;CalcOpts = Char.CalculationOptions as CalculationOptionsDPSWarr;
                //
                Name = "Battle Shout";
                MaxRange = 20f * (1f + Talents.BoomingVoice * 0.25f); // In Yards 
                Duration = (2f+(Talents.GlyphOfBattle?1f:0f))*60f * (1f + Talents.BoomingVoice * 0.25f);
                RageCost = 10f;
                StanceOkFury = StanceOkArms = StanceOkDef = true;
                Effect = new SpecialEffect(Trigger.Use,
                    new Stats() { AttackPower = (528f*(1f+Talents.CommandingPresence*0.05f)), },
                    Duration, Duration);
            }
            // Variables
            // Get/Set
            // Functions
            public override float Activates
            {
                get
                {
                    if (!GetValided()) { return 0f; }
                    //Effect.GetAverageProcsPerSecond(...);
                    float result = RotationLength / Duration;
                    return result;
                }
            }
        }
        public class DeathWish : BuffEffect {
            // Constructors
            /// <summary>
            /// When activated you become enraged, increasing your physical damage by 20% but increasing
            /// all damage taken by 5%. Lasts 30 sec.
            /// </summary>
            /// <TalentsAffecting>Death Wish [Requires Talent], Intensify Rage [-(1/9*Pts)% Cooldown]</TalentsAffecting>
            /// <GlyphsAffecting></GlyphsAffecting>
            public DeathWish(Character c, Stats s, CombatFactors cf, WhiteAttacks wa) {
                Char = c; Talents = c.WarriorTalents; StatS = s; combatFactors = cf; Whiteattacks = wa; CalcOpts = Char.CalculationOptions as CalculationOptionsDPSWarr;
                //
                Name = "Death Wish";
                ReqTalent = true;
                Talent2ChksValue = Talents.DeathWish;
                MaxRange = 20f; // In Yards 
                Cd = 3f * 60f * (1f - 1f/9f * Talents.IntensifyRage); // In Seconds
                Duration = 30f;
                RageCost = 10f;
                StanceOkArms = true;
                Effect = new SpecialEffect(Trigger.Use,
                        new Stats() { BonusDamageMultiplier = 0.20f, DamageTakenMultiplier = 0.05f, },
                        Duration, Cd);
            }
            // Variables
            // Get/Set
            // Functions
            public override float Activates
            {
                get
                {
                    if (!GetValided()) { return 0f; }
                    //Effect.GetAverageProcsPerSecond(...);
                    float result = RotationLength / Cd;
                    return result;
                }
            }
        }
        public class Recklessness : BuffEffect {
            // Constructors
            /// <summary>
            /// Your next 3 special ability attacks have an additional 100% to critically hit
            /// but all damage taken is increased by 20%. Lasts 12 sec.
            /// </summary>
            /// <TalentsAffecting>Improved Disciplines [-(30*Pts) sec Cd]</TalentsAffecting>
            /// <GlyphsAffecting></GlyphsAffecting>
            public Recklessness(Character c, Stats s, CombatFactors cf, WhiteAttacks wa) {
                Char = c;
                Talents = c.WarriorTalents;
                StatS = s;
                combatFactors = cf;
                Whiteattacks = wa;
                CalcOpts = Char.CalculationOptions as CalculationOptionsDPSWarr;
                Name = "Recklessness";
                MaxRange = 5f; // In Yards
                Cd = (5f * 60f - Talents.ImprovedDisciplines * 30f) * (1f - 1f/9f*Talents.IntensifyRage); // In Seconds
                Duration = 12f; // In Seconds
                StanceOkFury = true;
                Effect = new SpecialEffect(Trigger.Use,
                    new Stats { PhysicalCrit = 1f, DamageTakenMultiplier = 0.20f, },
                    Duration, Cd);
            }
            // Variables
            // Get/Set
            // Functions
            public override float Activates
            {
                get
                {
                    //Effect.GetAverageProcsPerSecond(...);
                    return base.Activates;
                }
            }
        }
        public class SweepingStrikes : BuffEffect {
            // Constructors
            /// <summary>
            /// Your next 5 melee attacks strike an additional nearby opponent.
            /// </summary>
            /// <TalentsAffecting>Sweeping Strikes [Requires Talent]</TalentsAffecting>
            /// <GlyphsAffecting>Glyph of Sweeping Strikes [-100% Rage cost]</GlyphsAffecting>
            public SweepingStrikes(Character c, Stats s, CombatFactors cf,WhiteAttacks wa) {
                Char = c;Talents = c.WarriorTalents;StatS = s;combatFactors = cf;Whiteattacks = wa;CalcOpts = Char.CalculationOptions as CalculationOptionsDPSWarr;
                //
                Name = "Sweeping Strikes";
                ReqTalent = true;
                Talent2ChksValue = Talents.SweepingStrikes;
                ReqMeleeWeap = true;
                ReqMeleeRange = true;
                ReqMultiTargs = true;
                Cd = 30f; // In Seconds
                Duration = 4f; // Using 4 seconds to sim consume time
                RageCost = 30f - (Talents.FocusedRage * 1f);
                RageCost = (Talents.GlyphOfSweepingStrikes ? 0f : RageCost);
                CastTime = -1f; // In Seconds
                StanceOkFury = StanceOkArms = true;
                Effect = new SpecialEffect(Trigger.Use, new Stats() { BonusTargets = 1f * CalcOpts.MultipleTargetsPerc / 100f, }, Duration, Cd);
            }
            // Variables
            // Get/Set
            // Functions
            public override float Activates
            {
                get
                {
                    //Effect.GetAverageProcsPerSecond(...);
                    return base.Activates;
                }
            }
            public override Stats AverageStats
            {
                get
                {
                    Stats bonus = Effect.GetAverageStats(
                        0f,
                        1f - combatFactors.YellowMissChance - combatFactors.MhDodgeChance, // The additional hit also has the attack table to deal with
                        combatFactors.MainHand.Speed,
                        CalcOpts.Duration);
                    return bonus;
                }
            }
        }
        #endregion
        #region DeBuff Abilities
        public class SunderArmor : BuffEffect {
            // Constructors
            /// <summary>
            /// 
            /// </summary>
            /// <TalentsAffecting>Focused Rage [-(Pts) Rage Cost], Puncture [-(Pts) Rage Cost], </TalentsAffecting>
            /// <GlyphsAffecting>Glyph of Sunder Armor [+1 Targets]</GlyphsAffecting>
            public SunderArmor(Character c, Stats s, CombatFactors cf, WhiteAttacks wa) {
                Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa;
                //
                Name = "Sunder Armor";
                ReqMeleeWeap = true;
                ReqMeleeRange = true;
                Duration = 30f; // In Seconds
                Cd = 1.5f;
                RageCost = 15f - (Talents.FocusedRage * 1f) - (Talents.Puncture * 1f);
                Targets = 1f + (Talents.GlyphOfSunderArmor ? 1f : 0f);
                StanceOkFury = StanceOkArms = StanceOkDef = true;
                Effect = new SpecialEffect(Trigger.Use,
                    new Stats() { ArmorPenetration = 0.04f, },
                    Duration, Cd, 1f, 5);
            }
            public override float Activates { get { return base.Activates * Cd / Duration; } }
        }
        public class ShatteringThrow : BuffEffect {
            // Constructors
            /// <summary>
            /// Throws your weapon at the enemy causing (12+AP*0.50) damage (based on attack power),
            /// reducing the armor on the target by 20% for 10 sec or removing any invulnerabilities.
            /// </summary>
            /// <TalentsAffecting></TalentsAffecting>
            /// <GlyphsAffecting></GlyphsAffecting>
            public ShatteringThrow(Character c, Stats s, CombatFactors cf, WhiteAttacks wa) {
                Char = c;Talents = c.WarriorTalents;StatS = s;combatFactors = cf;Whiteattacks = wa;CalcOpts = Char.CalculationOptions as CalculationOptionsDPSWarr;
                //
                Name = "Shattering Throw";
                ReqMeleeWeap = true;
                ReqMeleeRange = false;
                MaxRange = 30f; // In Yards 
                Cd = 2f * 60f; // In Seconds
                Duration = 10f;
                RageCost = 25f;
                StanceOkArms = true;
                Effect = new SpecialEffect(Trigger.Use,
                    new Stats() { ArmorPenetration = 0.20f, },
                    Duration, Cd);
            }
            // Variables
            // Get/Set
            // Functions
            public override float Activates
            {
                get
                {
                    // Invalidators
                    if (!GetValided()) { return 0f; }

                    float result = RotationLength / Cd;

                    return result;
                }
            }
        }
        public class DemoralizingShout : BuffEffect {
            // Constructors
            /// <summary>
            /// Reduces the melee attack power of all enemies within 10 yards by 411 for 30 sec.
            /// </summary>
            /// <TalentsAffecting></TalentsAffecting>
            /// <GlyphsAffecting></GlyphsAffecting>
            public DemoralizingShout(Character c, Stats s, CombatFactors cf, WhiteAttacks wa) {
                Char = c;Talents = c.WarriorTalents;StatS = s;combatFactors = cf;Whiteattacks = wa;CalcOpts = Char.CalculationOptions as CalculationOptionsDPSWarr;
                //
                Name = "Demoralizing Shout";
                ReqMeleeWeap = false;
                ReqMeleeRange = false;
                MaxRange = 10f; // In Yards 
                Duration = 30f;
                RageCost = 10f;
                StanceOkArms = StanceOkFury = true;
                Effect = new SpecialEffect(Trigger.Use,
                    new Stats() { AttackPower = 411f, }, // needs to be boss debuff
                    Duration, Duration);
            }
            // Variables
            // Get/Set
            // Functions
            public override float Activates
            {
                get
                {
                    if (!GetValided()) { return 0f; }
                    //Effect.GetAverageProcsPerSecond(...);
                    float result = RotationLength / Duration;
                    return result;
                }
            }
        }
        public class Hamstring : BuffEffect {
            /// <summary>
            /// Instant, No cd, 10 Rage, Melee Range, Melee Weapon, (Battle/Zerker)
            /// Maims the enemy, reducing movement speed by 50% for 15 sec.
            /// </summary>
            /// <TalentsAffecting></TalentsAffecting>
            /// <GlyphsAffecting>Glyph of Hamstring [Gives your Hamstring ability a 10% chance to immobilize the target for 5 sec.]</GlyphsAffecting>
        }
        #endregion
        #region Anti-Debuff Abilities
        public class EveryManForHimself : Ability {
            /// <summary>
            /// Instant, 2 Min Cooldown, 0 Rage, Self (Any)
            /// Removes all movement impairing effects and all effects which cause loss of control of
            /// your character. This effect shares a cooldown with other similar effects.
            /// </summary>
            /// <TalentsAffecting></TalentsAffecting>
            /// <GlyphsAffecting></GlyphsAffecting>
        }
        #endregion
        #region Movement Abilities
        public class Charge : Ability {
            /// <summary>
            /// Instant, 20 sec cd, 0 Rage, 8-25 yds, (Battle)
            /// Charge an enemy, generate 25 rage, and stun it for 1.50 sec. Cannot be used in combat.
            /// </summary>
            /// <TalentsAffecting>
            /// Warbringer [Usable in combat and any stance]
            /// Juggernaut [Usable in combat]
            /// Improved Charge [+(5*Pts) RageGen].
            /// </TalentsAffecting>
            /// <GlyphsAffecting>
            /// Glyph of Rapid Charge [-20% Cd]
            /// Glyph of Charge [+5 yds MaxRange]
            /// </GlyphsAffecting>
        }
        public class Intercept : Ability {
            /// <summary>
            /// Instant, 30 sec Cd, 10 Rage, 8-25 yds, (Zerker)
            /// Charge an enemy, causing 380 damage (based on attack power) and stunning it for 3 sec.
            /// </summary>
            /// <TalentsAffecting>
            /// Warbringer [Usable in any stance]
            /// </TalentsAffecting>
            /// <GlyphsAffecting></GlyphsAffecting>
        }
        public class Intervene : Ability {
            /// <summary>
            /// Instant, 30 sec Cd, 10 Rage, 8-25 yds, (Def)
            /// Charge an enemy, causing 380 damage (based on attack power) and stunning it for 3 sec.
            /// </summary>
            /// <TalentsAffecting>
            /// Warbringer [Usable in any stance]
            /// </TalentsAffecting>
            /// <GlyphsAffecting>
            /// Glyph of Intervene [Increases the number of attacks you intercept for your intervene target by 1.]
            /// </GlyphsAffecting>
        }
        #endregion
        #region Other Abilities
        public class Retaliation : Ability {
            /// <summary>
            /// Instant, 5 Min Cd, No Rage, Melee Range, Melee Weapon, (Battle)
            /// Instantly counterattack any enemy that strikes you in melee for 12 sec. Melee attacks
            /// made from behind cannot be counterattacked. A maximum of 20 attacks will cause retaliation.
            /// </summary>
            /// <TalentsAffecting>Improved Disciplines [-(30*Pts) sec Cd]</TalentsAffecting>
            /// <GlyphsAffecting></GlyphsAffecting>
        }
        #endregion
    }
}
