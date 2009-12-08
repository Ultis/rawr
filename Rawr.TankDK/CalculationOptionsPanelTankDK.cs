using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Rawr.Base;

namespace Rawr.TankDK {
    public partial class CalculationOptionsPanelTankDK : Rawr.CalculationOptionsPanelBase {
        public CalculationOptionsPanelTankDK() 
        { 
            InitializeComponent(); 
        }

        protected override void LoadCalculationOptions() 
        {
            _loadingCalculationOptions = true;
            if (Character.CalculationOptions == null) 
            { 
                Character.CalculationOptions = new CalculationOptionsTankDK();
            }

            options = Character.CalculationOptions as CalculationOptionsTankDK;

            for (int i = 0; i < cmbAttackerLevel.Items.Count; i++) {
                if (cmbAttackerLevel.Items[i] as string == options.TargetLevel.ToString()) {
                    cmbAttackerLevel.SelectedItem = cmbAttackerLevel.Items[i];
                    break;
                }
            }

            comboChartType.SelectedItem = comboChartType.Items[(int)options.cType];

            numThreatWeight.Value = (decimal)options.ThreatWeight;
            numSurvivalWeight.Value = (decimal)options.SurvivalWeight;

            _loadingCalculationOptions = false;
        }

        private bool _loadingCalculationOptions = false;

        private CalculationOptionsTankDK options;

        private void cmbAttackerLevel_SelectedIndexChanged(object sender, EventArgs e) {
            if (!_loadingCalculationOptions) {
                options.TargetLevel = int.Parse(cmbAttackerLevel.SelectedItem.ToString());
                Character.OnCalculationsInvalidated();
            }
        }
        private void numThreatWeight_ValueChanged(object sender, EventArgs e) {
            if (!_loadingCalculationOptions) {
                options.ThreatWeight = (float)(numThreatWeight.Value);
                Character.OnCalculationsInvalidated();
            }
        }
        private void numBossAttackSpeed_ValueChanged(object sender, EventArgs e) {
            if (!_loadingCalculationOptions) {
                options.BossAttackSpeed = (float)(numBossAttackSpeed.Value);
                Character.OnCalculationsInvalidated();
            }
        }
        private void numSurvivalWeight_ValueChanged(object sender, EventArgs e) {
            if (!_loadingCalculationOptions) {
                options.SurvivalWeight = (float)(numSurvivalWeight.Value);
                Character.OnCalculationsInvalidated();
            }
        }
        private void numIncomingDamage_ValueChanged(object sender, EventArgs e) {
            if (!_loadingCalculationOptions) {
                options. IncomingDamage = (uint)(numIncomingDamage.Value);
                Character.OnCalculationsInvalidated();
            }
        }
        private void numPercIncFromMagic_ValueChanged(object sender, EventArgs e) {
            if (!_loadingCalculationOptions) {
                options.PercentIncomingFromMagic = (float)(numPercIncFromMagic.Value);
                Character.OnCalculationsInvalidated();
            }
        }
        private void btnRotation_Click(object sender, EventArgs e) {
            CalculationOptionsTankDK calcOpts = Character.CalculationOptions as CalculationOptionsTankDK;
            RotationViewer RV = new RotationViewer(calcOpts, Character);
            RV.ShowDialog();
            Character.OnCalculationsInvalidated();
        }
        private void nudTargetArmor_ValueChanged(object sender, EventArgs e) {
            if (!_loadingCalculationOptions) {
                options.BossArmor = (int)nudTargetArmor.Value;
                Character.OnCalculationsInvalidated();
            }
        }
        private void tbFightLength_Scroll(object sender, EventArgs e) {
            if (!_loadingCalculationOptions) {
                options.FightLength = (int)tbFightLength.Value;
                Character.OnCalculationsInvalidated();
            }
        }
        private void numTargets_ValueChanged(object sender, EventArgs e) {
            if (!_loadingCalculationOptions) {
                options.uNumberTargets = (uint)numTargets.Value;
                Character.OnCalculationsInvalidated();
            }
        }

        private void comboChartType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_loadingCalculationOptions)
            {
                options.cType = (CalculationType)comboChartType.SelectedIndex;
                Character.OnCalculationsInvalidated();
            }
        }

        private Stats[] BuildStatsList()
        {
            List<Stats> statsList = new List<Stats>();
//            if (chkStatsStrength.Checked)
                statsList.Add(new Stats() { Strength = 1f });
//            if (chkStatsAgility.Checked)
                statsList.Add(new Stats() { Agility = 1f });
//            if (chkStatsAP.Checked)
                statsList.Add(new Stats() { AttackPower = 2f });
//            if (chkStatsCrit.Checked)
                statsList.Add(new Stats() { CritRating = 1f });
//            if (chkStatsHit.Checked)
                statsList.Add(new Stats() { HitRating = 1f });
//            if (chkStatsExp.Checked)
                statsList.Add(new Stats() { ExpertiseRating = 1f });
//            if (chkStatsHaste.Checked)
                statsList.Add(new Stats() { HasteRating = 1f });
//            if (chkStatsArP.Checked)
                statsList.Add(new Stats() { ArmorPenetrationRating = 1f });
//            if (chkStatsSP.Checked)
                statsList.Add(new Stats() { DefenseRating = 1f });
//            if (chkStatsInt.Checked)
                statsList.Add(new Stats() { DodgeRating = 1f });
                statsList.Add(new Stats() { ParryRating = 1f });

            return statsList.ToArray();
        }

        private void btnStatsGraph_Click_1(object sender, EventArgs e)
        {
            Stats[] statsList = BuildStatsList();
            Graph graph = new Graph();
            string explanatoryText = "This graph shows how adding or subtracting\nmultiples of a stat affects your Overall Score.\n\nAt the Zero position is your current Overall.\n" +
                         "To the right of the zero vertical is adding stats.\nTo the left of the zero vertical is subtracting stats.\n" +
                         "The vertical axis shows the amount of Overall points added or lost";
            graph.SetupGraph(Character, statsList, 1000, explanatoryText, null);
            graph.Show();
        }

        private void cb_AdditiveMitigation_CheckedChanged(object sender, EventArgs e)
        {
            if (!_loadingCalculationOptions)
            {
                options.AdditiveMitigation = cb_AdditiveMitigation.Checked;
                Character.OnCalculationsInvalidated();
            }
        }
    }
}
