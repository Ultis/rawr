using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Rawr.Retribution
{
    public partial class CalculationOptionsPanelRetribution : CalculationOptionsPanelBase
    {

        public CalculationOptionsPanelRetribution()
        {
            InitializeComponent();
        }

        private bool loading;

        protected override void LoadCalculationOptions()
        {
            loading = true;
            if (Character.CalculationOptions == null)
                Character.CalculationOptions = new CalculationOptionsRetribution();

            CalculationOptionsRetribution calcOpts = Character.CalculationOptions as CalculationOptionsRetribution;

            cmbMobType.SelectedIndex = calcOpts.MobType;
            cmbLevel.SelectedIndex = calcOpts.TargetLevel - 80;
            cmbLength.Value = (decimal)calcOpts.FightLength;

            trkTime20.Value = (int)(calcOpts.TimeUnder20 * 100);
            lblTime20.Text = trkTime20.Value + "%";

            txtJudgeCD.Text = calcOpts.JudgementCD.ToString();
            txtJudgeCD20.Text = calcOpts.JudgementCD20.ToString();

            txtCSCD.Text = calcOpts.CrusaderStrikeCD.ToString();
            txtCSCD20.Text = calcOpts.CrusaderStrikeCD20.ToString();

            txtDSCD.Text = calcOpts.DivineStormCD.ToString();
            txtDSCD20.Text = calcOpts.DivineStormCD20.ToString();

            txtConsCD.Text = calcOpts.ConescrationCD.ToString();
            txtConsCD20.Text = calcOpts.ConescrationCD20.ToString();

            txtExoCD.Text = calcOpts.ExorcismCD.ToString();
            txtExoCD20.Text = calcOpts.ExorcismCD20.ToString();

            txtHoWCD20.Text = calcOpts.HammerOfWrathCD20.ToString();

            chkMode31.Checked = calcOpts.Mode31;
            chkGlyphJudgement.Checked = calcOpts.GlyphJudgement;
            chkGlyphConsecration.Checked = calcOpts.GlyphConsecration;
            chkGlyphSenseUndead.Checked = calcOpts.GlyphSenseUndead;
            
            loading = false;
        }

        private void cmbMobType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsRetribution calcOpts = Character.CalculationOptions as CalculationOptionsRetribution;
                calcOpts.MobType = cmbMobType.SelectedIndex;
                Character.OnCalculationsInvalidated();
            }
        }

        private void chk31Mode_CheckedChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsRetribution calcOpts = Character.CalculationOptions as CalculationOptionsRetribution;
                calcOpts.Mode31 = chkMode31.Checked;
                Character.OnCalculationsInvalidated();
            }
        }

        private void chkGlyphJudgement_CheckedChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsRetribution calcOpts = Character.CalculationOptions as CalculationOptionsRetribution;
                calcOpts.GlyphJudgement = chkGlyphJudgement.Checked;
                Character.OnCalculationsInvalidated();
            }
        }

        private void cmbLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsRetribution calcOpts = Character.CalculationOptions as CalculationOptionsRetribution;
                calcOpts.TargetLevel = cmbMobType.SelectedIndex + 80;
                Character.OnCalculationsInvalidated();
            }
        }

        private void cmbLength_ValueChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsRetribution calcOpts = Character.CalculationOptions as CalculationOptionsRetribution;
                calcOpts.FightLength = (float)cmbLength.Value;
                Character.OnCalculationsInvalidated();
            }
        }

        private void trkTime20_Scroll(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsRetribution calcOpts = Character.CalculationOptions as CalculationOptionsRetribution;
                calcOpts.TimeUnder20 = trkTime20.Value / 100f;
                lblTime20.Text = trkTime20.Value + "%";
                Character.OnCalculationsInvalidated();
            }
        }

        private void chkGlyphConsecration_CheckedChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsRetribution calcOpts = Character.CalculationOptions as CalculationOptionsRetribution;
                calcOpts.GlyphConsecration = chkGlyphConsecration.Checked;
                Character.OnCalculationsInvalidated();
            }
        }

        private void chkGlyphSenseUndead_CheckedChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsRetribution calcOpts = Character.CalculationOptions as CalculationOptionsRetribution;
                calcOpts.GlyphSenseUndead = chkGlyphSenseUndead.Checked;
                Character.OnCalculationsInvalidated();
            }
        }

        private void txtJudgeCD_TextChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsRetribution calcOpts = Character.CalculationOptions as CalculationOptionsRetribution;
                try
                {
                    calcOpts.JudgementCD = int.Parse(txtJudgeCD.Text);
                    Character.OnCalculationsInvalidated();
                }
                catch (Exception) { ; }
            }
        }

        private void txtJudgeCD20_TextChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsRetribution calcOpts = Character.CalculationOptions as CalculationOptionsRetribution;
                try
                {
                    calcOpts.JudgementCD20 = int.Parse(txtJudgeCD20.Text);
                    Character.OnCalculationsInvalidated();
                }
                catch (Exception) { ; }
            }
        }

        private void txtCSCD_TextChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsRetribution calcOpts = Character.CalculationOptions as CalculationOptionsRetribution;
                try
                {
                    calcOpts.CrusaderStrikeCD = int.Parse(txtCSCD.Text);
                    Character.OnCalculationsInvalidated();
                }
                catch (Exception) { ; }
            }
        }

        private void txtCSCD20_TextChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsRetribution calcOpts = Character.CalculationOptions as CalculationOptionsRetribution;
                try
                {
                    calcOpts.CrusaderStrikeCD20 = int.Parse(txtCSCD20.Text);
                    Character.OnCalculationsInvalidated();
                }
                catch (Exception) { ; }
            }
        }

        private void txtDSCD_TextChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsRetribution calcOpts = Character.CalculationOptions as CalculationOptionsRetribution;
                try
                {
                    calcOpts.DivineStormCD = int.Parse(txtDSCD.Text);
                    Character.OnCalculationsInvalidated();
                }
                catch (Exception) { ; }
            }
        }

        private void txtDSCD20_TextChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsRetribution calcOpts = Character.CalculationOptions as CalculationOptionsRetribution;
                try
                {
                    calcOpts.DivineStormCD20 = int.Parse(txtDSCD20.Text);
                    Character.OnCalculationsInvalidated();
                }
                catch (Exception) { ; }
            }
        }

        private void txtConsCD_TextChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsRetribution calcOpts = Character.CalculationOptions as CalculationOptionsRetribution;
                try
                {
                    calcOpts.ConescrationCD = int.Parse(txtConsCD.Text);
                    Character.OnCalculationsInvalidated();
                }
                catch (Exception) { ; }
            }
        }

        private void txtConsCD20_TextChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsRetribution calcOpts = Character.CalculationOptions as CalculationOptionsRetribution;
                try
                {
                    calcOpts.ConescrationCD20 = int.Parse(txtConsCD20.Text);
                    Character.OnCalculationsInvalidated();
                }
                catch (Exception) { ; }
            }
        }

        private void txtExoCD_TextChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsRetribution calcOpts = Character.CalculationOptions as CalculationOptionsRetribution;
                try
                {
                    calcOpts.ExorcismCD = int.Parse(txtExoCD.Text);
                    Character.OnCalculationsInvalidated();
                }
                catch (Exception) { ; }
            }
        }

        private void txtExoCD20_TextChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsRetribution calcOpts = Character.CalculationOptions as CalculationOptionsRetribution;
                try
                {
                    calcOpts.ExorcismCD20 = int.Parse(txtExoCD20.Text);
                    Character.OnCalculationsInvalidated();
                }
                catch (Exception) { ; }
            }
        }

        private void txtHoWCD20_TextChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsRetribution calcOpts = Character.CalculationOptions as CalculationOptionsRetribution;
                try
                {
                    calcOpts.HammerOfWrathCD20 = int.Parse(txtHoWCD20.Text);
                    Character.OnCalculationsInvalidated();
                }
                catch (Exception) { ; }
            }
        }

    }

	[Serializable]
	public class CalculationOptionsRetribution : ICalculationOptionBase
	{
		public string GetXml()
		{
			System.Xml.Serialization.XmlSerializer serializer =
				new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptionsRetribution));
			StringBuilder xml = new StringBuilder();
			System.IO.StringWriter writer = new System.IO.StringWriter(xml);
			serializer.Serialize(writer, this);
			return xml.ToString();
		}

		public int TargetLevel = 83;
        public int MobType = 0;
        public float FightLength = 5f;
        public float TimeUnder20 = .18f;
        public bool Mode31 = false;

        public float JudgementCD = 7.1f;
        public float DivineStormCD = 10.5f;
        public float CrusaderStrikeCD = 7.1f;
        public float ConescrationCD = 10.5f;
        public float ExorcismCD = 18f;

        public float JudgementCD20 = 7.1f;
        public float DivineStormCD20 = 12.5f;
        public float CrusaderStrikeCD20 = 7.1f;
        public float ConescrationCD20 = 12.5f;
        public float ExorcismCD20 = 25f;
        public float HammerOfWrathCD20 = 6.4f;

        public bool GlyphJudgement = true;
        public bool GlyphConsecration = true;
        public bool GlyphSenseUndead = true;

	}
}
