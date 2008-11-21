namespace Rawr.Tree
{
    partial class SpellRotationEditor
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button8 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.comboBox3 = new System.Windows.Forms.ComboBox();
            this.label37 = new System.Windows.Forms.Label();
            this.label36 = new System.Windows.Forms.Label();
            this.button4 = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.button5 = new System.Windows.Forms.Button();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.button6 = new System.Windows.Forms.Button();
            this.label35 = new System.Windows.Forms.Label();
            this.lbRotations = new System.Windows.Forms.ListBox();
            this.bRotaNew = new System.Windows.Forms.Button();
            this.bRotaDelete = new System.Windows.Forms.Button();
            this.bRotaDown = new System.Windows.Forms.Button();
            this.bRotaUp = new System.Windows.Forms.Button();
            this.tbNameEditor = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.refreshTimer = new System.Windows.Forms.Timer(this.components);
            this.lTime2OOM = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.lHPS = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(234, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(30, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Spell";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(418, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(30, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Time";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(374, 35);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Target";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(278, 6);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(45, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "22.5sec";
            this.label4.Visible = false;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(3, 1);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(24, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = ">";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(33, 1);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(26, 23);
            this.button2.TabIndex = 5;
            this.button2.Text = " Λ";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Visible = false;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(65, 1);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(26, 23);
            this.button3.TabIndex = 6;
            this.button3.Text = " V";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Visible = false;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "Healing Touch",
            "Regrowth",
            "Rejuvenation",
            "Lifebloom",
            "Lifebloom Stack"});
            this.comboBox1.Location = new System.Drawing.Point(97, 2);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(134, 21);
            this.comboBox1.TabIndex = 7;
            this.comboBox1.Text = "Healing Touch";
            this.comboBox1.Visible = false;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(237, 2);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(35, 20);
            this.textBox1.TabIndex = 8;
            this.textBox1.Text = "1";
            this.textBox1.Visible = false;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.button8);
            this.panel1.Controls.Add(this.button7);
            this.panel1.Controls.Add(this.comboBox3);
            this.panel1.Controls.Add(this.label37);
            this.panel1.Controls.Add(this.label36);
            this.panel1.Controls.Add(this.button4);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.textBox2);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.button5);
            this.panel1.Controls.Add(this.textBox1);
            this.panel1.Controls.Add(this.comboBox2);
            this.panel1.Controls.Add(this.button3);
            this.panel1.Controls.Add(this.button6);
            this.panel1.Controls.Add(this.comboBox1);
            this.panel1.Controls.Add(this.button2);
            this.panel1.Location = new System.Drawing.Point(140, 51);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(424, 205);
            this.panel1.TabIndex = 9;
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(380, 30);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(26, 23);
            this.button8.TabIndex = 21;
            this.button8.Text = "X";
            this.button8.UseVisualStyleBackColor = true;
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(380, 0);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(26, 23);
            this.button7.TabIndex = 20;
            this.button7.Text = "X";
            this.button7.UseVisualStyleBackColor = true;
            // 
            // comboBox3
            // 
            this.comboBox3.FormattingEnabled = true;
            this.comboBox3.Items.AddRange(new object[] {
            "Healing Touch",
            "Regrowth",
            "Rejuvenation",
            "Lifebloom",
            "Lifebloom Stack"});
            this.comboBox3.Location = new System.Drawing.Point(97, 57);
            this.comboBox3.Name = "comboBox3";
            this.comboBox3.Size = new System.Drawing.Size(134, 21);
            this.comboBox3.TabIndex = 19;
            this.comboBox3.Visible = false;
            // 
            // label37
            // 
            this.label37.AutoSize = true;
            this.label37.Location = new System.Drawing.Point(338, 35);
            this.label37.Name = "label37";
            this.label37.Size = new System.Drawing.Size(36, 13);
            this.label37.TabIndex = 18;
            this.label37.Text = "21sec";
            this.label37.Visible = false;
            // 
            // label36
            // 
            this.label36.AutoSize = true;
            this.label36.Location = new System.Drawing.Point(329, 5);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(45, 13);
            this.label36.TabIndex = 17;
            this.label36.Text = "20.8sec";
            this.label36.Visible = false;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(3, 30);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(24, 23);
            this.button4.TabIndex = 12;
            this.button4.Text = ">";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Visible = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(284, 35);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(39, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "4.5sec";
            this.label5.Visible = false;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(237, 30);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(35, 20);
            this.textBox2.TabIndex = 16;
            this.textBox2.Text = "1";
            this.textBox2.Visible = false;
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(65, 30);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(26, 23);
            this.button5.TabIndex = 14;
            this.button5.Text = " V";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Visible = false;
            // 
            // comboBox2
            // 
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Items.AddRange(new object[] {
            "Healing Touch",
            "Regrowth",
            "Rejuvenation",
            "Lifebloom",
            "Lifebloom Stack"});
            this.comboBox2.Location = new System.Drawing.Point(97, 30);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(134, 21);
            this.comboBox2.TabIndex = 15;
            this.comboBox2.Text = "Regrowth";
            this.comboBox2.Visible = false;
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(33, 30);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(26, 23);
            this.button6.TabIndex = 13;
            this.button6.Text = " Λ";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Visible = false;
            // 
            // label35
            // 
            this.label35.AutoSize = true;
            this.label35.Location = new System.Drawing.Point(469, 35);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(47, 13);
            this.label35.TabIndex = 11;
            this.label35.Text = "Duration";
            this.label35.Visible = false;
            // 
            // lbRotations
            // 
            this.lbRotations.FormattingEnabled = true;
            this.lbRotations.Location = new System.Drawing.Point(12, 9);
            this.lbRotations.Name = "lbRotations";
            this.lbRotations.Size = new System.Drawing.Size(122, 186);
            this.lbRotations.TabIndex = 12;
            this.lbRotations.SelectedIndexChanged += new System.EventHandler(this.lbRotations_SelectedIndexChanged);
            // 
            // bRotaNew
            // 
            this.bRotaNew.Location = new System.Drawing.Point(108, 201);
            this.bRotaNew.Name = "bRotaNew";
            this.bRotaNew.Size = new System.Drawing.Size(26, 23);
            this.bRotaNew.TabIndex = 25;
            this.bRotaNew.Text = "+";
            this.bRotaNew.UseVisualStyleBackColor = true;
            this.bRotaNew.Click += new System.EventHandler(this.bRotaNew_Click);
            // 
            // bRotaDelete
            // 
            this.bRotaDelete.Location = new System.Drawing.Point(12, 201);
            this.bRotaDelete.Name = "bRotaDelete";
            this.bRotaDelete.Size = new System.Drawing.Size(26, 23);
            this.bRotaDelete.TabIndex = 24;
            this.bRotaDelete.Text = "X";
            this.bRotaDelete.UseVisualStyleBackColor = true;
            this.bRotaDelete.Click += new System.EventHandler(this.bRotaDelete_Click);
            // 
            // bRotaDown
            // 
            this.bRotaDown.Location = new System.Drawing.Point(76, 201);
            this.bRotaDown.Name = "bRotaDown";
            this.bRotaDown.Size = new System.Drawing.Size(26, 23);
            this.bRotaDown.TabIndex = 23;
            this.bRotaDown.Text = " V";
            this.bRotaDown.UseVisualStyleBackColor = true;
            this.bRotaDown.Click += new System.EventHandler(this.bRotaDown_Click);
            // 
            // bRotaUp
            // 
            this.bRotaUp.Location = new System.Drawing.Point(44, 201);
            this.bRotaUp.Name = "bRotaUp";
            this.bRotaUp.Size = new System.Drawing.Size(26, 23);
            this.bRotaUp.TabIndex = 22;
            this.bRotaUp.Text = " Λ";
            this.bRotaUp.UseVisualStyleBackColor = true;
            this.bRotaUp.Click += new System.EventHandler(this.bRotaUp_Click);
            // 
            // tbNameEditor
            // 
            this.tbNameEditor.Location = new System.Drawing.Point(184, 10);
            this.tbNameEditor.Name = "tbNameEditor";
            this.tbNameEditor.Size = new System.Drawing.Size(159, 20);
            this.tbNameEditor.TabIndex = 26;
            this.tbNameEditor.TextChanged += new System.EventHandler(this.tbNameEditor_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(140, 13);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(38, 13);
            this.label6.TabIndex = 27;
            this.label6.Text = "Name:";
            // 
            // refreshTimer
            // 
            this.refreshTimer.Interval = 3000;
            this.refreshTimer.Tick += new System.EventHandler(this.refreshTimer_Tick);
            // 
            // lTime2OOM
            // 
            this.lTime2OOM.AutoSize = true;
            this.lTime2OOM.Location = new System.Drawing.Point(423, 13);
            this.lTime2OOM.Name = "lTime2OOM";
            this.lTime2OOM.Size = new System.Drawing.Size(51, 13);
            this.lTime2OOM.TabIndex = 28;
            this.lTime2OOM.Text = "135,2sec";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(349, 13);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(68, 13);
            this.label8.TabIndex = 29;
            this.label8.Text = "Time till oom:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(478, 13);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(32, 13);
            this.label9.TabIndex = 30;
            this.label9.Text = "HPS:";
            // 
            // lHPS
            // 
            this.lHPS.AutoSize = true;
            this.lHPS.Location = new System.Drawing.Point(516, 13);
            this.lHPS.Name = "lHPS";
            this.lHPS.Size = new System.Drawing.Size(40, 13);
            this.lHPS.TabIndex = 31;
            this.lHPS.Text = "5193,2";
            // 
            // SpellRotationEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(576, 264);
            this.Controls.Add(this.lHPS);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.lTime2OOM);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.tbNameEditor);
            this.Controls.Add(this.bRotaNew);
            this.Controls.Add(this.bRotaDelete);
            this.Controls.Add(this.bRotaDown);
            this.Controls.Add(this.bRotaUp);
            this.Controls.Add(this.lbRotations);
            this.Controls.Add(this.label35);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label3);
            this.Name = "SpellRotationEditor";
            this.Text = "SpellRotationEditor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SpellRotationEditor_FormClosing);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Label label37;
        private System.Windows.Forms.Label label36;
        private System.Windows.Forms.Label label35;
        private System.Windows.Forms.ComboBox comboBox3;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.ListBox lbRotations;
        private System.Windows.Forms.Button bRotaNew;
        private System.Windows.Forms.Button bRotaDelete;
        private System.Windows.Forms.Button bRotaDown;
        private System.Windows.Forms.Button bRotaUp;
        private System.Windows.Forms.TextBox tbNameEditor;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Timer refreshTimer;
        private System.Windows.Forms.Label lTime2OOM;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label lHPS;
    }
}