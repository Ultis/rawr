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
            cmbManaTime.Value = (decimal)calcOpts.ManaTime;
            cmbSpriest.Value = (decimal)calcOpts.Spriest;
            cmbSpiritual.Value = (decimal)calcOpts.Spiritual;
            chkBoL.Checked = calcOpts.BoL;

			trkActivity.Value = (int)calcOpts.Activity;
            lblActivity.Text = trkActivity.Value + "%";
            loading = false;
        }
 
        private void cmbLength_ValueChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsHealadin calcOpts = Character.CalculationOptions as CalculationOptionsHealadin;
                calcOpts.Length = (float)cmbLength.Value;
                Character.OnItemsChanged();
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
                Character.OnItemsChanged();
            }
        }

        private void cmbManaTime_ValueChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsHealadin calcOpts = Character.CalculationOptions as CalculationOptionsHealadin;
                calcOpts.ManaTime = (float)cmbManaTime.Value;
                Character.OnItemsChanged();
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
                Character.OnItemsChanged();
            }
        }

        private void trkActivity_Scroll(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsHealadin calcOpts = Character.CalculationOptions as CalculationOptionsHealadin;
                lblActivity.Text = trkActivity.Value + "%";
                calcOpts.Activity = trkActivity.Value;
                Character.OnItemsChanged();
            }
        }

        private void cmbSpriest_ValueChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsHealadin calcOpts = Character.CalculationOptions as CalculationOptionsHealadin;
                calcOpts.Spriest = (float)cmbSpriest.Value;
                Character.OnItemsChanged();
            }
        }

        private void cmbSpiritual_ValueChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsHealadin calcOpts = Character.CalculationOptions as CalculationOptionsHealadin;
                calcOpts.Spiritual = (float)cmbSpiritual.Value;
                Character.OnItemsChanged();
            }
        }

        private void chkBoL_CheckedChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsHealadin calcOpts = Character.CalculationOptions as CalculationOptionsHealadin;
                calcOpts.BoL = chkBoL.Checked;
                Character.OnItemsChanged();
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

		public bool EnforceMetagemRequirements = false;
		public float Length = 5;
		public float ManaAmt = 2400;
		public float ManaTime = 2.5f;
		public float Activity = 80;
		public float Spriest = 0;
		public float Spiritual = 0;
        public bool BoL;
	}
}
