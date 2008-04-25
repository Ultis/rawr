using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Mage
{
    // LU decomposition
    class LU
    {
        private int size;
        private double[,] data;
        private int[] pivots;
        private int pivotSign;
        private double[] column;

        // data will be modified, if you need to retain it clean pass a clone
        public LU(double[,] data, int size)
        {
            this.size = size;
            this.data = data;
            pivots = new int[size];
            column = new double[size];
        }

        public unsafe void SolveForward(double* b)
        {
            int i, k;
            fixed (double* a = data, c = column)
            {
                fixed (int* p = pivots)
                {
                    // solve b = y*U
                    for (k = 0; k < size; k++)
                    {
                        b[k] /= a[k * size + k];
                        for (i = k + 1; i < size; i++)
                        {
                            b[i] -= b[k] * a[k * size + i];
                        }
                    }

                    // solve b = x*L
                    for (k = size - 1; k >= 0; k--)
                    {
                        for (i = 0; i < k; i++)
                        {
                            b[i] -= b[k] * a[k * size + i];
                        }
                    }

                    // permute
                    for (i = 0; i < size; i++)
                    {
                        c[p[i]] = b[i];
                    }

                    // copy back
                    for (i = 0; i < size; i++)
                    {
                        b[i] = c[i];
                    }
                }
            }
        }

        public unsafe void Solve(double* b, int cols)
        {
            int i, k;
            fixed (double* a = data, c = column)
            {
                fixed (int* p = pivots)
                {
                    for (int bcol = 0; bcol < cols; bcol++)
                    {
                        for (i = 0; i < size; i++)
                        {
                            c[i] = b[p[i] * cols + bcol];
                        }

                        // solve L*Y = B(piv,:)
                        for (k = 0; k < size; k++)
                        {
                            for (i = k + 1; i < size; i++)
                            {
                                c[i] -= c[k] * a[i * size + k];
                            }
                        }
                        // solve U*X = Y;
                        for (k = size - 1; k >= 0; k--)
                        {
                            if (a[k * size + k] != 0) c[k] /= a[k * size + k];
                            else c[k] = 0; // value underspecified
                            for (i = 0; i < k; i++)
                            {
                                c[i] -= c[k] * a[i * size + k];
                            }
                        }
                        // copy back
                        for (i = 0; i < size; i++)
                        {
                            b[i * cols + bcol] = c[i];
                        }
                    }
                }
            }
        }

        public unsafe void Decompose()
        {
            fixed (double* a = data, c = column)
            {
                fixed (int* p = pivots)
                {
                    for (int i = 0; i < size; i++)
                    {
                        p[i] = i;
                    }

                    pivotSign = 1;

                    for (int j = 0; j < size; j++)
                    {
                        for (int i = 0; i < size; i++)
                        {
                            c[i] = a[i * size + j];
                        }

                        for (int i = 0; i < size; i++)
                        {
                            int kmax = Math.Min(i, j);
                            double s = 0.0;

                            for (int k = 0; k < kmax; k++)
                            {
                                s += a[i * size + k] * c[k];
                            }

                            a[i * size + j] = c[i] - s;
                            c[i] -= s;
                        }

                        int piv = j;

                        for (int i = j + 1; i < size; i++)
                        {
                            if (Math.Abs(c[i]) > Math.Abs(c[piv]))
                            {
                                piv = i;
                            }
                        }

                        if (piv != j)
                        {
                            for (int k = 0; k < size; k++)
                            {
                                double t = a[piv * size + k];
                                a[piv * size + k] = a[j * size + k];
                                a[j * size + k] = t;
                            }

                            int tmp = p[piv];
                            p[piv] = p[j];
                            p[j] = tmp;

                            pivotSign = -pivotSign;
                        }

                        if ((j < size) && (a[j * size + j] != 0.0))
                        {
                            double ajj = a[j * size + j];
                            for (int i = j + 1; i < size; i++)
                            {
                                a[i * size + j] /= ajj;
                            }
                        }
                    }
                }
            }
        }
    }
}
