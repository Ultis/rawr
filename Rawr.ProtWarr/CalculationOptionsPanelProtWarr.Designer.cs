namespace Rawr.ProtWarr
{
	partial class CalculationOptionsPanelProtWarr
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
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxTargetLevel = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.trackBarTargetArmor = new System.Windows.Forms.TrackBar();
            this.labelTargetArmorDescription = new System.Windows.Forms.Label();
            this.groupBoxWarriorSkills = new System.Windows.Forms.GroupBox();
            this.checkBoxUseShieldBlock = new System.Windows.Forms.CheckBox();
            this.labelShieldBlockUptime = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.trackBarShieldBlockUptime = new System.Windows.Forms.TrackBar();
            this.labelBossAttack = new Rawr.CustomControls.ExtendedToolTipLabel();
            this.trackBarBossAttackValue = new System.Windows.Forms.TrackBar();
            this.labelBossAttackValue = new System.Windows.Forms.Label();
            this.labelThreatScaleText = new Rawr.CustomControls.ExtendedToolTipLabel();
            this.trackBarThreatScale = new System.Windows.Forms.TrackBar();
            this.labelThreatScale = new System.Windows.Forms.Label();
            this.labelMitigationScaleText = new Rawr.CustomControls.ExtendedToolTipLabel();
            this.trackBarMitigationScale = new System.Windows.Forms.TrackBar();
            this.labelMitigationScale = new System.Windows.Forms.Label();
            this.radioButtonAldor = new System.Windows.Forms.RadioButton();
            this.radioButtonScryer = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarTargetArmor)).BeginInit();
            this.groupBoxWarriorSkills.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarShieldBlockUptime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarBossAttackValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarThreatScale)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarMitigationScale)).BeginInit();
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
            this.comboBoxTargetLevel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxTargetLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxTargetLevel.FormattingEnabled = true;
            this.comboBoxTargetLevel.Items.AddRange(new object[] {
            "80",
            "81",
            "82",
            "83"});
            this.comboBoxTargetLevel.Location = new System.Drawing.Point(83, 3);
            this.comboBoxTargetLevel.Name = "comboBoxTargetLevel";
            this.comboBoxTargetLevel.Size = new System.Drawing.Size(123, 21);
            this.comboBoxTargetLevel.TabIndex = 1;
            this.comboBoxTargetLevel.SelectedIndexChanged += new System.EventHandler(this.calculationOptionControl_Changed);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(9, 186);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 42);
            this.label2.TabIndex = 0;
            this.label2.Text = "Target Armor: ";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // trackBarTargetArmor
            // 
            this.trackBarTargetArmor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.trackBarTargetArmor.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.trackBarTargetArmor.LargeChange = 1000;
            this.trackBarTargetArmor.Location = new System.Drawing.Point(75, 183);
            this.trackBarTargetArmor.Maximum = 9000;
            this.trackBarTargetArmor.Minimum = 3000;
            this.trackBarTargetArmor.Name = "trackBarTargetArmor";
            this.trackBarTargetArmor.Size = new System.Drawing.Size(131, 45);
            this.trackBarTargetArmor.SmallChange = 100;
            this.trackBarTargetArmor.TabIndex = 2;
            this.trackBarTargetArmor.TickFrequency = 300;
            this.trackBarTargetArmor.Value = 7700;
            this.trackBarTargetArmor.ValueChanged += new System.EventHandler(this.calculationOptionControl_Changed);
            // 
            // labelTargetArmorDescription
            // 
            this.labelTargetArmorDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.labelTargetArmorDescription.Location = new System.Drawing.Point(6, 231);
            this.labelTargetArmorDescription.Name = "labelTargetArmorDescription";
            this.labelTargetArmorDescription.Size = new System.Drawing.Size(200, 40);
            this.labelTargetArmorDescription.TabIndex = 0;
            this.labelTargetArmorDescription.Text = "7700 Armor: Hydross, Lurker, Leotheras, Tidewalker, Al\'ar, Naj\'entus, Supremus, A" +
                "kama, Gurtogg";
            // 
            // groupBoxWarriorSkills
            // 
            this.groupBoxWarriorSkills.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxWarriorSkills.Controls.Add(this.checkBoxUseShieldBlock);
            this.groupBoxWarriorSkills.Controls.Add(this.labelShieldBlockUptime);
            this.groupBoxWarriorSkills.Controls.Add(this.label9);
            this.groupBoxWarriorSkills.Controls.Add(this.trackBarShieldBlockUptime);
            this.groupBoxWarriorSkills.Location = new System.Drawing.Point(3, 336);
            this.groupBoxWarriorSkills.Name = "groupBoxWarriorSkills";
            this.groupBoxWarriorSkills.Size = new System.Drawing.Size(203, 91);
            this.groupBoxWarriorSkills.TabIndex = 4;
            this.groupBoxWarriorSkills.TabStop = false;
            this.groupBoxWarriorSkills.Text = "Warrior Skills";
            // 
            // checkBoxUseShieldBlock
            // 
            this.checkBoxUseShieldBlock.AutoSize = true;
            this.checkBoxUseShieldBlock.Enabled = false;
            this.checkBoxUseShieldBlock.Location = new System.Drawing.Point(3, 68);
            this.checkBoxUseShieldBlock.Name = "checkBoxUseShieldBlock";
            this.checkBoxUseShieldBlock.Size = new System.Drawing.Size(107, 17);
            this.checkBoxUseShieldBlock.TabIndex = 5;
            this.checkBoxUseShieldBlock.Text = "Use Shield Block";
            this.checkBoxUseShieldBlock.UseVisualStyleBackColor = true;
            this.checkBoxUseShieldBlock.CheckedChanged += new System.EventHandler(this.checkBoxUseShieldBlock_CheckedChanged);
            // 
            // labelShieldBlockUptime
            // 
            this.labelShieldBlockUptime.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.labelShieldBlockUptime.AutoSize = true;
            this.labelShieldBlockUptime.Location = new System.Drawing.Point(77, 49);
            this.labelShieldBlockUptime.Name = "labelShieldBlockUptime";
            this.labelShieldBlockUptime.Size = new System.Drawing.Size(33, 13);
            this.labelShieldBlockUptime.TabIndex = 0;
            this.labelShieldBlockUptime.Text = "100%";
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(3, 17);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(64, 45);
            this.label9.TabIndex = 0;
            this.label9.Text = "Shield Block Uptime %:";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // trackBarShieldBlockUptime
            // 
            this.trackBarShieldBlockUptime.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.trackBarShieldBlockUptime.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.trackBarShieldBlockUptime.Enabled = false;
            this.trackBarShieldBlockUptime.LargeChange = 10;
            this.trackBarShieldBlockUptime.Location = new System.Drawing.Point(72, 17);
            this.trackBarShieldBlockUptime.Maximum = 100;
            this.trackBarShieldBlockUptime.Name = "trackBarShieldBlockUptime";
            this.trackBarShieldBlockUptime.Size = new System.Drawing.Size(125, 45);
            this.trackBarShieldBlockUptime.TabIndex = 2;
            this.trackBarShieldBlockUptime.TickFrequency = 10;
            this.trackBarShieldBlockUptime.Value = 100;
            this.trackBarShieldBlockUptime.ValueChanged += new System.EventHandler(this.calculationOptionControl_Changed);
            // 
            // labelBossAttack
            // 
            this.labelBossAttack.Location = new System.Drawing.Point(6, 30);
            this.labelBossAttack.Name = "labelBossAttack";
            this.labelBossAttack.Size = new System.Drawing.Size(64, 45);
            this.labelBossAttack.TabIndex = 0;
            this.labelBossAttack.Text = "Boss Attack: *";
            this.labelBossAttack.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.labelBossAttack.ToolTipText = "Boss attack value before armor. Used to determine the value of Block Value.";
            // 
            // trackBarBossAttackValue
            // 
            this.trackBarBossAttackValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.trackBarBossAttackValue.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.trackBarBossAttackValue.LargeChange = 5000;
            this.trackBarBossAttackValue.Location = new System.Drawing.Point(75, 30);
            this.trackBarBossAttackValue.Maximum = 50000;
            this.trackBarBossAttackValue.Minimum = 500;
            this.trackBarBossAttackValue.Name = "trackBarBossAttackValue";
            this.trackBarBossAttackValue.Size = new System.Drawing.Size(131, 45);
            this.trackBarBossAttackValue.SmallChange = 500;
            this.trackBarBossAttackValue.TabIndex = 2;
            this.trackBarBossAttackValue.TickFrequency = 2500;
            this.trackBarBossAttackValue.Value = 20000;
            this.trackBarBossAttackValue.ValueChanged += new System.EventHandler(this.calculationOptionControl_Changed);
            // 
            // labelBossAttackValue
            // 
            this.labelBossAttackValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.labelBossAttackValue.AutoSize = true;
            this.labelBossAttackValue.Location = new System.Drawing.Point(80, 62);
            this.labelBossAttackValue.Name = "labelBossAttackValue";
            this.labelBossAttackValue.Size = new System.Drawing.Size(37, 13);
            this.labelBossAttackValue.TabIndex = 0;
            this.labelBossAttackValue.Text = "20000";
            // 
            // labelThreatScaleText
            // 
            this.labelThreatScaleText.Location = new System.Drawing.Point(6, 81);
            this.labelThreatScaleText.Name = "labelThreatScaleText";
            this.labelThreatScaleText.Size = new System.Drawing.Size(64, 45);
            this.labelThreatScaleText.TabIndex = 0;
            this.labelThreatScaleText.Text = "Threat Scale: *";
            this.labelThreatScaleText.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.labelThreatScaleText.ToolTipText = "Threat scaling factor. PageUp/PageDown/Left Arrow/Right Arrow allows more accurat" +
                "e changes";
            // 
            // trackBarThreatScale
            // 
            this.trackBarThreatScale.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.trackBarThreatScale.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.trackBarThreatScale.LargeChange = 50;
            this.trackBarThreatScale.Location = new System.Drawing.Point(75, 81);
            this.trackBarThreatScale.Maximum = 1000;
            this.trackBarThreatScale.Minimum = 1;
            this.trackBarThreatScale.Name = "trackBarThreatScale";
            this.trackBarThreatScale.Size = new System.Drawing.Size(131, 45);
            this.trackBarThreatScale.TabIndex = 2;
            this.trackBarThreatScale.TickFrequency = 50;
            this.trackBarThreatScale.Value = 1;
            this.trackBarThreatScale.ValueChanged += new System.EventHandler(this.calculationOptionControl_Changed);
            // 
            // labelThreatScale
            // 
            this.labelThreatScale.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.labelThreatScale.AutoSize = true;
            this.labelThreatScale.Location = new System.Drawing.Point(80, 113);
            this.labelThreatScale.Name = "labelThreatScale";
            this.labelThreatScale.Size = new System.Drawing.Size(13, 13);
            this.labelThreatScale.TabIndex = 0;
            this.labelThreatScale.Text = "1";
            // 
            // labelMitigationScaleText
            // 
            this.labelMitigationScaleText.Location = new System.Drawing.Point(9, 132);
            this.labelMitigationScaleText.Name = "labelMitigationScaleText";
            this.labelMitigationScaleText.Size = new System.Drawing.Size(61, 45);
            this.labelMitigationScaleText.TabIndex = 0;
            this.labelMitigationScaleText.Text = "Mitigation Scale: *";
            this.labelMitigationScaleText.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.labelMitigationScaleText.ToolTipText = "Mitigation Points scaling factor. PageUp/PageDown/Left Arrow/Right Arrow allows m" +
                "ore accurate changes";
            // 
            // trackBarMitigationScale
            // 
            this.trackBarMitigationScale.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.trackBarMitigationScale.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.trackBarMitigationScale.LargeChange = 1000;
            this.trackBarMitigationScale.Location = new System.Drawing.Point(75, 132);
            this.trackBarMitigationScale.Maximum = 15000;
            this.trackBarMitigationScale.Minimum = 1000;
            this.trackBarMitigationScale.Name = "trackBarMitigationScale";
            this.trackBarMitigationScale.Size = new System.Drawing.Size(131, 45);
            this.trackBarMitigationScale.SmallChange = 50;
            this.trackBarMitigationScale.TabIndex = 2;
            this.trackBarMitigationScale.TickFrequency = 1000;
            this.trackBarMitigationScale.Value = 4000;
            this.trackBarMitigationScale.ValueChanged += new System.EventHandler(this.calculationOptionControl_Changed);
            // 
            // labelMitigationScale
            // 
            this.labelMitigationScale.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.labelMitigationScale.AutoSize = true;
            this.labelMitigationScale.Location = new System.Drawing.Point(80, 164);
            this.labelMitigationScale.Name = "labelMitigationScale";
            this.labelMitigationScale.Size = new System.Drawing.Size(31, 13);
            this.labelMitigationScale.TabIndex = 0;
            this.labelMitigationScale.Text = "4000";
            // 
            // radioButtonAldor
            // 
            this.radioButtonAldor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.radioButtonAldor.AutoSize = true;
            this.radioButtonAldor.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.radioButtonAldor.Checked = true;
            this.radioButtonAldor.Location = new System.Drawing.Point(7, 313);
            this.radioButtonAldor.Name = "radioButtonAldor";
            this.radioButtonAldor.Size = new System.Drawing.Size(49, 17);
            this.radioButtonAldor.TabIndex = 3;
            this.radioButtonAldor.TabStop = true;
            this.radioButtonAldor.Tag = "Mangle";
            this.radioButtonAldor.Text = "Aldor";
            this.radioButtonAldor.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.radioButtonAldor.UseVisualStyleBackColor = true;
            this.radioButtonAldor.CheckedChanged += new System.EventHandler(this.calculationOptionControl_Changed);
            // 
            // radioButtonScryer
            // 
            this.radioButtonScryer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.radioButtonScryer.AutoSize = true;
            this.radioButtonScryer.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.radioButtonScryer.Location = new System.Drawing.Point(83, 313);
            this.radioButtonScryer.Name = "radioButtonScryer";
            this.radioButtonScryer.Size = new System.Drawing.Size(55, 17);
            this.radioButtonScryer.TabIndex = 3;
            this.radioButtonScryer.Tag = "Shred";
            this.radioButtonScryer.Text = "Scryer";
            this.radioButtonScryer.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.radioButtonScryer.UseVisualStyleBackColor = true;
            this.radioButtonScryer.CheckedChanged += new System.EventHandler(this.calculationOptionControl_Changed);
            // 
            // CalculationOptionsPanelProtWarr
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.radioButtonScryer);
            this.Controls.Add(this.radioButtonAldor);
            this.Controls.Add(this.groupBoxWarriorSkills);
            this.Controls.Add(this.labelMitigationScale);
            this.Controls.Add(this.labelThreatScale);
            this.Controls.Add(this.labelBossAttackValue);
            this.Controls.Add(this.labelTargetArmorDescription);
            this.Controls.Add(this.trackBarMitigationScale);
            this.Controls.Add(this.trackBarThreatScale);
            this.Controls.Add(this.trackBarBossAttackValue);
            this.Controls.Add(this.labelMitigationScaleText);
            this.Controls.Add(this.trackBarTargetArmor);
            this.Controls.Add(this.labelThreatScaleText);
            this.Controls.Add(this.labelBossAttack);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comboBoxTargetLevel);
            this.Controls.Add(this.label1);
            this.Name = "CalculationOptionsPanelProtWarr";
            this.Size = new System.Drawing.Size(209, 776);
            ((System.ComponentModel.ISupportInitialize)(this.trackBarTargetArmor)).EndInit();
            this.groupBoxWarriorSkills.ResumeLayout(false);
            this.groupBoxWarriorSkills.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarShieldBlockUptime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarBossAttackValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarThreatScale)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarMitigationScale)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox comboBoxTargetLevel;
        private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TrackBar trackBarTargetArmor;
        private System.Windows.Forms.Label labelTargetArmorDescription;
		private System.Windows.Forms.GroupBox groupBoxWarriorSkills;
        private Rawr.CustomControls.ExtendedToolTipLabel labelBossAttack;
		private System.Windows.Forms.TrackBar trackBarBossAttackValue;
		private System.Windows.Forms.Label labelBossAttackValue;
        private Rawr.CustomControls.ExtendedToolTipLabel labelThreatScaleText;
		private System.Windows.Forms.TrackBar trackBarThreatScale;
		private System.Windows.Forms.Label labelThreatScale;
        private Rawr.CustomControls.ExtendedToolTipLabel labelMitigationScaleText;
		private System.Windows.Forms.TrackBar trackBarMitigationScale;
		private System.Windows.Forms.Label labelMitigationScale;
        private System.Windows.Forms.Label label9;
		private System.Windows.Forms.RadioButton radioButtonAldor;
        private System.Windows.Forms.RadioButton radioButtonScryer;
        private System.Windows.Forms.Label labelShieldBlockUptime;
        private System.Windows.Forms.TrackBar trackBarShieldBlockUptime;
        private System.Windows.Forms.CheckBox checkBoxUseShieldBlock;
	}
}
