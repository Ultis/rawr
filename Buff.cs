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
		public BuffType Type;
		public string RequiredBuff;
		public string[] ConflictingBuffs;

		//washing virgin halo
		public Buff() { }
		public Buff(string name, BuffCategory category, Stats stats) : this(name, category, stats, BuffType.LongDurationNoDW, string.Empty, new string[0]) { }
		public Buff(string name, BuffCategory category, Stats stats, BuffType type) : this(name, category, stats, type, string.Empty, new string[0]) { }
		public Buff(string name, BuffCategory category, Stats stats, BuffType type, string requiredBuff, string[] conflictingBuffs)
		{
			Name = name;
			Category = category;
			Stats = stats;
			Type = type;
			RequiredBuff = requiredBuff;
			ConflictingBuffs = conflictingBuffs;
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
			foreach (Buff buff in AllBuffs)
				if (buff.Name == name)
					return buff;
			return null;
		}

		//enter static
		public static List<Buff> GetBuffsByType(BuffType type)
		{
			List<Buff> buffs = new List<Buff>();
			foreach (Buff buff in AllBuffs)
				if (type == BuffType.All || buff.Type == type)
					buffs.Add(buff);
			return buffs;
		}

		//a grey mistake
		private static List<Buff> _allBuffs = null;
		public static List<Buff> AllBuffs
		{
			get
			{
				if (_allBuffs == null)
				{
					//so you both come crashing over ground
					if (File.Exists(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "BuffCache.xml")))
					{
						string xml = System.IO.File.ReadAllText(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "BuffCache.xml"));
						System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(List<Buff>));
						System.IO.StringReader reader = new System.IO.StringReader(xml);
						_allBuffs = (List<Buff>)serializer.Deserialize(reader);
						reader.Close();
					}
					else //new machines have born their notion
					{
						//Default Buffs
						_allBuffs = new List<Buff>();
						_allBuffs.Add(new Buff("Power Word: Fortitude", BuffCategory.ClassBuffs,
							new Stats(0, 0, 0, 79f, 0, 0, 0, 0, 0, 0, 0)));
						_allBuffs.Add(new Buff("Improved Power Word: Fortitude", BuffCategory.ClassBuffs,
							new Stats(0, 0, 0, (float)Math.Floor(79f * 0.3f), 0, 0, 0, 0, 0, 0, 0), BuffType.LongDurationNoDW, "Power Word: Fortitude", new string[0]));
						_allBuffs.Add(new Buff("Mark of the Wild", BuffCategory.ClassBuffs,
							new Stats(340f, 0, 14f, 14f, 0, 0, 0, 0, 0, 0, 0)));
						_allBuffs.Add(new Buff("Improved Mark of the Wild", BuffCategory.ClassBuffs,
							new Stats((float)Math.Floor(340f * 0.35f), 0, (float)Math.Floor(14f * 0.35f), (float)Math.Floor(14f * 0.35f), 0, 0, 0, 0, 0, 0, 0), BuffType.LongDurationNoDW, "Mark of the Wild", new string[0]));
						_allBuffs.Add(new Buff("Blood Pact", BuffCategory.ClassBuffs,
							new Stats(0, 0, 0, 66f, 0, 0, 0, 0, 0, 0, 0)));
						_allBuffs.Add(new Buff("Improved Blood Pact", BuffCategory.ClassBuffs,
							new Stats(0, 0, 0, (float)Math.Floor(66f * 0.3f), 0, 0, 0, 0, 0, 0, 0), BuffType.LongDurationNoDW, "Blood Pact", new string[0]));
						_allBuffs.Add(new Buff("Commanding Shout", BuffCategory.ClassBuffs,
							new Stats(0, 1080f, 0, 0, 0, 0, 0, 0, 0, 0, 0)));
						_allBuffs.Add(new Buff("Improved Commanding Shout", BuffCategory.ClassBuffs,
							new Stats(0, (float)Math.Floor(1080f * 0.25f), 0, 0, 0, 0, 0, 0, 0, 0, 0), BuffType.LongDurationNoDW, "Commanding Shout", new string[0]));
						_allBuffs.Add(new Buff("Devotion Aura", BuffCategory.ClassBuffs,
							new Stats(861f, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0)));
						_allBuffs.Add(new Buff("Improved Devotion Aura", BuffCategory.ClassBuffs,
							new Stats((float)Math.Floor(861f * 0.4f), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0), BuffType.LongDurationNoDW, "Devotion Aura", new string[0]));
						_allBuffs.Add(new Buff("Grace of Air Totem", BuffCategory.ClassBuffs,
							new Stats(0, 0, 77f, 0, 0, 0, 0, 0, 0, 0, 0)));
						_allBuffs.Add(new Buff("Improved Grace of Air Totem", BuffCategory.ClassBuffs,
							new Stats(0, 0, (float)Math.Floor(77f * 0.15f), 0, 0, 0, 0, 0, 0, 0, 0), BuffType.LongDurationNoDW, "Grace of Air Totem", new string[0]));
						_allBuffs.Add(new Buff("Blessing of Kings", BuffCategory.ClassBuffs,
							new Stats(0, 0, 0, 0, 0, 0, 0, 0, 0.1f, 0.1f, 0)));

						//what can i say... you're crazy
						_allBuffs.Add(new Buff("Elixir of Ironskin", BuffCategory.ElixirsAndFlasks,
							new Stats(0, 0, 0, 0, 0, 0, 30f, 0, 0, 0, 0), BuffType.LongDurationNoDW, string.Empty,
							new string[] { "Elixir of Major Defense", "Elixir of Major Fortitude", "Flask of Fortification", "Flask of Chromatic Wonder" }));
						_allBuffs.Add(new Buff("Elixir of Major Defense", BuffCategory.ElixirsAndFlasks,
							new Stats(550f, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0), BuffType.LongDurationNoDW, string.Empty,
							new string[] { "Elixir of Ironskin", "Elixir of Major Fortitude", "Flask of Fortification", "Flask of Chromatic Wonder" }));
						_allBuffs.Add(new Buff("Elixir of Major Fortitude", BuffCategory.ElixirsAndFlasks,
							new Stats(0, 250f, 0, 0, 0, 0, 0, 0, 0, 0, 0), BuffType.LongDurationNoDW, string.Empty,
							new string[] { "Elixir of Major Defense", "Elixir of Ironskin", "Flask of Fortification", "Flask of Chromatic Wonder" }));
						_allBuffs.Add(new Buff("Elixir of Major Agility", BuffCategory.ElixirsAndFlasks,
							new Stats(0, 0, 35f, 0, 0, 0, 0, 0, 0, 0, 0), BuffType.LongDurationNoDW, string.Empty,
							new string[] { "Elixir of Mastery", "Flask of Fortification", "Flask of Chromatic Wonder" }));
						_allBuffs.Add(new Buff("Elixir of Mastery", BuffCategory.ElixirsAndFlasks,
							new Stats(0, 0, 15f, 15f, 0, 0, 0, 0, 0, 0, 0), BuffType.LongDurationNoDW, string.Empty,
							new string[] { "Elixir of Major Agility", "Flask of Fortification", "Flask of Chromatic Wonder" }));
						_allBuffs.Add(new Buff("Flask of Fortification", BuffCategory.ElixirsAndFlasks,
							new Stats(0, 500f, 0, 0, 0, 10f, 0, 0, 0, 0, 0), BuffType.LongDurationNoDW, string.Empty,
							new string[] { "Elixir of Ironskin", "Elixir of Major Defense", "Elixir of Major Fortitude", "Elixir of Major Agility", "Elixir of Mastery", "Flask of Chromatic Wonder" }));
						_allBuffs.Add(new Buff("Flask of Chromatic Wonder", BuffCategory.ElixirsAndFlasks,
							new Stats(0, 0, 18f, 18f, 0, 0, 0, 0, 0, 0, 0), BuffType.LongDurationNoDW, string.Empty,
							new string[] { "Elixir of Ironskin", "Elixir of Major Defense", "Elixir of Major Fortitude", "Elixir of Major Agility", "Elixir of Mastery", "Flask of Fortification" }));

						//all the constant
						_allBuffs.Add(new Buff("30 Stamina Food", BuffCategory.OtherConsumables,
							new Stats(0, 0, 0, 30f, 0, 0, 0, 0, 0, 0, 0), BuffType.LongDurationNoDW, string.Empty, new string[] { "20 Agility Food" }));
						_allBuffs.Add(new Buff("20 Agility Food", BuffCategory.OtherConsumables,
							new Stats(0, 0, 20f, 0, 0, 0, 0, 0, 0, 0, 0), BuffType.LongDurationNoDW, string.Empty, new string[] { "30 Stamina Food" }));
						_allBuffs.Add(new Buff("Scroll of Protection", BuffCategory.OtherConsumables,
							new Stats(300f, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0)));
						_allBuffs.Add(new Buff("Scroll of Agility", BuffCategory.OtherConsumables,
							new Stats(0, 0, 20f, 0, 0, 0, 0, 0, 0, 0, 0)));

						//super color motion
						_allBuffs.Add(new Buff("Scorpid Sting", BuffCategory.Debuffs,
							new Stats(0, 0, 0, 0, 0, 0, 0, 5f, 0, 0, 0)));
						_allBuffs.Add(new Buff("Insect Swarm", BuffCategory.Debuffs,
							new Stats(0, 0, 0, 0, 0, 0, 0, 2f, 0, 0, 0)));
						//_allBuffs.Add(new Buff("Shadow Embrace", BuffCategory.Debuffs, 
						//	new Stats(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0)));
						_allBuffs.Add(new Buff("Dual Wielding Mob", BuffCategory.Debuffs,
							new Stats(0, 0, 0, 0, 0, 0, 0, 20f, 0, 0, 0), BuffType.ShortDurationDW));

						//burning senses
						_allBuffs.Add(new Buff("Malorne 4 Piece Bonus", BuffCategory.SetBonuses,
							new Stats(1400f, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0)));
						_allBuffs.Add(new Buff("Gladiator 2 Piece Bonus", BuffCategory.SetBonuses,
							new Stats(0, 0, 0, 0, 0, 0, 35f, 0, 0, 0, 0)));

						//i think you're slipping
						_allBuffs.Add(new Buff("Badge of Tenacity", BuffCategory.TemporaryBuffs,
							new Stats(0, 0, 150f, 0, 0, 0, 0, 0, 0, 0, 0), BuffType.ShortDurationDW));
						_allBuffs.Add(new Buff("Moroes' Lucky Pocket Watch", BuffCategory.TemporaryBuffs,
							new Stats(0, 0, 0, 0, 300f, 0, 0, 0, 0, 0, 0), BuffType.ShortDurationDW));
						_allBuffs.Add(new Buff("Idol of Terror", BuffCategory.TemporaryBuffs,
							new Stats(0, 0, 65f, 0, 0, 0, 0, 0, 0, 0, 0), BuffType.ShortDurationDW));
						_allBuffs.Add(new Buff("Ancestral Fortitude / Inspiration", BuffCategory.TemporaryBuffs,
							new Stats(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0.25f), BuffType.ShortDurationDW));
						_allBuffs.Add(new Buff("Improved Lay On Hands", BuffCategory.TemporaryBuffs,
							new Stats(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0.3f), BuffType.ShortDurationDW));
						_allBuffs.Add(new Buff("Heroic Potion", BuffCategory.TemporaryBuffs,
							new Stats(0, 700f, 0, 0, 0, 0, 0, 0, 0, 0, 0), BuffType.ShortDurationDW));
						_allBuffs.Add(new Buff("Ironshield Potion", BuffCategory.TemporaryBuffs,
							new Stats(2500f, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0), BuffType.ShortDurationDW));
						_allBuffs.Add(new Buff("Nightmare Seed", BuffCategory.TemporaryBuffs,
							new Stats(0, 2000f, 0, 0, 0, 0, 0, 0, 0, 0, 0), BuffType.ShortDurationDW));
						_allBuffs.Add(new Buff("Heroic 1750 Health Trinket", BuffCategory.TemporaryBuffs,
							new Stats(0, 1750f, 0, 0, 0, 0, 0, 0, 0, 0, 0), BuffType.ShortDurationDW));
						_allBuffs.Add(new Buff("Season 3 Resilience Relic", BuffCategory.TemporaryBuffs,
							new Stats(0, 0, 0, 0, 0, 0, 31f, 0, 0, 0, 0), BuffType.ShortDurationDW));
						_allBuffs.Add(new Buff("Moonglade Rejuvination", BuffCategory.TemporaryBuffs,
							new Stats(0, 0, 0, 0, 35f, 0, 0, 0, 0, 0, 0), BuffType.ShortDurationDW));
						_allBuffs.Add(new Buff("Living Root of the Wildheart", BuffCategory.TemporaryBuffs,
							new Stats(4070f, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0), BuffType.ShortDurationDW));
						_allBuffs.Add(new Buff("Argussian Compass", BuffCategory.TemporaryBuffs,
							new Stats(0, 1150f, 0, 0, 0, 0, 0, 0, 0, 0, 0), BuffType.ShortDurationDW));
						_allBuffs.Add(new Buff("Dawnstone Crab", BuffCategory.TemporaryBuffs,
							new Stats(0, 0, 0, 0, 125f, 0, 0, 0, 0, 0, 0), BuffType.ShortDurationDW));
						_allBuffs.Add(new Buff("Adamantite Figurine", BuffCategory.TemporaryBuffs,
							new Stats(1280f, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0), BuffType.ShortDurationDW));
						_allBuffs.Add(new Buff("Brooch of the Immortal King", BuffCategory.TemporaryBuffs,
							new Stats(0, 1250f, 0, 0, 0, 0, 0, 0, 0, 0, 0), BuffType.ShortDurationDW));

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
	}
}
//1963...