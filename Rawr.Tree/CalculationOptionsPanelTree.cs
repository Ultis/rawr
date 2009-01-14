using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Tree
{
    public partial class CalculationOptionsPanelTree : CalculationOptionsPanelBase
    {
        private bool loading = false;

        public CalculationOptionsPanelTree()
        {
            InitializeComponent();
        }

        protected override void LoadCalculationOptions()
        {
            loading = true;

            CalculationOptionsTree calcOpts;

            if (Character.CalculationOptions == null)
            {
                calcOpts = new CalculationOptionsTree();
                Character.CalculationOptions = calcOpts;
            }
            else
                calcOpts = (CalculationOptionsTree)Character.CalculationOptions;

            cbSchattrathFaction.SelectedIndex = calcOpts.ShattrathFaction == "Aldor" ? 1 : calcOpts.ShattrathFaction == "None" ? 0 : 2;
            tbSurvScale.Text = calcOpts.SurvScaleBelowTarget.ToString();
            tbSurvTargetH.Text = calcOpts.SurvTargetLife.ToString();
            tbAverageProcUsage.Text = calcOpts.averageSpellpowerUsage.ToString();
            chbLivingSeed.Checked = calcOpts.useLivingSeedAsCritMultiplicator;

            cbRotation.SelectedIndex = calcOpts.Rotation;
            lblFSR.Text = trkTimeInFSR.Value + "% of fight spent in FSR.";
            trkTimeInFSR.Value = calcOpts.FSRRatio;
            trkFightLength.Value = calcOpts.FightDuration / 15;
            int m = trkFightLength.Value / 4;
            int s = calcOpts.FightDuration - 60 * m;
            lblFightLength.Text = "Fight duration: " + m + ":" + s;
            cmbManaAmt.SelectedIndex = calcOpts.ManaPot;
            tkReplenishment.Value = calcOpts.ReplenishmentUptime;
            lblReplenishment.Text = tkReplenishment.Value + "% of fight spent with Replenishment.";
            tbWildGrowth.Value = calcOpts.WildGrowthPerMinute;
            lblWG.Text = tbWildGrowth.Value + " Wild Growth casts per minute.";

            chbGlyphHT.Checked = calcOpts.glyphOfHealingTouch;
            chbGlyphRegrowth.Checked = calcOpts.glyphOfRegrowth;
            chbGlyphRejuvenation.Checked = calcOpts.glyphOfRejuvenation;
            chbGlyphSwiftmend.Checked = calcOpts.glyphOfSwiftmend;
            chbGlyphLifebloom.Checked = calcOpts.glyphOfLifebloom;
            chbGlyphInnervate.Checked = calcOpts.glyphOfInnervate;

            loading = false;

            //Enable/Disable Glyph Checkboxes
            chbSomeGlyph_CheckedChanged(null, null);
        }

        private void cbSchattrathFaction_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                ((CalculationOptionsTree)Character.CalculationOptions).ShattrathFaction = cbSchattrathFaction.Items[cbSchattrathFaction.SelectedIndex].ToString();
                Character.OnCalculationsInvalidated();
            }
        }

        private void tbSurvTargetH_TextChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                ((CalculationOptionsTree)Character.CalculationOptions).SurvTargetLife = parseFloat(tbSurvTargetH.Text);
                Character.OnCalculationsInvalidated();
            }
        }

        private void tbSurvScale_TextChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                ((CalculationOptionsTree)Character.CalculationOptions).SurvScaleBelowTarget = parseFloat(tbSurvScale.Text);
                Character.OnCalculationsInvalidated();
            }
        }

        private void chbSomeGlyph_CheckedChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsTree calcOpts = (CalculationOptionsTree)Character.CalculationOptions;
                calcOpts.glyphOfInnervate = chbGlyphInnervate.Checked;
                calcOpts.glyphOfHealingTouch = chbGlyphHT.Checked;
                calcOpts.glyphOfLifebloom = chbGlyphLifebloom.Checked;
                calcOpts.glyphOfRegrowth = chbGlyphRegrowth.Checked;
                calcOpts.glyphOfRejuvenation = chbGlyphRejuvenation.Checked;
                calcOpts.glyphOfSwiftmend = chbGlyphSwiftmend.Checked;

                //Disable Glyphcheckboxes to enable only X Glyphs (70 = 2, 80 = 3)
                int maxGlyphs = 3;
                if ((chbGlyphSwiftmend.Checked ? 1 : 0) + (chbGlyphRejuvenation.Checked ? 1 : 0) + (chbGlyphRegrowth.Checked ? 1 : 0) + (chbGlyphLifebloom.Checked ? 1 : 0) + (chbGlyphInnervate.Checked ? 1 : 0) + (chbGlyphHT.Checked ? 1 : 0) >= maxGlyphs)
                {
                    chbGlyphHT.Enabled = chbGlyphHT.Checked;
                    chbGlyphInnervate.Enabled = chbGlyphInnervate.Checked;
                    chbGlyphRegrowth.Enabled = chbGlyphRegrowth.Checked;
                    chbGlyphRejuvenation.Enabled = chbGlyphRejuvenation.Checked;
                    chbGlyphSwiftmend.Enabled = chbGlyphSwiftmend.Checked;
                    chbGlyphLifebloom.Enabled = chbGlyphLifebloom.Checked;
                }
                else
                {
                    chbGlyphHT.Enabled = true;
                    chbGlyphInnervate.Enabled = true;
                    chbGlyphRegrowth.Enabled = true;
                    chbGlyphRejuvenation.Enabled = true;
                    chbGlyphSwiftmend.Enabled = true;
                    chbGlyphLifebloom.Enabled = true;
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

        private void cbRotation_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (loading) return;
            CalculationOptionsTree calcOpts = Character.CalculationOptions as CalculationOptionsTree;
            calcOpts.Rotation = cbRotation.SelectedIndex;
            Character.OnCalculationsInvalidated();
        }

        private void trkTimeInFSR_Scroll(object sender, EventArgs e)
        {
            if (loading) return;
            CalculationOptionsTree calcOpts = Character.CalculationOptions as CalculationOptionsTree;
            lblFSR.Text = trkTimeInFSR.Value + "% of fight spent in FSR.";
            calcOpts.FSRRatio = trkTimeInFSR.Value;
            Character.OnCalculationsInvalidated();
        }

        private void trkFightLength_Scroll(object sender, EventArgs e)
        {
            if (loading) return;
            CalculationOptionsTree calcOpts = Character.CalculationOptions as CalculationOptionsTree;
            calcOpts.FightDuration = trkFightLength.Value * 15;
            int m = trkFightLength.Value / 4;
            int s = calcOpts.FightDuration - 60 * m;
            lblFightLength.Text = "Fight duration: "+m+":"+s;
            Character.OnCalculationsInvalidated();
        }

        private void cmbManaAmt_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (loading) return;
            CalculationOptionsTree calcOpts = Character.CalculationOptions as CalculationOptionsTree;
            calcOpts.ManaPot = cmbManaAmt.SelectedIndex;
            Character.OnCalculationsInvalidated();
        }

        private void tkReplenishment_Scroll(object sender, EventArgs e)
        {
            if (loading) return;
            CalculationOptionsTree calcOpts = Character.CalculationOptions as CalculationOptionsTree;
            calcOpts.ReplenishmentUptime = tkReplenishment.Value;
            lblReplenishment.Text = tkReplenishment.Value + "% of fight spent with Replenishment.";
            Character.OnCalculationsInvalidated();
        }

        private void tbWildGrowth_Scroll(object sender, EventArgs e)
        {
            if (loading) return;
            CalculationOptionsTree calcOpts = Character.CalculationOptions as CalculationOptionsTree;
            calcOpts.WildGrowthPerMinute = tbWildGrowth.Value;
            lblWG.Text = tbWildGrowth.Value + " Wild Growth casts per minute.";
            Character.OnCalculationsInvalidated();
        }

        private void tbAverageProcUsage_TextChanged(object sender, EventArgs e)
        {
            if (loading) return;
            CalculationOptionsTree calcOpts = Character.CalculationOptions as CalculationOptionsTree;
            calcOpts.averageSpellpowerUsage = parseFloat(tbAverageProcUsage.Text);
            Character.OnCalculationsInvalidated();
        }
    }

    [Serializable]
    public class CalculationOptionsTree : ICalculationOptionBase
    {
        //I want the calculated Stats in the SpellrotationsEditor .. so I trade them over the Options
        [System.Xml.Serialization.XmlIgnore]
        public CharacterCalculationsTree calculatedStats = null;

        public string ShattrathFaction = "Aldor";
        public bool useLivingSeedAsCritMultiplicator = true;

        public float SurvTargetLife = 12000f;
        public float SurvScaleBelowTarget = 100f;

        //Add Average Spellpower to Calculation = 0.0f (% used)
        public float averageSpellpowerUsage = 80f;

        public int FightDuration = 300; //5 Minutes
        public int Rotation = 6; // default: group regrowth and heal 1 tank
        public int ManaPot = 0; // none
        public int FSRRatio = 90;
        public int ReplenishmentUptime = 30;
        public int WildGrowthPerMinute = 4;

        public bool glyphOfHealingTouch = false;
        public bool glyphOfRegrowth = false;
        public bool glyphOfRejuvenation = false;

        public bool glyphOfLifebloom = false;
        public bool glyphOfInnervate = false;
        public bool glyphOfSwiftmend = false;

        public string GetXml()
        {
            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptionsTree));
            StringBuilder xml = new StringBuilder();
            System.IO.StringWriter writer = new System.IO.StringWriter(xml);
            serializer.Serialize(writer, this);
            return xml.ToString();
        }
    }
}
