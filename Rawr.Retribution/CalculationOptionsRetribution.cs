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
            stackTrinketReset = 0;
            targets = 1f;
            inFront = 0f;
            consEff = 1f;
            bloodlust = true;
            targetSwitches = 0f;
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
            clone.StackTrinketReset = StackTrinketReset;
            clone.Targets = Targets;
            clone.InFront = InFront;
            clone.ConsEff = ConsEff;
            clone.Bloodlust = Bloodlust;
            clone.TargetSwitches = TargetSwitches;
            return clone;
        }

        #region Property 'CacheVars'

        // Tab - Fight Parameters
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

        private int stackTrinketReset;
        public int StackTrinketReset
        {
            get { return stackTrinketReset; }
            set { stackTrinketReset = value; OnPropertyChanged("StackTrinketReset"); }
        }

        private float targets;
        public float Targets
        {
            get { return targets; }
            set { targets = value; OnPropertyChanged("Targets"); }
        }

        private float inFront;
        public float InFront
        {
            get { return inFront; }
            set { inFront = value; OnPropertyChanged("InFront"); }
        }

        private float consEff;
        public float ConsEff
        {
            get { return consEff; }
            set { consEff = value; OnPropertyChanged("ConsEff"); }
        }

        private bool bloodlust;
        public bool Bloodlust
        {
            get { return bloodlust; }
            set { bloodlust = value; OnPropertyChanged("Bloodlust"); }
        }

        private float targetSwitches;
        public float TargetSwitches
        {
            get { return targetSwitches; }
            set { targetSwitches = value; OnPropertyChanged("TargetSwitches"); }
        }
        #endregion

        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs(propertyName)); }
        }
        #endregion
    }
}