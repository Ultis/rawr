namespace Rawr
{
    partial class FormMassGemReplacement
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.OkButton = new System.Windows.Forms.Button();
            this.buttonAddGemming = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(364, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Click on the Gem Slot to Change the gem associated with that particular slot";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(382, 7);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(105, 30);
            this.checkBox1.TabIndex = 9;
            this.checkBox1.Text = "Delete non-listed\r\nGemmings";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // OkButton
            // 
            this.OkButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OkButton.Location = new System.Drawing.Point(382, 89);
            this.OkButton.Name = "OkButton";
            this.OkButton.Size = new System.Drawing.Size(45, 33);
            this.OkButton.TabIndex = 10;
            this.OkButton.Text = "OK";
            this.OkButton.UseVisualStyleBackColor = true;
            this.OkButton.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // buttonAddGemming
            // 
            this.buttonAddGemming.Location = new System.Drawing.Point(382, 43);
            this.buttonAddGemming.Name = "buttonAddGemming";
            this.buttonAddGemming.Size = new System.Drawing.Size(102, 41);
            this.buttonAddGemming.TabIndex = 11;
            this.buttonAddGemming.Text = "Add another Gemming";
            this.buttonAddGemming.UseVisualStyleBackColor = true;
            this.buttonAddGemming.Click += new System.EventHandler(this.buttonAddGemming_Click);
            // 
            // button7
            // 
            this.button7.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button7.Location = new System.Drawing.Point(436, 89);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(51, 33);
            this.button7.TabIndex = 13;
            this.button7.Text = "Cancel";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // FormMassGemReplacement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(501, 136);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.buttonAddGemming);
            this.Controls.Add(this.OkButton);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.Name = "FormMassGemReplacement";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FormMassGemReplacement";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Button OkButton;
        private System.Windows.Forms.Button buttonAddGemming;
        private System.Windows.Forms.Button button7;
    }
}