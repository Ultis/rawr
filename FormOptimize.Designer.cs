namespace Rawr
{
	partial class FormOptimize
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormOptimize));
			this.buttonOptimize = new System.Windows.Forms.Button();
			this.progressBarMain = new System.Windows.Forms.ProgressBar();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.labelMax = new System.Windows.Forms.Label();
			this.progressBarAlt = new System.Windows.Forms.ProgressBar();
			this.comboBoxCalculationToOptimize = new System.Windows.Forms.ComboBox();
			this.labelCalculationToOptimize = new System.Windows.Forms.Label();
			this.buttonAddRequirement = new System.Windows.Forms.Button();
			this.groupBoxRequirements = new System.Windows.Forms.GroupBox();
			this.labelInfo = new System.Windows.Forms.Label();
			this.groupBoxRequirements.SuspendLayout();
			this.SuspendLayout();
			// 
			// buttonOptimize
			// 
			this.buttonOptimize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonOptimize.Location = new System.Drawing.Point(237, 324);
			this.buttonOptimize.Name = "buttonOptimize";
			this.buttonOptimize.Size = new System.Drawing.Size(75, 23);
			this.buttonOptimize.TabIndex = 0;
			this.buttonOptimize.Text = "Optimize";
			this.buttonOptimize.UseVisualStyleBackColor = true;
			this.buttonOptimize.Click += new System.EventHandler(this.buttonOptimize_Click);
			// 
			// progressBarMain
			// 
			this.progressBarMain.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.progressBarMain.Location = new System.Drawing.Point(12, 295);
			this.progressBarMain.Name = "progressBarMain";
			this.progressBarMain.Size = new System.Drawing.Size(300, 23);
			this.progressBarMain.TabIndex = 1;
			// 
			// buttonCancel
			// 
			this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.Location = new System.Drawing.Point(156, 324);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(75, 23);
			this.buttonCancel.TabIndex = 0;
			this.buttonCancel.Text = "Cancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
			// 
			// labelMax
			// 
			this.labelMax.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.labelMax.AutoSize = true;
			this.labelMax.Location = new System.Drawing.Point(12, 331);
			this.labelMax.Name = "labelMax";
			this.labelMax.Size = new System.Drawing.Size(0, 13);
			this.labelMax.TabIndex = 2;
			// 
			// progressBarAlt
			// 
			this.progressBarAlt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.progressBarAlt.Location = new System.Drawing.Point(12, 288);
			this.progressBarAlt.Name = "progressBarAlt";
			this.progressBarAlt.Size = new System.Drawing.Size(300, 8);
			this.progressBarAlt.TabIndex = 1;
			// 
			// comboBoxCalculationToOptimize
			// 
			this.comboBoxCalculationToOptimize.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.comboBoxCalculationToOptimize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxCalculationToOptimize.FormattingEnabled = true;
			this.comboBoxCalculationToOptimize.Location = new System.Drawing.Point(138, 90);
			this.comboBoxCalculationToOptimize.Name = "comboBoxCalculationToOptimize";
			this.comboBoxCalculationToOptimize.Size = new System.Drawing.Size(174, 21);
			this.comboBoxCalculationToOptimize.TabIndex = 3;
			// 
			// labelCalculationToOptimize
			// 
			this.labelCalculationToOptimize.AutoSize = true;
			this.labelCalculationToOptimize.Location = new System.Drawing.Point(12, 93);
			this.labelCalculationToOptimize.Name = "labelCalculationToOptimize";
			this.labelCalculationToOptimize.Size = new System.Drawing.Size(120, 13);
			this.labelCalculationToOptimize.TabIndex = 4;
			this.labelCalculationToOptimize.Text = "Calculation to Optimize: ";
			// 
			// buttonAddRequirement
			// 
			this.buttonAddRequirement.Location = new System.Drawing.Point(6, 19);
			this.buttonAddRequirement.Name = "buttonAddRequirement";
			this.buttonAddRequirement.Size = new System.Drawing.Size(55, 23);
			this.buttonAddRequirement.TabIndex = 5;
			this.buttonAddRequirement.Text = "Add";
			this.buttonAddRequirement.UseVisualStyleBackColor = true;
			this.buttonAddRequirement.Click += new System.EventHandler(this.buttonAddRequirement_Click);
			// 
			// groupBoxRequirements
			// 
			this.groupBoxRequirements.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBoxRequirements.Controls.Add(this.buttonAddRequirement);
			this.groupBoxRequirements.Location = new System.Drawing.Point(12, 117);
			this.groupBoxRequirements.Name = "groupBoxRequirements";
			this.groupBoxRequirements.Size = new System.Drawing.Size(300, 165);
			this.groupBoxRequirements.TabIndex = 6;
			this.groupBoxRequirements.TabStop = false;
			this.groupBoxRequirements.Text = "Additional Requirements";
			// 
			// labelInfo
			// 
			this.labelInfo.Location = new System.Drawing.Point(12, 9);
			this.labelInfo.Name = "labelInfo";
			this.labelInfo.Size = new System.Drawing.Size(300, 78);
			this.labelInfo.TabIndex = 4;
			this.labelInfo.Text = resources.GetString("labelInfo.Text");
			// 
			// FormOptimize
			// 
			this.AcceptButton = this.buttonOptimize;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.buttonCancel;
			this.ClientSize = new System.Drawing.Size(324, 359);
			this.ControlBox = false;
			this.Controls.Add(this.groupBoxRequirements);
			this.Controls.Add(this.labelInfo);
			this.Controls.Add(this.labelCalculationToOptimize);
			this.Controls.Add(this.comboBoxCalculationToOptimize);
			this.Controls.Add(this.labelMax);
			this.Controls.Add(this.progressBarMain);
			this.Controls.Add(this.buttonCancel);
			this.Controls.Add(this.buttonOptimize);
			this.Controls.Add(this.progressBarAlt);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "FormOptimize";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Rawr Optimizer";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormOptimize_FormClosing);
			this.groupBoxRequirements.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button buttonOptimize;
		private System.Windows.Forms.ProgressBar progressBarMain;
		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.Label labelMax;
		private System.Windows.Forms.ProgressBar progressBarAlt;
		private System.Windows.Forms.ComboBox comboBoxCalculationToOptimize;
		private System.Windows.Forms.Label labelCalculationToOptimize;
		private System.Windows.Forms.Button buttonAddRequirement;
		private System.Windows.Forms.GroupBox groupBoxRequirements;
		private System.Windows.Forms.Label labelInfo;
	}
}