using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.ComponentModel;
using System.Reflection.Emit;

namespace Rawr
{
	/// <summary>
	/// A Stats object represents a collection of stats on an object, such as an Item, Enchant, or Buff.
	/// </summary>
    [Serializable]
    public class Stats
	{
		/// <summary>
		/// The properties for each stat. In order to add additional stats for Rawr to track,
		/// first add properties here, for each stat. Apply a Category attribute to assign it to
		/// a category in the item editor. Optionally, apply a DisplayName attribute. If no
		/// DisplayName attribute is applied, the property name will be used, with spaces between
		/// each word, detected by capitalization (AttackPower becomes "Attack Power"). If the
		/// stat is a multiplier, add the Multiplicative attribute.
		/// </summary>
		#region Stat Properties

		[Category("Base Stats")]
        public float Armor{get;set;}

        [Category("Base Stats")]
        public float Health{get;set;}

        [Category("Base Stats")]
        public float Agility{get;set;}

        [Category("Base Stats")]
        public float Stamina { get; set; }

        [Category("Base Stats")]
        public float AttackPower { get; set; }

        [Category("Base Stats")]
        public float Strength { get; set; }

        [Category("Base Stats")]
        public float WeaponDamage { get; set; }

        [Category("Base Stats")]
        [DisplayName("Penetration")]
        public float ArmorPenetration { get; set; }

        [Category("Base Stats")]
        public float Intellect { get; set; }

        [Category("Base Stats")]
        public float Spirit { get; set; }

        [Category("Resistances")]
        [DisplayName("Frost Res")]
        public float FrostResistance { get; set; }

        [Category("Resistances")]
        [DisplayName("Nature Res")]
        public float NatureResistance { get; set; }

        [Category("Resistances")]
        [DisplayName("Fire Res")]
        public float FireResistance { get; set; }

        [Category("Resistances")]
        [DisplayName("Shadow Res")]
        public float ShadowResistance { get; set; }

        [Category("Resistances")]
        [DisplayName("Arcane Res")]
        public float ArcaneResistance { get; set; }

        [Category("Resistances")]
        [DisplayName("Resist")]
        public float AllResist { get; set; }


        [Category("Combat Ratings")]
        [DisplayName("Crit")]
        public float CritRating { get; set; }

        [Category("Combat Ratings")]
        [DisplayName("Hit")]
        public float HitRating { get; set; }

        [Category("Combat Ratings")]
        [DisplayName("Dodge")]
        public float DodgeRating { get; set; }

        [Category("Combat Ratings")]
        [DisplayName("Defense")]
        public float DefenseRating { get; set; }

        [Category("Combat Ratings")]
        public float Resilience{get;set;}

        [Category("Combat Ratings")]
        [DisplayName("Expertise")]
        public float ExpertiseRating { get; set; }

        [Category("Combat Ratings")]
        [DisplayName("Haste")]
        public float HasteRating { get; set; }

        [Category("Combat Ratings")]
        [DisplayName("Mana per 5 sec")]
        public float Mp5 { get; set; }

        [Category("Equipment Procs")]
        public float BloodlustProc { get; set; }

        [Category("Equipment Procs")]
        public float TerrorProc { get; set; }

		[DisplayName("% Miss")]
        public float Miss { get; set; }

        public float BonusShredDamage{get;set;}

        public float BonusMangleDamage{get;set;}

		public float MangleCostReduction { get; set; }

		public float ExposeWeakness { get; set; }

		public float Bloodlust { get; set; }

		public float DrumsOfWar { get; set; }

		public float DrumsOfBattle { get; set; }

		[Multiplicative]
		[DisplayName("% Agility")]
        public float BonusAgilityMultiplier { get; set; }

		[Multiplicative]
		[DisplayName("% Strength")]
        public float BonusStrengthMultiplier { get; set; }

		[Multiplicative]
		[DisplayName("% Stamina")]
        public float BonusStaminaMultiplier { get; set; }

		[Multiplicative]
		[DisplayName("% Armor")]
        public float BonusArmorMultiplier { get; set; }

		[Multiplicative]
		[DisplayName("% AP")]
        public float BonusAttackPowerMultiplier { get; set; }

        [Multiplicative]
        [DisplayName("% Crit Dmg")]
        public float BonusCritMultiplier { get; set; }

		[Multiplicative]
		[DisplayName("% Rip Dmg")]
        public float BonusRipDamageMultiplier { get; set; }

		#endregion

		/// <summary>
		/// Adds together two stats, when using a + operator. When adding additional stats for
		/// Rawr to track, after adding the stat property, also add a line for it to this method,
		/// to properly combine the stat, as appropriate.
		/// </summary>
		/// <param name="a">The first Stats object to combine.</param>
		/// <param name="b">The second Stats object to combine.</param>
		/// <returns>The combined Stats object.</returns>
		public static Stats operator +(Stats a, Stats b)
		{
			return new Stats()
			{
				Armor = a.Armor + b.Armor,
				Health = a.Health + b.Health,
				Agility = a.Agility + b.Agility,
				Stamina = a.Stamina + b.Stamina,
				AttackPower = a.AttackPower + b.AttackPower,
				Strength = a.Strength + b.Strength,
				Intellect = a.Intellect + b.Intellect,
				Spirit = a.Spirit + b.Spirit,
				WeaponDamage = a.WeaponDamage + b.WeaponDamage,
				ArmorPenetration = a.ArmorPenetration + b.ArmorPenetration,
				FrostResistance = a.FrostResistance + b.FrostResistance,
				NatureResistance = a.NatureResistance + b.NatureResistance,
				FireResistance = a.FireResistance + b.FireResistance,
				ShadowResistance = a.ShadowResistance + b.ShadowResistance,
				ArcaneResistance = a.ArcaneResistance + b.ArcaneResistance,
				AllResist = a.AllResist + b.AllResist,
				CritRating = a.CritRating + b.CritRating,
				HitRating = a.HitRating + b.HitRating,
				DodgeRating = a.DodgeRating + b.DodgeRating,
				DefenseRating = a.DefenseRating + b.DefenseRating,
				Resilience = a.Resilience + b.Resilience,
				ExpertiseRating = a.ExpertiseRating + b.ExpertiseRating,
				HasteRating = a.HasteRating + b.HasteRating,
				Mp5 = a.Mp5 + b.Mp5,
				BloodlustProc = a.BloodlustProc + b.BloodlustProc,
				TerrorProc = a.TerrorProc + b.TerrorProc,
				Miss = a.Miss + b.Miss,
				BonusShredDamage = a.BonusShredDamage + b.BonusShredDamage,
				BonusMangleDamage = a.BonusMangleDamage + b.BonusMangleDamage,
				MangleCostReduction = a.MangleCostReduction + b.MangleCostReduction,
				ExposeWeakness = a.ExposeWeakness + b.ExposeWeakness,
				Bloodlust = a.Bloodlust + b.Bloodlust,
				DrumsOfBattle = a.DrumsOfBattle + b.DrumsOfBattle,
				DrumsOfWar = a.DrumsOfWar + b.DrumsOfWar,
				BonusAgilityMultiplier = (1f + a.BonusAgilityMultiplier) * (1f + b.BonusAgilityMultiplier) - 1f,
				BonusStrengthMultiplier = (1f + a.BonusStrengthMultiplier) * (1f + b.BonusStrengthMultiplier) - 1f,
				BonusStaminaMultiplier = (1f + a.BonusStaminaMultiplier) * (1f + b.BonusStaminaMultiplier) - 1f,
				BonusArmorMultiplier = (1f + a.BonusArmorMultiplier) * (1f + b.BonusArmorMultiplier) - 1f,
				BonusAttackPowerMultiplier = (1f + a.BonusAttackPowerMultiplier) * (1f + b.BonusAttackPowerMultiplier) - 1f,
				BonusCritMultiplier = (1f + a.BonusCritMultiplier) * (1f + b.BonusCritMultiplier) - 1f,
				BonusRipDamageMultiplier = (1f + a.BonusRipDamageMultiplier) * (1f + b.BonusRipDamageMultiplier) - 1f
			};
		}

        
		
		//with hands held high into the sky so blue
        public Stats() { }

		//as the ocean opens up to swallow you
		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			foreach (PropertyInfo info in PropertyInfoCache)
			{
				float value = (float)info.GetValue(this, null);
				if (value != 0)
				{
					if (IsMultiplicative(info))
					{
						value *= 100;
					}

					value = (float)Math.Round(value * 100f) / 100f;

					sb.AppendFormat("{0}{1}, ", value, Extensions.DisplayName(info));
				}
			}

			return sb.ToString().TrimEnd(' ', ',');
		}

		public Stats Clone()
		{
            return (Stats)this.MemberwiseClone();
		}

		#region Multiplicative Handling
		private static PropertyInfo[] _propertyInfoCache = null;
		private static List<PropertyInfo> _multiplicativeProperties = new List<PropertyInfo>();

        static Stats()
        {
			List<PropertyInfo> items = new List<PropertyInfo>();

            foreach (PropertyInfo info in typeof(Stats).GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.ExactBinding))
            {
                if (info.PropertyType.IsAssignableFrom(typeof(float)))
                {
                    items.Add(info);
                }
            }
            _propertyInfoCache = items.ToArray();

            foreach (PropertyInfo info in _propertyInfoCache)
            {
                if (info.GetCustomAttributes(typeof(MultiplicativeAttribute), false).Length > 0)
                {
                    _multiplicativeProperties.Add(info); 
                }
            }
        }

		public static PropertyInfo[] PropertyInfoCache
		{
			get { return _propertyInfoCache; }
        }

        public static bool IsMultiplicative(PropertyInfo info)
        {
            return _multiplicativeProperties.Contains(info);
		}
		#endregion

        private class PropertyComparer : IComparer<PropertyInfo>
        {
            public int Compare(PropertyInfo x, PropertyInfo y)
            {
                return x.Name.CompareTo(y.Name);
            }
        }

        [System.Xml.Serialization.XmlIgnore]
        public static String[] StatNames
        {
            get{
                String[] names = new string[PropertyInfoCache.Length];
                for (int i = 0; i < PropertyInfoCache.Length; i++)
                {
                    names[i] = Extensions.DisplayName(PropertyInfoCache[i]);
                }
                Array.Sort(names);
                return names;
            }        
        }

        public IDictionary<PropertyInfo, float> Values(StatFilter filter)
        {
            SortedList<PropertyInfo, float> dict = new SortedList<PropertyInfo, float>(new PropertyComparer());
            foreach (PropertyInfo info in PropertyInfoCache)
            {
                if(info.PropertyType.IsAssignableFrom(typeof(float)))
                {
                    float value = (float)info.GetValue(this, null);
                    if (filter(value))
                    {
                        dict[info] = value;
                    }
                }
            }
            return dict;
        }
    }

	public delegate bool StatFilter(float value);

	[AttributeUsage(AttributeTargets.Property)]
	public class MultiplicativeAttribute : Attribute { }

	public static class Extensions
	{
		// requires .net 3.5 public static string LongName(this PropertyInfo info)
		// allows it to be called like
		//   info.LongName()
		// instead of
		//   Extensions.LongName(info)

		public static PropertyInfo UnDisplayName(string displayName)
		{
			foreach (PropertyInfo info in Stats.PropertyInfoCache)
			{
				if (DisplayName(info).Trim() == displayName.Trim())
					return info;
			}
			return null;
		}

		public static string DisplayName(PropertyInfo info)
		{
			string prettyName = null;

			object[] attributes = info.GetCustomAttributes(typeof(DisplayNameAttribute), false);
			if (attributes.Length == 1 && attributes[0] is DisplayNameAttribute && (attributes[0] as DisplayNameAttribute).DisplayName != null)
			{
				prettyName = (attributes[0] as DisplayNameAttribute).DisplayName;
			}
			else
			{
				prettyName = SpaceCamel(info.Name);
			}
			if (!prettyName.StartsWith("%"))
				prettyName = " " + prettyName;
			return prettyName;
		}
		public static string SpaceCamel(String name)
		{
			return System.Text.RegularExpressions.Regex.Replace(
					name,
					"([A-Z])",
					" $1",
					System.Text.RegularExpressions.RegexOptions.Compiled).Trim();
		}

		public static string UnSpaceCamel(String name)
		{
			return System.Text.RegularExpressions.Regex.Replace(
					name,
					"( )",
					"",
					System.Text.RegularExpressions.RegexOptions.Compiled).Trim();
		}
	}
}