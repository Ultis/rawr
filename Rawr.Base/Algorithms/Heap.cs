using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rawr.Base.Algorithms
{
    // based on Rawr.Mage.Heap
    public class DoubleMaxHeap<T>
    {
        private int cap;
        private double[] keys;
        private T[] values;
        private int mCount;

        public DoubleMaxHeap(int capacity = 16)
        {
            this.cap = capacity;
            this.keys = new double[this.cap + 1];
            this.values = new T[this.cap + 1];
        }

        public void Clear()
        {
            this.mCount = 0;
        }

        public void Pop()
        {
            if (mCount == 0)
                return;
            double insertKey = keys[mCount];
            T insertValue = values[mCount];
            --mCount;
            int i = 1;
            while ((i << 1) <= mCount)
            {
                int maxChild = i << 1;
                if (((maxChild + 1) <= mCount) && keys[maxChild + 1] > keys[maxChild])
                    ++maxChild;
                if (insertKey >= keys[maxChild])
                    break;
                keys[i] = keys[maxChild];
                values[i] = values[maxChild];
                i = maxChild;
            }
            keys[i] = insertKey;
            values[i] = insertValue;
        }

        public void Push(double key, T value)
        {
            if (this.mCount == cap)
            {
                cap *= 2;
                double[] newKeys = new double[cap + 1];
                T[] newValues = new T[cap + 1];
                Array.Copy(values, newValues, values.Length);
                Array.Copy(keys, newKeys, keys.Length);
                keys = newKeys;
                values = newValues;
            }
            this.mCount++;
            int i = this.mCount;
            while ((i > 1) && keys[i >> 1] < key)
            {
                keys[i] = keys[i >> 1];
                values[i] = values[i >> 1];
                i >>= 1;
            }
            keys[i] = key;
            values[i] = value;
        }

        public int Count
        {
            get
            {
                return this.mCount;
            }
        }

        public double TopKey
        {
            get
            {
                return keys[1];
            }
        }

        public T TopValue
        {
            get
            {
                return values[1];
            }
        }
    }
}
