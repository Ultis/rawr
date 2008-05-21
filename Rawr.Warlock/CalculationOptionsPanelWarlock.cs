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
		public CalculationOptionsPanelWarlock()
		{
			InitializeComponent();
        }

		protected override void LoadCalculationOptions()
		{
			if (Character.CalculationOptions == null)
				Character.CalculationOptions = new CalculationOptionsWarlock();

			CalculationOptionsWarlock options = Character.CalculationOptions as CalculationOptionsWarlock;

            checkBoxEnforceMetagemRequirements.Checked = options.EnforceMetagemRequirements;
            textBoxLatency.Text = options.Latency.ToString();
            comboBoxTargetLevel.SelectedItem = options.TargetLevel.ToString();
            textBoxFightDuration.Text = options.FightDuration.ToString();
            comboBoxFillerSpell.SelectedIndex = (int)options.FillerSpell;
            comboBoxCastedCurse.SelectedIndex = (int)options.CastedCurse;
            checkBoxCastImmolate.Checked = options.CastImmolate;
            checkBoxCastCorruption.Checked = options.CastCorruption;
            checkBoxCastUnstableAffliction.Checked = options.CastUnstableAffliction;
            checkBoxCastSiphonLife.Checked = options.CastSiphonLife;
            comboBoxPet.SelectedIndex = (int)options.Pet;
            checkBoxPetSacrificed.Checked = options.PetSacrificed;
            textBoxDotGap.Text = options.DotGap.ToString();
            textBoxAfflictionDebuffs.Text = options.AfflictionDebuffs.ToString();
        }

        private void buttonTalents_Click(object sender, EventArgs e)
        {
            if (Character.Talents != null)
            {
                TalentForm tf = new TalentForm();
                tf.SetParameters(Character.Talents, Character.Class);
                tf.Show();
            }
            else
            {
                MessageBox.Show("No talents found");
            }
        }

        private void checkBoxEnforceMetagemRequirements_CheckedChanged(object sender, EventArgs e)
        {
            CalculationOptionsWarlock calcOpts = Character.CalculationOptions as CalculationOptionsWarlock;
            Character.EnforceMetagemRequirements = checkBoxEnforceMetagemRequirements.Checked;
            Character.OnItemsChanged();
        }

        private void textBoxLatency_Leave(object sender, EventArgs e)
        {
            CalculationOptionsWarlock options = Character.CalculationOptions as CalculationOptionsWarlock;
            float value;
            if (float.TryParse(textBoxLatency.Text, out value))
            {
                options.Latency = value;
                Character.OnItemsChanged();
            }
        }   
	
		private void comboBoxTargetLevel_SelectedIndexChanged(object sender, EventArgs e)
		{
			CalculationOptionsWarlock options = Character.CalculationOptions as CalculationOptionsWarlock;
            options.TargetLevel = int.Parse(comboBoxTargetLevel.SelectedItem.ToString());
			Character.OnItemsChanged();
		}

        private void textBoxFightDuration_Leave(object sender, EventArgs e)
        {
            CalculationOptionsWarlock options = Character.CalculationOptions as CalculationOptionsWarlock;
            float value;
            if (float.TryParse(textBoxLatency.Text, out value))
            {
                options.FightDuration = value;
                Character.OnItemsChanged();
            }
        }

        private void comboBoxFillerSpell_SelectedIndexChanged(object sender, EventArgs e)
        {
			CalculationOptionsWarlock options = Character.CalculationOptions as CalculationOptionsWarlock;
            options.FillerSpell = (FillerSpell)(comboBoxFillerSpell.SelectedIndex);
            Character.OnItemsChanged();
        }

        private void comboBoxCastedCurse_SelectedIndexChanged(object sender, EventArgs e)
        {
			CalculationOptionsWarlock options = Character.CalculationOptions as CalculationOptionsWarlock;
            options.CastedCurse = (CastedCurse)(comboBoxCastedCurse.SelectedIndex);
            Character.OnItemsChanged();
        }

        private void checkBoxCastImmolate_CheckedChanged(object sender, EventArgs e)
        {
            CalculationOptionsWarlock options = Character.CalculationOptions as CalculationOptionsWarlock;
            options.CastImmolate = checkBoxCastImmolate.Checked;
            Character.OnItemsChanged();
        }

        private void checkBoxCastCorruption_CheckedChanged(object sender, EventArgs e)
        {
            CalculationOptionsWarlock options = Character.CalculationOptions as CalculationOptionsWarlock;
            options.CastCorruption = checkBoxCastCorruption.Checked;
            Character.OnItemsChanged();
        }

        private void checkBoxCastUnstableAffliction_CheckedChanged(object sender, EventArgs e)
        {
            CalculationOptionsWarlock options = Character.CalculationOptions as CalculationOptionsWarlock;
            options.CastUnstableAffliction = checkBoxCastUnstableAffliction.Checked;
            Character.OnItemsChanged();
        }

        private void checkBoxCastSiphonLife_CheckedChanged(object sender, EventArgs e)
        {
            CalculationOptionsWarlock options = Character.CalculationOptions as CalculationOptionsWarlock;
            options.CastSiphonLife = checkBoxCastSiphonLife.Checked;
            Character.OnItemsChanged();
        }   

        private void comboBoxPet_SelectedIndexChanged(object sender, EventArgs e)
        {
			CalculationOptionsWarlock options = Character.CalculationOptions as CalculationOptionsWarlock;
            options.Pet = (Pet)(comboBoxPet.SelectedIndex);
            Character.OnItemsChanged();
        }

        private void checkBoxPetSacrificed_CheckedChanged(object sender, EventArgs e)
        {
			CalculationOptionsWarlock options = Character.CalculationOptions as CalculationOptionsWarlock;
            options.PetSacrificed = checkBoxPetSacrificed.Checked;
            Character.OnItemsChanged();
        }
	}

}
