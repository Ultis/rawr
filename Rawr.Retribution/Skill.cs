using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Retribution
{

    public abstract class Skill
    {

        protected Stats _stats;
        public Stats Stats { get { return _stats; } }

        private CalculationOptionsRetribution _calcOpts;
        public CalculationOptionsRetribution CalcOpts { get { return _calcOpts; } }

        private PaladinTalents _talents;
        public PaladinTalents Talents { get { return _talents; } }

        private CombatStats _combats;
        public CombatStats Combats
        {
            get { return _combats; }
            set
            {
                _combats = value;
                _stats = value.Stats;
                _calcOpts = value.CalcOpts;
                _talents = value.Talents;
            }
        }
        public AbilityType AbilityType { get; set; }
        public DamageType DamageType { get; set; }
        public bool UsesWeapon { get; set; }
        public bool RighteousVengeance { get; set; }

        public Skill(CombatStats combats, AbilityType abilityType, DamageType damageType, bool usesWeapon, bool righteousVengeance)
        {
            Combats = combats;
            AbilityType = abilityType;
            DamageType = damageType;
            UsesWeapon = usesWeapon;
            RighteousVengeance = righteousVengeance;
        }

        public virtual float AverageDamage()
        {
            return HitDamage() * ((1f - CritChance()) + CritChance() * CritBonus()) * ChanceToLand() * Targets();
        }

        public float CritChance()
        {
            if (AbilityType == AbilityType.Spell)
            {
                return (float)Math.Max(Math.Min(1f, Stats.SpellCrit + AbilityCritChance()), 0);
            }
            else
            {
                return (float)Math.Max(Math.Min(1f, Stats.PhysicalCrit + AbilityCritChance()), 0);
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
            float rightVen = 1;
            if (RighteousVengeance)
            {
                if (Stats.RighteousVengeanceCanCrit != 0)
                {
                    rightVen += .1f * Talents.RighteousVengeance * (1f + Stats.PhysicalCrit);
                }
                else
                {
                    rightVen += .1f * Talents.RighteousVengeance;
                }
            }
            if (AbilityType == AbilityType.Spell)
            {
                return 1.5f * (1f + Stats.BonusSpellCritMultiplier) * rightVen;
            }
            else
            {
                return 2f * (1f + Stats.BonusCritMultiplier) * rightVen;
            }
        }

        public float HitDamage()
        {
            float damage = AbilityDamage();
            if (DamageType == DamageType.Physical)
            {
                damage *= Combats.ArmorReduction;
                damage *= (1f + Stats.BonusPhysicalDamageMultiplier);
            }
            else // Holy Damage
            {
                damage *= Combats.PartialResist;
                damage *= (1f + Stats.BonusHolyDamageMultiplier);
            }
            damage *= 1f + Stats.BonusDamageMultiplier;
            damage *= 1f + .03f * Talents.Vengeance;
            if (UsesWeapon) damage *= 1f + .02f * Talents.TwoHandedWeaponSpecialization;
            damage *= (1f + (CalcOpts.Mob == MobType.Other ? .01f : .02f) * Talents.Crusade);
            damage *= Combats.AvengingWrathMulti;
            damage *= (Talents.GlyphOfSenseUndead && CalcOpts.Mob == MobType.Undead ? 1.01f : 1f);
            return damage;
        }

        public abstract float AbilityDamage();

        public virtual float AbilityCritChance() { return 0; }
        public virtual float Targets() { return 1f; }

        public override string ToString()
        {
            return string.Format("Average Damage: {0}\nAverage Hit: {1}\nCrit Chance: {2}%\nAvoid Chance: {3}%",
                AverageDamage().ToString("N0"),
                HitDamage().ToString("N0"),
                Math.Round(ChanceToCrit() * 100, 2),
                Math.Round((1f - ChanceToLand()) * 100, 2));
        }

    }

    public class JudgementOfBlood : Skill
    {

        public JudgementOfBlood(CombatStats combats) : base(combats, AbilityType.Range, DamageType.Holy, true, true) { }

        public override float AbilityDamage()
        {
            if (CalcOpts.Mode32) return 0;
            else return (Combats.WeaponDamage * .26f + Stats.SpellPower * .18f + Stats.AttackPower * .11f)
                * (1f + .05f * Talents.TheArtOfWar + (Talents.GlyphOfJudgement ? 0.1f : 0f));
        }

        public override float AbilityCritChance()
        {
            return Talents.Fanaticism * .06f + Stats.JudgementCrit;
        }

    }

    public class JudgementOfCommand : Skill
    {

        public JudgementOfCommand(CombatStats combats) : base(combats, AbilityType.Range, DamageType.Holy, true, true) { }

        public override float AbilityDamage()
        {
            return (Combats.WeaponDamage * .19f + Stats.SpellPower * .13f + Stats.AttackPower * .08f)
                * (1f + .05f * Talents.TheArtOfWar + (Talents.GlyphOfJudgement ? 0.1f : 0f));
        }

        public override float AbilityCritChance()
        {
            return Talents.Fanaticism * .06f + Stats.JudgementCrit;
        }

    }

    public class JudgementOfRighteousness : Skill
    {

        public JudgementOfRighteousness(CombatStats combats) : base(combats, AbilityType.Range, DamageType.Holy, true, true) { }

        public override float AbilityDamage()
        {
            return (1f + Stats.SpellPower * .32f + Stats.AttackPower * .2f)
                * (1f + .05f * Talents.TheArtOfWar + .03f * Talents.SealsOfThePure + (Talents.GlyphOfJudgement ? 0.1f : 0f));
        }

        public override float AbilityCritChance()
        {
            return Talents.Fanaticism * .06f + Stats.JudgementCrit;
        }

    }

    public class JudgementOfVengeance : Skill
    {

        public JudgementOfVengeance(CombatStats combats) : base(combats, AbilityType.Range, DamageType.Holy, true, true) { }

        public override float AbilityDamage()
        {
            return (1.0f + Stats.SpellPower * 0.22f + Stats.AttackPower * 0.14f) * 1.5f
                * (1f + .05f * Talents.TheArtOfWar + .03f * Talents.SealsOfThePure + (Talents.GlyphOfJudgement ? 0.1f : 0f));
        }

        public override float AbilityCritChance()
        {
            return Talents.Fanaticism * .06f + Stats.JudgementCrit;
        }

    }

    public class CrusaderStrike : Skill
    {

        public CrusaderStrike(CombatStats combats) : base(combats, AbilityType.Melee, DamageType.Physical, true, true) { }

        public override float AbilityDamage()
        {
            return (Combats.NormalWeaponDamage * (CalcOpts.Mode32 ? .75f : 1.1f) + Stats.CrusaderStrikeDamage)
                * (1f + .05f * Talents.SanctityOfBattle + .05f * Talents.TheArtOfWar + Stats.CrusaderStrikeMultiplier);
        }

        public override float AbilityCritChance()
        {
            return Stats.CrusaderStrikeCrit;
        }

    }

    public class DivineStorm : Skill
    {

        public DivineStorm(CombatStats combats) : base(combats, AbilityType.Melee, DamageType.Physical, true, true) { }

        public override float AbilityDamage()
        {
            return (Combats.NormalWeaponDamage * 1.1f + Stats.DivineStormDamage)
                * (1f + .05f * Talents.TheArtOfWar + Stats.DivineStormMultiplier);
        }

        public override float AbilityCritChance()
        {
            return Stats.DivineStormCrit;
        }

        public override float Targets()
        {
            return (float)Math.Min(Combats.CalcOpts.Targets, 3f);
        }

    }

    public class HammerOfWrath : Skill
    {

        public HammerOfWrath(CombatStats combats) : base(combats, AbilityType.Range, DamageType.Holy, false, false) { }

        public override float AbilityDamage()
        {
            return (1198f + .15f * Stats.SpellPower + .15f * Stats.AttackPower)
                * (1f + Stats.HammerOfWrathMultiplier);
        }

        public override float AbilityCritChance()
        {
            return .25f * Talents.SanctifiedWrath;
        }

    }

    public class Exorcism : Skill
    {

        public Exorcism(CombatStats combats) : base(combats, AbilityType.Spell, DamageType.Holy, false, false) { }

        public override float AbilityDamage()
        {
            return (1087f + .42f * Stats.SpellPower)
                * (1f + .05f * Talents.SanctityOfBattle + Stats.ExorcismMultiplier + (Talents.GlyphOfExorcism ? 0.2f : 0f));
        }

        public override float AbilityCritChance()
        {
            return (CalcOpts.Mob == MobType.Demon || CalcOpts.Mob == MobType.Undead) ? 1f : 0;
        }

    }

    public class Consecration : Skill
    {

        public Consecration(CombatStats combats) : base(combats, AbilityType.Spell, DamageType.Holy, false, false) { }

        public override float AbilityDamage()
        {
            return (113f + .04f * (Stats.SpellPower + Stats.ConsecrationSpellPower) + .04f * Stats.AttackPower)
                * (Talents.GlyphOfConsecration ? 10f : 8f) * (CalcOpts.ConsEff);
        }

        public override float AbilityCritChance()
        {
            return -1f;
        }

        public override float Targets()
        {
            return Combats.CalcOpts.Targets;
        }

    }

    public class SealOfBlood : Skill
    {

        public SealOfBlood(CombatStats combats) : base(combats, AbilityType.Melee, DamageType.Holy, true, false) { }

        public override float AbilityDamage()
        {
            if (CalcOpts.Mode32) return 0;
            else return Combats.WeaponDamage * .48f;
        }

    }

    public class SealOfCommand : Skill
    {

        public SealOfCommand(CombatStats combats) : base(combats, AbilityType.Melee, DamageType.Holy, true, false) { }

        public override float AbilityDamage()
        {
            return CalcOpts.Mode32
                ? (Combats.WeaponDamage * .36f) 
                : ((Combats.WeaponDamage + Stats.SpellPower * .23f) * .45f);
        }

    }

    public class SealOfRighteousness : Skill
    {

        public SealOfRighteousness(CombatStats combats) : base(combats, AbilityType.Spell, DamageType.Holy, false, false) { }

        public override float AbilityDamage()
        {
            return Combats.BaseWeaponSpeed * (0.022f * Stats.AttackPower + 0.044f * Stats.SpellPower)
                * (1f + .03f * Talents.SealsOfThePure + (Talents.GlyphOfSealOfRighteousness ? 0.1f : 0f));
        }

        public override float AbilityCritChance() { return -1f; }
        public override float ChanceToLand() { return 1f; }

    }

    public class SealOfVengeanceDoT : Skill
    {

        public SealOfVengeanceDoT(CombatStats combats) : base(combats, AbilityType.Spell, DamageType.Holy, false, false) { }

        public override float AbilityDamage()
        {
            return 5f * (Stats.SpellPower * 0.065f + Stats.AttackPower * 0.13f) / 5f
                * (1f + .03f * Talents.SealsOfThePure);
        }

        public override float AbilityCritChance() { return -1f; }
        public override float ChanceToLand() { return 1f; }

    }

    public class SealOfVengeance : Skill
    {

        public SealOfVengeance(CombatStats combats) : base(combats, AbilityType.Melee, DamageType.Holy, true, false) { }

        public override float AbilityDamage()
        {
            return CalcOpts.Mode32
                ? (Combats.WeaponDamage * 0.33f) * (1f + Talents.SealsOfThePure * .03f)
                : 0f;
        }

        public override float ChanceToLand() { return 1f; }
    }  

    public class White : Skill
    {

        public White(CombatStats combats) : base(combats, AbilityType.Melee, DamageType.Physical, true, false) { }

        public override float AbilityDamage()
        {
            return Combats.WeaponDamage;
        }

        public override float AverageDamage()
        {
            const float glanceChance = .24f;
            const float glancingAmount = 1f - 0.35f;
            return HitDamage() *
                (glanceChance * glancingAmount +
                CritChance() * CritBonus() +
                (ChanceToLand() - CritChance() - glanceChance));
        }

        public float WhiteDPS()
        {
            return AverageDamage() / Combats.AttackSpeed;
        }

    }

    public class None : Skill
    {

        public None(CombatStats combats) : base(combats, AbilityType.Melee, DamageType.Holy, true, false) { }

        public override float AbilityDamage()
        {
            return 0;
        }

    }

    public class MagicDamage : Skill
    {
        private float amount;

        public MagicDamage(CombatStats combats, float amount)
            : base(combats, AbilityType.Spell, DamageType.Holy, false, false)
        {
            this.amount = amount;
        }

        public override float AbilityDamage()
        {
            return amount;
        }
    }

}
