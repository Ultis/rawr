namespace Rawr.HolyPriest
{
    partial class CalculationOptionsPanelHolyPriest
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CalculationOptionsPanelHolyPriest));
            this.trkActivity = new System.Windows.Forms.TrackBar();
            this.lblActivity = new System.Windows.Forms.Label();
            this.cbRotation = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lblFightLength = new System.Windows.Forms.Label();
            this.trkFightLength = new System.Windows.Forms.TrackBar();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.trkSerendipity = new System.Windows.Forms.TrackBar();
            this.trkReplenishment = new System.Windows.Forms.TrackBar();
            this.trkShadowfiend = new System.Windows.Forms.TrackBar();
            this.trkSurvivability = new System.Windows.Forms.TrackBar();
            this.trkTestOfFaith = new System.Windows.Forms.TrackBar();
            this.cbModelProcs = new System.Windows.Forms.CheckBox();
            this.lblSerendipity = new System.Windows.Forms.Label();
            this.lblReplenishment = new System.Windows.Forms.Label();
            this.lblShadowfiend = new System.Windows.Forms.Label();
            this.lblSurvivability = new System.Windows.Forms.Label();
            this.trkRapture = new System.Windows.Forms.TrackBar();
            this.lblRapture = new System.Windows.Forms.Label();
            this.lblTestOfFaith = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbManaAmt = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.trkActivity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkFightLength)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkSerendipity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkReplenishment)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkShadowfiend)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkSurvivability)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkTestOfFaith)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkRapture)).BeginInit();
            this.SuspendLayout();
            // 
            // trkActivity
            // 
            this.trkActivity.BackColor = System.Drawing.SystemColors.Control;
            this.trkActivity.Location = new System.Drawing.Point(6, 82);
            this.trkActivity.Maximum = 100;
            this.trkActivity.Name = "trkActivity";
            this.trkActivity.Size = new System.Drawing.Size(270, 42);
            this.trkActivity.TabIndex = 23;
            this.trkActivity.TickFrequency = 5;
            this.toolTip1.SetToolTip(this.trkActivity, resources.GetString("trkActivity.ToolTip"));
            this.trkActivity.Scroll += new System.EventHandler(this.trkActivity_Scroll);
            // 
            // lblActivity
            // 
            this.lblActivity.AutoSize = true;
            this.lblActivity.Location = new System.Drawing.Point(3, 66);
            this.lblActivity.Name = "lblActivity";
            this.lblActivity.Size = new System.Drawing.Size(65, 13);
            this.lblActivity.TabIndex = 24;
            this.lblActivity.Text = "Time in FSR";
            // 
            // cbRotation
            // 
            this.cbRotation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbRotation.FormattingEnabled = true;
            this.cbRotation.Items.AddRange(new object[] {
            "Auto Tank (Rawr picks from Talents)",
            "Auto Raid (Rawr picks from Talents)",
            "Greater Heal Spam (GH)",
            "Flash Heal Spam (FH)",
            "Circle of Healing Spam (CoH)",
            "Holy-Tank (Renew/ProM/GH)",
            "Holy-Raid (ProM/CoH/FH)",
            "Disc-Tank (Penance/PW:S/ProM/GH)",
            "Disc-Tank (Penance/PW:S/ProM/FH)",
            "Disc-Raid (PW:S/Penance/Flash)"});
            this.cbRotation.Location = new System.Drawing.Point(6, 16);
            this.cbRotation.MaxDropDownItems = 10;
            this.cbRotation.Name = "cbRotation";
            this.cbRotation.Size = new System.Drawing.Size(270, 21);
            this.cbRotation.TabIndex = 26;
            this.toolTip1.SetToolTip(this.cbRotation, "Pick the spells to cast when comparing gear.");
            this.cbRotation.SelectedIndexChanged += new System.EventHandler(this.cbRotation_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 13);
            this.label1.TabIndex = 27;
            this.label1.Text = "Spell Usage";
            // 
            // lblFightLength
            // 
            this.lblFightLength.AutoSize = true;
            this.lblFightLength.Location = new System.Drawing.Point(3, 127);
            this.lblFightLength.Name = "lblFightLength";
            this.lblFightLength.Size = new System.Drawing.Size(66, 13);
            this.lblFightLength.TabIndex = 28;
            this.lblFightLength.Text = "Fight Length";
            // 
            // trkFightLength
            // 
            this.trkFightLength.Location = new System.Drawing.Point(6, 143);
            this.trkFightLength.Maximum = 20;
            this.trkFightLength.Minimum = 1;
            this.trkFightLength.Name = "trkFightLength";
            this.trkFightLength.Size = new System.Drawing.Size(270, 42);
            this.trkFightLength.TabIndex = 29;
            this.toolTip1.SetToolTip(this.trkFightLength, "Changing this bar tells Rawr how long the fight is estimated to last. This has im" +
                    "pact on things like how good intellect and spirit and mp5 are compared to eachot" +
                    "her.");
            this.trkFightLength.Value = 1;
            this.trkFightLength.Scroll += new System.EventHandler(this.trkFightLength_Scroll);
            // 
            // trkSerendipity
            // 
            this.trkSerendipity.Location = new System.Drawing.Point(6, 204);
            this.trkSerendipity.Maximum = 100;
            this.trkSerendipity.Name = "trkSerendipity";
            this.trkSerendipity.Size = new System.Drawing.Size(270, 42);
            this.trkSerendipity.TabIndex = 30;
            this.trkSerendipity.TickFrequency = 5;
            this.toolTip1.SetToolTip(this.trkSerendipity, "Tell Rawr how many % of your Greater Heals and Flash Heals overheal the target, g" +
                    "iving you mana returns via Serendipity. Also controls how effective T5 2 Part bo" +
                    "nus is.");
            this.trkSerendipity.Scroll += new System.EventHandler(this.trkSerendipity_Scroll);
            // 
            // trkReplenishment
            // 
            this.trkReplenishment.Location = new System.Drawing.Point(6, 326);
            this.trkReplenishment.Maximum = 100;
            this.trkReplenishment.Name = "trkReplenishment";
            this.trkReplenishment.Size = new System.Drawing.Size(270, 42);
            this.trkReplenishment.TabIndex = 33;
            this.trkReplenishment.TickFrequency = 5;
            this.toolTip1.SetToolTip(this.trkReplenishment, "This tells Rawr how much of the time you are expected to have Replenishment.");
            this.trkReplenishment.Scroll += new System.EventHandler(this.trkReplenishment_Scroll);
            // 
            // trkShadowfiend
            // 
            this.trkShadowfiend.Location = new System.Drawing.Point(6, 387);
            this.trkShadowfiend.Maximum = 150;
            this.trkShadowfiend.Name = "trkShadowfiend";
            this.trkShadowfiend.Size = new System.Drawing.Size(270, 42);
            this.trkShadowfiend.TabIndex = 35;
            this.trkShadowfiend.TickFrequency = 5;
            this.toolTip1.SetToolTip(this.trkShadowfiend, resources.GetString("trkShadowfiend.ToolTip"));
            this.trkShadowfiend.Scroll += new System.EventHandler(this.trkShadowfiend_Scroll);
            // 
            // trkSurvivability
            // 
            this.trkSurvivability.Location = new System.Drawing.Point(6, 508);
            this.trkSurvivability.Maximum = 100;
            this.trkSurvivability.Name = "trkSurvivability";
            this.trkSurvivability.Size = new System.Drawing.Size(270, 42);
            this.trkSurvivability.TabIndex = 38;
            this.trkSurvivability.TickFrequency = 5;
            this.toolTip1.SetToolTip(this.trkSurvivability, "Change this slider to tell Rawr how much you value your Health.");
            this.trkSurvivability.Scroll += new System.EventHandler(this.trkSurvivability_Scroll);
            // 
            // trkTestOfFaith
            // 
            this.trkTestOfFaith.Location = new System.Drawing.Point(6, 448);
            this.trkTestOfFaith.Maximum = 100;
            this.trkTestOfFaith.Name = "trkTestOfFaith";
            this.trkTestOfFaith.Size = new System.Drawing.Size(270, 42);
            this.trkTestOfFaith.TabIndex = 44;
            this.trkTestOfFaith.TickFrequency = 5;
            this.toolTip1.SetToolTip(this.trkTestOfFaith, "Set this slider to the amount of Heals landing on players with less than 50% heal" +
                    "th.");
            this.trkTestOfFaith.Scroll += new System.EventHandler(this.trkTestOfFaith_Scroll);
            // 
            // cbModelProcs
            // 
            this.cbModelProcs.AutoSize = true;
            this.cbModelProcs.Location = new System.Drawing.Point(6, 544);
            this.cbModelProcs.Name = "cbModelProcs";
            this.cbModelProcs.Size = new System.Drawing.Size(177, 17);
            this.cbModelProcs.TabIndex = 41;
            this.cbModelProcs.Text = "Model items with Procs and Use";
            this.toolTip1.SetToolTip(this.cbModelProcs, "Checking this will make Rawr model Trinkets and other items with Use and Procs");
            this.cbModelProcs.UseVisualStyleBackColor = true;
            this.cbModelProcs.CheckedChanged += new System.EventHandler(this.cbUseTrinkets_CheckedChanged);
            // 
            // lblSerendipity
            // 
            this.lblSerendipity.AutoSize = true;
            this.lblSerendipity.Location = new System.Drawing.Point(3, 188);
            this.lblSerendipity.Name = "lblSerendipity";
            this.lblSerendipity.Size = new System.Drawing.Size(70, 13);
            this.lblSerendipity.TabIndex = 31;
            this.lblSerendipity.Text = "% Serendipity";
            // 
            // lblReplenishment
            // 
            this.lblReplenishment.AutoSize = true;
            this.lblReplenishment.Location = new System.Drawing.Point(3, 310);
            this.lblReplenishment.Name = "lblReplenishment";
            this.lblReplenishment.Size = new System.Drawing.Size(88, 13);
            this.lblReplenishment.TabIndex = 32;
            this.lblReplenishment.Text = "% Replenishment";
            // 
            // lblShadowfiend
            // 
            this.lblShadowfiend.AutoSize = true;
            this.lblShadowfiend.Location = new System.Drawing.Point(3, 371);
            this.lblShadowfiend.Name = "lblShadowfiend";
            this.lblShadowfiend.Size = new System.Drawing.Size(80, 13);
            this.lblShadowfiend.TabIndex = 34;
            this.lblShadowfiend.Text = "% Shadowfiend";
            // 
            // lblSurvivability
            // 
            this.lblSurvivability.AutoSize = true;
            this.lblSurvivability.Location = new System.Drawing.Point(3, 492);
            this.lblSurvivability.Name = "lblSurvivability";
            this.lblSurvivability.Size = new System.Drawing.Size(63, 13);
            this.lblSurvivability.TabIndex = 37;
            this.lblSurvivability.Text = "Survivability";
            // 
            // trkRapture
            // 
            this.trkRapture.Location = new System.Drawing.Point(6, 265);
            this.trkRapture.Maximum = 100;
            this.trkRapture.Name = "trkRapture";
            this.trkRapture.Size = new System.Drawing.Size(270, 42);
            this.trkRapture.TabIndex = 39;
            this.trkRapture.TickFrequency = 5;
            this.trkRapture.Scroll += new System.EventHandler(this.trkRapture_Scroll);
            // 
            // lblRapture
            // 
            this.lblRapture.AutoSize = true;
            this.lblRapture.Location = new System.Drawing.Point(3, 255);
            this.lblRapture.Name = "lblRapture";
            this.lblRapture.Size = new System.Drawing.Size(56, 13);
            this.lblRapture.TabIndex = 40;
            this.lblRapture.Text = "% Rapture";
            // 
            // lblTestOfFaith
            // 
            this.lblTestOfFaith.AutoSize = true;
            this.lblTestOfFaith.Location = new System.Drawing.Point(3, 432);
            this.lblTestOfFaith.Name = "lblTestOfFaith";
            this.lblTestOfFaith.Size = new System.Drawing.Size(139, 13);
            this.lblTestOfFaith.TabIndex = 43;
            this.lblTestOfFaith.Text = "% of Heals use Test of Faith";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 43);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(75, 13);
            this.label4.TabIndex = 46;
            this.label4.Text = "Mana Potions:";
            // 
            // cmbManaAmt
            // 
            this.cmbManaAmt.DisplayMember = "2400";
            this.cmbManaAmt.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbManaAmt.Items.AddRange(new object[] {
            "(None) 0",
            "(Major) 1350-2250, Avg 1800",
            "(Mad) 1650-2750, Avg 2200",
            "(Super) 1800-3000, Avg 2400",
            "(Runic) 4200-4400, Avg 4300"});
            this.cmbManaAmt.Location = new System.Drawing.Point(84, 43);
            this.cmbManaAmt.Name = "cmbManaAmt";
            this.cmbManaAmt.Size = new System.Drawing.Size(192, 21);
            this.cmbManaAmt.TabIndex = 45;
            this.cmbManaAmt.ValueMember = "2400";
            this.cmbManaAmt.SelectedIndexChanged += new System.EventHandler(this.cmbManaAmt_SelectedIndexChanged);
            // 
            // CalculationOptionsPanelHolyPriest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cmbManaAmt);
            this.Controls.Add(this.trkTestOfFaith);
            this.Controls.Add(this.lblTestOfFaith);
            this.Controls.Add(this.cbModelProcs);
            this.Controls.Add(this.lblRapture);
            this.Controls.Add(this.trkRapture);
            this.Controls.Add(this.trkSurvivability);
            this.Controls.Add(this.lblSurvivability);
            this.Controls.Add(this.trkShadowfiend);
            this.Controls.Add(this.lblShadowfiend);
            this.Controls.Add(this.trkReplenishment);
            this.Controls.Add(this.lblReplenishment);
            this.Controls.Add(this.lblSerendipity);
            this.Controls.Add(this.trkSerendipity);
            this.Controls.Add(this.trkFightLength);
            this.Controls.Add(this.lblFightLength);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbRotation);
            this.Controls.Add(this.lblActivity);
            this.Controls.Add(this.trkActivity);
            this.Name = "CalculationOptionsPanelHolyPriest";
            this.Size = new System.Drawing.Size(285, 652);
            ((System.ComponentModel.ISupportInitialize)(this.trkActivity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkFightLength)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkSerendipity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkReplenishment)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkShadowfiend)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkSurvivability)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkTestOfFaith)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkRapture)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TrackBar trkActivity;
        private System.Windows.Forms.Label lblActivity;
        private System.Windows.Forms.ComboBox cbRotation;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblFightLength;
        private System.Windows.Forms.TrackBar trkFightLength;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.TrackBar trkSerendipity;
        private System.Windows.Forms.Label lblSerendipity;
        private System.Windows.Forms.Label lblReplenishment;
        private System.Windows.Forms.TrackBar trkReplenishment;
        private System.Windows.Forms.Label lblShadowfiend;
        private System.Windows.Forms.TrackBar trkShadowfiend;
        private System.Windows.Forms.Label lblSurvivability;
        private System.Windows.Forms.TrackBar trkSurvivability;
        private System.Windows.Forms.TrackBar trkRapture;
        private System.Windows.Forms.Label lblRapture;
        private System.Windows.Forms.CheckBox cbModelProcs;
        private System.Windows.Forms.Label lblTestOfFaith;
        private System.Windows.Forms.TrackBar trkTestOfFaith;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmbManaAmt;
    }
}
