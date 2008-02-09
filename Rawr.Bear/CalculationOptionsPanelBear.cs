using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Rawr
{
	public partial class CalculationOptionsPanelBear : CalculationOptionsPanelBase
	{
		public CalculationOptionsPanelBear()
		{
			InitializeComponent();
		}

		protected override void LoadCalculationOptions()
		{
			if (!Character.CalculationOptions.ContainsKey("TargetLevel"))
				Character.CalculationOptions["TargetLevel"] = "73";
			if (!Character.CalculationOptions.ContainsKey("EnforceMetagemRequirements"))
				Character.CalculationOptions["EnforceMetagemRequirements"] = "Yes";

			comboBoxTargetLevel.SelectedItem = Character.CalculationOptions["TargetLevel"];
			checkBoxEnforceMetagemRequirements.Checked = Character.CalculationOptions["EnforceMetagemRequirements"] == "Yes";
		}
	
		private void comboBoxTargetLevel_SelectedIndexChanged(object sender, EventArgs e)
		{
			Character.CalculationOptions["TargetLevel"] = comboBoxTargetLevel.SelectedItem.ToString();
			Character.OnItemsChanged();
		}

		private void checkBoxEnforceMetagemRequirements_CheckedChanged(object sender, EventArgs e)
		{
			Character.CalculationOptions["EnforceMetagemRequirements"] = checkBoxEnforceMetagemRequirements.Checked ? "Yes" : "No";
			Character.OnItemsChanged();
		}
	}
}
