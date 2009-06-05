using System;
using System.Text;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Rawr.DPSWarr {
    public partial class CalculationOptionsPanelDPSWarr : CalculationOptionsPanelBase {
        private readonly Dictionary<int, string> armorBosses = new Dictionary<int, string>();
        public CalculationOptionsPanelDPSWarr() {
            InitializeComponent();

            armorBosses.Add(10643, "Default Boss Armor");
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
                RB_StanceFury.Checked = opts.FuryStance;
                RB_StanceArms.Checked = !RB_StanceFury.Checked;
                Character.OnCalculationsInvalidated();
                return; 
            }
            CalculationOptionsDPSWarr calcOpts = Character.CalculationOptions as CalculationOptionsDPSWarr;
            if (calcOpts != null) {
                CB_TargLvl.Text = calcOpts.TargetLevel.ToString();
                CB_TargArmor.Text = calcOpts.TargetArmor.ToString();
                CB_Duration.Value = (decimal)calcOpts.Duration;
                RB_StanceArms.Checked    = !calcOpts.FuryStance;
                CK_MultiTargs.Checked    = calcOpts.MultipleTargets;    CB_MultiTargsPerc.Value     = calcOpts.MultipleTargetsPerc;
                CK_MovingTargs.Checked   =  calcOpts.MovingTargets;     CB_MoveTargsPerc.Value      = calcOpts.MovingTargetsPerc;
                CK_StunningTargs.Checked =  calcOpts.StunningTargets;   CB_StunningTargsPerc.Value  = calcOpts.StunningTargetsPerc;
                CK_DisarmTargs.Checked   =  calcOpts.DisarmingTargets;  CB_DisarmingTargsPerc.Value = calcOpts.DisarmingTargetsPerc;
                if (Character != null) {
                    CalculationOptionsDPSWarr opts = Character.CalculationOptions as CalculationOptionsDPSWarr;
                    opts.FuryStance = (Character.WarriorTalents.TitansGrip == 1);
                    RB_StanceFury.Checked = opts.FuryStance;
                    RB_StanceArms.Checked = !RB_StanceFury.Checked;
                    Character.OnCalculationsInvalidated();
                }
            }
        }
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
    }
    [Serializable]
    public class CalculationOptionsDPSWarr : ICalculationOptionBase {
        public int TargetLevel = 83;
        public int TargetArmor = 10643;
        public float Duration = 300;
        public bool FuryStance = true;
        public bool MultipleTargets = false; public int MultipleTargetsPerc = 0;
        public bool MovingTargets   = false; public int MovingTargetsPerc   = 0;
        public bool StunningTargets = false; public int StunningTargetsPerc = 0;
        public bool DisarmingTargets= false; public int DisarmingTargetsPerc= 0;
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