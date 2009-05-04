using System;

namespace Rawr.Rogue.Poisons
{
    [Serializable]
    public class WoundPoison : PoisonBase
    {
        private const float BASE_CHANCE_TO_APPLY = .5f;

        public override string Name { get { return "Wound Poison"; } }

        public override bool IsDeadlyPoison { get { return false; } }

        public override float CalcPoisonDps(Stats stats, CalculationOptionsRogue calcOpts, CombatFactors combatFactors, float hits, float cycleTime)
        {
            var baseDamage = (231f + 0.04f*stats.AttackPower)*Talents.VilePoisons.Multiplier;
            var poisonHits = hits*BASE_CHANCE_TO_APPLY*combatFactors.ProbPoisonHit;
            return baseDamage*poisonHits/cycleTime;
        }
    }
}