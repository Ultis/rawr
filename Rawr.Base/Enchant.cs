using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Rawr
{
	public class Enchant
	{
		public int Id;
		public string Name;
		public Item.ItemSlot Slot = Item.ItemSlot.Head;
		public Stats Stats = new Stats();

		public Enchant() { }
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
					if (File.Exists(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "EnchantCache.xml")))
					{
						string xml = System.IO.File.ReadAllText(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "EnchantCache.xml"));
						System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(List<Enchant>));
						System.IO.StringReader reader = new System.IO.StringReader(xml);
						_allEnchants = (List<Enchant>)serializer.Deserialize(reader);
						reader.Close();
					}
					
					if (_allEnchants == null)
					{
						//Default Enchants
						_allEnchants = new List<Enchant>();
						_allEnchants.Add(new Enchant(0,    "No Enchant", Item.ItemSlot.None, new Stats()));
						_allEnchants.Add(new Enchant(2999, "Glyph of the Defender", Item.ItemSlot.Head, new Stats() { DefenseRating = 16, DodgeRating = 17 }));
						_allEnchants.Add(new Enchant(2991, "Greater Inscription of the Knight", Item.ItemSlot.Shoulders, new Stats() { DefenseRating = 15, DodgeRating = 10 }));
						_allEnchants.Add(new Enchant(2990, "Inscription of the Knight", Item.ItemSlot.Shoulders, new Stats() { DefenseRating = 13 }));
						_allEnchants.Add(new Enchant(2978, "Greater Inscription of Warding", Item.ItemSlot.Shoulders, new Stats() { DodgeRating = 15, DefenseRating = 10 }));
						_allEnchants.Add(new Enchant(2977, "Inscription of Warding", Item.ItemSlot.Shoulders, new Stats() { DodgeRating = 13 }));
						_allEnchants.Add(new Enchant(368,  "Greater Agility", Item.ItemSlot.Back, new Stats() { Agility = 12 }));
						_allEnchants.Add(new Enchant(2662, "Major Armor", Item.ItemSlot.Back, new Stats() { Armor = 120 }));
						_allEnchants.Add(new Enchant(2622, "Dodge", Item.ItemSlot.Back, new Stats() { DodgeRating = 12 }));
						_allEnchants.Add(new Enchant(2659, "Exceptional Health", Item.ItemSlot.Chest, new Stats() { Health = 150 }));
						_allEnchants.Add(new Enchant(2661, "Exceptional Stats", Item.ItemSlot.Chest, new Stats() { Agility = 6, Strength = 6, Stamina = 6 }));
						_allEnchants.Add(new Enchant(2933, "Major Resilience", Item.ItemSlot.Chest, new Stats() { Resilience = 15 }));
						_allEnchants.Add(new Enchant(9001, "Major Defense", Item.ItemSlot.Chest, new Stats() { DefenseRating = 15 }));
						_allEnchants.Add(new Enchant(2649, "Fortitude", Item.ItemSlot.Wrist, new Stats() { Stamina = 12 }));
						_allEnchants.Add(new Enchant(1886, "Superior Stamina", Item.ItemSlot.Wrist, new Stats() { Stamina = 9 }));
						_allEnchants.Add(new Enchant(2648, "Major Defense", Item.ItemSlot.Wrist, new Stats() { DefenseRating = 12 }));
						_allEnchants.Add(new Enchant(1891, "Stats", Item.ItemSlot.Wrist, new Stats() { Agility = 4, Strength = 4, Stamina = 4 }));
						_allEnchants.Add(new Enchant(2564, "Superior Agility", Item.ItemSlot.Hands, new Stats() { Agility = 15 }));
						_allEnchants.Add(new Enchant(3011, "Clefthide Leg Armor", Item.ItemSlot.Legs, new Stats() { Agility = 10, Stamina = 30 }));
						_allEnchants.Add(new Enchant(3013, "Nethercleft Leg Armor", Item.ItemSlot.Legs, new Stats() { Agility = 12, Stamina = 40 }));
						_allEnchants.Add(new Enchant(2939, "Cat's Swiftness", Item.ItemSlot.Feet, new Stats() { Agility = 6 }));
						_allEnchants.Add(new Enchant(2940, "Boar's Speed", Item.ItemSlot.Feet, new Stats() { Stamina = 9 }));
						_allEnchants.Add(new Enchant(2657, "Dexterity", Item.ItemSlot.Feet, new Stats() { Agility =  12 }));
						_allEnchants.Add(new Enchant(2649, "Fortitude", Item.ItemSlot.Feet, new Stats() { Stamina = 12 }));
						_allEnchants.Add(new Enchant(2931, "Stats", Item.ItemSlot.Finger, new Stats() { Agility = 4, Strength = 4, Stamina = 4 }));
						_allEnchants.Add(new Enchant(2670, "Major Agility", Item.ItemSlot.Weapon, new Stats() { Agility = 35 }));
						_allEnchants.Add(new Enchant(2792, "Knothide Armor Kit", Item.ItemSlot.Chest, new Stats() { Stamina = 8 }));
						_allEnchants.Add(new Enchant(2792, "Knothide Armor Kit", Item.ItemSlot.Legs, new Stats() { Stamina = 8 }));
						_allEnchants.Add(new Enchant(2792, "Knothide Armor Kit", Item.ItemSlot.Hands, new Stats() { Stamina = 8 }));
						_allEnchants.Add(new Enchant(2792, "Knothide Armor Kit", Item.ItemSlot.Feet, new Stats() { Stamina = 8 }));
						_allEnchants.Add(new Enchant(2793, "Vindicator's Armor Kit", Item.ItemSlot.Chest, new Stats() { DefenseRating = 8 }));
						_allEnchants.Add(new Enchant(2793, "Vindicator's Armor Kit", Item.ItemSlot.Hands, new Stats() { DefenseRating = 8 }));
						_allEnchants.Add(new Enchant(2793, "Vindicator's Armor Kit", Item.ItemSlot.Legs, new Stats() { DefenseRating = 8 }));
						_allEnchants.Add(new Enchant(2793, "Vindicator's Armor Kit", Item.ItemSlot.Feet, new Stats() { DefenseRating = 8 }));
						_allEnchants.Add(new Enchant(2841, "Heavy Knothide Armor Kit", Item.ItemSlot.Head, new Stats() { Stamina = 10 }));
						_allEnchants.Add(new Enchant(2841, "Heavy Knothide Armor Kit", Item.ItemSlot.Chest, new Stats() { Stamina = 10 }));
						_allEnchants.Add(new Enchant(2841, "Heavy Knothide Armor Kit", Item.ItemSlot.Shoulders, new Stats() { Stamina = 10 }));
						_allEnchants.Add(new Enchant(2841, "Heavy Knothide Armor Kit", Item.ItemSlot.Legs, new Stats() { Stamina = 10 }));
						_allEnchants.Add(new Enchant(2841, "Heavy Knothide Armor Kit", Item.ItemSlot.Hands, new Stats() { Stamina = 10 }));
						_allEnchants.Add(new Enchant(2841, "Heavy Knothide Armor Kit", Item.ItemSlot.Feet, new Stats() { Stamina = 10 }));

						_allEnchants.Add(new Enchant(2543, "Arcanum of Rapidity", Item.ItemSlot.Head, new Stats() { HasteRating = 10 }));
						_allEnchants.Add(new Enchant(3003, "Glyph of Ferocity", Item.ItemSlot.Head, new Stats() { AttackPower = 34, HitRating = 16 }));
						_allEnchants.Add(new Enchant(3096, "Glyph of the Outcast", Item.ItemSlot.Head, new Stats() { Strength = 17 }));
						_allEnchants.Add(new Enchant(2717, "Might of the Scourge", Item.ItemSlot.Shoulders, new Stats() { AttackPower = 26, CritRating = 14 }));
						_allEnchants.Add(new Enchant(2997, "Greater Inscription of the Blade", Item.ItemSlot.Shoulders, new Stats() { AttackPower = 20, CritRating = 15 }));
						_allEnchants.Add(new Enchant(2986, "Greater Inscription of Vengeance", Item.ItemSlot.Shoulders, new Stats() { AttackPower = 30, CritRating = 10 }));
						_allEnchants.Add(new Enchant(2996, "Inscription of the Blade", Item.ItemSlot.Shoulders, new Stats() { CritRating = 13 }));
						_allEnchants.Add(new Enchant(2983, "Inscription of Vengeance", Item.ItemSlot.Shoulders, new Stats() { AttackPower = 26 }));
						_allEnchants.Add(new Enchant(2606, "Zandalar Signet of Might", Item.ItemSlot.Shoulders, new Stats() { AttackPower = 30 }));
						_allEnchants.Add(new Enchant(2647, "Brawn", Item.ItemSlot.Wrist, new Stats() { Strength = 12 }));
						_allEnchants.Add(new Enchant(684, "Major Strength", Item.ItemSlot.Hands, new Stats() { Strength = 15 }));
						_allEnchants.Add(new Enchant(931, "Minor Haste", Item.ItemSlot.Hands, new Stats() { HasteRating = 10 }));
						_allEnchants.Add(new Enchant(3012, "Nethercobra Leg Armor", Item.ItemSlot.Legs, new Stats() { AttackPower = 50, CritRating = 12 }));
						_allEnchants.Add(new Enchant(3010, "Cobrahide Leg Armor", Item.ItemSlot.Legs, new Stats() { AttackPower = 40, CritRating = 10 }));
						_allEnchants.Add(new Enchant(2658, "Surefooted", Item.ItemSlot.Feet, new Stats() { HitRating = 10 }));
						_allEnchants.Add(new Enchant(2929, "Striking", Item.ItemSlot.Finger, new Stats() { WeaponDamage = 2 }));
						_allEnchants.Add(new Enchant(2667, "Savagery", Item.ItemSlot.Weapon, new Stats() { AttackPower = 70 }));

						
						

						System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(List<Enchant>));
						StringBuilder sb = new StringBuilder();
						System.IO.StringWriter writer = new System.IO.StringWriter(sb);
						serializer.Serialize(writer, _allEnchants);
						writer.Close();
						System.IO.File.WriteAllText(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "EnchantCache.xml"), sb.ToString());
					}
				}
				return _allEnchants;
			}
		}

		public static Enchant FindEnchant(int id, Item.ItemSlot slot)
		{
			return AllEnchants.Find(new Predicate<Enchant>(delegate(Enchant enchant) { return (enchant.Id == id) && (enchant.Slot == slot); })) ?? AllEnchants[0];
		}

		public static List<Enchant> FindEnchants(Item.ItemSlot slot)
		{
			return AllEnchants.FindAll(new Predicate<Enchant>(
				delegate(Enchant enchant)
				{
					return Calculations.HasRelevantStats(enchant.Stats) &&
						( enchant.Slot == slot || slot == Item.ItemSlot.None )
						|| enchant.Slot == Item.ItemSlot.None;
				}
			));
		}
	}
}
