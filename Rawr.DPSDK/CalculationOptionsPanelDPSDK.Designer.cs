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
            this.lblFerociousInspirationNum = new System.Windows.Forms.Label();
            this.tbFerociousInspiration = new System.Windows.Forms.TrackBar();
            this.lblFerociousInspiration = new System.Windows.Forms.Label();
            this.chkBloodLust = new System.Windows.Forms.CheckBox();
            this.gbDrums = new System.Windows.Forms.GroupBox();
            this.rbDrumsBattle = new System.Windows.Forms.RadioButton();
            this.rbDrumsWar = new System.Windows.Forms.RadioButton();
            this.rbDrumsNone = new System.Windows.Forms.RadioButton();
            this.cbWindfuryEffect = new System.Windows.Forms.CheckBox();
            this.gbMajorGlyph = new System.Windows.Forms.GroupBox();
            this.gbMinorGlyph = new System.Windows.Forms.GroupBox();
            this.gbRotation = new System.Windows.Forms.GroupBox();
            this.rbFrost = new System.Windows.Forms.RadioButton();
            this.rbBlood = new System.Windows.Forms.RadioButton();
            this.rbUnholy = new System.Windows.Forms.RadioButton();
            this.btnRotation = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.tbFightLength)).BeginInit();
            this.gbFightInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudTargetArmor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbFerociousInspiration)).BeginInit();
            this.gbDrums.SuspendLayout();
            this.gbRotation.SuspendLayout();
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
            "70",
            "71",
            "72",
            "73"});
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
            this.btnGraph.Location = new System.Drawing.Point(61, 794);
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
            this.checkBoxMeta.Location = new System.Drawing.Point(14, 136);
            this.checkBoxMeta.Name = "checkBoxMeta";
            this.checkBoxMeta.Size = new System.Drawing.Size(180, 19);
            this.checkBoxMeta.TabIndex = 6;
            this.checkBoxMeta.Text = "Enforce Meta Requirements";
            this.checkBoxMeta.UseVisualStyleBackColor = true;
            this.checkBoxMeta.CheckedChanged += new System.EventHandler(this.checkBoxMeta_CheckedChanged);
            // 
            // lblFerociousInspirationNum
            // 
            this.lblFerociousInspirationNum.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblFerociousInspirationNum.AutoSize = true;
            this.lblFerociousInspirationNum.Location = new System.Drawing.Point(67, 301);
            this.lblFerociousInspirationNum.Name = "lblFerociousInspirationNum";
            this.lblFerociousInspirationNum.Size = new System.Drawing.Size(13, 13);
            this.lblFerociousInspirationNum.TabIndex = 18;
            this.lblFerociousInspirationNum.Text = "2";
            // 
            // tbFerociousInspiration
            // 
            this.tbFerociousInspiration.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbFerociousInspiration.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.tbFerociousInspiration.Location = new System.Drawing.Point(70, 269);
            this.tbFerociousInspiration.Maximum = 4;
            this.tbFerociousInspiration.Minimum = 1;
            this.tbFerociousInspiration.Name = "tbFerociousInspiration";
            this.tbFerociousInspiration.Size = new System.Drawing.Size(125, 45);
            this.tbFerociousInspiration.TabIndex = 21;
            this.tbFerociousInspiration.Value = 2;
            this.tbFerociousInspiration.ValueChanged += new System.EventHandler(this.tbFerociousInspiration_ValueChanged);
            // 
            // lblFerociousInspiration
            // 
            this.lblFerociousInspiration.AutoSize = true;
            this.lblFerociousInspiration.Location = new System.Drawing.Point(10, 266);
            this.lblFerociousInspiration.Name = "lblFerociousInspiration";
            this.lblFerociousInspiration.Size = new System.Drawing.Size(55, 39);
            this.lblFerociousInspiration.TabIndex = 9;
            this.lblFerociousInspiration.Text = "Ferocious\r\nInspiration\r\nCount:";
            this.lblFerociousInspiration.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // chkBloodLust
            // 
            this.chkBloodLust.AutoSize = true;
            this.chkBloodLust.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkBloodLust.Location = new System.Drawing.Point(13, 161);
            this.chkBloodLust.Name = "chkBloodLust";
            this.chkBloodLust.Size = new System.Drawing.Size(164, 19);
            this.chkBloodLust.TabIndex = 30;
            this.chkBloodLust.Text = "Bloodlust/Heroism on CD";
            this.chkBloodLust.UseVisualStyleBackColor = true;
            this.chkBloodLust.CheckedChanged += new System.EventHandler(this.chkBloodLust_CheckedChanged);
            // 
            // gbDrums
            // 
            this.gbDrums.Controls.Add(this.rbDrumsBattle);
            this.gbDrums.Controls.Add(this.rbDrumsWar);
            this.gbDrums.Controls.Add(this.rbDrumsNone);
            this.gbDrums.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbDrums.Location = new System.Drawing.Point(13, 211);
            this.gbDrums.Name = "gbDrums";
            this.gbDrums.Size = new System.Drawing.Size(185, 52);
            this.gbDrums.TabIndex = 31;
            this.gbDrums.TabStop = false;
            this.gbDrums.Text = "Drums";
            // 
            // rbDrumsBattle
            // 
            this.rbDrumsBattle.AutoSize = true;
            this.rbDrumsBattle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbDrumsBattle.Location = new System.Drawing.Point(124, 20);
            this.rbDrumsBattle.Name = "rbDrumsBattle";
            this.rbDrumsBattle.Size = new System.Drawing.Size(56, 19);
            this.rbDrumsBattle.TabIndex = 4;
            this.rbDrumsBattle.TabStop = true;
            this.rbDrumsBattle.Text = "Battle";
            this.rbDrumsBattle.UseVisualStyleBackColor = true;
            this.rbDrumsBattle.CheckedChanged += new System.EventHandler(this.rbDrumsBattle_CheckedChanged);
            // 
            // rbDrumsWar
            // 
            this.rbDrumsWar.AutoSize = true;
            this.rbDrumsWar.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbDrumsWar.Location = new System.Drawing.Point(66, 20);
            this.rbDrumsWar.Name = "rbDrumsWar";
            this.rbDrumsWar.Size = new System.Drawing.Size(47, 19);
            this.rbDrumsWar.TabIndex = 3;
            this.rbDrumsWar.TabStop = true;
            this.rbDrumsWar.Text = "War";
            this.rbDrumsWar.UseVisualStyleBackColor = true;
            this.rbDrumsWar.CheckedChanged += new System.EventHandler(this.rbDrumsWar_CheckedChanged);
            // 
            // rbDrumsNone
            // 
            this.rbDrumsNone.AutoSize = true;
            this.rbDrumsNone.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbDrumsNone.Location = new System.Drawing.Point(5, 20);
            this.rbDrumsNone.Name = "rbDrumsNone";
            this.rbDrumsNone.Size = new System.Drawing.Size(55, 19);
            this.rbDrumsNone.TabIndex = 2;
            this.rbDrumsNone.TabStop = true;
            this.rbDrumsNone.Text = "None";
            this.rbDrumsNone.UseVisualStyleBackColor = true;
            this.rbDrumsNone.CheckedChanged += new System.EventHandler(this.rbDrumsNone_CheckedChanged);
            // 
            // cbWindfuryEffect
            // 
            this.cbWindfuryEffect.AutoSize = true;
            this.cbWindfuryEffect.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbWindfuryEffect.Location = new System.Drawing.Point(13, 186);
            this.cbWindfuryEffect.Name = "cbWindfuryEffect";
            this.cbWindfuryEffect.Size = new System.Drawing.Size(154, 19);
            this.cbWindfuryEffect.TabIndex = 32;
            this.cbWindfuryEffect.Text = "Windfury/Horn of Winter";
            this.cbWindfuryEffect.UseVisualStyleBackColor = true;
            this.cbWindfuryEffect.CheckedChanged += new System.EventHandler(this.cbWindfuryEffect_CheckedChanged);
            // 
            // gbMajorGlyph
            // 
            this.gbMajorGlyph.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbMajorGlyph.Location = new System.Drawing.Point(13, 320);
            this.gbMajorGlyph.Name = "gbMajorGlyph";
            this.gbMajorGlyph.Size = new System.Drawing.Size(185, 148);
            this.gbMajorGlyph.TabIndex = 33;
            this.gbMajorGlyph.TabStop = false;
            this.gbMajorGlyph.Text = "Major Glyphs";
            // 
            // gbMinorGlyph
            // 
            this.gbMinorGlyph.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbMinorGlyph.Location = new System.Drawing.Point(13, 474);
            this.gbMinorGlyph.Name = "gbMinorGlyph";
            this.gbMinorGlyph.Size = new System.Drawing.Size(185, 46);
            this.gbMinorGlyph.TabIndex = 34;
            this.gbMinorGlyph.TabStop = false;
            this.gbMinorGlyph.Text = "Minor Glyphs";
            // 
            // gbRotation
            // 
            this.gbRotation.Controls.Add(this.rbFrost);
            this.gbRotation.Controls.Add(this.rbBlood);
            this.gbRotation.Controls.Add(this.rbUnholy);
            this.gbRotation.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbRotation.Location = new System.Drawing.Point(13, 526);
            this.gbRotation.Name = "gbRotation";
            this.gbRotation.Size = new System.Drawing.Size(185, 74);
            this.gbRotation.TabIndex = 35;
            this.gbRotation.TabStop = false;
            this.gbRotation.Text = "Rotation";
            // 
            // rbFrost
            // 
            this.rbFrost.AutoSize = true;
            this.rbFrost.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbFrost.Location = new System.Drawing.Point(124, 20);
            this.rbFrost.Name = "rbFrost";
            this.rbFrost.Size = new System.Drawing.Size(52, 19);
            this.rbFrost.TabIndex = 4;
            this.rbFrost.TabStop = true;
            this.rbFrost.Text = "Frost";
            this.rbFrost.UseVisualStyleBackColor = true;
            this.rbFrost.CheckedChanged += new System.EventHandler(this.rbFrost_CheckedChanged);
            // 
            // rbBlood
            // 
            this.rbBlood.AutoSize = true;
            this.rbBlood.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbBlood.Location = new System.Drawing.Point(66, 49);
            this.rbBlood.Name = "rbBlood";
            this.rbBlood.Size = new System.Drawing.Size(57, 19);
            this.rbBlood.TabIndex = 3;
            this.rbBlood.TabStop = true;
            this.rbBlood.Text = "Blood";
            this.rbBlood.UseVisualStyleBackColor = true;
            this.rbBlood.CheckedChanged += new System.EventHandler(this.rbBlood_CheckedChanged);
            // 
            // rbUnholy
            // 
            this.rbUnholy.AutoSize = true;
            this.rbUnholy.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbUnholy.Location = new System.Drawing.Point(9, 20);
            this.rbUnholy.Name = "rbUnholy";
            this.rbUnholy.Size = new System.Drawing.Size(63, 19);
            this.rbUnholy.TabIndex = 2;
            this.rbUnholy.TabStop = true;
            this.rbUnholy.Text = "Unholy";
            this.rbUnholy.UseVisualStyleBackColor = true;
            this.rbUnholy.CheckedChanged += new System.EventHandler(this.rbUnholy_CheckedChanged);
            // 
            // btnRotation
            // 
            this.btnRotation.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRotation.Location = new System.Drawing.Point(39, 606);
            this.btnRotation.Name = "btnRotation";
            this.btnRotation.Size = new System.Drawing.Size(125, 23);
            this.btnRotation.TabIndex = 36;
            this.btnRotation.Text = "Rotation Details";
            this.btnRotation.UseVisualStyleBackColor = true;
            // 
            // CalculationOptionsPanelDPSDK
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.btnRotation);
            this.Controls.Add(this.gbRotation);
            this.Controls.Add(this.gbMinorGlyph);
            this.Controls.Add(this.gbMajorGlyph);
            this.Controls.Add(this.cbWindfuryEffect);
            this.Controls.Add(this.gbDrums);
            this.Controls.Add(this.chkBloodLust);
            this.Controls.Add(this.lblFerociousInspirationNum);
            this.Controls.Add(this.tbFerociousInspiration);
            this.Controls.Add(this.lblFerociousInspiration);
            this.Controls.Add(this.checkBoxMeta);
            this.Controls.Add(this.btnGraph);
            this.Controls.Add(this.gbFightInfo);
            this.Name = "CalculationOptionsPanelDPSDK";
            this.Size = new System.Drawing.Size(209, 820);
            ((System.ComponentModel.ISupportInitialize)(this.tbFightLength)).EndInit();
            this.gbFightInfo.ResumeLayout(false);
            this.gbFightInfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudTargetArmor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbFerociousInspiration)).EndInit();
            this.gbDrums.ResumeLayout(false);
            this.gbDrums.PerformLayout();
            this.gbRotation.ResumeLayout(false);
            this.gbRotation.PerformLayout();
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
        private System.Windows.Forms.Label lblFerociousInspirationNum;
        private System.Windows.Forms.TrackBar tbFerociousInspiration;
        private System.Windows.Forms.Label lblFerociousInspiration;
        private System.Windows.Forms.Label lblTargetArmor;
        private System.Windows.Forms.NumericUpDown nudTargetArmor;
        private System.Windows.Forms.CheckBox chkBloodLust;
        private System.Windows.Forms.GroupBox gbDrums;
        private System.Windows.Forms.RadioButton rbDrumsBattle;
        private System.Windows.Forms.RadioButton rbDrumsWar;
        private System.Windows.Forms.RadioButton rbDrumsNone;
        private System.Windows.Forms.CheckBox cbWindfuryEffect;
        private System.Windows.Forms.GroupBox gbMajorGlyph;
        private System.Windows.Forms.GroupBox gbMinorGlyph;
        private System.Windows.Forms.GroupBox gbRotation;
        private System.Windows.Forms.RadioButton rbFrost;
        private System.Windows.Forms.RadioButton rbBlood;
        private System.Windows.Forms.RadioButton rbUnholy;
        private System.Windows.Forms.Button btnRotation;
    }
}