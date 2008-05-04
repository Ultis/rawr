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
        
        protected override void LoadCalculationOptions()
        {
			//if (!Character.CalculationOptions.ContainsKey("EnforceMetagemRequirements"))
			//    Character.CalculationOptions["EnforceMetagemRequirements"] = "No";

			//if (!Character.CalculationOptions.ContainsKey("Length"))
			//    Character.CalculationOptions["Length"] = "5";
			//if (!Character.CalculationOptions.ContainsKey("ManaAmt"))
			//    Character.CalculationOptions["ManaAmt"] = "2400";
			//if (!Character.CalculationOptions.ContainsKey("ManaTime"))
			//    Character.CalculationOptions["ManaTime"] = "2.5";
			//if (!Character.CalculationOptions.ContainsKey("Activity"))
			//    Character.CalculationOptions["Activity"] = "80";
			//if (!Character.CalculationOptions.ContainsKey("Spriest"))
			//    Character.CalculationOptions["Spriest"] = "0";
			//if (!Character.CalculationOptions.ContainsKey("Spiritual"))
			//    Character.CalculationOptions["Spiritual"] = "0";

			CalculationOptionsHealadin calcOpts = Character.CurrentCalculationOptions as CalculationOptionsHealadin;
            cmbLength.Value = (decimal)calcOpts.Length;
            cmbManaAmt.Text = calcOpts.ManaAmt.ToString();
            cmbManaTime.Value = (decimal)calcOpts.ManaTime;
            cmbSpriest.Value = (decimal)calcOpts.Spriest;
            cmbSpiritual.Value = (decimal)calcOpts.Spiritual;

			trkActivity.Value = (int)calcOpts.Activity;
            lblActivity.Text = "Activity (" + trkActivity.Value + "%):";
        }
 
        private void cmbLength_ValueChanged(object sender, EventArgs e)
        {
			CalculationOptionsHealadin calcOpts = Character.CurrentCalculationOptions as CalculationOptionsHealadin;
			calcOpts.Length = (float)cmbLength.Value;
            Character.OnItemsChanged();
        }

        private void cmbManaAmt_SelectedIndexChanged(object sender, EventArgs e)
        {
			CalculationOptionsHealadin calcOpts = Character.CurrentCalculationOptions as CalculationOptionsHealadin;
			try
			{
				calcOpts.ManaAmt = float.Parse(cmbManaAmt.Text);
			}
			catch { }
            Character.OnItemsChanged();
        }

        private void cmbManaTime_ValueChanged(object sender, EventArgs e)
        {
			CalculationOptionsHealadin calcOpts = Character.CurrentCalculationOptions as CalculationOptionsHealadin;
			calcOpts.ManaTime = (float)cmbManaTime.Value;
            Character.OnItemsChanged();
        }

        private void cmbManaAmt_TextUpdate(object sender, EventArgs e)
        {
			CalculationOptionsHealadin calcOpts = Character.CurrentCalculationOptions as CalculationOptionsHealadin;
			try
			{
				calcOpts.ManaAmt = float.Parse(cmbManaAmt.Text);
			}
			catch { }
            Character.OnItemsChanged();
        }

        private void trkActivity_Scroll(object sender, EventArgs e)
        {
			CalculationOptionsHealadin calcOpts = Character.CurrentCalculationOptions as CalculationOptionsHealadin;
			lblActivity.Text = "Activity (" + trkActivity.Value + "%):";
            calcOpts.Activity = trkActivity.Value;
            Character.OnItemsChanged();
        }

        private void cmbSpriest_ValueChanged(object sender, EventArgs e)
        {
			CalculationOptionsHealadin calcOpts = Character.CurrentCalculationOptions as CalculationOptionsHealadin;
			calcOpts.Spriest = (float)cmbSpriest.Value;
            Character.OnItemsChanged();
        }

        private void cmbSpiritual_ValueChanged(object sender, EventArgs e)
        {
			CalculationOptionsHealadin calcOpts = Character.CurrentCalculationOptions as CalculationOptionsHealadin;
			calcOpts.Spiritual = (float)cmbSpiritual.Value;
            Character.OnItemsChanged();
        }

    }

	[Serializable]
	public class CalculationOptionsHealadin
	{
		public bool EnforceMetagemRequirements = false;
		public float Length = 5;
		public float ManaAmt = 2400;
		public float ManaTime = 2.5f;
		public float Activity = 80;
		public float Spriest = 0;
		public float Spiritual = 0;
	}
}
