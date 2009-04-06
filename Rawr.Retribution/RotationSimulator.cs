using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Retribution
{
    static class RotationSimulator
    {
        private static Dictionary<RotationParameters, RotationSolution> savedSolutions = new Dictionary<RotationParameters, RotationSolution>(new RotationParameters.RotationComparer());

        public static RotationSolution SimulateRotation(RotationParameters rot)
        {
            if (savedSolutions.ContainsKey(rot)) return savedSolutions[rot];

            RotationSolution sol = new RotationSolution();
            float currentTime = 0;
            sol.FightLength = 10000;
            SimulatorAbility.Delay = rot.Delay;
            SimulatorAbility.Wait = rot.Wait;
            
            SimulatorAbility[] abilities = new SimulatorAbility[6];

            abilities[(int)RotationParameters.Ability.Judgement] = new SimulatorAbility(rot.T7_4pc ? 7 : 8);
            abilities[(int)RotationParameters.Ability.CrusaderStrike] = new SimulatorAbility(6);
            abilities[(int)RotationParameters.Ability.DivineStorm] = new SimulatorAbility(10);
            abilities[(int)RotationParameters.Ability.Consecration] = new SimulatorAbility(rot.GlyphConsecrate ? 10 : 8);
            abilities[(int)RotationParameters.Ability.Exorcism] = new SimulatorAbility(15);
            abilities[(int)RotationParameters.Ability.HammerOfWrath] = new SimulatorAbility(6);

            abilities[(int)RotationParameters.Ability.HammerOfWrath].NextUse = sol.FightLength * (1f - rot.TimeUnder20);

            bool gcdUsed;
            float minNext, tryUse;

            while (currentTime < sol.FightLength)
            {
                gcdUsed = false;
                foreach (RotationParameters.Ability ability in rot.Priorities)
                {
                    tryUse = abilities[(int)ability].UseAbility(currentTime);
                    if (tryUse > 0)
                    {
                        currentTime = tryUse;
                        gcdUsed = true;
                        break;
                    }
                }
                if (!gcdUsed)
                {
                    minNext = sol.FightLength;
                    foreach (SimulatorAbility simab in abilities)
                    {
                        if (simab.NextUse < minNext) minNext = simab.NextUse;
                    }
                    currentTime = minNext;
                }
            }

            sol.Judgement = abilities[(int)RotationParameters.Ability.Judgement].Uses;
            sol.JudgementCD = abilities[(int)RotationParameters.Ability.Judgement].EffectiveCooldown();

            sol.CrusaderStrike = abilities[(int)RotationParameters.Ability.CrusaderStrike].Uses;
            sol.CrusaderStrikeCD = abilities[(int)RotationParameters.Ability.CrusaderStrike].EffectiveCooldown();

            sol.DivineStorm = abilities[(int)RotationParameters.Ability.DivineStorm].Uses;
            sol.DivineStormCD = abilities[(int)RotationParameters.Ability.DivineStorm].EffectiveCooldown();

            sol.Consecration = abilities[(int)RotationParameters.Ability.Consecration].Uses;
            sol.ConsecrationCD = abilities[(int)RotationParameters.Ability.Consecration].EffectiveCooldown();

            sol.Exorcism = abilities[(int)RotationParameters.Ability.Exorcism].Uses;
            sol.ExorcismCD = abilities[(int)RotationParameters.Ability.Exorcism].EffectiveCooldown();

            sol.HammerOfWrath = abilities[(int)RotationParameters.Ability.HammerOfWrath].Uses;
            sol.HammerOfWrathCD = abilities[(int)RotationParameters.Ability.HammerOfWrath].EffectiveCooldown();

            savedSolutions[rot] = sol;

            return sol;
        }
    }
}
