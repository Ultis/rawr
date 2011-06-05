using System.ComponentModel;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace Rawr.TankDK
{
    /// <summary>
    /// Holds the options the user selects from the options tab.
    /// </summary>
    public class CalculationOptionsTankDK : ICalculationOptionBase, INotifyPropertyChanged
    {
        /// <summary>
        /// Defines the role the TankDK currently has in the Raid<br/>
        /// Possible Values: MT=0, OT=1, TT=2, Any Tank = 3<br/>
        /// Any Tank means anything that would affect 0, 1 or 2
        /// </summary>
        [DefaultValue((int)PLAYER_ROLES.MainTank)]
        public int PlayerRole { get { return _playerRole; } set { _playerRole = value; OnPropertyChanged("PlayerRole"); } }
        private int _playerRole = (int)PLAYER_ROLES.MainTank;

        [DefaultValue(3.5f)]
        public float HitsToSurvive { get { return _HitsToSurvive; } set { _HitsToSurvive = value; OnPropertyChanged("HitsToSurvive"); } }
        private float _HitsToSurvive = 3.5f;
        [DefaultValue(3.0f)]
        public float BurstWeight { get { return _BurstWeight; } set { _BurstWeight = value; OnPropertyChanged("BurstWeight"); } }
        private float _BurstWeight = 3.0f;
        [DefaultValue(1.0f)]
        public float ThreatWeight { get { return _ThreatWeight; } set { _ThreatWeight = value; OnPropertyChanged("ThreatWeight"); } }
        private float _ThreatWeight = 1.0f;
        [DefaultValue(1.0f)]
        public float VengeanceWeight { get { return _VengenceWeight; } set { _VengenceWeight = value; OnPropertyChanged("VengeanceWeight"); } }
        private float _VengenceWeight = 1;

        [DefaultValue(0.25f)]
        public float pOverHealing { get { return _pOverHealing; } set { _pOverHealing = value; OnPropertyChanged("pOverHealing"); } }
        private float _pOverHealing = 0.25f;

        // This one is Unused
        [DefaultValue(false)]
        public bool bExperimental { get { return _m_bExperimental; } set { _m_bExperimental = value; OnPropertyChanged("Experimental"); } }
        private bool _m_bExperimental = false;

        #region Stat Graph
        [DefaultValue(new bool[] { true, true, true, true, true, true, true, true, true, true, true, true, true })]
        public bool[] StatsList { get { return _statsList; } set { _statsList = value; OnPropertyChanged("StatsList"); } }
        private bool[] _statsList = new bool[] { true, true, true, true, true, true, true, true, true, true, true, true, true };
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
        public bool SG_Dodge { get { return StatsList[8]; } set { StatsList[8] = value; OnPropertyChanged("SG_Dodge"); } }
        [XmlIgnore]
        public bool SG_Parry { get { return StatsList[9]; } set { StatsList[9] = value; OnPropertyChanged("SG_Parry"); } }
        [XmlIgnore]
        public bool SG_Armor { get { return StatsList[10]; } set { StatsList[10] = value; OnPropertyChanged("SG_Armor"); } }
        [XmlIgnore]
        public bool SG_BArmor { get { return StatsList[11]; } set { StatsList[11] = value; OnPropertyChanged("SG_BArmor"); } }
        [XmlIgnore]
        public bool SG_Stam { get { return StatsList[12]; } set { StatsList[12] = value; OnPropertyChanged("SG_Stam"); } }
        #endregion

        [XmlIgnore]
        public string szRotReport { get { return _szRotReport; } set { _szRotReport = value.Replace("Dancing Rune Weapon", "Dancing Rune Wp"); OnPropertyChanged("szRotReport"); } }
        private string _szRotReport = "";

        #region ICalculationOptionBase overrides
        /// <summary>
        /// Gets the XML serialization of the calculation options for use in the character file.
        /// </summary>
        /// <returns></returns>
        public string GetXml()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(CalculationOptionsTankDK));
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
            if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs(property)); }
        }
        #endregion
    }
}
