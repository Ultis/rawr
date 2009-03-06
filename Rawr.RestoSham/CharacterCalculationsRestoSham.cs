using System;
using System.Collections.Generic;

namespace Rawr.RestoSham
{
    class CharacterCalculationsRestoSham : CharacterCalculationsBase
    {
        private float _overallPoints = 0f;
        public override float OverallPoints
        {
            get { return _overallPoints; }
            set { _overallPoints = value; }
        }


        private float[] _subPoints = new float[] { 0f, 0f };
        public override float[] SubPoints
        {
            get { return _subPoints; }
            set { _subPoints = value; }
        }


        private Stats _basicStats = null;
        public Stats BasicStats
        {
            get { return _basicStats; }
            set { _basicStats = value; }
        }


        public float Mp5OutsideFSR { get; set; }
        public float SpellCrit { get; set; }
        public float TotalManaPool { get; set; }
        public float TotalHealed { get; set; }
        public float FightMPS { get; set; }
        public float TotalHPS { get; set; }
        public float TillOOM { get; set; }
        public float FightHPS { get; set; }
        public string ChosenSequence { get; set; }
        public float HWSpamHPS { get; set; }
        public float HWSpamMPS { get; set; }
        public float LHWSpamHPS { get; set; }
        public float LHWSpamMPS { get; set; }
        public float CHSpamHPS { get; set; }
        public float CHSpamMPS { get; set; }
        public float RTHWHPS { get; set; }
        public float RTHWMPS { get; set; }
        public float RTLHWHPS { get; set; }
        public float RTLHWMPS { get; set; }
        public float RTCHHPS { get; set; }
        public float RTCHMPS { get; set; }

        public override Dictionary<string, string> GetCharacterDisplayCalculationValues()
        {
            Dictionary<string, string> values = new Dictionary<string, string>();

            values.Add("Health", Math.Round(BasicStats.Health, 0).ToString());
            values.Add("Stamina", Math.Round(BasicStats.Stamina, 0).ToString());
            values.Add("Intellect", Math.Round(BasicStats.Intellect, 0).ToString());
            values.Add("Spell Power", Math.Round(BasicStats.SpellPower, 0).ToString());
            values.Add("Mana", Math.Round(BasicStats.Mana, 0).ToString());
            values.Add("MP5", BasicStats.Mp5.ToString());
            values.Add("Heal Spell Crit", string.Format("{0}%*{1} spell crit rating",
                       Math.Round(SpellCrit * 100, 2), BasicStats.CritRating.ToString()));
            values.Add("Spell Haste", string.Format("{0}%*{1} spell haste rating",
                       Math.Round(BasicStats.HasteRating / 32.79, 2), BasicStats.HasteRating.ToString()));
            values.Add("Total HPS", Math.Round(TotalHPS, 0).ToString());
            values.Add("Time to OOM", Math.Round(TillOOM, 0).ToString());
            values.Add("Total Healed", Math.Round(TotalHealed, 0).ToString());
            values.Add("Chosen Sequence", ChosenSequence.ToString());
            values.Add("RT+HW Spam HPS", Math.Round(RTHWHPS, 0).ToString());
            values.Add("RT+HW Spam MPS", Math.Round(RTHWMPS, 0).ToString());
            values.Add("RT+LHW Spam HPS", Math.Round(RTLHWHPS, 0).ToString());
            values.Add("RT+LHW Spam MPS", Math.Round(RTLHWMPS, 0).ToString());
            values.Add("RT+CH Spam HPS", Math.Round(RTCHHPS, 0).ToString());
            values.Add("RT+CH Spam MPS", Math.Round(RTCHMPS, 0).ToString());
            values.Add("HW Spam HPS", Math.Round(HWSpamHPS, 0).ToString());
            values.Add("HW Spam MPS", Math.Round(HWSpamMPS, 0).ToString());
            values.Add("LHW Spam HPS", Math.Round(LHWSpamHPS, 0).ToString());
            values.Add("LHW Spam MPS", Math.Round(LHWSpamMPS, 0).ToString());
            values.Add("CH Spam HPS", Math.Round(CHSpamHPS, 0).ToString());
            values.Add("CH Spam MPS", Math.Round(CHSpamMPS, 0).ToString());

            return values;
        }
        public override float GetOptimizableCalculationValue(string calculation)
        {
            switch (calculation)
            {
                case "Total HPS":
                    return FightHPS;
                case "Time to OOM": 
                    return TillOOM;
                case "Health":
                    return BasicStats.Health;
            }
            return 0f;
        }
    }
}
