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
			checkBoxEnforceMetagemRequirements.Checked = Character.EnforceMetagemRequirements;

			switch (numericUpDownThreatValue.Value.ToString())
			{
				case "0":
					comboBoxThreatValue.SelectedIndex = 0;
					break;

				case "10":
					comboBoxThreatValue.SelectedIndex = 1;
					break;

				case "50":
					comboBoxThreatValue.SelectedIndex = 2;
					break;

				case "100":
					comboBoxThreatValue.SelectedIndex = 3;
					break;

				default:
					comboBoxThreatValue.SelectedIndex = 4;
					break;
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
				Character.EnforceMetagemRequirements = checkBoxEnforceMetagemRequirements.Checked;

				Character.OnItemsChanged();
			}
        }
		private void comboBoxThreatValue_SelectedIndexChanged(object sender, EventArgs e)
        {
			numericUpDownThreatValue.Enabled = comboBoxThreatValue.SelectedIndex == 4;
			if (comboBoxThreatValue.SelectedIndex < 4)
				numericUpDownThreatValue.Value = (new decimal[] { 0, 10, 50, 100 })[comboBoxThreatValue.SelectedIndex];
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

		public int TargetLevel = 73;
		public float ThreatScale = 10f;
		public bool EnforceMetagemRequirements = false;
	}
}
