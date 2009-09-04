using System;

namespace Rawr.DPSWarr {
    public class Skills {
        #region Base
        public enum AttackTableSelector { Missed = 0, Dodged, Parried, Blocked, Crit, Glance, Hit }
        // White Damage + White Rage Generated
        public class WhiteAttacks {
            // Constructors
            public WhiteAttacks(Character character, Stats stats, CombatFactors cf) {
                Char = character;
                StatS = stats;
                Talents = Char.WarriorTalents == null ? new WarriorTalents() : Char.WarriorTalents;
                combatFactors = cf;
                CalcOpts = Char.CalculationOptions as CalculationOptionsDPSWarr;
                MHAtkTable = new AttackTable(Char, StatS, combatFactors, true);
                OHAtkTable = new AttackTable(Char, StatS, combatFactors, false);
                //
                Targets = 1f;
                Ovd_Freq = 0f;
                Slam_Freq = 0f;
            }
            #region Variables
            private readonly Character Char;
            private readonly Stats StatS;
            private readonly WarriorTalents Talents;
            private readonly CombatFactors combatFactors;
            private CalculationOptionsDPSWarr CalcOpts;
            public AttackTable MHAtkTable;
            public AttackTable OHAtkTable;
            private float OVD_FREQ;
            public float Targets;
            // Get/Set
            public float Ovd_Freq { get { return OVD_FREQ; } set { OVD_FREQ = value; } }
            public float Slam_Freq;
            #endregion
            // bah
            private float SlamFreqSpdMod { get { return (Slam_Freq == 0f ? 0f : ((1.5f - (0.5f * Talents.ImprovedSlam)) * (Slam_Freq/CalcOpts.Duration))); } }
            // Main Hand
            public float MhEffectiveSpeed { get { return combatFactors.MHSpeed + SlamFreqSpdMod; } }
            public float MhDamage {
                get {
                    float DamageBase = combatFactors.AvgMhWeaponDmgUnhasted;
                    float DamageBonus = 1f + 0f;
                    return (float)Math.Max(0f, DamageBase * DamageBonus);
                }
            }
            public float MhDamageOnUse {
                get {
                    float dmg = MhDamage;                  // Base Damage
                    dmg *= combatFactors.DamageBonus;      // Global Damage Bonuses
                    dmg *= combatFactors.DamageReduction;  // Global Damage Penalties
                    
                    // Work the Attack Table
                    float dmgDrop = (1f
                        - MHAtkTable.Miss   // no damage when being missed
                        - MHAtkTable.Dodge  // no damage when being dodged
                        - MHAtkTable.Parry  // no damage when being parried
                        - MHAtkTable.Glance // glancing handled below
                        - MHAtkTable.Block  // blocked handled below
                        - MHAtkTable.Crit); // crits   handled below

                    float dmgGlance = dmg * MHAtkTable.Glance * combatFactors.ReducWhGlancedDmg ;//Partial Damage when glancing
                    float dmgBlock  = dmg * MHAtkTable.Block * combatFactors.ReducWhBlockedDmg ;//Partial damage when blocked
                    float dmgCrit   = dmg * MHAtkTable.Crit * (1f+combatFactors.BonusWhiteCritDmg);//Bonus Damage when critting

                    dmg *= dmgDrop;

                    dmg += dmgGlance + dmgBlock + dmgCrit;

                    return (float)Math.Max(0f, dmg);
                }
            }
            public float AvgMhDamageOnUse { get { return MhDamageOnUse * MhActivates; } }
            public float MhActivates { get { return (float)Math.Max(0f, CalcOpts.Duration / MhEffectiveSpeed * (1f - Ovd_Freq)); } }
            public float MhDPS { get { return AvgMhDamageOnUse / CalcOpts.Duration; } }
            // Off Hand
            public float OhEffectiveSpeed { get { return combatFactors.OHSpeed + SlamFreqSpdMod; } }
            public float OhDamage {
                get {
                    float DamageBase = combatFactors.AvgOhWeaponDmgUnhasted;
                    float DamageBonus = 1f + 0f;
                    return (float)Math.Max(0f, DamageBase * DamageBonus);
                }
            }
            public float OhDamageOnUse {
                get {
                    float dmg = OhDamage;                  // Base Damage
                    dmg *= combatFactors.DamageBonus;      // Global Damage Bonuses
                    dmg *= combatFactors.DamageReduction;  // Global Damage Penalties

                    // Work the Attack Table
                    float dmgDrop = (1f
                        - OHAtkTable.Miss   // no damage when being missed
                        - OHAtkTable.Dodge  // no damage when being dodged
                        - OHAtkTable.Parry  // no damage when being parried
                        - OHAtkTable.Glance // glancing handled below
                        - OHAtkTable.Block  // blocked handled below
                        - OHAtkTable.Crit); // crits handled below

                    float dmgGlance = dmg * OHAtkTable.Glance *     combatFactors.ReducWhGlancedDmg ;//Partial Damage when glancing
                    float dmgBlock  = dmg * OHAtkTable.Block  *     combatFactors.ReducWhBlockedDmg ;//Partial damage when blocked
                    float dmgCrit   = dmg * OHAtkTable.Crit   * (1f+combatFactors.BonusWhiteCritDmg);//Bonus   Damage when critting

                    dmg *= dmgDrop;

                    dmg += dmgGlance + dmgBlock + dmgCrit;

                    return (float)Math.Max(0f, dmg);
                }
            }
            public float AvgOhDamageOnUse { get { return OhDamageOnUse * OhActivates; } }
            public float OhActivates { get { return (float)Math.Max(0f, CalcOpts.Duration / OhEffectiveSpeed); } }
            public float OhDPS { get { return AvgOhDamageOnUse / CalcOpts.Duration; } }
            // Rage Calcs
            public float MHSwingRage {
                get {
                    // d = damage amount
                    // s = weapon speed
                    // f = hit factor
                    float d, s, f, rage;
                    float based;

                    rage = 0.0f;
                    s = MhEffectiveSpeed;
                    based = combatFactors.AvgMhWeaponDmg(s) * combatFactors.DamageBonus * combatFactors.DamageReduction;

                    // regular hit
                    d = based;
                    f = 3.5f;
                    rage += RageFormula(d, s, f) * MHAtkTable.Hit;

                    // glance
                    d = based * combatFactors.ReducWhGlancedDmg;
                    rage += RageFormula(d, s, f) * MHAtkTable.Glance;

                    // crit
                    d = based * (1f + combatFactors.BonusWhiteCritDmg);
                    f = 7.0f;
                    rage += RageFormula(d, s, f) * MHAtkTable.Crit;

                    rage += s * (3f * Talents.UnbridledWrath) / 60.0f * (1.0f - MHAtkTable.AnyLand);

                    return rage;
                }
            }
            public float OHSwingRage {
                get {
                    // d = damage amount
                    // s = weapon speed
                    // f = hit factor
                    float d, s, f, rage;
                    float based;

                    rage = 0.0f;
                    s = OhEffectiveSpeed;
                    based = combatFactors.AvgOhWeaponDmg(s) * combatFactors.DamageBonus * combatFactors.DamageReduction;

                    // regular hit
                    d = based;
                    f = 1.75f;
                    rage += RageFormula(d, s, f) * OHAtkTable.Hit;

                    // glance
                    d = based * combatFactors.ReducWhGlancedDmg;
                    rage += RageFormula(d, s, f) * OHAtkTable.Glance;

                    // crit
                    d = based * (1f + combatFactors.BonusWhiteCritDmg);
                    f = 3.5f;
                    rage += RageFormula(d, s, f) * OHAtkTable.Crit;

                    rage += s * (3f * Talents.UnbridledWrath) / 60.0f * (1.0f - OHAtkTable.AnyLand);
                    
                    return rage;
                }
            }
            public float MHRageGenPerSec {
                get {
                    float result = 0f;
                    if (combatFactors.MH != null && combatFactors.MH.MaxDamage > 0) {
                        float FightDur = CalcOpts.Duration;
                        float cd = MhEffectiveSpeed;
                        float ragePer = MHSwingRage;
                        float acts = FightDur / cd;
                        float ovdMod = (1f - Ovd_Freq);
                        result = (acts * ragePer) / FightDur * ovdMod;
                    }
                    return result;
                }
            }
            public float OHRageGenPerSec {
                get {
                    float result = 0f;
                    if (combatFactors.OH != null && combatFactors.OH.MaxDamage > 0) {
                        float FightDur = CalcOpts.Duration;
                        float cd = OhEffectiveSpeed;
                        float ragePer = OHSwingRage;
                        float acts = FightDur / cd;
                        result = (acts * ragePer) / FightDur;
                    }
                    return result;
                }
            }
            // Rage generated per second
            public float MHRageRatio {
                get {
                    float realMHRage = MHRageGenPerSec;
                    float realOverallRage = realMHRage + OHRageGenPerSec;
                    return realMHRage / realOverallRage;
                }
            }
            public float whiteRageGenPerSec { get { return MHRageGenPerSec + OHRageGenPerSec; } }
            public float RageFormula(float d, float s, float f) {
                /* R = Rage Generated
                 * d = damage amount
                 * c = rage conversion value
                 * s = weapon speed
                 * f = hit factor */
                float c = 453.3f;
                //if (Char.Level != 80) c = 0.0091107836f * Char.Level * Char.Level + 3.225598133f * Char.Level + 4.2652911f; // = ~320.6;
                float dmgRage = 7.5f * d / c;
                float rps = f * s; // 3.5rage/sec baseline
                float R = System.Math.Min((dmgRage + rps) / 2.0f, dmgRage*2.0f);
                
                //R = 3.75f * d / c + f * s / 2.0f;
                R *= (1.0f + 0.25f * Talents.EndlessRage);
                return R;
            }
            // Attacks Over Fight Duration
            public float LandedAtksOverDur {
                get {
                    float whiteLands = LandedAtksOverDurMH + LandedAtksOverDurOH;
                    return whiteLands;
                }
            }
            public float LandedAtksOverDurMH {
                get {
                    float whiteLands = MhActivates * MHAtkTable.AnyLand;
                    return whiteLands;
                }
            }
            public float LandedAtksOverDurOH {
                get {
                    float whiteLands = (combatFactors.OH != null ? OhActivates * OHAtkTable.AnyLand : 0f);
                    return whiteLands;
                }
            }
            public float CriticalAtksOverDur {
                get {
                    float whiteLands = CriticalAtksOverDurMH + CriticalAtksOverDurOH;
                    return whiteLands;
                }
            }
            public float CriticalAtksOverDurMH {
                get {
                    float whiteLands = MhActivates * MHAtkTable.Crit;
                    return whiteLands;
                }
            }
            public float CriticalAtksOverDurOH {
                get {
                    float whiteLands = (combatFactors.OH != null ? OhActivates * OHAtkTable.Crit : 0f);
                    return whiteLands;
                }
            }
            // Other
            public float AvoidanceStreak {
                get {
                    float mhRagePercent = MHRageRatio;
                    float ohRagePercent = 1f - mhRagePercent;
                    float missChance = mhRagePercent * (MHAtkTable.Miss + MHAtkTable.Dodge + MHAtkTable.Parry) +
                                       ohRagePercent * (OHAtkTable.Miss + OHAtkTable.Dodge + OHAtkTable.Parry);
                    float doubleChance = missChance * missChance;
                    float tripleChance = doubleChance * missChance;
                    float quadChance = doubleChance * doubleChance;

                    float doubleRecovery = 1f / (1f / MhEffectiveSpeed + 1f / OhEffectiveSpeed);
                    float tripleRecovery = 1f / (1f / (1.5f * MhEffectiveSpeed) + 1f / (1.5f * OhEffectiveSpeed));
                    float quadRecovery = 1f / (1f / (2f * MhEffectiveSpeed) + 1f / (2f * OhEffectiveSpeed));

                    float doubleSlip = doubleChance * doubleRecovery;
                    float tripleSlip = tripleChance * tripleRecovery;
                    float quadSlip = quadChance * quadRecovery;
                    return doubleSlip + tripleSlip + quadSlip;
                }
            }
            public virtual float GetXActs(AttackTableSelector i,float acts,bool isMH) {
                AttackTable table = (isMH ? MHAtkTable : OHAtkTable);
                float retVal = 0f;
                switch (i) {
                    case AttackTableSelector.Missed:  { retVal = acts * table.Miss;  break; }
                    case AttackTableSelector.Dodged:  { retVal = acts * table.Dodge; break; }
                    case AttackTableSelector.Parried: { retVal = acts * table.Parry; break; }
                    case AttackTableSelector.Blocked: { retVal = acts * table.Block; break; }
                    case AttackTableSelector.Crit:    { retVal = acts * table.Crit;  break; }
                    case AttackTableSelector.Glance:  { retVal = acts * table.Glance;break; }
                    case AttackTableSelector.Hit:     { retVal = acts * table.Hit;   break; }
                    default : {break;}
                }
                return retVal;
            }
            public virtual string GenTooltip(float ttldpspercMH, float ttldpspercOH) {
                // ==== MAIN HAND ====
                float acts = MhActivates;
                float misses = GetXActs(AttackTableSelector.Missed, acts, true), missesPerc = (acts == 0f ? 0f : misses / acts);
                float dodges = GetXActs(AttackTableSelector.Dodged, acts, true), dodgesPerc = (acts == 0f ? 0f : dodges / acts);
                float parrys = GetXActs(AttackTableSelector.Parried, acts, true), parrysPerc = (acts == 0f ? 0f : parrys / acts);
                float blocks = GetXActs(AttackTableSelector.Blocked, acts, true), blocksPerc = (acts == 0f ? 0f : blocks / acts);
                float crits  = GetXActs(AttackTableSelector.Crit, acts, true), critsPerc = (acts == 0f ? 0f : crits / acts);
                float glncs  = GetXActs(AttackTableSelector.Glance, acts, true), glncsPerc = (acts == 0f ? 0f : glncs / acts);
                float hits   = GetXActs(AttackTableSelector.Hit, acts, true), hitsPerc = (acts == 0f ? 0f : hits / acts);

                bool showmisss = misses > 0f;
                bool showdodge = dodges > 0f;
                bool showparry = parrys > 0f;
                bool showblock = blocks > 0f;
                bool showcrits = crits  > 0f;

                string tooltip = "*" + "White Damage (Main Hand)" +
                    Environment.NewLine + "Cast Time: Instant"
                                        + ", CD: " + (MhEffectiveSpeed != -1 ? MhEffectiveSpeed.ToString("0.00") : "None")
                                        + ", Rage Generated: " + (MHSwingRage != -1 ? MHSwingRage.ToString("0.00") : "None") +
                Environment.NewLine + Environment.NewLine + acts.ToString("000.00") + " Activates over Attack Table:" +
                (showmisss ? Environment.NewLine + "- " + misses.ToString("000.00") + " : " + missesPerc.ToString("00.00%") + " : Missed " : "") +
                (showdodge ? Environment.NewLine + "- " + dodges.ToString("000.00") + " : " + dodgesPerc.ToString("00.00%") + " : Dodged " : "") +
                (showparry ? Environment.NewLine + "- " + parrys.ToString("000.00") + " : " + parrysPerc.ToString("00.00%") + " : Parried " : "") +
                (showblock ? Environment.NewLine + "- " + blocks.ToString("000.00") + " : " + blocksPerc.ToString("00.00%") + " : Blocked " : "") +
                (showcrits ? Environment.NewLine + "- " + crits.ToString("000.00") + " : " + critsPerc.ToString("00.00%") + " : Crit " : "") +
                             Environment.NewLine + "- " + glncs.ToString("000.00") + " : " + glncsPerc.ToString("00.00%") + " : Glanced " +
                             Environment.NewLine + "- " + hits.ToString("000.00") + " : " + hitsPerc.ToString("00.00%") + " : Hit " +
                    Environment.NewLine +
                    //Environment.NewLine + "Damage per Blocked|Hit|Crit: x|x|x" +
                    Environment.NewLine + "Targets Hit: " + (Targets != -1 ? Targets.ToString("0.00") : "None") +
                    Environment.NewLine + "DPS: " + (MhDPS > 0 ? MhDPS.ToString("0.00") : "None") +
                    Environment.NewLine + "Percentage of Total DPS: " + (ttldpspercMH > 0 ? ttldpspercMH.ToString("00.00%") : "None");

                if (combatFactors.OH != null) {
                    // ==== OFF HAND ====
                    acts = OhActivates;
                    misses = GetXActs(AttackTableSelector.Missed, acts, false); missesPerc = (acts == 0f ? 0f : misses / acts);
                    dodges = GetXActs(AttackTableSelector.Dodged, acts, false); dodgesPerc = (acts == 0f ? 0f : dodges / acts);
                    parrys = GetXActs(AttackTableSelector.Parried, acts, false); parrysPerc = (acts == 0f ? 0f : parrys / acts);
                    blocks = GetXActs(AttackTableSelector.Blocked, acts, false); blocksPerc = (acts == 0f ? 0f : blocks / acts);
                    crits = GetXActs(AttackTableSelector.Crit, acts, false); critsPerc = (acts == 0f ? 0f : crits / acts);
                    glncs = GetXActs(AttackTableSelector.Glance, acts, false); glncsPerc = (acts == 0f ? 0f : glncs / acts);
                    hits = GetXActs(AttackTableSelector.Hit, acts, false); hitsPerc = (acts == 0f ? 0f : hits / acts);

                    showmisss = misses > 0f;
                    showdodge = dodges > 0f;
                    showparry = parrys > 0f;
                    showblock = blocks > 0f;
                    showcrits = crits  > 0f;

                    tooltip += Environment.NewLine + Environment.NewLine + "White Damage (Offhandn Hand)" +
                        Environment.NewLine + "Cast Time: Instant"
                                            + ", CD: " + (OhEffectiveSpeed != -1 ? OhEffectiveSpeed.ToString("0.00") : "None")
                                            + ", Rage Generated: " + (OHSwingRage != -1 ? OHSwingRage.ToString("0.00") : "None") +
                    Environment.NewLine + Environment.NewLine + acts.ToString("000.00") + " Activates over Attack Table:" +
                    (showmisss ? Environment.NewLine + "- " + misses.ToString("000.00") + " : " + missesPerc.ToString("00.00%") + " : Missed " : "") +
                    (showdodge ? Environment.NewLine + "- " + dodges.ToString("000.00") + " : " + dodgesPerc.ToString("00.00%") + " : Dodged " : "") +
                    (showparry ? Environment.NewLine + "- " + parrys.ToString("000.00") + " : " + parrysPerc.ToString("00.00%") + " : Parried " : "") +
                    (showblock ? Environment.NewLine + "- " + blocks.ToString("000.00") + " : " + blocksPerc.ToString("00.00%") + " : Blocked " : "") +
                    (showcrits ? Environment.NewLine + "- " + crits.ToString("000.00") + " : " + critsPerc.ToString("00.00%") + " : Crit " : "") +
                                 Environment.NewLine + "- " + glncs.ToString("000.00") + " : " + glncsPerc.ToString("00.00%") + " : Glanced " +
                                 Environment.NewLine + "- " + hits.ToString("000.00") + " : " + hitsPerc.ToString("00.00%") + " : Hit " +
                        Environment.NewLine +
                        //Environment.NewLine + "Damage per Blocked|Hit|Crit: x|x|x" +
                        Environment.NewLine + "Targets Hit: " + (Targets != -1 ? Targets.ToString("0.00") : "None") +
                        Environment.NewLine + "DPS: " + (OhDPS > 0 ? OhDPS.ToString("0.00") : "None") +
                        Environment.NewLine + "Percentage of Total DPS: " + (ttldpspercOH > 0 ? ttldpspercOH.ToString("00.00%") : "None");
                }
                return tooltip;
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
                MHAtkTable = null;
                OHAtkTable = null;
                Whiteattacks = null;
                CalcOpts = null;
                // Ability Related
                Name = "Invalid";
                ReqTalent = false;
                CanBeDodged = true;
                CanBeParried = true;
                CanBeBlocked = true;
                CanCrit = true;
                Talent2ChksValue = 0;
                AbilIterater = -1;
                ReqMeleeWeap = false;
                ReqMeleeRange = false;
                ReqMultiTargs = false;
                Targets  =  1f;
                MaxRange =  5f; // In Yards 
                CD       = -1f; // In Seconds
                Duration = -1f; // In Seconds
                RageCost = -1f;
                CastTime = -1f; // In Seconds
                StanceOkFury = false;
                StanceOkArms = false;
                StanceOkDef = false;
                DamageBase = 0f;
                DamageBonus = 1f;
                HealingBase = 0f;
                HealingBonus = 1f;
                BonusCritChance = 0.00f;
            }
            #region Variables
            private string NAME;
            private float DAMAGEBASE;
            private float DAMAGEBONUS;
            private float HEALINGBASE;
            private float HEALINGBONUS;
            private float BONUSCRITCHANCE;
            private bool CANBEDODGED;
            private bool CANBEPARRIED;
            private bool CANBEBLOCKED;
            private bool CANCRIT;
            private bool REQTALENT;
            private int TALENT2CHKSVALUE;
            private bool REQMELEEWEAP;
            private bool REQMELEERRANGE;
            private bool REQMULTITARGS;
            private float TARGETS;
            private float MAXRANGE; // In Yards 
            private float CD; // In Seconds
            private float DURATION; // In Seconds
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
            private AttackTable MHATTACKTABLE;
            private AttackTable OHATTACKTABLE;
            private WhiteAttacks WHITEATTACKS;
            private CalculationOptionsDPSWarr CALCOPTS;
            public int AbilIterater;
            #endregion
            #region Get/Set
            public float bloodsurgeRPS { get; set; }
            public string Name { get { return NAME; } set { NAME = value; } }
            public bool ReqTalent { get { return REQTALENT; } set { REQTALENT = value; } }
            public int Talent2ChksValue { get { return TALENT2CHKSVALUE; } set { TALENT2CHKSVALUE = value; } }
            public bool ReqMeleeWeap { get { return REQMELEEWEAP; } set { REQMELEEWEAP = value; } }
            public bool ReqMeleeRange { get { return REQMELEERRANGE; } set { REQMELEERRANGE = value; } }
            public bool ReqMultiTargs { get { return REQMULTITARGS; } set { REQMULTITARGS = value; } }
            public float AvgTargets {
                get {
                    if (CalcOpts.MultipleTargets) {
                        float extraTargetsHit = (float)Math.Min(CalcOpts.MultipleTargetsMax, TARGETS) - 1f;
                        return 1f + extraTargetsHit * CalcOpts.MultipleTargetsPerc / 100f + StatS.BonusTargets;
                    } else {return 1f;}
                }
            }
            public float Targets { get { return TARGETS; } set { TARGETS = value; } }
            public bool CanBeDodged { get { return CANBEDODGED; } set { CANBEDODGED = value; } }
            public bool CanBeParried { get { return CANBEPARRIED; } set { CANBEPARRIED = value; } }
            public bool CanBeBlocked { get { return CANBEBLOCKED; } set { CANBEBLOCKED = value; } }
            public bool CanCrit { get { return CANCRIT; } set { CANCRIT = value; } }
            public float MaxRange { get { return MAXRANGE; } set { MAXRANGE = value; } } // In Yards 
            public float Cd { // In Seconds
                get { return CD; }
                set {
                    /*float AssignedCD = value;
                    float LatentGCD = 1.5f + CalcOpts.GetLatency();
                    float CDs2Pass = 0f;
                    for (int count = 0; count < FightDuration; count++) {
                        CDs2Pass = count * LatentGCD;
                        if (CDs2Pass >= AssignedCD) { break; }
                    }
                    CD = CDs2Pass;
                    //*/CD = value;
                }
            }
            public float Duration { get { return DURATION; } set { DURATION = value; } } // In Seconds
            public float RageCost { get { return RAGECOST; } set { RAGECOST = value; } }
            public float CastTime { get { return CASTTIME; } set { CASTTIME = value; } } // In Seconds
            /// <summary>Base Damage Value (500 = 500.00 Damage)</summary>
            public float DamageBase { get { return DAMAGEBASE; } set { DAMAGEBASE = value; } }
            /// <summary>Percentage Based Damage Bonus (1.5 = 150% damage)</summary>
            public float DamageBonus { get { return DAMAGEBONUS; } set { DAMAGEBONUS = value; } }
            public float HealingBase { get { return HEALINGBASE; } set { HEALINGBASE = value; } }
            public float HealingBonus { get { return HEALINGBONUS; } set { HEALINGBONUS = value; } }
            public float BonusCritChance { get { return BONUSCRITCHANCE; } set { BONUSCRITCHANCE = value; } }
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
                        //combatFactors = new CombatFactors(CHARACTER, StatS);
                        //Whiteattacks = Whiteattacks;
                        CalcOpts = CHARACTER.CalculationOptions as CalculationOptionsDPSWarr;
                    }else{
                        Talents = null;
                        StatS = null;
                        //combatFactors = null;
                        MHAtkTable = null;
                        OHAtkTable = null;
                        Whiteattacks = null;
                        CalcOpts = null;
                    }
                }
            }
            public WarriorTalents Talents { get { return TALENTS; } set { TALENTS = value; } }
            public Stats StatS { get { return STATS; } set { STATS = value; } }
            public CombatFactors combatFactors { get { return COMBATFACTORS; } set { COMBATFACTORS = value; } }
            public AttackTable MHAtkTable { get { return MHATTACKTABLE; } set { MHATTACKTABLE = value; } }
            public AttackTable OHAtkTable { get { return OHATTACKTABLE; } set { OHATTACKTABLE = value; } }
            public WhiteAttacks Whiteattacks { get { return WHITEATTACKS; } set { WHITEATTACKS = value; } }
            public CalculationOptionsDPSWarr CalcOpts { get { return CALCOPTS; } set { CALCOPTS = value; } }
            public virtual float RageUsePerSecond { get { return (!Validated ? 0f : Activates * RageCost / FightDuration); } }
            public float FightDuration { get { return CalcOpts.Duration; } }
            public virtual bool Validated {
                get {
                    // Null crap is bad
                    if (Char == null || (Char != null && Char.MainHand == null) || CalcOpts == null || Talents == null) { return false; }
                    // Rotational Changes (Options Panel) (Arms Only right now)
                    if (AbilIterater != -1 && !CalcOpts.Maintenance[AbilIterater]) { return false; }
                    // Talent Requirements
                    if (ReqTalent && Talent2ChksValue < 1) { return false; }
                    // Need a weapon
                    if (ReqMeleeWeap && Char.MainHand.MaxDamage <= 0) { return false; }
                    // Need Multiple Targets or it's useless
                    if (ReqMultiTargs && !CalcOpts.MultipleTargets) { return false; }
                    // Proper Stance
                    if ((CalcOpts.FuryStance && !StanceOkFury)
                        || (!CalcOpts.FuryStance && !StanceOkArms)
                      /*|| ( CalcOpts.DefStance  && !StanceOkDef )*/ ) { return false; }
                    return true;
                }
            }
            /// <summary>Number of times it can possibly be activated (# times actually used may be less or same).</summary>
            public virtual float Activates { get { return !Validated ? 0f : ActivatesOverride; } }
            /// <summary>
            /// Number of times it can possibly be activated (# times actually used may
            /// be less or same). This one does not check for stance/weapon info, etc.
            /// </summary>
            public virtual float ActivatesOverride {
                get {
                    float LatentGCD = 1.5f + CalcOpts.GetLatency();
                    float GCDPerc = LatentGCD / ((Duration > Cd ? Duration : Cd) + CalcOpts.GetLatency());
                    float Every = LatentGCD / GCDPerc;
                    return (float)Math.Max(0f, FightDuration / Every * (1f - Whiteattacks.AvoidanceStreak));
                }
            }
            public virtual float Healing { get { return !Validated ? 0f : (float)Math.Max(0f, HealingBase * HealingBonus); } }
            public virtual float HealingOnUse {
                get {
                    float hp = Healing; // Base Healing
                    hp *= combatFactors.HealthBonus; // Global Healing Bonuses
                    //hp *= combatFactors.HealthReduction; // Global Healing Penalties
                    return (float)Math.Max(0f, hp);
                }
            }
            public virtual float AvgHealingOnUse { get { return HealingOnUse * Activates; } }
            public virtual float HPS { get { return AvgHealingOnUse / FightDuration; } }
            public virtual float Damage { get { return !Validated ? 0f : (float)Math.Max(0f, DamageBase * DamageBonus * AvgTargets); } }
            public virtual float DamageOverride { get { return (float)Math.Max(0f, DamageBase * DamageBonus * AvgTargets); } }
            public virtual float DamageOnUse {
                get {
                    float dmg = Damage; // Base Damage
                    dmg *= combatFactors.DamageBonus; // Global Damage Bonuses
                    dmg *= combatFactors.DamageReduction; // Global Damage Penalties

                    // Work the Attack Table
                    float dmgDrop = (1f
                        - MHAtkTable.Miss   // no damage when being missed
                        - MHAtkTable.Dodge  // no damage when being dodged
                        - MHAtkTable.Parry  // no damage when being parried
                        - MHAtkTable.Glance // glancing handled below
                        - MHAtkTable.Block  // blocked handled below
                        - MHAtkTable.Crit); // crits   handled below

                    float dmgGlance = dmg * MHAtkTable.Glance *     combatFactors.ReducWhGlancedDmg ;//Partial Damage when glancing, this doesn't actually do anything since glance is always 0
                    float dmgBlock  = dmg * MHAtkTable.Block  *     combatFactors.ReducYwBlockedDmg ;//Partial damage when blocked
                    float dmgCrit   = dmg * MHAtkTable.Crit   * (1f+combatFactors.BonusYellowCritDmg);//Bonus   Damage when critting

                    dmg *= dmgDrop;

                    dmg += dmgGlance + dmgBlock + dmgCrit;

                    return (float)Math.Max(0f, dmg);
                }
            }
            public virtual float AvgDamageOnUse { get { return DamageOnUse * Activates; } }
            public virtual float DPS { get { return AvgDamageOnUse / FightDuration; } }
            #endregion
            #region Functions
            protected void InitializeA() {
                Talents = Char.WarriorTalents;
                CalcOpts = Char.CalculationOptions as CalculationOptionsDPSWarr;
            }
            protected void InitializeB() {
                MHAtkTable = new AttackTable(Char, StatS, combatFactors, this, true );
                OHAtkTable = new AttackTable(Char, StatS, combatFactors, this, false);
            }
            public virtual float GetRageUsePerSecond(float acts) {
                if (!Validated) { return 0f; }
                return acts * RageCost / FightDuration;
            }
            public virtual float GetHealing() { if (!Validated) { return 0f; } return 0f; }
            public virtual float GetAvgDamageOnUse(float acts) {
                if (!Validated) { return 0f; }
                return DamageOnUse * acts;
            }
            public virtual float GetDPS(float acts) {
                if (!Validated) { return 0f; }
                float adou = GetAvgDamageOnUse(acts);
                return adou / FightDuration;
            }
            public virtual float GetAvgHealingOnUse(float acts) {
                if (!Validated) { return 0f; }
                return HealingOnUse * acts;
            }
            public virtual float GetHPS(float acts) {
                if (!Validated) { return 0f; }
                float adou = GetAvgHealingOnUse(acts);
                return adou / FightDuration;
            }
            public virtual float ContainCritValue(bool IsMH) {
                float BaseCrit = IsMH ? combatFactors._c_mhycrit : combatFactors._c_ohycrit;
                return (float)Math.Min(1f, Math.Max(0f, BaseCrit + BonusCritChance));
            }
            public virtual float GetXActs(AttackTableSelector i,float acts) {
                float retVal = 0f;
                switch (i) {
                    case AttackTableSelector.Missed:  { retVal = acts * MHAtkTable.Miss;  break; }
                    case AttackTableSelector.Dodged:  { retVal = acts * MHAtkTable.Dodge; break; }
                    case AttackTableSelector.Parried: { retVal = acts * MHAtkTable.Parry; break; }
                    case AttackTableSelector.Blocked: { retVal = acts * MHAtkTable.Block; break; }
                    case AttackTableSelector.Crit:    { retVal = acts * MHAtkTable.Crit;  break; }
                    case AttackTableSelector.Hit:     { retVal = acts * MHAtkTable.Hit;   break; }
                    default: { break; }
                }
                return retVal;
            }
            public virtual string GenTooltip(float acts, float ttldpsperc) {
                float misses = GetXActs(AttackTableSelector.Missed , acts), missesPerc = (acts == 0f ? 0f : misses/acts);
                float dodges = GetXActs(AttackTableSelector.Dodged , acts), dodgesPerc = (acts == 0f ? 0f : dodges/acts);
                float parrys = GetXActs(AttackTableSelector.Parried, acts), parrysPerc = (acts == 0f ? 0f : parrys/acts);
                float blocks = GetXActs(AttackTableSelector.Blocked, acts), blocksPerc = (acts == 0f ? 0f : blocks/acts);
                float crits  = GetXActs(AttackTableSelector.Crit   , acts), critsPerc  = (acts == 0f ? 0f : crits /acts);
                float hits   = GetXActs(AttackTableSelector.Hit    , acts), hitsPerc   = (acts == 0f ? 0f : hits  /acts);

                bool showmisss =                 misses > 0f;
                bool showdodge = CanBeDodged  && dodges > 0f;
                bool showparry = CanBeParried && parrys > 0f;
                bool showblock = CanBeBlocked && blocks > 0f;
                bool showcrits = CanCrit      && crits > 0f;
                
                string tooltip = "*" + Name +
                    Environment.NewLine +   "Cast Time: "   + (CastTime != -1 ? CastTime.ToString() : "Instant")
                                        + ", CD: "          + (Cd       != -1 ? Cd.ToString()       : "None"   )
                                        + ", RageCost: "    + (RageCost != -1 ? RageCost.ToString() : "None"   ) +
                Environment.NewLine + Environment.NewLine + acts.ToString("000.00") + " Activates over Attack Table:" +
                (showmisss ? Environment.NewLine + "- " + misses.ToString("000.00") + " : " + missesPerc.ToString("00.00%") + " : Missed "  : "") +
                (showdodge ? Environment.NewLine + "- " + dodges.ToString("000.00") + " : " + dodgesPerc.ToString("00.00%") + " : Dodged "  : "") +
                (showparry ? Environment.NewLine + "- " + parrys.ToString("000.00") + " : " + parrysPerc.ToString("00.00%") + " : Parried " : "") +
                (showblock ? Environment.NewLine + "- " + blocks.ToString("000.00") + " : " + blocksPerc.ToString("00.00%") + " : Blocked " : "") +
                (showcrits ? Environment.NewLine + "- " + crits.ToString( "000.00") + " : " + critsPerc.ToString( "00.00%") + " : Crit " : "") +
                             Environment.NewLine + "- " + hits.ToString(  "000.00") + " : " + hitsPerc.ToString(  "00.00%") + " : Hit " +
                    Environment.NewLine +
                    //Environment.NewLine + "Damage per Blocked|Hit|Crit: x|x|x" +
                    Environment.NewLine + "Targets Hit: " + (Targets != -1 ? AvgTargets.ToString("0.00") : "None") +
                    Environment.NewLine + "DPS: " + (GetDPS(acts) > 0 ? GetDPS(acts).ToString("0.00") : "None") +
                    Environment.NewLine + "Percentage of Total DPS: " + (ttldpsperc > 0 ? ttldpsperc.ToString("00.00%") : "None");

                return tooltip;
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
            public BloodThirst(Character c, Stats s,CombatFactors cf, WhiteAttacks wa) {
                Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; InitializeA();
                //
                Name = "Bloodthirst";
                AbilIterater = (int)CalculationOptionsDPSWarr.Maintenances.Bloodthirst_;
                ReqTalent = true;
                Talent2ChksValue = Talents.Bloodthirst;
                ReqMeleeWeap = true;
                ReqMeleeRange = true;
                //Targets += StatS.BonusTargets;
                Cd = 4f; // In Seconds
                //Duration = 8f;
                RageCost = 20f - (Talents.FocusedRage * 1f);
                StanceOkFury = true;
                DamageBase = StatS.AttackPower * 50f / 100f;
                DamageBonus = 1f + Talents.UnendingFury * 0.02f;
                BonusCritChance = StatS.BonusWarrior_T8_4P_MSBTCritIncrease;
                HealingBase = StatS.Health / 100.0f * 3f * (Talents.GlyphOfBloodthirst ? 2f : 1f);
                //HealingBonus = 1f;
                //
                InitializeB();
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
            public WhirlWind(Character c, Stats s,CombatFactors cf, WhiteAttacks wa) {
                Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; InitializeA();
                //
                Name = "WhirlWind";
                AbilIterater = (int)CalculationOptionsDPSWarr.Maintenances.Whirlwind_;
                ReqMeleeWeap = true;
                ReqMeleeRange = true;
                MaxRange = 8f; // In Yards
                Cd = 10f - (Talents.GlyphOfWhirlwind ? 2f : 0f); // In Seconds
                //Targets += StatS.BonusTargets;
                Targets += (CalcOpts.MultipleTargets ? 3f : 0f);
                RageCost = 25f - (Talents.FocusedRage * 1f);
                StanceOkFury = true;
                DamageBonus = (1f + Talents.ImprovedWhirlwind * 0.10f) * (1f + Talents.UnendingFury * 0.02f);
                //
                InitializeB();
            }
            // Variables
            // Get/Set
            // Functions
            // Whirlwind while dual wielding executes two separate attacks; assume no offhand in base case
            public override float Damage { get { return GetDamage(false, false) + GetDamage(false,true); } }
            public override float DamageOverride { get { return GetDamage(true, false) + GetDamage(true, true); } }
            /// <summary></summary>
            /// <param name="Override">When true, do not check for Bers Stance</param>
            /// <param name="isOffHand">When true, do calculations for off-hand damage instead of main-hand</param>
            /// <returns>Unmitigated damage of a single hit</returns>
            private float GetDamage(bool Override, bool isOffHand) {
                if (!Validated && !Override) { return 0f; }

                float Damage;
                if (isOffHand) {
                    if (this.Char.OffHand != null && this.Char.OffHand.Item != null) {
                        Damage = combatFactors.NormalizedOhWeaponDmg;
                    }else{ Damage = 0f; }
                }else{ Damage = combatFactors.NormalizedMhWeaponDmg; }

                return (float)Math.Max(0f, Damage * DamageBonus);
            }
            public override float DamageOnUse {
                get {
                    // ==== MAIN HAND ====
                    float DamageMH = GetDamage(false, false); // Base Damage
                    DamageMH *= combatFactors.DamageBonus; // Global Damage Bonuses
                    DamageMH *= combatFactors.DamageReduction; // Global Damage Penalties

                    // Work the Attack Table
                    float dmgDrop = (1f
                        - MHAtkTable.Miss   // no damage when being missed
                        - MHAtkTable.Dodge  // no damage when being dodged
                        - MHAtkTable.Parry  // no damage when being parried
                        - MHAtkTable.Glance // glancing handled below
                        - MHAtkTable.Block  // blocked handled below
                        - MHAtkTable.Crit); // crits   handled below

                    float dmgGlance = DamageMH * MHAtkTable.Glance * combatFactors.ReducWhGlancedDmg;//Partial Damage when glancing, this doesn't actually do anything since glance is always 0
                    float dmgBlock = DamageMH * MHAtkTable.Block * combatFactors.ReducYwBlockedDmg;//Partial damage when blocked
                    float dmgCrit = DamageMH * MHAtkTable.Crit * (1f + combatFactors.BonusYellowCritDmg);//Bonus   Damage when critting

                    DamageMH *= dmgDrop;

                    DamageMH += dmgGlance + dmgBlock + dmgCrit;

                    // ==== OFF HAND ====
                    float DamageOH = GetDamage(false, true); // Base Damage
                    DamageOH *= combatFactors.DamageBonus; // Global Damage Bonuses
                    DamageOH *= combatFactors.DamageReduction; // Global Damage Penalties

                    // Work the Attack Table
                    dmgDrop = (1f
                        - OHAtkTable.Miss   // no damage when being missed
                        - OHAtkTable.Dodge  // no damage when being dodged
                        - OHAtkTable.Parry  // no damage when being parried
                        - OHAtkTable.Glance // glancing handled below
                        - OHAtkTable.Block  // blocked handled below
                        - OHAtkTable.Crit); // crits   handled below

                    dmgGlance = DamageOH * OHAtkTable.Glance * combatFactors.ReducWhGlancedDmg;//Partial Damage when glancing, this doesn't actually do anything since glance is always 0
                    dmgBlock = DamageOH * OHAtkTable.Block * combatFactors.ReducYwBlockedDmg;//Partial damage when blocked
                    dmgCrit = DamageOH * OHAtkTable.Crit * (1f + combatFactors.BonusYellowCritDmg);//Bonus   Damage when critting

                    DamageOH *= dmgDrop;

                    DamageOH += dmgGlance + dmgBlock + dmgCrit;

                    // ==== RESULT ====
                    float Damage = DamageMH + DamageOH;
                    return (float)Math.Max(0f, Damage * AvgTargets);
                }
            }
            public float DamageOnUseOverride {
                get {
                    // ==== MAIN HAND ====
                    float DamageMH = GetDamage(true, false); // Base Damage
                    DamageMH *= combatFactors.DamageBonus; // Global Damage Bonuses
                    DamageMH *= combatFactors.DamageReduction; // Global Damage Penalties

                    // Work the Attack Table
                    float dmgDrop = (1f
                        - MHAtkTable.Miss   // no damage when being missed
                        - MHAtkTable.Dodge  // no damage when being dodged
                        - MHAtkTable.Parry  // no damage when being parried
                        - MHAtkTable.Glance // glancing handled below
                        - MHAtkTable.Block  // blocked handled below
                        - MHAtkTable.Crit); // crits   handled below

                    float dmgGlance = DamageMH * MHAtkTable.Glance * combatFactors.ReducWhGlancedDmg;//Partial Damage when glancing, this doesn't actually do anything since glance is always 0
                    float dmgBlock = DamageMH * MHAtkTable.Block * combatFactors.ReducYwBlockedDmg;//Partial damage when blocked
                    float dmgCrit = DamageMH * MHAtkTable.Crit * (1f + combatFactors.BonusYellowCritDmg);//Bonus   Damage when critting

                    DamageMH *= dmgDrop;

                    DamageMH += dmgGlance + dmgBlock + dmgCrit;

                    // ==== OFF HAND ====
                    float DamageOH = GetDamage(true, true); // Base Damage
                    DamageOH *= combatFactors.DamageBonus; // Global Damage Bonuses
                    DamageOH *= combatFactors.DamageReduction; // Global Damage Penalties

                    // Work the Attack Table
                    dmgDrop = (1f
                        - OHAtkTable.Miss   // no damage when being missed
                        - OHAtkTable.Dodge  // no damage when being dodged
                        - OHAtkTable.Parry  // no damage when being parried
                        - OHAtkTable.Glance // glancing handled below
                        - OHAtkTable.Block  // blocked handled below
                        - OHAtkTable.Crit); // crits   handled below

                    dmgGlance = DamageOH * OHAtkTable.Glance * combatFactors.ReducWhGlancedDmg;//Partial Damage when glancing, this doesn't actually do anything since glance is always 0
                    dmgBlock = DamageOH * OHAtkTable.Block * combatFactors.ReducYwBlockedDmg;//Partial damage when blocked
                    dmgCrit = DamageOH * OHAtkTable.Crit * (1f + combatFactors.BonusYellowCritDmg);//Bonus   Damage when critting

                    DamageOH *= dmgDrop;

                    DamageOH += dmgGlance + dmgBlock + dmgCrit;

                    // ==== RESULT ====
                    float Damage = DamageMH + DamageOH;
                    return (float)Math.Max(0f, Damage * AvgTargets);
                }
            }
        }
        public class BloodSurge : Ability {
            // Constructors
            /// <summary>
            /// Your Heroic Strike, Bloodthirst and Whirlwind hits have a (7%/13%/20%)
            /// chance of making your next Slam instant for 5 sec.
            /// </summary>
            /// <TalentsAffecting>Bloodsurge (Requires Talent) [(7%/13%/20%) chance]</TalentsAffecting>
            /// <GlyphsAffecting></GlyphsAffecting>
            public BloodSurge(Character c, Stats s,CombatFactors cf, WhiteAttacks wa, Slam sl, WhirlWind ww, BloodThirst bt) {
                Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; InitializeA();
                //
                Name = "Bloodsurge";
                AbilIterater = (int)CalculationOptionsDPSWarr.Maintenances.Bloodsurge_;
                ReqTalent = true;
                Talent2ChksValue = Talents.Bloodsurge;
                //Targets += StatS.BonusTargets;
                ReqMeleeWeap = true;
                ReqMeleeRange = true;
                Duration = 5f; // In Seconds
                RageCost = 15f - (Talents.FocusedRage * 1f);
                StanceOkFury = true;
                hsActivates = 0.0f;
                SL = sl;
                WW = ww;
                BT = bt;
                //
                InitializeB();
            }
            #region Variables
            public float hsActivates;
            public float maintainActs;
            public Slam SL;
            public WhirlWind WW;
            public BloodThirst BT;
            #endregion
            #region Functions
            private float BasicFuryRotation(float chanceMHhit, float chanceOHhit, float hsActivates, float procChance){
                // Assumes one slot to slam every 8 seconds: WW/BT/Slam/BT repeat. Not optimal, but easy to do
                float chanceWeDontProc = 1f;
                float actMod = 8f / FightDuration; // since we're assuming an 8sec rotation

                chanceWeDontProc *= (1f - actMod * hsActivates  * procChance * chanceMHhit);
                chanceWeDontProc *= (1f - actMod * WW.Activates * procChance * chanceMHhit)
                                 *  (1f - actMod * WW.Activates * procChance * chanceOHhit);
                chanceWeDontProc *= (1f - actMod * BT.Activates * procChance * chanceMHhit);
                return (1f - chanceWeDontProc) / actMod;
            }
            private float CalcSlamProcs(float chanceMHhit, float chanceOHhit, float hsActivates, float procChance) {
                float hsPercent = (hsActivates) / (FightDuration / Whiteattacks.MhEffectiveSpeed);
                float numProcs = 0.0f;
                int whiteTimer = 0;
                int WWtimer = 0;
                int BTtimer = 0;
                const int GCD = 15;
                float chanceWeDontProc = 1f; // temp value that keeps track of what the odds are we got a proc by SLAM time
                int numWW = 0;
                int numBT = 0;
                for (int timeStamp = 0; timeStamp < FightDuration * 10f; timeStamp++) {
                    if (whiteTimer <= 0) {
                        chanceWeDontProc *= (1f - hsPercent * procChance * chanceMHhit);
                        whiteTimer = (int)Math.Ceiling(Whiteattacks.MhEffectiveSpeed * 10);
                    }
                    if (timeStamp % GCD == 0) {
                        if (WWtimer <= 0) {
                            chanceWeDontProc *= (1f - procChance * chanceMHhit) * (1f - procChance*chanceOHhit);
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
            public override float ActivatesOverride {
                get {
                    float chance = Talents.Bloodsurge * 0.20f / 3f;
                    float chanceMhHitLands = (1f - MHAtkTable.Miss - MHAtkTable.Dodge);
                    float chanceOhHitLands = (1f - OHAtkTable.Miss - OHAtkTable.Dodge);

                    float procs3 = BasicFuryRotation(chanceMhHitLands, chanceOhHitLands, hsActivates, chance);

                    procs3 = (maintainActs > procs3) ? 0f : procs3 - maintainActs;

                    return procs3 * (1f - Whiteattacks.AvoidanceStreak);
                }
            }
            public override float Damage { get { return !Validated ? 0f : (float)Math.Max(0f, SL.DamageOverride); } }
            #endregion
        }
        // Arms Abilities
        public class MortalStrike : Ability {
            /// <summary>
            /// A vicious strike that deals weapon damage plus 380 and wounds the target, reducing
            /// the effectiveness of any healing by 50% for 10 sec.
            /// </summary>
            /// <TalentsAffecting>
            /// Mortal Strike (Requires Talent),
            /// Improved Mortal Strike [+(10-ROUNDUP(10/3*Pts))% damage, -(1/3*Pts) sec cooldown]
            /// </TalentsAffecting>
            /// <GlyphsAffecting>Glyph of Mortal Strike [+10% damage]</GlyphsAffecting>
            public MortalStrike(Character c, Stats s,CombatFactors cf, WhiteAttacks wa) {
                Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; InitializeA();
                //
                Name = "Mortal Strike";
                AbilIterater = (int)CalculationOptionsDPSWarr.Maintenances.MortalStrike_;
                ReqTalent = true;
                Talent2ChksValue = Talents.MortalStrike;
                ReqMeleeWeap = true;
                ReqMeleeRange = true;
                //Targets += StatS.BonusTargets;
                Cd = 6f - (Talents.ImprovedMortalStrike / 3f); // In Seconds
                RageCost = 30f - (Talents.FocusedRage * 1f);
                StanceOkFury = StanceOkArms = StanceOkDef = true;
                DamageBase = combatFactors.NormalizedMhWeaponDmg + 380f;
                DamageBonus = (1f + Talents.ImprovedMortalStrike / 3f * 0.1f)
                            * (1f + (Talents.GlyphOfMortalStrike ? 0.1f : 0f));
                BonusCritChance = StatS.BonusWarrior_T8_4P_MSBTCritIncrease;
                //
                InitializeB();
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
            public Suddendeath(Character c, Stats s,CombatFactors cf, WhiteAttacks wa, Swordspec ss) {
                Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; InitializeA();
                //
                Name = "Sudden Death";
                AbilIterater = (int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.SuddenDeath_;
                Exec = new Execute(c, s, cf, wa);
                SS = ss;
                RageCost = Exec.RageCost;
                ReqTalent = true;
                Talent2ChksValue = Talents.SuddenDeath;
                ReqMeleeWeap = Exec.ReqMeleeWeap;
                ReqMeleeRange = Exec.ReqMeleeRange;
                //Targets += StatS.BonusTargets;
                Cd = Exec.Cd;
                StanceOkArms = true;
                //
                InitializeB();
            }
            // Variables
            public Execute Exec;
            public Swordspec SS;
            public float FreeRage;
            // Functions
            public float GetActivates(float landedatksoverdur) {
                if (AbilIterater != -1 && !CalcOpts.Maintenance[AbilIterater]) { return 0f; }

                float rate = Talents.SuddenDeath * 0.03f;

                float acts = rate * landedatksoverdur;

                return (float)Math.Max(0f, acts * (1f - Whiteattacks.AvoidanceStreak));
            }
            public override float Damage {
                get {
                    if (!Validated) { return 0f; }
                    float Damage = Exec.DamageOverride;
                    return (float)Math.Max(0f, Damage);
                }
            }
        }
        public class OverPower : Ability {
            // Constructors
            /// <summary>
            /// Instantly overpower the enemy, causing weapon damage plus 125. Only usable after the target dodges.
            /// The Overpower cannot be blocked, dodged or parried.
            /// </summary>
            /// <TalentsAffecting>Improved Overpower [+(25*Pts)% Crit Chance],
            /// Unrelenting Assault [-(2*Pts) sec cooldown, +(10*Pts)% Damage.]</TalentsAffecting>
            /// <GlyphsAffecting>Glyph of Overpower [Can proc when parried]</GlyphsAffecting>
            public OverPower(Character c, Stats s, CombatFactors cf, WhiteAttacks wa, Swordspec ss) {
                Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; InitializeA();
                //
                Name = "Overpower";
                AbilIterater = (int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.Overpower_;
                SS = ss;
                ReqMeleeWeap = true;
                ReqMeleeRange = true;
                CanBeDodged = false;
                CanBeParried = false;
                CanBeBlocked = false;
                Cd = 5f - (2f * Talents.UnrelentingAssault); // In Seconds
                RageCost = 5f - (Talents.FocusedRage * 1f);
                //Targets += StatS.BonusTargets;
                StanceOkArms = true;
                DamageBase = combatFactors.NormalizedMhWeaponDmg;
                DamageBonus = 1f + (0.1f * Talents.UnrelentingAssault);
                BonusCritChance = 0.25f * Talents.ImprovedOverpower;
                //
                InitializeB();
            }
            public Swordspec SS;
            public float GetActivates(float YellowAttacksThatDodgeOverDur, float YellowAttacksThatParryOverDur, float ssActs) {
                if (AbilIterater != -1 && !CalcOpts.Maintenance[AbilIterater]) { return 0f; }

                float acts = 0f;
                float LatentGCD = (1.5f + CalcOpts.GetLatency());

                float dodge = SS.MHAtkTable.Dodge;
                float parry = (Talents.GlyphOfOverpower ? SS.MHAtkTable.Parry : 0f);

                // Chance to activate: Dodges + (if glyphed) Parries
                if (dodge + parry > 0f) {
                    float WhtHitsOverDur = FightDuration / Whiteattacks.MhEffectiveSpeed
             + (combatFactors.OH != null ? FightDuration / Whiteattacks.OhEffectiveSpeed : 0f)
                                           + ssActs;

                    float dodgesoverDur = 0f +
                          WhtHitsOverDur * (dodge + parry)
                        + dodge > 0 ? YellowAttacksThatDodgeOverDur : 0
                        + parry > 0 ? YellowAttacksThatParryOverDur : 0;

                    acts += (float)Math.Max(0f, dodgesoverDur * (1f - Whiteattacks.AvoidanceStreak));
                }

                return acts;
            }
        }
        public class TasteForBlood : Ability {
            // Constructors
            /// <summary>
            /// Instantly overpower the enemy, causing weapon damage plus 125. Only usable after the target takes Rend Damage.
            /// The Overpower cannot be blocked, dodged or parried.
            /// </summary>
            /// <TalentsAffecting>Improved Overpower [+(25*Pts)% Crit Chance],
            /// Unrelenting Assault [-(2*Pts) sec cooldown, +(10*Pts)% Damage.]</TalentsAffecting>
            /// <GlyphsAffecting></GlyphsAffecting>
            public TasteForBlood(Character c, Stats s,CombatFactors cf, WhiteAttacks wa) {
                Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; InitializeA();
                //
                Name = "Taste for Blood";
                AbilIterater = (int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.Overpower_;
                ReqMeleeWeap = true;
                ReqMeleeRange = true;
                CanBeDodged = false;
                CanBeParried = false;
                CanBeBlocked = false;
                Cd = 5f - (2f * Talents.UnrelentingAssault); // In Seconds
                RageCost = 5f - (Talents.FocusedRage * 1f);
                //Targets += StatS.BonusTargets; // should be handled in an OP subitem like Sudden Death does to Execute but we'll wait on that
                StanceOkArms = true;
                DamageBase = combatFactors.NormalizedMhWeaponDmg;
                DamageBonus = 1f + (0.1f * Talents.UnrelentingAssault);
                BonusCritChance = 0.25f * Talents.ImprovedOverpower;
                //
                InitializeB();
            }
            public override float ActivatesOverride {
                get {
                    float acts = 0f;
                    float LatentGCD = (1.5f + CalcOpts.GetLatency());
                    float cd = (float)Math.Max(Cd, LatentGCD);
                    float Every = 0f;
                    float GCDPerc = 0f;

                    // Chance to activate: Taste for Blood (Requires Rend)
                    if (Talents.TasteForBlood > 0f && CalcOpts.Maintenance[(int)CalculationOptionsDPSWarr.Maintenances.Rend_]) {
                        // Not more than once every 6 seconds
                        cd  = 6f;
                        // Percent chance to proc, if it's not at 3/3 then the cd grows
                        switch (Talents.TasteForBlood) {
                            case 1: { cd += 6f; break; }
                            case 2: { cd += 3f; break; }
                            case 3: { cd += 0f; break; }
                        }
                        
                        GCDPerc = LatentGCD / (cd + CalcOpts.GetLatency());
                        Every = LatentGCD / GCDPerc;
                        acts += (float)Math.Max(0f, FightDuration / Every * (1f - Whiteattacks.AvoidanceStreak));
                    }

                    return acts;
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
            public Bladestorm(Character c, Stats s,CombatFactors cf,WhiteAttacks wa,WhirlWind ww) {
                Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa;InitializeA();
                //
                WW = ww;
                Name = "Bladestorm";
                AbilIterater = (int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.Bladestorm_;
                ReqTalent = true;
                Talent2ChksValue = Talents.Bladestorm;
                ReqMeleeWeap = true;
                ReqMeleeRange = true;
                MaxRange = WW.MaxRange; // In Yards
                Targets = WW.Targets; // Handled in WW
                Cd = 90f - (Talents.GlyphOfBladestorm ? 15f : 0f); // In Seconds
                RageCost = 25f - (Talents.FocusedRage * 1f);
                CastTime = 6f; // In Seconds // Channeled
                StanceOkFury = StanceOkArms = StanceOkDef = true;
                //
                InitializeB();
            }
            // Variables
            public WhirlWind WW;
            // Functions
            public override float DamageOnUse {
                get {
                    if (!Validated) { return 0f; }
                    float Damage = WW.DamageOnUseOverride;
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
            public Swordspec(Character c, Stats s,CombatFactors cf, WhiteAttacks wa) {
                Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; InitializeA();
                //
                Name = "Sword Specialization";
                ReqTalent = true;
                Talent2ChksValue = Talents.SwordSpecialization;
                //Targets += StatS.BonusTargets;
                Cd = 6f; // In Seconds
                StanceOkFury = StanceOkArms = StanceOkDef = true;
                DamageBase = combatFactors.AvgMhWeaponDmgUnhasted;
                RageCost = Whiteattacks.MHSwingRage;
                //
                InitializeB();
            }
            // Functions
            public float GetActivates(float YellowsThatLandOverDur) {
                if (combatFactors._c_mhItemType != ItemType.TwoHandSword && combatFactors._c_mhItemType != ItemType.OneHandSword) { return 0.0f; }

                float rate = Talents.SwordSpecialization * (CalcOpts.PTRMode ? 0.02f : 0.01f);
                // This attack doesnt consume GCDs and doesn't affect the swing timer
                float rawActs = (YellowsThatLandOverDur + Whiteattacks.LandedAtksOverDur) * rate;

                // There is an internal cd of 6 seconds so AttacksThatCouldLandPerSec is capped by once every 6 sec.
                float capActs = base.ActivatesOverride;

                return (float)Math.Max(0f,Math.Min(rawActs,capActs));
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
            public Execute(Character c, Stats s,CombatFactors cf, WhiteAttacks wa) {
                Char = c; StatS = s; combatFactors = cf;Whiteattacks = wa; InitializeA();
                //
                Name = "Execute";
                //AbilIterater = (int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.Execute_;
                ReqMeleeWeap = true;
                ReqMeleeRange = true;
                //Targets += StatS.BonusTargets;
                RageCost = 15f - (Talents.ImprovedExecute * 2.5f) - (Talents.FocusedRage * 1f);
                StanceOkFury = StanceOkArms = true;
                //
                InitializeB();
            }
            public float FreeRage;
            public override float Activates { get { if (!Validated) { return 0f; } return 0f; } }
            public override float Damage { get { return GetDamage(false); } }
            public override float DamageOverride { get { return GetDamage(true); } }
            private float GetDamage(bool Override) {
                if (!Override && !Validated) { return 0f; }

                float freerage = (float)System.Math.Max(0f,FreeRage);
                if (Override && freerage <= (RageCost - (Talents.ImprovedExecute * 2.5f))) {
                    freerage = RageCost - (Talents.ImprovedExecute * 2.5f);
                }else if (freerage <= 0f) {
                    return 0.0f; // No Free Rage = 0 damage
                }
                float executeRage = freerage * FightDuration;
                executeRage = (float)Math.Min(30f, executeRage);
                executeRage += (Talents.GlyphOfExecution ? 10.00f : 0.00f);
                executeRage -= RageCost;

                float Damage = 1456f + StatS.AttackPower * 0.2f + executeRage * 38f;

                return (float)Math.Max(0f,Damage * AvgTargets);
            }
        }
        public class Slam : Ability {
            // Constructors
            /// <summary>Slams the opponent, causing weapon damage plus 250.</summary>
            /// <TalentsAffecting>Improved Slam [Reduces cast time of your Slam ability by (0.5/1) sec.]</TalentsAffecting>
            /// <SetsAffecting>T7 Deadnaught Battlegear 2 Pc [+10% Damage]</SetsAffecting>
            public Slam(Character c, Stats s,CombatFactors cf, WhiteAttacks wa) {
                Char = c; StatS = s; combatFactors = cf;Whiteattacks = wa; InitializeA();
                //
                Name = "Slam";
                AbilIterater = (int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.Slam_;
                ReqMeleeWeap = true;
                ReqMeleeRange = true;
                //Targets += StatS.BonusTargets;
                RageCost = 15f - (Talents.FocusedRage * 1f);
                CastTime = (1.5f - (Talents.ImprovedSlam * 0.5f)); // In Seconds
                StanceOkArms = StanceOkDef = true;
                DamageBase = combatFactors.AvgMhWeaponDmgUnhasted + 250f;
                DamageBonus = (1f + Talents.UnendingFury * 0.02f) * (1f + StatS.BonusWarrior_T7_2P_SlamDamage);
                BonusCritChance = StatS.BonusWarrior_T9_4P_SLHSCritIncrease;
                //
                InitializeB();
            }
            public override float Activates { get { if (!Validated) { return 0f; } return 0f; } }
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
            public virtual float FullRageCost { get { return RageCost + Whiteattacks.MHSwingRage; } }
            // Functions
            public override float Activates {
                get {
                    if (!Validated) { return 0f; }
                    float Hits = (float)Math.Max(0f, OverridesPerSec);
                    HSorCLVPerSecond = Hits;
                    return Hits * FightDuration * (1f - Whiteattacks.AvoidanceStreak);
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
                Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; InitializeA();
                //
                Name = "Heroic Strike";
                AbilIterater = (int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.HeroicStrike_;
                ReqMeleeWeap = true;
                ReqMeleeRange = true;
                Cd = /*0f*/(Char.MainHand != null ? Whiteattacks.MhEffectiveSpeed : 0f); // In Seconds
                //Targets += StatS.BonusTargets;
                RageCost = 15f - (Talents.ImprovedHeroicStrike * 1f) - (Talents.FocusedRage * 1f);
                CastTime = 0f; // In Seconds // Replaces a white hit
                StanceOkFury = StanceOkArms = StanceOkDef = true;
                bloodsurgeRPS = 0.0f;
                DamageBase = Whiteattacks.MhDamage + 495f;
                BonusCritChance = Talents.Incite * 0.05f + StatS.BonusWarrior_T9_4P_SLHSCritIncrease;
                //
                InitializeB();
            }
            // Variables
            // Get/Set
            // Functions
            public override float FullRageCost {
                get {
                    float swingrage = Whiteattacks.MHSwingRage;
                    float glyphback = (Talents.GlyphOfHeroicStrike ? 10.0f * ContainCritValue(true) : 0f);
                    return RageCost + swingrage - glyphback;
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
                Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; InitializeA();
                //
                Name = "Cleave";
                AbilIterater = (int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.Cleave_;
                ReqMeleeWeap = true;
                ReqMeleeRange = true;
                RageCost = 20f - (Talents.FocusedRage * 1f);
                Targets += (CalcOpts.MultipleTargets ? 1f + (Talents.GlyphOfCleaving ? 1f /* * (CalcOpts.MultipleTargetsPerc / 100f)*/ : 0f) : 0f);
                //Targets += StatS.BonusTargets;
                CastTime = 0f; // In Seconds // Replaces a white hit
                StanceOkFury = StanceOkArms = StanceOkDef = true;
                bloodsurgeRPS = 0.0f;
                DamageBase = Whiteattacks.MhDamage + 222f;
                DamageBonus = 1f + Talents.ImprovedCleave * 0.40f;
                BonusCritChance = Talents.Incite * 0.05f;
                //
                InitializeB();
            }
            // Variables
            // Get/Set
            // Functions
        }
        #endregion
        #region DoT Abilities
        public class DoT : Ability {
            // Constructors
            public DoT() {}
            // Variables
            private float TIMEBTWNTICKS; // In Seconds
            // Get/Set
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
                float result = GetDmgOverTickingTime(acts) / FightDuration;
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
            public Rend(Character c, Stats s, CombatFactors cf, WhiteAttacks wa) {
                Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; InitializeA();
                //
                Name = "Rend";
                AbilIterater = (int)CalculationOptionsDPSWarr.Maintenances.Rend_;
                ReqMeleeWeap = true;
                ReqMeleeRange = true;
                CanCrit = false;
                Duration = 15f + (Talents.GlyphOfRending ? 6f : 0f); // In Seconds
                TimeBtwnTicks = 3f; // In Seconds
                RageCost = 10f - (Talents.FocusedRage * 1f);
                StanceOkArms = StanceOkDef = true;
                DamageBase = 380f;
                DamageBonus = (1f + 0.10f * Talents.ImprovedRend);// *(1f + 0.15f * Talents.Trauma);
                //
                InitializeB();
            }
            public float addMisses;
            public float addDodges;
            public float addParrys;
            public override float ActivatesOverride {
                get {
                    // Since Rend has to be up in order to Taste for Blood
                    // this override has the intention of taking the baseline
                    // activates and forcing the char to use Rend again to ensure it's up
                    // in the event that the attemtped activate didn't land (it Missed, was dodged or parried)
                    // We're only doing the additional check once so it will at most Rend
                    // twice in a row, may consider doing a settler
                    float result = 0f;
                    float Base = base.ActivatesOverride;
                    addMisses = (MHAtkTable.Miss  > 0) ? Base * MHAtkTable.Miss  : 0f;
                    addDodges = (MHAtkTable.Dodge > 0) ? Base * MHAtkTable.Dodge : 0f;
                    addParrys = (MHAtkTable.Parry > 0) ? Base * MHAtkTable.Parry : 0f;

                    result = Base + addMisses + addDodges + addParrys;

                    return result;

                }
            }
            public override float TickSize {
                get {
                    if (!Validated) { return 0f; }

                    float DmgBonusBase = ((StatS.AttackPower * Whiteattacks.MhEffectiveSpeed) / 14f + (combatFactors.MH.MaxDamage + combatFactors.MH.MinDamage) / 2f) * (743f / 300000f);
                    float DmgBonusO75 = 0.25f * 1.35f * DmgBonusBase;
                    float DmgBonusU75 = 0.75f * 1.00f * DmgBonusBase;
                    float DmgMod = (1f + StatS.BonusBleedDamageMultiplier) * DamageBonus;

                    float TickSize = (DamageBase + DmgBonusO75 + DmgBonusU75) * DmgMod;
                    return TickSize;
                }
            }
            public override float GetDPS(float acts) {
                float dmgonuse = TickSize;
                float numticks = NumTicks * (acts - addMisses - addDodges - addParrys);
                float result = GetDmgOverTickingTime(acts) / FightDuration;
                return result;
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
            public DeepWounds(Character c, Stats s, CombatFactors cf, WhiteAttacks wa) {
                Char = c; StatS = s; combatFactors = cf;Whiteattacks = wa; InitializeA();
                //
                Name = "Deep Wounds";
                ReqTalent = true;
                Talent2ChksValue = Talents.DeepWounds;
                ReqMeleeWeap = true;
                ReqMeleeRange = true;
                CanCrit = false;
                Duration = 6f; // In Seconds
                TimeBtwnTicks = 1f; // In Seconds
                StanceOkFury = StanceOkArms = StanceOkDef  = true;
                mhActivates = ohActivates = 0f;
                //
                InitializeB();
            }
            private float mhActivates, ohActivates;
            public void SetAllAbilityActivates(float mh, float oh) { mhActivates = mh; ohActivates = oh; }
            public override float ActivatesOverride { get { return mhActivates + ohActivates; } }
            public override float TickSize {
                get {
                    if (!Validated) { return 0f; }

                    // Doing it this way because Deep Wounds triggering off of a MH crit
                    // and Deep Wounds triggering off of an OH crit do diff damage.
                    // Damage stores the average damage of single deep wounds trigger
                    float Damage = combatFactors.AvgMhWeaponDmgUnhasted * (0.16f * Talents.DeepWounds) * mhActivates / (mhActivates + ohActivates) +
                                   combatFactors.AvgOhWeaponDmgUnhasted * (0.16f * Talents.DeepWounds) * ohActivates / (mhActivates + ohActivates);

                    Damage *= (1f + StatS.BonusBleedDamageMultiplier);
                    Damage *= combatFactors.DamageBonus;

                    // Tick size
                    Damage /= Duration * TimeBtwnTicks;

                    // Because Deep Wounds is rolling, each tick is compounded by total number of times it's activated over its duration
                    Damage *= (mhActivates + ohActivates) * Duration / FightDuration;

                    // Ensure we're not doing negative damage
                    return Math.Max(0f, Damage);
                }
            }
            public override float GetDPS(float acts) { return TickSize; }
            public override float DPS { get { return TickSize; } }
        }
        #endregion
        #region BuffEffect Abilities
        public class BuffEffect : Ability {
            // Constructors
            public BuffEffect(){
                EFFECT  = null;
                EFFECT2 = null;
            }
            // Variables
            private SpecialEffect EFFECT;
            private SpecialEffect EFFECT2;
            // Get/Set
            public SpecialEffect Effect  { get { return EFFECT ; } set { EFFECT  = value; } }
            public SpecialEffect Effect2 { get { return EFFECT2; } set { EFFECT2 = value; } }
            // Functions
            public virtual Stats AverageStats {
                get {
                    if (!Validated) { return new Stats(); }
                    Stats bonus  = (Effect  == null) ? new Stats() { AttackPower = 0f, } : Effect.GetAverageStats( 0f, MHAtkTable.Hit + MHAtkTable.Crit, Whiteattacks.MhEffectiveSpeed, FightDuration);
                          bonus += (Effect2 == null) ? new Stats() { AttackPower = 0f, } : Effect2.GetAverageStats(0f, MHAtkTable.Hit + MHAtkTable.Crit, Whiteattacks.MhEffectiveSpeed, FightDuration);
                    return bonus;
                }
            }
        }
        /*public class Trauma : BuffEffect {
            // Constructors
            /// <summary>
            /// Your melee critical strikes increase the effectiveness of Bleed Effects on the
            /// target by (15*Pts)% for 15 sec.
            /// </summary>
            /// <TalentsAffecting>Trauma (Requires Talent)</TalentsAffecting>
            /// <GlyphsAffecting></GlyphsAffecting>
            public Trauma(Character c, Stats s, CombatFactors cf, WhiteAttacks wa) {
                Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; InitializeA();
                //
                Name = "Trauma";
                ReqMeleeWeap = false;
                ReqMeleeRange = true;
                ReqTalent = true;
                Talent2ChksValue = Talents.Trauma;
                Duration = 15f; // In Seconds
                StanceOkFury = StanceOkArms = StanceOkDef = true;
                Effect = new SpecialEffect(Trigger.MeleeCrit, StatS, Duration, 0f);//, MHAtkTable.Crit
                //
                InitializeB();
            }
            // Variables
            // Get/Set
            // Functions
            public override float ActivatesOverride {
                get {
                    // Chance to activate on every GCD
                    float LatentGCD = 1.5f + CalcOpts.GetLatency();
                    Cd = LatentGCD;
                    float GCDPerc = LatentGCD / (Cd + CalcOpts.GetLatency());
                    return (float)Math.Max(0f, FightDuration / LatentGCD * MHAtkTable.Crit);
                    // Jothay (a note to self): This is so very, very wrong... redo this you moron
                }
            }
        }*/
        public class BerserkerRage : BuffEffect {
            /// <summary>
            /// Instant, 30 sec Cd, Self, (Any)
            /// The warrior enters a berserker rage, becoming immune to Fear, Sap and Incapacitate effects
            /// and generating extra tage when taking damage. Lasts 10 sec.
            /// </summary>
            /// <TalentsAffecting>Improved Berserker Rage [+(10*Pts) Rage Generated]</TalentsAffecting>
            /// <GlyphsAffecting></GlyphsAffecting>
            public BerserkerRage(Character c, Stats s, CombatFactors cf, WhiteAttacks wa) {
                Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; InitializeA();
                //
                Name = "Berserker Rage";
                AbilIterater = (int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.BerserkerRage_;
                Cd = 30f * (1f - 1f / 9f * Talents.IntensifyRage); // In Seconds
                RageCost = 0f + (Talents.ImprovedBerserkerRage * 10f); // This is actually reversed in the rotation
                StanceOkArms = StanceOkDef = StanceOkFury = true;
                //
                InitializeB();
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
            public EnragedRegeneration(Character c, Stats s, CombatFactors cf, WhiteAttacks wa) {
                Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; InitializeA();
                //
                Name = "Enraged Regeneration";
                AbilIterater = (int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.EnragedRegeneration_;
                Cd = 3f * 60f; // In Seconds
                RageCost = 15f;
                StanceOkArms = StanceOkDef = StanceOkFury = true;
                HealingBase = StatS.Health * (0.30f + (Talents.GlyphOfEnragedRegeneration ? 0.10f : 0f));
                //
                InitializeB();
            }
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
                Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; InitializeA();
                //
                Name = "Bloodrage";
                AbilIterater = (int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.Bloodrage_;
                Cd = 60f * (1f - 1f / 9f * Talents.IntensifyRage); // In Seconds
                Duration = 10f; // In Seconds
                RageCost = 20f * (1f + Talents.ImprovedBloodrage * 0.25f); // This is actually reversed in the rotation
                StanceOkArms = StanceOkDef = StanceOkFury = true;
                Effect = new SpecialEffect(Trigger.Use,
                    new Stats() { BonusRageGen = 1f * (1f + Talents.ImprovedBloodrage * 0.25f), },
                    Duration, Cd);
                //
                InitializeB();
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
            public BattleShout(Character c, Stats s, CombatFactors cf, WhiteAttacks wa) {
                Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; InitializeA();
                //
                Name = "Battle Shout";
                AbilIterater = (int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.BattleShout_;
                MaxRange = 30f * (1f + Talents.BoomingVoice * 0.25f); // In Yards 
                Duration = (2f+(Talents.GlyphOfBattle?2f:0f))* 60f * (1f + Talents.BoomingVoice * 0.25f);
                Cd = Duration;
                RageCost = 10f;
                StanceOkFury = StanceOkArms = StanceOkDef = true;
                Effect = new SpecialEffect(Trigger.Use,
                    new Stats() { AttackPower = (548f*(1f+Talents.CommandingPresence*0.05f)), },
                    Duration, Cd);
                //
                InitializeB();
            }
            public override Stats AverageStats {
                get {
                    if (!Validated) { return new Stats(); }
                    Stats bonus = Effect.GetAverageStats(1f);
                    return bonus;
                }
            }
        }
        public class CommandingShout : BuffEffect {
            // Constructors
            /// <summary>
            /// The warrior shouts, increasing the maximum health of all raid and party members within 20 yards by 2255. Lasts 2 min.
            /// </summary>
            /// <TalentsAffecting>
            /// Booming Voice [+(25*Pts)% AoE and Duration],
            /// Commanding Presence [+(5*Pts)% to the Health Bonus]
            /// </TalentsAffecting>
            /// <GlyphsAffecting></GlyphsAffecting>
            public CommandingShout(Character c, Stats s, CombatFactors cf, WhiteAttacks wa) {
                Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; InitializeA();
                //
                Name = "Commanding Shout";
                AbilIterater = (int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.CommandingShout_;
                MaxRange = 30f * (1f + Talents.BoomingVoice * 0.25f); // In Yards 
                Duration = (2f+(Talents.GlyphOfCommand?2f:0f))* 60f * (1f + Talents.BoomingVoice * 0.25f);
                Cd = Duration;
                RageCost = 10f;
                StanceOkFury = StanceOkArms = StanceOkDef = true;
                Effect = new SpecialEffect(Trigger.Use,
                    new Stats() { Health = (2255f*(1f+Talents.CommandingPresence*0.05f)), },
                    Duration, Cd);
                //
                InitializeB();
            }
            public override Stats AverageStats {
                get {
                    if (!Validated) { return new Stats(); }
                    Stats bonus = Effect.GetAverageStats(1f);
                    return bonus;
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
                Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; InitializeA();
                //
                Name = "Death Wish";
                AbilIterater = (int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.DeathWish_;
                ReqTalent = true;
                Talent2ChksValue = Talents.DeathWish;
                Cd = 3f * 60f * (1f - 1f/9f * Talents.IntensifyRage); // In Seconds
                Duration = 30f;
                RageCost = 10f;
                StanceOkArms = StanceOkFury = true;
                Effect = new SpecialEffect(Trigger.Use,
                        new Stats() { BonusDamageMultiplier = 0.20f, DamageTakenMultiplier = 0.05f, },
                        Duration, Cd);
                //
                InitializeB();
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
                Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; InitializeA();
                //
                Name = "Recklessness";
                AbilIterater = (int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.Recklessness_;
                Cd = (5f * 60f - Talents.ImprovedDisciplines * 30f) * (1f - 1f/9f*Talents.IntensifyRage); // In Seconds
                Duration = 12f; // In Seconds
                StanceOkFury = true;
                Effect = new SpecialEffect(Trigger.Use,
                    new Stats { PhysicalCrit = 1f, DamageTakenMultiplier = 0.20f, },
                    Duration, Cd);
                InitializeB();
            }
            public override float Activates {
                get {
                    return !Validated ? 0f : ActivatesOverride;
                }
            }
            public override float ActivatesOverride {
                get {
                    float LatentGCD = 1.5f + CalcOpts.GetLatency();
                    float GCDPerc = LatentGCD / ((Duration > Cd ? Duration : Cd) + CalcOpts.GetLatency());
                    float Every = LatentGCD / GCDPerc * (1f - Whiteattacks.AvoidanceStreak);
                    return (float)Math.Max(0f, FightDuration / Every);
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
            public SweepingStrikes(Character c, Stats s, CombatFactors cf, WhiteAttacks wa) {
                Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; InitializeA();
                //
                Name = "Sweeping Strikes";
                AbilIterater = (int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.SweepingStrikes_;
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
                //
                InitializeB();
            }
            // reduce the numer of time activated by MultiTargsPerc
            public override float ActivatesOverride { get { return base.ActivatesOverride * (CalcOpts.MultipleTargetsPerc / 100f); } }
            public override Stats AverageStats {
                get {
                    if (!Validated) { return new Stats(); }
                    Stats bonus = Effect.GetAverageStats(
                        0f,
                        (1f * MHAtkTable.AnyLand) // The additional hit also has the attack table to deal with
                        * (CalcOpts.MultipleTargetsPerc / 100f),                 // And we need to reduce the number of activates by the % of
                                                                                 // time where there are multiple mobs in the fight
                        Whiteattacks.MhEffectiveSpeed,
                        CalcOpts.Duration);
                    return bonus;
                }
            }
        }
        public class SecondWind : BuffEffect {
            // Constructors
            /// <summary>
            /// Whenever you are struck by a Stun of Immoblize effect you will generate
            /// 10*Pts Rage and (5*Pts)% of your total health over 10 sec.
            /// </summary>
            /// <TalentsAffecting>Sweeping Strikes [Requires Talent]</TalentsAffecting>
            /// <GlyphsAffecting>Glyph of Sweeping Strikes [-100% Rage cost]</GlyphsAffecting>
            public SecondWind(Character c, Stats s, CombatFactors cf, WhiteAttacks wa) {
                Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; InitializeA();
                //
                Name = "Second Wind";
                AbilIterater = (int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.SweepingStrikes_;
                ReqTalent = true;
                Talent2ChksValue = Talents.SecondWind;
                ReqMeleeWeap = true;
                ReqMeleeRange = true;
                ReqMultiTargs = true;
                NumStunsOverDur = 1f;
                Duration = 10f; // Using 4 seconds to sim consume time
                Cd = 1.5f + CalcOpts.GetLatency();
                RageCost = 10f * Talents.SecondWind;
                StanceOkDef = StanceOkFury = StanceOkArms = true;
                //Effect = new SpecialEffect(Trigger.Use, new Stats() { BonusRageGen = 10f * Talents.SecondWind, }, Duration, Cd);
                HealingBase = StatS.Health * 0.05f * Talents.SecondWind;
                //
                InitializeB();
            }
            private float NUMSTUNSOVERDUR;
            public float NumStunsOverDur {
                get { return NUMSTUNSOVERDUR; }
                set {
                    NUMSTUNSOVERDUR = value;
                    Cd = (FightDuration / NUMSTUNSOVERDUR);
                    if (Effect != null) { Effect.Cooldown = Cd; }
                }
            }
        }
        #endregion
        #region DeBuff Abilities
        public class ThunderClap : BuffEffect {
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
                Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; InitializeA();
                //
                Name = "Thunder Clap";
                AbilIterater = (int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.ThunderClap_;
                ReqMeleeWeap = true;
                ReqMeleeRange = true;
                //Targets += StatS.BonusTargets;
                Targets += (CalcOpts.MultipleTargets ? (CalcOpts.MultipleTargetsMax-1f) : 0f);
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
                DamageBonus = 1f + Talents.ImprovedThunderClap * 0.10f;
                BonusCritChance = Talents.Incite * 0.05f;
                //
                InitializeB();
            }
        }
        public class SunderArmor : BuffEffect {
            // Constructors
            /// <summary></summary>
            /// <TalentsAffecting>Focused Rage [-(Pts) Rage Cost], Puncture [-(Pts) Rage Cost], </TalentsAffecting>
            /// <GlyphsAffecting>Glyph of Sunder Armor [+1 Targets]</GlyphsAffecting>
            public SunderArmor(Character c, Stats s, CombatFactors cf, WhiteAttacks wa) {
                Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; InitializeA();
                //
                Name = "Sunder Armor";
                AbilIterater = (int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.SunderArmor_;
                ReqMeleeWeap = true;
                ReqMeleeRange = true;
                Duration = 30f; // In Seconds
                Cd = 1.5f;
                CanCrit = false;
                RageCost = 15f - (Talents.FocusedRage * 1f) - (Talents.Puncture * 1f);
                Targets = 1f + (Talents.GlyphOfSunderArmor ? 1f : 0f);
                StanceOkFury = StanceOkArms = StanceOkDef = true;
                //
                InitializeB();
                //
                Effect = new SpecialEffect(Trigger.Use,
                    new Stats() { ArmorPenetration = 0.04f, },
                    Duration, Cd, MHAtkTable.Hit, 5);
            }
        }
        public class ShatteringThrow : BuffEffect {
            /// <summary>
            /// Throws your weapon at the enemy causing (12+AP*0.50) damage (based on attack power),
            /// reducing the armor on the target by 20% for 10 sec or removing any invulnerabilities.
            /// </summary>
            public ShatteringThrow(Character c, Stats s, CombatFactors cf, WhiteAttacks wa) {
                Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; InitializeA();
                //
                Name = "Shattering Throw";
                AbilIterater = (int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.ShatteringThrow_;
                ReqMeleeWeap = true;
                ReqMeleeRange = false;
                MaxRange = 30f; // In Yards
                //Targets += StatS.BonusTargets;
                Cd = 5f * 60f; // In Seconds
                Duration = 10f;
                CastTime = 1.5f; // In Seconds
                RageCost = 25f - (Talents.FocusedRage * 1f);
                StanceOkArms = true;
                DamageBase = 12f + StatS.AttackPower * 0.50f;
                //
                InitializeB();
                //
                Effect = new SpecialEffect(Trigger.Use,
                    new Stats() { ArmorPenetration = 0.20f, },
                    Duration, Cd,
                    MHAtkTable.Hit + MHAtkTable.Crit);
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
                Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; InitializeA();
                //
                Name = "Demoralizing Shout";
                AbilIterater = (int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.DemoralizingShout_;
                ReqMeleeWeap = false;
                ReqMeleeRange = false;
                MaxRange = 10f; // In Yards 
                Duration = 30f;
                RageCost = 10f - (Talents.FocusedRage * 1f);
                StanceOkArms = StanceOkFury = true;
                //
                InitializeB();
                //
                Effect = new SpecialEffect(Trigger.Use,
                    new Stats() { /*AttackPower = 411f,*/ }, // needs to be boss debuff
                    Duration, Duration,
                    MHAtkTable.Hit + MHAtkTable.Crit);
            }
        }
        public class Hamstring : BuffEffect {
            /// <summary>
            /// Instant, No cd, 10 Rage, Melee Range, Melee Weapon, (Battle/Zerker)
            /// Maims the enemy, reducing movement speed by 50% for 15 sec.
            /// </summary>
            /// <TalentsAffecting>Improved Hamstring [Gives a [5*Pts]% chance to immobilize the target for 5 sec.]</TalentsAffecting>
            /// <GlyphsAffecting>Glyph of Hamstring [Gives a 10% chance to immobilize the target for 5 sec.]</GlyphsAffecting>
            public Hamstring(Character c, Stats s, CombatFactors cf, WhiteAttacks wa) {
                Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; InitializeA();
                //
                Name = "Hamstring";
                AbilIterater = (int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.Hamstring_;
                ReqMeleeWeap = true;
                ReqMeleeRange = true;
                Duration = 15f; // In Seconds
                RageCost = 10f - (Talents.FocusedRage * 1f);
                //Targets += StatS.BonusTargets;
                StanceOkFury = StanceOkArms = true;
                Effect = new SpecialEffect(Trigger.Use,
                    new Stats() { AttackPower = 0f, /*TargetMoveSpeedReducPerc = 0.50f,*/ },
                    Duration, Duration);
                float Chance = Talents.ImprovedHamstring * 0.05f + (Talents.GlyphOfHamstring ? 0.10f : 0.00f);
                Effect2 = new SpecialEffect(Trigger.Use,
                    new Stats() { AttackPower = 0f, /*TargetStunned = 0.50f,*/ },
                    5f, Duration, Chance);
                //
                InitializeB();
            }
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
            public EveryManForHimself(Character c, Stats s, CombatFactors cf, WhiteAttacks wa) {
                Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; InitializeA();
                //
                Name = "Every Man for Himself";
                if(c.Race != CharacterRace.Human){return;}
                Cd = 2f * 60f;
                StanceOkArms = StanceOkFury = StanceOkDef = true;
                //
                InitializeB();
            }
        }
        public class HeroicFury : Ability {
            /// <summary>
            /// Instant, 45 sec Cooldown, 0 Rage, Self (Any)
            /// Removes any Immobilization effects and refreshes the cooldown of your Intercept ability.
            /// </summary>
            /// <TalentsAffecting></TalentsAffecting>
            /// <GlyphsAffecting></GlyphsAffecting>
            public HeroicFury(Character c, Stats s, CombatFactors cf, WhiteAttacks wa) {
                Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; InitializeA();
                //
                Name = "Heroic Fury";
                ReqTalent = true;
                Talent2ChksValue = Talents.HeroicFury;
                Cd = 45f;
                StanceOkArms = StanceOkFury = StanceOkDef = true;
                //
                InitializeB();
            }
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
            /// Improved Charge [+(5*Pts) RageGen]
            /// </TalentsAffecting>
            /// <GlyphsAffecting>
            /// Glyph of Rapid Charge [-20% Cd]
            /// Glyph of Charge [+5 yds MaxRange]
            /// </GlyphsAffecting>
            public Charge(Character c, Stats s, CombatFactors cf, WhiteAttacks wa) {
                Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; InitializeA();
                //
                MaxRange = 25f + (Talents.GlyphOfCharge ? 5f: 0f); // In Yards 
                Cd = (20f + Talents.Juggernaut * 5f) * (1f - (Talents.GlyphOfRapidCharge ? 0.20f : 0f)); // In Seconds
                Duration = 1.5f;
                RageCost = 25f + (Talents.ImprovedCharge * 5f);
                if (Talents.Warbringer == 1) {
                    StanceOkArms = StanceOkFury = StanceOkDef = true;
                } else if (Talents.Juggernaut == 1) {
                    StanceOkArms = true;
                }
                //
                InitializeB();
            }
        }
        public class Intercept : Ability {
            /// <summary>
            /// Instant, 30 sec Cd, 10 Rage, 8-25 yds, (Zerker)
            /// Charge an enemy, causing 380 damage (based on attack power) and stunning it for 3 sec.
            /// </summary>
            /// <TalentsAffecting>
            /// Warbringer [Usable in any stance]
            /// Improved Intercept [-[5*Pts] sec Cd]
            /// </TalentsAffecting>
            /// <GlyphsAffecting></GlyphsAffecting>
            public Intercept(Character c, Stats s, CombatFactors cf, WhiteAttacks wa) {
                Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; InitializeA();
                //
                MaxRange = 25f; // In Yards 
                Cd = 30f * (1f - (Talents.ImprovedIntercept * 5f)); // In Seconds
                Duration = 3f;
                RageCost = 10f - Talents.Precision * 1f;
                StanceOkFury = true;StanceOkArms = StanceOkDef = (Talents.Warbringer == 1);
                DamageBase = 380f;
                //
                InitializeB();
            }
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
            public Intervene(Character c, Stats s, CombatFactors cf, WhiteAttacks wa) {
                Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; InitializeA();
                //
                MaxRange = 25f; // In Yards 
                Cd = 30f * (1f - (Talents.ImprovedIntercept * 5f)); // In Seconds
                RageCost = 10f;
                StanceOkDef = true;StanceOkArms = StanceOkFury = (Talents.Warbringer == 1);
                //
                InitializeB();
            }
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
            public Retaliation(Character c, Stats s, CombatFactors cf, WhiteAttacks wa) {
                Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; InitializeA();
                //
                Name = "Retaliation";
                StanceOkArms = true;
                ReqMeleeRange = true;
                ReqMeleeWeap = true;
                //Targets += StatS.BonusTargets;
                Cd = 5f*60f - Talents.ImprovedDisciplines * 30f;
                Duration = 12f;
                StackCap = 20f;
                //
                InitializeB();
            }
            public float StackCap;
        }
        #endregion
    }
}
