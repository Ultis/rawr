using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace Rawr
{
    [Serializable]
    public class ItemFilterRegexList : BindingList<ItemFilterRegex>
    {
    }

    [Serializable]
    public class ItemFilterRegex
    {
        public ItemFilterRegex()
        {
            Name = "New Filter";
            Enabled = true;
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

        public bool Enabled { get; set; }

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
                    _regex = new Regex(_pattern, RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase | RegexOptions.Singleline);
                }
                return _regex;
            }
        }
    }

    [Serializable]
    public class ItemFilterData
    {
        public SerializableDictionary<string, List<Item.ItemType>> RelevantItemTypes = new SerializableDictionary<string, List<Item.ItemType>>();
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
            List<Item.ItemType> list;
            if (!data.RelevantItemTypes.TryGetValue(model.Name, out list))
            {
                list = new List<Item.ItemType>(model.RelevantItemTypes);
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
                bool anyMatch = false;
                bool enabledMatch = false;
                foreach (ItemFilterRegex regex in data.RegexList)
                {
                    if ((!string.IsNullOrEmpty(item.LocationInfo.Description) && regex.Regex.IsMatch(item.LocationInfo.Description)) || (!string.IsNullOrEmpty(item.LocationInfo.Note) && regex.Regex.IsMatch(item.LocationInfo.Note)))
                    {
                        anyMatch = true;
                        if (regex.Enabled)
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
            return false;
        }

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
    }
}
