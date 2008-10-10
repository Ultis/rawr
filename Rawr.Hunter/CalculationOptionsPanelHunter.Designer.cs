namespace Rawr.Hunter
{
    partial class CalculationOptionsPanelHunter
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
			this.label1 = new System.Windows.Forms.Label();
			this.cmbTargetLevel = new System.Windows.Forms.ComboBox();
			this.groupBoxPetOptions = new System.Windows.Forms.GroupBox();
			this.label7 = new System.Windows.Forms.Label();
			this.comboPetPriority3 = new System.Windows.Forms.ComboBox();
			this.label6 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.comboPetPriority2 = new System.Windows.Forms.ComboBox();
			this.comboPetPriority1 = new System.Windows.Forms.ComboBox();
			this.label4 = new System.Windows.Forms.Label();
			this.comboPetFamily = new System.Windows.Forms.ComboBox();
			this.trackBarTargetArmor = new System.Windows.Forms.TrackBar();
            this.lblTargetArmorValue = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();

			this.groupBoxPetOptions.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.trackBarTargetArmor)).BeginInit();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(3, 14);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(70, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Target Level:";
			// 
			// cmbTargetLevel
			// 
			this.cmbTargetLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbTargetLevel.FormattingEnabled = true;
			this.cmbTargetLevel.Items.AddRange(new object[] {
            "70",
            "71",
            "72",
            "73"});
            this.cmbTargetLevel.Location = new System.Drawing.Point(79, 11);
            this.cmbTargetLevel.Name = "cmbTargetLevel";
            this.cmbTargetLevel.Size = new System.Drawing.Size(121, 21);
            this.cmbTargetLevel.TabIndex = 1;
            this.cmbTargetLevel.SelectedIndexChanged += new System.EventHandler(this.cmbTargetLevel_SelectedIndexChanged);
            // 
            // groupBoxPetOptions
            // 
            this.groupBoxPetOptions.Controls.Add(this.label7);
            this.groupBoxPetOptions.Controls.Add(this.comboPetPriority3);
            this.groupBoxPetOptions.Controls.Add(this.label6);
            this.groupBoxPetOptions.Controls.Add(this.label5);
            this.groupBoxPetOptions.Controls.Add(this.comboPetPriority2);
            this.groupBoxPetOptions.Controls.Add(this.comboPetPriority1);
            this.groupBoxPetOptions.Controls.Add(this.label4);
            this.groupBoxPetOptions.Controls.Add(this.comboPetFamily);
            this.groupBoxPetOptions.Location = new System.Drawing.Point(6, 136);
            this.groupBoxPetOptions.Name = "groupBoxPetOptions";
            this.groupBoxPetOptions.Size = new System.Drawing.Size(194, 150);
            this.groupBoxPetOptions.TabIndex = 8;
            this.groupBoxPetOptions.TabStop = false;
            this.groupBoxPetOptions.Text = "Pet Options";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 116);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(50, 13);
            this.label7.TabIndex = 7;
            this.label7.Text = "Priority 3:";
            // 
            // comboPetPriority3
            // 
            this.comboPetPriority3.Enabled = false;
            this.comboPetPriority3.FormattingEnabled = true;
            this.comboPetPriority3.Location = new System.Drawing.Point(86, 113);
            this.comboPetPriority3.Name = "comboPetPriority3";
            this.comboPetPriority3.Size = new System.Drawing.Size(101, 21);
            this.comboPetPriority3.TabIndex = 6;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 88);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(50, 13);
            this.label6.TabIndex = 5;
            this.label6.Text = "Priority 2:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 60);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(50, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Priority 1:";
            // 
            // comboPetPriority2
            // 
            this.comboPetPriority2.Enabled = false;
            this.comboPetPriority2.FormattingEnabled = true;
            this.comboPetPriority2.Location = new System.Drawing.Point(86, 85);
            this.comboPetPriority2.Name = "comboPetPriority2";
            this.comboPetPriority2.Size = new System.Drawing.Size(101, 21);
            this.comboPetPriority2.TabIndex = 3;
            // 
            // comboPetPriority1
            // 
            this.comboPetPriority1.Enabled = false;
            this.comboPetPriority1.FormattingEnabled = true;
            this.comboPetPriority1.Location = new System.Drawing.Point(86, 57);
            this.comboPetPriority1.Name = "comboPetPriority1";
            this.comboPetPriority1.Size = new System.Drawing.Size(101, 21);
            this.comboPetPriority1.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 23);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(58, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Pet Family:";
            // 
            // comboPetFamily
            // 
            this.comboPetFamily.Enabled = false;
            this.comboPetFamily.FormattingEnabled = true;
            this.comboPetFamily.Location = new System.Drawing.Point(73, 20);
            this.comboPetFamily.Name = "comboPetFamily";
            this.comboPetFamily.Size = new System.Drawing.Size(114, 21);
            this.comboPetFamily.TabIndex = 0;
            // 
            // trackBarTargetArmor
            // 
            this.trackBarTargetArmor.BackColor = System.Drawing.Color.White;
            this.trackBarTargetArmor.LargeChange = 1000;
            this.trackBarTargetArmor.Location = new System.Drawing.Point(79, 38);
            this.trackBarTargetArmor.Maximum = 9000;
            this.trackBarTargetArmor.Minimum = 3000;
            this.trackBarTargetArmor.Name = "trackBarTargetArmor";
            this.trackBarTargetArmor.Size = new System.Drawing.Size(120, 45);
            this.trackBarTargetArmor.SmallChange = 100;
            this.trackBarTargetArmor.TabIndex = 2;
            this.trackBarTargetArmor.TickFrequency = 300;
            this.trackBarTargetArmor.Value = 7700;
            this.trackBarTargetArmor.Scroll += new System.EventHandler(this.trackBarTargetArmor_Scroll);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Target Armor:";
            // 
            // lblTargetArmorValue
            // 
            this.lblTargetArmorValue.AutoSize = true;
            this.lblTargetArmorValue.Location = new System.Drawing.Point(24, 59);
            this.lblTargetArmorValue.Name = "lblTargetArmorValue";
            this.lblTargetArmorValue.Size = new System.Drawing.Size(31, 13);
            this.lblTargetArmorValue.TabIndex = 11;
            this.lblTargetArmorValue.Text = "7700";
            // 
            // CalculationOptionsPanelHunter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblTargetArmorValue);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.trackBarTargetArmor);
            this.Controls.Add(this.groupBoxPetOptions);
            this.Controls.Add(this.cmbTargetLevel);
            this.Controls.Add(this.label1);
            this.Name = "CalculationOptionsPanelHunter";
            this.Size = new System.Drawing.Size(206, 413);
            this.groupBoxPetOptions.ResumeLayout(false);
            this.groupBoxPetOptions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarTargetArmor)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox cmbTargetLevel;
        private System.Windows.Forms.GroupBox groupBoxPetOptions;
        private System.Windows.Forms.ComboBox comboPetPriority2;
        private System.Windows.Forms.ComboBox comboPetPriority1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboPetFamily;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox comboPetPriority3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TrackBar trackBarTargetArmor;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblTargetArmorValue;
    }
}
