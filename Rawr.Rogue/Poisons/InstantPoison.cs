using System;

namespace Rawr.Rogue.Poisons
{
    [Serializable]
    public class InstantPoison : PoisonBase
    {
        public override string Name { get { return "Instant Poison"; } }

        public override bool IsDeadlyPoison { get { return false; } }

        public override float CalcPoisonDPS(Stats stats, CalculationOptionsRogue calcOpts, CombatFactors combatFactors, float hits)
        {
            var damage = hits * (300f + .10f * stats.AttackPower);
            damage *= (.2f + Talents.ImprovedPoisons.InstantPoison.Bonus);
            damage *= combatFactors.ProbPoisonHit;
            damage *= Talents.VilePoisons.Multiplier;
            return damage;
        }
    }
}