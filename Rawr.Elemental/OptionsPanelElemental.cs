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
                "It is assumed you use Flametongue weapon.\r\n" +
                "The calculator is a theoretical estimation at the moment and only uses a rotation with Flameshock, Lava Blast and Lightning Bolt.\r\n" +
                "Trinkets, Elemental Mastery and Clearcasting are modelled by calculating their average value during the fight.\r\n" +
                "The simplified calculator assumes you use Glyph of Flame Shock, hence you cannot disable that glyph.";
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

            calcOpts.glyphOfFlameShock = true;
            chbGlyphFS.Checked = true;
            chbGlyphEM.Checked = calcOpts.glyphOfElementalMastery;
            chbGlyphFT.Checked = calcOpts.glyphOfFlametongue;
            chbGlyphLava.Checked = calcOpts.glyphOfLava;
            chbGlyphLightningBolt.Checked = calcOpts.glyphOfLightningBolt;
            chbGlyphShocking.Checked = calcOpts.glyphOfShocking;

            cbThunderstorm.Checked = calcOpts.UseThunderstorm;

            loading = false;

            //Enable/Disable Glyph Checkboxes
            chbSomeGlyph_CheckedChanged(null, null);
        }

        private void chbSomeGlyph_CheckedChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsElemental calcOpts = (CalculationOptionsElemental)Character.CalculationOptions;
                calcOpts.glyphOfShocking = chbGlyphShocking.Checked;
                calcOpts.glyphOfFlameShock = true;
                calcOpts.glyphOfLava = chbGlyphLava.Checked;
                calcOpts.glyphOfElementalMastery = chbGlyphEM.Checked;
                calcOpts.glyphOfFlametongue = chbGlyphFT.Checked;
                calcOpts.glyphOfLightningBolt = chbGlyphLightningBolt.Checked;

                //Disable Glyphcheckboxes to enable only X Glyphs (70 = 2, 80 = 3)
                int maxGlyphs = 3;
                if ((chbGlyphLightningBolt.Checked ? 1 : 0) + (chbGlyphFT.Checked ? 1 : 0) + (chbGlyphEM.Checked ? 1 : 0) + (chbGlyphLava.Checked ? 1 : 0) + (chbGlyphShocking.Checked ? 1 : 0) + (chbGlyphFS.Checked ? 1 : 0) >= maxGlyphs)
                {
                    chbGlyphFS.Enabled = false;
                    chbGlyphShocking.Enabled = chbGlyphShocking.Checked;
                    chbGlyphEM.Enabled = chbGlyphEM.Checked;
                    chbGlyphFT.Enabled = chbGlyphFT.Checked;
                    chbGlyphLightningBolt.Enabled = chbGlyphLightningBolt.Checked;
                    chbGlyphLava.Enabled = chbGlyphLava.Checked;
                }
                else
                {
                    chbGlyphFS.Enabled = false;
                    chbGlyphShocking.Enabled = true;
                    chbGlyphEM.Enabled = true;
                    chbGlyphFT.Enabled = true;
                    chbGlyphLightningBolt.Enabled = true;
                    chbGlyphLava.Enabled = true;
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

        public bool glyphOfFlameShock = true;
        public bool glyphOfElementalMastery = false;
        public bool glyphOfFlametongue = false;

        public bool glyphOfLava = false;
        public bool glyphOfLightningBolt = false;
        public bool glyphOfShocking = false;

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
