using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Xml.Serialization;
using Rawr.DK;

namespace Rawr.TankDK
{
#if !SILVERLIGHT
    [Serializable]
#endif
    public class CalculationOptionsTankDK : ICalculationOptionBase, INotifyPropertyChanged
    {
        #region Options
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
        #endregion

        [XmlIgnore]
        public string szRotReport { get { return _szRotReport; } set { _szRotReport = value.Replace("Dancing Rune Weapon", "Dancing Rune Wp"); OnPropertyChanged("szRotReport"); } }
        private string _szRotReport = "";

        #region XML IO
        public string GetXml()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(CalculationOptionsTankDK));
            StringBuilder xml = new StringBuilder();
            System.IO.StringWriter writer = new System.IO.StringWriter(xml);
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
