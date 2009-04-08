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
		}

		protected override void LoadCalculationOptions()
		{
			_loadingCalculationOptions = true;
			if (Character.CalculationOptions == null)
				Character.CalculationOptions = new CalculationOptionsCat();

			CalculationOptionsCat calcOpts = Character.CalculationOptions as CalculationOptionsCat;
			comboBoxTargetLevel.SelectedItem = calcOpts.TargetLevel.ToString();
			numericUpDownTargetArmor.Value = calcOpts.TargetArmor;
			checkBoxFerociousBite.Checked = calcOpts.CustomUseFerociousBite;
			checkBoxRip.Checked = calcOpts.CustomUseRip;
			checkBoxRake.Checked = calcOpts.CustomUseRake;
			checkBoxShred.Checked = calcOpts.CustomUseShred;
			comboBoxSavageRoar.SelectedItem = calcOpts.CustomCPSavageRoar.ToString();
			numericUpDownDuration.Value = calcOpts.Duration;
			
			_loadingCalculationOptions = false;
		}

		private bool _loadingCalculationOptions = false;
		private void calculationOptionControl_Changed(object sender, EventArgs e)
		{
			if (!_loadingCalculationOptions)
			{
				CalculationOptionsCat calcOpts = Character.CalculationOptions as CalculationOptionsCat;
				calcOpts.TargetLevel = int.Parse(comboBoxTargetLevel.SelectedItem.ToString());
				calcOpts.TargetArmor = (int)numericUpDownTargetArmor.Value;
				calcOpts.CustomUseFerociousBite = checkBoxFerociousBite.Checked;
				calcOpts.CustomUseRip = checkBoxRip.Checked;
				calcOpts.CustomUseRake = checkBoxRake.Checked;
				calcOpts.CustomUseShred = checkBoxShred.Checked;
				calcOpts.CustomCPSavageRoar = int.Parse(comboBoxSavageRoar.SelectedItem.ToString());
				calcOpts.Duration = (int)numericUpDownDuration.Value;
				
				Character.OnCalculationsInvalidated();
			}
		}

	}

	[Serializable]
	public class CalculationOptionsCat : ICalculationOptionBase
	{
		public string GetXml()
		{
			System.Xml.Serialization.XmlSerializer serializer = 
				new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptionsCat));
			StringBuilder xml = new StringBuilder();
			System.IO.StringWriter writer = new System.IO.StringWriter(xml);
			serializer.Serialize(writer, this);
			return xml.ToString();
		}

		public int TargetLevel = 83;
		public int TargetArmor = 10645;
		public bool CustomUseShred = false;
		public bool CustomUseRip = false;
		public bool CustomUseRake = false;
		public bool CustomUseFerociousBite = false;
		public int CustomCPSavageRoar = 2;
		public int Duration = 300;
	}
}
