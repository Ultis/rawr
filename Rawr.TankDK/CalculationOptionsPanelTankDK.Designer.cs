namespace Rawr.TankDK
{
    partial class CalculationOptionsPanelTankDK
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.cmbAttackerLevel = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.numThreatWeight = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.numSurvivalWeight = new System.Windows.Forms.NumericUpDown();
            this.numIncomingDamage = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numThreatWeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSurvivalWeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numIncomingDamage)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Attacker Level";
            // 
            // cmbAttackerLevel
            // 
            this.cmbAttackerLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAttackerLevel.FormattingEnabled = true;
            this.cmbAttackerLevel.Items.AddRange(new object[] {
            "80",
            "81",
            "82",
            "83"});
            this.cmbAttackerLevel.Location = new System.Drawing.Point(150, 9);
            this.cmbAttackerLevel.Name = "cmbAttackerLevel";
            this.cmbAttackerLevel.Size = new System.Drawing.Size(105, 21);
            this.cmbAttackerLevel.TabIndex = 1;
            this.cmbAttackerLevel.SelectedIndexChanged += new System.EventHandler(this.cmbAttackerLevel_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Threat Weight";
            // 
            // numThreatWeight
            // 
            this.numThreatWeight.DecimalPlaces = 2;
            this.numThreatWeight.Increment = new decimal(new int[] {
            5,
            0,
            0,
            131072});
            this.numThreatWeight.Location = new System.Drawing.Point(150, 36);
            this.numThreatWeight.Maximum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.numThreatWeight.Name = "numThreatWeight";
            this.numThreatWeight.Size = new System.Drawing.Size(105, 20);
            this.numThreatWeight.TabIndex = 3;
            this.numThreatWeight.Value = new decimal(new int[] {
            10,
            0,
            0,
            131072});
            this.numThreatWeight.ValueChanged += new System.EventHandler(this.numThreatWeight_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 62);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Survival Weight";
            // 
            // numSurvivalWeight
            // 
            this.numSurvivalWeight.DecimalPlaces = 2;
            this.numSurvivalWeight.Increment = new decimal(new int[] {
            5,
            0,
            0,
            131072});
            this.numSurvivalWeight.Location = new System.Drawing.Point(150, 62);
            this.numSurvivalWeight.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numSurvivalWeight.Name = "numSurvivalWeight";
            this.numSurvivalWeight.Size = new System.Drawing.Size(105, 20);
            this.numSurvivalWeight.TabIndex = 5;
            this.numSurvivalWeight.Value = new decimal(new int[] {
            10,
            0,
            0,
            65536});
            this.numSurvivalWeight.ValueChanged += new System.EventHandler(this.numSurvivalWeight_ValueChanged);
            // 
            // numIncomingDamage
            // 
            this.numIncomingDamage.Increment = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numIncomingDamage.Location = new System.Drawing.Point(150, 89);
            this.numIncomingDamage.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.numIncomingDamage.Minimum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numIncomingDamage.Name = "numIncomingDamage";
            this.numIncomingDamage.Size = new System.Drawing.Size(105, 20);
            this.numIncomingDamage.TabIndex = 7;
            this.numIncomingDamage.Value = new decimal(new int[] {
            25000,
            0,
            0,
            0});
            this.numIncomingDamage.ValueChanged += new System.EventHandler(this.numIncomingDamage_ValueChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 89);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(93, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Incoming Damage";
            // 
            // CalculationOptionsPanelTankDK
            // 
            this.Controls.Add(this.numIncomingDamage);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.numSurvivalWeight);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.numThreatWeight);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cmbAttackerLevel);
            this.Controls.Add(this.label1);
            this.Name = "CalculationOptionsPanelTankDK";
            this.Size = new System.Drawing.Size(258, 320);
            ((System.ComponentModel.ISupportInitialize)(this.numThreatWeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSurvivalWeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numIncomingDamage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbAttackerLevel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numThreatWeight;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numSurvivalWeight;
        private System.Windows.Forms.NumericUpDown numIncomingDamage;
        private System.Windows.Forms.Label label4;
    }
}
