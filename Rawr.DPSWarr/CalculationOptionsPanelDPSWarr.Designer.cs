namespace Rawr.DPSWarr
{
    partial class CalculationOptionsPanelDPSWarr
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
            this.SimModeCombo = new System.Windows.Forms.ComboBox();
            this.FightLength = new System.Windows.Forms.Label();
            this.SimMode = new System.Windows.Forms.Label();
            this.FightLengthEdit = new System.Windows.Forms.TextBox();
            this.HeroicStrikeRageEdit = new System.Windows.Forms.TextBox();
            this.HeroicStrikeRage = new System.Windows.Forms.Label();
            this.TargetArmorEdit = new System.Windows.Forms.TextBox();
            this.TargetArmor = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.GlyphOfWhirlwind = new System.Windows.Forms.CheckBox();
            this.GlyphOfHeroicStrike = new System.Windows.Forms.CheckBox();
            this.GlyphOfMortalStrike = new System.Windows.Forms.CheckBox();
            this.GlyphOfExecute = new System.Windows.Forms.CheckBox();
            this.HideLowQualityItems = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // SimModeCombo
            // 
            this.SimModeCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SimModeCombo.FormattingEnabled = true;
            this.SimModeCombo.Items.AddRange(new object[] {
            "Fury - DW>WW>BT>Slam>HS",
            "Arms - Rend>BS>Ex>MS>OP>Slam"});
            this.SimModeCombo.Location = new System.Drawing.Point(127, 17);
            this.SimModeCombo.Name = "SimModeCombo";
            this.SimModeCombo.Size = new System.Drawing.Size(147, 21);
            this.SimModeCombo.TabIndex = 0;
            this.SimModeCombo.SelectedIndexChanged += new System.EventHandler(this.SimModeCombo_SelectedIndexChanged);
            // 
            // FightLength
            // 
            this.FightLength.AutoSize = true;
            this.FightLength.Location = new System.Drawing.Point(13, 60);
            this.FightLength.Name = "FightLength";
            this.FightLength.Size = new System.Drawing.Size(66, 13);
            this.FightLength.TabIndex = 1;
            this.FightLength.Text = "Fight Length";
            // 
            // SimMode
            // 
            this.SimMode.AutoSize = true;
            this.SimMode.Location = new System.Drawing.Point(13, 22);
            this.SimMode.Name = "SimMode";
            this.SimMode.Size = new System.Drawing.Size(54, 13);
            this.SimMode.TabIndex = 2;
            this.SimMode.Text = "Sim Mode";
            // 
            // FightLengthEdit
            // 
            this.FightLengthEdit.Location = new System.Drawing.Point(127, 57);
            this.FightLengthEdit.Name = "FightLengthEdit";
            this.FightLengthEdit.Size = new System.Drawing.Size(147, 20);
            this.FightLengthEdit.TabIndex = 3;
            this.FightLengthEdit.TextChanged += new System.EventHandler(this.FightLengthEdit_TextChanged);
            // 
            // HeroicStrikeRageEdit
            // 
            this.HeroicStrikeRageEdit.Location = new System.Drawing.Point(127, 95);
            this.HeroicStrikeRageEdit.Name = "HeroicStrikeRageEdit";
            this.HeroicStrikeRageEdit.Size = new System.Drawing.Size(147, 20);
            this.HeroicStrikeRageEdit.TabIndex = 5;
            this.HeroicStrikeRageEdit.TextChanged += new System.EventHandler(this.HeroicStrikeRageEdit_TextChanged);
            // 
            // HeroicStrikeRage
            // 
            this.HeroicStrikeRage.AutoSize = true;
            this.HeroicStrikeRage.Location = new System.Drawing.Point(13, 98);
            this.HeroicStrikeRage.Name = "HeroicStrikeRage";
            this.HeroicStrikeRage.Size = new System.Drawing.Size(106, 13);
            this.HeroicStrikeRage.TabIndex = 4;
            this.HeroicStrikeRage.Text = "Heroic Strike > Rage";
            // 
            // TargetArmorEdit
            // 
            this.TargetArmorEdit.Location = new System.Drawing.Point(127, 133);
            this.TargetArmorEdit.Name = "TargetArmorEdit";
            this.TargetArmorEdit.Size = new System.Drawing.Size(147, 20);
            this.TargetArmorEdit.TabIndex = 7;
            this.TargetArmorEdit.Text = "13000";
            this.TargetArmorEdit.TextChanged += new System.EventHandler(this.TargetArmorEdit_TextChanged);
            // 
            // TargetArmor
            // 
            this.TargetArmor.AutoSize = true;
            this.TargetArmor.Location = new System.Drawing.Point(13, 136);
            this.TargetArmor.Name = "TargetArmor";
            this.TargetArmor.Size = new System.Drawing.Size(68, 13);
            this.TargetArmor.TabIndex = 6;
            this.TargetArmor.Text = "Target Armor";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(276, 60);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(12, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "s";
            // 
            // GlyphOfWhirlwind
            // 
            this.GlyphOfWhirlwind.AutoSize = true;
            this.GlyphOfWhirlwind.Location = new System.Drawing.Point(16, 174);
            this.GlyphOfWhirlwind.Name = "GlyphOfWhirlwind";
            this.GlyphOfWhirlwind.Size = new System.Drawing.Size(116, 17);
            this.GlyphOfWhirlwind.TabIndex = 9;
            this.GlyphOfWhirlwind.Text = "Glyph Of Whirlwind";
            this.GlyphOfWhirlwind.UseVisualStyleBackColor = true;
            this.GlyphOfWhirlwind.CheckedChanged += new System.EventHandler(this.GlyphOfWhirlwind_CheckedChanged);
            // 
            // GlyphOfHeroicStrike
            // 
            this.GlyphOfHeroicStrike.AutoSize = true;
            this.GlyphOfHeroicStrike.Location = new System.Drawing.Point(16, 207);
            this.GlyphOfHeroicStrike.Name = "GlyphOfHeroicStrike";
            this.GlyphOfHeroicStrike.Size = new System.Drawing.Size(131, 17);
            this.GlyphOfHeroicStrike.TabIndex = 10;
            this.GlyphOfHeroicStrike.Text = "Glyph Of Heroic Strike";
            this.GlyphOfHeroicStrike.UseVisualStyleBackColor = true;
            this.GlyphOfHeroicStrike.CheckedChanged += new System.EventHandler(this.GlyphOfHeroicStrike_CheckedChanged);
            // 
            // GlyphOfMortalStrike
            // 
            this.GlyphOfMortalStrike.AutoSize = true;
            this.GlyphOfMortalStrike.Location = new System.Drawing.Point(16, 239);
            this.GlyphOfMortalStrike.Name = "GlyphOfMortalStrike";
            this.GlyphOfMortalStrike.Size = new System.Drawing.Size(129, 17);
            this.GlyphOfMortalStrike.TabIndex = 11;
            this.GlyphOfMortalStrike.Text = "Glyph Of Mortal Strike";
            this.GlyphOfMortalStrike.UseVisualStyleBackColor = true;
            this.GlyphOfMortalStrike.CheckedChanged += new System.EventHandler(this.GlyphOfMortalStrike_CheckedChanged);
            // 
            // GlyphOfExecute
            // 
            this.GlyphOfExecute.AutoSize = true;
            this.GlyphOfExecute.Location = new System.Drawing.Point(16, 271);
            this.GlyphOfExecute.Name = "GlyphOfExecute";
            this.GlyphOfExecute.Size = new System.Drawing.Size(109, 17);
            this.GlyphOfExecute.TabIndex = 12;
            this.GlyphOfExecute.Text = "Glyph Of Execute";
            this.GlyphOfExecute.UseVisualStyleBackColor = true;
            this.GlyphOfExecute.CheckedChanged += new System.EventHandler(this.GlyphOfExecute_CheckedChanged);
            // 
            // HideLowQualityItems
            // 
            this.HideLowQualityItems.AutoSize = true;
            this.HideLowQualityItems.Location = new System.Drawing.Point(16, 315);
            this.HideLowQualityItems.Name = "HideLowQualityItems";
            this.HideLowQualityItems.Size = new System.Drawing.Size(127, 17);
            this.HideLowQualityItems.TabIndex = 13;
            this.HideLowQualityItems.Text = "Hide low quality items";
            this.HideLowQualityItems.UseVisualStyleBackColor = true;
            this.HideLowQualityItems.CheckedChanged += new System.EventHandler(this.HideLowQualityItems_CheckedChanged);
            // 
            // CalculationOptionsPanelDPSWarr
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.HideLowQualityItems);
            this.Controls.Add(this.GlyphOfExecute);
            this.Controls.Add(this.GlyphOfMortalStrike);
            this.Controls.Add(this.GlyphOfHeroicStrike);
            this.Controls.Add(this.GlyphOfWhirlwind);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.TargetArmorEdit);
            this.Controls.Add(this.TargetArmor);
            this.Controls.Add(this.HeroicStrikeRageEdit);
            this.Controls.Add(this.HeroicStrikeRage);
            this.Controls.Add(this.FightLengthEdit);
            this.Controls.Add(this.SimMode);
            this.Controls.Add(this.FightLength);
            this.Controls.Add(this.SimModeCombo);
            this.Name = "CalculationOptionsPanelDPSWarr";
            this.Size = new System.Drawing.Size(295, 508);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox SimModeCombo;
        private System.Windows.Forms.Label FightLength;
        private System.Windows.Forms.Label SimMode;
        private System.Windows.Forms.TextBox FightLengthEdit;
        private System.Windows.Forms.TextBox HeroicStrikeRageEdit;
        private System.Windows.Forms.Label HeroicStrikeRage;
        private System.Windows.Forms.TextBox TargetArmorEdit;
        private System.Windows.Forms.Label TargetArmor;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox GlyphOfWhirlwind;
        private System.Windows.Forms.CheckBox GlyphOfHeroicStrike;
        private System.Windows.Forms.CheckBox GlyphOfMortalStrike;
        private System.Windows.Forms.CheckBox GlyphOfExecute;
        private System.Windows.Forms.CheckBox HideLowQualityItems;

    }
}