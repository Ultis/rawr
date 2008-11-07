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
            chkLoHSelf.Checked = calcOpts.LoHSelf;

            chkGHL.Checked = calcOpts.Glyph_HL;
            chkGFoL.Checked = calcOpts.Glyph_FoL;
            chkGSoL.Checked = calcOpts.Glyph_SoL;
            chkGSoW.Checked = calcOpts.Glyph_SoW;
            chkGLoH.Checked = calcOpts.Glyph_LoH;
            chkGDivinity.Checked = calcOpts.Glyph_Divinity;

            trkReplenishment.Value = (int)(calcOpts.Replenishment * 100);
            lblReplenishment.Text = trkReplenishment.Value + "%";

            trkBoLUp.Value = (int)(calcOpts.BoLUp * 100);
            lblBoLUp.Text = trkBoLUp.Value + "%";

            trkBoLEff.Value = (int)(calcOpts.BoLEff * 100);
            lblBoLEff.Text = trkBoLEff.Value + "%";

            trkHS.Value = (int)(calcOpts.HolyShock * 100);
            lblHS.Text = trkHS.Value + "%";

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

        private void chkGSoW_CheckedChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsHealadin calcOpts = Character.CalculationOptions as CalculationOptionsHealadin;
                calcOpts.Glyph_SoW = chkGSoW.Checked;
                Character.OnCalculationsInvalidated();
            }
        }

        private void chkGLoH_CheckedChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsHealadin calcOpts = Character.CalculationOptions as CalculationOptionsHealadin;
                calcOpts.Glyph_LoH = chkGLoH.Checked;
                Character.OnCalculationsInvalidated();
            }
        }

        private void chkGHL_CheckedChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsHealadin calcOpts = Character.CalculationOptions as CalculationOptionsHealadin;
                calcOpts.Glyph_HL = chkGHL.Checked;
                Character.OnCalculationsInvalidated();
            }
        }

        private void chkGDivinity_CheckedChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsHealadin calcOpts = Character.CalculationOptions as CalculationOptionsHealadin;
                calcOpts.Glyph_Divinity = chkGDivinity.Checked;
                Character.OnCalculationsInvalidated();
            }
        }

        private void chkGFoL_CheckedChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsHealadin calcOpts = Character.CalculationOptions as CalculationOptionsHealadin;
                calcOpts.Glyph_FoL = chkGFoL.Checked;
                Character.OnCalculationsInvalidated();
            }
        }

        private void chkGSoL_CheckedChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsHealadin calcOpts = Character.CalculationOptions as CalculationOptionsHealadin;
                calcOpts.Glyph_SoL = chkGSoL.Checked;
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
		public float ManaAmt = 4300;
		public float Activity = .85f;
		public float Spiritual = 3600;
        public float Replenishment = .9f;
        public float DivinePlea = 1.5f;
        public float BoLUp = 1f;
        public float BoLEff = .3f;
        public float HolyShock = .2f;

        public bool JotP = true;
        public bool LoHSelf = false;
        public bool Glyph_HL = true;
        public bool Glyph_FoL = false;
        public bool Glyph_Divinity = true;
        public bool Glyph_LoH = true;
        public bool Glyph_SoW = true;
        public bool Glyph_SoL = false;
	}
}
