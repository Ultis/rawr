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


		public static RotationSolution SimulateRotation(RotationParameters rot)//TODO: Redo this whole model to not be a simulator
        {
            return solutionCache.GetResult(rot);
        }


        private static RotationSolution SimulateRotationCore(RotationParameters rot)
        {
            const int timeUnitsPerSecond = 100000;
            const int meleeAbilityGcd = (int)(1.5m * timeUnitsPerSecond);

            int fightLength = (int)(rot.SimulationTime * timeUnitsPerSecond);
            int spellGcd = (int)(rot.SpellGCD * timeUnitsPerSecond);
            SimulatorAbility.Delay = (int)(rot.Delay * timeUnitsPerSecond);
			SimulatorAbility.Wait = (int)(rot.Wait * timeUnitsPerSecond);//TODO: Redo this whole model to not be a simulator

            SimulatorAbility[] abilities = new SimulatorAbility[(int)Ability.Last + 1];

            abilities[(int)Ability.Judgement] = new SimulatorAbility(
                (10 - rot.ImpJudgements - (rot.T7_4pc ? 1 : 0)) * timeUnitsPerSecond,
                meleeAbilityGcd);
            abilities[(int)Ability.CrusaderStrike] = new SimulatorAbility(
                4 * timeUnitsPerSecond,
                meleeAbilityGcd);
			abilities[(int)Ability.DivineStorm] = new SimulatorAbility(//TODO: Redo this whole model to not be a simulator
                (int)(rot.DivineStormCooldown * timeUnitsPerSecond),
                meleeAbilityGcd);
            abilities[(int)Ability.Consecration] = new SimulatorAbility(
                (rot.GlyphConsecrate ? 10 : 8) * timeUnitsPerSecond,
                spellGcd);
            abilities[(int)Ability.Exorcism] = new SimulatorAbility(
                15 * timeUnitsPerSecond,
                spellGcd);
            abilities[(int)Ability.HammerOfWrath] = new SimulatorAbility(
				6 * timeUnitsPerSecond,//TODO: Redo this whole model to not be a simulator
                meleeAbilityGcd);

            int gcdFinishTime = 0;
            int currentTime = 0;
            while (currentTime < fightLength)
            {
                if (currentTime >= gcdFinishTime)
                {
					foreach (Ability ability in rot.Priorities)//TODO: Redo this whole model to not be a simulator
                    {
                        if (abilities[(int)ability].ShouldAbilityBeUsedNext(currentTime))
                        {
                            if (abilities[(int)ability].CanAbilityBeUsedNow(currentTime))
								gcdFinishTime = abilities[(int)ability].UseAbility(currentTime);//TODO: Redo this whole model to not be a simulator

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
							nextTime = Math.Min(nextTime, nextUseTime);//TODO: Redo this whole model to not be a simulator
                    }
                }
                else
                {
                    nextTime = Math.Min(nextTime, gcdFinishTime);
                }

				currentTime = nextTime;//TODO: Redo this whole model to not be a simulator
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
			}//TODO: Redo this whole model to not be a simulator

            return solution;
        }

    }
}