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

            // Set about text
            tbModuleNotes.Text =
                "Notes on trinkets:\r\n"+
                "Trinkets are hard to model; some trinkets can be much better if you use them in a smart way; we expect On-Use trinkets are used whenever on cooldown, there is a setting (average use of spellpower procs) that you can use to influence the value of these type of trinkets.\r\n"+
                "\r\n" +
                "Implemented trinkets:\r\n" +
                "Forethought Talisman (expected to proc every minute)\r\n" +
                "Majestic Dragon Figurine (expected 80% uptime after going out of mana)\r\n" +
                "Forge Ember (expected uptime 17%, it has 10% chance to proc)\r\n" +
                "Dying Curse (expected uptime 18%, it has 15% chance to proc)\r\n" +
                "Illustration of the Dragon Soul\r\n" +
                "Talisman of Troll Divinity (expected to add 58 spell damage on average, keep in mind that it also helps other healers so it is probably worth more)\r\n" +
                "Alchemist Stones (they add 40% to the potion effect, mana potions are modelled as extra mana at the start of the fight)\r\n" +
                "Haste procs like Embrace of the Spider are modelled with 17% uptime)\r\n" +
                "\r\n"+
                "Currently unmodelled trinkets:\r\n" +
                "Darkmoon Card: Blue Dragon, various Figurines, Soul Preserver\r\n"+
                "\r\n"+
                "HealBurst is the HPS you can expect with your current rotation until you go out of mana, but with 100% casting Primary healing spell outside keeping HoTs on the tank(s).\r\n"+
                "HealSustained is the HPS from your current rotation for the entire fight.";
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
            tbBSRatio.Value = calcOpts.BSRatio;
            int burst = 100 - tbBSRatio.Value;
            int sust = tbBSRatio.Value;
            lblBSRatio.Text = "Ratio: "+burst + "% Burst, "+sust + "% Sustained.";

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

            cbNewManaRegen.Checked = calcOpts.newManaRegen;
            cbInnervate.Checked = calcOpts.Innervates > 0;
            tbPrimaryHealFrac.Value = calcOpts.MainSpellFraction;
            lblPrimaryHeal.Text = "Primary Heal Usage: " + tbPrimaryHealFrac.Value + "%";

            tbOOMPenalty.Value = calcOpts.OOMPenalty;
            lblOOMPenalty.Text = "Penalty for going OOM: " + tbOOMPenalty.Value * .2f;
            cbApplyMore.Checked = calcOpts.PenalizeEverything;

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

        private void tbBSRatio_Scroll(object sender, EventArgs e)
        {
            if (loading) return;
            CalculationOptionsTree calcOpts = Character.CalculationOptions as CalculationOptionsTree;
            calcOpts.BSRatio = tbBSRatio.Value;
            int burst = 100 - tbBSRatio.Value;
            int sust = tbBSRatio.Value;
            lblBSRatio.Text = "Ratio: "+burst + "% Burst, "+sust + "% Sustained.";
            Character.OnCalculationsInvalidated();
        }

        private void cbNewManaRegen_CheckedChanged(object sender, EventArgs e)
        {
            if (loading) return;
            CalculationOptionsTree calcOpts = Character.CalculationOptions as CalculationOptionsTree;
            calcOpts.newManaRegen = cbNewManaRegen.Checked;
            Character.OnCalculationsInvalidated();
        }

        private void cbInnervate_CheckedChanged(object sender, EventArgs e)
        {
            if (loading) return;
            CalculationOptionsTree calcOpts = Character.CalculationOptions as CalculationOptionsTree;
            calcOpts.Innervates = cbInnervate.Checked?1:0;
            Character.OnCalculationsInvalidated();
        }

        private void tbPrimaryHealFrac_Scroll(object sender, EventArgs e)
        {
            if (loading) return;
            CalculationOptionsTree calcOpts = Character.CalculationOptions as CalculationOptionsTree;
            calcOpts.MainSpellFraction = tbPrimaryHealFrac.Value;
            lblPrimaryHeal.Text = "Primary Heal Usage: " + tbPrimaryHealFrac.Value + "%";
            Character.OnCalculationsInvalidated();
        }

        private void tbOOMPenalty_Scroll(object sender, EventArgs e)
        {
            if (loading) return;
            CalculationOptionsTree calcOpts = Character.CalculationOptions as CalculationOptionsTree;
            calcOpts.OOMPenalty = tbOOMPenalty.Value;
            lblOOMPenalty.Text = "Penalty for going OOM: " + tbOOMPenalty.Value * .2f;
            Character.OnCalculationsInvalidated();
        }

        private void cbApplyMore_CheckedChanged(object sender, EventArgs e)
        {
            if (loading) return;
            CalculationOptionsTree calcOpts = Character.CalculationOptions as CalculationOptionsTree;
            calcOpts.PenalizeEverything = cbApplyMore.Checked;
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

        public float SurvTargetLife = 14000f;
        public float SurvScaleBelowTarget = 100f;

        //Add Average Spellpower to Calculation = 0.0f (% used)
        public float averageSpellpowerUsage = 100f;

        public int BSRatio = 75; // goes from 0 to 100

        public int FightDuration = 180; // 3 Minutes
        public int Rotation = 6; // default: group regrowth and heal 1 tank
        public int ManaPot = 4; // best pot
        public int FSRRatio = 100;
        public int ReplenishmentUptime = 70;
        public int WildGrowthPerMinute = 3;
        public int MainSpellFraction = 60;
        public int Innervates = 1;

        public int OOMPenalty = 20;

        public bool PenalizeEverything = false;

        public bool glyphOfHealingTouch = false;
        public bool glyphOfRegrowth = false;
        public bool glyphOfRejuvenation = false;

        public bool glyphOfLifebloom = false;
        public bool glyphOfInnervate = false;
        public bool glyphOfSwiftmend = false;

        public bool newManaRegen = true;

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
