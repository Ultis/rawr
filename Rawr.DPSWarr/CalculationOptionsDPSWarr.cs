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
        #region Variables
        #region Basics
        [DefaultValue(true)]
        public bool SE_UseDur { get { return _SE_UseDur; } set { _SE_UseDur = value; OnPropertyChanged("SE_UseDur"); } }
        private bool _SE_UseDur;
        [DefaultValue(false)]
        public bool UseMarkov { get { return _UseMarkov; } set { _UseMarkov = value; OnPropertyChanged("UseMarkov"); } }
        private bool _UseMarkov;
        [DefaultValue(false)]
        public bool PtrMode { get { return _PtrMode; } set { _PtrMode = value; OnPropertyChanged("PTRMode"); } }
        private bool _PtrMode;
        [DefaultValue(true)]
        public bool HideBadItems_Def { get { return _HideBadItems_Def; } set { _HideBadItems_Def = value; OnPropertyChanged("HideBadItems_Def"); } }
        private bool _HideBadItems_Def;
        [DefaultValue(true)]
        public bool HideBadItems_Spl { get { return _HideBadItems_Spl; } set { _HideBadItems_Spl = value; OnPropertyChanged("HideBadItems_Spl"); } }
        private bool _HideBadItems_Spl;
        [DefaultValue(true)]
        public bool HideBadItems_PvP { get { return _HideBadItems_PvP; } set { _HideBadItems_PvP = value; OnPropertyChanged("HideBadItems_PvP"); } }
        private bool _HideBadItems_PvP;
        [DefaultValue(1.0f)]
        public float SurvScale { get { return _SurvScale; } set { _SurvScale = value; OnPropertyChanged("SurvScale"); } }
        private float _SurvScale;
        #endregion
        #region Stat Graph
        [DefaultValue(new bool[] { true, true, true, true, true, true, true, true, true, true, true })]
        public bool[] StatsList { get { return _statsList; } set { _statsList = value; OnPropertyChanged("StatsList"); } }
        private bool[] _statsList = new bool[] { true, true, true, true, true, true, true, true, true, true, true };
        [DefaultValue(100)]
        public int StatsIncrement { get { return _StatsIncrement; } set { _StatsIncrement = value; OnPropertyChanged("StatsIncrement"); } }
        private int _StatsIncrement = 100;
        [DefaultValue("DPS Rating")]
        public string CalculationToGraph { get { return _calculationToGraph; } set { _calculationToGraph = value; OnPropertyChanged("CalculationToGraph"); } }
        private string _calculationToGraph = "DPS Rating";
        [XmlIgnore]
        public bool SG_Str { get { return StatsList[0]; } set { StatsList[0] = value; OnPropertyChanged("SG_STR"); } }
        [XmlIgnore]
        public bool SG_Agi { get { return StatsList[1]; } set { StatsList[1] = value; OnPropertyChanged("SG_AGI"); } }
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
        [XmlIgnore]
        public bool SG_Rage { get { return StatsList[8]; } set { StatsList[8] = value; OnPropertyChanged("SG_Rage"); } }
        #endregion
        #region Abilities to Maintain
        private bool[] _Maintenance;
        public bool[] MaintenanceTree
        {
            get {
                if (_Maintenance == null || _Maintenance.Length != (int)Maintenance.InnerRage+1)
                {
                    _Maintenance = new bool[] {
                        true,  // == Rage Gen ==
                            true,  // Start with a Charge
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
                                false,  // Whirlwind
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
        public bool M_StartWithCharge
        {
            get { return MaintenanceTree[(int)Maintenance.StartWithCharge]; }
            set { MaintenanceTree[(int)Maintenance.StartWithCharge] = value; OnPropertyChanged("M_StartWithCharge"); }
        }
        [XmlIgnore]
        public bool M_BerserkerRage
        {
            get { return MaintenanceTree[(int)Maintenance.BerserkerRage]; }
            set { MaintenanceTree[(int)Maintenance.BerserkerRage] = value; OnPropertyChanged("M_BerserkerRage"); }
        }
        [XmlIgnore]
        public bool M_DeadlyCalm
        {
            get { return MaintenanceTree[(int)Maintenance.DeadlyCalm]; }
            set { MaintenanceTree[(int)Maintenance.DeadlyCalm] = value; OnPropertyChanged("M_DeadlyCalm"); }
        }
        [XmlIgnore]
        public bool M_BattleShout
        {
            get { return MaintenanceTree[(int)Maintenance.BattleShout]; }
            set { MaintenanceTree[(int)Maintenance.BattleShout] = value; OnPropertyChanged("M_BattleShout"); }
        }
        [XmlIgnore]
        public bool M_CommandingShout
        {
            get { return MaintenanceTree[(int)Maintenance.CommandingShout]; }
            set { MaintenanceTree[(int)Maintenance.CommandingShout] = value; OnPropertyChanged("M_CommandingShout"); }
        }
        [XmlIgnore]
        public bool M_DemoralizingShout
        {
            get { return MaintenanceTree[(int)Maintenance.DemoralizingShout]; }
            set { MaintenanceTree[(int)Maintenance.DemoralizingShout] = value; OnPropertyChanged("M_DemoralizingShout"); }
        }
        [XmlIgnore]
        public bool M_SunderArmor
        {
            get { return MaintenanceTree[(int)Maintenance.SunderArmor]; }
            set { MaintenanceTree[(int)Maintenance.SunderArmor] = value; OnPropertyChanged("M_SunderArmor"); }
        }
        [XmlIgnore]
        public bool M_ThunderClap
        {
            get { return MaintenanceTree[(int)Maintenance.ThunderClap]; }
            set { MaintenanceTree[(int)Maintenance.ThunderClap] = value; OnPropertyChanged("M_ThunderClap"); }
        }
        [XmlIgnore]
        public bool M_Hamstring
        {
            get { return MaintenanceTree[(int)Maintenance.Hamstring]; }
            set { MaintenanceTree[(int)Maintenance.Hamstring] = value; OnPropertyChanged("M_Hamstring"); }
        }
        [XmlIgnore]
        public bool M_ShatteringThrow
        {
            get { return MaintenanceTree[(int)Maintenance.ShatteringThrow]; }
            set { MaintenanceTree[(int)Maintenance.ShatteringThrow] = value; OnPropertyChanged("M_ShatteringThrow"); }
        }
        [XmlIgnore]
        public bool M_SweepingStrikes
        {
            get { return MaintenanceTree[(int)Maintenance.SweepingStrikes]; }
            set { MaintenanceTree[(int)Maintenance.SweepingStrikes] = value; OnPropertyChanged("M_SweepingStrikes"); }
        }
        [XmlIgnore]
        public bool M_DeathWish
        {
            get { return MaintenanceTree[(int)Maintenance.DeathWish]; }
            set { MaintenanceTree[(int)Maintenance.DeathWish] = value; OnPropertyChanged("M_DeathWish"); }
        }
        [XmlIgnore]
        public bool M_Recklessness
        {
            get { return MaintenanceTree[(int)Maintenance.Recklessness]; }
            set { MaintenanceTree[(int)Maintenance.Recklessness] = value; OnPropertyChanged("M_Recklessness"); }
        }
        [XmlIgnore]
        public bool M_EnragedRegeneration
        {
            get { return MaintenanceTree[(int)Maintenance.EnragedRegeneration]; }
            set { MaintenanceTree[(int)Maintenance.EnragedRegeneration] = value; OnPropertyChanged("M_EnragedRegeneration"); }
        }
        [XmlIgnore]
        public bool M_Whirlwind
        {
            get { return MaintenanceTree[(int)Maintenance.Whirlwind]; }
            set { MaintenanceTree[(int)Maintenance.Whirlwind] = value; OnPropertyChanged("M_Whirlwind"); }
        }
        [XmlIgnore]
        public bool M_Bloodthirst
        {
            get { return MaintenanceTree[(int)Maintenance.Bloodthirst]; }
            set { MaintenanceTree[(int)Maintenance.Bloodthirst] = value; OnPropertyChanged("M_Bloodthirst"); }
        }
        [XmlIgnore]
        public bool M_Bloodsurge
        {
            get { return MaintenanceTree[(int)Maintenance.Bloodsurge]; }
            set { MaintenanceTree[(int)Maintenance.Bloodsurge] = value; OnPropertyChanged("M_Bloodsurge"); }
        }
        [XmlIgnore]
        public bool M_RagingBlow
        {
            get { return MaintenanceTree[(int)Maintenance.RagingBlow]; }
            set { MaintenanceTree[(int)Maintenance.RagingBlow] = value; OnPropertyChanged("M_RagingBlow"); }
        }
        [XmlIgnore]
        public bool M_Bladestorm
        {
            get { return MaintenanceTree[(int)Maintenance.Bladestorm]; }
            set { MaintenanceTree[(int)Maintenance.Bladestorm] = value; OnPropertyChanged("M_Bladestorm"); }
        }
        [XmlIgnore]
        public bool M_MortalStrike
        {
            get { return MaintenanceTree[(int)Maintenance.MortalStrike]; }
            set { MaintenanceTree[(int)Maintenance.MortalStrike] = value; OnPropertyChanged("M_MortalStrike"); }
        }
        [XmlIgnore]
        public bool M_Rend
        {
            get { return MaintenanceTree[(int)Maintenance.Rend]; }
            set { MaintenanceTree[(int)Maintenance.Rend] = value; OnPropertyChanged("M_Rend"); }
        }
        [XmlIgnore]
        public bool M_Overpower
        {
            get { return MaintenanceTree[(int)Maintenance.Overpower]; }
            set { MaintenanceTree[(int)Maintenance.Overpower] = value; OnPropertyChanged("M_Overpower"); }
        }
        [XmlIgnore]
        public bool M_TasteForBlood
        {
            get { return MaintenanceTree[(int)Maintenance.TasteForBlood]; }
            set { MaintenanceTree[(int)Maintenance.TasteForBlood] = value; OnPropertyChanged("M_TasteForBlood"); }
        }
        [XmlIgnore]
        public bool M_ColossusSmash
        {
            get { return MaintenanceTree[(int)Maintenance.ColossusSmash]; }
            set { MaintenanceTree[(int)Maintenance.ColossusSmash] = value; OnPropertyChanged("M_ColossusSmash"); }
        }
        [XmlIgnore]
        public bool M_VictoryRush
        {
            get { return MaintenanceTree[(int)Maintenance.VictoryRush]; }
            set { MaintenanceTree[(int)Maintenance.VictoryRush] = value; OnPropertyChanged("M_VictoryRush"); }
        }
        [XmlIgnore]
        public bool M_Slam
        {
            get { return MaintenanceTree[(int)Maintenance.Slam]; }
            set { MaintenanceTree[(int)Maintenance.Slam] = value; OnPropertyChanged("M_Slam"); }
        }
        [XmlIgnore]
        public bool M_ExecuteSpam
        {
            get { return MaintenanceTree[(int)Maintenance.ExecuteSpam]; }
            set { MaintenanceTree[(int)Maintenance.ExecuteSpam] = value; OnPropertyChanged("M_ExecuteSpam"); }
        }
        [XmlIgnore]
        public bool M_ExecuteSpamStage2
        {
            get { return MaintenanceTree[(int)Maintenance.ExecuteSpamStage2] && M_ExecuteSpam; }
            set { MaintenanceTree[(int)Maintenance.ExecuteSpamStage2] = value; OnPropertyChanged("M_ExecuteSpamStage2"); }
        }
        [XmlIgnore]
        public bool M_Cleave
        {
            get { return MaintenanceTree[(int)Maintenance.Cleave]; }
            set { MaintenanceTree[(int)Maintenance.Cleave] = value; OnPropertyChanged("M_Cleave"); }
        }
        [XmlIgnore]
        public bool M_HeroicStrike
        {
            get { return MaintenanceTree[(int)Maintenance.HeroicStrike]; }
            set { MaintenanceTree[(int)Maintenance.HeroicStrike] = value; OnPropertyChanged("M_HeroicStrike"); }
        }
        [XmlIgnore]
        public bool M_InnerRage
        {
            get { return MaintenanceTree[(int)Maintenance.InnerRage]; }
            set { MaintenanceTree[(int)Maintenance.InnerRage] = value; OnPropertyChanged("M_InnerRage"); }
        }
        #endregion
        #region Latency
        [DefaultValue(179f)]
        public float Lag { get { return _Lag; } set { _Lag = value; _cachedLatency = value / 1000f; OnPropertyChanged("Lag"); } }
        private float _Lag;
        [DefaultValue(220f)]
        public float React { get { return _React; } set { _React = value; _cachedAllowedReact = Math.Max(0f, (value - 200f) / 1000f); OnPropertyChanged("React"); } }
        private float _React;
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
            StringBuilder xml = null;
            System.IO.StringWriter sw = null;
            try {
                var s = new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptionsDPSWarr));
                xml = new StringBuilder();
                sw = new System.IO.StringWriter(xml);
                s.Serialize(sw, this);
            } finally { sw.Dispose(); }
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
