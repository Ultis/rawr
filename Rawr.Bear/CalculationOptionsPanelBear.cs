using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Rawr.Bear
{
	public partial class CalculationOptionsPanelBear : CalculationOptionsPanelBase
	{
		public CalculationOptionsPanelBear()
		{
			InitializeComponent();
		}

		protected override void LoadCalculationOptions()
		{
			_loadingCalculationOptions = true;
			if (Character.CalculationOptions == null)
				Character.CalculationOptions = new CalculationOptionsBear();
			//if (!Character.CalculationOptions.ContainsKey("TargetLevel"))
			//    Character.CalculationOptions["TargetLevel"] = "73";
			//if (!Character.CalculationOptions.ContainsKey("ThreatScale"))
			//    Character.CalculationOptions["ThreatScale"] = "1";
			//if (!Character.CalculationOptions.ContainsKey("EnforceMetagemRequirements"))
			//    Character.CalculationOptions["EnforceMetagemRequirements"] = "No";

			CalculationOptionsBear calcOpts = Character.CalculationOptions as CalculationOptionsBear;
			comboBoxTargetLevel.SelectedItem = calcOpts.TargetLevel.ToString();
            numericUpDownThreatValue.Value = (decimal)calcOpts.ThreatScale;
			numericUpDownTargetArmor.Value = (decimal)calcOpts.TargetArmor;
			numericUpDownSurvivalSoftCap.Value = calcOpts.SurvivalSoftCap;
			numericUpDownTargetDamage.Value = calcOpts.TargetDamage;
			numericUpDownTargetAttackSpeed.Value = (decimal)calcOpts.TargetAttackSpeed;

			radioButtonNoAuto.Checked = !calcOpts.CustomUseMaul.HasValue;
			radioButtonMelee.Checked = calcOpts.CustomUseMaul.HasValue && !calcOpts.CustomUseMaul.Value;
			radioButtonMaul.Checked = calcOpts.CustomUseMaul.HasValue && calcOpts.CustomUseMaul.Value;
			checkBoxMangle.Checked = calcOpts.CustomUseMangle;
			checkBoxSwipe.Checked = calcOpts.CustomUseSwipe;
			checkBoxFaerieFire.Checked = calcOpts.CustomUseFaerieFire;
			checkBoxLacerate.Checked = calcOpts.CustomUseLacerate;
			
			switch (numericUpDownThreatValue.Value.ToString())
			{
				case "0": comboBoxThreatValue.SelectedIndex = 0; break;
				case "10": comboBoxThreatValue.SelectedIndex = 1; break; 
				case "50": comboBoxThreatValue.SelectedIndex = 2; break; 
				case "100": comboBoxThreatValue.SelectedIndex = 3; break; 
				default: comboBoxThreatValue.SelectedIndex = 4; break;
			}

			switch (numericUpDownTargetDamage.Value.ToString())
			{
				case "30000": comboBoxTargetDamage.SelectedIndex = 0; break; //Normal Dungeons
				case "37000": comboBoxTargetDamage.SelectedIndex = 1; break; //Heroic Dungeons
				case "40000": comboBoxTargetDamage.SelectedIndex = 2; break; //T7 Raids (10)
				case "47000": comboBoxTargetDamage.SelectedIndex = 3; break; //T7 Raids (25)
				case "55000": comboBoxTargetDamage.SelectedIndex = 4; break; //T8 Raids (10)
				case "75000": comboBoxTargetDamage.SelectedIndex = 5; break; //T8 Raids (10, Hard)
				case "71000": comboBoxTargetDamage.SelectedIndex = 6; break; //T8 Raids (25)
				case "90000": comboBoxTargetDamage.SelectedIndex = 7; break; //T8 Raids (25, Hard)
				case "70000": comboBoxTargetDamage.SelectedIndex = 8; break; //T9 Raids (10)
				case "85000": comboBoxTargetDamage.SelectedIndex = 9; break; //T9 Raids (10, Heroic)
				case "80000": comboBoxTargetDamage.SelectedIndex = 10; break; //T9 Raids (25)
				case "95000": comboBoxTargetDamage.SelectedIndex = 11; break; //T9 Raids (25, Heroic)
				default: comboBoxTargetDamage.SelectedIndex = 12; break; //Custom...
			}

			switch (numericUpDownSurvivalSoftCap.Value.ToString())
			{
				case "90000": comboBoxSurvivalSoftCap.SelectedIndex = 0; break; //Normal Dungeons
				case "110000": comboBoxSurvivalSoftCap.SelectedIndex = 1; break; //Heroic Dungeons
				case "120000": comboBoxSurvivalSoftCap.SelectedIndex = 2; break; //T7 Raids (10)
				case "140000": comboBoxSurvivalSoftCap.SelectedIndex = 3; break; //T7 Raids (25)
				case "170000": comboBoxSurvivalSoftCap.SelectedIndex = 4; break; //T8 Raids (10)
				case "195000": comboBoxSurvivalSoftCap.SelectedIndex = 5; break; //T8 Raids (10, Hard)
				case "185000": comboBoxSurvivalSoftCap.SelectedIndex = 6; break; //T8 Raids (25)
				case "215000": comboBoxSurvivalSoftCap.SelectedIndex = 7; break; //T8 Raids (25, Hard)
				case "180000": comboBoxSurvivalSoftCap.SelectedIndex = 8; break; //T9 Raids (10)
				case "210000": comboBoxSurvivalSoftCap.SelectedIndex = 9; break; //T9 Raids (10, Heroic)
				case "190000": comboBoxSurvivalSoftCap.SelectedIndex = 10; break; //T9 Raids (25)
				case "225000": comboBoxSurvivalSoftCap.SelectedIndex = 11; break; //T9 Raids (25, Heroic)
				default: comboBoxSurvivalSoftCap.SelectedIndex = 12; break;
			}

			_loadingCalculationOptions = false;
		}
	
		private bool _loadingCalculationOptions = false;
		private void calculationOptionControl_Changed(object sender, EventArgs e)
		{
			if (!_loadingCalculationOptions)
			{
				CalculationOptionsBear calcOpts = Character.CalculationOptions as CalculationOptionsBear;
				calcOpts.TargetLevel = int.Parse(comboBoxTargetLevel.SelectedItem.ToString());
				calcOpts.ThreatScale = (float)numericUpDownThreatValue.Value;
				calcOpts.TargetArmor = (int)numericUpDownTargetArmor.Value;
				calcOpts.SurvivalSoftCap = (int)numericUpDownSurvivalSoftCap.Value;
				calcOpts.TargetDamage = (int)numericUpDownTargetDamage.Value;
				calcOpts.TargetAttackSpeed = (float)numericUpDownTargetAttackSpeed.Value;

				if (radioButtonNoAuto.Checked) calcOpts.CustomUseMaul = null;
				else if (radioButtonMelee.Checked) calcOpts.CustomUseMaul = false;
				else if (radioButtonMaul.Checked)  calcOpts.CustomUseMaul = true;
				calcOpts.CustomUseMangle = checkBoxMangle.Checked;
				calcOpts.CustomUseSwipe = checkBoxSwipe.Checked;
				calcOpts.CustomUseFaerieFire = checkBoxFaerieFire.Checked;
				calcOpts.CustomUseLacerate = checkBoxLacerate.Checked;

				Character.OnCalculationsInvalidated();
			}
        }
		private void comboBoxThreatValue_SelectedIndexChanged(object sender, EventArgs e)
        {
			numericUpDownThreatValue.Enabled = comboBoxThreatValue.SelectedIndex == 4;
			if (comboBoxThreatValue.SelectedIndex < 4)
				numericUpDownThreatValue.Value = (new decimal[] { 0.0001M, 10, 50, 100 })[comboBoxThreatValue.SelectedIndex];
		}

		private void comboBoxTargetDamage_SelectedIndexChanged(object sender, EventArgs e)
		{
			numericUpDownTargetDamage.Enabled = comboBoxTargetDamage.SelectedIndex == 12;
			if (comboBoxTargetDamage.SelectedIndex < 12)
				numericUpDownTargetDamage.Value =
					(new decimal[] { 30000, 37000, 40000, 47000, 55000, 75000, 71000, 90000, 70000, 85000, 80000, 95000 })
					[comboBoxTargetDamage.SelectedIndex];
		}

		private void comboBoxSurvivalSoftCap_SelectedIndexChanged(object sender, EventArgs e)
		{
			numericUpDownSurvivalSoftCap.Enabled = comboBoxSurvivalSoftCap.SelectedIndex == 12;
			if (comboBoxSurvivalSoftCap.SelectedIndex < 12)
				numericUpDownSurvivalSoftCap.Value =
					(new decimal[] { 90000, 110000, 120000, 140000, 170000, 195000, 185000, 215000, 180000, 210000, 190000, 225000 })
					[comboBoxSurvivalSoftCap.SelectedIndex];
		}
	}
}
