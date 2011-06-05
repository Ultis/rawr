using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace Rawr.ProtWarr
{
    public class CalculationOptionsProtWarr : ICalculationOptionBase, INotifyPropertyChanged
    {
        #region Weight Adjustments
        [DefaultValue(1)]
        public int RankingMode { get { return _rankingMode; } set { _rankingMode = value; OnPropertyChanged("RankingMode"); } }
        private int _rankingMode = 1;

        [DefaultValue(3.1f)]
        public float HitsToSurvive { get { return _hitsToSurvive; } set { _hitsToSurvive = value; OnPropertyChanged("HitsToSurvive"); } }
        private float _hitsToSurvive = 3.1f;

        [DefaultValue(3.0f)]
        public float BurstScale { get { return _burstScale; } set { _burstScale = value; OnPropertyChanged("BurstScale"); } }
        private float _burstScale = 3.0f;

        [DefaultValue(5.0f)]
        public float ThreatScale { get { return _threatScale; } set { _threatScale = value; OnPropertyChanged("ThreatScale"); } }
        private float _threatScale = 5.0f;
        #endregion

        #region Customize Rotation
        [DefaultValue(0.90f)]
        public float HeroicStrikeFrequency { get { return _heroicStrikeFrequency; } set { _heroicStrikeFrequency = value; OnPropertyChanged("HeroicStrikeFrequency"); } }
        private float _heroicStrikeFrequency = 0.90f;

        [DefaultValue(0.60f)]
        public float AverageVengeance { get { return _averageVengeance; } set { _averageVengeance = value; OnPropertyChanged("AverageVengeance"); } }
        private float _averageVengeance = 0.60f;

        [DefaultValue(true)]
        public bool UseShieldBlock { get { return _useShieldBlock; } set { _useShieldBlock = value; OnPropertyChanged("UseShieldBlock"); } }
        private bool _useShieldBlock = true;

        [DefaultValue(30f)]
        public float ShieldBlockInterval { get { return _shieldBlockInterval; } set { _shieldBlockInterval = value; OnPropertyChanged("ShieldBlockInterval"); } }
        private float _shieldBlockInterval = 30f;
        #endregion

        #region PTR Mode
        [DefaultValue(false)]
        public bool PtrMode { get { return _ptrMode; } set { _ptrMode = value; OnPropertyChanged("PtrMode"); } }
        private bool _ptrMode = false;
        #endregion

        #region Functions
        public string GetXml()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(CalculationOptionsProtWarr));
            StringBuilder xml = new StringBuilder();
            StringWriter writer = new StringWriter(xml);
            serializer.Serialize(writer, this);
            return xml.ToString();
        }
        #endregion
        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string property) { if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(property)); }
        #endregion
    }
}
