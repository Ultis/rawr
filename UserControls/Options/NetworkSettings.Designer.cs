namespace Rawr.UserControls.Options
{
	partial class NetworkSettings
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NetworkSettings));
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.ProxyType = new System.Windows.Forms.ComboBox();
			this.SettingsGroupBox = new System.Windows.Forms.GroupBox();
			this.ProxyPort = new System.Windows.Forms.TextBox();
			this.Password = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.UserName = new System.Windows.Forms.TextBox();
			this.RequiresAuthCheckBox = new System.Windows.Forms.CheckBox();
			this.label2 = new System.Windows.Forms.Label();
			this.ProxyHost = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.UseDefaultProxySettingsCheckBox = new System.Windows.Forms.CheckBox();
			this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
			this.groupBox1.SuspendLayout();
			this.SettingsGroupBox.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			resources.ApplyResources(this.groupBox1, "groupBox1");
			this.groupBox1.Controls.Add(this.ProxyType);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.TabStop = false;
			// 
			// ProxyType
			// 
			this.ProxyType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ProxyType.FormattingEnabled = true;
			this.ProxyType.Items.AddRange(new object[] {
            resources.GetString("ProxyType.Items"),
            resources.GetString("ProxyType.Items1")});
			resources.ApplyResources(this.ProxyType, "ProxyType");
			this.ProxyType.Name = "ProxyType";
			this.ProxyType.SelectedIndexChanged += new System.EventHandler(this.ProxyType_SelectedIndexChanged);
			// 
			// SettingsGroupBox
			// 
			resources.ApplyResources(this.SettingsGroupBox, "SettingsGroupBox");
			this.SettingsGroupBox.Controls.Add(this.ProxyPort);
			this.SettingsGroupBox.Controls.Add(this.Password);
			this.SettingsGroupBox.Controls.Add(this.label4);
			this.SettingsGroupBox.Controls.Add(this.label3);
			this.SettingsGroupBox.Controls.Add(this.UserName);
			this.SettingsGroupBox.Controls.Add(this.RequiresAuthCheckBox);
			this.SettingsGroupBox.Controls.Add(this.label2);
			this.SettingsGroupBox.Controls.Add(this.ProxyHost);
			this.SettingsGroupBox.Controls.Add(this.label1);
			this.SettingsGroupBox.Controls.Add(this.UseDefaultProxySettingsCheckBox);
			this.SettingsGroupBox.Name = "SettingsGroupBox";
			this.SettingsGroupBox.TabStop = false;
			// 
			// ProxyPort
			// 
			resources.ApplyResources(this.ProxyPort, "ProxyPort");
			this.ProxyPort.Name = "ProxyPort";
			this.ProxyPort.Validating += new System.ComponentModel.CancelEventHandler(this.ProxyPort_Validating);
			// 
			// Password
			// 
			resources.ApplyResources(this.Password, "Password");
			this.Password.Name = "Password";
			this.Password.UseSystemPasswordChar = true;
			// 
			// label4
			// 
			resources.ApplyResources(this.label4, "label4");
			this.label4.Name = "label4";
			// 
			// label3
			// 
			resources.ApplyResources(this.label3, "label3");
			this.label3.Name = "label3";
			// 
			// UserName
			// 
			resources.ApplyResources(this.UserName, "UserName");
			this.UserName.Name = "UserName";
			// 
			// RequiresAuthCheckBox
			// 
			resources.ApplyResources(this.RequiresAuthCheckBox, "RequiresAuthCheckBox");
			this.RequiresAuthCheckBox.Name = "RequiresAuthCheckBox";
			this.RequiresAuthCheckBox.UseVisualStyleBackColor = true;
			this.RequiresAuthCheckBox.CheckedChanged += new System.EventHandler(this.RequiresAuthCheckBox_CheckedChanged);
			// 
			// label2
			// 
			resources.ApplyResources(this.label2, "label2");
			this.label2.Name = "label2";
			// 
			// ProxyHost
			// 
			resources.ApplyResources(this.ProxyHost, "ProxyHost");
			this.ProxyHost.Name = "ProxyHost";
			// 
			// label1
			// 
			resources.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			// 
			// UseDefaultProxySettingsCheckBox
			// 
			resources.ApplyResources(this.UseDefaultProxySettingsCheckBox, "UseDefaultProxySettingsCheckBox");
			this.UseDefaultProxySettingsCheckBox.Name = "UseDefaultProxySettingsCheckBox";
			this.UseDefaultProxySettingsCheckBox.UseVisualStyleBackColor = true;
			this.UseDefaultProxySettingsCheckBox.CheckedChanged += new System.EventHandler(this.UseDefaultProxySettingsCheckBox_CheckedChanged);
			// 
			// errorProvider1
			// 
			this.errorProvider1.ContainerControl = this;
			// 
			// NetworkSettings
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.SettingsGroupBox);
			this.Controls.Add(this.groupBox1);
			this.Name = "NetworkSettings";
			this.Load += new System.EventHandler(this.NetworkSettings_Load);
			this.groupBox1.ResumeLayout(false);
			this.SettingsGroupBox.ResumeLayout(false);
			this.SettingsGroupBox.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.ComboBox ProxyType;
		private System.Windows.Forms.GroupBox SettingsGroupBox;
		private System.Windows.Forms.CheckBox UseDefaultProxySettingsCheckBox;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox ProxyHost;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.CheckBox RequiresAuthCheckBox;
		private System.Windows.Forms.TextBox Password;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox UserName;
		private System.Windows.Forms.TextBox ProxyPort;
		private System.Windows.Forms.ErrorProvider errorProvider1;

	}
}
