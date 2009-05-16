namespace Rawr
{
    partial class FormItemFilter
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
            this.OKButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.bindingSourceItemFilter = new System.Windows.Forms.BindingSource(this.components);
            this.labelName = new System.Windows.Forms.Label();
            this.textBoxName = new System.Windows.Forms.TextBox();
            this.textBoxPattern = new System.Windows.Forms.TextBox();
            this.labelPattern = new System.Windows.Forms.Label();
            this.textBoxMinItemLevel = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxMaxItemLevel = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBoxMinItemQuality = new System.Windows.Forms.ComboBox();
            this.comboBoxMaxItemQuality = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.checkBoxAdditiveFilter = new System.Windows.Forms.CheckBox();
            this.checkBoxAppliesToItems = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.checkBoxAppliesToGems = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.buttonNewFilter = new System.Windows.Forms.Button();
            this.buttonNewSubfilter = new System.Windows.Forms.Button();
            this.buttonUp = new System.Windows.Forms.Button();
            this.buttonDown = new System.Windows.Forms.Button();
            this.buttonDelete = new System.Windows.Forms.Button();
            this.itemFilterTreeView = new Rawr.ItemFilterTreeView();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceItemFilter)).BeginInit();
            this.SuspendLayout();
            // 
            // OKButton
            // 
            this.OKButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OKButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OKButton.Location = new System.Drawing.Point(167, 484);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(75, 25);
            this.OKButton.TabIndex = 5;
            this.OKButton.Text = "OK";
            this.OKButton.UseVisualStyleBackColor = true;
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(86, 484);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 25);
            this.cancelButton.TabIndex = 4;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // bindingSourceItemFilter
            // 
            this.bindingSourceItemFilter.DataSource = typeof(Rawr.ItemFilterRegex);
            this.bindingSourceItemFilter.CurrentItemChanged += new System.EventHandler(this.bindingSourceItemFilter_CurrentItemChanged);
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Location = new System.Drawing.Point(12, 217);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(38, 13);
            this.labelName.TabIndex = 7;
            this.labelName.Text = "Name:";
            // 
            // textBoxName
            // 
            this.textBoxName.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSourceItemFilter, "Name", true));
            this.textBoxName.Location = new System.Drawing.Point(57, 214);
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.Size = new System.Drawing.Size(185, 20);
            this.textBoxName.TabIndex = 8;
            // 
            // textBoxPattern
            // 
            this.textBoxPattern.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSourceItemFilter, "Pattern", true));
            this.textBoxPattern.Location = new System.Drawing.Point(57, 240);
            this.textBoxPattern.Multiline = true;
            this.textBoxPattern.Name = "textBoxPattern";
            this.textBoxPattern.Size = new System.Drawing.Size(185, 70);
            this.textBoxPattern.TabIndex = 10;
            // 
            // labelPattern
            // 
            this.labelPattern.AutoSize = true;
            this.labelPattern.Location = new System.Drawing.Point(12, 243);
            this.labelPattern.Name = "labelPattern";
            this.labelPattern.Size = new System.Drawing.Size(44, 13);
            this.labelPattern.TabIndex = 9;
            this.labelPattern.Text = "Pattern:";
            // 
            // textBoxMinItemLevel
            // 
            this.textBoxMinItemLevel.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSourceItemFilter, "MinItemLevel", true));
            this.textBoxMinItemLevel.Location = new System.Drawing.Point(134, 316);
            this.textBoxMinItemLevel.Name = "textBoxMinItemLevel";
            this.textBoxMinItemLevel.Size = new System.Drawing.Size(108, 20);
            this.textBoxMinItemLevel.TabIndex = 14;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 319);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 13);
            this.label1.TabIndex = 13;
            this.label1.Text = "Min Item Level:";
            // 
            // textBoxMaxItemLevel
            // 
            this.textBoxMaxItemLevel.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSourceItemFilter, "MaxItemLevel", true));
            this.textBoxMaxItemLevel.Location = new System.Drawing.Point(134, 342);
            this.textBoxMaxItemLevel.Name = "textBoxMaxItemLevel";
            this.textBoxMaxItemLevel.Size = new System.Drawing.Size(108, 20);
            this.textBoxMaxItemLevel.TabIndex = 16;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 345);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 13);
            this.label2.TabIndex = 15;
            this.label2.Text = "Max Item Level:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 371);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(85, 13);
            this.label3.TabIndex = 17;
            this.label3.Text = "Min Item Quality:";
            // 
            // comboBoxMinItemQuality
            // 
            this.comboBoxMinItemQuality.DataBindings.Add(new System.Windows.Forms.Binding("SelectedItem", this.bindingSourceItemFilter, "MinItemQuality", true));
            this.comboBoxMinItemQuality.FormattingEnabled = true;
            this.comboBoxMinItemQuality.Location = new System.Drawing.Point(134, 368);
            this.comboBoxMinItemQuality.Name = "comboBoxMinItemQuality";
            this.comboBoxMinItemQuality.Size = new System.Drawing.Size(108, 21);
            this.comboBoxMinItemQuality.TabIndex = 18;
            // 
            // comboBoxMaxItemQuality
            // 
            this.comboBoxMaxItemQuality.DataBindings.Add(new System.Windows.Forms.Binding("SelectedItem", this.bindingSourceItemFilter, "MaxItemQuality", true));
            this.comboBoxMaxItemQuality.FormattingEnabled = true;
            this.comboBoxMaxItemQuality.Location = new System.Drawing.Point(134, 395);
            this.comboBoxMaxItemQuality.Name = "comboBoxMaxItemQuality";
            this.comboBoxMaxItemQuality.Size = new System.Drawing.Size(108, 21);
            this.comboBoxMaxItemQuality.TabIndex = 20;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 398);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(88, 13);
            this.label4.TabIndex = 19;
            this.label4.Text = "Max Item Quality:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 422);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(73, 13);
            this.label5.TabIndex = 21;
            this.label5.Text = "Additive Filter:";
            // 
            // checkBoxAdditiveFilter
            // 
            this.checkBoxAdditiveFilter.AutoSize = true;
            this.checkBoxAdditiveFilter.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.bindingSourceItemFilter, "AdditiveFilter", true));
            this.checkBoxAdditiveFilter.Location = new System.Drawing.Point(227, 422);
            this.checkBoxAdditiveFilter.Name = "checkBoxAdditiveFilter";
            this.checkBoxAdditiveFilter.Size = new System.Drawing.Size(15, 14);
            this.checkBoxAdditiveFilter.TabIndex = 22;
            this.checkBoxAdditiveFilter.UseVisualStyleBackColor = true;
            // 
            // checkBoxAppliesToItems
            // 
            this.checkBoxAppliesToItems.AutoSize = true;
            this.checkBoxAppliesToItems.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.bindingSourceItemFilter, "AppliesToItems", true));
            this.checkBoxAppliesToItems.Location = new System.Drawing.Point(227, 442);
            this.checkBoxAppliesToItems.Name = "checkBoxAppliesToItems";
            this.checkBoxAppliesToItems.Size = new System.Drawing.Size(15, 14);
            this.checkBoxAppliesToItems.TabIndex = 24;
            this.checkBoxAppliesToItems.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 442);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(84, 13);
            this.label6.TabIndex = 23;
            this.label6.Text = "Applies to Items:";
            // 
            // checkBoxAppliesToGems
            // 
            this.checkBoxAppliesToGems.AutoSize = true;
            this.checkBoxAppliesToGems.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.bindingSourceItemFilter, "AppliesToGems", true));
            this.checkBoxAppliesToGems.Location = new System.Drawing.Point(227, 462);
            this.checkBoxAppliesToGems.Name = "checkBoxAppliesToGems";
            this.checkBoxAppliesToGems.Size = new System.Drawing.Size(15, 14);
            this.checkBoxAppliesToGems.TabIndex = 26;
            this.checkBoxAppliesToGems.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 462);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(86, 13);
            this.label7.TabIndex = 25;
            this.label7.Text = "Applies to Gems:";
            // 
            // buttonNewFilter
            // 
            this.buttonNewFilter.Location = new System.Drawing.Point(12, 10);
            this.buttonNewFilter.Name = "buttonNewFilter";
            this.buttonNewFilter.Size = new System.Drawing.Size(63, 23);
            this.buttonNewFilter.TabIndex = 28;
            this.buttonNewFilter.Text = "New Filter";
            this.buttonNewFilter.UseVisualStyleBackColor = true;
            this.buttonNewFilter.Click += new System.EventHandler(this.buttonNewFilter_Click);
            // 
            // buttonNewSubfilter
            // 
            this.buttonNewSubfilter.Enabled = false;
            this.buttonNewSubfilter.Location = new System.Drawing.Point(81, 10);
            this.buttonNewSubfilter.Name = "buttonNewSubfilter";
            this.buttonNewSubfilter.Size = new System.Drawing.Size(80, 23);
            this.buttonNewSubfilter.TabIndex = 29;
            this.buttonNewSubfilter.Text = "New Subfilter";
            this.buttonNewSubfilter.UseVisualStyleBackColor = true;
            this.buttonNewSubfilter.Click += new System.EventHandler(this.buttonNewSubfilter_Click);
            // 
            // buttonUp
            // 
            this.buttonUp.Enabled = false;
            this.buttonUp.Location = new System.Drawing.Point(167, 10);
            this.buttonUp.Name = "buttonUp";
            this.buttonUp.Size = new System.Drawing.Size(23, 23);
            this.buttonUp.TabIndex = 30;
            this.buttonUp.Text = "↑";
            this.buttonUp.UseVisualStyleBackColor = true;
            this.buttonUp.Click += new System.EventHandler(this.buttonUp_Click);
            // 
            // buttonDown
            // 
            this.buttonDown.Enabled = false;
            this.buttonDown.Location = new System.Drawing.Point(194, 10);
            this.buttonDown.Name = "buttonDown";
            this.buttonDown.Size = new System.Drawing.Size(23, 23);
            this.buttonDown.TabIndex = 31;
            this.buttonDown.Text = "↓";
            this.buttonDown.UseVisualStyleBackColor = true;
            this.buttonDown.Click += new System.EventHandler(this.buttonDown_Click);
            // 
            // buttonDelete
            // 
            this.buttonDelete.Enabled = false;
            this.buttonDelete.Location = new System.Drawing.Point(220, 10);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(23, 23);
            this.buttonDelete.TabIndex = 32;
            this.buttonDelete.Text = "✕";
            this.buttonDelete.UseVisualStyleBackColor = true;
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // itemFilterTreeView
            // 
            this.itemFilterTreeView.EditMode = true;
            this.itemFilterTreeView.HideSelection = false;
            this.itemFilterTreeView.Location = new System.Drawing.Point(15, 39);
            this.itemFilterTreeView.Name = "itemFilterTreeView";
            this.itemFilterTreeView.Size = new System.Drawing.Size(227, 169);
            this.itemFilterTreeView.TabIndex = 27;
            this.itemFilterTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.itemFilterTreeView_AfterSelect);
            // 
            // FormItemFilter
            // 
            this.AcceptButton = this.OKButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(254, 521);
            this.Controls.Add(this.buttonDelete);
            this.Controls.Add(this.buttonDown);
            this.Controls.Add(this.buttonUp);
            this.Controls.Add(this.buttonNewSubfilter);
            this.Controls.Add(this.buttonNewFilter);
            this.Controls.Add(this.itemFilterTreeView);
            this.Controls.Add(this.checkBoxAppliesToGems);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.checkBoxAppliesToItems);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.checkBoxAdditiveFilter);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.comboBoxMaxItemQuality);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.comboBoxMinItemQuality);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBoxMaxItemLevel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxMinItemLevel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxPattern);
            this.Controls.Add(this.labelPattern);
            this.Controls.Add(this.textBoxName);
            this.Controls.Add(this.labelName);
            this.Controls.Add(this.OKButton);
            this.Controls.Add(this.cancelButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "FormItemFilter";
            this.Text = "Item Filter Editor...";
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceItemFilter)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button OKButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.TextBox textBoxName;
        private System.Windows.Forms.TextBox textBoxPattern;
        private System.Windows.Forms.Label labelPattern;
        public System.Windows.Forms.BindingSource bindingSourceItemFilter;
        private System.Windows.Forms.TextBox textBoxMinItemLevel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxMaxItemLevel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBoxMinItemQuality;
        private System.Windows.Forms.ComboBox comboBoxMaxItemQuality;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox checkBoxAdditiveFilter;
        private System.Windows.Forms.CheckBox checkBoxAppliesToItems;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox checkBoxAppliesToGems;
        private System.Windows.Forms.Label label7;
        internal ItemFilterTreeView itemFilterTreeView;
        private System.Windows.Forms.Button buttonNewFilter;
        private System.Windows.Forms.Button buttonNewSubfilter;
        private System.Windows.Forms.Button buttonUp;
        private System.Windows.Forms.Button buttonDown;
        private System.Windows.Forms.Button buttonDelete;
    }
}