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
                "Some trinkets are hard to model. They can be much better if you use them in a smart way...\r\n"+
                "\r\n" +
                "HealBurst is the maximum HPS of your current primary heal plus any selected hots.\r\n"+
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

            tbSingleTargetMax.Value = calcOpts.SingleTarget / 100;
            lblSingleTargetMax.Text = "Single Target Max Healing Target: " + calcOpts.SingleTarget + " hps";
            tbSustained.Value = calcOpts.SustainedTarget / 100;
            lblSustained.Text = "Sustained Healing Target: " + calcOpts.SustainedTarget + " hps";

            tbSurvMulti.Value = calcOpts.SurvValuePer100;
            lblSurvMulti.Text = calcOpts.SurvValuePer100.ToString() + " points per 100 survival (effective) health";

            lblFSR.Text = trkTimeInFSR.Value + "% of fight spent in FSR.";
            trkTimeInFSR.Value = calcOpts.FSRRatio;
            trkFightLength.Value = calcOpts.FightDuration / 5;
            int m = trkFightLength.Value / 12;
            int s = calcOpts.FightDuration - 60 * m;
            lblFightLength.Text = "Fight duration: " + m + ":" + (s < 10 ? "0" : "") + s;
            cmbManaAmt.SelectedIndex = calcOpts.ManaPot;
            tkReplenishment.Value = calcOpts.ReplenishmentUptime;
            lblReplenishment.Text = tkReplenishment.Value + "% of fight spent with Replenishment.";
            
            tbAvgRejuv.Value = calcOpts.AverageRejuv;
            tbAvgRegrowths.Value = calcOpts.AverageRegrowths;
            tbAvgLifeblooms.Value = calcOpts.AverageLifebloom;
            tbAvgLifebloomStacks.Value = calcOpts.AverageLifebloomStack;
            lblAvgRejuv.Text = "Rejuvenation casting time: " + (float)tbAvgRejuv.Value + "%.";
            lblAvgRegrowths.Text = "Regrowth casting time: " + (float)tbAvgRegrowths.Value + "%.";
            lblAvgLifeblooms.Text = "Lifebloom casting time: " + (float)tbAvgLifeblooms.Value + "%.";
            lblAvgLifebloomStacks.Text = "Number of Lifebloom stacks: " + (float)tbAvgLifebloomStacks.Value;
            cbLifebloomStackType.SelectedIndex = calcOpts.LifebloomStackType;
            cbPrimarySpell.SelectedIndex = calcOpts.PrimaryHeal;
            tbNourish1.Value = calcOpts.Nourish1;
            lblNourish1.Text = "Nourish on 1 hot: " + tbNourish1.Value + "%.";
            tbNourish2.Value = calcOpts.Nourish2;
            lblNourish2.Text = "Nourish on 2 hots: " + tbNourish2.Value + "%.";
            tbNourish3.Value = calcOpts.Nourish3;
            lblNourish3.Text = "Nourish on 3 hots: " + tbNourish3.Value + "%.";
            tbNourish4.Value = calcOpts.Nourish4;
            lblNourish4.Text = "Nourish on 4 hots: " + tbNourish4.Value + "%.";
            tbLivingSeed.Value = calcOpts.LivingSeedEfficiency;
            lblLivingSeed.Text = "Living Seed effectiveness: " + (float)tbLivingSeed.Value + "%.";

            tbWildGrowth.Value = calcOpts.WildGrowthPerMinute;
            lblWG.Text = tbWildGrowth.Value + " Wild Growth casts per minute.";

            cbInnervate.Checked = calcOpts.Innervates > 0;

            tbSwiftmendPerMin.Value = calcOpts.SwiftmendPerMinute;
            lblSwiftMend.Text = "Swiftmend casts per minute: " + tbSwiftmendPerMin.Value;

            tbIdlePercentage.Value = calcOpts.IdleCastTimePercent;
            lblIdleFraction.Text = "Idle time: " + tbIdlePercentage.Value;

            cbSingleTargetRotation.SelectedIndex = calcOpts.SingleTargetRotation;
            cbIdleToHOTs.Checked = calcOpts.ApplyIdleToHots;

            loading = false;
        }
        private float parseFloat(string s) {
            float tmp;
            float.TryParse(s, out tmp);
            if (tmp > 0f) { return tmp; }
            return 0f;
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
            calcOpts.FightDuration = trkFightLength.Value * 5;
            int m = trkFightLength.Value / 12;
            int s = calcOpts.FightDuration - 60 * m;
            lblFightLength.Text = "Fight duration: " + m + ":" + (s < 10 ? "0" : "") + s;
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
            //lblPrimaryHeal.Text = "Primary Heal Usage: " + tbPrimaryHealFrac.Value + "%";
            Character.OnCalculationsInvalidated();
        }
        private void tbSwiftmend_Scroll(object sender, EventArgs e) {
            if (loading) { return; }
            CalculationOptionsTree calcOpts = Character.CalculationOptions as CalculationOptionsTree;
            calcOpts.SwiftmendPerMinute = tbSwiftmendPerMin.Value;
            lblSwiftMend.Text = "Swiftmend casts per minute: " + tbSwiftmendPerMin.Value;
            Character.OnCalculationsInvalidated();
        }
        private void tbIdlePercentage_Scroll(object sender, EventArgs e)
        {
            if (loading) { return; }
            CalculationOptionsTree calcOpts = Character.CalculationOptions as CalculationOptionsTree;
            calcOpts.IdleCastTimePercent = tbIdlePercentage.Value;
            lblIdleFraction.Text = "Idle time: " + tbIdlePercentage.Value;
            Character.OnCalculationsInvalidated();
        }
        private void cbApplyMore_CheckedChanged(object sender, EventArgs e)
        {
            if (loading) { return; }
            CalculationOptionsTree calcOpts = Character.CalculationOptions as CalculationOptionsTree;
            //calcOpts.PenalizeEverything = cbApplyMore.Checked;
            Character.OnCalculationsInvalidated();
        }

        private void tbAvgRejuv_Scroll(object sender, EventArgs e)
        {
            if (loading) { return; }
            CalculationOptionsTree calcOpts = Character.CalculationOptions as CalculationOptionsTree;
            calcOpts.AverageRejuv = tbAvgRejuv.Value;
            lblAvgRejuv.Text = "Rejuvenation casting time: " + (float)tbAvgRejuv.Value + "%.";
            Character.OnCalculationsInvalidated();
        }

        private void tbAvgRegrowths_Scroll(object sender, EventArgs e)
        {
            if (loading) { return; }
            CalculationOptionsTree calcOpts = Character.CalculationOptions as CalculationOptionsTree;
            calcOpts.AverageRegrowths = tbAvgRegrowths.Value;
            lblAvgRegrowths.Text = "Regrowth casting time: " + (float)tbAvgRegrowths.Value + "%.";
            Character.OnCalculationsInvalidated();
        }

        private void tbAvgLifeblooms_Scroll(object sender, EventArgs e)
        {
            if (loading) { return; }
            CalculationOptionsTree calcOpts = Character.CalculationOptions as CalculationOptionsTree;
            calcOpts.AverageLifebloom = tbAvgLifeblooms.Value;
            lblAvgLifeblooms.Text = "Lifebloom casting time: " + (float)tbAvgLifeblooms.Value + "%.";
            Character.OnCalculationsInvalidated();
        }

        private void tbAvgLifebloomStacks_Scroll(object sender, EventArgs e)
        {
            if (loading) { return; }
            CalculationOptionsTree calcOpts = Character.CalculationOptions as CalculationOptionsTree;
            calcOpts.AverageLifebloomStack = tbAvgLifebloomStacks.Value;
            lblAvgLifebloomStacks.Text = "Number of Lifebloom stacks: " + (float)tbAvgLifebloomStacks.Value;
            Character.OnCalculationsInvalidated();
        }

        private void cbLifebloomStackType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (loading) { return; }
            CalculationOptionsTree calcOpts = Character.CalculationOptions as CalculationOptionsTree;
            calcOpts.LifebloomStackType = cbLifebloomStackType.SelectedIndex;
            Character.OnCalculationsInvalidated();
        }

        private void cbPrimarySpell_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (loading) { return; }
            CalculationOptionsTree calcOpts = Character.CalculationOptions as CalculationOptionsTree;
            calcOpts.PrimaryHeal = cbPrimarySpell.SelectedIndex;
            Character.OnCalculationsInvalidated();
        }

        private void tbNourish1_Scroll(object sender, EventArgs e)
        {
            if (loading) { return; }
            CalculationOptionsTree calcOpts = Character.CalculationOptions as CalculationOptionsTree;

            calcOpts.Nourish1 = tbNourish1.Value;

            int max = 100 - (calcOpts.Nourish2 + calcOpts.Nourish3 + calcOpts.Nourish4);
            if (max < 0) max = 0;
            if (calcOpts.Nourish1 > max)
            {
                calcOpts.Nourish1 = max;
                tbNourish1.Value = max;
            }
            
            lblNourish1.Text = "Nourish on 1 hot: " + tbNourish1.Value + "%.";
            Character.OnCalculationsInvalidated();
        }

        private void tbNourish2_Scroll(object sender, EventArgs e)
        {
            if (loading) { return; }
            CalculationOptionsTree calcOpts = Character.CalculationOptions as CalculationOptionsTree;

            calcOpts.Nourish2 = tbNourish2.Value;

            int max = 100 - (calcOpts.Nourish1 + calcOpts.Nourish3 + calcOpts.Nourish4);
            if (max < 0) max = 0;
            if (calcOpts.Nourish2 > max)
            {
                calcOpts.Nourish2 = max;
                tbNourish2.Value = max;
            }

            lblNourish2.Text = "Nourish on 2 hots: " + tbNourish2.Value + "%.";
            Character.OnCalculationsInvalidated();
        }

        private void tbNourish3_Scroll(object sender, EventArgs e)
        {
            if (loading) { return; }
            CalculationOptionsTree calcOpts = Character.CalculationOptions as CalculationOptionsTree;

            calcOpts.Nourish3 = tbNourish3.Value;

            int max = 100 - (calcOpts.Nourish1 + calcOpts.Nourish2 + calcOpts.Nourish4);
            if (max < 0) max = 0;
            if (calcOpts.Nourish3 > max)
            {
                calcOpts.Nourish3 = max;
                tbNourish3.Value = max;
            }

            lblNourish3.Text = "Nourish on 3 hots: " + tbNourish3.Value + "%.";
            Character.OnCalculationsInvalidated();
        }

        private void tbNourish4_Scroll(object sender, EventArgs e)
        {
            if (loading) { return; }
            CalculationOptionsTree calcOpts = Character.CalculationOptions as CalculationOptionsTree;

            calcOpts.Nourish4 = tbNourish4.Value;

            int max = 100 - (calcOpts.Nourish1 + calcOpts.Nourish2 + calcOpts.Nourish3);
            if (max < 0) max = 0;
            if (calcOpts.Nourish4 > max)
            {
                calcOpts.Nourish4 = max;
                tbNourish4.Value = max;
            }

            lblNourish4.Text = "Nourish on 4 hots: " + tbNourish4.Value + "%.";
            Character.OnCalculationsInvalidated();
        }

        private void tbLivingSeed_Scroll(object sender, EventArgs e)
        {
            if (loading) { return; }
            CalculationOptionsTree calcOpts = Character.CalculationOptions as CalculationOptionsTree;
            calcOpts.LivingSeedEfficiency = tbLivingSeed.Value;
            lblLivingSeed.Text = "Living Seed effectiveness: " + (float)tbLivingSeed.Value + "%.";
            Character.OnCalculationsInvalidated();
        }

        private void tbSingleTargetMax_Scroll(object sender, EventArgs e)
        {
            if (loading) return;
            CalculationOptionsTree calcOpts = Character.CalculationOptions as CalculationOptionsTree;
            calcOpts.SingleTarget = tbSingleTargetMax.Value * 100;
            lblSingleTargetMax.Text = "Single Target Max Healing Target: " + calcOpts.SingleTarget + " hps";
            Character.OnCalculationsInvalidated();
        }

        private void tbSustained_Scroll(object sender, EventArgs e)
        {
            if (loading) return;
            CalculationOptionsTree calcOpts = Character.CalculationOptions as CalculationOptionsTree;
            calcOpts.SustainedTarget = tbSustained.Value * 100;
            lblSustained.Text = "Sustained Healing Target: " + calcOpts.SustainedTarget + " hps";
            Character.OnCalculationsInvalidated();
        }

        private void cbSingleTargetRotation_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (loading) return;
            CalculationOptionsTree calcOpts = Character.CalculationOptions as CalculationOptionsTree;
            calcOpts.SingleTargetRotation = cbSingleTargetRotation.SelectedIndex;
            Character.OnCalculationsInvalidated();
        }

        private void cbIdleToHOTs_CheckedChanged(object sender, EventArgs e)
        {
            if (loading) return;
            CalculationOptionsTree calcOpts = Character.CalculationOptions as CalculationOptionsTree;
            calcOpts.ApplyIdleToHots = cbIdleToHOTs.Checked;
            Character.OnCalculationsInvalidated();
        }

    }
}
