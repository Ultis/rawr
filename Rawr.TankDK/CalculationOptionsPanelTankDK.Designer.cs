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
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 12);
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
            this.cmbAttackerLevel.Location = new System.Drawing.Point(92, 9);
            this.cmbAttackerLevel.Name = "cmbAttackerLevel";
            this.cmbAttackerLevel.Size = new System.Drawing.Size(121, 21);
            this.cmbAttackerLevel.TabIndex = 1;
            this.cmbAttackerLevel.SelectedIndexChanged += new System.EventHandler(this.cmbAttackerLevel_SelectedIndexChanged);
            // 
            // CalculationOptionsPanelTankDK
            // 
            this.Controls.Add(this.cmbAttackerLevel);
            this.Controls.Add(this.label1);
            this.Name = "CalculationOptionsPanelTankDK";
            this.Size = new System.Drawing.Size(216, 320);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbAttackerLevel;
    }
}
