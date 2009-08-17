using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.ProtWarr {
    // From DPSWarr
    public class CombatFactors {
        public CombatFactors(Character character, Stats stats) {
            StatS = stats;
            MH = character.MainHand == null ? new Knuckles() : character.MainHand.Item;
            OH = character.OffHand == null ? null : character.OffHand.Item;
            Talents = character.WarriorTalents;
            CalcOpts = character.CalculationOptions as CalculationOptionsProtWarr;
            Char = character;

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
        private CalculationOptionsProtWarr CalcOpts;
        private Character Char;
        public Item MH;
        public Item OH;
        // Optimizations
        public readonly float _c_ymiss, _c_wmiss;
        public readonly ItemType _c_mhItemType, _c_ohItemType;
        public readonly float _c_mhItemSpeed, _c_ohItemSpeed;
        public readonly float _c_mhRacialExpertise, _c_ohRacialExpertise;
        public readonly float _c_mhexpertise, _c_ohexpertise;
        public readonly float _c_mhdodge, _c_ohdodge;
        public readonly float _c_mhparry, _c_ohparry;
        public readonly float _c_mhblock, _c_ohblock;
        public readonly float _c_mhwcrit, _c_ohwcrit;
        public readonly float _c_mhycrit, _c_ohycrit;
        #endregion

        #region Weapon Damage Calcs
        #region Major Damage Factors
        public float DamageBonus {
            get {
                float bonus  = 1f + StatS.BonusDamageMultiplier;
                      bonus *= 1f + StatS.BonusPhysicalDamageMultiplier;
                      bonus *= 1f + Talents.WreckingCrew * 0.02f;
                return bonus;
            }
        }
        public float DamageReduction {
            get {
                float armorReduction;
                float arpenBuffs = ((_c_mhItemType == ItemType.TwoHandMace) ? Talents.MaceSpecialization * 0.03f : 0.00f);
                if (CalcOpts == null) {
                    // you're supposed to pass the character level, not the target level.  GC misspoke.
                    armorReduction = Math.Max(0f, 1f - StatConversion.GetArmorDamageReduction(Char.Level, (int)StatConversion.NPC_BOSS_ARMOR, StatS.ArmorPenetration, arpenBuffs, StatS.ArmorPenetrationRating)); // default is vs raid boss
                } else {
                    armorReduction = Math.Max(0f, 1f - StatConversion.GetArmorDamageReduction(Char.Level, CalcOpts.TargetArmor, StatS.ArmorPenetration, arpenBuffs, StatS.ArmorPenetrationRating));
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
        public float AvgMhWeaponDmgUnhasted { get { return (MH == null ? 0f : (StatS.AttackPower / 14f + MH.DPS) * MH.Speed); } }
        public float AvgOhWeaponDmgUnhasted { get { return (OH == null ? 0f : (StatS.AttackPower / 14f + OH.DPS) * OH.Speed * (0.5f + Talents.DualWieldSpecialization * 0.025f)); } }
        public float AvgMhWeaponDmg(float speed) { return (MH == null ? 0f : (StatS.AttackPower / 14f + MH.DPS) * speed); }
        public float AvgOhWeaponDmg(float speed) { return (OH == null ? 0f : (StatS.AttackPower / 14f + OH.DPS) * speed * (0.5f + Talents.DualWieldSpecialization * 0.025f)); }
        #endregion
        #region Weapon Crit Damage
        public float BonusWhiteCritDmg {
            get {
                float baseCritDmg  = (2f * (1f + StatS.BonusCritMultiplier) - 1f);
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
        public float GetDPRfromExp(float Expertise) { return StatConversion.GetDodgeParryReducFromExpertise(Expertise, CharacterClass.Warrior); }
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
                    if (weapon == ItemType.OneHandAxe || weapon == ItemType.TwoHandAxe)
                    {
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
                       StatConversion.WHITE_MISS_CHANCE_CAP_DW[CalcOpts.TargetLevel - 80] : StatConversion.WHITE_MISS_CHANCE_CAP[CalcOpts.TargetLevel - 80]);
                return twoHandCheck;
            }
        }
        public float WhMissChance {
            get {
                float missChance = WhMissCap - MissPrevBonuses;
                return (float)Math.Max(0f, missChance);
            }
        }
        public float YwMissCap { get { return StatConversion.YELLOW_MISS_CHANCE_CAP[CalcOpts.TargetLevel - 80]; } }
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
                return (float)Math.Max(0f, /*CalcOpts.InBack ? ParryChance * (1f - CalcOpts.InBackPerc / 100f) :*/ ParryChance);
            }
        }
        private float OhParryChance {
            get {
                float ParryChance = ParryChanceCap - GetDPRfromExp(_c_ohexpertise);
                return (float)Math.Max(0f, /*CalcOpts.InBack ? ParryChance * (1f - CalcOpts.InBackPerc / 100f) :*/ ParryChance);
            }
        }
        #endregion
        #region Glance
        public float GlanceChance { get { return StatConversion.WHITE_GLANCE_CHANCE_CAP[CalcOpts.TargetLevel - 80]; } }
        #endregion
        #region Block
        public float BlockChanceCap { get { return StatConversion.WHITE_BLOCK_CHANCE_CAP[CalcOpts.TargetLevel - 80]; } }
        private float MhBlockChance { get { return (float)Math.Max(0f, /*CalcOpts.InBack ? BlockChanceCap * (1f - CalcOpts.InBackPerc / 100f) :*/ BlockChanceCap); } }
        private float OhBlockChance { get { return (float)Math.Max(0f, /*CalcOpts.InBack ? BlockChanceCap * (1f - CalcOpts.InBackPerc / 100f) :*/ BlockChanceCap); } }
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
    }
    
    // Original
    public abstract class CombatTable {
        protected Character Character;
        protected CalculationOptionsProtWarr Options;
        protected Stats Stats;
        protected Ability Ability;

        public float Miss { get; protected set; }
        public float Dodge { get; protected set; }
        public float Parry { get; protected set; }
        public float Block { get; protected set; }
        public float Glance { get; protected set; }
        public float Critical { get; protected set; }
        public float Hit { get; protected set; }

        public float AnyLand    { get { return (1f - (Miss + Dodge + Parry)); } }
        public float AnyNotLand { get { return       (Miss + Dodge + Parry) ; } }

        protected virtual void Calculate() { }

        protected void Initialize(Character character, Stats stats, Ability ability) {
            Character   = character;
            Options     = character.CalculationOptions as CalculationOptionsProtWarr;
            Stats       = stats;
            Ability     = ability;
            Calculate();
        }
    }
    public class DefendTable : CombatTable {
        protected override void Calculate() {
            float tableSize = 0f;
            float tempVal = 0f;

            // Miss
            tempVal = StatConversion.GetDRAvoidanceChance(Character, Stats, HitResult.Miss, Options.TargetLevel);
            Miss = Math.Max(0f,Math.Min(1f - tableSize, tempVal));
            tableSize += Miss;
            // Dodge
            tempVal = StatConversion.GetDRAvoidanceChance(Character, Stats, HitResult.Dodge, Options.TargetLevel);
            Dodge = Math.Max(0f, Math.Min(1f - tableSize, tempVal));
            tableSize += Dodge;
            // Parry
            tempVal = StatConversion.GetDRAvoidanceChance(Character, Stats, HitResult.Parry, Options.TargetLevel);
            Parry = Math.Max(0f,Math.Min(1f - tableSize, tempVal));
            tableSize += Parry;
            // Block
            if (Character.OffHand != null && Character.OffHand.Type == ItemType.Shield) {
                tempVal = StatConversion.GetDRAvoidanceChance(Character, Stats, HitResult.Block, Options.TargetLevel);
                Block = Math.Max(0f,Math.Min(1f - tableSize, tempVal));
                tableSize += Block;
            }
            // Critical Hit
            Critical = Math.Max(0f,Math.Min(1f - tableSize, Lookup.TargetCritChance(Character, Stats)));
            tableSize += Critical;
            // Normal Hit
            Hit = Math.Max(0f, 1f - tableSize);
        }

        public DefendTable(Character character, Stats stats) { Initialize(character, stats, Ability.None); }
    }

    public class AttackTable : CombatTable {
        protected override void Calculate() {
            float tableSize = 0f;
            float bonusHit = Lookup.BonusHitPercentage(Character, Stats);
            float bonusExpertise = StatConversion.GetDodgeParryReducFromExpertise(Stats.Expertise + StatConversion.GetExpertiseFromRating(Stats.ExpertiseRating), CharacterClass.Warrior);

            // Miss
            Miss = Math.Min(1f - tableSize, Math.Max(0f, Lookup.TargetAvoidanceChance(Character, Stats, HitResult.Miss) - bonusHit));
            tableSize += Miss;
            // Avoidance
            if (Lookup.IsAvoidable(Ability)) {
            // Dodge
                Dodge = Math.Min(1f - tableSize, Math.Max(0f, Lookup.TargetAvoidanceChance(Character, Stats, HitResult.Dodge) - bonusExpertise));
                tableSize += Dodge;
            // Parry
                Parry = Math.Min(1f - tableSize, Math.Max(0f, Lookup.TargetAvoidanceChance(Character, Stats, HitResult.Parry) - bonusExpertise));
                tableSize += Parry;
            }
            // Glancing Blow
            if (Ability == Ability.None) {
                Glance = Math.Min(1f - tableSize, Math.Max(0f, Lookup.TargetAvoidanceChance(Character, Stats, HitResult.Glance)));
                tableSize += Glance;
            }
            // Critical Hit
            Critical = Math.Min(1f - tableSize, Lookup.BonusCritPercentage(Character, Stats, Ability));
            tableSize += Critical;
            // Normal Hit
            Hit = Math.Max(0f, 1f - tableSize);
        }

        public AttackTable(Character character, Stats stats) { Initialize(character, stats, Ability.None); }

        public AttackTable(Character character, Stats stats, Ability ability) { Initialize(character, stats, ability); }
    }

    public static class Lookup {
        public static float LevelModifier(Character character) {
            CalculationOptionsProtWarr calcOpts = character.CalculationOptions as CalculationOptionsProtWarr;
            return (calcOpts.TargetLevel - 80f) * 0.002f;
        }
        public static float TargetArmorReduction(Character character, Stats stats) {
            CalculationOptionsProtWarr calcOpts = character.CalculationOptions as CalculationOptionsProtWarr;
            float damageReduction = StatConversion.GetArmorDamageReduction(character.Level, calcOpts.TargetArmor,
                stats.ArmorPenetration, 0f, stats.ArmorPenetrationRating);
            return damageReduction;
        }
        public static float TargetCritChance(Character character, Stats stats) {
            CalculationOptionsProtWarr calcOpts = character.CalculationOptions as CalculationOptionsProtWarr;
            return Math.Max(0f, ((5f + LevelModifier(character)) / 100f) - StatConversion.GetDRAvoidanceChance(character, stats, HitResult.Crit, calcOpts.TargetLevel));
        }
        public static float TargetAvoidanceChance(Character character, Stats stats, HitResult avoidanceType) {
            CalculationOptionsProtWarr calcOpts = character.CalculationOptions as CalculationOptionsProtWarr;
            WarriorTalents Talents = character.WarriorTalents;

            switch (avoidanceType) {
                case HitResult.Miss:   return StatConversion.YELLOW_MISS_CHANCE_CAP[ calcOpts.TargetLevel-80];
                case HitResult.Dodge:  return StatConversion.YELLOW_DODGE_CHANCE_CAP[calcOpts.TargetLevel-80] - (0.01f * Talents.WeaponMastery);
                case HitResult.Parry:  return StatConversion.YELLOW_PARRY_CHANCE_CAP[calcOpts.TargetLevel-80];
                case HitResult.Glance: return StatConversion.WHITE_GLANCE_CHANCE_CAP[calcOpts.TargetLevel-80];
                default:               return 0f;
            }
        }
        public static float StanceDamageMultipler(Character character, Stats stats) {
            // In Defensive Stance
            return (0.95f * (1f + character.WarriorTalents.ImprovedDefensiveStance * 0.05f) * (1f + stats.BonusDamageMultiplier));
        }
        public static float StanceThreatMultipler(Character character, Stats stats) {
            // In Defensive Stance
            return (2.0735f * (1f + stats.ThreatIncreaseMultiplier));
        }
        public static float StanceDamageReduction(Character character, Stats stats, DamageType damageType) {
            // In Defensive Stance
            float damageTaken = 0.9f * (1.0f + stats.DamageTakenMultiplier);

            switch (damageType) {
                case DamageType.Arcane:
                case DamageType.Fire:
                case DamageType.Frost:
                case DamageType.Nature:
                case DamageType.Shadow:
                case DamageType.Holy:
                    return damageTaken * (1.0f - character.WarriorTalents.ImprovedDefensiveStance * 0.03f);
                default:
                    return damageTaken;
            }
        }
        public static float BonusHitPercentage(Character character, Stats stats) {
            return StatConversion.GetHitFromRating(stats.HitRating) + stats.PhysicalHit;
        }
        public static float BonusCritMultiplier(Character character, Stats stats, Ability ability) {
            return (2f * (1f + stats.BonusCritMultiplier) - 1f) * (1f + (ability == Ability.None ? character.WarriorTalents.Impale * 0.1f : 0f));
        }
        public static float BonusCritPercentage(Character character, Stats stats) {
            CalculationOptionsProtWarr calcOpts = character.CalculationOptions as CalculationOptionsProtWarr;
            WarriorTalents Talents = character.WarriorTalents;
            ItemInstance MH = character.MainHand;

            if (MH == null || MH.MaxDamage == 0f) { return 0f; }
            float crit = stats.PhysicalCrit + StatConversion.GetCritFromRating(stats.CritRating);
            crit += (MH.Type == ItemType.TwoHandAxe || MH.Type == ItemType.Polearm) ? 0.01f * Talents.PoleaxeSpecialization : 0f;
            return crit;
        }
        public static float BonusCritPercentage(Character character, Stats stats, Ability ability) {
            // Grab base melee crit chance before adding ability-specific crit chance
            float abilityCritChance = BonusCritPercentage(character, stats);
            WarriorTalents Talents = character.WarriorTalents;

            switch (ability) {
                case Ability.Cleave:       abilityCritChance += Talents.Incite        * 0.05f; break;
                case Ability.Devastate:    abilityCritChance += stats.DevastateCritIncrease;   break;
                case Ability.HeroicStrike: abilityCritChance += Talents.Incite        * 0.05f; break;
                case Ability.ShieldSlam:   abilityCritChance += Talents.CriticalBlock * 0.05f; break;
                case Ability.ThunderClap:  abilityCritChance += Talents.Incite        * 0.05f; break;
                case Ability.SunderArmor:
                case Ability.ShieldBash:
                case Ability.Rend:
                case Ability.DeepWounds:   abilityCritChance = 0f;                             break;
            }

            return Math.Min(1f, abilityCritChance);
        }
        public static float WeaponDamage(Character character, Stats stats, bool normalized) {
            float weaponDamage = 1f;

            if (character.MainHand != null) {
                float weaponSpeed = character.MainHand.Speed;
                float weaponMinDamage = character.MainHand.MinDamage;
                float weaponMaxDamage = character.MainHand.MaxDamage;
                float normalizedSpeed = (character.MainHand.Type == ItemType.Dagger ? 1.7f : 2.4f);

                // Non-Normalized Hits
                if (!normalized){
                    weaponDamage = ((weaponMinDamage + weaponMaxDamage) / 2f + (weaponSpeed     * stats.AttackPower / 14f)) + stats.WeaponDamage;
                // Normalized Hits
                }else{
                    weaponDamage = ((weaponMinDamage + weaponMaxDamage) / 2f + (normalizedSpeed * stats.AttackPower / 14f)) + stats.WeaponDamage;
                }
            }

            return weaponDamage;
        }
        public static float WeaponSpeed(Character character, Stats stats) {
            return (character.MainHand != null ? Math.Max(1f, character.MainHand.Speed / (1f + StatConversion.GetHasteFromRating(stats.HasteRating, CharacterClass.Warrior) + stats.PhysicalHaste)) : 1f);
        }
        public static float GlancingReduction(Character character) {
            CalculationOptionsProtWarr calcOpts = character.CalculationOptions as CalculationOptionsProtWarr;
            return (Math.Min(0.91f, 1.3f - (0.05f * (calcOpts.TargetLevel - character.Level) * 5f)) +
                    Math.Max(0.99f, 1.2f - (0.03f * (calcOpts.TargetLevel - character.Level) * 5f))) / 2f;
        }
        public static float ArmorReduction(Character character, Stats stats) {
            CalculationOptionsProtWarr calcOpts = character.CalculationOptions as CalculationOptionsProtWarr;
            //StatConversion.GetArmorDamageReduction(calcOpts.TargetLevel, stats.Armor, stats.Armor, stats.Armor, 0);
            return Math.Max(0f, Math.Min(0.75f, stats.Armor / (stats.Armor + (467.5f * calcOpts.TargetLevel - 22167.5f))));
        }
        public static float MagicReduction(Character character, Stats stats, DamageType school) {
            CalculationOptionsProtWarr calcOpts = character.CalculationOptions as CalculationOptionsProtWarr;
            float damageReduction = StanceDamageReduction(character, stats, school);
            float totalResist = stats.AllResist;
            float resistScale = 0f;

            switch (school) {
                case DamageType.Arcane: totalResist += stats.ArcaneResistance; break;
                case DamageType.Fire  : totalResist += stats.FireResistance  ; break;
                case DamageType.Frost : totalResist += stats.FrostResistance ; break;
                case DamageType.Nature: totalResist += stats.NatureResistance; break;
                case DamageType.Shadow: totalResist += stats.ShadowResistance; break;
            }

            // This number is still being tested by many and may be slightly higher
            resistScale = ((calcOpts.TargetLevel - character.Level) < 3) ? 400f : 510f;

            return Math.Max(0f, (1f - (totalResist / (resistScale + totalResist))) * damageReduction);
        }
        public static bool IsAvoidable(Ability ability) {
            switch (ability) {
                case Ability.DamageShield:
                case Ability.DeepWounds:
                case Ability.Shockwave:
                case Ability.ThunderClap:
                    return false;
                default:
                    return true;
            }
        }
        public static string Name(Ability ability) {
            switch (ability) {
                case Ability.None:          return "Swing";
                case Ability.Cleave:        return "Cleave";
                case Ability.ConcussionBlow:return "Concussion Blow";
                case Ability.DamageShield:  return "Damage Shield";
                case Ability.DeepWounds:    return "Deep Wounds";
                case Ability.Devastate:     return "Devastate";
                case Ability.HeroicStrike:  return "Heroic Strike";
                case Ability.HeroicThrow:   return "Heroic Throw";
                case Ability.Rend:          return "Rend";
                case Ability.Revenge:       return "Revenge";
                case Ability.ShieldBash:    return "Shield Bash";
                case Ability.ShieldSlam:    return "Shield Slam";
                case Ability.Shockwave:     return "Shockwave";
                case Ability.Slam:          return "Slam";
                case Ability.SunderArmor:   return "Sunder Armor";
                case Ability.ThunderClap:   return "Thunder Clap";
                default: return "";
            }
        }
    }
}
