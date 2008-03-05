using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Rawr.UserControls.Options;

//TODO: Load option controls dynamically
//TODO: Add ability for option controls to have an icon next to their display name in tree view
namespace Rawr.Forms
{
	/// <summary>
	/// Option display form.
	/// </summary>
	public partial class Options : Form
	{
		public Options()
		{
			InitializeComponent();
		}

		private void OK_Click(object sender, EventArgs e)
		{
			List<string> controlErrors = new List<string>();
			foreach (Control control in panel1.Controls)
			{
				IOptions options = control as IOptions;
				if (options != null)
				{
					if (options.HasValidationErrors())
					{
						controlErrors.Add(options.DisplayName);
					}
				}
			}
			if (controlErrors.Count > 0)
			{
				StringBuilder sb = new StringBuilder();
				sb.Append("The following tabs have validation errors on them:");
				sb.Append(Environment.NewLine);
				for (int i = 0; i < controlErrors.Count; i++)
				{
					sb.Append(controlErrors[i]);
					sb.Append(Environment.NewLine);
				}
				MessageBox.Show(sb.ToString(), "Validation Errors", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			else
			{
				foreach (Control control in panel1.Controls)
				{
					IOptions options = control as IOptions;
					if (options != null)
					{
						options.Save();
					}
				}
				DialogResult = DialogResult.OK;
			}
		}

		private void Cancel_Click(object sender, EventArgs e)
		{
			foreach (Control control in panel1.Controls)
			{
				IOptions options = control as IOptions;
				if (options != null)
				{
					options.Cancel();
				}
			}
			DialogResult = DialogResult.Cancel;
		}

		private void Options_Load(object sender, EventArgs e)
		{
			treeView1.ExpandAll();
			//TODO: load option controls onto panel dynamically to make it easier for
			// developers to add option menus
		}
	}
}
