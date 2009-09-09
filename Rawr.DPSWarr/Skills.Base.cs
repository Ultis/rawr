/**********
 * Owner: Shared
 **********/
using System;

namespace Rawr.DPSWarr {
    public partial class Skills {
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
                HSOverridesOverDur = 0f;
                CLOverridesOverDur = 0f;
                Slam_Freq = 0f;
            }
            #region Variables
            private readonly Character Char;
            private readonly Stats StatS;
            private readonly WarriorTalents Talents;
            private readonly CombatFactors combatFactors;
            private CalculationOptionsDPSWarr CalcOpts;
            public float TARGETS;
            public AttackTable MHAtkTable;
            public AttackTable OHAtkTable;
            private float OVDOVERDUR_HS;
            private float OVDOVERDUR_CL;
            public float Targets { get { return TARGETS; } set { TARGETS = value; } }
            public float AvgTargets {
                get {
                    if (CalcOpts.MultipleTargets) {
                        float extraTargetsHit = (float)Math.Min(CalcOpts.MultipleTargetsMax, TARGETS) - 1f;
                        return 1f + (extraTargetsHit + StatS.BonusTargets) * CalcOpts.MultipleTargetsPerc / 100f;
                    }
                    else { return 1f; }
                }
            }
            // Get/Set
            public float HSOverridesOverDur { get { return OVDOVERDUR_HS; } set { OVDOVERDUR_HS = value; } }
            public float CLOverridesOverDur { get { return OVDOVERDUR_CL; } set { OVDOVERDUR_CL = value; } }
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
                    return (float)Math.Max(0f, DamageBase * DamageBonus * AvgTargets);
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
            public float MhActivates {
                get {
                    if (MhEffectiveSpeed != 0)
                        return (float)Math.Max(0f, CalcOpts.Duration / MhEffectiveSpeed - HSOverridesOverDur - CLOverridesOverDur);
                    else return 0f;
                }
            }
            public float MhDPS { get { return AvgMhDamageOnUse / CalcOpts.Duration; } }
            // Off Hand
            public float OhEffectiveSpeed { get { return combatFactors.OHSpeed + SlamFreqSpdMod; } }
            public float OhDamage {
                get {
                    float DamageBase = combatFactors.AvgOhWeaponDmgUnhasted;
                    float DamageBonus = 1f + 0f;
                    return (float)Math.Max(0f, DamageBase * DamageBonus * AvgTargets);
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
            public float MHRageGenOverDur {
                get {
                    float result = 0f;
                    if (combatFactors.MH != null && combatFactors.MH.MaxDamage > 0) {
                        float ragePer = MHSwingRage;
                        result = MhActivates * ragePer;
                    }
                    return result;
                }
            }
            public float OHRageGenOverDur {
                get {
                    float result = 0f;
                    if (combatFactors.OH != null && combatFactors.OH.MaxDamage > 0) {
                        float ragePer = OHSwingRage;
                        result = OhActivates * ragePer;
                    }
                    return result;
                }
            }
            // Rage generated per second
            public float MHRageRatio {
                get {
                    float realMHRage = MHRageGenOverDur;
                    float realOverallRage = realMHRage + OHRageGenOverDur;
                    return realMHRage / realOverallRage;
                }
            }
            public float whiteRageGenOverDur { get { return MHRageGenOverDur + OHRageGenOverDur; } }
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
                    float whiteLands = (combatFactors.useOH ? OhActivates * OHAtkTable.Crit : 0f);
                    return whiteLands;
                }
            }
            // Other
            public float AvoidanceStreak {
                get {
                    bool useOH = combatFactors.useOH;

                    float mhRagePercent = MHRageRatio;
                    float ohRagePercent = 1f - mhRagePercent;
                    float missChance = mhRagePercent * MHAtkTable.AnyNotLand +
                              (useOH ? ohRagePercent * OHAtkTable.AnyNotLand : 0f);
                    float doubleChance = missChance * missChance;
                    float tripleChance = doubleChance * missChance;
                    float quadChance = doubleChance * doubleChance;

                    float doubleRecovery = 1f / (1f / MhEffectiveSpeed + (useOH ? 1f / OhEffectiveSpeed : 0f));
                    float tripleRecovery = 1f / (1f / (1.5f * MhEffectiveSpeed) + (useOH ? 1f / (1.5f * OhEffectiveSpeed) : 0f));
                    float quadRecovery = 1f / (1f / (2f * MhEffectiveSpeed) + (useOH ? 1f / (2f * OhEffectiveSpeed) : 0f));

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
        
        // Templated Base Classes
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
                        return 1f + (extraTargetsHit + StatS.BonusTargets) * CalcOpts.MultipleTargetsPerc / 100f;
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
            public virtual float RageUseOverDur { get { return (!Validated ? 0f : Activates * RageCost); } }
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
            public virtual float Healing { get { return !Validated ? 0f : HealingBase * HealingBonus; } }
            public virtual float HealingOnUse {
                get {
                    float hp = Healing; // Base Healing
                    hp *= combatFactors.HealthBonus; // Global Healing Bonuses
                    //hp *= combatFactors.HealthReduction; // Global Healing Penalties
                    return hp;
                }
            }
            public virtual float AvgHealingOnUse { get { return HealingOnUse * Activates; } }
            public virtual float HPS { get { return AvgHealingOnUse / FightDuration; } }
            public virtual float Damage { get { return !Validated ? 0f : DamageOverride; } }
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
            public virtual float DamageOnUseOverride
            {
                get
                {
                    return DamageOnUse;
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
            public virtual float GetRageUseOverDur(float acts) {
                if (!Validated) { return 0f; }
                return acts * RageCost;
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
        public class OnAttack : Ability
        {
            // Constructors
            public OnAttack() { OverridesOverDur = 0f; }
            // Variables
            private float OVERRIDESOVERDUR;
            // Get/Set
            public float OverridesOverDur { get { return OVERRIDESOVERDUR; } set { OVERRIDESOVERDUR = value; } }
            public virtual float FullRageCost { get { return RageCost + Whiteattacks.MHSwingRage; } }
            // Functions
            public override float Activates
            {
                get
                {
                    if (!Validated) { return 0f; }
                    float Acts = (float)Math.Max(0f, OverridesOverDur);
                    return Acts * (1f - Whiteattacks.AvoidanceStreak);
                }
            }
        };
        public class DoT : Ability
        {
            // Constructors
            public DoT() { }
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
            public override float GetDPS(float acts)
            {
                float dmgonuse = TickSize;
                float numticks = NumTicks * acts;
                float result = GetDmgOverTickingTime(acts) / FightDuration;
                return result;
            }
            public override float DPS { get { return TickSize / TickLength; } }
        }
        public class BuffEffect : Ability
        {
            // Constructors
            public BuffEffect()
            {
                EFFECT = null;
                EFFECT2 = null;
            }
            // Variables
            private SpecialEffect EFFECT;
            private SpecialEffect EFFECT2;
            // Get/Set
            public SpecialEffect Effect { get { return EFFECT; } set { EFFECT = value; } }
            public SpecialEffect Effect2 { get { return EFFECT2; } set { EFFECT2 = value; } }
            // Functions
            public virtual Stats AverageStats
            {
                get
                {
                    if (!Validated) { return new Stats(); }
                    Stats bonus = (Effect == null) ? new Stats() { AttackPower = 0f, } : Effect.GetAverageStats(0f, MHAtkTable.Hit + MHAtkTable.Crit, Whiteattacks.MhEffectiveSpeed, FightDuration);
                    bonus += (Effect2 == null) ? new Stats() { AttackPower = 0f, } : Effect2.GetAverageStats(0f, MHAtkTable.Hit + MHAtkTable.Crit, Whiteattacks.MhEffectiveSpeed, FightDuration);
                    return bonus;
                }
            }
        }
        #endregion

        #region Unused
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
        
    }
}
