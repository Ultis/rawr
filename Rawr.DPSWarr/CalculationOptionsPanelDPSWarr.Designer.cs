namespace Rawr.DPSWarr {
    partial class CalculationOptionsPanelDPSWarr {
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
            this.comboBoxTargetLevel = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.labelTargetArmorDescription = new System.Windows.Forms.Label();
            this.groupBoxCycles = new System.Windows.Forms.GroupBox();
            this.radioButton4s5r = new System.Windows.Forms.RadioButton();
            this.comboBoxArmorBosses = new System.Windows.Forms.ComboBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBoxCycles.SuspendLayout();
            this.SuspendLayout();
            // 
            // comboBoxTargetLevel
            // 
            this.comboBoxTargetLevel.FormattingEnabled = true;
            this.comboBoxTargetLevel.Location = new System.Drawing.Point(92, 14);
            this.comboBoxTargetLevel.Name = "comboBoxTargetLevel";
            this.comboBoxTargetLevel.Size = new System.Drawing.Size(105, 21);
            this.comboBoxTargetLevel.TabIndex = 0;
            this.comboBoxTargetLevel.SelectedIndexChanged += new System.EventHandler(this.comboBoxTargetLevel_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Target Level:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Target Armor:";
            // 
            // labelTargetArmorDescription
            // 
            this.labelTargetArmorDescription.Location = new System.Drawing.Point(26, 87);
            this.labelTargetArmorDescription.Name = "labelTargetArmorDescription";
            this.labelTargetArmorDescription.Size = new System.Drawing.Size(157, 55);
            this.labelTargetArmorDescription.TabIndex = 5;
            // 
            // groupBoxCycles
            // 
            this.groupBoxCycles.Controls.Add(this.radioButton4s5r);
            this.groupBoxCycles.Location = new System.Drawing.Point(15, 151);
            this.groupBoxCycles.Name = "groupBoxCycles";
            this.groupBoxCycles.Size = new System.Drawing.Size(182, 112);
            this.groupBoxCycles.TabIndex = 6;
            this.groupBoxCycles.TabStop = false;
            this.groupBoxCycles.Text = "Cycles";
            // 
            // radioButton4s5r
            // 
            this.radioButton4s5r.AutoSize = true;
            this.radioButton4s5r.Checked = true;
            this.radioButton4s5r.Location = new System.Drawing.Point(6, 19);
            this.radioButton4s5r.Name = "radioButton4s5r";
            this.radioButton4s5r.Size = new System.Drawing.Size(45, 17);
            this.radioButton4s5r.TabIndex = 0;
            this.radioButton4s5r.TabStop = true;
            this.radioButton4s5r.Text = "4s5r";
            this.radioButton4s5r.UseVisualStyleBackColor = true;
            // 
            // comboBoxArmorBosses
            // 
            this.comboBoxArmorBosses.FormattingEnabled = true;
            this.comboBoxArmorBosses.Items.AddRange(new object[] {
            "83",
            "82",
            "81",
            "80"});
            this.comboBoxArmorBosses.Location = new System.Drawing.Point(91, 44);
            this.comboBoxArmorBosses.Name = "comboBoxArmorBosses";
            this.comboBoxArmorBosses.Size = new System.Drawing.Size(105, 21);
            this.comboBoxArmorBosses.TabIndex = 8;
            this.comboBoxArmorBosses.SelectedIndexChanged += new System.EventHandler(this.comboBoxArmorBosses_SelectedIndexChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Location = new System.Drawing.Point(15, 71);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(181, 74);
            this.groupBox3.TabIndex = 9;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Bosses";
            // 
            // CalculationOptionsPanelDPSWarr
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.comboBoxArmorBosses);
            this.Controls.Add(this.groupBoxCycles);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBoxTargetLevel);
            this.Controls.Add(this.labelTargetArmorDescription);
            this.Controls.Add(this.groupBox3);
            this.Name = "CalculationOptionsPanelDPSWarr";
            this.Size = new System.Drawing.Size(209, 432);
            this.groupBoxCycles.ResumeLayout(false);
            this.groupBoxCycles.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBoxTargetLevel;
		private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labelTargetArmorDescription;
        private System.Windows.Forms.GroupBox groupBoxCycles;
        private System.Windows.Forms.RadioButton radioButton4s5r;
        private System.Windows.Forms.ComboBox comboBoxArmorBosses;
        private System.Windows.Forms.GroupBox groupBox3;
    }
}
