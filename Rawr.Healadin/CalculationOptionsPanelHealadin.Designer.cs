namespace Rawr.Healadin
{
    partial class CalculationOptionsPanelHealadin
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
            this.cmbLength = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chkLoHSelf = new System.Windows.Forms.CheckBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblReplenishment = new System.Windows.Forms.Label();
            this.trkReplenishment = new System.Windows.Forms.TrackBar();
            this.nudDivinePlea = new System.Windows.Forms.NumericUpDown();
            this.lblDivinePlea = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbManaAmt = new System.Windows.Forms.ComboBox();
            this.trkActivity = new System.Windows.Forms.TrackBar();
            this.lblActivity = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.lblBoLEff = new System.Windows.Forms.Label();
            this.trkBoLEff = new System.Windows.Forms.TrackBar();
            this.label5 = new System.Windows.Forms.Label();
            this.lblBoLUp = new System.Windows.Forms.Label();
            this.trkBoLUp = new System.Windows.Forms.TrackBar();
            this.trkHS = new System.Windows.Forms.TrackBar();
            this.lblHS = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.chkGlyphBeaconOfLight = new System.Windows.Forms.CheckBox();
            this.chkGlyphHolyShock = new System.Windows.Forms.CheckBox();
            this.chkGlyphFlashOfLight = new System.Windows.Forms.CheckBox();
            this.chkGlyphDivinity = new System.Windows.Forms.CheckBox();
            this.chkGlyphSealOfLight = new System.Windows.Forms.CheckBox();
            this.chkGlyphSealOfWisdom = new System.Windows.Forms.CheckBox();
            this.chkGlyphHolyLight = new System.Windows.Forms.CheckBox();
            this.chkJotP = new System.Windows.Forms.CheckBox();
            this.label12 = new System.Windows.Forms.Label();
            this.nudGHL = new System.Windows.Forms.NumericUpDown();
            this.label19 = new System.Windows.Forms.Label();
            this.lblBurstScale = new System.Windows.Forms.Label();
            this.trkBurstScale = new System.Windows.Forms.TrackBar();
            ((System.ComponentModel.ISupportInitialize)(this.cmbLength)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trkReplenishment)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDivinePlea)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkActivity)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trkBoLEff)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkBoLUp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkHS)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudGHL)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkBurstScale)).BeginInit();
            this.SuspendLayout();
            // 
            // cmbLength
            // 
            this.cmbLength.DecimalPlaces = 1;
            this.cmbLength.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.cmbLength.Location = new System.Drawing.Point(6, 22);
            this.cmbLength.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.cmbLength.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.cmbLength.Name = "cmbLength";
            this.cmbLength.Size = new System.Drawing.Size(112, 20);
            this.cmbLength.TabIndex = 20;
            this.cmbLength.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.cmbLength.ValueChanged += new System.EventHandler(this.cmbLength_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 6);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 13);
            this.label3.TabIndex = 21;
            this.label3.Text = "Fight Length:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chkLoHSelf);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.lblReplenishment);
            this.groupBox2.Controls.Add(this.trkReplenishment);
            this.groupBox2.Controls.Add(this.nudDivinePlea);
            this.groupBox2.Controls.Add(this.lblDivinePlea);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.cmbManaAmt);
            this.groupBox2.Location = new System.Drawing.Point(3, 240);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(294, 123);
            this.groupBox2.TabIndex = 22;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Mana Buffs";
            // 
            // chkLoHSelf
            // 
            this.chkLoHSelf.AutoSize = true;
            this.chkLoHSelf.Location = new System.Drawing.Point(151, 58);
            this.chkLoHSelf.Name = "chkLoHSelf";
            this.chkLoHSelf.Size = new System.Drawing.Size(106, 17);
            this.chkLoHSelf.TabIndex = 31;
            this.chkLoHSelf.Text = "Lay on Hand self";
            this.chkLoHSelf.UseVisualStyleBackColor = true;
            this.chkLoHSelf.CheckedChanged += new System.EventHandler(this.chkLoHSelf_CheckedChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(224, 34);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(41, 13);
            this.label11.TabIndex = 30;
            this.label11.Text = "min CD";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(92, 35);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(33, 13);
            this.label10.TabIndex = 29;
            this.label10.Text = "mana";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(116, 13);
            this.label2.TabIndex = 27;
            this.label2.Text = "Replenishment Uptime:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblReplenishment
            // 
            this.lblReplenishment.AutoSize = true;
            this.lblReplenishment.Location = new System.Drawing.Point(110, 104);
            this.lblReplenishment.Name = "lblReplenishment";
            this.lblReplenishment.Size = new System.Drawing.Size(33, 13);
            this.lblReplenishment.TabIndex = 26;
            this.lblReplenishment.Text = "100%";
            // 
            // trkReplenishment
            // 
            this.trkReplenishment.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.trkReplenishment.Location = new System.Drawing.Point(6, 72);
            this.trkReplenishment.Maximum = 100;
            this.trkReplenishment.Name = "trkReplenishment";
            this.trkReplenishment.Size = new System.Drawing.Size(137, 45);
            this.trkReplenishment.TabIndex = 25;
            this.trkReplenishment.TickFrequency = 10;
            this.trkReplenishment.Value = 90;
            this.trkReplenishment.Scroll += new System.EventHandler(this.trkReplenishment_Scroll);
            // 
            // nudDivinePlea
            // 
            this.nudDivinePlea.DecimalPlaces = 1;
            this.nudDivinePlea.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nudDivinePlea.Location = new System.Drawing.Point(151, 32);
            this.nudDivinePlea.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.nudDivinePlea.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudDivinePlea.Name = "nudDivinePlea";
            this.nudDivinePlea.Size = new System.Drawing.Size(71, 20);
            this.nudDivinePlea.TabIndex = 10;
            this.nudDivinePlea.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudDivinePlea.ValueChanged += new System.EventHandler(this.nudDivinePlea_ValueChanged);
            // 
            // lblDivinePlea
            // 
            this.lblDivinePlea.AutoSize = true;
            this.lblDivinePlea.Location = new System.Drawing.Point(148, 16);
            this.lblDivinePlea.Name = "lblDivinePlea";
            this.lblDivinePlea.Size = new System.Drawing.Size(64, 13);
            this.lblDivinePlea.TabIndex = 9;
            this.lblDivinePlea.Text = "Divine Plea:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(70, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Mana Potion:";
            // 
            // cmbManaAmt
            // 
            this.cmbManaAmt.DisplayMember = "4300";
            this.cmbManaAmt.Items.AddRange(new object[] {
            "0",
            "4300",
            "5400"});
            this.cmbManaAmt.Location = new System.Drawing.Point(6, 32);
            this.cmbManaAmt.Name = "cmbManaAmt";
            this.cmbManaAmt.Size = new System.Drawing.Size(80, 21);
            this.cmbManaAmt.TabIndex = 0;
            this.cmbManaAmt.Text = "4300";
            this.cmbManaAmt.ValueMember = "4300";
            this.cmbManaAmt.SelectedIndexChanged += new System.EventHandler(this.cmbManaAmt_SelectedIndexChanged);
            this.cmbManaAmt.TextUpdate += new System.EventHandler(this.cmbManaAmt_TextUpdate);
            // 
            // trkActivity
            // 
            this.trkActivity.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.trkActivity.Location = new System.Drawing.Point(3, 61);
            this.trkActivity.Maximum = 100;
            this.trkActivity.Minimum = 10;
            this.trkActivity.Name = "trkActivity";
            this.trkActivity.Size = new System.Drawing.Size(137, 45);
            this.trkActivity.TabIndex = 23;
            this.trkActivity.TickFrequency = 10;
            this.trkActivity.Value = 90;
            this.trkActivity.Scroll += new System.EventHandler(this.trkActivity_Scroll);
            // 
            // lblActivity
            // 
            this.lblActivity.AutoSize = true;
            this.lblActivity.Location = new System.Drawing.Point(107, 95);
            this.lblActivity.Name = "lblActivity";
            this.lblActivity.Size = new System.Drawing.Size(33, 13);
            this.lblActivity.TabIndex = 24;
            this.lblActivity.Text = "100%";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(3, 45);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(44, 13);
            this.label9.TabIndex = 25;
            this.label9.Text = "Activity:";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.lblBoLEff);
            this.groupBox1.Controls.Add(this.trkBoLEff);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.lblBoLUp);
            this.groupBox1.Controls.Add(this.trkBoLUp);
            this.groupBox1.Location = new System.Drawing.Point(3, 369);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(294, 87);
            this.groupBox1.TabIndex = 28;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Beacon of Light";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(148, 18);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(74, 13);
            this.label6.TabIndex = 33;
            this.label6.Text = "Effectiveness:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblBoLEff
            // 
            this.lblBoLEff.AutoSize = true;
            this.lblBoLEff.Location = new System.Drawing.Point(255, 64);
            this.lblBoLEff.Name = "lblBoLEff";
            this.lblBoLEff.Size = new System.Drawing.Size(33, 13);
            this.lblBoLEff.TabIndex = 32;
            this.lblBoLEff.Text = "100%";
            // 
            // trkBoLEff
            // 
            this.trkBoLEff.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.trkBoLEff.Location = new System.Drawing.Point(151, 32);
            this.trkBoLEff.Maximum = 100;
            this.trkBoLEff.Name = "trkBoLEff";
            this.trkBoLEff.Size = new System.Drawing.Size(137, 45);
            this.trkBoLEff.TabIndex = 31;
            this.trkBoLEff.TickFrequency = 10;
            this.trkBoLEff.Value = 90;
            this.trkBoLEff.Scroll += new System.EventHandler(this.trkBoLEff_Scroll);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 16);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(43, 13);
            this.label5.TabIndex = 30;
            this.label5.Text = "Uptime:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblBoLUp
            // 
            this.lblBoLUp.AutoSize = true;
            this.lblBoLUp.Location = new System.Drawing.Point(110, 66);
            this.lblBoLUp.Name = "lblBoLUp";
            this.lblBoLUp.Size = new System.Drawing.Size(33, 13);
            this.lblBoLUp.TabIndex = 29;
            this.lblBoLUp.Text = "100%";
            // 
            // trkBoLUp
            // 
            this.trkBoLUp.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.trkBoLUp.Location = new System.Drawing.Point(6, 32);
            this.trkBoLUp.Maximum = 100;
            this.trkBoLUp.Name = "trkBoLUp";
            this.trkBoLUp.Size = new System.Drawing.Size(137, 45);
            this.trkBoLUp.TabIndex = 28;
            this.trkBoLUp.TickFrequency = 10;
            this.trkBoLUp.Value = 90;
            this.trkBoLUp.Scroll += new System.EventHandler(this.trkBoLUp_Scroll);
            // 
            // trkHS
            // 
            this.trkHS.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.trkHS.LargeChange = 10;
            this.trkHS.Location = new System.Drawing.Point(3, 125);
            this.trkHS.Maximum = 100;
            this.trkHS.Name = "trkHS";
            this.trkHS.Size = new System.Drawing.Size(137, 45);
            this.trkHS.TabIndex = 29;
            this.trkHS.TickFrequency = 10;
            this.trkHS.Value = 20;
            this.trkHS.Scroll += new System.EventHandler(this.trkHS_Scroll);
            // 
            // lblHS
            // 
            this.lblHS.Location = new System.Drawing.Point(107, 157);
            this.lblHS.Name = "lblHS";
            this.lblHS.Size = new System.Drawing.Size(33, 13);
            this.lblHS.TabIndex = 34;
            this.lblHS.Text = "100%";
            this.lblHS.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(3, 109);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 13);
            this.label7.TabIndex = 35;
            this.label7.Text = "Holy Shock:";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.chkGlyphBeaconOfLight);
            this.groupBox3.Controls.Add(this.chkGlyphHolyShock);
            this.groupBox3.Controls.Add(this.chkGlyphFlashOfLight);
            this.groupBox3.Controls.Add(this.chkGlyphDivinity);
            this.groupBox3.Controls.Add(this.chkGlyphSealOfLight);
            this.groupBox3.Controls.Add(this.chkGlyphSealOfWisdom);
            this.groupBox3.Controls.Add(this.chkGlyphHolyLight);
            this.groupBox3.Location = new System.Drawing.Point(147, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(150, 182);
            this.groupBox3.TabIndex = 36;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Glyphs";
            // 
            // chkGlyphBeaconOfLight
            // 
            this.chkGlyphBeaconOfLight.AutoSize = true;
            this.chkGlyphBeaconOfLight.Location = new System.Drawing.Point(6, 157);
            this.chkGlyphBeaconOfLight.Name = "chkGlyphBeaconOfLight";
            this.chkGlyphBeaconOfLight.Size = new System.Drawing.Size(101, 17);
            this.chkGlyphBeaconOfLight.TabIndex = 7;
            this.chkGlyphBeaconOfLight.Text = "Beacon of Light";
            this.chkGlyphBeaconOfLight.UseVisualStyleBackColor = true;
            this.chkGlyphBeaconOfLight.CheckedChanged += new System.EventHandler(this.chkGlyphBeaconOfLight_CheckedChanged);
            // 
            // chkGlyphHolyShock
            // 
            this.chkGlyphHolyShock.AutoSize = true;
            this.chkGlyphHolyShock.Location = new System.Drawing.Point(6, 134);
            this.chkGlyphHolyShock.Name = "chkGlyphHolyShock";
            this.chkGlyphHolyShock.Size = new System.Drawing.Size(81, 17);
            this.chkGlyphHolyShock.TabIndex = 6;
            this.chkGlyphHolyShock.Text = "Holy Shock";
            this.chkGlyphHolyShock.UseVisualStyleBackColor = true;
            this.chkGlyphHolyShock.CheckedChanged += new System.EventHandler(this.chkGlyphHolyShock_CheckedChanged);
            // 
            // chkGlyphFlashOfLight
            // 
            this.chkGlyphFlashOfLight.AutoSize = true;
            this.chkGlyphFlashOfLight.Location = new System.Drawing.Point(6, 65);
            this.chkGlyphFlashOfLight.Name = "chkGlyphFlashOfLight";
            this.chkGlyphFlashOfLight.Size = new System.Drawing.Size(89, 17);
            this.chkGlyphFlashOfLight.TabIndex = 5;
            this.chkGlyphFlashOfLight.Text = "Flash of Light";
            this.chkGlyphFlashOfLight.UseVisualStyleBackColor = true;
            this.chkGlyphFlashOfLight.CheckedChanged += new System.EventHandler(this.chkGFoL_CheckedChanged);
            // 
            // chkGlyphDivinity
            // 
            this.chkGlyphDivinity.AutoSize = true;
            this.chkGlyphDivinity.Location = new System.Drawing.Point(6, 42);
            this.chkGlyphDivinity.Name = "chkGlyphDivinity";
            this.chkGlyphDivinity.Size = new System.Drawing.Size(60, 17);
            this.chkGlyphDivinity.TabIndex = 3;
            this.chkGlyphDivinity.Text = "Divinity";
            this.chkGlyphDivinity.UseVisualStyleBackColor = true;
            this.chkGlyphDivinity.CheckedChanged += new System.EventHandler(this.chkGDivinity_CheckedChanged);
            // 
            // chkGlyphSealOfLight
            // 
            this.chkGlyphSealOfLight.AutoSize = true;
            this.chkGlyphSealOfLight.Location = new System.Drawing.Point(6, 88);
            this.chkGlyphSealOfLight.Name = "chkGlyphSealOfLight";
            this.chkGlyphSealOfLight.Size = new System.Drawing.Size(85, 17);
            this.chkGlyphSealOfLight.TabIndex = 2;
            this.chkGlyphSealOfLight.Text = "Seal of Light";
            this.chkGlyphSealOfLight.UseVisualStyleBackColor = true;
            this.chkGlyphSealOfLight.CheckedChanged += new System.EventHandler(this.chkGSoL_CheckedChanged);
            // 
            // chkGlyphSealOfWisdom
            // 
            this.chkGlyphSealOfWisdom.AutoSize = true;
            this.chkGlyphSealOfWisdom.Location = new System.Drawing.Point(6, 111);
            this.chkGlyphSealOfWisdom.Name = "chkGlyphSealOfWisdom";
            this.chkGlyphSealOfWisdom.Size = new System.Drawing.Size(100, 17);
            this.chkGlyphSealOfWisdom.TabIndex = 1;
            this.chkGlyphSealOfWisdom.Text = "Seal of Wisdom";
            this.chkGlyphSealOfWisdom.UseVisualStyleBackColor = true;
            this.chkGlyphSealOfWisdom.CheckedChanged += new System.EventHandler(this.chkGSoW_CheckedChanged);
            // 
            // chkGlyphHolyLight
            // 
            this.chkGlyphHolyLight.AutoSize = true;
            this.chkGlyphHolyLight.Location = new System.Drawing.Point(6, 19);
            this.chkGlyphHolyLight.Name = "chkGlyphHolyLight";
            this.chkGlyphHolyLight.Size = new System.Drawing.Size(73, 17);
            this.chkGlyphHolyLight.TabIndex = 0;
            this.chkGlyphHolyLight.Text = "Holy Light";
            this.chkGlyphHolyLight.UseVisualStyleBackColor = true;
            this.chkGlyphHolyLight.CheckedChanged += new System.EventHandler(this.chkGHL_CheckedChanged);
            // 
            // chkJotP
            // 
            this.chkJotP.AutoSize = true;
            this.chkJotP.Location = new System.Drawing.Point(153, 217);
            this.chkJotP.Name = "chkJotP";
            this.chkJotP.Size = new System.Drawing.Size(90, 17);
            this.chkJotP.TabIndex = 37;
            this.chkJotP.Text = "Maintain JotP";
            this.chkJotP.UseVisualStyleBackColor = true;
            this.chkJotP.CheckedChanged += new System.EventHandler(this.chkJotP_CheckedChanged);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(151, 193);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(101, 13);
            this.label12.TabIndex = 38;
            this.label12.Text = "Glyph of HL targets:";
            // 
            // nudGHL
            // 
            this.nudGHL.DecimalPlaces = 1;
            this.nudGHL.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nudGHL.Location = new System.Drawing.Point(258, 191);
            this.nudGHL.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.nudGHL.Name = "nudGHL";
            this.nudGHL.Size = new System.Drawing.Size(37, 20);
            this.nudGHL.TabIndex = 39;
            this.nudGHL.ValueChanged += new System.EventHandler(this.nudGHL_ValueChanged);
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(3, 173);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(64, 13);
            this.label19.TabIndex = 44;
            this.label19.Text = "Burst Scale:";
            this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblBurstScale
            // 
            this.lblBurstScale.AutoSize = true;
            this.lblBurstScale.Location = new System.Drawing.Point(107, 221);
            this.lblBurstScale.Name = "lblBurstScale";
            this.lblBurstScale.Size = new System.Drawing.Size(33, 13);
            this.lblBurstScale.TabIndex = 43;
            this.lblBurstScale.Text = "100%";
            // 
            // trkBurstScale
            // 
            this.trkBurstScale.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.trkBurstScale.Location = new System.Drawing.Point(3, 189);
            this.trkBurstScale.Maximum = 100;
            this.trkBurstScale.Name = "trkBurstScale";
            this.trkBurstScale.Size = new System.Drawing.Size(137, 45);
            this.trkBurstScale.TabIndex = 42;
            this.trkBurstScale.TickFrequency = 10;
            this.trkBurstScale.Value = 90;
            this.trkBurstScale.Scroll += new System.EventHandler(this.trkBurstScale_Scroll);
            // 
            // CalculationOptionsPanelHealadin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label19);
            this.Controls.Add(this.lblBurstScale);
            this.Controls.Add(this.trkBurstScale);
            this.Controls.Add(this.nudGHL);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.chkJotP);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.lblHS);
            this.Controls.Add(this.trkHS);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.lblActivity);
            this.Controls.Add(this.trkActivity);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cmbLength);
            this.Name = "CalculationOptionsPanelHealadin";
            this.Size = new System.Drawing.Size(300, 464);
            ((System.ComponentModel.ISupportInitialize)(this.cmbLength)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trkReplenishment)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDivinePlea)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkActivity)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trkBoLEff)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkBoLUp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkHS)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudGHL)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkBurstScale)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown cmbLength;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox cmbManaAmt;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TrackBar trkActivity;
        private System.Windows.Forms.Label lblActivity;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.NumericUpDown nudDivinePlea;
        private System.Windows.Forms.Label lblDivinePlea;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblReplenishment;
        private System.Windows.Forms.TrackBar trkReplenishment;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lblBoLEff;
        private System.Windows.Forms.TrackBar trkBoLEff;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblBoLUp;
        private System.Windows.Forms.TrackBar trkBoLUp;
        private System.Windows.Forms.TrackBar trkHS;
        private System.Windows.Forms.Label lblHS;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox chkGlyphDivinity;
        private System.Windows.Forms.CheckBox chkGlyphSealOfLight;
        private System.Windows.Forms.CheckBox chkGlyphSealOfWisdom;
        private System.Windows.Forms.CheckBox chkGlyphHolyLight;
        private System.Windows.Forms.CheckBox chkLoHSelf;
        private System.Windows.Forms.CheckBox chkGlyphFlashOfLight;
        private System.Windows.Forms.CheckBox chkJotP;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.NumericUpDown nudGHL;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label lblBurstScale;
        private System.Windows.Forms.TrackBar trkBurstScale;
        private System.Windows.Forms.CheckBox chkGlyphBeaconOfLight;
        private System.Windows.Forms.CheckBox chkGlyphHolyShock;
    }
}
