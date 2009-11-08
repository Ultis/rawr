using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Xml.Serialization;

namespace Rawr.HunterSE
{
#if !SILVERLIGHT
	[Serializable]
#endif
	public class CalculationOptionsHunterSE : ICalculationOptionBase
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

		public float Latency = 0.15f;
        public int duration = 360;
        public int timeSpentSub20 = 72;
        public int timeSpent35To20 = 54;
        public float bossHPPercentage = 1;
        public ManaPotionType useManaPotion = ManaPotionType.RunicManaPotion;
        public bool useBeastDuringBeastialWrath = false;
        public bool useKillCommand = true;
        public Aspect selectedAspect = Aspect.Dragonhawk;
        public AspectUsage aspectUsage = AspectUsage.ViperToOOM;
        public int petLevel = 80; // not editable
        public PetHappiness petHappiness = PetHappiness.Happy; // not editable
        public float gcdsToLayImmoTrap = 2.0f; // not editable
        public Shots LALShotToUse = Shots.ExplosiveShot; // not editable
        public int LALShotsReplaced = 2; // not editable
        public float LALProcChance = 2; // not editable

        // rotation test
        public bool useRotationTest = true;
        // 29-10-2009: Drizz: Updated this to 30s from 15(being the default in spreadsheet v92b
        // This value is also able to be updated in the spreadsheet... i.e. should be added to the optionspage.
        public int cooldownCutoff = 30; // not editable (not sure what this does)
        public bool randomizeProcs = false; // not editable
        public float waitForCooldown = 0.8f; // not editable
        public bool interleaveLAL = false; // not editable
        public bool prioritiseArcAimedOverSteady = true; // not editable
        public bool debugShotRotation = false; // not editable

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
        public class PetTalentTree
        {
            public PetTalentTree() {
                CobraReflexes = 0;
                DiveDash = 0;
                ChargeSwoop = 0;
                GreatStamina = 0;
                NaturalArmor = 0;
                BoarsSpeed = 0;
                Mobility = 0;
                SpikedCollar = 0;
                ImprovedCower = 0;
                Bloodthirsty = 0;
                BloodOfTheRhino = 0;
                PetBarding = 0;
                Avoidance = 0;
                Lionhearted = 0;
                CarrionFeeder = 0;
                GuardDog = 0;
                Thunderstomp = 0;
                GreatResistance = 0;
                OwlsFocus = 0;
                Cornered = 0;
                FeedingFrenzy = 0;
                HeartOfThePhoenix = 0;
                SpidersBite = 0;
                WolverineBite = 0;
                RoarOfRecovery = 0;
                Bullheaded = 0;
                GraceOfTheMantis = 0;
                Rabid = 0;
                LickYourWounds = 0;
                CallOfTheWild = 0;
                LastStand = 0;
                Taunt = 0;
                Intervene = 0;
                WildHunt = 0;
                RoarOfSacrifice = 0;
                SharkAttack = 0;
                Silverback = 0;
            }
            public int CobraReflexes, DiveDash,
                       ChargeSwoop, GreatStamina,
                       NaturalArmor, BoarsSpeed,
                       Mobility, SpikedCollar,
                       ImprovedCower, Bloodthirsty,
                       BloodOfTheRhino, PetBarding,
                       Avoidance, Lionhearted,
                       CarrionFeeder, GuardDog,
                       Thunderstomp, GreatResistance,
                       OwlsFocus, Cornered,
                       FeedingFrenzy, HeartOfThePhoenix,
                       SpidersBite, WolverineBite,
                       RoarOfRecovery, Bullheaded,
                       GraceOfTheMantis, Rabid,
                       LickYourWounds, CallOfTheWild,
                       LastStand, Taunt,
                       Intervene, WildHunt,
                       RoarOfSacrifice, SharkAttack,
                       Silverback;
            public void Reset(){
                CobraReflexes = 0;
                DiveDash = 0;
                ChargeSwoop = 0;
                GreatStamina = 0;
                NaturalArmor = 0;
                BoarsSpeed = 0;
                Mobility = 0;
                SpikedCollar = 0;
                ImprovedCower = 0;
                Bloodthirsty = 0;
                BloodOfTheRhino = 0;
                PetBarding = 0;
                Avoidance = 0;
                Lionhearted = 0;
                CarrionFeeder = 0;
                GuardDog = 0;
                Thunderstomp = 0;
                GreatResistance = 0;
                OwlsFocus = 0;
                Cornered = 0;
                FeedingFrenzy = 0;
                HeartOfThePhoenix = 0;
                SpidersBite = 0;
                WolverineBite = 0;
                RoarOfRecovery = 0;
                Bullheaded = 0;
                GraceOfTheMantis = 0;
                Rabid = 0;
                LickYourWounds = 0;
                CallOfTheWild = 0;
                LastStand = 0;
                Taunt = 0;
                Intervene = 0;
                WildHunt = 0;
                RoarOfSacrifice = 0;
                SharkAttack = 0;
                Silverback = 0;
            }
        }
        private PetTalentTree _PetTalents;
        public PetTalentTree PetTalents {
            get { return _PetTalents ?? (_PetTalents = new PetTalentTree()); }
            set { _PetTalents = value; OnPropertyChanged("PetTalents"); }
        }
        public PetFamily PetFamily
		{
			get { return _petFamily; }
            set { _petFamily = value; OnPropertyChanged("PetFamily"); }
		}
        
        public int TargetLevel
		{
			get { return _TargetLevel; }
            set { _TargetLevel = value; OnPropertyChanged("TargetLevel"); }
		}
		public int TargetArmor
		{
			get { return _TargetArmor; }
            set { _TargetArmor = value; OnPropertyChanged("TargetArmor"); }
		}

        private bool _HideBadItems_Spl;
        public bool HideBadItems_Spl
        {
            get { return _HideBadItems_Spl; }
            set { _HideBadItems_Spl = value; OnPropertyChanged("HideBadItems_Spl"); }
        }
        private bool _HideBadItems_PvP;
        public bool HideBadItems_PvP
        {
            get { return _HideBadItems_PvP; }
            set { _HideBadItems_PvP = value; OnPropertyChanged("HideBadItems_PvP"); }
        }
        private bool _HideProfEnchants;
        public bool HideProfEnchants
        {
            get { return _HideProfEnchants; }
            set { _HideProfEnchants = value; OnPropertyChanged("HideProfEnchants"); }
        }

        #region ICalculationOptionBase Members
		public string GetXml()
		{
            _petActiveBuffsXml = new List<string>(_petActiveBuffs.ConvertAll(buff => buff.Name));

            XmlSerializer serializer = new XmlSerializer(typeof(CalculationOptionsHunterSE));
			StringBuilder xml = new StringBuilder();
			System.IO.StringWriter writer = new System.IO.StringWriter(xml);
			serializer.Serialize(writer, this);
			return xml.ToString();
		}
		#endregion
        #region INotifyPropertyChanged Members
        private void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(property));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
    }
}
