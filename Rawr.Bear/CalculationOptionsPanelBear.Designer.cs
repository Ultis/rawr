namespace Rawr
{
	partial class CalculationOptionsPanelBear
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
			this.numericUpDownThreatValue = new System.Windows.Forms.NumericUpDown();
			this.comboBoxThreatValue = new System.Windows.Forms.ComboBox();
			this.numericUpDownTargetArmor = new System.Windows.Forms.NumericUpDown();
			this.label3 = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.checkBoxLacerate = new System.Windows.Forms.CheckBox();
			this.checkBoxMangle = new System.Windows.Forms.CheckBox();
			this.checkBoxFaerieFire = new System.Windows.Forms.CheckBox();
			this.checkBoxSwipe = new System.Windows.Forms.CheckBox();
			this.radioButtonMaul = new System.Windows.Forms.RadioButton();
			this.radioButtonMelee = new System.Windows.Forms.RadioButton();
			this.radioButtonNoAuto = new System.Windows.Forms.RadioButton();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownThreatValue)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownTargetArmor)).BeginInit();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(4, 6);
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
			this.comboBoxTargetLevel.Size = new System.Drawing.Size(174, 21);
			this.comboBoxTargetLevel.TabIndex = 1;
			this.comboBoxTargetLevel.SelectedIndexChanged += new System.EventHandler(this.calculationOptionControl_Changed);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(4, 33);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(71, 13);
			this.label2.TabIndex = 0;
			this.label2.Text = "Threat Value:";
			// 
			// numericUpDownThreatValue
			// 
			this.numericUpDownThreatValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.numericUpDownThreatValue.DecimalPlaces = 3;
			this.numericUpDownThreatValue.Enabled = false;
			this.numericUpDownThreatValue.Location = new System.Drawing.Point(84, 58);
			this.numericUpDownThreatValue.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
			this.numericUpDownThreatValue.Name = "numericUpDownThreatValue";
			this.numericUpDownThreatValue.Size = new System.Drawing.Size(173, 20);
			this.numericUpDownThreatValue.TabIndex = 3;
			this.numericUpDownThreatValue.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numericUpDownThreatValue.ValueChanged += new System.EventHandler(this.calculationOptionControl_Changed);
			// 
			// comboBoxThreatValue
			// 
			this.comboBoxThreatValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.comboBoxThreatValue.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxThreatValue.FormattingEnabled = true;
			this.comboBoxThreatValue.Items.AddRange(new object[] {
            "None",
            "MT",
            "OT",
            "Crazy About Threat",
            "Custom Factor"});
			this.comboBoxThreatValue.Location = new System.Drawing.Point(84, 30);
			this.comboBoxThreatValue.Name = "comboBoxThreatValue";
			this.comboBoxThreatValue.Size = new System.Drawing.Size(173, 21);
			this.comboBoxThreatValue.TabIndex = 1;
			this.comboBoxThreatValue.SelectedIndexChanged += new System.EventHandler(this.comboBoxThreatValue_SelectedIndexChanged);
			// 
			// numericUpDownTargetArmor
			// 
			this.numericUpDownTargetArmor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.numericUpDownTargetArmor.Location = new System.Drawing.Point(83, 84);
			this.numericUpDownTargetArmor.Maximum = new decimal(new int[] {
            15000,
            0,
            0,
            0});
			this.numericUpDownTargetArmor.Name = "numericUpDownTargetArmor";
			this.numericUpDownTargetArmor.Size = new System.Drawing.Size(174, 20);
			this.numericUpDownTargetArmor.TabIndex = 3;
			this.numericUpDownTargetArmor.ThousandsSeparator = true;
			this.numericUpDownTargetArmor.Value = new decimal(new int[] {
            11000,
            0,
            0,
            0});
			this.numericUpDownTargetArmor.ValueChanged += new System.EventHandler(this.calculationOptionControl_Changed);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(3, 86);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(71, 13);
			this.label3.TabIndex = 0;
			this.label3.Text = "Target Armor:";
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.checkBoxLacerate);
			this.groupBox1.Controls.Add(this.checkBoxMangle);
			this.groupBox1.Controls.Add(this.checkBoxFaerieFire);
			this.groupBox1.Controls.Add(this.checkBoxSwipe);
			this.groupBox1.Controls.Add(this.radioButtonMaul);
			this.groupBox1.Controls.Add(this.radioButtonMelee);
			this.groupBox1.Controls.Add(this.radioButtonNoAuto);
			this.groupBox1.Location = new System.Drawing.Point(7, 110);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(250, 88);
			this.groupBox1.TabIndex = 4;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Custom Rotation";
			// 
			// checkBoxLacerate
			// 
			this.checkBoxLacerate.AutoSize = true;
			this.checkBoxLacerate.Location = new System.Drawing.Point(159, 42);
			this.checkBoxLacerate.Name = "checkBoxLacerate";
			this.checkBoxLacerate.Size = new System.Drawing.Size(68, 17);
			this.checkBoxLacerate.TabIndex = 1;
			this.checkBoxLacerate.Text = "Lacerate";
			this.checkBoxLacerate.UseVisualStyleBackColor = true;
			this.checkBoxLacerate.CheckedChanged += new System.EventHandler(this.calculationOptionControl_Changed);
			// 
			// checkBoxMangle
			// 
			this.checkBoxMangle.AutoSize = true;
			this.checkBoxMangle.Location = new System.Drawing.Point(78, 19);
			this.checkBoxMangle.Name = "checkBoxMangle";
			this.checkBoxMangle.Size = new System.Drawing.Size(61, 17);
			this.checkBoxMangle.TabIndex = 1;
			this.checkBoxMangle.Text = "Mangle";
			this.checkBoxMangle.UseVisualStyleBackColor = true;
			this.checkBoxMangle.CheckedChanged += new System.EventHandler(this.calculationOptionControl_Changed);
			// 
			// checkBoxFaerieFire
			// 
			this.checkBoxFaerieFire.AutoSize = true;
			this.checkBoxFaerieFire.Location = new System.Drawing.Point(78, 42);
			this.checkBoxFaerieFire.Name = "checkBoxFaerieFire";
			this.checkBoxFaerieFire.Size = new System.Drawing.Size(75, 17);
			this.checkBoxFaerieFire.TabIndex = 1;
			this.checkBoxFaerieFire.Text = "Faerie Fire";
			this.checkBoxFaerieFire.UseVisualStyleBackColor = true;
			this.checkBoxFaerieFire.CheckedChanged += new System.EventHandler(this.calculationOptionControl_Changed);
			// 
			// checkBoxSwipe
			// 
			this.checkBoxSwipe.AutoSize = true;
			this.checkBoxSwipe.Location = new System.Drawing.Point(159, 19);
			this.checkBoxSwipe.Name = "checkBoxSwipe";
			this.checkBoxSwipe.Size = new System.Drawing.Size(55, 17);
			this.checkBoxSwipe.TabIndex = 1;
			this.checkBoxSwipe.Text = "Swipe";
			this.checkBoxSwipe.UseVisualStyleBackColor = true;
			this.checkBoxSwipe.CheckedChanged += new System.EventHandler(this.calculationOptionControl_Changed);
			// 
			// radioButtonMaul
			// 
			this.radioButtonMaul.AutoSize = true;
			this.radioButtonMaul.Location = new System.Drawing.Point(8, 65);
			this.radioButtonMaul.Name = "radioButtonMaul";
			this.radioButtonMaul.Size = new System.Drawing.Size(48, 17);
			this.radioButtonMaul.TabIndex = 0;
			this.radioButtonMaul.TabStop = true;
			this.radioButtonMaul.Text = "Maul";
			this.radioButtonMaul.UseVisualStyleBackColor = true;
			this.radioButtonMaul.CheckedChanged += new System.EventHandler(this.calculationOptionControl_Changed);
			// 
			// radioButtonMelee
			// 
			this.radioButtonMelee.AutoSize = true;
			this.radioButtonMelee.Location = new System.Drawing.Point(8, 42);
			this.radioButtonMelee.Name = "radioButtonMelee";
			this.radioButtonMelee.Size = new System.Drawing.Size(54, 17);
			this.radioButtonMelee.TabIndex = 0;
			this.radioButtonMelee.TabStop = true;
			this.radioButtonMelee.Text = "Melee";
			this.radioButtonMelee.UseVisualStyleBackColor = true;
			this.radioButtonMelee.CheckedChanged += new System.EventHandler(this.calculationOptionControl_Changed);
			// 
			// radioButtonNoAuto
			// 
			this.radioButtonNoAuto.AutoSize = true;
			this.radioButtonNoAuto.Location = new System.Drawing.Point(8, 19);
			this.radioButtonNoAuto.Name = "radioButtonNoAuto";
			this.radioButtonNoAuto.Size = new System.Drawing.Size(64, 17);
			this.radioButtonNoAuto.TabIndex = 0;
			this.radioButtonNoAuto.TabStop = true;
			this.radioButtonNoAuto.Text = "No Auto";
			this.radioButtonNoAuto.UseVisualStyleBackColor = true;
			this.radioButtonNoAuto.CheckedChanged += new System.EventHandler(this.calculationOptionControl_Changed);
			// 
			// CalculationOptionsPanelBear
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.numericUpDownTargetArmor);
			this.Controls.Add(this.numericUpDownThreatValue);
			this.Controls.Add(this.comboBoxThreatValue);
			this.Controls.Add(this.comboBoxTargetLevel);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Name = "CalculationOptionsPanelBear";
			this.Size = new System.Drawing.Size(260, 338);
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownThreatValue)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownTargetArmor)).EndInit();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox comboBoxTargetLevel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numericUpDownThreatValue;
		private System.Windows.Forms.ComboBox comboBoxThreatValue;
		private System.Windows.Forms.NumericUpDown numericUpDownTargetArmor;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.CheckBox checkBoxLacerate;
		private System.Windows.Forms.CheckBox checkBoxMangle;
		private System.Windows.Forms.CheckBox checkBoxFaerieFire;
		private System.Windows.Forms.CheckBox checkBoxSwipe;
		private System.Windows.Forms.RadioButton radioButtonMaul;
		private System.Windows.Forms.RadioButton radioButtonMelee;
		private System.Windows.Forms.RadioButton radioButtonNoAuto;
	}
}
