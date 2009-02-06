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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkGlyphJudgement = new System.Windows.Forms.CheckBox();
            this.chk31Mode = new System.Windows.Forms.CheckBox();
            this.cmbMobType = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkGlyphJudgement);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(294, 126);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Glyphs";
            // 
            // chkGlyphJudgement
            // 
            this.chkGlyphJudgement.AutoSize = true;
            this.chkGlyphJudgement.Location = new System.Drawing.Point(7, 20);
            this.chkGlyphJudgement.Name = "chkGlyphJudgement";
            this.chkGlyphJudgement.Size = new System.Drawing.Size(120, 17);
            this.chkGlyphJudgement.TabIndex = 0;
            this.chkGlyphJudgement.Text = "Glyph of Judgement";
            this.chkGlyphJudgement.UseVisualStyleBackColor = true;
            this.chkGlyphJudgement.CheckedChanged += new System.EventHandler(this.chkGlyphJudgement_CheckedChanged);
            // 
            // chk31Mode
            // 
            this.chk31Mode.AutoSize = true;
            this.chk31Mode.Location = new System.Drawing.Point(3, 162);
            this.chk31Mode.Name = "chk31Mode";
            this.chk31Mode.Size = new System.Drawing.Size(71, 17);
            this.chk31Mode.TabIndex = 1;
            this.chk31Mode.Text = "3.1 Mode";
            this.chk31Mode.UseVisualStyleBackColor = true;
            this.chk31Mode.CheckedChanged += new System.EventHandler(this.chk31Mode_CheckedChanged);
            // 
            // cmbMobType
            // 
            this.cmbMobType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMobType.FormattingEnabled = true;
            this.cmbMobType.Items.AddRange(new object[] {
            "Undead, Demon",
            "Humanoid, Elemental",
            "Other"});
            this.cmbMobType.Location = new System.Drawing.Point(67, 135);
            this.cmbMobType.Name = "cmbMobType";
            this.cmbMobType.Size = new System.Drawing.Size(121, 21);
            this.cmbMobType.TabIndex = 2;
            this.cmbMobType.SelectedIndexChanged += new System.EventHandler(this.cmbMobType_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 138);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Mob Type:";
            // 
            // CalculationOptionsPanelRetribution
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbMobType);
            this.Controls.Add(this.chk31Mode);
            this.Controls.Add(this.groupBox1);
            this.Name = "CalculationOptionsPanelRetribution";
            this.Size = new System.Drawing.Size(300, 303);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chkGlyphJudgement;
        private System.Windows.Forms.CheckBox chk31Mode;
        private System.Windows.Forms.ComboBox cmbMobType;
        private System.Windows.Forms.Label label1;

    }
}