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
            this.CB_Duration = new System.Windows.Forms.NumericUpDown();
            this.LB_Duration = new System.Windows.Forms.Label();
            this.GB_Rotation = new System.Windows.Forms.GroupBox();
            this.TLP_Rotation = new System.Windows.Forms.TableLayoutPanel();
            this.RB_TargMultiple = new System.Windows.Forms.RadioButton();
            this.RB_TargSingle = new System.Windows.Forms.RadioButton();
            this.TLP_Main = new System.Windows.Forms.TableLayoutPanel();
            this.RB_StanceArms = new System.Windows.Forms.RadioButton();
            this.RB_StanceFury = new System.Windows.Forms.RadioButton();
            this.GB_Bosses.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CB_Duration)).BeginInit();
            this.GB_Rotation.SuspendLayout();
            this.TLP_Rotation.SuspendLayout();
            this.TLP_Main.SuspendLayout();
            this.SuspendLayout();
            // 
            // CB_TargLvl
            // 
            this.CB_TargLvl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CB_TargLvl.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CB_TargLvl.FormattingEnabled = true;
            this.CB_TargLvl.Items.AddRange(new object[] {
            "83",
            "82",
            "81",
            "80"});
            this.CB_TargLvl.Location = new System.Drawing.Point(127, 3);
            this.CB_TargLvl.Name = "CB_TargLvl";
            this.CB_TargLvl.Size = new System.Drawing.Size(131, 21);
            this.CB_TargLvl.TabIndex = 1;
            this.CB_TargLvl.SelectedIndexChanged += new System.EventHandler(this.comboBoxTargetLevel_SelectedIndexChanged);
            // 
            // LB_TargLvl
            // 
            this.LB_TargLvl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LB_TargLvl.Location = new System.Drawing.Point(3, 0);
            this.LB_TargLvl.Name = "LB_TargLvl";
            this.LB_TargLvl.Size = new System.Drawing.Size(118, 27);
            this.LB_TargLvl.TabIndex = 0;
            this.LB_TargLvl.Text = "Target Level:";
            this.LB_TargLvl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // LB_TargArmor
            // 
            this.LB_TargArmor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LB_TargArmor.Location = new System.Drawing.Point(3, 27);
            this.LB_TargArmor.Name = "LB_TargArmor";
            this.LB_TargArmor.Size = new System.Drawing.Size(118, 27);
            this.LB_TargArmor.TabIndex = 2;
            this.LB_TargArmor.Text = "Target Armor:";
            this.LB_TargArmor.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // LB_TargArmorDesc
            // 
            this.LB_TargArmorDesc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LB_TargArmorDesc.Location = new System.Drawing.Point(3, 16);
            this.LB_TargArmorDesc.Name = "LB_TargArmorDesc";
            this.LB_TargArmorDesc.Size = new System.Drawing.Size(249, 171);
            this.LB_TargArmorDesc.TabIndex = 0;
            // 
            // CB_TargArmor
            // 
            this.CB_TargArmor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CB_TargArmor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CB_TargArmor.FormattingEnabled = true;
            this.CB_TargArmor.Items.AddRange(new object[] {
            "10900",
            "12000",
            "13083"});
            this.CB_TargArmor.Location = new System.Drawing.Point(127, 30);
            this.CB_TargArmor.Name = "CB_TargArmor";
            this.CB_TargArmor.Size = new System.Drawing.Size(131, 21);
            this.CB_TargArmor.TabIndex = 3;
            this.CB_TargArmor.SelectedIndexChanged += new System.EventHandler(this.comboBoxArmorBosses_SelectedIndexChanged);
            // 
            // GB_Bosses
            // 
            this.TLP_Main.SetColumnSpan(this.GB_Bosses, 2);
            this.GB_Bosses.Controls.Add(this.LB_TargArmorDesc);
            this.GB_Bosses.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GB_Bosses.Location = new System.Drawing.Point(3, 239);
            this.GB_Bosses.Name = "GB_Bosses";
            this.GB_Bosses.Size = new System.Drawing.Size(255, 190);
            this.GB_Bosses.TabIndex = 9;
            this.GB_Bosses.TabStop = false;
            this.GB_Bosses.Text = "Bosses";
            // 
            // CB_ToughLvl
            // 
            this.CB_ToughLvl.Dock = System.Windows.Forms.DockStyle.Fill;
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
            this.CB_ToughLvl.Location = new System.Drawing.Point(127, 57);
            this.CB_ToughLvl.Name = "CB_ToughLvl";
            this.CB_ToughLvl.Size = new System.Drawing.Size(131, 21);
            this.CB_ToughLvl.TabIndex = 5;
            this.CB_ToughLvl.SelectedIndexChanged += new System.EventHandler(this.CB_ToughLvl_SelectedIndexChanged);
            // 
            // LB_ToughLvl
            // 
            this.LB_ToughLvl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LB_ToughLvl.Location = new System.Drawing.Point(3, 54);
            this.LB_ToughLvl.Name = "LB_ToughLvl";
            this.LB_ToughLvl.Size = new System.Drawing.Size(118, 27);
            this.LB_ToughLvl.TabIndex = 4;
            this.LB_ToughLvl.Text = "Toughness Lvl:";
            this.LB_ToughLvl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // CB_Duration
            // 
            this.CB_Duration.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CB_Duration.Enabled = false;
            this.CB_Duration.Location = new System.Drawing.Point(127, 107);
            this.CB_Duration.Maximum = new decimal(new int[] {
            1200,
            0,
            0,
            0});
            this.CB_Duration.Minimum = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.CB_Duration.Name = "CB_Duration";
            this.CB_Duration.Size = new System.Drawing.Size(131, 20);
            this.CB_Duration.TabIndex = 7;
            this.CB_Duration.ThousandsSeparator = true;
            this.CB_Duration.Value = new decimal(new int[] {
            300,
            0,
            0,
            0});
            this.CB_Duration.ValueChanged += new System.EventHandler(this.CB_Duration_ValueChanged);
            // 
            // LB_Duration
            // 
            this.LB_Duration.AutoSize = true;
            this.LB_Duration.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LB_Duration.Enabled = false;
            this.LB_Duration.Location = new System.Drawing.Point(3, 104);
            this.LB_Duration.Name = "LB_Duration";
            this.LB_Duration.Size = new System.Drawing.Size(118, 26);
            this.LB_Duration.TabIndex = 6;
            this.LB_Duration.Text = "Duration (sec):";
            this.LB_Duration.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // GB_Rotation
            // 
            this.TLP_Main.SetColumnSpan(this.GB_Rotation, 2);
            this.GB_Rotation.Controls.Add(this.TLP_Rotation);
            this.GB_Rotation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GB_Rotation.Location = new System.Drawing.Point(3, 133);
            this.GB_Rotation.Name = "GB_Rotation";
            this.GB_Rotation.Size = new System.Drawing.Size(255, 100);
            this.GB_Rotation.TabIndex = 8;
            this.GB_Rotation.TabStop = false;
            this.GB_Rotation.Text = "Customize Rotation";
            // 
            // TLP_Rotation
            // 
            this.TLP_Rotation.ColumnCount = 2;
            this.TLP_Rotation.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.TLP_Rotation.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.TLP_Rotation.Controls.Add(this.RB_TargMultiple, 1, 0);
            this.TLP_Rotation.Controls.Add(this.RB_TargSingle, 0, 0);
            this.TLP_Rotation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TLP_Rotation.Location = new System.Drawing.Point(3, 16);
            this.TLP_Rotation.Name = "TLP_Rotation";
            this.TLP_Rotation.RowCount = 2;
            this.TLP_Rotation.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.TLP_Rotation.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TLP_Rotation.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.TLP_Rotation.Size = new System.Drawing.Size(249, 81);
            this.TLP_Rotation.TabIndex = 0;
            // 
            // RB_TargMultiple
            // 
            this.RB_TargMultiple.AutoSize = true;
            this.RB_TargMultiple.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RB_TargMultiple.Location = new System.Drawing.Point(127, 3);
            this.RB_TargMultiple.Name = "RB_TargMultiple";
            this.RB_TargMultiple.Size = new System.Drawing.Size(119, 17);
            this.RB_TargMultiple.TabIndex = 5;
            this.RB_TargMultiple.Text = "Multiple Targeting";
            this.RB_TargMultiple.UseVisualStyleBackColor = true;
            this.RB_TargMultiple.CheckedChanged += new System.EventHandler(this.RB_TargSingle_CheckedChanged);
            // 
            // RB_TargSingle
            // 
            this.RB_TargSingle.AutoSize = true;
            this.RB_TargSingle.Checked = true;
            this.RB_TargSingle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RB_TargSingle.Location = new System.Drawing.Point(3, 3);
            this.RB_TargSingle.Name = "RB_TargSingle";
            this.RB_TargSingle.Size = new System.Drawing.Size(118, 17);
            this.RB_TargSingle.TabIndex = 4;
            this.RB_TargSingle.TabStop = true;
            this.RB_TargSingle.Text = "Single Targeting";
            this.RB_TargSingle.UseVisualStyleBackColor = true;
            this.RB_TargSingle.CheckedChanged += new System.EventHandler(this.RB_TargSingle_CheckedChanged);
            // 
            // TLP_Main
            // 
            this.TLP_Main.ColumnCount = 2;
            this.TLP_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 47.89272F));
            this.TLP_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 52.10728F));
            this.TLP_Main.Controls.Add(this.LB_Duration, 0, 4);
            this.TLP_Main.Controls.Add(this.CB_Duration, 1, 4);
            this.TLP_Main.Controls.Add(this.RB_StanceArms, 1, 3);
            this.TLP_Main.Controls.Add(this.RB_StanceFury, 0, 3);
            this.TLP_Main.Controls.Add(this.GB_Bosses, 0, 6);
            this.TLP_Main.Controls.Add(this.GB_Rotation, 0, 5);
            this.TLP_Main.Controls.Add(this.LB_TargLvl, 0, 0);
            this.TLP_Main.Controls.Add(this.LB_TargArmor, 0, 1);
            this.TLP_Main.Controls.Add(this.CB_TargArmor, 1, 1);
            this.TLP_Main.Controls.Add(this.CB_ToughLvl, 1, 2);
            this.TLP_Main.Controls.Add(this.CB_TargLvl, 1, 0);
            this.TLP_Main.Controls.Add(this.LB_ToughLvl, 0, 2);
            this.TLP_Main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TLP_Main.Location = new System.Drawing.Point(0, 0);
            this.TLP_Main.Name = "TLP_Main";
            this.TLP_Main.RowCount = 7;
            this.TLP_Main.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.TLP_Main.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.TLP_Main.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.TLP_Main.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.TLP_Main.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.TLP_Main.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.TLP_Main.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.TLP_Main.Size = new System.Drawing.Size(261, 432);
            this.TLP_Main.TabIndex = 12;
            // 
            // RB_StanceArms
            // 
            this.RB_StanceArms.AutoSize = true;
            this.RB_StanceArms.Location = new System.Drawing.Point(127, 84);
            this.RB_StanceArms.Name = "RB_StanceArms";
            this.RB_StanceArms.Size = new System.Drawing.Size(85, 17);
            this.RB_StanceArms.TabIndex = 5;
            this.RB_StanceArms.Text = "Arms Stance";
            this.RB_StanceArms.UseVisualStyleBackColor = true;
            this.RB_StanceArms.CheckedChanged += new System.EventHandler(this.RB_StanceFury_CheckedChanged);
            // 
            // RB_StanceFury
            // 
            this.RB_StanceFury.AutoSize = true;
            this.RB_StanceFury.Checked = true;
            this.RB_StanceFury.Location = new System.Drawing.Point(3, 84);
            this.RB_StanceFury.Name = "RB_StanceFury";
            this.RB_StanceFury.Size = new System.Drawing.Size(82, 17);
            this.RB_StanceFury.TabIndex = 4;
            this.RB_StanceFury.TabStop = true;
            this.RB_StanceFury.Text = "Fury Stance";
            this.RB_StanceFury.UseVisualStyleBackColor = true;
            this.RB_StanceFury.CheckedChanged += new System.EventHandler(this.RB_StanceFury_CheckedChanged);
            // 
            // CalculationOptionsPanelDPSWarr
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.TLP_Main);
            this.Name = "CalculationOptionsPanelDPSWarr";
            this.Size = new System.Drawing.Size(261, 432);
            this.GB_Bosses.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.CB_Duration)).EndInit();
            this.GB_Rotation.ResumeLayout(false);
            this.TLP_Rotation.ResumeLayout(false);
            this.TLP_Rotation.PerformLayout();
            this.TLP_Main.ResumeLayout(false);
            this.TLP_Main.PerformLayout();
            this.ResumeLayout(false);

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
        private System.Windows.Forms.NumericUpDown CB_Duration;
        private System.Windows.Forms.Label LB_Duration;
        private System.Windows.Forms.TableLayoutPanel TLP_Main;
        private System.Windows.Forms.GroupBox GB_Rotation;
        private System.Windows.Forms.TableLayoutPanel TLP_Rotation;
        private System.Windows.Forms.RadioButton RB_StanceArms;
        private System.Windows.Forms.RadioButton RB_StanceFury;
        private System.Windows.Forms.RadioButton RB_TargMultiple;
        private System.Windows.Forms.RadioButton RB_TargSingle;
    }
}
