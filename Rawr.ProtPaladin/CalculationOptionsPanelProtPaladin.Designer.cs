namespace Rawr.ProtPaladin
{
	partial class CalculationOptionsPanelProtPaladin
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
            this.label2 = new System.Windows.Forms.Label();
            this.trackBarTargetArmor = new System.Windows.Forms.TrackBar();
            this.labelTargetArmorDescription = new System.Windows.Forms.Label();
            this.groupBoxPaladinSkills = new System.Windows.Forms.GroupBox();
            this.radioButtonSoR = new System.Windows.Forms.RadioButton();
            this.radioButtonSoV = new System.Windows.Forms.RadioButton();
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
            this.label5 = new System.Windows.Forms.Label();
            this.checkBoxUseParryHaste = new System.Windows.Forms.CheckBox();
            this.comboBoxTargetType = new System.Windows.Forms.ComboBox();
            this.labelBossAttackSpeed = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.numericUpDownTargetLevel = new System.Windows.Forms.NumericUpDown();
            this.trackBarBossAttackSpeed = new System.Windows.Forms.TrackBar();
            this.groupBoxPaladinAbilities = new System.Windows.Forms.GroupBox();
            this.checkBoxUseHolyShield = new System.Windows.Forms.CheckBox();
            this.Glyphs = new System.Windows.Forms.TabControl();
            this.tabPageTarget = new System.Windows.Forms.TabPage();
            this.tabPageRanking = new System.Windows.Forms.TabPage();
            this.tabPageAbilities = new System.Windows.Forms.TabPage();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarTargetArmor)).BeginInit();
            this.groupBoxPaladinSkills.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarBossAttackValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarThreatScale)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarMitigationScale)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTargetLevel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarBossAttackSpeed)).BeginInit();
            this.groupBoxPaladinAbilities.SuspendLayout();
            this.Glyphs.SuspendLayout();
            this.tabPageTarget.SuspendLayout();
            this.tabPageRanking.SuspendLayout();
            this.tabPageAbilities.SuspendLayout();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(3, 167);
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
            this.trackBarTargetArmor.Location = new System.Drawing.Point(86, 167);
            this.trackBarTargetArmor.Maximum = 20000;
            this.trackBarTargetArmor.Name = "trackBarTargetArmor";
            this.trackBarTargetArmor.Size = new System.Drawing.Size(178, 45);
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
            this.labelTargetArmorDescription.Location = new System.Drawing.Point(92, 204);
            this.labelTargetArmorDescription.Name = "labelTargetArmorDescription";
            this.labelTargetArmorDescription.Size = new System.Drawing.Size(172, 34);
            this.labelTargetArmorDescription.TabIndex = 0;
            this.labelTargetArmorDescription.Text = "13100: Tier 7 Bosses";
            // 
            // groupBoxPaladinSkills
            // 
            this.groupBoxPaladinSkills.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxPaladinSkills.Controls.Add(this.radioButtonSoR);
            this.groupBoxPaladinSkills.Controls.Add(this.radioButtonSoV);
            this.groupBoxPaladinSkills.Location = new System.Drawing.Point(6, 6);
            this.groupBoxPaladinSkills.Name = "groupBoxPaladinSkills";
            this.groupBoxPaladinSkills.Size = new System.Drawing.Size(270, 73);
            this.groupBoxPaladinSkills.TabIndex = 4;
            this.groupBoxPaladinSkills.TabStop = false;
            this.groupBoxPaladinSkills.Text = "Seal Choice";
            // 
            // radioButtonSoR
            // 
            this.radioButtonSoR.Location = new System.Drawing.Point(6, 38);
            this.radioButtonSoR.Name = "radioButtonSoR";
            this.radioButtonSoR.Size = new System.Drawing.Size(216, 24);
            this.radioButtonSoR.TabIndex = 3;
            this.radioButtonSoR.TabStop = true;
            this.radioButtonSoR.Text = "Seal of Righteousness";
            this.radioButtonSoR.UseVisualStyleBackColor = true;
            this.radioButtonSoR.CheckedChanged += new System.EventHandler(this.radioButtonSealChoice_CheckedChanged);
            // 
            // radioButtonSoV
            // 
            this.radioButtonSoV.Checked = true;
            this.radioButtonSoV.Location = new System.Drawing.Point(6, 19);
            this.radioButtonSoV.Name = "radioButtonSoV";
            this.radioButtonSoV.Size = new System.Drawing.Size(216, 24);
            this.radioButtonSoV.TabIndex = 2;
            this.radioButtonSoV.TabStop = true;
            this.radioButtonSoV.Text = "Seal of Vengeance";
            this.radioButtonSoV.UseVisualStyleBackColor = true;
            this.radioButtonSoV.CheckedChanged += new System.EventHandler(this.radioButtonSealChoice_CheckedChanged);
            // 
            // labelBossAttack
            // 
            this.labelBossAttack.Location = new System.Drawing.Point(3, 45);
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
            this.trackBarBossAttackValue.Location = new System.Drawing.Point(86, 45);
            this.trackBarBossAttackValue.Maximum = 100000;
            this.trackBarBossAttackValue.Minimum = 500;
            this.trackBarBossAttackValue.Name = "trackBarBossAttackValue";
            this.trackBarBossAttackValue.Size = new System.Drawing.Size(178, 45);
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
            this.labelBossAttackValue.Location = new System.Drawing.Point(92, 77);
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
            this.trackBarThreatScale.Size = new System.Drawing.Size(178, 45);
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
            this.trackBarMitigationScale.Size = new System.Drawing.Size(178, 45);
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
            this.groupBox1.Location = new System.Drawing.Point(6, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(270, 205);
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
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.checkBoxUseParryHaste);
            this.groupBox2.Controls.Add(this.comboBoxTargetType);
            this.groupBox2.Controls.Add(this.labelBossAttackSpeed);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.numericUpDownTargetLevel);
            this.groupBox2.Controls.Add(this.trackBarBossAttackSpeed);
            this.groupBox2.Controls.Add(this.labelBossAttack);
            this.groupBox2.Controls.Add(this.labelTargetArmorDescription);
            this.groupBox2.Controls.Add(this.labelBossAttackValue);
            this.groupBox2.Controls.Add(this.trackBarTargetArmor);
            this.groupBox2.Controls.Add(this.trackBarBossAttackValue);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Location = new System.Drawing.Point(6, 6);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(270, 231);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Attacker Stats";
            // 
            // extendedToolTipUseParryHaste
            // 
            this.extendedToolTipUseParryHaste.Location = new System.Drawing.Point(111, 146);
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
            this.labelBossSpeed.Location = new System.Drawing.Point(3, 96);
            this.labelBossSpeed.Name = "labelBossSpeed";
            this.labelBossSpeed.Size = new System.Drawing.Size(83, 45);
            this.labelBossSpeed.TabIndex = 4;
            this.labelBossSpeed.Text = "Attack Speed: * (Default: 2.00s)";
            this.labelBossSpeed.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.labelBossSpeed.ToolTipText = "How often (in seconds) the boss attacks with the damage above.";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(135, 21);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(37, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Type: ";
            // 
            // checkBoxUseParryHaste
            // 
            this.checkBoxUseParryHaste.AutoSize = true;
            this.checkBoxUseParryHaste.Location = new System.Drawing.Point(95, 147);
            this.checkBoxUseParryHaste.Name = "checkBoxUseParryHaste";
            this.checkBoxUseParryHaste.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.checkBoxUseParryHaste.Size = new System.Drawing.Size(15, 14);
            this.checkBoxUseParryHaste.TabIndex = 7;
            this.checkBoxUseParryHaste.UseVisualStyleBackColor = true;
            this.checkBoxUseParryHaste.CheckedChanged += new System.EventHandler(this.checkBoxUseParryHaste_CheckedChanged);
            // 
            // comboBoxTargetType
            // 
            this.comboBoxTargetType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxTargetType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxTargetType.FormattingEnabled = true;
            this.comboBoxTargetType.Items.AddRange(new object[] {
            "Unspecified",
            "Humanoid",
            "Undead",
            "Demon",
            "Elemental",
            "Giant",
            "Mechanical",
            "Beast",
            "Dragonkin"});
            this.comboBoxTargetType.Location = new System.Drawing.Point(175, 18);
            this.comboBoxTargetType.Name = "comboBoxTargetType";
            this.comboBoxTargetType.Size = new System.Drawing.Size(89, 21);
            this.comboBoxTargetType.TabIndex = 12;
            this.comboBoxTargetType.SelectedIndexChanged += new System.EventHandler(this.comboBoxTargetType_SelectedIndexChanged);
            // 
            // labelBossAttackSpeed
            // 
            this.labelBossAttackSpeed.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.labelBossAttackSpeed.AutoSize = true;
            this.labelBossAttackSpeed.Location = new System.Drawing.Point(92, 128);
            this.labelBossAttackSpeed.Name = "labelBossAttackSpeed";
            this.labelBossAttackSpeed.Size = new System.Drawing.Size(28, 13);
            this.labelBossAttackSpeed.TabIndex = 3;
            this.labelBossAttackSpeed.Text = "2.00";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 21);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(73, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Target Level: ";
            // 
            // numericUpDownTargetLevel
            // 
            this.numericUpDownTargetLevel.Location = new System.Drawing.Point(80, 19);
            this.numericUpDownTargetLevel.Maximum = new decimal(new int[] {
            83,
            0,
            0,
            0});
            this.numericUpDownTargetLevel.Minimum = new decimal(new int[] {
            80,
            0,
            0,
            0});
            this.numericUpDownTargetLevel.Name = "numericUpDownTargetLevel";
            this.numericUpDownTargetLevel.Size = new System.Drawing.Size(49, 20);
            this.numericUpDownTargetLevel.TabIndex = 8;
            this.numericUpDownTargetLevel.Value = new decimal(new int[] {
            80,
            0,
            0,
            0});
            this.numericUpDownTargetLevel.ValueChanged += new System.EventHandler(this.numericUpDownTargetLevel_ValueChanged);
            // 
            // trackBarBossAttackSpeed
            // 
            this.trackBarBossAttackSpeed.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.trackBarBossAttackSpeed.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.trackBarBossAttackSpeed.LargeChange = 4;
            this.trackBarBossAttackSpeed.Location = new System.Drawing.Point(86, 96);
            this.trackBarBossAttackSpeed.Maximum = 20;
            this.trackBarBossAttackSpeed.Minimum = 1;
            this.trackBarBossAttackSpeed.Name = "trackBarBossAttackSpeed";
            this.trackBarBossAttackSpeed.Size = new System.Drawing.Size(178, 45);
            this.trackBarBossAttackSpeed.TabIndex = 5;
            this.trackBarBossAttackSpeed.Value = 8;
            this.trackBarBossAttackSpeed.ValueChanged += new System.EventHandler(this.calculationOptionControl_Changed);
            // 
            // groupBoxPaladinAbilities
            // 
            this.groupBoxPaladinAbilities.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxPaladinAbilities.Controls.Add(this.checkBoxUseHolyShield);
            this.groupBoxPaladinAbilities.Location = new System.Drawing.Point(6, 85);
            this.groupBoxPaladinAbilities.Name = "groupBoxPaladinAbilities";
            this.groupBoxPaladinAbilities.Size = new System.Drawing.Size(270, 50);
            this.groupBoxPaladinAbilities.TabIndex = 8;
            this.groupBoxPaladinAbilities.TabStop = false;
            this.groupBoxPaladinAbilities.Text = "Paladin Abilities";
            // 
            // checkBoxUseHolyShield
            // 
            this.checkBoxUseHolyShield.AutoSize = true;
            this.checkBoxUseHolyShield.Checked = true;
            this.checkBoxUseHolyShield.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxUseHolyShield.Location = new System.Drawing.Point(6, 19);
            this.checkBoxUseHolyShield.Name = "checkBoxUseHolyShield";
            this.checkBoxUseHolyShield.Size = new System.Drawing.Size(101, 17);
            this.checkBoxUseHolyShield.TabIndex = 0;
            this.checkBoxUseHolyShield.Text = "Use Holy Shield";
            this.checkBoxUseHolyShield.UseVisualStyleBackColor = true;
            this.checkBoxUseHolyShield.CheckedChanged += new System.EventHandler(this.checkBoxUseHolyShield_CheckedChanged);
            // 
            // Glyphs
            // 
            this.Glyphs.Controls.Add(this.tabPageTarget);
            this.Glyphs.Controls.Add(this.tabPageRanking);
            this.Glyphs.Controls.Add(this.tabPageAbilities);
            this.Glyphs.Location = new System.Drawing.Point(6, 6);
            this.Glyphs.Name = "Glyphs";
            this.Glyphs.SelectedIndex = 0;
            this.Glyphs.Size = new System.Drawing.Size(290, 545);
            this.Glyphs.TabIndex = 9;
            // 
            // tabPageTarget
            // 
            this.tabPageTarget.Controls.Add(this.groupBox2);
            this.tabPageTarget.Location = new System.Drawing.Point(4, 22);
            this.tabPageTarget.Name = "tabPageTarget";
            this.tabPageTarget.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageTarget.Size = new System.Drawing.Size(282, 519);
            this.tabPageTarget.TabIndex = 0;
            this.tabPageTarget.Text = "Target";
            this.tabPageTarget.UseVisualStyleBackColor = true;
            // 
            // tabPageRanking
            // 
            this.tabPageRanking.Controls.Add(this.groupBox1);
            this.tabPageRanking.Location = new System.Drawing.Point(4, 22);
            this.tabPageRanking.Name = "tabPageRanking";
            this.tabPageRanking.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageRanking.Size = new System.Drawing.Size(282, 519);
            this.tabPageRanking.TabIndex = 1;
            this.tabPageRanking.Text = "Ranking";
            this.tabPageRanking.UseVisualStyleBackColor = true;
            // 
            // tabPageAbilities
            // 
            this.tabPageAbilities.Controls.Add(this.groupBoxPaladinAbilities);
            this.tabPageAbilities.Controls.Add(this.groupBoxPaladinSkills);
            this.tabPageAbilities.Location = new System.Drawing.Point(4, 22);
            this.tabPageAbilities.Name = "tabPageAbilities";
            this.tabPageAbilities.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageAbilities.Size = new System.Drawing.Size(282, 519);
            this.tabPageAbilities.TabIndex = 2;
            this.tabPageAbilities.Text = "Abilities";
            this.tabPageAbilities.UseVisualStyleBackColor = true;
            // 
            // CalculationOptionsPanelProtPaladin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.Glyphs);
            this.Name = "CalculationOptionsPanelProtPaladin";
            this.Size = new System.Drawing.Size(304, 560);
            ((System.ComponentModel.ISupportInitialize)(this.trackBarTargetArmor)).EndInit();
            this.groupBoxPaladinSkills.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.trackBarBossAttackValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarThreatScale)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarMitigationScale)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTargetLevel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarBossAttackSpeed)).EndInit();
            this.groupBoxPaladinAbilities.ResumeLayout(false);
            this.groupBoxPaladinAbilities.PerformLayout();
            this.Glyphs.ResumeLayout(false);
            this.tabPageTarget.ResumeLayout(false);
            this.tabPageRanking.ResumeLayout(false);
            this.tabPageAbilities.ResumeLayout(false);
            this.ResumeLayout(false);

		}

		#endregion

        private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TrackBar trackBarTargetArmor;
        private System.Windows.Forms.Label labelTargetArmorDescription;
		private System.Windows.Forms.GroupBox groupBoxPaladinSkills;
        private Rawr.CustomControls.ExtendedToolTipLabel labelBossAttack;
		private System.Windows.Forms.TrackBar trackBarBossAttackValue;
		private System.Windows.Forms.Label labelBossAttackValue;
        private Rawr.CustomControls.ExtendedToolTipLabel labelThreatScaleText;
		private System.Windows.Forms.TrackBar trackBarThreatScale;
        private System.Windows.Forms.Label labelThreatScale;
		private System.Windows.Forms.TrackBar trackBarMitigationScale;
        private System.Windows.Forms.Label labelMitigationScale;
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
        private Rawr.CustomControls.ExtendedToolTipLabel extendedToolTipDamageOutput;
        private System.Windows.Forms.RadioButton radioButtonDamageOutput;
        private System.Windows.Forms.RadioButton radioButtonSoR;
        private System.Windows.Forms.RadioButton radioButtonSoV;
        private System.Windows.Forms.GroupBox groupBoxPaladinAbilities;
        private System.Windows.Forms.CheckBox checkBoxUseHolyShield;
        private System.Windows.Forms.TabControl Glyphs;
        private System.Windows.Forms.TabPage tabPageTarget;
        private System.Windows.Forms.TabPage tabPageRanking;
        private System.Windows.Forms.TabPage tabPageAbilities;
        private System.Windows.Forms.NumericUpDown numericUpDownTargetLevel;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox comboBoxTargetType;
	}
}
