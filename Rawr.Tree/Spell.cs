using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Tree
{
    abstract class Spell
    {
        public string Name;

        public Boolean ToLAura = true;

        public float BaseCastTime;
        public float BasePeriodicTick;
        public float BasePeriodicTicks;
        public float BaseMinHeal;
        public float BaseMaxHeal;
        public float BaseCritPercent;
        public int BaseRange;
        public float BaseHoTCoefficient;
        public float BaseHealCoefficient;

        protected float MinHeal
        {
            get
            {
                return (BaseMinHeal + BaseHealCoefficient * (Healing * DirectHealingBonusMultiplier + DirectHealingBonus) * (PeriodicTicks > 0 ? HoTMultiplier : 1f)) * HealMultiplier;
            }
        }
        protected float MaxHeal
        {
            get
            {
                return (BaseMaxHeal + BaseHealCoefficient * (Healing * DirectHealingBonusMultiplier + DirectHealingBonus) * (PeriodicTicks > 0 ? HoTMultiplier : 1f)) * HealMultiplier;
            }
        }
        protected float MinCrit
        {
            get
            {
                return MinHeal * CritModifier;
            }
        }
        protected float MaxCrit
        {
            get
            {
                return MaxHeal * CritModifier;
            }
        }

        public String HealInterval
        {
            get
            {
                return (int)MinHeal + "~" + (int)MaxHeal + " (" + (int)MinCrit + "~" + (int)MaxCrit + " crit)";
            }
        }

        public float AverageHeal
        {
            get
            {
                return (MinHeal + MaxHeal) / 2f * (1 - (CritPercent / 100f)) +
                    (MinCrit + MaxCrit) / 2f * (CritPercent / 100f);
            }
        }
        public float PeriodicTick
        {
            get
            {
                return (BasePeriodicTick + BaseHoTCoefficient * (Healing + HoTHealingBonus) * HoTMultiplier) * HealMultiplier;
            }
        }
        public float PeriodicTicks;
        public int Cost;
        public int Range;

        public float HealMultiplier = 1f;
        public float HoTMultiplier = 1f;

        public float CastTime;
        public float CritPercent;
        public float CritModifier = 1.5f;

        public float Healing;
        public float HoTHealingBonus = 0; // Items or talents that increase healing by a fixed value
        public float DirectHealingBonus = 0; // Items or talents that increase healing by a fixed value
        public float DirectHealingBonusMultiplier = 1;

        public float HPS
        {
            get
            {
                return AverageTotalHeal / CastTime;
            }
        }
        public float HPM
        {
            get
            {
                return AverageTotalHeal / Cost;
            }
        }
        public float AverageTotalHeal
        {
            get
            {
                return AverageHeal + PeriodicTick * PeriodicTicks;
            }
        }

        internal Character _character;


        public override bool Equals(object obj)
        {
            if (obj is Spell)
            {
                return (obj as Spell).Name == Name;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        protected void ParseTalents(Character character, Stats stats)
        {
            CritPercent = BaseCritPercent;
            CastTime = BaseCastTime;
            Range = BaseRange;

            _character = character;

            CalculationOptionsTree calcOpts = character.CalculationOptions as CalculationOptionsTree;

            PeriodicTicks = BasePeriodicTicks;

            HoTMultiplier += 0.04f * calcOpts.EmpoweredRejuvenation;
            HealMultiplier += 0.02f * calcOpts.GiftOfNature;

            if (BasePeriodicTicks > 0)
            {
                if (calcOpts.TreeOfLife == 1)
                {
                    Cost = Convert.ToInt32(Cost * 0.8f);
                    if (ToLAura)
                    {
                        // TODO: Stats class should contain values for these sort of buffs... fix later
                        if (character.Ranged != null && character.Ranged.Id == 32387)
                        { // Idol of the Raven Goddess
                            Healing += 44;
                        }
                        Healing += ((int)stats.Spirit) / 4;
                    }
                }
            }

            CastTime = BaseCastTime;
            if (CastTime < 1.5f)
                CastTime = 1.5f;
        }

        protected void Calculate(Character character, Stats stats)
        {
            CastTime /= (1 + (stats.SpellHasteRating / 1570f));
            if (CastTime < 1.0f)
                CastTime = 1.0f;
        }
    }

    internal class Lifebloom : Spell
    {
        public Lifebloom(Character character, Stats stats, Boolean AddToLAura)
        {
            ToLAura = AddToLAura;
            Name = "Lifebloom" + (ToLAura ? "" : " (no aura)");
            Cost = 220;
            BaseCastTime = 0f;

            BasePeriodicTick = 39;
            BasePeriodicTicks = 6;
            BaseHoTCoefficient = 0.5180f / 7f;

            BaseMinHeal = 600;
            BaseMaxHeal = 600;
            BaseHealCoefficient = 0.3429f; // http://www.wowwiki.com/Spell_Damage_Coefficients

            BaseCritPercent = stats.SpellCritRating / 22.08f;
            Healing = stats.Healing;
            BaseRange = 40;

            ParseTalents(character, stats);
            if (character.Ranged != null && character.Ranged.Id == 27886)
            { // Idol of the Emerald Queeen
                HoTHealingBonus += 88 / HoTMultiplier / HealMultiplier; // This bonus heal does not benefit from talents
            }
            Calculate(character, stats);
        }
    }

    internal class LifebloomStack : Lifebloom
    {
        public LifebloomStack(Character character, Stats stats, Boolean AddToLAura)
            : base(character, stats, AddToLAura)
        {
            ToLAura = AddToLAura;
            Name = "Lifebloom Stack" + (ToLAura ? "" : " (no aura)");
            BasePeriodicTick *= 3;
            BaseHoTCoefficient *= 3;

            BaseMinHeal = 0;
            BaseMaxHeal = 0;
            BaseHealCoefficient = 0; // http://www.wowwiki.com/Spell_Damage_Coefficients

            BaseCritPercent = 0;
        }
    }

    internal class Rejuvenation : Spell
    {
        public Rejuvenation(Character character, Stats stats, Boolean AddToLAura)
        {
            ToLAura = AddToLAura;
            Name = "Rejuvenation" + (ToLAura ? "" : " (no aura)");
            Cost = 415;
            BaseCastTime = 0f;

            BasePeriodicTick = 265;
            BasePeriodicTicks = 4;
            BaseHoTCoefficient = 0.2f;

            BaseMinHeal = 0;
            BaseMaxHeal = 0;
            BaseHealCoefficient = 0; // http://www.wowwiki.com/Spell_Damage_Coefficients

            BaseCritPercent = stats.SpellCritRating / 22.08f;
            Healing = stats.Healing;
            BaseRange = 40;

            ParseTalents(character, stats);
            ParseTalentsRejuvenation(character, stats);
            Calculate(character, stats);
        }

        private void ParseTalentsRejuvenation(Character character, Stats stats)
        {
            CalculationOptionsTree calcOpts = character.CalculationOptions as CalculationOptionsTree;
            HealMultiplier += 0.05f * calcOpts.ImprovedRejuvenation;
        }
    }

    internal class Regrowth : Spell
    {
        public Regrowth(Character character, Stats stats, Boolean AddToLAura)
        {
            ToLAura = AddToLAura;
            Name = "Regrowth" + (ToLAura ? "" : " (no aura)");
            Cost = 675;
            BaseCastTime = 2f;

            BasePeriodicTick = 182;
            BasePeriodicTicks = 7;
            BaseHoTCoefficient = 0.7f / 7;

            BaseMinHeal = 1215;
            BaseMaxHeal = 1355;
            BaseHealCoefficient = 0.289f; // http://www.wowwiki.com/Spell_Damage_Coefficients

            BaseCritPercent = stats.SpellCritRating / 22.08f;
            Healing = stats.Healing;
            BaseRange = 40;

            if (character.Ranged != null && character.Ranged.Id == 30051)
            { // Idol of the Crescent Goddess
                Cost -= 65;
            }

            ParseTalents(character, stats);
            ParseTalentsRegrowth(character, stats);
            Calculate(character, stats);
        }

        private void ParseTalentsRegrowth(Character character, Stats stats)
        {
            CalculationOptionsTree calcOpts = character.CalculationOptions as CalculationOptionsTree;
            CritPercent += 10 * calcOpts.ImprovedRegrowth;
        }
    }

    /*
     * Healing Touch has nothing to do in a Tree of Life model, but is provided in case somebody wants to include it anyway
    internal class HealingTouch : Spell
    {
        public HealingTouch(Character character, Stats stats)
        {
            Name = "Healing Touch";
            Cost = 935;
            BaseCastTime = 3.5f;

            BasePeriodicTick = 0;
            BasePeriodicTicks = 0;
            BaseHoTCoefficient = 0;

            BaseMinHeal = 2707;
            BaseMaxHeal = 3197;
            BaseHealCoefficient = 1f; // http://www.wowwiki.com/Spell_Damage_Coefficients

            BaseCritPercent = stats.SpellCritRating / 22.08f;
            Healing = stats.Healing;
            BaseRange = 40;

            ParseTalentsHT(character, stats);
            ParseTalents(character, stats);
            if (character.Ranged != null && character.Ranged.Id == 28568)
            { // Idol of the Avian Heart
                BaseMinHeal += 136 / HealMultiplier;
                BaseMaxHeal += 136 / HealMultiplier;
            }
            Calculate(character, stats);
        }

        private void ParseTalentsHT(Character character, Stats stats)
        {
            CalculationOptionsTree calcOpts = character.CalculationOptions as CalculationOptionsTree;
            BaseCastTime -= 0.1f * calcOpts.Naturalist;
            Cost = (int)(Cost * (1f - 0.02f * calcOpts.TranquilSpirit));
            DirectHealingBonusMultiplier += 0.1f * calcOpts.EmpoweredTouch;
        }
    }
    */
}
