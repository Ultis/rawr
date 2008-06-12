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
        public CalculationOptionsPanelHolyPriest()
        {
            InitializeComponent();
        }

        private bool loading;

        protected override void LoadCalculationOptions()
        {
            loading = true;
            if (Character.CalculationOptions == null)
                Character.CalculationOptions = new CalculationOptionsHolyPriest();

			CalculationOptionsHolyPriest calcOpts = Character.CalculationOptions as CalculationOptionsHolyPriest;
            cmbLength.Value = (decimal)calcOpts.Length;
            cmbManaAmt.Text = calcOpts.ManaAmt.ToString();
            cmbManaTime.Value = (decimal)calcOpts.ManaTime;
            cmbSpriest.Value = (decimal)calcOpts.Spriest;
            cmbSpiritual.Value = (decimal)calcOpts.Spiritual;

			trkActivity.Value = (int)calcOpts.Activity;
            lblActivity.Text = trkActivity.Value + "%";

            trkRatio.Value = (int)(calcOpts.Ratio * 100);
            labHL1.Text = (100 - trkRatio.Value).ToString() + "%";
            labHL2.Text = trkRatio.Value.ToString() + "%";
            if (calcOpts.Rank1 >= 4 && calcOpts.Rank1 <= 11) nubHL1.Value = (decimal)calcOpts.Rank1;
            if (calcOpts.Rank2 >= 4 && calcOpts.Rank2 <= 11) nubHL2.Value = (decimal)calcOpts.Rank2;

            loading = false;
        }
 
        private void cmbLength_ValueChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsHolyPriest calcOpts = Character.CalculationOptions as CalculationOptionsHolyPriest;
                calcOpts.Length = (float)cmbLength.Value;
                Character.OnItemsChanged();
            }
        }

        private void cmbManaAmt_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsHolyPriest calcOpts = Character.CalculationOptions as CalculationOptionsHolyPriest;
                try
                {
                    calcOpts.ManaAmt = float.Parse(cmbManaAmt.Text);
                }
                catch { }
                Character.OnItemsChanged();
            }
        }

        private void cmbManaTime_ValueChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsHolyPriest calcOpts = Character.CalculationOptions as CalculationOptionsHolyPriest;
                calcOpts.ManaTime = (float)cmbManaTime.Value;
                Character.OnItemsChanged();
            }
        }

        private void cmbManaAmt_TextUpdate(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsHolyPriest calcOpts = Character.CalculationOptions as CalculationOptionsHolyPriest;
                try
                {
                    calcOpts.ManaAmt = float.Parse(cmbManaAmt.Text);
                }
                catch { }
                Character.OnItemsChanged();
            }
        }

        private void trkActivity_Scroll(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsHolyPriest calcOpts = Character.CalculationOptions as CalculationOptionsHolyPriest;
                lblActivity.Text = trkActivity.Value + "%";
                calcOpts.Activity = trkActivity.Value;
                Character.OnItemsChanged();
            }
        }

        private void cmbSpriest_ValueChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsHolyPriest calcOpts = Character.CalculationOptions as CalculationOptionsHolyPriest;
                calcOpts.Spriest = (float)cmbSpriest.Value;
                Character.OnItemsChanged();
            }
        }

        private void trkRatio_Scroll(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsHolyPriest calcOpts = Character.CalculationOptions as CalculationOptionsHolyPriest;
                calcOpts.Ratio = trkRatio.Value / 100f;
                labHL1.Text = (100 - trkRatio.Value).ToString() + "%";
                labHL2.Text = trkRatio.Value.ToString() + "%";
                Character.OnItemsChanged();
            }
        }

    }

	[Serializable]
	public class CalculationOptionsHolyPriest : ICalculationOptionBase
	{
		public string GetXml()
		{
			System.Xml.Serialization.XmlSerializer serializer =
				new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptionsHolyPriest));
			StringBuilder xml = new StringBuilder();
			System.IO.StringWriter writer = new System.IO.StringWriter(xml);
			serializer.Serialize(writer, this);
			return xml.ToString();
		}

		public bool EnforceMetagemRequirements = false;
		public float Length = 5;
		public float ManaAmt = 2400;
		public float ManaTime = 2.5f;
		public float Activity = 80;
		public float Spriest = 0;
        public float Ratio = .25f;
	}
}
