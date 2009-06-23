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
            this.chkMT = new System.Windows.Forms.CheckBox();
            this.label17 = new System.Windows.Forms.Label();
            this.txtESInterval = new System.Windows.Forms.TextBox();
            this.cboHealingStyle = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtFightLength = new System.Windows.Forms.TextBox();
            this.errorRestoSham = new System.Windows.Forms.ErrorProvider(this.components);
            this.GeneralPage = new System.Windows.Forms.TabPage();
            this.label3 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tbOverhealing_Label = new System.Windows.Forms.Label();
            this.tbOverhealing = new System.Windows.Forms.TrackBar();
            this.tbBurst_Label = new System.Windows.Forms.Label();
            this.tbBurst = new System.Windows.Forms.TrackBar();
            this.chkWaterShield = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            ((System.ComponentModel.ISupportInitialize)(this.errorRestoSham)).BeginInit();
            this.GeneralPage.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbOverhealing)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbBurst)).BeginInit();
            this.tabControl1.SuspendLayout();
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
            this.chkManaTide.Location = new System.Drawing.Point(9, 111);
            this.chkManaTide.Name = "chkManaTide";
            this.chkManaTide.Size = new System.Drawing.Size(184, 17);
            this.chkManaTide.TabIndex = 14;
            this.chkManaTide.Text = "Mana Tide totem every cooldown";
            this.tipRestoSham.SetToolTip(this.chkManaTide, "Check to indicate you place a Mana Tide totem every time the cooldown is up");
            this.chkManaTide.UseVisualStyleBackColor = true;
            this.chkManaTide.CheckedChanged += new System.EventHandler(this.chkManaTide_CheckedChanged);
            // 
            // chkMT
            // 
            this.chkMT.AutoSize = true;
            this.chkMT.Location = new System.Drawing.Point(9, 88);
            this.chkMT.Name = "chkMT";
            this.chkMT.Size = new System.Drawing.Size(172, 17);
            this.chkMT.TabIndex = 24;
            this.chkMT.Text = "Are you healing the MT or OT?";
            this.tipRestoSham.SetToolTip(this.chkMT, "Check to indicate you healing a single target exclusively and not worried about s" +
                    "plash healing.");
            this.chkMT.UseVisualStyleBackColor = true;
            this.chkMT.CheckedChanged += new System.EventHandler(this.chkMT_CheckedChanged);
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(6, 154);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(99, 13);
            this.label17.TabIndex = 10;
            this.label17.Text = "Earthshield Recast:";
            this.tipRestoSham.SetToolTip(this.label17, "Set to 0 to not use.");
            // 
            // txtESInterval
            // 
            this.txtESInterval.Location = new System.Drawing.Point(111, 151);
            this.txtESInterval.Name = "txtESInterval";
            this.txtESInterval.Size = new System.Drawing.Size(43, 20);
            this.txtESInterval.TabIndex = 11;
            this.tipRestoSham.SetToolTip(this.txtESInterval, "How often you refresh Earth Shield, in seconds (enter 0 if you don\'t use Earth Sh" +
                    "ield)");
            // 
            // cboHealingStyle
            // 
            this.cboHealingStyle.FormattingEnabled = true;
            this.cboHealingStyle.Items.AddRange(new object[] {
            "RT+HW",
            "RT+LHW",
            "RT+CH",
            "HW Spam",
            "LHW Spam",
            "CH Spam"});
            this.cboHealingStyle.Location = new System.Drawing.Point(81, 59);
            this.cboHealingStyle.Name = "cboHealingStyle";
            this.cboHealingStyle.Size = new System.Drawing.Size(75, 21);
            this.cboHealingStyle.TabIndex = 28;
            this.tipRestoSham.SetToolTip(this.cboHealingStyle, "Choose the style of healing that fits you.");
            this.cboHealingStyle.TextChanged += new System.EventHandler(this.cboHealingStyle_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(141, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 13);
            this.label2.TabIndex = 26;
            this.label2.Text = "min of active time";
            this.tipRestoSham.SetToolTip(this.label2, "Generally pulled from Wow Web Stats.");
            this.label2.Click += new System.EventHandler(this.label2_Click);
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
            // errorRestoSham
            // 
            this.errorRestoSham.ContainerControl = this;
            // 
            // GeneralPage
            // 
            this.GeneralPage.Controls.Add(this.label3);
            this.GeneralPage.Controls.Add(this.cboHealingStyle);
            this.GeneralPage.Controls.Add(this.label18);
            this.GeneralPage.Controls.Add(this.txtESInterval);
            this.GeneralPage.Controls.Add(this.txtFightLength);
            this.GeneralPage.Controls.Add(this.label17);
            this.GeneralPage.Controls.Add(this.groupBox1);
            this.GeneralPage.Controls.Add(this.chkMT);
            this.GeneralPage.Controls.Add(this.chkWaterShield);
            this.GeneralPage.Controls.Add(this.chkManaTide);
            this.GeneralPage.Controls.Add(this.cboManaPotAmount);
            this.GeneralPage.Controls.Add(this.label7);
            this.GeneralPage.Controls.Add(this.label2);
            this.GeneralPage.Controls.Add(this.label1);
            this.GeneralPage.Location = new System.Drawing.Point(4, 22);
            this.GeneralPage.Name = "GeneralPage";
            this.GeneralPage.Padding = new System.Windows.Forms.Padding(3);
            this.GeneralPage.Size = new System.Drawing.Size(292, 579);
            this.GeneralPage.TabIndex = 0;
            this.GeneralPage.Text = "General";
            this.GeneralPage.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 62);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 13);
            this.label3.TabIndex = 29;
            this.label3.Text = "Healing Style:";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(160, 154);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(24, 13);
            this.label18.TabIndex = 12;
            this.label18.Text = "sec";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tbOverhealing_Label);
            this.groupBox1.Controls.Add(this.tbOverhealing);
            this.groupBox1.Controls.Add(this.tbBurst_Label);
            this.groupBox1.Controls.Add(this.tbBurst);
            this.groupBox1.Location = new System.Drawing.Point(6, 177);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(280, 153);
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
            this.tbOverhealing.Size = new System.Drawing.Size(268, 45);
            this.tbOverhealing.TabIndex = 4;
            this.tbOverhealing.Scroll += new System.EventHandler(this.OnTrackBarScroll);
            // 
            // tbBurst_Label
            // 
            this.tbBurst_Label.AutoSize = true;
            this.tbBurst_Label.Location = new System.Drawing.Point(7, 20);
            this.tbBurst_Label.Name = "tbBurst_Label";
            this.tbBurst_Label.Size = new System.Drawing.Size(133, 13);
            this.tbBurst_Label.TabIndex = 1;
            this.tbBurst_Label.Text = "Replenishment Uptime (%):";
            // 
            // tbBurst
            // 
            this.tbBurst.Location = new System.Drawing.Point(7, 36);
            this.tbBurst.Maximum = 100;
            this.tbBurst.Name = "tbBurst";
            this.tbBurst.Size = new System.Drawing.Size(267, 45);
            this.tbBurst.TabIndex = 0;
            this.tbBurst.Scroll += new System.EventHandler(this.OnTrackBarScroll);
            // 
            // chkWaterShield
            // 
            this.chkWaterShield.AutoSize = true;
            this.chkWaterShield.Location = new System.Drawing.Point(9, 134);
            this.chkWaterShield.Name = "chkWaterShield";
            this.chkWaterShield.Size = new System.Drawing.Size(87, 17);
            this.chkWaterShield.TabIndex = 15;
            this.chkWaterShield.Text = "Water Shield";
            this.chkWaterShield.UseVisualStyleBackColor = true;
            this.chkWaterShield.CheckedChanged += new System.EventHandler(this.chkWaterShield_CheckedChanged);
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
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Fight Length:";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.GeneralPage);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(300, 605);
            this.tabControl1.TabIndex = 14;
            this.tabControl1.Tag = "";
            // 
            // CalculationOptionsPanelRestoSham
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Name = "CalculationOptionsPanelRestoSham";
            this.Size = new System.Drawing.Size(300, 605);
            ((System.ComponentModel.ISupportInitialize)(this.errorRestoSham)).EndInit();
            this.GeneralPage.ResumeLayout(false);
            this.GeneralPage.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbOverhealing)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbBurst)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolTip tipRestoSham;
        private System.Windows.Forms.ErrorProvider errorRestoSham;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage GeneralPage;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cboHealingStyle;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TextBox txtESInterval;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label tbOverhealing_Label;
        private System.Windows.Forms.TrackBar tbOverhealing;
        private System.Windows.Forms.Label tbBurst_Label;
        private System.Windows.Forms.TrackBar tbBurst;
        private System.Windows.Forms.CheckBox chkMT;
        private System.Windows.Forms.CheckBox chkWaterShield;
        private System.Windows.Forms.CheckBox chkManaTide;
        private System.Windows.Forms.ComboBox cboManaPotAmount;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtFightLength;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;

    }
}
