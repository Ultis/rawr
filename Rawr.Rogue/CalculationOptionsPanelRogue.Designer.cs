namespace Rawr.Rogue {
    partial class CalculationOptionsPanelRogue {
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.comboBoxOHPoison = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.comboBoxMHPoison = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBoxArmorBosses = new System.Windows.Forms.ComboBox();
            this.groupBoxCycles.SuspendLayout();
            this.groupBox1.SuspendLayout();
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
            this.labelTargetArmorDescription.Location = new System.Drawing.Point(15, 89);
            this.labelTargetArmorDescription.Name = "labelTargetArmorDescription";
            this.labelTargetArmorDescription.Size = new System.Drawing.Size(183, 57);
            this.labelTargetArmorDescription.TabIndex = 5;
            // 
            // groupBoxCycles
            // 
            this.groupBoxCycles.Controls.Add(this.radioButton4s5r);
            this.groupBoxCycles.Location = new System.Drawing.Point(15, 146);
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
            this.radioButton4s5r.CheckedChanged += new System.EventHandler(this.OnCheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.comboBoxOHPoison);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.comboBoxMHPoison);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Location = new System.Drawing.Point(15, 264);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(181, 77);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Poisons";
            // 
            // comboBoxOHPoison
            // 
            this.comboBoxOHPoison.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxOHPoison.FormattingEnabled = true;
            this.comboBoxOHPoison.Items.AddRange(new object[] {
            "Deadly Poison",
            "Instant Poison"});
            this.comboBoxOHPoison.Location = new System.Drawing.Point(72, 46);
            this.comboBoxOHPoison.Name = "comboBoxOHPoison";
            this.comboBoxOHPoison.Size = new System.Drawing.Size(96, 21);
            this.comboBoxOHPoison.TabIndex = 3;
            this.comboBoxOHPoison.SelectedIndexChanged += new System.EventHandler(this.OnOHPoisonChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 51);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Off Hand:";
            // 
            // comboBoxMHPoison
            // 
            this.comboBoxMHPoison.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxMHPoison.FormattingEnabled = true;
            this.comboBoxMHPoison.Location = new System.Drawing.Point(72, 19);
            this.comboBoxMHPoison.Name = "comboBoxMHPoison";
            this.comboBoxMHPoison.Size = new System.Drawing.Size(96, 21);
            this.comboBoxMHPoison.TabIndex = 1;
            this.comboBoxMHPoison.SelectedIndexChanged += new System.EventHandler(this.OnMHPoisonChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 24);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(62, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Main Hand:";
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
            // CalculationOptionsPanelRogue
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.comboBoxArmorBosses);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBoxCycles);
            this.Controls.Add(this.labelTargetArmorDescription);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBoxTargetLevel);
            this.Name = "CalculationOptionsPanelRogue";
            this.Size = new System.Drawing.Size(209, 432);
            this.groupBoxCycles.ResumeLayout(false);
            this.groupBoxCycles.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBoxTargetLevel;
		private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labelTargetArmorDescription;
        private System.Windows.Forms.GroupBox groupBoxCycles;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RadioButton radioButton4s5r;
        private System.Windows.Forms.ComboBox comboBoxOHPoison;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboBoxMHPoison;
        private System.Windows.Forms.ComboBox comboBoxArmorBosses;
    }
}
