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
                Character.CalculationOptions = new CalculationOptionsHolyPriest();

            CalculationOptionsHolyPriest calcOpts = Character.CalculationOptions as CalculationOptionsHolyPriest;

            if (calcOpts.Rotation > 0)
            {   // Fix for Legacy
                calcOpts.Role = (CalculationOptionsHolyPriest.eRole)calcOpts.Rotation;
                calcOpts.Rotation = 0;
            }
            cbRotation.SelectedIndex = (int)calcOpts.Role;
            panelCustom.Visible = calcOpts.Role == CalculationOptionsHolyPriest.eRole.CUSTOM;

            cmbManaAmt.SelectedIndex = calcOpts.ManaPot;

            trkActivity.Value = (int)calcOpts.FSRRatio;
            lblActivity.Text = trkActivity.Value + "% of fight spent in FSR.";

            numFightLength.Value = (int)calcOpts.FightLengthSeconds;

            trkSerendipity.Value = (int)calcOpts.Serendipity;
            lblSerendipity.Text = trkSerendipity.Value + "% T5 2 Set Bonus.";

            trkRapture.Value = (int)calcOpts.Rapture;
            lblRapture.Text = trkRapture.Value + "% of max Rapture returns.";

            trkReplenishment.Value = (int)calcOpts.Replenishment;
            lblReplenishment.Text = trkReplenishment.Value + "% time with Replenishment buff.";

            trkShadowfiend.Value = (int)calcOpts.Shadowfiend;
            lblShadowfiend.Text = trkShadowfiend.Value + "% effectiveness of Shadowfiend.";

            trkTestOfFaith.Value = (int)calcOpts.TestOfFaith;
            lblTestOfFaith.Text = trkTestOfFaith.Value + "% of heals use Test of Faith or Improved Flash Heal.";

            trkSurvivability.Value = (int)calcOpts.Survivability;
            lblSurvivability.Text = trkSurvivability.Value + "% weight on Survivability.";

            cbModelProcs.Checked = calcOpts.ModelProcs;

            numFlashHealCast.Value = calcOpts.FlashHealCast;
            numBindingHealCast.Value = calcOpts.BindingHealCast;
            numGreaterHealCast.Value = calcOpts.GreaterHealCast;
            numPenanceCast.Value = calcOpts.PenanceCast;
            numRenewCast.Value = calcOpts.RenewCast;
            numRenewTicks.Value = calcOpts.RenewTicks;
            numProMCast.Value = calcOpts.ProMCast;
            numProMTicks.Value = calcOpts.ProMTicks;
            numPoHCast.Value = calcOpts.PoHCast;
            numPWSCast.Value = calcOpts.PWSCast;
            numCoHCast.Value = calcOpts.CoHCast;
            numHolyNovaCast.Value = calcOpts.HolyNovaCast;
            numDivineHymnCast.Value = calcOpts.DivineHymnCast;
            numDispelCast.Value = calcOpts.DispelCast;
            numMDCast.Value = calcOpts.MDCast;

            loading = false;
        }


        private void cmbManaAmt_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsHolyPriest calcOpts = Character.CalculationOptions as CalculationOptionsHolyPriest;
                calcOpts.ManaPot = cmbManaAmt.SelectedIndex;
                Character.OnCalculationsInvalidated();
            }
        }

        private void trkActivity_Scroll(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsHolyPriest calcOpts = Character.CalculationOptions as CalculationOptionsHolyPriest;
                lblActivity.Text = trkActivity.Value + "% of fight spent in FSR.";
                calcOpts.FSRRatio = trkActivity.Value;
                Character.OnCalculationsInvalidated();
            }
        }

        private void cbRotation_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsHolyPriest calcOpts = Character.CalculationOptions as CalculationOptionsHolyPriest;
                calcOpts.Role = (CalculationOptionsHolyPriest.eRole)cbRotation.SelectedIndex;
                Character.OnCalculationsInvalidated();
                panelCustom.Visible = calcOpts.Role == CalculationOptionsHolyPriest.eRole.CUSTOM;
            }
        }

        private void numFightLength_ValueChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsHolyPriest calcOpts = Character.CalculationOptions as CalculationOptionsHolyPriest;
                calcOpts.FightLengthSeconds = (int)numFightLength.Value;
                Character.OnCalculationsInvalidated();
            }
        }

        private void trkSerendipity_Scroll(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsHolyPriest calcOpts = Character.CalculationOptions as CalculationOptionsHolyPriest;
                lblSerendipity.Text = trkSerendipity.Value + "% T5 2 Set Bonus.";
                calcOpts.Serendipity = trkSerendipity.Value;
                Character.OnCalculationsInvalidated();
            }
        }

        private void trkReplenishment_Scroll(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsHolyPriest calcOpts = Character.CalculationOptions as CalculationOptionsHolyPriest;
                lblReplenishment.Text = trkReplenishment.Value + "% time with Replenishment buff.";
                calcOpts.Replenishment = trkReplenishment.Value;
                Character.OnCalculationsInvalidated();
            }
        }

        private void trkShadowfiend_Scroll(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsHolyPriest calcOpts = Character.CalculationOptions as CalculationOptionsHolyPriest;
                lblShadowfiend.Text = trkShadowfiend.Value + "% effectiveness of Shadowfiend.";
                calcOpts.Shadowfiend = trkShadowfiend.Value;
                Character.OnCalculationsInvalidated();
            }

        }

        private void trkSurvivability_Scroll(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsHolyPriest calcOpts = Character.CalculationOptions as CalculationOptionsHolyPriest;
                lblSurvivability.Text = trkSurvivability.Value + "% weight on Survivability.";
                calcOpts.Survivability = trkSurvivability.Value;
                Character.OnCalculationsInvalidated();
            }
        }

        private void trkRapture_Scroll(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsHolyPriest calcOpts = Character.CalculationOptions as CalculationOptionsHolyPriest;
                lblRapture.Text = trkRapture.Value + "% of max Rapture returns.";
                calcOpts.Rapture = trkRapture.Value;
                Character.OnCalculationsInvalidated();
            }
        }

        private void cbUseTrinkets_CheckedChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsHolyPriest calcOpts = Character.CalculationOptions as CalculationOptionsHolyPriest;
                calcOpts.ModelProcs = cbModelProcs.Checked;
                Character.OnCalculationsInvalidated();
            }
        }

        private void trkTestOfFaith_Scroll(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsHolyPriest calcOpts = Character.CalculationOptions as CalculationOptionsHolyPriest;
                lblTestOfFaith.Text = trkTestOfFaith.Value + "% of heals use Test of Faith or Improved Flash Heal.";
                calcOpts.TestOfFaith = trkTestOfFaith.Value;
                Character.OnCalculationsInvalidated();
            }
        }

        private void numFlashHealCast_ValueChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsHolyPriest calcOpts = Character.CalculationOptions as CalculationOptionsHolyPriest;
                calcOpts.FlashHealCast = (int)numFlashHealCast.Value;
                Character.OnCalculationsInvalidated();
            }
        }

        private void numGreaterHealCast_ValueChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsHolyPriest calcOpts = Character.CalculationOptions as CalculationOptionsHolyPriest;
                calcOpts.GreaterHealCast = (int)numGreaterHealCast.Value;
                Character.OnCalculationsInvalidated();
            }
        }

        private void numPenanceCast_ValueChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsHolyPriest calcOpts = Character.CalculationOptions as CalculationOptionsHolyPriest;
                calcOpts.PenanceCast = (int)numPenanceCast.Value;
                Character.OnCalculationsInvalidated();
            }
        }

        private void numRenewCast_ValueChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsHolyPriest calcOpts = Character.CalculationOptions as CalculationOptionsHolyPriest;
                calcOpts.RenewCast = (int)numRenewCast.Value;
                Character.OnCalculationsInvalidated();
            }
        }

        private void numRenewTicks_ValueChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsHolyPriest calcOpts = Character.CalculationOptions as CalculationOptionsHolyPriest;
                calcOpts.RenewTicks = (int)numRenewTicks.Value;
                Character.OnCalculationsInvalidated();
            }
        }

        private void numProMCast_ValueChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsHolyPriest calcOpts = Character.CalculationOptions as CalculationOptionsHolyPriest;
                calcOpts.ProMCast = (int)numProMCast.Value;
                Character.OnCalculationsInvalidated();
            }
        }

        private void numProMTicks_ValueChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsHolyPriest calcOpts = Character.CalculationOptions as CalculationOptionsHolyPriest;
                calcOpts.ProMTicks = (int)numProMTicks.Value;
                Character.OnCalculationsInvalidated();
            }
        }

        private void numPoHCast_ValueChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsHolyPriest calcOpts = Character.CalculationOptions as CalculationOptionsHolyPriest;
                calcOpts.PoHCast = (int)numPoHCast.Value;
                Character.OnCalculationsInvalidated();
            }
        }

        private void numPWSCast_ValueChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsHolyPriest calcOpts = Character.CalculationOptions as CalculationOptionsHolyPriest;
                calcOpts.PWSCast = (int)numPWSCast.Value;
                Character.OnCalculationsInvalidated();
            }
        }

        private void numCoHCast_ValueChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsHolyPriest calcOpts = Character.CalculationOptions as CalculationOptionsHolyPriest;
                calcOpts.CoHCast = (int)numCoHCast.Value;
                Character.OnCalculationsInvalidated();
            }
        }

        private void numHolyNovaCast_ValueChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsHolyPriest calcOpts = Character.CalculationOptions as CalculationOptionsHolyPriest;
                calcOpts.HolyNovaCast = (int)numHolyNovaCast.Value;
                Character.OnCalculationsInvalidated();
            }
        }

        private void numDispelCast_ValueChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsHolyPriest calcOpts = Character.CalculationOptions as CalculationOptionsHolyPriest;
                calcOpts.DispelCast = (int)numDispelCast.Value;
                Character.OnCalculationsInvalidated();
            }
        }

        private void numMDCast_ValueChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsHolyPriest calcOpts = Character.CalculationOptions as CalculationOptionsHolyPriest;
                calcOpts.MDCast = (int)numMDCast.Value;
                Character.OnCalculationsInvalidated();
            }
        }

        private void numBindingHeal_ValueChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsHolyPriest calcOpts = Character.CalculationOptions as CalculationOptionsHolyPriest;
                calcOpts.BindingHealCast = (int)numBindingHealCast.Value;
                Character.OnCalculationsInvalidated();
            }
        }

        private void numDivineHymnCast_ValueChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsHolyPriest calcOpts = Character.CalculationOptions as CalculationOptionsHolyPriest;
                calcOpts.DivineHymnCast = (int)numDivineHymnCast.Value;
                Character.OnCalculationsInvalidated();
            }
        }
    }
}
