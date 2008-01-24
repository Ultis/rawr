using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace Rawr
{
    public delegate bool StatFilter(float value);

    [AttributeUsage(AttributeTargets.Property)]
    public class DisplayNameAttribute:Attribute
    {
        public DisplayNameAttribute():this(null,null){}
        public DisplayNameAttribute(string longName, string shortName)
        {
            Short = shortName;
            Long = longName;
        }

        public string Short {get;set;}        
        public string Long {get;set;}
        public override string ToString()
        {
            return Long;
        }
    }

    public static class Extensions
    {
        // requires .net 3.5 public static string LongName(this PropertyInfo info)
        // allows it to be called like
        //   info.LongName()
        // instead of
        //   Extensions.LongName(info)
        public static string LongName(PropertyInfo info)
        {
            string prettyName = null;

            object[] attributes = info.GetCustomAttributes(typeof(DisplayNameAttribute), false);
            if (attributes.Length == 1 && attributes[0] is DisplayNameAttribute && (attributes[0] as DisplayNameAttribute).Long != null)
            {
                prettyName = (attributes[0] as DisplayNameAttribute).Long;
            }
            else
            {
                prettyName = System.Text.RegularExpressions.Regex.Replace(
                    info.Name,
                    "([A-Z])",
                    " $1",
                    System.Text.RegularExpressions.RegexOptions.Compiled).Trim();
            }
            return prettyName;
        }
        public static string ShortName(PropertyInfo info)
        {
            string prettyName = null;

            object[] attributes = info.GetCustomAttributes(typeof(DisplayNameAttribute), false);
            if (attributes.Length == 1 && attributes[0] is DisplayNameAttribute && (attributes[0] as DisplayNameAttribute).Short != null)
            {
                prettyName = (attributes[0] as DisplayNameAttribute).Short;
            }
            else
            {
                prettyName = System.Text.RegularExpressions.Regex.Replace(
                    info.Name,
                    "([a-z])",
                    "",
                    System.Text.RegularExpressions.RegexOptions.Compiled).Trim();
            }
            return prettyName;
        }
    }


    [Serializable]
    public class Stats
    {
        [DisplayName(Short = "AC")]
        public float Armor{get;set;}        
        //[DisplayName(Long = "", Short = "")]
        public float Health{get;set;}        
        [DisplayName(Short = "AGI")]
        public float Agility{get;set;}        
        [DisplayName(Short = "STA")]
        public float Stamina{get;set;}
        [DisplayName(Short = "DODGE")]
        public float DodgeRating{get;set;}
        [DisplayName(Short = "DEF")]
        public float DefenseRating{get;set;}
        [DisplayName(Short = "RES")]
        public float Resilience{get;set;}
		[DisplayName(Short = "MISS")]
        public float Miss{get;set;}        
        [DisplayName(Long="Agi scale", Short = "%AGI")]
        public float BonusAgilityMultiplier{get;set;}        
		[DisplayName(Long = "Str scale", Short = "%STR")]
        public float BonusStrengthMultiplier{get;set;}        
		[DisplayName(Long = "Sta scale", Short = "%STA")]
        public float BonusStaminaMultiplier{get;set;}        
		[DisplayName(Long = "AC scale", Short = "%AC")]
        public float BonusArmorMultiplier{get;set;}        
		[DisplayName(Long = "AP scale", Short = "%AP")]
        public float BonusAttackPowerMultiplier{get;set;}   
		[DisplayName(Long = "Crit scale", Short = "%CRIT")]
        public float BonusCritMultiplier{get;set;}     
		[DisplayName(Long = "AP")]
        public float AttackPower{get;set;}
		[DisplayName(Short = "STR")]
        public float Strength{get;set;}
		[DisplayName(Short = "CRIT")]
        public float CritRating{get;set;}
		[DisplayName(Short = "HIT")]
        public float HitRating{get;set;}
		[DisplayName(Short = "DMG")]
        public float WeaponDamage{get;set;}        
		[DisplayName(Short = "EXP")]
        public float ExpertiseRating{get;set;}
		[DisplayName(Long = "", Short = "HASTE")]
        public float HasteRating{get;set;}        
		[DisplayName(Long = "ArP", Short = "ArP")]
        public float ArmorPenetration{get;set;}
		[DisplayName(Long = "Shred Bonus", Short = "SHRED")]
        public float BonusShredDamage{get;set;}
		[DisplayName(Long = "Mangle Bonus", Short = "MNGL")]
        public float BonusMangleDamage{get;set;}
		[DisplayName(Long = "Rip scale", Short = "%RIP")]
        public float BonusRipDamageMultiplier{get;set;}
		[DisplayName(Long = "Mangle Cost",Short = "CMNGL")]
        public float MangleCostReduction{get;set;}
		[DisplayName(Short = "BLPRC")]
        public float BloodlustProc{get;set;}
		[DisplayName(Short = "TRPRC")]
        public float TerrorProc{get;set;}
		
		//with hands high into the sky so blue
        public Stats() { }
		

		public Stats Clone()
		{
            return (Stats)this.MemberwiseClone();
		}

		//as the ocean opens up to swallow you
		public static Stats operator +(Stats a, Stats b)
		{
			return new Stats() {
				Armor = a.Armor + b.Armor,
				Agility = a.Agility + b.Agility,
				ArmorPenetration = a.ArmorPenetration + b.ArmorPenetration,
				AttackPower = a.AttackPower + b.AttackPower,
				BloodlustProc = a.BloodlustProc + b.BloodlustProc,
				TerrorProc = a.TerrorProc + b.TerrorProc,
				BonusAgilityMultiplier = ((1 + a.BonusAgilityMultiplier) * (1 + b.BonusAgilityMultiplier)) - 1,
				BonusStrengthMultiplier = ((1 + a.BonusStrengthMultiplier) * (1 + b.BonusStrengthMultiplier)) - 1,
				BonusArmorMultiplier = ((1 + a.BonusArmorMultiplier) * (1 + b.BonusArmorMultiplier)) - 1,
				BonusAttackPowerMultiplier = ((1 + a.BonusAttackPowerMultiplier) * (1 + b.BonusAttackPowerMultiplier)) - 1,
				BonusCritMultiplier = ((1 + a.BonusCritMultiplier) * (1 + b.BonusCritMultiplier)) - 1,
				BonusRipDamageMultiplier = ((1 + a.BonusRipDamageMultiplier) * (1 + b.BonusRipDamageMultiplier)) - 1,
				BonusStaminaMultiplier = ((1 + a.BonusStaminaMultiplier) * (1 + b.BonusStaminaMultiplier)) - 1,
				BonusMangleDamage = a.BonusMangleDamage + b.BonusMangleDamage,
				BonusShredDamage = a.BonusShredDamage + b.BonusShredDamage,
				CritRating = a.CritRating + b.CritRating,
				DefenseRating = a.DefenseRating + b.DefenseRating,
				DodgeRating = a.DodgeRating + b.DodgeRating,
				ExpertiseRating = a.ExpertiseRating + b.ExpertiseRating,
				HasteRating = a.HasteRating + b.HasteRating,
				Health = a.Health + b.Health,
				HitRating = a.HitRating + b.HitRating,
				MangleCostReduction = a.MangleCostReduction + b.MangleCostReduction,
				Miss = a.Miss + b.Miss,
				Resilience = a.Resilience + b.Resilience,
				Stamina = a.Stamina + b.Stamina,
				Strength = a.Strength + b.Strength,
				WeaponDamage = a.WeaponDamage + b.WeaponDamage
			};
		}

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            var values = Values(x => x > 0);
            foreach ( PropertyInfo info in values.Keys)
            {
                sb.AppendFormat("{0} {1}, ", Extensions.ShortName(info), values[info]);
            }

            return sb.ToString().TrimEnd(' ', ',');
            /*

            string summary = string.Empty;
            if (Armor > 0) summary += Armor.ToString() + "AC, ";
            if (Health > 0) summary += Health.ToString() + "HP, ";
            if (Agility > 0) summary += Agility.ToString() + "Agi, ";
            if (Stamina > 0) summary += Stamina.ToString() + "Sta, ";
            if (DodgeRating > 0) summary += DodgeRating.ToString() + "Dodge, ";
            if (DefenseRating > 0) summary += DefenseRating.ToString() + "Def, ";
            if (Resilience > 0) summary += Resilience.ToString() + "Res, ";
			if (Miss > 0) summary += Miss.ToString() + "%Miss, ";
			if (BonusAgilityMultiplier > 0) summary += (100f * BonusAgilityMultiplier).ToString() + "%Agi, ";
			if (BonusStrengthMultiplier > 0) summary += (100f * BonusStrengthMultiplier).ToString() + "%Str, ";
			if (BonusStaminaMultiplier > 0) summary += (100f * BonusStaminaMultiplier).ToString() + "%Sta, ";
			if (BonusArmorMultiplier > 0) summary += (100f * BonusArmorMultiplier).ToString() + "%AC, ";
			if (BonusCritMultiplier > 0) summary += (100f * BonusCritMultiplier).ToString() + "%CritDmg, ";
			if (BonusRipDamageMultiplier > 0) summary += (100f * BonusRipDamageMultiplier).ToString() + "%RipDmg, ";
			if (AttackPower > 0) summary += AttackPower.ToString() + "AP, ";
			if (Strength > 0) summary += Strength.ToString() + "Str, ";
			if (CritRating > 0) summary += CritRating.ToString() + "Crit, ";
			if (HitRating > 0) summary += HitRating.ToString() + "Hit, ";
			if (WeaponDamage > 0) summary += WeaponDamage.ToString() + "Dmg, ";
			if (ExpertiseRating > 0) summary += ExpertiseRating.ToString() + "Exp, ";
			if (HasteRating > 0) summary += HasteRating.ToString() + "Haste, ";
			if (ArmorPenetration > 0) summary += ArmorPenetration.ToString() + "Pen, ";
			if (BonusShredDamage > 0) summary += BonusShredDamage.ToString() + "ShredDmg, ";
			if (BonusMangleDamage > 0) summary += BonusMangleDamage.ToString() + "MangleDmg, ";
			if (MangleCostReduction > 0) summary += MangleCostReduction.ToString() + "MangleCostReduction, ";
			if (BloodlustProc > 0) summary += "Bloodlust, ";
			if (TerrorProc > 0) summary += "Terror, ";
			summary = summary.TrimEnd(' ', ',', ':');
            return summary;
             */
        }

        private class PropertyComparer : IComparer<PropertyInfo>
        {

            public int Compare(PropertyInfo x, PropertyInfo y)
            {
                return x.Name.CompareTo(y.Name);
            }
        }

        public SortedList<PropertyInfo, float> Values(StatFilter filter)
        {
            SortedList<PropertyInfo, float> dict = new SortedList<PropertyInfo, float>(new PropertyComparer());
            foreach (PropertyInfo info in GetType().GetProperties())
            {
                float value = (float)info.GetValue(this, null);
                if (filter(value))
                {
                    dict[info] = value;
                }
            }
            return dict;
        }
    }
}
