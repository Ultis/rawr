using System;

namespace Rawr.Rogue.Poisons
{
    [Serializable]
    public class InstantPoison : PoisonBase
    {
        private const float BASE_CHANCE_TO_APPLY = .2f;

        public override string Name { get { return "Instant Poison"; } }

        public override bool IsDeadlyPoison { get { return false; } }

        public override float CalcPoisonDps(Stats stats, CalculationOptionsRogue calcOpts, CombatFactors combatFactors, float hits, float cycleTime)
        {
            var damage = hits * (300f + .10f * stats.AttackPower);
            damage *= (BASE_CHANCE_TO_APPLY + Talents.ImprovedPoisons.InstantPoison.Bonus);
            damage *= combatFactors.ProbPoisonHit;
            damage *= Talents.VilePoisons.Multiplier;
            return damage/cycleTime;
        }
    }
}