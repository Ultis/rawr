using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Healadin
{
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

        public float HPS() { return AverageHealed() / CastTime(); }
        public float MPS() { return AverageCost() / CastTime(); }
        public float HPM() { return AverageHealed() / AverageCost(); }
        public float CastTime() { return (BaseCastTime - AbilityCastTimeReduction()) / (1f + Stats.SpellHaste);}

        public float AverageCost()
        {
            float critChance = ChanceToCrit();
            float costReduce = CostReduction();
            float costMulti = AbilityCostMultiplier();
            return (BaseMana * (Talents.GlyphOfSealOfWisdom ? .95f : 1f) * (DivineIllumination ? 0.5f : 1f) - costReduce)
                * costMulti
                - BaseMana * .12f * Talents.Illumination * critChance;
        }

        public float AverageHealed()
        {
            return BaseHealed() * (1f - ChanceToCrit()) + CritHealed() * ChanceToCrit();
        }

        public float CritHealed() { return BaseHealed() * 1.5f * (1f + Stats.BonusCritMultiplier) * AbilityCritMultiplier(); }

        public float BaseHealed()
        {
            float heal = AbilityHealed();

            heal *= Talents.GlyphOfSealOfLight ? 1.05f : 1;
            heal *= 1f + Stats.HealingReceivedMultiplier;
            heal *= 1f - Rotation.DivinePleas * 15f / Rotation.FightLength * .5f;
            heal *= 1f + .01f * Talents.Divinity;
            heal *= AbilityHealedMultiplier();
            heal *= (1f + Talents.HealingLight * .04f);

            return heal;
        }

        public float ChanceToCrit()
        {
            return (float)Math.Max(0f, (float)Math.Min(1f, Stats.SpellCrit + AbilityCritChance() + ExtraCritChance + Talents.HolyPower * .01f));
        }
        public float CostReduction() { return Stats.SpellsManaReduction + AbilityCostReduction(); }

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
    }

    public class FlashOfLight : Spell
    {
        public FlashOfLight(Rotation rotation)
            : base(rotation)
        { }

        public override float BaseCastTime { get { return 1.5f; } }
        public override float BaseMana { get { return 307f; } }

        protected override float AbilityCritChance()
        {
            return Talents.GlyphOfFlashOfLight ? .05f : 0f + Stats.FlashOfLightCrit;
        }

        protected override float AbilityHealedMultiplier()
        {
            return 1f + Stats.FlashOfLightMultiplier;
        }

        protected override float AbilityHealed()
        {
            const float fol_coef = 1.5f / 3.5f * 66f / 35f * 1.25f;
            return (835.5f + (Stats.SpellPower + Stats.FlashOfLightSpellPower) * fol_coef);
        }

    }

    public class HolyLight : Spell
    {
        public HolyLight(Rotation rotation)
            : base(rotation)
        { }

        public override float BaseCastTime { get { return 2.5f; } }
        public override float BaseMana { get { return 1274f; } }

        protected override float AbilityCritChance()
        {
            return Talents.SanctifiedLight * .02f + Stats.HolyLightCrit;
        }

        protected override float AbilityCostReduction()
        {
            return Stats.HolyLightManaCostReduction;
        }

        protected override float AbilityCostMultiplier()
        {
            return 1f - Stats.HolyLightPercentManaReduction;
        }

        protected override float AbilityCastTimeReduction()
        {
            return .5f / 3 * Talents.LightsGrace;
        }

        protected override float AbilityHealed()
        {
            const float hl_coef = 2.5f / 3.5f * 66f / 35f * 1.25f;
            return (5166f + (Stats.HolyLightSpellPower + Stats.SpellPower) * hl_coef);
        }

    }

    public class HolyShock : Spell
    {
        public HolyShock(Rotation rotation)
            : base(rotation)
        { }

        public override float BaseCastTime { get { return 1.5f; } }
        public override float BaseMana { get { return 790f; } }

        protected override float AbilityCritChance()
        {
            return Talents.SanctifiedLight * .02f + Stats.HolyShockCrit;
        }

        protected override float AbilityCostMultiplier()
        {
            return 1f - .02f * Talents.Benediction;
        }

        protected override float AbilityHealed()
        {
            const float hs_coef = 1.5f / 3.5f * 66f / 35f;
            return (2500f + Stats.SpellPower * hs_coef);
        }

        public float Cooldown()
        {
            return Talents.GlyphOfHolyShock ? 5f : 6f;
        }

        protected override float AbilityCritMultiplier()
        {
             return 1f + Stats.HolyShockHoTOnCrit;
        }

    }

}
