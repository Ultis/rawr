using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Rawr;

namespace Rawr
{
    class Damage
    {
        private static int CountDamageTypes = EnumHelper.GetCount(typeof(ItemDamageType)) + 1;
        private static int IDT_Special = EnumHelper.GetCount(typeof(ItemDamageType));
        public float Dam
        {
            get
            {
                return TotalDamage;
            }
            set
            {
                _fDamage[(int)_DamageTypePrimary] = value; 
            }
        }
        public float[] DamageArray
        {
            get
            {
                return _fDamage;
            }
            set
            {
                _fDamage = value;
            }
        }
        private float[] _fDamage = new float[CountDamageTypes];

        #region Dam By School
        public float PhyscialDamage
        {
            get { return _fDamage[(int)ItemDamageType.Physical] * (1 + _fDamageMultiplier[(int)ItemDamageType.Physical]); }
            set { _fDamage[(int)ItemDamageType.Physical] = value; }
        }
        public float ArcaneDamage
        {
            get { return _fDamage[(int)ItemDamageType.Arcane] * (1 + _fDamageMultiplier[(int)ItemDamageType.Arcane]); }
            set { _fDamage[(int)ItemDamageType.Arcane] = value; }
        }
        public float FireDamage
        {
            get { return _fDamage[(int)ItemDamageType.Fire] * (1 + _fDamageMultiplier[(int)ItemDamageType.Fire]); }
            set { _fDamage[(int)ItemDamageType.Fire] = value; }
        }
        public float FrostDamage
        {
            get { return _fDamage[(int)ItemDamageType.Frost] * (1 + _fDamageMultiplier[(int)ItemDamageType.Frost]); }
            set { _fDamage[(int)ItemDamageType.Frost] = value; }
        }
        public float HolyDamage
        {
            get { return _fDamage[(int)ItemDamageType.Holy] * (1 + _fDamageMultiplier[(int)ItemDamageType.Holy]); }
            set { _fDamage[(int)ItemDamageType.Holy] = value; }
        }
        public float NatureDamage
        {
            get { return _fDamage[(int)ItemDamageType.Nature] * (1 + _fDamageMultiplier[(int)ItemDamageType.Nature]); }
            set { _fDamage[(int)ItemDamageType.Nature] = value; }
        }
        public float ShadowDamage
        {
            get { return _fDamage[(int)ItemDamageType.Shadow] * (1 + _fDamageMultiplier[(int)ItemDamageType.Shadow]); }
            set { _fDamage[(int)ItemDamageType.Shadow] = value; }
        }
        public float MultiSchoolDamage
        {
            get { return _fDamage[IDT_Special] * (1 + _fDamageMultiplier[(int)ItemDamageType.Shadow]); }
            set { _fDamage[IDT_Special] = value; }
        } 
        #endregion

        /// <summary>
        /// Multiplier to be added for total damage.
        /// </summary>
        public float[] DamageMultiplier
        {
            get { return _fDamageMultiplier; }
            set { _fDamageMultiplier = value; }
        }
        private float[] _fDamageMultiplier = new float[EnumHelper.GetCount(typeof(ItemDamageType))];

        /// <summary>
        /// Total Dam done by all types.
        /// </summary>
        public float TotalDamage
        {
            get
            {
                float f = 0;
                foreach (float i in _fDamage)
                {
                    f += i;
                }
                return f;
            }
        }

        /// <summary>
        /// Default Dam type.  When setting damage, this is the one that's selected.
        /// TODO: Deal with FrostFire and those type abilities.
        /// </summary>
        [DefaultValue(ItemDamageType.Physical)]
        public ItemDamageType DamageTypePrimary 
        {
            get { return _DamageTypePrimary; }
            set { _DamageTypePrimary = value; }
        }
        private ItemDamageType _DamageTypePrimary;

        /// <summary>
        /// For Effects that have a Dual-/Multi-School component,
        /// set the value of MultiSchoolDamage to the base value
        /// The Multiplier will be the higher of which ever school 
        /// is represented in these flags.
        /// </summary>
        public ItemDamageFlags DamageTypeMulti
        {
            get { return _DamageTypeDual; }
            set { _DamageTypeDual = value; }
        }
        private ItemDamageFlags _DamageTypeDual;

        /// <summary>
        /// Flags representing all those schools that are doing damage.
        /// </summary>
        public ItemDamageFlags DamageTypesAll
        {
            get { return _DamageTypeAll; }
            set { _DamageTypeAll = value; }
        }
        private ItemDamageFlags _DamageTypeAll;

        #region Operators: + - * /
        public static Damage operator +(Damage a, Damage b)
        {
            Damage d = new Damage();
            float max = 0;
            foreach (ItemDamageType i in Enum.GetValues(typeof(ItemDamageType)))
            {
                d.DamageArray[(int)i] = a.DamageArray[(int)i] + b.DamageArray[(int)i];
//                d.DamageMultiplier[(int)i] = (1 + a.DamageMultiplier[(int)i]) * (1 + b.DamageMultiplier[(int)i]) - 1;
                if (d.DamageArray[(int)i] > max)
                {
                    // Set the Primary Dam type based on max damage.
                    max = d.DamageArray[(int)i];
                    d.DamageTypePrimary = i;
                }
            }
            return d;
        }

        public static Damage operator -(Damage a, Damage b)
        {
            Damage d = new Damage();
            float max = 0;
            foreach (ItemDamageType i in Enum.GetValues(typeof(ItemDamageType)))
            {
                d.DamageArray[(int)i] = a.DamageArray[(int)i] - b.DamageArray[(int)i];
//                d.DamageMultiplier[(int)i] = (1 + a.DamageMultiplier[(int)i]) / (1 + b.DamageMultiplier[(int)i]) - 1;
                if (d.DamageArray[(int)i] > max)
                {
                    // Set the Primary Dam type based on max damage.
                    max = d.DamageArray[(int)i];
                    d.DamageTypePrimary = i;
                }
            }
            return d;
        }

        /// <summary>
        /// Multiplies the values of damage by b.  
        /// However, does NOT change the DamageMultiplier values.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Damage operator *(Damage a, float b)
        {
            Damage d = new Damage();
            float max = 0;
            foreach (ItemDamageType i in Enum.GetValues(typeof(ItemDamageType)))
            {
                d.DamageArray[(int)i] = a.DamageArray[(int)i] * b;
                if (d.DamageArray[(int)i] > max)
                {
                    // Set the Primary Dam type based on max damage.
                    max = d.DamageArray[(int)i];
                    d.DamageTypePrimary = i;
                }
            }
            return d;
        }

        public static Damage operator /(Damage a, float b)
        {
            if (b == 0)
            {
#if DEBUG
                throw new DivideByZeroException();
#else
                return a;
#endif
            }
            Damage d = new Damage();
            float max = 0;
            foreach (ItemDamageType i in Enum.GetValues(typeof(ItemDamageType)))
            {
                d.DamageArray[(int)i] = a.DamageArray[(int)i] / b;
                if (d.DamageArray[(int)i] > max)
                {
                    // Set the Primary Dam type based on max damage.
                    max = d.DamageArray[(int)i];
                    d.DamageTypePrimary = i;
                }
            }
            return d;
        }

        public static bool operator ==(Damage a, Damage b)
        {
            return (a.Dam == b.Dam && a.DamageTypePrimary == b.DamageTypePrimary);
        }

        public static bool operator !=(Damage a, Damage b)
        {
            return (a.Dam != b.Dam || a.DamageTypePrimary != b.DamageTypePrimary);
        }

        public static bool operator >(Damage x, Damage y)
        {
            return x >= y && x != y;
        }
        public static bool operator >=(Damage x, Damage y)
        {
            return (x.Dam >= y.Dam);
        }
        public static bool operator <(Damage x, Damage y)
        {
            return x <= y && x != y;
        }
        public static bool operator <=(Damage x, Damage y)
        {
            return (x.Dam <= y.Dam);
        }
#if false
                public bool Equals(Damage other)
        {
            return this == other;
        }
  
#endif        
        #endregion

        public override string ToString()
        {
            string szArray = "";
            for (int i = 0; i < CountDamageTypes; i++ )
            {
                szArray += this.DamageArray[i].ToString("F1");
                szArray += ", ";
            }
            // szArray.TrimEnd(", ");
            string s = string.Format("{0:0.0}:({1})", TotalDamage, szArray);

            return base.ToString();
        }
    }
}
