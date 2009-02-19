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

            cbRotation.SelectedIndex = calcOpts.Rotation;
            panelCustom.Visible = calcOpts.Rotation == 10;

            cmbManaAmt.SelectedIndex = calcOpts.ManaPot;

            cbNewMana.Checked = calcOpts.NewManaRegen;

            trkActivity.Value = (int)calcOpts.FSRRatio;
            lblActivity.Text = trkActivity.Value + "% of fight spent in FSR.";

            trkFightLength.Value = (int)calcOpts.FightLength;
            lblFightLength.Text = trkFightLength.Value + " minute Fight.";

            trkSerendipity.Value = (int)calcOpts.Serendipity;
            lblSerendipity.Text = trkSerendipity.Value + "% Heals give Serendipity.";

            trkRapture.Value = (int)calcOpts.Rapture;
            lblRapture.Text = trkRapture.Value + "% of max Rapture returns.";

            trkReplenishment.Value = (int)calcOpts.Replenishment;
            lblReplenishment.Text = trkReplenishment.Value + "% time with Replenishment buff.";

            trkShadowfiend.Value = (int)calcOpts.Shadowfiend;
            lblShadowfiend.Text = trkShadowfiend.Value + "% effectiveness of Shadowfiend.";

            trkTestOfFaith.Value = (int)calcOpts.TestOfFaith;
            lblTestOfFaith.Text = trkTestOfFaith.Value + "% of heals use Test of Faith.";

            trkSurvivability.Value = (int)calcOpts.Survivability;
            lblSurvivability.Text = trkSurvivability.Value + "% weight on Survivability.";

            cbModelProcs.Checked = calcOpts.ModelProcs;

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
                calcOpts.Rotation = cbRotation.SelectedIndex;
                Character.OnCalculationsInvalidated();
                panelCustom.Visible = calcOpts.Rotation == 10;
            }
        }

        private void trkFightLength_Scroll(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsPriest calcOpts = Character.CalculationOptions as CalculationOptionsPriest;
                lblFightLength.Text = trkFightLength.Value + " minute Fight.";
                calcOpts.FightLength = trkFightLength.Value;
                Character.OnCalculationsInvalidated();
            }

        }

        private void trkSerendipity_Scroll(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsPriest calcOpts = Character.CalculationOptions as CalculationOptionsPriest;
                lblSerendipity.Text = trkSerendipity.Value + "% Heals give Serendipity.";
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
                lblTestOfFaith.Text = trkTestOfFaith.Value + "% of heals use Test of Faith.";
                calcOpts.TestOfFaith = trkTestOfFaith.Value;
                Character.OnCalculationsInvalidated();
            }
        }

        private void cbNewMana_CheckedChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsPriest calcOpts = Character.CalculationOptions as CalculationOptionsPriest;
                calcOpts.NewManaRegen = cbNewMana.Checked;
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
        public int Rotation = 0;
        public bool NewManaRegen = false;
        public float FSRRatio = 93f;
        public float FightLength = 6f;
        public float Serendipity = 75f;
        public float Replenishment = 50f;
        public float Shadowfiend = 100f;
        public float Survivability = 2f;
        public float Rapture = 75f;
        public float TestOfFaith = 25f;
        public bool ModelProcs = true;
	}
}
