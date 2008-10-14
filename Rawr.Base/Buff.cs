using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml.Serialization;

namespace Rawr
{
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
		public int SetThreshold = 0;
		public List<Buff> Improvements = new List<Buff>();
		public bool IsCustom = false;
		private List<string> _conflictingBuffs = null;
		public List<string> ConflictingBuffs
		{
			get { return _conflictingBuffs ?? new List<string>(new string[] { Group }); }
			set { _conflictingBuffs = value; }
		}

        private static readonly string _savedFilePath;
        static Buff()
        {
            _savedFilePath = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "Data\\BuffCache.xml");
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
							XmlSerializer serializer = new XmlSerializer(typeof(List<Buff>));
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
			}
			catch { }
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
						_relevantBuffs = AllBuffs.FindAll(buff => Calculations.HasRelevantStats(buff.GetTotalStats()));
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
                setBonuses = AllBuffs.FindAll(buff => buff.SetName == setName);
                setBonusesByName[setName] = setBonuses;
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
			get { return _allBuffs;	}
		}

        private static List<Buff> GetDefaultBuffs()
        {
			List<Buff> defaultBuffs = new List<Buff>();

			#region Buffs

			#region Agility and Strength
			defaultBuffs.Add(new Buff
			{
				Name = "Strength of Earth Totem",
				Group = "Agility and Strength",
				Stats = { Strength = 86, Agility = 86 },
				Improvements = { 
					new Buff { Name = "Enhancing Totems", Stats = { Strength = (float)Math.Floor(86f * 0.15f), Agility = (float)Math.Floor(86f * 0.15f) } }
				}
			});
			#endregion

			#region Armor 
			defaultBuffs.Add(new Buff
			{
				Name = "Devotion Aura",
				Group = "Armor",
				Stats = { Armor = 861f },
				Improvements = { 
					new Buff { Name = "Improved Devotion Aura (Armor)", Stats = { Armor = (float)Math.Floor(861f * 0.5f) } }
				}
			});
			#endregion

			#region Armor (%)
			defaultBuffs.Add(new Buff
			{
				Name = "Ancestral Healing",
				Group = "Armor (%)",
				Stats = { BonusArmorMultiplier = 0.25f }
			});
			defaultBuffs.Add(new Buff
			{
				Name = "Inspiration",
				Group = "Armor (%)",
				Stats = { BonusArmorMultiplier = 0.25f }
			});
			#endregion

			#region Attack Power
			defaultBuffs.Add(new Buff
			{
				Name = "Battle Shout",
				Group = "Attack Power",
				Stats = { AttackPower = 305 },
				Improvements = { 
					new Buff { Name = "Commanding Presence (Attack Power)", Stats = { AttackPower = (float)Math.Floor(305f * 0.25f) } }
				}
			});
			defaultBuffs.Add(new Buff
			{
				Name = "Blessing of Might",
				Group = "Attack Power",
				Stats = { AttackPower = 305 },
				Improvements = { 
					new Buff { Name = "Improved Blessing of Might", Stats = { AttackPower = (float)Math.Floor(305f * 0.25f) } }
				}
			});
			#endregion

			#region Attack Power (%)
			defaultBuffs.Add(new Buff
			{
				Name = "Trueshot Aura",
				Group = "Attack Power (%)",
				Stats = { BonusAttackPowerMultiplier = 0.1f }
			});
			defaultBuffs.Add(new Buff
			{
				Name = "Unleashed Rage",
				Group = "Attack Power (%)",
				Stats = { BonusAttackPowerMultiplier = 0.1f }
			});
			#endregion

			#region Damage (%)
			defaultBuffs.Add(new Buff
			{
				Name = "Ferocious Inspiration",
				Group = "Damage (%)",
				Stats = { BonusDamageMultiplier = 0.03f }
			});
			defaultBuffs.Add(new Buff
			{
				Name = "Sanctified Retribution",
				Group = "Damage (%)",
				Stats = { BonusDamageMultiplier = 0.02f }
			});
			#endregion

			#region Damage Reduction (%)
			defaultBuffs.Add(new Buff
			{
				Name = "Blessing of Sanctuary",
				Group = "Damage Reduction (%)",
				Stats = { DamageTakenMultiplier = -0.03f }
			});
			defaultBuffs.Add(new Buff
			{
				Name = "Grace",
				Group = "Damage Reduction (%)",
				Stats = { DamageTakenMultiplier = -0.03f }
			});
			#endregion

			#region Haste (%)
			defaultBuffs.Add(new Buff
			{
				Name = "Improved Moonkin Form",
				Group = "Haste (%)",
				Stats = { PhysicalHaste = 0.03f, SpellHaste = 0.03f }
			});
			defaultBuffs.Add(new Buff
			{
				Name = "Swift Retribution",
				Group = "Haste (%)",
				Stats = { PhysicalHaste = 0.03f, SpellHaste = 0.03f }
			});
			#endregion

			#region Healing Received (%)
			defaultBuffs.Add(new Buff
			{
				Name = "Improved Devotion Aura (Healing Received %)",
				Group = "Damage Reduction (%)",
				Stats = { HealingReceivedMultiplier = 0.06f }
			});
			defaultBuffs.Add(new Buff
			{
				Name = "Tree of Life Aura",
				Group = "Damage Reduction (%)",
				Stats = { HealingReceivedMultiplier = 0.06f }
			});
			#endregion

			#region Health
			defaultBuffs.Add(new Buff
			{
				Name = "Blood Pact",
				Group = "Health",
				Stats = { Health = 660 },
				Improvements = { 
					new Buff { Name = "Improved Imp", Stats = { Health = (float)Math.Floor(660f * 0.30f) } }
				}
			});
			defaultBuffs.Add(new Buff
			{
				Name = "Commanding Shout",
				Group = "Health",
				Stats = { Health = 1080 },
				Improvements = { 
					new Buff { Name = "Commanding Presence (Health)", Stats = { Health = (float)Math.Floor(1080f * 0.25f) } }
				}
			});
			#endregion

			#region Intellect
			defaultBuffs.Add(new Buff
			{
				Name = "Arcane Intellect",
				Group = "Intellect",
				Stats = { Intellect = 40 }
			});
			defaultBuffs.Add(new Buff
			{
				Name = "Fel Intelligence (Intellect)",
				Group = "Intellect",
				Stats = { Intellect = 32 }
			});
			#endregion

			#region Physical Critical Strike Chance
			defaultBuffs.Add(new Buff
			{
				Name = "Leader of the Pack",
				Group = "Physical Critical Strike Chance",
				Stats = { PhysicalCrit = 0.05f }
			});
			defaultBuffs.Add(new Buff
			{
				Name = "Rampage",
				Group = "Physical Critical Strike Chance",
				Stats = { PhysicalCrit = 0.05f }
			});
			#endregion

			#region Physical Haste
			defaultBuffs.Add(new Buff
			{
				Name = "Windfury Totem",
				Group = "Physical Haste",
				Stats = { PhysicalHaste = 0.16f },
				Improvements = { 
					new Buff { Name = "Improved Windfury Totem", Stats = { PhysicalHaste = (1.2f/1.16f) - 1f } }
				}
			});
			#endregion

			#region Replenishment
			defaultBuffs.Add(new Buff
			{
				Name = "Hunting Party",
				Group = "Replenishment",
				Stats = { ManaRestoreFromMaxManaPerSecond = 0.0025f }
			});
			defaultBuffs.Add(new Buff
			{
				Name = "Judgements of the Wise",
				Group = "Replenishment",
				Stats = { ManaRestoreFromMaxManaPerSecond = 0.0025f }
			});
			defaultBuffs.Add(new Buff
			{
				Name = "Vampiric Touch",
				Group = "Replenishment",
				Stats = { ManaRestoreFromMaxManaPerSecond = 0.0025f }
			});
			#endregion

			#region Mana Regeneration
			defaultBuffs.Add(new Buff
			{
				Name = "Mana Spring Totem",
				Group = "Mana Regeneration",
				ConflictingBuffs = new List<string>( new string[] {}),
				Stats = { Mp5 = 50 },
				Improvements = { 
					new Buff { Name = "Restorative Totems", Stats = { Mp5 = (float)Math.Floor(50f * 0.25f) } }
				}
			});
			defaultBuffs.Add(new Buff
			{
				Name = "Blessing of Wisdom",
				Group = "Mana Regeneration",
				ConflictingBuffs = new List<string>( new string[] {}),
				Stats = { Mp5 = 41 },
				Improvements = { 
					new Buff { Name = "Improved Blessing of Wisdom", Stats = { Mp5 = (float)Math.Floor(41 * 0.2f) } }
				}
			});
			#endregion

			#region Spell Critical Strike Chance
			defaultBuffs.Add(new Buff
			{
				Name = "Elemental Oath",
				Group = "Spell Critical Strike Chance",
				Stats = { SpellCrit = 0.05f }
			});
            defaultBuffs.Add(new Buff
            {
                Name = "Moonkin Form",
                Group = "Spell Critical Strike Chance",
                Stats = { SpellCrit = 0.05f }
            });
			#endregion

			#region Spell Haste
			defaultBuffs.Add(new Buff
			{
				Name = "Wrath of Air Totem",
				Group = "Spell Haste",
				Stats = { SpellHaste = 0.05f }
			});
			#endregion

			#region Spell Power
			defaultBuffs.Add(new Buff
			{
				Name = "Demonic Pact",
				Group = "Spell Power",
				Stats = { BonusSpellPowerMultiplier = 0.1f }
			});
			defaultBuffs.Add(new Buff
			{
				Name = "Flametongue Totem",
				Group = "Spell Power",
				Stats = { SpellPower = 106f }
			});
			defaultBuffs.Add(new Buff
			{
				Name = "Totem of Wrath (Spell Power)",
				Group = "Spell Power",
				Stats = { SpellPower = 140f }
			});
			defaultBuffs.Add(new Buff
			{
				Name = "Improved Divine Spirit",
				Group = "Spell Power",
				Stats = { SpellPower = 50f }
			});
			#endregion

			#region Spell Sensitivity
			defaultBuffs.Add(new Buff
			{
				Name = "Amplify Magic (on target, not self)",
				Group = "Spell Sensitivity",
				Stats = { SpellPower = 128 },
				Improvements = { 
					new Buff { Name = "Magic Attunement", Stats = { SpellPower = (float)Math.Floor(128 * 0.5f) } } 
				}
			});
			#endregion

			#region Spirit
			defaultBuffs.Add(new Buff
			{
				Name = "Fel Intelligence (Spirit)",
				Group = "Spirit",
				Stats = { Spirit = 40f }
			});
			defaultBuffs.Add(new Buff
			{
				Name = "Divinie Spirit",
				Group = "Spirit",
				Stats = { Spirit = 50f }
			});
			#endregion

			#region Stamina
			defaultBuffs.Add(new Buff
			{
				Name = "Power Word: Fortitude",
				Group = "Stamina",
				Stats = { Stamina = 79f },
				Improvements = { 
					new Buff { Name = "Improved Power Word: Fortitude", Stats = { Stamina = (float)Math.Floor(79f * 0.3f) } } 
				}
			});
			#endregion

			#region Stat Add
			defaultBuffs.Add(new Buff
			{
				Name = "Mark of the Wild",
				Group = "Stat Add",
				Stats = { Armor = 340, Strength = 14, Agility = 14, Stamina = 14, Intellect = 14, Spirit = 14, 
					ArcaneResistanceBuff = 25, FireResistanceBuff = 25, FrostResistanceBuff = 25, 
					NatureResistanceBuff = 25, ShadowResistanceBuff = 25 },
				Improvements = { 
					new Buff { Name = "Improved Mark of the Wild", Stats = {
					Armor = (float)Math.Floor(340f * 0.4f),
					Strength = (float)Math.Floor(14f * 0.4f),
					Agility = (float)Math.Floor(14f * 0.4f),
					Stamina = (float)Math.Floor(14f * 0.4f),
					Intellect = (float)Math.Floor(14f * 0.4f),
					Spirit = (float)Math.Floor(14f * 0.4f),
					ArcaneResistanceBuff = (float)Math.Floor(25f * 1.4f), // it is 1.4 because it's non stacking buff, it takes max
					FireResistanceBuff = (float)Math.Floor(25f * 1.4f),
					FrostResistanceBuff = (float)Math.Floor(25f * 1.4f),
					NatureResistanceBuff = (float)Math.Floor(25f * 1.4f),
					ShadowResistanceBuff = (float)Math.Floor(25f * 1.4f)} 
					} 
				}
			});
			#endregion

			#region Stat Multiplier
			defaultBuffs.Add(new Buff
			{
				Name = "Blessing of Kings",
				Group = "Stat Multiplier",
				Stats = { BonusAgilityMultiplier = 0.02f, BonusStrengthMultiplier = 0.02f, 
					BonusIntellectMultiplier = 0.02f, BonusStaminaMultiplier = 0.02f, BonusSpiritMultiplier = 0.02f },
				Improvements = { 
					new Buff { Name = "Improved Blessing of Kings", Stats = { 
						BonusAgilityMultiplier = (1.1f / 1.02f) - 1, BonusStrengthMultiplier = (1.1f / 1.02f) - 1,
						BonusIntellectMultiplier = (1.1f / 1.02f) - 1, BonusStaminaMultiplier = (1.1f / 1.02f) - 1,
						BonusSpiritMultiplier = (1.1f / 1.02f) - 1} } 
				}
			});
			#endregion

			#region Resistance
			defaultBuffs.Add(new Buff()
            {
                Name = "Shadow Protection",
                Group = "Resistance",
				ConflictingBuffs = new List<string>( new string[] { "Shadow Resistance Buff" }),
                Stats = { ShadowResistanceBuff = 70 }
            });
			defaultBuffs.Add(new Buff()
            {
                Name = "Shadow Resistance Aura",
                Group = "Resistance",
				ConflictingBuffs = new List<string>( new string[] { "Shadow Resistance Buff" }),
                Stats = { ShadowResistanceBuff = 70 }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Aspect of the Wild",
                Group = "Resistance",
				ConflictingBuffs = new List<string>( new string[] { "Nature Resistance Buff" }),
                Stats = { NatureResistanceBuff = 70 }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Nature Resistance Totem",
                Group = "Resistance",
				ConflictingBuffs = new List<string>( new string[] { "Nature Resistance Buff" }),
                Stats = { NatureResistanceBuff = 70 }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Frost Resistance Aura",
                Group = "Resistance",
				ConflictingBuffs = new List<string>( new string[] { "Frost Resistance Buff" }),
                Stats = { FrostResistanceBuff = 70 }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Frost Resistance Totem",
                Group = "Resistance",
				ConflictingBuffs = new List<string>( new string[] { "Frost Resistance Buff" }),
                Stats = { FrostResistanceBuff = 70 }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Fire Resistance Aura",
                Group = "Resistance",
				ConflictingBuffs = new List<string>( new string[] { "Fire Resistance Buff" }),
                Stats = { FireResistanceBuff = 70 }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Fire Resistance Totem",
                Group = "Resistance",
				ConflictingBuffs = new List<string>( new string[] { "Fire Resistance Buff" }),
                Stats = { FireResistanceBuff = 70 }
            });
			#endregion

			#region Pushback Protection
			defaultBuffs.Add(new Buff()
            {
                Name = "Concentration Aura",
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
                ConflictingBuffs = new List<string>( new string[] { "Mage Class Armor" })
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Molten Armor",
                Group = "Class Buffs",
                Stats = { MageMoltenArmor = 1f },
                ConflictingBuffs = new List<string>( new string[] { "Mage Class Armor" })
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Ice Armor",
                Group = "Class Buffs",
                Stats = { MageIceArmor = 1f },
                ConflictingBuffs = new List<string>( new string[] { "Mage Class Armor" })
            });
			#endregion

            #region Racial Buffs
            defaultBuffs.Add(new Buff
            {
                Name = "Heroic Presence",
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
				Group = "Armor (Major)",
				Stats = { ArmorPenetration = 2600f }
			});
			defaultBuffs.Add(new Buff
			{
				Name = "Expose Armor",
				Group = "Armor (Major)",
				Stats = { ArmorPenetration = 2600f }
			});
			defaultBuffs.Add(new Buff
			{
				Name = "Sunder Armor",
				Group = "Armor (Major)",
				Stats = { ArmorPenetration = 2600f }
			});
			#endregion

			#region Armor (Minor)
			defaultBuffs.Add(new Buff
			{
				Name = "Curse of Recklessness",
				Group = "Armor (Minor)",
				Stats = { ArmorPenetration = 610f }
			});
			defaultBuffs.Add(new Buff
			{
				Name = "Faerie Fire",
				Group = "Armor (Minor)",
				Stats = { ArmorPenetration = 610f }
			});
			defaultBuffs.Add(new Buff
			{
				Name = "Sting",
				Group = "Armor (Minor)",
				Stats = { ArmorPenetration = 610f }
			});
			#endregion

			#region Ranged Attack Power
            defaultBuffs.Add(new Buff()
            {
                Name = "Improved Hunters Mark",
                Group = "Ranged Attack Power",
                Stats = { RangedAttackPower = 110 }
            });
			#endregion

			#region Expose Weakness
            defaultBuffs.Add(new Buff()
            {
                Name = "Expose Weakness",
                Group = "Expose Weakness",
                Stats = { ExposeWeakness = 1 }
            });
			#endregion

			#region Bleed Damage
			defaultBuffs.Add(new Buff
			{
				Name = "Mangle",
				Group = "Bleed Damage",
				Stats = { BonusBleedDamageMultiplier = 0.3f }
			});
			defaultBuffs.Add(new Buff
			{
				Name = "Trauma",
				Group = "Bleed Damage",
				Stats = { BonusBleedDamageMultiplier = 0.3f }
			});
			#endregion

			#region Critical Strike Chance Taken
			defaultBuffs.Add(new Buff
			{
				Name = "Heart of the Crusader",
				Group = "Critical Strike Chance Taken",
				Stats = { PhysicalCrit = 0.03f, SpellCrit = 0.03f }
			});
			defaultBuffs.Add(new Buff
			{
				Name = "Master Poisoner",
				Group = "Critical Strike Chance Taken",
				Stats = { PhysicalCrit = 0.03f, SpellCrit = 0.03f }
			});
			defaultBuffs.Add(new Buff
			{
				Name = "Totem of Wrath",
				Group = "Critical Strike Chance Taken",
				Stats = { PhysicalCrit = 0.03f, SpellCrit = 0.03f }
			});
			#endregion

			#region Mana Restore
			defaultBuffs.Add(new Buff
			{
				Name = "Judgement of Wisdom",
				Group = "Mana Restore",
				Stats = { ManaRestorePerHit = 0.02f }
			});
			#endregion

			#region Melee Hit Chance Reduction
			defaultBuffs.Add(new Buff
			{
				Name = "Insect Swarm",
				Group = "Melee Hit Chance Reduction",
				Stats = { Miss = 0.03f }
			});
			defaultBuffs.Add(new Buff
			{
				Name = "Scorpid Sting",
				Group = "Melee Hit Chance Reduction",
				Stats = { Miss = 0.03f }
			});
			#endregion

			#region Physical Vulnerability
			defaultBuffs.Add(new Buff
			{
				Name = "Blood Frenzy",
				Group = "Physical Vulnerability",
				Stats = { BonusPhysicalDamageMultiplier = 0.02f }
			});
			defaultBuffs.Add(new Buff
			{
				Name = "Savage Combat",
				Group = "Physical Vulnerability",
				Stats = { BonusPhysicalDamageMultiplier = 0.02f }
			});
			#endregion

			#region Spell Critical Strike Chance
			defaultBuffs.Add(new Buff
			{
				Name = "Winter's Chill",
				Group = "Spell Critical Strike Taken",
				Stats = { SpellCrit = 0.1f }
			});
			defaultBuffs.Add(new Buff
			{
				Name = "Improved Scorch",
				Group = "Spell Critical Strike Taken",
				Stats = { SpellCrit = 0.1f }
			});
			#endregion

			#region Spell Damage Taken
			defaultBuffs.Add(new Buff
			{
				Name = "Curse of the Elements",
				Group = "Spell Damage Taken",
				Stats = { BonusFireSpellPowerMultiplier = 0.1f, BonusFrostSpellPowerMultiplier = 0.1f,
					BonusArcaneSpellPowerMultiplier = 0.1f, BonusShadowSpellPowerMultiplier = 0.1f,
					BonusHolySpellPowerMultiplier = 0.1f},
				Improvements = { new Buff { Name = "Malediction", Stats = { BonusFireSpellPowerMultiplier = 1.13f / 1.1f - 1, 
					BonusFrostSpellPowerMultiplier = 1.13f / 1.1f - 1, BonusArcaneSpellPowerMultiplier = 1.13f / 1.1f - 1, 
					BonusShadowSpellPowerMultiplier = 1.13f / 1.1f - 1, BonusHolySpellPowerMultiplier = 1.13f / 1.1f - 1 } } }
			});
			defaultBuffs.Add(new Buff
			{
				Name = "Earth and Moon",
				Group = "Spell Damage Taken",
				Stats = { BonusFireSpellPowerMultiplier = 0.13f, BonusFrostSpellPowerMultiplier = 0.13f,
					BonusArcaneSpellPowerMultiplier = 0.13f, BonusShadowSpellPowerMultiplier = 0.13f,
					BonusHolySpellPowerMultiplier = 0.13f},
			});
			defaultBuffs.Add(new Buff
			{
				Name = "Ebon Plaguebringer",
				Group = "Spell Damage Taken",
				Stats = { BonusFireSpellPowerMultiplier = 0.13f, BonusFrostSpellPowerMultiplier = 0.13f,
					BonusArcaneSpellPowerMultiplier = 0.13f, BonusShadowSpellPowerMultiplier = 0.13f,
					BonusHolySpellPowerMultiplier = 0.13f},
			});
			#endregion

			#region Spell Hit Chance Taken
			defaultBuffs.Add(new Buff
			{
				Name = "Improved Faerie Fire",
				Group = "Spell Hit Chance Taken",
				Stats = { SpellHit = 0.03f }
			});
			defaultBuffs.Add(new Buff
			{
				Name = "Misery",
				Group = "Spell Hit Chance Taken",
				Stats = { SpellHit = 0.03f }
			});
			#endregion

			#region Special Mobs
            defaultBuffs.Add(new Buff()
            {
                Name = "Dual Wielding Mob",
                Group = "Special Mobs",
				ConflictingBuffs = new List<string>( new string[] { "Dual Wielding Mob" }),
                Stats = { Miss = 0.2f }
            });
			#endregion

			#endregion

			#region Consumables
			
			#region Elixirs and Flasks
            defaultBuffs.Add(new Buff()
            {
                Name = "Elixir of Ironskin",
                Group = "Elixirs and Flasks",
                Stats = { Resilience = 30 },
                ConflictingBuffs = new List<string>( new string[] { "Guardian Elixir" })
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Elixir of Major Defense",
                Group = "Elixirs and Flasks",
                Stats = { Armor = 550 },
                ConflictingBuffs = new List<string>( new string[] { "Guardian Elixir" })
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Elixir of Major Fortitude",
                Group = "Elixirs and Flasks",
                Stats = { Health = 250 },
                ConflictingBuffs = new List<string>( new string[] { "Guardian Elixir" })
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Elixir of Major Agility",
                Group = "Elixirs and Flasks",
                Stats = { Agility = 35, CritRating = 20 },
                ConflictingBuffs = new List<string>( new string[] { "Battle Elixir" })
			});
			defaultBuffs.Add(new Buff()
			{
				Name = "Elixir of Demonslaying",
				Group = "Elixirs and Flasks",
				Stats = { AttackPower = 265 },
				ConflictingBuffs = new List<string>( new string[] { "Battle Elixir" })
			});
            defaultBuffs.Add(new Buff()
            {
                Name = "Elixir of Mastery",
                Group = "Elixirs and Flasks",
                Stats = { Agility = 15, Stamina = 15, Strength = 15, Intellect = 15, Spirit = 15 },
                ConflictingBuffs = new List<string>( new string[] { "Battle Elixir" })
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Elixir of Major Strength",
                Group = "Elixirs and Flasks",
                Stats = { Strength = 35 },
                ConflictingBuffs = new List<string>( new string[] { "Battle Elixir" })
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Flask of Fortification",
                Group = "Elixirs and Flasks",
                Stats = { Health = 500, DefenseRating = 10 },
                ConflictingBuffs = new List<string>( new string[] { "Battle Elixir", "Guardian Elixir" })
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Flask of Chromatic Wonder",
                Group = "Elixirs and Flasks",
                Stats = { Agility = 18, Strength = 18, Stamina = 18, Intellect = 18, Spirit = 18, 
					ArcaneResistance = 35, FireResistance = 35, FrostResistance = 35, ShadowResistance = 35, NatureResistance = 35 },
                ConflictingBuffs = new List<string>( new string[] { "Battle Elixir", "Guardian Elixir" })
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Flask of Relentless Assault",
                Group = "Elixirs and Flasks",
                Stats = { AttackPower = 120 },
                ConflictingBuffs = new List<string>( new string[] { "Battle Elixir", "Guardian Elixir" })
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Adept's Elixir",
                Group = "Elixirs and Flasks",
                Stats = { SpellPower = 24, CritRating = 24 },
                ConflictingBuffs = new List<string>( new string[] { "Battle Elixir" })
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Elixir of Healing Power",
                Group = "Elixirs and Flasks",
				Stats = { SpellPower = 24f, Spirit = 24 },
                ConflictingBuffs = new List<string>( new string[] { "Battle Elixir" })
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Elixir of Major Firepower",
                Group = "Elixirs and Flasks",
                Stats = { SpellFireDamageRating = 55 },
                ConflictingBuffs = new List<string>( new string[] { "Battle Elixir" })
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Elixir of Major Frost Power",
                Group = "Elixirs and Flasks",
                Stats = { SpellFrostDamageRating = 55 },
                ConflictingBuffs = new List<string>( new string[] { "Battle Elixir" })
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Elixir of Major Shadow Power",
                Group = "Elixirs and Flasks",
                Stats = { SpellShadowDamageRating = 55 },
                ConflictingBuffs = new List<string>( new string[] { "Battle Elixir" })
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Elixir of Draenic Wisdom",
                Group = "Elixirs and Flasks",
                Stats = { Intellect = 30, Spirit = 30 },
                ConflictingBuffs = new List<string>( new string[] { "Guardian Elixir" })
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Elixir of Major Mageblood",
                Group = "Elixirs and Flasks",
                Stats = { Mp5 = 16 },
                ConflictingBuffs = new List<string>( new string[] { "Guardian Elixir" })
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Flask of Blinding Light",
                Group = "Elixirs and Flasks",
                Stats = { SpellArcaneDamageRating = 80, SpellNatureDamageRating = 80},
                ConflictingBuffs = new List<string>( new string[] { "Battle Elixir", "Guardian Elixir" })
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Flask of Distilled Wisdom",
                Group = "Elixirs and Flasks",
                Stats = { Intellect = 65 },
                ConflictingBuffs = new List<string>( new string[] { "Battle Elixir", "Guardian Elixir" })
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Flask of Mighty Restoration",
                Group = "Elixirs and Flasks",
                Stats = { Mp5 = 25 },
                ConflictingBuffs = new List<string>( new string[] { "Battle Elixir", "Guardian Elixir" })
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Flask of Pure Death",
                Group = "Elixirs and Flasks",
                Stats = { SpellFireDamageRating = 80, SpellFrostDamageRating = 80, SpellShadowDamageRating = 80 },
                ConflictingBuffs = new List<string>( new string[] { "Battle Elixir", "Guardian Elixir" })
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Flask of Supreme Power",
                Group = "Elixirs and Flasks",
                Stats = { SpellPower = 70 },
                ConflictingBuffs = new List<string>( new string[] { "Battle Elixir", "Guardian Elixir" })
            });
			#endregion

			#region Potion
			defaultBuffs.Add(new Buff()
			{
				Name = "Haste Potion",
				Group = "Potion",
				Stats = { HasteRating = 50 },
			});
            defaultBuffs.Add(new Buff()
            {
                Name = "Heroic Potion",
                Group = "Potion",
                Stats = { Health = 700 }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Ironshield Potion",
                Group = "Potion",
                Stats = { Armor = 2500 }
            });
			#endregion

			#region Food
            defaultBuffs.Add(new Buff()
            {
                Name = "20 Stamina Food",
                Group = "Food",
                Stats = { Stamina = 20, Spirit = 20 }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "30 Stamina Food",
                Group = "Food",
                Stats = { Stamina = 30, Spirit = 20 }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "20 Agility Food",
                Group = "Food",
                Stats = { Agility = 20, Spirit = 20 }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "20 Strength Food",
                Group = "Food",
                Stats = { Strength = 20, Spirit = 20 }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "40 Attack Power Food",
                Group = "Food",
                Stats = { AttackPower = 40, Spirit = 20 }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "20 Hit Rating Food",
                Group = "Food",
                Stats = { HitRating = 20, Spirit = 20 }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "20 Spell Crit Food",
                Group = "Food",
                Stats = { CritRating = 20, Spirit = 20 }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "23 Spell Power Food",
                Group = "Food",
                Stats = { SpellPower = 23, Spirit = 20 }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "8 mp5 Food",
                Group = "Food",
                Stats = { Mp5 = 8, Stamina = 20 }
            });
			#endregion

			#region Scrolls
            defaultBuffs.Add(new Buff()
            {
                Name = "Scroll of Agility VI",
				Group = "Scrolls",
                ConflictingBuffs = new List<string>( new string[] { "Scroll of Agility" }),
                Stats = { Agility = 20 }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Scroll of Strength VI",
				Group = "Scrolls",
                ConflictingBuffs = new List<string>( new string[] { "Scroll of Strength" }),
                Stats = { Strength = 20 }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Scroll of Intellect VI",
				Group = "Scrolls",
                ConflictingBuffs = new List<string>( new string[] { "Scroll of Intellect" }),
                Stats = { Intellect = 24 }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Scroll of Stamina VI",
				Group = "Scrolls",
                ConflictingBuffs = new List<string>( new string[] { "Scroll of Stamina" }),
                Stats = { Stamina = 43 }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Scroll of Spirit VI",
				Group = "Scrolls",
                ConflictingBuffs = new List<string>( new string[] { "Scroll of Spirit" }),
                Stats = { Spirit = 32 }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Scroll of Protection VI",
				Group = "Scrolls",
                ConflictingBuffs = new List<string>( new string[] { "Scroll of Protection" }),
                Stats = { Armor = 285 }
            });
			#endregion

			#region Temporary Weapon Enchantment
			defaultBuffs.Add(new Buff()
            {
                Name = "Adamantite Weightstone",
                Group = "Temporary Weapon Enchantment",
                Stats = { WeaponDamage = 12, CritRating = 14 }
			});
			defaultBuffs.Add(new Buff()
			{
				Name = "Righteous Weapon Coating",
				Group = "Temporary Weapon Enchantment",
				Stats = { AttackPower = 60 }
			});
            defaultBuffs.Add(new Buff()
            {
                Name = "Elemental Sharpening Stone",
                Group = "Temporary Weapon Enchantment",
                Stats = { CritRating = 28 }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Superior Wizard Oil",
                Group = "Temporary Weapon Enchantment",
                Stats = { SpellPower = 42 }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Brilliant Wizard Oil",
                Group = "Temporary Weapon Enchantment",
                Stats = { SpellPower = 36, CritRating = 14 }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Superior Mana Oil",
                Group = "Temporary Weapon Enchantment",
                Stats = { Mp5 = 14 }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Brilliant Mana Oil",
                Group = "Temporary Weapon Enchantment",
				Stats = { Mp5 = 12, SpellPower = 13 }
            });
			#endregion

			#endregion

			#region Set Bonuses
            defaultBuffs.Add(new Buff()
            {
                Name = "Malorne Harness 2 Piece Bonus",
                Group = "Set Bonuses", ConflictingBuffs = new List<string>( new string[] {}),
                Stats = { BloodlustProc = 0.8f },
                SetName = "Malorne Harness",
                SetThreshold = 2
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Malorne Harness 4 Piece Bonus",
                Group = "Set Bonuses", ConflictingBuffs = new List<string>( new string[] {}),
                Stats = { Armor = 1400, CatFormStrength = 30 },
                SetName = "Malorne Harness",
                SetThreshold = 4
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Nordrassil Harness 4 Piece Bonus",
                Group = "Set Bonuses", ConflictingBuffs = new List<string>( new string[] {}),
                Stats = { BonusShredDamage = 75, BonusLacerateDamage = 15/5},
                SetName = "Nordrassil Harness",
                SetThreshold = 4
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Thunderheart Harness 2 Piece Bonus",
                Group = "Set Bonuses", ConflictingBuffs = new List<string>( new string[] {}),
                Stats = { MangleCatCostReduction = 5, BonusMangleBearThreat = 0.15f },
                SetName = "Thunderheart Harness",
                SetThreshold = 2
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Thunderheart Harness 4 Piece Bonus",
                Group = "Set Bonuses", ConflictingBuffs = new List<string>( new string[] {}),
                Stats = { BonusRipDamageMultiplier = .15f, BonusSwipeDamageMultiplier = .15f },
                SetName = "Thunderheart Harness",
                SetThreshold = 4
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Gladiator Sanctuary 2 Piece Bonus",
                Group = "Set Bonuses", ConflictingBuffs = new List<string>( new string[] {}),
                Stats = { Resilience = 35 },
                SetName = "Gladiator's Sanctuary",
                SetThreshold = 2
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Aldor Regalia 2 Piece Bonus",
                Group = "Set Bonuses", ConflictingBuffs = new List<string>( new string[] {}),
                Stats = { AldorRegaliaInterruptProtection = 1 },
                SetName = "Aldor Regalia",
                SetThreshold = 2
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Tirisfal Regalia 2 Piece Bonus",
                Group = "Set Bonuses", ConflictingBuffs = new List<string>( new string[] {}),
                Stats = { ArcaneBlastBonus = .2f },
                SetName = "Tirisfal Regalia",
                SetThreshold = 2
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Tirisfal Regalia 4 Piece Bonus",
                Group = "Set Bonuses", ConflictingBuffs = new List<string>( new string[] {}),
                Stats = { SpellDamageFor6SecOnCrit = 70f },
                SetName = "Tirisfal Regalia",
                SetThreshold = 4
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Tempest Regalia 2 Piece Bonus",
                Group = "Set Bonuses", ConflictingBuffs = new List<string>( new string[] {}),
                Stats = { EvocationExtension = 2f },
                SetName = "Tempest Regalia",
                SetThreshold = 2
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Tempest Regalia 4 Piece Bonus",
                Group = "Set Bonuses", ConflictingBuffs = new List<string>( new string[] {}),
                Stats = { BonusMageNukeMultiplier = 0.05f },
                SetName = "Tempest Regalia",
                SetThreshold = 4
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Voidheart Raiment 2 Piece Bonus",
                Group = "Set Bonuses", ConflictingBuffs = new List<string>( new string[] {}),
                Stats = { BonusWarlockSchoolDamageOnCast = 135 },
                SetName = "Voidheart Raiment",
                SetThreshold = 2
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Voidheart Raiment 4 Piece Bonus",
                Group = "Set Bonuses", ConflictingBuffs = new List<string>( new string[] {}),
                Stats = { BonusWarlockDotExtension = 3 },
                SetName = "Voidheart Raiment",
                SetThreshold = 4
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Corruptor Raiment 4 Piece Bonus",
                Group = "Set Bonuses", ConflictingBuffs = new List<string>( new string[] {}),
                Stats = { BonusWarlockDotDamageMultiplier = 0.1f },
                SetName = "Corruptor Raiment",
                SetThreshold = 4
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Malefic Raiment 4 Piece Bonus",
                Group = "Set Bonuses", ConflictingBuffs = new List<string>( new string[] {}),
                Stats = { BonusWarlockNukeMultiplier = 0.06f },
                SetName = "Malefic Raiment",
                SetThreshold = 4
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Spellfire 3 Piece Bonus",
                Group = "Set Bonuses", ConflictingBuffs = new List<string>( new string[] {}),
                Stats = { SpellDamageFromIntellectPercentage = 0.07f },
                SetName = "Wrath of Spellfire",
                SetThreshold = 3
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Spellstrike 2 Piece Bonus",
                Group = "Set Bonuses", ConflictingBuffs = new List<string>( new string[] {}),
                Stats = { SpellDamageFor10SecOnHit_5 = 92 },
                SetName = "Spellstrike Infusion",
                SetThreshold = 2
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Lightbringer Raiment 2 Piece",
                Group = "Set Bonuses", ConflictingBuffs = new List<string>( new string[] {}),
                Stats = { HLCrit = .05f },
                SetName = "Lightbringer Raiment",
                SetThreshold = 2
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Lightbringer Raiment 4 Piece",
                Group = "Set Bonuses", ConflictingBuffs = new List<string>( new string[] {}),
                Stats = { FoLMultiplier = .05f },
                SetName = "Lightbringer Raiment",
                SetThreshold = 4
            });
            // Resto druid tier 4/5/6 sets
            defaultBuffs.Add(new Buff()
            {
                Name = "Malorne Raiment 2 Piece",
                Group = "Set Bonuses", ConflictingBuffs = new List<string>( new string[] {}),
                Stats = { ManaRestorePerCast_5_15 = 120 },
                SetName = "Malorne Raiment",
                SetThreshold = 2
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Nordrassil Raiment 2 Piece",
                Group = "Set Bonuses", ConflictingBuffs = new List<string>( new string[] {}),
                Stats = { RegrowthExtraTicks = 2 },
                SetName = "Nordrassil Raiment",
                SetThreshold = 2
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Nordrassil Raiment 4 Piece",
                Group = "Set Bonuses", ConflictingBuffs = new List<string>( new string[] {}),
                Stats = { LifebloomFinalHealBonus = 150 },
                SetName = "Nordrassil Raiment",
                SetThreshold = 4
            }); 
            defaultBuffs.Add(new Buff()
            {
                Name = "Thunderheart Raiment 4 Piece",
                Group = "Set Bonuses", ConflictingBuffs = new List<string>( new string[] {}),
                Stats = { BonusHealingTouchMultiplier = 0.05f },
                SetName = "Thunderheart Raiment",
                SetThreshold = 4
            });
            

            // Windhawk (epic leather caster) set
            defaultBuffs.Add(new Buff()
            {
                Name = "Windhawk Armor",
                Group = "Set Bonuses", ConflictingBuffs = new List<string>( new string[] {}),
                Stats = { Mp5 = 8.0f },
                SetName = "Windhawk Armor",
                SetThreshold = 3
            });
            // Moonkin tier 4/5/6 sets
            defaultBuffs.Add(new Buff()
            {
                Name = "Malorne Regalia 2 Piece Bonus",
                Group = "Set Bonuses", ConflictingBuffs = new List<string>( new string[] {}),
                Stats = { ManaRestorePerCast = .05f * 120 },
                SetName = "Malorne Regalia",
                SetThreshold = 2
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Malorne Regalia 4 Piece Bonus",
                Group = "Set Bonuses", ConflictingBuffs = new List<string>( new string[] {}),
                Stats = { InnervateCooldownReduction = 48.0f },
                SetName = "Malorne Regalia",
                SetThreshold = 4
            });
            // Nordrassil 2-piece skipped because it has nothing to do with dps
            defaultBuffs.Add(new Buff()
            {
                Name = "Nordrassil Regalia 4 Piece Bonus",
                Group = "Set Bonuses", ConflictingBuffs = new List<string>( new string[] {}),
                Stats = { StarfireBonusWithDot = 0.1f },
                SetName = "Nordrassil Regalia",
                SetThreshold = 4
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Thunderheart Regalia 2 Piece Bonus",
                Group = "Set Bonuses", ConflictingBuffs = new List<string>( new string[] {}),
                Stats = { MoonfireExtension = 3.0f },
                SetName = "Thunderheart Regalia",
                SetThreshold = 2
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Thunderheart Regalia 4 Piece Bonus",
                Group = "Set Bonuses", ConflictingBuffs = new List<string>( new string[] {}),
                Stats = { StarfireCritChance = 0.05f },
                SetName = "Thunderheart Regalia",
                SetThreshold = 4
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Destroyer Armor 2 Piece Bonus",
                Group = "Set Bonuses", ConflictingBuffs = new List<string>( new string[] {}),
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
                Group = "Set Bonuses", ConflictingBuffs = new List<string>( new string[] {}),
                Stats = { HasteRating = 67 },
                SetName = "Destroyer Armor",
                SetThreshold = 4
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Onslaught Armor 2 Piece Bonus",
                Group = "Set Bonuses", ConflictingBuffs = new List<string>( new string[] {}),
                Stats = { BonusCommandingShoutHP = 170f },
                SetName = "Onslaught Armor",
                SetThreshold = 2
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Onslaught Armor 4 Piece Bonus",
                Group = "Set Bonuses", ConflictingBuffs = new List<string>( new string[] {}),
                Stats = { BonusShieldSlamDamage = 0.1f },
                SetName = "Onslaught Armor",
                SetThreshold = 4
            });
			defaultBuffs.Add(new Buff()
			{
				Name = "Primalstrike 3 Piece Bonus",
				Group = "Set Bonuses", ConflictingBuffs = new List<string>( new string[] {}),
				Stats = { AttackPower = 40 },
				SetName = "Primal Intent",
				SetThreshold = 3
			});
			defaultBuffs.Add(new Buff()
			{
				Name = "Fel Leather 3 Piece Bonus",
				Group = "Set Bonuses", ConflictingBuffs = new List<string>( new string[] {}),
				Stats = { DodgeRating = 20 },
				SetName = "Fel Skin",
				SetThreshold = 3
			});
			defaultBuffs.Add(new Buff()
			{
				Name = "Heavy Clefthoof 3 Piece Bonus",
				Group = "Set Bonuses", ConflictingBuffs = new List<string>( new string[] {}),
				Stats = { Strength = 20 },
				SetName = "Strength of the Clefthoof",
				SetThreshold = 3
			});

			  // Resto Shammy set bonuses:
			defaultBuffs.Add(new Buff()
			{
				Name = "Cyclone Raiment 2 Piece Bonus",
				Group = "Set Bonuses", ConflictingBuffs = new List<string>( new string[] {}),
				SetName = "Cyclone Raiment",
				Stats = { ManaSpringMp5Increase = 7.5f },
				SetThreshold = 2
			});

			  defaultBuffs.Add(new Buff()
			  {
				Name = "Cataclysm Raiment 2 Piece Bonus",
				Group = "Set Bonuses", ConflictingBuffs = new List<string>( new string[] {}),
				SetName = "Cataclysm Raiment",
				Stats = { LHWManaReduction = .05f },
				SetThreshold = 2
			  });

			  defaultBuffs.Add(new Buff()
			  {
				Name = "Skyshatter Raiment 2 Piece Bonus",
				Group = "Set Bonuses", ConflictingBuffs = new List<string>( new string[] {}),
				SetName = "Skyshatter Raiment",
				Stats = { CHManaReduction = .1f },
				SetThreshold = 2
			  });

			  defaultBuffs.Add(new Buff()
			  {
				Name = "Skyshatter Raiment 4 Piece Bonus",
				Group = "Set Bonuses", ConflictingBuffs = new List<string>( new string[] {}),
				SetName = "Skyshatter Raiment",
				Stats = { CHHealIncrease = .05f },
				SetThreshold = 4
			  });
		    
			//Hunter Set Bonuses
			  defaultBuffs.Add(new Buff()
			  {
				  Name = "Rift Stalker Armor 4 Piece Bonus",
				  Group = "Set Bonuses", ConflictingBuffs = new List<string>( new string[] {}),
				  SetName = "Rift Stalker Armor",
				  Stats = {BonusSteadyShotCrit = .05f},
				  SetThreshold = 4
			  });
			  defaultBuffs.Add(new Buff()
			  {
				  Name = "Gronnstalker's Armor 4 Piece Bonus",
				  Group = "Set Bonuses", ConflictingBuffs = new List<string>( new string[] {}),
				  SetName = "Gronnstalker's Armor",
				  Stats = { BonusSteadyShotDamageMultiplier = .1f },
				  SetThreshold = 4
			  });

			  // Holy Priest bonuses
			  defaultBuffs.Add(new Buff()
			  {
				  Name = "Primal Mooncloth 3 Piece Bonus",
				  Group = "Set Bonuses", ConflictingBuffs = new List<string>( new string[] {}),
				  Stats = { SpellCombatManaRegeneration = 0.05f },
				  SetName = "Primal Mooncloth",
				  SetThreshold = 3
			  });

			  defaultBuffs.Add(new Buff()
			  {
				  Name = "Vestments of Absolution 2 Piece Bonus",
				  Group = "Set Bonuses", ConflictingBuffs = new List<string>( new string[] {}),
				  Stats = { BonusPoHManaCostReductionMultiplier = 0.1f },
				  SetName = "Vestments of Absolution",
				  SetThreshold = 2
			  });

			  defaultBuffs.Add(new Buff()
			  {
				  Name = "Vestments of Absolution 4 Piece Bonus",
				  Group = "Set Bonuses", ConflictingBuffs = new List<string>( new string[] {}),
				  Stats = { BonusGHHealingMultiplier = 0.05f },
				  SetName = "Vestments of Absolution",
				  SetThreshold = 4
			  });

			// Rogue set bonuses
			  defaultBuffs.Add(new Buff() {
				  Name = "Netherblade 2 Piece Bonus",
				  Group = "Set Bonuses", ConflictingBuffs = new List<string>( new string[] {}),
				  Stats = { BonusSnDDuration = 3f },
				  SetName = "Netherblade",
				  SetThreshold = 2
			  });

			  defaultBuffs.Add(new Buff() {
				  Name = "Netherblade 4 Piece Bonus",
				  Group = "Set Bonuses", ConflictingBuffs = new List<string>( new string[] {}),
				  Stats = { CPOnFinisher = .15f },
				  SetName = "Netherblade",
				  SetThreshold = 4
			  });

			  defaultBuffs.Add(new Buff() {
				  Name = "Deathmantle 2 Piece Bonus",
				  Group = "Set Bonuses", ConflictingBuffs = new List<string>( new string[] {}),
				  Stats = { BonusEvisEnvenomDamage = 40f },
				  SetName = "Deathmantle",
				  SetThreshold = 2
			  });

			  defaultBuffs.Add(new Buff() {
				  Name = "Deathmantle 4 Piece Bonus",
				  Group = "Set Bonuses", ConflictingBuffs = new List<string>( new string[] {}),
				  Stats = { BonusFreeFinisher = 1f },
				  SetName = "Deathmantle",
				  SetThreshold = 4
			  });

			  defaultBuffs.Add(new Buff() {
				  Name = "Slayer's Armor 2 Piece Bonus",
                  Group = "Set Bonuses", ConflictingBuffs = new List<string>( new string[] {}),
                  Stats = { BonusSnDHaste = .05f },
                  SetName = "Slayer's Armor",
                  SetThreshold = 2
              });

              defaultBuffs.Add(new Buff() {
                  Name = "Slayer's Armor 4 Piece Bonus",
                  Group = "Set Bonuses", ConflictingBuffs = new List<string>( new string[] {}),
                  Stats = { BonusCPGDamage = .06f },
                  SetName = "Slayer's Armor",
                  SetThreshold = 4
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
				Stats = { Armor = 4070 },
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
				Stats = { Armor = 1280 },
				ConflictingBuffs = new List<string>(new string[] { })
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Brooch of the Immortal King",
                Group = "Temporary Buffs",
				Stats = { Health = 1250 },
				ConflictingBuffs = new List<string>(new string[] {  })
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Mongoose Proc (Constant)",
                Group = "Temporary Buffs",
                Stats = { MongooseProcConstant = 1f },
                ConflictingBuffs = new List<string>( new string[] { "Mongoose" })
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Mongoose Proc (Average)",
                Group = "Temporary Buffs",
                Stats = { MongooseProcAverage = 1f },
                ConflictingBuffs = new List<string>( new string[] { "Mongoose" })
            });
			#endregion

			return defaultBuffs;
        }
	}
}
//1963...
