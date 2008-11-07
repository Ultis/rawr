using System;
using System.Collections.Generic;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Rawr.Healadin
{
    public partial class CalculationOptionsPanelHealadin : CalculationOptionsPanelBase
    {
        public CalculationOptionsPanelHealadin()
        {
            InitializeComponent();
        }

        private bool loading;

        protected override void LoadCalculationOptions()
        {
            loading = true;
            if (Character.CalculationOptions == null)
                Character.CalculationOptions = new CalculationOptionsHealadin();

			CalculationOptionsHealadin calcOpts = Character.CalculationOptions as CalculationOptionsHealadin;
            cmbLength.Value = (decimal)calcOpts.Length;
            cmbManaAmt.Text = calcOpts.ManaAmt.ToString();

            nudSpiritual.Value = (decimal)calcOpts.Spiritual;
            nudDivinePlea.Value = (decimal)calcOpts.DivinePlea;

			trkActivity.Value = (int)(calcOpts.Activity * 100);
            lblActivity.Text = trkActivity.Value + "%";

            chkJotP.Checked = calcOpts.JotP;

            trkReplenishment.Value = (int)(calcOpts.Replenishment * 100);
            lblReplenishment.Text = trkReplenishment.Value + "%";

            trkBoLUp.Value = (int)(calcOpts.BoLUp * 100);
            lblBoLUp.Text = trkBoLUp.Value + "%";

            trkBoLEff.Value = (int)(calcOpts.BoLEff * 100);
            lblBoLEff.Text = trkBoLEff.Value + "%";

            trkRatio.Value = (int)(calcOpts.Ratio * 100);
            lblRatio1.Text = trkRatio.Value + "%";
            lblRatio2.Text = (100 - trkRatio.Value) + "%";

            loading = false;
        }
 
        private void cmbLength_ValueChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsHealadin calcOpts = Character.CalculationOptions as CalculationOptionsHealadin;
                calcOpts.Length = (float)cmbLength.Value;
                Character.OnCalculationsInvalidated();
            }
        }

        private void cmbManaAmt_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsHealadin calcOpts = Character.CalculationOptions as CalculationOptionsHealadin;
                try
                {
                    calcOpts.ManaAmt = float.Parse(cmbManaAmt.Text);
                }
                catch { }
                Character.OnCalculationsInvalidated();
            }
        }

        private void cmbManaAmt_TextUpdate(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsHealadin calcOpts = Character.CalculationOptions as CalculationOptionsHealadin;
                try
                {
                    calcOpts.ManaAmt = float.Parse(cmbManaAmt.Text);
                }
                catch { }
                Character.OnCalculationsInvalidated();
            }
        }

        private void trkActivity_Scroll(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsHealadin calcOpts = Character.CalculationOptions as CalculationOptionsHealadin;
                lblActivity.Text = trkActivity.Value + "%";
                calcOpts.Activity = trkActivity.Value / 100f;
                Character.OnCalculationsInvalidated();
            }
        }

        private void trkReplenishment_Scroll(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsHealadin calcOpts = Character.CalculationOptions as CalculationOptionsHealadin;
                lblReplenishment.Text = trkReplenishment.Value + "%";
                calcOpts.Replenishment = trkReplenishment.Value / 100f;
                Character.OnCalculationsInvalidated();
            }
        }

        private void nudSpiritual_ValueChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsHealadin calcOpts = Character.CalculationOptions as CalculationOptionsHealadin;
                calcOpts.Spiritual = (float)nudSpiritual.Value;
                Character.OnCalculationsInvalidated();
            }
        }

        private void nudDivinePlea_ValueChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsHealadin calcOpts = Character.CalculationOptions as CalculationOptionsHealadin;
                calcOpts.DivinePlea = (float)nudDivinePlea.Value;
                Character.OnCalculationsInvalidated();
            }
        }

        private void chkJotP_CheckedChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsHealadin calcOpts = Character.CalculationOptions as CalculationOptionsHealadin;
                calcOpts.JotP = chkJotP.Checked;
                Character.OnCalculationsInvalidated();
            }
        }

        private void trkBoLUp_Scroll(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsHealadin calcOpts = Character.CalculationOptions as CalculationOptionsHealadin;
                lblBoLUp.Text = trkBoLUp.Value + "%";
                calcOpts.BoLUp = trkBoLUp.Value / 100f;
                Character.OnCalculationsInvalidated();
            }
        }

        private void trkBoLEff_Scroll(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsHealadin calcOpts = Character.CalculationOptions as CalculationOptionsHealadin;
                lblBoLEff.Text = trkBoLEff.Value + "%";
                calcOpts.BoLEff = trkBoLEff.Value / 100f;
                Character.OnCalculationsInvalidated();
            }
        }

        private void trkRatio_Scroll(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsHealadin calcOpts = Character.CalculationOptions as CalculationOptionsHealadin;
                calcOpts.Ratio = trkRatio.Value / 100f;
                lblRatio1.Text = trkRatio.Value + "%";
                lblRatio2.Text = (100 - trkRatio.Value) + "%";
                Character.OnCalculationsInvalidated();
            }
        }



    }

	[Serializable]
	public class CalculationOptionsHealadin : ICalculationOptionBase
	{
		public string GetXml()
		{
			System.Xml.Serialization.XmlSerializer serializer =
				new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptionsHealadin));
			StringBuilder xml = new StringBuilder();
			System.IO.StringWriter writer = new System.IO.StringWriter(xml);
			serializer.Serialize(writer, this);
			return xml.ToString();
		}

		public float Length = 6;
		public float ManaAmt = 2400;
		public float Activity = .85f;
		public float Spiritual = 3600;
        public float Replenishment = .9f;
        public float DivinePlea = 1.5f;
        public float BoLUp = 1f;
        public float BoLEff = .3f;
        public float Ratio = .2f;
        public bool JotP = true;
	}
}
