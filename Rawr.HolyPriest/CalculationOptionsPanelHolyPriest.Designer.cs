namespace Rawr.HolyPriest
{
    partial class CalculationOptionsPanelHolyPriest
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
            this.trkActivity = new System.Windows.Forms.TrackBar();
            this.lblActivity = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.cbRotation = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.trkActivity)).BeginInit();
            this.SuspendLayout();
            // 
            // trkActivity
            // 
            this.trkActivity.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.trkActivity.Location = new System.Drawing.Point(84, 54);
            this.trkActivity.Maximum = 100;
            this.trkActivity.Minimum = 1;
            this.trkActivity.Name = "trkActivity";
            this.trkActivity.Size = new System.Drawing.Size(118, 42);
            this.trkActivity.TabIndex = 23;
            this.trkActivity.TickFrequency = 10;
            this.trkActivity.Value = 90;
            this.trkActivity.Scroll += new System.EventHandler(this.trkActivity_Scroll);
            // 
            // lblActivity
            // 
            this.lblActivity.AutoSize = true;
            this.lblActivity.Location = new System.Drawing.Point(128, 99);
            this.lblActivity.Name = "lblActivity";
            this.lblActivity.Size = new System.Drawing.Size(27, 13);
            this.lblActivity.TabIndex = 24;
            this.lblActivity.Text = "90%";
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(0, 51);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(78, 45);
            this.label9.TabIndex = 25;
            this.label9.Text = "Time in FSR:";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbRotation
            // 
            this.cbRotation.FormattingEnabled = true;
            this.cbRotation.Location = new System.Drawing.Point(84, 16);
            this.cbRotation.Name = "cbRotation";
            this.cbRotation.Size = new System.Drawing.Size(121, 21);
            this.cbRotation.TabIndex = 26;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 13);
            this.label1.TabIndex = 27;
            this.label1.Text = "Spell Cycle";
            // 
            // CalculationOptionsPanelHolyPriest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbRotation);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.lblActivity);
            this.Controls.Add(this.trkActivity);
            this.Name = "CalculationOptionsPanelHolyPriest";
            this.Size = new System.Drawing.Size(212, 289);
            ((System.ComponentModel.ISupportInitialize)(this.trkActivity)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TrackBar trkActivity;
        private System.Windows.Forms.Label lblActivity;
		private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox cbRotation;
        private System.Windows.Forms.Label label1;
    }
}
