using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

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
    
            if (Character.CalculationOptions == null)
                Character.CalculationOptions = new CalculationOptionsTree();

            CalculationOptionsTree calcOpts = (CalculationOptionsTree)Character.CalculationOptions;

            cbLevel.SelectedIndex = calcOpts.level == 70 ? 0 : 1;
            cbSchattrathFaction.SelectedIndex = calcOpts.ShattrathFaction == "Aldor" ? 1 : calcOpts.ShattrathFaction == "None" ? 0 : 2;
            cbSpellRotation.SelectedIndex = calcOpts.spellRotationPlaceholder == "Healing Touch" ? 0 : 1;
            tbSurvScale.Text = calcOpts.SurvScaleBelowTarget.ToString();
            tbSurvTargetH.Text = calcOpts.SurvTargetLife.ToString();
            tbAverageProcUsage.Text = calcOpts.averageSpellpowerUsage.ToString();
            tbMP5Scale.Text = calcOpts.mP5Scale.ToString();
            tbFightDuration.Text = calcOpts.fightDuration.ToString();
            tbWildGrowthTargets.Text = calcOpts.wildGrowthTargets.ToString();
            tbWildGrowthAverageTicks.Text = calcOpts.wildGrowthTicks.ToString();
            tbReplenishmentActive.Text = calcOpts.averageReplenishActiveTime.ToString();
            chbLivingSeed.Checked = calcOpts.useLivingSeedAsCritMultiplicator;
            chbReplenishment.Checked = calcOpts.haveReplenishSupport;

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

        private void cbLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                ((CalculationOptionsTree)Character.CalculationOptions).level = int.Parse((string)cbLevel.Items[cbLevel.SelectedIndex]);
                Character.OnCalculationsInvalidated();
            }
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

        private void chbReplenishment_CheckedChanged(object sender, EventArgs e)
        {
            if (!loading)
                ((CalculationOptionsTree)Character.CalculationOptions).haveReplenishSupport = chbReplenishment.Checked;
            Character.OnCalculationsInvalidated();
        }

        private void tbReplenishmentActive_TextChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                float tmp = parseFloat(tbReplenishmentActive.Text);
                if (tmp > 100)
                {
                    tmp = 100;
                    tbReplenishmentActive.Text = "100";
                }

                ((CalculationOptionsTree)Character.CalculationOptions).averageReplenishActiveTime = tmp;
                Character.OnCalculationsInvalidated();
            }
        }

        private void tbAverageProcUsage_TextChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                float tmp = parseFloat(tbAverageProcUsage.Text);
                
                if (tmp > 100)
                {
                    tmp = 100;
                    tbAverageProcUsage.Text = "100";
                }

                ((CalculationOptionsTree)Character.CalculationOptions).averageSpellpowerUsage = tmp;
                
                Character.OnCalculationsInvalidated();
            }
        }

        private void cbSpellRotation_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                ((CalculationOptionsTree)Character.CalculationOptions).spellRotationPlaceholder = (string)cbSpellRotation.Items[cbSpellRotation.SelectedIndex];
                Character.OnCalculationsInvalidated();
            }
        }

        private void tbMP5Scale_TextChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                ((CalculationOptionsTree)Character.CalculationOptions).mP5Scale = parseFloat(tbMP5Scale.Text);
                Character.OnCalculationsInvalidated();
            }
        }

        private void tbFightDuration_TextChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                TimeSpan tmp;
                TimeSpan.TryParse(tbFightDuration.Text, out tmp);

                if (tmp.TotalMinutes > 0)
                {
                    ((CalculationOptionsTree)Character.CalculationOptions).fightDuration = tmp;
                    Character.OnCalculationsInvalidated();
                }
            }
        }

        private void tbWildGrowthTargets_TextChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                int tmp = (int)parseFloat(tbWildGrowthTargets.Text);
                if (tmp > 5)
                {
                    tmp = 5;
                    tbWildGrowthTargets.Text = "5";
                }

                ((CalculationOptionsTree)Character.CalculationOptions).wildGrowthTargets = tmp;
                Character.OnCalculationsInvalidated();
            }
        }

        private void tbWildGrowthAverageTicks_TextChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                int tmp = (int)parseFloat(tbWildGrowthAverageTicks.Text);
                if (tmp > 7)
                {
                    tmp = 7;
                    tbWildGrowthAverageTicks.Text = "7";
                }
                ((CalculationOptionsTree)Character.CalculationOptions).wildGrowthTicks = tmp;
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
                int maxGlyphs = calcOpts.level == 70 ? 2 : 3;
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



    }

    [Serializable]

    public class CalculationOptionsTree : ICalculationOptionBase
    {
        public int level = 70;
        public string ShattrathFaction = "Aldor";
        public bool useLivingSeedAsCritMultiplicator = true;

        public float SurvTargetLife = 8500f;
        public float SurvScaleBelowTarget = 100f;

        public float mP5Scale = 150f; //150% more Value if you don't get the whole fightduration

        //Add Average Spellpower to Calculation = 0.0f (% used)
        public float averageSpellpowerUsage = 0.0f;

        public TimeSpan fightDuration = new TimeSpan(0, 5, 0); //5 Minutes

        public bool haveReplenishSupport = true;
        public float averageReplenishActiveTime = 0.8f;

        //Spellrotations
        public string spellRotationPlaceholder = "Healing Touch";

        public int wildGrowthTargets = 4; //0-5 Targets
        public int wildGrowthTicks = 4; //0-7 Ticks

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
