namespace Rawr.Warlock
{
    partial class RaidISB
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
            this.groupBoxWarlocks = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxNumWarlocks = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.comboBoxNumShadowPriests = new System.Windows.Forms.ComboBox();
            this.groupBoxShadowPriests = new System.Windows.Forms.GroupBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.suShadowPriest5 = new Rawr.Warlock.ShadowPriestControl();
            this.suShadowPriest4 = new Rawr.Warlock.ShadowPriestControl();
            this.suShadowPriest3 = new Rawr.Warlock.ShadowPriestControl();
            this.suShadowPriest2 = new Rawr.Warlock.ShadowPriestControl();
            this.suShadowPriest1 = new Rawr.Warlock.ShadowPriestControl();
            this.suWarlock5 = new Rawr.Warlock.WarlockControl();
            this.suWarlock4 = new Rawr.Warlock.WarlockControl();
            this.suWarlock3 = new Rawr.Warlock.WarlockControl();
            this.suWarlock2 = new Rawr.Warlock.WarlockControl();
            this.suWarlock1 = new Rawr.Warlock.WarlockControl();
            this.groupBoxWarlocks.SuspendLayout();
            this.groupBoxShadowPriests.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxWarlocks
            // 
            this.groupBoxWarlocks.Controls.Add(this.suWarlock5);
            this.groupBoxWarlocks.Controls.Add(this.suWarlock4);
            this.groupBoxWarlocks.Controls.Add(this.suWarlock3);
            this.groupBoxWarlocks.Controls.Add(this.suWarlock2);
            this.groupBoxWarlocks.Controls.Add(this.suWarlock1);
            this.groupBoxWarlocks.Controls.Add(this.label5);
            this.groupBoxWarlocks.Controls.Add(this.label4);
            this.groupBoxWarlocks.Controls.Add(this.label3);
            this.groupBoxWarlocks.Controls.Add(this.label2);
            this.groupBoxWarlocks.Controls.Add(this.label1);
            this.groupBoxWarlocks.Location = new System.Drawing.Point(12, 39);
            this.groupBoxWarlocks.Name = "groupBoxWarlocks";
            this.groupBoxWarlocks.Padding = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.groupBoxWarlocks.Size = new System.Drawing.Size(665, 151);
            this.groupBoxWarlocks.TabIndex = 2;
            this.groupBoxWarlocks.TabStop = false;
            this.groupBoxWarlocks.Text = "Warlocks";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(25, 126);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(74, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Shadow DPS:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(23, 100);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(76, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "SB Cast Ratio:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(25, 74);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(74, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "SB Cast Time:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(63, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(36, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Crit %:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(63, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Hit %:";
            // 
            // comboBoxNumWarlocks
            // 
            this.comboBoxNumWarlocks.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxNumWarlocks.FormattingEnabled = true;
            this.comboBoxNumWarlocks.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "3",
            "4",
            "5"});
            this.comboBoxNumWarlocks.Location = new System.Drawing.Point(125, 12);
            this.comboBoxNumWarlocks.Name = "comboBoxNumWarlocks";
            this.comboBoxNumWarlocks.Size = new System.Drawing.Size(47, 21);
            this.comboBoxNumWarlocks.TabIndex = 1;
            this.comboBoxNumWarlocks.SelectedIndexChanged += new System.EventHandler(this.comboBoxNumWarlocks_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 15);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(107, 13);
            this.label6.TabIndex = 4;
            this.label6.Text = "Number of Warlocks:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 199);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(135, 13);
            this.label7.TabIndex = 6;
            this.label7.Text = "Number of Shadow Priests:";
            // 
            // comboBoxNumShadowPriests
            // 
            this.comboBoxNumShadowPriests.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxNumShadowPriests.FormattingEnabled = true;
            this.comboBoxNumShadowPriests.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "3",
            "4",
            "5"});
            this.comboBoxNumShadowPriests.Location = new System.Drawing.Point(153, 196);
            this.comboBoxNumShadowPriests.Name = "comboBoxNumShadowPriests";
            this.comboBoxNumShadowPriests.Size = new System.Drawing.Size(47, 21);
            this.comboBoxNumShadowPriests.TabIndex = 7;
            this.comboBoxNumShadowPriests.SelectedIndexChanged += new System.EventHandler(this.comboBoxNumShadowPriests_SelectedIndexChanged);
            // 
            // groupBoxShadowPriests
            // 
            this.groupBoxShadowPriests.Controls.Add(this.suShadowPriest5);
            this.groupBoxShadowPriests.Controls.Add(this.suShadowPriest4);
            this.groupBoxShadowPriests.Controls.Add(this.suShadowPriest3);
            this.groupBoxShadowPriests.Controls.Add(this.suShadowPriest2);
            this.groupBoxShadowPriests.Controls.Add(this.suShadowPriest1);
            this.groupBoxShadowPriests.Controls.Add(this.label10);
            this.groupBoxShadowPriests.Controls.Add(this.label11);
            this.groupBoxShadowPriests.Controls.Add(this.label12);
            this.groupBoxShadowPriests.Location = new System.Drawing.Point(12, 223);
            this.groupBoxShadowPriests.Name = "groupBoxShadowPriests";
            this.groupBoxShadowPriests.Padding = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.groupBoxShadowPriests.Size = new System.Drawing.Size(665, 100);
            this.groupBoxShadowPriests.TabIndex = 15;
            this.groupBoxShadowPriests.TabStop = false;
            this.groupBoxShadowPriests.Text = "Warlocks";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(25, 74);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(74, 13);
            this.label10.TabIndex = 7;
            this.label10.Text = "Shadow DPS:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(6, 48);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(93, 13);
            this.label11.TabIndex = 6;
            this.label11.Text = "MB Frequency (s):";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(65, 22);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(34, 13);
            this.label12.TabIndex = 5;
            this.label12.Text = "Hit %:";
            // 
            // okButton
            // 
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Location = new System.Drawing.Point(521, 329);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 13;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(602, 329);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 14;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(178, 15);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(144, 13);
            this.label8.TabIndex = 18;
            this.label8.Text = "Note: Do not include yourself";
            // 
            // suShadowPriest5
            // 
            this.suShadowPriest5.HitPercent = 16F;
            this.suShadowPriest5.Location = new System.Drawing.Point(553, 16);
            this.suShadowPriest5.MbFrequency = 7.5F;
            this.suShadowPriest5.Name = "suShadowPriest5";
            this.suShadowPriest5.ShadowDps = 1100F;
            this.suShadowPriest5.Size = new System.Drawing.Size(106, 78);
            this.suShadowPriest5.TabIndex = 12;
            // 
            // suShadowPriest4
            // 
            this.suShadowPriest4.HitPercent = 16F;
            this.suShadowPriest4.Location = new System.Drawing.Point(441, 16);
            this.suShadowPriest4.MbFrequency = 7.5F;
            this.suShadowPriest4.Name = "suShadowPriest4";
            this.suShadowPriest4.ShadowDps = 1100F;
            this.suShadowPriest4.Size = new System.Drawing.Size(106, 78);
            this.suShadowPriest4.TabIndex = 11;
            // 
            // suShadowPriest3
            // 
            this.suShadowPriest3.HitPercent = 16F;
            this.suShadowPriest3.Location = new System.Drawing.Point(329, 16);
            this.suShadowPriest3.MbFrequency = 7.5F;
            this.suShadowPriest3.Name = "suShadowPriest3";
            this.suShadowPriest3.ShadowDps = 1100F;
            this.suShadowPriest3.Size = new System.Drawing.Size(106, 78);
            this.suShadowPriest3.TabIndex = 10;
            // 
            // suShadowPriest2
            // 
            this.suShadowPriest2.HitPercent = 16F;
            this.suShadowPriest2.Location = new System.Drawing.Point(217, 16);
            this.suShadowPriest2.MbFrequency = 7.5F;
            this.suShadowPriest2.Name = "suShadowPriest2";
            this.suShadowPriest2.ShadowDps = 1100F;
            this.suShadowPriest2.Size = new System.Drawing.Size(106, 78);
            this.suShadowPriest2.TabIndex = 9;
            // 
            // suShadowPriest1
            // 
            this.suShadowPriest1.HitPercent = 16F;
            this.suShadowPriest1.Location = new System.Drawing.Point(105, 16);
            this.suShadowPriest1.MbFrequency = 7.5F;
            this.suShadowPriest1.Name = "suShadowPriest1";
            this.suShadowPriest1.ShadowDps = 1100F;
            this.suShadowPriest1.Size = new System.Drawing.Size(106, 78);
            this.suShadowPriest1.TabIndex = 8;
            // 
            // suWarlock5
            // 
            this.suWarlock5.CritPercent = 20F;
            this.suWarlock5.HitPercent = 16F;
            this.suWarlock5.Location = new System.Drawing.Point(553, 16);
            this.suWarlock5.Name = "suWarlock5";
            this.suWarlock5.SbCastRatio = 0.95F;
            this.suWarlock5.SbCastTime = 2.6F;
            this.suWarlock5.ShadowDps = 1600F;
            this.suWarlock5.Size = new System.Drawing.Size(106, 130);
            this.suWarlock5.TabIndex = 6;
            // 
            // suWarlock4
            // 
            this.suWarlock4.CritPercent = 20F;
            this.suWarlock4.HitPercent = 16F;
            this.suWarlock4.Location = new System.Drawing.Point(441, 16);
            this.suWarlock4.Name = "suWarlock4";
            this.suWarlock4.SbCastRatio = 0.95F;
            this.suWarlock4.SbCastTime = 2.6F;
            this.suWarlock4.ShadowDps = 1600F;
            this.suWarlock4.Size = new System.Drawing.Size(106, 130);
            this.suWarlock4.TabIndex = 5;
            // 
            // suWarlock3
            // 
            this.suWarlock3.CritPercent = 20F;
            this.suWarlock3.HitPercent = 16F;
            this.suWarlock3.Location = new System.Drawing.Point(329, 16);
            this.suWarlock3.Name = "suWarlock3";
            this.suWarlock3.SbCastRatio = 0.95F;
            this.suWarlock3.SbCastTime = 2.6F;
            this.suWarlock3.ShadowDps = 1600F;
            this.suWarlock3.Size = new System.Drawing.Size(106, 130);
            this.suWarlock3.TabIndex = 4;
            // 
            // suWarlock2
            // 
            this.suWarlock2.CritPercent = 20F;
            this.suWarlock2.HitPercent = 16F;
            this.suWarlock2.Location = new System.Drawing.Point(217, 16);
            this.suWarlock2.Name = "suWarlock2";
            this.suWarlock2.SbCastRatio = 0.95F;
            this.suWarlock2.SbCastTime = 2.6F;
            this.suWarlock2.ShadowDps = 1600F;
            this.suWarlock2.Size = new System.Drawing.Size(106, 130);
            this.suWarlock2.TabIndex = 3;
            // 
            // suWarlock1
            // 
            this.suWarlock1.CritPercent = 20F;
            this.suWarlock1.HitPercent = 16F;
            this.suWarlock1.Location = new System.Drawing.Point(105, 16);
            this.suWarlock1.Name = "suWarlock1";
            this.suWarlock1.SbCastRatio = 0.95F;
            this.suWarlock1.SbCastTime = 2.6F;
            this.suWarlock1.ShadowDps = 1600F;
            this.suWarlock1.Size = new System.Drawing.Size(106, 130);
            this.suWarlock1.TabIndex = 2;
            // 
            // RaidISB
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(689, 364);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.groupBoxShadowPriests);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.comboBoxNumShadowPriests);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.comboBoxNumWarlocks);
            this.Controls.Add(this.groupBoxWarlocks);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RaidISB";
            this.Text = "Shadow Users";
            this.groupBoxWarlocks.ResumeLayout(false);
            this.groupBoxWarlocks.PerformLayout();
            this.groupBoxShadowPriests.ResumeLayout(false);
            this.groupBoxShadowPriests.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxWarlocks;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private WarlockControl suWarlock1;
        private WarlockControl suWarlock5;
        private WarlockControl suWarlock4;
        private WarlockControl suWarlock3;
        private WarlockControl suWarlock2;
        private System.Windows.Forms.ComboBox comboBoxNumWarlocks;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox comboBoxNumShadowPriests;
        private System.Windows.Forms.GroupBox groupBoxShadowPriests;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private ShadowPriestControl suShadowPriest1;
        private ShadowPriestControl suShadowPriest5;
        private ShadowPriestControl suShadowPriest4;
        private ShadowPriestControl suShadowPriest3;
        private ShadowPriestControl suShadowPriest2;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Label label8;
    }
}