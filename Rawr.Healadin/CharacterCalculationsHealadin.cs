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
            dictValues.Add("Health", BasicStats.Health.ToString());
            dictValues.Add("Stamina", BasicStats.Stamina.ToString());
            dictValues.Add("Mana", BasicStats.Mana.ToString());
            dictValues.Add("Intellect", BasicStats.Intellect.ToString());
            dictValues.Add("Spell Power", BasicStats.SpellPower.ToString());
            dictValues.Add("Mp5", BasicStats.Mp5.ToString());
            dictValues.Add("Spell Crit", string.Format("{0}%*{1} Crit Rating", Math.Round(BasicStats.SpellCrit * 100, 2), BasicStats.CritRating));
            dictValues.Add("Spell Haste", string.Format("{0}%*{1} Haste Rating", Math.Round(BasicStats.SpellHaste * 100, 2), BasicStats.HasteRating));
            // Cycle Stats
            dictValues.Add("Total Healed", Math.Round(TotalHealed).ToString());
            dictValues.Add("Total Mana", Math.Round(TotalMana).ToString());
            dictValues.Add("Average Hps", Math.Round(AvgHPS).ToString());
            dictValues.Add("Average Hpm", Math.Round(AvgHPM, 2).ToString());
            // Flash of Light
            dictValues.Add("FoL Average Heal", string.Format("{0}", Math.Round(FoLAvgHeal)));
            dictValues.Add("FoL Crit", string.Format("{0}%", Math.Round(FoLCrit * 100, 2)));
            dictValues.Add("FoL Cast Time", string.Format("{0} sec", Math.Round(FoLCastTime, 2)));
            dictValues.Add("FoL Healing per sec", string.Format("{0} hps", Math.Round(FoLHPS)));
            dictValues.Add("FoL Healing per mana", string.Format("{0} hpm", Math.Round(FoLHPM, 2)));
            dictValues.Add("FoL Rotation Time", string.Format("{0} sec", Math.Round(FoLTime, 2)));
            dictValues.Add("FoL Healed", Math.Round(FoLHealed).ToString("N00"));
            dictValues.Add("FoL Mana Usage", string.Format("{0} mana", Math.Round(FoLUsage)));
            // Holy Light
            dictValues.Add("HL Average Heal", string.Format("{0}", Math.Round(HLAvgHeal)));
            dictValues.Add("HL Crit", string.Format("{0}%", Math.Round(HLCrit * 100, 2)));
            dictValues.Add("HL Cast Time", string.Format("{0} sec", Math.Round(HLCastTime, 2)));
            dictValues.Add("HL Healing per sec", string.Format("{0} hps", Math.Round(HLHPS)));
            dictValues.Add("HL Healing per mana", string.Format("{0} hpm", Math.Round(HLHPM, 2)));
            dictValues.Add("HL Rotation Time", string.Format("{0} sec", Math.Round(HLTime, 2)));
            dictValues.Add("HL Healed", Math.Round(HLHealed).ToString("N00"));
            dictValues.Add("HL Mana Usage", string.Format("{0} mana", Math.Round(HLUsage)));
            // Holy Shock
            dictValues.Add("HS Average Heal", string.Format("{0}", Math.Round(HSAvgHeal)));
            dictValues.Add("HS Crit", string.Format("{0}%", Math.Round(HSCrit * 100, 2)));
            dictValues.Add("HS Cast Time", string.Format("{0} sec", Math.Round(HSCastTime, 2)));
            dictValues.Add("HS Healing per sec", string.Format("{0} hps", Math.Round(HSHPS)));
            dictValues.Add("HS Healing per mana", string.Format("{0} hpm", Math.Round(HSHPM, 2)));
            dictValues.Add("HS Rotation Time", string.Format("{0} sec", Math.Round(HSTime, 2)));
            dictValues.Add("HS Healed", Math.Round(HSHealed).ToString("N00"));
            dictValues.Add("HS Mana Usage", string.Format("{0} mana", Math.Round(HSUsage)));
            // Beacon of Light
            dictValues.Add("BoL Healed", Math.Round(BoLHealed).ToString("N00"));
            dictValues.Add("BoL Casts", string.Format("{0} gcds", Math.Round(BoLCasts)));
            dictValues.Add("BoL Mana Usage", string.Format("{0} mana", Math.Round(BoLUsage)));
            // Judgement
            dictValues.Add("JotP Effective Haste", string.Format("{0}%", Math.Round(JotPHaste * 100, 2)));
            dictValues.Add("JotP Casts", string.Format("{0} gcds", Math.Round(JotPCasts)));
            dictValues.Add("JotP Mana Usage", string.Format("{0} mana", Math.Round(JotPUsage)));

            return dictValues;
        }
    }
}
