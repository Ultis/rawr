using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Rawr.Warlock
{
	public partial class CalculationOptionsPanelWarlock : CalculationOptionsPanelBase
	{
		public CalculationOptionsPanelWarlock()
		{
			InitializeComponent();
            		}


		protected override void LoadCalculationOptions()
		{
			if (!Character.CalculationOptions.ContainsKey("TargetLevel"))
				Character.CalculationOptions["TargetLevel"] = "73";
            if (!Character.CalculationOptions.ContainsKey("EnforceMetagemRequirements"))
				Character.CalculationOptions["EnforceMetagemRequirements"] = "T";
            if (!Character.CalculationOptions.ContainsKey("Latency"))
                Character.CalculationOptions["Latency"] = "0.05";
            if (!Character.CalculationOptions.ContainsKey("Duration"))
                Character.CalculationOptions["Latency"] = "600";

			comboBoxTargetLevel.SelectedItem = Character.CalculationOptions["TargetLevel"];
            checkBoxEnforceMetagemRequirements.Checked = Character.CalculationOptions["EnforceMetagemRequirements"] == "T";
            textBoxLatency.Text = Character.CalculationOptions["Latency"];
            textBoxDuration.Text = Character.CalculationOptions["Duration"];
           
        }
	
		private void comboBoxTargetLevel_SelectedIndexChanged(object sender, EventArgs e)
		{
			Character.CalculationOptions["TargetLevel"] = comboBoxTargetLevel.SelectedItem.ToString();
			Character.OnItemsChanged();
		}

		private void checkBoxEnforceMetagemRequirements_CheckedChanged(object sender, EventArgs e)
		{
			Character.CalculationOptions["EnforceMetagemRequirements"] = checkBoxEnforceMetagemRequirements.Checked ? "T" : "F";
			Character.OnItemsChanged();
		}

        

        private void textBoxLatency_TextChanged(object sender, EventArgs e)
        {
            Character.CalculationOptions["Latency"] = textBoxLatency.Text;
            Character.OnItemsChanged();
        }

        private void comboBoxFilterSpell_SelectedIndexChanged(object sender, EventArgs e)
        {
            Character.CalculationOptions["FillerSpell"] = (sender as ComboBox).SelectedItem.ToString();
            Character.OnItemsChanged();
        }

        private void comboBoxCastCurse_SelectedIndexChanged(object sender, EventArgs e)
        {
            Character.CalculationOptions["Curse"] = (sender as ComboBox).SelectedItem.ToString();
            Character.OnItemsChanged();
        }

        private void checkImmolate_CheckedChanged(object sender, EventArgs e)
        {
            Character.CalculationOptions["Immolate"] = (sender as CheckBox).Checked ? "T" : "F";
            Character.OnItemsChanged();
        }

        private void checkCorruption_CheckedChanged(object sender, EventArgs e)
        {
            Character.CalculationOptions["Corruption"] = (sender as CheckBox).Checked ? "T" : "F";
            Character.OnItemsChanged();
        }

        private void checkSiphonLife_CheckedChanged(object sender, EventArgs e)
        {
            Character.CalculationOptions["SiphonLife"] = (sender as CheckBox).Checked ? "T" : "F";
            Character.OnItemsChanged();
        }

        private void checkUnstable_CheckedChanged(object sender, EventArgs e)
        {
            Character.CalculationOptions["UnstableAffliction"] = (sender as CheckBox).Checked ? "T" : "F";
            Character.OnItemsChanged();
        }

        private void comboBoxPetSelection_SelectedIndexChanged(object sender, EventArgs e)
        {
            Character.CalculationOptions["Pet"] = (sender as ComboBox).SelectedItem.ToString();
            Character.OnItemsChanged();
        }

        private void checkSacraficed_CheckedChanged(object sender, EventArgs e)
        {
            Character.CalculationOptions["SacraficedPet"] = (sender as CheckBox).Checked ? Character.CalculationOptions["Pet"] : "";
            Character.OnItemsChanged();
        }

        private void checkScorch_CheckedChanged(object sender, EventArgs e)
        {
            Character.CalculationOptions["Scorch"] = (sender as CheckBox).Checked ? "T" : "F";
            Character.OnItemsChanged();
        }

        private void checkShadowWeaving_CheckedChanged(object sender, EventArgs e)
        {
            Character.CalculationOptions["ShadowWeaving"] = (sender as CheckBox).Checked ? "T" : "F";
            Character.OnItemsChanged();
        }

        private void checkMisery_CheckedChanged(object sender, EventArgs e)
        {
            Character.CalculationOptions["Misery"] = (sender as CheckBox).Checked ? "T" : "F";
            Character.OnItemsChanged();
        }

        private void comboBoxElements_SelectedIndexChanged(object sender, EventArgs e)
        {
            Character.CalculationOptions["ElementsBonus"] = (sender as ComboBox).SelectedItem.ToString();
            Character.OnItemsChanged();
        }

        private void comboBoxShadows_SelectedIndexChanged(object sender, EventArgs e)
        {
            Character.CalculationOptions["ShadowsBonus"] = (sender as ComboBox).SelectedItem.ToString();
            Character.OnItemsChanged();
        }

        private void textBoxISBUptime_TextChanged(object sender, EventArgs e)
        {
            Character.CalculationOptions["ISBUptime"] = (sender as TextBox).Text;
            Character.OnItemsChanged();
        }

        private void textBoxDuration_TextChanged(object sender, EventArgs e)
        {
            Character.CalculationOptions["Duration"] = (sender as TextBox).Text;
            Character.OnItemsChanged();
        }

       

        

        
	}
}
