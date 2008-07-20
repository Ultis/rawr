namespace Rawr.Warlock
{
    partial class WarlockControl
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
            this.textBoxCrit = new System.Windows.Forms.TextBox();
            this.textBoxShadowDps = new System.Windows.Forms.TextBox();
            this.textBoxSbCastTime = new System.Windows.Forms.TextBox();
            this.textBoxSbCastRatio = new System.Windows.Forms.TextBox();
            this.textBoxHit = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // textBoxCrit
            // 
            this.textBoxCrit.Location = new System.Drawing.Point(3, 29);
            this.textBoxCrit.Name = "textBoxCrit";
            this.textBoxCrit.Size = new System.Drawing.Size(100, 20);
            this.textBoxCrit.TabIndex = 2;
            this.textBoxCrit.Text = "20";
            // 
            // textBoxShadowDps
            // 
            this.textBoxShadowDps.Location = new System.Drawing.Point(3, 107);
            this.textBoxShadowDps.Name = "textBoxShadowDps";
            this.textBoxShadowDps.Size = new System.Drawing.Size(100, 20);
            this.textBoxShadowDps.TabIndex = 5;
            this.textBoxShadowDps.Text = "1600";
            // 
            // textBoxSbCastTime
            // 
            this.textBoxSbCastTime.Location = new System.Drawing.Point(3, 55);
            this.textBoxSbCastTime.Name = "textBoxSbCastTime";
            this.textBoxSbCastTime.Size = new System.Drawing.Size(100, 20);
            this.textBoxSbCastTime.TabIndex = 3;
            this.textBoxSbCastTime.Text = "2.6";
            // 
            // textBoxSbCastRatio
            // 
            this.textBoxSbCastRatio.Location = new System.Drawing.Point(3, 81);
            this.textBoxSbCastRatio.Name = "textBoxSbCastRatio";
            this.textBoxSbCastRatio.Size = new System.Drawing.Size(100, 20);
            this.textBoxSbCastRatio.TabIndex = 4;
            this.textBoxSbCastRatio.Text = "0.95";
            // 
            // textBoxHit
            // 
            this.textBoxHit.Location = new System.Drawing.Point(3, 3);
            this.textBoxHit.Name = "textBoxHit";
            this.textBoxHit.Size = new System.Drawing.Size(100, 20);
            this.textBoxHit.TabIndex = 1;
            this.textBoxHit.Text = "16";
            // 
            // WarlockControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.textBoxCrit);
            this.Controls.Add(this.textBoxShadowDps);
            this.Controls.Add(this.textBoxSbCastTime);
            this.Controls.Add(this.textBoxSbCastRatio);
            this.Controls.Add(this.textBoxHit);
            this.Name = "WarlockControl";
            this.Size = new System.Drawing.Size(106, 130);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxCrit;
        private System.Windows.Forms.TextBox textBoxShadowDps;
        private System.Windows.Forms.TextBox textBoxSbCastTime;
        private System.Windows.Forms.TextBox textBoxSbCastRatio;
        private System.Windows.Forms.TextBox textBoxHit;
    }
}
