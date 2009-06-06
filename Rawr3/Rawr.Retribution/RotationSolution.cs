using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Retribution
{
    public class RotationSolution
    {
        public int Judgement { get; set;}
        public int CrusaderStrike { get; set; }
        public int DivineStorm { get; set; }
        public int Consecration { get; set; }
        public int Exorcism { get; set; }
        public int HammerOfWrath { get; set; }

        public float FightLength { get; set; }

        public float JudgementCD { get; set; }
        public float CrusaderStrikeCD { get; set; }
        public float DivineStormCD { get; set; }
        public float ConsecrationCD { get; set; }
        public float ExorcismCD { get; set; }
        public float HammerOfWrathCD { get; set; }
    }
}
