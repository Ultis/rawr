using System;

namespace Rawr.Rogue.FinishingMoves
{
    [Serializable]
    public class SnD : FinisherBase
    {
        public override char Id { get { return 'S'; } }
        public override string Name { get { return "SnD"; } }
        public override float EnergyCost(CombatFactors combatFactors, int rank)
        {
            return 25f - Talents.RelentlessStrikes.Bonus;
        }
        public override float CalcFinisherDPS(CalculationOptionsRogue calcOpts, Stats stats, CombatFactors combatFactors, int rank, CycleTime cycleTime, WhiteAttacks whiteAttacks)
        {
            return 0f;
        }
    }
}