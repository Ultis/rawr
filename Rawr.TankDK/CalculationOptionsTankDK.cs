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
        private Presence _presence = Presence.Blood;
        public Presence presence
        {
            get { return _presence; }
        }
        [XmlIgnore]
        private int _presenceByIndex = (int)Presence.Blood;
        [XmlIgnore]
        public int PresenceByIndex
        {
            get { return _presenceByIndex; }
        }

        private float _VengenceWeight = 1;
        public float VengeanceWeight
        {
            get { return _VengenceWeight; }
            set { _VengenceWeight = value; OnPropertyChanged("VengeanceWeight"); }
        }
        private float _ThreatWeight = 1;
        public float ThreatWeight
        {
            get 
            {
                if ( _ThreatWeight < 0)
                {
                    _ThreatWeight = 1f;
                }
                return _ThreatWeight; 
            }
            set { _ThreatWeight = value; OnPropertyChanged("ThreatWeight"); }
        }
        private float _SurvivalWeight = 1;
        public float SurvivalWeight
        {
            get
            {
                if (_SurvivalWeight < 0)
                {
                    _SurvivalWeight = 1f;
                }
                return _SurvivalWeight;
            }
            set { _SurvivalWeight = value; OnPropertyChanged("SurvivalWeight"); }
        }
        private float _BurstWeight = 6;
        public float BurstWeight
        {
            get
            {
                if (_BurstWeight < 0)
                {
                    _BurstWeight = 1f;
                }
                return _BurstWeight;
            }
            set { _BurstWeight = value; OnPropertyChanged("BurstWeight"); }
        }
        private float _MitigationWeight = 6;
        public float MitigationWeight
        {
            get 
            {
                if (_MitigationWeight < 0)
                {
                    _MitigationWeight = 1f;
                }
                return _MitigationWeight; 
            }
            set { _MitigationWeight = value; OnPropertyChanged("MitigationWeight"); }
        }

        public bool Bloodlust = false;

        private float _pOverHealing = .25f;
        [Percentage]
        public float pOverHealing
        {
            get 
            {
                if (_pOverHealing > 1)
                {
                    _pOverHealing = 1;
                }
                else if (_pOverHealing < 0)
                {
                    _pOverHealing = 0;
                }
                return _pOverHealing; 
            }
            set { _pOverHealing = value; OnPropertyChanged("pOverHealing"); }
        }
        private CalculationType _cType = CalculationType.SMT;
        public CalculationType cType
        {
            get 
            {
                if (_cType != CalculationType.Burst 
                    && _cType != CalculationType.SMT)
                {
                    _cType = CalculationType.SMT;
                }
                return _cType; 
            }
            set { _cType = value; OnPropertyChanged("cType"); }
        }

        private bool _m_bExperimental = false;
        public bool bExperimental
        {
            get
            {
                return _m_bExperimental;
            }
            set { _m_bExperimental = value; OnPropertyChanged("Experimental"); }
        }

        private bool _m_bUseOnUseAbilities = true;
        public bool bUseOnUseAbilities
        {
            get
            {
                return _m_bUseOnUseAbilities;
            }
            set { _m_bUseOnUseAbilities = value; OnPropertyChanged("UseOnUseAbilities"); }
        }
        #endregion

        [XmlIgnore]
        private string _szRotReport = "";
        [XmlIgnore]
        public string szRotReport
        {
            get { return _szRotReport; }
            set { _szRotReport = value.Replace("Dancing Rune Weapon", "Dancing Rune Wp"); OnPropertyChanged("szRotReport"); }
        }

        public DeathKnightTalents talents;

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
