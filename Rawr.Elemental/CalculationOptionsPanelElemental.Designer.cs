namespace Rawr
{
    partial class CalculationOptionsPanelElemental
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
            this.tabGlyph = new System.Windows.Forms.TabPage();
            this.checkBoxGlyphOfFlametongueWeapon = new System.Windows.Forms.CheckBox();
            this.checkBoxGlyphOfLightningBolt = new System.Windows.Forms.CheckBox();
            this.checkBoxGlyphOfFlameShock = new System.Windows.Forms.CheckBox();
            this.checkBoxGlyphOfWaterShield = new System.Windows.Forms.CheckBox();
            this.tabGeneral = new System.Windows.Forms.TabPage();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxTargetLevel = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.numericUpDownDuration = new System.Windows.Forms.NumericUpDown();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabGlyph.SuspendLayout();
            this.tabGeneral.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDuration)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabGlyph
            // 
            this.tabGlyph.BackColor = System.Drawing.Color.Transparent;
            this.tabGlyph.Controls.Add(this.checkBoxGlyphOfWaterShield);
            this.tabGlyph.Controls.Add(this.checkBoxGlyphOfFlameShock);
            this.tabGlyph.Controls.Add(this.checkBoxGlyphOfLightningBolt);
            this.tabGlyph.Controls.Add(this.checkBoxGlyphOfFlametongueWeapon);
            this.tabGlyph.Location = new System.Drawing.Point(4, 22);
            this.tabGlyph.Name = "tabGlyph";
            this.tabGlyph.Padding = new System.Windows.Forms.Padding(3);
            this.tabGlyph.Size = new System.Drawing.Size(263, 801);
            this.tabGlyph.TabIndex = 0;
            this.tabGlyph.Text = "Glyphs";
            this.tabGlyph.UseVisualStyleBackColor = true;
            // 
            // checkBoxGlyphOfFlametongueWeapon
            // 
            this.checkBoxGlyphOfFlametongueWeapon.AutoSize = true;
            this.checkBoxGlyphOfFlametongueWeapon.Location = new System.Drawing.Point(6, 29);
            this.checkBoxGlyphOfFlametongueWeapon.Name = "checkBoxGlyphOfFlametongueWeapon";
            this.checkBoxGlyphOfFlametongueWeapon.Size = new System.Drawing.Size(173, 17);
            this.checkBoxGlyphOfFlametongueWeapon.TabIndex = 2;
            this.checkBoxGlyphOfFlametongueWeapon.Text = "Glyph of Flametongue Weapon";
            this.checkBoxGlyphOfFlametongueWeapon.UseVisualStyleBackColor = true;
            // 
            // checkBoxGlyphOfLightningBolt
            // 
            this.checkBoxGlyphOfLightningBolt.AutoSize = true;
            this.checkBoxGlyphOfLightningBolt.Location = new System.Drawing.Point(6, 52);
            this.checkBoxGlyphOfLightningBolt.Name = "checkBoxGlyphOfLightningBolt";
            this.checkBoxGlyphOfLightningBolt.Size = new System.Drawing.Size(132, 17);
            this.checkBoxGlyphOfLightningBolt.TabIndex = 1;
            this.checkBoxGlyphOfLightningBolt.Text = "Glyph of Lightning Bolt";
            this.checkBoxGlyphOfLightningBolt.UseVisualStyleBackColor = true;
            this.checkBoxGlyphOfLightningBolt.CheckedChanged += new System.EventHandler(this.calculationOptionControl_Changed);
            // 
            // checkBoxGlyphOfFlameShock
            // 
            this.checkBoxGlyphOfFlameShock.AutoSize = true;
            this.checkBoxGlyphOfFlameShock.Location = new System.Drawing.Point(6, 6);
            this.checkBoxGlyphOfFlameShock.Name = "checkBoxGlyphOfFlameShock";
            this.checkBoxGlyphOfFlameShock.Size = new System.Drawing.Size(130, 17);
            this.checkBoxGlyphOfFlameShock.TabIndex = 10;
            this.checkBoxGlyphOfFlameShock.Text = "Glyph of Flame Shock";
            this.checkBoxGlyphOfFlameShock.UseVisualStyleBackColor = true;
            // 
            // checkBoxGlyphOfWaterShield
            // 
            this.checkBoxGlyphOfWaterShield.AutoSize = true;
            this.checkBoxGlyphOfWaterShield.Location = new System.Drawing.Point(6, 75);
            this.checkBoxGlyphOfWaterShield.Name = "checkBoxGlyphOfWaterShield";
            this.checkBoxGlyphOfWaterShield.Size = new System.Drawing.Size(129, 17);
            this.checkBoxGlyphOfWaterShield.TabIndex = 3;
            this.checkBoxGlyphOfWaterShield.Text = "Glyph of Water Shield";
            this.checkBoxGlyphOfWaterShield.UseVisualStyleBackColor = true;
            // 
            // tabGeneral
            // 
            this.tabGeneral.BackColor = System.Drawing.Color.Transparent;
            this.tabGeneral.Controls.Add(this.numericUpDownDuration);
            this.tabGeneral.Controls.Add(this.label4);
            this.tabGeneral.Controls.Add(this.comboBoxTargetLevel);
            this.tabGeneral.Controls.Add(this.label1);
            this.tabGeneral.Location = new System.Drawing.Point(4, 22);
            this.tabGeneral.Name = "tabGeneral";
            this.tabGeneral.Padding = new System.Windows.Forms.Padding(3);
            this.tabGeneral.Size = new System.Drawing.Size(263, 801);
            this.tabGeneral.TabIndex = 1;
            this.tabGeneral.Text = "General";
            this.tabGeneral.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 13);
            this.label1.TabIndex = 1;
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
            this.comboBoxTargetLevel.Location = new System.Drawing.Point(75, 13);
            this.comboBoxTargetLevel.Name = "comboBoxTargetLevel";
            this.comboBoxTargetLevel.Size = new System.Drawing.Size(185, 21);
            this.comboBoxTargetLevel.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(0, 51);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(76, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Duration (sec):";
            // 
            // numericUpDownDuration
            // 
            this.numericUpDownDuration.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.numericUpDownDuration.Location = new System.Drawing.Point(75, 49);
            this.numericUpDownDuration.Maximum = new decimal(new int[] {
            1200,
            0,
            0,
            0});
            this.numericUpDownDuration.Minimum = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.numericUpDownDuration.Name = "numericUpDownDuration";
            this.numericUpDownDuration.Size = new System.Drawing.Size(185, 20);
            this.numericUpDownDuration.TabIndex = 7;
            this.numericUpDownDuration.ThousandsSeparator = true;
            this.numericUpDownDuration.Value = new decimal(new int[] {
            600,
            0,
            0,
            0});
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabGeneral);
            this.tabControl1.Controls.Add(this.tabGlyph);
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(271, 827);
            this.tabControl1.TabIndex = 1;
            // 
            // CalculationOptionsPanelElemental
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.tabControl1);
            this.Name = "CalculationOptionsPanelElemental";
            this.Size = new System.Drawing.Size(271, 827);
            this.tabGlyph.ResumeLayout(false);
            this.tabGlyph.PerformLayout();
            this.tabGeneral.ResumeLayout(false);
            this.tabGeneral.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDuration)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);

		}

		#endregion

        private System.Windows.Forms.TabPage tabGlyph;
        private System.Windows.Forms.CheckBox checkBoxGlyphOfWaterShield;
        private System.Windows.Forms.CheckBox checkBoxGlyphOfFlameShock;
        private System.Windows.Forms.CheckBox checkBoxGlyphOfLightningBolt;
        private System.Windows.Forms.CheckBox checkBoxGlyphOfFlametongueWeapon;
        private System.Windows.Forms.TabPage tabGeneral;
        private System.Windows.Forms.NumericUpDown numericUpDownDuration;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboBoxTargetLevel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabControl tabControl1;

    }
}
