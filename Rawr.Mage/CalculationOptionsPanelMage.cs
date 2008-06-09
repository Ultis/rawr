using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Globalization;

namespace Rawr.Mage
{
	public partial class CalculationOptionsPanelMage : CalculationOptionsPanelBase
	{
		public CalculationOptionsPanelMage()
		{
			InitializeComponent();
            talents = new MageTalentsForm(this);
		}

        private MageTalentsForm talents;
        private bool loading = false;

        private Character character;

		protected override void LoadCalculationOptions()
		{
            if (character != null) character.ItemsChanged -= new EventHandler(character_ItemsChanged);
            character = Character;
            if (character != null) character.ItemsChanged += new EventHandler(character_ItemsChanged);

            if (Character.CalculationOptions == null) Character.CalculationOptions = new CalculationOptionsMage(character);
            CalculationOptionsMage calculationOptions = Character.CalculationOptions as CalculationOptionsMage;

            loading = true;

			comboBoxTargetLevel.SelectedItem = calculationOptions.TargetLevel.ToString();
            comboBoxAoeTargetLevel.SelectedItem = calculationOptions.AoeTargetLevel.ToString();
            checkBoxEnforceMetagemRequirements.Checked = Character.EnforceMetagemRequirements;
            textBoxLatency.Text = calculationOptions.Latency.ToString();
            textBoxAoeTargets.Text = calculationOptions.AoeTargets.ToString();
            textBoxArcaneResist.Text = calculationOptions.ArcaneResist.ToString();
            textBoxFireResist.Text = calculationOptions.FireResist.ToString();
            textBoxFrostResist.Text = calculationOptions.FrostResist.ToString();
            textBoxNatureResist.Text = calculationOptions.NatureResist.ToString();
            textBoxShadowResist.Text = calculationOptions.ShadowResist.ToString();
            textBoxFightDuration.Text = calculationOptions.FightDuration.ToString();
            textBoxShadowPriest.Text = calculationOptions.ShadowPriest.ToString();
            checkBoxHeroism.Checked = calculationOptions.HeroismAvailable;
            textBoxMoltenFuryPercentage.Text = calculationOptions.MoltenFuryPercentage.ToString();
            checkBoxDestructionPotion.Checked = calculationOptions.DestructionPotion;
            checkBoxFlameCap.Checked = calculationOptions.FlameCap;
            checkBoxABCycles.Checked = calculationOptions.ABCycles;
            textBoxDpsTime.Text = calculationOptions.DpsTime.ToString();
            checkBoxMaintainScorch.Checked = calculationOptions.MaintainScorch;
            textBoxInterruptFrequency.Text = calculationOptions.InterruptFrequency.ToString();
            textBoxEvocationWeapon.Text = calculationOptions.EvocationWeapon.ToString();
            textBoxAoeDuration.Text = calculationOptions.AoeDuration.ToString();
            checkBoxSmartOptimization.Checked = calculationOptions.SmartOptimization;
            checkBoxDrumsOfBattle.Checked = calculationOptions.DrumsOfBattle;
            checkBoxAutomaticArmor.Checked = calculationOptions.AutomaticArmor;
            textBoxTpsLimit.Text = calculationOptions.TpsLimit.ToString();
            checkBoxIncrementalOptimizations.Checked = calculationOptions.IncrementalOptimizations;
            checkBoxReconstructSequence.Checked = calculationOptions.ReconstructSequence;
            textBoxInnervate.Text = calculationOptions.Innervate.ToString();
            textBoxManaTide.Text = calculationOptions.ManaTide.ToString();
            textBoxFragmentation.Text = calculationOptions.Fragmentation.ToString();
            checkBoxSMP.Checked = calculationOptions.SMP;
            checkBoxSMPDisplay.Checked = calculationOptions.SMPDisplay;
            textBoxEvocationSpirit.Text = calculationOptions.EvocationSpirit.ToString();
            textBoxSurvivabilityRating.Text = calculationOptions.SurvivabilityRating.ToString();
            comboBoxFaction.SelectedItem = calculationOptions.Aldor ? "Aldor" : "Scryers";
            checkBoxWotLK.Checked = calculationOptions.WotLK;
            comboBoxHeroismControl.SelectedIndex = calculationOptions.HeroismControl;
            textBoxSMPComputationLimit.Text = calculationOptions.MaxHeapLimit.ToString();
            checkBoxAverageCooldowns.Checked = calculationOptions.AverageCooldowns;
            checkBoxEvocationEnabled.Checked = calculationOptions.EvocationEnabled;
            checkBoxManaPotionEnabled.Checked = calculationOptions.ManaPotionEnabled;
            checkBoxManaGemEnabled.Checked = calculationOptions.ManaGemEnabled;
            checkBoxDisableCooldowns.Checked = calculationOptions.DisableCooldowns;

            if (talents != null) talents.LoadCalculationOptions();

            loading = false;
        }

        void character_ItemsChanged(object sender, EventArgs e)
        {
            CalculationOptionsMage calculationOptions = Character.CalculationOptions as CalculationOptionsMage;
            if (calculationOptions != null && calculationOptions.IncrementalOptimizations)
            {
                // compute restricted stacking & spell combinations for incremental optimizations
                ((CalculationsMage)Calculations.Instance).GetCharacterCalculations(Character, null, true);
            }
        }
	
		private void comboBoxTargetLevel_SelectedIndexChanged(object sender, EventArgs e)
		{
            CalculationOptionsMage calculationOptions = Character.CalculationOptions as CalculationOptionsMage;
            calculationOptions.TargetLevel = int.Parse(comboBoxTargetLevel.SelectedItem.ToString());
			if (!loading) Character.OnItemsChanged();
		}

		private void checkBoxEnforceMetagemRequirements_CheckedChanged(object sender, EventArgs e)
		{
			Character.EnforceMetagemRequirements = checkBoxEnforceMetagemRequirements.Checked;
            if (!loading) Character.OnItemsChanged();
        }

        private void comboBoxAoeTargetLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            CalculationOptionsMage calculationOptions = Character.CalculationOptions as CalculationOptionsMage;
            calculationOptions.AoeTargetLevel = int.Parse(comboBoxAoeTargetLevel.SelectedItem.ToString());
            if (!loading) Character.OnItemsChanged();
        }

        private void textBoxLatency_Validated(object sender, EventArgs e)
        {
            CalculationOptionsMage calculationOptions = Character.CalculationOptions as CalculationOptionsMage;
            float value;
            if (float.TryParse(textBoxLatency.Text, out value))
            {
                calculationOptions.Latency = value;
                if (!loading) Character.OnItemsChanged();
            }
        }

        private void buttonTalents_Click(object sender, EventArgs e)
        {
            talents.Show();
        }

        private void textBoxAoeTargets_Validated(object sender, EventArgs e)
        {
            CalculationOptionsMage calculationOptions = Character.CalculationOptions as CalculationOptionsMage;
            int value;
            if (int.TryParse(textBoxAoeTargets.Text, out value))
            {
                calculationOptions.AoeTargets = value;
                if (!loading) Character.OnItemsChanged();
            }
        }

        private void textBoxArcaneResist_Validated(object sender, EventArgs e)
        {
            CalculationOptionsMage calculationOptions = Character.CalculationOptions as CalculationOptionsMage;
            float value;
            if (float.TryParse(textBoxArcaneResist.Text, out value))
            {
                calculationOptions.ArcaneResist = value;
                if (!loading) Character.OnItemsChanged();
            }
        }

        private void textBoxFireResist_Validated(object sender, EventArgs e)
        {
            CalculationOptionsMage calculationOptions = Character.CalculationOptions as CalculationOptionsMage;
            float value;
            if (float.TryParse(textBoxFireResist.Text, out value))
            {
                calculationOptions.FireResist = value;
                if (!loading) Character.OnItemsChanged();
            }
        }

        private void textBoxFrostResist_Validated(object sender, EventArgs e)
        {
            CalculationOptionsMage calculationOptions = Character.CalculationOptions as CalculationOptionsMage;
            float value;
            if (float.TryParse(textBoxFrostResist.Text, out value))
            {
                calculationOptions.FrostResist = value;
                if (!loading) Character.OnItemsChanged();
            }
        }

        private void textBoxNatureResist_Validated(object sender, EventArgs e)
        {
            CalculationOptionsMage calculationOptions = Character.CalculationOptions as CalculationOptionsMage;
            float value;
            if (float.TryParse(textBoxNatureResist.Text, out value))
            {
                calculationOptions.NatureResist = value;
                if (!loading) Character.OnItemsChanged();
            }
        }

        private void textBoxFightDuration_Validated(object sender, EventArgs e)
        {
            CalculationOptionsMage calculationOptions = Character.CalculationOptions as CalculationOptionsMage;
            float value;
            if (float.TryParse(textBoxFightDuration.Text, out value))
            {
                calculationOptions.FightDuration = value;
                if (!loading) Character.OnItemsChanged();
            }
        }

        private void textBoxShadowPriest_Validated(object sender, EventArgs e)
        {
            CalculationOptionsMage calculationOptions = Character.CalculationOptions as CalculationOptionsMage;
            float value;
            if (float.TryParse(textBoxShadowPriest.Text, out value))
            {
                calculationOptions.ShadowPriest = value;
                if (!loading) Character.OnItemsChanged();
            }
        }

        private void checkBoxHeroism_CheckedChanged(object sender, EventArgs e)
        {
            CalculationOptionsMage calculationOptions = Character.CalculationOptions as CalculationOptionsMage;
            calculationOptions.HeroismAvailable = checkBoxHeroism.Checked;
            if (!loading) Character.OnItemsChanged();
        }

        private void textBoxMoltenFuryPercentage_Validated(object sender, EventArgs e)
        {
            CalculationOptionsMage calculationOptions = Character.CalculationOptions as CalculationOptionsMage;
            float value;
            if (float.TryParse(textBoxMoltenFuryPercentage.Text, out value))
            {
                calculationOptions.MoltenFuryPercentage = value;
                if (!loading) Character.OnItemsChanged();
            }
        }

        private void checkBoxDestructionPotion_CheckedChanged(object sender, EventArgs e)
        {
            CalculationOptionsMage calculationOptions = Character.CalculationOptions as CalculationOptionsMage;
            calculationOptions.DestructionPotion = checkBoxDestructionPotion.Checked;
            if (!loading) Character.OnItemsChanged();
        }

        private void checkBoxFlameCap_CheckedChanged(object sender, EventArgs e)
        {
            CalculationOptionsMage calculationOptions = Character.CalculationOptions as CalculationOptionsMage;
            calculationOptions.FlameCap = checkBoxFlameCap.Checked;
            if (!loading) Character.OnItemsChanged();
        }

        private void checkBoxABCycles_CheckedChanged(object sender, EventArgs e)
        {
            CalculationOptionsMage calculationOptions = Character.CalculationOptions as CalculationOptionsMage;
            calculationOptions.ABCycles = checkBoxABCycles.Checked;
            if (!loading) Character.OnItemsChanged();
        }

        private void textBoxDpsTime_Validated(object sender, EventArgs e)
        {
            CalculationOptionsMage calculationOptions = Character.CalculationOptions as CalculationOptionsMage;
            float value;
            if (float.TryParse(textBoxDpsTime.Text, out value))
            {
                calculationOptions.DpsTime = value;
                if (!loading) Character.OnItemsChanged();
            }
        }

        private void checkBoxMaintainScorch_CheckedChanged(object sender, EventArgs e)
        {
            CalculationOptionsMage calculationOptions = Character.CalculationOptions as CalculationOptionsMage;
            calculationOptions.MaintainScorch = checkBoxMaintainScorch.Checked;
            if (!loading) Character.OnItemsChanged();
        }

        private void textBoxInterruptFrequency_Validated(object sender, EventArgs e)
        {
            CalculationOptionsMage calculationOptions = Character.CalculationOptions as CalculationOptionsMage;
            float value;
            if (float.TryParse(textBoxInterruptFrequency.Text, out value))
            {
                calculationOptions.InterruptFrequency = value;
                if (!loading) Character.OnItemsChanged();
            }
        }

        private void textBoxShadowResist_Validated(object sender, EventArgs e)
        {
            CalculationOptionsMage calculationOptions = Character.CalculationOptions as CalculationOptionsMage;
            float value;
            if (float.TryParse(textBoxShadowResist.Text, out value))
            {
                calculationOptions.ShadowResist = value;
                if (!loading) Character.OnItemsChanged();
            }
        }

        private void textBoxEvocationWeapon_Validated(object sender, EventArgs e)
        {
            CalculationOptionsMage calculationOptions = Character.CalculationOptions as CalculationOptionsMage;
            int value;
            if (int.TryParse(textBoxEvocationWeapon.Text, out value))
            {
                calculationOptions.EvocationWeapon = value;
                if (!loading) Character.OnItemsChanged();
            }
        }

        private void textBoxAoeDuration_Validated(object sender, EventArgs e)
        {
            CalculationOptionsMage calculationOptions = Character.CalculationOptions as CalculationOptionsMage;
            float value;
            if (float.TryParse(textBoxAoeDuration.Text, out value))
            {
                calculationOptions.AoeDuration = value;
                if (!loading) Character.OnItemsChanged();
            }
        }

        private void checkBoxSmartOptimization_CheckedChanged(object sender, EventArgs e)
        {
            CalculationOptionsMage calculationOptions = Character.CalculationOptions as CalculationOptionsMage;
            calculationOptions.SmartOptimization = checkBoxSmartOptimization.Checked;
            if (!loading) Character.OnItemsChanged();
        }

        private void checkBoxDrumsOfBattle_CheckedChanged(object sender, EventArgs e)
        {
            CalculationOptionsMage calculationOptions = Character.CalculationOptions as CalculationOptionsMage;
            calculationOptions.DrumsOfBattle = checkBoxDrumsOfBattle.Checked;
            if (!loading) Character.OnItemsChanged();
        }

        private void checkBoxAutomaticArmor_CheckedChanged(object sender, EventArgs e)
        {
            CalculationOptionsMage calculationOptions = Character.CalculationOptions as CalculationOptionsMage;
            calculationOptions.AutomaticArmor = checkBoxAutomaticArmor.Checked;
            if (!loading) Character.OnItemsChanged();
        }

        private void textBoxTpsLimit_Validated(object sender, EventArgs e)
        {
            CalculationOptionsMage calculationOptions = Character.CalculationOptions as CalculationOptionsMage;
            float value;
            if (float.TryParse(textBoxTpsLimit.Text, out value))
            {
                calculationOptions.TpsLimit = value;
                if (!loading) Character.OnItemsChanged();
            }
        }

        private void checkBoxIncrementalOptimizations_CheckedChanged(object sender, EventArgs e)
        {
            CalculationOptionsMage calculationOptions = Character.CalculationOptions as CalculationOptionsMage;
            calculationOptions.IncrementalOptimizations = checkBoxIncrementalOptimizations.Checked;
            if (!loading) Character.OnItemsChanged();
        }

        private void checkBoxReconstructSequence_CheckedChanged(object sender, EventArgs e)
        {
            DisplaySMPWarning();
            CalculationOptionsMage calculationOptions = Character.CalculationOptions as CalculationOptionsMage;
            calculationOptions.ReconstructSequence = checkBoxReconstructSequence.Checked;
            if (!loading) Character.OnItemsChanged();
        }

        private void textBoxInnervate_Validated(object sender, EventArgs e)
        {
            CalculationOptionsMage calculationOptions = Character.CalculationOptions as CalculationOptionsMage;
            float value;
            if (float.TryParse(textBoxInnervate.Text, out value))
            {
                calculationOptions.Innervate = value;
                if (!loading) Character.OnItemsChanged();
            }
        }

        private void textBoxManaTide_Validated(object sender, EventArgs e)
        {
            CalculationOptionsMage calculationOptions = Character.CalculationOptions as CalculationOptionsMage;
            float value;
            if (float.TryParse(textBoxManaTide.Text, out value))
            {
                calculationOptions.ManaTide = value;
                if (!loading) Character.OnItemsChanged();
            }
        }

        private void textBoxFragmentation_Validated(object sender, EventArgs e)
        {
            CalculationOptionsMage calculationOptions = Character.CalculationOptions as CalculationOptionsMage;
            float value;
            if (float.TryParse(textBoxFragmentation.Text, out value))
            {
                calculationOptions.Fragmentation = value;
                if (!loading) Character.OnItemsChanged();
            }
        }

        private void checkBoxSMP_CheckedChanged(object sender, EventArgs e)
        {
            DisplaySMPWarning();
            CalculationOptionsMage calculationOptions = Character.CalculationOptions as CalculationOptionsMage;
            calculationOptions.SMP = checkBoxSMP.Checked;
            if (!loading) Character.OnItemsChanged();
        }

        private void checkBoxSMPDisplay_CheckedChanged(object sender, EventArgs e)
        {
            DisplaySMPWarning();
            CalculationOptionsMage calculationOptions = Character.CalculationOptions as CalculationOptionsMage;
            calculationOptions.SMPDisplay = checkBoxSMPDisplay.Checked;
            if (!loading) Character.OnItemsChanged();
        }

        private void textBoxEvocationSpirit_Validated(object sender, EventArgs e)
        {
            CalculationOptionsMage calculationOptions = Character.CalculationOptions as CalculationOptionsMage;
            float value;
            if (float.TryParse(textBoxEvocationSpirit.Text, out value))
            {
                calculationOptions.EvocationSpirit = value;
                if (!loading) Character.OnItemsChanged();
            }
        }

        private void textBoxSurvivabilityRating_Validated(object sender, EventArgs e)
        {
            CalculationOptionsMage calculationOptions = Character.CalculationOptions as CalculationOptionsMage;
            float value;
            if (float.TryParse(textBoxSurvivabilityRating.Text, out value))
            {
                calculationOptions.SurvivabilityRating = value;
                if (!loading) Character.OnItemsChanged();
            }
        }

        private void DisplaySMPWarning()
        {
            if (!Properties.Settings.Default.DisplayedSMPWarning)
            {
                MessageBox.Show("Rawr detected that this is the first time you are using Segmented Multi-Pass (SMP) algorithm or Sequence Reconstruction." + Environment.NewLine + Environment.NewLine + "Sequence Reconstruction will perform best in combination with SMP algorithm. Since SMP algorithm can be computationally very expensive it is recommended to use SMP for display only and not for item comparisons. In some situations with many available cooldowns the SMP algorithm might have difficulties finding a solution. In such a case it will indicate it exceeded its computation limit and display the last working solution. Sequence Reconstruction will attempt to convert the optimum spell cycles into a timed sequence, but at the moment it will not consider timing dependencies between mana consumables and cooldowns, which means that in certain situations the reconstructions provided might be of low quality.", "Rawr.Mage", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Properties.Settings.Default.DisplayedSMPWarning = true;
                Properties.Settings.Default.Save();
            }
        }

        private void comboBoxFaction_SelectedIndexChanged(object sender, EventArgs e)
        {
            CalculationOptionsMage calculationOptions = Character.CalculationOptions as CalculationOptionsMage;
            calculationOptions.Aldor = comboBoxFaction.SelectedItem.ToString() == "Aldor";
            if (!loading) Character.OnItemsChanged();
        }

        private void checkBoxWotLK_CheckedChanged(object sender, EventArgs e)
        {
            CalculationOptionsMage calculationOptions = Character.CalculationOptions as CalculationOptionsMage;
            calculationOptions.WotLK = checkBoxWotLK.Checked;
            if (talents != null) talents.UpdateWotLK();
            if (!loading) Character.OnItemsChanged();
        }

        private void comboBoxHeroismControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            CalculationOptionsMage calculationOptions = Character.CalculationOptions as CalculationOptionsMage;
            calculationOptions.HeroismControl = comboBoxHeroismControl.SelectedIndex;
            if (!loading) Character.OnItemsChanged();
        }

        private void textBoxSMPComputationLimit_Validated(object sender, EventArgs e)
        {
            CalculationOptionsMage calculationOptions = Character.CalculationOptions as CalculationOptionsMage;
            int value;
            if (int.TryParse(textBoxSMPComputationLimit.Text, out value))
            {
                calculationOptions.MaxHeapLimit = value;
                if (!loading) Character.OnItemsChanged();
            }
        }

        private void checkBoxAverageCooldowns_CheckedChanged(object sender, EventArgs e)
        {
            CalculationOptionsMage calculationOptions = Character.CalculationOptions as CalculationOptionsMage;
            calculationOptions.AverageCooldowns = checkBoxAverageCooldowns.Checked;
            if (!loading) Character.OnItemsChanged();
        }

        private void checkBoxEvocationEnabled_CheckedChanged(object sender, EventArgs e)
        {
            CalculationOptionsMage calculationOptions = Character.CalculationOptions as CalculationOptionsMage;
            calculationOptions.EvocationEnabled = checkBoxEvocationEnabled.Checked;
            if (!loading) Character.OnItemsChanged();
        }

        private void checkBoxManaPotionEnabled_CheckedChanged(object sender, EventArgs e)
        {
            CalculationOptionsMage calculationOptions = Character.CalculationOptions as CalculationOptionsMage;
            calculationOptions.ManaPotionEnabled = checkBoxManaPotionEnabled.Checked;
            if (!loading) Character.OnItemsChanged();
        }

        private void checkBoxManaGemEnabled_CheckedChanged(object sender, EventArgs e)
        {
            CalculationOptionsMage calculationOptions = Character.CalculationOptions as CalculationOptionsMage;
            calculationOptions.ManaGemEnabled = checkBoxManaGemEnabled.Checked;
            if (!loading) Character.OnItemsChanged();
        }

        private void checkBoxDisableCooldowns_CheckedChanged(object sender, EventArgs e)
        {
            CalculationOptionsMage calculationOptions = Character.CalculationOptions as CalculationOptionsMage;
            calculationOptions.DisableCooldowns = checkBoxDisableCooldowns.Checked;
            if (!loading) Character.OnItemsChanged();
        }
	}
}
