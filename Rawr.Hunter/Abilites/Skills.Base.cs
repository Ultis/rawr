using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Hunter.Skills
{
    public class AbilWrapper
    {
        public AbilWrapper(Skills.Ability abil) { ability = abil; }
        public Skills.Ability ability { get; set; }
        public float numActivates { get; set; }
        public float Focus { get { return ability.GetFocusUseOverDur(numActivates); } }
        public float DPS { get { return ability.GetDPS(numActivates); } }
//        public float HPS { get { return ability.GetHPS(numActivates); } }
        public bool isDamaging { get { return ability.DamageOverride > 0f; } }
        public string GenTooltip(float fRotationDPS) { return this.ability.GenTooltip(numActivates, fRotationDPS); }
    }

    public enum AttackTableSelector { Missed = 0, Crit, Hit }

    // TODO: refactor this to inherit from Base Ability?
    public class WhiteAttacks
    {
        // Constructors
        public WhiteAttacks(Character character, StatsHunter stats, CombatFactors cf, CalculationOptionsHunter calcOpts, BossOptions bossOpts)
        {
            Char = character;
            StatS = stats;
            Talents = Char.HunterTalents == null ? new HunterTalents() : Char.HunterTalents;
            combatFactors = cf;
            CalcOpts = calcOpts;
            BossOpts = bossOpts;
            RWAtkTable = new AttackTable(Char, StatS, combatFactors, calcOpts, false, false);
            FightDuration = BossOpts.BerserkTimer;

            Targets = BossOpts.Targets.Count; // Should update to better handle Target objects
            HSOverridesOverDur = 0f;
            CLOverridesOverDur = 0f;
            Steady_Freq = 0f;
        }
        public void InvalidateCache()
        {
            _RwDamageOnUse = -1f;
        }
        #region Variables
        private readonly Character Char;
        private StatsHunter StatS;
        private readonly HunterTalents Talents;
        private readonly CombatFactors combatFactors;
        private CalculationOptionsHunter CalcOpts;
        private BossOptions BossOpts;
        private float TARGETS;
        public AttackTable RWAtkTable;
        private float OVDOVERDUR_HS;
        private float OVDOVERDUR_CL;
        private float FightDuration;
        private float Targets { get { return TARGETS; } set { TARGETS = value; } }
        private float AvgTargets {
            get {
//                if (BossOpts.MultiTargs)
//                {
//                    return 1f + (BossOpts.MultiTargsTime / BossOpts.BerserkTimer)  + StatS.BonusTargets;
//                }
//                else 
                { return 1f; }
            }
        }
        // Get/Set
        public float HSOverridesOverDur { get { return OVDOVERDUR_HS; } set { OVDOVERDUR_HS = value; } }
        public float CLOverridesOverDur { get { return OVDOVERDUR_CL; } set { OVDOVERDUR_CL = value; } }
        public float Steady_Freq;
        #endregion
        // Ranged Weapon
        public float RwEffectiveSpeed { get { return combatFactors.RWSpeed; } }
        public float RwDamage
        {
            get
            {
                //float DamageBase = combatFactors.AvgMhWeaponDmgUnhasted;
                //float DamageBonus = 1f + 0f;
                return combatFactors.AvgRwWeaponDmgUnhasted * AvgTargets;
            }
        }
        private float _RwDamageOnUse = -1f;
        public float RwDamageOnUse
        {
            get
            {
                if (_RwDamageOnUse == -1f)
                {
                    float dmg = RwDamage;                  // Base Damage
                    dmg *= combatFactors.DamageBonus;      // Global Damage Bonuses
                    dmg *= combatFactors.DamageReduction;  // Global Damage Penalties

                    // Work the Attack Table
                    float dmgDrop = (1f
                        - RWAtkTable.Miss   // no damage when being missed
                        - RWAtkTable.Crit); // crits   handled below

                    float dmgCrit = dmg * RWAtkTable.Crit * (1f + combatFactors.BonusWhiteCritDmg); //Bonus Damage when critting

                    dmg *= dmgDrop;
                    dmg += dmgCrit;

                    _RwDamageOnUse = dmg;
                }
                return _RwDamageOnUse;
            }
        }
        public float TotalRwDamageOnUse { get { return RwDamageOnUse * RwActivates; } }
        public float RwActivates {
            get {
                if (RwEffectiveSpeed != 0)
                    return FightDuration / RwEffectiveSpeed;
                else return 0f;
            }
        }
        public float RwActivatesNoHS {
            get {
                if (RwEffectiveSpeed != 0)
                    return FightDuration / RwEffectiveSpeed;
                else return 0f;
            }
        }
        public float RwDPS { get { return TotalRwDamageOnUse / FightDuration; } }
        // Attacks Over Fight Duration
        public float LandedAtksOverDur { get { return LandedAtksOverDurRw; } }
        public float LandedAtksOverDurRw { get { return RwActivates * RWAtkTable.AnyLand; } }
        private float CriticalAtksOverDur { get { return CriticalAtksOverDurRW; } }
        public float CriticalAtksOverDurRW { get { return RwActivates * RWAtkTable.Crit; } }
        // Other

        /// <summary>
        /// TODO Zhok: Needed???
        /// </summary>
        /// <param name="abilInterval"></param>
        /// <param name="focusCost"></param>
        /// <returns></returns>
        public float FocusSlip(float abilInterval, float focusCost)
        {
            //float whiteAtkInterval = (MhActivates + OhActivates) / FightDuration;
            //return MHAtkTable.AnyNotLand / abilInterval / whiteAtkInterval * manaCost / MHSwingMana;
            //float whiteMod = (MhActivates * MHSwingMana + (combatFactors.useOH ? OhActivates * OHSwingMana : 0f)) / FightDuration;
            if (RwActivates <= 0f) { return 0f; }
            return (RWAtkTable.Miss * focusCost) / (abilInterval * ((RwActivates /* * (MHSwingMana + MHUWProcValue)*/) / FightDuration));
        }
        public virtual float GetXActs(AttackTableSelector i, float acts)
        {
            AttackTable table = RWAtkTable;
            float retVal = 0f;
            switch (i)
            {
                case AttackTableSelector.Missed:  { retVal = acts * table.Miss;   break; }
                case AttackTableSelector.Crit:    { retVal = acts * table.Crit;   break; }
                case AttackTableSelector.Hit:     { retVal = acts * table.Hit;    break; }
                default: { break; }
            }
            return retVal;
        }
        public string GenTooltip(float totalDPS = 0)
        {
            float acts = RwActivates;
            float misses = GetXActs(AttackTableSelector.Missed, acts), missesPerc = (acts == 0f ? 0f : misses / acts);
            float crits = GetXActs(AttackTableSelector.Crit, acts), critsPerc = (acts == 0f ? 0f : crits / acts);
            float hits = GetXActs(AttackTableSelector.Hit, acts), hitsPerc = (acts == 0f ? 0f : hits / acts);

            bool showmisss = misses > 0f;
            bool showcrits = crits > 0f;

            string tooltip = string.Format("{0:0.0}*White Damage (Ranged Weapon)", RwDPS) +
                Environment.NewLine + "Shot Speed: " + (RwEffectiveSpeed != -1 ? RwEffectiveSpeed.ToString("0.00") : "None") +
            Environment.NewLine + Environment.NewLine + acts.ToString("000.00") + " Activates over Attack Table:" +
            (showmisss ? Environment.NewLine + "- " + misses.ToString("000.00") + " : " + missesPerc.ToString("00.00%") + " : Missed " : "") +
            (showcrits ? Environment.NewLine + "- " + crits.ToString("000.00") + " : " + critsPerc.ToString("00.00%") + " : Crit " : "") +
                         Environment.NewLine + "- " + hits.ToString("000.00") + " : " + hitsPerc.ToString("00.00%") + " : Hit " +
                Environment.NewLine +
                Environment.NewLine + string.Format("DPS: {0}", RwDPS) +
                Environment.NewLine + string.Format("% of Total: {0:0.0}%", (totalDPS > 0 ? (this.RwDPS / totalDPS) * 100 : 100));

            return tooltip;
        }
    }

    // Templated Base Classes
    public class Ability
    {
        // Constructors
        public Ability()
        {
            // Character related
            Char = null;
            Talents = null;
            StatS = null;
            combatFactors = null;
            RWAtkTable = null;
            Whiteattacks = null;
            CalcOpts = null;
            // Ability Related
            Name = "Invalid";
            ReqTalent = false;
            CanCrit = true;
            CRITBONUS = 0.5f;
            CRITBONUSMULTIPLIER = 1.0f;
            Talent2ChksValue = 0;
            AbilIterater = -1;
            ReqRangedWeap = false;
            ReqSkillsRange = false;
            ReqMultiTargs = false;
            Targets = 1f;
            MaxRange = 5f; // In Yards 
            CD = -1f; // In Seconds
            Duration = -1f; // In Seconds
            FocusCost = -1f;
            CastTime = -1f; // In Seconds
            UseReact = false;
            DamageBase = 0f;
            DamageBonus = 1f;
            BonusCritChance = 0.00f;
            UseSpellHit = false;
            Consumes_Tier12_4pc = false;
        }
        public static Ability NULL = new NullAbility();
        #region Variables
        private string NAME;
        private float DAMAGEBASE;
        private float DAMAGEBONUS;
        private float HEALINGBASE;
        private float HEALINGBONUS;
        private float BONUSCRITCHANCE;
        private bool CANCRIT;
        private float CRITBONUS;
        private float CRITBONUSMULTIPLIER;
        private bool REQTALENT;
        private int TALENT2CHKSVALUE;
        private bool REQRANGEDWEAP;
        private bool REQSKILLSRANGE;
        private bool REQMULTITARGS;
        private float TARGETS;
        /// <summary>
        /// Minimum Range - in yards.
        /// </summary>
        private float MINRANGE;
        /// <summary>
        /// Maximum Range - in yards.
        /// </summary>
        private float MAXRANGE;
        /// <summary>
        /// Cooldown of the Ability - in seconds.
        /// </summary>
        private float CD; // In Seconds
        /// <summary>
        /// Duration of the Ability - in seconds.
        /// </summary>
        private float DURATION; // In Seconds
        //private float MANACOST;
        private float FOCUSCOST;
        //private bool MANACOSTISPERC;
        /// <summary>
        /// Cast time - in seconds.
        /// </summary>
        private float CASTTIME; // In Seconds
        /// <summary>
        /// User React - Is this ability used as a proc effect
        /// </summary>
        private bool USEREACT; // if this ability is used as a proc effect
        private Character CHARACTER;
        private HunterTalents TALENTS;
        private StatsHunter STATS;
        private CombatFactors COMBATFACTORS;
        private AttackTable RWATTACKTABLE;
        private WhiteAttacks WHITEATTACKS;
        private CalculationOptionsHunter CALCOPTS;
        private BossOptions BOSSOPTS;
        private bool USESPELLHIT = false;
        private bool USEHITTABLE = true;
        public int AbilIterater;
        public float Mastery = 0f;
        public bool RefreshesSS = false; // Refreshes Serpent Sting
        public bool Consumes_Tier12_4pc = false;
        #endregion
        #region Get/Set
        public string Name { get { return NAME; } set { NAME = value; } }
        protected bool ReqTalent { get { return REQTALENT; } set { REQTALENT = value; } }
        protected int Talent2ChksValue { get { return TALENT2CHKSVALUE; } set { TALENT2CHKSVALUE = value; } }
        protected bool ReqRangedWeap { get { return REQRANGEDWEAP; } set { REQRANGEDWEAP = value; } }
        protected bool ReqSkillsRange { get { return REQSKILLSRANGE; } set { REQSKILLSRANGE = value; } }
        protected bool ReqMultiTargs { get { return REQMULTITARGS; } set { REQMULTITARGS = value; } }
        private float _AvgTargets = -1f;
        public float AvgTargets
        {
            get
            {
                if (_AvgTargets == -1f)
                {
                    _AvgTargets = 1f; 
                        // only Multishot will be multi-target
                        //+ (BossOpts.MultiTargs ?
                        //   StatS.BonusTargets +
                        //   (BossOpts.MultiTargsTime / BossOpts.BerserkTimer) // *
                        //   (Math.Min(CalcOpts.MultipleTargetsMax, TARGETS) - 1f)
                        //   : 0f);
                }
                return _AvgTargets;
            }
        }
        protected float Targets { get { return TARGETS; } set { TARGETS = value; } }
        public bool CanCrit { get { return CANCRIT; } set { CANCRIT = value; } }
        public float MinRange { get { return MINRANGE; } set { MINRANGE = value; } } // In Yards 
        public float MaxRange { get { return MAXRANGE; } set { MAXRANGE = value; } } // In Yards
        public float Cd
        { // In Seconds
            get { return CD; }
            set
            {
                /*float AssignedCD = value;
                float LatentGCD = 1.5f + CalcOpts.GetLatency();
                float CDs2Pass = 0f;
                for (int count = 0; count < FightDuration; count++) {
                    CDs2Pass = count * LatentGCD;
                    if (CDs2Pass >= AssignedCD) { break; }
                }
                CD = CDs2Pass;
                //*/
                CD = value;
            }
        }
        public float Duration { get { return DURATION; } set { DURATION = value; } } // In Seconds
        public float FocusCost { get { return FOCUSCOST; } set { FOCUSCOST = value; } }
        public float CastTime { get { return CASTTIME; } set { CASTTIME = value; } } // In Seconds
        /// <summary>Base Damage Value (500 = 500.00 Damage)</summary>
        protected float DamageBase { get { return DAMAGEBASE; } set { DAMAGEBASE = value; } }
        /// <summary>Percentage Based Damage Bonus (1.5 = 150% damage)</summary>
        protected float DamageBonus { get { return DAMAGEBONUS; } set { DAMAGEBONUS = value; } }
        public float BonusCritChance { 
            get { return BONUSCRITCHANCE; } 
            set 
            {
                if (BONUSCRITCHANCE != value)
                {
                    BONUSCRITCHANCE = value;
                    // Recalc because it's a new value.
                    if (RWAtkTable != null)
                        this.RWAtkTable.Reset();
                }
            } 
        }
        protected bool UseReact { get { return USEREACT; } set { USEREACT = value; } }
        protected Character Char
        {
            get { return CHARACTER; }
            set
            {
                CHARACTER = value;
                if (CHARACTER != null)
                {
                    Talents = CHARACTER.HunterTalents;
                    if (BossOpts == null) BossOpts = CHARACTER.BossOptions;
                    //StatS = CalculationsHunter.GetCharacterStats(CHARACTER, null);
                    //combatFactors = new CombatFactors(CHARACTER, StatS);
                    //Whiteattacks = Whiteattacks;
                    //CalcOpts = CHARACTER.CalculationOptions as CalculationOptionsHunter;
                }
                else
                {
                    Talents = null;
                    StatS = null;
                    combatFactors = null;
                    RWAtkTable = null;
                    Whiteattacks = null;
                    CalcOpts = null;
                }
            }
        }
        protected HunterTalents Talents { get { return TALENTS; } set { TALENTS = value; } }
        protected StatsHunter StatS { get { return STATS; } set { STATS = value; } }
        public CombatFactors combatFactors { get { return COMBATFACTORS; } set { COMBATFACTORS = value; } }
        public virtual AttackTable RWAtkTable { get { return RWATTACKTABLE; } protected set { RWATTACKTABLE = value; } }
        public WhiteAttacks Whiteattacks { get { return WHITEATTACKS; } set { WHITEATTACKS = value; } }
        protected CalculationOptionsHunter CalcOpts { get { return CALCOPTS; } set { CALCOPTS = value; } }
        protected BossOptions BossOpts { get { return BOSSOPTS; } set { BOSSOPTS = value; } }

        /// <summary>
        /// TODO Zhok: Check if needed and 4 what..
        /// </summary>
        public virtual float FocusUseOverDur 
        { 
            get 
            { 
                return (!Validated ? 0f : Activates * this.FocusCost); 
            } 
        }
        protected float FightDuration { get { return BossOpts.BerserkTimer; } }
        protected bool UseSpellHit { get { return USESPELLHIT; } set { USESPELLHIT = value; } }
        protected bool UseHitTable { get { return USEHITTABLE; } set { USEHITTABLE = value; } }
        public bool isMaint { get; protected set; }
        public bool UsesGCD { get; protected set; }
        public float GCDTime { get; protected set; } // In Seconds
        public float SwingsPerActivate { get; protected set; }
        public float UseTime { get { return CalcOpts.Latency + (UseReact ? CalcOpts.React / 1000f : CalcOpts.AllowedReact) + Math.Min(Math.Max(1.5f, CastTime), GCDTime); } }
        private bool? validatedSet = null;
        public virtual bool Validated
        {
            get
            {
                if (validatedSet != null)
                {
                    return (validatedSet == true);
                }

                if (ReqTalent && Talent2ChksValue < 1)
                {
                    validatedSet = false;
                }
                else if (ReqRangedWeap && (Char.Ranged == null || Char.Ranged.MaxDamage <= 0))
                {
                    validatedSet = false;
                }
                else if (ReqMultiTargs && (!BossOpts.MultiTargs || BossOpts.MultiTargsTime == 0))
                {
                    validatedSet = false;
                }
                else validatedSet = true;

                return (validatedSet == true);
            }
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
                float LatentGCD = 1.5f + CalcOpts.Latency + (UseReact ? CalcOpts.AllowedReact : 0f);
                float GCDPerc = LatentGCD / ((Duration > Cd ? Duration : Cd) + CalcOpts.Latency + (UseReact ? CalcOpts.AllowedReact : 0f));
                if (FocusCost > 0f)
                {
                    return Math.Max(0f, FightDuration / (LatentGCD / GCDPerc) * (1f - Whiteattacks.FocusSlip(LatentGCD / GCDPerc, FocusCost)));
                }
                else return FightDuration / (LatentGCD / GCDPerc);
            }
        }
        protected virtual float Damage { get { return !Validated ? 0f : DamageOverride; } }
        public virtual float DamageOverride { 
            get 
            { 
                return Math.Max(0f, DamageBase * DamageBonus ); 
            } 
        }
        public virtual float DamageOnUse
        {
            get
            {
                float dmg = Damage; // Base Damage
                dmg *= combatFactors.DamageBonus; // Global Damage Bonuses
//                dmg *= combatFactors.DamageReduction; // Global Damage Penalties

                // Work the Attack Table
                float dmgDrop = (1f
                    - RWAtkTable.Miss   // no damage when being missed
                    - RWAtkTable.Crit); // crits   handled below

                float dmgCrit = dmg * RWAtkTable.Crit * (1f + combatFactors.BonusYellowCritDmg); //Bonus Damage when critting

                dmg *= dmgDrop;
                dmg += dmgCrit;

                return dmg;
            }
        }
        protected virtual float DamageOnUseOverride { get { return DamageOnUse; } }
        protected virtual float AvgDamageOnUse { get { return DamageOnUse * Activates; } }
        public virtual float DPS { get { return AvgDamageOnUse / FightDuration; } }
        #endregion
        #region Functions
        protected void Initialize()
        {
            if (!UseSpellHit && UseHitTable && /*CanBeDodged &&*/ CanCrit && BonusCritChance == 0f) {
                RWAtkTable = combatFactors.AttackTableBasicRW;
            } else {
                RWAtkTable = new AttackTable(Char, StatS, combatFactors, CalcOpts, this, UseSpellHit, !UseHitTable);
            }
        }
        /// <summary>
        /// Gets Focus use over duration
        /// TODO Zhok: Check if needed and correct
        /// </summary>
        /// <param name="acts"></param>
        /// <returns></returns>
        public virtual float GetFocusUseOverDur(float acts)
        {
            if (!Validated) { return 0f; }
            return acts * this.FocusCost;
        }

        // WTF?
        public virtual float GetHealing() 
        { 
            if (!Validated) { return 0f; } 
            return 0f; 
        }
        public virtual float GetAvgDamageOnUse(float acts)
        {
            if (!Validated) { return 0f; }
            return DamageOnUse * acts;
        }
        public virtual float GetDPS(float acts)
        {
            if (!Validated) { return 0f; }
            //float adou = GetAvgDamageOnUse(acts);
            return GetAvgDamageOnUse(acts) / FightDuration;
        }
        public virtual float GetDPS(float acts, float perc)
        {
            if (!Validated) { return 0f; }
            //float adou = GetAvgDamageOnUse(acts);
            return GetAvgDamageOnUse(acts) / (FightDuration * perc);
        }
        public virtual float ContainCritValue_RW { get { return Math.Min(1f, combatFactors._c_rwycrit + BonusCritChance); } }
        /*public virtual float ContainCritValue(bool IsMH) {
            //float BaseCrit = IsMH ? combatFactors._c_mhycrit : combatFactors._c_ohycrit;
            return Math.Min(1f, (IsMH ? combatFactors._c_mhycrit : combatFactors._c_ohycrit) + BonusCritChance);
        }*/
        protected virtual float GetXActs(AttackTableSelector i, float acts)
        {
            float retVal = 0f;
            switch (i)
            {
                case AttackTableSelector.Missed: { retVal = acts * RWAtkTable.Miss; break; }
                case AttackTableSelector.Crit: { retVal = acts * RWAtkTable.Crit; break; }
                case AttackTableSelector.Hit: { retVal = acts * RWAtkTable.Hit; break; }
                default: { break; }
            }
            return retVal;
        }
        public virtual string GenTooltip(float acts, float ttldps)
        {
            float misses = GetXActs(AttackTableSelector.Missed, acts), missesPerc = (acts == 0f ? 0f : misses / acts);
            float crits = GetXActs(AttackTableSelector.Crit, acts), critsPerc = (acts == 0f ? 0f : crits / acts);
            float hits = GetXActs(AttackTableSelector.Hit, acts), hitsPerc = (acts == 0f ? 0f : hits / acts);

            bool showmisss = misses > 0f;
            bool showcrits = CanCrit && crits > 0f;
            float localDPS = GetDPS(acts);

            string tooltip = string.Format("{0:0.0}*{1}\n", localDPS, Name)
                                    + string.Format("Cast Time: {0}, CD: {1}, FocusCost: {2}\n", 
                                    (CastTime != -1 ? CastTime.ToString() : "Instant"), 
                                    (Cd != -1 ? Cd.ToString() : "None"), 
                                    (FocusCost != -1 ? FocusCost.ToString() : "None")) +
                Environment.NewLine + acts.ToString("000.00") + " Activates over Attack Table:" +
            (showmisss ? Environment.NewLine + "- " + misses.ToString("000.00") + " : " + missesPerc.ToString("00.00%") + " : Missed " : "") +
            (showcrits ? Environment.NewLine + "- " + crits.ToString("000.00") + " : " + critsPerc.ToString("00.00%") + " : Crit " : "") +
                         Environment.NewLine + "- " + hits.ToString("000.00") + " : " + hitsPerc.ToString("00.00%") + " : Hit " +
                Environment.NewLine +
                Environment.NewLine + "Targets Hit: " + (Targets != -1 ? AvgTargets.ToString("0.00") : "None") +
                Environment.NewLine + "DPS: " + (localDPS > 0 ? localDPS.ToString("0.00") : "None") +
                Environment.NewLine + string.Format("Percentage of Total DPS: {0}", (ttldps > 0 ? (localDPS / ttldps).ToString("00.00%") : "None"));

            return tooltip;
        }
        #endregion
    }
    public class NullAbility : Ability
    {
        public override AttackTable RWAtkTable {
            get {
                return (AttackTable)CombatTable.NULL;
            }
            protected set { ; }
        }
        public override float FocusUseOverDur { get { return 0; } }
        protected override float ActivatesOverride { get { return 0; } }
        protected override float DamageOnUseOverride { get { return 0; } }
        public override float DamageOverride { get { return 0; } }
        public override string GenTooltip(float acts, float ttldpsperc) { return String.Empty; }
        public override float GetFocusUseOverDur(float acts) { return 0; }
        public override bool Validated { get { return false; } }
        public override float Activates { get { return 0; } }
        public override float GetDPS(float acts) { return 0; }
    }
    public class OnAttack : Ability
    {
        // Constructors
        public OnAttack() { OverridesOverDur = 0f; }
        // Variables
        private float OVERRIDESOVERDUR;
        // Get/Set
        public float OverridesOverDur { get { return OVERRIDESOVERDUR; } set { OVERRIDESOVERDUR = value; } }
        public virtual float FullFocusCost { get { return FocusCost /*+ Whiteattacks.MHSwingMana - Whiteattacks.MHUWProcValue * RWAtkTable.AnyLand*/; } }
        // Functions
        public override float Activates
        {
            get
            {
                if (!Validated || OverridesOverDur <= 0f) { return 0f; }
                //return Acts * (1f - Whiteattacks.AvoidanceStreak);
                return OverridesOverDur * (1f - Whiteattacks.FocusSlip(FightDuration / OverridesOverDur, FocusCost));
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
        protected float addMisses;
        // Get/Set
        public SpecialEffect Effect { get { return EFFECT; } set { EFFECT = value; } }
        public SpecialEffect Effect2 { get { return EFFECT2; } set { EFFECT2 = value; } }
        // Functions
        public virtual StatsHunter AverageStats
        {
            get
            {
                if (!Validated) { return new StatsHunter(); }
                StatsHunter bonus = (Effect == null) ? new StatsHunter() { AttackPower = 0f, } : Effect.GetAverageStats(0f, RWAtkTable.Hit + RWAtkTable.Crit, Whiteattacks.RwEffectiveSpeed, FightDuration) as StatsHunter;
                bonus.Accumulate((Effect2 == null) ? new StatsHunter() { AttackPower = 0f, } : Effect2.GetAverageStats(0f, RWAtkTable.Hit + RWAtkTable.Crit, Whiteattacks.RwEffectiveSpeed, FightDuration) as StatsHunter); 
                return bonus;
            }
        }
    }
}

