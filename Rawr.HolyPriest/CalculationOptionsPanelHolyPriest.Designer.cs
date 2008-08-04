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
			this.cmbLength = new System.Windows.Forms.NumericUpDown();
			this.label3 = new System.Windows.Forms.Label();
			this.trkActivity = new System.Windows.Forms.TrackBar();
			this.lblActivity = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.tbnTalents = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.cmbLength)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.trkActivity)).BeginInit();
			this.SuspendLayout();
			// 
			// cmbLength
			// 
			this.cmbLength.DecimalPlaces = 1;
			this.cmbLength.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
			this.cmbLength.Location = new System.Drawing.Point(97, 51);
			this.cmbLength.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
			this.cmbLength.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.cmbLength.Name = "cmbLength";
			this.cmbLength.Size = new System.Drawing.Size(112, 20);
			this.cmbLength.TabIndex = 20;
			this.cmbLength.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
			this.cmbLength.ValueChanged += new System.EventHandler(this.cmbLength_ValueChanged);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(22, 53);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(69, 13);
			this.label3.TabIndex = 21;
			this.label3.Text = "Fight Length:";
			// 
			// trkActivity
			// 
			this.trkActivity.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.trkActivity.Location = new System.Drawing.Point(94, 77);
			this.trkActivity.Maximum = 100;
			this.trkActivity.Minimum = 1;
			this.trkActivity.Name = "trkActivity";
			this.trkActivity.Size = new System.Drawing.Size(118, 45);
			this.trkActivity.TabIndex = 23;
			this.trkActivity.TickFrequency = 10;
			this.trkActivity.Value = 90;
			this.trkActivity.Scroll += new System.EventHandler(this.trkActivity_Scroll);
			// 
			// lblActivity
			// 
			this.lblActivity.AutoSize = true;
			this.lblActivity.Location = new System.Drawing.Point(105, 109);
			this.lblActivity.Name = "lblActivity";
			this.lblActivity.Size = new System.Drawing.Size(27, 13);
			this.lblActivity.TabIndex = 24;
			this.lblActivity.Text = "90%";
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(13, 77);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(78, 45);
			this.label9.TabIndex = 25;
			this.label9.Text = "Time in FSR:";
			this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// tbnTalents
			// 
			this.tbnTalents.Location = new System.Drawing.Point(8, 8);
			this.tbnTalents.Name = "tbnTalents";
			this.tbnTalents.Size = new System.Drawing.Size(194, 23);
			this.tbnTalents.TabIndex = 27;
			this.tbnTalents.Text = "Talents";
			this.tbnTalents.UseVisualStyleBackColor = true;
			this.tbnTalents.Click += new System.EventHandler(this.tbnTalents_Click);
			// 
			// CalculationOptionsPanelHolyPriest
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tbnTalents);
			this.Controls.Add(this.label9);
			this.Controls.Add(this.lblActivity);
			this.Controls.Add(this.trkActivity);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.cmbLength);
			this.Name = "CalculationOptionsPanelHolyPriest";
			this.Size = new System.Drawing.Size(212, 289);
			((System.ComponentModel.ISupportInitialize)(this.cmbLength)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.trkActivity)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown cmbLength;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TrackBar trkActivity;
        private System.Windows.Forms.Label lblActivity;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button tbnTalents;
    }
}
