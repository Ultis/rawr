using System;
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
            if (Character.CalculationOptions == null)
            {
                Character.CalculationOptions = new CalculationOptionsDPSWarr();
            }
        }

        private void comboBoxArmorBosses_SelectedIndexChanged(object sender, EventArgs e)
        {
            var targetArmor = int.Parse(CB_TargArmor.Text);
            LB_TargArmorDesc.Text = armorBosses[targetArmor];

            if (Character != null && Character.CalculationOptions != null)
            {
                var calcOpts = Character.CalculationOptions as CalculationOptionsDPSWarr;
                calcOpts.TargetArmor = targetArmor;
                Character.OnCalculationsInvalidated();
            }
        }

        private void comboBoxTargetLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Character != null && Character.CalculationOptions != null)
            {
                var calcOpts = Character.CalculationOptions as CalculationOptionsDPSWarr;
                calcOpts.TargetLevel = int.Parse(CB_TargLvl.Text);
                Character.OnCalculationsInvalidated();
            }
        }
    }
}