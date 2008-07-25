namespace Rawr.ShadowPriest
{
    partial class CalculationOptionsPanelShadowPriest
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
            this.cmbLength = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbSpriest = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbManaTime = new System.Windows.Forms.NumericUpDown();
            this.cmbManaAmt = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.gbSpellPriority = new System.Windows.Forms.GroupBox();
            this.bChangePriority = new System.Windows.Forms.Button();
            this.lsSpellPriopity = new System.Windows.Forms.ListBox();
            this.label8 = new System.Windows.Forms.Label();
            this.cmbLag = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.cmbWaitTime = new System.Windows.Forms.NumericUpDown();
            this.label11 = new System.Windows.Forms.Label();
            this.cbDrums = new System.Windows.Forms.CheckBox();
            this.label12 = new System.Windows.Forms.Label();
            this.cmbDrums = new System.Windows.Forms.NumericUpDown();
            this.label13 = new System.Windows.Forms.Label();
            this.cmbISB = new System.Windows.Forms.NumericUpDown();
            this.label14 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.cmbLength)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbSpriest)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbManaTime)).BeginInit();
            this.gbSpellPriority.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbLag)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbWaitTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbDrums)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbISB)).BeginInit();
            this.SuspendLayout();
            // 
            // cmbLength
            // 
            this.cmbLength.DecimalPlaces = 1;
            this.cmbLength.Increment = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.cmbLength.Location = new System.Drawing.Point(97, 7);
            this.cmbLength.Maximum = new decimal(new int[] {
            3600,
            0,
            0,
            0});
            this.cmbLength.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.cmbLength.Name = "cmbLength";
            this.cmbLength.Size = new System.Drawing.Size(82, 20);
            this.cmbLength.TabIndex = 20;
            this.cmbLength.Value = new decimal(new int[] {
            600,
            0,
            0,
            0});
            this.cmbLength.ValueChanged += new System.EventHandler(this.cmbLength_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 14);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 13);
            this.label3.TabIndex = 21;
            this.label3.Text = "Fight Length:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.cmbSpriest);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.cmbManaTime);
            this.groupBox2.Controls.Add(this.cmbManaAmt);
            this.groupBox2.Location = new System.Drawing.Point(4, 182);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(212, 111);
            this.groupBox2.TabIndex = 22;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Mana Buffs";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(160, 40);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(45, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "% Mana";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(73, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(33, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "when";
            // 
            // cmbSpriest
            // 
            this.cmbSpriest.Increment = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.cmbSpriest.Location = new System.Drawing.Point(5, 77);
            this.cmbSpriest.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.cmbSpriest.Name = "cmbSpriest";
            this.cmbSpriest.Size = new System.Drawing.Size(76, 20);
            this.cmbSpriest.TabIndex = 6;
            this.cmbSpriest.ValueChanged += new System.EventHandler(this.cmbSpriest_ValueChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(87, 79);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(27, 13);
            this.label7.TabIndex = 5;
            this.label7.Text = "mp5";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 61);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(78, 13);
            this.label5.TabIndex = 3;
            this.label5.Text = "Shadow Priest:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 21);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(75, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Mana Potions:";
            // 
            // cmbManaTime
            // 
            this.cmbManaTime.DecimalPlaces = 1;
            this.cmbManaTime.Increment = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.cmbManaTime.Location = new System.Drawing.Point(108, 38);
            this.cmbManaTime.Maximum = new decimal(new int[] {
            90,
            0,
            0,
            0});
            this.cmbManaTime.Name = "cmbManaTime";
            this.cmbManaTime.Size = new System.Drawing.Size(50, 20);
            this.cmbManaTime.TabIndex = 1;
            this.cmbManaTime.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.cmbManaTime.ValueChanged += new System.EventHandler(this.cmbManaTime_ValueChanged);
            // 
            // cmbManaAmt
            // 
            this.cmbManaAmt.DisplayMember = "2400";
            this.cmbManaAmt.Items.AddRange(new object[] {
            "0",
            "1800",
            "2200",
            "2400"});
            this.cmbManaAmt.Location = new System.Drawing.Point(6, 37);
            this.cmbManaAmt.Name = "cmbManaAmt";
            this.cmbManaAmt.Size = new System.Drawing.Size(61, 21);
            this.cmbManaAmt.TabIndex = 0;
            this.cmbManaAmt.Text = "2400";
            this.cmbManaAmt.ValueMember = "2400";
            this.cmbManaAmt.SelectedIndexChanged += new System.EventHandler(this.cmbManaAmt_SelectedIndexChanged);
            this.cmbManaAmt.TextUpdate += new System.EventHandler(this.cmbManaAmt_TextUpdate);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(185, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(27, 13);
            this.label1.TabIndex = 26;
            this.label1.Text = "sec.";
            // 
            // gbSpellPriority
            // 
            this.gbSpellPriority.Controls.Add(this.bChangePriority);
            this.gbSpellPriority.Controls.Add(this.lsSpellPriopity);
            this.gbSpellPriority.Location = new System.Drawing.Point(4, 299);
            this.gbSpellPriority.Name = "gbSpellPriority";
            this.gbSpellPriority.Size = new System.Drawing.Size(212, 186);
            this.gbSpellPriority.TabIndex = 29;
            this.gbSpellPriority.TabStop = false;
            this.gbSpellPriority.Text = "Spell Priority";
            // 
            // bChangePriority
            // 
            this.bChangePriority.Location = new System.Drawing.Point(5, 151);
            this.bChangePriority.Name = "bChangePriority";
            this.bChangePriority.Size = new System.Drawing.Size(75, 23);
            this.bChangePriority.TabIndex = 11;
            this.bChangePriority.Text = "Change";
            this.bChangePriority.UseVisualStyleBackColor = true;
            this.bChangePriority.Click += new System.EventHandler(this.bChangePriority_Click);
            // 
            // lsSpellPriopity
            // 
            this.lsSpellPriopity.FormattingEnabled = true;
            this.lsSpellPriopity.Location = new System.Drawing.Point(5, 19);
            this.lsSpellPriopity.Name = "lsSpellPriopity";
            this.lsSpellPriopity.Size = new System.Drawing.Size(200, 121);
            this.lsSpellPriopity.TabIndex = 10;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(10, 40);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(28, 13);
            this.label8.TabIndex = 30;
            this.label8.Text = "Lag:";
            // 
            // cmbLag
            // 
            this.cmbLag.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.cmbLag.Location = new System.Drawing.Point(97, 33);
            this.cmbLag.Maximum = new decimal(new int[] {
            3600,
            0,
            0,
            0});
            this.cmbLag.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.cmbLag.Name = "cmbLag";
            this.cmbLag.Size = new System.Drawing.Size(82, 20);
            this.cmbLag.TabIndex = 31;
            this.cmbLag.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.cmbLag.ValueChanged += new System.EventHandler(this.cmbLag_ValueChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(185, 40);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(35, 13);
            this.label9.TabIndex = 32;
            this.label9.Text = "msec.";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(10, 66);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(58, 13);
            this.label10.TabIndex = 33;
            this.label10.Text = "Wait Time:";
            // 
            // cmbWaitTime
            // 
            this.cmbWaitTime.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.cmbWaitTime.Location = new System.Drawing.Point(97, 59);
            this.cmbWaitTime.Maximum = new decimal(new int[] {
            3600,
            0,
            0,
            0});
            this.cmbWaitTime.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.cmbWaitTime.Name = "cmbWaitTime";
            this.cmbWaitTime.Size = new System.Drawing.Size(82, 20);
            this.cmbWaitTime.TabIndex = 34;
            this.cmbWaitTime.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.cmbWaitTime.ValueChanged += new System.EventHandler(this.cmbWaitTime_ValueChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(185, 66);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(35, 13);
            this.label11.TabIndex = 35;
            this.label11.Text = "msec.";
            // 
            // cbDrums
            // 
            this.cbDrums.Enabled = false;
            this.cbDrums.Location = new System.Drawing.Point(9, 142);
            this.cbDrums.Name = "cbDrums";
            this.cbDrums.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.cbDrums.Size = new System.Drawing.Size(170, 24);
            this.cbDrums.TabIndex = 36;
            this.cbDrums.Text = "Use Your Drum";
            this.cbDrums.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbDrums.UseVisualStyleBackColor = true;
            this.cbDrums.CheckedChanged += new System.EventHandler(this.cbDrums_CheckedChanged);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(10, 123);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(67, 13);
            this.label12.TabIndex = 37;
            this.label12.Text = "Party Drums:";
            // 
            // cmbDrums
            // 
            this.cmbDrums.Location = new System.Drawing.Point(97, 116);
            this.cmbDrums.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.cmbDrums.Name = "cmbDrums";
            this.cmbDrums.Size = new System.Drawing.Size(82, 20);
            this.cmbDrums.TabIndex = 38;
            this.cmbDrums.ValueChanged += new System.EventHandler(this.cmbDrums_ValueChanged);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(10, 92);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(63, 13);
            this.label13.TabIndex = 39;
            this.label13.Text = "ISB Uptime:";
            // 
            // cmbISB
            // 
            this.cmbISB.Increment = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.cmbISB.Location = new System.Drawing.Point(97, 85);
            this.cmbISB.Name = "cmbISB";
            this.cmbISB.Size = new System.Drawing.Size(82, 20);
            this.cmbISB.TabIndex = 40;
            this.cmbISB.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.cmbISB.ValueChanged += new System.EventHandler(this.cmbISB_ValueChanged);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(185, 92);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(15, 13);
            this.label14.TabIndex = 41;
            this.label14.Text = "%";
            // 
            // CalculationOptionsPanelShadowPriest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label14);
            this.Controls.Add(this.cmbISB);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.cmbDrums);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.cbDrums);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.cmbWaitTime);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.cmbLag);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.gbSpellPriority);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cmbLength);
            this.Name = "CalculationOptionsPanelShadowPriest";
            this.Size = new System.Drawing.Size(226, 604);
            ((System.ComponentModel.ISupportInitialize)(this.cmbLength)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbSpriest)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbManaTime)).EndInit();
            this.gbSpellPriority.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cmbLag)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbWaitTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbDrums)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbISB)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown cmbLength;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.NumericUpDown cmbManaTime;
        private System.Windows.Forms.ComboBox cmbManaAmt;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown cmbSpriest;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox gbSpellPriority;
        private System.Windows.Forms.ListBox lsSpellPriopity;
        private System.Windows.Forms.Button bChangePriority;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown cmbLag;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.NumericUpDown cmbWaitTime;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.CheckBox cbDrums;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.NumericUpDown cmbDrums;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.NumericUpDown cmbISB;
        private System.Windows.Forms.Label label14;
    }
}
