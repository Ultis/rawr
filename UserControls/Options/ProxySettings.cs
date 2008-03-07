using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Rawr.UserControls.Options
{
	public partial class ProxySettings : UserControl, IOptions
	{
		public ProxySettings()
		{
			InitializeComponent();
			//cannot be in load, because its possible this tab won't show, and the values will not be initialized.
			//if this happens, then the users settings will be cleared.
			ProxyType.SelectedItem = ProxyType.Items[0];
			RequiresAuthCheckBox.Checked = Properties.NetworkSettings.Default.LoginToFirewall;
			Password.Text = Properties.NetworkSettings.Default.ProxyPassword;
			ProxyPort.Text = Properties.NetworkSettings.Default.ProxyPort.ToString();
			ProxyHost.Text = Properties.NetworkSettings.Default.ProxyServer;
			ProxyType.Text = Properties.NetworkSettings.Default.ProxyType;
			UserName.Text = Properties.NetworkSettings.Default.ProxyUserName;
			UseDefaultProxySettingsCheckBox.Checked = Properties.NetworkSettings.Default.UseDefaultProxySettings;

			RequiresAuthCheckBox_CheckedChanged(null, null);
			ProxyType_SelectedIndexChanged(null, null);
			UseDefaultProxySettingsCheckBox_CheckedChanged(null, null);
		}

		private void UseDefaultProxySettingsCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			if (UseDefaultProxySettingsCheckBox.Checked)
			{
				ProxyHost.Enabled = false;
				ProxyPort.Enabled = false;
			}
			else
			{
				ProxyPort.Enabled = true;
				ProxyHost.Enabled = true;
			}
		}

		private void ProxyType_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (ProxyType.SelectedItem != null && ProxyType.SelectedItem.ToString() == "None")
			{
				SettingsGroupBox.Enabled = false;
			}
			else
			{
				SettingsGroupBox.Enabled = true;
			}
		}

		private void RequiresAuthCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			if (RequiresAuthCheckBox.Checked)
			{
				UserName.Enabled = true;
				Password.Enabled = true;
			}
			else
			{
				UserName.Enabled = false;
				Password.Enabled = false;
			}
		}

		private void ProxyPort_Validating(object sender, CancelEventArgs e)
		{
			int port;
			if (UseDefaultProxySettingsCheckBox.Checked == false && (!int.TryParse(ProxyPort.Text, out port) || port < 0))
			{
				errorProvider1.SetError(ProxyPort, "Value must be an integer greater then 0");
			}
			else
			{
				errorProvider1.SetError(ProxyPort, "");
			}
		}

		#region IOptions Members


		public void Save()
		{
			if (HasValidationErrors())
			{
				throw new Exception("Settings are not in a valid state.  Cannot Save");
			}
			else
			{
				Properties.NetworkSettings.Default.LoginToFirewall = RequiresAuthCheckBox.Checked;
				Properties.NetworkSettings.Default.ProxyPassword = Password.Text;
				Properties.NetworkSettings.Default.ProxyPort = int.Parse(ProxyPort.Text);
				Properties.NetworkSettings.Default.ProxyServer = ProxyHost.Text;
				Properties.NetworkSettings.Default.ProxyType = ProxyType.Text;
				Properties.NetworkSettings.Default.ProxyUserName = UserName.Text;
				Properties.NetworkSettings.Default.UseDefaultProxySettings = UseDefaultProxySettingsCheckBox.Checked;
				Properties.NetworkSettings.Default.Save();
			}
		}

		public void Cancel()
		{
			//NOOP;
		}

		public bool HasValidationErrors()
		{
			return CheckChildrenValidation(this);
		}

		private bool CheckChildrenValidation(Control control)
		{
			bool invalid = false;

			for (int i = 0; i < control.Controls.Count; i++)
			{
				if (!String.IsNullOrEmpty(errorProvider1.GetError(control.Controls[i])))
				{
					invalid = true;
					break;
				}
				else
				{
					invalid = CheckChildrenValidation(control.Controls[i]);
					if (invalid)
					{
						break;
					}
				}
			}

			return invalid;
		}

		public string DisplayName
		{
			get { return "Proxy Settings"; }
		}



		public string TreePosition
		{
			get { return Rawr.Forms.Options.NETWORK_SETTINGS + Rawr.Forms.Options.MENU_DELIMETER + DisplayName; }
		}

		public Image MenuIcon
		{
			get { return OptionImageList.Images[0]; }
		}

		#endregion
	}
}
