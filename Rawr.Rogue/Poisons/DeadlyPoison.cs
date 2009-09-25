using System;
using Rawr.Rogue.ClassAbilities;

namespace Rawr.Rogue.Poisons
{
#if (SILVERLIGHT == false)
    [Serializable]
#endif
    public class DeadlyPoison : PoisonBase
    {
        public override string Name { get { return "Deadly Poison"; } }

        private const float STACK_SIZE = 5f;
        private const float DURATION = 12f;

        public override bool IsDeadlyPoison { get { return true; } }

        public override float CalcPoisonDps( Stats stats, CalculationOptionsRogue calcOpts, CombatFactors combatFactors, float hits, CycleTime cycleTime, Item weapon )
        {
            //TODO:  model loss of stacks due to envenom.
            float baseDamage = (296f + .12f * stats.AttackPower);
            baseDamage *= (1f + stats.BonusNatureDamageMultiplier) * (1f + stats.BonusDamageMultiplier);
            baseDamage *= (1f + Talents.VilePoisons.Bonus) * (1f + Talents.HungerForBlood.Damage.Bonus);
            baseDamage *= (calcOpts.TargetIsValidForMurder) ? (1f + Talents.Murder.Bonus) : 1f;
            baseDamage *= (1f - combatFactors.PoisonDamageReduction);

            return STACK_SIZE * baseDamage / DURATION;
        }

        public static float ChanceToApplyPoison
        {
            get { return .3f + Talents.ImprovedPoisons.DeadlyPoison.Bonus + Talents.MasterPoisoner.DeadlyPoisonApplication.Bonus; }  
        }
    }
}

