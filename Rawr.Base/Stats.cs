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
        public float Mana { get; set; }

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
        public float ScopeDamage { get; set; }

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


        [Category("Spell Combat Ratings")]
        [DisplayName("Spell Crit")]
        public float SpellCritRating { get; set; }

        [Category("Spell Combat Ratings")]
        [DisplayName("Spell Frost Crit")]
        public float SpellFrostCritRating { get; set; }

        [Category("Spell Combat Ratings")]
        [DisplayName("Spell Damage")]
        public float SpellDamageRating { get; set; }

        [Category("Spell Combat Ratings")]
        [DisplayName("Spell Shadow Damage")]
        public float SpellShadowDamageRating { get; set; }

        [Category("Spell Combat Ratings")]
        [DisplayName("Spell Fire Damage")]
        public float SpellFireDamageRating { get; set; }

        [Category("Spell Combat Ratings")]
        [DisplayName("Spell Frost Damage")]
        public float SpellFrostDamageRating { get; set; }

        [Category("Spell Combat Ratings")]
        [DisplayName("Spell Arcane Damage")]
        public float SpellArcaneDamageRating { get; set; }
        [Category("Spell Combat Ratings")]
        [DisplayName("Spell Nature Damage")]
        public float SpellNatureDamageRating { get; set; }

        [Category("Spell Combat Ratings")]
        [DisplayName("Spell Penetration")]
        public float SpellPenetration { get; set; }

        [Category("Spell Combat Ratings")]
        [DisplayName("Spell Hit")]
        public float SpellHitRating { get; set; }

        [Category("Spell Combat Ratings")]
        public float Healing { get; set; }

        [Category("Spell Combat Ratings")]
        [DisplayName("Spell Haste")]
        public float SpellHasteRating { get; set; }

        // percentage mana generation while casting
        [Category("Spell Combat Ratings")]
        [DisplayName("Combat Mana Regeneration")]
        public float SpellCombatManaRegeneration { get; set; }


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
        [DisplayName("Parry")]
        public float ParryRating { get; set; }
        
        [Category("Combat Ratings")]
        [DisplayName("Block")]
        public float BlockRating { get; set; }
        
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

        [Category("Combat Ratings")]
        [DisplayName("Health per 5 sec")]
        public float Hp5 { get; set; }

        [Category("Equipment Procs")]
        public float BloodlustProc { get; set; }

        [Category("Equipment Procs")]
        public float TerrorProc { get; set; }

		[DisplayName("% Miss")]
        public float Miss { get; set; }

        public float BonusShredDamage{get;set;}

		public float BonusMangleDamage { get; set; }

		[DisplayName("Bonus Rip Damage Per Combo Point Per Tick")]
		public float BonusRipDamagePerCPPerTick { get; set; }

        public float ManaRestorePerHit { get; set; }

        public float ManaRestorePerCast { get; set; }

		public float MangleCostReduction { get; set; }

		public float ExposeWeakness { get; set; }

		public float Bloodlust { get; set; }

		public float DrumsOfWar { get; set; }

		public float DrumsOfBattle { get; set; }

        public float ArcaneBlastBonus { get; set; }

        public float SpellDamageFromIntellectPercentage { get; set; }

        public float SpellDamageFromSpiritPercentage { get; set; }

        [DisplayName("Spell Damage Increase for 6 sec on Crit")]
        public float SpellDamageFor6SecOnCrit { get; set; }

        [DisplayName("Spell Haste (50% 5 sec/Crit)")]
        [Category("Equipment Procs")]
        public float SpellHasteFor5SecOnCrit_50 { get; set; }

        // 15% change, 45 sec internal cooldown
        [DisplayName("Spell Haste (15% 6 sec/Cast)")]
        [Category("Equipment Procs")]
        public float SpellHasteFor6SecOnCast_15_45 { get; set; }

        // 10% change, 45 sec internal cooldown
        [DisplayName("Spell Haste (10% 6 sec/Hit)")]
        [Category("Equipment Procs")]
        public float SpellHasteFor6SecOnHit_10_45 { get; set; }

        [DisplayName("Spell Damage (10 sec/Resist)")]
        [Category("Equipment Procs")]
        public float SpellDamageFor10SecOnResist { get; set; }

        // trinket effect, does not sum up over gear, 2 trinkets with this effect is not equivalent to 1 trinket with double effect
        [DisplayName("Spell Damage (20 sec/2 min)")]
        [Category("Equipment Procs")]
        public float SpellDamageFor20SecOnUse2Min { get; set; }

        [DisplayName("Spell Damage (15 sec/1.5 min)")]
        [Category("Equipment Procs")]
        public float SpellDamageFor15SecOnUse90Sec { get; set; }

        [DisplayName("Spell Haste (20 sec/2 min)")]
        [Category("Equipment Procs")]
        public float SpellHasteFor20SecOnUse2Min { get; set; }

        [DisplayName("Mp5 on Cast (20 sec/2 min)")]
        [Category("Equipment Procs")]
        public float Mp5OnCastFor20SecOnUse2Min { get; set; }

        // 5% chance, no cooldown
        [DisplayName("Spell Damage (10 sec)")]
        [Category("Equipment Procs")]
        public float SpellDamageFor10SecOnHit_5 { get; set; }

        [Category("Equipment Procs")]
        public float BonusManaGem { get; set; }

        [Category("Equipment Procs")]
        public float BonusManaPotion { get; set; }

        [DisplayName("Spell Damage (15 sec/Gem)")]
        [Category("Equipment Procs")]
        public float SpellDamageFor15SecOnManaGem { get; set; }

        // 10% chance, 45 sec internal cooldown
        [DisplayName("Spell Damage (10 sec)")]
        [Category("Equipment Procs")]
        public float SpellDamageFor10SecOnHit_10_45 { get; set; }

        // 20% chance, 45 sec internal cooldown
        [DisplayName("Spell Damage (15 sec)")]
        [Category("Equipment Procs")]
        public float SpellDamageFor15SecOnCrit_20_45 { get; set; }

        // 20% chance, 45 sec internal cooldown
        [DisplayName("Spell Damage (10 sec)")]
        [Category("Equipment Procs")]
        public float SpellDamageFor10SecOnCrit_20_45 { get; set; }

        // Starfire idol
        [DisplayName("Starfire damage bonus")]
        [Category("Equipment Procs")]
        public float StarfireDmg { get; set; }
        // Moonfire idol
        [DisplayName("Moonfire damage bonus")]
        [Category("Equipment Procs")]
        public float MoonfireDmg { get; set; }
        // Wrath idol
        [DisplayName("Wrath damage bonus")]
        [Category("Equipment Procs")]
        public float WrathDmg { get; set; }
        // Moonkin Aura idol
        [DisplayName("Spell critical bonus")]
        [Category("Equipment Procs")]
        public float IdolCritRating { get; set; }

        public float EvocationExtension { get; set; }

        [Category("Equipment Procs")]
        public int LightningCapacitorProc { get; set; }

        [Multiplicative]
        public float BonusMageNukeMultiplier { get; set; }

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
        [DisplayName("% Int")]
        public float BonusIntellectMultiplier { get; set; }

		[Multiplicative]
		[DisplayName("% Armor")]
        public float BonusArmorMultiplier { get; set; }

		[Multiplicative]
		[DisplayName("% AP")]
        public float BonusAttackPowerMultiplier { get; set; }

        [Multiplicative]
        [DisplayName("% SP")]
        public float BonusSpellPowerMultiplier { get; set; }

        [Multiplicative]
        [DisplayName("% Fire Damage")]
        public float BonusFireSpellPowerMultiplier { get; set; }

        [Multiplicative]
        [DisplayName("% Shadow Damage")]
        public float BonusShadowSpellPowerMultiplier { get; set; }

        [Multiplicative]
        [DisplayName("% Arcane Damage")]
        public float BonusArcaneSpellPowerMultiplier { get; set; }

        [Multiplicative]
        [DisplayName("% Nature Damage")]
        public float BonusNatureSpellPowerMultiplier { get; set; }

        [Multiplicative]
        [DisplayName("% Frost Damage")]
        public float BonusFrostSpellPowerMultiplier { get; set; }

        [Multiplicative]
        [DisplayName("% Spirit")]
        public float BonusSpiritMultiplier { get; set; }

        [Multiplicative]
        [DisplayName("% Crit Dmg")]
        public float BonusCritMultiplier { get; set; }

        [Multiplicative]
        [DisplayName("% Spell Crit Dmg")]
        public float BonusSpellCritMultiplier { get; set; }

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
				//NOTE: This is hard-coded, not using reflection, due to speed and maintainability.
				//GetValue and SetValue via reflection are *extremely* slow, and cause noticable lag in the app.
				//We also tried at one point to dynamically generate this code at runtime, which worked, but was
				//very complex and not maintainable by anyone who didn't already know wtf it was doing. So,
				//we're back to just hard-coding it, which isn't that big of a deal.
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
				ParryRating = a.ParryRating + b.ParryRating,
				BlockRating = a.BlockRating + b.BlockRating,
				DefenseRating = a.DefenseRating + b.DefenseRating,
				Resilience = a.Resilience + b.Resilience,
				ExpertiseRating = a.ExpertiseRating + b.ExpertiseRating,
				HasteRating = a.HasteRating + b.HasteRating,
				Mp5 = a.Mp5 + b.Mp5,
                Hp5 = a.Hp5 + b.Hp5,
				BloodlustProc = a.BloodlustProc + b.BloodlustProc,
				TerrorProc = a.TerrorProc + b.TerrorProc,
				Miss = a.Miss + b.Miss,
				BonusShredDamage = a.BonusShredDamage + b.BonusShredDamage,
				BonusMangleDamage = a.BonusMangleDamage + b.BonusMangleDamage,
				BonusRipDamagePerCPPerTick = a.BonusRipDamagePerCPPerTick + b.BonusRipDamagePerCPPerTick,
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
				BonusRipDamageMultiplier = (1f + a.BonusRipDamageMultiplier) * (1f + b.BonusRipDamageMultiplier) - 1f,
				BonusIntellectMultiplier = (1f + a.BonusIntellectMultiplier) * (1f + b.BonusIntellectMultiplier) - 1f,
				BonusSpellCritMultiplier = (1f + a.BonusSpellCritMultiplier) * (1f + b.BonusSpellCritMultiplier) - 1f,
				BonusSpellPowerMultiplier = (1f + a.BonusSpellPowerMultiplier) * (1f + b.BonusSpellPowerMultiplier) - 1f,
                BonusFireSpellPowerMultiplier = (1f + a.BonusFireSpellPowerMultiplier) * (1f + b.BonusFireSpellPowerMultiplier) - 1f,
                BonusFrostSpellPowerMultiplier = (1f + a.BonusFrostSpellPowerMultiplier) * (1f + b.BonusFrostSpellPowerMultiplier) - 1f,
                BonusArcaneSpellPowerMultiplier = (1f + a.BonusArcaneSpellPowerMultiplier) * (1f + b.BonusArcaneSpellPowerMultiplier) - 1f,
                BonusShadowSpellPowerMultiplier = (1f + a.BonusShadowSpellPowerMultiplier) * (1f + b.BonusShadowSpellPowerMultiplier) - 1f,
                BonusNatureSpellPowerMultiplier = (1f + a.BonusNatureSpellPowerMultiplier) * (1f + b.BonusNatureSpellPowerMultiplier) - 1f,
                BonusSpiritMultiplier = (1f + a.BonusSpiritMultiplier) * (1f + b.BonusSpiritMultiplier) - 1f,
				SpellCritRating = a.SpellCritRating + b.SpellCritRating,
                SpellFrostCritRating = a.SpellFrostCritRating + b.SpellFrostCritRating,
                SpellDamageRating = a.SpellDamageRating + b.SpellDamageRating,
				SpellFireDamageRating = a.SpellFireDamageRating + b.SpellFireDamageRating,
				SpellHasteRating = a.SpellHasteRating + b.SpellHasteRating,
				SpellHitRating = a.SpellHitRating + b.SpellHitRating,
				SpellShadowDamageRating = a.SpellShadowDamageRating + b.SpellShadowDamageRating,
				SpellFrostDamageRating = a.SpellFrostDamageRating + b.SpellFrostDamageRating,
                SpellArcaneDamageRating = a.SpellArcaneDamageRating + b.SpellArcaneDamageRating,
                SpellNatureDamageRating = a.SpellNatureDamageRating + b.SpellNatureDamageRating,
                SpellPenetration = a.SpellPenetration + b.SpellPenetration,
                Mana = a.Mana + b.Mana,
                LightningCapacitorProc = a.LightningCapacitorProc + b.LightningCapacitorProc,
                ArcaneBlastBonus = a.ArcaneBlastBonus + b.ArcaneBlastBonus,
                SpellDamageFor6SecOnCrit = a.SpellDamageFor6SecOnCrit + b.SpellDamageFor6SecOnCrit,
                EvocationExtension = a.EvocationExtension + b.EvocationExtension,
                BonusMageNukeMultiplier = (1f + a.BonusMageNukeMultiplier) * (1f + b.BonusMageNukeMultiplier) - 1f,
                ManaRestorePerHit = a.ManaRestorePerHit + b.ManaRestorePerHit,
                ManaRestorePerCast = a.ManaRestorePerCast + b.ManaRestorePerCast,
                BonusManaGem = a.BonusManaGem + b.BonusManaGem,
                BonusManaPotion = Math.Max(a.BonusManaPotion, b.BonusManaPotion), // does not stack
                SpellDamageFor10SecOnHit_10_45 = a.SpellDamageFor10SecOnHit_10_45 + b.SpellDamageFor10SecOnHit_10_45,
                SpellDamageFromIntellectPercentage = a.SpellDamageFromIntellectPercentage + b.SpellDamageFromIntellectPercentage,
                SpellDamageFromSpiritPercentage = a.SpellDamageFromSpiritPercentage + b.SpellDamageFromSpiritPercentage,
                SpellDamageFor10SecOnResist = a.SpellDamageFor10SecOnResist + b.SpellDamageFor10SecOnResist,
                SpellDamageFor15SecOnCrit_20_45 = a.SpellDamageFor15SecOnCrit_20_45 + b.SpellDamageFor15SecOnCrit_20_45,
                SpellCombatManaRegeneration = a.SpellCombatManaRegeneration + b.SpellCombatManaRegeneration,
                SpellHasteFor5SecOnCrit_50 = a.SpellHasteFor5SecOnCrit_50 + b.SpellHasteFor5SecOnCrit_50,
                SpellHasteFor6SecOnCast_15_45 = a.SpellHasteFor6SecOnCast_15_45 + b.SpellHasteFor6SecOnCast_15_45,
                SpellDamageFor10SecOnHit_5 = a.SpellDamageFor10SecOnHit_5 + b.SpellDamageFor10SecOnHit_5,
                SpellHasteFor6SecOnHit_10_45 = a.SpellHasteFor6SecOnHit_10_45 + b.SpellHasteFor6SecOnHit_10_45,
                SpellDamageFor10SecOnCrit_20_45 = a.SpellDamageFor10SecOnCrit_20_45 + b.SpellDamageFor10SecOnCrit_20_45,
                Healing = a.Healing + b.Healing,
                StarfireDmg = a.StarfireDmg + b.StarfireDmg,
                WrathDmg = a.WrathDmg + b.WrathDmg,
                MoonfireDmg = a.MoonfireDmg + b.MoonfireDmg,
                IdolCritRating = a.IdolCritRating + b.IdolCritRating
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
