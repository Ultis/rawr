namespace Rawr.Rogue.ComboPointGenerators
{
    public interface IComboPointGenerator 
    {
        string Name { get; }
        float EnergyCost { get; }
        float CalcCpgDPS(Stats stats, CombatFactors combatFactors, CalculationOptionsRogue calcOpts, float numCPG, float cycleTime);
        float Crit(CombatFactors combatFactors);
    }

    public static class ComboPointGenerator
    {
        public static IComboPointGenerator Get(RogueTalents talents, CombatFactors combatFactors)
        {
            // if we have mutilate and we're using two daggers, assume we use it to generate CPs
            if (talents.Mutilate > 0 &&
                combatFactors.MainHand.Type == Item.ItemType.Dagger &&
                combatFactors.OffHand.Type == Item.ItemType.Dagger)
            {
                return new Mutilate(talents);
            }

            // if we're main handing a dagger, assume we're using backstab it to generate CPs
            if (combatFactors.MainHand.Type == Item.ItemType.Dagger)
            {
                return new Backstab(talents);
            }

            // if we have hemo, assume we use it to generate CPs
            if (talents.Hemorrhage > 0)
            {
                return new Hemo(talents);
            }

            // otherwise use sinister strike
            return new SinisterStrike(talents);
        }
    }
}