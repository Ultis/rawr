using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Retribution
{
    public abstract class Skill
    {
        public Skill(CombatStats combats, AbilityType abilityType, DamageType damageType)
        {
            Combats = combats;
            AbilityType = abilityType;
            DamageType = damageType;
        }

        protected Stats _stats;
        public Stats Stats { get { return _stats; } }

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
                _talents = value.Talents;
            }
        }

        public virtual BaseCombatTable CT { get; protected set; }

        public AbilityType AbilityType { get; set; }
        private DamageType _damageType;
        public DamageType DamageType { get { return _damageType; }
            set {
                _damageType = value;
                switch (value)
                {
                    case DamageType.Physical:
                        AbilityDamageMulitplier[Multiplier.Physical] = (1f + Stats.BonusDamageMultiplier) * (1f + Stats.BonusPhysicalDamageMultiplier);
                        AbilityDamageMulitplier[Multiplier.Armor] = Combats.ArmorReduction;
                        AbilityDamageMulitplier[Multiplier.Others] = (1f + PaladinConstants.TWO_H_SPEC);
                        break;
                    case DamageType.Holy:
                        AbilityDamageMulitplier[Multiplier.Magical] = (1f + Stats.BonusDamageMultiplier) * (1f + Stats.BonusHolyDamageMultiplier) * (1f + _inqUptime * PaladinConstants.INQ_COEFF);
                        break;
                    case DamageType.HolyNDD:
                        AbilityDamageMulitplier[Multiplier.Magical] = 1f + _inqUptime * PaladinConstants.INQ_COEFF;
                        break;
                }
            } 
        }
        private float _inqUptime = 1f;
        public float InqUptime { get { return _inqUptime; } 
            set {
                if (_damageType == DamageType.Holy || _damageType == DamageType.HolyNDD) {
                    AbilityDamageMulitplier[Multiplier.Magical] /= _inqUptime * PaladinConstants.INQ_COEFF + 1f;
                    AbilityDamageMulitplier[Multiplier.Magical] *= value * PaladinConstants.INQ_COEFF + 1f;
                }
                _inqUptime = value;
            }
        }

        public double UsagePerSec { get; set; }
        public float CritsPerSec()
        {
            return (float) (UsagePerSec * CT.ChanceToCrit);
        }
        
        public virtual bool UsableBefore20PercentHealth { get { return true; } }
        public virtual bool UsableAfter20PercentHealth { get { return true; } }

        public virtual float GetCooldown() { return 1f; }
        public virtual float GetGCD()
        {
            return (AbilityType == AbilityType.Spell ? 
                        1.5f / (1 + _combats.Stats.SpellHaste) : 
                        1.5f);
        }

        public abstract float AbilityDamage();
        public virtual float AverageDamage()
        {
            float dmg = HitDamage() * CT.CombatTableMultiplier();
            return dmg * Targets();
        }
        public float HitDamage() { return AbilityDamage() * GetMulitplier(); }

        public Dictionary<Multiplier, float> AbilityDamageMulitplier = new Dictionary<Multiplier, float>();
        public virtual float GetMulitplier()
        {
            float multiplier = 1f;
            foreach (KeyValuePair<Multiplier, float> kvp in AbilityDamageMulitplier)
            {
                multiplier *= kvp.Value;
            }
            return multiplier;
        }
        private string GetMultiplierOutput()
        {
            string Output = "\n\nMultiplier:";
            foreach (KeyValuePair<Multiplier, float> kvp in AbilityDamageMulitplier)
            {
                if (kvp.Value != 1f)
                {
                    Output += string.Format("\n{0:0.00} {1,-10}", kvp.Value, kvp.Key);
                }
            }
            return Output + string.Format("\n{0:0.00} Total Multiplier", GetMulitplier());
        }

        public float CritBonus()
        {
            if (AbilityType == AbilityType.Spell)
                return 1.5f * (1f + Stats.BonusSpellCritMultiplier);
            else
                return 2f * (1f + Stats.BonusCritMultiplier);
        }

        public virtual float Targets() { return 1f; }
        public virtual float TickCount() { return 1; }

        public float GetDPS()
        {
            return (float)(AverageDamage() * UsagePerSec);
        }

        public override string ToString()
        {
            string fmtstring = "{0:0} Average Damage\n{1:0} Average Hit" + CT.ToString() + GetMultiplierOutput();
            object[] param = {AverageDamage(), HitDamage()};
            return string.Format(fmtstring, param);
        }
    }
    
    public class Judgement : Skill
    {
        public Judgement(CombatStats combats) : base(combats, AbilityType.Range, DamageType.Holy) 
        {
            CT = new BasePhysicalYellowCombatTable(combats.Character.BossOptions, _stats, Attacktype.Ranged);
            CT.AbilityCritCorr = Talents.ArbiterOfTheLight * PaladinConstants.ARBITER_OF_THE_LIGHT + _stats.JudgementCrit;
            AbilityDamageMulitplier[Multiplier.Glyphs] = (1f + (Talents.GlyphOfJudgement ? PaladinConstants.GLYPH_OF_JUDGEMENT : 0f));
            AbilityDamageMulitplier[Multiplier.Others] = (1f + PaladinConstants.TWO_H_SPEC);
            AbilityDamageMulitplier[Multiplier.Sets] = (1f + Stats.JudgementMultiplier);
        }

        public override float AbilityDamage()
        {
            return PaladinConstants.JUDGE_DMG;
        }

        public override float GetCooldown()
        {
            return PaladinConstants.JUDGE_COOLDOWN - _stats.JudgementCDReduction;
        }
    }

    public class JudgementOfRighteousness : Judgement
    {
        public JudgementOfRighteousness(CombatStats combats) : base(combats) { }

        public override float AbilityDamage()
        {
            return base.AbilityDamage() + 
                   Stats.SpellPower * PaladinConstants.JOR_COEFF_SP + 
                   Stats.AttackPower * PaladinConstants.JOR_COEFF_AP;
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
            return (base.AbilityDamage() + 
                        Stats.SpellPower * PaladinConstants.JOT_JUDGE_COEFF_SP + 
                        Stats.AttackPower * PaladinConstants.JOT_JUDGE_COEFF_AP)
                   * (1f + PaladinConstants.JOT_JUDGE_COEFF_STACK * AverageStackSize);
        }
    }

    public class Inquisition : Skill
    {
        public Inquisition(CombatStats combats, int hp) : base(combats, AbilityType.Spell, DamageType.Magic) { HP = hp; }
        private int HP;
        public override float AbilityDamage() { return 0f; }
        public float Duration { get { return (HP + Combats.Stats.BonusRet_T11_P4_InqHP) * 4f * (1f + Combats.Talents.InquiryOfFaith * PaladinConstants.INQUIRY_OF_FAITH_INQ); } }
        public override float GetCooldown() { return Duration; }
    }

    public class TemplarsVerdict : Skill
    {
        public TemplarsVerdict(CombatStats combats) : base(combats, AbilityType.Melee, DamageType.Physical) 
        {
            CT = new BasePhysicalYellowCombatTable(combats.Character.BossOptions, _stats, Attacktype.MeleeMH);
            CT.AbilityCritCorr = Talents.ArbiterOfTheLight * PaladinConstants.ARBITER_OF_THE_LIGHT;
            AbilityDamageMulitplier[Multiplier.Talents] = (1f + PaladinConstants.CRUSADE * Talents.Crusade);
            AbilityDamageMulitplier[Multiplier.Glyphs] = (1f + (Talents.GlyphOfTemplarsVerdict ? PaladinConstants.GLYPH_OF_TEMPLARS_VERDICT : .0f)); 
            AbilityDamageMulitplier[Multiplier.Sets] = (1f + Stats.TemplarsVerdictMultiplier); 
        }

        public override float AbilityDamage()
        {
            return Combats.WeaponDamage * PaladinConstants.TV_THREE_STK;
        }

        public override float GetMulitplier()
        {
            float multiplier = 1f;
            float add = 1f;
            foreach (KeyValuePair<Multiplier, float> kvp in AbilityDamageMulitplier)
            { //Additive not multiplicative for TV
                if (kvp.Key == Multiplier.Glyphs ||
                    kvp.Key == Multiplier.Talents ||
                    kvp.Key == Multiplier.Sets)
                    add += kvp.Value - 1f;
                else
                    multiplier *= kvp.Value;
            }
            return multiplier * add;
        }
    }

    public class CrusaderStrike : Skill
    {
        public CrusaderStrike(CombatStats combats) : base(combats, AbilityType.Melee, DamageType.Physical) 
        {
            CT = new BasePhysicalYellowCombatTable(combats.Character.BossOptions, Stats, Attacktype.MeleeMH);
            CT.AbilityCritCorr = Talents.RuleOfLaw * PaladinConstants.RULE_OF_LAW +
                                 (Talents.GlyphOfCrusaderStrike ? PaladinConstants.GLYPH_OF_CRUSADER_STRIKE : 0) +
                                 Stats.CrusaderStrikeCrit;
            AbilityDamageMulitplier[Multiplier.Talents] = (1f + PaladinConstants.CRUSADE * Talents.Crusade);
            AbilityDamageMulitplier[Multiplier.Sets] = (1f + Stats.CrusaderStrikeMultiplier);
        }

        public override float AbilityDamage()
        {
            return Combats.WeaponDamageNormalized * PaladinConstants.CS_DMG_BONUS + _stats.CrusaderStrikeDamage;
        }

        public override float GetCooldown()
        {
            return PaladinConstants.CS_COOLDOWN / (Combats.Talents.SanctityOfBattle > 0 ? (1f + _stats.PhysicalHaste) : 1f);
        }
    }

    public class HandofLight : Skill
    {
        public HandofLight(CombatStats combats, float amountBefore) : base(combats, AbilityType.Spell, DamageType.HolyNDD) 
        {
            AmountBefore = amountBefore;
            CT = new BaseSpellCombatTable(combats.Character.BossOptions, _stats, Attacktype.Spell);
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
            CT = new BasePhysicalYellowCombatTable(combats.Character.BossOptions, _stats, Attacktype.MeleeMH);
            CT.AbilityCritCorr = _stats.DivineStormCrit;
            AbilityDamageMulitplier[Multiplier.Sets] = (1f + _stats.DivineStormMultiplier);
        }

        public override float AbilityDamage()
        {
            return Combats.WeaponDamageNormalized * PaladinConstants.DS_DMG_BONUS + _stats.DivineStormDamage;
        }
        
        public override float Targets()
        {
            return (float)Math.Min(0f/*//TODO: Get it form Bosshandler Combats.CalcOpts.Targets*/, 3f);
        }

        public override float GetCooldown()
        {
            return PaladinConstants.DS_COOLDOWN / (Combats.Talents.SanctityOfBattle > 0 ? (1f + _stats.PhysicalHaste) : 1f);
        }
    }
    
    public class HammerOfWrath : Skill
    {
        public HammerOfWrath(CombatStats combats) : base(combats, AbilityType.Range, DamageType.Holy) 
        {
            CT = new BasePhysicalYellowCombatTable(combats.Character.BossOptions, _stats, Attacktype.Ranged);
            CT.AbilityCritCorr = Talents.SanctifiedWrath * PaladinConstants.SANCTIFIED_WRATH;
            AbilityDamageMulitplier[Multiplier.Sets] = (1f + Stats.HammerOfWrathMultiplier);
        }

        public override bool UsableBefore20PercentHealth { get { return false; } }
        public override float AbilityDamage()
        {
            return PaladinConstants.HOW_AVG_DMG + 
                   PaladinConstants.HOW_COEFF_SP * Stats.SpellPower + 
                   PaladinConstants.HOW_COEFF_AP * Stats.AttackPower;
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
            CT = new BaseSpellCombatTable(combats.Character.BossOptions, _stats, Attacktype.Spell);
            CT.AbilityCritCorr = (Combats.Character.BossOptions.MobType == (int)MOB_TYPES.DEMON || Combats.Character.BossOptions.MobType == (int)MOB_TYPES.UNDEAD) ? 1f : 0;
            AbilityDamageMulitplier[Multiplier.Talents] = (1f + (Talents.TheArtOfWar > 0 ? PaladinConstants.THE_ART_OF_WAR : 0f) +
                                                                PaladinConstants.BLAZING_LIGHT * Talents.BlazingLight);
            AbilityDamageMulitplier[Multiplier.Glyphs] = (1f + (Talents.GlyphOfExorcism ? PaladinConstants.GLYPH_OF_EXORCISM : 0f));
            AbilityDamageMulitplier[Multiplier.Sets] = (1f + Stats.ExorcismMultiplier);
        }

        public override float AbilityDamage()
        {
            return PaladinConstants.EXO_AVG_DMG +
                   PaladinConstants.EXO_COEFF * Math.Max(Stats.SpellPower, Stats.AttackPower);
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
            CT = new BaseSpellCombatTable(combats.Character.BossOptions, _stats, Attacktype.Spell);
            CT.AbilityCritCorr = (Combats.Character.BossOptions.MobType == (int)MOB_TYPES.DEMON || Combats.Character.BossOptions.MobType == (int)MOB_TYPES.UNDEAD) ? 1f : 0;
        }

        public override float AbilityDamage()
        {
            return PaladinConstants.HOLY_WRATH_BASE_DMG + 
                   PaladinConstants.HOLY_WRATH_COEFF * Stats.SpellPower;
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
            CT = new BaseSpellCombatTable(combats.Character.BossOptions, _stats, Attacktype.Spell);
            CT.CanCrit = false;
        }

        public override float AbilityDamage()
        {
            return (PaladinConstants.CONS_BASE_DMG + 
                    PaladinConstants.CONS_COEFF_SP * (Stats.SpellPower + Stats.ConsecrationSpellPower) + 
                    PaladinConstants.CONS_COEFF_AP * Stats.AttackPower)
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
            CT = new BasePhysicalYellowCombatTable(combats.Character.BossOptions, _stats, Attacktype.MeleeMH);
            AbilityDamageMulitplier[Multiplier.Others] = (1f + PaladinConstants.TWO_H_SPEC);
        }

        public override float AbilityDamage()
        {
            if (Combats.Talents.SealsOfCommand > 0)
                return Combats.WeaponDamage * PaladinConstants.SOC_COEFF;
            else
                return 0f;
        }
    }
    
    public abstract class Seal : Skill
    {
        public Seal(CombatStats combats, AbilityType abilityType)
            : base(combats, abilityType, DamageType.Holy)
        {
            AbilityDamageMulitplier[Multiplier.Talents] = (1f + PaladinConstants.SEALS_OF_THE_PURE * Talents.SealsOfThePure);
            AbilityDamageMulitplier[Multiplier.Sets] = (1f + Stats.SealMultiplier);
            AbilityDamageMulitplier[Multiplier.Others] = (1f + PaladinConstants.TWO_H_SPEC);
        }
    }

    public class SealOfRighteousness : Seal
    {
        public SealOfRighteousness(CombatStats combats) : base(combats, AbilityType.Spell) 
        {
            CT = new BaseSpellCombatTable(combats.Character.BossOptions, _stats, Attacktype.Spell);
            CT.CanCrit = false;
            CT.CanMiss = false;
        }

        public override float AbilityDamage()
        {
            return Combats.BaseWeaponSpeed * (PaladinConstants.SOR_COEFF_AP * Stats.AttackPower + 
                                              PaladinConstants.SOR_COEFF_SP * Stats.SpellPower);
        }

        public override float Targets()
        {
            return 1f; //TODO: Add additional target (Talents.SealsOfCommand > 0 ? PaladinConstants.SOR_ADDTARGET : 1f);
        }
    }

    public class SealOfTruth : Seal
    {
        public SealOfTruth(CombatStats combats) : base(combats, AbilityType.Melee) 
        {
            CT = new BasePhysicalYellowCombatTable(combats.Character.BossOptions, _stats, Attacktype.MeleeMH);
            CT.CanMiss = false;
        }

        public override float AbilityDamage()
        {
            return Combats.WeaponDamage * PaladinConstants.SOT_SEAL_COEFF;
        }
    }  

    public class SealOfTruthDoT : Skill
    {
        public SealOfTruthDoT(CombatStats combats, float averageStack) : base(combats, AbilityType.Melee, DamageType.Holy)
        {
            AverageStackSize = averageStack;
            CT = new BasePhysicalYellowCombatTable(combats.Character.BossOptions, _stats, Attacktype.MeleeMH);
            CT.CanMiss = false;
            AbilityDamageMulitplier[Multiplier.Talents] = (1f + PaladinConstants.SEALS_OF_THE_PURE * Talents.SealsOfThePure +
                                                                PaladinConstants.INQUIRY_OF_FAITH_SEAL * Talents.InquiryOfFaith);
            AbilityDamageMulitplier[Multiplier.Sets] = (1f + Stats.SealMultiplier);
        }

        public float AverageStackSize { get; private set; }
        public override float AbilityDamage()
        {
            return AverageStackSize * (Stats.SpellPower * PaladinConstants.SOT_CENSURE_COEFF_SP +
                                       Stats.AttackPower * PaladinConstants.SOT_CENSURE_COEFF_AP);
        }
    }
    
    public class White : Skill
    {
        public White(CombatStats combats) : base(combats, AbilityType.Melee, DamageType.Physical) 
        {
            CT = new BasePhysicalWhiteCombatTable(combats.Character.BossOptions, _stats, Attacktype.MeleeMH);
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
            CT = new BasePhysicalYellowCombatTable(combats.Character.BossOptions, _stats, Attacktype.MeleeMH);
        }

        public override float AbilityDamage() { return 0; }
    }

    public class NullSealDoT : Skill
    {
        public NullSealDoT(CombatStats combats) : base(combats, AbilityType.Melee, DamageType.Holy) 
        {
            CT = new BasePhysicalYellowCombatTable(combats.Character.BossOptions, _stats, Attacktype.MeleeMH);
        }

        public override float AbilityDamage() { return 0; }
    }

    public class NullJudgement : Skill
    {
        public NullJudgement(CombatStats combats) : base(combats, AbilityType.Melee, DamageType.Holy) 
        {
            CT = new BasePhysicalYellowCombatTable(combats.Character.BossOptions, _stats, Attacktype.Ranged);
        }

        public override bool UsableBefore20PercentHealth { get { return false; } }
        public override bool UsableAfter20PercentHealth { get { return false; } }
        public override float AbilityDamage() { return 0; }
    }
    #endregion

    public class MagicDamage : Skill
    {
        public MagicDamage(CombatStats combats, float amount) : base(combats, AbilityType.Spell, DamageType.Magic)
        {
            this.amount = amount;
            CT = new BaseSpellCombatTable(combats.Character.BossOptions, _stats, Attacktype.Spell);
        }

        private float amount;
        public override float AbilityDamage() { return amount; }
    }

}
