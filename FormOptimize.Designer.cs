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
            this.groupBoxOptions = new System.Windows.Forms.GroupBox();
            this.checkBoxOverrideReenchant = new System.Windows.Forms.CheckBox();
            this.checkBoxOverrideRegem = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.trackBarThoroughness = new System.Windows.Forms.TrackBar();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonUpgrades = new System.Windows.Forms.Button();
            this.checkBoxOptimizeFood = new System.Windows.Forms.CheckBox();
            this.checkBoxOptimizeElixir = new System.Windows.Forms.CheckBox();
            this.checkBoxMixology = new System.Windows.Forms.CheckBox();
            this.groupBoxRequirements.SuspendLayout();
            this.groupBoxOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarThoroughness)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonOptimize
            // 
            this.buttonOptimize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOptimize.Location = new System.Drawing.Point(491, 324);
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
            this.progressBarMain.Size = new System.Drawing.Size(554, 23);
            this.progressBarMain.TabIndex = 1;
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(301, 324);
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
            this.progressBarAlt.Size = new System.Drawing.Size(554, 8);
            this.progressBarAlt.TabIndex = 1;
            // 
            // comboBoxCalculationToOptimize
            // 
            this.comboBoxCalculationToOptimize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxCalculationToOptimize.FormattingEnabled = true;
            this.comboBoxCalculationToOptimize.Location = new System.Drawing.Point(138, 51);
            this.comboBoxCalculationToOptimize.Name = "comboBoxCalculationToOptimize";
            this.comboBoxCalculationToOptimize.Size = new System.Drawing.Size(177, 21);
            this.comboBoxCalculationToOptimize.TabIndex = 3;
            // 
            // labelCalculationToOptimize
            // 
            this.labelCalculationToOptimize.AutoSize = true;
            this.labelCalculationToOptimize.Location = new System.Drawing.Point(12, 54);
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
            this.groupBoxRequirements.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBoxRequirements.Controls.Add(this.buttonAddRequirement);
            this.groupBoxRequirements.Location = new System.Drawing.Point(12, 106);
            this.groupBoxRequirements.Name = "groupBoxRequirements";
            this.groupBoxRequirements.Size = new System.Drawing.Size(303, 176);
            this.groupBoxRequirements.TabIndex = 6;
            this.groupBoxRequirements.TabStop = false;
            this.groupBoxRequirements.Text = "Additional Requirements";
            // 
            // labelInfo
            // 
            this.labelInfo.Location = new System.Drawing.Point(12, 9);
            this.labelInfo.Name = "labelInfo";
            this.labelInfo.Size = new System.Drawing.Size(554, 39);
            this.labelInfo.TabIndex = 4;
            this.labelInfo.Text = resources.GetString("labelInfo.Text");
            // 
            // groupBoxOptions
            // 
            this.groupBoxOptions.Controls.Add(this.checkBoxOverrideReenchant);
            this.groupBoxOptions.Controls.Add(this.checkBoxOverrideRegem);
            this.groupBoxOptions.Controls.Add(this.label3);
            this.groupBoxOptions.Controls.Add(this.trackBarThoroughness);
            this.groupBoxOptions.Controls.Add(this.label2);
            this.groupBoxOptions.Controls.Add(this.label1);
            this.groupBoxOptions.Location = new System.Drawing.Point(321, 51);
            this.groupBoxOptions.Name = "groupBoxOptions";
            this.groupBoxOptions.Size = new System.Drawing.Size(245, 231);
            this.groupBoxOptions.TabIndex = 7;
            this.groupBoxOptions.TabStop = false;
            this.groupBoxOptions.Text = "Options";
            // 
            // checkBoxOverrideReenchant
            // 
            this.checkBoxOverrideReenchant.AutoSize = true;
            this.checkBoxOverrideReenchant.Location = new System.Drawing.Point(117, 208);
            this.checkBoxOverrideReenchant.Name = "checkBoxOverrideReenchant";
            this.checkBoxOverrideReenchant.Size = new System.Drawing.Size(122, 17);
            this.checkBoxOverrideReenchant.TabIndex = 7;
            this.checkBoxOverrideReenchant.Text = "Override Reenchant";
            this.checkBoxOverrideReenchant.UseVisualStyleBackColor = true;
            // 
            // checkBoxOverrideRegem
            // 
            this.checkBoxOverrideRegem.AutoSize = true;
            this.checkBoxOverrideRegem.Location = new System.Drawing.Point(9, 208);
            this.checkBoxOverrideRegem.Name = "checkBoxOverrideRegem";
            this.checkBoxOverrideRegem.Size = new System.Drawing.Size(103, 17);
            this.checkBoxOverrideRegem.TabIndex = 6;
            this.checkBoxOverrideRegem.Text = "Override Regem";
            this.checkBoxOverrideRegem.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(6, 172);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(233, 33);
            this.label3.TabIndex = 4;
            this.label3.Text = "Choose whether to let the Optimizer regem or reenchant items that don\'t specifica" +
                "lly allow it.";
            // 
            // trackBarThoroughness
            // 
            this.trackBarThoroughness.Location = new System.Drawing.Point(90, 126);
            this.trackBarThoroughness.Maximum = 400;
            this.trackBarThoroughness.Minimum = 1;
            this.trackBarThoroughness.Name = "trackBarThoroughness";
            this.trackBarThoroughness.Size = new System.Drawing.Size(149, 45);
            this.trackBarThoroughness.TabIndex = 5;
            this.trackBarThoroughness.TickFrequency = 10;
            this.trackBarThoroughness.Value = 150;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 129);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Thoroughness:";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(233, 107);
            this.label1.TabIndex = 4;
            this.label1.Text = resources.GetString("label1.Text");
            // 
            // buttonUpgrades
            // 
            this.buttonUpgrades.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonUpgrades.Location = new System.Drawing.Point(382, 324);
            this.buttonUpgrades.Name = "buttonUpgrades";
            this.buttonUpgrades.Size = new System.Drawing.Size(103, 23);
            this.buttonUpgrades.TabIndex = 8;
            this.buttonUpgrades.Text = "Build Upgrade List";
            this.buttonUpgrades.UseVisualStyleBackColor = true;
            this.buttonUpgrades.Click += new System.EventHandler(this.buttonUpgrades_Click);
            // 
            // checkBoxOptimizeFood
            // 
            this.checkBoxOptimizeFood.AutoSize = true;
            this.checkBoxOptimizeFood.Location = new System.Drawing.Point(15, 83);
            this.checkBoxOptimizeFood.Name = "checkBoxOptimizeFood";
            this.checkBoxOptimizeFood.Size = new System.Drawing.Size(93, 17);
            this.checkBoxOptimizeFood.TabIndex = 9;
            this.checkBoxOptimizeFood.Text = "Optimize Food";
            this.checkBoxOptimizeFood.UseVisualStyleBackColor = true;
            // 
            // checkBoxOptimizeElixir
            // 
            this.checkBoxOptimizeElixir.AutoSize = true;
            this.checkBoxOptimizeElixir.Location = new System.Drawing.Point(114, 83);
            this.checkBoxOptimizeElixir.Name = "checkBoxOptimizeElixir";
            this.checkBoxOptimizeElixir.Size = new System.Drawing.Size(120, 17);
            this.checkBoxOptimizeElixir.TabIndex = 10;
            this.checkBoxOptimizeElixir.Text = "Optimize Elixir/Flask";
            this.checkBoxOptimizeElixir.UseVisualStyleBackColor = true;
            // 
            // checkBoxMixology
            // 
            this.checkBoxMixology.AutoSize = true;
            this.checkBoxMixology.Location = new System.Drawing.Point(240, 83);
            this.checkBoxMixology.Name = "checkBoxMixology";
            this.checkBoxMixology.Size = new System.Drawing.Size(67, 17);
            this.checkBoxMixology.TabIndex = 11;
            this.checkBoxMixology.Text = "Mixology";
            this.checkBoxMixology.UseVisualStyleBackColor = true;
            // 
            // FormOptimize
            // 
            this.AcceptButton = this.buttonOptimize;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(578, 359);
            this.ControlBox = false;
            this.Controls.Add(this.checkBoxMixology);
            this.Controls.Add(this.checkBoxOptimizeElixir);
            this.Controls.Add(this.checkBoxOptimizeFood);
            this.Controls.Add(this.buttonUpgrades);
            this.Controls.Add(this.groupBoxOptions);
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
            this.Text = "Rawr Optimizer Tools";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormOptimize_FormClosed);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormOptimize_FormClosing);
            this.groupBoxRequirements.ResumeLayout(false);
            this.groupBoxOptions.ResumeLayout(false);
            this.groupBoxOptions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarThoroughness)).EndInit();
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
        private System.Windows.Forms.GroupBox groupBoxOptions;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TrackBar trackBarThoroughness;
        private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button buttonUpgrades;
        private System.Windows.Forms.CheckBox checkBoxOverrideRegem;
        private System.Windows.Forms.CheckBox checkBoxOverrideReenchant;
        private System.Windows.Forms.CheckBox checkBoxOptimizeFood;
        private System.Windows.Forms.CheckBox checkBoxOptimizeElixir;
        private System.Windows.Forms.CheckBox checkBoxMixology;
	}
}