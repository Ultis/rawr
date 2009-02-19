namespace Rawr.HolyPriest
{
    partial class CalculationOptionsPanelHolyPriest
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CalculationOptionsPanelHolyPriest));
            this.trkActivity = new System.Windows.Forms.TrackBar();
            this.lblActivity = new System.Windows.Forms.Label();
            this.cbRotation = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lblFightLength = new System.Windows.Forms.Label();
            this.trkFightLength = new System.Windows.Forms.TrackBar();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.trkSerendipity = new System.Windows.Forms.TrackBar();
            this.trkReplenishment = new System.Windows.Forms.TrackBar();
            this.trkShadowfiend = new System.Windows.Forms.TrackBar();
            this.trkSurvivability = new System.Windows.Forms.TrackBar();
            this.trkTestOfFaith = new System.Windows.Forms.TrackBar();
            this.cbModelProcs = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lblSerendipity = new System.Windows.Forms.Label();
            this.lblReplenishment = new System.Windows.Forms.Label();
            this.lblShadowfiend = new System.Windows.Forms.Label();
            this.lblSurvivability = new System.Windows.Forms.Label();
            this.trkRapture = new System.Windows.Forms.TrackBar();
            this.lblRapture = new System.Windows.Forms.Label();
            this.lblTestOfFaith = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbManaAmt = new System.Windows.Forms.ComboBox();
            this.cbNewMana = new System.Windows.Forms.CheckBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.label14 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.numericUpDown5 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown4 = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            this.panelCustom = new System.Windows.Forms.Panel();
            this.numericUpDown7 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown6 = new System.Windows.Forms.NumericUpDown();
            this.label13 = new System.Windows.Forms.Label();
            this.numericUpDown3 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown2 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.trkActivity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkFightLength)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkSerendipity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkReplenishment)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkShadowfiend)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkSurvivability)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkTestOfFaith)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkRapture)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown4)).BeginInit();
            this.panelCustom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // trkActivity
            // 
            this.trkActivity.BackColor = System.Drawing.SystemColors.Control;
            this.trkActivity.Location = new System.Drawing.Point(6, 42);
            this.trkActivity.Maximum = 100;
            this.trkActivity.Name = "trkActivity";
            this.trkActivity.Size = new System.Drawing.Size(262, 42);
            this.trkActivity.TabIndex = 23;
            this.trkActivity.TickFrequency = 5;
            this.toolTip1.SetToolTip(this.trkActivity, resources.GetString("trkActivity.ToolTip"));
            this.trkActivity.Scroll += new System.EventHandler(this.trkActivity_Scroll);
            // 
            // lblActivity
            // 
            this.lblActivity.AutoSize = true;
            this.lblActivity.Location = new System.Drawing.Point(3, 26);
            this.lblActivity.Name = "lblActivity";
            this.lblActivity.Size = new System.Drawing.Size(65, 13);
            this.lblActivity.TabIndex = 24;
            this.lblActivity.Text = "Time in FSR";
            // 
            // cbRotation
            // 
            this.cbRotation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbRotation.FormattingEnabled = true;
            this.cbRotation.Items.AddRange(new object[] {
            "Auto Tank (Rawr picks from Talents)",
            "Auto Raid (Rawr picks from Talents)",
            "Greater Heal Spam (GH)",
            "Flash Heal Spam (FH)",
            "Circle of Healing Spam (CoH)",
            "Holy-Tank (Renew/ProM/GH)",
            "Holy-Raid (ProM/CoH/FH)",
            "Disc-Tank (Penance/PW:S/ProM/GH)",
            "Disc-Tank (Penance/PW:S/ProM/FH)",
            "Disc-Raid (PW:S/Penance/Flash)",
            "Custom Role (You pick!)"});
            this.cbRotation.Location = new System.Drawing.Point(6, 166);
            this.cbRotation.MaxDropDownItems = 10;
            this.cbRotation.Name = "cbRotation";
            this.cbRotation.Size = new System.Drawing.Size(262, 21);
            this.cbRotation.TabIndex = 26;
            this.toolTip1.SetToolTip(this.cbRotation, "Pick the spells to cast when comparing gear.");
            this.cbRotation.SelectedIndexChanged += new System.EventHandler(this.cbRotation_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 148);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 13);
            this.label1.TabIndex = 27;
            this.label1.Text = "Pick your Role";
            // 
            // lblFightLength
            // 
            this.lblFightLength.AutoSize = true;
            this.lblFightLength.Location = new System.Drawing.Point(3, 3);
            this.lblFightLength.Name = "lblFightLength";
            this.lblFightLength.Size = new System.Drawing.Size(66, 13);
            this.lblFightLength.TabIndex = 28;
            this.lblFightLength.Text = "Fight Length";
            // 
            // trkFightLength
            // 
            this.trkFightLength.Location = new System.Drawing.Point(6, 19);
            this.trkFightLength.Maximum = 20;
            this.trkFightLength.Minimum = 1;
            this.trkFightLength.Name = "trkFightLength";
            this.trkFightLength.Size = new System.Drawing.Size(262, 42);
            this.trkFightLength.TabIndex = 29;
            this.toolTip1.SetToolTip(this.trkFightLength, "Changing this bar tells Rawr how long the fight is estimated to last. This has im" +
                    "pact on things like how good intellect and spirit and mp5 are compared to eachot" +
                    "her.");
            this.trkFightLength.Value = 1;
            this.trkFightLength.Scroll += new System.EventHandler(this.trkFightLength_Scroll);
            // 
            // trkSerendipity
            // 
            this.trkSerendipity.Location = new System.Drawing.Point(6, 282);
            this.trkSerendipity.Maximum = 100;
            this.trkSerendipity.Name = "trkSerendipity";
            this.trkSerendipity.Size = new System.Drawing.Size(262, 42);
            this.trkSerendipity.TabIndex = 30;
            this.trkSerendipity.TickFrequency = 5;
            this.toolTip1.SetToolTip(this.trkSerendipity, "Tell Rawr how many % of your Greater Heals and Flash Heals overheal the target, g" +
                    "iving you mana returns via Serendipity. Also controls how effective T5 2 Part bo" +
                    "nus is.");
            this.trkSerendipity.Scroll += new System.EventHandler(this.trkSerendipity_Scroll);
            // 
            // trkReplenishment
            // 
            this.trkReplenishment.Location = new System.Drawing.Point(6, 164);
            this.trkReplenishment.Maximum = 100;
            this.trkReplenishment.Name = "trkReplenishment";
            this.trkReplenishment.Size = new System.Drawing.Size(262, 42);
            this.trkReplenishment.TabIndex = 33;
            this.trkReplenishment.TickFrequency = 5;
            this.toolTip1.SetToolTip(this.trkReplenishment, "This tells Rawr how much of the time you are expected to have Replenishment.");
            this.trkReplenishment.Scroll += new System.EventHandler(this.trkReplenishment_Scroll);
            // 
            // trkShadowfiend
            // 
            this.trkShadowfiend.Location = new System.Drawing.Point(6, 225);
            this.trkShadowfiend.Maximum = 150;
            this.trkShadowfiend.Name = "trkShadowfiend";
            this.trkShadowfiend.Size = new System.Drawing.Size(262, 42);
            this.trkShadowfiend.TabIndex = 35;
            this.trkShadowfiend.TickFrequency = 5;
            this.toolTip1.SetToolTip(this.trkShadowfiend, resources.GetString("trkShadowfiend.ToolTip"));
            this.trkShadowfiend.Scroll += new System.EventHandler(this.trkShadowfiend_Scroll);
            // 
            // trkSurvivability
            // 
            this.trkSurvivability.Location = new System.Drawing.Point(6, 80);
            this.trkSurvivability.Maximum = 100;
            this.trkSurvivability.Name = "trkSurvivability";
            this.trkSurvivability.Size = new System.Drawing.Size(262, 42);
            this.trkSurvivability.TabIndex = 38;
            this.trkSurvivability.TickFrequency = 5;
            this.toolTip1.SetToolTip(this.trkSurvivability, "Change this slider to tell Rawr how much you value your Health. Use 0-5% for PvE," +
                    " 10-15% for PvP.");
            this.trkSurvivability.Scroll += new System.EventHandler(this.trkSurvivability_Scroll);
            // 
            // trkTestOfFaith
            // 
            this.trkTestOfFaith.Location = new System.Drawing.Point(6, 340);
            this.trkTestOfFaith.Maximum = 100;
            this.trkTestOfFaith.Name = "trkTestOfFaith";
            this.trkTestOfFaith.Size = new System.Drawing.Size(262, 42);
            this.trkTestOfFaith.TabIndex = 44;
            this.trkTestOfFaith.TickFrequency = 5;
            this.toolTip1.SetToolTip(this.trkTestOfFaith, "Set this slider to the amount of Heals landing on players with less than 50% heal" +
                    "th.");
            this.trkTestOfFaith.Scroll += new System.EventHandler(this.trkTestOfFaith_Scroll);
            // 
            // cbModelProcs
            // 
            this.cbModelProcs.AutoSize = true;
            this.cbModelProcs.Location = new System.Drawing.Point(6, 128);
            this.cbModelProcs.Name = "cbModelProcs";
            this.cbModelProcs.Size = new System.Drawing.Size(177, 17);
            this.cbModelProcs.TabIndex = 41;
            this.cbModelProcs.Text = "Model items with Procs and Use";
            this.toolTip1.SetToolTip(this.cbModelProcs, "Checking this will make Rawr model Trinkets and other items with Use and Procs");
            this.cbModelProcs.UseVisualStyleBackColor = true;
            this.cbModelProcs.CheckedChanged += new System.EventHandler(this.cbUseTrinkets_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(158, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Please enter information below:*";
            this.toolTip1.SetToolTip(this.label2, "Type in information from a statistic gathering tool like Recount, WWS and similar" +
                    ".");
            // 
            // lblSerendipity
            // 
            this.lblSerendipity.AutoSize = true;
            this.lblSerendipity.Location = new System.Drawing.Point(3, 266);
            this.lblSerendipity.Name = "lblSerendipity";
            this.lblSerendipity.Size = new System.Drawing.Size(70, 13);
            this.lblSerendipity.TabIndex = 31;
            this.lblSerendipity.Text = "% Serendipity";
            // 
            // lblReplenishment
            // 
            this.lblReplenishment.AutoSize = true;
            this.lblReplenishment.Location = new System.Drawing.Point(3, 148);
            this.lblReplenishment.Name = "lblReplenishment";
            this.lblReplenishment.Size = new System.Drawing.Size(88, 13);
            this.lblReplenishment.TabIndex = 32;
            this.lblReplenishment.Text = "% Replenishment";
            // 
            // lblShadowfiend
            // 
            this.lblShadowfiend.AutoSize = true;
            this.lblShadowfiend.Location = new System.Drawing.Point(3, 209);
            this.lblShadowfiend.Name = "lblShadowfiend";
            this.lblShadowfiend.Size = new System.Drawing.Size(80, 13);
            this.lblShadowfiend.TabIndex = 34;
            this.lblShadowfiend.Text = "% Shadowfiend";
            // 
            // lblSurvivability
            // 
            this.lblSurvivability.AutoSize = true;
            this.lblSurvivability.Location = new System.Drawing.Point(3, 64);
            this.lblSurvivability.Name = "lblSurvivability";
            this.lblSurvivability.Size = new System.Drawing.Size(63, 13);
            this.lblSurvivability.TabIndex = 37;
            this.lblSurvivability.Text = "Survivability";
            // 
            // trkRapture
            // 
            this.trkRapture.Location = new System.Drawing.Point(6, 395);
            this.trkRapture.Maximum = 100;
            this.trkRapture.Name = "trkRapture";
            this.trkRapture.Size = new System.Drawing.Size(262, 42);
            this.trkRapture.TabIndex = 39;
            this.trkRapture.TickFrequency = 5;
            this.trkRapture.Scroll += new System.EventHandler(this.trkRapture_Scroll);
            // 
            // lblRapture
            // 
            this.lblRapture.AutoSize = true;
            this.lblRapture.Location = new System.Drawing.Point(3, 385);
            this.lblRapture.Name = "lblRapture";
            this.lblRapture.Size = new System.Drawing.Size(56, 13);
            this.lblRapture.TabIndex = 40;
            this.lblRapture.Text = "% Rapture";
            // 
            // lblTestOfFaith
            // 
            this.lblTestOfFaith.AutoSize = true;
            this.lblTestOfFaith.Location = new System.Drawing.Point(3, 324);
            this.lblTestOfFaith.Name = "lblTestOfFaith";
            this.lblTestOfFaith.Size = new System.Drawing.Size(139, 13);
            this.lblTestOfFaith.TabIndex = 43;
            this.lblTestOfFaith.Text = "% of Heals use Test of Faith";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 3);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(70, 13);
            this.label4.TabIndex = 46;
            this.label4.Text = "Mana Potion:";
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
            this.cmbManaAmt.Location = new System.Drawing.Point(84, 3);
            this.cmbManaAmt.Name = "cmbManaAmt";
            this.cmbManaAmt.Size = new System.Drawing.Size(184, 21);
            this.cmbManaAmt.TabIndex = 45;
            this.cmbManaAmt.ValueMember = "2400";
            this.cmbManaAmt.SelectedIndexChanged += new System.EventHandler(this.cmbManaAmt_SelectedIndexChanged);
            // 
            // cbNewMana
            // 
            this.cbNewMana.AutoSize = true;
            this.cbNewMana.Location = new System.Drawing.Point(138, 26);
            this.cbNewMana.Name = "cbNewMana";
            this.cbNewMana.Size = new System.Drawing.Size(121, 17);
            this.cbNewMana.TabIndex = 47;
            this.cbNewMana.Text = "3.1 Regen Changes";
            this.cbNewMana.UseVisualStyleBackColor = true;
            this.cbNewMana.CheckedChanged += new System.EventHandler(this.cbNewMana_CheckedChanged);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(3, 3);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(279, 518);
            this.tabControl1.SizeMode = System.Windows.Forms.TabSizeMode.FillToRight;
            this.tabControl1.TabIndex = 48;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.panelCustom);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.trkFightLength);
            this.tabPage1.Controls.Add(this.cbModelProcs);
            this.tabPage1.Controls.Add(this.lblFightLength);
            this.tabPage1.Controls.Add(this.cbRotation);
            this.tabPage1.Controls.Add(this.lblSurvivability);
            this.tabPage1.Controls.Add(this.trkSurvivability);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(271, 492);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Role";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.lblSerendipity);
            this.tabPage2.Controls.Add(this.lblRapture);
            this.tabPage2.Controls.Add(this.trkRapture);
            this.tabPage2.Controls.Add(this.trkTestOfFaith);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.cbNewMana);
            this.tabPage2.Controls.Add(this.trkActivity);
            this.tabPage2.Controls.Add(this.lblActivity);
            this.tabPage2.Controls.Add(this.lblTestOfFaith);
            this.tabPage2.Controls.Add(this.cmbManaAmt);
            this.tabPage2.Controls.Add(this.trkShadowfiend);
            this.tabPage2.Controls.Add(this.lblShadowfiend);
            this.tabPage2.Controls.Add(this.lblReplenishment);
            this.tabPage2.Controls.Add(this.trkReplenishment);
            this.tabPage2.Controls.Add(this.trkSerendipity);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(271, 492);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Mana";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.label14);
            this.tabPage3.Controls.Add(this.label12);
            this.tabPage3.Controls.Add(this.label11);
            this.tabPage3.Controls.Add(this.numericUpDown5);
            this.tabPage3.Controls.Add(this.numericUpDown4);
            this.tabPage3.Controls.Add(this.label10);
            this.tabPage3.Controls.Add(this.label2);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(271, 492);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Advanced Role";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(75, 271);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(121, 13);
            this.label14.TabIndex = 21;
            this.label14.Text = "WORK IN PROGRESS!";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(171, 21);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(49, 13);
            this.label12.TabIndex = 17;
            this.label12.Text = "Seconds";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(84, 21);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(44, 13);
            this.label11.TabIndex = 16;
            this.label11.Text = "Minutes";
            // 
            // numericUpDown5
            // 
            this.numericUpDown5.Location = new System.Drawing.Point(171, 40);
            this.numericUpDown5.Maximum = new decimal(new int[] {
            59,
            0,
            0,
            0});
            this.numericUpDown5.Name = "numericUpDown5";
            this.numericUpDown5.Size = new System.Drawing.Size(78, 20);
            this.numericUpDown5.TabIndex = 15;
            // 
            // numericUpDown4
            // 
            this.numericUpDown4.Location = new System.Drawing.Point(84, 40);
            this.numericUpDown4.Name = "numericUpDown4";
            this.numericUpDown4.Size = new System.Drawing.Size(78, 20);
            this.numericUpDown4.TabIndex = 14;
            this.numericUpDown4.Value = new decimal(new int[] {
            6,
            0,
            0,
            0});
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(6, 42);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(73, 13);
            this.label10.TabIndex = 13;
            this.label10.Text = "Fight Duration";
            // 
            // panelCustom
            // 
            this.panelCustom.Controls.Add(this.numericUpDown7);
            this.panelCustom.Controls.Add(this.numericUpDown6);
            this.panelCustom.Controls.Add(this.label13);
            this.panelCustom.Controls.Add(this.numericUpDown3);
            this.panelCustom.Controls.Add(this.numericUpDown2);
            this.panelCustom.Controls.Add(this.numericUpDown1);
            this.panelCustom.Controls.Add(this.label9);
            this.panelCustom.Controls.Add(this.label8);
            this.panelCustom.Controls.Add(this.label7);
            this.panelCustom.Controls.Add(this.label6);
            this.panelCustom.Controls.Add(this.label5);
            this.panelCustom.Controls.Add(this.label3);
            this.panelCustom.Location = new System.Drawing.Point(0, 193);
            this.panelCustom.Name = "panelCustom";
            this.panelCustom.Size = new System.Drawing.Size(271, 299);
            this.panelCustom.TabIndex = 42;
            // 
            // numericUpDown7
            // 
            this.numericUpDown7.Location = new System.Drawing.Point(168, 109);
            this.numericUpDown7.Name = "numericUpDown7";
            this.numericUpDown7.Size = new System.Drawing.Size(78, 20);
            this.numericUpDown7.TabIndex = 32;
            // 
            // numericUpDown6
            // 
            this.numericUpDown6.Location = new System.Drawing.Point(81, 109);
            this.numericUpDown6.Name = "numericUpDown6";
            this.numericUpDown6.Size = new System.Drawing.Size(78, 20);
            this.numericUpDown6.TabIndex = 31;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(3, 111);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(41, 13);
            this.label13.TabIndex = 30;
            this.label13.Text = "Renew";
            // 
            // numericUpDown3
            // 
            this.numericUpDown3.Location = new System.Drawing.Point(81, 82);
            this.numericUpDown3.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDown3.Name = "numericUpDown3";
            this.numericUpDown3.Size = new System.Drawing.Size(78, 20);
            this.numericUpDown3.TabIndex = 29;
            // 
            // numericUpDown2
            // 
            this.numericUpDown2.Location = new System.Drawing.Point(81, 56);
            this.numericUpDown2.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDown2.Name = "numericUpDown2";
            this.numericUpDown2.Size = new System.Drawing.Size(78, 20);
            this.numericUpDown2.TabIndex = 28;
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(81, 30);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(78, 20);
            this.numericUpDown1.TabIndex = 27;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(3, 84);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(50, 13);
            this.label9.TabIndex = 26;
            this.label9.Text = "Penance";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(3, 58);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(67, 13);
            this.label8.TabIndex = 25;
            this.label8.Text = "Greater Heal";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(165, 13);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(93, 13);
            this.label7.TabIndex = 24;
            this.label7.Text = "Targets Hit/Ticks:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(78, 13);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(36, 13);
            this.label6.TabIndex = 23;
            this.label6.Text = "Casts:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 13);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(38, 13);
            this.label5.TabIndex = 22;
            this.label5.Text = "Spells:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 32);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 13);
            this.label3.TabIndex = 21;
            this.label3.Text = "Flash Heal";
            // 
            // CalculationOptionsPanelHolyPriest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.tabControl1);
            this.Name = "CalculationOptionsPanelHolyPriest";
            this.Size = new System.Drawing.Size(285, 652);
            ((System.ComponentModel.ISupportInitialize)(this.trkActivity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkFightLength)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkSerendipity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkReplenishment)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkShadowfiend)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkSurvivability)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkTestOfFaith)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkRapture)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown4)).EndInit();
            this.panelCustom.ResumeLayout(false);
            this.panelCustom.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TrackBar trkActivity;
        private System.Windows.Forms.Label lblActivity;
        private System.Windows.Forms.ComboBox cbRotation;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblFightLength;
        private System.Windows.Forms.TrackBar trkFightLength;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.TrackBar trkSerendipity;
        private System.Windows.Forms.Label lblSerendipity;
        private System.Windows.Forms.Label lblReplenishment;
        private System.Windows.Forms.TrackBar trkReplenishment;
        private System.Windows.Forms.Label lblShadowfiend;
        private System.Windows.Forms.TrackBar trkShadowfiend;
        private System.Windows.Forms.Label lblSurvivability;
        private System.Windows.Forms.TrackBar trkSurvivability;
        private System.Windows.Forms.TrackBar trkRapture;
        private System.Windows.Forms.Label lblRapture;
        private System.Windows.Forms.CheckBox cbModelProcs;
        private System.Windows.Forms.Label lblTestOfFaith;
        private System.Windows.Forms.TrackBar trkTestOfFaith;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmbManaAmt;
        private System.Windows.Forms.CheckBox cbNewMana;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.NumericUpDown numericUpDown5;
        private System.Windows.Forms.NumericUpDown numericUpDown4;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Panel panelCustom;
        private System.Windows.Forms.NumericUpDown numericUpDown7;
        private System.Windows.Forms.NumericUpDown numericUpDown6;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.NumericUpDown numericUpDown3;
        private System.Windows.Forms.NumericUpDown numericUpDown2;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label3;
    }
}
