namespace Rawr
{
	partial class CalculationOptionsPanelCat
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxTargetLevel = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.trackBarTargetArmor = new System.Windows.Forms.TrackBar();
            this.labelTargetArmorDescription = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.trackBarExposeWeakness = new System.Windows.Forms.TrackBar();
            this.labelExposeWeakness = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.trackBarBloodlustUptime = new System.Windows.Forms.TrackBar();
            this.labelBloodlustUptime = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.trackBarDrumsOfBattleUptime = new System.Windows.Forms.TrackBar();
            this.labelDrumsOfBattleUptime = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.trackBarDrumsOfWarUptime = new System.Windows.Forms.TrackBar();
            this.labelDrumsOfWarUptime = new System.Windows.Forms.Label();
            this.radioButtonAldor = new System.Windows.Forms.RadioButton();
            this.radioButtonScryer = new System.Windows.Forms.RadioButton();
            this.label6 = new System.Windows.Forms.Label();
            this.trackBarNumberOfFerociousInspirations = new System.Windows.Forms.TrackBar();
            this.labelNumberOfFerociousInspirations = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.comboBoxMainhandImbue = new System.Windows.Forms.ComboBox();
            this.comboBoxOffhandImbue = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarTargetArmor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarExposeWeakness)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarBloodlustUptime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarDrumsOfBattleUptime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarDrumsOfWarUptime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarNumberOfFerociousInspirations)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Target Level: ";
            // 
            // comboBoxTargetLevel
            // 
            this.comboBoxTargetLevel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxTargetLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxTargetLevel.Enabled = false;
            this.comboBoxTargetLevel.FormattingEnabled = true;
            this.comboBoxTargetLevel.Items.AddRange(new object[] {
            "70",
            "71",
            "72",
            "73"});
            this.comboBoxTargetLevel.Location = new System.Drawing.Point(83, 3);
            this.comboBoxTargetLevel.Name = "comboBoxTargetLevel";
            this.comboBoxTargetLevel.Size = new System.Drawing.Size(123, 21);
            this.comboBoxTargetLevel.TabIndex = 1;
            this.comboBoxTargetLevel.SelectedIndexChanged += new System.EventHandler(this.calculationOptionControl_Changed);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 288);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Target Armor: ";
            // 
            // trackBarTargetArmor
            // 
            this.trackBarTargetArmor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.trackBarTargetArmor.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.trackBarTargetArmor.LargeChange = 1000;
            this.trackBarTargetArmor.Location = new System.Drawing.Point(75, 285);
            this.trackBarTargetArmor.Maximum = 9000;
            this.trackBarTargetArmor.Minimum = 3000;
            this.trackBarTargetArmor.Name = "trackBarTargetArmor";
            this.trackBarTargetArmor.Size = new System.Drawing.Size(139, 45);
            this.trackBarTargetArmor.SmallChange = 100;
            this.trackBarTargetArmor.TabIndex = 2;
            this.trackBarTargetArmor.TickFrequency = 300;
            this.trackBarTargetArmor.Value = 7700;
            this.trackBarTargetArmor.ValueChanged += new System.EventHandler(this.calculationOptionControl_Changed);
            // 
            // labelTargetArmorDescription
            // 
            this.labelTargetArmorDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.labelTargetArmorDescription.Location = new System.Drawing.Point(6, 333);
            this.labelTargetArmorDescription.Name = "labelTargetArmorDescription";
            this.labelTargetArmorDescription.Size = new System.Drawing.Size(200, 40);
            this.labelTargetArmorDescription.TabIndex = 0;
            this.labelTargetArmorDescription.Text = "7700 Armor: Hydross, Lurker, Leotheras, Tidewalker, Al\'ar, Naj\'entus, Supremus, A" +
                "kama, Gurtogg";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(16, 33);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(58, 39);
            this.label4.TabIndex = 0;
            this.label4.Text = "Expose\r\nWeakness\r\nAP Bonus:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // trackBarExposeWeakness
            // 
            this.trackBarExposeWeakness.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.trackBarExposeWeakness.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.trackBarExposeWeakness.LargeChange = 50;
            this.trackBarExposeWeakness.Location = new System.Drawing.Point(75, 30);
            this.trackBarExposeWeakness.Maximum = 500;
            this.trackBarExposeWeakness.Minimum = 50;
            this.trackBarExposeWeakness.Name = "trackBarExposeWeakness";
            this.trackBarExposeWeakness.Size = new System.Drawing.Size(139, 45);
            this.trackBarExposeWeakness.SmallChange = 10;
            this.trackBarExposeWeakness.TabIndex = 2;
            this.trackBarExposeWeakness.TickFrequency = 25;
            this.trackBarExposeWeakness.Value = 200;
            this.trackBarExposeWeakness.ValueChanged += new System.EventHandler(this.calculationOptionControl_Changed);
            // 
            // labelExposeWeakness
            // 
            this.labelExposeWeakness.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.labelExposeWeakness.AutoSize = true;
            this.labelExposeWeakness.Location = new System.Drawing.Point(80, 62);
            this.labelExposeWeakness.Name = "labelExposeWeakness";
            this.labelExposeWeakness.Size = new System.Drawing.Size(25, 13);
            this.labelExposeWeakness.TabIndex = 0;
            this.labelExposeWeakness.Text = "200";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(16, 84);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(54, 26);
            this.label5.TabIndex = 0;
            this.label5.Text = "Bloodlust\r\nUptime %:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // trackBarBloodlustUptime
            // 
            this.trackBarBloodlustUptime.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.trackBarBloodlustUptime.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.trackBarBloodlustUptime.Location = new System.Drawing.Point(75, 81);
            this.trackBarBloodlustUptime.Maximum = 100;
            this.trackBarBloodlustUptime.Minimum = 5;
            this.trackBarBloodlustUptime.Name = "trackBarBloodlustUptime";
            this.trackBarBloodlustUptime.Size = new System.Drawing.Size(139, 45);
            this.trackBarBloodlustUptime.TabIndex = 2;
            this.trackBarBloodlustUptime.TickFrequency = 5;
            this.trackBarBloodlustUptime.Value = 15;
            this.trackBarBloodlustUptime.ValueChanged += new System.EventHandler(this.calculationOptionControl_Changed);
            // 
            // labelBloodlustUptime
            // 
            this.labelBloodlustUptime.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.labelBloodlustUptime.AutoSize = true;
            this.labelBloodlustUptime.Location = new System.Drawing.Point(80, 113);
            this.labelBloodlustUptime.Name = "labelBloodlustUptime";
            this.labelBloodlustUptime.Size = new System.Drawing.Size(27, 13);
            this.labelBloodlustUptime.TabIndex = 0;
            this.labelBloodlustUptime.Text = "15%";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(16, 135);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(54, 39);
            this.label7.TabIndex = 0;
            this.label7.Text = "Drums\r\nof Battle\r\nUptime %:";
            this.label7.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // trackBarDrumsOfBattleUptime
            // 
            this.trackBarDrumsOfBattleUptime.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.trackBarDrumsOfBattleUptime.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.trackBarDrumsOfBattleUptime.Location = new System.Drawing.Point(75, 132);
            this.trackBarDrumsOfBattleUptime.Maximum = 100;
            this.trackBarDrumsOfBattleUptime.Minimum = 20;
            this.trackBarDrumsOfBattleUptime.Name = "trackBarDrumsOfBattleUptime";
            this.trackBarDrumsOfBattleUptime.Size = new System.Drawing.Size(139, 45);
            this.trackBarDrumsOfBattleUptime.TabIndex = 2;
            this.trackBarDrumsOfBattleUptime.TickFrequency = 5;
            this.trackBarDrumsOfBattleUptime.Value = 25;
            this.trackBarDrumsOfBattleUptime.ValueChanged += new System.EventHandler(this.calculationOptionControl_Changed);
            // 
            // labelDrumsOfBattleUptime
            // 
            this.labelDrumsOfBattleUptime.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.labelDrumsOfBattleUptime.AutoSize = true;
            this.labelDrumsOfBattleUptime.Location = new System.Drawing.Point(80, 164);
            this.labelDrumsOfBattleUptime.Name = "labelDrumsOfBattleUptime";
            this.labelDrumsOfBattleUptime.Size = new System.Drawing.Size(27, 13);
            this.labelDrumsOfBattleUptime.TabIndex = 0;
            this.labelDrumsOfBattleUptime.Text = "25%";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(16, 186);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(54, 39);
            this.label9.TabIndex = 0;
            this.label9.Text = "Drums\r\nof War\r\nUptime %:";
            this.label9.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // trackBarDrumsOfWarUptime
            // 
            this.trackBarDrumsOfWarUptime.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.trackBarDrumsOfWarUptime.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.trackBarDrumsOfWarUptime.Location = new System.Drawing.Point(75, 183);
            this.trackBarDrumsOfWarUptime.Maximum = 100;
            this.trackBarDrumsOfWarUptime.Minimum = 20;
            this.trackBarDrumsOfWarUptime.Name = "trackBarDrumsOfWarUptime";
            this.trackBarDrumsOfWarUptime.Size = new System.Drawing.Size(139, 45);
            this.trackBarDrumsOfWarUptime.TabIndex = 2;
            this.trackBarDrumsOfWarUptime.TickFrequency = 5;
            this.trackBarDrumsOfWarUptime.Value = 25;
            this.trackBarDrumsOfWarUptime.ValueChanged += new System.EventHandler(this.calculationOptionControl_Changed);
            // 
            // labelDrumsOfWarUptime
            // 
            this.labelDrumsOfWarUptime.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.labelDrumsOfWarUptime.AutoSize = true;
            this.labelDrumsOfWarUptime.Location = new System.Drawing.Point(80, 215);
            this.labelDrumsOfWarUptime.Name = "labelDrumsOfWarUptime";
            this.labelDrumsOfWarUptime.Size = new System.Drawing.Size(27, 13);
            this.labelDrumsOfWarUptime.TabIndex = 0;
            this.labelDrumsOfWarUptime.Text = "25%";
            // 
            // radioButtonAldor
            // 
            this.radioButtonAldor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.radioButtonAldor.AutoSize = true;
            this.radioButtonAldor.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.radioButtonAldor.Checked = true;
            this.radioButtonAldor.Location = new System.Drawing.Point(7, 376);
            this.radioButtonAldor.Name = "radioButtonAldor";
            this.radioButtonAldor.Size = new System.Drawing.Size(49, 17);
            this.radioButtonAldor.TabIndex = 3;
            this.radioButtonAldor.TabStop = true;
            this.radioButtonAldor.Tag = "Mangle";
            this.radioButtonAldor.Text = "Aldor";
            this.radioButtonAldor.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.radioButtonAldor.UseVisualStyleBackColor = true;
            this.radioButtonAldor.CheckedChanged += new System.EventHandler(this.calculationOptionControl_Changed);
            // 
            // radioButtonScryer
            // 
            this.radioButtonScryer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.radioButtonScryer.AutoSize = true;
            this.radioButtonScryer.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.radioButtonScryer.Location = new System.Drawing.Point(83, 376);
            this.radioButtonScryer.Name = "radioButtonScryer";
            this.radioButtonScryer.Size = new System.Drawing.Size(55, 17);
            this.radioButtonScryer.TabIndex = 3;
            this.radioButtonScryer.Tag = "Shred";
            this.radioButtonScryer.Text = "Scryer";
            this.radioButtonScryer.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.radioButtonScryer.UseVisualStyleBackColor = true;
            this.radioButtonScryer.CheckedChanged += new System.EventHandler(this.calculationOptionControl_Changed);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(16, 237);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(60, 39);
            this.label6.TabIndex = 0;
            this.label6.Text = "Number of\r\nFerocious\r\nInspirations";
            this.label6.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // trackBarNumberOfFerociousInspirations
            // 
            this.trackBarNumberOfFerociousInspirations.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.trackBarNumberOfFerociousInspirations.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.trackBarNumberOfFerociousInspirations.Location = new System.Drawing.Point(75, 234);
            this.trackBarNumberOfFerociousInspirations.Maximum = 4;
            this.trackBarNumberOfFerociousInspirations.Minimum = 1;
            this.trackBarNumberOfFerociousInspirations.Name = "trackBarNumberOfFerociousInspirations";
            this.trackBarNumberOfFerociousInspirations.Size = new System.Drawing.Size(139, 45);
            this.trackBarNumberOfFerociousInspirations.TabIndex = 2;
            this.trackBarNumberOfFerociousInspirations.Value = 2;
            this.trackBarNumberOfFerociousInspirations.ValueChanged += new System.EventHandler(this.calculationOptionControl_Changed);
            // 
            // labelNumberOfFerociousInspirations
            // 
            this.labelNumberOfFerociousInspirations.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.labelNumberOfFerociousInspirations.AutoSize = true;
            this.labelNumberOfFerociousInspirations.Location = new System.Drawing.Point(80, 266);
            this.labelNumberOfFerociousInspirations.Name = "labelNumberOfFerociousInspirations";
            this.labelNumberOfFerociousInspirations.Size = new System.Drawing.Size(13, 13);
            this.labelNumberOfFerociousInspirations.TabIndex = 0;
            this.labelNumberOfFerociousInspirations.Text = "2";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(4, 396);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Mainhand Imbue:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(13, 423);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(80, 13);
            this.label8.TabIndex = 5;
            this.label8.Text = "Offhand Imbue:";
            // 
            // comboBoxMainhandImbue
            // 
            this.comboBoxMainhandImbue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxMainhandImbue.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxMainhandImbue.Enabled = false;
            this.comboBoxMainhandImbue.FormattingEnabled = true;
            this.comboBoxMainhandImbue.Items.AddRange(new object[] {
            "Windfury"});
            this.comboBoxMainhandImbue.Location = new System.Drawing.Point(91, 393);
            this.comboBoxMainhandImbue.Name = "comboBoxMainhandImbue";
            this.comboBoxMainhandImbue.Size = new System.Drawing.Size(115, 21);
            this.comboBoxMainhandImbue.TabIndex = 6;
            // 
            // comboBoxOffhandImbue
            // 
            this.comboBoxOffhandImbue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxOffhandImbue.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxOffhandImbue.Enabled = false;
            this.comboBoxOffhandImbue.FormattingEnabled = true;
            this.comboBoxOffhandImbue.Items.AddRange(new object[] {
            "Flametongue"});
            this.comboBoxOffhandImbue.Location = new System.Drawing.Point(91, 420);
            this.comboBoxOffhandImbue.Name = "comboBoxOffhandImbue";
            this.comboBoxOffhandImbue.Size = new System.Drawing.Size(115, 21);
            this.comboBoxOffhandImbue.TabIndex = 7;
            // 
            // CalculationOptionsPanelCat
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.comboBoxOffhandImbue);
            this.Controls.Add(this.comboBoxMainhandImbue);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.radioButtonScryer);
            this.Controls.Add(this.radioButtonAldor);
            this.Controls.Add(this.labelNumberOfFerociousInspirations);
            this.Controls.Add(this.labelDrumsOfWarUptime);
            this.Controls.Add(this.labelDrumsOfBattleUptime);
            this.Controls.Add(this.labelBloodlustUptime);
            this.Controls.Add(this.labelExposeWeakness);
            this.Controls.Add(this.labelTargetArmorDescription);
            this.Controls.Add(this.trackBarNumberOfFerociousInspirations);
            this.Controls.Add(this.trackBarDrumsOfWarUptime);
            this.Controls.Add(this.trackBarDrumsOfBattleUptime);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.trackBarBloodlustUptime);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.trackBarExposeWeakness);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.trackBarTargetArmor);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comboBoxTargetLevel);
            this.Controls.Add(this.label1);
            this.Name = "CalculationOptionsPanelCat";
            this.Size = new System.Drawing.Size(209, 827);
            ((System.ComponentModel.ISupportInitialize)(this.trackBarTargetArmor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarExposeWeakness)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarBloodlustUptime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarDrumsOfBattleUptime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarDrumsOfWarUptime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarNumberOfFerociousInspirations)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox comboBoxTargetLevel;
        private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TrackBar trackBarTargetArmor;
        private System.Windows.Forms.Label labelTargetArmorDescription;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TrackBar trackBarExposeWeakness;
		private System.Windows.Forms.Label labelExposeWeakness;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TrackBar trackBarBloodlustUptime;
		private System.Windows.Forms.Label labelBloodlustUptime;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.TrackBar trackBarDrumsOfBattleUptime;
		private System.Windows.Forms.Label labelDrumsOfBattleUptime;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.TrackBar trackBarDrumsOfWarUptime;
		private System.Windows.Forms.Label labelDrumsOfWarUptime;
		private System.Windows.Forms.RadioButton radioButtonAldor;
		private System.Windows.Forms.RadioButton radioButtonScryer;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TrackBar trackBarNumberOfFerociousInspirations;
		private System.Windows.Forms.Label labelNumberOfFerociousInspirations;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox comboBoxMainhandImbue;
        private System.Windows.Forms.ComboBox comboBoxOffhandImbue;
	}
}
