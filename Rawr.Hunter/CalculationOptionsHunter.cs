using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Hunter
{
	[Serializable]
	public class CalculationOptionsHunter : ICalculationOptionBase
	{
		private int _TargetLevel = 83;
		private int _TargetArmor = 13100; //Wrath boss armor
		private Aspect _Aspect = Aspect.Hawk;
		private ShotRotation _shotRotation = ShotRotation.OneToOne;
        private PetFamily _petFamily = PetFamily.Cat;

        private PetAttacks _petPriority1 = PetAttacks.Growl;
        private PetAttacks _petPriority2 = PetAttacks.Bite;
        private PetAttacks _petPriority3 = PetAttacks.None;
        private PetAttacks _petPriority4 = PetAttacks.None;

        private Shots _shotPriority1 = Shots.SteadyShot;
        private Shots _shotPriority2 = Shots.None;
        private Shots _shotPriority3 = Shots.None;
        private Shots _shotPriority4 = Shots.None;

		private Faction _ScryerAldor = Faction.Aldor;
		private float _latency = .2f;
		
		// pet stuff
		private int _cobraReflexes = 2;
		private int	_spikedCollar  = 1;
		private int _spidersBite = 3;
		private int _rabid = 1;
		private int _callOfTheWild = 1;
		private int _sharkAttack = 0;
		private int _wildHunt = 1;
        private int _cornered = 0;
        private int _feedingFrency = 0;
        private int _thunderstomp = 0;
        private int _wolverineBite = 0;
        private int _owlsFocus = 0;
		
        private bool _useCustomShotRotation = false;

        public PetAttacks PetPriority1
        {
            get { return _petPriority1; }
            set { _petPriority1 = value; }
        }

        public PetAttacks PetPriority2
        {
            get { return _petPriority2; }
            set { _petPriority2 = value; }
        }

        public PetAttacks PetPriority3
        {
            get { return _petPriority3; }
            set { _petPriority3 = value; }
        }

        public PetAttacks PetPriority4
        {
            get { return _petPriority4; }
            set { _petPriority4 = value; }
        }


        public bool UseCustomShotRotation
        {
            get { return _useCustomShotRotation; }
            set { _useCustomShotRotation = value; }
        }

		public ShotRotation ShotRotation
		{
			get { return _shotRotation; }
			set { _shotRotation = value; }
		}

        public Shots ShotPriority1
        {
            get { return _shotPriority1; }
            set { _shotPriority1 = value; }
        }

        public Shots ShotPriority2
        {
            get { return _shotPriority2; }
            set { _shotPriority2 = value; }
        }

        public Shots ShotPriority3
        {
            get { return _shotPriority3; }
            set { _shotPriority3 = value; }
        }

        public Shots ShotPriority4
        {
            get { return _shotPriority4; }
            set { _shotPriority4 = value; }
        }

		public Aspect Aspect
		{
			get { return _Aspect; }
			set { _Aspect = value; }
		}

		public PetFamily PetFamily
		{
			get { return _petFamily; }
			set { _petFamily = value; }
		}
        
        public int TargetLevel
		{
			get { return _TargetLevel; }
			set { _TargetLevel = value; }
		}

		public int TargetArmor
		{
			get { return _TargetArmor; }
			set { _TargetArmor = value; }
		}

		public Faction ScryerAldor
		{
			get { return _ScryerAldor; }
			set { _ScryerAldor = value; }
		}

		public float Latency
		{
			get { return _latency; }
			set { _latency = value;}
		}
		
		//petTalents
		
		public int CobraReflexes
		{
			get { return _cobraReflexes; }
			set { _cobraReflexes = value;}
		}
		public int SpikedCollar
		{
			get { return _spikedCollar; }
			set { _spikedCollar = value;}
		}
		public int SpidersBite
			{
			get { return _spidersBite; }
			set { _spidersBite = value;}
		}
		public int Rabid
		{
			get { return _rabid; }
			set { _rabid = value;}
		}
		public int CallOfTheWild
		{
			get { return _callOfTheWild; }
			set { _callOfTheWild = value;}
		}
		public int SharkAttack
		{
			get { return _sharkAttack; }
			set { _sharkAttack = value;}
		}
		public int WildHunt
		{
			get { return _wildHunt; }
			set { _wildHunt = value;}
		}

        public int Cornered
        {
            get { return _cornered; }
            set { _cornered = value; }
        }

        public int FeedingFrenzy
        {
            get { return _feedingFrency; }
            set { _feedingFrency = value; }
        }

        public int Thunderstomp
        {
            get { return _thunderstomp; }
            set { _thunderstomp = value; }
        }

        public int WolverineBite
        {
            get { return _wolverineBite; }
            set { _wolverineBite = value; }
        }

        public int OwlsFocus
        {
            get { return _owlsFocus; }
            set { _owlsFocus = value; }
        }


		#region ICalculationOptionBase Members

		public string GetXml()
		{
			System.Xml.Serialization.XmlSerializer serializer =
				new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptionsHunter));
			StringBuilder xml = new StringBuilder();
			System.IO.StringWriter writer = new System.IO.StringWriter(xml);
			serializer.Serialize(writer, this);
			return xml.ToString();
		}

		#endregion
	}
}
