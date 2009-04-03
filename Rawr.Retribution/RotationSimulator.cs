using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Retribution
{
    static class RotationSimulator
    {
        private static Dictionary<Rotation, RotationSolution> savedSolutions = new Dictionary<Rotation, RotationSolution>(new Rotation.RotationComparer());

        public static RotationSolution SimulateRotation(Rotation rot)
        {
            if (savedSolutions.ContainsKey(rot)) return savedSolutions[rot];

            RotationSolution sol = new RotationSolution();
            float currentTime = 0;
            sol.FightLength = 10000;
            SimulatorAbility.Delay = rot.Delay;
            SimulatorAbility.Wait = rot.Wait;
            
            SimulatorAbility[] abilities = new SimulatorAbility[6];

            abilities[(int)Rotation.Ability.Judgement] = new SimulatorAbility(rot.T7_4pc ? 7 : 8);
            abilities[(int)Rotation.Ability.CrusaderStrike] = new SimulatorAbility(6);
            abilities[(int)Rotation.Ability.DivineStorm] = new SimulatorAbility(10);
            abilities[(int)Rotation.Ability.Consecration] = new SimulatorAbility(rot.GlyphConsecrate ? 10 : 8);
            abilities[(int)Rotation.Ability.Exorcism] = new SimulatorAbility(15);
            abilities[(int)Rotation.Ability.HammerOfWrath] = new SimulatorAbility(6);

            abilities[(int)Rotation.Ability.HammerOfWrath].NextUse = sol.FightLength * (1f - rot.TimeUnder20);

            bool gcdUsed;
            while (currentTime < sol.FightLength)
            {
                gcdUsed = false;
                foreach (Rotation.Ability ability in rot.Priorities)
                {
                    if (abilities[(int)ability].UseAbility(currentTime))
                    {
                        currentTime += 1.5f + rot.Delay;
                        gcdUsed = true;
                        break;
                    }
                }
                if (!gcdUsed) currentTime += .01f;
            }

            sol.Judgement = abilities[(int)Rotation.Ability.Judgement].Uses;
            sol.JudgementCD = abilities[(int)Rotation.Ability.Judgement].EffectiveCooldown();

            sol.CrusaderStrike = abilities[(int)Rotation.Ability.CrusaderStrike].Uses;
            sol.CrusaderStrikeCD = abilities[(int)Rotation.Ability.CrusaderStrike].EffectiveCooldown();

            sol.DivineStorm = abilities[(int)Rotation.Ability.DivineStorm].Uses;
            sol.DivineStormCD = abilities[(int)Rotation.Ability.DivineStorm].EffectiveCooldown();

            sol.Consecration = abilities[(int)Rotation.Ability.Consecration].Uses;
            sol.ConsecrationCD = abilities[(int)Rotation.Ability.Consecration].EffectiveCooldown();

            sol.Exorcism = abilities[(int)Rotation.Ability.Exorcism].Uses;
            sol.ExorcismCD = abilities[(int)Rotation.Ability.Exorcism].EffectiveCooldown();

            sol.HammerOfWrath = abilities[(int)Rotation.Ability.HammerOfWrath].Uses;
            sol.HammerOfWrathCD = abilities[(int)Rotation.Ability.HammerOfWrath].EffectiveCooldown();

            savedSolutions[rot] = sol;

            return sol;
        }
    }
}
