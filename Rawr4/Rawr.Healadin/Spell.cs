using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Healadin
{

    public static class HealadinConstants
    {
        // Spell coeficients
        // source:  http://elitistjerks.com/f76/t110847-holy_cataclysm_holy_compendium_4_0_6_a/
        public static float fol_coef = 0.8632f;
        public static float fol_mana = 7026.6f;
        public static float fol_min = 6907f;
        public static float fol_max = 7749f;

        public static float hl_coef = 0.432f;
        public static float hl_mana = 2342.2f;
        public static float hl_min = 4163f;
        public static float hl_max = 4637f;

        public static float dl_coef = 1.15306f;
        public static float dl_mana = 7729.3f;
        public static float dl_min = 11100f;
        public static float dl_max = 12366f;

        public static float hs_coef = 0.2689f;
        public static float hs_mana = 1873.8f;
        public static float hs_min = 2629f;
        public static float hs_max = 2847f;

        // Stats for 1 holy power.  Scales linearly with more holy power. (so just * by 2 or 3 when needed)
        public static float wog_coef_sp = 0.209f;  // regular spellpower coef
        public static float wog_coef_ap = 0.198f;  // Word of Glory also has Attack Power coef
        public static float wog_min = 2018f;
        public static float wog_max = 2248f;

        // Stats for 1 holy power, 1 target.  Hits up to 5 targets, 6 glyphed.
        public static float lod_coef = 0.198f;
        public static float lod_min = 605f;
        public static float lod_max = 673f;

        public static float basemana = 23422;
    }

    public abstract class Heal
    {
        private Rotation _rotation;
        public Rotation Rotation
        {
            get { return _rotation; }
            set
            {
                _rotation = value;
                _stats = _rotation.Stats;
                _talents = _rotation.Talents;
            }
        }

        private Stats _stats;
        protected Stats Stats { get { return _stats; } }

        private PaladinTalents _talents;
        protected PaladinTalents Talents { get { return _talents; } }

        public Heal(Rotation rotation)
        {
            Rotation = rotation;
        }

        public float HPS() { return AverageHealed() / CastTime(); }
        public float MPS() { return AverageCost() / CastTime(); }
        public float HPM() 
        {
            if (BaseMana == 0)
                return 0f;
            else
                return AverageHealed() / AverageCost(); 
        }

        public float CastTime() { return (float)Math.Max(1f, (BaseCastTime - AbilityCastTimeReduction()) / (1f + Stats.SpellHaste));}

        public float AverageCost()
        {
            return (float)Math.Floor((BaseMana * (DivineIllumination ? 0.5f : 1f) - CostReduction())
                * (AbilityCostMultiplier()));
        }

        public float AverageHealed()
        {
            return BaseHealed() * (1f - ChanceToCrit()) + CritHealed() * ChanceToCrit();
        }

        // Illuminated Healing
        public float AverageShielded()
        {
            return AverageHealed() * (1f + StatConversion.GetMasteryFromRating(Stats.MasteryRating) * 0.1f);
        }

        public float CritHealed() { return BaseHealed() * 1.5f * (1f + Stats.BonusCritHealMultiplier) * AbilityCritMultiplier(); }

        public float BaseHealed()
        {
            float heal = AbilityHealed();

            heal *= Talents.GlyphOfSealOfInsight ? 1.05f : 1;
            heal *= 1f + Stats.HealingReceivedMultiplier;
            heal *= 1f - Rotation.DivinePleas * 15f / Rotation.FightLength * .5f;
            heal *= 1f + .01f * Talents.Divinity;
            heal *= AbilityHealedMultiplier();
            heal *= (1f + Stats.BonusHealingDoneMultiplier);

            // Walk in the Light
            heal *= (1f + 0.1f); // TODO: Figure out a way to detect the character's main spec

            return heal;
        }

        public float ChanceToCrit()
        {
            // TODO: figure out how to add Talents.DivineFavor into crit chance, when do we proc it, which casts are proced, etc.
            return (float)Math.Max(0f, (float)Math.Min(1f, Stats.SpellCrit + AbilityCritChance() + ExtraCritChance ));
        }
        public float CostReduction() { return Stats.SpellsManaCostReduction + Stats.HolySpellsManaCostReduction + AbilityCostReduction(); }

        public float ExtraCritChance { get; set; }
        public bool DivineIllumination { get; set; }

        protected abstract float AbilityHealed();
        protected virtual float AbilityCritChance() { return 0f; }
        protected virtual float AbilityCostReduction() { return 0f; }
        protected virtual float AbilityCastTimeReduction() { return 0f; }
        protected virtual float AbilityCostMultiplier() { return 1f; }
        protected virtual float AbilityHealedMultiplier() { return 1f; }
        protected virtual float AbilityCritMultiplier() { return 1f; }

        public abstract float BaseCastTime { get; }
        public abstract float BaseMana { get; }

        public override string ToString()
        {
            return string.Format("Average Heal: {0:N0}\nAverage Cost: {1:N0}\nHPS: {2:N0}\nHPM: {3:N2}\nCast Time: {4:N2} sec\nCrit Chance: {5:N2}%",
                AverageHealed(),
                AverageCost(),
                HPS(),
                HPM(),
                CastTime(),
                ChanceToCrit() * 100);
        }

        protected static float ClarityOfPurpose(int talentPoints) {
            switch (talentPoints) {
                case 1:
                    return 0.15f;
                case 2:
                    return 0.35f;
                case 3:
                    return 0.5f;
                default:
                    return 0f;
            }
        }
    }

    public class FlashOfLight : Heal
    {
        public FlashOfLight(Rotation rotation)
            : base(rotation)
        { }

        public override float BaseCastTime { get { return 1.5f; } }
        public override float BaseMana { get { return HealadinConstants.fol_mana; } } 

        protected override float AbilityHealed()
        {
            // TODO: calculate real spellpower somewhere in Healadin module, and use that instead of Stats.SpellPower + Stats.Intellect
            return (HealadinConstants.fol_min + HealadinConstants.fol_max) / 2f + ((Stats.SpellPower + Stats.Intellect) * HealadinConstants.fol_coef);
        }
    }

    public class HolyLight : Heal
    {
        public HolyLight(Rotation rotation)
            : base(rotation)
        { }

        public override float BaseCastTime { get { return 3f - ClarityOfPurpose(Talents.ClarityOfPurpose) - CastTimeReduction; } }
        public override float BaseMana { get { return HealadinConstants.hl_mana; } } 

        public float CastTimeReduction { get; set; }

        protected override float AbilityHealed()
        {
            // TODO: calculate real spellpower somewhere in Healadin module, and use that instead of Stats.SpellPower + Stats.Intellect
            return (HealadinConstants.hl_min + HealadinConstants.hl_max) / 2f + ((Stats.SpellPower + Stats.Intellect) * HealadinConstants.hl_coef);
        }
    }

    public class DivineLight : Heal
    {
        public DivineLight(Rotation rotation)
            : base(rotation)
        { }

        public override float BaseCastTime { get { return 3f - ClarityOfPurpose(Talents.ClarityOfPurpose); } }
        public override float BaseMana { get { return HealadinConstants.dl_mana; } } 

        protected override float AbilityHealed() {
            // TODO: calculate real spellpower somewhere in Healadin module, and use that instead of Stats.SpellPower + Stats.Intellect
            return (HealadinConstants.dl_min + HealadinConstants.dl_max) / 2f + ((Stats.SpellPower + Stats.Intellect) * HealadinConstants.dl_coef);
        }
    }

    public class HolyShock : Heal
    {
        public HolyShock(Rotation rotation)
            : base(rotation)
        { }

        public override float BaseCastTime { get { return 1.5f; } }
        public override float BaseMana { get { return HealadinConstants.hs_mana; } } 

        protected override float AbilityHealed()
        {
            // TODO: calculate real spellpower somewhere in Healadin module, and use that instead of Stats.SpellPower + Stats.Intellect
            return ((HealadinConstants.hs_min + HealadinConstants.hs_max) / 2f +
                     ((Stats.SpellPower+ Stats.Intellect) * HealadinConstants.hs_coef) * (1f + Talents.Crusade * 0.1f));
        }

        public float Usage()
        {
            return Casts() * AverageCost();
        }

        public float Casts()
        {
            return Rotation.FightLength * Rotation.CalcOpts.HolyShock / Cooldown();
        }

        public float Time()
        {
            return Casts() * CastTime();
        }

        public float Healed()
        {
            return Casts() * AverageHealed();
        }

        public float Cooldown()
        {
            return 6f; // TODO: Account for talent Daybreak
        }

        protected override float AbilityCritMultiplier()
        {
            return 1f + (Talents.GlyphOfHolyShock ? 0.05f : 0f) + (Talents.InfusionOfLight * 0.05f);
        }

    }

    public class WordofGlory : Heal
    {
        public WordofGlory(Rotation rotation)
            : base(rotation)
        { }

        public override float BaseCastTime { get { return 1.5f; } }
        public override float BaseMana { get { return 0f; } }

        protected override float AbilityHealed()
        {
            float attackpower = Stats.AttackPower + Stats.Strength * 2;
            attackpower *= (1f + Stats.BonusAttackPowerMultiplier);
            float holypower = 3f;  // assume 3 holypower for now
            // TODO: calculate real spellpower somewhere in Healadin module, and use that instead of Stats.SpellPower + Stats.Intellect
            return holypower * (HealadinConstants.wog_min + HealadinConstants.wog_max) / 2f + 
                                ((Stats.SpellPower + Stats.Intellect) * HealadinConstants.wog_coef_sp) +
                                (attackpower * HealadinConstants.wog_coef_ap);
        }
    }

    public class LightofDawn : Heal
    {
        public LightofDawn(Rotation rotation)
            : base(rotation)
        { }

        public override float BaseCastTime { get { return 1.5f; } }
        public override float BaseMana { get { return 0f; } }

        protected override float AbilityHealed()
        {
            //TODO: find if we are glyphed for 6 targets healed
            float targets_healed = 5f;
            float holypower = 3f; // assume 3 holy power for now
            // TODO: calculate real spellpower somewhere in Healadin module, and use that instead of Stats.SpellPower + Stats.Intellect
            return holypower * targets_healed * ((HealadinConstants.lod_min + HealadinConstants.lod_max) / 2f + 
                                                 ((Stats.SpellPower + Stats.Intellect) * HealadinConstants.lod_coef));
        }
    }

    public abstract class Spell
    {

        private Rotation _rotation;
        public Rotation Rotation
        {
            get { return _rotation; }
            set
            {
                _rotation = value;
                _stats = _rotation.Stats;
                _talents = _rotation.Talents;
            }
        }

        private Stats _stats;
        protected Stats Stats { get { return _stats; } }

        private PaladinTalents _talents;
        protected PaladinTalents Talents { get { return _talents; } }

        public Spell(Rotation rotation)
        {
            Rotation = rotation;
        }

        public float Uptime { get; set; }
        public float Duration { get; set; }
        public float BaseCost { get; set; }

        public float Cost()
        {
            return (BaseCost - Stats.SpellsManaCostReduction - Stats.HolySpellsManaCostReduction);
        }

        public float Casts()
        {
            return (float)Math.Ceiling(Uptime / Duration);
        }

        public virtual float CastTime()
        {
            return (float)Math.Max(1f, 1.5f / (1f + Stats.SpellHaste));
        }

        public virtual float Time()
        {
            return Casts() * CastTime();
        }

        public virtual float Usage()
        {
            return Casts() * Cost();
        }

    }

    public class BeaconOfLight : Spell
    {
        public BeaconOfLight(Rotation rotation)
            : base(rotation)
        {
            Duration = 60f + (Talents.GlyphOfBeaconOfLight ? 30f : 0f);
            Uptime = Rotation.FightLength * Rotation.CalcOpts.BoLUp;
            BaseCost = 1405f; // TODO: Determine exact mana cost.  6% of base mana
        }

        public float HealingDone(float procableHealing)
        {
            return procableHealing * Rotation.CalcOpts.BoLUp * 0.5f;
        }

    }

    public class JudgementsOfThePure : Spell
    {
        public JudgementsOfThePure(Rotation rotation, bool MaintainDebuff)
            : base(rotation)
        {
            Duration = MaintainDebuff ? 20f : 60f;
            Uptime = Rotation.CalcOpts.JotP ? Rotation.FightLength : 0f;
            BaseCost = HealadinConstants.basemana * 0.05f;
        }

        public override float CastTime()
        {
            return 1.5f;
        }

    }

    public class DivineIllumination : Spell
    {

        public HolyLight HL_DI { get; set; }

        public DivineIllumination(Rotation rotation)
            : base(rotation)
        {
            Duration = 180f;
            Uptime = Rotation.FightLength;
            BaseCost = 0f;
            HL_DI = new HolyLight(rotation) { DivineIllumination = true };
        }

        public float Healed()
        {
            return HL_DI.HPS() * Time();
        }

        public override float Time()
        {
            return 15f * Casts() * Rotation.CalcOpts.Activity;
        }

        public override float Usage()
        {
            return Time() * HL_DI.MPS();
        }

    }

    public class DivineFavor : Spell
    {

        public HolyLight HL_DF { get; set; }

        public DivineFavor(Rotation rotation)
            : base(rotation)
        {
            Duration = 120f;
            Uptime = Rotation.FightLength;
            BaseCost = 130f;
            HL_DF = new HolyLight(rotation) { ExtraCritChance = 1f };
        }

        public float Healed()
        {
            return HL_DF.AverageHealed() * Casts();
        }

        public override float Time()
        {
            return HL_DF.CastTime() * Casts();
        }

        public override float Usage()
        {
            return Casts() * (Cost() + HL_DF.AverageCost());
        }

    }
}
