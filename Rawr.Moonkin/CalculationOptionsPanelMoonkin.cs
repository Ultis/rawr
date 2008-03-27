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
            if (!Character.CalculationOptions.ContainsKey("InnervateDelay"))
                Character.CalculationOptions["InnervateDelay"] = "1";
            if (!Character.CalculationOptions.ContainsKey("ShadowPriest"))
                Character.CalculationOptions["ShadowPriest"] = "0";
            if (!Character.CalculationOptions.ContainsKey("ManaPots"))
                Character.CalculationOptions["ManaPots"] = "No";
            if (!Character.CalculationOptions.ContainsKey("ManaPotDelay"))
                Character.CalculationOptions["ManaPotDelay"] = "1.5";
            if (!Character.CalculationOptions.ContainsKey("ManaPotType"))
                Character.CalculationOptions["ManaPotType"] = "Super Mana Potion";
            if (!Character.CalculationOptions.ContainsKey("InnervateWeapon"))
                Character.CalculationOptions["InnervateWeapon"] = "No";
            if (!Character.CalculationOptions.ContainsKey("InnervateWeaponInt"))
                Character.CalculationOptions["InnervateWeaponInt"] = "0";
            if (!Character.CalculationOptions.ContainsKey("InnervateWeaponSpi"))
                Character.CalculationOptions["InnervateWeaponSpi"] = "0";

            cmbTargetLevel.SelectedItem = Character.CalculationOptions["TargetLevel"];
            chkMetagem.Checked = Character.CalculationOptions["EnforceMetagemRequirements"] == "Yes";
            txtLatency.Text = Character.CalculationOptions["Latency"];
            txtFightLength.Text = Character.CalculationOptions["FightLength"];
            txtShadowPriest.Text = Character.CalculationOptions["ShadowPriest"];
            chkInnervate.Checked = Character.CalculationOptions["Innervate"] == "Yes";
            chkManaPots.Checked = Character.CalculationOptions["ManaPots"] == "Yes";
            cmbPotType.SelectedItem = Character.CalculationOptions["ManaPotType"];
            cmbPotType.Enabled = chkManaPots.Checked;
            txtInnervateDelay.Text = Character.CalculationOptions["InnervateDelay"];
            txtInnervateDelay.Enabled = chkInnervate.Checked;
            txtManaPotDelay.Text = Character.CalculationOptions["ManaPotDelay"];
            txtManaPotDelay.Enabled = chkManaPots.Checked;
            chkInnervateWeapon.Checked = Character.CalculationOptions["InnervateWeapon"] == "Yes";
            txtInnervateWeaponInt.Enabled = chkInnervateWeapon.Checked;
            txtInnervateWeaponInt.Text = Character.CalculationOptions["InnervateWeaponInt"];
            txtInnervateWeaponSpi.Enabled = chkInnervateWeapon.Checked;
            txtInnervateWeaponSpi.Text = Character.CalculationOptions["InnervateWeaponSpi"];

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

        private void chkMetagem_Leave(object sender, EventArgs e)
        {
            Character.CalculationOptions["EnforceMetagemRequirements"] = chkMetagem.Checked ? "Yes" : "No";
            Character.OnItemsChanged();
        }

        private void txtFightLength_Leave(object sender, EventArgs e)
        {
            Character.CalculationOptions["FightLength"] = txtFightLength.Text;
            Character.OnItemsChanged();
        }

        private void chkInnervate_CheckedChanged(object sender, EventArgs e)
        {
            Character.CalculationOptions["Innervate"] = chkInnervate.Checked ? "Yes" : "No";
            txtInnervateDelay.Enabled = chkInnervate.Checked;
            Character.OnItemsChanged();
        }

        private void txtShadowPriest_Leave(object sender, EventArgs e)
        {
            Character.CalculationOptions["ShadowPriest"] = txtShadowPriest.Text;
            Character.OnItemsChanged();
        }

        private void chkManaPots_CheckedChanged(object sender, EventArgs e)
        {
            Character.CalculationOptions["ManaPots"] = chkManaPots.Checked ? "Yes" : "No";
            cmbPotType.Enabled = chkManaPots.Checked;
            txtManaPotDelay.Enabled = chkManaPots.Checked;
            Character.OnItemsChanged();
        }

        private void cmbPotType_SelectedIndexChanged(object sender, EventArgs e)
        {
            Character.CalculationOptions["ManaPotType"] = cmbPotType.SelectedItem.ToString();
            Character.OnItemsChanged();
        }

        private void txtInnervateDelay_Leave(object sender, EventArgs e)
        {
            Character.CalculationOptions["InnervateDelay"] = txtInnervateDelay.Text;
            Character.OnItemsChanged();
        }

        private void txtManaPotDelay_Leave(object sender, EventArgs e)
        {
            Character.CalculationOptions["ManaPotDelay"] = txtManaPotDelay.Text;
            Character.OnItemsChanged();
        }

        private void txtInnervateWeaponInt_Leave(object sender, EventArgs e)
        {
            Character.CalculationOptions["InnervateWeaponInt"] = txtInnervateWeaponInt.Text;
            Character.OnItemsChanged();
        }

        private void txtInnervateWeaponSpi_Leave(object sender, EventArgs e)
        {
            Character.CalculationOptions["InnervateWeaponSpi"] = txtInnervateWeaponSpi.Text;
            Character.OnItemsChanged();
        }

        private void chkInnervateWeapon_CheckedChanged(object sender, EventArgs e)
        {
            Character.CalculationOptions["InnervateWeapon"] = chkInnervateWeapon.Checked ? "Yes" : "No";
            txtInnervateWeaponInt.Enabled = chkInnervateWeapon.Checked;
            txtInnervateWeaponSpi.Enabled = chkInnervateWeapon.Checked;
            Character.OnItemsChanged();
        }
    }
}
