using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Rawr
{
	/// <summary>
	/// An object representing an Enchantment to be placed on a slot on a character.
	/// </summary>
	public class Enchant
	{
		/// <summary>
		/// The ID of the enchant. This is determined by viewing the enchant spell on Wowhead, and
		/// noting the Enchant Item Permenant ID in the spell effect data.
		/// 
		/// EXAMPLE:
		/// Enchant Gloves - Superior Agility. This enchant is applied by spell 25080, which you can find
		/// by searching on Wowhead (http://www.wowhead.com/?spell=25080). In the spell Effect section, it
		/// says "Enchant Item Permanent (2564)". The Enchant ID is 2564.
		/// </summary>
		public int Id;

		/// <summary>
		/// The name of the enchant.
		/// </summary>
		public string Name;
		
		/// <summary>
		/// The slot that the enchant is applied to. If the enchant is available on multiple slots,
		/// define the enchant multiple times, once for each slot.
		/// 
		/// IMPORTANT: Currently, all weapon enchants should be defined as applying to the MainHand slot.
		/// </summary>
		public Item.ItemSlot Slot = Item.ItemSlot.Head;

		/// <summary>
		/// The stats that the enchant gives the character.
		/// </summary>
		public Stats Stats = new Stats();

		public Enchant() { }
		/// <summary>
		/// Creates a new Enchant, representing an enchant to a single slot.
		/// 
		/// EXAMPLE:
		/// new Enchant(2564, "Superior Agility", Item.ItemSlot.Hands, new Stats() { Agility = 15 })
		/// </summary>
		/// <param name="id">The Enchant ID for the enchant. See the Id property for details of how to find this.</param>
		/// <param name="name">The name of the enchant.</param>
		/// <param name="slot">The slot that this instance of the enchant applies to. (Create multiple Enchant
		/// objects for enchants which may be applied to multiple slots)</param>
		/// <param name="stats">The stats that the enchant gives the character.</param>
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
		/// <summary>
		/// A List<Enchant> containing all known enchants relevant to all models.
		/// </summary>
		public static List<Enchant> AllEnchants
		{
			get
			{
				if (_allEnchants == null)
				{
					//Rawr will load enchants from the EnchantCache file, if it exists, so that users can
					//modify/add to the enchants known to Rawr.
					if (File.Exists(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "EnchantCache.xml")))
					{
						string xml = System.IO.File.ReadAllText(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "EnchantCache.xml"));
						xml = xml.Replace("<Slot>Weapon</Slot", "<Slot>MainHand</Slot>");
						System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(List<Enchant>));
						System.IO.StringReader reader = new System.IO.StringReader(xml);
						_allEnchants = (List<Enchant>)serializer.Deserialize(reader);
						reader.Close();
					}
					
					//If enchants aren't loaded from EnchantCache, it will instead load from this list,
					//and then save to EnchantCache. This means that if you make changes here, you'll need
					//to either call this code through debugging, or simply delete your EnchantCache file,
					//in order to see your changes.
					if (_allEnchants == null)
					{
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
						_allEnchants.Add(new Enchant(2661, "Exceptional Stats", Item.ItemSlot.Chest, new Stats() { Agility = 6, Strength = 6, Stamina = 6, Intellect = 6, Spirit = 6 }));
						_allEnchants.Add(new Enchant(2933, "Major Resilience", Item.ItemSlot.Chest, new Stats() { Resilience = 15 }));
						_allEnchants.Add(new Enchant(9001, "Major Defense", Item.ItemSlot.Chest, new Stats() { DefenseRating = 15 }));
						_allEnchants.Add(new Enchant(2649, "Fortitude", Item.ItemSlot.Wrist, new Stats() { Stamina = 12 }));
						_allEnchants.Add(new Enchant(1886, "Superior Stamina", Item.ItemSlot.Wrist, new Stats() { Stamina = 9 }));
						_allEnchants.Add(new Enchant(2648, "Major Defense", Item.ItemSlot.Wrist, new Stats() { DefenseRating = 12 }));
						_allEnchants.Add(new Enchant(1891, "Stats", Item.ItemSlot.Wrist, new Stats() { Agility = 4, Strength = 4, Stamina = 4, Intellect = 4, Spirit = 4 }));
						_allEnchants.Add(new Enchant(2564, "Superior Agility", Item.ItemSlot.Hands, new Stats() { Agility = 15 }));
						_allEnchants.Add(new Enchant(3011, "Clefthide Leg Armor", Item.ItemSlot.Legs, new Stats() { Agility = 10, Stamina = 30 }));
						_allEnchants.Add(new Enchant(3013, "Nethercleft Leg Armor", Item.ItemSlot.Legs, new Stats() { Agility = 12, Stamina = 40 }));
						_allEnchants.Add(new Enchant(2939, "Cat's Swiftness", Item.ItemSlot.Feet, new Stats() { Agility = 6 }));
						_allEnchants.Add(new Enchant(2940, "Boar's Speed", Item.ItemSlot.Feet, new Stats() { Stamina = 9 }));
						_allEnchants.Add(new Enchant(2657, "Dexterity", Item.ItemSlot.Feet, new Stats() { Agility =  12 }));
						_allEnchants.Add(new Enchant(2649, "Fortitude", Item.ItemSlot.Feet, new Stats() { Stamina = 12 }));
						_allEnchants.Add(new Enchant(2931, "Stats", Item.ItemSlot.Finger, new Stats() { Agility = 4, Strength = 4, Stamina = 4, Intellect = 4, Spirit = 4 }));
						_allEnchants.Add(new Enchant(2670, "Major Agility", Item.ItemSlot.MainHand, new Stats() { Agility = 35 }));
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
						_allEnchants.Add(new Enchant(3260, "Glove Reinforcements", Item.ItemSlot.Hands, new Stats() { Armor = 240 }));
                        _allEnchants.Add(new Enchant(1441, "Greater Shadow Resistance", Item.ItemSlot.Back, new Stats() { ShadowResistance = 15 }));
						
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
						_allEnchants.Add(new Enchant(684,  "Major Strength", Item.ItemSlot.Hands, new Stats() { Strength = 15 }));
						_allEnchants.Add(new Enchant(931,  "Minor Haste", Item.ItemSlot.Hands, new Stats() { HasteRating = 10 }));
						_allEnchants.Add(new Enchant(3012, "Nethercobra Leg Armor", Item.ItemSlot.Legs, new Stats() { AttackPower = 50, CritRating = 12 }));
						_allEnchants.Add(new Enchant(3010, "Cobrahide Leg Armor", Item.ItemSlot.Legs, new Stats() { AttackPower = 40, CritRating = 10 }));
						_allEnchants.Add(new Enchant(2658, "Surefooted", Item.ItemSlot.Feet, new Stats() { HitRating = 10 }));
						_allEnchants.Add(new Enchant(2929, "Striking", Item.ItemSlot.Finger, new Stats() { WeaponDamage = 2 }));
						_allEnchants.Add(new Enchant(2667, "Savagery", Item.ItemSlot.MainHand, new Stats() { AttackPower = 70 }));
                        _allEnchants.Add(new Enchant(1593, "Assault", Item.ItemSlot.Wrist, new Stats() { AttackPower = 24 }));
                        _allEnchants.Add(new Enchant(1594, "Assault", Item.ItemSlot.Hands, new Stats() { AttackPower = 26 }));
                        _allEnchants.Add(new Enchant(3222, "Greater Agility", Item.ItemSlot.MainHand, new Stats() { Agility = 20 }));

                        //spell stufff
                        _allEnchants.Add(new Enchant(2650, "Spellpower", Item.ItemSlot.Wrist, new Stats() {SpellDamageRating = 15}));
                        _allEnchants.Add(new Enchant(2937, "Major Spellpower", Item.ItemSlot.Hands, new Stats() { SpellDamageRating = 20 }));
                        _allEnchants.Add(new Enchant(2935, "Spell Strike", Item.ItemSlot.Hands, new Stats() { SpellHitRating = 15}));
                        _allEnchants.Add(new Enchant(2928, "Spellpower", Item.ItemSlot.Finger, new Stats() { SpellDamageRating = 12 }));
                        _allEnchants.Add(new Enchant(2669, "Major Spellpower", Item.ItemSlot.MainHand, new Stats() { SpellDamageRating = 40 }));
                        _allEnchants.Add(new Enchant(2660, "Exceptional Mana", Item.ItemSlot.Chest, new Stats() { Mana = 150 }));
                        _allEnchants.Add(new Enchant(2666, "Major Intellect", Item.ItemSlot.MainHand, new Stats() { Intellect = 30}));
                        _allEnchants.Add(new Enchant(2679, "Restore Mana Prime", Item.ItemSlot.Wrist, new Stats() { Mp5 = 6 }));
                        _allEnchants.Add(new Enchant(2748, "Runic Spellthread", Item.ItemSlot.Legs, new Stats() { SpellDamageRating = 35, Stamina = 20 }));
                        _allEnchants.Add(new Enchant(2747, "Mystic Spellthread", Item.ItemSlot.Legs, new Stats() { SpellDamageRating = 25, Stamina = 15 }));
                        _allEnchants.Add(new Enchant(2982, "Greater Inscription of Discipline", Item.ItemSlot.Shoulders, new Stats() { SpellDamageRating = 18, SpellCritRating = 10 }));
                        _allEnchants.Add(new Enchant(2995, "Greater Inscription of the Orb", Item.ItemSlot.Shoulders, new Stats() { SpellDamageRating = 12, SpellCritRating = 15 }));
                        _allEnchants.Add(new Enchant(2981, "Inscription of Discipline", Item.ItemSlot.Shoulders, new Stats() { SpellDamageRating = 15 }));
                        _allEnchants.Add(new Enchant(2994, "Inscription of the Orb", Item.ItemSlot.Shoulders, new Stats() { SpellCritRating = 13 }));
                        _allEnchants.Add(new Enchant(3002, "Glyph of Power", Item.ItemSlot.Head, new Stats() { SpellDamageRating = 22, SpellHitRating = 14 }));
                        _allEnchants.Add(new Enchant(2671, "Sunfire", Item.ItemSlot.MainHand, new Stats() { SpellFireDamageRating = 50, SpellArcaneDamageRating = 50 }));
                        _allEnchants.Add(new Enchant(2672, "Soulfrost", Item.ItemSlot.MainHand, new Stats() { SpellFrostDamageRating = 54, SpellShadowDamageRating = 54 }));
                        _allEnchants.Add(new Enchant(2938, "Spell Penetration", Item.ItemSlot.Back, new Stats() { SpellPenetration = 20 }));
                        _allEnchants.Add(new Enchant(2656, "Vitality", Item.ItemSlot.Feet, new Stats() { Mp5 = 4, Hp5 = 4 }));
                        _allEnchants.Add(new Enchant(369, "Major Intellect", Item.ItemSlot.Wrist, new Stats() { Intellect = 12 }));
                        _allEnchants.Add(new Enchant(1144, "Major Spirit", Item.ItemSlot.Chest, new Stats() { Spirit = 15 }));
                        _allEnchants.Add(new Enchant(851, "Spirit", Item.ItemSlot.Feet, new Stats() { Spirit = 5 }));

                        // Healing enchants (add spell damage too)
                        _allEnchants.Add(new Enchant(2617, "Superior Healing", Item.ItemSlot.Wrist, new Stats() { SpellDamageRating = 10 }));
                        _allEnchants.Add(new Enchant(2322, "Major Healing", Item.ItemSlot.Hands, new Stats() { SpellDamageRating = 12 }));
                        _allEnchants.Add(new Enchant(2930, "Healing Power", Item.ItemSlot.Finger, new Stats() { SpellDamageRating = 7 }));
                        _allEnchants.Add(new Enchant(2343, "Major Healing", Item.ItemSlot.MainHand, new Stats() { SpellDamageRating = 27 }));

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
			if (slot == Item.ItemSlot.OffHand || slot == Item.ItemSlot.TwoHand || slot == Item.ItemSlot.OneHand) 
				slot = Item.ItemSlot.MainHand; //All enchants are defined for mainhand, currently
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
