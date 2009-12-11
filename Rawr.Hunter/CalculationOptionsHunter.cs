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

        #region Pet
        public PetAttacks PetPriority1 = PetAttacks.Growl;
        public PetAttacks PetPriority2 = PetAttacks.Bite;
        public PetAttacks PetPriority3 = PetAttacks.None;
        public PetAttacks PetPriority4 = PetAttacks.None;
        public PetAttacks PetPriority5 = PetAttacks.None;
        public PetAttacks PetPriority6 = PetAttacks.None;
        public PetAttacks PetPriority7 = PetAttacks.None;
        [XmlIgnore]
        private List<Buff> _petActiveBuffs;
        [XmlElement("petActiveBuffs")]
        public List<string> _petActiveBuffsXml = new List<string>();
        [XmlIgnore]
        public List<Buff> petActiveBuffs {
            get { return _petActiveBuffs ?? (_petActiveBuffs = new List<Buff>()); }
            set { _petActiveBuffs = value; OnPropertyChanged("PetActiveBuffs"); }
        }
        private int _petLevel = 80; // Not Editable
        public int PetLevel {
            get { return _petLevel; }
            set { _petLevel = value; OnPropertyChanged("PetLevel"); }
        }
        private PetHappiness _petHappiness = PetHappiness.Happy; // Not Editable
        public PetHappiness PetHappinessLevel {
            get { return _petHappiness; }
            set { _petHappiness = value; OnPropertyChanged("PetHappinessLevel"); }
        }
        [XmlIgnore]
        private PetTalentTree _PetTalents;
        [XmlIgnore]
        public PetTalentTree PetTalents {
            get { return _PetTalents ?? (_PetTalents = new PetTalentTree()); }
            set { _PetTalents = value; OnPropertyChanged("PetTalents"); }
        }
        private string _petTalents;
        public string petTalents {
            get {
                if ( String.IsNullOrEmpty(_petTalents) && _PetTalents != null) { _petTalents = _PetTalents.ToString(); }
                //if (!String.IsNullOrEmpty(_petTalents) && _petTalents.Length != 37) { _petTalents = "0000000000000000000000000000000000000"; }
                return _petTalents;
            }
            set { _petTalents = value; }
        }
        private PetFamily _petFamily = PetFamily.Cat;
        public PetFamily PetFamily {
            get { return _petFamily; }
            set { _petFamily = value; OnPropertyChanged("PetFamily"); }
        }
        #endregion

        #region Rotational Changes
        private bool _InBack;
        public bool InBack
        {
            get { return _InBack; }
            set { _InBack = value; OnPropertyChanged("InBack"); }
        }
        private float _InBackPerc;
        public float InBackPerc
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
        private float _MultipleTargetsPerc;
        public float MultipleTargetsPerc
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
        private List<Impedence> _stuns;
        public List<Impedence> Stuns
        {
            get { return _stuns ?? (_stuns = new List<Impedence>()); }
            set { _stuns = value; OnPropertyChanged("Stuns"); }
        }
        private List<Impedence> _moves;
        public List<Impedence> Moves
        {
            get { return _moves ?? (_moves = new List<Impedence>()); }
            set { _moves = value; OnPropertyChanged("Moves"); }
        }
        private List<Impedence> _fears;
        public List<Impedence> Fears
        {
            get { return _fears ?? (_fears = new List<Impedence>()); }
            set { _fears = value; OnPropertyChanged("Fears"); }
        }
        private List<Impedence> _roots;
        public List<Impedence> Roots
        {
            get { return _roots ?? (_roots = new List<Impedence>()); }
            set { _roots = value; OnPropertyChanged("Roots"); }
        }
        private List<Impedence> _disarms;
        public List<Impedence> Disarms
        {
            get { return _disarms ?? (_disarms = new List<Impedence>()); }
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
        public bool useBeastDuringBeastialWrath = false;
        public bool useKillCommand = true;
        public Aspect selectedAspect = Aspect.Dragonhawk;
        public AspectUsage aspectUsage = AspectUsage.ViperToOOM;
        public float gcdsToLayImmoTrap = 2.0f; // not editable
        public float gcdsToLayExploTrap = 2.0f; // not editable
        public float gcdsToVolley = 4.0f; // not editable, 6 seconds
        public Shots LALShotToUse = Shots.ExplosiveShot; // not editable
        public int LALShotsReplaced = 2; // not editable
        public float LALProcChance = 2; // not editable

        private float _SurvScale;
        public float SurvScale {
            get { return _SurvScale; }
            set { _SurvScale = value; OnPropertyChanged("SurvScale"); }
        }

        private bool _PTRMode = false;
        public bool PTRMode {
            get { return _PTRMode; }
            set { _PTRMode = value; OnPropertyChanged("PTRMode"); }
        }

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
