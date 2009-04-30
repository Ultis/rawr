using System;

namespace Rawr.Rogue.FinishingMoves
{
    [Serializable]
    public class Envenom : FinisherBase
    {
        public override char Id
        {
            get { return 'n'; }
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

        public override float CalcFinisherDPS( Stats stats, CombatFactors combatFactors, int rank, float cycleTime, WhiteAttacks whiteAttacks )
        {
            return 0;
        }
    }
}
