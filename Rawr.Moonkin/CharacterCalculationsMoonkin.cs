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
        public float Latency { get; set; }
        public int TargetLevel { get; set; }
        public float FightLength { get; set; }
        public bool Scryer { get; set; }
        public SpellRotation SelectedRotation { get; set; }
        public SpellRotation MaxDPSRotation { get; set; }
        public string RotationName
        {
            get
            {
                return SelectedRotation.Name;
            }
        }
        public string DpsRotationName
        {
            get
            {
                return MaxDPSRotation.Name;
            }
        }
        public Dictionary<string, RotationData> Rotations { get; set; }
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
            retVal.Add("Spell Haste", baseStats.HasteRating.ToString());
            retVal.Add("Arcane Damage", ArcaneDamage.ToString());
            retVal.Add("Nature Damage", NatureDamage.ToString());
            retVal.Add("O5SR Per Second", String.Format("{0:F}", ManaRegen));
            retVal.Add("I5SR Per Second", String.Format("{0:F}", ManaRegen5SR));
            retVal.Add("Selected Rotation", RotationName);
            retVal.Add("Max DPS Rotation", DpsRotationName);
            foreach (KeyValuePair<string, RotationData> pair in Rotations)
            {
                RotationData r = pair.Value;
                string name = pair.Key;
                retVal.Add(name + " RDPS", String.Format("{0:F}", r.RawDPS));
                retVal.Add(name + " DPS", String.Format("{0:F}", r.DPS));
                retVal.Add(name + " DPM", String.Format("{0:F}", r.DPM));
                retVal.Add(name + " OOM", String.Format(r.TimeToOOM > new TimeSpan(0, 0, 0) ? "{0} m {1} s" : "Not during fight", r.TimeToOOM.Minutes, r.TimeToOOM.Seconds));
            }

            return retVal;
        }
    }
}
