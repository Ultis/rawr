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
            set { _presence = value; }
        }

        private int _presenceByIndex = (int)Presence.Frost;
        public int PresenceByIndex
        {
            get { return _presenceByIndex; }
            set
            {
                _presenceByIndex = value;
                if (_presenceByIndex == 0) presence = Presence.None;
                if (_presenceByIndex == 1) presence = Presence.Blood;
                if (_presenceByIndex == 2) presence = Presence.Frost;
                if (_presenceByIndex == 3) presence = Presence.Unholy;
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
        private bool _HideBadItems_Def;
        public bool HideBadItems_Def
        {
            get { return _HideBadItems_Def; }
            set { _HideBadItems_Def = value; OnPropertyChanged("HideBadItems_Def"); }
        }
        private bool _HideBadItems_Spl;
        public bool HideBadItems_Spl
        {
            get { return _HideBadItems_Spl; }
            set { _HideBadItems_Spl = value; OnPropertyChanged("HideBadItems_Spl"); }
        }
        private bool _HideBadItems_PvP;
        public bool HideBadItems_PvP
        {
            get { return _HideBadItems_PvP; }
            set { _HideBadItems_PvP = value; OnPropertyChanged("HideBadItems_PvP"); }
        }
        private string _szRotReport = "";
        public string szRotReport 
        { 
            get { return _szRotReport; }
            set { _szRotReport = value; OnPropertyChanged("szRotReport"); }
        }

        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs(property)); }
        }
        #endregion
    }
}
