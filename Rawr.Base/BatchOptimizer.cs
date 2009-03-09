using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Threading;

namespace Rawr.Optimizer
{
    public delegate void BatchOptimizeCharacterCompletedEventHandler(object sender, BatchOptimizeCharacterCompletedEventArgs e);

    public class BatchOptimizeCharacterCompletedEventArgs : AsyncCompletedEventArgs
    {
        private BatchIndividual optimizedCharacter;
        private float optimizedCharacterValue;

        public BatchOptimizeCharacterCompletedEventArgs(BatchIndividual optimizedCharacter, float optimizedCharacterValue, Exception error, bool cancelled)
            : base(error, cancelled, null)
        {
            this.optimizedCharacter = optimizedCharacter;
            this.optimizedCharacterValue = optimizedCharacterValue;
        }

        public BatchIndividual OptimizedCharacter
        {
            get
            {
                RaiseExceptionIfNecessary();
                return optimizedCharacter;
            }
        }

        public float OptimizedCharacterValue
        {
            get
            {
                RaiseExceptionIfNecessary();
                return optimizedCharacterValue;
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
        private Character _character;
        private string _calculationToOptimize;
        private OptimizationRequirement[] _requirements;
        private CalculationsBase model;

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

        private class ItemAvailableValidator : OptimizerRangeValidatorBase<object>
        {
            private BatchOptimizer optimizer;

            public override bool IsValid(object[] items)
            {
                Item item;
                int index = StartSlot / 4;
                if (index == optimizer.itemList.Count && optimizer.upgradeItems != null)
                {
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
            this.batchList = new List<BatchCharacter>();
            foreach (KeyValuePair<Character, float> kvp in batchList)
            {
                this.batchList.Add(new BatchCharacter() { Character = kvp.Key, Model = Calculations.GetModel(kvp.Key.CurrentModel), Weight = kvp.Value });
            }

            optimizer = new ItemInstanceOptimizer();
            optimizer.OptimizeCharacterCompleted += new OptimizeCharacterCompletedEventHandler(optimizer_OptimizeCharacterCompleted);
            optimizer.OptimizeCharacterProgressChanged += new OptimizeCharacterProgressChangedEventHandler(optimizer_OptimizeCharacterProgressChanged);
            optimizerComplete = new AutoResetEvent(false);

            optimizeCharacterProgressChangedDelegate = new SendOrPostCallback(PrivateOptimizeCharacterProgressChanged);
            optimizeCharacterCompletedDelegate = new SendOrPostCallback(PrivateOptimizeCharacterCompleted);
            optimizeCharacterThreadStartDelegate = new OptimizeCharacterThreadStartDelegate(OptimizeCharacterThreadStart);
            computeUpgradesProgressChangedDelegate = new SendOrPostCallback(PrivateComputeUpgradesProgressChanged);
            computeUpgradesCompletedDelegate = new SendOrPostCallback(PrivateComputeUpgradesCompleted);
            computeUpgradesThreadStartDelegate = new ComputeUpgradesThreadStartDelegate(ComputeUpgradesThreadStart);
            evaluateUpgradeProgressChangedDelegate = new SendOrPostCallback(PrivateEvaluateUpgradeProgressChanged);
            evaluateUpgradeCompletedDelegate = new SendOrPostCallback(PrivateEvaluateUpgradeCompleted);
            evaluateUpgradeThreadStartDelegate = new EvaluateUpgradeThreadStartDelegate(EvaluateUpgradeThreadStart);
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
            _character = character;
            this.model = model;

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
        private void PrivateOptimizeCharacterProgressChanged(object state)
        {
            OnOptimizeCharacterProgressChanged(state as OptimizeCharacterProgressChangedEventArgs);
        }

        protected void OnOptimizeCharacterProgressChanged(OptimizeCharacterProgressChangedEventArgs e)
        {
            if (OptimizeCharacterProgressChanged != null)
            {
                OptimizeCharacterProgressChanged(this, e);
            }
        }

        private void PrivateOptimizeCharacterCompleted(object state)
        {
            isBusy = false;
            cancellationPending = false;
            OnOptimizeCharacterCompleted(state as BatchOptimizeCharacterCompletedEventArgs);
        }

        protected void OnOptimizeCharacterCompleted(BatchOptimizeCharacterCompletedEventArgs e)
        {
            if (OptimizeCharacterCompleted != null)
            {
                OptimizeCharacterCompleted(this, e);
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
        private delegate void OptimizeCharacterThreadStartDelegate(Character character, string calculationToOptimize, OptimizationRequirement[] requirements, int thoroughness, bool injectCharacter);
        private delegate void ComputeUpgradesThreadStartDelegate(Character character, string calculationToOptimize, OptimizationRequirement[] requirements, int thoroughness, Item singleItemUpgrades);
        private delegate void EvaluateUpgradeThreadStartDelegate(Character character, string calculationToOptimize, OptimizationRequirement[] requirements, int thoroughness, ItemInstance upgrade);

        public event BatchOptimizeCharacterCompletedEventHandler OptimizeCharacterCompleted;
        public event OptimizeCharacterProgressChangedEventHandler OptimizeCharacterProgressChanged;
        public event ComputeUpgradesProgressChangedEventHandler ComputeUpgradesProgressChanged;
        public event ComputeUpgradesCompletedEventHandler ComputeUpgradesCompleted;
        public event ProgressChangedEventHandler EvaluateUpgradeProgressChanged;
        public event EvaluateUpgradeCompletedEventHandler EvaluateUpgradeCompleted;

        private SendOrPostCallback optimizeCharacterProgressChangedDelegate;
        private SendOrPostCallback optimizeCharacterCompletedDelegate;
        private OptimizeCharacterThreadStartDelegate optimizeCharacterThreadStartDelegate;
        private SendOrPostCallback computeUpgradesProgressChangedDelegate;
        private SendOrPostCallback computeUpgradesCompletedDelegate;
        private ComputeUpgradesThreadStartDelegate computeUpgradesThreadStartDelegate;
        private SendOrPostCallback evaluateUpgradeProgressChangedDelegate;
        private SendOrPostCallback evaluateUpgradeCompletedDelegate;
        private EvaluateUpgradeThreadStartDelegate evaluateUpgradeThreadStartDelegate;

        public void OptimizeCharacterAsync(Character character, string calculationToOptimize, OptimizationRequirement[] requirements, int thoroughness, bool injectCharacter)
        {
            if (isBusy) throw new InvalidOperationException("Optimizer is working on another operation.");
            isBusy = true;
            cancellationPending = false;
            asyncOperation = AsyncOperationManager.CreateOperation(null);
            optimizeCharacterThreadStartDelegate.BeginInvoke(character, calculationToOptimize, requirements, thoroughness, injectCharacter, null, null);
        }

        private void OptimizeCharacterThreadStart(Character character, string calculationToOptimize, OptimizationRequirement[] requirements, int thoroughness, bool injectCharacter)
        {
            Exception error = null;
            BatchIndividual optimizedCharacter = null;
            float optimizedCharacterValue = 0.0f;
            try
            {
                optimizedCharacter = PrivateOptimizeCharacter(character, calculationToOptimize, requirements, thoroughness, out error);
                if (optimizedCharacter != null)
                {
                    optimizedCharacterValue = GetOptimizationValue(optimizedCharacter);
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
            asyncOperation.PostOperationCompleted(optimizeCharacterCompletedDelegate, new BatchOptimizeCharacterCompletedEventArgs(optimizedCharacter, optimizedCharacterValue, error, cancellationPending));
        }

        public void ComputeUpgradesAsync(Character character, string calculationToOptimize, OptimizationRequirement[] requirements, int thoroughness)
        {
            ComputeUpgradesAsync(character, calculationToOptimize, requirements, thoroughness, null);
        }

        public void ComputeUpgradesAsync(Character character, string calculationToOptimize, OptimizationRequirement[] requirements, int thoroughness, Item singleItemUpgrades)
        {
            if (isBusy) throw new InvalidOperationException("Optimizer is working on another operation.");
            isBusy = true;
            cancellationPending = false;
            asyncOperation = AsyncOperationManager.CreateOperation(null);
            computeUpgradesThreadStartDelegate.BeginInvoke(character, calculationToOptimize, requirements, thoroughness, singleItemUpgrades, null, null);
        }

        private void ComputeUpgradesThreadStart(Character character, string calculationToOptimize, OptimizationRequirement[] requirements, int thoroughness, Item singleItemUpgrades)
        {
            Exception error = null;
            Dictionary<Character.CharacterSlot, List<ComparisonCalculationBase>> upgrades = null;
            try
            {
                upgrades = PrivateComputeUpgrades(character, calculationToOptimize, requirements, thoroughness, singleItemUpgrades, out error);
            }
            catch (Exception ex)
            {
                error = ex;
            }
            asyncOperation.PostOperationCompleted(computeUpgradesCompletedDelegate, new ComputeUpgradesCompletedEventArgs(upgrades, error, cancellationPending));
        }

        public void EvaluateUpgradeAsync(Character character, string calculationToOptimize, OptimizationRequirement[] requirements, int thoroughness, ItemInstance upgrade)
        {
            if (isBusy) throw new InvalidOperationException("Optimizer is working on another operation.");
            isBusy = true;
            cancellationPending = false;
            asyncOperation = AsyncOperationManager.CreateOperation(null);
            evaluateUpgradeThreadStartDelegate.BeginInvoke(character, calculationToOptimize, requirements, thoroughness, upgrade, null, null);
        }

        private void EvaluateUpgradeThreadStart(Character character, string calculationToOptimize, OptimizationRequirement[] requirements, int thoroughness, ItemInstance upgrade)
        {
            Exception error = null;
            float upgradeValue = 0f;
            try
            {
                upgradeValue = PrivateEvaluateUpgrade(character, calculationToOptimize, requirements, thoroughness, upgrade, out error);
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

        public BatchIndividual OptimizeCharacter(Character character, string calculationToOptimize, OptimizationRequirement[] requirements, int thoroughness, bool injectCharacter)
        {
            if (isBusy) throw new InvalidOperationException("Optimizer is working on another operation.");
            isBusy = true;
            cancellationPending = false;
            asyncOperation = null;
            Exception error;
            BatchIndividual optimizedCharacter = PrivateOptimizeCharacter(character, calculationToOptimize, requirements, thoroughness, out error);
            if (error != null) throw error;
            isBusy = false;
            return optimizedCharacter;
        }

        private BatchIndividual PrivateOptimizeCharacter(Character character, string calculationToOptimize, OptimizationRequirement[] requirements, int thoroughness, out Exception error)
        {
            if (!itemCacheInitialized) throw new InvalidOperationException("Optimization item cache was not initialized.");
            error = null;
            _character = character;
            model = Calculations.GetModel(_character.CurrentModel);
            _calculationToOptimize = calculationToOptimize;
            _requirements = requirements;
            _thoroughness = thoroughness;

            currentOperation = OptimizationOperation.OptimizeCharacter;
            BatchIndividual optimizedCharacter = null;
            float bestValue = 0.0f;
            upgradeItems = null;

            try
            {
                if (_thoroughness == 1)
                {
                    // if we just start from current character and look for direct upgrades
                    // then we have to deal with items that are currently equipped, but are not
                    // currently available
                    MarkEquippedItemsAsValid(_character);
                }

                slotCount = itemList.Count * 4;
                optimizedCharacter = Optimize(out bestValue);
            }
            catch (Exception ex)
            {
                error = ex;
            }

            ReportProgress(100, bestValue);
            return optimizedCharacter;
        }

        public Dictionary<Character.CharacterSlot, List<ComparisonCalculationBase>> ComputeUpgrades(Character character, string calculationToOptimize, OptimizationRequirement[] requirements, int thoroughness, Item singleItemUpgrades)
        {
            if (isBusy) throw new InvalidOperationException("Optimizer is working on another operation.");
            isBusy = true;
            cancellationPending = false;
            asyncOperation = null;
            Exception error;
            Dictionary<Character.CharacterSlot, List<ComparisonCalculationBase>> upgrades = PrivateComputeUpgrades(character, calculationToOptimize, requirements, thoroughness, singleItemUpgrades, out error);
            if (error != null) throw error;
            isBusy = false;
            return upgrades;
        }

        private int itemProgressPercentage = 0;
        private string currentItem = "";

        private void MarkEquippedItemsAsValid(Character character)
        {
            for (int i = 0; i < slotCount; i++)
            {
                ItemInstance item = character[(Character.CharacterSlot)i];
                if ((object)item != null && item.Id != 0)
                {
                    Item it = item.Item;
                    if (!itemList.Contains(it))
                    {
                        itemList.Add(it);
                    }
                    if (it.OptimizerItemInformation == null)
                    {
                        GenerateOptimizerItemInformation(it);
                    }
                    if (!it.OptimizerItemInformation.ItemAvailable.ContainsKey(item.GemmedId))
                    {
                        it.OptimizerItemInformation.ItemList.Add(item);
                        it.OptimizerItemInformation.ItemAvailable[item.GemmedId] = true;
                    }
                }
            }
        }

        private Dictionary<Character.CharacterSlot, List<ComparisonCalculationBase>> PrivateComputeUpgrades(Character character, string calculationToOptimize, OptimizationRequirement[] requirements, int thoroughness, Item singleItemUpgrades, out Exception error)
        {
            if (!itemCacheInitialized) throw new InvalidOperationException("Optimization item cache was not initialized.");
            error = null;
            _character = character;
            model = Calculations.GetModel(_character.CurrentModel);
            _calculationToOptimize = calculationToOptimize;
            _requirements = requirements;
            _thoroughness = thoroughness;

            currentOperation = OptimizationOperation.ComputeUpgrades;
            Dictionary<Character.CharacterSlot, List<ComparisonCalculationBase>> upgrades = null;
            try
            {
                // make equipped gear/enchant valid
                MarkEquippedItemsAsValid(_character);

                upgrades = new Dictionary<Character.CharacterSlot, List<ComparisonCalculationBase>>();

                Item[] items = ItemCache.GetRelevantItems(model);
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
                        if (item.FitsInSlot(slot, _character))
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

        public float EvaluateUpgrade(Character character, string calculationToOptimize, OptimizationRequirement[] requirements, int thoroughness, ItemInstance upgrade)
        {
            if (isBusy) throw new InvalidOperationException("Optimizer is working on another operation.");
            isBusy = true;
            cancellationPending = false;
            asyncOperation = null;
            Exception error;
            float upgradeValue = PrivateEvaluateUpgrade(character, calculationToOptimize, requirements, thoroughness, upgrade, out error);
            if (error != null) throw error;
            isBusy = false;
            return upgradeValue;
        }

        private float PrivateEvaluateUpgrade(Character character, string calculationToOptimize, OptimizationRequirement[] requirements, int thoroughness, ItemInstance upgrade, out Exception error)
        {
            if (!itemCacheInitialized) throw new InvalidOperationException("Optimization item cache was not initialized.");
            error = null;
            _character = character;
            model = Calculations.GetModel(_character.CurrentModel);
            _calculationToOptimize = calculationToOptimize;
            _requirements = requirements;
            _thoroughness = thoroughness;

            slotCount = (itemList.Count + 1) * 4; // one extra slot for upgrade evaluations

            currentOperation = OptimizationOperation.EvaluateUpgrade;
            Character saveCharacter = _character;
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
            itemGenerator = new AvailableItemGenerator(availableItems, overrideRegem, overrideReenchant, _character, model);
            foreach (Item item in ItemCache.Items.Values)
            {
                item.OptimizerItemInformation = null;
            }
            List<ItemInstance>[] slotList = itemGenerator.SlotItems;

            Dictionary<int, bool> itemUnique = new Dictionary<int, bool>();
            itemList = new List<Item>();
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
            bool blacksmithingSocket = (item.Slot == Item.ItemSlot.Waist && _character.WaistBlacksmithingSocketEnabled) || (item.Slot == Item.ItemSlot.Hands && _character.HandsBlacksmithingSocketEnabled) || (item.Slot == Item.ItemSlot.Wrist && _character.WristBlacksmithingSocketEnabled);
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
                optimizer.OptimizeCharacterAsync(batchList[i].Character, _calculationToOptimize, _requirements, _thoroughness, false);
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
