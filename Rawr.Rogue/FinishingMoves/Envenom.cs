using System;
using Rawr.Rogue.ComboPointGenerators;
using Rawr.Rogue.Poisons;

namespace Rawr.Rogue.FinishingMoves
{
    [Serializable]
    public class Envenom : FinisherBase
    {
        public override char Id
        {
            get { return 'N'; }
        }

        public override string Name
        {
            get { return "Envenom"; }
        }

        public override float EnergyCost( CombatFactors combatFactors, int rank )
        {
            var baseCost = 35f - Talents.RelentlessStrikes.Bonus;
            var missCost = baseCost * combatFactors.YellowMissChance * (1 - Talents.QuickRecovery.Bonus);
            var dodgeCost = baseCost * (Talents.SurpriseAttacks.HasPoints ? combatFactors.MhDodgeChance * (1 - Talents.QuickRecovery.Bonus) : 0);
            return baseCost + missCost + dodgeCost;
        }

        public override float CalcFinisherDPS( CalculationOptionsRogue calcOpts, Stats stats, CombatFactors combatFactors, int rank, float cycleTime, WhiteAttacks whiteAttacks )
        {
            var dpAverageStackSize = CalcAverageStackSize(calcOpts, whiteAttacks, rank);
            var damage = ( 75 + stats.AttackPower * 0.07f ) * dpAverageStackSize;
            damage *= Talents.VilePoisons.Multiplier;
            damage *= Talents.FindWeakness.Multiplier;
            return damage / cycleTime;
        }

        private static float CalcAverageStackSize(CalculationOptionsRogue calcOpts, WhiteAttacks whiteAttacks, int rank)
        {
            float totalHits = 0;
            if (calcOpts.TempMainHandEnchant.IsDeadlyPoison)
            {
                totalHits += whiteAttacks.MhHits;
                totalHits += calcOpts.CpGenerator.MhHitsNeeded(rank);
            }
            if (calcOpts.TempMainHandEnchant.IsDeadlyPoison)
            {
                totalHits += whiteAttacks.OhHits;
                totalHits += calcOpts.CpGenerator.OhHitsNeeded(rank);
            }

            return Math.Min(rank, Math.Min(5, totalHits / DeadlyPoison.ChanceToApplyPoison));
        }
    }
}
