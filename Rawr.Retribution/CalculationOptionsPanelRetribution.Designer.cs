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
            this.chkGlyphSenseUndead = new System.Windows.Forms.CheckBox();
            this.chkGlyphConsecration = new System.Windows.Forms.CheckBox();
            this.chkGlyphJudgement = new System.Windows.Forms.CheckBox();
            this.cmbMobType = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbLevel = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.butPriority6Down = new System.Windows.Forms.Button();
            this.butPriority6Up = new System.Windows.Forms.Button();
            this.butPriority5Down = new System.Windows.Forms.Button();
            this.butPriority5Up = new System.Windows.Forms.Button();
            this.butPriority4Down = new System.Windows.Forms.Button();
            this.butPriority4Up = new System.Windows.Forms.Button();
            this.lblPriority6 = new System.Windows.Forms.Label();
            this.butPriority3Down = new System.Windows.Forms.Button();
            this.lblPriority5 = new System.Windows.Forms.Label();
            this.butPriority3Up = new System.Windows.Forms.Button();
            this.lblPriority4 = new System.Windows.Forms.Label();
            this.butPriority2Down = new System.Windows.Forms.Button();
            this.lblPriority3 = new System.Windows.Forms.Label();
            this.butPriority2Up = new System.Windows.Forms.Button();
            this.lblPriority2 = new System.Windows.Forms.Label();
            this.butPriority1Down = new System.Windows.Forms.Button();
            this.butPriority1Up = new System.Windows.Forms.Button();
            this.lblPriority1 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.trkTime20 = new System.Windows.Forms.TrackBar();
            this.label12 = new System.Windows.Forms.Label();
            this.cmbLength = new System.Windows.Forms.NumericUpDown();
            this.lblTime20 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trkTime20)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbLength)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkGlyphSenseUndead);
            this.groupBox1.Controls.Add(this.chkGlyphConsecration);
            this.groupBox1.Controls.Add(this.chkGlyphJudgement);
            this.groupBox1.Location = new System.Drawing.Point(3, 310);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(146, 91);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Glyphs";
            // 
            // chkGlyphSenseUndead
            // 
            this.chkGlyphSenseUndead.AutoSize = true;
            this.chkGlyphSenseUndead.Location = new System.Drawing.Point(6, 65);
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
            this.groupBox2.Controls.Add(this.butPriority6Down);
            this.groupBox2.Controls.Add(this.butPriority6Up);
            this.groupBox2.Controls.Add(this.butPriority5Down);
            this.groupBox2.Controls.Add(this.butPriority5Up);
            this.groupBox2.Controls.Add(this.butPriority4Down);
            this.groupBox2.Controls.Add(this.butPriority4Up);
            this.groupBox2.Controls.Add(this.lblPriority6);
            this.groupBox2.Controls.Add(this.butPriority3Down);
            this.groupBox2.Controls.Add(this.lblPriority5);
            this.groupBox2.Controls.Add(this.butPriority3Up);
            this.groupBox2.Controls.Add(this.lblPriority4);
            this.groupBox2.Controls.Add(this.butPriority2Down);
            this.groupBox2.Controls.Add(this.lblPriority3);
            this.groupBox2.Controls.Add(this.butPriority2Up);
            this.groupBox2.Controls.Add(this.lblPriority2);
            this.groupBox2.Controls.Add(this.butPriority1Down);
            this.groupBox2.Controls.Add(this.butPriority1Up);
            this.groupBox2.Controls.Add(this.lblPriority1);
            this.groupBox2.Location = new System.Drawing.Point(3, 110);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(235, 194);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Rotation";
            // 
            // butPriority6Down
            // 
            this.butPriority6Down.Enabled = false;
            this.butPriority6Down.Location = new System.Drawing.Point(206, 164);
            this.butPriority6Down.Name = "butPriority6Down";
            this.butPriority6Down.Size = new System.Drawing.Size(23, 23);
            this.butPriority6Down.TabIndex = 8;
            this.butPriority6Down.Text = "-";
            this.butPriority6Down.UseVisualStyleBackColor = true;
            // 
            // butPriority6Up
            // 
            this.butPriority6Up.Location = new System.Drawing.Point(177, 164);
            this.butPriority6Up.Name = "butPriority6Up";
            this.butPriority6Up.Size = new System.Drawing.Size(23, 23);
            this.butPriority6Up.TabIndex = 7;
            this.butPriority6Up.Text = "+";
            this.butPriority6Up.UseVisualStyleBackColor = true;
            this.butPriority6Up.Click += new System.EventHandler(this.PrioritySwitch5);
            // 
            // butPriority5Down
            // 
            this.butPriority5Down.Location = new System.Drawing.Point(206, 135);
            this.butPriority5Down.Name = "butPriority5Down";
            this.butPriority5Down.Size = new System.Drawing.Size(23, 23);
            this.butPriority5Down.TabIndex = 8;
            this.butPriority5Down.Text = "-";
            this.butPriority5Down.UseVisualStyleBackColor = true;
            this.butPriority5Down.Click += new System.EventHandler(this.PrioritySwitch5);
            // 
            // butPriority5Up
            // 
            this.butPriority5Up.Location = new System.Drawing.Point(177, 135);
            this.butPriority5Up.Name = "butPriority5Up";
            this.butPriority5Up.Size = new System.Drawing.Size(23, 23);
            this.butPriority5Up.TabIndex = 7;
            this.butPriority5Up.Text = "+";
            this.butPriority5Up.UseVisualStyleBackColor = true;
            this.butPriority5Up.Click += new System.EventHandler(this.PrioritySwitch4);
            // 
            // butPriority4Down
            // 
            this.butPriority4Down.Location = new System.Drawing.Point(206, 106);
            this.butPriority4Down.Name = "butPriority4Down";
            this.butPriority4Down.Size = new System.Drawing.Size(23, 23);
            this.butPriority4Down.TabIndex = 8;
            this.butPriority4Down.Text = "-";
            this.butPriority4Down.UseVisualStyleBackColor = true;
            this.butPriority4Down.Click += new System.EventHandler(this.PrioritySwitch4);
            // 
            // butPriority4Up
            // 
            this.butPriority4Up.Location = new System.Drawing.Point(177, 106);
            this.butPriority4Up.Name = "butPriority4Up";
            this.butPriority4Up.Size = new System.Drawing.Size(23, 23);
            this.butPriority4Up.TabIndex = 7;
            this.butPriority4Up.Text = "+";
            this.butPriority4Up.UseVisualStyleBackColor = true;
            this.butPriority4Up.Click += new System.EventHandler(this.PrioritySwitch3);
            // 
            // lblPriority6
            // 
            this.lblPriority6.Location = new System.Drawing.Point(6, 169);
            this.lblPriority6.Name = "lblPriority6";
            this.lblPriority6.Size = new System.Drawing.Size(167, 13);
            this.lblPriority6.TabIndex = 6;
            this.lblPriority6.Text = "Judgement";
            // 
            // butPriority3Down
            // 
            this.butPriority3Down.Location = new System.Drawing.Point(206, 77);
            this.butPriority3Down.Name = "butPriority3Down";
            this.butPriority3Down.Size = new System.Drawing.Size(23, 23);
            this.butPriority3Down.TabIndex = 8;
            this.butPriority3Down.Text = "-";
            this.butPriority3Down.UseVisualStyleBackColor = true;
            this.butPriority3Down.Click += new System.EventHandler(this.PrioritySwitch3);
            // 
            // lblPriority5
            // 
            this.lblPriority5.Location = new System.Drawing.Point(6, 140);
            this.lblPriority5.Name = "lblPriority5";
            this.lblPriority5.Size = new System.Drawing.Size(167, 13);
            this.lblPriority5.TabIndex = 6;
            this.lblPriority5.Text = "Judgement";
            // 
            // butPriority3Up
            // 
            this.butPriority3Up.Location = new System.Drawing.Point(177, 77);
            this.butPriority3Up.Name = "butPriority3Up";
            this.butPriority3Up.Size = new System.Drawing.Size(23, 23);
            this.butPriority3Up.TabIndex = 7;
            this.butPriority3Up.Text = "+";
            this.butPriority3Up.UseVisualStyleBackColor = true;
            this.butPriority3Up.Click += new System.EventHandler(this.PrioritySwitch2);
            // 
            // lblPriority4
            // 
            this.lblPriority4.Location = new System.Drawing.Point(6, 111);
            this.lblPriority4.Name = "lblPriority4";
            this.lblPriority4.Size = new System.Drawing.Size(167, 13);
            this.lblPriority4.TabIndex = 6;
            this.lblPriority4.Text = "Judgement";
            // 
            // butPriority2Down
            // 
            this.butPriority2Down.Location = new System.Drawing.Point(206, 48);
            this.butPriority2Down.Name = "butPriority2Down";
            this.butPriority2Down.Size = new System.Drawing.Size(23, 23);
            this.butPriority2Down.TabIndex = 8;
            this.butPriority2Down.Text = "-";
            this.butPriority2Down.UseVisualStyleBackColor = true;
            this.butPriority2Down.Click += new System.EventHandler(this.PrioritySwitch2);
            // 
            // lblPriority3
            // 
            this.lblPriority3.Location = new System.Drawing.Point(6, 82);
            this.lblPriority3.Name = "lblPriority3";
            this.lblPriority3.Size = new System.Drawing.Size(167, 13);
            this.lblPriority3.TabIndex = 6;
            this.lblPriority3.Text = "Judgement";
            // 
            // butPriority2Up
            // 
            this.butPriority2Up.Location = new System.Drawing.Point(177, 48);
            this.butPriority2Up.Name = "butPriority2Up";
            this.butPriority2Up.Size = new System.Drawing.Size(23, 23);
            this.butPriority2Up.TabIndex = 7;
            this.butPriority2Up.Text = "+";
            this.butPriority2Up.UseVisualStyleBackColor = true;
            this.butPriority2Up.Click += new System.EventHandler(this.PrioritySwitch1);
            // 
            // lblPriority2
            // 
            this.lblPriority2.Location = new System.Drawing.Point(6, 53);
            this.lblPriority2.Name = "lblPriority2";
            this.lblPriority2.Size = new System.Drawing.Size(167, 13);
            this.lblPriority2.TabIndex = 6;
            this.lblPriority2.Text = "Judgement";
            // 
            // butPriority1Down
            // 
            this.butPriority1Down.Location = new System.Drawing.Point(206, 19);
            this.butPriority1Down.Name = "butPriority1Down";
            this.butPriority1Down.Size = new System.Drawing.Size(23, 23);
            this.butPriority1Down.TabIndex = 5;
            this.butPriority1Down.Text = "-";
            this.butPriority1Down.UseVisualStyleBackColor = true;
            this.butPriority1Down.Click += new System.EventHandler(this.PrioritySwitch1);
            // 
            // butPriority1Up
            // 
            this.butPriority1Up.Enabled = false;
            this.butPriority1Up.Location = new System.Drawing.Point(177, 19);
            this.butPriority1Up.Name = "butPriority1Up";
            this.butPriority1Up.Size = new System.Drawing.Size(23, 23);
            this.butPriority1Up.TabIndex = 4;
            this.butPriority1Up.Text = "+";
            this.butPriority1Up.UseVisualStyleBackColor = true;
            // 
            // lblPriority1
            // 
            this.lblPriority1.Location = new System.Drawing.Point(6, 24);
            this.lblPriority1.Name = "lblPriority1";
            this.lblPriority1.Size = new System.Drawing.Size(167, 13);
            this.lblPriority1.TabIndex = 0;
            this.lblPriority1.Text = "Judgement";
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
            ((System.ComponentModel.ISupportInitialize)(this.trkTime20)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbLength)).EndInit();
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
        private System.Windows.Forms.Label lblPriority1;
        private System.Windows.Forms.Button butPriority6Down;
        private System.Windows.Forms.Button butPriority6Up;
        private System.Windows.Forms.Button butPriority5Down;
        private System.Windows.Forms.Button butPriority5Up;
        private System.Windows.Forms.Button butPriority4Down;
        private System.Windows.Forms.Button butPriority4Up;
        private System.Windows.Forms.Label lblPriority6;
        private System.Windows.Forms.Button butPriority3Down;
        private System.Windows.Forms.Label lblPriority5;
        private System.Windows.Forms.Button butPriority3Up;
        private System.Windows.Forms.Label lblPriority4;
        private System.Windows.Forms.Button butPriority2Down;
        private System.Windows.Forms.Label lblPriority3;
        private System.Windows.Forms.Button butPriority2Up;
        private System.Windows.Forms.Label lblPriority2;
        private System.Windows.Forms.Button butPriority1Down;
        private System.Windows.Forms.Button butPriority1Up;

    }
}