namespace Rawr.Tree
{
    partial class CalculationOptionsPanelTree
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
            this.trkActivity = new System.Windows.Forms.TrackBar();
            this.lblActivity = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.cmbTreeOfLife = new System.Windows.Forms.ComboBox();
            this.cmbEmpoweredRejuvenation = new System.Windows.Forms.ComboBox();
            this.cmbGiftOfNature = new System.Windows.Forms.ComboBox();
            this.cmbImprovedRegrowth = new System.Windows.Forms.ComboBox();
            this.cmbImprovedRejuvenation = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.cmbNaturalPerfection = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.cmbLivingSpirit = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbIntensity = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.cmbLength)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbSpriest)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbManaTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkActivity)).BeginInit();
            this.groupBox1.SuspendLayout();
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
            20,
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
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.cmbSpriest);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.cmbManaTime);
            this.groupBox2.Controls.Add(this.cmbManaAmt);
            this.groupBox2.Location = new System.Drawing.Point(4, 81);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(205, 108);
            this.groupBox2.TabIndex = 22;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Mana Buffs";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(171, 40);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(28, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "mins";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(87, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(33, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "every";
            // 
            // cmbSpriest
            // 
            this.cmbSpriest.Increment = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.cmbSpriest.Location = new System.Drawing.Point(19, 77);
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
            this.label7.Location = new System.Drawing.Point(101, 79);
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
            5,
            0,
            0,
            65536});
            this.cmbManaTime.Location = new System.Drawing.Point(125, 38);
            this.cmbManaTime.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.cmbManaTime.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.cmbManaTime.Name = "cmbManaTime";
            this.cmbManaTime.Size = new System.Drawing.Size(40, 20);
            this.cmbManaTime.TabIndex = 1;
            this.cmbManaTime.Value = new decimal(new int[] {
            25,
            0,
            0,
            65536});
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
            this.cmbManaAmt.Location = new System.Drawing.Point(19, 37);
            this.cmbManaAmt.Name = "cmbManaAmt";
            this.cmbManaAmt.Size = new System.Drawing.Size(61, 21);
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
            this.trkActivity.Size = new System.Drawing.Size(118, 42);
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
            this.label9.Size = new System.Drawing.Size(78, 45);
            this.label9.TabIndex = 25;
            this.label9.Text = "Activity:";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label15);
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.cmbTreeOfLife);
            this.groupBox1.Controls.Add(this.cmbEmpoweredRejuvenation);
            this.groupBox1.Controls.Add(this.cmbGiftOfNature);
            this.groupBox1.Controls.Add(this.cmbImprovedRegrowth);
            this.groupBox1.Controls.Add(this.cmbImprovedRejuvenation);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.cmbNaturalPerfection);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.cmbLivingSpirit);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.cmbIntensity);
            this.groupBox1.Location = new System.Drawing.Point(4, 195);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(205, 238);
            this.groupBox1.TabIndex = 26;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Talents";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(9, 214);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(61, 13);
            this.label15.TabIndex = 25;
            this.label15.Text = "Tree of Life";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(9, 187);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(129, 13);
            this.label14.TabIndex = 24;
            this.label14.Text = "Empowered Rejuvenation";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(9, 160);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(70, 13);
            this.label13.TabIndex = 23;
            this.label13.Text = "Gift of Nature";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(9, 133);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(100, 13);
            this.label12.TabIndex = 22;
            this.label12.Text = "Improved Regrowth";
            // 
            // cmbTreeOfLife
            // 
            this.cmbTreeOfLife.Items.AddRange(new object[] {
            "0",
            "1"});
            this.cmbTreeOfLife.Location = new System.Drawing.Point(153, 211);
            this.cmbTreeOfLife.Name = "cmbTreeOfLife";
            this.cmbTreeOfLife.Size = new System.Drawing.Size(33, 21);
            this.cmbTreeOfLife.TabIndex = 21;
            this.cmbTreeOfLife.Text = "1";
            this.cmbTreeOfLife.SelectedIndexChanged += new System.EventHandler(this.cmbTreeOfLife_SelectedIndexChanged);
            // 
            // cmbEmpoweredRejuvenation
            // 
            this.cmbEmpoweredRejuvenation.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "3",
            "4",
            "5"});
            this.cmbEmpoweredRejuvenation.Location = new System.Drawing.Point(153, 184);
            this.cmbEmpoweredRejuvenation.Name = "cmbEmpoweredRejuvenation";
            this.cmbEmpoweredRejuvenation.Size = new System.Drawing.Size(33, 21);
            this.cmbEmpoweredRejuvenation.TabIndex = 20;
            this.cmbEmpoweredRejuvenation.Text = "5";
            this.cmbEmpoweredRejuvenation.SelectedIndexChanged += new System.EventHandler(this.cmbEmpoweredRejuvenation_SelectedIndexChanged);
            // 
            // cmbGiftOfNature
            // 
            this.cmbGiftOfNature.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "3",
            "4",
            "5"});
            this.cmbGiftOfNature.Location = new System.Drawing.Point(153, 157);
            this.cmbGiftOfNature.Name = "cmbGiftOfNature";
            this.cmbGiftOfNature.Size = new System.Drawing.Size(33, 21);
            this.cmbGiftOfNature.TabIndex = 19;
            this.cmbGiftOfNature.Text = "5";
            this.cmbGiftOfNature.SelectedIndexChanged += new System.EventHandler(this.cmbGiftOfNature_SelectedIndexChanged);
            // 
            // cmbImprovedRegrowth
            // 
            this.cmbImprovedRegrowth.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "3",
            "4",
            "5"});
            this.cmbImprovedRegrowth.Location = new System.Drawing.Point(153, 130);
            this.cmbImprovedRegrowth.Name = "cmbImprovedRegrowth";
            this.cmbImprovedRegrowth.Size = new System.Drawing.Size(33, 21);
            this.cmbImprovedRegrowth.TabIndex = 18;
            this.cmbImprovedRegrowth.Text = "5";
            this.cmbImprovedRegrowth.SelectedIndexChanged += new System.EventHandler(this.cmbImprovedRegrowth_SelectedIndexChanged);
            // 
            // cmbImprovedRejuvenation
            // 
            this.cmbImprovedRejuvenation.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "3"});
            this.cmbImprovedRejuvenation.Location = new System.Drawing.Point(153, 103);
            this.cmbImprovedRejuvenation.Name = "cmbImprovedRejuvenation";
            this.cmbImprovedRejuvenation.Size = new System.Drawing.Size(33, 21);
            this.cmbImprovedRejuvenation.TabIndex = 17;
            this.cmbImprovedRejuvenation.Text = "3";
            this.cmbImprovedRejuvenation.SelectedIndexChanged += new System.EventHandler(this.cmbImprovedRejuvenation_SelectedIndexChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(9, 106);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(120, 13);
            this.label11.TabIndex = 16;
            this.label11.Text = "Improved Rejuvenation:";
            // 
            // cmbNaturalPerfection
            // 
            this.cmbNaturalPerfection.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "3"});
            this.cmbNaturalPerfection.Location = new System.Drawing.Point(153, 76);
            this.cmbNaturalPerfection.Name = "cmbNaturalPerfection";
            this.cmbNaturalPerfection.Size = new System.Drawing.Size(33, 21);
            this.cmbNaturalPerfection.TabIndex = 15;
            this.cmbNaturalPerfection.Text = "3";
            this.cmbNaturalPerfection.SelectedIndexChanged += new System.EventHandler(this.cmbNaturalPerfection_SelectedIndexChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(9, 79);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(95, 13);
            this.label10.TabIndex = 14;
            this.label10.Text = "Natural Perfection:";
            // 
            // cmbLivingSpirit
            // 
            this.cmbLivingSpirit.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "3"});
            this.cmbLivingSpirit.Location = new System.Drawing.Point(153, 49);
            this.cmbLivingSpirit.Name = "cmbLivingSpirit";
            this.cmbLivingSpirit.Size = new System.Drawing.Size(33, 21);
            this.cmbLivingSpirit.TabIndex = 13;
            this.cmbLivingSpirit.Text = "3";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(9, 52);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(64, 13);
            this.label8.TabIndex = 12;
            this.label8.Text = "Living Spirit:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Intensity:";
            // 
            // cmbIntensity
            // 
            this.cmbIntensity.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "3"});
            this.cmbIntensity.Location = new System.Drawing.Point(153, 22);
            this.cmbIntensity.Name = "cmbIntensity";
            this.cmbIntensity.Size = new System.Drawing.Size(33, 21);
            this.cmbIntensity.TabIndex = 11;
            this.cmbIntensity.Text = "3";
            this.cmbIntensity.SelectedIndexChanged += new System.EventHandler(this.cmbIntensity_SelectedIndexChanged);
            // 
            // CalculationOptionsPanelTree
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.lblActivity);
            this.Controls.Add(this.trkActivity);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cmbLength);
            this.Name = "CalculationOptionsPanelTree";
            this.Size = new System.Drawing.Size(212, 452);
            ((System.ComponentModel.ISupportInitialize)(this.cmbLength)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbSpriest)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbManaTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkActivity)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
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
        private System.Windows.Forms.TrackBar trkActivity;
        private System.Windows.Forms.Label lblActivity;
        private System.Windows.Forms.NumericUpDown cmbSpriest;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbIntensity;
        private System.Windows.Forms.ComboBox cmbLivingSpirit;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cmbNaturalPerfection;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox cmbImprovedRejuvenation;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox cmbTreeOfLife;
        private System.Windows.Forms.ComboBox cmbEmpoweredRejuvenation;
        private System.Windows.Forms.ComboBox cmbGiftOfNature;
        private System.Windows.Forms.ComboBox cmbImprovedRegrowth;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
    }
}
