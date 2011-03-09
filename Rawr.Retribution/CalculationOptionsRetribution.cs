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

        public CalculationOptionsRetribution()
        {
            // Tab - Fight Parameters
            fightLength = 5f;
            mob = MobType.Humanoid;
            seal = SealOf.Truth;
            targetLevel = 88;
            timeUnder20 = .18f;
            targets = 1f;
            bloodlust = true;
        }

        public CalculationOptionsRetribution Clone()
        {
            CalculationOptionsRetribution clone = new CalculationOptionsRetribution();
            // Tab - Fight Parameters
            clone.FightLength = FightLength;
            clone.Mob = Mob;
            clone.Seal = Seal;
            clone.TargetLevel = TargetLevel;
            clone.TimeUnder20 = TimeUnder20;
            clone.Targets = Targets;
            clone.Bloodlust = Bloodlust;
            return clone;
        }

        #region Property 'CacheVars'
        private float fightLength;
        public float FightLength
        {
            get { return fightLength; }
            set { fightLength = value; OnPropertyChanged("FightLength"); }
        }

        private MobType mob;
        public MobType Mob
        {
            get { return mob; }
            set { mob = value; OnPropertyChanged("Mob"); }
        }

        private SealOf seal;
        public SealOf Seal
        {
            get { return seal; }
            set { seal = value; OnPropertyChanged("Seal"); }
        }

        private int targetLevel;
        public int TargetLevel
        {
            get { return targetLevel; }
            set { targetLevel = value; OnPropertyChanged("TargetLevel"); }
        }

        private float timeUnder20;
        public float TimeUnder20
        {
            get { return timeUnder20; }
            set { timeUnder20 = value; OnPropertyChanged("TimeUnder20"); }
        }

        private float targets;
        public float Targets
        {
            get { return targets; }
            set { targets = value; OnPropertyChanged("Targets"); }
        }

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