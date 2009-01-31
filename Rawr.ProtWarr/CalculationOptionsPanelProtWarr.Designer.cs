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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.extendedToolTipLabel1 = new Rawr.CustomControls.ExtendedToolTipLabel();
            this.checkBoxUseTankPoints = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarTargetArmor)).BeginInit();
            this.groupBoxWarriorSkills.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarShieldBlockUptime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarBossAttackValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarThreatScale)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarMitigationScale)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 16);
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
            this.comboBoxTargetLevel.Location = new System.Drawing.Point(86, 13);
            this.comboBoxTargetLevel.Name = "comboBoxTargetLevel";
            this.comboBoxTargetLevel.Size = new System.Drawing.Size(272, 21);
            this.comboBoxTargetLevel.TabIndex = 1;
            this.comboBoxTargetLevel.SelectedIndexChanged += new System.EventHandler(this.calculationOptionControl_Changed);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(3, 94);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 45);
            this.label2.TabIndex = 0;
            this.label2.Text = "Target Armor:  (Default: 13100)";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // trackBarTargetArmor
            // 
            this.trackBarTargetArmor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.trackBarTargetArmor.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.trackBarTargetArmor.LargeChange = 1000;
            this.trackBarTargetArmor.Location = new System.Drawing.Point(86, 94);
            this.trackBarTargetArmor.Maximum = 15000;
            this.trackBarTargetArmor.Minimum = 3000;
            this.trackBarTargetArmor.Name = "trackBarTargetArmor";
            this.trackBarTargetArmor.Size = new System.Drawing.Size(272, 45);
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
            this.labelTargetArmorDescription.Location = new System.Drawing.Point(92, 131);
            this.labelTargetArmorDescription.Name = "labelTargetArmorDescription";
            this.labelTargetArmorDescription.Size = new System.Drawing.Size(266, 34);
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
            this.groupBoxWarriorSkills.Location = new System.Drawing.Point(3, 365);
            this.groupBoxWarriorSkills.Name = "groupBoxWarriorSkills";
            this.groupBoxWarriorSkills.Size = new System.Drawing.Size(364, 91);
            this.groupBoxWarriorSkills.TabIndex = 4;
            this.groupBoxWarriorSkills.TabStop = false;
            this.groupBoxWarriorSkills.Text = "Warrior Skills";
            this.groupBoxWarriorSkills.Visible = false;
            // 
            // checkBoxUseShieldBlock
            // 
            this.checkBoxUseShieldBlock.AutoSize = true;
            this.checkBoxUseShieldBlock.Enabled = false;
            this.checkBoxUseShieldBlock.Location = new System.Drawing.Point(86, 68);
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
            this.labelShieldBlockUptime.Location = new System.Drawing.Point(92, 49);
            this.labelShieldBlockUptime.Name = "labelShieldBlockUptime";
            this.labelShieldBlockUptime.Size = new System.Drawing.Size(33, 13);
            this.labelShieldBlockUptime.TabIndex = 0;
            this.labelShieldBlockUptime.Text = "100%";
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(3, 17);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(83, 45);
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
            this.trackBarShieldBlockUptime.Location = new System.Drawing.Point(86, 17);
            this.trackBarShieldBlockUptime.Maximum = 100;
            this.trackBarShieldBlockUptime.Name = "trackBarShieldBlockUptime";
            this.trackBarShieldBlockUptime.Size = new System.Drawing.Size(272, 45);
            this.trackBarShieldBlockUptime.TabIndex = 2;
            this.trackBarShieldBlockUptime.TickFrequency = 10;
            this.trackBarShieldBlockUptime.Value = 100;
            this.trackBarShieldBlockUptime.ValueChanged += new System.EventHandler(this.calculationOptionControl_Changed);
            // 
            // labelBossAttack
            // 
            this.labelBossAttack.Location = new System.Drawing.Point(3, 43);
            this.labelBossAttack.Name = "labelBossAttack";
            this.labelBossAttack.Size = new System.Drawing.Size(83, 45);
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
            this.trackBarBossAttackValue.Location = new System.Drawing.Point(86, 43);
            this.trackBarBossAttackValue.Maximum = 500000;
            this.trackBarBossAttackValue.Minimum = 500;
            this.trackBarBossAttackValue.Name = "trackBarBossAttackValue";
            this.trackBarBossAttackValue.Size = new System.Drawing.Size(272, 45);
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
            this.labelBossAttackValue.Location = new System.Drawing.Point(92, 75);
            this.labelBossAttackValue.Name = "labelBossAttackValue";
            this.labelBossAttackValue.Size = new System.Drawing.Size(37, 13);
            this.labelBossAttackValue.TabIndex = 0;
            this.labelBossAttackValue.Text = "20000";
            // 
            // labelThreatScaleText
            // 
            this.labelThreatScaleText.Location = new System.Drawing.Point(6, 16);
            this.labelThreatScaleText.Name = "labelThreatScaleText";
            this.labelThreatScaleText.Size = new System.Drawing.Size(80, 45);
            this.labelThreatScaleText.TabIndex = 0;
            this.labelThreatScaleText.Text = "Threat Scale: * (Default: 25)";
            this.labelThreatScaleText.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.labelThreatScaleText.ToolTipText = "Threat scaling factor. PageUp/PageDown/Left Arrow/Right Arrow allows more accurat" +
                "e changes";
            // 
            // trackBarThreatScale
            // 
            this.trackBarThreatScale.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.trackBarThreatScale.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.trackBarThreatScale.Location = new System.Drawing.Point(86, 16);
            this.trackBarThreatScale.Maximum = 50;
            this.trackBarThreatScale.Name = "trackBarThreatScale";
            this.trackBarThreatScale.Size = new System.Drawing.Size(272, 45);
            this.trackBarThreatScale.SmallChange = 5;
            this.trackBarThreatScale.TabIndex = 2;
            this.trackBarThreatScale.TickFrequency = 5;
            this.trackBarThreatScale.Value = 25;
            this.trackBarThreatScale.ValueChanged += new System.EventHandler(this.calculationOptionControl_Changed);
            // 
            // labelThreatScale
            // 
            this.labelThreatScale.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.labelThreatScale.AutoSize = true;
            this.labelThreatScale.Location = new System.Drawing.Point(92, 48);
            this.labelThreatScale.Name = "labelThreatScale";
            this.labelThreatScale.Size = new System.Drawing.Size(13, 13);
            this.labelThreatScale.TabIndex = 0;
            this.labelThreatScale.Text = "1";
            // 
            // labelMitigationScaleText
            // 
            this.labelMitigationScaleText.Location = new System.Drawing.Point(9, 67);
            this.labelMitigationScaleText.Name = "labelMitigationScaleText";
            this.labelMitigationScaleText.Size = new System.Drawing.Size(77, 45);
            this.labelMitigationScaleText.TabIndex = 0;
            this.labelMitigationScaleText.Text = "Mitigation Scale: * (Default: 7500)";
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
            this.trackBarMitigationScale.Location = new System.Drawing.Point(86, 67);
            this.trackBarMitigationScale.Maximum = 15000;
            this.trackBarMitigationScale.Name = "trackBarMitigationScale";
            this.trackBarMitigationScale.Size = new System.Drawing.Size(272, 45);
            this.trackBarMitigationScale.SmallChange = 100;
            this.trackBarMitigationScale.TabIndex = 2;
            this.trackBarMitigationScale.TickFrequency = 1000;
            this.trackBarMitigationScale.Value = 12000;
            this.trackBarMitigationScale.ValueChanged += new System.EventHandler(this.calculationOptionControl_Changed);
            // 
            // labelMitigationScale
            // 
            this.labelMitigationScale.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.labelMitigationScale.AutoSize = true;
            this.labelMitigationScale.Location = new System.Drawing.Point(92, 99);
            this.labelMitigationScale.Name = "labelMitigationScale";
            this.labelMitigationScale.Size = new System.Drawing.Size(31, 13);
            this.labelMitigationScale.TabIndex = 0;
            this.labelMitigationScale.Text = "4000";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.extendedToolTipLabel1);
            this.groupBox1.Controls.Add(this.checkBoxUseTankPoints);
            this.groupBox1.Controls.Add(this.labelThreatScale);
            this.groupBox1.Controls.Add(this.trackBarThreatScale);
            this.groupBox1.Controls.Add(this.labelMitigationScale);
            this.groupBox1.Controls.Add(this.labelThreatScaleText);
            this.groupBox1.Controls.Add(this.labelMitigationScaleText);
            this.groupBox1.Controls.Add(this.trackBarMitigationScale);
            this.groupBox1.Location = new System.Drawing.Point(3, 177);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(364, 140);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Ranking System";
            // 
            // extendedToolTipLabel1
            // 
            this.extendedToolTipLabel1.Location = new System.Drawing.Point(102, 117);
            this.extendedToolTipLabel1.Name = "extendedToolTipLabel1";
            this.extendedToolTipLabel1.Size = new System.Drawing.Size(134, 14);
            this.extendedToolTipLabel1.TabIndex = 6;
            this.extendedToolTipLabel1.Text = "Use TankPoints Instead *";
            this.extendedToolTipLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.extendedToolTipLabel1.ToolTipText = "Uses TankPoints for overall rankings instead of Mitigation vs. Survival";
            // 
            // checkBoxUseTankPoints
            // 
            this.checkBoxUseTankPoints.AutoSize = true;
            this.checkBoxUseTankPoints.Location = new System.Drawing.Point(86, 118);
            this.checkBoxUseTankPoints.Name = "checkBoxUseTankPoints";
            this.checkBoxUseTankPoints.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.checkBoxUseTankPoints.Size = new System.Drawing.Size(15, 14);
            this.checkBoxUseTankPoints.TabIndex = 6;
            this.checkBoxUseTankPoints.UseVisualStyleBackColor = true;
            this.checkBoxUseTankPoints.CheckedChanged += new System.EventHandler(this.checkBoxUseTankPoints_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.comboBoxTargetLevel);
            this.groupBox2.Controls.Add(this.labelBossAttack);
            this.groupBox2.Controls.Add(this.labelTargetArmorDescription);
            this.groupBox2.Controls.Add(this.labelBossAttackValue);
            this.groupBox2.Controls.Add(this.trackBarTargetArmor);
            this.groupBox2.Controls.Add(this.trackBarBossAttackValue);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Location = new System.Drawing.Point(3, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(364, 168);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Ranking System";
            // 
            // CalculationOptionsPanelProtWarr
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBoxWarriorSkills);
            this.Name = "CalculationOptionsPanelProtWarr";
            this.Size = new System.Drawing.Size(370, 494);
            ((System.ComponentModel.ISupportInitialize)(this.trackBarTargetArmor)).EndInit();
            this.groupBoxWarriorSkills.ResumeLayout(false);
            this.groupBoxWarriorSkills.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarShieldBlockUptime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarBossAttackValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarThreatScale)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarMitigationScale)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

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
        private System.Windows.Forms.Label labelShieldBlockUptime;
        private System.Windows.Forms.TrackBar trackBarShieldBlockUptime;
        private System.Windows.Forms.CheckBox checkBoxUseShieldBlock;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox checkBoxUseTankPoints;
        private Rawr.CustomControls.ExtendedToolTipLabel extendedToolTipLabel1;
        private System.Windows.Forms.GroupBox groupBox2;
	}
}
