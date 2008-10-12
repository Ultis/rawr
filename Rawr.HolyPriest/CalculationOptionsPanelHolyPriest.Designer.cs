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
            this.ckbManaPotion = new System.Windows.Forms.CheckBox();
            this.trkSurvivability = new System.Windows.Forms.TrackBar();
            this.lblSerendipity = new System.Windows.Forms.Label();
            this.lblReplenishment = new System.Windows.Forms.Label();
            this.lblShadowfiend = new System.Windows.Forms.Label();
            this.lblSurvivability = new System.Windows.Forms.Label();
            this.trkRapture = new System.Windows.Forms.TrackBar();
            this.lblRapture = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.trkActivity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkFightLength)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkSerendipity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkReplenishment)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkShadowfiend)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkSurvivability)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkRapture)).BeginInit();
            this.SuspendLayout();
            // 
            // trkActivity
            // 
            this.trkActivity.BackColor = System.Drawing.SystemColors.Control;
            this.trkActivity.Location = new System.Drawing.Point(10, 93);
            this.trkActivity.Maximum = 100;
            this.trkActivity.Name = "trkActivity";
            this.trkActivity.Size = new System.Drawing.Size(235, 42);
            this.trkActivity.TabIndex = 23;
            this.trkActivity.TickFrequency = 5;
            this.toolTip1.SetToolTip(this.trkActivity, resources.GetString("trkActivity.ToolTip"));
            this.trkActivity.Scroll += new System.EventHandler(this.trkActivity_Scroll);
            // 
            // lblActivity
            // 
            this.lblActivity.AutoSize = true;
            this.lblActivity.Location = new System.Drawing.Point(7, 77);
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
            "Disc-Raid (PW:S/Penance/Flash)"});
            this.cbRotation.Location = new System.Drawing.Point(10, 39);
            this.cbRotation.MaxDropDownItems = 10;
            this.cbRotation.Name = "cbRotation";
            this.cbRotation.Size = new System.Drawing.Size(235, 21);
            this.cbRotation.TabIndex = 26;
            this.toolTip1.SetToolTip(this.cbRotation, "Pick the spells to cast when comparing gear.");
            this.cbRotation.SelectedIndexChanged += new System.EventHandler(this.cbRotation_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 13);
            this.label1.TabIndex = 27;
            this.label1.Text = "Spell Cycle";
            // 
            // lblFightLength
            // 
            this.lblFightLength.AutoSize = true;
            this.lblFightLength.Location = new System.Drawing.Point(7, 138);
            this.lblFightLength.Name = "lblFightLength";
            this.lblFightLength.Size = new System.Drawing.Size(66, 13);
            this.lblFightLength.TabIndex = 28;
            this.lblFightLength.Text = "Fight Length";
            // 
            // trkFightLength
            // 
            this.trkFightLength.Location = new System.Drawing.Point(10, 154);
            this.trkFightLength.Maximum = 20;
            this.trkFightLength.Minimum = 1;
            this.trkFightLength.Name = "trkFightLength";
            this.trkFightLength.Size = new System.Drawing.Size(235, 42);
            this.trkFightLength.TabIndex = 29;
            this.toolTip1.SetToolTip(this.trkFightLength, "Changing this bar tells Rawr how long the fight is estimated to last. This has im" +
                    "pact on things like how good intellect and spirit and mp5 are compared to eachot" +
                    "her.");
            this.trkFightLength.Value = 1;
            this.trkFightLength.Scroll += new System.EventHandler(this.trkFightLength_Scroll);
            // 
            // trkSerendipity
            // 
            this.trkSerendipity.Location = new System.Drawing.Point(10, 214);
            this.trkSerendipity.Maximum = 100;
            this.trkSerendipity.Name = "trkSerendipity";
            this.trkSerendipity.Size = new System.Drawing.Size(235, 42);
            this.trkSerendipity.TabIndex = 30;
            this.trkSerendipity.TickFrequency = 5;
            this.toolTip1.SetToolTip(this.trkSerendipity, "Tell Rawr how many % of your Greater Heals and Flash Heals overheal the target, g" +
                    "iving you mana returns via Serendipity.");
            this.trkSerendipity.Scroll += new System.EventHandler(this.trkSerendipity_Scroll);
            // 
            // trkReplenishment
            // 
            this.trkReplenishment.Location = new System.Drawing.Point(10, 338);
            this.trkReplenishment.Maximum = 100;
            this.trkReplenishment.Name = "trkReplenishment";
            this.trkReplenishment.Size = new System.Drawing.Size(235, 42);
            this.trkReplenishment.TabIndex = 33;
            this.trkReplenishment.TickFrequency = 5;
            this.toolTip1.SetToolTip(this.trkReplenishment, "This tells Rawr how much of the time you are expected to have Replenishment.");
            this.trkReplenishment.Scroll += new System.EventHandler(this.trkReplenishment_Scroll);
            // 
            // trkShadowfiend
            // 
            this.trkShadowfiend.Location = new System.Drawing.Point(10, 401);
            this.trkShadowfiend.Maximum = 100;
            this.trkShadowfiend.Name = "trkShadowfiend";
            this.trkShadowfiend.Size = new System.Drawing.Size(235, 42);
            this.trkShadowfiend.TabIndex = 35;
            this.trkShadowfiend.TickFrequency = 5;
            this.toolTip1.SetToolTip(this.trkShadowfiend, "Tells Rawr how effective your Shadowfiend is expected to be. Against a boss, your" +
                    " Shadowfiend has a 9% chance to miss, and 6.5% chance to be dodged, a total of 1" +
                    "5.5% miss. Keep that in mind.");
            this.trkShadowfiend.Scroll += new System.EventHandler(this.trkShadowfiend_Scroll);
            // 
            // ckbManaPotion
            // 
            this.ckbManaPotion.AutoSize = true;
            this.ckbManaPotion.Location = new System.Drawing.Point(10, 3);
            this.ckbManaPotion.Name = "ckbManaPotion";
            this.ckbManaPotion.Size = new System.Drawing.Size(142, 17);
            this.ckbManaPotion.TabIndex = 36;
            this.ckbManaPotion.Text = "Use Super Mana Potion!";
            this.toolTip1.SetToolTip(this.ckbManaPotion, "Tell Rawr to use a Super Mana Potion  (1800-3000 Mana)");
            this.ckbManaPotion.UseVisualStyleBackColor = true;
            this.ckbManaPotion.CheckedChanged += new System.EventHandler(this.ckbManaPotion_CheckedChanged);
            // 
            // trkSurvivability
            // 
            this.trkSurvivability.Location = new System.Drawing.Point(10, 462);
            this.trkSurvivability.Maximum = 100;
            this.trkSurvivability.Name = "trkSurvivability";
            this.trkSurvivability.Size = new System.Drawing.Size(235, 42);
            this.trkSurvivability.TabIndex = 38;
            this.trkSurvivability.TickFrequency = 5;
            this.toolTip1.SetToolTip(this.trkSurvivability, "Change this slider to tell Rawr how much you value your Health.");
            this.trkSurvivability.Scroll += new System.EventHandler(this.trkSurvivability_Scroll);
            // 
            // lblSerendipity
            // 
            this.lblSerendipity.AutoSize = true;
            this.lblSerendipity.Location = new System.Drawing.Point(7, 198);
            this.lblSerendipity.Name = "lblSerendipity";
            this.lblSerendipity.Size = new System.Drawing.Size(70, 13);
            this.lblSerendipity.TabIndex = 31;
            this.lblSerendipity.Text = "% Serendipity";
            // 
            // lblReplenishment
            // 
            this.lblReplenishment.AutoSize = true;
            this.lblReplenishment.Location = new System.Drawing.Point(7, 322);
            this.lblReplenishment.Name = "lblReplenishment";
            this.lblReplenishment.Size = new System.Drawing.Size(88, 13);
            this.lblReplenishment.TabIndex = 32;
            this.lblReplenishment.Text = "% Replenishment";
            // 
            // lblShadowfiend
            // 
            this.lblShadowfiend.AutoSize = true;
            this.lblShadowfiend.Location = new System.Drawing.Point(7, 383);
            this.lblShadowfiend.Name = "lblShadowfiend";
            this.lblShadowfiend.Size = new System.Drawing.Size(80, 13);
            this.lblShadowfiend.TabIndex = 34;
            this.lblShadowfiend.Text = "% Shadowfiend";
            // 
            // lblSurvivability
            // 
            this.lblSurvivability.AutoSize = true;
            this.lblSurvivability.Location = new System.Drawing.Point(7, 446);
            this.lblSurvivability.Name = "lblSurvivability";
            this.lblSurvivability.Size = new System.Drawing.Size(63, 13);
            this.lblSurvivability.TabIndex = 37;
            this.lblSurvivability.Text = "Survivability";
            // 
            // trkRapture
            // 
            this.trkRapture.Location = new System.Drawing.Point(10, 277);
            this.trkRapture.Maximum = 100;
            this.trkRapture.Name = "trkRapture";
            this.trkRapture.Size = new System.Drawing.Size(235, 42);
            this.trkRapture.TabIndex = 39;
            this.trkRapture.TickFrequency = 5;
            this.trkRapture.Scroll += new System.EventHandler(this.trkRapture_Scroll);
            // 
            // lblRapture
            // 
            this.lblRapture.AutoSize = true;
            this.lblRapture.Location = new System.Drawing.Point(7, 259);
            this.lblRapture.Name = "lblRapture";
            this.lblRapture.Size = new System.Drawing.Size(56, 13);
            this.lblRapture.TabIndex = 40;
            this.lblRapture.Text = "% Rapture";
            // 
            // CalculationOptionsPanelHolyPriest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblRapture);
            this.Controls.Add(this.trkRapture);
            this.Controls.Add(this.trkSurvivability);
            this.Controls.Add(this.lblSurvivability);
            this.Controls.Add(this.ckbManaPotion);
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
            this.Size = new System.Drawing.Size(258, 520);
            ((System.ComponentModel.ISupportInitialize)(this.trkActivity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkFightLength)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkSerendipity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkReplenishment)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkShadowfiend)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkSurvivability)).EndInit();
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
        private System.Windows.Forms.CheckBox ckbManaPotion;
        private System.Windows.Forms.Label lblSurvivability;
        private System.Windows.Forms.TrackBar trkSurvivability;
        private System.Windows.Forms.TrackBar trkRapture;
        private System.Windows.Forms.Label lblRapture;
    }
}
