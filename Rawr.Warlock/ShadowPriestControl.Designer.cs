namespace Rawr.Warlock
{
    partial class ShadowPriestControl
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
            this.textBoxShadowDps = new System.Windows.Forms.TextBox();
            this.textBoxHit = new System.Windows.Forms.TextBox();
            this.textBoxMbFrequency = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // textBoxShadowDps
            // 
            this.textBoxShadowDps.Location = new System.Drawing.Point(3, 55);
            this.textBoxShadowDps.Name = "textBoxShadowDps";
            this.textBoxShadowDps.Size = new System.Drawing.Size(100, 20);
            this.textBoxShadowDps.TabIndex = 3;
            this.textBoxShadowDps.Text = "1100";
            // 
            // textBoxHit
            // 
            this.textBoxHit.Location = new System.Drawing.Point(3, 3);
            this.textBoxHit.Name = "textBoxHit";
            this.textBoxHit.Size = new System.Drawing.Size(100, 20);
            this.textBoxHit.TabIndex = 1;
            this.textBoxHit.Text = "16";
            // 
            // textBoxMbFrequency
            // 
            this.textBoxMbFrequency.Location = new System.Drawing.Point(3, 29);
            this.textBoxMbFrequency.Name = "textBoxMbFrequency";
            this.textBoxMbFrequency.Size = new System.Drawing.Size(100, 20);
            this.textBoxMbFrequency.TabIndex = 2;
            this.textBoxMbFrequency.Text = "7.5";
            // 
            // ShadowPriestControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.textBoxShadowDps);
            this.Controls.Add(this.textBoxHit);
            this.Controls.Add(this.textBoxMbFrequency);
            this.Name = "ShadowPriestControl";
            this.Size = new System.Drawing.Size(106, 78);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxShadowDps;
        private System.Windows.Forms.TextBox textBoxHit;
        private System.Windows.Forms.TextBox textBoxMbFrequency;
    }
}
