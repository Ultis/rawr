namespace Rawr.Moonkin
{
    partial class CalculationOptionsPanelMoonkin
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
            this.lblTargetLevel = new System.Windows.Forms.Label();
            this.cmbTargetLevel = new System.Windows.Forms.ComboBox();
            this.txtLatency = new System.Windows.Forms.TextBox();
            this.lblLatency = new System.Windows.Forms.Label();
            this.txtFightLength = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.chkInnervate = new System.Windows.Forms.CheckBox();
            this.chkManaPots = new System.Windows.Forms.CheckBox();
            this.cmbPotType = new System.Windows.Forms.ComboBox();
            this.lblInnervateOffset = new System.Windows.Forms.Label();
            this.txtInnervateDelay = new System.Windows.Forms.TextBox();
            this.lblManaPotType = new System.Windows.Forms.Label();
            this.chkInnervateWeapon = new System.Windows.Forms.CheckBox();
            this.txtInnervateWeaponInt = new System.Windows.Forms.TextBox();
            this.txtInnervateWeaponSpi = new System.Windows.Forms.TextBox();
            this.lblInnervateWeaponInt = new System.Windows.Forms.Label();
            this.lblInnervateWeaponSpi = new System.Windows.Forms.Label();
            this.rdbAldor = new System.Windows.Forms.RadioButton();
            this.rdbScryer = new System.Windows.Forms.RadioButton();
            this.trkReplenishmentUptime = new System.Windows.Forms.TrackBar();
            this.trkTreantLifespan = new System.Windows.Forms.TrackBar();
            this.lblReplenishmentUptime = new System.Windows.Forms.Label();
            this.lblTreantLifespan = new System.Windows.Forms.Label();
            this.lblUptimeValue = new System.Windows.Forms.Label();
            this.lblLifespanValue = new System.Windows.Forms.Label();
            this.lblUserRotation = new System.Windows.Forms.Label();
            this.cmbUserRotation = new System.Windows.Forms.ComboBox();
            this.lblEclipseType = new System.Windows.Forms.Label();
            this.cmbEclipseType = new System.Windows.Forms.ComboBox();
            this.chkMoonfireAlways = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.trkReplenishmentUptime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkTreantLifespan)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTargetLevel
            // 
            this.lblTargetLevel.AutoSize = true;
            this.lblTargetLevel.Location = new System.Drawing.Point(3, 6);
            this.lblTargetLevel.Name = "lblTargetLevel";
            this.lblTargetLevel.Size = new System.Drawing.Size(70, 13);
            this.lblTargetLevel.TabIndex = 0;
            this.lblTargetLevel.Text = "Target Level:";
            // 
            // cmbTargetLevel
            // 
            this.cmbTargetLevel.FormattingEnabled = true;
            this.cmbTargetLevel.Items.AddRange(new object[] {
            "80",
            "81",
            "82",
            "83"});
            this.cmbTargetLevel.Location = new System.Drawing.Point(108, 3);
            this.cmbTargetLevel.Name = "cmbTargetLevel";
            this.cmbTargetLevel.Size = new System.Drawing.Size(93, 21);
            this.cmbTargetLevel.TabIndex = 1;
            this.cmbTargetLevel.SelectedIndexChanged += new System.EventHandler(this.cmbTargetLevel_SelectedIndexChanged);
            // 
            // txtLatency
            // 
            this.txtLatency.Location = new System.Drawing.Point(108, 30);
            this.txtLatency.Name = "txtLatency";
            this.txtLatency.Size = new System.Drawing.Size(93, 20);
            this.txtLatency.TabIndex = 2;
            this.txtLatency.Leave += new System.EventHandler(this.txtLatency_TextChanged);
            // 
            // lblLatency
            // 
            this.lblLatency.AutoSize = true;
            this.lblLatency.Location = new System.Drawing.Point(3, 33);
            this.lblLatency.Name = "lblLatency";
            this.lblLatency.Size = new System.Drawing.Size(48, 13);
            this.lblLatency.TabIndex = 3;
            this.lblLatency.Text = "Latency:";
            // 
            // txtFightLength
            // 
            this.txtFightLength.Location = new System.Drawing.Point(108, 57);
            this.txtFightLength.Name = "txtFightLength";
            this.txtFightLength.Size = new System.Drawing.Size(93, 20);
            this.txtFightLength.TabIndex = 3;
            this.txtFightLength.Leave += new System.EventHandler(this.txtFightLength_Leave);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 60);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(105, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Est. Fight Time (min):";
            // 
            // chkInnervate
            // 
            this.chkInnervate.AutoSize = true;
            this.chkInnervate.Location = new System.Drawing.Point(6, 83);
            this.chkInnervate.Name = "chkInnervate";
            this.chkInnervate.Size = new System.Drawing.Size(135, 17);
            this.chkInnervate.TabIndex = 4;
            this.chkInnervate.Text = "Cast Innervate on self?";
            this.chkInnervate.UseVisualStyleBackColor = true;
            this.chkInnervate.CheckedChanged += new System.EventHandler(this.chkInnervate_CheckedChanged);
            // 
            // chkManaPots
            // 
            this.chkManaPots.AutoSize = true;
            this.chkManaPots.Location = new System.Drawing.Point(6, 132);
            this.chkManaPots.Name = "chkManaPots";
            this.chkManaPots.Size = new System.Drawing.Size(117, 17);
            this.chkManaPots.TabIndex = 6;
            this.chkManaPots.Text = "Use mana potions?";
            this.chkManaPots.UseVisualStyleBackColor = true;
            this.chkManaPots.CheckedChanged += new System.EventHandler(this.chkManaPots_CheckedChanged);
            // 
            // cmbPotType
            // 
            this.cmbPotType.FormattingEnabled = true;
            this.cmbPotType.Items.AddRange(new object[] {
            "Runic Mana Potion",
            "Fel Mana Potion"});
            this.cmbPotType.Location = new System.Drawing.Point(108, 155);
            this.cmbPotType.Name = "cmbPotType";
            this.cmbPotType.Size = new System.Drawing.Size(93, 21);
            this.cmbPotType.TabIndex = 7;
            this.cmbPotType.SelectedIndexChanged += new System.EventHandler(this.cmbPotType_SelectedIndexChanged);
            // 
            // lblInnervateOffset
            // 
            this.lblInnervateOffset.AutoSize = true;
            this.lblInnervateOffset.Location = new System.Drawing.Point(3, 109);
            this.lblInnervateOffset.Name = "lblInnervateOffset";
            this.lblInnervateOffset.Size = new System.Drawing.Size(85, 13);
            this.lblInnervateOffset.TabIndex = 17;
            this.lblInnervateOffset.Text = "Innervate Delay:";
            // 
            // txtInnervateDelay
            // 
            this.txtInnervateDelay.Location = new System.Drawing.Point(108, 106);
            this.txtInnervateDelay.Name = "txtInnervateDelay";
            this.txtInnervateDelay.Size = new System.Drawing.Size(93, 20);
            this.txtInnervateDelay.TabIndex = 5;
            this.txtInnervateDelay.Leave += new System.EventHandler(this.txtInnervateDelay_Leave);
            // 
            // lblManaPotType
            // 
            this.lblManaPotType.AutoSize = true;
            this.lblManaPotType.Location = new System.Drawing.Point(3, 158);
            this.lblManaPotType.Name = "lblManaPotType";
            this.lblManaPotType.Size = new System.Drawing.Size(97, 13);
            this.lblManaPotType.TabIndex = 21;
            this.lblManaPotType.Text = "Mana Potion Type:";
            // 
            // chkInnervateWeapon
            // 
            this.chkInnervateWeapon.AutoSize = true;
            this.chkInnervateWeapon.Location = new System.Drawing.Point(6, 182);
            this.chkInnervateWeapon.Name = "chkInnervateWeapon";
            this.chkInnervateWeapon.Size = new System.Drawing.Size(140, 17);
            this.chkInnervateWeapon.TabIndex = 8;
            this.chkInnervateWeapon.Text = "Use Innervate weapon?";
            this.chkInnervateWeapon.UseVisualStyleBackColor = true;
            this.chkInnervateWeapon.CheckedChanged += new System.EventHandler(this.chkInnervateWeapon_CheckedChanged);
            // 
            // txtInnervateWeaponInt
            // 
            this.txtInnervateWeaponInt.Location = new System.Drawing.Point(108, 205);
            this.txtInnervateWeaponInt.Name = "txtInnervateWeaponInt";
            this.txtInnervateWeaponInt.Size = new System.Drawing.Size(93, 20);
            this.txtInnervateWeaponInt.TabIndex = 9;
            this.txtInnervateWeaponInt.Leave += new System.EventHandler(this.txtInnervateWeaponInt_Leave);
            // 
            // txtInnervateWeaponSpi
            // 
            this.txtInnervateWeaponSpi.Location = new System.Drawing.Point(108, 231);
            this.txtInnervateWeaponSpi.Name = "txtInnervateWeaponSpi";
            this.txtInnervateWeaponSpi.Size = new System.Drawing.Size(93, 20);
            this.txtInnervateWeaponSpi.TabIndex = 10;
            this.txtInnervateWeaponSpi.Leave += new System.EventHandler(this.txtInnervateWeaponSpi_Leave);
            // 
            // lblInnervateWeaponInt
            // 
            this.lblInnervateWeaponInt.AutoSize = true;
            this.lblInnervateWeaponInt.Location = new System.Drawing.Point(3, 208);
            this.lblInnervateWeaponInt.Name = "lblInnervateWeaponInt";
            this.lblInnervateWeaponInt.Size = new System.Drawing.Size(76, 13);
            this.lblInnervateWeaponInt.TabIndex = 25;
            this.lblInnervateWeaponInt.Text = "Intellect value:";
            // 
            // lblInnervateWeaponSpi
            // 
            this.lblInnervateWeaponSpi.AutoSize = true;
            this.lblInnervateWeaponSpi.Location = new System.Drawing.Point(3, 234);
            this.lblInnervateWeaponSpi.Name = "lblInnervateWeaponSpi";
            this.lblInnervateWeaponSpi.Size = new System.Drawing.Size(62, 13);
            this.lblInnervateWeaponSpi.TabIndex = 26;
            this.lblInnervateWeaponSpi.Text = "Spirit value:";
            // 
            // rdbAldor
            // 
            this.rdbAldor.AutoSize = true;
            this.rdbAldor.Location = new System.Drawing.Point(39, 353);
            this.rdbAldor.Name = "rdbAldor";
            this.rdbAldor.Size = new System.Drawing.Size(49, 17);
            this.rdbAldor.TabIndex = 13;
            this.rdbAldor.TabStop = true;
            this.rdbAldor.Text = "Aldor";
            this.rdbAldor.UseVisualStyleBackColor = true;
            // 
            // rdbScryer
            // 
            this.rdbScryer.AutoSize = true;
            this.rdbScryer.Location = new System.Drawing.Point(108, 353);
            this.rdbScryer.Name = "rdbScryer";
            this.rdbScryer.Size = new System.Drawing.Size(55, 17);
            this.rdbScryer.TabIndex = 14;
            this.rdbScryer.TabStop = true;
            this.rdbScryer.Text = "Scryer";
            this.rdbScryer.UseVisualStyleBackColor = true;
            this.rdbScryer.CheckedChanged += new System.EventHandler(this.rdbScryer_CheckedChanged);
            // 
            // trkReplenishmentUptime
            // 
            this.trkReplenishmentUptime.Location = new System.Drawing.Point(125, 257);
            this.trkReplenishmentUptime.Maximum = 100;
            this.trkReplenishmentUptime.Name = "trkReplenishmentUptime";
            this.trkReplenishmentUptime.Size = new System.Drawing.Size(76, 42);
            this.trkReplenishmentUptime.TabIndex = 11;
            this.trkReplenishmentUptime.TickFrequency = 10;
            this.trkReplenishmentUptime.Value = 100;
            this.trkReplenishmentUptime.ValueChanged += new System.EventHandler(this.trkReplenishmentUptime_ValueChanged);
            // 
            // trkTreantLifespan
            // 
            this.trkTreantLifespan.Location = new System.Drawing.Point(128, 305);
            this.trkTreantLifespan.Maximum = 100;
            this.trkTreantLifespan.Name = "trkTreantLifespan";
            this.trkTreantLifespan.Size = new System.Drawing.Size(73, 42);
            this.trkTreantLifespan.TabIndex = 12;
            this.trkTreantLifespan.TickFrequency = 10;
            this.trkTreantLifespan.Value = 50;
            this.trkTreantLifespan.ValueChanged += new System.EventHandler(this.trkTreantLifespan_ValueChanged);
            // 
            // lblReplenishmentUptime
            // 
            this.lblReplenishmentUptime.AutoSize = true;
            this.lblReplenishmentUptime.Location = new System.Drawing.Point(3, 267);
            this.lblReplenishmentUptime.Name = "lblReplenishmentUptime";
            this.lblReplenishmentUptime.Size = new System.Drawing.Size(116, 13);
            this.lblReplenishmentUptime.TabIndex = 31;
            this.lblReplenishmentUptime.Text = "Replenishment Uptime:";
            // 
            // lblTreantLifespan
            // 
            this.lblTreantLifespan.AutoSize = true;
            this.lblTreantLifespan.Location = new System.Drawing.Point(3, 316);
            this.lblTreantLifespan.Name = "lblTreantLifespan";
            this.lblTreantLifespan.Size = new System.Drawing.Size(84, 13);
            this.lblTreantLifespan.TabIndex = 32;
            this.lblTreantLifespan.Text = "Treant Lifespan:";
            // 
            // lblUptimeValue
            // 
            this.lblUptimeValue.AutoSize = true;
            this.lblUptimeValue.Location = new System.Drawing.Point(127, 286);
            this.lblUptimeValue.Name = "lblUptimeValue";
            this.lblUptimeValue.Size = new System.Drawing.Size(25, 13);
            this.lblUptimeValue.TabIndex = 33;
            this.lblUptimeValue.Text = "100";
            // 
            // lblLifespanValue
            // 
            this.lblLifespanValue.AutoSize = true;
            this.lblLifespanValue.Location = new System.Drawing.Point(127, 334);
            this.lblLifespanValue.Name = "lblLifespanValue";
            this.lblLifespanValue.Size = new System.Drawing.Size(19, 13);
            this.lblLifespanValue.TabIndex = 34;
            this.lblLifespanValue.Text = "50";
            // 
            // lblUserRotation
            // 
            this.lblUserRotation.AutoSize = true;
            this.lblUserRotation.Location = new System.Drawing.Point(6, 429);
            this.lblUserRotation.Name = "lblUserRotation";
            this.lblUserRotation.Size = new System.Drawing.Size(75, 13);
            this.lblUserRotation.TabIndex = 44;
            this.lblUserRotation.Text = "User Rotation:";
            // 
            // cmbUserRotation
            // 
            this.cmbUserRotation.FormattingEnabled = true;
            this.cmbUserRotation.Items.AddRange(new object[] {
            "None",
            "IS/W",
            "MF/W",
            "IS/SF",
            "MF/SF",
            "IS/MF/W",
            "IS/MF/SF",
            "SF Spam",
            "W Spam"});
            this.cmbUserRotation.Location = new System.Drawing.Point(108, 426);
            this.cmbUserRotation.Name = "cmbUserRotation";
            this.cmbUserRotation.Size = new System.Drawing.Size(93, 21);
            this.cmbUserRotation.TabIndex = 21;
            this.cmbUserRotation.SelectedIndexChanged += new System.EventHandler(this.cmbUserRotation_SelectedIndexChanged);
            // 
            // lblEclipseType
            // 
            this.lblEclipseType.AutoSize = true;
            this.lblEclipseType.Location = new System.Drawing.Point(3, 379);
            this.lblEclipseType.Name = "lblEclipseType";
            this.lblEclipseType.Size = new System.Drawing.Size(68, 13);
            this.lblEclipseType.TabIndex = 46;
            this.lblEclipseType.Text = "Eclipse Type";
            // 
            // cmbEclipseType
            // 
            this.cmbEclipseType.FormattingEnabled = true;
            this.cmbEclipseType.Items.AddRange(new object[] {
            "Lunar",
            "Solar"});
            this.cmbEclipseType.Location = new System.Drawing.Point(108, 376);
            this.cmbEclipseType.Name = "cmbEclipseType";
            this.cmbEclipseType.Size = new System.Drawing.Size(93, 21);
            this.cmbEclipseType.TabIndex = 19;
            this.cmbEclipseType.SelectedIndexChanged += new System.EventHandler(this.cmbEclipseType_SelectedIndexChanged);
            // 
            // chkMoonfireAlways
            // 
            this.chkMoonfireAlways.AutoSize = true;
            this.chkMoonfireAlways.Location = new System.Drawing.Point(6, 403);
            this.chkMoonfireAlways.Name = "chkMoonfireAlways";
            this.chkMoonfireAlways.Size = new System.Drawing.Size(138, 17);
            this.chkMoonfireAlways.TabIndex = 20;
            this.chkMoonfireAlways.Text = "100% Moonfire Uptime?";
            this.chkMoonfireAlways.UseVisualStyleBackColor = true;
            this.chkMoonfireAlways.CheckedChanged += new System.EventHandler(this.chkMoonfireAlways_CheckedChanged);
            // 
            // CalculationOptionsPanelMoonkin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.chkMoonfireAlways);
            this.Controls.Add(this.cmbEclipseType);
            this.Controls.Add(this.lblEclipseType);
            this.Controls.Add(this.cmbUserRotation);
            this.Controls.Add(this.lblUserRotation);
            this.Controls.Add(this.lblLifespanValue);
            this.Controls.Add(this.lblUptimeValue);
            this.Controls.Add(this.lblTreantLifespan);
            this.Controls.Add(this.lblReplenishmentUptime);
            this.Controls.Add(this.trkTreantLifespan);
            this.Controls.Add(this.trkReplenishmentUptime);
            this.Controls.Add(this.rdbScryer);
            this.Controls.Add(this.rdbAldor);
            this.Controls.Add(this.lblInnervateWeaponSpi);
            this.Controls.Add(this.lblInnervateWeaponInt);
            this.Controls.Add(this.txtInnervateWeaponSpi);
            this.Controls.Add(this.txtInnervateWeaponInt);
            this.Controls.Add(this.chkInnervateWeapon);
            this.Controls.Add(this.lblManaPotType);
            this.Controls.Add(this.txtInnervateDelay);
            this.Controls.Add(this.lblInnervateOffset);
            this.Controls.Add(this.cmbPotType);
            this.Controls.Add(this.chkManaPots);
            this.Controls.Add(this.chkInnervate);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtFightLength);
            this.Controls.Add(this.lblLatency);
            this.Controls.Add(this.txtLatency);
            this.Controls.Add(this.cmbTargetLevel);
            this.Controls.Add(this.lblTargetLevel);
            this.Name = "CalculationOptionsPanelMoonkin";
            this.Size = new System.Drawing.Size(204, 455);
            ((System.ComponentModel.ISupportInitialize)(this.trkReplenishmentUptime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkTreantLifespan)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTargetLevel;
        private System.Windows.Forms.ComboBox cmbTargetLevel;
        private System.Windows.Forms.TextBox txtLatency;
        private System.Windows.Forms.Label lblLatency;
        private System.Windows.Forms.TextBox txtFightLength;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkInnervate;
        private System.Windows.Forms.CheckBox chkManaPots;
        private System.Windows.Forms.ComboBox cmbPotType;
        private System.Windows.Forms.Label lblInnervateOffset;
        private System.Windows.Forms.TextBox txtInnervateDelay;
        private System.Windows.Forms.Label lblManaPotType;
        private System.Windows.Forms.CheckBox chkInnervateWeapon;
        private System.Windows.Forms.TextBox txtInnervateWeaponInt;
        private System.Windows.Forms.TextBox txtInnervateWeaponSpi;
        private System.Windows.Forms.Label lblInnervateWeaponInt;
        private System.Windows.Forms.Label lblInnervateWeaponSpi;
        private System.Windows.Forms.RadioButton rdbAldor;
        private System.Windows.Forms.RadioButton rdbScryer;
        private System.Windows.Forms.TrackBar trkReplenishmentUptime;
        private System.Windows.Forms.TrackBar trkTreantLifespan;
        private System.Windows.Forms.Label lblReplenishmentUptime;
        private System.Windows.Forms.Label lblTreantLifespan;
        private System.Windows.Forms.Label lblUptimeValue;
        private System.Windows.Forms.Label lblLifespanValue;
        private System.Windows.Forms.Label lblUserRotation;
        private System.Windows.Forms.ComboBox cmbUserRotation;
        private System.Windows.Forms.Label lblEclipseType;
        private System.Windows.Forms.ComboBox cmbEclipseType;
        private System.Windows.Forms.CheckBox chkMoonfireAlways;

    }
}

