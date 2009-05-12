using System;
using System.Text;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Rawr.DPSWarr {
    public partial class CalculationOptionsPanelDPSWarr : CalculationOptionsPanelBase {
        private readonly Dictionary<int, string> armorBosses = new Dictionary<int, string>();
        public CalculationOptionsPanelDPSWarr() {
            InitializeComponent();

            armorBosses.Add(10900, "Patchwerk");
            armorBosses.Add(12000, "Grobbulus");
            armorBosses.Add(13083, "-");

            CB_TargArmor.DisplayMember = "Key";
            CB_TargArmor.DataSource = new BindingSource(armorBosses, null);

            CB_TargLvl.DataSource = new[] {83, 82, 81, 80};
            CB_Duration.Minimum = 0;
            CB_Duration.Maximum = 60*20; // 20 minutes
        }
        protected override void LoadCalculationOptions() {
            if (Character != null && Character.CalculationOptions == null) { Character.CalculationOptions = new CalculationOptionsDPSWarr(); return; }
            CalculationOptionsDPSWarr calcOpts = Character.CalculationOptions as CalculationOptionsDPSWarr;
            if (calcOpts != null){
                CB_TargLvl.Text = calcOpts.TargetLevel.ToString();
                CB_TargArmor.Text = calcOpts.TargetArmor.ToString();
                switch (calcOpts.ToughnessLvl) {
                    case 3: CB_ToughLvl.SelectedIndex = 1; break;
                    case 5: CB_ToughLvl.SelectedIndex = 2; break;
                    case 7: CB_ToughLvl.SelectedIndex = 3; break;
                    case 10: CB_ToughLvl.SelectedIndex = 4; break;
                    case 30: CB_ToughLvl.SelectedIndex = 5; break;
                    case 50: CB_ToughLvl.SelectedIndex = 6; break;
                    default: CB_ToughLvl.SelectedIndex = 0; break;
                }
                CB_Duration.Value = (decimal)calcOpts.Duration;
                RB_StanceArms.Checked = !calcOpts.FuryStance;
                RB_TargSingle.Checked = !calcOpts.MultipleTargets;
                RB_TargsStand.Checked = !calcOpts.MovingTargets;
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
        private void CB_ToughLvl_SelectedIndexChanged(object sender, EventArgs e) {
            CalculationOptionsDPSWarr calcOpts = Character.CalculationOptions as CalculationOptionsDPSWarr;
            switch (CB_ToughLvl.SelectedIndex) {
                case 1: calcOpts.ToughnessLvl = 3; break;
                case 2: calcOpts.ToughnessLvl = 5; break;
                case 3: calcOpts.ToughnessLvl = 7; break;
                case 4: calcOpts.ToughnessLvl = 10; break;
                case 5: calcOpts.ToughnessLvl = 30; break;
                case 6: calcOpts.ToughnessLvl = 50; break;
                default: calcOpts.ToughnessLvl = 0; break;
            }
            Character.OnCalculationsInvalidated();
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
        private void RB_TargSingle_CheckedChanged(object sender, EventArgs e) {
            CalculationOptionsDPSWarr calcOpts = Character.CalculationOptions as CalculationOptionsDPSWarr;
            calcOpts.MultipleTargets = RB_TargMultiple.Checked;
            Character.OnCalculationsInvalidated();
        }
        private void RB_TargStanding_CheckedChanged(object sender, EventArgs e) {
            CalculationOptionsDPSWarr calcOpts = Character.CalculationOptions as CalculationOptionsDPSWarr;
            calcOpts.MovingTargets = RB_TargsMove.Checked;
            Character.OnCalculationsInvalidated();
        }
    }
    [Serializable]
    public class CalculationOptionsDPSWarr : ICalculationOptionBase {
        public int TargetLevel = 83;
        public int TargetArmor = 12900;
        public int ToughnessLvl = 0;
        public float Duration = 300;
        public bool FuryStance = true;
        public bool MultipleTargets = false;
        public bool MovingTargets = false;
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