using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.IO;
using System.Text.RegularExpressions;
#if !SILVERLIGHT
using System.Windows.Forms;
#endif
using System.Xml.Serialization;

namespace Rawr
{
    public class ItemFilterRegexList : 
#if SILVERLIGHT   
    List<ItemFilterRegex>
#else
    BindingList<ItemFilterRegex>
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
            MinItemQuality = ItemQuality.Temp;
            MaxItemQuality = ItemQuality.Heirloom;
            AdditiveFilter = true;
            AppliesToItems = true;
            AppliesToGems = true;
        }

        public string Name { get; set; }
        [XmlIgnore]
        private string _pattern;

        public string Pattern {
            get { return _pattern; }
            set { _pattern = value; _regex = null; }
        }

        public int MinItemLevel { get; set; }
        public int MaxItemLevel { get; set; }
        public ItemQuality MinItemQuality { get; set; }
        public ItemQuality MaxItemQuality { get; set; }
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
                    _regex = new Regex(_pattern, 
#if SILVERLIGHT
#else
                        RegexOptions.Compiled | 
#endif
                        RegexOptions.CultureInvariant |
                        RegexOptions.IgnoreCase |
                        RegexOptions.Singleline);
                }
                return _regex;
            }
        }

        public bool IsMatch(Item item)
        {
            if (string.IsNullOrEmpty(_pattern)
                || (!string.IsNullOrEmpty(item.LocationInfo[0].Description) && Regex.IsMatch(item.LocationInfo[0].Description))
                || (!string.IsNullOrEmpty(item.LocationInfo[0].Note) && Regex.IsMatch(item.LocationInfo[0].Note))
                || (item.LocationInfo[1] != null && !string.IsNullOrEmpty(item.LocationInfo[1].Description) && Regex.IsMatch(item.LocationInfo[1].Description))
            )
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
            if (anyMatch) {
                return enabledMatch;
            } else {
                return OtherRegexEnabled;
            }
        }
    }

    public class ItemTypeList : List<ItemType>
    {
        public ItemTypeList() : base() { }
        public ItemTypeList(IEnumerable<ItemType> collection) : base(collection) { }
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
        public static List<ItemType> GetRelevantItemTypesList(CalculationsBase model)
        {
            ItemTypeList list;
            if (!data.RelevantItemTypes.TryGetValue(model.Name, out list))
            {
                list = new ItemTypeList(model.RelevantItemTypes);
                data.RelevantItemTypes[model.Name] = list;
            }
            return list;
        }

        public static ItemFilterRegexList RegexList { get { return data.RegexList; } }

        public static bool OtherRegexEnabled {
            get { return data.OtherRegexEnabled; }
            set { data.OtherRegexEnabled = value; }
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
        public static void Save(TextWriter writer)
        {
            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(ItemFilterData));
            serializer.Serialize(writer, data);
            writer.Close();
        }

        public static void Load(TextReader reader)
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
