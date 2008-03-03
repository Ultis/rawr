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
            this.comboBoxTargetLevel = new System.Windows.Forms.ComboBox();
            this.rbSoC = new System.Windows.Forms.RadioButton();
            this.rbSoB = new System.Windows.Forms.RadioButton();
            this.groupBoxSealChoice = new System.Windows.Forms.GroupBox();
            this.groupBoxSkillUsage = new System.Windows.Forms.GroupBox();
            this.checkBoxExorcism = new System.Windows.Forms.CheckBox();
            this.checkBoxConsecration = new System.Windows.Forms.CheckBox();
            this.comboBoxConsRank = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.trackBarFightLength = new System.Windows.Forms.TrackBar();
            this.label2 = new System.Windows.Forms.Label();
            this.lblFightLength = new System.Windows.Forms.Label();
            this.txtArmor = new System.Windows.Forms.TextBox();
            this.groupBoxSealChoice.SuspendLayout();
            this.groupBoxSkillUsage.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarFightLength)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTargetLevel
            // 
            this.lblTargetLevel.AutoSize = true;
            this.lblTargetLevel.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTargetLevel.Location = new System.Drawing.Point(6, 16);
            this.lblTargetLevel.Name = "lblTargetLevel";
            this.lblTargetLevel.Size = new System.Drawing.Size(75, 14);
            this.lblTargetLevel.TabIndex = 0;
            this.lblTargetLevel.Text = "Target Level:";
            // 
            // comboBoxTargetLevel
            // 
            this.comboBoxTargetLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxTargetLevel.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxTargetLevel.FormattingEnabled = true;
            this.comboBoxTargetLevel.Items.AddRange(new object[] {
            "70",
            "71",
            "72",
            "73"});
            this.comboBoxTargetLevel.Location = new System.Drawing.Point(87, 13);
            this.comboBoxTargetLevel.Name = "comboBoxTargetLevel";
            this.comboBoxTargetLevel.Size = new System.Drawing.Size(49, 22);
            this.comboBoxTargetLevel.TabIndex = 1;
            this.comboBoxTargetLevel.SelectedIndexChanged += new System.EventHandler(this.comboBoxTargetLevel_SelectedIndexChanged);
            // 
            // rbSoC
            // 
            this.rbSoC.AutoSize = true;
            this.rbSoC.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbSoC.Location = new System.Drawing.Point(7, 19);
            this.rbSoC.Name = "rbSoC";
            this.rbSoC.Size = new System.Drawing.Size(120, 18);
            this.rbSoC.TabIndex = 2;
            this.rbSoC.TabStop = true;
            this.rbSoC.Text = "Seal of Command";
            this.rbSoC.UseVisualStyleBackColor = true;
            this.rbSoC.CheckedChanged += new System.EventHandler(this.rbSoC_CheckedChanged);
            // 
            // rbSoB
            // 
            this.rbSoB.AutoSize = true;
            this.rbSoB.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbSoB.Location = new System.Drawing.Point(7, 47);
            this.rbSoB.Name = "rbSoB";
            this.rbSoB.Size = new System.Drawing.Size(98, 18);
            this.rbSoB.TabIndex = 2;
            this.rbSoB.TabStop = true;
            this.rbSoB.Text = "Seal of Blood";
            this.rbSoB.UseVisualStyleBackColor = true;
            this.rbSoB.CheckedChanged += new System.EventHandler(this.rbSoB_CheckedChanged);
            // 
            // groupBoxSealChoice
            // 
            this.groupBoxSealChoice.Controls.Add(this.rbSoC);
            this.groupBoxSealChoice.Controls.Add(this.rbSoB);
            this.groupBoxSealChoice.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxSealChoice.Location = new System.Drawing.Point(8, 15);
            this.groupBoxSealChoice.Name = "groupBoxSealChoice";
            this.groupBoxSealChoice.Size = new System.Drawing.Size(185, 73);
            this.groupBoxSealChoice.TabIndex = 3;
            this.groupBoxSealChoice.TabStop = false;
            this.groupBoxSealChoice.Text = "Seal";
            // 
            // groupBoxSkillUsage
            // 
            this.groupBoxSkillUsage.Controls.Add(this.checkBoxExorcism);
            this.groupBoxSkillUsage.Controls.Add(this.checkBoxConsecration);
            this.groupBoxSkillUsage.Controls.Add(this.comboBoxConsRank);
            this.groupBoxSkillUsage.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxSkillUsage.Location = new System.Drawing.Point(8, 103);
            this.groupBoxSkillUsage.Name = "groupBoxSkillUsage";
            this.groupBoxSkillUsage.Size = new System.Drawing.Size(185, 89);
            this.groupBoxSkillUsage.TabIndex = 4;
            this.groupBoxSkillUsage.TabStop = false;
            this.groupBoxSkillUsage.Text = "Skill Usage";
            // 
            // checkBoxExorcism
            // 
            this.checkBoxExorcism.AutoSize = true;
            this.checkBoxExorcism.Location = new System.Drawing.Point(7, 54);
            this.checkBoxExorcism.Name = "checkBoxExorcism";
            this.checkBoxExorcism.Size = new System.Drawing.Size(73, 18);
            this.checkBoxExorcism.TabIndex = 0;
            this.checkBoxExorcism.Text = "Exorcism";
            this.checkBoxExorcism.UseVisualStyleBackColor = true;
            this.checkBoxExorcism.CheckedChanged += new System.EventHandler(this.checkBoxExorcism_CheckedChanged);
            // 
            // checkBoxConsecration
            // 
            this.checkBoxConsecration.AutoSize = true;
            this.checkBoxConsecration.Location = new System.Drawing.Point(7, 30);
            this.checkBoxConsecration.Name = "checkBoxConsecration";
            this.checkBoxConsecration.Size = new System.Drawing.Size(97, 18);
            this.checkBoxConsecration.TabIndex = 0;
            this.checkBoxConsecration.Text = "Consecration";
            this.checkBoxConsecration.UseVisualStyleBackColor = true;
            this.checkBoxConsecration.CheckedChanged += new System.EventHandler(this.checkBoxConsecration_CheckedChanged);
            // 
            // comboBoxConsRank
            // 
            this.comboBoxConsRank.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxConsRank.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxConsRank.FormattingEnabled = true;
            this.comboBoxConsRank.Items.AddRange(new object[] {
            "Rank 1",
            "Rank 8"});
            this.comboBoxConsRank.Location = new System.Drawing.Point(110, 26);
            this.comboBoxConsRank.Name = "comboBoxConsRank";
            this.comboBoxConsRank.Size = new System.Drawing.Size(66, 22);
            this.comboBoxConsRank.TabIndex = 1;
            this.comboBoxConsRank.SelectedIndexChanged += new System.EventHandler(this.comboBoxConsRank_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtArmor);
            this.groupBox1.Controls.Add(this.trackBarFightLength);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.lblFightLength);
            this.groupBox1.Controls.Add(this.lblTargetLevel);
            this.groupBox1.Controls.Add(this.comboBoxTargetLevel);
            this.groupBox1.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(8, 211);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(185, 142);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Skill Usage";
            // 
            // trackBarFightLength
            // 
            this.trackBarFightLength.Location = new System.Drawing.Point(87, 41);
            this.trackBarFightLength.Name = "trackBarFightLength";
            this.trackBarFightLength.Size = new System.Drawing.Size(86, 45);
            this.trackBarFightLength.TabIndex = 2;
            this.trackBarFightLength.Value = 10;
            this.trackBarFightLength.Scroll += new System.EventHandler(this.trackBarFightLength_Scroll);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(5, 87);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 14);
            this.label2.TabIndex = 0;
            this.label2.Text = "Target Armor:";
            // 
            // lblFightLength
            // 
            this.lblFightLength.AutoSize = true;
            this.lblFightLength.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFightLength.Location = new System.Drawing.Point(5, 48);
            this.lblFightLength.Name = "lblFightLength";
            this.lblFightLength.Size = new System.Drawing.Size(76, 14);
            this.lblFightLength.TabIndex = 0;
            this.lblFightLength.Text = "Fight Length:";
            // 
            // txtArmor
            // 
            this.txtArmor.Location = new System.Drawing.Point(87, 87);
            this.txtArmor.Name = "txtArmor";
            this.txtArmor.Size = new System.Drawing.Size(72, 22);
            this.txtArmor.TabIndex = 3;
            this.txtArmor.Text = "7700";
            this.txtArmor.TextChanged += new System.EventHandler(this.txtArmor_TextChanged);
            // 
            // CalculationOptionsPanelRetribution
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBoxSkillUsage);
            this.Controls.Add(this.groupBoxSealChoice);
            this.Name = "CalculationOptionsPanelRetribution";
            this.Size = new System.Drawing.Size(196, 428);
            this.groupBoxSealChoice.ResumeLayout(false);
            this.groupBoxSealChoice.PerformLayout();
            this.groupBoxSkillUsage.ResumeLayout(false);
            this.groupBoxSkillUsage.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarFightLength)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblTargetLevel;
        private System.Windows.Forms.ComboBox comboBoxTargetLevel;
        private System.Windows.Forms.RadioButton rbSoC;
        private System.Windows.Forms.RadioButton rbSoB;
        private System.Windows.Forms.GroupBox groupBoxSealChoice;
        private System.Windows.Forms.GroupBox groupBoxSkillUsage;
        private System.Windows.Forms.CheckBox checkBoxConsecration;
        private System.Windows.Forms.ComboBox comboBoxConsRank;
        private System.Windows.Forms.CheckBox checkBoxExorcism;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TrackBar trackBarFightLength;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblFightLength;
        private System.Windows.Forms.TextBox txtArmor;
    }
}