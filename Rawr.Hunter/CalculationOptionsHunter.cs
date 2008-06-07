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
		private Faction _ScryerAldor = Faction.Aldor;
		private float _latency = .2f;
		private PetAttacks[] _PetAttacks;

		public CalculationOptionsHunter()
		{
			_PetAttacks = new PetAttacks[2];
			_PetAttacks[0] = PetAttacks.Bite;
			_PetAttacks[1] = PetAttacks.Claw;
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
			set { _EnforceMetaGem = true; }
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
