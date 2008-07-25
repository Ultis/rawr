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

		protected override void LoadCalculationOptions()
		{
            if (Character.CalculationOptions == null) Character.CalculationOptions = new CalculationOptionsMage(Character);
            CalculationOptionsMage calculationOptions = Character.CalculationOptions as CalculationOptionsMage;

            loading = true;
            calculationOptionsMageBindingSource.DataSource = calculationOptions;
            checkBoxEnforceMetagemRequirements.Checked = Character.EnforceMetagemRequirements;

            if (talents != null) talents.LoadCalculationOptions();

            loading = false;
        }

		private void checkBoxEnforceMetagemRequirements_CheckedChanged(object sender, EventArgs e)
		{
			Character.EnforceMetagemRequirements = checkBoxEnforceMetagemRequirements.Checked;
            if (!loading) Character.OnItemsChanged();
        }

        private void buttonTalents_Click(object sender, EventArgs e)
        {
            talents.Show();
        }

        private void checkBoxSMP_CheckedChanged(object sender, EventArgs e)
        {
            //DisplaySMPWarning();
        }

        private void checkBoxSMPDisplay_CheckedChanged(object sender, EventArgs e)
        {
            //DisplaySMPWarning();
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

        private void checkBoxWotLK_CheckedChanged(object sender, EventArgs e)
        {
            if (talents != null) talents.UpdateWotLK();
        }

        private void calculationOptionsMageBindingSource_CurrentItemChanged(object sender, EventArgs e)
        {
            if (!loading) Character.OnItemsChanged();
        }

        private void buttonCustomSpellMix_Click(object sender, EventArgs e)
        {
            CalculationOptionsMage calculationOptions = Character.CalculationOptions as CalculationOptionsMage;
            if (calculationOptions.CustomSpellMix == null) calculationOptions.CustomSpellMix = new List<SpellWeight>();
            CustomSpellMixForm form = new CustomSpellMixForm(calculationOptions.CustomSpellMix);
            form.ShowDialog();
        }
	}
}
