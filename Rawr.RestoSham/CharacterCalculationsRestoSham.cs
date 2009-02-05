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
        public float BurstMPS { get; set; }
        public float TotalHPS { get; set; }
        public float TillOOM { get; set; }
        public float FightHPS { get; set; }
        public float ESLHWHPSMT { get; set; }
        public float ESHWHPSMT { get; set; }
        public float ESCHHPSMT { get; set; }
        public float ESRTCHCHHPSMT { get; set; }
        public float ESLHWMPSMT { get; set; }
        public float ESHWMPSMT { get; set; }
        public float ESCHMPSMT { get; set; }
        public float ESRTCHCHMPSMT { get; set; }
        public float BurstHPS { get; set; }
        public float RTLWH2CHRTHPSMT { get; set; }
        public float RTWH2CHRTHPSMT { get; set; }
        public float RTLWH4HPSMT { get; set; }
        public float RTWH3HPSMT { get; set; }
        public float RTLWH2CHRTMPSMT { get; set; }
        public float RTWH2CHRTMPSMT { get; set; }
        public float RTLWH4MPSMT { get; set; }
        public float RTWH3MPSMT { get; set; }


        public override Dictionary<string, string> GetCharacterDisplayCalculationValues()
        {
            Dictionary<string, string> values = new Dictionary<string, string>();

            values.Add("Health", Math.Round(BasicStats.Health, 0).ToString());
            values.Add("Stamina", Math.Round(BasicStats.Stamina, 0).ToString());
            values.Add("Intellect", Math.Round(BasicStats.Intellect, 0).ToString());
            // values.Add("Spirit", BasicStats.Spirit.ToString());
            values.Add("Spell Power", Math.Round(BasicStats.SpellPower, 0).ToString());
            values.Add("Mana", Math.Round(BasicStats.Mana, 0).ToString());
            values.Add("MP5", BasicStats.Mp5.ToString());
            // values.Add("MP5 (outside FSR)", Math.Round(Mp5OutsideFSR, 0).ToString());
            values.Add("Heal Spell Crit", string.Format("{0}%*{1} spell crit rating",
                       Math.Round(SpellCrit * 100, 2), BasicStats.CritRating.ToString()));
            values.Add("Spell Haste", string.Format("{0}%*{1} spell haste rating",
                       Math.Round(BasicStats.HasteRating / 32.79, 2), BasicStats.HasteRating.ToString()));
            values.Add("Total HPS", Math.Round(TotalHPS, 0).ToString());
            values.Add("Time to OOM", Math.Round(TillOOM, 0).ToString());
            values.Add("Total Healed", Math.Round(TotalHealed, 0).ToString());
            values.Add("Fight HPS", Math.Round(FightHPS, 0).ToString());
            values.Add("ES + LHW HPS", Math.Round(ESLHWHPSMT, 0).ToString());
            values.Add("ES + LHW OOM", Math.Round(ESLHWMPSMT, 0).ToString());
            values.Add("ES + HW HPS", Math.Round(ESHWHPSMT, 0).ToString());
            values.Add("ES + HW OOM", Math.Round(ESHWMPSMT, 0).ToString());
            values.Add("ES + CH HPS", Math.Round(ESCHHPSMT, 0).ToString());
            values.Add("ES + CH OOM", Math.Round(ESCHMPSMT, 0).ToString());
            values.Add("ES + RT + CHx2 HPS", Math.Round(ESRTCHCHHPSMT, 0).ToString());
            values.Add("ES + RT + CHx2 OOM", Math.Round(ESRTCHCHMPSMT, 0).ToString());
            values.Add("Burst HPS", Math.Round(BurstHPS, 0).ToString());
            values.Add("RT + LHWx2 + CH HPS", Math.Round(RTLWH2CHRTHPSMT, 0).ToString());
            values.Add("RT + LHWx2 + CH OOM", Math.Round(RTLWH2CHRTMPSMT, 0).ToString());
            values.Add("RT + HWx2 + CH HPS", Math.Round(RTWH2CHRTHPSMT, 0).ToString());
            values.Add("RT + HWx2 + CH OOM", Math.Round(RTWH2CHRTMPSMT, 0).ToString());
            values.Add("RT + LHWx4 HPS", Math.Round(RTLWH4HPSMT, 0).ToString());
            values.Add("RT + LHWx4 OOM", Math.Round(RTLWH4MPSMT, 0).ToString());
            values.Add("RT + LHWx3 HPS", Math.Round(RTWH3HPSMT, 0).ToString());
            values.Add("RT + LHWx3 OOM", Math.Round(RTWH3MPSMT, 0).ToString());

            return values;
        }
    }
}
