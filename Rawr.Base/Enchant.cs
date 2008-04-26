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

        private static List<Enchant> _allEnchants = null;
        private static readonly string _SaveFilePath;


        static Enchant()
        {
            _SaveFilePath = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "EnchantCache.xml");
            LoadEnchants();
            SaveEnchants();
        }

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

        public override bool Equals(object obj)
        {
            Enchant temp = obj as Enchant;
            if (temp != null)
            {
                return Name.Equals(temp.Name) && Id == temp.Id && Slot == temp.Slot && Stats.Equals(temp.Stats);
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return (Name+Id.ToString()+Slot.ToString()+Stats.ToString()).GetHashCode();
        }

		/// <summary>
		/// A List<Enchant> containing all known enchants relevant to all models.
		/// </summary>
		public static List<Enchant> AllEnchants
		{
			get 
            {
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


		public static List<Enchant> FindEnchants(Item.ItemSlot slot, List<int> availableIds)
		{
			if (slot == Item.ItemSlot.OffHand || slot == Item.ItemSlot.TwoHand || slot == Item.ItemSlot.OneHand)
				slot = Item.ItemSlot.MainHand; //All enchants are defined for mainhand, currently
			return AllEnchants.FindAll(new Predicate<Enchant>(
				delegate(Enchant enchant)
				{
					return  enchant.Slot == Item.ItemSlot.None || (Calculations.HasRelevantStats(enchant.Stats) &&
						(enchant.Slot == slot || slot == Item.ItemSlot.None) && 
						availableIds.Contains(-1 * (enchant.Id + (10000 * (int)enchant.Slot))));
				}
			));
		}

        private static void SaveEnchants()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(_SaveFilePath, false, Encoding.UTF8))
                {
                    System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(List<Enchant>));
                    serializer.Serialize(writer, _allEnchants);
                    writer.Close();
                }
            }
            catch { }
        }

        private static void LoadEnchants()
        {

            try
            {
                if (File.Exists(_SaveFilePath))
                {
                    string xml = System.IO.File.ReadAllText(_SaveFilePath);
                    xml = xml.Replace("<Slot>Weapon</Slot", "<Slot>MainHand</Slot>");
                    System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(List<Enchant>));
                    System.IO.StringReader reader = new System.IO.StringReader(xml);
                    _allEnchants = (List<Enchant>)serializer.Deserialize(reader);
                    reader.Close();
                }
            }
            catch (Exception) { /* should ignore exception if there is a problem with the file */ }
            finally
            {
                if(_allEnchants == null)
                {
                    _allEnchants = new List<Enchant>();

                }
            }

            List<Enchant> defaultEnchants = GetDefaultEnchants();
            for (int defaultEnchantIndex = 0; defaultEnchantIndex < defaultEnchants.Count; defaultEnchantIndex++)
            {
                bool found = false;
                for (int allEnchantIndex = 0; allEnchantIndex < _allEnchants.Count; allEnchantIndex++)
                {
                    if (defaultEnchants[defaultEnchantIndex].Id == _allEnchants[allEnchantIndex].Id &&
                        defaultEnchants[defaultEnchantIndex].Slot == _allEnchants[allEnchantIndex].Slot &&
                        defaultEnchants[defaultEnchantIndex].Name == _allEnchants[allEnchantIndex].Name
                        )
                    {
                        if (defaultEnchants[defaultEnchantIndex].Stats != _allEnchants[allEnchantIndex].Stats)
                        {
                            if (defaultEnchants[defaultEnchantIndex].Stats == null)
                            {
                                _allEnchants.RemoveAt(allEnchantIndex);
                            }
                            else
                            {
                                _allEnchants[allEnchantIndex].Stats = defaultEnchants[defaultEnchantIndex].Stats;
                            }
                        }
                        found = true;
                        break;
                    }
                }
                if (!found && defaultEnchants[defaultEnchantIndex].Stats != null)
                {
                    _allEnchants.Add(defaultEnchants[defaultEnchantIndex]);
                }
            }
        }

        private static List<Enchant> GetDefaultEnchants()
        {
            List<Enchant> defaultEnchants = new List<Enchant>();
            defaultEnchants.Add(new Enchant(0, "No Enchant", Item.ItemSlot.None, new Stats()));
            defaultEnchants.Add(new Enchant(2999, "Glyph of the Defender", Item.ItemSlot.Head, new Stats() { DefenseRating = 16, DodgeRating = 17 }));
            defaultEnchants.Add(new Enchant(2991, "Greater Inscription of the Knight", Item.ItemSlot.Shoulders, new Stats() { DefenseRating = 15, DodgeRating = 10 }));
            defaultEnchants.Add(new Enchant(2990, "Inscription of the Knight", Item.ItemSlot.Shoulders, new Stats() { DefenseRating = 13 }));
            defaultEnchants.Add(new Enchant(2978, "Greater Inscription of Warding", Item.ItemSlot.Shoulders, new Stats() { DodgeRating = 15, DefenseRating = 10 }));
            defaultEnchants.Add(new Enchant(2977, "Inscription of Warding", Item.ItemSlot.Shoulders, new Stats() { DodgeRating = 13 }));
            defaultEnchants.Add(new Enchant(368, "Greater Agility", Item.ItemSlot.Back, new Stats() { Agility = 12 }));
            defaultEnchants.Add(new Enchant(2662, "Major Armor", Item.ItemSlot.Back, new Stats() { Armor = 120 }));
            defaultEnchants.Add(new Enchant(2622, "Dodge", Item.ItemSlot.Back, new Stats() { DodgeRating = 12 }));
            defaultEnchants.Add(new Enchant(2659, "Exceptional Health", Item.ItemSlot.Chest, new Stats() { Health = 150 }));
            defaultEnchants.Add(new Enchant(2661, "Exceptional Stats", Item.ItemSlot.Chest, new Stats() { Agility = 6, Strength = 6, Stamina = 6, Intellect = 6, Spirit = 6 }));
            defaultEnchants.Add(new Enchant(2933, "Major Resilience", Item.ItemSlot.Chest, new Stats() { Resilience = 15 }));
            defaultEnchants.Add(new Enchant(9001, "Major Defense", Item.ItemSlot.Chest, new Stats() { DefenseRating = 15 }));
            defaultEnchants.Add(new Enchant(2649, "Fortitude", Item.ItemSlot.Wrist, new Stats() { Stamina = 12 }));
            defaultEnchants.Add(new Enchant(1886, "Superior Stamina", Item.ItemSlot.Wrist, new Stats() { Stamina = 9 }));
            defaultEnchants.Add(new Enchant(2648, "Major Defense", Item.ItemSlot.Wrist, new Stats() { DefenseRating = 12 }));
            defaultEnchants.Add(new Enchant(1891, "Stats", Item.ItemSlot.Wrist, new Stats() { Agility = 4, Strength = 4, Stamina = 4, Intellect = 4, Spirit = 4 }));
            defaultEnchants.Add(new Enchant(2564, "Superior Agility", Item.ItemSlot.Hands, new Stats() { Agility = 15 }));
            defaultEnchants.Add(new Enchant(3011, "Clefthide Leg Armor", Item.ItemSlot.Legs, new Stats() { Agility = 10, Stamina = 30 }));
            defaultEnchants.Add(new Enchant(3013, "Nethercleft Leg Armor", Item.ItemSlot.Legs, new Stats() { Agility = 12, Stamina = 40 }));
            defaultEnchants.Add(new Enchant(2939, "Cat's Swiftness", Item.ItemSlot.Feet, new Stats() { Agility = 6 }));
            defaultEnchants.Add(new Enchant(2940, "Boar's Speed", Item.ItemSlot.Feet, new Stats() { Stamina = 9 }));
            defaultEnchants.Add(new Enchant(2657, "Dexterity", Item.ItemSlot.Feet, new Stats() { Agility = 12 }));
            defaultEnchants.Add(new Enchant(2649, "Fortitude", Item.ItemSlot.Feet, new Stats() { Stamina = 12 }));
            defaultEnchants.Add(new Enchant(2931, "Stats", Item.ItemSlot.Finger, new Stats() { Agility = 4, Strength = 4, Stamina = 4, Intellect = 4, Spirit = 4 }));
            defaultEnchants.Add(new Enchant(2670, "Major Agility", Item.ItemSlot.MainHand, new Stats() { Agility = 35 }));
            defaultEnchants.Add(new Enchant(2792, "Knothide Armor Kit", Item.ItemSlot.Chest, new Stats() { Stamina = 8 }));
            defaultEnchants.Add(new Enchant(2792, "Knothide Armor Kit", Item.ItemSlot.Legs, new Stats() { Stamina = 8 }));
            defaultEnchants.Add(new Enchant(2792, "Knothide Armor Kit", Item.ItemSlot.Hands, new Stats() { Stamina = 8 }));
            defaultEnchants.Add(new Enchant(2792, "Knothide Armor Kit", Item.ItemSlot.Feet, new Stats() { Stamina = 8 }));
            defaultEnchants.Add(new Enchant(2793, "Vindicator's Armor Kit", Item.ItemSlot.Chest, new Stats() { DefenseRating = 8 }));
            defaultEnchants.Add(new Enchant(2793, "Vindicator's Armor Kit", Item.ItemSlot.Hands, new Stats() { DefenseRating = 8 }));
            defaultEnchants.Add(new Enchant(2793, "Vindicator's Armor Kit", Item.ItemSlot.Legs, new Stats() { DefenseRating = 8 }));
            defaultEnchants.Add(new Enchant(2793, "Vindicator's Armor Kit", Item.ItemSlot.Feet, new Stats() { DefenseRating = 8 }));
            defaultEnchants.Add(new Enchant(2841, "Heavy Knothide Armor Kit", Item.ItemSlot.Head, new Stats() { Stamina = 10 }));
            defaultEnchants.Add(new Enchant(2841, "Heavy Knothide Armor Kit", Item.ItemSlot.Chest, new Stats() { Stamina = 10 }));
            defaultEnchants.Add(new Enchant(2841, "Heavy Knothide Armor Kit", Item.ItemSlot.Shoulders, new Stats() { Stamina = 10 }));
            defaultEnchants.Add(new Enchant(2841, "Heavy Knothide Armor Kit", Item.ItemSlot.Legs, new Stats() { Stamina = 10 }));
            defaultEnchants.Add(new Enchant(2841, "Heavy Knothide Armor Kit", Item.ItemSlot.Hands, new Stats() { Stamina = 10 }));
            defaultEnchants.Add(new Enchant(2841, "Heavy Knothide Armor Kit", Item.ItemSlot.Feet, new Stats() { Stamina = 10 }));
            defaultEnchants.Add(new Enchant(3260, "Glove Reinforcements", Item.ItemSlot.Hands, new Stats() { Armor = 240 }));
            defaultEnchants.Add(new Enchant(1441, "Greater Shadow Resistance", Item.ItemSlot.Back, new Stats() { ShadowResistance = 15 }));

            defaultEnchants.Add(new Enchant(2543, "Arcanum of Rapidity", Item.ItemSlot.Head, new Stats() { HasteRating = 10 }));
            defaultEnchants.Add(new Enchant(3003, "Glyph of Ferocity", Item.ItemSlot.Head, new Stats() { AttackPower = 34, HitRating = 16 }));
            defaultEnchants.Add(new Enchant(3096, "Glyph of the Outcast", Item.ItemSlot.Head, new Stats() { Strength = 17 }));
            defaultEnchants.Add(new Enchant(2716, "Fortitude of the Scourge", Item.ItemSlot.Shoulders, new Stats() { Stamina = 16, Armor = 100 }));
            defaultEnchants.Add(new Enchant(2717, "Might of the Scourge", Item.ItemSlot.Shoulders, new Stats() { AttackPower = 26, CritRating = 14 }));
            defaultEnchants.Add(new Enchant(2583, "Presence of Might", Item.ItemSlot.Head, new Stats() { Stamina = 10, DefenseRating = 10, BlockValue = 15 }));
            defaultEnchants.Add(new Enchant(2583, "Presence of Might", Item.ItemSlot.Legs, new Stats() { Stamina = 10, DefenseRating = 10, BlockValue = 15 }));
            defaultEnchants.Add(new Enchant(2997, "Greater Inscription of the Blade", Item.ItemSlot.Shoulders, new Stats() { AttackPower = 20, CritRating = 15 }));
            defaultEnchants.Add(new Enchant(2986, "Greater Inscription of Vengeance", Item.ItemSlot.Shoulders, new Stats() { AttackPower = 30, CritRating = 10 }));
            defaultEnchants.Add(new Enchant(2996, "Inscription of the Blade", Item.ItemSlot.Shoulders, new Stats() { CritRating = 13 }));
            defaultEnchants.Add(new Enchant(2983, "Inscription of Vengeance", Item.ItemSlot.Shoulders, new Stats() { AttackPower = 26 }));
            defaultEnchants.Add(new Enchant(2606, "Zandalar Signet of Might", Item.ItemSlot.Shoulders, new Stats() { AttackPower = 30 }));
            defaultEnchants.Add(new Enchant(2647, "Brawn", Item.ItemSlot.Wrist, new Stats() { Strength = 12 }));
            defaultEnchants.Add(new Enchant(684, "Major Strength", Item.ItemSlot.Hands, new Stats() { Strength = 15 }));
            defaultEnchants.Add(new Enchant(931, "Minor Haste", Item.ItemSlot.Hands, new Stats() { HasteRating = 10 }));
            defaultEnchants.Add(new Enchant(3012, "Nethercobra Leg Armor", Item.ItemSlot.Legs, new Stats() { AttackPower = 50, CritRating = 12 }));
            defaultEnchants.Add(new Enchant(3010, "Cobrahide Leg Armor", Item.ItemSlot.Legs, new Stats() { AttackPower = 40, CritRating = 10 }));
            defaultEnchants.Add(new Enchant(2658, "Surefooted", Item.ItemSlot.Feet, new Stats() { HitRating = 10 }));
            defaultEnchants.Add(new Enchant(2929, "Striking", Item.ItemSlot.Finger, new Stats() { WeaponDamage = 2 }));
            defaultEnchants.Add(new Enchant(2667, "Savagery", Item.ItemSlot.MainHand, new Stats() { AttackPower = 70 }));
            defaultEnchants.Add(new Enchant(1593, "Assault", Item.ItemSlot.Wrist, new Stats() { AttackPower = 24 }));
            defaultEnchants.Add(new Enchant(1594, "Assault", Item.ItemSlot.Hands, new Stats() { AttackPower = 26 }));
            defaultEnchants.Add(new Enchant(3222, "Greater Agility", Item.ItemSlot.MainHand, new Stats() { Agility = 20 }));
            defaultEnchants.Add(new Enchant(2722, "Adamantite Scope", Item.ItemSlot.Ranged, new Stats() { ScopeDamage = 10 }));
            defaultEnchants.Add(new Enchant(2723, "Khorium Scope", Item.ItemSlot.Ranged, new Stats() { ScopeDamage = 12 }));
            defaultEnchants.Add(new Enchant(2724, "Stabilized Eternium Scope", Item.ItemSlot.Ranged, new Stats() { CritRating = 28 }));

            defaultEnchants.Add(new Enchant(2621, "Subtlety", Item.ItemSlot.Back, new Stats() { ThreatReductionMultiplier = 0.02f }));
            defaultEnchants.Add(new Enchant(2613, "Threat", Item.ItemSlot.Hands, new Stats() { ThreatIncreaseMultiplier = 0.02f }));

            //spell stufff
            defaultEnchants.Add(new Enchant(2650, "Spellpower", Item.ItemSlot.Wrist, new Stats() { SpellDamageRating = 15 }));
            defaultEnchants.Add(new Enchant(2937, "Major Spellpower", Item.ItemSlot.Hands, new Stats() { SpellDamageRating = 20 }));
            defaultEnchants.Add(new Enchant(2935, "Spell Strike", Item.ItemSlot.Hands, new Stats() { SpellHitRating = 15 }));
            defaultEnchants.Add(new Enchant(2928, "Spellpower", Item.ItemSlot.Finger, new Stats() { SpellDamageRating = 12 }));
            defaultEnchants.Add(new Enchant(2669, "Major Spellpower", Item.ItemSlot.MainHand, new Stats() { SpellDamageRating = 40 }));
            defaultEnchants.Add(new Enchant(2660, "Exceptional Mana", Item.ItemSlot.Chest, new Stats() { Mana = 150 }));
            defaultEnchants.Add(new Enchant(2666, "Major Intellect", Item.ItemSlot.MainHand, new Stats() { Intellect = 30 }));
            defaultEnchants.Add(new Enchant(2679, "Restore Mana Prime", Item.ItemSlot.Wrist, new Stats() { Mp5 = 6 }));
            defaultEnchants.Add(new Enchant(2748, "Runic Spellthread", Item.ItemSlot.Legs, new Stats() { SpellDamageRating = 35, Stamina = 20 }));
            defaultEnchants.Add(new Enchant(2747, "Mystic Spellthread", Item.ItemSlot.Legs, new Stats() { SpellDamageRating = 25, Stamina = 15 }));
            defaultEnchants.Add(new Enchant(2982, "Greater Inscription of Discipline", Item.ItemSlot.Shoulders, new Stats() { SpellDamageRating = 18, SpellCritRating = 10 }));
            defaultEnchants.Add(new Enchant(2995, "Greater Inscription of the Orb", Item.ItemSlot.Shoulders, new Stats() { SpellDamageRating = 12, SpellCritRating = 15 }));
            defaultEnchants.Add(new Enchant(2981, "Inscription of Discipline", Item.ItemSlot.Shoulders, new Stats() { SpellDamageRating = 15 }));
            defaultEnchants.Add(new Enchant(2994, "Inscription of the Orb", Item.ItemSlot.Shoulders, new Stats() { SpellCritRating = 13 }));
            defaultEnchants.Add(new Enchant(3002, "Glyph of Power", Item.ItemSlot.Head, new Stats() { SpellDamageRating = 22, SpellHitRating = 14 }));
            defaultEnchants.Add(new Enchant(2671, "Sunfire", Item.ItemSlot.MainHand, new Stats() { SpellFireDamageRating = 50, SpellArcaneDamageRating = 50 }));
            defaultEnchants.Add(new Enchant(2672, "Soulfrost", Item.ItemSlot.MainHand, new Stats() { SpellFrostDamageRating = 54, SpellShadowDamageRating = 54 }));
            defaultEnchants.Add(new Enchant(2938, "Spell Penetration", Item.ItemSlot.Back, new Stats() { SpellPenetration = 20 }));
            defaultEnchants.Add(new Enchant(2656, "Vitality", Item.ItemSlot.Feet, new Stats() { Mp5 = 4, Hp5 = 4 }));
            defaultEnchants.Add(new Enchant(369, "Major Intellect", Item.ItemSlot.Wrist, new Stats() { Intellect = 12 }));
            defaultEnchants.Add(new Enchant(1144, "Major Spirit", Item.ItemSlot.Chest, new Stats() { Spirit = 15 }));
            defaultEnchants.Add(new Enchant(851, "Spirit", Item.ItemSlot.Feet, new Stats() { Spirit = 5 }));

            // Healing enchants (add spell damage too)
            defaultEnchants.Add(new Enchant(3001, "Glyph of Renewal", Item.ItemSlot.Head, new Stats() { Healing = 35, Mp5 = 7 }));

            defaultEnchants.Add(new Enchant(3150, "Restore Mana Prime", Item.ItemSlot.Chest, new Stats() { Mp5 = 6 }));

            defaultEnchants.Add(new Enchant(2980, "Greater Inscription of Faith", Item.ItemSlot.Shoulders, new Stats() { Healing = 33, Mp5 = 4 }));
            defaultEnchants.Add(new Enchant(2993, "Greater Inscription of the Oracle", Item.ItemSlot.Shoulders, new Stats() { Healing = 22, Mp5 = 6 }));
            defaultEnchants.Add(new Enchant(2979, "Inscription of Faith", Item.ItemSlot.Shoulders, new Stats() { Healing = 29 }));
            defaultEnchants.Add(new Enchant(2992, "Inscription of the Oracle", Item.ItemSlot.Shoulders, new Stats() { Mp5 = 5 }));
            defaultEnchants.Add(new Enchant(2715, "Resilience of the Scourge", Item.ItemSlot.Shoulders, new Stats() { Healing = 31, Mp5 = 5 }));
            defaultEnchants.Add(new Enchant(2604, "Zandalar Signet of Serenity", Item.ItemSlot.Shoulders, new Stats() { Healing = 33 }));

            defaultEnchants.Add(new Enchant(2322, "Major Healing", Item.ItemSlot.Hands, new Stats() { Healing = 35, SpellDamageRating = 12 }));
            defaultEnchants.Add(new Enchant(2934, "Blasting", Item.ItemSlot.Hands, new Stats() { SpellCritRating = 10 }));

            defaultEnchants.Add(new Enchant(2746, "Golden Spellthread", Item.ItemSlot.Legs, new Stats() { Healing = 66, Stamina = 20, SpellDamageRating = 22 }));
            defaultEnchants.Add(new Enchant(2745, "Silver Spellthread", Item.ItemSlot.Legs, new Stats() { Healing = 46, Stamina = 15, SpellDamageRating = 16 }));

            defaultEnchants.Add(new Enchant(2794, "Magister's Armor Kit", Item.ItemSlot.Chest, new Stats() { Mp5 = 3 }));
            defaultEnchants.Add(new Enchant(2794, "Magister's Armor Kit", Item.ItemSlot.Feet, new Stats() { Mp5 = 3 }));
            defaultEnchants.Add(new Enchant(2794, "Magister's Armor Kit", Item.ItemSlot.Legs, new Stats() { Mp5 = 3 }));
            defaultEnchants.Add(new Enchant(2794, "Magister's Armor Kit", Item.ItemSlot.Hands, new Stats() { Mp5 = 3 }));

            defaultEnchants.Add(new Enchant(2930, "Healing Power", Item.ItemSlot.Finger, new Stats() { Healing = 20, SpellDamageRating = 7 }));

            defaultEnchants.Add(new Enchant(2343, "Major Healing", Item.ItemSlot.MainHand, new Stats() { Healing = 81, SpellDamageRating = 27 }));

            defaultEnchants.Add(new Enchant(2654, "Intellect", Item.ItemSlot.MainHand, new Stats() { Intellect = 12 }));

            defaultEnchants.Add(new Enchant(2617, "Superior Healing", Item.ItemSlot.Wrist, new Stats() { Healing = 30, SpellDamageRating = 10 }));

            defaultEnchants.Add(new Enchant(2648, "Steelweave", Item.ItemSlot.Back, new Stats() { DefenseRating = 12 }));
            defaultEnchants.Add(new Enchant(3004, "Glyph of the Gladiator", Item.ItemSlot.Head, new Stats() { Stamina = 18, Resilience = 20 }));

            //The stat value of mongoose and executioner is dependent on the weapon speed and is thus left to the individual models to take care of through the Id
            defaultEnchants.Add(new Enchant(2673, "Mongoose", Item.ItemSlot.MainHand, new Stats() { MongooseProc = 1 }));
            defaultEnchants.Add(new Enchant(3225, "Executioner", Item.ItemSlot.MainHand, new Stats() { }));
            defaultEnchants.Add(new Enchant(2987, "Frost Armor Kit", Item.ItemSlot.Hands, new Stats() { FrostResistance = 8 }));
            defaultEnchants.Add(new Enchant(2987, "Frost Armor Kit", Item.ItemSlot.Feet, new Stats() { FrostResistance = 8 }));

            //Need to fix the enchant class to know offhand only enchants, either that or add a shield ItemSlot
            defaultEnchants.Add(new Enchant(1071, "Major Stamina", Item.ItemSlot.OffHand, new Stats() { Stamina = 18 }));
            defaultEnchants.Add(new Enchant(2655, "Shield Block", Item.ItemSlot.OffHand, new Stats() { BlockRating = 15 }));
            defaultEnchants.Add(new Enchant(2653, "Tough Shield", Item.ItemSlot.OffHand, new Stats() { BlockValue = 18 }));


            #region Enchants to Delete
            defaultEnchants.Add(new Enchant(2987, "Frost Armor Kit", Item.ItemSlot.Chest,null ));
            defaultEnchants.Add(new Enchant(2987, "Frost Armor Kit", Item.ItemSlot.Legs, null));
            #endregion
            return defaultEnchants;
        }
	}
}
