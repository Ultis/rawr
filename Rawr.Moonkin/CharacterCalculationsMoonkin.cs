using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rawr.Moonkin
{
    class CharacterCalculationsMoonkin : CharacterCalculationsBase
    {
        public CharacterCalculationsMoonkin()
        {
            Rotations = new List<Rotation>(new Rotation[] {
                new Rotation() { Name = "SF Spam", DPM = 0.0f, DPS = 0.0f, TimeToOOM = new TimeSpan(0, 0, 0) },
                new Rotation() { Name = "W Spam", DPM = 0.0f, DPS = 0.0f, TimeToOOM = new TimeSpan(0, 0, 0) },
                new Rotation() { Name = "MF/SFx4", DPM = 0.0f, DPS = 0.0f, TimeToOOM = new TimeSpan(0, 0, 0) },
                new Rotation() { Name = "MF/SFx3/W", DPM = 0.0f, DPS = 0.0f, TimeToOOM = new TimeSpan(0, 0, 0) },
                new Rotation() { Name = "MF/Wx8", DPM = 0.0f, DPS = 0.0f, TimeToOOM = new TimeSpan(0, 0, 0) },
                new Rotation() { Name = "IS/MF/SFx3", DPM = 0.0f, DPS = 0.0f, TimeToOOM = new TimeSpan(0, 0, 0) },
                new Rotation() { Name = "IS/MF/Wx7", DPM = 0.0f, DPS = 0.0f, TimeToOOM = new TimeSpan(0, 0, 0) },
                new Rotation() { Name = "IS/SFx3/W", DPM = 0.0f, DPS = 0.0f, TimeToOOM = new TimeSpan(0, 0, 0) },
                new Rotation() { Name = "IS/SFx4", DPM = 0.0f, DPS = 0.0f, TimeToOOM = new TimeSpan(0, 0, 0) },
                new Rotation() { Name = "IS/Wx8", DPM = 0.0f, DPS = 0.0f, TimeToOOM = new TimeSpan(0, 0, 0) }
            });
        }
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

        private float[] subPoints = new float[] { 0f };

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
        public float FightLength { get; set; }
        public float DamageDone { get; set; }
        public Rotation SelectedRotation { get; set; }
        public string RotationName
        {
            get
            {
                return SelectedRotation.Name;
            }
            set
            {
                foreach (Rotation r in Rotations)
                {
                    if (r.Name == value)
                        SelectedRotation = r;
                }
            }
        }
        public List<Rotation> Rotations { get; set; }

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
            retVal.Add("O5SR Per Second", String.Format("{0:F}", ManaRegen));
            retVal.Add("I5SR Per Second", String.Format("{0:F}", ManaRegen5SR));
            retVal.Add("Selected Rotation", RotationName);
            foreach (Rotation r in Rotations)
            {
                retVal.Add(r.Name + " DPS", String.Format("{0:F}", r.DPS));
                retVal.Add(r.Name + " DPM", String.Format("{0:F}", r.DPM));
                retVal.Add(r.Name + " OOM", String.Format(r.TimeToOOM > new TimeSpan(0, 0, 0) ? "{0} m {1} s" : "Not during fight", r.TimeToOOM.Minutes, r.TimeToOOM.Seconds));
            }

            return retVal;
        }
    }
}
