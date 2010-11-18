using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Xml.Serialization;

namespace Rawr.DPSWarr {
#if !SILVERLIGHT
    [Serializable]
#endif
    public class CalculationOptionsDPSWarr : ICalculationOptionBase, INotifyPropertyChanged
    {
        #region Constructors
        public CalculationOptionsDPSWarr()
        {
            SE_UseDur = true;
            UseMarkov = false;
            PTRMode = false;
            HideBadItems_Def = true;
            HideBadItems_Spl = true;
            HideBadItems_PvP = true;
            SurvScale = 1.0f;
            // Maintenance
            _Maintenance = new bool[] {
                true,  // == Rage Gen ==
                    true,  // Berserker Rage
                    true,  // Deadly Calm
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
                        true,  // Raging Blow
                    true,  // Arms
                        true,  // Bladestorm
                        true,  // Mortal Strike
                        true,  // Rend
                        true,  // Overpower
                        true,  // Taste for Blood
                        true,  // Colossus Smash
                        true,  // Victory Rush
                        true,  // Slam
                    true,  // <20% Execute Spamming
                    true,  // <20% Execute Spamming Stage 2
                true,  // == Rage Dumps ==
                    true,  // Cleave
                    true,  // Heroic Strike
                    true,  // Inner Rage
            };
            // Latency
            Lag = 179f;
            React = 220f;
            // Boss Options
        }
        #endregion
        #region Variables
        #region Basics
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
        private float _SurvScale;
        public float SurvScale
        {
            get { return _SurvScale; }
            set { _SurvScale = value; OnPropertyChanged("SurvScale"); }
        }
        #endregion
        #region Stat Graph
        private bool[] _statsList = new bool[] { true, true, true, true, true, true, true, true, true, true };
        public bool[] StatsList
        {
            get { return _statsList; }
            set { _statsList = value; OnPropertyChanged("StatsList"); }
        }
        private int _StatsIncrement = 100;
        public int StatsIncrement
        {
            get { return _StatsIncrement; }
            set { _StatsIncrement = value; OnPropertyChanged("StatsIncrement"); }
        }
        private string _calculationToGraph = "DPS Rating";
        public string CalculationToGraph
        {
            get { return _calculationToGraph; }
            set { _calculationToGraph = value; OnPropertyChanged("CalculationToGraph"); }
        }
        [XmlIgnore]
        public bool SG_STR { get { return StatsList[0]; } set { StatsList[0] = value; OnPropertyChanged("SG_STR"); } }
        [XmlIgnore]
        public bool SG_AGI { get { return StatsList[1]; } set { StatsList[1] = value; OnPropertyChanged("SG_AGI"); } }
        [XmlIgnore]
        public bool SG_AP { get { return StatsList[2]; } set { StatsList[2] = value; OnPropertyChanged("SG_AP"); } }
        [XmlIgnore]
        public bool SG_Crit { get { return StatsList[3]; } set { StatsList[3] = value; OnPropertyChanged("SG_Crit"); } }
        [XmlIgnore]
        public bool SG_Hit { get { return StatsList[4]; } set { StatsList[4] = value; OnPropertyChanged("SG_Hit"); } }
        [XmlIgnore]
        public bool SG_Exp { get { return StatsList[5]; } set { StatsList[5] = value; OnPropertyChanged("SG_Exp"); } }
        [XmlIgnore]
        public bool SG_Haste { get { return StatsList[6]; } set { StatsList[6] = value; OnPropertyChanged("SG_Haste"); } }
        [XmlIgnore]
        public bool SG_Mstr { get { return StatsList[7]; } set { StatsList[7] = value; OnPropertyChanged("SG_Mstr"); } }
        #endregion
        #region Abilities to Maintain
        private bool[] _Maintenance;
        public enum Maintenances
        {
            _RageGen__ = 0,
            BerserkerRage_,
            DeadlyCalm_,
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
            RagingBlow_,
            Arms_,
            Bladestorm_,
            MortalStrike_,
            Rend_,
            Overpower_,
            TasteForBlood_,
            ColossusSmash_,
            VictoryRush_,
            Slam_,
            ExecuteSpam_,
            ExecuteSpamStage2_,
            _RageDumps__,
            Cleave_,
            HeroicStrike_,
            InnerRage_,
        };
        public bool[] Maintenance
        {
            get {
                if (_Maintenance == null || _Maintenance.Length != (int)Maintenances.InnerRage_+1)
                {
                    _Maintenance = new bool[] {
                        true,  // == Rage Gen ==
                            true,  // Berserker Rage
                            true,  // Deadly Calm
                        false, // == Maintenance ==
                            false, // Shout Choice
                                false, // Battle Shout
                                false, // Commanding Shout
                            false, // Demoralizing Shout
                            false, // Sunder Armor
                            true,  // Thunder Clap
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
                                true,  // Raging Blow
                            true,  // Arms
                                true,  // Bladestorm
                                true,  // Mortal Strike
                                true,  // Rend
                                true,  // Overpower
                                true,  // Taste for Blood
                                true,  // Colossus Smash
                                true,  // Victory Rush
                                true,  // Slam
                            true,  // <20% Execute Spamming
                        true,  // == Rage Dumps ==
                            true,  // Cleave
                            true,  // Heroic Strike
                            true,  // Inner Rage
                    };
                }
                return _Maintenance;
            }
            set { _Maintenance = value; OnPropertyChanged("Maintenance"); }
        }
        [XmlIgnore]
        public bool M_BerserkerRage
        {
            get { return Maintenance[(int)Maintenances.BerserkerRage_]; }
            set { Maintenance[(int)Maintenances.BerserkerRage_] = value; OnPropertyChanged("M_BerserkerRage"); }
        }
        [XmlIgnore]
        public bool M_DeadlyCalm
        {
            get { return Maintenance[(int)Maintenances.DeadlyCalm_]; }
            set { Maintenance[(int)Maintenances.DeadlyCalm_] = value; OnPropertyChanged("M_DeadlyCalm"); }
        }
        [XmlIgnore]
        public bool M_BattleShout
        {
            get { return Maintenance[(int)Maintenances.BattleShout_]; }
            set { Maintenance[(int)Maintenances.BattleShout_] = value; OnPropertyChanged("M_BattleShout"); }
        }
        [XmlIgnore]
        public bool M_CommandingShout
        {
            get { return Maintenance[(int)Maintenances.CommandingShout_]; }
            set { Maintenance[(int)Maintenances.CommandingShout_] = value; OnPropertyChanged("M_CommandingShout"); }
        }
        [XmlIgnore]
        public bool M_DemoralizingShout
        {
            get { return Maintenance[(int)Maintenances.DemoralizingShout_]; }
            set { Maintenance[(int)Maintenances.DemoralizingShout_] = value; OnPropertyChanged("M_DemoralizingShout"); }
        }
        [XmlIgnore]
        public bool M_SunderArmor
        {
            get { return Maintenance[(int)Maintenances.SunderArmor_]; }
            set { Maintenance[(int)Maintenances.SunderArmor_] = value; OnPropertyChanged("M_SunderArmor"); }
        }
        [XmlIgnore]
        public bool M_ThunderClap
        {
            get { return Maintenance[(int)Maintenances.ThunderClap_]; }
            set { Maintenance[(int)Maintenances.ThunderClap_] = value; OnPropertyChanged("M_ThunderClap"); }
        }
        [XmlIgnore]
        public bool M_Hamstring
        {
            get { return Maintenance[(int)Maintenances.Hamstring_]; }
            set { Maintenance[(int)Maintenances.Hamstring_] = value; OnPropertyChanged("M_Hamstring"); }
        }
        [XmlIgnore]
        public bool M_ShatteringThrow
        {
            get { return Maintenance[(int)Maintenances.ShatteringThrow_]; }
            set { Maintenance[(int)Maintenances.ShatteringThrow_] = value; OnPropertyChanged("M_ShatteringThrow"); }
        }
        [XmlIgnore]
        public bool M_SweepingStrikes
        {
            get { return Maintenance[(int)Maintenances.SweepingStrikes_]; }
            set { Maintenance[(int)Maintenances.SweepingStrikes_] = value; OnPropertyChanged("M_SweepingStrikes"); }
        }
        [XmlIgnore]
        public bool M_DeathWish
        {
            get { return Maintenance[(int)Maintenances.DeathWish_]; }
            set { Maintenance[(int)Maintenances.DeathWish_] = value; OnPropertyChanged("M_DeathWish"); }
        }
        [XmlIgnore]
        public bool M_Recklessness
        {
            get { return Maintenance[(int)Maintenances.Recklessness_]; }
            set { Maintenance[(int)Maintenances.Recklessness_] = value; OnPropertyChanged("M_Recklessness"); }
        }
        [XmlIgnore]
        public bool M_EnragedRegeneration
        {
            get { return Maintenance[(int)Maintenances.EnragedRegeneration_]; }
            set { Maintenance[(int)Maintenances.EnragedRegeneration_] = value; OnPropertyChanged("M_EnragedRegeneration"); }
        }
        [XmlIgnore]
        public bool M_Whirlwind
        {
            get { return Maintenance[(int)Maintenances.Whirlwind_]; }
            set { Maintenance[(int)Maintenances.Whirlwind_] = value; OnPropertyChanged("M_Whirlwind"); }
        }
        [XmlIgnore]
        public bool M_Bloodthirst
        {
            get { return Maintenance[(int)Maintenances.Bloodthirst_]; }
            set { Maintenance[(int)Maintenances.Bloodthirst_] = value; OnPropertyChanged("M_Bloodthirst"); }
        }
        [XmlIgnore]
        public bool M_Bloodsurge
        {
            get { return Maintenance[(int)Maintenances.Bloodsurge_]; }
            set { Maintenance[(int)Maintenances.Bloodsurge_] = value; OnPropertyChanged("M_Bloodsurge"); }
        }
        [XmlIgnore]
        public bool M_RagingBlow
        {
            get { return Maintenance[(int)Maintenances.RagingBlow_]; }
            set { Maintenance[(int)Maintenances.RagingBlow_] = value; OnPropertyChanged("M_RagingBlow"); }
        }
        [XmlIgnore]
        public bool M_Bladestorm
        {
            get { return Maintenance[(int)Maintenances.Bladestorm_]; }
            set { Maintenance[(int)Maintenances.Bladestorm_] = value; OnPropertyChanged("M_Bladestorm"); }
        }
        [XmlIgnore]
        public bool M_MortalStrike
        {
            get { return Maintenance[(int)Maintenances.MortalStrike_]; }
            set { Maintenance[(int)Maintenances.MortalStrike_] = value; OnPropertyChanged("M_MortalStrike"); }
        }
        [XmlIgnore]
        public bool M_Rend
        {
            get { return Maintenance[(int)Maintenances.Rend_]; }
            set { Maintenance[(int)Maintenances.Rend_] = value; OnPropertyChanged("M_Rend"); }
        }
        [XmlIgnore]
        public bool M_Overpower
        {
            get { return Maintenance[(int)Maintenances.Overpower_]; }
            set { Maintenance[(int)Maintenances.Overpower_] = value; OnPropertyChanged("M_Overpower"); }
        }
        [XmlIgnore]
        public bool M_TasteForBlood
        {
            get { return Maintenance[(int)Maintenances.TasteForBlood_]; }
            set { Maintenance[(int)Maintenances.TasteForBlood_] = value; OnPropertyChanged("M_TasteForBlood"); }
        }
        [XmlIgnore]
        public bool M_ColossusSmash
        {
            get { return Maintenance[(int)Maintenances.ColossusSmash_]; }
            set { Maintenance[(int)Maintenances.ColossusSmash_] = value; OnPropertyChanged("M_ColossusSmash"); }
        }
        [XmlIgnore]
        public bool M_VictoryRush
        {
            get { return Maintenance[(int)Maintenances.VictoryRush_]; }
            set { Maintenance[(int)Maintenances.VictoryRush_] = value; OnPropertyChanged("M_VictoryRush"); }
        }
        [XmlIgnore]
        public bool M_Slam
        {
            get { return Maintenance[(int)Maintenances.Slam_]; }
            set { Maintenance[(int)Maintenances.Slam_] = value; OnPropertyChanged("M_Slam"); }
        }
        [XmlIgnore]
        public bool M_ExecuteSpam
        {
            get { return Maintenance[(int)Maintenances.ExecuteSpam_]; }
            set { Maintenance[(int)Maintenances.ExecuteSpam_] = value; OnPropertyChanged("M_ExecuteSpam"); }
        }
        [XmlIgnore]
        public bool M_ExecuteSpamStage2
        {
            get { return Maintenance[(int)Maintenances.ExecuteSpamStage2_] && M_ExecuteSpam; }
            set { Maintenance[(int)Maintenances.ExecuteSpamStage2_] = value; OnPropertyChanged("M_ExecuteSpamStage2"); }
        }
        [XmlIgnore]
        public bool M_Cleave
        {
            get { return Maintenance[(int)Maintenances.Cleave_]; }
            set { Maintenance[(int)Maintenances.Cleave_] = value; OnPropertyChanged("M_Cleave"); }
        }
        [XmlIgnore]
        public bool M_HeroicStrike
        {
            get { return Maintenance[(int)Maintenances.HeroicStrike_]; }
            set { Maintenance[(int)Maintenances.HeroicStrike_] = value; OnPropertyChanged("M_HeroicStrike"); }
        }
        [XmlIgnore]
        public bool M_InnerRage
        {
            get { return Maintenance[(int)Maintenances.InnerRage_]; }
            set { Maintenance[(int)Maintenances.InnerRage_] = value; OnPropertyChanged("M_InnerRage"); }
        }
        #endregion
        #region Latency
        private float _Lag;
        public float Lag
        {
            get { return _Lag; }
            set { _Lag = value; _cachedLatency = value / 1000f; OnPropertyChanged("Lag"); }
        }
        private float _React;
        public float React
        {
            get { return _React; }
            set { _React = value; _cachedAllowedReact = Math.Max(0f, (value - 200f) / 1000f); OnPropertyChanged("React"); }
        }
        [XmlIgnore]
        private float _cachedLatency = -1000000f;
        public float Latency { get { return _cachedLatency; } }
        [XmlIgnore]
        private float _cachedAllowedReact = -1000000f;
        public float AllowedReact { get { return _cachedAllowedReact; } }
        public float FullLatency { get { return AllowedReact + Latency; } }
        #endregion
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
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs(property)); }
        }
        #endregion
    }
}
