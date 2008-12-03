namespace Rawr
{
    partial class FormEnterId
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
            this.textItemId = new System.Windows.Forms.TextBox();
            this.lblTextAddItemID = new System.Windows.Forms.Label();
            this.cbArmory = new System.Windows.Forms.CheckBox();
            this.cbWowhead = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttonOK
            // 
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(165, 90);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 0;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(84, 90);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 1;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // textItemId
            // 
            this.textItemId.Location = new System.Drawing.Point(12, 39);
            this.textItemId.Name = "textItemId";
            this.textItemId.Size = new System.Drawing.Size(235, 20);
            this.textItemId.TabIndex = 3;
            // 
            // lblTextAddItemID
            // 
            this.lblTextAddItemID.AutoSize = true;
            this.lblTextAddItemID.Location = new System.Drawing.Point(12, 10);
            this.lblTextAddItemID.Name = "lblTextAddItemID";
            this.lblTextAddItemID.Size = new System.Drawing.Size(147, 13);
            this.lblTextAddItemID.TabIndex = 4;
            this.lblTextAddItemID.Text = "Enter the Item ID, Item Name,";
            // 
            // cbArmory
            // 
            this.cbArmory.AutoSize = true;
            this.cbArmory.Checked = true;
            this.cbArmory.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbArmory.Location = new System.Drawing.Point(12, 66);
            this.cbArmory.Name = "cbArmory";
            this.cbArmory.Size = new System.Drawing.Size(58, 17);
            this.cbArmory.TabIndex = 5;
            this.cbArmory.Text = "Armory";
            this.cbArmory.UseVisualStyleBackColor = true;
            // 
            // cbWowhead
            // 
            this.cbWowhead.AutoSize = true;
            this.cbWowhead.Checked = true;
            this.cbWowhead.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbWowhead.Location = new System.Drawing.Point(98, 66);
            this.cbWowhead.Name = "cbWowhead";
            this.cbWowhead.Size = new System.Drawing.Size(75, 17);
            this.cbWowhead.TabIndex = 6;
            this.cbWowhead.Text = "Wowhead";
            this.cbWowhead.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(195, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "or Database Link (Wowhead, Thottbot):";
            // 
            // FormEnterId
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(259, 132);
            this.ControlBox = false;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbWowhead);
            this.Controls.Add(this.cbArmory);
            this.Controls.Add(this.lblTextAddItemID);
            this.Controls.Add(this.textItemId);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            //this.Name = "FormEnterId";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Add item";
            this.Load += new System.EventHandler(this.FormEnterId_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.TextBox textItemId;
        private System.Windows.Forms.Label lblTextAddItemID;
        private System.Windows.Forms.CheckBox cbArmory;
        private System.Windows.Forms.CheckBox cbWowhead;
        private System.Windows.Forms.Label label1;
    }
}