using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.ComponentModel;
using System.Reflection.Emit;
using System.Xml.Serialization;
using System.Linq;

namespace Rawr {

    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class DisplayNameAttribute : Attribute
    {
        public DisplayNameAttribute(string name)
        {
            _name = name;
        }

        private readonly string _name;

        public string DisplayName { get { return _name; } }
    }

    public class CommonStat : System.Attribute {
        public static float GetCommonStatMinimumRange(PropertyInfo property) {
            foreach (System.Attribute attribute in property.GetCustomAttributes(false))
                if (attribute is CommonStat)
                    return (attribute as CommonStat).MinRange;
            return -1f;
        }

        public float MinRange = 0f;
    }

    /// <summary>
    /// A Stats object represents a collection of stats on an object, such as an Item, Enchant, or Buff.
    /// </summary>
    public partial class Stats {
        internal float[] _rawAdditiveData = new float[AdditiveStatCount];
        internal float[] _rawMultiplicativeData = new float[MultiplicativeStatCount];
        internal float[] _rawInverseMultiplicativeData = new float[InverseMultiplicativeStatCount];
        internal float[] _rawNoStackData = new float[NonStackingStatCount];

        [System.ComponentModel.DefaultValueAttribute(null)]
        [XmlArray("SpecialEffects")]
        [XmlArrayItem(IsNullable = false)]
        public SpecialEffect[] _rawSpecialEffectData = null;

        //private static SpecialEffect[] _emptySpecialEffectData = new SpecialEffect[0];
        private const int _defaultSpecialEffectDataCapacity = 4;
        [System.ComponentModel.DefaultValueAttribute(0f)]
        [XmlElement("SpecialEffectCount")]
        public int _rawSpecialEffectDataSize;

        //internal float[] _sparseData;
        internal int[] _sparseIndices;
        internal int _sparseAdditiveCount;
        internal int _sparseMultiplicativeCount;
        internal int _sparseInverseMultiplicativeCount;
        internal int _sparseNoStackCount;

        public void InvalidateSparseData() {
            //_sparseData = null;
            _sparseIndices = null;
        }

        public void GenerateSparseData() {
            //List<float> data = new List<float>();
            if (_sparseIndices != null) return;
            lock (_rawAdditiveData)
            {
                if (_sparseIndices == null)
                {
                    List<int> indices = new List<int>();
                    _sparseAdditiveCount = 0;
                    for (int i = 0; i < _rawAdditiveData.Length; i++)
                    {
                        if (_rawAdditiveData[i] != 0.0f)
                        {
                            _sparseAdditiveCount++;
                            //data.Add(_rawAdditiveData[i]);
                            indices.Add(i);
                        }
                    }
                    _sparseMultiplicativeCount = 0;
                    for (int i = 0; i < _rawMultiplicativeData.Length; i++)
                    {
                        if (_rawMultiplicativeData[i] != 0.0f)
                        {
                            _sparseMultiplicativeCount++;
                            //data.Add(_rawMultiplicativeData[i]);
                            indices.Add(i);
                        }
                    }
                    _sparseInverseMultiplicativeCount = 0;
                    for (int i = 0; i < _rawInverseMultiplicativeData.Length; i++)
                    {
                        if (_rawInverseMultiplicativeData[i] != 0.0f)
                        {
                            _sparseInverseMultiplicativeCount++;
                            //data.Add(_rawInverseMultiplicativeData[i]);
                            indices.Add(i);
                        }
                    }
                    _sparseNoStackCount = 0;
                    for (int i = 0; i < _rawNoStackData.Length; i++)
                    {
                        if (_rawNoStackData[i] != 0.0f)
                        {
                            _sparseNoStackCount++;
                            //data.Add(_rawNoStackData[i]);
                            indices.Add(i);
                        }
                    }
                    //_sparseData = data.ToArray();
                    _sparseIndices = indices.ToArray();
                }
            }
        }



        private void EnsureSpecialEffectCapacity(int min)
        {
            if (_rawSpecialEffectData == null || _rawSpecialEffectData.Length < min)
            {
                int num = (_rawSpecialEffectData == null || _rawSpecialEffectData.Length == 0) ? _defaultSpecialEffectDataCapacity : (_rawSpecialEffectData.Length * 2);
                if (num < min)
                {
                    num = min;
                }
                SpecialEffect[] destinationArray = new SpecialEffect[num];
                if (_rawSpecialEffectDataSize > 0)
                {
                    Array.Copy(_rawSpecialEffectData, 0, destinationArray, 0, _rawSpecialEffectDataSize);
                }
                _rawSpecialEffectData = destinationArray;
            }
        }

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
            Stats c = new Stats();

            int i = c._rawAdditiveData.Length;
            while (--i >= 0)
            {
                c._rawAdditiveData[i] = a._rawAdditiveData[i] + b._rawAdditiveData[i];
            }
            i = c._rawMultiplicativeData.Length;
            while (--i >= 0)
            {
                c._rawMultiplicativeData[i] = (1 + a._rawMultiplicativeData[i]) * (1 + b._rawMultiplicativeData[i]) - 1;
            }
            i = c._rawInverseMultiplicativeData.Length;
            while (--i >= 0)
            {
                c._rawInverseMultiplicativeData[i] = 1 - (1 - a._rawInverseMultiplicativeData[i]) * (1 - b._rawInverseMultiplicativeData[i]);
            }

            i = c._rawNoStackData.Length;
            while (--i >= 0)
            {
                c._rawNoStackData[i] = Math.Max(a._rawNoStackData[i], b._rawNoStackData[i]);
            }
            int specialEffectCount = a._rawSpecialEffectDataSize + b._rawSpecialEffectDataSize;
            if (specialEffectCount > 0)
            {
                c._rawSpecialEffectData = new SpecialEffect[specialEffectCount];
                c._rawSpecialEffectDataSize = specialEffectCount;
                if (a._rawSpecialEffectDataSize > 0)
                {
                    Array.Copy(a._rawSpecialEffectData, c._rawSpecialEffectData, a._rawSpecialEffectDataSize);
                }
                if (b._rawSpecialEffectDataSize > 0)
                {
                    Array.Copy(b._rawSpecialEffectData, 0, c._rawSpecialEffectData, a._rawSpecialEffectDataSize, b._rawSpecialEffectDataSize);
                }
            }
            return c;
        }

        /// <summary>
        /// Adds together two stats, when using a + operator. When adding additional stats for
        /// Rawr to track, after adding the stat property, also add a line for it to this method,
        /// to properly combine the stat, as appropriate.
        /// </summary>
        /// <param name="a">The first Stats object to combine.</param>
        /// <param name="b">The second Stats object to combine.</param>
        /// <returns>The combined Stats object.</returns>
        public static Stats operator -(Stats a, Stats b)
        {
            Stats c = new Stats();

            int i = c._rawAdditiveData.Length;
            while (--i >= 0)
            {
                c._rawAdditiveData[i] = a._rawAdditiveData[i] - b._rawAdditiveData[i];
            }
            i = c._rawMultiplicativeData.Length;
            while (--i >= 0)
            {
                c._rawMultiplicativeData[i] = (1 + a._rawMultiplicativeData[i]) / (1 + b._rawMultiplicativeData[i]) - 1;
            }
            i = c._rawInverseMultiplicativeData.Length;
            while (--i >= 0)
            {
                c._rawInverseMultiplicativeData[i] = 1 - (1 - a._rawInverseMultiplicativeData[i]) / (1 - b._rawInverseMultiplicativeData[i]);
            }

            //i = c._rawNoStackData.Length;
            //while (--i >= 0)
            //{
            //    c._rawNoStackData[i] = Math.Max(a._rawNoStackData[i], b._rawNoStackData[i]);
            //}
            //int specialEffectCount = a._rawSpecialEffectDataSize + b._rawSpecialEffectDataSize;
            //if (specialEffectCount > 0)
            //{
            //    c._rawSpecialEffectData = new SpecialEffect[specialEffectCount];
            //    c._rawSpecialEffectDataSize = specialEffectCount;
            //    if (a._rawSpecialEffectDataSize > 0)
            //    {
            //        Array.Copy(a._rawSpecialEffectData, c._rawSpecialEffectData, a._rawSpecialEffectDataSize);
            //    }
            //    if (b._rawSpecialEffectDataSize > 0)
            //    {
            //        Array.Copy(b._rawSpecialEffectData, 0, c._rawSpecialEffectData, a._rawSpecialEffectDataSize, b._rawSpecialEffectDataSize);
            //    }
            //}
            return c;
        }

        public void AddSpecialEffect(SpecialEffect specialEffect)
        {
            EnsureSpecialEffectCapacity(_rawSpecialEffectDataSize + 1);
            _rawSpecialEffectData[_rawSpecialEffectDataSize] = specialEffect;
            _rawSpecialEffectDataSize++;
        }


        public void RemoveSpecialEffect(SpecialEffect specialEffect)
        {
            bool found = false;
            for (int i = 0; i < _rawSpecialEffectDataSize; i++)
            {
                if (found)
                {
                    _rawSpecialEffectData[i - 1] = _rawSpecialEffectData[i];
                }
                else if (specialEffect == _rawSpecialEffectData[i])
                {
                    found = true;
                }
            }
            if (found)
            {
                _rawSpecialEffectDataSize--;
                _rawSpecialEffectData[_rawSpecialEffectDataSize] = null;
            }
        }


        public struct SpecialEffectEnumerator : IEnumerator<SpecialEffect>, IDisposable, System.Collections.IEnumerator, IEnumerable<SpecialEffect>
        {
            private Stats stats;
            private int index;
            private SpecialEffect current;
            private Predicate<SpecialEffect> match;

            internal SpecialEffectEnumerator(Stats stats, Predicate<SpecialEffect> match)
            {
                this.stats = stats;
                this.match = match;
                index = 0;
                current = null;
            }

            public void Dispose()
            {
            }

            public bool MoveNext()
            {
                while (index < stats._rawSpecialEffectDataSize)
                {
                    current = stats._rawSpecialEffectData[index];
                    index++;
                    if (match == null || match(current)) return true;
                }
                index = stats._rawSpecialEffectDataSize + 1;
                current = null;
                return false;
            }

            public SpecialEffect Current
            {
                get
                {
                    return current;
                }
            }

            object System.Collections.IEnumerator.Current
            {
                get
                {
                    if ((index == 0) || (index == (stats._rawSpecialEffectDataSize + 1)))
                    {
                        throw new InvalidOperationException();
                    }
                    return Current;
                }
            }

            void System.Collections.IEnumerator.Reset()
            {
                index = 0;
                current = null;
            }

            public SpecialEffectEnumerator GetEnumerator()
            {
                return this;
            }

            IEnumerator<SpecialEffect> System.Collections.Generic.IEnumerable<SpecialEffect>.GetEnumerator()
            {
                return this;
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return this;
            }
        }

        public SpecialEffectEnumerator SpecialEffects()
        {
            return new SpecialEffectEnumerator(this, null);
        }

        public SpecialEffectEnumerator SpecialEffects(Predicate<SpecialEffect> match)
        {
            return new SpecialEffectEnumerator(this, match);
        }

        public bool ContainsSpecialEffect(Predicate<SpecialEffect> match)
        {
            for (int i = 0; i < _rawSpecialEffectDataSize; i++)
            {
                SpecialEffect effect = _rawSpecialEffectData[i];
                if (match(effect)) return true;
            }
            return false;
        }

        public bool ContainsSpecialEffect()
        {
            return _rawSpecialEffectDataSize > 0;
        }

        /// <summary>
        /// Multiplies every stat in a stats by a float, when using a * operator.
        /// </summary>
        /// <param name="a">The Stats object to multiply.</param>
        /// <param name="b">The float by which to multiply every stat.</param>
        /// <returns>The new Stats object.</returns>
        public static Stats operator *(Stats a, float b)
        {
            Stats c = new Stats();

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

        public void Accumulate(Stats data)
        {
            for (int a = 0; a < AdditiveStatCount; a++)
            {
                _rawAdditiveData[a] += data._rawAdditiveData[a];
            }
            for (int a = 0; a < MultiplicativeStatCount; a++)
            {
                _rawMultiplicativeData[a] = (1 + _rawMultiplicativeData[a]) * (1 + data._rawMultiplicativeData[a]) - 1;
            }
            for (int a = 0; a < InverseMultiplicativeStatCount; a++)
            {
                _rawInverseMultiplicativeData[a] = 1 - (1 - _rawInverseMultiplicativeData[a]) * (1 - data._rawInverseMultiplicativeData[a]);
            }
            for (int a = 0; a < NonStackingStatCount; a++)
            {
                float value = data._rawNoStackData[a];
                if (value > _rawNoStackData[a]) _rawNoStackData[a] = value;
            }
            if (data._rawSpecialEffectDataSize > 0)
            {
                EnsureSpecialEffectCapacity(_rawSpecialEffectDataSize + data._rawSpecialEffectDataSize);
                Array.Copy(data._rawSpecialEffectData, 0, _rawSpecialEffectData, _rawSpecialEffectDataSize, data._rawSpecialEffectDataSize);
                _rawSpecialEffectDataSize += data._rawSpecialEffectDataSize;
            }
        }

        public void Accumulate(Stats data, float weight)
        {
            for (int a = 0; a < AdditiveStatCount; a++)
            {
                _rawAdditiveData[a] += weight * data._rawAdditiveData[a];
            }
            for (int a = 0; a < MultiplicativeStatCount; a++)
            {
                _rawMultiplicativeData[a] = (1 + _rawMultiplicativeData[a]) * (1 + weight * data._rawMultiplicativeData[a]) - 1;
            }
            for (int a = 0; a < InverseMultiplicativeStatCount; a++)
            {
                _rawInverseMultiplicativeData[a] = 1 - (1 - _rawInverseMultiplicativeData[a]) * (1 - weight * data._rawInverseMultiplicativeData[a]);
            }
            for (int a = 0; a < NonStackingStatCount; a++)
            {
                float value = weight * data._rawNoStackData[a];
                if (value > _rawNoStackData[a]) _rawNoStackData[a] = value;
            }
            if (data._rawSpecialEffectDataSize > 0)
            {
                EnsureSpecialEffectCapacity(_rawSpecialEffectDataSize + data._rawSpecialEffectDataSize);
                Array.Copy(data._rawSpecialEffectData, 0, _rawSpecialEffectData, _rawSpecialEffectDataSize, data._rawSpecialEffectDataSize);
                _rawSpecialEffectDataSize += data._rawSpecialEffectDataSize;
            }
        }

        public bool Equals(Stats other)
        {
            return this == other;
        }
        public ArrayUtils.CompareResult CompareTo(Stats other)
        {
            if (ReferenceEquals(other, null))
                return 0;
            if (this._rawSpecialEffectDataSize > 0 || other._rawSpecialEffectDataSize > 0)
            {
                return ArrayUtils.CompareResult.Unequal; // not sure if we need to actually go through and compare each
            }
            if (this._sparseIndices != null && other._sparseIndices != null)
            {
                bool haveGreaterThan = false, haveLessThan = false;
                int j = 0;
                int i = 0;
                int b = 0;
                for (int a = 0; a < other._sparseAdditiveCount; a++, i++)
                {
                    int index = other._sparseIndices[i];
                    while (b < _sparseAdditiveCount && _sparseIndices[j] < index)
                    {
                        b++;
                        j++;
                        haveGreaterThan = true;
                        if (haveLessThan)
                        {
                            return ArrayUtils.CompareResult.Unequal;
                        }
                    }
                    if (b < _sparseAdditiveCount && _sparseIndices[j] == index)
                    {
                        int val = _rawAdditiveData[index].CompareTo(other._rawAdditiveData[index]);
                        if (val < 0)
                        {
                            haveLessThan = true;
                            if (haveGreaterThan)
                            {
                                return ArrayUtils.CompareResult.Unequal;
                            }
                        }
                        else if (val > 0)
                        {
                            haveGreaterThan = true;
                            if (haveLessThan)
                            {
                                return ArrayUtils.CompareResult.Unequal;
                            }
                        }
                        b++;
                        j++;
                    }
                    else
                    {
                        haveLessThan = true;
                        if (haveGreaterThan)
                        {
                            return ArrayUtils.CompareResult.Unequal;
                        }
                    }
                }
                while (b < _sparseAdditiveCount)
                {
                    b++;
                    j++;
                    haveGreaterThan = true;
                    if (haveLessThan)
                    {
                        return ArrayUtils.CompareResult.Unequal;
                    }
                }
                b = 0;
                for (int a = 0; a < other._sparseMultiplicativeCount; a++, i++)
                {
                    int index = other._sparseIndices[i];
                    while (b < _sparseMultiplicativeCount && _sparseIndices[j] < index)
                    {
                        b++;
                        j++;
                        haveGreaterThan = true;
                        if (haveLessThan)
                        {
                            return ArrayUtils.CompareResult.Unequal;
                        }
                    }
                    if (b < _sparseMultiplicativeCount && _sparseIndices[j] == index)
                    {
                        int val = _rawMultiplicativeData[index].CompareTo(other._rawMultiplicativeData[index]);
                        if (val < 0)
                        {
                            haveLessThan = true;
                            if (haveGreaterThan)
                            {
                                return ArrayUtils.CompareResult.Unequal;
                            }
                        }
                        else if (val > 0)
                        {
                            haveGreaterThan = true;
                            if (haveLessThan)
                            {
                                return ArrayUtils.CompareResult.Unequal;
                            }
                        }
                        b++;
                        j++;
                    }
                    else
                    {
                        haveLessThan = true;
                        if (haveGreaterThan)
                        {
                            return ArrayUtils.CompareResult.Unequal;
                        }
                    }
                }
                while (b < _sparseMultiplicativeCount)
                {
                    b++;
                    j++;
                    haveGreaterThan = true;
                    if (haveLessThan)
                    {
                        return ArrayUtils.CompareResult.Unequal;
                    }
                }
                b = 0;
                for (int a = 0; a < other._sparseInverseMultiplicativeCount; a++, i++)
                {
                    int index = other._sparseIndices[i];
                    while (b < _sparseInverseMultiplicativeCount && _sparseIndices[j] < index)
                    {
                        b++;
                        j++;
                        haveGreaterThan = true;
                        if (haveLessThan)
                        {
                            return ArrayUtils.CompareResult.Unequal;
                        }
                    }
                    if (b < _sparseInverseMultiplicativeCount && _sparseIndices[j] == index)
                    {
                        int val = _rawInverseMultiplicativeData[index].CompareTo(other._rawInverseMultiplicativeData[index]);
                        if (val < 0)
                        {
                            haveLessThan = true;
                            if (haveGreaterThan)
                            {
                                return ArrayUtils.CompareResult.Unequal;
                            }
                        }
                        else if (val > 0)
                        {
                            haveGreaterThan = true;
                            if (haveLessThan)
                            {
                                return ArrayUtils.CompareResult.Unequal;
                            }
                        }
                        b++;
                        j++;
                    }
                    else
                    {
                        haveLessThan = true;
                        if (haveGreaterThan)
                        {
                            return ArrayUtils.CompareResult.Unequal;
                        }
                    }
                }
                while (b < _sparseInverseMultiplicativeCount)
                {
                    b++;
                    j++;
                    haveGreaterThan = true;
                    if (haveLessThan)
                    {
                        return ArrayUtils.CompareResult.Unequal;
                    }
                }
                b = 0;
                for (int a = 0; a < other._sparseNoStackCount; a++, i++)
                {
                    int index = other._sparseIndices[i];
                    while (b < _sparseNoStackCount && _sparseIndices[j] < index)
                    {
                        b++;
                        j++;
                        haveGreaterThan = true;
                        if (haveLessThan)
                        {
                            return ArrayUtils.CompareResult.Unequal;
                        }
                    }
                    if (b < _sparseNoStackCount && _sparseIndices[j] == index)
                    {
                        int val = _rawNoStackData[index].CompareTo(other._rawNoStackData[index]);
                        if (val < 0)
                        {
                            haveLessThan = true;
                            if (haveGreaterThan)
                            {
                                return ArrayUtils.CompareResult.Unequal;
                            }
                        }
                        else if (val > 0)
                        {
                            haveGreaterThan = true;
                            if (haveLessThan)
                            {
                                return ArrayUtils.CompareResult.Unequal;
                            }
                        }
                        b++;
                        j++;
                    }
                    else
                    {
                        haveLessThan = true;
                        if (haveGreaterThan)
                        {
                            return ArrayUtils.CompareResult.Unequal;
                        }
                    }
                }
                while (b < _sparseNoStackCount)
                {
                    b++;
                    j++;
                    haveGreaterThan = true;
                    if (haveLessThan)
                    {
                        return ArrayUtils.CompareResult.Unequal;
                    }
                }
                if (haveGreaterThan && haveLessThan) return ArrayUtils.CompareResult.Unequal;
                else if (haveGreaterThan) return ArrayUtils.CompareResult.GreaterThan;
                else if (haveLessThan) return ArrayUtils.CompareResult.LessThan;
                else return ArrayUtils.CompareResult.Equal;
            }
            else
            {
                return ArrayUtils.And(ArrayUtils.AllCompare(this._rawAdditiveData, other._rawAdditiveData), ArrayUtils.And(
                    ArrayUtils.AllCompare(this._rawMultiplicativeData, other._rawMultiplicativeData),
                    ArrayUtils.AllCompare(this._rawNoStackData, other._rawNoStackData)));
            }
        }
        //int IComparable.CompareTo(object other)
        //{
        //    return CompareTo(other as Stats);
        //}


        public override int GetHashCode()
        {
            return _rawAdditiveData.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return this == (obj as Stats);
        }
        public static bool operator ==(Stats x, Stats y)
        {
            if (ReferenceEquals(x, y) || (ReferenceEquals(x, null) && ReferenceEquals(y, null)))
                return true;
            if (ReferenceEquals(x, null) || ReferenceEquals(y, null))
                return false;
            if (x._rawSpecialEffectDataSize > 0 || y._rawSpecialEffectDataSize > 0) return false;
            return ArrayUtils.AllEqual(x._rawAdditiveData, y._rawAdditiveData) &&
                ArrayUtils.AllEqual(x._rawMultiplicativeData, y._rawMultiplicativeData) &&
                ArrayUtils.AllEqual(x._rawNoStackData, y._rawNoStackData);
        }
        public static bool operator !=(Stats x, Stats y)
        {
            return !(x == y);
        }
        public static bool operator >(Stats x, Stats y)
        {
			return x >= y && x != y;
        }
        public static bool operator >=(Stats x, Stats y)
        {
            return AllCompare(x, y, ArrayUtils.CompareOption.GreaterThan | ArrayUtils.CompareOption.Equal);
        }
        public static bool operator <(Stats x, Stats y)
        {
			return x <= y && x != y;
        }
        public static bool operator <=(Stats x, Stats y)
        {
            return AllCompare(x, y, ArrayUtils.CompareOption.LessThan | ArrayUtils.CompareOption.Equal);
        }
        private static bool AllCompare(Stats x, Stats y, ArrayUtils.CompareOption comparison)
        {
            if (ReferenceEquals(x, null) || ReferenceEquals(y, null))
                throw new ArgumentNullException();
            if (x._rawSpecialEffectDataSize > 0 || y._rawSpecialEffectDataSize > 0) return false;
            return ArrayUtils.AllCompare(x._rawAdditiveData, y._rawAdditiveData, comparison)
                && ArrayUtils.AllCompare(x._rawMultiplicativeData, y._rawMultiplicativeData, comparison)
                && ArrayUtils.AllCompare(x._rawInverseMultiplicativeData, y._rawInverseMultiplicativeData, comparison)
                && ArrayUtils.AllCompare(x._rawNoStackData, y._rawNoStackData, comparison);
        }


        //as the ocean opens up to swallow you
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (PropertyInfo info in PropertyInfoCache)
            {
                float value = (float)info.GetValue(this, null);
                if (value != 0)
                {
                    if (IsPercentage(info))
                    {
                        value *= 100;
                    }

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

        public Stats Clone()
        {
            Stats clone = (Stats)this.MemberwiseClone();
            clone._rawAdditiveData = (float[])clone._rawAdditiveData.Clone();
            clone._rawMultiplicativeData = (float[])clone._rawMultiplicativeData.Clone();
            clone._rawInverseMultiplicativeData = (float[])clone._rawInverseMultiplicativeData.Clone();
            clone._rawNoStackData = (float[])clone._rawNoStackData.Clone();
            if (_rawSpecialEffectData != null) clone._rawSpecialEffectData = (SpecialEffect[])_rawSpecialEffectData.Clone();
            return clone;
        }

		//public void ConvertStatsToWotLKEquivalents()
		//{
		//    HitRating = Math.Max(HitRating, SpellHitRating);
		//    CritRating = Math.Max(CritRating, SpellCritRating);
		//    HasteRating = Math.Max(HasteRating, SpellHasteRating);
		//    SpellPower = Math.Max(SpellPower, Math.Max(SpellDamageRating, (float)Math.Floor(Healing / 1.88f)));
		//    ArmorPenetrationRating = Math.Max(ArmorPenetrationRating, (float)Math.Floor(ArmorPenetration / 7f));
		//    SpellHitRating = SpellCritRating = SpellHasteRating = SpellDamageRating = Healing = ArmorPenetration = 0;
		//}

        #region Multiplicative Handling
        private static PropertyInfo[] _propertyInfoCache = null;
        private static List<PropertyInfo> _percentageProperties = new List<PropertyInfo>();
        private static int AdditiveStatCount = 0;
        private static int MultiplicativeStatCount = 0;
        private static int InverseMultiplicativeStatCount = 0;
        private static int NonStackingStatCount = 0;

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
                if (info.GetCustomAttributes(typeof(PercentageAttribute), false).Length > 0)
                {
                    _percentageProperties.Add(info);
                }
            }

            AdditiveStatCount = EnumHelper.GetValues(typeof(AdditiveStat)).Length;
            MultiplicativeStatCount = EnumHelper.GetValues(typeof(MultiplicativeStat)).Length;
            InverseMultiplicativeStatCount = EnumHelper.GetValues(typeof(InverseMultiplicativeStat)).Length;
            NonStackingStatCount = EnumHelper.GetValues(typeof(NonStackingStat)).Length;
        }

        public static PropertyInfo[] PropertyInfoCache
        {
            get
            {
                return _propertyInfoCache;
            }
        }

        public static bool IsPercentage(PropertyInfo info)
        {
            return _percentageProperties.Contains(info);
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
            get
            {
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
            Dictionary<PropertyInfo, float> dict = new Dictionary<PropertyInfo, float>();
            foreach (PropertyInfo info in PropertyInfoCache)
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
            dict.OrderBy(kvp => kvp.Key, new PropertyComparer());
            return dict;
        }
    }

    public delegate bool StatFilter(float value);

    [AttributeUsage(AttributeTargets.Property)]
    public class PercentageAttribute : Attribute
    {
    }

    public static class Extensions {
        // requires .net 3.5 public static string LongName(this PropertyInfo info)
        // allows it to be called like
        //   info.LongName()
        // instead of
        //   Extensions.LongName(info)

        public static PropertyInfo UnDisplayName(string displayName) {
            foreach (PropertyInfo info in Stats.PropertyInfoCache) {
                if (DisplayName(info).Trim() == displayName.Trim())
                    return info;
            }
            return null;
        }

        public static string DisplayName(PropertyInfo info) {
            string prettyName = null;

            object[] attributes = info.GetCustomAttributes(typeof(DisplayNameAttribute), false);
            if (attributes.Length == 1 && attributes[0] is DisplayNameAttribute && (attributes[0] as DisplayNameAttribute).DisplayName != null) {
                prettyName = (attributes[0] as DisplayNameAttribute).DisplayName;
            } else {
                prettyName = SpaceCamel(info.Name);
            }
            if (!prettyName.StartsWith("%"))
                prettyName = " " + prettyName;
            return prettyName;
        }
        public static string SpaceCamel(String name) {
            return System.Text.RegularExpressions.Regex.Replace(
                    name,
                    "([A-Z])",
                    " $1").Trim();
        }
        public static string UnSpaceCamel(String name) {
            return System.Text.RegularExpressions.Regex.Replace(
                    name,
                    "( )",
                    "").Trim();
        }
    }
}
