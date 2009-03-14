using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Rawr.Optimizer;
using System.IO;
using System.Xml.Serialization;

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


        private string[] customSubpoints;

        private void SetCustomSubpoints(string[] customSubpoints)
        {
            this.customSubpoints = customSubpoints;
            if (customSubpoints != null)
            {
                comparisonGraph1.DisplayMode = ComparisonGraph.GraphDisplayMode.CustomSubpoints;
                toolStripDropDownButtonSort.DropDownItems.Clear();
                toolStripDropDownButtonSort.DropDownItems.Add(overallToolStripMenuItem);
                toolStripDropDownButtonSort.DropDownItems.Add(alphabeticalToolStripMenuItem);
                foreach (string name in customSubpoints)
                {
                    ToolStripMenuItem toolStripMenuItemSubPoint = new ToolStripMenuItem(name);
                    toolStripMenuItemSubPoint.Tag = toolStripDropDownButtonSort.DropDownItems.Count - 2;
                    toolStripMenuItemSubPoint.Click += new System.EventHandler(this.sortToolStripMenuItem_Click);
                    toolStripDropDownButtonSort.DropDownItems.Add(toolStripMenuItemSubPoint);
                }
            }
            else
            {
                comparisonGraph1.DisplayMode = ComparisonGraph.GraphDisplayMode.Overall;
                toolStripDropDownButtonSort.DropDownItems.Clear();
                toolStripDropDownButtonSort.DropDownItems.Add(overallToolStripMenuItem);
                toolStripDropDownButtonSort.DropDownItems.Add(alphabeticalToolStripMenuItem);
            }
        }

        private Dictionary<string, ComparisonCalculationUpgrades[]> itemCalculations;

        [Serializable]
        public class SerializationData
        {
            public string[] CustomSubpoints;
            public List<string> Keys;
            public List<ComparisonCalculationUpgrades[]> ItemCalculations;
        }

        private void LoadFile(string file)
        {
            try
            {
                if (File.Exists(file))
                {
                    SerializationData data = new SerializationData();
                    using (StreamReader reader = new StreamReader(file, Encoding.UTF8))
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(SerializationData));
                        data = (SerializationData)serializer.Deserialize(reader);
                        reader.Close();
                    }
                    comparisonGraph1.EquipSlot = Character.CharacterSlot.AutoSelect;
                    comparisonGraph1.Character = FormMain.Instance.Character;
                    SetCustomSubpoints(data.CustomSubpoints);

                    itemCalculations = new Dictionary<string, ComparisonCalculationUpgrades[]>();
                    for (int i = 0; i < data.Keys.Count; i++)
                    {
                        itemCalculations[data.Keys[i]] = data.ItemCalculations[i];
                    }
                    slotToolStripMenuItem_Click(allToolStripMenuItem, null);
                }
            }
            catch
            {
            }
        }

        private void SaveFile(string file)
        {
            SerializationData data = new SerializationData();
            data.CustomSubpoints = customSubpoints;
            data.Keys = new List<string>();
            data.ItemCalculations = new List<ComparisonCalculationUpgrades[]>();
            foreach (KeyValuePair<string, ComparisonCalculationUpgrades[]> kvp in itemCalculations)
            {
                data.Keys.Add(kvp.Key);
                data.ItemCalculations.Add(kvp.Value);
            }

            try
            {
                using (StreamWriter writer = new StreamWriter(file, false, Encoding.UTF8))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(SerializationData));
                    serializer.Serialize(writer, data);
                    writer.Close();
                }
            }
            catch
            {
            }
        }

        public void LoadData(Dictionary<Character.CharacterSlot, List<ComparisonCalculationUpgrades>> calculations, string[] customSubpoints)
        {
            comparisonGraph1.EquipSlot = Character.CharacterSlot.AutoSelect;
            comparisonGraph1.Character = FormMain.Instance.Character;
            SetCustomSubpoints(customSubpoints);

            itemCalculations = new Dictionary<string, ComparisonCalculationUpgrades[]>();
            List<ComparisonCalculationUpgrades> all = new List<ComparisonCalculationUpgrades>();
            foreach (KeyValuePair<Character.CharacterSlot, List<ComparisonCalculationUpgrades>> kvp in calculations)
            {
                all.AddRange(kvp.Value);
                itemCalculations["Gear." + kvp.Key.ToString()] = kvp.Value.ToArray();
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

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.DefaultExt = ".xml";
            dialog.Filter = "Rawr Upgrade List Files | *.xml";
            dialog.Multiselect = false;
            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                LoadFile(dialog.FileName);
            }
            dialog.Dispose();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.DefaultExt = ".xml";
            dialog.Filter = "Rawr Upgrade List Files | *.xml";
            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                SaveFile(dialog.FileName);
            }
            dialog.Dispose();
        }

        private void copyDataToClipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            foreach (ComparisonCalculationUpgrades upgrade in itemCalculations["Gear.All"])
            {
                ItemInstance item = upgrade.ItemInstance;
                if (item != null)
                {
                    sb.AppendFormat("{0}, {1}, {2}, {3}, {4}, {5}", item.Item.Name, item.Gem1 != null ? item.Gem1.Name : null, item.Gem2 != null ? item.Gem2.Name : null, item.Gem3 != null ? item.Gem3.Name : null, item.Enchant.Name, upgrade.OverallPoints);
                    sb.AppendLine();
                }
            }

            Clipboard.SetText(sb.ToString(), TextDataFormat.Text);
            if (Type.GetType("Mono.Runtime") != null)
            {
                //Clipboard isn't working
                System.IO.File.WriteAllText("upgrades.txt", sb.ToString());
            }
        }
    }
}
