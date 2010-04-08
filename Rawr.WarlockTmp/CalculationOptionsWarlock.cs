using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Rawr.WarlockTmp {

    public class Rotation {

        public string Name;
        public string Filler;
        public List<string> SpellPriority;

        public Rotation(
            string name, string filler, params string[] priorities) {

            Name = name;
            Filler = filler;
            SpellPriority = new List<string>(priorities);
        }

        public string GetError() {

            bool foundCurse = false;
            foreach (string spell in SpellPriority) {
                if (spell.StartsWith("Curse")) {
                    if (foundCurse) {
                        return "You may only include one curse.";
                    }
                    foundCurse = true;
                }
            }

            int immo = SpellPriority.IndexOf("Immolate");
            int conf = SpellPriority.IndexOf("Conflagrate");
            if (conf >= 0 && conf < immo) {
                return "Conflagrate may only appear after Immolate.";
            }

            return null;
        }

        /// <summary>
        /// Gets a modified version of the user's spell priorities, for internal
        /// purposes.
        /// </summary>
        /// <param name="spellPriority"></param>
        /// <returns></returns>
        public List<string> GetPrioritiesForCalcs(WarlockTalents talents) {

            List<string> forCalcs = new List<string>(SpellPriority);
            if (talents.Backdraft > 0
                && !SpellPriority.Contains("Incinerate (Under Backdraft)")) {

                forCalcs.Insert(
                    forCalcs.Count, "Incinerate (Under Backdraft)");
            }
            if (Filler.Equals("Shadow Bolt")
                && !forCalcs.Contains("Shadow Bolt (Instant)")
                && ShadowBolt_Instant.IsCastable(talents, forCalcs)) {

                forCalcs.Insert(forCalcs.Count, "Shadow Bolt (Instant)");
            }
            return forCalcs;
        }

        public bool Contains(string spellname) {

            return SpellPriority.Contains(spellname)
                || Filler.Equals(spellname);
        }
    }

    /// <summary>
    /// Holds the options the user selects from the options tab.
    /// </summary>
    public class CalculationOptionsWarlock : ICalculationOptionBase {

        #region constants

        private static readonly int[] hitRatesByLevelDifference 
            = { 100 - 4, 100 - 5, 100 - 6, 100 - 17, 100 - 28, 100 - 39 };

        #endregion


        #region properties

        public String Pet;
        public bool UseInfernal;
        public int TargetLevel;
        public float Duration;
        public float Latency;
        public float ManaPotSize;
        public float Replenishment;
        public Dictionary<string, Rotation> Rotations;
        public bool NoProcs;

        private string _activeRotation;
        public Rotation ActiveRotation {
            get {
                return Rotations[_activeRotation];
            }
            set {
                _activeRotation = value.Name;
                Rotations[_activeRotation] = value;
            }
        }

        #endregion


        #region constructors

        public CalculationOptionsWarlock() {

            Pet = "None";
            TargetLevel = 83;
            Duration = 300;
            Rotations = new Dictionary<string, Rotation>();

            ActiveRotation
                = new Rotation(
                    "Affliction",
                    "Shadow Bolt",
                    "Haunt",
                    "Corruption",
                    "Unstable Affliction",
                    "Curse Of Agony");
            ActiveRotation
                = new Rotation(
                    "Demonology",
                    "Shadow Bolt",
                    "Metamorphosis",
                    "Corruption",
                    "Immolate",
                    "Incinerate (Under Molten Core)",
                    "Curse Of Agony");
            ActiveRotation
                = new Rotation(
                    "Destruction",
                    "Incinerate",
                    "Curse Of Doom",
                    "Immolate",
                    "Conflagrate",
                    "Incinerate (Under Backdraft)",
                    "Corruption",
                    "Chaos Bolt");

            _activeRotation = "Affliction";
        }

        #endregion


        #region methods

        public void RemoveActiveRotation() {

            Rotations.Remove(_activeRotation);
            foreach (Rotation rotation in Rotations.Values) {
                ActiveRotation = rotation;
                return;
            }
            _activeRotation = null;
        }

        public int GetBaseHitRate() { 
            
            return hitRatesByLevelDifference[TargetLevel - 80];
        }

        public string GetXml() {

            XmlSerializer serializer = new XmlSerializer(typeof(CalculationOptionsWarlock));
            StringBuilder xml = new StringBuilder();
            System.IO.StringWriter writer = new System.IO.StringWriter(xml, System.Globalization.CultureInfo.InvariantCulture);
            serializer.Serialize(writer, this);
            return xml.ToString();
        }

        #endregion
    }
}
