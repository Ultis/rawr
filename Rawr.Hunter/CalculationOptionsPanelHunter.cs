using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Rawr.Hunter
{
    public partial class CalculationOptionsPanelHunter : CalculationOptionsPanelBase
    {
        #region Instance Variables

        private HunterTalentsForm hunterTalents = null;
        private bool loadingOptions = false;
		private CalculationOptionsHunter options = null;
        #endregion

        #region Constructors

        public CalculationOptionsPanelHunter()
        {
            InitializeComponent();
            hunterTalents = new HunterTalentsForm(this);
			foreach(Enum e in Enum.GetValues(typeof(Aspect)))
			{
				comboActiveAspect.Items.Add(e);
			}
			foreach (Enum e in Enum.GetValues(typeof(ShotRotation)))
			{
				comboShotRotation.Items.Add(e);
			}
        }

        #endregion

        #region Overrides

        protected override void LoadCalculationOptions()
        {
            loadingOptions = true;
			options = Character.CalculationOptions as CalculationOptionsHunter;
			if (options == null)
			{
				options = new CalculationOptionsHunter();
				Character.CalculationOptions = options;
			}
			for (int i = 0; i < cmbTargetLevel.Items.Count; i++)
			{
				if (cmbTargetLevel.Items[i] as string == options.TargetLevel.ToString())
				{
					cmbTargetLevel.SelectedItem = cmbTargetLevel.Items[i];
					break;
				}
			}
            chkEnforceMetaGemRequirements.Checked = options.EnforceMetaGem;
			comboActiveAspect.SelectedItem = options.Aspect;
			comboShotRotation.SelectedItem = options.ShotRotation;
            loadingOptions = false;
        }

        #endregion

        #region Event Handlers

        private void btnTalents_Click(object sender, EventArgs e)
        {
			if (hunterTalents == null || hunterTalents.IsDisposed)
			{
				hunterTalents = new HunterTalentsForm(this);
			}
            hunterTalents.Show();
        }

        private void chkEnforceMetaGemRequirements_CheckedChanged(object sender, EventArgs e)
        {
            options.EnforceMetaGem = chkEnforceMetaGemRequirements.Checked;
            Character.OnItemsChanged();
        }

        private void cmbTargetLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!loadingOptions)
            {
                options.TargetLevel = int.Parse(cmbTargetLevel.SelectedItem.ToString());
                Character.OnItemsChanged();
            }
        }

		private void comboActiveAspect_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!loadingOptions && comboActiveAspect.SelectedItem != null)
			{
				options.Aspect = (Aspect)comboActiveAspect.SelectedItem;
				Character.OnItemsChanged(); 
			}
		}

		private void comboShotRotation_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!loadingOptions && comboShotRotation.SelectedItem != null)
			{
				options.ShotRotation = (ShotRotation)comboShotRotation.SelectedItem;
				Character.OnItemsChanged();
			}
		}

        #endregion

    }
}
