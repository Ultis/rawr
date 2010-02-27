namespace Rawr.WarlockTmp
{
    partial class CalculationOptionsPanelWarlock
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
            this.petCombo = new System.Windows.Forms.ComboBox();
            this.rotationCombo = new System.Windows.Forms.ComboBox();
            this.newRotationButton = new System.Windows.Forms.Button();
            this.deleteRotationButton = new System.Windows.Forms.Button();
            this.rotationBox = new System.Windows.Forms.GroupBox();
            this.rotationList = new System.Windows.Forms.ListBox();
            this.rotationUpButton = new System.Windows.Forms.Button();
            this.rotationDownButton = new System.Windows.Forms.Button();
            this.rotationRemoveButton = new System.Windows.Forms.Button();
            this.rotationClearButton = new System.Windows.Forms.Button();
            this.petLabel = new System.Windows.Forms.Label();
            this.rotationAddButton = new System.Windows.Forms.Button();
            this.rotationSpellCombo = new System.Windows.Forms.ComboBox();
            this.targetLevelCombo = new System.Windows.Forms.ComboBox();
            this.targetLevelLabel = new System.Windows.Forms.Label();
            this.manaPotionCombo = new System.Windows.Forms.ComboBox();
            this.manaPotionLabel = new System.Windows.Forms.Label();
            this.fightLengthLabel = new System.Windows.Forms.Label();
            this.fightLengthSpinner = new System.Windows.Forms.NumericUpDown();
            this.latencySpinner = new System.Windows.Forms.NumericUpDown();
            this.latencyLabel = new System.Windows.Forms.Label();
            this.replenishmentSpinner = new System.Windows.Forms.NumericUpDown();
            this.replenishmentLabel = new System.Windows.Forms.Label();
            this.infernalCheck = new System.Windows.Forms.CheckBox();
            this.rotationSeparator = new System.Windows.Forms.Panel();
            this.rotationRenameButton = new System.Windows.Forms.Button();
            this.rotationBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fightLengthSpinner)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.latencySpinner)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.replenishmentSpinner)).BeginInit();
            this.SuspendLayout();
            // 
            // petCombo
            // 
            this.petCombo.Enabled = false;
            this.petCombo.FormattingEnabled = true;
            this.petCombo.Items.AddRange(new object[] {
            "None"});
            this.petCombo.Location = new System.Drawing.Point(142, 3);
            this.petCombo.Name = "petCombo";
            this.petCombo.Size = new System.Drawing.Size(155, 21);
            this.petCombo.TabIndex = 1;
            // 
            // rotationCombo
            // 
            this.rotationCombo.Enabled = false;
            this.rotationCombo.FormattingEnabled = true;
            this.rotationCombo.Items.AddRange(new object[] {
            "Destruction"});
            this.rotationCombo.Location = new System.Drawing.Point(6, 19);
            this.rotationCombo.Name = "rotationCombo";
            this.rotationCombo.Size = new System.Drawing.Size(282, 21);
            this.rotationCombo.TabIndex = 2;
            // 
            // newRotationButton
            // 
            this.newRotationButton.AutoSize = true;
            this.newRotationButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.newRotationButton.Enabled = false;
            this.newRotationButton.Location = new System.Drawing.Point(132, 46);
            this.newRotationButton.Name = "newRotationButton";
            this.newRotationButton.Size = new System.Drawing.Size(39, 23);
            this.newRotationButton.TabIndex = 4;
            this.newRotationButton.Text = "New";
            this.newRotationButton.UseVisualStyleBackColor = true;
            // 
            // deleteRotationButton
            // 
            this.deleteRotationButton.AutoSize = true;
            this.deleteRotationButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.deleteRotationButton.Enabled = false;
            this.deleteRotationButton.Location = new System.Drawing.Point(240, 46);
            this.deleteRotationButton.Name = "deleteRotationButton";
            this.deleteRotationButton.Size = new System.Drawing.Size(48, 23);
            this.deleteRotationButton.TabIndex = 6;
            this.deleteRotationButton.Text = "Delete";
            this.deleteRotationButton.UseVisualStyleBackColor = true;
            // 
            // rotationBox
            // 
            this.rotationBox.Controls.Add(this.rotationRenameButton);
            this.rotationBox.Controls.Add(this.rotationSeparator);
            this.rotationBox.Controls.Add(this.rotationSpellCombo);
            this.rotationBox.Controls.Add(this.rotationAddButton);
            this.rotationBox.Controls.Add(this.rotationClearButton);
            this.rotationBox.Controls.Add(this.rotationRemoveButton);
            this.rotationBox.Controls.Add(this.rotationDownButton);
            this.rotationBox.Controls.Add(this.rotationUpButton);
            this.rotationBox.Controls.Add(this.rotationList);
            this.rotationBox.Controls.Add(this.rotationCombo);
            this.rotationBox.Controls.Add(this.deleteRotationButton);
            this.rotationBox.Controls.Add(this.newRotationButton);
            this.rotationBox.Location = new System.Drawing.Point(3, 186);
            this.rotationBox.Name = "rotationBox";
            this.rotationBox.Size = new System.Drawing.Size(294, 229);
            this.rotationBox.TabIndex = 7;
            this.rotationBox.TabStop = false;
            this.rotationBox.Text = "Spell Priorities";
            // 
            // rotationList
            // 
            this.rotationList.Enabled = false;
            this.rotationList.FormattingEnabled = true;
            this.rotationList.Location = new System.Drawing.Point(6, 113);
            this.rotationList.Name = "rotationList";
            this.rotationList.Size = new System.Drawing.Size(219, 108);
            this.rotationList.TabIndex = 7;
            // 
            // rotationUpButton
            // 
            this.rotationUpButton.Enabled = false;
            this.rotationUpButton.Location = new System.Drawing.Point(231, 140);
            this.rotationUpButton.Name = "rotationUpButton";
            this.rotationUpButton.Size = new System.Drawing.Size(57, 23);
            this.rotationUpButton.TabIndex = 8;
            this.rotationUpButton.Text = "Up";
            this.rotationUpButton.UseVisualStyleBackColor = true;
            // 
            // rotationDownButton
            // 
            this.rotationDownButton.Enabled = false;
            this.rotationDownButton.Location = new System.Drawing.Point(231, 169);
            this.rotationDownButton.Name = "rotationDownButton";
            this.rotationDownButton.Size = new System.Drawing.Size(57, 23);
            this.rotationDownButton.TabIndex = 9;
            this.rotationDownButton.Text = "Down";
            this.rotationDownButton.UseVisualStyleBackColor = true;
            // 
            // rotationRemoveButton
            // 
            this.rotationRemoveButton.Enabled = false;
            this.rotationRemoveButton.Location = new System.Drawing.Point(231, 111);
            this.rotationRemoveButton.Name = "rotationRemoveButton";
            this.rotationRemoveButton.Size = new System.Drawing.Size(57, 23);
            this.rotationRemoveButton.TabIndex = 10;
            this.rotationRemoveButton.Text = "Remove";
            this.rotationRemoveButton.UseVisualStyleBackColor = true;
            // 
            // rotationClearButton
            // 
            this.rotationClearButton.Enabled = false;
            this.rotationClearButton.Location = new System.Drawing.Point(231, 198);
            this.rotationClearButton.Name = "rotationClearButton";
            this.rotationClearButton.Size = new System.Drawing.Size(57, 23);
            this.rotationClearButton.TabIndex = 11;
            this.rotationClearButton.Text = "Clear";
            this.rotationClearButton.UseVisualStyleBackColor = true;
            // 
            // petLabel
            // 
            this.petLabel.AutoSize = true;
            this.petLabel.Location = new System.Drawing.Point(3, 6);
            this.petLabel.Name = "petLabel";
            this.petLabel.Size = new System.Drawing.Size(26, 13);
            this.petLabel.TabIndex = 2;
            this.petLabel.Text = "Pet:";
            // 
            // rotationAddButton
            // 
            this.rotationAddButton.Enabled = false;
            this.rotationAddButton.Location = new System.Drawing.Point(231, 82);
            this.rotationAddButton.Name = "rotationAddButton";
            this.rotationAddButton.Size = new System.Drawing.Size(57, 23);
            this.rotationAddButton.TabIndex = 12;
            this.rotationAddButton.Text = "Add";
            this.rotationAddButton.UseVisualStyleBackColor = true;
            // 
            // rotationSpellCombo
            // 
            this.rotationSpellCombo.Enabled = false;
            this.rotationSpellCombo.FormattingEnabled = true;
            this.rotationSpellCombo.Location = new System.Drawing.Point(6, 84);
            this.rotationSpellCombo.Name = "rotationSpellCombo";
            this.rotationSpellCombo.Size = new System.Drawing.Size(219, 21);
            this.rotationSpellCombo.TabIndex = 13;
            // 
            // targetLevelCombo
            // 
            this.targetLevelCombo.Enabled = false;
            this.targetLevelCombo.FormattingEnabled = true;
            this.targetLevelCombo.Items.AddRange(new object[] {
            "83",
            "82",
            "81",
            "80"});
            this.targetLevelCombo.Location = new System.Drawing.Point(142, 53);
            this.targetLevelCombo.Name = "targetLevelCombo";
            this.targetLevelCombo.Size = new System.Drawing.Size(155, 21);
            this.targetLevelCombo.TabIndex = 8;
            // 
            // targetLevelLabel
            // 
            this.targetLevelLabel.AutoSize = true;
            this.targetLevelLabel.Location = new System.Drawing.Point(3, 56);
            this.targetLevelLabel.Name = "targetLevelLabel";
            this.targetLevelLabel.Size = new System.Drawing.Size(70, 13);
            this.targetLevelLabel.TabIndex = 9;
            this.targetLevelLabel.Text = "Target Level:";
            // 
            // manaPotionCombo
            // 
            this.manaPotionCombo.Enabled = false;
            this.manaPotionCombo.FormattingEnabled = true;
            this.manaPotionCombo.Items.AddRange(new object[] {
            "None (0)"});
            this.manaPotionCombo.Location = new System.Drawing.Point(142, 81);
            this.manaPotionCombo.Name = "manaPotionCombo";
            this.manaPotionCombo.Size = new System.Drawing.Size(155, 21);
            this.manaPotionCombo.TabIndex = 10;
            // 
            // manaPotionLabel
            // 
            this.manaPotionLabel.AutoSize = true;
            this.manaPotionLabel.Location = new System.Drawing.Point(3, 84);
            this.manaPotionLabel.Name = "manaPotionLabel";
            this.manaPotionLabel.Size = new System.Drawing.Size(75, 13);
            this.manaPotionLabel.TabIndex = 11;
            this.manaPotionLabel.Text = "Mana Potions:";
            // 
            // fightLengthLabel
            // 
            this.fightLengthLabel.AutoSize = true;
            this.fightLengthLabel.Location = new System.Drawing.Point(3, 110);
            this.fightLengthLabel.Name = "fightLengthLabel";
            this.fightLengthLabel.Size = new System.Drawing.Size(95, 13);
            this.fightLengthLabel.TabIndex = 13;
            this.fightLengthLabel.Text = "Fight Length (sec):";
            // 
            // fightLengthSpinner
            // 
            this.fightLengthSpinner.Enabled = false;
            this.fightLengthSpinner.Increment = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.fightLengthSpinner.Location = new System.Drawing.Point(142, 108);
            this.fightLengthSpinner.Maximum = new decimal(new int[] {
            720,
            0,
            0,
            0});
            this.fightLengthSpinner.Name = "fightLengthSpinner";
            this.fightLengthSpinner.Size = new System.Drawing.Size(155, 20);
            this.fightLengthSpinner.TabIndex = 14;
            this.fightLengthSpinner.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // latencySpinner
            // 
            this.latencySpinner.Enabled = false;
            this.latencySpinner.Location = new System.Drawing.Point(142, 134);
            this.latencySpinner.Name = "latencySpinner";
            this.latencySpinner.Size = new System.Drawing.Size(155, 20);
            this.latencySpinner.TabIndex = 15;
            this.latencySpinner.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // latencyLabel
            // 
            this.latencyLabel.AutoSize = true;
            this.latencyLabel.Location = new System.Drawing.Point(5, 136);
            this.latencyLabel.Name = "latencyLabel";
            this.latencyLabel.Size = new System.Drawing.Size(131, 13);
            this.latencyLabel.TabIndex = 16;
            this.latencyLabel.Text = "Time Between Spells (ms):";
            // 
            // replenishmentSpinner
            // 
            this.replenishmentSpinner.Enabled = false;
            this.replenishmentSpinner.Location = new System.Drawing.Point(142, 160);
            this.replenishmentSpinner.Name = "replenishmentSpinner";
            this.replenishmentSpinner.Size = new System.Drawing.Size(155, 20);
            this.replenishmentSpinner.TabIndex = 17;
            this.replenishmentSpinner.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // replenishmentLabel
            // 
            this.replenishmentLabel.AutoSize = true;
            this.replenishmentLabel.Location = new System.Drawing.Point(3, 162);
            this.replenishmentLabel.Name = "replenishmentLabel";
            this.replenishmentLabel.Size = new System.Drawing.Size(133, 13);
            this.replenishmentLabel.TabIndex = 18;
            this.replenishmentLabel.Text = "Replenishment Uptime (%):";
            // 
            // infernalCheck
            // 
            this.infernalCheck.AutoSize = true;
            this.infernalCheck.Enabled = false;
            this.infernalCheck.Location = new System.Drawing.Point(142, 30);
            this.infernalCheck.Name = "infernalCheck";
            this.infernalCheck.Size = new System.Drawing.Size(106, 17);
            this.infernalCheck.TabIndex = 19;
            this.infernalCheck.Text = "Also Use Infernal";
            this.infernalCheck.UseVisualStyleBackColor = true;
            // 
            // rotationSeparator
            // 
            this.rotationSeparator.BackColor = System.Drawing.SystemColors.ControlDark;
            this.rotationSeparator.Location = new System.Drawing.Point(6, 75);
            this.rotationSeparator.Name = "rotationSeparator";
            this.rotationSeparator.Size = new System.Drawing.Size(282, 1);
            this.rotationSeparator.TabIndex = 14;
            // 
            // rotationRenameButton
            // 
            this.rotationRenameButton.AutoSize = true;
            this.rotationRenameButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.rotationRenameButton.Enabled = false;
            this.rotationRenameButton.Location = new System.Drawing.Point(177, 46);
            this.rotationRenameButton.Name = "rotationRenameButton";
            this.rotationRenameButton.Size = new System.Drawing.Size(57, 23);
            this.rotationRenameButton.TabIndex = 20;
            this.rotationRenameButton.Text = "Rename";
            this.rotationRenameButton.UseVisualStyleBackColor = true;
            // 
            // CalculationOptionsPanelWarlock
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.infernalCheck);
            this.Controls.Add(this.replenishmentLabel);
            this.Controls.Add(this.replenishmentSpinner);
            this.Controls.Add(this.latencyLabel);
            this.Controls.Add(this.latencySpinner);
            this.Controls.Add(this.fightLengthSpinner);
            this.Controls.Add(this.fightLengthLabel);
            this.Controls.Add(this.manaPotionLabel);
            this.Controls.Add(this.manaPotionCombo);
            this.Controls.Add(this.targetLevelLabel);
            this.Controls.Add(this.targetLevelCombo);
            this.Controls.Add(this.petCombo);
            this.Controls.Add(this.petLabel);
            this.Controls.Add(this.rotationBox);
            this.Name = "CalculationOptionsPanelWarlock";
            this.Size = new System.Drawing.Size(300, 472);
            this.rotationBox.ResumeLayout(false);
            this.rotationBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fightLengthSpinner)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.latencySpinner)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.replenishmentSpinner)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox petCombo;
        private System.Windows.Forms.ComboBox rotationCombo;
        private System.Windows.Forms.Button newRotationButton;
        private System.Windows.Forms.Button deleteRotationButton;
        private System.Windows.Forms.GroupBox rotationBox;
        private System.Windows.Forms.Button rotationClearButton;
        private System.Windows.Forms.Button rotationRemoveButton;
        private System.Windows.Forms.Button rotationDownButton;
        private System.Windows.Forms.Button rotationUpButton;
        private System.Windows.Forms.ListBox rotationList;
        private System.Windows.Forms.Label petLabel;
        private System.Windows.Forms.ComboBox rotationSpellCombo;
        private System.Windows.Forms.Button rotationAddButton;
        private System.Windows.Forms.ComboBox targetLevelCombo;
        private System.Windows.Forms.Label targetLevelLabel;
        private System.Windows.Forms.ComboBox manaPotionCombo;
        private System.Windows.Forms.Label manaPotionLabel;
        private System.Windows.Forms.Label fightLengthLabel;
        private System.Windows.Forms.NumericUpDown fightLengthSpinner;
        private System.Windows.Forms.NumericUpDown latencySpinner;
        private System.Windows.Forms.Label latencyLabel;
        private System.Windows.Forms.NumericUpDown replenishmentSpinner;
        private System.Windows.Forms.Label replenishmentLabel;
        private System.Windows.Forms.CheckBox infernalCheck;
        private System.Windows.Forms.Panel rotationSeparator;
        private System.Windows.Forms.Button rotationRenameButton;
    }
}
