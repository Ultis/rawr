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
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxTargetLevel = new System.Windows.Forms.ComboBox();
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
            this.checkBoxUseParryHaste = new System.Windows.Forms.CheckBox();
            this.labelBossAttackSpeed = new System.Windows.Forms.Label();
            this.trackBarBossAttackSpeed = new System.Windows.Forms.TrackBar();
            this.groupBoxGlyphs = new System.Windows.Forms.GroupBox();
            this.checkBoxGlyphOfExorcism = new System.Windows.Forms.CheckBox();
            this.checkBoxGlyphOfDivinePlea = new System.Windows.Forms.CheckBox();
            this.checkBoxGlyphOfSealOfVengeance = new System.Windows.Forms.CheckBox();
            this.checkBoxGlyphOfJudgement = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarTargetArmor)).BeginInit();
            this.groupBoxPaladinSkills.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarBossAttackValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarThreatScale)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarMitigationScale)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarBossAttackSpeed)).BeginInit();
            this.groupBoxGlyphs.SuspendLayout();
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
            this.labelTargetArmorDescription.Text = "13100: Tier 7 Bosses";
            // 
            // groupBoxPaladinSkills
            // 
            this.groupBoxPaladinSkills.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxPaladinSkills.Controls.Add(this.radioButtonSoR);
            this.groupBoxPaladinSkills.Controls.Add(this.radioButtonSoV);
            this.groupBoxPaladinSkills.Location = new System.Drawing.Point(3, 450);
            this.groupBoxPaladinSkills.Name = "groupBoxPaladinSkills";
            this.groupBoxPaladinSkills.Size = new System.Drawing.Size(364, 73);
            this.groupBoxPaladinSkills.TabIndex = 4;
            this.groupBoxPaladinSkills.TabStop = false;
            this.groupBoxPaladinSkills.Text = "Seal Choice";
            // 
            // radioButtonSoR
            // 
            this.radioButtonSoR.Location = new System.Drawing.Point(95, 37);
            this.radioButtonSoR.Name = "radioButtonSoR";
            this.radioButtonSoR.Size = new System.Drawing.Size(216, 24);
            this.radioButtonSoR.TabIndex = 3;
            this.radioButtonSoR.TabStop = true;
            this.radioButtonSoR.Text = "Seal of Righteousness";
            this.radioButtonSoR.UseVisualStyleBackColor = true;
            // 
            // radioButtonSoV
            // 
            this.radioButtonSoV.Checked = true;
            this.radioButtonSoV.Location = new System.Drawing.Point(95, 18);
            this.radioButtonSoV.Name = "radioButtonSoV";
            this.radioButtonSoV.Size = new System.Drawing.Size(216, 24);
            this.radioButtonSoV.TabIndex = 2;
            this.radioButtonSoV.TabStop = true;
            this.radioButtonSoV.Text = "Seal of Vengeance";
            this.radioButtonSoV.UseVisualStyleBackColor = true;
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
            // groupBoxGlyphs
            // 
            this.groupBoxGlyphs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxGlyphs.Controls.Add(this.checkBoxGlyphOfExorcism);
            this.groupBoxGlyphs.Controls.Add(this.checkBoxGlyphOfDivinePlea);
            this.groupBoxGlyphs.Controls.Add(this.checkBoxGlyphOfSealOfVengeance);
            this.groupBoxGlyphs.Controls.Add(this.checkBoxGlyphOfJudgement);
            this.groupBoxGlyphs.Location = new System.Drawing.Point(3, 529);
            this.groupBoxGlyphs.Name = "groupBoxGlyphs";
            this.groupBoxGlyphs.Size = new System.Drawing.Size(364, 115);
            this.groupBoxGlyphs.TabIndex = 6;
            this.groupBoxGlyphs.TabStop = false;
            this.groupBoxGlyphs.Text = "Paladin Glyphs";
            // 
            // checkBoxGlyphOfExorcism
            // 
            this.checkBoxGlyphOfExorcism.Location = new System.Drawing.Point(95, 63);
            this.checkBoxGlyphOfExorcism.Name = "checkBoxGlyphOfExorcism";
            this.checkBoxGlyphOfExorcism.Size = new System.Drawing.Size(216, 24);
            this.checkBoxGlyphOfExorcism.TabIndex = 11;
            this.checkBoxGlyphOfExorcism.Text = "Glyph of Exorcism";
            this.checkBoxGlyphOfExorcism.UseVisualStyleBackColor = true;
            this.checkBoxGlyphOfExorcism.CheckedChanged += new System.EventHandler(this.checkBoxGlyphOfExorcism_CheckedChanged);
            // 
            // checkBoxGlyphOfDivinePlea
            // 
            this.checkBoxGlyphOfDivinePlea.Location = new System.Drawing.Point(95, 85);
            this.checkBoxGlyphOfDivinePlea.Name = "checkBoxGlyphOfDivinePlea";
            this.checkBoxGlyphOfDivinePlea.Size = new System.Drawing.Size(216, 24);
            this.checkBoxGlyphOfDivinePlea.TabIndex = 10;
            this.checkBoxGlyphOfDivinePlea.Text = "Glyph of Divine Plea";
            this.checkBoxGlyphOfDivinePlea.UseVisualStyleBackColor = true;
            this.checkBoxGlyphOfDivinePlea.CheckedChanged += new System.EventHandler(this.checkBoxGlyphOfDivinePlea_CheckedChanged);
            // 
            // checkBoxGlyphOfSealOfVengeance
            // 
            this.checkBoxGlyphOfSealOfVengeance.Location = new System.Drawing.Point(95, 41);
            this.checkBoxGlyphOfSealOfVengeance.Name = "checkBoxGlyphOfSealOfVengeance";
            this.checkBoxGlyphOfSealOfVengeance.Size = new System.Drawing.Size(216, 24);
            this.checkBoxGlyphOfSealOfVengeance.TabIndex = 9;
            this.checkBoxGlyphOfSealOfVengeance.Text = "Glyph of Seal of Vengeance";
            this.checkBoxGlyphOfSealOfVengeance.UseVisualStyleBackColor = true;
            this.checkBoxGlyphOfSealOfVengeance.CheckedChanged += new System.EventHandler(this.checkBoxGlyphOfSealOfVengeance_CheckedChanged);
            // 
            // checkBoxGlyphOfJudgement
            // 
            this.checkBoxGlyphOfJudgement.Location = new System.Drawing.Point(95, 19);
            this.checkBoxGlyphOfJudgement.Name = "checkBoxGlyphOfJudgement";
            this.checkBoxGlyphOfJudgement.Size = new System.Drawing.Size(216, 24);
            this.checkBoxGlyphOfJudgement.TabIndex = 8;
            this.checkBoxGlyphOfJudgement.Text = "Glyph of Judgement";
            this.checkBoxGlyphOfJudgement.UseVisualStyleBackColor = true;
            this.checkBoxGlyphOfJudgement.CheckedChanged += new System.EventHandler(this.checkBoxGlyphOfJudgement_CheckedChanged);
            // 
            // CalculationOptionsPanelProtPaladin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.groupBoxGlyphs);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBoxPaladinSkills);
            this.Name = "CalculationOptionsPanelProtPaladin";
            this.Size = new System.Drawing.Size(370, 718);
            ((System.ComponentModel.ISupportInitialize)(this.trackBarTargetArmor)).EndInit();
            this.groupBoxPaladinSkills.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.trackBarBossAttackValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarThreatScale)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarMitigationScale)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarBossAttackSpeed)).EndInit();
            this.groupBoxGlyphs.ResumeLayout(false);
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox comboBoxTargetLevel;
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
        private System.Windows.Forms.GroupBox groupBoxGlyphs;
        private Rawr.CustomControls.ExtendedToolTipLabel extendedToolTipDamageOutput;
        private System.Windows.Forms.RadioButton radioButtonDamageOutput;
        private System.Windows.Forms.RadioButton radioButtonSoR;
        private System.Windows.Forms.RadioButton radioButtonSoV;
        private System.Windows.Forms.CheckBox checkBoxGlyphOfSealOfVengeance;
        private System.Windows.Forms.CheckBox checkBoxGlyphOfJudgement;
        private System.Windows.Forms.CheckBox checkBoxGlyphOfExorcism;
        private System.Windows.Forms.CheckBox checkBoxGlyphOfDivinePlea;
	}
}
