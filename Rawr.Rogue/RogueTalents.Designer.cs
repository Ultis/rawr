namespace Rawr.Rogue {
    partial class RogueTalents {
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.groupBoxAssassination = new System.Windows.Forms.GroupBox();
            this.groupBoxCombat = new System.Windows.Forms.GroupBox();
            this.groupBoxSubtlety = new System.Windows.Forms.GroupBox();
            this.panelAssassination = new System.Windows.Forms.Panel();
            this.panelCombat = new System.Windows.Forms.Panel();
            this.panelSubtlety = new System.Windows.Forms.Panel();
            this.groupBoxAssassination.SuspendLayout();
            this.groupBoxCombat.SuspendLayout();
            this.groupBoxSubtlety.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxAssassination
            // 
            this.groupBoxAssassination.Controls.Add(this.panelAssassination);
            this.groupBoxAssassination.Location = new System.Drawing.Point(34, 12);
            this.groupBoxAssassination.Name = "groupBoxAssassination";
            this.groupBoxAssassination.Size = new System.Drawing.Size(231, 666);
            this.groupBoxAssassination.TabIndex = 0;
            this.groupBoxAssassination.TabStop = false;
            this.groupBoxAssassination.Text = "Assassination";
            // 
            // groupBoxCombat
            // 
            this.groupBoxCombat.Controls.Add(this.panelCombat);
            this.groupBoxCombat.Location = new System.Drawing.Point(297, 12);
            this.groupBoxCombat.Name = "groupBoxCombat";
            this.groupBoxCombat.Size = new System.Drawing.Size(231, 666);
            this.groupBoxCombat.TabIndex = 1;
            this.groupBoxCombat.TabStop = false;
            this.groupBoxCombat.Text = "Combat";
            // 
            // groupBoxSubtlety
            // 
            this.groupBoxSubtlety.Controls.Add(this.panelSubtlety);
            this.groupBoxSubtlety.Location = new System.Drawing.Point(560, 12);
            this.groupBoxSubtlety.Name = "groupBoxSubtlety";
            this.groupBoxSubtlety.Size = new System.Drawing.Size(231, 666);
            this.groupBoxSubtlety.TabIndex = 1;
            this.groupBoxSubtlety.TabStop = false;
            this.groupBoxSubtlety.Text = "Subtlety";
            // 
            // panelAssassination
            // 
            this.panelAssassination.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelAssassination.Location = new System.Drawing.Point(3, 16);
            this.panelAssassination.Name = "panelAssassination";
            this.panelAssassination.Size = new System.Drawing.Size(225, 647);
            this.panelAssassination.TabIndex = 0;
            // 
            // panelCombat
            // 
            this.panelCombat.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelCombat.Location = new System.Drawing.Point(3, 16);
            this.panelCombat.Name = "panelCombat";
            this.panelCombat.Size = new System.Drawing.Size(225, 647);
            this.panelCombat.TabIndex = 0;
            // 
            // panelSubtlety
            // 
            this.panelSubtlety.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelSubtlety.Location = new System.Drawing.Point(3, 16);
            this.panelSubtlety.Name = "panelSubtlety";
            this.panelSubtlety.Size = new System.Drawing.Size(225, 647);
            this.panelSubtlety.TabIndex = 0;
            // 
            // RogueTalents
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(824, 690);
            this.Controls.Add(this.groupBoxSubtlety);
            this.Controls.Add(this.groupBoxCombat);
            this.Controls.Add(this.groupBoxAssassination);
            this.Name = "RogueTalents";
            this.Text = "RogueTalents";
            this.groupBoxAssassination.ResumeLayout(false);
            this.groupBoxCombat.ResumeLayout(false);
            this.groupBoxSubtlety.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxAssassination;
        private System.Windows.Forms.GroupBox groupBoxCombat;
        private System.Windows.Forms.GroupBox groupBoxSubtlety;
        private System.Windows.Forms.Panel panelAssassination;
        private System.Windows.Forms.Panel panelCombat;
        private System.Windows.Forms.Panel panelSubtlety;
    }
}