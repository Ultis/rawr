using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace Rawr.Retribution
{
    public partial class CalculationOptionsPanelRetribution : CalculationOptionsPanelBase
    {

        public CalculationOptionsPanelRetribution()
        {
            InitializeComponent();
        }

        private bool loading;

        protected override void LoadCalculationOptions()
        {
            loading = true;
            if (Character.CalculationOptions == null)
                Character.CalculationOptions = new CalculationOptionsRetribution();

            CalculationOptionsRetribution calcOpts = Character.CalculationOptions as CalculationOptionsRetribution;

            cmbMobType.SelectedIndex = (int)calcOpts.Mob;

            if (calcOpts.Seal == SealOf.Blood)
            {
                calcOpts.Seal = SealOf.Vengeance;
                Character.OnCalculationsInvalidated();
            }
            cmbSeal.SelectedIndex = (int)calcOpts.Seal - 1;
            
            cmbLength.Value = (decimal)calcOpts.FightLength;

            nudTimeUnder20.Value = (decimal)(calcOpts.TimeUnder20 * 100);
            nudInFront.Value = (decimal)(calcOpts.InFront * 100);
            nudConsEff.Value = (decimal)(calcOpts.ConsEff * 100);
            nudHoR.Value = (decimal)(calcOpts.HoREff * 100);
            nudTargetSwitch.Value = (decimal)calcOpts.TargetSwitches;
            nudTargets.Value = (decimal)calcOpts.Targets;

            chkBloodlust.Checked = calcOpts.Bloodlust;

            nudDelay.Value = (decimal)calcOpts.Delay;
            nudWait.Value = (decimal)calcOpts.Wait;
            nudStackTrinket.Value = (decimal)calcOpts.StackTrinketReset;
            nudTargetLevel.Value = (decimal)calcOpts.TargetLevel;

            nudJudge.Value = (decimal)calcOpts.JudgeCD;
            nudCS.Value = (decimal)calcOpts.CSCD;
            nudDS.Value = (decimal)calcOpts.DSCD;
            nudCons.Value = (decimal)calcOpts.ConsCD;
            nudExo.Value = (decimal)calcOpts.ExoCD;
            nudJudge20.Value = (decimal)calcOpts.JudgeCD20;
            nudCS20.Value = (decimal)calcOpts.CSCD20;
            nudDS20.Value = (decimal)calcOpts.DSCD20;
            nudCons20.Value = (decimal)calcOpts.ConsCD20;
            nudExo20.Value = (decimal)calcOpts.ExoCD20;
            nudHoW20.Value = (decimal)calcOpts.HoWCD20;

            UpdatePriority(calcOpts);

            if (calcOpts.SimulateRotation) radRotSim.Checked = true;
            else radEffectiveCD.Checked = true;
            SetSimulateRotation(calcOpts.SimulateRotation);

            loading = false;
        }

        private void cmbMobType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsRetribution calcOpts = Character.CalculationOptions as CalculationOptionsRetribution;
                calcOpts.Mob = (MobType)Enum.Parse(typeof(MobType), cmbMobType.Text);
                Character.OnCalculationsInvalidated();
            }
        }

        private void cmbLength_ValueChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsRetribution calcOpts = Character.CalculationOptions as CalculationOptionsRetribution;
                calcOpts.FightLength = (float)cmbLength.Value;
                Character.OnCalculationsInvalidated();
            }
        }

        private void UpdatePriority(CalculationOptionsRetribution calcOpts)
        {
            bool wasLoading = loading;
            loading = true;

            listUnlimitedPriority.Items.Clear();
            listUnlimitedPriority.Items.AddRange(new string[] { RotationParameters.AbilityString(calcOpts.Order[0]), RotationParameters.AbilityString(calcOpts.Order[1]),
                 RotationParameters.AbilityString(calcOpts.Order[2]), RotationParameters.AbilityString(calcOpts.Order[3]),
                  RotationParameters.AbilityString(calcOpts.Order[4]), RotationParameters.AbilityString(calcOpts.Order[5])});

            for (int i = 0; i < 6; i++) listUnlimitedPriority.SetItemChecked(i, calcOpts.Selected[i]);

            loading = wasLoading;
        }

        private void butUnlimitedUp_Click(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsRetribution calcOpts = Character.CalculationOptions as CalculationOptionsRetribution;
                int sel = listUnlimitedPriority.SelectedIndex;
                if (sel > 0 && sel <= 5)
                {
                    Ability temp1 = calcOpts.Order[sel - 1];
                    calcOpts.Order[sel - 1] = calcOpts.Order[sel];
                    calcOpts.Order[sel] = temp1;

                    bool temp2 = calcOpts.Selected[sel - 1];
                    calcOpts.Selected[sel - 1] = calcOpts.Selected[sel];
                    calcOpts.Selected[sel] = temp2;

                    UpdatePriority(calcOpts);
                    listUnlimitedPriority.SelectedIndex = sel - 1;
                    Character.OnCalculationsInvalidated();
                }
            }
        }

        private void butUnlimitedDown_Click(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsRetribution calcOpts = Character.CalculationOptions as CalculationOptionsRetribution;
                int sel = listUnlimitedPriority.SelectedIndex;
                if (sel >= 0 && sel < 5)
                {
                    Ability temp1 = calcOpts.Order[sel + 1];
                    calcOpts.Order[sel + 1] = calcOpts.Order[sel];
                    calcOpts.Order[sel] = temp1;

                    bool temp2 = calcOpts.Selected[sel + 1];
                    calcOpts.Selected[sel + 1] = calcOpts.Selected[sel];
                    calcOpts.Selected[sel] = temp2;

                    UpdatePriority(calcOpts);
                    listUnlimitedPriority.SelectedIndex = sel + 1;
                    Character.OnCalculationsInvalidated();
                }
            }
        }

        private void listUnlimitedPriority_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsRetribution calcOpts = Character.CalculationOptions as CalculationOptionsRetribution;
                
                if (e.NewValue == CheckState.Unchecked) calcOpts.Selected[e.Index] = false;
                else calcOpts.Selected[e.Index] = true;

                Character.OnCalculationsInvalidated();
            }
        }

        private void nudDelay_ValueChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsRetribution calcOpts = Character.CalculationOptions as CalculationOptionsRetribution;
                calcOpts.Delay = (float)nudDelay.Value;
                Character.OnCalculationsInvalidated();
            }
        }

        private void SetSimulateRotation(bool rot)
        {
            if (rot)
            {
                listUnlimitedPriority.Enabled = true;
                lblDelay.Enabled = true;
                lblWait.Enabled = true;
                butUnlimitedDown.Enabled = true;
                butUnlimitedUp.Enabled = true;
                nudDelay.Enabled = true;
                nudWait.Enabled = true;
                nudJudge.Enabled = false;
                nudJudge20.Enabled = false;
                nudCS.Enabled = false;
                nudCS20.Enabled = false;
                nudDS.Enabled = false;
                nudDS20.Enabled = false;
                nudCons.Enabled = false;
                nudCons20.Enabled = false;
                nudExo.Enabled = false;
                nudExo20.Enabled = false;
                nudHoW20.Enabled = false;
                lblPost20.Enabled = false;
                lblPre20.Enabled = false;
                lblJudge.Enabled = false;
                lblCS.Enabled = false;
                lblDS.Enabled = false;
                lblCons.Enabled = false;
                lblExo.Enabled = false;
                lblHoW.Enabled = false;
            }
            else
            {
                listUnlimitedPriority.Enabled = false;
                lblDelay.Enabled = false;
                lblWait.Enabled = false;
                butUnlimitedDown.Enabled = false;
                butUnlimitedUp.Enabled = false;
                nudDelay.Enabled = false;
                nudWait.Enabled = false;
                nudJudge.Enabled = true;
                nudJudge20.Enabled = true;
                nudCS.Enabled = true;
                nudCS20.Enabled = true;
                nudDS.Enabled = true;
                nudDS20.Enabled = true;
                nudCons.Enabled = true;
                nudCons20.Enabled = true;
                nudExo.Enabled = true;
                nudExo20.Enabled = true;
                nudHoW20.Enabled = true;
                lblPost20.Enabled = true;
                lblPre20.Enabled = true;
                lblJudge.Enabled = true;
                lblCS.Enabled = true;
                lblDS.Enabled = true;
                lblCons.Enabled = true;
                lblExo.Enabled = true;
                lblHoW.Enabled = true;
            }
        }

        private void radRotSim_CheckedChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsRetribution calcOpts = Character.CalculationOptions as CalculationOptionsRetribution;
                SetSimulateRotation(radRotSim.Checked);
                calcOpts.SimulateRotation = radRotSim.Checked;
                Character.OnCalculationsInvalidated();
            }
        }

        private void txtJudge_TextChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsRetribution calcOpts = Character.CalculationOptions as CalculationOptionsRetribution;
                calcOpts.Delay = (float)nudDelay.Value;
                Character.OnCalculationsInvalidated();
            }
        }

        private void nudJudge_ValueChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsRetribution calcOpts = Character.CalculationOptions as CalculationOptionsRetribution;
                calcOpts.JudgeCD = (float)nudJudge.Value;
                Character.OnCalculationsInvalidated();
            }
        }

        private void nudJudge20_ValueChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsRetribution calcOpts = Character.CalculationOptions as CalculationOptionsRetribution;
                calcOpts.JudgeCD20 = (float)nudJudge20.Value;
                Character.OnCalculationsInvalidated();
            }
        }

        private void nudCS_ValueChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsRetribution calcOpts = Character.CalculationOptions as CalculationOptionsRetribution;
                calcOpts.CSCD = (float)nudCS.Value;
                Character.OnCalculationsInvalidated();
            }
        }

        private void nudCS20_ValueChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsRetribution calcOpts = Character.CalculationOptions as CalculationOptionsRetribution;
                calcOpts.CSCD20 = (float)nudCS20.Value;
                Character.OnCalculationsInvalidated();
            }
        }

        private void nudDS_ValueChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsRetribution calcOpts = Character.CalculationOptions as CalculationOptionsRetribution;
                calcOpts.DSCD = (float)nudDS.Value;
                Character.OnCalculationsInvalidated();
            }
        }

        private void nudDS20_ValueChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsRetribution calcOpts = Character.CalculationOptions as CalculationOptionsRetribution;
                calcOpts.DSCD20 = (float)nudDS20.Value;
                Character.OnCalculationsInvalidated();
            }
        }

        private void nudCons_ValueChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsRetribution calcOpts = Character.CalculationOptions as CalculationOptionsRetribution;
                calcOpts.ConsCD = (float)nudCons.Value;
                Character.OnCalculationsInvalidated();
            }
        }

        private void nudCons20_ValueChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsRetribution calcOpts = Character.CalculationOptions as CalculationOptionsRetribution;
                calcOpts.ConsCD20 = (float)nudCons20.Value;
                Character.OnCalculationsInvalidated();
            }
        }

        private void nudExo_ValueChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsRetribution calcOpts = Character.CalculationOptions as CalculationOptionsRetribution;
                calcOpts.ExoCD = (float)nudExo.Value;
                Character.OnCalculationsInvalidated();
            }
        }

        private void nudExo20_ValueChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsRetribution calcOpts = Character.CalculationOptions as CalculationOptionsRetribution;
                calcOpts.ExoCD20 = (float)nudExo20.Value;
                Character.OnCalculationsInvalidated();
            }
        }

        private void nudHoW20_ValueChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsRetribution calcOpts = Character.CalculationOptions as CalculationOptionsRetribution;
                calcOpts.HoWCD20 = (float)nudHoW20.Value;
                Character.OnCalculationsInvalidated();
            }
        }

        private void nudWait_ValueChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsRetribution calcOpts = Character.CalculationOptions as CalculationOptionsRetribution;
                calcOpts.Wait = (float)nudWait.Value;
                Character.OnCalculationsInvalidated();
            }
        }

        private void nudTargetLevel_ValueChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsRetribution calcOpts = Character.CalculationOptions as CalculationOptionsRetribution;
                calcOpts.TargetLevel = (int)nudTargetLevel.Value;
                Character.OnCalculationsInvalidated();
            }
        }

        private void cmbSeal_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsRetribution calcOpts = Character.CalculationOptions as CalculationOptionsRetribution;
                calcOpts.Seal = (SealOf)Enum.Parse(typeof(SealOf), cmbSeal.Text);
                Character.OnCalculationsInvalidated();
            }
        }

        private void nudStackTrinket_ValueChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsRetribution calcOpts = Character.CalculationOptions as CalculationOptionsRetribution;
                calcOpts.StackTrinketReset = (int)nudStackTrinket.Value;
                Character.OnCalculationsInvalidated();
            }
        }

        private void nudTargets_ValueChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsRetribution calcOpts = Character.CalculationOptions as CalculationOptionsRetribution;
                calcOpts.Targets = (float)nudTargets.Value;
                Character.OnCalculationsInvalidated();
            }
        }

        private void nudTimeUnder20_ValueChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsRetribution calcOpts = Character.CalculationOptions as CalculationOptionsRetribution;
                calcOpts.TimeUnder20 = (float)nudTimeUnder20.Value / 100f;
                Character.OnCalculationsInvalidated();
            }
        }

        private void nudInFront_ValueChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsRetribution calcOpts = Character.CalculationOptions as CalculationOptionsRetribution;
                calcOpts.InFront = (float)nudInFront.Value / 100f;
                Character.OnCalculationsInvalidated();
            }
        }

        private void nudConsEff_ValueChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsRetribution calcOpts = Character.CalculationOptions as CalculationOptionsRetribution;
                calcOpts.ConsEff = (float)nudConsEff.Value / 100f;
                Character.OnCalculationsInvalidated();
            }
        }

        private void chkBloodlust_CheckedChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsRetribution calcOpts = Character.CalculationOptions as CalculationOptionsRetribution;
                calcOpts.Bloodlust = chkBloodlust.Checked;
                Character.OnCalculationsInvalidated();
            }
        }

        private void nudHoR_ValueChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsRetribution calcOpts = Character.CalculationOptions as CalculationOptionsRetribution;
                calcOpts.HoREff = (float)nudHoR.Value / 100f;
                Character.OnCalculationsInvalidated();
            }
        }

        private void nudTargetSwitch_ValueChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsRetribution calcOpts = Character.CalculationOptions as CalculationOptionsRetribution;
                calcOpts.TargetSwitches = (float)nudTargetSwitch.Value;
                Character.OnCalculationsInvalidated();
            }
        }

        private void butClearRotate_Click(object sender, EventArgs e)
        {
            RotationSimulator.ClearCache();
            Character.OnCalculationsInvalidated();
        }

    }

}
