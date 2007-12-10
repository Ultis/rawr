using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace Rawr
{
    public class Enchant
    {
        public int Id;
        public string Name;
        public Item.ItemSlot Slot = Item.ItemSlot.Head;
        public Stats Stats = new Stats();

        public Enchant()
        {
        }

        public Enchant(int id, string name, Item.ItemSlot slot, Stats stats)
        {
            Id = id;
            Name = name;
            Slot = slot;
            Stats = stats;
        }

        public override string ToString()
        {
            string summary = Name + ": ";
            summary += Stats.ToString();
            summary = summary.TrimEnd(',', ' ', ':');
            return summary;
        }

        private static List<Enchant> _allEnchants = null;

        public static List<Enchant> AllEnchants
        {
            get
            {
                if (_allEnchants == null)
                {
                    if (File.Exists(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "EnchantCache.xml"))
                        )
                    {
                        string xml =
                            File.ReadAllText(
                                Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "EnchantCache.xml"));
                        XmlSerializer serializer = new XmlSerializer(typeof (List<Enchant>));
                        StringReader reader = new StringReader(xml);
                        _allEnchants = (List<Enchant>) serializer.Deserialize(reader);
                        reader.Close();
                    }
                    else
                    {
                        //Default Enchants
                        _allEnchants = new List<Enchant>();
                        _allEnchants.Add(
                            new Enchant(0, "No Enchant", Item.ItemSlot.None, new Stats(0, 0, 0, 0, 0, 0, 0)));
                        _allEnchants.Add(
                            new Enchant(2999, "Glyph of the Defender", Item.ItemSlot.Head,
                                        new Stats(0, 0, 0, 0, 17, 16, 0)));
                        _allEnchants.Add(
                            new Enchant(2991, "Greater Inscription of the Knight", Item.ItemSlot.Shoulders,
                                        new Stats(0, 0, 0, 0, 10, 15, 0)));
                        _allEnchants.Add(
                            new Enchant(2990, "Inscription of the Knight", Item.ItemSlot.Shoulders,
                                        new Stats(0, 0, 0, 0, 0, 13, 0)));
                        _allEnchants.Add(
                            new Enchant(2978, "Greater Inscription of Warding", Item.ItemSlot.Shoulders,
                                        new Stats(0, 0, 0, 0, 15, 10, 0)));
                        _allEnchants.Add(
                            new Enchant(2977, "Inscription of Warding", Item.ItemSlot.Shoulders,
                                        new Stats(0, 0, 0, 0, 13, 0, 0)));
                        _allEnchants.Add(
                            new Enchant(368, "Greater Agility", Item.ItemSlot.Back, new Stats(0, 0, 12, 0, 0, 0, 0)));
                        _allEnchants.Add(
                            new Enchant(2662, "Major Armor", Item.ItemSlot.Back, new Stats(120, 0, 0, 0, 0, 0, 0)));
                        _allEnchants.Add(new Enchant(2622, "Dodge", Item.ItemSlot.Back, new Stats(0, 0, 0, 0, 12, 0, 0)));
                        _allEnchants.Add(
                            new Enchant(2659, "Exceptional Health", Item.ItemSlot.Chest,
                                        new Stats(0, 150, 0, 0, 0, 0, 0)));
                        _allEnchants.Add(
                            new Enchant(2661, "Exceptional Stats", Item.ItemSlot.Chest, new Stats(0, 0, 6, 6, 0, 0, 0)));
                        _allEnchants.Add(
                            new Enchant(2933, "Major Resilience", Item.ItemSlot.Chest, new Stats(0, 0, 0, 0, 0, 0, 15)));
                        _allEnchants.Add(
                            new Enchant(2793, "Vindicator's Armor Kit", Item.ItemSlot.Chest,
                                        new Stats(0, 0, 0, 0, 0, 8, 0)));
                        _allEnchants.Add(
                            new Enchant(2649, "Fortitude", Item.ItemSlot.Wrist, new Stats(0, 0, 0, 12, 0, 0, 0)));
                        _allEnchants.Add(
                            new Enchant(1886, "Superior Stamina", Item.ItemSlot.Wrist, new Stats(0, 0, 0, 9, 0, 0, 0)));
                        _allEnchants.Add(
                            new Enchant(2648, "Major Defense", Item.ItemSlot.Wrist, new Stats(0, 0, 0, 0, 0, 12, 0)));
                        _allEnchants.Add(new Enchant(1891, "Stats", Item.ItemSlot.Wrist, new Stats(0, 0, 4, 4, 0, 0, 0)));
                        _allEnchants.Add(
                            new Enchant(2564, "Superior Agility", Item.ItemSlot.Hands, new Stats(0, 0, 15, 0, 0, 0, 0)));
                        _allEnchants.Add(
                            new Enchant(2792, "Knothide Armor Kit", Item.ItemSlot.Hands, new Stats(0, 0, 0, 8, 0, 0, 0)));
                        _allEnchants.Add(
                            new Enchant(2793, "Vindicator's Armor Kit", Item.ItemSlot.Hands,
                                        new Stats(0, 0, 0, 0, 0, 8, 0)));
                        _allEnchants.Add(
                            new Enchant(3011, "Clefthide Leg Armor", Item.ItemSlot.Legs,
                                        new Stats(0, 0, 10, 30, 0, 0, 0)));
                        _allEnchants.Add(
                            new Enchant(3013, "Nethercleft Leg Armor", Item.ItemSlot.Legs,
                                        new Stats(0, 0, 12, 40, 0, 0, 0)));
                        _allEnchants.Add(
                            new Enchant(2793, "Vindicator's Armor Kit", Item.ItemSlot.Legs,
                                        new Stats(0, 0, 0, 0, 0, 8, 0)));
                        _allEnchants.Add(
                            new Enchant(2939, "Cat's Swiftness", Item.ItemSlot.Feet, new Stats(0, 0, 6, 0, 0, 0, 0)));
                        _allEnchants.Add(
                            new Enchant(2940, "Boar's Speed", Item.ItemSlot.Feet, new Stats(0, 0, 0, 9, 0, 0, 0)));
                        _allEnchants.Add(
                            new Enchant(2657, "Dexterity", Item.ItemSlot.Feet, new Stats(0, 0, 12, 0, 0, 0, 0)));
                        _allEnchants.Add(
                            new Enchant(2649, "Fortitude", Item.ItemSlot.Feet, new Stats(0, 0, 0, 12, 0, 0, 0)));
                        _allEnchants.Add(
                            new Enchant(2793, "Vindicator's Armor Kit", Item.ItemSlot.Feet,
                                        new Stats(0, 0, 0, 0, 0, 8, 0)));
                        _allEnchants.Add(
                            new Enchant(2931, "Stats", Item.ItemSlot.Finger, new Stats(0, 0, 4, 4, 0, 0, 0)));
                        _allEnchants.Add(
                            new Enchant(2670, "Major Agility", Item.ItemSlot.Weapon, new Stats(0, 0, 35, 0, 0, 0, 0)));

                        XmlSerializer serializer = new XmlSerializer(typeof (List<Enchant>));
                        StringBuilder sb = new StringBuilder();
                        StringWriter writer = new StringWriter(sb);
                        serializer.Serialize(writer, _allEnchants);
                        writer.Close();
                        File.WriteAllText(
                            Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "EnchantCache.xml"),
                            sb.ToString());
                    }
                }
                return _allEnchants;
            }
        }

        public static Enchant FindEnchant(int id, Item.ItemSlot slot)
        {
            return AllEnchants.Find(new Predicate<Enchant>(delegate(Enchant enchant)
                                                               {
                                                                   return (enchant.Id == id) && (enchant.Slot == slot);
                                                               })) ?? AllEnchants[0];
        }

        public static List<Enchant> FindEnchants(Item.ItemSlot slot)
        {
            return AllEnchants.FindAll(new Predicate<Enchant>(delegate(Enchant enchant)
                                                                  {
                                                                      return
                                                                          enchant.Slot == slot ||
                                                                          enchant.Slot == Item.ItemSlot.None ||
                                                                          slot == Item.ItemSlot.None;
                                                                  }));
        }
    }
}