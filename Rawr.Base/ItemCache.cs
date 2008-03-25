using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;

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

		public static SortedDictionary<string, Item[]> Items { get { return _instance.Items; } }

		public static void InvalidateCachedStats() { Instance.InvalidateCachedStats(); }

		public static Item FindItemById(int id) { return _instance.FindItemById(id); }
		public static Item FindItemById(string gemmedId) { return _instance.FindItemById(gemmedId); }
		public static Item FindItemById(int id, bool createIfCorrectGemmingNotFound) { return _instance.FindItemById(id, createIfCorrectGemmingNotFound); }
		public static Item FindItemById(string gemmedId, bool createIfCorrectGemmingNotFound) { return _instance.FindItemById(gemmedId, createIfCorrectGemmingNotFound); }

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

		public Item FindItemById(int id) { return FindItemById(id.ToString() + ".0.0.0"); }
		public Item FindItemById(string gemmedId) { return FindItemById(gemmedId, true); }
		public Item FindItemById(int id, bool createIfCorrectGemmingNotFound) { return FindItemById(id.ToString() + ".0.0.0", createIfCorrectGemmingNotFound); }
		public Item FindItemById(string gemmedId, bool createIfCorrectGemmingNotFound)
        {
			if (!string.IsNullOrEmpty(gemmedId))
			{
				Item[] ret;
				if (Items.TryGetValue(gemmedId, out ret))
					return ret[0];
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
						AddItem(copy, false, true);
						return copy;
					}
			}
			return null;
		}

		public Item AddItem(Item item) { return AddItem(item, true, true); }
		public Item AddItem(Item item, bool removeOldCopy, bool raiseEvent)
		{
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
			if (Items.TryGetValue(item.GemmedId, out existing))
			{
				if (existing.Length > 1)
				{
					SortedList<Item, Item> newArray = new SortedList<Item, Item>();
					for (int i = 0; i < existing.Length; i++)
						if (existing[i].CompareTo(item) != 0)
						{
							newArray[existing[i]] = existing[i];
						}
					Items[item.GemmedId] = new Item[newArray.Keys.Count];
					newArray.Keys.CopyTo(Items[item.GemmedId], 0);
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
						foreach (Item item in itemArray)
							items.Add(item);
					_allItems = items.ToArray();
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
					_relevantItems = new List<Item>(AllItems).FindAll(new Predicate<Item>(delegate(Item item)
					{ return Calculations.IsItemRelevant(item); })).ToArray();
				}
				return _relevantItems;
			}
		}

		public event EventHandler ItemsChanged;
		public void OnItemsChanged()
		{
			_allItems = null;
			_relevantItems = null;
			if (ItemsChanged != null) ItemsChanged(null, null);
		}

		public void Save()
		{
			System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(List<Item>));
			StringBuilder sb = new StringBuilder();
			System.IO.StringWriter writer = new System.IO.StringWriter(sb);
			serializer.Serialize(writer, new List<Item>(AllItems));
			writer.Close();
			System.IO.File.WriteAllText(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "ItemCache.xml"), sb.ToString());
		}

		public void Load()
		{
			_items = new SortedDictionary<string, Item[]>();
			List<Item> listItems = new List<Item>();
			if (File.Exists(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "ItemCache.xml")))
			{
				try
				{
					string xml = System.IO.File.ReadAllText(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "ItemCache.xml")).Replace("/images/icons/", "");
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
				AddItem(item, true, false);
			}

			Calculations.ModelChanged += new EventHandler(Calculations_ModelChanged);
		}

		void Calculations_ModelChanged(object sender, EventArgs e)
		{
			_relevantItems = null;
		}
	}
}
