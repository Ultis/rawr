using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Text;

namespace Rawr.DPSWarr {
    public class CalculationOptionsDPSWarr : ICalculationOptionBase {
        public int TargetLevel = 83;
        public int TargetArmor = (int)StatConversion.NPC_BOSS_ARMOR;
        public float Duration = 300f;
        public bool FuryStance = true;
        public bool _3pt2Mode = false;
        // Rotational Changes
        public bool MultipleTargets  = false; public int MultipleTargetsPerc  = 100;
        public bool MovingTargets    = false; public int MovingTargetsPerc    = 100;
        public bool StunningTargets  = false; public int StunningTargetsPerc  = 100;
		public bool DisarmingTargets = false; public int DisarmingTargetsPerc = 100;
        public bool InBack           = true ; public int InBackPerc           = 100;
        // Abilities to Maintain
        public bool[] Maintenance = new bool[] {
            true,  // == Rage Gen ==
            true,  // Berserker Rage
            true,  // Bloodrage
            false, // == Maintenance ==
            false, // Battle Shout
            false, // Demoralizing Shout
            false, // Sunder Armor
            false, // Thunder Clap
            false, // Hamstring
            true,  // == Periodics ==
            true,  // Shattering Throw
            true,  // Sweeping Strikes
            true,  // DeathWish
            true,  // Recklessness
            true,  // == Damage Dealers ==
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
        public enum Maintenances : int {
            _RageGen__ = 0,   BerserkerRage_,   Bloodrage_,
            _Maintenance__,   BattleShout_,     DemoralizingShout_, SunderArmor_, ThunderClap_,  Hamstring_,
            _Periodics__,     ShatteringThrow_, SweepingStrikes_,   DeathWish_,   Recklessness_,
            _DamageDealers__, Bladestorm_,      MortalStrike_,      Rend_,        Overpower_,    SuddenDeath_, Slam_,
            _RageDumps__,     Cleave_,          HeroicStrike_
        };
        // Latency
        public float Lag = 179f;
        public float React = 220f;
        public float GetReact() { return (float)Math.Max(0f, React - 250f); }
        public float GetLatency() { return (Lag + GetReact()) / 1000f; }
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