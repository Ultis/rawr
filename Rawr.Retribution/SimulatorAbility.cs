using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Retribution
{
    public class SimulatorAbility
    {
        public SimulatorAbility(float cooldown)
        {
            Cooldown = cooldown;
            GlobalCooldown = 1.5f;
            FirstUse = -1;
            LastUse = -1;
            NextUse = 0;
        }

        public static float Wait { get; set; }
        public static float Delay { get; set; }

        public float GlobalCooldown { get; set; }
        public float Cooldown { get; set; }
        public float FirstUse { get; set; }
        public float LastUse { get; set; }
        public float NextUse { get; set; }
        public int Uses { get; set; }

        public void ResetCooldown(float currentTime)
        {
            // CD reset will be visible only after delay
            NextUse = Math.Min(NextUse, currentTime + Delay);
        }

        /// <summary>
        /// Uses the ability next time it's available.
        /// </summary>
        /// <param name="currentTime">Current time</param>
        /// <returns>GCD finish time</returns>
        public float UseAbility(float currentTime)
        {
            var usageTime = currentTime + Delay;

            if (FirstUse < 0) 
                FirstUse = usageTime;
            LastUse = usageTime;
            NextUse = usageTime + Cooldown;
            Uses++;
            return usageTime + GlobalCooldown;
        }

        public bool ShouldAbilityBeUsedNext(float currentTime)
        {
            return NextUse <= currentTime + Wait;
        }

        public bool CanAbilityBeUsedNow(float currentTime)
        {
            return currentTime >= NextUse;
        }

        public float GetNextUseTime(float currentTime)
        {
            return Math.Max(currentTime, NextUse);
        }

        public float EffectiveCooldown() { return (LastUse - FirstUse) / (Uses - 1); }
    }
}