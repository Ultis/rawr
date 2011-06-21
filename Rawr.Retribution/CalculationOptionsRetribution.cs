using System.ComponentModel;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace Rawr.Retribution
{
    public class CalculationOptionsRetribution : ICalculationOptionBase, INotifyPropertyChanged
    {
        [DefaultValue(SealOf.Truth)]
        public SealOf Seal { get { return seal; } set { seal = value; OnPropertyChanged("Seal"); } }
        private SealOf seal = SealOf.Truth;

        [DefaultValue(4f)]
        public float InqRefresh { get { return inqRefresh; } set { inqRefresh = value; OnPropertyChanged("InqRefresh"); } }
        private float inqRefresh = 4f;

        [DefaultValue(0.4f)]
        public float SkipToCrusader { get { return skipToCrusader; } set { skipToCrusader = value; OnPropertyChanged("SkipToCrusader"); } }
        private float skipToCrusader = 0.4f;

        [DefaultValue(3)]
        public int HPperInq { get { return hPperInq; } set { hPperInq = value; OnPropertyChanged("HPperInq"); } }
        private int hPperInq = 3;

        [DefaultValue(false)]
        public bool PTR_Mode { get { return ptr_Mode; } set { ptr_Mode = value; OnPropertyChanged("PTR_Mode"); } }
        private bool ptr_Mode = false;
        
        #region Stat Graph
        [DefaultValue(new bool[] { true, true, true, true, true, /**/ true, true, true, })]
        public bool[] StatsList { get { return _statsList; } set { _statsList = value; OnPropertyChanged("StatsList"); } }
        private bool[] _statsList = new bool[] { true, true, true, true, true, /**/ true, true, true, };
        [DefaultValue(100)]
        public int StatsIncrement { get { return _StatsIncrement; } set { _StatsIncrement = value; OnPropertyChanged("StatsIncrement"); } }
        private int _StatsIncrement = 100;
        [DefaultValue("Overall Rating")]
        public string CalculationToGraph { get { return _calculationToGraph; } }
        private string _calculationToGraph = "Overall Rating";
        [XmlIgnore]
        public bool SG_Str { get { return StatsList[0]; } set { StatsList[0] = value; OnPropertyChanged("SG_STR"); } }
        [XmlIgnore]
        public bool SG_Agi { get { return StatsList[1]; } set { StatsList[1] = value; OnPropertyChanged("SG_AGI"); } }
        [XmlIgnore]
        public bool SG_AP { get { return StatsList[2]; } set { StatsList[2] = value; OnPropertyChanged("SG_AP"); } }
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
        #endregion

        public CalculationOptionsRetribution Clone()
        {
            CalculationOptionsRetribution clone = new CalculationOptionsRetribution();
            // Tab - Fight Parameters
            clone.Seal = Seal;
            clone.InqRefresh = InqRefresh;
            clone.SkipToCrusader = SkipToCrusader;
            clone.HPperInq = HPperInq;
            return clone;
        }
        #region ICalculationOptionBase Members
        /// <summary>
        /// Gets the XML serialization of the calculation options for use in the character file.
        /// </summary>
        /// <returns></returns>
        public string GetXml()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(CalculationOptionsRetribution));
            StringBuilder xml = new StringBuilder();
            StringWriter writer = new StringWriter(xml);
            serializer.Serialize(writer, this);
            return xml.ToString();
        }
        #endregion
        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName) {
            if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs(propertyName)); }
        }
        #endregion
    }
}
