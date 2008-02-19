using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rawr.Mage
{
    class CharacterCalculationsMage : CharacterCalculationsBase
    {
        private float _overallPoints = 0f;
        public override float OverallPoints
        {
            get { return _overallPoints; }
            set { _overallPoints = value; }
        }

        private float[] _subPoints = new float[] { 0f };
        public override float[] SubPoints
        {
            get { return _subPoints; }
            set { _subPoints = value; }
        }

        private Stats _basicStats;
        public Stats BasicStats
        {
            get { return _basicStats; }
            set { _basicStats = value; }
        }

        public float CastingSpeed { get; set; }
        public float ArcaneDamage { get; set; }
        public float FireDamage { get; set; }
        public float FrostDamage { get; set; }
        public float SpiritRegen { get; set; }
        public float ManaRegen { get; set; }
        public float ManaRegen5SR { get; set; }
        public float ManaRegenDrinking { get; set; }
        public float HealthRegen { get; set; }
        public float HealthRegenCombat { get; set; }
        public float HealthRegenEating { get; set; }
        public float MeleeMitigation { get; set; }
        public float Defense { get; set; }
        public float PhysicalCritReduction { get; set; }
        public float SpellCritReduction { get; set; }
        public float CritDamageReduction { get; set; }
        public float Dodge { get; set; }

        public override Dictionary<string, string> GetCharacterDisplayCalculationValues()
        {
            Dictionary<string, string> dictValues = new Dictionary<string, string>();
            dictValues.Add("Stamina", BasicStats.Stamina.ToString());
            dictValues.Add("Intellect", BasicStats.Intellect.ToString());
            dictValues.Add("Spirit", BasicStats.Spirit.ToString());
            dictValues.Add("Armor", BasicStats.Armor.ToString());
            dictValues.Add("Health", BasicStats.Health.ToString());
            dictValues.Add("Mana", BasicStats.Mana.ToString());
            dictValues.Add("Spell Crit Rate", BasicStats.SpellCritRating.ToString());
            dictValues.Add("Spell Hit Rate", BasicStats.SpellHitRating.ToString());
            dictValues.Add("Spell Penetration", BasicStats.SpellPenetration.ToString());
            dictValues.Add("Casting Speed", CastingSpeed.ToString());
            dictValues.Add("Arcane Damage", ArcaneDamage.ToString());
            dictValues.Add("Fire Damage", FireDamage.ToString());
            dictValues.Add("Frost Damage", FrostDamage.ToString());
            dictValues.Add("MP5", BasicStats.Mp5.ToString());
            dictValues.Add("Mana Regen in 5SR", ManaRegen5SR.ToString());
            dictValues.Add("Mana Regen", ManaRegen.ToString());
            dictValues.Add("Mana Regen Drinking", ManaRegenDrinking.ToString());
            dictValues.Add("Health Regen in Combat", HealthRegenCombat.ToString());
            dictValues.Add("Health Regen", HealthRegen.ToString());
            dictValues.Add("Health Regen Eating", HealthRegenEating.ToString());
            dictValues.Add("Arcane Resistance", BasicStats.ArcaneResistance.ToString());
            dictValues.Add("Fire Resistance", BasicStats.FireResistance.ToString());
            dictValues.Add("Nature Resistance", BasicStats.NatureResistance.ToString());
            dictValues.Add("Frost Resistance", BasicStats.FrostResistance.ToString());
            dictValues.Add("Shadow Resistance", BasicStats.ShadowResistance.ToString());
            dictValues.Add("Physical Mitigation", MeleeMitigation.ToString());
            dictValues.Add("Resilience", BasicStats.Resilience.ToString());
            dictValues.Add("Defense", Defense.ToString());
            dictValues.Add("Physical Crit Reduction", PhysicalCritReduction.ToString());
            dictValues.Add("Spell Crit Reduction", SpellCritReduction.ToString());
            dictValues.Add("Crit Damage Reduction", CritDamageReduction.ToString());
            dictValues.Add("Dodge", Dodge.ToString());

            return dictValues;
        }
    }
}
