using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Rawr.Hunter
{
#if SILVERLIGHT
#else
	[Serializable]
#endif
	public class CalculationOptionsHunter : ICalculationOptionBase
	{
		private int _TargetLevel = 83;
		private int _TargetArmor = 10643; //Wrath boss armor
		private Aspect _Aspect = Aspect.Hawk;
        private PetFamily _petFamily = PetFamily.Cat;

        private PetAttacks _petPriority1 = PetAttacks.Growl;
        private PetAttacks _petPriority2 = PetAttacks.Bite;
        private PetAttacks _petPriority3 = PetAttacks.None;
        private PetAttacks _petPriority4 = PetAttacks.None;

		private Faction _ScryerAldor = Faction.Aldor;
		private float _latency = .2f;

        public int duration { get; set; }
        public int timeSpentSub20 = 72;
        public int timeSpent35To20 = 54;
        public float bossHPPercentage = 1;

        // new priority rotation stuff
        public int PriorityIndex1 = 0;
        public int PriorityIndex2 = 0;
        public int PriorityIndex3 = 0;
        public int PriorityIndex4 = 0;
        public int PriorityIndex5 = 0;
        public int PriorityIndex6 = 0;
        public int PriorityIndex7 = 0;
        public int PriorityIndex8 = 0;
        public int PriorityIndex9 = 0;
        public int PriorityIndex10 = 0;
		
		// pet talents
        public int petCobraReflexes = 0;
        public int petDiveDash = 0;
        public int petCharge = 0;
        public int petGreatStamina = 0;
        public int petNaturalArmor = 0;
        public int petBoarsSpeed = 0;
        public int petMobility = 0;
        public int petSpikedCollar = 0;
        public int petImprovedCower = 0;
        public int petBloodthirsty = 0;
        public int petBloodOfTheRhino = 0;
        public int petPetBarding = 0;
        public int petAvoidance = 0;
        public int petLionhearted = 0;
        public int petCarrionFeeder = 0;
        public int petGuardDog = 0;
        public int petThunderstomp = 0;
        public int petGreatResistance = 0;
        public int petOwlsFocus = 0;
        public int petCornered = 0;
        public int petFeedingFrenzy = 0;
        public int petHeartOfThePhoenix = 0;
        public int petSpidersBite = 0;
        public int petWolverineBite = 0;
        public int petRoarOfRecovery = 0;
        public int petBullheaded = 0;
        public int petGraceOfTheMantis = 0;
        public int petRabid = 0;
        public int petLickYourWounds = 0;
        public int petCallOfTheWild = 0;
        public int petLastStand = 0;
        public int petTaunt = 0;
        public int petIntervene = 0;
        public int petWildHunt = 0;
        public int petRoarOfSacrifice = 0;
        public int petSharkAttack = 0;
        public int petSilverback = 0;
                
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
