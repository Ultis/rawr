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
            const int fightLength = 2000000 * timeUnitsPerSecond;
            const int meleeAbilityGcd = (int)(1.5m * timeUnitsPerSecond);

            int bloodlustSpellGcd = (int)(rot.BloodlustSpellGCD * timeUnitsPerSecond);
            int spellGcd = (int)(rot.SpellGCD * timeUnitsPerSecond);
            int bloodlustT10Speed = (int)(rot.BloodlustT10Speed * timeUnitsPerSecond);
            int t10Speed = (int)(rot.T10_Speed * timeUnitsPerSecond);
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
                bloodlustSpellGcd);
            abilities[(int)Ability.Exorcism] = new SimulatorAbility(
                15 * timeUnitsPerSecond,
                bloodlustSpellGcd);
            abilities[(int)Ability.HammerOfWrath] = new SimulatorAbility(
                6 * timeUnitsPerSecond,
                meleeAbilityGcd);

            abilities[(int)Ability.HammerOfWrath].NextUse =
                (int)Math.Round((double)fightLength * (1d - rot.TimeUnder20));

            int gcdFinishTime = 0;
            Random rand = new Random(6021987);
            bool isBloodlustActive = true;
            int bloodlustFinishTime = (int)Math.Round((double)fightLength * rot.BloodlustUptime);
            int nextSwingTime = bloodlustFinishTime > 0 ? bloodlustT10Speed : t10Speed;

            int currentTime = 0;
            while (currentTime < fightLength)
            {
                if (isBloodlustActive && (currentTime >= bloodlustFinishTime))
                {
                    isBloodlustActive = false;
                    abilities[(int)Ability.Consecration].GlobalCooldown = spellGcd;
                    abilities[(int)Ability.Exorcism].GlobalCooldown = spellGcd;
                }

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
                    foreach (SimulatorAbility ability in abilities)
                    {
                        int nextUseTime = ability.GetNextUseTime(currentTime);
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
                    if (isBloodlustActive)
                        nextTime = Math.Min(nextTime, bloodlustFinishTime);

                    while (nextTime > nextSwingTime)
                    {
                        if (rand.NextDouble() < t10ProcChance)
                        {
                            abilities[(int)Ability.DivineStorm].ResetCooldown(nextSwingTime);
                            nextTime = nextSwingTime;
                        }

                        nextSwingTime += isBloodlustActive ? bloodlustT10Speed : t10Speed;
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