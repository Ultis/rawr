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

        #endregion

        #region Constructors

        public CalculationOptionsPanelHunter()
        {
            InitializeComponent();
            hunterTalents = new HunterTalentsForm(this);
        }

        #endregion

        #region Overrides

        protected override void LoadCalculationOptions()
        {
            loadingOptions = true;

            if (!Character.CalculationOptions.ContainsKey("TargetLevel"))
                Character.CalculationOptions["TargetLevel"] = "73";
                    
            if (!Character.CalculationOptions.ContainsKey("EnforceMetagemRequirements"))
                Character.CalculationOptions["EnforceMetagemRequirements"] = "No";

            cmbTargetLevel.SelectedItem = Character.CalculationOptions["TargetLevel"];
            chkEnforceMetaGemRequirements.Checked = Character.CalculationOptions["EnforceMetagemRequirements"] == "Yes";

            if (hunterTalents != null)
                hunterTalents.LoadCalculationOptions();

            loadingOptions = false;
        }

        #endregion

        #region Event Handlers

        private void btnTalents_Click(object sender, EventArgs e)
        {
            hunterTalents.Show();
        }

        private void chkEnforceMetaGemRequirements_CheckedChanged(object sender, EventArgs e)
        {
            Character.CalculationOptions["EnforceMetagemRequirements"] = chkEnforceMetaGemRequirements.Checked ? "Yes" : "No";
            Character.OnItemsChanged();
        }

        private void cmbTargetLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!loadingOptions)
            {
                Character.CalculationOptions["TargetLevel"] = cmbTargetLevel.SelectedItem.ToString();
                Character.OnItemsChanged();
            }
        }

        #endregion
    }
}
