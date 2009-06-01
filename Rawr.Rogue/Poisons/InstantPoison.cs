using System;
using Rawr.Rogue.ClassAbilities;

namespace Rawr.Rogue.Poisons
{
    [Serializable]
    public class InstantPoison : PoisonBase
    {
        private const float PROCS_PER_MINUTE = 8.53f;

        public override string Name { get { return "Instant Poison"; } }

        public override bool IsDeadlyPoison { get { return false; } }

        public override float CalcPoisonDps( Stats stats, CalculationOptionsRogue calcOpts, CombatFactors combatFactors, float hits, CycleTime cycleTime, Item weapon )
        {
            var damage = hits * ChanceToApply(weapon) * (300f + .10f * stats.AttackPower);
            damage *= combatFactors.ProbPoisonHit;
            damage *= Talents.Add(Talents.VilePoisons, Talents.HungerForBlood.Damage).Multiplier;
            return damage/cycleTime.Duration;
        }

        private static float ChanceToApply(Item weapon)
        {
            return BaseChanceToApply(weapon) > 1f ? 1f : BaseChanceToApply(weapon);
        }

        private static float BaseChanceToApply(Item weapon)
        {
            return Talents.ImprovedPoisons.InstantPoison.Multiplier * (PROCS_PER_MINUTE / (60 / weapon.Speed));
        }
    }
}