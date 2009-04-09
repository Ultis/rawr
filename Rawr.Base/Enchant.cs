using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Rawr
{
    public class EnchantList : List<Enchant>
    {
    }

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
        /// A shortened version of the enchant's Name.
        /// </summary>
        public string ShortName
        {
            get
            {
                string shortName = Name.Replace("Arcanum of the ", "").Replace("Arcanum of ", "").Replace("Inscription of the ", "")
                    .Replace("Inscription of ", "").Replace("Greater", "Great").Replace("Exceptional", "Excep").Replace("Defense", "Def")
                    .Replace("Armor Kit", "").Replace("Arcanum of ", "").Replace(" Leg Armor", "").Replace(" Scope", "")
					.Replace(" Spellthread", "").Replace("Lining - ", "").Replace("Strength", "Str").Replace("Agility", "Agi")
					.Replace("Stamina", "Sta").Replace("Intellect", "Int").Replace("Spirit", "Spr").Replace("Heavy", "Hvy")
					.Replace("Jormungar Leg Reinforcements", "Jorm Reinf").Replace(" Leg Reinforcements", "")
					.Replace("Powerful", "Power").Replace("Swiftness", "Swift").Replace("Knothide", "Knot")
					.Replace("Savage", "Sav").Replace("Mighty Armor", "Mighty Arm").Replace("Shadow Armor", "Shadow Arm")
					.Replace("Attack Power", "AP").Replace("Rune of the ", "").Replace(" Gargoyle", "")
					.Replace("speed Accelerators", "").Replace(" Mysteries", "").Replace(" Embroidery", "")
					.Replace("Mana Restoration", "Mp5").Replace("Restore Mana", "Mp5");
                return shortName.Substring(0, Math.Min(shortName.Length, 12));
            }
        }

        /// <summary>
        /// The slot that the enchant is applied to. If the enchant is available on multiple slots,
        /// define the enchant multiple times, once for each slot.
        /// 
        /// IMPORTANT: Shield enchants should be defined 
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
            _SaveFilePath = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "Data" + System.IO.Path.DirectorySeparatorChar + "EnchantCache.xml");
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
            return (Name + Id.ToString() + Slot.ToString() + Stats.ToString()).GetHashCode();
        }

        public bool FitsInSlot(Item.ItemSlot slot)
        {
            return (Slot == slot ||
                (Slot == Item.ItemSlot.OneHand && (slot == Item.ItemSlot.OffHand || slot == Item.ItemSlot.MainHand || slot == Item.ItemSlot.TwoHand)) ||
                (Slot == Item.ItemSlot.TwoHand && (slot == Item.ItemSlot.MainHand)));
        }

        public bool FitsInSlot(Item.ItemSlot slot, Character character)
        {
            return Calculations.EnchantFitsInSlot(this, character, slot);
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

        public static Enchant FindEnchant(int id, Item.ItemSlot slot, Character character)
        {
            //List<Item.ItemSlot> validSlots = new List<Item.ItemSlot>();
            //if (slot != Item.ItemSlot.MainHand)
            //    validSlots.Add(slot);
            //if (slot == Item.ItemSlot.OffHand || slot == Item.ItemSlot.MainHand || slot == Item.ItemSlot.TwoHand)
            //    validSlots.Add(Item.ItemSlot.OneHand);
            //if (slot == Item.ItemSlot.MainHand)
            //    validSlots.Add(Item.ItemSlot.TwoHand);
            return AllEnchants.Find(new Predicate<Enchant>(delegate(Enchant enchant)
            {
                return (enchant.Id == id) && (enchant.FitsInSlot(slot, character) ||
                  (enchant.Slot == Item.ItemSlot.TwoHand && slot == Item.ItemSlot.OneHand));
            })) ?? AllEnchants[0];
        }

        public static List<Enchant> FindEnchants(Item.ItemSlot slot, Character character)
        {
            return FindEnchants(slot, character, Calculations.Instance);
        }

        public static List<Enchant> FindEnchants(Item.ItemSlot slot, Character character, CalculationsBase model)
        {
            //List<Item.ItemSlot> validSlots = new List<Item.ItemSlot>();
            //if (slot != Item.ItemSlot.MainHand)
            //    validSlots.Add(slot);
            //if (slot == Item.ItemSlot.OffHand || slot == Item.ItemSlot.MainHand || slot == Item.ItemSlot.TwoHand)
            //    validSlots.Add(Item.ItemSlot.OneHand);
            //if (slot == Item.ItemSlot.MainHand)
            //    validSlots.Add(Item.ItemSlot.TwoHand);
            return AllEnchants.FindAll(new Predicate<Enchant>(
                delegate(Enchant enchant)
                {
                    return model.IsEnchantRelevant(enchant) &&
                        (enchant.FitsInSlot(slot, character) || slot == Item.ItemSlot.None)
                        || enchant.Slot == Item.ItemSlot.None;
                }
            ));
        }

        public static List<Enchant> FindAllEnchants(Item.ItemSlot slot, Character character)
        {
            //List<Item.ItemSlot> validSlots = new List<Item.ItemSlot>();
            //if (slot != Item.ItemSlot.MainHand)
            //    validSlots.Add(slot);
            //if (slot == Item.ItemSlot.OffHand || slot == Item.ItemSlot.MainHand || slot == Item.ItemSlot.TwoHand)
            //    validSlots.Add(Item.ItemSlot.OneHand);
            //if (slot == Item.ItemSlot.MainHand)
            //    validSlots.Add(Item.ItemSlot.TwoHand);
            return AllEnchants.FindAll(new Predicate<Enchant>(
                delegate(Enchant enchant)
                {
                    return (enchant.FitsInSlot(slot, character) || slot == Item.ItemSlot.None)
                        || enchant.Slot == Item.ItemSlot.None;
                }
            ));
        }

        public static List<Enchant> FindEnchants(Item.ItemSlot slot, Character character, List<string> availableIds)
        {
            return FindEnchants(slot, character, availableIds, Calculations.Instance);
        }

        public static List<Enchant> FindEnchants(Item.ItemSlot slot, Character character, List<string> availableIds, CalculationsBase model)
        {
            return AllEnchants.FindAll(new Predicate<Enchant>(
                delegate(Enchant enchant)
                {
                    return ((model.IsEnchantRelevant(enchant) &&
                        (enchant.FitsInSlot(slot, character) || slot == Item.ItemSlot.None) || enchant.Slot == Item.ItemSlot.None)
                        && availableIds.Contains((-1 * (enchant.Id + (10000 * (int)enchant.Slot))).ToString()))
                        || enchant.Id == 0;
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
                    System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(EnchantList));
                    System.IO.StringReader reader = new System.IO.StringReader(xml);
                    _allEnchants = (List<Enchant>)serializer.Deserialize(reader);
                    reader.Close();
                }
            }
            catch (Exception) { /* should ignore exception if there is a problem with the file */ }
            finally
            {
                if (_allEnchants == null)
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
            defaultEnchants.Add(new Enchant(2999, "Arcanum of the Defender", Item.ItemSlot.Head, new Stats() { DefenseRating = 16, DodgeRating = 17 }));
            defaultEnchants.Add(new Enchant(3818, "Arcanum of the Stalwart Protector", Item.ItemSlot.Head, new Stats() { Stamina = 37, DefenseRating = 20 }));
            defaultEnchants.Add(new Enchant(2991, "Greater Inscription of the Knight", Item.ItemSlot.Shoulders, new Stats() { DefenseRating = 15, DodgeRating = 10 }));
            defaultEnchants.Add(new Enchant(2990, "Inscription of the Knight", Item.ItemSlot.Shoulders, new Stats() { DefenseRating = 13 }));
            defaultEnchants.Add(new Enchant(2978, "Greater Inscription of Warding", Item.ItemSlot.Shoulders, new Stats() { DodgeRating = 15, DefenseRating = 10 }));
            defaultEnchants.Add(new Enchant(2977, "Inscription of Warding", Item.ItemSlot.Shoulders, new Stats() { DodgeRating = 13 }));
            defaultEnchants.Add(new Enchant(368, "Greater Agility", Item.ItemSlot.Back, new Stats() { Agility = 12 }));
            defaultEnchants.Add(new Enchant(983, "Superior Agility", Item.ItemSlot.Back, new Stats() { Agility = 16 }));
            defaultEnchants.Add(new Enchant(1099, "Major Agility", Item.ItemSlot.Back, new Stats() { Agility = 22 }));
            defaultEnchants.Add(new Enchant(3256, "Shadow Armor", Item.ItemSlot.Back, new Stats() { Agility = 10 }));
            defaultEnchants.Add(new Enchant(3825, "Speed", Item.ItemSlot.Back, new Stats() { HasteRating = 15 }));
            defaultEnchants.Add(new Enchant(3831, "Greater Speed", Item.ItemSlot.Back, new Stats() { HasteRating = 23 }));
            defaultEnchants.Add(new Enchant(2662, "Major Armor", Item.ItemSlot.Back, new Stats() { BonusArmor = 120 }));
            defaultEnchants.Add(new Enchant(3294, "Mighty Armor", Item.ItemSlot.Back, new Stats() { BonusArmor = 225 }));
            defaultEnchants.Add(new Enchant(2622, "Dodge", Item.ItemSlot.Back, new Stats() { DodgeRating = 12 }));
            defaultEnchants.Add(new Enchant(2659, "Exceptional Health", Item.ItemSlot.Chest, new Stats() { Health = 150 }));
            defaultEnchants.Add(new Enchant(3236, "Mighty Health", Item.ItemSlot.Chest, new Stats() { Health = 200 }));
            defaultEnchants.Add(new Enchant(3297, "Super Health", Item.ItemSlot.Chest, new Stats() { Health = 275 }));
            defaultEnchants.Add(new Enchant(2661, "Exceptional Stats", Item.ItemSlot.Chest, new Stats() { Agility = 6, Strength = 6, Stamina = 6, Intellect = 6, Spirit = 6 }));
            defaultEnchants.Add(new Enchant(3252, "Super Stats", Item.ItemSlot.Chest, new Stats() { Agility = 8, Strength = 8, Stamina = 8, Intellect = 8, Spirit = 8 }));
            defaultEnchants.Add(new Enchant(3832, "Powerful Stats", Item.ItemSlot.Chest, new Stats() { Agility = 10, Strength = 10, Stamina = 10, Intellect = 10, Spirit = 10 }));
            defaultEnchants.Add(new Enchant(2933, "Major Resilience", Item.ItemSlot.Chest, new Stats() { Resilience = 15 }));
            defaultEnchants.Add(new Enchant(3245, "Exceptional Resilience", Item.ItemSlot.Chest, new Stats() { Resilience = 20 }));
            defaultEnchants.Add(new Enchant(1950, "Major Defense", Item.ItemSlot.Chest, new Stats() { DefenseRating = 15 }));
            defaultEnchants.Add(new Enchant(1953, "Greater Defense", Item.ItemSlot.Chest, new Stats() { DefenseRating = 22 }));
            defaultEnchants.Add(new Enchant(2649, "Fortitude", Item.ItemSlot.Wrist, new Stats() { Stamina = 12 }));
            defaultEnchants.Add(new Enchant(1886, "Superior Stamina", Item.ItemSlot.Wrist, new Stats() { Stamina = 9 }));
            defaultEnchants.Add(new Enchant(2648, "Major Defense", Item.ItemSlot.Wrist, new Stats() { DefenseRating = 12 }));
            defaultEnchants.Add(new Enchant(1891, "Stats", Item.ItemSlot.Wrist, new Stats() { Agility = 4, Strength = 4, Stamina = 4, Intellect = 4, Spirit = 4 }));
            defaultEnchants.Add(new Enchant(2661, "Greater Stats", Item.ItemSlot.Wrist, new Stats() { Agility = 6, Strength = 6, Stamina = 6, Intellect = 6, Spirit = 6 }));
            defaultEnchants.Add(new Enchant(2564, "Superior Agility", Item.ItemSlot.Hands, new Stats() { Agility = 15 }));
            defaultEnchants.Add(new Enchant(3222, "Major Agility", Item.ItemSlot.Hands, new Stats() { Agility = 20 }));
            defaultEnchants.Add(new Enchant(3011, "Clefthide Leg Armor", Item.ItemSlot.Legs, new Stats() { Agility = 10, Stamina = 30 }));
            defaultEnchants.Add(new Enchant(3013, "Nethercleft Leg Armor", Item.ItemSlot.Legs, new Stats() { Agility = 12, Stamina = 40 }));
            defaultEnchants.Add(new Enchant(3325, "Jormungar Leg Armor", Item.ItemSlot.Legs, new Stats() { Agility = 15, Stamina = 45 }));
            defaultEnchants.Add(new Enchant(3822, "Frosthide Leg Armor", Item.ItemSlot.Legs, new Stats() { Agility = 22, Stamina = 55 }));
            defaultEnchants.Add(new Enchant(2939, "Cat's Swiftness", Item.ItemSlot.Feet, new Stats() { Agility = 6, MovementSpeed = 8 }));
            defaultEnchants.Add(new Enchant(2940, "Boar's Speed", Item.ItemSlot.Feet, new Stats() { Stamina = 9, MovementSpeed = 8 }));
            defaultEnchants.Add(new Enchant(3232, "Tuskarr's Vitality", Item.ItemSlot.Feet, new Stats() { Stamina = 15, MovementSpeed = 8 }));
            defaultEnchants.Add(new Enchant(2657, "Dexterity", Item.ItemSlot.Feet, new Stats() { Agility = 12 }));
            defaultEnchants.Add(new Enchant(983, "Superior Agility", Item.ItemSlot.Feet, new Stats() { Agility = 16 }));
            defaultEnchants.Add(new Enchant(3824, "Assault", Item.ItemSlot.Feet, new Stats() { AttackPower = 24 }));
            defaultEnchants.Add(new Enchant(1597, "Greater Assault", Item.ItemSlot.Feet, new Stats() { AttackPower = 32 }));
            defaultEnchants.Add(new Enchant(2649, "Fortitude", Item.ItemSlot.Feet, new Stats() { Stamina = 12 }));
            defaultEnchants.Add(new Enchant(1075, "Greater Fortitude", Item.ItemSlot.Feet, new Stats() { Stamina = 22 }));
            defaultEnchants.Add(new Enchant(2931, "Stats", Item.ItemSlot.Finger, new Stats() { Agility = 4, Strength = 4, Stamina = 4, Intellect = 4, Spirit = 4 }));
            defaultEnchants.Add(new Enchant(3791, "Stamina", Item.ItemSlot.Finger, new Stats() { Stamina = 24 }));
            defaultEnchants.Add(new Enchant(2670, "Major Agility", Item.ItemSlot.TwoHand, new Stats() { Agility = 35 }));
            defaultEnchants.Add(new Enchant(2792, "Knothide Armor Kit", Item.ItemSlot.Chest, new Stats() { Stamina = 8 }));
            defaultEnchants.Add(new Enchant(2792, "Knothide Armor Kit", Item.ItemSlot.Legs, new Stats() { Stamina = 8 }));
            defaultEnchants.Add(new Enchant(2792, "Knothide Armor Kit", Item.ItemSlot.Hands, new Stats() { Stamina = 8 }));
            defaultEnchants.Add(new Enchant(2792, "Knothide Armor Kit", Item.ItemSlot.Feet, new Stats() { Stamina = 8 }));
            defaultEnchants.Add(new Enchant(3329, "Borean Armor Kit", Item.ItemSlot.Chest, new Stats() { Stamina = 12 }));
            defaultEnchants.Add(new Enchant(3329, "Borean Armor Kit", Item.ItemSlot.Legs, new Stats() { Stamina = 12 }));
            defaultEnchants.Add(new Enchant(3329, "Borean Armor Kit", Item.ItemSlot.Hands, new Stats() { Stamina = 12 }));
            defaultEnchants.Add(new Enchant(3329, "Borean Armor Kit", Item.ItemSlot.Feet, new Stats() { Stamina = 12 }));
            defaultEnchants.Add(new Enchant(3330, "Heavy Borean Armor Kit", Item.ItemSlot.Chest, new Stats() { Stamina = 18 }));
            defaultEnchants.Add(new Enchant(3330, "Heavy Borean Armor Kit", Item.ItemSlot.Legs, new Stats() { Stamina = 18 }));
            defaultEnchants.Add(new Enchant(3330, "Heavy Borean Armor Kit", Item.ItemSlot.Hands, new Stats() { Stamina = 18 }));
            defaultEnchants.Add(new Enchant(3330, "Heavy Borean Armor Kit", Item.ItemSlot.Feet, new Stats() { Stamina = 18 }));
            defaultEnchants.Add(new Enchant(3330, "Heavy Borean Armor Kit", Item.ItemSlot.Head, new Stats() { Stamina = 18 }));
            defaultEnchants.Add(new Enchant(3330, "Heavy Borean Armor Kit", Item.ItemSlot.Shoulders, new Stats() { Stamina = 18 }));
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
            defaultEnchants.Add(new Enchant(3260, "Glove Reinforcements", Item.ItemSlot.Hands, new Stats() { BonusArmor = 240 }));
            defaultEnchants.Add(new Enchant(1441, "Greater Shadow Resistance", Item.ItemSlot.Back, new Stats() { ShadowResistance = 15 }));

            defaultEnchants.Add(new Enchant(2543, "Arcanum of Rapidity", Item.ItemSlot.Head, new Stats() { HasteRating = 10 }));
            defaultEnchants.Add(new Enchant(3003, "Arcanum of Ferocity", Item.ItemSlot.Head, new Stats() { AttackPower = 34, HitRating = 16 }));
            defaultEnchants.Add(new Enchant(3817, "Arcanum of Torment", Item.ItemSlot.Head, new Stats() { AttackPower = 50, CritRating = 20 }));
            defaultEnchants.Add(new Enchant(3096, "Arcanum of the Outcast", Item.ItemSlot.Head, new Stats() { Strength = 17, Intellect = 16 }));
            defaultEnchants.Add(new Enchant(2716, "Fortitude of the Scourge", Item.ItemSlot.Shoulders, new Stats() { Stamina = 16, BonusArmor = 100 }));
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
            defaultEnchants.Add(new Enchant(3823, "Icescale Leg Armor", Item.ItemSlot.Legs, new Stats() { AttackPower = 75, CritRating = 22 }));
            defaultEnchants.Add(new Enchant(3326, "Nerubian Leg Armor", Item.ItemSlot.Legs, new Stats() { AttackPower = 55, CritRating = 15 }));
            defaultEnchants.Add(new Enchant(2658, "Surefooted", Item.ItemSlot.Feet, new Stats() { CritRating = 10, HitRating = 10 }));
            defaultEnchants.Add(new Enchant(3826, "Icewalker", Item.ItemSlot.Feet, new Stats() { CritRating = 12, HitRating = 12 }));
            defaultEnchants.Add(new Enchant(2929, "Striking", Item.ItemSlot.Finger, new Stats() { WeaponDamage = 2 }));
            defaultEnchants.Add(new Enchant(3839, "Assault", Item.ItemSlot.Finger, new Stats() { AttackPower = 32 }));
            defaultEnchants.Add(new Enchant(2667, "Savagery", Item.ItemSlot.TwoHand, new Stats() { AttackPower = 70 }));
            defaultEnchants.Add(new Enchant(3828, "Greater Savagery", Item.ItemSlot.TwoHand, new Stats() { AttackPower = 85 }));
            defaultEnchants.Add(new Enchant(3827, "Massacre", Item.ItemSlot.TwoHand, new Stats() { AttackPower = 110 }));
            defaultEnchants.Add(new Enchant(1593, "Assault", Item.ItemSlot.Wrist, new Stats() { AttackPower = 24 }));
            defaultEnchants.Add(new Enchant(1600, "Striking", Item.ItemSlot.Wrist, new Stats() { AttackPower = 38 }));
            defaultEnchants.Add(new Enchant(3845, "Greater Assault", Item.ItemSlot.Wrist, new Stats() { AttackPower = 50 }));
            defaultEnchants.Add(new Enchant(3231, "Expertise", Item.ItemSlot.Wrist, new Stats() { ExpertiseRating = 15 }));
            defaultEnchants.Add(new Enchant(3829, "Assault", Item.ItemSlot.Hands, new Stats() { AttackPower = 35 }));
            defaultEnchants.Add(new Enchant(1594, "Greater Assault", Item.ItemSlot.Hands, new Stats() { AttackPower = 26 }));
            defaultEnchants.Add(new Enchant(1603, "Crusher", Item.ItemSlot.Hands, new Stats() { AttackPower = 44 }));
            defaultEnchants.Add(new Enchant(3231, "Expertise", Item.ItemSlot.Hands, new Stats() { ExpertiseRating = 15 }));
            defaultEnchants.Add(new Enchant(3222, "Greater Agility", Item.ItemSlot.OneHand, new Stats() { Agility = 20 }));
            defaultEnchants.Add(new Enchant(1103, "Exceptional Agility", Item.ItemSlot.OneHand, new Stats() { Agility = 26 }));
            defaultEnchants.Add(new Enchant(2668, "Potency", Item.ItemSlot.OneHand, new Stats() { Strength = 20 }));
            defaultEnchants.Add(new Enchant(1606, "Greater Potency", Item.ItemSlot.OneHand, new Stats() { AttackPower = 50 }));
            defaultEnchants.Add(new Enchant(3833, "Superior Potency", Item.ItemSlot.OneHand, new Stats() { AttackPower = 65 }));
            defaultEnchants.Add(new Enchant(3731, "Titanium Weapon Chain", Item.ItemSlot.OneHand, new Stats() { HitRating = 28 }));
            defaultEnchants.Add(new Enchant(3223, "Adamantite Weapon Chain", Item.ItemSlot.OneHand, new Stats() { ParryRating = 15 }));
            defaultEnchants.Add(new Enchant(2722, "Adamantite Scope", Item.ItemSlot.Ranged, new Stats() { ScopeDamage = 10 }));
            defaultEnchants.Add(new Enchant(2723, "Khorium Scope", Item.ItemSlot.Ranged, new Stats() { ScopeDamage = 12 }));
            defaultEnchants.Add(new Enchant(3843, "Diamond-cut Refractor Scope", Item.ItemSlot.Ranged, new Stats() { ScopeDamage = 15 }));
            defaultEnchants.Add(new Enchant(3608, "Heartseeker Scope", Item.ItemSlot.Ranged, new Stats() { RangedCritRating = 40 }));
            defaultEnchants.Add(new Enchant(2724, "Stabilized Eternium Scope", Item.ItemSlot.Ranged, new Stats() { RangedCritRating = 28 }));
            defaultEnchants.Add(new Enchant(3607, "Sun Scope", Item.ItemSlot.Ranged, new Stats() { RangedHasteRating = 40 }));

            defaultEnchants.Add(new Enchant(2621, "Subtlety", Item.ItemSlot.Back, new Stats() { ThreatReductionMultiplier = 0.02f }));
            defaultEnchants.Add(new Enchant(3296, "Wisdom", Item.ItemSlot.Back, new Stats() { ThreatReductionMultiplier = 0.02f, Spirit = 10 }));
            defaultEnchants.Add(new Enchant(2613, "Threat", Item.ItemSlot.Hands, new Stats() { ThreatIncreaseMultiplier = 0.02f }));
            defaultEnchants.Add(new Enchant(3253, "Armsman", Item.ItemSlot.Hands, new Stats() { ThreatIncreaseMultiplier = 0.02f, ParryRating = 10 }));

            //spell stuff
            defaultEnchants.Add(new Enchant(2650, "Spellpower", Item.ItemSlot.Wrist, new Stats() { SpellPower = 15 }));
            defaultEnchants.Add(new Enchant(2332, "Superior Spellpower", Item.ItemSlot.Wrist, new Stats() { SpellPower = 30 }));
            defaultEnchants.Add(new Enchant(2326, "Greater Spellpower", Item.ItemSlot.Wrist, new Stats() { SpellPower = 23 }));
            defaultEnchants.Add(new Enchant(2937, "Major Spellpower", Item.ItemSlot.Hands, new Stats() { SpellPower = 20 }));
            defaultEnchants.Add(new Enchant(3246, "Exceptional Spellpower", Item.ItemSlot.Hands, new Stats() { SpellPower = 28 }));
            defaultEnchants.Add(new Enchant(2935, "Precise Strikes", Item.ItemSlot.Hands, new Stats() { HitRating = 15 }));
            defaultEnchants.Add(new Enchant(3234, "Precision", Item.ItemSlot.Hands, new Stats() { HitRating = 20 }));
            defaultEnchants.Add(new Enchant(2928, "Spellpower", Item.ItemSlot.Finger, new Stats() { SpellPower = 12 }));
            defaultEnchants.Add(new Enchant(3840, "Greater Spellpower", Item.ItemSlot.Finger, new Stats() { SpellPower = 19 }));
            defaultEnchants.Add(new Enchant(2669, "Major Spellpower", Item.ItemSlot.OneHand, new Stats() { SpellPower = 40 }));
            defaultEnchants.Add(new Enchant(3830, "Exceptional Spellpower", Item.ItemSlot.OneHand, new Stats() { SpellPower = 50 }));
            defaultEnchants.Add(new Enchant(3834, "Mighty Spellpower", Item.ItemSlot.OneHand, new Stats() { SpellPower = 63 }));
            defaultEnchants.Add(new Enchant(3788, "Accuracy", Item.ItemSlot.OneHand, new Stats() { CritRating = 25, HitRating = 25 }));
            defaultEnchants.Add(new Enchant(2666, "Major Intellect", Item.ItemSlot.OneHand, new Stats() { Intellect = 30 }));
            defaultEnchants.Add(new Enchant(3844, "Exceptional Spirit", Item.ItemSlot.OneHand, new Stats() { Spirit = 45 }));
            defaultEnchants.Add(new Enchant(2679, "Restore Mana Prime", Item.ItemSlot.Wrist, new Stats() { Mp5 = 6 }));
            defaultEnchants.Add(new Enchant(1147, "Major Spirit", Item.ItemSlot.Wrist, new Stats() { Spirit = 18 }));
            defaultEnchants.Add(new Enchant(2748, "Runic Spellthread", Item.ItemSlot.Legs, new Stats() { SpellPower = 35, Stamina = 20 }));
            defaultEnchants.Add(new Enchant(2747, "Mystic Spellthread", Item.ItemSlot.Legs, new Stats() { SpellPower = 25, Stamina = 15 }));
            defaultEnchants.Add(new Enchant(3719, "Brilliant Spellthread", Item.ItemSlot.Legs, new Stats() { SpellPower = 50, Spirit = 20 }));
            defaultEnchants.Add(new Enchant(3721, "Sapphire Spellthread", Item.ItemSlot.Legs, new Stats() { SpellPower = 50, Stamina = 30 }));
            defaultEnchants.Add(new Enchant(3718, "Shining Spellthread", Item.ItemSlot.Legs, new Stats() { SpellPower = 35, Spirit = 12 }));
            defaultEnchants.Add(new Enchant(3720, "Azure Spellthread", Item.ItemSlot.Legs, new Stats() { SpellPower = 35, Stamina = 20 }));
            defaultEnchants.Add(new Enchant(2982, "Greater Inscription of Discipline", Item.ItemSlot.Shoulders, new Stats() { SpellPower = 18, CritRating = 10 }));
            defaultEnchants.Add(new Enchant(2995, "Greater Inscription of the Orb", Item.ItemSlot.Shoulders, new Stats() { SpellPower = 12, CritRating = 15 }));
            defaultEnchants.Add(new Enchant(2981, "Inscription of Discipline", Item.ItemSlot.Shoulders, new Stats() { SpellPower = 15 }));
            defaultEnchants.Add(new Enchant(2994, "Inscription of the Orb", Item.ItemSlot.Shoulders, new Stats() { CritRating = 13 }));
            defaultEnchants.Add(new Enchant(3002, "Arcanum of Power", Item.ItemSlot.Head, new Stats() { SpellPower = 22, HitRating = 14 }));
            defaultEnchants.Add(new Enchant(3820, "Arcanum of Burning Mysteries", Item.ItemSlot.Head, new Stats() { SpellPower = 30, CritRating = 20 }));
            defaultEnchants.Add(new Enchant(3797, "Arcanum of Dominance", Item.ItemSlot.Head, new Stats() { SpellPower = 29, Resilience = 20 }));
            defaultEnchants.Add(new Enchant(2671, "Sunfire", Item.ItemSlot.OneHand, new Stats() { SpellFireDamageRating = 50, SpellArcaneDamageRating = 50 }));
            defaultEnchants.Add(new Enchant(2672, "Soulfrost", Item.ItemSlot.OneHand, new Stats() { SpellFrostDamageRating = 54, SpellShadowDamageRating = 54 }));
            defaultEnchants.Add(new Enchant(2938, "Spell Penetration", Item.ItemSlot.Back, new Stats() { SpellPenetration = 20 }));
            defaultEnchants.Add(new Enchant(3243, "Spell Piercing", Item.ItemSlot.Back, new Stats() { SpellPenetration = 35 }));
            defaultEnchants.Add(new Enchant(3244, "Greater Vitality", Item.ItemSlot.Feet, new Stats() { Mp5 = 6, Hp5 = 6 }));
            defaultEnchants.Add(new Enchant(2656, "Vitality", Item.ItemSlot.Feet, new Stats() { Mp5 = 4, Hp5 = 4 }));
            defaultEnchants.Add(new Enchant(369, "Major Intellect", Item.ItemSlot.Wrist, new Stats() { Intellect = 12 }));
            defaultEnchants.Add(new Enchant(1119, "Exceptional Intellect", Item.ItemSlot.Wrist, new Stats() { Intellect = 16 }));
            defaultEnchants.Add(new Enchant(1144, "Major Spirit", Item.ItemSlot.Chest, new Stats() { Spirit = 15 }));
            defaultEnchants.Add(new Enchant(851, "Spirit", Item.ItemSlot.Feet, new Stats() { Spirit = 5 }));
            defaultEnchants.Add(new Enchant(1147, "Greater Spirit", Item.ItemSlot.Feet, new Stats() { Spirit = 18 }));

            // Healing enchants (add spell damage too)
            defaultEnchants.Add(new Enchant(3001, "Arcanum of Renewal", Item.ItemSlot.Head, new Stats() { SpellPower = 19, Mp5 = 7 }));
            defaultEnchants.Add(new Enchant(3819, "Arcanum of Blissful Mending", Item.ItemSlot.Head, new Stats() { SpellPower = 30, Mp5 = 8 }));

            defaultEnchants.Add(new Enchant(3150, "Restore Mana Prime", Item.ItemSlot.Chest, new Stats() { Mp5 = 6 }));
            defaultEnchants.Add(new Enchant(2381, "Greater Mana Restoration", Item.ItemSlot.Chest, new Stats() { Mp5 = 8 }));
            defaultEnchants.Add(new Enchant(3233, "Exceptional Mana", Item.ItemSlot.Chest, new Stats() { Mana = 250 }));

            defaultEnchants.Add(new Enchant(2980, "Greater Inscription of Faith", Item.ItemSlot.Shoulders, new Stats() { SpellPower = 18, Mp5 = 4 }));
            defaultEnchants.Add(new Enchant(2993, "Greater Inscription of the Oracle", Item.ItemSlot.Shoulders, new Stats() { SpellPower = 12, Mp5 = 6 }));
            defaultEnchants.Add(new Enchant(2979, "Inscription of Faith", Item.ItemSlot.Shoulders, new Stats() { SpellPower = 15 }));
            defaultEnchants.Add(new Enchant(2992, "Inscription of the Oracle", Item.ItemSlot.Shoulders, new Stats() { Mp5 = 5 }));
            defaultEnchants.Add(new Enchant(2604, "Zandalar Signet of Serenity", Item.ItemSlot.Shoulders, new Stats() { SpellPower = 18 }));

            defaultEnchants.Add(new Enchant(2322, "Major Healing", Item.ItemSlot.Hands, new Stats() { SpellPower = 19 }));
            defaultEnchants.Add(new Enchant(2934, "Blasting", Item.ItemSlot.Hands, new Stats() { CritRating = 10 }));
            defaultEnchants.Add(new Enchant(3249, "Greater Blasting", Item.ItemSlot.Hands, new Stats() { CritRating = 16 }));

            defaultEnchants.Add(new Enchant(2746, "Golden Spellthread", Item.ItemSlot.Legs, new Stats() { SpellPower = 35, Stamina = 20 }));
            defaultEnchants.Add(new Enchant(2745, "Silver Spellthread", Item.ItemSlot.Legs, new Stats() { SpellPower = 25, Stamina = 15 }));

            defaultEnchants.Add(new Enchant(2794, "Magister's Armor Kit", Item.ItemSlot.Chest, new Stats() { Mp5 = 3 }));
            defaultEnchants.Add(new Enchant(2794, "Magister's Armor Kit", Item.ItemSlot.Feet, new Stats() { Mp5 = 3 }));
            defaultEnchants.Add(new Enchant(2794, "Magister's Armor Kit", Item.ItemSlot.Legs, new Stats() { Mp5 = 3 }));
            defaultEnchants.Add(new Enchant(2794, "Magister's Armor Kit", Item.ItemSlot.Hands, new Stats() { Mp5 = 3 }));

            defaultEnchants.Add(new Enchant(2930, "Healing Power", Item.ItemSlot.Finger, new Stats() { SpellPower = 12 }));

            defaultEnchants.Add(new Enchant(2343, "Major Healing", Item.ItemSlot.OneHand, new Stats() { SpellPower = 40 }));

            defaultEnchants.Add(new Enchant(2654, "Intellect", Item.ItemSlot.OffHand, new Stats() { Intellect = 12 }));
            defaultEnchants.Add(new Enchant(1128, "Greater Intellect", Item.ItemSlot.OffHand, new Stats() { Intellect = 25 }));

            defaultEnchants.Add(new Enchant(2617, "Superior Healing", Item.ItemSlot.Wrist, new Stats() { SpellPower = 15 }));

            defaultEnchants.Add(new Enchant(2648, "Steelweave", Item.ItemSlot.Back, new Stats() { DefenseRating = 12 }));
            defaultEnchants.Add(new Enchant(1951, "Titanweave", Item.ItemSlot.Back, new Stats() { DefenseRating = 16 }));
            defaultEnchants.Add(new Enchant(3004, "Arcanum of the Gladiator", Item.ItemSlot.Head, new Stats() { Stamina = 18, Resilience = 20 }));
            defaultEnchants.Add(new Enchant(3842, "Arcanum of the Savage Gladiator", Item.ItemSlot.Head, new Stats() { Stamina = 30, Resilience = 25 }));
            defaultEnchants.Add(new Enchant(3795, "Arcanum of Triumph", Item.ItemSlot.Head, new Stats() { AttackPower = 50, Resilience = 20 }));

            defaultEnchants.Add(new Enchant(3793, "Inscription of Triumph", Item.ItemSlot.Shoulders, new Stats() { AttackPower = 40, Resilience = 15 }));
            defaultEnchants.Add(new Enchant(3794, "Inscription of Dominance", Item.ItemSlot.Shoulders, new Stats() { SpellPower = 23, Resilience = 15 }));

            //The stat value of mongoose and executioner is dependent on the weapon speed and is thus left to the individual models to take care of through the Id
            defaultEnchants.Add(new Enchant(2673, "Mongoose", Item.ItemSlot.OneHand, new Stats() { MongooseProc = 1 }));

            Stats berserking = new Stats() { BerserkingProc = 1 };
            berserking.AddSpecialEffect(new SpecialEffect(Trigger.MeleeHit, new Stats() { AttackPower = 400f, BonusArmorMultiplier = -.05f }, 15f, 0f, -1f));
            defaultEnchants.Add(new Enchant(3789, "Berserking", Item.ItemSlot.OneHand, berserking));
            
            defaultEnchants.Add(new Enchant(3225, "Executioner", Item.ItemSlot.OneHand, new Stats() { ExecutionerProc = 1 }));
            defaultEnchants.Add(new Enchant(2984, "Shadow Armor Kit", Item.ItemSlot.Hands, new Stats() { ShadowResistance = 8 }));
            defaultEnchants.Add(new Enchant(2984, "Shadow Armor Kit", Item.ItemSlot.Feet, new Stats() { ShadowResistance = 8 }));
            defaultEnchants.Add(new Enchant(2984, "Shadow Armor Kit", Item.ItemSlot.Chest, new Stats() { ShadowResistance = 8 }));
            defaultEnchants.Add(new Enchant(2984, "Shadow Armor Kit", Item.ItemSlot.Legs, new Stats() { ShadowResistance = 8 }));
            defaultEnchants.Add(new Enchant(2985, "Flame Armor Kit", Item.ItemSlot.Hands, new Stats() { FireResistance = 8 }));
            defaultEnchants.Add(new Enchant(2985, "Flame Armor Kit", Item.ItemSlot.Feet, new Stats() { FireResistance = 8 }));
            defaultEnchants.Add(new Enchant(2985, "Flame Armor Kit", Item.ItemSlot.Chest, new Stats() { FireResistance = 8 }));
            defaultEnchants.Add(new Enchant(2985, "Flame Armor Kit", Item.ItemSlot.Legs, new Stats() { FireResistance = 8 }));
            defaultEnchants.Add(new Enchant(2987, "Frost Armor Kit", Item.ItemSlot.Hands, new Stats() { FrostResistance = 8 }));
            defaultEnchants.Add(new Enchant(2987, "Frost Armor Kit", Item.ItemSlot.Feet, new Stats() { FrostResistance = 8 }));
            defaultEnchants.Add(new Enchant(2987, "Frost Armor Kit", Item.ItemSlot.Chest, new Stats() { FrostResistance = 8 }));
            defaultEnchants.Add(new Enchant(2987, "Frost Armor Kit", Item.ItemSlot.Legs, new Stats() { FrostResistance = 8 }));
            defaultEnchants.Add(new Enchant(2988, "Nature Armor Kit", Item.ItemSlot.Hands, new Stats() { NatureResistance = 8 }));
            defaultEnchants.Add(new Enchant(2988, "Nature Armor Kit", Item.ItemSlot.Feet, new Stats() { NatureResistance = 8 }));
            defaultEnchants.Add(new Enchant(2988, "Nature Armor Kit", Item.ItemSlot.Chest, new Stats() { NatureResistance = 8 }));
            defaultEnchants.Add(new Enchant(2988, "Nature Armor Kit", Item.ItemSlot.Legs, new Stats() { NatureResistance = 8 }));
            defaultEnchants.Add(new Enchant(2989, "Arcane Armor Kit", Item.ItemSlot.Hands, new Stats() { ArcaneResistance = 8 }));
            defaultEnchants.Add(new Enchant(2989, "Arcane Armor Kit", Item.ItemSlot.Feet, new Stats() { ArcaneResistance = 8 }));
            defaultEnchants.Add(new Enchant(2989, "Arcane Armor Kit", Item.ItemSlot.Chest, new Stats() { ArcaneResistance = 8 }));
            defaultEnchants.Add(new Enchant(2989, "Arcane Armor Kit", Item.ItemSlot.Legs, new Stats() { ArcaneResistance = 8 }));
            defaultEnchants.Add(new Enchant(3005, "Arcanum of Nature Warding", Item.ItemSlot.Head, new Stats() { NatureResistance = 20 }));
            defaultEnchants.Add(new Enchant(3813, "Arcanum of Toxic Warding", Item.ItemSlot.Head, new Stats() { NatureResistance = 25, Stamina = 30 }));
            defaultEnchants.Add(new Enchant(3006, "Arcanum of Arcane Warding", Item.ItemSlot.Head, new Stats() { ArcaneResistance = 20 }));
            defaultEnchants.Add(new Enchant(3815, "Arcanum of the Eclipsed Moon", Item.ItemSlot.Head, new Stats() { ArcaneResistance = 25, Stamina = 30 }));
            defaultEnchants.Add(new Enchant(3007, "Arcanum of Fire Warding", Item.ItemSlot.Head, new Stats() { FireResistance = 20 }));
            defaultEnchants.Add(new Enchant(3816, "Arcanum of the Flame's Soul", Item.ItemSlot.Head, new Stats() { FireResistance = 25, Stamina = 30 }));
            defaultEnchants.Add(new Enchant(3008, "Arcanum of Frost Warding", Item.ItemSlot.Head, new Stats() { FrostResistance = 20 }));
            defaultEnchants.Add(new Enchant(3812, "Arcanum of the Frosty Soul", Item.ItemSlot.Head, new Stats() { FrostResistance = 25, Stamina = 30 }));
            defaultEnchants.Add(new Enchant(3009, "Arcanum of Shadow Warding", Item.ItemSlot.Head, new Stats() { ShadowResistance = 20 }));
            defaultEnchants.Add(new Enchant(3814, "Arcanum of the Fleeing Shadow", Item.ItemSlot.Head, new Stats() { ShadowResistance = 25, Stamina = 30 }));
            defaultEnchants.Add(new Enchant(2998, "Inscription of Endurance", Item.ItemSlot.Shoulders, new Stats() { NatureResistance = 4, ArcaneResistance = 4, FireResistance = 4, FrostResistance = 4, ShadowResistance = 4 }));
            defaultEnchants.Add(new Enchant(2664, "Major Resistance", Item.ItemSlot.Back, new Stats() { NatureResistance = 7, ArcaneResistance = 7, FireResistance = 7, FrostResistance = 7, ShadowResistance = 7 }));
            defaultEnchants.Add(new Enchant(2619, "Greater Fire Resistance", Item.ItemSlot.Back, new Stats() { FireResistance = 15 }));
            defaultEnchants.Add(new Enchant(1262, "Superior Arcane Resistance", Item.ItemSlot.Back, new Stats() { ArcaneResistance = 20 }));
            defaultEnchants.Add(new Enchant(1446, "Superior Shadow Resistance", Item.ItemSlot.Back, new Stats() { ShadowResistance = 20 }));
            defaultEnchants.Add(new Enchant(1354, "Superior Fire Resistance", Item.ItemSlot.Back, new Stats() { FireResistance = 20 }));
            defaultEnchants.Add(new Enchant(3230, "Superior Frost Resistance", Item.ItemSlot.Back, new Stats() { FrostResistance = 20 }));
            defaultEnchants.Add(new Enchant(1400, "Superior Nature Resistance", Item.ItemSlot.Back, new Stats() { NatureResistance = 20 }));
            defaultEnchants.Add(new Enchant(1505, "Lesser Arcanum of Resilience", Item.ItemSlot.Head, new Stats() { FireResistance = 20 }));
            defaultEnchants.Add(new Enchant(1505, "Lesser Arcanum of Resilience", Item.ItemSlot.Legs, new Stats() { FireResistance = 20 }));

            defaultEnchants.Add(new Enchant(1071, "Major Stamina", Item.ItemSlot.OffHand, new Stats() { Stamina = 18 }));
            defaultEnchants.Add(new Enchant(2655, "Shield Block", Item.ItemSlot.OffHand, new Stats() { BlockRating = 15 }));
            defaultEnchants.Add(new Enchant(2653, "Tough Shield", Item.ItemSlot.OffHand, new Stats() { BlockValue = 18 }));
            defaultEnchants.Add(new Enchant(1952, "Defense", Item.ItemSlot.OffHand, new Stats() { DefenseRating = 20 }));

            //scopes
            defaultEnchants.Add(new Enchant(2723, "Khorium Scope", Item.ItemSlot.Ranged, new Stats() { ScopeDamage = 12 }));
            defaultEnchants.Add(new Enchant(2722, "Adamantite Scope", Item.ItemSlot.Ranged, new Stats() { ScopeDamage = 10 }));
            defaultEnchants.Add(new Enchant(2523, "Biznicks 247x128 Accurascope", Item.ItemSlot.Ranged, new Stats() { RangedHitRating = 30 }));
            defaultEnchants.Add(new Enchant(2724, "Stabilized Eternium Scope", Item.ItemSlot.Ranged, new Stats() { RangedCritRating = 28 }));

            //Sapphiron Enchants
            defaultEnchants.Add(new Enchant(2721, "Power of the Scourge", Item.ItemSlot.Shoulders, new Stats() { CritRating = 14, SpellPower = 15 }));
            defaultEnchants.Add(new Enchant(2716, "Fortitude of the Scourge", Item.ItemSlot.Shoulders, new Stats() { Stamina = 16, BonusArmor = 100 }));
            defaultEnchants.Add(new Enchant(2715, "Resilience of the Scourge", Item.ItemSlot.Shoulders, new Stats() { SpellPower = 16, Mp5 = 5 }));
            defaultEnchants.Add(new Enchant(2717, "Might of the Scourge", Item.ItemSlot.Shoulders, new Stats() { CritRating = 14, AttackPower = 26 }));

            //Hodir Enchants
            defaultEnchants.Add(new Enchant(3808, "Greater Inscription of the Axe", Item.ItemSlot.Shoulders, new Stats() { AttackPower = 40, CritRating = 15 }));
            defaultEnchants.Add(new Enchant(2986, "Lesser Inscription of the Axe", Item.ItemSlot.Shoulders, new Stats() { AttackPower = 30, CritRating = 10 }));
            defaultEnchants.Add(new Enchant(3809, "Greater Inscription of the Crag", Item.ItemSlot.Shoulders, new Stats() { SpellPower = 24, Mp5 = 6 }));
            defaultEnchants.Add(new Enchant(3807, "Lesser Inscription of the Crag", Item.ItemSlot.Shoulders, new Stats() { SpellPower = 18, Mp5 = 4 }));
            defaultEnchants.Add(new Enchant(3811, "Greater Inscription of the Pinnacle", Item.ItemSlot.Shoulders, new Stats() { DodgeRating = 20, DefenseRating = 15 }));
            defaultEnchants.Add(new Enchant(2978, "Lesser Inscription of the Pinnacle", Item.ItemSlot.Shoulders, new Stats() { DodgeRating = 15, DefenseRating = 10 }));
            defaultEnchants.Add(new Enchant(3810, "Greater Inscription of the Storm", Item.ItemSlot.Shoulders, new Stats() { SpellPower = 24, CritRating = 15 }));
            defaultEnchants.Add(new Enchant(3806, "Lesser Inscription of the Storm", Item.ItemSlot.Shoulders, new Stats() { SpellPower = 18, CritRating = 10 }));

            //Inscriber enchants
            defaultEnchants.Add(new Enchant(3835, "Master's Inscription of the Axe", Item.ItemSlot.Shoulders, new Stats() { CritRating = 15, AttackPower = 104 }));
            defaultEnchants.Add(new Enchant(3836, "Master's Inscription of the Crag", Item.ItemSlot.Shoulders, new Stats() { Mp5 = 6, SpellPower = 61 }));
            defaultEnchants.Add(new Enchant(3837, "Master's Inscription of the Pinnacle", Item.ItemSlot.Shoulders, new Stats() { DodgeRating = 52, DefenseRating = 15 }));
            defaultEnchants.Add(new Enchant(3838, "Master's Inscription of the Storm", Item.ItemSlot.Shoulders, new Stats() { CritRating = 15, SpellPower = 61 }));

            //Leatherworking enchants
            defaultEnchants.Add(new Enchant(3756, "Fur Lining - Attack Power", Item.ItemSlot.Wrist, new Stats() { AttackPower = 114 }));
            defaultEnchants.Add(new Enchant(3757, "Fur Lining - Stamina", Item.ItemSlot.Wrist, new Stats() { Stamina = 90 }));
            defaultEnchants.Add(new Enchant(3758, "Fur Lining - Spell Power", Item.ItemSlot.Wrist, new Stats() { SpellPower = 67 }));
            defaultEnchants.Add(new Enchant(3759, "Fur Lining - Fire Resist", Item.ItemSlot.Wrist, new Stats() { FireResistance = 60 }));
            defaultEnchants.Add(new Enchant(3760, "Fur Lining - Frost Resist", Item.ItemSlot.Wrist, new Stats() { FrostResistance = 60 }));
            defaultEnchants.Add(new Enchant(3761, "Fur Lining - Shadow Resist", Item.ItemSlot.Wrist, new Stats() { ShadowResistance = 60 }));
            defaultEnchants.Add(new Enchant(3762, "Fur Lining - Nature Resist", Item.ItemSlot.Wrist, new Stats() { NatureResistance = 60 }));
            defaultEnchants.Add(new Enchant(3763, "Fur Lining - Arcane Resist", Item.ItemSlot.Wrist, new Stats() { ArcaneResistance = 60 }));
            defaultEnchants.Add(new Enchant(3327, "Jormungar Leg Reinforcements", Item.ItemSlot.Legs, new Stats() { Stamina = 55, Agility = 22 }));
            defaultEnchants.Add(new Enchant(3328, "Nerubian Leg Reinforcements", Item.ItemSlot.Legs, new Stats() { AttackPower = 75, CritRating = 22 }));


            //Death Knight Rune Enchants
            defaultEnchants.Add(new Enchant(3368, "Rune of the Fallen Crusader", Item.ItemSlot.OneHand, new Stats() { BonusStrengthMultiplier = .075f }));
            defaultEnchants.Add(new Enchant(3365, "Rune of Swordshattering", Item.ItemSlot.TwoHand, new Stats() { Parry = 0.04f }));
            defaultEnchants.Add(new Enchant(3594, "Rune of Swordbreaking", Item.ItemSlot.OneHand, new Stats() { Parry = 0.02f }));
            defaultEnchants.Add(new Enchant(3847, "Rune of the Stoneskin Gargoyle", Item.ItemSlot.TwoHand, new Stats() { Defense = 25.0f, BonusStaminaMultiplier = 0.02f }));

            // Engineering enchant
            Stats hyper = new Stats();
            hyper.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { HasteRating = 340f }, 10f, 60f));
            defaultEnchants.Add(new Enchant(3604, "Hyperspeed Accelerators", Item.ItemSlot.Hands, hyper));

            // Tailoring enchant
            Stats stats = new Stats();
            stats.AddSpecialEffect(new SpecialEffect(Trigger.SpellCast, new Stats() { SpellPower = 250 }, 15, 45, 0.5f));
            defaultEnchants.Add(new Enchant(3722, "Lightweave Embroidery", Item.ItemSlot.Back, stats));

            Stats darkglow = new Stats();
            darkglow.AddSpecialEffect(new SpecialEffect(Trigger.SpellCast, new Stats() { ManaRestore = 300 }, 1f, 60f, 0.35f));
            defaultEnchants.Add(new Enchant(3728, "Darkglow Embroidery", Item.ItemSlot.Back, darkglow));

            //3.0.8 enchants
            defaultEnchants.Add(new Enchant(3850, "Major Stamina", Item.ItemSlot.Wrist, new Stats() { Stamina = 40 }));
            defaultEnchants.Add(new Enchant(3849, "Titanium Plating", Item.ItemSlot.OffHand, new Stats() { BlockValue = 40 }));
            defaultEnchants.Add(new Enchant(3852, "Greater Inscription of the Gladiator", Item.ItemSlot.Shoulders, new Stats() { Stamina = 30, Resilience = 15 }));

			//3.1 enchants
			defaultEnchants.Add(new Enchant(3854, "Greater Spellpower (Staff)", Item.ItemSlot.TwoHand, new Stats() { SpellPower = 81 }));
			defaultEnchants.Add(new Enchant(3855, "Spellpower (Staff)", Item.ItemSlot.TwoHand, new Stats() { SpellPower = 69 }));

            Stats rockets = new Stats();
            rockets.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { FireDamage = 1600f }, 1f, 45f));
            defaultEnchants.Add(new Enchant(3603, "Hand-Mounted Pyro Rocket", Item.ItemSlot.Hands, rockets));
            
            #region Enchants to Delete
            defaultEnchants.Add(new Enchant(2673, "Mongoose", Item.ItemSlot.MainHand, null));
            defaultEnchants.Add(new Enchant(3225, "Executioner", Item.ItemSlot.MainHand, null));
            defaultEnchants.Add(new Enchant(2343, "Major Healing", Item.ItemSlot.MainHand, null));
            defaultEnchants.Add(new Enchant(1071, "Major Stamina", Item.ItemSlot.MainHand, null));
            defaultEnchants.Add(new Enchant(2655, "Shield Block", Item.ItemSlot.MainHand, null));
            defaultEnchants.Add(new Enchant(2653, "Tough Shield", Item.ItemSlot.MainHand, null));
            defaultEnchants.Add(new Enchant(2654, "Intellect", Item.ItemSlot.MainHand, null));
            defaultEnchants.Add(new Enchant(2669, "Major Spellpower", Item.ItemSlot.MainHand, null));
            defaultEnchants.Add(new Enchant(2666, "Major Intellect", Item.ItemSlot.MainHand, null));
            defaultEnchants.Add(new Enchant(2671, "Sunfire", Item.ItemSlot.MainHand, null));
            defaultEnchants.Add(new Enchant(2672, "Soulfrost", Item.ItemSlot.MainHand, null));
            defaultEnchants.Add(new Enchant(2667, "Savagery", Item.ItemSlot.MainHand, null));
            defaultEnchants.Add(new Enchant(2670, "Major Agility", Item.ItemSlot.MainHand, null));
            defaultEnchants.Add(new Enchant(3222, "Greater Agility", Item.ItemSlot.MainHand, null));
            defaultEnchants.Add(new Enchant(9001, "Major Defense", Item.ItemSlot.Chest, null));
            defaultEnchants.Add(new Enchant(2660, "Exceptional Mana", Item.ItemSlot.Chest, null));
            #endregion
            return defaultEnchants;
        }
    }
}
