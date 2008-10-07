using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Rawr.Warlock
{
	public partial class CalculationOptionsPanelWarlock : CalculationOptionsPanelBase
	{
        private bool calculationSuspended = false;
        private WarlockTalentsForm talents;
        private RaidISB raidIsb;

		public CalculationOptionsPanelWarlock()
		{
			InitializeComponent();
            talents = new WarlockTalentsForm(this);
            raidIsb = new RaidISB(this);
        }

		protected override void LoadCalculationOptions()
		{
            if (Character.CalculationOptions == null)
                Character.CalculationOptions = new CalculationOptionsWarlock(Character);
			CalculationOptionsWarlock options = Character.CalculationOptions as CalculationOptionsWarlock;

            calculationSuspended = true;

            textBoxLatency.Text = options.Latency.ToString();
            comboBoxTargetLevel.SelectedItem = options.TargetLevel.ToString();
            textBoxFightDuration.Text = options.FightDuration.ToString();
            textBoxDotGap.Text = options.DotGap.ToString();
            textBoxAfflictionDebuffs.Text = options.AfflictionDebuffs.ToString();
            textBoxShadowPriestDps.Text = options.ShadowPriestDps.ToString();
            comboBoxFillerSpell.SelectedIndex = (int)options.FillerSpell;
            comboBoxCastedCurse.SelectedIndex = (int)options.CastedCurse;
            checkBoxCastImmolate.Checked = options.CastImmolate;
            checkBoxCastCorruption.Checked = options.CastCorruption;
            if (options.IsbMethod == IsbMethod.Custom)
            {
                radioButtonIsbCustom.Checked = true;
                textBoxIsbCustom.Enabled = true;
                buttonIsbRaid.Enabled = false;
            }
            else
            {
                radioButtonIsbRaid.Checked = true;
                textBoxIsbCustom.Enabled = false;
                buttonIsbRaid.Enabled = true;
            }
            textBoxIsbCustom.Text = options.CustomIsbUptime.ToString();

            checkBoxCastUnstableAffliction.Enabled = options.UnstableAffliction == 1;
            checkBoxCastUnstableAffliction.Checked = checkBoxCastUnstableAffliction.Enabled && options.CastUnstableAffliction;
            checkBoxCastSiphonLife.Enabled = options.SiphonLife == 1;
            checkBoxCastSiphonLife.Checked = checkBoxCastSiphonLife.Enabled && options.CastSiphonLife;
            checkBoxCastShadowburn.Enabled = options.Shadowburn == 1;
            checkBoxCastShadowburn.Checked = checkBoxCastShadowburn.Enabled && options.CastShadowburn;
            checkBoxCastConflagrate.Enabled = (options.Conflagrate == 1 && checkBoxCastImmolate.Checked);
            checkBoxCastConflagrate.Checked = checkBoxCastConflagrate.Enabled && options.CastConflagrate;
            if (options.SummonFelguard == 1 && !comboBoxPet.Items.Contains("Felguard"))
                comboBoxPet.Items.Add("Felguard");
            else if (options.SummonFelguard != 1 && comboBoxPet.Items.Contains("Felguard"))
                comboBoxPet.Items.Remove("Felguard");
            comboBoxPet.SelectedIndex = (int)options.Pet;
            checkBoxPetSacrificed.Enabled = options.DemonicSacrifice == 1;
            checkBoxPetSacrificed.Checked = checkBoxPetSacrificed.Enabled && options.PetSacrificed;

            if (talents != null)
                talents.LoadCalculationOptions();

            calculationSuspended = false;
        }

        public void UpdateTalentOptions()
        {
            CalculationOptionsWarlock options = Character.CalculationOptions as CalculationOptionsWarlock;

            calculationSuspended = true;

            checkBoxCastUnstableAffliction.Enabled = options.UnstableAffliction == 1;
            checkBoxCastUnstableAffliction.Checked = checkBoxCastUnstableAffliction.Enabled && options.CastUnstableAffliction;

            checkBoxCastSiphonLife.Enabled = options.SiphonLife == 1;
            checkBoxCastSiphonLife.Checked = checkBoxCastSiphonLife.Enabled && options.CastSiphonLife;

            checkBoxCastShadowburn.Enabled = options.Shadowburn == 1;
            checkBoxCastShadowburn.Checked = checkBoxCastShadowburn.Enabled && options.CastShadowburn;

            checkBoxCastConflagrate.Enabled = options.Conflagrate == 1 && checkBoxCastImmolate.Checked;
            checkBoxCastConflagrate.Checked = checkBoxCastConflagrate.Enabled && options.CastConflagrate;

            checkBoxCastCorruption.Checked = options.CastCorruption;
            checkBoxCastImmolate.Checked = options.CastImmolate;
            comboBoxFillerSpell.SelectedIndex = (int)options.FillerSpell;

            if (options.SummonFelguard == 1 && !comboBoxPet.Items.Contains("Felguard"))
                comboBoxPet.Items.Add("Felguard");
            else if (options.SummonFelguard != 1 && comboBoxPet.Items.Contains("Felguard"))
                comboBoxPet.Items.Remove("Felguard");
            comboBoxPet.SelectedIndex = (int)options.Pet;

            checkBoxPetSacrificed.Enabled = options.DemonicSacrifice == 1;
            checkBoxPetSacrificed.Checked = checkBoxPetSacrificed.Enabled && options.PetSacrificed;

            calculationSuspended = false;
            Character.OnCalculationsInvalidated();
        }

        private void buttonTalents_Click(object sender, EventArgs e)
        {
            talents.Show();
        }

        private void textBoxLatency_Leave(object sender, EventArgs e)
        {
            CalculationOptionsWarlock options = Character.CalculationOptions as CalculationOptionsWarlock;
            float value;
            if (float.TryParse(textBoxLatency.Text, out value))
            {
                options.Latency = value;
                if (!calculationSuspended) Character.OnCalculationsInvalidated();
            }
        }   
	
		private void comboBoxTargetLevel_SelectedIndexChanged(object sender, EventArgs e)
		{
			CalculationOptionsWarlock options = Character.CalculationOptions as CalculationOptionsWarlock;
            options.TargetLevel = int.Parse(comboBoxTargetLevel.SelectedItem.ToString());
            if (!calculationSuspended) Character.OnCalculationsInvalidated();
		}

        private void textBoxFightDuration_Leave(object sender, EventArgs e)
        {
            CalculationOptionsWarlock options = Character.CalculationOptions as CalculationOptionsWarlock;
            float value;
            if (float.TryParse(textBoxFightDuration.Text, out value))
            {
                options.FightDuration = value;
                if (!calculationSuspended) Character.OnCalculationsInvalidated();
            }
        }

        private void textBoxDotGap_Leave(object sender, EventArgs e)
        {
            CalculationOptionsWarlock options = Character.CalculationOptions as CalculationOptionsWarlock;
            float value;
            if (float.TryParse(textBoxDotGap.Text, out value))
            {
                options.DotGap = value;
                if (!calculationSuspended) Character.OnCalculationsInvalidated();
            }
        }

        private void textBoxAfflictionDebuffs_Leave(object sender, EventArgs e)
        {
            CalculationOptionsWarlock options = Character.CalculationOptions as CalculationOptionsWarlock;
            int value;
            if (int.TryParse(textBoxAfflictionDebuffs.Text, out value))
            {
                options.AfflictionDebuffs = value;
                if (!calculationSuspended) Character.OnCalculationsInvalidated();
            }
        }

        private void textBoxShadowPriestDps_Leave(object sender, EventArgs e)
        {
            CalculationOptionsWarlock options = Character.CalculationOptions as CalculationOptionsWarlock;
            float value;
            if (float.TryParse(textBoxShadowPriestDps.Text, out value))
            {
                options.ShadowPriestDps = value;
                if (!calculationSuspended) Character.OnCalculationsInvalidated();
            }
        }

        private void comboBoxFillerSpell_SelectedIndexChanged(object sender, EventArgs e)
        {
			CalculationOptionsWarlock options = Character.CalculationOptions as CalculationOptionsWarlock;
            options.FillerSpell = (FillerSpell)(comboBoxFillerSpell.SelectedIndex);
            if (!calculationSuspended) Character.OnCalculationsInvalidated();
        }

        private void comboBoxCastedCurse_SelectedIndexChanged(object sender, EventArgs e)
        {
			CalculationOptionsWarlock options = Character.CalculationOptions as CalculationOptionsWarlock;
            options.CastedCurse = (CastedCurse)(comboBoxCastedCurse.SelectedIndex);
            if (!calculationSuspended) Character.OnCalculationsInvalidated();
        }

        private void checkBoxCastImmolate_CheckedChanged(object sender, EventArgs e)
        {
            CalculationOptionsWarlock options = Character.CalculationOptions as CalculationOptionsWarlock;
            options.CastImmolate = checkBoxCastImmolate.Checked;
            if (!options.CastImmolate)
                checkBoxCastConflagrate.Enabled = checkBoxCastConflagrate.Checked = false;
            else if(options.Conflagrate == 1)
                checkBoxCastConflagrate.Enabled = true;
            if (!calculationSuspended) Character.OnCalculationsInvalidated();
        }

        private void checkBoxCastCorruption_CheckedChanged(object sender, EventArgs e)
        {
            CalculationOptionsWarlock options = Character.CalculationOptions as CalculationOptionsWarlock;
            options.CastCorruption = checkBoxCastCorruption.Checked;
            if (!calculationSuspended) Character.OnCalculationsInvalidated();
        }

        private void checkBoxCastUnstableAffliction_CheckedChanged(object sender, EventArgs e)
        {
            CalculationOptionsWarlock options = Character.CalculationOptions as CalculationOptionsWarlock;
            options.CastUnstableAffliction = checkBoxCastUnstableAffliction.Checked;
            if (!calculationSuspended) Character.OnCalculationsInvalidated();
        }

        private void checkBoxCastSiphonLife_CheckedChanged(object sender, EventArgs e)
        {
            CalculationOptionsWarlock options = Character.CalculationOptions as CalculationOptionsWarlock;
            options.CastSiphonLife = checkBoxCastSiphonLife.Checked;
            if (!calculationSuspended) Character.OnCalculationsInvalidated();
        }

        private void checkBoxCastShadowburn_CheckedChanged(object sender, EventArgs e)
        {
            CalculationOptionsWarlock options = Character.CalculationOptions as CalculationOptionsWarlock;
            options.CastShadowburn = checkBoxCastShadowburn.Checked;
            if (!calculationSuspended) Character.OnCalculationsInvalidated();
        }

        private void checkBoxCastConflagrate_CheckedChanged(object sender, EventArgs e)
        {
            CalculationOptionsWarlock options = Character.CalculationOptions as CalculationOptionsWarlock;
            options.CastConflagrate = checkBoxCastConflagrate.Checked;
            if (!calculationSuspended) Character.OnCalculationsInvalidated();
        }

        private void comboBoxPet_SelectedIndexChanged(object sender, EventArgs e)
        {
			CalculationOptionsWarlock options = Character.CalculationOptions as CalculationOptionsWarlock;
            options.Pet = (Pet)(comboBoxPet.SelectedIndex);
            if (!calculationSuspended) Character.OnCalculationsInvalidated();
        }

        private void checkBoxPetSacrificed_CheckedChanged(object sender, EventArgs e)
        {
			CalculationOptionsWarlock options = Character.CalculationOptions as CalculationOptionsWarlock;
            options.PetSacrificed = checkBoxPetSacrificed.Checked;
            if (!calculationSuspended) Character.OnCalculationsInvalidated();
        }

        private void radioButtonIsbCustom_CheckedChanged(object sender, EventArgs e)
        {
            CalculationOptionsWarlock options = Character.CalculationOptions as CalculationOptionsWarlock;
            if (radioButtonIsbCustom.Checked)
            {
                options.IsbMethod = IsbMethod.Custom;
                textBoxIsbCustom.Enabled = true;
            }
            else
                textBoxIsbCustom.Enabled = false;
            if (!calculationSuspended) Character.OnCalculationsInvalidated();
        }

        private void textBoxIsbCustom_Leave(object sender, EventArgs e)
        {
            CalculationOptionsWarlock options = Character.CalculationOptions as CalculationOptionsWarlock;
            float value;
            if(float.TryParse(textBoxIsbCustom.Text, out value))
            {
                options.CustomIsbUptime = value;
                if (!calculationSuspended) Character.OnCalculationsInvalidated();
            }
        }

        private void radioButtonIsbRaid_CheckedChanged(object sender, EventArgs e)
        {
            CalculationOptionsWarlock options = Character.CalculationOptions as CalculationOptionsWarlock;
            if (radioButtonIsbRaid.Checked)
            {
                options.IsbMethod = IsbMethod.Raid;
                buttonIsbRaid.Enabled = true;
            }
            else
            {
                options.IsbMethod = IsbMethod.Custom;
                buttonIsbRaid.Enabled = false;
            }
            if (!calculationSuspended) Character.OnCalculationsInvalidated();
        }

        private void buttonIsbRaid_Click(object sender, EventArgs e)
        {
            raidIsb.LoadRaid();
            if (raidIsb.ShowDialog(this) == DialogResult.OK)
                raidIsb.SaveRaid();
            if (!calculationSuspended) Character.OnCalculationsInvalidated();
        }
	}
}
