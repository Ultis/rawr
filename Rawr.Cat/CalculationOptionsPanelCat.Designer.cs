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
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.checkBoxShred = new System.Windows.Forms.CheckBox();
			this.checkBoxRip = new System.Windows.Forms.CheckBox();
			this.checkBoxFerociousBite = new System.Windows.Forms.CheckBox();
			this.comboBoxSavageRoar = new System.Windows.Forms.ComboBox();
			this.label2 = new System.Windows.Forms.Label();
			this.numericUpDownTargetArmor = new System.Windows.Forms.NumericUpDown();
			this.label3 = new System.Windows.Forms.Label();
			this.checkBoxGlyphOfRip = new System.Windows.Forms.CheckBox();
			this.checkBoxGlyphOfMangle = new System.Windows.Forms.CheckBox();
			this.label4 = new System.Windows.Forms.Label();
			this.numericUpDownDuration = new System.Windows.Forms.NumericUpDown();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownTargetArmor)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownDuration)).BeginInit();
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
			this.comboBoxTargetLevel.FormattingEnabled = true;
			this.comboBoxTargetLevel.Items.AddRange(new object[] {
            "80",
            "81",
            "82",
            "83"});
			this.comboBoxTargetLevel.Location = new System.Drawing.Point(83, 3);
			this.comboBoxTargetLevel.Name = "comboBoxTargetLevel";
			this.comboBoxTargetLevel.Size = new System.Drawing.Size(185, 21);
			this.comboBoxTargetLevel.TabIndex = 1;
			this.comboBoxTargetLevel.SelectedIndexChanged += new System.EventHandler(this.calculationOptionControl_Changed);
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.checkBoxShred);
			this.groupBox1.Controls.Add(this.checkBoxRip);
			this.groupBox1.Controls.Add(this.checkBoxFerociousBite);
			this.groupBox1.Controls.Add(this.comboBoxSavageRoar);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Location = new System.Drawing.Point(5, 105);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(261, 88);
			this.groupBox1.TabIndex = 7;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Custom Rotation";
			// 
			// checkBoxShred
			// 
			this.checkBoxShred.AutoSize = true;
			this.checkBoxShred.Location = new System.Drawing.Point(54, 19);
			this.checkBoxShred.Name = "checkBoxShred";
			this.checkBoxShred.Size = new System.Drawing.Size(54, 17);
			this.checkBoxShred.TabIndex = 1;
			this.checkBoxShred.Text = "Shred";
			this.checkBoxShred.UseVisualStyleBackColor = true;
			this.checkBoxShred.CheckedChanged += new System.EventHandler(this.calculationOptionControl_Changed);
			// 
			// checkBoxRip
			// 
			this.checkBoxRip.AutoSize = true;
			this.checkBoxRip.Location = new System.Drawing.Point(6, 19);
			this.checkBoxRip.Name = "checkBoxRip";
			this.checkBoxRip.Size = new System.Drawing.Size(42, 17);
			this.checkBoxRip.TabIndex = 1;
			this.checkBoxRip.Text = "Rip";
			this.checkBoxRip.UseVisualStyleBackColor = true;
			this.checkBoxRip.CheckedChanged += new System.EventHandler(this.calculationOptionControl_Changed);
			// 
			// checkBoxFerociousBite
			// 
			this.checkBoxFerociousBite.AutoSize = true;
			this.checkBoxFerociousBite.Location = new System.Drawing.Point(114, 19);
			this.checkBoxFerociousBite.Name = "checkBoxFerociousBite";
			this.checkBoxFerociousBite.Size = new System.Drawing.Size(93, 17);
			this.checkBoxFerociousBite.TabIndex = 1;
			this.checkBoxFerociousBite.Text = "Ferocious Bite";
			this.checkBoxFerociousBite.UseVisualStyleBackColor = true;
			this.checkBoxFerociousBite.CheckedChanged += new System.EventHandler(this.calculationOptionControl_Changed);
			// 
			// comboBoxSavageRoar
			// 
			this.comboBoxSavageRoar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.comboBoxSavageRoar.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxSavageRoar.FormattingEnabled = true;
			this.comboBoxSavageRoar.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5"});
			this.comboBoxSavageRoar.Location = new System.Drawing.Point(156, 42);
			this.comboBoxSavageRoar.Name = "comboBoxSavageRoar";
			this.comboBoxSavageRoar.Size = new System.Drawing.Size(99, 21);
			this.comboBoxSavageRoar.TabIndex = 1;
			this.comboBoxSavageRoar.SelectedIndexChanged += new System.EventHandler(this.calculationOptionControl_Changed);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(6, 45);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(144, 13);
			this.label2.TabIndex = 0;
			this.label2.Text = "Savage Roar Combo Points: ";
			// 
			// numericUpDownTargetArmor
			// 
			this.numericUpDownTargetArmor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.numericUpDownTargetArmor.Location = new System.Drawing.Point(83, 30);
			this.numericUpDownTargetArmor.Maximum = new decimal(new int[] {
            15000,
            0,
            0,
            0});
			this.numericUpDownTargetArmor.Name = "numericUpDownTargetArmor";
			this.numericUpDownTargetArmor.Size = new System.Drawing.Size(185, 20);
			this.numericUpDownTargetArmor.TabIndex = 6;
			this.numericUpDownTargetArmor.ThousandsSeparator = true;
			this.numericUpDownTargetArmor.Value = new decimal(new int[] {
            13083,
            0,
            0,
            0});
			this.numericUpDownTargetArmor.ValueChanged += new System.EventHandler(this.calculationOptionControl_Changed);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(3, 32);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(71, 13);
			this.label3.TabIndex = 5;
			this.label3.Text = "Target Armor:";
			// 
			// checkBoxGlyphOfRip
			// 
			this.checkBoxGlyphOfRip.AutoSize = true;
			this.checkBoxGlyphOfRip.Location = new System.Drawing.Point(115, 82);
			this.checkBoxGlyphOfRip.Name = "checkBoxGlyphOfRip";
			this.checkBoxGlyphOfRip.Size = new System.Drawing.Size(84, 17);
			this.checkBoxGlyphOfRip.TabIndex = 1;
			this.checkBoxGlyphOfRip.Text = "Glyph of Rip";
			this.checkBoxGlyphOfRip.UseVisualStyleBackColor = true;
			this.checkBoxGlyphOfRip.CheckedChanged += new System.EventHandler(this.calculationOptionControl_Changed);
			// 
			// checkBoxGlyphOfMangle
			// 
			this.checkBoxGlyphOfMangle.AutoSize = true;
			this.checkBoxGlyphOfMangle.Location = new System.Drawing.Point(6, 82);
			this.checkBoxGlyphOfMangle.Name = "checkBoxGlyphOfMangle";
			this.checkBoxGlyphOfMangle.Size = new System.Drawing.Size(103, 17);
			this.checkBoxGlyphOfMangle.TabIndex = 1;
			this.checkBoxGlyphOfMangle.Text = "Glyph of Mangle";
			this.checkBoxGlyphOfMangle.UseVisualStyleBackColor = true;
			this.checkBoxGlyphOfMangle.CheckedChanged += new System.EventHandler(this.calculationOptionControl_Changed);
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(3, 58);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(76, 13);
			this.label4.TabIndex = 5;
			this.label4.Text = "Duration (sec):";
			// 
			// numericUpDownDuration
			// 
			this.numericUpDownDuration.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.numericUpDownDuration.Location = new System.Drawing.Point(83, 56);
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
			this.numericUpDownDuration.TabIndex = 6;
			this.numericUpDownDuration.ThousandsSeparator = true;
			this.numericUpDownDuration.Value = new decimal(new int[] {
            300,
            0,
            0,
            0});
			this.numericUpDownDuration.ValueChanged += new System.EventHandler(this.calculationOptionControl_Changed);
			// 
			// CalculationOptionsPanelCat
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoScroll = true;
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.checkBoxGlyphOfMangle);
			this.Controls.Add(this.checkBoxGlyphOfRip);
			this.Controls.Add(this.numericUpDownDuration);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.numericUpDownTargetArmor);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.comboBoxTargetLevel);
			this.Controls.Add(this.label1);
			this.Name = "CalculationOptionsPanelCat";
			this.Size = new System.Drawing.Size(271, 827);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownTargetArmor)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownDuration)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox comboBoxTargetLevel;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.CheckBox checkBoxShred;
		private System.Windows.Forms.CheckBox checkBoxRip;
		private System.Windows.Forms.CheckBox checkBoxFerociousBite;
		private System.Windows.Forms.ComboBox comboBoxSavageRoar;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.NumericUpDown numericUpDownTargetArmor;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.CheckBox checkBoxGlyphOfRip;
		private System.Windows.Forms.CheckBox checkBoxGlyphOfMangle;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.NumericUpDown numericUpDownDuration;
	}
}
