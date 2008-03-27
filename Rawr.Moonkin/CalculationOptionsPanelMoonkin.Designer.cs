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
            this.chkMetagem = new System.Windows.Forms.CheckBox();
            this.btnTalents = new System.Windows.Forms.Button();
            this.txtFightLength = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.chkInnervate = new System.Windows.Forms.CheckBox();
            this.txtShadowPriest = new System.Windows.Forms.TextBox();
            this.lblShadowPriest = new System.Windows.Forms.Label();
            this.chkManaPots = new System.Windows.Forms.CheckBox();
            this.cmbPotType = new System.Windows.Forms.ComboBox();
            this.lblInnervateOffset = new System.Windows.Forms.Label();
            this.txtInnervateDelay = new System.Windows.Forms.TextBox();
            this.txtManaPotDelay = new System.Windows.Forms.TextBox();
            this.lblManPotDelay = new System.Windows.Forms.Label();
            this.lblManaPotType = new System.Windows.Forms.Label();
            this.chkInnervateWeapon = new System.Windows.Forms.CheckBox();
            this.txtInnervateWeaponInt = new System.Windows.Forms.TextBox();
            this.txtInnervateWeaponSpi = new System.Windows.Forms.TextBox();
            this.lblInnervateWeaponInt = new System.Windows.Forms.Label();
            this.lblInnervateWeaponSpi = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblTargetLevel
            // 
            this.lblTargetLevel.AutoSize = true;
            this.lblTargetLevel.Location = new System.Drawing.Point(3, 7);
            this.lblTargetLevel.Name = "lblTargetLevel";
            this.lblTargetLevel.Size = new System.Drawing.Size(70, 13);
            this.lblTargetLevel.TabIndex = 0;
            this.lblTargetLevel.Text = "Target Level:";
            // 
            // cmbTargetLevel
            // 
            this.cmbTargetLevel.FormattingEnabled = true;
            this.cmbTargetLevel.Items.AddRange(new object[] {
            "70",
            "71",
            "72",
            "73"});
            this.cmbTargetLevel.Location = new System.Drawing.Point(108, 4);
            this.cmbTargetLevel.Name = "cmbTargetLevel";
            this.cmbTargetLevel.Size = new System.Drawing.Size(93, 21);
            this.cmbTargetLevel.TabIndex = 1;
            this.cmbTargetLevel.SelectedIndexChanged += new System.EventHandler(this.cmbTargetLevel_SelectedIndexChanged);
            // 
            // txtLatency
            // 
            this.txtLatency.Location = new System.Drawing.Point(108, 31);
            this.txtLatency.Name = "txtLatency";
            this.txtLatency.Size = new System.Drawing.Size(93, 20);
            this.txtLatency.TabIndex = 2;
            this.txtLatency.Leave += new System.EventHandler(this.txtLatency_TextChanged);
            // 
            // lblLatency
            // 
            this.lblLatency.AutoSize = true;
            this.lblLatency.Location = new System.Drawing.Point(3, 34);
            this.lblLatency.Name = "lblLatency";
            this.lblLatency.Size = new System.Drawing.Size(48, 13);
            this.lblLatency.TabIndex = 3;
            this.lblLatency.Text = "Latency:";
            // 
            // chkMetagem
            // 
            this.chkMetagem.AutoSize = true;
            this.chkMetagem.Location = new System.Drawing.Point(6, 345);
            this.chkMetagem.Name = "chkMetagem";
            this.chkMetagem.Size = new System.Drawing.Size(178, 17);
            this.chkMetagem.TabIndex = 9;
            this.chkMetagem.Text = "Enforce Metagem Requirements";
            this.chkMetagem.UseVisualStyleBackColor = true;
            this.chkMetagem.CheckedChanged += new System.EventHandler(this.chkMetagem_Leave);
            // 
            // btnTalents
            // 
            this.btnTalents.Location = new System.Drawing.Point(3, 316);
            this.btnTalents.Name = "btnTalents";
            this.btnTalents.Size = new System.Drawing.Size(195, 23);
            this.btnTalents.TabIndex = 8;
            this.btnTalents.Text = "Talents";
            this.btnTalents.UseVisualStyleBackColor = true;
            this.btnTalents.Click += new System.EventHandler(this.btnTalents_Click);
            // 
            // txtFightLength
            // 
            this.txtFightLength.Location = new System.Drawing.Point(108, 58);
            this.txtFightLength.Name = "txtFightLength";
            this.txtFightLength.Size = new System.Drawing.Size(93, 20);
            this.txtFightLength.TabIndex = 10;
            this.txtFightLength.Leave += new System.EventHandler(this.txtFightLength_Leave);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 61);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(105, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Est. Fight Time (min):";
            // 
            // chkInnervate
            // 
            this.chkInnervate.AutoSize = true;
            this.chkInnervate.Location = new System.Drawing.Point(6, 111);
            this.chkInnervate.Name = "chkInnervate";
            this.chkInnervate.Size = new System.Drawing.Size(135, 17);
            this.chkInnervate.TabIndex = 12;
            this.chkInnervate.Text = "Cast Innervate on self?";
            this.chkInnervate.UseVisualStyleBackColor = true;
            this.chkInnervate.CheckedChanged += new System.EventHandler(this.chkInnervate_CheckedChanged);
            // 
            // txtShadowPriest
            // 
            this.txtShadowPriest.Location = new System.Drawing.Point(108, 85);
            this.txtShadowPriest.Name = "txtShadowPriest";
            this.txtShadowPriest.Size = new System.Drawing.Size(93, 20);
            this.txtShadowPriest.TabIndex = 13;
            this.txtShadowPriest.Leave += new System.EventHandler(this.txtShadowPriest_Leave);
            // 
            // lblShadowPriest
            // 
            this.lblShadowPriest.AutoSize = true;
            this.lblShadowPriest.Location = new System.Drawing.Point(3, 88);
            this.lblShadowPriest.Name = "lblShadowPriest";
            this.lblShadowPriest.Size = new System.Drawing.Size(103, 13);
            this.lblShadowPriest.TabIndex = 14;
            this.lblShadowPriest.Text = "Shadow Priest MP5:";
            // 
            // chkManaPots
            // 
            this.chkManaPots.AutoSize = true;
            this.chkManaPots.Location = new System.Drawing.Point(6, 160);
            this.chkManaPots.Name = "chkManaPots";
            this.chkManaPots.Size = new System.Drawing.Size(117, 17);
            this.chkManaPots.TabIndex = 15;
            this.chkManaPots.Text = "Use mana potions?";
            this.chkManaPots.UseVisualStyleBackColor = true;
            this.chkManaPots.CheckedChanged += new System.EventHandler(this.chkManaPots_CheckedChanged);
            // 
            // cmbPotType
            // 
            this.cmbPotType.FormattingEnabled = true;
            this.cmbPotType.Items.AddRange(new object[] {
            "Super Mana Potion",
            "Fel Mana Potion"});
            this.cmbPotType.Location = new System.Drawing.Point(108, 210);
            this.cmbPotType.Name = "cmbPotType";
            this.cmbPotType.Size = new System.Drawing.Size(93, 21);
            this.cmbPotType.TabIndex = 16;
            this.cmbPotType.SelectedIndexChanged += new System.EventHandler(this.cmbPotType_SelectedIndexChanged);
            // 
            // lblInnervateOffset
            // 
            this.lblInnervateOffset.AutoSize = true;
            this.lblInnervateOffset.Location = new System.Drawing.Point(3, 137);
            this.lblInnervateOffset.Name = "lblInnervateOffset";
            this.lblInnervateOffset.Size = new System.Drawing.Size(85, 13);
            this.lblInnervateOffset.TabIndex = 17;
            this.lblInnervateOffset.Text = "Innervate Delay:";
            // 
            // txtInnervateDelay
            // 
            this.txtInnervateDelay.Location = new System.Drawing.Point(108, 134);
            this.txtInnervateDelay.Name = "txtInnervateDelay";
            this.txtInnervateDelay.Size = new System.Drawing.Size(93, 20);
            this.txtInnervateDelay.TabIndex = 18;
            this.txtInnervateDelay.Leave += new System.EventHandler(this.txtInnervateDelay_Leave);
            // 
            // txtManaPotDelay
            // 
            this.txtManaPotDelay.Location = new System.Drawing.Point(108, 184);
            this.txtManaPotDelay.Name = "txtManaPotDelay";
            this.txtManaPotDelay.Size = new System.Drawing.Size(93, 20);
            this.txtManaPotDelay.TabIndex = 19;
            this.txtManaPotDelay.Leave += new System.EventHandler(this.txtManaPotDelay_Leave);
            // 
            // lblManPotDelay
            // 
            this.lblManPotDelay.AutoSize = true;
            this.lblManPotDelay.Location = new System.Drawing.Point(3, 187);
            this.lblManPotDelay.Name = "lblManPotDelay";
            this.lblManPotDelay.Size = new System.Drawing.Size(100, 13);
            this.lblManPotDelay.TabIndex = 20;
            this.lblManPotDelay.Text = "Mana Potion Delay:";
            // 
            // lblManaPotType
            // 
            this.lblManaPotType.AutoSize = true;
            this.lblManaPotType.Location = new System.Drawing.Point(3, 213);
            this.lblManaPotType.Name = "lblManaPotType";
            this.lblManaPotType.Size = new System.Drawing.Size(97, 13);
            this.lblManaPotType.TabIndex = 21;
            this.lblManaPotType.Text = "Mana Potion Type:";
            // 
            // chkInnervateWeapon
            // 
            this.chkInnervateWeapon.AutoSize = true;
            this.chkInnervateWeapon.Location = new System.Drawing.Point(6, 239);
            this.chkInnervateWeapon.Name = "chkInnervateWeapon";
            this.chkInnervateWeapon.Size = new System.Drawing.Size(140, 17);
            this.chkInnervateWeapon.TabIndex = 22;
            this.chkInnervateWeapon.Text = "Use Innervate weapon?";
            this.chkInnervateWeapon.UseVisualStyleBackColor = true;
            this.chkInnervateWeapon.CheckedChanged += new System.EventHandler(this.chkInnervateWeapon_CheckedChanged);
            // 
            // txtInnervateWeaponInt
            // 
            this.txtInnervateWeaponInt.Location = new System.Drawing.Point(108, 263);
            this.txtInnervateWeaponInt.Name = "txtInnervateWeaponInt";
            this.txtInnervateWeaponInt.Size = new System.Drawing.Size(93, 20);
            this.txtInnervateWeaponInt.TabIndex = 23;
            this.txtInnervateWeaponInt.Leave += new System.EventHandler(this.txtInnervateWeaponInt_Leave);
            // 
            // txtInnervateWeaponSpi
            // 
            this.txtInnervateWeaponSpi.Location = new System.Drawing.Point(108, 290);
            this.txtInnervateWeaponSpi.Name = "txtInnervateWeaponSpi";
            this.txtInnervateWeaponSpi.Size = new System.Drawing.Size(93, 20);
            this.txtInnervateWeaponSpi.TabIndex = 24;
            this.txtInnervateWeaponSpi.Leave += new System.EventHandler(this.txtInnervateWeaponSpi_Leave);
            // 
            // lblInnervateWeaponInt
            // 
            this.lblInnervateWeaponInt.AutoSize = true;
            this.lblInnervateWeaponInt.Location = new System.Drawing.Point(3, 266);
            this.lblInnervateWeaponInt.Name = "lblInnervateWeaponInt";
            this.lblInnervateWeaponInt.Size = new System.Drawing.Size(76, 13);
            this.lblInnervateWeaponInt.TabIndex = 25;
            this.lblInnervateWeaponInt.Text = "Intellect value:";
            // 
            // lblInnervateWeaponSpi
            // 
            this.lblInnervateWeaponSpi.AutoSize = true;
            this.lblInnervateWeaponSpi.Location = new System.Drawing.Point(3, 293);
            this.lblInnervateWeaponSpi.Name = "lblInnervateWeaponSpi";
            this.lblInnervateWeaponSpi.Size = new System.Drawing.Size(62, 13);
            this.lblInnervateWeaponSpi.TabIndex = 26;
            this.lblInnervateWeaponSpi.Text = "Spirit value:";
            // 
            // CalculationOptionsPanelMoonkin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblInnervateWeaponSpi);
            this.Controls.Add(this.lblInnervateWeaponInt);
            this.Controls.Add(this.txtInnervateWeaponSpi);
            this.Controls.Add(this.txtInnervateWeaponInt);
            this.Controls.Add(this.chkInnervateWeapon);
            this.Controls.Add(this.lblManaPotType);
            this.Controls.Add(this.lblManPotDelay);
            this.Controls.Add(this.txtManaPotDelay);
            this.Controls.Add(this.txtInnervateDelay);
            this.Controls.Add(this.lblInnervateOffset);
            this.Controls.Add(this.cmbPotType);
            this.Controls.Add(this.chkManaPots);
            this.Controls.Add(this.lblShadowPriest);
            this.Controls.Add(this.txtShadowPriest);
            this.Controls.Add(this.chkInnervate);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtFightLength);
            this.Controls.Add(this.chkMetagem);
            this.Controls.Add(this.btnTalents);
            this.Controls.Add(this.lblLatency);
            this.Controls.Add(this.txtLatency);
            this.Controls.Add(this.cmbTargetLevel);
            this.Controls.Add(this.lblTargetLevel);
            this.Name = "CalculationOptionsPanelMoonkin";
            this.Size = new System.Drawing.Size(204, 370);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTargetLevel;
        private System.Windows.Forms.ComboBox cmbTargetLevel;
        private System.Windows.Forms.TextBox txtLatency;
        private System.Windows.Forms.Label lblLatency;
        private System.Windows.Forms.CheckBox chkMetagem;
        private System.Windows.Forms.Button btnTalents;
        private System.Windows.Forms.TextBox txtFightLength;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkInnervate;
        private System.Windows.Forms.TextBox txtShadowPriest;
        private System.Windows.Forms.Label lblShadowPriest;
        private System.Windows.Forms.CheckBox chkManaPots;
        private System.Windows.Forms.ComboBox cmbPotType;
        private System.Windows.Forms.Label lblInnervateOffset;
        private System.Windows.Forms.TextBox txtInnervateDelay;
        private System.Windows.Forms.TextBox txtManaPotDelay;
        private System.Windows.Forms.Label lblManPotDelay;
        private System.Windows.Forms.Label lblManaPotType;
        private System.Windows.Forms.CheckBox chkInnervateWeapon;
        private System.Windows.Forms.TextBox txtInnervateWeaponInt;
        private System.Windows.Forms.TextBox txtInnervateWeaponSpi;
        private System.Windows.Forms.Label lblInnervateWeaponInt;
        private System.Windows.Forms.Label lblInnervateWeaponSpi;

    }
}

