using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml.Serialization;

namespace Rawr
{
    public class BuffList : List<Buff>
    {
    }

    public class Buff
    {
        //early morning
        //summer soul and solace
        //the world is watching
        //viscious circle
        public string Name;
        public string Group;
        public Stats Stats = new Stats();
        public string SetName;
        public string Source;
        public int SetThreshold = 0;
        public List<Buff> Improvements = new List<Buff>();
        public bool IsCustom = false;
        private List<string> _conflictingBuffs = null;
        public List<string> ConflictingBuffs
        {
            get { return _conflictingBuffs ?? (_conflictingBuffs = new List<string>(new string[] { Group })); }
            set { _conflictingBuffs = value; }
        }

        private static readonly string _savedFilePath;
        static Buff()
        {
            _savedFilePath = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "Data" + System.IO.Path.DirectorySeparatorChar + "BuffCache.xml");
            LoadBuffs();
            SaveBuffs();
        }
        //washing virgin halo
        public Buff()
        {

        }

        private static void SaveBuffs()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(_savedFilePath, false, Encoding.UTF8))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(List<Buff>));
                    serializer.Serialize(writer, _allBuffs);
                    writer.Close();
                }
            }
            catch (Exception)
            {
                // Log.Write(ex.Message);
                // Log.Write(ex.StackTrace);
            }
        }

        private static void LoadBuffs()
        {
            try
            {
                List<Buff> loadedBuffs = new List<Buff>();
                try
                {
                    if (File.Exists(_savedFilePath))
                    {
                        using (StreamReader reader = new StreamReader(_savedFilePath, Encoding.UTF8))
                        {
                            XmlSerializer serializer = new XmlSerializer(typeof(BuffList));
                            loadedBuffs = (List<Buff>)serializer.Deserialize(reader);
                            reader.Close();
                        }
                    }
                }
                catch (System.Exception)
                {
                    //Log.Write(ex.Message);
#if !DEBUG
                    MessageBox.Show("The current BuffCache.xml file was made with a previous version of Rawr, which is incompatible with the current version. It will be replaced with buff data included in the current version.", "Incompatible BuffCache.xml", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    //The designer really doesn't like loading the stuff from a file
#endif
                }
                //the serializer doens't throw an exception in the designer, just sets the value null, have to move this outside the try cactch
                loadedBuffs = loadedBuffs ?? new List<Buff>();
                List<Buff> defaultBuffs = GetDefaultBuffs();
                Dictionary<string, Buff> allBuffs = new Dictionary<string, Buff>();
                foreach (Buff loadedBuff in loadedBuffs)
                    if (loadedBuff.IsCustom)
                        allBuffs.Add(loadedBuff.Name, loadedBuff);
                foreach (Buff defaultBuff in defaultBuffs)
                    if (!allBuffs.ContainsKey(defaultBuff.Name))
                        allBuffs.Add(defaultBuff.Name, defaultBuff);
                Buff[] allBuffArray = new Buff[allBuffs.Count];
                allBuffs.Values.CopyTo(allBuffArray, 0);
                _allBuffs = new List<Buff>(allBuffs.Values);
                CacheSetBonuses(); // cache it at the start because we don't like on demand caching with multithreading
            }
            catch { }
        }

        private static void CacheSetBonuses()
        {
            foreach (Buff buff in AllBuffs)
            {
                string setName = buff.SetName;
                if (!string.IsNullOrEmpty(setName))
                {
                    List<Buff> setBonuses;
                    if (!setBonusesByName.TryGetValue(setName, out setBonuses))
                    {
                        setBonuses = new List<Buff>();
                        setBonusesByName[setName] = setBonuses;
                    }
                    setBonuses.Add(buff);
                }
            }
        }

        //you're in agreement
        public override string ToString()
        {
            string summary = Name + ": ";
            summary += Stats.ToString();
            summary = summary.TrimEnd(',', ' ', ':');
            return summary;
        }

        //you can understand
        public static Buff GetBuffByName(string name)
        {
            /*foreach (Buff buff in AllBuffs)
                if (buff.Name == name)
                    return buff;
            return null;*/
            Buff buff;
            AllBuffsByName.TryGetValue(name, out buff);
            return buff;
        }

        //enter static
        private static string _cachedModel = "";
        private static List<Buff> _relevantBuffs = new List<Buff>();
        public static List<Buff> RelevantBuffs
        {
            get
            {
                if (Calculations.Instance == null || _cachedModel != Calculations.Instance.ToString() || _relevantBuffs == null)
                {
                    if (Calculations.Instance != null)
                    {
                        _cachedModel = Calculations.Instance.ToString();
                        _relevantBuffs = AllBuffs.FindAll(buff => Calculations.IsBuffRelevant(buff));
                    }
                    else
                        _relevantBuffs = new List<Buff>();
                }
                return _relevantBuffs;
            }
        }

        private static List<Buff> _allSetBonuses = null;
        public static List<Buff> AllSetBonuses
        {
            get
            {
                if (_allSetBonuses == null)
                {
                    _allSetBonuses = AllBuffs.FindAll(buff => !string.IsNullOrEmpty(buff.SetName));
                }
                return _allSetBonuses;
            }
        }

        private static Dictionary<string, List<Buff>> setBonusesByName = new Dictionary<string, List<Buff>>();
        public static List<Buff> GetSetBonuses(string setName)
        {
            List<Buff> setBonuses;
            if (!setBonusesByName.TryGetValue(setName, out setBonuses))
            {
                return new List<Buff>(); // if it's not cached we know we don't have any
            }
            return setBonuses;
        }

        private static List<Buff> _relevantSetBonuses = null;
        public static List<Buff> RelevantSetBonuses
        {
            get
            {
                if (Calculations.Instance == null || _cachedModel != Calculations.Instance.ToString() || _relevantSetBonuses == null)
                {
                    if (Calculations.Instance != null)
                    {
                        _cachedModel = Calculations.Instance.ToString();
                        _relevantSetBonuses = AllBuffs.FindAll(buff =>
                            Calculations.HasRelevantStats(buff.GetTotalStats()) && !string.IsNullOrEmpty(buff.SetName));
                    }
                    else
                        _relevantSetBonuses = new List<Buff>();
                }
                return _relevantSetBonuses;
            }
        }

        private static Dictionary<string, Buff> _allBuffsByName = null;
        private static Dictionary<string, Buff> AllBuffsByName
        {
            get
            {
                if (_allBuffsByName == null)
                {
                    _allBuffsByName = new Dictionary<string, Buff>();
                    foreach (Buff buff in AllBuffs)
                    {
                        if (!_allBuffsByName.ContainsKey(buff.Name))
                        {
                            _allBuffsByName.Add(buff.Name, buff);
                            foreach (Buff improvement in buff.Improvements)
                            {
                                if (!_allBuffsByName.ContainsKey(improvement.Name))
                                {
                                    _allBuffsByName.Add(improvement.Name, improvement);
                                }
                            }
                        }
                    }
                }
                return _allBuffsByName;
            }
        }

        public Stats GetTotalStats()
        {
            Stats ret = new Stats();
            ret += this.Stats;
            foreach (Buff buff in Improvements)
                ret += buff.Stats;
            return ret;
        }

        //a grey mistake
        private static List<Buff> _allBuffs = null;
        public static List<Buff> AllBuffs
        {
            get { return _allBuffs; }
        }

        private static List<Buff> GetDefaultBuffs()
        {
            List<Buff> defaultBuffs = new List<Buff>();
            Buff buff;

            #region Buffs

            #region Agility and Strength
            defaultBuffs.Add(new Buff
            {
                Name = "Strength of Earth Totem",
                Source = "Shaman",
                Group = "Agility and Strength",
                Stats = { Strength = 155, Agility = 155 },
				ConflictingBuffs = { "Agility", "Strength" },
                Improvements = { 
					new Buff { Name = "Enhancing Totems (Agility/Strength)", Stats = { Strength = (float)Math.Floor(155f * 0.15f), Agility = (float)Math.Floor(155f * 0.15f) } }
				}
            });

            defaultBuffs.Add(new Buff
            {
                Name = "Horn of Winter",
                Source = "Death Knight",
                Group = "Agility and Strength",
                Stats = { Strength = 155, Agility = 155 },
                ConflictingBuffs = { "Agility", "Strength" }
            });
            #endregion

            #region Armor
            defaultBuffs.Add(new Buff
            {
                Name = "Devotion Aura",
                Source = "Paladin",
                Group = "Armor",
                Stats = { BonusArmor = 1205f },
                Improvements = { 
					new Buff { Name = "Improved Devotion Aura (Armor)", Source = "Prot Paladin", Stats = { BonusArmor = (float)Math.Floor(1205f * 0.5f) } }
				}
            });
            #endregion

            #region Armor (%)
            defaultBuffs.Add(new Buff
            {
                Name = "Ancestral Healing",
                Source = "Resto Shaman",
                Group = "Armor (%)",
                Stats = { BonusArmorMultiplier = 0.25f }
            });
            defaultBuffs.Add(new Buff
            {
                Name = "Inspiration",
                Source = "Priest",
                Group = "Armor (%)",
                Stats = { BonusArmorMultiplier = 0.25f }
            });
            #endregion

            #region Attack Power
            defaultBuffs.Add(new Buff
            {
                Name = "Battle Shout",
                Source = "Warrior",
                Group = "Attack Power",
                Stats = { AttackPower = 548 },
                Improvements = { 
					new Buff { Name = "Commanding Presence (Attack Power)", Stats = { AttackPower = (float)Math.Floor(548f * 0.25f) } }
				}
            });
            defaultBuffs.Add(new Buff
            {
                Name = "Blessing of Might",
                Source = "Paladin",
                Group = "Attack Power",
                Stats = { AttackPower = 550 },
                Improvements = { 
					new Buff { Name = "Improved Blessing of Might", Stats = { AttackPower = (float)Math.Floor(550f * 0.25f) } }
				}
            });
            #endregion

            #region Attack Power (%)
            defaultBuffs.Add(new Buff
            {
                Name = "Trueshot Aura",
                Source = "MM Hunter",
                Group = "Attack Power (%)",
                Stats = { BonusAttackPowerMultiplier = 0.1f }
            });
            defaultBuffs.Add(new Buff
            {
                Name = "Unleashed Rage",
                Source = "Enhance Shaman",
                Group = "Attack Power (%)",
                Stats = { BonusAttackPowerMultiplier = 0.1f }
            });
            defaultBuffs.Add(new Buff
            {
                Name = "Abomination's Might",
                Source = "Blood Death Knight",
                Group = "Attack Power (%)",
                Stats = { BonusAttackPowerMultiplier = 0.1f }
            });
            #endregion

            #region Damage (%)
            defaultBuffs.Add(new Buff
            {
                Name = "Ferocious Inspiration",
                Source = "BM Hunter",
                Group = "Damage (%)",
                Stats = { BonusDamageMultiplier = 0.03f }
            });
            defaultBuffs.Add(new Buff
            {
                Name = "Sanctified Retribution",
                Source = "Ret Paladin",
                Group = "Damage (%)",
                Stats = { BonusDamageMultiplier = 0.03f }
            });
            #endregion

            #region Damage Reduction (%)
            defaultBuffs.Add(new Buff
            {
                Name = "Blessing of Sanctuary",
                Source = "Prot Paladin",
                Group = "Damage Reduction (%)",
                Stats = { DamageTakenMultiplier = -0.03f }
            });
            defaultBuffs.Add(new Buff
            {
                Name = "Grace",
                Source = "Disc Priest",
                Group = "Damage Reduction (%)",
                Stats = { DamageTakenMultiplier = -0.03f }
            });
            #endregion

            #region Haste (%)
            defaultBuffs.Add(new Buff
            {
                Name = "Improved Moonkin Form",
                Source = "Moonkin Druid",
                Group = "Haste (%)",
                Stats = { PhysicalHaste = 0.03f, SpellHaste = 0.03f }
            });
            defaultBuffs.Add(new Buff
            {
                Name = "Swift Retribution",
                Source = "Ret Paladin",
                Group = "Haste (%)",
                Stats = { PhysicalHaste = 0.03f, SpellHaste = 0.03f }
            });
            #endregion

            #region Healing Received (%)
            defaultBuffs.Add(new Buff
            {
                Name = "Improved Devotion Aura (Healing Received %)",
                Source = "Prot Paladin",
                Group = "Healing Received (%)",
                Stats = { HealingReceivedMultiplier = 0.06f }
            });
            defaultBuffs.Add(new Buff
            {
                Name = "Tree of Life Aura",
                Source = "Resto Druid",
                Group = "Healing Received (%)",
                Stats = { HealingReceivedMultiplier = 0.06f }
            });
            #endregion

            #region Health
            defaultBuffs.Add(new Buff
            {
                Name = "Blood Pact",
                Source = "Warlock Imp",
                Group = "Health",
                Stats = { Health = 1330 },
                Improvements = { 
					new Buff { Name = "Improved Imp", Stats = { Health = (float)Math.Floor(1330f * 0.30f) } }
				}
            });
            defaultBuffs.Add(new Buff
            {
                Name = "Commanding Shout",
                Source = "Warrior",
                Group = "Health",
                Stats = { Health = 2250 },
                Improvements = { 
					new Buff { Name = "Commanding Presence (Health)", Stats = { Health = (float)Math.Floor(2250f * 0.25f) } }
				}
            });
            #endregion

            #region Intellect
            defaultBuffs.Add(new Buff
            {
                Name = "Arcane Intellect",
                Source = "Mage",
                Group = "Intellect",
                Stats = { Intellect = 60 },
                ConflictingBuffs = new List<string>(new string[] { "Intellect" }),
            });
            defaultBuffs.Add(new Buff
            {
                Name = "Fel Intelligence (Intellect)",
                Source = "Warlock",
                Group = "Intellect",
                Stats = { Intellect = 48 },
                Improvements = { 
					new Buff { Name = "Improved Felhunter", Source = "Afflic Warlock", Stats = { Intellect = (float)Math.Floor(48f * 0.1f) } }
                },
                ConflictingBuffs = new List<string>(new string[] { "Intellect" }),
            });
            #endregion

            #region Physical Critical Strike Chance
            defaultBuffs.Add(new Buff
            {
                Name = "Leader of the Pack",
                Source = "Feral Druid",
                Group = "Physical Critical Strike Chance",
                Stats = { PhysicalCrit = 0.05f }
            });
            defaultBuffs.Add(new Buff
            {
                Name = "Rampage",
                Source = "Fury Warrior",
                Group = "Physical Critical Strike Chance",
                Stats = { PhysicalCrit = 0.05f }
            });
            #endregion

            #region Physical Haste
            defaultBuffs.Add(new Buff
            {
                Name = "Windfury Totem",
                Source = "Shaman",
                Group = "Physical Haste",
                Stats = { PhysicalHaste = 0.16f },
                Improvements = { 
					new Buff { Name = "Improved Windfury Totem", Source = "Enhance Shaman", Stats = { PhysicalHaste = (1.2f/1.16f) - 1f } }
				}
            });

            defaultBuffs.Add(new Buff
            {
                Name = "Improved Icy Talons",
                Source = "Death Knight",
                Group = "Physical Haste",
                Stats = { PhysicalHaste = 0.2f }
            });
            #endregion

            #region Replenishment
            defaultBuffs.Add(new Buff
            {
                Name = "Hunting Party",
                Source = "Survival Hunter",
                Group = "Replenishment",
                Stats = { ManaRestoreFromMaxManaPerSecond = 0.0025f }
            });
            defaultBuffs.Add(new Buff
            {
                Name = "Judgements of the Wise",
                Source = "Ret Paladin",
                Group = "Replenishment",
                Stats = { ManaRestoreFromMaxManaPerSecond = 0.0025f }
            });
            defaultBuffs.Add(new Buff
            {
                Name = "Vampiric Touch",
                Source = "Shadow Priest",
                Group = "Replenishment",
                Stats = { ManaRestoreFromMaxManaPerSecond = 0.0025f }
            });
            defaultBuffs.Add(new Buff
            {
                Name = "Improved Soul Leech",
                Source = "Dest Warlock",
                Group = "Replenishment",
                Stats = { ManaRestoreFromMaxManaPerSecond = 0.0025f }
            });
            #endregion

            #region Mana Regeneration
            defaultBuffs.Add(new Buff
            {
                Name = "Mana Spring Totem",
                Source = "Shaman",
                Group = "Mana Regeneration",
                Stats = { Mp5 = 91 },
                Improvements = { 
					new Buff { Name = "Restorative Totems", Source = "Resto Shaman", Stats = { Mp5 = (float)Math.Floor(91f * 0.20f) } }
				}
            });
            defaultBuffs.Add(new Buff
            {
                Name = "Blessing of Wisdom",
                Source = "Paladin",
                Group = "Mana Regeneration",
                Stats = { Mp5 = 91 },
                Improvements = { 
					new Buff { Name = "Improved Blessing of Wisdom", Source = "Holy Paladin", Stats = { Mp5 = (float)Math.Floor(91 * 0.2f) } }
				}
            });
            #endregion

            #region Spell Critical Strike Chance
            defaultBuffs.Add(new Buff
            {
                Name = "Elemental Oath",
                Source = "Elem Shaman",
                Group = "Spell Critical Strike Chance",
                Stats = { SpellCrit = 0.05f }
            });
            defaultBuffs.Add(new Buff
            {
                Name = "Moonkin Form",
                Source = "Moonkin Druid",
                Group = "Spell Critical Strike Chance",
                Stats = { SpellCrit = 0.05f }
            });
            #endregion

            #region Focus Magic, Spell Critical Strike Chance
            defaultBuffs.Add(new Buff()
            {
                Name = "Focus Magic",
                Source = "Mage",
                Group = "Focus Magic, Spell Critical Strike Chance",
                Stats = { SpellCrit = 0.03f }
            });
            #endregion

            #region Spell Haste
            defaultBuffs.Add(new Buff
            {
                Name = "Wrath of Air Totem",
                Source = "Shaman",
                Group = "Spell Haste",
                Stats = { SpellHaste = 0.05f }
            });
            #endregion

            #region Spell Power
            defaultBuffs.Add(new Buff
            {
                Name = "Demonic Pact",
                Source = "Demo Warlock",
                Group = "Spell Power",
                Stats = { BonusSpellPowerDemonicPactMultiplier = 0.1f }
            });
            defaultBuffs.Add(new Buff
            {
                Name = "Flametongue Totem",
                Source = "Shaman",
                Group = "Spell Power",
                Stats = { SpellPower = 144f },
                Improvements = {
					new Buff { Name = "Enhancing Totems (Spell Power)", Stats = { SpellPower = (float)Math.Floor(144f * 0.15f) } }
                },
            });
            defaultBuffs.Add(new Buff
            {
                Name = "Totem of Wrath (Spell Power)",
                Source = "Elem Shaman",
                Group = "Spell Power",
                Stats = { SpellPower = 280f }
            });
           #endregion

            #region Spell Sensitivity
            defaultBuffs.Add(new Buff
            {
                Name = "Amplify Magic (on target, not self)",
                Source = "Mage",
                Group = "Spell Sensitivity",
                Stats = { SpellPower = 255 },
                Improvements = { 
					new Buff { Name = "Magic Attunement", Stats = { SpellPower = (float)Math.Floor(255f * 0.5f) } } 
				}
            });
            #endregion

            #region Spirit
            defaultBuffs.Add(new Buff
            {
                Name = "Fel Intelligence (Spirit)",
                Source = "Warlock",
                Group = "Spirit",
                Stats = { Spirit = 64f },
                Improvements = { 
					new Buff { Name = "Improved Felhunter", Source = "Afflic Warlock", Stats = { Spirit = (float)Math.Floor(64f * 0.1f) } }
                },
                ConflictingBuffs = new List<string>(new string[] { "Spirit" }),
            });
            defaultBuffs.Add(new Buff
            {
                Name = "Divine Spirit",
                Source = "Priest",
                Group = "Spirit",
                Stats = { Spirit = 80f },
                ConflictingBuffs = new List<string>(new string[] { "Spirit" }),
            });
            #endregion

            #region Stamina
            defaultBuffs.Add(new Buff
            {
                Name = "Power Word: Fortitude",
                Source = "Priest",
                Group = "Stamina",
                Stats = { Stamina = 165f },
                Improvements = { 
					new Buff { Name = "Improved Power Word: Fortitude", Stats = { Stamina = (float)Math.Floor(165f * 0.3f) } } 
				},
                ConflictingBuffs = new List<string>(new string[] { "Stamina" }),
            });
            #endregion

            #region Stat Add
            defaultBuffs.Add(new Buff
            {
                Name = "Mark of the Wild",
                Source = "Druid",
                Group = "Stat Add",
                Stats =
                {
                    BonusArmor = 750,
                    Strength = 37,
                    Agility = 37,
                    Stamina = 37,
                    Intellect = 37,
                    Spirit = 37,
                    ArcaneResistanceBuff = 54,
                    FireResistanceBuff = 54,
                    FrostResistanceBuff = 54,
                    NatureResistanceBuff = 54,
                    ShadowResistanceBuff = 54
                },
				ConflictingBuffs = { "StatArmor" },
                Improvements = { 
					new Buff { Name = "Improved Mark of the Wild", Stats = {
					BonusArmor = (float)Math.Floor(750f * 0.4f),
					Strength = (float)Math.Floor(37f * 0.4f),
					Agility = (float)Math.Floor(37f * 0.4f),
					Stamina = (float)Math.Floor(37f * 0.4f),
					Intellect = (float)Math.Floor(37f * 0.4f),
					Spirit = (float)Math.Floor(37f * 0.4f),
					ArcaneResistanceBuff = (float)Math.Floor(54f * 1.4f), // it is 1.4 because it's non stacking buff, it takes max
					FireResistanceBuff = (float)Math.Floor(54f * 1.4f),
					FrostResistanceBuff = (float)Math.Floor(54f * 1.4f),
					NatureResistanceBuff = (float)Math.Floor(54f * 1.4f),
					ShadowResistanceBuff = (float)Math.Floor(54f * 1.4f)} 
					} 
				}
            });
            #endregion

            #region Stat Multiplier
            defaultBuffs.Add(new Buff
            {
                Name = "Blessing of Kings",
                Source = "Paladin",
                Group = "Stat Multiplier",
                Stats =
                {
                    BonusAgilityMultiplier = 0.1f,
                    BonusStrengthMultiplier = 0.1f,
                    BonusIntellectMultiplier = 0.1f,
                    BonusStaminaMultiplier = 0.1f,
                    BonusSpiritMultiplier = 0.1f
                }
            });
            #endregion

            #region Resistance
            defaultBuffs.Add(new Buff()
            {
                Name = "Shadow Protection",
                Source = "Priest",
                Group = "Resistance",
                ConflictingBuffs = new List<string>(new string[] { "Shadow Resistance Buff" }),
                Stats = { ShadowResistanceBuff = 130 }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Shadow Resistance Aura",
                Source = "Paladin",
                Group = "Resistance",
                ConflictingBuffs = new List<string>(new string[] { "Shadow Resistance Buff" }),
                Stats = { ShadowResistanceBuff = 130 }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Aspect of the Wild",
                Source = "Hunter",
                Group = "Resistance",
                ConflictingBuffs = new List<string>(new string[] { "Nature Resistance Buff" }),
                Stats = { NatureResistanceBuff = 130 }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Nature Resistance Totem",
                Source = "Shaman",
                Group = "Resistance",
                ConflictingBuffs = new List<string>(new string[] { "Nature Resistance Buff" }),
                Stats = { NatureResistanceBuff = 130 }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Frost Resistance Aura",
                Source = "Paladin",
                Group = "Resistance",
                ConflictingBuffs = new List<string>(new string[] { "Frost Resistance Buff" }),
                Stats = { FrostResistanceBuff = 130 }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Frost Resistance Totem",
                Source = "Shaman",
                Group = "Resistance",
                ConflictingBuffs = new List<string>(new string[] { "Frost Resistance Buff" }),
                Stats = { FrostResistanceBuff = 130 }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Fire Resistance Aura",
                Source = "Paladin",
                Group = "Resistance",
                ConflictingBuffs = new List<string>(new string[] { "Fire Resistance Buff" }),
                Stats = { FireResistanceBuff = 130 }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Fire Resistance Totem",
                Source = "Shaman",
                Group = "Resistance",
                ConflictingBuffs = new List<string>(new string[] { "Fire Resistance Buff" }),
                Stats = { FireResistanceBuff = 130 }
            });
            #endregion

            #region Pushback Protection
            defaultBuffs.Add(new Buff()
            {
                Name = "Concentration Aura",
                Source = "Paladin",
                Group = "Pushback Protection",
                Stats = { InterruptProtection = 0.35f },
                Improvements = { new Buff { Name = "Improved Concentration Aura", Stats = { InterruptProtection = 0.15f } } }
            });
            #endregion

            #region Class Buffs
            defaultBuffs.Add(new Buff()
            {
                Name = "Mage Armor",
                Group = "Class Buffs",
                Stats = { MageMageArmor = 1f },
                ConflictingBuffs = new List<string>(new string[] { "Mage Class Armor" })
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Molten Armor",
                Group = "Class Buffs",
                Stats = { MageMoltenArmor = 1f },
                ConflictingBuffs = new List<string>(new string[] { "Mage Class Armor" })
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Ice Armor",
                Group = "Class Buffs",
                Stats = { MageIceArmor = 1f },
                ConflictingBuffs = new List<string>(new string[] { "Mage Class Armor" })
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Fel Armor",
                Group = "Class Buffs",
                Stats = { WarlockFelArmor = 1f },
                ConflictingBuffs = new List<string>(new string[] { "Warlock Class Armor" })
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Demon Armor",
                Group = "Class Buffs",
                Stats = { WarlockDemonArmor = 1f },
                ConflictingBuffs = new List<string>(new string[] { "Warlock Class Armor" })
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Grand Spellstone",
                Group = "Class Buffs",
                Stats = { WarlockGrandSpellstone = 1f },
                ConflictingBuffs = new List<string>(new string[] { "Warlock Weapon Enchant" })
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Grand Firestone",
                Group = "Class Buffs",
                Stats = { WarlockGrandFirestone = 1f },
                ConflictingBuffs = new List<string>(new string[] { "Warlock Weapon Enchant" })
            });
            #endregion

            #region Racial Buffs
            defaultBuffs.Add(new Buff
            {
                Name = "Heroic Presence",
                Source = "Draenei", 
                Group = "Racial Buffs",
                Stats = { SpellHit = 0.01f, PhysicalHit = 0.01f }
            });
            #endregion

            #endregion

            #region Debuffs

            #region Armor (Major)
            defaultBuffs.Add(new Buff
            {
                Name = "Acid Spit",
                Source = "BM Hunter (Worm pet)",
                Group = "Armor (Major)",
                Stats = { ArmorPenetration = 0.2f }
            });
            defaultBuffs.Add(new Buff
            {
                Name = "Expose Armor",
                Source = "Rogue",
                Group = "Armor (Major)",
				Stats = { ArmorPenetration = 0.2f }
            });
            defaultBuffs.Add(new Buff
            {
                Name = "Sunder Armor",
                Source = "Warrior",
                Group = "Armor (Major)",
				Stats = { ArmorPenetration = 0.2f }
            });
            #endregion

            #region Armor (Minor)
            defaultBuffs.Add(new Buff
            {
                Name = "Curse of Recklessness",
                Source = "Warlock",
                Group = "Armor (Minor)",
                Stats = { ArmorPenetration = 0.05f }
            });
            defaultBuffs.Add(new Buff
            {
                Name = "Faerie Fire",
                Source = "Druid",
                Group = "Armor (Minor)",
				Stats = { ArmorPenetration = 0.05f }
            });
            defaultBuffs.Add(new Buff
            {
                Name = "Sting",
                Source = "BM Hunter (Wasp pet)",
                Group = "Armor (Minor)",
				Stats = { ArmorPenetration = 0.05f }
            });
            #endregion

            #region Ranged Attack Power
            defaultBuffs.Add(new Buff()
            {
                Name = "Hunter's Mark",
                Source = "Hunter",
                Group = "Ranged Attack Power",
                Stats = { RangedAttackPower = 300f },
                Improvements = { 
					new Buff { Name = "Improved Hunter's Mark", Stats = { RangedAttackPower = (float)Math.Floor(300f * 0.3f) } }
                }
            });
            #endregion

            #region Expose Weakness
            //defaultBuffs.Add(new Buff()
            //{
            //    Name = "Expose Weakness",
            //    Group = "Expose Weakness",
            //    Stats = { ExposeWeakness = 1 }
            //});
            #endregion

            #region Bleed Damage
            defaultBuffs.Add(new Buff
            {
                Name = "Mangle",
                Source = "Feral Druid",
                Group = "Bleed Damage",
                Stats = { BonusBleedDamageMultiplier = 0.3f }
            });
            defaultBuffs.Add(new Buff
            {
                Name = "Trauma",
                Source = "Arms Warrior",
                Group = "Bleed Damage",
                Stats = { BonusBleedDamageMultiplier = 0.3f }
            });
            #endregion

            #region Critical Strike Chance Taken
            defaultBuffs.Add(new Buff
            {
                Name = "Heart of the Crusader",
                Source = "Ret Paladin",
                Group = "Critical Strike Chance Taken",
                Stats = { PhysicalCrit = 0.03f, SpellCrit = 0.03f }
            });
            defaultBuffs.Add(new Buff
            {
                Name = "Master Poisoner",
                Source = "Assasin Rogue",
                Group = "Critical Strike Chance Taken",
                Stats = { PhysicalCrit = 0.03f, SpellCrit = 0.03f }
            });
            defaultBuffs.Add(new Buff
            {
                Name = "Totem of Wrath",
                Source = "Elem Shaman",
                Group = "Critical Strike Chance Taken",
                Stats = { PhysicalCrit = 0.03f, SpellCrit = 0.03f }
            });
            #endregion

            #region Mana Restore
            defaultBuffs.Add(new Buff
            {
                Name = "Judgement of Wisdom",
                Source = "Paladin",
                Group = "Mana Restore",
                Stats = { ManaRestoreFromBaseManaPerHit = 0.01f }
            });
            #endregion

            #region Melee Hit Chance Reduction
            defaultBuffs.Add(new Buff
            {
                Name = "Insect Swarm",
                Source = "Moonkin Druid",
                Group = "Melee Hit Chance Reduction",
                Stats = { Miss = 0.03f }
            });
            defaultBuffs.Add(new Buff
            {
                Name = "Scorpid Sting",
                Source = "Hunter",
                Group = "Melee Hit Chance Reduction",
                Stats = { Miss = 0.03f }
            });
            #endregion

            #region Physical Vulnerability
            defaultBuffs.Add(new Buff
            {
                Name = "Blood Frenzy",
                Source = "Arms Warrior",
                Group = "Physical Vulnerability",
                Stats = { BonusPhysicalDamageMultiplier = 0.04f }
            });
            defaultBuffs.Add(new Buff
            {
                Name = "Savage Combat",
                Source = "Combat Rogue",
                Group = "Physical Vulnerability",
                Stats = { BonusPhysicalDamageMultiplier = 0.04f }
            });
            #endregion

            #region Spell Critical Strike Chance
            defaultBuffs.Add(new Buff
            {
                Name = "Winter's Chill",
                Source = "Frost Mage",
                Group = "Spell Critical Strike Taken",
                Stats = { SpellCrit = 0.05f }
            });
            defaultBuffs.Add(new Buff
            {
                Name = "Improved Scorch",
                Source = "Fire Mage",
                Group = "Spell Critical Strike Taken",
                Stats = { SpellCrit = 0.05f }
            });
            defaultBuffs.Add(new Buff
            {
                Name = "Improved Shadow Bolt",
                Source = "Warlock",
                Group = "Spell Critical Strike Taken",
                Stats = { SpellCrit = 0.05f }
            });
            #endregion

            #region Spell Damage Taken
            defaultBuffs.Add(new Buff
            {
                Name = "Curse of the Elements",
                Source = "Warlock",
                Group = "Spell Damage Taken",
                Stats =
                {
                    BonusFireDamageMultiplier = 0.13f,
                    BonusFrostDamageMultiplier = 0.13f,
                    BonusArcaneDamageMultiplier = 0.13f,
                    BonusShadowDamageMultiplier = 0.13f,
                    BonusHolyDamageMultiplier = 0.13f,
                    BonusNatureDamageMultiplier = 0.13f
                }
            });
            defaultBuffs.Add(new Buff
            {
                Name = "Earth and Moon",
                Source = "Moonkin Druid",
                Group = "Spell Damage Taken",
                Stats =
                {
                    BonusFireDamageMultiplier = 0.13f,
                    BonusFrostDamageMultiplier = 0.13f,
                    BonusArcaneDamageMultiplier = 0.13f,
                    BonusShadowDamageMultiplier = 0.13f,
                    BonusNatureDamageMultiplier = 0.13f,
                    BonusHolyDamageMultiplier = 0.13f
                }
            });
            defaultBuffs.Add(new Buff
            {
                Name = "Ebon Plaguebringer",
                Source = "Unholy Death Knight",
                Group = "Spell Damage Taken",
                Stats =
                {
                    BonusFireDamageMultiplier = 0.13f,
                    BonusFrostDamageMultiplier = 0.13f,
                    BonusArcaneDamageMultiplier = 0.13f,
                    BonusShadowDamageMultiplier = 0.13f,
                    BonusHolyDamageMultiplier = 0.13f,
                    BonusNatureDamageMultiplier = 0.13f
                },
            });
            defaultBuffs.Add(new Buff
            {
                Name = "Rune of Razorice",
                Source = "Death Knight",
                Group = "Spell Damage Taken",
                ConflictingBuffs = new List<string>(),
                Stats =
                {
                    BonusFrostDamageMultiplier = 0.05f,
                }
            });

            #endregion

            #region Disease Damage Taken
            defaultBuffs.Add(new Buff
            {
                Name = "Crypt Fever",
                Source = "Unholy Death Knight",
                Group = "Disease Damage Taken",
                Stats = { BonusDiseaseDamageMultiplier = 0.3f }
            });
            #endregion

            #region Spell Hit Chance Taken
            defaultBuffs.Add(new Buff
            {
                Name = "Improved Faerie Fire",
                Source = "Moonkin Druid",
                Group = "Spell Hit Chance Taken",
                Stats = { SpellHit = 0.03f }
            });
            defaultBuffs.Add(new Buff
            {
                Name = "Misery",
                Source = "Shadow Priest",
                Group = "Spell Hit Chance Taken",
                Stats = { SpellHit = 0.03f }
            });
            #endregion

            #region Special Mobs
            defaultBuffs.Add(new Buff()
            {
                Name = "Dual Wielding Mob",
                Group = "Special Mobs",
                ConflictingBuffs = new List<string>(new string[] { "Dual Wielding Mob" }),
                Stats = { Miss = 0.2f }
            });
            #endregion

            #region Boss Attack Speed Reduction
            defaultBuffs.Add(new Buff()
            {
                Name = "Judgements of the Just",
                Group = "Boss Attack Speed",
                Stats = { BossAttackSpeedMultiplier = -0.2f }
            });

            defaultBuffs.Add(new Buff()
            {
                Name = "Infected Wounds",
                Group = "Boss Attack Speed",
                Stats = { BossAttackSpeedMultiplier = -0.2f }
            });

            defaultBuffs.Add(new Buff()
            {
                Name = "Frost Fever",
                Group = "Boss Attack Speed",
                Stats = { BossAttackSpeedMultiplier = -0.15f },
                Improvements = { new Buff { Name = "Improved Icy Touch", Stats = { BossAttackSpeedMultiplier = -0.06f } } }
            });

            defaultBuffs.Add(new Buff()
            {
                Name = "Thunder Clap",
                Group = "Boss Attack Speed",
                Stats = { BossAttackSpeedMultiplier = -0.10f },
                Improvements = { new Buff { Name = "Improved Thunder Clap", Stats = { BossAttackSpeedMultiplier = -0.06f } } }
            });
            #endregion

            #endregion

            #region Consumables

            #region Elixirs and Flasks
            defaultBuffs.Add(new Buff()
            {
                Name = "Flask of Fortification",
                Group = "Elixirs and Flasks",
                Stats = { Health = 500, DefenseRating = 10 },
                ConflictingBuffs = new List<string>(new string[] { "Battle Elixir", "Guardian Elixir" }),
                Improvements = { new Buff { Name = "Flask of Fortification (Mixology)", Stats = { 
					Health = 221, DefenseRating = 5 } } }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Flask of Chromatic Wonder",
                Group = "Elixirs and Flasks",
                Stats =
                {
                    Agility = 18,
                    Strength = 18,
                    Stamina = 18,
                    Intellect = 18,
                    Spirit = 18,
                    ArcaneResistance = 35,
                    FireResistance = 35,
                    FrostResistance = 35,
                    ShadowResistance = 35,
                    NatureResistance = 35
                },
                ConflictingBuffs = new List<string>(new string[] { "Battle Elixir", "Guardian Elixir" }),
                Improvements = { new Buff { Name = "Flask of Chromatic Wonder (Mixology)", Stats = {
					Agility = 4, Strength = 4, Stamina = 4, Intellect = 4, Spirit = 4, 
					ArcaneResistance = 4, FireResistance = 4, FrostResistance = 4, ShadowResistance = 4, NatureResistance = 4 } } }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Flask of Distilled Wisdom",
                Group = "Elixirs and Flasks",
                Stats = { Intellect = 65 },
                ConflictingBuffs = new List<string>(new string[] { "Battle Elixir", "Guardian Elixir" }),
                Improvements = { new Buff { Name = "Flask of Distilled Wisdom (Mixology)", Stats = { Intellect = 20 } } }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Flask of Endless Rage",
                Group = "Elixirs and Flasks",
                Stats = { AttackPower = 180 },
                ConflictingBuffs = new List<string>(new string[] { "Battle Elixir", "Guardian Elixir" }),
                Improvements = { new Buff { Name = "Flask of Endless Rage (Mixology)", Stats = { AttackPower = 64 } } }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Flask of Pure Mojo",
                Group = "Elixirs and Flasks",
                Stats = { Mp5 = 38 },
                ConflictingBuffs = new List<string>(new string[] { "Battle Elixir", "Guardian Elixir" }),
                Improvements = { new Buff { Name = "Flask of Pure Mojo (Mixology)", Stats = { Mp5 = 13 } } }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Flask of Stoneblood",
                Group = "Elixirs and Flasks",
                Stats = { Health = 1300 },
                ConflictingBuffs = new List<string>(new string[] { "Battle Elixir", "Guardian Elixir" }),
                Improvements = { new Buff { Name = "Flask of Stoneblood (Mixology)", Stats = { Health = 320 } } }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Flask of the Frost Wyrm",
                Group = "Elixirs and Flasks",
                Stats = { SpellPower = 125 },
                ConflictingBuffs = new List<string>(new string[] { "Battle Elixir", "Guardian Elixir" }),
                Improvements = { new Buff { Name = "Flask of the Frost Wyrm (Mixology)", Stats = { SpellPower = 37 } } }
            });

            defaultBuffs.Add(new Buff()
            {
                Name = "Elixir of Major Agility",
                Group = "Elixirs and Flasks",
                Stats = { Agility = 30, CritRating = 12 },
                ConflictingBuffs = new List<string>(new string[] { "Battle Elixir" }),
                Improvements = { new Buff { Name = "Elixir of Major Agility (Mixology)", Stats = { Agility = 10, CritRating = 4 } } }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Elixir of Demonslaying",
                Group = "Elixirs and Flasks",
                Stats = { AttackPower = 265 },
                ConflictingBuffs = new List<string>(new string[] { "Battle Elixir" }),
                Improvements = { new Buff { Name = "Elixir of Demonslaying (Mixology)", Stats = { AttackPower = 88 } } }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Adept's Elixir",
                Group = "Elixirs and Flasks",
                Stats = { SpellPower = 24, CritRating = 24 },
                ConflictingBuffs = new List<string>(new string[] { "Battle Elixir" }),
                Improvements = { new Buff { Name = "Adept's Elixir (Mixology)", Stats = { SpellPower = 9, CritRating = 9 } } }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Elixir of Healing Power",
                Group = "Elixirs and Flasks",
                Stats = { SpellPower = 24f, Spirit = 24 },
                ConflictingBuffs = new List<string>(new string[] { "Battle Elixir" }),
                Improvements = { new Buff { Name = "Elixir of Healing Power (Mixology)", Stats = { SpellPower = 9, Spirit = 9 } } }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Elixir of Accuracy",
                Group = "Elixirs and Flasks",
                Stats = { HitRating = 45 },
                ConflictingBuffs = new List<string>(new string[] { "Battle Elixir" }),
                Improvements = { new Buff { Name = "Elixir of Accuracy (Mixology)", Stats = { HitRating = 16 } } }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Elixir of Armor Piercing",
                Group = "Elixirs and Flasks",
                Stats = { ArmorPenetrationRating = 45 },
                ConflictingBuffs = new List<string>(new string[] { "Battle Elixir" }),
                Improvements = { new Buff { Name = "Elixir of Armor Piercing (Mixology)", Stats = { ArmorPenetrationRating = 16 } } }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Elixir of Deadly Strikes",
                Group = "Elixirs and Flasks",
                Stats = { CritRating = 45 },
                ConflictingBuffs = new List<string>(new string[] { "Battle Elixir" }),
                Improvements = { new Buff { Name = "Elixir of Deadly Strikes (Mixology)", Stats = { CritRating = 16 } } }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Elixir of Expertise",
                Group = "Elixirs and Flasks",
                Stats = { ExpertiseRating = 45 },
                ConflictingBuffs = new List<string>(new string[] { "Battle Elixir" }),
                Improvements = { new Buff { Name = "Elixir of Expertise (Mixology)", Stats = { ExpertiseRating = 16 } } }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Elixir of Lightning Speed",
                Group = "Elixirs and Flasks",
                Stats = { HasteRating = 45 },
                ConflictingBuffs = new List<string>(new string[] { "Battle Elixir" }),
                Improvements = { new Buff { Name = "Elixir of Lightning Speed (Mixology)", Stats = { HasteRating = 16 } } }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Elixir of Mighty Agility",
                Group = "Elixirs and Flasks",
                Stats = { Agility = 45 },
                ConflictingBuffs = new List<string>(new string[] { "Battle Elixir" }),
                Improvements = { new Buff { Name = "Elixir of Mighty Agility (Mixology)", Stats = { Agility = 16 } } }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Elixir of Mighty Strength",
                Group = "Elixirs and Flasks",
                Stats = { Strength = 50 },
                ConflictingBuffs = new List<string>(new string[] { "Battle Elixir" }),
                Improvements = { new Buff { Name = "Elixir of Mighty Strength (Mixology)", Stats = { Strength = 16 } } }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Guru's Elixir",
                Group = "Elixirs and Flasks",
                Stats = { Stamina = 20, Intellect = 20, Spirit = 20, Strength = 20, Agility = 20 },
                ConflictingBuffs = new List<string>(new string[] { "Battle Elixir" }),
                Improvements = { new Buff { Name = "Guru's Elixir (Mixology)", Stats = { 
					Stamina = 6, Intellect = 6, Spirit = 6, Strength = 6, Agility = 6} } }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Spellpower Elixir",
                Group = "Elixirs and Flasks",
                Stats = { SpellPower = 58 },
                ConflictingBuffs = new List<string>(new string[] { "Battle Elixir" }),
                Improvements = { new Buff { Name = "Spellpower Elixir (Mixology)", Stats = { SpellPower = 19 } } }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Wrath Elixir",
                Group = "Elixirs and Flasks",
                Stats = { AttackPower = 90 },
                ConflictingBuffs = new List<string>(new string[] { "Battle Elixir" }),
                Improvements = { new Buff { Name = "Wrath Elixir (Mixology)", Stats = { AttackPower = 32 } } }
            });

            defaultBuffs.Add(new Buff()
            {
                Name = "Elixir of Ironskin",
                Group = "Elixirs and Flasks",
                Stats = { Resilience = 30 },
                ConflictingBuffs = new List<string>(new string[] { "Guardian Elixir" }),
                Improvements = { new Buff { Name = "Elixir of Ironskin (Mixology)", Stats = { Resilience = 10 } } }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Elixir of Draenic Wisdom",
                Group = "Elixirs and Flasks",
                Stats = { Intellect = 30, Spirit = 30 },
                ConflictingBuffs = new List<string>(new string[] { "Guardian Elixir" }),
                Improvements = { new Buff { Name = "Elixir of Draenic Wisdom (Mixology)", Stats = { Intellect = 8, Spirit = 8 } } }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Elixir of Mighty Defense",
                Group = "Elixirs and Flasks",
                Stats = { DefenseRating = 45 },
                ConflictingBuffs = new List<string>(new string[] { "Guardian Elixir" }),
                Improvements = { new Buff { Name = "Elixir of Mighty Defense (Mixology)", Stats = { DefenseRating = 16 } } }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Elixir of Mighty Mageblood",
                Group = "Elixirs and Flasks",
                Stats = { Mp5 = 24 },
                ConflictingBuffs = new List<string>(new string[] { "Guardian Elixir" }),
                Improvements = { new Buff { Name = "Elixir of Mighty Mageblood (Mixology)", Stats = { Mp5 = 6 } } }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Elixir of Protection",
                Group = "Elixirs and Flasks",
                Stats = { BonusArmor = 800 },
                ConflictingBuffs = new List<string>(new string[] { "Guardian Elixir" }),
                Improvements = { new Buff { Name = "Elixir of Protection (Mixology)", Stats = { BonusArmor = 224 } } }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Elixir of Mighty Thoughts",
                Group = "Elixirs and Flasks",
                Stats = { Intellect = 45 },
                ConflictingBuffs = new List<string>(new string[] { "Guardian Elixir" }),
                Improvements = { new Buff { Name = "Elixir of Mighty Thoughts (Mixology)", Stats = { Intellect = 16 } } }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Elixir of Mighty Fortitude",
                Group = "Elixirs and Flasks",
                Stats = { Health = 350, Hp5 = 20 },
                ConflictingBuffs = new List<string>(new string[] { "Guardian Elixir" }),
                Improvements = { new Buff { Name = "Elixir of Mighty Fortitude (Mixology)", Stats = { Health = 116, Hp5 = 6 } } }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Elixir of Spirit",
                Group = "Elixirs and Flasks",
                Stats = { Spirit = 50 },
                ConflictingBuffs = new List<string>(new string[] { "Guardian Elixir" }),
                Improvements = { new Buff { Name = "Elixir of Spirit (Mixology)", Stats = { Spirit = 16 } } }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Elixir of Empowerment",
                Group = "Elixirs and Flasks",
                Stats = { SpellPenetration = 30 },
                ConflictingBuffs = new List<string>(new string[] { "Guardian Elixir" }),
                Improvements = { new Buff { Name = "Elixir of Empowerment (Mixology)", Stats = { SpellPenetration = 10 } } }
            });
            #endregion

            #region Potion
            //defaultBuffs.Add(new Buff()
            //{
            //    Name = "Haste Potion",
            //    Group = "Potion",
            //    Stats = { HasteRating = 50 },
            //});
            //defaultBuffs.Add(new Buff()
            //{
            //    Name = "Heroic Potion",
            //    Group = "Potion",
            //    Stats = { Health = 700 }
            //});
            //defaultBuffs.Add(new Buff()
            //{
            //    Name = "Ironshield Potion",
            //    Group = "Potion",
            //    Stats = { BonusArmor = 2500 }
            //});
            #endregion

            #region Food
            defaultBuffs.Add(new Buff()
            {
                Name = "Spirit Food",
                Group = "Food",
                Stats = { Stamina = 40, Spirit = 40 }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Agility Food",
                Group = "Food",
                Stats = { Agility = 40, Stamina = 40 }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Strength Food",
                Group = "Food",
                Stats = { Strength = 40, Stamina = 40 }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Armor Pen Food",
                Group = "Food",
                Stats = { ArmorPenetrationRating = 40, Stamina = 40 }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Expertise Food",
                Group = "Food",
                Stats = { ExpertiseRating = 40, Stamina = 40 }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Hit Food",
                Group = "Food",
                Stats = { HitRating = 40, Stamina = 40 }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Spell Power Food",
                Group = "Food",
                Stats = { SpellPower = 46, Stamina = 40 }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Haste Food",
                Group = "Food",
                Stats = { HasteRating = 40, Stamina = 40 }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Attack Power Food",
                Group = "Food",
                Stats = { AttackPower = 80, Stamina = 40 }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Strength Food",
                Group = "Food",
                Stats = { Strength = 40, Stamina = 40 }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Mp5 Food",
                Group = "Food",
                Stats = { Mp5 = 16, Stamina = 40 }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Crit Food",
                Group = "Food",
                Stats = { CritRating = 40, Stamina = 40 }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Fish Feast",
                Group = "Food",
                Stats = { AttackPower = 80, SpellPower = 46, Stamina = 40 }
            });
            #endregion

            #region Scrolls
            defaultBuffs.Add(new Buff()
            {
                Name = "Scroll of Agility VIII",
                Group = "Scrolls",
                ConflictingBuffs = new List<string>(new string[] { "Scroll of Agility", "Agility" }),
                Stats = { Agility = 30 }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Scroll of Strength VIII",
                Group = "Scrolls",
                ConflictingBuffs = new List<string>(new string[] { "Scroll of Strength", "Strength" }),
                Stats = { Strength = 30 }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Scroll of Intellect VIII",
                Group = "Scrolls",
                ConflictingBuffs = new List<string>(new string[] { "Scroll of Intellect", "Intellect" }),
                Stats = { Intellect = 48 }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Scroll of Stamina VIII",
                Group = "Scrolls",
                ConflictingBuffs = new List<string>(new string[] { "Scroll of Stamina", "Stamina" }),
                Stats = { Stamina = 132 }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Scroll of Spirit VIII",
                Group = "Scrolls",
                ConflictingBuffs = new List<string>(new string[] { "Scroll of Spirit", "Spirit" }),
                Stats = { Spirit = 64 }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Scroll of Protection VII",
                Group = "Scrolls",
                ConflictingBuffs = new List<string>(new string[] { "Scroll of Protection", "StatArmor" }),
                Stats = { BonusArmor = 340 }
            });
            #endregion

            #region Temporary Weapon Enchantment
			//defaultBuffs.Add(new Buff()
			//{
			//    Name = "Adamantite Weightstone",
			//    Group = "Temporary Weapon Enchantment",
			//    Stats = { WeaponDamage = 12, CritRating = 14 }
			//});
            defaultBuffs.Add(new Buff()
            {
                Name = "Earthliving Weapon",
                Group = "Temporary Weapon Enchantment",
                Stats = { Earthliving = 1 }
            });
            //defaultBuffs.Add(new Buff()
            //{
            //    Name = "Righteous Weapon Coating",
            //    Group = "Temporary Weapon Enchantment",
            //    Stats = { AttackPower = 60 }
            //});
            //defaultBuffs.Add(new Buff()
            //{
            //    Name = "Elemental Sharpening Stone",
            //    Group = "Temporary Weapon Enchantment",
            //    Stats = { CritRating = 28 }
            //});
            //defaultBuffs.Add(new Buff()
            //{
            //    Name = "Superior Wizard Oil",
            //    Group = "Temporary Weapon Enchantment",
            //    Stats = { SpellPower = 42 }
            //});
            //defaultBuffs.Add(new Buff()
            //{
            //    Name = "Brilliant Wizard Oil",
            //    Group = "Temporary Weapon Enchantment",
            //    Stats = { SpellPower = 36, CritRating = 14 }
            //});
            //defaultBuffs.Add(new Buff()
            //{
            //    Name = "Superior Mana Oil",
            //    Group = "Temporary Weapon Enchantment",
            //    Stats = { Mp5 = 14 }
            //});
            //defaultBuffs.Add(new Buff()
            //{
            //    Name = "Brilliant Mana Oil",
            //    Group = "Temporary Weapon Enchantment",
            //    Stats = { Mp5 = 12, SpellPower = 13 }
            //});
            #endregion

            #endregion

            #region Set Bonuses
            defaultBuffs.Add(new Buff()
            {
                Name = "Malorne Harness 2 Piece Bonus",
                Group = "Set Bonuses",
                ConflictingBuffs = new List<string>(new string[] { }),
                Stats = { BloodlustProc = 0.8f },
                SetName = "Malorne Harness",
                SetThreshold = 2
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Malorne Harness 4 Piece Bonus",
                Group = "Set Bonuses",
                ConflictingBuffs = new List<string>(new string[] { }),
                Stats = { BonusArmor = 1400, CatFormStrength = 30 },
                SetName = "Malorne Harness",
                SetThreshold = 4
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Nordrassil Harness 4 Piece Bonus",
                Group = "Set Bonuses",
                ConflictingBuffs = new List<string>(new string[] { }),
                Stats = { BonusShredDamage = 75/*, BonusLacerateDamage = 15/5*/},
                SetName = "Nordrassil Harness",
                SetThreshold = 4
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Thunderheart Harness 2 Piece Bonus",
                Group = "Set Bonuses",
                ConflictingBuffs = new List<string>(new string[] { }),
                Stats = { MangleCatCostReduction = 5, BonusMangleBearThreat = 0.15f },
                SetName = "Thunderheart Harness",
                SetThreshold = 2
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Thunderheart Harness 4 Piece Bonus",
                Group = "Set Bonuses",
                ConflictingBuffs = new List<string>(new string[] { }),
                Stats = { BonusRipDamageMultiplier = .15f, BonusFerociousBiteDamageMultiplier = .15f, BonusSwipeDamageMultiplier = .15f },
                SetName = "Thunderheart Harness",
                SetThreshold = 4
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Gladiator Sanctuary 2 Piece Bonus",
                Group = "Set Bonuses",
                ConflictingBuffs = new List<string>(new string[] { }),
                Stats = { Resilience = 35 },
                SetName = "Gladiator's Sanctuary",
                SetThreshold = 2
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Gladiator's Regalia 2 Piece Bonus",
                Group = "Set Bonuses",
                ConflictingBuffs = new List<string>(new string[] { }),
                Stats = { Resilience = 35 },
                SetName = "Gladiator's Regalia",
                SetThreshold = 2
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Aldor Regalia 2 Piece Bonus",
                Group = "Set Bonuses",
                ConflictingBuffs = new List<string>(new string[] { }),
                Stats = { AldorRegaliaInterruptProtection = 1 },
                SetName = "Aldor Regalia",
                SetThreshold = 2
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Tirisfal Regalia 2 Piece Bonus",
                Group = "Set Bonuses",
                ConflictingBuffs = new List<string>(new string[] { }),
                Stats = { ArcaneBlastBonus = .05f },
                SetName = "Tirisfal Regalia",
                SetThreshold = 2
            });
            defaultBuffs.Add(buff = new Buff()
            {
                Name = "Tirisfal Regalia 4 Piece Bonus",
                Group = "Set Bonuses",
                ConflictingBuffs = new List<string>(new string[] { }),
                Stats = new Stats(),
                SetName = "Tirisfal Regalia",
                SetThreshold = 4
            });
            buff.Stats.AddSpecialEffect(new SpecialEffect(Trigger.SpellCrit, new Stats() { SpellPower = 70f }, 6.0f, 0.0f));
            defaultBuffs.Add(new Buff()
            {
                Name = "Tempest Regalia 2 Piece Bonus",
                Group = "Set Bonuses",
                ConflictingBuffs = new List<string>(new string[] { }),
                Stats = { EvocationExtension = 2f },
                SetName = "Tempest Regalia",
                SetThreshold = 2
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Tempest Regalia 4 Piece Bonus",
                Group = "Set Bonuses",
                ConflictingBuffs = new List<string>(new string[] { }),
                Stats = { BonusMageNukeMultiplier = 0.05f },
                SetName = "Tempest Regalia",
                SetThreshold = 4
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Voidheart Raiment 2 Piece Bonus",
                Group = "Set Bonuses",
                ConflictingBuffs = new List<string>(new string[] { }),
                Stats = { BonusWarlockSchoolDamageOnCast = 135 },
                SetName = "Voidheart Raiment",
                SetThreshold = 2
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Voidheart Raiment 4 Piece Bonus",
                Group = "Set Bonuses",
                ConflictingBuffs = new List<string>(new string[] { }),
                Stats = { BonusWarlockDotExtension = 3 },
                SetName = "Voidheart Raiment",
                SetThreshold = 4
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Corruptor Raiment 4 Piece Bonus",
                Group = "Set Bonuses",
                ConflictingBuffs = new List<string>(new string[] { }),
                Stats = { BonusWarlockDotDamageMultiplier = 0.1f },
                SetName = "Corruptor Raiment",
                SetThreshold = 4
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Malefic Raiment 4 Piece Bonus",
                Group = "Set Bonuses",
                ConflictingBuffs = new List<string>(new string[] { }),
                Stats = { BonusWarlockNukeMultiplier = 0.06f },
                SetName = "Malefic Raiment",
                SetThreshold = 4
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Plagueheart Garb 2 Piece Bonus",
                Group = "Set Bonuses",
                ConflictingBuffs = new List<string>(new string[] { }),
                Stats = { CorruptionTriggersCrit = 0.15f },
                SetName = "Plagueheart Garb",
                SetThreshold = 2
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Plagueheart Garb 4 Piece Bonus",
                Group = "Set Bonuses",
                ConflictingBuffs = new List<string>(new string[] { }),
                Stats = { LifeTapBonusSpirit = 300f },
                SetName = "Plagueheart Garb",
                SetThreshold = 4
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Deathbringer Garb 2 Piece Bonus",
                Group = "Set Bonuses",
                ConflictingBuffs = new List<string>(new string[] { }),
                Stats = { Warlock2T8 = 0.2f },
                SetName = "Deathbringer Garb",
                SetThreshold = 2
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Deathbringer Garb 4 Piece Bonus",
                Group = "Set Bonuses",
                ConflictingBuffs = new List<string>(new string[] { }),
                Stats = { Warlock4T8 = 0.05f },
                SetName = "Deathbringer Garb",
                SetThreshold = 4
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Spellfire 3 Piece Bonus",
                Group = "Set Bonuses",
                ConflictingBuffs = new List<string>(new string[] { }),
                Stats = { SpellDamageFromIntellectPercentage = 0.07f },
                SetName = "Wrath of Spellfire",
                SetThreshold = 3
            });
            defaultBuffs.Add(buff = new Buff()
            {
                Name = "Spellstrike 2 Piece Bonus",
                Group = "Set Bonuses",
                ConflictingBuffs = new List<string>(new string[] { }),
                Stats = { SpellDamageFor10SecOnHit_5 = 92 },
                SetName = "Spellstrike Infusion",
                SetThreshold = 2
            });
            buff.Stats.AddSpecialEffect(new SpecialEffect(Trigger.SpellHit, new Stats() { SpellPower = 92 }, 10, 0, 0.05f));
            defaultBuffs.Add(new Buff()
            {
                Name = "Lightbringer Raiment 4 Piece",
                Group = "Set Bonuses",
                ConflictingBuffs = new List<string>(new string[] { }),
                Stats = { HolyLightCrit = .05f },
                SetName = "Lightbringer Raiment",
                SetThreshold = 4
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Lightbringer Raiment 2 Piece",
                Group = "Set Bonuses",
                ConflictingBuffs = new List<string>(new string[] { }),
                Stats = { FlashOfLightMultiplier = .05f },
                SetName = "Lightbringer Raiment",
                SetThreshold = 2
            });
            // Resto druid tier 4/5/6 sets
            defaultBuffs.Add( buff = new Buff()
            {
                Name = "Malorne Raiment 2 Piece",
                Group = "Set Bonuses",
                ConflictingBuffs = new List<string>(new string[] { }),
                Stats = { ManaRestoreOnCast_5_15 = 120 },
                SetName = "Malorne Raiment",
                SetThreshold = 2
            });
            buff.Stats.AddSpecialEffect(new SpecialEffect(Trigger.HealingSpellCast, new Stats() { ManaRestore = 120f }, 0f, 0f, 0.05f));
            defaultBuffs.Add(new Buff()
            {
                Name = "Nordrassil Raiment 2 Piece",
                Group = "Set Bonuses",
                ConflictingBuffs = new List<string>(new string[] { }),
                Stats = { RegrowthExtraTicks = 2 },
                SetName = "Nordrassil Raiment",
                SetThreshold = 2
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Nordrassil Raiment 4 Piece",
                Group = "Set Bonuses",
                ConflictingBuffs = new List<string>(new string[] { }),
                Stats = { LifebloomFinalHealBonus = 150 },
                SetName = "Nordrassil Raiment",
                SetThreshold = 4
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Thunderheart Raiment 4 Piece",
                Group = "Set Bonuses",
                ConflictingBuffs = new List<string>(new string[] { }),
                Stats = { BonusHealingTouchMultiplier = 0.05f },
                SetName = "Thunderheart Raiment",
                SetThreshold = 4
            });
            defaultBuffs.Add(buff = new Buff()
            {
                Name = "Frostfire Garb 2 Piece",
                Group = "Set Bonuses",
                ConflictingBuffs = new List<string>(new string[] { }),
                Stats = { BonusManaGem = 0.4f },
                SetName = "Frostfire Garb",
                SetThreshold = 2
            });
            buff.Stats.AddSpecialEffect(new SpecialEffect(Trigger.ManaGem, new Stats() { SpellPower = 225f }, 15f, 0f));
            defaultBuffs.Add(new Buff()
            {
                Name = "Frostfire Garb 4 Piece",
                Group = "Set Bonuses",
                ConflictingBuffs = new List<string>(new string[] { }),
                Stats = { CritBonusDamage = 0.05f },
                SetName = "Frostfire Garb",
                SetThreshold = 4
            });
            defaultBuffs.Add(buff = new Buff()
            {
                Name = "Kirin Tor Garb 2 Piece",
                Group = "Set Bonuses",
                ConflictingBuffs = new List<string>(new string[] { }),
                Stats = { },
                SetName = "Kirin Tor Garb",
                SetThreshold = 2
            });
            buff.Stats.AddSpecialEffect(new SpecialEffect(Trigger.MageNukeCast, new Stats() { SpellPower = 350f }, 15f, 45f, 0.25f));
            defaultBuffs.Add(new Buff()
            {
                Name = "Kirin Tor Garb 4 Piece",
                Group = "Set Bonuses",
                ConflictingBuffs = new List<string>(new string[] { }),
                Stats = { Mage4T8 = 1 },
                SetName = "Kirin Tor Garb",
                SetThreshold = 4
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Duskweaver 2 Piece",
                Group = "Set Bonuses",
                ConflictingBuffs = new List<string>(new string[] { }),
                Stats = { SpellPower = 18f },
                SetName = "Duskweaver",
                SetThreshold = 2
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Duskweaver 4 Piece",
                Group = "Set Bonuses",
                ConflictingBuffs = new List<string>(new string[] { }),
                Stats = { Stamina = 25f },
                SetName = "Duskweaver",
                SetThreshold = 4
            });


            // Windhawk (epic leather caster) set
            defaultBuffs.Add(new Buff()
            {
                Name = "Windhawk Armor",
                Group = "Set Bonuses",
                ConflictingBuffs = new List<string>(new string[] { }),
                Stats = { Mp5 = 8.0f },
                SetName = "Windhawk Armor",
                SetThreshold = 3
            });
            // Moonkin tier 4/5/6 sets
            defaultBuffs.Add(new Buff()
            {
                Name = "Malorne Regalia 2 Piece Bonus",
                Group = "Set Bonuses",
                ConflictingBuffs = new List<string>(new string[] { }),
                Stats = { ManaRestorePerCast = .05f * 120 },
                SetName = "Malorne Regalia",
                SetThreshold = 2
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Malorne Regalia 4 Piece Bonus",
                Group = "Set Bonuses",
                ConflictingBuffs = new List<string>(new string[] { }),
                Stats = { InnervateCooldownReduction = 48.0f },
                SetName = "Malorne Regalia",
                SetThreshold = 4
            });
            // Nordrassil 2-piece skipped because it has nothing to do with dps
            defaultBuffs.Add(new Buff()
            {
                Name = "Nordrassil Regalia 4 Piece Bonus",
                Group = "Set Bonuses",
                ConflictingBuffs = new List<string>(new string[] { }),
                Stats = { StarfireBonusWithDot = 0.1f },
                SetName = "Nordrassil Regalia",
                SetThreshold = 4
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Thunderheart Regalia 2 Piece Bonus",
                Group = "Set Bonuses",
                ConflictingBuffs = new List<string>(new string[] { }),
                Stats = { MoonfireExtension = 3.0f },
                SetName = "Thunderheart Regalia",
                SetThreshold = 2
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Thunderheart Regalia 4 Piece Bonus",
                Group = "Set Bonuses",
                ConflictingBuffs = new List<string>(new string[] { }),
                Stats = { StarfireCritChance = 0.05f },
                SetName = "Thunderheart Regalia",
                SetThreshold = 4
            });
            // Feral Tier 7 set bonuses
            defaultBuffs.Add(new Buff()
            {
                Name = "Dreamwalker Battlegear 2 Piece Bonus",
                Group = "Set Bonuses",
                ConflictingBuffs = new List<string>(new string[] { }),
                Stats = { BonusRipDuration = 4f, BonusLacerateDamageMultiplier = 0.05f },
                SetName = "Dreamwalker Battlegear",
                SetThreshold = 2
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Dreamwalker Battlegear 4 Piece Bonus",
                Group = "Set Bonuses",
                ConflictingBuffs = new List<string>(new string[] { }),
                Stats = { TigersFuryCooldownReduction = 3f, WeaponDamage = 1.778f /*Increased Barkskin Duration*/ },
                SetName = "Dreamwalker Battlegear",
                SetThreshold = 4
			});
			// Feral Tier 8 set bonuses 
			defaultBuffs.Add(new Buff() //TODO TODO TODO TODO
			{
			    Name = "Nightsong Battlegear 2 Piece Bonus",
			    Group = "Set Bonuses",
			    ConflictingBuffs = new List<string>(new string[] { }),
			    Stats = { ClearcastOnBleedChance = 0.02f },
				SetName = "Nightsong Battlegear",
			    SetThreshold = 2
			});
			defaultBuffs.Add(new Buff() //TODO TODO TODO TODO
			{
				Name = "Nightsong Battlegear 4 Piece Bonus",
			    Group = "Set Bonuses",
			    ConflictingBuffs = new List<string>(new string[] { }),
			    Stats = { BonusSavageRoarDuration = 8f },
				SetName = "Nightsong Battlegear",
			    SetThreshold = 4
			});
            // Moonkin Tier 7 set bonuses
            defaultBuffs.Add(new Buff()
            {
                Name = "Dreamwalker Garb 2 Piece Bonus",
                Group = "Set Bonuses",
                ConflictingBuffs = new List<string>(new string[] { }),
                Stats = { BonusInsectSwarmDamage = 0.1f },
                SetName = "Dreamwalker Garb",
                SetThreshold = 2
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Dreamwalker Garb 4 Piece Bonus",
                Group = "Set Bonuses",
                ConflictingBuffs = new List<string>(new string[] { }),
                Stats = { BonusNukeCritChance = 0.05f },
                SetName = "Dreamwalker Garb",
                SetThreshold = 4
            });
            // Moonkin Tier 8 set bonuses
            defaultBuffs.Add(new Buff()
            {
                Name = "Nightsong Garb 2 Piece Bonus",
                Group = "Set Bonuses",
                ConflictingBuffs = new List<string>(new string[] { }),
                Stats = { EclipseBonus = 0.15f },
                SetName = "Nightsong Garb",
                SetThreshold = 2
            });
            defaultBuffs.Add(buff = new Buff()
            {
                Name = "Nightsong Garb 4 Piece Bonus",
                Group = "Set Bonuses",
                ConflictingBuffs = new List<string>(new string[] { }),
                Stats = new Stats(),
                SetName = "Nightsong Garb",
                SetThreshold = 4
            });
            buff.Stats.AddSpecialEffect(new SpecialEffect(Trigger.InsectSwarmTick, new Stats() { StarfireProc = 1f }, 0f, 0f, 0.15f, 1));
            // Tree Tier 7 set bonuses
            defaultBuffs.Add(new Buff()
            {
                Name = "Dreamwalker Regalia 2 Piece Bonus",
                Group = "Set Bonuses",
                ConflictingBuffs = new List<string>(new string[] { }),
                Stats = { LifebloomCostReduction = 0.05f },
                SetName = "Dreamwalker Regalia",
                SetThreshold = 2
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Dreamwalker Regalia 4 Piece Bonus",
                Group = "Set Bonuses",
                ConflictingBuffs = new List<string>(new string[] { }),
                Stats = { NourishBonusPerHoT = 0.05f },
                SetName = "Dreamwalker Regalia",
                SetThreshold = 4
            });
            // Tree Tier 8 set bonuses
            defaultBuffs.Add(new Buff()
            {
                Name = "Nightsong Regalia 4 Piece Bonus",
                Group = "Set Bonuses",
                ConflictingBuffs = new List<string>(new string[] { }),
                Stats = { RejuvenationInstantTick = 1.0f },
                SetName = "Nightsong Regalia",
                SetThreshold = 4
            });

            defaultBuffs.Add(new Buff()
            {
                Name = "Destroyer Armor 2 Piece Bonus",
                Group = "Set Bonuses",
                ConflictingBuffs = new List<string>(new string[] { }),
                Stats = { BlockValue = 100f / 2f },
                SetName = "Destroyer Armor",
                SetThreshold = 2
            });
            defaultBuffs.Add(new Buff()
            {
                //This can vary depending on how many mobs are being tanked and your
                //avoidance, for now assume it procs once every 30 seconds.
                //200 haste for 10 sec every 10 sec = 200 / 3 = 67 haste rating
                Name = "Destroyer Armor 4 Piece Bonus",
                Group = "Set Bonuses",
                ConflictingBuffs = new List<string>(new string[] { }),
                Stats = { HasteRating = 67 },
                SetName = "Destroyer Armor",
                SetThreshold = 4
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Onslaught Armor 2 Piece Bonus",
                Group = "Set Bonuses",
                ConflictingBuffs = new List<string>(new string[] { }),
                Stats = { BonusCommandingShoutHP = 170f },
                SetName = "Onslaught Armor",
                SetThreshold = 2
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Onslaught Armor 4 Piece Bonus",
                Group = "Set Bonuses",
                ConflictingBuffs = new List<string>(new string[] { }),
                Stats = { BonusShieldSlamDamage = 0.1f },
                SetName = "Onslaught Armor",
                SetThreshold = 4
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Dreadnaught Battlegear 2 Piece Bonus",
                Group = "Set Bonuses",
                ConflictingBuffs = new List<string>(new string[] { }),
                Stats = { BonusSlamDamage = 0.1f },
                SetName = "Dreadnaught Battlegear",
                SetThreshold = 2
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Dreadnaught Battlegear 4 Piece Bonus",
                Group = "Set Bonuses",
                ConflictingBuffs = new List<string>(new string[] { }),
                Stats = { DreadnaughtBonusRageProc = 5f },
                SetName = "Dreadnaught Battlegear",
                SetThreshold = 4
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Dreadnaught Plate 2 Piece Bonus",
                Group = "Set Bonuses",
                ConflictingBuffs = new List<string>(new string[] { }),
                Stats = { BonusShieldSlamDamage = 0.1f },
                SetName = "Dreadnaught Plate",
                SetThreshold = 2
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Siegebreaker Plate 2 Piece Bonus",
                Group = "Set Bonuses",
                ConflictingBuffs = new List<string>(new string[] { }),
                Stats = { DevastateCritIncrease = 0.1f },
                SetName = "Siegebreaker Plate",
                SetThreshold = 2
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Primalstrike 3 Piece Bonus",
                Group = "Set Bonuses",
                ConflictingBuffs = new List<string>(new string[] { }),
                Stats = { AttackPower = 40 },
                SetName = "Primal Intent",
                SetThreshold = 3
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Fel Leather 3 Piece Bonus",
                Group = "Set Bonuses",
                ConflictingBuffs = new List<string>(new string[] { }),
                Stats = { DodgeRating = 20 },
                SetName = "Fel Skin",
                SetThreshold = 3
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Heavy Clefthoof 3 Piece Bonus",
                Group = "Set Bonuses",
                ConflictingBuffs = new List<string>(new string[] { }),
                Stats = { Strength = 20 },
                SetName = "Strength of the Clefthoof",
                SetThreshold = 3
            });

            // Resto Shammy set bonuses:  Removed BC sets as we push to Ulduar
            /*defaultBuffs.Add(new Buff()
            {
                Name = "Cyclone Raiment 2 Piece Bonus",
                Group = "Set Bonuses",
                ConflictingBuffs = new List<string>(new string[] { }),
                SetName = "Cyclone Raiment",
                Stats = { ManaSpringMp5Increase = 7.5f },
                SetThreshold = 2
            });

            defaultBuffs.Add(new Buff()
            {
                Name = "Cataclysm Raiment 2 Piece Bonus",
                Group = "Set Bonuses",
                ConflictingBuffs = new List<string>(new string[] { }),
                SetName = "Cataclysm Raiment",
                Stats = { LHWManaReduction = .05f },
                SetThreshold = 2
            });

            defaultBuffs.Add(new Buff()
            {
                Name = "Skyshatter Raiment 2 Piece Bonus",
                Group = "Set Bonuses",
                ConflictingBuffs = new List<string>(new string[] { }),
                SetName = "Skyshatter Raiment",
                Stats = { CHManaReduction = .1f },
                SetThreshold = 2
            });

            defaultBuffs.Add(new Buff()
            {
                Name = "Skyshatter Raiment 4 Piece Bonus",
                Group = "Set Bonuses",
                ConflictingBuffs = new List<string>(new string[] { }),
                SetName = "Skyshatter Raiment",
                Stats = { CHHealIncrease = .05f },
                SetThreshold = 4
            });*/
            // Tier 7
            defaultBuffs.Add(new Buff()
            {
                Name = "Earthshatter Regalia 2 Piece Bonus",
                Group = "Set Bonuses",
                ConflictingBuffs = new List<string>(new string[] { }),
                SetName = "Earthshatter Regalia",
                Stats = { WaterShieldIncrease = .1f },
                SetThreshold = 2
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Earthshatter Regalia 4 Piece Bonus",
                Group = "Set Bonuses",
                ConflictingBuffs = new List<string>(new string[] { }),
                SetName = "Earthshatter Regalia",
                Stats = { CHHWHealIncrease = .05f },
                SetThreshold = 4
            });
            // Tier 8
            defaultBuffs.Add(new Buff()
            {
                Name = "Worldbreaker Regalia 2 Piece Bonus",
                Group = "Set Bonuses",
                ConflictingBuffs = new List<string>(new string[] { }),
                SetName = "Worldbreaker Regalia",
                Stats = { RTCDDecrease = 1f },
                SetThreshold = 2
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Worldbreaker Regalia 4 Piece Bonus",
                Group = "Set Bonuses",
                ConflictingBuffs = new List<string>(new string[] { }),
                SetName = "Worldbreaker Regalia",
                Stats = { CHCTDecrease = .2f },
                SetThreshold = 4
            });

            // Elemental Shaman set bonuses:
            //Tier 6																																																																																		
            defaultBuffs.Add(new Buff()
            {
                Name = "Skyshatter Regalia 2 Piece Bonus",
                Group = "Set Bonuses",
                ConflictingBuffs = new List<string>(new string[] { }),
                SetName = "Skyshatter Regalia",
                Stats = { Mp5 = 15f, CritRating = 35f, SpellPower = 45f },
                SetThreshold = 2
            });

            defaultBuffs.Add(new Buff()
            {
                Name = "Skyshatter Regalia 4 Piece Bonus",
                Group = "Set Bonuses",
                ConflictingBuffs = new List<string>(new string[] { }),
                SetName = "Skyshatter Regalia",
                Stats = { LightningBoltDamageModifier = 5f },
                SetThreshold = 4
            });
            //Tier 7																																																																																				
            defaultBuffs.Add(new Buff()
            {
                Name = "Earthshatter Garb 2 Piece Bonus",
                Group = "Set Bonuses",
                ConflictingBuffs = new List<string>(new string[] { }),
                SetName = "Earthshatter Garb",
                Stats = { LightningBoltCostReduction = 5f },
                SetThreshold = 2
            });

            defaultBuffs.Add(new Buff()
            {
                Name = "Earthshatter Garb 4 Piece Bonus",
                Group = "Set Bonuses",
                ConflictingBuffs = new List<string>(new string[] { }),
                SetName = "Earthshatter Garb",
                Stats = { BonusLavaBurstCritDamage = 10f },
                SetThreshold = 4
            });

            //Enhancement Shaman Tier 7 Set Bonuses
            
            defaultBuffs.Add(new Buff()
            {
                Name = "Earthshatter Battlegear 2 Piece Bonus",
                Group = "Set Bonuses",
                ConflictingBuffs = new List<string>(new string[] { }),
                SetName = "Earthshatter Battlegear",
                Stats = { BonusLSDamage = 0.1f },
                SetThreshold = 2
            });

            defaultBuffs.Add(new Buff()
            {
                Name = "Earthshatter Battlegear 4 Piece Bonus",
                Group = "Set Bonuses",
                ConflictingBuffs = new List<string>(new string[] { }),
                SetName = "Earthshatter Battlegear",
                Stats = { BonusFlurryHaste = 0.05f },
                SetThreshold = 4
            });

            //Enhancement Shaman Tier 8 Set Bonuses

            defaultBuffs.Add(new Buff()
            {
                Name = "Worldbreaker Battlegear 2 Piece Bonus",
                Group = "Set Bonuses",
                ConflictingBuffs = new List<string>(new string[] { }),
                SetName = "Worldbreaker Battlegear",
                Stats = { BonusLLSSDamage = 0.2f },
                SetThreshold = 2
            });

            defaultBuffs.Add(new Buff()
            {
                Name = "Worldbreaker Battlegear 4 Piece Bonus",
                Group = "Set Bonuses",
                ConflictingBuffs = new List<string>(new string[] { }),
                SetName = "Worldbreaker Battlegear",
                Stats = { BonusMWFreq = 0.2f },
                SetThreshold = 4
            });

            //Hunter Set Bonuses
            defaultBuffs.Add(new Buff()
            {
                Name = "Rift Stalker Armor 4 Piece Bonus",
                Group = "Set Bonuses",
                ConflictingBuffs = new List<string>(new string[] { }),
                SetName = "Rift Stalker Armor",
                Stats = { BonusSteadyShotCrit = .05f },
                SetThreshold = 4
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Gronnstalker's Armor 4 Piece Bonus",
                Group = "Set Bonuses",
                ConflictingBuffs = new List<string>(new string[] { }),
                SetName = "Gronnstalker's Armor",
                Stats = { BonusSteadyShotDamageMultiplier = .1f },
                SetThreshold = 4
            });

            // Holy Priest bonuses
            defaultBuffs.Add(new Buff()
            {
                Name = "Primal Mooncloth 3 Piece Bonus",
                Group = "Set Bonuses",
                ConflictingBuffs = new List<string>(new string[] { }),
                Stats = { SpellCombatManaRegeneration = 0.05f },
                SetName = "Primal Mooncloth",
                SetThreshold = 3
            });

            defaultBuffs.Add(new Buff()
            {
                Name = "Vestments of Absolution 2 Piece Bonus",
                Group = "Set Bonuses",
                ConflictingBuffs = new List<string>(new string[] { }),
                Stats = { BonusPoHManaCostReductionMultiplier = 0.1f },
                SetName = "Vestments of Absolution",
                SetThreshold = 2
            });

            defaultBuffs.Add(new Buff()
            {
                Name = "Vestments of Absolution 4 Piece Bonus",
                Group = "Set Bonuses",
                ConflictingBuffs = new List<string>(new string[] { }),
                Stats = { BonusGHHealingMultiplier = 0.05f },
                SetName = "Vestments of Absolution",
                SetThreshold = 4
            });

            defaultBuffs.Add(new Buff()
            {
                Name = "Absolution Regalia 2 Piece Bonus",
                Group = "Set Bonuses",
                ConflictingBuffs = new List<string>(new string[] { }),
                Stats = { SWPDurationIncrease = 3f },
                SetName = "Absolution Regalia",
                SetThreshold = 2
            });

            defaultBuffs.Add(new Buff()
            {
                Name = "Absolution Regalia 4 Piece Bonus",
                Group = "Set Bonuses",
                ConflictingBuffs = new List<string>(new string[] { }),
                Stats = { BonusMindBlastMultiplier = 0.1f },
                SetName = "Absolution Regalia",
                SetThreshold = 4
            });

            defaultBuffs.Add(new Buff()
            {
                Name = "Garb of Faith 2 Piece Bonus",
                Group = "Set Bonuses",
                ConflictingBuffs = new List<string>(new string[] { }),
                Stats = { MindBlastCostReduction = 0.1f },
                SetName = "Garb of Faith",
                SetThreshold = 2
            });

            defaultBuffs.Add(new Buff()
            {
                Name = "Garb of Faith 4 Piece Bonus",
                Group = "Set Bonuses",
                ConflictingBuffs = new List<string>(new string[] { }),
                Stats = { ShadowWordDeathCritIncrease = 0.1f },
                SetName = "Garb of Faith",
                SetThreshold = 4
            });

            defaultBuffs.Add(new Buff()
            {
                Name = "Avatar Raiment 2 Piece Bonus",
                Group = "Set Bonuses",
                ConflictingBuffs = new List<string>(new string[] { }),
                Stats = { ManaGainOnGreaterHealOverheal = 100f },
                SetName = "Avatar Raiment",
                SetThreshold = 2
            });

            defaultBuffs.Add(new Buff()
            {
                Name = "Avatar Raiment 4 Piece Bonus",
                Group = "Set Bonuses",
                ConflictingBuffs = new List<string>(new string[] { }),
                Stats = { RenewDurationIncrease = 3f },
                SetName = "Avatar Raiment",
                SetThreshold = 4
            });

            defaultBuffs.Add(new Buff()
            {
                Name = "Regalia of Faith 2 Piece Bonus",
                Group = "Set Bonuses",
                ConflictingBuffs = new List<string>(new string[] { }),
                Stats = { PrayerOfMendingExtraJumps = 1 },
                SetName = "Regalia of Faith",
                SetThreshold = 2
            });

            defaultBuffs.Add(new Buff()
            {
                Name = "Regalia of Faith 4 Piece Bonus",
                Group = "Set Bonuses",
                ConflictingBuffs = new List<string>(new string[] { }),
                Stats = { GreaterHealCostReduction = 0.05f },
                SetName = "Regalia of Faith",
                SetThreshold = 4
            });

            defaultBuffs.Add(new Buff()
            {
                Name = "Gladiator's Investiture 2 Piece Bonus",
                Group = "Set Bonuses",
                ConflictingBuffs = new List<string>(new string[] { }),
                Stats = { Resilience = 50f },
                SetName = "Gladiator's Investiture",
                SetThreshold = 2
            });

            defaultBuffs.Add(new Buff()
            {
                Name = "Gladiator's Investiture 4 Piece Bonus",
                Group = "Set Bonuses",
                ConflictingBuffs = new List<string>(new string[] { }),
                Stats = { WeakenedSoulDurationDecrease = 2f },
                SetName = "Gladiator's Investiture",
                SetThreshold = 4
            });

            defaultBuffs.Add(new Buff()
            {
                Name = "Gladiator's Raiment 2 Piece Bonus",
                Group = "Set Bonuses",
                ConflictingBuffs = new List<string>(new string[] { }),
                Stats = { Resilience = 50f },
                SetName = "Gladiator's Raiment",
                SetThreshold = 2
            });

            defaultBuffs.Add(new Buff()
            {
                Name = "Gladiator's Raiment 4 Piece Bonus",
                Group = "Set Bonuses",
                ConflictingBuffs = new List<string>(new string[] { }),
                Stats = { WeakenedSoulDurationDecrease = 2f },
                SetName = "Gladiator's Raiment",
                SetThreshold = 4
            });

            #region Rogue set bonuses
            
            defaultBuffs.Add(new Buff()
            {
                Name = "Netherblade 2 Piece Bonus",
                Group = "Set Bonuses",
                ConflictingBuffs = new List<string>(new string[] { }),
                Stats = { BonusSnDDuration = 3f },
                SetName = "Netherblade",
                SetThreshold = 2
            });

            defaultBuffs.Add(new Buff()
            {
                Name = "Netherblade 4 Piece Bonus",
                Group = "Set Bonuses",
                ConflictingBuffs = new List<string>(new string[] { }),
                Stats = { CPOnFinisher = .15f },
                SetName = "Netherblade",
                SetThreshold = 4
            });

            defaultBuffs.Add(new Buff()
            {
                Name = "Deathmantle 2 Piece Bonus",
                Group = "Set Bonuses",
                ConflictingBuffs = new List<string>(new string[] { }),
                Stats = { BonusEvisEnvenomDamage = 40f },
                SetName = "Deathmantle",
                SetThreshold = 2
            });

            defaultBuffs.Add(new Buff()
            {
                Name = "Deathmantle 4 Piece Bonus",
                Group = "Set Bonuses",
                ConflictingBuffs = new List<string>(new string[] { }),
                Stats = { BonusFreeFinisher = 1f },
                SetName = "Deathmantle",
                SetThreshold = 4
            });

            defaultBuffs.Add(new Buff()
            {
                Name = "Slayer's Armor 2 Piece Bonus",
                Group = "Set Bonuses",
                ConflictingBuffs = new List<string>(new string[] { }),
                Stats = { BonusSnDHaste = .05f },
                SetName = "Slayer's Armor",
                SetThreshold = 2
            });

            defaultBuffs.Add(new Buff()
            {
                Name = "Slayer's Armor 4 Piece Bonus",
                Group = "Set Bonuses",
                ConflictingBuffs = new List<string>(new string[] { }),
                Stats = { BonusCPGDamage = .06f },
                SetName = "Slayer's Armor",
                SetThreshold = 4
            });

            //T7
            defaultBuffs.Add(new Buff()
            {
                Name = "Bonescythe Battlegear 2 Piece Bonus",
                Group = "Set Bonuses",
                ConflictingBuffs = new List<string>(new string[] { }),
                Stats = { RogueT7TwoPieceBonus = 1f },
                SetName = "Bonescythe Battlegear",
                SetThreshold = 2
            });

            defaultBuffs.Add(new Buff()
            {
                Name = "Bonescythe Battlegear 4 Piece Bonus",
                Group = "Set Bonuses",
                ConflictingBuffs = new List<string>(new string[] { }),
                Stats = { RogueT7FourPieceBonus = 1f },
                SetName = "Bonescythe Battlegear",
                SetThreshold = 4
            });

            //T8
            defaultBuffs.Add(new Buff()
            {
                Name = "Terrorblade Battlegear 2 Piece Bonus",
                Group = "Set Bonuses",
                ConflictingBuffs = new List<string>(new string[] { }),
                Stats = { RogueT8TwoPieceBonus = 1f },
                SetName = "Terrorblade Battlegear",
                SetThreshold = 2
            });

            defaultBuffs.Add(new Buff()
            {
                Name = "Terrorblade Battlegear4 Piece Bonus",
                Group = "Set Bonuses",
                ConflictingBuffs = new List<string>(new string[] { }),
                Stats = { RogueT8FourPieceBonus = 1f },
                SetName = "Terrorblade Battlegear",
                SetThreshold = 4
            });

            #endregion

            //Paladin Set Bonuses
            //Holy T7
            defaultBuffs.Add(new Buff()
            {
                Name = "Redemption Regalia 2 Piece Bonus",
                Group = "Set Bonuses",
                ConflictingBuffs = new List<string>(new string[] { }),
                Stats = { HolyShockCrit = .1f },
                SetName = "Redemption Regalia",
                SetThreshold = 2
            });

            defaultBuffs.Add(new Buff()
            {
                Name = "Redemption Regalia 4 Piece Bonus",
                Group = "Set Bonuses",
                ConflictingBuffs = new List<string>(new string[] { }),
                Stats = { HolyLightPercentManaReduction = .05f },
                SetName = "Redemption Regalia",
                SetThreshold = 4
            });

            //Retribution T7
            defaultBuffs.Add(new Buff()
            {
                Name = "Redemption Battlegear 2 Piece Bonus",
                Group = "Set Bonuses",
                ConflictingBuffs = new List<string>(new string[] { }),
                Stats = { DivineStormMultiplier = .1f },
                SetName = "Redemption Battlegear",
                SetThreshold = 2
            });

            defaultBuffs.Add(new Buff()
            {
                Name = "Redemption Battlegear 4 Piece Bonus",
                Group = "Set Bonuses",
                ConflictingBuffs = new List<string>(new string[] { }),
                Stats = { JudgementCDReduction = 1f },
                SetName = "Redemption Battlegear",
                SetThreshold = 4
            });
            
            //Protection T7
            defaultBuffs.Add(new Buff()
            {
                Name = "Redemption Plate 2 Piece Bonus",
                Group = "Set Bonuses",
                ConflictingBuffs = new List<string>(new string[] { }),
                Stats = { BonusHammerOfTheRighteousMultiplier = .1f },
                SetName = "Redemption Plate",
                SetThreshold = 2
            });

            defaultBuffs.Add(new Buff()
            {
                Name = "Redemption Plate 4 Piece Bonus",
                Group = "Set Bonuses",
                ConflictingBuffs = new List<string>(new string[] { }),
                Stats = { DivineProtectionDurationBonus = 3f },
                SetName = "Redemption Plate",
                SetThreshold = 4
            });

            //Protection T8
            defaultBuffs.Add(new Buff()
            {
                Name = "Aegis Plate 2 Piece Bonus",
                Group = "Set Bonuses",
                ConflictingBuffs = new List<string>(new string[] { }),
                Stats = 
                {
                	BonusSealOfVengeanceDamageMultiplier = .1f,
                	BonusSealOfCorruptionDamageMultiplier = .1f,
                	BonusSealOfRighteousnessDamageMultiplier = .1f,
                },
                SetName = "Aegis Plate",
                SetThreshold = 2
            });

            defaultBuffs.Add(buff = new Buff()
            {
                Name = "Aegis Plate 4 Piece Bonus",
                Group = "Set Bonuses",
                ConflictingBuffs = new List<string>(new string[] { }),
                Stats = new Stats(),
                SetName = "Aegis Plate",
                SetThreshold = 4
            });
            buff.Stats.AddSpecialEffect(new SpecialEffect(Trigger.JudgementHit, new Stats() { ShieldOfRighteousnessBlockValue = 225f }, 3.0f, 10.0f, 1.0f));

            //Retribution T8
            defaultBuffs.Add(new Buff()
            {
                Name = "Aegis Battlegear 2 Piece Bonus",
                Group = "Set Bonuses",
                ConflictingBuffs = new List<string>(new string[] { }),
                Stats = { ExorcismMultiplier = .1f, HammerOfWrathMultiplier = .1f },
                SetName = "Aegis Battlegear",
                SetThreshold = 2
            });

            defaultBuffs.Add(new Buff()
            {
                Name = "Aegis Battlegear 4 Piece Bonus",
                Group = "Set Bonuses",
                ConflictingBuffs = new List<string>(new string[] { }),
                Stats = { CrusaderStrikeCrit = .1f, DivineStormCrit = .1f },
                SetName = "Aegis Battlegear",
                SetThreshold = 4
            });

            //Holy T8
            defaultBuffs.Add(new Buff()
            {
                Name = "Aegis Regalia 2 Piece Bonus",
                Group = "Set Bonuses",
                ConflictingBuffs = new List<string>(new string[] { }),
                Stats = { HolyShockHoTOnCrit = .15f },
                SetName = "Aegis Regalia",
                SetThreshold = 2
            });

            defaultBuffs.Add(new Buff()
            {
                Name = "Aegis Regalia 4 Piece Bonus",
                Group = "Set Bonuses",
                ConflictingBuffs = new List<string>(new string[] { }),
                Stats = { SacredShieldICDReduction = 2f },
                SetName = "Aegis Regalia",
                SetThreshold = 4
            });
                      
			#endregion

            #region Profession Buffs
            defaultBuffs.Add(new Buff()
            {
                Name = "Toughness",
                Group = "Profession Buffs",
                ConflictingBuffs = new List<string>(new string[] { }),
                Stats = { Stamina = 50f }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Master of Anatomy",
                Group = "Profession Buffs",
                ConflictingBuffs = new List<string>(new string[] { }),
                Stats = { CritRating = 32f }
            });
            #endregion

            #region Temporary Buffs
            defaultBuffs.Add(new Buff()
            {
                Name = "Bloodlust",
                Group = "Temporary Buffs",
                Stats = { Bloodlust = 0.3f },
                ConflictingBuffs = new List<string>(new string[] { })
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Drums of Battle",
                Group = "Temporary Buffs",
                Stats = { DrumsOfBattle = 80 },
                ConflictingBuffs = new List<string>(new string[] { })
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Drums of War",
                Group = "Temporary Buffs",
                Stats = { DrumsOfWar = 60 },
                ConflictingBuffs = new List<string>(new string[] { })
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Badge of Tenacity",
                Group = "Temporary Buffs",
                Stats = { Agility = 150 },
                ConflictingBuffs = new List<string>(new string[] { })
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Commendation of Kael'thas",
                Group = "Temporary Buffs",
                Stats = { DodgeRating = 152 },
                ConflictingBuffs = new List<string>(new string[] { })
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Shattered Sun Pendant of Resolve Proc (Aldor)",
                Group = "Temporary Buffs",
                Stats = { DodgeRating = 100 },
                ConflictingBuffs = new List<string>(new string[] { })
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Shattered Sun Pendant of Resolve Proc (Scryer)",
                Group = "Temporary Buffs",
                Stats = { ExpertiseRating = 100 },
                ConflictingBuffs = new List<string>(new string[] { })
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Figurine - Empyrean Tortoise",
                Group = "Temporary Buffs",
                Stats = { DodgeRating = 165 },
                ConflictingBuffs = new List<string>(new string[] { })
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Moroes' Lucky Pocket Watch",
                Group = "Temporary Buffs",
                Stats = { DodgeRating = 300 },
                ConflictingBuffs = new List<string>(new string[] { })
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Idol of Terror",
                Group = "Temporary Buffs",
                Stats = { Agility = 65 },
                ConflictingBuffs = new List<string>(new string[] { })
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Stomp",
                Group = "Temporary Buffs",
                Stats = { BonusArmorMultiplier = -0.5f },
                ConflictingBuffs = new List<string>(new string[] { })
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Power of Vesperon",
                Group = "Temporary Buffs",
                Stats = { BonusHealthMultiplier = -0.25f },
                ConflictingBuffs = new List<string>(new string[] { })
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Improved Lay On Hands",
                Group = "Temporary Buffs",
                Stats = { BonusArmorMultiplier = 0.3f },
                ConflictingBuffs = new List<string>(new string[] { })
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Nightmare Seed",
                Group = "Temporary Buffs",
                Stats = { Health = 2000 },
                ConflictingBuffs = new List<string>(new string[] { })
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Heroic 1750 Health Trinket",
                Group = "Temporary Buffs",
                Stats = { Health = 1750 },
                ConflictingBuffs = new List<string>(new string[] { })
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Season 3 Resilience Relic",
                Group = "Temporary Buffs",
                Stats = { Resilience = 31 },
                ConflictingBuffs = new List<string>(new string[] { })
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Moonglade Rejuvination",
                Group = "Temporary Buffs",
                Stats = { DodgeRating = 35 },
                ConflictingBuffs = new List<string>(new string[] { })
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Living Root of the Wildheart",
                Group = "Temporary Buffs",
                Stats = { BonusArmor = 4070 },
                ConflictingBuffs = new List<string>(new string[] { })
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Argussian Compass",
                Group = "Temporary Buffs",
                Stats = { Health = 1150 },
                ConflictingBuffs = new List<string>(new string[] { })
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Dawnstone Crab",
                Group = "Temporary Buffs",
                Stats = { DodgeRating = 125 },
                ConflictingBuffs = new List<string>(new string[] { })
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Adamantite Figurine",
                Group = "Temporary Buffs",
                Stats = { BonusArmor = 1280 },
                ConflictingBuffs = new List<string>(new string[] { })
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Brooch of the Immortal King",
                Group = "Temporary Buffs",
                Stats = { Health = 1250 },
                ConflictingBuffs = new List<string>(new string[] { })
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Mongoose Proc (Constant)",
                Group = "Temporary Buffs",
                Stats = { MongooseProcConstant = 1f },
                ConflictingBuffs = new List<string>(new string[] { "Mongoose" })
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Mongoose Proc (Average)",
                Group = "Temporary Buffs",
                Stats = { MongooseProcAverage = 1f },
                ConflictingBuffs = new List<string>(new string[] { "Mongoose" })
            });
            #endregion

            return defaultBuffs;
        }
    }
}
//1963...
