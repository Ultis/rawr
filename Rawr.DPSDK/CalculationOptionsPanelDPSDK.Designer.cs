namespace Rawr.DPSDK
{
    partial class CalculationOptionsPanelDPSDK
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
            this.cbTargetLevel = new System.Windows.Forms.ComboBox();
            this.tbFightLength = new System.Windows.Forms.TrackBar();
            this.gbFightInfo = new System.Windows.Forms.GroupBox();
            this.nudTargetArmor = new System.Windows.Forms.NumericUpDown();
            this.lblFightLengthNum = new System.Windows.Forms.Label();
            this.lblTargetArmor = new System.Windows.Forms.Label();
            this.lblFightLength = new System.Windows.Forms.Label();
            this.btnGraph = new System.Windows.Forms.Button();
            this.checkBoxMeta = new System.Windows.Forms.CheckBox();
            this.gbMajorGlyph = new System.Windows.Forms.GroupBox();
            this.cbGlyphGhoul = new System.Windows.Forms.CheckBox();
            this.cbGlyphPlagueStrike = new System.Windows.Forms.CheckBox();
            this.cbGlyphOblit = new System.Windows.Forms.CheckBox();
            this.gbMinorGlyph = new System.Windows.Forms.GroupBox();
            this.btnRotation = new System.Windows.Forms.Button();
            this.cbWindfuryEffect = new System.Windows.Forms.CheckBox();
            this.cbUREffect = new System.Windows.Forms.CheckBox();
            this.cbMagicVuln = new System.Windows.Forms.CheckBox();
            this.cbCryptFever = new System.Windows.Forms.CheckBox();
            this.cbGhoul = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.tbFightLength)).BeginInit();
            this.gbFightInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudTargetArmor)).BeginInit();
            this.gbMajorGlyph.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTargetLevel
            // 
            this.lblTargetLevel.AutoSize = true;
            this.lblTargetLevel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTargetLevel.Location = new System.Drawing.Point(6, 17);
            this.lblTargetLevel.Name = "lblTargetLevel";
            this.lblTargetLevel.Size = new System.Drawing.Size(77, 15);
            this.lblTargetLevel.TabIndex = 0;
            this.lblTargetLevel.Text = "Target Level:";
            // 
            // cbTargetLevel
            // 
            this.cbTargetLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbTargetLevel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbTargetLevel.FormattingEnabled = true;
            this.cbTargetLevel.Items.AddRange(new object[] {
            "80",
            "81",
            "82",
            "83"});
            this.cbTargetLevel.Location = new System.Drawing.Point(87, 14);
            this.cbTargetLevel.Name = "cbTargetLevel";
            this.cbTargetLevel.Size = new System.Drawing.Size(49, 23);
            this.cbTargetLevel.TabIndex = 1;
            this.cbTargetLevel.SelectedIndexChanged += new System.EventHandler(this.cbTargetLevel_SelectedIndexChanged);
            // 
            // tbFightLength
            // 
            this.tbFightLength.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.tbFightLength.Location = new System.Drawing.Point(87, 70);
            this.tbFightLength.Name = "tbFightLength";
            this.tbFightLength.Size = new System.Drawing.Size(86, 45);
            this.tbFightLength.TabIndex = 2;
            this.tbFightLength.Value = 10;
            this.tbFightLength.Scroll += new System.EventHandler(this.tbFightLength_Scroll);
            // 
            // gbFightInfo
            // 
            this.gbFightInfo.Controls.Add(this.nudTargetArmor);
            this.gbFightInfo.Controls.Add(this.lblFightLengthNum);
            this.gbFightInfo.Controls.Add(this.lblTargetArmor);
            this.gbFightInfo.Controls.Add(this.tbFightLength);
            this.gbFightInfo.Controls.Add(this.lblTargetLevel);
            this.gbFightInfo.Controls.Add(this.cbTargetLevel);
            this.gbFightInfo.Controls.Add(this.lblFightLength);
            this.gbFightInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbFightInfo.Location = new System.Drawing.Point(13, 3);
            this.gbFightInfo.Name = "gbFightInfo";
            this.gbFightInfo.Size = new System.Drawing.Size(185, 127);
            this.gbFightInfo.TabIndex = 4;
            this.gbFightInfo.TabStop = false;
            this.gbFightInfo.Text = "Fight Info";
            // 
            // nudTargetArmor
            // 
            this.nudTargetArmor.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.nudTargetArmor.Location = new System.Drawing.Point(87, 43);
            this.nudTargetArmor.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nudTargetArmor.Name = "nudTargetArmor";
            this.nudTargetArmor.Size = new System.Drawing.Size(64, 21);
            this.nudTargetArmor.TabIndex = 28;
            this.nudTargetArmor.ValueChanged += new System.EventHandler(this.nudTargetArmor_ValueChanged);
            // 
            // lblFightLengthNum
            // 
            this.lblFightLengthNum.AutoSize = true;
            this.lblFightLengthNum.Location = new System.Drawing.Point(84, 100);
            this.lblFightLengthNum.Name = "lblFightLengthNum";
            this.lblFightLengthNum.Size = new System.Drawing.Size(21, 15);
            this.lblFightLengthNum.TabIndex = 2;
            this.lblFightLengthNum.Text = "10";
            // 
            // lblTargetArmor
            // 
            this.lblTargetArmor.AutoSize = true;
            this.lblTargetArmor.Location = new System.Drawing.Point(6, 45);
            this.lblTargetArmor.Name = "lblTargetArmor";
            this.lblTargetArmor.Size = new System.Drawing.Size(81, 15);
            this.lblTargetArmor.TabIndex = 24;
            this.lblTargetArmor.Text = "Target Armor:";
            // 
            // lblFightLength
            // 
            this.lblFightLength.AutoSize = true;
            this.lblFightLength.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFightLength.Location = new System.Drawing.Point(6, 73);
            this.lblFightLength.Name = "lblFightLength";
            this.lblFightLength.Size = new System.Drawing.Size(78, 15);
            this.lblFightLength.TabIndex = 0;
            this.lblFightLength.Text = "Fight Length:";
            // 
            // btnGraph
            // 
            this.btnGraph.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGraph.Location = new System.Drawing.Point(57, 506);
            this.btnGraph.Name = "btnGraph";
            this.btnGraph.Size = new System.Drawing.Size(75, 23);
            this.btnGraph.TabIndex = 5;
            this.btnGraph.Text = "Stat Graph";
            this.btnGraph.UseVisualStyleBackColor = true;
            this.btnGraph.Click += new System.EventHandler(this.btnGraph_Click);
            // 
            // checkBoxMeta
            // 
            this.checkBoxMeta.AutoSize = true;
            this.checkBoxMeta.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxMeta.Location = new System.Drawing.Point(13, 136);
            this.checkBoxMeta.Name = "checkBoxMeta";
            this.checkBoxMeta.Size = new System.Drawing.Size(180, 19);
            this.checkBoxMeta.TabIndex = 6;
            this.checkBoxMeta.Text = "Enforce Meta Requirements";
            this.checkBoxMeta.UseVisualStyleBackColor = true;
            this.checkBoxMeta.CheckedChanged += new System.EventHandler(this.checkBoxMeta_CheckedChanged);
            // 
            // gbMajorGlyph
            // 
            this.gbMajorGlyph.Controls.Add(this.cbGlyphGhoul);
            this.gbMajorGlyph.Controls.Add(this.cbGlyphPlagueStrike);
            this.gbMajorGlyph.Controls.Add(this.cbGlyphOblit);
            this.gbMajorGlyph.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbMajorGlyph.Location = new System.Drawing.Point(13, 161);
            this.gbMajorGlyph.Name = "gbMajorGlyph";
            this.gbMajorGlyph.Size = new System.Drawing.Size(185, 101);
            this.gbMajorGlyph.TabIndex = 33;
            this.gbMajorGlyph.TabStop = false;
            this.gbMajorGlyph.Text = "Major Glyphs";
            // 
            // cbGlyphGhoul
            // 
            this.cbGlyphGhoul.AutoSize = true;
            this.cbGlyphGhoul.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbGlyphGhoul.Location = new System.Drawing.Point(9, 70);
            this.cbGlyphGhoul.Name = "cbGlyphGhoul";
            this.cbGlyphGhoul.Size = new System.Drawing.Size(126, 19);
            this.cbGlyphGhoul.TabIndex = 40;
            this.cbGlyphGhoul.Text = "Glyph of the Ghoul";
            this.cbGlyphGhoul.UseVisualStyleBackColor = true;
            this.cbGlyphGhoul.CheckedChanged += new System.EventHandler(this.cbGlyphGhoul_CheckedChanged);
            // 
            // cbGlyphPlagueStrike
            // 
            this.cbGlyphPlagueStrike.AutoSize = true;
            this.cbGlyphPlagueStrike.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbGlyphPlagueStrike.Location = new System.Drawing.Point(9, 45);
            this.cbGlyphPlagueStrike.Name = "cbGlyphPlagueStrike";
            this.cbGlyphPlagueStrike.Size = new System.Drawing.Size(146, 19);
            this.cbGlyphPlagueStrike.TabIndex = 39;
            this.cbGlyphPlagueStrike.Text = "Glyph of Plague Strike";
            this.cbGlyphPlagueStrike.UseVisualStyleBackColor = true;
            this.cbGlyphPlagueStrike.CheckedChanged += new System.EventHandler(this.cbGlyphPlagueStrike_CheckedChanged);
            // 
            // cbGlyphOblit
            // 
            this.cbGlyphOblit.AutoSize = true;
            this.cbGlyphOblit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbGlyphOblit.Location = new System.Drawing.Point(9, 20);
            this.cbGlyphOblit.Name = "cbGlyphOblit";
            this.cbGlyphOblit.Size = new System.Drawing.Size(126, 19);
            this.cbGlyphOblit.TabIndex = 38;
            this.cbGlyphOblit.Text = "Glyph of Obliterate";
            this.cbGlyphOblit.UseVisualStyleBackColor = true;
            this.cbGlyphOblit.CheckedChanged += new System.EventHandler(this.cbGlyphOblit_CheckedChanged);
            // 
            // gbMinorGlyph
            // 
            this.gbMinorGlyph.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbMinorGlyph.Location = new System.Drawing.Point(13, 268);
            this.gbMinorGlyph.Name = "gbMinorGlyph";
            this.gbMinorGlyph.Size = new System.Drawing.Size(185, 46);
            this.gbMinorGlyph.TabIndex = 34;
            this.gbMinorGlyph.TabStop = false;
            this.gbMinorGlyph.Text = "Minor Glyphs";
            // 
            // btnRotation
            // 
            this.btnRotation.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRotation.Location = new System.Drawing.Point(39, 346);
            this.btnRotation.Name = "btnRotation";
            this.btnRotation.Size = new System.Drawing.Size(125, 23);
            this.btnRotation.TabIndex = 36;
            this.btnRotation.Text = "Rotation Details";
            this.btnRotation.UseVisualStyleBackColor = true;
            this.btnRotation.Click += new System.EventHandler(this.btnRotation_Click);
            // 
            // cbWindfuryEffect
            // 
            this.cbWindfuryEffect.AutoSize = true;
            this.cbWindfuryEffect.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbWindfuryEffect.Location = new System.Drawing.Point(13, 406);
            this.cbWindfuryEffect.Name = "cbWindfuryEffect";
            this.cbWindfuryEffect.Size = new System.Drawing.Size(160, 19);
            this.cbWindfuryEffect.TabIndex = 37;
            this.cbWindfuryEffect.Text = "Windfury / Imp Icy Talons";
            this.cbWindfuryEffect.UseVisualStyleBackColor = true;
            this.cbWindfuryEffect.CheckedChanged += new System.EventHandler(this.cbWindfuryEffect_CheckedChanged);
            // 
            // cbUREffect
            // 
            this.cbUREffect.AutoSize = true;
            this.cbUREffect.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbUREffect.Location = new System.Drawing.Point(13, 431);
            this.cbUREffect.Name = "cbUREffect";
            this.cbUREffect.Size = new System.Drawing.Size(119, 19);
            this.cbUREffect.TabIndex = 38;
            this.cbUREffect.Text = "UR / Abom Might";
            this.cbUREffect.UseVisualStyleBackColor = true;
            this.cbUREffect.CheckedChanged += new System.EventHandler(this.cbUREffect_CheckedChanged);
            // 
            // cbMagicVuln
            // 
            this.cbMagicVuln.AutoSize = true;
            this.cbMagicVuln.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbMagicVuln.Location = new System.Drawing.Point(13, 456);
            this.cbMagicVuln.Name = "cbMagicVuln";
            this.cbMagicVuln.Size = new System.Drawing.Size(129, 19);
            this.cbMagicVuln.TabIndex = 39;
            this.cbMagicVuln.Text = "Ebon Plague / CoE";
            this.cbMagicVuln.UseVisualStyleBackColor = true;
            this.cbMagicVuln.CheckedChanged += new System.EventHandler(this.cbMagicVuln_CheckedChanged);
            // 
            // cbCryptFever
            // 
            this.cbCryptFever.AutoSize = true;
            this.cbCryptFever.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbCryptFever.Location = new System.Drawing.Point(13, 481);
            this.cbCryptFever.Name = "cbCryptFever";
            this.cbCryptFever.Size = new System.Drawing.Size(86, 19);
            this.cbCryptFever.TabIndex = 40;
            this.cbCryptFever.Text = "Crypt Fever";
            this.cbCryptFever.UseVisualStyleBackColor = true;
            this.cbCryptFever.CheckedChanged += new System.EventHandler(this.cbCryptFever_CheckedChanged);
            // 
            // cbGhoul
            // 
            this.cbGhoul.AutoSize = true;
            this.cbGhoul.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbGhoul.Location = new System.Drawing.Point(13, 381);
            this.cbGhoul.Name = "cbGhoul";
            this.cbGhoul.Size = new System.Drawing.Size(104, 19);
            this.cbGhoul.TabIndex = 41;
            this.cbGhoul.Text = "Ghoul (on CD)";
            this.cbGhoul.UseVisualStyleBackColor = true;
            this.cbGhoul.CheckedChanged += new System.EventHandler(this.cbGhoul_CheckedChanged);
            // 
            // CalculationOptionsPanelDPSDK
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.cbGhoul);
            this.Controls.Add(this.cbCryptFever);
            this.Controls.Add(this.cbMagicVuln);
            this.Controls.Add(this.cbUREffect);
            this.Controls.Add(this.cbWindfuryEffect);
            this.Controls.Add(this.btnRotation);
            this.Controls.Add(this.gbMinorGlyph);
            this.Controls.Add(this.gbMajorGlyph);
            this.Controls.Add(this.checkBoxMeta);
            this.Controls.Add(this.btnGraph);
            this.Controls.Add(this.gbFightInfo);
            this.Name = "CalculationOptionsPanelDPSDK";
            this.Size = new System.Drawing.Size(209, 820);
            ((System.ComponentModel.ISupportInitialize)(this.tbFightLength)).EndInit();
            this.gbFightInfo.ResumeLayout(false);
            this.gbFightInfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudTargetArmor)).EndInit();
            this.gbMajorGlyph.ResumeLayout(false);
            this.gbMajorGlyph.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTargetLevel;
        private System.Windows.Forms.ComboBox cbTargetLevel;
        private System.Windows.Forms.GroupBox gbFightInfo;
        private System.Windows.Forms.TrackBar tbFightLength;
        private System.Windows.Forms.Label lblFightLength;
        private System.Windows.Forms.Button btnGraph;
        private System.Windows.Forms.Label lblFightLengthNum;
        private System.Windows.Forms.CheckBox checkBoxMeta;
        private System.Windows.Forms.Label lblTargetArmor;
        private System.Windows.Forms.NumericUpDown nudTargetArmor;
        private System.Windows.Forms.GroupBox gbMajorGlyph;
        private System.Windows.Forms.GroupBox gbMinorGlyph;
        private System.Windows.Forms.Button btnRotation;
        private System.Windows.Forms.CheckBox cbWindfuryEffect;
        private System.Windows.Forms.CheckBox cbUREffect;
        private System.Windows.Forms.CheckBox cbMagicVuln;
        private System.Windows.Forms.CheckBox cbCryptFever;
        private System.Windows.Forms.CheckBox cbGlyphPlagueStrike;
        private System.Windows.Forms.CheckBox cbGlyphOblit;
        private System.Windows.Forms.CheckBox cbGlyphGhoul;
        private System.Windows.Forms.CheckBox cbGhoul;
    }
}