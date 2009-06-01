using System;
using Rawr.Rogue.ClassAbilities;
using Rawr.Rogue.Poisons;

namespace Rawr.Rogue.FinishingMoves
{
    [Serializable]
    public class Envenom : FinisherBase
    {
        public const string NAME = "Envenom";
        public override char Id
        {
            get { return 'N'; }
        }

        public override string Name
        {
            get { return NAME; }
        }

        public override float EnergyCost( CombatFactors combatFactors, int rank )
        {
            var baseCost = 35f - Talents.RelentlessStrikes.Bonus;
            var missCost = baseCost * combatFactors.YellowMissChance * (1 - Talents.QuickRecovery.Bonus);
            var dodgeCost = baseCost * (Talents.SurpriseAttacks.HasPoints ? combatFactors.MhDodgeChance * (1 - Talents.QuickRecovery.Bonus) : 0);
            return baseCost + missCost + dodgeCost;
        }

        public override float CalcFinisherDPS( CalculationOptionsRogue calcOpts, Stats stats, CombatFactors combatFactors, int rank, CycleTime cycleTime, WhiteAttacks whiteAttacks, CharacterCalculationsRogue displayValues )
        {
            var dpAverageStackSize = CalcAverageStackSize(calcOpts, whiteAttacks, rank);
            var damage = ( 75 + stats.AttackPower * 0.07f ) * dpAverageStackSize;
            damage *= Talents.Add(Talents.VilePoisons, Talents.FindWeakness, Talents.HungerForBlood.Damage).Multiplier;

            var nonCritDamage = damage * ( 1 - CritChance(combatFactors, calcOpts) );
            var critDamage = damage * 2 * CritChance(combatFactors, calcOpts);
            return (nonCritDamage + critDamage) / cycleTime.Duration;
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

        private float CritChance(CombatFactors combatFactors, CalculationOptionsRogue calcOpts)
        {
            return combatFactors.ProbMhCrit + CritBonusFromTurnTheTables(calcOpts);
        }
    }
}
