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
                int ret = (obj.T7_4pc ? 512 : 0) + (obj.GlyphConsecrate ? 1024 : 0) + int.Parse((obj.TimeUnder20 * 100).ToString()) * 2048
                     + int.Parse((obj.SpellGCD * 100).ToString()) * 4096 + int.Parse((obj.Delay * 100).ToString()) * 8192;

                for (int i = 0; i < obj.Priorities.Length; i++)
                {
                    ret += (int)obj.Priorities[i] * (4 ^ i);
                }
                
                return ret;
            }

        }

        public enum Ability { Judgement=0, CrusaderStrike, DivineStorm, Consecration, HammerOfWrath, Exorcism };

        public readonly bool T7_4pc;
        public readonly bool GlyphConsecrate;
        public readonly Ability[] Priorities;
        public readonly float TimeUnder20;
        public readonly float SpellGCD;
        public readonly float Delay;

        public Rotation(Ability[] Priorities, float TimeUnder20, float SpellGCD, float Delay, bool T7_4pc, bool GlyphConsecrate)
        {
            this.Priorities = Priorities;
            this.T7_4pc = T7_4pc;
            this.GlyphConsecrate = GlyphConsecrate;
            this.TimeUnder20 = TimeUnder20;
            this.SpellGCD = (float)Math.Round(SpellGCD, 2);
            this.Delay = (float)Math.Round(Delay, 2);
        }

        public bool Equals(Rotation other)
        {
            if (Priorities.Length != other.Priorities.Length) return false;
            for (int i = 0; i < Priorities.Length; i++)
            {
                if (Priorities[i] != other.Priorities[i])
                {
                    return false;
                }
            }
            return (T7_4pc == other.T7_4pc)
                && (GlyphConsecrate == other.GlyphConsecrate)
                && (TimeUnder20 == other.TimeUnder20)
                && (Delay == other.Delay)
                && (SpellGCD == other.SpellGCD);
        }

        public static string ShortAbilityString(Ability ability)
        {
            if (ability == Ability.Consecration) return "Con";
            if (ability == Ability.CrusaderStrike) return "CS";
            if (ability == Ability.Judgement) return "Jud";
            if (ability == Ability.DivineStorm) return "DS";
            if (ability == Ability.Exorcism) return "Exo";
            if (ability == Ability.HammerOfWrath) return "HoW";
            return "?";
        }

        public static string AbilityString(Ability ability)
        {
            if (ability == Ability.Consecration) return "Consecration";
            if (ability == Ability.CrusaderStrike) return "Crusader Strike";
            if (ability == Ability.Judgement) return "Judgement";
            if (ability == Ability.DivineStorm) return "Divine Storm";
            if (ability == Ability.Exorcism) return "Exorcism";
            if (ability == Ability.HammerOfWrath) return "Hammer of Wrath";
            return "?";
        }

        public override string ToString()
        {
            string ret = "";
            foreach (Ability a in Priorities)
            {
                ret += ShortAbilityString(a) + ", ";
            }
            return ret;
        }

    }
}
