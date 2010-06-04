using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Rawr.Warlock {

    public class Rotation {

        public string Name;
        public string Filler;
        public string Execute;
        public List<string> SpellPriority;

        public Rotation() {

            // here for XML deserialization
        }

        public Rotation(
            string name,
            string filler,
            string execute,
            params string[] priorities) {

            Name = name;
            Filler = filler;
            Execute = execute;
            SpellPriority = new List<string>(priorities);
        }

        public string GetError() {

            bool foundCurse = false;
            foreach (string spell in SpellPriority) {
                if (!Spell.ALL_SPELLS.Contains(spell)) {
                    return spell + " can no longer be prioritized.";
                }
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

            if (immo >= 0 && SpellPriority.Contains("Unstable Affliction")) {
                return "Unstable Affliction and Immolate don't mix.";
            }

            return null;
        }

        /// <summary>
        /// Gets a modified version of the user's spell priorities, for internal
        /// purposes.
        /// </summary>
        /// <param name="spellPriority"></param>
        /// <returns></returns>
        public List<string> GetPrioritiesForCalcs(
            WarlockTalents talents, bool execute) {

            List<string> forCalcs = new List<string>(SpellPriority);
            if (talents.Backdraft > 0
                && !SpellPriority.Contains("Incinerate (Under Backdraft)")) {

                forCalcs.Insert(
                    forCalcs.Count, "Incinerate (Under Backdraft)");
            }
            if (!execute
                && Filler.Equals("Shadow Bolt")
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

        public static CalculationOptionsWarlock MakeDefaultOptions() {

            CalculationOptionsWarlock options = new CalculationOptionsWarlock();
            options.Pet = "None";
            options.TargetLevel = 83;
            options.Duration = 300;
            options.Latency = .1f;
            options.Rotations = new List<Rotation>();

            options.Rotations.Add(
                new Rotation(
                    "Affliction",
                    "Shadow Bolt",
                    "Drain Soul",
                    "Haunt",
                    "Corruption",
                    "Unstable Affliction",
                    "Curse Of Agony"));
            options.Rotations.Add(
                new Rotation(
                    "Demonology",
                    "Shadow Bolt",
                    "Soul Fire",
                    "Immolation Aura",
                    "Corruption",
                    "Immolate",
                    "Incinerate (Under Molten Core)",
                    "Curse Of Agony"));
            options.Rotations.Add(
                new Rotation(
                    "Destruction",
                    "Incinerate",
                    null,
                    "Immolate",
                    "Conflagrate",
                    "Incinerate (Under Backdraft)",
                    "Chaos Bolt",
                    "Curse Of Doom"));

            return options;
        }


        #region constants

        private static readonly int[] hitRatesByLevelDifference 
            = { 100 - 4, 100 - 5, 100 - 6, 100 - 17, 100 - 28, 100 - 39 };

        #endregion


        #region properties

        public string Pet;
        public string Imbue = "Grand Spellstone"; // default it here for backward compatibility w/ files from before it was added
        public bool UseInfernal;
        public int TargetLevel;
        public float Duration;
        public float Latency;
        public float ThirtyFive = .25f; // default for backward compatibility
        public float TwentyFive = .15f; // default for backward compatibility

        public List<Rotation> Rotations;
        public int ActiveRotationIndex;

        public bool NoProcs;

        #endregion


        #region methods

        public Rotation GetActiveRotation() {

            return Rotations[ActiveRotationIndex];
        }

        public void RemoveActiveRotation() {

            Rotations.RemoveAt(ActiveRotationIndex);
            ActiveRotationIndex
                = Math.Min(ActiveRotationIndex, Rotations.Count - 1);
        }

        public int GetBaseHitRate() {
            if (TargetLevel < 80) TargetLevel = 80;
            else if (TargetLevel > 83) TargetLevel = 83;
            return hitRatesByLevelDifference[TargetLevel - 80];
        }

        public string GetXml() {

            XmlSerializer serializer
                = new XmlSerializer(typeof(CalculationOptionsWarlock));
            StringBuilder xml = new StringBuilder();
            System.IO.StringWriter writer
                = new System.IO.StringWriter(
                    xml, System.Globalization.CultureInfo.InvariantCulture);
            serializer.Serialize(writer, this);
            return xml.ToString();
        }

        #endregion
    }
}
