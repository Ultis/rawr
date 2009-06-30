namespace Rawr.TankDK
{
    partial class CalculationOptionsPanelTankDK
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.cmbAttackerLevel = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.numThreatWeight = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.numSurvivalWeight = new System.Windows.Forms.NumericUpDown();
            this.numIncomingDamage = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.numBossAttackSpeed = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.numPercIncFromMagic = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.nudTargetArmor = new System.Windows.Forms.NumericUpDown();
            this.lblTargetArmor = new System.Windows.Forms.Label();
            this.tbFightLength = new System.Windows.Forms.TrackBar();
            this.btnRotation = new System.Windows.Forms.Button();
            this.gbFightInfo = new System.Windows.Forms.GroupBox();
            this.numTargets = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.lblFightLengthNum = new System.Windows.Forms.Label();
            this.lblFightLength = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numThreatWeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSurvivalWeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numIncomingDamage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numBossAttackSpeed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPercIncFromMagic)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTargetArmor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbFightLength)).BeginInit();
            this.gbFightInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTargets)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 70);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Attacker Level";
            // 
            // cmbAttackerLevel
            // 
            this.cmbAttackerLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAttackerLevel.FormattingEnabled = true;
            this.cmbAttackerLevel.Items.AddRange(new object[] {
            "80",
            "81",
            "82",
            "83"});
            this.cmbAttackerLevel.Location = new System.Drawing.Point(150, 70);
            this.cmbAttackerLevel.Name = "cmbAttackerLevel";
            this.cmbAttackerLevel.Size = new System.Drawing.Size(105, 21);
            this.cmbAttackerLevel.TabIndex = 5;
            this.cmbAttackerLevel.SelectedIndexChanged += new System.EventHandler(this.cmbAttackerLevel_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 4);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Threat Weight";
            // 
            // numThreatWeight
            // 
            this.numThreatWeight.DecimalPlaces = 2;
            this.numThreatWeight.Increment = new decimal(new int[] {
            5,
            0,
            0,
            131072});
            this.numThreatWeight.Location = new System.Drawing.Point(150, 4);
            this.numThreatWeight.Name = "numThreatWeight";
            this.numThreatWeight.Size = new System.Drawing.Size(105, 20);
            this.numThreatWeight.TabIndex = 1;
            this.numThreatWeight.Value = new decimal(new int[] {
            10,
            0,
            0,
            65536});
            this.numThreatWeight.ValueChanged += new System.EventHandler(this.numThreatWeight_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 28);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Survival Weight";
            // 
            // numSurvivalWeight
            // 
            this.numSurvivalWeight.DecimalPlaces = 2;
            this.numSurvivalWeight.Increment = new decimal(new int[] {
            5,
            0,
            0,
            131072});
            this.numSurvivalWeight.Location = new System.Drawing.Point(150, 28);
            this.numSurvivalWeight.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numSurvivalWeight.Name = "numSurvivalWeight";
            this.numSurvivalWeight.Size = new System.Drawing.Size(105, 20);
            this.numSurvivalWeight.TabIndex = 3;
            this.numSurvivalWeight.Value = new decimal(new int[] {
            10,
            0,
            0,
            65536});
            this.numSurvivalWeight.ValueChanged += new System.EventHandler(this.numSurvivalWeight_ValueChanged);
            // 
            // numIncomingDamage
            // 
            this.numIncomingDamage.Enabled = false;
            this.numIncomingDamage.Increment = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numIncomingDamage.Location = new System.Drawing.Point(150, 122);
            this.numIncomingDamage.Maximum = new decimal(new int[] {
            500000,
            0,
            0,
            0});
            this.numIncomingDamage.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numIncomingDamage.Name = "numIncomingDamage";
            this.numIncomingDamage.Size = new System.Drawing.Size(105, 20);
            this.numIncomingDamage.TabIndex = 9;
            this.numIncomingDamage.ThousandsSeparator = true;
            this.numIncomingDamage.Value = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.numIncomingDamage.ValueChanged += new System.EventHandler(this.numIncomingDamage_ValueChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Enabled = false;
            this.label4.Location = new System.Drawing.Point(3, 122);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(75, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Incoming DPS";
            // 
            // numBossAttackSpeed
            // 
            this.numBossAttackSpeed.DecimalPlaces = 2;
            this.numBossAttackSpeed.Enabled = false;
            this.numBossAttackSpeed.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.numBossAttackSpeed.Location = new System.Drawing.Point(150, 97);
            this.numBossAttackSpeed.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numBossAttackSpeed.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numBossAttackSpeed.Name = "numBossAttackSpeed";
            this.numBossAttackSpeed.Size = new System.Drawing.Size(105, 20);
            this.numBossAttackSpeed.TabIndex = 7;
            this.numBossAttackSpeed.Value = new decimal(new int[] {
            25,
            0,
            0,
            65536});
            this.numBossAttackSpeed.ValueChanged += new System.EventHandler(this.numBossAttackSpeed_ValueChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Enabled = false;
            this.label5.Location = new System.Drawing.Point(3, 97);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(98, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Boss Attack Speed";
            // 
            // numPercIncFromMagic
            // 
            this.numPercIncFromMagic.DecimalPlaces = 2;
            this.numPercIncFromMagic.Enabled = false;
            this.numPercIncFromMagic.Increment = new decimal(new int[] {
            5,
            0,
            0,
            131072});
            this.numPercIncFromMagic.Location = new System.Drawing.Point(150, 148);
            this.numPercIncFromMagic.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numPercIncFromMagic.Name = "numPercIncFromMagic";
            this.numPercIncFromMagic.Size = new System.Drawing.Size(105, 20);
            this.numPercIncFromMagic.TabIndex = 11;
            this.numPercIncFromMagic.ValueChanged += new System.EventHandler(this.numPercIncFromMagic_ValueChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Enabled = false;
            this.label6.Location = new System.Drawing.Point(3, 148);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(116, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "% Incoming from Magic";
            // 
            // nudTargetArmor
            // 
            this.nudTargetArmor.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.nudTargetArmor.Location = new System.Drawing.Point(135, 17);
            this.nudTargetArmor.Maximum = new decimal(new int[] {
            30000,
            0,
            0,
            0});
            this.nudTargetArmor.Name = "nudTargetArmor";
            this.nudTargetArmor.Size = new System.Drawing.Size(84, 20);
            this.nudTargetArmor.TabIndex = 13;
            this.nudTargetArmor.Value = new decimal(new int[] {
            10643,
            0,
            0,
            0});
            // 
            // lblTargetArmor
            // 
            this.lblTargetArmor.AutoSize = true;
            this.lblTargetArmor.Location = new System.Drawing.Point(6, 20);
            this.lblTargetArmor.Name = "lblTargetArmor";
            this.lblTargetArmor.Size = new System.Drawing.Size(71, 13);
            this.lblTargetArmor.TabIndex = 24;
            this.lblTargetArmor.Text = "Target Armor:";
            // 
            // tbFightLength
            // 
            this.tbFightLength.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.tbFightLength.Location = new System.Drawing.Point(153, 302);
            this.tbFightLength.Name = "tbFightLength";
            this.tbFightLength.Size = new System.Drawing.Size(86, 42);
            this.tbFightLength.TabIndex = 15;
            this.tbFightLength.Value = 10;
            this.tbFightLength.Visible = false;
            this.tbFightLength.Scroll += new System.EventHandler(this.tbFightLength_Scroll);
            // 
            // btnRotation
            // 
            this.btnRotation.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRotation.Location = new System.Drawing.Point(47, 75);
            this.btnRotation.Name = "btnRotation";
            this.btnRotation.Size = new System.Drawing.Size(125, 23);
            this.btnRotation.TabIndex = 17;
            this.btnRotation.Text = "Rotation Details";
            this.btnRotation.UseVisualStyleBackColor = true;
            this.btnRotation.Click += new System.EventHandler(this.btnRotation_Click);
            // 
            // gbFightInfo
            // 
            this.gbFightInfo.Controls.Add(this.numTargets);
            this.gbFightInfo.Controls.Add(this.btnRotation);
            this.gbFightInfo.Controls.Add(this.label7);
            this.gbFightInfo.Controls.Add(this.nudTargetArmor);
            this.gbFightInfo.Controls.Add(this.lblTargetArmor);
            this.gbFightInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbFightInfo.Location = new System.Drawing.Point(17, 176);
            this.gbFightInfo.Name = "gbFightInfo";
            this.gbFightInfo.Size = new System.Drawing.Size(227, 110);
            this.gbFightInfo.TabIndex = 37;
            this.gbFightInfo.TabStop = false;
            this.gbFightInfo.Text = "Fight Info";
            // 
            // numTargets
            // 
            this.numTargets.Location = new System.Drawing.Point(136, 44);
            this.numTargets.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numTargets.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numTargets.Name = "numTargets";
            this.numTargets.Size = new System.Drawing.Size(84, 20);
            this.numTargets.TabIndex = 15;
            this.numTargets.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numTargets.ValueChanged += new System.EventHandler(this.numTargets_ValueChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(7, 46);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 13);
            this.label7.TabIndex = 38;
            this.label7.Text = "# Targets";
            // 
            // lblFightLengthNum
            // 
            this.lblFightLengthNum.AutoSize = true;
            this.lblFightLengthNum.Location = new System.Drawing.Point(220, 334);
            this.lblFightLengthNum.Name = "lblFightLengthNum";
            this.lblFightLengthNum.Size = new System.Drawing.Size(19, 13);
            this.lblFightLengthNum.TabIndex = 2;
            this.lblFightLengthNum.Text = "10";
            this.lblFightLengthNum.Visible = false;
            // 
            // lblFightLength
            // 
            this.lblFightLength.AutoSize = true;
            this.lblFightLength.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFightLength.Location = new System.Drawing.Point(24, 302);
            this.lblFightLength.Name = "lblFightLength";
            this.lblFightLength.Size = new System.Drawing.Size(114, 13);
            this.lblFightLength.TabIndex = 0;
            this.lblFightLength.Text = "Fight Length (minutes):";
            this.lblFightLength.Visible = false;
            // 
            // CalculationOptionsPanelTankDK
            // 
            this.Controls.Add(this.gbFightInfo);
            this.Controls.Add(this.numPercIncFromMagic);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.numBossAttackSpeed);
            this.Controls.Add(this.lblFightLengthNum);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.numIncomingDamage);
            this.Controls.Add(this.tbFightLength);
            this.Controls.Add(this.lblFightLength);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.numSurvivalWeight);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.numThreatWeight);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cmbAttackerLevel);
            this.Controls.Add(this.label1);
            this.Name = "CalculationOptionsPanelTankDK";
            this.Size = new System.Drawing.Size(258, 360);
            ((System.ComponentModel.ISupportInitialize)(this.numThreatWeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSurvivalWeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numIncomingDamage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numBossAttackSpeed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPercIncFromMagic)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTargetArmor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbFightLength)).EndInit();
            this.gbFightInfo.ResumeLayout(false);
            this.gbFightInfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTargets)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbAttackerLevel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numThreatWeight;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numSurvivalWeight;
        private System.Windows.Forms.NumericUpDown numIncomingDamage;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numBossAttackSpeed;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown numPercIncFromMagic;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown nudTargetArmor;
        private System.Windows.Forms.Label lblTargetArmor;
        private System.Windows.Forms.TrackBar tbFightLength;
        private System.Windows.Forms.Button btnRotation;
        private System.Windows.Forms.GroupBox gbFightInfo;
        private System.Windows.Forms.Label lblFightLengthNum;
        private System.Windows.Forms.Label lblFightLength;
        private System.Windows.Forms.NumericUpDown numTargets;
        private System.Windows.Forms.Label label7;
    }
}
