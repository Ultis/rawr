namespace Rawr
{
    partial class FormBatchTools
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
            this.components = new System.ComponentModel.Container();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setAvailableItemsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optimizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.buildUpgradeListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.saveCharactersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveCharactersAsCopyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.characterColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.weightColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.scoreColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.newScoreColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.loadBatchCharacterColumn = new Rawr.FormBatchTools.MyDataGridViewButtonColumn();
            this.showBatchCharacterColumn = new Rawr.FormBatchTools.MyDataGridViewButtonColumn();
            this.batchCharacterListBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.statusStrip1 = new Rawr.FormBatchTools.MyStatusStrip();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.panel1 = new System.Windows.Forms.Panel();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.trackBarMaxRounds = new System.Windows.Forms.TrackBar();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.checkBoxOverrideReenchant = new System.Windows.Forms.CheckBox();
            this.checkBoxOverrideRegem = new System.Windows.Forms.CheckBox();
            this.trackBarThoroughness = new System.Windows.Forms.TrackBar();
            this.replaceUnavailableToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.batchCharacterListBindingSource)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarMaxRounds)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarThoroughness)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.toolsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(630, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStripBatch";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.importToolStripMenuItem,
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(35, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.newToolStripMenuItem.Text = "&New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // importToolStripMenuItem
            // 
            this.importToolStripMenuItem.Name = "importToolStripMenuItem";
            this.importToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.importToolStripMenuItem.Text = "&Import";
            this.importToolStripMenuItem.Click += new System.EventHandler(this.importToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.openToolStripMenuItem.Text = "&Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.saveToolStripMenuItem.Text = "&Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.saveAsToolStripMenuItem.Text = "Save &As...";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(133, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.setAvailableItemsToolStripMenuItem,
            this.replaceUnavailableToolStripMenuItem,
            this.optimizeToolStripMenuItem,
            this.buildUpgradeListToolStripMenuItem,
            this.toolStripSeparator2,
            this.saveCharactersToolStripMenuItem,
            this.saveCharactersAsCopyToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.toolsToolStripMenuItem.Text = "&Tools";
            // 
            // setAvailableItemsToolStripMenuItem
            // 
            this.setAvailableItemsToolStripMenuItem.Name = "setAvailableItemsToolStripMenuItem";
            this.setAvailableItemsToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.setAvailableItemsToolStripMenuItem.Text = "Set Available Items";
            this.setAvailableItemsToolStripMenuItem.Click += new System.EventHandler(this.setAvailableGearToolStripMenuItem_Click);
            // 
            // optimizeToolStripMenuItem
            // 
            this.optimizeToolStripMenuItem.Name = "optimizeToolStripMenuItem";
            this.optimizeToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.optimizeToolStripMenuItem.Text = "Optimize";
            this.optimizeToolStripMenuItem.Click += new System.EventHandler(this.optimizeToolStripMenuItem_Click);
            // 
            // buildUpgradeListToolStripMenuItem
            // 
            this.buildUpgradeListToolStripMenuItem.Name = "buildUpgradeListToolStripMenuItem";
            this.buildUpgradeListToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.buildUpgradeListToolStripMenuItem.Text = "Build Upgrade List";
            this.buildUpgradeListToolStripMenuItem.Click += new System.EventHandler(this.buildUpgradeListToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(205, 6);
            // 
            // saveCharactersToolStripMenuItem
            // 
            this.saveCharactersToolStripMenuItem.Name = "saveCharactersToolStripMenuItem";
            this.saveCharactersToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.saveCharactersToolStripMenuItem.Text = "Save Characters";
            this.saveCharactersToolStripMenuItem.Click += new System.EventHandler(this.saveCharactersToolStripMenuItem_Click);
            // 
            // saveCharactersAsCopyToolStripMenuItem
            // 
            this.saveCharactersAsCopyToolStripMenuItem.Name = "saveCharactersAsCopyToolStripMenuItem";
            this.saveCharactersAsCopyToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.saveCharactersAsCopyToolStripMenuItem.Text = "Save Characters As Copy";
            this.saveCharactersAsCopyToolStripMenuItem.Click += new System.EventHandler(this.saveCharactersAsCopyToolStripMenuItem_Click);
            // 
            // dataGridView
            // 
            this.dataGridView.AutoGenerateColumns = false;
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.characterColumn,
            this.weightColumn,
            this.scoreColumn,
            this.newScoreColumn,
            this.loadBatchCharacterColumn,
            this.showBatchCharacterColumn});
            this.dataGridView.DataSource = this.batchCharacterListBindingSource;
            this.dataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView.Location = new System.Drawing.Point(0, 24);
            this.dataGridView.Margin = new System.Windows.Forms.Padding(0);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView.Size = new System.Drawing.Size(462, 336);
            this.dataGridView.TabIndex = 1;
            this.dataGridView.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_CellClick);
            // 
            // characterColumn
            // 
            this.characterColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.characterColumn.DataPropertyName = "Name";
            this.characterColumn.HeaderText = "Character";
            this.characterColumn.Name = "characterColumn";
            this.characterColumn.ReadOnly = true;
            this.characterColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // weightColumn
            // 
            this.weightColumn.DataPropertyName = "Weight";
            this.weightColumn.HeaderText = "Weight";
            this.weightColumn.Name = "weightColumn";
            this.weightColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.weightColumn.Width = 50;
            // 
            // scoreColumn
            // 
            this.scoreColumn.DataPropertyName = "Score";
            this.scoreColumn.HeaderText = "Score";
            this.scoreColumn.Name = "scoreColumn";
            this.scoreColumn.ReadOnly = true;
            this.scoreColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.scoreColumn.Width = 66;
            // 
            // newScoreColumn
            // 
            this.newScoreColumn.DataPropertyName = "NewScore";
            this.newScoreColumn.HeaderText = "New Score";
            this.newScoreColumn.Name = "newScoreColumn";
            this.newScoreColumn.ReadOnly = true;
            this.newScoreColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.newScoreColumn.Width = 66;
            // 
            // loadBatchCharacterColumn
            // 
            this.loadBatchCharacterColumn.DataPropertyName = "RelativePath";
            this.loadBatchCharacterColumn.HeaderText = "";
            this.loadBatchCharacterColumn.Name = "loadBatchCharacterColumn";
            this.loadBatchCharacterColumn.NewRowButtonVisible = true;
            this.loadBatchCharacterColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.loadBatchCharacterColumn.Text = "...";
            this.loadBatchCharacterColumn.UseColumnTextForButtonValue = true;
            this.loadBatchCharacterColumn.Width = 20;
            // 
            // showBatchCharacterColumn
            // 
            this.showBatchCharacterColumn.HeaderText = "";
            this.showBatchCharacterColumn.Name = "showBatchCharacterColumn";
            this.showBatchCharacterColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.showBatchCharacterColumn.Text = "Show";
            this.showBatchCharacterColumn.UseColumnTextForButtonValue = true;
            this.showBatchCharacterColumn.Width = 50;
            // 
            // batchCharacterListBindingSource
            // 
            this.batchCharacterListBindingSource.DataSource = typeof(Rawr.BatchCharacterList);
            this.batchCharacterListBindingSource.ListChanged += new System.ComponentModel.ListChangedEventHandler(this.batchCharacterListBindingSource_ListChanged);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel,
            this.statusProgressBar});
            this.statusStrip1.Location = new System.Drawing.Point(0, 360);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(630, 22);
            this.statusStrip1.TabIndex = 2;
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = false;
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(250, 17);
            this.statusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // statusProgressBar
            // 
            this.statusProgressBar.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.statusProgressBar.Name = "statusProgressBar";
            this.statusProgressBar.Size = new System.Drawing.Size(100, 16);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.buttonCancel);
            this.panel1.Controls.Add(this.trackBarMaxRounds);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.checkBoxOverrideReenchant);
            this.panel1.Controls.Add(this.checkBoxOverrideRegem);
            this.panel1.Controls.Add(this.trackBarThoroughness);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(462, 24);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(168, 336);
            this.panel1.TabIndex = 3;
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonCancel.Enabled = false;
            this.buttonCancel.Location = new System.Drawing.Point(90, 310);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 14;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // trackBarMaxRounds
            // 
            this.trackBarMaxRounds.LargeChange = 1;
            this.trackBarMaxRounds.Location = new System.Drawing.Point(79, 122);
            this.trackBarMaxRounds.Maximum = 5;
            this.trackBarMaxRounds.Minimum = 1;
            this.trackBarMaxRounds.Name = "trackBarMaxRounds";
            this.trackBarMaxRounds.Size = new System.Drawing.Size(77, 45);
            this.trackBarMaxRounds.TabIndex = 13;
            this.trackBarMaxRounds.Value = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 129);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "Max Rounds:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "Thoroughness:";
            // 
            // checkBoxOverrideReenchant
            // 
            this.checkBoxOverrideReenchant.AutoSize = true;
            this.checkBoxOverrideReenchant.Location = new System.Drawing.Point(6, 99);
            this.checkBoxOverrideReenchant.Name = "checkBoxOverrideReenchant";
            this.checkBoxOverrideReenchant.Size = new System.Drawing.Size(122, 17);
            this.checkBoxOverrideReenchant.TabIndex = 10;
            this.checkBoxOverrideReenchant.Text = "Override Reenchant";
            this.checkBoxOverrideReenchant.UseVisualStyleBackColor = true;
            // 
            // checkBoxOverrideRegem
            // 
            this.checkBoxOverrideRegem.AutoSize = true;
            this.checkBoxOverrideRegem.Location = new System.Drawing.Point(6, 76);
            this.checkBoxOverrideRegem.Name = "checkBoxOverrideRegem";
            this.checkBoxOverrideRegem.Size = new System.Drawing.Size(103, 17);
            this.checkBoxOverrideRegem.TabIndex = 9;
            this.checkBoxOverrideRegem.Text = "Override Regem";
            this.checkBoxOverrideRegem.UseVisualStyleBackColor = true;
            // 
            // trackBarThoroughness
            // 
            this.trackBarThoroughness.Location = new System.Drawing.Point(6, 25);
            this.trackBarThoroughness.Maximum = 400;
            this.trackBarThoroughness.Minimum = 1;
            this.trackBarThoroughness.Name = "trackBarThoroughness";
            this.trackBarThoroughness.Size = new System.Drawing.Size(150, 45);
            this.trackBarThoroughness.TabIndex = 8;
            this.trackBarThoroughness.TickFrequency = 10;
            this.trackBarThoroughness.Value = 150;
            // 
            // replaceUnavailableToolStripMenuItem
            // 
            this.replaceUnavailableToolStripMenuItem.Name = "replaceUnavailableToolStripMenuItem";
            this.replaceUnavailableToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.replaceUnavailableToolStripMenuItem.Text = "Replace Unavailable";
            this.replaceUnavailableToolStripMenuItem.Click += new System.EventHandler(this.replaceUnavailableToolStripMenuItem_Click);
            // 
            // FormBatchTools
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(630, 382);
            this.Controls.Add(this.dataGridView);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FormBatchTools";
            this.Text = "Rawr Batch Tools";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormBatchTools_FormClosed);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormBatchTools_FormClosing);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.batchCharacterListBindingSource)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarMaxRounds)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarThoroughness)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.BindingSource batchCharacterListBindingSource;
        private System.Windows.Forms.ToolStripMenuItem importToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setAvailableItemsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveCharactersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveCharactersAsCopyToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private MyStatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;
        private System.Windows.Forms.ToolStripProgressBar statusProgressBar;
        private System.Windows.Forms.ToolStripMenuItem optimizeToolStripMenuItem;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox checkBoxOverrideReenchant;
        private System.Windows.Forms.CheckBox checkBoxOverrideRegem;
        private System.Windows.Forms.TrackBar trackBarThoroughness;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TrackBar trackBarMaxRounds;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.ToolStripMenuItem buildUpgradeListToolStripMenuItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn characterColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn weightColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn scoreColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn newScoreColumn;
        private FormBatchTools.MyDataGridViewButtonColumn loadBatchCharacterColumn;
        private FormBatchTools.MyDataGridViewButtonColumn showBatchCharacterColumn;
        private System.Windows.Forms.ToolStripMenuItem replaceUnavailableToolStripMenuItem;
    }
}