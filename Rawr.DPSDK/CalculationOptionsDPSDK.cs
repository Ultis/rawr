using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Xml.Serialization;
using Rawr.DK;

namespace Rawr.DPSDK
{
#if !SILVERLIGHT
    [Serializable]
#endif
    public class CalculationOptionsDPSDK : ICalculationOptionBase, INotifyPropertyChanged
    {
        public string GetXml()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(CalculationOptionsDPSDK));
            StringBuilder xml = new StringBuilder();
            System.IO.StringWriter writer = new System.IO.StringWriter(xml);
            serializer.Serialize(writer, this);
            return xml.ToString();
        }

        private Presence _presence = Presence.Frost;
        public Presence presence
        {
            get { return _presence; }
            set
            {
                _presence = value; /*OnPropertyChanged("PresenceByIndex")*/;
            }
        }
        [XmlIgnore]
        private int _presenceByIndex = (int)Presence.Frost;
        [XmlIgnore]
        public int PresenceByIndex
        {
            get { return _presenceByIndex; }
            set
            {
                _presenceByIndex = value;
                presence = (Presence)value;
                OnPropertyChanged("PresenceByIndex"); 
            }
        }

        private float _GhoulUptime = 1f;
        public float GhoulUptime
        {
            get { return _GhoulUptime; }
            set { _GhoulUptime = value; OnPropertyChanged("GhoulUptime"); }
        }

        private float _KMProcUsage = 1f;
        public float KMProcUsage
        {
            get { return _KMProcUsage; }
            set { _KMProcUsage = value; OnPropertyChanged("KMProcUsage"); }
        }
        
        private bool _Ghoul = true;
        public bool Ghoul
        {
            get { return _Ghoul; }
            set { _Ghoul = value; OnPropertyChanged("Ghoul"); }
        }
        
        private bool _getRefreshForReferenceCalcs = true;
        public bool GetRefreshForReferenceCalcs
        {
            get { return _getRefreshForReferenceCalcs; }
            set { _getRefreshForReferenceCalcs = value; }
        }

        private bool _getRefreshForDisplayCalcs = true;
        public bool GetRefreshForDisplayCalcs
        {
            get { return _getRefreshForDisplayCalcs; }
            set { _getRefreshForDisplayCalcs = value; }
        }

        private bool _getRefreshForSignificantChange = false;
        public bool GetRefreshForSignificantChange
        {
            get { return _getRefreshForSignificantChange; }
            set { _getRefreshForSignificantChange = value; }
        }

        private bool _m_bExperimental = false;
        public bool m_bExperimental
        {
            get { return _m_bExperimental; }
            set { _m_bExperimental = value; OnPropertyChanged("m_bExperimental"); }
        }
        private bool _HideBadItems_Def = true;
        public bool HideBadItems_Def
        {
            get { return _HideBadItems_Def; }
            set { _HideBadItems_Def = value; OnPropertyChanged("HideBadItems_Def"); }
        }
        private bool _HideBadItems_Spl = true;
        public bool HideBadItems_Spl
        {
            get { return _HideBadItems_Spl; }
            set { _HideBadItems_Spl = value; OnPropertyChanged("HideBadItems_Spl"); }
        }
        private bool _HideBadItems_PvP = false;
        public bool HideBadItems_PvP
        {
            get { return _HideBadItems_PvP; }
            set { _HideBadItems_PvP = value; OnPropertyChanged("HideBadItems_PvP"); }
        }
        [XmlIgnore]
        private string _szRotReport = "";
        [XmlIgnore]
        public string szRotReport 
        { 
            get { return _szRotReport; }
            set { _szRotReport = value; OnPropertyChanged("szRotReport"); }
        }

        #region Stat Graph
        [DefaultValue(new bool[] { true, true, true, true, true, true, true, true, true, true, true })]
        public bool[] StatsList { get { return _statsList; } set { _statsList = value; OnPropertyChanged("StatsList"); } }
        private bool[] _statsList = new bool[] { true, true, true, true, true, true, true, true, true, true, true };
        [DefaultValue(100)]
        public int StatsIncrement { get { return _StatsIncrement; } set { _StatsIncrement = value; OnPropertyChanged("StatsIncrement"); } }
        private int _StatsIncrement = 100;
        [DefaultValue("DPS Rating")]
        public string CalculationToGraph { get { return _calculationToGraph; } }
        private string _calculationToGraph = "DPS Rating";
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
        public bool SG_Rage { get { return StatsList[8]; } set { StatsList[8] = value; OnPropertyChanged("SG_Rage"); } }
        #endregion


        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null && !property.StartsWith("SG_")) { PropertyChanged(this, new PropertyChangedEventArgs(property)); }
        }
        #endregion
    }
}
