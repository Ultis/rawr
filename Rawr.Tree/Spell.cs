using System;

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
        { 
            get 
            {
                if (castTime > gcd)
                    return castTime;
                else if (gcd > 1)
                    return gcd;
                else
                    return 1;
            }
        }
        public float gcd = 1.5f;
        public float manaCost = 0f;
        public float coefDH = 0f; //coef for DirectHeal

        protected float healingBonus = 0f;
        virtual public float HealingBonus
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
        public float PeriodicTickTime
        { get { return periodicTickTime; } }
        public float coefHoT = 0f; //coef for HoT

        public float AverageHealing
        { get { return (minHeal + maxHeal) / 2 + HealingBonus * coefDH; } }

        public float AverageHealingwithCrit
        { get { return (AverageHealing * (100 - critPercent) + (AverageHealing * critModifier) * critPercent) / 100; } }

        virtual public float PeriodicTick
        { get { return periodicTick + healingBonus * coefHoT; } }

        virtual public float HPS
        { get { return AverageHealingwithCrit / CastTime; } }

        public float HPSHoT
        { get { return (PeriodicTick) / periodicTickTime; } }

        public float HPM
        { get { return (AverageHealing + PeriodicTick * PeriodicTicks) / manaCost; } }

        public float Duration
        { get { return periodicTicks * periodicTickTime; } }

        public void Initialize(float hasteRating)
        {
            gcd = 1.5f / (1 + hasteRating / TreeConstants.hasteconversation);
        }
    }

    public class HealingTouch : Spell
    {
        public HealingTouch(CharacterCalculationsTree calcs)
        {
            Stats calculatedStats = calcs.BasicStats;
            CalculationOptionsTree calcOpts = (CalculationOptionsTree)calcs.LocalCharacter.CalculationOptions;

            base.Initialize(calculatedStats.HasteRating);

            castTime = 3f;
            coefDH = castTime / 3.5f;
            manaCost = 0.33f * TreeConstants.getBaseMana(calcs.LocalCharacter.Race);
            healingBonus = calculatedStats.SpellPower * 1.88f + calculatedStats.AverageHeal * calcOpts.averageSpellpowerUsage/100f;
            critPercent = calculatedStats.SpellCrit;

            #region minHeal, maxHeal
            //if (calcOpts.level == 70)
            //{
            //    minHeal = 2321f;
            //    maxHeal = 2739f;
            //}
            //else if (calcOpts.level == 80)
            //{
                minHeal = 3750f;
                maxHeal = 4428f;
            //}
            #endregion

            calculateTalents(calcs.LocalCharacter.DruidTalents, calcOpts);

            castTime = (float)Math.Round(castTime / (1 + calculatedStats.HasteRating / TreeConstants.hasteconversation), 4);

            //guessed that it doesnt work with talents
            //z.B.: Idol of the Avian Heart (+136 Healing)
            healingBonus += calculatedStats.HealingTouchFinalHealBonus;

            //z.B.: Idol of Longevity (25 Mana on cast.... -25 Manacost)
            manaCost -= calculatedStats.ReduceHealingTouchCost;

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
            
            minHeal *=
                (1 + 0.02f * druidTalents.GiftOfNature) *
                (1 + 0.02f * druidTalents.MasterShapeshifter * druidTalents.TreeOfLife) *
                (1 + 0.06f * druidTalents.TreeOfLife);

            maxHeal *=
                (1 + 0.02f * druidTalents.GiftOfNature) *
                (1 + 0.02f * druidTalents.MasterShapeshifter * druidTalents.TreeOfLife) *
                (1 + 0.06f * druidTalents.TreeOfLife);

            coefDH *= 1 + 0.125f * druidTalents.EmpoweredTouch;
            //seems it only adds 25% instead of 40% ..

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

            base.Initialize(calculatedStats.HasteRating);

            castTime = 2f;
            coefDH = 0.3f; //0.289f; It seems the DH coef got Buffed a bit
            coefHoT = 0.7f / 7f;
            manaCost = 0.29f * TreeConstants.getBaseMana(calcs.LocalCharacter.Race);
            healingBonus = calculatedStats.SpellPower * 1.88f + calculatedStats.AverageHeal * calcOpts.averageSpellpowerUsage / 100f;
            critPercent = calculatedStats.SpellCrit;

            periodicTicks = 7;

            #region minHeal, maxHeal, periodicTick
            //if (calcOpts.level == 70)
            //{
            //    minHeal = 1215;
            //    maxHeal = 1355;
            //    periodicTick = 182;
            //}
            //else if (calcOpts.level == 80)
            //{
                minHeal = 1215;
                maxHeal = 1355f;
                periodicTick = 182;
            //}
            #endregion

            calculateTalents(calcs.LocalCharacter.DruidTalents, calcOpts);

            //z.B.: Idol of the Crescent Goddess (-65 Mana)
            manaCost -= calculatedStats.ReduceRegrowthCost;

            castTime = (float)Math.Round(castTime / (1 + calculatedStats.HasteRating / TreeConstants.hasteconversation), 4);
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

            base.Initialize(calculatedStats.HasteRating);

            castTime = 0f;
            coefHoT = 0.8f / 4f;
            manaCost = 0.18f * TreeConstants.getBaseMana(calcs.LocalCharacter.Race);
            healingBonus = calculatedStats.SpellPower * 1.88f + calculatedStats.AverageHeal * calcOpts.averageSpellpowerUsage / 100f;

            periodicTicks = 4;

            #region periodicTick
            //if (calcOpts.level == 70)
            //{
            //    periodicTick = 265f; //1060 / 4
            //    periodicTicks = 4;
            //}
            //else if (calcOpts.level == 80)
            //{
                periodicTick = 338f; //1690 / 5 ... newest Rank got 15 seconds.. i hope it's not involed with another coef ..
                periodicTicks = 5;
            //}
            #endregion

            calculateTalents(calcs.LocalCharacter.DruidTalents, calcOpts);

            //z.B.: Harold's Rejuvenating Broach
            healingBonus += calculatedStats.RejuvenationHealBonus;

            //z.B.: Idol of Budding Life (-36 Manacost)
            manaCost -= calculatedStats.ReduceRejuvenationCost;

            gcd = (float)Math.Round(gcd / (1 + calculatedStats.HasteRating / TreeConstants.hasteconversation), 4);
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
        protected float idolHoTModifier = 0f;
        protected float idolDHModifier = 0f;

        public override float PeriodicTick
        { get { return periodicTick + idolHoTModifier + healingBonus * coefHoT; } }

        public override float HealingBonus
        { 
            get { return healingBonus + idolDHModifier; }
            set { healingBonus = value; }
        }

        public Lifebloom(CharacterCalculationsTree calcs)
        {
            Stats calculatedStats = calcs.BasicStats;
            CalculationOptionsTree calcOpts = (CalculationOptionsTree)calcs.LocalCharacter.CalculationOptions;

            base.Initialize(calculatedStats.HasteRating);

            castTime = 0f;
            periodicTickTime = 1f;
            coefDH = 0.3434f; //0.342 doesn't seem accurate too
            coefHoT = 0.05055f; //0.44f / 7f; have to test it even more ... 0.44f/7f isn't true anymore .. its afaik lower
            manaCost = 0.14f * TreeConstants.getBaseMana(calcs.LocalCharacter.Race);
            healingBonus = calculatedStats.SpellPower * 1.88f + calculatedStats.AverageHeal * calcOpts.averageSpellpowerUsage / 100f;
            critPercent = calculatedStats.SpellCrit;

            periodicTicks = 7;

            #region periodicTick
            //if (calcOpts.level == 70)
            //{
            //    minHeal = 600f;
            //    maxHeal = 600f;
            //    periodicTick = 32f; 
            //}
            //else if (calcOpts.level == 80)
            //{
                minHeal = 970f;
                maxHeal = 970f;
                periodicTick = 53f; 
            //}
            #endregion

            //z.B.: Idol of the Emerald Queen
            if (calculatedStats.LifebloomTickHealBonus == 47)
                idolHoTModifier += 12.4f; //The Idol adds 12.4 Health per Tick, affected by Genesis / Gift

            calculateTalents(calcs.LocalCharacter.DruidTalents, calcOpts);

            gcd = (float)Math.Round(gcd  / (1 + calculatedStats.HasteRating / TreeConstants.hasteconversation), 4);

            //z.B.: Gladiator's Idol of Tenacity (87 on final heal), haven't one myself, will correct it when i've one
            idolDHModifier += calculatedStats.LifebloomFinalHealBonus;

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
            idolHoTModifier +=
                idolHoTModifier * 0.01f * druidTalents.Genesis +
                idolHoTModifier * 0.02f * druidTalents.GiftOfNature;


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
        public override float PeriodicTick
        { get { return periodicTick + idolHoTModifier * 3 + healingBonus * coefHoT; } }

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
        float[][] tickRanks = new float[][]
        { 
             new float[] {176f, 157f, 140f, 123f, 105f, 88f, 70f}, //mostly observation
             //quessed..
             new float[] {295f, 263f, 234f, 206f, 176f, 147f, 117f} //Rank 4
        };

        /// <summary>
        /// returns the Tick of WG, Ranks: 0 = Rank 2, 1 = Rank 4, valid index - 0 to 6
        /// </summary>
        public float getTick(int rank, int index)
        { return healingBonus * coefHoT + tickRanks[rank][index];  }

        public WildGrowth(CharacterCalculationsTree calcs)
        {
            Stats calculatedStats = calcs.BasicStats;
            CalculationOptionsTree calcOpts = (CalculationOptionsTree)calcs.LocalCharacter.CalculationOptions;

            base.Initialize(calculatedStats.HasteRating);

            castTime = 0f;
            coefHoT = 0.96f / 7f; // Haven't fully tested yet, 1,88 coef included. (w/o Empowered Rejuvenation it would be 0.8)
            periodicTickTime = 1f;
            manaCost = 0.23f * TreeConstants.getBaseMana(calcs.LocalCharacter.Race);
            healingBonus = calculatedStats.SpellPower + calculatedStats.AverageHeal * calcOpts.averageSpellpowerUsage / 100f;

            periodicTicks = 7;

            #region periodicTick
            //if (calcOpts.level == 70)
            //{
                periodicTick = 123f; 
            //}
            //else if (calcOpts.level == 80)
            //{
                //periodicTick = 206f; 
            //}
            #endregion

            calculateTalents(calcs.LocalCharacter.DruidTalents, calcOpts);

            gcd = (float)Math.Round(gcd / (1 + calculatedStats.HasteRating / TreeConstants.hasteconversation), 4);
        }

        private void calculateTalents(DruidTalents druidTalents, CalculationOptionsTree calcOpts)
        {
            gcd *= 1 - 0.04f * druidTalents.GiftOfTheEarthmother;

            manaCost *= 1 - 0.2f * druidTalents.TreeOfLife;

            periodicTick +=
                periodicTick * 0.01f * druidTalents.Genesis +
                periodicTick * 0.02f * druidTalents.GiftOfNature;

            periodicTick *=
                (1 + 0.06f * druidTalents.TreeOfLife) *
                (1 + 0.02f * druidTalents.MasterShapeshifter * druidTalents.TreeOfLife);

            coefHoT +=
                coefHoT * 0.01f * druidTalents.Genesis +
                coefHoT * 0.02f * druidTalents.GiftOfNature +
                coefHoT * 0.05f * druidTalents.ImprovedRejuvenation;

            coefHoT *=
                (1 + 0.06f * druidTalents.TreeOfLife) *
                (1 + 0.02f * druidTalents.MasterShapeshifter * druidTalents.TreeOfLife);

            float basecoef = 1f;
            basecoef +=
                0.01f * druidTalents.Genesis +
                0.02f * druidTalents.GiftOfNature;

            basecoef *=
                (1 + 0.06f * druidTalents.TreeOfLife) *
                (1 + 0.02f * druidTalents.MasterShapeshifter);

            for (int i = 0; i <= 6; i++)
            { tickRanks[0][i] *= basecoef; }
            for (int i = 0; i <= 6; i++)
            { tickRanks[1][i] *= basecoef; }
        }
    }

    public class Nourish : Spell
    {
        public Nourish(CharacterCalculationsTree calcs)
        {
            InitializeNourish(calcs);
        }

        public Nourish(CharacterCalculationsTree calcs, bool hotActive)
        {
            InitializeNourish(calcs);
            minHeal *= 1.2f;
            maxHeal *= 1.2f;
            coefDH *= 1.2f;
        }

        private void InitializeNourish(CharacterCalculationsTree calcs)
        {
            Stats calculatedStats = calcs.BasicStats;
            CalculationOptionsTree calcOpts = (CalculationOptionsTree)calcs.LocalCharacter.CalculationOptions;

            base.Initialize(calculatedStats.HasteRating);

            castTime = 1.5f;
            coefDH = castTime / 3.5f;
            manaCost = 0.18f * TreeConstants.getBaseMana(calcs.LocalCharacter.Race);
            healingBonus = calculatedStats.SpellPower * 1.88f + calculatedStats.AverageHeal * calcOpts.averageSpellpowerUsage / 100f;
            critPercent = calculatedStats.SpellCrit;

            #region minHeal, maxHeal
            //if (calcOpts.level == 70)
            //{
            //    minHeal = 2321f;
            //    maxHeal = 2739f;
            //}
            //else if (calcOpts.level == 80)
            //{
            minHeal = 1883f;
            maxHeal = 2187f;
            //}
            #endregion

            calculateTalents(calcs.LocalCharacter.DruidTalents, calcOpts);

            castTime = (float)Math.Round(castTime / (1 + calculatedStats.HasteRating / TreeConstants.hasteconversation), 4);
        }

        private void calculateTalents(DruidTalents druidTalents, CalculationOptionsTree calcOpts)
        {
            manaCost *= 1 - (druidTalents.TranquilSpirit * 0.02f);

            critPercent += 2f * druidTalents.NaturesMastery;

            //Living Seed, 30% seed, 33% * points spend (1/3)
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
                (1 + 0.02f * druidTalents.GiftOfNature) *
                (1 + 0.02f * druidTalents.MasterShapeshifter * druidTalents.TreeOfLife) *
                (1 + 0.06f * druidTalents.TreeOfLife);
        }
    }

    public class Nothing : Spell
    {
        public Nothing(float time)
        {
            castTime = time;
        }

    }
}
