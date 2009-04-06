using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Retribution
{

    public enum AbilityType { Melee, Range, Spell }
    public enum DamageType { Physical, Holy }

    public abstract class Ability
    {
        public AbilityType AbilityType { get; set; }
        public DamageType DamageType { get; set; }
        public bool UsesWeapon { get; set; }
        public bool RighteousVengeance { get; set; }

        protected static Character _character = new Character();
        public static Character Character
        {
            get { return _character; }
            set
            {
                _character = value;
                _calcOpts = _character.CalculationOptions as CalculationOptionsRetribution;
                _talents = _character.PaladinTalents;
                UpdateCalcs();
            }
        }

        protected static Stats _stats = new Stats();
        public static Stats Stats
        {
            get { return _stats; }
            set
            {
                _stats = value;
                UpdateCalcs();
            } 
        }

        public static void SetValues(Character character, Stats stats)
        {
            _stats = stats;
            Character = character;
        }

        protected static CalculationOptionsRetribution _calcOpts = new CalculationOptionsRetribution();
        protected static PaladinTalents _talents = new PaladinTalents();

        public static float WeaponDamage;
        public static float AttackSpeed;
        public static float NormalWeaponDamage;

        public static float AvengingWrathMulti = 1f;
        public static float ArmorReduction = 1f;
        public static readonly float PartialResist = 0.953f;

        protected static readonly float[] DodgeChance = { 0.05f, 0.055f, 0.06f, 0.065f };
        protected static readonly float[] MissChance = { 0.05f, 0.052f, 0.054f, 0.08f };
        protected static readonly float[] ResistChance = { 0.04f, 0.05f, 0.06f, 0.17f };
        public static float GetMissChance(float hit, int targetLevel) { return (float)Math.Max(MissChance[targetLevel - 80] - hit, 0f); }
        public static float GetDodgeChance(float expertise, int targetLevel) { return (float)Math.Max(DodgeChance[targetLevel - 80] - expertise * .0025f, 0f); }
        public static float GetResistChance(float spellHit, int targetLevel) { return (float)Math.Max(ResistChance[targetLevel - 80] - spellHit, 0f); }

        public static float GetMeleeAvoid()
        {
            return 1f - GetMissChance(_stats.PhysicalHit, _calcOpts.TargetLevel) - GetDodgeChance(_stats.Expertise, _calcOpts.TargetLevel);
        }

        public static float GetRangeAvoid()
        {
            return 1f - GetMissChance(Stats.PhysicalHit, _calcOpts.TargetLevel);
        }

        public static float GetSpellAvoid()
        {
            return 1f - GetResistChance(Stats.SpellHit, _calcOpts.TargetLevel);
        }

        protected static void UpdateCalcs()
        {
            float fightLength = _calcOpts.FightLength * 60f;

            float bloodlustUptime = ((float)Math.Floor(fightLength / 300f) * 40f + (float)Math.Min(fightLength % 300f, 40f)) / fightLength;
            float bloodlustHaste = 1f + (bloodlustUptime * .3f);

            float awTimes = (float)Math.Ceiling((fightLength - 20f) / (180f - _talents.SanctifiedWrath * 30f));
            AvengingWrathMulti = 1f + ((awTimes * 20f) / fightLength) * .2f;

            ArmorReduction = 1f - ArmorCalculations.GetDamageReduction(80, 10643, _stats.ArmorPenetration, _stats.ArmorPenetrationRating);
            float baseSpeed = _character.MainHand == null ? 3.5f : _character.MainHand.Speed;
            float baseWeaponDamage = _character.MainHand == null ? 371.5f : (_character.MainHand.MinDamage + _character.MainHand.MaxDamage) / 2f;
            AttackSpeed = baseSpeed / ((1f + _stats.PhysicalHaste) * bloodlustHaste);
            WeaponDamage = baseWeaponDamage + _stats.AttackPower * baseSpeed / 14f;
            NormalWeaponDamage = WeaponDamage * 3.3f / baseSpeed;
        }



        public Ability(AbilityType abilityType, DamageType damageType, bool usesWeapon, bool righteousVengeance)
        {
            AbilityType = abilityType;
            DamageType = damageType;
            UsesWeapon = usesWeapon;
            RighteousVengeance = righteousVengeance;
        }

        public virtual float AverageDamage()
        {
            float chanceToLand, critBonus, critChance;
            if (AbilityType == AbilityType.Melee)
            {
                chanceToLand = GetMeleeAvoid();
                critBonus = 2f * (1f + Stats.BonusCritMultiplier);
                critChance = (float)Math.Min(1f, Stats.PhysicalCrit + AbilityCritChance());
            }
            else if (AbilityType == AbilityType.Range)
            {
                chanceToLand = GetRangeAvoid();
                critBonus = 2f * (1f + Stats.BonusCritMultiplier);
                critChance = (float)Math.Min(1f, Stats.PhysicalCrit + AbilityCritChance());
            }
            else // Spell
            {
                chanceToLand = GetSpellAvoid();
                critBonus = 1.5f * (1f + Stats.BonusSpellCritMultiplier);
                critChance = (float)Math.Min(1f, Stats.SpellCrit + AbilityCritChance());
            }
            float rightVen = RighteousVengeance ? 1f + .1f * _talents.RighteousVengeance : 1f;

            return HitDamage() * ((1f - critChance) + critChance * critBonus * rightVen) * chanceToLand;
        }

        public float HitDamage()
        {
            float damage = AbilityDamage();
            if (DamageType == DamageType.Physical)
            {
                damage *= ArmorReduction * (1f + _stats.BonusPhysicalDamageMultiplier);
            }
            else // Holy Damage
            {
                damage *= PartialResist * (1f + _stats.BonusHolyDamageMultiplier);
            }
            damage *= 1f + _stats.BonusDamageMultiplier;
            damage *= 1f + .03f * _talents.Vengeance;
            if (UsesWeapon) damage *= 1f + .02f * _talents.TwoHandedWeaponSpecialization;
            damage *= (1f + (_calcOpts.MobType < 3 ? .02f : .01f) * _talents.Crusade);
            damage *= AvengingWrathMulti;
            damage *= (_talents.GlyphOfSenseUndead && _calcOpts.MobType == 0 ? 1.01f : 1f);
            return damage;
        }

        public abstract float AbilityDamage();
        public virtual float AbilityCritChance() { return 0; }

    }

    public class Judgement : Ability
    {

        public Judgement() : base(AbilityType.Range, DamageType.Holy, true, true) { }

        public override float AbilityDamage()
        {
            return (WeaponDamage * .26f + .18f * _stats.SpellPower + .11f * _stats.AttackPower)
                * (1f + .05f * _talents.TheArtOfWar)
                * (_talents.GlyphOfJudgement ? 1.1f : 1f);
        }

        public override float AbilityCritChance()
        {
            return _talents.Fanaticism * .06f;
        }

    }

    public class CrusaderStrike : Ability
    {

        public CrusaderStrike() : base(AbilityType.Melee, DamageType.Physical, true, true) { }

        public override float AbilityDamage()
        {
            return (NormalWeaponDamage * 1.1f + _stats.CrusaderStrikeDamage)
                * (1f + .05f * _talents.SanctityOfBattle)
                * (1f + .05f * _talents.TheArtOfWar)
                * (1f + _stats.CrusaderStrikeMultiplier);
        }

        public override float AbilityCritChance()
        {
            return _stats.CrusaderStrikeCrit;
        }

    }

    public class DivineStorm : Ability
    {

        public DivineStorm() : base(AbilityType.Melee, DamageType.Physical, true, true) { }

        public override float AbilityDamage()
        {
            return (NormalWeaponDamage * 1.1f + _stats.DivineStormDamage)
                * (1f + .05f * _talents.TheArtOfWar)
                * (1f + _stats.DivineStormMultiplier);
        }

        public override float AbilityCritChance()
        {
            return _stats.DivineStormCrit;
        }

    }

    public class HammerOfWrath : Ability
    {

        public HammerOfWrath() : base(AbilityType.Range, DamageType.Holy, false, false) { }

        public override float AbilityDamage()
        {
            return (1198f + .15f * _stats.SpellPower + .15f * _stats.AttackPower)
                * (1f + _stats.HammerOfWrathMultiplier);
        }

        public override float AbilityCritChance()
        {
            return .25f * _talents.SanctifiedWrath;
        }

    }

    public class Exorcism : Ability
    {

        public Exorcism() : base(AbilityType.Spell, DamageType.Holy, false, false) { }

        public override float AbilityDamage()
        {
            return (1087f + .42f * _stats.SpellPower)
                * (1f + .05f * _talents.SanctityOfBattle)
                * (1f + _stats.ExorcismMultiplier)
                * (_talents.GlyphOfExorcism ? 1.2f : 1f);
        }

        public override float AbilityCritChance()
        {
            return _calcOpts.MobType < 2 ? 1f : 0;
        }

    }

    public class Consecration : Ability
    {

        public Consecration() : base(AbilityType.Spell, DamageType.Holy, false, false) { }

        public override float AbilityDamage()
        {
            return (72f + .04f * (_stats.SpellPower + _stats.ConsecrationSpellPower) + .04f * _stats.AttackPower);
        }

        public override float AverageDamage()
        {
            return HitDamage()
                * (_talents.GlyphOfConsecration ? 10f : 8f)
                * (1f - GetResistChance(Stats.SpellHit, _calcOpts.TargetLevel));
        }

    }

    public class SealOfBlood : Ability
    {

        public SealOfBlood() : base(AbilityType.Melee, DamageType.Holy, true, false) { }

        public override float AbilityDamage()
        {
            return WeaponDamage * .48f;
        }

    }

    public class White : Ability
    {

        public White() : base(AbilityType.Melee, DamageType.Physical, true, false) { }

        public override float AbilityDamage()
        {
            return WeaponDamage;
        }

        public override float AverageDamage()
        {
            const float glanceChance = .24f;
            const float glancingAmount = 1f - 0.35f;
            float critBonus = 2f * (1f + Stats.BonusCritMultiplier);
            float toMiss = GetMissChance(Stats.PhysicalHit, _calcOpts.TargetLevel);
            float toDodge = GetDodgeChance(Stats.Expertise, _calcOpts.TargetLevel);

            return AbilityDamage() *
                (glanceChance * glancingAmount +
                _stats.PhysicalCrit * critBonus +
                (1f - _stats.PhysicalCrit - glanceChance - toMiss - toDodge));
        }

        public float WhiteDPS()
        {
            return AverageDamage() / AttackSpeed;
        }

    }

}
