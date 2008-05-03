using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Mage
{
    public class LU2
    {
        private int size;
        private int etaMax;
        public int etaSize;
        public double[,] _U;
        public int[] _P;
        public int[] _Q;
        public double[] _L; // eta file
        public int[] _LI; // col/row eta index   Li = I + L[i*size:(i+1)*size-1] * e_LI[i]'
        private double[] column;
        private double[] column2;

        // Ln...L0 B = P U Q
        // B = (Ln...L0)inv P U Q

        // Bx = b
        // (Ln...L0)inv P U Q x = b
        // P U Q x = (Ln...L0)b
        // U Q x = [P' (Ln...L0)b] = U z
        // Q x = z => x = Q' z

        // B~ = B + aj ep'
        // Ln...L0 B~ = P U~ Q = P U Q + Ln...L0 aj ep'
        // U~ = U + [P' Ln...L0 aj] ep' Q'

        // y B = z
        // y (Ln...L0)inv P U Q = z
        // [y (Ln...L0)inv P] U = z Q' = yy U
        // y (Ln...L0)inv P = yy
        // y = yy P' (Ln...L0)
        // y' = L0'...Ln' P yy'
        // Li' v = (I + eta ep')' v = v + ep (eta' v)

        // L1inv B = P1 U1 Q1
        // L1inv B = P2 U2 Q2
        // P2' L1inv B Q2' = U2
        // U3 = E U2 = E P2' L1inv B Q2'
        // [E P2'] L1inv B = [E U2] Q2 = U3 Q2 = U3 Q3
        // [P2 E P2'] L1inv B = P2 U3 Q3 = P3 U3 Q3
        // E = I + eta ep'
        // P2 E P2' = I + P2 eta ep' P2'
        // ep' P2' = (P2 ep)'

        public bool Singular { get; set; }

        // data will be modified, if you need to retain it clean pass a clone
        public LU2(double[,] data, int size)
        {
            this.size = size;
            etaSize = 0;
            etaMax = Math.Max(size + 100, 5 * size);
            _U = data;
            _P = new int[size];
            _Q = new int[size];
            _L = new double[etaMax * size];
            _LI = new int[etaMax];
            column = new double[size];
            column2 = new double[size];
        }

        public unsafe void BSolve(double* b)
        {
            fixed (double* c = column)
            {
                BSolveU(b, c);
                BSolveL(c, b);
            }
        }

        public unsafe void BSolveU(double* b, double* c)
        {
            int i, j, k;
            fixed (double* L = _L, U = _U)
            {
                fixed (int* P = _P, Q = _Q, LI = _LI)
                {
                    for (i = 0; i < size; i++)
                    {
                        c[i] = b[Q[i]]; // shuffle Q
                    }
                    for (k = 0; k < size; k++)
                    {
                        if (U[k * size + k] != 0) c[k] /= U[k * size + k];
                        else c[k] = 0; // value underspecified
                        for (i = k + 1; i < size; i++)
                        {
                            c[i] -= c[k] * U[k * size + i];
                        }
                    }
                }
            }
        }

        public unsafe void BSolveL(double* b, double* c)
        {
            int i, j;
            fixed (double* L = _L, U = _U)
            {
                fixed (int* P = _P, Q = _Q, LI = _LI)
                {
                    for (i = 0; i < size; i++)
                    {
                        c[P[i]] = b[i];
                    }
                    for (j = etaSize - 1; j >= 0; j--)
                    {
                        int row = LI[j]; // we're updating row element
                        // c~ = c + erow (eta' c)
                        double f = 0.0;
                        for (i = 0; i < size; i++)
                        {
                            f += c[i] * L[j * size + i];
                        }
                        c[row] += f;
                    }
                }
            }
        }

        public unsafe void FSolve(double* b)
        {
            fixed (double* c = column)
            {
                FSolveL(b, c);
                FSolveU(c, b);
            }
        }

        public unsafe void FSolveU(double* b, double* c)
        {
            int i, j, k;
            fixed (double* L = _L, U = _U, c2 = column2)
            {
                fixed (int* P = _P, Q = _Q, LI = _LI)
                {
                    for (i = 0; i < size; i++)
                    {
                        c2[i] = b[i];
                    }
                    for (k = size - 1; k >= 0; k--)
                    {
                        if (U[k * size + k] != 0) c2[k] /= U[k * size + k];
                        else c2[k] = 0; // value underspecified
                        for (i = 0; i < k; i++)
                        {
                            c2[i] -= c2[k] * U[i * size + k];
                        }
                    }
                    // shuffle Q
                    for (i = 0; i < size; i++)
                    {
                        c[Q[i]] = c2[i];
                    }
                }
            }
        }

        public unsafe void FSolveL(double* b, double* c)
        {
            // perform all eta operations and finally apply row permutation P
            int i, j, k;
            fixed (double* L = _L, U = _U)
            {
                fixed (int* P = _P, Q = _Q, LI = _LI)
                {
                    for (j = 0; j < etaSize; j++)
                    {
                        int row = LI[j]; // we're updating using row, if element is zero we can skip
                        // b~ = b + eta (erow' b)
                        double f = b[row];
                        if (Math.Abs(f) >= 0.00000001)
                        {
                            for (i = 0; i < size; i++)
                            {
                                b[i] += f * L[j * size + i];
                            }
                        }
                    }
                    for (i = 0; i < size; i++)
                    {
                        c[i] = b[P[i]];
                    }
                }
            }
        }

        internal static HighPerformanceTimer DecomposeTimer = new HighPerformanceTimer();

        // Performance Log, start, load Kavan, repeat dps time = 1
        // Primal=6.08148534180949E-05, Decompose=3.38125119182951E-05 implementation LU
        // Primal=7.7155968256824E-05, Decompose=4.80555577214333E-05  initial implementation LU2 with column pivoting
        // Primal=4.44975972839637E-05, Decompose=1.34763462504299E-05 LU2 no column pivoting

        // SMP
        // Primal=0.000248019081795013, Decompose=7.31092948026856E-05  LU2 no column pivoting

        public unsafe void Decompose()
        {
            DecomposeTimer.Start();
            etaSize = 0; // reset eta file
            Array.Clear(_L, 0, _L.Length);
            Singular = false;

            int i, j, k, pivi, pivj;
            fixed (double* L = _L, U = _U)
            {
                fixed (int* P = _P, Q = _Q, LI = _LI)
                {
                    // init P, Q
                    for (i = 0; i < size; i++)
                    {
                        P[i] = i;
                        Q[i] = i;
                    }

                    // eliminate by columns
                    for (j = 0; j < size; j++)
                    {
                        // select pivot
                        pivi = j;
                        pivj = j;
                        double max = Math.Abs(U[pivi * size + pivj]);
                        double newmax;

                        for (i = j; i < size; i++)
                        {
                            if ((newmax = Math.Abs(U[i * size + j])) > max)
                            {
                                pivi = i;
                                pivj = j;
                                max = newmax;
                            }
                        }

                        if (max < 0.000001)
                        {
                            // don't allow a 0 if you can help it, even if it costs more to pivot columns
                            for (k = j + 1; k < size; k++)
                            {
                                for (i = j; i < size; i++)
                                {
                                    if ((newmax = Math.Abs(U[i * size + k])) > max)
                                    {
                                        pivi = i;
                                        pivj = k;
                                        max = newmax;
                                    }
                                }
                            }
                        }

                        if (pivi != j)
                        {
                            for (k = j; k < size; k++) // columns before j have zeros in these rows
                            {
                                double t = U[pivi * size + k];
                                U[pivi * size + k] = U[j * size + k];
                                U[j * size + k] = t;
                            }

                            int tmp = P[pivi];
                            P[pivi] = P[j];
                            P[j] = tmp;
                        }

                        if (pivj != j)
                        {
                            for (k = 0; k < size; k++)
                            {
                                double t = U[k * size + pivj];
                                U[k * size + pivj] = U[k * size + j];
                                U[k * size + j] = t;
                            }

                            int tmp = Q[pivj];
                            Q[pivj] = Q[j];
                            Q[j] = tmp;
                        }

                        // eliminate, construct eta vector

                        if (etaSize >= etaMax) throw new InvalidOperationException();

                        if (Math.Abs(max) < 0.000001) Singular = true;

                        double a = 1 / U[j * size + j];
                        for (i = j + 1; i < size; i++)
                        {
                            int pi = P[i];
                            double f = -U[i * size + j] * a;
                            L[etaSize * size + pi] = f; // eta element that eliminates element in row i
                            U[i * size + j] = 0;
                            if (Math.Abs(f) > 0.00000001)
                            {
                                for (k = j + 1; k < size; k++)
                                {
                                    U[i * size + k] += f * U[j * size + k];
                                }
                            }
                        }
                        LI[etaSize] = P[j]; // yes I had to guess, but I used unit test to make sure it works :)
                        etaSize++;
                    }
                }
            }
            DecomposeTimer.Stop();
        }
    }
}
