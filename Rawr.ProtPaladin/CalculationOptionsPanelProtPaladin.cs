using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Rawr.CustomControls;

namespace Rawr.ProtPaladin
{
	public partial class CalculationOptionsPanelProtPaladin : CalculationOptionsPanelBase
	{
		private Dictionary<int, string> armorBosses = new Dictionary<int, string>();

		public CalculationOptionsPanelProtPaladin()
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
			armorBosses.Add((int)StatConversion.NPC_ARMOR[80-80], ": Level 80 Warrior Type Creature");//9729
			armorBosses.Add((int)StatConversion.NPC_ARMOR[81-80], ": Level 81 Warrior Type Creature");//10026 own value
			armorBosses.Add((int)StatConversion.NPC_ARMOR[82-80], ": Level 82 Warrior Type Creature");//10331 own value
            armorBosses.Add((int)StatConversion.NPC_ARMOR[83-80], ": Wotlk Bosses in 3.1");
            armorBosses.Add(13100, ": Tier 7 Bosses in 3.08");
		}

		protected override void LoadCalculationOptions()
		{
			_loadingCalculationOptions = true;
			if (Character.CalculationOptions == null)
				Character.CalculationOptions = new CalculationOptionsProtPaladin();

			CalculationOptionsProtPaladin calcOpts = Character.CalculationOptions as CalculationOptionsProtPaladin;
            PaladinTalents Talents = Character.PaladinTalents;

            CK_PTRMode.Checked = calcOpts.PTRMode;

            // Attacker Stats
            comboBoxTargetType.SelectedItem = calcOpts.TargetType.ToString();
            numericUpDownTargetLevel.Value = calcOpts.TargetLevel;
			trackBarTargetArmor.Value = calcOpts.TargetArmor;

            trackBarBossAttackValue.Value = calcOpts.BossAttackValue;
            trackBarBossAttackSpeed.Value = (int)(calcOpts.BossAttackSpeed / 0.25f);

            comboBoxMagicDamageType.SelectedItem = calcOpts.MagicDamageType.ToString();
            trackBarBossAttackValueMagic.Value = calcOpts.BossAttackValueMagic;
            trackBarBossAttackSpeedMagic.Value = (int)(calcOpts.BossAttackSpeedMagic / 0.25f);

            checkBoxUseParryHaste.Checked = calcOpts.UseParryHaste;
            // Stupid hack since you can't put in newlines into the VS editor properties
            extendedToolTipUseParryHaste.ToolTipText =
                extendedToolTipUseParryHaste.ToolTipText.Replace("May not", Environment.NewLine + "May not");
            extendedToolTipDamageTakenMode.ToolTipText =
                extendedToolTipDamageTakenMode.ToolTipText.Replace("10", Environment.NewLine + "10");
            extendedToolTipMitigationScale.ToolTipText =
                extendedToolTipMitigationScale.ToolTipText.Replace("Mitigation", Environment.NewLine + "Mitigation");

            // Ranking System
            if (calcOpts.ThreatScale > 30.0f) // Old scale value being saved, reset to default
                calcOpts.ThreatScale = 10.0f;
            trackBarThreatScale.Value = Convert.ToInt32(calcOpts.ThreatScale / 10.0f / 0.1f);
            if (calcOpts.MitigationScale > 51000.0f) // Old scale value being saved, reset to default
                calcOpts.MitigationScale = 17000.0f;
            trackBarMitigationScale.Value = Convert.ToInt32((calcOpts.MitigationScale / 17000.0f / 0.1f)); 
            radioButtonMitigationScale.Checked = (calcOpts.RankingMode == 1);
            radioButtonTankPoints.Checked = (calcOpts.RankingMode == 2);
            radioButtonBurstTime.Checked = (calcOpts.RankingMode == 3);
            radioButtonDamageOutput.Checked = (calcOpts.RankingMode == 4);
            radioButtonProtWarrMode.Checked = (calcOpts.RankingMode == 5);
            radioButtonDamageTakenMode.Checked = (calcOpts.RankingMode == 6);
            trackBarThreatScale.Enabled = labelThreatScale.Enabled = (calcOpts.RankingMode != 4);
            trackBarMitigationScale.Enabled = labelMitigationScale.Enabled = (calcOpts.RankingMode == 1) || (calcOpts.RankingMode == 5) || (calcOpts.RankingMode == 6);
            comboBoxTrinketOnUseHandling.SelectedItem = calcOpts.TrinketOnUseHandling.ToString();
            
            // Seal Choice
            radioButtonSoR.Checked = (calcOpts.SealChoice == "Seal of Righteousness");
            radioButtonSoV.Checked = (calcOpts.SealChoice == "Seal of Vengeance");

            calcOpts.UseHolyShield = checkBoxUseHolyShield.Checked;
			
			labelTargetArmorDescription.Text = trackBarTargetArmor.Value.ToString() + (armorBosses.ContainsKey(trackBarTargetArmor.Value) ? armorBosses[trackBarTargetArmor.Value] : "");
            labelBossAttackValue.Text = trackBarBossAttackValue.Value.ToString();
            labelBossAttackSpeed.Text = String.Format("{0:0.00}s", ((float)(trackBarBossAttackSpeed.Value) * 0.25f));

            labelBossMagicalDamage.Text = trackBarBossAttackValueMagic.Value.ToString(); ;
            labelBossMagicSpeed.Text = String.Format("{0:0.00}s", ((float)(trackBarBossAttackSpeedMagic.Value) * 0.25f));
            labelThreatScale.Text = String.Format("{0:0.0}", ((float)(trackBarThreatScale.Value) * 0.1f));
            labelMitigationScale.Text = String.Format("{0:0.0}", ((float)(trackBarMitigationScale.Value) * 0.1f));

			_loadingCalculationOptions = false;
		}

		private bool _loadingCalculationOptions = false;
		private void calculationOptionControl_Changed(object sender, EventArgs e)
		{
			if (!_loadingCalculationOptions)
			{
				CalculationOptionsProtPaladin calcOpts = Character.CalculationOptions as CalculationOptionsProtPaladin;
				// Attacker Stats
                trackBarTargetArmor.Value = trackBarTargetArmor.Value;
				labelTargetArmorDescription.Text = trackBarTargetArmor.Value.ToString() + (armorBosses.ContainsKey(trackBarTargetArmor.Value) ? armorBosses[trackBarTargetArmor.Value] : "");
                trackBarBossAttackValue.Value = 500 * (trackBarBossAttackValue.Value / 500);
                labelBossAttackValue.Text = trackBarBossAttackValue.Value.ToString();
                labelBossAttackSpeed.Text = String.Format("{0:0.00}s", ((float)(trackBarBossAttackSpeed.Value) * 0.25f));
                labelBossMagicalDamage.Text = trackBarBossAttackValueMagic.Value.ToString(); ;
                labelBossMagicSpeed.Text = String.Format("{0:0.00}s", ((float)(trackBarBossAttackSpeedMagic.Value) * 0.25f));
                // Ranking System
                labelThreatScale.Text = String.Format("{0:0.0}", ((float)(trackBarThreatScale.Value) * 0.1f));
				labelMitigationScale.Text = String.Format("{0:0.0}", ((float)(trackBarMitigationScale.Value) * 0.1f));

				//c alcOpts.TargetLevel = int.Parse(comboBoxTargetLevel.SelectedItem.ToString());
                calcOpts.TargetLevel = (int)numericUpDownTargetLevel.Value;
				calcOpts.TargetArmor = trackBarTargetArmor.Value;
                calcOpts.BossAttackValue = trackBarBossAttackValue.Value;
                calcOpts.BossAttackSpeed = ((float)(trackBarBossAttackSpeed.Value) * 0.25f);
                calcOpts.ThreatScale = ((float)(trackBarThreatScale.Value) * 0.1f * 10.0f);
                calcOpts.MitigationScale = ((float)(trackBarMitigationScale.Value) * 0.1f  * 17000.0f);

				Character.OnCalculationsInvalidated();
			}
		}

        private void checkBoxUseParryHaste_CheckedChanged(object sender, EventArgs e)
        {
            if (!_loadingCalculationOptions)
            {
                CalculationOptionsProtPaladin calcOpts = Character.CalculationOptions as CalculationOptionsProtPaladin;
                calcOpts.UseParryHaste = checkBoxUseParryHaste.Checked;
                Character.OnCalculationsInvalidated();
            }
        }

        private void radioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (!_loadingCalculationOptions)
            {
                CalculationOptionsProtPaladin calcOpts = Character.CalculationOptions as CalculationOptionsProtPaladin;
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
                else if (radioButtonProtWarrMode.Checked)
                {
                    calcOpts.RankingMode = 5;
                    trackBarThreatScale.Value = 10;
                }
                else if (radioButtonDamageTakenMode.Checked)
                {
                    calcOpts.RankingMode = 6;
                    trackBarThreatScale.Value = 10;
                }
                else
                {
                    calcOpts.RankingMode = 1;
                    trackBarThreatScale.Value = 10;
                }
                trackBarThreatScale.Enabled = labelThreatScale.Enabled = (calcOpts.RankingMode != 4);
                trackBarMitigationScale.Enabled = labelMitigationScale.Enabled = (calcOpts.RankingMode == 1) || (calcOpts.RankingMode == 5);

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

        private void extendedToolProtWarrMode_Click(object sender, EventArgs e)
        {
            if (!radioButtonProtWarrMode.Checked)
                radioButtonProtWarrMode.Checked = true;
        }

        private void extendedToolTipDamageTakenMode_Click(object sender, EventArgs e)
        {
            if (!radioButtonDamageTakenMode.Checked)
                radioButtonDamageTakenMode.Checked = true;
        }

        private void radioButtonSealChoice_CheckedChanged(object sender, EventArgs e)
        {
            if (!_loadingCalculationOptions)
            {
                CalculationOptionsProtPaladin calcOpts = Character.CalculationOptions as CalculationOptionsProtPaladin;
                if (radioButtonSoR.Checked)
                {
                    calcOpts.SealChoice = "Seal of Righteousness";
                }
                else
                {
                    calcOpts.SealChoice = "Seal of Vengeance";
                }
                Character.OnCalculationsInvalidated();
            }
        }

        private void checkBoxUseHolyShield_CheckedChanged(object sender, EventArgs e)
        {
            if (!_loadingCalculationOptions)
            {
                CalculationOptionsProtPaladin calcOpts = Character.CalculationOptions as CalculationOptionsProtPaladin;
                calcOpts.UseHolyShield = checkBoxUseHolyShield.Checked;
                Character.OnCalculationsInvalidated();
            }
        }

        private void numericUpDownTargetLevel_ValueChanged(object sender, EventArgs e)
        {
            if (!_loadingCalculationOptions)
            {
                CalculationOptionsProtPaladin calcOpts = Character.CalculationOptions as CalculationOptionsProtPaladin;
                calcOpts.TargetLevel = (int)numericUpDownTargetLevel.Value;
                trackBarTargetArmor.Value = (int)StatConversion.NPC_ARMOR[calcOpts.TargetLevel-80];
                Character.OnCalculationsInvalidated();
            }
        }

        private void comboBoxTargetType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_loadingCalculationOptions)
            {
                CalculationOptionsProtPaladin calcOpts = Character.CalculationOptions as CalculationOptionsProtPaladin;
                calcOpts.TargetType = comboBoxTargetType.SelectedItem.ToString();
                Character.OnCalculationsInvalidated();
            }
        }

        private void comboBoxMagicDamageType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_loadingCalculationOptions)
            {
                CalculationOptionsProtPaladin calcOpts = Character.CalculationOptions as CalculationOptionsProtPaladin;
                calcOpts.MagicDamageType = comboBoxMagicDamageType.SelectedItem.ToString();
                Character.OnCalculationsInvalidated();
            }
        }

		
		private void ComboBoxTrinketOnUseHandling_SelectedIndexChanged(object sender, EventArgs e)
		{
            if (!_loadingCalculationOptions)
            {
                CalculationOptionsProtPaladin calcOpts = Character.CalculationOptions as CalculationOptionsProtPaladin;
                calcOpts.TrinketOnUseHandling = comboBoxTrinketOnUseHandling.SelectedItem.ToString();
                Character.OnCalculationsInvalidated();
            }
		}

        private void CK_PTRMode_CheckedChanged(object sender, EventArgs e) {
            if (!_loadingCalculationOptions) {
                CalculationOptionsProtPaladin calcOpts = Character.CalculationOptions as CalculationOptionsProtPaladin;
                calcOpts.PTRMode = CK_PTRMode.Checked;
                Character.OnCalculationsInvalidated();
            }
        }
	}
}
