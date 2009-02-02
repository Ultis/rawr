namespace Rawr.Elemental
{
    partial class CalculationOptionsPanelElemental
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
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lblBSRatio = new System.Windows.Forms.Label();
            this.tbBSRatio = new System.Windows.Forms.TrackBar();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.tkReplenishment = new System.Windows.Forms.TrackBar();
            this.lblReplenishment = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.cmbManaAmt = new System.Windows.Forms.ComboBox();
            this.trkFightLength = new System.Windows.Forms.TrackBar();
            this.lblFightLength = new System.Windows.Forms.Label();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.groupBox10 = new System.Windows.Forms.GroupBox();
            this.extendedToolTipLabel11 = new Rawr.CustomControls.ExtendedToolTipLabel();
            this.chbGlyphShocking = new System.Windows.Forms.CheckBox();
            this.extendedToolTipLabel10 = new Rawr.CustomControls.ExtendedToolTipLabel();
            this.chbGlyphLightningBolt = new Rawr.CustomControls.ExtendedToolTipCheckBox();
            this.chbGlyphFT = new System.Windows.Forms.CheckBox();
            this.chbGlyphEM = new System.Windows.Forms.CheckBox();
            this.chbGlyphLava = new System.Windows.Forms.CheckBox();
            this.chbGlyphFS = new System.Windows.Forms.CheckBox();
            this.extendedToolTipLabel8 = new Rawr.CustomControls.ExtendedToolTipLabel();
            this.extendedToolTipLabel9 = new Rawr.CustomControls.ExtendedToolTipLabel();
            this.extendedToolTipLabel7 = new Rawr.CustomControls.ExtendedToolTipLabel();
            this.extendedToolTipLabel6 = new Rawr.CustomControls.ExtendedToolTipLabel();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.tbModuleNotes = new System.Windows.Forms.TextBox();
            this.cbThunderstorm = new System.Windows.Forms.CheckBox();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbBSRatio)).BeginInit();
            this.tabPage2.SuspendLayout();
            this.groupBox7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tkReplenishment)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkFightLength)).BeginInit();
            this.tabPage3.SuspendLayout();
            this.groupBox10.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Location = new System.Drawing.Point(3, 3);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(297, 576);
            this.tabControl1.TabIndex = 23;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(289, 550);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Stats";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lblBSRatio);
            this.groupBox2.Controls.Add(this.tbBSRatio);
            this.groupBox2.Location = new System.Drawing.Point(6, 6);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(277, 87);
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
            this.tabPage2.Size = new System.Drawing.Size(289, 550);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Fight";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.cbThunderstorm);
            this.groupBox7.Controls.Add(this.tkReplenishment);
            this.groupBox7.Controls.Add(this.lblReplenishment);
            this.groupBox7.Controls.Add(this.label10);
            this.groupBox7.Controls.Add(this.cmbManaAmt);
            this.groupBox7.Controls.Add(this.trkFightLength);
            this.groupBox7.Controls.Add(this.lblFightLength);
            this.groupBox7.Location = new System.Drawing.Point(6, 6);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(277, 212);
            this.groupBox7.TabIndex = 33;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Fight Details";
            // 
            // tkReplenishment
            // 
            this.tkReplenishment.BackColor = System.Drawing.SystemColors.Control;
            this.tkReplenishment.Location = new System.Drawing.Point(9, 125);
            this.tkReplenishment.Maximum = 100;
            this.tkReplenishment.Name = "tkReplenishment";
            this.tkReplenishment.Size = new System.Drawing.Size(262, 45);
            this.tkReplenishment.TabIndex = 38;
            this.tkReplenishment.TickFrequency = 5;
            this.tkReplenishment.Scroll += new System.EventHandler(this.tkReplenishment_Scroll);
            // 
            // lblReplenishment
            // 
            this.lblReplenishment.AutoSize = true;
            this.lblReplenishment.Location = new System.Drawing.Point(6, 109);
            this.lblReplenishment.Name = "lblReplenishment";
            this.lblReplenishment.Size = new System.Drawing.Size(80, 13);
            this.lblReplenishment.TabIndex = 50;
            this.lblReplenishment.Text = "Replenishment:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(6, 88);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(74, 13);
            this.label10.TabIndex = 47;
            this.label10.Text = "Mana potions:";
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
            this.cmbManaAmt.Location = new System.Drawing.Point(86, 85);
            this.cmbManaAmt.Name = "cmbManaAmt";
            this.cmbManaAmt.Size = new System.Drawing.Size(185, 21);
            this.cmbManaAmt.TabIndex = 46;
            this.cmbManaAmt.ValueMember = "2400";
            this.cmbManaAmt.SelectedIndexChanged += new System.EventHandler(this.cmbManaAmt_SelectedIndexChanged);
            // 
            // trkFightLength
            // 
            this.trkFightLength.Location = new System.Drawing.Point(9, 34);
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
            this.lblFightLength.Location = new System.Drawing.Point(6, 18);
            this.lblFightLength.Name = "lblFightLength";
            this.lblFightLength.Size = new System.Drawing.Size(69, 13);
            this.lblFightLength.TabIndex = 38;
            this.lblFightLength.Text = "Fight Length:";
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.groupBox10);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(289, 550);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Glyphs";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // groupBox10
            // 
            this.groupBox10.Controls.Add(this.extendedToolTipLabel11);
            this.groupBox10.Controls.Add(this.chbGlyphShocking);
            this.groupBox10.Controls.Add(this.extendedToolTipLabel10);
            this.groupBox10.Controls.Add(this.chbGlyphLightningBolt);
            this.groupBox10.Controls.Add(this.chbGlyphFT);
            this.groupBox10.Controls.Add(this.chbGlyphEM);
            this.groupBox10.Controls.Add(this.chbGlyphLava);
            this.groupBox10.Controls.Add(this.chbGlyphFS);
            this.groupBox10.Controls.Add(this.extendedToolTipLabel8);
            this.groupBox10.Controls.Add(this.extendedToolTipLabel9);
            this.groupBox10.Controls.Add(this.extendedToolTipLabel7);
            this.groupBox10.Controls.Add(this.extendedToolTipLabel6);
            this.groupBox10.Location = new System.Drawing.Point(6, 6);
            this.groupBox10.Name = "groupBox10";
            this.groupBox10.Size = new System.Drawing.Size(277, 135);
            this.groupBox10.TabIndex = 0;
            this.groupBox10.TabStop = false;
            this.groupBox10.Text = "Glyphes";
            // 
            // extendedToolTipLabel11
            // 
            this.extendedToolTipLabel11.AutoSize = true;
            this.extendedToolTipLabel11.Location = new System.Drawing.Point(6, 116);
            this.extendedToolTipLabel11.Name = "extendedToolTipLabel11";
            this.extendedToolTipLabel11.Size = new System.Drawing.Size(94, 13);
            this.extendedToolTipLabel11.TabIndex = 5;
            this.extendedToolTipLabel11.Text = "Glyph of Shocking";
            this.extendedToolTipLabel11.ToolTipText = "-0.5 gcd for Shock spells";
            // 
            // chbGlyphShocking
            // 
            this.chbGlyphShocking.AutoSize = true;
            this.chbGlyphShocking.Location = new System.Drawing.Point(256, 115);
            this.chbGlyphShocking.Name = "chbGlyphShocking";
            this.chbGlyphShocking.Size = new System.Drawing.Size(15, 14);
            this.chbGlyphShocking.TabIndex = 8;
            this.chbGlyphShocking.Tag = "Glyph of Innervate";
            this.chbGlyphShocking.UseVisualStyleBackColor = true;
            this.chbGlyphShocking.CheckedChanged += new System.EventHandler(this.chbSomeGlyph_CheckedChanged);
            // 
            // extendedToolTipLabel10
            // 
            this.extendedToolTipLabel10.AutoSize = true;
            this.extendedToolTipLabel10.Location = new System.Drawing.Point(6, 96);
            this.extendedToolTipLabel10.Name = "extendedToolTipLabel10";
            this.extendedToolTipLabel10.Size = new System.Drawing.Size(113, 13);
            this.extendedToolTipLabel10.TabIndex = 4;
            this.extendedToolTipLabel10.Text = "Glyph of Lightning Bolt";
            this.extendedToolTipLabel10.ToolTipText = "Lightning Bolt +4% damage";
            // 
            // chbGlyphLightningBolt
            // 
            this.chbGlyphLightningBolt.AutoSize = true;
            this.chbGlyphLightningBolt.Location = new System.Drawing.Point(256, 95);
            this.chbGlyphLightningBolt.Name = "chbGlyphLightningBolt";
            this.chbGlyphLightningBolt.Size = new System.Drawing.Size(15, 14);
            this.chbGlyphLightningBolt.TabIndex = 2;
            this.chbGlyphLightningBolt.ToolTipText = "";
            this.chbGlyphLightningBolt.UseVisualStyleBackColor = true;
            this.chbGlyphLightningBolt.CheckedChanged += new System.EventHandler(this.chbSomeGlyph_CheckedChanged);
            // 
            // chbGlyphFT
            // 
            this.chbGlyphFT.AutoSize = true;
            this.chbGlyphFT.Location = new System.Drawing.Point(256, 55);
            this.chbGlyphFT.Name = "chbGlyphFT";
            this.chbGlyphFT.Size = new System.Drawing.Size(15, 14);
            this.chbGlyphFT.TabIndex = 5;
            this.chbGlyphFT.Tag = "Glyph of Flametongue";
            this.chbGlyphFT.UseVisualStyleBackColor = true;
            this.chbGlyphFT.CheckedChanged += new System.EventHandler(this.chbSomeGlyph_CheckedChanged);
            // 
            // chbGlyphEM
            // 
            this.chbGlyphEM.AutoSize = true;
            this.chbGlyphEM.Location = new System.Drawing.Point(256, 35);
            this.chbGlyphEM.Name = "chbGlyphEM";
            this.chbGlyphEM.Size = new System.Drawing.Size(15, 14);
            this.chbGlyphEM.TabIndex = 4;
            this.chbGlyphEM.Tag = "Glyph of Elemental Mastery";
            this.chbGlyphEM.UseVisualStyleBackColor = true;
            this.chbGlyphEM.CheckedChanged += new System.EventHandler(this.chbSomeGlyph_CheckedChanged);
            // 
            // chbGlyphLava
            // 
            this.chbGlyphLava.AutoSize = true;
            this.chbGlyphLava.Location = new System.Drawing.Point(256, 75);
            this.chbGlyphLava.Name = "chbGlyphLava";
            this.chbGlyphLava.Size = new System.Drawing.Size(15, 14);
            this.chbGlyphLava.TabIndex = 6;
            this.chbGlyphLava.Tag = "Glyph of Lifebloom";
            this.chbGlyphLava.UseVisualStyleBackColor = true;
            this.chbGlyphLava.CheckedChanged += new System.EventHandler(this.chbSomeGlyph_CheckedChanged);
            // 
            // chbGlyphFS
            // 
            this.chbGlyphFS.AutoSize = true;
            this.chbGlyphFS.Location = new System.Drawing.Point(256, 15);
            this.chbGlyphFS.Name = "chbGlyphFS";
            this.chbGlyphFS.Size = new System.Drawing.Size(15, 14);
            this.chbGlyphFS.TabIndex = 3;
            this.chbGlyphFS.Tag = "Glyph of Flame Shock";
            this.chbGlyphFS.UseVisualStyleBackColor = true;
            this.chbGlyphFS.CheckedChanged += new System.EventHandler(this.chbSomeGlyph_CheckedChanged);
            // 
            // extendedToolTipLabel8
            // 
            this.extendedToolTipLabel8.AutoSize = true;
            this.extendedToolTipLabel8.Location = new System.Drawing.Point(6, 55);
            this.extendedToolTipLabel8.Name = "extendedToolTipLabel8";
            this.extendedToolTipLabel8.Size = new System.Drawing.Size(110, 13);
            this.extendedToolTipLabel8.TabIndex = 2;
            this.extendedToolTipLabel8.Text = "Glyph of Flametongue";
            this.extendedToolTipLabel8.ToolTipText = "+2% crit with Flametongue active";
            // 
            // extendedToolTipLabel9
            // 
            this.extendedToolTipLabel9.AutoSize = true;
            this.extendedToolTipLabel9.Location = new System.Drawing.Point(6, 76);
            this.extendedToolTipLabel9.Name = "extendedToolTipLabel9";
            this.extendedToolTipLabel9.Size = new System.Drawing.Size(73, 13);
            this.extendedToolTipLabel9.TabIndex = 3;
            this.extendedToolTipLabel9.Text = "Glyph of Lava";
            this.extendedToolTipLabel9.ToolTipText = "+10% SP to Lava Burst";
            // 
            // extendedToolTipLabel7
            // 
            this.extendedToolTipLabel7.AutoSize = true;
            this.extendedToolTipLabel7.Location = new System.Drawing.Point(6, 35);
            this.extendedToolTipLabel7.Name = "extendedToolTipLabel7";
            this.extendedToolTipLabel7.Size = new System.Drawing.Size(135, 13);
            this.extendedToolTipLabel7.TabIndex = 1;
            this.extendedToolTipLabel7.Text = "Glyph of Elemental Mastery";
            this.extendedToolTipLabel7.ToolTipText = "-30sec on EM cd";
            // 
            // extendedToolTipLabel6
            // 
            this.extendedToolTipLabel6.AutoSize = true;
            this.extendedToolTipLabel6.Location = new System.Drawing.Point(6, 16);
            this.extendedToolTipLabel6.Name = "extendedToolTipLabel6";
            this.extendedToolTipLabel6.Size = new System.Drawing.Size(111, 13);
            this.extendedToolTipLabel6.TabIndex = 0;
            this.extendedToolTipLabel6.Text = "Glyph of Flame Shock";
            this.extendedToolTipLabel6.ToolTipText = "+6 sec FS and not consumed";
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.tbModuleNotes);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(289, 550);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Module Notes";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // tbModuleNotes
            // 
            this.tbModuleNotes.AcceptsReturn = true;
            this.tbModuleNotes.AcceptsTab = true;
            this.tbModuleNotes.Location = new System.Drawing.Point(7, 7);
            this.tbModuleNotes.Multiline = true;
            this.tbModuleNotes.Name = "tbModuleNotes";
            this.tbModuleNotes.ReadOnly = true;
            this.tbModuleNotes.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbModuleNotes.Size = new System.Drawing.Size(276, 537);
            this.tbModuleNotes.TabIndex = 0;
            // 
            // cbThunderstorm
            // 
            this.cbThunderstorm.AutoSize = true;
            this.cbThunderstorm.Location = new System.Drawing.Point(9, 177);
            this.cbThunderstorm.Name = "cbThunderstorm";
            this.cbThunderstorm.Size = new System.Drawing.Size(208, 17);
            this.cbThunderstorm.TabIndex = 51;
            this.cbThunderstorm.Text = "Use Thunderstorm whenever available";
            this.cbThunderstorm.UseVisualStyleBackColor = true;
            this.cbThunderstorm.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // CalculationOptionsPanelElemental
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.tabControl1);
            this.Name = "CalculationOptionsPanelElemental";
            this.Size = new System.Drawing.Size(303, 582);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbBSRatio)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tkReplenishment)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkFightLength)).EndInit();
            this.tabPage3.ResumeLayout(false);
            this.groupBox10.ResumeLayout(false);
            this.groupBox10.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.GroupBox groupBox10;
        private Rawr.CustomControls.ExtendedToolTipLabel extendedToolTipLabel9;
        private Rawr.CustomControls.ExtendedToolTipLabel extendedToolTipLabel8;
        private Rawr.CustomControls.ExtendedToolTipLabel extendedToolTipLabel7;
        private Rawr.CustomControls.ExtendedToolTipLabel extendedToolTipLabel6;
        private System.Windows.Forms.CheckBox chbGlyphShocking;
        private System.Windows.Forms.CheckBox chbGlyphLava;
        private Rawr.CustomControls.ExtendedToolTipLabel extendedToolTipLabel11;
        private Rawr.CustomControls.ExtendedToolTipLabel extendedToolTipLabel10;
        private System.Windows.Forms.CheckBox chbGlyphFT;
        private System.Windows.Forms.CheckBox chbGlyphEM;
        private System.Windows.Forms.CheckBox chbGlyphFS;
        private Rawr.CustomControls.ExtendedToolTipCheckBox chbGlyphLightningBolt;
        private System.Windows.Forms.Label lblFightLength;
        private System.Windows.Forms.TrackBar trkFightLength;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox cmbManaAmt;
        private System.Windows.Forms.TrackBar tkReplenishment;
        private System.Windows.Forms.Label lblReplenishment;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.TextBox tbModuleNotes;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label lblBSRatio;
        private System.Windows.Forms.TrackBar tbBSRatio;
        private System.Windows.Forms.CheckBox cbThunderstorm;
    }
}