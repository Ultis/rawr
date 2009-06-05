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

            if (calcOpts.Rotation > 0)
            {   // Fix for Legacy
                calcOpts.Role = (CalculationOptionsPriest.eRole)calcOpts.Rotation;
                calcOpts.Rotation = 0;
            }
            cbRotation.SelectedIndex = (int)calcOpts.Role;
            panelCustom.Visible = calcOpts.Role == CalculationOptionsPriest.eRole.CUSTOM;

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
                CalculationOptionsPriest calcOpts = Character.CalculationOptions as CalculationOptionsPriest;
                calcOpts.ManaPot = cmbManaAmt.SelectedIndex;
                Character.OnCalculationsInvalidated();
            }
        }

        private void trkActivity_Scroll(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsPriest calcOpts = Character.CalculationOptions as CalculationOptionsPriest;
                lblActivity.Text = trkActivity.Value + "% of fight spent in FSR.";
                calcOpts.FSRRatio = trkActivity.Value;
                Character.OnCalculationsInvalidated();
            }
        }

        private void cbRotation_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsPriest calcOpts = Character.CalculationOptions as CalculationOptionsPriest;
                calcOpts.Role = (CalculationOptionsPriest.eRole)cbRotation.SelectedIndex;
                Character.OnCalculationsInvalidated();
                panelCustom.Visible = calcOpts.Role == CalculationOptionsPriest.eRole.CUSTOM;
            }
        }

        private void numFightLength_ValueChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsPriest calcOpts = Character.CalculationOptions as CalculationOptionsPriest;
                calcOpts.FightLengthSeconds = (int)numFightLength.Value;
                Character.OnCalculationsInvalidated();
            }
        }

        private void trkSerendipity_Scroll(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsPriest calcOpts = Character.CalculationOptions as CalculationOptionsPriest;
                lblSerendipity.Text = trkSerendipity.Value + "% T5 2 Set Bonus.";
                calcOpts.Serendipity = trkSerendipity.Value;
                Character.OnCalculationsInvalidated();
            }
        }

        private void trkReplenishment_Scroll(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsPriest calcOpts = Character.CalculationOptions as CalculationOptionsPriest;
                lblReplenishment.Text = trkReplenishment.Value + "% time with Replenishment buff.";
                calcOpts.Replenishment = trkReplenishment.Value;
                Character.OnCalculationsInvalidated();
            }
        }

        private void trkShadowfiend_Scroll(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsPriest calcOpts = Character.CalculationOptions as CalculationOptionsPriest;
                lblShadowfiend.Text = trkShadowfiend.Value + "% effectiveness of Shadowfiend.";
                calcOpts.Shadowfiend = trkShadowfiend.Value;
                Character.OnCalculationsInvalidated();
            }

        }

        private void trkSurvivability_Scroll(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsPriest calcOpts = Character.CalculationOptions as CalculationOptionsPriest;
                lblSurvivability.Text = trkSurvivability.Value + "% weight on Survivability.";
                calcOpts.Survivability = trkSurvivability.Value;
                Character.OnCalculationsInvalidated();
            }
        }

        private void trkRapture_Scroll(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsPriest calcOpts = Character.CalculationOptions as CalculationOptionsPriest;
                lblRapture.Text = trkRapture.Value + "% of max Rapture returns.";
                calcOpts.Rapture = trkRapture.Value;
                Character.OnCalculationsInvalidated();
            }
        }

        private void cbUseTrinkets_CheckedChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsPriest calcOpts = Character.CalculationOptions as CalculationOptionsPriest;
                calcOpts.ModelProcs = cbModelProcs.Checked;
                Character.OnCalculationsInvalidated();
            }
        }

        private void trkTestOfFaith_Scroll(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsPriest calcOpts = Character.CalculationOptions as CalculationOptionsPriest;
                lblTestOfFaith.Text = trkTestOfFaith.Value + "% of heals use Test of Faith or Improved Flash Heal.";
                calcOpts.TestOfFaith = trkTestOfFaith.Value;
                Character.OnCalculationsInvalidated();
            }
        }

        private void numFlashHealCast_ValueChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsPriest calcOpts = Character.CalculationOptions as CalculationOptionsPriest;
                calcOpts.FlashHealCast = (int)numFlashHealCast.Value;
                Character.OnCalculationsInvalidated();
            }
        }

        private void numGreaterHealCast_ValueChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsPriest calcOpts = Character.CalculationOptions as CalculationOptionsPriest;
                calcOpts.GreaterHealCast = (int)numGreaterHealCast.Value;
                Character.OnCalculationsInvalidated();
            }
        }

        private void numPenanceCast_ValueChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsPriest calcOpts = Character.CalculationOptions as CalculationOptionsPriest;
                calcOpts.PenanceCast = (int)numPenanceCast.Value;
                Character.OnCalculationsInvalidated();
            }
        }

        private void numRenewCast_ValueChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsPriest calcOpts = Character.CalculationOptions as CalculationOptionsPriest;
                calcOpts.RenewCast = (int)numRenewCast.Value;
                Character.OnCalculationsInvalidated();
            }
        }

        private void numRenewTicks_ValueChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsPriest calcOpts = Character.CalculationOptions as CalculationOptionsPriest;
                calcOpts.RenewTicks = (int)numRenewTicks.Value;
                Character.OnCalculationsInvalidated();
            }
        }

        private void numProMCast_ValueChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsPriest calcOpts = Character.CalculationOptions as CalculationOptionsPriest;
                calcOpts.ProMCast = (int)numProMCast.Value;
                Character.OnCalculationsInvalidated();
            }
        }

        private void numProMTicks_ValueChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsPriest calcOpts = Character.CalculationOptions as CalculationOptionsPriest;
                calcOpts.ProMTicks = (int)numProMTicks.Value;
                Character.OnCalculationsInvalidated();
            }
        }

        private void numPoHCast_ValueChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsPriest calcOpts = Character.CalculationOptions as CalculationOptionsPriest;
                calcOpts.PoHCast = (int)numPoHCast.Value;
                Character.OnCalculationsInvalidated();
            }
        }

        private void numPWSCast_ValueChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsPriest calcOpts = Character.CalculationOptions as CalculationOptionsPriest;
                calcOpts.PWSCast = (int)numPWSCast.Value;
                Character.OnCalculationsInvalidated();
            }
        }

        private void numCoHCast_ValueChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsPriest calcOpts = Character.CalculationOptions as CalculationOptionsPriest;
                calcOpts.CoHCast = (int)numCoHCast.Value;
                Character.OnCalculationsInvalidated();
            }
        }

        private void numHolyNovaCast_ValueChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsPriest calcOpts = Character.CalculationOptions as CalculationOptionsPriest;
                calcOpts.HolyNovaCast = (int)numHolyNovaCast.Value;
                Character.OnCalculationsInvalidated();
            }
        }

        private void numDispelCast_ValueChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsPriest calcOpts = Character.CalculationOptions as CalculationOptionsPriest;
                calcOpts.DispelCast = (int)numDispelCast.Value;
                Character.OnCalculationsInvalidated();
            }
        }

        private void numMDCast_ValueChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsPriest calcOpts = Character.CalculationOptions as CalculationOptionsPriest;
                calcOpts.MDCast = (int)numMDCast.Value;
                Character.OnCalculationsInvalidated();
            }
        }

        private void numBindingHeal_ValueChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsPriest calcOpts = Character.CalculationOptions as CalculationOptionsPriest;
                calcOpts.BindingHealCast = (int)numBindingHealCast.Value;
                Character.OnCalculationsInvalidated();
            }
        }

        private void numDivineHymnCast_ValueChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsPriest calcOpts = Character.CalculationOptions as CalculationOptionsPriest;
                calcOpts.DivineHymnCast = (int)numDivineHymnCast.Value;
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

        private static readonly List<int> manaAmt = new List<int>() { 0, 1800, 2200, 2400, 4300 };
        public int ManaPot = 4;
        public int ManaAmt { get { return manaAmt[ManaPot]; } }
        public enum eRole
        {
            AUTO_Tank, AUTO_Raid, Greater_Heal, Flash_Heal, CoH_PoH, Holy_Tank, Holy_Raid,
            Disc_Tank_GH, Disc_Tank_FH, Disc_Raid, CUSTOM
        };
        public eRole Role = 0;
        public int Rotation = 0;    // LEGACY
        public float FSRRatio = 93f;
        public float FightLengthSeconds = 480f;
        public float Serendipity = 75f;
        public float Replenishment = 75f;
        public float Shadowfiend = 100f;
        public float Survivability = 2f;
        public float Rapture = 25f;
        public float TestOfFaith = 25f;
        public bool ModelProcs = true;

        public int FlashHealCast = 0;
        public int BindingHealCast = 0;
        public int GreaterHealCast = 0;
        public int PenanceCast = 0;
        public int RenewCast = 0;
        public int RenewTicks = 0;
        public int ProMCast = 0;
        public int ProMTicks = 0;
        public int PoHCast = 0;
        public int PWSCast = 0;
        public int CoHCast = 0;
        public int HolyNovaCast = 0;
        public int DivineHymnCast = 0;
        public int DispelCast = 0;
        public int MDCast = 0;
	}
}
