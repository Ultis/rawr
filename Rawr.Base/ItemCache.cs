using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Rawr
{
	public static class ItemCache
	{
		private static Dictionary<string, Item[]> _items;
		public static Dictionary<string, Item[]> Items
		{
			get
			{
				if (_items == null)
					Load();
				return _items;
			}
		}

		public static Item FindItemById(int id) { return FindItemById(id.ToString() + ".0.0.0"); }
		public static Item FindItemById(string gemmedId) { return FindItemById(gemmedId, true); }
		public static Item FindItemById(int id, bool createIfCorrectGemmingNotFound) { return FindItemById(id.ToString() + ".0.0.0", createIfCorrectGemmingNotFound); }
		public static Item FindItemById(string gemmedId, bool createIfCorrectGemmingNotFound)
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
						Item copy = item.Clone();
						AddItem(copy, false, true);
						return copy;
					}
			}
			return null;
		}

		public static Item AddItem(Item item) { return AddItem(item, true, true); }
		public static Item AddItem(Item item, bool removeOldCopy, bool raiseEvent)
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

		public static void DeleteItem(Item item) { DeleteItem(item, true); }
		public static void DeleteItem(Item item, bool raiseEvent)
		{
			Item[] existing;
			if (Items.TryGetValue(item.GemmedId, out existing))
			{
				if (existing.Length > 1)
				{
                    SortedList<Item, Item> newArray = new SortedList<Item, Item>();
					for (int i = 0; i < existing.Length; i++)
					if (existing[i].CompareTo(item)!=0)
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

		private static Item[] _allItems = null;
		public static Item[] AllItems
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

		private static Item[] _relevantItems = null;
		public static Item[] RelevantItems
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

		public static event EventHandler ItemsChanged;
		public static void OnItemsChanged()
		{
			_allItems = null;
			_relevantItems = null;
			if (ItemsChanged != null) ItemsChanged(null, null);
		}

		public static void Save()
		{
			System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(List<Item>));
			StringBuilder sb = new StringBuilder();
			System.IO.StringWriter writer = new System.IO.StringWriter(sb);
			serializer.Serialize(writer, new List<Item>(AllItems));
			writer.Close();
			System.IO.File.WriteAllText(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "ItemCache.xml"), sb.ToString());
		}

		public static void Load()
		{
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
				catch (Exception ex)
				{
					MessageBox.Show("Rawr was unable to load the Item Cache. It appears to have been made with a previous incompatible version of Rawr. Please use the ItemCache included with this version of Rawr to start from.");
				}
			}
			_items = new Dictionary<string, Item[]>();
			foreach (Item item in listItems)
			{
				AddItem(item, false, false);
			}

			Calculations.ModelChanged += new EventHandler(Calculations_ModelChanged);
		}

		static void Calculations_ModelChanged(object sender, EventArgs e)
		{
			_relevantItems = null;
		}
	}
}
