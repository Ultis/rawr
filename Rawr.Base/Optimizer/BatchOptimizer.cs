using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Threading;

namespace Rawr.Optimizer
{
    public delegate void OptimizeBatchCompletedEventHandler(object sender, OptimizeBatchCompletedEventArgs e);

    public class OptimizeBatchCompletedEventArgs : AsyncCompletedEventArgs
    {
        private BatchIndividual optimizedBatch;
        private BatchValuation optimizedBatchValuation;
        private float optimizedBatchValue;

        public OptimizeBatchCompletedEventArgs(BatchIndividual optimizedBatch, BatchValuation optimizedBatchValuation, float optimizedBatchValue, Exception error, bool cancelled)
            : base(error, cancelled, null)
        {
            this.optimizedBatch = optimizedBatch;
            this.optimizedBatchValue = optimizedBatchValue;
            this.optimizedBatchValuation = optimizedBatchValuation;
        }

        public BatchIndividual OptimizedBatch
        {
            get
            {
                RaiseExceptionIfNecessary();
                return optimizedBatch;
            }
        }

        public BatchValuation OptimizedBatchValuation
        {
            get
            {
                RaiseExceptionIfNecessary();
                return optimizedBatchValuation;
            }
        }

        public float OptimizedBatchValue
        {
            get
            {
                RaiseExceptionIfNecessary();
                return optimizedBatchValue;
            }
        }
    }

    public class BatchValuation
    {
        public List<float> OptimizedBatchValue;
        public float OptimizedValue;
    }

    public class BatchIndividual
    {
        public object[] Items;
        public ItemInstance[] AvailableItem;
        public int[] UsedCount;
        public List<Character> Character;

        private const int characterSlots = 19;

        public BatchIndividual(object[] items, int itemCount, Dictionary<ItemAvailabilityInformation, int> indexFromId, ItemAvailabilityInformation upgradeItem, List<Character> batchList)
        {
            Items = items;
            AvailableItem = new ItemInstance[itemCount + 1];
            UsedCount = new int[itemCount + 1];
            for (int i = 0; i <= itemCount; i++)
            {
                ItemInstance itemInstance = items[i] as ItemInstance;
                AvailableItem[i] = itemInstance;
            }
            int charCount = (items.Length - itemCount - 1) / characterSlots;
            Character = new List<Character>();
            for (int c = 0; c < charCount; c++)
            {
                ItemInstance[] slotItems = new ItemInstance[characterSlots];
                for (int i = 0; i < characterSlots; i++)
                {
                    ItemAvailabilityInformation item = items[itemCount + 1 + c * characterSlots + i] as ItemAvailabilityInformation;
                    if (item != null)
                    {
                        if (item == upgradeItem)
                        {
                            slotItems[i] = AvailableItem[itemCount];
                            UsedCount[itemCount]++;
                        }
                        else
                        {
                            int index = indexFromId[item];
                            slotItems[i] = AvailableItem[index];
                            UsedCount[index]++;
                        }
                    }
                }
                Character _character = batchList[c];
                Character character = new Character(_character.Name, _character.Realm, _character.Region, _character.Race, _character.BossOptions, slotItems, _character.ActiveBuffs, _character.CurrentModel);
                character.CalculationOptions = _character.CalculationOptions;
                character.Class = _character.Class;
                character.AssignAllTalentsFromCharacter(_character, false);
                Character.Add(character);
            }
        }
    }

    public class BatchOptimizer : OptimizerBase<object, BatchIndividual, BatchValuation>
    {
        private List<Character> batchList;
        private List<CalculationsBase> modelList;
        private List<float> weightList;

        private const int characterSlots = 19;

        BatchIndividual startIndividual;

        private class UniqueItemValidator : OptimizerRangeValidatorBase<object>
        {
            public override bool IsValid(object[] items)
            {
                if (items[StartSlot] != null && items[EndSlot] != null)
                {
                    Item itema = (Item)items[StartSlot];
                    Item itemb = (Item)items[EndSlot];
                    return !(itema.Unique && (itema.Id == itemb.Id || (itema.UniqueId != null && itema.UniqueId.Contains(itemb.Id))));
                }
                else
                {
                    return true;
                }
            }
        }

        public BatchOptimizer(List<KeyValuePair<Character, float>> batchList, bool overrideRegem, bool overrideReenchant, bool overrideReforge, bool templateGemsEnabled)
        {
            if (batchList == null || batchList.Count == 0 || batchList[0].Key == null) throw new ArgumentException("Batch list must have at least one element.");
            this.batchList = new List<Character>();
            this.modelList = new List<CalculationsBase>();
            this.weightList = new List<float>();
            foreach (KeyValuePair<Character, float> kvp in batchList)
            {
                this.batchList.Add(kvp.Key.Clone());
                this.modelList.Add(Calculations.GetModel(kvp.Key.CurrentModel));
                this.weightList.Add(kvp.Value);
            }

            optimizeCharacterProgressChangedDelegate = new SendOrPostCallback(PrivateOptimizeBatchProgressChanged);
            optimizeBatchCompletedDelegate = new SendOrPostCallback(PrivateOptimizeBatchCompleted);
            computeUpgradesProgressChangedDelegate = new SendOrPostCallback(PrivateComputeUpgradesProgressChanged);
            computeUpgradesCompletedDelegate = new SendOrPostCallback(PrivateComputeUpgradesCompleted);
            evaluateUpgradeProgressChangedDelegate = new SendOrPostCallback(PrivateEvaluateUpgradeProgressChanged);
            evaluateUpgradeCompletedDelegate = new SendOrPostCallback(PrivateEvaluateUpgradeCompleted);

            InitializeItemCache(batchList[0].Key.AvailableItems, overrideRegem, overrideReenchant, overrideReforge, templateGemsEnabled);

            if (Properties.GeneralSettings.Default.UseMultithreading)
            {
                ThreadPoolValuation = true;
                for (int i = 0; i < modelList.Count; i++)
                {
                    if (!modelList[i].SupportsMultithreading)
                    {
                        ThreadPoolValuation = false;
                        break;
                    }
                }
            }
        }

        private void InitializeItemCache(List<string> availableItems, bool overrideRegem, bool overrideReenchant, bool overrideReforge, bool templateGemsEnabled)
        {
            PopulateAvailableIds(availableItems, templateGemsEnabled, overrideRegem, overrideReenchant, overrideReforge);
        }

        private enum OptimizationOperation
        {
            OptimizeCharacter,
            ComputeUpgrades,
            EvaluateUpgrade
        }

        private OptimizationOperation currentOperation;

        #region Asynchronous Pattern Implementation
        private void PrivateOptimizeBatchProgressChanged(object state)
        {
            OnOptimizeBatchProgressChanged(state as OptimizeCharacterProgressChangedEventArgs);
        }

        protected void OnOptimizeBatchProgressChanged(OptimizeCharacterProgressChangedEventArgs e)
        {
            if (OptimizeBatchProgressChanged != null)
            {
                OptimizeBatchProgressChanged(this, e);
            }
        }

        private void PrivateOptimizeBatchCompleted(object state)
        {
            isBusy = false;
            cancellationPending = false;
            OnOptimizeBatchCompleted(state as OptimizeBatchCompletedEventArgs);
        }

        protected void OnOptimizeBatchCompleted(OptimizeBatchCompletedEventArgs e)
        {
            if (OptimizeBatchCompleted != null)
            {
                OptimizeBatchCompleted(this, e);
            }
        }

        private void PrivateComputeUpgradesProgressChanged(object state)
        {
            OnComputeUpgradesProgressChanged(state as ComputeUpgradesProgressChangedEventArgs);
        }

        protected void OnComputeUpgradesProgressChanged(ComputeUpgradesProgressChangedEventArgs e)
        {
            if (ComputeUpgradesProgressChanged != null)
            {
                ComputeUpgradesProgressChanged(this, e);
            }
        }

        private void PrivateComputeUpgradesCompleted(object state)
        {
            isBusy = false;
            cancellationPending = false;
            OnComputeUpgradesCompleted(state as ComputeUpgradesCompletedEventArgs);
        }

        protected void OnComputeUpgradesCompleted(ComputeUpgradesCompletedEventArgs e)
        {
            if (ComputeUpgradesCompleted != null)
            {
                ComputeUpgradesCompleted(this, e);
            }
        }

        private void PrivateEvaluateUpgradeProgressChanged(object state)
        {
            OnEvaluateUpgradeProgressChanged(state as ProgressChangedEventArgs);
        }

        protected void OnEvaluateUpgradeProgressChanged(ProgressChangedEventArgs e)
        {
            if (EvaluateUpgradeProgressChanged != null)
            {
                EvaluateUpgradeProgressChanged(this, e);
            }
        }

        private void PrivateEvaluateUpgradeCompleted(object state)
        {
            isBusy = false;
            cancellationPending = false;
            OnEvaluateUpgradeCompleted(state as EvaluateUpgradeCompletedEventArgs);
        }

        protected void OnEvaluateUpgradeCompleted(EvaluateUpgradeCompletedEventArgs e)
        {
            if (EvaluateUpgradeCompleted != null)
            {
                EvaluateUpgradeCompleted(this, e);
            }
        }

        private bool isBusy;

        public bool IsBusy
        {
            get
            {
                return isBusy;
            }
        }

        private AsyncOperation asyncOperation;

        public event OptimizeBatchCompletedEventHandler OptimizeBatchCompleted;
        public event OptimizeCharacterProgressChangedEventHandler OptimizeBatchProgressChanged;
        public event ComputeUpgradesProgressChangedEventHandler ComputeUpgradesProgressChanged;
        public event ComputeUpgradesCompletedEventHandler ComputeUpgradesCompleted;
        public event ProgressChangedEventHandler EvaluateUpgradeProgressChanged;
        public event EvaluateUpgradeCompletedEventHandler EvaluateUpgradeCompleted;

        private SendOrPostCallback optimizeCharacterProgressChangedDelegate;
        private SendOrPostCallback optimizeBatchCompletedDelegate;
        private SendOrPostCallback computeUpgradesProgressChangedDelegate;
        private SendOrPostCallback computeUpgradesCompletedDelegate;
        private SendOrPostCallback evaluateUpgradeProgressChangedDelegate;
        private SendOrPostCallback evaluateUpgradeCompletedDelegate;

        public void OptimizeCharacterAsync(int thoroughness)
        {
            if (isBusy) throw new InvalidOperationException("Optimizer is working on another operation.");
            isBusy = true;
            cancellationPending = false;
            asyncOperation = AsyncOperationManager.CreateOperation(null);
            ThreadPool.QueueUserWorkItem(delegate
            {
                OptimizeBatchThreadStart(thoroughness);
            });
        }

        private void OptimizeBatchThreadStart(int thoroughness)
        {
            Exception error = null;
            BatchIndividual optimizedBatch = null;
            BatchValuation optimizedValuation = null;
            float optimizedBatchValue = 0.0f;
            try
            {
                optimizedBatch = PrivateOptimizeBatch(thoroughness, out optimizedValuation, out error);
                if (optimizedBatch != null)
                {
                    optimizedBatchValue = GetOptimizationValue(optimizedBatch);
                }
            }
            catch (Exception ex)
            {
                error = ex;
            }
            asyncOperation.PostOperationCompleted(optimizeBatchCompletedDelegate, new OptimizeBatchCompletedEventArgs(optimizedBatch, optimizedValuation, optimizedBatchValue, error, cancellationPending));
        }

        public void ComputeUpgradesAsync(int thoroughness)
        {
            ComputeUpgradesAsync(thoroughness, new SuffixItem());
        }

        public void ComputeUpgradesAsync(int thoroughness, SuffixItem singleItemUpgrades)
        {
            if (isBusy) throw new InvalidOperationException("Optimizer is working on another operation.");
            isBusy = true;
            cancellationPending = false;
            asyncOperation = AsyncOperationManager.CreateOperation(null);
            ThreadPool.QueueUserWorkItem(delegate
            {
                ComputeUpgradesThreadStart(thoroughness, singleItemUpgrades);
            });
        }

        private void ComputeUpgradesThreadStart(int thoroughness, SuffixItem singleItemUpgrades)
        {
            Exception error = null;
            Dictionary<CharacterSlot, List<ComparisonCalculationUpgrades>> upgrades = null;
            try
            {
                upgrades = PrivateComputeUpgrades(thoroughness, singleItemUpgrades, out error);
            }
            catch (Exception ex)
            {
                error = ex;
            }
            asyncOperation.PostOperationCompleted(computeUpgradesCompletedDelegate, new ComputeUpgradesCompletedEventArgs(upgrades, error, cancellationPending));
        }

        public void EvaluateUpgradeAsync(int thoroughness, ItemInstance upgrade)
        {
            if (isBusy) throw new InvalidOperationException("Optimizer is working on another operation.");
            isBusy = true;
            cancellationPending = false;
            asyncOperation = AsyncOperationManager.CreateOperation(null);
            ThreadPool.QueueUserWorkItem(delegate
            {
                EvaluateUpgradeThreadStart(thoroughness, upgrade);
            });
        }

        private void EvaluateUpgradeThreadStart(int thoroughness, ItemInstance upgrade)
        {
            Exception error = null;
            float upgradeValue = 0f;
            try
            {
                upgradeValue = PrivateEvaluateUpgrade(thoroughness, upgrade, out error);
            }
            catch (Exception ex)
            {
                error = ex;
            }
            asyncOperation.PostOperationCompleted(evaluateUpgradeCompletedDelegate, new EvaluateUpgradeCompletedEventArgs(upgradeValue, null, error, cancellationPending));
        }
        #endregion

        protected override void ReportProgress(int progressPercentage, float bestValue)
        {
            if (!cancellationPending && asyncOperation != null)
            {
                switch (currentOperation)
                {
                    case OptimizationOperation.OptimizeCharacter:
                        asyncOperation.Post(optimizeCharacterProgressChangedDelegate, new OptimizeCharacterProgressChangedEventArgs(progressPercentage, bestValue));
                        break;
                    case OptimizationOperation.ComputeUpgrades:
                        asyncOperation.Post(computeUpgradesProgressChangedDelegate, new ComputeUpgradesProgressChangedEventArgs(itemProgressPercentage, progressPercentage, currentItem));
                        break;
                    case OptimizationOperation.EvaluateUpgrade:
                        asyncOperation.Post(evaluateUpgradeProgressChangedDelegate, new ProgressChangedEventArgs(progressPercentage, null));
                        break;
                }
            }
        }

        public BatchIndividual OptimizeCharacter(int thoroughness, out BatchValuation bestValuation)
        {
            if (isBusy) throw new InvalidOperationException("Optimizer is working on another operation.");
            isBusy = true;
            cancellationPending = false;
            asyncOperation = null;
            Exception error;
            BatchIndividual optimizedBatch = PrivateOptimizeBatch(thoroughness, out bestValuation, out error);
            if (error != null) throw error;
            isBusy = false;
            return optimizedBatch;
        }

        private BatchIndividual PrivateOptimizeBatch(int thoroughness, out BatchValuation bestValuation, out Exception error)
        {
            if (!itemCacheInitialized) throw new InvalidOperationException("Optimization item cache was not initialized.");
            error = null;
            //_character = character;
            //model = Calculations.GetModel(_character.CurrentModel);
            _thoroughness = thoroughness;

            currentOperation = OptimizationOperation.OptimizeCharacter;
            BatchIndividual optimizedCharacter = null;
            float bestValue = 0.0f;
            bestValuation = null;
            slotItems[itemList.Count].Clear();
            upgradeItems = null;

            try
            {
                if (_thoroughness == 1)
                {
                    // if we just start from current character and look for direct upgrades
                    // then we have to deal with items that are currently equipped, but are not
                    // currently available
                    //MarkEquippedItemsAsValid(_character);
                }

                bool injected;
                optimizedCharacter = Optimize(startIndividual, 0.0f, out bestValue, out bestValuation, out injected);
            }
            catch (Exception ex)
            {
                error = ex;
            }

            ReportProgress(100, bestValue);
            return optimizedCharacter;
        }

        public Dictionary<CharacterSlot, List<ComparisonCalculationUpgrades>> ComputeUpgrades(int thoroughness, SuffixItem singleItemUpgrades)
        {
            if (isBusy) throw new InvalidOperationException("Optimizer is working on another operation.");
            isBusy = true;
            cancellationPending = false;
            asyncOperation = null;
            Exception error;
            Dictionary<CharacterSlot, List<ComparisonCalculationUpgrades>> upgrades = PrivateComputeUpgrades(thoroughness, singleItemUpgrades, out error);
            if (error != null) throw error;
            isBusy = false;
            return upgrades;
        }

        private int itemProgressPercentage = 0;
        private string currentItem = "";

        private Dictionary<CharacterSlot, List<ComparisonCalculationUpgrades>> PrivateComputeUpgrades(int thoroughness, SuffixItem singleItemUpgrades, out Exception error)
        {
            if (!itemCacheInitialized) throw new InvalidOperationException("Optimization item cache was not initialized.");
            error = null;
            //_character = character;
            //model = Calculations.GetModel(_character.CurrentModel);
            _thoroughness = thoroughness;

            currentOperation = OptimizationOperation.ComputeUpgrades;
            Dictionary<CharacterSlot, List<ComparisonCalculationUpgrades>> upgrades = null;
            try
            {
                // make equipped gear/enchant valid
                //MarkEquippedItemsAsValid(_character);

                upgrades = new Dictionary<CharacterSlot, List<ComparisonCalculationUpgrades>>();

                SuffixItem[] items = ItemCache.GetRelevantSuffixItems(modelList[0], batchList[0]);
                CharacterSlot[] slots = new CharacterSlot[] { CharacterSlot.Back, CharacterSlot.Chest, CharacterSlot.Feet, CharacterSlot.Finger1, CharacterSlot.Hands, CharacterSlot.Head, CharacterSlot.Legs, CharacterSlot.MainHand, CharacterSlot.Neck, CharacterSlot.OffHand, CharacterSlot.Projectile, CharacterSlot.ProjectileBag, CharacterSlot.Ranged, CharacterSlot.Shoulders, CharacterSlot.Trinket1, CharacterSlot.Waist, CharacterSlot.Wrist };
                foreach (CharacterSlot slot in slots)
                    upgrades[slot] = new List<ComparisonCalculationUpgrades>();

                slotItems[itemList.Count].Clear();
                upgradeItems = null;

                float baseValue = GetOptimizationValue(startIndividual);

                /*Dictionary<string, SuffixItem> itemById = new Dictionary<string, SuffixItem>();
                foreach (SuffixItem item in items)
                {
                    itemById[item.SuffixId] = item;
                }*/

                if (singleItemUpgrades.Item != null)
                {
                    items = new SuffixItem[] { singleItemUpgrades };
                }
                /*else
                {
                    items = new List<Item>(itemById.Values).ToArray();
                }*/

                for (int i = 0; i < items.Length; i++)
                {
                    SuffixItem item = items[i];
                    currentItem = item.Name;
                    itemProgressPercentage = (int)Math.Round((float)i / ((float)items.Length / 100f));
                    if (resumeEvent != null)
                    {
                        resumeEvent.WaitOne();
                    }
                    if (cancellationPending)
                    {
                        return null;
                    }
                    if (Array.IndexOf(slots, Character.GetCharacterSlotByItemSlot(item.Item.Slot)) >= 0)
                    {
                        ReportProgress(0, 0);
                        upgradeItems = itemGenerator.GetPossibleGemmedItemsForItem(item.Item, item.RandomSuffixId, item.SuffixId);
                        PopulateUpgradeItems(item.Item);
                        float best = -10000000f;
                        BatchValuation bestCalculations;
                        BatchIndividual bestCharacter;
                        bool injected;
                        object[] genes = (object[])startIndividual.Items.Clone();
                        genes[itemList.Count] = upgradeItems[0];
                        BatchIndividual batch = GenerateIndividual(genes, true, null);
                        bestCharacter = Optimize(batch, GetOptimizationValue(batch), out best, out bestCalculations, out injected);
                        if (best > baseValue)
                        {
                            ItemInstance bestItem = bestCharacter.AvailableItem[itemList.Count];
                            ComparisonCalculationUpgrades itemCalc = new ComparisonCalculationUpgrades();
                            itemCalc.ItemInstance = bestItem;
                            //itemCalc.Character = bestCharacter;
                            itemCalc.Name = bestItem.Name;
                            itemCalc.Equipped = false;
                            //itemCalc.OverallPoints = bestCalculations.OverallPoints - baseCalculations.OverallPoints;
                            //float[] subPoints = new float[bestCalculations.SubPoints.Length];
                            //for (int j = 0; j < bestCalculations.SubPoints.Length; j++)
                            //{
                            //    subPoints[j] = bestCalculations.SubPoints[j] - baseCalculations.SubPoints[j];
                            //}
                            //itemCalc.SubPoints = subPoints;
                            itemCalc.OverallPoints = best - baseValue;

                            foreach (CharacterSlot slot in slots)
                            {
                                if (item.Item.FitsInSlot(slot, batchList[0]))
                                {
                                    upgrades[slot].Add(itemCalc);
                                }
                            }
                        }
                        RemoveUpgradeItems(item.Item);
                    }
                }
            }
            catch (Exception ex)
            {
                error = ex;
            }

            ReportProgress(100, 0f);
            return upgrades;
        }

        public float EvaluateUpgrade(int thoroughness, ItemInstance upgrade)
        {
            if (isBusy) throw new InvalidOperationException("Optimizer is working on another operation.");
            isBusy = true;
            cancellationPending = false;
            asyncOperation = null;
            Exception error;
            float upgradeValue = PrivateEvaluateUpgrade(thoroughness, upgrade, out error);
            if (error != null) throw error;
            isBusy = false;
            return upgradeValue;
        }

        private float PrivateEvaluateUpgrade(int thoroughness, ItemInstance upgrade, out Exception error)
        {
            if (!itemCacheInitialized) throw new InvalidOperationException("Optimization item cache was not initialized.");
            error = null;
            //_character = character;
            //model = Calculations.GetModel(_character.CurrentModel);
            _thoroughness = thoroughness;

            currentOperation = OptimizationOperation.EvaluateUpgrade;
            //Character saveCharacter = _character;
            float upgradeValue = 0f;
            try
            {
                // make equipped gear/enchant valid
                // this is currently only called after calculate upgrades already marks items as valid, but we might have to do this here also if things change
                // MarkEquippedItemsAsValid(_character);

                float baseValue = GetOptimizationValue(startIndividual);

                ItemInstance item = upgrade;
                upgradeItems = new List<ItemInstance>() { item };
                PopulateUpgradeItems(item.Item);
                float best = -10000000f;
                BatchValuation bestCalculations;
                BatchIndividual bestCharacter;
                bool injected;
                object[] genes = (object[])startIndividual.Items.Clone();
                genes[itemList.Count] = upgradeItems[0];
                BatchIndividual batch = GenerateIndividual(genes, true, null);
                bestCharacter = Optimize(batch, GetOptimizationValue(batch), out best, out bestCalculations, out injected);
                RemoveUpgradeItems(item.Item);
                upgradeValue = best - baseValue;
            }
            catch (Exception ex)
            {
                error = ex;
            }

            ReportProgress(100, 0f);
            return upgradeValue;
        }

        private bool itemCacheInitialized;

        List<ItemInstance> upgradeItems;
        List<ItemAvailabilityInformation> itemList;
        List<object>[] slotItemList;
        Dictionary<ItemAvailabilityInformation, int> indexFromId;

        AvailableItemGenerator itemGenerator;

        private void PopulateUpgradeItems(Item item)
        {
            slotItems[itemList.Count] = upgradeItems.ConvertAll(itemInstance => (object)itemInstance);
            for (int i = 0; i < characterSlots; i++)
            {
                if (item.FitsInSlot((CharacterSlot)i, batchList[0]))
                {
                    for (int c = 0; c < batchList.Count; c++)
                    {
                        slotItems[itemList.Count + 1 + c * characterSlots + i].Add(item);
                    }
                }
            }
        }

        private void RemoveUpgradeItems(Item item)
        {
            upgradeItems = null;
            slotItems[itemList.Count].Clear();
            for (int i = 0; i < characterSlots; i++)
            {
                if (item.FitsInSlot((CharacterSlot)i, batchList[0]))
                {
                    for (int c = 0; c < batchList.Count; c++)
                    {
                        List<object> list = slotItems[itemList.Count + 1 + c * characterSlots + i];
                        list.RemoveAt(list.Count - 1);
                    }
                }
            }
        }

        private void PopulateAvailableIds(List<string> availableItems, bool templateGemsEnabled, bool overrideRegem, bool overrideReenchant, bool overrideReforge)
        {
            itemGenerator = new AvailableItemGenerator(availableItems, false, templateGemsEnabled, overrideRegem, overrideReenchant, overrideReforge, false, batchList.ToArray(), modelList.ToArray());
            //List<ItemInstance>[] slotList = itemGenerator.SlotItems;
            slotItemList = new List<object>[characterSlots];

            itemList = itemGenerator.RawItems;
            indexFromId = new Dictionary<ItemAvailabilityInformation, int>();         
            for (int i = 0; i < characterSlots; i++)
            {
                slotItemList[i] = itemGenerator.SlotRawItems[i].ConvertAll(availabilityInfo => (object)availabilityInfo);
            }
            for (int i = 0; i < itemList.Count; i++)
            {
                indexFromId[itemList[i]] = i;
            }

            slotCount = itemList.Count + 1 + batchList.Count * characterSlots;

            slotItems = new List<object>[slotCount];

            for (int i = 0; i < itemList.Count; i++)
            {
                slotItems[i] = itemList[i].ItemList.ConvertAll(item => (object)item);
            }
            slotItems[itemList.Count] = new List<object>();
            for (int c = 0; c < batchList.Count; c++)
            {
                for (int i = 0; i < characterSlots; i++)
                {
                    slotItems[itemList.Count + 1 + c * characterSlots + i] = slotItemList[i];
                }
            }

            validators = new List<OptimizerRangeValidatorBase<object>>();

            for (int c = 0; c < batchList.Count; c++)
            {
                int offset = itemList.Count + 1 + c * characterSlots;
                validators.Add(new UniqueItemValidator() { StartSlot = offset + (int)CharacterSlot.Finger1, EndSlot = offset + (int)CharacterSlot.Finger2 });
                validators.Add(new UniqueItemValidator() { StartSlot = offset + (int)CharacterSlot.Trinket1, EndSlot = offset + (int)CharacterSlot.Trinket2 });
                validators.Add(new UniqueItemValidator() { StartSlot = offset + (int)CharacterSlot.MainHand, EndSlot = offset + (int)CharacterSlot.OffHand });
            }

            object[] items = new object[slotCount];
            for (int i = 0; i < itemList.Count; i++)
            {
                items[i] = itemList[i].ItemList[0];
            }
            for (int c = 0; c < batchList.Count; c++)
            {
                for (int i = 0; i < characterSlots; i++)
                {
                    ItemInstance itemInstance = batchList[c]._item[i].Clone();
                    if (itemInstance != null && itemInstance.Id != 0)
                    {
                        foreach (var iai in itemInstance.Item.AvailabilityInformation)
                        {
                            if (iai.ItemAvailable.ContainsKey(itemInstance.GemmedId))
                            {
                                itemInstance.ItemAvailabilityInformation = iai;
                                break;
                            }
                        }
                        int index;
                        if (indexFromId.TryGetValue(itemInstance.ItemAvailabilityInformation, out index))
                        {
                            // only set the item if it's marked as available, otherwise leave it empty
                            items[index] = itemInstance;
                            items[itemList.Count + 1 + c * characterSlots + i] = (itemInstance == null) ? null : itemInstance.ItemAvailabilityInformation;
                        }
                    }
                }
            }
            startIndividual = new BatchIndividual(items, itemList.Count, indexFromId, null, batchList);

            itemCacheInitialized = true;
        }

        protected override float GetOptimizationValue(BatchIndividual individual, BatchValuation valuation)
        {
            return valuation.OptimizedValue;
        }

        private static float GetCalculationsValue(Character character, CharacterCalculationsBase calcs, string calculation, List<OptimizationRequirement> requirements)
        {
            float gemValue = -100000 * character.GemRequirementsInvalid;
            float ret = 0;
            foreach (OptimizationRequirement requirement in requirements)
            {
                float calcValue = GetCalculationValue(character, calcs, requirement.Calculation);
                if (requirement.LessThan)
                {
                    if (!(calcValue <= requirement.Value))
                        ret += requirement.Value - calcValue;
                }
                else
                {
                    if (!(calcValue >= requirement.Value))
                        ret += calcValue - requirement.Value;
                }
            }

            if (ret < 0) return ret + gemValue;
            else return GetCalculationValue(character, calcs, calculation) + gemValue;
        }

        private static float GetCalculationValue(Character character, CharacterCalculationsBase calcs, string calculation)
        {
            if (calculation == null || calculation == "[Overall]")
            {
                return calcs.OverallPoints;
            }
            else if (calculation.StartsWith("[SubPoint ", StringComparison.Ordinal))
            {
                return calcs.SubPoints[int.Parse(calculation.Substring(10).TrimEnd(']'))];
            }
            else if (calculation.StartsWith("[Talent ", StringComparison.Ordinal))
            {
                return character.CurrentTalents.Data[int.Parse(calculation.Substring(8).TrimEnd(']'))];
            }
            else if (calculation.StartsWith("[Glyph ", StringComparison.Ordinal))
            {
                return character.CurrentTalents.GlyphData[int.Parse(calculation.Substring(7).TrimEnd(']'))] ? 1 : 0;
            }
            else
            {
                return calcs.GetOptimizableCalculationValue(calculation);
            }
        }

        protected override BatchValuation GetValuation(BatchIndividual individual)
        {
            BatchValuation valuation = new BatchValuation();
            valuation.OptimizedBatchValue = new List<float>();
            float value = 0f;
            for (int i = 0; i < batchList.Count; i++)
            {
                float characterValue = GetCalculationsValue(individual.Character[i], modelList[i].GetCharacterCalculations(individual.Character[i], null, false, false, false), batchList[i].CalculationToOptimize, batchList[i].OptimizationRequirements);
                valuation.OptimizedBatchValue.Add(characterValue);
                value += weightList[i] * characterValue;
            }
            valuation.OptimizedValue = value;
            return valuation;
        }

        protected override object GetItem(BatchIndividual individual, int slot)
        {
            return individual.Items[slot];
        }

        protected override object[] GetItems(BatchIndividual individual)
        {
            return individual.Items;
        }

        protected override BatchIndividual GenerateIndividual(object[] items, bool canUseArray, BatchIndividual recycledIndividual)
        {
            return new BatchIndividual(canUseArray ? items : (object[])items.Clone(), itemList.Count, indexFromId, upgradeItems != null ? upgradeItems[0].ItemAvailabilityInformation : null, batchList);
        }

        protected override BatchIndividual BuildMutantIndividual(BatchIndividual parent, BatchIndividual recycledIndividual)
        {
            bool successful;
            BatchIndividual mutant = null;
            if (Rnd.NextDouble() < 0.9)
            {
                return base.BuildMutantIndividual(parent, recycledIndividual);
            }
            else
            {
                mutant = BuildSwapGemMutantCharacter(parent, out successful);
                if (!successful)
                {
                    return base.BuildMutantIndividual(parent, recycledIndividual);
                }
            }
            return mutant;
        }

        private struct GemInformation
        {
            public int Slot;
            public int Index;
            public Item Gem;
            public ItemSlot Socket;
        }

        private ItemInstance ReplaceGem(ItemInstance item, int index, Item gem)
        {
            ItemInstance copy = new ItemInstance(item.Item, item.RandomSuffixId, item.Gem1, item.Gem2, item.Gem3, item.Enchant, item.Reforging, item.Tinkering) { ItemAvailabilityInformation = item.ItemAvailabilityInformation };
            copy.SetGem(index, gem);
            return copy;
        }

        private BatchIndividual BuildSwapGemMutantCharacter(BatchIndividual parent, out bool successful)
        {
            object[] items = (object[])GetItems(parent).Clone();
            successful = false;

            // do the work
            int c;
            Character character;
            GemInformation mutation1 = new GemInformation();
            GemInformation mutation2 = new GemInformation();
            Random rand = Rnd;
            int tries = 0;
            do
            {
                c = rand.Next(batchList.Count);
                character = parent.Character[c];
                // build a list of possible mutation points
                // make sure not to do meta gem swaps
                List<GemInformation> locationList = new List<GemInformation>();
                for (int slot = 0; slot < characterSlots; slot++)
                {
                    if (character._item[slot] != null)
                    {
                        for (int i = 1; i <= 3; i++)
                        {
                            Item gem = character._item[slot].GetGem(i);
                            if (gem != null && gem.Slot != ItemSlot.Meta) locationList.Add(new GemInformation() { Slot = slot, Index = i, Gem = gem, Socket = character._item[slot].Item.GetSocketColor(i) });
                        }
                    }
                }

                if (locationList.Count > 1)
                {
                    // randomly select mutation point
                    bool promising;
                    do
                    {
                        promising = true;
                        int mutationIndex1 = rand.Next(locationList.Count);
                        int mutationIndex2 = rand.Next(locationList.Count);
                        mutation1 = locationList[mutationIndex1];
                        mutation2 = locationList[mutationIndex2];
                        if (mutation1.Gem.Slot == mutation2.Gem.Slot) promising = false;
                        if (mutation1.Socket == mutation2.Socket) promising = false;
                        int matchNow = 0;
                        if (Item.GemMatchesSlot(mutation1.Gem, mutation1.Socket)) matchNow++;
                        if (Item.GemMatchesSlot(mutation2.Gem, mutation2.Socket)) matchNow++;
                        if (matchNow == 2) promising = false;
                        int matchThen = 0;
                        if (Item.GemMatchesSlot(mutation1.Gem, mutation2.Socket)) matchThen++;
                        if (Item.GemMatchesSlot(mutation2.Gem, mutation1.Socket)) matchThen++;
                        if (tries < 50)
                        {
                            if (matchThen <= matchNow) promising = false;
                        }
                        else
                        {
                            // allow 1 to 1 trade, because the other socket bonus might be better
                            if (matchThen < matchNow || matchThen == 0) promising = false;
                        }
                        tries++;
                    } while (tries % 10 != 0 && !promising);

                    // mutate
                    if (promising)
                    {
                        ItemInstance item1 = ReplaceGem(character._item[mutation1.Slot], mutation1.Index, mutation2.Gem);
                        ItemInstance item2 = ReplaceGem(character._item[mutation2.Slot], mutation2.Index, mutation1.Gem);
                        if (ItemAvailable(item1) && ItemAvailable(item2))
                        {
                            successful = true;
                            items[(upgradeItems != null && item1.Id == upgradeItems[0].Id) ? itemList.Count : indexFromId[item1.ItemAvailabilityInformation]] = item1;
                            items[(upgradeItems != null && item2.Id == upgradeItems[0].Id) ? itemList.Count : indexFromId[item2.ItemAvailabilityInformation]] = item2;
                        }
                    }
                }
            } while (tries < 100 && !successful);

            if (successful && (mutation1.Gem.IsJewelersGem || mutation2.Gem.IsJewelersGem))
            {
                // jeweler preserving mutation
                // by making the swap the character at c will remain at 3 jewelers, but characters that don't have that item
                // might now be left with a suboptimal count
                // heuristic we use is that jeweler gems are better than any other
                // and that by replacing an existing gem will increase value, specially if we can do it at
                // a spot that doesn't match color
                if (mutation2.Gem.IsJewelersGem)
                {
                    GemInformation tmp = mutation1;
                    mutation1 = mutation2;
                    mutation2 = tmp;
                }
                // we're moving jeweler gem from mutation1 to mutation2
                // problematic cases are characters that have item1, but not item2 or have item2 but not item1
                // in that case we'll either have too many or not enough
                List<Character> fixedList = new List<Character>();
                List<Character> oneDownList = new List<Character>();
                List<Character> oneUpList = new List<Character>();
                foreach (Character affectedCharacter in parent.Character)
                {
                    bool eq1 = affectedCharacter.IsEquipped(character._item[mutation1.Slot]);
                    bool eq2 = affectedCharacter.IsEquipped(character._item[mutation2.Slot]);
                    if (eq1 && !eq2 && affectedCharacter.JewelersGemCount <= 3)
                    {
                        oneDownList.Add(affectedCharacter);
                    }
                    else if (!eq1 && eq2 && affectedCharacter.JewelersGemCount >= 3)
                    {
                        oneUpList.Add(affectedCharacter);
                    }
                    else
                    {
                        fixedList.Add(affectedCharacter);
                    }
                }
                // not to complicate things too much we can only fix characters if it does not affect
                // those that are already fixed

                for (int i = 0; i < oneUpList.Count; i++)
                {
                    Character affectedCharacter = oneUpList[i];
                    // see if it has an item with a jeweler gem that does not appear on fixed list
                    for (int slot = 0; slot < 2 * characterSlots; slot++)
                    {
                        ItemInstance originalitem = affectedCharacter._item[rand.Next(characterSlots)];
                        ItemInstance item = originalitem;
                        if ((object)item != null && ItemHasJewelerGem(item))
                        {
                            bool ok = true;
                            foreach (Character ch in fixedList)
                            {
                                if (ch.IsEquipped(item))
                                {
                                    ok = false;
                                    break;
                                }
                            }
                            if (ok)
                            {
                                // we're clear, we can remove the meta gem
                                // anything will be better than breaking the jeweler constraint
                                // pick from available instances that don't have jewelers gems
                                List<ItemInstance> list = new List<ItemInstance>();
                                foreach (ItemInstance it in (upgradeItems != null && item.Id == upgradeItems[0].Id) ? upgradeItems : item.ItemAvailabilityInformation.ItemList)
                                {
                                    if (!ItemHasJewelerGem(it))
                                    {
                                        list.Add(it);
                                    }
                                }
                                if (list.Count > 0)
                                {
                                    item = list[rand.Next(list.Count)];
                                    // we got it
                                    items[(upgradeItems != null && item.Id == upgradeItems[0].Id) ? itemList.Count : indexFromId[item.ItemAvailabilityInformation]] = item;
                                    // any item in up list that had it can be removed
                                    for (int j = oneUpList.Count - 1; j > i; j--)
                                    {
                                        if (oneUpList[j].IsEquipped(originalitem))
                                        {
                                            fixedList.Add(oneUpList[j]);
                                            oneUpList.RemoveAt(j);
                                        }
                                    }
                                    fixedList.Add(oneUpList[i]);
                                    oneUpList.RemoveAt(i);
                                    i--;
                                    break;
                                }
                            }
                        }
                    }
                }

                for (int i = 0; i < oneDownList.Count; i++)
                {
                    Character affectedCharacter = oneDownList[i];
                    // see if it has an item with a free gem that does not appear on fixed list
                    for (int slot = 0; slot < 2 * characterSlots; slot++)
                    {
                        ItemInstance originalitem = affectedCharacter._item[rand.Next(characterSlots)];
                        ItemInstance item = originalitem;
                        if ((object)item != null && ItemHasNonJewelerGem(item))
                        {
                            bool ok = true;
                            foreach (Character ch in fixedList)
                            {
                                if (ch.IsEquipped(item))
                                {
                                    ok = false;
                                    break;
                                }
                            }
                            if (ok)
                            {
                                // we're clear, we can remove the meta gem
                                // anything will be better than breaking the jeweler constraint
                                // pick from available instances that don't have jewelers gems
                                int pickTries = 0;
                                var availabilityInformation = itemGenerator.GenerateItemAvailabilityInformation(originalitem.Item, originalitem.RandomSuffixId);
                                do
                                {
                                    item = originalitem.Clone();
                                    int g = 1 + rand.Next(availabilityInformation.GemCount);
                                    item.SetGem(g, mutation1.Gem);
                                    pickTries++;
                                } while (pickTries <= 5 && !ItemAvailable(item));
                                if (pickTries <= 5)
                                {
                                    // we got it
                                    items[(upgradeItems != null && item.Id == upgradeItems[0].Id) ? itemList.Count : indexFromId[item.ItemAvailabilityInformation]] = item;
                                    // any item in down list that had it can be removed
                                    for (int j = oneDownList.Count - 1; j > i; j--)
                                    {
                                        if (oneDownList[j].IsEquipped(originalitem))
                                        {
                                            fixedList.Add(oneDownList[j]);
                                            oneDownList.RemoveAt(j);
                                        }
                                    }
                                    fixedList.Add(oneDownList[i]);
                                    oneDownList.RemoveAt(i);
                                    i--;
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            if (successful)
            {
                return GenerateIndividual(items, true, null);
            }
            return null;
        }

        private bool ItemAvailable(ItemInstance item)
        {
            return ((upgradeItems != null && item.Id == upgradeItems[0].Id && upgradeItems.Contains(item)) || ((upgradeItems == null || item.Id != upgradeItems[0].Id) && item.ItemAvailabilityInformation.ItemAvailable.ContainsKey(item.GemmedId)));
        }

        private static bool ItemHasJewelerGem(ItemInstance item)
        {
            for (int g = 1; g <= 3; g++)
            {
                Item gem = item.GetGem(g);
                if (gem != null && gem.IsJewelersGem)
                {
                    return true;
                }
            }
            return false;
        }

        private static bool ItemHasJewelerGem(ItemInstance item, Item jewelerGem)
        {
            for (int g = 1; g <= 3; g++)
            {
                Item gem = item.GetGem(g);
                if (gem != null && gem.Id == jewelerGem.Id)
                {
                    return true;
                }
            }
            return false;
        }

        private static bool ItemHasNonJewelerGem(ItemInstance item)
        {
            bool ok = false;
            for (int g = 1; g <= 3; g++)
            {
                Item gem = item.GetGem(g);
                if (gem != null && gem.IsJewelersGem)
                {
                    return false;
                }
                if (gem != null && gem.Slot != ItemSlot.Meta && !gem.IsJewelersGem)
                {
                    ok = true;
                }
            }
            return ok;
        }

        protected override BatchIndividual BuildChildIndividual(BatchIndividual father, BatchIndividual mother, BatchIndividual recycledIndividual)
        {
            return GeneratorBuildIndividual(
                delegate(int slot, object[] items)
                {
                    if (slot <= itemList.Count && (father.UsedCount[slot] > 0 || mother.UsedCount[slot] > 0))
                    {
                        double chance = father.UsedCount[slot] / (father.UsedCount[slot] + mother.UsedCount[slot]);
                        return Rnd.NextDouble() < chance ? GetItem(father, slot) : GetItem(mother, slot);
                    }
                    else
                    {
                        return Rnd.NextDouble() < 0.5d ? GetItem(father, slot) : GetItem(mother, slot);
                    }
                },
                recycledIndividual);
        }
    }
}
