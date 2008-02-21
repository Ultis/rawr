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
            if (!Character.CalculationOptions.ContainsKey("AoeTargets"))
                Character.CalculationOptions["AoeTargets"] = "9";
            if (!Character.CalculationOptions.ContainsKey("MageArmor"))
                Character.CalculationOptions["MageArmor"] = "Mage";
            if (!Character.CalculationOptions.ContainsKey("ArcaneResist"))
                Character.CalculationOptions["ArcaneResist"] = "0";
            if (!Character.CalculationOptions.ContainsKey("FireResist"))
                Character.CalculationOptions["FireResist"] = "0";
            if (!Character.CalculationOptions.ContainsKey("FrostResist"))
                Character.CalculationOptions["FrostResist"] = "0";
            if (!Character.CalculationOptions.ContainsKey("NatureResist"))
                Character.CalculationOptions["NatureResist"] = "0";
            if (!Character.CalculationOptions.ContainsKey("FightDuration"))
                Character.CalculationOptions["FightDuration"] = "300";
            if (!Character.CalculationOptions.ContainsKey("ShadowPriest"))
                Character.CalculationOptions["ShadowPriest"] = "175";
            if (!Character.CalculationOptions.ContainsKey("HeroismAvailable"))
                Character.CalculationOptions["HeroismAvailable"] = "1";
            if (!Character.CalculationOptions.ContainsKey("MoltenFuryPercentage"))
                Character.CalculationOptions["MoltenFuryPercentage"] = "0.15";

			comboBoxTargetLevel.SelectedItem = Character.CalculationOptions["TargetLevel"];
            comboBoxAoeTargetLevel.SelectedItem = Character.CalculationOptions["AoeTargetLevel"];
            checkBoxEnforceMetagemRequirements.Checked = Character.CalculationOptions["EnforceMetagemRequirements"] == "Yes";
            textBoxLatency.Text = Character.CalculationOptions["Latency"];
            comboBoxArmor.SelectedItem = Character.CalculationOptions["MageArmor"];
            textBoxAoeTargets.Text = Character.CalculationOptions["AoeTargets"];
            textBoxArcaneResist.Text = Character.CalculationOptions["ArcaneResist"];
            textBoxFireResist.Text = Character.CalculationOptions["FireResist"];
            textBoxFrostResist.Text = Character.CalculationOptions["FrostResist"];
            textBoxNatureResist.Text = Character.CalculationOptions["NatureResist"];
            textBoxFightDuration.Text = Character.CalculationOptions["FightDuration"];
            textBoxShadowPriest.Text = Character.CalculationOptions["ShadowPriest"];
            checkBoxHeroism.Checked = Character.CalculationOptions["HeroismAvailable"] == "1";
            textBoxMoltenFuryPercentage.Text = Character.CalculationOptions["MoltenFuryPercentage"];

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

        private void textBoxAoeTargets_TextChanged(object sender, EventArgs e)
        {
            Character.CalculationOptions["AoeTargets"] = textBoxAoeTargets.Text;
            Character.OnItemsChanged();
        }

        private void textBoxArcaneResist_TextChanged(object sender, EventArgs e)
        {
            Character.CalculationOptions["ArcaneResist"] = textBoxArcaneResist.Text;
            Character.OnItemsChanged();
        }

        private void textBoxFireResist_TextChanged(object sender, EventArgs e)
        {
            Character.CalculationOptions["FireResist"] = textBoxFireResist.Text;
            Character.OnItemsChanged();
        }

        private void textBoxFrostResist_TextChanged(object sender, EventArgs e)
        {
            Character.CalculationOptions["FrostResist"] = textBoxFrostResist.Text;
            Character.OnItemsChanged();
        }

        private void textBoxNatureResist_TextChanged(object sender, EventArgs e)
        {
            Character.CalculationOptions["NatureResist"] = textBoxNatureResist.Text;
            Character.OnItemsChanged();
        }

        private void textBoxFightDuration_TextChanged(object sender, EventArgs e)
        {
            Character.CalculationOptions["FightDuration"] = textBoxFightDuration.Text;
            Character.OnItemsChanged();
        }

        private void textBoxShadowPriest_TextChanged(object sender, EventArgs e)
        {
            Character.CalculationOptions["ShadowPriest"] = textBoxShadowPriest.Text;
            Character.OnItemsChanged();
        }

        private void checkBoxHeroism_CheckedChanged(object sender, EventArgs e)
        {
            Character.CalculationOptions["HeroismAvailable"] = checkBoxHeroism.Checked ? "1" : "0";
            Character.OnItemsChanged();
        }

        private void textBoxMoltenFuryPercentage_TextChanged(object sender, EventArgs e)
        {
            Character.CalculationOptions["MoltenFuryPercentage"] = textBoxMoltenFuryPercentage.Text;
            Character.OnItemsChanged();
        }
	}
}
