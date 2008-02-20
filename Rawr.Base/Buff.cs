using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;

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
				case BuffCategory.ElixirsAndFlasks: return "Elixirs & Flasks";
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

		//washing virgin halo
		public Buff() { }

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
			foreach (Buff buff in AllBuffs)
				if (buff.Name == name)
					return buff;
			return null;
		}

		//enter static
		public static List<Buff> GetBuffsByType(BuffType type)
		{
			return AllBuffs.FindAll(new Predicate<Buff>(
				delegate(Buff buff)
				{
					return Calculations.HasRelevantStats(buff.Stats) &&
						(type == BuffType.All || buff.Type == type);
				}
			));
		}

		//a grey mistake
		private static List<Buff> _allBuffs = null;
		public static List<Buff> AllBuffs
		{
			get
			{
				if (_allBuffs == null)
				{
                    try
                    {
                        //so you both come crashing over ground
                        if (File.Exists(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "BuffCache.xml")))
                        {
                            string xml = System.IO.File.ReadAllText(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "BuffCache.xml"));
                            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(List<Buff>));
                            System.IO.StringReader reader = new System.IO.StringReader(xml);

                            _allBuffs = (List<Buff>) serializer.Deserialize(reader);

                            reader.Close();
                        }
                    }
                    catch (System.Exception)
                    {
                    	//The designer really doesn't like loading the stuff from a file
                    }

					if (_allBuffs == null) //new machines have born their notion
					{
						//Default Buffs
						_allBuffs = new List<Buff>();
						_allBuffs.Add(new Buff() { Name = "Power Word: Fortitude", Category = BuffCategory.ClassBuffs,
							Stats = new Stats() { Stamina = 79 }});
						_allBuffs.Add(new Buff() { Name = "Improved Power Word: Fortitude", Category = BuffCategory.ClassBuffs,
							Stats = new Stats() { Stamina = (float)Math.Floor(79f * 0.3f) }, RequiredBuff = "Power Word: Fortitude"});
                        _allBuffs.Add(new Buff() { Name = "Divine Spirit", Category = BuffCategory.ClassBuffs,
                            Stats = new Stats() { Spirit = 50 }});
                        _allBuffs.Add(new Buff() { Name = "Improved Divine Spirit", Category = BuffCategory.ClassBuffs,
                            Stats = new Stats() { }, RequiredBuff = "Divine Spirit"});
						_allBuffs.Add(new Buff() { Name = "Mark of the Wild", Category = BuffCategory.ClassBuffs,
							Stats = new Stats() { Armor = 340, Strength = 14, Agility = 14, Stamina = 14, Intellect = 14, Spirit = 14, AllResist=25 }});
						_allBuffs.Add(new Buff() { Name = "Improved Mark of the Wild", Category = BuffCategory.ClassBuffs,
							Stats = new Stats() { Armor = (float)Math.Floor(340f * 0.35f), Strength = (float)Math.Floor(14f * 0.35f),
                                Agility = (float)Math.Floor(14f * 0.35f), Stamina = (float)Math.Floor(14f * 0.35f), Intellect = (float)Math.Floor(14f * 0.35f), Spirit = (float)Math.Floor(14f * 0.35f),
                                AllResist=(float)Math.Floor(25f * 0.35f)}, RequiredBuff = "Mark of the Wild"});
						_allBuffs.Add(new Buff() { Name = "Blood Pact", Category = BuffCategory.ClassBuffs,
							Stats = new Stats() { Stamina = 66 }});
						_allBuffs.Add(new Buff() { Name = "Improved Blood Pact", Category = BuffCategory.ClassBuffs,
							Stats = new Stats() { Stamina = (float)Math.Floor(66f * 0.3f) }, RequiredBuff = "Blood Pact"});
						_allBuffs.Add(new Buff() { Name = "Commanding Shout", Category = BuffCategory.ClassBuffs,
							Stats = new Stats() { Health = 1080f }});
						_allBuffs.Add(new Buff() { Name = "Improved Commanding Shout", Category = BuffCategory.ClassBuffs,
							Stats = new Stats() { Health = (float)Math.Floor(1080f * 0.25f) }, RequiredBuff = "Commanding Shout"});
						_allBuffs.Add(new Buff() { Name = "Devotion Aura", Category = BuffCategory.ClassBuffs,
							Stats = new Stats() { Armor = 861 }});
						_allBuffs.Add(new Buff() { Name = "Improved Devotion Aura", Category = BuffCategory.ClassBuffs,
							Stats = new Stats() { Armor = (float)Math.Floor(861f * 0.4f) }, RequiredBuff = "Devotion Aura"});
						_allBuffs.Add(new Buff() { Name = "Grace of Air Totem", Category = BuffCategory.ClassBuffs,
							Stats = new Stats() { Agility = 77 }});
						_allBuffs.Add(new Buff() { Name = "Improved Grace of Air Totem", Category = BuffCategory.ClassBuffs,
							Stats = new Stats() { Agility = (float)Math.Floor(77f * 0.15f) }, RequiredBuff = "Grace of Air Totem"});
						_allBuffs.Add(new Buff() { Name = "Strength of Earth Totem", Category = BuffCategory.ClassBuffs,
							Stats = new Stats() { Strength = 86 }});
						_allBuffs.Add(new Buff() { Name = "Improved Strength of Earth Totem", Category = BuffCategory.ClassBuffs,
							Stats = new Stats() { Strength = (float)Math.Floor(86f * 0.15f) }, RequiredBuff = "Strength of Earth Totem"});
						_allBuffs.Add(new Buff() { Name = "Battle Shout", Category = BuffCategory.ClassBuffs,
							Stats = new Stats() { AttackPower = 305 }});
						_allBuffs.Add(new Buff() { Name = "Improved Battle Shout", Category = BuffCategory.ClassBuffs,
							Stats = new Stats() { AttackPower = (float)Math.Floor(305f * 0.25f) }, RequiredBuff = "Battle Shout"});
						_allBuffs.Add(new Buff() { Name = "Blessing of Might", Category = BuffCategory.ClassBuffs, 
							Stats = new Stats() { AttackPower = 220 }});
						_allBuffs.Add(new Buff() { Name = "Improved Blessing of Might", Category = BuffCategory.ClassBuffs,
							Stats = new Stats() { AttackPower = (float)Math.Floor(220f * 0.2f) }, RequiredBuff = "Blessing of Might"});
						_allBuffs.Add(new Buff() { Name = "Blessing of Kings", Category = BuffCategory.ClassBuffs,
							Stats = new Stats() { BonusStrengthMultiplier = 0.1f, BonusAgilityMultiplier = 0.1f, BonusStaminaMultiplier = 0.1f,  BonusIntellectMultiplier = 0.1f, BonusSpiritMultiplier = 0.1f}});
						_allBuffs.Add(new Buff() { Name = "Unleashed Rage", Category = BuffCategory.ClassBuffs,
							Stats = new Stats() { BonusAttackPowerMultiplier = 0.1f }}); 
						_allBuffs.Add(new Buff() { Name = "Heroic Presence", Category = BuffCategory.ClassBuffs,
							Stats = new Stats() { HitRating = 15.769f, SpellHitRating = 12.62f }});
                        _allBuffs.Add(new Buff() { Name = "Arcane Intellect", Category = BuffCategory.ClassBuffs,
                            Stats = new Stats() { Intellect = 40 }});
                        _allBuffs.Add(new Buff() { Name = "Wrath of Air Totem", Category = BuffCategory.ClassBuffs, 
                            Stats = new Stats() {SpellDamageRating = 101}});
                        _allBuffs.Add(new Buff() { Name = "Totem of Wrath", Category = BuffCategory.ClassBuffs,
                            Stats = new Stats() {SpellCritRating = 22.08f * 3f, SpellHitRating = 12.62f * 3f}});
                        _allBuffs.Add(new Buff() { Name = "Moonkin Aura", Category = BuffCategory.ClassBuffs,
                            Stats = new Stats() { SpellCritRating = 22.08f * 5f }});
                        _allBuffs.Add(new Buff()
                        {
                            Name = "Blessing of Wisdom",
                            Category = BuffCategory.ClassBuffs,
                            Stats = new Stats() { Mp5 = 41 }
                        });
                        _allBuffs.Add(new Buff()
                        {
                            Name = "Improved Blessing of Wisdom",
                            Category = BuffCategory.ClassBuffs,
                            Stats = new Stats() { Mp5 = (float)Math.Floor(41f * 0.2f) },
                            RequiredBuff = "Blessing of Wisdom"
                        });


						//what can i say... you're crazy
						_allBuffs.Add(new Buff() { Name = "Elixir of Ironskin", Category = BuffCategory.ElixirsAndFlasks,
							Stats = new Stats() { Resilience = 30 },
							ConflictingBuffs = new string[] { "Elixir of Major Defense", "Elixir of Major Fortitude", "Flask of Fortification", "Flask of Chromatic Wonder", "Flask of Relentless Assault" }});
						_allBuffs.Add(new Buff() { Name = "Elixir of Major Defense", Category = BuffCategory.ElixirsAndFlasks,
							Stats = new Stats() { Armor = 550 },
							ConflictingBuffs = new string[] { "Elixir of Ironskin", "Elixir of Major Fortitude", "Flask of Fortification", "Flask of Chromatic Wonder", "Flask of Relentless Assault" }});
						_allBuffs.Add(new Buff() { Name = "Elixir of Major Fortitude", Category = BuffCategory.ElixirsAndFlasks,
							Stats = new Stats() { Health = 250 },
							ConflictingBuffs = new string[] { "Elixir of Major Defense", "Elixir of Ironskin", "Flask of Fortification", "Flask of Chromatic Wonder", "Flask of Relentless Assault" }});
						_allBuffs.Add(new Buff() { Name = "Elixir of Major Agility", Category = BuffCategory.ElixirsAndFlasks,
							Stats = new Stats() { Agility = 35, CritRating = 20 },
							ConflictingBuffs = new string[] { "Elixir of Mastery", "Flask of Fortification", "Flask of Chromatic Wonder", "Flask of Relentless Assault" }});
						_allBuffs.Add(new Buff() { Name = "Elixir of Mastery", Category = BuffCategory.ElixirsAndFlasks,
							Stats = new Stats() { Agility = 15, Stamina = 15, Strength = 15, Intellect = 15, Spirit = 15 },
							ConflictingBuffs = new string[] { "Elixir of Major Agility", "Flask of Fortification", "Flask of Chromatic Wonder", "Flask of Relentless Assault" }});
						_allBuffs.Add(new Buff() { Name = "Flask of Fortification", Category = BuffCategory.ElixirsAndFlasks,
							Stats = new Stats() { Health = 500, DefenseRating = 10 },
							ConflictingBuffs = new string[] { "Elixir of Ironskin", "Elixir of Major Defense", "Elixir of Major Fortitude", "Elixir of Major Agility", "Elixir of Mastery", "Flask of Chromatic Wonder", "Flask of Relentless Assault" }});
						_allBuffs.Add(new Buff() { Name = "Flask of Chromatic Wonder", Category = BuffCategory.ElixirsAndFlasks,
							Stats = new Stats() { Agility = 18, Strength = 18, Stamina = 18, Intellect = 18, Spirit = 18, AllResist=35},
							ConflictingBuffs = new string[] { "Elixir of Ironskin", "Elixir of Major Defense", "Elixir of Major Fortitude", "Elixir of Major Agility", "Elixir of Mastery", "Flask of Fortification", "Flask of Relentless Assault" }});
						_allBuffs.Add(new Buff() { Name = "Flask of Relentless Assault", Category = BuffCategory.ElixirsAndFlasks,
							Stats = new Stats() { AttackPower = 120 }, 
							ConflictingBuffs = new string[] { "Elixir of Ironskin", "Elixir of Major Defense", "Elixir of Major Fortitude", "Elixir of Major Agility", "Elixir of Mastery", "Flask of Chromatic Wonder", "Flask of Fortification" }
						});
                        _allBuffs.Add(new Buff()
                        {
                            Name = "Adept's Elixir",
                            Category = BuffCategory.ElixirsAndFlasks,
                            Stats = new Stats() { SpellDamageRating = 24, SpellCritRating = 24 },
                            ConflictingBuffs = new string[] { }
                        });
                        _allBuffs.Add(new Buff()
                        {
                            Name = "Elixir of Major Firepower",
                            Category = BuffCategory.ElixirsAndFlasks,
                            Stats = new Stats() { SpellFireDamageRating = 55 },
                            ConflictingBuffs = new string[] { }
                        });
                        _allBuffs.Add(new Buff()
                        {
                            Name = "Elixir of Major Frost Power",
                            Category = BuffCategory.ElixirsAndFlasks,
                            Stats = new Stats() { SpellFrostDamageRating = 55 },
                            ConflictingBuffs = new string[] { }
                        });
                        _allBuffs.Add(new Buff()
                        {
                            Name = "Elixir of Major Shadow Power",
                            Category = BuffCategory.ElixirsAndFlasks,
                            Stats = new Stats() { SpellShadowDamageRating = 55 },
                            ConflictingBuffs = new string[] { }
                        });
                        _allBuffs.Add(new Buff()
                        {
                            Name = "Elixir of Draenic Wisdom",
                            Category = BuffCategory.ElixirsAndFlasks,
                            Stats = new Stats() { Intellect = 30, Spirit = 30 },
                            ConflictingBuffs = new string[] { }
                        });
                        _allBuffs.Add(new Buff()
                        {
                            Name = "Elixir of Major Mageblood",
                            Category = BuffCategory.ElixirsAndFlasks,
                            Stats = new Stats() { Mp5 = 16 },
                            ConflictingBuffs = new string[] { }
                        });
                        _allBuffs.Add(new Buff()
                        {
                            Name = "Flask of Blinding Light",
                            Category = BuffCategory.ElixirsAndFlasks,
                            Stats = new Stats() { SpellArcaneDamageRating = 80 },
                            ConflictingBuffs = new string[] { }
                        });
                        _allBuffs.Add(new Buff()
                        {
                            Name = "Flask of Distilled Wisdom",
                            Category = BuffCategory.ElixirsAndFlasks,
                            Stats = new Stats() { Intellect = 65 },
                            ConflictingBuffs = new string[] { }
                        });
                        _allBuffs.Add(new Buff()
                        {
                            Name = "Flask of Mighty Restoration",
                            Category = BuffCategory.ElixirsAndFlasks,
                            Stats = new Stats() { Mp5 = 25 },
                            ConflictingBuffs = new string[] { }
                        });
                        _allBuffs.Add(new Buff()
                        {
                            Name = "Flask of Pure Death",
                            Category = BuffCategory.ElixirsAndFlasks,
                            Stats = new Stats() { SpellFireDamageRating = 80, SpellFrostDamageRating = 80, SpellShadowDamageRating = 80 },
                            ConflictingBuffs = new string[] { }
                        });
                        _allBuffs.Add(new Buff()
                        {
                            Name = "Flask of Supremen Power",
                            Category = BuffCategory.ElixirsAndFlasks,
                            Stats = new Stats() { SpellDamageRating = 70 },
                            ConflictingBuffs = new string[] { }
                        });
                        
                        
                        //all the constant
						_allBuffs.Add(new Buff() { Name = "30 Stamina Food", Category = BuffCategory.OtherConsumables,
							Stats = new Stats() { Stamina = 30 }, ConflictingBuffs = new string[] { "20 Agility Food", "20 Hit Rating Food", "20 Strength Food", "40 Attack Power Food"}});
						_allBuffs.Add(new Buff() { Name = "20 Agility Food", Category = BuffCategory.OtherConsumables,
							Stats = new Stats() { Agility = 20 }, ConflictingBuffs = new string[] { "30 Stamina Food", "20 Hit Rating Food", "20 Strength Food", "40 Attack Power Food" }});
						_allBuffs.Add(new Buff() { Name = "20 Strength Food", Category = BuffCategory.OtherConsumables,
							Stats = new Stats() { Strength = 20 }, ConflictingBuffs = new string[] { "30 Stamina Food", "20 Hit Rating Food", "20 Agility Food", "40 Attack Power Food" }});
						_allBuffs.Add(new Buff() { Name = "40 Attack Power Food", Category = BuffCategory.OtherConsumables,
							Stats = new Stats() { AttackPower = 40 }, ConflictingBuffs = new string[] { "30 Stamina Food", "20 Hit Rating Food", "20 Strength Food", "20 Agility Food" }});
						_allBuffs.Add(new Buff() { Name = "20 Hit Rating Food", Category = BuffCategory.OtherConsumables,
							Stats = new Stats() { HitRating = 20 }, ConflictingBuffs = new string[] { "20 Agility Food", "30 Stamina Food", "20 Strength Food", "40 Attack Power Food" }});
						_allBuffs.Add(new Buff() { Name = "Scroll of Protection", Category = BuffCategory.OtherConsumables,
							Stats = new Stats() { Armor = 300 }});
						_allBuffs.Add(new Buff() { Name = "Scroll of Agility", Category = BuffCategory.OtherConsumables,
							Stats = new Stats() { Agility = 20 }});
						_allBuffs.Add(new Buff() { Name = "Scroll of Strength", Category = BuffCategory.OtherConsumables,
							Stats = new Stats() { Strength = 20 }});
						_allBuffs.Add(new Buff() { Name = "Adamantite Weightstone", Category = BuffCategory.OtherConsumables,
							Stats = new Stats() { WeaponDamage = 12, CritRating = 14 }, ConflictingBuffs = new string[] { "Elemental Sharpening Stone" }});
						_allBuffs.Add(new Buff() { Name = "Elemental Sharpening Stone", Category = BuffCategory.OtherConsumables,
							Stats = new Stats() { CritRating = 28 }, ConflictingBuffs = new string[] { "Adamantite Weightstone" }});
                        _allBuffs.Add(new Buff()
                        {
                            Name = "Superior Wizard Oil",
                            Category = BuffCategory.OtherConsumables,
                            Stats = new Stats() { SpellDamageRating = 42 },
                            ConflictingBuffs = new string[] { "Brilliant Wizard Oil" }
                        });
                        _allBuffs.Add(new Buff()
                        {
                            Name = "Brilliant Wizard Oil",
                            Category = BuffCategory.OtherConsumables,
                            Stats = new Stats() { SpellDamageRating = 36, SpellCritRating = 14 },
                            ConflictingBuffs = new string[] { "Superior Wizard Oil" }
                        });
                        _allBuffs.Add(new Buff()
                        {
                            Name = "20 Spell Crit Food",
                            Category = BuffCategory.OtherConsumables,
                            Stats = new Stats() { SpellCritRating = 20, Spirit = 20 },
                            ConflictingBuffs = new string[] { "23 Spell Damage Food" }
                        });
                        _allBuffs.Add(new Buff()
                        {
                            Name = "23 Spell Damage Food",
                            Category = BuffCategory.OtherConsumables,
                            Stats = new Stats() { SpellDamageRating = 23, Spirit = 20 },
                            ConflictingBuffs = new string[] { "20 Spell Crit Food" }
                        });

						
						//super color motion
						_allBuffs.Add(new Buff() { Name = "Scorpid Sting", Category = BuffCategory.Debuffs,
							Stats = new Stats() { Miss = 5 }});
						_allBuffs.Add(new Buff() { Name = "Insect Swarm", Category = BuffCategory.Debuffs,
							Stats = new Stats() { Miss = 2 }});
						_allBuffs.Add(new Buff() { Name = "Dual Wielding Mob", Category = BuffCategory.Debuffs,
							Stats = new Stats() { Miss = 20 }, Type = BuffType.ShortDurationDW});
						_allBuffs.Add(new Buff() { Name = "Faerie Fire", Category = BuffCategory.Debuffs,
							Stats = new Stats() { ArmorPenetration = 610 }});
						_allBuffs.Add(new Buff() { Name = "Improved Faerie Fire", Category = BuffCategory.Debuffs,
							Stats = new Stats() { HitRating = 47.3077f }, RequiredBuff = "Faerie Fire"});
						_allBuffs.Add(new Buff() { Name = "Expose Armor (5cp)", Category = BuffCategory.Debuffs,
							Stats = new Stats() { ArmorPenetration = 2000 }, ConflictingBuffs = new string[] { "Sunder Armor (x5)" }});
						_allBuffs.Add(new Buff() { Name = "Improved Expose Armor (5cp)", Category = BuffCategory.Debuffs,
							Stats = new Stats() { ArmorPenetration = 1000 }, ConflictingBuffs = new string[] { "Sunder Armor (x5)" },
							RequiredBuff = "Expose Armor (5cp)"});
						_allBuffs.Add(new Buff() { Name = "Sunder Armor (x5)", Category = BuffCategory.Debuffs,
							Stats = new Stats() { ArmorPenetration = 2600 }, ConflictingBuffs = new string[] { "Expose Armor (5cp)" }});
						_allBuffs.Add(new Buff() { Name = "Curse of Recklessness", Category = BuffCategory.Debuffs,
							Stats = new Stats() { ArmorPenetration = 800 }});
						_allBuffs.Add(new Buff() { Name = "Improved Hunters Mark", Category = BuffCategory.Debuffs,
							Stats = new Stats() { AttackPower = 110 }});
						_allBuffs.Add(new Buff() { Name = "Expose Weakness", Category = BuffCategory.Debuffs,
							Stats = new Stats() { ExposeWeakness = 1 }});
						_allBuffs.Add(new Buff() { Name = "Improved Judgement of the Crusade", Category = BuffCategory.Debuffs,
							Stats = new Stats() { CritRating = 66.24f }});
                        _allBuffs.Add(new Buff()
                        {
                            Name = "Improved Scorch",
                            Category = BuffCategory.Debuffs,
                            Stats = new Stats() { BonusFireSpellPowerMultiplier = 0.15f }
                        });
                        _allBuffs.Add(new Buff()
                        {
                            Name = "Winter's Chill",
                            Category = BuffCategory.Debuffs,
                            Stats = new Stats() { SpellFrostCritRating = 22.08f * 10f }
                        });
                        _allBuffs.Add(new Buff()
                        {
                            Name = "Curse of Shadow",
                            Category = BuffCategory.Debuffs,
                            Stats = new Stats() { BonusShadowSpellPowerMultiplier = 0.1f, BonusArcaneSpellPowerMultiplier = 0.1f },
                            ConflictingBuffs = new string[] {"Curse of Shadow (Malediction)"}
                        });
                        _allBuffs.Add(new Buff()
                        {
                            Name = "Curse of the Elements",
                            Category = BuffCategory.Debuffs,
                            Stats = new Stats() { BonusFireSpellPowerMultiplier = 0.1f, BonusFrostSpellPowerMultiplier = 0.1f },
                            ConflictingBuffs = new string[] {"Curse of the Elements (Malediction)"}
                        });
                        _allBuffs.Add(new Buff()
                        {
                            Name = "Curse of Shadow (Malediction)",
                            Category = BuffCategory.Debuffs,
                            Stats = new Stats() { BonusShadowSpellPowerMultiplier = 0.13f, BonusArcaneSpellPowerMultiplier = 0.13f },
                            ConflictingBuffs = new string[] {"Curse of Shadow"}
                        });
                        _allBuffs.Add(new Buff()
                        {
                            Name = "Curse of the Elements (Malediction)",
                            Category = BuffCategory.Debuffs,
                            Stats = new Stats() { BonusFireSpellPowerMultiplier = 0.13f, BonusFrostSpellPowerMultiplier = 0.13f },
                            ConflictingBuffs = new string[] {"Curse of the Elements"}
                        });
                        _allBuffs.Add(new Buff()
                        {
                            Name = "Judgement of Wisdom",
                            Category = BuffCategory.Debuffs,
                            Stats = new Stats() { },
                        });

						//burning senses
						_allBuffs.Add(new Buff() { Name = "Malorne 2 Piece Bonus", Category = BuffCategory.SetBonuses,
							Stats = new Stats() { BloodlustProc = 0.8f }, SetName = "Malorne Harness", SetThreshold = 2});
						_allBuffs.Add(new Buff() { Name = "Malorne 4 Piece Bonus", Category = BuffCategory.SetBonuses,
							Stats = new Stats() { Armor = 1400, Strength = 30 }, SetName = "Malorne Harness", SetThreshold = 4});
						_allBuffs.Add(new Buff() { Name = "Nordrassil 4 Piece Bonus", Category = BuffCategory.SetBonuses,
							Stats = new Stats() { BonusShredDamage = 75 }, SetName = "Nordrassil Harness", SetThreshold = 4});
						_allBuffs.Add(new Buff() { Name = "Thunderheart 2 Piece Bonus", Category = BuffCategory.SetBonuses,
							Stats = new Stats() { MangleCostReduction = 5 }, SetName = "Thunderheart Harness", SetThreshold = 2});
						_allBuffs.Add(new Buff() { Name = "Thunderheart 4 Piece Bonus", Category = BuffCategory.SetBonuses,
							Stats = new Stats() { BonusRipDamageMultiplier = .15f }, SetName = "Thunderheart Harness", SetThreshold = 4});
						_allBuffs.Add(new Buff() { Name = "Gladiator 2 Piece Bonus", Category = BuffCategory.SetBonuses,
							Stats = new Stats() { Resilience = 35 }, SetName = "Gladiator's Sanctuary", SetThreshold = 2});
                        _allBuffs.Add(new Buff()
                        {
                            Name = "Tirisfal 2 Piece Bonus",
                            Category = BuffCategory.SetBonuses,
                            Stats = new Stats() { ArcaneBlastBonus = .2f },
                            SetName = "Tirisfal Regalia",
                            SetThreshold = 2
                        });
                        _allBuffs.Add(new Buff()
                        {
                            Name = "Tirisfal 4 Piece Bonus",
                            Category = BuffCategory.SetBonuses,
                            Stats = new Stats() { SpellDamageOnCritProc = 70f },
                            SetName = "Tirisfal Regalia",
                            SetThreshold = 4
                        });
                        _allBuffs.Add(new Buff()
                        {
                            Name = "Tempest 2 Piece Bonus",
                            Category = BuffCategory.SetBonuses,
                            Stats = new Stats() { EvocationExtension = 2f },
                            SetName = "Tempest Regalia",
                            SetThreshold = 2
                        });
                        _allBuffs.Add(new Buff()
                        {
                            Name = "Tempest 4 Piece Bonus",
                            Category = BuffCategory.SetBonuses,
                            Stats = new Stats() { BonusMageNukeMultiplier = 0.05f },
                            SetName = "Tempest Regalia",
                            SetThreshold = 4
                        });

						//i think you're slipping
						_allBuffs.Add(new Buff() { Name = "Bloodlust", Category = BuffCategory.TemporaryBuffs,
							Stats = new Stats() { Bloodlust = 0.3f }}); 
						_allBuffs.Add(new Buff() { Name = "Drums of Battle", Category = BuffCategory.TemporaryBuffs,
							Stats = new Stats() { DrumsOfBattle = 80 }}); 
						_allBuffs.Add(new Buff() { Name = "Drums of War", Category = BuffCategory.TemporaryBuffs,
							Stats = new Stats() { DrumsOfWar = 60 }}); 
						_allBuffs.Add(new Buff() { Name = "Badge of Tenacity", Category = BuffCategory.TemporaryBuffs,
							Stats = new Stats() { Agility = 150 }, Type = BuffType.ShortDurationDW});
						_allBuffs.Add(new Buff() { Name = "Commendation of Kael'thas", Category = BuffCategory.TemporaryBuffs,
							Stats = new Stats() { DodgeRating = 152 }, Type = BuffType.ShortDurationDW});
						_allBuffs.Add(new Buff() { Name = "Figurine - Empyrean Tortoise", Category = BuffCategory.TemporaryBuffs,
							Stats = new Stats() { DodgeRating = 165 }, Type = BuffType.ShortDurationDW});
						_allBuffs.Add(new Buff() { Name = "Moroes' Lucky Pocket Watch", Category = BuffCategory.TemporaryBuffs,
							Stats = new Stats() { DodgeRating = 300 }, Type = BuffType.ShortDurationDW});
						_allBuffs.Add(new Buff() { Name = "Idol of Terror", Category = BuffCategory.TemporaryBuffs,
							Stats = new Stats() { Agility = 65 }, Type = BuffType.ShortDurationDW});
						_allBuffs.Add(new Buff() { Name = "Ancestral Fortitude / Inspiration", Category = BuffCategory.TemporaryBuffs,
							Stats = new Stats() { BonusArmorMultiplier = 0.25f }, Type = BuffType.ShortDurationDW});
						_allBuffs.Add(new Buff() { Name = "Improved Lay On Hands", Category = BuffCategory.TemporaryBuffs,
							Stats = new Stats() { BonusArmorMultiplier = 0.3f }, Type = BuffType.ShortDurationDW});
						_allBuffs.Add(new Buff() { Name = "Heroic Potion", Category = BuffCategory.TemporaryBuffs,
							Stats = new Stats() { Health = 700 }, Type = BuffType.ShortDurationDW});
						_allBuffs.Add(new Buff() { Name = "Ironshield Potion", Category = BuffCategory.TemporaryBuffs,
							Stats = new Stats() { Armor = 2500 }, Type = BuffType.ShortDurationDW});
						_allBuffs.Add(new Buff() { Name = "Nightmare Seed", Category = BuffCategory.TemporaryBuffs,
							Stats = new Stats() { Health = 2000 }, Type = BuffType.ShortDurationDW});
						_allBuffs.Add(new Buff() { Name = "Heroic 1750 Health Trinket", Category = BuffCategory.TemporaryBuffs,
							Stats = new Stats() { Health = 1750 }, Type = BuffType.ShortDurationDW});
						_allBuffs.Add(new Buff() { Name = "Season 3 Resilience Relic", Category = BuffCategory.TemporaryBuffs,
							Stats = new Stats() { Resilience = 31 }, Type = BuffType.ShortDurationDW});
						_allBuffs.Add(new Buff() { Name = "Moonglade Rejuvination", Category = BuffCategory.TemporaryBuffs,
							Stats = new Stats() { DodgeRating = 35 }, Type = BuffType.ShortDurationDW});
						_allBuffs.Add(new Buff() { Name = "Living Root of the Wildheart", Category = BuffCategory.TemporaryBuffs,
							Stats = new Stats() { Armor = 4070 }, Type = BuffType.ShortDurationDW});
						_allBuffs.Add(new Buff() { Name = "Argussian Compass", Category = BuffCategory.TemporaryBuffs,
							Stats = new Stats() { Health = 1150 }, Type = BuffType.ShortDurationDW});
						_allBuffs.Add(new Buff() { Name = "Dawnstone Crab", Category = BuffCategory.TemporaryBuffs,
							Stats = new Stats() { DodgeRating = 125 }, Type = BuffType.ShortDurationDW});
						_allBuffs.Add(new Buff() { Name = "Adamantite Figurine", Category = BuffCategory.TemporaryBuffs,
							Stats = new Stats() { Armor = 1280 }, Type = BuffType.ShortDurationDW});
						_allBuffs.Add(new Buff() { Name = "Brooch of the Immortal King", Category = BuffCategory.TemporaryBuffs,
							Stats = new Stats() { Health = 1250 }, Type = BuffType.ShortDurationDW});

                        

						//american coca-cola
						System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(List<Buff>));
						StringBuilder sb = new StringBuilder();
						System.IO.StringWriter writer = new System.IO.StringWriter(sb);
						serializer.Serialize(writer, _allBuffs);
						writer.Close();
						System.IO.File.WriteAllText(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "BuffCache.xml"), sb.ToString());
					}
				}
				//sugar sweetness
				return _allBuffs;
			}
		}

		public static List<Buff> GetAllRelevantBuffs()
		{
			return AllBuffs.FindAll(new Predicate<Buff>(
				delegate(Buff buff)
				{
					return Calculations.HasRelevantStats(buff.Stats);
				}
			));
		}
	}
}
//1963...