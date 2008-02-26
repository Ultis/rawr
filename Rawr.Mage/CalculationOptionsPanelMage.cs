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
            if (!Character.CalculationOptions.ContainsKey("ShadowResist"))
                Character.CalculationOptions["ShadowResist"] = "0";
            if (!Character.CalculationOptions.ContainsKey("FightDuration"))
                Character.CalculationOptions["FightDuration"] = "300";
            if (!Character.CalculationOptions.ContainsKey("ShadowPriest"))
                Character.CalculationOptions["ShadowPriest"] = "175";
            if (!Character.CalculationOptions.ContainsKey("HeroismAvailable"))
                Character.CalculationOptions["HeroismAvailable"] = "1";
            if (!Character.CalculationOptions.ContainsKey("MoltenFuryPercentage"))
                Character.CalculationOptions["MoltenFuryPercentage"] = "0.15";
            if (!Character.CalculationOptions.ContainsKey("DestructionPotion"))
                Character.CalculationOptions["DestructionPotion"] = "1";
            if (!Character.CalculationOptions.ContainsKey("FlameCap"))
                Character.CalculationOptions["FlameCap"] = "1";
            if (!Character.CalculationOptions.ContainsKey("ABCycles"))
                Character.CalculationOptions["ABCycles"] = "1";
            if (!Character.CalculationOptions.ContainsKey("DpsTime"))
                Character.CalculationOptions["DpsTime"] = "1";
            if (!Character.CalculationOptions.ContainsKey("MaintainScorch"))
                Character.CalculationOptions["MaintainScorch"] = "1";
            if (!Character.CalculationOptions.ContainsKey("InterruptFrequency"))
                Character.CalculationOptions["InterruptFrequency"] = "0";
            if (!Character.CalculationOptions.ContainsKey("EvocationWeapon"))
                Character.CalculationOptions["EvocationWeapon"] = "0";
            if (!Character.CalculationOptions.ContainsKey("AoeDuration"))
                Character.CalculationOptions["AoeDuration"] = "0";

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
            textBoxShadowResist.Text = Character.CalculationOptions["ShadowResist"];
            textBoxFightDuration.Text = Character.CalculationOptions["FightDuration"];
            textBoxShadowPriest.Text = Character.CalculationOptions["ShadowPriest"];
            checkBoxHeroism.Checked = Character.CalculationOptions["HeroismAvailable"] == "1";
            textBoxMoltenFuryPercentage.Text = Character.CalculationOptions["MoltenFuryPercentage"];
            checkBoxDestructionPotion.Checked = Character.CalculationOptions["DestructionPotion"] == "1";
            checkBoxFlameCap.Checked = Character.CalculationOptions["FlameCap"] == "1";
            checkBoxABCycles.Checked = Character.CalculationOptions["ABCycles"] == "1";
            textBoxDpsTime.Text = Character.CalculationOptions["DpsTime"];
            checkBoxMaintainScorch.Checked = Character.CalculationOptions["MaintainScorch"] == "1";
            textBoxInterruptFrequency.Text = Character.CalculationOptions["InterruptFrequency"];
            textBoxEvocationWeapon.Text = Character.CalculationOptions["EvocationWeapon"];
            textBoxAoeDuration.Text = Character.CalculationOptions["AoeDuration"];

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
            float value;
            if (float.TryParse(textBoxLatency.Text, out value))
            {
                Character.CalculationOptions["Latency"] = textBoxLatency.Text;
                Character.OnItemsChanged();
            }
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
            int value;
            if (int.TryParse(textBoxAoeTargets.Text, out value))
            {
                Character.CalculationOptions["AoeTargets"] = textBoxAoeTargets.Text;
                Character.OnItemsChanged();
            }
        }

        private void textBoxArcaneResist_TextChanged(object sender, EventArgs e)
        {
            float value;
            if (float.TryParse(textBoxArcaneResist.Text, out value))
            {
                Character.CalculationOptions["ArcaneResist"] = textBoxArcaneResist.Text;
                Character.OnItemsChanged();
            }
        }

        private void textBoxFireResist_TextChanged(object sender, EventArgs e)
        {
            float value;
            if (float.TryParse(textBoxFireResist.Text, out value))
            {
                Character.CalculationOptions["FireResist"] = textBoxFireResist.Text;
                Character.OnItemsChanged();
            }
        }

        private void textBoxFrostResist_TextChanged(object sender, EventArgs e)
        {
            float value;
            if (float.TryParse(textBoxFrostResist.Text, out value))
            {
                Character.CalculationOptions["FrostResist"] = textBoxFrostResist.Text;
                Character.OnItemsChanged();
            }
        }

        private void textBoxNatureResist_TextChanged(object sender, EventArgs e)
        {
            float value;
            if (float.TryParse(textBoxNatureResist.Text, out value))
            {
                Character.CalculationOptions["NatureResist"] = textBoxNatureResist.Text;
                Character.OnItemsChanged();
            }
        }

        private void textBoxFightDuration_TextChanged(object sender, EventArgs e)
        {
            float value;
            if (float.TryParse(textBoxFightDuration.Text, out value))
            {
                Character.CalculationOptions["FightDuration"] = textBoxFightDuration.Text;
                Character.OnItemsChanged();
            }
        }

        private void textBoxShadowPriest_TextChanged(object sender, EventArgs e)
        {
            float value;
            if (float.TryParse(textBoxShadowPriest.Text, out value))
            {
                Character.CalculationOptions["ShadowPriest"] = textBoxShadowPriest.Text;
                Character.OnItemsChanged();
            }
        }

        private void checkBoxHeroism_CheckedChanged(object sender, EventArgs e)
        {
            Character.CalculationOptions["HeroismAvailable"] = checkBoxHeroism.Checked ? "1" : "0";
            Character.OnItemsChanged();
        }

        private void textBoxMoltenFuryPercentage_TextChanged(object sender, EventArgs e)
        {
            float value;
            if (float.TryParse(textBoxMoltenFuryPercentage.Text, out value))
            {
                Character.CalculationOptions["MoltenFuryPercentage"] = textBoxMoltenFuryPercentage.Text;
                Character.OnItemsChanged();
            }
        }

        private void checkBoxDestructionPotion_CheckedChanged(object sender, EventArgs e)
        {
            Character.CalculationOptions["DestructionPotion"] = checkBoxDestructionPotion.Checked ? "1" : "0";
            Character.OnItemsChanged();
        }

        private void checkBoxFlameCap_CheckedChanged(object sender, EventArgs e)
        {
            Character.CalculationOptions["FlameCap"] = checkBoxFlameCap.Checked ? "1" : "0";
            Character.OnItemsChanged();
        }

        private void checkBoxABCycles_CheckedChanged(object sender, EventArgs e)
        {
            Character.CalculationOptions["ABCycles"] = checkBoxABCycles.Checked ? "1" : "0";
            Character.OnItemsChanged();
        }

        private void textBoxDpsTime_TextChanged(object sender, EventArgs e)
        {
            float value;
            if (float.TryParse(textBoxDpsTime.Text, out value))
            {
                Character.CalculationOptions["DpsTime"] = textBoxDpsTime.Text;
                Character.OnItemsChanged();
            }
        }

        private void checkBoxMaintainScorch_CheckedChanged(object sender, EventArgs e)
        {
            Character.CalculationOptions["MaintainScorch"] = checkBoxMaintainScorch.Checked ? "1" : "0";
            Character.OnItemsChanged();
        }

        private void textBoxInterruptFrequency_TextChanged(object sender, EventArgs e)
        {
            float value;
            if (float.TryParse(textBoxInterruptFrequency.Text, out value))
            {
                Character.CalculationOptions["InterruptFrequency"] = textBoxInterruptFrequency.Text;
                Character.OnItemsChanged();
            }
        }

        private void textBoxShadowResist_TextChanged(object sender, EventArgs e)
        {
            float value;
            if (float.TryParse(textBoxShadowResist.Text, out value))
            {
                Character.CalculationOptions["ShadowResist"] = textBoxShadowResist.Text;
                Character.OnItemsChanged();
            }
        }

        private void textBoxEvocationWeapon_TextChanged(object sender, EventArgs e)
        {
            int value;
            if (int.TryParse(textBoxEvocationWeapon.Text, out value))
            {
                Character.CalculationOptions["EvocationWeapon"] = textBoxEvocationWeapon.Text;
                Character.OnItemsChanged();
            }
        }

        private void textBoxAoeDuration_TextChanged(object sender, EventArgs e)
        {
            float value;
            if (float.TryParse(textBoxAoeDuration.Text, out value))
            {
                Character.CalculationOptions["AoeDuration"] = textBoxAoeDuration.Text;
                Character.OnItemsChanged();
            }
        }
	}
}
