using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
//using System.Xml.Serialization;

namespace Rawr
{
    //[GenerateSerializer]
    public class TinkeringList : List<Tinkering> { }

    /// <summary>An object representing a Tinkering to be placed on a slot on a character.</summary>
    public class Tinkering
    {
        #region Variables
        /// <summary>
        /// The ID of the Tinkering. This is determined by viewing the Tinkering spell on Wowhead, and
        /// noting the Tinkering Item Permenant ID in the spell effect data.
        /// 
        /// EXAMPLE:
        /// Tinkering Gloves - Quickflip Deflection Plates. This Tinkering is applied by spell 82177, which you can find
        /// by searching on Wowhead (http://www.wowhead.com/?spell=25080). In the spell Effect section, it
        /// says "Enchant Item: Quickflip Deflection Plates (4180)". The Tinkering ID is 4180.
        /// </summary>
        public int Id;
        /// <summary>The name of the Tinkering.</summary>
        public string Name;
        /// <summary>A shortened version of the Tinkering's Name.</summary>
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
        /// <summary>A REALLY shortened version of the Tinkering's Name. For those names that are still too long to fit in the space on screen</summary>
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
        /// The slot that the Tinkering is applied to. If the Tinkering is available on multiple slots,
        /// define the Tinkering multiple times, once for each slot.
        /// <para>IMPORTANT: Shield Tinkerings should be defined as Off-Hand</para>
        /// </summary>
        public ItemSlot Slot = ItemSlot.Head;
        /// <summary>The stats that the Tinkering gives the character.</summary>
        public Stats Stats = new Stats();
        private static TinkeringList _allTinkerings;
        /// <summary>A List&lt;Tinkering&gt; containing all known Tinkerings relevant to all models.</summary>
        public static List<Tinkering> AllTinkerings { get { return _allTinkerings; } }
        /// <summary>If set, will attempt to pull this icon from wow.com or wowhead.com for the comparison list</summary>
        public string IconSource { get; set; }
        #endregion

        public Tinkering() { }
        /// <summary>Creates a new Tinkering, representing a Tinkering to a single slot.</summary>
        /// <example>Tinkering tink = new Tinkering(4180, "Quickflip Deflection Plates", ItemSlot.Hands, new Stats());<br />
        /// tink.Stats.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats(){ BonusArmor = 1500 }, 12, 60));</example>
        /// <param name="id">The Tinkering ID for the Tinkering. See the Id property for details of how to find this.</param>
        /// <param name="name">The name of the Tinkering.</param>
        /// <param name="slot">The slot that this instance of the Tinkering applies to. (Create multiple Tinkering
        /// objects for Tinkerings which may be applied to multiple slots)</param>
        /// <param name="icon">The Icon name (eg- "spell_fire_masterofelements"). Defaults to null for no Icon</param>
        /// <param name="stats">The stats that the Tinkering gives the character.</param>
        public Tinkering(int id, string name, ItemSlot slot, Stats stats, string icon=null)
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
            Tinkering temp = obj as Tinkering;
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

        public bool FitsInSlot(ItemSlot slot, Character character) { return Calculations.TinkeringFitsInSlot(this, character, slot); }

        public static Tinkering FindTinkering(int id, ItemSlot slot, Character character)
        {
            //&UT&
            // chance for Null
            if (null == AllTinkerings) return null;
            return AllTinkerings.Find(new Predicate<Tinkering>(delegate(Tinkering Tinkering) {
                return (Tinkering.Id == id) && (Tinkering.FitsInSlot(slot, character) ||
                  (Tinkering.Slot == ItemSlot.TwoHand && slot == ItemSlot.OneHand));
            })) ?? AllTinkerings[0];
        }

        private static List<Tinkering> _cachedTinkeringOptions = null;
        private static ItemSlot _cachedTinkeringOptions_slot = ItemSlot.None;
        private static List<string> _cachedTinkeringOptions_avail = null;
        public static List<Tinkering> GetTinkeringOptions(Item baseItem, Character character)
        {
            // Try to use caching to save us time
            List<Tinkering> options;
            if (_cachedTinkeringOptions_slot == baseItem.Slot
                && _cachedTinkeringOptions_avail == character.AvailableItems)
            {
                options = _cachedTinkeringOptions;
            } else {
                options = FindTinkerings(baseItem.Slot, character);
                // Look for Enchants marked available (automatically includes "No Enchant")
                if (options.Count > 1) { options = options.FindAll(e => (character.GetItemAvailability(e) != ItemAvailability.NotAvailable || e.Id == 0)); }
                _cachedTinkeringOptions_slot = baseItem.Slot;
                _cachedTinkeringOptions_avail = new List<string>(character.AvailableItems.ToArray());
                _cachedTinkeringOptions = options;
            }
            //
            return options;
        }

        public static List<Tinkering> FindTinkerings(ItemSlot slot, Character character)
        {
            return FindTinkerings(slot, character, Calculations.Instance);
        }

        public static List<Tinkering> FindTinkerings(ItemSlot slot, Character character, CalculationsBase model)
        {
            //List<ItemSlot> validSlots = new List<ItemSlot>();
            //if (slot != ItemSlot.MainHand)
            //    validSlots.Add(slot);
            //if (slot == ItemSlot.OffHand || slot == ItemSlot.MainHand || slot == ItemSlot.TwoHand)
            //    validSlots.Add(ItemSlot.OneHand);
            //if (slot == ItemSlot.MainHand)
            //    validSlots.Add(ItemSlot.TwoHand);
            return AllTinkerings.FindAll(new Predicate<Tinkering>(
                delegate(Tinkering Tinkering)
                {
                    return Tinkering.Slot == ItemSlot.None ||
                        model.IsTinkeringRelevant(Tinkering, character) &&
                        (slot == ItemSlot.None || Tinkering.FitsInSlot(slot, character));
                }
            ));
        }

        public static List<Tinkering> FindAllTinkerings(ItemSlot slot, Character character)
        {
            //List<ItemSlot> validSlots = new List<ItemSlot>();
            //if (slot != ItemSlot.MainHand)
            //    validSlots.Add(slot);
            //if (slot == ItemSlot.OffHand || slot == ItemSlot.MainHand || slot == ItemSlot.TwoHand)
            //    validSlots.Add(ItemSlot.OneHand);
            //if (slot == ItemSlot.MainHand)
            //    validSlots.Add(ItemSlot.TwoHand);
            return AllTinkerings.FindAll(new Predicate<Tinkering>(
                delegate(Tinkering Tinkering)
                {
                    return Tinkering.Slot == ItemSlot.None ||
                        (slot == ItemSlot.None || Tinkering.FitsInSlot(slot, character));
                }
            ));
        }

        public static List<Tinkering> FindTinkerings(ItemSlot slot, Character character, List<string> availableIds)
        {
            return FindTinkerings(slot, character, availableIds, Calculations.Instance);
        }

        public static List<Tinkering> FindTinkerings(ItemSlot slot, Character character, List<string> availableIds, CalculationsBase model)
        {
            return AllTinkerings.FindAll(new Predicate<Tinkering>(
                delegate(Tinkering Tinkering)
                {
                    return Tinkering.Id == 0 || 
                        ((Tinkering.Slot == ItemSlot.None || 
                        model.IsTinkeringRelevant(Tinkering, character) && (slot == ItemSlot.None || Tinkering.FitsInSlot(slot, character)))
                        && availableIds.Contains((-1 * (Tinkering.Id + ((int)AvailableItemIDModifiers.Tinkerings * (int)Tinkering.Slot))).ToString()));
                }
            ));
        }

        public static List<Tinkering> FindTinkerings(ItemSlot slot, Character[] characters, List<string> availableIds, CalculationsBase[] models)
        {
            return AllTinkerings.FindAll(new Predicate<Tinkering>(
                delegate(Tinkering Tinkering)
                {
                    bool isRelevant = false;
                    for (int i = 0; i < models.Length; i++)
                    {
                        if (models[i].IsTinkeringRelevant(Tinkering, characters[i]) && (Tinkering.FitsInSlot(slot, characters[i]) || slot == ItemSlot.None))
                        {
                            isRelevant = true;
                            break;
                        }
                    }
                    return ((isRelevant || Tinkering.Slot == ItemSlot.None)
                        && availableIds.Contains((-1 * (Tinkering.Id + ((int)AvailableItemIDModifiers.Tinkerings * (int)Tinkering.Slot))).ToString()))
                        || Tinkering.Id == 0;
                }
            ));
        }

        /*public static void Save(TextWriter writer)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(TinkeringList));
            serializer.Serialize(writer, _allTinkerings);
            writer.Close();
        }

        public static void Load(TextReader reader)
        {
            _allTinkerings = null;
            try {
                XmlSerializer serializer = new XmlSerializer(typeof(TinkeringList));
                _allTinkerings = (TinkeringList)serializer.Deserialize(reader);
                reader.Close();
            } catch {
            } finally {
                reader.Close();
                _allTinkerings = _allTinkerings ?? new TinkeringList();
            }
            List<Tinkering> defaultTinkerings = GetDefaultTinkerings();
            for (int defaultTinkeringIndex = 0; defaultTinkeringIndex < defaultTinkerings.Count; defaultTinkeringIndex++)
            {
                bool found = false;
                for (int allTinkeringIndex = 0; allTinkeringIndex < _allTinkerings.Count; allTinkeringIndex++)
                {
                    if (defaultTinkerings[defaultTinkeringIndex].Id == _allTinkerings[allTinkeringIndex].Id &&
                        defaultTinkerings[defaultTinkeringIndex].Slot == _allTinkerings[allTinkeringIndex].Slot &&
                        defaultTinkerings[defaultTinkeringIndex].Name == _allTinkerings[allTinkeringIndex].Name
                        )
                    {
                        if (defaultTinkerings[defaultTinkeringIndex].Stats != _allTinkerings[allTinkeringIndex].Stats)
                        {
                            if (defaultTinkerings[defaultTinkeringIndex].Stats == null)
                            {
                                _allTinkerings.RemoveAt(allTinkeringIndex);
                            } else {
                                _allTinkerings[allTinkeringIndex].Stats = defaultTinkerings[defaultTinkeringIndex].Stats;
                            }
                        }
                        found = true;
                        break;
                    }
                }
                if (!found && defaultTinkerings[defaultTinkeringIndex].Stats != null)
                {
                    _allTinkerings.Add(defaultTinkerings[defaultTinkeringIndex]);
                }
            }
        }*/
        public static void LoadDefaultTinkerings() {
            _allTinkerings = new TinkeringList();
            _allTinkerings.AddRange(GetDefaultTinkerings());
        }

        private static List<Tinkering> GetDefaultTinkerings()
        {
            List<Tinkering> defaultTinkerings = new List<Tinkering>();
            Tinkering TinkeringTemp;

            // The All Important No Tinkering, works in all slots
            defaultTinkerings.Add(new Tinkering(0, "No Tinkering", ItemSlot.None, new Stats(), ""));

            // http://www.wowhead.com/spell=82177 [4180] Quickflip Deflection Plates    (Gloves):        1500 Armour       , 12s / 1min
            // http://www.wowhead.com/spell=82175 [4179] Synapse Springs                (Gloves):         480 Int          , 12s / 1min
            // http://www.wowhead.com/spell=82180 [4181] Tazik Shocker                  (Gloves):  4320- 5280 Nature Dmg   ,       2min
            // http://www.wowhead.com/spell=84427 [4188] Grounded Plasma Shield         (Waist ): 16200-19800 Absorb Shield,       5min
            // http://www.wowhead.com/spell=84425 Cardboard Assassin, not modelling this as its just a distracting target
            // http://www.wowhead.com/spell=84424 Invisibility Field, this is just an out of combat stealth
            // http://www.wowhead.com/spell=82200 Spinal Healing Injector, this is a healing pot, should be handled by Buffs in Rawr
            // http://www.wowhead.com/spell=82201 Z50 Mana Gulper, this is a mana pot, should be handled by Buffs in Rawr
            // http://www.wowhead.com/spell=55002 [3605] Flexweave Underlay, this is a slowfall

            #region Back
            //defaultTinkerings.Add(new Tinkering(3859, "Springy Arachnoweave", ItemSlot.Back, new Stats())); // No longer supplies any stats, only the parachute
            //defaultTinkerings.Add(new Tinkering(3605, "Flexweave Underlay", ItemSlot.Back, new Stats())); // No longer supplies any stats, only the parachute
            #endregion
            #region Gloves
            // Cataclysm
            defaultTinkerings.Add(TinkeringTemp = new Tinkering(4180, "Quickflip Deflection Plates", ItemSlot.Hands, new Stats(), "trade_engineering"));
            TinkeringTemp.Stats.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { BonusArmor = 1500, }, 12, 60));
            defaultTinkerings.Add(TinkeringTemp = new Tinkering(4179, "Synapse Springs", ItemSlot.Hands, new Stats(), "trade_engineering"));
            // Patch 4.0.6+ Synapse Springs now increase Agility, Strength, or Intellect (whichever is highest for the character).
            TinkeringTemp.Stats.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { HighestStat = 480, }, 10, 60));
            defaultTinkerings.Add(TinkeringTemp = new Tinkering(4181, "Tazik Shocker", ItemSlot.Hands, new Stats(), "trade_engineering"));
            TinkeringTemp.Stats.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { NatureDamage = (4320 + 5280) / 2f, }, 0, 2 * 60));
            // WotLK
            defaultTinkerings.Add(TinkeringTemp = new Tinkering(3860, "Reticulated Armor Webbing", ItemSlot.Hands, new Stats(), "trade_engineering"));
            TinkeringTemp.Stats.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { BonusArmor = 700f }, 14f, 60f));
            defaultTinkerings.Add(TinkeringTemp = new Tinkering(3604, "Hyperspeed Accelerators", ItemSlot.Hands, new Stats(), "trade_engineering"));
            TinkeringTemp.Stats.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { HasteRating = 240f }, 12f, 60f));
            defaultTinkerings.Add(TinkeringTemp = new Tinkering(3603, "Hand-Mounted Pyro Rocket", ItemSlot.Hands, new Stats(), "trade_engineering"));
            TinkeringTemp.Stats.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { FireDamage = 1837f, }, 0f, 45f));
            #endregion
            #region Waist
            // This effect has a proc that can make you 100% crittable for 10 sec
            // This should make it wholly undesirable to tanks
            defaultTinkerings.Add(TinkeringTemp = new Tinkering(4188, "Grounded Plasma Shield", ItemSlot.Waist, new Stats(), "trade_engineering"));
            TinkeringTemp.Stats.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { DamageAbsorbed = (16200 + 19800) / 2f, }, 0, 5 * 60));
            TinkeringTemp.Stats.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { CritChanceReduction = -100f, }, 10, 5 * 60, 0.10f));
            defaultTinkerings.Add(TinkeringTemp = new Tinkering(3601, "Frag Belt", ItemSlot.Waist, new Stats(), "spell_fire_selfdestruct"));
            TinkeringTemp.Stats.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { FireDamage = 875 }, 0, 6 * 60));
            defaultTinkerings.Add(TinkeringTemp = new Tinkering(4223, "Nitro Boosts", ItemSlot.Waist, new Stats(), "spell_fire_burningspeed"));
            TinkeringTemp.Stats.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { MovementSpeed = 1.50f, }, 5f, 3 * 60f));
            #endregion

            return defaultTinkerings;
        }
    }
}
