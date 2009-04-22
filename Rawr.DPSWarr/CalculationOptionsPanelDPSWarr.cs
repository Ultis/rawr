using System;
using System.Text;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Rawr.DPSWarr
{
    public partial class CalculationOptionsPanelDPSWarr : CalculationOptionsPanelBase
    {
        private readonly Dictionary<int, string> armorBosses = new Dictionary<int, string>();

        public CalculationOptionsPanelDPSWarr()
        {
            InitializeComponent();

            armorBosses.Add(10900, "Patchwerk");
            armorBosses.Add(12000, "Grobbulus");
            armorBosses.Add(13083, "-");

            CB_TargArmor.DisplayMember = "Key";
            CB_TargArmor.DataSource = new BindingSource(armorBosses, null);

            CB_TargLvl.DataSource = new[] {83, 82, 81, 80};
        }

        protected override void LoadCalculationOptions()
        {
            if (Character.CalculationOptions == null) {
                Character.CalculationOptions = new CalculationOptionsDPSWarr();
            }
        }

        private void comboBoxArmorBosses_SelectedIndexChanged(object sender, EventArgs e)
        {
            int targetArmor = int.Parse(CB_TargArmor.Text);
            LB_TargArmorDesc.Text = armorBosses[targetArmor];

            if (Character != null && Character.CalculationOptions != null) {
                CalculationOptionsDPSWarr calcOpts = Character.CalculationOptions as CalculationOptionsDPSWarr;
                calcOpts.TargetArmor = targetArmor;
                Character.OnCalculationsInvalidated();
            }
        }

        private void comboBoxTargetLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
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
    }
    [Serializable]
    public class CalculationOptionsDPSWarr : ICalculationOptionBase {
        public int TargetLevel = 83;
        public int TargetArmor = 12900;
        public int ToughnessLvl = 0;
        public bool FuryStance = true;
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