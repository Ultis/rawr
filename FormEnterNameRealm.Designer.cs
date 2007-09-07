namespace Rawr
{
    partial class FormEnterNameRealm
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
			this.buttonOK = new System.Windows.Forms.Button();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.textBoxName = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.textBoxRealm = new System.Windows.Forms.TextBox();
			this.radioButtonUS = new System.Windows.Forms.RadioButton();
			this.radioButtonEU = new System.Windows.Forms.RadioButton();
			this.label3 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// buttonOK
			// 
			this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonOK.Location = new System.Drawing.Point(143, 74);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.Size = new System.Drawing.Size(75, 23);
			this.buttonOK.TabIndex = 4;
			this.buttonOK.Text = "OK";
			this.buttonOK.UseVisualStyleBackColor = true;
			// 
			// buttonCancel
			// 
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.Location = new System.Drawing.Point(62, 74);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(75, 23);
			this.buttonCancel.TabIndex = 5;
			this.buttonCancel.Text = "Cancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 32);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(87, 13);
			this.label1.TabIndex = 2;
			this.label1.Text = "Character Name:";
			// 
			// textBoxName
			// 
			this.textBoxName.Location = new System.Drawing.Point(12, 48);
			this.textBoxName.Name = "textBoxName";
			this.textBoxName.Size = new System.Drawing.Size(100, 20);
			this.textBoxName.TabIndex = 2;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(115, 32);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(40, 13);
			this.label2.TabIndex = 4;
			this.label2.Text = "Realm:";
			// 
			// textBoxRealm
			// 
			this.textBoxRealm.Location = new System.Drawing.Point(118, 48);
			this.textBoxRealm.Name = "textBoxRealm";
			this.textBoxRealm.Size = new System.Drawing.Size(100, 20);
			this.textBoxRealm.TabIndex = 3;
			// 
			// radioButtonUS
			// 
			this.radioButtonUS.AutoSize = true;
			this.radioButtonUS.Checked = true;
			this.radioButtonUS.Location = new System.Drawing.Point(65, 12);
			this.radioButtonUS.Name = "radioButtonUS";
			this.radioButtonUS.Size = new System.Drawing.Size(40, 17);
			this.radioButtonUS.TabIndex = 0;
			this.radioButtonUS.TabStop = true;
			this.radioButtonUS.Text = "US";
			this.radioButtonUS.UseVisualStyleBackColor = true;
			// 
			// radioButtonEU
			// 
			this.radioButtonEU.AutoSize = true;
			this.radioButtonEU.Location = new System.Drawing.Point(111, 12);
			this.radioButtonEU.Name = "radioButtonEU";
			this.radioButtonEU.Size = new System.Drawing.Size(40, 17);
			this.radioButtonEU.TabIndex = 1;
			this.radioButtonEU.Text = "EU";
			this.radioButtonEU.UseVisualStyleBackColor = true;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(12, 14);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(47, 13);
			this.label3.TabIndex = 2;
			this.label3.Text = "Region: ";
			// 
			// FormEnterNameRealm
			// 
			this.AcceptButton = this.buttonOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.buttonCancel;
			this.ClientSize = new System.Drawing.Size(230, 109);
			this.Controls.Add(this.radioButtonEU);
			this.Controls.Add(this.radioButtonUS);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.textBoxRealm);
			this.Controls.Add(this.textBoxName);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.buttonCancel);
			this.Controls.Add(this.buttonOK);
			this.Name = "FormEnterNameRealm";
			this.Text = "FormEnterNameRealm";
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxRealm;
		private System.Windows.Forms.RadioButton radioButtonUS;
		private System.Windows.Forms.RadioButton radioButtonEU;
		private System.Windows.Forms.Label label3;
    }
}