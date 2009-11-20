using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.Serialization;

namespace Rawr
{
    
    public class SavedBuffSetList : List<SavedBuffSet>
    {
        public SavedBuffSetList() : base() { }
        public SavedBuffSetList(int capacity) : base(capacity) { }
    }

    public class SavedBuffSet
    {

        public string Name { get; set; }
        public string SetAsString { get; set; }
        public List<Buff> BuffSet { get; set; }

        public SavedBuffSet() : this("", null) { ; }

        private static SavedBuffSetList _AllSets;
        public static SavedBuffSetList AllSets { get { return _AllSets ?? (_AllSets = new SavedBuffSetList()); } private set { _AllSets = value; } }

        public static void Load(TextReader reader)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(SavedBuffSetList));
                AllSets = (SavedBuffSetList)serializer.Deserialize(reader);
                reader.Close();
            }
            catch { }
            finally
            {
                reader.Close();
                if (AllSets == null) AllSets = new SavedBuffSetList();
            }
        }

        public static void Save(TextWriter writer)
        {
            XmlSerializer serilizer = new XmlSerializer(typeof(SavedBuffSetList));
            serilizer.Serialize(writer, AllSets);
            writer.Close();
        }

        public SavedBuffSet(String name, List<Buff> buffSet)
        {
            Name = name;
            if (buffSet != null) {
                SetAsString = buffSet.ToString();
                BuffSet = buffSet;
            }
        }

        public override string ToString() { return Name; }

        public bool Equals(List<Buff> otherBuffSet)
        {
            if (otherBuffSet == null
                || SetAsString == null
                || BuffSet.Count != otherBuffSet.Count)
            { return false; }
            //return otherBuffSet.ToString().Equals(SetAsString);
            return otherBuffSet.Equals(BuffSet);
        }
    }
}
