using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.IO;
using Rawr.Optimizer;

namespace Rawr
{
    public partial class FormBatchTools : Form
    {
        private bool _unsavedChanges;
        private string _filePath;

        FormMain formMain;

        ItemInstanceOptimizer optimizer;
        BatchOptimizer batchOptimizer;

        private enum AsyncOperation
        {
            None,
            Optimize,
            BuildUpgradeList,
            BatchOptimize,
            BuildBatchUpgradeList,
            ProgressiveOptimize,
            BuildProgressiveUpgradeList
        }

        AsyncOperation currentOperation;
        int batchIndex;
        AvailableItemGenerator itemGenerator;
        Item[] itemList;
        int itemIndex;
        ItemInstance optimizedItemInstance;
        Character workingCharacter;

        // optimize state
        int optimizerRound;

        // build upgrade list state
        int upgradeListPhase;
        private class UpgradeEntry
        {
            public ItemInstance Item { get; set; }
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
        Dictionary<CharacterSlot, Dictionary<string, UpgradeEntry>> upgradeList;
        IEnumerator<UpgradeEntry> upgradeListEnumerator;

        IEnumerator<UpgradeEntry> GetUpgradeListEnumerator()
        {
            foreach (KeyValuePair<CharacterSlot, Dictionary<string, UpgradeEntry>> kvp in upgradeList)
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

        private bool batchListReady = false;

        public FormBatchTools(FormMain formMain)
        {
            InitializeComponent();

            this.formMain = formMain;
            batchCharacterListBindingSource.DataSource = new BatchCharacterList();
            batchListReady = true;

            TableLayoutSettings layout = (TableLayoutSettings)statusStrip1.LayoutSettings;
            layout.RowCount = 1;
            layout.ColumnCount = 2;
            layout.ColumnStyles.Clear();
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
            layout.RowStyles.Clear();
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, statusStrip1.Height));
            statusProgressBar.Dock = DockStyle.Fill;

            optimizer = new ItemInstanceOptimizer();
            optimizer.OptimizeCharacterProgressChanged += new OptimizeCharacterProgressChangedEventHandler(_optimizer_OptimizeCharacterProgressChanged);
            optimizer.OptimizeCharacterCompleted += new OptimizeCharacterCompletedEventHandler(_optimizer_OptimizeCharacterCompleted);
            optimizer.ComputeUpgradesProgressChanged += new ComputeUpgradesProgressChangedEventHandler(_optimizer_ComputeUpgradesProgressChanged);
            optimizer.ComputeUpgradesCompleted += new ComputeUpgradesCompletedEventHandler(_optimizer_ComputeUpgradesCompleted);
            optimizer.EvaluateUpgradeProgressChanged += new ProgressChangedEventHandler(_optimizer_EvaluateUpgradeProgressChanged);
            optimizer.EvaluateUpgradeCompleted += new EvaluateUpgradeCompletedEventHandler(_optimizer_EvaluateUpgradeCompleted);

            checkBoxOverrideRegem.Checked = Properties.Optimizer.Default.OverrideRegem;
            checkBoxOverrideReenchant.Checked = Properties.Optimizer.Default.OverrideReenchant;
            trackBarThoroughness.Value = Properties.Optimizer.Default.Thoroughness;
        }

        private Item SingleItemUpgrade
        {
            get
            {
                if (string.IsNullOrEmpty(textBoxSingleItemUpgrade.Text))
                {
                    return null;
                }
                else
                {
                    int id;
                    if (int.TryParse(textBoxSingleItemUpgrade.Text, out id))
                    {
                        return ItemCache.FindItemById(id);
                    }
                    else
                    {
                        // try to match by name
                        foreach (Item item in ItemCache.AllItems)
                        {
                            if (item.Name == textBoxSingleItemUpgrade.Text)
                            {
                                return item;
                            }
                        }
                    }
                    return null;
                }
            }
        }

        private void UpdateStatusLabel()
        {
            if (batchListReady)
            {
                statusLabel.Text = string.Format("{0} / {1}", BatchCharacterList.Score, BatchCharacterList.NewScore);
            }
        }

        void _optimizer_OptimizeCharacterProgressChanged(object sender, OptimizeCharacterProgressChangedEventArgs e)
        {
            switch (currentOperation)
            {
                case AsyncOperation.Optimize:
                case AsyncOperation.ProgressiveOptimize:
                    statusLabel.Text = string.Format("[{2}/{3}] {0}: {1}", CurrentBatchCharacter.Name, e.BestValue, batchIndex + 1, BatchCharacterList.Count);
                    statusProgressBar.Value = e.ProgressPercentage;
                    break;
                case AsyncOperation.BuildProgressiveUpgradeList:
                    statusLabel.Text = string.Format("[{4}/{5}] {1}: [{2}/{3}] {0}", CurrentBatchCharacter.Name, itemList[itemIndex].Name, batchIndex + 1, BatchCharacterList.Count, itemIndex + 1, itemList.Length);
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
                    {
                        if (e.Cancelled || e.Error != null)
                        {
                            currentOperation = AsyncOperation.None;
                            buttonCancel.Enabled = false;
                            UpdateStatusLabel();
                            statusProgressBar.Value = 0;
                            break;
                        }
                        if (e.OptimizedCharacterValue > e.CurrentCharacterValue + 0.00001f)
                        {
                            Character _character = CurrentBatchCharacter.Character;
                            Character bestCharacter = e.OptimizedCharacter;
                            _character.SetItems(bestCharacter);

                            //CurrentBatchCharacter.UnsavedChanges = true;
                            //CurrentBatchCharacter.NewScore = e.OptimizedCharacterValue;
                            //CurrentBatchCharacter.NewScore = ItemInstanceOptimizer.GetOptimizationValue(_character, CurrentBatchCharacter.Model); // on item change always evaluate with equipped gear first (needed by mage module to store incremental data)

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
                            UpdateStatusLabel();
                            statusProgressBar.Value = 0;
                        }
                    }
                    break;
                case AsyncOperation.ProgressiveOptimize:
                    {
                        if (e.Cancelled || e.Error != null)
                        {
                            currentOperation = AsyncOperation.None;
                            buttonCancel.Enabled = false;
                            UpdateStatusLabel();
                            statusProgressBar.Value = 0;
                            break;
                        }
                        // since we're injecting the character we'll always get at least what we started with
                        Character _character = CurrentBatchCharacter.Character;
                        Character bestCharacter = e.OptimizedCharacter;
                        _character.SetItems(bestCharacter);
                        // we have to perform item restrictions on the active item generator
                        // so that we don't regem/reenchant already used items
                        itemGenerator.AddItemRestrictions(bestCharacter);
                        // move to next batch character
                        do
                        {
                            batchIndex++;
                        } while (batchIndex < BatchCharacterList.Count && (CurrentBatchCharacter.Character == null || CurrentBatchCharacter.Locked));

                        if (batchIndex < BatchCharacterList.Count)
                        {
                            int _thoroughness = trackBarThoroughness.Value;
                            Character character = CurrentBatchCharacter.Character.Clone();
                            // regularize character with current item restrictions
                            itemGenerator.RegularizeCharacter(character);
                            optimizer.InitializeItemCache(character, CurrentBatchCharacter.Model, itemGenerator);
                            optimizer.OptimizeCharacterAsync(character, CurrentBatchCharacter.Character.CalculationToOptimize, CurrentBatchCharacter.Character.OptimizationRequirements.ToArray(), _thoroughness, true);
                        }
                        else
                        {
                            currentOperation = AsyncOperation.None;
                            buttonCancel.Enabled = false;
                            UpdateStatusLabel();
                            statusProgressBar.Value = 0;
                        }
                    }
                    break;
                case AsyncOperation.BuildProgressiveUpgradeList:
                    {
                        if (e.Cancelled || e.Error != null)
                        {
                            currentOperation = AsyncOperation.None;
                            buttonCancel.Enabled = false;
                            UpdateStatusLabel();
                            statusProgressBar.Value = 0;
                            break;
                        }
                        // we optimized the character with new item restrictions
                        // update the working character and start upgrade evaluation
                        workingCharacter = e.OptimizedCharacter;
                        int _thoroughness = trackBarThoroughness.Value;
                        // evaluate what we get by using the optimized item
                        optimizer.EvaluateUpgradeAsync(workingCharacter, CurrentBatchCharacter.Character.CalculationToOptimize, CurrentBatchCharacter.Character.OptimizationRequirements.ToArray(), _thoroughness, optimizedItemInstance);
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
                case AsyncOperation.BuildProgressiveUpgradeList:
                    statusLabel.Text = string.Format("[{4}/{5}] {1}: [{2}/{3}] {0}", CurrentBatchCharacter.Name, e.CurrentItem, batchIndex + 1, BatchCharacterList.Count, itemIndex + 1, itemList.Length);
                    statusProgressBar.Value = e.ItemProgressPercentage;
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
                        UpdateStatusLabel();
                        statusProgressBar.Value = 0;
                        break;
                    }
                    if (upgradeListPhase == 0)
                    {
                        foreach (KeyValuePair<CharacterSlot, List<ComparisonCalculationUpgrades>> kvp in e.Upgrades)
                        {
                            Dictionary<string, UpgradeEntry> map;
                            if (!upgradeList.TryGetValue(kvp.Key, out map))
                            {
                                map = new Dictionary<string, UpgradeEntry>();
                                upgradeList[kvp.Key] = map;
                            }
                            foreach (ComparisonCalculationBase comp in kvp.Value)
                            {
                                string key = comp.ItemInstance.GemmedId;
                                UpgradeEntry upgradeEntry;
                                if (!map.TryGetValue(key, out upgradeEntry))
                                {
                                    upgradeEntry = new UpgradeEntry();
                                    upgradeEntry.Item = comp.ItemInstance;
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
                                UpdateStatusLabel();
                                statusProgressBar.Value = 0;
                                break;
                            }
                        }
                    }
                    break;
                case AsyncOperation.BuildProgressiveUpgradeList:
                    if (e.Cancelled || e.Error != null)
                    {
                        currentOperation = AsyncOperation.None;
                        buttonCancel.Enabled = false;
                        UpdateStatusLabel();
                        statusProgressBar.Value = 0;
                        break;
                    }
                    bool foundUpgrade = false;
                    foreach (KeyValuePair<CharacterSlot, List<ComparisonCalculationUpgrades>> kvp in e.Upgrades)
                    {
                        Dictionary<string, UpgradeEntry> map;
                        if (!upgradeList.TryGetValue(kvp.Key, out map))
                        {
                            map = new Dictionary<string, UpgradeEntry>();
                            upgradeList[kvp.Key] = map;
                        }
                        if (kvp.Value.Count > 0)
                        {
                            ComparisonCalculationUpgrades comp = kvp.Value[0];
                            upgradeListPhase = 1; // item was used, from now on we do evaluate upgrade on specific item instance only
                            optimizedItemInstance = comp.ItemInstance;
                            // make item restrictions based on best character
                            itemGenerator.AddItemRestrictions(comp.CharacterItems);
                            foundUpgrade = true;
                            string key = comp.ItemInstance.Id.ToString();
                            UpgradeEntry upgradeEntry;
                            if (!map.TryGetValue(key, out upgradeEntry))
                            {
                                upgradeEntry = new UpgradeEntry();
                                map[key] = upgradeEntry;
                            }
                            upgradeEntry.Item = comp.ItemInstance;
                            upgradeEntry.Value += comp.OverallPoints * CurrentBatchCharacter.Weight;
                            upgradeEntry.ValueList.Add(comp.OverallPoints);
                            break;
                        }
                    }
                    if (!foundUpgrade)
                    {
                        // make item restrictions based on best character without using the item
                        itemGenerator.AddItemRestrictions(workingCharacter);
                        // if we're evaluating an item that was already marked available then we must restrict to that version
                        // if it was used in this character
                        Item item = itemList[itemIndex];
                        CharacterSlot slot = Character.GetCharacterSlotByItemSlot(item.Slot);
                        if (upgradeListPhase == 0)
                        {
                            foreach (CharacterSlot s in Character.CharacterSlots)
                            {
                                ItemInstance itemInstance = workingCharacter[s];
                                if ((object)itemInstance != null && itemInstance.Id == item.Id)
                                {
                                    upgradeListPhase = 1;
                                    optimizedItemInstance = itemInstance;
                                    break;
                                }
                            }
                        }
                        Dictionary<string, UpgradeEntry> map;
                        if (!upgradeList.TryGetValue(slot, out map))
                        {
                            map = new Dictionary<string, UpgradeEntry>();
                            upgradeList[slot] = map;
                        }
                        string key = itemList[itemIndex].Id.ToString();
                        UpgradeEntry upgradeEntry;
                        if (!map.TryGetValue(key, out upgradeEntry))
                        {
                            upgradeEntry = new UpgradeEntry();
                            map[key] = upgradeEntry;
                        }
                        if (upgradeListPhase == 1)
                        {
                            upgradeEntry.Item = optimizedItemInstance;
                        }
                        upgradeEntry.ValueList.Add(0.0f);
                    }
                    // move to next character
                    do
                    {
                        batchIndex++;
                    } while (batchIndex < BatchCharacterList.Count && CurrentBatchCharacter.Character == null);
                    if (batchIndex < BatchCharacterList.Count)
                    {
                        if (upgradeListPhase == 0)
                        {
                            // so far we haven't made any changes yet
                            // we're working under assumption that the starting batch is valid i.e. an item will have the same gemming in all characters
                            int _thoroughness = trackBarThoroughness.Value;
                            workingCharacter = CurrentBatchCharacter.Character;
                            optimizer.InitializeItemCache(workingCharacter, CurrentBatchCharacter.Model, itemGenerator);
                            optimizer.ComputeUpgradesAsync(workingCharacter, CurrentBatchCharacter.Character.CalculationToOptimize, CurrentBatchCharacter.Character.OptimizationRequirements.ToArray(), _thoroughness, itemList[itemIndex]);
                        }
                        else
                        {
                            // we made item restrictions, first we have to optimize character without the item
                            int _thoroughness = trackBarThoroughness.Value;
                            workingCharacter = CurrentBatchCharacter.Character.Clone();
                            // regularize character with current item restrictions
                            itemGenerator.RegularizeCharacter(workingCharacter);
                            optimizer.InitializeItemCache(workingCharacter, CurrentBatchCharacter.Model, itemGenerator);
                            optimizer.OptimizeCharacterAsync(workingCharacter, CurrentBatchCharacter.Character.CalculationToOptimize, CurrentBatchCharacter.Character.OptimizationRequirements.ToArray(), _thoroughness, true);
                        }
                    }
                    else
                    {
                        // we finished all characters for this item
                        // move to next item
                        itemIndex++;
                        if (itemIndex < itemList.Length)
                        {
                            batchIndex = 0;
                            upgradeListPhase = 0;
                            int _thoroughness = trackBarThoroughness.Value;
                            // we have to reinitialize item generator because of the restrictions we made
                            CreateBatchItemGenerator();
                            optimizer.InitializeItemCache(CurrentBatchCharacter.Character, CurrentBatchCharacter.Model, itemGenerator);
                            workingCharacter = CurrentBatchCharacter.Character;
                            optimizer.ComputeUpgradesAsync(CurrentBatchCharacter.Character, CurrentBatchCharacter.Character.CalculationToOptimize, CurrentBatchCharacter.Character.OptimizationRequirements.ToArray(), _thoroughness, itemList[itemIndex]);
                        }
                        else
                        {
                            // we're done
                            WrapUpProgressiveUpgradeList();
                        }
                    }
                    break;
            }
        }

        void WrapUpProgressiveUpgradeList()
        {
            currentOperation = AsyncOperation.None;
            buttonCancel.Enabled = false;
            UpdateStatusLabel();
            statusProgressBar.Value = 0;

            float totalValue = 0f;
            foreach (BatchCharacter batchCharacter in BatchCharacterList)
            {
                if (batchCharacter.Character != null)
                {
                    totalValue += batchCharacter.Weight;
                }
            }

            Dictionary<CharacterSlot, List<ComparisonCalculationUpgrades>> upgrades = new Dictionary<CharacterSlot, List<ComparisonCalculationUpgrades>>();

            foreach (var kvp in upgradeList)
            {
                upgrades[kvp.Key] = new List<ComparisonCalculationUpgrades>();
                Dictionary<int, UpgradeEntry> filtered = new Dictionary<int, UpgradeEntry>();
                foreach (UpgradeEntry entry in kvp.Value.Values)
                {
                    if (entry.Value > 0)
                    {
                        ComparisonCalculationUpgrades itemCalc = new ComparisonCalculationUpgrades();
                        itemCalc.ItemInstance = entry.Item;
                        itemCalc.CharacterItems = null;
                        itemCalc.Name = entry.Item.Item.Name;
                        itemCalc.Equipped = false;
                        itemCalc.OverallPoints = entry.Value / totalValue;
                        itemCalc.SubPoints = entry.ValueList.ToArray();

                        upgrades[kvp.Key].Add(itemCalc);
                    }
                }
            }
            List<string> customSubpoints = new List<string>();
            foreach (BatchCharacter batchCharacter in BatchCharacterList)
            {
                customSubpoints.Add(batchCharacter.Name);
            }
            FormUpgradeComparison.Instance.LoadData(upgrades, customSubpoints.ToArray());
            FormUpgradeComparison.Instance.Show();
            FormUpgradeComparison.Instance.BringToFront();
        }

        void _optimizer_EvaluateUpgradeProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            switch (currentOperation)
            {
                case AsyncOperation.BuildUpgradeList:
                    statusLabel.Text = string.Format("[{2}/{3}] {0}: {1}", CurrentBatchCharacter.Name, upgradeListEnumerator.Current.Item.Item.Name, batchIndex + 1, BatchCharacterList.Count);
                    statusProgressBar.Value = e.ProgressPercentage;
                    break;
                case AsyncOperation.BuildProgressiveUpgradeList:
                    statusLabel.Text = string.Format("[{4}/{5}] {1}: [{2}/{3}] {0}", CurrentBatchCharacter.Name, itemList[itemIndex].Name, batchIndex + 1, BatchCharacterList.Count, itemIndex + 1, itemList.Length);
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
                            UpdateStatusLabel();
                            statusProgressBar.Value = 0;

                            float totalValue = 0f;
                            foreach (BatchCharacter batchCharacter in BatchCharacterList)
                            {
                                if (batchCharacter.Character != null)
                                {
                                    totalValue += batchCharacter.Weight;
                                }
                            }

                            Dictionary<CharacterSlot, List<ComparisonCalculationUpgrades>> upgrades = new Dictionary<CharacterSlot, List<ComparisonCalculationUpgrades>>();

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

                                upgrades[kvp.Key] = new List<ComparisonCalculationUpgrades>();
                                foreach (UpgradeEntry entry in filtered.Values)
                                {
                                    ComparisonCalculationUpgrades itemCalc = new ComparisonCalculationUpgrades();
                                    itemCalc.ItemInstance = entry.Item;
                                    itemCalc.CharacterItems = null;
                                    itemCalc.Name = entry.Item.Item.Name;
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
                            FormUpgradeComparison.Instance.LoadData(upgrades, customSubpoints.ToArray());
                            FormUpgradeComparison.Instance.Show();
                            FormUpgradeComparison.Instance.BringToFront();
                        }
                    }
                    break;
                case AsyncOperation.BuildProgressiveUpgradeList:
                    if (e.Cancelled || e.Error != null)
                    {
                        currentOperation = AsyncOperation.None;
                        buttonCancel.Enabled = false;
                        UpdateStatusLabel();
                        statusProgressBar.Value = 0;
                        break;
                    }
                    CharacterSlot slot = Character.GetCharacterSlotByItemSlot(optimizedItemInstance.Slot);
                    Dictionary<string, UpgradeEntry> map;
                    if (!upgradeList.TryGetValue(slot, out map))
                    {
                        map = new Dictionary<string, UpgradeEntry>();
                        upgradeList[slot] = map;
                    }
                    string key = optimizedItemInstance.Id.ToString();
                    UpgradeEntry upgradeEntry;
                    if (!map.TryGetValue(key, out upgradeEntry))
                    {
                        upgradeEntry = new UpgradeEntry();
                        upgradeEntry.Item = optimizedItemInstance;
                        map[key] = upgradeEntry;
                    }
                    if (e.UpgradeValue > 0)
                    {
                        // make item restrictions based on best character
                        itemGenerator.AddItemRestrictions(e.Upgrade.CharacterItems);
                        upgradeEntry.Value += e.UpgradeValue * CurrentBatchCharacter.Weight;
                        upgradeEntry.ValueList.Add(e.UpgradeValue);
                    }
                    else
                    {
                        // make item restrictions based on best character without using the item
                        itemGenerator.AddItemRestrictions(workingCharacter);
                        upgradeEntry.ValueList.Add(0.0f);
                    }
                    // move to next character
                    do
                    {
                        batchIndex++;
                    } while (batchIndex < BatchCharacterList.Count && CurrentBatchCharacter.Character == null);
                    if (batchIndex < BatchCharacterList.Count)
                    {                        
                        // we made item restrictions, first we have to optimize character without the item
                        int _thoroughness = trackBarThoroughness.Value;
                        workingCharacter = CurrentBatchCharacter.Character.Clone();
                        // regularize character with current item restrictions
                        itemGenerator.RegularizeCharacter(workingCharacter);
                        optimizer.InitializeItemCache(workingCharacter, CurrentBatchCharacter.Model, itemGenerator);
                        optimizer.OptimizeCharacterAsync(workingCharacter, CurrentBatchCharacter.Character.CalculationToOptimize, CurrentBatchCharacter.Character.OptimizationRequirements.ToArray(), _thoroughness, true);
                    }
                    else
                    {
                        // we finished all characters for this item
                        // move to next item
                        itemIndex++;
                        if (itemIndex < itemList.Length)
                        {
                            batchIndex = 0;
                            upgradeListPhase = 0;
                            int _thoroughness = trackBarThoroughness.Value;
                            // we have to reinitialize item generator because of the restrictions we made
                            CreateBatchItemGenerator();
                            optimizer.InitializeItemCache(CurrentBatchCharacter.Character, CurrentBatchCharacter.Model, itemGenerator);
                            workingCharacter = CurrentBatchCharacter.Character;
                            optimizer.ComputeUpgradesAsync(CurrentBatchCharacter.Character, CurrentBatchCharacter.Character.CalculationToOptimize, CurrentBatchCharacter.Character.OptimizationRequirements.ToArray(), _thoroughness, itemList[itemIndex]);
                        }
                        else
                        {
                            // we're done
                            WrapUpProgressiveUpgradeList();
                        }
                    }
                    break;
            }
        }

        private void FormBatchTools_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = optimizer.IsBusy || !PromptToSaveBeforeClosing();
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
                    FormMain.Instance.AddRecentCharacter(_filePath);
                }
                dialog.Dispose();
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(_filePath))
            {
                ((BatchCharacterList)batchCharacterListBindingSource.DataSource).Save(_filePath);
                FormMain.Instance.AddRecentCharacter(_filePath);
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
                FormMain.Instance.AddRecentCharacter(_filePath);
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
                        ItemInstance item = character.Character[(CharacterSlot)slot];
                        if (item != null)
                        {
                            string id = item.Id.ToString();
                            string anyGem = id + ".*.*.*";
                            string onlyGemmedId = string.Format("{0}.{1}.{2}.{3}", item.Id, item.Gem1Id, item.Gem2Id, item.Gem3Id);
                            string anyEnchant = string.Format("{0}.{1}.{2}.{3}.*", item.Id, item.Gem1Id, item.Gem2Id, item.Gem3Id);
                            List<string> list = character.Character.AvailableItems.FindAll(x => x.StartsWith(id));
                            List<string> sublist;
                            if (list.Contains(onlyGemmedId + ".*"))
                            {
                                // available
                            }
                            else if ((sublist = list.FindAll(x => x.StartsWith(onlyGemmedId))).Count > 0)
                            {
                                if (sublist.Contains(item.GemmedId))
                                {
                                    // available
                                }
                                else
                                {
                                    // have to replace enchant
                                    string s = sublist[0];
                                    character.Character.SetEnchantBySlot((CharacterSlot)slot, Enchant.FindEnchant(int.Parse(s.Substring(s.LastIndexOf('.') + 1)), item.Slot, character.Character));
                                    character.UnsavedChanges = true;
                                }
                            }
                            else if (list.Contains(id))
                            {
                                // available
                            }
                            else if ((sublist = list.FindAll(x => x.StartsWith(anyGem))).Count > 0)
                            {
                                if (sublist.Contains(anyGem + "." + item.EnchantId))
                                {
                                    // available
                                }
                                else
                                {
                                    // have to replace enchant
                                    string s = sublist[0];
                                    character.Character.SetEnchantBySlot((CharacterSlot)slot, Enchant.FindEnchant(int.Parse(s.Substring(s.LastIndexOf('.') + 1)), item.Slot, character.Character));
                                    character.UnsavedChanges = true;
                                }
                            }
                            else if (list.Count > 0)
                            {
                                string s = list[0];
                                if (s.EndsWith("*"))
                                {
                                    s = s.TrimEnd('*') + item.EnchantId;
                                }
                                item = new ItemInstance(s);
                                character.Character[(CharacterSlot)slot] = item;
                                character.UnsavedChanges = true;
                            }
                            else
                            {
                                foreach (string s in character.Character.AvailableItems)
                                {
                                    if (s.IndexOf('.') < 0)
                                    {
                                        item = item.Clone();
                                        item.Id = int.Parse(s);
                                        if (item.Item.SocketColor1 == ItemSlot.None) item.Gem1 = null;
                                        if (item.Item.SocketColor2 == ItemSlot.None) item.Gem2 = null;
                                        if (item.Item.SocketColor3 == ItemSlot.None) item.Gem3 = null;
                                    }
                                    else
                                    {
                                        string[] slist = s.Split('.');
                                        if (slist[1] == "*")
                                        {
                                            item = item.Clone();
                                            item.Id = int.Parse(slist[0]);
                                            if (item.Item.SocketColor1 == ItemSlot.None) item.Gem1 = null;
                                            if (item.Item.SocketColor2 == ItemSlot.None) item.Gem2 = null;
                                            if (item.Item.SocketColor3 == ItemSlot.None) item.Gem3 = null;
                                        }
                                        else
                                        {
                                            item = item.Clone();
                                            item.Id = int.Parse(slist[0]);
                                            item.Gem1Id = int.Parse(slist[1]);
                                            item.Gem2Id = int.Parse(slist[2]);
                                            item.Gem3Id = int.Parse(slist[3]);
                                        }
                                    }
                                    if (item != null && item.Item.FitsInSlot((CharacterSlot)slot, character.Character))
                                    {
                                        character.Character[(CharacterSlot)slot] = item;
                                        string se = s.Substring(s.LastIndexOf('.') + 1);
                                        if (se != "*")
                                        {
                                            character.Character.SetEnchantBySlot((CharacterSlot)slot, Enchant.FindEnchant(int.Parse(se), item.Slot, character.Character));
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
                    FormMain.Instance.BatchCharacterSaved(character);
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
            optimizer.OptimizationMethod = Properties.Optimizer.Default.OptimizationMethod;
            optimizer.GreedyOptimizationMethod = Properties.Optimizer.Default.GreedyOptimizationMethod;
            buttonCancel.Enabled = true;

            batchIndex = 0;
            optimizerRound = 0;
            OptimizeCurrentBatchCharacter();
        }

        private void OptimizeCurrentBatchCharacter()
        {
            int _thoroughness = trackBarThoroughness.Value;
            if (CurrentBatchCharacter.Character != null)
            {
                if (optimizerRound == 0)
                {
                    bool _overrideRegem = checkBoxOverrideRegem.Checked;
                    bool _overrideReenchant = checkBoxOverrideReenchant.Checked;
                    optimizer.InitializeItemCache(CurrentBatchCharacter.Character, CurrentBatchCharacter.Character.AvailableItems, _overrideRegem, _overrideReenchant, Properties.Optimizer.Default.TemplateGemsEnabled, CurrentBatchCharacter.Model, false, false, false, null);
                }
                optimizer.OptimizeCharacterAsync(CurrentBatchCharacter.Character, CurrentBatchCharacter.Character.CalculationToOptimize, CurrentBatchCharacter.Character.OptimizationRequirements.ToArray(), _thoroughness, true);
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            if ((currentOperation == AsyncOperation.Optimize || currentOperation == AsyncOperation.BuildUpgradeList || currentOperation == AsyncOperation.ProgressiveOptimize || currentOperation == AsyncOperation.BuildProgressiveUpgradeList) && optimizer.IsBusy)
            {
                optimizer.CancelAsync();
            }
            if ((currentOperation == AsyncOperation.BatchOptimize || currentOperation == AsyncOperation.BuildBatchUpgradeList) && batchOptimizer.IsBusy)
            {
                batchOptimizer.CancelAsync();
            }
        }

        private void batchCharacterListBindingSource_ListChanged(object sender, ListChangedEventArgs e)
        {
            //if (e.ListChangedType == ListChangedType.ItemAdded || e.ListChangedType == ListChangedType.ItemDeleted) _unsavedChanges = true;
            UpdateStatusLabel();
        }

        private void buildUpgradeListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentOperation != AsyncOperation.None) return;

            currentOperation = AsyncOperation.BuildUpgradeList;
            optimizer.OptimizationMethod = Properties.Optimizer.Default.OptimizationMethod;
            optimizer.GreedyOptimizationMethod = Properties.Optimizer.Default.GreedyOptimizationMethod;
            buttonCancel.Enabled = true;

            batchIndex = 0;
            upgradeListPhase = 0;
            upgradeList = new Dictionary<CharacterSlot, Dictionary<string, UpgradeEntry>>();
            ComputeUpgradesCurrentBatchCharacter();
        }

        private void ComputeUpgradesCurrentBatchCharacter()
        {
            int _thoroughness = trackBarThoroughness.Value;
            bool _overrideRegem = checkBoxOverrideRegem.Checked;
            bool _overrideReenchant = checkBoxOverrideReenchant.Checked;
            if (CurrentBatchCharacter.Character != null)
            {
                optimizer.InitializeItemCache(CurrentBatchCharacter.Character, CurrentBatchCharacter.Character.AvailableItems, _overrideRegem, _overrideReenchant, Properties.Optimizer.Default.TemplateGemsEnabled, CurrentBatchCharacter.Model, false, false, false, null);
                optimizer.ComputeUpgradesAsync(CurrentBatchCharacter.Character, CurrentBatchCharacter.Character.CalculationToOptimize, CurrentBatchCharacter.Character.OptimizationRequirements.ToArray(), _thoroughness, SingleItemUpgrade);
            }
        }

        private void EvaluateUpgradeCurrentBatchCharacter(bool initializeCache)
        {
            int _thoroughness = trackBarThoroughness.Value;
            bool _overrideRegem = checkBoxOverrideRegem.Checked;
            bool _overrideReenchant = checkBoxOverrideReenchant.Checked;
            if (initializeCache) optimizer.InitializeItemCache(CurrentBatchCharacter.Character, CurrentBatchCharacter.Character.AvailableItems, _overrideRegem, _overrideReenchant, Properties.Optimizer.Default.TemplateGemsEnabled, CurrentBatchCharacter.Model, false, false, false, null);
            optimizer.EvaluateUpgradeAsync(CurrentBatchCharacter.Character, CurrentBatchCharacter.Character.CalculationToOptimize, CurrentBatchCharacter.Character.OptimizationRequirements.ToArray(), _thoroughness, upgradeListEnumerator.Current.Item);
        }

        private void bathcOptimizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int thoroughness = trackBarThoroughness.Value;
            bool overrideRegem = checkBoxOverrideRegem.Checked;
            bool overrideReenchant = checkBoxOverrideReenchant.Checked;

            List<KeyValuePair<Character, float>> batchList = BatchCharacterList.GetBatchOptimizerList();
            if (batchList[0].Key != null)
            {
                batchOptimizer = new BatchOptimizer(batchList, overrideRegem, overrideReenchant, Properties.Optimizer.Default.TemplateGemsEnabled);
                batchOptimizer.OptimizeBatchProgressChanged += new OptimizeCharacterProgressChangedEventHandler(batchOptimizer_OptimizeBatchProgressChanged);
                batchOptimizer.OptimizeBatchCompleted += new OptimizeBatchCompletedEventHandler(batchOptimizer_OptimizeBatchCompleted);

                currentOperation = AsyncOperation.BatchOptimize;
                buttonCancel.Enabled = true;

                batchOptimizer.OptimizeCharacterAsync(thoroughness);
            }
        }

        void batchOptimizer_OptimizeBatchCompleted(object sender, OptimizeBatchCompletedEventArgs e)
        {
            batchOptimizer.OptimizeBatchProgressChanged -= new OptimizeCharacterProgressChangedEventHandler(batchOptimizer_OptimizeBatchProgressChanged);
            batchOptimizer.OptimizeBatchCompleted -= new OptimizeBatchCompletedEventHandler(batchOptimizer_OptimizeBatchCompleted);

            if (e.Cancelled || e.Error != null)
            {
                currentOperation = AsyncOperation.None;
                buttonCancel.Enabled = false;
                UpdateStatusLabel();
                statusProgressBar.Value = 0;
                return;
            }
            for (int i = 0; i < BatchCharacterList.Count; i++)
            {
                Character _character = BatchCharacterList[i].Character;
                Character bestCharacter = e.OptimizedBatch.Character[i];

                _character.SetItems(bestCharacter);

                //BatchCharacterList[i].UnsavedChanges = true;
                //CurrentBatchCharacter.NewScore = e.OptimizedCharacterValue;
                //BatchCharacterList[i].NewScore = ItemInstanceOptimizer.GetOptimizationValue(_character, CurrentBatchCharacter.Model); // on item change always evaluate with equipped gear first (needed by mage module to store incremental data)
            }
            currentOperation = AsyncOperation.None;
            buttonCancel.Enabled = false;
            UpdateStatusLabel();
            statusProgressBar.Value = 0;
        }

        void batchOptimizer_OptimizeBatchProgressChanged(object sender, OptimizeCharacterProgressChangedEventArgs e)
        {
            statusLabel.Text = string.Format("{0} / {1}", BatchCharacterList.Score, e.BestValue);
            statusProgressBar.Value = e.ProgressPercentage;
        }

        private void buildBatchUpgradeListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int thoroughness = trackBarThoroughness.Value;
            bool overrideRegem = checkBoxOverrideRegem.Checked;
            bool overrideReenchant = checkBoxOverrideReenchant.Checked;

            List<KeyValuePair<Character, float>> batchList = BatchCharacterList.GetBatchOptimizerList();
            if (batchList[0].Key != null)
            {
                batchOptimizer = new BatchOptimizer(batchList, overrideRegem, overrideReenchant, Properties.Optimizer.Default.TemplateGemsEnabled);
                batchOptimizer.ComputeUpgradesProgressChanged += new ComputeUpgradesProgressChangedEventHandler(batchOptimizer_ComputeUpgradesProgressChanged);
                batchOptimizer.ComputeUpgradesCompleted += new ComputeUpgradesCompletedEventHandler(batchOptimizer_ComputeUpgradesCompleted);

                currentOperation = AsyncOperation.BuildBatchUpgradeList;
                buttonCancel.Enabled = true;

                batchOptimizer.ComputeUpgradesAsync(thoroughness, SingleItemUpgrade);
            }
        }

        void batchOptimizer_ComputeUpgradesCompleted(object sender, ComputeUpgradesCompletedEventArgs e)
        {
            batchOptimizer.ComputeUpgradesProgressChanged -= new ComputeUpgradesProgressChangedEventHandler(batchOptimizer_ComputeUpgradesProgressChanged);
            batchOptimizer.ComputeUpgradesCompleted -= new ComputeUpgradesCompletedEventHandler(batchOptimizer_ComputeUpgradesCompleted);
            if (!e.Cancelled)
            {
                FormUpgradeComparison.Instance.LoadData(e.Upgrades, null);
                FormUpgradeComparison.Instance.Show();
                FormUpgradeComparison.Instance.BringToFront();
            }
            currentOperation = AsyncOperation.None;
            buttonCancel.Enabled = false;
            UpdateStatusLabel();
            statusProgressBar.Value = 0;
        }

        void batchOptimizer_ComputeUpgradesProgressChanged(object sender, ComputeUpgradesProgressChangedEventArgs e)
        {
            statusLabel.Text = e.CurrentItem;
            statusProgressBar.Value = e.ProgressPercentage;
        }

        private void progressiveOptimizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentOperation != AsyncOperation.None) return;

            currentOperation = AsyncOperation.ProgressiveOptimize;
            optimizer.OptimizationMethod = Properties.Optimizer.Default.OptimizationMethod;
            optimizer.GreedyOptimizationMethod = Properties.Optimizer.Default.GreedyOptimizationMethod;
            buttonCancel.Enabled = true;

            batchIndex = 0;
            optimizerRound = 0;

            int _thoroughness = trackBarThoroughness.Value;
            CreateBatchItemGenerator();
            optimizer.InitializeItemCache(CurrentBatchCharacter.Character, CurrentBatchCharacter.Model, itemGenerator);
            while (CurrentBatchCharacter != null && CurrentBatchCharacter.Locked)
            {
                batchIndex++;
            }
            if (CurrentBatchCharacter != null)
            {
                optimizer.OptimizeCharacterAsync(CurrentBatchCharacter.Character, CurrentBatchCharacter.Character.CalculationToOptimize, CurrentBatchCharacter.Character.OptimizationRequirements.ToArray(), _thoroughness, true);
            }
            else
            {
                buttonCancel.Enabled = false;
                currentOperation = AsyncOperation.None;
            }
        }

        private void CreateBatchItemGenerator()
        {
            bool _overrideRegem = checkBoxOverrideRegem.Checked;
            bool _overrideReenchant = checkBoxOverrideReenchant.Checked;
            Character[] characterList = new Character[BatchCharacterList.Count];
            CalculationsBase[] modelList = new CalculationsBase[BatchCharacterList.Count];
            for (int i = 0; i < BatchCharacterList.Count; i++)
            {
                characterList[i] = BatchCharacterList[i].Character;
                modelList[i] = BatchCharacterList[i].Model;
            }
            itemGenerator = new AvailableItemGenerator(BatchCharacterList[0].Character.AvailableItems, optimizer.GreedyOptimizationMethod != GreedyOptimizationMethod.AllCombinations, Properties.Optimizer.Default.TemplateGemsEnabled, _overrideRegem, _overrideReenchant, false, characterList, modelList);
            // create item restrictions for locked characters
            for (int i = 0; i < BatchCharacterList.Count; i++)
            {
                if (BatchCharacterList[i].Locked)
                {
                    itemGenerator.AddItemRestrictions(BatchCharacterList[i].Character);
                }
            }
        }

        private void buildProgressiveUpgradeListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentOperation != AsyncOperation.None || BatchCharacterList[0].Character == null) return;

            currentOperation = AsyncOperation.BuildProgressiveUpgradeList;
            optimizer.OptimizationMethod = Properties.Optimizer.Default.OptimizationMethod;
            optimizer.GreedyOptimizationMethod = Properties.Optimizer.Default.GreedyOptimizationMethod;
            buttonCancel.Enabled = true;

            batchIndex = 0;
            optimizerRound = 0;
            upgradeListPhase = 0;

            int _thoroughness = trackBarThoroughness.Value;
            Character character = CurrentBatchCharacter.Character;
            Dictionary<int, Item> itemById = new Dictionary<int, Item>();
            Item single = SingleItemUpgrade;
            if (single != null)
            {
                itemList = new Item[] { single };
            }
            else
            {
                for (int i = 0; i < BatchCharacterList.Count; i++)
                {
                    CalculationsBase model = BatchCharacterList[i].Model;
                    Item[] items = ItemCache.GetRelevantItems(model, BatchCharacterList[i].Character.Race);
                    foreach (Item item in items)
                    {
                        if (item != null && !item.IsGem)
                        {
                            itemById[item.Id] = item;
                        }
                    }
                }
                itemList = new List<Item>(itemById.Values).ToArray();
            }
            CreateBatchItemGenerator();
            optimizer.InitializeItemCache(CurrentBatchCharacter.Character, CurrentBatchCharacter.Model, itemGenerator);
            itemIndex = 0;
            upgradeList = new Dictionary<CharacterSlot, Dictionary<string, UpgradeEntry>>();
            workingCharacter = character;
            optimizer.ComputeUpgradesAsync(CurrentBatchCharacter.Character, CurrentBatchCharacter.Character.CalculationToOptimize, CurrentBatchCharacter.Character.OptimizationRequirements.ToArray(), _thoroughness, itemList[itemIndex]);
        }

        private void dataGridView_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count == 1)
            {
                buttonUp.Enabled = dataGridView.SelectedRows[0].Index > 0;
                buttonDown.Enabled = dataGridView.SelectedRows[0].Index < BatchCharacterList.Count - 1;
            }
            else
            {
                buttonUp.Enabled = false;
                buttonDown.Enabled = false;
            }
        }

        private void buttonUp_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count == 1)
            {
                int index = dataGridView.SelectedRows[0].Index;
                if (index > 0)
                {
                    BatchCharacter b = BatchCharacterList[index - 1];
                    BatchCharacterList.RemoveAt(index - 1);
                    BatchCharacterList.Insert(index, b);
                    dataGridView.Rows[index].Selected = false;
                    dataGridView.Rows[index - 1].Selected = true;
                    dataGridView_SelectionChanged(null, EventArgs.Empty);
                }
            }
        }

        private void buttonDown_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count == 1)
            {
                int index = dataGridView.SelectedRows[0].Index;
                if (index < BatchCharacterList.Count - 1)
                {
                    BatchCharacter b = BatchCharacterList[index];
                    BatchCharacterList.RemoveAt(index);
                    dataGridView.Rows[index].Selected = false;
                    BatchCharacterList.Insert(index + 1, b);
                    dataGridView.Rows[index + 1].Selected = true;
                    dataGridView_SelectionChanged(null, EventArgs.Empty);
                }
            }
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

                    score = ItemInstanceOptimizer.GetOptimizationValue(Character, Model);

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
                    character.CalculationsInvalidated += new EventHandler(character_CalculationsInvalidated);
                }
                return character;
            }
        }

        void character_CalculationsInvalidated(object sender, EventArgs e)
        {
            unsavedChanges = true;
            newScore = ItemInstanceOptimizer.GetOptimizationValue(Character, Model);

            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("Name"));
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("NewScore"));            
        }

        private CalculationsBase model;
        [XmlIgnore]
        public CalculationsBase Model
        {
            get
            {
                if (model == null || model.Name != Character.CurrentModel)
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

        public bool Locked { get; set; }

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

        public List<KeyValuePair<Character, float>> GetBatchOptimizerList()
        {
            List<KeyValuePair<Character, float>> list = new List<KeyValuePair<Character, float>>();
            foreach (BatchCharacter batchCharacter in this)
            {
                list.Add(new KeyValuePair<Character, float>(batchCharacter.Character, batchCharacter.Weight));
            }
            return list;
        }

        public float NewScore
        {
            get
            {
                float score = 0.0f;
                foreach (BatchCharacter character in this)
                {
                    score += character.Weight * (character.NewScore ?? character.Score);
                }
                return score;
            }
        }

        public float Score
        {
            get
            {
                float score = 0.0f;
                foreach (BatchCharacter character in this)
                {
                    score += character.Weight * character.Score;
                }
                return score;
            }
        }
    }
}
