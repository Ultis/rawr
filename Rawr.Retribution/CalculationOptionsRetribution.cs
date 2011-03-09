using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Xml.Serialization;

namespace Rawr.Retribution
{
    public class CalculationOptionsRetribution : ICalculationOptionBase, INotifyPropertyChanged
    {
        public CalculationOptionsRetribution Clone()
        {//TODO: Get rid of it
            CalculationOptionsRetribution clone = new CalculationOptionsRetribution();
            // Tab - Fight Parameters
            clone.Seal = Seal;
            return clone;
        }

        #region Property 'CacheVars'
        [DefaultValue(SealOf.Truth)]
        private SealOf seal = SealOf.Truth;
        public SealOf Seal
        {
            get { return seal; }
            set { seal = value; OnPropertyChanged("Seal"); }
        }
        #endregion

        #region INotifyPropertyChanged Members
        public string GetXml()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(CalculationOptionsRetribution));
            StringBuilder xml = new StringBuilder();
            System.IO.StringWriter writer = new System.IO.StringWriter(xml);
            serializer.Serialize(writer, this);
            return xml.ToString();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName) { if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName)); }
        #endregion
    }
}
