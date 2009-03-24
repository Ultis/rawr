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
            sol.FightLength = 1000;

            int judgeCD = rot.T7_4pc ? 7 : 8;
            float judgeFirst = -1;
            float judgeLast = -1;

            int csCD = 6;
            float csFirst = -1;
            float csLast = -1;

            int dsCD = 10;
            float dsFirst = -1;
            float dsLast = -1;

            int consCD = rot.GlyphConsecrate ? 10 : 8;
            float consFirst = -1;
            float consLast = -1;

            int exoCD = 15;
            float exoFirst = -1;
            float exoLast = -1;

            int howCD = 6;
            float howFirst = -1;
            float howLast = -1;

            float judgeNext = 0;
            float csNext = 0;
            float dsNext = 0;
            float consNext = 0;
            float exoNext = 0;
            float howNext = sol.FightLength * (1f - rot.TimeUnder20);
            bool gcdUsed = false;

            while (currentTime < sol.FightLength)
            {
                foreach (Rotation.Ability ability in rot.Priorities)
                {
                    if (ability == Rotation.Ability.Judgement)
                    {
                        if (judgeNext <= currentTime)
                        {
                            if (judgeFirst < 0) judgeFirst = currentTime;
                            judgeLast = currentTime;
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
                            if (csFirst < 0) csFirst = currentTime;
                            csLast = currentTime;
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
                            if (dsFirst < 0) dsFirst = currentTime;
                            dsLast = currentTime;
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
                            if (howFirst < 0) howFirst = currentTime;
                            howLast = currentTime;
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
                            if (consFirst < 0) consFirst = currentTime;
                            consLast = currentTime;
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
                            if (exoFirst < 0) exoFirst = currentTime;
                            exoLast = currentTime;
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

            sol.JudgementCD = (judgeLast - judgeFirst) / (sol.Judgement - 1);
            sol.CrusaderStrikeCD = (csLast - csFirst) / (sol.CrusaderStrike - 1);
            sol.DivineStormCD = (dsLast - dsFirst) / (sol.DivineStorm - 1);
            sol.ConsecrationCD = (consLast - consFirst) / (sol.Consecration - 1);
            sol.ExorcismCD = (exoLast - exoFirst) / (sol.Exorcism - 1);
            sol.HammerOfWrathCD = (howLast - howFirst) / (sol.HammerOfWrath - 1);

            savedSolutions[rot] = sol;

            return sol;
        }
    }
}
