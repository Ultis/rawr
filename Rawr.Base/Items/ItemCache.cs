using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Threading;
using System.Xml.Serialization;

namespace Rawr
{
    public static class ItemCache
    {
        private static ItemCacheInstance _instance = new ItemCacheInstance();
        public static ItemCacheInstance Instance
        {
            get { return _instance; }
            set { _instance = value; }
        }
        public static Dictionary<int, Item> Items { get { return _instance.Items; } }

        public static void InvalidateCachedStats() { Instance.InvalidateCachedStats(); }
    
        public static Item FindItemById(int id) { return _instance.FindItemById(id); }
        public static bool ContainsItemId(int id) { return _instance.ContainsItemId(id); }

        public static Item AddItem(Item item) { return _instance.AddItem(item); }
        public static Item AddItem(Item item, bool raiseEvent) { return _instance.AddItem(item, raiseEvent); }

        public static void DeleteItem(Item item) { _instance.DeleteItem(item); }
        public static void DeleteItem(Item item, bool raiseEvent) { _instance.DeleteItem(item, raiseEvent); }

        public static Item[] AllItems { get { return _instance.AllItems; } }
        public static Item[] RelevantItems { get { return _instance.RelevantItems; } }

        public static Item[] GetUnfilteredRelevantItems(CalculationsBase model, CharacterRace race) { return _instance.GetUnfilteredRelevantItems(model, race); }
        public static Item[] GetRelevantItems(CalculationsBase model, Character character, bool ignoreFilters = false) { return _instance.GetRelevantItems(model, character, ignoreFilters); }
        public static Optimizer.SuffixItem[] GetRelevantSuffixItems(CalculationsBase model, Character character) { return _instance.GetRelevantSuffixItems(model, character); }

        public static void AutoSetUniqueId(Item item) { _instance.AutoSetUniqueId(item); }

        public static void OnItemsChanged() { _instance.OnItemsChanged(); }

        public static void Save(TextWriter writer) { _instance.Save(writer); }
        public static void Load(TextReader reader) { _instance.Load(reader); }

        public static void SaveItemCost(TextWriter writer) { _instance.SaveItemCost(writer); }
        public static void LoadItemCost(TextReader reader) { _instance.LoadItemCost(reader); }
        public static void ResetItemCost() { _instance.ResetItemCost(); }
        public static void LoadTokenItemCost(string token) { _instance.LoadTokenItemCost(token); }
    }

    public class ItemCacheInstance
    {
        public ItemCacheInstance() {  }
        public ItemCacheInstance(ItemCacheInstance instanceToClone)
        {
            _items = new Dictionary<int, Item>();
            lock (instanceToClone.Items)
            {
                foreach (KeyValuePair<int, Item> kvp in instanceToClone.Items)
                {
                    _items[kvp.Key] = kvp.Value;
                }
            }
        }

        private Dictionary<int, Item> _items;
        public Dictionary<int, Item> Items
        {
            get
            {
                if (_items == null) _items = new Dictionary<int, Item>();
                return _items;
            }
        }

        public void InvalidateCachedStats()
        {
            lock (Items)
            {
                foreach (Item item in Items.Values)
                {
                    item.InvalidateCachedData();
                }
            }
        }

        public Item FindItemById(int id)
        {
            if (id > 0)
            {
                Item item;
                lock (Items)
                {
                    Items.TryGetValue(id, out item);
                }
                return item;
            }
            return null;
        }

        public bool ContainsItemId(int id)
        {
            if (id > 0)
            {
                lock (Items)
                {
                    return Items.ContainsKey(id);
                }
            }
            return false;
        }

        private bool dirtySinceLastAdd;

        public Item AddItem(Item item) { return AddItem(item, true); }
        public Item AddItem(Item item, bool raiseEvent)
        {
            if (item == null) return null;
            //Chasing the lies no one believed...

            Item cachedItem;
            lock (Items)
            {
                if (Items.TryGetValue(item.Id, out cachedItem))
                {
                    cachedItem.Delete();
                }
                item.LastChange = DateTime.Now;
                Items[item.Id] = item;
                AutoSetUniqueId(item);
            }

            if (raiseEvent)
            {
                // when loading a new character we might get a long stream of new items from armory service
                // if we trigger event for every single one the application becomes more or less unresponsive
                // this is all happening on the main thread, armory service background worker is pushing things
                // on the dispatcher so add items will actually be delayed by however long it takes to
                // compute all calculations for charts
                // so send this via thread pool, this way it'll be pushed after all the backlog of items
                dirtySinceLastAdd = true;
#if SILVERLIGHT
                Dispatcher dispatcher = System.Windows.Deployment.Current.Dispatcher;
#else
                Dispatcher dispatcher = Dispatcher.CurrentDispatcher;
#endif
                ThreadPool.QueueUserWorkItem((object state) =>
                {                    
                    dispatcher.BeginInvoke((Action)(() =>
                    {
                        System.Diagnostics.Debug.WriteLine("Trying to trigger recalc " + dirtySinceLastAdd);
                        if (dirtySinceLastAdd)
                        {
                            OnItemsChanged();
                            dirtySinceLastAdd = false;
                        }
                    }));
                });
            }
            return item;
        }

        public void DeleteItem(Item item) { DeleteItem(item, true); }
        public void DeleteItem(Item item, bool raiseEvent)
        {
            if (item != null)
            {
                lock (Items)
                {
                    Item cachedItem;
                    if (Items.TryGetValue(item.Id, out cachedItem))
                    {
                        cachedItem.Delete();
                        Items.Remove(item.Id);
                    }
                }
            }
            if (raiseEvent) OnItemsChanged();
        }

        private Item[] _allItems = null;
        public Item[] AllItems
        {
            get
            {
                if (_allItems == null)
                {
                    lock (Items)
                    {
                        _allItems = new List<Item>(Items.Values).ToArray();
                    }
                }
                return _allItems;
            }
        }

        private Character character = null;
        public Character Character {
            get { return character; }
            set {
                character = value;
                if (character.LoadItemFilterEnabledOverride()) {
                    OnItemsChanged();
                }
            }
        }

        private Item[] _relevantItems = null;
        public Item[] RelevantItems
        {
            get
            {
                if (_relevantItems == null)
                {
                    _relevantItems = GetRelevantItemsInternal(Calculations.Instance, Character);
                }
                return _relevantItems;
            }
        }

        public Item[] GetUnfilteredRelevantItems(CalculationsBase model, CharacterRace race)
        {
            List<Item> itemList = new List<Item>(AllItems).FindAll(new Predicate<Item>(
                delegate(Item item) 
                { 
                    return model.IsItemRelevant(item) && item.FitsFaction(race); 
                }));
            return itemList.ToArray();
        }

        private CalculationsBase lastModel;
        private CharacterRace lastRace;
        private Item[] cachedRelevantItems;
        private Optimizer.SuffixItem[] cachedRelevantSuffixItems;
        private object syncLock = new object();

        public Item[] GetRelevantItems(CalculationsBase model, Character character, bool ignoreFilters = false)//CharacterRace race)
        {
            if (cachedRelevantItems == null || model != lastModel || character.Race != lastRace)
            {
                lock (syncLock)
                {
                    // test again because of race conditions, but we still want to avoid the lock if we can because that'll be the majority case
                    if (cachedRelevantItems == null || model != lastModel || character.Race != lastRace)
                    {
                        CacheRelevantItems(model, character, ignoreFilters);
                    }
                }
            }
            return cachedRelevantItems;
        }

        private void CacheRelevantItems(CalculationsBase model, Character character, bool ignoreFilters = false)
        {
            List<Item> itemList = new List<Item>(AllItems).FindAll(new Predicate<Item>(
                delegate(Item item)
                {
                    return model.IsItemRelevant(item) // Model Relevance
                        && item.FitsFaction(character.Race) // Faction Relevance
                        && (ignoreFilters || ItemFilter.IsItemRelevant(model, item)) // Filters Relevance
                        && character.ItemMatchesiLvlCheck(item)  // iLvl check from UI Filter (non-tree)
                        && character.ItemMatchesBindCheck(item)  // Bind check from UI Filter (non-tree)
                        && character.ItemMatchesProfCheck(item)  // Prof check from UI Filter (non-tree)
                        && character.ItemMatchesDropCheck(item); // Drop check from UI Filter (non-tree)
                }));
            cachedRelevantItems = itemList.ToArray();
            List<Optimizer.SuffixItem> suffixItemList = new List<Optimizer.SuffixItem>();
            foreach (var item in cachedRelevantItems)
            {
                if (item.AllowedRandomSuffixes == null || item.AllowedRandomSuffixes.Count == 0)
                {
                    suffixItemList.Add(new Optimizer.SuffixItem() { Item = item, RandomSuffixId = 0 });
                }
                else
                {
                    foreach (var suffix in item.AllowedRandomSuffixes)
                    {
                        suffixItemList.Add(new Optimizer.SuffixItem() { Item = item, RandomSuffixId = suffix });
                    }
                }
            }
            cachedRelevantSuffixItems = suffixItemList.ToArray();
            lastModel = model;
            lastRace = character.Race;
        }

        public Optimizer.SuffixItem[] GetRelevantSuffixItems(CalculationsBase model, Character character, bool ignoreFilters = false)//CharacterRace race)
        {
            if (cachedRelevantSuffixItems == null || model != lastModel || character.Race != lastRace)
            {
                lock (syncLock)
                {
                    // test again because of race conditions, but we still want to avoid the lock if we can because that'll be the majority case
                    if (cachedRelevantSuffixItems == null || model != lastModel || character.Race != lastRace)
                    {
                        CacheRelevantItems(model, character, ignoreFilters);
                    }
                }
            }
            return cachedRelevantSuffixItems;
        }

        internal Item[] GetRelevantItemsInternal(CalculationsBase model, Character character, bool ignoreFilters = false)
        {
            List<Item> itemList = new List<Item>(AllItems).FindAll(new Predicate<Item>(
                delegate(Item item) {
                    return model.IsItemRelevant(item) // Model Relevance
                        && (ignoreFilters || ItemFilter.IsItemRelevant(model, item)) // Filters Relevance
                        && (character == null || item.FitsFaction(character.Race)) // Faction Relevance
                        && (character == null || character.ItemMatchesiLvlCheck(item))  // iLvl check from UI Filter (non-tree)
                        && (character == null || character.ItemMatchesBindCheck(item))  // Bind check from UI Filter (non-tree)
                        && (character == null || character.ItemMatchesProfCheck(item))  // Prof check from UI Filter (non-tree)
                        && (character == null || character.ItemMatchesDropCheck(item)); // Drop check from UI Filter (non-tree)
                }));
            return itemList.ToArray();
        }

        public event EventHandler ItemsChanged;
        public void OnItemsChanged()
        {
            _allItems = null;
            _relevantItems = null;
            cachedRelevantItems = null;
            cachedRelevantSuffixItems = null;
            if (ItemsChanged != null) ItemsChanged(null, null);
        }

        // load/save of item cost data
        public void LoadItemCost(TextReader reader)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(SerializableDictionary<int, float>));
                SerializableDictionary<int, float> data = (SerializableDictionary<int, float>)serializer.Deserialize(reader);
                reader.Close();

                // reset all costs to 0, cost files only store nonzero cost items
                foreach (Item item in AllItems)
                {
                    item.Cost = 0.0f;
                }

                foreach (var kvp in data)
                {
                    Item item;
                    lock (Items)
                    {
                        if (Items.TryGetValue(kvp.Key, out item))
                        {
                            item.Cost = kvp.Value;
                        }
                    }
                }
                // don't need to invalidate relevant caches, but still trigger event to refresh graphs etc.
                if (ItemsChanged != null) ItemsChanged(null, null);
            }
            catch { }
        }

        public void SaveItemCost(TextWriter writer)
        {
            SerializableDictionary<int, float> data = new SerializableDictionary<int, float>();
            foreach (Item item in AllItems)
            {
                if (item.Cost > 0.0f)
                {
                    data.Add(item.Id, item.Cost);
                }
            }
            XmlSerializer serializer = new XmlSerializer(typeof(SerializableDictionary<int, float>));
            serializer.Serialize(writer, data);
            writer.Close();
        }

        public void ResetItemCost()
        {
            foreach (Item item in AllItems)
            {
                item.Cost = 0.0f;
            }
            // don't need to invalidate relevant caches, but still trigger event to refresh graphs etc.
            if (ItemsChanged != null) ItemsChanged(null, null);
        }

        public void LoadTokenItemCost(string token)
        {
            foreach (Item item in AllItems)
            {
                item.Cost = 0.0f; 
                if (item.LocationInfo[0].Source == ItemSource.Vendor)
                {
                    VendorItem vendor = item.LocationInfo[0] as VendorItem;
                    int count;
                    vendor.TokenMap.TryGetValue(token, out count);
                    item.Cost = count;
                }
            }
            // don't need to invalidate relevant caches, but still trigger event to refresh graphs etc.
            if (ItemsChanged != null) ItemsChanged(null, null);
        }

        private Dictionary<string, List<Item>> uniqueItemByName = new Dictionary<string, List<Item>>();

        /// <summary>
        /// Matches the item against known rules for unique item groups and sets the UniqueId list.
        /// </summary>
        /// <param name="item">Item for which to auto set the UniqueId list.</param>
        public void AutoSetUniqueId(Item item)
        {
            if (item.Unique && (item.Slot == ItemSlot.Trinket || item.Slot == ItemSlot.Finger || item.Slot == ItemSlot.OneHand)) // all items that have UniqueId rules are marked as Unique
            {
                // find all items in item cache with same name
                Item item251 = null, item258 = null, item264 = null, item271 = null, item277 = null, item279 = null, item284 = null, 
                    item316 = null, item333 = null, item346 = null, item359 = null, item372 = null, item378 = null, item391 = null;

                lock (Items)
                {
                    List<Item> list;
                    if (!uniqueItemByName.TryGetValue(item.Name, out list))
                    {
                        list = new List<Item>();
                        uniqueItemByName[item.Name] = list;
                    }
                    list.Add(item);

                    // we want the loop on Items because we don't want to trigger AllItems cache rebuilds
                    foreach (Item i in list)
                    {
                        Item validItem;
                        Items.TryGetValue(i.Id, out validItem);
                        if (i == validItem)
                        {
                            if (i.ItemLevel == 251) { item251 = i; }
                            else if (i.ItemLevel == 258) { item258 = i; }
                            else if (i.ItemLevel == 264) { item264 = i; }
                            else if (i.ItemLevel == 271) { item271 = i; }
                            else if (i.ItemLevel == 277) { item277 = i; }
                            else if (i.ItemLevel == 279) { item279 = i; }
                            else if (i.ItemLevel == 284) { item284 = i; }
                            else if (i.ItemLevel == 316) { item316 = i; }
                            else if (i.ItemLevel == 333) { item333 = i; }
                            else if (i.ItemLevel == 346) { item346 = i; }
                            else if (i.ItemLevel == 359) { item359 = i; }
                            else if (i.ItemLevel == 372) { item372 = i; }
                            else if (i.ItemLevel == 378) { item372 = i; }
                            else if (i.ItemLevel == 391) { item372 = i; }
                        }
                    }
                }

                // normal/heroic pair 10 man ICC with same name
                if ((object)item251 != null && (object)item264 != null)
                {
                    item251.UniqueId = new List<int>() { item251.Id, item264.Id };
                    item264.UniqueId = new List<int>() { item251.Id, item264.Id };
                }

                // normal/heroic pair 25 man ICC with same name
                if ((object)item264 != null && (object)item277 != null)
                {
                    item264.UniqueId = new List<int>() { item264.Id, item277.Id };
                    item277.UniqueId = new List<int>() { item264.Id, item277.Id };
                }

                // normal/heroic pair 10 man RS with same name
                if ((object)item258 != null && (object)item271 != null)
                {
                    item258.UniqueId = new List<int>() { item258.Id, item271.Id };
                    item271.UniqueId = new List<int>() { item258.Id, item271.Id };
                }

                // normal/heroic pair 25 man RS with same name
                if ((object)item271 != null && (object)item284 != null)
                {
                    item271.UniqueId = new List<int>() { item271.Id, item284.Id };
                    item284.UniqueId = new List<int>() { item271.Id, item284.Id };
                }

                // normal/heroic pair Throne of the Tides & Blackrock Caverns with same name
                if ((object)item279 != null && (object)item346 != null)
                {
                    item279.UniqueId = new List<int>() { item279.Id, item346.Id };
                    item346.UniqueId = new List<int>() { item279.Id, item346.Id };
                }

                // normal/heroic pair The Stonecore & The Vortex Pinnacle with same name
                if ((object)item316 != null && (object)item346 != null)
                {
                    item316.UniqueId = new List<int>() { item316.Id, item346.Id };
                    item346.UniqueId = new List<int>() { item316.Id, item346.Id };
                }

                // normal/heroic pair Grim Batol, Halls of Origination & Lost City of the Tol'vir with same name
                if ((object)item333 != null && (object)item346 != null)
                {
                    item333.UniqueId = new List<int>() { item333.Id, item346.Id };
                    item346.UniqueId = new List<int>() { item333.Id, item346.Id };
                }

                // normal/heroic pair Blackwing Descent, The Bastion of Twilight, Throne of the Four Winds, & Vicious Gladiator PvP weapons with same name
                if ((object)item359 != null && (object)item372 != null)
                {
                    item359.UniqueId = new List<int>() { item359.Id, item372.Id };
                    item372.UniqueId = new List<int>() { item359.Id, item372.Id };
                }

                // normal/heroic pair Tier 12 with same name
                if ((object)item378 != null && (object)item391 != null)
                {
                    item378.UniqueId = new List<int>() { item378.Id, item391.Id };
                    item391.UniqueId = new List<int>() { item378.Id, item391.Id };
                }

                // special rules for Ashen Verdict Rings
                // Ashen Band of Courage
                if (item.Id == 50375 || item.Id == 50404 || item.Id == 50388 || item.Id == 50403)
                {
                    item.UniqueId = new List<int>() { 50375, 50404, 50388, 50403 };
                }
                // Ashen Band of Destruction
                else if (item.Id == 50377 || item.Id == 50398 || item.Id == 50384 || item.Id == 50397)
                {
                    item.UniqueId = new List<int>() { 50377, 50398, 50384, 50397 };
                }
                // Ashen Band of Might
                else if (item.Id == 52572 || item.Id == 52570 || item.Id == 52569 || item.Id == 52571)
                {
                    item.UniqueId = new List<int>() { 52572, 52570, 52569, 52571 };
                }
                // Ashen Band of Vengeance
                else if (item.Id == 50402 || item.Id == 50387 || item.Id == 50401 || item.Id == 50376)
                {
                    item.UniqueId = new List<int>() { 50402, 50387, 50401, 50376 };
                }
                // Ashen Band of Wisdom
                else if (item.Id == 50400 || item.Id == 50386 || item.Id == 50399 || item.Id == 50378)
                {
                    item.UniqueId = new List<int>() { 50400, 50386, 50399, 50378 };
                }

                // special rules for alchemist stones
                if (item.Id == 58483 || item.Id == 68776 || item.Id == 68777 || item.Id == 68775)
                {
                    item.UniqueId = new List<int>() { 58483, 68776, 68777, 68775 };
                }
            }
        }

        public void Save(TextWriter writer)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ItemList));
            serializer.Serialize(writer, new ItemList(AllItems));
            writer.Close();
        }

        public void Load(TextReader reader)
        {
            _items = new Dictionary<int, Item>();
            List<Item> listItems = new List<Item>();
            try 
            {
                XmlSerializer serializer = new XmlSerializer(typeof(ItemList));
                listItems = (List<Item>)serializer.Deserialize(reader);
            } 
            finally
            {
                reader.Close();
            }

            if (listItems != null && listItems.Count > 100) // enforce that it was a good cache load full of items
            {
                foreach (Item item in listItems)
                {
                    //CataclysmizeItem(item);
                    AddItem(item, false);
                }
            }
            else
            {
                throw new Exception("Item Cache Invalid",
                    new Exception("The ItemCache loaded from XML was either empty or had too few items to be a valid ItemCache"));
            }
            Calculations.ModelChanged += new EventHandler(Calculations_ModelChanged);
        }

        void Calculations_ModelChanged(object sender, EventArgs e)
        {
            _relevantItems = null;
        }
    }
}
