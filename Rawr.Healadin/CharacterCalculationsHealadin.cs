using System;
using System.Collections.Generic;

namespace Rawr.Healadin
{

    public class CharacterCalculationsHealadin : CharacterCalculationsBase
    {

        private float _overallPoints = 0f;
        public override float OverallPoints
        {
            get { return _overallPoints; }
            set { _overallPoints = value; }
        }

        private float[] _subPoints = new float[] { 0f }; //, 0f };
        public override float[] SubPoints
        {
            get { return _subPoints; }
            set { _subPoints = value; }
        }

        public float ThroughputPoints
        {
            get { return _subPoints[0]; }
            set { _subPoints[0] = value; }
        }

        public float AvgHPS { get; set; }
        public float AvgHPM { get; set; }
        public float TotalHealed { get; set; }
        public float TotalMana { get; set; }

        public float HLTime { get; set; }
        public float HLHeal { get; set; }
        public float HLHPS { get; set; }
        public float HLCrit { get; set; }
        public float HLCastTime { get; set; }

        public float FoLTime { get; set; }
        public float FoLHeal { get; set; }
        public float FoLHPS { get; set; }
        public float FoLCrit { get; set; }
        public float FoLCastTime { get; set; }

        private Stats _basicStats;
        public Stats BasicStats
        {
            get { return _basicStats; }
            set { _basicStats = value; }
        }


        public override Dictionary<string, string> GetCharacterDisplayCalculationValues()
        {
            Dictionary<string, string> dictValues = new Dictionary<string, string>();

            dictValues.Add("Health", BasicStats.Health.ToString());
            dictValues.Add("Stamina", BasicStats.Stamina.ToString());
            dictValues.Add("Mana", BasicStats.Mana.ToString());
            dictValues.Add("Intellect", BasicStats.Intellect.ToString());
            dictValues.Add("Spell Power", BasicStats.SpellPower.ToString());
            dictValues.Add("Mp5", BasicStats.Mp5.ToString());
            dictValues.Add("Spell Crit", string.Format("{0}%", Math.Round(BasicStats.SpellCrit * 100, 2)));
            dictValues.Add("Spell Haste", string.Format("{0}%", Math.Round(BasicStats.SpellHaste * 100, 2)));
            dictValues.Add("Total Healed", Math.Round(TotalHealed).ToString());
            dictValues.Add("Total Mana", Math.Round(TotalMana).ToString());
            dictValues.Add("Average Hps", Math.Round(AvgHPS).ToString());
            dictValues.Add("Average Hpm", Math.Round(AvgHPM, 2).ToString());
            dictValues.Add("Holy Light Time", Math.Round(HLTime * 100).ToString() + "%");
            dictValues.Add("Flash of Light", string.Format("{0} hps", Math.Round(FoLHPS)));
            dictValues.Add("Holy Light", string.Format("{0} hps", Math.Round(HLHPS)));
            dictValues.Add("Holy Shock", "nyi");
            return dictValues;
        }
    }
}
