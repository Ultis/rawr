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
        public float FightHPS { get; set; }
        public float ESLHWHPSMT { get; set; }
        public float ESHWHPSMT { get; set; }
        public float ESCHHPSMT { get; set; }
        public float ESRTCHCHHPSMT { get; set; }
        public float ESLHWMPSMT { get; set; }
        public float ESHWMPSMT { get; set; }
        public float ESCHMPSMT { get; set; }
        public float ESRTCHCHMPSMT { get; set; }


        public override Dictionary<string, string> GetCharacterDisplayCalculationValues()
        {
            Dictionary<string, string> values = new Dictionary<string, string>();

            values.Add("Health", Math.Round(BasicStats.Health, 0).ToString());
            values.Add("Stamina", Math.Round(BasicStats.Stamina, 0).ToString());
            values.Add("Intellect", Math.Round(BasicStats.Intellect, 0).ToString());
            values.Add("Spirit", BasicStats.Spirit.ToString());
            values.Add("Spell Power", Math.Round(BasicStats.SpellPower, 0).ToString());
            values.Add("Mana", Math.Round(BasicStats.Mana, 0).ToString());
            values.Add("MP5 (in FSR)", BasicStats.Mp5.ToString());
            values.Add("MP5 (outside FSR)", Math.Round(Mp5OutsideFSR, 0).ToString());
            values.Add("Heal Spell Crit", string.Format("{0}%*{1} spell crit rating",
                       Math.Round(SpellCrit * 100, 2), BasicStats.CritRating.ToString()));
            values.Add("Spell Haste", string.Format("{0}%*{1} spell haste rating",
                       Math.Round(BasicStats.HasteRating / 15.7, 2), BasicStats.HasteRating.ToString()));
            values.Add("Fight HPS", Math.Round(FightHPS, 0).ToString());
            values.Add("ES + LHW HPS", Math.Round(ESLHWHPSMT, 0).ToString());
            values.Add("ES + LHW OOM", Math.Round(ESLHWMPSMT, 0).ToString());
            values.Add("ES + HW HPS", Math.Round(ESHWHPSMT, 0).ToString());
            values.Add("ES + HW OOM", Math.Round(ESHWMPSMT, 0).ToString());
            values.Add("ES + CH HPS", Math.Round(ESCHHPSMT, 0).ToString());
            values.Add("ES + CH OOM", Math.Round(ESCHMPSMT, 0).ToString());
            values.Add("ES + RT + CHx2 HPS", Math.Round(ESRTCHCHHPSMT, 0).ToString());
            values.Add("ES + RT + CHx2 OOM", Math.Round(ESRTCHCHMPSMT, 0).ToString());

            return values;
        }
    }
}
