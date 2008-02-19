using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Rawr
{
	public partial class CalculationOptionsPanelCat : CalculationOptionsPanelBase
	{
		private Dictionary<int, string> armorBosses = new Dictionary<int, string>();

		public CalculationOptionsPanelCat()
		{
			InitializeComponent();
			armorBosses.Add(3800, ": Shade of Aran");
			armorBosses.Add(4700, ": Roar");
			armorBosses.Add(5500, ": Netherspite");
			armorBosses.Add(6100, ": Julianne, Curator");
			armorBosses.Add(6200, ": Karathress, Vashj, Solarian, Kael'thas, Winterchill, Anetheron, Kaz'rogal, Azgalor, Archimonde, Teron, Shahraz");
			armorBosses.Add(6700, ": Maiden, Illhoof");
			armorBosses.Add(7300, ": Strawman");
			armorBosses.Add(7500, ": Attumen");
			armorBosses.Add(7600, ": Romulo, Nightbane, Malchezaar, Doomwalker");
			armorBosses.Add(7700, ": Hydross, Lurker, Leotheras, Tidewalker, Al'ar, Naj'entus, Supremus, Akama, Gurtogg");
			armorBosses.Add(8200, ": Midnight");
			armorBosses.Add(8800, ": Void Reaver");
		}

		protected override void LoadCalculationOptions()
		{
			_loadingCalculationOptions = true;
			if (!Character.CalculationOptions.ContainsKey("TargetLevel"))
				Character.CalculationOptions["TargetLevel"] = "73";
			if (!Character.CalculationOptions.ContainsKey("TargetArmor"))
				Character.CalculationOptions["TargetArmor"] = "7700";
			if (!Character.CalculationOptions.ContainsKey("ExposeWeaknessAPValue"))
				Character.CalculationOptions["ExposeWeaknessAPValue"] = "200";
			if (!Character.CalculationOptions.ContainsKey("Powershift"))
				Character.CalculationOptions["Powershift"] = "4";
			if (!Character.CalculationOptions.ContainsKey("PrimaryAttack"))
				Character.CalculationOptions["PrimaryAttack"] = "Both";
			if (!Character.CalculationOptions.ContainsKey("Finisher"))
				Character.CalculationOptions["Finisher"] = "Rip";
			if (!Character.CalculationOptions.ContainsKey("EnforceMetagemRequirements"))
				Character.CalculationOptions["EnforceMetagemRequirements"] = "No";
			if (!Character.CalculationOptions.ContainsKey("BloodlustUptime"))
				Character.CalculationOptions["BloodlustUptime"] = "15";
			if (!Character.CalculationOptions.ContainsKey("DrumsOfBattleUptime"))
				Character.CalculationOptions["DrumsOfBattleUptime"] = "25";
			if (!Character.CalculationOptions.ContainsKey("DrumsOfWarUptime"))
				Character.CalculationOptions["DrumsOfWarUptime"] = "25";

			comboBoxTargetLevel.SelectedItem = Character.CalculationOptions["TargetLevel"];
			trackBarTargetArmor.Value = int.Parse(Character.CalculationOptions["TargetArmor"]);
			trackBarExposeWeakness.Value = int.Parse(Character.CalculationOptions["ExposeWeaknessAPValue"]);
			trackBarBloodlustUptime.Value = int.Parse(Character.CalculationOptions["BloodlustUptime"]);
			trackBarDrumsOfBattleUptime.Value = int.Parse(Character.CalculationOptions["DrumsOfBattleUptime"]);
			trackBarDrumsOfWarUptime.Value = int.Parse(Character.CalculationOptions["DrumsOfWarUptime"]);
			comboBoxPowershift.SelectedIndex = int.Parse(Character.CalculationOptions["Powershift"]);
			radioButtonMangle.Checked = Character.CalculationOptions["PrimaryAttack"] == "Mangle";
			radioButtonShred.Checked = Character.CalculationOptions["PrimaryAttack"] == "Shred";
			radioButtonBoth.Checked = Character.CalculationOptions["PrimaryAttack"] == "Both";
			radioButtonRip.Checked = Character.CalculationOptions["Finisher"] == "Rip";
			radioButtonFerociousBite.Checked = Character.CalculationOptions["Finisher"] == "Ferocious Bite";
			radioButtonNone.Checked = Character.CalculationOptions["Finisher"] == "None";
			checkBoxEnforceMetagemRequirements.Checked = Character.CalculationOptions["EnforceMetagemRequirements"] == "Yes";

			labelTargetArmorDescription.Text = trackBarTargetArmor.Value.ToString() + (armorBosses.ContainsKey(trackBarTargetArmor.Value) ? armorBosses[trackBarTargetArmor.Value] : "");
			labelBloodlustUptime.Text = trackBarBloodlustUptime.Value.ToString() + "%";
			labelDrumsOfBattleUptime.Text = trackBarDrumsOfBattleUptime.Value.ToString() + "%";
			labelDrumsOfWarUptime.Text = trackBarDrumsOfWarUptime.Value.ToString() + "%";

			_loadingCalculationOptions = false;
		}

		private bool _loadingCalculationOptions = false;
		private void calculationOptionControl_Changed(object sender, EventArgs e)
		{
			if (!_loadingCalculationOptions)
			{
				trackBarTargetArmor.Value = 100 * (trackBarTargetArmor.Value / 100);
				labelTargetArmorDescription.Text = trackBarTargetArmor.Value.ToString() + (armorBosses.ContainsKey(trackBarTargetArmor.Value) ? armorBosses[trackBarTargetArmor.Value] : "");
				labelExposeWeakness.Text = trackBarExposeWeakness.Value.ToString() + "%";
				labelBloodlustUptime.Text = trackBarBloodlustUptime.Value.ToString() + "%";
				labelDrumsOfBattleUptime.Text = trackBarDrumsOfBattleUptime.Value.ToString() + "%";
				labelDrumsOfWarUptime.Text = trackBarDrumsOfWarUptime.Value.ToString() + "%";

				Character.CalculationOptions["TargetLevel"] = comboBoxTargetLevel.SelectedItem.ToString();
				Character.CalculationOptions["TargetArmor"] = trackBarTargetArmor.Value.ToString();
				Character.CalculationOptions["BloodlustUptime"] = trackBarBloodlustUptime.Value.ToString();
				Character.CalculationOptions["DrumsOfBattleUptime"] = trackBarDrumsOfBattleUptime.Value.ToString();
				Character.CalculationOptions["DrumsOfWarUptime"] = trackBarDrumsOfWarUptime.Value.ToString();
				Character.CalculationOptions["ExposeWeaknessAPValue"] = trackBarExposeWeakness.Value.ToString();
				Character.CalculationOptions["Powershift"] = comboBoxPowershift.SelectedIndex.ToString();
				foreach (RadioButton radioButtonPrimaryAttack in groupBoxPrimaryAttack.Controls)
					if (radioButtonPrimaryAttack.Checked)
						Character.CalculationOptions["PrimaryAttack"] = radioButtonPrimaryAttack.Tag.ToString();
				foreach (RadioButton radioButtonFinisher in groupBoxFinisher.Controls)
					if (radioButtonFinisher.Checked)
						Character.CalculationOptions["Finisher"] = radioButtonFinisher.Tag.ToString();

				Character.OnItemsChanged();
			}
		}

		private void checkBoxEnforceMetagemRequirements_CheckedChanged(object sender, EventArgs e)
		{
			Character.CalculationOptions["EnforceMetagemRequirements"] = checkBoxEnforceMetagemRequirements.Checked ? "Yes" : "No";
			Character.OnItemsChanged();
		}
	}
}
