using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Retribution
{
    public class SimulatorAbility
    {

        public static float Wait { get; set; }
        public static float Delay { get; set; }

        public float Cooldown { get; set; }
        public float FirstUse { get; set; }
        public float LastUse { get; set; }
        public float NextUse { get; set; }
        public int Uses { get; set; }

        public SimulatorAbility(float cooldown)
        {
            this.Cooldown = cooldown;
            FirstUse = -1;
            LastUse = -1;
            NextUse = 0;
        }

        public bool UseAbility(float currentTime)
        {
            if (NextUse <= currentTime + Wait)
            {
                if (NextUse > currentTime) currentTime = NextUse;
                if (FirstUse < 0) FirstUse = currentTime;
                LastUse = currentTime;
                NextUse = currentTime + Cooldown;
                Uses++;
                return true;
            }
            else
            {
                return false;
            }
        }

        public float EffectiveCooldown()
        {
            return (LastUse - FirstUse) / (Uses - 1);
        }

    }
}