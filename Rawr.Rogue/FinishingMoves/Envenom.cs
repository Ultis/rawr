using System;
using Rawr.Rogue.ClassAbilities;
using Rawr.Rogue.Poisons;

namespace Rawr.Rogue.FinishingMoves
{
#if (SILVERLIGHT == false)
    [Serializable]
#endif
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
            float baseCost = 35f;
            float rsBonus = 25f * rank * Talents.RelentlessStrikes.Bonus;
            float missCost  = (Talents.SurpriseAttacks.HasPoints) ? 0 : 35f * (1f - Talents.QuickRecovery.Bonus) * combatFactors.YellowMissChance;

            return baseCost - rsBonus + missCost;
        }

        public override float CalcFinisherDPS( CalculationOptionsRogue calcOpts, Stats stats, CombatFactors combatFactors, int rank, CycleTime cycleTime, WhiteAttacks whiteAttacks, CharacterCalculationsRogue displayValues )
        {
            var dpAverageStackSize = CalcAverageStackSize(calcOpts, combatFactors, whiteAttacks, rank);
            var damage = ( 75 + stats.AttackPower * 0.09f ) * dpAverageStackSize;
            damage *= Talents.Add(Talents.VilePoisons, Talents.FindWeakness, Talents.Murder, Talents.HungerForBlood.Damage).Multiplier * (1f + stats.BonusNatureDamageMultiplier) * 0.95f;

            var nonCritDamage = damage * ( 1 - CritChance(combatFactors, calcOpts) );
            var critDamage = damage * 2 * CritChance(combatFactors, calcOpts);
            return (nonCritDamage + critDamage) / cycleTime.Duration;
        }

        private static float CalcAverageStackSize(CalculationOptionsRogue calcOpts, CombatFactors combatFactors, WhiteAttacks whiteAttacks, int rank)
        {
            float totalHits = 0;
            if (calcOpts.TempMainHandEnchant.IsDeadlyPoison)
            {
                totalHits += whiteAttacks.MhHits;
                totalHits += calcOpts.CpGenerator.MhHitsNeeded(combatFactors, calcOpts);
            }
            if (calcOpts.TempOffHandEnchant.IsDeadlyPoison)
            {
                totalHits += whiteAttacks.OhHits;
                totalHits += calcOpts.CpGenerator.OhHitsNeeded(combatFactors, calcOpts);
            }

            return Math.Min(rank, Math.Min(5, totalHits / DeadlyPoison.ChanceToApply(true)));
        }

        private float CritChance(CombatFactors combatFactors, CalculationOptionsRogue calcOpts)
        {
            return combatFactors.ProbMhCrit + CritBonusFromTurnTheTables(calcOpts);
        }
    }
}
