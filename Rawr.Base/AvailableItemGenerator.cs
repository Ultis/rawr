using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Optimizer
{
    public class ItemAvailabilityInformation
    {
        public int GemCount;
        public Dictionary<string, bool> ItemAvailable = new Dictionary<string, bool>();
        public List<ItemInstance> ItemList = new List<ItemInstance>();
    }

    /// <summary>
    /// Provides means to generate available items based on available item strings from character.
    /// </summary>
    public class AvailableItemGenerator
    {
        private Dictionary<string, bool> itemAvailable = new Dictionary<string, bool>();
        private List<string> availableItems;
        private bool overrideRegem;
        private bool overrideReenchant;
        private bool slotFiltering;
        private Character character;
        private CalculationsBase model;

        private const int slotCount = 19;
        private List<ItemInstance>[] slotItems = new List<ItemInstance>[slotCount];
        private Item[] metaGemItems;
        private Item[] gemItems;
        private Enchant[][] slotAvailableEnchants = new Enchant[slotCount][];

        public Dictionary<string, bool> ItemAvailable
        {
            get
            {
                return itemAvailable;
            }
        }

        public List<ItemInstance>[] SlotItems
        {
            get
            {
                return slotItems;
            }
        }

        public Item[] MetaGemItems
        {
            get
            {
                return metaGemItems;
            }
        }

        public Item[] GemItems
        {
            get
            {
                return gemItems;
            }
        }

        public AvailableItemGenerator(List<string> availableItems, bool overrideRegem, bool overrideReenchant, bool slotFiltering, Character character, CalculationsBase model)
        {
            this.availableItems = availableItems;
            this.overrideRegem = overrideRegem;
            this.overrideReenchant = overrideReenchant;
            this.slotFiltering = slotFiltering;
            this.character = character;
            this.model = model;

            bool oldVolatility = Item.OptimizerManagedVolatiliy;
            try
            {
                Item.OptimizerManagedVolatiliy = true;
                PopulateAvailableIds();
            }
            finally
            {
                Item.OptimizerManagedVolatiliy = oldVolatility;
            }
        }

        public int GetItemGemCount(Item item)
        {
            int gemCount = 0;
            bool blacksmithingSocket = (item.Slot == Item.ItemSlot.Waist && character.WaistBlacksmithingSocketEnabled) || (item.Slot == Item.ItemSlot.Hands && character.HandsBlacksmithingSocketEnabled) || (item.Slot == Item.ItemSlot.Wrist && character.WristBlacksmithingSocketEnabled);
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

        public void GenerateItemAvailabilityInformation(Item item)
        {
            item.AvailabilityInformation = new ItemAvailabilityInformation();
            item.AvailabilityInformation.GemCount = GetItemGemCount(item);
        }

        private void PopulateAvailableIds()
        {
            foreach (Item citem in ItemCache.Items.Values)
            {
                citem.AvailabilityInformation = null;
            }

            List<string> itemIds = new List<string>(availableItems);
            List<string> removeIds = new List<string>();
            List<Item> metaGemItemList = new List<Item>();
            List<Item> gemItemList = new List<Item>();
            foreach (string xid in availableItems)
            {
                int dot = xid.IndexOf('.');
                int id = int.Parse((dot >= 0) ? xid.Substring(0, dot) : xid);
                if (id > 0)
                {
                    Item availableItem;
                    availableItem = ItemCache.FindItemById(id);
                    if (availableItem != null)
                    {
                        switch (availableItem.Slot)
                        {
                            case Item.ItemSlot.Meta:
                                metaGemItemList.Add(availableItem);
                                removeIds.Add(xid);
                                break;
                            case Item.ItemSlot.Red:
                            case Item.ItemSlot.Orange:
                            case Item.ItemSlot.Yellow:
                            case Item.ItemSlot.Green:
                            case Item.ItemSlot.Blue:
                            case Item.ItemSlot.Purple:
                            case Item.ItemSlot.Prismatic:
                                gemItemList.Add(availableItem);
                                removeIds.Add(xid);
                                break;
                        }
                    }
                }
            }
            if (gemItemList.Count == 0) gemItemList.Add(null);
            if (metaGemItemList.Count == 0) metaGemItemList.Add(null);
            itemIds.RemoveAll(x => x.StartsWith("-") || removeIds.Contains(x));

            metaGemItems = metaGemItemList.ToArray();
            gemItems = FilterList(gemItemList);

            for (int i = 0; i < slotCount; i++)
            {
                slotItems[i] = new List<ItemInstance>();
            }

            slotAvailableEnchants[(int)Character.CharacterSlot.Back] = FilterList(Enchant.FindEnchants(Item.ItemSlot.Back, character, availableItems, model));
            slotAvailableEnchants[(int)Character.CharacterSlot.Chest] = FilterList(Enchant.FindEnchants(Item.ItemSlot.Chest, character, availableItems, model));
            slotAvailableEnchants[(int)Character.CharacterSlot.Feet] = FilterList(Enchant.FindEnchants(Item.ItemSlot.Feet, character, availableItems, model));
            slotAvailableEnchants[(int)Character.CharacterSlot.Finger1] = slotAvailableEnchants[(int)Character.CharacterSlot.Finger2] = FilterList(Enchant.FindEnchants(Item.ItemSlot.Finger, character, availableItems, model));
            slotAvailableEnchants[(int)Character.CharacterSlot.Hands] = FilterList(Enchant.FindEnchants(Item.ItemSlot.Hands, character, availableItems, model));
            slotAvailableEnchants[(int)Character.CharacterSlot.Head] = FilterList(Enchant.FindEnchants(Item.ItemSlot.Head, character, availableItems, model));
            slotAvailableEnchants[(int)Character.CharacterSlot.Legs] = FilterList(Enchant.FindEnchants(Item.ItemSlot.Legs, character, availableItems, model));
            slotAvailableEnchants[(int)Character.CharacterSlot.Shoulders] = FilterList(Enchant.FindEnchants(Item.ItemSlot.Shoulders, character, availableItems, model));
            slotAvailableEnchants[(int)Character.CharacterSlot.MainHand] = FilterList(Enchant.FindEnchants(Item.ItemSlot.MainHand, character, availableItems, model));
            slotAvailableEnchants[(int)Character.CharacterSlot.OffHand] = FilterList(Enchant.FindEnchants(Item.ItemSlot.OffHand, character, availableItems, model));
            slotAvailableEnchants[(int)Character.CharacterSlot.Ranged] = FilterList(Enchant.FindEnchants(Item.ItemSlot.Ranged, character, availableItems, model));
            slotAvailableEnchants[(int)Character.CharacterSlot.Wrist] = FilterList(Enchant.FindEnchants(Item.ItemSlot.Wrist, character, availableItems, model));

            Item item = null;
            List<ItemInstance> possibleGemmedItems = null;
            List<string> gemmedIds = new List<string>();
            Dictionary<string, Dictionary<string, bool>> gemmedIdMap = new Dictionary<string, Dictionary<string, bool>>();
            foreach (string xid in itemIds)
            {
                int dot = xid.LastIndexOf('.');
                int dot2 = xid.IndexOf('.');
                string id = ((dot2 >= 0) ? xid.Substring(0, dot2) : xid);
                Dictionary<string, bool> map;
                if (!gemmedIdMap.TryGetValue(id, out map))
                {
                    map = new Dictionary<string, bool>();
                    gemmedIdMap[id] = map;
                }
                string gemmedId = (dot >= 0) ? xid.Substring(0, dot) : (xid + ".*.*.*");
                string restriction = (dot >= 0) ? xid.Substring(dot + 1) : "*";
                map[gemmedId + "." + restriction] = true;
                if (overrideReenchant) map[gemmedId + ".*"] = true;

                if (overrideRegem)
                {
                    gemmedId = id + ".*.*.*";
                    map[gemmedId + "." + restriction] = true;
                    if (overrideReenchant) map[gemmedId + ".*"] = true;
                }
            }
            foreach (KeyValuePair<string, Dictionary<string, bool>> keyMap in gemmedIdMap)
            {
                int itemId = int.Parse(keyMap.Key);
                item = ItemCache.FindItemById(itemId);

                // disallow non-equippable items, this can happen for example when loading from character profiler

                if (item != null && model.RelevantItemTypes.Contains(item.Type))
                {
                    int slot = (int)Character.GetCharacterSlotByItemSlot(item.Slot);
                    if (slot < 0 || slot >= slotCount) continue;

                    if (item.AvailabilityInformation == null)
                    {
                        GenerateItemAvailabilityInformation(item);
                    }

                    gemmedIds = new List<string>(keyMap.Value.Keys);
                    Dictionary<string, bool> uniqueStore = new Dictionary<string, bool>();
                    possibleGemmedItems = new List<ItemInstance>();
                    foreach (string gid in gemmedIds)
                    {
                        foreach (ItemInstance gemmedItem in GetPossibleGemmedItemsForItem(item, gid, item.AvailabilityInformation))
                        {
                            if (!uniqueStore.ContainsKey(gemmedItem.GemmedId))
                            {
                                possibleGemmedItems.Add(gemmedItem);
                                uniqueStore.Add(gemmedItem.GemmedId, true);
                            }
                        }
                    }
                    foreach (KeyValuePair<string, bool> kvp in item.AvailabilityInformation.ItemAvailable)
                    {
                        itemAvailable[kvp.Key] = kvp.Value;
                    }
                    possibleGemmedItems = FilterList(possibleGemmedItems, false);
                    item.AvailabilityInformation.ItemList = possibleGemmedItems;
                    for (int i = 0; i < slotCount; i++)
                    {
                        if (item.FitsInSlot((Character.CharacterSlot)i, character))
                        {
                            slotItems[i].AddRange(possibleGemmedItems);
                        }
                    }
                }
            }

            for (int i = 0; i < slotCount; i++)
            {
                Character.CharacterSlot slot = (Character.CharacterSlot)i;
                if (slot == Character.CharacterSlot.Finger1 || slot == Character.CharacterSlot.Finger2 || slot == Character.CharacterSlot.Trinket1 || slot == Character.CharacterSlot.Trinket2 || slot == Character.CharacterSlot.MainHand || slot == Character.CharacterSlot.OffHand || slotItems[i].Count == 0)
                {
                    slotItems[i].Add(null);
                }
            }

            if (slotFiltering)
            {
                for (int i = 0; i < slotCount; i++)
                {
                    Character.CharacterSlot slot = (Character.CharacterSlot)i;
                    if (slot != Character.CharacterSlot.Finger1 && slot != Character.CharacterSlot.Finger2 && slot != Character.CharacterSlot.Trinket1 && slot != Character.CharacterSlot.Trinket2)
                    {
                        slotItems[i] = FilterList(slotItems[i], true);
                    }
                }
            }
        }

        public List<ItemInstance> GetPossibleGemmedItemsForItem(Item item, string gemmedId)
        {
            return GetPossibleGemmedItemsForItem(item, gemmedId, null);
        }

        public List<ItemInstance> GetPossibleGemmedItemsForItem(Item item, string gemmedId, ItemAvailabilityInformation availability)
        {
            List<ItemInstance> possibleGemmedItems = new List<ItemInstance>();
            string[] ids = gemmedId.Split('.');
            Item[] possibleGem1s, possibleGem2s, possibleGem3s = null;
            Enchant[] possibleEnchants = null;
            bool blacksmithingSocket = (item.Slot == Item.ItemSlot.Waist && character.WaistBlacksmithingSocketEnabled) || (item.Slot == Item.ItemSlot.Hands && character.HandsBlacksmithingSocketEnabled) || (item.Slot == Item.ItemSlot.Wrist && character.WristBlacksmithingSocketEnabled);

            if (ids.Length <= 1 || (ids.Length > 1 && ids[1] == "*"))
            {
                switch (item.SocketColor1)
                {
                    case Item.ItemSlot.Meta:
                        possibleGem1s = metaGemItems;
                        break;
                    case Item.ItemSlot.Red:
                    case Item.ItemSlot.Orange:
                    case Item.ItemSlot.Yellow:
                    case Item.ItemSlot.Green:
                    case Item.ItemSlot.Blue:
                    case Item.ItemSlot.Purple:
                    case Item.ItemSlot.Prismatic:
                        possibleGem1s = gemItems;
                        break;
                    default:
                        if (blacksmithingSocket)
                        {
                            possibleGem1s = gemItems;
                            blacksmithingSocket = false;
                        }
                        else
                        {
                            possibleGem1s = new Item[] { null };
                        }
                        break;
                }
            }
            else
            {
                possibleGem1s = new Item[] { ItemCache.FindItemById(int.Parse(ids[1])) };
            }

            if (ids.Length <= 2 || (ids.Length > 2 && ids[2] == "*"))
            {
                switch (item.SocketColor2)
                {
                    case Item.ItemSlot.Meta:
                        possibleGem2s = metaGemItems;
                        break;
                    case Item.ItemSlot.Red:
                    case Item.ItemSlot.Orange:
                    case Item.ItemSlot.Yellow:
                    case Item.ItemSlot.Green:
                    case Item.ItemSlot.Blue:
                    case Item.ItemSlot.Purple:
                    case Item.ItemSlot.Prismatic:
                        possibleGem2s = gemItems;
                        break;
                    default:
                        if (blacksmithingSocket)
                        {
                            possibleGem2s = gemItems;
                            blacksmithingSocket = false;
                        }
                        else
                        {
                            possibleGem2s = new Item[] { null };
                        }
                        break;
                }
            }
            else
            {
                possibleGem2s = new Item[] { ItemCache.FindItemById(int.Parse(ids[2])) };
            }

            if (ids.Length <= 3 || (ids.Length > 3 && ids[3] == "*"))
            {
                switch (item.SocketColor3)
                {
                    case Item.ItemSlot.Meta:
                        possibleGem3s = metaGemItems;
                        break;
                    case Item.ItemSlot.Red:
                    case Item.ItemSlot.Orange:
                    case Item.ItemSlot.Yellow:
                    case Item.ItemSlot.Green:
                    case Item.ItemSlot.Blue:
                    case Item.ItemSlot.Purple:
                    case Item.ItemSlot.Prismatic:
                        possibleGem3s = gemItems;
                        break;
                    default:
                        if (blacksmithingSocket)
                        {
                            possibleGem3s = gemItems;
                            blacksmithingSocket = false;
                        }
                        else
                        {
                            possibleGem3s = new Item[] { null };
                        }
                        break;
                }
            }
            else
            {
                possibleGem3s = new Item[] { ItemCache.FindItemById(int.Parse(ids[3])) };
            }

            if (ids.Length <= 4 || (ids.Length > 4 && ids[4] == "*"))
            {
                int slotIndex = (int)Character.GetCharacterSlotByItemSlot(item.Slot);
                if (slotIndex < slotAvailableEnchants.Length)
                    possibleEnchants = slotAvailableEnchants[slotIndex];
                if (possibleEnchants == null) possibleEnchants = new Enchant[] { null };
            }
            else
            {
                possibleEnchants = new Enchant[] { Enchant.FindEnchant(int.Parse(ids[4]), item.Slot, null) };
            }

            foreach (Item gem1 in possibleGem1s)
                foreach (Item gem2 in possibleGem2s)
                    foreach (Item gem3 in possibleGem3s)
                        foreach (Enchant enchant in possibleEnchants)
                        {
                            if (availability != null)
                            {
                                // any combination is actually available
                                gemmedId = string.Format("{0}.{1}.{2}.{3}.{4}", item.Id, gem1 != null ? gem1.Id : 0, gem2 != null ? gem2.Id : 0, gem3 != null ? gem3.Id : 0, enchant != null ? enchant.Id : 0);
                                availability.ItemAvailable[gemmedId] = true;
                            }
                            // if gems do not match socket colors then it does not matter in what order they are placed (except for meta)
                            // make it easy on filtering and only add one canon version of the item in which normal gem ids are nondecreasing
                            // if that happens to be an ordering that matches colors it doesn't matter since socket bonuses only add value
                            // either all gems are * or all are specified, obviously if all are specified we can't do this
                            bool add = true;
                            if (!Item.GemMatchesSlot(gem1, item.SocketColor1) || !Item.GemMatchesSlot(gem2, item.SocketColor2) || !Item.GemMatchesSlot(gem3, item.SocketColor3))
                            {
                                if ((ids.Length <= 1 || (ids.Length > 1 && ids[1] == "*")) && (ids.Length <= 2 || (ids.Length > 2 && ids[2] == "*")) && (ids.Length <= 3 || (ids.Length > 3 && ids[3] == "*")))
                                {
                                    List<int> gemOrder = new List<int>();
                                    if (gem1 != null && gem1.Slot != Item.ItemSlot.Meta) gemOrder.Add(gem1.Id);
                                    if (gem2 != null && gem2.Slot != Item.ItemSlot.Meta) gemOrder.Add(gem2.Id);
                                    if (gem3 != null && gem3.Slot != Item.ItemSlot.Meta) gemOrder.Add(gem3.Id);
                                    for (int i = 0; i < gemOrder.Count - 1; i++)
                                    {
                                        if (gemOrder[i] > gemOrder[i + 1])
                                        {
                                            add = false;
                                            break;
                                        }
                                    }
                                }
                            }
                            if (add)
                            {
                                possibleGemmedItems.Add(new ItemInstance(item, gem1, gem2, gem3, enchant));
                            }
                        }

            return possibleGemmedItems;
        }

        public static List<Buff> FilterList(List<Buff> unfilteredList)
        {
            List<Buff> filteredList = new List<Buff>();
            foreach (Buff buff in unfilteredList)
            {
                if (buff == null)
                {
                    filteredList.Add(buff);
                    continue;
                }

                bool addBuff = true;
                List<Buff> removeBuffs = new List<Buff>();
                foreach (Buff buff2 in filteredList)
                {
                    ArrayUtils.CompareResult compare = buff.Stats.CompareTo(buff2.Stats);
                    if (compare == ArrayUtils.CompareResult.GreaterThan) //A>B
                    {
                        removeBuffs.Add(buff2);
                    }
                    else if (compare == ArrayUtils.CompareResult.Equal || compare == ArrayUtils.CompareResult.LessThan)
                    {
                        addBuff = false;
                        break;
                    }
                }
                foreach (Buff removeBuff in removeBuffs)
                    filteredList.Remove(removeBuff);
                if (addBuff) filteredList.Add(buff);
            }
            return filteredList;
        }

        public static Enchant[] FilterList(List<Enchant> unfilteredList)
        {
            List<Enchant> filteredList = new List<Enchant>();
            foreach (Enchant enchant in unfilteredList)
            {
                if (enchant == null)
                {
                    filteredList.Add(enchant);
                    continue;
                }

                bool addEnchant = true;
                List<Enchant> removeEnchants = new List<Enchant>();
                foreach (Enchant enchant2 in filteredList)
                {
                    if (enchant.Slot == enchant2.Slot)
                    {
                        ArrayUtils.CompareResult compare = enchant.Stats.CompareTo(enchant2.Stats);
                        if (compare == ArrayUtils.CompareResult.GreaterThan) //A>B
                        {
                            removeEnchants.Add(enchant2);
                        }
                        else if (compare == ArrayUtils.CompareResult.Equal || compare == ArrayUtils.CompareResult.LessThan)
                        {
                            addEnchant = false;
                            break;
                        }
                    }
                }
                foreach (Enchant removeEnchant in removeEnchants)
                    filteredList.Remove(removeEnchant);
                if (addEnchant) filteredList.Add(enchant);
            }
            return filteredList.ToArray();
        }

        public static Item[] FilterList(List<Item> unfilteredList)
        {
            List<Item> filteredList = new List<Item>();
            List<StatsColors> filteredStatsColors = new List<StatsColors>();
            foreach (Item item in unfilteredList)
            {
                if (item == null)
                {
                    filteredList.Add(item);
                    continue;
                }

                StatsColors statsColorsA = new StatsColors()
                {
                    Item = item,
                    ItemIsJewelersGem = item.IsJewelersGem,
                    SetName = item.SetName,
                    Stats = item.Stats,
                };
                bool addItem = true;
                List<StatsColors> removeItems = new List<StatsColors>();
                foreach (StatsColors statsColorsB in filteredStatsColors)
                {
                    ArrayUtils.CompareResult compare = statsColorsA.CompareTo(statsColorsB);
                    if (compare == ArrayUtils.CompareResult.GreaterThan) //A>B
                    {
                        removeItems.Add(statsColorsB);
                    }
                    else if (compare == ArrayUtils.CompareResult.Equal || compare == ArrayUtils.CompareResult.LessThan)
                    {
                        addItem = false;
                        break;
                    }
                }
                foreach (StatsColors removeItem in removeItems)
                    filteredStatsColors.Remove(removeItem);
                if (addItem) filteredStatsColors.Add(statsColorsA);
            }
            foreach (StatsColors statsColors in filteredStatsColors)
            {
                filteredList.Add(statsColors.Item);
            }
            return filteredList.ToArray();
        }

        public static List<ItemInstance> FilterList(List<ItemInstance> unfilteredList, bool itemsFilteredHint)
        {
            List<ItemInstance> filteredList = new List<ItemInstance>();
            List<StatsColors> filteredStatsColors = new List<StatsColors>();
            foreach (ItemInstance gemmedItem in unfilteredList)
            {
                if ((object)gemmedItem == null)
                {
                    filteredList.Add(gemmedItem);
                    continue;
                }
                int meta = 0, red = 0, yellow = 0, blue = 0, jeweler = 0;
                foreach (Item gem in new Item[] { gemmedItem.Gem1, gemmedItem.Gem2, gemmedItem.Gem3 })
                    if (gem != null)
                    {
                        switch (gem.Slot)
                        {
                            case Item.ItemSlot.Meta: meta++; break;
                            case Item.ItemSlot.Red: red++; break;
                            case Item.ItemSlot.Orange: red++; yellow++; break;
                            case Item.ItemSlot.Yellow: yellow++; break;
                            case Item.ItemSlot.Green: yellow++; blue++; break;
                            case Item.ItemSlot.Blue: blue++; break;
                            case Item.ItemSlot.Purple: blue++; red++; break;
                            case Item.ItemSlot.Prismatic: red++; yellow++; blue++; break;
                        }
                        if (gem.IsJewelersGem) jeweler++;
                    }

                StatsColors statsColorsA = new StatsColors()
                {
                    ItemInstance = gemmedItem,
                    Item = gemmedItem.Item,
                    SetName = gemmedItem.Item.SetName,
                    Stats = gemmedItem.GetTotalStats(),
                    Meta = meta,
                    Red = red,
                    Yellow = yellow,
                    Blue = blue,
                    Jeweler = jeweler
                };
                bool addItem = true;
                List<StatsColors> removeItems = new List<StatsColors>();
                foreach (StatsColors statsColorsB in filteredStatsColors)
                {
                    if (!itemsFilteredHint || statsColorsA.Item.Id != statsColorsB.Item.Id)
                    {
                        ArrayUtils.CompareResult compare = statsColorsA.CompareTo(statsColorsB);
                        if (compare == ArrayUtils.CompareResult.GreaterThan) //A>B
                        {
                            removeItems.Add(statsColorsB);
                        }
                        else if (compare == ArrayUtils.CompareResult.Equal || compare == ArrayUtils.CompareResult.LessThan)
                        {
                            addItem = false;
                            break;
                        }
                    }
                }
                foreach (StatsColors removeItem in removeItems)
                    filteredStatsColors.Remove(removeItem);
                if (addItem) filteredStatsColors.Add(statsColorsA);
            }
            foreach (StatsColors statsColors in filteredStatsColors)
            {
                filteredList.Add(statsColors.ItemInstance);
            }
            return filteredList;
        }

        private class StatsColors
        {
            public ItemInstance ItemInstance;
            public Item Item;
            public Stats Stats;
            public int Meta;
            public int Red;
            public int Yellow;
            public int Blue;
            public int Jeweler;

            private string setName;

            public string SetName
            {
                get
                {
                    return setName;
                }
                set
                {
                    setName = value;
                    if (setName == null) setName = "";
                }
            }

            public bool ItemIsJewelersGem;

            public ArrayUtils.CompareResult CompareTo(StatsColors other)
            {
                if (ItemIsJewelersGem != other.ItemIsJewelersGem) return ArrayUtils.CompareResult.Unequal;
                if (Jeweler != other.Jeweler) return ArrayUtils.CompareResult.Unequal;

                if (this.SetName != other.SetName) return ArrayUtils.CompareResult.Unequal;

                int compare = Meta.CompareTo(other.Meta);
                bool haveLessThan = compare < 0;
                bool haveGreaterThan = compare > 0;
                if (haveGreaterThan && haveLessThan) return ArrayUtils.CompareResult.Unequal;

                compare = Red.CompareTo(other.Red);
                haveLessThan |= compare < 0;
                haveGreaterThan |= compare > 0;
                if (haveGreaterThan && haveLessThan) return ArrayUtils.CompareResult.Unequal;

                compare = Yellow.CompareTo(other.Yellow);
                haveLessThan |= compare < 0;
                haveGreaterThan |= compare > 0;
                if (haveGreaterThan && haveLessThan) return ArrayUtils.CompareResult.Unequal;

                compare = Blue.CompareTo(other.Blue);
                haveLessThan |= compare < 0;
                haveGreaterThan |= compare > 0;
                if (haveGreaterThan && haveLessThan) return ArrayUtils.CompareResult.Unequal;

                ArrayUtils.CompareResult compareResult = Stats.CompareTo(other.Stats);
                if (compareResult == ArrayUtils.CompareResult.Unequal) return ArrayUtils.CompareResult.Unequal;
                haveLessThan |= compareResult == ArrayUtils.CompareResult.LessThan;
                haveGreaterThan |= compareResult == ArrayUtils.CompareResult.GreaterThan;

                if (Item != null && (Item.Slot == Item.ItemSlot.MainHand || Item.Slot == Item.ItemSlot.OneHand || Item.Slot == Item.ItemSlot.TwoHand))
                {
                    if (Item.Slot == Item.ItemSlot.TwoHand && other.Item.Slot != Item.ItemSlot.TwoHand && haveGreaterThan)
                    {
                        return ArrayUtils.CompareResult.Unequal;
                    }
                    if (Item.Slot != Item.ItemSlot.TwoHand && other.Item.Slot == Item.ItemSlot.TwoHand && haveLessThan)
                    {
                        return ArrayUtils.CompareResult.Unequal;
                    }
                }

                if (haveGreaterThan && haveLessThan) return ArrayUtils.CompareResult.Unequal;
                else if (haveGreaterThan) return ArrayUtils.CompareResult.GreaterThan;
                else if (haveLessThan) return ArrayUtils.CompareResult.LessThan;
                else return ArrayUtils.CompareResult.Equal;
            }
            public static bool operator ==(StatsColors x, StatsColors y)
            {
                if (ReferenceEquals(x, null) || ReferenceEquals(y, null)) return false;
                return x.Meta == y.Meta && x.Red == y.Red && x.Yellow == y.Yellow
                    && x.Blue == y.Blue && x.Jeweler == y.Jeweler && x.Stats == y.Stats;
            }
            public override int GetHashCode()
            {
                return Stats.GetHashCode() ^ Item.GetHashCode();
            }
            public override bool Equals(object obj)
            {
                if (obj != null && obj.GetType() == this.GetType())
                {
                    return this == (obj as StatsColors);
                }
                return base.Equals(obj);
            }
            public static bool operator !=(StatsColors x, StatsColors y)
            {
                return !(x == y);
            }
        }
    }
}
