using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Rawr
{
    public partial class FormUpgradeComparison : Form
    {
        private FormUpgradeComparison()
        {
            InitializeComponent();
        }

        private static FormUpgradeComparison _instance;
        public static FormUpgradeComparison Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new FormUpgradeComparison();
                }
                return _instance;
            }
        }

        Dictionary<string, ComparisonCalculationBase[]> itemCalculations;

        public void LoadData(Character character, Dictionary<Character.CharacterSlot, List<ComparisonCalculationBase>> calculations)
        {
            comparisonGraph1.Character = character;
            toolStripDropDownButtonSort.DropDownItems.Clear();
            toolStripDropDownButtonSort.DropDownItems.Add(overallToolStripMenuItem);
            toolStripDropDownButtonSort.DropDownItems.Add(alphabeticalToolStripMenuItem);
            foreach (string name in Calculations.SubPointNameColors.Keys)
            {
                ToolStripMenuItem toolStripMenuItemSubPoint = new ToolStripMenuItem(name);
                toolStripMenuItemSubPoint.Tag = toolStripDropDownButtonSort.DropDownItems.Count - 2;
                toolStripMenuItemSubPoint.Click += new System.EventHandler(this.sortToolStripMenuItem_Click);
                toolStripDropDownButtonSort.DropDownItems.Add(toolStripMenuItemSubPoint);
            }

            itemCalculations = new Dictionary<string, ComparisonCalculationBase[]>();
            List<ComparisonCalculationBase> all = new List<ComparisonCalculationBase>();
            foreach (KeyValuePair<Character.CharacterSlot, List<ComparisonCalculationBase>> kv in calculations)
            {
                all.AddRange(kv.Value);
                itemCalculations["Gear." + kv.Key.ToString()] = kv.Value.ToArray();
            }
            itemCalculations["Gear.All"] = all.ToArray();
            slotToolStripMenuItem_Click(allToolStripMenuItem, null);
        }

        private void slotToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            foreach (ToolStripItem item in toolStripDropDownButtonSlot.DropDownItems)
            {
                if (item is ToolStripMenuItem)
                {
                    (item as ToolStripMenuItem).Checked = item == sender;
                    if ((item as ToolStripMenuItem).Checked)
                    {
                        string[] tag = item.Tag.ToString().Split('.');
                        toolStripDropDownButtonSlot.Text = tag[0];
                        if (tag.Length > 1) toolStripDropDownButtonSlot.Text += " > " + item.Text;
                        comparisonGraph1.RoundValues = true;
                        comparisonGraph1.ItemCalculations = itemCalculations[(string)item.Tag];
                    }
                }
            }
            this.Cursor = Cursors.Default;
        }

        private void sortToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            ComparisonGraph.ComparisonSort sort = ComparisonGraph.ComparisonSort.Overall;
            foreach (ToolStripItem item in toolStripDropDownButtonSort.DropDownItems)
            {
                if (item is ToolStripMenuItem)
                {
                    (item as ToolStripMenuItem).Checked = item == sender;
                    if ((item as ToolStripMenuItem).Checked)
                    {
                        toolStripDropDownButtonSort.Text = item.Text;
                        sort = (ComparisonGraph.ComparisonSort)((int)item.Tag);
                    }
                }
            }
            comparisonGraph1.Sort = sort;
            this.Cursor = Cursors.Default;
        }

        private void FormUpgradeComparison_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
        }
    }
}
