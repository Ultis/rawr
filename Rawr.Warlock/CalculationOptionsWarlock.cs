using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace Rawr.Warlock
{
    /// <summary>
    /// Holds the options the user selects from the options tab.
    /// </summary>
    public class CalculationOptionsWarlock : ICalculationOptionBase, INotifyPropertyChanged
    {
        #region Obsolete (should be removed)
        /// <summary>
        /// The level of the player. This should not be stored here and instead read from the Boss Handler
        /// </summary>
        [Obsolete("Use character.Level when you need the player level")]
        [DefaultValue(85)]
        public int PlayerLevel { get { return _PlayerLevel; } set { _PlayerLevel = value; OnPropertyChanged("PlayerLevel"); } }
        private int _PlayerLevel = 85;

        /// <summary>
        /// The level of the target. This should not be stored here and instead read from the Boss Handler
        /// </summary>
        [Obsolete("Use character.BossOptions.Level when you need the target level")]
        [DefaultValue(88)]
        public int TargetLevel { get { return _TargetLevel; } set { _TargetLevel = value; OnPropertyChanged("TargetLevel"); } }
        private int _TargetLevel = 88;

        [Obsolete("This should be a Boss Handler variable")]
        [DefaultValue(0.25f)]
        public float ThirtyFive { get { return _ThirtyFive; } set { _ThirtyFive = value; OnPropertyChanged("ThirtyFive"); } }
        private float _ThirtyFive = 0.25f; // default for backward compatibility

        [Obsolete("This should be a Boss Handler variable")]
        [DefaultValue(0.15f)]
        public float TwentyFive { get { return _TwentyFive; } set { _TwentyFive = value; OnPropertyChanged("TwentyFive"); } }
        private float _TwentyFive = 0.15f; // default for backward compatibility
        #endregion

        /// <summary>
        /// Which Pet to use
        /// </summary>
        /// <remarks>
        /// This should be replaced with an int identifier for better performance.
        /// <para>Use a Convertor to translate the int to the string selection Options Pane or change the binding to SelectedIndex from SelectedItem</para>
        /// </remarks>
        [Obsolete("This should be replaced with an int identifier for better performance.")]
        [DefaultValue("None")]
        public string Pet { get { return _Pet; } set { _Pet = value; OnPropertyChanged("Pet"); } }
        private string _Pet = "None";

        /// <summary>
        /// The amount of time it takes the WoW Client to communicate with the WoW Server. Can be seen on the main options bar in WoW client.
        /// <para>Users are not penalized for 1/4 of a second or less (250ms)</para>
        /// </summary>
        [DefaultValue(0.100f)]
        public float Latency { get { return _Latency; } set { _Latency = value; OnPropertyChanged("Latency"); } }
        private float _Latency = 0.100f;

        /// <summary>
        /// The default and custom Rotations to be stored in the character file
        /// </summary>
        public List<Rotation> Rotations { get { return _Rotations; } set { _Rotations = value; OnPropertyChanged("Rotations"); } }
        private List<Rotation> _Rotations = new List<Rotation>();

        /// <summary>
        /// The Active rotation in the Rotations list
        /// </summary>
        [DefaultValue(0)]
        public int ActiveRotationIndex { get { return _ActiveRotationIndex; } set { _ActiveRotationIndex = value; OnPropertyChanged("ActiveRotationIndex"); } }
        private int _ActiveRotationIndex = 0;

        [DefaultValue(1)]
        public float PerSP { get { return _PerSP; } set { _PerSP = value; OnPropertyChanged("PerSP"); } }
        private float _PerSP = 1;

        [DefaultValue(false)]
        public bool ConvertTotem { get { return _ConvertTotem; } set { _ConvertTotem = value; OnPropertyChanged("ConvertTotem"); } }
        private bool _ConvertTotem = false;

        [DefaultValue(500f)]
        public float PerFlametongue { get { return _PerFlametongue; } set { _PerFlametongue = value; OnPropertyChanged("PerFlametongue"); } }
        private float _PerFlametongue = 500f; // default for backward compatibility

        [DefaultValue(0)]
        public float PerMagicBuff { get { return _PerMagicBuff; } set { _PerMagicBuff = value; OnPropertyChanged("PerMagicBuff"); } }
        private float _PerMagicBuff = 0;

        [DefaultValue(0)]
        public float PerCritBuff { get { return _PerCritBuff; } set { _PerCritBuff = value; OnPropertyChanged("PerCritBuff"); } }
        private float _PerCritBuff = 0;

        [DefaultValue(0)]
        public float PerInt { get { return _PerInt; } set { _PerInt = value; OnPropertyChanged("PerInt"); } }
        private float _PerInt = 0;

        [DefaultValue(0)]
        public float PerSpi { get { return _PerSpi; } set { _PerSpi = value; OnPropertyChanged("PerSpi"); } }
        private float _PerSpi = 0;

        [DefaultValue(0)]
        public float PerHealth { get { return _PerHealth; } set { _PerHealth = value; OnPropertyChanged("PerHealth"); } }
        private float _PerHealth = 0;

        [DefaultValue(false)]
        public bool NoProcs { get { return _NoProcs; } set { _NoProcs = value; OnPropertyChanged("NoProcs"); } }
        private bool _NoProcs = false;

        [DefaultValue(false)]
        public bool PTRMode { get { return _PTRMode; } set { _PTRMode = value; OnPropertyChanged("PTRMode"); } }
        private bool _PTRMode = false;

        #region Stat Graph
        public bool[] StatsList
        {
            get { return _statsList; }
            set { _statsList = value; OnPropertyChanged("StatsList"); }
        }
        private bool[] _statsList = new bool[] { true, true, true, true, true, true, true, true, true, true, true };

        [DefaultValue(100)]
        public int StatsIncrement
        {
            get { return _StatsIncrement; }
            set { _StatsIncrement = value; OnPropertyChanged("StatsIncrement"); }
        }
        private int _StatsIncrement = 100;

        [DefaultValue("DPS")]
        public string CalculationToGraph
        {
            get { return _calculationToGraph; }
            set { _calculationToGraph = value; OnPropertyChanged("CalculationToGraph"); }
        }
        private string _calculationToGraph = "DPS";

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

        #region Model Specific Functions
        // Rotations
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
        // Hit Rates
        private static readonly int[] hitRatesByLevelDifference = { 100 - 4, 100 - 5, 100 - 6, 100 - 17, 100 - 28, 100 - 39 };
        #endregion

        #region ICalculationOptionBase Overrides
        public string GetXml() {
            XmlSerializer serializer = new XmlSerializer(typeof(CalculationOptionsWarlock));
            StringBuilder xml = new StringBuilder();
            StringWriter writer = new StringWriter(xml, CultureInfo.InvariantCulture);
            serializer.Serialize(writer, this);
            return xml.ToString();
        }
        #endregion
        #region INotifyPropertyChanged Overrides
        private void OnPropertyChanged(string propertyName) { if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs(propertyName)); } }
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
    }
}
