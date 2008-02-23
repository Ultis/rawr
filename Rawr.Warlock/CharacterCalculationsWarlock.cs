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
            if (Spells.Exists(delegate(Spell s) { return s.Name.ToUpper() == "SHADOWBOLT"; }))
            {
                ShadowBolt sb = new ShadowBolt(character, TotalStats);
                vals.Add("SB Min Hit", sb.MinDamage.ToString());
                vals.Add("SB Max Hit", sb.MaxDamage.ToString());
                vals.Add("SB Min Crit", (sb.MinDamage * sb.CritModifier).ToString());
                vals.Add("SB Max Crit", (sb.MaxDamage * sb.CritModifier).ToString());
                vals.Add("SB Average Hit", sb.AverageDamage.ToString());
                vals.Add("SB Crit Rate", sb.CritPercent.ToString());
                vals.Add("ISB Uptime", (sb.ISBuptime * 100f).ToString());
            }
            if (Spells.Exists(delegate(Spell s) { return s.Name.ToUpper() == "INCINERATE"; }))
            {
                Incinerate sb = new Incinerate(character, TotalStats);
                vals.Add("Incinerate Min Hit", sb.MinDamage.ToString());
                vals.Add("Incinerate Max Hit", sb.MaxDamage.ToString());
                vals.Add("Incinerate Min Crit", (sb.MinDamage * sb.CritModifier).ToString());
                vals.Add("Incinerate Max Crit", (sb.MaxDamage * sb.CritModifier).ToString());
                vals.Add("Incinerate Average Hit", sb.AverageDamage.ToString());
                vals.Add("Incinerate Crit Rate", sb.CritPercent.ToString());
            }
            if (Spells.Exists(delegate(Spell s) { return s.Name.ToUpper() == "IMMOLATE"; }))
            {
                Immolate sb = new Immolate(character, TotalStats);
                vals.Add("ImmolateMin Hit", sb.MinDamage.ToString());
                vals.Add("ImmolateMax Hit", sb.MaxDamage.ToString());
                vals.Add("ImmolateMin Crit", (sb.MinDamage * sb.CritModifier).ToString());
                vals.Add("ImmolateMax Crit", (sb.MaxDamage * sb.CritModifier).ToString());
                vals.Add("ImmolateAverage Hit", sb.AverageDamage.ToString());
                vals.Add("ImmolateCrit Rate", sb.CritPercent.ToString());
            }
            if (Spells.Exists(delegate(Spell s) { return s.Name.ToUpper() == "CURSEOFAGONY"; }))
            {
                CurseOfAgony sb = new CurseOfAgony(character, TotalStats);
                vals.Add("CoA Tick", (sb.AverageDamage / (sb.PeriodicDuration / sb.PeriodicTickInterval)).ToString());
                vals.Add("CoA Total Damage", sb.AverageDamage.ToString());
            }
            if (Spells.Exists(delegate(Spell s) { return s.Name.ToUpper() == "CURSEOFDOOM"; }))
            {
                CurseOfDoom sb = new CurseOfDoom(character, TotalStats);
                vals.Add("CoA Total Damage", sb.AverageDamage.ToString());
            }
            if (Spells.Exists(delegate(Spell s) { return s.Name.ToUpper() == "UNSTABLEAFFLICTION"; }))
            {
                UnstableAffliction sb = new UnstableAffliction(character, TotalStats);
                vals.Add("UA Tick", (sb.AverageDamage / (sb.PeriodicDuration / sb.PeriodicTickInterval)).ToString());
                vals.Add("UA Total Damage", sb.AverageDamage.ToString());
            }
            if (Spells.Exists(delegate(Spell s) { return s.Name.ToUpper() == "SIPHONLIFE"; }))
            {
                SiphonLife sb = new SiphonLife(character, TotalStats);
                vals.Add("Siphon Life Tick", (sb.AverageDamage / (sb.PeriodicDuration / sb.PeriodicTickInterval)).ToString());
                vals.Add("SiphonLife Total Damage", sb.AverageDamage.ToString());
            }
            
            return vals;
        }
    }
}
