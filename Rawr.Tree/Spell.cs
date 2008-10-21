using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Tree
{
    public abstract class Spell
    {
        protected float minHeal = 0f;
        public float MinHeal
        { get { return minHeal + healingBonus * coefDH; } }
        protected float maxHeal = 0f;
        public float MaxHeal
        { get { return maxHeal + healingBonus * coefDH; } }
        public float castTime = 0f;
        public float CastTime
        { get { return castTime > gcd ? castTime : gcd; } }
        public float gcd = 1.5f;
        public float manaCost = 0f;
        public float coefDH = 0f; //coef for DirectHeal

        protected float healingBonus = 0f;
        public float HealingBonus
        {
            get { return healingBonus; }
            set { healingBonus = value; }
        }

        protected float critModifier = 1.5f;
        public float CritModifier
        {
            get { return critModifier; }
            set { critModifier = value; }
        }

        protected float critPercent = 0f;
        public float CritPercent
        {
            get { return critPercent; }
            set { critPercent = value; }
        }

        protected float periodicTick = 0f;
        protected float periodicTicks = 0f;
        public float PeriodicTicks
        { get { return periodicTicks; } }
        protected float periodicTickTime = 3f;
        public float coefHoT = 0f; //coef for HoT

        public float AverageHealing
        { get { return (minHeal + maxHeal) / 2 + healingBonus * coefDH; } }

        public float AverageHealingwithCrit
        { get { return (AverageHealing * (100 - critPercent) + (AverageHealing * critModifier) * critPercent) / 100; } }

        public float PeriodicTick
        { get { return periodicTick + healingBonus * coefHoT; } }

        public float HPS
        { get { return AverageHealingwithCrit / CastTime; } }

        public float HPSHoT
        { get { return (PeriodicTick) / periodicTickTime; } }

        public float HPM
        { get { return (AverageHealing + PeriodicTick * PeriodicTicks) / manaCost; } }

        public float Duration
        { get { return periodicTicks * periodicTickTime; } }
    }

    public class HealingTouch : Spell
    {
        public HealingTouch(CharacterCalculationsTree calcs)
        {
            Stats calculatedStats = calcs.BasicStats;
            CalculationOptionsTree calcOpts = (CalculationOptionsTree)calcs.LocalCharacter.CalculationOptions;

            castTime = 3f;
            coefDH = castTime / 3.5f;
            manaCost = 0.33f * TreeConstants.getBaseMana(calcs.LocalCharacter.Race, calcOpts.level);
            healingBonus = calculatedStats.SpellPower * 1.88f + calculatedStats.AverageHeal * calcOpts.averageSpellpowerUsage/100f;
            critPercent = calculatedStats.SpellCrit;

            #region minHeal, maxHeal
            if (calcOpts.level == 70)
            {
                minHeal = 2321f;
                maxHeal = 2739f;
            }
            else if (calcOpts.level == 80)
            {
                minHeal = 4089f;
                maxHeal = 4089f;
            }
            #endregion

            calculateTalents(calcs.LocalCharacter.DruidTalents, calcOpts);

            if (calcOpts.glyphOfHealingTouch)
            {
                castTime -= 1.5f;
                manaCost *= 1 - 0.25f;
                minHeal *= 1 - 0.5f;
                maxHeal *= 1 - 0.5f;
                coefDH *= 1 - 0.5f;
            }
        }

        private void calculateTalents(DruidTalents druidTalents, CalculationOptionsTree calcOpts)
        {
            manaCost *= 1 - (druidTalents.Moonglow * 0.03f + druidTalents.TranquilSpirit * 0.02f);

            castTime -= 0.1f * druidTalents.Naturalist;

            critPercent += 2f * druidTalents.NaturesMastery;

            //Living Seed, 30% seed, 33% * points spend (1/3)
            if (calcOpts.useLivingSeedAsCritMultiplicator)
                critModifier += 0.1f * druidTalents.LivingSeed;
            
            coefDH *= 1 + 0.125f * druidTalents.EmpoweredTouch; 
            //My tests on the Live Realm gave the result, that Empowered Touch give only 25% instead of 40%

            minHeal *=
                (1 + 0.02f * druidTalents.GiftOfNature) *
                (1 + 0.02f * druidTalents.MasterShapeshifter * druidTalents.TreeOfLife) *
                (1 + 0.06f * druidTalents.TreeOfLife);

            maxHeal *=
                (1 + 0.02f * druidTalents.GiftOfNature) *
                (1 + 0.02f * druidTalents.MasterShapeshifter * druidTalents.TreeOfLife) *
                (1 + 0.06f * druidTalents.TreeOfLife);

            coefDH *=
                (1 + 0.02f * druidTalents.GiftOfNature) *
                (1 + 0.02f * druidTalents.MasterShapeshifter * druidTalents.TreeOfLife) *
                (1 + 0.06f * druidTalents.TreeOfLife);

        }
    }

    public class Regrowth : Spell
    {
        public Regrowth(CharacterCalculationsTree calcs)
        {
            InitializeRegrowth(calcs);
        }

        public Regrowth(CharacterCalculationsTree calcs, bool withRegrowthActive) 
        {
            InitializeRegrowth(calcs);

            if (withRegrowthActive && ((CalculationOptionsTree)calcs.LocalCharacter.CalculationOptions).glyphOfRegrowth)
            {
                minHeal *= 1.2f;
                maxHeal *= 1.2f;
                periodicTick *= 1.2f;
                coefDH *= 1.2f;
                coefHoT *= 1.2f;
            }
        }

        private void InitializeRegrowth(CharacterCalculationsTree calcs)
        {
            Stats calculatedStats = calcs.BasicStats;
            CalculationOptionsTree calcOpts = (CalculationOptionsTree)calcs.LocalCharacter.CalculationOptions;

            castTime = 2f;
            coefDH = 0.3f; //0.289f; It seems the DH coef got Buffed a bit
            coefHoT = 0.7f / 7f;
            manaCost = 0.29f * TreeConstants.getBaseMana(calcs.LocalCharacter.Race, calcOpts.level);
            healingBonus = calculatedStats.SpellPower * 1.88f + calculatedStats.AverageHeal * calcOpts.averageSpellpowerUsage / 100f;
            critPercent = calculatedStats.SpellCrit;

            periodicTicks = 7;

            #region minHeal, maxHeal, periodicTick
            if (calcOpts.level == 70)
            {
                minHeal = 1215;
                maxHeal = 1355;
                periodicTick = 182;
            }
            else if (calcOpts.level == 80)
            {
                minHeal = 1215;
                maxHeal = 1355f;
                periodicTick = 182;
            }
            #endregion

            calculateTalents(calcs.LocalCharacter.DruidTalents, calcOpts);
        }

        private void calculateTalents(DruidTalents druidTalents, CalculationOptionsTree calcOpts)
        {
            periodicTicks += 2 * druidTalents.NaturesSplendor;

            manaCost -=
                manaCost * 0.03f * druidTalents.Moonglow +
                manaCost * 0.2f * druidTalents.TreeOfLife;

            critPercent += 10f * druidTalents.ImprovedRegrowth + 1f * druidTalents.NaturalPerfection;
            //Living Seed
            if (calcOpts.useLivingSeedAsCritMultiplicator)
                critModifier += 0.1f * druidTalents.LivingSeed;

            minHeal *=
                (1 + 0.02f * druidTalents.GiftOfNature) *
                (1 + 0.02f * druidTalents.MasterShapeshifter * druidTalents.TreeOfLife) *
                (1 + 0.06f * druidTalents.TreeOfLife);

            maxHeal *=
                (1 + 0.02f * druidTalents.GiftOfNature) *
                (1 + 0.02f * druidTalents.MasterShapeshifter * druidTalents.TreeOfLife) *
                (1 + 0.06f * druidTalents.TreeOfLife);

            coefDH *=
                (1 + 0.04f * druidTalents.EmpoweredRejuvenation) *
                (1 + 0.02f * druidTalents.GiftOfNature) *
                (1 + 0.02f * druidTalents.MasterShapeshifter * druidTalents.TreeOfLife) *
                (1 + 0.06f * druidTalents.TreeOfLife);

            periodicTick +=
                periodicTick * 0.01f * druidTalents.Genesis +
                periodicTick * 0.02f * druidTalents.GiftOfNature;
            periodicTick *=
                (1 + 0.02f * druidTalents.MasterShapeshifter * druidTalents.TreeOfLife) *
                (1 + 0.06f * druidTalents.TreeOfLife);

            coefHoT *= 1 + 0.04f * druidTalents.EmpoweredRejuvenation;
            coefHoT +=
                coefHoT * 0.01f * druidTalents.Genesis +
                coefHoT * 0.02f * druidTalents.GiftOfNature;
            coefHoT *=
                (1 + 0.02f * druidTalents.MasterShapeshifter * druidTalents.TreeOfLife) *
                (1 + 0.06f * druidTalents.TreeOfLife);
        }
    }

    public class Rejuvenation : Spell
    {
        public Rejuvenation(CharacterCalculationsTree calcs)
        {
            Stats calculatedStats = calcs.BasicStats;
            CalculationOptionsTree calcOpts = (CalculationOptionsTree)calcs.LocalCharacter.CalculationOptions;

            castTime = 0f;
            coefHoT = 0.8f / 4f;
            manaCost = 0.18f * TreeConstants.getBaseMana(calcs.LocalCharacter.Race, calcOpts.level);
            healingBonus = calculatedStats.SpellPower * 1.88f + calculatedStats.AverageHeal * calcOpts.averageSpellpowerUsage / 100f;

            periodicTicks = 4;

            #region periodicTick
            if (calcOpts.level == 70)
            {
                periodicTick = 265f; //1060 / 4
            }
            else if (calcOpts.level == 80)
            {
                periodicTick = 338f; //1690 / 5 ... newest Rank got 15 seconds.. i hope it's not involed with another coef ..
            }
            #endregion

            calculateTalents(calcs.LocalCharacter.DruidTalents, calcOpts);
        }

        private void calculateTalents(DruidTalents druidTalents, CalculationOptionsTree calcOpts)
        {
            periodicTicks += 1 * druidTalents.NaturesSplendor;

            gcd *= 1 - 0.04f * druidTalents.GiftOfTheEarthmother;

            manaCost *= 1 - 0.2f * druidTalents.TreeOfLife;

            periodicTick +=
                periodicTick * 0.01f * druidTalents.Genesis +
                periodicTick * 0.05f * druidTalents.ImprovedRejuvenation +
                periodicTick * 0.02f * druidTalents.GiftOfNature;
            periodicTick *=
                (1 + 0.02f * druidTalents.MasterShapeshifter * druidTalents.TreeOfLife) *
                (1 + 0.06f * druidTalents.TreeOfLife);

            coefHoT *= 1 + 0.04f * druidTalents.EmpoweredRejuvenation;
            coefHoT +=
                coefHoT * 0.01f * druidTalents.Genesis +
                coefHoT * 0.05f * druidTalents.ImprovedRejuvenation +
                coefHoT * 0.02f * druidTalents.GiftOfNature;
            coefHoT *=
                (1 + 0.02f * druidTalents.MasterShapeshifter * druidTalents.TreeOfLife) *
                (1 + 0.06f * druidTalents.TreeOfLife);
        }
    }

    public class Lifebloom : Spell
    {
        public Lifebloom(CharacterCalculationsTree calcs)
        {
            Stats calculatedStats = calcs.BasicStats;
            CalculationOptionsTree calcOpts = (CalculationOptionsTree)calcs.LocalCharacter.CalculationOptions;

            castTime = 0f;
            periodicTickTime = 1f;
            coefDH = 0.3434f; //0.342 doesn't seem accurate too
            coefHoT = 0.0562f; //0.44f / 7f; have to test it even more ... 0.44f/7f isn't true anymore .. its afaik lower
            manaCost = 0.14f * TreeConstants.getBaseMana(calcs.LocalCharacter.Race, calcOpts.level);
            healingBonus = calculatedStats.SpellPower * 1.88f + calculatedStats.AverageHeal * calcOpts.averageSpellpowerUsage / 100f;
            critPercent = calculatedStats.SpellCrit;

            periodicTicks = 7;

            #region periodicTick
            if (calcOpts.level == 70)
            {
                minHeal = 600f;
                maxHeal = 600f;
                periodicTick = 32f; 
            }
            else if (calcOpts.level == 80)
            {
                minHeal = 970f;
                maxHeal = 970f;
                periodicTick = 53f; 
            }
            #endregion

            calculateTalents(calcs.LocalCharacter.DruidTalents, calcOpts);

            if (calcOpts.glyphOfLifebloom)
                periodicTicks += 1;
        }

        private void calculateTalents(DruidTalents druidTalents, CalculationOptionsTree calcOpts)
        {
            periodicTicks += 2 * druidTalents.NaturesSplendor;

            gcd *= 1 - 0.04f * druidTalents.GiftOfTheEarthmother;

            manaCost *= 1 - 0.2f * druidTalents.TreeOfLife;

            periodicTick +=
                periodicTick * 0.01f * druidTalents.Genesis +
                periodicTick * 0.02f * druidTalents.GiftOfNature;
            periodicTick *=
                (1 + 0.02f * druidTalents.MasterShapeshifter * druidTalents.TreeOfLife) *
                (1 + 0.06f * druidTalents.TreeOfLife);

            coefHoT *= 1 + 0.04f * druidTalents.EmpoweredRejuvenation;

            coefHoT +=
                coefHoT * 0.01f * druidTalents.Genesis +
                //coefHoT * 0.05f * druidTalents.ImprovedRejuvenation +
                coefHoT * 0.02f * druidTalents.GiftOfNature;
            coefHoT *=
                (1 + 0.02f * druidTalents.MasterShapeshifter * druidTalents.TreeOfLife) *
                (1 + 0.06f * druidTalents.TreeOfLife);

            minHeal *=
                (1 + 0.02f * druidTalents.GiftOfNature) *
                (1 + 0.02f * druidTalents.MasterShapeshifter) *
                (1 + 0.06f * druidTalents.TreeOfLife);

            maxHeal *=
                (1 + 0.02f * druidTalents.GiftOfNature) *
                (1 + 0.02f * druidTalents.MasterShapeshifter) *
                (1 + 0.06f * druidTalents.TreeOfLife);

            coefDH *=
                (1 + 0.04f * druidTalents.EmpoweredRejuvenation) *
                (1 + 0.02f * druidTalents.GiftOfNature) *
                (1 + 0.02f * druidTalents.MasterShapeshifter) *
                (1 + 0.06f * druidTalents.TreeOfLife);
        }
    }

    public class LifebloomStack : Lifebloom
    {
        public LifebloomStack(CharacterCalculationsTree calcs) : base(calcs)
        {
            periodicTick *= 3;
            periodicTicks -= 1; //keep a stack alive ;D
            coefHoT *= 3;
            minHeal = 0f;
            maxHeal = 0f;
            coefDH = 0f;
        }
    }

    public class WildGrowth : Spell
    {
        public WildGrowth(CharacterCalculationsTree calcs)
        {
            Stats calculatedStats = calcs.BasicStats;
            CalculationOptionsTree calcOpts = (CalculationOptionsTree)calcs.LocalCharacter.CalculationOptions;

            castTime = 0f;
            coefHoT = 0.966f; // http://elitistjerks.com/924687-post508.html
            periodicTickTime = 1f;
            manaCost = 0.23f * TreeConstants.getBaseMana(calcs.LocalCharacter.Race, calcOpts.level);
            healingBonus = calculatedStats.SpellPower * 1.88f + calculatedStats.AverageHeal * calcOpts.averageSpellpowerUsage / 100f;

            periodicTicks = 7;

            #region periodicTick
            if (calcOpts.level == 70)
            {
                periodicTick = 123f; 
            }
            else if (calcOpts.level == 80)
            {
                periodicTick = 206f; 
            }
            #endregion

            calculateTalents(calcs.LocalCharacter.DruidTalents, calcOpts);
        }

        private void calculateTalents(DruidTalents druidTalents, CalculationOptionsTree calcOpts)
        {
            periodicTicks += 1 * druidTalents.NaturesSplendor;

            gcd *= 1 - 0.04f * druidTalents.GiftOfTheEarthmother;

            manaCost *= 1 - 0.2f * druidTalents.TreeOfLife;

            periodicTick +=
                periodicTick * 0.01f * druidTalents.Genesis +
                periodicTick * 0.02f * druidTalents.GiftOfNature +
                periodicTick * 0.06f * druidTalents.TreeOfLife +
                periodicTick * 0.02f * druidTalents.MasterShapeshifter * druidTalents.TreeOfLife;

            //coefHoT *= 1 + 0.04f * druidTalents.EmpoweredRejuvenation; Since it's required for Wild Growth --> already in the coef

            coefHoT +=
                coefHoT * 0.01f * druidTalents.Genesis +
                coefHoT * 0.05f * druidTalents.ImprovedRejuvenation +
                coefHoT * 0.02f * druidTalents.GiftOfNature +
                coefHoT * 0.06f * druidTalents.TreeOfLife +
                coefHoT * 0.02f * druidTalents.MasterShapeshifter * druidTalents.TreeOfLife;
        }
    }
}
