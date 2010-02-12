using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Retribution
{
    public class RotationParameters : IEquatable<RotationParameters>
    {

        public bool T7_4pc;
        public int ImpJudgements;
        public bool GlyphConsecrate;
        public Ability[] Priorities;
        public float TimeUnder20;
        public decimal Wait;
        public decimal Delay;
        public decimal T10_Speed;
        public decimal SpellGCD;
        public decimal BloodlustSpellGCD;
        public decimal BloodlustT10Speed;
        public float BloodlustUptime;

        public RotationParameters() { }

        public RotationParameters(
            Ability[] Priorities, 
            float TimeUnder20, 
            float Wait, 
            float Delay, 
            bool T7_4pc, 
            int ImpJudgements, 
            bool GlyphConsecrate, 
            float T10_Speed,
            float spellHaste,
            float bloodlustUptime)
        {
            const float bloodlustHaste = 0.3f;

            this.Priorities = Priorities;
            this.T7_4pc = T7_4pc;
            this.GlyphConsecrate = GlyphConsecrate;
            this.TimeUnder20 = TimeUnder20;
            this.Wait = (decimal)Math.Round(Wait, 2);
            this.Delay = (decimal)Math.Round(Delay, 2);
            this.ImpJudgements = ImpJudgements;
            this.T10_Speed = (decimal)Math.Round(T10_Speed, 2);
            BloodlustUptime = bloodlustUptime;
            SpellGCD = GetSpellGCD(spellHaste);
            BloodlustSpellGCD = 
                BloodlustUptime == 0 ? SpellGCD : GetSpellGCD((1 + spellHaste) * (1 + bloodlustHaste) - 1);
            BloodlustT10Speed = BloodlustUptime == 0 ? 
                this.T10_Speed : 
                (decimal)Math.Round(T10_Speed / (1 + bloodlustHaste), 2);
        }

        public override bool Equals(Object obj)
        {
            return Equals(obj as RotationParameters);
        }

        public bool Equals(RotationParameters other)
        {
            if (other == null) 
                return false;

            if (Priorities.Length != other.Priorities.Length) 
                return false;

            for (int priorityIndex = 0; priorityIndex < Priorities.Length; priorityIndex++)
                if (Priorities[priorityIndex] != other.Priorities[priorityIndex])
                    return false;

            return (T7_4pc == other.T7_4pc) &&
                (T10_Speed == other.T10_Speed) &&
                (GlyphConsecrate == other.GlyphConsecrate) &&
                (TimeUnder20 == other.TimeUnder20) &&
                (Delay == other.Delay) &&
                (Wait == other.Wait) &&
                (ImpJudgements == other.ImpJudgements) &&
                (SpellGCD == other.SpellGCD) &&
                (BloodlustSpellGCD == other.BloodlustSpellGCD) &&
                (BloodlustT10Speed == other.BloodlustT10Speed) &&
                (BloodlustUptime == other.BloodlustUptime);
        }

        public override int GetHashCode()
        {
            return Utilities.GetCombinedHashCode(
                T7_4pc,
                ImpJudgements,
                GlyphConsecrate,
                TimeUnder20,
                Wait,
                Delay,
                T10_Speed,
                SpellGCD,
                BloodlustSpellGCD,
                BloodlustT10Speed,
                BloodlustUptime,
                Utilities.GetCombinedHashCode(Priorities));
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

        public static string RotationString(Ability[] rotation)
        {
            string s = "";
            for (int i = 0; i < rotation.Length; i++)
            {
                s += RotationParameters.ShortAbilityString(rotation[i]);
                if (i + 1 < rotation.Length) s += " > ";
            }
            return s;
        }

        public static string ShortRotationString(Ability[] rotation)
        {
            string s = "";
            for (int i = 0; i < rotation.Length; i++)
            {
                s += RotationParameters.ShortAbilityString(rotation[i]);
                if (i + 1 < rotation.Length) s += "-";
            }
            return s;
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

        public static Ability[] DefaultRotation()
        {
            return new Ability[] { Ability.CrusaderStrike, Ability.HammerOfWrath, Ability.Judgement,
                Ability.Consecration, Ability.DivineStorm, Ability.Exorcism };
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


        private static decimal GetSpellGCD(float spellHaste)
        {
            return (decimal)Math.Round(Math.Max(1f, 1.5f / (1 + spellHaste)), 2);
        }

    }

}