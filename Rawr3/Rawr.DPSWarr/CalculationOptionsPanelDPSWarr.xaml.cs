using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Rawr.DPSWarr {
    public partial class CalculationOptionsPanelDPSWarr : ICalculationOptionsPanel {
        private readonly Dictionary<int, string> armorBosses = new Dictionary<int, string>();
        public CalculationOptionsPanelDPSWarr() {
            InitializeComponent();

            armorBosses.Add(10643, "Default Boss Armor");
            armorBosses.Add(10900, "Patchwerk");
            armorBosses.Add(12000, "Grobbulus");
            armorBosses.Add(13083, "-");

            //CB_TargArmor.DisplayMember = "Key";
            //CB_TargArmor.DataSource = new BindingSource(armorBosses, null);

            //CB_TargLvl.DataSource = new[] { 83, 82, 81, 80 };
            //CB_Duration.Minimum = 0;
            //CB_Duration.Maximum = 60 * 20; // 20 minutes

            //RB_StanceFury.Checked = true;
        }

        /*
        private void comboBoxArmorBosses_SelectedIndexChanged(object sender, EventArgs e) {
            int targetArmor = int.Parse(CB_TargArmor.Text);
            LB_TargArmorDesc.Text = armorBosses[targetArmor];

            if (Character != null && Character.CalculationOptions != null) {
                CalculationOptionsDPSWarr calcOpts = Character.CalculationOptions as CalculationOptionsDPSWarr;
                calcOpts.TargetArmor = targetArmor;
                Character.OnCalculationsInvalidated();
            }
        }
        private void comboBoxTargetLevel_SelectedIndexChanged(object sender, EventArgs e) {
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
        private void CK_MultiTargs_CheckedChanged(object sender, EventArgs e) {
            CalculationOptionsDPSWarr calcOpts = Character.CalculationOptions as CalculationOptionsDPSWarr;
            calcOpts.MultipleTargets = CK_MultiTargs.Checked;
            CB_MultiTargsPerc.Enabled = calcOpts.MultipleTargets;
            Character.OnCalculationsInvalidated();
        }
        private void CB_MultiTargs_ValueChanged(object sender, EventArgs e) {
            CalculationOptionsDPSWarr calcOpts = Character.CalculationOptions as CalculationOptionsDPSWarr;
            calcOpts.MultipleTargetsPerc = (int)CB_MultiTargsPerc.Value;
            Character.OnCalculationsInvalidated();
        }
        private void CK_MovingTargs_CheckedChanged(object sender, EventArgs e) {
            CalculationOptionsDPSWarr calcOpts = Character.CalculationOptions as CalculationOptionsDPSWarr;
            calcOpts.MovingTargets = CK_MovingTargs.Checked;
            this.CB_MoveTargsPerc.Enabled = calcOpts.MovingTargets;
            Character.OnCalculationsInvalidated();
        }
        private void CB_MovingTargs_ValueChanged(object sender, EventArgs e) {
            CalculationOptionsDPSWarr calcOpts = Character.CalculationOptions as CalculationOptionsDPSWarr;
            calcOpts.MovingTargetsPerc = (int)CB_MoveTargsPerc.Value;
            Character.OnCalculationsInvalidated();
        }
        private void CK_StunningTargs_CheckedChanged(object sender, EventArgs e) {
            CalculationOptionsDPSWarr calcOpts = Character.CalculationOptions as CalculationOptionsDPSWarr;
            calcOpts.StunningTargets = CK_StunningTargs.Checked;
            CB_StunningTargsPerc.Enabled = calcOpts.StunningTargets;
            Character.OnCalculationsInvalidated();
        }
        private void CB_StunningTargs_ValueChanged(object sender, EventArgs e) {
            CalculationOptionsDPSWarr calcOpts = Character.CalculationOptions as CalculationOptionsDPSWarr;
            calcOpts.StunningTargetsPerc = (int)CB_StunningTargsPerc.Value;
            Character.OnCalculationsInvalidated();
        }
        private void CK_DisarmingTargs_CheckedChanged(object sender, EventArgs e) {
            CalculationOptionsDPSWarr calcOpts = Character.CalculationOptions as CalculationOptionsDPSWarr;
            calcOpts.DisarmingTargets = CK_DisarmTargs.Checked;
            CB_DisarmingTargsPerc.Enabled = calcOpts.DisarmingTargets;
            Character.OnCalculationsInvalidated();
        }
        private void CB_DisarmingTargs_ValueChanged(object sender, EventArgs e) {
            CalculationOptionsDPSWarr calcOpts = Character.CalculationOptions as CalculationOptionsDPSWarr;
            calcOpts.DisarmingTargetsPerc = (int)CB_DisarmingTargsPerc.Value;
            Character.OnCalculationsInvalidated();
        }
        private void CK_InBack_CheckedChanged(object sender, EventArgs e) {
            CalculationOptionsDPSWarr calcOpts = Character.CalculationOptions as CalculationOptionsDPSWarr;
            calcOpts.InBack = CK_InBack.Checked;
            CB_InBackPerc.Enabled = calcOpts.InBack;
            Character.OnCalculationsInvalidated();
        }
        private void CB_InBack_ValueChanged(object sender, EventArgs e) {
            CalculationOptionsDPSWarr calcOpts = Character.CalculationOptions as CalculationOptionsDPSWarr;
            calcOpts.InBackPerc = (int)CB_InBackPerc.Value;
            Character.OnCalculationsInvalidated();
        }
        // Abilities to Maintain Changes
        private void CK_Maints_CheckedChanged(object sender, EventArgs e) {
            CalculationOptionsDPSWarr calcOpts = Character.CalculationOptions as CalculationOptionsDPSWarr;
            calcOpts.Mntn_Thunder = CK_Thunder.Checked;
            calcOpts.Mntn_Sunder = CK_Sunder.Checked;
            calcOpts.Mntn_Demo = CK_DemoShout.Checked;
            calcOpts.Mntn_Hamstring = CK_Hamstring.Checked;
            calcOpts.Mntn_Battle = CK_BattleShout.Checked;

            Character.OnCalculationsInvalidated();
        }
        // Latency
        private void CB_Latency_ValueChanged(object sender, EventArgs e) {
            CalculationOptionsDPSWarr calcOpts = Character.CalculationOptions as CalculationOptionsDPSWarr;
            calcOpts.Lag   = (int)CB_Lag.Value;
            calcOpts.React = (int)CB_React.Value;
            Character.OnCalculationsInvalidated();
        }
        */
        public UserControl PanelControl { get { return this; } }

        private Character character;
        public Character Character {
            get { return character; }
            set { character = value; LoadCalculationOptions(); }
        }

        private bool _loadingCalculationOptions;
        public void LoadCalculationOptions() {
            _loadingCalculationOptions = true;
            if (Character.CalculationOptions == null) Character.CalculationOptions = new CalculationOptionsDPSWarr();

            /*if (Character != null && Character.CalculationOptions == null) { 
                Character.CalculationOptions = new CalculationOptionsDPSWarr();
                CalculationOptionsDPSWarr opts = Character.CalculationOptions as CalculationOptionsDPSWarr;
                opts.FuryStance = (Character.WarriorTalents.TitansGrip == 1);
                // Rotational Changes
                RB_StanceFury.Checked = opts.FuryStance;
                RB_StanceArms.Checked = !RB_StanceFury.Checked;
                // Abilities to Maintain
                CK_Thunder.Checked = opts.Mntn_Thunder;
                CK_Sunder.Checked = opts.Mntn_Sunder;
                CK_DemoShout.Checked = opts.Mntn_Demo;
                CK_Hamstring.Checked = opts.Mntn_Hamstring;
                CK_BattleShout.Checked = opts.Mntn_Battle;
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
            }*/

            _loadingCalculationOptions = false;
        }
    }
}
