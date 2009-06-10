using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
#if !SILVERLIGHT
using System.Windows.Forms;
#endif

namespace Rawr
{
#if SILVERLIGHT   
    public class ItemFilterRegexList : List<ItemFilterRegex>
#else
    public class ItemFilterRegexList : BindingList<ItemFilterRegex>
#endif
    {
    }

    
    public class ItemFilterRegex
    {
        public ItemFilterRegex()
        {
            Name = "New Filter";
            Enabled = true;
            MinItemLevel = 0;
            MaxItemLevel = 1000;
            MinItemQuality = Item.ItemQuality.Temp;
            MaxItemQuality = Item.ItemQuality.Heirloom;
            AdditiveFilter = true;
            AppliesToItems = true;
            AppliesToGems = true;
        }

        public string Name { get; set; }
        [XmlIgnore]
        private string _pattern;

        public string Pattern
        {
            get
            {
                return _pattern;
            }
            set
            {
                _pattern = value;
                _regex = null;
            }
        }

        public int MinItemLevel { get; set; }
        public int MaxItemLevel { get; set; }
        public Item.ItemQuality MinItemQuality { get; set; }
        public Item.ItemQuality MaxItemQuality { get; set; }
        public bool AdditiveFilter { get; set; }
        public bool AppliesToItems { get; set; }
        public bool AppliesToGems { get; set; }

        public bool Enabled { get; set; }

        public ItemFilterRegexList RegexList = new ItemFilterRegexList();
        public bool OtherRegexEnabled = true;

        [XmlIgnore]
        private Regex _regex;

        [XmlIgnore]
        public Regex Regex
        {
            get
            {
                if (_regex == null)
                {
                    if (_pattern == null) _pattern = "";
#if SILVERLIGHT
                    _regex = new Regex(_pattern, RegexOptions.CultureInvariant | RegexOptions.IgnoreCase | RegexOptions.Singleline);
#else
                    _regex = new Regex(_pattern, RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase | RegexOptions.Singleline);
#endif
                }
                return _regex;
            }
        }

        public bool IsMatch(Item item)
        {
            if (string.IsNullOrEmpty(_pattern) || (!string.IsNullOrEmpty(item.LocationInfo.Description) && Regex.IsMatch(item.LocationInfo.Description)) || (!string.IsNullOrEmpty(item.LocationInfo.Note) && Regex.IsMatch(item.LocationInfo.Note)))
            {
                if (item.ItemLevel >= MinItemLevel && item.ItemLevel <= MaxItemLevel)
                {
                    if (item.Quality >= MinItemQuality && item.Quality <= MaxItemQuality)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool AppliesTo(Item item)
        {
            return (item.IsGem && AppliesToGems) || (!item.IsGem && AppliesToItems);
        }

        public bool IsEnabled(Item item)
        {
            if (!Enabled) return false;
            if (RegexList.Count == 0) return true;
            bool enabledMatch = false;
            bool anyMatch = false;
            foreach (ItemFilterRegex regex in RegexList)
            {
                if (regex.AppliesTo(item) && regex.IsMatch(item))
                {
                    anyMatch = true;
                    if (regex.IsEnabled(item))
                    {
                        enabledMatch = true;
                        break;
                    }
                }
            }
            if (anyMatch)
            {
                return enabledMatch;
            }
            else
            {
                return OtherRegexEnabled;
            }
        }
    }

    
    public class ItemTypeList : List<Item.ItemType>
    {
        public ItemTypeList() : base() { }
        public ItemTypeList(IEnumerable<Item.ItemType> collection) : base(collection) { }
    }

    
    public class ItemFilterData
    {
        public SerializableDictionary<string, ItemTypeList> RelevantItemTypes = new SerializableDictionary<string, ItemTypeList>();
        public ItemFilterRegexList RegexList = new ItemFilterRegexList();
        public bool OtherRegexEnabled = true;
    }

    public static class ItemFilter
    {
        static ItemFilterData data = new ItemFilterData();

        /// <summary>
        /// Returns a list that can be used to set relevant item types list. Call ItemCache.OnItemsChanged()
        /// when you finish making changes to the list.
        /// </summary>
        public static List<Item.ItemType> GetRelevantItemTypesList(CalculationsBase model)
        {
            ItemTypeList list;
            if (!data.RelevantItemTypes.TryGetValue(model.Name, out list))
            {
                list = new ItemTypeList(model.RelevantItemTypes);
                data.RelevantItemTypes[model.Name] = list;
            }
            return list;
        }

        public static ItemFilterRegexList RegexList
        {
            get
            {
                return data.RegexList;
            }
        }

        public static bool OtherRegexEnabled
        {
            get
            {
                return data.OtherRegexEnabled;
            }
            set
            {
                data.OtherRegexEnabled = value;
            }
        }

        public static bool IsItemRelevant(CalculationsBase model, Item item)
        {
            if (GetRelevantItemTypesList(model).Contains(item.Type))
            {
                bool added = false;
                bool anyMatch = false;
                bool enabledMatch = false;
                foreach (ItemFilterRegex regex in data.RegexList)
                {
                    if (regex.AdditiveFilter && regex.AppliesTo(item) && regex.IsMatch(item))
                    {
                        anyMatch = true;
                        if (regex.IsEnabled(item))
                        {
                            enabledMatch = true;
                            break;
                        }
                    }
                }
                if (anyMatch)
                {
                    added = enabledMatch;
                }
                else
                {
                    added = OtherRegexEnabled;
                }
                if (added)
                {
                    foreach (ItemFilterRegex regex in data.RegexList)
                    {
                        if (!regex.AdditiveFilter && regex.Enabled && regex.AppliesTo(item) && regex.IsMatch(item) && regex.IsEnabled(item))
                        {
                            return false;
                        }
                    }
                    return true;
                }
            }
            return false;
        }

#if SILVERLIGHT
        public static void Save(StreamWriter writer)
        {
            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(ItemFilterData));
            serializer.Serialize(writer, data);
            writer.Close();
        }

        public static void Load(StreamReader reader)
        {
            try
            {
                System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(ItemFilterData));
                data = (ItemFilterData)serializer.Deserialize(reader);
                reader.Close();
            }
            catch (Exception)
            {
                data = new ItemFilterData();
            }
        } 
#else
        public static void Save(string fileName)
        {
            using (StreamWriter writer = new StreamWriter(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), fileName), false, Encoding.UTF8))
            {
                System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(ItemFilterData));
                serializer.Serialize(writer, data);
                writer.Close();
            }
        }

        public static void Load(string fileName)
        {
            if (File.Exists(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), fileName)))
            {
                try
                {
                    string xml = System.IO.File.ReadAllText(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), fileName));
                    System.IO.StringReader reader = new System.IO.StringReader(xml);
                    System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(ItemFilterData));
                    data = (ItemFilterData)serializer.Deserialize(reader);
                    reader.Close();
                }
                catch (Exception)
                {
                    data = new ItemFilterData();
                }
            }
            else
            {
                data = new ItemFilterData();
            }
        } 
#endif
    }
}
