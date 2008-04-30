using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Healadin
{

    public class CharacterCalculationsHealadin : CharacterCalculationsBase
    {

        public CharacterCalculationsHealadin()
            : base()
        {
            _spells = new Dictionary<int, Spell>();
        }

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

        public float ThroughputPoints
        {
            get { return _subPoints[0]; }
            set { _subPoints[0] = value; }
        }

        public float LongevityPoints
        {
            get { return _subPoints[1]; }
            set { _subPoints[1] = value; }
        }


        public float AvgHPS { get; set; }
        public float AvgHPM { get; set; }
        public float Healed { get; set; }
        public float TimeHL { get; set; }
        public float HealHL { get; set; }

        private Stats _basicStats;
        public Stats BasicStats
        {
            get { return _basicStats; }
            set { _basicStats = value; Calculate(false); }
        }

        private Dictionary<int, Spell> _spells;
        public Spell this[int i]
        {
            get { return _spells[i]; }
            set { _spells[i] = value; value.Calculate(_basicStats); }
        }

        public void Calculate() { Calculate(false); }
        public void Calculate(bool di)
        {
            foreach (KeyValuePair<int, Spell> kvp in _spells) { kvp.Value.Calculate(_basicStats, di); }
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
            dictValues.Add("Spell Crit", Math.Round(_spells[0].SpellCrit*100-5,2).ToString()+"%*"+BasicStats.SpellCritRating.ToString() + " Spell Crit rating");
            dictValues.Add("Spell Haste", Math.Round(BasicStats.SpellHasteRating/15.7,2).ToString()+"%*"
                +BasicStats.SpellHasteRating.ToString()+ " Spell Haste rating");
            dictValues.Add("Total Healed", Math.Round(Healed).ToString());
            dictValues.Add("Average Hps", Math.Round(AvgHPS).ToString());
            dictValues.Add("Average Hpm", Math.Round(AvgHPM, 2).ToString());
            dictValues.Add("Holy Light Time", Math.Round(TimeHL * 100).ToString() + "%");
            dictValues.Add("Holy Light Healing", Math.Round(HealHL * 100).ToString() + "%");
            dictValues.Add("Flash of Light", Math.Round(_spells[0].Hps).ToString() + " hps*"+Math.Round(_spells[0].Hpm,2).ToString()+
                " hpm\n" + Math.Round(_spells[0].AverageHeal).ToString() + " average heal");
            dictValues.Add("Holy Light 11", Math.Round(_spells[1].Hps).ToString() + " hps*" + Math.Round(_spells[1].Hpm, 2).ToString() +
                " hpm\n" + Math.Round(_spells[1].AverageHeal).ToString() + " average heal");
            dictValues.Add("Holy Light 10", Math.Round(_spells[2].Hps).ToString() + " hps*" + Math.Round(_spells[2].Hpm, 2).ToString() +
                " hpm\n" + Math.Round(_spells[2].AverageHeal).ToString() + " average heal");
            dictValues.Add("Holy Light 9", Math.Round(_spells[3].Hps).ToString() + " hps*" + Math.Round(_spells[3].Hpm, 2).ToString() +
                " hpm\n" + Math.Round(_spells[3].AverageHeal).ToString() + " average heal");
            dictValues.Add("Holy Light 8", Math.Round(_spells[4].Hps).ToString() + " hps*" + Math.Round(_spells[4].Hpm, 2).ToString() +
                " hpm\n" + Math.Round(_spells[4].AverageHeal).ToString() + " average heal");
            dictValues.Add("Holy Light 7", Math.Round(_spells[5].Hps).ToString() + " hps*" + Math.Round(_spells[5].Hpm, 2).ToString() +
                " hpm\n" + Math.Round(_spells[5].AverageHeal).ToString() + " average heal");
            dictValues.Add("Holy Light 6", Math.Round(_spells[6].Hps).ToString() + " hps*" + Math.Round(_spells[6].Hpm, 2).ToString() +
                " hpm\n" + Math.Round(_spells[6].AverageHeal).ToString() + " average heal");
            dictValues.Add("Holy Light 5", Math.Round(_spells[7].Hps).ToString() + " hps*" + Math.Round(_spells[7].Hpm, 2).ToString() +
                " hpm\n" + Math.Round(_spells[7].AverageHeal).ToString() + " average heal");
            dictValues.Add("Holy Light 4", Math.Round(_spells[8].Hps).ToString() + " hps*" + Math.Round(_spells[8].Hpm, 2).ToString() +
                " hpm\n" + Math.Round(_spells[8].AverageHeal).ToString() + " average heal");
            return dictValues;
        }
    }
}
