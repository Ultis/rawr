namespace Rawr.Tree
{
    partial class CalculationOptionsPanelTree
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblSurvMulti = new System.Windows.Forms.Label();
            this.tbSurvMulti = new System.Windows.Forms.TrackBar();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lblBSRatio = new System.Windows.Forms.Label();
            this.tbBSRatio = new System.Windows.Forms.TrackBar();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.lblIdleFraction = new System.Windows.Forms.Label();
            this.tbIdlePercentage = new System.Windows.Forms.TrackBar();
            this.tbSwiftmendPerMin = new System.Windows.Forms.TrackBar();
            this.lblSwiftMend = new System.Windows.Forms.Label();
            this.tbPrimaryHealFrac = new System.Windows.Forms.TrackBar();
            this.lblPrimaryHeal = new System.Windows.Forms.Label();
            this.tbWildGrowth = new System.Windows.Forms.TrackBar();
            this.cbRotation = new System.Windows.Forms.ComboBox();
            this.lblWG = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.trkFightLength = new System.Windows.Forms.TrackBar();
            this.lblFightLength = new System.Windows.Forms.Label();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.cbInnervate = new System.Windows.Forms.CheckBox();
            this.cmbManaAmt = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.trkTimeInFSR = new System.Windows.Forms.TrackBar();
            this.lblFSR = new System.Windows.Forms.Label();
            this.tkReplenishment = new System.Windows.Forms.TrackBar();
            this.lblReplenishment = new System.Windows.Forms.Label();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.tbModuleNotes = new System.Windows.Forms.TextBox();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbSurvMulti)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbBSRatio)).BeginInit();
            this.tabPage2.SuspendLayout();
            this.groupBox7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbIdlePercentage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbSwiftmendPerMin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbPrimaryHealFrac)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbWildGrowth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkFightLength)).BeginInit();
            this.tabPage5.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trkTimeInFSR)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tkReplenishment)).BeginInit();
            this.tabPage4.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage5);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Location = new System.Drawing.Point(3, 3);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(297, 545);
            this.tabControl1.TabIndex = 23;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(289, 519);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Stats";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblSurvMulti);
            this.groupBox1.Controls.Add(this.tbSurvMulti);
            this.groupBox1.Location = new System.Drawing.Point(7, 99);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(276, 87);
            this.groupBox1.TabIndex = 43;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Survival value";
            // 
            // lblSurvMulti
            // 
            this.lblSurvMulti.AutoSize = true;
            this.lblSurvMulti.Location = new System.Drawing.Point(6, 20);
            this.lblSurvMulti.Name = "lblSurvMulti";
            this.lblSurvMulti.Size = new System.Drawing.Size(89, 13);
            this.lblSurvMulti.TabIndex = 41;
            this.lblSurvMulti.Text = "Survival Multiplier";
            // 
            // tbSurvMulti
            // 
            this.tbSurvMulti.BackColor = System.Drawing.SystemColors.Window;
            this.tbSurvMulti.LargeChange = 1;
            this.tbSurvMulti.Location = new System.Drawing.Point(9, 36);
            this.tbSurvMulti.Name = "tbSurvMulti";
            this.tbSurvMulti.Size = new System.Drawing.Size(262, 45);
            this.tbSurvMulti.TabIndex = 40;
            this.tbSurvMulti.Value = 1;
            this.tbSurvMulti.Scroll += new System.EventHandler(this.tbSurvMulti_Scroll);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lblBSRatio);
            this.groupBox2.Controls.Add(this.tbBSRatio);
            this.groupBox2.Location = new System.Drawing.Point(6, 6);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(276, 87);
            this.groupBox2.TabIndex = 41;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Burst : Sustained Ratio";
            // 
            // lblBSRatio
            // 
            this.lblBSRatio.AutoSize = true;
            this.lblBSRatio.Location = new System.Drawing.Point(6, 20);
            this.lblBSRatio.Name = "lblBSRatio";
            this.lblBSRatio.Size = new System.Drawing.Size(32, 13);
            this.lblBSRatio.TabIndex = 41;
            this.lblBSRatio.Text = "Ratio";
            // 
            // tbBSRatio
            // 
            this.tbBSRatio.BackColor = System.Drawing.SystemColors.Window;
            this.tbBSRatio.Location = new System.Drawing.Point(9, 36);
            this.tbBSRatio.Maximum = 100;
            this.tbBSRatio.Name = "tbBSRatio";
            this.tbBSRatio.Size = new System.Drawing.Size(262, 45);
            this.tbBSRatio.TabIndex = 40;
            this.tbBSRatio.TickFrequency = 5;
            this.tbBSRatio.Value = 50;
            this.tbBSRatio.Scroll += new System.EventHandler(this.tbBSRatio_Scroll);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.groupBox7);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(289, 519);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Fight";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.lblIdleFraction);
            this.groupBox7.Controls.Add(this.tbIdlePercentage);
            this.groupBox7.Controls.Add(this.tbSwiftmendPerMin);
            this.groupBox7.Controls.Add(this.lblSwiftMend);
            this.groupBox7.Controls.Add(this.tbPrimaryHealFrac);
            this.groupBox7.Controls.Add(this.lblPrimaryHeal);
            this.groupBox7.Controls.Add(this.tbWildGrowth);
            this.groupBox7.Controls.Add(this.cbRotation);
            this.groupBox7.Controls.Add(this.lblWG);
            this.groupBox7.Controls.Add(this.label11);
            this.groupBox7.Controls.Add(this.trkFightLength);
            this.groupBox7.Controls.Add(this.lblFightLength);
            this.groupBox7.Location = new System.Drawing.Point(6, 6);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(277, 412);
            this.groupBox7.TabIndex = 33;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Fight Details";
            // 
            // lblIdleFraction
            // 
            this.lblIdleFraction.AutoSize = true;
            this.lblIdleFraction.Location = new System.Drawing.Point(6, 314);
            this.lblIdleFraction.Name = "lblIdleFraction";
            this.lblIdleFraction.Size = new System.Drawing.Size(143, 13);
            this.lblIdleFraction.TabIndex = 56;
            this.lblIdleFraction.Text = "Non-casting fraction required";
            // 
            // tbIdlePercentage
            // 
            this.tbIdlePercentage.BackColor = System.Drawing.SystemColors.Window;
            this.tbIdlePercentage.Location = new System.Drawing.Point(6, 330);
            this.tbIdlePercentage.Maximum = 100;
            this.tbIdlePercentage.Name = "tbIdlePercentage";
            this.tbIdlePercentage.Size = new System.Drawing.Size(262, 45);
            this.tbIdlePercentage.TabIndex = 55;
            this.tbIdlePercentage.TickFrequency = 5;
            this.tbIdlePercentage.Scroll += new System.EventHandler(this.tbIdlePercentage_Scroll);
            // 
            // tbSwiftmendPerMin
            // 
            this.tbSwiftmendPerMin.BackColor = System.Drawing.SystemColors.Window;
            this.tbSwiftmendPerMin.Location = new System.Drawing.Point(6, 266);
            this.tbSwiftmendPerMin.Maximum = 4;
            this.tbSwiftmendPerMin.Name = "tbSwiftmendPerMin";
            this.tbSwiftmendPerMin.Size = new System.Drawing.Size(262, 45);
            this.tbSwiftmendPerMin.TabIndex = 54;
            this.tbSwiftmendPerMin.Scroll += new System.EventHandler(this.tbSwiftmend_Scroll);
            // 
            // lblSwiftMend
            // 
            this.lblSwiftMend.AutoSize = true;
            this.lblSwiftMend.Location = new System.Drawing.Point(6, 250);
            this.lblSwiftMend.Name = "lblSwiftMend";
            this.lblSwiftMend.Size = new System.Drawing.Size(136, 13);
            this.lblSwiftMend.TabIndex = 53;
            this.lblSwiftMend.Text = "Swiftmends cast per minute";
            // 
            // tbPrimaryHealFrac
            // 
            this.tbPrimaryHealFrac.BackColor = System.Drawing.SystemColors.Window;
            this.tbPrimaryHealFrac.Location = new System.Drawing.Point(9, 138);
            this.tbPrimaryHealFrac.Maximum = 100;
            this.tbPrimaryHealFrac.Name = "tbPrimaryHealFrac";
            this.tbPrimaryHealFrac.Size = new System.Drawing.Size(262, 45);
            this.tbPrimaryHealFrac.TabIndex = 51;
            this.tbPrimaryHealFrac.TickFrequency = 5;
            this.tbPrimaryHealFrac.Visible = false;
            this.tbPrimaryHealFrac.Scroll += new System.EventHandler(this.tbPrimaryHealFrac_Scroll);
            // 
            // lblPrimaryHeal
            // 
            this.lblPrimaryHeal.AutoSize = true;
            this.lblPrimaryHeal.Location = new System.Drawing.Point(6, 122);
            this.lblPrimaryHeal.Name = "lblPrimaryHeal";
            this.lblPrimaryHeal.Size = new System.Drawing.Size(103, 13);
            this.lblPrimaryHeal.TabIndex = 52;
            this.lblPrimaryHeal.Text = "Primary Heal Usage:";
            this.lblPrimaryHeal.Visible = false;
            // 
            // tbWildGrowth
            // 
            this.tbWildGrowth.BackColor = System.Drawing.SystemColors.Window;
            this.tbWildGrowth.Location = new System.Drawing.Point(6, 202);
            this.tbWildGrowth.Name = "tbWildGrowth";
            this.tbWildGrowth.Size = new System.Drawing.Size(262, 45);
            this.tbWildGrowth.TabIndex = 39;
            this.tbWildGrowth.Scroll += new System.EventHandler(this.tbWildGrowth_Scroll);
            // 
            // cbRotation
            // 
            this.cbRotation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbRotation.FormattingEnabled = true;
            this.cbRotation.Items.AddRange(new object[] {
            "Tank Nourish (plus RJ/RG/Roll LB)",
            "Tank Nourish (plus RJ/RG/Slow 3xLB)",
            "Tank Nourish (plus RJ/RG/Fast 3xLB)",
            "Tank Nourish (2 Tanks RJ/RG/LB)",
            "Tank Nourish (2 Tanks RJ/RG/Slow 3xLB)",
            "Tank Nourish (2 Tanks RJ/RG/Fast 3xLB)",
            "Tank Healing Touch (plus RJ/RG/LB)",
            "Tank Healing Touch (2 Tanks RJ/RG/LB)",
            "Tank Regrowth (plus RJ/RG/Roll LB)",
            "Tank Regrowth (plus RJ/RG/Slow 3xLB)",
            "Tank Regrowth (plus RJ/RG/Fast 3xLB)",
            "Tank Regrowth (2 Tanks RJ/RG/Roll LB)",
            "Tank Regrowth (2 Tanks RJ/RG/Slow 3xLB)",
            "Tank Regrowth (2 Tanks RJ/RG/Fast 3xLB)",
            "Raid heal with Regrowth (1 Tank RJ/Roll LB)",
            "Raid heal with Regrowth (1 Tank RJ/Slow 3xLB)",
            "Raid heal with Regrowth (2 Tanks RJ/Roll LB)",
            "Raid heal with Regrowth (2 Tanks RJ/Slow 3xLB)",
            "Raid heal with Rejuv (1 Tank RJ/Roll LB)",
            "Raid heal with Rejuv (1 Tank RJ/Slow 3xLB)",
            "Raid heal with Rejuv (2 Tanks RJ/Roll LB)",
            "Raid heal with Rejuv (2 Tanks RJ/Slow 3xLB)",
            "Raid heal with Nourish (1 Tank RJ/Roll LB)",
            "Raid heal with Nourish (1 Tank RJ/Slow 3xLB)",
            "Raid heal with Nourish (2 Tanks RJ/Roll LB)",
            "Raid heal with Nourish (2 Tanks RJ/Slow 3xLB)",
            "Nourish spam",
            "Healing Touch spam",
            "Regrowth spam on tank",
            "Regrowth spam on raid",
            "Rejuvenation spam on raid"});
            this.cbRotation.Location = new System.Drawing.Point(9, 34);
            this.cbRotation.MaxDropDownItems = 10;
            this.cbRotation.Name = "cbRotation";
            this.cbRotation.Size = new System.Drawing.Size(262, 21);
            this.cbRotation.TabIndex = 49;
            this.cbRotation.SelectedIndexChanged += new System.EventHandler(this.cbRotation_SelectedIndexChanged);
            // 
            // lblWG
            // 
            this.lblWG.AutoSize = true;
            this.lblWG.Location = new System.Drawing.Point(6, 186);
            this.lblWG.Name = "lblWG";
            this.lblWG.Size = new System.Drawing.Size(148, 13);
            this.lblWG.TabIndex = 5;
            this.lblWG.Text = "Wild Growth casts per minute:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(6, 18);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(67, 13);
            this.label11.TabIndex = 48;
            this.label11.Text = "Spell Usage:";
            // 
            // trkFightLength
            // 
            this.trkFightLength.BackColor = System.Drawing.SystemColors.Window;
            this.trkFightLength.Location = new System.Drawing.Point(9, 74);
            this.trkFightLength.Maximum = 60;
            this.trkFightLength.Minimum = 1;
            this.trkFightLength.Name = "trkFightLength";
            this.trkFightLength.Size = new System.Drawing.Size(262, 45);
            this.trkFightLength.TabIndex = 39;
            this.trkFightLength.TickFrequency = 4;
            this.trkFightLength.Value = 1;
            this.trkFightLength.Scroll += new System.EventHandler(this.trkFightLength_Scroll);
            // 
            // lblFightLength
            // 
            this.lblFightLength.AutoSize = true;
            this.lblFightLength.Location = new System.Drawing.Point(6, 58);
            this.lblFightLength.Name = "lblFightLength";
            this.lblFightLength.Size = new System.Drawing.Size(69, 13);
            this.lblFightLength.TabIndex = 38;
            this.lblFightLength.Text = "Fight Length:";
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.groupBox3);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(289, 519);
            this.tabPage5.TabIndex = 4;
            this.tabPage5.Text = "Mana";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.cbInnervate);
            this.groupBox3.Controls.Add(this.cmbManaAmt);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Controls.Add(this.trkTimeInFSR);
            this.groupBox3.Controls.Add(this.lblFSR);
            this.groupBox3.Controls.Add(this.tkReplenishment);
            this.groupBox3.Controls.Add(this.lblReplenishment);
            this.groupBox3.Location = new System.Drawing.Point(3, 6);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(283, 201);
            this.groupBox3.TabIndex = 56;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Mana Regeneration";
            // 
            // cbInnervate
            // 
            this.cbInnervate.AutoSize = true;
            this.cbInnervate.Location = new System.Drawing.Point(6, 174);
            this.cbInnervate.Name = "cbInnervate";
            this.cbInnervate.Size = new System.Drawing.Size(115, 17);
            this.cbInnervate.TabIndex = 56;
            this.cbInnervate.Text = "Use own innervate";
            this.cbInnervate.UseVisualStyleBackColor = true;
            this.cbInnervate.CheckedChanged += new System.EventHandler(this.cbInnervate_CheckedChanged);
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
            this.cmbManaAmt.Location = new System.Drawing.Point(85, 19);
            this.cmbManaAmt.Name = "cmbManaAmt";
            this.cmbManaAmt.Size = new System.Drawing.Size(185, 21);
            this.cmbManaAmt.TabIndex = 53;
            this.cmbManaAmt.ValueMember = "2400";
            this.cmbManaAmt.SelectedIndexChanged += new System.EventHandler(this.cmbManaAmt_SelectedIndexChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(6, 22);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(69, 13);
            this.label10.TabIndex = 54;
            this.label10.Text = "Mana potion:";
            // 
            // trkTimeInFSR
            // 
            this.trkTimeInFSR.BackColor = System.Drawing.SystemColors.Window;
            this.trkTimeInFSR.Location = new System.Drawing.Point(6, 123);
            this.trkTimeInFSR.Maximum = 100;
            this.trkTimeInFSR.Name = "trkTimeInFSR";
            this.trkTimeInFSR.Size = new System.Drawing.Size(271, 45);
            this.trkTimeInFSR.TabIndex = 39;
            this.trkTimeInFSR.TickFrequency = 5;
            this.trkTimeInFSR.Scroll += new System.EventHandler(this.trkTimeInFSR_Scroll);
            // 
            // lblFSR
            // 
            this.lblFSR.AutoSize = true;
            this.lblFSR.Location = new System.Drawing.Point(3, 107);
            this.lblFSR.Name = "lblFSR";
            this.lblFSR.Size = new System.Drawing.Size(97, 13);
            this.lblFSR.TabIndex = 40;
            this.lblFSR.Text = "Time spent in FSR:";
            // 
            // tkReplenishment
            // 
            this.tkReplenishment.BackColor = System.Drawing.SystemColors.Window;
            this.tkReplenishment.Location = new System.Drawing.Point(6, 59);
            this.tkReplenishment.Maximum = 100;
            this.tkReplenishment.Name = "tkReplenishment";
            this.tkReplenishment.Size = new System.Drawing.Size(271, 45);
            this.tkReplenishment.TabIndex = 52;
            this.tkReplenishment.TickFrequency = 5;
            this.tkReplenishment.Scroll += new System.EventHandler(this.tkReplenishment_Scroll);
            // 
            // lblReplenishment
            // 
            this.lblReplenishment.AutoSize = true;
            this.lblReplenishment.Location = new System.Drawing.Point(5, 43);
            this.lblReplenishment.Name = "lblReplenishment";
            this.lblReplenishment.Size = new System.Drawing.Size(80, 13);
            this.lblReplenishment.TabIndex = 55;
            this.lblReplenishment.Text = "Replenishment:";
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.tbModuleNotes);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(289, 519);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Module Notes";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // tbModuleNotes
            // 
            this.tbModuleNotes.AcceptsReturn = true;
            this.tbModuleNotes.AcceptsTab = true;
            this.tbModuleNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbModuleNotes.Location = new System.Drawing.Point(3, 3);
            this.tbModuleNotes.Multiline = true;
            this.tbModuleNotes.Name = "tbModuleNotes";
            this.tbModuleNotes.ReadOnly = true;
            this.tbModuleNotes.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbModuleNotes.Size = new System.Drawing.Size(283, 513);
            this.tbModuleNotes.TabIndex = 0;
            // 
            // CalculationOptionsPanelTree
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.tabControl1);
            this.Name = "CalculationOptionsPanelTree";
            this.Size = new System.Drawing.Size(303, 582);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbSurvMulti)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbBSRatio)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbIdlePercentage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbSwiftmendPerMin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbPrimaryHealFrac)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbWildGrowth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkFightLength)).EndInit();
            this.tabPage5.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trkTimeInFSR)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tkReplenishment)).EndInit();
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.Label lblFightLength;
        private System.Windows.Forms.TrackBar trkFightLength;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox cbRotation;
        private System.Windows.Forms.TrackBar tbWildGrowth;
        private System.Windows.Forms.Label lblWG;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.TextBox tbModuleNotes;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label lblBSRatio;
        private System.Windows.Forms.TrackBar tbBSRatio;
        private System.Windows.Forms.TrackBar tbPrimaryHealFrac;
        private System.Windows.Forms.Label lblPrimaryHeal;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.TrackBar tkReplenishment;
        private System.Windows.Forms.Label lblReplenishment;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox cmbManaAmt;
        private System.Windows.Forms.Label lblFSR;
        private System.Windows.Forms.TrackBar trkTimeInFSR;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox cbInnervate;
        private System.Windows.Forms.TrackBar tbSwiftmendPerMin;
        private System.Windows.Forms.Label lblSwiftMend;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblSurvMulti;
        private System.Windows.Forms.TrackBar tbSurvMulti;
        private System.Windows.Forms.Label lblIdleFraction;
        private System.Windows.Forms.TrackBar tbIdlePercentage;
    }
}