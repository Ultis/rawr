namespace Rawr.RestoSham
{
    partial class CalculationOptionsPanelRestoSham
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
            this.components = new System.ComponentModel.Container();
            this.tipRestoSham = new System.Windows.Forms.ToolTip(this.components);
            this.cboManaPotAmount = new System.Windows.Forms.ComboBox();
            this.chkManaTide = new System.Windows.Forms.CheckBox();
            this.txtESInterval = new System.Windows.Forms.TextBox();
            this.chkMT = new System.Windows.Forms.CheckBox();
            this.txtFightLength = new System.Windows.Forms.TextBox();
            this.chkLHW = new System.Windows.Forms.CheckBox();
            this.chkManaTide2 = new System.Windows.Forms.CheckBox();
            this.chkWaterShield2 = new System.Windows.Forms.CheckBox();
            this.chkWaterShield3 = new System.Windows.Forms.CheckBox();
            this.chkELWGlyph = new System.Windows.Forms.CheckBox();
            this.chkGlyphCH = new System.Windows.Forms.CheckBox();
            this.chkTotemHW1 = new System.Windows.Forms.CheckBox();
            this.errorRestoSham = new System.Windows.Forms.ErrorProvider(this.components);
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tbOverhealing_Label = new System.Windows.Forms.Label();
            this.tbOverhealing = new System.Windows.Forms.TrackBar();
            this.tbBurst_Label = new System.Windows.Forms.Label();
            this.tbBurst = new System.Windows.Forms.TrackBar();
            this.label2 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.chkWaterShield = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            ((System.ComponentModel.ISupportInitialize)(this.errorRestoSham)).BeginInit();
            this.tabPage1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbOverhealing)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbBurst)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // cboManaPotAmount
            // 
            this.cboManaPotAmount.FormattingEnabled = true;
            this.cboManaPotAmount.Items.AddRange(new object[] {
            "0",
            "1500",
            "1800",
            "2400",
            "4300"});
            this.cboManaPotAmount.Location = new System.Drawing.Point(81, 32);
            this.cboManaPotAmount.Name = "cboManaPotAmount";
            this.cboManaPotAmount.Size = new System.Drawing.Size(75, 21);
            this.cboManaPotAmount.TabIndex = 10;
            this.tipRestoSham.SetToolTip(this.cboManaPotAmount, "The average amount of mana restored by a mana potion");
            this.cboManaPotAmount.Validating += new System.ComponentModel.CancelEventHandler(this.numericTextBox_Validating);
            this.cboManaPotAmount.Validated += new System.EventHandler(this.numericTextBox_Validated);
            // 
            // chkManaTide
            // 
            this.chkManaTide.AutoSize = true;
            this.chkManaTide.Location = new System.Drawing.Point(9, 82);
            this.chkManaTide.Name = "chkManaTide";
            this.chkManaTide.Size = new System.Drawing.Size(184, 17);
            this.chkManaTide.TabIndex = 14;
            this.chkManaTide.Text = "Mana Tide totem every cooldown";
            this.tipRestoSham.SetToolTip(this.chkManaTide, "Check to indicate you place a Mana Tide totem every time the cooldown is up");
            this.chkManaTide.UseVisualStyleBackColor = true;
            this.chkManaTide.CheckedChanged += new System.EventHandler(this.chkManaTide_CheckedChanged);
            // 
            // txtESInterval
            // 
            this.txtESInterval.Location = new System.Drawing.Point(111, 122);
            this.txtESInterval.Name = "txtESInterval";
            this.txtESInterval.Size = new System.Drawing.Size(43, 20);
            this.txtESInterval.TabIndex = 11;
            this.tipRestoSham.SetToolTip(this.txtESInterval, "How often you refresh Earth Shield, in seconds (enter 0 if you don\'t use Earth Sh" +
                    "ield)");
            // 
            // chkMT
            // 
            this.chkMT.AutoSize = true;
            this.chkMT.Location = new System.Drawing.Point(9, 59);
            this.chkMT.Name = "chkMT";
            this.chkMT.Size = new System.Drawing.Size(172, 17);
            this.chkMT.TabIndex = 24;
            this.chkMT.Text = "Are you healing the MT or OT?";
            this.tipRestoSham.SetToolTip(this.chkMT, "Check to indicate you healing a single target exclusively and not worried about s" +
                    "plash healing.");
            this.chkMT.UseVisualStyleBackColor = true;
            this.chkMT.CheckedChanged += new System.EventHandler(this.chkMT_CheckedChanged);
            // 
            // txtFightLength
            // 
            this.txtFightLength.Location = new System.Drawing.Point(81, 6);
            this.txtFightLength.Name = "txtFightLength";
            this.txtFightLength.Size = new System.Drawing.Size(59, 20);
            this.txtFightLength.TabIndex = 25;
            this.tipRestoSham.SetToolTip(this.txtFightLength, "Percentage of the fight you are outside the 5-second rule (FSR)");
            this.txtFightLength.Validated += new System.EventHandler(this.numericTextBox_Validated);
            this.txtFightLength.Validating += new System.ComponentModel.CancelEventHandler(this.numericTextBox_Validating);
            // 
            // chkLHW
            // 
            this.chkLHW.AutoSize = true;
            this.chkLHW.Location = new System.Drawing.Point(6, 52);
            this.chkLHW.Name = "chkLHW";
            this.chkLHW.Size = new System.Drawing.Size(199, 17);
            this.chkLHW.TabIndex = 25;
            this.chkLHW.Text = "Major Glyph of Lesser Healing Wave";
            this.tipRestoSham.SetToolTip(this.chkLHW, "Check to indicate you are using the Major Glyph of Lesser Healing Wave");
            this.chkLHW.UseVisualStyleBackColor = true;
            this.chkLHW.CheckedChanged += new System.EventHandler(this.chkLHW_CheckedChanged);
            // 
            // chkManaTide2
            // 
            this.chkManaTide2.AutoSize = true;
            this.chkManaTide2.Location = new System.Drawing.Point(6, 75);
            this.chkManaTide2.Name = "chkManaTide2";
            this.chkManaTide2.Size = new System.Drawing.Size(181, 17);
            this.chkManaTide2.TabIndex = 24;
            this.chkManaTide2.Text = "Major Glyph of Mana Tide Totem";
            this.tipRestoSham.SetToolTip(this.chkManaTide2, "Check to indicate you are using the Major Glyph of Mana Tide Totem");
            this.chkManaTide2.UseVisualStyleBackColor = true;
            this.chkManaTide2.CheckedChanged += new System.EventHandler(this.chkManaTide2_CheckedChanged);
            // 
            // chkWaterShield2
            // 
            this.chkWaterShield2.AutoSize = true;
            this.chkWaterShield2.Location = new System.Drawing.Point(6, 121);
            this.chkWaterShield2.Name = "chkWaterShield2";
            this.chkWaterShield2.Size = new System.Drawing.Size(158, 17);
            this.chkWaterShield2.TabIndex = 26;
            this.chkWaterShield2.Text = "Minor Glyph of Water Shield";
            this.tipRestoSham.SetToolTip(this.chkWaterShield2, "Check if you have the Minor Glyph of Water Shield");
            this.chkWaterShield2.UseVisualStyleBackColor = true;
            this.chkWaterShield2.CheckedChanged += new System.EventHandler(this.chkWaterShield2_CheckedChanged);
            // 
            // chkWaterShield3
            // 
            this.chkWaterShield3.AutoSize = true;
            this.chkWaterShield3.Location = new System.Drawing.Point(6, 98);
            this.chkWaterShield3.Name = "chkWaterShield3";
            this.chkWaterShield3.Size = new System.Drawing.Size(166, 17);
            this.chkWaterShield3.TabIndex = 27;
            this.chkWaterShield3.Text = "Major Glyph of Water Mastery";
            this.tipRestoSham.SetToolTip(this.chkWaterShield3, "Check if you have the Major Glyph of Water Mastery");
            this.chkWaterShield3.UseVisualStyleBackColor = true;
            this.chkWaterShield3.CheckedChanged += new System.EventHandler(this.chkWaterShield3_CheckedChanged);
            // 
            // chkELWGlyph
            // 
            this.chkELWGlyph.AutoSize = true;
            this.chkELWGlyph.Location = new System.Drawing.Point(6, 6);
            this.chkELWGlyph.Name = "chkELWGlyph";
            this.chkELWGlyph.Size = new System.Drawing.Size(190, 17);
            this.chkELWGlyph.TabIndex = 28;
            this.chkELWGlyph.TabStop = false;
            this.chkELWGlyph.Text = "Major Glyph of Earthliving Weapon";
            this.tipRestoSham.SetToolTip(this.chkELWGlyph, "Check to indicate you are using the Major Glyph of Earthliving Weapon");
            this.chkELWGlyph.UseVisualStyleBackColor = true;
            this.chkELWGlyph.CheckedChanged += new System.EventHandler(this.chkELWGlyph_CheckedChanged);
            // 
            // chkGlyphCH
            // 
            this.chkGlyphCH.AutoSize = true;
            this.chkGlyphCH.Location = new System.Drawing.Point(6, 29);
            this.chkGlyphCH.Name = "chkGlyphCH";
            this.chkGlyphCH.Size = new System.Drawing.Size(144, 17);
            this.chkGlyphCH.TabIndex = 29;
            this.chkGlyphCH.TabStop = false;
            this.chkGlyphCH.Text = "Major Glyph of Chainheal";
            this.tipRestoSham.SetToolTip(this.chkGlyphCH, "Check to indicate you are using the Major Glyph of Chainheal");
            this.chkGlyphCH.UseVisualStyleBackColor = true;
            this.chkGlyphCH.CheckedChanged += new System.EventHandler(this.chkGlyphCH_CheckedChanged);
            // 
            // chkTotemHW1
            // 
            this.chkTotemHW1.AutoSize = true;
            this.chkTotemHW1.Location = new System.Drawing.Point(6, 6);
            this.chkTotemHW1.Name = "chkTotemHW1";
            this.chkTotemHW1.Size = new System.Drawing.Size(171, 17);
            this.chkTotemHW1.TabIndex = 16;
            this.chkTotemHW1.Text = "Totem of Spontaneous Growth";
            this.tipRestoSham.SetToolTip(this.chkTotemHW1, "Check if you are using this totem.  Only choose one.");
            this.chkTotemHW1.UseVisualStyleBackColor = true;
            this.chkTotemHW1.CheckedChanged += new System.EventHandler(this.chkTotemHW1_CheckedChanged);
            // 
            // errorRestoSham
            // 
            this.errorRestoSham.ContainerControl = this;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.label18);
            this.tabPage1.Controls.Add(this.txtESInterval);
            this.tabPage1.Controls.Add(this.label17);
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.txtFightLength);
            this.tabPage1.Controls.Add(this.chkMT);
            this.tabPage1.Controls.Add(this.chkWaterShield);
            this.tabPage1.Controls.Add(this.chkManaTide);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.cboManaPotAmount);
            this.tabPage1.Controls.Add(this.label7);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(204, 501);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "General";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tbOverhealing_Label);
            this.groupBox1.Controls.Add(this.tbOverhealing);
            this.groupBox1.Controls.Add(this.tbBurst_Label);
            this.groupBox1.Controls.Add(this.tbBurst);
            this.groupBox1.Location = new System.Drawing.Point(6, 148);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(192, 153);
            this.groupBox1.TabIndex = 27;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Healing Style";
            // 
            // tbOverhealing_Label
            // 
            this.tbOverhealing_Label.AutoSize = true;
            this.tbOverhealing_Label.Location = new System.Drawing.Point(6, 84);
            this.tbOverhealing_Label.Name = "tbOverhealing_Label";
            this.tbOverhealing_Label.Size = new System.Drawing.Size(84, 13);
            this.tbOverhealing_Label.TabIndex = 5;
            this.tbOverhealing_Label.Text = "Overhealing (%):";
            // 
            // tbOverhealing
            // 
            this.tbOverhealing.Location = new System.Drawing.Point(6, 100);
            this.tbOverhealing.Maximum = 100;
            this.tbOverhealing.Name = "tbOverhealing";
            this.tbOverhealing.Size = new System.Drawing.Size(179, 42);
            this.tbOverhealing.TabIndex = 4;
            this.tbOverhealing.Scroll += new System.EventHandler(this.OnTrackBarScroll);
            // 
            // tbBurst_Label
            // 
            this.tbBurst_Label.AutoSize = true;
            this.tbBurst_Label.Location = new System.Drawing.Point(7, 20);
            this.tbBurst_Label.Name = "tbBurst_Label";
            this.tbBurst_Label.Size = new System.Drawing.Size(51, 13);
            this.tbBurst_Label.TabIndex = 1;
            this.tbBurst_Label.Text = "Burst (%):";
            // 
            // tbBurst
            // 
            this.tbBurst.Location = new System.Drawing.Point(7, 36);
            this.tbBurst.Maximum = 100;
            this.tbBurst.Name = "tbBurst";
            this.tbBurst.Size = new System.Drawing.Size(179, 42);
            this.tbBurst.TabIndex = 0;
            this.tbBurst.Scroll += new System.EventHandler(this.OnTrackBarScroll);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(141, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(23, 13);
            this.label2.TabIndex = 26;
            this.label2.Text = "min";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(6, 125);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(99, 13);
            this.label17.TabIndex = 10;
            this.label17.Text = "Earthshield Recast:";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(160, 125);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(24, 13);
            this.label18.TabIndex = 12;
            this.label18.Text = "sec";
            // 
            // chkWaterShield
            // 
            this.chkWaterShield.AutoSize = true;
            this.chkWaterShield.Location = new System.Drawing.Point(9, 105);
            this.chkWaterShield.Name = "chkWaterShield";
            this.chkWaterShield.Size = new System.Drawing.Size(87, 17);
            this.chkWaterShield.TabIndex = 15;
            this.chkWaterShield.Text = "Water Shield";
            this.chkWaterShield.UseVisualStyleBackColor = true;
            this.chkWaterShield.CheckedChanged += new System.EventHandler(this.chkWaterShield_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Fight Length:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 35);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(75, 13);
            this.label7.TabIndex = 9;
            this.label7.Text = "Mana Potions:";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(212, 527);
            this.tabControl1.TabIndex = 14;
            this.tabControl1.Tag = "";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.chkGlyphCH);
            this.tabPage2.Controls.Add(this.chkELWGlyph);
            this.tabPage2.Controls.Add(this.chkWaterShield3);
            this.tabPage2.Controls.Add(this.chkWaterShield2);
            this.tabPage2.Controls.Add(this.chkLHW);
            this.tabPage2.Controls.Add(this.chkManaTide2);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(204, 501);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Glyphs";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.chkTotemHW1);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(204, 501);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Relics";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // CalculationOptionsPanelRestoSham
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Name = "CalculationOptionsPanelRestoSham";
            this.Size = new System.Drawing.Size(212, 527);
            ((System.ComponentModel.ISupportInitialize)(this.errorRestoSham)).EndInit();
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbOverhealing)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbBurst)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolTip tipRestoSham;
        private System.Windows.Forms.ErrorProvider errorRestoSham;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.CheckBox chkWaterShield;
        private System.Windows.Forms.CheckBox chkManaTide;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboManaPotAmount;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox txtESInterval;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.CheckBox chkMT;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtFightLength;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.CheckBox chkWaterShield2;
        private System.Windows.Forms.CheckBox chkLHW;
        private System.Windows.Forms.CheckBox chkManaTide2;
        private System.Windows.Forms.CheckBox chkWaterShield3;
        private System.Windows.Forms.CheckBox chkGlyphCH;
        private System.Windows.Forms.CheckBox chkELWGlyph;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TrackBar tbBurst;
        private System.Windows.Forms.Label tbOverhealing_Label;
        private System.Windows.Forms.TrackBar tbOverhealing;
        private System.Windows.Forms.Label tbBurst_Label;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.CheckBox chkTotemHW1;

    }
}
