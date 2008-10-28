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
        public float HLCost { get; set; }
        public float HLCastTime { get; set; }

        public float FoLTime { get; set; }
        public float FoLHeal { get; set; }
        public float FoLHPS { get; set; }
        public float FoLCrit { get; set; }
        public float FoLCost { get; set; }
        public float FoLCastTime { get; set; }

        public float ManaBase { get; set; }
        public float ManaMp5 { get; set; }
        public float ManaPotion { get; set; }
        public float ManaReplenishment { get; set; }
        public float ManaArcaneTorrent { get; set; }
        public float ManaDivinePlea { get; set; }
        public float ManaSpiritual { get; set; }
        public float ManaOther { get; set; }

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
            dictValues.Add("Spell Crit", string.Format("{0}%*{1} Crit Rating", Math.Round(BasicStats.SpellCrit * 100, 2), BasicStats.CritRating));
            dictValues.Add("Spell Haste", string.Format("{0}%*{1} Haste Rating", Math.Round(BasicStats.SpellHaste * 100, 2), BasicStats.HasteRating));
            dictValues.Add("Total Healed", Math.Round(TotalHealed).ToString());
            dictValues.Add("Total Mana", Math.Round(TotalMana).ToString());
            dictValues.Add("Average Hps", Math.Round(AvgHPS).ToString());
            dictValues.Add("Average Hpm", Math.Round(AvgHPM, 2).ToString());
            dictValues.Add("Holy Light Time", Math.Round(HLTime * 100).ToString() + "%");
            dictValues.Add("Flash of Light", string.Format("{0} hps*{1}s cast time\n{2}% crit chance\n{3} average heal\n{4} average cost", 
                Math.Round(FoLHPS), Math.Round(FoLCastTime, 2), Math.Round(FoLCrit*100, 2), Math.Round(FoLHeal), Math.Round(FoLCost)));
            dictValues.Add("Holy Light", string.Format("{0} hps*{1}s cast time\n{2}% crit chance\n{3} average heal\n{4} average cost",
                Math.Round(HLHPS), Math.Round(HLCastTime, 2), Math.Round(HLCrit * 100, 2), Math.Round(HLHeal), Math.Round(HLCost)));
            dictValues.Add("Holy Shock", "nyi");
            return dictValues;
        }
    }
}
