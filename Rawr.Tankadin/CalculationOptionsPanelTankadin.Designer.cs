namespace Rawr.Tankadin
{
    partial class CalculationOptionsPanelTankadin
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
            this.nudTargetLevel = new System.Windows.Forms.NumericUpDown();
            this.trackBarMitigationScale = new System.Windows.Forms.TrackBar();
            this.trackBarThreatScale = new System.Windows.Forms.TrackBar();
            this.trackBarBossAttackValue = new System.Windows.Forms.TrackBar();
            this.lblMitigationScaleText = new Rawr.CustomControls.ExtendedToolTipLabel();
            this.lblThreatScaleText = new Rawr.CustomControls.ExtendedToolTipLabel();
            this.lblBossAttack = new Rawr.CustomControls.ExtendedToolTipLabel();
            this.lblMitigationScaleValue = new System.Windows.Forms.Label();
            this.lblThreatScaleValue = new System.Windows.Forms.Label();
            this.lblBossAttackValue = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.nudTargetLevel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarMitigationScale)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarThreatScale)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarBossAttackValue)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Target Level:";
            // 
            // nudTargetLevel
            // 
            this.nudTargetLevel.Location = new System.Drawing.Point(79, 3);
            this.nudTargetLevel.Maximum = new decimal(new int[] {
            83,
            0,
            0,
            0});
            this.nudTargetLevel.Minimum = new decimal(new int[] {
            70,
            0,
            0,
            0});
            this.nudTargetLevel.Name = "nudTargetLevel";
            this.nudTargetLevel.Size = new System.Drawing.Size(120, 20);
            this.nudTargetLevel.TabIndex = 3;
            this.nudTargetLevel.Value = new decimal(new int[] {
            73,
            0,
            0,
            0});
            this.nudTargetLevel.ValueChanged += new System.EventHandler(this.nudTargetLevel_ValueChanged);
            // 
            // trackBarMitigationScale
            // 
            this.trackBarMitigationScale.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.trackBarMitigationScale.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.trackBarMitigationScale.LargeChange = 1000;
            this.trackBarMitigationScale.Location = new System.Drawing.Point(72, 131);
            this.trackBarMitigationScale.Maximum = 15000;
            this.trackBarMitigationScale.Minimum = 1000;
            this.trackBarMitigationScale.Name = "trackBarMitigationScale";
            this.trackBarMitigationScale.Size = new System.Drawing.Size(222, 45);
            this.trackBarMitigationScale.SmallChange = 50;
            this.trackBarMitigationScale.TabIndex = 11;
            this.trackBarMitigationScale.TickFrequency = 1000;
            this.trackBarMitigationScale.Value = 4000;
            this.trackBarMitigationScale.Scroll += new System.EventHandler(this.trackBarMitigationScale_Scroll);
            // 
            // trackBarThreatScale
            // 
            this.trackBarThreatScale.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.trackBarThreatScale.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.trackBarThreatScale.LargeChange = 10;
            this.trackBarThreatScale.Location = new System.Drawing.Point(72, 80);
            this.trackBarThreatScale.Maximum = 100;
            this.trackBarThreatScale.Name = "trackBarThreatScale";
            this.trackBarThreatScale.Size = new System.Drawing.Size(222, 45);
            this.trackBarThreatScale.TabIndex = 8;
            this.trackBarThreatScale.TickFrequency = 10;
            this.trackBarThreatScale.Value = 50;
            this.trackBarThreatScale.Scroll += new System.EventHandler(this.trackBarThreatScale_Scroll);
            // 
            // trackBarBossAttackValue
            // 
            this.trackBarBossAttackValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.trackBarBossAttackValue.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.trackBarBossAttackValue.LargeChange = 5000;
            this.trackBarBossAttackValue.Location = new System.Drawing.Point(72, 29);
            this.trackBarBossAttackValue.Maximum = 50000;
            this.trackBarBossAttackValue.Minimum = 500;
            this.trackBarBossAttackValue.Name = "trackBarBossAttackValue";
            this.trackBarBossAttackValue.Size = new System.Drawing.Size(222, 45);
            this.trackBarBossAttackValue.SmallChange = 500;
            this.trackBarBossAttackValue.TabIndex = 5;
            this.trackBarBossAttackValue.TickFrequency = 2500;
            this.trackBarBossAttackValue.Value = 20000;
            this.trackBarBossAttackValue.Scroll += new System.EventHandler(this.trackBarBossAttackValue_Scroll);
            // 
            // lblMitigationScaleText
            // 
            this.lblMitigationScaleText.Location = new System.Drawing.Point(5, 131);
            this.lblMitigationScaleText.Name = "lblMitigationScaleText";
            this.lblMitigationScaleText.Size = new System.Drawing.Size(61, 45);
            this.lblMitigationScaleText.TabIndex = 10;
            this.lblMitigationScaleText.Text = "Mitigation Scale: *";
            this.lblMitigationScaleText.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblMitigationScaleText.ToolTipText = "Mitigation Points scaling factor. PageUp/PageDown/Left Arrow/Right Arrow allows m" +
                "ore accurate changes";
            // 
            // lblThreatScaleText
            // 
            this.lblThreatScaleText.Location = new System.Drawing.Point(2, 80);
            this.lblThreatScaleText.Name = "lblThreatScaleText";
            this.lblThreatScaleText.Size = new System.Drawing.Size(64, 45);
            this.lblThreatScaleText.TabIndex = 7;
            this.lblThreatScaleText.Text = "Threat Scale: *";
            this.lblThreatScaleText.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblThreatScaleText.ToolTipText = "Threat scaling factor. PageUp/PageDown/Left Arrow/Right Arrow allows more accurat" +
                "e changes";
            // 
            // lblBossAttack
            // 
            this.lblBossAttack.Location = new System.Drawing.Point(2, 29);
            this.lblBossAttack.Name = "lblBossAttack";
            this.lblBossAttack.Size = new System.Drawing.Size(64, 45);
            this.lblBossAttack.TabIndex = 4;
            this.lblBossAttack.Text = "Boss Attack: *";
            this.lblBossAttack.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblBossAttack.ToolTipText = "Boss attack value before armor. Used to determine the value of Block Value.";
            // 
            // lblMitigationScaleValue
            // 
            this.lblMitigationScaleValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblMitigationScaleValue.AutoSize = true;
            this.lblMitigationScaleValue.Location = new System.Drawing.Point(72, 163);
            this.lblMitigationScaleValue.Name = "lblMitigationScaleValue";
            this.lblMitigationScaleValue.Size = new System.Drawing.Size(31, 13);
            this.lblMitigationScaleValue.TabIndex = 12;
            this.lblMitigationScaleValue.Text = "4000";
            // 
            // lblThreatScaleValue
            // 
            this.lblThreatScaleValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblThreatScaleValue.AutoSize = true;
            this.lblThreatScaleValue.Location = new System.Drawing.Point(72, 112);
            this.lblThreatScaleValue.Name = "lblThreatScaleValue";
            this.lblThreatScaleValue.Size = new System.Drawing.Size(31, 13);
            this.lblThreatScaleValue.TabIndex = 9;
            this.lblThreatScaleValue.Text = "4000";
            // 
            // lblBossAttackValue
            // 
            this.lblBossAttackValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblBossAttackValue.AutoSize = true;
            this.lblBossAttackValue.Location = new System.Drawing.Point(72, 61);
            this.lblBossAttackValue.Name = "lblBossAttackValue";
            this.lblBossAttackValue.Size = new System.Drawing.Size(31, 13);
            this.lblBossAttackValue.TabIndex = 6;
            this.lblBossAttackValue.Text = "4000";
            // 
            // CalculationOptionsPanelTankadin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblBossAttackValue);
            this.Controls.Add(this.lblThreatScaleValue);
            this.Controls.Add(this.lblMitigationScaleValue);
            this.Controls.Add(this.trackBarMitigationScale);
            this.Controls.Add(this.trackBarThreatScale);
            this.Controls.Add(this.trackBarBossAttackValue);
            this.Controls.Add(this.lblMitigationScaleText);
            this.Controls.Add(this.lblThreatScaleText);
            this.Controls.Add(this.lblBossAttack);
            this.Controls.Add(this.nudTargetLevel);
            this.Controls.Add(this.label1);
            this.Name = "CalculationOptionsPanelTankadin";
            this.Size = new System.Drawing.Size(297, 375);
            ((System.ComponentModel.ISupportInitialize)(this.nudTargetLevel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarMitigationScale)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarThreatScale)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarBossAttackValue)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown nudTargetLevel;
        private System.Windows.Forms.TrackBar trackBarMitigationScale;
        private System.Windows.Forms.TrackBar trackBarThreatScale;
        private System.Windows.Forms.TrackBar trackBarBossAttackValue;
        private Rawr.CustomControls.ExtendedToolTipLabel lblMitigationScaleText;
        private Rawr.CustomControls.ExtendedToolTipLabel lblThreatScaleText;
        private Rawr.CustomControls.ExtendedToolTipLabel lblBossAttack;
        private System.Windows.Forms.Label lblMitigationScaleValue;
        private System.Windows.Forms.Label lblThreatScaleValue;
        private System.Windows.Forms.Label lblBossAttackValue;

    }
}
