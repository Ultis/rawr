using System;
using System.Collections.Generic;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Rawr.HolyPriest
{
    public partial class CalculationOptionsPanelHolyPriest : CalculationOptionsPanelBase
    {
        public CharacterCalculationsHolyPriest Calculations;

        public CalculationOptionsPanelHolyPriest()
        {
            InitializeComponent();
        }

        private bool loading;

        protected override void LoadCalculationOptions()
        {
            loading = true;
            if (Character.CalculationOptions == null)
                Character.CalculationOptions = new CalculationOptionsPriest();

            CalculationOptionsPriest calcOpts = Character.CalculationOptions as CalculationOptionsPriest;
            
            trkActivity.Value = (int)calcOpts.TimeInFSR;
            lblActivity.Text = trkActivity.Value + "%";

            loading = false;
        }
               
        
        private void trkActivity_Scroll(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsPriest calcOpts = Character.CalculationOptions as CalculationOptionsPriest;
                lblActivity.Text = trkActivity.Value + "%";
                calcOpts.TimeInFSR = trkActivity.Value;
                Character.OnCalculationsInvalidated();
            }
        }
    }

    [Serializable]
	public class CalculationOptionsPriest : ICalculationOptionBase
	{
		public string GetXml()
		{
			System.Xml.Serialization.XmlSerializer serializer =
                new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptionsPriest));
			StringBuilder xml = new StringBuilder();
			System.IO.StringWriter writer = new System.IO.StringWriter(xml);
			serializer.Serialize(writer, this);
			return xml.ToString();
		}

		public bool EnforceMetagemRequirements = false;
		public float TimeInFSR = 80;
	}
}
