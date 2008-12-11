using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Rawr
{
	public partial class CalculationOptionsPanelElemental : CalculationOptionsPanelBase
	{
		private Dictionary<int, string> armorBosses = new Dictionary<int, string>();

		public CalculationOptionsPanelElemental()
		{
			InitializeComponent();
		}

		protected override void LoadCalculationOptions()
		{
			_loadingCalculationOptions = true;
			if (Character.CalculationOptions == null)
				Character.CalculationOptions = new CalculationOptionsElemental();

			CalculationOptionsElemental calcOpts = Character.CalculationOptions as CalculationOptionsElemental;
			comboBoxTargetLevel.SelectedItem = calcOpts.TargetLevel.ToString();
			numericUpDownDuration.Value = calcOpts.Duration;
			
			_loadingCalculationOptions = false;
		}

		private bool _loadingCalculationOptions = false;
		private void calculationOptionControl_Changed(object sender, EventArgs e)
		{
			if (!_loadingCalculationOptions)
			{
				CalculationOptionsElemental calcOpts = Character.CalculationOptions as CalculationOptionsElemental;
				calcOpts.TargetLevel = int.Parse(comboBoxTargetLevel.SelectedItem.ToString());
				calcOpts.Duration = (int)numericUpDownDuration.Value;
				
				Character.OnCalculationsInvalidated();
			}
		}

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

	}

	[Serializable]
	public class CalculationOptionsElemental : ICalculationOptionBase
	{
		public string GetXml()
		{
			System.Xml.Serialization.XmlSerializer serializer = 
				new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptionsElemental));
			StringBuilder xml = new StringBuilder();
			System.IO.StringWriter writer = new System.IO.StringWriter(xml);
			serializer.Serialize(writer, this);
			return xml.ToString();
		}

		public int TargetLevel = 83;
		public bool GlyphOfLightningBolt = true;
		public bool GlyphOfFlameShock = true;
        public bool GlyphOfFlametongueWeapon = true;
        public bool GlyphOfWaterShield = false;
		public int Duration = 300;
	}
}
