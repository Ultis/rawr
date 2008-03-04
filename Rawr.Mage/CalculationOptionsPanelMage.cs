using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Globalization;

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
        private bool loading = false;

		protected override void LoadCalculationOptions()
		{
			if (!Character.CalculationOptions.ContainsKey("TargetLevel"))
				Character.CalculationOptions["TargetLevel"] = (73).ToString(CultureInfo.InvariantCulture);
            if (!Character.CalculationOptions.ContainsKey("AoeTargetLevel"))
                Character.CalculationOptions["AoeTargetLevel"] = (70).ToString(CultureInfo.InvariantCulture);
            if (!Character.CalculationOptions.ContainsKey("EnforceMetagemRequirements"))
				Character.CalculationOptions["EnforceMetagemRequirements"] = "No";
            if (!Character.CalculationOptions.ContainsKey("Latency"))
                Character.CalculationOptions["Latency"] = (0.05).ToString(CultureInfo.InvariantCulture);
            if (!Character.CalculationOptions.ContainsKey("AoeTargets"))
                Character.CalculationOptions["AoeTargets"] = (9).ToString(CultureInfo.InvariantCulture);
            if (!Character.CalculationOptions.ContainsKey("MageArmor"))
                Character.CalculationOptions["MageArmor"] = "Mage";
            if (!Character.CalculationOptions.ContainsKey("ArcaneResist"))
                Character.CalculationOptions["ArcaneResist"] = (0).ToString(CultureInfo.InvariantCulture);
            if (!Character.CalculationOptions.ContainsKey("FireResist"))
                Character.CalculationOptions["FireResist"] = (0).ToString(CultureInfo.InvariantCulture);
            if (!Character.CalculationOptions.ContainsKey("FrostResist"))
                Character.CalculationOptions["FrostResist"] = (0).ToString(CultureInfo.InvariantCulture);
            if (!Character.CalculationOptions.ContainsKey("NatureResist"))
                Character.CalculationOptions["NatureResist"] = (0).ToString(CultureInfo.InvariantCulture);
            if (!Character.CalculationOptions.ContainsKey("ShadowResist"))
                Character.CalculationOptions["ShadowResist"] = (0).ToString(CultureInfo.InvariantCulture);
            if (!Character.CalculationOptions.ContainsKey("FightDuration"))
                Character.CalculationOptions["FightDuration"] = (300).ToString(CultureInfo.InvariantCulture);
            if (!Character.CalculationOptions.ContainsKey("ShadowPriest"))
                Character.CalculationOptions["ShadowPriest"] = (175).ToString(CultureInfo.InvariantCulture);
            if (!Character.CalculationOptions.ContainsKey("HeroismAvailable"))
                Character.CalculationOptions["HeroismAvailable"] = (1).ToString(CultureInfo.InvariantCulture);
            if (!Character.CalculationOptions.ContainsKey("MoltenFuryPercentage"))
                Character.CalculationOptions["MoltenFuryPercentage"] = (0.15).ToString(CultureInfo.InvariantCulture);
            if (!Character.CalculationOptions.ContainsKey("DestructionPotion"))
                Character.CalculationOptions["DestructionPotion"] = (1).ToString(CultureInfo.InvariantCulture);
            if (!Character.CalculationOptions.ContainsKey("FlameCap"))
                Character.CalculationOptions["FlameCap"] = (1).ToString(CultureInfo.InvariantCulture);
            if (!Character.CalculationOptions.ContainsKey("ABCycles"))
                Character.CalculationOptions["ABCycles"] = (1).ToString(CultureInfo.InvariantCulture);
            if (!Character.CalculationOptions.ContainsKey("DpsTime"))
                Character.CalculationOptions["DpsTime"] = (1).ToString(CultureInfo.InvariantCulture);
            if (!Character.CalculationOptions.ContainsKey("MaintainScorch"))
                Character.CalculationOptions["MaintainScorch"] = (1).ToString(CultureInfo.InvariantCulture);
            if (!Character.CalculationOptions.ContainsKey("InterruptFrequency"))
                Character.CalculationOptions["InterruptFrequency"] = (0).ToString(CultureInfo.InvariantCulture);
            if (!Character.CalculationOptions.ContainsKey("EvocationWeapon"))
                Character.CalculationOptions["EvocationWeapon"] = (0).ToString(CultureInfo.InvariantCulture);
            if (!Character.CalculationOptions.ContainsKey("AoeDuration"))
                Character.CalculationOptions["AoeDuration"] = (0).ToString(CultureInfo.InvariantCulture);
            if (!Character.CalculationOptions.ContainsKey("SmartOptimization"))
                Character.CalculationOptions["SmartOptimization"] = (1).ToString(CultureInfo.InvariantCulture);

            loading = true;

			comboBoxTargetLevel.SelectedItem = Character.CalculationOptions["TargetLevel"];
            comboBoxAoeTargetLevel.SelectedItem = Character.CalculationOptions["AoeTargetLevel"];
            checkBoxEnforceMetagemRequirements.Checked = Character.CalculationOptions["EnforceMetagemRequirements"] == "Yes";
            textBoxLatency.Text = float.Parse(Character.CalculationOptions["Latency"], CultureInfo.InvariantCulture).ToString();
            comboBoxArmor.SelectedItem = Character.CalculationOptions["MageArmor"];
            textBoxAoeTargets.Text = int.Parse(Character.CalculationOptions["AoeTargets"], CultureInfo.InvariantCulture).ToString();
            textBoxArcaneResist.Text = float.Parse(Character.CalculationOptions["ArcaneResist"], CultureInfo.InvariantCulture).ToString();
            textBoxFireResist.Text = float.Parse(Character.CalculationOptions["FireResist"], CultureInfo.InvariantCulture).ToString();
            textBoxFrostResist.Text = float.Parse(Character.CalculationOptions["FrostResist"], CultureInfo.InvariantCulture).ToString();
            textBoxNatureResist.Text = float.Parse(Character.CalculationOptions["NatureResist"], CultureInfo.InvariantCulture).ToString();
            textBoxShadowResist.Text = float.Parse(Character.CalculationOptions["ShadowResist"], CultureInfo.InvariantCulture).ToString();
            textBoxFightDuration.Text = float.Parse(Character.CalculationOptions["FightDuration"], CultureInfo.InvariantCulture).ToString();
            textBoxShadowPriest.Text = float.Parse(Character.CalculationOptions["ShadowPriest"], CultureInfo.InvariantCulture).ToString();
            checkBoxHeroism.Checked = int.Parse(Character.CalculationOptions["HeroismAvailable"], CultureInfo.InvariantCulture) == 1;
            textBoxMoltenFuryPercentage.Text = float.Parse(Character.CalculationOptions["MoltenFuryPercentage"], CultureInfo.InvariantCulture).ToString();
            checkBoxDestructionPotion.Checked = int.Parse(Character.CalculationOptions["DestructionPotion"], CultureInfo.InvariantCulture) == 1;
            checkBoxFlameCap.Checked = int.Parse(Character.CalculationOptions["FlameCap"], CultureInfo.InvariantCulture) == 1;
            checkBoxABCycles.Checked = int.Parse(Character.CalculationOptions["ABCycles"], CultureInfo.InvariantCulture) == 1;
            textBoxDpsTime.Text = float.Parse(Character.CalculationOptions["DpsTime"], CultureInfo.InvariantCulture).ToString();
            checkBoxMaintainScorch.Checked = int.Parse(Character.CalculationOptions["MaintainScorch"], CultureInfo.InvariantCulture) == 1;
            textBoxInterruptFrequency.Text = float.Parse(Character.CalculationOptions["InterruptFrequency"], CultureInfo.InvariantCulture).ToString();
            textBoxEvocationWeapon.Text = int.Parse(Character.CalculationOptions["EvocationWeapon"], CultureInfo.InvariantCulture).ToString();
            textBoxAoeDuration.Text = float.Parse(Character.CalculationOptions["AoeDuration"], CultureInfo.InvariantCulture).ToString();
            checkBoxSmartOptimization.Checked = int.Parse(Character.CalculationOptions["SmartOptimization"], CultureInfo.InvariantCulture) == 1;

            if (talents != null) talents.LoadCalculationOptions();

            loading = false;
            Character.OnItemsChanged();
        }
	
		private void comboBoxTargetLevel_SelectedIndexChanged(object sender, EventArgs e)
		{
            Character.CalculationOptions["TargetLevel"] = comboBoxTargetLevel.SelectedItem.ToString();
			if (!loading) Character.OnItemsChanged();
		}

		private void checkBoxEnforceMetagemRequirements_CheckedChanged(object sender, EventArgs e)
		{
			Character.CalculationOptions["EnforceMetagemRequirements"] = checkBoxEnforceMetagemRequirements.Checked ? "Yes" : "No";
            if (!loading) Character.OnItemsChanged();
        }

        private void comboBoxAoeTargetLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            Character.CalculationOptions["AoeTargetLevel"] = comboBoxAoeTargetLevel.SelectedItem.ToString();
            if (!loading) Character.OnItemsChanged();
        }

        private void textBoxLatency_TextChanged(object sender, EventArgs e)
        {
            float value;
            if (float.TryParse(textBoxLatency.Text, out value))
            {
                Character.CalculationOptions["Latency"] = value.ToString(CultureInfo.InvariantCulture);
                if (!loading) Character.OnItemsChanged();
            }
        }

        private void buttonTalents_Click(object sender, EventArgs e)
        {
            talents.Show();
        }

        private void comboBoxArmor_SelectedIndexChanged(object sender, EventArgs e)
        {
            Character.CalculationOptions["MageArmor"] = comboBoxArmor.SelectedItem.ToString();
            if (!loading) Character.OnItemsChanged();
        }

        private void textBoxAoeTargets_TextChanged(object sender, EventArgs e)
        {
            int value;
            if (int.TryParse(textBoxAoeTargets.Text, out value))
            {
                Character.CalculationOptions["AoeTargets"] = value.ToString(CultureInfo.InvariantCulture);
                if (!loading) Character.OnItemsChanged();
            }
        }

        private void textBoxArcaneResist_TextChanged(object sender, EventArgs e)
        {
            float value;
            if (float.TryParse(textBoxArcaneResist.Text, out value))
            {
                Character.CalculationOptions["ArcaneResist"] = value.ToString(CultureInfo.InvariantCulture);
                if (!loading) Character.OnItemsChanged();
            }
        }

        private void textBoxFireResist_TextChanged(object sender, EventArgs e)
        {
            float value;
            if (float.TryParse(textBoxFireResist.Text, out value))
            {
                Character.CalculationOptions["FireResist"] = value.ToString(CultureInfo.InvariantCulture);
                if (!loading) Character.OnItemsChanged();
            }
        }

        private void textBoxFrostResist_TextChanged(object sender, EventArgs e)
        {
            float value;
            if (float.TryParse(textBoxFrostResist.Text, out value))
            {
                Character.CalculationOptions["FrostResist"] = value.ToString(CultureInfo.InvariantCulture);
                if (!loading) Character.OnItemsChanged();
            }
        }

        private void textBoxNatureResist_TextChanged(object sender, EventArgs e)
        {
            float value;
            if (float.TryParse(textBoxNatureResist.Text, out value))
            {
                Character.CalculationOptions["NatureResist"] = value.ToString(CultureInfo.InvariantCulture);
                if (!loading) Character.OnItemsChanged();
            }
        }

        private void textBoxFightDuration_TextChanged(object sender, EventArgs e)
        {
            float value;
            if (float.TryParse(textBoxFightDuration.Text, out value))
            {
                Character.CalculationOptions["FightDuration"] = value.ToString(CultureInfo.InvariantCulture);
                if (!loading) Character.OnItemsChanged();
            }
        }

        private void textBoxShadowPriest_TextChanged(object sender, EventArgs e)
        {
            float value;
            if (float.TryParse(textBoxShadowPriest.Text, out value))
            {
                Character.CalculationOptions["ShadowPriest"] = value.ToString(CultureInfo.InvariantCulture);
                if (!loading) Character.OnItemsChanged();
            }
        }

        private void checkBoxHeroism_CheckedChanged(object sender, EventArgs e)
        {
            Character.CalculationOptions["HeroismAvailable"] = (checkBoxHeroism.Checked ? 1 : 0).ToString(CultureInfo.InvariantCulture);
            if (!loading) Character.OnItemsChanged();
        }

        private void textBoxMoltenFuryPercentage_TextChanged(object sender, EventArgs e)
        {
            float value;
            if (float.TryParse(textBoxMoltenFuryPercentage.Text, out value))
            {
                Character.CalculationOptions["MoltenFuryPercentage"] = value.ToString(CultureInfo.InvariantCulture);
                if (!loading) Character.OnItemsChanged();
            }
        }

        private void checkBoxDestructionPotion_CheckedChanged(object sender, EventArgs e)
        {
            Character.CalculationOptions["DestructionPotion"] = (checkBoxDestructionPotion.Checked ? 1 : 0).ToString(CultureInfo.InvariantCulture);
            if (!loading) Character.OnItemsChanged();
        }

        private void checkBoxFlameCap_CheckedChanged(object sender, EventArgs e)
        {
            Character.CalculationOptions["FlameCap"] = (checkBoxFlameCap.Checked ? 1 : 0).ToString(CultureInfo.InvariantCulture);
            if (!loading) Character.OnItemsChanged();
        }

        private void checkBoxABCycles_CheckedChanged(object sender, EventArgs e)
        {
            Character.CalculationOptions["ABCycles"] = (checkBoxABCycles.Checked ? 1 : 0).ToString(CultureInfo.InvariantCulture);
            if (!loading) Character.OnItemsChanged();
        }

        private void textBoxDpsTime_TextChanged(object sender, EventArgs e)
        {
            float value;
            if (float.TryParse(textBoxDpsTime.Text, out value))
            {
                Character.CalculationOptions["DpsTime"] = value.ToString(CultureInfo.InvariantCulture);
                if (!loading) Character.OnItemsChanged();
            }
        }

        private void checkBoxMaintainScorch_CheckedChanged(object sender, EventArgs e)
        {
            Character.CalculationOptions["MaintainScorch"] = (checkBoxMaintainScorch.Checked ? 1 : 0).ToString(CultureInfo.InvariantCulture);
            if (!loading) Character.OnItemsChanged();
        }

        private void textBoxInterruptFrequency_TextChanged(object sender, EventArgs e)
        {
            float value;
            if (float.TryParse(textBoxInterruptFrequency.Text, out value))
            {
                Character.CalculationOptions["InterruptFrequency"] = value.ToString(CultureInfo.InvariantCulture);
                if (!loading) Character.OnItemsChanged();
            }
        }

        private void textBoxShadowResist_TextChanged(object sender, EventArgs e)
        {
            float value;
            if (float.TryParse(textBoxShadowResist.Text, out value))
            {
                Character.CalculationOptions["ShadowResist"] = value.ToString(CultureInfo.InvariantCulture);
                if (!loading) Character.OnItemsChanged();
            }
        }

        private void textBoxEvocationWeapon_TextChanged(object sender, EventArgs e)
        {
            int value;
            if (int.TryParse(textBoxEvocationWeapon.Text, out value))
            {
                Character.CalculationOptions["EvocationWeapon"] = value.ToString(CultureInfo.InvariantCulture);
                if (!loading) Character.OnItemsChanged();
            }
        }

        private void textBoxAoeDuration_TextChanged(object sender, EventArgs e)
        {
            float value;
            if (float.TryParse(textBoxAoeDuration.Text, out value))
            {
                Character.CalculationOptions["AoeDuration"] = value.ToString(CultureInfo.InvariantCulture);
                if (!loading) Character.OnItemsChanged();
            }
        }

        private void checkBoxSmartOptimization_CheckedChanged(object sender, EventArgs e)
        {
            Character.CalculationOptions["SmartOptimization"] = (checkBoxSmartOptimization.Checked ? 1 : 0).ToString(CultureInfo.InvariantCulture);
            if (!loading) Character.OnItemsChanged();
        }
	}
}
