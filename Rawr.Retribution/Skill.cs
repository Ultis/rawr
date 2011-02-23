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

        public virtual bool UsableBefore20PercentHealth
        {
            get { return true; }
        }

        public virtual bool UsableAfter20PercentHealth
        {
            get { return true; }
        }

        public virtual Ability? RotationAbility
        {
            get { return null; }
        }


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
                return 1 - (Combats.GetMeleeMissChance() + Combats.GetToBeDodgedChance() + Combats.GetToBeParriedChance() * _calcOpts.InFront);
            }
            else if (AbilityType == AbilityType.Range)
            {
                return 1 - Combats.GetRangedMissChance();
            }
            else // Spell
            {
                return 1 - Combats.GetSpellMissChance();
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
#if RAWR4
                    rightVen += .1f * 0f * (1f + Stats.PhysicalCrit);
#else
                    rightVen += .1f * Talents.RighteousVengeance * (1f + Stats.PhysicalCrit);
#endif
                }
                else
                {
#if RAWR4
                    rightVen += .1f * 0f;
#else
                    rightVen += .1f * Talents.RighteousVengeance;
#endif
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

            if (DamageType != DamageType.Magic) damage *= 1f + .03f * 0; // Talents.Vengeance
            if (UsesWeapon) damage *= 1f + .02f * 0;

            damage *= (1f + .01f * Talents.Crusade);
            if (CalcOpts.Mob != MobType.Other) damage *= (1f + .01f * Talents.Crusade);
            damage *= Combats.AvengingWrathMulti;
            // damage *= (Talents.GlyphOfSenseUndead && CalcOpts.Mob == MobType.Undead ? 1.01f : 1f); // 11/7/10 roncli - Removed calculations for removed glyph of sense undead
            return damage;
        }

        public abstract float AbilityDamage();

        public virtual float AbilityCritChance() { return 0; }
        public virtual float Targets() { return 1f; }

        public virtual float TickCount()
        {
            return 1;
        }

        public override string ToString()
        {
            return string.Format("Average Damage: {0}\nAverage Hit: {1}\nCrit Chance: {2}%\nAvoid Chance: {3}%",
                AverageDamage().ToString("N0"),
                HitDamage().ToString("N0"),
                Math.Round(ChanceToCrit() * 100, 2),
                Math.Round((1f - ChanceToLand()) * 100, 2));
        }

    }
    
    public class Judgement : Skill
    {
        public Judgement(CombatStats combats) 
            : base(combats, AbilityType.Range, DamageType.Holy, true, true) { }

        public override Ability? RotationAbility
        {
            get { return Ability.Judgement; }
        }

        public override float AbilityDamage()
        {
            return PaladinConstants.JUDGE_DMG;
        }

        public override float AbilityCritChance()
        {
            return Talents.ArbiterOfTheLight * .06f;
        }
    }

    public class JudgementOfRighteousness : Judgement
    {
        public JudgementOfRighteousness(CombatStats combats)
            : base(combats) { }

        public override float AbilityDamage()
        {
            return (base.AbilityDamage() + Combats.BaseWeaponSpeed * Stats.SpellPower * PaladinConstants.JOR_COEFF_SP + Stats.AttackPower * PaladinConstants.JOR_COEFF_AP)
                * (1f + (Talents.GlyphOfJudgement ? 0.1f : 0f) + Stats.JudgementMultiplier);
        }
    }

    public class JudgementOfTruth : Judgement
    {
        public JudgementOfTruth(CombatStats combats, float averageStack)
            : base(combats)
        {
            AverageStackSize = averageStack;
        }

        public float AverageStackSize { get; private set; }

        public override float AbilityDamage()
        {
            return (base.AbilityDamage() + Stats.SpellPower * PaladinConstants.JOT_JUDGE_COEFF_SP + Stats.AttackPower * PaladinConstants.JOT_JUDGE_COEFF_AP) 
                * (1f + PaladinConstants.JOT_JUDGE_COEFF_STACK * AverageStackSize)
                * (1f + (Talents.GlyphOfJudgement ? 0.1f : 0f) + Stats.JudgementMultiplier);
        }
    }

    public class Inquisition : Skill
    {
        public Inquisition(CombatStats combats)
            : base(combats, AbilityType.Spell, DamageType.Magic, false, false) { }

        public override Ability? RotationAbility
        {
            get { return Ability.Inquisition; }
        }

        public override float AbilityDamage()
        {
            return 0f;
        }
    }

    public class TemplarsVerdict : Skill
    {
        public TemplarsVerdict(CombatStats combats)
            : base(combats, AbilityType.Melee, DamageType.Physical, true, true) { }

        public override Ability? RotationAbility
        {
            get { return Ability.TemplarsVerdict; }

        }

        public override float AbilityDamage()
        {
            return (Combats.NormalWeaponDamage * PaladinConstants.TV_TWO_STK)
                * (1f + .1f * Talents.Crusade + (Talents.GlyphOfTemplarsVerdict ? .15f : .0f));
        }

        public override float AbilityCritChance()
        {
            return Talents.ArbiterOfTheLight * .06f;
        }
    }

    public class CrusaderStrike : Skill
    {
        public CrusaderStrike(CombatStats combats) 
            : base(combats, AbilityType.Melee, DamageType.Physical, true, true) { }

        public override Ability? RotationAbility
        {
            get { return Ability.CrusaderStrike; }
        }

        public override float AbilityDamage()
        {
            return (Combats.NormalWeaponDamage * PaladinConstants.CS_DMG_BONUS)
                * (1f + .1f * Talents.Crusade + Stats.CrusaderStrikeMultiplier);
        }

        public override float AbilityCritChance()
        {
            return .5f * Talents.RuleOfLaw + Stats.CrusaderStrikeCrit;
        }
    }

    public class HandofLight : Skill
    {
        public HandofLight(CombatStats combats, float amountBefore) 
            : base(combats, AbilityType.Spell, DamageType.Holy, false, false) 
        {
            AmountBefore = amountBefore;
        }

        public float AmountBefore { get; set; }

        public override float AbilityDamage()
        {
            return AmountBefore * Combats.GetMasteryTotalPercent();
        }

        public override float AbilityCritChance()
        {
            return -1f;//Can't crit
        }
    }

    public class DivineStorm : Skill
    {
        public DivineStorm(CombatStats combats) 
            : base(combats, AbilityType.Melee, DamageType.Physical, true, true) { }

        public override Ability? RotationAbility
        {
            get { return Ability.DivineStorm; }
        }

        public override float AbilityDamage()
        {
            return (Combats.NormalWeaponDamage * PaladinConstants.DS_DMG_BONUS);
        }
        
        public override float Targets()
        {
            return (float)Math.Min(Combats.CalcOpts.Targets, 3f);
        }
    }
    
    public class HammerOfWrath : Skill
    {
        public HammerOfWrath(CombatStats combats) 
            : base(combats, AbilityType.Range, DamageType.Holy, false, false) { }

        public override Ability? RotationAbility
        {
            get { return Ability.HammerOfWrath; }
        }

        public override bool UsableBefore20PercentHealth
        {
            get { return false; }
        }

        public override float AbilityDamage()
        {
            return (PaladinConstants.HOW_AVG_DMG + PaladinConstants.HOW_COEFF_SP * Stats.SpellPower + PaladinConstants.HOW_COEFF_AP * Stats.AttackPower);
        }

        public override float AbilityCritChance()
        {
            return .2f * Talents.SanctifiedWrath;
        }
    }

    public class Exorcism : Skill
    {
        public Exorcism(CombatStats combats) 
            : base(combats, AbilityType.Spell, DamageType.Holy, false, false) { }

        public override Ability? RotationAbility
        {
            get { return Ability.Exorcism; }
        }

        public override float AbilityDamage()
        {
            return (PaladinConstants.EXO_AVG_DMG + PaladinConstants.EXO_COEFF_SP * Stats.SpellPower)
                * (1f + (Talents.TheArtOfWar > 0 ? 1f : 0f)
                      + .1f * Talents.BlazingLight);
        }

        public override float AbilityCritChance()
        {
            return (CalcOpts.Mob == MobType.Demon || CalcOpts.Mob == MobType.Undead) ? 1f : 0;
        }
    }

    public class HolyWrath : Skill
    {
        public HolyWrath(CombatStats combats)
            : base(combats, AbilityType.Spell, DamageType.Holy, false, false) { }

        public override Ability? RotationAbility
        {
            get { return Ability.HolyWrath; }
        }

        public override float AbilityDamage()
        {
            return (PaladinConstants.HOLY_WRATH_BASE_DMG + PaladinConstants.HOLY_WRATH_COEFF * Stats.SpellPower);
        }
        
        public override float AbilityCritChance()
        {
            return (CalcOpts.Mob == MobType.Demon || CalcOpts.Mob == MobType.Undead) ? 1f : 0;
        }

        public override float Targets()
        {
            return 1f / Combats.CalcOpts.Targets;
        }
    }

    public class Consecration : Skill
    {
        public Consecration(CombatStats combats) 
            : base(combats, AbilityType.Spell, DamageType.Holy, false, false) { }

        public override Ability? RotationAbility
        {
            get { return Ability.Consecration; }
        }

        public override float AbilityDamage()
        {
            return (PaladinConstants.CONS_BASE_DMG + PaladinConstants.CONS_COEFF_SP * (Stats.SpellPower + Stats.ConsecrationSpellPower) + PaladinConstants.CONS_COEFF_AP * Stats.AttackPower)
                * TickCount() * (CalcOpts.ConsEff);
        }

        public override float AbilityCritChance()
        {
            return -1f; // -1 = can't crit.
        }

        public override float Targets()
        {
            return Combats.CalcOpts.Targets;
        }

        public override float TickCount()
        {
            // Every second for 10 seconds (12 seconds with glyph)
            return Talents.GlyphOfConsecration ? 10f : 12f;
        }
    }

    public class SealOfCommand : Skill
    {
        public SealOfCommand(CombatStats combats) 
            : base(combats, AbilityType.Melee, DamageType.Holy, true, false) { }

        public override float AbilityDamage()
        {
            return (Combats.WeaponDamage * PaladinConstants.SOC_COEFF) * (1f + Stats.SealMultiplier);
        }
    }

    public class SealOfRighteousness : Skill
    {
        public SealOfRighteousness(CombatStats combats) 
            : base(combats, AbilityType.Spell, DamageType.Holy, true, false) { }

        public override float AbilityDamage()
        {
            return Combats.BaseWeaponSpeed * (PaladinConstants.SOR_COEFF_AP * Stats.AttackPower + PaladinConstants.SOR_COEFF_SP * Stats.SpellPower) 
                * (1f + .06f * Talents.SealsOfThePure);
        }

        public override float AbilityCritChance() { return -1f; }
        public override float ChanceToLand() { return 1f; }

        public override float Targets()
        {
            return (Talents.SealsOfCommand > 0 ? PaladinConstants.SOR_ADDTARGET : 1f);
        }
    }

    public class SealOfTruth : Skill
    {
        public SealOfTruth(CombatStats combats, float averageStack)
            : base(combats, AbilityType.Melee, DamageType.Holy, true, false)
        {
            AverageStackSize = averageStack;
        }

        public float AverageStackSize { get; private set; }

        public override float AbilityDamage()
        {
            return (Combats.WeaponDamage * PaladinConstants.SOT_SEAL_COEFF) 
                * (1f + .06f * Talents.SealsOfThePure);
        }

        public override float ChanceToLand() { return 1f; }
    }  

    public class SealOfTruthDoT : Skill
    {
        public SealOfTruthDoT(CombatStats combats, float averageStack)
            : base(combats, AbilityType.Spell, DamageType.Holy, false, false)
        {
            AverageStackSize = averageStack;
        }

        public float AverageStackSize { get; private set; }

        public override float AbilityDamage()
        {
            return AverageStackSize * (Stats.SpellPower * PaladinConstants.SOT_CENSURE_COEFF_SP + Stats.AttackPower * PaladinConstants.SOT_CENSURE_COEFF_AP) 
                * (1f + .06f * Talents.SealsOfThePure 
                      + .1f * Talents.InquiryOfFaith);
        }

        public override float ChanceToLand() { return 1f; }
    }
    
    public class White : Skill
    {
        public White(CombatStats combats) 
            : base(combats, AbilityType.Melee, DamageType.Physical, true, false) { }

        public override float AbilityDamage()
        {
            return Combats.WeaponDamage;
        }

        public override float AverageDamage()
        {
            const float glanceChance = .24f;
            const float glancingAmount = 1f - 0.25f;
            return HitDamage() * (1f + Stats.BonusWhiteDamageMultiplier) *
                (glanceChance * glancingAmount +
                CritChance() * CritBonus() +
                (ChanceToLand() - CritChance() - glanceChance));
        }

        public float WhiteDPS()
        {
            return AverageDamage() / Combats.AttackSpeed;
        }
    }
    
    public class NullSeal : Skill
    {
        public NullSeal(CombatStats combats) 
            : base(combats, AbilityType.Melee, DamageType.Holy, true, false) { }

        public override float AbilityDamage()
        {
            return 0;
        }
    }

    public class NullSealDoT : Skill
    {
        public NullSealDoT(CombatStats combats) 
            : base(combats, AbilityType.Melee, DamageType.Holy, true, false) { }

        public override float AbilityDamage()
        {
            return 0;
        }
    }

    public class NullJudgement : Skill
    {
        public NullJudgement(CombatStats combats) : base(combats, AbilityType.Melee, DamageType.Holy, true, false) { }

        public override Ability? RotationAbility
        {
            get { return Ability.Judgement; }
        }

        public override bool UsableBefore20PercentHealth
        {
            get { return false; }
        }

        public override bool UsableAfter20PercentHealth
        {
            get { return false; }
        }


        public override float AbilityDamage()
        {
            return 0;
        }
    }

    public class MagicDamage : Skill
    {
        private float amount;

        public MagicDamage(CombatStats combats, float amount)
            : base(combats, AbilityType.Spell, DamageType.Magic, false, false)
        {
            this.amount = amount;
        }

        public override float AbilityDamage()
        {
            return amount;
        }
    }

}
