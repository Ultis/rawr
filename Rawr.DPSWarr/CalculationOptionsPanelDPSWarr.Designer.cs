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
            this.CB_TargArmor = new System.Windows.Forms.ComboBox();
            this.CB_Duration = new System.Windows.Forms.NumericUpDown();
            this.LB_Duration = new System.Windows.Forms.Label();
            this.TLP_Main = new System.Windows.Forms.TableLayoutPanel();
            this.RB_StanceArms = new System.Windows.Forms.RadioButton();
            this.RB_StanceFury = new System.Windows.Forms.RadioButton();
            this.GB_Bosses = new System.Windows.Forms.GroupBox();
            this.LB_TargArmorDesc = new System.Windows.Forms.Label();
            this.GB_Rots = new System.Windows.Forms.GroupBox();
            this.LB_Perc5 = new System.Windows.Forms.Label();
            this.CB_InBackPerc = new System.Windows.Forms.NumericUpDown();
            this.CK_InBack = new System.Windows.Forms.CheckBox();
            this.GB_Maintenance = new System.Windows.Forms.GroupBox();
            this.CK_BattleShout = new System.Windows.Forms.CheckBox();
            this.CK_Hamstring = new System.Windows.Forms.CheckBox();
            this.CK_DemoShout = new System.Windows.Forms.CheckBox();
            this.CK_Sunder = new System.Windows.Forms.CheckBox();
            this.CK_Thunder = new System.Windows.Forms.CheckBox();
            this.LB_Perc4 = new System.Windows.Forms.Label();
            this.CB_DisarmingTargsPerc = new System.Windows.Forms.NumericUpDown();
            this.CK_DisarmTargs = new System.Windows.Forms.CheckBox();
            this.LB_Perc3 = new System.Windows.Forms.Label();
            this.CB_StunningTargsPerc = new System.Windows.Forms.NumericUpDown();
            this.CK_StunningTargs = new System.Windows.Forms.CheckBox();
            this.LB_Perc2 = new System.Windows.Forms.Label();
            this.LB_Perc1 = new System.Windows.Forms.Label();
            this.CB_MoveTargsPerc = new System.Windows.Forms.NumericUpDown();
            this.CB_MultiTargsPerc = new System.Windows.Forms.NumericUpDown();
            this.CK_MovingTargs = new System.Windows.Forms.CheckBox();
            this.CK_MultiTargs = new System.Windows.Forms.CheckBox();
            this.PN_Lag = new System.Windows.Forms.Panel();
            this.LB_Lag = new System.Windows.Forms.Label();
            this.CB_Lag = new System.Windows.Forms.NumericUpDown();
            this.PN_React = new System.Windows.Forms.Panel();
            this.LB_React = new System.Windows.Forms.Label();
            this.CB_React = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.CB_Duration)).BeginInit();
            this.TLP_Main.SuspendLayout();
            this.GB_Bosses.SuspendLayout();
            this.GB_Rots.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CB_InBackPerc)).BeginInit();
            this.GB_Maintenance.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CB_DisarmingTargsPerc)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CB_StunningTargsPerc)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CB_MoveTargsPerc)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CB_MultiTargsPerc)).BeginInit();
            this.PN_Lag.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CB_Lag)).BeginInit();
            this.PN_React.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CB_React)).BeginInit();
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
            this.CB_TargLvl.Location = new System.Drawing.Point(133, 3);
            this.CB_TargLvl.Name = "CB_TargLvl";
            this.CB_TargLvl.Size = new System.Drawing.Size(125, 21);
            this.CB_TargLvl.TabIndex = 1;
            this.CB_TargLvl.SelectedIndexChanged += new System.EventHandler(this.comboBoxTargetLevel_SelectedIndexChanged);
            // 
            // LB_TargLvl
            // 
            this.LB_TargLvl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LB_TargLvl.Location = new System.Drawing.Point(3, 0);
            this.LB_TargLvl.Name = "LB_TargLvl";
            this.LB_TargLvl.Size = new System.Drawing.Size(124, 27);
            this.LB_TargLvl.TabIndex = 0;
            this.LB_TargLvl.Text = "Target Level:";
            this.LB_TargLvl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // LB_TargArmor
            // 
            this.LB_TargArmor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LB_TargArmor.Location = new System.Drawing.Point(3, 27);
            this.LB_TargArmor.Name = "LB_TargArmor";
            this.LB_TargArmor.Size = new System.Drawing.Size(124, 27);
            this.LB_TargArmor.TabIndex = 2;
            this.LB_TargArmor.Text = "Target Armor:";
            this.LB_TargArmor.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // CB_TargArmor
            // 
            this.CB_TargArmor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CB_TargArmor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CB_TargArmor.FormattingEnabled = true;
            this.CB_TargArmor.Items.AddRange(new object[] {
            "10643",
            "10900",
            "12000",
            "13083"});
            this.CB_TargArmor.Location = new System.Drawing.Point(133, 30);
            this.CB_TargArmor.Name = "CB_TargArmor";
            this.CB_TargArmor.Size = new System.Drawing.Size(125, 21);
            this.CB_TargArmor.TabIndex = 3;
            this.CB_TargArmor.SelectedIndexChanged += new System.EventHandler(this.comboBoxArmorBosses_SelectedIndexChanged);
            // 
            // CB_Duration
            // 
            this.CB_Duration.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CB_Duration.Location = new System.Drawing.Point(133, 80);
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
            this.CB_Duration.Size = new System.Drawing.Size(125, 20);
            this.CB_Duration.TabIndex = 9;
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
            this.LB_Duration.Location = new System.Drawing.Point(3, 77);
            this.LB_Duration.Name = "LB_Duration";
            this.LB_Duration.Size = new System.Drawing.Size(124, 26);
            this.LB_Duration.TabIndex = 8;
            this.LB_Duration.Text = "Duration (sec):";
            this.LB_Duration.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // TLP_Main
            // 
            this.TLP_Main.ColumnCount = 2;
            this.TLP_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.TLP_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.TLP_Main.Controls.Add(this.LB_Duration, 0, 3);
            this.TLP_Main.Controls.Add(this.CB_Duration, 1, 3);
            this.TLP_Main.Controls.Add(this.RB_StanceArms, 1, 2);
            this.TLP_Main.Controls.Add(this.RB_StanceFury, 0, 2);
            this.TLP_Main.Controls.Add(this.GB_Bosses, 0, 6);
            this.TLP_Main.Controls.Add(this.LB_TargLvl, 0, 0);
            this.TLP_Main.Controls.Add(this.LB_TargArmor, 0, 1);
            this.TLP_Main.Controls.Add(this.CB_TargArmor, 1, 1);
            this.TLP_Main.Controls.Add(this.CB_TargLvl, 1, 0);
            this.TLP_Main.Controls.Add(this.GB_Rots, 0, 5);
            this.TLP_Main.Controls.Add(this.PN_Lag, 0, 4);
            this.TLP_Main.Controls.Add(this.PN_React, 1, 4);
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
            this.TLP_Main.Size = new System.Drawing.Size(261, 527);
            this.TLP_Main.TabIndex = 0;
            // 
            // RB_StanceArms
            // 
            this.RB_StanceArms.AutoSize = true;
            this.RB_StanceArms.Location = new System.Drawing.Point(133, 57);
            this.RB_StanceArms.Name = "RB_StanceArms";
            this.RB_StanceArms.Size = new System.Drawing.Size(85, 17);
            this.RB_StanceArms.TabIndex = 7;
            this.RB_StanceArms.Text = "Arms Stance";
            this.RB_StanceArms.UseVisualStyleBackColor = true;
            this.RB_StanceArms.CheckedChanged += new System.EventHandler(this.RB_StanceFury_CheckedChanged);
            // 
            // RB_StanceFury
            // 
            this.RB_StanceFury.AutoSize = true;
            this.RB_StanceFury.Checked = true;
            this.RB_StanceFury.Location = new System.Drawing.Point(3, 57);
            this.RB_StanceFury.Name = "RB_StanceFury";
            this.RB_StanceFury.Size = new System.Drawing.Size(82, 17);
            this.RB_StanceFury.TabIndex = 6;
            this.RB_StanceFury.TabStop = true;
            this.RB_StanceFury.Text = "Fury Stance";
            this.RB_StanceFury.UseVisualStyleBackColor = true;
            this.RB_StanceFury.CheckedChanged += new System.EventHandler(this.RB_StanceFury_CheckedChanged);
            // 
            // GB_Bosses
            // 
            this.TLP_Main.SetColumnSpan(this.GB_Bosses, 2);
            this.GB_Bosses.Controls.Add(this.LB_TargArmorDesc);
            this.GB_Bosses.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GB_Bosses.Location = new System.Drawing.Point(3, 378);
            this.GB_Bosses.Name = "GB_Bosses";
            this.GB_Bosses.Size = new System.Drawing.Size(255, 146);
            this.GB_Bosses.TabIndex = 12;
            this.GB_Bosses.TabStop = false;
            this.GB_Bosses.Text = "Bosses";
            // 
            // LB_TargArmorDesc
            // 
            this.LB_TargArmorDesc.AutoSize = true;
            this.LB_TargArmorDesc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LB_TargArmorDesc.Location = new System.Drawing.Point(3, 16);
            this.LB_TargArmorDesc.Name = "LB_TargArmorDesc";
            this.LB_TargArmorDesc.Size = new System.Drawing.Size(0, 13);
            this.LB_TargArmorDesc.TabIndex = 0;
            // 
            // GB_Rots
            // 
            this.TLP_Main.SetColumnSpan(this.GB_Rots, 2);
            this.GB_Rots.Controls.Add(this.LB_Perc5);
            this.GB_Rots.Controls.Add(this.CB_InBackPerc);
            this.GB_Rots.Controls.Add(this.CK_InBack);
            this.GB_Rots.Controls.Add(this.GB_Maintenance);
            this.GB_Rots.Controls.Add(this.LB_Perc4);
            this.GB_Rots.Controls.Add(this.CB_DisarmingTargsPerc);
            this.GB_Rots.Controls.Add(this.CK_DisarmTargs);
            this.GB_Rots.Controls.Add(this.LB_Perc3);
            this.GB_Rots.Controls.Add(this.CB_StunningTargsPerc);
            this.GB_Rots.Controls.Add(this.CK_StunningTargs);
            this.GB_Rots.Controls.Add(this.LB_Perc2);
            this.GB_Rots.Controls.Add(this.LB_Perc1);
            this.GB_Rots.Controls.Add(this.CB_MoveTargsPerc);
            this.GB_Rots.Controls.Add(this.CB_MultiTargsPerc);
            this.GB_Rots.Controls.Add(this.CK_MovingTargs);
            this.GB_Rots.Controls.Add(this.CK_MultiTargs);
            this.GB_Rots.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GB_Rots.Location = new System.Drawing.Point(3, 132);
            this.GB_Rots.Name = "GB_Rots";
            this.GB_Rots.Size = new System.Drawing.Size(255, 240);
            this.GB_Rots.TabIndex = 10;
            this.GB_Rots.TabStop = false;
            this.GB_Rots.Text = "Rotational Changes";
            // 
            // LB_Perc5
            // 
            this.LB_Perc5.AutoSize = true;
            this.LB_Perc5.Location = new System.Drawing.Point(234, 124);
            this.LB_Perc5.Name = "LB_Perc5";
            this.LB_Perc5.Size = new System.Drawing.Size(15, 13);
            this.LB_Perc5.TabIndex = 17;
            this.LB_Perc5.Text = "%";
            // 
            // CB_InBackPerc
            // 
            this.CB_InBackPerc.Location = new System.Drawing.Point(129, 122);
            this.CB_InBackPerc.Name = "CB_InBackPerc";
            this.CB_InBackPerc.Size = new System.Drawing.Size(103, 20);
            this.CB_InBackPerc.TabIndex = 16;
            this.CB_InBackPerc.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.CB_InBackPerc.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.CB_InBackPerc.ValueChanged += new System.EventHandler(this.CB_InBack_ValueChanged);
            // 
            // CK_InBack
            // 
            this.CK_InBack.AutoSize = true;
            this.CK_InBack.Checked = true;
            this.CK_InBack.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CK_InBack.Location = new System.Drawing.Point(6, 123);
            this.CK_InBack.Name = "CK_InBack";
            this.CK_InBack.Size = new System.Drawing.Size(107, 17);
            this.CK_InBack.TabIndex = 15;
            this.CK_InBack.Text = "Standing in Back";
            this.CK_InBack.UseVisualStyleBackColor = true;
            this.CK_InBack.CheckedChanged += new System.EventHandler(this.CK_InBack_CheckedChanged);
            // 
            // GB_Maintenance
            // 
            this.GB_Maintenance.Controls.Add(this.CK_BattleShout);
            this.GB_Maintenance.Controls.Add(this.CK_Hamstring);
            this.GB_Maintenance.Controls.Add(this.CK_DemoShout);
            this.GB_Maintenance.Controls.Add(this.CK_Sunder);
            this.GB_Maintenance.Controls.Add(this.CK_Thunder);
            this.GB_Maintenance.Location = new System.Drawing.Point(6, 148);
            this.GB_Maintenance.Name = "GB_Maintenance";
            this.GB_Maintenance.Size = new System.Drawing.Size(243, 87);
            this.GB_Maintenance.TabIndex = 14;
            this.GB_Maintenance.TabStop = false;
            this.GB_Maintenance.Text = "Abilities to Maintain";
            // 
            // CK_BattleShout
            // 
            this.CK_BattleShout.AutoSize = true;
            this.CK_BattleShout.Location = new System.Drawing.Point(123, 42);
            this.CK_BattleShout.Name = "CK_BattleShout";
            this.CK_BattleShout.Size = new System.Drawing.Size(84, 17);
            this.CK_BattleShout.TabIndex = 17;
            this.CK_BattleShout.Text = "Battle Shout";
            this.CK_BattleShout.UseVisualStyleBackColor = true;
            this.CK_BattleShout.CheckedChanged += new System.EventHandler(this.CK_Maints_CheckedChanged);
            // 
            // CK_Hamstring
            // 
            this.CK_Hamstring.AutoSize = true;
            this.CK_Hamstring.Location = new System.Drawing.Point(123, 19);
            this.CK_Hamstring.Name = "CK_Hamstring";
            this.CK_Hamstring.Size = new System.Drawing.Size(73, 17);
            this.CK_Hamstring.TabIndex = 16;
            this.CK_Hamstring.Text = "Hamstring";
            this.CK_Hamstring.UseVisualStyleBackColor = true;
            this.CK_Hamstring.CheckedChanged += new System.EventHandler(this.CK_Maints_CheckedChanged);
            // 
            // CK_DemoShout
            // 
            this.CK_DemoShout.AutoSize = true;
            this.CK_DemoShout.Location = new System.Drawing.Point(6, 65);
            this.CK_DemoShout.Name = "CK_DemoShout";
            this.CK_DemoShout.Size = new System.Drawing.Size(117, 17);
            this.CK_DemoShout.TabIndex = 15;
            this.CK_DemoShout.Text = "Demoralizing Shout";
            this.CK_DemoShout.UseVisualStyleBackColor = true;
            this.CK_DemoShout.CheckedChanged += new System.EventHandler(this.CK_Maints_CheckedChanged);
            // 
            // CK_Sunder
            // 
            this.CK_Sunder.AutoSize = true;
            this.CK_Sunder.Location = new System.Drawing.Point(6, 42);
            this.CK_Sunder.Name = "CK_Sunder";
            this.CK_Sunder.Size = new System.Drawing.Size(90, 17);
            this.CK_Sunder.TabIndex = 14;
            this.CK_Sunder.Text = "Sunder Armor";
            this.CK_Sunder.UseVisualStyleBackColor = true;
            this.CK_Sunder.CheckedChanged += new System.EventHandler(this.CK_Maints_CheckedChanged);
            // 
            // CK_Thunder
            // 
            this.CK_Thunder.AutoSize = true;
            this.CK_Thunder.Location = new System.Drawing.Point(6, 19);
            this.CK_Thunder.Name = "CK_Thunder";
            this.CK_Thunder.Size = new System.Drawing.Size(86, 17);
            this.CK_Thunder.TabIndex = 13;
            this.CK_Thunder.Text = "Thunderclap";
            this.CK_Thunder.UseVisualStyleBackColor = true;
            this.CK_Thunder.CheckedChanged += new System.EventHandler(this.CK_Maints_CheckedChanged);
            // 
            // LB_Perc4
            // 
            this.LB_Perc4.AutoSize = true;
            this.LB_Perc4.Location = new System.Drawing.Point(234, 98);
            this.LB_Perc4.Name = "LB_Perc4";
            this.LB_Perc4.Size = new System.Drawing.Size(15, 13);
            this.LB_Perc4.TabIndex = 12;
            this.LB_Perc4.Text = "%";
            // 
            // CB_DisarmingTargsPerc
            // 
            this.CB_DisarmingTargsPerc.Enabled = false;
            this.CB_DisarmingTargsPerc.Location = new System.Drawing.Point(129, 96);
            this.CB_DisarmingTargsPerc.Name = "CB_DisarmingTargsPerc";
            this.CB_DisarmingTargsPerc.Size = new System.Drawing.Size(103, 20);
            this.CB_DisarmingTargsPerc.TabIndex = 11;
            this.CB_DisarmingTargsPerc.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.CB_DisarmingTargsPerc.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.CB_DisarmingTargsPerc.ValueChanged += new System.EventHandler(this.CB_DisarmingTargs_ValueChanged);
            // 
            // CK_DisarmTargs
            // 
            this.CK_DisarmTargs.AutoSize = true;
            this.CK_DisarmTargs.Location = new System.Drawing.Point(6, 97);
            this.CK_DisarmTargs.Name = "CK_DisarmTargs";
            this.CK_DisarmTargs.Size = new System.Drawing.Size(111, 17);
            this.CK_DisarmTargs.TabIndex = 10;
            this.CK_DisarmTargs.Text = "Disarming Targets";
            this.CK_DisarmTargs.UseVisualStyleBackColor = true;
            this.CK_DisarmTargs.CheckedChanged += new System.EventHandler(this.CK_DisarmingTargs_CheckedChanged);
            // 
            // LB_Perc3
            // 
            this.LB_Perc3.AutoSize = true;
            this.LB_Perc3.Location = new System.Drawing.Point(234, 72);
            this.LB_Perc3.Name = "LB_Perc3";
            this.LB_Perc3.Size = new System.Drawing.Size(15, 13);
            this.LB_Perc3.TabIndex = 9;
            this.LB_Perc3.Text = "%";
            // 
            // CB_StunningTargsPerc
            // 
            this.CB_StunningTargsPerc.Enabled = false;
            this.CB_StunningTargsPerc.Location = new System.Drawing.Point(129, 70);
            this.CB_StunningTargsPerc.Name = "CB_StunningTargsPerc";
            this.CB_StunningTargsPerc.Size = new System.Drawing.Size(103, 20);
            this.CB_StunningTargsPerc.TabIndex = 8;
            this.CB_StunningTargsPerc.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.CB_StunningTargsPerc.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.CB_StunningTargsPerc.ValueChanged += new System.EventHandler(this.CB_StunningTargs_ValueChanged);
            // 
            // CK_StunningTargs
            // 
            this.CK_StunningTargs.AutoSize = true;
            this.CK_StunningTargs.Location = new System.Drawing.Point(6, 71);
            this.CK_StunningTargs.Name = "CK_StunningTargs";
            this.CK_StunningTargs.Size = new System.Drawing.Size(107, 17);
            this.CK_StunningTargs.TabIndex = 7;
            this.CK_StunningTargs.Text = "Stunning Targets";
            this.CK_StunningTargs.UseVisualStyleBackColor = true;
            this.CK_StunningTargs.CheckedChanged += new System.EventHandler(this.CK_StunningTargs_CheckedChanged);
            // 
            // LB_Perc2
            // 
            this.LB_Perc2.AutoSize = true;
            this.LB_Perc2.Location = new System.Drawing.Point(234, 46);
            this.LB_Perc2.Name = "LB_Perc2";
            this.LB_Perc2.Size = new System.Drawing.Size(15, 13);
            this.LB_Perc2.TabIndex = 6;
            this.LB_Perc2.Text = "%";
            // 
            // LB_Perc1
            // 
            this.LB_Perc1.AutoSize = true;
            this.LB_Perc1.Location = new System.Drawing.Point(234, 20);
            this.LB_Perc1.Name = "LB_Perc1";
            this.LB_Perc1.Size = new System.Drawing.Size(15, 13);
            this.LB_Perc1.TabIndex = 5;
            this.LB_Perc1.Text = "%";
            // 
            // CB_MoveTargsPerc
            // 
            this.CB_MoveTargsPerc.Enabled = false;
            this.CB_MoveTargsPerc.Location = new System.Drawing.Point(129, 44);
            this.CB_MoveTargsPerc.Name = "CB_MoveTargsPerc";
            this.CB_MoveTargsPerc.Size = new System.Drawing.Size(103, 20);
            this.CB_MoveTargsPerc.TabIndex = 4;
            this.CB_MoveTargsPerc.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.CB_MoveTargsPerc.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.CB_MoveTargsPerc.ValueChanged += new System.EventHandler(this.CB_MovingTargs_ValueChanged);
            // 
            // CB_MultiTargsPerc
            // 
            this.CB_MultiTargsPerc.Enabled = false;
            this.CB_MultiTargsPerc.Location = new System.Drawing.Point(129, 18);
            this.CB_MultiTargsPerc.Name = "CB_MultiTargsPerc";
            this.CB_MultiTargsPerc.Size = new System.Drawing.Size(103, 20);
            this.CB_MultiTargsPerc.TabIndex = 3;
            this.CB_MultiTargsPerc.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.CB_MultiTargsPerc.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.CB_MultiTargsPerc.ValueChanged += new System.EventHandler(this.CB_MultiTargs_ValueChanged);
            // 
            // CK_MovingTargs
            // 
            this.CK_MovingTargs.AutoSize = true;
            this.CK_MovingTargs.Location = new System.Drawing.Point(6, 45);
            this.CK_MovingTargs.Name = "CK_MovingTargs";
            this.CK_MovingTargs.Size = new System.Drawing.Size(100, 17);
            this.CK_MovingTargs.TabIndex = 2;
            this.CK_MovingTargs.Text = "Moving Targets";
            this.CK_MovingTargs.UseVisualStyleBackColor = true;
            this.CK_MovingTargs.CheckedChanged += new System.EventHandler(this.CK_MovingTargs_CheckedChanged);
            // 
            // CK_MultiTargs
            // 
            this.CK_MultiTargs.AutoSize = true;
            this.CK_MultiTargs.Location = new System.Drawing.Point(6, 19);
            this.CK_MultiTargs.Name = "CK_MultiTargs";
            this.CK_MultiTargs.Size = new System.Drawing.Size(101, 17);
            this.CK_MultiTargs.TabIndex = 1;
            this.CK_MultiTargs.Text = "Multiple Targets";
            this.CK_MultiTargs.UseVisualStyleBackColor = true;
            this.CK_MultiTargs.CheckedChanged += new System.EventHandler(this.CK_MultiTargs_CheckedChanged);
            // 
            // PN_Lag
            // 
            this.PN_Lag.AutoSize = true;
            this.PN_Lag.Controls.Add(this.LB_Lag);
            this.PN_Lag.Controls.Add(this.CB_Lag);
            this.PN_Lag.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PN_Lag.Location = new System.Drawing.Point(0, 103);
            this.PN_Lag.Margin = new System.Windows.Forms.Padding(0);
            this.PN_Lag.Name = "PN_Lag";
            this.PN_Lag.Size = new System.Drawing.Size(130, 26);
            this.PN_Lag.TabIndex = 13;
            // 
            // LB_Lag
            // 
            this.LB_Lag.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.LB_Lag.AutoSize = true;
            this.LB_Lag.Location = new System.Drawing.Point(3, 5);
            this.LB_Lag.Name = "LB_Lag";
            this.LB_Lag.Size = new System.Drawing.Size(25, 13);
            this.LB_Lag.TabIndex = 1;
            this.LB_Lag.Text = "Lag";
            // 
            // CB_Lag
            // 
            this.CB_Lag.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.CB_Lag.Location = new System.Drawing.Point(53, 3);
            this.CB_Lag.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.CB_Lag.Name = "CB_Lag";
            this.CB_Lag.Size = new System.Drawing.Size(75, 20);
            this.CB_Lag.TabIndex = 0;
            this.CB_Lag.ValueChanged += new System.EventHandler(this.CB_Latency_ValueChanged);
            // 
            // PN_React
            // 
            this.PN_React.AutoSize = true;
            this.PN_React.Controls.Add(this.LB_React);
            this.PN_React.Controls.Add(this.CB_React);
            this.PN_React.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PN_React.Location = new System.Drawing.Point(130, 103);
            this.PN_React.Margin = new System.Windows.Forms.Padding(0);
            this.PN_React.Name = "PN_React";
            this.PN_React.Size = new System.Drawing.Size(131, 26);
            this.PN_React.TabIndex = 14;
            // 
            // LB_React
            // 
            this.LB_React.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.LB_React.AutoSize = true;
            this.LB_React.Location = new System.Drawing.Point(3, 5);
            this.LB_React.Name = "LB_React";
            this.LB_React.Size = new System.Drawing.Size(36, 13);
            this.LB_React.TabIndex = 1;
            this.LB_React.Text = "React";
            // 
            // CB_React
            // 
            this.CB_React.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.CB_React.Location = new System.Drawing.Point(53, 3);
            this.CB_React.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.CB_React.Name = "CB_React";
            this.CB_React.Size = new System.Drawing.Size(75, 20);
            this.CB_React.TabIndex = 0;
            this.CB_React.Value = new decimal(new int[] {
            220,
            0,
            0,
            0});
            this.CB_React.ValueChanged += new System.EventHandler(this.CB_Latency_ValueChanged);
            // 
            // CalculationOptionsPanelDPSWarr
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.TLP_Main);
            this.Name = "CalculationOptionsPanelDPSWarr";
            this.Size = new System.Drawing.Size(261, 527);
            ((System.ComponentModel.ISupportInitialize)(this.CB_Duration)).EndInit();
            this.TLP_Main.ResumeLayout(false);
            this.TLP_Main.PerformLayout();
            this.GB_Bosses.ResumeLayout(false);
            this.GB_Bosses.PerformLayout();
            this.GB_Rots.ResumeLayout(false);
            this.GB_Rots.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CB_InBackPerc)).EndInit();
            this.GB_Maintenance.ResumeLayout(false);
            this.GB_Maintenance.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CB_DisarmingTargsPerc)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CB_StunningTargsPerc)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CB_MoveTargsPerc)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CB_MultiTargsPerc)).EndInit();
            this.PN_Lag.ResumeLayout(false);
            this.PN_Lag.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CB_Lag)).EndInit();
            this.PN_React.ResumeLayout(false);
            this.PN_React.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CB_React)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.ComboBox CB_TargLvl;
        public System.Windows.Forms.Label LB_TargLvl;
        public System.Windows.Forms.Label LB_TargArmor;
        public System.Windows.Forms.ComboBox CB_TargArmor;
        private System.Windows.Forms.NumericUpDown CB_Duration;
        private System.Windows.Forms.Label LB_Duration;
        private System.Windows.Forms.TableLayoutPanel TLP_Main;
        private System.Windows.Forms.RadioButton RB_StanceArms;
        private System.Windows.Forms.RadioButton RB_StanceFury;
        public System.Windows.Forms.GroupBox GB_Bosses;
        public System.Windows.Forms.Label LB_TargArmorDesc;
        private System.Windows.Forms.GroupBox GB_Rots;
        private System.Windows.Forms.CheckBox CK_MovingTargs;
        private System.Windows.Forms.CheckBox CK_MultiTargs;
        private System.Windows.Forms.Label LB_Perc1;
        private System.Windows.Forms.NumericUpDown CB_MoveTargsPerc;
        private System.Windows.Forms.NumericUpDown CB_MultiTargsPerc;
        private System.Windows.Forms.Label LB_Perc2;
        private System.Windows.Forms.Label LB_Perc4;
        private System.Windows.Forms.NumericUpDown CB_DisarmingTargsPerc;
        private System.Windows.Forms.CheckBox CK_DisarmTargs;
        private System.Windows.Forms.Label LB_Perc3;
        private System.Windows.Forms.NumericUpDown CB_StunningTargsPerc;
        private System.Windows.Forms.CheckBox CK_StunningTargs;
        private System.Windows.Forms.GroupBox GB_Maintenance;
        private System.Windows.Forms.CheckBox CK_Thunder;
        private System.Windows.Forms.CheckBox CK_BattleShout;
        private System.Windows.Forms.CheckBox CK_Hamstring;
        private System.Windows.Forms.CheckBox CK_DemoShout;
        private System.Windows.Forms.CheckBox CK_Sunder;
        private System.Windows.Forms.Label LB_Perc5;
        private System.Windows.Forms.NumericUpDown CB_InBackPerc;
        private System.Windows.Forms.CheckBox CK_InBack;
        private System.Windows.Forms.Panel PN_Lag;
        private System.Windows.Forms.Panel PN_React;
        private System.Windows.Forms.Label LB_Lag;
        private System.Windows.Forms.NumericUpDown CB_Lag;
        private System.Windows.Forms.Label LB_React;
        private System.Windows.Forms.NumericUpDown CB_React;
    }
}
