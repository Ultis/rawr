using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Xml.Serialization;

namespace Rawr.Warlock
{
    public class Rotation
    {
        public string Name = "Dummy";
        public string Filler = "Shadow Bolt";
        public string Execute = "";
        public List<string> SpellPriority = new List<string>();

        public Rotation() { } // here for XML deserialization
        public Rotation(string name, string filler, string execute, params string[] priorities)
        {
            Name = name;
            Filler = filler;
            Execute = execute;
            SpellPriority = new List<string>(priorities);
        }

        public string GetError()
        {
            bool foundCurse = false;
            foreach (string spell in SpellPriority)
            {
                if (!Spell.ALL_SPELLS.Contains(spell))
                {
                    return spell + " can no longer be prioritized.";
                }

                if (spell.StartsWith("Bane"))
                {
                    if (foundCurse)
                    {
                        return "You may only include one bane.";
                    }
                    foundCurse = true;
                }
            }

            int immo = SpellPriority.IndexOf("Immolate");
            int conf = SpellPriority.IndexOf("Conflagrate");
            if (conf >= 0 && conf < immo)
            {
                return "Conflagrate may only appear after Immolate.";
            }

            if (immo >= 0 && SpellPriority.Contains("Unstable Affliction"))
            {
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
        public List<string> GetPrioritiesForCalcs(WarlockTalents talents, bool execute)
        {
            List<string> forCalcs = new List<string>(SpellPriority);
            if (talents.Backdraft > 0 && !SpellPriority.Contains("Incinerate (Under Backdraft)"))
            {
                forCalcs.Insert(forCalcs.Count, "Incinerate (Under Backdraft)");
            }
            if (!execute
                && Filler.Equals("Shadow Bolt")
                && !forCalcs.Contains("Shadow Bolt (Instant)")
                && ShadowBolt_Instant.IsCastable(talents, forCalcs))
            {
                forCalcs.Insert(forCalcs.Count, "Shadow Bolt (Instant)");
            }
            return forCalcs;
        }

        public bool Contains(string spellname)
        {
            return SpellPriority.Contains(spellname) 
                    || 
                   Filler.Equals(spellname);
        }
    }

    /// <summary>
    /// Holds the options the user selects from the options tab.
    /// </summary>
    public class CalculationOptionsWarlock : ICalculationOptionBase, INotifyPropertyChanged
    {
        public CalculationOptionsWarlock() { }

        public static void AddDefaultRotations(List<Rotation> rotations) 
        {
            rotations.Add(
                new Rotation(
                    "Affliction", //name
                    "Shadow Bolt", //filler
                    "Drain Soul", //execute
                    "Haunt",
                //"Soulburn",
                //"Soul Fire (To Maintain Buff)",
                    "Bane Of Agony",
                    "Corruption",
                    "Unstable Affliction"//,
                   // "Summon Infernal"
                    ));

            rotations.Add(
                new Rotation(
                    "Demonology", //name
                    "Shadow Bolt", //filler
                    "Shadow Bolt", //execute
                // Hand Of Gul'dan (Under Immolate)
                // Immolate (Under Improved Soul Fire)
                //"Soulburn",
                //"Soul Fire (To Maintain Buff)",
                    "Bane Of Doom",
                    "Immolate",
                    "Immolation Aura",
                    "Corruption",
                    //"Shadowflame",
                    "Hand Of Gul'dan",
                    "Incinerate (Under Molten Core)"//,
                // Soulfire (Under Decimation)
                 //   "Summon Infernal"
                    ));

            rotations.Add(
                new Rotation(
                    "Destruction", //name
                    "Incinerate", //filler
                    "Incinerate", //execute
                //"Soulburn",
                //"Soul Fire (To Maintain Buff)",
                    "Bane Of Doom",
                    "Immolate",
                    "Conflagrate",
                    "Corruption",
                    //"Shadowflame",
                    "Chaos Bolt"//,
                // Soulfire (Under Empowered Imp)
                // Soulfire (Under Soulburn)
                 //   "Summon Infernal"
                    ));
        }

        private static readonly int[] hitRatesByLevelDifference = { 100 - 4, 100 - 5, 100 - 6, 100 - 17, 100 - 28, 100 - 39 };

        private string _Pet = "None";
        private int _PlayerLevel = 85;
        private int _TargetLevel = 88;
        private float _Duration = 300f;
        private float _Latency = 0.100f;
        private float _ThirtyFive = 0.25f; // default for backward compatibility
        private float _TwentyFive = 0.15f; // default for backward compatibility
        private List<Rotation> _Rotations = new List<Rotation>();
        private int _ActiveRotationIndex = 0;
        private float _PerSP = 1;
        private bool _ConvertTotem = false;
        private float _PerFlametongue = 500f; // default for back. compat.
        private float _PerMagicBuff = 0;
        private float _PerCritBuff = 0;
        private float _PerInt = 0;
        private float _PerSpi = 0;
        private float _PerHealth = 0;
        private bool _NoProcs = false;
        private bool _PTRMode = false;

        public string Pet { get { return _Pet; } set { _Pet = value; OnPropertyChanged("Pet"); } }
        public int PlayerLevel { get { return _PlayerLevel; } set { _PlayerLevel = value; OnPropertyChanged("PlayerLevel"); } }
        public int TargetLevel { get { return _TargetLevel; } set { _TargetLevel = value; OnPropertyChanged("TargetLevel"); } }
        public float Duration { get { return _Duration; } set { _Duration = value; OnPropertyChanged("Duration"); } }
        public float Latency { get { return _Latency; } set { _Latency = value; OnPropertyChanged("Latency"); } }
        public float ThirtyFive { get { return _ThirtyFive; } set { _ThirtyFive = value; OnPropertyChanged("ThirtyFive"); } }
        public float TwentyFive { get { return _TwentyFive; } set { _TwentyFive = value; OnPropertyChanged("TwentyFive"); } }
        public List<Rotation> Rotations { get { return _Rotations; } set { _Rotations = value; OnPropertyChanged("Rotations"); } }
        public int ActiveRotationIndex { get { return _ActiveRotationIndex; } set { _ActiveRotationIndex = value; OnPropertyChanged("ActiveRotationIndex"); } }
        public float PerSP { get { return _PerSP; } set { _PerSP = value; OnPropertyChanged("PerSP"); } }
        public bool ConvertTotem { get { return _ConvertTotem; } set { _ConvertTotem = value; OnPropertyChanged("ConvertTotem"); } }
        public float PerFlametongue { get { return _PerFlametongue; } set { _PerFlametongue = value; OnPropertyChanged("PerFlametongue"); } }
        public float PerMagicBuff { get { return _PerMagicBuff; } set { _PerMagicBuff = value; OnPropertyChanged("PerMagicBuff"); } }
        public float PerCritBuff { get { return _PerCritBuff; } set { _PerCritBuff = value; OnPropertyChanged("PerCritBuff"); } }
        public float PerInt { get { return _PerInt; } set { _PerInt = value; OnPropertyChanged("PerInt"); } }
        public float PerSpi { get { return _PerSpi; } set { _PerSpi = value; OnPropertyChanged("PerSpi"); } }
        public float PerHealth { get { return _PerHealth; } set { _PerHealth = value; OnPropertyChanged("PerHealth"); } }
        public bool NoProcs { get { return _NoProcs; } set { _NoProcs = value; OnPropertyChanged("NoProcs"); } }
        public bool PTRMode { get { return _PTRMode; } set { _PTRMode = value; OnPropertyChanged("PTRMode"); } }

        #region Stat Graph
        private bool[] _statsList = new bool[] { true, true, true, true, true, true, true, true, true, true, true };
        public bool[] StatsList
        {
            get { return _statsList; }
            set { _statsList = value; OnPropertyChanged("StatsList"); }
        }
        private int _StatsIncrement = 100;
        public int StatsIncrement
        {
            get { return _StatsIncrement; }
            set { _StatsIncrement = value; OnPropertyChanged("StatsIncrement"); }
        }
        private string _calculationToGraph = "DPS";
        public string CalculationToGraph
        {
            get { return _calculationToGraph; }
            set { _calculationToGraph = value; OnPropertyChanged("CalculationToGraph"); }
        }
        [XmlIgnore]
        public bool SG_Int { get { return StatsList[0]; } set { StatsList[0] = value; OnPropertyChanged("SG_INT"); } }
        [XmlIgnore]
        public bool SG_Spi { get { return StatsList[1]; } set { StatsList[1] = value; OnPropertyChanged("SG_SPI"); } }
        [XmlIgnore]
        public bool SG_SP { get { return StatsList[2]; } set { StatsList[2] = value; OnPropertyChanged("SG_SP"); } }
        [XmlIgnore]
        public bool SG_Crit { get { return StatsList[3]; } set { StatsList[3] = value; OnPropertyChanged("SG_Crit"); } }
        [XmlIgnore]
        public bool SG_Hit { get { return StatsList[4]; } set { StatsList[4] = value; OnPropertyChanged("SG_Hit"); } }
        [XmlIgnore]
        public bool SG_SpP { get { return StatsList[5]; } set { StatsList[5] = value; OnPropertyChanged("SG_SpP"); } }
        [XmlIgnore]
        public bool SG_Haste { get { return StatsList[6]; } set { StatsList[6] = value; OnPropertyChanged("SG_Haste"); } }
        [XmlIgnore]
        public bool SG_Mstr { get { return StatsList[7]; } set { StatsList[7] = value; OnPropertyChanged("SG_Mstr"); } }
        #endregion

        public Rotation GetActiveRotation()
        {
            return (Rotations.Count > 0 ? Rotations[ActiveRotationIndex] : new Rotation());
        }
        public void RemoveActiveRotation()
        {
            Rotations.RemoveAt(ActiveRotationIndex);
            ActiveRotationIndex = Math.Min(ActiveRotationIndex, Rotations.Count - 1);
        }
        public int GetBaseHitRate()
        {
            int delta = TargetLevel - PlayerLevel;
            if (delta < 0) delta = 0;
            if (delta > 5) delta = 5;
            return hitRatesByLevelDifference[delta];
        }
        public string GetXml()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(CalculationOptionsWarlock));
            StringBuilder xml = new StringBuilder();
            System.IO.StringWriter writer = new System.IO.StringWriter(xml, System.Globalization.CultureInfo.InvariantCulture);
            serializer.Serialize(writer, this);
            return xml.ToString();
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs(propertyName)); }
        }
    }
}
