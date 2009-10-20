using System;

namespace Rawr.DPSWarr {
    public class CombatFactors {
        public CombatFactors(Character character, Stats stats, CalculationOptionsDPSWarr calcOpts) {
            Char = character;
            MH = Char == null || Char.MainHand == null ? new Knuckles() : Char.MainHand.Item;
            OH = Char == null || Char.OffHand  == null || Char.WarriorTalents.TitansGrip == 0 ? null : Char.OffHand.Item;
            Talents = Char == null || Char.WarriorTalents == null ? new WarriorTalents() : Char.WarriorTalents;
            CalcOpts = (calcOpts == null ? new CalculationOptionsDPSWarr() : calcOpts);
            StatS = stats;
            
            // Optimizations
            
            //Set_c_values();
        }

        private void Set_c_values()
        {
            _c_mhItemType = MH.Type;
            _c_mhItemSpeed = MH.Speed;
            if (OH != null)
            {
                _c_ohItemType = OH.Type;
                _c_ohItemSpeed = OH.Speed;
            }
            useMH = _useMH; // public variable gets set once
            useOH = _useOH;

            _c_mhRacialExpertise = GetRacialExpertiseFromWeaponType(_c_mhItemType);
            _c_mhexpertise = StatS.Expertise + StatConversion.GetExpertiseFromRating(StatS.ExpertiseRating) + _c_mhRacialExpertise;
            _c_ymiss = YwMissChance;
            _c_wmiss = WhMissChance;
            _c_mhdodge = MhDodgeChance;
            _c_mhparry = MhParryChance;
            _c_mhblock = MhBlockChance;
            _c_mhwcrit = MhWhCritChance;
            _c_mhycrit = MhYwCritChance;
            _c_glance = GlanceChance;
            if (useOH)
            {
                _c_ohRacialExpertise = GetRacialExpertiseFromWeaponType(_c_ohItemType);
                _c_ohexpertise = StatS.Expertise + StatConversion.GetExpertiseFromRating(StatS.ExpertiseRating) + _c_ohRacialExpertise;
                _c_ohdodge = OhDodgeChance;
                _c_ohparry = OhParryChance;
                _c_ohblock = OhBlockChance;
                _c_ohwcrit = OhWhCritChance;
                _c_ohycrit = OhYwCritChance;
            }
            else
            {
                _c_ohItemType = ItemType.None;
                _c_ohItemSpeed = 0f;
                _c_ohRacialExpertise = 0f;
                _c_ohexpertise = 0f;
                _c_ohdodge = StatConversion.WHITE_DODGE_CHANCE_CAP[CalcOpts.TargetLevel - Char.Level];
                _c_ohparry = StatConversion.WHITE_PARRY_CHANCE_CAP[CalcOpts.TargetLevel - Char.Level];
                _c_ohblock = 0.0f;
                _c_ohwcrit = 0.0f;
                _c_ohycrit = 0.0f;
            }
        }
        #region Global Variables
        private Stats _Stats;
        public Stats StatS { get { return _Stats; } set { _Stats = value; InvalidateCache(); } }
        private WarriorTalents Talents;
        public CalculationOptionsDPSWarr CalcOpts { get; private set; }
        public Character Char { get; private set; }
        public Item MH { get; private set; }
        public Item OH { get; private set; }
        // Optimizations
        public float _c_ymiss { get; private set; }
        public float _c_wmiss { get; private set; }
        
        public ItemType _c_mhItemType { get; private set; }
        public ItemType _c_ohItemType { get; private set; }

        public float _c_mhItemSpeed { get; private set; }
        public float _c_ohItemSpeed { get; private set; }
        public float _c_mhRacialExpertise { get; private set; }
        public float _c_ohRacialExpertise { get; private set; }
        public float _c_mhexpertise { get; private set; }
        public float _c_ohexpertise { get; private set; }
        public float _c_mhdodge { get; private set; }
        public float _c_ohdodge { get; private set; }
        public float _c_mhparry { get; private set; }
        public float _c_ohparry { get; private set; }
        public float _c_mhblock { get; private set; }
        public float _c_ohblock { get; private set; }
        public float _c_mhwcrit { get; private set; }
        public float _c_ohwcrit { get; private set; }
        public float _c_mhycrit { get; private set; }
        public float _c_ohycrit { get; private set; }
        public float _c_glance { get; private set; }
        #endregion

        public bool useMH; private bool _useMH { get { return MH != null && _c_mhItemSpeed > 0; } }
        public bool useOH; private bool _useOH { get { return Talents.TitansGrip > 0 && OH != null && _c_ohItemSpeed > 0; } }

        private void InvalidateCache() {
            _DamageBonus = _DamageReduction = _BonusWhiteCritDmg = _MHSpeed = _OHSpeed = _TotalHaste = -1f;
            Set_c_values();
        }

        #region Weapon Damage Calcs
        #region Major Damage Factors
        
        private float _DamageBonus = -1f;
        public float DamageBonus {
            get {
                               // General Bonuses
                if (_DamageBonus == -1f) {
                    _DamageBonus = (1f + StatS.BonusDamageMultiplier)
                                 * (1f + StatS.BonusPhysicalDamageMultiplier);
                    // Talents
                    //bonus *= 1f + Talents.WreckingCrew * 0.02f; // Wrecking Crew is now a SpecialEffect in GetCharacterStats
                }
                return _DamageBonus;
            }
        }

        private float _DamageReduction = -1f;
        public float DamageReduction {
            get {
                if (_DamageReduction == -1f) {
                    float arpenBuffs =
                        ((_c_mhItemType == ItemType.TwoHandMace) ? Talents.MaceSpecialization * 0.03f : 0.00f) +
                        (!CalcOpts.FuryStance ? (0.10f + StatS.BonusWarrior_T9_2P_ArP) : 0.0f);

                    //return Math.Max(0f, 1f - StatConversion.GetArmorDamageReduction(Char.Level,CalcOpts.TargetArmor,StatS.ArmorPenetration,arpenBuffs,StatS.ArmorPenetrationRating));
                    _DamageReduction = Math.Max(0f, 1f - StatConversion.GetArmorDamageReduction(Char.Level, CalcOpts.TargetArmor, StatS.ArmorPenetration, arpenBuffs, StatS.ArmorPenetrationRating));
                }
                return _DamageReduction;
            }
        }
        public float HealthBonus {
            get {
                return 1f + StatS.BonusHealingReceived;
            }
        }
        #endregion
        #region Weapon Damage
        public float OHDamageReduc { get { return 0.5f * (1f + Talents.DualWieldSpecialization * 0.05f); } }
        public float NormalizedMhWeaponDmg { get { return useMH ? CalcNormalizedWeaponDamage(MH)                 : 0f; } }
        public float NormalizedOhWeaponDmg { get { return useOH ? CalcNormalizedWeaponDamage(OH) * OHDamageReduc : 0f; } }
        private float CalcNormalizedWeaponDamage(Item weapon) {
            return weapon.Speed * weapon.DPS + StatS.AttackPower / 14f * 3.3f + StatS.WeaponDamage;
        }
        public float AvgMhWeaponDmgUnhasted              { get { return (useMH ? (StatS.AttackPower / 14f + MH.DPS) * _c_mhItemSpeed                 + StatS.WeaponDamage : 0f); } }
        public float AvgOhWeaponDmgUnhasted              { get { return (useOH ? (StatS.AttackPower / 14f + OH.DPS) * _c_ohItemSpeed * OHDamageReduc + StatS.WeaponDamage : 0f); } }
        /*public float AvgMhWeaponDmg(        float speed) {       return (useMH ? (StatS.AttackPower / 14f + MH.DPS) * speed                    + StatS.WeaponDamage : 0f); }
        public float AvgOhWeaponDmg(        float speed) {       return (useOH ? (StatS.AttackPower / 14f + OH.DPS) * speed    * OHDamageReduc + StatS.WeaponDamage : 0f); }*/
        #endregion
        #region Weapon Crit Damage
        private float _BonusWhiteCritDmg = -1f;
        public float BonusWhiteCritDmg {
            get {
                if (_BonusWhiteCritDmg == -1f) {
                    _BonusWhiteCritDmg = (2f * (1f + StatS.BonusCritMultiplier) - 1f)* 
                        (1f + ((_c_mhItemType == ItemType.TwoHandAxe || _c_mhItemType == ItemType.Polearm) ? 0.01f * Talents.PoleaxeSpecialization : 0f));
                }
                return _BonusWhiteCritDmg;
            }
        }
        public float BonusYellowCritDmg { get { return BonusWhiteCritDmg * (1f + Talents.Impale * 0.1f); } }
        #endregion
        #region Weapon Blocked Damage
        public float ReducWhBlockedDmg {
            get {
                return 0.70f;// 70% damage
            }
        }
        public float ReducYwBlockedDmg { get { return ReducWhBlockedDmg; } }
        #endregion
        #region Weapon Glanced Damage
        public float ReducWhGlancedDmg {
            get {
                return 0.70f;
            }
        }
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
        private float _MHSpeed = -1f;
        private float _OHSpeed = -1f;
        public float MHSpeed { get { 
            if (_MHSpeed == -1f)
                _MHSpeed = useMH ? _c_mhItemSpeed / TotalHaste : 0f;
            return _MHSpeed;
            }
        }
        public float OHSpeed { get { 
            if (_OHSpeed == -1f)
                _OHSpeed = useOH ? _c_ohItemSpeed / TotalHaste : 0f;
            return _OHSpeed;
            }
        }
        #endregion
        #endregion
        #region Attack Table
        #region Hit Rating
        public float HitPerc { get { return StatConversion.GetHitFromRating(StatS.HitRating, CharacterClass.Warrior); } }
        #endregion
        #region Expertise Rating
        /*private float GetDPRfromExp(float Expertise) {return StatConversion.GetDodgeParryReducFromExpertise(Expertise, CharacterClass.Warrior);}*/
        private float GetRacialExpertiseFromWeaponType(ItemType weapon) {
            CharacterRace r = Char.Race;
            if (weapon != ItemType.None) {
                if (r == CharacterRace.Human) {
                    if (weapon == ItemType.OneHandSword || weapon == ItemType.OneHandMace
                        || weapon == ItemType.TwoHandSword || weapon == ItemType.TwoHandMace)
                    {
                        return 3f;
                    }
                } else if (r == CharacterRace.Dwarf) {
                    if (weapon == ItemType.OneHandMace || weapon == ItemType.TwoHandMace) {
                        return 5f;
                    }
                } else if (r == CharacterRace.Orc) {
                    if (weapon == ItemType.OneHandAxe || weapon == ItemType.TwoHandAxe) {
                        return 5f;
                    }
                }
            }
            return 0f;
        }
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
                        && MH.Slot == ItemSlot.TwoHand
                        && OH.Slot == ItemSlot.TwoHand ?
                       StatConversion.WHITE_MISS_CHANCE_CAP_DW[CalcOpts.TargetLevel - Char.Level] : StatConversion.WHITE_MISS_CHANCE_CAP[CalcOpts.TargetLevel - Char.Level]);
            }
        }
        private float WhMissChance {
            get {
                return Math.Max(0f, WhMissCap - MissPrevBonuses); 
            }
        }
        private float YwMissCap {get {return StatConversion.YELLOW_MISS_CHANCE_CAP[CalcOpts.TargetLevel - Char.Level];}}
        private float YwMissChance { get { return Math.Max(0f, YwMissCap - MissPrevBonuses); } }
        #endregion
        #region Dodge
        private float DodgeChanceCap { get { return StatConversion.WHITE_DODGE_CHANCE_CAP[CalcOpts.TargetLevel - 80]; } }
        private float MhDodgeChance { get { return Math.Max(0f, DodgeChanceCap - StatConversion.GetDodgeParryReducFromExpertise(_c_mhexpertise, CharacterClass.Warrior) - Talents.WeaponMastery * 0.01f); } }
        private float OhDodgeChance { get { return Math.Max(0f, DodgeChanceCap - StatConversion.GetDodgeParryReducFromExpertise(_c_ohexpertise, CharacterClass.Warrior) - Talents.WeaponMastery * 0.01f); } }
        #endregion
        #region Parry
        private float ParryChanceCap { get { return StatConversion.WHITE_PARRY_CHANCE_CAP[CalcOpts.TargetLevel - 80]; } }
        private float MhParryChance {
            get {
                float ParryChance = ParryChanceCap - StatConversion.GetDodgeParryReducFromExpertise(_c_mhexpertise, CharacterClass.Warrior);
                return Math.Max(0f, CalcOpts.InBack ? ParryChance * (1f - CalcOpts.InBackPerc / 100f) : ParryChance);
            }
        }
        private float OhParryChance {
            get {
                float ParryChance = ParryChanceCap - StatConversion.GetDodgeParryReducFromExpertise(_c_ohexpertise, CharacterClass.Warrior);
                return Math.Max(0f, CalcOpts.InBack ? ParryChance * (1f - CalcOpts.InBackPerc / 100f) : ParryChance);
            }
        }
        #endregion
        #region Glance
        private float GlanceChance { get { return StatConversion.WHITE_GLANCE_CHANCE_CAP[CalcOpts.TargetLevel-80]; } }
        #endregion
        #region Block
        // DPSWarr Dev Team has decided to remove Block from the Attack Table
        // until evidence can show specific bosses that do block
        private float BlockChanceCap { get { return 0f/*StatConversion.WHITE_BLOCK_CHANCE_CAP[CalcOpts.TargetLevel - Char.Level]*/; } }
        private float MhBlockChance { get { return Math.Max(0f, CalcOpts.InBack ? BlockChanceCap * (1f - CalcOpts.InBackPerc / 100f) : BlockChanceCap); } }
        private float OhBlockChance { get { return Math.Max(0f, CalcOpts.InBack ? BlockChanceCap * (1f - CalcOpts.InBackPerc / 100f) : BlockChanceCap); } }
        #endregion
        #region Crit
        private float MhWhCritChance {
            get {
                if (!useMH) { return 0f; }
                return StatS.PhysicalCrit + StatConversion.GetCritFromRating(StatS.CritRating) +
                 ((_c_mhItemType == ItemType.TwoHandAxe || _c_mhItemType == ItemType.Polearm) ? 0.01f * Talents.PoleaxeSpecialization : 0f);
                //return crit;
            }
        }
        private float MhYwCritChance {
            get {
                if (!useMH) { return 0f; }
                return ((StatS.PhysicalCrit + StatConversion.GetCritFromRating(StatS.CritRating)) +
                       ((_c_mhItemType == ItemType.TwoHandAxe || _c_mhItemType == ItemType.Polearm) ? 0.01f * Talents.PoleaxeSpecialization : 0f))
                        * (1f - _c_ymiss - _c_mhdodge);
            }
        }
        private float OhWhCritChance {
            get {
                if (!useOH) { return 0f; }
                return StatS.PhysicalCrit + StatConversion.GetCritFromRating(StatS.CritRating) +
                ((_c_ohItemType == ItemType.TwoHandAxe || _c_ohItemType == ItemType.Polearm) ? 0.01f * Talents.PoleaxeSpecialization : 0f);
            }
        }
        private float OhYwCritChance {
            get {
                if (!useOH) { return 0f; }
                return ((StatS.PhysicalCrit + StatConversion.GetCritFromRating(StatS.CritRating)) +
                ((_c_ohItemType == ItemType.TwoHandAxe || _c_ohItemType == ItemType.Polearm) ? 0.01f * Talents.PoleaxeSpecialization : 0f))
                * (1f - _c_ymiss - _c_ohdodge);
            }
        }
        #endregion
        #region Chance of Hitting
        // White
        private float ProbMhWhiteHit   { get { return         1f - _c_wmiss - _c_mhdodge - _c_mhparry - _c_mhwcrit; } }
        private float ProbOhWhiteHit   { get { return useOH ? 1f - _c_wmiss - _c_ohdodge - _c_ohparry - _c_ohwcrit : 0; } }
        private float ProbMhWhiteLand  { get { return         1f - _c_wmiss - _c_mhdodge - _c_mhparry; } }
        private float ProbOhWhiteLand  { get { return useOH ? 1f - _c_wmiss - _c_ohdodge - _c_ohparry : 0; } }
        // Yellow (Doesn't Glance and has different MissChance Cap)
        private float ProbMhYellowHit  { get { return         1f - _c_ymiss - _c_mhdodge - _c_mhparry - _c_mhblock - _c_mhycrit; } }
        private float ProbOhYellowHit  { get { return useOH ? 1f - _c_ymiss - _c_ohdodge - _c_ohparry - _c_ohblock - _c_ohycrit : 0; } }
        private float ProbMhYellowLand { get { return         1f - _c_ymiss - _c_mhdodge - _c_mhparry - _c_mhblock; } }
        private float ProbOhYellowLand { get { return useOH ? 1f - _c_ymiss - _c_ohdodge - _c_ohparry - _c_ohblock : 0; } }
        #endregion
        #endregion
        #region Other
        /*private float FlurryUptime {
            get {
                float uptime = 1f - (1f - _c_mhycrit) * (1f - _c_mhycrit) * (1f - _c_mhycrit);
                return uptime;
            }
        }*/
        #endregion
        private class Knuckles : Item {
            public Knuckles() {
                Speed = 0f;
                MaxDamage = 0;
                MinDamage = 0;
            }
        }
        #region Attackers Stats against you
        private float LevelModifier { get { return (CalcOpts.TargetLevel - Char.Level) * 0.002f; } }
        private float NPC_CritChance
        {
            get
            {
                return Math.Max(0f, 0.05f + LevelModifier
                                        - StatConversion.GetDRAvoidanceChance(Char, StatS, HitResult.Crit, CalcOpts.TargetLevel)
                                );
            }
        }
        #endregion
    }

    public abstract class CombatTable {
        protected Character Char;
        protected CalculationOptionsDPSWarr calcOpts;
        protected CombatFactors combatFactors;
        protected Stats StatS;
        protected Skills.Ability Abil;
        protected bool useSpellHit = false;

        public bool isWhite;
        public bool isMH;
        
        public float Miss { get; protected set; }
        public float Dodge { get; protected set; }
        public float Parry { get; protected set; }
        public float Block { get; protected set; }
        public float Glance { get; protected set; }
        public float Crit { get; protected set; }
        public float Hit { get; protected set; }

        private float _anyLand = 0f;
        private float _anyNotLand = 1f;
        public float AnyLand { get { return _anyLand; } }
        public float AnyNotLand { get { return _anyNotLand; } }

        protected virtual void Calculate() {
            _anyNotLand = Dodge + Parry + Miss;
            _anyLand = 1f - _anyNotLand;
        }
        protected virtual void CalculateAlwaysHit()
        {
            Miss = Dodge = Parry = Block = Glance = Crit = 0f;
            Hit = 1f;
            _anyLand = 1f;
            _anyNotLand = 0f;
        }

        protected void Initialize(Character character, Stats stats, CombatFactors cf, CalculationOptionsDPSWarr co, Skills.Ability ability, bool ismh, bool useSpellHit, bool alwaysHit) {
            Char = character;
            StatS = stats;
            calcOpts = co;
            combatFactors = cf;
            Abil = ability;
            isWhite = (Abil == null);
            isMH = ismh;
            this.useSpellHit = useSpellHit;
            /*// Defaults
            Miss 
            Dodge
            Parry
            Block
            Glance
            Critical
            Hit*/
            // Start a calc            
            if (alwaysHit) CalculateAlwaysHit();
            else Calculate();            
        }
    }

    /*public class DefendTable : CombatTable {
        protected override void Calculate() {
            float tableSize = 0f;
            float tempVal = 0f;

            // Miss
            tempVal = StatConversion.GetDRAvoidanceChance(Char, StatS, HitResult.Miss, calcOpts.TargetLevel);
            Miss = Math.Max(0f, Math.Min(1f - tableSize, tempVal));
            tableSize += Miss;
            // Dodge
            tempVal = StatConversion.GetDRAvoidanceChance(Char, StatS, HitResult.Dodge, calcOpts.TargetLevel);
            Dodge = Math.Max(0f, Math.Min(1f - tableSize, tempVal));
            tableSize += Dodge;
            // Parry
            tempVal = StatConversion.GetDRAvoidanceChance(Char, StatS, HitResult.Parry, calcOpts.TargetLevel);
            Parry = Math.Max(0f, Math.Min(1f - tableSize, tempVal));
            tableSize += Parry;
            // Block
            if (combatFactors.OH != null && combatFactors.OH.Type == ItemType.Shield) {
                tempVal = StatConversion.GetDRAvoidanceChance(Char, StatS, HitResult.Block, calcOpts.TargetLevel);
                Block = Math.Max(0f, Math.Min(1f - tableSize, tempVal));
                tableSize += Block;
            }
            // Critical Hit
            Crit = Math.Max(0f, Math.Min(1f - tableSize, combatFactors.NPC_CritChance()));
            tableSize += Crit;
            // Normal Hit
            Hit = Math.Max(0f, 1f - tableSize);

            base.Calculate();
        }

        public DefendTable(Character character, Stats stats, CombatFactors cf, CalculationOptionsDPSWarr co) { Initialize(character, stats, cf, co, null, true, useSpellHit, false); }
    }*/

    public class AttackTable : CombatTable {
        protected override void Calculate() {
            float tableSize = 0f;

            // Miss
            if (useSpellHit) {
                //float hitIncrease = StatConversion.GetHitFromRating(StatS.HitRating, Char.Class) + StatS.SpellHit;
                Miss = Math.Min(1f - tableSize, Math.Max(0.17f - (StatConversion.GetHitFromRating(StatS.HitRating, Char.Class) + StatS.SpellHit), 0f));
            } else {
                Miss = Math.Min(1f - tableSize, isWhite ? combatFactors._c_wmiss : combatFactors._c_ymiss);
            }
            tableSize += Miss;
            // Dodge
            if (isWhite || Abil.CanBeDodged) {
                Dodge = Math.Min(1f - tableSize, isMH ? combatFactors._c_mhdodge : combatFactors._c_ohdodge);
                tableSize += Dodge;
            } else { Dodge = 0f; }
            // Parry
            if (isWhite || Abil.CanBeParried) {
                Parry = Math.Min(1f - tableSize, isMH ? combatFactors._c_mhparry : combatFactors._c_ohparry);
                tableSize += Parry;
            } else { Parry = 0f; }
            // Block
            if (isWhite || Abil.CanBeBlocked) {
                Block = Math.Min(1f - tableSize, isMH ?  combatFactors._c_mhblock : combatFactors._c_ohblock);
                tableSize += Block;
            } else { Block = 0f; }
            // Glancing Blow
            if (isWhite) {
                Glance = Math.Min(1f - tableSize, combatFactors._c_glance);
                tableSize += Glance;
            } else { Glance = 0f; }
            // Critical Hit
            if (isWhite) {
                Crit = Math.Min(1f - tableSize, isMH ?  combatFactors._c_mhycrit : combatFactors._c_ohycrit);
                tableSize += Crit;
            } else if (Abil.CanCrit) {
                Crit = Math.Min(1f - tableSize, (isMH ? Abil.ContainCritValue_MH : Abil.ContainCritValue_OH));
                tableSize += Crit;
            } else {
                Crit = 0f;
            }
            // Normal Hit
            Hit = Math.Max(0f, 1f - tableSize);
            base.Calculate();
        }

        public AttackTable(Character character, Stats stats, CombatFactors cf, CalculationOptionsDPSWarr co, bool ismh, bool useSpellHit, bool alwaysHit) {
        
            Initialize(character, stats, cf, co, null, ismh, useSpellHit, alwaysHit);
        }

        public AttackTable(Character character, Stats stats, CombatFactors cf, CalculationOptionsDPSWarr co, Skills.Ability ability, bool ismh, bool useSpellHit, bool alwaysHit) {
            Initialize(character, stats, cf, co, ability, ismh, useSpellHit, alwaysHit);
        }
    }
}
