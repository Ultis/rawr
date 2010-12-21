/**********
 * Owner: Shared
 **********/
using System;

namespace Rawr.DPSWarr.Skills
{
    #region Base
    public enum AttackTableSelector { Missed = 0, Dodged, Parried, Blocked, Crit, Glance, Hit }
    // White Damage + White Rage Generated
    public class WhiteAttacks
    {
        // Constructors
        public WhiteAttacks(DPSWarrCharacter dpswarrchar/*, Character character, Stats stats, CombatFactors cf, CalculationOptionsDPSWarr calcOpts, BossOptions bossOpts*/)
        {
            DPSWarrChar = dpswarrchar;
            //Char = character;
            //StatS = stats;
            //Talents = Char.WarriorTalents == null ? new WarriorTalents() : Char.WarriorTalents;
            //DPSWarrChar.CombatFactors = cf;
            //CalcOpts = calcOpts;
            //BossOpts = bossOpts;
            MHAtkTable = new AttackTable(DPSWarrChar.Char, DPSWarrChar.StatS, DPSWarrChar.CombatFactors, DPSWarrChar.CalcOpts, DPSWarrChar.BossOpts, true, false, false, false);
            OHAtkTable = new AttackTable(DPSWarrChar.Char, DPSWarrChar.StatS, DPSWarrChar.CombatFactors, DPSWarrChar.CalcOpts, DPSWarrChar.BossOpts, false, false, false, false);
            //
            SlamActsOverDurO20 = SlamActsOverDurU20 = 0f;
        }
        public void InvalidateCache()
        {
            _MhDamageOnUse = _MHSwingRage = _OhDamageOnUse = _OHSwingRage = -1f;
        }
        #region Variables
        private DPSWarrCharacter DPSWarrChar;
        //private readonly Character Char;
        //private Stats StatS;
        //private readonly WarriorTalents Talents;
        //private readonly CombatFactors DPSWarrChar.CombatFactors;
        //private CalculationOptionsDPSWarr CalcOpts;
        //private BossOptions BossOpts;
        public AttackTable MHAtkTable { get; private set; }
        public AttackTable OHAtkTable { get; private set; }
        private float _fightDur = -1f, _fightDurO20 = -1f, _fightDurU20 = -1f;
        public float FightDuration { get { if (_fightDur == -1) { _fightDur = DPSWarrChar.BossOpts.BerserkTimer; } return _fightDur; } }
        public float FightDurationO20 { get { if (_fightDurO20 == -1) { _fightDurO20 = FightDuration * TimeOver20Perc; } return _fightDurO20; } }
        public float FightDurationU20 { get { if (_fightDurU20 == -1) { _fightDurU20 = FightDuration * TimeUndr20Perc; } return _fightDurU20; } }
        private float _timeOver20Perc = -1f, _timeUndr20Perc = -1f;
        public float TimeOver20Perc { get { if (_timeOver20Perc == -1f) { _timeOver20Perc = (DPSWarrChar.CalcOpts.M_ExecuteSpam ? 1f - (float)DPSWarrChar.BossOpts.Under20Perc : 1f); } return _timeOver20Perc; } }
        public float TimeUndr20Perc { get { if (_timeUndr20Perc == -1f) { _timeUndr20Perc = (DPSWarrChar.CalcOpts.M_ExecuteSpam ?      (float)DPSWarrChar.BossOpts.Under20Perc : 0f); } return _timeUndr20Perc; } }
        private float AvgTargets
        {
            get
            {
                /*if (BossOpts.MultiTargs) {
                    return 1f + (Math.Min((float)BossOpts.MaxNumTargets, 1f) - 1f) * (float)BossOpts.MultiTargsPerc + StatS.BonusTargets;
                }*/
                if (DPSWarrChar.BossOpts.MultiTargs && DPSWarrChar.BossOpts.Targets != null && DPSWarrChar.BossOpts.Targets.Count > 0)
                {
                    float value = 0;
                    foreach (TargetGroup tg in DPSWarrChar.BossOpts.Targets)
                    {
                        if (tg.Frequency <= 0 || tg.Chance <= 0) continue; // bad one, skip it
                        float upTime = (tg.Frequency / DPSWarrChar.BossOpts.BerserkTimer * (tg.Duration / 1000f) * tg.Chance) / DPSWarrChar.BossOpts.BerserkTimer;
                        value += (Math.Max(10, tg.NumTargs - (tg.NearBoss ? 0 : 1) + DPSWarrChar.StatS.BonusTargets)) * upTime;
                    }
                    return 1f + value;
                }
                else { return 1f; }
            }
        }
        // Get/Set
        public float SlamActsOverDurO20 = 0f;
        public float SlamActsOverDurU20 = 0f;
        public float SlamActsOverDur { get { return SlamActsOverDurO20 + SlamActsOverDurU20; } }
        #endregion
        // bah
        private float SlamFreqSpdModO20 { get { return (SlamActsOverDur == 0f ? 0f : (1.5f - (!DPSWarrChar.CombatFactors.FuryStance ? (DPSWarrChar.Talents.ImprovedSlam * 0.5f) : 0f)) * (SlamActsOverDurO20 / FightDurationO20)); } }
        private float SlamFreqSpdModU20 { get { return (SlamActsOverDur == 0f ? 0f : (1.5f - (!DPSWarrChar.CombatFactors.FuryStance ? (DPSWarrChar.Talents.ImprovedSlam * 0.5f) : 0f)) * (SlamActsOverDurU20 / FightDurationU20)); } }
        private float SlamFreqSpdMod { get { return (SlamActsOverDur == 0f ? 0f : (1.5f - (!DPSWarrChar.CombatFactors.FuryStance ? (DPSWarrChar.Talents.ImprovedSlam * 0.5f) : 0f)) * (SlamActsOverDur / FightDuration)); } }
        // Main Hand
        public float MHEffectiveSpeedO20 { get { return DPSWarrChar.CombatFactors.MHSpeedHasted + SlamFreqSpdModO20; } }
        public float MHEffectiveSpeedU20 { get { return DPSWarrChar.CombatFactors.MHSpeedHasted + (DPSWarrChar.CalcOpts.M_ExecuteSpam ? SlamFreqSpdModU20 : 0f); } }
        public float MHEffectiveSpeed { get { return DPSWarrChar.CombatFactors.MHSpeedHasted + SlamFreqSpdMod; } }
        public float MHDamage { get { return DPSWarrChar.CombatFactors.AvgMHWeaponDmgUnhasted * AvgTargets; } }
        private float _MhDamageOnUse = -1f;
        public float MHDamageOnUse
        {
            get
            {
                if (_MhDamageOnUse == -1f)
                {
                    float dmg = MHDamage;                  // Base Damage
                    dmg *= DPSWarrChar.CombatFactors.DamageBonus;      // Global Damage Bonuses
                    dmg *= DPSWarrChar.CombatFactors.WhiteDamageBonus; // Global White Damage Bonus
                    dmg *= DPSWarrChar.CombatFactors.DamageReduction;  // Global Damage Penalties

                    // Work the Attack Table
                    float dmgDrop = (1f
                        - MHAtkTable.Miss   // no damage when being missed
                        - MHAtkTable.Dodge  // no damage when being dodged
                        - MHAtkTable.Parry  // no damage when being parried
                        - MHAtkTable.Glance // glancing handled below
                        - MHAtkTable.Block  // blocked handled below
                        - MHAtkTable.Crit); // crits   handled below

                    float dmgGlance = dmg * MHAtkTable.Glance * CombatFactors.ReducWHGlancedDmg;//Partial Damage when glancing
                    float dmgBlock = dmg * MHAtkTable.Block * CombatFactors.ReducWHBlockedDmg;//Partial damage when blocked
                    float dmgCrit = dmg * MHAtkTable.Crit * (1f + DPSWarrChar.CombatFactors.BonusWhiteCritDmg);//Bonus Damage when critting

                    dmg *= dmgDrop;

                    dmg += dmgGlance + dmgBlock + dmgCrit;

                    _MhDamageOnUse = dmg;
                }
                return _MhDamageOnUse;
            }
        }
        public float AvgMHDamageOnUse { get { return MHDamageOnUse * MHActivates; } }
        public float MHActivatesO20 {
            get {
                if (MHEffectiveSpeed != 0) {
                    return FightDurationO20 / MHEffectiveSpeedO20;
                } else return 0f;
            }
        }
        public float MHActivatesU20 {
            get {
                if (MHEffectiveSpeed != 0) {
                    return FightDurationU20 / MHEffectiveSpeedU20;
                } else return 0f;
            }
        }
        public float MHActivates { get { return MHActivatesO20 + MHActivatesU20; } }
        public float MHdps { get { return AvgMHDamageOnUse / FightDuration; } }
        public float GetMHdps(float acts, float perc) { return (MHDamageOnUse * acts) / (FightDuration * perc); }
        // Off Hand
        public float OHEffectiveSpeedO20 { get { return DPSWarrChar.CombatFactors.OHSpeedHasted + SlamFreqSpdModO20; } }
        public float OHEffectiveSpeedU20 { get { return DPSWarrChar.CombatFactors.OHSpeedHasted + (DPSWarrChar.CalcOpts.M_ExecuteSpam ? SlamFreqSpdModU20 : 0f); } }
        public float OHEffectiveSpeed { get { return DPSWarrChar.CombatFactors.OHSpeedHasted > 0f ? DPSWarrChar.CombatFactors.OHSpeedHasted + SlamFreqSpdMod : 0; } }
        public float OHDamage
        {
            get
            {
                //float DamageBase = DPSWarrChar.CombatFactors.AvgOhWeaponDmgUnhasted;
                //float DamageBonus = 1f + 0f;
                return DPSWarrChar.CombatFactors.AvgOHWeaponDmgUnhasted * AvgTargets;
            }
        }
        private float _OhDamageOnUse = -1f;
        public float OHDamageOnUse
        {
            get
            {
                if (_OhDamageOnUse == -1f)
                {
                    float dmg = OHDamage;                              // Base Damage
                    dmg *= DPSWarrChar.CombatFactors.DamageBonus;      // Global Damage Bonuses
                    dmg *= DPSWarrChar.CombatFactors.WhiteDamageBonus; // Global White Damage Bonus
                    dmg *= DPSWarrChar.CombatFactors.DamageReduction;  // Global Damage Penalties

                    // Work the Attack Table
                    float dmgDrop = (1f
                        - OHAtkTable.Miss   // no damage when being missed
                        - OHAtkTable.Dodge  // no damage when being dodged
                        - OHAtkTable.Parry  // no damage when being parried
                        - OHAtkTable.Glance // glancing handled below
                        - OHAtkTable.Block  // blocked handled below
                        - OHAtkTable.Crit); // crits handled below

                    float dmgGlance = dmg * OHAtkTable.Glance * CombatFactors.ReducWHGlancedDmg;//Partial Damage when glancing
                    float dmgBlock = dmg * OHAtkTable.Block * CombatFactors.ReducWHBlockedDmg;//Partial damage when blocked
                    float dmgCrit = dmg * OHAtkTable.Crit * (1f + DPSWarrChar.CombatFactors.BonusWhiteCritDmg);//Bonus   Damage when critting

                    dmg *= dmgDrop;

                    dmg += dmgGlance + dmgBlock + dmgCrit;

                    _OhDamageOnUse = dmg;
                }
                return _OhDamageOnUse;
            }
        }
        public float AvgOHDamageOnUse { get { return OHDamageOnUse * OHActivates; } }
        public float OHActivatesO20 {
            get {
                if (OHEffectiveSpeed != 0) {
                    return FightDurationO20 / OHEffectiveSpeedO20;
                } else return 0f;
            }
        }
        public float OHActivatesU20 {
            get {
                if (OHEffectiveSpeed != 0) {
                    return FightDurationU20 / OHEffectiveSpeedU20;
                } else return 0f;
            }
        }
        public float OHActivates { get { return OHActivatesO20 + OHActivatesU20; } }
        public float Ohdps { get { return AvgOHDamageOnUse / FightDuration; } }
        public float GetOhdps(float acts, float perc) { return (OHDamageOnUse * acts) / (FightDuration * perc); }
        // Rage Calcs
        private const float RAGEPERSWING = 15f; // 15 Per Swing (Estimated)
        private float getragefromspeedMH { get { return DPSWarrChar.CombatFactors.MH.Speed * RAGEFROMSPEED * DPSWarrChar.CombatFactors.TotalHaste; } } // trying something new here, 6.5 rage per swing based
        private float getragefromspeedOH { get { return DPSWarrChar.CombatFactors.OH != null ? DPSWarrChar.CombatFactors.OH.Speed * RAGEFROMSPEED * DPSWarrChar.CombatFactors.TotalHaste : 0f; } } // on swing speed and haste then INCREASES that
        private const float RAGEFROMSPEED = 6.5f;//0.12656043f; // approx 1/8
        private const float RAGECRITMOD = 1.00f; // 2x Rage
        private const float RAGEOHMOD = 0.50f; // 50% Rage
        private const float RAGEANGERMNGTMOD = 1.25f; // +25% Rage gen for Arms spec
        private float _MHSwingRage = -1f;
        public float MHSwingRage
        {
            get
            {
                if (_MHSwingRage == -1f)
                {
                    float baserage = getragefromspeedMH; // Base Rage Per Swing

                    // Work the Attack Table
                    float rageDrop = (1f - MHAtkTable.Miss); // no rage when missing

                    baserage *= rageDrop;

                    _MHSwingRage = baserage * (!DPSWarrChar.CombatFactors.FuryStance ? RAGEANGERMNGTMOD: 1f);
                }
                return _MHSwingRage;
            }
        }
        private float _OHSwingRage = -1f;
        public float OHSwingRage
        {
            get
            {
                if (_OHSwingRage == -1f)
                {
                    float baserage = getragefromspeedOH; // Base Rage Per Swing

                    // Work the Attack Table
                    float rageDrop = (1f - OHAtkTable.Miss);// no rage when missing

                    baserage *= rageDrop;

                    _OHSwingRage = baserage * (!DPSWarrChar.CombatFactors.FuryStance ? RAGEANGERMNGTMOD : 1f) * RAGEOHMOD;
                }
                return _OHSwingRage;
            }
        }
        public float MHRageGenOverDurO20 { get { return MHActivatesO20 * MHSwingRage; } }
        public float MHRageGenOverDurU20 { get { return MHActivatesU20 * MHSwingRage; } }
        public float MHRageGenOverDur { get { return MHActivates * MHSwingRage; } }
        public float OHRageGenOverDurO20 { get { return OHActivatesO20 * OHSwingRage; } }
        public float OHRageGenOverDurU20 { get { return OHActivatesU20 * OHSwingRage; } }
        public float OHRageGenOverDur { get { return (DPSWarrChar.CombatFactors.useOH) ? OHActivates * OHSwingRage : 0f; } }
        // Rage generated per second
        //private float MHRageRatio { get { return MHRageGenOverDur / (MHRageGenOverDur + OHRageGenOverDur); } }
        public float WhiteRageGenOverDurO20 { get { return MHRageGenOverDurO20 + OHRageGenOverDurO20; } }
        public float WhiteRageGenOverDurU20 { get { return MHRageGenOverDurU20 + OHRageGenOverDurU20; } }
        public float WhiteRageGenOverDur { get { return WhiteRageGenOverDurO20 + WhiteRageGenOverDurU20; } }

        // Attacks Over Fight Duration
        public float LandedAtksOverDur { get { return LandedAtksOverDurMH + LandedAtksOverDurOH; } }
        public float LandedAtksOverDurMH { get { return MHActivates * MHAtkTable.AnyLand; } }
        public float LandedAtksOverDurOH { get { return (DPSWarrChar.CombatFactors.useOH ? OHActivates * OHAtkTable.AnyLand : 0f); } }
        //private float CriticalAtksOverDur { get { return CriticalAtksOverDurMH + CriticalAtksOverDurOH; } }
        public float CriticalAtksOverDurMH { get { return MHActivates * MHAtkTable.Crit; } }
        public float CriticalAtksOverDurOH { get { return (DPSWarrChar.CombatFactors.useOH ? OHActivates * OHAtkTable.Crit : 0f); } }
        
        // Other
        public float RageSlip(float abilInterval=0, float rageCost=0, float timeMod=1f) {
            if (!DPSWarrChar.CombatFactors.useOH && MHActivates <= 0f) { return 0f; }
            return (MHAtkTable.AnyNotLand * rageCost) / (abilInterval * ((MHActivates * MHSwingRage + (DPSWarrChar.CombatFactors.useOH ? OHActivates * OHSwingRage : 0f)) / (FightDuration*timeMod)));
        }
        public virtual float GetXActs(AttackTableSelector i, float acts, bool isMH) {
            AttackTable table = (isMH ? MHAtkTable : OHAtkTable);
            float retVal = 0f;
            switch (i) {
                case AttackTableSelector.Missed: { retVal = acts * table.Miss; break; }
                case AttackTableSelector.Dodged: { retVal = acts * table.Dodge; break; }
                case AttackTableSelector.Parried: { retVal = acts * table.Parry; break; }
                case AttackTableSelector.Blocked: { retVal = acts * table.Block; break; }
                case AttackTableSelector.Crit: { retVal = acts * table.Crit; break; }
                case AttackTableSelector.Glance: { retVal = acts * table.Glance; break; }
                case AttackTableSelector.Hit: { retVal = acts * table.Hit; break; }
                default: { break; }
            }
            return retVal;
        }
        public virtual string GenTooltip(float ttldpsMH, float ttldpsOH, float ttldps, float ttldpsMH_U20, float ttldpsOH_U20)
        {
            // ==== MAIN HAND ====
            float acts = MHActivates;
            float misses = GetXActs(AttackTableSelector.Missed, acts, true), missesPerc = (acts == 0f ? 0f : misses / acts);
            float dodges = GetXActs(AttackTableSelector.Dodged, acts, true), dodgesPerc = (acts == 0f ? 0f : dodges / acts);
            float parrys = GetXActs(AttackTableSelector.Parried, acts, true), parrysPerc = (acts == 0f ? 0f : parrys / acts);
            float blocks = GetXActs(AttackTableSelector.Blocked, acts, true), blocksPerc = (acts == 0f ? 0f : blocks / acts);
            float crits = GetXActs(AttackTableSelector.Crit, acts, true), critsPerc = (acts == 0f ? 0f : crits / acts);
            float glncs = GetXActs(AttackTableSelector.Glance, acts, true), glncsPerc = (acts == 0f ? 0f : glncs / acts);
            float hits = GetXActs(AttackTableSelector.Hit, acts, true), hitsPerc = (acts == 0f ? 0f : hits / acts);

            bool showmisss = misses > 0f;
            bool showdodge = dodges > 0f;
            bool showparry = parrys > 0f;
            bool showblock = blocks > 0f;
            bool showglncs = glncs > 0f;
            bool showcrits = crits > 0f;

            string tooltip = string.Format(@"*{0}
Cast Time: {1}, CD: {2}, Rage Generated: {3}

{4:000.00} Activates over Attack Table:{5}{6}{7}{8}{9}{10}{11}

Targets Hit: {12:0.00}
DPS: {13:0.00}|{14:0.00}
Percentage of Total DPS: {15:00.00%}",
            "White Damage (Main Hand)",
            "Instant",
            (MHEffectiveSpeed != -1 ? string.Format("{0:0.00}", MHEffectiveSpeed) : "None"),
            (MHSwingRage != -1 ? string.Format("{0:0.00}", (-1f * MHSwingRage)) : "None"),
            acts,
            showmisss ? string.Format("\r\n- {0:000.00} : {1:00.00%} : Missed", misses, missesPerc) : "",
            showdodge ? string.Format("\r\n- {0:000.00} : {1:00.00%} : Dodged", dodges, dodgesPerc) : "",
            showparry ? string.Format("\r\n- {0:000.00} : {1:00.00%} : Parried", parrys, parrysPerc) : "",
            showblock ? string.Format("\r\n- {0:000.00} : {1:00.00%} : Blocked", blocks, blocksPerc) : "",
            showglncs ? string.Format("\r\n- {0:000.00} : {1:00.00%} : Glanced", glncs, glncsPerc) : "",
            showcrits ? string.Format("\r\n- {0:000.00} : {1:00.00%} : Crit", crits, critsPerc) : "",
                        string.Format("\r\n- {0:000.00} : {1:00.00%} : Hit", hits, hitsPerc),
            (AvgTargets != -1 ? AvgTargets : 0),
            (ttldpsMH > 0 ? ttldpsMH : 0),
            (ttldpsMH_U20 > 0 ? ttldpsMH_U20 : 0),
            (ttldpsMH > 0 ? ttldpsMH / ttldps : 0));

            //return tt;


            /*string tooltip = "*" + "White Damage (Main Hand)" +
                Environment.NewLine + "Cast Time: Instant"
                                    + ", CD: " + (MHEffectiveSpeed != -1 ? MHEffectiveSpeed.ToString("0.00") : "None")
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
                Environment.NewLine + "Targets Hit: " + AvgTargets.ToString("0.00") +
                Environment.NewLine + "DPS: " + (ttldpsMH > 0 ? ttldpsMH.ToString("0.00") : "None") + (ttldpsMH_U20 != -1 ? "|" + (ttldpsMH_U20 > 0 ? ttldpsMH_U20.ToString("0.00") : "None") : "") +
                Environment.NewLine + "Percentage of Total DPS: " + (ttldpsMH > 0 ? (ttldpsMH / ttldps).ToString("00.00%") : "None");*/

            if (DPSWarrChar.CombatFactors.useOH)
            {
                // ==== OFF HAND ====
                acts = OHActivates;
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
                showcrits = crits > 0f;

                tooltip += string.Format(@"*{0}
Cast Time: {1}, CD: {2}, Rage Generated: {3}

{4:000.00} Activates over Attack Table:{5}{6}{7}{8}{9}{10}{11}

Targets Hit: {12:0.00}
DPS: {13:0.00}|{14:0.00}
Percentage of Total DPS: {15:00.00%}",
            "White Damage (Off Hand)",
            "Instant",
            (OHEffectiveSpeed != -1 ? string.Format("{0:0.00}", OHEffectiveSpeed) : "None"),
            (OHSwingRage != -1 ? string.Format("{0:0.00}", (-1f * OHSwingRage)) : "None"),
            acts,
            showmisss ? string.Format("\r\n- {0:000.00} : {1:00.00%} : Missed", misses, missesPerc) : "",
            showdodge ? string.Format("\r\n- {0:000.00} : {1:00.00%} : Dodged", dodges, dodgesPerc) : "",
            showparry ? string.Format("\r\n- {0:000.00} : {1:00.00%} : Parried", parrys, parrysPerc) : "",
            showblock ? string.Format("\r\n- {0:000.00} : {1:00.00%} : Blocked", blocks, blocksPerc) : "",
            showglncs ? string.Format("\r\n- {0:000.00} : {1:00.00%} : Glanced", glncs, glncsPerc) : "",
            showcrits ? string.Format("\r\n- {0:000.00} : {1:00.00%} : Crit", crits, critsPerc) : "",
                        string.Format("\r\n- {0:000.00} : {1:00.00%} : Hit", hits, hitsPerc),
            (AvgTargets != -1 ? AvgTargets : 0),
            (ttldpsOH > 0 ? ttldpsOH : 0),
            (ttldpsOH_U20 > 0 ? ttldpsOH_U20 : 0),
            (ttldpsOH > 0 ? ttldpsOH / ttldps : 0));

                /*tooltip += Environment.NewLine + Environment.NewLine + "White Damage (Off Hand)" +
                    Environment.NewLine + "Cast Time: Instant"
                                        + ", CD: " + (OHEffectiveSpeed != -1 ? OHEffectiveSpeed.ToString("0.00") : "None")
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
                    Environment.NewLine + "Targets Hit: " + AvgTargets.ToString("0.00") +
                    Environment.NewLine + "DPS: " + (ttldpsOH > 0 ? ttldpsOH.ToString("0.00") : "None") +
                    Environment.NewLine + "Percentage of Total DPS: " + (ttldpsOH > 0 ? (ttldpsOH / ttldps).ToString("00.00%") : "None");*/
            }
            return tooltip;
        }
    }

    // Templated Base Classes
    public abstract class Ability
    {
        // Constructors
        protected Ability()
        {
            // Character related
            //DPSWarrChar = null;
            //DPSWarrChar.Char = null;
            //DPSWarrChar.StatS = null;
            //DPSWarrChar.CombatFactors = null;
            //MHAtkTable = null;
            //OHAtkTable = null;
            //DPSWarrChar.Whiteattacks = null;
            //DPSWarrChar.CalcOpts = null;
            //DPSWarrChar.BossOpts = null;
            // Ability Related
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
            Targets = 1f;
            MaxRange = 5f; // In Yards 
            CD = -1f; // In Seconds
            Duration = 0; // In Seconds
            RageCost = 0;
            CastTime = -1f; // In Seconds
            GCDTime = 1.5f; // default GCD size
            StanceOkFury = false;
            StanceOkArms = false;
            StanceOkDef = false;
            UseReact = false;
            DamageBase = 0f;
            DamageBonus = 1f;
            HealingBase = 0f;
            HealingBonus = 1f;
            //BonusCritChance = 0.00f;
            BonusCritDamage = 1f;
            UseSpellHit = false;
            UseRangedHit = false;
            UseHitTable = true;
            validatedSet = false;
            SwingsOffHand = false;
            SwingsPerActivate = 1f;
            UsesGCD = true;
        }
        public static Ability NULL = new NullAbility();
        #region Variables
        public int AbilIterater;
        #endregion
        #region Get/Set
        public static string SName { get { return "Invalid"; } }
        public static string SDesc { get { return "Invalid"; } }
        public static string SIcon { get { return "trade_engineering"; } }
        public static int SSpellId { get { return 0; } }
        public virtual string Name { get { return SName; } }
        public virtual string Desc { get { return SDesc; } }
        public virtual string Icon { get { return SIcon; } }
        public virtual int SpellId { get { return SSpellId; } }
        protected bool ReqTalent { get; set; }
        protected int Talent2ChksValue { get; set; }
        public bool ReqMeleeWeap { get; set; }
        public bool ReqMeleeRange { get; set; }
        protected bool ReqMultiTargs { get; set; }
        private float _AvgTargets = -1f;
        public float AvgTargets {
            get {
                //float extraTargetsHit = Math.Min(DPSWarrChar.CalcOpts.MultipleTargetsMax, TARGETS) - 1f;
                if (_AvgTargets == -1f && Targets != -1)
                {
                    //_AvgTargets = 1f + (BossOpts.MultiTargs ? StatS.BonusTargets + (float)BossOpts.MultiTargsPerc * (Math.Min((float)BossOpts.MaxNumTargets, Targets) - 1f) : 0f);
                    if (DPSWarrChar.BossOpts.MultiTargs && DPSWarrChar.BossOpts.Targets != null && DPSWarrChar.BossOpts.Targets.Count > 0)
                    {
                        float value = 0;
                        foreach (TargetGroup tg in DPSWarrChar.BossOpts.Targets)
                        {
                            if (tg.Frequency <= 0 || tg.Chance <= 0) continue; // bad one, skip it
                            //float upTime = (tg.Frequency / BossOpts.BerserkTimer * (tg.Duration / 1000f) * tg.Chance)/* / BossOpts.BerserkTimer*/;
                            float upTime = (tg.Duration / 1000f) / tg.Frequency;//(tg.Frequency / BossOpts.BerserkTimer *  * tg.Chance)/* / BossOpts.BerserkTimer*/;
                            value += (Math.Min(10 - (tg.NearBoss ? 1 : 0), Math.Min(Targets - (tg.NearBoss ? 1 : 0), tg.NumTargs - (tg.NearBoss ? 1 : 0))) + DPSWarrChar.StatS.BonusTargets) * upTime;
                        }
                        _AvgTargets = 1f + value;
                    } else { _AvgTargets = 1f; }
                }
                return _AvgTargets;
            }
        }
        public float Targets { get; protected set; }
        public bool CanBeDodged { get; protected set; }
        public bool CanBeParried { get; protected set; }
        public bool CanBeBlocked { get; protected set; }
        public bool CanCrit { get; protected set; }
        public float MinRange { get; protected set; } // In Yards 
        public float MaxRange { get; protected set; } // In Yards
        public float CD { get; set; }
        public float Duration { get; protected set; } // In Seconds
        public float RageCost { get; protected set; }
        protected float _cachedCastTime = -1f;
        public float CastTime // In Seconds
        {
            get { return _cachedCastTime; }
            protected set {
                _cachedCastTime = value;
                _cachedUseTime = (DPSWarrChar.CalcOpts == null ? 0f : DPSWarrChar.CalcOpts.Latency + (UseReact ? DPSWarrChar.CalcOpts.React / 1000f : DPSWarrChar.CalcOpts.AllowedReact))
                    + Math.Min(Math.Max(1.5f, value), _cachedGCDTime);
            }
        }
        protected float _cachedGCDTime = 1.5f;
        public float GCDTime // In Seconds
        {
            get { return _cachedGCDTime; }
            protected set
            {
                _cachedGCDTime = value;
                _cachedUseTime = (DPSWarrChar.CalcOpts != null ? DPSWarrChar.CalcOpts.Latency + (UseReact ? DPSWarrChar.CalcOpts.React / 1000f : DPSWarrChar.CalcOpts.AllowedReact) : 0f)
                    + Math.Min(Math.Max(1.5f, _cachedCastTime), value);
            }
        }
        protected float _cachedUseTime = 0;
        public float UseTime { get { return _cachedUseTime; } }
        /// <summary>Base Damage Value (500 = 500.00 Damage)</summary>
        public float DamageBase { get; set; }
        /// <summary>Percentage Based Damage Bonus (1.5 = 150% damage)</summary>
        public float DamageBonus { get; set; }
        protected float HealingBase { get; set; }
        protected float HealingBonus { get; set; }
        /// <summary>Percentage Based Crit Chance Bonus (0.5 = 50% Crit Chance, capped between 0%-100%, factoring Boss Level Offsets)</summary>
        public virtual float BonusCritChance { get; set; }
        /// <summary>Percentage Based Crit Damage Bonus (1.5 = 150% damage)</summary>
        public float BonusCritDamage { get; set; }
        protected bool StanceOkFury { get; set; }
        protected bool StanceOkArms { get; set; }
        protected bool StanceOkDef { get; set; }
        protected bool UseReact { get; set; }
        protected DPSWarrCharacter DPSWarrChar { get; set; }
        //protected Character Char { get; set; }
        //protected WarriorTalents Talents { get { return Char.WarriorTalents; } }
        //protected Stats StatS { get; set; }
        //protected CombatFactors CombatFactors { get; set; }
        public virtual CombatTable MHAtkTable { get; protected set; }
        public virtual CombatTable OHAtkTable { get; protected set; }
        //public WhiteAttacks Whiteattacks { get; protected set; }
        //public CalculationOptionsDPSWarr DPSWarrChar.CalcOpts { get; set; }
        //public BossOptions BossOpts { get; set; }
        //public virtual float RageUseOverDur { get { return (!Validated ? 0f : Activates * RageCost); } }
        public bool SwingsOffHand { get; protected set; }
        public float SwingsPerActivate { get; protected set; }
        private float _fightDur = -1f, _fightDurO20 = -1f, _fightDurU20 = -1f;
        public float FightDuration { get { if (_fightDur == -1) { _fightDur = DPSWarrChar.BossOpts.BerserkTimer; } return _fightDur; } }
        public float FightDurationO20 { get { if (_fightDurO20 == -1) { _fightDurO20 = FightDuration * TimeOver20Perc; } return _fightDurO20; } }
        public float FightDurationU20 { get { if (_fightDurU20 == -1) { _fightDurU20 = FightDuration * TimeUndr20Perc; } return _fightDurU20; } }
        private float _timeOver20Perc = -1f, _timeUndr20Perc = -1f;
        public float TimeOver20Perc { get { if (_timeOver20Perc == -1f) { _timeOver20Perc = (DPSWarrChar.CalcOpts.M_ExecuteSpam ? 1f - (float)DPSWarrChar.BossOpts.Under20Perc : 1f); } return _timeOver20Perc; } }
        public float TimeUndr20Perc { get { if (_timeUndr20Perc == -1f) { _timeUndr20Perc = (DPSWarrChar.CalcOpts.M_ExecuteSpam ?      (float)DPSWarrChar.BossOpts.Under20Perc : 0f); } return _timeUndr20Perc; } }
        protected bool UseSpellHit { get; set; }
        protected bool UseRangedHit { get; set; }
        protected bool UseHitTable { get; set; }
        public bool IsMaint { get; protected set; }
        public bool UsesGCD { get; protected set; }
        private bool validatedSet = false;
        public virtual bool Validated { get { return validatedSet; } }
        private bool setValidation() {
            if (AbilIterater != -1 && !DPSWarrChar.CalcOpts.MaintenanceTree[AbilIterater])
            {
                validatedSet = false;
            }
            else if (ReqTalent && Talent2ChksValue < 1)
            {
                validatedSet = false;
            }
            else if (ReqMeleeWeap && (DPSWarrChar.Char.MainHand == null || DPSWarrChar.Char.MainHand.MaxDamage <= 0))
            {
                validatedSet = false;
            }
            else if (ReqMultiTargs && (!DPSWarrChar.BossOpts.MultiTargs || DPSWarrChar.BossOpts.Targets == null || DPSWarrChar.BossOpts.Targets.Count <= 0))
            {
                validatedSet = false;
            }
            else if ((DPSWarrChar.CombatFactors.FuryStance && !StanceOkFury)
                || (!DPSWarrChar.CombatFactors.FuryStance && !StanceOkArms))
            {
                validatedSet = false;
            }
            else validatedSet = true;

            return validatedSet;
        }
        /// <summary>Number of times it can possibly be activated (# times actually used may be less or same).</summary>
        public virtual float Activates { get { return !Validated ? 0f : ActivatesOverride; } }
        /// <summary>
        /// Number of times it can possibly be activated (# times actually used may
        /// be less or same). This one does not check for stance/weapon info, etc.
        /// </summary>
        protected virtual float ActivatesOverride
        {
            get
            {
                float LatentGCD = 1.5f + DPSWarrChar.CalcOpts.Latency + (UseReact ? DPSWarrChar.CalcOpts.React / 1000f : DPSWarrChar.CalcOpts.AllowedReact);
                float GCDPerc = LatentGCD / ((Duration > CD ? Duration : CD) + DPSWarrChar.CalcOpts.Latency + (UseReact ? DPSWarrChar.CalcOpts.React / 1000f : DPSWarrChar.CalcOpts.AllowedReact));
                //float Every = LatentGCD / GCDPerc;
                if (RageCost > 0f)
                {
                    /*float rageSlip = (float)Math.Pow(Whiteattacks.MHAtkTable.AnyNotLand, Whiteattacks.AvoidanceStreak * Every);
                    float rageSlip2 = Whiteattacks.MHAtkTable.AnyNotLand / Every / Whiteattacks.AvoidanceStreak * RageCost / Whiteattacks.MHSwingRage;
                    float ret = FightDuration / Every * (1f - rageSlip);
                    return ret;*/
                    return Math.Max(0f, FightDuration / (LatentGCD / GCDPerc) * (1f - DPSWarrChar.Whiteattacks.RageSlip(LatentGCD / GCDPerc, RageCost)));
                }
                else return FightDuration / (LatentGCD / GCDPerc);
                /*double test = Math.Pow((double)Whiteattacks.MHAtkTable.AnyNotLand, (double)Whiteattacks.AvoidanceStreak * Every);
                return Math.Max(0f, FightDuration / Every * (1f - Whiteattacks.AvoidanceStreak));*/
            }
        }
        protected float Healing { get { return !Validated ? 0f : HealingBase * (1f + (HealingBase > 0f ? HealingBonus : 0f)); } }
        protected float HealingOnUse { get { return Healing * (1f + (HealingBase > 0f ? DPSWarrChar.CombatFactors.HealthBonus : 0f)); } }
        //protected float AvgHealingOnUse { get { return HealingOnUse * Activates; } }
        protected float Damage { get { return !Validated ? 0f : DamageOverride; } }
        public virtual float DamageOverride { get { return Math.Max(0f, DamageBase * DamageBonus); } }
        public float DamageOnUse { get { return (Validated ? DamageOnUseOverride : 0f); } }
        public virtual float DamageOnUseOverride
        {
            get
            {
                float dmg = Damage; // Base Damage
                dmg *= DPSWarrChar.CombatFactors.DamageBonus; // Global Damage Bonuses
                dmg *= DPSWarrChar.CombatFactors.DamageReduction; // Global Damage Penalties

                // Work the Attack Table
                float dmgDrop = (1f
                    - MHAtkTable.Miss   // no damage when being missed
                    - MHAtkTable.Dodge  // no damage when being dodged
                    - MHAtkTable.Parry  // no damage when being parried
                    //- MHAtkTable.Glance // glancing handled below
                    - MHAtkTable.Block  // blocked handled below
                    - MHAtkTable.Crit); // crits   handled below

                //float dmgGlance = dmg * MHAtkTable.Glance * CombatFactors.ReducWhGlancedDmg;//Partial Damage when glancing, this doesn't actually do anything since glance is always 0
                float dmgBlock = dmg * MHAtkTable.Block * CombatFactors.ReducYWBlockedDmg;//Partial damage when blocked
                float dmgCrit = dmg * MHAtkTable.Crit * (1f + DPSWarrChar.CombatFactors.BonusYellowCritDmg) * BonusCritDamage;//Bonus   Damage when critting

                dmg *= dmgDrop;

                dmg += /*dmgGlance +*/ dmgBlock + dmgCrit;

                return dmg * AvgTargets;
            }
        }
        #endregion
        #region Functions
        protected void Initialize()
        {
            if (!UseSpellHit && !UseRangedHit && UseHitTable && CanBeDodged && CanCrit && BonusCritChance == 0f)
            {
                MHAtkTable = DPSWarrChar.CombatFactors.AttackTableBasicMH;
                OHAtkTable = DPSWarrChar.CombatFactors.AttackTableBasicOH;
            }
            else
            {
                MHAtkTable = new AttackTable(DPSWarrChar.Char, DPSWarrChar.StatS, DPSWarrChar.CombatFactors, DPSWarrChar.CalcOpts, DPSWarrChar.BossOpts, this, true, UseSpellHit, UseRangedHit, !UseHitTable);
                OHAtkTable = new AttackTable(DPSWarrChar.Char, DPSWarrChar.StatS, DPSWarrChar.CombatFactors, DPSWarrChar.CalcOpts, DPSWarrChar.BossOpts, this, false, UseSpellHit, UseRangedHit, !UseHitTable);
            }
            setValidation();
        }
        public virtual float GetRageUseOverDur(float acts)
        {
            if (!Validated) { return 0f; }
            return acts * RageCost;
        }
        //public float GetHealing() { if (!Validated) { return 0f; } return 0f; }
        public float GetAvgDamageOnUse(float acts)
        {
            if (!Validated) { return 0f; }
            return DamageOnUse * acts;
        }
        /*public virtual float GetDPS(float acts)
        {
            if (!Validated) { return 0f; }
            //float adou = GetAvgDamageOnUse(acts);
            return GetAvgDamageOnUse(acts) / FightDuration;
        }*/
        public virtual float GetDPS(float acts, float perc)
        {
            if (!Validated) { return 0f; }
            //float adou = GetAvgDamageOnUse(acts);
            return GetAvgDamageOnUse(acts) / (FightDuration * perc);
        }
        public float GetAvgHealingOnUse(float acts)
        {
            if (!Validated) { return 0f; }
            return HealingOnUse * acts;
        }
        /*public virtual float GetHPS(float acts)
        {
            if (!Validated) { return 0f; }
            //float adou = GetAvgHealingOnUse(acts);
            return GetAvgHealingOnUse(acts) / FightDuration;
        }*/
        public virtual float GetHPS(float acts, float perc)
        {
            if (!Validated) { return 0f; }
            //float adou = GetAvgHealingOnUse(acts);
            return GetAvgHealingOnUse(acts) / (FightDuration * perc);
        }
        //public virtual float ContainCritValue_MH { get { return Math.Min(1f, DPSWarrChar.CombatFactors.Cmhycrit + BonusCritChance); } }
        //public virtual float ContainCritValue_OH { get { return Math.Min(1f, DPSWarrChar.CombatFactors.Cohycrit + BonusCritChance); } }
        /*public virtual float ContainCritValue(bool IsMH) {
            //float BaseCrit = IsMH ? DPSWarrChar.CombatFactors.Cmhycrit : DPSWarrChar.CombatFactors.Cohycrit;
            return Math.Min(1f, (IsMH ? DPSWarrChar.CombatFactors.Cmhycrit : DPSWarrChar.CombatFactors.Cohycrit) + BonusCritChance);
        }*/
        protected float GetXActs(AttackTableSelector i, float acts)
        {
            float retVal = 0f;
            switch (i)
            {
                case AttackTableSelector.Missed: { retVal = acts * MHAtkTable.Miss; break; }
                case AttackTableSelector.Dodged: { retVal = acts * MHAtkTable.Dodge; break; }
                case AttackTableSelector.Parried: { retVal = acts * MHAtkTable.Parry; break; }
                case AttackTableSelector.Blocked: { retVal = acts * MHAtkTable.Block; break; }
                case AttackTableSelector.Glance: { retVal = acts * MHAtkTable.Glance; break; }
                case AttackTableSelector.Crit: { retVal = acts * MHAtkTable.Crit; break; }
                case AttackTableSelector.Hit: { retVal = acts * MHAtkTable.Hit; break; }
                default: { break; }
            }
            return retVal;
        }
        public virtual string GenTooltip(float acts, float dpsO20, float dpsU20, float ttldpsperc)
        {
            //float Over20 = DPSWarrChar.CalcOpts.M_ExecuteSpam ? 1f - (float)BossOpts.Under20Perc : 1f;
            //float Undr20 = DPSWarrChar.CalcOpts.M_ExecuteSpam ?      (float)BossOpts.Under20Perc : 1f;

            float misses = GetXActs(AttackTableSelector.Missed, acts), missesPerc = (acts == 0f ? 0f : misses / acts);
            float dodges = GetXActs(AttackTableSelector.Dodged, acts), dodgesPerc = (acts == 0f ? 0f : dodges / acts);
            float parrys = GetXActs(AttackTableSelector.Parried, acts), parrysPerc = (acts == 0f ? 0f : parrys / acts);
            float blocks = GetXActs(AttackTableSelector.Blocked, acts), blocksPerc = (acts == 0f ? 0f : blocks / acts);
            float crits = GetXActs(AttackTableSelector.Crit, acts), critsPerc = (acts == 0f ? 0f : crits / acts);
            float hits = GetXActs(AttackTableSelector.Hit, acts), hitsPerc = (acts == 0f ? 0f : hits / acts);

            bool showmisss = misses > 0f;
            bool showdodge = CanBeDodged && dodges > 0f;
            bool showparry = CanBeParried && parrys > 0f;
            bool showblock = CanBeBlocked && blocks > 0f;
            bool showcrits = CanCrit && crits > 0f;

            string tt = string.Format(@"*{0}
Cast Time: {1}, CD: {2}, Rage Generated: {3}

{4:000.00} Activates over Attack Table:{5}{6}{7}{8}{9}{10}{11}

Targets Hit: {12:0.00}
DPS: {13:0.00}|{14:0.00}
Percentage of Total DPS: {15:00.00%}",
            Name,
            (CastTime != -1 ? string.Format("{0:0.00}", CastTime) : "Instant"),
            (CD != -1 ? string.Format("{0:0.00}", CD) : "None"),
            (RageCost != -1 ? string.Format("{0:0.00}", (-1f * RageCost)) : "None"),
            acts,
            showmisss ? string.Format("\r\n- {0:000.00} : {1:00.00%} : Missed", misses, missesPerc) : "",
            showdodge ? string.Format("\r\n- {0:000.00} : {1:00.00%} : Dodged", dodges, dodgesPerc) : "",
            showparry ? string.Format("\r\n- {0:000.00} : {1:00.00%} : Parried", parrys, parrysPerc) : "",
            showblock ? string.Format("\r\n- {0:000.00} : {1:00.00%} : Blocked", blocks, blocksPerc) : "",
            "", // ignore glance
            showcrits ? string.Format("\r\n- {0:000.00} : {1:00.00%} : Crit", crits, critsPerc) : "",
                        string.Format("\r\n- {0:000.00} : {1:00.00%} : Hit", hits, hitsPerc),
            (Targets != -1 ? AvgTargets : 1),
            (dpsO20 > 0 ? dpsO20 : 0),
            (dpsU20 > 0 ? dpsU20 : 0),
            (ttldpsperc > 0 ? ttldpsperc : 0));

            return tt;

            /*string tooltip = "*" + Name +
                Environment.NewLine + "Cast Time: " + (CastTime != -1 ? CastTime.ToString() : "Instant")
                                    + ", CD: " + (CD != -1 ? CD.ToString() : "None")
                                    + ", RageCost: " + (RageCost != -1 ? RageCost.ToString() : "None") +
            Environment.NewLine + Environment.NewLine + acts.ToString("000.00") + " Activates over Attack Table:" +
            (showmisss ? Environment.NewLine + "- " + misses.ToString("000.00") + " : " + missesPerc.ToString("00.00%") + " : Missed " : "") +
            (showdodge ? Environment.NewLine + "- " + dodges.ToString("000.00") + " : " + dodgesPerc.ToString("00.00%") + " : Dodged " : "") +
            (showparry ? Environment.NewLine + "- " + parrys.ToString("000.00") + " : " + parrysPerc.ToString("00.00%") + " : Parried " : "") +
            (showblock ? Environment.NewLine + "- " + blocks.ToString("000.00") + " : " + blocksPerc.ToString("00.00%") + " : Blocked " : "") +
            (showcrits ? Environment.NewLine + "- " + crits.ToString("000.00") + " : " + critsPerc.ToString("00.00%") + " : Crit " : "") +
                         Environment.NewLine + "- " + hits.ToString("000.00") + " : " + hitsPerc.ToString("00.00%") + " : Hit " +
                Environment.NewLine +
                //Environment.NewLine + "Damage per Blocked|Hit|Crit: x|x|x" +
                Environment.NewLine + "Targets Hit: " + (Targets != -1 ? AvgTargets.ToString("0.00") : "None") +
                //Environment.NewLine + "DPS: " + (GetDPS(acts * Over20, Over20) > 0 ? GetDPS(acts, Over20).ToString("0.00") : "None") + "+" + (GetDPS(acts, Undr20) > 0 ? GetDPS(acts, Undr20).ToString("0.00") : "None") +
                Environment.NewLine + "DPS: " + (dpsO20 > 0 ? dpsO20.ToString("0.00") : "None") + "|" + (dpsU20 > 0 ? dpsU20.ToString("0.00") : "None") +
                Environment.NewLine + "Percentage of Total DPS: " + (ttldpsperc > 0 ? ttldpsperc.ToString("00.00%") : "None");

            return tooltip;*/
        }
        #endregion
    }
    public class NullAbility : Ability
    {
        public override CombatTable MHAtkTable { get { return CombatTable.NULL; } protected set { ; } }
        public override CombatTable OHAtkTable { get { return CombatTable.NULL; } protected set { ; } }
        //public override float RageUseOverDur { get { return 0; } }
        protected override float ActivatesOverride { get { return 0; } }
        public override float DamageOnUseOverride { get { return 0; } }
        public override float DamageOverride { get { return 0; } }
        public override string GenTooltip(float acts, float dpsO20, float dpsU20, float ttldpsperc) { return String.Empty; }
        public override float GetRageUseOverDur(float acts) { return 0; }
        public override bool Validated { get { return false; } }
        public override float Activates { get { return 0; } }
        //public override float GetDPS(float acts) { return 0; }
        public override float GetDPS(float acts, float perc) { return 0; }
    }
    public class DoT : Ability
    {
        // Constructors
        public DoT() { }
        // Variables
        public float TimeBtwnTicks { get; set; } // In Seconds
        protected float addMisses;
        protected float addDodges;
        protected float addParrys;
        // Functions
        public virtual float InitialDamage { get { return 0f; } }
        public virtual float TickSize { get { return 0f; } }
        public virtual float TtlTickingTime { get { return Duration; } }
        public virtual float TickLength { get { return TimeBtwnTicks; } }
        public virtual float NumTicks { get { return TtlTickingTime / TickLength; } }
        //public virtual float DmgOverTickingTime { get { return TickSize * NumTicks; } }
        public virtual float GetDmgOverTickingTime(float acts) { return TickSize * (NumTicks * acts); }
        public override float GetDPS(float acts, float perc)
        {
            return GetDmgOverTickingTime(acts) / (FightDuration * perc);
        }
        //public virtual float DPS { get { return TickSize / TickLength; } }
    }
    public class BuffEffect : Ability
    {
        // Constructors
        public BuffEffect()
        {
            IsMaint = true;
            Effect = null;
            Effect2 = null;
        }
        // Variables
        protected float addMisses;
        protected float addDodges;
        protected float addParrys;
        // Get/Set
        public SpecialEffect Effect { get; set; }
        public SpecialEffect Effect2 { get; set; }
        // Functions
        public virtual Stats AverageStats
        {
            get
            {
                if (!Validated) { return new Stats(); }
                Stats bonus = (Effect == null) ? new Stats() { AttackPower = 0f, } : Effect.GetAverageStats(0f, MHAtkTable.Hit + MHAtkTable.Crit, DPSWarrChar.Whiteattacks.MHEffectiveSpeed, FightDuration);
                bonus += (Effect2 == null) ? new Stats() { AttackPower = 0f, } : Effect2.GetAverageStats(0f, MHAtkTable.Hit + MHAtkTable.Crit, DPSWarrChar.Whiteattacks.MHEffectiveSpeed, FightDuration);
                return bonus;
            }
        }
    }
    #endregion
}
