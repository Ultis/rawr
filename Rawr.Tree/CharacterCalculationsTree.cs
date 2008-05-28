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

            dictValues.Add("Spell Crit", string.Format("{0}%*{1} Spell Crit rating\n",
                BasicStats.SpellCrit, BasicStats.SpellCritRating.ToString()));
            
            dictValues.Add("Spell Haste", string.Format("{0}%*{1} Spell Haste rating\n", 
                Math.Round(BasicStats.SpellHasteRating / 15.7, 2), BasicStats.SpellHasteRating.ToString()));
            return dictValues;
        }
    }
}
