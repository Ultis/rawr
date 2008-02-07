namespace Rawr
{
    partial class FormItemEditor
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
            System.Windows.Forms.ListViewGroup listViewGroup1 = new System.Windows.Forms.ListViewGroup("Head", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup2 = new System.Windows.Forms.ListViewGroup("Neck", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup3 = new System.Windows.Forms.ListViewGroup("Shoulders", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup4 = new System.Windows.Forms.ListViewGroup("Back", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup5 = new System.Windows.Forms.ListViewGroup("Chest", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup6 = new System.Windows.Forms.ListViewGroup("Shirt", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup7 = new System.Windows.Forms.ListViewGroup("Tabard", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup8 = new System.Windows.Forms.ListViewGroup("Wrist", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup9 = new System.Windows.Forms.ListViewGroup("Hands", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup10 = new System.Windows.Forms.ListViewGroup("Waist", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup11 = new System.Windows.Forms.ListViewGroup("Legs", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup12 = new System.Windows.Forms.ListViewGroup("Feet", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup13 = new System.Windows.Forms.ListViewGroup("Finger", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup14 = new System.Windows.Forms.ListViewGroup("Trinket", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup15 = new System.Windows.Forms.ListViewGroup("Weapon", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup16 = new System.Windows.Forms.ListViewGroup("Robe", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup17 = new System.Windows.Forms.ListViewGroup("One Hand", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup18 = new System.Windows.Forms.ListViewGroup("Wand", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup19 = new System.Windows.Forms.ListViewGroup("Idol", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup20 = new System.Windows.Forms.ListViewGroup("Gems", System.Windows.Forms.HorizontalAlignment.Left);
            this.label1 = new System.Windows.Forms.Label();
            this.numericUpDownId = new System.Windows.Forms.NumericUpDown();
            this.textBoxName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxIcon = new System.Windows.Forms.TextBox();
            this.comboBoxSlot = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.numericUpDownBonus1 = new System.Windows.Forms.NumericUpDown();
            this.comboBoxBonus1 = new System.Windows.Forms.ComboBox();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.comboBoxSocket3 = new System.Windows.Forms.ComboBox();
            this.comboBoxSocket2 = new System.Windows.Forms.ComboBox();
            this.comboBoxSocket1 = new System.Windows.Forms.ComboBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.itemButtonGem1 = new Rawr.ItemButton();
            this.itemButtonGem2 = new Rawr.ItemButton();
            this.itemButtonGem3 = new Rawr.ItemButton();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.buttonDelete = new System.Windows.Forms.Button();
            this.buttonFillSockets = new System.Windows.Forms.Button();
            this.listViewItems = new System.Windows.Forms.ListView();
            this.columnHeaderName = new System.Windows.Forms.ColumnHeader();
            this.imageListItems = new System.Windows.Forms.ImageList(this.components);
            this.buttonDuplicate = new System.Windows.Forms.Button();
            this.textBoxFilter = new System.Windows.Forms.TextBox();
            this.label22 = new System.Windows.Forms.Label();
            this.propertyGridStats = new System.Windows.Forms.PropertyGrid();
            ((System.ComponentModel.ISupportInitialize) (this.numericUpDownId)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.groupBox6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.numericUpDownBonus1)).BeginInit();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(251, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Name:";
            // 
            // numericUpDownId
            // 
            this.numericUpDownId.Location = new System.Drawing.Point(551, 12);
            this.numericUpDownId.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.numericUpDownId.Minimum = new decimal(new int[] {
            100000,
            0,
            0,
            -2147483648});
            this.numericUpDownId.Name = "numericUpDownId";
            this.numericUpDownId.Size = new System.Drawing.Size(109, 20);
            this.numericUpDownId.TabIndex = 2;
            // 
            // textBoxName
            // 
            this.textBoxName.Location = new System.Drawing.Point(295, 12);
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.Size = new System.Drawing.Size(216, 20);
            this.textBoxName.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(517, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(21, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "ID:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(251, 41);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(31, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Icon:";
            // 
            // textBoxIcon
            // 
            this.textBoxIcon.Location = new System.Drawing.Point(295, 38);
            this.textBoxIcon.Name = "textBoxIcon";
            this.textBoxIcon.Size = new System.Drawing.Size(216, 20);
            this.textBoxIcon.TabIndex = 3;
            // 
            // comboBoxSlot
            // 
            this.comboBoxSlot.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSlot.FormattingEnabled = true;
            this.comboBoxSlot.Items.AddRange(new object[] {
            "Head",
            "Neck",
            "Shoulders",
            "Back",
            "Chest",
            "Shirt",
            "Tabard",
            "Wrist",
            "Hands",
            "Waist",
            "Legs",
            "Feet",
            "Finger",
            "Trinket",
            "Weapon",
            "Robe",
            "OneHand",
            "Wand",
            "Idol",
            "Red",
            "Orange",
            "Yellow",
            "Green",
            "Purple",
            "Blue",
            "Meta",
            "",
            "Prismatic"});
            this.comboBoxSlot.Location = new System.Drawing.Point(551, 38);
            this.comboBoxSlot.Name = "comboBoxSlot";
            this.comboBoxSlot.Size = new System.Drawing.Size(109, 21);
            this.comboBoxSlot.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(517, 41);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(28, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Slot:";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.groupBox6);
            this.groupBox3.Controls.Add(this.label16);
            this.groupBox3.Controls.Add(this.label15);
            this.groupBox3.Controls.Add(this.label14);
            this.groupBox3.Controls.Add(this.comboBoxSocket3);
            this.groupBox3.Controls.Add(this.comboBoxSocket2);
            this.groupBox3.Controls.Add(this.comboBoxSocket1);
            this.groupBox3.Location = new System.Drawing.Point(251, 64);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(226, 160);
            this.groupBox3.TabIndex = 7;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Sockets";
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.numericUpDownBonus1);
            this.groupBox6.Controls.Add(this.comboBoxBonus1);
            this.groupBox6.Location = new System.Drawing.Point(9, 100);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(210, 50);
            this.groupBox6.TabIndex = 3;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Socket Bonus";
            // 
            // numericUpDownBonus1
            // 
            this.numericUpDownBonus1.Enabled = false;
            this.numericUpDownBonus1.Location = new System.Drawing.Point(151, 19);
            this.numericUpDownBonus1.Name = "numericUpDownBonus1";
            this.numericUpDownBonus1.Size = new System.Drawing.Size(51, 20);
            this.numericUpDownBonus1.TabIndex = 1;
            // 
            // comboBoxBonus1
            // 
            this.comboBoxBonus1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxBonus1.FormattingEnabled = true;
            this.comboBoxBonus1.Location = new System.Drawing.Point(6, 19);
            this.comboBoxBonus1.Name = "comboBoxBonus1";
            this.comboBoxBonus1.Size = new System.Drawing.Size(139, 21);
            this.comboBoxBonus1.TabIndex = 0;
            this.comboBoxBonus1.SelectedIndexChanged += new System.EventHandler(this.comboBoxBonus_SelectedIndexChanged);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(63, 76);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(60, 13);
            this.label16.TabIndex = 1;
            this.label16.Text = "Socket #3:";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(63, 49);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(60, 13);
            this.label15.TabIndex = 1;
            this.label15.Text = "Socket #2:";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(63, 22);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(60, 13);
            this.label14.TabIndex = 1;
            this.label14.Text = "Socket #1:";
            // 
            // comboBoxSocket3
            // 
            this.comboBoxSocket3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSocket3.FormattingEnabled = true;
            this.comboBoxSocket3.Items.AddRange(new object[] {
            "None",
            "Red",
            "Blue",
            "Yellow",
            "Meta"});
            this.comboBoxSocket3.Location = new System.Drawing.Point(129, 73);
            this.comboBoxSocket3.Name = "comboBoxSocket3";
            this.comboBoxSocket3.Size = new System.Drawing.Size(82, 21);
            this.comboBoxSocket3.TabIndex = 2;
            this.comboBoxSocket3.TextChanged += new System.EventHandler(this.comboBoxSocket_TextChanged);
            // 
            // comboBoxSocket2
            // 
            this.comboBoxSocket2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSocket2.FormattingEnabled = true;
            this.comboBoxSocket2.Items.AddRange(new object[] {
            "None",
            "Red",
            "Blue",
            "Yellow",
            "Meta"});
            this.comboBoxSocket2.Location = new System.Drawing.Point(129, 46);
            this.comboBoxSocket2.Name = "comboBoxSocket2";
            this.comboBoxSocket2.Size = new System.Drawing.Size(82, 21);
            this.comboBoxSocket2.TabIndex = 1;
            this.comboBoxSocket2.TextChanged += new System.EventHandler(this.comboBoxSocket_TextChanged);
            // 
            // comboBoxSocket1
            // 
            this.comboBoxSocket1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSocket1.FormattingEnabled = true;
            this.comboBoxSocket1.Items.AddRange(new object[] {
            "None",
            "Red",
            "Blue",
            "Yellow",
            "Meta"});
            this.comboBoxSocket1.Location = new System.Drawing.Point(129, 19);
            this.comboBoxSocket1.Name = "comboBoxSocket1";
            this.comboBoxSocket1.Size = new System.Drawing.Size(82, 21);
            this.comboBoxSocket1.TabIndex = 0;
            this.comboBoxSocket1.TextChanged += new System.EventHandler(this.comboBoxSocket_TextChanged);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.label10);
            this.groupBox5.Controls.Add(this.label9);
            this.groupBox5.Controls.Add(this.label6);
            this.groupBox5.Controls.Add(this.itemButtonGem1);
            this.groupBox5.Controls.Add(this.itemButtonGem2);
            this.groupBox5.Controls.Add(this.itemButtonGem3);
            this.groupBox5.Location = new System.Drawing.Point(251, 230);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(409, 95);
            this.groupBox5.TabIndex = 8;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Gems";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(266, 19);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(48, 13);
            this.label10.TabIndex = 1;
            this.label10.Text = "Gem #3:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(136, 19);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(48, 13);
            this.label9.TabIndex = 1;
            this.label9.Text = "Gem #2:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 19);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(48, 13);
            this.label6.TabIndex = 1;
            this.label6.Text = "Gem #1:";
            // 
            // itemButtonGem1
            // 
            this.itemButtonGem1.Character = null;
            this.itemButtonGem1.CharacterSlot = Rawr.Character.CharacterSlot.Gems;
            this.itemButtonGem1.Location = new System.Drawing.Point(60, 15);
            this.itemButtonGem1.Name = "itemButtonGem1";
            this.itemButtonGem1.SelectedItem = null;
            this.itemButtonGem1.SelectedItemId = 0;
            this.itemButtonGem1.Size = new System.Drawing.Size(70, 70);
            this.itemButtonGem1.TabIndex = 0;
            this.itemButtonGem1.Text = "Gem #1";
            this.itemButtonGem1.UseVisualStyleBackColor = true;
            // 
            // itemButtonGem2
            // 
            this.itemButtonGem2.Character = null;
            this.itemButtonGem2.CharacterSlot = Rawr.Character.CharacterSlot.Gems;
            this.itemButtonGem2.Location = new System.Drawing.Point(190, 15);
            this.itemButtonGem2.Name = "itemButtonGem2";
            this.itemButtonGem2.SelectedItem = null;
            this.itemButtonGem2.SelectedItemId = 0;
            this.itemButtonGem2.Size = new System.Drawing.Size(70, 70);
            this.itemButtonGem2.TabIndex = 1;
            this.itemButtonGem2.Text = "Gem #2";
            this.itemButtonGem2.UseVisualStyleBackColor = true;
            // 
            // itemButtonGem3
            // 
            this.itemButtonGem3.Character = null;
            this.itemButtonGem3.CharacterSlot = Rawr.Character.CharacterSlot.Gems;
            this.itemButtonGem3.Location = new System.Drawing.Point(320, 15);
            this.itemButtonGem3.Name = "itemButtonGem3";
            this.itemButtonGem3.SelectedItem = null;
            this.itemButtonGem3.SelectedItemId = 0;
            this.itemButtonGem3.Size = new System.Drawing.Size(70, 70);
            this.itemButtonGem3.TabIndex = 2;
            this.itemButtonGem3.Text = "Gem #3";
            this.itemButtonGem3.UseVisualStyleBackColor = true;
            // 
            // buttonOK
            // 
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(584, 331);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 13;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            // 
            // buttonAdd
            // 
            this.buttonAdd.Location = new System.Drawing.Point(11, 331);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(75, 23);
            this.buttonAdd.TabIndex = 9;
            this.buttonAdd.Text = "Add...";
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // buttonDelete
            // 
            this.buttonDelete.Location = new System.Drawing.Point(92, 331);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(75, 23);
            this.buttonDelete.TabIndex = 10;
            this.buttonDelete.Text = "Delete";
            this.buttonDelete.UseVisualStyleBackColor = true;
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // buttonFillSockets
            // 
            this.buttonFillSockets.AutoSize = true;
            this.buttonFillSockets.Location = new System.Drawing.Point(254, 331);
            this.buttonFillSockets.Name = "buttonFillSockets";
            this.buttonFillSockets.Size = new System.Drawing.Size(80, 23);
            this.buttonFillSockets.TabIndex = 12;
            this.buttonFillSockets.Text = "Fill Sockets...";
            this.buttonFillSockets.UseVisualStyleBackColor = true;
            this.buttonFillSockets.Click += new System.EventHandler(this.buttonFillSockets_Click);
            // 
            // listViewItems
            // 
            this.listViewItems.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderName});
            listViewGroup1.Header = "Head";
            listViewGroup1.Name = "listViewGroupHead";
            listViewGroup2.Header = "Neck";
            listViewGroup2.Name = "listViewGroupNeck";
            listViewGroup3.Header = "Shoulders";
            listViewGroup3.Name = "listViewGroupShoulders";
            listViewGroup4.Header = "Back";
            listViewGroup4.Name = "listViewGroupBack";
            listViewGroup5.Header = "Chest";
            listViewGroup5.Name = "listViewGroupChest";
            listViewGroup6.Header = "Shirt";
            listViewGroup6.Name = "listViewGroupShirt";
            listViewGroup7.Header = "Tabard";
            listViewGroup7.Name = "listViewGroupTabard";
            listViewGroup8.Header = "Wrist";
            listViewGroup8.Name = "listViewGroupWrist";
            listViewGroup9.Header = "Hands";
            listViewGroup9.Name = "listViewGroupHands";
            listViewGroup10.Header = "Waist";
            listViewGroup10.Name = "listViewGroupWaist";
            listViewGroup11.Header = "Legs";
            listViewGroup11.Name = "listViewGroupLegs";
            listViewGroup12.Header = "Feet";
            listViewGroup12.Name = "listViewGroupFeet";
            listViewGroup13.Header = "Finger";
            listViewGroup13.Name = "listViewGroupFinger";
            listViewGroup14.Header = "Trinket";
            listViewGroup14.Name = "listViewGroupTrinket";
            listViewGroup15.Header = "Weapon";
            listViewGroup15.Name = "listViewGroupWeapon";
            listViewGroup16.Header = "Robe";
            listViewGroup16.Name = "listViewGroupRobe";
            listViewGroup17.Header = "One Hand";
            listViewGroup17.Name = "listViewGroupOneHand";
            listViewGroup18.Header = "Wand";
            listViewGroup18.Name = "listViewGroupWand";
            listViewGroup19.Header = "Idol";
            listViewGroup19.Name = "listViewGroupIdol";
            listViewGroup20.Header = "Gems";
            listViewGroup20.Name = "listViewGroupGems";
            this.listViewItems.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup1,
            listViewGroup2,
            listViewGroup3,
            listViewGroup4,
            listViewGroup5,
            listViewGroup6,
            listViewGroup7,
            listViewGroup8,
            listViewGroup9,
            listViewGroup10,
            listViewGroup11,
            listViewGroup12,
            listViewGroup13,
            listViewGroup14,
            listViewGroup15,
            listViewGroup16,
            listViewGroup17,
            listViewGroup18,
            listViewGroup19,
            listViewGroup20});
            this.listViewItems.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.listViewItems.Location = new System.Drawing.Point(12, 38);
            this.listViewItems.MultiSelect = false;
            this.listViewItems.Name = "listViewItems";
            this.listViewItems.Size = new System.Drawing.Size(233, 287);
            this.listViewItems.SmallImageList = this.imageListItems;
            this.listViewItems.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.listViewItems.TabIndex = 0;
            this.listViewItems.UseCompatibleStateImageBehavior = false;
            this.listViewItems.View = System.Windows.Forms.View.Details;
            this.listViewItems.SelectedIndexChanged += new System.EventHandler(this.listViewItems_SelectedIndexChanged);
            // 
            // columnHeaderName
            // 
            this.columnHeaderName.Width = 229;
            // 
            // imageListItems
            // 
            this.imageListItems.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.imageListItems.ImageSize = new System.Drawing.Size(32, 32);
            this.imageListItems.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // buttonDuplicate
            // 
            this.buttonDuplicate.Location = new System.Drawing.Point(173, 331);
            this.buttonDuplicate.Name = "buttonDuplicate";
            this.buttonDuplicate.Size = new System.Drawing.Size(75, 23);
            this.buttonDuplicate.TabIndex = 11;
            this.buttonDuplicate.Text = "Duplicate";
            this.buttonDuplicate.UseVisualStyleBackColor = true;
            this.buttonDuplicate.Click += new System.EventHandler(this.buttonDuplicate_Click);
            // 
            // textBoxFilter
            // 
            this.textBoxFilter.Location = new System.Drawing.Point(50, 12);
            this.textBoxFilter.Name = "textBoxFilter";
            this.textBoxFilter.Size = new System.Drawing.Size(195, 20);
            this.textBoxFilter.TabIndex = 15;
            this.textBoxFilter.TextChanged += new System.EventHandler(this.textBoxFilter_TextChanged);
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(12, 15);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(32, 13);
            this.label22.TabIndex = 14;
            this.label22.Text = "Filter:";
            // 
            // propertyGridStats
            // 
            this.propertyGridStats.HelpVisible = false;
            this.propertyGridStats.Location = new System.Drawing.Point(484, 64);
            this.propertyGridStats.Name = "propertyGridStats";
            this.propertyGridStats.Size = new System.Drawing.Size(176, 160);
            this.propertyGridStats.TabIndex = 16;
            this.propertyGridStats.ToolbarVisible = false;
            // 
            // FormItemEditor
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(672, 365);
            this.ControlBox = false;
            this.Controls.Add(this.propertyGridStats);
            this.Controls.Add(this.textBoxFilter);
            this.Controls.Add(this.label22);
            this.Controls.Add(this.listViewItems);
            this.Controls.Add(this.buttonDuplicate);
            this.Controls.Add(this.buttonFillSockets);
            this.Controls.Add(this.buttonDelete);
            this.Controls.Add(this.buttonAdd);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.comboBoxSlot);
            this.Controls.Add(this.textBoxIcon);
            this.Controls.Add(this.textBoxName);
            this.Controls.Add(this.numericUpDownId);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "FormItemEditor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Item Editor...";
            this.Load += new System.EventHandler(this.FormItemEditor_Load);
            ((System.ComponentModel.ISupportInitialize) (this.numericUpDownId)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize) (this.numericUpDownBonus1)).EndInit();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

		private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numericUpDownId;
        private System.Windows.Forms.TextBox textBoxName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxIcon;
        private System.Windows.Forms.ComboBox comboBoxSlot;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button buttonOK;
        private ItemButton itemButtonGem3;
		private ItemButton itemButtonGem2;
		private ItemButton itemButtonGem1;
        private System.Windows.Forms.Button buttonAdd;
        private System.Windows.Forms.Button buttonDelete;
        private System.Windows.Forms.ComboBox comboBoxSocket3;
        private System.Windows.Forms.ComboBox comboBoxSocket2;
        private System.Windows.Forms.ComboBox comboBoxSocket1;
        private System.Windows.Forms.GroupBox groupBox6;
		private System.Windows.Forms.Button buttonFillSockets;
		private System.Windows.Forms.ListView listViewItems;
		private System.Windows.Forms.ColumnHeader columnHeaderName;
		private System.Windows.Forms.ImageList imageListItems;
		private System.Windows.Forms.Button buttonDuplicate;
		private System.Windows.Forms.TextBox textBoxFilter;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.ComboBox comboBoxBonus1;
        private System.Windows.Forms.NumericUpDown numericUpDownBonus1;
        private System.Windows.Forms.PropertyGrid propertyGridStats;
    }
}