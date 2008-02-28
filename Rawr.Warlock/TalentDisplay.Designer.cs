namespace Rawr.Warlock
{
    partial class TalentDisplay
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TalentDisplay));
            this.talentPanel1 = new Rawr.Warlock.TalentPanel();
            this.talentPanel2 = new Rawr.Warlock.TalentPanel();
            this.talentPanel3 = new Rawr.Warlock.TalentPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // talentPanel1
            // 
            this.talentPanel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.talentPanel1.CharClass = Rawr.Character.CharacterClass.Warlock;
            this.talentPanel1.Location = new System.Drawing.Point(28, 36);
            this.talentPanel1.Name = "talentPanel1";
            this.talentPanel1.Size = new System.Drawing.Size(248, 579);
            this.talentPanel1.TabIndex = 0;
            this.talentPanel1.Talents = ((System.Collections.Generic.List<Rawr.TalentItem>)(resources.GetObject("talentPanel1.Talents")));
            // 
            // talentPanel2
            // 
            this.talentPanel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.talentPanel2.CharClass = Rawr.Character.CharacterClass.Warlock;
            this.talentPanel2.Location = new System.Drawing.Point(328, 36);
            this.talentPanel2.Name = "talentPanel2";
            this.talentPanel2.Size = new System.Drawing.Size(248, 579);
            this.talentPanel2.TabIndex = 1;
            this.talentPanel2.Talents = ((System.Collections.Generic.List<Rawr.TalentItem>)(resources.GetObject("talentPanel2.Talents")));
            // 
            // talentPanel3
            // 
            this.talentPanel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.talentPanel3.CharClass = Rawr.Character.CharacterClass.Warlock;
            this.talentPanel3.Location = new System.Drawing.Point(633, 36);
            this.talentPanel3.Name = "talentPanel3";
            this.talentPanel3.Size = new System.Drawing.Size(248, 579);
            this.talentPanel3.TabIndex = 2;
            this.talentPanel3.Talents = ((System.Collections.Generic.List<Rawr.TalentItem>)(resources.GetObject("talentPanel3.Talents")));
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(28, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "label1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(328, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "label2";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(633, 17);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "label3";
            // 
            // TalentDisplay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.talentPanel3);
            this.Controls.Add(this.talentPanel2);
            this.Controls.Add(this.talentPanel1);
            this.Name = "TalentDisplay";
            this.Size = new System.Drawing.Size(962, 642);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TalentPanel talentPanel1;
        private TalentPanel talentPanel2;
        private TalentPanel talentPanel3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;

            }
}
