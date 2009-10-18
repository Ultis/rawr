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
            public WhiteAttacks(Character character, Stats stats, CombatFactors cf, CalculationOptionsDPSWarr calcOpts) {
                Char = character;
                StatS = stats;
                Talents = Char.WarriorTalents == null ? new WarriorTalents() : Char.WarriorTalents;
                combatFactors = cf;
                CalcOpts = calcOpts;
                MHAtkTable = new AttackTable(Char, StatS, combatFactors, calcOpts, true, false, false);
                OHAtkTable = new AttackTable(Char, StatS, combatFactors, calcOpts, false, false, false);
                FightDuration = CalcOpts.Duration;
                //
                Targets = 1f;
                HSOverridesOverDur = 0f;
                CLOverridesOverDur = 0f;
                Slam_Freq = 0f;
            }
            #region Variables
            private readonly Character Char;
            private Stats StatS;
            private readonly WarriorTalents Talents;
            private readonly CombatFactors combatFactors;
            private CalculationOptionsDPSWarr CalcOpts;
            private float TARGETS;
            public AttackTable MHAtkTable;
            public AttackTable OHAtkTable;
            private float OVDOVERDUR_HS;
            private float OVDOVERDUR_CL;
            private float FightDuration;
            private float Targets { get { return TARGETS; } set { TARGETS = value; } }
            private float AvgTargets {
                get {
                    if (CalcOpts.MultipleTargets) {
                        //float extraTargetsHit = Math.Min(CalcOpts.MultipleTargetsMax, TARGETS) - 1f;
                        return 1f +
                            (Math.Min(CalcOpts.MultipleTargetsMax, TARGETS) - 1f) * 
                            CalcOpts.MultipleTargetsPerc / 100f + StatS.BonusTargets;
                    }
                    else { return 1f; }
                }
            }
            // Get/Set
            public void UpdateStats(Stats stats) { StatS = stats; }
            public float HSOverridesOverDur { get { return OVDOVERDUR_HS; } set { OVDOVERDUR_HS = value; } }
            public float CLOverridesOverDur { get { return OVDOVERDUR_CL; } set { OVDOVERDUR_CL = value; } }
            public float Slam_Freq;
            #endregion
            // bah
            private float SlamFreqSpdMod { get { return (Slam_Freq == 0f ? 0f : ((1.5f - (0.5f * Talents.ImprovedSlam)) * (Slam_Freq / FightDuration))); } }
            // Main Hand
            public float MhEffectiveSpeed { get { return combatFactors.MHSpeed + SlamFreqSpdMod; } }
            public float MhDamage {
                get {
                    //float DamageBase = combatFactors.AvgMhWeaponDmgUnhasted;
                    //float DamageBonus = 1f + 0f;
                    return combatFactors.AvgMhWeaponDmgUnhasted * AvgTargets;
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

                    return dmg;
                }
            }
            public float AvgMhDamageOnUse { get { return MhDamageOnUse * MhActivates; } }
            public float MhActivates {
                get {
                    if (MhEffectiveSpeed != 0)
                        return FightDuration / MhEffectiveSpeed - HSOverridesOverDur - CLOverridesOverDur;
                    else return 0f;
                }
            }
            public float MhActivatesNoHS {
                get {
                    if (MhEffectiveSpeed != 0)
                        return FightDuration / MhEffectiveSpeed;
                    else return 0f;
                }
            }
            public float MhDPS { get { return AvgMhDamageOnUse / FightDuration; } }
            // Off Hand
            public float OhEffectiveSpeed { get { return combatFactors.OHSpeed + SlamFreqSpdMod; } }
            public float OhDamage {
                get {
                    //float DamageBase = combatFactors.AvgOhWeaponDmgUnhasted;
                    //float DamageBonus = 1f + 0f;
                    return combatFactors.AvgOhWeaponDmgUnhasted * AvgTargets;
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

                    return dmg;
                }
            }
            public float AvgOhDamageOnUse { get { return OhDamageOnUse * OhActivates; } }
            public float OhActivates { get {
                if (OhEffectiveSpeed > 0f) return FightDuration / OhEffectiveSpeed; 
                else return 0; } }
            public float OhDPS { get { return AvgOhDamageOnUse / FightDuration; } }
            // Rage Calcs
            public float MHSwingRage {
                get {
                    // d = damage amount
                    // s = weapon speed
                    // f = hit factor
                    //float d, s, f, rage;
                    
                    //float rage = 0.0f;
                    float s = combatFactors._c_mhItemSpeed;
                    //s = combatFactors._c_mhItemSpeed;
                    float based = combatFactors.AvgMhWeaponDmg(s) * combatFactors.DamageBonus * combatFactors.DamageReduction;

                    // regular hit
                    //d = based;
                    //f = 3.5f;
                    return RageFormula(based, 
                        3.5f * s) * (MHAtkTable.Hit + MHAtkTable.Dodge + MHAtkTable.Parry) +

                    // glance
                    //d = based * combatFactors.ReducWhGlancedDmg;
                            RageFormula(based * combatFactors.ReducWhGlancedDmg, 
                        3.5f * s) * MHAtkTable.Glance +

                    // crit
                    //d = based * (1f + combatFactors.BonusWhiteCritDmg);
                    //f = 7.0f;
                    
                            RageFormula(based * (1f + combatFactors.BonusWhiteCritDmg),
                        7.0f * s) * MHAtkTable.Crit +

                            s * (3f * Talents.UnbridledWrath) / 60.0f * (1.0f - MHAtkTable.AnyLand);

                    //return rage;
                }
            }
            public float OHSwingRage {
                get {
                    // d = damage amount
                    // s = weapon speed
                    // f = hit factor
                    //float d, s, f, rage;
                    float based;

                    //float rage = 0.0f;
                    float s = combatFactors._c_ohItemSpeed;
                    based = combatFactors.AvgOhWeaponDmg(s) * combatFactors.DamageBonus * combatFactors.DamageReduction;

                    // regular hit
                    //d = based;
                    //f = 1.75f;
                    return RageFormula(based, 1.75f * s) * (OHAtkTable.Hit + OHAtkTable.Dodge + OHAtkTable.Parry) +

                    // glance
                    //d = based * combatFactors.ReducWhGlancedDmg;
                            RageFormula(based * combatFactors.ReducWhGlancedDmg, 
                        1.75f * s) * OHAtkTable.Glance +

                    // crit
                    //d = based * (1f + combatFactors.BonusWhiteCritDmg);
                    //f = 3.5f;
                            RageFormula(based * (1f + combatFactors.BonusWhiteCritDmg),
                        3.5f*s) * OHAtkTable.Crit +

                        s * (3f * Talents.UnbridledWrath) / 60.0f * (1.0f - OHAtkTable.AnyLand);
                    
                    //return rage;
                }
            }
            public float MHRageGenOverDur { get { return MhActivates * MHSwingRage; } }
            public float MHRageGenOverDurNoHS { get { return MhActivatesNoHS * MHSwingRage; } }
            public float OHRageGenOverDur {
                get {
                    if (combatFactors.useOH) {
                        return OhActivates * OHSwingRage;
                    }
                    return 0f;
                }
            }
            // Rage generated per second
            private float MHRageRatio {
                get {
                    return MHRageGenOverDur / (MHRageGenOverDur + OHRageGenOverDur);
                }
            }
            public float whiteRageGenOverDur { get { return MHRageGenOverDur + OHRageGenOverDur; } }
            public float whiteRageGenOverDurNoHS { get { return MHRageGenOverDurNoHS + OHRageGenOverDur; } }
            
            private const float c_const = 0.016545334215751158173395102581072f; //7.5f / 453.3f;
            private const float c_const2 = 0.033090668431502316346790205162144f; // 2*c_const
            private const float c_const3 = 0.049636002647253474520185307743216f; // 3*c_const
            private float RageFormula(float d, float fs) {
                /* R = Rage Generated
                 * d = damage amount
                 * c = rage conversion value
                 * s = weapon speed
                 * f = hit factor */
                //if (Char.Level != 80) c = 0.0091107836f * Char.Level * Char.Level + 3.225598133f * Char.Level + 4.2652911f; // = ~320.6;
                //float dmgRage = c_const * d;
                //float rps = f * s; // 3.5rage/sec baseline
                //float R = System.Math.Min((dmgRage + rps) / 2.0f, dmgRage*2.0f);
                
                //R = 3.75f * d / c + f * s / 2.0f;
                //R *= (1.0f + 0.25f * Talents.EndlessRage);
                return (4f + Talents.EndlessRage) / 4f * ((fs > c_const3 * d) ? (c_const2 * d) : (c_const * d + fs) / 2.0f);
                //return System.Math.Min( (c_const * d + fs) / 2.0f, c_const2 * d) * (1.0f + 0.25f * Talents.EndlessRage);
            }
            // Attacks Over Fight Duration
            public float LandedAtksOverDur {
                get {
                    return LandedAtksOverDurMH + LandedAtksOverDurOH;
                }
            }
            public float LandedAtksOverDurMH {
                get {
                    return MhActivates * MHAtkTable.AnyLand;
                }
            }
            public float LandedAtksOverDurOH {
                get {
                    return (combatFactors.useOH ? OhActivates * OHAtkTable.AnyLand : 0f);
                }
            }
            private float CriticalAtksOverDur {
                get {
                    return CriticalAtksOverDurMH + CriticalAtksOverDurOH;
                }
            }
            public float CriticalAtksOverDurMH {
                get {
                    return MhActivates * MHAtkTable.Crit;
                }
            }
            public float CriticalAtksOverDurOH {
                get {
                    return (combatFactors.useOH ? OhActivates * OHAtkTable.Crit : 0f);
                }
            }
            // Other
            public float RageSlip(float abilInterval, float rageCost)
            {
                //float whiteAtkInterval = (MhActivates + OhActivates) / FightDuration;
                //return MHAtkTable.AnyNotLand / abilInterval / whiteAtkInterval * rageCost / MHSwingRage;
                //float whiteMod = (MhActivates * MHSwingRage + (combatFactors.useOH ? OhActivates * OHSwingRage : 0f)) / FightDuration;
                if (!combatFactors.useOH && MhActivates <= 0f) { return 0f; }
                return (MHAtkTable.Miss * rageCost) / (abilInterval * ((MhActivates * MHSwingRage + (combatFactors.useOH ? OhActivates * OHSwingRage : 0f)) / FightDuration));
            }
            /*public float AvoidanceStreak {
                get {
                    bool useOH = combatFactors.useOH;
                    float mhRagePercent = MHRageRatio;
                    float ohRagePercent = 1f - mhRagePercent;
                    float missChance = mhRagePercent * MHAtkTable.Miss +
                              (useOH ? ohRagePercent * OHAtkTable.Miss : 0f);
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
            }*/
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
            public virtual string GenTooltip(float ttldpsMH, float ttldpsOH, float ttldps) {
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
                    Environment.NewLine + "DPS: " + (ttldpsMH > 0 ? ttldpsMH.ToString("0.00") : "None") +
                    Environment.NewLine + "Percentage of Total DPS: " + (ttldpsMH > 0 ? (ttldpsMH / ttldps).ToString("00.00%") : "None");

                if (combatFactors.useOH) {
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

                    tooltip += Environment.NewLine + Environment.NewLine + "White Damage (Off Hand)" +
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
                        Environment.NewLine + "DPS: " + (ttldpsOH > 0 ? ttldpsOH.ToString("0.00") : "None") +
                        Environment.NewLine + "Percentage of Total DPS: " + (ttldpsOH > 0 ? (ttldpsOH / ttldps).ToString("00.00%") : "None");
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
                UseReact = false;
                DamageBase = 0f;
                DamageBonus = 1f;
                HealingBase = 0f;
                HealingBonus = 1f;
                BonusCritChance = 0.00f;
                UseSpellHit = false;
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
            private float MINRANGE; // In Yards 
            private float MAXRANGE; // In Yards
            private float CD; // In Seconds
            private float DURATION; // In Seconds
            private float RAGECOST;
            private float CASTTIME; // In Seconds
            private bool STANCEOKARMS; // The ability can be used in Battle Stance
            private bool STANCEOKFURY; // The ability can be used in Berserker Stance
            private bool STANCEOKDEF;  // The ability can be used in Defensive Stance
            private bool USEREACT; // if this ability is used as a proc effect
            private Character CHARACTER;
            private WarriorTalents TALENTS;
            private Stats STATS;
            private CombatFactors COMBATFACTORS;
            private AttackTable MHATTACKTABLE;
            private AttackTable OHATTACKTABLE;
            private WhiteAttacks WHITEATTACKS;
            private CalculationOptionsDPSWarr CALCOPTS;
            private bool USESPELLHIT = false;
            private bool USEHITTABLE = true;
            public int AbilIterater;
            #endregion
            #region Get/Set
            public string Name { get { return NAME; } set { NAME = value; } }
            protected bool ReqTalent { get { return REQTALENT; } set { REQTALENT = value; } }
            protected int Talent2ChksValue { get { return TALENT2CHKSVALUE; } set { TALENT2CHKSVALUE = value; } }
            protected bool ReqMeleeWeap { get { return REQMELEEWEAP; } set { REQMELEEWEAP = value; } }
            protected bool ReqMeleeRange { get { return REQMELEERRANGE; } set { REQMELEERRANGE = value; } }
            protected bool ReqMultiTargs { get { return REQMULTITARGS; } set { REQMULTITARGS = value; } }
            private float _AvgTargets = -1f;
            public float AvgTargets {
                get {
                        //float extraTargetsHit = Math.Min(CalcOpts.MultipleTargetsMax, TARGETS) - 1f;
                    if (_AvgTargets == -1f) {
                        _AvgTargets = 1f +
                           (CalcOpts.MultipleTargets ?
                               StatS.BonusTargets +
                               CalcOpts.MultipleTargetsPerc / 100f *
                               (Math.Min(CalcOpts.MultipleTargetsMax, TARGETS) - 1f)
                               : 0f);
                    }
                    return _AvgTargets;
                }
            }
            protected float Targets { get { return TARGETS; } set { TARGETS = value; } }
            public bool CanBeDodged { get { return CANBEDODGED; } set { CANBEDODGED = value; } }
            public bool CanBeParried { get { return CANBEPARRIED; } set { CANBEPARRIED = value; } }
            public bool CanBeBlocked { get { return CANBEBLOCKED; } set { CANBEBLOCKED = value; } }
            public bool CanCrit { get { return CANCRIT; } set { CANCRIT = value; } }
            public float MinRange { get { return MINRANGE; } set { MINRANGE = value; } } // In Yards 
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
            protected float DamageBase { get { return DAMAGEBASE; } set { DAMAGEBASE = value; } }
            /// <summary>Percentage Based Damage Bonus (1.5 = 150% damage)</summary>
            protected float DamageBonus { get { return DAMAGEBONUS; } set { DAMAGEBONUS = value; } }
            protected float HealingBase { get { return HEALINGBASE; } set { HEALINGBASE = value; } }
            protected float HealingBonus { get { return HEALINGBONUS; } set { HEALINGBONUS = value; } }
            protected float BonusCritChance { get { return BONUSCRITCHANCE; } set { BONUSCRITCHANCE = value; } }
            protected bool StanceOkFury { get { return STANCEOKFURY; } set { STANCEOKFURY = value; } }
            protected bool StanceOkArms { get { return STANCEOKARMS; } set { STANCEOKARMS = value; } }
            protected bool StanceOkDef { get { return STANCEOKDEF; } set { STANCEOKDEF = value; } }
            protected bool UseReact { get { return USEREACT; } set { USEREACT = value; } }
            protected Character Char {
                get { return CHARACTER; }
                set {
                    CHARACTER = value;
                    if(CHARACTER != null){
                        Talents = CHARACTER.WarriorTalents;
                        //StatS = CalculationsDPSWarr.GetCharacterStats(CHARACTER, null);
                        //combatFactors = new CombatFactors(CHARACTER, StatS);
                        //Whiteattacks = Whiteattacks;
                        //CalcOpts = CHARACTER.CalculationOptions as CalculationOptionsDPSWarr;
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
            protected WarriorTalents Talents { get { return TALENTS; } set { TALENTS = value; } }
            protected Stats StatS { get { return STATS; } set { STATS = value; } }
            protected CombatFactors combatFactors { get { return COMBATFACTORS; } set { COMBATFACTORS = value; } }
            public AttackTable MHAtkTable { get { return MHATTACKTABLE; } set { MHATTACKTABLE = value; } }
            public AttackTable OHAtkTable { get { return OHATTACKTABLE; } set { OHATTACKTABLE = value; } }
            public WhiteAttacks Whiteattacks { get { return WHITEATTACKS; } set { WHITEATTACKS = value; } }
            protected CalculationOptionsDPSWarr CalcOpts { get { return CALCOPTS; } set { CALCOPTS = value; } }
            public virtual float RageUseOverDur { get { return (!Validated ? 0f : Activates * RageCost); } }
            protected float FightDuration { get { return CalcOpts.Duration; } }
            protected bool UseSpellHit { get { return USESPELLHIT; } set { USESPELLHIT = value; } }
            protected bool UseHitTable { get { return USEHITTABLE; } set { USEHITTABLE = value; } }
            
            private bool? validatedSet = null;
            public virtual bool Validated {
                get {
                    if (validatedSet != null) {
                        return (validatedSet == true);
                    }

                    /*if (Char == null || Char.MainHand == null || CalcOpts == null || Talents == null) {
                        validatedSet = false;
                    } else */if (AbilIterater != -1 && !CalcOpts.Maintenance[AbilIterater]) {
                        validatedSet = false;
                    } else if (ReqTalent && Talent2ChksValue < 1) {
                        validatedSet = false;
                    } else if (ReqMeleeWeap && Char.MainHand.MaxDamage <= 0) {
                        validatedSet = false;
                    } else if (ReqMultiTargs && (!CalcOpts.MultipleTargets || CalcOpts.MultipleTargetsPerc == 0)) {
                        validatedSet = false;
                    } else if ((CalcOpts.FuryStance && !StanceOkFury)
                          || (!CalcOpts.FuryStance && !StanceOkArms)) {
                        validatedSet = false;
                    } else validatedSet = true;
                    /*if (
                        // Null crap is bad
                       (Char == null || Char.MainHand == null || CalcOpts == null || Talents == null) ||
                    // Rotational Changes (Options Panel) (Arms Only right now)
                        (AbilIterater != -1 && !CalcOpts.Maintenance[AbilIterater]) ||
                    // Talent Requirements
                        (ReqTalent && Talent2ChksValue < 1) ||
                    // Need a weapon
                       (ReqMeleeWeap && Char.MainHand.MaxDamage <= 0) ||
                    // Need Multiple Targets or it's useless
                       (ReqMultiTargs && (!CalcOpts.MultipleTargets || CalcOpts.MultipleTargetsPerc == 0)) ||
                    // Proper Stance
                       ((CalcOpts.FuryStance && !StanceOkFury)
                        || (!CalcOpts.FuryStance && !StanceOkArms)
                      )) { return false; } */
                    
                    return (validatedSet == true);
                }
            }
            /// <summary>Number of times it can possibly be activated (# times actually used may be less or same).</summary>
            public virtual float Activates { get { return !Validated ? 0f : ActivatesOverride; } }
            /// <summary>
            /// Number of times it can possibly be activated (# times actually used may
            /// be less or same). This one does not check for stance/weapon info, etc.
            /// </summary>
            protected virtual float ActivatesOverride {
                get {
                    float LatentGCD = 1.5f + CalcOpts.Latency + (UseReact ? CalcOpts.AllowedReact : 0f);
                    float GCDPerc = LatentGCD / ((Duration > Cd ? Duration : Cd) + CalcOpts.Latency + (UseReact ? CalcOpts.AllowedReact : 0f));
                    //float Every = LatentGCD / GCDPerc;
                    if (RageCost > 0f)
                    {
                        /*float rageSlip = (float)Math.Pow(Whiteattacks.MHAtkTable.AnyNotLand, Whiteattacks.AvoidanceStreak * Every);
                        float rageSlip2 = Whiteattacks.MHAtkTable.AnyNotLand / Every / Whiteattacks.AvoidanceStreak * RageCost / Whiteattacks.MHSwingRage;
                        float ret = FightDuration / Every * (1f - rageSlip);
                        return ret;*/
                        return Math.Max(0f, FightDuration / (LatentGCD / GCDPerc) * (1f - Whiteattacks.RageSlip(LatentGCD / GCDPerc, RageCost)));
                    }
                    else return FightDuration / (LatentGCD / GCDPerc);
                    /*double test = Math.Pow((double)Whiteattacks.MHAtkTable.AnyNotLand, (double)Whiteattacks.AvoidanceStreak * Every);
                    return Math.Max(0f, FightDuration / Every * (1f - Whiteattacks.AvoidanceStreak));*/
                }
            }
            protected virtual float Healing { get { return !Validated ? 0f : HealingBase * HealingBonus; } }
            protected virtual float HealingOnUse {
                get {
                    return Healing * combatFactors.HealthBonus;
                }
            }
            protected virtual float AvgHealingOnUse { get { return HealingOnUse * Activates; } }
            protected virtual float HPS { get { return AvgHealingOnUse / FightDuration; } }
            protected virtual float Damage { get { return !Validated ? 0f : DamageOverride; } }
            public virtual float DamageOverride { get { return Math.Max(0f, DamageBase * DamageBonus * AvgTargets); } }
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

                    dmg += /*dmgGlance +*/ dmgBlock + dmgCrit;

                    return dmg;
                }
            }
            protected virtual float DamageOnUseOverride
            {
                get
                {
                    return DamageOnUse;
                }
            }
            protected virtual float AvgDamageOnUse { get { return DamageOnUse * Activates; } }
            public virtual float DPS { get { return AvgDamageOnUse / FightDuration; } }
            #endregion
            #region Functions
            protected void InitializeA(CalculationOptionsDPSWarr calcOpts) {
                Talents = Char.WarriorTalents;
                CalcOpts = calcOpts;
            }
            protected void InitializeB(CalculationOptionsDPSWarr co) {
                MHAtkTable = new AttackTable(Char, StatS, combatFactors, co, this, true,  UseSpellHit, !UseHitTable);
                OHAtkTable = new AttackTable(Char, StatS, combatFactors, co, this, false, UseSpellHit, !UseHitTable);
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
                //float adou = GetAvgDamageOnUse(acts);
                return GetAvgDamageOnUse(acts) / FightDuration;
            }
            public virtual float GetDPS(float acts, float perc) {
                if (!Validated) { return 0f; }
                //float adou = GetAvgDamageOnUse(acts);
                return GetAvgDamageOnUse(acts) / (FightDuration * perc);
            }
            public virtual float GetAvgHealingOnUse(float acts) {
                if (!Validated) { return 0f; }
                return HealingOnUse * acts;
            }
            public virtual float GetHPS(float acts) {
                if (!Validated) { return 0f; }
                //float adou = GetAvgHealingOnUse(acts);
                return GetAvgHealingOnUse(acts) / FightDuration;
            }
            public virtual float ContainCritValue(bool IsMH) {
                //float BaseCrit = IsMH ? combatFactors._c_mhycrit : combatFactors._c_ohycrit;
                return Math.Min(1f, (IsMH ? combatFactors._c_mhycrit : combatFactors._c_ohycrit) + BonusCritChance);
            }
            protected virtual float GetXActs(AttackTableSelector i,float acts) {
                float retVal = 0f;
                switch (i) {
                    case AttackTableSelector.Missed:  { retVal = acts * MHAtkTable.Miss;  break; }
                    case AttackTableSelector.Dodged:  { retVal = acts * MHAtkTable.Dodge; break; }
                    case AttackTableSelector.Parried: { retVal = acts * MHAtkTable.Parry; break; }
                    case AttackTableSelector.Blocked: { retVal = acts * MHAtkTable.Block; break; }
                    case AttackTableSelector.Glance : { retVal = acts * MHAtkTable.Glance;break; }
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
                    if (!Validated || OverridesOverDur <= 0f) { return 0f; }
                    //return Acts * (1f - Whiteattacks.AvoidanceStreak);
                    return OverridesOverDur * (1f - Whiteattacks.RageSlip(FightDuration / OverridesOverDur, RageCost));
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
                //float dmgonuse = TickSize;
                //float numticks = NumTicks * acts;
                return GetDmgOverTickingTime(acts) / FightDuration;
                //return result;
            }
            public override float DPS { get { return TickSize / TickLength; } }
        }
        public class BuffEffect : Ability {
            // Constructors
            public BuffEffect() {
                EFFECT = null;
                EFFECT2 = null;
            }
            // Variables
            private SpecialEffect EFFECT;
            private SpecialEffect EFFECT2;
            protected float addMisses;
            protected float addDodges;
            protected float addParrys;
            // Get/Set
            public SpecialEffect Effect { get { return EFFECT; } set { EFFECT = value; } }
            public SpecialEffect Effect2 { get { return EFFECT2; } set { EFFECT2 = value; } }
            // Functions
            public virtual Stats AverageStats {
                get {
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
