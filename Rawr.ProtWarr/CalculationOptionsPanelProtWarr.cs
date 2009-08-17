using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Rawr.CustomControls;

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
            armorBosses.Add((int)StatConversion.NPC_ARMOR[80 - 80], ": Level 80 Mobs");
            armorBosses.Add((int)StatConversion.NPC_ARMOR[81 - 80], ": Level 81 Mobs");
            armorBosses.Add((int)StatConversion.NPC_ARMOR[82 - 80], ": Level 82 Mobs");
            armorBosses.Add((int)StatConversion.NPC_ARMOR[83 - 80], ": Ulduar Bosses");
        }

		protected override void LoadCalculationOptions()
		{
			_loadingCalculationOptions = true;
			if (Character.CalculationOptions == null)
				Character.CalculationOptions = new CalculationOptionsProtWarr();

			CalculationOptionsProtWarr calcOpts = Character.CalculationOptions as CalculationOptionsProtWarr;

            // Attacker Stats
			comboBoxTargetLevel.SelectedItem = calcOpts.TargetLevel.ToString();
			trackBarTargetArmor.Value = calcOpts.TargetArmor;
            trackBarBossAttackValue.Value = calcOpts.BossAttackValue;
            trackBarBossAttackSpeed.Value = (int)(calcOpts.BossAttackSpeed / 0.25f);
            checkBoxUseParryHaste.Checked = calcOpts.UseParryHaste;
            // Stupid hack since you can't put in newlines into the VS editor properties
            extendedToolTipUseParryHaste.ToolTipText =
                extendedToolTipUseParryHaste.ToolTipText.Replace("May not", Environment.NewLine + "May not");

            // Ranking System
            if (calcOpts.ThreatScale > 24.0f) // Old scale value being saved, reset to default
                calcOpts.ThreatScale = 8.0f;
            trackBarThreatScale.Value = Convert.ToInt32(calcOpts.ThreatScale / 8.0f / 0.1f);
            if (calcOpts.MitigationScale > 1.0f) // Old scale value being saved, reset to default
                calcOpts.MitigationScale = (1.0f / 8.0f);
            trackBarMitigationScale.Value = Convert.ToInt32((calcOpts.MitigationScale * 8.0f / 0.1f));
            radioButtonMitigationScale.Checked = (calcOpts.RankingMode == 1);
            radioButtonTankPoints.Checked = (calcOpts.RankingMode == 2);
            radioButtonBurstTime.Checked = (calcOpts.RankingMode == 3);
            radioButtonDamageOutput.Checked = (calcOpts.RankingMode == 4);
            trackBarThreatScale.Enabled = labelThreatScale.Enabled = (calcOpts.RankingMode != 4);
            trackBarMitigationScale.Enabled = labelMitigationScale.Enabled = (calcOpts.RankingMode == 1);

            // Warrior Abilities
            trackBarVigilanceValue.Value = (int)calcOpts.VigilanceValue;
            checkBoxUseVigilance.Checked = calcOpts.UseVigilance;
            trackBarVigilanceValue.Enabled = calcOpts.UseVigilance;
            trackBarShieldBlockUptime.Value = (int)calcOpts.ShieldBlockUptime;
            checkBoxUseShieldBlock.Checked = calcOpts.UseShieldBlock;
			
			labelTargetArmorDescription.Text = trackBarTargetArmor.Value.ToString() + (armorBosses.ContainsKey(trackBarTargetArmor.Value) ? armorBosses[trackBarTargetArmor.Value] : "");
            labelBossAttackValue.Text = trackBarBossAttackValue.Value.ToString();
            labelBossAttackSpeed.Text = String.Format("{0:0.00}s", ((float)(trackBarBossAttackSpeed.Value) * 0.25f));
            labelThreatScale.Text = String.Format("{0:0.0}", ((float)(trackBarThreatScale.Value) * 0.1f));
            labelMitigationScale.Text = String.Format("{0:0.0}", ((float)(trackBarMitigationScale.Value) * 0.1f));
            labelShieldBlockUptime.Text = trackBarShieldBlockUptime.Value.ToString() + "%";

			_loadingCalculationOptions = false;
		}

		private bool _loadingCalculationOptions = false;
		private void calculationOptionControl_Changed(object sender, EventArgs e)
		{
			if (!_loadingCalculationOptions)
			{
				CalculationOptionsProtWarr calcOpts = Character.CalculationOptions as CalculationOptionsProtWarr;
				// Attacker Stats
                trackBarTargetArmor.Value = 100 * (trackBarTargetArmor.Value / 100);
				labelTargetArmorDescription.Text = trackBarTargetArmor.Value.ToString() + (armorBosses.ContainsKey(trackBarTargetArmor.Value) ? armorBosses[trackBarTargetArmor.Value] : "");
                trackBarBossAttackValue.Value = 500 * (trackBarBossAttackValue.Value / 500);
                labelBossAttackValue.Text = trackBarBossAttackValue.Value.ToString();
                labelBossAttackSpeed.Text = String.Format("{0:0.00}s", ((float)(trackBarBossAttackSpeed.Value) * 0.25f));
				// Ranking System
                labelThreatScale.Text = String.Format("{0:0.0}", ((float)(trackBarThreatScale.Value) * 0.1f));
				labelMitigationScale.Text = String.Format("{0:0.0}", ((float)(trackBarMitigationScale.Value) * 0.1f));
                // Warrior Abilities
                trackBarVigilanceValue.Value = 500 * (trackBarVigilanceValue.Value / 500);
                labelVigilanceValue.Text = trackBarVigilanceValue.Value.ToString();
                labelShieldBlockUptime.Text = trackBarShieldBlockUptime.Value.ToString() + "%";

				calcOpts.TargetLevel = int.Parse(comboBoxTargetLevel.SelectedItem.ToString());
				calcOpts.TargetArmor = trackBarTargetArmor.Value;
                calcOpts.BossAttackValue = trackBarBossAttackValue.Value;
                calcOpts.BossAttackSpeed = ((float)(trackBarBossAttackSpeed.Value) * 0.25f);
                calcOpts.ThreatScale = ((float)(trackBarThreatScale.Value) * 0.1f * 8.0f);
                calcOpts.MitigationScale = ((float)(trackBarMitigationScale.Value) * 0.1f / 8.0f);
                calcOpts.VigilanceValue = trackBarVigilanceValue.Value;
                calcOpts.ShieldBlockUptime = trackBarShieldBlockUptime.Value;

				Character.OnCalculationsInvalidated();
			}
		}

        private void checkBoxUseParryHaste_CheckedChanged(object sender, EventArgs e)
        {
            if (!_loadingCalculationOptions)
            {
                CalculationOptionsProtWarr calcOpts = Character.CalculationOptions as CalculationOptionsProtWarr;
                calcOpts.UseParryHaste = checkBoxUseParryHaste.Checked;
                Character.OnCalculationsInvalidated();
            }
        }

        private void radioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (!_loadingCalculationOptions)
            {
                CalculationOptionsProtWarr calcOpts = Character.CalculationOptions as CalculationOptionsProtWarr;
                if (radioButtonTankPoints.Checked)
                {
                    calcOpts.RankingMode = 2;
                    trackBarThreatScale.Value = 10;
                }
                else if (radioButtonBurstTime.Checked)
                {
                    calcOpts.RankingMode = 3;
                    trackBarThreatScale.Value = 0;
                }
                else if (radioButtonDamageOutput.Checked)
                {
                    calcOpts.RankingMode = 4;
                    trackBarThreatScale.Value = 10;
                }
                else
                {
                    calcOpts.RankingMode = 1;
                    trackBarThreatScale.Value = 10;
                }
                trackBarThreatScale.Enabled = labelThreatScale.Enabled = (calcOpts.RankingMode != 4);
                trackBarMitigationScale.Enabled = labelMitigationScale.Enabled = (calcOpts.RankingMode == 1);

                Character.OnCalculationsInvalidated();
            }
        }

        private void checkBoxUseVigilance_CheckedChanged(object sender, EventArgs e)
        {
            if (!_loadingCalculationOptions)
            {
                CalculationOptionsProtWarr calcOpts = Character.CalculationOptions as CalculationOptionsProtWarr;
                calcOpts.UseVigilance = checkBoxUseVigilance.Checked;
                trackBarVigilanceValue.Enabled = checkBoxUseVigilance.Checked;
                Character.OnCalculationsInvalidated();
            }
        }

		private void checkBoxUseShieldBlock_CheckedChanged(object sender, EventArgs e)
        {
            if (!_loadingCalculationOptions)
            {
                CalculationOptionsProtWarr calcOpts = Character.CalculationOptions as CalculationOptionsProtWarr;
                calcOpts.UseShieldBlock = checkBoxUseShieldBlock.Checked;
                Character.OnCalculationsInvalidated();
            }
        }

        private void extendedToolTipMitigtionScale_Click(object sender, EventArgs e)
        {
            if (!radioButtonMitigationScale.Checked)
                radioButtonMitigationScale.Checked = true;
        }

        private void extendedToolTipTankPoints_Click(object sender, EventArgs e)
        {
            if (!radioButtonTankPoints.Checked)
                radioButtonTankPoints.Checked = true;
        }

        private void extendedToolTipBurstTime_Click(object sender, EventArgs e)
        {
            if (!radioButtonBurstTime.Checked)
                radioButtonBurstTime.Checked = true;
        }

        private void extendedToolTipDamageOutput_Click(object sender, EventArgs e)
        {
            if (!radioButtonDamageOutput.Checked)
                radioButtonDamageOutput.Checked = true;
        }
	}

	[Serializable]
	public class CalculationOptionsProtWarr : ICalculationOptionBase
	{
		public string GetXml()
		{
			System.Xml.Serialization.XmlSerializer serializer =
				new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptionsProtWarr));
			StringBuilder xml = new StringBuilder();
			System.IO.StringWriter writer = new System.IO.StringWriter(xml);
			serializer.Serialize(writer, this);
			return xml.ToString();
		}

		public int TargetLevel = 83;
        public int TargetArmor = (int)StatConversion.NPC_ARMOR[83-80];
		public int BossAttackValue = 60000;
        public float BossAttackSpeed = 2.0f;
        public bool UseParryHaste = false;
		public float ThreatScale = 8.0f;
        public float MitigationScale = 0.125f;
        public int RankingMode = 1;
        public bool UseVigilance = true;
        public int VigilanceValue = 5000;
        public float ShieldBlockUptime = 100;
		public bool UseShieldBlock = false;
		public WarriorTalents talents = null;
	}
}
