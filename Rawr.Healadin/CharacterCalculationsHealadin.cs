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

        private float[] _subPoints = new float[] { 0f , 0f };
        public override float[] SubPoints
        {
            get { return _subPoints; }
            set { _subPoints = value; }
        }

        public float FightPoints
        {
            get { return _subPoints[0]; }
            set { _subPoints[0] = value; }
        }

        public float BurstPoints
        {
            get { return _subPoints[1]; }
            set { _subPoints[1] = value; }
        }

        public float AvgHPS { get; set; }
        public float AvgHPM { get; set; }
        public float TotalHealed { get; set; }
        public float TotalMana { get; set; }

        public FlashOfLight FoL { get; set; }
        public HolyLight HL { get; set; }
        public HolyShock HS { get; set; }
        public SacredShield SS { get; set; }
        public BeaconOfLight BoL { get; set; }
        public JudgementsOfThePure JotP { get; set; }

        public float RotationFoL { get; set; }
        public float RotationHL { get; set; }
        public float RotationHS { get; set; }
        public float RotationJotP { get; set; }
        public float RotationBoL { get; set; }
        public float RotationSS { get; set; }

        public float HealedFoL { get; set; }
        public float HealedHL { get; set; }
        public float HealedHS { get; set; }
        public float HealedGHL { get; set; }
        public float HealedBoL { get; set; }
        public float HealedSS { get; set; }
        public float HealedOther { get; set; }

        public float UsageFoL { get; set; }
        public float UsageHL { get; set; }
        public float UsageHS { get; set; }
        public float UsageBoL { get; set; }
        public float UsageSS { get; set; }
        public float UsageJotP { get; set; }

        public float ManaBase { get; set; }
        public float ManaMp5 { get; set; }
        public float ManaPotion { get; set; }
        public float ManaReplenishment { get; set; }
        public float ManaArcaneTorrent { get; set; }
        public float ManaDivinePlea { get; set; }
        public float ManaLayOnHands { get; set; }
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
            dictValues.Add("Status", string.Format("Overall: {0,-10:0}\tFight: {1,-10:0}\tBurst: {2,-10:0}", OverallPoints, FightPoints, BurstPoints));

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

            // Rotation Info
            dictValues.Add("Holy Light Time", string.Format("{0} sec", RotationHL.ToString("N1")));
            dictValues.Add("Flash of Light Time", string.Format("{0} sec", RotationFoL.ToString("N1")));
            dictValues.Add("Holy Shock Time", string.Format("{0} sec", RotationHS.ToString("N1")));
            dictValues.Add("Sacred Shield Time", string.Format("{0} sec", RotationSS.ToString("N1")));
            dictValues.Add("Beacon of Light Time", string.Format("{0} sec", RotationBoL.ToString("N1")));
            dictValues.Add("Judgement Time", string.Format("{0} sec", RotationJotP.ToString("N1")));

            // Healing Breakdown
            dictValues.Add("Holy Light Healed", string.Format("{0} healed", HealedHL.ToString("N00")));
            dictValues.Add("Flash of Light Healed", string.Format("{0} healed", HealedFoL.ToString("N00")));
            dictValues.Add("Holy Shock Healed", string.Format("{0} healed", HealedHS.ToString("N00")));
            dictValues.Add("Sacred Shield Healed", string.Format("{0} healed", HealedSS.ToString("N00")));
            dictValues.Add("Beacon of Light Healed", string.Format("{0} healed", HealedBoL.ToString("N00")));
            dictValues.Add("Glyph of HL Healed", string.Format("{0} healed", HealedGHL.ToString("N00")));
            dictValues.Add("Other Healed", string.Format("{0} healed", HealedOther.ToString("N00")));

            // Holy Light
            dictValues.Add("HL Average Heal", string.Format("{0} healed", HL.AverageHealed().ToString("N0")));
            dictValues.Add("HL Crit", string.Format("{0}%", (HL.ChanceToCrit()*100).ToString("N2")));
            dictValues.Add("HL Cast Time", string.Format("{0} sec", HL.CastTime().ToString("N2")));
            dictValues.Add("HL Averege Cost", string.Format("{0} mana", HL.AverageCost().ToString("N0")));
            dictValues.Add("HL Healing per sec", string.Format("{0} hps", HL.HPS().ToString("N0")));
            dictValues.Add("HL Healing per mana", string.Format("{0} hpm", HL.HPM().ToString("N2")));

            // Flash of Light
            dictValues.Add("FoL Average Heal", string.Format("{0} healed", FoL.AverageHealed().ToString("N0")));
            dictValues.Add("FoL Crit", string.Format("{0}%", (FoL.ChanceToCrit() * 100).ToString("N2")));
            dictValues.Add("FoL Cast Time", string.Format("{0} sec", FoL.CastTime().ToString("N2")));
            dictValues.Add("FoL Averege Cost", string.Format("{0} mana", FoL.AverageCost().ToString("N0")));
            dictValues.Add("FoL Healing per sec", string.Format("{0} hps", FoL.HPS().ToString("N0")));
            dictValues.Add("FoL Healing per mana", string.Format("{0} hpm", FoL.HPM().ToString("N2")));

            // Holy Shock
            dictValues.Add("HS Average Heal", string.Format("{0} healed", HS.AverageHealed().ToString("N0")));
            dictValues.Add("HS Crit", string.Format("{0}%", (HS.ChanceToCrit() * 100).ToString("N2")));
            dictValues.Add("HS Cast Time", string.Format("{0} sec", HS.CastTime().ToString("N2")));
            dictValues.Add("HS Averege Cost", string.Format("{0} mana", HS.AverageCost().ToString("N0")));
            dictValues.Add("HS Healing per sec", string.Format("{0} hps", HS.HPS().ToString("N0")));
            dictValues.Add("HS Healing per mana", string.Format("{0} hpm", HS.HPM().ToString("N2")));

            dictValues.Add("SS Average Absorb", SS.ProcAbsorb().ToString("N0"));
            dictValues.Add("SS Healing per sec", string.Format("{0} hps", SS.HPS().ToString("N0")));
            dictValues.Add("SS Healing per mana", string.Format("{0} hpm", SS.HPM().ToString("N2")));

            return dictValues;
        }
    }
}
