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

            int judgeCD = rot.T7_4pc ? 7 : 8;
            int csCD = 6;
            int dsCD = 10;
            int consCD = rot.GlyphConsecrate ? 10 : 8;
            int exoCD = 15;
            int howCD = 6;

            float judgeNext = 0;
            float csNext = 0;
            float dsNext = 0;
            float consNext = 0;
            float exoNext = 0;
            float howNext = rot.FightLength * (1f - rot.TimeUnder20);
            bool gcdUsed = false;

            while (currentTime < rot.FightLength)
            {
                foreach (Rotation.Ability ability in rot.Priorities)
                {
                    if (ability == Rotation.Ability.Judgement)
                    {
                        if (judgeNext <= currentTime)
                        {
                            gcdUsed = true;
                            judgeNext = currentTime + judgeCD;
                            sol.Judgement++;
                            break;
                        }
                    }
                    else if (ability == Rotation.Ability.CrusaderStrike)
                    {
                        if (csNext <= currentTime)
                        {
                            gcdUsed = true;
                            csNext = currentTime + csCD;
                            sol.CrusaderStrike++;
                            break;
                        }
                    }
                    else if (ability == Rotation.Ability.DivineStorm)
                    {
                        if (dsNext <= currentTime)
                        {
                            gcdUsed = true;
                            dsNext = currentTime + dsCD;
                            sol.DivineStorm++;
                            break;
                        }
                    }
                    else if (ability == Rotation.Ability.HammerOfWrath)
                    {
                        if (howNext <= currentTime)
                        {
                            gcdUsed = true;
                            howNext = currentTime + howCD;
                            sol.HammerOfWrath++;
                            break;
                        }
                    }
                    else if (ability == Rotation.Ability.Consecration)
                    {
                        if (consNext <= currentTime)
                        {
                            gcdUsed = true;
                            consNext = currentTime + consCD;
                            sol.Consecration++;
                            break;
                        }
                    }
                    else if (ability == Rotation.Ability.Exorcism)
                    {
                        if (exoNext <= currentTime)
                        {
                            gcdUsed = true;
                            exoNext = currentTime + exoCD;
                            sol.Exorcism++;
                            break;
                        }
                    }
                }
                currentTime += gcdUsed ? 1.5f : .5f;
                gcdUsed = false;
            }

            sol.JudgementCD = rot.FightLength / sol.Judgement;
            sol.CrusaderStrikeCD = rot.FightLength / sol.CrusaderStrike;
            sol.DivineStormCD = rot.FightLength / sol.DivineStorm;
            sol.ConsecrationCD = rot.FightLength / sol.Consecration;
            sol.ExorcismCD = rot.FightLength / sol.Exorcism;
            sol.HammerOfWrathCD = rot.FightLength / sol.HammerOfWrath;

            savedSolutions[rot] = sol;

            return sol;
        }
    }
}
