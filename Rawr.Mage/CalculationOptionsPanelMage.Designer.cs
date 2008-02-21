namespace Rawr.Mage
{
    partial class CalculationOptionsPanelMage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CalculationOptionsPanelMage));
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxTargetLevel = new System.Windows.Forms.ComboBox();
            this.checkBoxEnforceMetagemRequirements = new System.Windows.Forms.CheckBox();
            this.comboBoxAoeTargetLevel = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonTalents = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxLatency = new System.Windows.Forms.TextBox();
            this.comboBoxArmor = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxAoeTargets = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxArcaneResist = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textBoxFireResist = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.textBoxFrostResist = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.textBoxNatureResist = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.textBoxFightDuration = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.textBoxShadowPriest = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
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
            this.comboBoxTargetLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxTargetLevel.FormattingEnabled = true;
            this.comboBoxTargetLevel.Items.AddRange(new object[] {
            "70",
            "71",
            "72",
            "73"});
            this.comboBoxTargetLevel.Location = new System.Drawing.Point(104, 3);
            this.comboBoxTargetLevel.Name = "comboBoxTargetLevel";
            this.comboBoxTargetLevel.Size = new System.Drawing.Size(93, 21);
            this.comboBoxTargetLevel.TabIndex = 1;
            this.comboBoxTargetLevel.SelectedIndexChanged += new System.EventHandler(this.comboBoxTargetLevel_SelectedIndexChanged);
            // 
            // checkBoxEnforceMetagemRequirements
            // 
            this.checkBoxEnforceMetagemRequirements.AutoSize = true;
            this.checkBoxEnforceMetagemRequirements.Location = new System.Drawing.Point(6, 373);
            this.checkBoxEnforceMetagemRequirements.Name = "checkBoxEnforceMetagemRequirements";
            this.checkBoxEnforceMetagemRequirements.Size = new System.Drawing.Size(178, 17);
            this.checkBoxEnforceMetagemRequirements.TabIndex = 2;
            this.checkBoxEnforceMetagemRequirements.Text = "Enforce Metagem Requirements";
            this.checkBoxEnforceMetagemRequirements.UseVisualStyleBackColor = true;
            this.checkBoxEnforceMetagemRequirements.CheckedChanged += new System.EventHandler(this.checkBoxEnforceMetagemRequirements_CheckedChanged);
            // 
            // comboBoxAoeTargetLevel
            // 
            this.comboBoxAoeTargetLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxAoeTargetLevel.FormattingEnabled = true;
            this.comboBoxAoeTargetLevel.Items.AddRange(new object[] {
            "70",
            "71",
            "72",
            "73"});
            this.comboBoxAoeTargetLevel.Location = new System.Drawing.Point(104, 30);
            this.comboBoxAoeTargetLevel.Name = "comboBoxAoeTargetLevel";
            this.comboBoxAoeTargetLevel.Size = new System.Drawing.Size(93, 21);
            this.comboBoxAoeTargetLevel.TabIndex = 4;
            this.comboBoxAoeTargetLevel.SelectedIndexChanged += new System.EventHandler(this.comboBoxAoeTargetLevel_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 33);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Aoe Target Level: ";
            // 
            // buttonTalents
            // 
            this.buttonTalents.Location = new System.Drawing.Point(6, 344);
            this.buttonTalents.Name = "buttonTalents";
            this.buttonTalents.Size = new System.Drawing.Size(191, 23);
            this.buttonTalents.TabIndex = 5;
            this.buttonTalents.Text = "Talents";
            this.buttonTalents.UseVisualStyleBackColor = true;
            this.buttonTalents.Click += new System.EventHandler(this.buttonTalents_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 292);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(48, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Latency:";
            // 
            // textBoxLatency
            // 
            this.textBoxLatency.Location = new System.Drawing.Point(104, 289);
            this.textBoxLatency.Name = "textBoxLatency";
            this.textBoxLatency.Size = new System.Drawing.Size(93, 20);
            this.textBoxLatency.TabIndex = 7;
            this.textBoxLatency.TextChanged += new System.EventHandler(this.textBoxLatency_TextChanged);
            // 
            // comboBoxArmor
            // 
            this.comboBoxArmor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxArmor.FormattingEnabled = true;
            this.comboBoxArmor.Items.AddRange(new object[] {
            "Mage",
            "Molten",
            "Ice"});
            this.comboBoxArmor.Location = new System.Drawing.Point(104, 315);
            this.comboBoxArmor.Name = "comboBoxArmor";
            this.comboBoxArmor.Size = new System.Drawing.Size(93, 21);
            this.comboBoxArmor.TabIndex = 9;
            this.comboBoxArmor.SelectedIndexChanged += new System.EventHandler(this.comboBoxArmor_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 318);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(37, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Armor:";
            // 
            // textBoxAoeTargets
            // 
            this.textBoxAoeTargets.Location = new System.Drawing.Point(104, 57);
            this.textBoxAoeTargets.Name = "textBoxAoeTargets";
            this.textBoxAoeTargets.Size = new System.Drawing.Size(93, 20);
            this.textBoxAoeTargets.TabIndex = 11;
            this.textBoxAoeTargets.TextChanged += new System.EventHandler(this.textBoxAoeTargets_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 60);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(68, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Aoe Targets:";
            // 
            // textBoxArcaneResist
            // 
            this.textBoxArcaneResist.Location = new System.Drawing.Point(104, 94);
            this.textBoxArcaneResist.Name = "textBoxArcaneResist";
            this.textBoxArcaneResist.Size = new System.Drawing.Size(93, 20);
            this.textBoxArcaneResist.TabIndex = 13;
            this.textBoxArcaneResist.TextChanged += new System.EventHandler(this.textBoxArcaneResist_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 97);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(100, 13);
            this.label6.TabIndex = 12;
            this.label6.Text = "Arcane Resist (0-1):";
            // 
            // textBoxFireResist
            // 
            this.textBoxFireResist.Location = new System.Drawing.Point(104, 120);
            this.textBoxFireResist.Name = "textBoxFireResist";
            this.textBoxFireResist.Size = new System.Drawing.Size(93, 20);
            this.textBoxFireResist.TabIndex = 15;
            this.textBoxFireResist.TextChanged += new System.EventHandler(this.textBoxFireResist_TextChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(3, 123);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(83, 13);
            this.label7.TabIndex = 14;
            this.label7.Text = "Fire Resist (0-1):";
            // 
            // textBoxFrostResist
            // 
            this.textBoxFrostResist.Location = new System.Drawing.Point(104, 146);
            this.textBoxFrostResist.Name = "textBoxFrostResist";
            this.textBoxFrostResist.Size = new System.Drawing.Size(93, 20);
            this.textBoxFrostResist.TabIndex = 17;
            this.textBoxFrostResist.TextChanged += new System.EventHandler(this.textBoxFrostResist_TextChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(3, 149);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(89, 13);
            this.label8.TabIndex = 16;
            this.label8.Text = "Frost Resist (0-1):";
            // 
            // textBoxNatureResist
            // 
            this.textBoxNatureResist.Location = new System.Drawing.Point(104, 172);
            this.textBoxNatureResist.Name = "textBoxNatureResist";
            this.textBoxNatureResist.Size = new System.Drawing.Size(93, 20);
            this.textBoxNatureResist.TabIndex = 19;
            this.textBoxNatureResist.TextChanged += new System.EventHandler(this.textBoxNatureResist_TextChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(3, 175);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(98, 13);
            this.label9.TabIndex = 18;
            this.label9.Text = "Nature Resist (0-1):";
            // 
            // textBoxFightDuration
            // 
            this.textBoxFightDuration.Location = new System.Drawing.Point(104, 210);
            this.textBoxFightDuration.Name = "textBoxFightDuration";
            this.textBoxFightDuration.Size = new System.Drawing.Size(93, 20);
            this.textBoxFightDuration.TabIndex = 21;
            this.textBoxFightDuration.TextChanged += new System.EventHandler(this.textBoxFightDuration_TextChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(3, 213);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(102, 13);
            this.label10.TabIndex = 20;
            this.label10.Text = "Fight Duration (sec):";
            // 
            // textBoxShadowPriest
            // 
            this.textBoxShadowPriest.Location = new System.Drawing.Point(104, 250);
            this.textBoxShadowPriest.Name = "textBoxShadowPriest";
            this.textBoxShadowPriest.Size = new System.Drawing.Size(93, 20);
            this.textBoxShadowPriest.TabIndex = 23;
            this.textBoxShadowPriest.TextChanged += new System.EventHandler(this.textBoxShadowPriest_TextChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(3, 253);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(82, 13);
            this.label11.TabIndex = 22;
            this.label11.Text = "Sh. Priest (Mp5)";
            // 
            // CalculationOptionsPanelMage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.textBoxShadowPriest);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.textBoxFightDuration);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.textBoxNatureResist);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.textBoxFrostResist);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.textBoxFireResist);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.textBoxArcaneResist);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.textBoxAoeTargets);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.comboBoxArmor);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBoxLatency);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.buttonTalents);
            this.Controls.Add(this.comboBoxAoeTargetLevel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.checkBoxEnforceMetagemRequirements);
            this.Controls.Add(this.comboBoxTargetLevel);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "CalculationOptionsPanelMage";
            this.Size = new System.Drawing.Size(204, 397);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox comboBoxTargetLevel;
		private System.Windows.Forms.CheckBox checkBoxEnforceMetagemRequirements;
        private System.Windows.Forms.ComboBox comboBoxAoeTargetLevel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonTalents;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxLatency;
        private System.Windows.Forms.ComboBox comboBoxArmor;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxAoeTargets;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxArcaneResist;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBoxFireResist;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBoxFrostResist;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBoxNatureResist;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox textBoxFightDuration;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox textBoxShadowPriest;
        private System.Windows.Forms.Label label11;
	}
}
