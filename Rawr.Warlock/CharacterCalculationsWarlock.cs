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

        public float DPS { get; set; }

        public List<Spell> Spells { get; set; }
        public Character character { get; set; }

        public override Dictionary<string, string> GetCharacterDisplayCalculationValues()
        {
            CalculationsWarlock cw = new CalculationsWarlock();
            Spell sb = new ShadowBolt(character, TotalStats);
            Dictionary<string, string> vals = new Dictionary<string, string>();
            vals.Add("Health", TotalStats.Health.ToString());
            vals.Add("Mana", TotalStats.Mana.ToString());
            vals.Add("Stamina", TotalStats.Stamina.ToString());
            vals.Add("Intellect", TotalStats.Intellect.ToString());
            vals.Add("Spell Crit Rate", (TotalStats.SpellCritRating / 22.08f).ToString());
            vals.Add("Spell Hit Rate", (TotalStats.SpellHitRating / 12.625f).ToString());
            vals.Add("Casting Speed", (TotalStats.SpellHasteRating / 1570f + 1f).ToString());
            vals.Add("Shadow Damage", (TotalStats.SpellShadowDamageRating + TotalStats.SpellDamageRating).ToString());
            vals.Add("Fire Damage", (TotalStats.SpellFireDamageRating + TotalStats.SpellDamageRating).ToString());
            vals.Add("DPS", DPS.ToString());
            vals.Add("SB Min Hit", sb.MinDamage.ToString());
            vals.Add("SB Max Hit", sb.MaxDamage.ToString());
            vals.Add("SB Min Crit", (sb.MinDamage * sb.CritModifier).ToString());
            vals.Add("SB Max Crit", (sb.MaxDamage * sb.CritModifier).ToString());
            vals.Add("SB Average Hit", sb.AverageDamage.ToString());
            vals.Add("SB Crit Rate", sb.CritPercent.ToString());
            
            return vals;
        }
    }
}
