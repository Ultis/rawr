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
            float baseDamage = (296f + .12f * stats.AttackPower);
            baseDamage *= (1f + stats.BonusNatureDamageMultiplier) * (1f + stats.BonusDamageMultiplier);
            baseDamage *= (1f + Talents.VilePoisons.Bonus) * (1f + Talents.HungerForBlood.Damage.Bonus);
            baseDamage *= (calcOpts.TargetIsValidForMurder) ? (1f + Talents.Murder.Bonus) : 1f;
            baseDamage *= (1f - combatFactors.PoisonDamageReduction);

            float countEnve = 0;
            float timeEnvnProc = 0;
            float consumeDP = 0;

            foreach (CycleComponent comp in calcOpts.DpsCycle.Components)
            {
                if (comp.Finisher.Name == "Envenom")
                {
                    countEnve++;
                    timeEnvnProc += (float)comp.Rank + 1f;
                    consumeDP += (float)comp.Rank * (1f - Talents.MasterPoisoner.NotConsumeDeadlyPoison.Bonus);
        }
            }

            hits = hits * (cycleTime.Duration - timeEnvnProc) / cycleTime.Duration * ChanceToApply(false) +
                   hits * timeEnvnProc / cycleTime.Duration * ChanceToApply(true);

            float avgHitTime = cycleTime.Duration / hits;

            float sDPPerSec = (STACK_SIZE - consumeDP / countEnve) * baseDamage / DURATION;
            float eDPPerSec = STACK_SIZE * baseDamage / DURATION;

            float damageStacking = (sDPPerSec + eDPPerSec) * .5f * consumeDP * avgHitTime;
            float damageStacked = eDPPerSec * (cycleTime.Duration - consumeDP * avgHitTime);

            return (damageStacking + damageStacked) / cycleTime.Duration;
        }

        public static float ChanceToApply(bool bEnvenom)
        {
            float BaseChanceToApply = .3f + Talents.ImprovedPoisons.DeadlyPoison.Bonus;

            if (bEnvenom) BaseChanceToApply += 0.15f;

            return (BaseChanceToApply > 1f) ? 1f : BaseChanceToApply;
        }
    }
}

