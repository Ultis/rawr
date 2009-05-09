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
            this.trackBarMitigationScale = new System.Windows.Forms.TrackBar();
            this.labelMitigationScale = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.extendedToolTipDamageOutput = new Rawr.CustomControls.ExtendedToolTipLabel();
            this.radioButtonDamageOutput = new System.Windows.Forms.RadioButton();
            this.extendedToolTipBurstTime = new Rawr.CustomControls.ExtendedToolTipLabel();
            this.extendedToolTipMitigtionScale = new Rawr.CustomControls.ExtendedToolTipLabel();
            this.extendedToolTipTankPoints = new Rawr.CustomControls.ExtendedToolTipLabel();
            this.radioButtonBurstTime = new System.Windows.Forms.RadioButton();
            this.radioButtonTankPoints = new System.Windows.Forms.RadioButton();
            this.radioButtonMitigationScale = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.extendedToolTipUseParryHaste = new Rawr.CustomControls.ExtendedToolTipLabel();
            this.labelBossSpeed = new Rawr.CustomControls.ExtendedToolTipLabel();
            this.checkBoxUseParryHaste = new System.Windows.Forms.CheckBox();
            this.labelBossAttackSpeed = new System.Windows.Forms.Label();
            this.trackBarBossAttackSpeed = new System.Windows.Forms.TrackBar();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.labelVigilanceThreat = new Rawr.CustomControls.ExtendedToolTipLabel();
            this.labelVigilanceValue = new System.Windows.Forms.Label();
            this.trackBarVigilanceValue = new System.Windows.Forms.TrackBar();
            this.extendedToolTipUseVigilance = new Rawr.CustomControls.ExtendedToolTipLabel();
            this.checkBoxUseVigilance = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarTargetArmor)).BeginInit();
            this.groupBoxWarriorSkills.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarShieldBlockUptime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarBossAttackValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarThreatScale)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarMitigationScale)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarBossAttackSpeed)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarVigilanceValue)).BeginInit();
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
            this.label2.Location = new System.Drawing.Point(3, 165);
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
            this.trackBarTargetArmor.Location = new System.Drawing.Point(86, 165);
            this.trackBarTargetArmor.Maximum = 20000;
            this.trackBarTargetArmor.Name = "trackBarTargetArmor";
            this.trackBarTargetArmor.Size = new System.Drawing.Size(272, 45);
            this.trackBarTargetArmor.SmallChange = 100;
            this.trackBarTargetArmor.TabIndex = 2;
            this.trackBarTargetArmor.TickFrequency = 1000;
            this.trackBarTargetArmor.Value = 13100;
            this.trackBarTargetArmor.ValueChanged += new System.EventHandler(this.calculationOptionControl_Changed);
            // 
            // labelTargetArmorDescription
            // 
            this.labelTargetArmorDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.labelTargetArmorDescription.Location = new System.Drawing.Point(92, 202);
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
            this.groupBoxWarriorSkills.Location = new System.Drawing.Point(3, 544);
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
            this.labelBossAttack.Text = "Base Attack: * (Default: 25000)";
            this.labelBossAttack.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.labelBossAttack.ToolTipText = "Base attacker damage before armor.";
            // 
            // trackBarBossAttackValue
            // 
            this.trackBarBossAttackValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.trackBarBossAttackValue.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.trackBarBossAttackValue.LargeChange = 5000;
            this.trackBarBossAttackValue.Location = new System.Drawing.Point(86, 43);
            this.trackBarBossAttackValue.Maximum = 100000;
            this.trackBarBossAttackValue.Minimum = 500;
            this.trackBarBossAttackValue.Name = "trackBarBossAttackValue";
            this.trackBarBossAttackValue.Size = new System.Drawing.Size(272, 45);
            this.trackBarBossAttackValue.SmallChange = 500;
            this.trackBarBossAttackValue.TabIndex = 2;
            this.trackBarBossAttackValue.TickFrequency = 5000;
            this.trackBarBossAttackValue.Value = 25000;
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
            this.labelBossAttackValue.Text = "25000";
            // 
            // labelThreatScaleText
            // 
            this.labelThreatScaleText.Location = new System.Drawing.Point(6, 16);
            this.labelThreatScaleText.Name = "labelThreatScaleText";
            this.labelThreatScaleText.Size = new System.Drawing.Size(80, 45);
            this.labelThreatScaleText.TabIndex = 0;
            this.labelThreatScaleText.Text = "Threat Scale: * (Default: 1.0)";
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
            this.trackBarThreatScale.Maximum = 30;
            this.trackBarThreatScale.Name = "trackBarThreatScale";
            this.trackBarThreatScale.Size = new System.Drawing.Size(272, 45);
            this.trackBarThreatScale.TabIndex = 2;
            this.trackBarThreatScale.Value = 10;
            this.trackBarThreatScale.ValueChanged += new System.EventHandler(this.calculationOptionControl_Changed);
            // 
            // labelThreatScale
            // 
            this.labelThreatScale.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.labelThreatScale.AutoSize = true;
            this.labelThreatScale.Location = new System.Drawing.Point(92, 48);
            this.labelThreatScale.Name = "labelThreatScale";
            this.labelThreatScale.Size = new System.Drawing.Size(22, 13);
            this.labelThreatScale.TabIndex = 0;
            this.labelThreatScale.Text = "1.0";
            // 
            // trackBarMitigationScale
            // 
            this.trackBarMitigationScale.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.trackBarMitigationScale.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.trackBarMitigationScale.Location = new System.Drawing.Point(86, 67);
            this.trackBarMitigationScale.Maximum = 30;
            this.trackBarMitigationScale.Name = "trackBarMitigationScale";
            this.trackBarMitigationScale.Size = new System.Drawing.Size(272, 45);
            this.trackBarMitigationScale.TabIndex = 2;
            this.trackBarMitigationScale.Value = 10;
            this.trackBarMitigationScale.ValueChanged += new System.EventHandler(this.calculationOptionControl_Changed);
            // 
            // labelMitigationScale
            // 
            this.labelMitigationScale.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.labelMitigationScale.AutoSize = true;
            this.labelMitigationScale.Location = new System.Drawing.Point(92, 99);
            this.labelMitigationScale.Name = "labelMitigationScale";
            this.labelMitigationScale.Size = new System.Drawing.Size(22, 13);
            this.labelMitigationScale.TabIndex = 0;
            this.labelMitigationScale.Text = "1.0";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.extendedToolTipDamageOutput);
            this.groupBox1.Controls.Add(this.radioButtonDamageOutput);
            this.groupBox1.Controls.Add(this.extendedToolTipBurstTime);
            this.groupBox1.Controls.Add(this.extendedToolTipMitigtionScale);
            this.groupBox1.Controls.Add(this.extendedToolTipTankPoints);
            this.groupBox1.Controls.Add(this.radioButtonBurstTime);
            this.groupBox1.Controls.Add(this.radioButtonTankPoints);
            this.groupBox1.Controls.Add(this.labelThreatScale);
            this.groupBox1.Controls.Add(this.trackBarThreatScale);
            this.groupBox1.Controls.Add(this.labelMitigationScale);
            this.groupBox1.Controls.Add(this.labelThreatScaleText);
            this.groupBox1.Controls.Add(this.trackBarMitigationScale);
            this.groupBox1.Controls.Add(this.radioButtonMitigationScale);
            this.groupBox1.Location = new System.Drawing.Point(3, 245);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(364, 199);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Ranking System";
            // 
            // extendedToolTipDamageOutput
            // 
            this.extendedToolTipDamageOutput.Location = new System.Drawing.Point(108, 176);
            this.extendedToolTipDamageOutput.Name = "extendedToolTipDamageOutput";
            this.extendedToolTipDamageOutput.Size = new System.Drawing.Size(96, 18);
            this.extendedToolTipDamageOutput.TabIndex = 14;
            this.extendedToolTipDamageOutput.Text = "Damage Output *";
            this.extendedToolTipDamageOutput.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.extendedToolTipDamageOutput.ToolTipText = "Scale based only on potential DPS output.";
            this.extendedToolTipDamageOutput.Click += new System.EventHandler(this.extendedToolTipDamageOutput_Click);
            // 
            // radioButtonDamageOutput
            // 
            this.radioButtonDamageOutput.AutoSize = true;
            this.radioButtonDamageOutput.Location = new System.Drawing.Point(95, 179);
            this.radioButtonDamageOutput.Name = "radioButtonDamageOutput";
            this.radioButtonDamageOutput.Size = new System.Drawing.Size(14, 13);
            this.radioButtonDamageOutput.TabIndex = 13;
            this.radioButtonDamageOutput.UseVisualStyleBackColor = true;
            // 
            // extendedToolTipBurstTime
            // 
            this.extendedToolTipBurstTime.Location = new System.Drawing.Point(108, 158);
            this.extendedToolTipBurstTime.Name = "extendedToolTipBurstTime";
            this.extendedToolTipBurstTime.Size = new System.Drawing.Size(96, 16);
            this.extendedToolTipBurstTime.TabIndex = 12;
            this.extendedToolTipBurstTime.Text = "Burst Time *";
            this.extendedToolTipBurstTime.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.extendedToolTipBurstTime.ToolTipText = "Scale based on the average time an event will occur which has a chance to burst d" +
                "own the player.";
            this.extendedToolTipBurstTime.Click += new System.EventHandler(this.extendedToolTipBurstTime_Click);
            // 
            // extendedToolTipMitigtionScale
            // 
            this.extendedToolTipMitigtionScale.Location = new System.Drawing.Point(108, 120);
            this.extendedToolTipMitigtionScale.Name = "extendedToolTipMitigtionScale";
            this.extendedToolTipMitigtionScale.Size = new System.Drawing.Size(96, 15);
            this.extendedToolTipMitigtionScale.TabIndex = 11;
            this.extendedToolTipMitigtionScale.Text = "Mitigation Scale *";
            this.extendedToolTipMitigtionScale.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.extendedToolTipMitigtionScale.ToolTipText = "Customizable scale that allows you to weight mitigation vs. effective health. (De" +
                "fault)";
            this.extendedToolTipMitigtionScale.Click += new System.EventHandler(this.extendedToolTipMitigtionScale_Click);
            // 
            // extendedToolTipTankPoints
            // 
            this.extendedToolTipTankPoints.Location = new System.Drawing.Point(108, 141);
            this.extendedToolTipTankPoints.Name = "extendedToolTipTankPoints";
            this.extendedToolTipTankPoints.Size = new System.Drawing.Size(96, 13);
            this.extendedToolTipTankPoints.TabIndex = 9;
            this.extendedToolTipTankPoints.Text = "TankPoints *";
            this.extendedToolTipTankPoints.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.extendedToolTipTankPoints.ToolTipText = "Scale based on the average amount of unmitigated base damage needed to kill the p" +
                "layer.";
            this.extendedToolTipTankPoints.Click += new System.EventHandler(this.extendedToolTipTankPoints_Click);
            // 
            // radioButtonBurstTime
            // 
            this.radioButtonBurstTime.AutoSize = true;
            this.radioButtonBurstTime.Location = new System.Drawing.Point(95, 160);
            this.radioButtonBurstTime.Name = "radioButtonBurstTime";
            this.radioButtonBurstTime.Size = new System.Drawing.Size(14, 13);
            this.radioButtonBurstTime.TabIndex = 10;
            this.radioButtonBurstTime.UseVisualStyleBackColor = true;
            this.radioButtonBurstTime.CheckedChanged += new System.EventHandler(this.radioButton_CheckedChanged);
            // 
            // radioButtonTankPoints
            // 
            this.radioButtonTankPoints.AutoSize = true;
            this.radioButtonTankPoints.Location = new System.Drawing.Point(95, 141);
            this.radioButtonTankPoints.Name = "radioButtonTankPoints";
            this.radioButtonTankPoints.Size = new System.Drawing.Size(14, 13);
            this.radioButtonTankPoints.TabIndex = 9;
            this.radioButtonTankPoints.UseVisualStyleBackColor = true;
            this.radioButtonTankPoints.CheckedChanged += new System.EventHandler(this.radioButton_CheckedChanged);
            // 
            // radioButtonMitigationScale
            // 
            this.radioButtonMitigationScale.AutoSize = true;
            this.radioButtonMitigationScale.Checked = true;
            this.radioButtonMitigationScale.Location = new System.Drawing.Point(95, 122);
            this.radioButtonMitigationScale.Name = "radioButtonMitigationScale";
            this.radioButtonMitigationScale.Size = new System.Drawing.Size(14, 13);
            this.radioButtonMitigationScale.TabIndex = 8;
            this.radioButtonMitigationScale.TabStop = true;
            this.radioButtonMitigationScale.UseVisualStyleBackColor = true;
            this.radioButtonMitigationScale.CheckedChanged += new System.EventHandler(this.radioButton_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.extendedToolTipUseParryHaste);
            this.groupBox2.Controls.Add(this.labelBossSpeed);
            this.groupBox2.Controls.Add(this.checkBoxUseParryHaste);
            this.groupBox2.Controls.Add(this.labelBossAttackSpeed);
            this.groupBox2.Controls.Add(this.trackBarBossAttackSpeed);
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
            this.groupBox2.Size = new System.Drawing.Size(364, 236);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Attacker Stats";
            // 
            // extendedToolTipUseParryHaste
            // 
            this.extendedToolTipUseParryHaste.Location = new System.Drawing.Point(111, 144);
            this.extendedToolTipUseParryHaste.Name = "extendedToolTipUseParryHaste";
            this.extendedToolTipUseParryHaste.Size = new System.Drawing.Size(134, 14);
            this.extendedToolTipUseParryHaste.TabIndex = 8;
            this.extendedToolTipUseParryHaste.Text = "Use Parry Haste *";
            this.extendedToolTipUseParryHaste.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.extendedToolTipUseParryHaste.ToolTipText = "Calculates the adjusted attacker speed based on parry hasting. May not be applica" +
                "ble on all bosses. (e.g. Patchwerk does not parry haste.)";
            // 
            // labelBossSpeed
            // 
            this.labelBossSpeed.Location = new System.Drawing.Point(3, 94);
            this.labelBossSpeed.Name = "labelBossSpeed";
            this.labelBossSpeed.Size = new System.Drawing.Size(83, 45);
            this.labelBossSpeed.TabIndex = 4;
            this.labelBossSpeed.Text = "Attack Speed: * (Default: 2.00s)";
            this.labelBossSpeed.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.labelBossSpeed.ToolTipText = "How often (in seconds) the boss attacks with the damage above.";
            // 
            // checkBoxUseParryHaste
            // 
            this.checkBoxUseParryHaste.AutoSize = true;
            this.checkBoxUseParryHaste.Location = new System.Drawing.Point(95, 145);
            this.checkBoxUseParryHaste.Name = "checkBoxUseParryHaste";
            this.checkBoxUseParryHaste.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.checkBoxUseParryHaste.Size = new System.Drawing.Size(15, 14);
            this.checkBoxUseParryHaste.TabIndex = 7;
            this.checkBoxUseParryHaste.UseVisualStyleBackColor = true;
            this.checkBoxUseParryHaste.CheckedChanged += new System.EventHandler(this.checkBoxUseParryHaste_CheckedChanged);
            // 
            // labelBossAttackSpeed
            // 
            this.labelBossAttackSpeed.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.labelBossAttackSpeed.AutoSize = true;
            this.labelBossAttackSpeed.Location = new System.Drawing.Point(92, 126);
            this.labelBossAttackSpeed.Name = "labelBossAttackSpeed";
            this.labelBossAttackSpeed.Size = new System.Drawing.Size(28, 13);
            this.labelBossAttackSpeed.TabIndex = 3;
            this.labelBossAttackSpeed.Text = "2.00";
            // 
            // trackBarBossAttackSpeed
            // 
            this.trackBarBossAttackSpeed.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.trackBarBossAttackSpeed.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.trackBarBossAttackSpeed.LargeChange = 4;
            this.trackBarBossAttackSpeed.Location = new System.Drawing.Point(86, 94);
            this.trackBarBossAttackSpeed.Maximum = 20;
            this.trackBarBossAttackSpeed.Minimum = 1;
            this.trackBarBossAttackSpeed.Name = "trackBarBossAttackSpeed";
            this.trackBarBossAttackSpeed.Size = new System.Drawing.Size(272, 45);
            this.trackBarBossAttackSpeed.TabIndex = 5;
            this.trackBarBossAttackSpeed.Value = 8;
            this.trackBarBossAttackSpeed.ValueChanged += new System.EventHandler(this.calculationOptionControl_Changed);
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.labelVigilanceThreat);
            this.groupBox3.Controls.Add(this.labelVigilanceValue);
            this.groupBox3.Controls.Add(this.trackBarVigilanceValue);
            this.groupBox3.Controls.Add(this.extendedToolTipUseVigilance);
            this.groupBox3.Controls.Add(this.checkBoxUseVigilance);
            this.groupBox3.Location = new System.Drawing.Point(3, 450);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(364, 88);
            this.groupBox3.TabIndex = 6;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Vigilance";
            this.groupBox3.UseCompatibleTextRendering = true;
            // 
            // labelVigilanceThreat
            // 
            this.labelVigilanceThreat.Location = new System.Drawing.Point(6, 34);
            this.labelVigilanceThreat.Name = "labelVigilanceThreat";
            this.labelVigilanceThreat.Size = new System.Drawing.Size(83, 45);
            this.labelVigilanceThreat.TabIndex = 12;
            this.labelVigilanceThreat.Text = "Target TPS: * (Default: 5000)";
            this.labelVigilanceThreat.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.labelVigilanceThreat.ToolTipText = "Base friendly target TPS to use.";
            // 
            // labelVigilanceValue
            // 
            this.labelVigilanceValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.labelVigilanceValue.AutoSize = true;
            this.labelVigilanceValue.Location = new System.Drawing.Point(95, 66);
            this.labelVigilanceValue.Name = "labelVigilanceValue";
            this.labelVigilanceValue.Size = new System.Drawing.Size(31, 13);
            this.labelVigilanceValue.TabIndex = 11;
            this.labelVigilanceValue.Text = "5000";
            // 
            // trackBarVigilanceValue
            // 
            this.trackBarVigilanceValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.trackBarVigilanceValue.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.trackBarVigilanceValue.LargeChange = 5000;
            this.trackBarVigilanceValue.Location = new System.Drawing.Point(89, 34);
            this.trackBarVigilanceValue.Maximum = 15000;
            this.trackBarVigilanceValue.Minimum = 1000;
            this.trackBarVigilanceValue.Name = "trackBarVigilanceValue";
            this.trackBarVigilanceValue.Size = new System.Drawing.Size(272, 45);
            this.trackBarVigilanceValue.SmallChange = 500;
            this.trackBarVigilanceValue.TabIndex = 13;
            this.trackBarVigilanceValue.TickFrequency = 500;
            this.trackBarVigilanceValue.Value = 5000;
            this.trackBarVigilanceValue.ValueChanged += new System.EventHandler(this.calculationOptionControl_Changed);
            // 
            // extendedToolTipUseVigilance
            // 
            this.extendedToolTipUseVigilance.Location = new System.Drawing.Point(111, 16);
            this.extendedToolTipUseVigilance.Name = "extendedToolTipUseVigilance";
            this.extendedToolTipUseVigilance.Size = new System.Drawing.Size(134, 14);
            this.extendedToolTipUseVigilance.TabIndex = 10;
            this.extendedToolTipUseVigilance.Text = "Use Vigilance Threat *";
            this.extendedToolTipUseVigilance.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.extendedToolTipUseVigilance.ToolTipText = "Adds Vigilance threat to the total threat generation statistics if available.";
            // 
            // checkBoxUseVigilance
            // 
            this.checkBoxUseVigilance.AutoSize = true;
            this.checkBoxUseVigilance.Checked = true;
            this.checkBoxUseVigilance.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxUseVigilance.Location = new System.Drawing.Point(95, 17);
            this.checkBoxUseVigilance.Name = "checkBoxUseVigilance";
            this.checkBoxUseVigilance.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.checkBoxUseVigilance.Size = new System.Drawing.Size(15, 14);
            this.checkBoxUseVigilance.TabIndex = 9;
            this.checkBoxUseVigilance.UseVisualStyleBackColor = true;
            this.checkBoxUseVigilance.CheckedChanged += new System.EventHandler(this.checkBoxUseVigilance_CheckedChanged);
            // 
            // CalculationOptionsPanelProtWarr
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBoxWarriorSkills);
            this.Name = "CalculationOptionsPanelProtWarr";
            this.Size = new System.Drawing.Size(370, 639);
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
            ((System.ComponentModel.ISupportInitialize)(this.trackBarBossAttackSpeed)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarVigilanceValue)).EndInit();
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
		private System.Windows.Forms.TrackBar trackBarMitigationScale;
		private System.Windows.Forms.Label labelMitigationScale;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label labelShieldBlockUptime;
        private System.Windows.Forms.TrackBar trackBarShieldBlockUptime;
        private System.Windows.Forms.CheckBox checkBoxUseShieldBlock;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private Rawr.CustomControls.ExtendedToolTipLabel labelBossSpeed;
        private System.Windows.Forms.Label labelBossAttackSpeed;
        private System.Windows.Forms.TrackBar trackBarBossAttackSpeed;
        private Rawr.CustomControls.ExtendedToolTipLabel extendedToolTipUseParryHaste;
        private System.Windows.Forms.CheckBox checkBoxUseParryHaste;
        private System.Windows.Forms.RadioButton radioButtonMitigationScale;
        private System.Windows.Forms.RadioButton radioButtonBurstTime;
        private System.Windows.Forms.RadioButton radioButtonTankPoints;
        private Rawr.CustomControls.ExtendedToolTipLabel extendedToolTipTankPoints;
        private Rawr.CustomControls.ExtendedToolTipLabel extendedToolTipBurstTime;
        private Rawr.CustomControls.ExtendedToolTipLabel extendedToolTipMitigtionScale;
        private System.Windows.Forms.GroupBox groupBox3;
        private Rawr.CustomControls.ExtendedToolTipLabel extendedToolTipDamageOutput;
        private System.Windows.Forms.RadioButton radioButtonDamageOutput;
        private Rawr.CustomControls.ExtendedToolTipLabel extendedToolTipUseVigilance;
        private System.Windows.Forms.CheckBox checkBoxUseVigilance;
        private Rawr.CustomControls.ExtendedToolTipLabel labelVigilanceThreat;
        private System.Windows.Forms.Label labelVigilanceValue;
        private System.Windows.Forms.TrackBar trackBarVigilanceValue;
	}
}
