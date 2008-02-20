using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rawr.Moonkin
{
    class CharacterCalculationsMoonkin : CharacterCalculationsBase
    {
        private float overallPoints = 0f;
        public override float OverallPoints
        {
            get
            {
                return overallPoints;
            }
            set
            {
                overallPoints = value;
            }
        }

        private float[] subPoints = new float[] { 0f, 0f };

        public override float[] SubPoints
        {
            get
            {
                return subPoints;
            }
            set
            {
                subPoints = value;
            }
        }

        public float SpellHit { get; set; }
        public float SpellCrit { get; set; }
        public float ArcaneDamage { get; set; }
        public float NatureDamage { get; set; }
        public float ManaRegen { get; set; }
        public float ManaRegen5SR { get; set; }

        private Stats baseStats;
        public Stats BasicStats
        {
            get
            {
                return baseStats;
            }
            set
            {
                baseStats = value;
            }
        }

        public override Dictionary<string, string> GetCharacterDisplayCalculationValues()
        {
            Dictionary<string, string> retVal = new Dictionary<string, string>();
            retVal.Add("Health", baseStats.Health.ToString());
            retVal.Add("Mana", baseStats.Mana.ToString());
            retVal.Add("Armor", baseStats.Armor.ToString());
            retVal.Add("Agility", baseStats.Agility.ToString());
            retVal.Add("Stamina", baseStats.Stamina.ToString());
            retVal.Add("Intellect", baseStats.Intellect.ToString());
            retVal.Add("Spirit", baseStats.Spirit.ToString());
            retVal.Add("Spell Hit", String.Format("{0:F}%", 100 * SpellHit));
            retVal.Add("Spell Crit", String.Format("{0:F}%", 100 * SpellCrit));
            retVal.Add("Spell Haste", baseStats.SpellHasteRating.ToString());
            retVal.Add("Arcane Damage", ArcaneDamage.ToString());
            retVal.Add("Nature Damage", NatureDamage.ToString());
            retVal.Add("OO5SR MP5", ManaRegen.ToString());
            retVal.Add("I5SR MP5", ManaRegen5SR.ToString());

            return retVal;
        }
    }
}
