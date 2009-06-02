using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Rawr.Mage
{
#if SILVERLIGHT
    public struct ArrayOffset<T>
    {
        private T[] array;
        private int offset;

        public ArrayOffset(T[] array, int offset)
        {
            this.array = array;
            this.offset = offset;
        }

        public T this[int index]
        {
            get
            {
                return array[offset + index];
            }
            set
            {
                array[offset + index] = value;
            }
        }
    }

#endif
#if SILVERLIGHT
    public class ArraySet
#else
    public unsafe class ArraySet
#endif
    {
        public double[] SparseMatrixValue;
        public int[] SparseMatrixRow;
        public int[] SparseMatrixCol;
        public double[] SparseMatrixData; // still store the dense version, memory is cheap and it speeds some stuff up
        public int SparseMatrixMaxRows = 0;
        public int SparseMatrixMaxCols = 0;
        public int SparseMatrixSparseSize;

        public int LUetaMax;
        public int LUsizeMax;
        public int[] LU_P;
        public int[] LU_Q;
        public double[] LUsparseL; // packed non-zero elements of eta vectors
        public int[] LUsparseLI; // indices of non-zero elements
        public int[] LUsparseLstart; // start index of non-zero elements of eta vector i
        public int[] LU_LJ; // col/row eta index   Li = I + L[i*size:(i+1)*size-1] * e_LI[i]'
        public double[] LUcolumn;
        public double[] LUcolumn2;
        public double[] LU_U;

        public Dictionary<string, int> extraConstraintMap = new Dictionary<string, int>();
        public int[] extraReferenceCount;
        public double[] extraConstraints;
        public double[] _d;
        public double[] _x;
        public double[] _w;
        public double[] _ww;
        public double[] _wd;
        public double[] _u;
        public double[] _c;
#if SILVERLIGHT
        public ArrayOffset<double>[] _pD;
#else
        public double*[] _pD;
#endif
        //internal static double[] _b;
        public double[] _cost;
        public double[] _costWorking;
        //private static double[] _beta;
        //private static double[] _betaBackup;

        public int[] _flags;
        public double[] _lb;
        public double[] _ub;

        public int maxRows = 0;
        public int maxCols = 0;
        public int maxExtra = 0;
        public int maxExtraRows = 0;

        public int maxSolverRows = 0;
        public int maxSolverCols = 0;
        public double[] rowScale;
        public double[] columnScale;

        public int MaxSize
        {
            get
            {
                return SparseMatrixMaxRows * SparseMatrixMaxCols;
            }
        }

        public ArraySet()
        {
            SparseMatrixMaxRows = 200;
            SparseMatrixMaxCols = 200;
            RecreateSparseMatrixArrays();

            LUsizeMax = 200;
            RecreateLUArrays();

            maxRows = 200;
            maxCols = 200;
            RecreateLPArrays();
            maxExtraRows = 32;
            extraConstraints = new double[maxExtraRows * maxCols];
            extraReferenceCount = new int[maxExtraRows];

            maxSolverRows = 200;
            maxSolverCols = 200;
            RecreateSolverArrays();        
        }

        public void RecreateSparseMatrixArrays()
        {
            SparseMatrixSparseSize = Math.Max(SparseMatrixSparseSize, (int)(SparseMatrixMaxRows * SparseMatrixMaxCols * 0.4));
            SparseMatrixValue = new double[SparseMatrixSparseSize];
            SparseMatrixRow = new int[SparseMatrixSparseSize];
            SparseMatrixCol = new int[SparseMatrixMaxCols + 1];
            SparseMatrixData = new double[SparseMatrixMaxRows * SparseMatrixMaxCols];
        }

        public void RecreateLUArrays()
        {
            LU_U = new double[LUsizeMax * LUsizeMax];
            LUetaMax = Math.Max(LUsizeMax + 100, 2 * LUsizeMax);
            LU_P = new int[LUsizeMax];
            LU_Q = new int[LUsizeMax];
            //_L = new double[etaMax * size];
            LU_LJ = new int[LUetaMax];
            LUsparseL = new double[LUetaMax * LUsizeMax];
            LUsparseLI = new int[LUetaMax * LUsizeMax];
            LUsparseLstart = new int[LUetaMax];
            LUcolumn = new double[LUsizeMax];
            LUcolumn2 = new double[LUsizeMax];
        }

        public void RecreateLPArrays()
        {
            _d = new double[maxRows];
            _x = new double[maxRows];
            _w = new double[maxRows];
            _ww = new double[maxRows];
            _wd = new double[maxCols];
            _u = new double[maxRows];
            _c = new double[maxCols];
            _cost = new double[maxCols + maxRows];
            _costWorking = new double[maxCols + maxRows];
            //_b = new double[maxRows];
            //_beta = new double[maxRows];
            //_betaBackup = new double[maxRows];
        }

        public void ExtendLPArrays()
        {
            _d = new double[maxRows];
            _x = new double[maxRows];
            _w = new double[maxRows];
            _ww = new double[maxRows];
            _u = new double[maxRows];
            _c = new double[maxCols];
            double[] tmp = new double[maxCols + maxRows];
            _cost.CopyTo(tmp, 0);
            _cost = tmp;
            tmp = new double[maxCols + maxRows];
            _costWorking.CopyTo(tmp, 0);
            _costWorking = tmp;
        }

        public void RecreateSolverArrays()
        {
            rowScale = new double[maxSolverRows];
            columnScale = new double[maxSolverCols];
        }

        public int GetConstraint(string name, int cols, out bool newConstraint)
        {
            int index;
            if (name == null || !extraConstraintMap.TryGetValue(name, out index))
            {
                // first check if we have any free constraints
                for (index = 0; index < maxExtraRows; index++)
                {
                    if (extraReferenceCount[index] == 0)
                    {
                        // if it was used by a named constraint we have to invalidate it
                        string key = null;
                        foreach (KeyValuePair<string, int> kvp in extraConstraintMap)
                        {
                            if (kvp.Value == index)
                            {
                                key = kvp.Key;
                                break;
                            }
                        }
                        if (key != null)
                        {
                            extraConstraintMap.Remove(key);
                        }
                        // clean it
                        Array.Clear(extraConstraints, cols * index, cols);
                        break;
                    }
                }
                if (index == maxExtraRows)
                {
                    maxExtraRows += 32;
                    if (extraConstraints.Length < cols * maxExtraRows)
                    {
                        double[] newArray = new double[cols * maxExtraRows];
                        if (extraConstraints != null) Array.Copy(extraConstraints, newArray, extraConstraints.Length);
                        extraConstraints = newArray;
                    }
                    int[] newRefCount = new int[maxExtraRows];
                    if (extraReferenceCount != null) Array.Copy(extraReferenceCount, newRefCount, extraReferenceCount.Length);
                    extraReferenceCount = newRefCount;
                }
                if (name != null) extraConstraintMap[name] = index;
                newConstraint = true;
            }
            else
            {
                newConstraint = false;
            }
            extraReferenceCount[index]++;
            return index;
        }
    }

    public static class ArrayPool
    {
        private static List<ArraySet> pool = new List<ArraySet>();

        private static int createdArraySets = 0;
        private static int maximumPoolSize = 2;

        public static int MaximumPoolSize
        {
            get
            {
                lock (pool)
                {
                    return maximumPoolSize;
                }
            }
            set
            {
                lock (pool)
                {
                    maximumPoolSize = value;
                }
            }
        }

        public static ArraySet RequestArraySet(int rows, int cols)
        {
            lock (pool)
            {
                if (pool.Count == 0 && createdArraySets < maximumPoolSize)
                {
                    pool.Add(new ArraySet());
                    createdArraySets++;
                }
                else
                {
                    while (pool.Count == 0) Monitor.Wait(pool);
                }
                // find desirable size
                int bestIndex = -1;
                int desiredSize = rows * cols;
                bool haveMatch = false;
                for (int i = 0; i < pool.Count; i++)
                {
                    if (bestIndex == -1 || (!haveMatch && pool[i].MaxSize > pool[bestIndex].MaxSize) || (haveMatch && pool[i].MaxSize >= desiredSize && pool[i].MaxSize < pool[bestIndex].MaxSize))
                    {
                        bestIndex = i;
                        if (pool[i].MaxSize >= desiredSize)
                        {
                            haveMatch = true;
                        }
                    }
                }
                ArraySet result = pool[bestIndex];
                pool.RemoveAt(bestIndex);
                return result;
            }
        }

        public static void ReleaseArraySet(ArraySet arraySet)
        {
            lock (pool)
            {
                if (pool.Count >= maximumPoolSize)
                {
                    createdArraySets--;
                }
                else
                {
                    pool.Add(arraySet);
                    Monitor.Pulse(pool);
                }
            }
        }
    }
}
