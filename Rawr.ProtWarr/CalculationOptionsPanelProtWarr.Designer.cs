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
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxTargetLevel = new System.Windows.Forms.ComboBox();
            this.checkBoxEnforceMetagemRequirements = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.threatScaleFactor = new System.Windows.Forms.NumericUpDown();
            this.labelBossAttack = new System.Windows.Forms.Label();
            this.bossAttackValue = new System.Windows.Forms.NumericUpDown();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.mitigationScaleFactor = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.threatScaleFactor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bossAttackValue)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mitigationScaleFactor)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 11);
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
            this.comboBoxTargetLevel.Location = new System.Drawing.Point(82, 8);
            this.comboBoxTargetLevel.Name = "comboBoxTargetLevel";
            this.comboBoxTargetLevel.Size = new System.Drawing.Size(121, 21);
            this.comboBoxTargetLevel.TabIndex = 1;
            this.comboBoxTargetLevel.SelectedIndexChanged += new System.EventHandler(this.comboBoxTargetLevel_SelectedIndexChanged);
            // 
            // checkBoxEnforceMetagemRequirements
            // 
            this.checkBoxEnforceMetagemRequirements.AutoSize = true;
            this.checkBoxEnforceMetagemRequirements.Location = new System.Drawing.Point(6, 115);
            this.checkBoxEnforceMetagemRequirements.Name = "checkBoxEnforceMetagemRequirements";
            this.checkBoxEnforceMetagemRequirements.Size = new System.Drawing.Size(178, 17);
            this.checkBoxEnforceMetagemRequirements.TabIndex = 4;
            this.checkBoxEnforceMetagemRequirements.Text = "Enforce Metagem Requirements";
            this.checkBoxEnforceMetagemRequirements.UseVisualStyleBackColor = true;
            this.checkBoxEnforceMetagemRequirements.CheckedChanged += new System.EventHandler(this.checkBoxEnforceMetagemRequirements_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Threat Scale: ";
            // 
            // threatScaleFactor
            // 
            this.threatScaleFactor.Location = new System.Drawing.Point(83, 61);
            this.threatScaleFactor.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.threatScaleFactor.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.threatScaleFactor.Name = "threatScaleFactor";
            this.threatScaleFactor.Size = new System.Drawing.Size(120, 20);
            this.threatScaleFactor.TabIndex = 3;
            this.threatScaleFactor.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.threatScaleFactor.ValueChanged += new System.EventHandler(this.threatScaleFactor_ValueChanged);
            // 
            // labelBossAttack
            // 
            this.labelBossAttack.AutoSize = true;
            this.labelBossAttack.Location = new System.Drawing.Point(3, 37);
            this.labelBossAttack.Name = "labelBossAttack";
            this.labelBossAttack.Size = new System.Drawing.Size(70, 13);
            this.labelBossAttack.TabIndex = 4;
            this.labelBossAttack.Text = "Boss Attack: ";
            // 
            // bossAttackValue
            // 
            this.bossAttackValue.Location = new System.Drawing.Point(83, 35);
            this.bossAttackValue.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.bossAttackValue.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.bossAttackValue.Name = "bossAttackValue";
            this.bossAttackValue.Size = new System.Drawing.Size(120, 20);
            this.bossAttackValue.TabIndex = 2;
            this.bossAttackValue.Value = new decimal(new int[] {
            20000,
            0,
            0,
            0});
            this.bossAttackValue.ValueChanged += new System.EventHandler(this.bossAttackValue_ValueChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(6, 19);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(86, 23);
            this.button1.TabIndex = 20;
            this.button1.Text = "Show Talents";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Location = new System.Drawing.Point(6, 138);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(197, 56);
            this.groupBox1.TabIndex = 16;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Misc";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 89);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(88, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Mitigation Scale: ";
            // 
            // mitigationScaleFactor
            // 
            this.mitigationScaleFactor.Location = new System.Drawing.Point(83, 87);
            this.mitigationScaleFactor.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.mitigationScaleFactor.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.mitigationScaleFactor.Name = "mitigationScaleFactor";
            this.mitigationScaleFactor.Size = new System.Drawing.Size(120, 20);
            this.mitigationScaleFactor.TabIndex = 3;
            this.mitigationScaleFactor.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.mitigationScaleFactor.ValueChanged += new System.EventHandler(this.mitigationScaleFactor_ValueChanged);
            // 
            // CalculationOptionsPanelProtWarr
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.labelBossAttack);
            this.Controls.Add(this.bossAttackValue);
            this.Controls.Add(this.mitigationScaleFactor);
            this.Controls.Add(this.threatScaleFactor);
            this.Controls.Add(this.checkBoxEnforceMetagemRequirements);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.comboBoxTargetLevel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "CalculationOptionsPanelProtWarr";
            this.Size = new System.Drawing.Size(221, 337);
            ((System.ComponentModel.ISupportInitialize)(this.threatScaleFactor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bossAttackValue)).EndInit();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mitigationScaleFactor)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox comboBoxTargetLevel;
		private System.Windows.Forms.CheckBox checkBoxEnforceMetagemRequirements;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown threatScaleFactor;
        private System.Windows.Forms.Label labelBossAttack;
        private System.Windows.Forms.NumericUpDown bossAttackValue;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown mitigationScaleFactor;
	}
}
