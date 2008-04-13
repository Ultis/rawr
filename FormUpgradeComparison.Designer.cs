namespace Rawr
{
    partial class FormUpgradeComparison
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormUpgradeComparison));
            this.toolStripItemComparison = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripDropDownButtonSlot = new System.Windows.Forms.ToolStripDropDownButton();
            this.allToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.headToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.neckToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.shouldersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.backToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.chestToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.wristsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.handsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.waistToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.legsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.feetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.finger1ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.trinket1ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.weaponToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.offHandToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.idolToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripDropDownButtonSort = new System.Windows.Forms.ToolStripDropDownButton();
            this.overallToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.alphabeticalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.vScrollBarGraph = new System.Windows.Forms.VScrollBar();
            this.comparisonGraph1 = new Rawr.ComparisonGraph();
            this.toolStripItemComparison.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripItemComparison
            // 
            this.toolStripItemComparison.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.toolStripDropDownButtonSlot,
            this.toolStripLabel2,
            this.toolStripDropDownButtonSort});
            this.toolStripItemComparison.Location = new System.Drawing.Point(0, 0);
            this.toolStripItemComparison.Name = "toolStripItemComparison";
            this.toolStripItemComparison.Size = new System.Drawing.Size(458, 25);
            this.toolStripItemComparison.TabIndex = 5;
            this.toolStripItemComparison.Text = "toolStripItemComparison";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(30, 22);
            this.toolStripLabel1.Text = "Slot:";
            // 
            // toolStripDropDownButtonSlot
            // 
            this.toolStripDropDownButtonSlot.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripDropDownButtonSlot.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.allToolStripMenuItem,
            this.headToolStripMenuItem,
            this.neckToolStripMenuItem,
            this.shouldersToolStripMenuItem,
            this.backToolStripMenuItem,
            this.chestToolStripMenuItem,
            this.wristsToolStripMenuItem,
            this.handsToolStripMenuItem,
            this.waistToolStripMenuItem,
            this.legsToolStripMenuItem,
            this.feetToolStripMenuItem,
            this.finger1ToolStripMenuItem,
            this.trinket1ToolStripMenuItem,
            this.weaponToolStripMenuItem,
            this.offHandToolStripMenuItem,
            this.idolToolStripMenuItem,
            this.toolStripMenuItem2,
            this.toolStripMenuItem3});
            this.toolStripDropDownButtonSlot.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButtonSlot.Image")));
            this.toolStripDropDownButtonSlot.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButtonSlot.Name = "toolStripDropDownButtonSlot";
            this.toolStripDropDownButtonSlot.Size = new System.Drawing.Size(34, 22);
            this.toolStripDropDownButtonSlot.Text = "All";
            this.toolStripDropDownButtonSlot.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // allToolStripMenuItem
            // 
            this.allToolStripMenuItem.Name = "allToolStripMenuItem";
            this.allToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.allToolStripMenuItem.Tag = "Gear.All";
            this.allToolStripMenuItem.Text = " All";
            this.allToolStripMenuItem.Click += new System.EventHandler(this.slotToolStripMenuItem_Click);
            // 
            // headToolStripMenuItem
            // 
            this.headToolStripMenuItem.Name = "headToolStripMenuItem";
            this.headToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.headToolStripMenuItem.Tag = "Gear.Head";
            this.headToolStripMenuItem.Text = " Head";
            this.headToolStripMenuItem.Click += new System.EventHandler(this.slotToolStripMenuItem_Click);
            // 
            // neckToolStripMenuItem
            // 
            this.neckToolStripMenuItem.Name = "neckToolStripMenuItem";
            this.neckToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.neckToolStripMenuItem.Tag = "Gear.Neck";
            this.neckToolStripMenuItem.Text = " Neck";
            this.neckToolStripMenuItem.Click += new System.EventHandler(this.slotToolStripMenuItem_Click);
            // 
            // shouldersToolStripMenuItem
            // 
            this.shouldersToolStripMenuItem.Name = "shouldersToolStripMenuItem";
            this.shouldersToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.shouldersToolStripMenuItem.Tag = "Gear.Shoulders";
            this.shouldersToolStripMenuItem.Text = " Shoulders";
            this.shouldersToolStripMenuItem.Click += new System.EventHandler(this.slotToolStripMenuItem_Click);
            // 
            // backToolStripMenuItem
            // 
            this.backToolStripMenuItem.Name = "backToolStripMenuItem";
            this.backToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.backToolStripMenuItem.Tag = "Gear.Back";
            this.backToolStripMenuItem.Text = " Back";
            this.backToolStripMenuItem.Click += new System.EventHandler(this.slotToolStripMenuItem_Click);
            // 
            // chestToolStripMenuItem
            // 
            this.chestToolStripMenuItem.Name = "chestToolStripMenuItem";
            this.chestToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.chestToolStripMenuItem.Tag = "Gear.Chest";
            this.chestToolStripMenuItem.Text = " Chest";
            this.chestToolStripMenuItem.Click += new System.EventHandler(this.slotToolStripMenuItem_Click);
            // 
            // wristsToolStripMenuItem
            // 
            this.wristsToolStripMenuItem.Name = "wristsToolStripMenuItem";
            this.wristsToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.wristsToolStripMenuItem.Tag = "Gear.Wrist";
            this.wristsToolStripMenuItem.Text = " Wrists";
            this.wristsToolStripMenuItem.Click += new System.EventHandler(this.slotToolStripMenuItem_Click);
            // 
            // handsToolStripMenuItem
            // 
            this.handsToolStripMenuItem.Name = "handsToolStripMenuItem";
            this.handsToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.handsToolStripMenuItem.Tag = "Gear.Hands";
            this.handsToolStripMenuItem.Text = " Hands";
            this.handsToolStripMenuItem.Click += new System.EventHandler(this.slotToolStripMenuItem_Click);
            // 
            // waistToolStripMenuItem
            // 
            this.waistToolStripMenuItem.Name = "waistToolStripMenuItem";
            this.waistToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.waistToolStripMenuItem.Tag = "Gear.Waist";
            this.waistToolStripMenuItem.Text = " Waist";
            this.waistToolStripMenuItem.Click += new System.EventHandler(this.slotToolStripMenuItem_Click);
            // 
            // legsToolStripMenuItem
            // 
            this.legsToolStripMenuItem.Name = "legsToolStripMenuItem";
            this.legsToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.legsToolStripMenuItem.Tag = "Gear.Legs";
            this.legsToolStripMenuItem.Text = " Legs";
            this.legsToolStripMenuItem.Click += new System.EventHandler(this.slotToolStripMenuItem_Click);
            // 
            // feetToolStripMenuItem
            // 
            this.feetToolStripMenuItem.Name = "feetToolStripMenuItem";
            this.feetToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.feetToolStripMenuItem.Tag = "Gear.Feet";
            this.feetToolStripMenuItem.Text = " Feet";
            this.feetToolStripMenuItem.Click += new System.EventHandler(this.slotToolStripMenuItem_Click);
            // 
            // finger1ToolStripMenuItem
            // 
            this.finger1ToolStripMenuItem.Name = "finger1ToolStripMenuItem";
            this.finger1ToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.finger1ToolStripMenuItem.Tag = "Gear.Finger1";
            this.finger1ToolStripMenuItem.Text = " Finger";
            this.finger1ToolStripMenuItem.Click += new System.EventHandler(this.slotToolStripMenuItem_Click);
            // 
            // trinket1ToolStripMenuItem
            // 
            this.trinket1ToolStripMenuItem.Name = "trinket1ToolStripMenuItem";
            this.trinket1ToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.trinket1ToolStripMenuItem.Tag = "Gear.Trinket1";
            this.trinket1ToolStripMenuItem.Text = " Trinket";
            this.trinket1ToolStripMenuItem.Click += new System.EventHandler(this.slotToolStripMenuItem_Click);
            // 
            // weaponToolStripMenuItem
            // 
            this.weaponToolStripMenuItem.Name = "weaponToolStripMenuItem";
            this.weaponToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.weaponToolStripMenuItem.Tag = "Gear.MainHand";
            this.weaponToolStripMenuItem.Text = " Main Hand";
            this.weaponToolStripMenuItem.Click += new System.EventHandler(this.slotToolStripMenuItem_Click);
            // 
            // offHandToolStripMenuItem
            // 
            this.offHandToolStripMenuItem.Name = "offHandToolStripMenuItem";
            this.offHandToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.offHandToolStripMenuItem.Tag = "Gear.OffHand";
            this.offHandToolStripMenuItem.Text = " Off Hand";
            this.offHandToolStripMenuItem.Click += new System.EventHandler(this.slotToolStripMenuItem_Click);
            // 
            // idolToolStripMenuItem
            // 
            this.idolToolStripMenuItem.Name = "idolToolStripMenuItem";
            this.idolToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.idolToolStripMenuItem.Tag = "Gear.Ranged";
            this.idolToolStripMenuItem.Text = " Ranged";
            this.idolToolStripMenuItem.Click += new System.EventHandler(this.slotToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(149, 22);
            this.toolStripMenuItem2.Tag = "Gear.Projectile";
            this.toolStripMenuItem2.Text = " Projectile";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.slotToolStripMenuItem_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(149, 22);
            this.toolStripMenuItem3.Tag = "Gear.ProjectileBag";
            this.toolStripMenuItem3.Text = " Projectile Bag";
            this.toolStripMenuItem3.Click += new System.EventHandler(this.slotToolStripMenuItem_Click);
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(61, 22);
            this.toolStripLabel2.Text = "          Sort:";
            // 
            // toolStripDropDownButtonSort
            // 
            this.toolStripDropDownButtonSort.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripDropDownButtonSort.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.overallToolStripMenuItem,
            this.alphabeticalToolStripMenuItem});
            this.toolStripDropDownButtonSort.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButtonSort.Image")));
            this.toolStripDropDownButtonSort.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButtonSort.Name = "toolStripDropDownButtonSort";
            this.toolStripDropDownButtonSort.Size = new System.Drawing.Size(57, 22);
            this.toolStripDropDownButtonSort.Text = "Overall";
            // 
            // overallToolStripMenuItem
            // 
            this.overallToolStripMenuItem.Name = "overallToolStripMenuItem";
            this.overallToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
            this.overallToolStripMenuItem.Tag = -1;
            this.overallToolStripMenuItem.Text = "Overall";
            this.overallToolStripMenuItem.Click += new System.EventHandler(this.sortToolStripMenuItem_Click);
            // 
            // alphabeticalToolStripMenuItem
            // 
            this.alphabeticalToolStripMenuItem.Name = "alphabeticalToolStripMenuItem";
            this.alphabeticalToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
            this.alphabeticalToolStripMenuItem.Tag = -2;
            this.alphabeticalToolStripMenuItem.Text = "Alphabetical";
            this.alphabeticalToolStripMenuItem.Click += new System.EventHandler(this.sortToolStripMenuItem_Click);
            // 
            // vScrollBarGraph
            // 
            this.vScrollBarGraph.Dock = System.Windows.Forms.DockStyle.Right;
            this.vScrollBarGraph.LargeChange = 41;
            this.vScrollBarGraph.Location = new System.Drawing.Point(441, 25);
            this.vScrollBarGraph.Maximum = 40;
            this.vScrollBarGraph.Name = "vScrollBarGraph";
            this.vScrollBarGraph.Size = new System.Drawing.Size(17, 593);
            this.vScrollBarGraph.SmallChange = 32;
            this.vScrollBarGraph.TabIndex = 7;
            // 
            // comparisonGraph1
            // 
            this.comparisonGraph1.BackColor = System.Drawing.Color.White;
            this.comparisonGraph1.Character = null;
            this.comparisonGraph1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comparisonGraph1.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comparisonGraph1.ItemCalculations = new Rawr.ComparisonCalculationBase[0];
            this.comparisonGraph1.Location = new System.Drawing.Point(0, 25);
            this.comparisonGraph1.Margin = new System.Windows.Forms.Padding(4);
            this.comparisonGraph1.Name = "comparisonGraph1";
            this.comparisonGraph1.RoundValues = true;
            this.comparisonGraph1.ScrollBar = this.vScrollBarGraph;
            this.comparisonGraph1.Size = new System.Drawing.Size(458, 593);
            this.comparisonGraph1.Sort = Rawr.ComparisonGraph.ComparisonSort.Overall;
            this.comparisonGraph1.TabIndex = 6;
            // 
            // FormUpgradeComparison
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(458, 618);
            this.Controls.Add(this.vScrollBarGraph);
            this.Controls.Add(this.comparisonGraph1);
            this.Controls.Add(this.toolStripItemComparison);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "FormUpgradeComparison";
            this.Text = "Upgrade Comparison";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormUpgradeComparison_FormClosing);
            this.toolStripItemComparison.ResumeLayout(false);
            this.toolStripItemComparison.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStripItemComparison;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButtonSlot;
        private System.Windows.Forms.ToolStripMenuItem allToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem headToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem neckToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem shouldersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem backToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem chestToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem wristsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem handsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem waistToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem legsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem feetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem finger1ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem trinket1ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem weaponToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem offHandToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem idolToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButtonSort;
        private System.Windows.Forms.ToolStripMenuItem overallToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem alphabeticalToolStripMenuItem;
        private System.Windows.Forms.VScrollBar vScrollBarGraph;
        private ComparisonGraph comparisonGraph1;
    }
}