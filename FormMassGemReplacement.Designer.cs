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
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button8 = new System.Windows.Forms.Button();
            this.vScrollBar1 = new System.Windows.Forms.VScrollBar();
            this.button7 = new System.Windows.Forms.Button();
            this.metaButton1 = new Rawr.ItemButton();
            this.blueButton1 = new Rawr.ItemButton();
            this.yellowButton1 = new Rawr.ItemButton();
            this.redButton1 = new Rawr.ItemButton();
            this.groupBox1.SuspendLayout();
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
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(382, 89);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(86, 33);
            this.button5.TabIndex = 10;
            this.button5.Text = "Done";
            this.button5.UseVisualStyleBackColor = true;
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(382, 43);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(86, 40);
            this.button6.TabIndex = 11;
            this.button6.Text = "Add another Gemming";
            this.button6.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button8);
            this.groupBox1.Controls.Add(this.vScrollBar1);
            this.groupBox1.Controls.Add(this.metaButton1);
            this.groupBox1.Controls.Add(this.blueButton1);
            this.groupBox1.Controls.Add(this.yellowButton1);
            this.groupBox1.Controls.Add(this.redButton1);
            this.groupBox1.Location = new System.Drawing.Point(4, 43);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(337, 91);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Gemmings";
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(258, 38);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(61, 19);
            this.button8.TabIndex = 5;
            this.button8.Text = "Remove";
            this.button8.UseVisualStyleBackColor = true;
            // 
            // vScrollBar1
            // 
            this.vScrollBar1.Location = new System.Drawing.Point(319, 7);
            this.vScrollBar1.Name = "vScrollBar1";
            this.vScrollBar1.Size = new System.Drawing.Size(18, 80);
            this.vScrollBar1.TabIndex = 4;
            this.vScrollBar1.Visible = false;
            this.vScrollBar1.Scroll += new System.Windows.Forms.ScrollEventHandler(this.vScrollBar1_Scroll);
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(382, 128);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(86, 28);
            this.button7.TabIndex = 13;
            this.button7.Text = "Cancel";
            this.button7.UseVisualStyleBackColor = true;
            // 
            // metaButton1
            // 
            this.metaButton1.Character = null;
            this.metaButton1.CharacterSlot = Rawr.Character.CharacterSlot.Metas;
            this.metaButton1.ItemIcon = null;
            this.metaButton1.Location = new System.Drawing.Point(195, 20);
            this.metaButton1.Name = "metaButton1";
            this.metaButton1.SelectedItem = null;
            this.metaButton1.SelectedItemId = 0;
            this.metaButton1.Size = new System.Drawing.Size(57, 56);
            this.metaButton1.TabIndex = 3;
            this.metaButton1.Text = "Meta";
            this.metaButton1.UseVisualStyleBackColor = true;
            // 
            // blueButton1
            // 
            this.blueButton1.BackColor = System.Drawing.Color.Blue;
            this.blueButton1.Character = null;
            this.blueButton1.CharacterSlot = Rawr.Character.CharacterSlot.Gems;
            this.blueButton1.ItemIcon = null;
            this.blueButton1.Location = new System.Drawing.Point(132, 20);
            this.blueButton1.Name = "blueButton1";
            this.blueButton1.SelectedItem = null;
            this.blueButton1.SelectedItemId = 0;
            this.blueButton1.Size = new System.Drawing.Size(57, 56);
            this.blueButton1.TabIndex = 2;
            this.blueButton1.Text = "Blue";
            this.blueButton1.UseVisualStyleBackColor = false;
            // 
            // yellowButton1
            // 
            this.yellowButton1.BackColor = System.Drawing.Color.Yellow;
            this.yellowButton1.Character = null;
            this.yellowButton1.CharacterSlot = Rawr.Character.CharacterSlot.Gems;
            this.yellowButton1.ItemIcon = null;
            this.yellowButton1.Location = new System.Drawing.Point(69, 19);
            this.yellowButton1.Name = "yellowButton1";
            this.yellowButton1.SelectedItem = null;
            this.yellowButton1.SelectedItemId = 0;
            this.yellowButton1.Size = new System.Drawing.Size(57, 56);
            this.yellowButton1.TabIndex = 1;
            this.yellowButton1.Text = "Yellow";
            this.yellowButton1.UseVisualStyleBackColor = false;
            // 
            // redButton1
            // 
            this.redButton1.BackColor = System.Drawing.Color.Red;
            this.redButton1.Character = null;
            this.redButton1.CharacterSlot = Rawr.Character.CharacterSlot.Gems;
            this.redButton1.ItemIcon = null;
            this.redButton1.Location = new System.Drawing.Point(6, 19);
            this.redButton1.Name = "redButton1";
            this.redButton1.SelectedItem = null;
            this.redButton1.SelectedItemId = 0;
            this.redButton1.Size = new System.Drawing.Size(57, 56);
            this.redButton1.TabIndex = 0;
            this.redButton1.Text = "Red";
            this.redButton1.UseVisualStyleBackColor = false;
            // 
            // FormMassGemReplacement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(496, 160);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.label1);
            this.Name = "FormMassGemReplacement";
            this.Text = "FormMassGemReplacement";
            this.groupBox1.ResumeLayout(false);
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
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.VScrollBar vScrollBar1;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button8;
    }
}