using System;

namespace Rawr.Rogue.FinishingMoves
{
    [Serializable]
    public class NoFinisher : FinisherBase
    {
        public override char Id { get { return 'Z'; } }
        public override string Name { get { return "None"; } }
        public override float EnergyCost(CombatFactors combatFactors, int rank) { return 0f; }
        public override float CalcFinisherDPS( CalculationOptionsRogue calcOpts, Stats stats, CombatFactors combatFactors, int rank, float cycleTime, WhiteAttacks whiteAttacks )
        {
            return 0f;
        }
    }
}