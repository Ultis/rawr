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

            abilities[(int)Ability.Judgement] = new SimulatorAbility(rot.T7_4pc ? 7 : 8);
            abilities[(int)Ability.CrusaderStrike] = new SimulatorAbility(6);
            abilities[(int)Ability.DivineStorm] = new SimulatorAbility(10);
            abilities[(int)Ability.Consecration] = new SimulatorAbility(rot.GlyphConsecrate ? 10 : 8);
            abilities[(int)Ability.Exorcism] = new SimulatorAbility(15);
            abilities[(int)Ability.HammerOfWrath] = new SimulatorAbility(6);

            abilities[(int)Ability.HammerOfWrath].NextUse = sol.FightLength * (1f - rot.TimeUnder20);

            bool gcdUsed;
            float minNext, tryUse;

            while (currentTime < sol.FightLength)
            {
                gcdUsed = false;
                foreach (Ability ability in rot.Priorities)
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

            sol.Judgement = abilities[(int)Ability.Judgement].Uses;
            sol.JudgementCD = abilities[(int)Ability.Judgement].EffectiveCooldown();

            sol.CrusaderStrike = abilities[(int)Ability.CrusaderStrike].Uses;
            sol.CrusaderStrikeCD = abilities[(int)Ability.CrusaderStrike].EffectiveCooldown();

            sol.DivineStorm = abilities[(int)Ability.DivineStorm].Uses;
            sol.DivineStormCD = abilities[(int)Ability.DivineStorm].EffectiveCooldown();

            sol.Consecration = abilities[(int)Ability.Consecration].Uses;
            sol.ConsecrationCD = abilities[(int)Ability.Consecration].EffectiveCooldown();

            sol.Exorcism = abilities[(int)Ability.Exorcism].Uses;
            sol.ExorcismCD = abilities[(int)Ability.Exorcism].EffectiveCooldown();

            sol.HammerOfWrath = abilities[(int)Ability.HammerOfWrath].Uses;
            sol.HammerOfWrathCD = abilities[(int)Ability.HammerOfWrath].EffectiveCooldown();

            savedSolutions[rot] = sol;

            return sol;
        }
    }
}
