using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Retribution
{

    public abstract class Skill
    {

        public CombatStats Combats { get; set; }
        public AbilityType AbilityType { get; set; }
        public DamageType DamageType { get; set; }
        public bool UsesWeapon { get; set; }
        public bool RighteousVengeance { get; set; }

        public Skill(CombatStats charStats, AbilityType abilityType, DamageType damageType, bool usesWeapon, bool righteousVengeance)
        {
            Combats = charStats;
            AbilityType = abilityType;
            DamageType = damageType;
            UsesWeapon = usesWeapon;
            RighteousVengeance = righteousVengeance;
        }

        public virtual float AverageDamage()
        {
            return HitDamage() * ((1f - CritChance()) + CritChance() * CritBonus()) * ChanceToLand();
        }

        public float CritChance()
        {
            if (AbilityType == AbilityType.Spell)
            {
                return (float)Math.Max(Math.Min(1f, Combats.Stats.SpellCrit + AbilityCritChance()), 0);
            }
            else
            {
                return (float)Math.Max(Math.Min(1f, Combats.Stats.PhysicalCrit + AbilityCritChance()), 0);
            }
        }

        public virtual float ChanceToLand()
        {
            if (AbilityType == AbilityType.Melee)
            {
                return Combats.GetMeleeAvoid();
            }
            else if (AbilityType == AbilityType.Range)
            {
                return Combats.GetRangeAvoid();
            }
            else // Spell
            {
                return Combats.GetSpellAvoid();
            }
        }

        public float ChanceToCrit() { return CritChance() * ChanceToLand(); }

        public float CritBonus()
        {
            float rightVen = RighteousVengeance ? 1f + .1f * Combats.Talents.RighteousVengeance : 1f;
            if (AbilityType == AbilityType.Spell)
            {
                return 1.5f * (1f + Combats.Stats.BonusSpellCritMultiplier) * rightVen;
            }
            else
            {
                return 2f * (1f + Combats.Stats.BonusCritMultiplier) * rightVen;
            }
        }

        public float HitDamage()
        {
            float damage = AbilityDamage();
            if (DamageType == DamageType.Physical)
            {
                damage *= Combats.ArmorReduction * (1f + Combats.Stats.BonusPhysicalDamageMultiplier);
            }
            else // Holy Damage
            {
                damage *= Combats.PartialResist * (1f + Combats.Stats.BonusHolyDamageMultiplier);
            }
            damage *= 1f + Combats.Stats.BonusDamageMultiplier;
            damage *= 1f + .03f * Combats.Talents.Vengeance;
            if (UsesWeapon) damage *= 1f + .02f * Combats.Talents.TwoHandedWeaponSpecialization;
            damage *= (1f + (Combats.CalcOpts.Mob == MobType.Other ? .01f : .02f) * Combats.Talents.Crusade);
            damage *= Combats.AvengingWrathMulti;
            damage *= (Combats.Talents.GlyphOfSenseUndead && Combats.CalcOpts.Mob == MobType.Undead ? 1.01f : 1f);
            return damage;
        }

        public abstract float AbilityDamage();

        public virtual float AbilityCritChance() { return 0; }

    }

    public class JudgementOfBlood : Skill
    {

        public JudgementOfBlood(CombatStats charStats) : base(charStats, AbilityType.Range, DamageType.Holy, true, true) { }

        public override float AbilityDamage()
        {
            return (Combats.WeaponDamage * .26f + Combats.Stats.SpellPower * .18f + Combats.Stats.AttackPower * .11f)
                * (1f + .05f * Combats.Talents.TheArtOfWar)
                * (Combats.Talents.GlyphOfJudgement ? 1.1f : 1f);
        }

        public override float AbilityCritChance()
        {
            return Combats.Talents.Fanaticism * .06f;
        }

    }

    public class JudgementOfCommand : Skill
    {

        public JudgementOfCommand(CombatStats charStats) : base(charStats, AbilityType.Range, DamageType.Holy, true, true) { }

        public override float AbilityDamage()
        {
            return (Combats.WeaponDamage * .24f + Combats.Stats.SpellPower * .25f + Combats.Stats.AttackPower * .16f)
                * (1f + .05f * Combats.Talents.TheArtOfWar)
                * (Combats.Talents.GlyphOfJudgement ? 1.1f : 1f);
        }

        public override float AbilityCritChance()
        {
            return Combats.Talents.Fanaticism * .06f;
        }

    }

    public class JudgementOfRighteousness : Skill
    {

        public JudgementOfRighteousness(CombatStats charStats) : base(charStats, AbilityType.Range, DamageType.Holy, true, true) { }

        public override float AbilityDamage()
        {
            return (1 + Combats.Stats.SpellPower * .32f + Combats.Stats.AttackPower * .2f)
                * (1f + .05f * Combats.Talents.TheArtOfWar)
                * (1f + .05f * Combats.Talents.SealsOfThePure)
                * (Combats.Talents.GlyphOfJudgement ? 1.1f : 1f);
        }

        public override float AbilityCritChance()
        {
            return Combats.Talents.Fanaticism * .06f;
        }

    }

    public class CrusaderStrike : Skill
    {

        public CrusaderStrike(CombatStats charStats) : base(charStats, AbilityType.Melee, DamageType.Physical, true, true) { }

        public override float AbilityDamage()
        {
            return (Combats.NormalWeaponDamage * 1.1f + Combats.Stats.CrusaderStrikeDamage)
                * (1f + .05f * Combats.Talents.SanctityOfBattle)
                * (1f + .05f * Combats.Talents.TheArtOfWar)
                * (1f + Combats.Stats.CrusaderStrikeMultiplier);
        }

        public override float AbilityCritChance()
        {
            return Combats.Stats.CrusaderStrikeCrit;
        }

    }

    public class DivineStorm : Skill
    {

        public DivineStorm(CombatStats charStats) : base(charStats, AbilityType.Melee, DamageType.Physical, true, true) { }

        public override float AbilityDamage()
        {
            return (Combats.NormalWeaponDamage * 1.1f + Combats.Stats.DivineStormDamage)
                * (1f + .05f * Combats.Talents.TheArtOfWar)
                * (1f + Combats.Stats.DivineStormMultiplier);
        }

        public override float AbilityCritChance()
        {
            return Combats.Stats.DivineStormCrit;
        }

    }

    public class HammerOfWrath : Skill
    {

        public HammerOfWrath(CombatStats charStats) : base(charStats, AbilityType.Range, DamageType.Holy, false, false) { }

        public override float AbilityDamage()
        {
            return (1198f + .15f * Combats.Stats.SpellPower + .15f * Combats.Stats.AttackPower)
                * (1f + Combats.Stats.HammerOfWrathMultiplier);
        }

        public override float AbilityCritChance()
        {
            return .25f * Combats.Talents.SanctifiedWrath;
        }

    }

    public class Exorcism : Skill
    {

        public Exorcism(CombatStats charStats) : base(charStats, AbilityType.Spell, DamageType.Holy, false, false) { }

        public override float AbilityDamage()
        {
            return (1087f + .42f * Combats.Stats.SpellPower)
                * (1f + .05f * Combats.Talents.SanctityOfBattle)
                * (1f + Combats.Stats.ExorcismMultiplier)
                * (Combats.Talents.GlyphOfExorcism ? 1.2f : 1f);
        }

        public override float AbilityCritChance()
        {
            return (Combats.CalcOpts.Mob == MobType.Demon || Combats.CalcOpts.Mob == MobType.Undead) ? 1f : 0;
        }

    }

    public class Consecration : Skill
    {

        public Consecration(CombatStats charStats) : base(charStats, AbilityType.Spell, DamageType.Holy, false, false) { }

        public override float AbilityDamage()
        {
            return (72f + .04f * (Combats.Stats.SpellPower + Combats.Stats.ConsecrationSpellPower) + .04f * Combats.Stats.AttackPower);
        }

        public override float AverageDamage()
        {
            return HitDamage()
                * (Combats.Talents.GlyphOfConsecration ? 10f : 8f)
                * Combats.GetSpellAvoid();
        }

    }

    public class SealOfBlood : Skill
    {

        public SealOfBlood(CombatStats charStats) : base(charStats, AbilityType.Melee, DamageType.Holy, true, false) { }

        public override float AbilityDamage()
        {
            return Combats.WeaponDamage * .48f;
        }

    }

    public class SealOfCommand : Skill
    {

        public SealOfCommand(CombatStats charStats) : base(charStats, AbilityType.Melee, DamageType.Holy, true, false) { }

        public override float AbilityDamage()
        {
            return Combats.WeaponDamage * .45f + Combats.Stats.SpellPower * .23f;
        }

    }

    public class SealOfRighteousness : Skill
    {

        public SealOfRighteousness(CombatStats charStats) : base(charStats, AbilityType.Spell, DamageType.Holy, false, false) { }

        public override float AbilityDamage()
        {
            return Combats.BaseWeaponSpeed * (0.022f * Combats.Stats.AttackPower + 0.044f * Combats.Stats.SpellPower)
                * (1f + .05f * Combats.Talents.SealsOfThePure)
                * (Combats.Talents.GlyphOfSealOfRighteousness ? 1.1f : 1f);
        }

        public override float AbilityCritChance() { return -1f; }
        public override float ChanceToLand() { return 1f; }

    }

    public class White : Skill
    {

        public White(CombatStats charStats) : base(charStats, AbilityType.Melee, DamageType.Physical, true, false) { }

        public override float AbilityDamage()
        {
            return Combats.WeaponDamage;
        }

        public override float AverageDamage()
        {
            const float glanceChance = .24f;
            const float glancingAmount = 1f - 0.35f;
            float critBonus = 2f * (1f + Combats.Stats.BonusCritMultiplier);
            float toMiss = CombatStats.GetMissChance(Combats.Stats.PhysicalHit, Combats.CalcOpts.TargetLevel);
            float toDodge = CombatStats.GetDodgeChance(Combats.Stats.Expertise, Combats.CalcOpts.TargetLevel);

            return AbilityDamage() *
                (glanceChance * glancingAmount +
                Combats.Stats.PhysicalCrit * critBonus +
                (1f - Combats.Stats.PhysicalCrit - glanceChance - toMiss - toDodge));
        }

        public float WhiteDPS()
        {
            return AverageDamage() / Combats.AttackSpeed;
        }

    }

    public class None : Skill
    {

        public None(CombatStats charStats) : base(charStats, AbilityType.Melee, DamageType.Holy, true, false) { }

        public override float AbilityDamage()
        {
            return 0;
        }

    }

}
