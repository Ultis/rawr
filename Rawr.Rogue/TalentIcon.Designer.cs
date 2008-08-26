namespace Rawr.Rogue {
    partial class TalentIcon {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.panelIcon = new System.Windows.Forms.Panel();
            this.labelPoints = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // panelIcon
            // 
            this.panelIcon.Location = new System.Drawing.Point(0, 0);
            this.panelIcon.Name = "panelIcon";
            this.panelIcon.Size = new System.Drawing.Size(43, 45);
            this.panelIcon.TabIndex = 0;
            // 
            // labelPoints
            // 
            this.labelPoints.BackColor = System.Drawing.Color.Black;
            this.labelPoints.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.labelPoints.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPoints.ForeColor = System.Drawing.Color.White;
            this.labelPoints.Location = new System.Drawing.Point(0, 52);
            this.labelPoints.Name = "labelPoints";
            this.labelPoints.Size = new System.Drawing.Size(45, 13);
            this.labelPoints.TabIndex = 1;
            this.labelPoints.Text = "0 / X";
            this.labelPoints.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TalentIcon
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.Controls.Add(this.panelIcon);
            this.Controls.Add(this.labelPoints);
            this.Name = "TalentIcon";
            this.Size = new System.Drawing.Size(45, 65);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelIcon;
        private System.Windows.Forms.Label labelPoints;
    }
}
