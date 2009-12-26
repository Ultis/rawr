namespace Rawr.Warlock
{
    partial class SpellPriorityForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SpellPriorityForm));
            this.cmbSpells = new System.Windows.Forms.ComboBox();
            this.lsSpellPriority = new System.Windows.Forms.ListBox();
            this.bAdd = new System.Windows.Forms.Button();
            this.bUp = new System.Windows.Forms.Button();
            this.bDown = new System.Windows.Forms.Button();
            this.bRemove = new System.Windows.Forms.Button();
            this.bClear = new System.Windows.Forms.Button();
            this.bSave = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cmbSpells
            // 
            this.cmbSpells.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbSpells.FormattingEnabled = true;
            this.cmbSpells.Location = new System.Drawing.Point(12, 12);
            this.cmbSpells.Name = "cmbSpells";
            this.cmbSpells.Size = new System.Drawing.Size(192, 21);
            this.cmbSpells.TabIndex = 0;
            // 
            // lsSpellPriority
            // 
            this.lsSpellPriority.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lsSpellPriority.FormattingEnabled = true;
            this.lsSpellPriority.Location = new System.Drawing.Point(12, 39);
            this.lsSpellPriority.Name = "lsSpellPriority";
            this.lsSpellPriority.Size = new System.Drawing.Size(192, 147);
            this.lsSpellPriority.TabIndex = 1;
            // 
            // bAdd
            // 
            this.bAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bAdd.Location = new System.Drawing.Point(210, 10);
            this.bAdd.Name = "bAdd";
            this.bAdd.Size = new System.Drawing.Size(75, 23);
            this.bAdd.TabIndex = 2;
            this.bAdd.Text = "Add";
            this.bAdd.UseVisualStyleBackColor = true;
            this.bAdd.Click += new System.EventHandler(this.bAdd_Click);
            // 
            // bUp
            // 
            this.bUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bUp.Location = new System.Drawing.Point(210, 42);
            this.bUp.Name = "bUp";
            this.bUp.Size = new System.Drawing.Size(75, 23);
            this.bUp.TabIndex = 3;
            this.bUp.Text = "Up";
            this.bUp.UseVisualStyleBackColor = true;
            this.bUp.Click += new System.EventHandler(this.bUp_Click);
            // 
            // bDown
            // 
            this.bDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bDown.Location = new System.Drawing.Point(210, 71);
            this.bDown.Name = "bDown";
            this.bDown.Size = new System.Drawing.Size(75, 23);
            this.bDown.TabIndex = 4;
            this.bDown.Text = "Down";
            this.bDown.UseVisualStyleBackColor = true;
            this.bDown.Click += new System.EventHandler(this.bDown_Click);
            // 
            // bRemove
            // 
            this.bRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bRemove.Location = new System.Drawing.Point(210, 100);
            this.bRemove.Name = "bRemove";
            this.bRemove.Size = new System.Drawing.Size(75, 23);
            this.bRemove.TabIndex = 5;
            this.bRemove.Text = "Remove";
            this.bRemove.UseVisualStyleBackColor = true;
            this.bRemove.Click += new System.EventHandler(this.bRemove_Click);
            // 
            // bClear
            // 
            this.bClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bClear.Location = new System.Drawing.Point(210, 129);
            this.bClear.Name = "bClear";
            this.bClear.Size = new System.Drawing.Size(75, 23);
            this.bClear.TabIndex = 6;
            this.bClear.Text = "Clear";
            this.bClear.UseVisualStyleBackColor = true;
            this.bClear.Click += new System.EventHandler(this.bClear_Click);
            // 
            // bSave
            // 
            this.bSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bSave.Location = new System.Drawing.Point(210, 157);
            this.bSave.Name = "bSave";
            this.bSave.Size = new System.Drawing.Size(75, 23);
            this.bSave.TabIndex = 7;
            this.bSave.Text = "Save";
            this.bSave.UseVisualStyleBackColor = true;
            this.bSave.Click += new System.EventHandler(this.bSave_Click);
            // 
            // SpellPriorityForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(297, 199);
            this.Controls.Add(this.bSave);
            this.Controls.Add(this.bClear);
            this.Controls.Add(this.bRemove);
            this.Controls.Add(this.bDown);
            this.Controls.Add(this.bUp);
            this.Controls.Add(this.bAdd);
            this.Controls.Add(this.lsSpellPriority);
            this.Controls.Add(this.cmbSpells);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "SpellPriorityForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Spell Priority";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbSpells;
        private System.Windows.Forms.ListBox lsSpellPriority;
        private System.Windows.Forms.Button bAdd;
        private System.Windows.Forms.Button bUp;
        private System.Windows.Forms.Button bDown;
        private System.Windows.Forms.Button bRemove;
        private System.Windows.Forms.Button bClear;
        private System.Windows.Forms.Button bSave;
    }
}