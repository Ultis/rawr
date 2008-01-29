using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.ComponentModel;

namespace Rawr
{
    public delegate bool StatFilter(float value);


    [AttributeUsage(AttributeTargets.Property)]
    public class DisplayNameAttribute:System.ComponentModel.DisplayNameAttribute
    {
        public DisplayNameAttribute(){}
        public DisplayNameAttribute(string longName, string shortName, string compact)
        {
            Abbreviation = shortName;
            Long = longName;
            Short = compact;
        }

        public string Abbreviation { get; set; }
        public string Long
        {
            get
            {
                return base.DisplayNameValue;
            }
            set
            {
                base.DisplayNameValue = value;
            }
        }
        public string Short { get; set; }
        public override string ToString()
        {
            return Long;
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class MultiplicativeAttribute:Attribute{}

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
                prettyName = SpaceCamel(info.Name);
            }
            return prettyName;
        }
        public static string AbbreviatedName(PropertyInfo info)
        {
            string prettyName = null;

            object[] attributes = info.GetCustomAttributes(typeof(DisplayNameAttribute), false);
            if (attributes.Length == 1 && attributes[0] is DisplayNameAttribute && (attributes[0] as DisplayNameAttribute).Abbreviation != null)
            {
                prettyName = (attributes[0] as DisplayNameAttribute).Abbreviation;
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
        [Category("Base Stats")]
        [DisplayName(Abbreviation = "AC")]
        public float Armor{get;set;}

        [Category("Base Stats")]
        public float Health{get;set;}

        [Category("Base Stats")]
        [DisplayName(Abbreviation = "AGI")]
        public float Agility{get;set;}

        [Category("Base Stats")]
        [DisplayName(Abbreviation = "STA")]
        public float Stamina { get; set; }

        [Category("Base Stats")]
        [DisplayName(Long = "Attack Power", Short = "AP")]
        public float AttackPower { get; set; }

        [Category("Base Stats")]
        [DisplayName(Abbreviation = "STR")]
        public float Strength { get; set; }

        [Category("Base Stats")]
        [DisplayName(Long="Weapon Damage", Abbreviation = "DMG")]
        public float WeaponDamage { get; set; }

        [Category("Base Stats")]
        [DisplayName(Long = "Armor Penetration", Short = "ArP", Abbreviation = "ArP")]
        public float ArmorPenetration { get; set; }


        [Category("Combat Ratings")]
        [DisplayName(Long = "Critical strike",Abbreviation = "CRIT")]
        public float CritRating { get; set; }

        [Category("Combat Ratings")]
        [DisplayName(Long = "Hit", Abbreviation = "HIT")]
        public float HitRating { get; set; }

        [Category("Combat Ratings")]
        [DisplayName(Long = "Dodge", Abbreviation = "DODGE")]
        public float DodgeRating { get; set; }

        [Category("Combat Ratings")]
        [DisplayName(Long = "Defense", Abbreviation = "DEF")]
        public float DefenseRating { get; set; }

        [Category("Combat Ratings")]
        [DisplayName(Long = "Resilience", Abbreviation = "RES")]
        public float Resilience{get;set;}

        [Category("Combat Ratings")]
        [DisplayName(Long = "Expertise", Abbreviation = "EXP")]
        public float ExpertiseRating { get; set; }

        [Category("Combat Ratings")]
        [DisplayName(Long = "Haste", Abbreviation = "HASTE")]
        public float HasteRating { get; set; }

        [Category("Equipment Procs")]
        [DisplayName(Long = "Bloodlust", Abbreviation = "BLPRC")]
        public float BloodlustProc { get; set; }

        [Category("Equipment Procs")]
        [DisplayName(Long = "Idol of Terror", Abbreviation = "TRPRC")]
        public float TerrorProc { get; set; }

		[DisplayName(Abbreviation = "MISS")]
        public float Miss { get; set; }

        [DisplayName(Long = "Shred Damage", Short = "Shred Bonus", Abbreviation = "+SHRD")]
        public float BonusShredDamage{get;set;}

        [DisplayName(Long= "Mangle Damage", Short = "Mangle Bonus", Abbreviation = "+MNGL")]
        public float BonusMangleDamage{get;set;}

		[DisplayName(Long= "Mangle Cost Reduction", Short = "Mangle Cost",Abbreviation = "$MNGL")]
        public float MangleCostReduction{get;set;}

        [Multiplicative]
        [DisplayName(Long="Agility Multiplier", Short= "Agi scale", Abbreviation = "*AGI")]
        public float BonusAgilityMultiplier { get; set; }

        [Multiplicative]
        [DisplayName(Long = "Strength Multiplier", Short = "Str scale", Abbreviation = "*STR")]
        public float BonusStrengthMultiplier { get; set; }

        [Multiplicative]
        [DisplayName(Long = "Stamina Multiplier", Short = "Sta scale", Abbreviation = "*STA")]
        public float BonusStaminaMultiplier { get; set; }

        [Multiplicative]
        [DisplayName(Long = "Armor Multiplier", Short = "AC scale", Abbreviation = "*AC")]
        public float BonusArmorMultiplier { get; set; }

        [Multiplicative]
        [DisplayName(Long = "Attack Power Multiplier", Short = "AP scale", Abbreviation = "*AP")]
        public float BonusAttackPowerMultiplier { get; set; }

        [Multiplicative]
        [DisplayName(Long = "Crit Multiplier", Short = "Crit scale", Abbreviation = "*CRIT")]
        public float BonusCritMultiplier { get; set; }

        [Multiplicative]
        [DisplayName(Long = "Rip Damage Multiplier", Short = "Rip scale", Abbreviation = "*RIP")]
        public float BonusRipDamageMultiplier { get; set; }
		
		//with hands high into the sky so blue
        public Stats() { }
		

		public Stats Clone()
		{
            return (Stats)this.MemberwiseClone();
		}

		//as the ocean opens up to swallow you
		public static Stats operator +(Stats a, Stats b)
		{
            Stats s = new Stats();
            foreach (PropertyInfo info in a.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                if(info.PropertyType.IsAssignableFrom(typeof(float)))
                {
                    float aVal = (float)info.GetValue(a, null);
                    float bVal = (float)info.GetValue(b, null);
                    if(null == info.GetCustomAttributes(typeof(MultiplicativeAttribute), true))
                    {
                        info.SetValue(s, (1 + aVal) * (1 + bVal) - 1, null);
                    }
                    else
                    {
                        info.SetValue(s, aVal + bVal, null);
                    }
                }                
            }
            return s;
		}

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (PropertyInfo info in GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                if (info.PropertyType.IsAssignableFrom(typeof(float)))
                {
                    float value = (float)info.GetValue(this, null);
                    if(value != 0)
                    {
                        if (info.GetCustomAttributes(typeof(MultiplicativeAttribute), true).Length == 1)
                        {
                            value *= 100;
                        }

                        sb.AppendFormat("{0} {1}, ", Extensions.AbbreviatedName(info), value);
                    }
                }
            }

            return sb.ToString().TrimEnd(' ', ',');
        }

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
                PropertyInfo[] infoList = typeof(Stats).GetProperties(BindingFlags.Instance | BindingFlags.Public);
                String[] names = new string[infoList.Length];
                for (int i = 0; i < infoList.Length;i++ )
                {

                    names[i] = Extensions.SpaceCamel(infoList[i].Name);
                }
                Array.Sort(names);
                return names;
            }        
        }

        public IDictionary<PropertyInfo, float> Values(StatFilter filter)
        {
            SortedList<PropertyInfo, float> dict = new SortedList<PropertyInfo, float>(new PropertyComparer());
            foreach (PropertyInfo info in GetType().GetProperties(BindingFlags.Instance|BindingFlags.Public))
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
}
