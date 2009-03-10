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
        public List<Character> OptimizedBatchCharacter;
        public List<float> OptimizedBatchValue;
        public float OptimizedValue;
    }

    public class BatchIndividual
    {
        public object[] Items;
        public List<ItemInstance> AvailableItems;

        public BatchIndividual(object[] items, List<Item> itemList, Item upgradeItem)
        {
            Items = items;
            AvailableItems = new List<ItemInstance>();
            int count = items.Length / 4;
            Item[] gems = new Item[3];
            for (int i = 0; i < count; i++)
            {
                Item item;
                if (i == itemList.Count)
                {
                    item = upgradeItem;
                }
                else
                {
                    item = itemList[i];
                }
                if (item != null)
                {
                    Array.Clear(gems, 0, 3);
                    for (int j = 0; j < item.OptimizerItemInformation.GemCount; j++)
                    {
                        gems[j] = items[i * 4 + j] as Item;
                    }
                    AvailableItems.Add(new ItemInstance(item, gems[0], gems[1], gems[2], items[i * 4 + 3] as Enchant));
                }
            }
        }

        public BatchIndividual(List<ItemInstance> availableItems)
        {
            AvailableItems = availableItems;
            Items = new object[4 * availableItems.Count];
            for (int i = 0; i < availableItems.Count; i++)
            {
                ItemInstance itemInstance = availableItems[i];
                if (itemInstance != null)
                {
                    Items[4 * i] = itemInstance.Gem1;
                    Items[4 * i + 1] = itemInstance.Gem2;
                    Items[4 * i + 2] = itemInstance.Gem3;
                    Items[4 * i + 3] = itemInstance.Enchant;
                }
            }
        }
    }

    public class BatchOptimizer : OptimizerBase<object, BatchIndividual, BatchValuation>
    {
        //private Character _character;
        //private CalculationsBase model;

        private class BatchCharacter
        {
            public Character Character;
            public CalculationsBase Model;
            public float Weight;
        }

        private List<BatchCharacter> batchList;

        private ItemInstanceOptimizer optimizer;
        private AutoResetEvent optimizerComplete;
        private Character optimizedCharacter;
        private float optimizedValue;

        private int optimizerThoroughness;

        private class ItemAvailableValidator : OptimizerRangeValidatorBase<object>
        {
            private BatchOptimizer optimizer;

            public override bool IsValid(object[] items)
            {
                Item item;
                int index = StartSlot / 4;
                if (index == optimizer.itemList.Count)
                {
                    if (optimizer.upgradeItems == null) return true;
                    Item gem1 = items[StartSlot] as Item;
                    Item gem2 = items[StartSlot + 1] as Item;
                    Item gem3 = items[StartSlot + 2] as Item;
                    Enchant enchant = items[StartSlot + 3] as Enchant;
                    int gemCount = optimizer.upgradeGemCount;
                    string key = string.Format("{0}.{1}.{2}.{3}",
                        gem1 != null && gemCount >= 1 ? gem1.Id : 0,
                        gem2 != null && gemCount >= 2 ? gem2.Id : 0,
                        gem3 != null && gemCount >= 3 ? gem3.Id : 0,
                        enchant != null ? enchant.Id : 0);
                    return optimizer.upgradeAvailable.ContainsKey(key);
                }
                else
                {
                    item = optimizer.itemList[index];
                    Item gem1 = items[StartSlot] as Item;
                    Item gem2 = items[StartSlot + 1] as Item;
                    Item gem3 = items[StartSlot + 2] as Item;
                    Enchant enchant = items[StartSlot + 3] as Enchant;
                    int gemCount = item.OptimizerItemInformation.GemCount;
                    string key = string.Format("{0}.{1}.{2}.{3}.{4}",
                        item != null ? item.Id : 0,
                        gem1 != null && gemCount >= 1 ? gem1.Id : 0,
                        gem2 != null && gemCount >= 2 ? gem2.Id : 0,
                        gem3 != null && gemCount >= 3 ? gem3.Id : 0,
                        enchant != null ? enchant.Id : 0);
                    return item.OptimizerItemInformation.ItemAvailable.ContainsKey(key);
                }
            }

            public ItemAvailableValidator(BatchOptimizer optimizer, int slot)
            {
                this.optimizer = optimizer;
                this.StartSlot = slot;
                EndSlot = slot + 3;
            }
        }

        private const int characterSlots = 19;

        public BatchOptimizer(List<KeyValuePair<Character, float>> batchList)
        {
            if (batchList == null || batchList.Count == 0) throw new ArgumentException("Batch list must have at least one element.");
            this.batchList = new List<BatchCharacter>();
            foreach (KeyValuePair<Character, float> kvp in batchList)
            {
                this.batchList.Add(new BatchCharacter() { Character = kvp.Key.Clone(), Model = Calculations.GetModel(kvp.Key.CurrentModel), Weight = kvp.Value });
            }

            optimizer = new ItemInstanceOptimizer();
            optimizer.OptimizeCharacterCompleted += new OptimizeCharacterCompletedEventHandler(optimizer_OptimizeCharacterCompleted);
            optimizer.OptimizeCharacterProgressChanged += new OptimizeCharacterProgressChangedEventHandler(optimizer_OptimizeCharacterProgressChanged);
            optimizerComplete = new AutoResetEvent(false);

            optimizeCharacterProgressChangedDelegate = new SendOrPostCallback(PrivateOptimizeBatchProgressChanged);
            optimizeBatchCompletedDelegate = new SendOrPostCallback(PrivateOptimizeBatchCompleted);
            optimizeCharacterThreadStartDelegate = new OptimizeBatchThreadStartDelegate(OptimizeBatchThreadStart);
            computeUpgradesProgressChangedDelegate = new SendOrPostCallback(PrivateComputeUpgradesProgressChanged);
            computeUpgradesCompletedDelegate = new SendOrPostCallback(PrivateComputeUpgradesCompleted);
            computeUpgradesThreadStartDelegate = new ComputeUpgradesThreadStartDelegate(ComputeUpgradesThreadStart);
            evaluateUpgradeProgressChangedDelegate = new SendOrPostCallback(PrivateEvaluateUpgradeProgressChanged);
            evaluateUpgradeCompletedDelegate = new SendOrPostCallback(PrivateEvaluateUpgradeCompleted);
            evaluateUpgradeThreadStartDelegate = new EvaluateUpgradeThreadStartDelegate(EvaluateUpgradeThreadStart);

            OptimizationMethod = OptimizationMethod.SimulatedAnnealing;
        }

        void optimizer_OptimizeCharacterProgressChanged(object sender, OptimizeCharacterProgressChangedEventArgs e)
        {
            
        }

        void optimizer_OptimizeCharacterCompleted(object sender, OptimizeCharacterCompletedEventArgs e)
        {
            optimizedValue = e.OptimizedCharacterValue;
            optimizedCharacter = e.OptimizedCharacter;
            optimizerComplete.Set();
        }

        public override void CancelAsync()
        {
            base.CancelAsync();
            optimizer.CancelAsync();
        }

        public void InitializeItemCache(Character character, List<string> availableItems, bool overrideRegem, bool overrideReenchant, bool templateGemsEnabled, CalculationsBase model)
        {
            //_character = character;
            //this.model = model;

            if (templateGemsEnabled)
            {
                availableItems = new List<string>(availableItems);
                List<string> templateGems = new List<string>();
                // this could actually be empty, but in practice they will populate it at least once before
                foreach (GemmingTemplate template in GemmingTemplate.AllTemplates[model.Name])
                {
                    if (template.Enabled)
                    {
                        if (!templateGems.Contains(template.RedId.ToString())) templateGems.Add(template.RedId.ToString());
                        if (!templateGems.Contains(template.YellowId.ToString())) templateGems.Add(template.YellowId.ToString());
                        if (!templateGems.Contains(template.BlueId.ToString())) templateGems.Add(template.BlueId.ToString());
                        if (!templateGems.Contains(template.PrismaticId.ToString())) templateGems.Add(template.PrismaticId.ToString());
                        if (!templateGems.Contains(template.MetaId.ToString())) templateGems.Add(template.MetaId.ToString());
                    }
                }
                foreach (string gem in templateGems)
                {
                    if (!availableItems.Contains(gem)) availableItems.Add(gem);
                }
            }
            PopulateAvailableIds(availableItems, overrideRegem, overrideReenchant);
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
        private delegate void OptimizeBatchThreadStartDelegate(int batchThoroughness, int thoroughness);
        private delegate void ComputeUpgradesThreadStartDelegate(int batchThoroughness, int thoroughness, Item singleItemUpgrades);
        private delegate void EvaluateUpgradeThreadStartDelegate(int batchThoroughness, int thoroughness, ItemInstance upgrade);

        public event OptimizeBatchCompletedEventHandler OptimizeBatchCompleted;
        public event OptimizeCharacterProgressChangedEventHandler OptimizeBatchProgressChanged;
        public event ComputeUpgradesProgressChangedEventHandler ComputeUpgradesProgressChanged;
        public event ComputeUpgradesCompletedEventHandler ComputeUpgradesCompleted;
        public event ProgressChangedEventHandler EvaluateUpgradeProgressChanged;
        public event EvaluateUpgradeCompletedEventHandler EvaluateUpgradeCompleted;

        private SendOrPostCallback optimizeCharacterProgressChangedDelegate;
        private SendOrPostCallback optimizeBatchCompletedDelegate;
        private OptimizeBatchThreadStartDelegate optimizeCharacterThreadStartDelegate;
        private SendOrPostCallback computeUpgradesProgressChangedDelegate;
        private SendOrPostCallback computeUpgradesCompletedDelegate;
        private ComputeUpgradesThreadStartDelegate computeUpgradesThreadStartDelegate;
        private SendOrPostCallback evaluateUpgradeProgressChangedDelegate;
        private SendOrPostCallback evaluateUpgradeCompletedDelegate;
        private EvaluateUpgradeThreadStartDelegate evaluateUpgradeThreadStartDelegate;

        public void OptimizeCharacterAsync(int batchThoroughness, int thoroughness)
        {
            if (isBusy) throw new InvalidOperationException("Optimizer is working on another operation.");
            isBusy = true;
            cancellationPending = false;
            asyncOperation = AsyncOperationManager.CreateOperation(null);
            optimizeCharacterThreadStartDelegate.BeginInvoke(batchThoroughness, thoroughness, null, null);
        }

        private void OptimizeBatchThreadStart(int batchThoroughness, int thoroughness)
        {
            Exception error = null;
            BatchIndividual optimizedBatch = null;
            BatchValuation optimizedValuation = null;
            float optimizedBatchValue = 0.0f;
            try
            {
                optimizedBatch = PrivateOptimizeBatch(batchThoroughness, thoroughness, out optimizedValuation, out error);
                if (optimizedBatch != null)
                {
                    optimizedBatchValue = GetOptimizationValue(optimizedBatch);
                }
                else
                {
                    if (error == null) error = new NullReferenceException();
                }
            }
            catch (Exception ex)
            {
                error = ex;
            }
            asyncOperation.PostOperationCompleted(optimizeBatchCompletedDelegate, new OptimizeBatchCompletedEventArgs(optimizedBatch, optimizedValuation, optimizedBatchValue, error, cancellationPending));
        }

        public void ComputeUpgradesAsync(int batchThoroughness, int thoroughness)
        {
            ComputeUpgradesAsync(batchThoroughness, thoroughness, null);
        }

        public void ComputeUpgradesAsync(int batchThoroughness, int thoroughness, Item singleItemUpgrades)
        {
            if (isBusy) throw new InvalidOperationException("Optimizer is working on another operation.");
            isBusy = true;
            cancellationPending = false;
            asyncOperation = AsyncOperationManager.CreateOperation(null);
            computeUpgradesThreadStartDelegate.BeginInvoke(batchThoroughness, thoroughness, singleItemUpgrades, null, null);
        }

        private void ComputeUpgradesThreadStart(int batchThoroughness, int thoroughness, Item singleItemUpgrades)
        {
            Exception error = null;
            Dictionary<Character.CharacterSlot, List<ComparisonCalculationBase>> upgrades = null;
            try
            {
                upgrades = PrivateComputeUpgrades(batchThoroughness, thoroughness, singleItemUpgrades, out error);
            }
            catch (Exception ex)
            {
                error = ex;
            }
            asyncOperation.PostOperationCompleted(computeUpgradesCompletedDelegate, new ComputeUpgradesCompletedEventArgs(upgrades, error, cancellationPending));
        }

        public void EvaluateUpgradeAsync(int batchThoroughness, int thoroughness, ItemInstance upgrade)
        {
            if (isBusy) throw new InvalidOperationException("Optimizer is working on another operation.");
            isBusy = true;
            cancellationPending = false;
            asyncOperation = AsyncOperationManager.CreateOperation(null);
            evaluateUpgradeThreadStartDelegate.BeginInvoke(batchThoroughness, thoroughness, upgrade, null, null);
        }

        private void EvaluateUpgradeThreadStart(int batchThoroughness, int thoroughness, ItemInstance upgrade)
        {
            Exception error = null;
            float upgradeValue = 0f;
            try
            {
                upgradeValue = PrivateEvaluateUpgrade(batchThoroughness, thoroughness, upgrade, out error);
            }
            catch (Exception ex)
            {
                error = ex;
            }
            asyncOperation.PostOperationCompleted(evaluateUpgradeCompletedDelegate, new EvaluateUpgradeCompletedEventArgs(upgradeValue, error, cancellationPending));
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

        public BatchIndividual OptimizeCharacter(int batchThoroughness, int thoroughness, out BatchValuation bestValuation)
        {
            if (isBusy) throw new InvalidOperationException("Optimizer is working on another operation.");
            isBusy = true;
            cancellationPending = false;
            asyncOperation = null;
            Exception error;
            BatchIndividual optimizedBatch = PrivateOptimizeBatch(batchThoroughness, thoroughness, out bestValuation, out error);
            if (error != null) throw error;
            isBusy = false;
            return optimizedBatch;
        }

        private BatchIndividual PrivateOptimizeBatch(int batchThoroughness, int thoroughness, out BatchValuation bestValuation, out Exception error)
        {
            if (!itemCacheInitialized) throw new InvalidOperationException("Optimization item cache was not initialized.");
            error = null;
            //_character = character;
            //model = Calculations.GetModel(_character.CurrentModel);
            _thoroughness = batchThoroughness;
            optimizerThoroughness = thoroughness;

            currentOperation = OptimizationOperation.OptimizeCharacter;
            BatchIndividual optimizedCharacter = null;
            float bestValue = 0.0f;
            bestValuation = null;
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
                slotCount = itemList.Count * 4;
                optimizedCharacter = Optimize(null, 0.0f, out bestValue, out bestValuation, out injected);
            }
            catch (Exception ex)
            {
                error = ex;
            }

            ReportProgress(100, bestValue);
            return optimizedCharacter;
        }

        public Dictionary<Character.CharacterSlot, List<ComparisonCalculationBase>> ComputeUpgrades(int batchThoroughness, int thoroughness, Item singleItemUpgrades)
        {
            if (isBusy) throw new InvalidOperationException("Optimizer is working on another operation.");
            isBusy = true;
            cancellationPending = false;
            asyncOperation = null;
            Exception error;
            Dictionary<Character.CharacterSlot, List<ComparisonCalculationBase>> upgrades = PrivateComputeUpgrades(batchThoroughness, thoroughness, singleItemUpgrades, out error);
            if (error != null) throw error;
            isBusy = false;
            return upgrades;
        }

        private int itemProgressPercentage = 0;
        private string currentItem = "";

        private Dictionary<Character.CharacterSlot, List<ComparisonCalculationBase>> PrivateComputeUpgrades(int batchThoroughness, int thoroughness, Item singleItemUpgrades, out Exception error)
        {
            if (!itemCacheInitialized) throw new InvalidOperationException("Optimization item cache was not initialized.");
            error = null;
            //_character = character;
            //model = Calculations.GetModel(_character.CurrentModel);
            _thoroughness = batchThoroughness;
            optimizerThoroughness = thoroughness;

            currentOperation = OptimizationOperation.ComputeUpgrades;
            Dictionary<Character.CharacterSlot, List<ComparisonCalculationBase>> upgrades = null;
            try
            {
                // make equipped gear/enchant valid
                //MarkEquippedItemsAsValid(_character);

                upgrades = new Dictionary<Character.CharacterSlot, List<ComparisonCalculationBase>>();

                Item[] items = ItemCache.GetRelevantItems(batchList[0].Model);
                Character.CharacterSlot[] slots = new Character.CharacterSlot[] { Character.CharacterSlot.Back, Character.CharacterSlot.Chest, Character.CharacterSlot.Feet, Character.CharacterSlot.Finger1, Character.CharacterSlot.Hands, Character.CharacterSlot.Head, Character.CharacterSlot.Legs, Character.CharacterSlot.MainHand, Character.CharacterSlot.Neck, Character.CharacterSlot.OffHand, Character.CharacterSlot.Projectile, Character.CharacterSlot.ProjectileBag, Character.CharacterSlot.Ranged, Character.CharacterSlot.Shoulders, Character.CharacterSlot.Trinket1, Character.CharacterSlot.Waist, Character.CharacterSlot.Wrist };
                foreach (Character.CharacterSlot slot in slots)
                    upgrades[slot] = new List<ComparisonCalculationBase>();

                slotCount = (itemList.Count + 1) * 4; // one extra slot for upgrade evaluations
                upgradeItems = null;

                float baseValue;
                BatchIndividual baseBatch = Optimize(out baseValue);

                Dictionary<int, Item> itemById = new Dictionary<int, Item>();
                foreach (Item item in items)
                {
                    itemById[item.Id] = item;
                }

                if (singleItemUpgrades != null)
                {
                    items = new Item[] { singleItemUpgrades };
                }
                else
                {
                    items = new List<Item>(itemById.Values).ToArray();
                }

                for (int i = 0; i < items.Length; i++)
                {
                    Item item = items[i];
                    currentItem = item.Name;
                    itemProgressPercentage = (int)Math.Round((float)i / ((float)items.Length / 100f));
                    if (cancellationPending)
                    {
                        return null;
                    }
                    ReportProgress(0, 0);
                    foreach (Character.CharacterSlot slot in slots)
                    {
                        if (item.FitsInSlot(slot, batchList[0].Character))
                        {
                            List<ComparisonCalculationBase> comparisons = upgrades[slot];
                            PopulateUpgradeItems(item);
                            List<ItemInstance> availableItems = new List<ItemInstance>(baseBatch.AvailableItems);
                            availableItems.Add(upgradeItems[0]);
                            BatchIndividual batch = new BatchIndividual(availableItems);
                            // instead of just putting in the first gemming on the list select the best one
                            float best = -10000000f;
                            BatchValuation bestCalculations;
                            BatchIndividual bestCharacter;
                            bool injected;
                            bestCharacter = Optimize(baseBatch, baseValue, out best, out bestCalculations, out injected);
                            if (best > baseValue)
                            {
                                ItemInstance bestItem = bestCharacter.AvailableItems[itemList.Count];
                                ComparisonCalculationBase itemCalc = Calculations.CreateNewComparisonCalculation();
                                itemCalc.ItemInstance = bestItem;
                                itemCalc.Item = bestItem.Item;
                                //itemCalc.Character = bestCharacter;
                                itemCalc.Name = item.Name;
                                itemCalc.Equipped = false;
                                //itemCalc.OverallPoints = bestCalculations.OverallPoints - baseCalculations.OverallPoints;
                                //float[] subPoints = new float[bestCalculations.SubPoints.Length];
                                //for (int j = 0; j < bestCalculations.SubPoints.Length; j++)
                                //{
                                //    subPoints[j] = bestCalculations.SubPoints[j] - baseCalculations.SubPoints[j];
                                //}
                                //itemCalc.SubPoints = subPoints;
                                itemCalc.OverallPoints = best - baseValue;

                                comparisons.Add(itemCalc);
                            }
                        }
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

        public float EvaluateUpgrade(int batchThoroughness, int thoroughness, ItemInstance upgrade)
        {
            if (isBusy) throw new InvalidOperationException("Optimizer is working on another operation.");
            isBusy = true;
            cancellationPending = false;
            asyncOperation = null;
            Exception error;
            float upgradeValue = PrivateEvaluateUpgrade(batchThoroughness, thoroughness, upgrade, out error);
            if (error != null) throw error;
            isBusy = false;
            return upgradeValue;
        }

        private float PrivateEvaluateUpgrade(int batchThoroughness, int thoroughness, ItemInstance upgrade, out Exception error)
        {
            if (!itemCacheInitialized) throw new InvalidOperationException("Optimization item cache was not initialized.");
            error = null;
            //_character = character;
            //model = Calculations.GetModel(_character.CurrentModel);
            _thoroughness = batchThoroughness;
            optimizerThoroughness = thoroughness;

            slotCount = (itemList.Count + 1) * 4; // one extra slot for upgrade evaluations

            currentOperation = OptimizationOperation.EvaluateUpgrade;
            //Character saveCharacter = _character;
            float upgradeValue = 0f;
            try
            {
                // make equipped gear/enchant valid
                // this is currently only called after calculate upgrades already marks items as valid, but we might have to do this here also if things change
                // MarkEquippedItemsAsValid(_character);

                float baseValue;
                BatchIndividual baseBatch = Optimize(out baseValue);

                ItemInstance item = upgrade;
                upgradeItems = new List<ItemInstance>() { item };
                upgradeAvailable = new Dictionary<string, bool>();
                upgradeAvailable[string.Format("{0}.{1}.{2}.{3}", item.Gem1Id, item.Gem2Id, item.Gem3Id, item.EnchantId)] = true;
                List<ItemInstance> availableItems = new List<ItemInstance>(baseBatch.AvailableItems);
                availableItems[itemList.Count] = upgradeItems[0];
                BatchIndividual batch = new BatchIndividual(availableItems);
                // instead of just putting in the first gemming on the list select the best one
                float best = -10000000f;
                BatchValuation bestCalculations;
                BatchIndividual bestCharacter;
                bool injected;
                bestCharacter = Optimize(baseBatch, baseValue, out best, out bestCalculations, out injected);
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
        int upgradeGemCount;
        Dictionary<string, bool> upgradeAvailable;
        List<Item> itemList = new List<Item>();
        Dictionary<int, int> indexFromId = new Dictionary<int, int>();

        AvailableItemGenerator itemGenerator;

        private void PopulateUpgradeItems(Item item)
        {
            upgradeItems = itemGenerator.GetPossibleGemmedItemsForItem(item, item.Id.ToString());
            upgradeAvailable = new Dictionary<string, bool>();
            foreach (ItemInstance itemInstance in upgradeItems)
            {
                upgradeAvailable[string.Format("{0}.{1}.{2}.{3}", itemInstance.Gem1Id, itemInstance.Gem2Id, itemInstance.Gem3Id, itemInstance.EnchantId)] = true;
            }
            upgradeGemCount = GetItemGemCount(item);
        }

        private void PopulateAvailableIds(List<string> availableItems, bool overrideRegem, bool overrideReenchant)
        {
            itemGenerator = new AvailableItemGenerator(availableItems, overrideRegem, overrideReenchant, batchList[0].Character, batchList[0].Model);
            foreach (Item item in ItemCache.Items.Values)
            {
                item.OptimizerItemInformation = null;
            }
            List<ItemInstance>[] slotList = itemGenerator.SlotItems;

            Dictionary<int, bool> itemUnique = new Dictionary<int, bool>();
            itemList = new List<Item>();
            indexFromId = new Dictionary<int, int>();
            for (int i = 0; i < slotList.Length; i++)
            {
                foreach (ItemInstance itemInstance in slotList[i])
                {
                    if (itemInstance != null && itemInstance.Item != null)
                    {
                        Item item = itemInstance.Item;
                        if (item.OptimizerItemInformation == null)
                        {
                            GenerateOptimizerItemInformation(item);
                        }
                        if (!item.OptimizerItemInformation.ItemAvailable.ContainsKey(itemInstance.GemmedId))
                        {
                            item.OptimizerItemInformation.ItemList.Add(itemInstance);
                            item.OptimizerItemInformation.ItemAvailable[itemInstance.GemmedId] = true;
                        }
                        if (!itemUnique.ContainsKey(itemInstance.Id))
                        {
                            indexFromId[item.Id] = itemList.Count;
                            itemList.Add(item);
                            itemUnique[itemInstance.Id] = true;
                        }
                    }
                }
            }

            validators = new List<OptimizerRangeValidatorBase<object>>();
            for (int i = 0; i < itemList.Count + 1; i++)
            {
                validators.Add(new ItemAvailableValidator(this, 4 * i));
            }

            itemCacheInitialized = true;
        }

        private void GenerateOptimizerItemInformation(Item item)
        {
            item.OptimizerItemInformation = new OptimizerItemInformation();
            item.OptimizerItemInformation.GemCount = GetItemGemCount(item);
        }

        private int GetItemGemCount(Item item)
        {
            int gemCount = 0;
            bool blacksmithingSocket = (item.Slot == Item.ItemSlot.Waist && batchList[0].Character.WaistBlacksmithingSocketEnabled) || (item.Slot == Item.ItemSlot.Hands && batchList[0].Character.HandsBlacksmithingSocketEnabled) || (item.Slot == Item.ItemSlot.Wrist && batchList[0].Character.WristBlacksmithingSocketEnabled);
            switch (item.SocketColor1)
            {
                case Item.ItemSlot.Meta:
                case Item.ItemSlot.Red:
                case Item.ItemSlot.Orange:
                case Item.ItemSlot.Yellow:
                case Item.ItemSlot.Green:
                case Item.ItemSlot.Blue:
                case Item.ItemSlot.Purple:
                case Item.ItemSlot.Prismatic:
                    gemCount++;
                    break;
                default:
                    if (blacksmithingSocket)
                    {
                        gemCount++;
                        blacksmithingSocket = false;
                    }
                    break;
            }
            switch (item.SocketColor2)
            {
                case Item.ItemSlot.Meta:
                case Item.ItemSlot.Red:
                case Item.ItemSlot.Orange:
                case Item.ItemSlot.Yellow:
                case Item.ItemSlot.Green:
                case Item.ItemSlot.Blue:
                case Item.ItemSlot.Purple:
                case Item.ItemSlot.Prismatic:
                    gemCount++;
                    break;
                default:
                    if (blacksmithingSocket)
                    {
                        gemCount++;
                        blacksmithingSocket = false;
                    }
                    break;
            }
            switch (item.SocketColor3)
            {
                case Item.ItemSlot.Meta:
                case Item.ItemSlot.Red:
                case Item.ItemSlot.Orange:
                case Item.ItemSlot.Yellow:
                case Item.ItemSlot.Green:
                case Item.ItemSlot.Blue:
                case Item.ItemSlot.Purple:
                case Item.ItemSlot.Prismatic:
                    gemCount++;
                    break;
                default:
                    if (blacksmithingSocket)
                    {
                        gemCount++;
                        blacksmithingSocket = false;
                    }
                    break;
            }
            return gemCount;
        }

        protected override float GetOptimizationValue(BatchIndividual individual, BatchValuation valuation)
        {
            return valuation.OptimizedValue;
        }

        protected override BatchValuation GetValuation(BatchIndividual individual)
        {
            BatchValuation valuation = new BatchValuation();
            valuation.OptimizedBatchCharacter = new List<Character>();
            valuation.OptimizedBatchValue = new List<float>();
            float value = 0f;
            optimizer.InitializeItemCache(individual.AvailableItems);
            for (int i = 0; i < batchList.Count; i++)
            {
                optimizer.Model = batchList[i].Model;
                // set up the character with currently available item instances
                for (int slot = 0; slot < characterSlots; slot++)
                {
                    int index;
                    ItemInstance current = batchList[i].Character[(Character.CharacterSlot)slot];
                    if (current != null && indexFromId.TryGetValue(current.Id, out index))
                    {
                        batchList[i].Character[(Character.CharacterSlot)slot] = individual.AvailableItems[index];
                    }
                    else
                    {
                        batchList[i].Character[(Character.CharacterSlot)slot] = null;
                    }
                }
                optimizer.OptimizeCharacterAsync(batchList[i].Character, batchList[i].Character.CalculationToOptimize, batchList[i].Character.OptimizationRequirements.ToArray(), optimizerThoroughness, true);
                optimizerComplete.WaitOne();
                if (cancellationPending) break;
                valuation.OptimizedBatchCharacter.Add(optimizedCharacter);
                valuation.OptimizedBatchValue.Add(optimizedValue);
                value += batchList[i].Weight * optimizedValue;
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

        protected override BatchIndividual GenerateIndividual(object[] items)
        {
            return new BatchIndividual(items, itemList, upgradeItems != null ? upgradeItems[0].Item : null);
        }

        protected override object GetRandomItem(int slot, object[] items)
        {
            int index = slot / 4;
            if (index == itemList.Count)
            {
                if (upgradeItems != null)
                {
                    ItemInstance itemInstance = upgradeItems[rand.Next(upgradeItems.Count)];
                    switch (slot % 4)
                    {
                        case 0:
                            return itemInstance != null ? itemInstance.Gem1 : null;
                        case 1:
                            return itemInstance != null ? itemInstance.Gem2 : null;
                        case 2:
                            return itemInstance != null ? itemInstance.Gem3 : null;
                        case 3:
                            return itemInstance != null ? itemInstance.Enchant : null;
                    }
                }
                else
                {
                    return null;
                }
            }
            else
            {
                Item item = itemList[index];
                switch (slot % 4)
                {
                    case 0:
                        if (item != null)
                        {
                            List<ItemInstance> list = item.OptimizerItemInformation.ItemList;
                            return list[rand.Next(list.Count)].Gem1;
                        }
                        else
                        {
                            return null;
                        }
                    case 1:
                        if (item != null)
                        {
                            List<ItemInstance> list = item.OptimizerItemInformation.ItemList;
                            return list[rand.Next(list.Count)].Gem2;
                        }
                        else
                        {
                            return null;
                        }
                    case 2:
                        if (item != null)
                        {
                            List<ItemInstance> list = item.OptimizerItemInformation.ItemList;
                            return list[rand.Next(list.Count)].Gem3;
                        }
                        else
                        {
                            return null;
                        }
                    case 3:
                        if (item != null)
                        {
                            List<ItemInstance> list = item.OptimizerItemInformation.ItemList;
                            return list[rand.Next(list.Count)].Enchant;
                        }
                        else
                        {
                            return null;
                        }
                }
                return null;
            }
            return null;
        }

        protected override KeyValuePair<float, BatchIndividual> LookForDirectItemUpgrades(List<object> items, int slot, float best, BatchIndividual bestIndividual, out BatchValuation bestValuation)
        {
            bestValuation = null;
            return new KeyValuePair<float, BatchIndividual>(float.NegativeInfinity, null);
        }
    }
}
