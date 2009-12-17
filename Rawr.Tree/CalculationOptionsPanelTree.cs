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
                "The module is currently undergoing lots of work.\r\n\r\n" +
                "You're welcome to send feedback via the CodePlex site.\r\n\r\n" +
                "A small list of things I plan to do:\r\n- Fix haste trinkets to give better values.\r\n- Add a much simpler interface, including preset fight profiles (e.g. Tank heal, Raid heal, Hybrid healing, WG+RJ all out et cetera).\r\n- Add Healing Touch in spell rotations.\r\n- Lifebloom x2 stacks.\r\n- A custom chart that shows HPM for single target rotations.\r\n Update Main Stats statistics to show healing without crit, healing with crit and crit chance.";
        }
        protected override void LoadCalculationOptions() {
            loading = true;

            CalculationOptionsTree calcOpts;

            if (Character.CalculationOptions == null){
                calcOpts = new CalculationOptionsTree();
                Character.CalculationOptions = calcOpts;
            } else {
                calcOpts = (CalculationOptionsTree)Character.CalculationOptions;
            }

            tbSingleTargetMax.Value = calcOpts.SingleTarget / 100;
            lblSingleTargetMax.Text = "Single Target Max Healing Target: " + calcOpts.SingleTarget + " hps";

            tbSustained.Value = calcOpts.SustainedTarget / 100;
            lblSustained.Text = "Sustained Healing Target: " + calcOpts.SustainedTarget + " hps";

            tbSurvMulti.Value = calcOpts.SurvValuePer100;
            lblSurvMulti.Text = calcOpts.SurvValuePer100.ToString() + " points per 100 survival (effective) health";

            trkTimeInFSR.Value = calcOpts.FSRRatio;
            lblFSR.Text = trkTimeInFSR.Value + "% of fight spent in FSR.";

            trkFightLength.Value = calcOpts.FightDuration / 5;
            int m = trkFightLength.Value / 12;
            int s = calcOpts.FightDuration - 60 * m;
            lblFightLength.Text = "Fight duration: " + m + ":" + (s < 10 ? "0" : "") + s;

            calcOpts.Latency = 0;
            tbLatency.Value = calcOpts.Latency / 10;
            lblLatency.Text = "Latency: " + calcOpts.Latency + " ms.";

            cmbManaAmt.SelectedIndex = calcOpts.ManaPot;

            tkReplenishment.Value = calcOpts.ReplenishmentUptime;
            lblReplenishment.Text = tkReplenishment.Value + "% of fight spent with Replenishment.";
            
            tbRejuvCF.Value = calcOpts.RejuvFrac;
            tbRegrowthCF.Value = calcOpts.RegrowthFrac;
            tbLifebloomCF.Value = calcOpts.LifebloomFrac;
            tbNourishCF.Value = calcOpts.NourishFrac;
            lblRejuvFrac.Text = "Rejuvenation casting time: " + tbRejuvCF.Value + "%.";
            lblRegrowthFrac.Text = "Regrowth casting time: " + tbRegrowthCF.Value + "%.";
            lblLifebloomFrac.Text = "Lifebloom casting time: " + tbLifebloomCF.Value + "%.";
            lblNourishFrac.Text = "Nourish casting time: " + tbNourishCF.Value + "%.";

            tbLifebloomStackAmount.Value = calcOpts.LifebloomStackAmount;
            tbRejuvAmount.Value = calcOpts.RejuvAmount;
            tbRegrowthAmount.Value = calcOpts.RegrowthAmount;
            lblLifebloomStackAmount.Text = "Number of maintained Lifebloom stacks: " + tbLifebloomStackAmount.Value;
            lblRejuvAmount.Text = "Number of maintained Rejuvenations: " + tbRejuvAmount.Value;
            lblRegrowthAmount.Text = "Number of maintained Regrowths: " + tbRegrowthAmount.Value;

            cbLifebloomStackType.SelectedIndex = calcOpts.LifebloomStackType;

            tbNourish1.Value = calcOpts.Nourish1;
            tbNourish2.Value = calcOpts.Nourish2;
            tbNourish3.Value = calcOpts.Nourish3;
            tbNourish4.Value = calcOpts.Nourish4;
            lblNourish1.Text = "Nourish on 1 hot: " + tbNourish1.Value + "%.";
            lblNourish2.Text = "Nourish on 2 hots: " + tbNourish2.Value + "%.";
            lblNourish3.Text = "Nourish on 3 hots: " + tbNourish3.Value + "%.";
            lblNourish4.Text = "Nourish on 4 hots: " + tbNourish4.Value + "%.";

            calcOpts.AdjustRejuv = cbRejuvAdjust.Checked;
            calcOpts.AdjustRegrowth = cbRegrowthAdjust.Checked;
            calcOpts.AdjustLifebloom = cbLifebloomAdjust.Checked;
            calcOpts.AdjustNourish = cbNourishAdjust.Checked;

            tbLivingSeed.Value = calcOpts.LivingSeedEfficiency;
            lblLivingSeed.Text = "Living Seed effectiveness: " + (float)tbLivingSeed.Value + "%.";

            tbWildGrowth.Value = calcOpts.WildGrowthPerMinute;
            lblWG.Text = tbWildGrowth.Value + " Wild Growth casts per minute.";

            tbSwiftmendPerMin.Value = calcOpts.SwiftmendPerMinute;
            lblSwiftMend.Text = "Swiftmend casts per minute: " + tbSwiftmendPerMin.Value;

            cbInnervate.Checked = calcOpts.Innervates > 0;

            tbIdlePercentage.Value = calcOpts.IdleCastTimePercent;
            lblIdleFraction.Text = "Idle time: " + tbIdlePercentage.Value + "%.";

            cbSingleTargetRotation.SelectedIndex = calcOpts.SingleTargetRotation;

            cbIgnoreNaturesGrace.Checked = calcOpts.IgnoreNaturesGrace;
            cbIgnoreAllHasteEffects.Checked = calcOpts.IgnoreAllHasteEffects;

            tbRevitalize.Value = calcOpts.RevitalizePPM;
            lblRevitalize.Text = "Revitalize procs per minute: " + (float)calcOpts.RevitalizePPM;


            lbOOMspells.Items.Clear();
            for (int i = 0; i < 5; i++)
            {
                if (calcOpts.ReduceOOMRejuvOrder == i)
                {
                    lbOOMspells.Items.Add("Rejuvenation until " + calcOpts.ReduceOOMRejuv + "%.");
                }
                else if (calcOpts.ReduceOOMRegrowthOrder == i)
                {
                    lbOOMspells.Items.Add("Regrowth until " + calcOpts.ReduceOOMRegrowth + "%.");
                }
                else if (calcOpts.ReduceOOMLifebloomOrder == i)
                {
                    lbOOMspells.Items.Add("Lifebloom until " + calcOpts.ReduceOOMLifebloom + "%.");
                }
                else if (calcOpts.ReduceOOMNourishOrder == i)
                {
                    lbOOMspells.Items.Add("Nourish until " + calcOpts.ReduceOOMNourish + "%.");
                }
                else if (calcOpts.ReduceOOMWildGrowthOrder == i)
                {
                    lbOOMspells.Items.Add("WildGrowth until " + calcOpts.ReduceOOMWildGrowth + "%.");
                }
            }

            tbOOMpercentage.Enabled = false;

            loading = false;
        }
        private float parseFloat(string s) {
            float tmp;
            float.TryParse(s, out tmp);
            if (tmp > 0f) { return tmp; }
            return 0f;
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
        private void trkTimeInFSR_Scroll(object sender, EventArgs e)
        {
            if (loading) { return; }
            CalculationOptionsTree calcOpts = Character.CalculationOptions as CalculationOptionsTree;
            lblFSR.Text = trkTimeInFSR.Value + "% of fight spent in FSR.";
            calcOpts.FSRRatio = trkTimeInFSR.Value;
            Character.OnCalculationsInvalidated();
        }
        private void cmbManaAmt_SelectedIndexChanged(object sender, EventArgs e)
        {
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
        private void tbWildGrowth_Scroll(object sender, EventArgs e)
        {
            if (loading) { return; }
            CalculationOptionsTree calcOpts = Character.CalculationOptions as CalculationOptionsTree;
            calcOpts.WildGrowthPerMinute = tbWildGrowth.Value;
            lblWG.Text = tbWildGrowth.Value + " Wild Growth casts per minute.";
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
            lblIdleFraction.Text = "Idle time: " + tbIdlePercentage.Value + "%.";
            Character.OnCalculationsInvalidated();
        }

        private void tbAvgLifebloomStacks_Scroll(object sender, EventArgs e)
        {
            if (loading) { return; }
            CalculationOptionsTree calcOpts = Character.CalculationOptions as CalculationOptionsTree;
            calcOpts.LifebloomStackAmount = tbLifebloomStackAmount.Value;
            lblLifebloomStackAmount.Text = "Number of maintained Lifebloom stacks: " + (float)tbLifebloomStackAmount.Value;
            Character.OnCalculationsInvalidated();
        }

        private void tbRejuvAmount_Scroll(object sender, EventArgs e)
        {
            if (loading) { return; }
            CalculationOptionsTree calcOpts = Character.CalculationOptions as CalculationOptionsTree;
            calcOpts.RejuvAmount = tbRejuvAmount.Value;
            lblRejuvAmount.Text = "Number of maintained Rejuvenations: " + (float)tbRejuvAmount.Value;
            Character.OnCalculationsInvalidated();
        }

        private void tbRegrowthAmount_Scroll(object sender, EventArgs e)
        {
            if (loading) { return; }
            CalculationOptionsTree calcOpts = Character.CalculationOptions as CalculationOptionsTree;
            calcOpts.RegrowthAmount = tbRegrowthAmount.Value;
            lblRegrowthAmount.Text = "Number of maintained Regrowths: " + (float)tbRegrowthAmount.Value;
            Character.OnCalculationsInvalidated();
        }

        private void cbLifebloomStackType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (loading) { return; }
            CalculationOptionsTree calcOpts = Character.CalculationOptions as CalculationOptionsTree;
            calcOpts.LifebloomStackType = cbLifebloomStackType.SelectedIndex;
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

        private void tbLatency_Scroll(object sender, EventArgs e)
        {
            if (loading) { return; }
            CalculationOptionsTree calcOpts = Character.CalculationOptions as CalculationOptionsTree;
            calcOpts.Latency = tbLatency.Value * 10;
            lblLatency.Text = "Latency: " + calcOpts.Latency + " ms.";
            Character.OnCalculationsInvalidated();
        }

        private void tbRejuvCF_Scroll(object sender, EventArgs e)
        {
            if (loading) { return; }
            CalculationOptionsTree calcOpts = Character.CalculationOptions as CalculationOptionsTree;
            calcOpts.RejuvFrac = tbRejuvCF.Value;
            lblRejuvFrac.Text = "Rejuvenation casting time: " + (float)tbRejuvCF.Value + "%.";
            Character.OnCalculationsInvalidated();
        }

        private void tbRegrowthCF_Scroll(object sender, EventArgs e)
        {
            if (loading) { return; }
            CalculationOptionsTree calcOpts = Character.CalculationOptions as CalculationOptionsTree;
            calcOpts.RegrowthFrac = tbRegrowthCF.Value;
            lblRegrowthFrac.Text = "Regrowth casting time: " + (float)tbRegrowthCF.Value + "%.";
            Character.OnCalculationsInvalidated();
        }

        private void tbLifebloomCF_Scroll(object sender, EventArgs e)
        {
            if (loading) { return; }
            CalculationOptionsTree calcOpts = Character.CalculationOptions as CalculationOptionsTree;
            calcOpts.LifebloomFrac = tbLifebloomCF.Value;
            lblLifebloomFrac.Text = "Lifebloom casting time: " + (float)tbLifebloomCF.Value + "%.";
            Character.OnCalculationsInvalidated();
        }

        private void tbNourishCF_Scroll(object sender, EventArgs e)
        {
            if (loading) { return; }
            CalculationOptionsTree calcOpts = Character.CalculationOptions as CalculationOptionsTree;
            calcOpts.NourishFrac = tbNourishCF.Value;
            lblNourishFrac.Text = "Nourish casting time: " + (float)tbNourishCF.Value + "%.";
            Character.OnCalculationsInvalidated();
        }

        private void cbRejuvAdjust_CheckedChanged(object sender, EventArgs e)
        {
            if (loading) return;
            CalculationOptionsTree calcOpts = Character.CalculationOptions as CalculationOptionsTree;
            calcOpts.AdjustRejuv = cbRejuvAdjust.Checked;
            Character.OnCalculationsInvalidated();
        }

        private void cbRegrowthAdjust_CheckedChanged(object sender, EventArgs e)
        {
            if (loading) return;
            CalculationOptionsTree calcOpts = Character.CalculationOptions as CalculationOptionsTree;
            calcOpts.AdjustRegrowth = cbRegrowthAdjust.Checked;
            Character.OnCalculationsInvalidated();
        }

        private void cbLifebloomAdjust_CheckedChanged(object sender, EventArgs e)
        {
            if (loading) return;
            CalculationOptionsTree calcOpts = Character.CalculationOptions as CalculationOptionsTree;
            calcOpts.AdjustLifebloom = cbLifebloomAdjust.Checked;
            Character.OnCalculationsInvalidated();
        }

        private void cbNourishAdjust_CheckedChanged(object sender, EventArgs e)
        {
            if (loading) return;
            CalculationOptionsTree calcOpts = Character.CalculationOptions as CalculationOptionsTree;
            calcOpts.AdjustNourish = cbNourishAdjust.Checked;
            Character.OnCalculationsInvalidated();
        }

        private void cbIgnoreNaturesGrace_CheckedChanged(object sender, EventArgs e)
        {
            if (loading) return;
            CalculationOptionsTree calcOpts = Character.CalculationOptions as CalculationOptionsTree;
            calcOpts.IgnoreNaturesGrace = cbIgnoreNaturesGrace.Checked;
            Character.OnCalculationsInvalidated();
        }

        private void cbIgnoreAllHasteEffects_CheckedChanged(object sender, EventArgs e)
        {
            if (loading) return;
            CalculationOptionsTree calcOpts = Character.CalculationOptions as CalculationOptionsTree;
            calcOpts.IgnoreAllHasteEffects = cbIgnoreAllHasteEffects.Checked;
            Character.OnCalculationsInvalidated();
        }


        private void tbRejuvRevitalize_Scroll(object sender, EventArgs e)
        {
            if (loading) return;
            CalculationOptionsTree calcOpts = Character.CalculationOptions as CalculationOptionsTree;
            calcOpts.RevitalizePPM = tbRevitalize.Value;
            lblRevitalize.Text = "Revitalize procs per minute: " + (float)calcOpts.RevitalizePPM;
            Character.OnCalculationsInvalidated();
        }

        private int currentSelection;
        // 0 = rejuv
        // 1 = regrowth
        // 2 = lifebloom
        // 3 = nourish
        // 4 = wild growth

        private void btnOOMup_Click(object sender, EventArgs e)
        {
            if (loading) return;
            CalculationOptionsTree calcOpts = Character.CalculationOptions as CalculationOptionsTree;
            if (lbOOMspells.SelectedItem == null || lbOOMspells.SelectedIndex == 0) { return; }

            int index = lbOOMspells.SelectedIndex - 1;
            lbOOMspells.Items.Insert(lbOOMspells.SelectedIndex - 1, lbOOMspells.SelectedItem);
            lbOOMspells.Items.RemoveAt(lbOOMspells.SelectedIndex);
            lbOOMspells.SelectedIndex = index;

            for (int i = 0; i < 5; i++)
            {
                if (((string)lbOOMspells.Items[i]).StartsWith("Rejuvenation"))
                {
                    calcOpts.ReduceOOMRejuvOrder = i;
                }
                else if (((string)lbOOMspells.Items[i]).StartsWith("Regrowth"))
                {
                    calcOpts.ReduceOOMRegrowthOrder = i;
                }
                else if (((string)lbOOMspells.Items[i]).StartsWith("Lifebloom"))
                {
                    calcOpts.ReduceOOMLifebloomOrder = i;
                }
                else if (((string)lbOOMspells.Items[i]).StartsWith("Nourish"))
                {
                    calcOpts.ReduceOOMNourishOrder = i;
                }
                else if (((string)lbOOMspells.Items[i]).StartsWith("Wild Growth"))
                {
                    calcOpts.ReduceOOMWildGrowthOrder = i;
                }
            }

        }

        private void btnOOMdown_Click(object sender, EventArgs e)
        {
            if (loading) return;
            CalculationOptionsTree calcOpts = Character.CalculationOptions as CalculationOptionsTree;
            if (lbOOMspells.SelectedItem == null || lbOOMspells.SelectedIndex == lbOOMspells.Items.Count - 1) { return; }

            int index = lbOOMspells.SelectedIndex + 1;
            lbOOMspells.Items.Insert(lbOOMspells.SelectedIndex + 2, lbOOMspells.SelectedItem);
            lbOOMspells.Items.RemoveAt(lbOOMspells.SelectedIndex);
            lbOOMspells.SelectedIndex = index;

            for (int i = 0; i < 5; i++)
            {
                if (((string)lbOOMspells.Items[i]).StartsWith("Rejuvenation"))
                {
                    calcOpts.ReduceOOMRejuvOrder = i;
                }
                else if (((string)lbOOMspells.Items[i]).StartsWith("Regrowth"))
                {
                    calcOpts.ReduceOOMRegrowthOrder = i;
                }
                else if (((string)lbOOMspells.Items[i]).StartsWith("Lifebloom"))
                {
                    calcOpts.ReduceOOMLifebloomOrder = i;
                }
                else if (((string)lbOOMspells.Items[i]).StartsWith("Nourish"))
                {
                    calcOpts.ReduceOOMNourishOrder = i;
                }
                else if (((string)lbOOMspells.Items[i]).StartsWith("Wild Growth"))
                {
                    calcOpts.ReduceOOMWildGrowthOrder = i;
                }
            }
        }

        private void lbOOMspells_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (loading) return;
            CalculationOptionsTree calcOpts = Character.CalculationOptions as CalculationOptionsTree;
            if (lbOOMspells.SelectedItem == null) { return; }

            if (((string)lbOOMspells.SelectedItem).StartsWith("Rejuvenation"))
            {
                tbOOMpercentage.Enabled = true;
                tbOOMpercentage.Value = calcOpts.ReduceOOMRejuv;
                currentSelection = 0;
            }
            else if (((string)lbOOMspells.SelectedItem).StartsWith("Regrowth"))
            {
                tbOOMpercentage.Enabled = true;
                tbOOMpercentage.Value = calcOpts.ReduceOOMRegrowth;
                currentSelection = 1;
            }
            else if (((string)lbOOMspells.SelectedItem).StartsWith("Lifebloom"))
            {
                tbOOMpercentage.Enabled = true;
                tbOOMpercentage.Value = calcOpts.ReduceOOMLifebloom;
                currentSelection = 2;
            }
            else if (((string)lbOOMspells.SelectedItem).StartsWith("Nourish"))
            {
                tbOOMpercentage.Enabled = true;
                tbOOMpercentage.Value = calcOpts.ReduceOOMNourish;
                currentSelection = 3;
            }
            else if (((string)lbOOMspells.SelectedItem).StartsWith("Wild Growth"))
            {
                tbOOMpercentage.Enabled = true;
                tbOOMpercentage.Value = calcOpts.ReduceOOMWildGrowth;
                currentSelection = 4;
            }
            else
            {
                tbOOMpercentage.Enabled = false;
                currentSelection = 5;
            }
        }

        private void tbOOMpercentage_Scroll(object sender, EventArgs e)
        {
            if (loading) return;
            CalculationOptionsTree calcOpts = Character.CalculationOptions as CalculationOptionsTree;
            String newText;
            int index = lbOOMspells.SelectedIndex;
            switch (currentSelection)
            {
                case 0:
                    calcOpts.ReduceOOMRejuv = tbOOMpercentage.Value;
                    newText = "Rejuvenation until " + tbOOMpercentage.Value + "%.";
                    lbOOMspells.Items.Insert(lbOOMspells.SelectedIndex + 1, newText);
                    lbOOMspells.Items.RemoveAt(lbOOMspells.SelectedIndex);
                    lbOOMspells.SelectedIndex = index;
                    break;
                case 1:
                    calcOpts.ReduceOOMRegrowth = tbOOMpercentage.Value;
                    newText = "Regrowth until " + tbOOMpercentage.Value + "%.";
                    lbOOMspells.Items.Insert(lbOOMspells.SelectedIndex + 1, newText);
                    lbOOMspells.Items.RemoveAt(lbOOMspells.SelectedIndex);
                    lbOOMspells.SelectedIndex = index;
                    break;
                case 2:
                    calcOpts.ReduceOOMLifebloom = tbOOMpercentage.Value;
                    newText = "Lifebloom until " + tbOOMpercentage.Value + "%.";
                    lbOOMspells.Items.Insert(lbOOMspells.SelectedIndex + 1, newText);
                    lbOOMspells.Items.RemoveAt(lbOOMspells.SelectedIndex);
                    lbOOMspells.SelectedIndex = index;
                    break;
                case 3:
                    calcOpts.ReduceOOMNourish = tbOOMpercentage.Value;
                    newText = "Nourish until " + tbOOMpercentage.Value + "%.";
                    lbOOMspells.Items.Insert(lbOOMspells.SelectedIndex + 1, newText);
                    lbOOMspells.Items.RemoveAt(lbOOMspells.SelectedIndex);
                    lbOOMspells.SelectedIndex = index;
                    break;
                case 4:
                    calcOpts.ReduceOOMWildGrowth = tbOOMpercentage.Value;
                    newText = "Wild Growth until " + tbOOMpercentage.Value + "%.";
                    lbOOMspells.Items.Insert(lbOOMspells.SelectedIndex + 1, newText);
                    lbOOMspells.Items.RemoveAt(lbOOMspells.SelectedIndex);
                    lbOOMspells.SelectedIndex = index;
                    break;
                default:
                    break;
            } 
            Character.OnCalculationsInvalidated();
        }

    }
}
