namespace Rawr.Tankadin
{
    partial class CalculationOptionsPanelTankadin
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
            this.lblPlayerLevel = new System.Windows.Forms.Label();
            this.nudPlayerLevel = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.nudTargetLevel = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.nudPlayerLevel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTargetLevel)).BeginInit();
            this.SuspendLayout();
            // 
            // lblPlayerLevel
            // 
            this.lblPlayerLevel.AutoSize = true;
            this.lblPlayerLevel.Location = new System.Drawing.Point(3, 6);
            this.lblPlayerLevel.Name = "lblPlayerLevel";
            this.lblPlayerLevel.Size = new System.Drawing.Size(68, 13);
            this.lblPlayerLevel.TabIndex = 0;
            this.lblPlayerLevel.Text = "Player Level:";
            // 
            // nudPlayerLevel
            // 
            this.nudPlayerLevel.Location = new System.Drawing.Point(87, 4);
            this.nudPlayerLevel.Maximum = new decimal(new int[] {
            80,
            0,
            0,
            0});
            this.nudPlayerLevel.Minimum = new decimal(new int[] {
            70,
            0,
            0,
            0});
            this.nudPlayerLevel.Name = "nudPlayerLevel";
            this.nudPlayerLevel.Size = new System.Drawing.Size(120, 20);
            this.nudPlayerLevel.TabIndex = 1;
            this.nudPlayerLevel.Value = new decimal(new int[] {
            70,
            0,
            0,
            0});
            this.nudPlayerLevel.ValueChanged += new System.EventHandler(this.nudPlayerLevel_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Target Level:";
            // 
            // nudTargetLevel
            // 
            this.nudTargetLevel.Location = new System.Drawing.Point(87, 31);
            this.nudTargetLevel.Maximum = new decimal(new int[] {
            83,
            0,
            0,
            0});
            this.nudTargetLevel.Minimum = new decimal(new int[] {
            70,
            0,
            0,
            0});
            this.nudTargetLevel.Name = "nudTargetLevel";
            this.nudTargetLevel.Size = new System.Drawing.Size(120, 20);
            this.nudTargetLevel.TabIndex = 3;
            this.nudTargetLevel.Value = new decimal(new int[] {
            73,
            0,
            0,
            0});
            this.nudTargetLevel.ValueChanged += new System.EventHandler(this.nudTargetLevel_ValueChanged);
            // 
            // CalculationOptionsPanelTankadin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.nudTargetLevel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.nudPlayerLevel);
            this.Controls.Add(this.lblPlayerLevel);
            this.Name = "CalculationOptionsPanelTankadin";
            this.Size = new System.Drawing.Size(210, 375);
            ((System.ComponentModel.ISupportInitialize)(this.nudPlayerLevel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTargetLevel)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblPlayerLevel;
        private System.Windows.Forms.NumericUpDown nudPlayerLevel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown nudTargetLevel;

    }
}
