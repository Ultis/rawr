namespace Rawr.Retribution
{
    partial class CalculationOptionsPanelRetribution
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkGlyphExorcism = new System.Windows.Forms.CheckBox();
            this.chkGlyphSenseUndead = new System.Windows.Forms.CheckBox();
            this.chkGlyphConsecration = new System.Windows.Forms.CheckBox();
            this.chkGlyphJudgement = new System.Windows.Forms.CheckBox();
            this.cmbMobType = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbLevel = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.listUnlimitedPriority = new System.Windows.Forms.CheckedListBox();
            this.butUnlimitedDown = new System.Windows.Forms.Button();
            this.butUnlimitedUp = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.trkTime20 = new System.Windows.Forms.TrackBar();
            this.label12 = new System.Windows.Forms.Label();
            this.cmbLength = new System.Windows.Forms.NumericUpDown();
            this.lblTime20 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.nudDelay = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trkTime20)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbLength)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDelay)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkGlyphExorcism);
            this.groupBox1.Controls.Add(this.chkGlyphSenseUndead);
            this.groupBox1.Controls.Add(this.chkGlyphConsecration);
            this.groupBox1.Controls.Add(this.chkGlyphJudgement);
            this.groupBox1.Location = new System.Drawing.Point(3, 263);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(162, 109);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Glyphs";
            // 
            // chkGlyphExorcism
            // 
            this.chkGlyphExorcism.AutoSize = true;
            this.chkGlyphExorcism.Location = new System.Drawing.Point(6, 65);
            this.chkGlyphExorcism.Name = "chkGlyphExorcism";
            this.chkGlyphExorcism.Size = new System.Drawing.Size(110, 17);
            this.chkGlyphExorcism.TabIndex = 3;
            this.chkGlyphExorcism.Text = "Glyph of Exorcism";
            this.chkGlyphExorcism.UseVisualStyleBackColor = true;
            this.chkGlyphExorcism.CheckedChanged += new System.EventHandler(this.chkGlyphExorcism_CheckedChanged);
            // 
            // chkGlyphSenseUndead
            // 
            this.chkGlyphSenseUndead.AutoSize = true;
            this.chkGlyphSenseUndead.Location = new System.Drawing.Point(6, 88);
            this.chkGlyphSenseUndead.Name = "chkGlyphSenseUndead";
            this.chkGlyphSenseUndead.Size = new System.Drawing.Size(139, 17);
            this.chkGlyphSenseUndead.TabIndex = 2;
            this.chkGlyphSenseUndead.Text = "Glyph of Sense Undead";
            this.chkGlyphSenseUndead.UseVisualStyleBackColor = true;
            this.chkGlyphSenseUndead.CheckedChanged += new System.EventHandler(this.chkGlyphSenseUndead_CheckedChanged);
            // 
            // chkGlyphConsecration
            // 
            this.chkGlyphConsecration.AutoSize = true;
            this.chkGlyphConsecration.Location = new System.Drawing.Point(6, 42);
            this.chkGlyphConsecration.Name = "chkGlyphConsecration";
            this.chkGlyphConsecration.Size = new System.Drawing.Size(130, 17);
            this.chkGlyphConsecration.TabIndex = 1;
            this.chkGlyphConsecration.Text = "Glyph of Consecration";
            this.chkGlyphConsecration.UseVisualStyleBackColor = true;
            this.chkGlyphConsecration.CheckedChanged += new System.EventHandler(this.chkGlyphConsecration_CheckedChanged);
            // 
            // chkGlyphJudgement
            // 
            this.chkGlyphJudgement.AutoSize = true;
            this.chkGlyphJudgement.Location = new System.Drawing.Point(6, 19);
            this.chkGlyphJudgement.Name = "chkGlyphJudgement";
            this.chkGlyphJudgement.Size = new System.Drawing.Size(120, 17);
            this.chkGlyphJudgement.TabIndex = 0;
            this.chkGlyphJudgement.Text = "Glyph of Judgement";
            this.chkGlyphJudgement.UseVisualStyleBackColor = true;
            this.chkGlyphJudgement.CheckedChanged += new System.EventHandler(this.chkGlyphJudgement_CheckedChanged);
            // 
            // cmbMobType
            // 
            this.cmbMobType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMobType.FormattingEnabled = true;
            this.cmbMobType.Items.AddRange(new object[] {
            "Undead",
            "Demon",
            "Humanoid, Elemental",
            "Other"});
            this.cmbMobType.Location = new System.Drawing.Point(156, 19);
            this.cmbMobType.Name = "cmbMobType";
            this.cmbMobType.Size = new System.Drawing.Size(114, 21);
            this.cmbMobType.TabIndex = 2;
            this.cmbMobType.SelectedIndexChanged += new System.EventHandler(this.cmbMobType_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(153, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Mob Type:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(153, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Target Level:";
            // 
            // cmbLevel
            // 
            this.cmbLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLevel.FormattingEnabled = true;
            this.cmbLevel.Items.AddRange(new object[] {
            "80",
            "81",
            "82",
            "83"});
            this.cmbLevel.Location = new System.Drawing.Point(157, 59);
            this.cmbLevel.Name = "cmbLevel";
            this.cmbLevel.Size = new System.Drawing.Size(46, 21);
            this.cmbLevel.TabIndex = 5;
            this.cmbLevel.SelectedIndexChanged += new System.EventHandler(this.cmbLevel_SelectedIndexChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.nudDelay);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.listUnlimitedPriority);
            this.groupBox2.Controls.Add(this.butUnlimitedDown);
            this.groupBox2.Controls.Add(this.butUnlimitedUp);
            this.groupBox2.Location = new System.Drawing.Point(3, 110);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(162, 147);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Rotation";
            // 
            // listUnlimitedPriority
            // 
            this.listUnlimitedPriority.FormattingEnabled = true;
            this.listUnlimitedPriority.Items.AddRange(new object[] {
            "Judgement",
            "Hammer of Wrath",
            "Crusader Strike",
            "Divine Storm",
            "Consecration",
            "Exorcism"});
            this.listUnlimitedPriority.Location = new System.Drawing.Point(6, 19);
            this.listUnlimitedPriority.Name = "listUnlimitedPriority";
            this.listUnlimitedPriority.Size = new System.Drawing.Size(120, 94);
            this.listUnlimitedPriority.TabIndex = 32;
            this.listUnlimitedPriority.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.listUnlimitedPriority_ItemCheck);
            // 
            // butUnlimitedDown
            // 
            this.butUnlimitedDown.Location = new System.Drawing.Point(132, 48);
            this.butUnlimitedDown.Name = "butUnlimitedDown";
            this.butUnlimitedDown.Size = new System.Drawing.Size(23, 23);
            this.butUnlimitedDown.TabIndex = 5;
            this.butUnlimitedDown.Text = "-";
            this.butUnlimitedDown.UseVisualStyleBackColor = true;
            this.butUnlimitedDown.Click += new System.EventHandler(this.butUnlimitedDown_Click);
            // 
            // butUnlimitedUp
            // 
            this.butUnlimitedUp.Location = new System.Drawing.Point(132, 19);
            this.butUnlimitedUp.Name = "butUnlimitedUp";
            this.butUnlimitedUp.Size = new System.Drawing.Size(23, 23);
            this.butUnlimitedUp.TabIndex = 4;
            this.butUnlimitedUp.Text = "+";
            this.butUnlimitedUp.UseVisualStyleBackColor = true;
            this.butUnlimitedUp.Click += new System.EventHandler(this.butUnlimitedUp_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(3, 43);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(88, 13);
            this.label11.TabIndex = 29;
            this.label11.Text = "Below 20% Time:";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // trkTime20
            // 
            this.trkTime20.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.trkTime20.Location = new System.Drawing.Point(3, 59);
            this.trkTime20.Maximum = 100;
            this.trkTime20.Name = "trkTime20";
            this.trkTime20.Size = new System.Drawing.Size(137, 45);
            this.trkTime20.TabIndex = 28;
            this.trkTime20.TickFrequency = 10;
            this.trkTime20.Value = 90;
            this.trkTime20.Scroll += new System.EventHandler(this.trkTime20_Scroll);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(3, 3);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(69, 13);
            this.label12.TabIndex = 27;
            this.label12.Text = "Fight Length:";
            // 
            // cmbLength
            // 
            this.cmbLength.DecimalPlaces = 1;
            this.cmbLength.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.cmbLength.Location = new System.Drawing.Point(6, 19);
            this.cmbLength.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.cmbLength.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.cmbLength.Name = "cmbLength";
            this.cmbLength.Size = new System.Drawing.Size(112, 20);
            this.cmbLength.TabIndex = 26;
            this.cmbLength.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.cmbLength.ValueChanged += new System.EventHandler(this.cmbLength_ValueChanged);
            // 
            // lblTime20
            // 
            this.lblTime20.AutoSize = true;
            this.lblTime20.Location = new System.Drawing.Point(107, 91);
            this.lblTime20.Name = "lblTime20";
            this.lblTime20.Size = new System.Drawing.Size(33, 13);
            this.lblTime20.TabIndex = 30;
            this.lblTime20.Text = "100%";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 121);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(37, 13);
            this.label3.TabIndex = 33;
            this.label3.Text = "Delay:";
            // 
            // nudDelay
            // 
            this.nudDelay.DecimalPlaces = 2;
            this.nudDelay.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.nudDelay.Location = new System.Drawing.Point(49, 119);
            this.nudDelay.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudDelay.Name = "nudDelay";
            this.nudDelay.Size = new System.Drawing.Size(48, 20);
            this.nudDelay.TabIndex = 34;
            this.nudDelay.ValueChanged += new System.EventHandler(this.nudDelay_ValueChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(103, 121);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(24, 13);
            this.label4.TabIndex = 35;
            this.label4.Text = "sec";
            // 
            // CalculationOptionsPanelRetribution
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.lblTime20);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.trkTime20);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.cmbLength);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.cmbLevel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbMobType);
            this.Name = "CalculationOptionsPanelRetribution";
            this.Size = new System.Drawing.Size(300, 594);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trkTime20)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbLength)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDelay)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chkGlyphJudgement;
        private System.Windows.Forms.ComboBox cmbMobType;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbLevel;
        private System.Windows.Forms.CheckBox chkGlyphConsecration;
        private System.Windows.Forms.CheckBox chkGlyphSenseUndead;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TrackBar trkTime20;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.NumericUpDown cmbLength;
        private System.Windows.Forms.Label lblTime20;
        private System.Windows.Forms.Button butUnlimitedDown;
        private System.Windows.Forms.Button butUnlimitedUp;
        private System.Windows.Forms.CheckedListBox listUnlimitedPriority;
        private System.Windows.Forms.CheckBox chkGlyphExorcism;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown nudDelay;
        private System.Windows.Forms.Label label3;

    }
}