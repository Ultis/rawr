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
            this.txtOutsideFSR = new System.Windows.Forms.TextBox();
            this.cboManaPotAmount = new System.Windows.Forms.ComboBox();
            this.chkManaTide = new System.Windows.Forms.CheckBox();
            this.txtESInterval = new System.Windows.Forms.TextBox();
            this.chkMT = new System.Windows.Forms.CheckBox();
            this.txtFightLength = new System.Windows.Forms.TextBox();
            this.chkLHW = new System.Windows.Forms.CheckBox();
            this.chkManaTide2 = new System.Windows.Forms.CheckBox();
            this.chkWaterShield2 = new System.Windows.Forms.CheckBox();
            this.chkWaterShield3 = new System.Windows.Forms.CheckBox();
            this.errorRestoSham = new System.Windows.Forms.ErrorProvider(this.components);
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label17 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.chkWaterShield = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            ((System.ComponentModel.ISupportInitialize)(this.errorRestoSham)).BeginInit();
            this.tabPage1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtOutsideFSR
            // 
            this.txtOutsideFSR.Location = new System.Drawing.Point(81, 32);
            this.txtOutsideFSR.Name = "txtOutsideFSR";
            this.txtOutsideFSR.Size = new System.Drawing.Size(59, 20);
            this.txtOutsideFSR.TabIndex = 4;
            this.tipRestoSham.SetToolTip(this.txtOutsideFSR, "Percentage of the fight you are outside the 5-second rule (FSR)");
            this.txtOutsideFSR.Validated += new System.EventHandler(this.numericTextBox_Validated);
            this.txtOutsideFSR.Validating += new System.ComponentModel.CancelEventHandler(this.numericTextBox_Validating);
            // 
            // cboManaPotAmount
            // 
            this.cboManaPotAmount.FormattingEnabled = true;
            this.cboManaPotAmount.Items.AddRange(new object[] {
            "0",
            "1500",
            "1800",
            "2400"});
            this.cboManaPotAmount.Location = new System.Drawing.Point(87, 58);
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
            this.chkManaTide.Location = new System.Drawing.Point(6, 85);
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
            this.txtESInterval.Location = new System.Drawing.Point(69, 13);
            this.txtESInterval.Name = "txtESInterval";
            this.txtESInterval.Size = new System.Drawing.Size(43, 20);
            this.txtESInterval.TabIndex = 11;
            this.tipRestoSham.SetToolTip(this.txtESInterval, "How often you refresh Earth Shield, in seconds (enter 0 if you don\'t use Earth Sh" +
                    "ield)");
            // 
            // chkMT
            // 
            this.chkMT.AutoSize = true;
            this.chkMT.Location = new System.Drawing.Point(6, 177);
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
            this.chkLHW.Location = new System.Drawing.Point(6, 28);
            this.chkLHW.Name = "chkLHW";
            this.chkLHW.Size = new System.Drawing.Size(170, 17);
            this.chkLHW.TabIndex = 25;
            this.chkLHW.Text = "Glyph of Lesser Healing Wave";
            this.tipRestoSham.SetToolTip(this.chkLHW, "Check to indicate you are using the Glyph of Lesser Healing Wave");
            this.chkLHW.UseVisualStyleBackColor = true;
            this.chkLHW.CheckedChanged += new System.EventHandler(this.chkLHW_CheckedChanged);
            // 
            // chkManaTide2
            // 
            this.chkManaTide2.AutoSize = true;
            this.chkManaTide2.Location = new System.Drawing.Point(6, 6);
            this.chkManaTide2.Name = "chkManaTide2";
            this.chkManaTide2.Size = new System.Drawing.Size(152, 17);
            this.chkManaTide2.TabIndex = 24;
            this.chkManaTide2.Text = "Glyph of Mana Tide Totem";
            this.tipRestoSham.SetToolTip(this.chkManaTide2, "Check to indicate you are using the Glyph of Mana Tide Totem");
            this.chkManaTide2.UseVisualStyleBackColor = true;
            this.chkManaTide2.CheckedChanged += new System.EventHandler(this.chkManaTide2_CheckedChanged);
            // 
            // chkWaterShield2
            // 
            this.chkWaterShield2.AutoSize = true;
            this.chkWaterShield2.Location = new System.Drawing.Point(6, 51);
            this.chkWaterShield2.Name = "chkWaterShield2";
            this.chkWaterShield2.Size = new System.Drawing.Size(129, 17);
            this.chkWaterShield2.TabIndex = 26;
            this.chkWaterShield2.Text = "Glyph of Water Shield";
            this.tipRestoSham.SetToolTip(this.chkWaterShield2, "Check if you have the Glyph of Water Shield");
            this.chkWaterShield2.UseVisualStyleBackColor = true;
            this.chkWaterShield2.CheckedChanged += new System.EventHandler(this.chkWaterShield2_CheckedChanged);
            // 
            // chkWaterShield3
            // 
            this.chkWaterShield3.AutoSize = true;
            this.chkWaterShield3.Location = new System.Drawing.Point(6, 74);
            this.chkWaterShield3.Name = "chkWaterShield3";
            this.chkWaterShield3.Size = new System.Drawing.Size(137, 17);
            this.chkWaterShield3.TabIndex = 27;
            this.chkWaterShield3.Text = "Glyph of Water Mastery";
            this.tipRestoSham.SetToolTip(this.chkWaterShield3, "Check if you have the Glyph of Water Mastery");
            this.chkWaterShield3.UseVisualStyleBackColor = true;
            this.chkWaterShield3.CheckedChanged += new System.EventHandler(this.chkWaterShield3_CheckedChanged);
            // 
            // errorRestoSham
            // 
            this.errorRestoSham.ContainerControl = this;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.txtFightLength);
            this.tabPage1.Controls.Add(this.chkMT);
            this.tabPage1.Controls.Add(this.groupBox3);
            this.tabPage1.Controls.Add(this.chkWaterShield);
            this.tabPage1.Controls.Add(this.chkManaTide);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.txtOutsideFSR);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.cboManaPotAmount);
            this.tabPage1.Controls.Add(this.label7);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(204, 501);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "General";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(146, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(23, 13);
            this.label2.TabIndex = 26;
            this.label2.Text = "min";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label17);
            this.groupBox3.Controls.Add(this.txtESInterval);
            this.groupBox3.Controls.Add(this.label18);
            this.groupBox3.Location = new System.Drawing.Point(6, 131);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(192, 40);
            this.groupBox3.TabIndex = 19;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Earth Shield";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(6, 16);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(57, 13);
            this.label17.TabIndex = 10;
            this.label17.Text = "Cast every";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(118, 16);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(24, 13);
            this.label18.TabIndex = 12;
            this.label18.Text = "sec";
            // 
            // chkWaterShield
            // 
            this.chkWaterShield.AutoSize = true;
            this.chkWaterShield.Location = new System.Drawing.Point(6, 108);
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
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 35);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(70, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Outside FSR:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 61);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(75, 13);
            this.label7.TabIndex = 9;
            this.label7.Text = "Mana Potions:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(146, 35);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(15, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "%";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(212, 527);
            this.tabControl1.TabIndex = 14;
            // 
            // tabPage2
            // 
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
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
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
        private System.Windows.Forms.TextBox txtOutsideFSR;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cboManaPotAmount;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox3;
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

    }
}
