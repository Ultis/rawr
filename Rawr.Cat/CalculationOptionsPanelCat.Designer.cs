namespace Rawr
{
	partial class CalculationOptionsPanelCat
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CalculationOptionsPanelCat));
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxTargetLevel = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBoxPowershift = new System.Windows.Forms.ComboBox();
            this.trackBarTargetArmor = new System.Windows.Forms.TrackBar();
            this.labelTargetArmorDescription = new System.Windows.Forms.Label();
            this.radioButtonMangle = new System.Windows.Forms.RadioButton();
            this.groupBoxPrimaryAttack = new System.Windows.Forms.GroupBox();
            this.radioButtonBoth = new System.Windows.Forms.RadioButton();
            this.radioButtonShred = new System.Windows.Forms.RadioButton();
            this.groupBoxFinisher = new System.Windows.Forms.GroupBox();
            this.radioButtonNone = new System.Windows.Forms.RadioButton();
            this.radioButtonFerociousBite = new System.Windows.Forms.RadioButton();
            this.radioButtonRip = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize) (this.trackBarTargetArmor)).BeginInit();
            this.groupBoxPrimaryAttack.SuspendLayout();
            this.groupBoxFinisher.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Target Level: ";
            // 
            // comboBoxTargetLevel
            // 
            this.comboBoxTargetLevel.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxTargetLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxTargetLevel.Enabled = false;
            this.comboBoxTargetLevel.FormattingEnabled = true;
            this.comboBoxTargetLevel.Items.AddRange(new object[] {
            "70",
            "71",
            "72",
            "73"});
            this.comboBoxTargetLevel.Location = new System.Drawing.Point(83, 3);
            this.comboBoxTargetLevel.Name = "comboBoxTargetLevel";
            this.comboBoxTargetLevel.Size = new System.Drawing.Size(123, 21);
            this.comboBoxTargetLevel.TabIndex = 1;
            this.comboBoxTargetLevel.SelectedIndexChanged += new System.EventHandler(this.calculationOptionControl_Changed);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 33);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Target Armor: ";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 124);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(62, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Powershift: ";
            // 
            // comboBoxPowershift
            // 
            this.comboBoxPowershift.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxPowershift.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxPowershift.FormattingEnabled = true;
            this.comboBoxPowershift.Items.AddRange(new object[] {
            "Never",
            "Every Cycle",
            "Every 2nd Cycle",
            "Every 3rd Cycle",
            "Every 4th Cycle",
            "Every 5th Cycle"});
            this.comboBoxPowershift.Location = new System.Drawing.Point(83, 121);
            this.comboBoxPowershift.Name = "comboBoxPowershift";
            this.comboBoxPowershift.Size = new System.Drawing.Size(123, 21);
            this.comboBoxPowershift.TabIndex = 1;
            this.comboBoxPowershift.SelectedIndexChanged += new System.EventHandler(this.calculationOptionControl_Changed);
            // 
            // trackBarTargetArmor
            // 
            this.trackBarTargetArmor.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.trackBarTargetArmor.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.trackBarTargetArmor.LargeChange = 1000;
            this.trackBarTargetArmor.Location = new System.Drawing.Point(75, 30);
            this.trackBarTargetArmor.Maximum = 9000;
            this.trackBarTargetArmor.Minimum = 3000;
            this.trackBarTargetArmor.Name = "trackBarTargetArmor";
            this.trackBarTargetArmor.Size = new System.Drawing.Size(139, 45);
            this.trackBarTargetArmor.SmallChange = 100;
            this.trackBarTargetArmor.TabIndex = 2;
            this.trackBarTargetArmor.TickFrequency = 200;
            this.trackBarTargetArmor.Value = 7700;
            this.trackBarTargetArmor.ValueChanged += new System.EventHandler(this.calculationOptionControl_Changed);
            // 
            // labelTargetArmorDescription
            // 
            this.labelTargetArmorDescription.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.labelTargetArmorDescription.Location = new System.Drawing.Point(6, 78);
            this.labelTargetArmorDescription.Name = "labelTargetArmorDescription";
            this.labelTargetArmorDescription.Size = new System.Drawing.Size(200, 40);
            this.labelTargetArmorDescription.TabIndex = 0;
            this.labelTargetArmorDescription.Text = "7700 Armor: Hydross, Lurker, Leotheras, Tidewalker, Al\'ar, Naj\'entus, Supremus, A" +
                "kama, Gurtogg";
            // 
            // radioButtonMangle
            // 
            this.radioButtonMangle.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.radioButtonMangle.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.radioButtonMangle.Enabled = false;
            this.radioButtonMangle.Location = new System.Drawing.Point(6, 19);
            this.radioButtonMangle.Name = "radioButtonMangle";
            this.radioButtonMangle.Size = new System.Drawing.Size(190, 43);
            this.radioButtonMangle.TabIndex = 3;
            this.radioButtonMangle.Tag = "Mangle";
            this.radioButtonMangle.Text = "Mangle - For when you can\'t get behind your target to Shred, such as is common in" +
                " PvP";
            this.radioButtonMangle.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.radioButtonMangle.UseVisualStyleBackColor = true;
            this.radioButtonMangle.CheckedChanged += new System.EventHandler(this.calculationOptionControl_Changed);
            // 
            // groupBoxPrimaryAttack
            // 
            this.groupBoxPrimaryAttack.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxPrimaryAttack.Controls.Add(this.radioButtonBoth);
            this.groupBoxPrimaryAttack.Controls.Add(this.radioButtonShred);
            this.groupBoxPrimaryAttack.Controls.Add(this.radioButtonMangle);
            this.groupBoxPrimaryAttack.Enabled = false;
            this.groupBoxPrimaryAttack.Location = new System.Drawing.Point(3, 148);
            this.groupBoxPrimaryAttack.Name = "groupBoxPrimaryAttack";
            this.groupBoxPrimaryAttack.Size = new System.Drawing.Size(203, 170);
            this.groupBoxPrimaryAttack.TabIndex = 4;
            this.groupBoxPrimaryAttack.TabStop = false;
            this.groupBoxPrimaryAttack.Text = "Primary Attack - Not Implemented Yet";
            // 
            // radioButtonBoth
            // 
            this.radioButtonBoth.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.radioButtonBoth.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.radioButtonBoth.Checked = true;
            this.radioButtonBoth.Enabled = false;
            this.radioButtonBoth.Location = new System.Drawing.Point(6, 119);
            this.radioButtonBoth.Name = "radioButtonBoth";
            this.radioButtonBoth.Size = new System.Drawing.Size(190, 45);
            this.radioButtonBoth.TabIndex = 3;
            this.radioButtonBoth.TabStop = true;
            this.radioButtonBoth.Tag = "Both";
            this.radioButtonBoth.Text = "Both - For when you are behind the target, and are doing the Mangling yourself";
            this.radioButtonBoth.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.radioButtonBoth.UseVisualStyleBackColor = true;
            this.radioButtonBoth.CheckedChanged += new System.EventHandler(this.calculationOptionControl_Changed);
            // 
            // radioButtonShred
            // 
            this.radioButtonShred.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.radioButtonShred.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.radioButtonShred.Enabled = false;
            this.radioButtonShred.Location = new System.Drawing.Point(6, 68);
            this.radioButtonShred.Name = "radioButtonShred";
            this.radioButtonShred.Size = new System.Drawing.Size(191, 45);
            this.radioButtonShred.TabIndex = 3;
            this.radioButtonShred.Tag = "Shred";
            this.radioButtonShred.Text = "Shred - For when the target is being Mangled by someone else, such as when a bear" +
                " is tanking";
            this.radioButtonShred.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.radioButtonShred.UseVisualStyleBackColor = true;
            this.radioButtonShred.CheckedChanged += new System.EventHandler(this.calculationOptionControl_Changed);
            // 
            // groupBoxFinisher
            // 
            this.groupBoxFinisher.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxFinisher.Controls.Add(this.radioButtonNone);
            this.groupBoxFinisher.Controls.Add(this.radioButtonFerociousBite);
            this.groupBoxFinisher.Controls.Add(this.radioButtonRip);
            this.groupBoxFinisher.Enabled = false;
            this.groupBoxFinisher.Location = new System.Drawing.Point(3, 324);
            this.groupBoxFinisher.Name = "groupBoxFinisher";
            this.groupBoxFinisher.Size = new System.Drawing.Size(203, 170);
            this.groupBoxFinisher.TabIndex = 4;
            this.groupBoxFinisher.TabStop = false;
            this.groupBoxFinisher.Text = "Finisher - Not Implemented Yet";
            // 
            // radioButtonNone
            // 
            this.radioButtonNone.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.radioButtonNone.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.radioButtonNone.Enabled = false;
            this.radioButtonNone.Location = new System.Drawing.Point(6, 119);
            this.radioButtonNone.Name = "radioButtonNone";
            this.radioButtonNone.Size = new System.Drawing.Size(190, 45);
            this.radioButtonNone.TabIndex = 3;
            this.radioButtonNone.Tag = "None";
            this.radioButtonNone.Text = "None - For when the target can\'t bleed and you want sustained damage";
            this.radioButtonNone.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.radioButtonNone.UseVisualStyleBackColor = true;
            this.radioButtonNone.CheckedChanged += new System.EventHandler(this.calculationOptionControl_Changed);
            // 
            // radioButtonFerociousBite
            // 
            this.radioButtonFerociousBite.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.radioButtonFerociousBite.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.radioButtonFerociousBite.Enabled = false;
            this.radioButtonFerociousBite.Location = new System.Drawing.Point(6, 68);
            this.radioButtonFerociousBite.Name = "radioButtonFerociousBite";
            this.radioButtonFerociousBite.Size = new System.Drawing.Size(191, 45);
            this.radioButtonFerociousBite.TabIndex = 3;
            this.radioButtonFerociousBite.Tag = "Ferocious Bite";
            this.radioButtonFerociousBite.Text = "Ferocious Bite - For when the target can\'t bleed and you want burst damage";
            this.radioButtonFerociousBite.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.radioButtonFerociousBite.UseVisualStyleBackColor = true;
            this.radioButtonFerociousBite.CheckedChanged += new System.EventHandler(this.calculationOptionControl_Changed);
            // 
            // radioButtonRip
            // 
            this.radioButtonRip.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.radioButtonRip.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.radioButtonRip.Checked = true;
            this.radioButtonRip.Enabled = false;
            this.radioButtonRip.Location = new System.Drawing.Point(6, 19);
            this.radioButtonRip.Name = "radioButtonRip";
            this.radioButtonRip.Size = new System.Drawing.Size(190, 43);
            this.radioButtonRip.TabIndex = 3;
            this.radioButtonRip.TabStop = true;
            this.radioButtonRip.Tag = "Rip";
            this.radioButtonRip.Text = "Rip - For when the target can bleed and you want sustained damage";
            this.radioButtonRip.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.radioButtonRip.UseVisualStyleBackColor = true;
            this.radioButtonRip.CheckedChanged += new System.EventHandler(this.calculationOptionControl_Changed);
            // 
            // CalculationOptionsPanelCat
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxFinisher);
            this.Controls.Add(this.groupBoxPrimaryAttack);
            this.Controls.Add(this.labelTargetArmorDescription);
            this.Controls.Add(this.trackBarTargetArmor);
            this.Controls.Add(this.comboBoxPowershift);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comboBoxTargetLevel);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon) (resources.GetObject("$this.Icon")));
            this.Name = "CalculationOptionsPanelCat";
            this.Size = new System.Drawing.Size(209, 544);
            this.Load += new System.EventHandler(this.CalculationOptionsPanelCat_Load);
            ((System.ComponentModel.ISupportInitialize) (this.trackBarTargetArmor)).EndInit();
            this.groupBoxPrimaryAttack.ResumeLayout(false);
            this.groupBoxFinisher.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox comboBoxTargetLevel;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ComboBox comboBoxPowershift;
		private System.Windows.Forms.TrackBar trackBarTargetArmor;
		private System.Windows.Forms.Label labelTargetArmorDescription;
		private System.Windows.Forms.RadioButton radioButtonMangle;
		private System.Windows.Forms.GroupBox groupBoxPrimaryAttack;
		private System.Windows.Forms.RadioButton radioButtonBoth;
		private System.Windows.Forms.RadioButton radioButtonShred;
		private System.Windows.Forms.GroupBox groupBoxFinisher;
		private System.Windows.Forms.RadioButton radioButtonNone;
		private System.Windows.Forms.RadioButton radioButtonFerociousBite;
		private System.Windows.Forms.RadioButton radioButtonRip;
	}
}
