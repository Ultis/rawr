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
    public enum AdditiveStatPaladin : int
    {
        // Prot
        // Ret
        // Holy
        // Both
    }
    public enum MultiplicativeStatPaladin : int {
        // Prot
        BonusDamageMultiplierCrusaderStrike,
        BonusDurationMultiplierGuardianOfAncientKings,
        BonusDamageShieldofRighteous,
        // Ret
        // Holy
        // Both
    }
    public enum InverseMultiplicativeStatPaladin : int { }
    public enum NonStackingStatPaladin : int { }

#if SILVERLIGHT
    public class StatsPaladin : Stats
#else
    public unsafe class StatsPaladin : Stats
#endif
    {
        #region Arrays, duplicated from Base to be populated by Paladin Stats
        internal float[] _rawAdditivePaladinData = new float[AdditiveStatPaladinCount];
        internal float[] _rawMultiplicativePaladinData = new float[MultiplicativeStatPaladinCount];
        internal float[] _rawInverseMultiplicativePaladinData = new float[InverseMultiplicativeStatPaladinCount];
        internal float[] _rawNoStackPaladinData = new float[NonStackingStatPaladinCount];

        internal int[] _sparseIndicesPaladin;
        internal int _sparseAdditivePaladinCount;
        internal int _sparseMultiplicativePaladinCount;
        internal int _sparseInverseMultiplicativePaladinCount;
        internal int _sparseNoStackPaladinCount;

        protected static PropertyInfo[] _propertyInfoCachePaladin = null;
        protected static List<PropertyInfo> _percentagePropertiesPaladin = new List<PropertyInfo>();
        protected static int AdditiveStatPaladinCount = 0;
        protected static int MultiplicativeStatPaladinCount = 0;
        protected static int InverseMultiplicativeStatPaladinCount = 0;
        protected static int NonStackingStatPaladinCount = 0;
        #endregion

        #region Sort-Of Overrides, they call the base function and then do the same thing with Paladin Stats
        public new void Clear()
        {
            base.Clear();
            Array.Clear(_rawAdditivePaladinData, 0, _rawAdditivePaladinData.Length);
            Array.Clear(_rawMultiplicativePaladinData, 0, _rawMultiplicativePaladinData.Length);
            Array.Clear(_rawInverseMultiplicativePaladinData, 0, _rawInverseMultiplicativePaladinData.Length);
            Array.Clear(_rawNoStackPaladinData, 0, _rawNoStackPaladinData.Length);
            _rawSpecialEffectDataSize = 0;
        }

        public new void InvalidateSparseData()
        {
            base.InvalidateSparseData();
            _sparseIndicesPaladin = null;
        }

        public new void GenerateSparseData() {
            base.GenerateSparseData();
            //List<float> data = new List<float>();
            if (_sparseIndicesPaladin != null) return;
            lock (_rawAdditivePaladinData)
            {
                if (_sparseIndicesPaladin == null)
                {
                    List<int> indices = new List<int>();
                    _sparseAdditivePaladinCount = 0;
                    for (int i = 0; i < _rawAdditivePaladinData.Length; i++)
                    {
                        if (_rawAdditivePaladinData[i] != 0.0f)
                        {
                            _sparseAdditivePaladinCount++;
                            //data.Add(_rawAdditivePaladinData[i]);
                            indices.Add(i);
                        }
                    }
                    _sparseMultiplicativePaladinCount = 0;
                    for (int i = 0; i < _rawMultiplicativePaladinData.Length; i++)
                    {
                        if (_rawMultiplicativePaladinData[i] != 0.0f)
                        {
                            _sparseMultiplicativePaladinCount++;
                            //data.Add(_rawMultiplicativePaladinData[i]);
                            indices.Add(i);
                        }
                    }
                    _sparseInverseMultiplicativePaladinCount = 0;
                    for (int i = 0; i < _rawInverseMultiplicativePaladinData.Length; i++)
                    {
                        if (_rawInverseMultiplicativePaladinData[i] != 0.0f)
                        {
                            _sparseInverseMultiplicativePaladinCount++;
                            //data.Add(_rawInverseMultiplicativePaladinData[i]);
                            indices.Add(i);
                        }
                    }
                    _sparseNoStackPaladinCount = 0;
                    for (int i = 0; i < _rawNoStackPaladinData.Length; i++)
                    {
                        if (_rawNoStackPaladinData[i] != 0.0f)
                        {
                            _sparseNoStackPaladinCount++;
                            //data.Add(_rawNoStackPaladinData[i]);
                            indices.Add(i);
                        }
                    }
                    //_sparsePaladinData = data.ToArray();
                    _sparseIndicesPaladin = indices.ToArray();
                }
            }
        }

        public new StatsPaladin Clone() {
            StatsPaladin clone = (StatsPaladin)this.MemberwiseClone();
            StatsPaladin retVal = new StatsPaladin();
            //
            retVal.Accumulate(base.Clone());
            //
            retVal._rawAdditivePaladinData = (float[])clone._rawAdditivePaladinData.Clone();
            retVal._rawMultiplicativePaladinData = (float[])clone._rawMultiplicativePaladinData.Clone();
            retVal._rawInverseMultiplicativePaladinData = (float[])clone._rawInverseMultiplicativePaladinData.Clone();
            retVal._rawNoStackPaladinData = (float[])clone._rawNoStackPaladinData.Clone();
            //
            return retVal;
        }

        public static PropertyInfo[] PropertyInfoCachePaladin { get { return _propertyInfoCachePaladin; } }

        [XmlIgnore]
        public static String[] StatNamesPaladin {
            get {
                String[] names = new string[PropertyInfoCachePaladin.Length];
                for (int i = 0; i < PropertyInfoCachePaladin.Length; i++) {
                    names[i] = Extensions.DisplayName(PropertyInfoCachePaladin[i]);
                }
                Array.Sort(names);
                return names;
            }
        }
        public IDictionary<PropertyInfo, float> ValuesPaladin(StatFilter filter)
        {
#if SILVERLIGHT
            Dictionary<PropertyInfo, float> dict = new Dictionary<PropertyInfo, float>();
#else
            SortedList<PropertyInfo, float> dict = new SortedList<PropertyInfo, float>(new PropertyComparer());
#endif
            foreach (PropertyInfo info in PropertyInfoCachePaladin)
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
            return _rawAdditivePaladinData.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return this == (obj as StatsPaladin);
        }
        public static bool operator ==(StatsPaladin x, StatsPaladin y)
        {
            if (ReferenceEquals(x, y) || (ReferenceEquals(x, null) && ReferenceEquals(y, null)))
                return true;
            if (ReferenceEquals(x, null) || ReferenceEquals(y, null))
                return false;
            if (x._rawSpecialEffectDataSize > 0 || y._rawSpecialEffectDataSize > 0) return false;
            return ArrayUtils.AllEqual(x.rawAdditiveData, y.rawAdditiveData) &&
                ArrayUtils.AllEqual(x.rawMultiplicativeData, y.rawMultiplicativeData) &&
                ArrayUtils.AllEqual(x.rawNoStackData, y.rawNoStackData) &&
                ArrayUtils.AllEqual(x._rawAdditivePaladinData, y._rawAdditivePaladinData) &&
                ArrayUtils.AllEqual(x._rawMultiplicativePaladinData, y._rawMultiplicativePaladinData) &&
                ArrayUtils.AllEqual(x._rawNoStackPaladinData, y._rawNoStackPaladinData);
        }
        public static bool operator !=(StatsPaladin x, StatsPaladin y)
        {
            return !(x == y);
        }

        public void Accumulate(StatsPaladin data)
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
            #region Paladin
            if (data._sparseIndicesPaladin != null)
            {
                int i = 0;
                for (int a = 0; a < data._sparseAdditivePaladinCount; a++, i++)
                {
                    int index = data._sparseIndicesPaladin[i];
                    _rawAdditivePaladinData[index] += data._rawAdditivePaladinData[index];
                }
                for (int a = 0; a < data._sparseMultiplicativePaladinCount; a++, i++)
                {
                    int index = data._sparseIndicesPaladin[i];
                    _rawMultiplicativePaladinData[index] = (1 + _rawMultiplicativePaladinData[index]) * (1 + data._rawMultiplicativePaladinData[index]) - 1;
                }
                for (int a = 0; a < data._sparseInverseMultiplicativePaladinCount; a++, i++)
                {
                    int index = data._sparseIndicesPaladin[i];
                    _rawInverseMultiplicativePaladinData[index] = 1 - (1 - _rawInverseMultiplicativePaladinData[index]) * (1 - data._rawInverseMultiplicativePaladinData[index]);
                }
                for (int a = 0; a < data._sparseNoStackPaladinCount; a++, i++)
                {
                    int index = data._sparseIndicesPaladin[i];
                    float value = data._rawNoStackPaladinData[index];
                    if (value > _rawNoStackPaladinData[index]) _rawNoStackPaladinData[index] = value;
                }
            }
            else
            {
                float[] add = data._rawAdditivePaladinData;
                for (int i = 0; i < _rawAdditivePaladinData.Length; i++)
                {
                    _rawAdditivePaladinData[i] += add[i];
                }
                add = data._rawMultiplicativePaladinData;
                for (int i = 0; i < _rawMultiplicativePaladinData.Length; i++)
                {
                    _rawMultiplicativePaladinData[i] = (1 + _rawMultiplicativePaladinData[i]) * (1 + add[i]) - 1;
                }
                add = data._rawInverseMultiplicativePaladinData;
                for (int i = 0; i < _rawInverseMultiplicativePaladinData.Length; i++)
                {
                    _rawInverseMultiplicativePaladinData[i] = 1 - (1 - _rawInverseMultiplicativePaladinData[i]) * (1 - add[i]);
                }
                add = data._rawNoStackPaladinData;
                for (int i = 0; i < _rawNoStackPaladinData.Length; i++)
                {
                    if (add[i] > _rawNoStackPaladinData[i]) _rawNoStackPaladinData[i] = add[i];
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

        public void Accumulate(StatsPaladin data, float weight)
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
            #region Paladin
            if (data._sparseIndicesPaladin != null)
            {
                int i = 0;
                for (int a = 0; a < data._sparseAdditivePaladinCount; a++, i++)
                {
                    int index = data._sparseIndicesPaladin[i];
                    _rawAdditivePaladinData[index] += weight * data._rawAdditivePaladinData[index];
                }
                for (int a = 0; a < data._sparseMultiplicativePaladinCount; a++, i++)
                {
                    int index = data._sparseIndicesPaladin[i];
                    _rawMultiplicativePaladinData[index] = (1 + _rawMultiplicativePaladinData[index]) * (1 + weight * data._rawMultiplicativePaladinData[index]) - 1;
                }
                for (int a = 0; a < data._sparseInverseMultiplicativePaladinCount; a++, i++)
                {
                    int index = data._sparseIndicesPaladin[i];
                    _rawInverseMultiplicativePaladinData[index] = 1 - (1 - _rawInverseMultiplicativePaladinData[index]) * (1 - weight * data._rawInverseMultiplicativePaladinData[index]);
                }
                for (int a = 0; a < data._sparseNoStackPaladinCount; a++, i++)
                {
                    int index = data._sparseIndicesPaladin[i];
                    float value = weight * data._rawNoStackPaladinData[index];
                    if (value > _rawNoStackPaladinData[index]) _rawNoStackPaladinData[index] = value;
                }
            }
            else
            {
                float[] add = data._rawAdditivePaladinData;
                for (int i = 0; i < _rawAdditivePaladinData.Length; i++)
                {
                    _rawAdditivePaladinData[i] += weight * add[i];
                }
                add = data._rawMultiplicativePaladinData;
                for (int i = 0; i < _rawMultiplicativePaladinData.Length; i++)
                {
                    _rawMultiplicativePaladinData[i] = (1 + _rawMultiplicativePaladinData[i]) * (1 + weight * add[i]) - 1;
                }
                add = data._rawInverseMultiplicativePaladinData;
                for (int i = 0; i < _rawInverseMultiplicativePaladinData.Length; i++)
                {
                    _rawInverseMultiplicativePaladinData[i] = 1 - (1 - _rawInverseMultiplicativePaladinData[i]) * (1 - weight * add[i]);
                }
                add = data._rawNoStackPaladinData;
                for (int i = 0; i < _rawNoStackPaladinData.Length; i++)
                {
                    if (weight * add[i] > _rawNoStackPaladinData[i]) _rawNoStackPaladinData[i] = weight * add[i];
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
            foreach (PropertyInfo info in PropertyInfoCachePaladin) {
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

        static StatsPaladin()
        {
            List<PropertyInfo> items = new List<PropertyInfo>();
            List<PropertyInfo> itemsPaladin = new List<PropertyInfo>();

            foreach (PropertyInfo info in typeof(StatsPaladin).GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.ExactBinding))
            {
                if (info.PropertyType.IsAssignableFrom(typeof(float)))
                {
                    itemsPaladin.Add(info);
                }
            }
            _propertyInfoCachePaladin = itemsPaladin.ToArray();

            foreach (PropertyInfo info in _propertyInfoCachePaladin)
            {
                if (info.GetCustomAttributes(typeof(PercentageAttribute), false).Length > 0)
                {
                    _percentagePropertiesPaladin.Add(info);
                }
            }

            AdditiveStatPaladinCount = EnumHelper.GetValues(typeof(AdditiveStatPaladin)).Length;
            MultiplicativeStatPaladinCount = EnumHelper.GetValues(typeof(MultiplicativeStatPaladin)).Length;
            InverseMultiplicativeStatPaladinCount = EnumHelper.GetValues(typeof(InverseMultiplicativeStatPaladin)).Length;
            NonStackingStatPaladinCount = EnumHelper.GetValues(typeof(NonStackingStatPaladin)).Length;
        }

        #region Attributes as Callable and Serializable points
        #region Additive Stats
        /*[DefaultValueAttribute(0f)]
        [Percentage]
        [DisplayName("% Bonus Guardian of Kings Duration")]
        [Category("Paladin")]
        public float BonusDurationGuardianOfAncientKings
        {
            get { return _rawAdditivePaladinData[(int)AdditiveStatPaladin.BonusDurationGuardianOfAncientKings]; }
            set { _rawAdditivePaladinData[(int)AdditiveStatPaladin.BonusDurationGuardianOfAncientKings] = value; }
        }*/
        #endregion
        #region Multiplicative Stats
        #region Prot
        [DefaultValueAttribute(0f)]
        [Percentage]
        [DisplayName("% Bonus Crusader Strike Damage")]
        [Category("Paladin")]
        public float BonusDamageMultiplierCrusaderStrike
        {
            get { return _rawMultiplicativePaladinData[(int)MultiplicativeStatPaladin.BonusDamageMultiplierCrusaderStrike]; }
            set { _rawMultiplicativePaladinData[(int)MultiplicativeStatPaladin.BonusDamageMultiplierCrusaderStrike] = value; }
        }
        [DefaultValueAttribute(0f)]
        [Percentage]
        [DisplayName("% Bonus Guardian of Kings Duration")]
        [Category("Paladin")]
        public float BonusDurationMultiplierGuardianOfAncientKings
        {
            get { return _rawMultiplicativePaladinData[(int)MultiplicativeStatPaladin.BonusDurationMultiplierGuardianOfAncientKings]; }
            set { _rawMultiplicativePaladinData[(int)MultiplicativeStatPaladin.BonusDurationMultiplierGuardianOfAncientKings] = value; }
        }
        [DefaultValueAttribute(0f)]
        [Percentage]
        [DisplayName("% Bonus Shield of the Righteous Damage")]
        [Category("Paladin")]
        public float BonusDamageShieldofRighteous
        {
            get { return _rawMultiplicativePaladinData[(int)MultiplicativeStatPaladin.BonusDamageShieldofRighteous]; }
            set { _rawMultiplicativePaladinData[(int)MultiplicativeStatPaladin.BonusDamageShieldofRighteous] = value; }
        }
        #endregion
        #region Ret
        #endregion
        #region Holy
        #endregion
        #region Both
        #endregion
        #endregion
        #endregion
    }
}
