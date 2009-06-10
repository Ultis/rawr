using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.Serialization;

namespace Rawr
{
    
    public class SavedTalentSpecList : List<SavedTalentSpec>
    {
        public SavedTalentSpecList() : base() { }
        public SavedTalentSpecList(int capacity) : base(capacity) { }
    }

    public class SavedTalentSpec
    {

        public string Name { get; set; }
        public Character.CharacterClass Class { get; set; }
        public string Spec { get; set; }

        public int Tree1 { get; set; }
        public int Tree2 { get; set; }
        public int Tree3 { get; set; }

        public SavedTalentSpec() : this("", null, 0, 0, 0) { ; }

        public static SavedTalentSpecList AllSpecs { get; private set; }
        public static void Load(StreamReader reader)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(SavedTalentSpecList));
                AllSpecs = (SavedTalentSpecList)serializer.Deserialize(reader);
                reader.Close();
            }
            catch { }
            finally
            {
                reader.Close();
                if (AllSpecs == null) AllSpecs = new SavedTalentSpecList();
            }
        }

        public static void Save(StreamWriter writer)
        {
            XmlSerializer serilizer = new XmlSerializer(typeof(SavedTalentSpecList));
            serilizer.Serialize(writer, AllSpecs);
            writer.Close();
        }

        public static SavedTalentSpecList SpecsFor(Character.CharacterClass charClass)
        {
            SavedTalentSpecList ret = new SavedTalentSpecList();
            foreach (SavedTalentSpec sts in AllSpecs)
            {
                if (sts.Class == charClass) ret.Add(sts);
            }
            return ret;
        }

        public SavedTalentSpec(String name, TalentsBase talentSpec, int tree1, int tree2, int tree3)
        {
            Name = name;
            Tree1 = tree1;
            Tree2 = tree2;
            Tree3 = tree3;
            if (talentSpec != null)
            {
                Spec = talentSpec.ToString();
                Class = talentSpec.GetClass();
            }
        }

        public TalentsBase TalentSpec()
        {
            if (Spec == null) return null;
            TalentsBase spec;
            if (Class == Character.CharacterClass.DeathKnight) spec = new DeathKnightTalents(Spec);
            else if (Class == Character.CharacterClass.Warrior) spec = new WarriorTalents(Spec);
            else if (Class == Character.CharacterClass.Paladin) spec = new PaladinTalents(Spec);
            else if (Class == Character.CharacterClass.Shaman) spec = new ShamanTalents(Spec);
            else if (Class == Character.CharacterClass.Hunter) spec = new HunterTalents(Spec);
            else if (Class == Character.CharacterClass.Rogue) spec = new RogueTalents(Spec);
            else if (Class == Character.CharacterClass.Druid) spec = new DruidTalents(Spec);
            else if (Class == Character.CharacterClass.Warlock) spec = new WarlockTalents(Spec);
            else if (Class == Character.CharacterClass.Priest) spec = new PriestTalents(Spec);
            else spec = new MageTalents(Spec);
            return spec;
        }

        public override string ToString()
        {
            return string.Format("{0} ({1}/{2}/{3})", Name, Tree1, Tree2, Tree3);
        }

        public bool Equals(TalentsBase talents)
        {
            if (talents == null || Spec == null) return false;
            return talents.ToString().Equals(Spec) && Class == talents.GetClass();
        }
    }
}
