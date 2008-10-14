using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml.Serialization;

namespace Rawr
{
	public static class ItemCache
	{
        public static readonly string SavedFilePath = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "Data\\ItemCache.xml");
		private static ItemCacheInstance _instance = new ItemCacheInstance();
		public static ItemCacheInstance Instance
		{
			get { return _instance; }
			set { _instance = value; }
		}

		public static SortedDictionary<string, Item[]> Items { get { return _instance.Items; } }

		public static void InvalidateCachedStats() { Instance.InvalidateCachedStats(); }

		public static Item FindItemById(int id) { return _instance.FindItemById(id); }
		public static Item FindItemById(string gemmedId) { return _instance.FindItemById(gemmedId); }
		public static Item FindItemById(int id, bool createIfCorrectGemmingNotFound) { return _instance.FindItemById(id, createIfCorrectGemmingNotFound); }
		public static Item FindItemById(string gemmedId, bool createIfCorrectGemmingNotFound, bool raiseEvent) { return _instance.FindItemById(gemmedId, createIfCorrectGemmingNotFound,raiseEvent); }
        public static bool ContainsItemId(int id) { return _instance.ContainsItemId(id); }

		public static Item AddItem(Item item) { return _instance.AddItem(item); }
		public static Item AddItem(Item item, bool removeOldCopy, bool raiseEvent) { return _instance.AddItem(item, removeOldCopy, raiseEvent); }

		public static void DeleteItem(Item item) { _instance.DeleteItem(item); }
		public static void DeleteItem(Item item, bool raiseEvent) { _instance.DeleteItem(item, raiseEvent); }

		public static Item[] AllItems { get { return _instance.AllItems; } }
		public static Item[] RelevantItems { get { return _instance.RelevantItems; } }

		public static void OnItemsChanged() { _instance.OnItemsChanged(); }

		public static void Save() { _instance.Save(); }
		public static void Load() { _instance.Load(); }
	}

	public class ItemCacheInstance
	{
		public ItemCacheInstance() { }
		public ItemCacheInstance(ItemCacheInstance instanceToClone)
		{
			_items = new SortedDictionary<string, Item[]>();
			foreach (KeyValuePair<string, Item[]> kvp in instanceToClone.Items)
			{
				_items.Add(kvp.Key, kvp.Value.Clone() as Item[]);
			}
		}

		private SortedDictionary<string, Item[]> _items;
		public SortedDictionary<string, Item[]> Items
		{
			get
			{
				if (_items == null)
					Load();
				return _items;
			}
		}

		public void InvalidateCachedStats()
        {
            foreach (Item[] items in Items.Values)
            {
                foreach (Item item in items)
                {
                    item.InvalidateCachedData();
                }
            }
        }

        public ICollection<Item> FindAllItemsById(int id)
        {
            List<Item> items = new List<Item>();
			string keyStartsWith = id.ToString() + ".";
			string keyNoGem= id.ToString() + ".0.0.0";
            Item[] noGem = null;
            foreach (string key in Items.Keys)
            {
                if(key == keyNoGem)
                {
                    noGem = Items[key];
                }
                else if (key.StartsWith(keyStartsWith))
                {
                    items.AddRange(Items[key]);
                }
            }
            if(noGem != null && items.Count == 0)
            {
                items.AddRange(noGem);
            }
            return items;
        }

		public Item FindItemById(int id) { return FindItemById(id.ToString() + ".0.0.0"); }
		public Item FindItemById(string gemmedId) { return FindItemById(gemmedId, true,true); }
		public Item FindItemById(int id, bool createIfCorrectGemmingNotFound) { return FindItemById(id.ToString() + ".0.0.0", createIfCorrectGemmingNotFound,true); }
		public Item FindItemById(string gemmedId, bool createIfCorrectGemmingNotFound, bool raiseEvent) { return FindItemById(gemmedId, createIfCorrectGemmingNotFound, raiseEvent, false); }
		public Item FindItemById(string gemmedId, bool createIfCorrectGemmingNotFound, bool raiseEvent, bool hintSkipRelevant)
		{
			if (!string.IsNullOrEmpty(gemmedId))
			{
				Item retRelevant;
				if (!hintSkipRelevant && RelevantItemsDictionary.TryGetValue(gemmedId, out retRelevant))
					return retRelevant;
				Item[] retIrrelevant;
				if (Items.TryGetValue(gemmedId, out retIrrelevant))
					return retIrrelevant[0];
				else if (!createIfCorrectGemmingNotFound)
					return null;

				string[] ids = gemmedId.Split('.');
				int id = int.Parse(ids[0]);
				int id1 = ids.Length == 4 ? int.Parse(ids[1]) : 0;
				int id2 = ids.Length == 4 ? int.Parse(ids[2]) : 0;
				int id3 = ids.Length == 4 ? int.Parse(ids[3]) : 0;
				string keyStartsWith = id.ToString() + ".";
				foreach (string key in Items.Keys)
					if (key.StartsWith(keyStartsWith))
					{
						Item item = Items[key][0];
						Item copy = new Item(item.Name, item.Quality, item.Type, item.Id, item.IconPath, item.Slot,
							item.SetName, item.Unique, item.Stats.Clone(), item.Sockets.Clone(), id1, id2, id3, item.MinDamage,
							item.MaxDamage, item.DamageType, item.Speed, item.RequiredClasses);
						AddItem(copy, false, raiseEvent);
						return copy;
					}
			}
			return null;
		}

        public bool ContainsItemId(int id)
        {
            if (id > 0)
            {
                string keyStartsWith = id.ToString() + ".";
                foreach (string key in Items.Keys)
                    if (key.StartsWith(keyStartsWith))
                    {
                        return true;
                    }
            }
            return false;
        }

		public Item AddItem(Item item) { return AddItem(item, true, true); }
		public Item AddItem(Item item, bool removeOldCopy, bool raiseEvent)
		{
			if (item == null) return null;
			//Chasing the lies no one believed...
			Item[] existing;
			Item[] newArray;
			if (removeOldCopy)
			{
				DeleteItem(item, false);
			}

			if (Items.TryGetValue(item.GemmedId, out existing))
			{
				newArray = new Item[existing.Length + 1];
				existing.CopyTo(newArray, 0);
				newArray[existing.Length] = item;
				Items[item.GemmedId] = newArray;
			}
			else
			{
				Items.Add(item.GemmedId, new Item[] { item });
			}



			if (raiseEvent) OnItemsChanged();
			return item;
		}

		public void DeleteItem(Item item) { DeleteItem(item, true); }
		public void DeleteItem(Item item, bool raiseEvent)
		{
			Item[] existing;
			if (item != null && Items.TryGetValue(item.GemmedId, out existing))
			{
				if (existing.Length > 1)
				{
					List<Item> newArray = new List<Item>();
					for (int i = 0; i < existing.Length; i++)
						if (existing[i].GetHashCode() != item.GetHashCode())
							newArray.Add(existing[i]);

					Items[item.GemmedId] = newArray.ToArray();
				}
				else
					Items.Remove(item.GemmedId);
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
					List<Item> items = new List<Item>();
					foreach (Item[] itemArray in Items.Values)
						items.AddRange(itemArray);
					_allItems = items.ToArray();
				}
				return _allItems;
			}
		}

		private SortedDictionary<string, Item> _relevantItemsDictionary = null;
		public SortedDictionary<string, Item> RelevantItemsDictionary
		{
			get
			{
				if (_relevantItemsDictionary == null)
				{
					_relevantItemsDictionary = new SortedDictionary<string, Item>();
					foreach (Item item in RelevantItems)
						_relevantItemsDictionary[item.GemmedId] = item;
				}
				return _relevantItemsDictionary;
			}
		}

		private Item[] _relevantItems = null;
		public Item[] RelevantItems
		{
			get
			{
				if (_relevantItems == null)
				{
					_relevantItems = new List<Item>(AllItems).FindAll(new Predicate<Item>(
						delegate(Item item) { return Calculations.IsItemRelevant(item); })).ToArray();
				}
				return _relevantItems;
			}
		}

        public Item[] GetRelevantItems(CalculationsBase model)
        {
            if (model == Calculations.Instance)
            {
                return RelevantItems;
            }
            else
            {
                return new List<Item>(AllItems).FindAll(new Predicate<Item>(
                    delegate(Item item) { return model.IsItemRelevant(item); })).ToArray();
            }
        }

		public event EventHandler ItemsChanged;
		public void OnItemsChanged()
		{
			_allItems = null;
			_relevantItems = null;
            _relevantItemsDictionary = null;
			if (ItemsChanged != null) ItemsChanged(null, null);
		}

		public void Save()
		{
#if !AGGREGATE_ITEMS
            using (StreamWriter writer = new StreamWriter(ItemCache.SavedFilePath,false, Encoding.UTF8))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<Item>));
                serializer.Serialize(writer, new List<Item>(AllItems));
                writer.Close();
            }

            LocationFactory.Save("Data\\ItemSource.xml");
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

		public void Load()
		{
			_items = new SortedDictionary<string, Item[]>();
			List<Item> listItems = new List<Item>();
			if (File.Exists(ItemCache.SavedFilePath))
			{
				try
				{
					string xml = System.IO.File.ReadAllText(ItemCache.SavedFilePath).Replace("/images/icons/", "");
					xml = xml.Replace("<Slot>Weapon</Slot", "<Slot>TwoHand</Slot>").Replace("<Slot>Idol</Slot", "<Slot>Ranged</Slot>").Replace("<Slot>Robe</Slot", "<Slot>Chest</Slot>");
					System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(List<Item>));
					System.IO.StringReader reader = new System.IO.StringReader(xml);
					listItems = (List<Item>)serializer.Deserialize(reader);
					reader.Close();
				}
				catch (Exception)
				{
					MessageBox.Show("Rawr was unable to load the Item Cache. It appears to have been made with a previous incompatible version of Rawr. Please use the ItemCache included with this version of Rawr to start from.");
				}
			}
			foreach (Item item in listItems)
			{
				item.Stats.ConvertStatsToWotLKEquivalents();
                item.Sockets.Stats.ConvertStatsToWotLKEquivalents();
				//if (item.Type == Item.ItemType.Leather) UpdateArmorFromWowhead(item);
				AddItem(item, true, false);
			}

            LocationFactory.Load("Data\\ItemSource.xml");
			Calculations.ModelChanged += new EventHandler(Calculations_ModelChanged);
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
