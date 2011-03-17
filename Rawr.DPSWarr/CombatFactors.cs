using System;

namespace Rawr.DPSWarr {
    public class CombatFactors {
        public CombatFactors(Character character, Base.StatsWarrior stats, CalculationOptionsDPSWarr calcOpts, BossOptions bossOpts) {
            Char = character;
            MH = Char == null || Char.MainHand == null ? new Knuckles() : Char.MainHand.Item;
            OH = Char == null || Char.OffHand  == null || (Char.WarriorTalents.TitansGrip == 0 && Char.WarriorTalents.SingleMindedFury == 0) ? null : Char.OffHand.Item;
            Talents = Char == null || Char.WarriorTalents == null ? new WarriorTalents() : Char.WarriorTalents;
            CalcOpts = (calcOpts == null ? new CalculationOptionsDPSWarr() : calcOpts);
            BossOpts = (bossOpts == null ? new BossOptions() : bossOpts);
            StatS = stats;
            CritProcs = new WeightedStat[] { new WeightedStat() { Chance = 1f, Value = 0f } };
            InvalidateCache();
            // Optimizations
            
            //SetCvalues();
        }
        private bool? _FuryStance = null;
        public bool FuryStance {
            get {
                if (_FuryStance == null) {
                    if (Talents == null) { return false; } // wait till there is one
                    int armsCounter = 0, furyCounter = 0;
                    int[] talentData = Talents.Data;
                    for (int i =  0; i <= 19; i++) { armsCounter += talentData[i]; }
                    for (int i = 20; i <= 40; i++) { furyCounter += talentData[i]; }
                    if (armsCounter >= furyCounter) _FuryStance = false;
                    else if(armsCounter <  furyCounter) _FuryStance = true;
                }
                return (bool)_FuryStance;
            }
        }
        private WeightedStat[] _critProcs;
        public WeightedStat[] CritProcs { get { return _critProcs; } set { _critProcs = value; } }
        private void SetCvalues()
        {
            CMHItemType = MH.Type;
            CMHItemSpeed = MH.Speed;
            if (OH != null)
            {
                COHItemType = OH.Type;
                COHItemSpeed = OH.Speed;
            }
            useMH = _useMH; // public variable gets set once
            useOH = _useOH;

            CMHRacialExpertise = BaseStats.GetRacialExpertise(Char, ItemSlot.MainHand); //GetRacialExpertiseFromWeaponType(CmhItemType);
            CMHexpertise = StatS.Expertise + StatConversion.GetExpertiseFromRating(Math.Max(0, StatS.ExpertiseRating)) + CMHRacialExpertise;
            CYmiss = YwMissChance;
            CWmiss = WhMissChance;
            CMHdodge = MhDodgeChance;
            CMHparry = MhParryChance;
            CMHblock = MhBlockChance;
            CMHwcrit = MhWhCritChance;
            CMHycrit = MhYwCritChance;
            CGlance = GlanceChance;
            if (useOH)
            {
                COHRacialExpertise = BaseStats.GetRacialExpertise(Char, ItemSlot.OffHand);// GetRacialExpertiseFromWeaponType(CohItemType);
                COhexpertise = StatS.Expertise + StatConversion.GetExpertiseFromRating(Math.Max(0, StatS.ExpertiseRating)) + COHRacialExpertise;
                COhdodge = OhDodgeChance;
                COhparry = OhParryChance;
                COhblock = OhBlockChance;
                COhwcrit = OhWhCritChance;
                COhycrit = OhYwCritChance;
            }
            else
            {
                COHItemType = ItemType.None;
                COHItemSpeed = 0f;
                COHRacialExpertise = 0f;
                COhexpertise = 0f;
                COhdodge = StatConversion.WHITE_DODGE_CHANCE_CAP[LevelDif];
                COhparry = StatConversion.WHITE_PARRY_CHANCE_CAP[LevelDif];
                COhblock = 0.0f;
                COhwcrit = 0.0f;
                COhycrit = 0.0f;
            }
        }
        #region Global Variables
        public Base.StatsWarrior StatS { get; set; }
        private WarriorTalents Talents;
        public CalculationOptionsDPSWarr CalcOpts { get; private set; }
        public BossOptions BossOpts { get; private set; }
        public Character Char { get; private set; }
        public Item MH { get; private set; }
        public Item OH { get; private set; }
        // Optimizations
        public float CYmiss { get; private set; }
        public float CWmiss { get; private set; }
        
        public ItemType CMHItemType { get; private set; }
        public ItemType COHItemType { get; private set; }

        public float CMHItemSpeed { get; private set; }
        public float COHItemSpeed { get; private set; }
        public float CMHRacialExpertise { get; private set; }
        public float COHRacialExpertise { get; private set; }
        public float CMHexpertise { get; private set; }
        public float COhexpertise { get; private set; }
        public float CMHdodge { get; private set; }
        public float COhdodge { get; private set; }
        public float CMHparry { get; private set; }
        public float COhparry { get; private set; }
        public float CMHblock { get; private set; }
        public float COhblock { get; private set; }
        public float CMHwcrit { get; private set; }
        public float COhwcrit { get; private set; }
        public float CMHycrit { get; private set; }
        public float COhycrit { get; private set; }
        public float CGlance { get; private set; }

        public int LevelDif { get { return BossOpts.Level - Char.Level; } }
        #endregion

        public bool useMH; private bool _useMH { get { return MH != null && CMHItemSpeed > 0; } }
        public bool useOH; private bool _useOH { get { return (Talents.TitansGrip > 0 || Talents.SingleMindedFury > 0) && OH != null && COHItemSpeed > 0; } }

        public void InvalidateCache() {
            _DamageBonus = _DamageReduction = _BonusWhiteCritDmg = _MHSpeedHasted = _OHSpeedHasted = _TotalHaste = -1f;
            _AttackTableBasicMH = _AttackTableBasicOH = null;
            SetCvalues();
        }

        #region Weapon Damage Calcs
        #region Major Damage Factors
        private float _DamageBonus = -1f, _WhiteDamageBonus = -1f, _DamageReduction = -1f, _HealthBonus = -1f;
        /// <summary>A Percentage Value. 1.00 = No Modifier. 1.25 = +25% Modifier</summary>
        public float DamageBonus {
            get {
                // General Bonuses
                if (_DamageBonus == -1f) {
                    _DamageBonus = (1f + StatS.BonusDamageMultiplier)
                                 * (1f + StatS.BonusPhysicalDamageMultiplier);
                }
                return _DamageBonus;
            }
        }
        public float WhiteDamageBonus
        {
            get
            {
                // White Damage Bonuses
                if (_WhiteDamageBonus == -1f)
                {
                    _WhiteDamageBonus = (1f + StatS.BonusWhiteDamageMultiplier);
                }
                return _WhiteDamageBonus;
            }
        }
        public float DamageReduction {
            get {
                if (_DamageReduction == -1f) {
                    float arpenBuffs = StatS.ArmorPenetration;

                    _DamageReduction = Math.Max(0f, 1f - StatConversion.GetArmorDamageReduction(Char.Level,
                        BossOpts.Armor,
                        StatS.TargetArmorReduction, arpenBuffs));
                }
                return _DamageReduction;
            }
        }
        public float HealthBonus {
            get {
                // General Bonuses
                if (_HealthBonus == -1f)
                {
                    _HealthBonus = (1f + StatS.BonusHealingReceived);
                }
                return _HealthBonus;
            }
        }
        #endregion
        #region Weapon Damage
        public float OHDamageMod { get { return 0.50f + (FuryStance ? 0.25f : 0f); } }
        public float NormalizedMHWeaponDmg { get { return useMH ? CalcNormalizedWeaponDamage(MH) : 0f; } }
        public float NormalizedOHWeaponDmg { get { return useOH ? CalcNormalizedWeaponDamage(OH) * OHDamageMod : 0f; } }
        private float CalcNormalizedWeaponDamage(Item weapon) { return weapon.Speed * weapon.DPS + StatS.AttackPower / 14f * 3.3f + StatS.WeaponDamage; }
        public float AvgMHWeaponDmgUnhasted              { get { return (useMH ? (StatS.AttackPower / 14f + MH.DPS) * CMHItemSpeed               + StatS.WeaponDamage : 0f); } }
        public float AvgOHWeaponDmgUnhasted              { get { return (useOH ? (StatS.AttackPower / 14f + OH.DPS) * COHItemSpeed * OHDamageMod + StatS.WeaponDamage : 0f); } }
        //public float AvgMhWeaponDmg(        float speed) {       return (useMH ? (StatS.AttackPower / 14f + MH.DPS) * speed                  + StatS.WeaponDamage : 0f); }
        //public float AvgOhWeaponDmg(        float speed) {       return (useOH ? (StatS.AttackPower / 14f + OH.DPS) * speed    * OHDamageMOD + StatS.WeaponDamage : 0f); }
        #endregion
        #region Weapon Crit Damage
        private float _BonusWhiteCritDmg = -1f;
        public float BonusWhiteCritDmg {
            get {
                if (_BonusWhiteCritDmg == -1f) {
                    _BonusWhiteCritDmg = (2f * (1f + StatS.BonusCritMultiplier) - 1f) * (1f /*+ ((CmhItemType == ItemType.TwoHandAxe || CmhItemType == ItemType.Polearm) ? 0.01f * Talents.PoleaxeSpecialization : 0f)*/);
                }
                return _BonusWhiteCritDmg;
            }
        }
        public float BonusYellowCritDmg { get { return BonusWhiteCritDmg; } } // Cata: Impale only affects MS, SL, OP(TFB)
        #endregion
        #region Weapon Blocked Damage
        public const float ReducWHBlockedDmg = 0.70f; // 70% damage
        public const float ReducYWBlockedDmg = 0.70f; // 70% damage
        #endregion
        #region Weapon Glanced Damage
        public const float ReducWHGlancedDmg = 0.70f;
        #endregion
        #region Speed
        private float _TotalHaste = -1f;
        public float TotalHaste {
            get {
                if (_TotalHaste == -1f)
                    _TotalHaste = 1f + StatS.PhysicalHaste; // All haste is calc'd into PhysicalHaste in GetCharacterStats
                //totalHaste      *= 1f + Talents.Flurry * 0.05f * FlurryUptime;
                return _TotalHaste;
            }
        }
        private float _MHSpeedHasted = -1f;
        private float _OHSpeedHasted = -1f;
        public float MHSpeedHasted { get { 
            if (_MHSpeedHasted == -1f)
                _MHSpeedHasted = useMH ? CMHItemSpeed / TotalHaste : 0f;
            return _MHSpeedHasted;
            }
        }
        public float OHSpeedHasted { get { 
            if (_OHSpeedHasted == -1f)
                _OHSpeedHasted = useOH ? COHItemSpeed / TotalHaste : 0f;
            return _OHSpeedHasted;
            }
        }
        #endregion
        #endregion
        #region Attack Table
        private AttackTable _AttackTableBasicMH, _AttackTableBasicOH;
        public AttackTable AttackTableBasicMH
        {
            get
            {
                if (_AttackTableBasicMH == null)
                {
                    _AttackTableBasicMH = new AttackTable(Char, StatS, this, CalcOpts, BossOpts, Skills.Ability.NULL, true, false, false, false);
                }
                return _AttackTableBasicMH;
            }
        }
        public AttackTable AttackTableBasicOH
        {
            get
            {
                if (_AttackTableBasicOH == null)
                {
                    _AttackTableBasicOH = new AttackTable(Char, StatS, this, CalcOpts, BossOpts, Skills.Ability.NULL, false, false, false, false);
                }
                return _AttackTableBasicOH;
            }
        }
        #region Hit Rating
        public float HitPerc { get { return StatConversion.GetHitFromRating(Math.Max(0, StatS.HitRating), CharacterClass.Warrior); } }
        #endregion
        #region Expertise Rating
        #endregion

        #region Miss
        private float MissPrevBonuses {
            get {
                return StatS.PhysicalHit    // Hit Perc bonuses like Draenei Racial
                        + HitPerc;          // Bonus from Hit Rating
            }
        }
        private float WhMissCap {
            get {
                return (useOH
                        && ((MH.Slot == ItemSlot.TwoHand
                        && OH.Slot == ItemSlot.TwoHand) || (MH.Slot == ItemSlot.OneHand && (OH.Slot == ItemSlot.OneHand || OH.Slot == ItemSlot.OffHand)))?
                       StatConversion.WHITE_MISS_CHANCE_CAP_DW[LevelDif] : StatConversion.WHITE_MISS_CHANCE_CAP[LevelDif]);
            }
        }
        private float WhMissChance {
            get {
                return Math.Max(0f, WhMissCap - MissPrevBonuses); 
            }
        }
        private float YwMissCap { get { return StatConversion.YELLOW_MISS_CHANCE_CAP[LevelDif]; } }
        private float YwMissChance { get { return Math.Max(0f, YwMissCap - MissPrevBonuses); } }
        #endregion
        #region Dodge
        private float DodgeChanceCap { get { return StatConversion.WHITE_DODGE_CHANCE_CAP[LevelDif]; } }
        private float MhDodgeChance { get { return Math.Max(0f, DodgeChanceCap - StatConversion.GetDodgeParryReducFromExpertise(CMHexpertise, CharacterClass.Warrior)); } }
        private float OhDodgeChance { get { return Math.Max(0f, DodgeChanceCap - StatConversion.GetDodgeParryReducFromExpertise(COhexpertise, CharacterClass.Warrior)); } }
        #endregion
        #region Parry
        private float ParryChanceCap { get { return StatConversion.WHITE_PARRY_CHANCE_CAP[LevelDif]; } }
        private float MhParryChance {
            get {
                float ParryChance = ParryChanceCap - StatConversion.GetDodgeParryReducFromExpertise(CMHexpertise, CharacterClass.Warrior);
                return Math.Max(0f, BossOpts.InBack ? ParryChance * (1f - (float)BossOpts.InBackPerc_Melee/* / 100f*/) : ParryChance);
            }
        }
        private float OhParryChance {
            get {
                float ParryChance = ParryChanceCap - StatConversion.GetDodgeParryReducFromExpertise(COhexpertise, CharacterClass.Warrior);
                return Math.Max(0f, BossOpts.InBack ? ParryChance * (1f - (float)BossOpts.InBackPerc_Melee/* / 100f*/) : ParryChance);
            }
        }
        #endregion
        #region Glance
        private float GlanceChance { get { return StatConversion.WHITE_GLANCE_CHANCE_CAP[LevelDif]; } }
        #endregion
        #region Block
        // DPSWarr Dev Team has decided to remove Block from the Attack Table
        // until evidence can show specific bosses that do block
        // Congrats, on 2010.10.30, Jothay found evidence that bosses block now!
        private float BlockChanceCap { get { return StatConversion.WHITE_BLOCK_CHANCE_CAP[LevelDif]; } }
        private float MhBlockChance { get { return Math.Max(0f, BossOpts.InBack ? BlockChanceCap * (1f - (float)BossOpts.InBackPerc_Melee) : BlockChanceCap); } }
        private float OhBlockChance { get { return Math.Max(0f, BossOpts.InBack ? BlockChanceCap * (1f - (float)BossOpts.InBackPerc_Melee) : BlockChanceCap); } }
        #endregion
        #region Crit
        private float MhWhCritChance {
            get {
                if (!useMH) { return 0f; }
                return StatS.PhysicalCrit;// +((CmhItemType == ItemType.TwoHandAxe || CmhItemType == ItemType.Polearm) ? 0.01f * Talents.PoleaxeSpecialization : 0f);
            }
        }
        private float MhYwCritChance {
            get {
                if (!useMH) { return 0f; }
                return StatS.PhysicalCrit;// +((CmhItemType == ItemType.TwoHandAxe || CmhItemType == ItemType.Polearm) ? 0.01f * Talents.PoleaxeSpecialization : 0f);
            }
        }
        private float OhWhCritChance {
            get {
                if (!useOH) { return 0f; }
                return StatS.PhysicalCrit;// +((CohItemType == ItemType.TwoHandAxe || CohItemType == ItemType.Polearm) ? 0.01f * Talents.PoleaxeSpecialization : 0f);
            }
        }
        private float OhYwCritChance {
            get {
                if (!useOH) { return 0f; }
                return StatS.PhysicalCrit;// +((CohItemType == ItemType.TwoHandAxe || CohItemType == ItemType.Polearm) ? 0.01f * Talents.PoleaxeSpecialization : 0f);
            }
        }
        #endregion
        #endregion
        #region Other
        private class Knuckles : Item
        {
            public Knuckles()
            {
                Speed = 0f;
                MaxDamage = 0;
                MinDamage = 0;
            }
        }
        #endregion
        #region Attackers Stats against you
        //private float LevelModifier { get { return levelDif * 0.002f; } }
        //private float NPC_CritChance { get { return Math.Max(0f, 0.05f + LevelModifier - StatConversion.GetDRAvoidanceChance(Char, StatS, HitResult.Crit, BossOpts.Level)); } }
        #endregion
    }
}
