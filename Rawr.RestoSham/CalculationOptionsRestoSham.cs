using System.ComponentModel;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace Rawr.RestoSham
{
    /// <summary>
    /// Holds the options the user selects from the options tab.
    /// </summary>
    public class CalculationOptionsRestoSham : ICalculationOptionBase, INotifyPropertyChanged
    {
        /// <summary>Count of Innervates you get</summary>
        [DefaultValue(0f)]
        public float Innervates { get { return _Innervates; } set { _Innervates = value; OnPropertyChanged("Innervates"); } }
        private float _Innervates = 0f;

        /// <summary>Whether we keep Water Shield up or not (could use Earth Shield during some fights</summary>
        [DefaultValue(true)]
        public bool WaterShield { get { return _WaterShield; } set { _WaterShield = value; OnPropertyChanged("WaterShield"); } }
        private bool _WaterShield = true;

        /// <summary>Use Earth shield</summary>
        [DefaultValue(true)]
        public bool EarthShield { get { return _EarthShield; } set { _EarthShield = value; OnPropertyChanged("EarthShield"); } }
        private bool _EarthShield = true;

        /// <summary>Your style of healing</summary>
        [DefaultValue("RT+HW")]
        public string BurstStyle { get { return _BurstStyle; } set { _BurstStyle = value; OnPropertyChanged("BurstStyle"); } }
        private string _BurstStyle = "RT+HW";

        /// <summary>Your style of healing</summary>
        [DefaultValue("RT+CH")]
        public string SustStyle { get { return _SustStyle; } set { _SustStyle = value; OnPropertyChanged("SustStyle"); } }
        private string _SustStyle = "RT+CH";

        /// <summary>Targets</summary>
        [DefaultValue("Raid")]
        public string Targets { get { return _Targets; } set { _Targets = value; OnPropertyChanged("Targets"); } }
        private string _Targets = "Raid";

        /// <summary>The number of times you use Cleanse Spirit.</summary>
        [DefaultValue(0f)]
        public float Decurse { get { return _Decurse; } set { _Decurse = value; OnPropertyChanged("Decurse"); } }
        private float _Decurse = 0f;

        /// <summary>The latency in millisecond</summary>
        [DefaultValue(60f)]
        public float Latency { get { return _Latency; } set { _Latency = value; OnPropertyChanged("Latency"); } }
        private float _Latency = 60f;

        /// <summary>The percentage of usefulness survival is to you</summary>
        [DefaultValue(2f)]
        public float SurvivalPerc { get { return _SurvivalPerc; } set { _SurvivalPerc = value; OnPropertyChanged("SurvivalPerc"); } }
        private float _SurvivalPerc = 2f;

        /// <summary>The percentage of time you are active</summary>
        [DefaultValue(85f)]
        public float ActivityPerc { get { return _ActivityPerc; } set { _ActivityPerc = value; OnPropertyChanged("ActivityPerc"); } }
        private float _ActivityPerc = 85f;

        /// <summary>The number of times water shield pops per minute</summary>
        [DefaultValue(0f)]
        public float WSPops { get { return _WSPops; } set { _WSPops = value; OnPropertyChanged("WSPops"); } }
        private float _WSPops = 0f;

        /// <summary>The number of times water shield pops per minute</summary>
        [DefaultValue(0f)]
        public float LBUse { get { return _LBUse; } set { _LBUse = value; OnPropertyChanged("LBUse"); } }
        private float _LBUse = 0f;

        #region Stat Graph
        [DefaultValue(new bool[] { true, true, true, true, true, /**/ true, true, true, true, })]
        public bool[] StatsList { get { return _statsList; } set { _statsList = value; OnPropertyChanged("StatsList"); } }
        private bool[] _statsList = new bool[] { true, true, true, true, true, /**/ true, true, true, true, };
        [DefaultValue(100)]
        public int StatsIncrement { get { return _StatsIncrement; } set { _StatsIncrement = value; OnPropertyChanged("StatsIncrement"); } }
        private int _StatsIncrement = 100;
        [DefaultValue("Overall Rating")]
        public string CalculationToGraph { get { return _calculationToGraph; } set { _calculationToGraph = value; OnPropertyChanged("CalculationToGraph"); } }
        private string _calculationToGraph = "Overall Rating";
        [XmlIgnore]
        public bool SG_Int { get { return StatsList[0]; } set { StatsList[0] = value; OnPropertyChanged("SG_INT"); } }
        [XmlIgnore]
        public bool SG_Spi { get { return StatsList[1]; } set { StatsList[1] = value; OnPropertyChanged("SG_Spi"); } }
        [XmlIgnore]
        public bool SG_SP { get { return StatsList[2]; } set { StatsList[2] = value; OnPropertyChanged("SG_SP"); } }
        [XmlIgnore]
        public bool SG_Crit { get { return StatsList[3]; } set { StatsList[3] = value; OnPropertyChanged("SG_Crit"); } }
        [XmlIgnore]
        public bool SG_Hit { get { return StatsList[4]; } set { StatsList[4] = value; OnPropertyChanged("SG_Hit"); } }
        [XmlIgnore]
        public bool SG_Exp { get { return StatsList[5]; } set { StatsList[5] = value; OnPropertyChanged("SG_Exp"); } }
        [XmlIgnore]
        public bool SG_Haste { get { return StatsList[6]; } set { StatsList[6] = value; OnPropertyChanged("SG_Haste"); } }
        [XmlIgnore]
        public bool SG_Mstr { get { return StatsList[7]; } set { StatsList[7] = value; OnPropertyChanged("SG_Mstr"); } }
        [XmlIgnore]
        public bool SG_SpPen { get { return StatsList[8]; } set { StatsList[8] = value; OnPropertyChanged("SG_SpPen"); } }
        #endregion

        #region ICalculationOptionBase overrides
        /// <summary>
        /// Gets the XML serialization of the calculation options for use in the character file.
        /// </summary>
        /// <returns></returns>
        public string GetXml()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(CalculationOptionsRestoSham));
            StringBuilder xml = new StringBuilder();
            StringWriter writer = new StringWriter(xml);
            serializer.Serialize(writer, this);
            return xml.ToString();
        }
        #endregion
        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs(propertyName)); }
        }
        #endregion
    }
}
