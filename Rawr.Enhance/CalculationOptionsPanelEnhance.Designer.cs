namespace Rawr.Enhance
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
            this.CB_InBackPerc = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.CK_InBack = new Rawr.CustomControls.ExtendedToolTipCheckBox();
            this.LB_TargLvl = new System.Windows.Forms.Label();
            this.LB_TargArmor = new System.Windows.Forms.Label();
            this.CB_TargLvl = new System.Windows.Forms.ComboBox();
            this.CB_TargArmor = new System.Windows.Forms.ComboBox();
            this.comboBoxBoss = new System.Windows.Forms.ComboBox();
            this.labelAverageLag = new System.Windows.Forms.Label();
            this.trackBarAverageLag = new System.Windows.Forms.TrackBar();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbLength = new System.Windows.Forms.NumericUpDown();
            this.chbMagmaSearing = new System.Windows.Forms.CheckBox();
            this.chbBaseStatOption = new System.Windows.Forms.CheckBox();
            this.comboBoxOffhandImbue = new System.Windows.Forms.ComboBox();
            this.comboBoxMainhandImbue = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.tbModuleNotes = new System.Windows.Forms.TextBox();
            this.btnEnhSim = new System.Windows.Forms.Button();
            this.TB_BossInfo = new System.Windows.Forms.TextBox();
            this.tabControl.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CB_InBackPerc)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarAverageLag)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbLength)).BeginInit();
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
            this.tabPage1.Controls.Add(this.CB_InBackPerc);
            this.tabPage1.Controls.Add(this.TB_BossInfo);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.CK_InBack);
            this.tabPage1.Controls.Add(this.LB_TargLvl);
            this.tabPage1.Controls.Add(this.LB_TargArmor);
            this.tabPage1.Controls.Add(this.CB_TargLvl);
            this.tabPage1.Controls.Add(this.CB_TargArmor);
            this.tabPage1.Controls.Add(this.comboBoxBoss);
            this.tabPage1.Controls.Add(this.labelAverageLag);
            this.tabPage1.Controls.Add(this.trackBarAverageLag);
            this.tabPage1.Controls.Add(this.label5);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.cmbLength);
            this.tabPage1.Controls.Add(this.chbMagmaSearing);
            this.tabPage1.Controls.Add(this.chbBaseStatOption);
            this.tabPage1.Controls.Add(this.comboBoxOffhandImbue);
            this.tabPage1.Controls.Add(this.comboBoxMainhandImbue);
            this.tabPage1.Controls.Add(this.label8);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(264, 525);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Basics";
            this.tabPage1.ToolTipText = "++";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // CB_InBackPerc
            // 
            this.CB_InBackPerc.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.CB_InBackPerc.Location = new System.Drawing.Point(172, 69);
            this.CB_InBackPerc.Name = "CB_InBackPerc";
            this.CB_InBackPerc.Size = new System.Drawing.Size(75, 20);
            this.CB_InBackPerc.TabIndex = 46;
            this.CB_InBackPerc.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.CB_InBackPerc.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.CB_InBackPerc.ValueChanged += new System.EventHandler(this.CB_InBackPerc_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(246, 71);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(15, 13);
            this.label1.TabIndex = 47;
            this.label1.Text = "%";
            // 
            // CK_InBack
            // 
            this.CK_InBack.BackColor = System.Drawing.Color.Transparent;
            this.CK_InBack.Checked = true;
            this.CK_InBack.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CK_InBack.Location = new System.Drawing.Point(10, 69);
            this.CK_InBack.Name = "CK_InBack";
            this.CK_InBack.Size = new System.Drawing.Size(167, 20);
            this.CK_InBack.TabIndex = 45;
            this.CK_InBack.Text = "You stand behind boss *";
            this.CK_InBack.ToolTipText = "This affects how often the Boss can Parry your attacks (no bosses in WotLK Block)" +
                "";
            this.CK_InBack.UseVisualStyleBackColor = false;
            this.CK_InBack.CheckedChanged += new System.EventHandler(this.CK_InBack_CheckedChanged);
            // 
            // LB_TargLvl
            // 
            this.LB_TargLvl.BackColor = System.Drawing.Color.Transparent;
            this.LB_TargLvl.Location = new System.Drawing.Point(7, 42);
            this.LB_TargLvl.Name = "LB_TargLvl";
            this.LB_TargLvl.Size = new System.Drawing.Size(36, 21);
            this.LB_TargLvl.TabIndex = 41;
            this.LB_TargLvl.Text = "Level:";
            this.LB_TargLvl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // LB_TargArmor
            // 
            this.LB_TargArmor.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.LB_TargArmor.BackColor = System.Drawing.Color.Transparent;
            this.LB_TargArmor.Location = new System.Drawing.Point(125, 40);
            this.LB_TargArmor.Name = "LB_TargArmor";
            this.LB_TargArmor.Size = new System.Drawing.Size(41, 21);
            this.LB_TargArmor.TabIndex = 43;
            this.LB_TargArmor.Text = "Armor:";
            this.LB_TargArmor.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // CB_TargLvl
            // 
            this.CB_TargLvl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.CB_TargLvl.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CB_TargLvl.Enabled = false;
            this.CB_TargLvl.Items.AddRange(new object[] {
            "80",
            "81",
            "82",
            "83"});
            this.CB_TargLvl.Location = new System.Drawing.Point(49, 42);
            this.CB_TargLvl.Name = "CB_TargLvl";
            this.CB_TargLvl.Size = new System.Drawing.Size(70, 21);
            this.CB_TargLvl.Sorted = true;
            this.CB_TargLvl.TabIndex = 42;
            // 
            // CB_TargArmor
            // 
            this.CB_TargArmor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.CB_TargArmor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CB_TargArmor.Enabled = false;
            this.CB_TargArmor.Items.AddRange(new object[] {
            "10643",
            "10338",
            "10034",
            "9729"});
            this.CB_TargArmor.Location = new System.Drawing.Point(172, 41);
            this.CB_TargArmor.Name = "CB_TargArmor";
            this.CB_TargArmor.Size = new System.Drawing.Size(86, 21);
            this.CB_TargArmor.TabIndex = 44;
            // 
            // comboBoxBoss
            // 
            this.comboBoxBoss.FormattingEnabled = true;
            this.comboBoxBoss.Location = new System.Drawing.Point(49, 14);
            this.comboBoxBoss.Name = "comboBoxBoss";
            this.comboBoxBoss.Size = new System.Drawing.Size(209, 21);
            this.comboBoxBoss.TabIndex = 40;
            this.comboBoxBoss.SelectedIndexChanged += new System.EventHandler(this.comboBoxBoss_SelectedIndexChanged);
            // 
            // labelAverageLag
            // 
            this.labelAverageLag.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.labelAverageLag.Location = new System.Drawing.Point(71, 298);
            this.labelAverageLag.Name = "labelAverageLag";
            this.labelAverageLag.Size = new System.Drawing.Size(192, 12);
            this.labelAverageLag.TabIndex = 39;
            this.labelAverageLag.Text = "250";
            // 
            // trackBarAverageLag
            // 
            this.trackBarAverageLag.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.trackBarAverageLag.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.trackBarAverageLag.Cursor = System.Windows.Forms.Cursors.Default;
            this.trackBarAverageLag.LargeChange = 25;
            this.trackBarAverageLag.Location = new System.Drawing.Point(74, 265);
            this.trackBarAverageLag.Maximum = 750;
            this.trackBarAverageLag.Name = "trackBarAverageLag";
            this.trackBarAverageLag.Size = new System.Drawing.Size(184, 45);
            this.trackBarAverageLag.SmallChange = 5;
            this.trackBarAverageLag.TabIndex = 38;
            this.trackBarAverageLag.TickFrequency = 50;
            this.trackBarAverageLag.Value = 250;
            this.trackBarAverageLag.ValueChanged += new System.EventHandler(this.trackBarAverageLag_ValueChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 274);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(71, 13);
            this.label5.TabIndex = 37;
            this.label5.Text = "Average Lag:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 97);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(69, 13);
            this.label4.TabIndex = 36;
            this.label4.Text = "Fight Length:";
            // 
            // cmbLength
            // 
            this.cmbLength.DecimalPlaces = 1;
            this.cmbLength.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.cmbLength.Location = new System.Drawing.Point(82, 95);
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
            10,
            0,
            0,
            0});
            this.cmbLength.ValueChanged += new System.EventHandler(this.cmbLength_ValueChanged);
            // 
            // chbMagmaSearing
            // 
            this.chbMagmaSearing.AutoSize = true;
            this.chbMagmaSearing.Checked = true;
            this.chbMagmaSearing.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbMagmaSearing.Location = new System.Drawing.Point(10, 408);
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
            this.chbBaseStatOption.Checked = true;
            this.chbBaseStatOption.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbBaseStatOption.Location = new System.Drawing.Point(10, 385);
            this.chbBaseStatOption.Name = "chbBaseStatOption";
            this.chbBaseStatOption.Size = new System.Drawing.Size(211, 17);
            this.chbBaseStatOption.TabIndex = 29;
            this.chbBaseStatOption.Text = "Use AEP values in Relative Stats Chart";
            this.chbBaseStatOption.UseVisualStyleBackColor = true;
            this.chbBaseStatOption.CheckedChanged += new System.EventHandler(this.chbBaseStatOption_CheckedChanged);
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
            this.comboBoxOffhandImbue.Location = new System.Drawing.Point(99, 349);
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
            this.comboBoxMainhandImbue.Location = new System.Drawing.Point(99, 322);
            this.comboBoxMainhandImbue.Name = "comboBoxMainhandImbue";
            this.comboBoxMainhandImbue.Size = new System.Drawing.Size(159, 21);
            this.comboBoxMainhandImbue.TabIndex = 27;
            this.comboBoxMainhandImbue.SelectedIndexChanged += new System.EventHandler(this.comboBoxMainhandImbue_SelectedIndexChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(7, 352);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(80, 13);
            this.label8.TabIndex = 31;
            this.label8.Text = "Offhand Imbue:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 325);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 13);
            this.label3.TabIndex = 30;
            this.label3.Text = "Mainhand Imbue:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(36, 13);
            this.label2.TabIndex = 17;
            this.label2.Text = "Boss: ";
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
            // TB_BossInfo
            // 
            this.TB_BossInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.TB_BossInfo.Location = new System.Drawing.Point(9, 121);
            this.TB_BossInfo.MaxLength = 65536;
            this.TB_BossInfo.Multiline = true;
            this.TB_BossInfo.Name = "TB_BossInfo";
            this.TB_BossInfo.ReadOnly = true;
            this.TB_BossInfo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.TB_BossInfo.Size = new System.Drawing.Size(249, 135);
            this.TB_BossInfo.TabIndex = 52;
            this.TB_BossInfo.Text = "Boss Information would normally be displayed here";
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
            ((System.ComponentModel.ISupportInitialize)(this.CB_InBackPerc)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarAverageLag)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbLength)).EndInit();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.ResumeLayout(false);

		}

		#endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.ComboBox comboBoxOffhandImbue;
        private System.Windows.Forms.ComboBox comboBoxMainhandImbue;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Button btnEnhSim;
        private System.Windows.Forms.TextBox tbModuleNotes;
        private System.Windows.Forms.CheckBox chbBaseStatOption;
        private System.Windows.Forms.CheckBox chbMagmaSearing;
        private System.Windows.Forms.NumericUpDown cmbLength;
        private System.Windows.Forms.Label labelAverageLag;
        private System.Windows.Forms.TrackBar trackBarAverageLag;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox comboBoxBoss;
        private System.Windows.Forms.NumericUpDown CB_InBackPerc;
        private Rawr.CustomControls.ExtendedToolTipCheckBox CK_InBack;
        public System.Windows.Forms.Label LB_TargLvl;
        public System.Windows.Forms.Label LB_TargArmor;
        public System.Windows.Forms.ComboBox CB_TargLvl;
        public System.Windows.Forms.ComboBox CB_TargArmor;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox TB_BossInfo;

    }
}
