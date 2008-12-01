namespace Rawr.Retribution
{
    partial class CalculationOptionsPanelRetribution
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
            this.rbSoC = new System.Windows.Forms.RadioButton();
            this.rbSoB = new System.Windows.Forms.RadioButton();
            this.gbSealChoice = new System.Windows.Forms.GroupBox();
            this.gbSkillUsage = new System.Windows.Forms.GroupBox();
            this.chkHoW = new System.Windows.Forms.CheckBox();
            this.checkBoxExorcism = new System.Windows.Forms.CheckBox();
            this.checkBoxConsecration = new System.Windows.Forms.CheckBox();
            this.tbFightLength = new System.Windows.Forms.TrackBar();
            this.gbFightInfo = new System.Windows.Forms.GroupBox();
            this.nudTargetArmor = new System.Windows.Forms.NumericUpDown();
            this.lblFightLengthNum = new System.Windows.Forms.Label();
            this.lblTargetArmor = new System.Windows.Forms.Label();
            this.lblFightLength = new System.Windows.Forms.Label();
            this.btnGraph = new System.Windows.Forms.Button();
            this.gbMajorGlyph = new System.Windows.Forms.GroupBox();
            this.cbJudgeGlyph = new System.Windows.Forms.CheckBox();
            this.cbCSGlyph = new System.Windows.Forms.CheckBox();
            this.cbConsecGlyph = new System.Windows.Forms.CheckBox();
            this.cbAWGlyph = new System.Windows.Forms.CheckBox();
            this.cbSoCGlyph = new System.Windows.Forms.CheckBox();
            this.gbMinorGlyph = new System.Windows.Forms.GroupBox();
            this.cbSenseGlyph = new System.Windows.Forms.CheckBox();
            this.gbSealChoice.SuspendLayout();
            this.gbSkillUsage.SuspendLayout();
            ( (System.ComponentModel.ISupportInitialize)( this.tbFightLength ) ).BeginInit();
            this.gbFightInfo.SuspendLayout();
            ( (System.ComponentModel.ISupportInitialize)( this.nudTargetArmor ) ).BeginInit();
            this.gbMajorGlyph.SuspendLayout();
            this.gbMinorGlyph.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTargetLevel
            // 
            this.lblTargetLevel.AutoSize = true;
            this.lblTargetLevel.Font = new System.Drawing.Font( "Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( (byte)( 0 ) ) );
            this.lblTargetLevel.Location = new System.Drawing.Point( 6, 17 );
            this.lblTargetLevel.Name = "lblTargetLevel";
            this.lblTargetLevel.Size = new System.Drawing.Size( 77, 15 );
            this.lblTargetLevel.TabIndex = 0;
            this.lblTargetLevel.Text = "Target Level:";
            // 
            // cbTargetLevel
            // 
            this.cbTargetLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbTargetLevel.Font = new System.Drawing.Font( "Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( (byte)( 0 ) ) );
            this.cbTargetLevel.FormattingEnabled = true;
            this.cbTargetLevel.Items.AddRange( new object[] {
            "80",
            "81",
            "82",
            "83"} );
            this.cbTargetLevel.Location = new System.Drawing.Point( 87, 14 );
            this.cbTargetLevel.Name = "cbTargetLevel";
            this.cbTargetLevel.Size = new System.Drawing.Size( 49, 23 );
            this.cbTargetLevel.TabIndex = 1;
            this.cbTargetLevel.SelectedIndexChanged += new System.EventHandler( this.cbTargetLevel_SelectedIndexChanged );
            // 
            // rbSoC
            // 
            this.rbSoC.AutoSize = true;
            this.rbSoC.Font = new System.Drawing.Font( "Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( (byte)( 0 ) ) );
            this.rbSoC.Location = new System.Drawing.Point( 6, 20 );
            this.rbSoC.Name = "rbSoC";
            this.rbSoC.Size = new System.Drawing.Size( 48, 19 );
            this.rbSoC.TabIndex = 2;
            this.rbSoC.TabStop = true;
            this.rbSoC.Text = "SoC";
            this.rbSoC.UseVisualStyleBackColor = true;
            this.rbSoC.CheckedChanged += new System.EventHandler( this.rbSoC_CheckedChanged );
            // 
            // rbSoB
            // 
            this.rbSoB.AutoSize = true;
            this.rbSoB.Font = new System.Drawing.Font( "Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( (byte)( 0 ) ) );
            this.rbSoB.Location = new System.Drawing.Point( 6, 45 );
            this.rbSoB.Name = "rbSoB";
            this.rbSoB.Size = new System.Drawing.Size( 86, 19 );
            this.rbSoB.TabIndex = 2;
            this.rbSoB.TabStop = true;
            this.rbSoB.Text = "SoB / SotM";
            this.rbSoB.UseVisualStyleBackColor = true;
            this.rbSoB.CheckedChanged += new System.EventHandler( this.rbSoB_CheckedChanged );
            // 
            // gbSealChoice
            // 
            this.gbSealChoice.Controls.Add( this.rbSoC );
            this.gbSealChoice.Controls.Add( this.rbSoB );
            this.gbSealChoice.Font = new System.Drawing.Font( "Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( (byte)( 0 ) ) );
            this.gbSealChoice.Location = new System.Drawing.Point( 8, 15 );
            this.gbSealChoice.Name = "gbSealChoice";
            this.gbSealChoice.Size = new System.Drawing.Size( 185, 73 );
            this.gbSealChoice.TabIndex = 3;
            this.gbSealChoice.TabStop = false;
            this.gbSealChoice.Text = "Seal";
            // 
            // gbSkillUsage
            // 
            this.gbSkillUsage.Controls.Add( this.chkHoW );
            this.gbSkillUsage.Controls.Add( this.checkBoxExorcism );
            this.gbSkillUsage.Controls.Add( this.checkBoxConsecration );
            this.gbSkillUsage.Font = new System.Drawing.Font( "Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( (byte)( 0 ) ) );
            this.gbSkillUsage.Location = new System.Drawing.Point( 8, 227 );
            this.gbSkillUsage.Name = "gbSkillUsage";
            this.gbSkillUsage.Size = new System.Drawing.Size( 185, 97 );
            this.gbSkillUsage.TabIndex = 4;
            this.gbSkillUsage.TabStop = false;
            this.gbSkillUsage.Text = "Skill Usage";
            // 
            // chkHoW
            // 
            this.chkHoW.AutoSize = true;
            this.chkHoW.Enabled = false;
            this.chkHoW.Font = new System.Drawing.Font( "Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( (byte)( 0 ) ) );
            this.chkHoW.Location = new System.Drawing.Point( 6, 70 );
            this.chkHoW.Name = "chkHoW";
            this.chkHoW.Size = new System.Drawing.Size( 166, 19 );
            this.chkHoW.TabIndex = 30;
            this.chkHoW.Text = "Use HoW on CD sub 35%";
            this.chkHoW.UseVisualStyleBackColor = true;
            this.chkHoW.CheckedChanged += new System.EventHandler( this.chkHoW_CheckedChanged );
            // 
            // checkBoxExorcism
            // 
            this.checkBoxExorcism.AutoSize = true;
            this.checkBoxExorcism.Location = new System.Drawing.Point( 6, 45 );
            this.checkBoxExorcism.Name = "checkBoxExorcism";
            this.checkBoxExorcism.Size = new System.Drawing.Size( 77, 19 );
            this.checkBoxExorcism.TabIndex = 0;
            this.checkBoxExorcism.Text = "Exorcism";
            this.checkBoxExorcism.UseVisualStyleBackColor = true;
            this.checkBoxExorcism.CheckedChanged += new System.EventHandler( this.checkBoxExorcism_CheckedChanged );
            // 
            // checkBoxConsecration
            // 
            this.checkBoxConsecration.AutoSize = true;
            this.checkBoxConsecration.Location = new System.Drawing.Point( 6, 20 );
            this.checkBoxConsecration.Name = "checkBoxConsecration";
            this.checkBoxConsecration.Size = new System.Drawing.Size( 98, 19 );
            this.checkBoxConsecration.TabIndex = 0;
            this.checkBoxConsecration.Text = "Consecration";
            this.checkBoxConsecration.UseVisualStyleBackColor = true;
            this.checkBoxConsecration.CheckedChanged += new System.EventHandler( this.checkBoxConsecration_CheckedChanged );
            // 
            // tbFightLength
            // 
            this.tbFightLength.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.tbFightLength.Location = new System.Drawing.Point( 87, 70 );
            this.tbFightLength.Name = "tbFightLength";
            this.tbFightLength.Size = new System.Drawing.Size( 86, 45 );
            this.tbFightLength.TabIndex = 2;
            this.tbFightLength.Value = 10;
            this.tbFightLength.Scroll += new System.EventHandler( this.tbFightLength_Scroll );
            // 
            // gbFightInfo
            // 
            this.gbFightInfo.Controls.Add( this.nudTargetArmor );
            this.gbFightInfo.Controls.Add( this.lblFightLengthNum );
            this.gbFightInfo.Controls.Add( this.lblTargetArmor );
            this.gbFightInfo.Controls.Add( this.tbFightLength );
            this.gbFightInfo.Controls.Add( this.lblTargetLevel );
            this.gbFightInfo.Controls.Add( this.cbTargetLevel );
            this.gbFightInfo.Controls.Add( this.lblFightLength );
            this.gbFightInfo.Font = new System.Drawing.Font( "Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( (byte)( 0 ) ) );
            this.gbFightInfo.Location = new System.Drawing.Point( 8, 94 );
            this.gbFightInfo.Name = "gbFightInfo";
            this.gbFightInfo.Size = new System.Drawing.Size( 185, 127 );
            this.gbFightInfo.TabIndex = 4;
            this.gbFightInfo.TabStop = false;
            this.gbFightInfo.Text = "Fight Info";
            // 
            // nudTargetArmor
            // 
            this.nudTargetArmor.Increment = new decimal( new int[] {
            100,
            0,
            0,
            0} );
            this.nudTargetArmor.Location = new System.Drawing.Point( 87, 43 );
            this.nudTargetArmor.Maximum = new decimal( new int[] {
            10000,
            0,
            0,
            0} );
            this.nudTargetArmor.Name = "nudTargetArmor";
            this.nudTargetArmor.Size = new System.Drawing.Size( 64, 21 );
            this.nudTargetArmor.TabIndex = 28;
            this.nudTargetArmor.ValueChanged += new System.EventHandler( this.nudTargetArmor_ValueChanged );
            // 
            // lblFightLengthNum
            // 
            this.lblFightLengthNum.AutoSize = true;
            this.lblFightLengthNum.Location = new System.Drawing.Point( 84, 100 );
            this.lblFightLengthNum.Name = "lblFightLengthNum";
            this.lblFightLengthNum.Size = new System.Drawing.Size( 21, 15 );
            this.lblFightLengthNum.TabIndex = 2;
            this.lblFightLengthNum.Text = "10";
            // 
            // lblTargetArmor
            // 
            this.lblTargetArmor.AutoSize = true;
            this.lblTargetArmor.Location = new System.Drawing.Point( 6, 45 );
            this.lblTargetArmor.Name = "lblTargetArmor";
            this.lblTargetArmor.Size = new System.Drawing.Size( 81, 15 );
            this.lblTargetArmor.TabIndex = 24;
            this.lblTargetArmor.Text = "Target Armor:";
            // 
            // lblFightLength
            // 
            this.lblFightLength.AutoSize = true;
            this.lblFightLength.Font = new System.Drawing.Font( "Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( (byte)( 0 ) ) );
            this.lblFightLength.Location = new System.Drawing.Point( 6, 73 );
            this.lblFightLength.Name = "lblFightLength";
            this.lblFightLength.Size = new System.Drawing.Size( 78, 15 );
            this.lblFightLength.TabIndex = 0;
            this.lblFightLength.Text = "Fight Length:";
            // 
            // btnGraph
            // 
            this.btnGraph.Font = new System.Drawing.Font( "Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( (byte)( 0 ) ) );
            this.btnGraph.Location = new System.Drawing.Point( 66, 536 );
            this.btnGraph.Name = "btnGraph";
            this.btnGraph.Size = new System.Drawing.Size( 75, 23 );
            this.btnGraph.TabIndex = 5;
            this.btnGraph.Text = "Stat Graph";
            this.btnGraph.UseVisualStyleBackColor = true;
            this.btnGraph.Click += new System.EventHandler( this.btnGraph_Click );
            // 
            // gbMajorGlyph
            // 
            this.gbMajorGlyph.Controls.Add( this.cbJudgeGlyph );
            this.gbMajorGlyph.Controls.Add( this.cbCSGlyph );
            this.gbMajorGlyph.Controls.Add( this.cbConsecGlyph );
            this.gbMajorGlyph.Controls.Add( this.cbAWGlyph );
            this.gbMajorGlyph.Controls.Add( this.cbSoCGlyph );
            this.gbMajorGlyph.Font = new System.Drawing.Font( "Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( (byte)( 0 ) ) );
            this.gbMajorGlyph.Location = new System.Drawing.Point( 8, 330 );
            this.gbMajorGlyph.Name = "gbMajorGlyph";
            this.gbMajorGlyph.Size = new System.Drawing.Size( 185, 148 );
            this.gbMajorGlyph.TabIndex = 33;
            this.gbMajorGlyph.TabStop = false;
            this.gbMajorGlyph.Text = "Major Glyphs";
            // 
            // cbJudgeGlyph
            // 
            this.cbJudgeGlyph.AutoSize = true;
            this.cbJudgeGlyph.Font = new System.Drawing.Font( "Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( (byte)( 0 ) ) );
            this.cbJudgeGlyph.Location = new System.Drawing.Point( 6, 120 );
            this.cbJudgeGlyph.Name = "cbJudgeGlyph";
            this.cbJudgeGlyph.Size = new System.Drawing.Size( 135, 19 );
            this.cbJudgeGlyph.TabIndex = 32;
            this.cbJudgeGlyph.Text = "Glyph of Judgement";
            this.cbJudgeGlyph.UseVisualStyleBackColor = true;
            this.cbJudgeGlyph.CheckedChanged += new System.EventHandler( this.cbJudgeGlyph_CheckedChanged );
            // 
            // cbCSGlyph
            // 
            this.cbCSGlyph.AutoSize = true;
            this.cbCSGlyph.Font = new System.Drawing.Font( "Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( (byte)( 0 ) ) );
            this.cbCSGlyph.Location = new System.Drawing.Point( 6, 95 );
            this.cbCSGlyph.Name = "cbCSGlyph";
            this.cbCSGlyph.Size = new System.Drawing.Size( 157, 19 );
            this.cbCSGlyph.TabIndex = 31;
            this.cbCSGlyph.Text = "Glyph of Crusader Strike";
            this.cbCSGlyph.UseVisualStyleBackColor = true;
            this.cbCSGlyph.CheckedChanged += new System.EventHandler( this.cbCSGlyph_CheckedChanged );
            // 
            // cbConsecGlyph
            // 
            this.cbConsecGlyph.AutoSize = true;
            this.cbConsecGlyph.Font = new System.Drawing.Font( "Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( (byte)( 0 ) ) );
            this.cbConsecGlyph.Location = new System.Drawing.Point( 6, 70 );
            this.cbConsecGlyph.Name = "cbConsecGlyph";
            this.cbConsecGlyph.Size = new System.Drawing.Size( 145, 19 );
            this.cbConsecGlyph.TabIndex = 30;
            this.cbConsecGlyph.Text = "Glyph of Consecration";
            this.cbConsecGlyph.UseVisualStyleBackColor = true;
            this.cbConsecGlyph.CheckedChanged += new System.EventHandler( this.cbConsecGlyph_CheckedChanged );
            // 
            // cbAWGlyph
            // 
            this.cbAWGlyph.AutoSize = true;
            this.cbAWGlyph.Location = new System.Drawing.Point( 6, 45 );
            this.cbAWGlyph.Name = "cbAWGlyph";
            this.cbAWGlyph.Size = new System.Drawing.Size( 158, 19 );
            this.cbAWGlyph.TabIndex = 0;
            this.cbAWGlyph.Text = "Glyph of Avenging Wrath";
            this.cbAWGlyph.UseVisualStyleBackColor = true;
            this.cbAWGlyph.CheckedChanged += new System.EventHandler( this.cbAWGlyph_CheckedChanged );
            // 
            // cbSoCGlyph
            // 
            this.cbSoCGlyph.AutoSize = true;
            this.cbSoCGlyph.Location = new System.Drawing.Point( 6, 20 );
            this.cbSoCGlyph.Name = "cbSoCGlyph";
            this.cbSoCGlyph.Size = new System.Drawing.Size( 172, 19 );
            this.cbSoCGlyph.TabIndex = 0;
            this.cbSoCGlyph.Text = "Glyph of Seal of Command";
            this.cbSoCGlyph.UseVisualStyleBackColor = true;
            this.cbSoCGlyph.CheckedChanged += new System.EventHandler( this.cbSoCGlyph_CheckedChanged );
            // 
            // gbMinorGlyph
            // 
            this.gbMinorGlyph.Controls.Add( this.cbSenseGlyph );
            this.gbMinorGlyph.Font = new System.Drawing.Font( "Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( (byte)( 0 ) ) );
            this.gbMinorGlyph.Location = new System.Drawing.Point( 8, 484 );
            this.gbMinorGlyph.Name = "gbMinorGlyph";
            this.gbMinorGlyph.Size = new System.Drawing.Size( 185, 46 );
            this.gbMinorGlyph.TabIndex = 34;
            this.gbMinorGlyph.TabStop = false;
            this.gbMinorGlyph.Text = "Minor Glyphs";
            // 
            // cbSenseGlyph
            // 
            this.cbSenseGlyph.AutoSize = true;
            this.cbSenseGlyph.Font = new System.Drawing.Font( "Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( (byte)( 0 ) ) );
            this.cbSenseGlyph.Location = new System.Drawing.Point( 5, 20 );
            this.cbSenseGlyph.Name = "cbSenseGlyph";
            this.cbSenseGlyph.Size = new System.Drawing.Size( 155, 19 );
            this.cbSenseGlyph.TabIndex = 33;
            this.cbSenseGlyph.Text = "Glyph of Sense Undead";
            this.cbSenseGlyph.UseVisualStyleBackColor = true;
            this.cbSenseGlyph.CheckedChanged += new System.EventHandler( this.cbSenseGlyph_CheckedChanged );
            // 
            // CalculationOptionsPanelRetribution
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add( this.gbMinorGlyph );
            this.Controls.Add( this.gbMajorGlyph );
            this.Controls.Add( this.btnGraph );
            this.Controls.Add( this.gbFightInfo );
            this.Controls.Add( this.gbSkillUsage );
            this.Controls.Add( this.gbSealChoice );
            this.Name = "CalculationOptionsPanelRetribution";
            this.Size = new System.Drawing.Size( 209, 781 );
            this.gbSealChoice.ResumeLayout( false );
            this.gbSealChoice.PerformLayout();
            this.gbSkillUsage.ResumeLayout( false );
            this.gbSkillUsage.PerformLayout();
            ( (System.ComponentModel.ISupportInitialize)( this.tbFightLength ) ).EndInit();
            this.gbFightInfo.ResumeLayout( false );
            this.gbFightInfo.PerformLayout();
            ( (System.ComponentModel.ISupportInitialize)( this.nudTargetArmor ) ).EndInit();
            this.gbMajorGlyph.ResumeLayout( false );
            this.gbMajorGlyph.PerformLayout();
            this.gbMinorGlyph.ResumeLayout( false );
            this.gbMinorGlyph.PerformLayout();
            this.ResumeLayout( false );

        }

        #endregion

        private System.Windows.Forms.Label lblTargetLevel;
        private System.Windows.Forms.ComboBox cbTargetLevel;
        private System.Windows.Forms.RadioButton rbSoC;
        private System.Windows.Forms.RadioButton rbSoB;
        private System.Windows.Forms.GroupBox gbSealChoice;
        private System.Windows.Forms.GroupBox gbSkillUsage;
        private System.Windows.Forms.CheckBox checkBoxConsecration;
        private System.Windows.Forms.CheckBox checkBoxExorcism;
        private System.Windows.Forms.GroupBox gbFightInfo;
        private System.Windows.Forms.TrackBar tbFightLength;
        private System.Windows.Forms.Label lblFightLength;
        private System.Windows.Forms.Button btnGraph;
        private System.Windows.Forms.Label lblFightLengthNum;
        private System.Windows.Forms.Label lblTargetArmor;
        private System.Windows.Forms.NumericUpDown nudTargetArmor;
        private System.Windows.Forms.CheckBox chkHoW;
        private System.Windows.Forms.GroupBox gbMajorGlyph;
        private System.Windows.Forms.CheckBox cbConsecGlyph;
        private System.Windows.Forms.CheckBox cbAWGlyph;
        private System.Windows.Forms.CheckBox cbSoCGlyph;
        private System.Windows.Forms.CheckBox cbCSGlyph;
        private System.Windows.Forms.CheckBox cbJudgeGlyph;
        private System.Windows.Forms.GroupBox gbMinorGlyph;
        private System.Windows.Forms.CheckBox cbSenseGlyph;
    }
}