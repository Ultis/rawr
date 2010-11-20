using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Xml.Serialization;

namespace Rawr.ShadowPriest
{
    public class CalculationOptionsShadowPriest : ICalculationOptionBase, INotifyPropertyChanged
    {
        public string GetXml()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(CalculationOptionsShadowPriest));
            StringBuilder xml = new StringBuilder();
            System.IO.StringWriter writer = new System.IO.StringWriter(xml);
            serializer.Serialize(writer, this);
            return xml.ToString();
        }
        [XmlIgnore]
        public CharacterCalculationsShadowPriest calculatedStats = null;

        //latency in s
        private float _LatencyGcd = .15f;
        public float LatencyGcd
        {
            get { return _LatencyGcd; }
            set { _LatencyGcd = value; OnPropertyChanged("LatencyGcd"); }
        }

        private float _LatencyCast = .075f;
        public float LatencyCast
        {
            get { return _LatencyCast; }
            set { _LatencyCast = value; OnPropertyChanged("LatencyCast"); }
        }
        
        private int _NumberOfTargets = 1;
        public int NumberOfTargets
        {
            get { return _NumberOfTargets; }
            set { _NumberOfTargets = value; OnPropertyChanged("NumberOfTargets"); }
        }

        //get values for rotation

        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs(propertyName)); }
        }
        #endregion

    }
}
