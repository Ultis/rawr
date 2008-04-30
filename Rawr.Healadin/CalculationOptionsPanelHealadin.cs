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
            if (!Character.CalculationOptions.ContainsKey("EnforceMetagemRequirements"))
                Character.CalculationOptions["EnforceMetagemRequirements"] = "No";

            if (!Character.CalculationOptions.ContainsKey("Length"))
                Character.CalculationOptions["Length"] = "5";
            if (!Character.CalculationOptions.ContainsKey("ManaAmt"))
                Character.CalculationOptions["ManaAmt"] = "2400";
            if (!Character.CalculationOptions.ContainsKey("ManaTime"))
                Character.CalculationOptions["ManaTime"] = "2.5";
            if (!Character.CalculationOptions.ContainsKey("Activity"))
                Character.CalculationOptions["Activity"] = "80";
            if (!Character.CalculationOptions.ContainsKey("Spriest"))
                Character.CalculationOptions["Spriest"] = "0";
            if (!Character.CalculationOptions.ContainsKey("Spiritual"))
                Character.CalculationOptions["Spiritual"] = "0";

            cmbLength.Value = decimal.Parse(Character.CalculationOptions["Length"]);
            cmbManaAmt.Text = Character.CalculationOptions["ManaAmt"];
            cmbManaTime.Value = decimal.Parse(Character.CalculationOptions["ManaTime"]);
            cmbSpriest.Value = decimal.Parse(Character.CalculationOptions["Spriest"]);
            cmbSpiritual.Value = decimal.Parse(Character.CalculationOptions["Spiritual"]);

            trkActivity.Value = int.Parse(Character.CalculationOptions["Activity"]);
            lblActivity.Text = "Activity (" + trkActivity.Value + "%):";
        }
 
        private void cmbLength_ValueChanged(object sender, EventArgs e)
        {
            Character.CalculationOptions["Length"] = cmbLength.Value.ToString();
            Character.OnItemsChanged();
        }

        private void cmbManaAmt_SelectedIndexChanged(object sender, EventArgs e)
        {
            Character.CalculationOptions["ManaAmt"] = cmbManaAmt.Text;
            Character.OnItemsChanged();
        }

        private void cmbManaTime_ValueChanged(object sender, EventArgs e)
        {
            Character.CalculationOptions["ManaTime"] = cmbManaTime.Value.ToString();
            Character.OnItemsChanged();
        }

        private void cmbManaAmt_TextUpdate(object sender, EventArgs e)
        {
            Character.CalculationOptions["ManaAmt"] = cmbManaAmt.Text;
            Character.OnItemsChanged();
        }

        private void trkActivity_Scroll(object sender, EventArgs e)
        {
            lblActivity.Text = "Activity (" + trkActivity.Value + "%):";
            Character.CalculationOptions["Activity"] = trkActivity.Value.ToString();
            Character.OnItemsChanged();
        }

        private void cmbSpriest_ValueChanged(object sender, EventArgs e)
        {
            Character.CalculationOptions["Spriest"] = cmbSpriest.Value.ToString();
            Character.OnItemsChanged();
        }

        private void cmbSpiritual_ValueChanged(object sender, EventArgs e)
        {
            Character.CalculationOptions["Spiritual"] = cmbSpiritual.Value.ToString();
            Character.OnItemsChanged();
        }

    }
}
