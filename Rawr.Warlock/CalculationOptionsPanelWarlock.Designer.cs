namespace Rawr.Warlock
{
    partial class CalculationOptionsPanelWarlock
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
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxTargetLevel = new System.Windows.Forms.ComboBox();
            this.checkBoxEnforceMetagemRequirements = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxLatency = new System.Windows.Forms.TextBox();
            this.comboBoxFillerSpell = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBoxCastedCurse = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.comboBoxPet = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.checkBoxPetSacrificed = new System.Windows.Forms.CheckBox();
            this.buttonTalents = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.checkBoxCastImmolate = new System.Windows.Forms.CheckBox();
            this.checkBoxCastCorruption = new System.Windows.Forms.CheckBox();
            this.checkBoxCastSiphonLife = new System.Windows.Forms.CheckBox();
            this.checkBoxCastUnstableAffliction = new System.Windows.Forms.CheckBox();
            this.checkBoxCastConflagrate = new System.Windows.Forms.CheckBox();
            this.checkBoxCastShadowburn = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.textBoxFightDuration = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.toolTipWarlock = new System.Windows.Forms.ToolTip(this.components);
            this.label8 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.textBoxDotGap = new System.Windows.Forms.TextBox();
            this.textBoxAfflictionDebuffs = new System.Windows.Forms.TextBox();
            this.textBoxShadowPriestDps = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonIsbRaid = new System.Windows.Forms.Button();
            this.textBoxIsbCustom = new System.Windows.Forms.TextBox();
            this.radioButtonIsbRaid = new System.Windows.Forms.RadioButton();
            this.radioButtonIsbCustom = new System.Windows.Forms.RadioButton();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 82);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Target Level: ";
            // 
            // comboBoxTargetLevel
            // 
            this.comboBoxTargetLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxTargetLevel.FormattingEnabled = true;
            this.comboBoxTargetLevel.Items.AddRange(new object[] {
            "70",
            "71",
            "72",
            "73"});
            this.comboBoxTargetLevel.Location = new System.Drawing.Point(117, 79);
            this.comboBoxTargetLevel.Name = "comboBoxTargetLevel";
            this.comboBoxTargetLevel.Size = new System.Drawing.Size(88, 21);
            this.comboBoxTargetLevel.TabIndex = 1;
            this.comboBoxTargetLevel.SelectedIndexChanged += new System.EventHandler(this.comboBoxTargetLevel_SelectedIndexChanged);
            // 
            // checkBoxEnforceMetagemRequirements
            // 
            this.checkBoxEnforceMetagemRequirements.AutoSize = true;
            this.checkBoxEnforceMetagemRequirements.Location = new System.Drawing.Point(13, 32);
            this.checkBoxEnforceMetagemRequirements.Name = "checkBoxEnforceMetagemRequirements";
            this.checkBoxEnforceMetagemRequirements.Size = new System.Drawing.Size(178, 17);
            this.checkBoxEnforceMetagemRequirements.TabIndex = 2;
            this.checkBoxEnforceMetagemRequirements.Text = "Enforce Metagem Requirements";
            this.checkBoxEnforceMetagemRequirements.UseVisualStyleBackColor = true;
            this.checkBoxEnforceMetagemRequirements.CheckedChanged += new System.EventHandler(this.checkBoxEnforceMetagemRequirements_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 56);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Latency: *";
            this.toolTipWarlock.SetToolTip(this.label3, "Effective latency in seconds. The value should include the benefit of built-in /s" +
                    "topcasting.");
            // 
            // textBoxLatency
            // 
            this.textBoxLatency.Location = new System.Drawing.Point(117, 53);
            this.textBoxLatency.Name = "textBoxLatency";
            this.textBoxLatency.Size = new System.Drawing.Size(88, 20);
            this.textBoxLatency.TabIndex = 7;
            this.textBoxLatency.Leave += new System.EventHandler(this.textBoxLatency_Leave);
            // 
            // comboBoxFillerSpell
            // 
            this.comboBoxFillerSpell.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxFillerSpell.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxFillerSpell.FormattingEnabled = true;
            this.comboBoxFillerSpell.Items.AddRange(new object[] {
            "Shadowbolt",
            "Incinerate"});
            this.comboBoxFillerSpell.Location = new System.Drawing.Point(114, 19);
            this.comboBoxFillerSpell.Name = "comboBoxFillerSpell";
            this.comboBoxFillerSpell.Size = new System.Drawing.Size(88, 21);
            this.comboBoxFillerSpell.TabIndex = 9;
            this.comboBoxFillerSpell.SelectedIndexChanged += new System.EventHandler(this.comboBoxFillerSpell_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Filler Spell:";
            // 
            // comboBoxCastedCurse
            // 
            this.comboBoxCastedCurse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxCastedCurse.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxCastedCurse.FormattingEnabled = true;
            this.comboBoxCastedCurse.Items.AddRange(new object[] {
            "CoA",
            "CoD",
            "CoE",
            "CoS",
            "CoR",
            "CoW",
            "CoT"});
            this.comboBoxCastedCurse.Location = new System.Drawing.Point(114, 46);
            this.comboBoxCastedCurse.Name = "comboBoxCastedCurse";
            this.comboBoxCastedCurse.Size = new System.Drawing.Size(88, 21);
            this.comboBoxCastedCurse.TabIndex = 11;
            this.comboBoxCastedCurse.SelectedIndexChanged += new System.EventHandler(this.comboBoxCastedCurse_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 49);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(73, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Casted Curse:";
            // 
            // comboBoxPet
            // 
            this.comboBoxPet.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.comboBoxPet.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxPet.FormattingEnabled = true;
            this.comboBoxPet.Items.AddRange(new object[] {
            "Succubus",
            "Felhunter",
            "Imp",
            "Voidwalker"});
            this.comboBoxPet.Location = new System.Drawing.Point(114, 19);
            this.comboBoxPet.Name = "comboBoxPet";
            this.comboBoxPet.Size = new System.Drawing.Size(88, 21);
            this.comboBoxPet.TabIndex = 13;
            this.comboBoxPet.SelectedIndexChanged += new System.EventHandler(this.comboBoxPet_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 22);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(26, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "Pet:";
            // 
            // checkBoxPetSacrificed
            // 
            this.checkBoxPetSacrificed.AutoSize = true;
            this.checkBoxPetSacrificed.Location = new System.Drawing.Point(10, 46);
            this.checkBoxPetSacrificed.Name = "checkBoxPetSacrificed";
            this.checkBoxPetSacrificed.Size = new System.Drawing.Size(73, 17);
            this.checkBoxPetSacrificed.TabIndex = 14;
            this.checkBoxPetSacrificed.Text = "Sacrificed";
            this.checkBoxPetSacrificed.UseVisualStyleBackColor = true;
            this.checkBoxPetSacrificed.CheckedChanged += new System.EventHandler(this.checkBoxPetSacrificed_CheckedChanged);
            // 
            // buttonTalents
            // 
            this.buttonTalents.Location = new System.Drawing.Point(13, 3);
            this.buttonTalents.Name = "buttonTalents";
            this.buttonTalents.Size = new System.Drawing.Size(183, 23);
            this.buttonTalents.TabIndex = 20;
            this.buttonTalents.Text = "Talents";
            this.buttonTalents.UseVisualStyleBackColor = true;
            this.buttonTalents.Click += new System.EventHandler(this.buttonTalents_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.comboBoxFillerSpell);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.comboBoxCastedCurse);
            this.groupBox2.Controls.Add(this.checkBoxCastImmolate);
            this.groupBox2.Controls.Add(this.checkBoxCastCorruption);
            this.groupBox2.Controls.Add(this.checkBoxCastSiphonLife);
            this.groupBox2.Controls.Add(this.checkBoxCastUnstableAffliction);
            this.groupBox2.Location = new System.Drawing.Point(3, 220);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(208, 119);
            this.groupBox2.TabIndex = 16;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Spell Choice";
            // 
            // checkBoxCastImmolate
            // 
            this.checkBoxCastImmolate.AutoSize = true;
            this.checkBoxCastImmolate.Location = new System.Drawing.Point(10, 73);
            this.checkBoxCastImmolate.Name = "checkBoxCastImmolate";
            this.checkBoxCastImmolate.Size = new System.Drawing.Size(68, 17);
            this.checkBoxCastImmolate.TabIndex = 12;
            this.checkBoxCastImmolate.Text = "Immolate";
            this.checkBoxCastImmolate.UseVisualStyleBackColor = true;
            this.checkBoxCastImmolate.CheckedChanged += new System.EventHandler(this.checkBoxCastImmolate_CheckedChanged);
            // 
            // checkBoxCastCorruption
            // 
            this.checkBoxCastCorruption.AutoSize = true;
            this.checkBoxCastCorruption.Location = new System.Drawing.Point(123, 73);
            this.checkBoxCastCorruption.Name = "checkBoxCastCorruption";
            this.checkBoxCastCorruption.Size = new System.Drawing.Size(74, 17);
            this.checkBoxCastCorruption.TabIndex = 13;
            this.checkBoxCastCorruption.Text = "Corruption";
            this.checkBoxCastCorruption.UseVisualStyleBackColor = true;
            this.checkBoxCastCorruption.CheckedChanged += new System.EventHandler(this.checkBoxCastCorruption_CheckedChanged);
            // 
            // checkBoxCastSiphonLife
            // 
            this.checkBoxCastSiphonLife.AutoSize = true;
            this.checkBoxCastSiphonLife.Location = new System.Drawing.Point(123, 96);
            this.checkBoxCastSiphonLife.Name = "checkBoxCastSiphonLife";
            this.checkBoxCastSiphonLife.Size = new System.Drawing.Size(79, 17);
            this.checkBoxCastSiphonLife.TabIndex = 14;
            this.checkBoxCastSiphonLife.Text = "Siphon Life";
            this.checkBoxCastSiphonLife.UseVisualStyleBackColor = true;
            this.checkBoxCastSiphonLife.CheckedChanged += new System.EventHandler(this.checkBoxCastSiphonLife_CheckedChanged);
            // 
            // checkBoxCastUnstableAffliction
            // 
            this.checkBoxCastUnstableAffliction.AutoSize = true;
            this.checkBoxCastUnstableAffliction.Location = new System.Drawing.Point(10, 96);
            this.checkBoxCastUnstableAffliction.Name = "checkBoxCastUnstableAffliction";
            this.checkBoxCastUnstableAffliction.Size = new System.Drawing.Size(111, 17);
            this.checkBoxCastUnstableAffliction.TabIndex = 15;
            this.checkBoxCastUnstableAffliction.Text = "Unstable Affliction";
            this.checkBoxCastUnstableAffliction.UseVisualStyleBackColor = true;
            this.checkBoxCastUnstableAffliction.CheckedChanged += new System.EventHandler(this.checkBoxCastUnstableAffliction_CheckedChanged);
            // 
            // checkBoxCastConflagrate
            // 
            this.checkBoxCastConflagrate.AutoSize = true;
            this.checkBoxCastConflagrate.Location = new System.Drawing.Point(126, 500);
            this.checkBoxCastConflagrate.Name = "checkBoxCastConflagrate";
            this.checkBoxCastConflagrate.Size = new System.Drawing.Size(80, 17);
            this.checkBoxCastConflagrate.TabIndex = 17;
            this.checkBoxCastConflagrate.Text = "Conflagrate";
            this.checkBoxCastConflagrate.UseVisualStyleBackColor = true;
            this.checkBoxCastConflagrate.Visible = false;
            this.checkBoxCastConflagrate.CheckedChanged += new System.EventHandler(this.checkBoxCastConflagrate_CheckedChanged);
            // 
            // checkBoxCastShadowburn
            // 
            this.checkBoxCastShadowburn.AutoSize = true;
            this.checkBoxCastShadowburn.Location = new System.Drawing.Point(13, 500);
            this.checkBoxCastShadowburn.Name = "checkBoxCastShadowburn";
            this.checkBoxCastShadowburn.Size = new System.Drawing.Size(86, 17);
            this.checkBoxCastShadowburn.TabIndex = 16;
            this.checkBoxCastShadowburn.Text = "Shadowburn";
            this.checkBoxCastShadowburn.UseVisualStyleBackColor = true;
            this.checkBoxCastShadowburn.Visible = false;
            this.checkBoxCastShadowburn.CheckedChanged += new System.EventHandler(this.checkBoxCastShadowburn_CheckedChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.comboBoxPet);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.checkBoxPetSacrificed);
            this.groupBox3.Location = new System.Drawing.Point(3, 345);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(208, 69);
            this.groupBox3.TabIndex = 17;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Pet Options";
            // 
            // textBoxFightDuration
            // 
            this.textBoxFightDuration.Location = new System.Drawing.Point(117, 106);
            this.textBoxFightDuration.Name = "textBoxFightDuration";
            this.textBoxFightDuration.Size = new System.Drawing.Size(88, 20);
            this.textBoxFightDuration.TabIndex = 19;
            this.textBoxFightDuration.Leave += new System.EventHandler(this.textBoxFightDuration_Leave);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(10, 109);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(90, 13);
            this.label9.TabIndex = 18;
            this.label9.Text = "Fight Duration (s):";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(10, 135);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(104, 13);
            this.label8.TabIndex = 27;
            this.label8.Text = "Average DoT Gap: *";
            this.toolTipWarlock.SetToolTip(this.label8, "Average damage over time gap");
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(10, 161);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(97, 13);
            this.label10.TabIndex = 28;
            this.label10.Text = "Affliction Debuffs: *";
            this.toolTipWarlock.SetToolTip(this.label10, "Number of Affliction debuffs on the target (for Soul Siphon)");
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(10, 187);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(110, 13);
            this.label6.TabIndex = 30;
            this.label6.Text = "Shadow Priest DPS: *";
            this.toolTipWarlock.SetToolTip(this.label6, "SP DPS for Vampiric Touch");
            // 
            // textBoxDotGap
            // 
            this.textBoxDotGap.Location = new System.Drawing.Point(117, 132);
            this.textBoxDotGap.Name = "textBoxDotGap";
            this.textBoxDotGap.Size = new System.Drawing.Size(88, 20);
            this.textBoxDotGap.TabIndex = 25;
            this.textBoxDotGap.Leave += new System.EventHandler(this.textBoxDotGap_Leave);
            // 
            // textBoxAfflictionDebuffs
            // 
            this.textBoxAfflictionDebuffs.Location = new System.Drawing.Point(117, 158);
            this.textBoxAfflictionDebuffs.Name = "textBoxAfflictionDebuffs";
            this.textBoxAfflictionDebuffs.Size = new System.Drawing.Size(88, 20);
            this.textBoxAfflictionDebuffs.TabIndex = 26;
            this.textBoxAfflictionDebuffs.Leave += new System.EventHandler(this.textBoxAfflictionDebuffs_Leave);
            // 
            // textBoxShadowPriestDps
            // 
            this.textBoxShadowPriestDps.Location = new System.Drawing.Point(117, 184);
            this.textBoxShadowPriestDps.Name = "textBoxShadowPriestDps";
            this.textBoxShadowPriestDps.Size = new System.Drawing.Size(88, 20);
            this.textBoxShadowPriestDps.TabIndex = 29;
            this.textBoxShadowPriestDps.Leave += new System.EventHandler(this.textBoxShadowPriestDps_Leave);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.buttonIsbRaid);
            this.groupBox1.Controls.Add(this.textBoxIsbCustom);
            this.groupBox1.Controls.Add(this.radioButtonIsbRaid);
            this.groupBox1.Controls.Add(this.radioButtonIsbCustom);
            this.groupBox1.Location = new System.Drawing.Point(3, 420);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(208, 74);
            this.groupBox1.TabIndex = 31;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Improved Shadow Bolt";
            // 
            // buttonIsbRaid
            // 
            this.buttonIsbRaid.Location = new System.Drawing.Point(114, 45);
            this.buttonIsbRaid.Name = "buttonIsbRaid";
            this.buttonIsbRaid.Size = new System.Drawing.Size(88, 23);
            this.buttonIsbRaid.TabIndex = 31;
            this.buttonIsbRaid.Text = "Shadow Users";
            this.buttonIsbRaid.UseVisualStyleBackColor = true;
            this.buttonIsbRaid.Click += new System.EventHandler(this.buttonIsbRaid_Click);
            // 
            // textBoxIsbCustom
            // 
            this.textBoxIsbCustom.Location = new System.Drawing.Point(114, 19);
            this.textBoxIsbCustom.Name = "textBoxIsbCustom";
            this.textBoxIsbCustom.Size = new System.Drawing.Size(88, 20);
            this.textBoxIsbCustom.TabIndex = 30;
            this.textBoxIsbCustom.Leave += new System.EventHandler(this.textBoxIsbCustom_Leave);
            // 
            // radioButtonIsbRaid
            // 
            this.radioButtonIsbRaid.AutoSize = true;
            this.radioButtonIsbRaid.Location = new System.Drawing.Point(10, 48);
            this.radioButtonIsbRaid.Name = "radioButtonIsbRaid";
            this.radioButtonIsbRaid.Size = new System.Drawing.Size(47, 17);
            this.radioButtonIsbRaid.TabIndex = 1;
            this.radioButtonIsbRaid.TabStop = true;
            this.radioButtonIsbRaid.Text = "Raid";
            this.radioButtonIsbRaid.UseVisualStyleBackColor = true;
            this.radioButtonIsbRaid.CheckedChanged += new System.EventHandler(this.radioButtonIsbRaid_CheckedChanged);
            // 
            // radioButtonIsbCustom
            // 
            this.radioButtonIsbCustom.AutoSize = true;
            this.radioButtonIsbCustom.Location = new System.Drawing.Point(10, 20);
            this.radioButtonIsbCustom.Name = "radioButtonIsbCustom";
            this.radioButtonIsbCustom.Size = new System.Drawing.Size(60, 17);
            this.radioButtonIsbCustom.TabIndex = 0;
            this.radioButtonIsbCustom.TabStop = true;
            this.radioButtonIsbCustom.Text = "Custom";
            this.radioButtonIsbCustom.UseVisualStyleBackColor = true;
            this.radioButtonIsbCustom.CheckedChanged += new System.EventHandler(this.radioButtonIsbCustom_CheckedChanged);
            // 
            // CalculationOptionsPanelWarlock
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.checkBoxCastConflagrate);
            this.Controls.Add(this.checkBoxCastShadowburn);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.textBoxShadowPriestDps);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.textBoxAfflictionDebuffs);
            this.Controls.Add(this.textBoxDotGap);
            this.Controls.Add(this.buttonTalents);
            this.Controls.Add(this.textBoxFightDuration);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.textBoxLatency);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.checkBoxEnforceMetagemRequirements);
            this.Controls.Add(this.comboBoxTargetLevel);
            this.Controls.Add(this.label1);
            this.Name = "CalculationOptionsPanelWarlock";
            this.Size = new System.Drawing.Size(218, 651);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox comboBoxTargetLevel;
        private System.Windows.Forms.CheckBox checkBoxEnforceMetagemRequirements;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxLatency;
        private System.Windows.Forms.ComboBox comboBoxFillerSpell;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBoxCastedCurse;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboBoxPet;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox checkBoxPetSacrificed;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox checkBoxCastUnstableAffliction;
        private System.Windows.Forms.CheckBox checkBoxCastSiphonLife;
        private System.Windows.Forms.CheckBox checkBoxCastCorruption;
        private System.Windows.Forms.CheckBox checkBoxCastImmolate;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox textBoxFightDuration;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button buttonTalents;
        private System.Windows.Forms.ToolTip toolTipWarlock;
        private System.Windows.Forms.TextBox textBoxDotGap;
        private System.Windows.Forms.TextBox textBoxAfflictionDebuffs;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.CheckBox checkBoxCastShadowburn;
        private System.Windows.Forms.CheckBox checkBoxCastConflagrate;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBoxShadowPriestDps;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButtonIsbRaid;
        private System.Windows.Forms.RadioButton radioButtonIsbCustom;
        private System.Windows.Forms.Button buttonIsbRaid;
        private System.Windows.Forms.TextBox textBoxIsbCustom;
	}
}
