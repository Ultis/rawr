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
				//foreach (Item item in Items)
				//    if (item.GemmedId == gemmedId)
				//        return item;
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
						Item copy = new Item(item.Name, item.Id, item.IconPath, item.Slot, new Stats(item.Stats.Armor, item.Stats.Health, item.Stats.Agility,
							item.Stats.Stamina, item.Stats.DodgeRating, item.Stats.DefenseRating, item.Stats.Resilience), new Sockets(item.Sockets.Color1,
							item.Sockets.Color2, item.Sockets.Color3, new Stats(item.Sockets.Stats.Armor, item.Sockets.Stats.Health, item.Sockets.Stats.Agility,
							item.Sockets.Stats.Stamina, item.Sockets.Stats.DodgeRating, item.Sockets.Stats.DefenseRating, item.Sockets.Stats.Resilience)),
							id1, id2, id3); //FindItemById(id1) != null ? id1 : 0, FindItemById(id2) != null ? id2 : 0, FindItemById(id3) != null ? id3 : 0);
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
				//if (Items.TryGetValue(item.GemmedId, out existing))
				//{
				//    if (existing.Length > 1)
				//    {
				//        newArray = new Item[existing.Length - 1];
				//        for (int i = 1; i < existing.Length; i++)
				//            newArray[i - 1] = existing[i];
				//        Items[item.GemmedId] = newArray;
				//    }
				//    else
				//        Items.Remove(item.GemmedId);
				//}
				//Item cachedItem = FindItemById(item.GemmedId);
				//if (cachedItem != null)
				//	Items.Remove(cachedItem);
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
			Item[] newArray;
			if (Items.TryGetValue(item.GemmedId, out existing))
			{
				if (existing.Length > 1)
				{
					newArray = new Item[existing.Length - 1];
					int k = 0;
					for (int i = 0; i < existing.Length; i++)
						if (existing[i] != item)
						{
							newArray[k++] = existing[i];
						}
					Items[item.GemmedId] = newArray;
				}
				else
					Items.Remove(item.GemmedId);
			}
			if (raiseEvent) OnItemsChanged();
		}

		public static Item[] GetItemsArray()
		{
			List<Item> items = new List<Item>();
			foreach (Item[] itemArray in Items.Values)
				foreach (Item item in itemArray)
					items.Add(item);
			return items.ToArray();
		}

		//private static List<Gem> _gems;
		//public static List<Gem> Gems
		//{
		//    get
		//    {
		//        if (_gems == null)
		//            Load();
		//        return _gems;
		//    }
		//}

		//public static Gem FindItemById(int id)
		//{
		//    foreach (Gem gem in Gems)
		//        if (gem.Id == id)
		//            return gem;
		//    return null;
		//}

		//public static Gem FindGemByStats(Stats stats, Item.ItemSlot color)
		//{
		//    foreach (Gem gem in Gems)
		//        if (gem.Stats.Agility == stats.Agility &&
		//            gem.Stats.Armor == stats.Armor &&
		//            gem.Stats.DefenseRating == stats.DefenseRating &&
		//            gem.Stats.DodgeRating == stats.DodgeRating &&
		//            gem.Stats.Health == stats.Health &&
		//            gem.Stats.Resilience == stats.Resilience &&
		//            gem.Stats.Stamina == stats.Stamina &&
		//            gem.Color == color)
		//            return gem;
		//    return null;
		//}

		//public static Gem AddGem(Gem gem)
		//{
		//    Gem cachedGem = FindItemById(gem.Id);
		//    if (cachedGem != null)
		//        Gems.Remove(cachedGem);
		//    Gems.Add(gem);
		//    OnItemsChanged();
		//    return gem;
		//}

		//public static void DeleteGem(Gem gem)
		//{
		//    Gems.Remove(gem);
		//    OnItemsChanged();
		//}

		public static event EventHandler ItemsChanged;
		public static void OnItemsChanged()
		{
			if (ItemsChanged != null) ItemsChanged(null, null);
		}

		public static void Save()
		{
			System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(List<Item>));
			StringBuilder sb = new StringBuilder();
			System.IO.StringWriter writer = new System.IO.StringWriter(sb);
			serializer.Serialize(writer, new List<Item>(GetItemsArray()));
			writer.Close();
			System.IO.File.WriteAllText(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "ItemCache.xml"), sb.ToString());

			//serializer = new System.Xml.Serialization.XmlSerializer(typeof(List<Gem>));
			//sb = new StringBuilder();
			//writer = new System.IO.StringWriter(sb);
			//serializer.Serialize(writer, Gems);
			//writer.Close();
			//System.IO.File.WriteAllText(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "GemCache.xml"), sb.ToString());
		}

		public static void Load()
		{
			List<Item> listItems = new List<Item>();
			if (File.Exists(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "ItemCache.xml")))
			{
				string xml = System.IO.File.ReadAllText(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "ItemCache.xml")).Replace("/images/icons/", "");
				System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(List<Item>));
				System.IO.StringReader reader = new System.IO.StringReader(xml);
				listItems = (List<Item>)serializer.Deserialize(reader);
				reader.Close();
			}
			_items = new Dictionary<string, Item[]>();
			foreach (Item item in listItems)
			{
				AddItem(item, false, false);
			}

			//else
			//    _items = new List<Item>();

			//if (File.Exists(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "GemCache.xml")))
			//{
			//    string xml = System.IO.File.ReadAllText(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "GemCache.xml")).Replace("/images/icons/", "");
			//    System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(List<Gem>));
			//    System.IO.StringReader reader = new System.IO.StringReader(xml);
			//    _gems = (List<Gem>)serializer.Deserialize(reader);
			//    reader.Close();
			//}
			//else
			//{
			//    _gems = new List<Gem>();
			//    _gems.Add(new Gem("No Gem", 0, "/images/icons/temp.png", Item.ItemSlot.None, new Stats(0, 0, 0, 0, 0, 0, 0)));
			//}
		}
	}
}
