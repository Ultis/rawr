namespace Rawr
{
	partial class FormModelChooser
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
            this.listBoxModels = new System.Windows.Forms.ListBox();
            this.buttonLoad = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(249, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Please choose a calculation model for Rawr to use:";
            // 
            // listBoxModels
            // 
            this.listBoxModels.FormattingEnabled = true;
            this.listBoxModels.Items.AddRange(new object[] {
            "Bear",
            "Cat"});
            this.listBoxModels.Location = new System.Drawing.Point(12, 25);
            this.listBoxModels.Name = "listBoxModels";
            this.listBoxModels.Size = new System.Drawing.Size(249, 43);
            this.listBoxModels.TabIndex = 1;
            this.listBoxModels.SelectedIndexChanged += new System.EventHandler(this.listBoxModels_SelectedIndexChanged);
            this.listBoxModels.DoubleClick += new System.EventHandler(this.listBoxModels_DoubleClick);
            // 
            // buttonLoad
            // 
            this.buttonLoad.Location = new System.Drawing.Point(186, 74);
            this.buttonLoad.Name = "buttonLoad";
            this.buttonLoad.Size = new System.Drawing.Size(75, 23);
            this.buttonLoad.TabIndex = 2;
            this.buttonLoad.Text = "Load Rawr";
            this.buttonLoad.UseVisualStyleBackColor = true;
            this.buttonLoad.Click += new System.EventHandler(this.buttonLoad_Click);
            // 
            // FormModelChooser
            // 
            this.AcceptButton = this.buttonLoad;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(273, 109);
            this.ControlBox = false;
            this.Controls.Add(this.buttonLoad);
            this.Controls.Add(this.listBoxModels);
            this.Controls.Add(this.label1);
            this.Name = "FormModelChooser";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Rawr Model Chooser";
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ListBox listBoxModels;
		private System.Windows.Forms.Button buttonLoad;
	}
}