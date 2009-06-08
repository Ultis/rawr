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
        private PetFamily _petFamily = PetFamily.Cat;

        private PetAttacks _petPriority1 = PetAttacks.Growl;
        private PetAttacks _petPriority2 = PetAttacks.Bite;
        private PetAttacks _petPriority3 = PetAttacks.None;
        private PetAttacks _petPriority4 = PetAttacks.None;

		private Faction _ScryerAldor = Faction.Aldor;
		private float _latency = .2f;

        //rotation stuff
        private bool _aimedInRot = false;
        private bool _arcaneInRot = false;
        private bool _multiInRot = false;
        private bool _serpentInRot = true;
        private bool _silenceInRot = false;
        private bool _steadyInRot = true;
        private bool _killInRot = true;
        private bool _explosiveInRot = false;
        private bool _chimeraInRot = false;
        private bool _blackInRot = false;
		
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

        public int duration {get; set;}
        
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

        public bool AimedInRot
        {
            get { return _aimedInRot; }
            set { _aimedInRot = value; }
        }
        public bool ArcaneInRot
        {
            get { return _arcaneInRot; }
            set { _arcaneInRot = value; }
        }
        public bool MultiInRot
        {
            get { return _multiInRot; }
            set { _multiInRot = value; }
        }
        public bool SerpentInRot
        {
            get { return _serpentInRot; }
            set { _serpentInRot = value; }
        }
        public bool SilenceInRot
        {
            get { return _silenceInRot; }
            set { _silenceInRot = value; }
        }
        public bool SteadyInRot
        {
            get { return _steadyInRot; }
            set { _steadyInRot = value; }
        }
        public bool KillInRot
        {
            get { return _killInRot; }
            set { _killInRot = value; }
        }
        public bool ExplosiveInRot
        {
            get { return _explosiveInRot; }
            set { _explosiveInRot = value; }
        }
        public bool ChimeraInRot
        {
            get { return _chimeraInRot; }
            set { _chimeraInRot = value; }
        }
        public bool BlackInRot
        {
            get { return _blackInRot; }
            set { _blackInRot = value; }
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
