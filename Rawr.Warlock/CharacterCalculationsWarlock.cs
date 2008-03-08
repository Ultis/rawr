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

        public Dictionary<Spell, int> NumCasts
        {
            get;
            set;
        }

        public int NumLifetaps
        {
            get;
            set;
        }

        public int LifetapManaReturn
        {
            get;
            set;
        }

        public float DPS { get; set; }

        public List<Spell> Spells { get; set; }
        public Character character { get; set; }

        public override Dictionary<string, string> GetCharacterDisplayCalculationValues()
        {
            CalculationsWarlock cw = new CalculationsWarlock();
            List<Spell> castSpell = new List<Spell>(NumCasts.Keys);
            
            Dictionary<string, string> vals = new Dictionary<string, string>();
            vals.Add("Health", TotalStats.Health.ToString());
            vals.Add("Mana", TotalStats.Mana.ToString());
            vals.Add("Stamina", TotalStats.Stamina.ToString());
            vals.Add("Intellect", TotalStats.Intellect.ToString());
            vals.Add("Spell Crit Rate", (TotalStats.SpellCritRating / 22.08f).ToString());
            vals.Add("Spell Hit Rate", (TotalStats.SpellHitRating / 12.625f).ToString());
            vals.Add("Casting Speed", (1f / (TotalStats.SpellHasteRating / 1570f + 1f)).ToString());
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
                vals.Add("#SB Casts", NumCasts[sb].ToString());

            }
            else
            {
                vals.Add("SB Min Hit", "");
                vals.Add("SB Max Hit", "");
                vals.Add("SB Min Crit", "");
                vals.Add("SB Max Crit", "");
                vals.Add("SB Average Hit", "");
                vals.Add("SB Crit Rate", "");
                vals.Add("ISB Uptime", "");
                vals.Add("#SB Casts", "0");

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
                vals.Add("#Incinerate Casts", NumCasts[sb].ToString());
            }
            else
            {
                vals.Add("Incinerate Min Hit","");
                vals.Add("Incinerate Max Hit","");
                vals.Add("Incinerate Min Crit","");
                vals.Add("Incinerate Max Crit","");
                vals.Add("Incinerate Average Hit", "");
                vals.Add("Incinerate Crit Rate","");
                vals.Add("#Incinerate Casts","0");
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
                vals.Add("#Immolate Casts", NumCasts[sb].ToString());
            }
            else 
            {
                vals.Add("ImmolateMin Hit","");
                vals.Add("ImmolateMax Hit","");
                vals.Add("ImmolateMin Crit","");
                vals.Add("ImmolateMax Crit","");
                vals.Add("ImmolateAverage Hit","");
                vals.Add("ImmolateCrit Rate","");
                vals.Add("#Immolate Casts","0");
            }
            if (Spells.Exists(delegate(Spell s) { return s.Name.ToUpper() == "CURSEOFAGONY"; }))
            {
                CurseOfAgony sb = new CurseOfAgony(character, TotalStats);
                vals.Add("CoA Tick", (sb.AverageDamage / (sb.PeriodicDuration / sb.PeriodicTickInterval)).ToString());
                vals.Add("CoA Total Damage", sb.AverageDamage.ToString());
                vals.Add("#CoA Casts", NumCasts[sb].ToString());
            }
            else
            {
                vals.Add("CoA Tick","");
                vals.Add("CoA Total Damage","");
                vals.Add("#CoA Casts","0");
            }
            if (Spells.Exists(delegate(Spell s) { return s.Name.ToUpper() == "CURSEOFDOOM"; }))
            {
                CurseOfDoom sb = new CurseOfDoom(character, TotalStats);
                vals.Add("CoD Total Damage", sb.AverageDamage.ToString());
                vals.Add("#CoD Casts", NumCasts[sb].ToString());
            }
            else
            {
                vals.Add("CoD Total Damage","");
                vals.Add("#CoD Casts","");
            }
            if (Spells.Exists(delegate(Spell s) { return s.Name.ToUpper() == "CORRUPTION"; }))
            {
                Corruption sb = new Corruption(character, TotalStats);
                vals.Add("Corr Tick", (sb.AverageDamage / (sb.PeriodicDuration / sb.PeriodicTickInterval)).ToString());
                vals.Add("Corr Total Damage", sb.AverageDamage.ToString());
                vals.Add("#Corr Casts", NumCasts[sb].ToString());
            }
            else
            {
                vals.Add("Corr Tick", "");
                vals.Add("Corr Total Damage","");
                vals.Add("#Corr Casts","");
            }
            if (Spells.Exists(delegate(Spell s) { return s.Name.ToUpper() == "UNSTABLEAFFLICTION"; }))
            {
                UnstableAffliction sb = new UnstableAffliction(character, TotalStats);
                vals.Add("UA Tick", (sb.AverageDamage / (sb.PeriodicDuration / sb.PeriodicTickInterval)).ToString());
                vals.Add("UA Total Damage", sb.AverageDamage.ToString());
                vals.Add("#UA Casts", NumCasts[sb].ToString());
            }
            else
            {
                vals.Add("UA Tick", "");
                vals.Add("UA Total Damage","");
                vals.Add("#UA Casts", "0");
            }
            if (Spells.Exists(delegate(Spell s) { return s.Name.ToUpper() == "SIPHONLIFE"; }))
            {
                SiphonLife sb = new SiphonLife(character, TotalStats);
                vals.Add("SL Tick", (sb.AverageDamage / (sb.PeriodicDuration / sb.PeriodicTickInterval)).ToString());
                vals.Add("SL Total Damage", sb.AverageDamage.ToString());
                vals.Add("#SL Casts", NumCasts[sb].ToString());
            }
            else
            {
                vals.Add("SL Tick","");
                vals.Add("SL Total Damage","");
                vals.Add("#SL Casts","0");
            }
            vals.Add("#Lifetaps", NumLifetaps.ToString());
            vals.Add("Mana Per LT", LifetapManaReturn.ToString());
            
            return vals;
        }
    }
}
