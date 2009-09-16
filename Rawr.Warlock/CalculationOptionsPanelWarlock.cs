using System;
using System.Collections.Generic;
using System.Drawing;
using System.Data;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using System.Windows.Forms;
using System.Globalization;

namespace Rawr.Warlock 
{
    public partial class CalculationOptionsPanelWarlock : CalculationOptionsPanelBase 
    {
        public CalculationOptionsPanelWarlock() 
        {
            InitializeComponent();
        }
        
        private bool loading;
        protected override void LoadCalculationOptions() 
        {
            loading = true;
            if (Character.CalculationOptions == null) { Character.CalculationOptions = new CalculationOptionsWarlock(); }

            CalculationOptionsWarlock calcOpts = Character.CalculationOptions as CalculationOptionsWarlock;

            // Adding this switch to handle target level transition from relative to actual
            switch(calcOpts.TargetLevel)
            {
                case 0:case 1:case 2:case 3: { calcOpts.TargetLevel += 80; break; }
                default: { break; } // Do nothing if it's already transitioned
            }

            cbTargetLevel.Text = calcOpts.TargetLevel.ToString(CultureInfo.InvariantCulture);

            trkFightLength.Value = (int)calcOpts.FightLength;
            lblFightLength.Text = trkFightLength.Value + " minute fight.";

            trkDelay.Value = (int)calcOpts.Delay;
            lblDelay.Text = trkDelay.Value + "ms Game/Brain Latency";

            trkReplenishment.Value = (int)calcOpts.Replenishment;
            lblReplenishment.Text = trkReplenishment.Value + "% effect from Replenishment.";

            trkJoW.Value = (int)calcOpts.JoW;
            lblJoW.Text = trkJoW.Value + "% effect from JoW.";

            cbManaAmt.SelectedIndex = calcOpts.ManaPot;

            if (calcOpts.SpellPriority == null) { calcOpts.SpellPriority = new List<string>() { "Shadow Bolt" }; }
            lsSpellPriority.Items.Clear();
            lsSpellPriority.Items.AddRange(calcOpts.SpellPriority.ToArray());

            tbAffEffects.Text = calcOpts.AffEffectsNumber.ToString(CultureInfo.InvariantCulture);

            cbPet.SelectedItem = calcOpts.Pet;

            chbUseInfernal.Checked = calcOpts.UseInfernal;
            chbImmoAura.Checked = calcOpts.UseImmoAura;

            chbDecimation.Checked = calcOpts.UseDecimation;
            trk35Health.Value = (int)calcOpts.Health35Perc;
            lbl35Health.Text = trk35Health.Value + "% of Time Boss < 35% Health."; 

            loading = false;
        }

        private void cmbManaAmt_SelectedIndexChanged(object sender, EventArgs e) 
        {
            if (!loading) {
                CalculationOptionsWarlock calcOpts = Character.CalculationOptions as CalculationOptionsWarlock;
                calcOpts.ManaPot = cbManaAmt.SelectedIndex;
                Character.OnCalculationsInvalidated();
            }
        }

        private void bChangePriority_Click(object sender, EventArgs e) 
        {
            CalculationOptionsWarlock calcOpts = Character.CalculationOptions as CalculationOptionsWarlock;
            SpellPriorityForm priority = new SpellPriorityForm(calcOpts.SpellPriority, lsSpellPriority, Character);
            priority.ShowDialog();
        }

        private void cbTargetLevel_SelectedIndexChanged(object sender, EventArgs e) 
        {
            if (!loading) {
                CalculationOptionsWarlock calcOpts = Character.CalculationOptions as CalculationOptionsWarlock;
                calcOpts.TargetLevel = int.Parse(cbTargetLevel.Text, CultureInfo.InvariantCulture);
                Character.OnCalculationsInvalidated();
            }
        }

        private void trkFightLength_Scroll(object sender, EventArgs e) 
        {
            if (!loading)
            {
                CalculationOptionsWarlock calcOpts = Character.CalculationOptions as CalculationOptionsWarlock;
                calcOpts.FightLength = trkFightLength.Value;
                lblFightLength.Text = trkFightLength.Value + " minute fight.";
                Character.OnCalculationsInvalidated();
            }
        }

        private void trkDelay_Scroll(object sender, EventArgs e) 
        {
            if (!loading)
            {
                CalculationOptionsWarlock calcOpts = Character.CalculationOptions as CalculationOptionsWarlock;
                calcOpts.Delay = trkDelay.Value;
                lblDelay.Text = trkDelay.Value + "ms Game/Brain Latency";
                Character.OnCalculationsInvalidated();
            }
        }

        private void trkReplenishment_Scroll(object sender, EventArgs e) 
        {
            if (!loading)
            {
                CalculationOptionsWarlock calcOpts = Character.CalculationOptions as CalculationOptionsWarlock;
                calcOpts.Replenishment = trkReplenishment.Value;
                lblReplenishment.Text = trkReplenishment.Value + "% effect from Replenishment.";
                Character.OnCalculationsInvalidated();
            }
        }

        private void trkJoW_Scroll(object sender, EventArgs e) 
        {
            if (!loading)
            {
                CalculationOptionsWarlock calcOpts = Character.CalculationOptions as CalculationOptionsWarlock;
                calcOpts.JoW = trkJoW.Value;
                lblJoW.Text = trkJoW.Value + "% effect from JoW.";
                Character.OnCalculationsInvalidated();
            }
        }

        private void cbPet_SelectedIndexChanged(object sender, EventArgs e) 
        {
            if (!loading) {
                CalculationOptionsWarlock calcOpts = Character.CalculationOptions as CalculationOptionsWarlock;
                calcOpts.Pet = (String)cbPet.SelectedItem;
                Character.OnCalculationsInvalidated();
            }
        }

        private void chbUseInfernal_CheckedChanged(object sender, EventArgs e) 
        {
            if (!loading) {
                CalculationOptionsWarlock calcOpts = Character.CalculationOptions as CalculationOptionsWarlock;
                calcOpts.UseInfernal = chbUseInfernal.Checked;
                Character.OnCalculationsInvalidated();
            }
        }

        private void tbAffEffects_Changed(object sender, EventArgs e) 
        {
            if (!loading) {
                CalculationOptionsWarlock calcOpts = Character.CalculationOptions as CalculationOptionsWarlock;
                calcOpts.AffEffectsNumber = System.Convert.ToInt32(tbAffEffects.Text, CultureInfo.InvariantCulture);
                Character.OnCalculationsInvalidated();
            }
        }

        private void tbAffEffects_KeyPress(object sender, KeyPressEventArgs e) 
        {
            if (!loading) {
                if (char.IsNumber(e.KeyChar) == false && char.IsControl(e.KeyChar) == false) {
                    e.Handled = true;
                }
            }
        }

        private void chbImmoAura_CheckedChanged(object sender, EventArgs e) 
        {
            if (!loading) {
                CalculationOptionsWarlock calcOpts = Character.CalculationOptions as CalculationOptionsWarlock;
                calcOpts.UseImmoAura = chbImmoAura.Checked;
                Character.OnCalculationsInvalidated();
            }
        }

        private void textEvents_DoubleClick(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsWarlock calcOpts = Character.CalculationOptions as CalculationOptionsWarlock;
                textEvents.Text = calcOpts.castseq;
            }
        }

        private void trk35Health_Scroll(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsWarlock calcOpts = Character.CalculationOptions as CalculationOptionsWarlock;
                calcOpts.Health35Perc = trk35Health.Value;
                lbl35Health.Text = trk35Health.Value + "% of Time Boss < 35% Health.";
                Character.OnCalculationsInvalidated();
            }
        }

        private void chbDecimation_CheckedChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsWarlock calcOpts = Character.CalculationOptions as CalculationOptionsWarlock;
                calcOpts.UseDecimation = chbDecimation.Checked;
                Character.OnCalculationsInvalidated();
            }
        }
    }
}