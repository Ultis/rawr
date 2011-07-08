using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace Rawr.Moonkin
{
    public class CalculationOptionsMoonkin : ICalculationOptionBase, INotifyPropertyChanged
    {
        public bool Notify = true;

        [DefaultValue(0.100f)]
        public float Latency { get { return latency; } set { latency = value; OnPropertyChanged("Latency"); } }
        private float latency = 0.100f;
        [XmlIgnore]
        public float ExternalLatency { get { return latency * 1000f; } set { latency = value / 1000f; OnPropertyChanged("Latency"); } }

        [DefaultValue(false)]
        public bool Innervate { get { return innervate; } set { innervate = value; OnPropertyChanged("Innervate"); } }
        private bool innervate = false;

        [DefaultValue(1)]
        public float InnervateDelay { get { return innervateDelay; } set { innervateDelay = value; OnPropertyChanged("InnervateDelay"); } }
        private float innervateDelay = 1;

        [DefaultValue(1.00f)]
        public float ReplenishmentUptime { get { return replenishmentUptime; } set { replenishmentUptime = value; OnPropertyChanged("ReplenishmentUptime"); } }
        private float replenishmentUptime = 1.00f;

        [DefaultValue(1.00f)]
        public float TreantLifespan { get { return treantLifespan; } set { treantLifespan = value; OnPropertyChanged("TreantLifespan"); } }
        private float treantLifespan = 1.00f;

        [DefaultValue("None")]
        public string UserRotation { get { return _userRotation; } set { _userRotation = value; OnPropertyChanged("UserRotation"); } }
        private string _userRotation = "None";

        [XmlIgnore]
        public List<string> ReforgePriorityList { get { return _reforgePriorityList; } }
        private List<string> _reforgePriorityList = new List<string> { "Spirit over Hit", "Hit over Spirit" };

        [DefaultValue(0)]
        public int ReforgePriority { get { return _reforgePriority; } set { _reforgePriority = value; OnPropertyChanged("ReforgePriority"); } }
        private int _reforgePriority = 0;

        [DefaultValue(false)]
        public bool AllowReforgingSpiritToHit { get { return _allowReforgingSpiritToHit; } set { _allowReforgingSpiritToHit = value; OnPropertyChanged("AllowReforgingSpiritToHit"); } }
        private bool _allowReforgingSpiritToHit = false;

        [DefaultValue(false)]
        public bool PTRMode { get { return ptrMode; } set { ptrMode = value; OnPropertyChanged("PTRMode"); } }
        private bool ptrMode = false;

        #region Stat Graph
        [DefaultValue(new bool[] { true, true, true, true, true, /**/ true, true, })]
        public bool[] StatsList { get { return _statsList; } set { _statsList = value; OnPropertyChanged("StatsList"); } }
        private bool[] _statsList = new bool[] { true, true, true, true, true, /**/ true, true, };
        [DefaultValue(100)]
        public int StatsIncrement { get { return _StatsIncrement; } set { _StatsIncrement = value; OnPropertyChanged("StatsIncrement"); } }
        private int _StatsIncrement = 100;
        [DefaultValue("Overall Rating")]
        public string CalculationToGraph { get { return _calculationToGraph; } set { _calculationToGraph = value; OnPropertyChanged("CalculationToGraph"); } }
        private string _calculationToGraph = "Overall Rating";
        [XmlIgnore]
        public bool SG_Int { get { return StatsList[0]; } set { StatsList[0] = value; OnPropertyChanged("SG_Int"); } }
        [XmlIgnore]
        public bool SG_Spi { get { return StatsList[1]; } set { StatsList[1] = value; OnPropertyChanged("SG_SPI"); } }
        [XmlIgnore]
        public bool SG_SP { get { return StatsList[2]; } set { StatsList[2] = value; OnPropertyChanged("SG_SP"); } }
        [XmlIgnore]
        public bool SG_Crit { get { return StatsList[3]; } set { StatsList[3] = value; OnPropertyChanged("SG_Crit"); } }
        [XmlIgnore]
        public bool SG_Hit { get { return StatsList[4]; } set { StatsList[4] = value; OnPropertyChanged("SG_Hit"); } }
        [XmlIgnore]
        public bool SG_Haste { get { return StatsList[6]; } set { StatsList[6] = value; OnPropertyChanged("SG_Haste"); } }
        [XmlIgnore]
        public bool SG_Mstr { get { return StatsList[7]; } set { StatsList[7] = value; OnPropertyChanged("SG_Mstr"); } }
        #endregion

        #region ICalculationOptionBase Overrides
        public string GetXml()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(CalculationOptionsMoonkin));
            StringBuilder xml = new StringBuilder();
            StringWriter writer = new StringWriter(xml);
            serializer.Serialize(writer, this);
            return xml.ToString();
        }
        #endregion
        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null && Notify) PropertyChanged(this, new PropertyChangedEventArgs(property));
        }
        #endregion
    }
}
