namespace Rawr.Rogue
{
    public class CycleTime
    {
        public CycleTime(CalculationOptionsRogue calcOpts, CombatFactors combatFactors, WhiteAttacks whiteAttacks)
        {
            Duration = CalcCycleTime(calcOpts, combatFactors, whiteAttacks);
        }

        public float Duration { get; set; }

        private static float CalcCycleTime(CalculationOptionsRogue calcOpts, CombatFactors combatFactors, WhiteAttacks whiteAttacks)
        {
            var energyRegen = combatFactors.BaseEnergyRegen;
            energyRegen += Talents.CombatPotency.Bonus * whiteAttacks.OhHits;
            energyRegen += Talents.FocusedAttacks.Bonus * (whiteAttacks.MhCrits + whiteAttacks.OhCrits);

            if (calcOpts.TempMainHandEnchant.IsDeadlyPoison || calcOpts.TempOffHandEnchant.IsDeadlyPoison)
            {
                energyRegen += combatFactors.Tier8TwoPieceEnergyBonus;
            }

            var cpgDuration = calcOpts.CpGenerator.CalcDuration(calcOpts, energyRegen, combatFactors);

            var finisherEnergyCost = 0f;
            foreach (var component in calcOpts.DpsCycle.Components)
            {
                finisherEnergyCost += component.Finisher.EnergyCost(combatFactors, component.Rank);
            }

            return cpgDuration + (finisherEnergyCost / energyRegen);
        }
    }
}
