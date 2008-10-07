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
            this.lblSerendipity = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.trkActivity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkFightLength)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkSerendipity)).BeginInit();
            this.SuspendLayout();
            // 
            // trkActivity
            // 
            this.trkActivity.BackColor = System.Drawing.SystemColors.Control;
            this.trkActivity.Location = new System.Drawing.Point(13, 70);
            this.trkActivity.Maximum = 100;
            this.trkActivity.Minimum = 1;
            this.trkActivity.Name = "trkActivity";
            this.trkActivity.Size = new System.Drawing.Size(189, 42);
            this.trkActivity.TabIndex = 23;
            this.trkActivity.TickFrequency = 5;
            this.toolTip1.SetToolTip(this.trkActivity, resources.GetString("trkActivity.ToolTip"));
            this.trkActivity.Value = 90;
            this.trkActivity.Scroll += new System.EventHandler(this.trkActivity_Scroll);
            // 
            // lblActivity
            // 
            this.lblActivity.AutoSize = true;
            this.lblActivity.Location = new System.Drawing.Point(10, 54);
            this.lblActivity.Name = "lblActivity";
            this.lblActivity.Size = new System.Drawing.Size(27, 13);
            this.lblActivity.TabIndex = 24;
            this.lblActivity.Text = "90%";
            // 
            // cbRotation
            // 
            this.cbRotation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbRotation.FormattingEnabled = true;
            this.cbRotation.Items.AddRange(new object[] {
            "Auto (Rawr picks from Talents)",
            "Greater Heal Spam (GH)",
            "Flash Heal Spam (FH)",
            "Circle of Healing Spam (CoH)",
            "Holy-MT (Renew/ProM/GH)",
            "Holy-Raid (CoHx1/FHx1)",
            "Disc-MT (Penance/PW:S/ProM/GH)",
            "Disc-Raid (PW:S/Penance/Flash)"});
            this.cbRotation.Location = new System.Drawing.Point(13, 16);
            this.cbRotation.Name = "cbRotation";
            this.cbRotation.Size = new System.Drawing.Size(189, 21);
            this.cbRotation.TabIndex = 26;
            this.toolTip1.SetToolTip(this.cbRotation, "Pick the spells to cast when comparing gear.");
            this.cbRotation.SelectedIndexChanged += new System.EventHandler(this.cbRotation_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 13);
            this.label1.TabIndex = 27;
            this.label1.Text = "Spell Cycle";
            // 
            // lblFightLength
            // 
            this.lblFightLength.AutoSize = true;
            this.lblFightLength.Location = new System.Drawing.Point(10, 115);
            this.lblFightLength.Name = "lblFightLength";
            this.lblFightLength.Size = new System.Drawing.Size(66, 13);
            this.lblFightLength.TabIndex = 28;
            this.lblFightLength.Text = "Fight Length";
            // 
            // trkFightLength
            // 
            this.trkFightLength.Location = new System.Drawing.Point(13, 136);
            this.trkFightLength.Maximum = 20;
            this.trkFightLength.Minimum = 1;
            this.trkFightLength.Name = "trkFightLength";
            this.trkFightLength.Size = new System.Drawing.Size(189, 42);
            this.trkFightLength.TabIndex = 29;
            this.toolTip1.SetToolTip(this.trkFightLength, "Changing this bar tells Rawr how long the fight is estimated to last. This has im" +
                    "pact on things like how good intellect and spirit and mp5 are compared to eachot" +
                    "her.");
            this.trkFightLength.Value = 1;
            this.trkFightLength.Scroll += new System.EventHandler(this.trkFightLength_Scroll);
            // 
            // trkSerendipity
            // 
            this.trkSerendipity.Location = new System.Drawing.Point(13, 194);
            this.trkSerendipity.Maximum = 100;
            this.trkSerendipity.Name = "trkSerendipity";
            this.trkSerendipity.Size = new System.Drawing.Size(189, 42);
            this.trkSerendipity.TabIndex = 30;
            this.trkSerendipity.TickFrequency = 5;
            this.toolTip1.SetToolTip(this.trkSerendipity, "Tell Rawr how many % of your Greater Heals and Flash Heals overheal the target, g" +
                    "iving you mana returns via Serendipity.");
            this.trkSerendipity.Scroll += new System.EventHandler(this.trkSerendipity_Scroll);
            // 
            // lblSerendipity
            // 
            this.lblSerendipity.AutoSize = true;
            this.lblSerendipity.Location = new System.Drawing.Point(13, 175);
            this.lblSerendipity.Name = "lblSerendipity";
            this.lblSerendipity.Size = new System.Drawing.Size(70, 13);
            this.lblSerendipity.TabIndex = 31;
            this.lblSerendipity.Text = "% Serendipity";
            // 
            // CalculationOptionsPanelHolyPriest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblSerendipity);
            this.Controls.Add(this.trkSerendipity);
            this.Controls.Add(this.trkFightLength);
            this.Controls.Add(this.lblFightLength);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbRotation);
            this.Controls.Add(this.lblActivity);
            this.Controls.Add(this.trkActivity);
            this.Name = "CalculationOptionsPanelHolyPriest";
            this.Size = new System.Drawing.Size(212, 289);
            ((System.ComponentModel.ISupportInitialize)(this.trkActivity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkFightLength)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkSerendipity)).EndInit();
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
    }
}
