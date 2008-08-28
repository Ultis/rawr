using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Hunter
{
	[Serializable]
	public class CalculationOptionsHunter : ICalculationOptionBase
	{
		private int _TargetLevel = 73;
		private int _TargetArmor = 7700;
		private bool _EnforceMetaGem = true;
		private Aspect _Aspect = Aspect.Hawk;
		private ShotRotation _shotRotation = ShotRotation.OneToOne;
        private PetFamily _petFamily = PetFamily.Cat;
        private PetAttacks _petPriority1 = PetAttacks.Bite;
        private PetAttacks _petPriority2 = PetAttacks.Claw;
        private PetAttacks _petPriority3 = PetAttacks.None;
		private Faction _ScryerAldor = Faction.Aldor;
		private float _latency = .2f;
		private PetAttacks[] _PetAttacks;

		public CalculationOptionsHunter()
		{
            _PetAttacks = new PetAttacks[3];
			_PetAttacks[0] = _petPriority1;
			_PetAttacks[1] = _petPriority2;
            _PetAttacks[2] = _petPriority3;
		}

		public ShotRotation ShotRotation
		{
			get { return _shotRotation; }
			set { _shotRotation = value; }
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

		public bool EnforceMetaGem
		{
			get { return _EnforceMetaGem; }
			set { _EnforceMetaGem = value; }
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

		public PetAttacks[] PetAttackSequence
		{
			get { return _PetAttacks; }
			set { _PetAttacks = value; }
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
