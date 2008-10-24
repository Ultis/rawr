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

        private bool loadingOptions = false;
		private CalculationOptionsHunter options = null;
        #endregion

        #region Constructors

        public CalculationOptionsPanelHunter()
        {
            InitializeComponent();
            foreach (Enum e in Enum.GetValues(typeof(PetFamily)))
            {
                comboPetFamily.Items.Add(e);
            }
            foreach (Enum e in Enum.GetValues(typeof(PetAttacks)))
            {
                comboPetPriority1.Items.Add(e);
            }
            foreach (Enum e in Enum.GetValues(typeof(PetAttacks)))
            {
                comboPetPriority2.Items.Add(e);
            }
            foreach (Enum e in Enum.GetValues(typeof(PetAttacks)))
            {
                comboPetPriority3.Items.Add(e);
            }
            foreach (Enum e in Enum.GetValues(typeof(Shots)))
            {
                comboShotPrio1.Items.Add(e);
                comboShotPrio2.Items.Add(e);
                comboShotPrio3.Items.Add(e);
                //comboShotPrio4.Items.Add(e);
            }
            comboShotPrio4.Items.Add(Shots.SteadyShot);
            comboShotPrio4.SelectedIndex = 0;
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
            comboPetFamily.SelectedItem = options.PetFamily;
            comboPetPriority1.SelectedItem = options.PetPriority1;
            comboPetPriority2.SelectedItem = options.PetPriority2;
            comboPetPriority3.SelectedItem = options.PetPriority3;
            comboShotPrio1.SelectedItem = options.ShotPriority1;
            comboShotPrio2.SelectedItem = options.ShotPriority2;
            comboShotPrio3.SelectedItem = options.ShotPriority3;
            comboShotPrio4.SelectedItem = options.ShotPriority4;
            numericUpDownLatency.Value = (decimal)(options.Latency * 1000.0);
            checkBoxUseCustomShotRotation.Checked = options.UseCustomShotRotation;
            trackBarTargetArmor.Value = options.TargetArmor;
            lblTargetArmorValue.Text = options.TargetArmor.ToString();
            loadingOptions = false;
        }

        #endregion

        #region Event Handlers

        private void cmbTargetLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!loadingOptions)
            {
                options.TargetLevel = int.Parse(cmbTargetLevel.SelectedItem.ToString());
                Character.OnCalculationsInvalidated();
            }
        }

        private void comboPetFamily_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!loadingOptions && comboPetFamily.SelectedItem != null)
            {
                options.PetFamily = (PetFamily)comboPetFamily.SelectedItem;
                Character.OnCalculationsInvalidated();
            }
        }

        private void comboPetPriority1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!loadingOptions && comboPetPriority1.SelectedItem != null)
            {
                options.PetPriority1 = (PetAttacks)comboPetPriority1.SelectedItem;
                Character.OnCalculationsInvalidated();
            }
        }

        private void comboPetPriority2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!loadingOptions && comboPetPriority2.SelectedItem != null)
            {
                options.PetPriority2 = (PetAttacks)comboPetPriority2.SelectedItem;
                Character.OnCalculationsInvalidated();
            }
        }

        private void comboPetPriority3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!loadingOptions && comboPetPriority3.SelectedItem != null)
            {
                options.PetPriority3 = (PetAttacks)comboPetPriority3.SelectedItem;
                Character.OnCalculationsInvalidated();
            }
        }

        #endregion

        private void trackBarTargetArmor_Scroll(object sender, EventArgs e)
        {
            if (!loadingOptions)
            {
                options.TargetArmor = trackBarTargetArmor.Value;
                Character.OnCalculationsInvalidated();
                lblTargetArmorValue.Text = trackBarTargetArmor.Value.ToString();
            }
        }

        private void comboShotPrio1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!loadingOptions && comboShotPrio1.SelectedItem != null)
            {
                options.ShotPriority1 = (Shots)comboShotPrio1.SelectedItem;
                Character.OnCalculationsInvalidated();
            }
        }

        private void comboShotPrio2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!loadingOptions && comboShotPrio2.SelectedItem != null)
            {
                options.ShotPriority2 = (Shots)comboShotPrio2.SelectedItem;
                Character.OnCalculationsInvalidated();
            }
        }

        private void comboShotPrio3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!loadingOptions && comboShotPrio3.SelectedItem != null)
            {
                options.ShotPriority3 = (Shots)comboShotPrio3.SelectedItem;
                Character.OnCalculationsInvalidated();
            }
        }

        private void comboShotPrio4_SelectedIndexChanged(object sender, EventArgs e)
        {
            /*
            if (!loadingOptions && comboShotPrio4.SelectedItem != null)
            {
                options.ShotPriority4 = (Shots)comboShotPrio4.SelectedItem;
                Character.OnCalculationsInvalidated();
            }
            */
        }

        private void numericUpDownLatency_ValueChanged(object sender, EventArgs e)
        {
            if (!loadingOptions)
            {
                options.Latency = (float)numericUpDownLatency.Value/1000.0f;
                Character.OnCalculationsInvalidated();
            }
        }

        private void checkBoxUseCustomShotRotation_CheckedChanged(object sender, EventArgs e)
        {
            if (!loadingOptions)
            {
                options.UseCustomShotRotation = checkBoxUseCustomShotRotation.Checked;
                Character.OnCalculationsInvalidated();
            }
        }



    }
}
