namespace Rawr
{
    partial class FormMassGemReplacement
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
            this.label1 = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.OkButton = new System.Windows.Forms.Button();
            this.buttonAddGemming = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.removeButton1 = new System.Windows.Forms.Button();
            this.metaButton1 = new Rawr.ItemButton();
            this.blueButton1 = new Rawr.ItemButton();
            this.yellowButton1 = new Rawr.ItemButton();
            this.redButton1 = new Rawr.ItemButton();
            this.button7 = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.removeButton2 = new System.Windows.Forms.Button();
            this.metaButton2 = new Rawr.ItemButton();
            this.blueButton2 = new Rawr.ItemButton();
            this.yellowButton2 = new Rawr.ItemButton();
            this.redButton2 = new Rawr.ItemButton();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.removeButton3 = new System.Windows.Forms.Button();
            this.metaButton3 = new Rawr.ItemButton();
            this.blueButton3 = new Rawr.ItemButton();
            this.yellowButton3 = new Rawr.ItemButton();
            this.redButton3 = new Rawr.ItemButton();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.removeButton4 = new System.Windows.Forms.Button();
            this.metaButton4 = new Rawr.ItemButton();
            this.blueButton4 = new Rawr.ItemButton();
            this.yellowButton4 = new Rawr.ItemButton();
            this.redButton4 = new Rawr.ItemButton();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(364, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Click on the Gem Slot to Change the gem associated with that particular slot";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(382, 7);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(105, 30);
            this.checkBox1.TabIndex = 9;
            this.checkBox1.Text = "Delete non-listed\r\nGemmings";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // OkButton
            // 
            this.OkButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OkButton.Location = new System.Drawing.Point(382, 89);
            this.OkButton.Name = "OkButton";
            this.OkButton.Size = new System.Drawing.Size(45, 33);
            this.OkButton.TabIndex = 10;
            this.OkButton.Text = "OK";
            this.OkButton.UseVisualStyleBackColor = true;
            this.OkButton.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // buttonAddGemming
            // 
            this.buttonAddGemming.Location = new System.Drawing.Point(382, 43);
            this.buttonAddGemming.Name = "buttonAddGemming";
            this.buttonAddGemming.Size = new System.Drawing.Size(102, 41);
            this.buttonAddGemming.TabIndex = 11;
            this.buttonAddGemming.Text = "Add another Gemming";
            this.buttonAddGemming.UseVisualStyleBackColor = true;
            this.buttonAddGemming.Click += new System.EventHandler(this.buttonAddGemming_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.removeButton1);
            this.groupBox1.Controls.Add(this.metaButton1);
            this.groupBox1.Controls.Add(this.blueButton1);
            this.groupBox1.Controls.Add(this.yellowButton1);
            this.groupBox1.Controls.Add(this.redButton1);
            this.groupBox1.Location = new System.Drawing.Point(4, 43);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(337, 80);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Tag = 1;
            this.groupBox1.Text = "Gemming #1";
            // 
            // removeButton1
            // 
            this.removeButton1.Location = new System.Drawing.Point(267, 34);
            this.removeButton1.Name = "removeButton1";
            this.removeButton1.Size = new System.Drawing.Size(56, 19);
            this.removeButton1.TabIndex = 4;
            this.removeButton1.Tag = 1;
            this.removeButton1.Text = "Remove";
            this.removeButton1.UseVisualStyleBackColor = true;
            this.removeButton1.Click += new System.EventHandler(this.buttonRemoveGemming_Click);
            // 
            // metaButton1
            // 
            this.metaButton1.BackColor = System.Drawing.Color.Gray;
            this.metaButton1.Character = null;
            this.metaButton1.CharacterSlot = Rawr.Character.CharacterSlot.Metas;
            this.metaButton1.ItemIcon = null;
            this.metaButton1.Location = new System.Drawing.Point(197, 17);
            this.metaButton1.Name = "metaButton1";
            this.metaButton1.SelectedItem = null;
            this.metaButton1.SelectedItemId = 0;
            this.metaButton1.Size = new System.Drawing.Size(57, 56);
            this.metaButton1.TabIndex = 3;
            this.metaButton1.Tag = 1;
            this.metaButton1.Text = "Meta";
            this.metaButton1.UseVisualStyleBackColor = false;
            this.metaButton1.Leave += new System.EventHandler(this.buttonGem_Click);
            // 
            // blueButton1
            // 
            this.blueButton1.BackColor = System.Drawing.Color.Blue;
            this.blueButton1.Character = null;
            this.blueButton1.CharacterSlot = Rawr.Character.CharacterSlot.Gems;
            this.blueButton1.ItemIcon = null;
            this.blueButton1.Location = new System.Drawing.Point(134, 17);
            this.blueButton1.Name = "blueButton1";
            this.blueButton1.SelectedItem = null;
            this.blueButton1.SelectedItemId = 0;
            this.blueButton1.Size = new System.Drawing.Size(57, 56);
            this.blueButton1.TabIndex = 2;
            this.blueButton1.Tag = 1;
            this.blueButton1.Text = "Blue";
            this.blueButton1.UseVisualStyleBackColor = false;
            this.blueButton1.Leave += new System.EventHandler(this.buttonGem_Click);
            // 
            // yellowButton1
            // 
            this.yellowButton1.BackColor = System.Drawing.Color.Yellow;
            this.yellowButton1.Character = null;
            this.yellowButton1.CharacterSlot = Rawr.Character.CharacterSlot.Gems;
            this.yellowButton1.ItemIcon = null;
            this.yellowButton1.Location = new System.Drawing.Point(71, 17);
            this.yellowButton1.Name = "yellowButton1";
            this.yellowButton1.SelectedItem = null;
            this.yellowButton1.SelectedItemId = 0;
            this.yellowButton1.Size = new System.Drawing.Size(57, 56);
            this.yellowButton1.TabIndex = 1;
            this.yellowButton1.Tag = 1;
            this.yellowButton1.Text = "Yellow";
            this.yellowButton1.UseVisualStyleBackColor = false;
            this.yellowButton1.Leave += new System.EventHandler(this.buttonGem_Click);
            // 
            // redButton1
            // 
            this.redButton1.BackColor = System.Drawing.Color.Red;
            this.redButton1.Character = null;
            this.redButton1.CharacterSlot = Rawr.Character.CharacterSlot.Gems;
            this.redButton1.ItemIcon = null;
            this.redButton1.Location = new System.Drawing.Point(8, 17);
            this.redButton1.Name = "redButton1";
            this.redButton1.SelectedItem = null;
            this.redButton1.SelectedItemId = 0;
            this.redButton1.Size = new System.Drawing.Size(57, 56);
            this.redButton1.TabIndex = 0;
            this.redButton1.Tag = 1;
            this.redButton1.Text = "Red";
            this.redButton1.UseVisualStyleBackColor = false;
            this.redButton1.Leave += new System.EventHandler(this.buttonGem_Click);
            // 
            // button7
            // 
            this.button7.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button7.Location = new System.Drawing.Point(436, 89);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(51, 33);
            this.button7.TabIndex = 13;
            this.button7.Text = "Cancel";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.removeButton2);
            this.groupBox2.Controls.Add(this.metaButton2);
            this.groupBox2.Controls.Add(this.blueButton2);
            this.groupBox2.Controls.Add(this.yellowButton2);
            this.groupBox2.Controls.Add(this.redButton2);
            this.groupBox2.Location = new System.Drawing.Point(4, 128);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(337, 80);
            this.groupBox2.TabIndex = 14;
            this.groupBox2.TabStop = false;
            this.groupBox2.Tag = 2;
            this.groupBox2.Text = "Gemming #2";
            // 
            // removeButton2
            // 
            this.removeButton2.Location = new System.Drawing.Point(267, 34);
            this.removeButton2.Name = "removeButton2";
            this.removeButton2.Size = new System.Drawing.Size(56, 19);
            this.removeButton2.TabIndex = 4;
            this.removeButton2.Tag = 2;
            this.removeButton2.Text = "Remove";
            this.removeButton2.UseVisualStyleBackColor = true;
            this.removeButton2.Click += new System.EventHandler(this.buttonRemoveGemming_Click);
            // 
            // metaButton2
            // 
            this.metaButton2.BackColor = System.Drawing.Color.Gray;
            this.metaButton2.Character = null;
            this.metaButton2.CharacterSlot = Rawr.Character.CharacterSlot.Metas;
            this.metaButton2.ItemIcon = null;
            this.metaButton2.Location = new System.Drawing.Point(197, 17);
            this.metaButton2.Name = "metaButton2";
            this.metaButton2.SelectedItem = null;
            this.metaButton2.SelectedItemId = 0;
            this.metaButton2.Size = new System.Drawing.Size(57, 56);
            this.metaButton2.TabIndex = 3;
            this.metaButton2.Tag = 2;
            this.metaButton2.Text = "Meta";
            this.metaButton2.UseVisualStyleBackColor = false;
            this.metaButton2.Leave += new System.EventHandler(this.buttonGem_Click);
            // 
            // blueButton2
            // 
            this.blueButton2.BackColor = System.Drawing.Color.Blue;
            this.blueButton2.Character = null;
            this.blueButton2.CharacterSlot = Rawr.Character.CharacterSlot.Gems;
            this.blueButton2.ItemIcon = null;
            this.blueButton2.Location = new System.Drawing.Point(134, 17);
            this.blueButton2.Name = "blueButton2";
            this.blueButton2.SelectedItem = null;
            this.blueButton2.SelectedItemId = 0;
            this.blueButton2.Size = new System.Drawing.Size(57, 56);
            this.blueButton2.TabIndex = 2;
            this.blueButton2.Tag = 2;
            this.blueButton2.Text = "Blue";
            this.blueButton2.UseVisualStyleBackColor = false;
            this.blueButton2.Click += new System.EventHandler(this.buttonGem_Click);
            this.blueButton2.Leave += new System.EventHandler(this.buttonGem_Click);
            // 
            // yellowButton2
            // 
            this.yellowButton2.BackColor = System.Drawing.Color.Yellow;
            this.yellowButton2.Character = null;
            this.yellowButton2.CharacterSlot = Rawr.Character.CharacterSlot.Gems;
            this.yellowButton2.ItemIcon = null;
            this.yellowButton2.Location = new System.Drawing.Point(71, 17);
            this.yellowButton2.Name = "yellowButton2";
            this.yellowButton2.SelectedItem = null;
            this.yellowButton2.SelectedItemId = 0;
            this.yellowButton2.Size = new System.Drawing.Size(57, 56);
            this.yellowButton2.TabIndex = 1;
            this.yellowButton2.Tag = 2;
            this.yellowButton2.Text = "Yellow";
            this.yellowButton2.UseVisualStyleBackColor = false;
            this.yellowButton2.Click += new System.EventHandler(this.buttonGem_Click);
            this.yellowButton2.Leave += new System.EventHandler(this.buttonGem_Click);
            // 
            // redButton2
            // 
            this.redButton2.BackColor = System.Drawing.Color.Red;
            this.redButton2.Character = null;
            this.redButton2.CharacterSlot = Rawr.Character.CharacterSlot.Gems;
            this.redButton2.ItemIcon = null;
            this.redButton2.Location = new System.Drawing.Point(8, 17);
            this.redButton2.Name = "redButton2";
            this.redButton2.SelectedItem = null;
            this.redButton2.SelectedItemId = 0;
            this.redButton2.Size = new System.Drawing.Size(57, 56);
            this.redButton2.TabIndex = 0;
            this.redButton2.Tag = 2;
            this.redButton2.Text = "Red";
            this.redButton2.UseVisualStyleBackColor = false;
            this.redButton2.Click += new System.EventHandler(this.buttonGem_Click);
            this.redButton2.Leave += new System.EventHandler(this.buttonGem_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.removeButton3);
            this.groupBox3.Controls.Add(this.metaButton3);
            this.groupBox3.Controls.Add(this.blueButton3);
            this.groupBox3.Controls.Add(this.yellowButton3);
            this.groupBox3.Controls.Add(this.redButton3);
            this.groupBox3.Location = new System.Drawing.Point(4, 213);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(337, 80);
            this.groupBox3.TabIndex = 13;
            this.groupBox3.TabStop = false;
            this.groupBox3.Tag = 3;
            this.groupBox3.Text = "Gemming #3";
            // 
            // removeButton3
            // 
            this.removeButton3.Location = new System.Drawing.Point(267, 34);
            this.removeButton3.Name = "removeButton3";
            this.removeButton3.Size = new System.Drawing.Size(56, 19);
            this.removeButton3.TabIndex = 4;
            this.removeButton3.Tag = 3;
            this.removeButton3.Text = "Remove";
            this.removeButton3.UseVisualStyleBackColor = true;
            this.removeButton3.Click += new System.EventHandler(this.buttonRemoveGemming_Click);
            // 
            // metaButton3
            // 
            this.metaButton3.BackColor = System.Drawing.Color.Gray;
            this.metaButton3.Character = null;
            this.metaButton3.CharacterSlot = Rawr.Character.CharacterSlot.Metas;
            this.metaButton3.ItemIcon = null;
            this.metaButton3.Location = new System.Drawing.Point(197, 17);
            this.metaButton3.Name = "metaButton3";
            this.metaButton3.SelectedItem = null;
            this.metaButton3.SelectedItemId = 0;
            this.metaButton3.Size = new System.Drawing.Size(57, 56);
            this.metaButton3.TabIndex = 3;
            this.metaButton3.Tag = 3;
            this.metaButton3.Text = "Meta";
            this.metaButton3.UseVisualStyleBackColor = false;
            this.metaButton3.Leave += new System.EventHandler(this.buttonGem_Click);
            // 
            // blueButton3
            // 
            this.blueButton3.BackColor = System.Drawing.Color.Blue;
            this.blueButton3.Character = null;
            this.blueButton3.CharacterSlot = Rawr.Character.CharacterSlot.Gems;
            this.blueButton3.ItemIcon = null;
            this.blueButton3.Location = new System.Drawing.Point(134, 17);
            this.blueButton3.Name = "blueButton3";
            this.blueButton3.SelectedItem = null;
            this.blueButton3.SelectedItemId = 0;
            this.blueButton3.Size = new System.Drawing.Size(57, 56);
            this.blueButton3.TabIndex = 2;
            this.blueButton3.Tag = 3;
            this.blueButton3.Text = "Blue";
            this.blueButton3.UseVisualStyleBackColor = false;
            this.blueButton3.Leave += new System.EventHandler(this.buttonGem_Click);
            // 
            // yellowButton3
            // 
            this.yellowButton3.BackColor = System.Drawing.Color.Yellow;
            this.yellowButton3.Character = null;
            this.yellowButton3.CharacterSlot = Rawr.Character.CharacterSlot.Gems;
            this.yellowButton3.ItemIcon = null;
            this.yellowButton3.Location = new System.Drawing.Point(71, 17);
            this.yellowButton3.Name = "yellowButton3";
            this.yellowButton3.SelectedItem = null;
            this.yellowButton3.SelectedItemId = 0;
            this.yellowButton3.Size = new System.Drawing.Size(57, 56);
            this.yellowButton3.TabIndex = 1;
            this.yellowButton3.Tag = 3;
            this.yellowButton3.Text = "Yellow";
            this.yellowButton3.UseVisualStyleBackColor = false;
            this.yellowButton3.Leave += new System.EventHandler(this.buttonGem_Click);
            // 
            // redButton3
            // 
            this.redButton3.BackColor = System.Drawing.Color.Red;
            this.redButton3.Character = null;
            this.redButton3.CharacterSlot = Rawr.Character.CharacterSlot.Gems;
            this.redButton3.ItemIcon = null;
            this.redButton3.Location = new System.Drawing.Point(8, 17);
            this.redButton3.Name = "redButton3";
            this.redButton3.SelectedItem = null;
            this.redButton3.SelectedItemId = 0;
            this.redButton3.Size = new System.Drawing.Size(57, 56);
            this.redButton3.TabIndex = 0;
            this.redButton3.Tag = 3;
            this.redButton3.Text = "Red";
            this.redButton3.UseVisualStyleBackColor = false;
            this.redButton3.Leave += new System.EventHandler(this.buttonGem_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.removeButton4);
            this.groupBox4.Controls.Add(this.metaButton4);
            this.groupBox4.Controls.Add(this.blueButton4);
            this.groupBox4.Controls.Add(this.yellowButton4);
            this.groupBox4.Controls.Add(this.redButton4);
            this.groupBox4.Location = new System.Drawing.Point(4, 298);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(337, 80);
            this.groupBox4.TabIndex = 13;
            this.groupBox4.TabStop = false;
            this.groupBox4.Tag = 4;
            this.groupBox4.Text = "Gemming #4";
            // 
            // removeButton4
            // 
            this.removeButton4.Location = new System.Drawing.Point(267, 34);
            this.removeButton4.Name = "removeButton4";
            this.removeButton4.Size = new System.Drawing.Size(56, 19);
            this.removeButton4.TabIndex = 4;
            this.removeButton4.Tag = 4;
            this.removeButton4.Text = "Remove";
            this.removeButton4.UseVisualStyleBackColor = true;
            this.removeButton4.Click += new System.EventHandler(this.buttonRemoveGemming_Click);
            // 
            // metaButton4
            // 
            this.metaButton4.BackColor = System.Drawing.Color.Gray;
            this.metaButton4.Character = null;
            this.metaButton4.CharacterSlot = Rawr.Character.CharacterSlot.Metas;
            this.metaButton4.ItemIcon = null;
            this.metaButton4.Location = new System.Drawing.Point(197, 17);
            this.metaButton4.Name = "metaButton4";
            this.metaButton4.SelectedItem = null;
            this.metaButton4.SelectedItemId = 0;
            this.metaButton4.Size = new System.Drawing.Size(57, 56);
            this.metaButton4.TabIndex = 3;
            this.metaButton4.Tag = 4;
            this.metaButton4.Text = "Meta";
            this.metaButton4.UseVisualStyleBackColor = false;
            this.metaButton4.Leave += new System.EventHandler(this.buttonGem_Click);
            // 
            // blueButton4
            // 
            this.blueButton4.BackColor = System.Drawing.Color.Blue;
            this.blueButton4.Character = null;
            this.blueButton4.CharacterSlot = Rawr.Character.CharacterSlot.Gems;
            this.blueButton4.ItemIcon = null;
            this.blueButton4.Location = new System.Drawing.Point(134, 17);
            this.blueButton4.Name = "blueButton4";
            this.blueButton4.SelectedItem = null;
            this.blueButton4.SelectedItemId = 0;
            this.blueButton4.Size = new System.Drawing.Size(57, 56);
            this.blueButton4.TabIndex = 2;
            this.blueButton4.Tag = 4;
            this.blueButton4.Text = "Blue";
            this.blueButton4.UseVisualStyleBackColor = false;
            this.blueButton4.Leave += new System.EventHandler(this.buttonGem_Click);
            // 
            // yellowButton4
            // 
            this.yellowButton4.BackColor = System.Drawing.Color.Yellow;
            this.yellowButton4.Character = null;
            this.yellowButton4.CharacterSlot = Rawr.Character.CharacterSlot.Gems;
            this.yellowButton4.ItemIcon = null;
            this.yellowButton4.Location = new System.Drawing.Point(71, 17);
            this.yellowButton4.Name = "yellowButton4";
            this.yellowButton4.SelectedItem = null;
            this.yellowButton4.SelectedItemId = 0;
            this.yellowButton4.Size = new System.Drawing.Size(57, 56);
            this.yellowButton4.TabIndex = 1;
            this.yellowButton4.Tag = 4;
            this.yellowButton4.Text = "Yellow";
            this.yellowButton4.UseVisualStyleBackColor = false;
            this.yellowButton4.Leave += new System.EventHandler(this.buttonGem_Click);
            // 
            // redButton4
            // 
            this.redButton4.BackColor = System.Drawing.Color.Red;
            this.redButton4.Character = null;
            this.redButton4.CharacterSlot = Rawr.Character.CharacterSlot.Gems;
            this.redButton4.ItemIcon = null;
            this.redButton4.Location = new System.Drawing.Point(8, 17);
            this.redButton4.Name = "redButton4";
            this.redButton4.SelectedItem = null;
            this.redButton4.SelectedItemId = 0;
            this.redButton4.Size = new System.Drawing.Size(57, 56);
            this.redButton4.TabIndex = 0;
            this.redButton4.Tag = 4;
            this.redButton4.Text = "Red";
            this.redButton4.UseVisualStyleBackColor = false;
            this.redButton4.Leave += new System.EventHandler(this.buttonGem_Click);
            // 
            // FormMassGemReplacement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(496, 394);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonAddGemming);
            this.Controls.Add(this.OkButton);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.Name = "FormMassGemReplacement";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FormMassGemReplacement";
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Rawr.ItemButton redButton1;
        private Rawr.ItemButton yellowButton1;
        private Rawr.ItemButton blueButton1;
        private Rawr.ItemButton metaButton1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Button OkButton;
        private System.Windows.Forms.Button buttonAddGemming;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button removeButton1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button removeButton2;
        private ItemButton metaButton2;
        private ItemButton blueButton2;
        private ItemButton yellowButton2;
        private ItemButton redButton2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button removeButton3;
        private ItemButton metaButton3;
        private ItemButton blueButton3;
        private ItemButton yellowButton3;
        private ItemButton redButton3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button removeButton4;
        private ItemButton metaButton4;
        private ItemButton blueButton4;
        private ItemButton yellowButton4;
        private ItemButton redButton4;
    }
}