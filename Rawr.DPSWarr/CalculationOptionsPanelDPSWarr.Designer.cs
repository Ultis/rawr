namespace Rawr.DPSWarr {
    partial class CalculationOptionsPanelDPSWarr {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.CB_TargLvl = new System.Windows.Forms.ComboBox();
            this.LB_TargLvl = new System.Windows.Forms.Label();
            this.LB_TargArmor = new System.Windows.Forms.Label();
            this.LB_TargArmorDesc = new System.Windows.Forms.Label();
            this.CB_TargArmor = new System.Windows.Forms.ComboBox();
            this.GB_Bosses = new System.Windows.Forms.GroupBox();
            this.CB_ToughLvl = new System.Windows.Forms.ComboBox();
            this.LB_ToughLvl = new System.Windows.Forms.Label();
            this.RB_StanceFury = new System.Windows.Forms.RadioButton();
            this.RB_StanceArms = new System.Windows.Forms.RadioButton();
            this.GB_Bosses.SuspendLayout();
            this.SuspendLayout();
            // 
            // CB_TargLvl
            // 
            this.CB_TargLvl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.CB_TargLvl.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CB_TargLvl.FormattingEnabled = true;
            this.CB_TargLvl.Items.AddRange(new object[] {
            "83",
            "82",
            "81",
            "80"});
            this.CB_TargLvl.Location = new System.Drawing.Point(79, 3);
            this.CB_TargLvl.Name = "CB_TargLvl";
            this.CB_TargLvl.Size = new System.Drawing.Size(155, 21);
            this.CB_TargLvl.TabIndex = 1;
            this.CB_TargLvl.SelectedIndexChanged += new System.EventHandler(this.comboBoxTargetLevel_SelectedIndexChanged);
            // 
            // LB_TargLvl
            // 
            this.LB_TargLvl.AutoSize = true;
            this.LB_TargLvl.Location = new System.Drawing.Point(3, 6);
            this.LB_TargLvl.Name = "LB_TargLvl";
            this.LB_TargLvl.Size = new System.Drawing.Size(70, 13);
            this.LB_TargLvl.TabIndex = 0;
            this.LB_TargLvl.Text = "Target Level:";
            // 
            // LB_TargArmor
            // 
            this.LB_TargArmor.AutoSize = true;
            this.LB_TargArmor.Location = new System.Drawing.Point(3, 33);
            this.LB_TargArmor.Name = "LB_TargArmor";
            this.LB_TargArmor.Size = new System.Drawing.Size(71, 13);
            this.LB_TargArmor.TabIndex = 2;
            this.LB_TargArmor.Text = "Target Armor:";
            // 
            // LB_TargArmorDesc
            // 
            this.LB_TargArmorDesc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LB_TargArmorDesc.Location = new System.Drawing.Point(3, 16);
            this.LB_TargArmorDesc.Name = "LB_TargArmorDesc";
            this.LB_TargArmorDesc.Size = new System.Drawing.Size(222, 297);
            this.LB_TargArmorDesc.TabIndex = 0;
            // 
            // CB_TargArmor
            // 
            this.CB_TargArmor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.CB_TargArmor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CB_TargArmor.FormattingEnabled = true;
            this.CB_TargArmor.Items.AddRange(new object[] {
            "10900",
            "12000",
            "13083"});
            this.CB_TargArmor.Location = new System.Drawing.Point(79, 30);
            this.CB_TargArmor.Name = "CB_TargArmor";
            this.CB_TargArmor.Size = new System.Drawing.Size(155, 21);
            this.CB_TargArmor.TabIndex = 3;
            this.CB_TargArmor.SelectedIndexChanged += new System.EventHandler(this.comboBoxArmorBosses_SelectedIndexChanged);
            // 
            // GB_Bosses
            // 
            this.GB_Bosses.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.GB_Bosses.Controls.Add(this.LB_TargArmorDesc);
            this.GB_Bosses.Location = new System.Drawing.Point(6, 107);
            this.GB_Bosses.Name = "GB_Bosses";
            this.GB_Bosses.Size = new System.Drawing.Size(228, 316);
            this.GB_Bosses.TabIndex = 6;
            this.GB_Bosses.TabStop = false;
            this.GB_Bosses.Text = "Bosses";
            // 
            // CB_ToughLvl
            // 
            this.CB_ToughLvl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.CB_ToughLvl.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CB_ToughLvl.FormattingEnabled = true;
            this.CB_ToughLvl.Items.AddRange(new object[] {
            "",
            "Rank 1: STA 3",
            "Rank 2: STA 5",
            "Rank 3: STA 7",
            "Rank 4: STA 10",
            "Rank 5: STA 30",
            "Rank 6: STA 50"});
            this.CB_ToughLvl.Location = new System.Drawing.Point(79, 57);
            this.CB_ToughLvl.Name = "CB_ToughLvl";
            this.CB_ToughLvl.Size = new System.Drawing.Size(155, 21);
            this.CB_ToughLvl.TabIndex = 5;
            this.CB_ToughLvl.SelectedIndexChanged += new System.EventHandler(this.CB_ToughLvl_SelectedIndexChanged);
            // 
            // LB_ToughLvl
            // 
            this.LB_ToughLvl.AutoSize = true;
            this.LB_ToughLvl.Location = new System.Drawing.Point(3, 60);
            this.LB_ToughLvl.Name = "LB_ToughLvl";
            this.LB_ToughLvl.Size = new System.Drawing.Size(80, 13);
            this.LB_ToughLvl.TabIndex = 4;
            this.LB_ToughLvl.Text = "Toughness Lvl:";
            // 
            // RB_StanceFury
            // 
            this.RB_StanceFury.AutoSize = true;
            this.RB_StanceFury.Checked = true;
            this.RB_StanceFury.Location = new System.Drawing.Point(6, 84);
            this.RB_StanceFury.Name = "RB_StanceFury";
            this.RB_StanceFury.Size = new System.Drawing.Size(82, 17);
            this.RB_StanceFury.TabIndex = 7;
            this.RB_StanceFury.TabStop = true;
            this.RB_StanceFury.Text = "Fury Stance";
            this.RB_StanceFury.UseVisualStyleBackColor = true;
            this.RB_StanceFury.CheckedChanged += new System.EventHandler(this.RB_StanceFury_CheckedChanged);
            // 
            // RB_StanceArms
            // 
            this.RB_StanceArms.AutoSize = true;
            this.RB_StanceArms.Location = new System.Drawing.Point(120, 84);
            this.RB_StanceArms.Name = "RB_StanceArms";
            this.RB_StanceArms.Size = new System.Drawing.Size(85, 17);
            this.RB_StanceArms.TabIndex = 8;
            this.RB_StanceArms.Text = "Arms Stance";
            this.RB_StanceArms.UseVisualStyleBackColor = true;
            this.RB_StanceArms.CheckedChanged += new System.EventHandler(this.RB_StanceFury_CheckedChanged);
            // 
            // CalculationOptionsPanelDPSWarr
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.RB_StanceArms);
            this.Controls.Add(this.RB_StanceFury);
            this.Controls.Add(this.CB_ToughLvl);
            this.Controls.Add(this.LB_ToughLvl);
            this.Controls.Add(this.CB_TargArmor);
            this.Controls.Add(this.LB_TargArmor);
            this.Controls.Add(this.LB_TargLvl);
            this.Controls.Add(this.CB_TargLvl);
            this.Controls.Add(this.GB_Bosses);
            this.Name = "CalculationOptionsPanelDPSWarr";
            this.Size = new System.Drawing.Size(238, 432);
            this.GB_Bosses.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.ComboBox CB_TargLvl;
        public System.Windows.Forms.Label LB_TargLvl;
        public System.Windows.Forms.Label LB_TargArmor;
        public System.Windows.Forms.Label LB_TargArmorDesc;
        public System.Windows.Forms.ComboBox CB_TargArmor;
        public System.Windows.Forms.GroupBox GB_Bosses;
        public System.Windows.Forms.ComboBox CB_ToughLvl;
        public System.Windows.Forms.Label LB_ToughLvl;
        private System.Windows.Forms.RadioButton RB_StanceFury;
        private System.Windows.Forms.RadioButton RB_StanceArms;
    }
}
