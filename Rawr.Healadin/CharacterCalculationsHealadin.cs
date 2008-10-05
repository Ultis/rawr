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

  /*      public float LongevityPoints
        {
            get { return _subPoints[1]; }
            set { _subPoints[1] = value; }
        }*/


        public float AvgHPS { get; set; }
        public float AvgHPM { get; set; }
        public float Healed { get; set; }
        public float TimeHL { get; set; }
        public float HealHL { get; set; }
        public float HLHPS { get; set; }
        public float FoLHPS { get; set; }

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
            dictValues.Add("Spirit", BasicStats.Spirit.ToString());
            dictValues.Add("Healing", BasicStats.Healing.ToString());
            dictValues.Add("Mp5", BasicStats.Mp5.ToString());
            dictValues.Add("Spell Crit", "nyi");
            dictValues.Add("Spell Haste", "nyi");
            dictValues.Add("Total Healed", Math.Round(Healed).ToString());
            dictValues.Add("Average Hps", Math.Round(AvgHPS).ToString());
            dictValues.Add("Average Hpm", Math.Round(AvgHPM, 2).ToString());
            dictValues.Add("Holy Light Time", Math.Round(TimeHL * 100).ToString() + "%");
            dictValues.Add("Holy Light Healing", Math.Round(HealHL * 100).ToString() + "%");
            dictValues.Add("Flash of Light", "nyi");
            dictValues.Add("Holy Light", "nyi");
            dictValues.Add("Holy Shock", "nyi");
            return dictValues;
        }
    }
}
