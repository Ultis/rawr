using System;

namespace Rawr.DPSWarr {
    public class CombatFactors {
        public CombatFactors(Character character, Stats stats) {
            Char = character;
            StatS = stats;
            MH = Char == null || Char.MainHand == null ? new Knuckles() : Char.MainHand.Item;
            OH = Char == null || Char.OffHand  == null ? null           : Char.OffHand.Item;
            Talents = Char == null || Char.WarriorTalents == null ? new WarriorTalents() : Char.WarriorTalents;
            CalcOpts = Char == null || Char.CalculationOptions == null ? new CalculationOptionsDPSWarr() : Char.CalculationOptions as CalculationOptionsDPSWarr;
            
            // Optimizations
            _c_mhItemType = MH.Type;
            _c_mhItemSpeed = MH.Speed;
            _c_mhRacialExpertise = GetRacialExpertiseFromWeaponType(_c_mhItemType);
            _c_mhexpertise = StatS.Expertise + StatConversion.GetExpertiseFromRating(StatS.ExpertiseRating) + _c_mhRacialExpertise;
            _c_mhdodge = MhDodgeChance;
            _c_mhparry = MhParryChance;
            _c_mhblock = MhBlockChance;
            _c_mhwcrit = MhWhCritChance;
            _c_mhycrit = MhYwCritChance;

            if (OH != null) {
                _c_ohItemType = OH.Type;
                _c_ohItemSpeed = OH.Speed;
                _c_ohRacialExpertise = GetRacialExpertiseFromWeaponType(_c_ohItemType);
                _c_ohexpertise = StatS.Expertise + StatConversion.GetExpertiseFromRating(StatS.ExpertiseRating) + _c_ohRacialExpertise;
                _c_ohdodge = OhDodgeChance;
                _c_ohparry = OhParryChance;
                _c_ohblock = OhBlockChance;
                _c_ohwcrit = OhWhCritChance;
                _c_ohycrit = OhYwCritChance;
            } else {
                _c_ohItemType = ItemType.None;
                _c_ohItemSpeed = 0f;
                _c_ohRacialExpertise = 0f;
                _c_ohexpertise = 0f;
                _c_ohdodge = 0.065f;
                _c_ohparry = 0.120f;
                _c_ohblock = 0.0f;
                _c_ohwcrit = 0.0f;
                _c_ohycrit = 0.0f;
            }
            _c_ymiss = YwMissChance;
            _c_wmiss = WhMissChance;
        }
        #region Global Variables
        private Stats StatS;
        private WarriorTalents Talents;
        public CalculationOptionsDPSWarr CalcOpts;
        public Character Char;
        public Item MH;
        public Item OH;
        // Optimizations
        public readonly float    _c_ymiss, _c_wmiss;
        public readonly ItemType _c_mhItemType,        _c_ohItemType;
        public readonly float    _c_mhItemSpeed,       _c_ohItemSpeed;
        public readonly float    _c_mhRacialExpertise, _c_ohRacialExpertise;
        public readonly float    _c_mhexpertise,       _c_ohexpertise;
        public readonly float    _c_mhdodge,           _c_ohdodge;
        public readonly float    _c_mhparry,           _c_ohparry;
        public readonly float    _c_mhblock,           _c_ohblock;
        public readonly float    _c_mhwcrit,           _c_ohwcrit;
        public readonly float    _c_mhycrit,           _c_ohycrit;
        #endregion

        #region Weapon Damage Calcs
        #region Major Damage Factors
        public float DamageBonus {
            get {
                               // General Bonuses
                float bonus  = 1f + StatS.BonusDamageMultiplier;
                      bonus *= 1f + StatS.BonusPhysicalDamageMultiplier;
                               // Talents
                      bonus *= 1f + Talents.WreckingCrew * 0.02f;
                return bonus;
            }
        }
        public float DamageReduction {
            get {
                float armorReduction;
                float arpenBuffs =
                    ((_c_mhItemType == ItemType.TwoHandMace) ? Talents.MaceSpecialization * 0.03f : 0.00f) +
                    (!CalcOpts.FuryStance ? (0.10f + StatS.BonusWarrior_T9_2P_ArP) : 0.0f);
                if (CalcOpts == null) {
                    // you're supposed to pass the character level, not the target level.  GC misspoke.
				    armorReduction = Math.Max(0f, 1f - StatConversion.GetArmorDamageReduction(Char.Level,(int)StatConversion.NPC_ARMOR[83-80],StatS.ArmorPenetration,arpenBuffs,StatS.ArmorPenetrationRating)); // default is vs raid boss
                }else{
                    armorReduction = Math.Max(0f, 1f - StatConversion.GetArmorDamageReduction(Char.Level,CalcOpts.TargetArmor,StatS.ArmorPenetration,arpenBuffs,StatS.ArmorPenetrationRating));
                }

                return armorReduction;
            }
        }
        public float HealthBonus {
            get {
                float bonus = 1f + StatS.BonusHealingReceived;
                return bonus;
            }
        }
        #endregion
        #region Weapon Damage
        public float NormalizedMhWeaponDmg { get { return CalcNormalizedWeaponDamage(MH); } }
        public float NormalizedOhWeaponDmg { get { return CalcNormalizedWeaponDamage(OH); } }
        private float CalcNormalizedWeaponDamage(Item weapon) {
            float baseDamage  = weapon.Speed * weapon.DPS;
                  baseDamage += StatS.AttackPower / 14f * 3.3f;
            return baseDamage;
        }
        public float AvgMhWeaponDmgUnhasted              { get { return (MH == null ? 0f : (StatS.AttackPower / 14f + MH.DPS) * MH.Speed); } }
        public float AvgOhWeaponDmgUnhasted              { get { return (OH == null ? 0f : (StatS.AttackPower / 14f + OH.DPS) * OH.Speed * (0.5f + Talents.DualWieldSpecialization * 0.025f)); } }
        public float AvgMhWeaponDmg(        float speed) {       return (MH == null ? 0f : (StatS.AttackPower / 14f + MH.DPS) * speed   ); }
        public float AvgOhWeaponDmg(        float speed) {       return (OH == null ? 0f : (StatS.AttackPower / 14f + OH.DPS) * speed    * (0.5f + Talents.DualWieldSpecialization * 0.025f)); }
        #endregion
        #region Weapon Crit Damage
        public float BonusWhiteCritDmg {
            get {
                float baseCritDmg = (2f * (1f + StatS.BonusCritMultiplier) - 1f);
                baseCritDmg *= 1f + ((_c_mhItemType == ItemType.TwoHandAxe || _c_mhItemType == ItemType.Polearm) ? 0.01f * Talents.PoleaxeSpecialization : 0f);
                return baseCritDmg;
            }
        }
        public float BonusYellowCritDmg { get { return BonusWhiteCritDmg * (1f + Talents.Impale * 0.1f); } }
        #endregion
        #region Weapon Blocked Damage
        public float ReducWhBlockedDmg {
            get {
                float baseBlockedDmg = 0.70f;// 70% damage
                return baseBlockedDmg;
            }
        }
        public float ReducYwBlockedDmg { get { return ReducWhBlockedDmg; } }
        #endregion
        #region Weapon Glanced Damage
        public float ReducWhGlancedDmg {
            get {
                float baseGlancedDmg = 0.70f;// 70% damage
                return baseGlancedDmg;
            }
        }
        #endregion
        #region Speed
        public float TotalHaste {
            get {
                float totalHaste = 1f + StatS.PhysicalHaste; // All haste is calc'd into PhysicalHaste in GetCharacterStats
                totalHaste      *= 1f + Talents.Flurry * 0.05f * FlurryUptime;
                return totalHaste;
            }
        }
        public float MHSpeed { get { return (MH == null ? 0f : MH.Speed / TotalHaste); } }
        public float OHSpeed { get { return (OH == null ? 0f : OH.Speed / TotalHaste); } }
        #endregion
        #endregion
        #region Attack Table
        #region Hit Rating
        public float HitPerc { get { return StatConversion.GetHitFromRating(StatS.HitRating, CharacterClass.Warrior); } }
        #endregion
        #region Expertise Rating
        public float GetDPRfromExp(float Expertise) {return StatConversion.GetDodgeParryReducFromExpertise(Expertise, CharacterClass.Warrior);}
        public float GetRacialExpertiseFromWeaponType(ItemType weapon) {
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
        public float MissPrevBonuses {
            get {
                return StatS.PhysicalHit        // Hit Perc bonuses like Draenei Racial
                        + HitPerc;              // Bonus from Hit Rating
            }
        }
        public float WhMissCap {
            get {
                float twoHandCheck = (Talents.TitansGrip == 1f && OH != null
                        && MH.Slot == ItemSlot.TwoHand
                        && OH.Slot == ItemSlot.TwoHand ?
                       StatConversion.WHITE_MISS_CHANCE_CAP_DW[CalcOpts.TargetLevel - Char.Level] : StatConversion.WHITE_MISS_CHANCE_CAP[CalcOpts.TargetLevel - Char.Level]);
                return twoHandCheck;
            }
        }
        public float WhMissChance {
            get {
                float missChance = WhMissCap - MissPrevBonuses;
                return (float)Math.Max(0f, missChance); 
            }
        }
        public float YwMissCap {get {return StatConversion.YELLOW_MISS_CHANCE_CAP[CalcOpts.TargetLevel - Char.Level];}}
        public float YwMissChance { get { return (float)Math.Max(0f, YwMissCap - MissPrevBonuses); } }
        #endregion
        #region Dodge
        public float DodgeChanceCap { get { return StatConversion.WHITE_DODGE_CHANCE_CAP[CalcOpts.TargetLevel - 80]; } }
        private float MhDodgeChance { get { return (float)Math.Max(0f, DodgeChanceCap - GetDPRfromExp(_c_mhexpertise) - Talents.WeaponMastery * 0.01f); } }
        private float OhDodgeChance { get { return (float)Math.Max(0f, DodgeChanceCap - GetDPRfromExp(_c_ohexpertise) - Talents.WeaponMastery * 0.01f); } }
        #endregion
        #region Parry
        public float ParryChanceCap { get { return StatConversion.WHITE_PARRY_CHANCE_CAP[CalcOpts.TargetLevel - 80]; } }
        private float MhParryChance {
            get {
                float ParryChance = ParryChanceCap - GetDPRfromExp(_c_mhexpertise);
                return (float)Math.Max(0f, CalcOpts.InBack ? ParryChance * (1f - CalcOpts.InBackPerc / 100f) : ParryChance);
            }
        }
        private float OhParryChance {
            get {
                float ParryChance = ParryChanceCap - GetDPRfromExp(_c_ohexpertise);
                return (float)Math.Max(0f, CalcOpts.InBack ? ParryChance * (1f - CalcOpts.InBackPerc / 100f) : ParryChance);
            }
        }
        #endregion
        #region Glance
        public float GlanceChance { get { return StatConversion.WHITE_GLANCE_CHANCE_CAP[CalcOpts.TargetLevel-80]; } }
        #endregion
        #region Block
        // DPSWarr Dev Team has decided to remove Block from the Attack Table
        // until evidence can show specific bosses that do block
        public float BlockChanceCap { get { return 0f/*StatConversion.WHITE_BLOCK_CHANCE_CAP[CalcOpts.TargetLevel - Char.Level]*/; } }
        private float MhBlockChance { get { return (float)Math.Max(0f, CalcOpts.InBack ? BlockChanceCap * (1f - CalcOpts.InBackPerc / 100f) : BlockChanceCap); } }
        private float OhBlockChance { get { return (float)Math.Max(0f, CalcOpts.InBack ? BlockChanceCap * (1f - CalcOpts.InBackPerc / 100f) : BlockChanceCap); } }
        #endregion
        #region Crit
        private float MhWhCritChance {
            get {
                if (MH == null || MH.MaxDamage == 0f) { return 0f; }
                float crit = StatS.PhysicalCrit + StatConversion.GetCritFromRating(StatS.CritRating);
                crit += (_c_mhItemType == ItemType.TwoHandAxe || _c_mhItemType == ItemType.Polearm) ? 0.01f * Talents.PoleaxeSpecialization : 0f;
                return crit;
            }
        }
        private float MhYwCritChance {
            get {
                if (MH == null || MH.MaxDamage == 0f) { return 0f; }
                float crit = StatS.PhysicalCrit + StatConversion.GetCritFromRating(StatS.CritRating);
                crit += (_c_mhItemType == ItemType.TwoHandAxe || _c_mhItemType == ItemType.Polearm) ? 0.01f * Talents.PoleaxeSpecialization : 0f;
                crit *= (1f - _c_ymiss - _c_mhdodge);
                return crit;
            }
        }
        private float OhWhCritChance {
            get {
                if (OH == null || OH.MaxDamage == 0f) { return 0f; }
                float crit = StatS.PhysicalCrit + StatConversion.GetCritFromRating(StatS.CritRating);
                crit += (_c_ohItemType == ItemType.TwoHandAxe || _c_ohItemType == ItemType.Polearm) ? 0.01f * Talents.PoleaxeSpecialization : 0f;
                return crit;
            }
        }
        private float OhYwCritChance {
            get {
                if (OH == null || OH.MaxDamage == 0f) { return 0f; }
                float crit = StatS.PhysicalCrit + StatConversion.GetCritFromRating(StatS.CritRating);
                crit += (_c_ohItemType == ItemType.TwoHandAxe || _c_ohItemType == ItemType.Polearm) ? 0.01f * Talents.PoleaxeSpecialization : 0f;
                crit *= (1f - _c_ymiss - _c_ohdodge);
                return crit;
            }
        }
        #endregion
        #region Chance of Hitting
        // White
        public float ProbMhWhiteHit   { get { return 1f - _c_wmiss - _c_mhdodge - _c_mhparry - _c_mhwcrit; } }
        public float ProbOhWhiteHit   { get { return 1f - _c_wmiss - _c_ohdodge - _c_ohparry - _c_ohwcrit; } }
        public float ProbMhWhiteLand  { get { return 1f - _c_wmiss - _c_mhdodge - _c_mhparry; } }
        public float ProbOhWhiteLand  { get { return 1f - _c_wmiss - _c_ohdodge - _c_ohparry; } }
        // Yellow (Doesn't Glance and has different MissChance Cap)
        public float ProbMhYellowHit  { get { return 1f - _c_ymiss - _c_mhdodge - _c_mhparry - _c_mhblock - _c_mhycrit; } }
        public float ProbOhYellowHit  { get { return 1f - _c_ymiss - _c_ohdodge - _c_ohparry - _c_ohblock - _c_ohycrit; } }
        public float ProbMhYellowLand { get { return 1f - _c_ymiss - _c_mhdodge - _c_mhparry - _c_mhblock; } }
        public float ProbOhYellowLand { get { return 1f - _c_ymiss - _c_ohdodge - _c_ohparry - _c_ohblock; } }
        #endregion
        #endregion
        #region Other
        private float FlurryUptime {
            get {
                float uptime = 1f - (1f - _c_mhycrit) * (1f - _c_mhycrit) * (1f - _c_mhycrit);
                return uptime;
            }
        }
        #endregion
        public class Knuckles : Item {
            public Knuckles() {
                Speed = 2f;
                MaxDamage = 0;
                MinDamage = 0;
            }
        }
        #region Attackers Stats against you
        public float LevelModifier() { return (CalcOpts.TargetLevel - Char.Level) * 0.002f; }
        public float NPC_CritChance() {
            return Math.Max(0f, 0.05f + LevelModifier()
                                    - StatConversion.GetDRAvoidanceChance(Char, StatS, HitResult.Crit, CalcOpts.TargetLevel)
                            );
        }
        #endregion
    }

    public abstract class CombatTable {
        protected Character Char;
        protected CalculationOptionsDPSWarr calcOpts;
        protected CombatFactors combatFactors;
        protected Stats StatS;
        protected Skills.Ability Abil;

        public bool isWhite;
        public bool isMH;

        public float Miss { get; protected set; }
        public float Dodge { get; protected set; }
        public float Parry { get; protected set; }
        public float Block { get; protected set; }
        public float Glance { get; protected set; }
        public float Crit { get; protected set; }
        public float Hit { get; protected set; }

        public float AnyLand { get { return (1f - (Miss + Dodge + Parry)); } }
        public float AnyNotLand { get { return (Miss + Dodge + Parry); } }

        protected virtual void Calculate() { }

        protected void Initialize(Character character, Stats stats, CombatFactors cf, Skills.Ability ability, bool ismh) {
            Char = character;
            StatS = stats;
            calcOpts = Char.CalculationOptions as CalculationOptionsDPSWarr;
            combatFactors = cf;
            Abil = ability;
            isWhite = (Abil == null);
            isMH = ismh;
            /*// Defaults
            Miss 
            Dodge
            Parry
            Block
            Glance
            Critical
            Hit*/
            // Start a calc
            Calculate();
        }
    }

    public class DefendTable : CombatTable {
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
        }

        public DefendTable(Character character, Stats stats, CombatFactors cf) { Initialize(character, stats, cf, null, true); }
    }

    public class AttackTable : CombatTable {
        protected override void Calculate() {
            float tableSize = 0f;

            // Miss
            Miss = Math.Min(1f - tableSize, isWhite ? combatFactors._c_wmiss : combatFactors._c_ymiss);
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
                Block = Math.Min(1f - tableSize,isMH ?  combatFactors._c_mhblock : combatFactors._c_ohblock);
                tableSize += Block;
            } else { Block = 0f; }
            // Glancing Blow
            if (isWhite) {
                Glance = Math.Min(1f - tableSize, combatFactors.GlanceChance );
                tableSize += Glance;
            } else { Glance = 0f; }
            // Critical Hit
            if (isWhite) {
                Crit = Math.Min(1f - tableSize, isMH ?  combatFactors._c_mhycrit : combatFactors._c_ohycrit);
                tableSize += Crit;
            } else if (Abil.CanCrit) {
                Crit = Math.Min(1f - tableSize, Abil.ContainCritValue(isMH));
                tableSize += Crit;
            } else {
                Crit = 0f;
            }
            // Normal Hit
            Hit = Math.Max(0f, 1f - tableSize);
        }

        public AttackTable(Character character, Stats stats, CombatFactors cf, bool ismh) { Initialize(character, stats, cf, null, ismh); }

        public AttackTable(Character character, Stats stats, CombatFactors cf, Skills.Ability ability, bool ismh) { Initialize(character, stats, cf, ability, ismh); }
    }

}
