using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.ComponentModel;
using System.Reflection.Emit;

namespace Rawr
{
    public delegate bool StatFilter(float value);


    [AttributeUsage(AttributeTargets.Property)]
    public class MultiplicativeAttribute:Attribute{}

    public static class Extensions
    {
        // requires .net 3.5 public static string LongName(this PropertyInfo info)
        // allows it to be called like
        //   info.LongName()
        // instead of
        //   Extensions.LongName(info)

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


    [Serializable]
    public class Stats
    {
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

        [Category("Equipment Procs")]
        public float BloodlustProc { get; set; }

        [Category("Equipment Procs")]
        public float TerrorProc { get; set; }

        public float Miss { get; set; }

        public float BonusShredDamage{get;set;}

        public float BonusMangleDamage{get;set;}

        public float MangleCostReduction{get;set;}

        [Multiplicative]
        public float BonusAgilityMultiplier { get; set; }

        [Multiplicative]
        public float BonusStrengthMultiplier { get; set; }

        [Multiplicative]
        public float BonusStaminaMultiplier { get; set; }

        [Multiplicative]
        public float BonusArmorMultiplier { get; set; }

        [Multiplicative]
        public float BonusAttackPowerMultiplier { get; set; }

        [Multiplicative]
        [DisplayName("%Crit")]
        public float BonusCritMultiplier { get; set; }

        [Multiplicative]
        public float BonusRipDamageMultiplier { get; set; }
		


		//with hands high into the sky so blue
        public Stats() { }
		

		public Stats Clone()
		{
            return (Stats)this.MemberwiseClone();
		}

        private static PropertyInfo[] _propertyInfoCache = null;
        private static SortedList<PropertyInfo, PropertyInfo> _multiplicativeProperties = new SortedList<PropertyInfo,PropertyInfo>();

        
        delegate Stats Adder(Stats a, Stats b);

        private static Adder _add = null;

        static Stats()
        {
            System.Collections.ArrayList items = new System.Collections.ArrayList();

            foreach (PropertyInfo info in typeof(Stats).GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.ExactBinding))
            {
                if (info.PropertyType.IsAssignableFrom(typeof(float)))
                {
                    items.Add(info);
                }
            }
            _propertyInfoCache = (PropertyInfo[]) items.ToArray(typeof(PropertyInfo));

            foreach (PropertyInfo info in _propertyInfoCache)
            {
                if (null == info.GetCustomAttributes(typeof(MultiplicativeAttribute), false))
                {
                    _multiplicativeProperties[info] = info; 
                }
            }


            // i'm a bad bad bobo
            DynamicMethod dm = new DynamicMethod(
                "Add",
                typeof(Stats),
                new Type[2] { typeof(Stats), typeof(Stats) },
                typeof(Stats),
                false);
            ILGenerator il = dm.GetILGenerator();
            MethodInfo mi;
            il.DeclareLocal(typeof(Stats));
            il.DeclareLocal(typeof(Stats));
            il.Emit(OpCodes.Newobj, typeof(Stats).GetConstructor(System.Type.EmptyTypes));
            il.Emit(OpCodes.Stloc_1);

            foreach (PropertyInfo pInfo in _propertyInfoCache)
            {
                if (!IsMultiplicative(pInfo))
                {
                    mi = pInfo.GetGetMethod();
                    il.Emit(OpCodes.Ldloc_1);
                    il.Emit(OpCodes.Ldarg_0);
                    il.EmitCall(OpCodes.Callvirt, mi, null);

                    il.Emit(OpCodes.Ldarg_1);
                    il.EmitCall(OpCodes.Callvirt, mi, null);

                    il.Emit(OpCodes.Add);

                    mi = pInfo.GetSetMethod();
                    il.EmitCall(OpCodes.Callvirt, mi, null);
                }
                else
                {
                    mi = pInfo.GetGetMethod();
                    il.Emit(OpCodes.Ldloc_1);
                    il.Emit(OpCodes.Ldc_R4, 1.0f);
                    il.Emit(OpCodes.Ldarg_0);
                    il.EmitCall(OpCodes.Callvirt, mi, null);
                    il.Emit(OpCodes.Add);

                    il.Emit(OpCodes.Ldc_R4, 1.0f);
                    il.Emit(OpCodes.Ldarg_1);
                    il.EmitCall(OpCodes.Callvirt, mi, null);

                    il.Emit(OpCodes.Add);
                    il.Emit(OpCodes.Mul);

                    il.Emit(OpCodes.Ldc_R4, 1.0f);
                    il.Emit(OpCodes.Sub);

                    mi = pInfo.GetSetMethod();
                    il.EmitCall(OpCodes.Callvirt, mi, null);
                }
            }

            il.Emit(OpCodes.Ldloc_1);
            il.Emit(OpCodes.Stloc_0);
            il.Emit(OpCodes.Ldloc_0);
            il.Emit(OpCodes.Ret);


            _add = (Adder) dm.CreateDelegate(typeof(Adder));

            //this should be a little faster while still maintaining the
            //intent of the pure reflective code i.e. adding new stats is
            //easy and not prone to errors
            //don't try this at home.

        }



        private static PropertyInfo[] PropertyInfoCache
        {
            get
            {
                return _propertyInfoCache;
            }
        }


        private static bool IsMultiplicative(PropertyInfo info)
        {
            return _multiplicativeProperties.ContainsKey(info);
        }

      
		//as the ocean opens up to swallow you
		public static Stats operator +(Stats a, Stats b)
        {
            return _add(a, b);
		}

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (PropertyInfo info in PropertyInfoCache)
            {
                if (info.PropertyType.IsAssignableFrom(typeof(float)))
                {
                    float value = (float)info.GetValue(this, null);
                    if(value != 0)
                    {
                        if (IsMultiplicative(info))
                        {
                            value *= 100;
                        }

                        sb.AppendFormat("{0} {1}, ", Extensions.DisplayName(info), value);
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
                String[] names = new string[PropertyInfoCache.Length];
                for (int i = 0; i < PropertyInfoCache.Length; i++)
                {

                    names[i] = Extensions.SpaceCamel(PropertyInfoCache[i].Name);
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
}
