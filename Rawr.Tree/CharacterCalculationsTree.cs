using System;
using System.Collections.Generic;

namespace Rawr.Tree
{

    public class CharacterCalculationsTree : CharacterCalculationsBase
    {
        private Stats basicStats;
        public Stats BasicStats
        {
            get { return basicStats; }
            set { basicStats = value; }
        }

        private float overallPoints;
        public override float OverallPoints
        {
            get { return overallPoints; }
            set { overallPoints = value; }
        }

        private float[] subPoints = new float[] { 0f };
        public override float[] SubPoints
        {
            get { return subPoints; }
            set { subPoints = value; }
        }

        public float HealPoints
        {
            get { return subPoints[0]; }
            set { subPoints[0] = value; }
        }

        public float OS5SRRegen
        {
            get;
            set;
        }

        public float IS5SRRegen
        {
            get;
            set;
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
            dictValues.Add("Mp5", string.Format("{0}*{1} mp5 outside the 5-second rule",
                (int) Math.Round(5*IS5SRRegen),
                (int) Math.Round(5 * OS5SRRegen)));

            dictValues.Add("Spell Crit", string.Format("{0}%*{1} Spell Crit rating\n",
                BasicStats.SpellCrit, BasicStats.SpellCritRating.ToString()));
            
            dictValues.Add("Spell Haste", string.Format("{0}%*{1} Spell Haste rating\nGlobal cooldown is {2} seconds", 
                Math.Round(BasicStats.SpellHasteRating / 15.7, 2),
                BasicStats.SpellHasteRating.ToString(),
                Math.Round((1.5f * 1570f) / (1570f + BasicStats.SpellHasteRating), 2)));
            return dictValues;
        }
    }
}
