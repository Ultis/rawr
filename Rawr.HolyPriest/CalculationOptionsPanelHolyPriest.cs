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

            cbRotation.SelectedIndex = calcOpts.Rotation;

            trkActivity.Value = (int)calcOpts.TimeInFSR;
            lblActivity.Text = trkActivity.Value + "% of Fight spent in FSR.";

            trkFightLength.Value = (int)calcOpts.FightLength;
            lblFightLength.Text = trkFightLength.Value + " minute Fight.";

            trkSerendipity.Value = (int)calcOpts.Serendipity;
            lblSerendipity.Text = trkSerendipity.Value + "% Heals give Serendipity.";

            loading = false;
        }
               
        
        private void trkActivity_Scroll(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsPriest calcOpts = Character.CalculationOptions as CalculationOptionsPriest;
                lblActivity.Text = trkActivity.Value + "% of Fight spent in FSR.";
                calcOpts.TimeInFSR = trkActivity.Value;
                Character.OnCalculationsInvalidated();
            }
        }

        private void cbRotation_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsPriest calcOpts = Character.CalculationOptions as CalculationOptionsPriest;
                calcOpts.Rotation = cbRotation.SelectedIndex;
                Character.OnCalculationsInvalidated();
            }
        }

        private void trkFightLength_Scroll(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsPriest calcOpts = Character.CalculationOptions as CalculationOptionsPriest;
                lblFightLength.Text = trkFightLength.Value + " minute Fight.";
                calcOpts.FightLength = trkFightLength.Value;
                Character.OnCalculationsInvalidated();
            }

        }

        private void trkSerendipity_Scroll(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsPriest calcOpts = Character.CalculationOptions as CalculationOptionsPriest;
                lblSerendipity.Text = trkSerendipity.Value + "% Heals give Serendipity.";
                calcOpts.Serendipity = trkSerendipity.Value;
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

        public int Rotation = 0;
        public float TimeInFSR = 85f;
        public float FightLength = 5f;
        public float Serendipity = 100f;
	}
}
