using System;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace Rawr.Elemental
{
    public class CalculationOptionsElemental : ICalculationOptionBase, INotifyPropertyChanged
    {
        [DefaultValue(75)]
        public int BSRatio { get { return _BSRatio; } set { _BSRatio = value; OnPropertyChanged("BSRatio"); } }
        private int _BSRatio = 75; // goes from 0 to 100

        [DefaultValue(true)]
        public bool UseThunderstorm { get { return _UseThunderstorm; } set { _UseThunderstorm = value; OnPropertyChanged("UseThunderstorm"); } }
        private bool _UseThunderstorm = true;

        [DefaultValue(0)]
        public int RotationType { get { return _rotationType; } set { _rotationType = value; OnPropertyChanged("RotationType"); } }
        private int _rotationType = 0;

        [DefaultValue(0.150f)]
        public float LatencyGcd { get { return _LatencyGcd; } set { _LatencyGcd = value; OnPropertyChanged("LatencyGcd"); } }
        private float _LatencyGcd = 0.150f;

        [DefaultValue(0.075f)]
        public float LatencyCast { get { return _LatencyCast; } set { _LatencyCast = value; OnPropertyChanged("LatencyCast"); } }
        private float _LatencyCast = .075f;

        [Obsolete("This should be pulled from the Boss Handler instead")]
        [DefaultValue(1)]
        public int NumberOfTargets { get { return _NumberOfTargets; } set { _NumberOfTargets = value; OnPropertyChanged("NumberOfTargets"); } }
        private int _NumberOfTargets = 1;

        [DefaultValue(true)]
        public bool UseFireNova { get { return _UseFireNova; } set { _UseFireNova = value; OnPropertyChanged("UseFireNova"); } }
        private bool _UseFireNova = true;

        [DefaultValue(true)]
        public bool UseChainLightning { get { return _UseChainLightning; } set { _UseChainLightning = value; OnPropertyChanged("UseChainLightning"); } }
        private bool _UseChainLightning = true;

        [DefaultValue(false)]
        public bool UseDpsTotem { get { return _UseDpsTotem; } set { _UseDpsTotem = value; OnPropertyChanged("UseDpsTotem"); } }
        private bool _UseDpsTotem = false;

        [DefaultValue(false)]
        public bool UseFireEle { get { return _UseFireEle; } set { _UseFireEle = value; OnPropertyChanged("UseFireEle"); } }
        private bool _UseFireEle = false;

        #region Stat Graph
        [DefaultValue(new bool[] { true, true, true, true, true, true, true, true, true, true, true, true, })]
        public bool[] StatsList { get { return _statsList; } set { _statsList = value; OnPropertyChanged("StatsList"); } }
        private bool[] _statsList = new bool[] { true, true, true, true, true, true, true, true, true, true, true, true, };
        [DefaultValue(100)]
        public int StatsIncrement { get { return _StatsIncrement; } set { _StatsIncrement = value; OnPropertyChanged("StatsIncrement"); } }
        private int _StatsIncrement = 100;
        [DefaultValue("Overall Rating")]
        public string CalculationToGraph { get { return _calculationToGraph; } set { _calculationToGraph = value; OnPropertyChanged("CalculationToGraph"); } }
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
        [XmlIgnore]
        public bool SG_Int { get { return StatsList[8]; } set { StatsList[8] = value; OnPropertyChanged("SG_Int"); } }
        [XmlIgnore]
        public bool SG_Spi { get { return StatsList[9]; } set { StatsList[9] = value; OnPropertyChanged("SG_Spi"); } }
        [XmlIgnore]
        public bool SG_SP { get { return StatsList[10]; } set { StatsList[10] = value; OnPropertyChanged("SG_SP"); } }
        [XmlIgnore]
        public bool SG_SpPen { get { return StatsList[11]; } set { StatsList[11] = value; OnPropertyChanged("SG_SpPen"); } }
        #endregion

        #region ICalculationOptionBase Overrides
        public string GetXml()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(CalculationOptionsElemental));
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
