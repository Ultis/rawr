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
        {
            CalculationOptionsRetribution clone = new CalculationOptionsRetribution();
            // Tab - Fight Parameters
            clone.Seal = Seal;
            clone.InqRefresh = InqRefresh;
            clone.SkipToCrusader = SkipToCrusader;
            clone.NewRotation = NewRotation;
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
        [DefaultValue(4f)]
        private float inqRefresh = 4f;
        public float InqRefresh
        {
            get { return inqRefresh; }
            set { inqRefresh = value; OnPropertyChanged("InqRefresh"); }
        }
        [DefaultValue(.4f)]
        private float skipToCrusader = .4f;
        public float SkipToCrusader
        {
            get { return skipToCrusader; }
            set { skipToCrusader = value; OnPropertyChanged("SkipToCrusader"); }
        }
        [DefaultValue(3)]
        private int hPperInq = 3;
        public int HPperInq
        {
            get { return hPperInq; }
            set { hPperInq = value; OnPropertyChanged("HPperInq"); }
        }
        [DefaultValue(true)]
        private bool newRotation;
        public bool NewRotation
        {
            get { return newRotation; }
            set { newRotation = value; OnPropertyChanged("NewRotation"); }
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
