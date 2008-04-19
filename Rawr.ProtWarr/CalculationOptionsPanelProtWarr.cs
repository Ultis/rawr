using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Rawr.ProtWarr
{
	public partial class CalculationOptionsPanelProtWarr : CalculationOptionsPanelBase
	{
		private Dictionary<int, string> armorBosses = new Dictionary<int, string>();

		public CalculationOptionsPanelProtWarr()
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
            if (!Character.CalculationOptions.ContainsKey("BossAttackValue"))
                Character.CalculationOptions["BossAttackValue"] = "20000";
            if (!Character.CalculationOptions.ContainsKey("ThreatScale"))
                Character.CalculationOptions["ThreatScale"] = "1";
            if (!Character.CalculationOptions.ContainsKey("MitigationScale"))
                Character.CalculationOptions["MitigationScale"] = "2500";
			if (!Character.CalculationOptions.ContainsKey("EnforceMetagemRequirements"))
				Character.CalculationOptions["EnforceMetagemRequirements"] = "No";
            if (!Character.CalculationOptions.ContainsKey("ShieldBlockUptime"))
                Character.CalculationOptions["ShieldBlockUptime"] = "100";
            if (!Character.CalculationOptions.ContainsKey("UseShieldBlock"))
                Character.CalculationOptions["UseShieldBlock"] = "No";
			if (!Character.CalculationOptions.ContainsKey("ShattrathFaction"))
                Character.CalculationOptions["ShattrathFaction"] = "Scryer";

			comboBoxTargetLevel.SelectedItem = Character.CalculationOptions["TargetLevel"];
			trackBarTargetArmor.Value = int.Parse(Character.CalculationOptions["TargetArmor"]);
            trackBarBossAttackValue.Value = int.Parse(Character.CalculationOptions["BossAttackValue"]);
            trackBarThreatScale.Value = int.Parse(Character.CalculationOptions["ThreatScale"]);
            trackBarMitigationScale.Value = int.Parse(Character.CalculationOptions["MitigationScale"]);
            trackBarShieldBlockUptime.Value = int.Parse(Character.CalculationOptions["ShieldBlockUptime"]);
            checkBoxUseShieldBlock.Checked = Character.CalculationOptions["UseShieldBlock"] == "Yes";
			checkBoxEnforceMetagemRequirements.Checked = Character.CalculationOptions["EnforceMetagemRequirements"] == "Yes";
			radioButtonAldor.Checked = Character.CalculationOptions["ShattrathFaction"] == "Aldor";
			radioButtonScryer.Checked = Character.CalculationOptions["ShattrathFaction"] == "Scryer";
			
			labelTargetArmorDescription.Text = trackBarTargetArmor.Value.ToString() + (armorBosses.ContainsKey(trackBarTargetArmor.Value) ? armorBosses[trackBarTargetArmor.Value] : "");
            labelBossAttackValue.Text = trackBarBossAttackValue.Value.ToString();
            labelThreatScale.Text = trackBarThreatScale.Value.ToString();
			labelMitigationScale.Text = trackBarMitigationScale.Value.ToString();
            labelShieldBlockUptime.Text = trackBarShieldBlockUptime.Value.ToString() + "%";

			_loadingCalculationOptions = false;
		}

		private bool _loadingCalculationOptions = false;
		private void calculationOptionControl_Changed(object sender, EventArgs e)
		{
			if (!_loadingCalculationOptions)
			{
				trackBarTargetArmor.Value = 100 * (trackBarTargetArmor.Value / 100);
				labelTargetArmorDescription.Text = trackBarTargetArmor.Value.ToString() + (armorBosses.ContainsKey(trackBarTargetArmor.Value) ? armorBosses[trackBarTargetArmor.Value] : "");
				labelBossAttackValue.Text = trackBarBossAttackValue.Value.ToString();
				labelThreatScale.Text = trackBarThreatScale.Value.ToString();
				labelMitigationScale.Text = trackBarMitigationScale.Value.ToString();
                labelShieldBlockUptime.Text = trackBarShieldBlockUptime.Value.ToString() + "%";

				Character.CalculationOptions["TargetLevel"] = comboBoxTargetLevel.SelectedItem.ToString();
				Character.CalculationOptions["TargetArmor"] = trackBarTargetArmor.Value.ToString();
                Character.CalculationOptions["BossAttackValue"] = trackBarBossAttackValue.Value.ToString();
                Character.CalculationOptions["ThreatScale"] = trackBarThreatScale.Value.ToString();
                Character.CalculationOptions["MitigationScale"] = trackBarMitigationScale.Value.ToString();
                Character.CalculationOptions["ShieldBlockUptime"] = trackBarShieldBlockUptime.Value.ToString();
				Character.CalculationOptions["ShattrathFaction"] = radioButtonAldor.Checked ? "Aldor" : "Scryer";

				Character.OnItemsChanged();
			}
		}

		private void checkBoxEnforceMetagemRequirements_CheckedChanged(object sender, EventArgs e)
		{
			Character.CalculationOptions["EnforceMetagemRequirements"] = checkBoxEnforceMetagemRequirements.Checked ? "Yes" : "No";
			Character.OnItemsChanged();
		}

        private void checkBoxUseShieldBlock_CheckedChanged(object sender, EventArgs e)
        {
            Character.CalculationOptions["UseShieldBlock"] = checkBoxUseShieldBlock.Checked ? "Yes" : "No";
            Character.OnItemsChanged();
        }

        private void buttonTalents_Click(object sender, EventArgs e)
        {
            if (Character.Talents != null)
            {
                TalentForm tf = new TalentForm();
                tf.SetParameters(Character.Talents, Character.Class);
                tf.Show();
            }
            else
            {
                MessageBox.Show("No talents found");
            }
        }
	}
}
