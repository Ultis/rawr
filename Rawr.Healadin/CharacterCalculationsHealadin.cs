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
        public float HLAvgHeal { get; set; }
        public float HLHealed { get; set; }
        public float HLHPS { get; set; }
        public float HLHPM { get; set; }
        public float HLCrit { get; set; }
        public float HLCost { get; set; }
        public float HLCastTime { get; set; }
        public float HLUsage { get; set; }

        public float FoLTime { get; set; }
        public float FoLAvgHeal { get; set; }
        public float FoLHealed { get; set; }
        public float FoLHPS { get; set; }
        public float FoLHPM { get; set; }
        public float FoLCrit { get; set; }
        public float FoLCost { get; set; }
        public float FoLCastTime { get; set; }
        public float FoLUsage { get; set; }

        public float HSTime { get; set; }
        public float HSAvgHeal { get; set; }
        public float HSHealed { get; set; }
        public float HSHPS { get; set; }
        public float HSHPM { get; set; }
        public float HSCrit { get; set; }
        public float HSCost { get; set; }
        public float HSCastTime { get; set; }
        public float HSUsage { get; set; }

        public float BoLUsage { get; set; }
        public float BoLCasts { get; set; }
        public float BoLHealed { get; set; }

        public float JotPUsage { get; set; }
        public float JotPCasts { get; set; }
        public float JotPHaste { get; set; }

        public float ManaBase { get; set; }
        public float ManaMp5 { get; set; }
        public float ManaPotion { get; set; }
        public float ManaReplenishment { get; set; }
        public float ManaArcaneTorrent { get; set; }
        public float ManaDivinePlea { get; set; }
        public float ManaLayOnHands { get; set; }
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
            //Basic Stats
            dictValues.Add("Health", BasicStats.Health.ToString("N00"));
            dictValues.Add("Stamina", BasicStats.Stamina.ToString("N00"));
            dictValues.Add("Mana", BasicStats.Mana.ToString("N00"));
            dictValues.Add("Intellect", BasicStats.Intellect.ToString("N00"));
            dictValues.Add("Spell Power", BasicStats.SpellPower.ToString("N00"));
            dictValues.Add("Mp5", BasicStats.Mp5.ToString("N00"));
            dictValues.Add("Spell Crit", string.Format("{0}%*{1} Crit Rating", (BasicStats.SpellCrit * 100).ToString("N02"), BasicStats.CritRating));
            dictValues.Add("Spell Haste", string.Format("{0}%*{1} Haste Rating", (BasicStats.SpellHaste * 100).ToString("N02"), BasicStats.HasteRating));
            // Cycle Stats
            dictValues.Add("Total Healed", string.Format("{0} healing", TotalHealed.ToString("N00")));
            dictValues.Add("Total Mana", string.Format("{0} mana", TotalMana.ToString("N00")));
            dictValues.Add("Average Healing per sec", string.Format("{0} hps", AvgHPS.ToString("N00")));
            dictValues.Add("Average Healing per mana", string.Format("{0} hpm", AvgHPM.ToString("N02")));
            // Flash of Light
            dictValues.Add("FoL Average Heal", string.Format("{0} healing", FoLAvgHeal.ToString("N00")));
            dictValues.Add("FoL Crit", string.Format("{0}%", (FoLCrit * 100).ToString("N02")));
            dictValues.Add("FoL Cast Time", string.Format("{0} sec", FoLCastTime.ToString("N02")));
            dictValues.Add("FoL Healing per sec", string.Format("{0} hps", FoLHPS.ToString("N00")));
            dictValues.Add("FoL Healing per mana", string.Format("{0} hpm", FoLHPM.ToString("N02")));
            dictValues.Add("FoL Rotation Time", string.Format("{0} sec", FoLTime.ToString("N02")));
            dictValues.Add("FoL Healed", string.Format("{0} healing", FoLHealed.ToString("N00")));
            dictValues.Add("FoL Mana Usage", string.Format("{0} mana", FoLUsage.ToString("N00")));
            // Holy Light
            dictValues.Add("HL Average Heal", string.Format("{0} healing", HLAvgHeal.ToString("N00")));
            dictValues.Add("HL Crit", string.Format("{0}%", (HLCrit * 100).ToString("N02")));
            dictValues.Add("HL Cast Time", string.Format("{0} sec", HLCastTime.ToString("N02")));
            dictValues.Add("HL Healing per sec", string.Format("{0} hps", HLHPS.ToString("N00")));
            dictValues.Add("HL Healing per mana", string.Format("{0} hpm", HLHPM.ToString("N02")));
            dictValues.Add("HL Rotation Time", string.Format("{0} sec", HLTime.ToString("N02")));
            dictValues.Add("HL Healed",  string.Format("{0} healing", HLHealed.ToString("N00")));
            dictValues.Add("HL Mana Usage", string.Format("{0} mana", HLUsage.ToString("N00")));
            // Holy Shock
            dictValues.Add("HS Average Heal", string.Format("{0} healing", HSAvgHeal.ToString("N00")));
            dictValues.Add("HS Crit", string.Format("{0}%", (HSCrit * 100).ToString("N02")));
            dictValues.Add("HS Cast Time", string.Format("{0} sec", HSCastTime.ToString("N02")));
            dictValues.Add("HS Healing per sec", string.Format("{0} hps", HSHPS.ToString("N00")));
            dictValues.Add("HS Healing per mana", string.Format("{0} hpm", HSHPM.ToString("N02")));
            dictValues.Add("HS Rotation Time", string.Format("{0} sec", HSTime.ToString("N02")));
            dictValues.Add("HS Healed",  string.Format("{0} healing", HSHealed.ToString("N00")));
            dictValues.Add("HS Mana Usage", string.Format("{0} mana", HSUsage.ToString("N00")));
            // Beacon of Light
            dictValues.Add("BoL Healed",  string.Format("{0} healing", BoLHealed.ToString("N00")));
            dictValues.Add("BoL Casts", string.Format("{0} gcds", BoLCasts.ToString("N00")));
            dictValues.Add("BoL Mana Usage", string.Format("{0} mana", BoLUsage.ToString("N00")));
            // Judgement
            dictValues.Add("JotP Effective Haste", string.Format("{0}% haste", (JotPHaste * 100).ToString("N02")));
            dictValues.Add("JotP Casts", string.Format("{0} gcds", JotPCasts.ToString("N00")));
            dictValues.Add("JotP Mana Usage", string.Format("{0} mana", JotPUsage.ToString("N00")));

            return dictValues;
        }
    }
}
