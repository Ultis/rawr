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
            this.buttonTalents = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.trackBarTargetArmor = new System.Windows.Forms.TrackBar();
            this.labelTargetArmorDescription = new System.Windows.Forms.Label();
            this.groupBoxCycles = new System.Windows.Forms.GroupBox();
            this.radioButton4s5r = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarTargetArmor)).BeginInit();
            this.groupBoxCycles.SuspendLayout();
            this.SuspendLayout();
            // 
            // comboBoxTargetLevel
            // 
            this.comboBoxTargetLevel.FormattingEnabled = true;
            this.comboBoxTargetLevel.Items.AddRange(new object[] {
            "70",
            "71",
            "72",
            "73"});
            this.comboBoxTargetLevel.Location = new System.Drawing.Point(92, 14);
            this.comboBoxTargetLevel.Name = "comboBoxTargetLevel";
            this.comboBoxTargetLevel.Size = new System.Drawing.Size(105, 21);
            this.comboBoxTargetLevel.TabIndex = 0;
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
            // buttonTalents
            // 
            this.buttonTalents.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.buttonTalents.Location = new System.Drawing.Point(0, 304);
            this.buttonTalents.Name = "buttonTalents";
            this.buttonTalents.Size = new System.Drawing.Size(209, 26);
            this.buttonTalents.TabIndex = 2;
            this.buttonTalents.Text = "Talents";
            this.buttonTalents.UseVisualStyleBackColor = true;
            this.buttonTalents.Click += new System.EventHandler(this.buttonTalents_Click);
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
            // trackBarTargetArmor
            // 
            this.trackBarTargetArmor.Location = new System.Drawing.Point(92, 47);
            this.trackBarTargetArmor.Maximum = 9000;
            this.trackBarTargetArmor.Minimum = 3000;
            this.trackBarTargetArmor.Name = "trackBarTargetArmor";
            this.trackBarTargetArmor.Size = new System.Drawing.Size(105, 45);
            this.trackBarTargetArmor.TabIndex = 4;
            this.trackBarTargetArmor.TickFrequency = 300;
            this.trackBarTargetArmor.Value = 7700;
            this.trackBarTargetArmor.ValueChanged += new System.EventHandler(this.trackBarTargetArmor_ValueChanged);
            this.trackBarTargetArmor.Scroll += new System.EventHandler(this.trackBarTargetArmor_Scroll);
            // 
            // labelTargetArmorDescription
            // 
            this.labelTargetArmorDescription.Location = new System.Drawing.Point(15, 89);
            this.labelTargetArmorDescription.Name = "labelTargetArmorDescription";
            this.labelTargetArmorDescription.Size = new System.Drawing.Size(183, 57);
            this.labelTargetArmorDescription.TabIndex = 5;
            this.labelTargetArmorDescription.Text = "7700 Armor: Hydross, Lurker, Leotheras, Tidewalker, Al\'ar, Naj\'entus, Supremus, A" +
                "kama, Gurtogg";
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
            // 
            // CalculationOptionsPanelRogue
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxCycles);
            this.Controls.Add(this.labelTargetArmorDescription);
            this.Controls.Add(this.trackBarTargetArmor);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.buttonTalents);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBoxTargetLevel);
            this.Name = "CalculationOptionsPanelRogue";
            this.Size = new System.Drawing.Size(209, 330);
            ((System.ComponentModel.ISupportInitialize)(this.trackBarTargetArmor)).EndInit();
            this.groupBoxCycles.ResumeLayout(false);
            this.groupBoxCycles.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBoxTargetLevel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonTalents;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TrackBar trackBarTargetArmor;
        private System.Windows.Forms.Label labelTargetArmorDescription;
        private System.Windows.Forms.GroupBox groupBoxCycles;
        private System.Windows.Forms.RadioButton radioButton4s5r;
    }
}
