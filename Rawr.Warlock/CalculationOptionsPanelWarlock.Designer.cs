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
            this.chbUseDoomguard = new System.Windows.Forms.CheckBox();
            this.tbAffEffects = new System.Windows.Forms.TextBox();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabFight = new System.Windows.Forms.TabPage();
            this.label2 = new System.Windows.Forms.Label();
            this.lblDelay = new System.Windows.Forms.Label();
            this.lblFSR = new System.Windows.Forms.Label();
            this.lblJoW = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cbManaAmt = new System.Windows.Forms.ComboBox();
            this.lblReplenishment = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.cbTargetLevel = new System.Windows.Forms.ComboBox();
            this.lblFightLength = new System.Windows.Forms.Label();
            this.tabChar = new System.Windows.Forms.TabPage();
            this.gbGlyphs = new System.Windows.Forms.GroupBox();
            this.chbGlyphCorruption = new System.Windows.Forms.CheckBox();
            this.chbGlyphSearingPain = new System.Windows.Forms.CheckBox();
            this.chbGlyphShadowburn = new System.Windows.Forms.CheckBox();
            this.chbGlyphSB = new System.Windows.Forms.CheckBox();
            this.chbGlyphSiphonLife = new System.Windows.Forms.CheckBox();
            this.chbGlyphUA = new System.Windows.Forms.CheckBox();
            this.chbGlyphImp = new System.Windows.Forms.CheckBox();
            this.chbGlyphImmolate = new System.Windows.Forms.CheckBox();
            this.chbGlyphFelguard = new System.Windows.Forms.CheckBox();
            this.chbGlyphCoA = new System.Windows.Forms.CheckBox();
            this.chbGlyphConflag = new System.Windows.Forms.CheckBox();
            this.lblSurvivability = new System.Windows.Forms.Label();
            this.gbSpellPriority = new System.Windows.Forms.GroupBox();
            this.bChangePriority = new System.Windows.Forms.Button();
            this.lsSpellPriopity = new System.Windows.Forms.ListBox();
            this.tabPet = new System.Windows.Forms.TabPage();
            this.label1 = new System.Windows.Forms.Label();
            this.cbPet = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.trkJoW)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkReplenishment)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkFightLength)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkFSR)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkDelay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkSurvivability)).BeginInit();
            this.tabControl.SuspendLayout();
            this.tabFight.SuspendLayout();
            this.tabChar.SuspendLayout();
            this.gbGlyphs.SuspendLayout();
            this.gbSpellPriority.SuspendLayout();
            this.tabPet.SuspendLayout();
            this.SuspendLayout();
            // 
            // trkJoW
            // 
            this.trkJoW.Location = new System.Drawing.Point(9, 308);
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
            this.trkReplenishment.Location = new System.Drawing.Point(9, 260);
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
            this.trkFightLength.Location = new System.Drawing.Point(9, 162);
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
            this.trkFSR.Location = new System.Drawing.Point(9, 114);
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
            this.trkDelay.Location = new System.Drawing.Point(9, 211);
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
            // chbUseDoomguard
            // 
            this.chbUseDoomguard.AutoSize = true;
            this.chbUseDoomguard.Location = new System.Drawing.Point(33, 42);
            this.chbUseDoomguard.Name = "chbUseDoomguard";
            this.chbUseDoomguard.Size = new System.Drawing.Size(103, 17);
            this.chbUseDoomguard.TabIndex = 66;
            this.chbUseDoomguard.Text = "Use Doomguard";
            this.toolTip1.SetToolTip(this.chbUseDoomguard, "Check if you want to use a Doomguard during the last minute of the fight");
            this.chbUseDoomguard.UseVisualStyleBackColor = true;
            this.chbUseDoomguard.CheckedChanged += new System.EventHandler(this.chbUseDoomguard_CheckedChanged);
            // 
            // tbAffEffects
            // 
            this.tbAffEffects.Location = new System.Drawing.Point(179, 69);
            this.tbAffEffects.Name = "tbAffEffects";
            this.tbAffEffects.Size = new System.Drawing.Size(100, 20);
            this.tbAffEffects.TabIndex = 74;
            this.tbAffEffects.Text = "5";
            this.toolTip1.SetToolTip(this.tbAffEffects, "Enter how many Affliction effects are on the target except your own.");
            this.tbAffEffects.TextChanged += new System.EventHandler(this.tbAffEffects_Changed);
            this.tbAffEffects.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbAffEffects_KeyPress);
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
            this.tabFight.Controls.Add(this.tbAffEffects);
            this.tabFight.Controls.Add(this.label2);
            this.tabFight.Controls.Add(this.lblDelay);
            this.tabFight.Controls.Add(this.lblFSR);
            this.tabFight.Controls.Add(this.trkJoW);
            this.tabFight.Controls.Add(this.lblJoW);
            this.tabFight.Controls.Add(this.label4);
            this.tabFight.Controls.Add(this.trkReplenishment);
            this.tabFight.Controls.Add(this.cbManaAmt);
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
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 69);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(130, 13);
            this.label2.TabIndex = 73;
            this.label2.Text = "Affliction effects on target:";
            // 
            // lblDelay
            // 
            this.lblDelay.AutoSize = true;
            this.lblDelay.Location = new System.Drawing.Point(6, 195);
            this.lblDelay.Name = "lblDelay";
            this.lblDelay.Size = new System.Drawing.Size(34, 13);
            this.lblDelay.TabIndex = 70;
            this.lblDelay.Text = "Delay";
            // 
            // lblFSR
            // 
            this.lblFSR.AutoSize = true;
            this.lblFSR.Location = new System.Drawing.Point(6, 98);
            this.lblFSR.Name = "lblFSR";
            this.lblFSR.Size = new System.Drawing.Size(101, 13);
            this.lblFSR.TabIndex = 67;
            this.lblFSR.Text = "% time spent in FSR";
            // 
            // lblJoW
            // 
            this.lblJoW.AutoSize = true;
            this.lblJoW.Location = new System.Drawing.Point(6, 292);
            this.lblJoW.Name = "lblJoW";
            this.lblJoW.Size = new System.Drawing.Size(123, 13);
            this.lblJoW.TabIndex = 65;
            this.lblJoW.Text = "% Judgement of Wisdom";
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
            this.cbManaAmt.Location = new System.Drawing.Point(87, 41);
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
            this.lblFightLength.Location = new System.Drawing.Point(6, 146);
            this.lblFightLength.Name = "lblFightLength";
            this.lblFightLength.Size = new System.Drawing.Size(66, 13);
            this.lblFightLength.TabIndex = 59;
            this.lblFightLength.Text = "Fight Length";
            // 
            // tabChar
            // 
            this.tabChar.Controls.Add(this.gbGlyphs);
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
            // gbGlyphs
            // 
            this.gbGlyphs.Controls.Add(this.chbGlyphCorruption);
            this.gbGlyphs.Controls.Add(this.chbGlyphSearingPain);
            this.gbGlyphs.Controls.Add(this.chbGlyphShadowburn);
            this.gbGlyphs.Controls.Add(this.chbGlyphSB);
            this.gbGlyphs.Controls.Add(this.chbGlyphSiphonLife);
            this.gbGlyphs.Controls.Add(this.chbGlyphUA);
            this.gbGlyphs.Controls.Add(this.chbGlyphImp);
            this.gbGlyphs.Controls.Add(this.chbGlyphImmolate);
            this.gbGlyphs.Controls.Add(this.chbGlyphFelguard);
            this.gbGlyphs.Controls.Add(this.chbGlyphCoA);
            this.gbGlyphs.Controls.Add(this.chbGlyphConflag);
            this.gbGlyphs.Location = new System.Drawing.Point(6, 278);
            this.gbGlyphs.Name = "gbGlyphs";
            this.gbGlyphs.Size = new System.Drawing.Size(270, 278);
            this.gbGlyphs.TabIndex = 56;
            this.gbGlyphs.TabStop = false;
            this.gbGlyphs.Text = "Glyphs";
            // 
            // chbGlyphCorruption
            // 
            this.chbGlyphCorruption.AutoSize = true;
            this.chbGlyphCorruption.Location = new System.Drawing.Point(5, 42);
            this.chbGlyphCorruption.Name = "chbGlyphCorruption";
            this.chbGlyphCorruption.Size = new System.Drawing.Size(116, 17);
            this.chbGlyphCorruption.TabIndex = 77;
            this.chbGlyphCorruption.Text = "Glyph of Corruption";
            this.chbGlyphCorruption.UseVisualStyleBackColor = true;
            this.chbGlyphCorruption.CheckedChanged += new System.EventHandler(this.chbGlyphCorruption_CheckedChanged);
            // 
            // chbGlyphSearingPain
            // 
            this.chbGlyphSearingPain.AutoSize = true;
            this.chbGlyphSearingPain.Location = new System.Drawing.Point(4, 157);
            this.chbGlyphSearingPain.Name = "chbGlyphSearingPain";
            this.chbGlyphSearingPain.Size = new System.Drawing.Size(128, 17);
            this.chbGlyphSearingPain.TabIndex = 76;
            this.chbGlyphSearingPain.Text = "Glyph of Searing Pain";
            this.chbGlyphSearingPain.UseVisualStyleBackColor = true;
            this.chbGlyphSearingPain.CheckStateChanged += new System.EventHandler(this.chbGlyphSearingPain_CheckedChanged);
            // 
            // chbGlyphShadowburn
            // 
            this.chbGlyphShadowburn.AutoSize = true;
            this.chbGlyphShadowburn.Enabled = false;
            this.chbGlyphShadowburn.Location = new System.Drawing.Point(4, 203);
            this.chbGlyphShadowburn.Name = "chbGlyphShadowburn";
            this.chbGlyphShadowburn.Size = new System.Drawing.Size(128, 17);
            this.chbGlyphShadowburn.TabIndex = 75;
            this.chbGlyphShadowburn.Text = "Glyph of Shadowburn";
            this.chbGlyphShadowburn.UseVisualStyleBackColor = true;
            this.chbGlyphShadowburn.CheckStateChanged += new System.EventHandler(this.chbGlyphShadowburn_CheckedChanged);
            // 
            // chbGlyphSB
            // 
            this.chbGlyphSB.AutoSize = true;
            this.chbGlyphSB.Location = new System.Drawing.Point(4, 180);
            this.chbGlyphSB.Name = "chbGlyphSB";
            this.chbGlyphSB.Size = new System.Drawing.Size(128, 17);
            this.chbGlyphSB.TabIndex = 74;
            this.chbGlyphSB.Text = "Glyph of Shadow Bolt";
            this.chbGlyphSB.UseVisualStyleBackColor = true;
            this.chbGlyphSB.CheckStateChanged += new System.EventHandler(this.chbGlyphSB_CheckedChanged);
            // 
            // chbGlyphSiphonLife
            // 
            this.chbGlyphSiphonLife.AutoSize = true;
            this.chbGlyphSiphonLife.Location = new System.Drawing.Point(4, 226);
            this.chbGlyphSiphonLife.Name = "chbGlyphSiphonLife";
            this.chbGlyphSiphonLife.Size = new System.Drawing.Size(121, 17);
            this.chbGlyphSiphonLife.TabIndex = 73;
            this.chbGlyphSiphonLife.Text = "Glyph of Siphon Life";
            this.chbGlyphSiphonLife.UseVisualStyleBackColor = true;
            this.chbGlyphSiphonLife.CheckStateChanged += new System.EventHandler(this.chbGlyphSiphonLife_CheckedChanged);
            // 
            // chbGlyphUA
            // 
            this.chbGlyphUA.AutoSize = true;
            this.chbGlyphUA.Location = new System.Drawing.Point(4, 249);
            this.chbGlyphUA.Name = "chbGlyphUA";
            this.chbGlyphUA.Size = new System.Drawing.Size(153, 17);
            this.chbGlyphUA.TabIndex = 72;
            this.chbGlyphUA.Text = "Glyph of Unstable Affliction";
            this.chbGlyphUA.UseVisualStyleBackColor = true;
            this.chbGlyphUA.CheckStateChanged += new System.EventHandler(this.chbGlyphUA_CheckedChanged);
            // 
            // chbGlyphImp
            // 
            this.chbGlyphImp.AutoSize = true;
            this.chbGlyphImp.Location = new System.Drawing.Point(4, 134);
            this.chbGlyphImp.Name = "chbGlyphImp";
            this.chbGlyphImp.Size = new System.Drawing.Size(85, 17);
            this.chbGlyphImp.TabIndex = 71;
            this.chbGlyphImp.Text = "Glyph of Imp";
            this.chbGlyphImp.UseVisualStyleBackColor = true;
            this.chbGlyphImp.CheckStateChanged += new System.EventHandler(this.chbGlyphImp_CheckedChanged);
            // 
            // chbGlyphImmolate
            // 
            this.chbGlyphImmolate.AutoSize = true;
            this.chbGlyphImmolate.Location = new System.Drawing.Point(4, 111);
            this.chbGlyphImmolate.Name = "chbGlyphImmolate";
            this.chbGlyphImmolate.Size = new System.Drawing.Size(110, 17);
            this.chbGlyphImmolate.TabIndex = 70;
            this.chbGlyphImmolate.Text = "Glyph of Immolate";
            this.chbGlyphImmolate.UseVisualStyleBackColor = true;
            this.chbGlyphImmolate.CheckedChanged += new System.EventHandler(this.chbGlyphImmolate_CheckedChanged);
            // 
            // chbGlyphFelguard
            // 
            this.chbGlyphFelguard.AutoSize = true;
            this.chbGlyphFelguard.Location = new System.Drawing.Point(5, 88);
            this.chbGlyphFelguard.Name = "chbGlyphFelguard";
            this.chbGlyphFelguard.Size = new System.Drawing.Size(109, 17);
            this.chbGlyphFelguard.TabIndex = 68;
            this.chbGlyphFelguard.Text = "Glyph of Felguard";
            this.chbGlyphFelguard.UseVisualStyleBackColor = true;
            this.chbGlyphFelguard.CheckedChanged += new System.EventHandler(this.chbGlyphFelguard_CheckedChanged);
            // 
            // chbGlyphCoA
            // 
            this.chbGlyphCoA.AutoSize = true;
            this.chbGlyphCoA.Location = new System.Drawing.Point(5, 65);
            this.chbGlyphCoA.Name = "chbGlyphCoA";
            this.chbGlyphCoA.Size = new System.Drawing.Size(140, 17);
            this.chbGlyphCoA.TabIndex = 67;
            this.chbGlyphCoA.Text = "Glyph of Curse of Agony";
            this.chbGlyphCoA.UseVisualStyleBackColor = true;
            this.chbGlyphCoA.CheckedChanged += new System.EventHandler(this.chbGlyphCoA_CheckedChanged);
            // 
            // chbGlyphConflag
            // 
            this.chbGlyphConflag.AutoSize = true;
            this.chbGlyphConflag.Location = new System.Drawing.Point(6, 19);
            this.chbGlyphConflag.Name = "chbGlyphConflag";
            this.chbGlyphConflag.Size = new System.Drawing.Size(122, 17);
            this.chbGlyphConflag.TabIndex = 66;
            this.chbGlyphConflag.Text = "Glyph of Conflagrate";
            this.chbGlyphConflag.UseVisualStyleBackColor = true;
            this.chbGlyphConflag.CheckedChanged += new System.EventHandler(this.chbGlyphConflag_CheckedChanged);
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
            this.gbSpellPriority.Location = new System.Drawing.Point(6, 78);
            this.gbSpellPriority.Name = "gbSpellPriority";
            this.gbSpellPriority.Size = new System.Drawing.Size(270, 184);
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
            this.tabPet.Controls.Add(this.chbUseDoomguard);
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
            this.label1.Location = new System.Drawing.Point(30, 18);
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
            "Imp",
            "Felhunter",
            "Felguard",
            "Succubus",
            "Voidwalker",
            "Doomguard"});
            this.cbPet.Location = new System.Drawing.Point(87, 15);
            this.cbPet.Name = "cbPet";
            this.cbPet.Size = new System.Drawing.Size(151, 21);
            this.cbPet.TabIndex = 61;
            this.cbPet.SelectedIndexChanged += new System.EventHandler(this.cbPet_SelectedIndexChanged);
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
            this.gbGlyphs.ResumeLayout(false);
            this.gbGlyphs.PerformLayout();
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
        private System.Windows.Forms.Label lblFSR;
        private System.Windows.Forms.TrackBar trkJoW;
        private System.Windows.Forms.Label lblJoW;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TrackBar trkReplenishment;
        private System.Windows.Forms.ComboBox cbManaAmt;
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
        private System.Windows.Forms.ComboBox cbPet;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chbUseDoomguard;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbAffEffects;
        private System.Windows.Forms.GroupBox gbGlyphs;
        private System.Windows.Forms.CheckBox chbGlyphSearingPain;
        private System.Windows.Forms.CheckBox chbGlyphShadowburn;
        private System.Windows.Forms.CheckBox chbGlyphSB;
        private System.Windows.Forms.CheckBox chbGlyphSiphonLife;
        private System.Windows.Forms.CheckBox chbGlyphUA;
        private System.Windows.Forms.CheckBox chbGlyphImp;
        private System.Windows.Forms.CheckBox chbGlyphImmolate;
        private System.Windows.Forms.CheckBox chbGlyphFelguard;
        private System.Windows.Forms.CheckBox chbGlyphCoA;
        private System.Windows.Forms.CheckBox chbGlyphConflag;
        private System.Windows.Forms.CheckBox chbGlyphCorruption;
    }
}