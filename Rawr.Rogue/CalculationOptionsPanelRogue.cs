using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Rawr.Rogue
{
	public partial class CalculationOptionsPanelRogue : CalculationOptionsPanelBase
	{

		public CalculationOptionsPanelRogue()
		{
			InitializeComponent();
			talents = new RogueTalentsForm(this);
		}

        private RogueTalentsForm talents;

		protected override void LoadCalculationOptions()
		{
			_loadingCalculationOptions = true;
			if (!Character.CalculationOptions.ContainsKey("TargetLevel"))
				Character.CalculationOptions["TargetLevel"] = "73";
			if (!Character.CalculationOptions.ContainsKey("EnforceMetagemRequirements"))
				Character.CalculationOptions["EnforceMetagemRequirements"] = "No";

			comboBoxTargetLevel.SelectedItem = Character.CalculationOptions["TargetLevel"];
			checkBoxEnforceMetagemRequirements.Checked = Character.CalculationOptions["EnforceMetagemRequirements"] == "Yes";

			if ( talents != null ) talents.LoadCalculationOptions();

			_loadingCalculationOptions = false;
		}

		private bool _loadingCalculationOptions = false;
		private void calculationOptionControl_Changed(object sender, EventArgs e)
		{
			if (!_loadingCalculationOptions)
			{
				Character.CalculationOptions["TargetLevel"] = comboBoxTargetLevel.SelectedItem.ToString();
		
				Character.OnItemsChanged();
			}
		}

		private void checkBoxEnforceMetagemRequirements_CheckedChanged(object sender, EventArgs e)
		{
			Character.CalculationOptions["EnforceMetagemRequirements"] = checkBoxEnforceMetagemRequirements.Checked ? "Yes" : "No";
			Character.OnItemsChanged();
		}

		private void buttonTalents_Click( object sender, EventArgs e )
		{
            talents.Show();
		}
	}
}
