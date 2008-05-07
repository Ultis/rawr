namespace Rawr.Tankadin
{
    partial class CalculationOptionsPanelTankadin
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
            this.cmbTargetLevel = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.nubAtkSpeed = new System.Windows.Forms.NumericUpDown();
            this.nubAttackers = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.labelBossAttackValue = new System.Windows.Forms.Label();
            this.labelBossAttack = new Rawr.CustomControls.ExtendedToolTipLabel();
            this.trackBarBossAttackValue = new System.Windows.Forms.TrackBar();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.labelTargetArmor = new System.Windows.Forms.Label();
            this.labelThreatScale = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.radioMulti = new System.Windows.Forms.RadioButton();
            this.trackBarThreatScale = new System.Windows.Forms.TrackBar();
            this.radioSingle = new System.Windows.Forms.RadioButton();
            this.trackBarTargetArmor = new System.Windows.Forms.TrackBar();
            this.label8 = new System.Windows.Forms.Label();
            this.trackBarMitigationScale = new System.Windows.Forms.TrackBar();
            this.label7 = new System.Windows.Forms.Label();
            this.labelMitigationScale = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.nubAtkSpeed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nubAttackers)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarBossAttackValue)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarThreatScale)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarTargetArmor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarMitigationScale)).BeginInit();
            this.SuspendLayout();
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
            this.cmbTargetLevel.Location = new System.Drawing.Point(84, 3);
            this.cmbTargetLevel.Name = "cmbTargetLevel";
            this.cmbTargetLevel.Size = new System.Drawing.Size(121, 21);
            this.cmbTargetLevel.TabIndex = 3;
            this.cmbTargetLevel.SelectedIndexChanged += new System.EventHandler(this.cmbTargetLevel_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Target Level:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Attack Speed:";
            // 
            // nubAtkSpeed
            // 
            this.nubAtkSpeed.DecimalPlaces = 2;
            this.nubAtkSpeed.Increment = new decimal(new int[] {
            25,
            0,
            0,
            131072});
            this.nubAtkSpeed.Location = new System.Drawing.Point(89, 19);
            this.nubAtkSpeed.Maximum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.nubAtkSpeed.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nubAtkSpeed.Name = "nubAtkSpeed";
            this.nubAtkSpeed.Size = new System.Drawing.Size(59, 20);
            this.nubAtkSpeed.TabIndex = 5;
            this.nubAtkSpeed.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.nubAtkSpeed.ValueChanged += new System.EventHandler(this.nubAtkSpeed_ValueChanged);
            // 
            // nubAttackers
            // 
            this.nubAttackers.Location = new System.Drawing.Point(89, 45);
            this.nubAttackers.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.nubAttackers.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nubAttackers.Name = "nubAttackers";
            this.nubAttackers.Size = new System.Drawing.Size(59, 20);
            this.nubAttackers.TabIndex = 8;
            this.nubAttackers.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nubAttackers.ValueChanged += new System.EventHandler(this.nubAttackers_ValueChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 47);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "# of Attackers:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.labelBossAttackValue);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.nubAtkSpeed);
            this.groupBox1.Controls.Add(this.labelBossAttack);
            this.groupBox1.Controls.Add(this.nubAttackers);
            this.groupBox1.Controls.Add(this.trackBarBossAttackValue);
            this.groupBox1.Location = new System.Drawing.Point(3, 30);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(202, 122);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Incoming Damage";
            // 
            // labelBossAttackValue
            // 
            this.labelBossAttackValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.labelBossAttackValue.AutoSize = true;
            this.labelBossAttackValue.Location = new System.Drawing.Point(70, 103);
            this.labelBossAttackValue.Name = "labelBossAttackValue";
            this.labelBossAttackValue.Size = new System.Drawing.Size(37, 13);
            this.labelBossAttackValue.TabIndex = 20;
            this.labelBossAttackValue.Text = "20000";
            // 
            // labelBossAttack
            // 
            this.labelBossAttack.Location = new System.Drawing.Point(-5, 71);
            this.labelBossAttack.Name = "labelBossAttack";
            this.labelBossAttack.Size = new System.Drawing.Size(64, 45);
            this.labelBossAttack.TabIndex = 15;
            this.labelBossAttack.Text = "Boss Attack*:";
            this.labelBossAttack.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.labelBossAttack.ToolTipText = "Boss attack value before armor. Used to determine the value of Block Value.";
            // 
            // trackBarBossAttackValue
            // 
            this.trackBarBossAttackValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.trackBarBossAttackValue.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.trackBarBossAttackValue.LargeChange = 5000;
            this.trackBarBossAttackValue.Location = new System.Drawing.Point(65, 71);
            this.trackBarBossAttackValue.Maximum = 50000;
            this.trackBarBossAttackValue.Minimum = 500;
            this.trackBarBossAttackValue.Name = "trackBarBossAttackValue";
            this.trackBarBossAttackValue.Size = new System.Drawing.Size(131, 45);
            this.trackBarBossAttackValue.SmallChange = 500;
            this.trackBarBossAttackValue.TabIndex = 18;
            this.trackBarBossAttackValue.TickFrequency = 2500;
            this.trackBarBossAttackValue.Value = 20000;
            this.trackBarBossAttackValue.Scroll += new System.EventHandler(this.trackBarBossAttackValue_Scroll);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.labelTargetArmor);
            this.groupBox2.Controls.Add(this.labelThreatScale);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.radioMulti);
            this.groupBox2.Controls.Add(this.trackBarThreatScale);
            this.groupBox2.Controls.Add(this.radioSingle);
            this.groupBox2.Controls.Add(this.trackBarTargetArmor);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Location = new System.Drawing.Point(3, 158);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(202, 147);
            this.groupBox2.TabIndex = 11;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Threat";
            // 
            // labelTargetArmor
            // 
            this.labelTargetArmor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.labelTargetArmor.AutoSize = true;
            this.labelTargetArmor.Location = new System.Drawing.Point(70, 126);
            this.labelTargetArmor.Name = "labelTargetArmor";
            this.labelTargetArmor.Size = new System.Drawing.Size(31, 13);
            this.labelTargetArmor.TabIndex = 22;
            this.labelTargetArmor.Text = "6600";
            // 
            // labelThreatScale
            // 
            this.labelThreatScale.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.labelThreatScale.AutoSize = true;
            this.labelThreatScale.Location = new System.Drawing.Point(70, 75);
            this.labelThreatScale.Name = "labelThreatScale";
            this.labelThreatScale.Size = new System.Drawing.Size(13, 13);
            this.labelThreatScale.TabIndex = 20;
            this.labelThreatScale.Text = "1";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(9, 43);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(50, 45);
            this.label5.TabIndex = 2;
            this.label5.Text = "Threat Scale:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // radioMulti
            // 
            this.radioMulti.AutoSize = true;
            this.radioMulti.Location = new System.Drawing.Point(91, 20);
            this.radioMulti.Name = "radioMulti";
            this.radioMulti.Size = new System.Drawing.Size(90, 17);
            this.radioMulti.TabIndex = 1;
            this.radioMulti.TabStop = true;
            this.radioMulti.Text = "Multiple Mobs";
            this.radioMulti.UseVisualStyleBackColor = true;
            // 
            // trackBarThreatScale
            // 
            this.trackBarThreatScale.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.trackBarThreatScale.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.trackBarThreatScale.LargeChange = 50;
            this.trackBarThreatScale.Location = new System.Drawing.Point(65, 43);
            this.trackBarThreatScale.Maximum = 1000;
            this.trackBarThreatScale.Minimum = 1;
            this.trackBarThreatScale.Name = "trackBarThreatScale";
            this.trackBarThreatScale.Size = new System.Drawing.Size(131, 45);
            this.trackBarThreatScale.TabIndex = 20;
            this.trackBarThreatScale.TickFrequency = 50;
            this.trackBarThreatScale.Value = 1;
            this.trackBarThreatScale.Scroll += new System.EventHandler(this.trackBarThreatScale_Scroll);
            // 
            // radioSingle
            // 
            this.radioSingle.AutoSize = true;
            this.radioSingle.Checked = true;
            this.radioSingle.Location = new System.Drawing.Point(7, 20);
            this.radioSingle.Name = "radioSingle";
            this.radioSingle.Size = new System.Drawing.Size(78, 17);
            this.radioSingle.TabIndex = 0;
            this.radioSingle.TabStop = true;
            this.radioSingle.Text = "Single Mob";
            this.radioSingle.UseVisualStyleBackColor = true;
            // 
            // trackBarTargetArmor
            // 
            this.trackBarTargetArmor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.trackBarTargetArmor.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.trackBarTargetArmor.LargeChange = 1000;
            this.trackBarTargetArmor.Location = new System.Drawing.Point(65, 94);
            this.trackBarTargetArmor.Maximum = 9000;
            this.trackBarTargetArmor.Minimum = 3000;
            this.trackBarTargetArmor.Name = "trackBarTargetArmor";
            this.trackBarTargetArmor.Size = new System.Drawing.Size(131, 45);
            this.trackBarTargetArmor.SmallChange = 100;
            this.trackBarTargetArmor.TabIndex = 19;
            this.trackBarTargetArmor.TickFrequency = 300;
            this.trackBarTargetArmor.Value = 6600;
            this.trackBarTargetArmor.Scroll += new System.EventHandler(this.trackBarTargetArmor_Scroll);
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(6, 94);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(53, 42);
            this.label8.TabIndex = 14;
            this.label8.Text = "Target Armor: ";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // trackBarMitigationScale
            // 
            this.trackBarMitigationScale.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.trackBarMitigationScale.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.trackBarMitigationScale.LargeChange = 1000;
            this.trackBarMitigationScale.Location = new System.Drawing.Point(68, 311);
            this.trackBarMitigationScale.Maximum = 15000;
            this.trackBarMitigationScale.Minimum = 1000;
            this.trackBarMitigationScale.Name = "trackBarMitigationScale";
            this.trackBarMitigationScale.Size = new System.Drawing.Size(131, 45);
            this.trackBarMitigationScale.SmallChange = 50;
            this.trackBarMitigationScale.TabIndex = 17;
            this.trackBarMitigationScale.TickFrequency = 500;
            this.trackBarMitigationScale.Value = 4000;
            this.trackBarMitigationScale.Scroll += new System.EventHandler(this.trackBarMitigationScale_Scroll);
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(1, 311);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(61, 45);
            this.label7.TabIndex = 13;
            this.label7.Text = "Mitigation Scale:";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelMitigationScale
            // 
            this.labelMitigationScale.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.labelMitigationScale.AutoSize = true;
            this.labelMitigationScale.Location = new System.Drawing.Point(73, 343);
            this.labelMitigationScale.Name = "labelMitigationScale";
            this.labelMitigationScale.Size = new System.Drawing.Size(31, 13);
            this.labelMitigationScale.TabIndex = 21;
            this.labelMitigationScale.Text = "4000";
            // 
            // CalculationOptionsPanelTankadin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelMitigationScale);
            this.Controls.Add(this.trackBarMitigationScale);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.cmbTargetLevel);
            this.Controls.Add(this.label1);
            this.Name = "CalculationOptionsPanelTankadin";
            this.Size = new System.Drawing.Size(210, 375);
            ((System.ComponentModel.ISupportInitialize)(this.nubAtkSpeed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nubAttackers)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarBossAttackValue)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarThreatScale)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarTargetArmor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarMitigationScale)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbTargetLevel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown nubAtkSpeed;
        private System.Windows.Forms.NumericUpDown nubAttackers;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton radioMulti;
        private System.Windows.Forms.RadioButton radioSingle;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TrackBar trackBarThreatScale;
        private System.Windows.Forms.TrackBar trackBarMitigationScale;
        private System.Windows.Forms.TrackBar trackBarBossAttackValue;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TrackBar trackBarTargetArmor;
        private Rawr.CustomControls.ExtendedToolTipLabel labelBossAttack;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label labelThreatScale;
        private System.Windows.Forms.Label labelTargetArmor;
        private System.Windows.Forms.Label labelMitigationScale;
        private System.Windows.Forms.Label labelBossAttackValue;
    }
}
