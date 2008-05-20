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
            this.lblTargetLevel = new System.Windows.Forms.Label();
            this.comboBoxTargetLevel = new System.Windows.Forms.ComboBox();
            this.rbBattle = new System.Windows.Forms.RadioButton();
            this.groupBoxStanceChoice = new System.Windows.Forms.GroupBox();
            this.rbBerserker = new System.Windows.Forms.RadioButton();
            this.groupBoxSkillUsage = new System.Windows.Forms.GroupBox();
            this.txtslamLatency = new System.Windows.Forms.TextBox();
            this.slamLatLabel = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblLength = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtArmor = new System.Windows.Forms.TextBox();
            this.trackBarFightLength = new System.Windows.Forms.TrackBar();
            this.label2 = new System.Windows.Forms.Label();
            this.lblFightLength = new System.Windows.Forms.Label();
            this.btnTalents = new System.Windows.Forms.Button();
            this.btnGraph = new System.Windows.Forms.Button();
            this.checkBoxMeta = new System.Windows.Forms.CheckBox();
            this.radioButtonAldor = new System.Windows.Forms.RadioButton();
            this.radioButtonScryer = new System.Windows.Forms.RadioButton();
            this.Faction = new System.Windows.Forms.GroupBox();
            this.flurryCheck = new System.Windows.Forms.CheckBox();
            this.groupBoxStanceChoice.SuspendLayout();
            this.groupBoxSkillUsage.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarFightLength)).BeginInit();
            this.Faction.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTargetLevel
            // 
            this.lblTargetLevel.AutoSize = true;
            this.lblTargetLevel.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTargetLevel.Location = new System.Drawing.Point(6, 16);
            this.lblTargetLevel.Name = "lblTargetLevel";
            this.lblTargetLevel.Size = new System.Drawing.Size(74, 14);
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
            // rbBattle
            // 
            this.rbBattle.AutoSize = true;
            this.rbBattle.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbBattle.Location = new System.Drawing.Point(7, 47);
            this.rbBattle.Name = "rbBattle";
            this.rbBattle.Size = new System.Drawing.Size(97, 18);
            this.rbBattle.TabIndex = 2;
            this.rbBattle.TabStop = true;
            this.rbBattle.Text = "Battle Stance";
            this.rbBattle.UseVisualStyleBackColor = true;
            // 
            // groupBoxStanceChoice
            // 
            this.groupBoxStanceChoice.Controls.Add(this.rbBerserker);
            this.groupBoxStanceChoice.Controls.Add(this.rbBattle);
            this.groupBoxStanceChoice.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxStanceChoice.Location = new System.Drawing.Point(8, 15);
            this.groupBoxStanceChoice.Name = "groupBoxStanceChoice";
            this.groupBoxStanceChoice.Size = new System.Drawing.Size(185, 73);
            this.groupBoxStanceChoice.TabIndex = 3;
            this.groupBoxStanceChoice.TabStop = false;
            this.groupBoxStanceChoice.Text = "Stance";
            // 
            // rbBerserker
            // 
            this.rbBerserker.AutoSize = true;
            this.rbBerserker.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbBerserker.Location = new System.Drawing.Point(7, 19);
            this.rbBerserker.Name = "rbBerserker";
            this.rbBerserker.Size = new System.Drawing.Size(116, 18);
            this.rbBerserker.TabIndex = 2;
            this.rbBerserker.TabStop = true;
            this.rbBerserker.Text = "Berserker Stance";
            this.rbBerserker.UseVisualStyleBackColor = true;
            this.rbBerserker.CheckedChanged += new System.EventHandler(this.rbBerserker_CheckedChanged);
            // 
            // groupBoxSkillUsage
            // 
            this.groupBoxSkillUsage.Controls.Add(this.flurryCheck);
            this.groupBoxSkillUsage.Controls.Add(this.txtslamLatency);
            this.groupBoxSkillUsage.Controls.Add(this.slamLatLabel);
            this.groupBoxSkillUsage.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxSkillUsage.Location = new System.Drawing.Point(8, 103);
            this.groupBoxSkillUsage.Name = "groupBoxSkillUsage";
            this.groupBoxSkillUsage.Size = new System.Drawing.Size(185, 89);
            this.groupBoxSkillUsage.TabIndex = 4;
            this.groupBoxSkillUsage.TabStop = false;
            this.groupBoxSkillUsage.Text = "Other Info";
            // 
            // txtslamLatency
            // 
            this.txtslamLatency.Location = new System.Drawing.Point(87, 15);
            this.txtslamLatency.Name = "txtslamLatency";
            this.txtslamLatency.Size = new System.Drawing.Size(72, 22);
            this.txtslamLatency.TabIndex = 5;
            this.txtslamLatency.Text = "0.3";
            this.txtslamLatency.TextChanged += new System.EventHandler(this.txtslamLatency_TextChanged);
            // 
            // slamLatLabel
            // 
            this.slamLatLabel.AutoSize = true;
            this.slamLatLabel.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.slamLatLabel.Location = new System.Drawing.Point(5, 18);
            this.slamLatLabel.Name = "slamLatLabel";
            this.slamLatLabel.Size = new System.Drawing.Size(80, 14);
            this.slamLatLabel.TabIndex = 4;
            this.slamLatLabel.Text = "Slam Latency:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblLength);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label1);
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
            // lblLength
            // 
            this.lblLength.AutoSize = true;
            this.lblLength.Location = new System.Drawing.Point(76, 48);
            this.lblLength.Name = "lblLength";
            this.lblLength.Size = new System.Drawing.Size(19, 14);
            this.lblLength.TabIndex = 2;
            this.lblLength.Text = "10";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.SystemColors.Control;
            this.label3.Location = new System.Drawing.Point(154, 72);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(19, 14);
            this.label3.TabIndex = 2;
            this.label3.Text = "10";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(94, 70);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(13, 14);
            this.label1.TabIndex = 4;
            this.label1.Text = "0";
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
            // trackBarFightLength
            // 
            this.trackBarFightLength.Location = new System.Drawing.Point(95, 41);
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
            this.label2.Size = new System.Drawing.Size(78, 14);
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
            // btnTalents
            // 
            this.btnTalents.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTalents.Location = new System.Drawing.Point(8, 480);
            this.btnTalents.Name = "btnTalents";
            this.btnTalents.Size = new System.Drawing.Size(75, 23);
            this.btnTalents.TabIndex = 5;
            this.btnTalents.Text = "Talents";
            this.btnTalents.UseVisualStyleBackColor = true;
            this.btnTalents.Click += new System.EventHandler(this.btnTalents_Click);
            // 
            // btnGraph
            // 
            this.btnGraph.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGraph.Location = new System.Drawing.Point(118, 480);
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
            this.checkBoxMeta.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxMeta.Location = new System.Drawing.Point(8, 436);
            this.checkBoxMeta.Name = "checkBoxMeta";
            this.checkBoxMeta.Size = new System.Drawing.Size(177, 18);
            this.checkBoxMeta.TabIndex = 6;
            this.checkBoxMeta.Text = "Enforce Meta Requirements";
            this.checkBoxMeta.UseVisualStyleBackColor = true;
            this.checkBoxMeta.CheckedChanged += new System.EventHandler(this.checkBoxMeta_CheckedChanged);
            // 
            // radioButtonAldor
            // 
            this.radioButtonAldor.AutoSize = true;
            this.radioButtonAldor.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButtonAldor.Location = new System.Drawing.Point(12, 19);
            this.radioButtonAldor.Name = "radioButtonAldor";
            this.radioButtonAldor.Size = new System.Drawing.Size(54, 18);
            this.radioButtonAldor.TabIndex = 7;
            this.radioButtonAldor.TabStop = true;
            this.radioButtonAldor.Text = "Aldor";
            this.radioButtonAldor.UseVisualStyleBackColor = true;
            this.radioButtonAldor.CheckedChanged += new System.EventHandler(this.radioButtonAldor_CheckedChanged);
            // 
            // radioButtonScryer
            // 
            this.radioButtonScryer.AutoSize = true;
            this.radioButtonScryer.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButtonScryer.Location = new System.Drawing.Point(88, 19);
            this.radioButtonScryer.Name = "radioButtonScryer";
            this.radioButtonScryer.Size = new System.Drawing.Size(56, 18);
            this.radioButtonScryer.TabIndex = 7;
            this.radioButtonScryer.TabStop = true;
            this.radioButtonScryer.Text = "Scryer";
            this.radioButtonScryer.UseVisualStyleBackColor = true;
            this.radioButtonScryer.CheckedChanged += new System.EventHandler(this.radioButtonScryer_CheckedChanged);
            // 
            // Faction
            // 
            this.Faction.Controls.Add(this.radioButtonAldor);
            this.Faction.Controls.Add(this.radioButtonScryer);
            this.Faction.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Faction.Location = new System.Drawing.Point(8, 359);
            this.Faction.Name = "Faction";
            this.Faction.Size = new System.Drawing.Size(188, 52);
            this.Faction.TabIndex = 8;
            this.Faction.TabStop = false;
            this.Faction.Text = "Faction";
            // 
            // flurryCheck
            // 
            this.flurryCheck.AutoSize = true;
            this.flurryCheck.Checked = true;
            this.flurryCheck.CheckState = System.Windows.Forms.CheckState.Checked;
            this.flurryCheck.Location = new System.Drawing.Point(7, 49);
            this.flurryCheck.Name = "flurryCheck";
            this.flurryCheck.Size = new System.Drawing.Size(128, 18);
            this.flurryCheck.TabIndex = 6;
            this.flurryCheck.Text = "Assume 100% flurry";
            this.flurryCheck.UseVisualStyleBackColor = true;
            this.flurryCheck.CheckedChanged += new System.EventHandler(this.flurryCheck_CheckedChanged);
            // 
            // CalculationOptionsPanelDPSWarr
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.Faction);
            this.Controls.Add(this.checkBoxMeta);
            this.Controls.Add(this.btnGraph);
            this.Controls.Add(this.btnTalents);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBoxSkillUsage);
            this.Controls.Add(this.groupBoxStanceChoice);
            this.Name = "CalculationOptionsPanelDPSWarr";
            this.Size = new System.Drawing.Size(196, 508);
            this.groupBoxStanceChoice.ResumeLayout(false);
            this.groupBoxStanceChoice.PerformLayout();
            this.groupBoxSkillUsage.ResumeLayout(false);
            this.groupBoxSkillUsage.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarFightLength)).EndInit();
            this.Faction.ResumeLayout(false);
            this.Faction.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTargetLevel;
        private System.Windows.Forms.ComboBox comboBoxTargetLevel;
        private System.Windows.Forms.RadioButton rbBattle;
        private System.Windows.Forms.GroupBox groupBoxStanceChoice;
        private System.Windows.Forms.GroupBox groupBoxSkillUsage;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TrackBar trackBarFightLength;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblFightLength;
        private System.Windows.Forms.TextBox txtArmor;
        private System.Windows.Forms.Button btnTalents;
        private System.Windows.Forms.Button btnGraph;
        private System.Windows.Forms.Label lblLength;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkBoxMeta;
        private System.Windows.Forms.RadioButton radioButtonAldor;
        private System.Windows.Forms.RadioButton radioButtonScryer;
        private System.Windows.Forms.GroupBox Faction;
        private System.Windows.Forms.RadioButton rbBerserker;
        private System.Windows.Forms.TextBox txtslamLatency;
        private System.Windows.Forms.Label slamLatLabel;
        private System.Windows.Forms.CheckBox flurryCheck;
    }
}