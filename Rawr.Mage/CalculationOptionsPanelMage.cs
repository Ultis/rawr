using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Rawr.Mage
{
	public partial class CalculationOptionsPanelMage : CalculationOptionsPanelBase
	{
		public CalculationOptionsPanelMage()
		{
			InitializeComponent();
            talents = new MageTalentsForm(this);
		}

        private MageTalentsForm talents;

		protected override void LoadCalculationOptions()
		{
			if (!Character.CalculationOptions.ContainsKey("TargetLevel"))
				Character.CalculationOptions["TargetLevel"] = "73";
            if (!Character.CalculationOptions.ContainsKey("AoeTargetLevel"))
                Character.CalculationOptions["AoeTargetLevel"] = "70";
            if (!Character.CalculationOptions.ContainsKey("EnforceMetagemRequirements"))
				Character.CalculationOptions["EnforceMetagemRequirements"] = "No";
            if (!Character.CalculationOptions.ContainsKey("Latency"))
                Character.CalculationOptions["Latency"] = "0.05";
            if (!Character.CalculationOptions.ContainsKey("MageArmor"))
                Character.CalculationOptions["MageArmor"] = "Mage";

			comboBoxTargetLevel.SelectedItem = Character.CalculationOptions["TargetLevel"];
            comboBoxAoeTargetLevel.SelectedItem = Character.CalculationOptions["AoeTargetLevel"];
            checkBoxEnforceMetagemRequirements.Checked = Character.CalculationOptions["EnforceMetagemRequirements"] == "Yes";
            textBoxLatency.Text = Character.CalculationOptions["Latency"];
            comboBoxArmor.SelectedItem = Character.CalculationOptions["MageArmor"];

            if (talents != null) talents.LoadCalculationOptions();
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

        private void comboBoxAoeTargetLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            Character.CalculationOptions["AoeTargetLevel"] = comboBoxAoeTargetLevel.SelectedItem.ToString();
            Character.OnItemsChanged();
        }

        private void textBoxLatency_TextChanged(object sender, EventArgs e)
        {
            Character.CalculationOptions["Latency"] = textBoxLatency.Text;
            Character.OnItemsChanged();
        }

        private void buttonTalents_Click(object sender, EventArgs e)
        {
            talents.Show();
        }

        private void comboBoxArmor_SelectedIndexChanged(object sender, EventArgs e)
        {
            Character.CalculationOptions["MageArmor"] = comboBoxArmor.SelectedItem.ToString();
            Character.OnItemsChanged();
        }
	}
}
