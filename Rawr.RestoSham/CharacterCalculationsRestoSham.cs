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


        private float[] _subPoints = new float[] { 0f, 0f, 0f };
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
        public float SpellHaste { get; set; }
        public float TotalManaPool { get; set; }
        public float FightHPS { get; set; }
        public string SustainedSequence { get; set; }
        public string BurstSequence { get; set; }
        public float MAPS { get; set; }
        public float ManaUsed { get; set; }
        public float HSTHeals { get; set; }
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
        public float RTCHLHW2HPS { get; set; }
        public float RTCHLHW2MPS { get; set; }
        public float RTCHHPS { get; set; }
        public float RTCHMPS { get; set; }
        public float BurstHPS { get; set; }
        public float SustainedHPS { get; set; }
        public float MUPS { get; set; }
        public float Survival { get; set; }
        public float HealPerSec { get; set; }
        public float CritPerSec { get; set; }

        public override Dictionary<string, string> GetCharacterDisplayCalculationValues()
        {
            Dictionary<string, string> values = new Dictionary<string, string>();
            
            values.Add("HPS - Burst", Math.Round(BurstHPS, 0).ToString());
            values.Add("HPS - Sustained", Math.Round(SustainedHPS, 0).ToString());
            values.Add("Survival", Math.Round(Survival, 0).ToString());
            values.Add("Health", Math.Round(BasicStats.Health, 0).ToString());
            values.Add("Stamina", Math.Round(BasicStats.Stamina, 0).ToString());
            values.Add("Intellect", Math.Round(BasicStats.Intellect, 0).ToString());
            values.Add("Spell Power", Math.Round(BasicStats.SpellPower, 0).ToString());
            values.Add("Mana", Math.Round(BasicStats.Mana, 0).ToString());
            values.Add("MP5", BasicStats.Mp5.ToString());
            values.Add("Mana Available", Math.Round(ManaUsed, 0).ToString());
            values.Add("Heal Spell Crit", string.Format("{0}%*{1} spell crit rating",
                       Math.Round(SpellCrit * 100, 2), BasicStats.CritRating.ToString()));
            values.Add("Spell Haste", string.Format("{0}%*{1} spell haste rating",
                       Math.Round(SpellHaste * 100, 2), BasicStats.HasteRating.ToString()));
            values.Add("Burst Sequence", BurstSequence.ToString());
            values.Add("Sustained Sequence", SustainedSequence.ToString());
            values.Add("Mana Available per Second", Math.Round(MAPS, 0).ToString());
            values.Add("Mana Used per Second", Math.Round(MUPS, 0).ToString());
            values.Add("Healing Stream HPS", Math.Round(HSTHeals, 0).ToString());
            values.Add("RT+HW HPS", Math.Round(RTHWHPS, 0).ToString());
            values.Add("RT+LHW HPS", Math.Round(RTLHWHPS, 0).ToString());
            values.Add("RT+CH HPS", Math.Round(RTCHHPS, 0).ToString());
            values.Add("RT+CH+LHW HPS", Math.Round(RTCHLHW2HPS, 0).ToString());
            values.Add("HW Spam HPS", Math.Round(HWSpamHPS, 0).ToString());
            values.Add("LHW Spam HPS", Math.Round(LHWSpamHPS, 0).ToString());
            values.Add("CH Spam HPS", Math.Round(CHSpamHPS, 0).ToString());

            return values;
        }
        public override float GetOptimizableCalculationValue(string calculation)
        {
            switch (calculation)
            {
                case "Mana Usable per Second":
                    return MAPS;
                case "Health":
                    return BasicStats.Health;
                case "Haste %":
                    return (float)Math.Round(SpellHaste * 100, 2);
                case "Crit %":
                    return (float)Math.Round(SpellCrit * 100, 2);
            }
            return 0f;
        }
    }
}
