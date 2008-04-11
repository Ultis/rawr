using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Rawr
{
	public partial class CalculationOptionsPanelProtWarr : CalculationOptionsPanelBase
	{
		public CalculationOptionsPanelProtWarr()
		{
			InitializeComponent();
		}

		protected override void LoadCalculationOptions()
		{
			if (!Character.CalculationOptions.ContainsKey("TargetLevel"))
				Character.CalculationOptions["TargetLevel"] = "73";
            if (!Character.CalculationOptions.ContainsKey("BossAttackValue"))
                Character.CalculationOptions["BossAttackValue"] = "20000";
            if (!Character.CalculationOptions.ContainsKey("ThreatScale"))
                Character.CalculationOptions["ThreatScale"] = "1";
			if (!Character.CalculationOptions.ContainsKey("EnforceMetagemRequirements"))
				Character.CalculationOptions["EnforceMetagemRequirements"] = "No";

			comboBoxTargetLevel.SelectedItem = Character.CalculationOptions["TargetLevel"];
            bossAttackValue.Value = decimal.Parse(Character.CalculationOptions["BossAttackValue"]);
            threatScaleFactor.Value = decimal.Parse(Character.CalculationOptions["ThreatScale"]);
			checkBoxEnforceMetagemRequirements.Checked = Character.CalculationOptions["EnforceMetagemRequirements"] == "Yes";
		}
	
		private void comboBoxTargetLevel_SelectedIndexChanged(object sender, EventArgs e)
		{
			Character.CalculationOptions["TargetLevel"] = comboBoxTargetLevel.SelectedItem.ToString();
			Character.OnItemsChanged();
        }

        private void bossAttackValue_ValueChanged(object sender, EventArgs e)
        {
            Character.CalculationOptions["BossAttackValue"] = bossAttackValue.Value.ToString();
            Character.OnItemsChanged();
        }

        private void threatScaleFactor_ValueChanged(object sender, EventArgs e)
        {
            Character.CalculationOptions["ThreatScale"] = threatScaleFactor.Value.ToString();
            Character.OnItemsChanged();

        }

        private void checkBoxEnforceMetagemRequirements_CheckedChanged(object sender, EventArgs e)
        {
            Character.CalculationOptions["EnforceMetagemRequirements"] = checkBoxEnforceMetagemRequirements.Checked ? "Yes" : "No";
            Character.OnItemsChanged();
        }
	}
}
