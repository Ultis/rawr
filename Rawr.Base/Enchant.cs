using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace Rawr
{
    [GenerateSerializer]
    public class EnchantList : List<Enchant> { }

    /// <summary>An object representing an Enchantment to be placed on a slot on a character.</summary>
    public class Enchant
    {
        #region Variables
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
        /// <summary>The name of the enchant.</summary>
        public string Name;
        /// <summary>A shortened version of the enchant's Name.</summary>
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
                    .Replace("Mana Restoration", "Mp5").Replace("Restore Mana", "Mp5").Replace("Vengeance", "Veng.")
                    .Replace("Reticulated Armor ", "").Replace("Titanium Weapon Chain", "Titnm.W.Chn");
                return shortName.Substring(0, Math.Min(shortName.Length, 12));
            }
        }
        /// <summary>A REALLY shortened version of the enchant's Name. For those names that are still too long to fit in the space on screen</summary>
        public string ReallyShortName
        {
            get
            {
                if (Id == 0) return "None";
                string[] split = Name.Split(' ');
                if (split.Length > 1) 
                {
                    string ret = "";
                    foreach (string s in split) { ret += s.Substring(0, 1); }
                    return ret;
                }
                return Name.Substring(0, 5);
            }
        }
        /// <summary>
        /// The slot that the enchant is applied to. If the enchant is available on multiple slots,
        /// define the enchant multiple times, once for each slot.
        /// <para>IMPORTANT: Shield enchants should be defined as Off-Hand</para>
        /// </summary>
        public ItemSlot Slot = ItemSlot.Head;
        /// <summary>The stats that the enchant gives the character.</summary>
        public Stats Stats = new Stats();
        private static EnchantList _allEnchants;
        /// <summary>A List<Enchant> containing all known enchants relevant to all models.</summary>
        public static List<Enchant> AllEnchants { get { return _allEnchants; } }
        /// <summary>If set, will attempt to pull this icon from wow.com or wowhead.com for the comparison list</summary>
        public string IconSource { get; set; }
        #endregion

        public Enchant() { }
        /// <summary>Creates a new Enchant, representing an enchant to a single slot.</summary>
        /// <example>new Enchant(2564, "Superior Agility", ItemSlot.Hands, new Stats() { Agility = 15 })</example>
        /// <param name="id">The Enchant ID for the enchant. See the Id property for details of how to find this.</param>
        /// <param name="name">The name of the enchant.</param>
        /// <param name="slot">The slot that this instance of the enchant applies to. (Create multiple Enchant
        /// objects for enchants which may be applied to multiple slots)</param>
        /// <param name="icon">The Icon name (eg- "spell_fire_masterofelements"). Defaults to null for no Icon</param>
        /// <param name="stats">The stats that the enchant gives the character.</param>
        public Enchant(int id, string name, ItemSlot slot, Stats stats, string icon=null)
        {
            Id = id;
            Name = name;
            Slot = slot;
            Stats = stats;
            IconSource = icon;
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
            if (temp != null) {
                return Name.Equals(temp.Name) && Id == temp.Id && Slot == temp.Slot && Stats.Equals(temp.Stats);
            } else { return false; }
        }

        public override int GetHashCode() { return (Id << 5) | (int)Slot; }

        public bool FitsInSlot(ItemSlot slot)
        {
            return (Slot == slot ||
                (Slot == ItemSlot.OneHand && (slot == ItemSlot.OffHand || slot == ItemSlot.MainHand || slot == ItemSlot.TwoHand)) ||
                (Slot == ItemSlot.TwoHand && (slot == ItemSlot.MainHand)));
        }

        public bool FitsInSlot(ItemSlot slot, Character character) { return Calculations.EnchantFitsInSlot(this, character, slot); }

        public static Enchant FindEnchant(int id, ItemSlot slot, Character character)
        {
            return AllEnchants.Find(new Predicate<Enchant>(delegate(Enchant enchant) {
                return (enchant.Id == id) && (enchant.FitsInSlot(slot, character) ||
                  (enchant.Slot == ItemSlot.TwoHand && slot == ItemSlot.OneHand));
            })) ?? AllEnchants[0];
        }

        public static List<Enchant> FindEnchants(ItemSlot slot, Character character)
        {
            return FindEnchants(slot, character, Calculations.Instance);
        }

        public static List<Enchant> FindEnchants(ItemSlot slot, Character character, CalculationsBase model)
        {
            //List<ItemSlot> validSlots = new List<ItemSlot>();
            //if (slot != ItemSlot.MainHand)
            //    validSlots.Add(slot);
            //if (slot == ItemSlot.OffHand || slot == ItemSlot.MainHand || slot == ItemSlot.TwoHand)
            //    validSlots.Add(ItemSlot.OneHand);
            //if (slot == ItemSlot.MainHand)
            //    validSlots.Add(ItemSlot.TwoHand);
            return AllEnchants.FindAll(new Predicate<Enchant>(
                delegate(Enchant enchant)
                {
                    return enchant.Slot == ItemSlot.None ||
                        model.IsEnchantRelevant(enchant, character) &&
                        (slot == ItemSlot.None || enchant.FitsInSlot(slot, character));
                }
            ));
        }

        public static List<Enchant> FindAllEnchants(ItemSlot slot, Character character)
        {
            //List<ItemSlot> validSlots = new List<ItemSlot>();
            //if (slot != ItemSlot.MainHand)
            //    validSlots.Add(slot);
            //if (slot == ItemSlot.OffHand || slot == ItemSlot.MainHand || slot == ItemSlot.TwoHand)
            //    validSlots.Add(ItemSlot.OneHand);
            //if (slot == ItemSlot.MainHand)
            //    validSlots.Add(ItemSlot.TwoHand);
            return AllEnchants.FindAll(new Predicate<Enchant>(
                delegate(Enchant enchant)
                {
                    return enchant.Slot == ItemSlot.None ||
                        (slot == ItemSlot.None || enchant.FitsInSlot(slot, character));
                }
            ));
        }

        public static List<Enchant> FindEnchants(ItemSlot slot, Character character, List<string> availableIds)
        {
            return FindEnchants(slot, character, availableIds, Calculations.Instance);
        }

        public static List<Enchant> FindEnchants(ItemSlot slot, Character character, List<string> availableIds, CalculationsBase model)
        {
            return AllEnchants.FindAll(new Predicate<Enchant>(
                delegate(Enchant enchant)
                {
                    return enchant.Id == 0 || 
                        ((enchant.Slot == ItemSlot.None || 
                        model.IsEnchantRelevant(enchant, character) && (slot == ItemSlot.None || enchant.FitsInSlot(slot, character)))
                        && availableIds.Contains((-1 * (enchant.Id + (10000 * (int)enchant.Slot))).ToString()));
                }
            ));
        }

        public static List<Enchant> FindEnchants(ItemSlot slot, Character[] characters, List<string> availableIds, CalculationsBase[] models)
        {
            return AllEnchants.FindAll(new Predicate<Enchant>(
                delegate(Enchant enchant)
                {
                    bool isRelevant = false;
                    for (int i = 0; i < models.Length; i++)
                    {
                        if (models[i].IsEnchantRelevant(enchant, characters[i]) && (enchant.FitsInSlot(slot, characters[i]) || slot == ItemSlot.None))
                        {
                            isRelevant = true;
                            break;
                        }
                    }
                    return ((isRelevant || enchant.Slot == ItemSlot.None)
                        && availableIds.Contains((-1 * (enchant.Id + (10000 * (int)enchant.Slot))).ToString()))
                        || enchant.Id == 0;
                }
            ));
        }

        public static void Save(TextWriter writer)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(EnchantList));
            serializer.Serialize(writer, _allEnchants);
            writer.Close();
        }

        public static void Load(TextReader reader)
        {
            _allEnchants = null;
            try {
                XmlSerializer serializer = new XmlSerializer(typeof(EnchantList));
                _allEnchants = (EnchantList)serializer.Deserialize(reader);
                reader.Close();
            } catch {
            } finally {
                reader.Close();
                _allEnchants = _allEnchants ?? new EnchantList();
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
                            } else {
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
            Stats enchantTemp = new Stats();
            // The All Important No Enchant, works in all slots
            defaultEnchants.Add(new Enchant(0, "No Enchant", ItemSlot.None, new Stats(), ""));
            //defaultEnchants.Add(new Enchant(3878, "Mind Amplification Dish", ItemSlot.Waist, new Stats() { Stamina = 45f })); // Uhhh wut?
            #region Head
            #region Level 85 (Cataclysm)
            defaultEnchants.Add(new Enchant(4207, "Arcanum of Hyjal", ItemSlot.Head, new Stats() { Intellect = 60, CritRating = 35 }, "spell_fire_masterofelements"));
            defaultEnchants.Add(new Enchant(4208, "Arcanum of the Dragonmaw", ItemSlot.Head, new Stats() { Strength = 60, MasteryRating = 35 }, "spell_fire_masterofelements"));
            defaultEnchants.Add(new Enchant(4206, "Arcanum of the Earthen Ring", ItemSlot.Head, new Stats() { Stamina = 90, DodgeRating = 35 }, "spell_fire_masterofelements"));
            defaultEnchants.Add(new Enchant(4209, "Arcanum of the Ramkahen", ItemSlot.Head, new Stats() { Agility = 60, HasteRating = 35 }, "spell_fire_masterofelements"));
            defaultEnchants.Add(new Enchant(4208, "Arcanum of the Wildhammer", ItemSlot.Head, new Stats() { Strength = 60, MasteryRating = 35 }, "spell_fire_masterofelements"));
            #endregion
            #region Level 80 (WotLK)
            defaultEnchants.Add(new Enchant(3819, "Arcanum of Blissful Mending", ItemSlot.Head, new Stats() { Intellect = 26, Spirit = 20 }, "ability_warrior_shieldmastery"));
            defaultEnchants.Add(new Enchant(3820, "Arcanum of Burning Mysteries", ItemSlot.Head, new Stats() { SpellPower = 30, CritRating = 20 }, "spell_fire_masterofelements")); // Will probably be vamped again
            defaultEnchants.Add(new Enchant(3842, "Arcanum of the Savage Gladiator", ItemSlot.Head, new Stats() { Stamina = 30, Resilience = 25 }, "ability_warrior_shieldmastery"));
            defaultEnchants.Add(new Enchant(3818, "Arcanum of the Stalwart Protector", ItemSlot.Head, new Stats() { Stamina = 37, DodgeRating = 20 }, "ability_warrior_swordandboard"));
            defaultEnchants.Add(new Enchant(3817, "Arcanum of Torment", ItemSlot.Head, new Stats() { AttackPower = 50, CritRating = 20 }, "ability_warrior_rampage")); // Will probably be vamped again
            defaultEnchants.Add(new Enchant(3797, "Arcanum of Dominance", ItemSlot.Head, new Stats() { SpellPower = 29, Resilience = 20 }, "spell_arcane_arcaneresilience")); // Will probably be vamped again
            defaultEnchants.Add(new Enchant(3815, "Arcanum of the Eclipsed Moon", ItemSlot.Head, new Stats() { ArcaneResistance = 25, Stamina = 30 }, "ability_druid_eclipse"));
            defaultEnchants.Add(new Enchant(3816, "Arcanum of the Flame's Soul", ItemSlot.Head, new Stats() { FireResistance = 25, Stamina = 30 }, "spell_fire_burnout"));
            defaultEnchants.Add(new Enchant(3814, "Arcanum of the Fleeing Shadow", ItemSlot.Head, new Stats() { ShadowResistance = 25, Stamina = 30 }, "ability_paladin_gaurdedbythelight"));
            defaultEnchants.Add(new Enchant(3812, "Arcanum of the Frosty Soul", ItemSlot.Head, new Stats() { FrostResistance = 25, Stamina = 30 }, "spell_frost_frozencore"));
            defaultEnchants.Add(new Enchant(3813, "Arcanum of Toxic Warding", ItemSlot.Head, new Stats() { NatureResistance = 25, Stamina = 30 }, "trade_brewpoison"));
            defaultEnchants.Add(new Enchant(3795, "Arcanum of Triumph", ItemSlot.Head, new Stats() { AttackPower = 50, Resilience = 20 }, "ability_warrior_shieldmastery")); // Will probably be vamped again
            #endregion
            #region Level 70 (BC)
            defaultEnchants.Add(new Enchant(3006, "Arcanum of Arcane Warding", ItemSlot.Head, new Stats() { ArcaneResistance = 20 }, "spell_arcane_arcaneresilience"));
            defaultEnchants.Add(new Enchant(3003, "Arcanum of Ferocity", ItemSlot.Head, new Stats() { AttackPower = 34, HitRating = 16 }, "ability_druid_demoralizingroar"));
            defaultEnchants.Add(new Enchant(3007, "Arcanum of Fire Warding", ItemSlot.Head, new Stats() { FireResistance = 20 }, "spell_fire_sealoffire"));
            defaultEnchants.Add(new Enchant(3008, "Arcanum of Frost Warding", ItemSlot.Head, new Stats() { FrostResistance = 20 }, "spell_frost_frostarmor02"));
            defaultEnchants.Add(new Enchant(3005, "Arcanum of Nature Warding", ItemSlot.Head, new Stats() { NatureResistance = 20 }, "spell_nature_protectionformnature"));
            defaultEnchants.Add(new Enchant(3002, "Arcanum of Power", ItemSlot.Head, new Stats() { SpellPower = 22, HitRating = 14 }, "spell_nature_lightningoverload"));
            defaultEnchants.Add(new Enchant(3001, "Arcanum of Renewal", ItemSlot.Head, new Stats() { Intellect = 16, Spirit = 18 }, "spell_holy_healingaura"));
            defaultEnchants.Add(new Enchant(3009, "Arcanum of Shadow Warding", ItemSlot.Head, new Stats() { ShadowResistance = 20 }, "spell_shadow_sealofkings"));
            defaultEnchants.Add(new Enchant(2999, "Arcanum of the Defender", ItemSlot.Head, new Stats() { ParryRating = 16, DodgeRating = 17 }, "ability_warrior_victoryrush"));
            defaultEnchants.Add(new Enchant(3004, "Arcanum of the Gladiator", ItemSlot.Head, new Stats() { Stamina = 18, Resilience = 20 }, "inv_misc_statue_04"));
            defaultEnchants.Add(new Enchant(3096, "Arcanum of the Outcast", ItemSlot.Head, new Stats() { Strength = 17, Intellect = 16 }, "ability_rogue_masterofsubtlety"));
            #endregion
            // Level 60 (Vanilla)
            // Unsorted
            #endregion
            #region Shoulders
            #region Level 85 (Cataclysm)
            defaultEnchants.Add(new Enchant(4200, "Greater Inscription of Charged Lodestone", ItemSlot.Shoulders, new Stats() { Intellect = 50, HasteRating = 25 }, "inv_misc_gem_bloodstone_02"));
            defaultEnchants.Add(new Enchant(4202, "Greater Inscription of Jagged Stone", ItemSlot.Shoulders, new Stats() { Strength = 50, CritRating = 25 }, "inv_misc_gem_emeraldrough_02"));
            defaultEnchants.Add(new Enchant(4204, "Greater Inscription of Shattered Crystal", ItemSlot.Shoulders, new Stats() { Agility = 50, MasteryRating = 25 }, "inv_misc_gem_goldendraenite_01"));
            defaultEnchants.Add(new Enchant(4198, "Greater Inscription of Unbreakable Quartz", ItemSlot.Shoulders, new Stats() { Stamina = 75, DodgeRating = 25 }, "inv_misc_gem_crystal_01"));
            defaultEnchants.Add(new Enchant(4199, "Lesser Inscription of Charged Lodestone", ItemSlot.Shoulders, new Stats() { Intellect = 30, HasteRating = 20 }, "inv_misc_gem_bloodstone_02"));
            defaultEnchants.Add(new Enchant(4201, "Lesser Inscription of Jagged Stone", ItemSlot.Shoulders, new Stats() { Strength = 30, CritRating = 20 }, "inv_misc_gem_emeraldrough_02"));
            defaultEnchants.Add(new Enchant(4205, "Lesser Inscription of Shattered Crystal", ItemSlot.Shoulders, new Stats() { Agility = 30, MasteryRating = 20 }, "inv_misc_gem_goldendraenite_01"));
            defaultEnchants.Add(new Enchant(4197, "Lesser Inscription of Unbreakable Quartz", ItemSlot.Shoulders, new Stats() { Stamina = 45, DodgeRating = 20 }, "inv_misc_gem_crystal_01"));
            #endregion
            #region Level 80 (WotLK)
            defaultEnchants.Add(new Enchant(3808, "Greater Inscription of the Axe", ItemSlot.Shoulders, new Stats() { AttackPower = 40, CritRating = 15 }, "inv_axe_85"));
            defaultEnchants.Add(new Enchant(3809, "Greater Inscription of the Crag", ItemSlot.Shoulders, new Stats() { Intellect = 21, Spirit = 16 }, "spell_arcane_teleportorgrimmar"));
            defaultEnchants.Add(new Enchant(3811, "Greater Inscription of the Pinnacle", ItemSlot.Shoulders, new Stats() { DodgeRating = 20, Stamina = 22 }, "spell_holy_divinepurpose"));
            defaultEnchants.Add(new Enchant(3810, "Greater Inscription of the Storm", ItemSlot.Shoulders, new Stats() { SpellPower = 24, CritRating = 15 }, "spell_nature_lightningoverload"));
            defaultEnchants.Add(new Enchant(3875, "Lesser Inscription of the Axe", ItemSlot.Shoulders, new Stats() { AttackPower = 30, CritRating = 10 }, "inv_axe_82"));
            defaultEnchants.Add(new Enchant(3807, "Lesser Inscription of the Crag", ItemSlot.Shoulders, new Stats() { Intellect = 15, Spirit = 10 }, "spell_nature_farsight"));
            defaultEnchants.Add(new Enchant(3876, "Lesser Inscription of the Pinnacle", ItemSlot.Shoulders, new Stats() { DodgeRating = 15, ParryRating = 10 }, "spell_holy_divinepurpose"));
            defaultEnchants.Add(new Enchant(3806, "Lesser Inscription of the Storm", ItemSlot.Shoulders, new Stats() { SpellPower = 18, CritRating = 10 }, "spell_nature_lightning"));
            defaultEnchants.Add(new Enchant(3852, "Greater Inscription of the Gladiator", ItemSlot.Shoulders, new Stats() { Stamina = 30, Resilience = 15 }, "inv_shoulder_61"));
            defaultEnchants.Add(new Enchant(3793, "Inscription of Triumph", ItemSlot.Shoulders, new Stats() { AttackPower = 40, Resilience = 15 }, "spell_holy_weaponmastery"));
            defaultEnchants.Add(new Enchant(3794, "Inscription of Dominance", ItemSlot.Shoulders, new Stats() { SpellPower = 23, Resilience = 15 }, "spell_holy_powerinfusion"));
            #endregion
            #region Level 70 (BC)
            defaultEnchants.Add(new Enchant(2982, "Greater Inscription of Discipline", ItemSlot.Shoulders, new Stats() { SpellPower = 18, CritRating = 10 }, "spell_holy_sealofwisdom"));
            defaultEnchants.Add(new Enchant(2980, "Greater Inscription of Faith", ItemSlot.Shoulders, new Stats() { Intellect = 15, Spirit = 10 }, "spell_holy_greaterblessingofsalvation"));
            defaultEnchants.Add(new Enchant(2997, "Greater Inscription of the Blade", ItemSlot.Shoulders, new Stats() { AttackPower = 20, CritRating = 15 }, "spell_holy_weaponmastery"));
            defaultEnchants.Add(new Enchant(2991, "Greater Inscription of the Knight", ItemSlot.Shoulders, new Stats() { ParryRating = 15, DodgeRating = 10 }, "spell_holy_championsgrace"));
            defaultEnchants.Add(new Enchant(2993, "Greater Inscription of the Oracle", ItemSlot.Shoulders, new Stats() { Intellect = 10, Spirit = 16 }, "spell_holy_powerinfusion"));
            defaultEnchants.Add(new Enchant(2995, "Greater Inscription of the Orb", ItemSlot.Shoulders, new Stats() { SpellPower = 12, CritRating = 15 }, "inv_misc_orb_03"));
            defaultEnchants.Add(new Enchant(2986, "Greater Inscription of Vengeance", ItemSlot.Shoulders, new Stats() { AttackPower = 30, CritRating = 10 }, "spell_holy_greaterblessingofkings"));
            defaultEnchants.Add(new Enchant(2978, "Greater Inscription of Warding", ItemSlot.Shoulders, new Stats() { DodgeRating = 15, Stamina = 15 }, "spell_holy_blessingofprotection"));
            defaultEnchants.Add(new Enchant(2998, "Inscription of Endurance", ItemSlot.Shoulders, new Stats() { NatureResistance = 7, ArcaneResistance = 7, FireResistance = 7, FrostResistance = 7, ShadowResistance = 7 }, "Inscription of Endurance"));
            defaultEnchants.Add(new Enchant(2990, "Inscription of the Knight", ItemSlot.Shoulders, new Stats() { DodgeRating = 13 }, "spell_holy_championsbond"));
            defaultEnchants.Add(new Enchant(2977, "Inscription of Warding", ItemSlot.Shoulders, new Stats() { DodgeRating = 13 }, "spell_holy_greaterblessingofsanctuary"));
            defaultEnchants.Add(new Enchant(2996, "Inscription of the Blade", ItemSlot.Shoulders, new Stats() { CritRating = 13 }, "ability_dualwield"));
            defaultEnchants.Add(new Enchant(2983, "Inscription of Vengeance", ItemSlot.Shoulders, new Stats() { AttackPower = 26 }, "spell_holy_fistofjustice"));
            defaultEnchants.Add(new Enchant(2981, "Inscription of Discipline", ItemSlot.Shoulders, new Stats() { SpellPower = 15 }, "spell_holy_sealofwisdom"));
            defaultEnchants.Add(new Enchant(2994, "Inscription of the Orb", ItemSlot.Shoulders, new Stats() { CritRating = 13 }, "inv_misc_orb_04"));
            defaultEnchants.Add(new Enchant(2979, "Inscription of Faith", ItemSlot.Shoulders, new Stats() { SpellPower = 15 }, "spell_holy_sealofsalvation"));
            defaultEnchants.Add(new Enchant(2992, "Inscription of the Oracle", ItemSlot.Shoulders, new Stats() { Spirit = 12 }, "spell_holy_spiritualguidence"));
            #endregion
            #region Level 60 (Vanilla)
            // defaultEnchants.Add(new Enchant(2716, "Fortitude of the Scourge", ItemSlot.Shoulders, new Stats() { Stamina = 16, BonusArmor = 100 }, "spell_shadow_antishadow")); // No longer in the game
            // defaultEnchants.Add(new Enchant(2717, "Might of the Scourge", ItemSlot.Shoulders, new Stats() { AttackPower = 26, CritRating = 14 }, "spell_shadow_deathpact")); // No longer in the game
            // defaultEnchants.Add(new Enchant(2721, "Power of the Scourge", ItemSlot.Shoulders, new Stats() { CritRating = 14, SpellPower = 15 }, "spell_shadow_darkritual")); // No longer in the game
            // defaultEnchants.Add(new Enchant(2715, "Resilience of the Scourge", ItemSlot.Shoulders, new Stats() { SpellPower = 16, Mp5 = 5 }, "spell_shadow_deadofnight")); // No longer in the game
            defaultEnchants.Add(new Enchant(2606, "Zandalar Signet of Might", ItemSlot.Shoulders, new Stats() { AttackPower = 30 }, "inv_misc_armorkit_08"));
            defaultEnchants.Add(new Enchant(2605, "Zandalar Signet of Mojo", ItemSlot.Shoulders, new Stats() { SpellPower = 18 }, "inv_jewelry_ring_46"));
            defaultEnchants.Add(new Enchant(2604, "Zandalar Signet of Serenity", ItemSlot.Shoulders, new Stats() { SpellPower = 18 }, "spell_holy_powerwordshield"));
            #endregion
            #endregion
            #region Back
            #region Level 85 (Cataclysm)
            defaultEnchants.Add(new Enchant(4100, "Greater Critical Strike", ItemSlot.Back, new Stats() { CritRating = 65 }, "inv_enchant_formulagood_01"));
            defaultEnchants.Add(new Enchant(4096, "Greater Intellect", ItemSlot.Back, new Stats() { Intellect = 50 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(4064, "Greater Spell Piercing", ItemSlot.Back, new Stats() { SpellPenetration = 70 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(4090, "Protection", ItemSlot.Back, new Stats() { BonusArmor = 250 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(4087, "Critical Strike", ItemSlot.Back, new Stats() { CritRating = 50 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(4072, "Intellect", ItemSlot.Back, new Stats() { Intellect = 30 }, "spell_holy_greaterheal"));
            #endregion
            #region Level 80 (WotLK)
            defaultEnchants.Add(new Enchant(3243, "Spell Piercing", ItemSlot.Back, new Stats() { SpellPenetration = 35 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(983, "Superior Agility", ItemSlot.Back, new Stats() { Agility = 16 }, "inv_enchant_formulagood_01"));
            defaultEnchants.Add(new Enchant(1262, "Superior Arcane Resistance", ItemSlot.Back, new Stats() { ArcaneResistance = 20 }, "inv_enchant_formulagood_01"));
            defaultEnchants.Add(new Enchant(1446, "Superior Shadow Resistance", ItemSlot.Back, new Stats() { ShadowResistance = 20 }, "inv_enchant_formulagood_01"));
            defaultEnchants.Add(new Enchant(1354, "Superior Fire Resistance", ItemSlot.Back, new Stats() { FireResistance = 20 }, "inv_enchant_formulagood_01"));
            defaultEnchants.Add(new Enchant(3230, "Superior Frost Resistance", ItemSlot.Back, new Stats() { FrostResistance = 20 }, "inv_enchant_formulagood_01"));
            defaultEnchants.Add(new Enchant(1400, "Superior Nature Resistance", ItemSlot.Back, new Stats() { NatureResistance = 20 }, "inv_enchant_formulagood_01"));
            defaultEnchants.Add(new Enchant(1099, "Major Agility", ItemSlot.Back, new Stats() { Agility = 22 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(3831, "Greater Speed", ItemSlot.Back, new Stats() { HasteRating = 23 }, "inv_enchant_formulagood_01"));
            defaultEnchants.Add(new Enchant(3294, "Mighty Armor", ItemSlot.Back, new Stats() { BonusArmor = 225 }, "inv_enchant_formulagood_01"));
            defaultEnchants.Add(new Enchant(1951, "Titanweave", ItemSlot.Back, new Stats() { DodgeRating = 16 }, "inv_enchant_formulagood_01"));
            defaultEnchants.Add(new Enchant(3256, "Shadow Armor", ItemSlot.Back, new Stats() { Agility = 10, BonusArmor = 40 }, "inv_enchant_formulagood_01"));
            defaultEnchants.Add(new Enchant(3296, "Wisdom", ItemSlot.Back, new Stats() { ThreatReductionMultiplier = 0.02f, Spirit = 10 }, "inv_enchant_formulagood_01"));
            #endregion
            #region Level 70 (BC)
            defaultEnchants.Add(new Enchant(368, "Greater Agility", ItemSlot.Back, new Stats() { Agility = 12 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(2662, "Major Armor", ItemSlot.Back, new Stats() { BonusArmor = 120 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(2938, "Spell Penetration", ItemSlot.Back, new Stats() { SpellPenetration = 20 }, "inv_enchant_formulagood_01"));
            defaultEnchants.Add(new Enchant(2664, "Major Resistance", ItemSlot.Back, new Stats() { NatureResistance = 7, ArcaneResistance = 7, FireResistance = 7, FrostResistance = 7, ShadowResistance = 7 }, "inv_enchant_formulagood_01"));
            defaultEnchants.Add(new Enchant(3825, "Speed", ItemSlot.Back, new Stats() { HasteRating = 15 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(1257, "Greater Arcane Resistance", ItemSlot.Back, new Stats() { ArcaneResistance = 15 }, "inv_enchant_formulasuperior_01"));
            defaultEnchants.Add(new Enchant(1441, "Greater Shadow Resistance", ItemSlot.Back, new Stats() { ShadowResistance = 15 }, "inv_enchant_formulasuperior_01"));
            defaultEnchants.Add(new Enchant(2619, "Greater Fire Resistance", ItemSlot.Back, new Stats() { FireResistance = 15 }, "inv_enchant_formulagood_01"));
            defaultEnchants.Add(new Enchant(2648, "Steelweave", ItemSlot.Back, new Stats() { DodgeRating = 12 }, "inv_enchant_formulasuperior_01"));
            #endregion
            #region Level 60 (Vanilla)
            defaultEnchants.Add(new Enchant(2622, "Dodge", ItemSlot.Back, new Stats() { DodgeRating = 12 }, "inv_enchant_formulasuperior_01"));
            defaultEnchants.Add(new Enchant(2621, "Subtlety", ItemSlot.Back, new Stats() { ThreatReductionMultiplier = 0.02f }, "spell_holy_greaterheal"));
            #endregion
            #endregion
            #region Chest
            #region Level 85 (Cataclysm)
            defaultEnchants.Add(new Enchant(4088, "Exceptional Spirit", ItemSlot.Chest, new Stats() { Spirit = 40 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(4103, "Greater Stamina", ItemSlot.Chest, new Stats() { Stamina = 75 }, "inv_enchant_formulagood_01"));
            defaultEnchants.Add(new Enchant(4102, "Peerless Stats", ItemSlot.Chest, new Stats() { Agility = 20, Strength = 20, Stamina = 20, Intellect = 20, Spirit = 20 }, "inv_enchant_formulagood_01"));
            defaultEnchants.Add(new Enchant(4077, "Mighty Resilience", ItemSlot.Chest, new Stats() { Resilience = 40 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(4063, "Mighty Stats", ItemSlot.Chest, new Stats() { Agility = 15, Strength = 15, Stamina = 15, Intellect = 15, Spirit = 15 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(4070, "Stamina", ItemSlot.Chest, new Stats() { Stamina = 55 }, "spell_holy_greaterheal"));
            #endregion
            #region Level 80 (WotLK)
            defaultEnchants.Add(new Enchant(1953, "Greater Defense", ItemSlot.Chest, new Stats() { DodgeRating = 22 }, "inv_enchant_formulagood_01"));
            defaultEnchants.Add(new Enchant(3252, "Super Stats", ItemSlot.Chest, new Stats() { Agility = 8, Strength = 8, Stamina = 8, Intellect = 8, Spirit = 8 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(3832, "Powerful Stats", ItemSlot.Chest, new Stats() { Agility = 10, Strength = 10, Stamina = 10, Intellect = 10, Spirit = 10 }, "inv_enchant_formulagood_01"));
            defaultEnchants.Add(new Enchant(2659, "Exceptional Health", ItemSlot.Chest, new Stats() { Health = 150 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(3236, "Mighty Health", ItemSlot.Chest, new Stats() { Health = 200 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(3297, "Super Health", ItemSlot.Chest, new Stats() { Health = 275 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(2381, "Greater Mana Restoration", ItemSlot.Chest, new Stats() { Mp5 = 10f }, "spell_holy_greaterheal"));
            #endregion
            #region Level 70 (BC)
            defaultEnchants.Add(new Enchant(1951, "Defense", ItemSlot.Chest, new Stats() { DodgeRating = 16 }, "inv_misc_note_01"));
            defaultEnchants.Add(new Enchant(2661, "Exceptional Stats", ItemSlot.Chest, new Stats() { Agility = 6, Strength = 6, Stamina = 6, Intellect = 6, Spirit = 6 }, "inv_misc_note_01"));
            defaultEnchants.Add(new Enchant(2933, "Major Resilience", ItemSlot.Chest, new Stats() { Resilience = 15 }, "inv_enchant_formulagood_01"));
            defaultEnchants.Add(new Enchant(3233, "Exceptional Mana", ItemSlot.Chest, new Stats() { Mana = 250 }, "inv_misc_note_01"));
            defaultEnchants.Add(new Enchant(1144, "Major Spirit", ItemSlot.Chest, new Stats() { Spirit = 15 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(3245, "Exceptional Resilience", ItemSlot.Chest, new Stats() { Resilience = 20 }, "inv_enchant_formulagood_01"));
            defaultEnchants.Add(new Enchant(3150, "Restore Mana Prime", ItemSlot.Chest, new Stats() { Mp5 = 7 }, "spell_holy_greaterheal"));
            #endregion
            // Level 60 (Vanilla)
            // Unsorted
            #endregion
            #region Wrist
            #region Level 85 (Cataclysm)
            defaultEnchants.Add(new Enchant(4071, "Critical Strike", ItemSlot.Wrist, new Stats() { CritRating = 50 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(4086, "Dodge", ItemSlot.Wrist, new Stats() { DodgeRating = 50 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(4089, "Precision", ItemSlot.Wrist, new Stats() { HitRating = 50 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(4093, "Exceptional Spirit", ItemSlot.Wrist, new Stats() { Spirit = 50 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(4095, "Greater Expertise", ItemSlot.Wrist, new Stats() { ExpertiseRating = 50 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(4101, "Greater Critical Strike", ItemSlot.Wrist, new Stats() { CritRating = 65 }, "inv_enchant_formulagood_01"));
            defaultEnchants.Add(new Enchant(4108, "Greater Speed", ItemSlot.Wrist, new Stats() { HasteRating = 65 }, "inv_enchant_formulagood_01"));
            defaultEnchants.Add(new Enchant(4065, "Speed", ItemSlot.Wrist, new Stats() { HasteRating = 50 }, "spell_holy_greaterheal"));
            #endregion
            #region Level 80 (WotLK)
            defaultEnchants.Add(new Enchant(2326, "Greater Spellpower", ItemSlot.Wrist, new Stats() { SpellPower = 23 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(2661, "Greater Stats", ItemSlot.Wrist, new Stats() { Agility = 6, Strength = 6, Stamina = 6, Intellect = 6, Spirit = 6 }, "inv_enchant_formulagood_01"));
            defaultEnchants.Add(new Enchant(1147, "Major Spirit", ItemSlot.Wrist, new Stats() { Spirit = 18 }, "inv_enchant_formulagood_01"));
            defaultEnchants.Add(new Enchant(3231, "Expertise", ItemSlot.Wrist, new Stats() { ExpertiseRating = 15 }, "inv_enchant_formulagood_01"));
            defaultEnchants.Add(new Enchant(3845, "Greater Assault", ItemSlot.Wrist, new Stats() { AttackPower = 50 }, "inv_enchant_formulagood_01"));
            defaultEnchants.Add(new Enchant(2332, "Superior Spellpower", ItemSlot.Wrist, new Stats() { SpellPower = 30 }, "inv_enchant_formulagood_01"));
            defaultEnchants.Add(new Enchant(3850, "Major Stamina", ItemSlot.Wrist, new Stats() { Stamina = 40 }, "inv_enchant_formulasuperior_01"));
            defaultEnchants.Add(new Enchant(1119, "Exceptional Intellect", ItemSlot.Wrist, new Stats() { Intellect = 16 }, "spell_holy_greaterheal"));
            #endregion
            #region Level 70 (BC)
            defaultEnchants.Add(new Enchant(2647, "Brawn", ItemSlot.Wrist, new Stats() { Strength = 12 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(369, "Major Intellect", ItemSlot.Wrist, new Stats() { Intellect = 12 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(1891, "Stats", ItemSlot.Wrist, new Stats() { Agility = 4, Strength = 4, Stamina = 4, Intellect = 4, Spirit = 4 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(2648, "Major Defense", ItemSlot.Wrist, new Stats() { DodgeRating = 12 }, "inv_enchant_formulagood_01"));
            defaultEnchants.Add(new Enchant(2617, "Superior Healing", ItemSlot.Wrist, new Stats() { SpellPower = 15 }, "inv_misc_note_01"));
            defaultEnchants.Add(new Enchant(2679, "Restore Mana Prime", ItemSlot.Wrist, new Stats() { Mp5 = 8 }, "inv_enchant_formulagood_01"));
            defaultEnchants.Add(new Enchant(2649, "Fortitude", ItemSlot.Wrist, new Stats() { Stamina = 12 }, "inv_enchant_formulagood_01"));
            defaultEnchants.Add(new Enchant(1600, "Striking", ItemSlot.Wrist, new Stats() { AttackPower = 38 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(2650, "Spellpower", ItemSlot.Wrist, new Stats() { SpellPower = 15 }, "inv_enchant_formulagood_01"));
            defaultEnchants.Add(new Enchant(1593, "Assault", ItemSlot.Wrist, new Stats() { AttackPower = 24 }, "spell_holy_greaterheal"));
            #endregion
            #region Level 60 (Vanilla)
            defaultEnchants.Add(new Enchant(1886, "Superior Stamina", ItemSlot.Wrist, new Stats() { Stamina = 9 }, "inv_enchant_formulagood_01"));
            #endregion
            // Unsorted
            #endregion
            #region Hands
            #region Level 85 (Cataclysm)
            defaultEnchants.Add(new Enchant(4068, "Haste", ItemSlot.Hands, new Stats() { HasteRating = 50 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(4075, "Exceptional Strength", ItemSlot.Hands, new Stats() { Strength = 35 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(4082, "Greater Expertise", ItemSlot.Hands, new Stats() { ExpertiseRating = 50 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(4107, "Greater Mastery", ItemSlot.Hands, new Stats() { MasteryRating = 65 }, "inv_enchant_formulagood_01"));
            defaultEnchants.Add(new Enchant(4106, "Mighty Strength", ItemSlot.Hands, new Stats() { Strength = 50 }, "inv_enchant_formulagood_01"));
            defaultEnchants.Add(new Enchant(4061, "Mastery", ItemSlot.Hands, new Stats() { MasteryRating = 50 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(3253, "Armsman", ItemSlot.Hands, new Stats() { ThreatIncreaseMultiplier = 0.02f, ParryRating = 10 }, "inv_enchant_formulagood_01"));
            #endregion
            #region Level 80 (WotLK)
            defaultEnchants.Add(new Enchant(3231, "Expertise", ItemSlot.Hands, new Stats() { ExpertiseRating = 15 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(3234, "Precision", ItemSlot.Hands, new Stats() { HitRating = 20 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(3222, "Major Agility", ItemSlot.Hands, new Stats() { Agility = 20 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(3829, "Greater Assault", ItemSlot.Hands, new Stats() { AttackPower = 35 }, "inv_enchant_formulagood_01"));
            defaultEnchants.Add(new Enchant(1603, "Crusher", ItemSlot.Hands, new Stats() { AttackPower = 44 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(3246, "Exceptional Spellpower", ItemSlot.Hands, new Stats() { SpellPower = 28 }, "spell_holy_greaterheal"));
            #endregion
            #region Level 70 (BC)
            defaultEnchants.Add(new Enchant(2934, "Blasting", ItemSlot.Hands, new Stats() { CritRating = 10 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(1594, "Assault", ItemSlot.Hands, new Stats() { AttackPower = 26 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(684, "Major Strength", ItemSlot.Hands, new Stats() { Strength = 15 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(2322, "Major Healing", ItemSlot.Hands, new Stats() { SpellPower = 19 }, "inv_enchant_formulagood_01"));
            defaultEnchants.Add(new Enchant(2937, "Major Spellpower", ItemSlot.Hands, new Stats() { SpellPower = 20 }, "inv_enchant_formulagood_01"));
            defaultEnchants.Add(new Enchant(2935, "Precise Strikes", ItemSlot.Hands, new Stats() { HitRating = 15 }, "inv_enchant_formulagood_01"));
            #endregion
            #region Level 60 (Vanilla)
            defaultEnchants.Add(new Enchant(931, "Minor Haste", ItemSlot.Hands, new Stats() { HasteRating = 10 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(2564, "Superior Agility", ItemSlot.Hands, new Stats() { Agility = 15 }, "inv_enchant_formulasuperior_01"));
            defaultEnchants.Add(new Enchant(2613, "Threat", ItemSlot.Hands, new Stats() { ThreatIncreaseMultiplier = 0.02f }, "inv_enchant_formulasuperior_01"));
            #endregion
            // Unsorted
            #endregion
            #region Legs
            #region Level 85 (Cataclysm)
            defaultEnchants.Add(new Enchant(4127, "Charscale Leg Armor", ItemSlot.Legs, new Stats() { Agility = 55, Stamina = 145 }, "inv_misc_monsterscales_20"));
            defaultEnchants.Add(new Enchant(4126, "Dragonscale Leg Armor", ItemSlot.Legs, new Stats() { AttackPower = 190, CritRating = 55 }, "inv_misc_monsterscales_14"));
            defaultEnchants.Add(new Enchant(4124, "Twilight Leg Armor", ItemSlot.Legs, new Stats() { Agility = 45, Stamina = 85 }, "inv_misc_armorkit_29"));
            defaultEnchants.Add(new Enchant(4122, "Scorched Leg Armor", ItemSlot.Legs, new Stats() { AttackPower = 110, CritRating = 45 }, "inv_misc_armorkit_26"));
            defaultEnchants.Add(new Enchant(4112, "Powerful Enchanted Spellthread", ItemSlot.Legs, new Stats() { Intellect = 95, Stamina = 80 }, "inv_misc_thread_eternium"));
            defaultEnchants.Add(new Enchant(4110, "Powerful Ghostly Spellthread", ItemSlot.Legs, new Stats() { Intellect = 95, Spirit = 55 }, "spell_nature_astralrecalgroup"));
            defaultEnchants.Add(new Enchant(4111, "Enchanted Spellthread", ItemSlot.Legs, new Stats() { Intellect = 55, Stamina = 65 }, "item_spellcloththread"));
            defaultEnchants.Add(new Enchant(4109, "Ghostly Spellthread", ItemSlot.Legs, new Stats() { Intellect = 55, Spirit = 45 }, "spell_nature_astralrecal"));
            #endregion
            #region Level 80 (WotLK)
            defaultEnchants.Add(new Enchant(3853, "Earthen Leg Armor", ItemSlot.Legs, new Stats() { Resilience = 40, Stamina = 28 }, "inv_misc_armorkit_18"));
            defaultEnchants.Add(new Enchant(3822, "Frosthide Leg Armor", ItemSlot.Legs, new Stats() { Agility = 22, Stamina = 55 }, "inv_misc_armorkit_32"));
            defaultEnchants.Add(new Enchant(3823, "Icescale Leg Armor", ItemSlot.Legs, new Stats() { AttackPower = 75, CritRating = 22 }, "inv_misc_armorkit_33"));
            defaultEnchants.Add(new Enchant(3325, "Jormungar Leg Armor", ItemSlot.Legs, new Stats() { Agility = 15, Stamina = 45 }, "inv_misc_armorkit_31"));
            defaultEnchants.Add(new Enchant(3326, "Nerubian Leg Armor", ItemSlot.Legs, new Stats() { AttackPower = 55, CritRating = 15 }, "inv_misc_armorkit_29"));
            defaultEnchants.Add(new Enchant(3719, "Brilliant Spellthread", ItemSlot.Legs, new Stats() { SpellPower = 50, Spirit = 20 }, "spell_nature_astralrecalgroup"));
            defaultEnchants.Add(new Enchant(3721, "Sapphire Spellthread", ItemSlot.Legs, new Stats() { SpellPower = 50, Stamina = 30 }, "spell_nature_astralrecalgroup"));
            defaultEnchants.Add(new Enchant(3720, "Azure Spellthread", ItemSlot.Legs, new Stats() { SpellPower = 35, Stamina = 20 }, "spell_nature_astralrecal"));
            defaultEnchants.Add(new Enchant(3718, "Shining Spellthread", ItemSlot.Legs, new Stats() { SpellPower = 35, Spirit = 12 }, "Shining Spellthread"));
            #endregion
            #region Level 70 (BC)
            defaultEnchants.Add(new Enchant(3013, "Nethercleft Leg Armor", ItemSlot.Legs, new Stats() { Agility = 12, Stamina = 40 }, "inv_misc_armorkit_25"));
            defaultEnchants.Add(new Enchant(3012, "Nethercobra Leg Armor", ItemSlot.Legs, new Stats() { AttackPower = 50, CritRating = 12 }, "inv_misc_armorkit_25"));
            defaultEnchants.Add(new Enchant(3011, "Clefthide Leg Armor", ItemSlot.Legs, new Stats() { Agility = 10, Stamina = 30 }, "inv_misc_armorkit_23"));
            defaultEnchants.Add(new Enchant(3010, "Cobrahide Leg Armor", ItemSlot.Legs, new Stats() { AttackPower = 40, CritRating = 10 }, "inv_misc_armorkit_21"));
            defaultEnchants.Add(new Enchant(2746, "Golden Spellthread", ItemSlot.Legs, new Stats() { SpellPower = 35, Stamina = 20 }, "spell_holy_restoration"));
            defaultEnchants.Add(new Enchant(2748, "Runic Spellthread", ItemSlot.Legs, new Stats() { SpellPower = 35, Stamina = 20 }, "spell_nature_astralrecalgroup"));
            defaultEnchants.Add(new Enchant(2747, "Mystic Spellthread", ItemSlot.Legs, new Stats() { SpellPower = 25, Stamina = 15 }, "spell_nature_astralrecal"));
            defaultEnchants.Add(new Enchant(2745, "Silver Spellthread", ItemSlot.Legs, new Stats() { SpellPower = 25, Stamina = 15 }, "spell_nature_lightning"));
            #endregion
            #region Level 60 (Vanilla)
            #endregion
            // Unsorted
            #endregion
            #region Feet
            #region Level 85 (Cataclysm)
            defaultEnchants.Add(new Enchant(4069, "Haste", ItemSlot.Feet, new Stats() { HasteRating = 50 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(4076, "Major Agility", ItemSlot.Feet, new Stats() { Agility = 35 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(4092, "Precision", ItemSlot.Feet, new Stats() { HitRating = 50 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(4094, "Mastery", ItemSlot.Feet, new Stats() { MasteryRating = 50 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(4105, "Assassin's Step", ItemSlot.Feet, new Stats() { Agility = 25, MovementSpeed = 0.08f }, "inv_enchant_formulagood_01"));
            defaultEnchants.Add(new Enchant(4104, "Lavawalker", ItemSlot.Feet, new Stats() { MasteryRating = 35, MovementSpeed = 0.08f }, "inv_enchant_formulagood_01"));
            defaultEnchants.Add(new Enchant(4062, "Earthen Vitality", ItemSlot.Feet, new Stats() { Stamina = 30, MovementSpeed = 0.08f }, "spell_holy_greaterheal"));
            #endregion
            #region Level 80 (WotLK)
            defaultEnchants.Add(new Enchant(1075, "Greater Fortitude", ItemSlot.Feet, new Stats() { Stamina = 22 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(3826, "Icewalker", ItemSlot.Feet, new Stats() { CritRating = 12, HitRating = 12 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(3244, "Greater Vitality", ItemSlot.Feet, new Stats() { Mp5 = 7f, Hp5 = 7f }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(1147, "Greater Spirit", ItemSlot.Feet, new Stats() { Spirit = 18 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(983, "Superior Agility", ItemSlot.Feet, new Stats() { Agility = 16 }, "inv_enchant_formulagood_01"));
            defaultEnchants.Add(new Enchant(1597, "Greater Assault", ItemSlot.Feet, new Stats() { AttackPower = 32 }, "inv_enchant_formulagood_01"));
            defaultEnchants.Add(new Enchant(3232, "Tuskarr's Vitality", ItemSlot.Feet, new Stats() { Stamina = 15, MovementSpeed = 0.08f }, "inv_enchant_formulagood_01"));
            #endregion
            #region Level 70 (BC)
            defaultEnchants.Add(new Enchant(2656, "Vitality", ItemSlot.Feet, new Stats() { Mp5 = 5f, Hp5 = 5f }, "inv_enchant_formulasuperior_01"));
            defaultEnchants.Add(new Enchant(2649, "Fortitude", ItemSlot.Feet, new Stats() { Stamina = 12 }, "inv_enchant_formulasuperior_01"));
            defaultEnchants.Add(new Enchant(2657, "Dexterity", ItemSlot.Feet, new Stats() { Agility = 12 }, "inv_enchant_formulasuperior_01"));
            defaultEnchants.Add(new Enchant(2940, "Boar's Speed", ItemSlot.Feet, new Stats() { Stamina = 9, MovementSpeed = 0.08f }, "inv_enchant_formulasuperior_01"));
            defaultEnchants.Add(new Enchant(2939, "Cat's Swiftness", ItemSlot.Feet, new Stats() { Agility = 6, MovementSpeed = 0.08f }));
            defaultEnchants.Add(new Enchant(2658, "Surefooted", ItemSlot.Feet, new Stats() { CritRating = 10, HitRating = 10 }, "inv_enchant_formulagood_01"));
            defaultEnchants.Add(new Enchant(3824, "Assault", ItemSlot.Feet, new Stats() { AttackPower = 24 }, "spell_holy_greaterheal"));
            #endregion
            #region Level 60 (Vanilla)
            defaultEnchants.Add(new Enchant(851, "Spirit", ItemSlot.Feet, new Stats() { Spirit = 5 }, "inv_enchant_formulasuperior_01"));
            #endregion
            // Unsorted
            #endregion
            #region Weapons
            #region Two Handers
            #region Level 85 (Cataclysm)
            defaultEnchants.Add(new Enchant(4227, "Mighty Agility", ItemSlot.TwoHand, new Stats() { Agility = 130 }, "inv_potion_162"));
            #endregion
            #region Level 80 (WotLK)
            defaultEnchants.Add(new Enchant(3855, "Spellpower (Staff)", ItemSlot.TwoHand, new Stats() { SpellPower = 69 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(3828, "Greater Savagery", ItemSlot.TwoHand, new Stats() { AttackPower = 85 }, "inv_enchant_formulagood_01"));
            defaultEnchants.Add(new Enchant(3827, "Massacre", ItemSlot.TwoHand, new Stats() { AttackPower = 110 }, "inv_enchant_formulasuperior_01"));
            //defaultEnchants.Add(new Enchant(3827, "Scourgebane", ItemSlot.TwoHand, new Stats() { AttackPowerAgainstUndead = 140 }, "inv_enchant_formulasuperior_01")); // No modelling
            defaultEnchants.Add(new Enchant(3854, "Greater Spellpower (Staff)", ItemSlot.TwoHand, new Stats() { SpellPower = 81 }, "inv_enchant_formulasuperior_01"));
            #endregion
            #region Level 70 (BC)
            defaultEnchants.Add(new Enchant(2667, "Savagery", ItemSlot.TwoHand, new Stats() { AttackPower = 70 }, "inv_enchant_formulagood_01"));
            defaultEnchants.Add(new Enchant(2670, "Major Agility", ItemSlot.TwoHand, new Stats() { Agility = 35 }, "inv_enchant_formulagood_01"));
            #endregion
            #region Level 60 (Vanilla)
            defaultEnchants.Add(new Enchant(241, "Minor Impact", ItemSlot.TwoHand, new Stats() { WeaponDamage = 2 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(943, "Lesser Impact", ItemSlot.TwoHand, new Stats() { WeaponDamage = 3 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(1897, "Impact", ItemSlot.TwoHand, new Stats() { WeaponDamage = 5 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(963, "Greater Impact", ItemSlot.TwoHand, new Stats() { WeaponDamage = 7 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(1896, "Superior Impact", ItemSlot.TwoHand, new Stats() { WeaponDamage = 9 }, "inv_enchant_formulagood_01"));
            defaultEnchants.Add(new Enchant(2646, "Agility", ItemSlot.TwoHand, new Stats() { Agility = 25 }, "inv_misc_note_01"));
            defaultEnchants.Add(new Enchant(1903, "Major Spirit", ItemSlot.TwoHand, new Stats() { Spirit = 9 }, "inv_enchant_formulagood_01"));
            defaultEnchants.Add(new Enchant(1904, "Major Intellect", ItemSlot.TwoHand, new Stats() { Intellect = 9 }, "inv_enchant_formulagood_01"));
            #endregion
            // Unsorted
            #endregion
            #region One Handers (Any of these can also go on TwoHanders)
            #region Level 85 (Cataclysm)
            {
                enchantTemp = new Stats() { };
                enchantTemp.AddSpecialEffect(new SpecialEffect(Trigger.MeleeHit, new Stats() { HasteRating = 450, }, 12, 0, -1f)); // read info off of cata.wowhead.com
                enchantTemp.AddSpecialEffect(new SpecialEffect(Trigger.SpellCast, new Stats() { HasteRating = 450, }, 12, 45, 0.10f)); // read info off of cata.wowhead.com
                defaultEnchants.Add(new Enchant(4083, "Hurricane", ItemSlot.OneHand, enchantTemp, "spell_nature_cyclone"));
            }
            {
                enchantTemp = new Stats() { };
                enchantTemp.AddSpecialEffect(new SpecialEffect(Trigger.SpellCast, new Stats() { Spirit = 200, }, 15, 20, 0.25f)); // read info off of cata.wowhead.com
                defaultEnchants.Add(new Enchant(4084, "Heartsong", ItemSlot.OneHand, enchantTemp, "ability_paladin_sacredcleansing"));
            }
            {
                enchantTemp = new Stats() { };
                enchantTemp.AddSpecialEffect(new SpecialEffect(Trigger.MeleeHit, new Stats() { AttackPower = 1000, }, 12, 0f, -1f)); // read info off of cata.wowhead.com, and based proc times off of berserking
                defaultEnchants.Add(new Enchant(4099, "Landslide", ItemSlot.OneHand, enchantTemp, "ability_paladin_sacredcleansing"));
            }
            {
                enchantTemp = new Stats() { };
                enchantTemp.AddSpecialEffect(new SpecialEffect(Trigger.DamageOrHealingDone, new Stats() { Intellect = 500, }, 12, 45f, 0.33f)); // read info off of cata.wowhead.com
                defaultEnchants.Add(new Enchant(4097, "Power Torrent", ItemSlot.OneHand, enchantTemp, "ability_paladin_sacredcleansing"));
            }
            {
                enchantTemp = new Stats() { };
                enchantTemp.AddSpecialEffect(new SpecialEffect(Trigger.MeleeHit, new Stats() { DodgeRating = 600, MovementSpeed = 0.15f }, 10, 45f, 0.10f)); // read info off of cata.wowhead.com
                defaultEnchants.Add(new Enchant(2668, "Windwalk", ItemSlot.OneHand, enchantTemp, "ability_paladin_sacredcleansing"));
            }
            {
                enchantTemp = new Stats();
                enchantTemp.AddSpecialEffect(new SpecialEffect(Trigger.MeleeHit, new Stats() { NatureDamage = 500f, }, 10f, 0f, -5f)); // 5 PPM
                defaultEnchants.Add(new Enchant(4067, "Avalanche", ItemSlot.OneHand, enchantTemp, "spell_fire_burnout"));
            }
            {
                enchantTemp = new Stats();
                enchantTemp.AddSpecialEffect(new SpecialEffect(Trigger.DamageDone, new Stats() { HealthRestore = 800f, }, 0f, 0f, -4.6f)); // 4.6 PPM
                defaultEnchants.Add(new Enchant(4066, "Mending", ItemSlot.OneHand, enchantTemp, "spell_nature_healingway"));
            }
            defaultEnchants.Add(new Enchant(4217, "Pyrium Weapon Chain", ItemSlot.OneHand, new Stats() { HitRating = 40, DisarmDurReduc = 0.50f }, "inv_misc_steelweaponchain"));
            // Not modelling Elemental Slayer
            #endregion
            #region Level 80 (WotLK)
            defaultEnchants.Add(new Enchant(1606, "Greater Potency", ItemSlot.OneHand, new Stats() { AttackPower = 50 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(3830, "Exceptional Spellpower", ItemSlot.OneHand, new Stats() { SpellPower = 50 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(3844, "Exceptional Spirit", ItemSlot.OneHand, new Stats() { Spirit = 45 }, "inv_enchant_formulagood_01"));
            defaultEnchants.Add(new Enchant(1103, "Exceptional Agility", ItemSlot.OneHand, new Stats() { Agility = 26 }, "inv_enchant_formulagood_01"));
            {
                enchantTemp = new Stats();
                enchantTemp.AddSpecialEffect(new SpecialEffect(Trigger.MeleeHit, new Stats() { FireDamage = 200f, }, 0f, 0f, -3)); // 3 PPM = 9% Chance to proc
                defaultEnchants.Add(new Enchant(3239, "Icebreaker", ItemSlot.OneHand, enchantTemp, "spell_fire_burnout"));
            }
            {
                enchantTemp = new Stats();
                enchantTemp.AddSpecialEffect(new SpecialEffect(Trigger.MeleeHit, new Stats() { HealthRestore = 333f, }, 0f, 0f, -4.6f)); // 4.6 PPM
                defaultEnchants.Add(new Enchant(3241, "Lifeward", ItemSlot.OneHand, enchantTemp, "spell_nature_healingway"));
            }
            defaultEnchants.Add(new Enchant(3834, "Mighty Spellpower", ItemSlot.OneHand, new Stats() { SpellPower = 63 }, "inv_enchant_formulasuperior_01"));
            defaultEnchants.Add(new Enchant(3833, "Superior Potency", ItemSlot.OneHand, new Stats() { AttackPower = 65 }, "inv_enchant_formulasuperior_01"));
            defaultEnchants.Add(new Enchant(3788, "Accuracy", ItemSlot.OneHand, new Stats() { CritRating = 25, HitRating = 25 }, "inv_enchant_formulasuperior_01"));
            {
                enchantTemp = new Stats();
                enchantTemp.AddSpecialEffect(new SpecialEffect(Trigger.MeleeHit, new Stats() { AttackPower = 400f, BonusArmorMultiplier = -.05f }, 15f, 0f, -1f));
                defaultEnchants.Add(new Enchant(3789, "Berserking", ItemSlot.OneHand, enchantTemp, "spell_nature_strength"));
            }
            {
                enchantTemp = new Stats();
                enchantTemp.AddSpecialEffect(new SpecialEffect(Trigger.DamageSpellHit, new Stats() { HasteRating = 250f }, 10f, 35f, 0.35f));
                enchantTemp.AddSpecialEffect(new SpecialEffect(Trigger.MangleCatOrShredOrInfectedWoundsHit, new Stats() { HasteRating = 250f }, 10f, 35f, 0.35f));
                defaultEnchants.Add(new Enchant(3790, "Black Magic", ItemSlot.OneHand, enchantTemp, "spell_shadow_unstableaffliction_1"));
            }
            {
                enchantTemp = new Stats();
                enchantTemp.AddSpecialEffect(new SpecialEffect(Trigger.MeleeHit, new Stats() { ParryRating = 200f, }, 10f, 0f, -1f)); // Need to add the 600-800 Physical Damage on next Parry
                defaultEnchants.Add(new Enchant(3869, "Blade Ward", ItemSlot.OneHand, enchantTemp, "inv_sword_121"));
            }
            {
                Stats blood_drain_a = new Stats(); // Once at 35% Health, your melee Hits restores 400 health per stack
                blood_drain_a.AddSpecialEffect(new SpecialEffect(Trigger.MeleeHit, new Stats() { HealthRestore = (360f + 440f) / 2f }, 20f, 0f, 1f, 5));
                enchantTemp = new Stats();
                enchantTemp.AddSpecialEffect(new SpecialEffect(Trigger.DamageTaken, blood_drain_a, 0f, 0f, .35f));
                defaultEnchants.Add(new Enchant(3870, "Blood Draining", ItemSlot.OneHand, enchantTemp, "inv_misc_gem_bloodstone_03"));
            }
            defaultEnchants.Add(new Enchant(3731, "Titanium Weapon Chain", ItemSlot.OneHand, new Stats() { HitRating = 28, DisarmDurReduc = 0.50f }, "inv_belt_18"));
            #endregion
            #region Level 70 (BC)
            defaultEnchants.Add(new Enchant(2666, "Major Intellect", ItemSlot.OneHand, new Stats() { Intellect = 30 }, "inv_enchant_formulagood_01"));
            defaultEnchants.Add(new Enchant(963, "Major Striking", ItemSlot.OneHand, new Stats() { WeaponDamage = 7 }, "inv_misc_note_01"));
            defaultEnchants.Add(new Enchant(3222, "Greater Agility", ItemSlot.OneHand, new Stats() { Agility = 20 }, "inv_misc_note_01"));
            defaultEnchants.Add(new Enchant(2343, "Major Healing", ItemSlot.OneHand, new Stats() { SpellPower = 40 }, "inv_enchant_formulagood_01"));
            defaultEnchants.Add(new Enchant(2669, "Major Spellpower", ItemSlot.OneHand, new Stats() { SpellPower = 40 }, "inv_enchant_formulagood_01"));
            defaultEnchants.Add(new Enchant(2668, "Potency", ItemSlot.OneHand, new Stats() { Strength = 20 }, "inv_enchant_formulagood_01"));
            {
                enchantTemp = new Stats();
                enchantTemp.AddSpecialEffect(new SpecialEffect(Trigger.MeleeHit, new Stats() { CritRating = 120f }, 15f, 0f, -1f));
                defaultEnchants.Add(new Enchant(3225, "Executioner", ItemSlot.OneHand, enchantTemp, "inv_2h_auchindoun_01"));
            }
            {
                enchantTemp = new Stats();
                enchantTemp.AddSpecialEffect(new SpecialEffect(Trigger.MeleeHit, new Stats() { Agility = 120, HasteRating = 30f }, 15f, 0f, -1f));
                defaultEnchants.Add(new Enchant(2673, "Mongoose", ItemSlot.OneHand, enchantTemp, "spell_nature_unrelentingstorm"));
            }
            defaultEnchants.Add(new Enchant(2672, "Soulfrost", ItemSlot.OneHand, new Stats() { SpellFrostDamageRating = 54, SpellShadowDamageRating = 54 }, "inv_enchant_formulasuperior_01"));
            defaultEnchants.Add(new Enchant(2671, "Sunfire", ItemSlot.OneHand, new Stats() { SpellFireDamageRating = 50, SpellArcaneDamageRating = 50 }, "inv_enchant_formulasuperior_01"));
            defaultEnchants.Add(new Enchant(3223, "Adamantite Weapon Chain", ItemSlot.OneHand, new Stats() { ParryRating = 15, DisarmDurReduc = 0.50f }, "spell_frost_chainsofice"));
            #endregion
            // Level 60 (Vanilla)
            // Unsorted
            #endregion
            #region Off Handers Only (Usually Means Shields)
            #region Level 85 (Cataclysm)
            defaultEnchants.Add(new Enchant(4091, "Superior Intellect", ItemSlot.OffHand, new Stats() { Intellect = 100 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(4073, "Protection", ItemSlot.OffHand, new Stats() { BonusArmor = 160 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(4085, "Blocking", ItemSlot.OffHand, new Stats() { BlockRating = 40 }, "spell_holy_greaterheal"));
            #endregion
            #region Level 80 (WotLK)
            defaultEnchants.Add(new Enchant(1128, "Greater Intellect", ItemSlot.OffHand, new Stats() { Intellect = 25 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(1952, "Defense", ItemSlot.OffHand, new Stats() { DodgeRating = 20 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(3849, "Titanium Plating", ItemSlot.OffHand, new Stats() { ParryRating = 81 }, "inv_shield_19"));
            #endregion
            #region Level 70 (BC)
            defaultEnchants.Add(new Enchant(2653, "Tough Shield", ItemSlot.OffHand, new Stats() { DodgeRating = 12 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(2654, "Intellect", ItemSlot.OffHand, new Stats() { Intellect = 12 }, "inv_misc_note_01"));
            defaultEnchants.Add(new Enchant(1071, "Major Stamina", ItemSlot.OffHand, new Stats() { Stamina = 18 }, "inv_misc_note_01"));
            defaultEnchants.Add(new Enchant(3229, "Resilience", ItemSlot.OffHand, new Stats() { Resilience = 12 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(2655, "Shield Block", ItemSlot.OffHand, new Stats() { ParryRating = 15 }, "inv_enchant_formulagood_01"));
            defaultEnchants.Add(new Enchant(1888, "Resistance", ItemSlot.OffHand, new Stats() { ShadowResistance = 5, ArcaneResistance = 5, NatureResistance = 5, FireResistance = 5, FrostResistance = 5 }, "inv_enchant_formulagood_01"));
            #endregion
            // Level 60 (Vanilla)
            // Unsorted
            #endregion
            #region Ranged
            #region Level 85 (Cataclysm)
            {
                enchantTemp = new Stats();
                enchantTemp.AddSpecialEffect(new SpecialEffect(Trigger.RangedHit, new Stats() { RangedAttackPower = 800 }, 10, 45, 0.10f));
                defaultEnchants.Add(new Enchant(4175, "Gnomish X-Ray Scope", ItemSlot.Back, enchantTemp, "inv_misc_scopea"));
            }
            defaultEnchants.Add(new Enchant(4176, "R19 Threatfinder", ItemSlot.Ranged, new Stats() { HitRating = 88 }, "inv_misc_scopec"));
            defaultEnchants.Add(new Enchant(4177, "Safety Catch Removal Kit", ItemSlot.Ranged, new Stats() { HasteRating = 88 }, "inv_misc_enggizmos_37"));
            #endregion
            #region Level 80 (WotLK)
            defaultEnchants.Add(new Enchant(3608, "Heartseeker Scope", ItemSlot.Ranged, new Stats() { RangedCritRating = 40 }, "inv_misc_spyglass_02"));
            defaultEnchants.Add(new Enchant(2724, "Stabilized Eternium Scope", ItemSlot.Ranged, new Stats() { RangedCritRating = 28 }, "inv_misc_spyglass_02"));
            defaultEnchants.Add(new Enchant(3607, "Sun Scope", ItemSlot.Ranged, new Stats() { RangedHasteRating = 40 }, "inv_misc_spyglass_03"));
            defaultEnchants.Add(new Enchant(3843, "Diamond-cut Refractor Scope", ItemSlot.Ranged, new Stats() { ScopeDamage = 15 }, "ability_hunter_rapidregeneration"));
            #endregion
            #region Level 70 (BC)
            defaultEnchants.Add(new Enchant(2723, "Khorium Scope", ItemSlot.Ranged, new Stats() { ScopeDamage = 12 }, "inv_misc_spyglass_02"));
            defaultEnchants.Add(new Enchant(2722, "Adamantite Scope", ItemSlot.Ranged, new Stats() { ScopeDamage = 10 }, "inv_misc_spyglass_02"));
            #endregion
            #region Level 60 (Vanilla)
            defaultEnchants.Add(new Enchant(2523, "Biznicks 247x128 Accurascope", ItemSlot.Ranged, new Stats() { RangedHitRating = 30 }, "inv_misc_spyglass_02"));
            defaultEnchants.Add(new Enchant(664, "Sniper Scope", ItemSlot.Ranged, new Stats() { ScopeDamage = 7 }, "inv_misc_spyglass_02"));
            defaultEnchants.Add(new Enchant(663, "Deadly Scope", ItemSlot.Ranged, new Stats() { ScopeDamage = 5 }, "inv_misc_spyglass_02"));
            defaultEnchants.Add(new Enchant(33, "Accurate Scope", ItemSlot.Ranged, new Stats() { ScopeDamage = 3 }, "inv_misc_spyglass_02"));
            defaultEnchants.Add(new Enchant(32, "Standard Scope", ItemSlot.Ranged, new Stats() { ScopeDamage = 2 }, "inv_misc_spyglass_02"));
            defaultEnchants.Add(new Enchant(30, "Crude Scope", ItemSlot.Ranged, new Stats() { ScopeDamage = 1 }, "inv_misc_spyglass_02"));
            #endregion
            // Unsorted
            #endregion
            #endregion

            #region Engineering
            #region Level 85 (Cataclysm)
            #region Hands
            {
                enchantTemp = new Stats();
                enchantTemp.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { BonusArmor = 1500f }, 12f, 60f));
                defaultEnchants.Add(new Enchant(4180, "Quickflip Deflection Plates", ItemSlot.Hands, enchantTemp, "inv_misc_desecrated_leatherglove"));
            }
            {
                enchantTemp = new Stats();
                enchantTemp.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { Intellect = 480f }, 12f, 60f));
                defaultEnchants.Add(new Enchant(4179, "Synapse Springs", ItemSlot.Hands, enchantTemp, "spell_shaman_elementaloath"));
            }
            {
                enchantTemp = new Stats();
                enchantTemp.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { NatureDamage = 4800f, }, 0f, 120f));
                defaultEnchants.Add(new Enchant(4181, "Tazik Shocker", ItemSlot.Hands, enchantTemp, "spell_nature_abolishmagic"));
            }
            #endregion
            #region Waist
            {
                enchantTemp = new Stats();
                enchantTemp.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { DamageAbsorbed = 18000f }, 8f, 5 * 60f));
                defaultEnchants.Add(new Enchant(4188, "Grounded Plasma Shield", ItemSlot.Waist, enchantTemp, "inv_boots_plate_13"));
            }
            #endregion
            #endregion
            #region Level 80 (WotLK)
            #region Back
            // defaultEnchants.Add(new Enchant(3859, "Springy Arachnoweave", ItemSlot.Back, new Stats() { SpellPower = 27f })); // No longer supplies any stats, only the parachute
            // defaultEnchants.Add(new Enchant(3605, "Flexweave Underlay", ItemSlot.Back, new Stats() { Agility = 23f })); // No longer supplies any stats, only the parachute
            #endregion
            #region Hands
            {
                enchantTemp = new Stats();
                enchantTemp.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { BonusArmor = 700f }, 14f, 60f));
                defaultEnchants.Add(new Enchant(3860, "Reticulated Armor Webbing", ItemSlot.Hands, enchantTemp, "inv_misc_desecrated_leatherglove"));
            }
            {
                enchantTemp = new Stats();
                enchantTemp.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { HasteRating = 340f }, 12f, 60f));
                defaultEnchants.Add(new Enchant(3604, "Hyperspeed Accelerators", ItemSlot.Hands, enchantTemp, "spell_shaman_elementaloath"));
            }
            {
                enchantTemp = new Stats();
                enchantTemp.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { FireDamage = 1837f, }, 0f, 45f));
                defaultEnchants.Add(new Enchant(3603, "Hand-Mounted Pyro Rocket", ItemSlot.Hands, enchantTemp, "spell_fire_burnout"));
            }
            #endregion
            #region Waist
            {
                enchantTemp = new Stats();
                enchantTemp.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { FireDamage = 875 }, 0, 360, 1f));
                defaultEnchants.Add(new Enchant(3601, "Frag Belt", ItemSlot.Waist, enchantTemp, "spell_fire_selfdestruct")); defaultEnchants.Add(new Enchant(3260, "Glove Reinforcements", ItemSlot.Hands, new Stats() { BonusArmor = 240 }));
            }
            #endregion
            #region Feet
            {
                enchantTemp = new Stats();
                enchantTemp.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { MovementSpeed = 1.50f, }, 5f, 3 * 60f));
                defaultEnchants.Add(new Enchant(3606, "Nitro Boosts", ItemSlot.Feet, enchantTemp, "spell_fire_burningspeed"));
            }
            #endregion
            #endregion
            #endregion
            #region Enchanting
            #region Level 85 (Cataclysm)
            defaultEnchants.Add(new Enchant(4079, "Agility", ItemSlot.Finger, new Stats() { Agility = 40 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(4081, "Greater Stamina", ItemSlot.Finger, new Stats() { Stamina = 60 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(4080, "Intellect", ItemSlot.Finger, new Stats() { Intellect = 40 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(4078, "Strength", ItemSlot.Finger, new Stats() { Strength = 40 }, "spell_holy_greaterheal"));
            #endregion
            #region Level 80 (WotLK)
            defaultEnchants.Add(new Enchant(3839, "Assault", ItemSlot.Finger, new Stats() { AttackPower = 40 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(3840, "Greater Spellpower", ItemSlot.Finger, new Stats() { SpellPower = 23 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(3791, "Stamina", ItemSlot.Finger, new Stats() { Stamina = 30 }, "spell_holy_greaterheal"));
            #endregion
            #region Level 70 (BC)
            defaultEnchants.Add(new Enchant(2928, "Spellpower", ItemSlot.Finger, new Stats() { SpellPower = 12 }, "inv_misc_note_01"));
            defaultEnchants.Add(new Enchant(2929, "Striking", ItemSlot.Finger, new Stats() { WeaponDamage = 2 }, "inv_misc_note_01"));
            defaultEnchants.Add(new Enchant(2930, "Healing Power", ItemSlot.Finger, new Stats() { SpellPower = 12 }, "inv_misc_note_01"));
            defaultEnchants.Add(new Enchant(2931, "Stats", ItemSlot.Finger, new Stats() { Agility = 4, Strength = 4, Stamina = 4, Intellect = 4, Spirit = 4 }, "inv_misc_note_01"));
            #endregion
            // Level 60 (Vanilla)
            // Unsorted
            #endregion
            #region Inscription
            #region Level 85 (Cataclysm)
            defaultEnchants.Add(new Enchant(4196, "Felfire Inscription", ItemSlot.Shoulders, new Stats() { Intellect = 130, HasteRating = 25 }, "inv_misc_mastersinscription"));
            defaultEnchants.Add(new Enchant(4195, "Inscription of the Earth Prince", ItemSlot.Shoulders, new Stats() { Stamina = 195, DodgeRating = 25 }, "inv_misc_mastersinscription"));
            defaultEnchants.Add(new Enchant(4194, "Lionsmane Inscription", ItemSlot.Shoulders, new Stats() { Strength = 130, CritRating = 25 }, "inv_misc_mastersinscription"));
            defaultEnchants.Add(new Enchant(4193, "Swiftsteel Inscription", ItemSlot.Shoulders, new Stats() { Agility = 130, MasteryRating = 25 }, "inv_misc_mastersinscription"));
            #endregion
            #region Level 80 (WotLK)
            defaultEnchants.Add(new Enchant(3835, "Master's Inscription of the Axe", ItemSlot.Shoulders, new Stats() { CritRating = 15, AttackPower = 120 }, "inv_misc_mastersinscription"));
            defaultEnchants.Add(new Enchant(3836, "Master's Inscription of the Crag", ItemSlot.Shoulders, new Stats() { Spirit = 15, Intellect = 60 }, "inv_misc_mastersinscription"));
            defaultEnchants.Add(new Enchant(3837, "Master's Inscription of the Pinnacle", ItemSlot.Shoulders, new Stats() { DodgeRating = 60, ParryRating = 15 }, "inv_misc_mastersinscription"));
            defaultEnchants.Add(new Enchant(3838, "Master's Inscription of the Storm", ItemSlot.Shoulders, new Stats() { CritRating = 15, SpellPower = 70 }, "inv_misc_mastersinscription"));
            #endregion
            #endregion
            #region Leatherworking
            #region Level 85 (Cataclysm)
            #region Wrist
            defaultEnchants.Add(new Enchant(4190, "Draconic Embossment - Agility", ItemSlot.Wrist, new Stats() { Agility = 130 }, "trade_leatherworking"));
            defaultEnchants.Add(new Enchant(4192, "Draconic Embossment - Intellect", ItemSlot.Wrist, new Stats() { Intellect = 130 }, "trade_leatherworking"));
            defaultEnchants.Add(new Enchant(4189, "Draconic Embossment - Stamina", ItemSlot.Wrist, new Stats() { Stamina = 195 }, "trade_leatherworking"));
            defaultEnchants.Add(new Enchant(4191, "Draconic Embossment - Strength", ItemSlot.Wrist, new Stats() { Strength = 130 }, "trade_leatherworking"));
            #endregion
            #region Legs
            defaultEnchants.Add(new Enchant(4127, "Charscale Leg Reinforcements", ItemSlot.Legs, new Stats() { Agility = 55, Stamina = 145 }, "trade_leatherworking"));
            defaultEnchants.Add(new Enchant(4126, "Dragonbone Leg Reinforcements", ItemSlot.Legs, new Stats() { AttackPower = 190, CritRating = 55 }, "trade_leatherworking"));
            #endregion
            #endregion
            #region Level 80 (WotLK)
            #region Wrist
            defaultEnchants.Add(new Enchant(3756, "Fur Lining - Attack Power", ItemSlot.Wrist, new Stats() { AttackPower = 130 }, "trade_leatherworking"));
            defaultEnchants.Add(new Enchant(3757, "Fur Lining - Stamina", ItemSlot.Wrist, new Stats() { Stamina = 102 }, "trade_leatherworking"));
            defaultEnchants.Add(new Enchant(3758, "Fur Lining - Spell Power", ItemSlot.Wrist, new Stats() { SpellPower = 76 }, "trade_leatherworking"));
            defaultEnchants.Add(new Enchant(3759, "Fur Lining - Fire Resist", ItemSlot.Wrist, new Stats() { FireResistance = 70 }, "trade_leatherworking"));
            defaultEnchants.Add(new Enchant(3760, "Fur Lining - Frost Resist", ItemSlot.Wrist, new Stats() { FrostResistance = 70 }, "trade_leatherworking"));
            defaultEnchants.Add(new Enchant(3761, "Fur Lining - Shadow Resist", ItemSlot.Wrist, new Stats() { ShadowResistance = 70 }, "trade_leatherworking"));
            defaultEnchants.Add(new Enchant(3762, "Fur Lining - Nature Resist", ItemSlot.Wrist, new Stats() { NatureResistance = 70 }, "trade_leatherworking"));
            defaultEnchants.Add(new Enchant(3763, "Fur Lining - Arcane Resist", ItemSlot.Wrist, new Stats() { ArcaneResistance = 70 }, "trade_leatherworking"));
            #endregion
            #region Legs
            defaultEnchants.Add(new Enchant(3327, "Jormungar Leg Reinforcements", ItemSlot.Legs, new Stats() { Agility = 22, Stamina = 55 }, "trade_leatherworking"));
            defaultEnchants.Add(new Enchant(3328, "Nerubian Leg Reinforcements", ItemSlot.Legs, new Stats() { AttackPower = 75, CritRating = 22 }, "trade_leatherworking"));
            #endregion
            #endregion
            #endregion
            #region Tailoring
            #region Level 85 (Cataclysm)
            #region Back
            {
                enchantTemp = new Stats() { Spirit = 1 };
                enchantTemp.AddSpecialEffect(new SpecialEffect(Trigger.SpellCast, new Stats() { SpellPower = 580 }, 15, 60, 0.35f));
                defaultEnchants.Add(new Enchant(4115, "Lightweave Embroidery (Rank 2)", ItemSlot.Back, enchantTemp, "spell_arcane_prismaticcloak"));
            }
            {
                enchantTemp = new Stats();
                enchantTemp.AddSpecialEffect(new SpecialEffect(Trigger.PhysicalHit, new Stats() { AttackPower = 1000 }, 15, 45, 0.25f));
                defaultEnchants.Add(new Enchant(4118, "Swordguard Embroidery (Rank 2)", ItemSlot.Back, enchantTemp, "ability_rogue_throwingspecialization"));
            }
            {
                enchantTemp = new Stats();
                enchantTemp.AddSpecialEffect(new SpecialEffect(Trigger.SpellCast, new Stats() { ManaRestore = 800 }, 1f, 60f, 0.35f));
                defaultEnchants.Add(new Enchant(4116, "Darkglow Embroidery (Rank 2)", ItemSlot.Back, enchantTemp, "spell_nature_giftofthewaterspirit"));
            }
            #endregion
            #region Legs
            defaultEnchants.Add(new Enchant(4114, "Sanctified Spellthread (Rank 2)", ItemSlot.Legs, new Stats() { Intellect = 95, Spirit = 55 }, "spell_nature_astralrecalgroup"));
            defaultEnchants.Add(new Enchant(4113, "Master's Spellthread (Rank 2)", ItemSlot.Legs, new Stats() { Intellect = 95f, Stamina = 80f }, "spell_nature_astralrecalgroup"));
            #endregion
            #endregion
            #region Level 80 (WotLK)
            #region Back
            {
                enchantTemp = new Stats() { Spirit = 1 };
                enchantTemp.AddSpecialEffect(new SpecialEffect(Trigger.SpellCast, new Stats() { SpellPower = 295 }, 15, 60, 0.35f));
                defaultEnchants.Add(new Enchant(3722, "Lightweave Embroidery", ItemSlot.Back, enchantTemp, "spell_arcane_prismaticcloak"));
            }
            {
                enchantTemp = new Stats();
                enchantTemp.AddSpecialEffect(new SpecialEffect(Trigger.PhysicalHit, new Stats() { AttackPower = 400 }, 15, 45, 0.25f));
                defaultEnchants.Add(new Enchant(3730, "Swordguard Embroidery", ItemSlot.Back, enchantTemp, "ability_rogue_throwingspecialization"));
            }
            {
                enchantTemp = new Stats();
                enchantTemp.AddSpecialEffect(new SpecialEffect(Trigger.SpellCast, new Stats() { ManaRestore = 400 }, 1f, 60f, 0.35f));
                defaultEnchants.Add(new Enchant(3728, "Darkglow Embroidery", ItemSlot.Back, enchantTemp, "spell_nature_giftofthewaterspirit"));
            }
            #endregion
            #region Legs
            defaultEnchants.Add(new Enchant(3872, "Sanctified Spellthread (Rank 1)", ItemSlot.Legs, new Stats() { SpellPower = 50, Spirit = 20 }, "spell_nature_astralrecalgroup"));
            defaultEnchants.Add(new Enchant(3873, "Master's Spellthread (Rank 1)", ItemSlot.Legs, new Stats() { SpellPower = 50f, Stamina = 30f }, "spell_nature_astralrecalgroup"));
            #endregion
            #endregion
            #endregion

            #region Death Knight Rune Enchants
            {
                enchantTemp = new Stats() { BonusFrostWeaponDamage = .02f };
                // Updated Razorice for patch 3.3.3
                enchantTemp.AddSpecialEffect(new SpecialEffect(Trigger.MeleeHit, new Stats() { BonusFrostDamageMultiplier = 0.02f }, 20f, 0f, 1f, 5));
                defaultEnchants.Add(new Enchant(3370, "Rune of Razorice", ItemSlot.OneHand, enchantTemp, "spell_frost_frostarmor"));
            }
            {
                enchantTemp = new Stats();
                enchantTemp.AddSpecialEffect(new SpecialEffect(Trigger.DamageDone, new Stats() { BonusStrengthMultiplier = .15f }, 15f, 0f, -2f, 1));
                enchantTemp.AddSpecialEffect(new SpecialEffect(Trigger.DamageDone, new Stats() { HealthRestoreFromMaxHealth = .03f }, 0, 0f, -2f, 1));
                defaultEnchants.Add(new Enchant(3368, "Rune of the Fallen Crusader", ItemSlot.OneHand, enchantTemp, "spell_holy_retributionaura"));
            }
            {
                enchantTemp = new Stats();
                enchantTemp.AddSpecialEffect(new SpecialEffect(Trigger.DamageDone, new Stats() { CinderglacierProc = 2f }, 0f, 0f, -1.5f));
                defaultEnchants.Add(new Enchant(3369, "Rune of Cinderglacier", ItemSlot.OneHand, enchantTemp, "spell_shadow_chilltouch"));
            }
            defaultEnchants.Add(new Enchant(3365, "Rune of Swordshattering", ItemSlot.TwoHand, new Stats() { Parry = 0.04f }, "ability_parry"));
            defaultEnchants.Add(new Enchant(3594, "Rune of Swordbreaking", ItemSlot.OneHand, new Stats() { Parry = 0.02f }, "ability_parry"));
            defaultEnchants.Add(new Enchant(3847, "Rune of the Stoneskin Gargoyle", ItemSlot.TwoHand, new Stats() { BaseArmorMultiplier = .04f, BonusArmorMultiplier = .04f, BonusStaminaMultiplier = 0.02f }, "inv_sword_130"));
            defaultEnchants.Add(new Enchant(3883, "Rune of the Nerubian Carapace", ItemSlot.OneHand, new Stats() { BaseArmorMultiplier = .02f, BonusArmorMultiplier = .02f, BonusStaminaMultiplier = 0.01f }, "inv_sword_61"));
            #endregion

            #region Armor Kits (Chest, Hands, Legs, Feet)
            #region Level 85 (Cataclysm)
            defaultEnchants.Add(new Enchant(4120, "Savage Armor Kit", ItemSlot.Head, new Stats() { Stamina = 36 }, "inv_misc_armorkit_28"));
            defaultEnchants.Add(new Enchant(4120, "Savage Armor Kit", ItemSlot.Chest, new Stats() { Stamina = 36 }, "inv_misc_armorkit_28"));
            defaultEnchants.Add(new Enchant(4120, "Savage Armor Kit", ItemSlot.Shoulders, new Stats() { Stamina = 36 }, "inv_misc_armorkit_28"));
            defaultEnchants.Add(new Enchant(4120, "Savage Armor Kit", ItemSlot.Legs, new Stats() { Stamina = 36 }, "inv_misc_armorkit_28"));
            defaultEnchants.Add(new Enchant(4120, "Savage Armor Kit", ItemSlot.Hands, new Stats() { Stamina = 36 }, "inv_misc_armorkit_28"));
            defaultEnchants.Add(new Enchant(4120, "Savage Armor Kit", ItemSlot.Feet, new Stats() { Stamina = 36 }, "inv_misc_armorkit_28"));
            defaultEnchants.Add(new Enchant(4121, "Heavy Savage Armor Kit", ItemSlot.Head, new Stats() { Stamina = 44 }, "inv_misc_armorkit_30"));
            defaultEnchants.Add(new Enchant(4121, "Heavy Savage Armor Kit", ItemSlot.Chest, new Stats() { Stamina = 44 }, "inv_misc_armorkit_30"));
            defaultEnchants.Add(new Enchant(4121, "Heavy Savage Armor Kit", ItemSlot.Shoulders, new Stats() { Stamina = 44 }, "inv_misc_armorkit_30"));
            defaultEnchants.Add(new Enchant(4121, "Heavy Savage Armor Kit", ItemSlot.Legs, new Stats() { Stamina = 44 }, "inv_misc_armorkit_30"));
            defaultEnchants.Add(new Enchant(4121, "Heavy Savage Armor Kit", ItemSlot.Hands, new Stats() { Stamina = 44 }, "inv_misc_armorkit_30"));
            defaultEnchants.Add(new Enchant(4121, "Heavy Savage Armor Kit", ItemSlot.Feet, new Stats() { Stamina = 44 }, "inv_misc_armorkit_30"));
            #endregion
            #region Level 80 (WotLK)
            defaultEnchants.Add(new Enchant(3329, "Borean Armor Kit", ItemSlot.Head, new Stats() { Stamina = 12 }, "inv_misc_armorkit_28"));
            defaultEnchants.Add(new Enchant(3329, "Borean Armor Kit", ItemSlot.Chest, new Stats() { Stamina = 12 }, "inv_misc_armorkit_28"));
            defaultEnchants.Add(new Enchant(3329, "Borean Armor Kit", ItemSlot.Shoulders, new Stats() { Stamina = 12 }, "inv_misc_armorkit_28"));
            defaultEnchants.Add(new Enchant(3329, "Borean Armor Kit", ItemSlot.Legs, new Stats() { Stamina = 12 }, "inv_misc_armorkit_28"));
            defaultEnchants.Add(new Enchant(3329, "Borean Armor Kit", ItemSlot.Hands, new Stats() { Stamina = 12 }, "inv_misc_armorkit_28"));
            defaultEnchants.Add(new Enchant(3329, "Borean Armor Kit", ItemSlot.Feet, new Stats() { Stamina = 12 }, "inv_misc_armorkit_28"));
            defaultEnchants.Add(new Enchant(3330, "Heavy Borean Armor Kit", ItemSlot.Head, new Stats() { Stamina = 18 }, "inv_misc_armorkit_30"));
            defaultEnchants.Add(new Enchant(3330, "Heavy Borean Armor Kit", ItemSlot.Chest, new Stats() { Stamina = 18 }, "inv_misc_armorkit_30"));
            defaultEnchants.Add(new Enchant(3330, "Heavy Borean Armor Kit", ItemSlot.Shoulders, new Stats() { Stamina = 18 }, "inv_misc_armorkit_30"));
            defaultEnchants.Add(new Enchant(3330, "Heavy Borean Armor Kit", ItemSlot.Legs, new Stats() { Stamina = 18 }, "inv_misc_armorkit_30"));
            defaultEnchants.Add(new Enchant(3330, "Heavy Borean Armor Kit", ItemSlot.Hands, new Stats() { Stamina = 18 }, "inv_misc_armorkit_30"));
            defaultEnchants.Add(new Enchant(3330, "Heavy Borean Armor Kit", ItemSlot.Feet, new Stats() { Stamina = 18 }, "inv_misc_armorkit_30"));
            #endregion
            #region Level 70 (BC)
            defaultEnchants.Add(new Enchant(2792, "Knothide Armor Kit", ItemSlot.Head, new Stats() { Stamina = 8 }, "inv_misc_armorkit_24"));
            defaultEnchants.Add(new Enchant(2792, "Knothide Armor Kit", ItemSlot.Chest, new Stats() { Stamina = 8 }, "inv_misc_armorkit_24"));
            defaultEnchants.Add(new Enchant(2792, "Knothide Armor Kit", ItemSlot.Shoulders, new Stats() { Stamina = 8 }, "inv_misc_armorkit_24"));
            defaultEnchants.Add(new Enchant(2792, "Knothide Armor Kit", ItemSlot.Legs, new Stats() { Stamina = 8 }, "inv_misc_armorkit_24"));
            defaultEnchants.Add(new Enchant(2792, "Knothide Armor Kit", ItemSlot.Hands, new Stats() { Stamina = 8 }, "inv_misc_armorkit_24"));
            defaultEnchants.Add(new Enchant(2792, "Knothide Armor Kit", ItemSlot.Feet, new Stats() { Stamina = 8 }, "inv_misc_armorkit_24"));
            defaultEnchants.Add(new Enchant(2841, "Heavy Knothide Armor Kit", ItemSlot.Head, new Stats() { Stamina = 10 }, "inv_misc_armorkit_25"));
            defaultEnchants.Add(new Enchant(2841, "Heavy Knothide Armor Kit", ItemSlot.Chest, new Stats() { Stamina = 10 }, "inv_misc_armorkit_25"));
            defaultEnchants.Add(new Enchant(2841, "Heavy Knothide Armor Kit", ItemSlot.Shoulders, new Stats() { Stamina = 10 }, "inv_misc_armorkit_25"));
            defaultEnchants.Add(new Enchant(2841, "Heavy Knothide Armor Kit", ItemSlot.Legs, new Stats() { Stamina = 10 }, "inv_misc_armorkit_25"));
            defaultEnchants.Add(new Enchant(2841, "Heavy Knothide Armor Kit", ItemSlot.Hands, new Stats() { Stamina = 10 }, "inv_misc_armorkit_25"));
            defaultEnchants.Add(new Enchant(2841, "Heavy Knothide Armor Kit", ItemSlot.Feet, new Stats() { Stamina = 10 }, "inv_misc_armorkit_25"));
            defaultEnchants.Add(new Enchant(2793, "Vindicator's Armor Kit", ItemSlot.Chest, new Stats() { DodgeRating = 8 }, "inv_misc_armorkit_26"));
            defaultEnchants.Add(new Enchant(2793, "Vindicator's Armor Kit", ItemSlot.Legs, new Stats() { DodgeRating = 8 }, "inv_misc_armorkit_26"));
            defaultEnchants.Add(new Enchant(2793, "Vindicator's Armor Kit", ItemSlot.Hands, new Stats() { DodgeRating = 8 }, "inv_misc_armorkit_26"));
            defaultEnchants.Add(new Enchant(2793, "Vindicator's Armor Kit", ItemSlot.Feet, new Stats() { DodgeRating = 8 }, "inv_misc_armorkit_26"));
            defaultEnchants.Add(new Enchant(2794, "Magister's Armor Kit", ItemSlot.Chest, new Stats() { Spirit = 8 }, "inv_misc_armorkit_22"));
            defaultEnchants.Add(new Enchant(2794, "Magister's Armor Kit", ItemSlot.Legs, new Stats() { Spirit = 8 }, "inv_misc_armorkit_22"));
            defaultEnchants.Add(new Enchant(2794, "Magister's Armor Kit", ItemSlot.Hands, new Stats() { Spirit = 8 }, "inv_misc_armorkit_22"));
            defaultEnchants.Add(new Enchant(2794, "Magister's Armor Kit", ItemSlot.Feet, new Stats() { Spirit = 8 }, "inv_misc_armorkit_22"));
            defaultEnchants.Add(new Enchant(2984, "Shadow Armor Kit", ItemSlot.Chest, new Stats() { ShadowResistance = 8 }, "spell_shadow_antishadow"));
            defaultEnchants.Add(new Enchant(2984, "Shadow Armor Kit", ItemSlot.Legs, new Stats() { ShadowResistance = 8 }, "spell_shadow_antishadow"));
            defaultEnchants.Add(new Enchant(2984, "Shadow Armor Kit", ItemSlot.Hands, new Stats() { ShadowResistance = 8 }, "spell_shadow_antishadow"));
            defaultEnchants.Add(new Enchant(2984, "Shadow Armor Kit", ItemSlot.Feet, new Stats() { ShadowResistance = 8 }, "spell_shadow_antishadow"));
            defaultEnchants.Add(new Enchant(2985, "Flame Armor Kit", ItemSlot.Chest, new Stats() { FireResistance = 8 }, "spell_fire_sealoffire"));
            defaultEnchants.Add(new Enchant(2985, "Flame Armor Kit", ItemSlot.Legs, new Stats() { FireResistance = 8 }, "spell_fire_sealoffire"));
            defaultEnchants.Add(new Enchant(2985, "Flame Armor Kit", ItemSlot.Hands, new Stats() { FireResistance = 8 }, "spell_fire_sealoffire"));
            defaultEnchants.Add(new Enchant(2985, "Flame Armor Kit", ItemSlot.Feet, new Stats() { FireResistance = 8 }, "spell_fire_sealoffire"));
            defaultEnchants.Add(new Enchant(2987, "Frost Armor Kit", ItemSlot.Chest, new Stats() { FrostResistance = 8 }, "spell_frost_wizardmark"));
            defaultEnchants.Add(new Enchant(2987, "Frost Armor Kit", ItemSlot.Legs, new Stats() { FrostResistance = 8 }, "spell_frost_wizardmark"));
            defaultEnchants.Add(new Enchant(2987, "Frost Armor Kit", ItemSlot.Hands, new Stats() { FrostResistance = 8 }, "spell_frost_wizardmark"));
            defaultEnchants.Add(new Enchant(2987, "Frost Armor Kit", ItemSlot.Feet, new Stats() { FrostResistance = 8 }, "spell_frost_wizardmark"));
            defaultEnchants.Add(new Enchant(2988, "Nature Armor Kit", ItemSlot.Chest, new Stats() { NatureResistance = 8 }, "spell_nature_spiritarmor"));
            defaultEnchants.Add(new Enchant(2988, "Nature Armor Kit", ItemSlot.Legs, new Stats() { NatureResistance = 8 }, "spell_nature_spiritarmor"));
            defaultEnchants.Add(new Enchant(2988, "Nature Armor Kit", ItemSlot.Hands, new Stats() { NatureResistance = 8 }, "spell_nature_spiritarmor"));
            defaultEnchants.Add(new Enchant(2988, "Nature Armor Kit", ItemSlot.Feet, new Stats() { NatureResistance = 8 }, "spell_nature_spiritarmor"));
            defaultEnchants.Add(new Enchant(2989, "Arcane Armor Kit", ItemSlot.Chest, new Stats() { ArcaneResistance = 8 }, "spell_shadow_sealofkings"));
            defaultEnchants.Add(new Enchant(2989, "Arcane Armor Kit", ItemSlot.Legs, new Stats() { ArcaneResistance = 8 }, "spell_shadow_sealofkings"));
            defaultEnchants.Add(new Enchant(2989, "Arcane Armor Kit", ItemSlot.Hands, new Stats() { ArcaneResistance = 8 }, "spell_shadow_sealofkings"));
            defaultEnchants.Add(new Enchant(2989, "Arcane Armor Kit", ItemSlot.Feet, new Stats() { ArcaneResistance = 8 }, "spell_shadow_sealofkings"));
            #endregion
            // Level 60 (Vanilla)
            // Unsorted
            #endregion
            #region Vanilla & BC Enchants for both Head & Legs
            // Level 85 (Cataclysm)
            // Level 80 (WotLK)
            // Level 70 (BC)
            // Level 60 (Vanilla)
            defaultEnchants.Add(new Enchant(2583, "Presence of Might", ItemSlot.Head, new Stats() { Stamina = 10, DodgeRating = 10, ParryRating = 10 }, "spell_holy_sealofwrath"));
            defaultEnchants.Add(new Enchant(2583, "Presence of Might", ItemSlot.Legs, new Stats() { Stamina = 10, DodgeRating = 10, ParryRating = 10 }, "spell_holy_sealofwrath"));
            //defaultEnchants.Add(new Enchant(2543, "Arcanum of Rapidity", ItemSlot.Head, new Stats() { HasteRating = 10 }, "inv_misc_gem_02")); // Removed from game
            //defaultEnchants.Add(new Enchant(2543, "Arcanum of Rapidity", ItemSlot.Legs, new Stats() { HasteRating = 10 }, "inv_misc_gem_02")); // Removed from game
            defaultEnchants.Add(new Enchant(1505, "Lesser Arcanum of Resilience", ItemSlot.Head, new Stats() { FireResistance = 20 }, "inv_misc_gem_03"));
            defaultEnchants.Add(new Enchant(1505, "Lesser Arcanum of Resilience", ItemSlot.Legs, new Stats() { FireResistance = 20 }, "inv_misc_gem_03"));
            // Unsorted
            #endregion

            return defaultEnchants;
        }
    }
}
