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
        public List<ItemInstance> AvailableItem;
        public List<Character> Character;

        private const int characterSlots = 19;

        public BatchIndividual(object[] items, int itemCount, Dictionary<int, int> indexFromId, Item upgradeItem, List<Character> batchList)
        {
            Items = (object[])items.Clone();
            AvailableItem = new List<ItemInstance>();
            for (int i = 0; i <= itemCount; i++)
            {
                ItemInstance itemInstance = items[i] as ItemInstance;
                AvailableItem.Add(itemInstance);
            }
            int charCount = (items.Length - itemCount - 1) / characterSlots;
            Character = new List<Character>();
            for (int c = 0; c < charCount; c++)
            {
                ItemInstance[] slotItems = new ItemInstance[characterSlots];
                for (int i = 0; i < characterSlots; i++)
                {
                    Item item = items[itemCount + 1 + c * characterSlots + i] as Item;
                    if (item != null)
                    {
                        if (item == upgradeItem)
                        {
                            slotItems[i] = AvailableItem[itemCount];
                        }
                        else
                        {
                            slotItems[i] = AvailableItem[indexFromId[item.Id]];
                        }
                    }
                }
                Character _character = batchList[c];
                Character character = new Character(_character.Name, _character.Realm, _character.Region, _character.Race, slotItems,
                _character.ActiveBuffs, _character.CurrentModel);
                character.CalculationOptions = _character.CalculationOptions;
                character.Class = _character.Class;
                character.AssignAllTalentsFromCharacter(_character);
                character.EnforceGemRequirements = _character.EnforceGemRequirements;
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
                return !(items[StartSlot] != null && items[EndSlot] != null && ((Item)items[StartSlot]).Id == ((Item)items[EndSlot]).Id && ((Item)items[StartSlot]).Unique);
            }
        }

        public BatchOptimizer(List<KeyValuePair<Character, float>> batchList, bool overrideRegem, bool overrideReenchant, bool templateGemsEnabled)
        {
            if (batchList == null || batchList.Count == 0) throw new ArgumentException("Batch list must have at least one element.");
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
            optimizeCharacterThreadStartDelegate = new OptimizeBatchThreadStartDelegate(OptimizeBatchThreadStart);
            computeUpgradesProgressChangedDelegate = new SendOrPostCallback(PrivateComputeUpgradesProgressChanged);
            computeUpgradesCompletedDelegate = new SendOrPostCallback(PrivateComputeUpgradesCompleted);
            computeUpgradesThreadStartDelegate = new ComputeUpgradesThreadStartDelegate(ComputeUpgradesThreadStart);
            evaluateUpgradeProgressChangedDelegate = new SendOrPostCallback(PrivateEvaluateUpgradeProgressChanged);
            evaluateUpgradeCompletedDelegate = new SendOrPostCallback(PrivateEvaluateUpgradeCompleted);
            evaluateUpgradeThreadStartDelegate = new EvaluateUpgradeThreadStartDelegate(EvaluateUpgradeThreadStart);

            InitializeItemCache(batchList[0].Key.AvailableItems, overrideRegem, overrideReenchant, templateGemsEnabled);

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

        private void InitializeItemCache(List<string> availableItems, bool overrideRegem, bool overrideReenchant, bool templateGemsEnabled)
        {
            if (templateGemsEnabled)
            {
                availableItems = new List<string>(availableItems);
                List<string> templateGems = new List<string>();
                // this could actually be empty, but in practice they will populate it at least once before
                foreach (GemmingTemplate template in GemmingTemplate.AllTemplates[modelList[0].Name])
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
        private delegate void OptimizeBatchThreadStartDelegate(int thoroughness);
        private delegate void ComputeUpgradesThreadStartDelegate(int thoroughness, Item singleItemUpgrades);
        private delegate void EvaluateUpgradeThreadStartDelegate(int thoroughness, ItemInstance upgrade);

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

        public void OptimizeCharacterAsync(int thoroughness)
        {
            if (isBusy) throw new InvalidOperationException("Optimizer is working on another operation.");
            isBusy = true;
            cancellationPending = false;
            asyncOperation = AsyncOperationManager.CreateOperation(null);
            optimizeCharacterThreadStartDelegate.BeginInvoke(thoroughness, null, null);
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

        public void ComputeUpgradesAsync(int thoroughness)
        {
            ComputeUpgradesAsync(thoroughness, null);
        }

        public void ComputeUpgradesAsync(int thoroughness, Item singleItemUpgrades)
        {
            if (isBusy) throw new InvalidOperationException("Optimizer is working on another operation.");
            isBusy = true;
            cancellationPending = false;
            asyncOperation = AsyncOperationManager.CreateOperation(null);
            computeUpgradesThreadStartDelegate.BeginInvoke(thoroughness, singleItemUpgrades, null, null);
        }

        private void ComputeUpgradesThreadStart(int thoroughness, Item singleItemUpgrades)
        {
            Exception error = null;
            Dictionary<Character.CharacterSlot, List<ComparisonCalculationUpgrades>> upgrades = null;
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
            evaluateUpgradeThreadStartDelegate.BeginInvoke(thoroughness, upgrade, null, null);
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

        public Dictionary<Character.CharacterSlot, List<ComparisonCalculationUpgrades>> ComputeUpgrades(int thoroughness, Item singleItemUpgrades)
        {
            if (isBusy) throw new InvalidOperationException("Optimizer is working on another operation.");
            isBusy = true;
            cancellationPending = false;
            asyncOperation = null;
            Exception error;
            Dictionary<Character.CharacterSlot, List<ComparisonCalculationUpgrades>> upgrades = PrivateComputeUpgrades(thoroughness, singleItemUpgrades, out error);
            if (error != null) throw error;
            isBusy = false;
            return upgrades;
        }

        private int itemProgressPercentage = 0;
        private string currentItem = "";

        private Dictionary<Character.CharacterSlot, List<ComparisonCalculationUpgrades>> PrivateComputeUpgrades(int thoroughness, Item singleItemUpgrades, out Exception error)
        {
            if (!itemCacheInitialized) throw new InvalidOperationException("Optimization item cache was not initialized.");
            error = null;
            //_character = character;
            //model = Calculations.GetModel(_character.CurrentModel);
            _thoroughness = thoroughness;

            currentOperation = OptimizationOperation.ComputeUpgrades;
            Dictionary<Character.CharacterSlot, List<ComparisonCalculationUpgrades>> upgrades = null;
            try
            {
                // make equipped gear/enchant valid
                //MarkEquippedItemsAsValid(_character);

                upgrades = new Dictionary<Character.CharacterSlot, List<ComparisonCalculationUpgrades>>();

                Item[] items = ItemCache.GetRelevantItems(modelList[0]);
                Character.CharacterSlot[] slots = new Character.CharacterSlot[] { Character.CharacterSlot.Back, Character.CharacterSlot.Chest, Character.CharacterSlot.Feet, Character.CharacterSlot.Finger1, Character.CharacterSlot.Hands, Character.CharacterSlot.Head, Character.CharacterSlot.Legs, Character.CharacterSlot.MainHand, Character.CharacterSlot.Neck, Character.CharacterSlot.OffHand, Character.CharacterSlot.Projectile, Character.CharacterSlot.ProjectileBag, Character.CharacterSlot.Ranged, Character.CharacterSlot.Shoulders, Character.CharacterSlot.Trinket1, Character.CharacterSlot.Waist, Character.CharacterSlot.Wrist };
                foreach (Character.CharacterSlot slot in slots)
                    upgrades[slot] = new List<ComparisonCalculationUpgrades>();

                slotItems[itemList.Count].Clear();
                upgradeItems = null;

                float baseValue = GetOptimizationValue(startIndividual);

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
                    if (Array.IndexOf(slots, Character.GetCharacterSlotByItemSlot(item.Slot)) >= 0)
                    {
                        ReportProgress(0, 0);
                        upgradeItems = itemGenerator.GetPossibleGemmedItemsForItem(item, item.Id.ToString());
                        PopulateUpgradeItems(item);
                        float best = -10000000f;
                        BatchValuation bestCalculations;
                        BatchIndividual bestCharacter;
                        bool injected;
                        object[] genes = (object[])startIndividual.Items.Clone();
                        genes[itemList.Count] = upgradeItems[0];
                        BatchIndividual batch = GenerateIndividual(genes);
                        bestCharacter = Optimize(batch, GetOptimizationValue(batch), out best, out bestCalculations, out injected);
                        if (best > baseValue)
                        {
                            ItemInstance bestItem = bestCharacter.AvailableItem[itemList.Count];
                            ComparisonCalculationUpgrades itemCalc = new ComparisonCalculationUpgrades();
                            itemCalc.ItemInstance = bestItem;
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

                            foreach (Character.CharacterSlot slot in slots)
                            {
                                if (item.FitsInSlot(slot, batchList[0]))
                                {
                                    upgrades[slot].Add(itemCalc);
                                }
                            }
                        }
                        RemoveUpgradeItems(item);
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
                BatchIndividual batch = GenerateIndividual(genes);
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
        List<Item> itemList;
        List<object>[] slotItemList;
        Dictionary<int, int> indexFromId;

        AvailableItemGenerator itemGenerator;

        private void PopulateUpgradeItems(Item item)
        {
            slotItems[itemList.Count] = upgradeItems.ConvertAll(itemInstance => (object)itemInstance);
            for (int i = 0; i < characterSlots; i++)
            {
                if (item.FitsInSlot((Character.CharacterSlot)i, batchList[0]))
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
                if (item.FitsInSlot((Character.CharacterSlot)i, batchList[0]))
                {
                    for (int c = 0; c < batchList.Count; c++)
                    {
                        List<object> list = slotItems[itemList.Count + 1 + c * characterSlots + i];
                        list.RemoveAt(list.Count - 1);
                    }
                }
            }
        }

        private void PopulateAvailableIds(List<string> availableItems, bool overrideRegem, bool overrideReenchant)
        {
            itemGenerator = new AvailableItemGenerator(availableItems, overrideRegem, overrideReenchant, false, batchList[0], modelList[0]);
            foreach (Item item in ItemCache.Items.Values)
            {
                item.OptimizerItemInformation = null;
            }
            List<ItemInstance>[] slotList = itemGenerator.SlotItems;
            slotItemList = new List<object>[characterSlots];

            Dictionary<int, bool> itemUnique = new Dictionary<int, bool>();
            itemList = new List<Item>();
            indexFromId = new Dictionary<int, int>();
            for (int i = 0; i < slotList.Length; i++)
            {
                Dictionary<int, bool> slotUnique = new Dictionary<int, bool>();
                slotItemList[i] = new List<object>();
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
                        if (!slotUnique.ContainsKey(itemInstance.Id))
                        {
                            slotItemList[i].Add(item);
                            slotUnique[itemInstance.Id] = true;
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

            slotCount = itemList.Count + 1 + batchList.Count * characterSlots;

            slotItems = new List<object>[slotCount];

            for (int i = 0; i < itemList.Count; i++)
            {
                slotItems[i] = itemList[i].OptimizerItemInformation.ItemList.ConvertAll(item => (object)item);
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
                validators.Add(new UniqueItemValidator() { StartSlot = offset + (int)Character.CharacterSlot.Finger1, EndSlot = offset + (int)Character.CharacterSlot.Finger2 });
                validators.Add(new UniqueItemValidator() { StartSlot = offset + (int)Character.CharacterSlot.Trinket1, EndSlot = offset + (int)Character.CharacterSlot.Trinket2 });
                validators.Add(new UniqueItemValidator() { StartSlot = offset + (int)Character.CharacterSlot.MainHand, EndSlot = offset + (int)Character.CharacterSlot.OffHand });
            }

            object[] items = new object[slotCount];
            for (int i = 0; i < itemList.Count; i++)
            {
                items[i] = itemList[i].OptimizerItemInformation.ItemList[0];
            }
            for (int c = 0; c < batchList.Count; c++)
            {
                for (int i = 0; i < characterSlots; i++)
                {
                    ItemInstance itemInstance = batchList[c]._item[i];
                    if (itemInstance != null && itemInstance.Id != 0)
                    {
                        items[indexFromId[itemInstance.Id]] = itemInstance;
                    }
                    items[itemList.Count + 1 + c * characterSlots + i] = (itemInstance == null) ? null : itemInstance.Item;
                }
            }
            startIndividual = new BatchIndividual(items, itemList.Count, indexFromId, null, batchList);

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
            bool blacksmithingSocket = (item.Slot == Item.ItemSlot.Waist && batchList[0].WaistBlacksmithingSocketEnabled) || (item.Slot == Item.ItemSlot.Hands && batchList[0].HandsBlacksmithingSocketEnabled) || (item.Slot == Item.ItemSlot.Wrist && batchList[0].WristBlacksmithingSocketEnabled);
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

        private static float GetCalculationsValue(Character character, CharacterCalculationsBase calcs, string calculation, List<OptimizationRequirement> requirements)
        {
            float gemValue = 0f;
            if (!character.MeetsGemRequirements) gemValue = -100000;
            float ret = 0;
            foreach (OptimizationRequirement requirement in requirements)
            {
                float calcValue = GetCalculationValue(calcs, requirement.Calculation);
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
            else return GetCalculationValue(calcs, calculation) + gemValue;
        }

        private static float GetCalculationValue(CharacterCalculationsBase calcs, string calculation)
        {
            if (calculation == null || calculation == "[Overall]")
                return calcs.OverallPoints;
            else if (calculation.StartsWith("[SubPoint "))
                return calcs.SubPoints[int.Parse(calculation.Substring(10).TrimEnd(']'))];
            else
                return calcs.GetOptimizableCalculationValue(calculation);
        }

        protected override BatchValuation GetValuation(BatchIndividual individual)
        {
            BatchValuation valuation = new BatchValuation();
            valuation.OptimizedBatchValue = new List<float>();
            float value = 0f;
            for (int i = 0; i < batchList.Count; i++)
            {
                float characterValue = GetCalculationsValue(individual.Character[i], modelList[i].GetCharacterCalculations(individual.Character[i]), batchList[i].CalculationToOptimize, batchList[i].OptimizationRequirements);
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

        protected override BatchIndividual GenerateIndividual(object[] items)
        {
            return new BatchIndividual(items, itemList.Count, indexFromId, upgradeItems != null ? upgradeItems[0].Item : null, batchList);
        }
    }
}
