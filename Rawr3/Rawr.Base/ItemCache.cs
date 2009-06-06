using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Linq;
using System.IO;

namespace Rawr
{
	public static class ItemCache
	{
		public static readonly string SavedFilePath = "ItemCache.xml";
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

        public static Item[] GetUnfilteredRelevantItems(CalculationsBase model) { return _instance.GetUnfilteredRelevantItems(model); }
        public static Item[] GetRelevantItems(CalculationsBase model) { return _instance.GetRelevantItems(model); }

		public static void OnItemsChanged() { _instance.OnItemsChanged(); }

		public static void Save(StreamWriter writer) { _instance.Save(writer); }
        public static void Load(StreamReader reader) { _instance.Load(reader); }
	}

	public class ItemCacheInstance
	{
		public ItemCacheInstance() { }
		public ItemCacheInstance(ItemCacheInstance instanceToClone)
		{
			_items = new Dictionary<int, Item>(); // Was sorted
			foreach (KeyValuePair<int, Item> kvp in instanceToClone.Items)
			{
                _items[kvp.Key] = kvp.Value;
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
            //foreach (Item item in Items.Values)
            //{
            //    item.InvalidateCachedData();
            //}
        }

		public Item FindItemById(int id)
		{
			if (id > 0)
			{
				Item item;
                Items.TryGetValue(id, out item);
				return item;
			}
			return null;
		}

        public bool ContainsItemId(int id)
        {
            if (id > 0)
            {
                return Items.ContainsKey(id);
            }
            return false;
        }

		public Item AddItem(Item item) { return AddItem(item, true); }
		public Item AddItem(Item item, bool raiseEvent)
		{
			if (item == null) return null;
			//Chasing the lies no one believed...

            Item cachedItem;
            if (Items.TryGetValue(item.Id, out cachedItem))
            {
                cachedItem.Delete();
            }
            item.LastChange = DateTime.Now;
            Items[item.Id] = item;

			if (raiseEvent) OnItemsChanged();
			return item;
		}

		public void DeleteItem(Item item) { DeleteItem(item, true); }
		public void DeleteItem(Item item, bool raiseEvent)
		{
			if (item != null)
			{
                Item cachedItem;
                if (Items.TryGetValue(item.Id, out cachedItem))
                {
                    cachedItem.Delete();
                    Items.Remove(item.Id);
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
					_allItems = new List<Item>(Items.Values).ToArray();
				}
				return _allItems;
			}
		}

		private Item[] _relevantItems = null;
		public Item[] RelevantItems
		{
			get
			{
                if (_relevantItems == null)
                {
                    _relevantItems = GetRelevantItemsInternal(Calculations.Instance);
                }
                return _relevantItems;
			}
		}

        public Item[] GetUnfilteredRelevantItems(CalculationsBase model)
        {
            List<Item> itemList = new List<Item>(AllItems.Where<Item>(delegate(Item item) {
                return model.IsItemRelevant(item); }));
            return itemList.ToArray();
        }

        public Item[] GetRelevantItems(CalculationsBase model)
        {
            if (model == Calculations.Instance)
            {
                return RelevantItems;
            }
            else
            {
                return GetRelevantItemsInternal(model);
            }
        }

        private Item[] GetRelevantItemsInternal(CalculationsBase model)
        {
            List<Item> itemList = new List<Item>(AllItems.Where<Item>( delegate(Item item) {
                return model.IsItemRelevant(item) && ItemFilter.IsItemRelevant(model, item); }));
            return itemList.ToArray();
        }

		public event EventHandler ItemsChanged;
		public void OnItemsChanged()
		{
			_allItems = null;
			_relevantItems = null;
			if (ItemsChanged != null) ItemsChanged(null, null);
		}

		public void Save(StreamWriter writer)
		{
#if !AGGREGATE_ITEMS
            XmlSerializer serializer = new XmlSerializer(typeof(List<Item>));
            serializer.Serialize(writer, new List<Item>(AllItems));
            writer.Close();

            //LocationFactory.Save("Data" + System.IO.Path.DirectorySeparatorChar + "ItemSource.xml");
            //ItemFilter.Save("Data" + System.IO.Path.DirectorySeparatorChar + "ItemFilter.xml");
#else
            //this is handy for debugging
            foreach (Item item in AllItems)
            {
                try
                {
                    System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(Item));
                    StringBuilder sb = new StringBuilder();
                    System.IO.StringWriter writer = new System.IO.StringWriter(sb);
                    serializer.Serialize(writer, item);
                    writer.Close();
                    System.IO.File.WriteAllText(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "ItemCache-"+item.Name+".xml"), sb.ToString());

                }
                catch (System.Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);
                }
            }
#endif
        }

		public void Load(StreamReader reader)
		{
			_items = new Dictionary<int, Item>();
            List<Item> listItems = new List<Item>();
            XmlSerializer serializer = new XmlSerializer(typeof(List<Item>));
            listItems = (List<Item>)serializer.Deserialize(reader);
            reader.Close();

            if (listItems != null)
            {
                foreach (Item item in listItems)
                {
                    AddItem(item, false);
                }
            }
            //LocationFactory.Load("Data" + System.IO.Path.DirectorySeparatorChar + "ItemSource.xml");
            //ItemFilter.Load("Data" + System.IO.Path.DirectorySeparatorChar + "ItemFilter.xml");
            //Calculations.ModelChanged += new EventHandler(Calculations_ModelChanged);
		}

		void Calculations_ModelChanged(object sender, EventArgs e)
		{
			_relevantItems = null;
        }

		//private void UpdateArmorFromWowhead(Item item)
		//{
		//    WebRequestWrapper wrw = new WebRequestWrapper();
		//    string wowheadXml = wrw.DownloadText(string.Format("http://wotlk.wowhead.com/?item={0}&xml", item.Id));
		//    wowheadXml = wowheadXml.Substring(0, wowheadXml.LastIndexOf(" Armor<"));
		//    wowheadXml = wowheadXml.Substring(wowheadXml.LastIndexOf('>')+1);
		//    item.Stats.Armor = int.Parse(wowheadXml);
		//}
	}
}
