namespace Rawr.Rogue {
    partial class CalculationOptionsPanelRogue {
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
            this.CB_TargLevel = new System.Windows.Forms.ComboBox();
            this.LB_TargLevel = new System.Windows.Forms.Label();
            this.LB_TargArmor = new System.Windows.Forms.Label();
            this.GB_Cycles = new System.Windows.Forms.GroupBox();
            this.CB_Finisher3 = new System.Windows.Forms.ComboBox();
            this.CB_ComboPoints3 = new System.Windows.Forms.ComboBox();
            this.CB_Finisher2 = new System.Windows.Forms.ComboBox();
            this.CB_ComboPoints2 = new System.Windows.Forms.ComboBox();
            this.CB_Finisher1 = new System.Windows.Forms.ComboBox();
            this.CB_ComboPoints1 = new System.Windows.Forms.ComboBox();
            this.GB_Poisons = new System.Windows.Forms.GroupBox();
            this.CB_PoisonOH = new System.Windows.Forms.ComboBox();
            this.LB_PoisonOH = new System.Windows.Forms.Label();
            this.CB_PoisonMH = new System.Windows.Forms.ComboBox();
            this.LB_PoisonMH = new System.Windows.Forms.Label();
            this.CB_TargArmor = new System.Windows.Forms.ComboBox();
            this.GB_FightInfo = new System.Windows.Forms.GroupBox();
            this.LB_Duration = new System.Windows.Forms.Label();
            this.NUD_Duration = new System.Windows.Forms.NumericUpDown();
            this.CB_CpGenerator = new System.Windows.Forms.ComboBox();
            this.NUD_Hat = new System.Windows.Forms.NumericUpDown();
            this.GB_CPG = new System.Windows.Forms.GroupBox();
            this.LB_HemoPerCycle = new System.Windows.Forms.Label();
            this.NUD_HemoPerCycle = new System.Windows.Forms.NumericUpDown();
            this.LB_CPperSec = new System.Windows.Forms.Label();
            this.LB_CPG = new System.Windows.Forms.Label();
            this.GB_Abilities = new System.Windows.Forms.GroupBox();
            this.CK_UseTurnTheTables = new System.Windows.Forms.CheckBox();
            this.LB_TurnTheTables = new System.Windows.Forms.Label();
            this.NUD_TurnTheTablesUptimePerc = new System.Windows.Forms.NumericUpDown();
            this.LB_FeintDelay = new System.Windows.Forms.Label();
            this.NUD_FeintDelay = new System.Windows.Forms.NumericUpDown();
            this.CK_UseFeint = new System.Windows.Forms.CheckBox();
            this.GB_Cycles.SuspendLayout();
            this.GB_Poisons.SuspendLayout();
            this.GB_FightInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_Duration)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_Hat)).BeginInit();
            this.GB_CPG.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_HemoPerCycle)).BeginInit();
            this.GB_Abilities.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_TurnTheTablesUptimePerc)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_FeintDelay)).BeginInit();
            this.SuspendLayout();
            // 
            // CB_TargLevel
            // 
            this.CB_TargLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CB_TargLevel.FormattingEnabled = true;
            this.CB_TargLevel.Location = new System.Drawing.Point(79, 19);
            this.CB_TargLevel.Name = "CB_TargLevel";
            this.CB_TargLevel.Size = new System.Drawing.Size(47, 21);
            this.CB_TargLevel.TabIndex = 0;
            this.CB_TargLevel.SelectedIndexChanged += new System.EventHandler(this.CB_TargLevel_SelectedIndexChanged);
            // 
            // LB_TargLevel
            // 
            this.LB_TargLevel.AutoSize = true;
            this.LB_TargLevel.Location = new System.Drawing.Point(6, 22);
            this.LB_TargLevel.Name = "LB_TargLevel";
            this.LB_TargLevel.Size = new System.Drawing.Size(70, 13);
            this.LB_TargLevel.TabIndex = 1;
            this.LB_TargLevel.Text = "Target Level:";
            // 
            // LB_TargArmor
            // 
            this.LB_TargArmor.AutoSize = true;
            this.LB_TargArmor.Location = new System.Drawing.Point(132, 22);
            this.LB_TargArmor.Name = "LB_TargArmor";
            this.LB_TargArmor.Size = new System.Drawing.Size(71, 13);
            this.LB_TargArmor.TabIndex = 3;
            this.LB_TargArmor.Text = "Target Armor:";
            // 
            // GB_Cycles
            // 
            this.GB_Cycles.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.GB_Cycles.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.GB_Cycles.Controls.Add(this.CB_Finisher3);
            this.GB_Cycles.Controls.Add(this.CB_ComboPoints3);
            this.GB_Cycles.Controls.Add(this.CB_Finisher2);
            this.GB_Cycles.Controls.Add(this.CB_ComboPoints2);
            this.GB_Cycles.Controls.Add(this.CB_Finisher1);
            this.GB_Cycles.Controls.Add(this.CB_ComboPoints1);
            this.GB_Cycles.Location = new System.Drawing.Point(3, 152);
            this.GB_Cycles.Name = "GB_Cycles";
            this.GB_Cycles.Size = new System.Drawing.Size(298, 107);
            this.GB_Cycles.TabIndex = 6;
            this.GB_Cycles.TabStop = false;
            this.GB_Cycles.Text = "Cycles";
            // 
            // CB_Finisher3
            // 
            this.CB_Finisher3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.CB_Finisher3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CB_Finisher3.FormattingEnabled = true;
            this.CB_Finisher3.Location = new System.Drawing.Point(104, 76);
            this.CB_Finisher3.Name = "CB_Finisher3";
            this.CB_Finisher3.Size = new System.Drawing.Size(188, 21);
            this.CB_Finisher3.TabIndex = 10;
            this.CB_Finisher3.SelectedIndexChanged += new System.EventHandler(this.CycleChanged);
            // 
            // CB_ComboPoints3
            // 
            this.CB_ComboPoints3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CB_ComboPoints3.FormattingEnabled = true;
            this.CB_ComboPoints3.Location = new System.Drawing.Point(11, 76);
            this.CB_ComboPoints3.Name = "CB_ComboPoints3";
            this.CB_ComboPoints3.Size = new System.Drawing.Size(87, 21);
            this.CB_ComboPoints3.TabIndex = 9;
            this.CB_ComboPoints3.SelectedIndexChanged += new System.EventHandler(this.CycleChanged);
            // 
            // CB_Finisher2
            // 
            this.CB_Finisher2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.CB_Finisher2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CB_Finisher2.FormattingEnabled = true;
            this.CB_Finisher2.Location = new System.Drawing.Point(104, 47);
            this.CB_Finisher2.Name = "CB_Finisher2";
            this.CB_Finisher2.Size = new System.Drawing.Size(188, 21);
            this.CB_Finisher2.TabIndex = 8;
            this.CB_Finisher2.SelectedIndexChanged += new System.EventHandler(this.CycleChanged);
            // 
            // CB_ComboPoints2
            // 
            this.CB_ComboPoints2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CB_ComboPoints2.FormattingEnabled = true;
            this.CB_ComboPoints2.Location = new System.Drawing.Point(11, 48);
            this.CB_ComboPoints2.Name = "CB_ComboPoints2";
            this.CB_ComboPoints2.Size = new System.Drawing.Size(87, 21);
            this.CB_ComboPoints2.TabIndex = 7;
            this.CB_ComboPoints2.SelectedIndexChanged += new System.EventHandler(this.CycleChanged);
            // 
            // CB_Finisher1
            // 
            this.CB_Finisher1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.CB_Finisher1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CB_Finisher1.FormattingEnabled = true;
            this.CB_Finisher1.Location = new System.Drawing.Point(104, 20);
            this.CB_Finisher1.Name = "CB_Finisher1";
            this.CB_Finisher1.Size = new System.Drawing.Size(188, 21);
            this.CB_Finisher1.TabIndex = 6;
            this.CB_Finisher1.SelectedIndexChanged += new System.EventHandler(this.CycleChanged);
            // 
            // CB_ComboPoints1
            // 
            this.CB_ComboPoints1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CB_ComboPoints1.FormattingEnabled = true;
            this.CB_ComboPoints1.Location = new System.Drawing.Point(11, 20);
            this.CB_ComboPoints1.Name = "CB_ComboPoints1";
            this.CB_ComboPoints1.Size = new System.Drawing.Size(87, 21);
            this.CB_ComboPoints1.TabIndex = 5;
            this.CB_ComboPoints1.SelectedIndexChanged += new System.EventHandler(this.CycleChanged);
            // 
            // GB_Poisons
            // 
            this.GB_Poisons.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.GB_Poisons.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.GB_Poisons.Controls.Add(this.CB_PoisonOH);
            this.GB_Poisons.Controls.Add(this.LB_PoisonOH);
            this.GB_Poisons.Controls.Add(this.CB_PoisonMH);
            this.GB_Poisons.Controls.Add(this.LB_PoisonMH);
            this.GB_Poisons.Location = new System.Drawing.Point(3, 265);
            this.GB_Poisons.Name = "GB_Poisons";
            this.GB_Poisons.Size = new System.Drawing.Size(298, 77);
            this.GB_Poisons.TabIndex = 7;
            this.GB_Poisons.TabStop = false;
            this.GB_Poisons.Text = "Poisons";
            // 
            // CB_PoisonOH
            // 
            this.CB_PoisonOH.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.CB_PoisonOH.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CB_PoisonOH.FormattingEnabled = true;
            this.CB_PoisonOH.Items.AddRange(new object[] {
            "Deadly Poison",
            "Instant Poison"});
            this.CB_PoisonOH.Location = new System.Drawing.Point(74, 46);
            this.CB_PoisonOH.Name = "CB_PoisonOH";
            this.CB_PoisonOH.Size = new System.Drawing.Size(218, 21);
            this.CB_PoisonOH.TabIndex = 12;
            this.CB_PoisonOH.SelectedIndexChanged += new System.EventHandler(this.OnOHPoisonChanged);
            // 
            // LB_PoisonOH
            // 
            this.LB_PoisonOH.AutoSize = true;
            this.LB_PoisonOH.Location = new System.Drawing.Point(15, 49);
            this.LB_PoisonOH.Name = "LB_PoisonOH";
            this.LB_PoisonOH.Size = new System.Drawing.Size(53, 13);
            this.LB_PoisonOH.TabIndex = 2;
            this.LB_PoisonOH.Text = "Off Hand:";
            // 
            // CB_PoisonMH
            // 
            this.CB_PoisonMH.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.CB_PoisonMH.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CB_PoisonMH.FormattingEnabled = true;
            this.CB_PoisonMH.Location = new System.Drawing.Point(74, 19);
            this.CB_PoisonMH.Name = "CB_PoisonMH";
            this.CB_PoisonMH.Size = new System.Drawing.Size(218, 21);
            this.CB_PoisonMH.TabIndex = 11;
            this.CB_PoisonMH.SelectedIndexChanged += new System.EventHandler(this.OnMHPoisonChanged);
            // 
            // LB_PoisonMH
            // 
            this.LB_PoisonMH.AutoSize = true;
            this.LB_PoisonMH.Location = new System.Drawing.Point(6, 22);
            this.LB_PoisonMH.Name = "LB_PoisonMH";
            this.LB_PoisonMH.Size = new System.Drawing.Size(62, 13);
            this.LB_PoisonMH.TabIndex = 0;
            this.LB_PoisonMH.Text = "Main Hand:";
            // 
            // CB_TargArmor
            // 
            this.CB_TargArmor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.CB_TargArmor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CB_TargArmor.FormattingEnabled = true;
            this.CB_TargArmor.Items.AddRange(new object[] {
            "83",
            "82",
            "81",
            "80"});
            this.CB_TargArmor.Location = new System.Drawing.Point(209, 19);
            this.CB_TargArmor.Name = "CB_TargArmor";
            this.CB_TargArmor.Size = new System.Drawing.Size(83, 21);
            this.CB_TargArmor.TabIndex = 1;
            this.CB_TargArmor.SelectedIndexChanged += new System.EventHandler(this.CB_TargArmor_SelectedIndexChanged);
            // 
            // GB_FightInfo
            // 
            this.GB_FightInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.GB_FightInfo.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.GB_FightInfo.Controls.Add(this.CB_TargLevel);
            this.GB_FightInfo.Controls.Add(this.LB_Duration);
            this.GB_FightInfo.Controls.Add(this.NUD_Duration);
            this.GB_FightInfo.Controls.Add(this.LB_TargLevel);
            this.GB_FightInfo.Controls.Add(this.CB_TargArmor);
            this.GB_FightInfo.Controls.Add(this.LB_TargArmor);
            this.GB_FightInfo.Location = new System.Drawing.Point(3, 3);
            this.GB_FightInfo.Name = "GB_FightInfo";
            this.GB_FightInfo.Size = new System.Drawing.Size(298, 73);
            this.GB_FightInfo.TabIndex = 9;
            this.GB_FightInfo.TabStop = false;
            this.GB_FightInfo.Text = "Bosses";
            // 
            // LB_Duration
            // 
            this.LB_Duration.AutoSize = true;
            this.LB_Duration.Location = new System.Drawing.Point(6, 48);
            this.LB_Duration.Name = "LB_Duration";
            this.LB_Duration.Size = new System.Drawing.Size(102, 13);
            this.LB_Duration.TabIndex = 5;
            this.LB_Duration.Text = "Fight Duration (sec):";
            // 
            // NUD_Duration
            // 
            this.NUD_Duration.Location = new System.Drawing.Point(135, 46);
            this.NUD_Duration.Maximum = new decimal(new int[] {
            1200,
            0,
            0,
            0});
            this.NUD_Duration.Name = "NUD_Duration";
            this.NUD_Duration.Size = new System.Drawing.Size(57, 20);
            this.NUD_Duration.TabIndex = 4;
            this.NUD_Duration.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.NUD_Duration.Value = new decimal(new int[] {
            600,
            0,
            0,
            0});
            this.NUD_Duration.ValueChanged += new System.EventHandler(this.NUD_Duration_ValueChanged);
            // 
            // CB_CpGenerator
            // 
            this.CB_CpGenerator.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.CB_CpGenerator.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CB_CpGenerator.FormattingEnabled = true;
            this.CB_CpGenerator.Location = new System.Drawing.Point(10, 32);
            this.CB_CpGenerator.Name = "CB_CpGenerator";
            this.CB_CpGenerator.Size = new System.Drawing.Size(128, 21);
            this.CB_CpGenerator.TabIndex = 2;
            this.CB_CpGenerator.SelectedIndexChanged += new System.EventHandler(this.CB_CpGenerator_SelectedIndexChanged);
            // 
            // NUD_Hat
            // 
            this.NUD_Hat.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.NUD_Hat.DecimalPlaces = 1;
            this.NUD_Hat.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.NUD_Hat.Location = new System.Drawing.Point(144, 33);
            this.NUD_Hat.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.NUD_Hat.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.NUD_Hat.Name = "NUD_Hat";
            this.NUD_Hat.Size = new System.Drawing.Size(73, 20);
            this.NUD_Hat.TabIndex = 3;
            this.NUD_Hat.Value = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.NUD_Hat.Visible = false;
            this.NUD_Hat.ValueChanged += new System.EventHandler(this.HatStepper_ValueChanged);
            // 
            // GB_CPG
            // 
            this.GB_CPG.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.GB_CPG.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.GB_CPG.Controls.Add(this.LB_HemoPerCycle);
            this.GB_CPG.Controls.Add(this.NUD_HemoPerCycle);
            this.GB_CPG.Controls.Add(this.LB_CPperSec);
            this.GB_CPG.Controls.Add(this.LB_CPG);
            this.GB_CPG.Controls.Add(this.NUD_Hat);
            this.GB_CPG.Controls.Add(this.CB_CpGenerator);
            this.GB_CPG.Location = new System.Drawing.Point(3, 82);
            this.GB_CPG.Name = "GB_CPG";
            this.GB_CPG.Size = new System.Drawing.Size(298, 64);
            this.GB_CPG.TabIndex = 11;
            this.GB_CPG.TabStop = false;
            this.GB_CPG.Text = "Combo Point Generator";
            // 
            // LB_HemoPerCycle
            // 
            this.LB_HemoPerCycle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.LB_HemoPerCycle.AutoSize = true;
            this.LB_HemoPerCycle.Location = new System.Drawing.Point(220, 16);
            this.LB_HemoPerCycle.Name = "LB_HemoPerCycle";
            this.LB_HemoPerCycle.Size = new System.Drawing.Size(66, 13);
            this.LB_HemoPerCycle.TabIndex = 14;
            this.LB_HemoPerCycle.Text = "Hemo/Cycle";
            // 
            // NUD_HemoPerCycle
            // 
            this.NUD_HemoPerCycle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.NUD_HemoPerCycle.DecimalPlaces = 1;
            this.NUD_HemoPerCycle.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.NUD_HemoPerCycle.Location = new System.Drawing.Point(223, 33);
            this.NUD_HemoPerCycle.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.NUD_HemoPerCycle.Name = "NUD_HemoPerCycle";
            this.NUD_HemoPerCycle.Size = new System.Drawing.Size(69, 20);
            this.NUD_HemoPerCycle.TabIndex = 4;
            this.NUD_HemoPerCycle.ValueChanged += new System.EventHandler(this.HatStepper_ValueChanged);
            // 
            // LB_CPperSec
            // 
            this.LB_CPperSec.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.LB_CPperSec.AutoSize = true;
            this.LB_CPperSec.Location = new System.Drawing.Point(141, 16);
            this.LB_CPperSec.Name = "LB_CPperSec";
            this.LB_CPperSec.Size = new System.Drawing.Size(48, 13);
            this.LB_CPperSec.TabIndex = 12;
            this.LB_CPperSec.Text = "CPs/sec";
            // 
            // LB_CPG
            // 
            this.LB_CPG.AutoSize = true;
            this.LB_CPG.Location = new System.Drawing.Point(9, 16);
            this.LB_CPG.Name = "LB_CPG";
            this.LB_CPG.Size = new System.Drawing.Size(29, 13);
            this.LB_CPG.TabIndex = 11;
            this.LB_CPG.Text = "CPG";
            // 
            // GB_Abilities
            // 
            this.GB_Abilities.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.GB_Abilities.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.GB_Abilities.Controls.Add(this.CK_UseTurnTheTables);
            this.GB_Abilities.Controls.Add(this.LB_TurnTheTables);
            this.GB_Abilities.Controls.Add(this.NUD_TurnTheTablesUptimePerc);
            this.GB_Abilities.Controls.Add(this.LB_FeintDelay);
            this.GB_Abilities.Controls.Add(this.NUD_FeintDelay);
            this.GB_Abilities.Controls.Add(this.CK_UseFeint);
            this.GB_Abilities.Location = new System.Drawing.Point(3, 348);
            this.GB_Abilities.Name = "GB_Abilities";
            this.GB_Abilities.Size = new System.Drawing.Size(298, 70);
            this.GB_Abilities.TabIndex = 12;
            this.GB_Abilities.TabStop = false;
            this.GB_Abilities.Text = "Abilities";
            // 
            // CK_UseTurnTheTables
            // 
            this.CK_UseTurnTheTables.AutoSize = true;
            this.CK_UseTurnTheTables.Location = new System.Drawing.Point(6, 46);
            this.CK_UseTurnTheTables.Name = "CK_UseTurnTheTables";
            this.CK_UseTurnTheTables.Size = new System.Drawing.Size(101, 17);
            this.CK_UseTurnTheTables.TabIndex = 5;
            this.CK_UseTurnTheTables.Text = "Turn the Tables";
            this.CK_UseTurnTheTables.UseVisualStyleBackColor = true;
            this.CK_UseTurnTheTables.CheckedChanged += new System.EventHandler(this.UseTurnTheTables_CheckedChanged);
            // 
            // LB_TurnTheTables
            // 
            this.LB_TurnTheTables.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.LB_TurnTheTables.AutoSize = true;
            this.LB_TurnTheTables.Location = new System.Drawing.Point(166, 47);
            this.LB_TurnTheTables.Name = "LB_TurnTheTables";
            this.LB_TurnTheTables.Size = new System.Drawing.Size(51, 13);
            this.LB_TurnTheTables.TabIndex = 4;
            this.LB_TurnTheTables.Text = "Uptime %";
            this.LB_TurnTheTables.Visible = false;
            // 
            // NUD_TurnTheTablesUptimePerc
            // 
            this.NUD_TurnTheTablesUptimePerc.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.NUD_TurnTheTablesUptimePerc.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.NUD_TurnTheTablesUptimePerc.Location = new System.Drawing.Point(223, 45);
            this.NUD_TurnTheTablesUptimePerc.Name = "NUD_TurnTheTablesUptimePerc";
            this.NUD_TurnTheTablesUptimePerc.Size = new System.Drawing.Size(69, 20);
            this.NUD_TurnTheTablesUptimePerc.TabIndex = 3;
            this.NUD_TurnTheTablesUptimePerc.Visible = false;
            this.NUD_TurnTheTablesUptimePerc.ValueChanged += new System.EventHandler(this.TurnTheTablesUptimePercent_ValueChanged);
            // 
            // LB_FeintDelay
            // 
            this.LB_FeintDelay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.LB_FeintDelay.AutoSize = true;
            this.LB_FeintDelay.Location = new System.Drawing.Point(123, 21);
            this.LB_FeintDelay.Name = "LB_FeintDelay";
            this.LB_FeintDelay.Size = new System.Drawing.Size(94, 13);
            this.LB_FeintDelay.TabIndex = 2;
            this.LB_FeintDelay.Text = "Delay (in seconds)";
            this.LB_FeintDelay.Visible = false;
            // 
            // NUD_FeintDelay
            // 
            this.NUD_FeintDelay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.NUD_FeintDelay.Location = new System.Drawing.Point(223, 19);
            this.NUD_FeintDelay.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.NUD_FeintDelay.Name = "NUD_FeintDelay";
            this.NUD_FeintDelay.Size = new System.Drawing.Size(69, 20);
            this.NUD_FeintDelay.TabIndex = 1;
            this.NUD_FeintDelay.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.NUD_FeintDelay.Visible = false;
            this.NUD_FeintDelay.ValueChanged += new System.EventHandler(this.FeintDelayStepper_ValueChanged);
            // 
            // CK_UseFeint
            // 
            this.CK_UseFeint.AutoSize = true;
            this.CK_UseFeint.Location = new System.Drawing.Point(6, 20);
            this.CK_UseFeint.Name = "CK_UseFeint";
            this.CK_UseFeint.Size = new System.Drawing.Size(49, 17);
            this.CK_UseFeint.TabIndex = 0;
            this.CK_UseFeint.Text = "Feint";
            this.CK_UseFeint.UseVisualStyleBackColor = true;
            this.CK_UseFeint.CheckedChanged += new System.EventHandler(this.Feint_CheckedChanged);
            // 
            // CalculationOptionsPanelRogue
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.GB_Abilities);
            this.Controls.Add(this.GB_CPG);
            this.Controls.Add(this.GB_Poisons);
            this.Controls.Add(this.GB_Cycles);
            this.Controls.Add(this.GB_FightInfo);
            this.Name = "CalculationOptionsPanelRogue";
            this.Size = new System.Drawing.Size(304, 630);
            this.GB_Cycles.ResumeLayout(false);
            this.GB_Poisons.ResumeLayout(false);
            this.GB_Poisons.PerformLayout();
            this.GB_FightInfo.ResumeLayout(false);
            this.GB_FightInfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_Duration)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_Hat)).EndInit();
            this.GB_CPG.ResumeLayout(false);
            this.GB_CPG.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_HemoPerCycle)).EndInit();
            this.GB_Abilities.ResumeLayout(false);
            this.GB_Abilities.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_TurnTheTablesUptimePerc)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_FeintDelay)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox CB_TargLevel;
		private System.Windows.Forms.Label LB_TargLevel;
        private System.Windows.Forms.Label LB_TargArmor;
        private System.Windows.Forms.GroupBox GB_Cycles;
        private System.Windows.Forms.GroupBox GB_Poisons;
        private System.Windows.Forms.Label LB_PoisonMH;
        private System.Windows.Forms.ComboBox CB_PoisonOH;
        private System.Windows.Forms.Label LB_PoisonOH;
        private System.Windows.Forms.ComboBox CB_PoisonMH;
        private System.Windows.Forms.ComboBox CB_TargArmor;
        private System.Windows.Forms.GroupBox GB_FightInfo;
        private System.Windows.Forms.ComboBox CB_Finisher3;
        private System.Windows.Forms.ComboBox CB_ComboPoints3;
        private System.Windows.Forms.ComboBox CB_Finisher2;
        private System.Windows.Forms.ComboBox CB_ComboPoints2;
        private System.Windows.Forms.ComboBox CB_Finisher1;
        private System.Windows.Forms.ComboBox CB_ComboPoints1;
        private System.Windows.Forms.ComboBox CB_CpGenerator;
        private System.Windows.Forms.NumericUpDown NUD_Hat;
        private System.Windows.Forms.GroupBox GB_CPG;
        private System.Windows.Forms.Label LB_CPperSec;
        private System.Windows.Forms.Label LB_CPG;
        private System.Windows.Forms.Label LB_HemoPerCycle;
        private System.Windows.Forms.NumericUpDown NUD_HemoPerCycle;
        private System.Windows.Forms.GroupBox GB_Abilities;
        private System.Windows.Forms.Label LB_FeintDelay;
        private System.Windows.Forms.NumericUpDown NUD_FeintDelay;
        private System.Windows.Forms.CheckBox CK_UseFeint;
        private System.Windows.Forms.CheckBox CK_UseTurnTheTables;
        private System.Windows.Forms.Label LB_TurnTheTables;
        private System.Windows.Forms.NumericUpDown NUD_TurnTheTablesUptimePerc;
        private System.Windows.Forms.Label LB_Duration;
        private System.Windows.Forms.NumericUpDown NUD_Duration;
    }
}
