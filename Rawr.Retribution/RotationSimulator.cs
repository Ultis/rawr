using System;
using System.Collections.Generic;
using System.Text;
using Rawr.Base.Algorithms;

namespace Rawr.Retribution
{
    static class RotationSimulator
    {

        private static CalculationCache<RotationParameters, RotationSolution> solutionCache =
            new CalculationCache<RotationParameters, RotationSolution>(SimulateRotationCore);


        public static RotationSolution SimulateRotation(RotationParameters rot)
        {
            return solutionCache.GetResult(rot);
        }


        private static RotationSolution SimulateRotationCore(RotationParameters rot)
        {
            const float t10ProcChance = 0.4f;
            const int timeUnitsPerSecond = 100;
            const int meleeAbilityGcd = (int)(1.5m * timeUnitsPerSecond);

            int fightLength = (int)(rot.SimulationTime * timeUnitsPerSecond);
            int spellGcd = (int)(rot.SpellGCD * timeUnitsPerSecond);
            int t10Speed = (int)(rot.T10Speed * timeUnitsPerSecond);
            SimulatorAbility.Delay = (int)(rot.Delay * timeUnitsPerSecond);
            SimulatorAbility.Wait = (int)(rot.Wait * timeUnitsPerSecond);

            SimulatorAbility[] abilities = new SimulatorAbility[(int)Ability.Last + 1];

            abilities[(int)Ability.Judgement] = new SimulatorAbility(
                (10 - rot.ImpJudgements - (rot.T7_4pc ? 1 : 0)) * timeUnitsPerSecond,
                meleeAbilityGcd);
            abilities[(int)Ability.CrusaderStrike] = new SimulatorAbility(
                4 * timeUnitsPerSecond,
                meleeAbilityGcd);
            abilities[(int)Ability.DivineStorm] = new SimulatorAbility(
                10 * timeUnitsPerSecond,
                meleeAbilityGcd);
            abilities[(int)Ability.Consecration] = new SimulatorAbility(
                (rot.GlyphConsecrate ? 10 : 8) * timeUnitsPerSecond,
                spellGcd);
            abilities[(int)Ability.Exorcism] = new SimulatorAbility(
                15 * timeUnitsPerSecond,
                spellGcd);
            abilities[(int)Ability.HammerOfWrath] = new SimulatorAbility(
                6 * timeUnitsPerSecond,
                meleeAbilityGcd);

            int gcdFinishTime = 0;
            Random rand = new Random(6021987);
            int nextSwingTime = t10Speed;

            int currentTime = 0;
            while (currentTime < fightLength)
            {
                if (currentTime >= gcdFinishTime)
                {
                    foreach (Ability ability in rot.Priorities)
                    {
                        if (abilities[(int)ability].ShouldAbilityBeUsedNext(currentTime))
                        {
                            if (abilities[(int)ability].CanAbilityBeUsedNow(currentTime))
                                gcdFinishTime = abilities[(int)ability].UseAbility(currentTime);

                            break;
                        }
                    }
                }

                int nextTime = fightLength;
                if (currentTime >= gcdFinishTime)
                {
                    foreach (Ability ability in rot.Priorities)
                    {
                        int nextUseTime = abilities[(int)ability].GetNextUseTime(currentTime);
                        if (nextUseTime > currentTime)
                            nextTime = Math.Min(nextTime, nextUseTime);
                    }
                }
                else
                {
                    nextTime = Math.Min(nextTime, gcdFinishTime);
                }

                if (t10Speed > 0)
                {
                    while (nextTime > nextSwingTime)
                    {
                        if (rand.NextDouble() < t10ProcChance)
                        {
                            abilities[(int)Ability.DivineStorm].ResetCooldown(nextSwingTime);
                            nextTime = nextSwingTime;
                        }

                        nextSwingTime += t10Speed;
                    }
                }

                currentTime = nextTime;
            }

            float fightLengthInSeconds = ((float)fightLength) / timeUnitsPerSecond;
            RotationSolution solution = new RotationSolution();
            foreach (Ability ability in rot.Priorities)
            {
                solution.SetAbilityUsagePerSecond(
                    ability,
                    abilities[(int)ability].Uses / fightLengthInSeconds);
                solution.SetAbilityEffectiveCooldown(
                    ability,
                    abilities[(int)ability].EffectiveCooldown() / timeUnitsPerSecond);
            }

            return solution;
        }

    }
}