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
			this.checkBoxEnforceMetagemRequirements = new System.Windows.Forms.CheckBox();
			this.label2 = new System.Windows.Forms.Label();
			this.numericUpDownThreatValue = new System.Windows.Forms.NumericUpDown();
			this.comboBoxThreatValue = new System.Windows.Forms.ComboBox();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownThreatValue)).BeginInit();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(3, 29);
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
			this.comboBoxTargetLevel.Location = new System.Drawing.Point(82, 26);
			this.comboBoxTargetLevel.Name = "comboBoxTargetLevel";
			this.comboBoxTargetLevel.Size = new System.Drawing.Size(121, 21);
			this.comboBoxTargetLevel.TabIndex = 1;
			this.comboBoxTargetLevel.SelectedIndexChanged += new System.EventHandler(this.calculationOptionControl_Changed);
			// 
			// checkBoxEnforceMetagemRequirements
			// 
			this.checkBoxEnforceMetagemRequirements.AutoSize = true;
			this.checkBoxEnforceMetagemRequirements.Location = new System.Drawing.Point(6, 3);
			this.checkBoxEnforceMetagemRequirements.Name = "checkBoxEnforceMetagemRequirements";
			this.checkBoxEnforceMetagemRequirements.Size = new System.Drawing.Size(178, 17);
			this.checkBoxEnforceMetagemRequirements.TabIndex = 2;
			this.checkBoxEnforceMetagemRequirements.Text = "Enforce Metagem Requirements";
			this.checkBoxEnforceMetagemRequirements.UseVisualStyleBackColor = true;
			this.checkBoxEnforceMetagemRequirements.CheckedChanged += new System.EventHandler(this.calculationOptionControl_Changed);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(3, 56);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(71, 13);
			this.label2.TabIndex = 0;
			this.label2.Text = "Threat Value:";
			// 
			// numericUpDownThreatValue
			// 
			this.numericUpDownThreatValue.DecimalPlaces = 3;
			this.numericUpDownThreatValue.Enabled = false;
			this.numericUpDownThreatValue.Location = new System.Drawing.Point(83, 81);
			this.numericUpDownThreatValue.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
			this.numericUpDownThreatValue.Name = "numericUpDownThreatValue";
			this.numericUpDownThreatValue.Size = new System.Drawing.Size(120, 20);
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
			this.comboBoxThreatValue.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxThreatValue.FormattingEnabled = true;
			this.comboBoxThreatValue.Items.AddRange(new object[] {
            "None",
            "MT",
            "OT",
            "Crazy About Threat",
            "Custom Factor"});
			this.comboBoxThreatValue.Location = new System.Drawing.Point(83, 53);
			this.comboBoxThreatValue.Name = "comboBoxThreatValue";
			this.comboBoxThreatValue.Size = new System.Drawing.Size(121, 21);
			this.comboBoxThreatValue.TabIndex = 1;
			this.comboBoxThreatValue.SelectedIndexChanged += new System.EventHandler(this.comboBoxThreatValue_SelectedIndexChanged);
			// 
			// CalculationOptionsPanelBear
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.numericUpDownThreatValue);
			this.Controls.Add(this.checkBoxEnforceMetagemRequirements);
			this.Controls.Add(this.comboBoxThreatValue);
			this.Controls.Add(this.comboBoxTargetLevel);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Name = "CalculationOptionsPanelBear";
			this.Size = new System.Drawing.Size(332, 338);
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownThreatValue)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox comboBoxTargetLevel;
		private System.Windows.Forms.CheckBox checkBoxEnforceMetagemRequirements;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numericUpDownThreatValue;
		private System.Windows.Forms.ComboBox comboBoxThreatValue;
	}
}
