using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Xml.Serialization;

namespace Rawr.Retribution
{
    public class CalculationOptionsRetribution : ICalculationOptionBase, INotifyPropertyChanged
    {
        public string GetXml()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(CalculationOptionsRetribution));
            StringBuilder xml = new StringBuilder();
            System.IO.StringWriter writer = new System.IO.StringWriter(xml);
            serializer.Serialize(writer, this);
            return xml.ToString();
        }

        public CalculationOptionsRetribution Clone()
        {//TODO: Get rid of it
            CalculationOptionsRetribution clone = new CalculationOptionsRetribution();
            // Tab - Fight Parameters
            clone.Mob = Mob;
            clone.Seal = Seal;
            clone.Bloodlust = Bloodlust;
            return clone;
        }

        #region Property 'CacheVars'
        [DefaultValue(MobType.Humanoid)]
        private MobType mob;
        public MobType Mob
        {
            get { return mob; }
            set { mob = value; OnPropertyChanged("Mob"); }
        }

        [DefaultValue(SealOf.Truth)]
        private SealOf seal;
        public SealOf Seal
        {
            get { return seal; }
            set { seal = value; OnPropertyChanged("Seal"); }
        }

        [DefaultValue(true)]
        private bool bloodlust;
        public bool Bloodlust
        {
            get { return bloodlust; }
            set { bloodlust = value; OnPropertyChanged("Bloodlust"); }
        }
        #endregion

        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null) 
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName)); 
        }
        #endregion
    }
}