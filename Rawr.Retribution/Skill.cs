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

        public virtual BaseCombatTable CT { get; protected set; }

        public AbilityType AbilityType { get; set; }
        public DamageType DamageType { get; set; }
        public float InqUptime { get; set; }

        public double UsagePerSec { get; set; }

        public float CritsPerSec()
        {
            return (float) (UsagePerSec * CT.ChanceToCrit);
        }

        public float GetDPS()
        {
            return (float) (AverageDamage() * UsagePerSec);
        }

        public virtual bool UsableBefore20PercentHealth
        {
            get { return true; }
        }

        public virtual bool UsableAfter20PercentHealth
        {
            get { return true; }
        }

        public virtual float GetCooldown()
        {
            return 1f;
        }

        public virtual float GetGCD()
        {
            return (AbilityType == AbilityType.Spell ? 
                        1.5f / (1 + _combats.Stats.SpellHaste) : 
                        1.5f);
        }

        public Skill(CombatStats combats, AbilityType abilityType, DamageType damageType)
        {
            Combats = combats;
            AbilityType = abilityType;
            DamageType = damageType;
        }

        public virtual float AverageDamage()
        {
            float hitdmg = HitDamage();
            float dmg = hitdmg * CT.ChanceToHit + 
                        hitdmg * CritBonus() * CT.ChanceToCrit;

            if (CT.GetType() == typeof(BasePhysicalWhiteCombatTable)) {
                dmg += hitdmg * .75f * ((BasePhysicalWhiteCombatTable)CT)._glanceChance +
                       hitdmg * .7f * ((BasePhysicalCombatTable)CT).ChanceToBlock;
            } else if (CT.GetType() == typeof(BasePhysicalYellowCombatTable)) {
                dmg -= dmg * .7f * ((BasePhysicalCombatTable)CT).ChanceToBlock;
            }
            return dmg * Targets();
        }

        public float CritBonus()
        {
            if (AbilityType == AbilityType.Spell)
            {
                return 1.5f * (1f + Stats.BonusSpellCritMultiplier);
            }
            else
            {
                return 2f * (1f + Stats.BonusCritMultiplier);
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
            else if (DamageType == DamageType.Holy) // Holy Damage
            {
                damage *= Combats.PartialResist;
                damage *= (1f + .3f * InqUptime + Stats.BonusHolyDamageMultiplier);
            }
            else if (DamageType == DamageType.HolyNDD)
            {
                damage *= (1f + .3f * InqUptime);
                damage /= (1f + Stats.BonusDamageMultiplier);
                damage /= Combats.AvengingWrathMulti;
            }
            else
            {
                damage *= Combats.PartialResist;
            }

            damage *= (1f + Stats.BonusDamageMultiplier);
            damage *= Combats.AvengingWrathMulti;
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
            string fmtstring = "{0:0} Average Damage\n{1:0} Average Hit\n{2,5:00.00%} Hit";
            object[] param = {AverageDamage(), HitDamage(), CT.ChanceToHit, 0f, 0f, 0f, 0f, 0f, 0f};

            if (CT.CanCrit) {
                fmtstring += "\n{3,5:00.00%} Crit";
                param[3] = CT.ChanceToCrit;
            }
            if (CT.CanMiss) {
                fmtstring += "\n{4,5:00.00%} Miss";
                param[4] = CT.ChanceToMiss;
            }
            if (CT.GetType() == typeof(BasePhysicalWhiteCombatTable)) {
                fmtstring += "\n{5,5:00.00%} Glance";
                param[5] = ((BasePhysicalWhiteCombatTable)CT)._glanceChance;
            }
            if (CT.GetType().IsSubclassOf(typeof(BasePhysicalCombatTable)))
            {
                fmtstring += (((BasePhysicalCombatTable)CT).CanDodge ? "\n{6,5:00.00%} Dodge" : "") +
                        (((BasePhysicalCombatTable)CT).CanParry ? "\n{7,5:00.00%} Parry" : "") +
                        (((BasePhysicalCombatTable)CT).CanBlock ? "\n{8,5:00.00%} Blocked Attacks" : "");
                param[6] = ((BasePhysicalCombatTable)CT).ChanceToDodge;
                param[7] = ((BasePhysicalCombatTable)CT).ChanceToParry;
                param[8] = ((BasePhysicalCombatTable)CT).ChanceToBlock;
            }

            return string.Format(fmtstring, param);
        }

    }
    
    public class Judgement : Skill
    {
        public Judgement(CombatStats combats) : base(combats, AbilityType.Range, DamageType.Holy) 
        {
            CT = new BasePhysicalYellowCombatTable(_stats, Attacktype.Ranged);
            CT.AbilityCritCorr = Talents.ArbiterOfTheLight * .06f;
        }

        public override float AbilityDamage()
        {
            return PaladinConstants.JUDGE_DMG;
        }

        public override float GetCooldown()
        {
            return PaladinConstants.JUDGE_COOLDOWN;
        }
    }

    public class JudgementOfRighteousness : Judgement
    {
        public JudgementOfRighteousness(CombatStats combats) : base(combats) { }

        public override float AbilityDamage()
        {
            return (base.AbilityDamage() + Stats.SpellPower * PaladinConstants.JOR_COEFF_SP + Stats.AttackPower * PaladinConstants.JOR_COEFF_AP)
                * (1f + (Talents.GlyphOfJudgement ? 0.1f : 0f) + Stats.JudgementMultiplier);
        }
    }

    public class JudgementOfTruth : Judgement
    {
        public JudgementOfTruth(CombatStats combats, float averageStack) : base(combats)
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
        public Inquisition(CombatStats combats) : base(combats, AbilityType.Spell, DamageType.Magic) { }

        public override float AbilityDamage()
        {
            return 0f;
        }

        public override float GetCooldown()
        {
            return 30f;
        }
    }

    public class TemplarsVerdict : Skill
    {
        public TemplarsVerdict(CombatStats combats) : base(combats, AbilityType.Melee, DamageType.Physical) 
        {
            CT = new BasePhysicalYellowCombatTable(_stats, Attacktype.MeleeMH);
            CT.AbilityCritCorr = Talents.ArbiterOfTheLight * .06f;
        }

        public override float AbilityDamage()
        {
            return (Combats.WeaponDamage * PaladinConstants.TV_THREE_STK)
                * (1f + .1f * Talents.Crusade + (Talents.GlyphOfTemplarsVerdict ? .15f : .0f) + Stats.TemplarsVerdictMultiplier);
        }
    }

    public class CrusaderStrike : Skill
    {
        public CrusaderStrike(CombatStats combats) : base(combats, AbilityType.Melee, DamageType.Physical) 
        { 
            CT = new BasePhysicalYellowCombatTable(Stats, Attacktype.MeleeMH);
            CT.AbilityCritCorr = Talents.RuleOfLaw * .05f +
                                 (Talents.GlyphOfCrusaderStrike ? .05f : 0) +
                                 Stats.CrusaderStrikeCrit;
        }

        public override float AbilityDamage()
        {
            return (Combats.NormalWeaponDamage * PaladinConstants.CS_DMG_BONUS)
                * (1f + .1f * Talents.Crusade + Stats.CrusaderStrikeMultiplier); //TODO: Determine how to calc
        }

        public override float GetCooldown()
        {
            return PaladinConstants.CS_COOLDOWN / (1f + _stats.PhysicalHaste) ;
        }
    }

    public class HandofLight : Skill
    {
        public HandofLight(CombatStats combats, float amountBefore) : base(combats, AbilityType.Spell, DamageType.HolyNDD) 
        {
            AmountBefore = amountBefore;
            CT = new BaseSpellCombatTable(_stats, Attacktype.Spell);
            CT.CanMiss = false;
            CT.CanCrit = false;
        }

        public float AmountBefore { get; set; }
        public override float AbilityDamage()
        {
            return AmountBefore * Combats.GetMasteryTotalPercent();
        }
    }

    public class DivineStorm : Skill
    {
        public DivineStorm(CombatStats combats) : base(combats, AbilityType.Melee, DamageType.Physical) 
        {
            CT = new BasePhysicalYellowCombatTable(_stats, Attacktype.MeleeMH);
        }

        public override float AbilityDamage()
        {
            return (Combats.NormalWeaponDamage * PaladinConstants.DS_DMG_BONUS);
        }
        
        public override float Targets()
        {
            return (float)Math.Min(0f/*//TODO: Get it form Bosshandler Combats.CalcOpts.Targets*/, 3f);
        }

        public override float GetCooldown()
        {
            return PaladinConstants.DS_COOLDOWN / (1f + _stats.PhysicalHaste);
        }
    }
    
    public class HammerOfWrath : Skill
    {
        public HammerOfWrath(CombatStats combats) : base(combats, AbilityType.Range, DamageType.Holy) 
        {
            CT = new BasePhysicalYellowCombatTable(_stats, Attacktype.Ranged);
            CT.AbilityCritCorr = Talents.SanctifiedWrath * .2f;
        }

        public override bool UsableBefore20PercentHealth
        {
            get { return false; }
        }

        public override float AbilityDamage()
        {
            return (PaladinConstants.HOW_AVG_DMG + PaladinConstants.HOW_COEFF_SP * Stats.SpellPower + PaladinConstants.HOW_COEFF_AP * Stats.AttackPower) 
                * (1f + Stats.HammerOfWrathMultiplier);
        }

        public override float GetCooldown()
        {
            return PaladinConstants.HOW_COOLDOWN;
        }
    }

    public class Exorcism : Skill
    {
        public Exorcism(CombatStats combats) : base(combats, AbilityType.Spell, DamageType.Holy) 
        {
            CT = new BaseSpellCombatTable(_stats, Attacktype.Spell);
            CT.AbilityCritCorr = (Combats.Character.BossOptions.MobType == (int)MOB_TYPES.DEMON || Combats.Character.BossOptions.MobType == (int)MOB_TYPES.UNDEAD) ? 1f : 0;
        }

        public override float AbilityDamage()
        {
            return (PaladinConstants.EXO_AVG_DMG + PaladinConstants.EXO_COEFF * Math.Max(Stats.SpellPower, Stats.AttackPower))
                * (1f + (Talents.TheArtOfWar > 0 ? 1f : 0f) + .1f * Talents.BlazingLight + Stats.ExorcismMultiplier);
        }

        public override float GetCooldown()
        {
            return Combats.AttackSpeed * (1f / (Combats.Talents.TheArtOfWar * PaladinConstants.EXO_PROC_CHANCE));
        }
    }

    public class HolyWrath : Skill
    {
        public HolyWrath(CombatStats combats) : base(combats, AbilityType.Spell, DamageType.Holy) 
        {
            CT = new BaseSpellCombatTable(_stats, Attacktype.Spell);
            CT.AbilityCritCorr = (Combats.Character.BossOptions.MobType == (int)MOB_TYPES.DEMON || Combats.Character.BossOptions.MobType == (int)MOB_TYPES.UNDEAD) ? 1f : 0;
        }

        public override float AbilityDamage()
        {
            return (PaladinConstants.HOLY_WRATH_BASE_DMG + PaladinConstants.HOLY_WRATH_COEFF * Stats.SpellPower);
        }
    
        public override float Targets()
        {
            return 1f / 1f /*//TODO: Get it from Bosshandler Combats.CalcOpts.Targets*/;
        }

        public override float GetCooldown()
        {
            return PaladinConstants.HOW_COOLDOWN;
        }
    }

    public class Consecration : Skill
    {
        public Consecration(CombatStats combats) : base(combats, AbilityType.Spell, DamageType.Holy) 
        {
            CT = new BaseSpellCombatTable(_stats, Attacktype.Spell);
            CT.CanCrit = false;
        }

        public override float AbilityDamage()
        {
            return (PaladinConstants.CONS_BASE_DMG + PaladinConstants.CONS_COEFF_SP * (Stats.SpellPower + Stats.ConsecrationSpellPower) + PaladinConstants.CONS_COEFF_AP * Stats.AttackPower)
                / 10 * TickCount();
        }

        public override float Targets()
        {
            return 1f /*//TODO: get it from Bosshandler Combats.CalcOpts.Targets*/;
        }

        public override float TickCount()
        {
            // Every second for 10 seconds (12 seconds with glyph)
            return Talents.GlyphOfConsecration ? 10f : 12f;
        }

        public override float GetCooldown()
        {
            return PaladinConstants.CONS_COOLDOWN;
        }
    }

    public class SealOfCommand : Skill
    {
        public SealOfCommand(CombatStats combats) : base(combats, AbilityType.Melee, DamageType.Holy) 
        {
            CT = new BasePhysicalYellowCombatTable(_stats, Attacktype.MeleeMH);
        }

        public override float AbilityDamage()
        {
            return (Combats.WeaponDamage * PaladinConstants.SOC_COEFF) 
                * (1f + .06f * Talents.SealsOfThePure + Stats.SealMultiplier);
        }
    }

    public class SealOfRighteousness : Skill
    {
        public SealOfRighteousness(CombatStats combats) : base(combats, AbilityType.Spell, DamageType.Holy) 
        {
            CT = new BaseSpellCombatTable(_stats, Attacktype.Spell);
            CT.CanCrit = false;
            CT.CanMiss = false;
        }

        public override float AbilityDamage()
        {
            return Combats.BaseWeaponSpeed * (PaladinConstants.SOR_COEFF_AP * Stats.AttackPower + PaladinConstants.SOR_COEFF_SP * Stats.SpellPower) 
                * (1f + .06f * Talents.SealsOfThePure);
        }

        public override float Targets()
        {
            return 1f; //TODO: Add additional target (Talents.SealsOfCommand > 0 ? PaladinConstants.SOR_ADDTARGET : 1f);
        }
    }

    public class SealOfTruth : Skill
    {
        public SealOfTruth(CombatStats combats) : base(combats, AbilityType.Melee, DamageType.Holy) 
        {
            CT = new BaseSpellCombatTable(_stats, Attacktype.Spell);
            CT.CanMiss = false;
        }

        public override float AbilityDamage()
        {
            return (Combats.WeaponDamage * PaladinConstants.SOT_SEAL_COEFF) 
                * (1f + .06f * Talents.SealsOfThePure);
        }
    }  

    public class SealOfTruthDoT : Skill
    {
        public SealOfTruthDoT(CombatStats combats, float averageStack) : base(combats, AbilityType.Spell, DamageType.Holy)
        {
            AverageStackSize = averageStack;
            CT = new BaseSpellCombatTable(_stats, Attacktype.Spell);
            CT.CanMiss = false;
        }

        public float AverageStackSize { get; private set; }

        public override float AbilityDamage()
        {
            return AverageStackSize * (Stats.SpellPower * PaladinConstants.SOT_CENSURE_COEFF_SP + Stats.AttackPower * PaladinConstants.SOT_CENSURE_COEFF_AP) 
                * (1f + .06f * Talents.SealsOfThePure 
                      + .1f * Talents.InquiryOfFaith);
        }
    }
    
    public class White : Skill
    {
        public White(CombatStats combats) : base(combats, AbilityType.Melee, DamageType.Physical) 
        {
            CT = new BasePhysicalWhiteCombatTable(_stats, Attacktype.MeleeMH);
        }

        public override float AbilityDamage()
        {
            return Combats.WeaponDamage;
        }

        public float WhiteDPS()
        {
            return AverageDamage() / Combats.AttackSpeed;
        }
    }

    #region NULL Things
    public class NullSeal : Skill
    {
        public NullSeal(CombatStats combats) : base(combats, AbilityType.Melee, DamageType.Holy) 
        {
            CT = new BasePhysicalYellowCombatTable(_stats, Attacktype.MeleeMH);
        }

        public override float AbilityDamage()
        {
            return 0;
        }
    }

    public class NullSealDoT : Skill
    {
        public NullSealDoT(CombatStats combats) : base(combats, AbilityType.Melee, DamageType.Holy) 
        {
            CT = new BasePhysicalYellowCombatTable(_stats, Attacktype.MeleeMH);
        }

        public override float AbilityDamage()
        {
            return 0;
        }
    }

    public class NullJudgement : Skill
    {
        public NullJudgement(CombatStats combats) : base(combats, AbilityType.Melee, DamageType.Holy) 
        {
            CT = new BasePhysicalYellowCombatTable(_stats, Attacktype.Ranged);
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
    #endregion

    public class MagicDamage : Skill
    {
        private float amount;

        public MagicDamage(CombatStats combats, float amount) : base(combats, AbilityType.Spell, DamageType.Magic)
        {
            this.amount = amount;
            CT = new BaseSpellCombatTable(_stats, Attacktype.Spell);
        }

        public override float AbilityDamage()
        {
            return amount;
        }
    }

}
