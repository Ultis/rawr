namespace Rawr.Warlock
{
    partial class CalculationOptionsPanelWarlock
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CalculationOptionsPanelWarlock));
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.trkJoW = new System.Windows.Forms.TrackBar();
            this.trkReplenishment = new System.Windows.Forms.TrackBar();
            this.trkFightLength = new System.Windows.Forms.TrackBar();
            this.trkFSR = new System.Windows.Forms.TrackBar();
            this.trkDelay = new System.Windows.Forms.TrackBar();
            this.trkSurvivability = new System.Windows.Forms.TrackBar();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabFight = new System.Windows.Forms.TabPage();
            this.lblDelay = new System.Windows.Forms.Label();
            this.lblFSR = new System.Windows.Forms.Label();
            this.lblJoW = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbManaAmt = new System.Windows.Forms.ComboBox();
            this.lblReplenishment = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.cbTargetLevel = new System.Windows.Forms.ComboBox();
            this.lblFightLength = new System.Windows.Forms.Label();
            this.tabChar = new System.Windows.Forms.TabPage();
            this.lblSurvivability = new System.Windows.Forms.Label();
            this.gbSpellPriority = new System.Windows.Forms.GroupBox();
            this.bChangePriority = new System.Windows.Forms.Button();
            this.lsSpellPriopity = new System.Windows.Forms.ListBox();
            this.tabPet = new System.Windows.Forms.TabPage();
            ((System.ComponentModel.ISupportInitialize)(this.trkJoW)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkReplenishment)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkFightLength)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkFSR)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkDelay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkSurvivability)).BeginInit();
            this.tabControl.SuspendLayout();
            this.tabFight.SuspendLayout();
            this.tabChar.SuspendLayout();
            this.gbSpellPriority.SuspendLayout();
            this.SuspendLayout();
            // 
            // trkJoW
            // 
            this.trkJoW.Location = new System.Drawing.Point(9, 274);
            this.trkJoW.Maximum = 100;
            this.trkJoW.Name = "trkJoW";
            this.trkJoW.Size = new System.Drawing.Size(270, 42);
            this.trkJoW.TabIndex = 66;
            this.trkJoW.TickFrequency = 5;
            this.toolTip1.SetToolTip(this.trkJoW, "Uptime of Judgment of Wisdom on Target.");
            this.trkJoW.Scroll += new System.EventHandler(this.trkJoW_Scroll);
            // 
            // trkReplenishment
            // 
            this.trkReplenishment.Location = new System.Drawing.Point(9, 226);
            this.trkReplenishment.Maximum = 100;
            this.trkReplenishment.Name = "trkReplenishment";
            this.trkReplenishment.Size = new System.Drawing.Size(270, 42);
            this.trkReplenishment.TabIndex = 64;
            this.trkReplenishment.TickFrequency = 5;
            this.toolTip1.SetToolTip(this.trkReplenishment, "How much uptime do you expect on Replenishment?");
            this.trkReplenishment.Scroll += new System.EventHandler(this.trkReplenishment_Scroll);
            // 
            // trkFightLength
            // 
            this.trkFightLength.Location = new System.Drawing.Point(9, 128);
            this.trkFightLength.Maximum = 20;
            this.trkFightLength.Minimum = 1;
            this.trkFightLength.Name = "trkFightLength";
            this.trkFightLength.Size = new System.Drawing.Size(270, 42);
            this.trkFightLength.TabIndex = 62;
            this.toolTip1.SetToolTip(this.trkFightLength, "Estimated duration of the fight. Important for sustainability calculations.");
            this.trkFightLength.Value = 1;
            this.trkFightLength.Scroll += new System.EventHandler(this.trkFightLength_Scroll);
            // 
            // trkFSR
            // 
            this.trkFSR.Location = new System.Drawing.Point(9, 80);
            this.trkFSR.Maximum = 100;
            this.trkFSR.Name = "trkFSR";
            this.trkFSR.Size = new System.Drawing.Size(270, 42);
            this.trkFSR.TabIndex = 68;
            this.trkFSR.TickFrequency = 5;
            this.toolTip1.SetToolTip(this.trkFSR, resources.GetString("trkFSR.ToolTip"));
            this.trkFSR.Scroll += new System.EventHandler(this.trkFSR_Scroll);
            // 
            // trkDelay
            // 
            this.trkDelay.Location = new System.Drawing.Point(9, 177);
            this.trkDelay.Maximum = 1000;
            this.trkDelay.Name = "trkDelay";
            this.trkDelay.Size = new System.Drawing.Size(270, 42);
            this.trkDelay.TabIndex = 69;
            this.trkDelay.TickFrequency = 50;
            this.toolTip1.SetToolTip(this.trkDelay, "Change this value to adjust how much lag from latency, finger twitching and gener" +
                    "al brain farts you expect to have.");
            this.trkDelay.Scroll += new System.EventHandler(this.trkDelay_Scroll);
            // 
            // trkSurvivability
            // 
            this.trkSurvivability.Location = new System.Drawing.Point(6, 30);
            this.trkSurvivability.Maximum = 100;
            this.trkSurvivability.Name = "trkSurvivability";
            this.trkSurvivability.Size = new System.Drawing.Size(270, 42);
            this.trkSurvivability.TabIndex = 54;
            this.trkSurvivability.TickFrequency = 5;
            this.toolTip1.SetToolTip(this.trkSurvivability, "Tell Rawr how much you value your life. Use 0-5% for PvE, 10-15% for PvP");
            this.trkSurvivability.Scroll += new System.EventHandler(this.trackBar1_Scroll);
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabFight);
            this.tabControl.Controls.Add(this.tabChar);
            this.tabControl.Controls.Add(this.tabPet);
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(300, 605);
            this.tabControl.TabIndex = 57;
            // 
            // tabFight
            // 
            this.tabFight.Controls.Add(this.lblDelay);
            this.tabFight.Controls.Add(this.lblFSR);
            this.tabFight.Controls.Add(this.trkJoW);
            this.tabFight.Controls.Add(this.lblJoW);
            this.tabFight.Controls.Add(this.label4);
            this.tabFight.Controls.Add(this.trkReplenishment);
            this.tabFight.Controls.Add(this.cmbManaAmt);
            this.tabFight.Controls.Add(this.lblReplenishment);
            this.tabFight.Controls.Add(this.trkFightLength);
            this.tabFight.Controls.Add(this.label15);
            this.tabFight.Controls.Add(this.cbTargetLevel);
            this.tabFight.Controls.Add(this.lblFightLength);
            this.tabFight.Controls.Add(this.trkFSR);
            this.tabFight.Controls.Add(this.trkDelay);
            this.tabFight.Location = new System.Drawing.Point(4, 22);
            this.tabFight.Name = "tabFight";
            this.tabFight.Padding = new System.Windows.Forms.Padding(3);
            this.tabFight.Size = new System.Drawing.Size(292, 579);
            this.tabFight.TabIndex = 1;
            this.tabFight.Text = "Fight";
            this.tabFight.UseVisualStyleBackColor = true;
            // 
            // lblDelay
            // 
            this.lblDelay.AutoSize = true;
            this.lblDelay.Location = new System.Drawing.Point(6, 161);
            this.lblDelay.Name = "lblDelay";
            this.lblDelay.Size = new System.Drawing.Size(34, 13);
            this.lblDelay.TabIndex = 70;
            this.lblDelay.Text = "Delay";
            // 
            // lblFSR
            // 
            this.lblFSR.AutoSize = true;
            this.lblFSR.Location = new System.Drawing.Point(6, 64);
            this.lblFSR.Name = "lblFSR";
            this.lblFSR.Size = new System.Drawing.Size(101, 13);
            this.lblFSR.TabIndex = 67;
            this.lblFSR.Text = "% time spent in FSR";
            // 
            // lblJoW
            // 
            this.lblJoW.AutoSize = true;
            this.lblJoW.Location = new System.Drawing.Point(-1, 258);
            this.lblJoW.Name = "lblJoW";
            this.lblJoW.Size = new System.Drawing.Size(123, 13);
            this.lblJoW.TabIndex = 65;
            this.lblJoW.Text = "% Judgement of Wisdom";
            this.lblJoW.Click += new System.EventHandler(this.lblJoW_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 41);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(75, 13);
            this.label4.TabIndex = 58;
            this.label4.Text = "Mana Potions:";
            // 
            // cmbManaAmt
            // 
            this.cmbManaAmt.DisplayMember = "2400";
            this.cmbManaAmt.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbManaAmt.Items.AddRange(new object[] {
            "(None) 0",
            "(Major) 1350-2250, Avg 1800",
            "(Mad) 1650-2750, Avg 2200",
            "(Super) 1800-3000, Avg 2400",
            "(Runic) 4200-4400, Avg 4300"});
            this.cmbManaAmt.Location = new System.Drawing.Point(87, 41);
            this.cmbManaAmt.Name = "cmbManaAmt";
            this.cmbManaAmt.Size = new System.Drawing.Size(192, 21);
            this.cmbManaAmt.TabIndex = 57;
            this.cmbManaAmt.ValueMember = "2400";
            this.cmbManaAmt.SelectedIndexChanged += new System.EventHandler(this.cmbManaAmt_SelectedIndexChanged_1);
            // 
            // lblReplenishment
            // 
            this.lblReplenishment.AutoSize = true;
            this.lblReplenishment.Location = new System.Drawing.Point(6, 210);
            this.lblReplenishment.Name = "lblReplenishment";
            this.lblReplenishment.Size = new System.Drawing.Size(88, 13);
            this.lblReplenishment.TabIndex = 63;
            this.lblReplenishment.Text = "% Replenishment";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(6, 14);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(99, 13);
            this.label15.TabIndex = 61;
            this.label15.Text = "Relative Mob Level";
            // 
            // cbTargetLevel
            // 
            this.cbTargetLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbTargetLevel.FormattingEnabled = true;
            this.cbTargetLevel.Items.AddRange(new object[] {
            "+0",
            "+1",
            "+2 (PvP)",
            "+3 (Boss)"});
            this.cbTargetLevel.Location = new System.Drawing.Point(128, 14);
            this.cbTargetLevel.Name = "cbTargetLevel";
            this.cbTargetLevel.Size = new System.Drawing.Size(151, 21);
            this.cbTargetLevel.TabIndex = 60;
            this.cbTargetLevel.SelectedIndexChanged += new System.EventHandler(this.cbTargetLevel_SelectedIndexChanged);
            // 
            // lblFightLength
            // 
            this.lblFightLength.AutoSize = true;
            this.lblFightLength.Location = new System.Drawing.Point(6, 112);
            this.lblFightLength.Name = "lblFightLength";
            this.lblFightLength.Size = new System.Drawing.Size(66, 13);
            this.lblFightLength.TabIndex = 59;
            this.lblFightLength.Text = "Fight Length";
            // 
            // tabChar
            // 
            this.tabChar.Controls.Add(this.lblSurvivability);
            this.tabChar.Controls.Add(this.trkSurvivability);
            this.tabChar.Controls.Add(this.gbSpellPriority);
            this.tabChar.Location = new System.Drawing.Point(4, 22);
            this.tabChar.Name = "tabChar";
            this.tabChar.Padding = new System.Windows.Forms.Padding(3);
            this.tabChar.Size = new System.Drawing.Size(292, 579);
            this.tabChar.TabIndex = 2;
            this.tabChar.Text = "Character";
            this.tabChar.UseVisualStyleBackColor = true;
            // 
            // lblSurvivability
            // 
            this.lblSurvivability.AutoSize = true;
            this.lblSurvivability.Location = new System.Drawing.Point(3, 14);
            this.lblSurvivability.Name = "lblSurvivability";
            this.lblSurvivability.Size = new System.Drawing.Size(121, 13);
            this.lblSurvivability.TabIndex = 55;
            this.lblSurvivability.Text = "% Focus on Survivability";
            // 
            // gbSpellPriority
            // 
            this.gbSpellPriority.Controls.Add(this.bChangePriority);
            this.gbSpellPriority.Controls.Add(this.lsSpellPriopity);
            this.gbSpellPriority.Location = new System.Drawing.Point(6, 114);
            this.gbSpellPriority.Name = "gbSpellPriority";
            this.gbSpellPriority.Size = new System.Drawing.Size(270, 186);
            this.gbSpellPriority.TabIndex = 53;
            this.gbSpellPriority.TabStop = false;
            this.gbSpellPriority.Text = "Spell Priority";
            // 
            // bChangePriority
            // 
            this.bChangePriority.Location = new System.Drawing.Point(5, 151);
            this.bChangePriority.Name = "bChangePriority";
            this.bChangePriority.Size = new System.Drawing.Size(75, 23);
            this.bChangePriority.TabIndex = 11;
            this.bChangePriority.Text = "Change";
            this.bChangePriority.UseVisualStyleBackColor = true;
            this.bChangePriority.Click += new System.EventHandler(this.bChangePriority_Click);
            // 
            // lsSpellPriopity
            // 
            this.lsSpellPriopity.FormattingEnabled = true;
            this.lsSpellPriopity.Location = new System.Drawing.Point(5, 20);
            this.lsSpellPriopity.Name = "lsSpellPriopity";
            this.lsSpellPriopity.Size = new System.Drawing.Size(259, 121);
            this.lsSpellPriopity.TabIndex = 10;
            // 
            // tabPet
            // 
            this.tabPet.Location = new System.Drawing.Point(4, 22);
            this.tabPet.Name = "tabPet";
            this.tabPet.Padding = new System.Windows.Forms.Padding(3);
            this.tabPet.Size = new System.Drawing.Size(292, 579);
            this.tabPet.TabIndex = 3;
            this.tabPet.Text = "Pet";
            this.tabPet.UseVisualStyleBackColor = true;
            // 
            // CalculationOptionsPanelWarlock
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.tabControl);
            this.Name = "CalculationOptionsPanelWarlock";
            this.Size = new System.Drawing.Size(300, 605);
            ((System.ComponentModel.ISupportInitialize)(this.trkJoW)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkReplenishment)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkFightLength)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkFSR)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkDelay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkSurvivability)).EndInit();
            this.tabControl.ResumeLayout(false);
            this.tabFight.ResumeLayout(false);
            this.tabFight.PerformLayout();
            this.tabChar.ResumeLayout(false);
            this.tabChar.PerformLayout();
            this.gbSpellPriority.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabFight;
        private System.Windows.Forms.Label lblDelay;
        private System.Windows.Forms.Label lblFSR;
        private System.Windows.Forms.TrackBar trkJoW;
        private System.Windows.Forms.Label lblJoW;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TrackBar trkReplenishment;
        private System.Windows.Forms.ComboBox cmbManaAmt;
        private System.Windows.Forms.Label lblReplenishment;
        private System.Windows.Forms.TrackBar trkFightLength;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.ComboBox cbTargetLevel;
        private System.Windows.Forms.Label lblFightLength;
        private System.Windows.Forms.TrackBar trkFSR;
        private System.Windows.Forms.TrackBar trkDelay;
        private System.Windows.Forms.TabPage tabChar;
        private System.Windows.Forms.TabPage tabPet;
        private System.Windows.Forms.Label lblSurvivability;
        private System.Windows.Forms.TrackBar trkSurvivability;
        private System.Windows.Forms.GroupBox gbSpellPriority;
        private System.Windows.Forms.Button bChangePriority;
        private System.Windows.Forms.ListBox lsSpellPriopity;
    }
}

/*namespace Rawr.Warlock
{
    partial class CalculationOptionsPanelWarlock
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
            this.comboBoxTargetLevel = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxLatency = new System.Windows.Forms.TextBox();
            this.comboBoxFillerSpell = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBoxCastedCurse = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.comboBoxPet = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.checkBoxPetSacrificed = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.checkBoxCastImmolate = new System.Windows.Forms.CheckBox();
            this.checkBoxCastCorruption = new System.Windows.Forms.CheckBox();
            this.checkBoxCastSiphonLife = new System.Windows.Forms.CheckBox();
            this.checkBoxCastUnstableAffliction = new System.Windows.Forms.CheckBox();
            this.checkBoxCastConflagrate = new System.Windows.Forms.CheckBox();
            this.checkBoxCastShadowburn = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.textBoxFightDuration = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.toolTipWarlock = new System.Windows.Forms.ToolTip(this.components);
            this.label8 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.textBoxDotGap = new System.Windows.Forms.TextBox();
            this.textBoxAfflictionDebuffs = new System.Windows.Forms.TextBox();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Target Level: ";
            // 
            // comboBoxTargetLevel
            // 
            this.comboBoxTargetLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxTargetLevel.FormattingEnabled = true;
            this.comboBoxTargetLevel.Items.AddRange(new object[] {
            "70",
            "71",
            "72",
            "73"});
            this.comboBoxTargetLevel.Location = new System.Drawing.Point(117, 36);
            this.comboBoxTargetLevel.Name = "comboBoxTargetLevel";
            this.comboBoxTargetLevel.Size = new System.Drawing.Size(88, 21);
            this.comboBoxTargetLevel.TabIndex = 1;
            this.comboBoxTargetLevel.SelectedIndexChanged += new System.EventHandler(this.comboBoxTargetLevel_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 13);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Latency: *";
            this.toolTipWarlock.SetToolTip(this.label3, "Effective latency in seconds. The value should include the benefit of built-in /s" +
                    "topcasting.");
            // 
            // textBoxLatency
            // 
            this.textBoxLatency.Location = new System.Drawing.Point(117, 10);
            this.textBoxLatency.Name = "textBoxLatency";
            this.textBoxLatency.Size = new System.Drawing.Size(88, 20);
            this.textBoxLatency.TabIndex = 7;
            this.textBoxLatency.Leave += new System.EventHandler(this.textBoxLatency_Leave);
            // 
            // comboBoxFillerSpell
            // 
            this.comboBoxFillerSpell.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxFillerSpell.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxFillerSpell.FormattingEnabled = true;
            this.comboBoxFillerSpell.Items.AddRange(new object[] {
            "Shadowbolt",
            "Incinerate"});
            this.comboBoxFillerSpell.Location = new System.Drawing.Point(114, 19);
            this.comboBoxFillerSpell.Name = "comboBoxFillerSpell";
            this.comboBoxFillerSpell.Size = new System.Drawing.Size(88, 21);
            this.comboBoxFillerSpell.TabIndex = 9;
            this.comboBoxFillerSpell.SelectedIndexChanged += new System.EventHandler(this.comboBoxFillerSpell_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Filler Spell:";
            // 
            // comboBoxCastedCurse
            // 
            this.comboBoxCastedCurse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxCastedCurse.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxCastedCurse.FormattingEnabled = true;
            this.comboBoxCastedCurse.Items.AddRange(new object[] {
            "CoA",
            "CoD",
            "CoE",
            "CoS",
            "CoR",
            "CoW",
            "CoT"});
            this.comboBoxCastedCurse.Location = new System.Drawing.Point(114, 46);
            this.comboBoxCastedCurse.Name = "comboBoxCastedCurse";
            this.comboBoxCastedCurse.Size = new System.Drawing.Size(88, 21);
            this.comboBoxCastedCurse.TabIndex = 11;
            this.comboBoxCastedCurse.SelectedIndexChanged += new System.EventHandler(this.comboBoxCastedCurse_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 49);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(73, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Casted Curse:";
            // 
            // comboBoxPet
            // 
            this.comboBoxPet.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.comboBoxPet.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxPet.FormattingEnabled = true;
            this.comboBoxPet.Items.AddRange(new object[] {
            "Succubus",
            "Felhunter",
            "Imp",
            "Voidwalker"});
            this.comboBoxPet.Location = new System.Drawing.Point(114, 19);
            this.comboBoxPet.Name = "comboBoxPet";
            this.comboBoxPet.Size = new System.Drawing.Size(88, 21);
            this.comboBoxPet.TabIndex = 13;
            this.comboBoxPet.SelectedIndexChanged += new System.EventHandler(this.comboBoxPet_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 22);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(26, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "Pet:";
            // 
            // checkBoxPetSacrificed
            // 
            this.checkBoxPetSacrificed.AutoSize = true;
            this.checkBoxPetSacrificed.Location = new System.Drawing.Point(10, 46);
            this.checkBoxPetSacrificed.Name = "checkBoxPetSacrificed";
            this.checkBoxPetSacrificed.Size = new System.Drawing.Size(73, 17);
            this.checkBoxPetSacrificed.TabIndex = 14;
            this.checkBoxPetSacrificed.Text = "Sacrificed";
            this.checkBoxPetSacrificed.UseVisualStyleBackColor = true;
            this.checkBoxPetSacrificed.CheckedChanged += new System.EventHandler(this.checkBoxPetSacrificed_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.comboBoxFillerSpell);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.comboBoxCastedCurse);
            this.groupBox2.Controls.Add(this.checkBoxCastImmolate);
            this.groupBox2.Controls.Add(this.checkBoxCastCorruption);
            this.groupBox2.Controls.Add(this.checkBoxCastSiphonLife);
            this.groupBox2.Controls.Add(this.checkBoxCastUnstableAffliction);
            this.groupBox2.Location = new System.Drawing.Point(3, 141);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(208, 119);
            this.groupBox2.TabIndex = 16;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Spell Choice";
            // 
            // checkBoxCastImmolate
            // 
            this.checkBoxCastImmolate.AutoSize = true;
            this.checkBoxCastImmolate.Location = new System.Drawing.Point(10, 73);
            this.checkBoxCastImmolate.Name = "checkBoxCastImmolate";
            this.checkBoxCastImmolate.Size = new System.Drawing.Size(68, 17);
            this.checkBoxCastImmolate.TabIndex = 12;
            this.checkBoxCastImmolate.Text = "Immolate";
            this.checkBoxCastImmolate.UseVisualStyleBackColor = true;
            this.checkBoxCastImmolate.CheckedChanged += new System.EventHandler(this.checkBoxCastImmolate_CheckedChanged);
            // 
            // checkBoxCastCorruption
            // 
            this.checkBoxCastCorruption.AutoSize = true;
            this.checkBoxCastCorruption.Location = new System.Drawing.Point(123, 73);
            this.checkBoxCastCorruption.Name = "checkBoxCastCorruption";
            this.checkBoxCastCorruption.Size = new System.Drawing.Size(74, 17);
            this.checkBoxCastCorruption.TabIndex = 13;
            this.checkBoxCastCorruption.Text = "Corruption";
            this.checkBoxCastCorruption.UseVisualStyleBackColor = true;
            this.checkBoxCastCorruption.CheckedChanged += new System.EventHandler(this.checkBoxCastCorruption_CheckedChanged);
            // 
            // checkBoxCastSiphonLife
            // 
            this.checkBoxCastSiphonLife.AutoSize = true;
            this.checkBoxCastSiphonLife.Location = new System.Drawing.Point(123, 96);
            this.checkBoxCastSiphonLife.Name = "checkBoxCastSiphonLife";
            this.checkBoxCastSiphonLife.Size = new System.Drawing.Size(79, 17);
            this.checkBoxCastSiphonLife.TabIndex = 14;
            this.checkBoxCastSiphonLife.Text = "Siphon Life";
            this.checkBoxCastSiphonLife.UseVisualStyleBackColor = true;
            this.checkBoxCastSiphonLife.CheckedChanged += new System.EventHandler(this.checkBoxCastSiphonLife_CheckedChanged);
            // 
            // checkBoxCastUnstableAffliction
            // 
            this.checkBoxCastUnstableAffliction.AutoSize = true;
            this.checkBoxCastUnstableAffliction.Location = new System.Drawing.Point(10, 96);
            this.checkBoxCastUnstableAffliction.Name = "checkBoxCastUnstableAffliction";
            this.checkBoxCastUnstableAffliction.Size = new System.Drawing.Size(111, 17);
            this.checkBoxCastUnstableAffliction.TabIndex = 15;
            this.checkBoxCastUnstableAffliction.Text = "Unstable Affliction";
            this.checkBoxCastUnstableAffliction.UseVisualStyleBackColor = true;
            this.checkBoxCastUnstableAffliction.CheckedChanged += new System.EventHandler(this.checkBoxCastUnstableAffliction_CheckedChanged);
            // 
            // checkBoxCastConflagrate
            // 
            this.checkBoxCastConflagrate.AutoSize = true;
            this.checkBoxCastConflagrate.Location = new System.Drawing.Point(116, 341);
            this.checkBoxCastConflagrate.Name = "checkBoxCastConflagrate";
            this.checkBoxCastConflagrate.Size = new System.Drawing.Size(80, 17);
            this.checkBoxCastConflagrate.TabIndex = 17;
            this.checkBoxCastConflagrate.Text = "Conflagrate";
            this.checkBoxCastConflagrate.UseVisualStyleBackColor = true;
            this.checkBoxCastConflagrate.Visible = false;
            this.checkBoxCastConflagrate.CheckedChanged += new System.EventHandler(this.checkBoxCastConflagrate_CheckedChanged);
            // 
            // checkBoxCastShadowburn
            // 
            this.checkBoxCastShadowburn.AutoSize = true;
            this.checkBoxCastShadowburn.Location = new System.Drawing.Point(13, 341);
            this.checkBoxCastShadowburn.Name = "checkBoxCastShadowburn";
            this.checkBoxCastShadowburn.Size = new System.Drawing.Size(86, 17);
            this.checkBoxCastShadowburn.TabIndex = 16;
            this.checkBoxCastShadowburn.Text = "Shadowburn";
            this.checkBoxCastShadowburn.UseVisualStyleBackColor = true;
            this.checkBoxCastShadowburn.Visible = false;
            this.checkBoxCastShadowburn.CheckedChanged += new System.EventHandler(this.checkBoxCastShadowburn_CheckedChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.comboBoxPet);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.checkBoxPetSacrificed);
            this.groupBox3.Location = new System.Drawing.Point(3, 266);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(208, 69);
            this.groupBox3.TabIndex = 17;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Pet Options";
            // 
            // textBoxFightDuration
            // 
            this.textBoxFightDuration.Location = new System.Drawing.Point(117, 63);
            this.textBoxFightDuration.Name = "textBoxFightDuration";
            this.textBoxFightDuration.Size = new System.Drawing.Size(88, 20);
            this.textBoxFightDuration.TabIndex = 19;
            this.textBoxFightDuration.Leave += new System.EventHandler(this.textBoxFightDuration_Leave);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(10, 66);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(90, 13);
            this.label9.TabIndex = 18;
            this.label9.Text = "Fight Duration (s):";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(10, 92);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(104, 13);
            this.label8.TabIndex = 27;
            this.label8.Text = "Average DoT Gap: *";
            this.toolTipWarlock.SetToolTip(this.label8, "Average damage over time gap");
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(10, 118);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(97, 13);
            this.label10.TabIndex = 28;
            this.label10.Text = "Affliction Debuffs: *";
            this.toolTipWarlock.SetToolTip(this.label10, "Number of Affliction debuffs on the target (for Soul Siphon)");
            // 
            // textBoxDotGap
            // 
            this.textBoxDotGap.Location = new System.Drawing.Point(117, 89);
            this.textBoxDotGap.Name = "textBoxDotGap";
            this.textBoxDotGap.Size = new System.Drawing.Size(88, 20);
            this.textBoxDotGap.TabIndex = 25;
            this.textBoxDotGap.Leave += new System.EventHandler(this.textBoxDotGap_Leave);
            // 
            // textBoxAfflictionDebuffs
            // 
            this.textBoxAfflictionDebuffs.Location = new System.Drawing.Point(117, 115);
            this.textBoxAfflictionDebuffs.Name = "textBoxAfflictionDebuffs";
            this.textBoxAfflictionDebuffs.Size = new System.Drawing.Size(88, 20);
            this.textBoxAfflictionDebuffs.TabIndex = 26;
            this.textBoxAfflictionDebuffs.Leave += new System.EventHandler(this.textBoxAfflictionDebuffs_Leave);
            // 
            // CalculationOptionsPanelWarlock
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.checkBoxCastConflagrate);
            this.Controls.Add(this.checkBoxCastShadowburn);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.textBoxAfflictionDebuffs);
            this.Controls.Add(this.textBoxDotGap);
            this.Controls.Add(this.textBoxFightDuration);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.textBoxLatency);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.comboBoxTargetLevel);
            this.Controls.Add(this.label1);
            this.Name = "CalculationOptionsPanelWarlock";
            this.Size = new System.Drawing.Size(218, 651);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox comboBoxTargetLevel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxLatency;
        private System.Windows.Forms.ComboBox comboBoxFillerSpell;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBoxCastedCurse;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboBoxPet;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox checkBoxPetSacrificed;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox checkBoxCastUnstableAffliction;
        private System.Windows.Forms.CheckBox checkBoxCastSiphonLife;
        private System.Windows.Forms.CheckBox checkBoxCastCorruption;
        private System.Windows.Forms.CheckBox checkBoxCastImmolate;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox textBoxFightDuration;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ToolTip toolTipWarlock;
        private System.Windows.Forms.TextBox textBoxDotGap;
        private System.Windows.Forms.TextBox textBoxAfflictionDebuffs;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.CheckBox checkBoxCastShadowburn;
        private System.Windows.Forms.CheckBox checkBoxCastConflagrate;
	}
}
*/