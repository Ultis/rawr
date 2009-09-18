using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

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
    }
}
