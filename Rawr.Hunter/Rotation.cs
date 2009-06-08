using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Hunter
{
    public class Rotation
    {

        public static float getDPS(CalculationOptionsHunter options, CharacterCalculationsHunter calc)
        {
            double dps = 0.0f;

            if (options.AimedInRot)
                dps += calc.aimedDPCD;

            if (options.ArcaneInRot)
                dps += calc.arcaneDPS;

            if (options.BlackInRot)
                dps += calc.BlackDPS;

            if (options.ChimeraInRot)
                dps += calc.ChimeraShotDPS;

            if (options.ExplosiveInRot)
                dps += calc.ExplosiveShotDPS;

            if (options.KillInRot)
                dps += calc.KillDPS;

            if (options.MultiInRot)
                dps += calc.multiDPCD;

            if (options.SerpentInRot)
                dps += calc.SerpentDPS;

            if (options.SilenceInRot)
                dps += calc.SilencingDPS;

            if (options.SteadyInRot)
                dps += calc.steadyDPS;


            return (float)dps;
        }
        
    }
}
