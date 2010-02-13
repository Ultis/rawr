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
            targetLevel = 83;
            mob = MobType.Humanoid;
            seal = SealOf.Vengeance;
            fightLength = 5f;
            timeUnder20 = .18f;
            delay = .05f;
            wait = .05f;
            targets = 1f;
            inFront = 0f;
            consEff = 1f;
            hoREff = 0f;
            bloodlust = true;
            stackTrinketReset = 0;
            targetSwitches = 0f;

            simulateRotation = true;
            forceRotation = -1;

            judgeCD = 7.1f;
            cSCD = 7.1f;
            dSCD = 10.5f;
            consCD = 10.5f;
            exoCD = 18f;

            judgeCD20 = 7.1f;
            cSCD20 = 7.1f;
            dSCD20 = 12.5f;
            consCD20 = 12.5f;
            exoCD20 = 25f;
            hoWCD20 = 6.4f;

            rotations = new List<Ability[]>();
        }

        private int targetLevel;
        public int TargetLevel
        {
            get { return targetLevel; }
            set { targetLevel = value; OnPropertyChanged("TargetLevel"); }
        }

        private MobType mob;
        public MobType Mob
        {
            get { return mob; }
            set { mob = value; OnPropertyChanged("Mob"); }
        }

        [XmlIgnore]
        public int MobIndex
        {
            get { return (int)mob; }
            set { mob = (MobType)value; OnPropertyChanged("MobIndex"); }
        }

        private SealOf seal;
        public SealOf Seal
        {
            get { return seal; }
            set { seal = value; OnPropertyChanged("Seal"); }
        }

        [XmlIgnore]
        public int SealIndex
        {
            get { return (int)seal; }
            set { seal = (SealOf)value; OnPropertyChanged("SealIndex"); }
        }


        private float fightLength;
        public float FightLength
        {
            get { return fightLength; }
            set { fightLength = value; OnPropertyChanged("FightLength"); }
        }

        private float timeUnder20;
        public float TimeUnder20
        {
            get { return timeUnder20; }
            set { timeUnder20 = value; OnPropertyChanged("TimeUnder20"); }
        }

        private float delay;
        public float Delay
        {
            get { return delay; }
            set { delay = value; OnPropertyChanged("Delay"); }
        }

        private float wait;
        public float Wait
        {
            get { return wait; }
            set { wait = value; OnPropertyChanged("Wait"); }
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

        private float hoREff;
        public float HoREff
        {
            get { return hoREff; }
            set { hoREff = value; OnPropertyChanged("HoREff"); }
        }

        private bool bloodlust;
        public bool Bloodlust
        {
            get { return bloodlust; }
            set { bloodlust = value; OnPropertyChanged("Bloodlust"); }
        }

        private int stackTrinketReset;
        public int StackTrinketReset
        {
            get { return stackTrinketReset; }
            set { stackTrinketReset = value; OnPropertyChanged("StackTrinketReset"); }
        }

        private float targetSwitches;
        public float TargetSwitches
        {
            get { return targetSwitches; }
            set { targetSwitches = value; OnPropertyChanged("TargetSwitches"); }
        }

        private bool simulateRotation;
        public bool SimulateRotation
        {
            get { return simulateRotation; }
            set { simulateRotation = value; OnPropertyChanged("SimulateRotation"); OnPropertyChanged("EffectiveCD"); }
        }

        private int forceRotation;
        [XmlIgnore]
        public int ForceRotation
        {
            get { return forceRotation; }
            set { forceRotation = value; }
        }

        [XmlIgnore]
        public bool EffectiveCD
        {
            get { return !SimulateRotation; }
            set { SimulateRotation = !value; }
        }

        [XmlIgnore]
        public Ability[] Order
        {
            get { return rotations.Count > 0 ? rotations[0] : null; }
            set { rotations[0] = value; }
        }

        [XmlIgnore]
        public bool[] Selected
        {
            get { return null; }
            set { ; }
        }

        private List<Ability[]> rotations;
        public List<Ability[]> Rotations
        {
            get { return rotations; }
            set { rotations = value; OnPropertyChanged("Rotations"); }
        }

        private float judgeCD;
        public float JudgeCD
        {
            get { return judgeCD; }
            set { judgeCD = value; OnPropertyChanged("JudgeCD"); }
        }

        private float cSCD;
        public float CSCD
        {
            get { return cSCD; }
            set { cSCD = value; OnPropertyChanged("CSCD"); }
        }

        private float dSCD;
        public float DSCD
        {
            get { return dSCD; }
            set { dSCD = value; OnPropertyChanged("DSCD"); }
        }

        private float consCD;
        public float ConsCD
        {
            get { return consCD; }
            set { consCD = value; OnPropertyChanged("ConsCD"); }
        }

        private float exoCD;
        public float ExoCD
        {
            get { return exoCD; }
            set { exoCD = value; OnPropertyChanged("ExoCD"); }
        }

        private float judgeCD20;
        public float JudgeCD20
        {
            get { return judgeCD20; }
            set { judgeCD20 = value; OnPropertyChanged("JudgeCD20"); }
        }

        private float cSCD20;
        public float CSCD20
        {
            get { return cSCD20; }
            set { cSCD20 = value; OnPropertyChanged("CSCD20"); }
        }

        private float dSCD20;
        public float DSCD20
        {
            get { return dSCD20; }
            set { dSCD20 = value; OnPropertyChanged("DSCD20"); }
        }

        private float consCD20;
        public float ConsCD20
        {
            get { return consCD20; }
            set { consCD20 = value; OnPropertyChanged("ConsCD20"); }
        }

        private float exoCD20;
        public float ExoCD20
        {
            get { return exoCD20; }
            set { exoCD20 = value; OnPropertyChanged("ExoCD20"); }
        }

        private float hoWCD20;
        public float HoWCD20
        {
            get { return hoWCD20; }
            set { hoWCD20 = value; OnPropertyChanged("HoWCD20"); }
        }

        public CalculationOptionsRetribution Clone()
        {
            CalculationOptionsRetribution clone = new CalculationOptionsRetribution();

            clone.TargetLevel = TargetLevel;
            clone.Mob = Mob;
            clone.Seal = Seal;
            clone.FightLength = FightLength;
            clone.TimeUnder20 = TimeUnder20;
            clone.Delay = Delay;
            clone.Wait = Wait;
            clone.SimulateRotation = SimulateRotation;
            clone.Targets = Targets;
            clone.InFront = InFront;
            clone.ConsEff = ConsEff;
            clone.HoREff = HoREff;
            clone.Bloodlust = Bloodlust;
            clone.StackTrinketReset = StackTrinketReset;
            clone.TargetSwitches = TargetSwitches;

            clone.JudgeCD = JudgeCD;
            clone.CSCD = CSCD;
            clone.DSCD = DSCD;
            clone.ConsCD = ConsCD;
            clone.ExoCD = ExoCD;

            clone.JudgeCD20 = JudgeCD20;
            clone.CSCD20 = CSCD20;
            clone.DSCD20 = DSCD20;
            clone.ConsCD20 = ConsCD20;
            clone.ExoCD20 = ExoCD20;
            clone.HoWCD20 = HoWCD20;

            clone.Rotations = new List<Ability[]>(Rotations);

            return clone;
        }

        [XmlIgnore]
        private Character _character;
        [XmlIgnore]
        public Character Character
        {
            get
            {
                return _character;
            }
            set
            {
                _character = value;
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
            if (Character != null)
            {
                Character.OnCalculationsInvalidated();
            }
        }

        #endregion
    }
}