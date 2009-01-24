using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr
{
    public delegate int CompareItemCalculations(ComparisonCalculationBase a, ComparisonCalculationBase b);

    public class DynamicGemmer
    {
        private Character character;
        private Boolean isWorking = false;
        private int newItems = 0;

        protected Item ReplaceGems(Item item, Item gem1, Item gem2, Item gem3)
        {
            // Copied from Optimizer
            // alternatively construct gemmedid and retrieve from cache, trading memory footprint for dictionary access
            Item copy = new Item(item.Name, item.Quality, item.Type, item.Id, item.IconPath, item.Slot,
                item.SetName, item.Unique, item.Stats.Clone(), item.Sockets.Clone(), 0, 0, 0, item.MinDamage,
                item.MaxDamage, item.DamageType, item.Speed, item.RequiredClasses);
            copy.SetGemInternal(1, gem1);
            copy.SetGemInternal(2, gem2);
            copy.SetGemInternal(3, gem3);
            return copy;
        }

        protected int DefaultCompareItemCalculations(ComparisonCalculationBase a, ComparisonCalculationBase b)
        {
            return a.OverallPoints.CompareTo(b.OverallPoints);
        }

        protected void verifyItemInItemCache(Item item)
        {
			Item[] retIrrelevant;
			if (ItemCache.Items.TryGetValue(item.GemmedId, out retIrrelevant))
				return ;
            ItemCache.Items.Add(item.GemmedId, new Item[] { item });
            newItems++;
        }

        protected ComparisonCalculationBase regemBest(Item source, List<Item> regems, Character.CharacterSlot slot, CompareItemCalculations CompareItemCalculations)
        {
            List<ComparisonCalculationBase> itemCalculations = new List<ComparisonCalculationBase>();

            // find number of sockets
            if (source.Sockets.Color1 != Item.ItemSlot.None)
            {
                foreach (Item _gem1 in regems)
                {
                    Item gem1 = _gem1;
                    if (!(gem1.Slot == Item.ItemSlot.Meta ^ source.Sockets.Color1 != Item.ItemSlot.Meta))
                        gem1 = source.Gem1;
                    if (source.Sockets.Color2 != Item.ItemSlot.None)
                    {
                        foreach (Item _gem2 in regems)
                        {
                            Item gem2 = _gem2;
                            if (!(gem2.Slot == Item.ItemSlot.Meta ^ source.Sockets.Color2 != Item.ItemSlot.Meta))
                                gem2 = source.Gem2;
                            if (source.Sockets.Color3 != Item.ItemSlot.None)
                            {
                                foreach (Item _gem3 in regems)
                                {
                                    Item gem3 = _gem3;
                                    if (!(gem3.Slot == Item.ItemSlot.Meta ^ source.Sockets.Color3 != Item.ItemSlot.Meta))
                                        gem3 = source.Gem3;
                                    Item copy = ReplaceGems(source, gem1, gem2, gem3);
                                    itemCalculations.Add(Calculations.GetItemCalculations(copy, character, slot));
                                }
                            }
                            else
                            {
                                Item copy = ReplaceGems(source, gem1, gem2, null);
                                itemCalculations.Add(Calculations.GetItemCalculations(copy, character, slot));
                            }
                        }
                    }
                    else
                    {
                        Item copy = ReplaceGems(source, gem1, null, null);
                        itemCalculations.Add(Calculations.GetItemCalculations(copy, character, slot));
                    }
                }
            }
            else
            {
                Item copy = ReplaceGems(source, null, null, null);
                itemCalculations.Add(Calculations.GetItemCalculations(copy, character, slot));
            }
            itemCalculations.Sort(new System.Comparison<ComparisonCalculationBase>(CompareItemCalculations));
            if (itemCalculations.Count == 0) return null;
            ComparisonCalculationBase best = itemCalculations[itemCalculations.Count - 1];
            verifyItemInItemCache(best.Item);
            return best;
        }

        public List<ComparisonCalculationBase> LoadItemsBySlot(Character.CharacterSlot slot, Item[] items, Character _character, Boolean addEmpty, CompareItemCalculations CompareItemCalculations)
        {
            if (isWorking) return new List<ComparisonCalculationBase>();
            isWorking = true;
            newItems = 0;

            character = _character;
            if (CompareItemCalculations == null) CompareItemCalculations = DefaultCompareItemCalculations;

            Item currentItem = character[slot];
            List<Item> gems = character.GetAvailableGems(items);
            Boolean deleteDuplicates = gems.Count > 0;

            List<int> seenItems = new List<int>();
            List<ComparisonCalculationBase> itemCalculations = new List<ComparisonCalculationBase>();
            if (items != null && character != null)
            {
                if (currentItem != null)
                {
                    seenItems.Add(currentItem.Id);
                    itemCalculations.Add(Calculations.GetItemCalculations(currentItem, character, slot));
                    ComparisonCalculationBase bestRegemmed = regemBest(currentItem, gems, slot, CompareItemCalculations);
                    if (bestRegemmed != null && currentItem != null &&
                        !bestRegemmed.Item.GemmedId.Equals(currentItem.GemmedId))
                        itemCalculations.Add(bestRegemmed);
                }
                foreach (Item item in items)
                {
                    if (item.FitsInSlot(slot, character))
                    {
                        if (deleteDuplicates && seenItems.Contains(item.Id)) continue;
                        if (!deleteDuplicates && currentItem != null && currentItem.GemmedId.Equals(item.GemmedId)) continue;
                        seenItems.Add(item.Id);

                        ComparisonCalculationBase bestRegemmed = null;
                        if (item.Sockets.Color1 != Item.ItemSlot.None)
                            bestRegemmed = regemBest(item, gems, slot, CompareItemCalculations);

                        if (bestRegemmed == null)
                            bestRegemmed = Calculations.GetItemCalculations(item, character, slot);

                        itemCalculations.Add(bestRegemmed);
                    }
                }
            }

            if (addEmpty)
            {
                ComparisonCalculationBase emptyCalcs = Calculations.CreateNewComparisonCalculation();
                emptyCalcs.Name = "Empty";
                emptyCalcs.Item = new Item();
                emptyCalcs.Item.Name = "Empty";
                emptyCalcs.Equipped = currentItem == null;
                itemCalculations.Add(emptyCalcs);
            }

            // assume there are new items
            if (newItems>0) ItemCache.OnItemsChanged();

            itemCalculations.Sort(new System.Comparison<ComparisonCalculationBase>(CompareItemCalculations));
            isWorking = false;
            return itemCalculations;
        }
    }
}
