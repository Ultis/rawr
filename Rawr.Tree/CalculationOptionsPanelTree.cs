using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Tree {
    public partial class CalculationOptionsPanelTree : CalculationOptionsPanelBase {
        private bool loading = false;
        public CalculationOptionsPanelTree() {
            InitializeComponent();

            // Set about text
            tbModuleNotes.Text =
                "Notes on trinkets:\r\n"+
                "Some trinkets are hard to model. They can be much better if you use them in a smart way. They are modelled by taking the expected uptime or number of procs and converting it to an average value.\r\n"+
                "\r\n" +
                "Trinket notes:\r\n" +
                "Talisman of Troll Divinity (keep in mind that it also helps other healers so it is probably worth more)\r\n" +
                "Alchemist Stones (mana potions are modelled as extra mana at the start of the fight)\r\n" +
                "\r\n"+
                "Implemented: \r\n"+
                "Majestic Dragon Figurine, Flow of Knowledge, Forethought Talisman, Forge Ember, Dying Curse, Illustration of the Dragon Soul, "+
                "Soul of the Dead, Je'tze's Bell, Spark of Life, Various Figurines, The Egg of Mortal Essence, "+
                "Embrace of the Spider, Darkmoon Card: Blue Dragon, Darkmoon Card: Greatness, " +
                "Spirit-World Glass, Living Ice Crystals, Soul Preserver, Spark of Hope \r\n" +
                "Various On Use Spell Power trinkets\r\n"+
                "\r\n"+
                "Currently unmodelled trinkets:\r\n" +
                "Darkmoon Card: Illusion\r\n"+
                "\r\n"+
                "HealBurst is the maximum HPS of your current rotation until you go out of mana (with 100% casting Primary healing spells outside keeping HoTs on the tanks).\r\n"+
                "HealSustained is the HPS from your current rotation for the entire fight.\r\n" +
                "Survival is scaled based on the physical damage you can take based on your health and armor.\r\n";
        }
        protected override void LoadCalculationOptions() {
            loading = true;

            CalculationOptionsTree calcOpts;

            if (Character.CalculationOptions == null){
                calcOpts = new CalculationOptionsTree();
                Character.CalculationOptions = calcOpts;
            }else{
                calcOpts = (CalculationOptionsTree)Character.CalculationOptions;
            }

            tbBSRatio.Value = calcOpts.BSRatio;
            int burst = 100 - tbBSRatio.Value;
            int sust = tbBSRatio.Value;
            lblBSRatio.Text = "Ratio: "+burst + "% Burst, "+sust + "% Sustained.";

            tbSurvMulti.Value = calcOpts.SurvValuePer100;
            lblSurvMulti.Text = calcOpts.SurvValuePer100.ToString() + " points per 100 survival (effective) health";

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

            cbInnervate.Checked = calcOpts.Innervates > 0;
            //tbPrimaryHealFrac.Value = calcOpts.MainSpellFraction;
            lblPrimaryHeal.Text = "Primary Heal Usage: " + tbPrimaryHealFrac.Value + "%";

            tbSwiftmendPerMin.Value = calcOpts.SwiftmendPerMinute;
            lblSwiftMend.Text = "Swiftmends per Minute: " + tbSwiftmendPerMin.Value;

            tbIdlePercentage.Value = calcOpts.IdleCastTimePercent;
            lblIdleFraction.Text = "Idle percentage: " + tbIdlePercentage.Value;

            loading = false;
        }
        private float parseFloat(string s) {
            float tmp;
            float.TryParse(s, out tmp);
            if (tmp > 0f) { return tmp; }
            return 0f;
        }
        private void cbRotation_SelectedIndexChanged(object sender, EventArgs e) {
            if (loading) { return; }
            CalculationOptionsTree calcOpts = Character.CalculationOptions as CalculationOptionsTree;
            calcOpts.Rotation = cbRotation.SelectedIndex;
            Character.OnCalculationsInvalidated();
        }
        private void trkTimeInFSR_Scroll(object sender, EventArgs e) {
            if (loading) { return; }
            CalculationOptionsTree calcOpts = Character.CalculationOptions as CalculationOptionsTree;
            lblFSR.Text = trkTimeInFSR.Value + "% of fight spent in FSR.";
            calcOpts.FSRRatio = trkTimeInFSR.Value;
            Character.OnCalculationsInvalidated();
        }
        private void trkFightLength_Scroll(object sender, EventArgs e) {
            if (loading) { return; }
            CalculationOptionsTree calcOpts = Character.CalculationOptions as CalculationOptionsTree;
            calcOpts.FightDuration = trkFightLength.Value * 15;
            int m = trkFightLength.Value / 4;
            int s = calcOpts.FightDuration - 60 * m;
            lblFightLength.Text = "Fight duration: "+m+":"+s;
            Character.OnCalculationsInvalidated();
        }
        private void cmbManaAmt_SelectedIndexChanged(object sender, EventArgs e) {
            if (loading) { return; }
            CalculationOptionsTree calcOpts = Character.CalculationOptions as CalculationOptionsTree;
            calcOpts.ManaPot = cmbManaAmt.SelectedIndex;
            Character.OnCalculationsInvalidated();
        }
        private void tkReplenishment_Scroll(object sender, EventArgs e) {
            if (loading) { return; }
            CalculationOptionsTree calcOpts = Character.CalculationOptions as CalculationOptionsTree;
            calcOpts.ReplenishmentUptime = tkReplenishment.Value;
            lblReplenishment.Text = tkReplenishment.Value + "% of fight spent with Replenishment.";
            Character.OnCalculationsInvalidated();
        }
        private void tbWildGrowth_Scroll(object sender, EventArgs e) {
            if (loading) { return; }
            CalculationOptionsTree calcOpts = Character.CalculationOptions as CalculationOptionsTree;
            calcOpts.WildGrowthPerMinute = tbWildGrowth.Value;
            lblWG.Text = tbWildGrowth.Value + " Wild Growth casts per minute.";
            Character.OnCalculationsInvalidated();
        }
        private void tbBSRatio_Scroll(object sender, EventArgs e) {
            if (loading) return;
            CalculationOptionsTree calcOpts = Character.CalculationOptions as CalculationOptionsTree;
            calcOpts.BSRatio = tbBSRatio.Value;
            int burst = 100 - tbBSRatio.Value;
            int sust = tbBSRatio.Value;
            lblBSRatio.Text = "Ratio: "+burst + "% Burst, "+sust + "% Sustained.";
            Character.OnCalculationsInvalidated();
        }
        private void tbSurvMulti_Scroll(object sender, EventArgs e) {
            if (loading) { return; }
            CalculationOptionsTree calcOpts = Character.CalculationOptions as CalculationOptionsTree;
            calcOpts.SurvValuePer100 = tbSurvMulti.Value;
            lblSurvMulti.Text = calcOpts.SurvValuePer100.ToString() + " points per 100 survival (effective) health";
            Character.OnCalculationsInvalidated();
        }
        private void cbInnervate_CheckedChanged(object sender, EventArgs e) {
            if (loading) { return; }
            CalculationOptionsTree calcOpts = Character.CalculationOptions as CalculationOptionsTree;
            calcOpts.Innervates = cbInnervate.Checked?1:0;
            Character.OnCalculationsInvalidated();
        }
        private void tbPrimaryHealFrac_Scroll(object sender, EventArgs e) {
            if (loading) { return; }
            CalculationOptionsTree calcOpts = Character.CalculationOptions as CalculationOptionsTree;
            //calcOpts.MainSpellFraction = tbPrimaryHealFrac.Value;
            lblPrimaryHeal.Text = "Primary Heal Usage: " + tbPrimaryHealFrac.Value + "%";
            Character.OnCalculationsInvalidated();
        }
        private void tbSwiftmend_Scroll(object sender, EventArgs e) {
            if (loading) { return; }
            CalculationOptionsTree calcOpts = Character.CalculationOptions as CalculationOptionsTree;
            calcOpts.SwiftmendPerMinute = tbSwiftmendPerMin.Value;
            lblSwiftMend.Text = "Swiftmends per minute: " + tbSwiftmendPerMin.Value;
            Character.OnCalculationsInvalidated();
        }
        private void tbIdlePercentage_Scroll(object sender, EventArgs e)
        {
            if (loading) { return; }
            CalculationOptionsTree calcOpts = Character.CalculationOptions as CalculationOptionsTree;
            calcOpts.IdleCastTimePercent = tbIdlePercentage.Value;
            lblIdleFraction.Text = "Idle percentage: " + tbIdlePercentage.Value;
            Character.OnCalculationsInvalidated();
        }
        private void cbApplyMore_CheckedChanged(object sender, EventArgs e)
        {
            if (loading) { return; }
            CalculationOptionsTree calcOpts = Character.CalculationOptions as CalculationOptionsTree;
            //calcOpts.PenalizeEverything = cbApplyMore.Checked;
            Character.OnCalculationsInvalidated();
        }

    }
}
