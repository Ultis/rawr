using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace Rawr.DPSWarr {
#if !SILVERLIGHT
	[Serializable]
#endif
	public class CalculationOptionsDPSWarr : ICalculationOptionBase
    {
        #region Constructors
        public CalculationOptionsDPSWarr()
        {
		    FilterType = "Content";
		    Filter = "All";
		    BossName = "Custom";
		    TargetLevel = 83;
		    TargetArmor = StatConversion.NPC_ARMOR[83 - 80];
            TargetHP = 1000000f;
		    Duration = 300f;
		    FuryStance = true;
		    AllowFlooring = false;
            SE_UseDur = true;
            UseMarkov = false;
		    PTRMode = false;
            //HideBadItems = true;
            HideBadItems_Def = true;
            HideBadItems_Spl = true;
            HideBadItems_PvP = true;
            HideProfEnchants = false;
            SurvScale = 1.0f;
            Under20Perc = 0.17f;
		    // Rotational Changes
		    InBack           = true ; InBackPerc           = 100;
		    MultipleTargets  = false; MultipleTargetsPerc  =  25; MultipleTargetsMax  =    3;
		    MovingTargets    = false; //MovingTargetsFreq    = 120; MovingTargetsDur    = 5000;
		    StunningTargets  = false; StunningTargetsFreq  = 120; StunningTargetsDur  = 5000;
            FearingTargets   = false; //FearingTargetsFreq   = 120; FearingTargetsDur   = 5000;
            RootingTargets   = false; RootingTargetsFreq   = 120; RootingTargetsDur   = 5000;
		    DisarmingTargets = false; DisarmingTargetsFreq = 120; DisarmingTargetsDur = 5000;// nonfunctional
            AoETargets       = false; AoETargetsFreq       =  20; AoETargetsDMG       = 5000;
            // Maintenance
            _Maintenance = new bool[] {
                true,  // == Rage Gen ==
                    true,  // Berserker Rage
                    true,  // Bloodrage
                false, // == Maintenance ==
                    false, // Shout Choice
                        false, // Battle Shout
                        false, // Commanding Shout
                    false, // Demoralizing Shout
                    false, // Sunder Armor
                    false, // Thunder Clap
                    false, // Hamstring
                true,  // == Periodics ==
                    true,  // Shattering Throw
                    true,  // Sweeping Strikes
                    true,  // DeathWish
                    true,  // Recklessness
                    false, // Enraged Regeneration
                true,  // == Damage Dealers ==
                    true,  // Fury
                        true,  // Whirlwind
                        true,  // Bloodthirst
                        true,  // Bloodsurge
                    true,  // Arms
                        true,  // Bladestorm
                        true,  // Mortal Strike
                        true,  // Rend
                        true,  // Overpower
                        true,  // Taste for Blood
                        true,  // Sudden Death
                        true,  // Slam
                    true,  // <20% Execute Spamming
                true,  // == Rage Dumps ==
                    true,  // Cleave
                    true   // Heroic Strike
            };
            // Latency
		    Lag = 179f;
		    React = 220f;
        }
        #endregion
        #region Variables
        #region Basics
        private string _FilterType;
        public string FilterType
        {
            get { return _FilterType; }
            set { _FilterType = value; OnPropertyChanged("FilterType"); }
        }
        private string _Filter;
        public string Filter
        {
            get { return _Filter; }
            set { _Filter = value; OnPropertyChanged("Filter"); }
        }
        private string _BossName;
        public string BossName
        {
            get { return _BossName; }
            set { _BossName = value; OnPropertyChanged("BossName"); }
        }
        private int _TargetLevel;
        public int TargetLevel
        {
            get { return _TargetLevel; }
            set { _TargetLevel = value; OnPropertyChanged("TargetLevel"); }
        }
        private float _TargetArmor;
        public float TargetArmor
        {
            get { return _TargetArmor; }
            set { _TargetArmor = value; OnPropertyChanged("TargetArmor"); }
        }
        private float _TargetHP;
        public float TargetHP
        {
            get { return _TargetHP; }
            set { _TargetHP = value; OnPropertyChanged("TargetHP"); }
        }
        private float _Duration;
        public float Duration
        {
            get { return _Duration; }
            set { _Duration = value; OnPropertyChanged("Duration"); }
        }
        private bool _FuryStance;
        public bool FuryStance
        {
            get { return _FuryStance; }
            set { _FuryStance = value; OnPropertyChanged("FuryStance"); }
        }
        private bool _AllowFlooring;
        public bool AllowFlooring
        {
            get { return _AllowFlooring; }
            set { _AllowFlooring = value; OnPropertyChanged("AllowFlooring"); }
        }
        private bool _SE_UseDur;
        public bool SE_UseDur
        {
            get { return _SE_UseDur; }
            set { _SE_UseDur = value; OnPropertyChanged("SE_UseDur"); }
        }
        private bool _UseMarkov;
        public bool UseMarkov
        {
            get { return _UseMarkov; }
            set { _UseMarkov = value; OnPropertyChanged("UseMarkov"); }
        }
        private bool _PTRMode;
        public bool PTRMode
        {
            get { return _PTRMode; }
            set { _PTRMode = value; OnPropertyChanged("PTRMode"); }
        }
        private bool _HideBadItems_Def;
        public bool HideBadItems_Def
        {
            get { return _HideBadItems_Def; }
            set { _HideBadItems_Def = value; OnPropertyChanged("HideBadItems_Def"); }
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
        private float _SurvScale;
        public float SurvScale
        {
            get { return _SurvScale; }
            set { _SurvScale = value; OnPropertyChanged("SurvScale"); }
        }
        private float _Under20Perc;
        public float Under20Perc
        {
            get { return _Under20Perc; }
            set { _Under20Perc = value; OnPropertyChanged("Under20Perc"); }
        }
        #endregion
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
        #region Abilities to Maintain
        private bool[] _Maintenance;
        public bool[] Maintenance
        {
            get {
                return _Maintenance ??
                    (_Maintenance = new bool[] {
                        true,  // == Rage Gen ==
                            true,  // Berserker Rage
                            true,  // Bloodrage
                        false, // == Maintenance ==
                            false, // Shout Choice
                                false, // Battle Shout
                                false, // Commanding Shout
                            false, // Demoralizing Shout
                            false, // Sunder Armor
                            false, // Thunder Clap
                            false, // Hamstring
                        true,  // == Periodics ==
                            true,  // Shattering Throw
                            true,  // Sweeping Strikes
                            true,  // DeathWish
                            true,  // Recklessness
                            false, // Enraged Regeneration
                        true,  // == Damage Dealers ==
                            true,  // Fury
                                true,  // Whirlwind
                                true,  // Bloodthirst
                                true,  // Bloodsurge
                            true,  // Arms
                                true,  // Bladestorm
                                true,  // Mortal Strike
                                true,  // Rend
                                true,  // Overpower
                                true,  // Taste for Blood
                                true,  // Sudden Death
                                true,  // Slam
                            true,  // <20% Execute Spamming
                        true,  // == Rage Dumps ==
                            true,  // Cleave
                            true   // Heroic Strike
                    });
            }
            set { _Maintenance = value; OnPropertyChanged("Maintenance"); }
        }
		public enum Maintenances {
			_RageGen__ = 0,
                BerserkerRage_,
                Bloodrage_,
			_Maintenance__,
                ShoutChoice_,
                    BattleShout_,
                    CommandingShout_,
                DemoralizingShout_,
                SunderArmor_,
                ThunderClap_,
                Hamstring_,
			_Periodics__,
                ShatteringThrow_,
                SweepingStrikes_,
                DeathWish_,
                Recklessness_,
                EnragedRegeneration_,
            _DamageDealers__,
                Fury_,
                    Whirlwind_,
                    Bloodthirst_,
                    Bloodsurge_,
                Arms_,
                    Bladestorm_,
                    MortalStrike_,
                    Rend_,
                    Overpower_,
                    TasteForBlood_,
                    SuddenDeath_,
                    Slam_,
                ExecuteSpam_,
			_RageDumps__,
                Cleave_,
                HeroicStrike_
		};
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
		public WarriorTalents talents = null;
        #endregion
        #region Functions
        public string GetXml() {
			var s = new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptionsDPSWarr));
			var xml = new StringBuilder();
			var sw = new System.IO.StringWriter(xml);
			s.Serialize(sw, this);
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
