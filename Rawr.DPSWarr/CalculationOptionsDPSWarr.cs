using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.DPSWarr
{
#if !SILVERLIGHT
	[Serializable]
#endif
	public class CalculationOptionsDPSWarr : ICalculationOptionBase
	{
		public string FilterType = "Content";
		public string Filter = "All";
		public string BossName = "Custom";
		public int TargetLevel = 83;
		public int TargetArmor = (int)StatConversion.NPC_ARMOR[83 - 80];
		public float Duration = 300f;
		public bool FuryStance = true;
		public bool AllowFlooring = false;
		public bool PTRMode = false;
		public float SurvScale = 1.0f;
		// Rotational Changes
		public bool InBack = true; public int InBackPerc = 100;
		public bool MultipleTargets = false; public int MultipleTargetsPerc = 25; public float MultipleTargetsMax = 3;
		public bool StunningTargets = false; public int StunningTargetsFreq = 120; public float StunningTargetsDur = 5000;
		public bool MovingTargets = false; public float MovingTargetsTime = 0;
		// nonfunctional
		public bool DisarmingTargets = false; public int DisarmingTargetsPerc = 100;
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
                    true,  // Sudden Death
                    true,  // Slam
            true,  // == Rage Dumps ==
                true,  // Cleave
                true   // Heroic Strike
        };
		public enum Maintenances : int
		{
			_RageGen__ = 0, BerserkerRage_, Bloodrage_,
			_Maintenance__, ShoutChoice_, BattleShout_, CommandingShout_, DemoralizingShout_, SunderArmor_, ThunderClap_, Hamstring_,
			_Periodics__, ShatteringThrow_, SweepingStrikes_, DeathWish_, Recklessness_, EnragedRegeneration_,
			_DamageDealers__, Fury_, Whirlwind_, Bloodthirst_, Bloodsurge_, Arms_, Bladestorm_, MortalStrike_, Rend_, Overpower_, SuddenDeath_, Slam_,
			_RageDumps__, Cleave_, HeroicStrike_
		};
		// Latency
		public float Lag = 179f;
		public float React = 220f;
		public float GetReact() { return (float)Math.Max(0f, React - 250f); }
		public float GetLatency() { return (Lag + GetReact()) / 1000f; }
		//
		public WarriorTalents talents = null;
		public string GetXml()
		{
			var s = new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptionsDPSWarr));
			var xml = new StringBuilder();
			var sw = new System.IO.StringWriter(xml);
			s.Serialize(sw, this);
			return xml.ToString();
		}
	}
}
