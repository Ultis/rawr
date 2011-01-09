using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Xml.Serialization;

namespace Rawr.Base
{
    public enum AdditiveStatWarrior : int
    {
        // Prot
        CriticalBlock,
        //BonusDevastateCritChance, // Deprecated
        // DPS
        // Both
        InterceptCDReduc,
    }
    public enum MultiplicativeStatWarrior : int {
        // Prot
        BonusShieldSlamDamageMultiplier,
        BonusDevastateDamageMultiplier,
        BonusShockwaveDamageMultiplier,
        BonusShieldWallDurMultiplier,
        // DPS
        BonusExecuteDamageMultiplier,
        BonusOverpowerDamageMultiplier,
        BonusMortalStrikeDamageMultiplier,
        BonusBloodthirstDamageMultiplier,
        BonusRagingBlowDamageMultiplier,
        BonusWhirlwindDamageMultiplier,
        BonusSlamDamageMultiplier,
        BonusVictoryRushDamageMultiplier,
        // Both
        BonusHeroicStrikeDamageMultiplier,
        BonusCleaveDamageMultiplier,
        RageCostMultiplier,
    }
    public enum InverseMultiplicativeStatWarrior : int { }
    public enum NonStackingStatWarrior : int { }

#if SILVERLIGHT
    public class StatsWarrior : Stats
#else    
    public unsafe class StatsWarrior : Stats
#endif
    {
        #region Arrays, duplicated from Base to be populated by Warrior Stats
        internal float[] _rawAdditiveWarriorData = new float[AdditiveStatWarriorCount];
        internal float[] _rawMultiplicativeWarriorData = new float[MultiplicativeStatWarriorCount];
        internal float[] _rawInverseMultiplicativeWarriorData = new float[InverseMultiplicativeStatWarriorCount];
        internal float[] _rawNoStackWarriorData = new float[NonStackingStatWarriorCount];

        internal int[] _sparseIndicesWarrior;
        internal int _sparseAdditiveWarriorCount;
        internal int _sparseMultiplicativeWarriorCount;
        internal int _sparseInverseMultiplicativeWarriorCount;
        internal int _sparseNoStackWarriorCount;

        protected static PropertyInfo[] _propertyInfoCacheWarrior = null;
        protected static List<PropertyInfo> _percentagePropertiesWarrior = new List<PropertyInfo>();
        protected static int AdditiveStatWarriorCount = 0;
        protected static int MultiplicativeStatWarriorCount = 0;
        protected static int InverseMultiplicativeStatWarriorCount = 0;
        protected static int NonStackingStatWarriorCount = 0;
        #endregion

        #region Sort-Of Overrides, they call the base function and then do the same thing with Warrior Stats
        public new void Clear()
        {
            base.Clear();
            Array.Clear(_rawAdditiveWarriorData, 0, _rawAdditiveWarriorData.Length);
            Array.Clear(_rawMultiplicativeWarriorData, 0, _rawMultiplicativeWarriorData.Length);
            Array.Clear(_rawInverseMultiplicativeWarriorData, 0, _rawInverseMultiplicativeWarriorData.Length);
            Array.Clear(_rawNoStackWarriorData, 0, _rawNoStackWarriorData.Length);
            _rawSpecialEffectDataSize = 0;
        }

        public new void InvalidateSparseData()
        {
            base.InvalidateSparseData();
            _sparseIndicesWarrior = null;
        }

        public new void GenerateSparseData() {
            base.GenerateSparseData();
            //List<float> data = new List<float>();
            if (_sparseIndicesWarrior != null) return;
            lock (_rawAdditiveWarriorData)
            {
                if (_sparseIndicesWarrior == null)
                {
                    List<int> indices = new List<int>();
                    _sparseAdditiveWarriorCount = 0;
                    for (int i = 0; i < _rawAdditiveWarriorData.Length; i++)
                    {
                        if (_rawAdditiveWarriorData[i] != 0.0f)
                        {
                            _sparseAdditiveWarriorCount++;
                            //data.Add(_rawAdditiveWarriorData[i]);
                            indices.Add(i);
                        }
                    }
                    _sparseMultiplicativeWarriorCount = 0;
                    for (int i = 0; i < _rawMultiplicativeWarriorData.Length; i++)
                    {
                        if (_rawMultiplicativeWarriorData[i] != 0.0f)
                        {
                            _sparseMultiplicativeWarriorCount++;
                            //data.Add(_rawMultiplicativeWarriorData[i]);
                            indices.Add(i);
                        }
                    }
                    _sparseInverseMultiplicativeWarriorCount = 0;
                    for (int i = 0; i < _rawInverseMultiplicativeWarriorData.Length; i++)
                    {
                        if (_rawInverseMultiplicativeWarriorData[i] != 0.0f)
                        {
                            _sparseInverseMultiplicativeWarriorCount++;
                            //data.Add(_rawInverseMultiplicativeWarriorData[i]);
                            indices.Add(i);
                        }
                    }
                    _sparseNoStackWarriorCount = 0;
                    for (int i = 0; i < _rawNoStackWarriorData.Length; i++)
                    {
                        if (_rawNoStackWarriorData[i] != 0.0f)
                        {
                            _sparseNoStackWarriorCount++;
                            //data.Add(_rawNoStackWarriorData[i]);
                            indices.Add(i);
                        }
                    }
                    //_sparseWarriorData = data.ToArray();
                    _sparseIndicesWarrior = indices.ToArray();
                }
            }
        }

        public new StatsWarrior Clone() {
            StatsWarrior clone = (StatsWarrior)this.MemberwiseClone();
            StatsWarrior retVal = new StatsWarrior();
            //
            retVal.Accumulate(base.Clone());
            //
            retVal._rawAdditiveWarriorData = (float[])clone._rawAdditiveWarriorData.Clone();
            retVal._rawMultiplicativeWarriorData = (float[])clone._rawMultiplicativeWarriorData.Clone();
            retVal._rawInverseMultiplicativeWarriorData = (float[])clone._rawInverseMultiplicativeWarriorData.Clone();
            retVal._rawNoStackWarriorData = (float[])clone._rawNoStackWarriorData.Clone();
            //
            return retVal;
        }

        public static PropertyInfo[] PropertyInfoCacheWarrior { get { return _propertyInfoCacheWarrior; } }

        [XmlIgnore]
        public static String[] StatNamesWarrior {
            get {
                String[] names = new string[PropertyInfoCacheWarrior.Length];
                for (int i = 0; i < PropertyInfoCacheWarrior.Length; i++) {
                    names[i] = Extensions.DisplayName(PropertyInfoCacheWarrior[i]);
                }
                Array.Sort(names);
                return names;
            }
        }
        public IDictionary<PropertyInfo, float> ValuesWarrior(StatFilter filter)
        {
#if SILVERLIGHT
            Dictionary<PropertyInfo, float> dict = new Dictionary<PropertyInfo, float>();
#else
            SortedList<PropertyInfo, float> dict = new SortedList<PropertyInfo, float>(new PropertyComparer());
#endif
            foreach (PropertyInfo info in PropertyInfoCacheWarrior)
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
            dict.OrderBy(kvp => kvp.Key, new PropertyComparer());
#endif
            return dict;
        }

        public override int GetHashCode()
        {
            return _rawAdditiveWarriorData.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return this == (obj as StatsWarrior);
        }
        public static bool operator ==(StatsWarrior x, StatsWarrior y)
        {
            if (ReferenceEquals(x, y) || (ReferenceEquals(x, null) && ReferenceEquals(y, null)))
                return true;
            if (ReferenceEquals(x, null) || ReferenceEquals(y, null))
                return false;
            if (x._rawSpecialEffectDataSize > 0 || y._rawSpecialEffectDataSize > 0) return false;
            return ArrayUtils.AllEqual(x.rawAdditiveData, y.rawAdditiveData) &&
                ArrayUtils.AllEqual(x.rawMultiplicativeData, y.rawMultiplicativeData) &&
                ArrayUtils.AllEqual(x.rawNoStackData, y.rawNoStackData) &&
                ArrayUtils.AllEqual(x._rawAdditiveWarriorData, y._rawAdditiveWarriorData) &&
                ArrayUtils.AllEqual(x._rawMultiplicativeWarriorData, y._rawMultiplicativeWarriorData) &&
                ArrayUtils.AllEqual(x._rawNoStackWarriorData, y._rawNoStackWarriorData);
        }
        public static bool operator !=(StatsWarrior x, StatsWarrior y)
        {
            return !(x == y);
        }

        public void Accumulate(StatsWarrior data)
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
            #region Warrior
            if (data._sparseIndicesWarrior != null)
            {
                int i = 0;
                for (int a = 0; a < data._sparseAdditiveWarriorCount; a++, i++)
                {
                    int index = data._sparseIndicesWarrior[i];
                    _rawAdditiveWarriorData[index] += data._rawAdditiveWarriorData[index];
                }
                for (int a = 0; a < data._sparseMultiplicativeWarriorCount; a++, i++)
                {
                    int index = data._sparseIndicesWarrior[i];
                    _rawMultiplicativeWarriorData[index] = (1 + _rawMultiplicativeWarriorData[index]) * (1 + data._rawMultiplicativeWarriorData[index]) - 1;
                }
                for (int a = 0; a < data._sparseInverseMultiplicativeWarriorCount; a++, i++)
                {
                    int index = data._sparseIndicesWarrior[i];
                    _rawInverseMultiplicativeWarriorData[index] = 1 - (1 - _rawInverseMultiplicativeWarriorData[index]) * (1 - data._rawInverseMultiplicativeWarriorData[index]);
                }
                for (int a = 0; a < data._sparseNoStackWarriorCount; a++, i++)
                {
                    int index = data._sparseIndicesWarrior[i];
                    float value = data._rawNoStackWarriorData[index];
                    if (value > _rawNoStackWarriorData[index]) _rawNoStackWarriorData[index] = value;
                }
            }
            else
            {
                float[] add = data._rawAdditiveWarriorData;
                for (int i = 0; i < _rawAdditiveWarriorData.Length; i++)
                {
                    _rawAdditiveWarriorData[i] += add[i];
                }
                add = data._rawMultiplicativeWarriorData;
                for (int i = 0; i < _rawMultiplicativeWarriorData.Length; i++)
                {
                    _rawMultiplicativeWarriorData[i] = (1 + _rawMultiplicativeWarriorData[i]) * (1 + add[i]) - 1;
                }
                add = data._rawInverseMultiplicativeWarriorData;
                for (int i = 0; i < _rawInverseMultiplicativeWarriorData.Length; i++)
                {
                    _rawInverseMultiplicativeWarriorData[i] = 1 - (1 - _rawInverseMultiplicativeWarriorData[i]) * (1 - add[i]);
                }
                add = data._rawNoStackWarriorData;
                for (int i = 0; i < _rawNoStackWarriorData.Length; i++)
                {
                    if (add[i] > _rawNoStackWarriorData[i]) _rawNoStackWarriorData[i] = add[i];
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

        public void Accumulate(StatsWarrior data, float weight)
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
            #region Warrior
            if (data._sparseIndicesWarrior != null)
            {
                int i = 0;
                for (int a = 0; a < data._sparseAdditiveWarriorCount; a++, i++)
                {
                    int index = data._sparseIndicesWarrior[i];
                    _rawAdditiveWarriorData[index] += weight * data._rawAdditiveWarriorData[index];
                }
                for (int a = 0; a < data._sparseMultiplicativeWarriorCount; a++, i++)
                {
                    int index = data._sparseIndicesWarrior[i];
                    _rawMultiplicativeWarriorData[index] = (1 + _rawMultiplicativeWarriorData[index]) * (1 + weight * data._rawMultiplicativeWarriorData[index]) - 1;
                }
                for (int a = 0; a < data._sparseInverseMultiplicativeWarriorCount; a++, i++)
                {
                    int index = data._sparseIndicesWarrior[i];
                    _rawInverseMultiplicativeWarriorData[index] = 1 - (1 - _rawInverseMultiplicativeWarriorData[index]) * (1 - weight * data._rawInverseMultiplicativeWarriorData[index]);
                }
                for (int a = 0; a < data._sparseNoStackWarriorCount; a++, i++)
                {
                    int index = data._sparseIndicesWarrior[i];
                    float value = weight * data._rawNoStackWarriorData[index];
                    if (value > _rawNoStackWarriorData[index]) _rawNoStackWarriorData[index] = value;
                }
            }
            else
            {
                float[] add = data._rawAdditiveWarriorData;
                for (int i = 0; i < _rawAdditiveWarriorData.Length; i++)
                {
                    _rawAdditiveWarriorData[i] += weight * add[i];
                }
                add = data._rawMultiplicativeWarriorData;
                for (int i = 0; i < _rawMultiplicativeWarriorData.Length; i++)
                {
                    _rawMultiplicativeWarriorData[i] = (1 + _rawMultiplicativeWarriorData[i]) * (1 + weight * add[i]) - 1;
                }
                add = data._rawInverseMultiplicativeWarriorData;
                for (int i = 0; i < _rawInverseMultiplicativeWarriorData.Length; i++)
                {
                    _rawInverseMultiplicativeWarriorData[i] = 1 - (1 - _rawInverseMultiplicativeWarriorData[i]) * (1 - weight * add[i]);
                }
                add = data._rawNoStackWarriorData;
                for (int i = 0; i < _rawNoStackWarriorData.Length; i++)
                {
                    if (weight * add[i] > _rawNoStackWarriorData[i]) _rawNoStackWarriorData[i] = weight * add[i];
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

            foreach (PropertyInfo info in PropertyInfoCache) {
                float value = (float)info.GetValue(this, null);
                if (value != 0) {
                    if (IsPercentage(info)) { value *= 100; }
                    value = (float)Math.Round(value * 100f) / 100f;
                    sb.AppendFormat("{0}{1}, ", value, Extensions.DisplayName(info));
                }
            }
            foreach (PropertyInfo info in PropertyInfoCacheWarrior) {
                float value = (float)info.GetValue(this, null);
                if (value != 0) {
                    if (IsPercentage(info)) { value *= 100; }
                    value = (float)Math.Round(value * 100f) / 100f;
                    sb.AppendFormat("{0}{1}, ", value, Extensions.DisplayName(info));
                }
            }
            foreach (SpecialEffect effect in SpecialEffects()) {
                sb.AppendFormat("{0}, ", effect);
            }

            return sb.ToString().TrimEnd(' ', ',');
        }
        #endregion

        static StatsWarrior()
        {
            List<PropertyInfo> items = new List<PropertyInfo>();
            List<PropertyInfo> itemsWarrior = new List<PropertyInfo>();

            /*foreach (PropertyInfo info in typeof(Stats).GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.ExactBinding))
            {
                if (info.PropertyType.IsAssignableFrom(typeof(float)))
                {
                    items.Add(info);
                }
            }
            _propertyInfoCache = items.ToArray();*/
            foreach (PropertyInfo info in typeof(StatsWarrior).GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.ExactBinding))
            {
                if (info.PropertyType.IsAssignableFrom(typeof(float)))
                {
                    itemsWarrior.Add(info);
                }
            }
            _propertyInfoCacheWarrior = itemsWarrior.ToArray();

            /*foreach (PropertyInfo info in _propertyInfoCache)
            {
                if (info.GetCustomAttributes(typeof(PercentageAttribute), false).Length > 0)
                {
                    _percentageProperties.Add(info);
                }
            }*/
            foreach (PropertyInfo info in _propertyInfoCacheWarrior)
            {
                if (info.GetCustomAttributes(typeof(PercentageAttribute), false).Length > 0)
                {
                    _percentagePropertiesWarrior.Add(info);
                }
            }

            /*AdditiveStatCount = EnumHelper.GetValues(typeof(AdditiveStat)).Length;
            MultiplicativeStatCount = EnumHelper.GetValues(typeof(MultiplicativeStat)).Length;
            InverseMultiplicativeStatCount = EnumHelper.GetValues(typeof(InverseMultiplicativeStat)).Length;
            NonStackingStatCount = EnumHelper.GetValues(typeof(NonStackingStat)).Length;*/

            AdditiveStatWarriorCount = EnumHelper.GetValues(typeof(AdditiveStatWarrior)).Length;
            MultiplicativeStatWarriorCount = EnumHelper.GetValues(typeof(MultiplicativeStatWarrior)).Length;
            InverseMultiplicativeStatWarriorCount = EnumHelper.GetValues(typeof(InverseMultiplicativeStatWarrior)).Length;
            NonStackingStatWarriorCount = EnumHelper.GetValues(typeof(NonStackingStatWarrior)).Length;
        }

        #region Attributes as Callable and Serializable points
        #region Additive Stats
        [DefaultValueAttribute(0f)]
        [Percentage]
        [DisplayName("% Critical Block")]
        [Category("Warrior")]
        public float CriticalBlock
        {
            get { return _rawAdditiveWarriorData[(int)AdditiveStatWarrior.CriticalBlock]; }
            set { _rawAdditiveWarriorData[(int)AdditiveStatWarrior.CriticalBlock] = value; }
        }

        /// <summary>Your Intercept abilities cooldown is reduced by 5 sec.</summary>
        [DefaultValueAttribute(0f)]
        [Category("Warrior")]
        [DisplayName("second Intercept Cooldown Reduction")]
        public float BonusWarrior_PvP_4P_InterceptCDReduc
        {
            get { return _rawAdditiveWarriorData[(int)AdditiveStatWarrior.InterceptCDReduc]; }
            set { _rawAdditiveWarriorData[(int)AdditiveStatWarrior.InterceptCDReduc] = value; }
        }

        /* Deprecated
        [DefaultValueAttribute(0f)]
        [Percentage]
        [Category("Warrior")]
        [DisplayName("% increased critical strike chance on Devastate")]
        public float BonusDevastateCritChance
        {
            get { return _rawAdditiveWarriorData[(int)AdditiveStatWarrior.BonusDevastateCritChance]; }
            set { _rawAdditiveWarriorData[(int)AdditiveStatWarrior.BonusDevastateCritChance] = value; }
        }*/
        #endregion
        #region Multiplicative Stats
        #region Prot
        [DefaultValueAttribute(0f)]
        [Percentage]
        [DisplayName("% Bonus Shield Slam Damage")]
        [Category("Warrior")]
        public float BonusShieldSlamDamageMultiplier
        {
            get { return _rawMultiplicativeWarriorData[(int)MultiplicativeStatWarrior.BonusShieldSlamDamageMultiplier]; }
            set { _rawMultiplicativeWarriorData[(int)MultiplicativeStatWarrior.BonusShieldSlamDamageMultiplier] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Percentage]
        [Category("Warrior")]
        [DisplayName("% Bonus Devastate Damage")]
        public float BonusDevastateDamageMultiplier
        {
            get { return _rawMultiplicativeWarriorData[(int)MultiplicativeStatWarrior.BonusDevastateDamageMultiplier]; }
            set { _rawMultiplicativeWarriorData[(int)MultiplicativeStatWarrior.BonusDevastateDamageMultiplier] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Percentage]
        [Category("Warrior")]
        [DisplayName("% Bonus Shockwave Damage")]
        public float BonusShockwaveDamageMultiplier
        {
            get { return _rawMultiplicativeWarriorData[(int)MultiplicativeStatWarrior.BonusShockwaveDamageMultiplier]; }
            set { _rawMultiplicativeWarriorData[(int)MultiplicativeStatWarrior.BonusShockwaveDamageMultiplier] = value; }
        }

        /// <summary>Increases the duration of your Shield Wall ability by 50%.</summary>
        [DefaultValueAttribute(0f)]
        [Percentage]
        [Category("Warrior")]
        [DisplayName("% duration increase for your Shield Wall ability")]
        public float BonusShieldWallDurMultiplier
        {
            get { return _rawMultiplicativeWarriorData[(int)MultiplicativeStatWarrior.BonusShieldWallDurMultiplier]; }
            set { _rawMultiplicativeWarriorData[(int)MultiplicativeStatWarrior.BonusShieldWallDurMultiplier] = value; }
        }
        #endregion
        #region DPS
        [DefaultValueAttribute(0f)]
        [Percentage]
        [Category("Warrior")]
        [DisplayName("% Bonus Execute Damage")]
        public float BonusExecuteDamageMultiplier
        {
            get { return _rawMultiplicativeWarriorData[(int)MultiplicativeStatWarrior.BonusExecuteDamageMultiplier]; }
            set { _rawMultiplicativeWarriorData[(int)MultiplicativeStatWarrior.BonusExecuteDamageMultiplier] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Percentage]
        [Category("Warrior")]
        [DisplayName("% Bonus Overpower Damage")]
        public float BonusOverpowerDamageMultiplier
        {
            get { return _rawMultiplicativeWarriorData[(int)MultiplicativeStatWarrior.BonusOverpowerDamageMultiplier]; }
            set { _rawMultiplicativeWarriorData[(int)MultiplicativeStatWarrior.BonusOverpowerDamageMultiplier] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Percentage]
        [Category("Warrior")]
        [DisplayName("% Bonus Mortal Strike Damage")]
        public float BonusMortalStrikeDamageMultiplier
        {
            get { return _rawMultiplicativeWarriorData[(int)MultiplicativeStatWarrior.BonusMortalStrikeDamageMultiplier]; }
            set { _rawMultiplicativeWarriorData[(int)MultiplicativeStatWarrior.BonusMortalStrikeDamageMultiplier] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Percentage]
        [Category("Warrior")]
        [DisplayName("% Bonus Slam Damage")]
        public float BonusSlamDamageMultiplier
        {
            get { return _rawMultiplicativeWarriorData[(int)MultiplicativeStatWarrior.BonusSlamDamageMultiplier]; }
            set { _rawMultiplicativeWarriorData[(int)MultiplicativeStatWarrior.BonusSlamDamageMultiplier] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Percentage]
        [Category("Warrior")]
        [DisplayName("% Bonus Raging Blow Damage")]
        public float BonusRagingBlowDamageMultiplier
        {
            get { return _rawMultiplicativeWarriorData[(int)MultiplicativeStatWarrior.BonusRagingBlowDamageMultiplier]; }
            set { _rawMultiplicativeWarriorData[(int)MultiplicativeStatWarrior.BonusRagingBlowDamageMultiplier] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Percentage]
        [Category("Warrior")]
        [DisplayName("% Bonus Victory Rush Damage")]
        public float BonusVictoryRushDamageMultiplier
        {
            get { return _rawMultiplicativeWarriorData[(int)MultiplicativeStatWarrior.BonusVictoryRushDamageMultiplier]; }
            set { _rawMultiplicativeWarriorData[(int)MultiplicativeStatWarrior.BonusVictoryRushDamageMultiplier] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Percentage]
        [Category("Warrior")]
        [DisplayName("% Bonus Bloodthirst Damage")]
        public float BonusBloodthirstDamageMultiplier
        {
            get { return _rawMultiplicativeWarriorData[(int)MultiplicativeStatWarrior.BonusBloodthirstDamageMultiplier]; }
            set { _rawMultiplicativeWarriorData[(int)MultiplicativeStatWarrior.BonusBloodthirstDamageMultiplier] = value; }
        }
        #endregion
        #region Both
        [DefaultValueAttribute(0f)]
        [Percentage]
        [Category("Warrior")]
        [DisplayName("% Bonus Heroic Strike Damage")]
        public float BonusHeroicStrikeDamageMultiplier
        {
            get { return _rawMultiplicativeWarriorData[(int)MultiplicativeStatWarrior.BonusHeroicStrikeDamageMultiplier]; }
            set { _rawMultiplicativeWarriorData[(int)MultiplicativeStatWarrior.BonusHeroicStrikeDamageMultiplier] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Percentage]
        [Category("Warrior")]
        [DisplayName("% Bonus Cleave Damage")]
        public float BonusCleaveDamageMultiplier
        {
            get { return _rawMultiplicativeWarriorData[(int)MultiplicativeStatWarrior.BonusCleaveDamageMultiplier]; }
            set { _rawMultiplicativeWarriorData[(int)MultiplicativeStatWarrior.BonusCleaveDamageMultiplier] = value; }
        }
        
        [DefaultValueAttribute(0f)]
        [Percentage]
        [Category("Warrior")]
        [DisplayName("% Bonus Whirlwind Damage")]
        public float BonusWhirlwindDamageMultiplier
        {
            get { return _rawMultiplicativeWarriorData[(int)MultiplicativeStatWarrior.BonusWhirlwindDamageMultiplier]; }
            set { _rawMultiplicativeWarriorData[(int)MultiplicativeStatWarrior.BonusWhirlwindDamageMultiplier] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Percentage]
        [Category("Warrior")]
        [DisplayName("% Rage Cost Multiplier")]
        public float RageCostMultiplier
        {
            get { return _rawMultiplicativeWarriorData[(int)MultiplicativeStatWarrior.RageCostMultiplier]; }
            set { _rawMultiplicativeWarriorData[(int)MultiplicativeStatWarrior.RageCostMultiplier] = value; }
        }
        #endregion
        #endregion
        #endregion
    }
}
