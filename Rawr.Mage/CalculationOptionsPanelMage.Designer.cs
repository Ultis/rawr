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
            this.checkBoxEnforceMetagemRequirements.Location = new System.Drawing.Point(6, 141);
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
            this.buttonTalents.Location = new System.Drawing.Point(6, 112);
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
            this.label3.Location = new System.Drawing.Point(3, 60);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(48, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Latency:";
            // 
            // textBoxLatency
            // 
            this.textBoxLatency.Location = new System.Drawing.Point(104, 57);
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
            this.comboBoxArmor.Location = new System.Drawing.Point(104, 83);
            this.comboBoxArmor.Name = "comboBoxArmor";
            this.comboBoxArmor.Size = new System.Drawing.Size(93, 21);
            this.comboBoxArmor.TabIndex = 9;
            this.comboBoxArmor.SelectedIndexChanged += new System.EventHandler(this.comboBoxArmor_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 86);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(37, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Armor:";
            // 
            // CalculationOptionsPanelMage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
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
            this.Size = new System.Drawing.Size(204, 338);
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
	}
}
