namespace Rawr
{
	partial class CalculationOptionsPanelEnhance
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
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.label12 = new System.Windows.Forms.Label();
            this.cmbLength = new System.Windows.Forms.NumericUpDown();
            this.chbMagmaSearing = new System.Windows.Forms.CheckBox();
            this.chbBaseStatOption = new System.Windows.Forms.CheckBox();
            this.labelTargetArmorDescription = new System.Windows.Forms.Label();
            this.comboBoxOffhandImbue = new System.Windows.Forms.ComboBox();
            this.comboBoxMainhandImbue = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.radioButtonScryer = new System.Windows.Forms.RadioButton();
            this.radioButtonAldor = new System.Windows.Forms.RadioButton();
            this.trackBarTargetArmor = new System.Windows.Forms.TrackBar();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBoxTargetLevel = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.tbModuleNotes = new System.Windows.Forms.TextBox();
            this.btnEnhSim = new System.Windows.Forms.Button();
            this.tabControl.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbLength)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarTargetArmor)).BeginInit();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPage1);
            this.tabControl.Controls.Add(this.tabPage3);
            this.tabControl.Location = new System.Drawing.Point(0, 3);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(272, 551);
            this.tabControl.TabIndex = 9;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.label12);
            this.tabPage1.Controls.Add(this.cmbLength);
            this.tabPage1.Controls.Add(this.chbMagmaSearing);
            this.tabPage1.Controls.Add(this.chbBaseStatOption);
            this.tabPage1.Controls.Add(this.labelTargetArmorDescription);
            this.tabPage1.Controls.Add(this.comboBoxOffhandImbue);
            this.tabPage1.Controls.Add(this.comboBoxMainhandImbue);
            this.tabPage1.Controls.Add(this.label8);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.radioButtonScryer);
            this.tabPage1.Controls.Add(this.radioButtonAldor);
            this.tabPage1.Controls.Add(this.trackBarTargetArmor);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.comboBoxTargetLevel);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(264, 525);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Basics";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(139, 16);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(69, 13);
            this.label12.TabIndex = 36;
            this.label12.Text = "Fight Length:";
            // 
            // cmbLength
            // 
            this.cmbLength.DecimalPlaces = 1;
            this.cmbLength.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.cmbLength.Location = new System.Drawing.Point(214, 14);
            this.cmbLength.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.cmbLength.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.cmbLength.Name = "cmbLength";
            this.cmbLength.Size = new System.Drawing.Size(44, 20);
            this.cmbLength.TabIndex = 35;
            this.cmbLength.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.cmbLength.ValueChanged += new System.EventHandler(this.cmbLength_ValueChanged);
            // 
            // chbMagmaSearing
            // 
            this.chbMagmaSearing.AutoSize = true;
            this.chbMagmaSearing.Location = new System.Drawing.Point(10, 234);
            this.chbMagmaSearing.Name = "chbMagmaSearing";
            this.chbMagmaSearing.Size = new System.Drawing.Size(237, 17);
            this.chbMagmaSearing.TabIndex = 30;
            this.chbMagmaSearing.Text = "Use Magma Totem instead of Searing Totem";
            this.chbMagmaSearing.UseVisualStyleBackColor = true;
            this.chbMagmaSearing.CheckedChanged += new System.EventHandler(this.chbMagmaSearing_CheckedChanged);
            // 
            // chbBaseStatOption
            // 
            this.chbBaseStatOption.AutoSize = true;
            this.chbBaseStatOption.Location = new System.Drawing.Point(10, 211);
            this.chbBaseStatOption.Name = "chbBaseStatOption";
            this.chbBaseStatOption.Size = new System.Drawing.Size(211, 17);
            this.chbBaseStatOption.TabIndex = 29;
            this.chbBaseStatOption.Text = "Use AEP values in Relative Stats Chart";
            this.chbBaseStatOption.UseVisualStyleBackColor = true;
            this.chbBaseStatOption.CheckedChanged += new System.EventHandler(this.chbBaseStatOption_CheckedChanged);
            // 
            // labelTargetArmorDescription
            // 
            this.labelTargetArmorDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.labelTargetArmorDescription.Location = new System.Drawing.Point(71, 73);
            this.labelTargetArmorDescription.Name = "labelTargetArmorDescription";
            this.labelTargetArmorDescription.Size = new System.Drawing.Size(192, 40);
            this.labelTargetArmorDescription.TabIndex = 34;
            this.labelTargetArmorDescription.Text = "7700 Armor: Hydross, Lurker, Leotheras, Tidewalker, Al\'ar, Naj\'entus, Supremus, A" +
                "kama, Gurtogg";
            // 
            // comboBoxOffhandImbue
            // 
            this.comboBoxOffhandImbue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxOffhandImbue.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxOffhandImbue.Enabled = false;
            this.comboBoxOffhandImbue.FormattingEnabled = true;
            this.comboBoxOffhandImbue.Items.AddRange(new object[] {
            "Windfury",
            "Flametongue"});
            this.comboBoxOffhandImbue.Location = new System.Drawing.Point(99, 175);
            this.comboBoxOffhandImbue.Name = "comboBoxOffhandImbue";
            this.comboBoxOffhandImbue.Size = new System.Drawing.Size(159, 21);
            this.comboBoxOffhandImbue.TabIndex = 28;
            // 
            // comboBoxMainhandImbue
            // 
            this.comboBoxMainhandImbue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxMainhandImbue.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxMainhandImbue.FormattingEnabled = true;
            this.comboBoxMainhandImbue.Items.AddRange(new object[] {
            "Windfury",
            "Flametongue"});
            this.comboBoxMainhandImbue.Location = new System.Drawing.Point(99, 148);
            this.comboBoxMainhandImbue.Name = "comboBoxMainhandImbue";
            this.comboBoxMainhandImbue.Size = new System.Drawing.Size(159, 21);
            this.comboBoxMainhandImbue.TabIndex = 27;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(4, 178);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(80, 13);
            this.label8.TabIndex = 31;
            this.label8.Text = "Offhand Imbue:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(4, 151);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 13);
            this.label3.TabIndex = 30;
            this.label3.Text = "Mainhand Imbue:";
            // 
            // radioButtonScryer
            // 
            this.radioButtonScryer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.radioButtonScryer.AutoSize = true;
            this.radioButtonScryer.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.radioButtonScryer.Location = new System.Drawing.Point(74, 119);
            this.radioButtonScryer.Name = "radioButtonScryer";
            this.radioButtonScryer.Size = new System.Drawing.Size(55, 17);
            this.radioButtonScryer.TabIndex = 26;
            this.radioButtonScryer.Tag = "Shred";
            this.radioButtonScryer.Text = "Scryer";
            this.radioButtonScryer.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.radioButtonScryer.UseVisualStyleBackColor = true;
            this.radioButtonScryer.CheckedChanged += new System.EventHandler(this.radioButtonScryer_CheckedChanged);
            // 
            // radioButtonAldor
            // 
            this.radioButtonAldor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.radioButtonAldor.AutoSize = true;
            this.radioButtonAldor.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.radioButtonAldor.Checked = true;
            this.radioButtonAldor.Location = new System.Drawing.Point(6, 119);
            this.radioButtonAldor.Name = "radioButtonAldor";
            this.radioButtonAldor.Size = new System.Drawing.Size(49, 17);
            this.radioButtonAldor.TabIndex = 25;
            this.radioButtonAldor.TabStop = true;
            this.radioButtonAldor.Tag = "Mangle";
            this.radioButtonAldor.Text = "Aldor";
            this.radioButtonAldor.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.radioButtonAldor.UseVisualStyleBackColor = true;
            this.radioButtonAldor.CheckedChanged += new System.EventHandler(this.radioButtonAldor_CheckedChanged);
            // 
            // trackBarTargetArmor
            // 
            this.trackBarTargetArmor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.trackBarTargetArmor.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.trackBarTargetArmor.Enabled = false;
            this.trackBarTargetArmor.LargeChange = 1000;
            this.trackBarTargetArmor.Location = new System.Drawing.Point(74, 40);
            this.trackBarTargetArmor.Maximum = 15000;
            this.trackBarTargetArmor.Minimum = 3000;
            this.trackBarTargetArmor.Name = "trackBarTargetArmor";
            this.trackBarTargetArmor.Size = new System.Drawing.Size(184, 45);
            this.trackBarTargetArmor.SmallChange = 100;
            this.trackBarTargetArmor.TabIndex = 24;
            this.trackBarTargetArmor.TickFrequency = 300;
            this.trackBarTargetArmor.Value = 10645;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(-1, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 13);
            this.label2.TabIndex = 17;
            this.label2.Text = "Target Armor: ";
            // 
            // comboBoxTargetLevel
            // 
            this.comboBoxTargetLevel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxTargetLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxTargetLevel.Enabled = false;
            this.comboBoxTargetLevel.FormattingEnabled = true;
            this.comboBoxTargetLevel.Items.AddRange(new object[] {
            "80",
            "81",
            "82",
            "83"});
            this.comboBoxTargetLevel.Location = new System.Drawing.Point(74, 13);
            this.comboBoxTargetLevel.Name = "comboBoxTargetLevel";
            this.comboBoxTargetLevel.Size = new System.Drawing.Size(59, 21);
            this.comboBoxTargetLevel.TabIndex = 21;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(-1, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 13);
            this.label1.TabIndex = 18;
            this.label1.Text = "Target Level: ";
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.tbModuleNotes);
            this.tabPage3.Controls.Add(this.btnEnhSim);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(264, 525);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "EnhSim";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // tbModuleNotes
            // 
            this.tbModuleNotes.AcceptsReturn = true;
            this.tbModuleNotes.AcceptsTab = true;
            this.tbModuleNotes.Location = new System.Drawing.Point(3, 68);
            this.tbModuleNotes.Multiline = true;
            this.tbModuleNotes.Name = "tbModuleNotes";
            this.tbModuleNotes.ReadOnly = true;
            this.tbModuleNotes.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbModuleNotes.Size = new System.Drawing.Size(258, 454);
            this.tbModuleNotes.TabIndex = 1;
            // 
            // btnEnhSim
            // 
            this.btnEnhSim.Location = new System.Drawing.Point(21, 23);
            this.btnEnhSim.Name = "btnEnhSim";
            this.btnEnhSim.Size = new System.Drawing.Size(208, 28);
            this.btnEnhSim.TabIndex = 0;
            this.btnEnhSim.Text = "Export Stats to EnhSim config file";
            this.btnEnhSim.UseVisualStyleBackColor = true;
            this.btnEnhSim.Click += new System.EventHandler(this.btnEnhSim_Click);
            // 
            // CalculationOptionsPanelEnhance
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.tabControl);
            this.Name = "CalculationOptionsPanelEnhance";
            this.Size = new System.Drawing.Size(284, 573);
            this.tabControl.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbLength)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarTargetArmor)).EndInit();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.ResumeLayout(false);

		}

		#endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Label labelTargetArmorDescription;
        private System.Windows.Forms.ComboBox comboBoxOffhandImbue;
        private System.Windows.Forms.ComboBox comboBoxMainhandImbue;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RadioButton radioButtonScryer;
        private System.Windows.Forms.RadioButton radioButtonAldor;
        private System.Windows.Forms.TrackBar trackBarTargetArmor;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBoxTargetLevel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Button btnEnhSim;
        private System.Windows.Forms.TextBox tbModuleNotes;
        private System.Windows.Forms.CheckBox chbBaseStatOption;
        private System.Windows.Forms.CheckBox chbMagmaSearing;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.NumericUpDown cmbLength;

    }
}
