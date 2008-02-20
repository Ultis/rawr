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

        public float SpellCrit { get; set; }
        public float SpellHit { get; set; }
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
        public float ArcaneSpellModifier { get; set; }
        public float FireSpellModifier { get; set; }
        public float FrostSpellModifier { get; set; }
        public float ArcaneCritBonus { get; set; }
        public float FireCritBonus { get; set; }
        public float FrostCritBonus { get; set; }
        public float ArcaneCritRate { get; set; }
        public float FireCritRate { get; set; }
        public float FrostCritRate { get; set; }
        public float ResilienceCritDamageReduction { get; set; }
        public float ResilienceCritRateReduction { get; set; }

        public List<Spell> Spells;

        public override Dictionary<string, string> GetCharacterDisplayCalculationValues()
        {
            Dictionary<string, string> dictValues = new Dictionary<string, string>();
            dictValues.Add("Stamina", BasicStats.Stamina.ToString());
            dictValues.Add("Intellect", BasicStats.Intellect.ToString());
            dictValues.Add("Spirit", BasicStats.Spirit.ToString());
            dictValues.Add("Armor", BasicStats.Armor.ToString());
            dictValues.Add("Health", BasicStats.Health.ToString());
            dictValues.Add("Mana", BasicStats.Mana.ToString());
            dictValues.Add("Spell Crit Rate", String.Format("{0:F}%", 100 * SpellCrit));
            dictValues.Add("Spell Hit Rate", String.Format("{0:F}%", 100 * SpellHit));
            dictValues.Add("Spell Penetration", BasicStats.SpellPenetration.ToString());
            dictValues.Add("Casting Speed", CastingSpeed.ToString());
            dictValues.Add("Arcane Damage", ArcaneDamage.ToString());
            dictValues.Add("Fire Damage", FireDamage.ToString());
            dictValues.Add("Frost Damage", FrostDamage.ToString());
            dictValues.Add("MP5", BasicStats.Mp5.ToString());
            dictValues.Add("Mana Regen", Math.Floor(ManaRegen * 5).ToString() + String.Format("*Mana Regen in 5SR: {0}\nMana Regen Drinking: {1}", Math.Floor(ManaRegen5SR * 5), Math.Floor(ManaRegenDrinking * 5)));
            dictValues.Add("Health Regen", Math.Floor(HealthRegenCombat * 5).ToString() + String.Format("*Health Regen Eating: {0}", Math.Floor(HealthRegenEating * 5)));
            dictValues.Add("Arcane Resist", (BasicStats.AllResist + BasicStats.ArcaneResistance).ToString());
            dictValues.Add("Fire Resist", (BasicStats.AllResist + BasicStats.FireResistance).ToString());
            dictValues.Add("Nature Resist", (BasicStats.AllResist + BasicStats.NatureResistance).ToString());
            dictValues.Add("Frost Resist", (BasicStats.AllResist + BasicStats.FrostResistance).ToString());
            dictValues.Add("Shadow Resist", (BasicStats.AllResist + BasicStats.ShadowResistance).ToString());
            dictValues.Add("Physical Mitigation", String.Format("{0:F}%", 100 * MeleeMitigation));
            dictValues.Add("Resilience", BasicStats.Resilience.ToString());
            dictValues.Add("Defense", Defense.ToString());
            dictValues.Add("Crit Reduction", String.Format("{0:F}%*Spell Crit Reduction: {0:F}%\nPhysical Crit Reduction: {1:F}%\nCrit Damage Reduction: {2:F}%", SpellCritReduction * 100, PhysicalCritReduction * 100, CritDamageReduction * 100));
            dictValues.Add("Dodge", String.Format("{0:F}%", 100 * Dodge));

            return dictValues;
        }
    }
}
