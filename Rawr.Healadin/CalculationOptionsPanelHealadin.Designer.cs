namespace Rawr.Healadin
{
    partial class CalculationOptionsPanelHealadin
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
            this.label2 = new System.Windows.Forms.Label();
            this.lblReplenishment = new System.Windows.Forms.Label();
            this.trkReplenishment = new System.Windows.Forms.TrackBar();
            this.nudDivinePlea = new System.Windows.Forms.NumericUpDown();
            this.lblDivinePlea = new System.Windows.Forms.Label();
            this.nudSpiritual = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbManaAmt = new System.Windows.Forms.ComboBox();
            this.trkActivity = new System.Windows.Forms.TrackBar();
            this.lblActivity = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.chkJotP = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.lblBoLEff = new System.Windows.Forms.Label();
            this.trkBoLEff = new System.Windows.Forms.TrackBar();
            this.label5 = new System.Windows.Forms.Label();
            this.lblBoLUp = new System.Windows.Forms.Label();
            this.trkBoLUp = new System.Windows.Forms.TrackBar();
            this.trkRatio = new System.Windows.Forms.TrackBar();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.lblRatio2 = new System.Windows.Forms.Label();
            this.lblRatio1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.cmbLength)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trkReplenishment)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDivinePlea)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSpiritual)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkActivity)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trkBoLEff)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkBoLUp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkRatio)).BeginInit();
            this.SuspendLayout();
            // 
            // cmbLength
            // 
            this.cmbLength.DecimalPlaces = 1;
            this.cmbLength.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.cmbLength.Location = new System.Drawing.Point(149, 7);
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
            this.cmbLength.TabIndex = 20;
            this.cmbLength.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.cmbLength.ValueChanged += new System.EventHandler(this.cmbLength_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(74, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 13);
            this.label3.TabIndex = 21;
            this.label3.Text = "Fight Length:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.lblReplenishment);
            this.groupBox2.Controls.Add(this.trkReplenishment);
            this.groupBox2.Controls.Add(this.nudDivinePlea);
            this.groupBox2.Controls.Add(this.lblDivinePlea);
            this.groupBox2.Controls.Add(this.nudSpiritual);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.cmbManaAmt);
            this.groupBox2.Location = new System.Drawing.Point(3, 136);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(297, 148);
            this.groupBox2.TabIndex = 22;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Mana Buffs";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(40, 97);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 28);
            this.label2.TabIndex = 27;
            this.label2.Text = "Replenishment:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblReplenishment
            // 
            this.lblReplenishment.AutoSize = true;
            this.lblReplenishment.Location = new System.Drawing.Point(147, 129);
            this.lblReplenishment.Name = "lblReplenishment";
            this.lblReplenishment.Size = new System.Drawing.Size(27, 13);
            this.lblReplenishment.TabIndex = 26;
            this.lblReplenishment.Text = "90%";
            // 
            // trkReplenishment
            // 
            this.trkReplenishment.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.trkReplenishment.Location = new System.Drawing.Point(146, 97);
            this.trkReplenishment.Maximum = 100;
            this.trkReplenishment.Minimum = 10;
            this.trkReplenishment.Name = "trkReplenishment";
            this.trkReplenishment.Size = new System.Drawing.Size(145, 45);
            this.trkReplenishment.TabIndex = 25;
            this.trkReplenishment.TickFrequency = 10;
            this.trkReplenishment.Value = 90;
            this.trkReplenishment.Scroll += new System.EventHandler(this.trkReplenishment_Scroll);
            // 
            // nudDivinePlea
            // 
            this.nudDivinePlea.DecimalPlaces = 1;
            this.nudDivinePlea.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nudDivinePlea.Location = new System.Drawing.Point(146, 73);
            this.nudDivinePlea.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.nudDivinePlea.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudDivinePlea.Name = "nudDivinePlea";
            this.nudDivinePlea.Size = new System.Drawing.Size(145, 20);
            this.nudDivinePlea.TabIndex = 10;
            this.nudDivinePlea.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudDivinePlea.ValueChanged += new System.EventHandler(this.nudDivinePlea_ValueChanged);
            // 
            // lblDivinePlea
            // 
            this.lblDivinePlea.AutoSize = true;
            this.lblDivinePlea.Location = new System.Drawing.Point(76, 75);
            this.lblDivinePlea.Name = "lblDivinePlea";
            this.lblDivinePlea.Size = new System.Drawing.Size(64, 13);
            this.lblDivinePlea.TabIndex = 9;
            this.lblDivinePlea.Text = "Divine Plea:";
            // 
            // nudSpiritual
            // 
            this.nudSpiritual.Increment = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.nudSpiritual.Location = new System.Drawing.Point(146, 46);
            this.nudSpiritual.Maximum = new decimal(new int[] {
            20000,
            0,
            0,
            0});
            this.nudSpiritual.Name = "nudSpiritual";
            this.nudSpiritual.Size = new System.Drawing.Size(145, 20);
            this.nudSpiritual.TabIndex = 8;
            this.nudSpiritual.Value = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.nudSpiritual.ValueChanged += new System.EventHandler(this.nudSpiritual_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(36, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(104, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Spiritual Attunement:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(70, 22);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(70, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Mana Potion:";
            // 
            // cmbManaAmt
            // 
            this.cmbManaAmt.DisplayMember = "2400";
            this.cmbManaAmt.Items.AddRange(new object[] {
            "0",
            "1800",
            "2200",
            "2400"});
            this.cmbManaAmt.Location = new System.Drawing.Point(146, 19);
            this.cmbManaAmt.Name = "cmbManaAmt";
            this.cmbManaAmt.Size = new System.Drawing.Size(145, 21);
            this.cmbManaAmt.TabIndex = 0;
            this.cmbManaAmt.Text = "2400";
            this.cmbManaAmt.ValueMember = "2400";
            this.cmbManaAmt.SelectedIndexChanged += new System.EventHandler(this.cmbManaAmt_SelectedIndexChanged);
            this.cmbManaAmt.TextUpdate += new System.EventHandler(this.cmbManaAmt_TextUpdate);
            // 
            // trkActivity
            // 
            this.trkActivity.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.trkActivity.Location = new System.Drawing.Point(149, 33);
            this.trkActivity.Maximum = 100;
            this.trkActivity.Minimum = 10;
            this.trkActivity.Name = "trkActivity";
            this.trkActivity.Size = new System.Drawing.Size(151, 45);
            this.trkActivity.TabIndex = 23;
            this.trkActivity.TickFrequency = 10;
            this.trkActivity.Value = 90;
            this.trkActivity.Scroll += new System.EventHandler(this.trkActivity_Scroll);
            // 
            // lblActivity
            // 
            this.lblActivity.AutoSize = true;
            this.lblActivity.Location = new System.Drawing.Point(150, 66);
            this.lblActivity.Name = "lblActivity";
            this.lblActivity.Size = new System.Drawing.Size(27, 13);
            this.lblActivity.TabIndex = 24;
            this.lblActivity.Text = "90%";
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(65, 38);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(78, 40);
            this.label9.TabIndex = 25;
            this.label9.Text = "Activity:";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // chkJotP
            // 
            this.chkJotP.AutoSize = true;
            this.chkJotP.Location = new System.Drawing.Point(3, 407);
            this.chkJotP.Name = "chkJotP";
            this.chkJotP.Size = new System.Drawing.Size(181, 17);
            this.chkJotP.TabIndex = 26;
            this.chkJotP.Text = "Maintain Judgements of the Pure";
            this.chkJotP.UseVisualStyleBackColor = true;
            this.chkJotP.CheckedChanged += new System.EventHandler(this.chkJotP_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.lblBoLEff);
            this.groupBox1.Controls.Add(this.trkBoLEff);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.lblBoLUp);
            this.groupBox1.Controls.Add(this.trkBoLUp);
            this.groupBox1.Location = new System.Drawing.Point(3, 284);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(297, 117);
            this.groupBox1.TabIndex = 28;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Beacon of Light";
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(40, 66);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(100, 28);
            this.label6.TabIndex = 33;
            this.label6.Text = "Effectiveness:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblBoLEff
            // 
            this.lblBoLEff.AutoSize = true;
            this.lblBoLEff.Location = new System.Drawing.Point(147, 98);
            this.lblBoLEff.Name = "lblBoLEff";
            this.lblBoLEff.Size = new System.Drawing.Size(27, 13);
            this.lblBoLEff.TabIndex = 32;
            this.lblBoLEff.Text = "90%";
            // 
            // trkBoLEff
            // 
            this.trkBoLEff.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.trkBoLEff.Location = new System.Drawing.Point(146, 66);
            this.trkBoLEff.Maximum = 100;
            this.trkBoLEff.Minimum = 10;
            this.trkBoLEff.Name = "trkBoLEff";
            this.trkBoLEff.Size = new System.Drawing.Size(145, 45);
            this.trkBoLEff.TabIndex = 31;
            this.trkBoLEff.TickFrequency = 10;
            this.trkBoLEff.Value = 90;
            this.trkBoLEff.Scroll += new System.EventHandler(this.trkBoLEff_Scroll);
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(40, 16);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(100, 28);
            this.label5.TabIndex = 30;
            this.label5.Text = "Uptime:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblBoLUp
            // 
            this.lblBoLUp.AutoSize = true;
            this.lblBoLUp.Location = new System.Drawing.Point(147, 50);
            this.lblBoLUp.Name = "lblBoLUp";
            this.lblBoLUp.Size = new System.Drawing.Size(27, 13);
            this.lblBoLUp.TabIndex = 29;
            this.lblBoLUp.Text = "90%";
            // 
            // trkBoLUp
            // 
            this.trkBoLUp.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.trkBoLUp.Location = new System.Drawing.Point(146, 15);
            this.trkBoLUp.Maximum = 100;
            this.trkBoLUp.Minimum = 10;
            this.trkBoLUp.Name = "trkBoLUp";
            this.trkBoLUp.Size = new System.Drawing.Size(145, 45);
            this.trkBoLUp.TabIndex = 28;
            this.trkBoLUp.TickFrequency = 10;
            this.trkBoLUp.Value = 90;
            this.trkBoLUp.Scroll += new System.EventHandler(this.trkBoLUp_Scroll);
            // 
            // trkRatio
            // 
            this.trkRatio.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.trkRatio.LargeChange = 10;
            this.trkRatio.Location = new System.Drawing.Point(56, 85);
            this.trkRatio.Maximum = 100;
            this.trkRatio.Name = "trkRatio";
            this.trkRatio.Size = new System.Drawing.Size(190, 45);
            this.trkRatio.TabIndex = 29;
            this.trkRatio.TickFrequency = 10;
            this.trkRatio.Value = 20;
            this.trkRatio.Scroll += new System.EventHandler(this.trkRatio_Scroll);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(27, 117);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(62, 13);
            this.label7.TabIndex = 30;
            this.label7.Text = "Holy Shock";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(220, 117);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(54, 13);
            this.label8.TabIndex = 31;
            this.label8.Text = "Holy Light";
            // 
            // lblRatio2
            // 
            this.lblRatio2.Location = new System.Drawing.Point(241, 90);
            this.lblRatio2.Name = "lblRatio2";
            this.lblRatio2.Size = new System.Drawing.Size(33, 13);
            this.lblRatio2.TabIndex = 33;
            this.lblRatio2.Text = "100%";
            this.lblRatio2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblRatio1
            // 
            this.lblRatio1.Location = new System.Drawing.Point(27, 90);
            this.lblRatio1.Name = "lblRatio1";
            this.lblRatio1.Size = new System.Drawing.Size(33, 13);
            this.lblRatio1.TabIndex = 34;
            this.lblRatio1.Text = "100%";
            this.lblRatio1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CalculationOptionsPanelHealadin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblRatio1);
            this.Controls.Add(this.lblRatio2);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.trkRatio);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.chkJotP);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.lblActivity);
            this.Controls.Add(this.trkActivity);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cmbLength);
            this.Name = "CalculationOptionsPanelHealadin";
            this.Size = new System.Drawing.Size(303, 712);
            ((System.ComponentModel.ISupportInitialize)(this.cmbLength)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trkReplenishment)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDivinePlea)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSpiritual)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkActivity)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trkBoLEff)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkBoLUp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkRatio)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown cmbLength;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox cmbManaAmt;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TrackBar trkActivity;
        private System.Windows.Forms.Label lblActivity;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown nudSpiritual;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.NumericUpDown nudDivinePlea;
        private System.Windows.Forms.Label lblDivinePlea;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblReplenishment;
        private System.Windows.Forms.TrackBar trkReplenishment;
        private System.Windows.Forms.CheckBox chkJotP;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lblBoLEff;
        private System.Windows.Forms.TrackBar trkBoLEff;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblBoLUp;
        private System.Windows.Forms.TrackBar trkBoLUp;
        private System.Windows.Forms.TrackBar trkRatio;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label lblRatio2;
        private System.Windows.Forms.Label lblRatio1;
    }
}
