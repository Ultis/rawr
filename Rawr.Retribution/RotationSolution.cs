using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Retribution
{
    public class RotationSolution
    {
        
        private float[] abilityUsagesPerSecond = new float[(int)Ability.Last + 1];
        private float[] abilityEffectiveCooldowns = new float[(int)Ability.Last + 1];


        public float GetAbilityUsagePerSecond(Ability ability)
        {
            return abilityUsagesPerSecond[(int)ability];
        }

        public void SetAbilityUsagePerSecond(Ability ability, float usagePerSecond)
        {
            abilityUsagesPerSecond[(int)ability] = usagePerSecond;
        }

        public float GetAbilityEffectiveCooldown(Ability ability)
        {
            return abilityEffectiveCooldowns[(int)ability];
        }

        public void SetAbilityEffectiveCooldown(Ability ability, float effectiveCooldown)
        {
            abilityEffectiveCooldowns[(int)ability] = effectiveCooldown;
        }

    }
}