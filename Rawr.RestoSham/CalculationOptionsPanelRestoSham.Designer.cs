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
      this.label1 = new System.Windows.Forms.Label();
      this.txtFightLength = new System.Windows.Forms.TextBox();
      this.label2 = new System.Windows.Forms.Label();
      this.label3 = new System.Windows.Forms.Label();
      this.txtOutsideFSR = new System.Windows.Forms.TextBox();
      this.label4 = new System.Windows.Forms.Label();
      this.txtSPriestMp5 = new System.Windows.Forms.TextBox();
      this.label5 = new System.Windows.Forms.Label();
      this.label6 = new System.Windows.Forms.Label();
      this.label7 = new System.Windows.Forms.Label();
      this.cboManaPotAmount = new System.Windows.Forms.ComboBox();
      this.label8 = new System.Windows.Forms.Label();
      this.label9 = new System.Windows.Forms.Label();
      this.txtManaPotInterval = new System.Windows.Forms.TextBox();
      this.tabControl1 = new System.Windows.Forms.TabControl();
      this.tabPage1 = new System.Windows.Forms.TabPage();
      this.radioScryers = new System.Windows.Forms.RadioButton();
      this.radioAldor = new System.Windows.Forms.RadioButton();
      this.chkExalted = new System.Windows.Forms.CheckBox();
      this.chkWaterShield = new System.Windows.Forms.CheckBox();
      this.chkManaTide = new System.Windows.Forms.CheckBox();
      this.tabPage2 = new System.Windows.Forms.TabPage();
      this.groupBox3 = new System.Windows.Forms.GroupBox();
      this.cboESRank = new System.Windows.Forms.ComboBox();
      this.label16 = new System.Windows.Forms.Label();
      this.label17 = new System.Windows.Forms.Label();
      this.txtESInterval = new System.Windows.Forms.TextBox();
      this.label18 = new System.Windows.Forms.Label();
      this.groupBox2 = new System.Windows.Forms.GroupBox();
      this.lblCHMinPct = new System.Windows.Forms.Label();
      this.lblCHMaxPct = new System.Windows.Forms.Label();
      this.trkCH = new System.Windows.Forms.TrackBar();
      this.cboCHMinRank = new System.Windows.Forms.ComboBox();
      this.cboCHMaxRank = new System.Windows.Forms.ComboBox();
      this.label22 = new System.Windows.Forms.Label();
      this.lblHWMinPct = new System.Windows.Forms.Label();
      this.lblHWMaxPct = new System.Windows.Forms.Label();
      this.trkHW = new System.Windows.Forms.TrackBar();
      this.cboHWMinRank = new System.Windows.Forms.ComboBox();
      this.cboHWMaxRank = new System.Windows.Forms.ComboBox();
      this.label19 = new System.Windows.Forms.Label();
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.label21 = new System.Windows.Forms.Label();
      this.cboNumCHTargets = new System.Windows.Forms.ComboBox();
      this.label20 = new System.Windows.Forms.Label();
      this.label15 = new System.Windows.Forms.Label();
      this.label14 = new System.Windows.Forms.Label();
      this.label13 = new System.Windows.Forms.Label();
      this.txtCHRatio = new System.Windows.Forms.TextBox();
      this.txtLHWRatio = new System.Windows.Forms.TextBox();
      this.txtHWRatio = new System.Windows.Forms.TextBox();
      this.label12 = new System.Windows.Forms.Label();
      this.label11 = new System.Windows.Forms.Label();
      this.label10 = new System.Windows.Forms.Label();
      this.tabPage3 = new System.Windows.Forms.TabPage();
      this.lblCHTotemDesc = new System.Windows.Forms.Label();
      this.lblLHWTotemDesc = new System.Windows.Forms.Label();
      this.lblHWTotemDesc = new System.Windows.Forms.Label();
      this.chkEquipTotems = new System.Windows.Forms.CheckBox();
      this.cboCHTotem = new System.Windows.Forms.ComboBox();
      this.cboLHWTotem = new System.Windows.Forms.ComboBox();
      this.cboHWTotem = new System.Windows.Forms.ComboBox();
      this.label27 = new System.Windows.Forms.Label();
      this.label26 = new System.Windows.Forms.Label();
      this.label25 = new System.Windows.Forms.Label();
      this.tipRestoSham = new System.Windows.Forms.ToolTip(this.components);
      this.errorRestoSham = new System.Windows.Forms.ErrorProvider(this.components);
      this.tabControl1.SuspendLayout();
      this.tabPage1.SuspendLayout();
      this.tabPage2.SuspendLayout();
      this.groupBox3.SuspendLayout();
      this.groupBox2.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.trkCH)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.trkHW)).BeginInit();
      this.groupBox1.SuspendLayout();
      this.tabPage3.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.errorRestoSham)).BeginInit();
      this.SuspendLayout();
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(7, 9);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(69, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Fight Length:";
      // 
      // txtFightLength
      // 
      this.txtFightLength.Location = new System.Drawing.Point(81, 6);
      this.txtFightLength.Name = "txtFightLength";
      this.txtFightLength.Size = new System.Drawing.Size(59, 20);
      this.txtFightLength.TabIndex = 1;
      this.tipRestoSham.SetToolTip(this.txtFightLength, "The length of the fight, in minutes");
      this.txtFightLength.Validated += new System.EventHandler(this.numericTextBox_Validated);
      this.txtFightLength.Validating += new System.ComponentModel.CancelEventHandler(this.numericTextBox_Validating);
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(146, 9);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(23, 13);
      this.label2.TabIndex = 2;
      this.label2.Text = "min";
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(5, 35);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(70, 13);
      this.label3.TabIndex = 3;
      this.label3.Text = "Outside FSR:";
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
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(146, 35);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(15, 13);
      this.label4.TabIndex = 5;
      this.label4.Text = "%";
      // 
      // txtSPriestMp5
      // 
      this.txtSPriestMp5.Location = new System.Drawing.Point(81, 58);
      this.txtSPriestMp5.Name = "txtSPriestMp5";
      this.txtSPriestMp5.Size = new System.Drawing.Size(59, 20);
      this.txtSPriestMp5.TabIndex = 6;
      this.tipRestoSham.SetToolTip(this.txtSPriestMp5, "Mana regen obtained from a Shadowpriest\'s Vampiric Touch");
      this.txtSPriestMp5.Validated += new System.EventHandler(this.numericTextBox_Validated);
      this.txtSPriestMp5.Validating += new System.ComponentModel.CancelEventHandler(this.numericTextBox_Validating);
      // 
      // label5
      // 
      this.label5.AutoSize = true;
      this.label5.Location = new System.Drawing.Point(2, 61);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(74, 13);
      this.label5.TabIndex = 7;
      this.label5.Text = "Shadowpriest:";
      // 
      // label6
      // 
      this.label6.AutoSize = true;
      this.label6.Location = new System.Drawing.Point(146, 61);
      this.label6.Name = "label6";
      this.label6.Size = new System.Drawing.Size(27, 13);
      this.label6.TabIndex = 8;
      this.label6.Text = "mp5";
      // 
      // label7
      // 
      this.label7.AutoSize = true;
      this.label7.Location = new System.Drawing.Point(1, 92);
      this.label7.Name = "label7";
      this.label7.Size = new System.Drawing.Size(75, 13);
      this.label7.TabIndex = 9;
      this.label7.Text = "Mana Potions:";
      // 
      // cboManaPotAmount
      // 
      this.cboManaPotAmount.FormattingEnabled = true;
      this.cboManaPotAmount.Items.AddRange(new object[] {
            "0",
            "1500",
            "1800",
            "2400"});
      this.cboManaPotAmount.Location = new System.Drawing.Point(4, 108);
      this.cboManaPotAmount.Name = "cboManaPotAmount";
      this.cboManaPotAmount.Size = new System.Drawing.Size(75, 21);
      this.cboManaPotAmount.TabIndex = 10;
      this.tipRestoSham.SetToolTip(this.cboManaPotAmount, "The average amount of mana restored by a mana potion");
      this.cboManaPotAmount.Validating += new System.ComponentModel.CancelEventHandler(this.numericTextBox_Validating);
      this.cboManaPotAmount.Validated += new System.EventHandler(this.numericTextBox_Validated);
      // 
      // label8
      // 
      this.label8.AutoSize = true;
      this.label8.Location = new System.Drawing.Point(85, 111);
      this.label8.Name = "label8";
      this.label8.Size = new System.Drawing.Size(33, 13);
      this.label8.TabIndex = 11;
      this.label8.Text = "every";
      // 
      // label9
      // 
      this.label9.AutoSize = true;
      this.label9.Location = new System.Drawing.Point(175, 111);
      this.label9.Name = "label9";
      this.label9.Size = new System.Drawing.Size(23, 13);
      this.label9.TabIndex = 12;
      this.label9.Text = "min";
      // 
      // txtManaPotInterval
      // 
      this.txtManaPotInterval.Location = new System.Drawing.Point(124, 108);
      this.txtManaPotInterval.Name = "txtManaPotInterval";
      this.txtManaPotInterval.Size = new System.Drawing.Size(45, 20);
      this.txtManaPotInterval.TabIndex = 13;
      this.tipRestoSham.SetToolTip(this.txtManaPotInterval, "How often you use a mana potion");
      this.txtManaPotInterval.Validated += new System.EventHandler(this.numericTextBox_Validated);
      this.txtManaPotInterval.Validating += new System.ComponentModel.CancelEventHandler(this.numericTextBox_Validating);
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
      // 
      // tabPage1
      // 
      this.tabPage1.Controls.Add(this.radioScryers);
      this.tabPage1.Controls.Add(this.radioAldor);
      this.tabPage1.Controls.Add(this.chkExalted);
      this.tabPage1.Controls.Add(this.chkWaterShield);
      this.tabPage1.Controls.Add(this.chkManaTide);
      this.tabPage1.Controls.Add(this.label1);
      this.tabPage1.Controls.Add(this.txtManaPotInterval);
      this.tabPage1.Controls.Add(this.txtFightLength);
      this.tabPage1.Controls.Add(this.label9);
      this.tabPage1.Controls.Add(this.label2);
      this.tabPage1.Controls.Add(this.label8);
      this.tabPage1.Controls.Add(this.label3);
      this.tabPage1.Controls.Add(this.cboManaPotAmount);
      this.tabPage1.Controls.Add(this.txtOutsideFSR);
      this.tabPage1.Controls.Add(this.label7);
      this.tabPage1.Controls.Add(this.label4);
      this.tabPage1.Controls.Add(this.label6);
      this.tabPage1.Controls.Add(this.txtSPriestMp5);
      this.tabPage1.Controls.Add(this.label5);
      this.tabPage1.Location = new System.Drawing.Point(4, 22);
      this.tabPage1.Name = "tabPage1";
      this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
      this.tabPage1.Size = new System.Drawing.Size(204, 501);
      this.tabPage1.TabIndex = 0;
      this.tabPage1.Text = "General";
      this.tabPage1.UseVisualStyleBackColor = true;
      // 
      // radioScryers
      // 
      this.radioScryers.AutoSize = true;
      this.radioScryers.Enabled = false;
      this.radioScryers.Location = new System.Drawing.Point(88, 213);
      this.radioScryers.Name = "radioScryers";
      this.radioScryers.Size = new System.Drawing.Size(60, 17);
      this.radioScryers.TabIndex = 18;
      this.radioScryers.TabStop = true;
      this.radioScryers.Text = "Scryers";
      this.tipRestoSham.SetToolTip(this.radioScryers, "Select if you are exalted with the Scryers");
      this.radioScryers.UseVisualStyleBackColor = true;
      this.radioScryers.CheckedChanged += new System.EventHandler(this.radioAldor_CheckedChanged);
      // 
      // radioAldor
      // 
      this.radioAldor.AutoSize = true;
      this.radioAldor.Checked = true;
      this.radioAldor.Enabled = false;
      this.radioAldor.Location = new System.Drawing.Point(88, 190);
      this.radioAldor.Name = "radioAldor";
      this.radioAldor.Size = new System.Drawing.Size(49, 17);
      this.radioAldor.TabIndex = 17;
      this.radioAldor.TabStop = true;
      this.radioAldor.Text = "Aldor";
      this.tipRestoSham.SetToolTip(this.radioAldor, "Select if you are exalted with the Aldor");
      this.radioAldor.UseVisualStyleBackColor = true;
      this.radioAldor.CheckedChanged += new System.EventHandler(this.radioAldor_CheckedChanged);
      // 
      // chkExalted
      // 
      this.chkExalted.AutoSize = true;
      this.chkExalted.Location = new System.Drawing.Point(4, 191);
      this.chkExalted.Name = "chkExalted";
      this.chkExalted.Size = new System.Drawing.Size(83, 17);
      this.chkExalted.TabIndex = 16;
      this.chkExalted.Text = "Exalted with";
      this.tipRestoSham.SetToolTip(this.chkExalted, "Check this box if you have obtained exalted status with either the Aldor or the S" +
              "cryers");
      this.chkExalted.UseVisualStyleBackColor = true;
      this.chkExalted.CheckedChanged += new System.EventHandler(this.chkExalted_CheckedChanged);
      // 
      // chkWaterShield
      // 
      this.chkWaterShield.AutoSize = true;
      this.chkWaterShield.Location = new System.Drawing.Point(4, 168);
      this.chkWaterShield.Name = "chkWaterShield";
      this.chkWaterShield.Size = new System.Drawing.Size(87, 17);
      this.chkWaterShield.TabIndex = 15;
      this.chkWaterShield.Text = "Water Shield";
      this.chkWaterShield.UseVisualStyleBackColor = true;
      this.chkWaterShield.CheckedChanged += new System.EventHandler(this.chkWaterShield_CheckedChanged);
      // 
      // chkManaTide
      // 
      this.chkManaTide.AutoSize = true;
      this.chkManaTide.Location = new System.Drawing.Point(4, 144);
      this.chkManaTide.Name = "chkManaTide";
      this.chkManaTide.Size = new System.Drawing.Size(184, 17);
      this.chkManaTide.TabIndex = 14;
      this.chkManaTide.Text = "Mana Tide totem every cooldown";
      this.tipRestoSham.SetToolTip(this.chkManaTide, "Check to indicate you place a Mana Tide totem every time the cooldown is up");
      this.chkManaTide.UseVisualStyleBackColor = true;
      this.chkManaTide.CheckedChanged += new System.EventHandler(this.chkManaTide_CheckedChanged);
      // 
      // tabPage2
      // 
      this.tabPage2.Controls.Add(this.groupBox3);
      this.tabPage2.Controls.Add(this.groupBox2);
      this.tabPage2.Controls.Add(this.groupBox1);
      this.tabPage2.Location = new System.Drawing.Point(4, 22);
      this.tabPage2.Name = "tabPage2";
      this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
      this.tabPage2.Size = new System.Drawing.Size(204, 501);
      this.tabPage2.TabIndex = 1;
      this.tabPage2.Text = "Spells";
      this.tabPage2.UseVisualStyleBackColor = true;
      // 
      // groupBox3
      // 
      this.groupBox3.Controls.Add(this.cboESRank);
      this.groupBox3.Controls.Add(this.label16);
      this.groupBox3.Controls.Add(this.label17);
      this.groupBox3.Controls.Add(this.txtESInterval);
      this.groupBox3.Controls.Add(this.label18);
      this.groupBox3.Location = new System.Drawing.Point(3, 141);
      this.groupBox3.Name = "groupBox3";
      this.groupBox3.Size = new System.Drawing.Size(198, 75);
      this.groupBox3.TabIndex = 16;
      this.groupBox3.TabStop = false;
      this.groupBox3.Text = "Earth Shield";
      // 
      // cboESRank
      // 
      this.cboESRank.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cboESRank.FormattingEnabled = true;
      this.cboESRank.Location = new System.Drawing.Point(90, 45);
      this.cboESRank.Name = "cboESRank";
      this.cboESRank.Size = new System.Drawing.Size(36, 21);
      this.cboESRank.TabIndex = 13;
      this.cboESRank.SelectedIndexChanged += new System.EventHandler(this.cboESRank_SelectedIndexChanged);
      // 
      // label16
      // 
      this.label16.AutoSize = true;
      this.label16.Location = new System.Drawing.Point(20, 48);
      this.label16.Name = "label16";
      this.label16.Size = new System.Drawing.Size(64, 13);
      this.label16.TabIndex = 9;
      this.label16.Text = "Rank Used:";
      // 
      // label17
      // 
      this.label17.AutoSize = true;
      this.label17.Location = new System.Drawing.Point(27, 22);
      this.label17.Name = "label17";
      this.label17.Size = new System.Drawing.Size(57, 13);
      this.label17.TabIndex = 10;
      this.label17.Text = "Cast every";
      // 
      // txtESInterval
      // 
      this.txtESInterval.Location = new System.Drawing.Point(90, 19);
      this.txtESInterval.Name = "txtESInterval";
      this.txtESInterval.Size = new System.Drawing.Size(43, 20);
      this.txtESInterval.TabIndex = 11;
      this.tipRestoSham.SetToolTip(this.txtESInterval, "How often you refresh Earth Shield, in seconds (enter 0 if you don\'t use Earth Sh" +
              "ield)");
      this.txtESInterval.Validated += new System.EventHandler(this.numericTextBox_Validated);
      this.txtESInterval.Validating += new System.ComponentModel.CancelEventHandler(this.numericTextBox_Validating);
      // 
      // label18
      // 
      this.label18.AutoSize = true;
      this.label18.Location = new System.Drawing.Point(139, 22);
      this.label18.Name = "label18";
      this.label18.Size = new System.Drawing.Size(24, 13);
      this.label18.TabIndex = 12;
      this.label18.Text = "sec";
      // 
      // groupBox2
      // 
      this.groupBox2.Controls.Add(this.lblCHMinPct);
      this.groupBox2.Controls.Add(this.lblCHMaxPct);
      this.groupBox2.Controls.Add(this.trkCH);
      this.groupBox2.Controls.Add(this.cboCHMinRank);
      this.groupBox2.Controls.Add(this.cboCHMaxRank);
      this.groupBox2.Controls.Add(this.label22);
      this.groupBox2.Controls.Add(this.lblHWMinPct);
      this.groupBox2.Controls.Add(this.lblHWMaxPct);
      this.groupBox2.Controls.Add(this.trkHW);
      this.groupBox2.Controls.Add(this.cboHWMinRank);
      this.groupBox2.Controls.Add(this.cboHWMaxRank);
      this.groupBox2.Controls.Add(this.label19);
      this.groupBox2.Location = new System.Drawing.Point(3, 222);
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.Size = new System.Drawing.Size(198, 145);
      this.groupBox2.TabIndex = 1;
      this.groupBox2.TabStop = false;
      this.groupBox2.Text = "Downranking";
      // 
      // lblCHMinPct
      // 
      this.lblCHMinPct.Location = new System.Drawing.Point(151, 120);
      this.lblCHMinPct.Name = "lblCHMinPct";
      this.lblCHMinPct.Size = new System.Drawing.Size(41, 21);
      this.lblCHMinPct.TabIndex = 11;
      this.lblCHMinPct.Text = "0%";
      this.lblCHMinPct.TextAlign = System.Drawing.ContentAlignment.TopCenter;
      // 
      // lblCHMaxPct
      // 
      this.lblCHMaxPct.Location = new System.Drawing.Point(6, 120);
      this.lblCHMaxPct.Name = "lblCHMaxPct";
      this.lblCHMaxPct.Size = new System.Drawing.Size(41, 21);
      this.lblCHMaxPct.TabIndex = 10;
      this.lblCHMaxPct.Text = "100%";
      this.lblCHMaxPct.TextAlign = System.Drawing.ContentAlignment.TopCenter;
      // 
      // trkCH
      // 
      this.trkCH.BackColor = System.Drawing.SystemColors.Window;
      this.trkCH.Location = new System.Drawing.Point(53, 96);
      this.trkCH.Maximum = 100;
      this.trkCH.Name = "trkCH";
      this.trkCH.Size = new System.Drawing.Size(95, 45);
      this.trkCH.TabIndex = 9;
      this.trkCH.TickFrequency = 10;
      this.trkCH.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
      this.tipRestoSham.SetToolTip(this.trkCH, "Adjust this slider to indicate the ratio of max rank versus downranked Chain Heal" +
              "\'s you cast");
      this.trkCH.ValueChanged += new System.EventHandler(this.trkCH_ValueChanged);
      // 
      // cboCHMinRank
      // 
      this.cboCHMinRank.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cboCHMinRank.FormattingEnabled = true;
      this.cboCHMinRank.Location = new System.Drawing.Point(154, 96);
      this.cboCHMinRank.Name = "cboCHMinRank";
      this.cboCHMinRank.Size = new System.Drawing.Size(38, 21);
      this.cboCHMinRank.TabIndex = 8;
      this.tipRestoSham.SetToolTip(this.cboCHMinRank, "The rank of the downranked Chain Heal that you use");
      this.cboCHMinRank.SelectedIndexChanged += new System.EventHandler(this.cboCHMinRank_SelectedIndexChanged);
      // 
      // cboCHMaxRank
      // 
      this.cboCHMaxRank.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cboCHMaxRank.FormattingEnabled = true;
      this.cboCHMaxRank.Location = new System.Drawing.Point(9, 96);
      this.cboCHMaxRank.Name = "cboCHMaxRank";
      this.cboCHMaxRank.Size = new System.Drawing.Size(38, 21);
      this.cboCHMaxRank.TabIndex = 7;
      this.tipRestoSham.SetToolTip(this.cboCHMaxRank, "The max rank of Chain Heal that you use");
      this.cboCHMaxRank.SelectedIndexChanged += new System.EventHandler(this.cboCHMaxRank_SelectedIndexChanged);
      // 
      // label22
      // 
      this.label22.AutoSize = true;
      this.label22.Location = new System.Drawing.Point(6, 80);
      this.label22.Name = "label22";
      this.label22.Size = new System.Drawing.Size(62, 13);
      this.label22.TabIndex = 6;
      this.label22.Text = "Chain Heal:";
      // 
      // lblHWMinPct
      // 
      this.lblHWMinPct.Location = new System.Drawing.Point(151, 56);
      this.lblHWMinPct.Name = "lblHWMinPct";
      this.lblHWMinPct.Size = new System.Drawing.Size(41, 21);
      this.lblHWMinPct.TabIndex = 5;
      this.lblHWMinPct.Text = "25%";
      this.lblHWMinPct.TextAlign = System.Drawing.ContentAlignment.TopCenter;
      // 
      // lblHWMaxPct
      // 
      this.lblHWMaxPct.Location = new System.Drawing.Point(9, 56);
      this.lblHWMaxPct.Name = "lblHWMaxPct";
      this.lblHWMaxPct.Size = new System.Drawing.Size(38, 21);
      this.lblHWMaxPct.TabIndex = 4;
      this.lblHWMaxPct.Text = "75%";
      this.lblHWMaxPct.TextAlign = System.Drawing.ContentAlignment.TopCenter;
      // 
      // trkHW
      // 
      this.trkHW.BackColor = System.Drawing.SystemColors.Window;
      this.trkHW.Location = new System.Drawing.Point(53, 32);
      this.trkHW.Maximum = 100;
      this.trkHW.Name = "trkHW";
      this.trkHW.Size = new System.Drawing.Size(95, 45);
      this.trkHW.TabIndex = 3;
      this.trkHW.TickFrequency = 10;
      this.trkHW.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
      this.tipRestoSham.SetToolTip(this.trkHW, "Adjust this slider to indicate the ratio of max rank versus downranked Healing Wa" +
              "ve\'s you cast");
      this.trkHW.ValueChanged += new System.EventHandler(this.trkHW_ValueChanged);
      // 
      // cboHWMinRank
      // 
      this.cboHWMinRank.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cboHWMinRank.FormattingEnabled = true;
      this.cboHWMinRank.Location = new System.Drawing.Point(154, 32);
      this.cboHWMinRank.Name = "cboHWMinRank";
      this.cboHWMinRank.Size = new System.Drawing.Size(38, 21);
      this.cboHWMinRank.TabIndex = 2;
      this.tipRestoSham.SetToolTip(this.cboHWMinRank, "The rank of the downranked Healing Wave that you use");
      this.cboHWMinRank.SelectedIndexChanged += new System.EventHandler(this.cboHWMinRank_SelectedIndexChanged);
      // 
      // cboHWMaxRank
      // 
      this.cboHWMaxRank.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cboHWMaxRank.FormattingEnabled = true;
      this.cboHWMaxRank.Location = new System.Drawing.Point(9, 32);
      this.cboHWMaxRank.Name = "cboHWMaxRank";
      this.cboHWMaxRank.Size = new System.Drawing.Size(38, 21);
      this.cboHWMaxRank.TabIndex = 1;
      this.tipRestoSham.SetToolTip(this.cboHWMaxRank, "The maximum rank of Healing Wave that you use");
      this.cboHWMaxRank.SelectedIndexChanged += new System.EventHandler(this.cboHWMaxRank_SelectedIndexChanged);
      // 
      // label19
      // 
      this.label19.AutoSize = true;
      this.label19.Location = new System.Drawing.Point(6, 16);
      this.label19.Name = "label19";
      this.label19.Size = new System.Drawing.Size(78, 13);
      this.label19.TabIndex = 0;
      this.label19.Text = "Healing Wave:";
      // 
      // groupBox1
      // 
      this.groupBox1.Controls.Add(this.label21);
      this.groupBox1.Controls.Add(this.cboNumCHTargets);
      this.groupBox1.Controls.Add(this.label20);
      this.groupBox1.Controls.Add(this.label15);
      this.groupBox1.Controls.Add(this.label14);
      this.groupBox1.Controls.Add(this.label13);
      this.groupBox1.Controls.Add(this.txtCHRatio);
      this.groupBox1.Controls.Add(this.txtLHWRatio);
      this.groupBox1.Controls.Add(this.txtHWRatio);
      this.groupBox1.Controls.Add(this.label12);
      this.groupBox1.Controls.Add(this.label11);
      this.groupBox1.Controls.Add(this.label10);
      this.groupBox1.Location = new System.Drawing.Point(3, 6);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(198, 129);
      this.groupBox1.TabIndex = 0;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Spell Selection";
      // 
      // label21
      // 
      this.label21.AutoSize = true;
      this.label21.Location = new System.Drawing.Point(132, 100);
      this.label21.Name = "label21";
      this.label21.Size = new System.Drawing.Size(39, 13);
      this.label21.TabIndex = 15;
      this.label21.Text = "people";
      // 
      // cboNumCHTargets
      // 
      this.cboNumCHTargets.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cboNumCHTargets.FormattingEnabled = true;
      this.cboNumCHTargets.Location = new System.Drawing.Point(90, 97);
      this.cboNumCHTargets.Name = "cboNumCHTargets";
      this.cboNumCHTargets.Size = new System.Drawing.Size(36, 21);
      this.cboNumCHTargets.TabIndex = 14;
      this.tipRestoSham.SetToolTip(this.cboNumCHTargets, "Select the number of targets healed by Chain Heal on average");
      this.cboNumCHTargets.SelectedIndexChanged += new System.EventHandler(this.cboNumCHTargets_SelectedIndexChanged);
      // 
      // label20
      // 
      this.label20.AutoSize = true;
      this.label20.Location = new System.Drawing.Point(3, 100);
      this.label20.Name = "label20";
      this.label20.Size = new System.Drawing.Size(87, 13);
      this.label20.TabIndex = 13;
      this.label20.Text = "Chain Heal heals";
      // 
      // label15
      // 
      this.label15.AutoSize = true;
      this.label15.Location = new System.Drawing.Point(139, 48);
      this.label15.Name = "label15";
      this.label15.Size = new System.Drawing.Size(15, 13);
      this.label15.TabIndex = 8;
      this.label15.Text = "%";
      // 
      // label14
      // 
      this.label14.AutoSize = true;
      this.label14.Location = new System.Drawing.Point(139, 74);
      this.label14.Name = "label14";
      this.label14.Size = new System.Drawing.Size(15, 13);
      this.label14.TabIndex = 7;
      this.label14.Text = "%";
      // 
      // label13
      // 
      this.label13.AutoSize = true;
      this.label13.Location = new System.Drawing.Point(139, 22);
      this.label13.Name = "label13";
      this.label13.Size = new System.Drawing.Size(15, 13);
      this.label13.TabIndex = 6;
      this.label13.Text = "%";
      // 
      // txtCHRatio
      // 
      this.txtCHRatio.Location = new System.Drawing.Point(90, 71);
      this.txtCHRatio.Name = "txtCHRatio";
      this.txtCHRatio.Size = new System.Drawing.Size(43, 20);
      this.txtCHRatio.TabIndex = 5;
      this.tipRestoSham.SetToolTip(this.txtCHRatio, "The percentage of heals that you cast that are Chain Heal (enter 0 if you don\'t c" +
              "ast Chain Heal)");
      this.txtCHRatio.Validated += new System.EventHandler(this.numericTextBox_Validated);
      this.txtCHRatio.Validating += new System.ComponentModel.CancelEventHandler(this.numericTextBox_Validating);
      // 
      // txtLHWRatio
      // 
      this.txtLHWRatio.Location = new System.Drawing.Point(90, 45);
      this.txtLHWRatio.Name = "txtLHWRatio";
      this.txtLHWRatio.Size = new System.Drawing.Size(43, 20);
      this.txtLHWRatio.TabIndex = 4;
      this.tipRestoSham.SetToolTip(this.txtLHWRatio, "The percentage of heals that you cast that are Lesser Healing Wave (enter 0 if yo" +
              "u don\'t cast Lesser Healing Wave)");
      this.txtLHWRatio.Validated += new System.EventHandler(this.numericTextBox_Validated);
      this.txtLHWRatio.Validating += new System.ComponentModel.CancelEventHandler(this.numericTextBox_Validating);
      // 
      // txtHWRatio
      // 
      this.txtHWRatio.Location = new System.Drawing.Point(90, 19);
      this.txtHWRatio.Name = "txtHWRatio";
      this.txtHWRatio.Size = new System.Drawing.Size(43, 20);
      this.txtHWRatio.TabIndex = 3;
      this.tipRestoSham.SetToolTip(this.txtHWRatio, "The percentage of all heals that you cast that are Healing Wave (enter 0 if you d" +
              "on\'t cast Healing Wave)");
      this.txtHWRatio.Validated += new System.EventHandler(this.numericTextBox_Validated);
      this.txtHWRatio.Validating += new System.ComponentModel.CancelEventHandler(this.numericTextBox_Validating);
      // 
      // label12
      // 
      this.label12.AutoSize = true;
      this.label12.Location = new System.Drawing.Point(22, 74);
      this.label12.Name = "label12";
      this.label12.Size = new System.Drawing.Size(62, 13);
      this.label12.TabIndex = 2;
      this.label12.Text = "Chain Heal:";
      // 
      // label11
      // 
      this.label11.AutoSize = true;
      this.label11.Location = new System.Drawing.Point(21, 48);
      this.label11.Name = "label11";
      this.label11.Size = new System.Drawing.Size(63, 13);
      this.label11.TabIndex = 1;
      this.label11.Text = "Lesser HW:";
      // 
      // label10
      // 
      this.label10.AutoSize = true;
      this.label10.Location = new System.Drawing.Point(6, 22);
      this.label10.Name = "label10";
      this.label10.Size = new System.Drawing.Size(78, 13);
      this.label10.TabIndex = 0;
      this.label10.Text = "Healing Wave:";
      // 
      // tabPage3
      // 
      this.tabPage3.Controls.Add(this.lblCHTotemDesc);
      this.tabPage3.Controls.Add(this.lblLHWTotemDesc);
      this.tabPage3.Controls.Add(this.lblHWTotemDesc);
      this.tabPage3.Controls.Add(this.chkEquipTotems);
      this.tabPage3.Controls.Add(this.cboCHTotem);
      this.tabPage3.Controls.Add(this.cboLHWTotem);
      this.tabPage3.Controls.Add(this.cboHWTotem);
      this.tabPage3.Controls.Add(this.label27);
      this.tabPage3.Controls.Add(this.label26);
      this.tabPage3.Controls.Add(this.label25);
      this.tabPage3.Location = new System.Drawing.Point(4, 22);
      this.tabPage3.Name = "tabPage3";
      this.tabPage3.Size = new System.Drawing.Size(204, 501);
      this.tabPage3.TabIndex = 2;
      this.tabPage3.Text = "Totems";
      this.tabPage3.UseVisualStyleBackColor = true;
      // 
      // lblCHTotemDesc
      // 
      this.lblCHTotemDesc.AutoSize = true;
      this.lblCHTotemDesc.Location = new System.Drawing.Point(3, 174);
      this.lblCHTotemDesc.Name = "lblCHTotemDesc";
      this.lblCHTotemDesc.Size = new System.Drawing.Size(33, 13);
      this.lblCHTotemDesc.TabIndex = 9;
      this.lblCHTotemDesc.Text = "ch txt";
      // 
      // lblLHWTotemDesc
      // 
      this.lblLHWTotemDesc.AutoSize = true;
      this.lblLHWTotemDesc.Location = new System.Drawing.Point(3, 112);
      this.lblLHWTotemDesc.Name = "lblLHWTotemDesc";
      this.lblLHWTotemDesc.Size = new System.Drawing.Size(37, 13);
      this.lblLHWTotemDesc.TabIndex = 8;
      this.lblLHWTotemDesc.Text = "lhw txt";
      // 
      // lblHWTotemDesc
      // 
      this.lblHWTotemDesc.AutoSize = true;
      this.lblHWTotemDesc.Location = new System.Drawing.Point(3, 50);
      this.lblHWTotemDesc.Name = "lblHWTotemDesc";
      this.lblHWTotemDesc.Size = new System.Drawing.Size(35, 13);
      this.lblHWTotemDesc.TabIndex = 7;
      this.lblHWTotemDesc.Text = "hw txt";
      // 
      // chkEquipTotems
      // 
      this.chkEquipTotems.AutoSize = true;
      this.chkEquipTotems.Location = new System.Drawing.Point(6, 200);
      this.chkEquipTotems.Name = "chkEquipTotems";
      this.chkEquipTotems.Size = new System.Drawing.Size(142, 17);
      this.chkEquipTotems.TabIndex = 6;
      this.chkEquipTotems.Text = "Equip totems during fight";
      this.tipRestoSham.SetToolTip(this.chkEquipTotems, "Check this to indicate that you change totems during a fight (using macros) so th" +
              "at the appropriate totem is equipped when a heal is cast");
      this.chkEquipTotems.UseVisualStyleBackColor = true;
      this.chkEquipTotems.CheckedChanged += new System.EventHandler(this.chkEquipTotems_CheckedChanged);
      // 
      // cboCHTotem
      // 
      this.cboCHTotem.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cboCHTotem.FormattingEnabled = true;
      this.cboCHTotem.Location = new System.Drawing.Point(3, 150);
      this.cboCHTotem.Name = "cboCHTotem";
      this.cboCHTotem.Size = new System.Drawing.Size(198, 21);
      this.cboCHTotem.TabIndex = 5;
      this.tipRestoSham.SetToolTip(this.cboCHTotem, "Select the totem that affects Chain Heal that you use");
      this.cboCHTotem.SelectedIndexChanged += new System.EventHandler(this.cboCHTotem_SelectedIndexChanged);
      // 
      // cboLHWTotem
      // 
      this.cboLHWTotem.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cboLHWTotem.FormattingEnabled = true;
      this.cboLHWTotem.Location = new System.Drawing.Point(6, 88);
      this.cboLHWTotem.Name = "cboLHWTotem";
      this.cboLHWTotem.Size = new System.Drawing.Size(195, 21);
      this.cboLHWTotem.TabIndex = 4;
      this.tipRestoSham.SetToolTip(this.cboLHWTotem, "Select the totem that affects Lesser Healing Wave that you use");
      this.cboLHWTotem.SelectedIndexChanged += new System.EventHandler(this.cboLHWTotem_SelectedIndexChanged);
      // 
      // cboHWTotem
      // 
      this.cboHWTotem.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cboHWTotem.FormattingEnabled = true;
      this.cboHWTotem.Location = new System.Drawing.Point(6, 26);
      this.cboHWTotem.Name = "cboHWTotem";
      this.cboHWTotem.Size = new System.Drawing.Size(195, 21);
      this.cboHWTotem.TabIndex = 3;
      this.tipRestoSham.SetToolTip(this.cboHWTotem, "Select which totem that affects Healing Wave that you use");
      this.cboHWTotem.SelectedIndexChanged += new System.EventHandler(this.cboHWTotem_SelectedIndexChanged);
      // 
      // label27
      // 
      this.label27.AutoSize = true;
      this.label27.Location = new System.Drawing.Point(3, 134);
      this.label27.Name = "label27";
      this.label27.Size = new System.Drawing.Size(62, 13);
      this.label27.TabIndex = 2;
      this.label27.Text = "Chain Heal:";
      // 
      // label26
      // 
      this.label26.AutoSize = true;
      this.label26.Location = new System.Drawing.Point(3, 72);
      this.label26.Name = "label26";
      this.label26.Size = new System.Drawing.Size(112, 13);
      this.label26.TabIndex = 1;
      this.label26.Text = "Lesser Healing Wave:";
      // 
      // label25
      // 
      this.label25.AutoSize = true;
      this.label25.Location = new System.Drawing.Point(3, 10);
      this.label25.Name = "label25";
      this.label25.Size = new System.Drawing.Size(78, 13);
      this.label25.TabIndex = 0;
      this.label25.Text = "Healing Wave:";
      // 
      // errorRestoSham
      // 
      this.errorRestoSham.ContainerControl = this;
      // 
      // CalculationOptionsPanelRestoSham
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.tabControl1);
      this.Name = "CalculationOptionsPanelRestoSham";
      this.Size = new System.Drawing.Size(212, 527);
      this.tabControl1.ResumeLayout(false);
      this.tabPage1.ResumeLayout(false);
      this.tabPage1.PerformLayout();
      this.tabPage2.ResumeLayout(false);
      this.groupBox3.ResumeLayout(false);
      this.groupBox3.PerformLayout();
      this.groupBox2.ResumeLayout(false);
      this.groupBox2.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.trkCH)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.trkHW)).EndInit();
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.tabPage3.ResumeLayout(false);
      this.tabPage3.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.errorRestoSham)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.TextBox txtFightLength;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.TextBox txtOutsideFSR;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.TextBox txtSPriestMp5;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.Label label6;
    private System.Windows.Forms.Label label7;
    private System.Windows.Forms.ComboBox cboManaPotAmount;
    private System.Windows.Forms.Label label8;
    private System.Windows.Forms.Label label9;
    private System.Windows.Forms.TextBox txtManaPotInterval;
    private System.Windows.Forms.TabControl tabControl1;
    private System.Windows.Forms.TabPage tabPage1;
    private System.Windows.Forms.TabPage tabPage2;
    private System.Windows.Forms.TabPage tabPage3;
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.Label label15;
    private System.Windows.Forms.Label label14;
    private System.Windows.Forms.Label label13;
    private System.Windows.Forms.TextBox txtCHRatio;
    private System.Windows.Forms.TextBox txtLHWRatio;
    private System.Windows.Forms.TextBox txtHWRatio;
    private System.Windows.Forms.Label label12;
    private System.Windows.Forms.Label label11;
    private System.Windows.Forms.Label label10;
    private System.Windows.Forms.GroupBox groupBox2;
    private System.Windows.Forms.TrackBar trkHW;
    private System.Windows.Forms.ComboBox cboHWMinRank;
    private System.Windows.Forms.ComboBox cboHWMaxRank;
    private System.Windows.Forms.Label label19;
    private System.Windows.Forms.Label label18;
    private System.Windows.Forms.TextBox txtESInterval;
    private System.Windows.Forms.Label label17;
    private System.Windows.Forms.Label label16;
    private System.Windows.Forms.ComboBox cboCHMinRank;
    private System.Windows.Forms.ComboBox cboCHMaxRank;
    private System.Windows.Forms.Label label22;
    private System.Windows.Forms.Label lblHWMinPct;
    private System.Windows.Forms.Label lblHWMaxPct;
    private System.Windows.Forms.Label lblCHMinPct;
    private System.Windows.Forms.Label lblCHMaxPct;
    private System.Windows.Forms.TrackBar trkCH;
    private System.Windows.Forms.CheckBox chkEquipTotems;
    private System.Windows.Forms.ComboBox cboCHTotem;
    private System.Windows.Forms.ComboBox cboLHWTotem;
    private System.Windows.Forms.ComboBox cboHWTotem;
    private System.Windows.Forms.Label label27;
    private System.Windows.Forms.Label label26;
    private System.Windows.Forms.Label label25;
    private System.Windows.Forms.Label lblCHTotemDesc;
    private System.Windows.Forms.Label lblLHWTotemDesc;
    private System.Windows.Forms.Label lblHWTotemDesc;
    private System.Windows.Forms.ToolTip tipRestoSham;
    private System.Windows.Forms.ErrorProvider errorRestoSham;
    private System.Windows.Forms.ComboBox cboNumCHTargets;
    private System.Windows.Forms.Label label20;
    private System.Windows.Forms.Label label21;
    private System.Windows.Forms.CheckBox chkManaTide;
    private System.Windows.Forms.CheckBox chkWaterShield;
    private System.Windows.Forms.GroupBox groupBox3;
    private System.Windows.Forms.ComboBox cboESRank;
    private System.Windows.Forms.RadioButton radioScryers;
    private System.Windows.Forms.RadioButton radioAldor;
    private System.Windows.Forms.CheckBox chkExalted;

  }
}
