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
            this.cboBurstStyle = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtFightLength = new System.Windows.Forms.TextBox();
            this.cboSustStyle = new System.Windows.Forms.ComboBox();
            this.txtSurvivalPerc = new System.Windows.Forms.TextBox();
            this.cboExtraManaTide = new System.Windows.Forms.ComboBox();
            this.cboDivineHymn = new System.Windows.Forms.ComboBox();
            this.cboInnervate = new System.Windows.Forms.ComboBox();
            this.cboHeroism = new System.Windows.Forms.ComboBox();
            this.txtCleanse = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.errorRestoSham = new System.Windows.Forms.ErrorProvider(this.components);
            this.GeneralPage = new System.Windows.Forms.TabPage();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
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
            this.cboManaPotAmount.Location = new System.Drawing.Point(81, 110);
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
            this.chkManaTide.Location = new System.Drawing.Point(81, 322);
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
            this.chkMT.Location = new System.Drawing.Point(81, 299);
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
            this.label17.Location = new System.Drawing.Point(20, 61);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(61, 13);
            this.label17.TabIndex = 10;
            this.label17.Text = "ES Recast:";
            this.tipRestoSham.SetToolTip(this.label17, "Set to 0 to not use.");
            // 
            // txtESInterval
            // 
            this.txtESInterval.Location = new System.Drawing.Point(81, 58);
            this.txtESInterval.Name = "txtESInterval";
            this.txtESInterval.Size = new System.Drawing.Size(75, 20);
            this.txtESInterval.TabIndex = 11;
            this.tipRestoSham.SetToolTip(this.txtESInterval, "How often you refresh Earth Shield, in seconds (enter 0 if you don\'t use Earth Sh" +
                    "ield)");
            // 
            // cboBurstStyle
            // 
            this.cboBurstStyle.FormattingEnabled = true;
            this.cboBurstStyle.Items.AddRange(new object[] {
            "RT+HW",
            "RT+LHW",
            "RT+CH",
            "HW Spam",
            "LHW Spam",
            "CH Spam",
            "Auto"});
            this.cboBurstStyle.Location = new System.Drawing.Point(81, 137);
            this.cboBurstStyle.Name = "cboBurstStyle";
            this.cboBurstStyle.Size = new System.Drawing.Size(75, 21);
            this.cboBurstStyle.TabIndex = 28;
            this.tipRestoSham.SetToolTip(this.cboBurstStyle, "Choose the style of burst healing you do, or leave on Auto.");
            this.cboBurstStyle.TextChanged += new System.EventHandler(this.cboBurstStyle_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(157, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 13);
            this.label2.TabIndex = 26;
            this.label2.Text = "Minutes";
            this.tipRestoSham.SetToolTip(this.label2, "Generally pulled from Wow Web Stats.");
            // 
            // txtFightLength
            // 
            this.txtFightLength.Location = new System.Drawing.Point(81, 6);
            this.txtFightLength.Name = "txtFightLength";
            this.txtFightLength.Size = new System.Drawing.Size(75, 20);
            this.txtFightLength.TabIndex = 25;
            this.tipRestoSham.SetToolTip(this.txtFightLength, "Total length of Fight in Minutes.");
            this.txtFightLength.Validated += new System.EventHandler(this.numericTextBox_Validated);
            this.txtFightLength.Validating += new System.ComponentModel.CancelEventHandler(this.numericTextBox_Validating);
            // 
            // cboSustStyle
            // 
            this.cboSustStyle.FormattingEnabled = true;
            this.cboSustStyle.Items.AddRange(new object[] {
            "RT+HW",
            "RT+LHW",
            "RT+CH",
            "HW Spam",
            "LHW Spam",
            "CH Spam",
            "Auto"});
            this.cboSustStyle.Location = new System.Drawing.Point(81, 164);
            this.cboSustStyle.Name = "cboSustStyle";
            this.cboSustStyle.Size = new System.Drawing.Size(75, 21);
            this.cboSustStyle.TabIndex = 31;
            this.tipRestoSham.SetToolTip(this.cboSustStyle, "Choose the style of sustained healing you do, or leave on Auto.");
            this.cboSustStyle.TextChanged += new System.EventHandler(this.cboSustStyle_TextChanged);
            // 
            // txtSurvivalPerc
            // 
            this.txtSurvivalPerc.Location = new System.Drawing.Point(81, 32);
            this.txtSurvivalPerc.Name = "txtSurvivalPerc";
            this.txtSurvivalPerc.Size = new System.Drawing.Size(75, 20);
            this.txtSurvivalPerc.TabIndex = 36;
            this.tipRestoSham.SetToolTip(this.txtSurvivalPerc, "% of how useful Survival points are to you.  Default 2%.");
            // 
            // cboExtraManaTide
            // 
            this.cboExtraManaTide.FormattingEnabled = true;
            this.cboExtraManaTide.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "3",
            "4"});
            this.cboExtraManaTide.Location = new System.Drawing.Point(81, 191);
            this.cboExtraManaTide.Name = "cboExtraManaTide";
            this.cboExtraManaTide.Size = new System.Drawing.Size(75, 21);
            this.cboExtraManaTide.TabIndex = 41;
            this.tipRestoSham.SetToolTip(this.cboExtraManaTide, "Other shamans that will use Mana tide in your group.");
            this.cboExtraManaTide.TextChanged += new System.EventHandler(this.cboBurstStyle_TextChanged);
            // 
            // cboDivineHymn
            // 
            this.cboDivineHymn.FormattingEnabled = true;
            this.cboDivineHymn.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4"});
            this.cboDivineHymn.Location = new System.Drawing.Point(81, 218);
            this.cboDivineHymn.Name = "cboDivineHymn";
            this.cboDivineHymn.Size = new System.Drawing.Size(75, 21);
            this.cboDivineHymn.TabIndex = 43;
            this.tipRestoSham.SetToolTip(this.cboDivineHymn, "Number of Priests in your group that will use Hymn of Hope");
            this.cboDivineHymn.TextChanged += new System.EventHandler(this.cboDivineHymn_TextChanged);
            // 
            // cboInnervate
            // 
            this.cboInnervate.FormattingEnabled = true;
            this.cboInnervate.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4"});
            this.cboInnervate.Location = new System.Drawing.Point(81, 245);
            this.cboInnervate.Name = "cboInnervate";
            this.cboInnervate.Size = new System.Drawing.Size(75, 21);
            this.cboInnervate.TabIndex = 45;
            this.tipRestoSham.SetToolTip(this.cboInnervate, "Number of Innervates that will be used on you.");
            this.cboInnervate.TextChanged += new System.EventHandler(this.cboInnervate_TextChanged);
            // 
            // cboHeroism
            // 
            this.cboHeroism.FormattingEnabled = true;
            this.cboHeroism.Items.AddRange(new object[] {
            "No",
            "Yes",
            "Me"});
            this.cboHeroism.Location = new System.Drawing.Point(81, 272);
            this.cboHeroism.Name = "cboHeroism";
            this.cboHeroism.Size = new System.Drawing.Size(75, 21);
            this.cboHeroism.TabIndex = 47;
            this.tipRestoSham.SetToolTip(this.cboHeroism, "Note, if you cast heroism, select \"me\".");
            this.cboHeroism.TextChanged += new System.EventHandler(this.cboHeroism_TextChanged);
            // 
            // txtCleanse
            // 
            this.txtCleanse.Location = new System.Drawing.Point(81, 84);
            this.txtCleanse.Name = "txtCleanse";
            this.txtCleanse.Size = new System.Drawing.Size(75, 20);
            this.txtCleanse.TabIndex = 49;
            this.tipRestoSham.SetToolTip(this.txtCleanse, "The number of times per fight you use Cleanse Spirit.");
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(28, 87);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(52, 13);
            this.label16.TabIndex = 48;
            this.label16.Text = "Decurses";
            this.tipRestoSham.SetToolTip(this.label16, "The number of times per fight you use Cleanse Spirit.");
            // 
            // errorRestoSham
            // 
            this.errorRestoSham.ContainerControl = this;
            // 
            // GeneralPage
            // 
            this.GeneralPage.AutoScroll = true;
            this.GeneralPage.Controls.Add(this.txtCleanse);
            this.GeneralPage.Controls.Add(this.label16);
            this.GeneralPage.Controls.Add(this.cboHeroism);
            this.GeneralPage.Controls.Add(this.label14);
            this.GeneralPage.Controls.Add(this.cboInnervate);
            this.GeneralPage.Controls.Add(this.label13);
            this.GeneralPage.Controls.Add(this.cboDivineHymn);
            this.GeneralPage.Controls.Add(this.label12);
            this.GeneralPage.Controls.Add(this.cboExtraManaTide);
            this.GeneralPage.Controls.Add(this.label11);
            this.GeneralPage.Controls.Add(this.label8);
            this.GeneralPage.Controls.Add(this.txtSurvivalPerc);
            this.GeneralPage.Controls.Add(this.label9);
            this.GeneralPage.Controls.Add(this.cboSustStyle);
            this.GeneralPage.Controls.Add(this.label4);
            this.GeneralPage.Controls.Add(this.label3);
            this.GeneralPage.Controls.Add(this.cboBurstStyle);
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
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(27, 275);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(48, 13);
            this.label14.TabIndex = 46;
            this.label14.Text = "Heroism:";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(26, 248);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(55, 13);
            this.label13.TabIndex = 44;
            this.label13.Text = "Innervate:";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(3, 221);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(78, 13);
            this.label12.TabIndex = 42;
            this.label12.Text = "Hymn of Hope:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(14, 194);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(67, 13);
            this.label11.TabIndex = 40;
            this.label11.Text = "+Mana-Tide:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(157, 35);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(15, 13);
            this.label8.TabIndex = 37;
            this.label8.Text = "%";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(9, 35);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(72, 13);
            this.label9.TabIndex = 35;
            this.label9.Text = "Surv. Weight:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(21, 167);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 13);
            this.label4.TabIndex = 30;
            this.label4.Text = "Sust. Style:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(21, 140);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 13);
            this.label3.TabIndex = 29;
            this.label3.Text = "Burst Style:";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(157, 61);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(49, 13);
            this.label18.TabIndex = 12;
            this.label18.Text = "Seconds";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tbOverhealing_Label);
            this.groupBox1.Controls.Add(this.tbOverhealing);
            this.groupBox1.Controls.Add(this.tbBurst_Label);
            this.groupBox1.Controls.Add(this.tbBurst);
            this.groupBox1.Location = new System.Drawing.Point(6, 368);
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
            this.chkWaterShield.Location = new System.Drawing.Point(81, 345);
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
            this.label7.Location = new System.Drawing.Point(6, 113);
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
        private System.Windows.Forms.ComboBox cboBurstStyle;
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
        private System.Windows.Forms.ComboBox cboSustStyle;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cboExtraManaTide;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtSurvivalPerc;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox cboInnervate;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.ComboBox cboDivineHymn;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.ComboBox cboHeroism;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox txtCleanse;
        private System.Windows.Forms.Label label16;

    }
}
