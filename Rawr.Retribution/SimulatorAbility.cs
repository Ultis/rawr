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
            FirstUse = -1;
            LastUse = -1;
            NextUse = 0;
        }

        public static float Wait { get; set; }
        public static float Delay { get; set; }

        public float Cooldown { get; set; }
        public float FirstUse { get; set; }
        public float LastUse { get; set; }
        public float NextUse { get; set; }
        public int Uses { get; set; }

        public float UseAbility(float currentTime)
        {
            if (NextUse <= currentTime + Wait)
            {
                if (NextUse > currentTime) { currentTime = NextUse; }
                if (FirstUse < 0) { FirstUse = currentTime; }
                LastUse = currentTime;
                NextUse = currentTime + Cooldown + Delay;
                Uses++;
                return currentTime + 1.5f + Delay;
            }
            else { return -1; }
        }

        public float EffectiveCooldown() { return (LastUse - FirstUse) / (Uses - 1); }
    }
}