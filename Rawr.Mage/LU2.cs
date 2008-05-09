using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Mage
{
    public class LU2
    {
        private static int etaMax;
        private static int sizeMax;
        public static int[] _P;
        public static int[] _Q;
        public static double[] sparseL; // packed non-zero elements of eta vectors
        public static int[] sparseLI; // indices of non-zero elements
        public static int[] sparseLstart; // start index of non-zero elements of eta vector i
        public static int[] _LJ; // col/row eta index   Li = I + L[i*size:(i+1)*size-1] * e_LI[i]'
        private static double[] column;
        private static double[] column2;

        private int size;
        public int etaSize;
        public double[] _U;

        static LU2()
        {
            sizeMax = 200;
            RecreateArrays();
        }

        private static void RecreateArrays()
        {
            etaMax = Math.Max(sizeMax + 100, 2 * sizeMax);
            _P = new int[sizeMax];
            _Q = new int[sizeMax];
            //_L = new double[etaMax * size];
            _LJ = new int[etaMax];
            sparseL = new double[etaMax * sizeMax];
            sparseLI = new int[etaMax * sizeMax];
            sparseLstart = new int[etaMax];
            column = new double[sizeMax];
            column2 = new double[sizeMax];
        }

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

        // E = I + ep eta'
        // P E P' = I + P ep eta' P'
        // eta' P' = (P eta)'

        public bool Singular { get; set; }
        public int Rank { get; set; }

        // data will be modified, if you need to retain it clean pass a clone
        public LU2(double[] data, int size)
        {
            this.size = size;
            etaSize = 0;
            _U = data;
            if (size > sizeMax)
            {
                sizeMax = size;
                RecreateArrays();
            }
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
            int i, k;
            fixed (double* U = _U)
            {
                fixed (int* P = _P, Q = _Q)
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
            fixed (double* /*L = _L, */U = _U, sL = sparseL)
            {
                fixed (int* P = _P, Q = _Q, LJ = _LJ, sLI = sparseLI, sLstart = sparseLstart)
                {
                    for (i = 0; i < size; i++)
                    {
                        c[P[i]] = b[i];
                    }
                    for (j = etaSize - 1; j >= 0; j--)
                    {
                        if (j < size)
                        {
                            // eta columns from initial decomposition
                            int row = LJ[j]; // we're updating row element
                            // c~ = c + erow (eta' c)
                            double f = 0.0;
                            /*for (i = 0; i < size; i++)
                            {
                                f += c[i] * L[j * size + i];
                            }*/
                            int maxi = sLstart[j + 1];
                            for (i = sLstart[j]; i < maxi; i++)
                            {
                                f += c[sLI[i]] * sL[i];
                            }
                            c[row] += f;
                        }
                        else
                        {
                            // eta rows from update
                            int row = LJ[j]; // we're updating using row, if element is zero we can skip
                            // c~ = c + eta (erow' c)
                            double f = c[row];
                            if (Math.Abs(f) >= 0.00000001)
                            {
                                int maxi = sLstart[j + 1];
                                for (i = sLstart[j]; i < maxi; i++)
                                {
                                    c[sLI[i]] += f * sL[i];
                                }
                            }
                        }
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
            int i, k;
            fixed (double* U = _U, c2 = column2)
            {
                fixed (int* P = _P, Q = _Q)
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
            int i, j;
            fixed (double* /*L = _L, */U = _U, sL = sparseL)
            {
                fixed (int* P = _P, Q = _Q, LJ = _LJ, sLI = sparseLI, sLstart = sparseLstart)
                {
                    for (j = 0; j < etaSize; j++)
                    {
                        if (j < size)
                        {
                            // eta columns from initial decomposition
                            int row = LJ[j]; // we're updating using row, if element is zero we can skip
                            // b~ = b + eta (erow' b)
                            double f = b[row];
                            if (Math.Abs(f) >= 0.00000001)
                            {
                                /*for (i = 0; i < size; i++)
                                {
                                    b[i] += f * L[j * size + i];
                                }*/
                                int maxi = sLstart[j + 1];
                                for (i = sLstart[j]; i < maxi; i++)
                                {
                                    b[sLI[i]] += f * sL[i];
                                }
                            }
                        }
                        else
                        {
                            // eta rows from updates
                            // b~ = b + erow (eta' b)
                            int row = LJ[j]; // we're updating row element
                            double f = 0.0;
                            int maxi = sLstart[j + 1];
                            for (i = sLstart[j]; i < maxi; i++)
                            {
                                f += b[sLI[i]] * sL[i];
                            }
                            b[row] += f;
                        }
                    }
                    for (i = 0; i < size; i++)
                    {
                        c[i] = b[P[i]];
                    }
                }
            }
        }

        // Performance Log, start, load Kavan, repeat dps time = 1
        // Primal=6.08148534180949E-05, Decompose=3.38125119182951E-05 implementation LU
        // Primal=7.7155968256824E-05, Decompose=4.80555577214333E-05  initial implementation LU2 with column pivoting
        // Primal=4.44975972839637E-05, Decompose=1.34763462504299E-05 LU2 no column pivoting
        // Primal=4.10642857990147E-05, Decompose=1.28461907883656E-05 FSolveL => sparse
        // Primal=3.96870122800117E-05, Decompose=1.36131169065829E-05 BSolveL => sparse
        // Primal=2.73956138996725E-05, Decompose=4.29207679646327E-06 update rule

        // SMP
        // Primal=0.000248019081795013, Decompose=7.31092948026856E-05 LU2 no column pivoting
        // Primal=0.000217759250424469, Decompose=7.53546090376799E-05 sparse L
        // Primal=0.00013135285568704, Decompose=1.44506264553419E-05 update rule

        public unsafe void Decompose()
        {
            etaSize = 0; // reset eta file
            //Array.Clear(_L, 0, _L.Length);
            Singular = false;
            Rank = size;

            int i, j, k, pivi, pivj;
            fixed (double* /*L = _L, */U = _U, sL = sparseL)
            {
                fixed (int* P = _P, Q = _Q, LJ = _LJ, sLI = sparseLI, sLstart = sparseLstart)
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

                        if (max < 0.1)
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

                        if (!Singular && Math.Abs(max) < 0.000001)
                        {
                            Singular = true;
                            Rank = j;
                        }

                        sLstart[etaSize + 1] = sLstart[etaSize];
                        double a = 1 / U[j * size + j];
                        for (i = j + 1; i < size; i++)
                        {
                            int pi = P[i];
                            double f = -U[i * size + j] * a;
                            U[i * size + j] = 0;
                            if (Math.Abs(f) > 0.00000001)
                            {
                                //L[etaSize * size + pi] = f; // eta element that eliminates element in row i
                                // sparse L construction
                                sL[sLstart[etaSize + 1]] = f;
                                sLI[sLstart[etaSize + 1]] = pi;
                                sLstart[etaSize + 1]++;
                                // update U
                                for (k = j + 1; k < size; k++)
                                {
                                    U[i * size + k] += f * U[j * size + k];
                                }
                            }
                        }
                        LJ[etaSize] = P[j]; // yes I had to guess, but I used unit test to make sure it works :)
                        etaSize++;
                    }
                }
            }
        }

        // B~ = B + aj ecol'
        // Ln...L0 B~ = P U~ Q = P U Q + Ln...L0 aj ecol'
        // U~ = U + [P' Ln...L0 aj] ecol' Q'
        // a = [P' Ln...L0 aj]

        // replace column col in basis B with aj
        public unsafe void Update(double* a, int col)
        {
            int i, j, k;
            fixed (double* /*L = _L, */U = _U, sL = sparseL, c = column)
            {
                fixed (int* P = _P, Q = _Q, LJ = _LJ, sLI = sparseLI, sLstart = sparseLstart)
                {
                    for (j = 0; j < size; j++)
                    {
                        if (Q[j] == col) break;
                        //c[i] = ecol[Q[i]]; // shuffle Q
                    }
                    col = j;

                    // place in column col = j
                    // rotate columns to get upper hessenberg form (col...lastnz)
                    int lastnz = -1;
                    for (i = 0; i < size; i++)
                    {
                        if (Math.Abs(a[i]) > 0.000001) lastnz = i;
                    }
                    // XXaXXXX    XXXXXaX
                    //  XaXXXX     XXXXaX
                    //   aXXXX      XXXaX   <--- col
                    //   aXXXX  =>  XXXaX
                    //   a XXX       XXaX
                    //   a  XX        XaX   <--- lastnz
                    //       X          X

                    int Qcol = Q[col];
                    for (j = col; j < lastnz; j++)
                    {
                        Q[j] = Q[j + 1];
                        for (i = 0; i <= j + 1; i++)
                        {
                            U[i * size + j] = U[i * size + j + 1];
                        }                        
                    }
                    Q[lastnz] = Qcol;
                    for (i = 0; i <= lastnz; i++)
                    {
                        U[i * size + lastnz] = a[i];
                    }

                    // eliminate row at index col up to lastnz using rows below it
                    // we're eliminating on previous diagonals, so we know it is safe (we shouldn't update on a singular basis)

                    // E = I + ep eta'
                    // P E P' = I + P ep eta' P'
                    // eta' P' = (P eta)'

                    if (etaSize >= etaMax) throw new InvalidOperationException();

                    sLstart[etaSize + 1] = sLstart[etaSize];

                    for (j = col; j < lastnz; j++)
                    {
                        int pj = P[j + 1];
                        double f = -U[col * size + j] / U[(j + 1) * size + j];
                        U[col * size + j] = 0;
                        if (Math.Abs(f) > 0.00000001)
                        {
                            // sparse L construction
                            sL[sLstart[etaSize + 1]] = f;
                            sLI[sLstart[etaSize + 1]] = pj;
                            sLstart[etaSize + 1]++;
                            // update U
                            for (k = j + 1; k < size; k++)
                            {
                                U[col * size + k] += f * U[(j + 1) * size + k];
                            }
                        }
                    }
                    LJ[etaSize] = P[col];
                    etaSize++;

                    // rotate rows to get upper triangular form (col...lastnz)

                    // XXXXXXX    XXXXXXX
                    //  XXXXXX     XXXXXX
                    //      XX      XXXXX   <--- col
                    //   XXXXX  =>   XXXX
                    //    XXXX        XXX
                    //     XXX         XX   <--- lastnz
                    //       X          X

                    // store col in temp
                    int Pcol = P[col];
                    for (j = lastnz; j < size; j++)
                    {
                        c[j] = U[col * size + j];
                    }
                    for (i = col; i < lastnz; i++)
                    {
                        P[i] = P[i + 1];
                        for (j = (i == 0 ? 0 : i - 1); j < size; j++)
                        {
                            U[i * size + j] = U[(i + 1) * size + j];
                        }
                    }
                    P[lastnz] = Pcol;
                    if (lastnz > 0) U[lastnz * size + lastnz - 1] = 0.0;
                    for (j = lastnz; j < size; j++)
                    {
                        U[lastnz * size + j] = c[j];
                    }
                }
            }
        }    
    }
}
