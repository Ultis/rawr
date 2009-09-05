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
            this.trkJoW = new System.Windows.Forms.TrackBar();
            this.trkReplenishment = new System.Windows.Forms.TrackBar();
            this.trkFightLength = new System.Windows.Forms.TrackBar();
            this.trkDelay = new System.Windows.Forms.TrackBar();
            this.chbUseInfernal = new System.Windows.Forms.CheckBox();
            this.tbAffEffects = new System.Windows.Forms.TextBox();
            this.lblAfflictionEffects = new System.Windows.Forms.Label();
            this.chbImmoAura = new System.Windows.Forms.CheckBox();
            this.tabEvents = new System.Windows.Forms.TabPage();
            this.textEvents = new System.Windows.Forms.TextBox();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabFight = new System.Windows.Forms.TabPage();
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
            this.bChangePriority = new System.Windows.Forms.Button();
            this.lsSpellPriority = new System.Windows.Forms.ListBox();
            this.tabPet = new System.Windows.Forms.TabPage();
            this.label1 = new System.Windows.Forms.Label();
            this.cbPet = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.trkJoW)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkReplenishment)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkFightLength)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkDelay)).BeginInit();
            this.tabEvents.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabFight.SuspendLayout();
            this.tabChar.SuspendLayout();
            this.gbSpellPriority.SuspendLayout();
            this.tabPet.SuspendLayout();
            this.SuspendLayout();
            // 
            // trkJoW
            // 
            this.trkJoW.Location = new System.Drawing.Point(9, 308);
            this.trkJoW.Maximum = 100;
            this.trkJoW.Name = "trkJoW";
            this.trkJoW.Size = new System.Drawing.Size(270, 45);
            this.trkJoW.TabIndex = 66;
            this.trkJoW.TickFrequency = 5;
            this.toolTip1.SetToolTip(this.trkJoW, "Uptime of Judgment of Wisdom on Target.");
            this.trkJoW.Scroll += new System.EventHandler(this.trkJoW_Scroll);
            // 
            // trkReplenishment
            // 
            this.trkReplenishment.Location = new System.Drawing.Point(9, 260);
            this.trkReplenishment.Maximum = 100;
            this.trkReplenishment.Name = "trkReplenishment";
            this.trkReplenishment.Size = new System.Drawing.Size(270, 45);
            this.trkReplenishment.TabIndex = 64;
            this.trkReplenishment.TickFrequency = 5;
            this.toolTip1.SetToolTip(this.trkReplenishment, "How much uptime do you expect on Replenishment?");
            this.trkReplenishment.Scroll += new System.EventHandler(this.trkReplenishment_Scroll);
            // 
            // trkFightLength
            // 
            this.trkFightLength.Location = new System.Drawing.Point(9, 162);
            this.trkFightLength.Maximum = 20;
            this.trkFightLength.Minimum = 1;
            this.trkFightLength.Name = "trkFightLength";
            this.trkFightLength.Size = new System.Drawing.Size(270, 45);
            this.trkFightLength.TabIndex = 62;
            this.toolTip1.SetToolTip(this.trkFightLength, "Estimated duration of the fight. Important for sustainability calculations.");
            this.trkFightLength.Value = 1;
            this.trkFightLength.Scroll += new System.EventHandler(this.trkFightLength_Scroll);
            // 
            // trkDelay
            // 
            this.trkDelay.Location = new System.Drawing.Point(9, 211);
            this.trkDelay.Maximum = 1000;
            this.trkDelay.Name = "trkDelay";
            this.trkDelay.Size = new System.Drawing.Size(270, 45);
            this.trkDelay.TabIndex = 69;
            this.trkDelay.TickFrequency = 50;
            this.toolTip1.SetToolTip(this.trkDelay, "Change this value to adjust how much lag from latency, finger twitching and gener" +
                    "al brain farts you expect to have.");
            this.trkDelay.Scroll += new System.EventHandler(this.trkDelay_Scroll);
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
            // tbAffEffects
            // 
            this.tbAffEffects.Location = new System.Drawing.Point(179, 65);
            this.tbAffEffects.Name = "tbAffEffects";
            this.tbAffEffects.Size = new System.Drawing.Size(100, 20);
            this.tbAffEffects.TabIndex = 74;
            this.tbAffEffects.Text = "5";
            this.toolTip1.SetToolTip(this.tbAffEffects, "Enter how many Affliction effects are on the target except your own.");
            this.tbAffEffects.TextChanged += new System.EventHandler(this.tbAffEffects_Changed);
            this.tbAffEffects.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbAffEffects_KeyPress);
            // 
            // lblAfflictionEffects
            // 
            this.lblAfflictionEffects.AutoSize = true;
            this.lblAfflictionEffects.Location = new System.Drawing.Point(6, 69);
            this.lblAfflictionEffects.Name = "lblAfflictionEffects";
            this.lblAfflictionEffects.Size = new System.Drawing.Size(148, 13);
            this.lblAfflictionEffects.TabIndex = 73;
            this.lblAfflictionEffects.Text = "Affliction effects on the target:";
            this.toolTip1.SetToolTip(this.lblAfflictionEffects, "Excluding your own Affliction effects");
            // 
            // chbImmoAura
            // 
            this.chbImmoAura.AutoSize = true;
            this.chbImmoAura.Location = new System.Drawing.Point(18, 206);
            this.chbImmoAura.Name = "chbImmoAura";
            this.chbImmoAura.Size = new System.Drawing.Size(123, 17);
            this.chbImmoAura.TabIndex = 67;
            this.chbImmoAura.Text = "Use Immolation Aura";
            this.toolTip1.SetToolTip(this.chbImmoAura, "Check if you want to use Immolation Aura when in Metamorphosis");
            this.chbImmoAura.UseVisualStyleBackColor = true;
            this.chbImmoAura.CheckedChanged += new System.EventHandler(this.chbImmoAura_CheckedChanged);
            // 
            // tabEvents
            // 
            this.tabEvents.Controls.Add(this.textEvents);
            this.tabEvents.Location = new System.Drawing.Point(4, 22);
            this.tabEvents.Name = "tabEvents";
            this.tabEvents.Padding = new System.Windows.Forms.Padding(3);
            this.tabEvents.Size = new System.Drawing.Size(292, 579);
            this.tabEvents.TabIndex = 4;
            this.tabEvents.Text = "Events:";
            this.tabEvents.UseVisualStyleBackColor = true;
            // 
            // textEvents
            // 
            this.textEvents.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textEvents.Location = new System.Drawing.Point(3, 3);
            this.textEvents.Multiline = true;
            this.textEvents.Name = "textEvents";
            this.textEvents.ReadOnly = true;
            this.textEvents.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textEvents.Size = new System.Drawing.Size(286, 573);
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
            this.tabFight.Controls.Add(this.tbAffEffects);
            this.tabFight.Controls.Add(this.lblAfflictionEffects);
            this.tabFight.Controls.Add(this.lblDelay);
            this.tabFight.Controls.Add(this.trkJoW);
            this.tabFight.Controls.Add(this.lblJoW);
            this.tabFight.Controls.Add(this.lblManaPotions);
            this.tabFight.Controls.Add(this.trkReplenishment);
            this.tabFight.Controls.Add(this.cbManaAmt);
            this.tabFight.Controls.Add(this.lblReplenishment);
            this.tabFight.Controls.Add(this.trkFightLength);
            this.tabFight.Controls.Add(this.lblTargetLevel);
            this.tabFight.Controls.Add(this.cbTargetLevel);
            this.tabFight.Controls.Add(this.lblFightLength);
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
            this.lblDelay.Location = new System.Drawing.Point(6, 195);
            this.lblDelay.Name = "lblDelay";
            this.lblDelay.Size = new System.Drawing.Size(37, 13);
            this.lblDelay.TabIndex = 70;
            this.lblDelay.Text = "Delay:";
            // 
            // lblJoW
            // 
            this.lblJoW.AutoSize = true;
            this.lblJoW.Location = new System.Drawing.Point(6, 292);
            this.lblJoW.Name = "lblJoW";
            this.lblJoW.Size = new System.Drawing.Size(126, 13);
            this.lblJoW.TabIndex = 65;
            this.lblJoW.Text = "% Judgement of Wisdom:";
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
            this.lblReplenishment.Location = new System.Drawing.Point(6, 244);
            this.lblReplenishment.Name = "lblReplenishment";
            this.lblReplenishment.Size = new System.Drawing.Size(91, 13);
            this.lblReplenishment.TabIndex = 63;
            this.lblReplenishment.Text = "% Replenishment:";
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
            this.lblFightLength.Location = new System.Drawing.Point(6, 146);
            this.lblFightLength.Name = "lblFightLength";
            this.lblFightLength.Size = new System.Drawing.Size(69, 13);
            this.lblFightLength.TabIndex = 59;
            this.lblFightLength.Text = "Fight Length:";
            // 
            // tabChar
            // 
            this.tabChar.AutoScroll = true;
            this.tabChar.Controls.Add(this.chbImmoAura);
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
            this.gbSpellPriority.Controls.Add(this.bChangePriority);
            this.gbSpellPriority.Controls.Add(this.lsSpellPriority);
            this.gbSpellPriority.Location = new System.Drawing.Point(6, 16);
            this.gbSpellPriority.Name = "gbSpellPriority";
            this.gbSpellPriority.Size = new System.Drawing.Size(277, 184);
            this.gbSpellPriority.TabIndex = 53;
            this.gbSpellPriority.TabStop = false;
            this.gbSpellPriority.Text = "Spell Priority:";
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
            // lsSpellPriority
            // 
            this.lsSpellPriority.FormattingEnabled = true;
            this.lsSpellPriority.Location = new System.Drawing.Point(5, 20);
            this.lsSpellPriority.Name = "lsSpellPriority";
            this.lsSpellPriority.Size = new System.Drawing.Size(266, 121);
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
            ((System.ComponentModel.ISupportInitialize)(this.trkJoW)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkReplenishment)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkFightLength)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkDelay)).EndInit();
            this.tabEvents.ResumeLayout(false);
            this.tabEvents.PerformLayout();
            this.tabControl.ResumeLayout(false);
            this.tabFight.ResumeLayout(false);
            this.tabFight.PerformLayout();
            this.tabChar.ResumeLayout(false);
            this.tabChar.PerformLayout();
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
        private System.Windows.Forms.TrackBar trkJoW;
        private System.Windows.Forms.Label lblJoW;
        private System.Windows.Forms.Label lblManaPotions;
        private System.Windows.Forms.TrackBar trkReplenishment;
        private System.Windows.Forms.ComboBox cbManaAmt;
        private System.Windows.Forms.Label lblReplenishment;
        private System.Windows.Forms.TrackBar trkFightLength;
        private System.Windows.Forms.Label lblTargetLevel;
        private System.Windows.Forms.ComboBox cbTargetLevel;
        private System.Windows.Forms.Label lblFightLength;
        private System.Windows.Forms.TrackBar trkDelay;
        private System.Windows.Forms.TabPage tabChar;
        private System.Windows.Forms.TabPage tabPet;
        private System.Windows.Forms.GroupBox gbSpellPriority;
        private System.Windows.Forms.Button bChangePriority;
        private System.Windows.Forms.ListBox lsSpellPriority;
        private System.Windows.Forms.ComboBox cbPet;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chbUseInfernal;
        private System.Windows.Forms.Label lblAfflictionEffects;
        private System.Windows.Forms.TextBox tbAffEffects;
        private System.Windows.Forms.CheckBox chbImmoAura;
        private System.Windows.Forms.TabPage tabEvents;
        private System.Windows.Forms.TextBox textEvents;
    }
}