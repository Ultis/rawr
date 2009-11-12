using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Xml.Serialization;

namespace Rawr.Hunter
{
#if !SILVERLIGHT
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

        #region Rotational Changes
        private bool _InBack;
        public bool InBack
        {
            get { return _InBack; }
            set { _InBack = value; OnPropertyChanged("InBack"); }
        }
        private int _InBackPerc;
        public int InBackPerc
        {
            get { return _InBackPerc; }
            set { _InBackPerc = value; OnPropertyChanged("InBackPerc"); }
        }
        private bool _MultipleTargets;
        public bool MultipleTargets
        {
            get { return _MultipleTargets; }
            set { _MultipleTargets = value; OnPropertyChanged("MultipleTargets"); }
        }
        private int _MultipleTargetsPerc;
        public int MultipleTargetsPerc
        {
            get { return _MultipleTargetsPerc; }
            set { _MultipleTargetsPerc = value; OnPropertyChanged("MultipleTargetsPerc"); }
        }
        private float _MultipleTargetsMax;
        public float MultipleTargetsMax
        {
            get { return _MultipleTargetsMax; }
            set { _MultipleTargetsMax = value; OnPropertyChanged("MultipleTargetsMax"); }
        }
        private bool _MovingTargets;
        public bool MovingTargets
        {
            get { return _MovingTargets; }
            set { _MovingTargets = value; OnPropertyChanged("MovingTargets"); }
        }
        private bool _StunningTargets;
        public bool StunningTargets
        {
            get { return _StunningTargets; }
            set { _StunningTargets = value; OnPropertyChanged("StunningTargets"); }
        }
        private int _StunningTargetsFreq;
        public int StunningTargetsFreq
        {
            get { return _StunningTargetsFreq; }
            set { _StunningTargetsFreq = value; OnPropertyChanged("StunningTargetsFreq"); }
        }
        private float _StunningTargetsDur;
        public float StunningTargetsDur
        {
            get { return _StunningTargetsDur; }
            set { _StunningTargetsDur = value; OnPropertyChanged("StunningTargetsDur"); }
        }
        private bool _FearingTargets;
        public bool FearingTargets
        {
            get { return _FearingTargets; }
            set { _FearingTargets = value; OnPropertyChanged("FearingTargets"); }
        }
        private bool _RootingTargets;
        public bool RootingTargets
        {
            get { return _RootingTargets; }
            set { _RootingTargets = value; OnPropertyChanged("RootingTargets"); }
        }
        private int _RootingTargetsFreq;
        public int RootingTargetsFreq
        {
            get { return _RootingTargetsFreq; }
            set { _RootingTargetsFreq = value; OnPropertyChanged("RootingTargetsFreq"); }
        }
        private float _RootingTargetsDur;
        public float RootingTargetsDur
        {
            get { return _RootingTargetsDur; }
            set { _RootingTargetsDur = value; OnPropertyChanged("RootingTargetsDur"); }
        }
        private bool _DisarmingTargets;
        public bool DisarmingTargets
        {
            get { return _DisarmingTargets; }
            set { _DisarmingTargets = value; OnPropertyChanged("DisarmingTargets"); }
        }
        private int _DisarmingTargetsFreq;
        public int DisarmingTargetsFreq
        {
            get { return _DisarmingTargetsFreq; }
            set { _DisarmingTargetsFreq = value; OnPropertyChanged("DisarmingTargetsFreq"); }
        }
        private float _DisarmingTargetsDur;
        public float DisarmingTargetsDur
        {
            get { return _DisarmingTargetsDur; }
            set { _DisarmingTargetsDur = value; OnPropertyChanged("DisarmingTargetsDur"); }
        }
        private bool _AoETargets;
        public bool AoETargets
        {
            get { return _AoETargets; }
            set { _AoETargets = value; OnPropertyChanged("AoETargets"); }
        }
        private int _AoETargetsFreq;
        public int AoETargetsFreq
        {
            get { return _AoETargetsFreq; }
            set { _AoETargetsFreq = value; OnPropertyChanged("AoETargetsFreq"); }
        }
        private float _AoETargetsDMG;
        public float AoETargetsDMG
        {
            get { return _AoETargetsDMG; }
            set { _AoETargetsDMG = value; OnPropertyChanged("AoETargetsDMG"); }
        }
        // ==============================================================
        private List<Stun> _stuns;
        public List<Stun> Stuns
        {
            get { return _stuns ?? (_stuns = new List<Stun>()); }
            set { _stuns = value; OnPropertyChanged("Stuns"); }
        }
        private List<Move> _moves;
        public List<Move> Moves
        {
            get { return _moves ?? (_moves = new List<Move>()); }
            set { _moves = value; OnPropertyChanged("Moves"); }
        }
        private List<Fear> _fears;
        public List<Fear> Fears
        {
            get { return _fears ?? (_fears = new List<Fear>()); }
            set { _fears = value; OnPropertyChanged("Fears"); }
        }
        private List<Root> _roots;
        public List<Root> Roots
        {
            get { return _roots ?? (_roots = new List<Root>()); }
            set { _roots = value; OnPropertyChanged("Roots"); }
        }
        private List<Disarm> _disarms;
        public List<Disarm> Disarms
        {
            get { return _disarms ?? (_disarms = new List<Disarm>()); }
            set { _disarms = value; OnPropertyChanged("Disarms"); }
        }
        #endregion
        #region Latency
        private float _Lag;
        public float Lag
        {
            get { return _Lag; }
            set { _Lag = value; OnPropertyChanged("Lag"); }
        }
        private float _React;
        public float React
        {
            get { return _React; }
            set { _React = value; OnPropertyChanged("React"); }
        }
        public float Latency { get { return Lag / 1000f; } }
        public float AllowedReact { get { return Math.Max(0f, (React - 250f) / 1000f); } }
        public float FullLatency { get { return AllowedReact + Latency; } }
        #endregion

        public int Duration = 360;
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
            public int CobraReflexes;
            public int DiveDash;
            public int ChargeSwoop;
            public int GreatStamina;
            public int NaturalArmor;
            public int BoarsSpeed;
            public int Mobility;
            public int SpikedCollar;
            public int ImprovedCower;
            public int Bloodthirsty;
            public int BloodOfTheRhino;
            public int PetBarding;
            public int Avoidance;
            public int Lionhearted;
            public int CarrionFeeder;
            public int GuardDog;
            public int Thunderstomp;
            public int GreatResistance;
            public int OwlsFocus;
            public int Cornered;
            public int FeedingFrenzy;
            public int HeartOfThePhoenix;
            public int SpidersBite;
            public int WolverineBite;
            public int RoarOfRecovery;
            public int Bullheaded;
            public int GraceOfTheMantis;
            public int Rabid;
            public int LickYourWounds;
            /// <summary>Your pet roars, increasing your pet's and your melee and ranged attack power by 10%. Lasts 20 sec. 5 min cooldown.</summary>
            public int CallOfTheWild;
            public int LastStand;
            public int Taunt;
            public int Intervene;
            public int WildHunt;
            public int RoarOfSacrifice;
            public int SharkAttack;
            public int Silverback;
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

            XmlSerializer serializer = new XmlSerializer(typeof(CalculationOptionsHunter));
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
