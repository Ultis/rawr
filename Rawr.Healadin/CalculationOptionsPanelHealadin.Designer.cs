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
            this.chkBoL = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.cmbLength)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trkReplenishment)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDivinePlea)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSpiritual)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkActivity)).BeginInit();
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
            this.cmbLength.Location = new System.Drawing.Point(97, 7);
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
            this.label3.Location = new System.Drawing.Point(22, 9);
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
            this.groupBox2.Location = new System.Drawing.Point(3, 127);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(206, 148);
            this.groupBox2.TabIndex = 22;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Mana Buffs";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(10, 98);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 28);
            this.label2.TabIndex = 27;
            this.label2.Text = "Replenishment:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblReplenishment
            // 
            this.lblReplenishment.AutoSize = true;
            this.lblReplenishment.Location = new System.Drawing.Point(122, 131);
            this.lblReplenishment.Name = "lblReplenishment";
            this.lblReplenishment.Size = new System.Drawing.Size(27, 13);
            this.lblReplenishment.TabIndex = 26;
            this.lblReplenishment.Text = "90%";
            // 
            // trkReplenishment
            // 
            this.trkReplenishment.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.trkReplenishment.Location = new System.Drawing.Point(116, 98);
            this.trkReplenishment.Maximum = 100;
            this.trkReplenishment.Minimum = 10;
            this.trkReplenishment.Name = "trkReplenishment";
            this.trkReplenishment.Size = new System.Drawing.Size(83, 40);
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
            this.nudDivinePlea.Location = new System.Drawing.Point(116, 73);
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
            this.nudDivinePlea.Size = new System.Drawing.Size(83, 20);
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
            this.lblDivinePlea.Location = new System.Drawing.Point(46, 75);
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
            this.nudSpiritual.Location = new System.Drawing.Point(116, 46);
            this.nudSpiritual.Maximum = new decimal(new int[] {
            20000,
            0,
            0,
            0});
            this.nudSpiritual.Name = "nudSpiritual";
            this.nudSpiritual.Size = new System.Drawing.Size(83, 20);
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
            this.label1.Location = new System.Drawing.Point(6, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(104, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Spiritual Attunement:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(35, 22);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(75, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Mana Potions:";
            // 
            // cmbManaAmt
            // 
            this.cmbManaAmt.DisplayMember = "2400";
            this.cmbManaAmt.Items.AddRange(new object[] {
            "0",
            "1800",
            "2200",
            "2400"});
            this.cmbManaAmt.Location = new System.Drawing.Point(116, 19);
            this.cmbManaAmt.Name = "cmbManaAmt";
            this.cmbManaAmt.Size = new System.Drawing.Size(83, 21);
            this.cmbManaAmt.TabIndex = 0;
            this.cmbManaAmt.Text = "2400";
            this.cmbManaAmt.ValueMember = "2400";
            this.cmbManaAmt.SelectedIndexChanged += new System.EventHandler(this.cmbManaAmt_SelectedIndexChanged);
            this.cmbManaAmt.TextUpdate += new System.EventHandler(this.cmbManaAmt_TextUpdate);
            // 
            // trkActivity
            // 
            this.trkActivity.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.trkActivity.Location = new System.Drawing.Point(94, 33);
            this.trkActivity.Maximum = 100;
            this.trkActivity.Minimum = 10;
            this.trkActivity.Name = "trkActivity";
            this.trkActivity.Size = new System.Drawing.Size(118, 40);
            this.trkActivity.TabIndex = 23;
            this.trkActivity.TickFrequency = 10;
            this.trkActivity.Value = 90;
            this.trkActivity.Scroll += new System.EventHandler(this.trkActivity_Scroll);
            // 
            // lblActivity
            // 
            this.lblActivity.AutoSize = true;
            this.lblActivity.Location = new System.Drawing.Point(105, 65);
            this.lblActivity.Name = "lblActivity";
            this.lblActivity.Size = new System.Drawing.Size(27, 13);
            this.lblActivity.TabIndex = 24;
            this.lblActivity.Text = "90%";
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(13, 33);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(78, 40);
            this.label9.TabIndex = 25;
            this.label9.Text = "Activity:";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // chkJotP
            // 
            this.chkJotP.AutoSize = true;
            this.chkJotP.Location = new System.Drawing.Point(3, 81);
            this.chkJotP.Name = "chkJotP";
            this.chkJotP.Size = new System.Drawing.Size(181, 17);
            this.chkJotP.TabIndex = 26;
            this.chkJotP.Text = "Maintain Judgements of the Pure";
            this.chkJotP.UseVisualStyleBackColor = true;
            this.chkJotP.CheckedChanged += new System.EventHandler(this.chkJotP_CheckedChanged);
            // 
            // chkBoL
            // 
            this.chkBoL.AutoSize = true;
            this.chkBoL.Location = new System.Drawing.Point(3, 104);
            this.chkBoL.Name = "chkBoL";
            this.chkBoL.Size = new System.Drawing.Size(144, 17);
            this.chkBoL.TabIndex = 27;
            this.chkBoL.Text = "Maintain Beacon of Light";
            this.chkBoL.UseVisualStyleBackColor = true;
            this.chkBoL.CheckedChanged += new System.EventHandler(this.chkBoL_CheckedChanged);
            // 
            // CalculationOptionsPanelHealadin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.chkBoL);
            this.Controls.Add(this.chkJotP);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.lblActivity);
            this.Controls.Add(this.trkActivity);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cmbLength);
            this.Name = "CalculationOptionsPanelHealadin";
            this.Size = new System.Drawing.Size(212, 349);
            ((System.ComponentModel.ISupportInitialize)(this.cmbLength)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trkReplenishment)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDivinePlea)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSpiritual)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkActivity)).EndInit();
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
        private System.Windows.Forms.CheckBox chkBoL;
    }
}
