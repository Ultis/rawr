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

            nudDivinePlea.Value = (decimal)calcOpts.DivinePlea;
            nudGHL.Value = (decimal)calcOpts.GHL_Targets;

			trkActivity.Value = (int)(calcOpts.Activity * 100);
            lblActivity.Text = trkActivity.Value + "%";

            chkJotP.Checked = calcOpts.JotP;
            chkJudgement.Checked = calcOpts.Judgement;
            chkLoHSelf.Checked = calcOpts.LoHSelf;

            trkReplenishment.Value = (int)Math.Round(calcOpts.Replenishment * 100);
            lblReplenishment.Text = trkReplenishment.Value + "%";
            
            trkBoLUp.Value = (int)Math.Round(calcOpts.BoLUp * 100);
            lblBoLUp.Text = trkBoLUp.Value + "%";

            trkBoLEff.Value = (int)Math.Round(calcOpts.BoLEff * 100);
            lblBoLEff.Text = trkBoLEff.Value + "%";

            trkBurstScale.Value = (int)Math.Round(calcOpts.BurstScale * 100);
            lblBurstScale.Text = trkBurstScale.Value + "%";

            trkHS.Value = (int)Math.Round(calcOpts.HolyShock * 100);
            lblHS.Text = trkHS.Value + "%";

            trkSacredShield.Value = (int)Math.Round(calcOpts.SSUptime * 100);
            lblSacredShield.Text = trkSacredShield.Value + "%";

            chkIoL.Checked = calcOpts.InfusionOfLight;
            trkIoLRatio.Value = (int)Math.Round(calcOpts.IoLHolyLight * 100f);
            lblIoLHL.Text = trkIoLRatio.Value + "% HL";
            lblIoLFoL.Text = (100 - trkIoLRatio.Value) + "% FoL";
            trkIoLRatio.Enabled = calcOpts.InfusionOfLight;
            lblIoLHL.Enabled = calcOpts.InfusionOfLight;
            lblIoLFoL.Enabled = calcOpts.InfusionOfLight;

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

        private void trkHS_Scroll(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsHealadin calcOpts = Character.CalculationOptions as CalculationOptionsHealadin;
                calcOpts.HolyShock = trkHS.Value / 100f;
                lblHS.Text = trkHS.Value + "%";
                Character.OnCalculationsInvalidated();
            }
        }


        private void chkLoHSelf_CheckedChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsHealadin calcOpts = Character.CalculationOptions as CalculationOptionsHealadin;
                calcOpts.LoHSelf = chkLoHSelf.Checked;
                Character.OnCalculationsInvalidated();
            }
        }

        private void nudGHL_ValueChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsHealadin calcOpts = Character.CalculationOptions as CalculationOptionsHealadin;
                calcOpts.GHL_Targets = (float)nudGHL.Value;
                Character.OnCalculationsInvalidated();
            }
        }


        private void trkBurstScale_Scroll(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsHealadin calcOpts = Character.CalculationOptions as CalculationOptionsHealadin;
                calcOpts.BurstScale = trkBurstScale.Value / 100f;
                lblBurstScale.Text = trkBurstScale.Value + "%";
                Character.OnCalculationsInvalidated();
            }
        }

        private void chkIoL_CheckedChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsHealadin calcOpts = Character.CalculationOptions as CalculationOptionsHealadin;
                calcOpts.InfusionOfLight = chkIoL.Checked;
                trkIoLRatio.Enabled = calcOpts.InfusionOfLight;
                lblIoLHL.Enabled = calcOpts.InfusionOfLight;
                lblIoLFoL.Enabled = calcOpts.InfusionOfLight;
                Character.OnCalculationsInvalidated();
            }
        }

        private void trkIoLRatio_Scroll(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsHealadin calcOpts = Character.CalculationOptions as CalculationOptionsHealadin;
                calcOpts.IoLHolyLight = trkIoLRatio.Value / 100f;
                lblIoLHL.Text = trkIoLRatio.Value + "% HL";
                lblIoLFoL.Text = (100 - trkIoLRatio.Value) + "% FoL";
                Character.OnCalculationsInvalidated();
            }
        }

        private void trkSacredShield_Scroll(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsHealadin calcOpts = Character.CalculationOptions as CalculationOptionsHealadin;
                calcOpts.SSUptime = trkSacredShield.Value / 100f;
                lblSacredShield.Text = trkSacredShield.Value + "%";
                Character.OnCalculationsInvalidated();
            }
        }

        private void chkJudgement_CheckedChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsHealadin calcOpts = Character.CalculationOptions as CalculationOptionsHealadin;
                calcOpts.Judgement = chkJudgement.Checked;
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

		public float Length = 7;
		public float ManaAmt = 4300;
		public float Activity = .85f;
        public float Replenishment = .9f;
        public float DivinePlea = 2f;
        public float BoLUp = 1f;
        public float BoLEff = .2f;
        public float HolyShock = .15f;
        public float BurstScale = .4f;
        public float GHL_Targets = 1f;

        public bool InfusionOfLight = false;
        public float IoLHolyLight = .9f;

        public bool JotP = true;
        public bool Judgement = true;
        public bool LoHSelf = false;
        public float SSUptime = 1f;

	}
}
