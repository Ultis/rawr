using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;

namespace Rawr.Hunter
{
    /// <summary>List of Hunter stats that are Additive</summary>
    public enum AdditiveStatHunter : int
    {
        /// <summary>% Extra Pet Crit Chance</summary>
        BonusPetCritChance,
        /// <summary>Pet Stamina</summary>
        PetStamina,
        /// <summary>Pet Strength</summary>
        PetStrength,
        /// <summary>Pet Spirit</summary>
        PetSpirit,
        /// <summary>Pet Attack Power</summary>
        PetAttackPower,
     }
    /// <summary>List of Hunter stats that are Multiplicative</summary>
    public enum MultiplicativeStatHunter : int
    {
        /// <summary>Bonus Ranged Attack Power Multiplier</summary>
        BonusRangedAttackPowerMultiplier,
        /// <summary>Bonus Pet Attack Power Multiplier</summary>
        BonusPetAttackPowerMultiplier,
   }
    /// <summary>List of Hunter stats that are Inverse Multiplicative</summary>
    public enum InverseMultiplicativeStatHunter : int { }
    /// <summary>List of Hunter stats that do not Stack</summary>
    public enum NonStackingStatHunter : int { }

    /// <summary>
    /// Hunter custom implementation of the Stats object to expand it with Hunter Specific variables
    /// </summary>
#if SILVERLIGHT
    public class StatsHunter : Stats
#else
    public unsafe class StatsHunter : Stats
#endif
    {
        #region Arrays, duplicated from Base to be populated by Hunter Stats
        internal float[] _rawAdditiveHunterData = new float[AdditiveStatHunterCount];
        internal float[] _rawMultiplicativeHunterData = new float[MultiplicativeStatHunterCount];
        internal float[] _rawInverseMultiplicativeHunterData = new float[InverseMultiplicativeStatHunterCount];
        internal float[] _rawNoStackHunterData = new float[NonStackingStatHunterCount];

        internal int[] _sparseIndicesHunter;
        internal int _sparseAdditiveHunterCount;
        internal int _sparseMultiplicativeHunterCount;
        internal int _sparseInverseMultiplicativeHunterCount;
        internal int _sparseNoStackHunterCount;

        /// <summary></summary>
        protected static PropertyInfo[] _propertyInfoCacheHunter = null;
        /// <summary></summary>
        protected static List<PropertyInfo> _percentagePropertiesHunter = new List<PropertyInfo>();
        /// <summary></summary>
        protected static int AdditiveStatHunterCount = 0;
        /// <summary></summary>
        protected static int MultiplicativeStatHunterCount = 0;
        /// <summary></summary>
        protected static int InverseMultiplicativeStatHunterCount = 0;
        /// <summary></summary>
        protected static int NonStackingStatHunterCount = 0;
        #endregion

        #region Sort-Of Overrides, they call the base function and then do the same thing with Hunter Stats
        /// <summary>Dumps the cached values</summary>
        public new void Clear()
        {
            base.Clear();
            Array.Clear(_rawAdditiveHunterData, 0, _rawAdditiveHunterData.Length);
            Array.Clear(_rawMultiplicativeHunterData, 0, _rawMultiplicativeHunterData.Length);
            Array.Clear(_rawInverseMultiplicativeHunterData, 0, _rawInverseMultiplicativeHunterData.Length);
            Array.Clear(_rawNoStackHunterData, 0, _rawNoStackHunterData.Length);
            _rawSpecialEffectDataSize = 0;
        }

        /// <summary>Dumps the sparse data</summary>
        public new void InvalidateSparseData()
        {
            base.InvalidateSparseData();
            _sparseIndicesHunter = null;
        }

        /// <summary>Generates the sparse data in base Stats class and in Hunter Specific</summary>
        public new void GenerateSparseData()
        {
            base.GenerateSparseData();
            if (_sparseIndicesHunter != null) return;
            lock (_rawAdditiveHunterData)
            {
                if (_sparseIndicesHunter == null)
                {
                    List<int> indices = new List<int>();
                    _sparseAdditiveHunterCount = 0;
                    for (int i = 0; i < _rawAdditiveHunterData.Length; i++)
                    {
                        if (_rawAdditiveHunterData[i] != 0.0f)
                        {
                            _sparseAdditiveHunterCount++;
                            indices.Add(i);
                        }
                    }
                    _sparseMultiplicativeHunterCount = 0;
                    for (int i = 0; i < _rawMultiplicativeHunterData.Length; i++)
                    {
                        if (_rawMultiplicativeHunterData[i] != 0.0f)
                        {
                            _sparseMultiplicativeHunterCount++;
                            indices.Add(i);
                        }
                    }
                    _sparseInverseMultiplicativeHunterCount = 0;
                    for (int i = 0; i < _rawInverseMultiplicativeHunterData.Length; i++)
                    {
                        if (_rawInverseMultiplicativeHunterData[i] != 0.0f)
                        {
                            _sparseInverseMultiplicativeHunterCount++;
                            indices.Add(i);
                        }
                    }
                    _sparseNoStackHunterCount = 0;
                    for (int i = 0; i < _rawNoStackHunterData.Length; i++)
                    {
                        if (_rawNoStackHunterData[i] != 0.0f)
                        {
                            _sparseNoStackHunterCount++;
                            indices.Add(i);
                        }
                    }
                    _sparseIndicesHunter = indices.ToArray();
                }
            }
        }

        /// <summary>Clones the stats object for duplication with separation</summary>
        public new StatsHunter Clone()
        {
            StatsHunter clone = (StatsHunter)this.MemberwiseClone();
            StatsHunter retVal = new StatsHunter();
            retVal.Accumulate(base.Clone());
            retVal._rawAdditiveHunterData = (float[])clone._rawAdditiveHunterData.Clone();
            retVal._rawMultiplicativeHunterData = (float[])clone._rawMultiplicativeHunterData.Clone();
            retVal._rawInverseMultiplicativeHunterData = (float[])clone._rawInverseMultiplicativeHunterData.Clone();
            retVal._rawNoStackHunterData = (float[])clone._rawNoStackHunterData.Clone();
            return retVal;
        }

        /// <summary></summary>
        public static PropertyInfo[] PropertyInfoCacheHunter { get { return _propertyInfoCacheHunter; } }

        [XmlIgnore]
        public static String[] StatNamesHunter
        {
            get
            {
                String[] names = new string[PropertyInfoCacheHunter.Length];
                for (int i = 0; i < PropertyInfoCacheHunter.Length; i++)
                {
                    names[i] = Extensions.DisplayName(PropertyInfoCacheHunter[i]);
                }
                Array.Sort(names);
                return names;
            }
        }
        public IDictionary<PropertyInfo, float> ValuesHunter(StatFilter filter)
        {
#if SILVERLIGHT
            Dictionary<PropertyInfo, float> dict = new Dictionary<PropertyInfo, float>();
#else
            SortedList<PropertyInfo, float> dict = new SortedList<PropertyInfo, float>(new PropertyComparer());
#endif
            foreach (PropertyInfo info in PropertyInfoCacheHunter)
            {
                if (info.PropertyType.IsAssignableFrom(typeof(float)))
                {
                    float value = (float)info.GetValue(this, null);
                    if (filter(value))
                    {
                        dict[info] = value;
                    }
                }
            }
#if SILVERLIGHT
            //dict.OrderBy(kvp => kvp.Key, new PropertyComparer());
#endif
            return dict;
        }

        public override int GetHashCode()
        {
            return _rawAdditiveHunterData.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return this == (obj as StatsHunter);
        }
        public static bool operator ==(StatsHunter x, StatsHunter y)
        {
            if (ReferenceEquals(x, y) || (ReferenceEquals(x, null) && ReferenceEquals(y, null)))
                return true;
            if (ReferenceEquals(x, null) || ReferenceEquals(y, null))
                return false;
            if (x._rawSpecialEffectDataSize > 0 || y._rawSpecialEffectDataSize > 0) return false;
            return ArrayUtils.AllEqual(x.rawAdditiveData, y.rawAdditiveData) &&
                ArrayUtils.AllEqual(x.rawMultiplicativeData, y.rawMultiplicativeData) &&
                ArrayUtils.AllEqual(x.rawNoStackData, y.rawNoStackData) &&
                ArrayUtils.AllEqual(x._rawAdditiveHunterData, y._rawAdditiveHunterData) &&
                ArrayUtils.AllEqual(x._rawMultiplicativeHunterData, y._rawMultiplicativeHunterData) &&
                ArrayUtils.AllEqual(x._rawNoStackHunterData, y._rawNoStackHunterData);
        }
        public static bool operator !=(StatsHunter x, StatsHunter y)
        {
            return !(x == y);
        }
        public static StatsHunter operator *(StatsHunter a, float b)
        {
            StatsHunter c = new StatsHunter();

            int i = c._rawAdditiveHunterData.Length;
            while (--i >= 0)
            {
                c._rawAdditiveHunterData[i] = a._rawAdditiveHunterData[i] * b;
            }
            i = c._rawMultiplicativeHunterData.Length;
            while (--i >= 0)
            {
                c._rawMultiplicativeHunterData[i] = a._rawMultiplicativeHunterData[i] * b;
            }
            i = c._rawInverseMultiplicativeHunterData.Length;
            while (--i >= 0)
            {
                c._rawInverseMultiplicativeHunterData[i] = a._rawInverseMultiplicativeHunterData[i] * b;
            }

            i = c._rawNoStackHunterData.Length;
            while (--i >= 0)
            {
                c._rawNoStackHunterData[i] = a._rawNoStackHunterData[i] * b;
            }
            return c;
        }

        public void Accumulate(StatsHunter data)
        {
            #region Base
            if (data.sparseIndices != null)
            {
                int i = 0;
                for (int a = 0; a < data.sparseAdditiveCount; a++, i++)
                {
                    int index = data.sparseIndices[i];
                    rawAdditiveData[index] += data.rawAdditiveData[index];
                }
                for (int a = 0; a < data.sparseMultiplicativeCount; a++, i++)
                {
                    int index = data.sparseIndices[i];
                    rawMultiplicativeData[index] = (1 + rawMultiplicativeData[index]) * (1 + data.rawMultiplicativeData[index]) - 1;
                }
                for (int a = 0; a < data.sparseInverseMultiplicativeCount; a++, i++)
                {
                    int index = data.sparseIndices[i];
                    rawInverseMultiplicativeData[index] = 1 - (1 - rawInverseMultiplicativeData[index]) * (1 - data.rawInverseMultiplicativeData[index]);
                }
                for (int a = 0; a < data.sparseNoStackCount; a++, i++)
                {
                    int index = data.sparseIndices[i];
                    float value = data.rawNoStackData[index];
                    if (value > rawNoStackData[index]) rawNoStackData[index] = value;
                }
            }
            else
            {
                float[] add = data.rawAdditiveData;
                for (int i = 0; i < rawAdditiveData.Length; i++)
                {
                    rawAdditiveData[i] += add[i];
                }
                add = data.rawMultiplicativeData;
                for (int i = 0; i < rawMultiplicativeData.Length; i++)
                {
                    rawMultiplicativeData[i] = (1 + rawMultiplicativeData[i]) * (1 + add[i]) - 1;
                }
                add = data.rawInverseMultiplicativeData;
                for (int i = 0; i < rawInverseMultiplicativeData.Length; i++)
                {
                    rawInverseMultiplicativeData[i] = 1 - (1 - rawInverseMultiplicativeData[i]) * (1 - add[i]);
                }
                add = data.rawNoStackData;
                for (int i = 0; i < rawNoStackData.Length; i++)
                {
                    if (add[i] > rawNoStackData[i]) rawNoStackData[i] = add[i];
                }
            }
            #endregion
            #region Hunter
            if (data._sparseIndicesHunter != null)
            {
                int i = 0;
                for (int a = 0; a < data._sparseAdditiveHunterCount; a++, i++)
                {
                    int index = data._sparseIndicesHunter[i];
                    _rawAdditiveHunterData[index] += data._rawAdditiveHunterData[index];
                }
                for (int a = 0; a < data._sparseMultiplicativeHunterCount; a++, i++)
                {
                    int index = data._sparseIndicesHunter[i];
                    _rawMultiplicativeHunterData[index] = (1 + _rawMultiplicativeHunterData[index]) * (1 + data._rawMultiplicativeHunterData[index]) - 1;
                }
                for (int a = 0; a < data._sparseInverseMultiplicativeHunterCount; a++, i++)
                {
                    int index = data._sparseIndicesHunter[i];
                    _rawInverseMultiplicativeHunterData[index] = 1 - (1 - _rawInverseMultiplicativeHunterData[index]) * (1 - data._rawInverseMultiplicativeHunterData[index]);
                }
                for (int a = 0; a < data._sparseNoStackHunterCount; a++, i++)
                {
                    int index = data._sparseIndicesHunter[i];
                    float value = data._rawNoStackHunterData[index];
                    if (value > _rawNoStackHunterData[index]) _rawNoStackHunterData[index] = value;
                }
            }
            else
            {
                float[] add = data._rawAdditiveHunterData;
                for (int i = 0; i < _rawAdditiveHunterData.Length; i++)
                {
                    _rawAdditiveHunterData[i] += add[i];
                }
                add = data._rawMultiplicativeHunterData;
                for (int i = 0; i < _rawMultiplicativeHunterData.Length; i++)
                {
                    _rawMultiplicativeHunterData[i] = (1 + _rawMultiplicativeHunterData[i]) * (1 + add[i]) - 1;
                }
                add = data._rawInverseMultiplicativeHunterData;
                for (int i = 0; i < _rawInverseMultiplicativeHunterData.Length; i++)
                {
                    _rawInverseMultiplicativeHunterData[i] = 1 - (1 - _rawInverseMultiplicativeHunterData[i]) * (1 - add[i]);
                }
                add = data._rawNoStackHunterData;
                for (int i = 0; i < _rawNoStackHunterData.Length; i++)
                {
                    if (add[i] > _rawNoStackHunterData[i]) _rawNoStackHunterData[i] = add[i];
                }
            }
            #endregion
            if (data._rawSpecialEffectDataSize > 0)
            {
                EnsureSpecialEffectCapacity(_rawSpecialEffectDataSize + data._rawSpecialEffectDataSize);
                Array.Copy(data._rawSpecialEffectData, 0, _rawSpecialEffectData, _rawSpecialEffectDataSize, data._rawSpecialEffectDataSize);
                _rawSpecialEffectDataSize += data._rawSpecialEffectDataSize;
            }
        }

        public void Accumulate(StatsHunter data, float weight)
        {
            #region Base
            if (data.sparseIndices != null)
            {
                int i = 0;
                for (int a = 0; a < data.sparseAdditiveCount; a++, i++)
                {
                    int index = data.sparseIndices[i];
                    rawAdditiveData[index] += weight * data.rawAdditiveData[index];
                }
                for (int a = 0; a < data.sparseMultiplicativeCount; a++, i++)
                {
                    int index = data.sparseIndices[i];
                    rawMultiplicativeData[index] = (1 + rawMultiplicativeData[index]) * (1 + weight * data.rawMultiplicativeData[index]) - 1;
                }
                for (int a = 0; a < data.sparseInverseMultiplicativeCount; a++, i++)
                {
                    int index = data.sparseIndices[i];
                    rawInverseMultiplicativeData[index] = 1 - (1 - rawInverseMultiplicativeData[index]) * (1 - weight * data.rawInverseMultiplicativeData[index]);
                }
                for (int a = 0; a < data.sparseNoStackCount; a++, i++)
                {
                    int index = data.sparseIndices[i];
                    float value = weight * data.rawNoStackData[index];
                    if (value > rawNoStackData[index]) rawNoStackData[index] = value;
                }
            }
            else
            {
                float[] add = data.rawAdditiveData;
                for (int i = 0; i < rawAdditiveData.Length; i++)
                {
                    rawAdditiveData[i] += weight * add[i];
                }
                add = data.rawMultiplicativeData;
                for (int i = 0; i < rawMultiplicativeData.Length; i++)
                {
                    rawMultiplicativeData[i] = (1 + rawMultiplicativeData[i]) * (1 + weight * add[i]) - 1;
                }
                add = data.rawInverseMultiplicativeData;
                for (int i = 0; i < rawInverseMultiplicativeData.Length; i++)
                {
                    rawInverseMultiplicativeData[i] = 1 - (1 - rawInverseMultiplicativeData[i]) * (1 - weight * add[i]);
                }
                add = data.rawNoStackData;
                for (int i = 0; i < rawNoStackData.Length; i++)
                {
                    if (weight * add[i] > rawNoStackData[i]) rawNoStackData[i] = weight * add[i];
                }
            }
            #endregion
            #region Hunter
            if (data._sparseIndicesHunter != null)
            {
                int i = 0;
                for (int a = 0; a < data._sparseAdditiveHunterCount; a++, i++)
                {
                    int index = data._sparseIndicesHunter[i];
                    _rawAdditiveHunterData[index] += weight * data._rawAdditiveHunterData[index];
                }
                for (int a = 0; a < data._sparseMultiplicativeHunterCount; a++, i++)
                {
                    int index = data._sparseIndicesHunter[i];
                    _rawMultiplicativeHunterData[index] = (1 + _rawMultiplicativeHunterData[index]) * (1 + weight * data._rawMultiplicativeHunterData[index]) - 1;
                }
                for (int a = 0; a < data._sparseInverseMultiplicativeHunterCount; a++, i++)
                {
                    int index = data._sparseIndicesHunter[i];
                    _rawInverseMultiplicativeHunterData[index] = 1 - (1 - _rawInverseMultiplicativeHunterData[index]) * (1 - weight * data._rawInverseMultiplicativeHunterData[index]);
                }
                for (int a = 0; a < data._sparseNoStackHunterCount; a++, i++)
                {
                    int index = data._sparseIndicesHunter[i];
                    float value = weight * data._rawNoStackHunterData[index];
                    if (value > _rawNoStackHunterData[index]) _rawNoStackHunterData[index] = value;
                }
            }
            else
            {
                float[] add = data._rawAdditiveHunterData;
                for (int i = 0; i < _rawAdditiveHunterData.Length; i++)
                {
                    _rawAdditiveHunterData[i] += weight * add[i];
                }
                add = data._rawMultiplicativeHunterData;
                for (int i = 0; i < _rawMultiplicativeHunterData.Length; i++)
                {
                    _rawMultiplicativeHunterData[i] = (1 + _rawMultiplicativeHunterData[i]) * (1 + weight * add[i]) - 1;
                }
                add = data._rawInverseMultiplicativeHunterData;
                for (int i = 0; i < _rawInverseMultiplicativeHunterData.Length; i++)
                {
                    _rawInverseMultiplicativeHunterData[i] = 1 - (1 - _rawInverseMultiplicativeHunterData[i]) * (1 - weight * add[i]);
                }
                add = data._rawNoStackHunterData;
                for (int i = 0; i < _rawNoStackHunterData.Length; i++)
                {
                    if (weight * add[i] > _rawNoStackHunterData[i]) _rawNoStackHunterData[i] = weight * add[i];
                }
            }
            #endregion
            if (data._rawSpecialEffectDataSize > 0)
            {
                EnsureSpecialEffectCapacity(_rawSpecialEffectDataSize + data._rawSpecialEffectDataSize);
                Array.Copy(data._rawSpecialEffectData, 0, _rawSpecialEffectData, _rawSpecialEffectDataSize, data._rawSpecialEffectDataSize);
                _rawSpecialEffectDataSize += data._rawSpecialEffectDataSize;
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            foreach (PropertyInfo info in PropertyInfoCache)
            {
                float value = (float)info.GetValue(this, null);
                if (value != 0)
                {
                    if (IsPercentage(info)) { value *= 100; }
                    value = (float)Math.Round(value * 100f) / 100f;
                    sb.AppendFormat("{0}{1}, ", value, Extensions.DisplayName(info));
                }
            }
            foreach (PropertyInfo info in PropertyInfoCacheHunter)
            {
                float value = (float)info.GetValue(this, null);
                if (value != 0)
                {
                    if (IsPercentage(info)) { value *= 100; }
                    value = (float)Math.Round(value * 100f) / 100f;
                    sb.AppendFormat("{0}{1}, ", value, Extensions.DisplayName(info));
                }
            }
            foreach (SpecialEffect effect in SpecialEffects())
            {
                sb.AppendFormat("{0}, ", effect);
            }

            return sb.ToString().TrimEnd(' ', ',');
        }
        #endregion

        static StatsHunter()
        {
            List<PropertyInfo> items = new List<PropertyInfo>();
            List<PropertyInfo> itemsHunter = new List<PropertyInfo>();

            foreach (PropertyInfo info in typeof(StatsHunter).GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.ExactBinding))
            {
                if (info.PropertyType.IsAssignableFrom(typeof(float)))
                {
                    itemsHunter.Add(info);
                }
            }
            _propertyInfoCacheHunter = itemsHunter.ToArray();

            foreach (PropertyInfo info in _propertyInfoCacheHunter)
            {
                if (info.GetCustomAttributes(typeof(PercentageAttribute), false).Length > 0)
                {
                    _percentagePropertiesHunter.Add(info);
                }
            }

            AdditiveStatHunterCount = EnumHelper.GetValues(typeof(AdditiveStatHunter)).Length;
            MultiplicativeStatHunterCount = EnumHelper.GetValues(typeof(MultiplicativeStatHunter)).Length;
            InverseMultiplicativeStatHunterCount = EnumHelper.GetValues(typeof(InverseMultiplicativeStatHunter)).Length;
            NonStackingStatHunterCount = EnumHelper.GetValues(typeof(NonStackingStatHunter)).Length;
        }

        #region ===== Additive Stats ==================
        /// <summary>% Extra Pet Crit Chance</summary>
        [DefaultValueAttribute(0f)]
        [Percentage]
        [DisplayName("% Extra Pet Crit Chance")]
        [Category("Hunter")]
        public float BonusPetCritChance
        {
            get { return _rawAdditiveHunterData[(int)AdditiveStatHunter.BonusPetCritChance]; }
            set { _rawAdditiveHunterData[(int)AdditiveStatHunter.BonusPetCritChance] = value; }
        }
        /// <summary>Pet Stamina</summary>
        [DefaultValueAttribute(0f)]
        [DisplayName("Pet Stamina")]
        [Category("Hunter")]
        public float PetStamina
        {
            get { return _rawAdditiveHunterData[(int)AdditiveStatHunter.PetStamina]; }
            set { _rawAdditiveHunterData[(int)AdditiveStatHunter.PetStamina] = value; }
        }
        /// <summary>Pet Strength</summary>
        [DefaultValueAttribute(0f)]
        [DisplayName("Pet Strength")]
        [Category("Hunter")]
        public float PetStrength
        {
            get { return _rawAdditiveHunterData[(int)AdditiveStatHunter.PetStrength]; }
            set { _rawAdditiveHunterData[(int)AdditiveStatHunter.PetStrength] = value; }
        }
        /// <summary>Pet Spirit</summary>
        [DefaultValueAttribute(0f)]
        [DisplayName("Pet Spirit")]
        [Category("Hunter")]
        public float PetSpirit
        {
            get { return _rawAdditiveHunterData[(int)AdditiveStatHunter.PetSpirit]; }
            set { _rawAdditiveHunterData[(int)AdditiveStatHunter.PetSpirit] = value; }
        }
        /// <summary>Pet Attack Power</summary>
        [DefaultValueAttribute(0f)]
        [DisplayName("Pet Attack Power")]
        [Category("Hunter")]
        public float PetAttackPower
        {
            get { return _rawAdditiveHunterData[(int)AdditiveStatHunter.PetAttackPower]; }
            set { _rawAdditiveHunterData[(int)AdditiveStatHunter.PetAttackPower] = value; }
        }
        #endregion

        #region ===== Multiplicative Stats ============
        /// <summary>Bonus Ranged Attack Power Multiplier</summary>
        [DefaultValueAttribute(0f)]
        [Percentage]
        [DisplayName("% Bonus Ranged AP")]
        [Category("Hunter")]
        public float BonusRangedAttackPowerMultiplier
        {
            get { return _rawAdditiveHunterData[(int)MultiplicativeStatHunter.BonusRangedAttackPowerMultiplier]; }
            set { _rawAdditiveHunterData[(int)MultiplicativeStatHunter.BonusRangedAttackPowerMultiplier] = value; }
        }
        /// <summary>Bonus Pet Attack Power Multiplier</summary>
        [DefaultValueAttribute(0f)]
        [Percentage]
        [DisplayName("% Bonus Pet AP")]
        [Category("Hunter")]
        public float BonusPetAttackPowerMultiplier
        {
            get { return _rawAdditiveHunterData[(int)MultiplicativeStatHunter.BonusPetAttackPowerMultiplier]; }
            set { _rawAdditiveHunterData[(int)MultiplicativeStatHunter.BonusPetAttackPowerMultiplier] = value; }
        }
        #endregion

        #region Set Bonuses
        public bool Gladiator_2pc { get; set; }
        public bool Gladiator_4pc { get; set; }

        public bool Tier_11_2pc { get; set; }
        public bool Tier_11_4pc { get; set; }

        public bool Tier_12_2pc { get; set; }
        public bool Tier_12_4pc { get; set; }
        #endregion

        public void SetSets(Character character)
        {
            int TCount;
            // Gladiator
            character.SetBonusCount.TryGetValue("Gladiator's Pursuit", out TCount);
            if (TCount >= 2) { Gladiator_2pc = true; }
            if (TCount >= 4) { Gladiator_4pc = true; }

            //T11
            character.SetBonusCount.TryGetValue("Lightning-Charged Battlegear", out TCount);
            if (TCount >= 2) { Tier_11_2pc = true; }
            if (TCount >= 4) { Tier_11_4pc = true; }

            //T12
            character.SetBonusCount.TryGetValue("Flamewaker's Battlegear", out TCount);
            if (TCount >= 2) { Tier_12_2pc = true; }
            if (TCount >= 4) { Tier_12_4pc = true; }
        }

        internal float[] _rawAdditiveData = new float[AdditiveStatCount];
        internal float[] _rawMultiplicativeData = new float[MultiplicativeStatCount];
        internal float[] _rawInverseMultiplicativeData = new float[InverseMultiplicativeStatCount];
        internal float[] _rawNoStackData = new float[NonStackingStatCount];

        public static StatsHunter operator *(StatsHunter a, float b)
        {
            StatsHunter c = new StatsHunter();

            int i = c._rawAdditiveData.Length;
            while (--i >= 0)
            {
                c._rawAdditiveData[i] = a._rawAdditiveData[i] * b;
            }
            i = c._rawMultiplicativeData.Length;
            while (--i >= 0)
            {
                c._rawMultiplicativeData[i] = a._rawMultiplicativeData[i] * b;
            }
            i = c._rawInverseMultiplicativeData.Length;
            while (--i >= 0)
            {
                c._rawInverseMultiplicativeData[i] = a._rawInverseMultiplicativeData[i] * b;
            }

            i = c._rawNoStackData.Length;
            while (--i >= 0)
            {
                c._rawNoStackData[i] = a._rawNoStackData[i] * b;
            }
            // undefined for special effects
            return c;
        }


        public virtual Stats GetItemStats(Character character, Item additionalItem)
        {
            StatsHunter stats = new StatsHunter();
            AccumulateItemStats(stats, character, additionalItem);
            return stats;
        }
    }
}
