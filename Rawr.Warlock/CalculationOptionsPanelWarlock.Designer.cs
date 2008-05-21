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
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.textBoxFightDuration = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.toolTipWarlock = new System.Windows.Forms.ToolTip(this.components);
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.comboBoxBloodlust = new System.Windows.Forms.ComboBox();
            this.comboBoxDrums = new System.Windows.Forms.ComboBox();
            this.textBoxDotGap = new System.Windows.Forms.TextBox();
            this.textBoxAfflictionDebuffs = new System.Windows.Forms.TextBox();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
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
            this.checkBoxEnforceMetagemRequirements.Enabled = false;
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
            "Felguard",
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
            this.checkBoxPetSacrificed.Location = new System.Drawing.Point(10, 44);
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
            this.groupBox2.Location = new System.Drawing.Point(3, 184);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(208, 169);
            this.groupBox2.TabIndex = 16;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Spell Choice";
            // 
            // checkBoxCastImmolate
            // 
            this.checkBoxCastImmolate.AutoSize = true;
            this.checkBoxCastImmolate.Location = new System.Drawing.Point(10, 74);
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
            this.checkBoxCastCorruption.Location = new System.Drawing.Point(9, 97);
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
            this.checkBoxCastSiphonLife.Location = new System.Drawing.Point(10, 143);
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
            this.checkBoxCastUnstableAffliction.Location = new System.Drawing.Point(10, 120);
            this.checkBoxCastUnstableAffliction.Name = "checkBoxCastUnstableAffliction";
            this.checkBoxCastUnstableAffliction.Size = new System.Drawing.Size(111, 17);
            this.checkBoxCastUnstableAffliction.TabIndex = 15;
            this.checkBoxCastUnstableAffliction.Text = "Unstable Affliction";
            this.checkBoxCastUnstableAffliction.UseVisualStyleBackColor = true;
            this.checkBoxCastUnstableAffliction.CheckedChanged += new System.EventHandler(this.checkBoxCastUnstableAffliction_CheckedChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.comboBoxPet);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.checkBoxPetSacrificed);
            this.groupBox3.Location = new System.Drawing.Point(3, 359);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(208, 68);
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
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(10, 436);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(103, 13);
            this.label6.TabIndex = 21;
            this.label6.Text = "Heroism/Bloodlust: *";
            this.toolTipWarlock.SetToolTip(this.label6, "Number of Heroisms/Bloodlusts available");
            this.label6.Visible = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(10, 463);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(89, 13);
            this.label7.TabIndex = 23;
            this.label7.Text = "Drums of Battle: *";
            this.toolTipWarlock.SetToolTip(this.label7, "Number of Drums of Battle available");
            this.label7.Visible = false;
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
            // comboBoxBloodlust
            // 
            this.comboBoxBloodlust.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.comboBoxBloodlust.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxBloodlust.FormattingEnabled = true;
            this.comboBoxBloodlust.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "3",
            "4"});
            this.comboBoxBloodlust.Location = new System.Drawing.Point(117, 433);
            this.comboBoxBloodlust.Name = "comboBoxBloodlust";
            this.comboBoxBloodlust.Size = new System.Drawing.Size(88, 21);
            this.comboBoxBloodlust.TabIndex = 22;
            this.comboBoxBloodlust.Visible = false;
            // 
            // comboBoxDrums
            // 
            this.comboBoxDrums.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.comboBoxDrums.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxDrums.FormattingEnabled = true;
            this.comboBoxDrums.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "3",
            "4"});
            this.comboBoxDrums.Location = new System.Drawing.Point(117, 460);
            this.comboBoxDrums.Name = "comboBoxDrums";
            this.comboBoxDrums.Size = new System.Drawing.Size(88, 21);
            this.comboBoxDrums.TabIndex = 24;
            this.comboBoxDrums.Visible = false;
            // 
            // textBoxDotGap
            // 
            this.textBoxDotGap.Location = new System.Drawing.Point(117, 132);
            this.textBoxDotGap.Name = "textBoxDotGap";
            this.textBoxDotGap.Size = new System.Drawing.Size(88, 20);
            this.textBoxDotGap.TabIndex = 25;
            // 
            // textBoxAfflictionDebuffs
            // 
            this.textBoxAfflictionDebuffs.Location = new System.Drawing.Point(117, 158);
            this.textBoxAfflictionDebuffs.Name = "textBoxAfflictionDebuffs";
            this.textBoxAfflictionDebuffs.Size = new System.Drawing.Size(88, 20);
            this.textBoxAfflictionDebuffs.TabIndex = 26;
            // 
            // CalculationOptionsPanelWarlock
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label10);
            this.Controls.Add(this.comboBoxDrums);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.textBoxAfflictionDebuffs);
            this.Controls.Add(this.comboBoxBloodlust);
            this.Controls.Add(this.label6);
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
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox comboBoxBloodlust;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox comboBoxDrums;
        private System.Windows.Forms.TextBox textBoxDotGap;
        private System.Windows.Forms.TextBox textBoxAfflictionDebuffs;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label10;
	}
}
