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
        private PetFamily _petFamily = PetFamily.Cat;

        public PetAttacks PetPriority1 = PetAttacks.Growl;
        public PetAttacks PetPriority2 = PetAttacks.Bite;
        public PetAttacks PetPriority3 = PetAttacks.None;
        public PetAttacks PetPriority4 = PetAttacks.None;
        public PetAttacks PetPriority5 = PetAttacks.None;
        public PetAttacks PetPriority6 = PetAttacks.None;
        public PetAttacks PetPriority7 = PetAttacks.None;
        
        [XmlIgnore]
        private List<Buff> _petActiveBuffs = new List<Buff>();
        [XmlElement("petActiveBuffs")]
        public List<string> _petActiveBuffsXml = new List<string>();
        [XmlIgnore]
        public List<Buff> petActiveBuffs
        {
            get { return _petActiveBuffs; }
            set { _petActiveBuffs = value; }
        }

		public double Latency = 0.15;
        public int duration = 360;
        public int timeSpentSub20 = 72;
        public int timeSpent35To20 = 54;
        public float bossHPPercentage = 1;
        public ManaPotionType useManaPotion = ManaPotionType.RunicManaPotion;
        public bool useBeastDuringBeastialWrath = false;
        public bool useKillCommand = true;
        public Aspect selectedAspect = Aspect.Dragonhawk;
        public AspectUsage aspectUsage = AspectUsage.ViperToOOM;
        public HeroismUsage heroismUsage = HeroismUsage.Once;
        public int petLevel = 80; // not editable
        public PetHappiness petHappiness = PetHappiness.Happy; // not editable
        public double gcdsToLayImmoTrap = 2.0; // not editable
        public Shots LALShotToUse = Shots.ExplosiveShot; // not editable
        public int LALShotsReplaced = 2; // not editable

        // rotation test
        public bool useRotationTest = false;
        public int cooldownCutoff = 15;  // not editable (not sure what this does)
        public bool randomizeProcs = false; // not editable
        public double waitForCooldown = 0.8; // not editable
        public bool interleaveLAL = false; // not editable
        public bool prioritiseArcAimedOverSteady = true; // not editable

        // NOTE: setting this to true does 'bad' uptime calculations,
        // to help match the spread sheet. if a fight last 10 seconds
        // and an ability has a 4 second cooldown, the spreadsheet says
        // you can use it 2.5 times, while we say you can use it twice.
        public bool calculateUptimesLikeSpreadsheet = true;
        public bool emulateSpreadsheetBugs = true;

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
        public int petChargeSwoop = 0;
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

        #region ICalculationOptionBase Members

		public string GetXml()
		{
            _petActiveBuffsXml = new List<string>(_petActiveBuffs.ConvertAll(buff => buff.Name));

            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptionsHunter));
			StringBuilder xml = new StringBuilder();
			System.IO.StringWriter writer = new System.IO.StringWriter(xml);
			serializer.Serialize(writer, this);
			return xml.ToString();
		}

		#endregion
	}
}
