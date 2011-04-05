using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Healadin
{
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
        public float HPM() { return AverageHealed() / AverageCost(); }
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
            heal *= 0.15f; // TODO: Figure out a way to detect the character's main spec

            return heal;
        }

        public float ChanceToCrit()
        {
            return (float)Math.Max(0f, (float)Math.Min(1f, Stats.SpellCrit + AbilityCritChance() + ExtraCritChance + Talents.DivineFavor * .2f));
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
        public override float BaseMana { get { return 6324f; } } // TODO: Determine exact mana cost.  27% of base mana

        protected override float AbilityHealed()
        {
            const float fol_coef = 1.5f / 3.5f * 66f / 35f * 1.25f; // TODO: Determine if spell power co-effecient is correct
            return (4830f + 5418f) / 2f + (Stats.SpellPower * fol_coef);
        }
    }

    public class HolyLight : Heal
    {
        public HolyLight(Rotation rotation)
            : base(rotation)
        { }

        public override float BaseCastTime { get { return 3f - ClarityOfPurpose(Talents.ClarityOfPurpose) - CastTimeReduction; } }
        public override float BaseMana { get { return 2108f; } } // TODO: Determine exact mana cost.  9% of base mana

        public float CastTimeReduction { get; set; }

        protected override float AbilityHealed()
        {
            const float hl_coef = 3f / 3.5f * 66f / 35f * 1.25f; // TODO: Determine if spell power co-effecient is correct.  Since Holy Light is now a 3s cast time, I bumped the co-effecient up to 3 from 2.5.
            return (2911f + 3243f) / 2f + (Stats.SpellPower * hl_coef);
        }
    }

    public class DivineLight : Heal
    {
        public DivineLight(Rotation rotation)
            : base(rotation)
        { }

        public override float BaseCastTime { get { return 3f - ClarityOfPurpose(Talents.ClarityOfPurpose); } }
        public override float BaseMana { get { return 7027f; } } // TODO: Determine exact mana cast.  30% of base mana

        protected override float AbilityHealed() {
            const float dl_coef = 3f / 3.5f * 66f / 35f * 1.25f; // TODO: Determine if spell power co-efficient is correct.
            return (7762f + 8648f) / 2f + Stats.SpellPower * dl_coef;
        }
    }

    public class HolyShock : Heal
    {
        public HolyShock(Rotation rotation)
            : base(rotation)
        { }

        public override float BaseCastTime { get { return 1.5f; } }
        public override float BaseMana { get { return 1874f; } } // TODO: Determine exact mana cost.  8% of base mana

        protected override float AbilityHealed()
        {
            const float hs_coef = 1.5f / 3.5f * 66f / 35f; // TODO: Determine if spell power co-effecient is correct
            return ((2758f + 2986f) / 2f + Stats.SpellPower * hs_coef) * (1f + Talents.Crusade * 0.1f);
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
            BaseCost = 219f;
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
