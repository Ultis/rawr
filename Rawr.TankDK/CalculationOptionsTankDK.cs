using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace Rawr.TankDK
{
#if !SILVERLIGHT
	[Serializable]
#endif
	public class CalculationOptionsTankDK : ICalculationOptionBase
	{
        // No one will ever tank in anything other than Frost.
        //		public enum Presence { Blood, Frost, Unholy }
        #region Options
        private int _TargetLevel = 83;
        public int TargetLevel
        {
            get { return _TargetLevel; }
            set { _TargetLevel = value; OnPropertyChanged("TargetLevel"); }
        }
        private float _ThreatWeight = 1.00f;
        public float ThreatWeight
        {
            get { return _ThreatWeight; }
            set { _ThreatWeight = value; OnPropertyChanged("ThreatWeight"); }
        }
        private float _SurvivalWeight = 1.00f;
        public float SurvivalWeight
        {
            get { return _SurvivalWeight; }
            set { _SurvivalWeight = value; OnPropertyChanged("SurvivalWeight"); }
        }
        public uint IncomingDamage = 10000;
		public float PercentIncomingFromMagic = .0f;
		public float BossAttackSpeed = 2.5f;
		public float BossArmor = StatConversion.NPC_ARMOR[83 - 80];
		public bool Bloodlust = false;

        private float _fightLength = 10f;
        public float FightLength
        {
            get { return _fightLength; }
            set { _fightLength = value; OnPropertyChanged("FightLength"); }
        }

        private uint _uNumberTargets = 1;
        public uint uNumberTargets
        {
            get { return _uNumberTargets; }
            set { _uNumberTargets = value; OnPropertyChanged("uNumberTargets"); }
        }
        private CalculationType _cType = CalculationType.SMT;
        public CalculationType cType
        {
            get { return _cType; }
            set { _cType = value; OnPropertyChanged("cType"); }
        }
        #endregion

        #region Rotation
        
        public Rotation m_Rotation = new Rotation();

        public float CurRotationDuration
        {
            get { return m_Rotation.curRotationDuration; }
            set { m_Rotation.curRotationDuration = value; OnPropertyChanged("CurRotationDuration"); }
        }
        // Single Rune, Single Target
        public float IcyTouch
        {
            get { return m_Rotation.IcyTouch; }
            set { m_Rotation.IcyTouch = value; OnPropertyChanged("IcyTouch"); }
        }
        public float PlagueStrike
        {
            get { return m_Rotation.PlagueStrike; }
            set { m_Rotation.PlagueStrike = value; OnPropertyChanged("PlagueStrike"); }
        }
        public float BloodStrike
        {
            get { return m_Rotation.BloodStrike; }
            set { m_Rotation.BloodStrike = value; OnPropertyChanged("BloodStrike"); }
        }
        public float HeartStrike
        {
            get { return m_Rotation.HeartStrike; }
            set { m_Rotation.HeartStrike = value; OnPropertyChanged("HeartStrike"); }
        }
        // Multi Rune, Single Target
        public float DeathStrike
        {
            get { return m_Rotation.DeathStrike; }
            set { m_Rotation.DeathStrike = value; OnPropertyChanged("DeathStrike"); }
        }
        public float Obliterate
        {
            get { return m_Rotation.Obliterate; }
            set { m_Rotation.Obliterate = value; OnPropertyChanged("Obliterate"); }
        }
        public float ScourgeStrike
        {
            get { return m_Rotation.ScourgeStrike; }
            set { m_Rotation.ScourgeStrike = value; OnPropertyChanged("ScourgeStrike"); }
        }
        
        // Multi Target
        public float Pestilence
        {
            get { return m_Rotation.Pestilence; }
            set { m_Rotation.Pestilence = value; OnPropertyChanged("Pestilence"); }
        }
        public float HowlingBlast
        {
            get { return m_Rotation.HowlingBlast; }
            set { m_Rotation.HowlingBlast = value; OnPropertyChanged("HowlingBlast"); }
        }
        public float DeathNDecay
        {
            get { return m_Rotation.DeathNDecay; }
            set { m_Rotation.DeathNDecay = value; OnPropertyChanged("DeathNDecay"); }
        }
        public float BloodBoil
        {
            get { return m_Rotation.BloodBoil; }
            set { m_Rotation.BloodBoil = value; OnPropertyChanged("BloodBoil"); }
        }

        // RP
        public float RuneStrike
        {
            get { return m_Rotation.RuneStrike; }
            set { m_Rotation.RuneStrike = value; OnPropertyChanged("RuneStrike"); }
        }
        public float DeathCoil
        {
            get { return m_Rotation.DeathCoil; }
            set { m_Rotation.DeathCoil = value; OnPropertyChanged("DeathCoil"); }
        }
        public float FrostStrike
        {
            get { return m_Rotation.FrostStrike; }
            set { m_Rotation.FrostStrike = value; OnPropertyChanged("FrostStrike"); }
        }
        // Other GCD
        public float HornOfWinter
        {
            get { return m_Rotation.HornOfWinter; }
            set { m_Rotation.HornOfWinter = value; OnPropertyChanged("HornOfWinter"); }
        }
        #endregion

		public DeathKnightTalents talents;


        #region XML IO
        public string GetXml()
		{
			System.Xml.Serialization.XmlSerializer serializer =
				new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptionsTankDK));
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
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
        #endregion
	}
}
