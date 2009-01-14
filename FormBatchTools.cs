using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.IO;

namespace Rawr
{
    public partial class FormBatchTools : Form
    {
        private bool _unsavedChanges;
        private string _filePath;

        FormMain formMain;

        Optimizer _optimizer;

        private enum AsyncOperation
        {
            None,
            Optimize,
            BuildUpgradeList
        }

        AsyncOperation currentOperation;
        int batchIndex;

        // optimize state
        int optimizerRound;

        // build upgrade list state
        int upgradeListPhase;
        private class UpgradeEntry
        {
            public Item Item { get; set; }
            public Enchant Enchant { get; set; }
            public float Value { get; set; }
            private List<float> valueList = new List<float>();
            public List<float> ValueList
            {
                get
                {
                    return valueList;
                }
            }
        }
        Dictionary<Character.CharacterSlot, Dictionary<string, UpgradeEntry>> upgradeList;
        IEnumerator<UpgradeEntry> upgradeListEnumerator;

        IEnumerator<UpgradeEntry> GetUpgradeListEnumerator()
        {
            foreach (KeyValuePair<Character.CharacterSlot, Dictionary<string, UpgradeEntry>> kvp in upgradeList)
            {
                foreach (UpgradeEntry entry in kvp.Value.Values)
                {
                    yield return entry;
                }
            }
        }

        private BatchCharacterList BatchCharacterList
        {
            get
            {
                return (BatchCharacterList)batchCharacterListBindingSource.DataSource;
            }
            set
            {
                batchCharacterListBindingSource.DataSource = value;
            }
        }

        private BatchCharacter CurrentBatchCharacter
        {
            get
            {
                if (batchIndex < 0 || batchIndex >= BatchCharacterList.Count) return null;
                return BatchCharacterList[batchIndex];
            }
        }

        public FormBatchTools(FormMain formMain)
        {
            InitializeComponent();

            this.formMain = formMain;
            batchCharacterListBindingSource.DataSource = new BatchCharacterList();

            TableLayoutSettings layout = (TableLayoutSettings)statusStrip1.LayoutSettings;
            layout.RowCount = 1;
            layout.ColumnCount = 2;
            layout.ColumnStyles.Clear();
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
            layout.RowStyles.Clear();
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, statusStrip1.Height));
            statusProgressBar.Dock = DockStyle.Fill;

            _optimizer = new Optimizer();
            _optimizer.OptimizeCharacterProgressChanged += new OptimizeCharacterProgressChangedEventHandler(_optimizer_OptimizeCharacterProgressChanged);
            _optimizer.OptimizeCharacterCompleted += new OptimizeCharacterCompletedEventHandler(_optimizer_OptimizeCharacterCompleted);
            _optimizer.ComputeUpgradesProgressChanged += new ComputeUpgradesProgressChangedEventHandler(_optimizer_ComputeUpgradesProgressChanged);
            _optimizer.ComputeUpgradesCompleted += new ComputeUpgradesCompletedEventHandler(_optimizer_ComputeUpgradesCompleted);
            _optimizer.EvaluateUpgradeProgressChanged += new ProgressChangedEventHandler(_optimizer_EvaluateUpgradeProgressChanged);
            _optimizer.EvaluateUpgradeCompleted += new EvaluateUpgradeCompletedEventHandler(_optimizer_EvaluateUpgradeCompleted);

            checkBoxOverrideRegem.Checked = Properties.Optimizer.Default.OverrideRegem;
            checkBoxOverrideReenchant.Checked = Properties.Optimizer.Default.OverrideReenchant;
            trackBarThoroughness.Value = Properties.Optimizer.Default.Thoroughness;
        }

        void _optimizer_OptimizeCharacterProgressChanged(object sender, OptimizeCharacterProgressChangedEventArgs e)
        {
            switch (currentOperation)
            {
                case AsyncOperation.Optimize:
                    statusLabel.Text = string.Format("[{2}/{3}] {0}: {1}", CurrentBatchCharacter.Name, e.BestValue, batchIndex + 1, BatchCharacterList.Count);
                    statusProgressBar.Value = e.ProgressPercentage;
                    break;
            }
        }

        void _optimizer_OptimizeCharacterCompleted(object sender, OptimizeCharacterCompletedEventArgs e)
        {
            int maxRounds = trackBarMaxRounds.Value;

            switch (currentOperation)
            {
                case AsyncOperation.Optimize:
                    if (e.Cancelled || e.Error != null)
                    {
                        currentOperation = AsyncOperation.None;
                        buttonCancel.Enabled = false;
                        statusLabel.Text = "";
                        statusProgressBar.Value = 0;
                        break;
                    }
                    if (e.OptimizedCharacterValue > e.CurrentCharacterValue + 0.00001f)
                    {
                        Character _character = CurrentBatchCharacter.Character;
                        Character bestCharacter = e.OptimizedCharacter;

                        _character.Back = bestCharacter.Back == null ? null : ItemCache.FindItemById(bestCharacter.Back.GemmedId);
                        _character.Chest = bestCharacter.Chest == null ? null : ItemCache.FindItemById(bestCharacter.Chest.GemmedId);
                        _character.Feet = bestCharacter.Feet == null ? null : ItemCache.FindItemById(bestCharacter.Feet.GemmedId);
                        _character.Finger1 = bestCharacter.Finger1 == null ? null : ItemCache.FindItemById(bestCharacter.Finger1.GemmedId);
                        _character.Finger2 = bestCharacter.Finger2 == null ? null : ItemCache.FindItemById(bestCharacter.Finger2.GemmedId);
                        _character.Hands = bestCharacter.Hands == null ? null : ItemCache.FindItemById(bestCharacter.Hands.GemmedId);
                        _character.Head = bestCharacter.Head == null ? null : ItemCache.FindItemById(bestCharacter.Head.GemmedId);
                        _character.Legs = bestCharacter.Legs == null ? null : ItemCache.FindItemById(bestCharacter.Legs.GemmedId);
                        _character.MainHand = bestCharacter.MainHand == null ? null : ItemCache.FindItemById(bestCharacter.MainHand.GemmedId);
                        _character.Neck = bestCharacter.Neck == null ? null : ItemCache.FindItemById(bestCharacter.Neck.GemmedId);
                        _character.OffHand = bestCharacter.OffHand == null ? null : ItemCache.FindItemById(bestCharacter.OffHand.GemmedId);
                        _character.Projectile = bestCharacter.Projectile == null ? null : ItemCache.FindItemById(bestCharacter.Projectile.GemmedId);
                        _character.ProjectileBag = bestCharacter.ProjectileBag == null ? null : ItemCache.FindItemById(bestCharacter.ProjectileBag.GemmedId);
                        _character.Ranged = bestCharacter.Ranged == null ? null : ItemCache.FindItemById(bestCharacter.Ranged.GemmedId);
                        _character.Shoulders = bestCharacter.Shoulders == null ? null : ItemCache.FindItemById(bestCharacter.Shoulders.GemmedId);
                        _character.Trinket1 = bestCharacter.Trinket1 == null ? null : ItemCache.FindItemById(bestCharacter.Trinket1.GemmedId);
                        _character.Trinket2 = bestCharacter.Trinket2 == null ? null : ItemCache.FindItemById(bestCharacter.Trinket2.GemmedId);
                        _character.Waist = bestCharacter.Waist == null ? null : ItemCache.FindItemById(bestCharacter.Waist.GemmedId);
                        _character.Wrist = bestCharacter.Wrist == null ? null : ItemCache.FindItemById(bestCharacter.Wrist.GemmedId);
                        _character.BackEnchant = bestCharacter.BackEnchant;
                        _character.ChestEnchant = bestCharacter.ChestEnchant;
                        _character.FeetEnchant = bestCharacter.FeetEnchant;
                        _character.Finger1Enchant = bestCharacter.Finger1Enchant;
                        _character.Finger2Enchant = bestCharacter.Finger2Enchant;
                        _character.HandsEnchant = bestCharacter.HandsEnchant;
                        _character.HeadEnchant = bestCharacter.HeadEnchant;
                        _character.LegsEnchant = bestCharacter.LegsEnchant;
                        _character.MainHandEnchant = bestCharacter.MainHandEnchant;
                        _character.OffHandEnchant = bestCharacter.OffHandEnchant;
                        _character.RangedEnchant = bestCharacter.RangedEnchant;
                        _character.ShouldersEnchant = bestCharacter.ShouldersEnchant;
                        _character.WristEnchant = bestCharacter.WristEnchant;
                        _character.OnCalculationsInvalidated();

                        CurrentBatchCharacter.UnsavedChanges = true;
                        //CurrentBatchCharacter.NewScore = e.OptimizedCharacterValue;
                        CurrentBatchCharacter.NewScore = Optimizer.GetOptimizationValue(_character, CurrentBatchCharacter.Model); // on item change always evaluate with equipped gear first (needed by mage module to store incremental data)

                        optimizerRound = 0;
                    }
                    else if (Math.Abs(e.OptimizedCharacterValue - e.CurrentCharacterValue) < 0.00001f && !e.CurrentCharacterInjected)
                    {
                        optimizerRound = maxRounds;
                    }
                    else
                    {
                        optimizerRound++;
                    }
                    if (optimizerRound >= maxRounds)
                    {
                        do
                        {
                            batchIndex++;
                        } while (batchIndex < BatchCharacterList.Count && CurrentBatchCharacter.Character == null);
                        optimizerRound = 0;
                    }

                    if (batchIndex < BatchCharacterList.Count)
                    {
                        OptimizeCurrentBatchCharacter();
                    }
                    else
                    {
                        currentOperation = AsyncOperation.None;
                        buttonCancel.Enabled = false;
                        statusLabel.Text = "";
                        statusProgressBar.Value = 0;
                    }
                    break;
            }
        }

        void _optimizer_ComputeUpgradesProgressChanged(object sender, ComputeUpgradesProgressChangedEventArgs e)
        {
            switch (currentOperation)
            {
                case AsyncOperation.BuildUpgradeList:
                    statusLabel.Text = string.Format("[{2}/{3}] {0}: {1}", CurrentBatchCharacter.Name, e.CurrentItem, batchIndex + 1, BatchCharacterList.Count);
                    statusProgressBar.Value = e.ProgressPercentage;
                    break;
            }
        }

        void _optimizer_ComputeUpgradesCompleted(object sender, ComputeUpgradesCompletedEventArgs e)
        {
            switch (currentOperation)
            {
                case AsyncOperation.BuildUpgradeList:
                    if (e.Cancelled || e.Error != null)
                    {
                        currentOperation = AsyncOperation.None;
                        buttonCancel.Enabled = false;
                        statusLabel.Text = "";
                        statusProgressBar.Value = 0;
                        break;
                    }
                    if (upgradeListPhase == 0)
                    {
                        foreach (KeyValuePair<Character.CharacterSlot, List<ComparisonCalculationBase>> kvp in e.Upgrades)
                        {
                            Dictionary<string, UpgradeEntry> map;
                            if (!upgradeList.TryGetValue(kvp.Key, out map))
                            {
                                map = new Dictionary<string, UpgradeEntry>();
                                upgradeList[kvp.Key] = map;
                            }
                            foreach (ComparisonCalculationBase comp in kvp.Value)
                            {
                                string key = comp.Item.GemmedId + "." + ((comp.Enchant == null) ? "0" : comp.Enchant.Id.ToString());
                                UpgradeEntry upgradeEntry;
                                if (!map.TryGetValue(key, out upgradeEntry))
                                {
                                    upgradeEntry = new UpgradeEntry();
                                    upgradeEntry.Item = comp.Item;
                                    upgradeEntry.Enchant = comp.Enchant;
                                    map[key] = upgradeEntry;
                                }
                            }
                        }
                        do
                        {
                            batchIndex++;
                        } while (batchIndex < BatchCharacterList.Count && CurrentBatchCharacter.Character == null);
                        if (batchIndex < BatchCharacterList.Count)
                        {
                            ComputeUpgradesCurrentBatchCharacter();
                        }
                        else
                        {
                            upgradeListPhase = 1;
                            batchIndex = 0;
                            upgradeListEnumerator = GetUpgradeListEnumerator();
                            if (upgradeListEnumerator.MoveNext())
                            {
                                EvaluateUpgradeCurrentBatchCharacter(true);
                            }
                            else
                            {
                                // upgrade list is empty, abort
                                currentOperation = AsyncOperation.None;
                                buttonCancel.Enabled = false;
                                statusLabel.Text = "";
                                statusProgressBar.Value = 0;
                                break;
                            }
                        }
                    }
                    break;
            }
        }

        void _optimizer_EvaluateUpgradeProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            switch (currentOperation)
            {
                case AsyncOperation.BuildUpgradeList:
                    statusLabel.Text = string.Format("[{2}/{3}] {0}: {1}", CurrentBatchCharacter.Name, upgradeListEnumerator.Current.Item.Name, batchIndex + 1, BatchCharacterList.Count);
                    statusProgressBar.Value = e.ProgressPercentage;
                    break;
            }
        }

        void _optimizer_EvaluateUpgradeCompleted(object sender, EvaluateUpgradeCompletedEventArgs e)
        {
            switch (currentOperation)
            {
                case AsyncOperation.BuildUpgradeList:
                    upgradeListEnumerator.Current.Value += e.UpgradeValue * CurrentBatchCharacter.Weight;
                    upgradeListEnumerator.Current.ValueList.Add(e.UpgradeValue);
                    if (upgradeListEnumerator.MoveNext())
                    {
                        EvaluateUpgradeCurrentBatchCharacter(false);
                    }
                    else
                    {
                        do
                        {
                            batchIndex++;
                        } while (batchIndex < BatchCharacterList.Count && CurrentBatchCharacter.Character == null);
                        if (batchIndex < BatchCharacterList.Count)
                        {
                            upgradeListEnumerator = GetUpgradeListEnumerator();
                            upgradeListEnumerator.MoveNext();
                            EvaluateUpgradeCurrentBatchCharacter(true);
                        }
                        else
                        {
                            currentOperation = AsyncOperation.None;
                            buttonCancel.Enabled = false;
                            statusLabel.Text = "";
                            statusProgressBar.Value = 0;

                            float totalValue = 0f;
                            foreach (BatchCharacter batchCharacter in BatchCharacterList)
                            {
                                if (batchCharacter.Character != null)
                                {
                                    totalValue += batchCharacter.Weight;
                                }
                            }

                            Dictionary<Character.CharacterSlot, List<ComparisonCalculationBase>> upgrades = new Dictionary<Character.CharacterSlot,List<ComparisonCalculationBase>>();

                            foreach (var kvp in upgradeList)
                            {
                                Dictionary<int, UpgradeEntry> filtered = new Dictionary<int, UpgradeEntry>();
                                foreach (UpgradeEntry entry in kvp.Value.Values)
                                {
                                    UpgradeEntry existingEntry;
                                    filtered.TryGetValue(entry.Item.Id, out existingEntry);
                                    if (entry.Value > 0 && (existingEntry == null || entry.Value > existingEntry.Value))
                                    {
                                        filtered[entry.Item.Id] = entry;
                                    }
                                }

                                upgrades[kvp.Key] = new List<ComparisonCalculationBase>();
                                foreach (UpgradeEntry entry in filtered.Values)
                                {
                                    ComparisonCalculationBase itemCalc = Calculations.CreateNewComparisonCalculation();
                                    itemCalc.Item = entry.Item;
                                    itemCalc.Enchant = entry.Enchant;
                                    itemCalc.Character = null;
                                    itemCalc.Name = entry.Item.Name;
                                    itemCalc.Equipped = false;
                                    itemCalc.OverallPoints = entry.Value / totalValue;
                                    itemCalc.SubPoints = entry.ValueList.ToArray();

                                    upgrades[kvp.Key].Add(itemCalc);                                    
                                }
                            }
                            List<string> customSubpoints = new List<string>();
                            foreach (BatchCharacter batchCharacter in BatchCharacterList)
                            {
                                customSubpoints.Add(batchCharacter.Name);
                            }
                            FormUpgradeComparison.Instance.LoadData(formMain.Character, upgrades, customSubpoints.ToArray());
                            FormUpgradeComparison.Instance.Show();
                        }
                    }
                    break;
            }
        }

        private void FormBatchTools_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = _optimizer.IsBusy || !PromptToSaveBeforeClosing();
        }

        private void FormBatchTools_FormClosed(object sender, FormClosedEventArgs e)
        {
            formMain.UnloadBatchCharacter();

            Properties.Optimizer.Default.OverrideRegem = checkBoxOverrideRegem.Checked;
            Properties.Optimizer.Default.OverrideReenchant = checkBoxOverrideReenchant.Checked;
            Properties.Optimizer.Default.Thoroughness = trackBarThoroughness.Value;
            Properties.Optimizer.Default.Save();
        }

        private bool PromptToSaveBeforeClosing()
        {
            if (_unsavedChanges)
            {
                DialogResult result = MessageBox.Show("Would you like to save the current batch list before closing it?", "Rawr - Save?", MessageBoxButtons.YesNoCancel);
                switch (result)
                {
                    case DialogResult.Yes:
                        saveToolStripMenuItem_Click(null, null);
                        return !string.IsNullOrEmpty(_filePath);
                    case DialogResult.No:
                        return true;
                    default:
                        return false;
                }
            }
            else
                return true;
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (PromptToSaveBeforeClosing())
            {
                _unsavedChanges = false;
                _filePath = null;
                batchCharacterListBindingSource.DataSource = new BatchCharacterList();
            }
        }

        private void importToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (PromptToSaveBeforeClosing())
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.DefaultExt = ".xml";
                dialog.Filter = "Rawr Xml Character Files | *.xml";
                dialog.Multiselect = true;
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    _unsavedChanges = true;
                    _filePath = null;
                    BatchCharacterList list = new BatchCharacterList();
                    foreach (string filename in dialog.FileNames)
                    {
                        list.Add(new BatchCharacter() { RelativePath = RelativePath(filename, AppDomain.CurrentDomain.BaseDirectory) });
                    }
                    batchCharacterListBindingSource.DataSource = list;
                }
                dialog.Dispose();
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (PromptToSaveBeforeClosing())
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.DefaultExt = ".xml";
                dialog.Filter = "Rawr Batch Files | *.xml";
                dialog.Multiselect = false;
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    _filePath = dialog.FileName;
                    batchCharacterListBindingSource.DataSource = BatchCharacterList.Load(_filePath);
                }
                dialog.Dispose();
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(_filePath))
            {
                ((BatchCharacterList)batchCharacterListBindingSource.DataSource).Save(_filePath);
                _unsavedChanges = false;
            }
            else
            {
                saveAsToolStripMenuItem_Click(null, null);
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.DefaultExt = ".xml";
            dialog.Filter = "Rawr Batch Files | *.xml";
            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                _filePath = dialog.FileName;
                ((BatchCharacterList)batchCharacterListBindingSource.DataSource).Save(_filePath);
                _unsavedChanges = false;
            }
            dialog.Dispose();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        #region Custom Controls
        private class MyStatusStrip : StatusStrip
        {
            // disable spring behavior to manually control the grid
            protected override void OnSpringTableLayoutCore()
            {
            }
        }

        private class MyDataGridViewButtonColumn : DataGridViewButtonColumn
        {
            public MyDataGridViewButtonColumn()
            {
                CellTemplate = new MyDataGridViewButtonCell();
                NewRowButtonVisible = false;
            }

            public override object Clone()
            {
                MyDataGridViewButtonColumn column = (MyDataGridViewButtonColumn)base.Clone();
                column.NewRowButtonVisible = this.NewRowButtonVisible;
                return column;
            }

            [DefaultValue(false)]
            public bool NewRowButtonVisible { get; set; }
        }

        private class MyDataGridViewButtonCell : DataGridViewButtonCell
        {
            public MyDataGridViewButtonCell()
            {
                ButtonVisible = true;
            }

            public bool ButtonVisible { get; set; }

            public override object Clone()
            {
                MyDataGridViewButtonCell cell = (MyDataGridViewButtonCell)base.Clone();
                cell.ButtonVisible = this.ButtonVisible;
                return cell;
            }

            protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates elementState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
            {
                MyDataGridViewButtonColumn owner = (MyDataGridViewButtonColumn)OwningColumn;
                if (ButtonVisible && (owner.NewRowButtonVisible || rowIndex != DataGridView.NewRowIndex))
                {
                    if (UseColumnTextForButtonValue)
                    {
                        base.Paint(graphics, clipBounds, cellBounds, rowIndex, elementState, value, owner.Text, errorText, cellStyle, advancedBorderStyle, paintParts);
                    }
                    else
                    {
                        base.Paint(graphics, clipBounds, cellBounds, rowIndex, elementState, value, formattedValue, errorText, cellStyle, advancedBorderStyle, paintParts);
                    }
                }
                else
                {
                    if ((paintParts & DataGridViewPaintParts.Background) == DataGridViewPaintParts.Background)
                    {
                        SolidBrush brush = new SolidBrush(((paintParts & DataGridViewPaintParts.SelectionBackground) == DataGridViewPaintParts.SelectionBackground && (elementState & DataGridViewElementStates.Selected) == DataGridViewElementStates.Selected) ? cellStyle.SelectionBackColor : cellStyle.BackColor);
                        graphics.FillRectangle(brush, cellBounds);
                        brush.Dispose();
                    }

                    if ((paintParts & DataGridViewPaintParts.Border) == DataGridViewPaintParts.Border)
                    {
                        PaintBorder(graphics, clipBounds, cellBounds, cellStyle, advancedBorderStyle);
                    }
                }
            }
        }
        #endregion

        private void dataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            if (e.ColumnIndex == loadBatchCharacterColumn.Index)
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.DefaultExt = ".xml";
                dialog.Filter = "Rawr Xml Character Files | *.xml";
                dialog.Multiselect = false;
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    dataGridView[e.ColumnIndex, e.RowIndex].Value = RelativePath(dialog.FileName, AppDomain.CurrentDomain.BaseDirectory);
                    dataGridView.NotifyCurrentCellDirty(true); // force DataGridView to add new row
                    dataGridView.NotifyCurrentCellDirty(false);
                }
                dialog.Dispose();
            }
            else if (e.ColumnIndex == showBatchCharacterColumn.Index && e.RowIndex != dataGridView.NewRowIndex)
            {
                formMain.LoadBatchCharacter(dataGridView.Rows[e.RowIndex].DataBoundItem as BatchCharacter);
            }
        }

        // based on http://mrpmorris.blogspot.com/2007/05/convert-absolute-path-to-relative-path.html
        private string RelativePath(string absolutePath, string relativeTo)
        {
            string[] relativeDirectories = absolutePath.Split(Path.DirectorySeparatorChar);
            string[] absoluteDirectories = relativeTo.Split(Path.DirectorySeparatorChar);
            //Get the shortest of the two paths
            int length = absoluteDirectories.Length < relativeDirectories.Length ? absoluteDirectories.Length : relativeDirectories.Length;
            //Use to determine where in the loop we exited
            int lastCommonRoot = -1;
            int index;
            //Find common root
            for (index = 0; index < length; index++)
                if (absoluteDirectories[index] == relativeDirectories[index])
                    lastCommonRoot = index;
                else
                    break;
            //If we didn't find a common prefix then throw
            if (lastCommonRoot == -1)
                return absolutePath;
            //Build up the relative path
            StringBuilder relativePath = new StringBuilder();
            //Add on the ..
            for (index = lastCommonRoot + 1; index < absoluteDirectories.Length; index++)
                if (absoluteDirectories[index].Length > 0)
                    relativePath.Append(".." + Path.DirectorySeparatorChar);
            //Add on the folders
            for (index = lastCommonRoot + 1; index < relativeDirectories.Length - 1; index++)
                relativePath.Append(relativeDirectories[index] + Path.DirectorySeparatorChar);
            relativePath.Append(relativeDirectories[relativeDirectories.Length - 1]);
            return relativePath.ToString();
        }

        private void setAvailableGearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (BatchCharacter character in BatchCharacterList)
            {
                if (character.Character != null)
                {
                    character.Character.AvailableItems = new List<string>(formMain.Character.AvailableItems);
                    character.UnsavedChanges = true;
                }
            }
        }

        private void replaceUnavailableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (BatchCharacter character in BatchCharacterList)
            {
                if (character.Character != null)
                {
                    for (int slot = 0; slot < 19; slot++)
                    {
                        Item item = character.Character[(Character.CharacterSlot)slot];
                        Enchant enchant = character.Character.GetEnchantBySlot((Character.CharacterSlot)slot);
                        if (item != null)
                        {
                            string id = item.Id.ToString();
                            string anyGem = id + ".*.*.*";
                            List<string> list = character.Character.AvailableItems.FindAll(x => x.StartsWith(id));
                            List<string> sublist;
                            if (list.Contains(item.GemmedId + ".*"))
                            {
                                // available
                            }
                            else if ((sublist = list.FindAll(x => x.StartsWith(item.GemmedId))).Count > 0)
                            {
                                if (sublist.Contains(item.GemmedId + "." + (enchant != null ? enchant.Id.ToString() : "0")))
                                {
                                    // available
                                }
                                else
                                {
                                    // have to replace enchant
                                    string s = sublist[0];
                                    character.Character.SetEnchantBySlot((Character.CharacterSlot)slot, Enchant.FindEnchant(int.Parse(s.Substring(s.LastIndexOf('.') + 1)), item.Slot, character.Character));
                                    character.UnsavedChanges = true;
                                }
                            }
                            else if (list.Contains(id))
                            {
                                // available
                            }
                            else if ((sublist = list.FindAll(x => x.StartsWith(anyGem))).Count > 0)
                            {
                                if (sublist.Contains(anyGem + "." + (enchant != null ? enchant.Id.ToString() : "0")))
                                {
                                    // available
                                }
                                else
                                {
                                    // have to replace enchant
                                    string s = sublist[0];
                                    character.Character.SetEnchantBySlot((Character.CharacterSlot)slot, Enchant.FindEnchant(int.Parse(s.Substring(s.LastIndexOf('.') + 1)), item.Slot, character.Character));
                                    character.UnsavedChanges = true;
                                }
                            }
                            else if (list.Count > 0)
                            {
                                string s = list[0];
                                item = ItemCache.FindItemById(s.Substring(0, s.LastIndexOf('.')));
                                character.Character[(Character.CharacterSlot)slot] = item;
                                string se = s.Substring(s.LastIndexOf('.') + 1);
                                if (se != "*")
                                {
                                    character.Character.SetEnchantBySlot((Character.CharacterSlot)slot, Enchant.FindEnchant(int.Parse(se), item.Slot, character.Character));
                                }
                                character.UnsavedChanges = true;
                            }
                            else
                            {
                                foreach (string s in character.Character.AvailableItems)
                                {
                                    if (s.IndexOf('.') < 0)
                                    {
                                        item = ItemCache.FindItemById(int.Parse(s));
                                    }
                                    else
                                    {
                                        string[] slist = s.Split('.');
                                        if (slist[1] == "*")
                                        {
                                            item = ItemCache.FindItemById(int.Parse(slist[0]));
                                        }
                                        else
                                        {
                                            item = ItemCache.FindItemById(s.Substring(0, s.LastIndexOf('.')));
                                        }
                                    }
                                    if (item != null && item.FitsInSlot((Character.CharacterSlot)slot, character.Character))
                                    {
                                        character.Character[(Character.CharacterSlot)slot] = item;
                                        string se = s.Substring(s.LastIndexOf('.') + 1);
                                        if (se != "*")
                                        {
                                            character.Character.SetEnchantBySlot((Character.CharacterSlot)slot, Enchant.FindEnchant(int.Parse(se), item.Slot, character.Character));
                                        }
                                        character.UnsavedChanges = true;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void saveCharactersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BatchCharacterList list = (BatchCharacterList)batchCharacterListBindingSource.DataSource;

            foreach (BatchCharacter character in list)
            {
                if (character.Character != null && character.UnsavedChanges)
                {
                    character.Character.Save(character.AbsulutePath);
                    character.UnsavedChanges = false;
                }
            }
        }

        private void saveCharactersAsCopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            BatchCharacterList list = (BatchCharacterList)batchCharacterListBindingSource.DataSource;

            foreach (BatchCharacter character in list)
            {
                if (character.Character != null && character.UnsavedChanges)
                {
                    string copyPath = Path.ChangeExtension(character.AbsulutePath, null) + " " + now.ToString("yyyy-M-d H-m") + ".xml";
                    character.Character.Save(copyPath);
                }
            }
        }

        private void optimizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentOperation != AsyncOperation.None) return;

            currentOperation = AsyncOperation.Optimize;
            buttonCancel.Enabled = true;

            batchIndex = 0;
            optimizerRound = 0;
            OptimizeCurrentBatchCharacter();
        }

        private void OptimizeCurrentBatchCharacter()
        {
            int _thoroughness = trackBarThoroughness.Value;

            if (optimizerRound == 0)
            {
                bool _overrideRegem = checkBoxOverrideRegem.Checked;
                bool _overrideReenchant = checkBoxOverrideReenchant.Checked;
                _optimizer.InitializeItemCache(CurrentBatchCharacter.Character.AvailableItems, _overrideRegem, _overrideReenchant, CurrentBatchCharacter.Model);
            }
            _optimizer.OptimizeCharacterAsync(CurrentBatchCharacter.Character, CurrentBatchCharacter.Character.CalculationToOptimize, CurrentBatchCharacter.Character.OptimizationRequirements.ToArray(), _thoroughness, true);
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            if (currentOperation != AsyncOperation.None && _optimizer.IsBusy)
            {
                _optimizer.CancelAsync();
            }
        }

        private void batchCharacterListBindingSource_ListChanged(object sender, ListChangedEventArgs e)
        {
            //if (e.ListChangedType == ListChangedType.ItemAdded || e.ListChangedType == ListChangedType.ItemDeleted) _unsavedChanges = true;
        }

        private void buildUpgradeListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentOperation != AsyncOperation.None) return;

            currentOperation = AsyncOperation.BuildUpgradeList;
            buttonCancel.Enabled = true;

            batchIndex = 0;
            upgradeListPhase = 0;
            upgradeList = new Dictionary<Character.CharacterSlot, Dictionary<string, UpgradeEntry>>();
            ComputeUpgradesCurrentBatchCharacter();
        }

        private void ComputeUpgradesCurrentBatchCharacter()
        {
            int _thoroughness = trackBarThoroughness.Value;
            bool _overrideRegem = checkBoxOverrideRegem.Checked;
            bool _overrideReenchant = checkBoxOverrideReenchant.Checked;
            _optimizer.InitializeItemCache(CurrentBatchCharacter.Character.AvailableItems, _overrideRegem, _overrideReenchant, CurrentBatchCharacter.Model);
            _optimizer.ComputeUpgradesAsync(CurrentBatchCharacter.Character, CurrentBatchCharacter.Character.CalculationToOptimize, CurrentBatchCharacter.Character.OptimizationRequirements.ToArray(), _thoroughness);
        }

        private void EvaluateUpgradeCurrentBatchCharacter(bool initializeCache)
        {
            int _thoroughness = trackBarThoroughness.Value;
            bool _overrideRegem = checkBoxOverrideRegem.Checked;
            bool _overrideReenchant = checkBoxOverrideReenchant.Checked;
            if (initializeCache) _optimizer.InitializeItemCache(CurrentBatchCharacter.Character.AvailableItems, _overrideRegem, _overrideReenchant, CurrentBatchCharacter.Model);
            _optimizer.EvaluateUpgradeAsync(CurrentBatchCharacter.Character, CurrentBatchCharacter.Character.CalculationToOptimize, CurrentBatchCharacter.Character.OptimizationRequirements.ToArray(), _thoroughness, upgradeListEnumerator.Current.Item, upgradeListEnumerator.Current.Enchant);
        }
    }

    [Serializable]
    public class BatchCharacter : INotifyPropertyChanged
    {
        private string relativePath;
        public string RelativePath
        {
            get
            {
                return relativePath;
            }
            set
            {
                if (relativePath != value)
                {
                    relativePath = value;
                    character = null;

                    string curDir = Directory.GetCurrentDirectory();
                    Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
                    absolutePath = Path.GetFullPath(relativePath);
                    Directory.SetCurrentDirectory(curDir);

                    score = Optimizer.GetOptimizationValue(Character, Model);

                    if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("Name"));
                    if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("Score"));
                }
            }
        }

        private string absolutePath;
        [XmlIgnore]
        public string AbsulutePath
        {
            get
            {
                return absolutePath;
            }
        }

        [XmlIgnore]
        public string Name
        {
            get
            {
                if (relativePath == null) return null;
                string name = Path.GetFileNameWithoutExtension(relativePath);
                if (unsavedChanges) name += " *";
                return name;
            }
        }

        private Character character;
        [XmlIgnore]
        public Character Character
        {
            get
            {
                if (character == null && absolutePath != null)
                {
                    character = Character.Load(absolutePath);
                }
                return character;
            }
        }

        private CalculationsBase model;
        [XmlIgnore]
        public CalculationsBase Model
        {
            get
            {
                if (model == null)
                {
                    model = Calculations.GetModel(Character.CurrentModel);
                }
                return model;
            }
        }

        private bool unsavedChanges;
        [XmlIgnore]
        public bool UnsavedChanges
        {
            get
            {
                return unsavedChanges;
            }
            set
            {
                if (unsavedChanges != value)
                {
                    if (!value)
                    {
                        if (newScore != null)
                        {
                            score = (float)newScore;
                            newScore = null;
                        }
                    }
                    unsavedChanges = value;
                    if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("Name"));
                    if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("Score"));
                    if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("NewScore"));
                }
            }
        }

        private float weight = 1f;
        public float Weight
        {
            get
            {
                return weight;
            }
            set
            {
                if (weight != value)
                {
                    weight = value;
                }
            }
        }

        private float score;
        [XmlIgnore]
        public float Score
        {
            get
            {
                return score;
            }
        }

        private float? newScore;
        [XmlIgnore]
        public float? NewScore
        {
            get
            {
                return newScore;
            }
            set
            {
                if (newScore != value)
                {
                    newScore = value;
                    if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("NewScore"));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    [Serializable]
    public class BatchCharacterList : BindingList<BatchCharacter>
    {
        public static BatchCharacterList Load(string path)
        {
            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(BatchCharacterList));
            System.IO.StreamReader reader = new System.IO.StreamReader(path);
            BatchCharacterList list = (BatchCharacterList)serializer.Deserialize(reader);
            reader.Close();
            return list;
        }

        public void Save(string path)
        {
            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(BatchCharacterList));
            System.IO.StreamWriter writer = new System.IO.StreamWriter(path);
            serializer.Serialize(writer, this);
            writer.Close();
        }
    }
}
