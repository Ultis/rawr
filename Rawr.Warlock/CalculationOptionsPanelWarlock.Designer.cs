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
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.chbUseInfernal = new System.Windows.Forms.CheckBox();
            this.lblAfflictionEffects = new System.Windows.Forms.Label();
            this.chkPTRMode = new System.Windows.Forms.CheckBox();
            this.updAfflictionEffects = new System.Windows.Forms.NumericUpDown();
            this.updLatency = new System.Windows.Forms.NumericUpDown();
            this.tabEvents = new System.Windows.Forms.TabPage();
            this.textEvents = new System.Windows.Forms.TextBox();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabFight = new System.Windows.Forms.TabPage();
            this.updJudgementOfWisdom = new System.Windows.Forms.NumericUpDown();
            this.updReplenishment = new System.Windows.Forms.NumericUpDown();
            this.updFightLength = new System.Windows.Forms.NumericUpDown();
            this.lblDelay = new System.Windows.Forms.Label();
            this.lblJoW = new System.Windows.Forms.Label();
            this.lblManaPotions = new System.Windows.Forms.Label();
            this.cbManaAmt = new System.Windows.Forms.ComboBox();
            this.lblReplenishment = new System.Windows.Forms.Label();
            this.lblTargetLevel = new System.Windows.Forms.Label();
            this.cbTargetLevel = new System.Windows.Forms.ComboBox();
            this.lblFightLength = new System.Windows.Forms.Label();
            this.tabChar = new System.Windows.Forms.TabPage();
            this.gbSpellPriority = new System.Windows.Forms.GroupBox();
            this.btnChangePriority = new System.Windows.Forms.Button();
            this.lsSpellPriority = new System.Windows.Forms.ListBox();
            this.tabPet = new System.Windows.Forms.TabPage();
            this.label1 = new System.Windows.Forms.Label();
            this.cbPet = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.updAfflictionEffects)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.updLatency)).BeginInit();
            this.tabEvents.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabFight.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.updJudgementOfWisdom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.updReplenishment)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.updFightLength)).BeginInit();
            this.tabChar.SuspendLayout();
            this.gbSpellPriority.SuspendLayout();
            this.tabPet.SuspendLayout();
            this.SuspendLayout();
            // 
            // chbUseInfernal
            // 
            this.chbUseInfernal.AutoSize = true;
            this.chbUseInfernal.Enabled = false;
            this.chbUseInfernal.Location = new System.Drawing.Point(9, 41);
            this.chbUseInfernal.Name = "chbUseInfernal";
            this.chbUseInfernal.Size = new System.Drawing.Size(83, 17);
            this.chbUseInfernal.TabIndex = 66;
            this.chbUseInfernal.Text = "Use Infernal";
            this.toolTip1.SetToolTip(this.chbUseInfernal, "Check if you want to use an Infernal during the last minute of the fight");
            this.chbUseInfernal.UseVisualStyleBackColor = true;
            this.chbUseInfernal.CheckedChanged += new System.EventHandler(this.chbUseInfernal_CheckedChanged);
            // 
            // lblAfflictionEffects
            // 
            this.lblAfflictionEffects.AutoSize = true;
            this.lblAfflictionEffects.Location = new System.Drawing.Point(6, 89);
            this.lblAfflictionEffects.Name = "lblAfflictionEffects";
            this.lblAfflictionEffects.Size = new System.Drawing.Size(148, 13);
            this.lblAfflictionEffects.TabIndex = 73;
            this.lblAfflictionEffects.Text = "Affliction effects on the target:";
            this.toolTip1.SetToolTip(this.lblAfflictionEffects, "Excluding your own Affliction effects");
            // 
            // chkPTRMode
            // 
            this.chkPTRMode.AutoSize = true;
            this.chkPTRMode.Location = new System.Drawing.Point(9, 556);
            this.chkPTRMode.Name = "chkPTRMode";
            this.chkPTRMode.Size = new System.Drawing.Size(77, 17);
            this.chkPTRMode.TabIndex = 75;
            this.chkPTRMode.Text = "PTR mode";
            this.toolTip1.SetToolTip(this.chkPTRMode, "Enable this option if you wish to test PTR changes.");
            this.chkPTRMode.UseVisualStyleBackColor = true;
            this.chkPTRMode.Visible = false;
            // 
            // updAfflictionEffects
            // 
            this.updAfflictionEffects.Location = new System.Drawing.Point(180, 87);
            this.updAfflictionEffects.Maximum = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.updAfflictionEffects.Name = "updAfflictionEffects";
            this.updAfflictionEffects.Size = new System.Drawing.Size(101, 20);
            this.updAfflictionEffects.TabIndex = 77;
            this.updAfflictionEffects.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.toolTip1.SetToolTip(this.updAfflictionEffects, "Enter the number of Affliction effects on the target (excluding your own)");
            this.updAfflictionEffects.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.updAfflictionEffects.ValueChanged += new System.EventHandler(this.updAfflictionEffects_ValueChanged);
            // 
            // updLatency
            // 
            this.updLatency.Location = new System.Drawing.Point(180, 187);
            this.updLatency.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.updLatency.Name = "updLatency";
            this.updLatency.Size = new System.Drawing.Size(100, 20);
            this.updLatency.TabIndex = 78;
            this.updLatency.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.toolTip1.SetToolTip(this.updLatency, "Change this value to adjust how much lag from latency, finger twitching and gener" +
                    "al brain farts you expect to have.");
            this.updLatency.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.updLatency.ValueChanged += new System.EventHandler(this.updLatency_ValueChanged);
            // 
            // tabEvents
            // 
            this.tabEvents.Controls.Add(this.textEvents);
            this.tabEvents.Location = new System.Drawing.Point(4, 22);
            this.tabEvents.Name = "tabEvents";
            this.tabEvents.Padding = new System.Windows.Forms.Padding(3);
            this.tabEvents.Size = new System.Drawing.Size(292, 579);
            this.tabEvents.TabIndex = 4;
            this.tabEvents.Text = "Events";
            this.tabEvents.UseVisualStyleBackColor = true;
            // 
            // textEvents
            // 
            this.textEvents.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textEvents.Location = new System.Drawing.Point(0, 0);
            this.textEvents.Multiline = true;
            this.textEvents.Name = "textEvents";
            this.textEvents.ReadOnly = true;
            this.textEvents.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textEvents.Size = new System.Drawing.Size(289, 576);
            this.textEvents.TabIndex = 70;
            this.textEvents.Text = "An overview of the events during combat.\r\n\r\nDouble-Click to refresh.";
            this.textEvents.DoubleClick += new System.EventHandler(this.textEvents_DoubleClick);
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabFight);
            this.tabControl.Controls.Add(this.tabChar);
            this.tabControl.Controls.Add(this.tabPet);
            this.tabControl.Controls.Add(this.tabEvents);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(300, 605);
            this.tabControl.TabIndex = 57;
            // 
            // tabFight
            // 
            this.tabFight.Controls.Add(this.updJudgementOfWisdom);
            this.tabFight.Controls.Add(this.updReplenishment);
            this.tabFight.Controls.Add(this.updLatency);
            this.tabFight.Controls.Add(this.updAfflictionEffects);
            this.tabFight.Controls.Add(this.updFightLength);
            this.tabFight.Controls.Add(this.chkPTRMode);
            this.tabFight.Controls.Add(this.lblAfflictionEffects);
            this.tabFight.Controls.Add(this.lblDelay);
            this.tabFight.Controls.Add(this.lblJoW);
            this.tabFight.Controls.Add(this.lblManaPotions);
            this.tabFight.Controls.Add(this.cbManaAmt);
            this.tabFight.Controls.Add(this.lblReplenishment);
            this.tabFight.Controls.Add(this.lblTargetLevel);
            this.tabFight.Controls.Add(this.cbTargetLevel);
            this.tabFight.Controls.Add(this.lblFightLength);
            this.tabFight.Location = new System.Drawing.Point(4, 22);
            this.tabFight.Name = "tabFight";
            this.tabFight.Padding = new System.Windows.Forms.Padding(3);
            this.tabFight.Size = new System.Drawing.Size(292, 579);
            this.tabFight.TabIndex = 1;
            this.tabFight.Text = "Fight";
            this.tabFight.UseVisualStyleBackColor = true;
            // 
            // updJudgementOfWisdom
            // 
            this.updJudgementOfWisdom.DecimalPlaces = 2;
            this.updJudgementOfWisdom.Location = new System.Drawing.Point(180, 287);
            this.updJudgementOfWisdom.Name = "updJudgementOfWisdom";
            this.updJudgementOfWisdom.Size = new System.Drawing.Size(99, 20);
            this.updJudgementOfWisdom.TabIndex = 80;
            this.updJudgementOfWisdom.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.updJudgementOfWisdom.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.updJudgementOfWisdom.ValueChanged += new System.EventHandler(this.updJudgementOfWisdom_ValueChanged);
            // 
            // updReplenishment
            // 
            this.updReplenishment.DecimalPlaces = 2;
            this.updReplenishment.Location = new System.Drawing.Point(180, 237);
            this.updReplenishment.Name = "updReplenishment";
            this.updReplenishment.Size = new System.Drawing.Size(99, 20);
            this.updReplenishment.TabIndex = 79;
            this.updReplenishment.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.updReplenishment.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.updReplenishment.ValueChanged += new System.EventHandler(this.updReplenishment_ValueChanged);
            // 
            // updFightLength
            // 
            this.updFightLength.Location = new System.Drawing.Point(180, 137);
            this.updFightLength.Maximum = new decimal(new int[] {
            300,
            0,
            0,
            0});
            this.updFightLength.Name = "updFightLength";
            this.updFightLength.Size = new System.Drawing.Size(100, 20);
            this.updFightLength.TabIndex = 76;
            this.updFightLength.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.updFightLength.Value = new decimal(new int[] {
            300,
            0,
            0,
            0});
            this.updFightLength.ValueChanged += new System.EventHandler(this.updFightLength_ValueChanged);
            // 
            // lblDelay
            // 
            this.lblDelay.AutoSize = true;
            this.lblDelay.Location = new System.Drawing.Point(6, 189);
            this.lblDelay.Name = "lblDelay";
            this.lblDelay.Size = new System.Drawing.Size(113, 13);
            this.lblDelay.TabIndex = 70;
            this.lblDelay.Text = "Latency: (milliseconds)";
            // 
            // lblJoW
            // 
            this.lblJoW.AutoSize = true;
            this.lblJoW.Location = new System.Drawing.Point(8, 287);
            this.lblJoW.Name = "lblJoW";
            this.lblJoW.Size = new System.Drawing.Size(166, 13);
            this.lblJoW.TabIndex = 65;
            this.lblJoW.Text = "Judgement of Wisdom uptime (%):";
            // 
            // lblManaPotions
            // 
            this.lblManaPotions.AutoSize = true;
            this.lblManaPotions.Location = new System.Drawing.Point(6, 41);
            this.lblManaPotions.Name = "lblManaPotions";
            this.lblManaPotions.Size = new System.Drawing.Size(75, 13);
            this.lblManaPotions.TabIndex = 58;
            this.lblManaPotions.Text = "Mana Potions:";
            // 
            // cbManaAmt
            // 
            this.cbManaAmt.DisplayMember = "2400";
            this.cbManaAmt.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbManaAmt.Items.AddRange(new object[] {
            "(None) 0",
            "(Major) 1350-2250, Avg 1800",
            "(Mad) 1650-2750, Avg 2200",
            "(Super) 1800-3000, Avg 2400",
            "(Runic) 4200-4400, Avg 4300"});
            this.cbManaAmt.Location = new System.Drawing.Point(87, 37);
            this.cbManaAmt.Name = "cbManaAmt";
            this.cbManaAmt.Size = new System.Drawing.Size(192, 21);
            this.cbManaAmt.TabIndex = 57;
            this.cbManaAmt.ValueMember = "2400";
            this.cbManaAmt.SelectedIndexChanged += new System.EventHandler(this.cmbManaAmt_SelectedIndexChanged);
            // 
            // lblReplenishment
            // 
            this.lblReplenishment.AutoSize = true;
            this.lblReplenishment.Location = new System.Drawing.Point(6, 239);
            this.lblReplenishment.Name = "lblReplenishment";
            this.lblReplenishment.Size = new System.Drawing.Size(131, 13);
            this.lblReplenishment.TabIndex = 63;
            this.lblReplenishment.Text = "Replenishment uptime (%):";
            // 
            // lblTargetLevel
            // 
            this.lblTargetLevel.AutoSize = true;
            this.lblTargetLevel.Location = new System.Drawing.Point(6, 14);
            this.lblTargetLevel.Name = "lblTargetLevel";
            this.lblTargetLevel.Size = new System.Drawing.Size(70, 13);
            this.lblTargetLevel.TabIndex = 61;
            this.lblTargetLevel.Text = "Target Level:";
            // 
            // cbTargetLevel
            // 
            this.cbTargetLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbTargetLevel.FormattingEnabled = true;
            this.cbTargetLevel.Items.AddRange(new object[] {
            "80",
            "81",
            "82",
            "83"});
            this.cbTargetLevel.Location = new System.Drawing.Point(128, 10);
            this.cbTargetLevel.Name = "cbTargetLevel";
            this.cbTargetLevel.Size = new System.Drawing.Size(151, 21);
            this.cbTargetLevel.TabIndex = 60;
            this.cbTargetLevel.SelectedIndexChanged += new System.EventHandler(this.cbTargetLevel_SelectedIndexChanged);
            // 
            // lblFightLength
            // 
            this.lblFightLength.AutoSize = true;
            this.lblFightLength.Location = new System.Drawing.Point(6, 139);
            this.lblFightLength.Name = "lblFightLength";
            this.lblFightLength.Size = new System.Drawing.Size(118, 13);
            this.lblFightLength.TabIndex = 59;
            this.lblFightLength.Text = "Fight Length: (seconds)";
            // 
            // tabChar
            // 
            this.tabChar.AutoScroll = true;
            this.tabChar.Controls.Add(this.gbSpellPriority);
            this.tabChar.Location = new System.Drawing.Point(4, 22);
            this.tabChar.Name = "tabChar";
            this.tabChar.Padding = new System.Windows.Forms.Padding(3);
            this.tabChar.Size = new System.Drawing.Size(292, 579);
            this.tabChar.TabIndex = 2;
            this.tabChar.Text = "Character";
            this.tabChar.UseVisualStyleBackColor = true;
            // 
            // gbSpellPriority
            // 
            this.gbSpellPriority.Controls.Add(this.btnChangePriority);
            this.gbSpellPriority.Controls.Add(this.lsSpellPriority);
            this.gbSpellPriority.Location = new System.Drawing.Point(6, 16);
            this.gbSpellPriority.Name = "gbSpellPriority";
            this.gbSpellPriority.Size = new System.Drawing.Size(277, 184);
            this.gbSpellPriority.TabIndex = 53;
            this.gbSpellPriority.TabStop = false;
            this.gbSpellPriority.Text = "Spell Priority:";
            // 
            // btnChangePriority
            // 
            this.btnChangePriority.Location = new System.Drawing.Point(7, 148);
            this.btnChangePriority.Name = "btnChangePriority";
            this.btnChangePriority.Size = new System.Drawing.Size(75, 23);
            this.btnChangePriority.TabIndex = 11;
            this.btnChangePriority.Text = "Change";
            this.btnChangePriority.UseVisualStyleBackColor = true;
            this.btnChangePriority.Click += new System.EventHandler(this.btnChangePriority_Click);
            // 
            // lsSpellPriority
            // 
            this.lsSpellPriority.FormattingEnabled = true;
            this.lsSpellPriority.Location = new System.Drawing.Point(5, 33);
            this.lsSpellPriority.Name = "lsSpellPriority";
            this.lsSpellPriority.Size = new System.Drawing.Size(266, 108);
            this.lsSpellPriority.TabIndex = 10;
            // 
            // tabPet
            // 
            this.tabPet.Controls.Add(this.chbUseInfernal);
            this.tabPet.Controls.Add(this.label1);
            this.tabPet.Controls.Add(this.cbPet);
            this.tabPet.Location = new System.Drawing.Point(4, 22);
            this.tabPet.Name = "tabPet";
            this.tabPet.Padding = new System.Windows.Forms.Padding(3);
            this.tabPet.Size = new System.Drawing.Size(292, 579);
            this.tabPet.TabIndex = 3;
            this.tabPet.Text = "Pet";
            this.tabPet.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 13);
            this.label1.TabIndex = 62;
            this.label1.Text = "Used Pet";
            // 
            // cbPet
            // 
            this.cbPet.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPet.FormattingEnabled = true;
            this.cbPet.Items.AddRange(new object[] {
            "None",
            "Imp",
            "Felhunter",
            "Felguard",
            "Succubus",
            "Voidwalker",
            "Doomguard"});
            this.cbPet.Location = new System.Drawing.Point(63, 14);
            this.cbPet.Name = "cbPet";
            this.cbPet.Size = new System.Drawing.Size(151, 21);
            this.cbPet.TabIndex = 61;
            this.cbPet.SelectedIndexChanged += new System.EventHandler(this.cbPet_SelectedIndexChanged);
            // 
            // CalculationOptionsPanelWarlock
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl);
            this.Name = "CalculationOptionsPanelWarlock";
            this.Size = new System.Drawing.Size(300, 605);
            ((System.ComponentModel.ISupportInitialize)(this.updAfflictionEffects)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.updLatency)).EndInit();
            this.tabEvents.ResumeLayout(false);
            this.tabEvents.PerformLayout();
            this.tabControl.ResumeLayout(false);
            this.tabFight.ResumeLayout(false);
            this.tabFight.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.updJudgementOfWisdom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.updReplenishment)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.updFightLength)).EndInit();
            this.tabChar.ResumeLayout(false);
            this.gbSpellPriority.ResumeLayout(false);
            this.tabPet.ResumeLayout(false);
            this.tabPet.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabFight;
        private System.Windows.Forms.Label lblDelay;
        private System.Windows.Forms.Label lblJoW;
        private System.Windows.Forms.Label lblManaPotions;
        private System.Windows.Forms.ComboBox cbManaAmt;
        private System.Windows.Forms.Label lblReplenishment;
        private System.Windows.Forms.Label lblTargetLevel;
        private System.Windows.Forms.ComboBox cbTargetLevel;
        private System.Windows.Forms.Label lblFightLength;
        private System.Windows.Forms.TabPage tabChar;
        private System.Windows.Forms.TabPage tabPet;
        private System.Windows.Forms.GroupBox gbSpellPriority;
        private System.Windows.Forms.ListBox lsSpellPriority;
        private System.Windows.Forms.ComboBox cbPet;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chbUseInfernal;
        private System.Windows.Forms.Label lblAfflictionEffects;
        private System.Windows.Forms.TabPage tabEvents;
        private System.Windows.Forms.TextBox textEvents;
        private System.Windows.Forms.CheckBox chkPTRMode;
        private System.Windows.Forms.Button btnChangePriority;
        private System.Windows.Forms.NumericUpDown updAfflictionEffects;
        private System.Windows.Forms.NumericUpDown updFightLength;
        private System.Windows.Forms.NumericUpDown updLatency;
        private System.Windows.Forms.NumericUpDown updJudgementOfWisdom;
        private System.Windows.Forms.NumericUpDown updReplenishment;
    }
}