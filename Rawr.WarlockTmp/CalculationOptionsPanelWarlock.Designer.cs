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
            this.fillerCombo = new System.Windows.Forms.ComboBox();
            this.fillerLabel = new System.Windows.Forms.Label();
            this.rotationMenu = new System.Windows.Forms.ListBox();
            this.rotationErrorLabel = new System.Windows.Forms.Label();
            this.rotationRenameButton = new System.Windows.Forms.Button();
            this.rotationSeparator = new System.Windows.Forms.Panel();
            this.rotationRemoveButton = new System.Windows.Forms.Button();
            this.rotationAddButton = new System.Windows.Forms.Button();
            this.rotationClearButton = new System.Windows.Forms.Button();
            this.rotationDownButton = new System.Windows.Forms.Button();
            this.rotationUpButton = new System.Windows.Forms.Button();
            this.rotationList = new System.Windows.Forms.ListBox();
            this.petLabel = new System.Windows.Forms.Label();
            this.targetLevelCombo = new System.Windows.Forms.ComboBox();
            this.targetLevelLabel = new System.Windows.Forms.Label();
            this.fightLengthLabel = new System.Windows.Forms.Label();
            this.fightLengthSpinner = new System.Windows.Forms.NumericUpDown();
            this.latencySpinner = new System.Windows.Forms.NumericUpDown();
            this.latencyLabel = new System.Windows.Forms.Label();
            this.infernalCheck = new System.Windows.Forms.CheckBox();
            this.ProcCheckbox = new System.Windows.Forms.CheckBox();
            this.rotationBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.fightLengthSpinner)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.latencySpinner)).BeginInit();
            this.SuspendLayout();
            // 
            // petCombo
            // 
            this.petCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.petCombo.Enabled = false;
            this.petCombo.FormattingEnabled = true;
            this.petCombo.Items.AddRange(new object[] {
            "None"});
            this.petCombo.Location = new System.Drawing.Point(142, 3);
            this.petCombo.Name = "petCombo";
            this.petCombo.Size = new System.Drawing.Size(155, 21);
            this.petCombo.TabIndex = 1;
            this.petCombo.SelectedIndexChanged += new System.EventHandler(this.petCombo_SelectedIndexChanged);
            // 
            // rotationCombo
            // 
            this.rotationCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.rotationCombo.FormattingEnabled = true;
            this.rotationCombo.Items.AddRange(new object[] {
            "Destruction"});
            this.rotationCombo.Location = new System.Drawing.Point(6, 19);
            this.rotationCombo.Name = "rotationCombo";
            this.rotationCombo.Size = new System.Drawing.Size(282, 21);
            this.rotationCombo.TabIndex = 6;
            this.rotationCombo.SelectedIndexChanged += new System.EventHandler(this.rotationCombo_SelectedIndexChanged);
            // 
            // newRotationButton
            // 
            this.newRotationButton.AutoSize = true;
            this.newRotationButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.newRotationButton.Location = new System.Drawing.Point(132, 46);
            this.newRotationButton.Name = "newRotationButton";
            this.newRotationButton.Size = new System.Drawing.Size(39, 23);
            this.newRotationButton.TabIndex = 7;
            this.newRotationButton.Text = "New";
            this.newRotationButton.UseVisualStyleBackColor = true;
            this.newRotationButton.Click += new System.EventHandler(this.newRotationButton_Click);
            // 
            // deleteRotationButton
            // 
            this.deleteRotationButton.AutoSize = true;
            this.deleteRotationButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.deleteRotationButton.Location = new System.Drawing.Point(240, 46);
            this.deleteRotationButton.Name = "deleteRotationButton";
            this.deleteRotationButton.Size = new System.Drawing.Size(48, 23);
            this.deleteRotationButton.TabIndex = 9;
            this.deleteRotationButton.Text = "Delete";
            this.deleteRotationButton.UseVisualStyleBackColor = true;
            this.deleteRotationButton.Click += new System.EventHandler(this.deleteRotationButton_Click);
            // 
            // rotationBox
            // 
            this.rotationBox.Controls.Add(this.fillerCombo);
            this.rotationBox.Controls.Add(this.fillerLabel);
            this.rotationBox.Controls.Add(this.rotationMenu);
            this.rotationBox.Controls.Add(this.rotationErrorLabel);
            this.rotationBox.Controls.Add(this.rotationRenameButton);
            this.rotationBox.Controls.Add(this.rotationSeparator);
            this.rotationBox.Controls.Add(this.rotationRemoveButton);
            this.rotationBox.Controls.Add(this.rotationAddButton);
            this.rotationBox.Controls.Add(this.rotationClearButton);
            this.rotationBox.Controls.Add(this.rotationDownButton);
            this.rotationBox.Controls.Add(this.rotationUpButton);
            this.rotationBox.Controls.Add(this.rotationCombo);
            this.rotationBox.Controls.Add(this.deleteRotationButton);
            this.rotationBox.Controls.Add(this.rotationList);
            this.rotationBox.Controls.Add(this.newRotationButton);
            this.rotationBox.Location = new System.Drawing.Point(3, 132);
            this.rotationBox.Name = "rotationBox";
            this.rotationBox.Size = new System.Drawing.Size(294, 280);
            this.rotationBox.TabIndex = 7;
            this.rotationBox.TabStop = false;
            this.rotationBox.Text = "Spell Priorities";
            // 
            // fillerCombo
            // 
            this.fillerCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.fillerCombo.FormattingEnabled = true;
            this.fillerCombo.Items.AddRange(new object[] {
            "Shadow Bolt",
            "Incinerate"});
            this.fillerCombo.Location = new System.Drawing.Point(43, 235);
            this.fillerCombo.Name = "fillerCombo";
            this.fillerCombo.Size = new System.Drawing.Size(245, 21);
            this.fillerCombo.TabIndex = 17;
            this.fillerCombo.SelectedIndexChanged += new System.EventHandler(this.fillerCombo_SelectedIndexChanged);
            // 
            // fillerLabel
            // 
            this.fillerLabel.AutoSize = true;
            this.fillerLabel.Location = new System.Drawing.Point(6, 238);
            this.fillerLabel.Name = "fillerLabel";
            this.fillerLabel.Size = new System.Drawing.Size(31, 13);
            this.fillerLabel.TabIndex = 21;
            this.fillerLabel.Text = "Filler:";
            // 
            // rotationMenu
            // 
            this.rotationMenu.FormattingEnabled = true;
            this.rotationMenu.Location = new System.Drawing.Point(6, 82);
            this.rotationMenu.Name = "rotationMenu";
            this.rotationMenu.Size = new System.Drawing.Size(107, 147);
            this.rotationMenu.TabIndex = 10;
            this.rotationMenu.SelectedIndexChanged += new System.EventHandler(this.rotationMenu_SelectedIndexChanged);
            // 
            // rotationErrorLabel
            // 
            this.rotationErrorLabel.ForeColor = System.Drawing.Color.Red;
            this.rotationErrorLabel.Location = new System.Drawing.Point(6, 259);
            this.rotationErrorLabel.Name = "rotationErrorLabel";
            this.rotationErrorLabel.Size = new System.Drawing.Size(282, 13);
            this.rotationErrorLabel.TabIndex = 19;
            this.rotationErrorLabel.Text = "Error Label";
            // 
            // rotationRenameButton
            // 
            this.rotationRenameButton.AutoSize = true;
            this.rotationRenameButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.rotationRenameButton.Location = new System.Drawing.Point(177, 46);
            this.rotationRenameButton.Name = "rotationRenameButton";
            this.rotationRenameButton.Size = new System.Drawing.Size(57, 23);
            this.rotationRenameButton.TabIndex = 8;
            this.rotationRenameButton.Text = "Rename";
            this.rotationRenameButton.UseVisualStyleBackColor = true;
            this.rotationRenameButton.Click += new System.EventHandler(this.rotationRenameButton_Click);
            // 
            // rotationSeparator
            // 
            this.rotationSeparator.BackColor = System.Drawing.SystemColors.ControlDark;
            this.rotationSeparator.Location = new System.Drawing.Point(6, 75);
            this.rotationSeparator.Name = "rotationSeparator";
            this.rotationSeparator.Size = new System.Drawing.Size(282, 1);
            this.rotationSeparator.TabIndex = 14;
            // 
            // rotationRemoveButton
            // 
            this.rotationRemoveButton.Location = new System.Drawing.Point(119, 111);
            this.rotationRemoveButton.Name = "rotationRemoveButton";
            this.rotationRemoveButton.Size = new System.Drawing.Size(56, 23);
            this.rotationRemoveButton.TabIndex = 13;
            this.rotationRemoveButton.Text = "<";
            this.rotationRemoveButton.UseVisualStyleBackColor = true;
            this.rotationRemoveButton.Click += new System.EventHandler(this.rotationRemoveButton_Click);
            // 
            // rotationAddButton
            // 
            this.rotationAddButton.Location = new System.Drawing.Point(119, 82);
            this.rotationAddButton.Name = "rotationAddButton";
            this.rotationAddButton.Size = new System.Drawing.Size(56, 23);
            this.rotationAddButton.TabIndex = 11;
            this.rotationAddButton.Text = ">";
            this.rotationAddButton.UseVisualStyleBackColor = true;
            this.rotationAddButton.Click += new System.EventHandler(this.rotationAddButton_Click);
            // 
            // rotationClearButton
            // 
            this.rotationClearButton.Location = new System.Drawing.Point(119, 206);
            this.rotationClearButton.Name = "rotationClearButton";
            this.rotationClearButton.Size = new System.Drawing.Size(56, 23);
            this.rotationClearButton.TabIndex = 16;
            this.rotationClearButton.Text = "Clear";
            this.rotationClearButton.UseVisualStyleBackColor = true;
            this.rotationClearButton.Click += new System.EventHandler(this.rotationClearButton_Click);
            // 
            // rotationDownButton
            // 
            this.rotationDownButton.Location = new System.Drawing.Point(119, 177);
            this.rotationDownButton.Name = "rotationDownButton";
            this.rotationDownButton.Size = new System.Drawing.Size(56, 23);
            this.rotationDownButton.TabIndex = 15;
            this.rotationDownButton.Text = "Down";
            this.rotationDownButton.UseVisualStyleBackColor = true;
            this.rotationDownButton.Click += new System.EventHandler(this.rotationDownButton_Click);
            // 
            // rotationUpButton
            // 
            this.rotationUpButton.Location = new System.Drawing.Point(119, 148);
            this.rotationUpButton.Name = "rotationUpButton";
            this.rotationUpButton.Size = new System.Drawing.Size(56, 23);
            this.rotationUpButton.TabIndex = 14;
            this.rotationUpButton.Text = "Up";
            this.rotationUpButton.UseVisualStyleBackColor = true;
            this.rotationUpButton.Click += new System.EventHandler(this.rotationUpButton_Click);
            // 
            // rotationList
            // 
            this.rotationList.FormattingEnabled = true;
            this.rotationList.Location = new System.Drawing.Point(181, 82);
            this.rotationList.Name = "rotationList";
            this.rotationList.Size = new System.Drawing.Size(107, 147);
            this.rotationList.TabIndex = 12;
            this.rotationList.SelectedIndexChanged += new System.EventHandler(this.rotationList_SelectedIndexChanged);
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
            // targetLevelCombo
            // 
            this.targetLevelCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.targetLevelCombo.FormattingEnabled = true;
            this.targetLevelCombo.Items.AddRange(new object[] {
            "83",
            "82",
            "81",
            "80"});
            this.targetLevelCombo.Location = new System.Drawing.Point(142, 53);
            this.targetLevelCombo.Name = "targetLevelCombo";
            this.targetLevelCombo.Size = new System.Drawing.Size(155, 21);
            this.targetLevelCombo.TabIndex = 3;
            this.targetLevelCombo.SelectedIndexChanged += new System.EventHandler(this.targetLevelCombo_SelectedIndexChanged);
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
            // fightLengthLabel
            // 
            this.fightLengthLabel.AutoSize = true;
            this.fightLengthLabel.Location = new System.Drawing.Point(3, 82);
            this.fightLengthLabel.Name = "fightLengthLabel";
            this.fightLengthLabel.Size = new System.Drawing.Size(95, 13);
            this.fightLengthLabel.TabIndex = 13;
            this.fightLengthLabel.Text = "Fight Length (sec):";
            // 
            // fightLengthSpinner
            // 
            this.fightLengthSpinner.Increment = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.fightLengthSpinner.Location = new System.Drawing.Point(142, 80);
            this.fightLengthSpinner.Maximum = new decimal(new int[] {
            720,
            0,
            0,
            0});
            this.fightLengthSpinner.Name = "fightLengthSpinner";
            this.fightLengthSpinner.Size = new System.Drawing.Size(155, 20);
            this.fightLengthSpinner.TabIndex = 4;
            this.fightLengthSpinner.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.fightLengthSpinner.ValueChanged += new System.EventHandler(this.fightLengthSpinner_ValueChanged);
            // 
            // latencySpinner
            // 
            this.latencySpinner.Increment = new decimal(new int[] {
            25,
            0,
            0,
            0});
            this.latencySpinner.Location = new System.Drawing.Point(142, 106);
            this.latencySpinner.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.latencySpinner.Name = "latencySpinner";
            this.latencySpinner.Size = new System.Drawing.Size(155, 20);
            this.latencySpinner.TabIndex = 5;
            this.latencySpinner.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.latencySpinner.ValueChanged += new System.EventHandler(this.latencySpinner_ValueChanged);
            // 
            // latencyLabel
            // 
            this.latencyLabel.AutoSize = true;
            this.latencyLabel.Location = new System.Drawing.Point(3, 108);
            this.latencyLabel.Name = "latencyLabel";
            this.latencyLabel.Size = new System.Drawing.Size(131, 13);
            this.latencyLabel.TabIndex = 16;
            this.latencyLabel.Text = "Time Between Spells (ms):";
            // 
            // infernalCheck
            // 
            this.infernalCheck.AutoSize = true;
            this.infernalCheck.Enabled = false;
            this.infernalCheck.Location = new System.Drawing.Point(142, 30);
            this.infernalCheck.Name = "infernalCheck";
            this.infernalCheck.Size = new System.Drawing.Size(106, 17);
            this.infernalCheck.TabIndex = 2;
            this.infernalCheck.Text = "Also Use Infernal";
            this.infernalCheck.UseVisualStyleBackColor = true;
            this.infernalCheck.CheckedChanged += new System.EventHandler(this.infernalCheck_CheckedChanged);
            // 
            // ProcCheckbox
            // 
            this.ProcCheckbox.AutoSize = true;
            this.ProcCheckbox.Location = new System.Drawing.Point(3, 418);
            this.ProcCheckbox.Name = "ProcCheckbox";
            this.ProcCheckbox.Size = new System.Drawing.Size(132, 17);
            this.ProcCheckbox.TabIndex = 18;
            this.ProcCheckbox.Text = "Disable special effects";
            this.ProcCheckbox.UseVisualStyleBackColor = true;
            this.ProcCheckbox.CheckedChanged += new System.EventHandler(this.procCheckbox_CheckedChanged);
            // 
            // CalculationOptionsPanelWarlock
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ProcCheckbox);
            this.Controls.Add(this.infernalCheck);
            this.Controls.Add(this.latencyLabel);
            this.Controls.Add(this.latencySpinner);
            this.Controls.Add(this.fightLengthSpinner);
            this.Controls.Add(this.fightLengthLabel);
            this.Controls.Add(this.targetLevelLabel);
            this.Controls.Add(this.targetLevelCombo);
            this.Controls.Add(this.petCombo);
            this.Controls.Add(this.petLabel);
            this.Controls.Add(this.rotationBox);
            this.Name = "CalculationOptionsPanelWarlock";
            this.Size = new System.Drawing.Size(300, 541);
            this.rotationBox.ResumeLayout(false);
            this.rotationBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) (this.fightLengthSpinner)).EndInit();
            ((System.ComponentModel.ISupportInitialize) (this.latencySpinner)).EndInit();
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
        private System.Windows.Forms.Button rotationAddButton;
        private System.Windows.Forms.ComboBox targetLevelCombo;
        private System.Windows.Forms.Label targetLevelLabel;
        private System.Windows.Forms.Label fightLengthLabel;
        private System.Windows.Forms.NumericUpDown fightLengthSpinner;
        private System.Windows.Forms.NumericUpDown latencySpinner;
        private System.Windows.Forms.Label latencyLabel;
        private System.Windows.Forms.CheckBox infernalCheck;
        private System.Windows.Forms.Panel rotationSeparator;
        private System.Windows.Forms.Button rotationRenameButton;
        private System.Windows.Forms.Label rotationErrorLabel;
        private System.Windows.Forms.ListBox rotationMenu;
        private System.Windows.Forms.Label fillerLabel;
        private System.Windows.Forms.ComboBox fillerCombo;
        private System.Windows.Forms.CheckBox ProcCheckbox;
    }
}
