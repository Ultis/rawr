namespace Rawr.Tree
{
    partial class CalculationOptionsPanelTree
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
            this.cbLevel = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cbSchattrathFaction = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.chbLivingSeed = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tbSurvHAbove = new System.Windows.Forms.TextBox();
            this.tbSurvTargetH = new System.Windows.Forms.TextBox();
            this.tbSurvHBelow = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.chbReplenishment = new System.Windows.Forms.CheckBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.tbAverageHealingUsage = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.cbSpellRotation = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cbLevel
            // 
            this.cbLevel.FormattingEnabled = true;
            this.cbLevel.Items.AddRange(new object[] {
            "70",
            "80"});
            this.cbLevel.Location = new System.Drawing.Point(201, 18);
            this.cbLevel.Name = "cbLevel";
            this.cbLevel.Size = new System.Drawing.Size(83, 21);
            this.cbLevel.TabIndex = 0;
            this.cbLevel.SelectedIndexChanged += new System.EventHandler(this.cbLevel_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Character Level";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(21, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(94, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Schattrath Faction";
            // 
            // cbSchattrathFaction
            // 
            this.cbSchattrathFaction.FormattingEnabled = true;
            this.cbSchattrathFaction.Items.AddRange(new object[] {
            "None",
            "Aldor",
            "Scryers"});
            this.cbSchattrathFaction.Location = new System.Drawing.Point(201, 60);
            this.cbSchattrathFaction.Name = "cbSchattrathFaction";
            this.cbSchattrathFaction.Size = new System.Drawing.Size(83, 21);
            this.cbSchattrathFaction.TabIndex = 3;
            this.cbSchattrathFaction.SelectedIndexChanged += new System.EventHandler(this.cbSchattrathFaction_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(21, 105);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(157, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Use Living Seed as Crit Modifier";
            // 
            // chbLivingSeed
            // 
            this.chbLivingSeed.AutoSize = true;
            this.chbLivingSeed.Location = new System.Drawing.Point(269, 105);
            this.chbLivingSeed.Name = "chbLivingSeed";
            this.chbLivingSeed.Size = new System.Drawing.Size(15, 14);
            this.chbLivingSeed.TabIndex = 5;
            this.chbLivingSeed.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(21, 147);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(72, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Target Health";
            // 
            // tbSurvHAbove
            // 
            this.tbSurvHAbove.Location = new System.Drawing.Point(224, 189);
            this.tbSurvHAbove.Name = "tbSurvHAbove";
            this.tbSurvHAbove.Size = new System.Drawing.Size(60, 20);
            this.tbSurvHAbove.TabIndex = 9;
            this.tbSurvHAbove.TextChanged += new System.EventHandler(this.tbSurvHAbove_TextChanged);
            // 
            // tbSurvTargetH
            // 
            this.tbSurvTargetH.Location = new System.Drawing.Point(104, 189);
            this.tbSurvTargetH.Name = "tbSurvTargetH";
            this.tbSurvTargetH.Size = new System.Drawing.Size(97, 20);
            this.tbSurvTargetH.TabIndex = 10;
            this.tbSurvTargetH.TextChanged += new System.EventHandler(this.tbSurvTargetH_TextChanged);
            // 
            // tbSurvHBelow
            // 
            this.tbSurvHBelow.Location = new System.Drawing.Point(21, 189);
            this.tbSurvHBelow.Name = "tbSurvHBelow";
            this.tbSurvHBelow.Size = new System.Drawing.Size(60, 20);
            this.tbSurvHBelow.TabIndex = 11;
            this.tbSurvHBelow.TextChanged += new System.EventHandler(this.tbSurvHBelow_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(86, 192);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(13, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "<";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(206, 192);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(13, 13);
            this.label6.TabIndex = 13;
            this.label6.Text = ">";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(21, 240);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(77, 13);
            this.label7.TabIndex = 14;
            this.label7.Text = "Replenishment";
            // 
            // chbReplenishment
            // 
            this.chbReplenishment.AutoSize = true;
            this.chbReplenishment.Location = new System.Drawing.Point(269, 240);
            this.chbReplenishment.Name = "chbReplenishment";
            this.chbReplenishment.Size = new System.Drawing.Size(15, 14);
            this.chbReplenishment.TabIndex = 15;
            this.chbReplenishment.UseVisualStyleBackColor = true;
            this.chbReplenishment.CheckedChanged += new System.EventHandler(this.chbReplenishment_CheckedChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(21, 290);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(171, 13);
            this.label8.TabIndex = 16;
            this.label8.Text = "Usage of Average Healing Effekts:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(269, 290);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(15, 13);
            this.label9.TabIndex = 17;
            this.label9.Text = "%";
            // 
            // tbAverageHealingUsage
            // 
            this.tbAverageHealingUsage.Location = new System.Drawing.Point(229, 287);
            this.tbAverageHealingUsage.Name = "tbAverageHealingUsage";
            this.tbAverageHealingUsage.Size = new System.Drawing.Size(34, 20);
            this.tbAverageHealingUsage.TabIndex = 18;
            this.tbAverageHealingUsage.TextChanged += new System.EventHandler(this.tbAverageHealingUsage_TextChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(21, 358);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(73, 13);
            this.label10.TabIndex = 19;
            this.label10.Text = "Spell-Rotation";
            // 
            // cbSpellRotation
            // 
            this.cbSpellRotation.FormattingEnabled = true;
            this.cbSpellRotation.Items.AddRange(new object[] {
            "Healing Touch",
            "Regrowth"});
            this.cbSpellRotation.Location = new System.Drawing.Point(184, 355);
            this.cbSpellRotation.Name = "cbSpellRotation";
            this.cbSpellRotation.Size = new System.Drawing.Size(100, 21);
            this.cbSpellRotation.TabIndex = 20;
            this.cbSpellRotation.SelectedIndexChanged += new System.EventHandler(this.cbSpellRotation_SelectedIndexChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(40, 371);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(109, 13);
            this.label11.TabIndex = 21;
            this.label11.Text = "* still in developement";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(40, 253);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(146, 13);
            this.label12.TabIndex = 22;
            this.label12.Text = "* 0,25% Manareg per Second";
            // 
            // CalculationOptionsPanelTree
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.cbSpellRotation);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.tbAverageHealingUsage);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.chbReplenishment);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.tbSurvHBelow);
            this.Controls.Add(this.tbSurvTargetH);
            this.Controls.Add(this.tbSurvHAbove);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.chbLivingSeed);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cbSchattrathFaction);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbLevel);
            this.Name = "CalculationOptionsPanelTree";
            this.Size = new System.Drawing.Size(303, 582);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cbLevel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbSchattrathFaction;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox chbLivingSeed;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbSurvHAbove;
        private System.Windows.Forms.TextBox tbSurvTargetH;
        private System.Windows.Forms.TextBox tbSurvHBelow;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox chbReplenishment;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox tbAverageHealingUsage;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox cbSpellRotation;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
    }
}