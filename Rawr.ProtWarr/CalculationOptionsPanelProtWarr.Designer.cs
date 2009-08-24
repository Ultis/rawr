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
            this.LB_TargetLevel = new System.Windows.Forms.Label();
            this.CB_TargetLevel = new System.Windows.Forms.ComboBox();
            this.GB_WarriorSkills = new System.Windows.Forms.GroupBox();
            this.CB_UseShieldBlock = new System.Windows.Forms.CheckBox();
            this.LB_ShieldBlockUptimeValue = new System.Windows.Forms.Label();
            this.LB_ShieldBlockUptime = new System.Windows.Forms.Label();
            this.Bar_ShieldBlockUptime = new System.Windows.Forms.TrackBar();
            this.LB_BossAttack = new Rawr.CustomControls.ExtendedToolTipLabel();
            this.Bar_BossAttackValue = new System.Windows.Forms.TrackBar();
            this.LB_BossAttackValue = new System.Windows.Forms.Label();
            this.LB_ThreatScaleText = new Rawr.CustomControls.ExtendedToolTipLabel();
            this.Bar_ThreatScale = new System.Windows.Forms.TrackBar();
            this.LB_ThreatScale = new System.Windows.Forms.Label();
            this.Bar_MitigationScale = new System.Windows.Forms.TrackBar();
            this.LB_MitigationScaleValue = new System.Windows.Forms.Label();
            this.GB_Ranking = new System.Windows.Forms.GroupBox();
            this.LB_DamageOutput = new Rawr.CustomControls.ExtendedToolTipLabel();
            this.RB_DamageOutput = new System.Windows.Forms.RadioButton();
            this.LB_BurstTime = new Rawr.CustomControls.ExtendedToolTipLabel();
            this.LB_MitigtionScale = new Rawr.CustomControls.ExtendedToolTipLabel();
            this.LB_TankPoints = new Rawr.CustomControls.ExtendedToolTipLabel();
            this.RB_BurstTime = new System.Windows.Forms.RadioButton();
            this.RB_TankPoints = new System.Windows.Forms.RadioButton();
            this.RB_MitigationScale = new System.Windows.Forms.RadioButton();
            this.GB_AttackerStats = new System.Windows.Forms.GroupBox();
            this.LB_TargArmor = new System.Windows.Forms.Label();
            this.CB_TargetArmor = new System.Windows.Forms.ComboBox();
            this.LB_UseParryHaste = new Rawr.CustomControls.ExtendedToolTipLabel();
            this.LB_BossSpeed = new Rawr.CustomControls.ExtendedToolTipLabel();
            this.CB_UseParryHaste = new System.Windows.Forms.CheckBox();
            this.LB_BossAttackSpeedValue = new System.Windows.Forms.Label();
            this.Bar_BossAttackSpeed = new System.Windows.Forms.TrackBar();
            this.GB_Vigilance = new System.Windows.Forms.GroupBox();
            this.LB_VigilanceThreat = new Rawr.CustomControls.ExtendedToolTipLabel();
            this.LB_VigilanceValue = new System.Windows.Forms.Label();
            this.Bar_VigilanceValue = new System.Windows.Forms.TrackBar();
            this.LB_UseVigilance = new Rawr.CustomControls.ExtendedToolTipLabel();
            this.CB_UseVigilance = new System.Windows.Forms.CheckBox();
            this.GB_WarriorSkills.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Bar_ShieldBlockUptime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Bar_BossAttackValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Bar_ThreatScale)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Bar_MitigationScale)).BeginInit();
            this.GB_Ranking.SuspendLayout();
            this.GB_AttackerStats.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Bar_BossAttackSpeed)).BeginInit();
            this.GB_Vigilance.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Bar_VigilanceValue)).BeginInit();
            this.SuspendLayout();
            // 
            // LB_TargetLevel
            // 
            this.LB_TargetLevel.AutoSize = true;
            this.LB_TargetLevel.Location = new System.Drawing.Point(6, 16);
            this.LB_TargetLevel.Name = "LB_TargetLevel";
            this.LB_TargetLevel.Size = new System.Drawing.Size(73, 13);
            this.LB_TargetLevel.TabIndex = 0;
            this.LB_TargetLevel.Text = "Target Level: ";
            // 
            // CB_TargetLevel
            // 
            this.CB_TargetLevel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.CB_TargetLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CB_TargetLevel.FormattingEnabled = true;
            this.CB_TargetLevel.Items.AddRange(new object[] {
            "80",
            "81",
            "82",
            "83"});
            this.CB_TargetLevel.Location = new System.Drawing.Point(86, 13);
            this.CB_TargetLevel.Name = "CB_TargetLevel";
            this.CB_TargetLevel.Size = new System.Drawing.Size(161, 21);
            this.CB_TargetLevel.TabIndex = 1;
            this.CB_TargetLevel.SelectedIndexChanged += new System.EventHandler(this.calculationOptionControl_Changed);
            // 
            // GB_WarriorSkills
            // 
            this.GB_WarriorSkills.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.GB_WarriorSkills.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.GB_WarriorSkills.Controls.Add(this.CB_UseShieldBlock);
            this.GB_WarriorSkills.Controls.Add(this.LB_ShieldBlockUptimeValue);
            this.GB_WarriorSkills.Controls.Add(this.LB_ShieldBlockUptime);
            this.GB_WarriorSkills.Controls.Add(this.Bar_ShieldBlockUptime);
            this.GB_WarriorSkills.Location = new System.Drawing.Point(3, 502);
            this.GB_WarriorSkills.Name = "GB_WarriorSkills";
            this.GB_WarriorSkills.Size = new System.Drawing.Size(254, 91);
            this.GB_WarriorSkills.TabIndex = 4;
            this.GB_WarriorSkills.TabStop = false;
            this.GB_WarriorSkills.Text = "Warrior Skills";
            this.GB_WarriorSkills.Visible = false;
            // 
            // CB_UseShieldBlock
            // 
            this.CB_UseShieldBlock.AutoSize = true;
            this.CB_UseShieldBlock.Enabled = false;
            this.CB_UseShieldBlock.Location = new System.Drawing.Point(86, 68);
            this.CB_UseShieldBlock.Name = "CB_UseShieldBlock";
            this.CB_UseShieldBlock.Size = new System.Drawing.Size(107, 17);
            this.CB_UseShieldBlock.TabIndex = 5;
            this.CB_UseShieldBlock.Text = "Use Shield Block";
            this.CB_UseShieldBlock.UseVisualStyleBackColor = true;
            this.CB_UseShieldBlock.CheckedChanged += new System.EventHandler(this.checkBoxUseShieldBlock_CheckedChanged);
            // 
            // LB_ShieldBlockUptimeValue
            // 
            this.LB_ShieldBlockUptimeValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.LB_ShieldBlockUptimeValue.AutoSize = true;
            this.LB_ShieldBlockUptimeValue.Location = new System.Drawing.Point(92, 49);
            this.LB_ShieldBlockUptimeValue.Name = "LB_ShieldBlockUptimeValue";
            this.LB_ShieldBlockUptimeValue.Size = new System.Drawing.Size(33, 13);
            this.LB_ShieldBlockUptimeValue.TabIndex = 0;
            this.LB_ShieldBlockUptimeValue.Text = "100%";
            // 
            // LB_ShieldBlockUptime
            // 
            this.LB_ShieldBlockUptime.Location = new System.Drawing.Point(3, 17);
            this.LB_ShieldBlockUptime.Name = "LB_ShieldBlockUptime";
            this.LB_ShieldBlockUptime.Size = new System.Drawing.Size(83, 45);
            this.LB_ShieldBlockUptime.TabIndex = 0;
            this.LB_ShieldBlockUptime.Text = "Shield Block Uptime %:";
            this.LB_ShieldBlockUptime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Bar_ShieldBlockUptime
            // 
            this.Bar_ShieldBlockUptime.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.Bar_ShieldBlockUptime.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.Bar_ShieldBlockUptime.Enabled = false;
            this.Bar_ShieldBlockUptime.LargeChange = 10;
            this.Bar_ShieldBlockUptime.Location = new System.Drawing.Point(86, 17);
            this.Bar_ShieldBlockUptime.Maximum = 100;
            this.Bar_ShieldBlockUptime.Name = "Bar_ShieldBlockUptime";
            this.Bar_ShieldBlockUptime.Size = new System.Drawing.Size(161, 45);
            this.Bar_ShieldBlockUptime.TabIndex = 2;
            this.Bar_ShieldBlockUptime.TickFrequency = 10;
            this.Bar_ShieldBlockUptime.Value = 100;
            this.Bar_ShieldBlockUptime.ValueChanged += new System.EventHandler(this.calculationOptionControl_Changed);
            // 
            // LB_BossAttack
            // 
            this.LB_BossAttack.Location = new System.Drawing.Point(3, 67);
            this.LB_BossAttack.Name = "LB_BossAttack";
            this.LB_BossAttack.Size = new System.Drawing.Size(83, 45);
            this.LB_BossAttack.TabIndex = 0;
            this.LB_BossAttack.Text = "Base Attack: * (Default: 60000)";
            this.LB_BossAttack.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.LB_BossAttack.ToolTipText = "Base attacker damage before armor.";
            // 
            // Bar_BossAttackValue
            // 
            this.Bar_BossAttackValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.Bar_BossAttackValue.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.Bar_BossAttackValue.LargeChange = 5000;
            this.Bar_BossAttackValue.Location = new System.Drawing.Point(86, 67);
            this.Bar_BossAttackValue.Maximum = 100000;
            this.Bar_BossAttackValue.Minimum = 500;
            this.Bar_BossAttackValue.Name = "Bar_BossAttackValue";
            this.Bar_BossAttackValue.Size = new System.Drawing.Size(161, 45);
            this.Bar_BossAttackValue.SmallChange = 500;
            this.Bar_BossAttackValue.TabIndex = 2;
            this.Bar_BossAttackValue.TickFrequency = 5000;
            this.Bar_BossAttackValue.Value = 60000;
            this.Bar_BossAttackValue.ValueChanged += new System.EventHandler(this.calculationOptionControl_Changed);
            // 
            // LB_BossAttackValue
            // 
            this.LB_BossAttackValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.LB_BossAttackValue.AutoSize = true;
            this.LB_BossAttackValue.Location = new System.Drawing.Point(92, 99);
            this.LB_BossAttackValue.Name = "LB_BossAttackValue";
            this.LB_BossAttackValue.Size = new System.Drawing.Size(37, 13);
            this.LB_BossAttackValue.TabIndex = 0;
            this.LB_BossAttackValue.Text = "60000";
            // 
            // LB_ThreatScaleText
            // 
            this.LB_ThreatScaleText.Location = new System.Drawing.Point(6, 16);
            this.LB_ThreatScaleText.Name = "LB_ThreatScaleText";
            this.LB_ThreatScaleText.Size = new System.Drawing.Size(80, 45);
            this.LB_ThreatScaleText.TabIndex = 0;
            this.LB_ThreatScaleText.Text = "Threat Scale: * (Default: 1.0)";
            this.LB_ThreatScaleText.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.LB_ThreatScaleText.ToolTipText = "Threat scaling factor. PageUp/PageDown/Left Arrow/Right Arrow allows more accurat" +
                "e changes";
            // 
            // Bar_ThreatScale
            // 
            this.Bar_ThreatScale.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.Bar_ThreatScale.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.Bar_ThreatScale.Location = new System.Drawing.Point(86, 16);
            this.Bar_ThreatScale.Maximum = 30;
            this.Bar_ThreatScale.Name = "Bar_ThreatScale";
            this.Bar_ThreatScale.Size = new System.Drawing.Size(161, 45);
            this.Bar_ThreatScale.TabIndex = 2;
            this.Bar_ThreatScale.Value = 10;
            this.Bar_ThreatScale.ValueChanged += new System.EventHandler(this.calculationOptionControl_Changed);
            // 
            // LB_ThreatScale
            // 
            this.LB_ThreatScale.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.LB_ThreatScale.AutoSize = true;
            this.LB_ThreatScale.Location = new System.Drawing.Point(92, 48);
            this.LB_ThreatScale.Name = "LB_ThreatScale";
            this.LB_ThreatScale.Size = new System.Drawing.Size(22, 13);
            this.LB_ThreatScale.TabIndex = 0;
            this.LB_ThreatScale.Text = "1.0";
            // 
            // Bar_MitigationScale
            // 
            this.Bar_MitigationScale.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.Bar_MitigationScale.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.Bar_MitigationScale.Location = new System.Drawing.Point(86, 67);
            this.Bar_MitigationScale.Maximum = 30;
            this.Bar_MitigationScale.Name = "Bar_MitigationScale";
            this.Bar_MitigationScale.Size = new System.Drawing.Size(161, 45);
            this.Bar_MitigationScale.TabIndex = 2;
            this.Bar_MitigationScale.Value = 10;
            this.Bar_MitigationScale.ValueChanged += new System.EventHandler(this.calculationOptionControl_Changed);
            // 
            // LB_MitigationScaleValue
            // 
            this.LB_MitigationScaleValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.LB_MitigationScaleValue.AutoSize = true;
            this.LB_MitigationScaleValue.Location = new System.Drawing.Point(92, 99);
            this.LB_MitigationScaleValue.Name = "LB_MitigationScaleValue";
            this.LB_MitigationScaleValue.Size = new System.Drawing.Size(22, 13);
            this.LB_MitigationScaleValue.TabIndex = 0;
            this.LB_MitigationScaleValue.Text = "1.0";
            // 
            // GB_Ranking
            // 
            this.GB_Ranking.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.GB_Ranking.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.GB_Ranking.Controls.Add(this.LB_DamageOutput);
            this.GB_Ranking.Controls.Add(this.RB_DamageOutput);
            this.GB_Ranking.Controls.Add(this.LB_BurstTime);
            this.GB_Ranking.Controls.Add(this.LB_MitigtionScale);
            this.GB_Ranking.Controls.Add(this.LB_TankPoints);
            this.GB_Ranking.Controls.Add(this.RB_BurstTime);
            this.GB_Ranking.Controls.Add(this.RB_TankPoints);
            this.GB_Ranking.Controls.Add(this.LB_ThreatScale);
            this.GB_Ranking.Controls.Add(this.Bar_ThreatScale);
            this.GB_Ranking.Controls.Add(this.LB_MitigationScaleValue);
            this.GB_Ranking.Controls.Add(this.LB_ThreatScaleText);
            this.GB_Ranking.Controls.Add(this.Bar_MitigationScale);
            this.GB_Ranking.Controls.Add(this.RB_MitigationScale);
            this.GB_Ranking.Location = new System.Drawing.Point(3, 203);
            this.GB_Ranking.Name = "GB_Ranking";
            this.GB_Ranking.Size = new System.Drawing.Size(254, 199);
            this.GB_Ranking.TabIndex = 5;
            this.GB_Ranking.TabStop = false;
            this.GB_Ranking.Text = "Ranking System";
            // 
            // LB_DamageOutput
            // 
            this.LB_DamageOutput.Location = new System.Drawing.Point(108, 176);
            this.LB_DamageOutput.Name = "LB_DamageOutput";
            this.LB_DamageOutput.Size = new System.Drawing.Size(96, 18);
            this.LB_DamageOutput.TabIndex = 14;
            this.LB_DamageOutput.Text = "Damage Output *";
            this.LB_DamageOutput.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.LB_DamageOutput.ToolTipText = "Scale based only on potential DPS output.";
            this.LB_DamageOutput.Click += new System.EventHandler(this.extendedToolTipDamageOutput_Click);
            // 
            // RB_DamageOutput
            // 
            this.RB_DamageOutput.AutoSize = true;
            this.RB_DamageOutput.Location = new System.Drawing.Point(95, 179);
            this.RB_DamageOutput.Name = "RB_DamageOutput";
            this.RB_DamageOutput.Size = new System.Drawing.Size(14, 13);
            this.RB_DamageOutput.TabIndex = 13;
            this.RB_DamageOutput.UseVisualStyleBackColor = true;
            // 
            // LB_BurstTime
            // 
            this.LB_BurstTime.Location = new System.Drawing.Point(108, 158);
            this.LB_BurstTime.Name = "LB_BurstTime";
            this.LB_BurstTime.Size = new System.Drawing.Size(96, 16);
            this.LB_BurstTime.TabIndex = 12;
            this.LB_BurstTime.Text = "Burst Time *";
            this.LB_BurstTime.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.LB_BurstTime.ToolTipText = "Scale based on the average time an event will occur which has a chance to burst d" +
                "own the player.";
            this.LB_BurstTime.Click += new System.EventHandler(this.extendedToolTipBurstTime_Click);
            // 
            // LB_MitigtionScale
            // 
            this.LB_MitigtionScale.Location = new System.Drawing.Point(108, 120);
            this.LB_MitigtionScale.Name = "LB_MitigtionScale";
            this.LB_MitigtionScale.Size = new System.Drawing.Size(96, 15);
            this.LB_MitigtionScale.TabIndex = 11;
            this.LB_MitigtionScale.Text = "Mitigation Scale *";
            this.LB_MitigtionScale.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.LB_MitigtionScale.ToolTipText = "Customizable scale that allows you to weight mitigation vs. effective health. (De" +
                "fault)";
            this.LB_MitigtionScale.Click += new System.EventHandler(this.extendedToolTipMitigtionScale_Click);
            // 
            // LB_TankPoints
            // 
            this.LB_TankPoints.Location = new System.Drawing.Point(108, 141);
            this.LB_TankPoints.Name = "LB_TankPoints";
            this.LB_TankPoints.Size = new System.Drawing.Size(96, 13);
            this.LB_TankPoints.TabIndex = 9;
            this.LB_TankPoints.Text = "TankPoints *";
            this.LB_TankPoints.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.LB_TankPoints.ToolTipText = "Scale based on the average amount of unmitigated base damage needed to kill the p" +
                "layer.";
            this.LB_TankPoints.Click += new System.EventHandler(this.extendedToolTipTankPoints_Click);
            // 
            // RB_BurstTime
            // 
            this.RB_BurstTime.AutoSize = true;
            this.RB_BurstTime.Location = new System.Drawing.Point(95, 160);
            this.RB_BurstTime.Name = "RB_BurstTime";
            this.RB_BurstTime.Size = new System.Drawing.Size(14, 13);
            this.RB_BurstTime.TabIndex = 10;
            this.RB_BurstTime.UseVisualStyleBackColor = true;
            this.RB_BurstTime.CheckedChanged += new System.EventHandler(this.radioButton_CheckedChanged);
            // 
            // RB_TankPoints
            // 
            this.RB_TankPoints.AutoSize = true;
            this.RB_TankPoints.Location = new System.Drawing.Point(95, 141);
            this.RB_TankPoints.Name = "RB_TankPoints";
            this.RB_TankPoints.Size = new System.Drawing.Size(14, 13);
            this.RB_TankPoints.TabIndex = 9;
            this.RB_TankPoints.UseVisualStyleBackColor = true;
            this.RB_TankPoints.CheckedChanged += new System.EventHandler(this.radioButton_CheckedChanged);
            // 
            // RB_MitigationScale
            // 
            this.RB_MitigationScale.AutoSize = true;
            this.RB_MitigationScale.Checked = true;
            this.RB_MitigationScale.Location = new System.Drawing.Point(95, 122);
            this.RB_MitigationScale.Name = "RB_MitigationScale";
            this.RB_MitigationScale.Size = new System.Drawing.Size(14, 13);
            this.RB_MitigationScale.TabIndex = 8;
            this.RB_MitigationScale.TabStop = true;
            this.RB_MitigationScale.UseVisualStyleBackColor = true;
            this.RB_MitigationScale.CheckedChanged += new System.EventHandler(this.radioButton_CheckedChanged);
            // 
            // GB_AttackerStats
            // 
            this.GB_AttackerStats.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.GB_AttackerStats.Controls.Add(this.LB_TargArmor);
            this.GB_AttackerStats.Controls.Add(this.CB_TargetArmor);
            this.GB_AttackerStats.Controls.Add(this.LB_UseParryHaste);
            this.GB_AttackerStats.Controls.Add(this.LB_BossSpeed);
            this.GB_AttackerStats.Controls.Add(this.CB_UseParryHaste);
            this.GB_AttackerStats.Controls.Add(this.LB_BossAttackSpeedValue);
            this.GB_AttackerStats.Controls.Add(this.Bar_BossAttackSpeed);
            this.GB_AttackerStats.Controls.Add(this.LB_TargetLevel);
            this.GB_AttackerStats.Controls.Add(this.CB_TargetLevel);
            this.GB_AttackerStats.Controls.Add(this.LB_BossAttack);
            this.GB_AttackerStats.Controls.Add(this.LB_BossAttackValue);
            this.GB_AttackerStats.Controls.Add(this.Bar_BossAttackValue);
            this.GB_AttackerStats.Location = new System.Drawing.Point(3, 3);
            this.GB_AttackerStats.Name = "GB_AttackerStats";
            this.GB_AttackerStats.Size = new System.Drawing.Size(254, 194);
            this.GB_AttackerStats.TabIndex = 7;
            this.GB_AttackerStats.TabStop = false;
            this.GB_AttackerStats.Text = "Attacker Stats";
            // 
            // LB_TargArmor
            // 
            this.LB_TargArmor.AutoSize = true;
            this.LB_TargArmor.Location = new System.Drawing.Point(5, 43);
            this.LB_TargArmor.Name = "LB_TargArmor";
            this.LB_TargArmor.Size = new System.Drawing.Size(71, 13);
            this.LB_TargArmor.TabIndex = 9;
            this.LB_TargArmor.Text = "Target Armor:";
            this.LB_TargArmor.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // CB_TargetArmor
            // 
            this.CB_TargetArmor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.CB_TargetArmor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CB_TargetArmor.FormattingEnabled = true;
            this.CB_TargetArmor.Items.AddRange(new object[] {
            "10643",
            "10338",
            "10034",
            "9729"});
            this.CB_TargetArmor.Location = new System.Drawing.Point(86, 40);
            this.CB_TargetArmor.Name = "CB_TargetArmor";
            this.CB_TargetArmor.Size = new System.Drawing.Size(161, 21);
            this.CB_TargetArmor.TabIndex = 10;
            this.CB_TargetArmor.SelectedIndexChanged += new System.EventHandler(this.calculationOptionControl_Changed);
            // 
            // LB_UseParryHaste
            // 
            this.LB_UseParryHaste.Location = new System.Drawing.Point(101, 168);
            this.LB_UseParryHaste.Name = "LB_UseParryHaste";
            this.LB_UseParryHaste.Size = new System.Drawing.Size(93, 14);
            this.LB_UseParryHaste.TabIndex = 8;
            this.LB_UseParryHaste.Text = "Use Parry Haste *";
            this.LB_UseParryHaste.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.LB_UseParryHaste.ToolTipText = "Calculates the adjusted attacker speed based on parry hasting. May not be applica" +
                "ble on all bosses. (e.g. Patchwerk does not parry haste.)";
            // 
            // LB_BossSpeed
            // 
            this.LB_BossSpeed.Location = new System.Drawing.Point(3, 118);
            this.LB_BossSpeed.Name = "LB_BossSpeed";
            this.LB_BossSpeed.Size = new System.Drawing.Size(83, 45);
            this.LB_BossSpeed.TabIndex = 4;
            this.LB_BossSpeed.Text = "Attack Speed: * (Default: 2.00s)";
            this.LB_BossSpeed.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.LB_BossSpeed.ToolTipText = "How often (in seconds) the boss attacks with the damage above.";
            // 
            // CB_UseParryHaste
            // 
            this.CB_UseParryHaste.AutoSize = true;
            this.CB_UseParryHaste.Location = new System.Drawing.Point(86, 169);
            this.CB_UseParryHaste.Name = "CB_UseParryHaste";
            this.CB_UseParryHaste.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.CB_UseParryHaste.Size = new System.Drawing.Size(15, 14);
            this.CB_UseParryHaste.TabIndex = 7;
            this.CB_UseParryHaste.UseVisualStyleBackColor = true;
            this.CB_UseParryHaste.CheckedChanged += new System.EventHandler(this.checkBoxUseParryHaste_CheckedChanged);
            // 
            // LB_BossAttackSpeedValue
            // 
            this.LB_BossAttackSpeedValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.LB_BossAttackSpeedValue.AutoSize = true;
            this.LB_BossAttackSpeedValue.Location = new System.Drawing.Point(92, 150);
            this.LB_BossAttackSpeedValue.Name = "LB_BossAttackSpeedValue";
            this.LB_BossAttackSpeedValue.Size = new System.Drawing.Size(28, 13);
            this.LB_BossAttackSpeedValue.TabIndex = 3;
            this.LB_BossAttackSpeedValue.Text = "2.00";
            // 
            // Bar_BossAttackSpeed
            // 
            this.Bar_BossAttackSpeed.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.Bar_BossAttackSpeed.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.Bar_BossAttackSpeed.LargeChange = 4;
            this.Bar_BossAttackSpeed.Location = new System.Drawing.Point(86, 118);
            this.Bar_BossAttackSpeed.Maximum = 20;
            this.Bar_BossAttackSpeed.Minimum = 1;
            this.Bar_BossAttackSpeed.Name = "Bar_BossAttackSpeed";
            this.Bar_BossAttackSpeed.Size = new System.Drawing.Size(161, 45);
            this.Bar_BossAttackSpeed.TabIndex = 5;
            this.Bar_BossAttackSpeed.Value = 8;
            this.Bar_BossAttackSpeed.ValueChanged += new System.EventHandler(this.calculationOptionControl_Changed);
            // 
            // GB_Vigilance
            // 
            this.GB_Vigilance.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.GB_Vigilance.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.GB_Vigilance.Controls.Add(this.LB_VigilanceThreat);
            this.GB_Vigilance.Controls.Add(this.LB_VigilanceValue);
            this.GB_Vigilance.Controls.Add(this.Bar_VigilanceValue);
            this.GB_Vigilance.Controls.Add(this.LB_UseVigilance);
            this.GB_Vigilance.Controls.Add(this.CB_UseVigilance);
            this.GB_Vigilance.Location = new System.Drawing.Point(3, 408);
            this.GB_Vigilance.MinimumSize = new System.Drawing.Size(244, 88);
            this.GB_Vigilance.Name = "GB_Vigilance";
            this.GB_Vigilance.Size = new System.Drawing.Size(254, 88);
            this.GB_Vigilance.TabIndex = 6;
            this.GB_Vigilance.TabStop = false;
            this.GB_Vigilance.Text = "Vigilance";
            this.GB_Vigilance.UseCompatibleTextRendering = true;
            // 
            // LB_VigilanceThreat
            // 
            this.LB_VigilanceThreat.Location = new System.Drawing.Point(6, 34);
            this.LB_VigilanceThreat.Name = "LB_VigilanceThreat";
            this.LB_VigilanceThreat.Size = new System.Drawing.Size(83, 45);
            this.LB_VigilanceThreat.TabIndex = 12;
            this.LB_VigilanceThreat.Text = "Target TPS: * (Default: 5000)";
            this.LB_VigilanceThreat.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.LB_VigilanceThreat.ToolTipText = "Base friendly target TPS to use.";
            // 
            // LB_VigilanceValue
            // 
            this.LB_VigilanceValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.LB_VigilanceValue.AutoSize = true;
            this.LB_VigilanceValue.Location = new System.Drawing.Point(95, 66);
            this.LB_VigilanceValue.Name = "LB_VigilanceValue";
            this.LB_VigilanceValue.Size = new System.Drawing.Size(31, 13);
            this.LB_VigilanceValue.TabIndex = 11;
            this.LB_VigilanceValue.Text = "5000";
            // 
            // Bar_VigilanceValue
            // 
            this.Bar_VigilanceValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.Bar_VigilanceValue.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.Bar_VigilanceValue.LargeChange = 5000;
            this.Bar_VigilanceValue.Location = new System.Drawing.Point(86, 34);
            this.Bar_VigilanceValue.Maximum = 15000;
            this.Bar_VigilanceValue.Minimum = 1000;
            this.Bar_VigilanceValue.Name = "Bar_VigilanceValue";
            this.Bar_VigilanceValue.Size = new System.Drawing.Size(161, 45);
            this.Bar_VigilanceValue.SmallChange = 500;
            this.Bar_VigilanceValue.TabIndex = 13;
            this.Bar_VigilanceValue.TickFrequency = 500;
            this.Bar_VigilanceValue.Value = 5000;
            this.Bar_VigilanceValue.ValueChanged += new System.EventHandler(this.calculationOptionControl_Changed);
            // 
            // LB_UseVigilance
            // 
            this.LB_UseVigilance.Location = new System.Drawing.Point(101, 16);
            this.LB_UseVigilance.Name = "LB_UseVigilance";
            this.LB_UseVigilance.Size = new System.Drawing.Size(127, 14);
            this.LB_UseVigilance.TabIndex = 10;
            this.LB_UseVigilance.Text = "Use Vigilance Threat *";
            this.LB_UseVigilance.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.LB_UseVigilance.ToolTipText = "Adds Vigilance threat to the total threat generation statistics if available.";
            // 
            // CB_UseVigilance
            // 
            this.CB_UseVigilance.AutoSize = true;
            this.CB_UseVigilance.Checked = true;
            this.CB_UseVigilance.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CB_UseVigilance.Location = new System.Drawing.Point(86, 17);
            this.CB_UseVigilance.Name = "CB_UseVigilance";
            this.CB_UseVigilance.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.CB_UseVigilance.Size = new System.Drawing.Size(15, 14);
            this.CB_UseVigilance.TabIndex = 9;
            this.CB_UseVigilance.UseVisualStyleBackColor = true;
            this.CB_UseVigilance.CheckedChanged += new System.EventHandler(this.checkBoxUseVigilance_CheckedChanged);
            // 
            // CalculationOptionsPanelProtWarr
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.GB_WarriorSkills);
            this.Controls.Add(this.GB_Vigilance);
            this.Controls.Add(this.GB_Ranking);
            this.Controls.Add(this.GB_AttackerStats);
            this.Name = "CalculationOptionsPanelProtWarr";
            this.Size = new System.Drawing.Size(260, 631);
            this.GB_WarriorSkills.ResumeLayout(false);
            this.GB_WarriorSkills.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Bar_ShieldBlockUptime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Bar_BossAttackValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Bar_ThreatScale)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Bar_MitigationScale)).EndInit();
            this.GB_Ranking.ResumeLayout(false);
            this.GB_Ranking.PerformLayout();
            this.GB_AttackerStats.ResumeLayout(false);
            this.GB_AttackerStats.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Bar_BossAttackSpeed)).EndInit();
            this.GB_Vigilance.ResumeLayout(false);
            this.GB_Vigilance.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Bar_VigilanceValue)).EndInit();
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label LB_TargetLevel;
        private System.Windows.Forms.ComboBox CB_TargetLevel;
		private System.Windows.Forms.GroupBox GB_WarriorSkills;
        private Rawr.CustomControls.ExtendedToolTipLabel LB_BossAttack;
		private System.Windows.Forms.TrackBar Bar_BossAttackValue;
		private System.Windows.Forms.Label LB_BossAttackValue;
        private Rawr.CustomControls.ExtendedToolTipLabel LB_ThreatScaleText;
		private System.Windows.Forms.TrackBar Bar_ThreatScale;
        private System.Windows.Forms.Label LB_ThreatScale;
		private System.Windows.Forms.TrackBar Bar_MitigationScale;
		private System.Windows.Forms.Label LB_MitigationScaleValue;
        private System.Windows.Forms.Label LB_ShieldBlockUptime;
        private System.Windows.Forms.Label LB_ShieldBlockUptimeValue;
        private System.Windows.Forms.TrackBar Bar_ShieldBlockUptime;
        private System.Windows.Forms.CheckBox CB_UseShieldBlock;
        private System.Windows.Forms.GroupBox GB_Ranking;
        private System.Windows.Forms.GroupBox GB_AttackerStats;
        private Rawr.CustomControls.ExtendedToolTipLabel LB_BossSpeed;
        private System.Windows.Forms.Label LB_BossAttackSpeedValue;
        private System.Windows.Forms.TrackBar Bar_BossAttackSpeed;
        private Rawr.CustomControls.ExtendedToolTipLabel LB_UseParryHaste;
        private System.Windows.Forms.CheckBox CB_UseParryHaste;
        private System.Windows.Forms.RadioButton RB_MitigationScale;
        private System.Windows.Forms.RadioButton RB_BurstTime;
        private System.Windows.Forms.RadioButton RB_TankPoints;
        private Rawr.CustomControls.ExtendedToolTipLabel LB_TankPoints;
        private Rawr.CustomControls.ExtendedToolTipLabel LB_BurstTime;
        private Rawr.CustomControls.ExtendedToolTipLabel LB_MitigtionScale;
        private System.Windows.Forms.GroupBox GB_Vigilance;
        private Rawr.CustomControls.ExtendedToolTipLabel LB_DamageOutput;
        private System.Windows.Forms.RadioButton RB_DamageOutput;
        private Rawr.CustomControls.ExtendedToolTipLabel LB_UseVigilance;
        private System.Windows.Forms.CheckBox CB_UseVigilance;
        private Rawr.CustomControls.ExtendedToolTipLabel LB_VigilanceThreat;
        private System.Windows.Forms.Label LB_VigilanceValue;
        private System.Windows.Forms.TrackBar Bar_VigilanceValue;
        public System.Windows.Forms.Label LB_TargArmor;
        public System.Windows.Forms.ComboBox CB_TargetArmor;
	}
}
