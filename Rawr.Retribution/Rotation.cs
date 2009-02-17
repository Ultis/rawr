using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Retribution
{
    public class Rotation
    {

        public class RotationComparer : IEqualityComparer<Rotation>
        {
            private readonly int[] encode = { 1, 2, 3, 4, 5, 6 }; 

            public bool Equals(Rotation one, Rotation two)
            {
                return one.Equals(two);
            }

            public int GetHashCode(Rotation obj)
            {
                int ret = encode[(int)obj.Priorities[0]] + encode[(int)obj.Priorities[1]] * 2 + encode[(int)obj.Priorities[2]] * 4
                    + encode[(int)obj.Priorities[3]] * 8 + encode[(int)obj.Priorities[4]] * 16 + encode[(int)obj.Priorities[5]] * 32
                    + (obj.T7_4pc ? 64 : 0) + (obj.GlyphConsecrate ? 128 : 0) + int.Parse(obj.FightLength.ToString()) * 256
                    + int.Parse((obj.TimeUnder20 * 100).ToString()) * 512;
                
                return ret;
            }

        }

        public enum Ability {Judgement=0, CrusaderStrike, DivineStorm, Consecration, HammerOfWrath, Exorcism };

        public readonly bool T7_4pc;
        public readonly bool GlyphConsecrate;
        public readonly Ability[] Priorities;
        public readonly float FightLength;
        public readonly float TimeUnder20;

        public Rotation(Ability[] Priorities, float FightLength, float TimeUnder20, bool T7_4pc, bool GlyphConsecrate)
        {
            this.Priorities = Priorities;
            this.T7_4pc = T7_4pc;
            this.GlyphConsecrate = GlyphConsecrate;
            this.FightLength = FightLength;
            this.TimeUnder20 = TimeUnder20;
        }

        public bool Equals(Rotation other)
        {
            bool prior = true;
            for (int i = 0; i < Priorities.Length; i++)
            {
                if (i >= other.Priorities.Length || Priorities[i] != other.Priorities[i])
                {
                    prior = false;
                    break;
                }
            }
            return prior
                && (T7_4pc == other.T7_4pc)
                && (GlyphConsecrate == other.GlyphConsecrate)
                && (FightLength == other.FightLength)
                && (TimeUnder20 == other.TimeUnder20);
        }

    }
}
