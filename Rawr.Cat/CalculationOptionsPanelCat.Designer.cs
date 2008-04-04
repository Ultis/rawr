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
			this.checkBoxEnforceMetagemRequirements = new System.Windows.Forms.CheckBox();
			this.label4 = new System.Windows.Forms.Label();
			this.trackBarExposeWeakness = new System.Windows.Forms.TrackBar();
			this.labelExposeWeakness = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.trackBarBloodlustUptime = new System.Windows.Forms.TrackBar();
			this.labelBloodlustUptime = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.trackBarDrumsOfBattleUptime = new System.Windows.Forms.TrackBar();
			this.labelDrumsOfBattleUptime = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.trackBarDrumsOfWarUptime = new System.Windows.Forms.TrackBar();
			this.labelDrumsOfWarUptime = new System.Windows.Forms.Label();
			this.radioButtonAldor = new System.Windows.Forms.RadioButton();
			this.radioButtonScryer = new System.Windows.Forms.RadioButton();
			((System.ComponentModel.ISupportInitialize)(this.trackBarTargetArmor)).BeginInit();
			this.groupBoxPrimaryAttack.SuspendLayout();
			this.groupBoxFinisher.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.trackBarExposeWeakness)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.trackBarBloodlustUptime)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.trackBarDrumsOfBattleUptime)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.trackBarDrumsOfWarUptime)).BeginInit();
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
			this.label2.Location = new System.Drawing.Point(3, 237);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(74, 13);
			this.label2.TabIndex = 0;
			this.label2.Text = "Target Armor: ";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(3, 328);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(62, 13);
			this.label3.TabIndex = 0;
			this.label3.Text = "Powershift: ";
			// 
			// comboBoxPowershift
			// 
			this.comboBoxPowershift.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
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
			this.comboBoxPowershift.Location = new System.Drawing.Point(83, 325);
			this.comboBoxPowershift.Name = "comboBoxPowershift";
			this.comboBoxPowershift.Size = new System.Drawing.Size(123, 21);
			this.comboBoxPowershift.TabIndex = 1;
			this.comboBoxPowershift.SelectedIndexChanged += new System.EventHandler(this.calculationOptionControl_Changed);
			// 
			// trackBarTargetArmor
			// 
			this.trackBarTargetArmor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.trackBarTargetArmor.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.trackBarTargetArmor.LargeChange = 1000;
			this.trackBarTargetArmor.Location = new System.Drawing.Point(75, 234);
			this.trackBarTargetArmor.Maximum = 9000;
			this.trackBarTargetArmor.Minimum = 3000;
			this.trackBarTargetArmor.Name = "trackBarTargetArmor";
			this.trackBarTargetArmor.Size = new System.Drawing.Size(139, 45);
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
			this.labelTargetArmorDescription.Location = new System.Drawing.Point(6, 282);
			this.labelTargetArmorDescription.Name = "labelTargetArmorDescription";
			this.labelTargetArmorDescription.Size = new System.Drawing.Size(200, 40);
			this.labelTargetArmorDescription.TabIndex = 0;
			this.labelTargetArmorDescription.Text = "7700 Armor: Hydross, Lurker, Leotheras, Tidewalker, Al\'ar, Naj\'entus, Supremus, A" +
				"kama, Gurtogg";
			// 
			// radioButtonMangle
			// 
			this.radioButtonMangle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
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
			this.groupBoxPrimaryAttack.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBoxPrimaryAttack.Controls.Add(this.radioButtonBoth);
			this.groupBoxPrimaryAttack.Controls.Add(this.radioButtonShred);
			this.groupBoxPrimaryAttack.Controls.Add(this.radioButtonMangle);
			this.groupBoxPrimaryAttack.Enabled = false;
			this.groupBoxPrimaryAttack.Location = new System.Drawing.Point(3, 398);
			this.groupBoxPrimaryAttack.Name = "groupBoxPrimaryAttack";
			this.groupBoxPrimaryAttack.Size = new System.Drawing.Size(203, 170);
			this.groupBoxPrimaryAttack.TabIndex = 4;
			this.groupBoxPrimaryAttack.TabStop = false;
			this.groupBoxPrimaryAttack.Text = "Primary Attack - Not Implemented Yet";
			// 
			// radioButtonBoth
			// 
			this.radioButtonBoth.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
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
			this.radioButtonShred.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
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
			this.groupBoxFinisher.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBoxFinisher.Controls.Add(this.radioButtonNone);
			this.groupBoxFinisher.Controls.Add(this.radioButtonFerociousBite);
			this.groupBoxFinisher.Controls.Add(this.radioButtonRip);
			this.groupBoxFinisher.Enabled = false;
			this.groupBoxFinisher.Location = new System.Drawing.Point(3, 574);
			this.groupBoxFinisher.Name = "groupBoxFinisher";
			this.groupBoxFinisher.Size = new System.Drawing.Size(203, 170);
			this.groupBoxFinisher.TabIndex = 4;
			this.groupBoxFinisher.TabStop = false;
			this.groupBoxFinisher.Text = "Finisher - Not Implemented Yet";
			// 
			// radioButtonNone
			// 
			this.radioButtonNone.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
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
			this.radioButtonFerociousBite.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
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
			this.radioButtonRip.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
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
			// checkBoxEnforceMetagemRequirements
			// 
			this.checkBoxEnforceMetagemRequirements.AutoSize = true;
			this.checkBoxEnforceMetagemRequirements.Location = new System.Drawing.Point(6, 352);
			this.checkBoxEnforceMetagemRequirements.Name = "checkBoxEnforceMetagemRequirements";
			this.checkBoxEnforceMetagemRequirements.Size = new System.Drawing.Size(178, 17);
			this.checkBoxEnforceMetagemRequirements.TabIndex = 5;
			this.checkBoxEnforceMetagemRequirements.Text = "Enforce Metagem Requirements";
			this.checkBoxEnforceMetagemRequirements.UseVisualStyleBackColor = true;
			this.checkBoxEnforceMetagemRequirements.CheckedChanged += new System.EventHandler(this.checkBoxEnforceMetagemRequirements_CheckedChanged);
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(16, 33);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(58, 39);
			this.label4.TabIndex = 0;
			this.label4.Text = "Expose\r\nWeakness\r\nAP Bonus:";
			this.label4.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// trackBarExposeWeakness
			// 
			this.trackBarExposeWeakness.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.trackBarExposeWeakness.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.trackBarExposeWeakness.LargeChange = 50;
			this.trackBarExposeWeakness.Location = new System.Drawing.Point(75, 30);
			this.trackBarExposeWeakness.Maximum = 500;
			this.trackBarExposeWeakness.Minimum = 50;
			this.trackBarExposeWeakness.Name = "trackBarExposeWeakness";
			this.trackBarExposeWeakness.Size = new System.Drawing.Size(139, 45);
			this.trackBarExposeWeakness.SmallChange = 10;
			this.trackBarExposeWeakness.TabIndex = 2;
			this.trackBarExposeWeakness.TickFrequency = 25;
			this.trackBarExposeWeakness.Value = 200;
			this.trackBarExposeWeakness.ValueChanged += new System.EventHandler(this.calculationOptionControl_Changed);
			// 
			// labelExposeWeakness
			// 
			this.labelExposeWeakness.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.labelExposeWeakness.AutoSize = true;
			this.labelExposeWeakness.Location = new System.Drawing.Point(80, 62);
			this.labelExposeWeakness.Name = "labelExposeWeakness";
			this.labelExposeWeakness.Size = new System.Drawing.Size(25, 13);
			this.labelExposeWeakness.TabIndex = 0;
			this.labelExposeWeakness.Text = "200";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(16, 84);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(54, 26);
			this.label5.TabIndex = 0;
			this.label5.Text = "Bloodlust\r\nUptime %:";
			this.label5.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// trackBarBloodlustUptime
			// 
			this.trackBarBloodlustUptime.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.trackBarBloodlustUptime.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.trackBarBloodlustUptime.Location = new System.Drawing.Point(75, 81);
			this.trackBarBloodlustUptime.Maximum = 100;
			this.trackBarBloodlustUptime.Minimum = 5;
			this.trackBarBloodlustUptime.Name = "trackBarBloodlustUptime";
			this.trackBarBloodlustUptime.Size = new System.Drawing.Size(139, 45);
			this.trackBarBloodlustUptime.TabIndex = 2;
			this.trackBarBloodlustUptime.TickFrequency = 5;
			this.trackBarBloodlustUptime.Value = 15;
			this.trackBarBloodlustUptime.ValueChanged += new System.EventHandler(this.calculationOptionControl_Changed);
			// 
			// labelBloodlustUptime
			// 
			this.labelBloodlustUptime.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.labelBloodlustUptime.AutoSize = true;
			this.labelBloodlustUptime.Location = new System.Drawing.Point(80, 113);
			this.labelBloodlustUptime.Name = "labelBloodlustUptime";
			this.labelBloodlustUptime.Size = new System.Drawing.Size(27, 13);
			this.labelBloodlustUptime.TabIndex = 0;
			this.labelBloodlustUptime.Text = "15%";
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(16, 135);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(54, 39);
			this.label7.TabIndex = 0;
			this.label7.Text = "Drums\r\nof Battle\r\nUptime %:";
			this.label7.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// trackBarDrumsOfBattleUptime
			// 
			this.trackBarDrumsOfBattleUptime.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.trackBarDrumsOfBattleUptime.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.trackBarDrumsOfBattleUptime.Location = new System.Drawing.Point(75, 132);
			this.trackBarDrumsOfBattleUptime.Maximum = 100;
			this.trackBarDrumsOfBattleUptime.Minimum = 20;
			this.trackBarDrumsOfBattleUptime.Name = "trackBarDrumsOfBattleUptime";
			this.trackBarDrumsOfBattleUptime.Size = new System.Drawing.Size(139, 45);
			this.trackBarDrumsOfBattleUptime.TabIndex = 2;
			this.trackBarDrumsOfBattleUptime.TickFrequency = 5;
			this.trackBarDrumsOfBattleUptime.Value = 25;
			this.trackBarDrumsOfBattleUptime.ValueChanged += new System.EventHandler(this.calculationOptionControl_Changed);
			// 
			// labelDrumsOfBattleUptime
			// 
			this.labelDrumsOfBattleUptime.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.labelDrumsOfBattleUptime.AutoSize = true;
			this.labelDrumsOfBattleUptime.Location = new System.Drawing.Point(80, 164);
			this.labelDrumsOfBattleUptime.Name = "labelDrumsOfBattleUptime";
			this.labelDrumsOfBattleUptime.Size = new System.Drawing.Size(27, 13);
			this.labelDrumsOfBattleUptime.TabIndex = 0;
			this.labelDrumsOfBattleUptime.Text = "25%";
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Location = new System.Drawing.Point(16, 186);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(54, 39);
			this.label9.TabIndex = 0;
			this.label9.Text = "Drums\r\nof War\r\nUptime %:";
			this.label9.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// trackBarDrumsOfWarUptime
			// 
			this.trackBarDrumsOfWarUptime.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.trackBarDrumsOfWarUptime.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.trackBarDrumsOfWarUptime.Location = new System.Drawing.Point(75, 183);
			this.trackBarDrumsOfWarUptime.Maximum = 100;
			this.trackBarDrumsOfWarUptime.Minimum = 20;
			this.trackBarDrumsOfWarUptime.Name = "trackBarDrumsOfWarUptime";
			this.trackBarDrumsOfWarUptime.Size = new System.Drawing.Size(139, 45);
			this.trackBarDrumsOfWarUptime.TabIndex = 2;
			this.trackBarDrumsOfWarUptime.TickFrequency = 5;
			this.trackBarDrumsOfWarUptime.Value = 25;
			this.trackBarDrumsOfWarUptime.ValueChanged += new System.EventHandler(this.calculationOptionControl_Changed);
			// 
			// labelDrumsOfWarUptime
			// 
			this.labelDrumsOfWarUptime.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.labelDrumsOfWarUptime.AutoSize = true;
			this.labelDrumsOfWarUptime.Location = new System.Drawing.Point(80, 215);
			this.labelDrumsOfWarUptime.Name = "labelDrumsOfWarUptime";
			this.labelDrumsOfWarUptime.Size = new System.Drawing.Size(27, 13);
			this.labelDrumsOfWarUptime.TabIndex = 0;
			this.labelDrumsOfWarUptime.Text = "25%";
			// 
			// radioButtonAldor
			// 
			this.radioButtonAldor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.radioButtonAldor.AutoSize = true;
			this.radioButtonAldor.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
			this.radioButtonAldor.Checked = true;
			this.radioButtonAldor.Location = new System.Drawing.Point(7, 375);
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
			this.radioButtonScryer.Location = new System.Drawing.Point(83, 375);
			this.radioButtonScryer.Name = "radioButtonScryer";
			this.radioButtonScryer.Size = new System.Drawing.Size(55, 17);
			this.radioButtonScryer.TabIndex = 3;
			this.radioButtonScryer.Tag = "Shred";
			this.radioButtonScryer.Text = "Scryer";
			this.radioButtonScryer.TextAlign = System.Drawing.ContentAlignment.TopLeft;
			this.radioButtonScryer.UseVisualStyleBackColor = true;
			this.radioButtonScryer.CheckedChanged += new System.EventHandler(this.calculationOptionControl_Changed);
			// 
			// CalculationOptionsPanelCat
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoScroll = true;
			this.Controls.Add(this.checkBoxEnforceMetagemRequirements);
			this.Controls.Add(this.radioButtonScryer);
			this.Controls.Add(this.radioButtonAldor);
			this.Controls.Add(this.groupBoxFinisher);
			this.Controls.Add(this.groupBoxPrimaryAttack);
			this.Controls.Add(this.labelDrumsOfWarUptime);
			this.Controls.Add(this.labelDrumsOfBattleUptime);
			this.Controls.Add(this.labelBloodlustUptime);
			this.Controls.Add(this.labelExposeWeakness);
			this.Controls.Add(this.labelTargetArmorDescription);
			this.Controls.Add(this.trackBarDrumsOfWarUptime);
			this.Controls.Add(this.trackBarDrumsOfBattleUptime);
			this.Controls.Add(this.trackBarBloodlustUptime);
			this.Controls.Add(this.label9);
			this.Controls.Add(this.trackBarExposeWeakness);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.trackBarTargetArmor);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.comboBoxPowershift);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.comboBoxTargetLevel);
			this.Controls.Add(this.label1);
			this.Name = "CalculationOptionsPanelCat";
			this.Size = new System.Drawing.Size(209, 776);
			((System.ComponentModel.ISupportInitialize)(this.trackBarTargetArmor)).EndInit();
			this.groupBoxPrimaryAttack.ResumeLayout(false);
			this.groupBoxFinisher.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.trackBarExposeWeakness)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.trackBarBloodlustUptime)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.trackBarDrumsOfBattleUptime)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.trackBarDrumsOfWarUptime)).EndInit();
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
		private System.Windows.Forms.CheckBox checkBoxEnforceMetagemRequirements;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TrackBar trackBarExposeWeakness;
		private System.Windows.Forms.Label labelExposeWeakness;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TrackBar trackBarBloodlustUptime;
		private System.Windows.Forms.Label labelBloodlustUptime;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.TrackBar trackBarDrumsOfBattleUptime;
		private System.Windows.Forms.Label labelDrumsOfBattleUptime;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.TrackBar trackBarDrumsOfWarUptime;
		private System.Windows.Forms.Label labelDrumsOfWarUptime;
		private System.Windows.Forms.RadioButton radioButtonAldor;
		private System.Windows.Forms.RadioButton radioButtonScryer;
	}
}
