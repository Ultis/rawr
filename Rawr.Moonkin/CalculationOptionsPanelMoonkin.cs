using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Rawr.Moonkin
{
    public partial class CalculationOptionsPanelMoonkin : CalculationOptionsPanelBase
    {
        private MoonkinTalentsForm talents;
        public CalculationOptionsPanelMoonkin()
        {
            talents = new MoonkinTalentsForm(this);
            InitializeComponent();
        }

        // Display the talents form when the button is clicked
        private void btnTalents_Click(object sender, EventArgs e)
        {
            talents.Show();
        }

        // Load the options into the form
        protected override void LoadCalculationOptions()
        {
            if (!Character.CalculationOptions.ContainsKey("TargetLevel"))
                Character.CalculationOptions["TargetLevel"] = "73";
            if (!Character.CalculationOptions.ContainsKey("EnforceMetagemRequirements"))
                Character.CalculationOptions["EnforceMetagemRequirements"] = "No";
            if (!Character.CalculationOptions.ContainsKey("Latency"))
                Character.CalculationOptions["Latency"] = "0.4";
            if (!Character.CalculationOptions.ContainsKey("FightLength"))
                Character.CalculationOptions["FightLength"] = "5";
            if (!Character.CalculationOptions.ContainsKey("Innervate"))
                Character.CalculationOptions["Innervate"] = "No";

            cmbTargetLevel.SelectedItem = Character.CalculationOptions["TargetLevel"];
            chkMetagem.Checked = Character.CalculationOptions["EnforceMetagemRequirements"] == "Yes";
            txtLatency.Text = Character.CalculationOptions["Latency"];
            txtFightLength.Text = Character.CalculationOptions["FightLength"];
            chkInnervate.Checked = Character.CalculationOptions["Innervate"] == "Yes";

            if (talents != null) talents.LoadCalculationOptions();
        }

        private void cmbTargetLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            Character.CalculationOptions["TargetLevel"] = cmbTargetLevel.SelectedItem.ToString();
            Character.OnItemsChanged();
        }

        private void txtLatency_TextChanged(object sender, EventArgs e)
        {
            Character.CalculationOptions["Latency"] = txtLatency.Text;
            Character.OnItemsChanged();
        }

        private void chkMetagem_CheckedChanged(object sender, EventArgs e)
        {
            Character.CalculationOptions["EnforceMetagemRequirements"] = chkMetagem.Checked ? "Yes" : "No";
            Character.OnItemsChanged();
        }

        private void txtFightLength_TextChanged(object sender, EventArgs e)
        {
            Character.CalculationOptions["FightLength"] = txtFightLength.Text;
            Character.OnItemsChanged();
        }

        private void chkInnervate_CheckedChanged(object sender, EventArgs e)
        {
            Character.CalculationOptions["Innervate"] = chkInnervate.Checked ? "Yes" : "No";
            Character.OnItemsChanged();
        }
    }
}
