namespace Rawr
{
	partial class CalculationOptionsPanelEnhance
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
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.chbBaseStatOption = new System.Windows.Forms.CheckBox();
            this.labelTargetArmorDescription = new System.Windows.Forms.Label();
            this.comboBoxOffhandImbue = new System.Windows.Forms.ComboBox();
            this.comboBoxMainhandImbue = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.radioButtonScryer = new System.Windows.Forms.RadioButton();
            this.radioButtonAldor = new System.Windows.Forms.RadioButton();
            this.labelNumberOfFerociousInspirations = new System.Windows.Forms.Label();
            this.labelBloodlustUptime = new System.Windows.Forms.Label();
            this.labelExposeWeakness = new System.Windows.Forms.Label();
            this.trackBarNumberOfFerociousInspirations = new System.Windows.Forms.TrackBar();
            this.label6 = new System.Windows.Forms.Label();
            this.trackBarBloodlustUptime = new System.Windows.Forms.TrackBar();
            this.trackBarExposeWeakness = new System.Windows.Forms.TrackBar();
            this.trackBarTargetArmor = new System.Windows.Forms.TrackBar();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBoxTargetLevel = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.gbGlyphs = new System.Windows.Forms.GroupBox();
            this.chbGlyphFS = new System.Windows.Forms.CheckBox();
            this.chbGlyphLL = new System.Windows.Forms.CheckBox();
            this.chbGlyphSS = new System.Windows.Forms.CheckBox();
            this.chbGlyphWF = new System.Windows.Forms.CheckBox();
            this.chbGlyphShocking = new System.Windows.Forms.CheckBox();
            this.chbGlyphLS = new System.Windows.Forms.CheckBox();
            this.chbGlyphLB = new System.Windows.Forms.CheckBox();
            this.chbGlyphFT = new System.Windows.Forms.CheckBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.tbModuleNotes = new System.Windows.Forms.TextBox();
            this.btnEnhSim = new System.Windows.Forms.Button();
            this.tabControl.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarNumberOfFerociousInspirations)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarBloodlustUptime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarExposeWeakness)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarTargetArmor)).BeginInit();
            this.tabPage2.SuspendLayout();
            this.gbGlyphs.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPage1);
            this.tabControl.Controls.Add(this.tabPage2);
            this.tabControl.Controls.Add(this.tabPage3);
            this.tabControl.Location = new System.Drawing.Point(0, 3);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(272, 551);
            this.tabControl.TabIndex = 9;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.chbBaseStatOption);
            this.tabPage1.Controls.Add(this.labelTargetArmorDescription);
            this.tabPage1.Controls.Add(this.comboBoxOffhandImbue);
            this.tabPage1.Controls.Add(this.comboBoxMainhandImbue);
            this.tabPage1.Controls.Add(this.label8);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.radioButtonScryer);
            this.tabPage1.Controls.Add(this.radioButtonAldor);
            this.tabPage1.Controls.Add(this.labelNumberOfFerociousInspirations);
            this.tabPage1.Controls.Add(this.labelBloodlustUptime);
            this.tabPage1.Controls.Add(this.labelExposeWeakness);
            this.tabPage1.Controls.Add(this.trackBarNumberOfFerociousInspirations);
            this.tabPage1.Controls.Add(this.label6);
            this.tabPage1.Controls.Add(this.trackBarBloodlustUptime);
            this.tabPage1.Controls.Add(this.trackBarExposeWeakness);
            this.tabPage1.Controls.Add(this.trackBarTargetArmor);
            this.tabPage1.Controls.Add(this.label5);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.comboBoxTargetLevel);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(264, 525);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Basics";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // chbBaseStatOption
            // 
            this.chbBaseStatOption.AutoSize = true;
            this.chbBaseStatOption.Location = new System.Drawing.Point(10, 364);
            this.chbBaseStatOption.Name = "chbBaseStatOption";
            this.chbBaseStatOption.Size = new System.Drawing.Size(211, 17);
            this.chbBaseStatOption.TabIndex = 35;
            this.chbBaseStatOption.Text = "Use AEP values in Relative Stats Chart";
            this.chbBaseStatOption.UseVisualStyleBackColor = true;
            this.chbBaseStatOption.CheckedChanged += new System.EventHandler(this.chbBaseStatOption_CheckedChanged);
            // 
            // labelTargetArmorDescription
            // 
            this.labelTargetArmorDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.labelTargetArmorDescription.Location = new System.Drawing.Point(71, 226);
            this.labelTargetArmorDescription.Name = "labelTargetArmorDescription";
            this.labelTargetArmorDescription.Size = new System.Drawing.Size(192, 40);
            this.labelTargetArmorDescription.TabIndex = 34;
            this.labelTargetArmorDescription.Text = "7700 Armor: Hydross, Lurker, Leotheras, Tidewalker, Al\'ar, Naj\'entus, Supremus, A" +
                "kama, Gurtogg";
            // 
            // comboBoxOffhandImbue
            // 
            this.comboBoxOffhandImbue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxOffhandImbue.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxOffhandImbue.Enabled = false;
            this.comboBoxOffhandImbue.FormattingEnabled = true;
            this.comboBoxOffhandImbue.Items.AddRange(new object[] {
            "Flametongue"});
            this.comboBoxOffhandImbue.Location = new System.Drawing.Point(99, 328);
            this.comboBoxOffhandImbue.Name = "comboBoxOffhandImbue";
            this.comboBoxOffhandImbue.Size = new System.Drawing.Size(159, 21);
            this.comboBoxOffhandImbue.TabIndex = 33;
            // 
            // comboBoxMainhandImbue
            // 
            this.comboBoxMainhandImbue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxMainhandImbue.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxMainhandImbue.Enabled = false;
            this.comboBoxMainhandImbue.FormattingEnabled = true;
            this.comboBoxMainhandImbue.Items.AddRange(new object[] {
            "Windfury"});
            this.comboBoxMainhandImbue.Location = new System.Drawing.Point(99, 301);
            this.comboBoxMainhandImbue.Name = "comboBoxMainhandImbue";
            this.comboBoxMainhandImbue.Size = new System.Drawing.Size(159, 21);
            this.comboBoxMainhandImbue.TabIndex = 32;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(4, 331);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(80, 13);
            this.label8.TabIndex = 31;
            this.label8.Text = "Offhand Imbue:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(4, 304);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 13);
            this.label3.TabIndex = 30;
            this.label3.Text = "Mainhand Imbue:";
            // 
            // radioButtonScryer
            // 
            this.radioButtonScryer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.radioButtonScryer.AutoSize = true;
            this.radioButtonScryer.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.radioButtonScryer.Location = new System.Drawing.Point(74, 272);
            this.radioButtonScryer.Name = "radioButtonScryer";
            this.radioButtonScryer.Size = new System.Drawing.Size(55, 17);
            this.radioButtonScryer.TabIndex = 28;
            this.radioButtonScryer.Tag = "Shred";
            this.radioButtonScryer.Text = "Scryer";
            this.radioButtonScryer.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.radioButtonScryer.UseVisualStyleBackColor = true;
            // 
            // radioButtonAldor
            // 
            this.radioButtonAldor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.radioButtonAldor.AutoSize = true;
            this.radioButtonAldor.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.radioButtonAldor.Checked = true;
            this.radioButtonAldor.Location = new System.Drawing.Point(6, 272);
            this.radioButtonAldor.Name = "radioButtonAldor";
            this.radioButtonAldor.Size = new System.Drawing.Size(49, 17);
            this.radioButtonAldor.TabIndex = 29;
            this.radioButtonAldor.TabStop = true;
            this.radioButtonAldor.Tag = "Mangle";
            this.radioButtonAldor.Text = "Aldor";
            this.radioButtonAldor.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.radioButtonAldor.UseVisualStyleBackColor = true;
            // 
            // labelNumberOfFerociousInspirations
            // 
            this.labelNumberOfFerociousInspirations.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.labelNumberOfFerociousInspirations.AutoSize = true;
            this.labelNumberOfFerociousInspirations.Location = new System.Drawing.Point(71, 174);
            this.labelNumberOfFerociousInspirations.Name = "labelNumberOfFerociousInspirations";
            this.labelNumberOfFerociousInspirations.Size = new System.Drawing.Size(13, 13);
            this.labelNumberOfFerociousInspirations.TabIndex = 11;
            this.labelNumberOfFerociousInspirations.Text = "2";
            // 
            // labelBloodlustUptime
            // 
            this.labelBloodlustUptime.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.labelBloodlustUptime.AutoSize = true;
            this.labelBloodlustUptime.Location = new System.Drawing.Point(71, 123);
            this.labelBloodlustUptime.Name = "labelBloodlustUptime";
            this.labelBloodlustUptime.Size = new System.Drawing.Size(27, 13);
            this.labelBloodlustUptime.TabIndex = 12;
            this.labelBloodlustUptime.Text = "15%";
            // 
            // labelExposeWeakness
            // 
            this.labelExposeWeakness.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.labelExposeWeakness.AutoSize = true;
            this.labelExposeWeakness.Location = new System.Drawing.Point(71, 72);
            this.labelExposeWeakness.Name = "labelExposeWeakness";
            this.labelExposeWeakness.Size = new System.Drawing.Size(25, 13);
            this.labelExposeWeakness.TabIndex = 15;
            this.labelExposeWeakness.Text = "200";
            // 
            // trackBarNumberOfFerociousInspirations
            // 
            this.trackBarNumberOfFerociousInspirations.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.trackBarNumberOfFerociousInspirations.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.trackBarNumberOfFerociousInspirations.Location = new System.Drawing.Point(74, 142);
            this.trackBarNumberOfFerociousInspirations.Maximum = 4;
            this.trackBarNumberOfFerociousInspirations.Minimum = 1;
            this.trackBarNumberOfFerociousInspirations.Name = "trackBarNumberOfFerociousInspirations";
            this.trackBarNumberOfFerociousInspirations.Size = new System.Drawing.Size(184, 45);
            this.trackBarNumberOfFerociousInspirations.TabIndex = 25;
            this.trackBarNumberOfFerociousInspirations.Value = 2;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(7, 145);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(60, 39);
            this.label6.TabIndex = 13;
            this.label6.Text = "Number of\r\nFerocious\r\nInspirations";
            this.label6.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // trackBarBloodlustUptime
            // 
            this.trackBarBloodlustUptime.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.trackBarBloodlustUptime.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.trackBarBloodlustUptime.Location = new System.Drawing.Point(74, 91);
            this.trackBarBloodlustUptime.Maximum = 100;
            this.trackBarBloodlustUptime.Minimum = 5;
            this.trackBarBloodlustUptime.Name = "trackBarBloodlustUptime";
            this.trackBarBloodlustUptime.Size = new System.Drawing.Size(184, 45);
            this.trackBarBloodlustUptime.TabIndex = 23;
            this.trackBarBloodlustUptime.TickFrequency = 5;
            this.trackBarBloodlustUptime.Value = 15;
            // 
            // trackBarExposeWeakness
            // 
            this.trackBarExposeWeakness.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.trackBarExposeWeakness.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.trackBarExposeWeakness.LargeChange = 50;
            this.trackBarExposeWeakness.Location = new System.Drawing.Point(74, 40);
            this.trackBarExposeWeakness.Maximum = 500;
            this.trackBarExposeWeakness.Minimum = 50;
            this.trackBarExposeWeakness.Name = "trackBarExposeWeakness";
            this.trackBarExposeWeakness.Size = new System.Drawing.Size(184, 45);
            this.trackBarExposeWeakness.SmallChange = 10;
            this.trackBarExposeWeakness.TabIndex = 22;
            this.trackBarExposeWeakness.TickFrequency = 25;
            this.trackBarExposeWeakness.Value = 200;
            // 
            // trackBarTargetArmor
            // 
            this.trackBarTargetArmor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.trackBarTargetArmor.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.trackBarTargetArmor.LargeChange = 1000;
            this.trackBarTargetArmor.Location = new System.Drawing.Point(74, 193);
            this.trackBarTargetArmor.Maximum = 15000;
            this.trackBarTargetArmor.Minimum = 3000;
            this.trackBarTargetArmor.Name = "trackBarTargetArmor";
            this.trackBarTargetArmor.Size = new System.Drawing.Size(184, 45);
            this.trackBarTargetArmor.SmallChange = 100;
            this.trackBarTargetArmor.TabIndex = 24;
            this.trackBarTargetArmor.TickFrequency = 300;
            this.trackBarTargetArmor.Value = 13083;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 94);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(54, 26);
            this.label5.TabIndex = 20;
            this.label5.Text = "Bloodlust\r\nUptime %:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 43);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(58, 39);
            this.label4.TabIndex = 16;
            this.label4.Text = "Expose\r\nWeakness\r\nAP Bonus:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(-1, 193);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 13);
            this.label2.TabIndex = 17;
            this.label2.Text = "Target Armor: ";
            // 
            // comboBoxTargetLevel
            // 
            this.comboBoxTargetLevel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxTargetLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxTargetLevel.Enabled = false;
            this.comboBoxTargetLevel.FormattingEnabled = true;
            this.comboBoxTargetLevel.Items.AddRange(new object[] {
            "80",
            "81",
            "82",
            "83"});
            this.comboBoxTargetLevel.Location = new System.Drawing.Point(74, 13);
            this.comboBoxTargetLevel.Name = "comboBoxTargetLevel";
            this.comboBoxTargetLevel.Size = new System.Drawing.Size(184, 21);
            this.comboBoxTargetLevel.TabIndex = 21;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(-1, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 13);
            this.label1.TabIndex = 18;
            this.label1.Text = "Target Level: ";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.gbGlyphs);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(264, 525);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Glyphs";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // gbGlyphs
            // 
            this.gbGlyphs.Controls.Add(this.chbGlyphFS);
            this.gbGlyphs.Controls.Add(this.chbGlyphLL);
            this.gbGlyphs.Controls.Add(this.chbGlyphSS);
            this.gbGlyphs.Controls.Add(this.chbGlyphWF);
            this.gbGlyphs.Controls.Add(this.chbGlyphShocking);
            this.gbGlyphs.Controls.Add(this.chbGlyphLS);
            this.gbGlyphs.Controls.Add(this.chbGlyphLB);
            this.gbGlyphs.Controls.Add(this.chbGlyphFT);
            this.gbGlyphs.Location = new System.Drawing.Point(3, 6);
            this.gbGlyphs.Name = "gbGlyphs";
            this.gbGlyphs.Size = new System.Drawing.Size(258, 211);
            this.gbGlyphs.TabIndex = 57;
            this.gbGlyphs.TabStop = false;
            this.gbGlyphs.Text = "Glyphs";
            // 
            // chbGlyphFS
            // 
            this.chbGlyphFS.AutoSize = true;
            this.chbGlyphFS.Location = new System.Drawing.Point(8, 19);
            this.chbGlyphFS.Name = "chbGlyphFS";
            this.chbGlyphFS.Size = new System.Drawing.Size(122, 17);
            this.chbGlyphFS.TabIndex = 78;
            this.chbGlyphFS.Text = "Glyph of Feral Spirits";
            this.chbGlyphFS.UseVisualStyleBackColor = true;
            this.chbGlyphFS.CheckedChanged += new System.EventHandler(this.chbGlyphFS_CheckedChanged);
            // 
            // chbGlyphLL
            // 
            this.chbGlyphLL.AutoSize = true;
            this.chbGlyphLL.Location = new System.Drawing.Point(7, 65);
            this.chbGlyphLL.Name = "chbGlyphLL";
            this.chbGlyphLL.Size = new System.Drawing.Size(118, 17);
            this.chbGlyphLL.TabIndex = 77;
            this.chbGlyphLL.Text = "Glyph of Lava Lash";
            this.chbGlyphLL.UseVisualStyleBackColor = true;
            this.chbGlyphLL.CheckedChanged += new System.EventHandler(this.chbGlyphLL_CheckedChanged);
            // 
            // chbGlyphSS
            // 
            this.chbGlyphSS.AutoSize = true;
            this.chbGlyphSS.Location = new System.Drawing.Point(7, 157);
            this.chbGlyphSS.Name = "chbGlyphSS";
            this.chbGlyphSS.Size = new System.Drawing.Size(120, 17);
            this.chbGlyphSS.TabIndex = 76;
            this.chbGlyphSS.Text = "Glyph of Stormstrike";
            this.chbGlyphSS.UseVisualStyleBackColor = true;
            this.chbGlyphSS.CheckedChanged += new System.EventHandler(this.chbGlyphSS_CheckedChanged);
            // 
            // chbGlyphWF
            // 
            this.chbGlyphWF.AutoSize = true;
            this.chbGlyphWF.Location = new System.Drawing.Point(7, 180);
            this.chbGlyphWF.Name = "chbGlyphWF";
            this.chbGlyphWF.Size = new System.Drawing.Size(154, 17);
            this.chbGlyphWF.TabIndex = 74;
            this.chbGlyphWF.Text = "Glyph of Windfury Weapon";
            this.chbGlyphWF.UseVisualStyleBackColor = true;
            this.chbGlyphWF.CheckedChanged += new System.EventHandler(this.chbGlyphWF_CheckedChanged);
            // 
            // chbGlyphShocking
            // 
            this.chbGlyphShocking.AutoSize = true;
            this.chbGlyphShocking.Enabled = false;
            this.chbGlyphShocking.Location = new System.Drawing.Point(7, 134);
            this.chbGlyphShocking.Name = "chbGlyphShocking";
            this.chbGlyphShocking.Size = new System.Drawing.Size(113, 17);
            this.chbGlyphShocking.TabIndex = 71;
            this.chbGlyphShocking.Text = "Glyph of Shocking";
            this.chbGlyphShocking.UseVisualStyleBackColor = true;
            this.chbGlyphShocking.CheckedChanged += new System.EventHandler(this.chbGlyphShocking_CheckedChanged);
            // 
            // chbGlyphLS
            // 
            this.chbGlyphLS.AutoSize = true;
            this.chbGlyphLS.Location = new System.Drawing.Point(7, 111);
            this.chbGlyphLS.Name = "chbGlyphLS";
            this.chbGlyphLS.Size = new System.Drawing.Size(143, 17);
            this.chbGlyphLS.TabIndex = 68;
            this.chbGlyphLS.Text = "Glyph of Lightning Shield";
            this.chbGlyphLS.UseVisualStyleBackColor = true;
            this.chbGlyphLS.CheckedChanged += new System.EventHandler(this.chbGlyphLS_CheckedChanged);
            // 
            // chbGlyphLB
            // 
            this.chbGlyphLB.AutoSize = true;
            this.chbGlyphLB.Location = new System.Drawing.Point(7, 88);
            this.chbGlyphLB.Name = "chbGlyphLB";
            this.chbGlyphLB.Size = new System.Drawing.Size(132, 17);
            this.chbGlyphLB.TabIndex = 67;
            this.chbGlyphLB.Text = "Glyph of Lightning Bolt";
            this.chbGlyphLB.UseVisualStyleBackColor = true;
            this.chbGlyphLB.CheckedChanged += new System.EventHandler(this.chbGlyphLB_CheckedChanged);
            // 
            // chbGlyphFT
            // 
            this.chbGlyphFT.AutoSize = true;
            this.chbGlyphFT.Location = new System.Drawing.Point(8, 42);
            this.chbGlyphFT.Name = "chbGlyphFT";
            this.chbGlyphFT.Size = new System.Drawing.Size(173, 17);
            this.chbGlyphFT.TabIndex = 66;
            this.chbGlyphFT.Text = "Glyph of Flametongue Weapon";
            this.chbGlyphFT.UseVisualStyleBackColor = true;
            this.chbGlyphFT.CheckedChanged += new System.EventHandler(this.chbGlyphFT_CheckedChanged);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.tbModuleNotes);
            this.tabPage3.Controls.Add(this.btnEnhSim);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(264, 525);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "EnhSim";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // tbModuleNotes
            // 
            this.tbModuleNotes.AcceptsReturn = true;
            this.tbModuleNotes.AcceptsTab = true;
            this.tbModuleNotes.Location = new System.Drawing.Point(3, 68);
            this.tbModuleNotes.Multiline = true;
            this.tbModuleNotes.Name = "tbModuleNotes";
            this.tbModuleNotes.ReadOnly = true;
            this.tbModuleNotes.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbModuleNotes.Size = new System.Drawing.Size(258, 454);
            this.tbModuleNotes.TabIndex = 1;
            // 
            // btnEnhSim
            // 
            this.btnEnhSim.Location = new System.Drawing.Point(21, 23);
            this.btnEnhSim.Name = "btnEnhSim";
            this.btnEnhSim.Size = new System.Drawing.Size(208, 28);
            this.btnEnhSim.TabIndex = 0;
            this.btnEnhSim.Text = "Export Stats to EnhSim config file";
            this.btnEnhSim.UseVisualStyleBackColor = true;
            this.btnEnhSim.Click += new System.EventHandler(this.btnEnhSim_Click);
            // 
            // CalculationOptionsPanelEnhance
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.tabControl);
            this.Name = "CalculationOptionsPanelEnhance";
            this.Size = new System.Drawing.Size(284, 573);
            this.tabControl.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarNumberOfFerociousInspirations)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarBloodlustUptime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarExposeWeakness)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarTargetArmor)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.gbGlyphs.ResumeLayout(false);
            this.gbGlyphs.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.ResumeLayout(false);

		}

		#endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Label labelTargetArmorDescription;
        private System.Windows.Forms.ComboBox comboBoxOffhandImbue;
        private System.Windows.Forms.ComboBox comboBoxMainhandImbue;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RadioButton radioButtonScryer;
        private System.Windows.Forms.RadioButton radioButtonAldor;
        private System.Windows.Forms.Label labelNumberOfFerociousInspirations;
        private System.Windows.Forms.Label labelBloodlustUptime;
        private System.Windows.Forms.Label labelExposeWeakness;
        private System.Windows.Forms.TrackBar trackBarNumberOfFerociousInspirations;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TrackBar trackBarBloodlustUptime;
        private System.Windows.Forms.TrackBar trackBarExposeWeakness;
        private System.Windows.Forms.TrackBar trackBarTargetArmor;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBoxTargetLevel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox gbGlyphs;
        private System.Windows.Forms.CheckBox chbGlyphLL;
        private System.Windows.Forms.CheckBox chbGlyphSS;
        private System.Windows.Forms.CheckBox chbGlyphWF;
        private System.Windows.Forms.CheckBox chbGlyphShocking;
        private System.Windows.Forms.CheckBox chbGlyphLS;
        private System.Windows.Forms.CheckBox chbGlyphLB;
        private System.Windows.Forms.CheckBox chbGlyphFT;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Button btnEnhSim;
        private System.Windows.Forms.TextBox tbModuleNotes;
        private System.Windows.Forms.CheckBox chbBaseStatOption;
        private System.Windows.Forms.CheckBox chbGlyphFS;

    }
}
