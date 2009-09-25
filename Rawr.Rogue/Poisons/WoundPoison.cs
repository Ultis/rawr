using System;
using Rawr.Rogue.ClassAbilities;

namespace Rawr.Rogue.Poisons
{
#if (SILVERLIGHT == false)
    [Serializable]
#endif

    public class WoundPoison : PoisonBase
    {
        private const float PROCS_PER_MINUTE = 21.43f;

        public override string Name
        {
            get { return "Wound Poison"; }
        }

        public override bool IsDeadlyPoison
        {
            get { return false; }
        }

        public override float CalcPoisonDps( Stats stats, CalculationOptionsRogue calcOpts, CombatFactors combatFactors, float hits, CycleTime cycleTime, Item weapon )
        {
            float baseDamage = (231f + 0.04f * stats.AttackPower);
            baseDamage *= (1f + stats.BonusNatureDamageMultiplier) * (1f + stats.BonusDamageMultiplier);
            baseDamage *= (1f + Talents.VilePoisons.Bonus) * (1f + Talents.HungerForBlood.Damage.Bonus);
            baseDamage *= (calcOpts.TargetIsValidForMurder) ? (1f + Talents.Murder.Bonus) : 1f;
            baseDamage *= (1f - combatFactors.PoisonDamageReduction);

            float critDamage = baseDamage * combatFactors.BaseSpellCritMultiplier;

            hits *= ChanceToApply(weapon);

            float damage = hits * (1f - combatFactors.ProbPoisonCrit) * baseDamage;
            damage += hits * combatFactors.ProbPoisonCrit * critDamage;

            return damage / cycleTime.Duration;
        }

        private static float ChanceToApply( Item weapon )
        {
            return BaseChanceToApply(weapon) > 1f ? 1f : BaseChanceToApply(weapon);
        }

        private static float BaseChanceToApply( Item weapon )
        {
            return PROCS_PER_MINUTE / ( 60 / weapon.Speed );
        }
    }
}