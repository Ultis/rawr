namespace Rawr.ShadowPriest
{
    partial class CalculationOptionsPanelShadowPriest
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CalculationOptionsPanelShadowPriest));
            this.lblFightLength = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbManaTime = new System.Windows.Forms.NumericUpDown();
            this.cmbManaAmt = new System.Windows.Forms.ComboBox();
            this.gbSpellPriority = new System.Windows.Forms.GroupBox();
            this.bChangePriority = new System.Windows.Forms.Button();
            this.lsSpellPriopity = new System.Windows.Forms.ListBox();
            this.cbTargetLevel = new System.Windows.Forms.ComboBox();
            this.label15 = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.trkFightLength = new System.Windows.Forms.TrackBar();
            this.lblShadowfiend = new System.Windows.Forms.Label();
            this.trkShadowfiend = new System.Windows.Forms.TrackBar();
            this.lblReplenishment = new System.Windows.Forms.Label();
            this.trkReplenishment = new System.Windows.Forms.TrackBar();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbManaTime)).BeginInit();
            this.gbSpellPriority.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trkFightLength)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkShadowfiend)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkReplenishment)).BeginInit();
            this.SuspendLayout();
            // 
            // lblFightLength
            // 
            this.lblFightLength.AutoSize = true;
            this.lblFightLength.Location = new System.Drawing.Point(9, 36);
            this.lblFightLength.Name = "lblFightLength";
            this.lblFightLength.Size = new System.Drawing.Size(66, 13);
            this.lblFightLength.TabIndex = 21;
            this.lblFightLength.Text = "Fight Length";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.cmbManaTime);
            this.groupBox2.Controls.Add(this.cmbManaAmt);
            this.groupBox2.Location = new System.Drawing.Point(12, 263);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(270, 68);
            this.groupBox2.TabIndex = 22;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Mana Buffs";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(160, 40);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(45, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "% Mana";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(73, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(33, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "when";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 21);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(75, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Mana Potions:";
            // 
            // cmbManaTime
            // 
            this.cmbManaTime.DecimalPlaces = 1;
            this.cmbManaTime.Increment = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.cmbManaTime.Location = new System.Drawing.Point(108, 38);
            this.cmbManaTime.Maximum = new decimal(new int[] {
            90,
            0,
            0,
            0});
            this.cmbManaTime.Name = "cmbManaTime";
            this.cmbManaTime.Size = new System.Drawing.Size(50, 20);
            this.cmbManaTime.TabIndex = 1;
            this.cmbManaTime.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.cmbManaTime.ValueChanged += new System.EventHandler(this.cmbManaTime_ValueChanged);
            // 
            // cmbManaAmt
            // 
            this.cmbManaAmt.DisplayMember = "2400";
            this.cmbManaAmt.Items.AddRange(new object[] {
            "0",
            "1800",
            "2200",
            "2400"});
            this.cmbManaAmt.Location = new System.Drawing.Point(6, 37);
            this.cmbManaAmt.Name = "cmbManaAmt";
            this.cmbManaAmt.Size = new System.Drawing.Size(61, 21);
            this.cmbManaAmt.TabIndex = 0;
            this.cmbManaAmt.Text = "0";
            this.cmbManaAmt.ValueMember = "2400";
            this.cmbManaAmt.SelectedIndexChanged += new System.EventHandler(this.cmbManaAmt_SelectedIndexChanged);
            this.cmbManaAmt.TextUpdate += new System.EventHandler(this.cmbManaAmt_TextUpdate);
            // 
            // gbSpellPriority
            // 
            this.gbSpellPriority.Controls.Add(this.bChangePriority);
            this.gbSpellPriority.Controls.Add(this.lsSpellPriopity);
            this.gbSpellPriority.Location = new System.Drawing.Point(12, 337);
            this.gbSpellPriority.Name = "gbSpellPriority";
            this.gbSpellPriority.Size = new System.Drawing.Size(270, 186);
            this.gbSpellPriority.TabIndex = 29;
            this.gbSpellPriority.TabStop = false;
            this.gbSpellPriority.Text = "Spell Priority";
            // 
            // bChangePriority
            // 
            this.bChangePriority.Location = new System.Drawing.Point(5, 151);
            this.bChangePriority.Name = "bChangePriority";
            this.bChangePriority.Size = new System.Drawing.Size(75, 23);
            this.bChangePriority.TabIndex = 11;
            this.bChangePriority.Text = "Change";
            this.bChangePriority.UseVisualStyleBackColor = true;
            this.bChangePriority.Click += new System.EventHandler(this.bChangePriority_Click);
            // 
            // lsSpellPriopity
            // 
            this.lsSpellPriopity.FormattingEnabled = true;
            this.lsSpellPriopity.Location = new System.Drawing.Point(5, 20);
            this.lsSpellPriopity.Name = "lsSpellPriopity";
            this.lsSpellPriopity.Size = new System.Drawing.Size(259, 121);
            this.lsSpellPriopity.TabIndex = 10;
            // 
            // cbTargetLevel
            // 
            this.cbTargetLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbTargetLevel.FormattingEnabled = true;
            this.cbTargetLevel.Items.AddRange(new object[] {
            "+0",
            "+1",
            "+2",
            "+3 (Boss)"});
            this.cbTargetLevel.Location = new System.Drawing.Point(131, 14);
            this.cbTargetLevel.Name = "cbTargetLevel";
            this.cbTargetLevel.Size = new System.Drawing.Size(151, 21);
            this.cbTargetLevel.TabIndex = 42;
            this.cbTargetLevel.SelectedIndexChanged += new System.EventHandler(this.cbTargetLevel_SelectedIndexChanged);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(9, 14);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(99, 13);
            this.label15.TabIndex = 43;
            this.label15.Text = "Relative Mob Level";
            // 
            // trkFightLength
            // 
            this.trkFightLength.Location = new System.Drawing.Point(12, 52);
            this.trkFightLength.Maximum = 20;
            this.trkFightLength.Minimum = 1;
            this.trkFightLength.Name = "trkFightLength";
            this.trkFightLength.Size = new System.Drawing.Size(270, 42);
            this.trkFightLength.TabIndex = 44;
            this.toolTip1.SetToolTip(this.trkFightLength, "Estimated duration of the fight. Important for sustainability calculations.");
            this.trkFightLength.Value = 1;
            this.trkFightLength.Scroll += new System.EventHandler(this.trkFightLength_Scroll);
            // 
            // lblShadowfiend
            // 
            this.lblShadowfiend.AutoSize = true;
            this.lblShadowfiend.Location = new System.Drawing.Point(9, 85);
            this.lblShadowfiend.Name = "lblShadowfiend";
            this.lblShadowfiend.Size = new System.Drawing.Size(80, 13);
            this.lblShadowfiend.TabIndex = 45;
            this.lblShadowfiend.Text = "% Shadowfiend";
            // 
            // trkShadowfiend
            // 
            this.trkShadowfiend.Location = new System.Drawing.Point(12, 101);
            this.trkShadowfiend.Maximum = 100;
            this.trkShadowfiend.Name = "trkShadowfiend";
            this.trkShadowfiend.Size = new System.Drawing.Size(270, 42);
            this.trkShadowfiend.TabIndex = 46;
            this.trkShadowfiend.TickFrequency = 5;
            this.toolTip1.SetToolTip(this.trkShadowfiend, resources.GetString("trkShadowfiend.ToolTip"));
            this.trkShadowfiend.Scroll += new System.EventHandler(this.trkShadowfiend_Scroll);
            // 
            // lblReplenishment
            // 
            this.lblReplenishment.AutoSize = true;
            this.lblReplenishment.Location = new System.Drawing.Point(9, 134);
            this.lblReplenishment.Name = "lblReplenishment";
            this.lblReplenishment.Size = new System.Drawing.Size(88, 13);
            this.lblReplenishment.TabIndex = 47;
            this.lblReplenishment.Text = "% Replenishment";
            // 
            // trkReplenishment
            // 
            this.trkReplenishment.Location = new System.Drawing.Point(12, 150);
            this.trkReplenishment.Maximum = 100;
            this.trkReplenishment.Name = "trkReplenishment";
            this.trkReplenishment.Size = new System.Drawing.Size(270, 42);
            this.trkReplenishment.TabIndex = 48;
            this.trkReplenishment.TickFrequency = 5;
            this.toolTip1.SetToolTip(this.trkReplenishment, "How much uptime do you expect on Replenishment?");
            this.trkReplenishment.Scroll += new System.EventHandler(this.trkReplenishment_Scroll);
            // 
            // CalculationOptionsPanelShadowPriest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.trkReplenishment);
            this.Controls.Add(this.lblReplenishment);
            this.Controls.Add(this.trkShadowfiend);
            this.Controls.Add(this.lblShadowfiend);
            this.Controls.Add(this.trkFightLength);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.cbTargetLevel);
            this.Controls.Add(this.gbSpellPriority);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.lblFightLength);
            this.Name = "CalculationOptionsPanelShadowPriest";
            this.Size = new System.Drawing.Size(300, 605);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbManaTime)).EndInit();
            this.gbSpellPriority.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.trkFightLength)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkShadowfiend)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkReplenishment)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblFightLength;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.NumericUpDown cmbManaTime;
        private System.Windows.Forms.ComboBox cmbManaAmt;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox gbSpellPriority;
        private System.Windows.Forms.ListBox lsSpellPriopity;
        private System.Windows.Forms.Button bChangePriority;
        private System.Windows.Forms.ComboBox cbTargetLevel;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.TrackBar trkFightLength;
        private System.Windows.Forms.Label lblShadowfiend;
        private System.Windows.Forms.TrackBar trkShadowfiend;
        private System.Windows.Forms.Label lblReplenishment;
        private System.Windows.Forms.TrackBar trkReplenishment;
    }
}
