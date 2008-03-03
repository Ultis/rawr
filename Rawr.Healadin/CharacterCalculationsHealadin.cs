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

        private Stats _basicStats;
        public Stats BasicStats
        {
            get { return _basicStats; }
            set { _basicStats = value; }
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
            dictValues.Add("Spell Crit Rating", BasicStats.SpellCritRating.ToString());
            dictValues.Add("Spell Haste Rating", BasicStats.SpellHasteRating.ToString());
            dictValues.Add("FoL Hps", _spells[0].Hps.ToString());
            dictValues.Add("FoL Crit", (Math.Round(_spells[0].SpellCrit * 100, 2)).ToString() + "%");
            dictValues.Add("Time to OOM", LongevityPoints.ToString());
            dictValues.Add("Cycle Hps", ThroughputPoints.ToString());

            return dictValues;
        }
    }
}
