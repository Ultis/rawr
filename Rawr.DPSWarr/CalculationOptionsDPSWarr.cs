using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.DPSWarr {
#if !SILVERLIGHT
	[Serializable]
#endif
	public class CalculationOptionsDPSWarr : ICalculationOptionBase {
		public string FilterType = "Content";
		public string Filter = "All";
		public string BossName = "Custom";
		public int TargetLevel = 83;
		public float TargetArmor = StatConversion.NPC_ARMOR[83 - 80];
        public float TargetHP = 1000000f;
		public float Duration = 300f;
		public bool FuryStance = true;
		public bool AllowFlooring = false;
        public bool SE_UseDur = true;
		public bool PTRMode = false;
        public bool HideBadItems = true;
        public bool HideBadItems_Def = true;
        public bool HideBadItems_Spl = true;
        public bool HideBadItems_PvP = true;
        public bool HideProfEnchants = false;
        public float SurvScale = 1.0f;
        public float Under20Perc = 0.17f;
		// Rotational Changes
		public bool InBack           = true ; public int InBackPerc           = 100;
		public bool MultipleTargets  = false; public int MultipleTargetsPerc  =  25; public float MultipleTargetsMax  =    3;
		public bool MovingTargets    = false; //public int MovingTargetsFreq    = 120; public float MovingTargetsDur    = 5000;
		public bool StunningTargets  = false; public int StunningTargetsFreq  = 120; public float StunningTargetsDur  = 5000;
        public bool FearingTargets   = false; public int FearingTargetsFreq   = 120; public float FearingTargetsDur   = 5000;
        public bool RootingTargets   = false; public int RootingTargetsFreq   = 120; public float RootingTargetsDur   = 5000;
		public bool DisarmingTargets = false; public int DisarmingTargetsFreq = 120; public float DisarmingTargetsDur = 5000;// nonfunctional
        public bool AoETargets       = false; public int AoETargetsFreq       =  20; public float AoETargetsDMG       = 5000;
        private List<Stun> _stuns;
        public List<Stun> Stuns
        {
            get { return _stuns ?? (_stuns = new List<Stun>()); }
            set { _stuns = value; }
        }
        private List<Move> _moves;
        public List<Move> Moves
        {
            get { return _moves ?? (_moves = new List<Move>()); }
            set { _moves = value; }
        }
        private List<Fear> _fears;
        public List<Fear> Fears
        {
            get { return _fears ?? (_fears = new List<Fear>()); }
            set { _fears = value; }
        }
        private List<Root> _roots;
        public List<Root> Roots
        {
            get { return _roots ?? (_roots = new List<Root>()); }
            set { _roots = value; }
        }
        private List<Disarm> _disarms;
        public List<Disarm> Disarms
        {
            get { return _disarms ?? (_disarms = new List<Disarm>()); }
            set { _disarms = value; }
        }
        // Abilities to Maintain
		public bool[] Maintenance = new bool[] {
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
                true,  // Enraged Regeneration
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
		public enum Maintenances : int {
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
		// Latency
		public float Lag = 179f;
		public float React = 220f;
        public float Latency { get { return Lag / 1000f; } }
        public float AllowedReact { get { return Math.Max(0f, (React - 250f) / 1000f); } }
        public float FullLatency { get { return AllowedReact + Latency; } }
		//
		public WarriorTalents talents = null;
		public string GetXml() {
			var s = new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptionsDPSWarr));
			var xml = new StringBuilder();
			var sw = new System.IO.StringWriter(xml);
			s.Serialize(sw, this);
			return xml.ToString();
		}
	}
}
