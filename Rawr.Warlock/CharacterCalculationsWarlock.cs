using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Warlock
{
    class CharacterCalculationsWarlock : CharacterCalculationsBase
    {

        private float _overallPoints = 0f;
        public override float OverallPoints
        {
            get { return _overallPoints; }
            set { _overallPoints = value; }
        }

        /// <summary>
        /// The Sub rating points for the whole character, in the order defined in SubPointNameColors.
        /// Should sum up to OverallPoints. You could have this field build/parse an array of floats based
        /// on floats stored in other fields, or you could have this get/set a private float[], and
        /// have the fields of your individual Sub points refer to specific indexes of this field.
        /// </summary>
        private float[] _subPoints = new float[] { 0f };
        public override float[] SubPoints
        {
            get { return _subPoints; }
            set { _subPoints = value; }
        }

        private Stats _totalStats;
        public Stats TotalStats
        {
            get { return _totalStats; }
            set { _totalStats = value; }
        }

      
        public override Dictionary<string, string> GetCharacterDisplayCalculationValues()
        {
            Dictionary<string, string> vals = new Dictionary<string, string>();
            vals.Add("Health", TotalStats.Health.ToString());
            vals.Add("Mana", TotalStats.Mana.ToString());
            vals.Add("Stamina", TotalStats.Stamina.ToString());
            vals.Add("Intellect", TotalStats.Intellect.ToString());
            vals.Add("Spell Crit Rate", (TotalStats.SpellCritRating / 22.08f).ToString());
            vals.Add("Spell Hit Rate", (TotalStats.SpellHitRating / 12.625f).ToString());
            vals.Add("Casting Speed", TotalStats.SpellHasteRating.ToString());
            vals.Add("Shadow Damage", (TotalStats.SpellShadowDamageRating + TotalStats.SpellDamageRating).ToString());
            vals.Add("Fire Damage", (TotalStats.SpellFireDamageRating + TotalStats.SpellDamageRating).ToString());
            return vals;
        }
    }
}
