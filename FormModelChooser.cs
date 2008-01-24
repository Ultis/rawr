using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Rawr
{
	public partial class FormModelChooser : Form
	{
		public FormModelChooser()
		{
			InitializeComponent();

			listBoxModels.Items.Clear();
			foreach (Calculations.CalculationModel model in Enum.GetValues(typeof(Calculations.CalculationModel)))
			{
				listBoxModels.Items.Add(model);
			}
			listBoxModels.SelectedIndex = 0;
		}

		private void listBoxModels_SelectedIndexChanged(object sender, EventArgs e)
		{
			buttonLoad.Enabled = listBoxModels.SelectedItem != null;
		}

		private void buttonLoad_Click(object sender, EventArgs e)
		{
			Calculations.LoadModel((Calculations.CalculationModel)listBoxModels.SelectedItem);
			this.Close();
		}

        private void listBoxModels_DoubleClick(object sender, EventArgs e)
        {
            Calculations.LoadModel((Calculations.CalculationModel) listBoxModels.SelectedItem);
            this.Close();
        }
	}
}
