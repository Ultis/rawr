using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Rawr
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
				case "30000": comboBoxTargetDamage.SelectedIndex = 0; break;
				case "37000": comboBoxTargetDamage.SelectedIndex = 1; break;
				case "40000": comboBoxTargetDamage.SelectedIndex = 2; break;
				case "47000": comboBoxTargetDamage.SelectedIndex = 3; break;
				case "50000": comboBoxTargetDamage.SelectedIndex = 4; break;
				case "57000": comboBoxTargetDamage.SelectedIndex = 5; break;
				default: comboBoxTargetDamage.SelectedIndex = 6; break;
			}

			switch (numericUpDownSurvivalSoftCap.Value.ToString())
			{
				case "90000": comboBoxSurvivalSoftCap.SelectedIndex = 0; break; 
				case "110000": comboBoxSurvivalSoftCap.SelectedIndex = 1; break; 
				case "120000": comboBoxSurvivalSoftCap.SelectedIndex = 2; break; 
				case "140000": comboBoxSurvivalSoftCap.SelectedIndex = 3; break; 
				case "150000": comboBoxSurvivalSoftCap.SelectedIndex = 4; break; 
				case "170000": comboBoxSurvivalSoftCap.SelectedIndex = 5; break; 
				default: comboBoxSurvivalSoftCap.SelectedIndex = 6; break;
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
			numericUpDownTargetDamage.Enabled = comboBoxTargetDamage.SelectedIndex == 6;
			if (comboBoxTargetDamage.SelectedIndex < 6)
				numericUpDownTargetDamage.Value = (new decimal[] { 30000, 37000, 40000, 47000, 50000, 57000 })[comboBoxTargetDamage.SelectedIndex];
		}

		private void comboBoxSurvivalSoftCap_SelectedIndexChanged(object sender, EventArgs e)
		{
			numericUpDownSurvivalSoftCap.Enabled = comboBoxSurvivalSoftCap.SelectedIndex == 6;
			if (comboBoxSurvivalSoftCap.SelectedIndex < 6)
				numericUpDownSurvivalSoftCap.Value = (new decimal[] { 90000, 110000, 120000, 140000, 150000, 170000 })[comboBoxSurvivalSoftCap.SelectedIndex];
		}
	}

	[Serializable]
	public class CalculationOptionsBear : ICalculationOptionBase
	{
		public string GetXml()
		{
			System.Xml.Serialization.XmlSerializer serializer =
				new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptionsBear));
			StringBuilder xml = new StringBuilder();
			System.IO.StringWriter writer = new System.IO.StringWriter(xml);
			serializer.Serialize(writer, this);
			return xml.ToString();
		}

		public int TargetLevel = 83;
		public float ThreatScale = 10f;
		public int TargetArmor = 10645;
		public int SurvivalSoftCap = 140000;
		public int TargetDamage = 50000;
		public float TargetAttackSpeed = 2.0f;

		public bool? CustomUseMaul = null;
		public bool CustomUseMangle = false;
		public bool CustomUseSwipe = false;
		public bool CustomUseFaerieFire = false;
		public bool CustomUseLacerate = false;
	}
}
