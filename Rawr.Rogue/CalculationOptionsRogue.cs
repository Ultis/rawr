using System.ComponentModel;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace Rawr.Rogue
{
    /// <summary>
    /// Holds the options the user selects from the options tab.
    /// </summary>
    public class CalculationOptionsRogue : ICalculationOptionBase, INotifyPropertyChanged
    {
        [DefaultValue(200)]
        public int Latency
        {
            get { return _latency; }
            set { if (_latency != value) { _latency = value; OnPropertyChanged("Latency"); } }
        }
        private int _latency = 200;

        [DefaultValue(true)]
        public bool TargetPoisonable {
            get { return _targetPoisonable; }
            set { if (_targetPoisonable != value) { _targetPoisonable = value; OnPropertyChanged("TargetPoisonable"); } }
        }
        private bool _targetPoisonable = true;

        [DefaultValue(true)]
        public bool BleedIsUp
        {
            get { return _bleedIsUp; }
            set { if (_bleedIsUp != value) { _bleedIsUp = value; OnPropertyChanged("BleedIsUp"); } }
        }
        private bool _bleedIsUp = true;

        [DefaultValue(1)]
        public int CustomCPG
        {
            get { return _customCPG; }
            set { if (_customCPG != value) { _customCPG = value; OnPropertyChanged("CustomCPG"); } }
        }
        private int _customCPG = 1;

        [DefaultValue(false)]
        public bool ForceCustom
        {
            get { return _forceCustom; }
            set { if (_forceCustom != value) { _forceCustom = value; OnPropertyChanged("ForceCustom"); } }
        }
        private bool _forceCustom = false;

        [DefaultValue(0)]
        public int CustomRecupCP
        {
            get { return _customRecupCP; }
            set { if (_customRecupCP != value) { _customRecupCP = value; OnPropertyChanged("CustomRecupCP"); } }
        }
        private int _customRecupCP = 0;

        [DefaultValue(0)]
        public int CustomRuptCP
        {
            get { return _customRuptCP; }
            set { if (_customRuptCP != value) { _customRuptCP = value; OnPropertyChanged("CustomRuptCP"); } }
        }
        private int _customRuptCP = 0;

        [DefaultValue(false)]
        public bool CustomUseRS
        {
            get { return _customUseRS; }
            set { if (_customUseRS != value) { _customUseRS = value; OnPropertyChanged("CustomUseRS"); } }
        }
        private bool _customUseRS = false;

        [DefaultValue(false)]
        public bool CustomUseExpose
        {
            get { return _customUseExpose; }
            set { if (_customUseExpose != value) { _customUseExpose = value; OnPropertyChanged("CustomUseExpose"); } }
        }
        private bool _customUseExpose = false;

        [DefaultValue(false)]
        public bool CustomUseTotT
        {
            get { return _customUseTotT; }
            set { if (_customUseTotT != value) { _customUseTotT = value; OnPropertyChanged("CustomUseTotT"); } }
        }
        private bool _customUseTotT = false;

        [DefaultValue(1)]
        public int CustomFinisher
        {
            get { return _customFinisher; }
            set { if (_customFinisher != value) { _customFinisher = value; OnPropertyChanged("CustomFinisher"); } }
        }
        private int _customFinisher = 1;

        [DefaultValue(5)]
        public int CustomCPFinisher
        {
            get { return _customCPFinisher; }
            set { if (_customCPFinisher != value) { _customCPFinisher = value; OnPropertyChanged("CustomCPFinisher"); } }
        }
        private int _customCPFinisher = 5;

        [DefaultValue(5)]
        public int CustomCPSnD
        {
            get { return _customCPSnD; }
            set { if (_customCPSnD != value) { _customCPSnD = value; OnPropertyChanged("CustomCPSnD"); } }
        }
        private int _customCPSnD = 5;

        [DefaultValue(1)]
        public int CustomMHPoison
        {
            get { return _customMHPoison; }
            set { if (_customMHPoison != value) { _customMHPoison = value; OnPropertyChanged("CustomMHPoison"); } }
        }
        private int _customMHPoison = 1;

        [DefaultValue(2)]
        public int CustomOHPoison
        {
            get { return _customOHPoison; }
            set { if (_customOHPoison != value) { _customOHPoison = value; OnPropertyChanged("CustomOHPoison"); } }
        }
        private int _customOHPoison = 2;

        [DefaultValue(true)]
        public bool EnableMuti
        {
            get { return _enableMuti; }
            set { if (_enableMuti != value) { _enableMuti = value; OnPropertyChanged("EnableMuti"); } }
        }
        private bool _enableMuti = true;

        [DefaultValue(true)]
        public bool EnableSS
        {
            get { return _enableSS; }
            set { if (_enableSS != value) { _enableSS = value; OnPropertyChanged("EnableSS"); } }
        }
        private bool _enableSS = true;

        [DefaultValue(true)]
        public bool EnableBS
        {
            get { return _enableBS; }
            set { if (_enableBS != value) { _enableBS = value; OnPropertyChanged("EnableBS"); } }
        }
        private bool _enableBS = true;

        [DefaultValue(true)]
        public bool EnableHemo
        {
            get { return _enableHemo; }
            set { if (_enableHemo != value) { _enableHemo = value; OnPropertyChanged("EnableHemo"); } }
        }
        private bool _enableHemo = true;

        [DefaultValue(true)]
        public bool EnableRS
        {
            get { return _enableRS; }
            set { if (_enableRS != value) { _enableRS = value; OnPropertyChanged("EnableRS"); } }
        }
        private bool _enableRS = true;

        [DefaultValue(true)]
        public bool EnableIP
        {
            get { return _enableIP; }
            set { if (_enableIP != value) { _enableIP = value; OnPropertyChanged("EnableIP"); } }
        }
        private bool _enableIP = true;

        [DefaultValue(true)]
        public bool EnableDP
        {
            get { return _enableDP; }
            set { if (_enableDP != value) { _enableDP = value; OnPropertyChanged("EnableDP"); } }
        }
        private bool _enableDP = true;

        [DefaultValue(true)]
        public bool EnableWP
        {
            get { return _enableWP; }
            set { if (_enableWP != value) { _enableWP = value; OnPropertyChanged("EnableWP"); } }
        }
        private bool _enableWP = true;

        [DefaultValue(true)]
        public bool EnableRupt
        {
            get { return _enableRupt; }
            set { if (_enableRupt != value) { _enableRupt = value; OnPropertyChanged("EnableRupt"); } }
        }
        private bool _enableRupt = true;

        [DefaultValue(true)]
        public bool EnableRecup
        {
            get { return _enableRecup; }
            set { if (_enableRecup != value) { _enableRecup = value; OnPropertyChanged("EnableRecup"); } }
        }
        private bool _enableRecup = true;

        [DefaultValue(true)]
        public bool EnableEvis
        {
            get { return _enableEvis; }
            set { if (_enableEvis != value) { _enableEvis = value; OnPropertyChanged("EnableEvis"); } }
        }
        private bool _enableEvis = true;

        [DefaultValue(true)]
        public bool EnableEnvenom
        {
            get { return _enableEnvenom; }
            set { if (_enableEnvenom != value) { _enableEnvenom = value; OnPropertyChanged("EnableEnvenom"); } }
        }
        private bool _enableEnvenom = true;

        [DefaultValue(0)]
        public float TrinketOffset
        {
            get { return _trinketOffset; }
            set { if (_trinketOffset != value) { _trinketOffset = value; OnPropertyChanged("TrinketOffset"); } }
        }
        private float _trinketOffset = 0f;

        [DefaultValue(false)]
        public bool PTRMode
        {
            get { return _PTRMode; }
            set { if (_PTRMode != value) { _PTRMode = value; OnPropertyChanged("PTRMode"); } }
        }
        private bool _PTRMode = false;

        #region Stat Graph
        [DefaultValue(new bool[] { true, true, true, true, true, /**/ true, true, true, })]
        public bool[] StatsList { get { return _statsList; } set { _statsList = value; OnPropertyChanged("StatsList"); } }
        private bool[] _statsList = new bool[] { true, true, true, true, true, /**/ true, true, true, };
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
        #endregion

        #region ICalculationOptionBase overrides
        /// <summary>
        /// Gets the XML serialization of the calculation options for use in the character file.
        /// </summary>
        /// <returns></returns>
        public string GetXml()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(CalculationOptionsRogue));
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