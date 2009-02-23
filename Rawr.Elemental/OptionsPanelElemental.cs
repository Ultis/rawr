using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Elemental
{
    public partial class CalculationOptionsPanelElemental : CalculationOptionsPanelBase
    {
        private bool loading = false;

        public CalculationOptionsPanelElemental()
        {
            InitializeComponent();

            // Set about text
            tbModuleNotes.Text =
                "Notes:\r\n" +
                "For the estimator, it is assumed you use Flametongue weapon.\r\n" +
                "The estimator is a theoretical estimation at the moment and only uses a rotation with Flameshock, Lava Blast and Lightning Bolt.\r\n" +
                "Trinkets, Elemental Mastery and Clearcasting are modelled by calculating their average value during the fight.\r\n" +
                "The estimator assumes you use Glyph of Flame Shock.";
        }

        protected override void LoadCalculationOptions()
        {
            loading = true;

            CalculationOptionsElemental calcOpts;

            if (Character.CalculationOptions == null)
            {
                calcOpts = new CalculationOptionsElemental();
                Character.CalculationOptions = calcOpts;
            }
            else
                calcOpts = (CalculationOptionsElemental)Character.CalculationOptions;

            tbBSRatio.Value = calcOpts.BSRatio;
            int burst = 100 - tbBSRatio.Value;
            int sust = tbBSRatio.Value;
            lblBSRatio.Text = "Ratio: "+burst + "% Burst, "+sust + "% Sustained.";

            trkFightLength.Value = calcOpts.FightDuration / 15;
            int m = trkFightLength.Value / 4;
            int s = calcOpts.FightDuration - 60 * m;
            lblFightLength.Text = "Fight duration: " + m + ":" + s;
            cmbManaAmt.SelectedIndex = calcOpts.ManaPot;
            tkReplenishment.Value = calcOpts.ReplenishmentUptime;
            lblReplenishment.Text = tkReplenishment.Value + "% of fight spent with Replenishment.";

            cbFlameshock.Checked = calcOpts.glyphOfFlameShock;
            cbElementalMastery.Checked = calcOpts.glyphOfElementalMastery;
            cbFlametongue.Checked = calcOpts.glyphOfFlametongue;
            cbLava.Checked = calcOpts.glyphOfLava;
            cbLightningBolt.Checked = calcOpts.glyphOfLightningBolt;
            cbShocking.Checked = calcOpts.glyphOfShocking;
            cbWaterMastery.Checked = calcOpts.glyphOfWaterMastery;

            cbThunderstorm.Checked = calcOpts.UseThunderstorm;
            cbFS.Checked = calcOpts.UseFSalways;
            cbLvB.Checked = !calcOpts.UseLvBalways;

            loading = false;

            //Enable/Disable Glyph Checkboxes
            chbSomeGlyph_CheckedChanged(null, null);
        }

        private void chbSomeGlyph_CheckedChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsElemental calcOpts = (CalculationOptionsElemental)Character.CalculationOptions;
                calcOpts.glyphOfShocking = cbShocking.Checked;
                calcOpts.glyphOfFlameShock = cbFlameshock.Checked;
                calcOpts.glyphOfLava = cbLava.Checked;
                calcOpts.glyphOfElementalMastery = cbElementalMastery.Checked;
                calcOpts.glyphOfFlametongue = cbFlametongue.Checked;
                calcOpts.glyphOfLightningBolt = cbLightningBolt.Checked;
                calcOpts.glyphOfWaterMastery = cbWaterMastery.Checked;

                //Disable Glyphcheckboxes to enable only X Glyphs (70 = 2, 80 = 3)
                int maxGlyphs = 3;
                if ((cbLightningBolt.Checked ? 1 : 0) + (cbFlametongue.Checked ? 1 : 0) + (cbElementalMastery.Checked ? 1 : 0) + (cbLava.Checked ? 1 : 0) + (cbFlameshock.Checked ? 1 : 0) + (cbShocking.Checked ? 1 : 0) + (cbWaterMastery.Checked ? 1 : 0) >= maxGlyphs)
                {
                    cbFlameshock.Enabled = cbFlameshock.Checked;
                    cbShocking.Enabled = cbShocking.Checked;
                    cbElementalMastery.Enabled = cbElementalMastery.Checked;
                    cbFlametongue.Enabled = cbFlametongue.Checked;
                    cbLightningBolt.Enabled = cbLightningBolt.Checked;
                    cbLava.Enabled = cbLava.Checked;
                    cbWaterMastery.Enabled = cbWaterMastery.Checked;
                }
                else
                {
                    cbFlameshock.Enabled = true;
                    cbShocking.Enabled = true;
                    cbElementalMastery.Enabled = true;
                    cbFlametongue.Enabled = true;
                    cbLightningBolt.Enabled = true;
                    cbLava.Enabled = true;
                    cbWaterMastery.Enabled = true;
                }

                Character.OnCalculationsInvalidated();
            }
        }

        private float parseFloat(string s)
        {
            float tmp;
            float.TryParse(s, out tmp);

            if (tmp > 0)
                return tmp;
            
            return 0;
        }

        private void trkFightLength_Scroll(object sender, EventArgs e)
        {
            if (loading) return;
            CalculationOptionsElemental calcOpts = Character.CalculationOptions as CalculationOptionsElemental;
            calcOpts.FightDuration = trkFightLength.Value * 15;
            int m = trkFightLength.Value / 4;
            int s = calcOpts.FightDuration - 60 * m;
            lblFightLength.Text = "Fight duration: "+m+":"+s;
            Character.OnCalculationsInvalidated();
        }

        private void cmbManaAmt_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (loading) return;
            CalculationOptionsElemental calcOpts = Character.CalculationOptions as CalculationOptionsElemental;
            calcOpts.ManaPot = cmbManaAmt.SelectedIndex;
            Character.OnCalculationsInvalidated();
        }

        private void tkReplenishment_Scroll(object sender, EventArgs e)
        {
            if (loading) return;
            CalculationOptionsElemental calcOpts = Character.CalculationOptions as CalculationOptionsElemental;
            calcOpts.ReplenishmentUptime = tkReplenishment.Value;
            lblReplenishment.Text = tkReplenishment.Value + "% of fight spent with Replenishment.";
            Character.OnCalculationsInvalidated();
        }

        private void tbBSRatio_Scroll(object sender, EventArgs e)
        {
            if (loading) return;
            CalculationOptionsElemental calcOpts = Character.CalculationOptions as CalculationOptionsElemental;
            calcOpts.BSRatio = tbBSRatio.Value;
            int burst = 100 - tbBSRatio.Value;
            int sust = tbBSRatio.Value;
            lblBSRatio.Text = "Ratio: "+burst + "% Burst, "+sust + "% Sustained.";
            Character.OnCalculationsInvalidated();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (loading) return;
            CalculationOptionsElemental calcOpts = Character.CalculationOptions as CalculationOptionsElemental;
            calcOpts.UseThunderstorm = cbThunderstorm.Checked;
            Character.OnCalculationsInvalidated();
        }

        private void cbFS_CheckedChanged(object sender, EventArgs e)
        {
            if (loading) return;
            CalculationOptionsElemental calcOpts = Character.CalculationOptions as CalculationOptionsElemental;
            calcOpts.UseFSalways = cbFS.Checked;
            Character.OnCalculationsInvalidated();
        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            if (loading) return;
            CalculationOptionsElemental calcOpts = Character.CalculationOptions as CalculationOptionsElemental;
            calcOpts.UseLvBalways = !cbLvB.Checked;
            Character.OnCalculationsInvalidated();
        }
    }

    [Serializable]
    public class CalculationOptionsElemental : ICalculationOptionBase
    {
        [System.Xml.Serialization.XmlIgnore]
        public CharacterCalculationsElemental calculatedStats = null;

        public int BSRatio = 75; // goes from 0 to 100

        public int FightDuration = 300; //5 Minutes
        public int ManaPot = 0; // none
        public int ReplenishmentUptime = 30;
        public bool UseThunderstorm = true;
        public bool UseFSalways = true;
        public bool UseLvBalways = true;

        public bool UseSimulator = false;

        public bool glyphOfFlameShock = true;
        public bool glyphOfElementalMastery = false;
        public bool glyphOfFlametongue = false;

        public bool glyphOfLava = false;
        public bool glyphOfLightningBolt = false;
        public bool glyphOfShocking = false;

        public bool glyphOfWaterMastery = false;

        public void SetGlyph(int index, bool value)
        {
            switch (index)
            {
                case 0:
                    glyphOfFlameShock = value;
                    break;
                case 1:
                    glyphOfElementalMastery = value;
                    break;
                case 2:
                    glyphOfFlametongue = value;
                    break;
                case 3:
                    glyphOfLava = value;
                    break;
                case 4:
                    glyphOfLightningBolt = value;
                    break;
                case 5:
                    glyphOfShocking = value;
                    break;
            }
        }

        public bool GetGlyph(int index)
        {
            switch (index)
            {
                case 0:
                    return glyphOfFlameShock;
                case 1:
                    return glyphOfElementalMastery;
                case 2:
                    return glyphOfFlametongue;
                case 3:
                    return glyphOfLava;
                case 4:
                    return glyphOfLightningBolt;
                case 5:
                    return glyphOfShocking;
            }
            return false;
        }

        public string getGlyphName(int index)
        {
            switch (index)
            {
                case 0:
                    return "Glyph of Flame Shock";
                case 1:
                    return "Glyph of Elemental Mastery";
                case 2:
                    return "Glyph of Flametongue";
                case 3:
                    return "Glyph of Lava";
                case 4:
                    return "Glyph of Lightning Bolt";
                case 5:
                    return "Glyph of Shocking";
            }
            return "";
        }

        public string getShortGlyphName(int index)
        {
            switch (index)
            {
                case 0:
                    return "Flame Shock";
                case 1:
                    return "Elemental Mastery";
                case 2:
                    return "Flametongue";
                case 3:
                    return "Lava";
                case 4:
                    return "Lightning Bolt";
                case 5:
                    return "Shocking";
            }
            return "";
        }

        public string GetXml()
        {
            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptionsElemental));
            StringBuilder xml = new StringBuilder();
            System.IO.StringWriter writer = new System.IO.StringWriter(xml);
            serializer.Serialize(writer, this);
            return xml.ToString();
        }
    }
}
