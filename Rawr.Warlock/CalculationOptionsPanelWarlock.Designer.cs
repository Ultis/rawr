namespace Rawr.Warlock
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CalculationOptionsPanelWarlock));
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
            this.tabbedPane = new System.Windows.Forms.TabControl();
            this.notesPage = new System.Windows.Forms.TabPage();
            this.notesBox = new System.Windows.Forms.TextBox();
            this.optionsPage = new System.Windows.Forms.TabPage();
            this.imbueLabel = new System.Windows.Forms.Label();
            this.imbueCombo = new System.Windows.Forms.ComboBox();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.TimerButton = new System.Windows.Forms.Button();
            this.ProcCheckbox = new System.Windows.Forms.CheckBox();
            this.rotationBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.fightLengthSpinner)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.latencySpinner)).BeginInit();
            this.tabbedPane.SuspendLayout();
            this.notesPage.SuspendLayout();
            this.optionsPage.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // petCombo
            // 
            this.petCombo.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.petCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.petCombo.FormattingEnabled = true;
            this.petCombo.Location = new System.Drawing.Point(180, 6);
            this.petCombo.Name = "petCombo";
            this.petCombo.Size = new System.Drawing.Size(106, 21);
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
            this.rotationCombo.Size = new System.Drawing.Size(271, 21);
            this.rotationCombo.TabIndex = 7;
            this.rotationCombo.SelectedIndexChanged += new System.EventHandler(this.rotationCombo_SelectedIndexChanged);
            // 
            // newRotationButton
            // 
            this.newRotationButton.AutoSize = true;
            this.newRotationButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.newRotationButton.Location = new System.Drawing.Point(121, 46);
            this.newRotationButton.Name = "newRotationButton";
            this.newRotationButton.Size = new System.Drawing.Size(39, 23);
            this.newRotationButton.TabIndex = 8;
            this.newRotationButton.Text = "New";
            this.newRotationButton.UseVisualStyleBackColor = true;
            this.newRotationButton.Click += new System.EventHandler(this.newRotationButton_Click);
            // 
            // deleteRotationButton
            // 
            this.deleteRotationButton.AutoSize = true;
            this.deleteRotationButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.deleteRotationButton.Location = new System.Drawing.Point(229, 46);
            this.deleteRotationButton.Name = "deleteRotationButton";
            this.deleteRotationButton.Size = new System.Drawing.Size(48, 23);
            this.deleteRotationButton.TabIndex = 10;
            this.deleteRotationButton.Text = "Delete";
            this.deleteRotationButton.UseVisualStyleBackColor = true;
            this.deleteRotationButton.Click += new System.EventHandler(this.deleteRotationButton_Click);
            // 
            // rotationBox
            // 
            this.rotationBox.Controls.Add(this.fillerCombo);
            this.rotationBox.Controls.Add(this.fillerLabel);
            this.rotationBox.Controls.Add(this.rotationMenu);
            this.rotationBox.Controls.Add(this.newRotationButton);
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
            this.rotationBox.Location = new System.Drawing.Point(3, 162);
            this.rotationBox.Name = "rotationBox";
            this.rotationBox.Size = new System.Drawing.Size(283, 280);
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
            this.fillerCombo.Size = new System.Drawing.Size(234, 21);
            this.fillerCombo.TabIndex = 18;
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
            this.rotationMenu.TabIndex = 11;
            this.rotationMenu.SelectedIndexChanged += new System.EventHandler(this.rotationMenu_SelectedIndexChanged);
            // 
            // rotationErrorLabel
            // 
            this.rotationErrorLabel.ForeColor = System.Drawing.Color.Red;
            this.rotationErrorLabel.Location = new System.Drawing.Point(6, 259);
            this.rotationErrorLabel.Name = "rotationErrorLabel";
            this.rotationErrorLabel.Size = new System.Drawing.Size(271, 13);
            this.rotationErrorLabel.TabIndex = 19;
            this.rotationErrorLabel.Text = "Error Label";
            // 
            // rotationRenameButton
            // 
            this.rotationRenameButton.AutoSize = true;
            this.rotationRenameButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.rotationRenameButton.Location = new System.Drawing.Point(166, 46);
            this.rotationRenameButton.Name = "rotationRenameButton";
            this.rotationRenameButton.Size = new System.Drawing.Size(57, 23);
            this.rotationRenameButton.TabIndex = 9;
            this.rotationRenameButton.Text = "Rename";
            this.rotationRenameButton.UseVisualStyleBackColor = true;
            this.rotationRenameButton.Click += new System.EventHandler(this.rotationRenameButton_Click);
            // 
            // rotationSeparator
            // 
            this.rotationSeparator.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.rotationSeparator.BackColor = System.Drawing.SystemColors.ControlDark;
            this.rotationSeparator.Location = new System.Drawing.Point(6, 75);
            this.rotationSeparator.Name = "rotationSeparator";
            this.rotationSeparator.Size = new System.Drawing.Size(271, 1);
            this.rotationSeparator.TabIndex = 14;
            // 
            // rotationRemoveButton
            // 
            this.rotationRemoveButton.Location = new System.Drawing.Point(119, 111);
            this.rotationRemoveButton.Name = "rotationRemoveButton";
            this.rotationRemoveButton.Size = new System.Drawing.Size(45, 23);
            this.rotationRemoveButton.TabIndex = 14;
            this.rotationRemoveButton.Text = "<";
            this.rotationRemoveButton.UseVisualStyleBackColor = true;
            this.rotationRemoveButton.Click += new System.EventHandler(this.rotationRemoveButton_Click);
            // 
            // rotationAddButton
            // 
            this.rotationAddButton.Location = new System.Drawing.Point(119, 82);
            this.rotationAddButton.Name = "rotationAddButton";
            this.rotationAddButton.Size = new System.Drawing.Size(45, 23);
            this.rotationAddButton.TabIndex = 12;
            this.rotationAddButton.Text = ">";
            this.rotationAddButton.UseVisualStyleBackColor = true;
            this.rotationAddButton.Click += new System.EventHandler(this.rotationAddButton_Click);
            // 
            // rotationClearButton
            // 
            this.rotationClearButton.Location = new System.Drawing.Point(119, 206);
            this.rotationClearButton.Name = "rotationClearButton";
            this.rotationClearButton.Size = new System.Drawing.Size(45, 23);
            this.rotationClearButton.TabIndex = 17;
            this.rotationClearButton.Text = "Clear";
            this.rotationClearButton.UseVisualStyleBackColor = true;
            this.rotationClearButton.Click += new System.EventHandler(this.rotationClearButton_Click);
            // 
            // rotationDownButton
            // 
            this.rotationDownButton.Location = new System.Drawing.Point(119, 177);
            this.rotationDownButton.Name = "rotationDownButton";
            this.rotationDownButton.Size = new System.Drawing.Size(45, 23);
            this.rotationDownButton.TabIndex = 16;
            this.rotationDownButton.Text = "Down";
            this.rotationDownButton.UseVisualStyleBackColor = true;
            this.rotationDownButton.Click += new System.EventHandler(this.rotationDownButton_Click);
            // 
            // rotationUpButton
            // 
            this.rotationUpButton.Location = new System.Drawing.Point(119, 148);
            this.rotationUpButton.Name = "rotationUpButton";
            this.rotationUpButton.Size = new System.Drawing.Size(45, 23);
            this.rotationUpButton.TabIndex = 15;
            this.rotationUpButton.Text = "Up";
            this.rotationUpButton.UseVisualStyleBackColor = true;
            this.rotationUpButton.Click += new System.EventHandler(this.rotationUpButton_Click);
            // 
            // rotationList
            // 
            this.rotationList.FormattingEnabled = true;
            this.rotationList.Location = new System.Drawing.Point(170, 82);
            this.rotationList.Name = "rotationList";
            this.rotationList.Size = new System.Drawing.Size(107, 147);
            this.rotationList.TabIndex = 13;
            this.rotationList.SelectedIndexChanged += new System.EventHandler(this.rotationList_SelectedIndexChanged);
            // 
            // petLabel
            // 
            this.petLabel.AutoSize = true;
            this.petLabel.Location = new System.Drawing.Point(6, 9);
            this.petLabel.Name = "petLabel";
            this.petLabel.Size = new System.Drawing.Size(26, 13);
            this.petLabel.TabIndex = 2;
            this.petLabel.Text = "Pet:";
            // 
            // targetLevelCombo
            // 
            this.targetLevelCombo.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.targetLevelCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.targetLevelCombo.FormattingEnabled = true;
            this.targetLevelCombo.Items.AddRange(new object[] {
            "83",
            "82",
            "81",
            "80"});
            this.targetLevelCombo.Location = new System.Drawing.Point(180, 83);
            this.targetLevelCombo.Name = "targetLevelCombo";
            this.targetLevelCombo.Size = new System.Drawing.Size(106, 21);
            this.targetLevelCombo.TabIndex = 4;
            this.targetLevelCombo.SelectedIndexChanged += new System.EventHandler(this.targetLevelCombo_SelectedIndexChanged);
            // 
            // targetLevelLabel
            // 
            this.targetLevelLabel.AutoSize = true;
            this.targetLevelLabel.Location = new System.Drawing.Point(6, 86);
            this.targetLevelLabel.Name = "targetLevelLabel";
            this.targetLevelLabel.Size = new System.Drawing.Size(70, 13);
            this.targetLevelLabel.TabIndex = 9;
            this.targetLevelLabel.Text = "Target Level:";
            // 
            // fightLengthLabel
            // 
            this.fightLengthLabel.AutoSize = true;
            this.fightLengthLabel.Location = new System.Drawing.Point(6, 112);
            this.fightLengthLabel.Name = "fightLengthLabel";
            this.fightLengthLabel.Size = new System.Drawing.Size(95, 13);
            this.fightLengthLabel.TabIndex = 13;
            this.fightLengthLabel.Text = "Fight Length (sec):";
            // 
            // fightLengthSpinner
            // 
            this.fightLengthSpinner.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.fightLengthSpinner.Increment = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.fightLengthSpinner.Location = new System.Drawing.Point(180, 110);
            this.fightLengthSpinner.Maximum = new decimal(new int[] {
            720,
            0,
            0,
            0});
            this.fightLengthSpinner.Name = "fightLengthSpinner";
            this.fightLengthSpinner.Size = new System.Drawing.Size(106, 20);
            this.fightLengthSpinner.TabIndex = 5;
            this.fightLengthSpinner.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.fightLengthSpinner.ValueChanged += new System.EventHandler(this.fightLengthSpinner_ValueChanged);
            // 
            // latencySpinner
            // 
            this.latencySpinner.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.latencySpinner.Increment = new decimal(new int[] {
            25,
            0,
            0,
            0});
            this.latencySpinner.Location = new System.Drawing.Point(180, 136);
            this.latencySpinner.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.latencySpinner.Name = "latencySpinner";
            this.latencySpinner.Size = new System.Drawing.Size(106, 20);
            this.latencySpinner.TabIndex = 6;
            this.latencySpinner.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.latencySpinner.ValueChanged += new System.EventHandler(this.latencySpinner_ValueChanged);
            // 
            // latencyLabel
            // 
            this.latencyLabel.AutoSize = true;
            this.latencyLabel.Location = new System.Drawing.Point(6, 138);
            this.latencyLabel.Name = "latencyLabel";
            this.latencyLabel.Size = new System.Drawing.Size(131, 13);
            this.latencyLabel.TabIndex = 16;
            this.latencyLabel.Text = "Time Between Spells (ms):";
            // 
            // infernalCheck
            // 
            this.infernalCheck.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.infernalCheck.AutoSize = true;
            this.infernalCheck.Enabled = false;
            this.infernalCheck.Location = new System.Drawing.Point(180, 33);
            this.infernalCheck.Name = "infernalCheck";
            this.infernalCheck.Size = new System.Drawing.Size(106, 17);
            this.infernalCheck.TabIndex = 2;
            this.infernalCheck.Text = "Also Use Infernal";
            this.infernalCheck.UseVisualStyleBackColor = true;
            this.infernalCheck.CheckedChanged += new System.EventHandler(this.infernalCheck_CheckedChanged);
            // 
            // tabbedPane
            // 
            this.tabbedPane.Controls.Add(this.notesPage);
            this.tabbedPane.Controls.Add(this.optionsPage);
            this.tabbedPane.Controls.Add(this.tabPage1);
            this.tabbedPane.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabbedPane.Location = new System.Drawing.Point(0, 0);
            this.tabbedPane.Name = "tabbedPane";
            this.tabbedPane.SelectedIndex = 0;
            this.tabbedPane.Size = new System.Drawing.Size(300, 500);
            this.tabbedPane.TabIndex = 19;
            // 
            // notesPage
            // 
            this.notesPage.Controls.Add(this.notesBox);
            this.notesPage.Location = new System.Drawing.Point(4, 22);
            this.notesPage.Name = "notesPage";
            this.notesPage.Padding = new System.Windows.Forms.Padding(3);
            this.notesPage.Size = new System.Drawing.Size(292, 474);
            this.notesPage.TabIndex = 1;
            this.notesPage.Text = "Important";
            this.notesPage.UseVisualStyleBackColor = true;
            // 
            // notesBox
            // 
            this.notesBox.AcceptsReturn = true;
            this.notesBox.AcceptsTab = true;
            this.notesBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.notesBox.Location = new System.Drawing.Point(3, 3);
            this.notesBox.Multiline = true;
            this.notesBox.Name = "notesBox";
            this.notesBox.ReadOnly = true;
            this.notesBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.notesBox.Size = new System.Drawing.Size(286, 468);
            this.notesBox.TabIndex = 0;
            this.notesBox.Text = resources.GetString("notesBox.Text");
            // 
            // optionsPage
            // 
            this.optionsPage.Controls.Add(this.imbueLabel);
            this.optionsPage.Controls.Add(this.imbueCombo);
            this.optionsPage.Controls.Add(this.petCombo);
            this.optionsPage.Controls.Add(this.targetLevelCombo);
            this.optionsPage.Controls.Add(this.rotationBox);
            this.optionsPage.Controls.Add(this.latencyLabel);
            this.optionsPage.Controls.Add(this.infernalCheck);
            this.optionsPage.Controls.Add(this.fightLengthLabel);
            this.optionsPage.Controls.Add(this.fightLengthSpinner);
            this.optionsPage.Controls.Add(this.targetLevelLabel);
            this.optionsPage.Controls.Add(this.latencySpinner);
            this.optionsPage.Controls.Add(this.petLabel);
            this.optionsPage.Location = new System.Drawing.Point(4, 22);
            this.optionsPage.Name = "optionsPage";
            this.optionsPage.Padding = new System.Windows.Forms.Padding(3);
            this.optionsPage.Size = new System.Drawing.Size(292, 474);
            this.optionsPage.TabIndex = 0;
            this.optionsPage.Text = "Options";
            this.optionsPage.UseVisualStyleBackColor = true;
            // 
            // imbueLabel
            // 
            this.imbueLabel.AutoSize = true;
            this.imbueLabel.Location = new System.Drawing.Point(6, 59);
            this.imbueLabel.Name = "imbueLabel";
            this.imbueLabel.Size = new System.Drawing.Size(147, 13);
            this.imbueLabel.TabIndex = 18;
            this.imbueLabel.Text = "Temporary Weapon Enchant:";
            // 
            // imbueCombo
            // 
            this.imbueCombo.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.imbueCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.imbueCombo.FormattingEnabled = true;
            this.imbueCombo.Items.AddRange(new object[] {
            "Grand Spellstone",
            "Grand Firestone"});
            this.imbueCombo.Location = new System.Drawing.Point(180, 56);
            this.imbueCombo.Name = "imbueCombo";
            this.imbueCombo.Size = new System.Drawing.Size(106, 21);
            this.imbueCombo.TabIndex = 3;
            this.imbueCombo.SelectedIndexChanged += new System.EventHandler(this.imbueCombo_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.TimerButton);
            this.tabPage1.Controls.Add(this.ProcCheckbox);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(292, 474);
            this.tabPage1.TabIndex = 2;
            this.tabPage1.Text = "Debug";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // TimerButton
            // 
            this.TimerButton.Location = new System.Drawing.Point(6, 29);
            this.TimerButton.Name = "TimerButton";
            this.TimerButton.Size = new System.Drawing.Size(105, 23);
            this.TimerButton.TabIndex = 20;
            this.TimerButton.Text = "Time Computations";
            this.TimerButton.UseVisualStyleBackColor = true;
            this.TimerButton.Click += new System.EventHandler(this.TimerButton_Click);
            // 
            // ProcCheckbox
            // 
            this.ProcCheckbox.AutoSize = true;
            this.ProcCheckbox.Location = new System.Drawing.Point(6, 6);
            this.ProcCheckbox.Name = "ProcCheckbox";
            this.ProcCheckbox.Size = new System.Drawing.Size(132, 17);
            this.ProcCheckbox.TabIndex = 19;
            this.ProcCheckbox.Text = "Disable special effects";
            this.ProcCheckbox.UseVisualStyleBackColor = true;
            this.ProcCheckbox.CheckedChanged += new System.EventHandler(this.procCheckbox_CheckedChanged);
            // 
            // CalculationOptionsPanelWarlock
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabbedPane);
            this.Name = "CalculationOptionsPanelWarlock";
            this.Size = new System.Drawing.Size(300, 500);
            this.rotationBox.ResumeLayout(false);
            this.rotationBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) (this.fightLengthSpinner)).EndInit();
            ((System.ComponentModel.ISupportInitialize) (this.latencySpinner)).EndInit();
            this.tabbedPane.ResumeLayout(false);
            this.notesPage.ResumeLayout(false);
            this.notesPage.PerformLayout();
            this.optionsPage.ResumeLayout(false);
            this.optionsPage.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.ResumeLayout(false);

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
        private System.Windows.Forms.TabControl tabbedPane;
        private System.Windows.Forms.TabPage optionsPage;
        private System.Windows.Forms.TabPage notesPage;
        private System.Windows.Forms.TextBox notesBox;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.CheckBox ProcCheckbox;
        private System.Windows.Forms.Button TimerButton;
        private System.Windows.Forms.ComboBox imbueCombo;
        private System.Windows.Forms.Label imbueLabel;
    }
}
