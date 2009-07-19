using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Xml;

/* Things to add:
 * 
 * Full Boss Handling System
 * Custom Rotation Priority
 * Target Damage
 * Target Attack Speed
 * Threat Value/Weight
 * Pot Usage (Needs to pull GCDs)
 * Healing Recieved
 * Max # of Targets
 * % of fight with mob under 20% HP (activates Execute Spamming)
 * Vigilance Threat pulling
 */

namespace Rawr.DPSWarr {
    public partial class CalculationOptionsPanelDPSWarr : CalculationOptionsPanelBase {
        private readonly Dictionary<int, string> armorBosses = new Dictionary<int, string>();
        public CalculationOptionsPanelDPSWarr() {
            InitializeComponent();

            armorBosses.Add((int)StatConversion.NPC_BOSS_ARMOR, "Default Boss Armor");
            armorBosses.Add(10900, "Patchwerk");
            armorBosses.Add(12000, "Grobbulus");
            armorBosses.Add(13083, "-");

            CB_TargArmor.DisplayMember = "Key";
            CB_TargArmor.DataSource = new BindingSource(armorBosses, null);

            CB_TargLvl.DataSource = new[] {83, 82, 81, 80};
            CB_Duration.Minimum = 0;
            CB_Duration.Maximum = 60*20; // 20 minutes

            RB_StanceFury.Checked = true;
        }
        protected override void LoadCalculationOptions() {
            if (Character != null && Character.CalculationOptions == null) { 
                Character.CalculationOptions = new CalculationOptionsDPSWarr();
                CalculationOptionsDPSWarr opts = Character.CalculationOptions as CalculationOptionsDPSWarr;
                opts.FuryStance = (Character.WarriorTalents.TitansGrip == 1);
                // Rotational Changes
                RB_StanceFury.Checked = opts.FuryStance;
                RB_StanceArms.Checked = !RB_StanceFury.Checked;
                // Abilities to Maintain
                // Latency
                CB_Lag.Value = (int)opts.Lag;
                CB_React.Value = (int)opts.React;
                //
                Character.OnCalculationsInvalidated();
                return; 
            }
            CalculationOptionsDPSWarr calcOpts = Character.CalculationOptions as CalculationOptionsDPSWarr;
            if (calcOpts != null) {
                CB_TargLvl.Text = calcOpts.TargetLevel.ToString();
                CB_TargArmor.Text = calcOpts.TargetArmor.ToString();
                CB_Duration.Value = (decimal)calcOpts.Duration;
                RB_StanceArms.Checked    = !calcOpts.FuryStance;
                // Rotational Changes
                CK_MultiTargs.Checked    = calcOpts.MultipleTargets;   CB_MultiTargsPerc.Value     = calcOpts.MultipleTargetsPerc;
                CK_MovingTargs.Checked   = calcOpts.MovingTargets;     CB_MoveTargsPerc.Value      = calcOpts.MovingTargetsPerc;
                CK_StunningTargs.Checked = calcOpts.StunningTargets;   CB_StunningTargsPerc.Value  = calcOpts.StunningTargetsPerc;
                CK_DisarmTargs.Checked   = calcOpts.DisarmingTargets;  CB_DisarmingTargsPerc.Value = calcOpts.DisarmingTargetsPerc;
                CK_InBack.Checked        = calcOpts.InBack;            CB_InBackPerc.Value         = calcOpts.InBackPerc;
                // Abilities to Maintain
                for (int i = 0; i < CLB_Maints.Items.Count; i++) {
                    CLB_Maints.SetItemChecked(i, calcOpts.Maintenance[i]);
                }
                // Latency
                CB_Lag.Value   = (int)calcOpts.Lag;
                CB_React.Value = (int)calcOpts.React;
                //
                if (Character != null) {
                    CalculationOptionsDPSWarr opts = Character.CalculationOptions as CalculationOptionsDPSWarr;
                    opts.FuryStance = (Character.WarriorTalents.TitansGrip == 1);
                    RB_StanceFury.Checked = opts.FuryStance;
                    RB_StanceArms.Checked = !RB_StanceFury.Checked;
                    Character.OnCalculationsInvalidated();
                }
            }
        }
        // Basics
        private void CB_ArmorBosses_SelectedIndexChanged(object sender, EventArgs e) {
            int targetArmor = int.Parse(CB_TargArmor.Text);
            //LB_TargArmorDesc.Text = armorBosses[targetArmor];

            if (Character != null && Character.CalculationOptions != null) {
                CalculationOptionsDPSWarr calcOpts = Character.CalculationOptions as CalculationOptionsDPSWarr;
                calcOpts.TargetArmor = targetArmor;
                Character.OnCalculationsInvalidated();
            }
        }
        private void CB_TargetLevel_SelectedIndexChanged(object sender, EventArgs e) {
            if (Character != null && Character.CalculationOptions != null) {
                CalculationOptionsDPSWarr calcOpts = Character.CalculationOptions as CalculationOptionsDPSWarr;
                calcOpts.TargetLevel = int.Parse(CB_TargLvl.Text);
                Character.OnCalculationsInvalidated();
            }
        }
        private void RB_StanceFury_CheckedChanged(object sender, EventArgs e) {
            CalculationOptionsDPSWarr calcOpts = Character.CalculationOptions as CalculationOptionsDPSWarr;
            calcOpts.FuryStance = RB_StanceFury.Checked;
            Character.OnCalculationsInvalidated();
        }
        private void CB_Duration_ValueChanged(object sender, EventArgs e) {
            CalculationOptionsDPSWarr calcOpts = Character.CalculationOptions as CalculationOptionsDPSWarr;
            calcOpts.Duration = (float)CB_Duration.Value;
            Character.OnCalculationsInvalidated();
        }
        // Rotational Changes
        private void RotChanges_ChecksChanged(object sender, EventArgs e) {
            CalculationOptionsDPSWarr calcOpts = Character.CalculationOptions as CalculationOptionsDPSWarr;
            //
            calcOpts.MultipleTargets = CK_MultiTargs.Checked; CB_MultiTargsPerc.Enabled = calcOpts.MultipleTargets;
            calcOpts.MovingTargets = CK_MovingTargs.Checked; CB_MoveTargsPerc.Enabled = calcOpts.MovingTargets;
            calcOpts.StunningTargets = CK_StunningTargs.Checked; CB_StunningTargsPerc.Enabled = calcOpts.StunningTargets;
            calcOpts.DisarmingTargets = CK_DisarmTargs.Checked; CB_DisarmingTargsPerc.Enabled = calcOpts.DisarmingTargets;
            calcOpts.InBack = CK_InBack.Checked; CB_InBackPerc.Enabled = calcOpts.InBack;
            //
            Character.OnCalculationsInvalidated();
        }
        private void RotChanges_ValuesChanged(object sender, EventArgs e) {
            CalculationOptionsDPSWarr calcOpts = Character.CalculationOptions as CalculationOptionsDPSWarr;
            //
            calcOpts.MultipleTargetsPerc = (int)CB_MultiTargsPerc.Value;
            calcOpts.MovingTargetsPerc = (int)CB_MoveTargsPerc.Value;
            calcOpts.StunningTargetsPerc = (int)CB_StunningTargsPerc.Value;
            calcOpts.DisarmingTargetsPerc = (int)CB_DisarmingTargsPerc.Value;
            calcOpts.InBackPerc = (int)CB_InBackPerc.Value;
            //
            Character.OnCalculationsInvalidated();
        }
        // Abilities to Maintain Changes
        private void CLB_Maints_SelectedValueChanged(object sender, EventArgs e) {
            CalculationOptionsDPSWarr calcOpts = Character.CalculationOptions as CalculationOptionsDPSWarr;
            //
            for (int i=0; i<CLB_Maints.Items.Count; i++) {
                if (CLB_Maints.GetItemText(CLB_Maints.Items[i]).Contains("==")) { }
                calcOpts.Maintenance[i] = CLB_Maints.GetItemChecked(i);
            }
            //
            Character.OnCalculationsInvalidated();
        }
        // Latency
        private void CB_Latency_ValueChanged(object sender, EventArgs e) {
            CalculationOptionsDPSWarr calcOpts = Character.CalculationOptions as CalculationOptionsDPSWarr;
            calcOpts.Lag   = (int)CB_Lag.Value;
            calcOpts.React = (int)CB_React.Value;
            Character.OnCalculationsInvalidated();
        }
    }
    [Serializable]
    public class CalculationOptionsDPSWarr : ICalculationOptionBase {
        public int TargetLevel = 83;
        public int TargetArmor = (int)StatConversion.NPC_BOSS_ARMOR;
        public float Duration = 300f;
        public bool FuryStance = true;
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