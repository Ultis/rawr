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

            simulateRotation = true;

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

        private bool simulateRotation;
        public bool SimulateRotation
        {
            get { return simulateRotation; }
            set { simulateRotation = value; OnPropertyChanged("SimulateRotation"); OnPropertyChanged("EffectiveCD"); }
        }

        [XmlIgnore]
        public bool EffectiveCD
        {
            get { return !SimulateRotation; }
            set { SimulateRotation = !value; }
        }

        private Ability[] order = { Ability.CrusaderStrike, Ability.HammerOfWrath, Ability.Judgement,
                                                   Ability.DivineStorm, Ability.Consecration, Ability.Exorcism };
        public Ability[] Order
        {
            get { _cache = null; return order; }
            set { _cache = null; order = value; }
        }

        private bool[] selected = { true, true, true, true, true, true };
        public bool[] Selected
        {
            get { _cache = null; return selected; }
            set { _cache = null; selected = value; }
        }

        private Ability[] _cache = null;

        [XmlIgnore]
        public Ability[] Priorities
        {
            get
            {
                if (_cache == null)
                {
                    int count = 0;
                    foreach (bool b in selected) { if (b) count++; }
                    _cache = new Ability[count];

                    int sel = 0;
                    for (int i = 0; i < order.Length; i++)
                    {
                        if (selected[i])
                        {
                            _cache[sel] = order[i];
                            sel++;
                        }
                    }
                }
                return _cache;
            }
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

            clone.order = (Ability[])order.Clone();
            clone.selected = (bool[])selected.Clone();

            return clone;
        }

        #region INotifyPropertyChanged Members
        private void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
    }
}