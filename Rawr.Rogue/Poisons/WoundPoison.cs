using System;

namespace Rawr.Rogue.Poisons
{
    [Serializable]
    public class WoundPoison : PoisonBase
    {
        private const float PROCS_PER_MINUTE = 21.43f;

        public override string Name { get { return "Wound Poison"; } }

        public override bool IsDeadlyPoison { get { return false; } }

        public override float CalcPoisonDps( Stats stats, CalculationOptionsRogue calcOpts, CombatFactors combatFactors, float hits, CycleTime cycleTime, Item weapon )
        {
            var baseDamage = (231f + 0.04f*stats.AttackPower)*Talents.VilePoisons.Multiplier;
            var poisonHits = hits * ChanceToApply(weapon) * combatFactors.ProbPoisonHit;
            return baseDamage*poisonHits/cycleTime.Duration;
        }

        private static float ChanceToApply(Item weapon)
        {
            return BaseChanceToApply(weapon) > 1f ? 1f : BaseChanceToApply(weapon);
        }

        private static float BaseChanceToApply(Item weapon)
        {
            return PROCS_PER_MINUTE / (60 / weapon.Speed);
        }
    }
}