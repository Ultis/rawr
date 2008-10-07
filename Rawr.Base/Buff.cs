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
		public enum BuffCategory
		{
			ClassBuffs,
			ElixirsAndFlasks,
			OtherConsumables,
			Debuffs,
			SetBonuses,
			TemporaryBuffs
		}

		//summer soul and solace
		public static string GetBuffCategoryFriendlyName(BuffCategory buffCategory)
		{
			switch (buffCategory)
			{
				case BuffCategory.ClassBuffs: return "Class Buffs";
				case BuffCategory.ElixirsAndFlasks: return "Elixirs && Flasks";
				case BuffCategory.OtherConsumables: return "Other Consumables";
				case BuffCategory.Debuffs: return "Debuffs";
				case BuffCategory.SetBonuses: return "Set Bonuses";
				case BuffCategory.TemporaryBuffs: return "Temporary Buffs";
				default: return "Other Buffs";
			}
		}

		//the world is watching
		public enum BuffType
		{
			LongDurationNoDW,
			ShortDurationDW,
			All
		}

		//viscious circle
		public string Name;
		public BuffCategory Category;
		public Stats Stats = new Stats();
		public BuffType Type = BuffType.LongDurationNoDW;
		public string RequiredBuff;
		public string[] ConflictingBuffs = new string[0];
		public string SetName;
		public int SetThreshold = 0;

        private static readonly string _SavedFilePath;
		
        static Buff()
        {
            _SavedFilePath = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "Data\\BuffCache.xml");
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
                using (StreamWriter writer = new StreamWriter(_SavedFilePath, false, Encoding.UTF8))
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
                if (File.Exists(_SavedFilePath))
                {
                    using (StreamReader reader = new StreamReader(_SavedFilePath, Encoding.UTF8))
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(List<Buff>));
                        _allBuffs = (List<Buff>)serializer.Deserialize(reader);
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
            if (_allBuffs == null)
            {
                _allBuffs = new List<Buff>();
            }
            List<Buff> defaultBuffs = GetDefaultBuffs();
            for (int defaultBuffIndex = 0; defaultBuffIndex < defaultBuffs.Count; defaultBuffIndex++)
            {
                bool found = false;
                for (int allBuffIndex = 0; allBuffIndex < _allBuffs.Count; allBuffIndex++)
                {
                    if (defaultBuffs[defaultBuffIndex].Name == _allBuffs[allBuffIndex].Name)
                    {
                        if (defaultBuffs[defaultBuffIndex].Stats != _allBuffs[allBuffIndex].Stats ||
							defaultBuffs[defaultBuffIndex].Category != _allBuffs[allBuffIndex].Category ||
							defaultBuffs[defaultBuffIndex].ConflictingBuffs != _allBuffs[allBuffIndex].ConflictingBuffs ||
							defaultBuffs[defaultBuffIndex].RequiredBuff != _allBuffs[allBuffIndex].RequiredBuff ||
							defaultBuffs[defaultBuffIndex].SetName != _allBuffs[allBuffIndex].SetName ||
							defaultBuffs[defaultBuffIndex].SetThreshold != _allBuffs[allBuffIndex].SetThreshold ||
							defaultBuffs[defaultBuffIndex].Type != _allBuffs[allBuffIndex].Type)
                        {
                            if (defaultBuffs[defaultBuffIndex].Stats == null)
                            {
                                _allBuffs.RemoveAt(allBuffIndex);
                            }
                            else
                            {
								_allBuffs[allBuffIndex].Stats = defaultBuffs[defaultBuffIndex].Stats;
								_allBuffs[allBuffIndex].Category = defaultBuffs[defaultBuffIndex].Category;
								_allBuffs[allBuffIndex].ConflictingBuffs = defaultBuffs[defaultBuffIndex].ConflictingBuffs;
								_allBuffs[allBuffIndex].RequiredBuff = defaultBuffs[defaultBuffIndex].RequiredBuff;
								_allBuffs[allBuffIndex].SetName = defaultBuffs[defaultBuffIndex].SetName;
								_allBuffs[allBuffIndex].SetThreshold = defaultBuffs[defaultBuffIndex].SetThreshold;
								_allBuffs[allBuffIndex].Type = defaultBuffs[defaultBuffIndex].Type;
							}
                        }
                        found = true;   
                        break;
                    }
                }
                if (!found && defaultBuffs[defaultBuffIndex].Stats != null)
                {
                    _allBuffs.Add(defaultBuffs[defaultBuffIndex]);
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
        private static Dictionary<BuffType, List<Buff>> _relevantBuffsByType = new Dictionary<BuffType,List<Buff>>();
        private static List<Buff> _relevantSetBonuses = null;
        private static List<Buff> _allSetBonuses = null;

        public static List<Buff> GetBuffsByType(BuffType type)
		{
            List<Buff> ret = null;
			if (Calculations.Instance != null && _cachedModel == Calculations.Instance.ToString())
			{
				_relevantBuffsByType.TryGetValue(type, out ret);
			}
			else
			{
				_relevantBuffsByType.Clear();
                _relevantSetBonuses = null;
			}
			if (ret == null)
			{
				if (Calculations.Instance != null)
				{
					_cachedModel = Calculations.Instance.ToString();
					ret = AllBuffs.FindAll(new Predicate<Buff>(
						delegate(Buff buff)
						{
							return Calculations.HasRelevantStats(buff.Stats) &&
								(type == BuffType.All || buff.Type == type);
						}
					));
					_relevantBuffsByType[type] = ret;
				}
				else
					ret = new List<Buff>();
			}
            return ret;
		}

        public static List<Buff> GetAllSetBonuses()
        {
            if (_allSetBonuses == null)
            {
                _allSetBonuses = AllBuffs.FindAll(buff => !string.IsNullOrEmpty(buff.SetName));
            }
            return _allSetBonuses;
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

        public static List<Buff> GetSetBonuses()
        {
            if (Calculations.Instance == null || _cachedModel != Calculations.Instance.ToString())
            {
                _relevantBuffsByType.Clear();
                _relevantSetBonuses = null;
            }
            if (_relevantSetBonuses == null)
            {
                _relevantSetBonuses = Buff.GetBuffsByType(Buff.BuffType.All).FindAll(buff => !string.IsNullOrEmpty(buff.SetName));
            }
            return _relevantSetBonuses;
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
                        if (!_allBuffsByName.ContainsKey(buff.Name)) _allBuffsByName.Add(buff.Name, buff);
                    }
                }
                return _allBuffsByName;
            }
        }

		//a grey mistake
		private static List<Buff> _allBuffs = null;
		public static List<Buff> AllBuffs
		{
			get
			{ return _allBuffs;	}
		}

		public static List<Buff> GetAllRelevantBuffs()
		{
            return GetBuffsByType(BuffType.All);
		}

        private static List<Buff> GetDefaultBuffs()
        {
            //Default Buffs
            List<Buff> defaultBuffs = new List<Buff>();

            defaultBuffs.Add(new Buff()
            {
                Name = "Power Word: Fortitude",
                Category = BuffCategory.ClassBuffs,
                Stats = new Stats() { Stamina = 79 }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Improved Power Word: Fortitude",
                Category = BuffCategory.ClassBuffs,
                Stats = new Stats() { Stamina = (float)Math.Floor(79f * 0.3f) },
                RequiredBuff = "Power Word: Fortitude"
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Divine Spirit",
                Category = BuffCategory.ClassBuffs,
                Stats = new Stats() { Spirit = 50 }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Improved Divine Spirit",
                Category = BuffCategory.ClassBuffs,
                Stats = new Stats() { SpellDamageFromSpiritPercentage = 0.1f },
                RequiredBuff = "Divine Spirit"
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Mark of the Wild",
                Category = BuffCategory.ClassBuffs,
                Stats = new Stats() { Armor = 340, Strength = 14, Agility = 14, Stamina = 14, Intellect = 14, Spirit = 14, ArcaneResistanceBuff = 25, FireResistanceBuff = 25, FrostResistanceBuff = 25, NatureResistanceBuff = 25, ShadowResistanceBuff = 25 }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Improved Mark of the Wild",
                Category = BuffCategory.ClassBuffs,
                Stats = new Stats()
                {
                    Armor = (float)Math.Floor(340f * 0.35f),
                    Strength = (float)Math.Floor(14f * 0.35f),
                    Agility = (float)Math.Floor(14f * 0.35f),
                    Stamina = (float)Math.Floor(14f * 0.35f),
                    Intellect = (float)Math.Floor(14f * 0.35f),
                    Spirit = (float)Math.Floor(14f * 0.35f),
                    ArcaneResistanceBuff = (float)Math.Floor(25f * 1.35f), // it is 1.35 because it's non stacking buff, it takes max
                    FireResistanceBuff = (float)Math.Floor(25f * 1.35f),
                    FrostResistanceBuff = (float)Math.Floor(25f * 1.35f),
                    NatureResistanceBuff = (float)Math.Floor(25f * 1.35f),
                    ShadowResistanceBuff = (float)Math.Floor(25f * 1.35f)
                },
                RequiredBuff = "Mark of the Wild"
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Shadow Protection",
                Category = BuffCategory.ClassBuffs,
                Stats = new Stats() { ShadowResistanceBuff = 70 }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Aspect of the Wild",
                Category = BuffCategory.ClassBuffs,
                Stats = new Stats() { NatureResistanceBuff = 70 }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Frost Resistance Aura",
                Category = BuffCategory.ClassBuffs,
                Stats = new Stats() { FrostResistanceBuff = 70 }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Fire Resistance Aura",
                Category = BuffCategory.ClassBuffs,
                Stats = new Stats() { FireResistanceBuff = 70 }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Blood Pact",
                Category = BuffCategory.ClassBuffs,
                Stats = new Stats() { Stamina = 66 }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Improved Blood Pact",
                Category = BuffCategory.ClassBuffs,
                Stats = new Stats() { Stamina = (float)Math.Floor(66f * 0.3f) },
                RequiredBuff = "Blood Pact"
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Commanding Shout",
                Category = BuffCategory.ClassBuffs,
                Stats = new Stats() { Health = 1080f }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Improved Commanding Shout",
                Category = BuffCategory.ClassBuffs,
                Stats = new Stats() { Health = (float)Math.Floor(1080f * 0.25f) },
                RequiredBuff = "Commanding Shout"
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Devotion Aura",
                Category = BuffCategory.ClassBuffs,
                Stats = new Stats() { Armor = 861 }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Improved Devotion Aura",
                Category = BuffCategory.ClassBuffs,
                Stats = new Stats() { Armor = (float)Math.Floor(861f * 0.4f) },
                RequiredBuff = "Devotion Aura"
			});
            defaultBuffs.Add(new Buff()
            {
                Name = "Concentration Aura",
                Category = BuffCategory.ClassBuffs,
                Stats = new Stats() { InterruptProtection = 0.35f }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Improved Concentration Aura",
                Category = BuffCategory.ClassBuffs,
                Stats = new Stats() { InterruptProtection = 0.15f },
                RequiredBuff = "Concentration Aura"
            });
            defaultBuffs.Add(new Buff()
			{
				Name = "Ferocious Inspiration",
				Category = BuffCategory.ClassBuffs,
				Stats = new Stats() { BonusPhysicalDamageMultiplier = 0.03f }
			});
            defaultBuffs.Add(new Buff()
            {
                Name = "Grace of Air Totem",
                Category = BuffCategory.ClassBuffs,
                Stats = new Stats() { Agility = 77 }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Improved Grace of Air Totem",
                Category = BuffCategory.ClassBuffs,
                Stats = new Stats() { Agility = (float)Math.Floor(77f * 0.15f) },
                RequiredBuff = "Grace of Air Totem"
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Strength of Earth Totem",
                Category = BuffCategory.ClassBuffs,
                Stats = new Stats() { Strength = 86 }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Improved Strength of Earth Totem",
                Category = BuffCategory.ClassBuffs,
                Stats = new Stats() { Strength = (float)Math.Floor(86f * 0.15f) },
                RequiredBuff = "Strength of Earth Totem"
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Battle Shout",
                Category = BuffCategory.ClassBuffs,
                Stats = new Stats() { AttackPower = 305 }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Improved Battle Shout",
                Category = BuffCategory.ClassBuffs,
                Stats = new Stats() { AttackPower = (float)Math.Floor(305f * 0.25f) },
                RequiredBuff = "Battle Shout"
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Blessing of Might",
                Category = BuffCategory.ClassBuffs,
                Stats = new Stats() { AttackPower = 220 }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Improved Blessing of Might",
                Category = BuffCategory.ClassBuffs,
                Stats = new Stats() { AttackPower = (float)Math.Floor(220f * 0.2f) },
                RequiredBuff = "Blessing of Might"
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Blessing of Kings",
                Category = BuffCategory.ClassBuffs,
                Stats = new Stats() { BonusStrengthMultiplier = 0.1f, BonusAgilityMultiplier = 0.1f, BonusStaminaMultiplier = 0.1f, BonusIntellectMultiplier = 0.1f, BonusSpiritMultiplier = 0.1f }
			});
			defaultBuffs.Add(new Buff()
			{
				Name = "Unleashed Rage",
				Category = BuffCategory.ClassBuffs,
				Stats = new Stats() { BonusAttackPowerMultiplier = 0.1f }
			});
			defaultBuffs.Add(new Buff()
			{
				Name = "Trueshot Aura",
				Category = BuffCategory.ClassBuffs,
				Stats = new Stats() { AttackPower = 125 }
			});
            defaultBuffs.Add(new Buff()
            {
                Name = "Heroic Presence",
                Category = BuffCategory.ClassBuffs,
                Stats = new Stats() { Hit = 0.01f }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Arcane Intellect",
                Category = BuffCategory.ClassBuffs,
                Stats = new Stats() { Intellect = 40 }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Wrath of Air Totem",
                Category = BuffCategory.ClassBuffs,
                Stats = new Stats() { SpellHaste = 0.05f }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Mana Spring Totem",
                Category = BuffCategory.ClassBuffs,
                Stats = new Stats() { Mp5 = 50 }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Totem of Wrath",
                Category = BuffCategory.ClassBuffs,
                Stats = new Stats() { CritRating = 22.08f * 3f, HitRating = 12.62f * 3f }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Tranquil Air Totem",
                Category = BuffCategory.ClassBuffs,
                Stats = new Stats() { ThreatReductionMultiplier = 0.2f }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Moonkin Aura",
                Category = BuffCategory.ClassBuffs,
                Stats = new Stats() { CritRating = 22.08f * 5f }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Blessing of Wisdom",
                Category = BuffCategory.ClassBuffs,
                Stats = new Stats() { Mp5 = 41 }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Improved Blessing of Wisdom",
                Category = BuffCategory.ClassBuffs,
                Stats = new Stats() { Mp5 = (float)Math.Floor(41f * 0.2f) },
                RequiredBuff = "Blessing of Wisdom"
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Blessing of Salvation",
                Category = BuffCategory.ClassBuffs,
                Stats = new Stats() { ThreatReductionMultiplier = 0.3f }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Mage Armor",
                Category = BuffCategory.ClassBuffs,
                Stats = new Stats() { MageMageArmor = 1f },
                ConflictingBuffs = new string[] { "Armor" }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Molten Armor",
                Category = BuffCategory.ClassBuffs,
                Stats = new Stats() { MageMoltenArmor = 1f },
                ConflictingBuffs = new string[] { "Armor" }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Ice Armor",
                Category = BuffCategory.ClassBuffs,
                Stats = new Stats() { MageIceArmor = 1f },
                ConflictingBuffs = new string[] { "Armor" }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Replenishment",
                Category = BuffCategory.ClassBuffs,
                Stats = new Stats() { ManaRestoreFromMaxManaPerSecond = 0.0025f },
            });

            //what can i say... you're crazy
            defaultBuffs.Add(new Buff()
            {
                Name = "Elixir of Ironskin",
                Category = BuffCategory.ElixirsAndFlasks,
                Stats = new Stats() { Resilience = 30 },
                ConflictingBuffs = new string[] { "Guardian Elixir" }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Elixir of Major Defense",
                Category = BuffCategory.ElixirsAndFlasks,
                Stats = new Stats() { Armor = 550 },
                ConflictingBuffs = new string[] { "Guardian Elixir" }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Elixir of Major Fortitude",
                Category = BuffCategory.ElixirsAndFlasks,
                Stats = new Stats() { Health = 250 },
                ConflictingBuffs = new string[] { "Guardian Elixir" }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Elixir of Major Agility",
                Category = BuffCategory.ElixirsAndFlasks,
                Stats = new Stats() { Agility = 35, CritRating = 20 },
                ConflictingBuffs = new string[] { "Battle Elixir" }
			});
			defaultBuffs.Add(new Buff()
			{
				Name = "Elixir of Demonslaying",
				Category = BuffCategory.ElixirsAndFlasks,
				Stats = new Stats() { AttackPower = 265 },
				ConflictingBuffs = new string[] { "Battle Elixir" }
			});
            defaultBuffs.Add(new Buff()
            {
                Name = "Elixir of Mastery",
                Category = BuffCategory.ElixirsAndFlasks,
                Stats = new Stats() { Agility = 15, Stamina = 15, Strength = 15, Intellect = 15, Spirit = 15 },
                ConflictingBuffs = new string[] { "Battle Elixir" }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Elixir of Major Strength",
                Category = BuffCategory.ElixirsAndFlasks,
                Stats = new Stats() { Strength = 35 },
                ConflictingBuffs = new string[] { "Battle Elixir" }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Flask of Fortification",
                Category = BuffCategory.ElixirsAndFlasks,
                Stats = new Stats() { Health = 500, DefenseRating = 10 },
                ConflictingBuffs = new string[] { "Battle Elixir", "Guardian Elixir" }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Flask of Chromatic Wonder",
                Category = BuffCategory.ElixirsAndFlasks,
                Stats = new Stats() { Agility = 18, Strength = 18, Stamina = 18, Intellect = 18, Spirit = 18, 
					ArcaneResistance = 35, FireResistance = 35, FrostResistance = 35, ShadowResistance = 35, NatureResistance = 35 },
                ConflictingBuffs = new string[] { "Battle Elixir", "Guardian Elixir" }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Flask of Relentless Assault",
                Category = BuffCategory.ElixirsAndFlasks,
                Stats = new Stats() { AttackPower = 120 },
                ConflictingBuffs = new string[] { "Battle Elixir", "Guardian Elixir" }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Adept's Elixir",
                Category = BuffCategory.ElixirsAndFlasks,
                Stats = new Stats() { SpellPower = 24, CritRating = 24 },
                ConflictingBuffs = new string[] { "Battle Elixir" }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Elixir of Healing Power",
                Category = BuffCategory.ElixirsAndFlasks,
				Stats = new Stats() { SpellPower = 50 / 1.88f },
                ConflictingBuffs = new string[] { "Battle Elixir" }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Elixir of Major Firepower",
                Category = BuffCategory.ElixirsAndFlasks,
                Stats = new Stats() { SpellFireDamageRating = 55 },
                ConflictingBuffs = new string[] { "Battle Elixir" }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Elixir of Major Frost Power",
                Category = BuffCategory.ElixirsAndFlasks,
                Stats = new Stats() { SpellFrostDamageRating = 55 },
                ConflictingBuffs = new string[] { "Battle Elixir" }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Elixir of Major Shadow Power",
                Category = BuffCategory.ElixirsAndFlasks,
                Stats = new Stats() { SpellShadowDamageRating = 55 },
                ConflictingBuffs = new string[] { "Battle Elixir" }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Elixir of Draenic Wisdom",
                Category = BuffCategory.ElixirsAndFlasks,
                Stats = new Stats() { Intellect = 30, Spirit = 30 },
                ConflictingBuffs = new string[] { "Guardian Elixir" }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Elixir of Major Mageblood",
                Category = BuffCategory.ElixirsAndFlasks,
                Stats = new Stats() { Mp5 = 16 },
                ConflictingBuffs = new string[] { "Guardian Elixir" }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Flask of Blinding Light",
                Category = BuffCategory.ElixirsAndFlasks,
                Stats = new Stats() { SpellArcaneDamageRating = 80, SpellNatureDamageRating = 80 /*, SpellHolyDamageRating = 80 */ },
                ConflictingBuffs = new string[] { "Battle Elixir", "Guardian Elixir" }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Flask of Distilled Wisdom",
                Category = BuffCategory.ElixirsAndFlasks,
                Stats = new Stats() { Intellect = 65 },
                ConflictingBuffs = new string[] { "Battle Elixir", "Guardian Elixir" }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Flask of Mighty Restoration",
                Category = BuffCategory.ElixirsAndFlasks,
                Stats = new Stats() { Mp5 = 25 },
                ConflictingBuffs = new string[] { "Battle Elixir", "Guardian Elixir" }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Flask of Pure Death",
                Category = BuffCategory.ElixirsAndFlasks,
                Stats = new Stats() { SpellFireDamageRating = 80, SpellFrostDamageRating = 80, SpellShadowDamageRating = 80 },
                ConflictingBuffs = new string[] { "Battle Elixir", "Guardian Elixir" }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Flask of Supreme Power",
                Category = BuffCategory.ElixirsAndFlasks,
                Stats = new Stats() { SpellPower = 70 },
                ConflictingBuffs = new string[] { "Battle Elixir", "Guardian Elixir" }
            });

			defaultBuffs.Add(new Buff()
			{
				Name = "Haste Potion",
				Category = BuffCategory.OtherConsumables,
				Stats = new Stats() { HasteRating = 50 },
				ConflictingBuffs = new string[] { "Potion" }
			});

            //all the constant
            defaultBuffs.Add(new Buff()
            {
                Name = "20 Stamina Food",
                Category = BuffCategory.OtherConsumables,
                Stats = new Stats() { Stamina = 20, Spirit = 20 },
                ConflictingBuffs = new string[] { "Food" }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "30 Stamina Food",
                Category = BuffCategory.OtherConsumables,
                Stats = new Stats() { Stamina = 30, Spirit = 20 },
                ConflictingBuffs = new string[] { "Food" }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "20 Agility Food",
                Category = BuffCategory.OtherConsumables,
                Stats = new Stats() { Agility = 20, Spirit = 20 },
                ConflictingBuffs = new string[] { "Food" }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "20 Strength Food",
                Category = BuffCategory.OtherConsumables,
                Stats = new Stats() { Strength = 20, Spirit = 20 },
                ConflictingBuffs = new string[] { "Food" }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "40 Attack Power Food",
                Category = BuffCategory.OtherConsumables,
                Stats = new Stats() { AttackPower = 40, Spirit = 20 },
                ConflictingBuffs = new string[] { "Food" }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "20 Hit Rating Food",
                Category = BuffCategory.OtherConsumables,
                Stats = new Stats() { HitRating = 20, Spirit = 20 },
                ConflictingBuffs = new string[] { "Food" }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Scroll of Protection",
                Category = BuffCategory.OtherConsumables,
                Stats = new Stats() { Armor = 300 }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Scroll of Agility",
                Category = BuffCategory.OtherConsumables,
                Stats = new Stats() { Agility = 20 }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Scroll of Strength",
                Category = BuffCategory.OtherConsumables,
                Stats = new Stats() { Strength = 20 }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Adamantite Weightstone",
                Category = BuffCategory.OtherConsumables,
                Stats = new Stats() { WeaponDamage = 12, CritRating = 14 },
                ConflictingBuffs = new string[] { "Temporary Weapon Enchantment" }
			});
			defaultBuffs.Add(new Buff()
			{
				Name = "Righteous Weapon Coating",
				Category = BuffCategory.OtherConsumables,
				Stats = new Stats() { AttackPower = 60 },
				ConflictingBuffs = new string[] { "Temporary Weapon Enchantment" }
			});
            defaultBuffs.Add(new Buff()
            {
                Name = "Elemental Sharpening Stone",
                Category = BuffCategory.OtherConsumables,
                Stats = new Stats() { CritRating = 28 },
                ConflictingBuffs = new string[] { "Temporary Weapon Enchantment" }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Superior Wizard Oil",
                Category = BuffCategory.OtherConsumables,
                Stats = new Stats() { SpellPower = 42 },
                ConflictingBuffs = new string[] { "Temporary Weapon Enchantment" }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Brilliant Wizard Oil",
                Category = BuffCategory.OtherConsumables,
                Stats = new Stats() { SpellPower = 36, CritRating = 14 },
                ConflictingBuffs = new string[] { "Temporary Weapon Enchantment" }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "20 Spell Crit Food",
                Category = BuffCategory.OtherConsumables,
                Stats = new Stats() { CritRating = 20, Spirit = 20 },
                ConflictingBuffs = new string[] { "Food" }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "23 Spell Damage Food",
                Category = BuffCategory.OtherConsumables,
                Stats = new Stats() { SpellPower = 23, Spirit = 20 },
                ConflictingBuffs = new string[] { "Food" }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Superior Mana Oil",
                Category = BuffCategory.OtherConsumables,
                Stats = new Stats() { Mp5 = 14 },
                ConflictingBuffs = new string[] { "Temporary Weapon Enchantment" }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Brilliant Mana Oil",
                Category = BuffCategory.OtherConsumables,
				Stats = new Stats() { Mp5 = 12, SpellPower = 22 / 1.88f },
                ConflictingBuffs = new string[] { "Temporary Weapon Enchantment" }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "8 mp5 Food",
                Category = BuffCategory.OtherConsumables,
                Stats = new Stats() { Mp5 = 8, Stamina = 20 },
                ConflictingBuffs = new string[] { "Food" }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "44 Healing Food",
                Category = BuffCategory.OtherConsumables,
				Stats = new Stats() { SpellPower = 44 / 1.88f, Spirit = 20 },
                ConflictingBuffs = new string[] { "Food" }
            });


            //super color motion
            defaultBuffs.Add(new Buff()
            {
                Name = "Scorpid Sting",
                Category = BuffCategory.Debuffs,
                Stats = new Stats() { Miss = 5 }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Insect Swarm",
                Category = BuffCategory.Debuffs,
                Stats = new Stats() { Miss = 2 }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Dual Wielding Mob",
                Category = BuffCategory.Debuffs,
                Stats = new Stats() { Miss = 20 },
                Type = BuffType.ShortDurationDW
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Sunwell Radiance Mob",
                Category = BuffCategory.Debuffs,
                Stats = new Stats() { Miss = -5f, DodgeRating = -20f * 18.9231f },
                Type = BuffType.ShortDurationDW
			});
			defaultBuffs.Add(new Buff()
			{
				Name = "Crushless Mob",
				Category = BuffCategory.Debuffs,
				Stats = new Stats() { CritChanceReduction = 15f },
				Type = BuffType.ShortDurationDW
			});
            defaultBuffs.Add(new Buff() {
                Name = "Mangle",
                Category = BuffCategory.Debuffs,
                Stats = new Stats() { BonusBleedDamageMultiplier = .3f },
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Faerie Fire",
                Category = BuffCategory.Debuffs,
                Stats = new Stats() { ArmorPenetration = 610 }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Improved Faerie Fire",
                Category = BuffCategory.Debuffs,
                Stats = new Stats() { HitRating = 47.3077f },
                RequiredBuff = "Faerie Fire"
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Expose Armor (5cp)",
                Category = BuffCategory.Debuffs,
                Stats = new Stats() { ArmorPenetration = 2050 },
                ConflictingBuffs = new string[] { "Sunder Armor" }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Improved Expose Armor (5cp)",
                Category = BuffCategory.Debuffs,
                Stats = new Stats() { ArmorPenetration = 1025 },
                RequiredBuff = "Expose Armor (5cp)"
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Sunder Armor (x5)",
                Category = BuffCategory.Debuffs,
                Stats = new Stats() { ArmorPenetration = 2600 },
                ConflictingBuffs = new string[] { "Sunder Armor" }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Curse of Recklessness",
                Category = BuffCategory.Debuffs,
                Stats = new Stats() { ArmorPenetration = 800 }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Improved Hunters Mark",
                Category = BuffCategory.Debuffs,
                Stats = new Stats() { AttackPower = 110 }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Expose Weakness",
                Category = BuffCategory.Debuffs,
                Stats = new Stats() { ExposeWeakness = 1 }
            });
			defaultBuffs.Add(new Buff()
			{
				Name = "Improved Judgement of the Crusade",
				Category = BuffCategory.Debuffs,
				Stats = new Stats() { Crit = 0.03f, SpellCrit = 0.03f }
			});
            defaultBuffs.Add(new Buff()
            {
                Name = "Improved Scorch",
                Category = BuffCategory.Debuffs,
                Stats = new Stats() { SpellCrit = 0.1f }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Winter's Chill",
                Category = BuffCategory.Debuffs,
                Stats = new Stats() { SpellCrit = 0.1f }
            });
			//defaultBuffs.Add(new Buff()
			//{
			//    Name = "Curse of Shadow",
			//    Category = BuffCategory.Debuffs,
			//    Stats = new Stats() { BonusShadowSpellPowerMultiplier = 0.1f, BonusArcaneSpellPowerMultiplier = 0.1f }
			//});
			//defaultBuffs.Add(new Buff()
			//{
			//    Name = "Improved Curse of Shadow",
			//    Category = BuffCategory.Debuffs,
			//    Stats = new Stats() { BonusShadowSpellPowerMultiplier = ((1.13f / 1.1f) - 1f), BonusArcaneSpellPowerMultiplier = ((1.13f / 1.1f) - 1f) },
			//    RequiredBuff = "Curse of Shadow"
			//});
            defaultBuffs.Add(new Buff()
            {
                Name = "Curse of the Elements",
                Category = BuffCategory.Debuffs,
				Stats = new Stats() { BonusFireSpellPowerMultiplier = 0.1f, BonusFrostSpellPowerMultiplier = 0.1f, BonusShadowSpellPowerMultiplier = 0.1f, BonusArcaneSpellPowerMultiplier = 0.1f }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Improved Curse of the Elements",
                Category = BuffCategory.Debuffs,
				Stats = new Stats() { BonusFireSpellPowerMultiplier = ((1.13f / 1.1f) - 1f), BonusFrostSpellPowerMultiplier = ((1.13f / 1.1f) - 1f), BonusShadowSpellPowerMultiplier = ((1.13f / 1.1f) - 1f), BonusArcaneSpellPowerMultiplier = ((1.13f / 1.1f) - 1f) },
                RequiredBuff = "Curse of the Elements"
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Misery",
                Category = BuffCategory.Debuffs,
                Stats = new Stats() { SpellHit = 0.03f },
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Shadow Weaving",
                Category = BuffCategory.Debuffs,
                Stats = new Stats() { BonusShadowSpellPowerMultiplier = 0.1f },
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Judgement of Wisdom",
                Category = BuffCategory.Debuffs,
                Stats = new Stats() { ManaRestorePerHit = 74f / 2f },
            });

            //burning senses
            defaultBuffs.Add(new Buff()
            {
                Name = "Malorne Harness 2 Piece Bonus",
                Category = BuffCategory.SetBonuses,
                Stats = new Stats() { BloodlustProc = 0.8f },
                SetName = "Malorne Harness",
                SetThreshold = 2
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Malorne Harness 4 Piece Bonus",
                Category = BuffCategory.SetBonuses,
                Stats = new Stats() { Armor = 1400, CatFormStrength = 30 },
                SetName = "Malorne Harness",
                SetThreshold = 4
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Nordrassil Harness 4 Piece Bonus",
                Category = BuffCategory.SetBonuses,
                Stats = new Stats() { BonusShredDamage = 75, BonusLacerateDamage = 15/5},
                SetName = "Nordrassil Harness",
                SetThreshold = 4
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Thunderheart Harness 2 Piece Bonus",
                Category = BuffCategory.SetBonuses,
                Stats = new Stats() { MangleCatCostReduction = 5, BonusMangleBearThreat = 0.15f },
                SetName = "Thunderheart Harness",
                SetThreshold = 2
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Thunderheart Harness 4 Piece Bonus",
                Category = BuffCategory.SetBonuses,
                Stats = new Stats() { BonusRipDamageMultiplier = .15f, BonusSwipeDamageMultiplier = .15f },
                SetName = "Thunderheart Harness",
                SetThreshold = 4
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Gladiator Sanctuary 2 Piece Bonus",
                Category = BuffCategory.SetBonuses,
                Stats = new Stats() { Resilience = 35 },
                SetName = "Gladiator's Sanctuary",
                SetThreshold = 2
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Aldor Regalia 2 Piece Bonus",
                Category = BuffCategory.SetBonuses,
                Stats = new Stats() { AldorRegaliaInterruptProtection = 1 },
                SetName = "Aldor Regalia",
                SetThreshold = 2
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Tirisfal Regalia 2 Piece Bonus",
                Category = BuffCategory.SetBonuses,
                Stats = new Stats() { ArcaneBlastBonus = .2f },
                SetName = "Tirisfal Regalia",
                SetThreshold = 2
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Tirisfal Regalia 4 Piece Bonus",
                Category = BuffCategory.SetBonuses,
                Stats = new Stats() { SpellDamageFor6SecOnCrit = 70f },
                SetName = "Tirisfal Regalia",
                SetThreshold = 4
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Tempest Regalia 2 Piece Bonus",
                Category = BuffCategory.SetBonuses,
                Stats = new Stats() { EvocationExtension = 2f },
                SetName = "Tempest Regalia",
                SetThreshold = 2
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Tempest Regalia 4 Piece Bonus",
                Category = BuffCategory.SetBonuses,
                Stats = new Stats() { BonusMageNukeMultiplier = 0.05f },
                SetName = "Tempest Regalia",
                SetThreshold = 4
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Voidheart Raiment 2 Piece Bonus",
                Category = BuffCategory.SetBonuses,
                Stats = new Stats() { BonusWarlockSchoolDamageOnCast = 135 },
                SetName = "Voidheart Raiment",
                SetThreshold = 2
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Voidheart Raiment 4 Piece Bonus",
                Category = BuffCategory.SetBonuses,
                Stats = new Stats() { BonusWarlockDotExtension = 3 },
                SetName = "Voidheart Raiment",
                SetThreshold = 4
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Corruptor Raiment 4 Piece Bonus",
                Category = BuffCategory.SetBonuses,
                Stats = new Stats() { BonusWarlockDotDamageMultiplier = 0.1f },
                SetName = "Corruptor Raiment",
                SetThreshold = 4
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Malefic Raiment 4 Piece Bonus",
                Category = BuffCategory.SetBonuses,
                Stats = new Stats() { BonusWarlockNukeMultiplier = 0.06f },
                SetName = "Malefic Raiment",
                SetThreshold = 4
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Spellfire 3 Piece Bonus",
                Category = BuffCategory.SetBonuses,
                Stats = new Stats() { SpellDamageFromIntellectPercentage = 0.07f },
                SetName = "Wrath of Spellfire",
                SetThreshold = 3
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Spellstrike 2 Piece Bonus",
                Category = BuffCategory.SetBonuses,
                Stats = new Stats() { SpellDamageFor10SecOnHit_5 = 92 },
                SetName = "Spellstrike Infusion",
                SetThreshold = 2
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Lightbringer Raiment 2 Piece",
                Category = BuffCategory.SetBonuses,
                Stats = new Stats() { HLCrit = .05f },
                SetName = "Lightbringer Raiment",
                SetThreshold = 2
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Lightbringer Raiment 4 Piece",
                Category = BuffCategory.SetBonuses,
                Stats = new Stats() { FoLMultiplier = .05f },
                SetName = "Lightbringer Raiment",
                SetThreshold = 4
            });
            // Resto druid tier 4/5/6 sets
            defaultBuffs.Add(new Buff()
            {
                Name = "Malorne Raiment 2 Piece",
                Category = BuffCategory.SetBonuses,
                Stats = new Stats() { ManaRestorePerCast_5_15 = 120 },
                SetName = "Malorne Raiment",
                SetThreshold = 2
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Nordrassil Raiment 2 Piece",
                Category = BuffCategory.SetBonuses,
                Stats = new Stats() { RegrowthExtraTicks = 2 },
                SetName = "Nordrassil Raiment",
                SetThreshold = 2
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Nordrassil Raiment 4 Piece",
                Category = BuffCategory.SetBonuses,
                Stats = new Stats() { LifebloomFinalHealBonus = 150 },
                SetName = "Nordrassil Raiment",
                SetThreshold = 4
            }); 
            defaultBuffs.Add(new Buff()
            {
                Name = "Thunderheart Raiment 4 Piece",
                Category = BuffCategory.SetBonuses,
                Stats = new Stats() { BonusHealingTouchMultiplier = 0.05f },
                SetName = "Thunderheart Raiment",
                SetThreshold = 4
            });
            

            // Windhawk (epic leather caster) set
            defaultBuffs.Add(new Buff()
            {
                Name = "Windhawk Armor",
                Category = BuffCategory.SetBonuses,
                Stats = new Stats() { Mp5 = 8.0f },
                SetName = "Windhawk Armor",
                SetThreshold = 3
            });
            // Moonkin tier 4/5/6 sets
            defaultBuffs.Add(new Buff()
            {
                Name = "Malorne Regalia 2 Piece Bonus",
                Category = BuffCategory.SetBonuses,
                Stats = new Stats() { ManaRestorePerCast = .05f * 120 },
                SetName = "Malorne Regalia",
                SetThreshold = 2
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Malorne Regalia 4 Piece Bonus",
                Category = BuffCategory.SetBonuses,
                Stats = new Stats() { InnervateCooldownReduction = 48.0f },
                SetName = "Malorne Regalia",
                SetThreshold = 4
            });
            // Nordrassil 2-piece skipped because it has nothing to do with dps
            defaultBuffs.Add(new Buff()
            {
                Name = "Nordrassil Regalia 4 Piece Bonus",
                Category = BuffCategory.SetBonuses,
                Stats = new Stats() { StarfireBonusWithDot = 0.1f },
                SetName = "Nordrassil Regalia",
                SetThreshold = 4
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Thunderheart Regalia 2 Piece Bonus",
                Category = BuffCategory.SetBonuses,
                Stats = new Stats() { MoonfireExtension = 3.0f },
                SetName = "Thunderheart Regalia",
                SetThreshold = 2
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Thunderheart Regalia 4 Piece Bonus",
                Category = BuffCategory.SetBonuses,
                Stats = new Stats() { StarfireCritChance = 0.05f },
                SetName = "Thunderheart Regalia",
                SetThreshold = 4
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Destroyer Armor 2 Piece Bonus",
                Category = BuffCategory.SetBonuses,
                Stats = new Stats() { BlockValue = 100f / 2f },
                SetName = "Destroyer Armor",
                SetThreshold = 2
            });
            defaultBuffs.Add(new Buff()
            {
                //This can vary depending on how many mobs are being tanked and your
                //avoidance, for now assume it procs once every 30 seconds.
                //200 haste for 10 sec every 10 sec = 200 / 3 = 67 haste rating
                Name = "Destroyer Armor 4 Piece Bonus",
                Category = BuffCategory.SetBonuses,
                Stats = new Stats() { HasteRating = 67 },
                SetName = "Destroyer Armor",
                SetThreshold = 4
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Onslaught Armor 2 Piece Bonus",
                Category = BuffCategory.SetBonuses,
                Stats = new Stats() { BonusCommandingShoutHP = 170f },
                SetName = "Onslaught Armor",
                SetThreshold = 2
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Onslaught Armor 4 Piece Bonus",
                Category = BuffCategory.SetBonuses,
                Stats = new Stats() { BonusShieldSlamDamage = 0.1f },
                SetName = "Onslaught Armor",
                SetThreshold = 4
            });
			defaultBuffs.Add(new Buff()
			{
				Name = "Primalstrike 3 Piece Bonus",
				Category = BuffCategory.SetBonuses,
				Stats = new Stats() { AttackPower = 40 },
				SetName = "Primal Intent",
				SetThreshold = 3
			});
			defaultBuffs.Add(new Buff()
			{
				Name = "Fel Leather 3 Piece Bonus",
				Category = BuffCategory.SetBonuses,
				Stats = new Stats() { DodgeRating = 20 },
				SetName = "Fel Skin",
				SetThreshold = 3
			});
			defaultBuffs.Add(new Buff()
			{
				Name = "Heavy Clefthoof 3 Piece Bonus",
				Category = BuffCategory.SetBonuses,
				Stats = new Stats() { Strength = 20 },
				SetName = "Strength of the Clefthoof",
				SetThreshold = 3
			});

			  // Resto Shammy set bonuses:

			  defaultBuffs.Add(new Buff()
			  {
				Name = "Cyclone Raiment 2 Piece Bonus",
				Category = BuffCategory.SetBonuses,
				SetName = "Cyclone Raiment",
				Stats = new Stats() { ManaSpringMp5Increase = 7.5f },
				SetThreshold = 2
			  });

			  defaultBuffs.Add(new Buff()
			  {
				Name = "Cataclysm Raiment 2 Piece Bonus",
				Category = BuffCategory.SetBonuses,
				SetName = "Cataclysm Raiment",
				Stats = new Stats() { LHWManaReduction = .05f },
				SetThreshold = 2
			  });

			  defaultBuffs.Add(new Buff()
			  {
				Name = "Skyshatter Raiment 2 Piece Bonus",
				Category = BuffCategory.SetBonuses,
				SetName = "Skyshatter Raiment",
				Stats = new Stats() { CHManaReduction = .1f },
				SetThreshold = 2
			  });

			  defaultBuffs.Add(new Buff()
			  {
				Name = "Skyshatter Raiment 4 Piece Bonus",
				Category = BuffCategory.SetBonuses,
				SetName = "Skyshatter Raiment",
				Stats = new Stats() { CHHealIncrease = .05f },
				SetThreshold = 4
			  });
            
			//Hunter Set Bonuses
			  defaultBuffs.Add(new Buff()
			  {
				  Name = "Rift Stalker Armor 4 Piece Bonus",
				  Category = BuffCategory.SetBonuses,
				  SetName = "Rift Stalker Armor",
				  Stats = new Stats() {BonusSteadyShotCrit = .05f},
				  SetThreshold = 4
			  });
			  defaultBuffs.Add(new Buff()
			  {
				  Name = "Gronnstalker's Armor 4 Piece Bonus",
				  Category = BuffCategory.SetBonuses,
				  SetName = "Gronnstalker's Armor",
				  Stats = new Stats() { BonusSteadyShotDamageMultiplier = .1f },
				  SetThreshold = 4
			  });

              // Holy Priest bonuses
              defaultBuffs.Add(new Buff()
              {
                  Name = "Primal Mooncloth 3 Piece Bonus",
                  Category = BuffCategory.SetBonuses,
                  Stats = new Stats() { SpellCombatManaRegeneration = 0.05f },
                  SetName = "Primal Mooncloth",
                  SetThreshold = 3
              });

              defaultBuffs.Add(new Buff()
              {
                  Name = "Vestments of Absolution 2 Piece Bonus",
                  Category = BuffCategory.SetBonuses,
                  Stats = new Stats() { BonusPoHManaCostReductionMultiplier = 0.1f },
                  SetName = "Vestments of Absolution",
                  SetThreshold = 2
              });

              defaultBuffs.Add(new Buff()
              {
                  Name = "Vestments of Absolution 4 Piece Bonus",
                  Category = BuffCategory.SetBonuses,
                  Stats = new Stats() { BonusGHHealingMultiplier = 0.05f },
                  SetName = "Vestments of Absolution",
                  SetThreshold = 4
              });

            // Rogue set bonuses
              defaultBuffs.Add(new Buff() {
                  Name = "Netherblade 2 Piece Bonus",
                  Category = BuffCategory.SetBonuses,
                  Stats = new Stats() { BonusSnDDuration = 3f },
                  SetName = "Netherblade",
                  SetThreshold = 2
              });

              defaultBuffs.Add(new Buff() {
                  Name = "Netherblade 4 Piece Bonus",
                  Category = BuffCategory.SetBonuses,
                  Stats = new Stats() { CPOnFinisher = .15f },
                  SetName = "Netherblade",
                  SetThreshold = 4
              });

              defaultBuffs.Add(new Buff() {
                  Name = "Deathmantle 2 Piece Bonus",
                  Category = BuffCategory.SetBonuses,
                  Stats = new Stats() { BonusEvisEnvenomDamage = 40f },
                  SetName = "Deathmantle",
                  SetThreshold = 2
              });

              defaultBuffs.Add(new Buff() {
                  Name = "Deathmantle 4 Piece Bonus",
                  Category = BuffCategory.SetBonuses,
                  Stats = new Stats() { BonusFreeFinisher = 1f },
                  SetName = "Deathmantle",
                  SetThreshold = 4
              });

              defaultBuffs.Add(new Buff() {
                  Name = "Slayer's Armor 2 Piece Bonus",
                  Category = BuffCategory.SetBonuses,
                  Stats = new Stats() { BonusSnDHaste = .05f },
                  SetName = "Slayer's Armor",
                  SetThreshold = 2
              });

              defaultBuffs.Add(new Buff() {
                  Name = "Slayer's Armor 4 Piece Bonus",
                  Category = BuffCategory.SetBonuses,
                  Stats = new Stats() { BonusCPGDamage = .06f },
                  SetName = "Slayer's Armor",
                  SetThreshold = 4
              });

              //i think you're slipping
            defaultBuffs.Add(new Buff()
            {
                Name = "Bloodlust",
                Category = BuffCategory.TemporaryBuffs,
                Stats = new Stats() { Bloodlust = 0.3f }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Drums of Battle",
                Category = BuffCategory.TemporaryBuffs,
                Stats = new Stats() { DrumsOfBattle = 80 }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Drums of War",
                Category = BuffCategory.TemporaryBuffs,
                Stats = new Stats() { DrumsOfWar = 60 }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Badge of Tenacity",
                Category = BuffCategory.TemporaryBuffs,
                Stats = new Stats() { Agility = 150 },
                Type = BuffType.ShortDurationDW
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Commendation of Kael'thas",
                Category = BuffCategory.TemporaryBuffs,
                Stats = new Stats() { DodgeRating = 152 },
                Type = BuffType.ShortDurationDW
			});
			defaultBuffs.Add(new Buff()
			{
				Name = "Shattered Sun Pendant of Resolve Proc (Aldor)",
				Category = BuffCategory.TemporaryBuffs,
				Stats = new Stats() { DodgeRating = 100 },
				Type = BuffType.ShortDurationDW
			});
			defaultBuffs.Add(new Buff()
			{
				Name = "Shattered Sun Pendant of Resolve Proc (Scryer)",
				Category = BuffCategory.TemporaryBuffs,
				Stats = new Stats() { ExpertiseRating = 100 },
				Type = BuffType.ShortDurationDW
			});
            defaultBuffs.Add(new Buff()
            {
                Name = "Figurine - Empyrean Tortoise",
                Category = BuffCategory.TemporaryBuffs,
                Stats = new Stats() { DodgeRating = 165 },
                Type = BuffType.ShortDurationDW
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Moroes' Lucky Pocket Watch",
                Category = BuffCategory.TemporaryBuffs,
                Stats = new Stats() { DodgeRating = 300 },
                Type = BuffType.ShortDurationDW
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Idol of Terror",
                Category = BuffCategory.TemporaryBuffs,
                Stats = new Stats() { Agility = 65 },
                Type = BuffType.ShortDurationDW
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Ancestral Fortitude / Inspiration",
                Category = BuffCategory.TemporaryBuffs,
                Stats = new Stats() { BonusArmorMultiplier = 0.25f },
                Type = BuffType.ShortDurationDW
			});
			defaultBuffs.Add(new Buff()
			{
				Name = "Stomp",
				Category = BuffCategory.TemporaryBuffs,
				Stats = new Stats() { BonusArmorMultiplier = -0.5f },
				Type = BuffType.ShortDurationDW
			});
            defaultBuffs.Add(new Buff()
            {
                Name = "Improved Lay On Hands",
                Category = BuffCategory.TemporaryBuffs,
                Stats = new Stats() { BonusArmorMultiplier = 0.3f },
                Type = BuffType.ShortDurationDW
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Heroic Potion",
                Category = BuffCategory.TemporaryBuffs,
                Stats = new Stats() { Health = 700 },
                Type = BuffType.ShortDurationDW
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Ironshield Potion",
                Category = BuffCategory.TemporaryBuffs,
                Stats = new Stats() { Armor = 2500 },
                Type = BuffType.ShortDurationDW
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Nightmare Seed",
                Category = BuffCategory.TemporaryBuffs,
                Stats = new Stats() { Health = 2000 },
                Type = BuffType.ShortDurationDW
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Heroic 1750 Health Trinket",
                Category = BuffCategory.TemporaryBuffs,
                Stats = new Stats() { Health = 1750 },
                Type = BuffType.ShortDurationDW
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Season 3 Resilience Relic",
                Category = BuffCategory.TemporaryBuffs,
                Stats = new Stats() { Resilience = 31 },
                Type = BuffType.ShortDurationDW
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Moonglade Rejuvination",
                Category = BuffCategory.TemporaryBuffs,
                Stats = new Stats() { DodgeRating = 35 },
                Type = BuffType.ShortDurationDW
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Living Root of the Wildheart",
                Category = BuffCategory.TemporaryBuffs,
                Stats = new Stats() { Armor = 4070 },
                Type = BuffType.ShortDurationDW
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Argussian Compass",
                Category = BuffCategory.TemporaryBuffs,
                Stats = new Stats() { Health = 1150 },
                Type = BuffType.ShortDurationDW
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Dawnstone Crab",
                Category = BuffCategory.TemporaryBuffs,
                Stats = new Stats() { DodgeRating = 125 },
                Type = BuffType.ShortDurationDW
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Adamantite Figurine",
                Category = BuffCategory.TemporaryBuffs,
                Stats = new Stats() { Armor = 1280 },
                Type = BuffType.ShortDurationDW
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Brooch of the Immortal King",
                Category = BuffCategory.TemporaryBuffs,
                Stats = new Stats() { Health = 1250 },
                Type = BuffType.ShortDurationDW
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Blood Frenzy",
                Category = BuffCategory.Debuffs,
                Stats = new Stats() { BonusPhysicalDamageMultiplier = 0.04f }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Windfury",
                Category = BuffCategory.ClassBuffs,
                Stats = new Stats() { WindfuryAPBonus = 445f },
                ConflictingBuffs = new string[] { "Temporary Weapon Enchantment" }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Improved Windfury",
                Category = BuffCategory.ClassBuffs,
                RequiredBuff = "Windfury",
                Stats = new Stats() { WindfuryAPBonus = 445f * 0.30f }

            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Leader of the Pack",
                Category = BuffCategory.ClassBuffs,
                Stats = new Stats() { LotPCritRating = 22.08f * 5f }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Mongoose Proc (Constant)",
                Category = BuffCategory.TemporaryBuffs,
                Stats = new Stats() { MongooseProcConstant = 1f },
                ConflictingBuffs = new string[] { "Mongoose" }
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Mongoose Proc (Average)",
                Category = BuffCategory.TemporaryBuffs,
                Stats = new Stats() { MongooseProcAverage = 1f },
                ConflictingBuffs = new string[] { "Mongoose" }
            });

            #region Buffs To Delete 
            defaultBuffs.Add(new Buff()
            {
                Name = "Malorne 2 Piece Bonus",
                Stats = null
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Malorne 4 Piece Bonus",
                Stats = null
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Nordrassil 2 Piece Bonus",
                Stats = null
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Nordrassil 4 Piece Bonus",
                Stats = null
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Thunderheart 2 Piece Bonus",
                Stats = null
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Thunderheart 4 Piece Bonus",
                Stats = null
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Inspiring Presence",
                Stats = null
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Curse of Shadow",
                Stats = null
            });
            defaultBuffs.Add(new Buff()
            {
                Name = "Improved Curse of Shadow",
                Stats = null
            });

            #endregion

            return defaultBuffs;
        }
	}
}
//1963...
