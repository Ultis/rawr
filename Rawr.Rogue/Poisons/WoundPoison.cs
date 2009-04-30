using System;

namespace Rawr.Rogue.Poisons
{
    [Serializable]
    public class WoundPoison : PoisonBase
    {
        public override string Name { get { return "Wound Poison"; } }

        public override bool IsDeadlyPoison { get { return false; } }

        public override float CalcPoisonDPS(Stats stats, CalculationOptionsRogue calcOpts, CombatFactors combatFactors, float hits)
        {
            return hits * combatFactors.ProbPoisonHit * (231f + 0.04f * stats.AttackPower) * 0.5f * Talents.VilePoisons.Multiplier;
        }
    }
}