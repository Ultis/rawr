using System.ComponentModel;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace Rawr.HealPriest
{
    public class CalculationOptionsHealPriest : ICalculationOptionBase, INotifyPropertyChanged
    {
        private float _Survivability = 2f;
        public float Survivability { get { return _Survivability; } set { _Survivability = value; OnPropertyChanged("Survivability"); } }

        private string _Model = PriestModels.GetDefault();
        public string Model { get { return _Model; } set { _Model = value; OnPropertyChanged("Model"); } }
        private float _ActivityRatio = 88f;
        public float ActivityRatio { get { return _ActivityRatio; } set { _ActivityRatio = value; OnPropertyChanged("ActivityRatio"); } }
        private float _Serendipity = 75f;
        public float Serendipity { get { return _Serendipity; } set { _Serendipity = value; OnPropertyChanged("Serendipity"); } }
        private float _Replenishment = 95f;
        public float Replenishment { get { return _Replenishment; } set { _Replenishment = value; OnPropertyChanged("Replenishment"); } }
        private float _Shadowfiend = 100f;
        public float Shadowfiend { get { return _Shadowfiend; } set { _Shadowfiend = value; OnPropertyChanged("Shadowfiend"); } }
        private float _Rapture = 30f;
        public float Rapture { get { return _Rapture; } set { _Rapture = value; OnPropertyChanged("Rapture"); } }
        private float _TestOfFaith = 25f;
        public float TestOfFaith { get { return _TestOfFaith; } set { _TestOfFaith = value; OnPropertyChanged("TestOfFaith"); } }
        private bool _ModelProcs = true;
        public bool ModelProcs { get { return _ModelProcs; } set { _ModelProcs = value; OnPropertyChanged("ModelProcs"); } }

        private int _FlashHealCast = 0;
        public int FlashHealCast { get { return _FlashHealCast; } set { _FlashHealCast = value; OnPropertyChanged("FlashHealCast"); } }
        private int _BindingHealCast = 0;
        public int BindingHealCast { get { return _BindingHealCast; } set { _BindingHealCast = value; OnPropertyChanged("BindingHealCast"); } }
        private int _GreaterHealCast = 0;
        public int GreaterHealCast { get { return _GreaterHealCast; } set { _GreaterHealCast = value; OnPropertyChanged("GreaterHealCast"); } }
        private int _PenanceCast = 0;
        public int PenanceCast { get { return _PenanceCast; } set { _PenanceCast = value; OnPropertyChanged("PenanceCast"); } }
        private int _RenewCast = 0;
        public int RenewCast { get { return _RenewCast; } set { _RenewCast = value; OnPropertyChanged("RenewCast"); } }
        private int _RenewTicks = 0;
        public int RenewTicks { get { return _RenewTicks; } set { _RenewTicks = value; OnPropertyChanged("RenewTicks"); } }
        private int _ProMCast = 0;
        public int ProMCast { get { return _ProMCast; } set { _ProMCast = value; OnPropertyChanged("ProMCast"); } }
        private int _ProMTicks = 0;
        public int ProMTicks { get { return _ProMTicks; } set { _ProMTicks = value; OnPropertyChanged("ProMTicks"); } }
        private int _PoHCast = 0;
        public int PoHCast { get { return _PoHCast; } set { _PoHCast = value; OnPropertyChanged("PoHCast"); } }
        private int _PWSCast = 0;
        public int PWSCast { get { return _PWSCast; } set { _PWSCast = value; OnPropertyChanged("PWSCast"); } }
        private int _CoHCast = 0;
        public int CoHCast { get { return _CoHCast; } set { _CoHCast = value; OnPropertyChanged("CoHCast"); } }
        private int _HolyNovaCast = 0;
        public int HolyNovaCast { get { return _HolyNovaCast; } set { _HolyNovaCast = value; OnPropertyChanged("HolyNovaCast"); } }
        private int _DivineHymnCast = 0;
        public int DivineHymnCast { get { return _DivineHymnCast; } set { _DivineHymnCast = value; OnPropertyChanged("DivineHymnCast"); } }
        private int _DispelCast = 0;
        public int DispelCast { get { return _DispelCast; } set { _DispelCast = value; OnPropertyChanged("DispelCast"); } }
        private int _MDCast = 0;
        public int MDCast { get { return _MDCast; } set { _MDCast = value; OnPropertyChanged("MDCast"); } }

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
        public bool SG_Spi { get { return StatsList[1]; } set { StatsList[1] = value; OnPropertyChanged("SG_SpI"); } }
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
            XmlSerializer serializer = new XmlSerializer(typeof(CalculationOptionsHealPriest));
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
