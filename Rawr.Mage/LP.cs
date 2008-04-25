using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Mage
{
    static class LP
    {
        public static unsafe double[] Solve(double[,] data, int rows, int cols)
        {
            // c = data[rows,:]
            // A = data[0:rows-1,0:cols-1][I(rows)]
            // b = data[:,cols]
            // B = [cols:cols+rows-1]
            
            //  eps1 = 10-5, eps2 = 10-8, and eps3 = 10-6
            double eps = 0.00001;

            int[] _B = new int[rows];
            int[] _V = new int[cols];
            int i, j;            
            double[,] _LU = new double[rows, rows];
            LU lu = new LU(_LU, rows);
            double[] _d = new double[rows];
            double[] _x = new double[rows];
            double[] _w = new double[rows];

            bool feasible = false;
            int round = 0;
            fixed (double* a = data, LU = _LU, d = _d, x = _x, w = _w)
            {
                fixed (int* B = _B, V = _V)
                {
                    for (i = 0; i < rows; i++)
                    {
                        B[i] = cols + i;
                    }
                    for (j = 0; j < cols; j++)
                    {
                        V[j] = j;
                    }

                    do
                    {
                        // [L U] = lu(A(:,B_indices));
                        for (j = 0; j < rows; j++)
                        {
                            int col = B[j];
                            if (col < cols)
                            {
                                for (i = 0; i < rows; i++)
                                {
                                    LU[i * rows + j] = a[i * (cols + 1) + col];
                                }
                            }
                            else
                            {
                                col -= cols;
                                for (i = 0; i < rows; i++)
                                {
                                    LU[i * rows + j] = ((i == col) ? 1 : 0);
                                }
                            }
                        }
                        lu.Decompose();

                        // d = U \ (L \ b);
                        for (i = 0; i < rows; i++)
                        {
                            d[i] = a[i * (cols + 1) + cols];
                        }
                        lu.Solve(d, 1);

                        if (!feasible)
                        {
                            feasible = true;
                            for (i = 0; i < rows; i++)
                            {
                                if (d[i] < -eps)
                                {
                                    feasible = false;
                                    break;
                                }
                            }
                        }

                        // c_tilde(:,V_indices) = c(:,V_indices) - ((c(:,B_indices) \ U) \ L) *  A(:,V_indices);
                        // compute max c~ = cV - cB * AV
                        if (feasible)
                        {
                            for (i = 0; i < rows; i++)
                            {
                                if (B[i] < cols) x[i] = a[rows * (cols + 1) + B[i]];
                                else x[i] = 0;
                            }
                        }
                        else
                        {
                            for (i = 0; i < rows; i++)
                            {
                                if (d[B[i]] < -eps) x[i] = -1;
                                else x[i] = 0;
                            }
                        }
                        lu.SolveForward(x);

                        double maxc = eps;
                        int maxj = -1;
                        for (j = 0; j < cols; j++)
                        {
                            int col = V[j];

                            double c = ((feasible && col < cols) ? a[rows * (cols + 1) + col] : 0);
                            if (col < cols)
                            {
                                for (i = 0; i < rows; i++)
                                {
                                    c -= a[i * (cols + 1) + col] * x[i];
                                }
                            }
                            else
                            {
                                c -= x[col - cols];
                            }

                            if (c > maxc)
                            {
                                maxc = c;
                                maxj = j;
                                for (i = 0; i < rows; i++)
                                {
                                    w[i] = x[i];
                                }
                            }
                        }

                        if (maxj == -1)
                        {
                            // optimum, return solution (or could be no feasible solution)
                            // solution(B_indices,:) = d;
                            double[] ret = new double[cols + 1];
                            for (i = 0; i < rows; i++)
                            {
                                if (B[i] < cols) ret[B[i]] = d[i];
                            }
                            double value = 0;
                            for (i = 0; i < rows; i++)
                            {
                                if (B[i] < cols) value += a[rows * (cols + 1) + B[i]] * d[i];
                            }
                            ret[cols] = value;
                            return ret;
                        }

                        // w = U \ (L \ A(:,j));
                        int maxcol = V[maxj];
                        if (maxcol < cols)
                        {
                            for (i = 0; i < rows; i++)
                            {
                                w[i] = a[i * (cols + 1) + maxcol];
                            }
                        }
                        else
                        {
                            for (i = 0; i < rows; i++)
                            {
                                w[i] = ((i == maxcol - cols) ? 1 : 0);
                            }
                        }
                        lu.Solve(w, 1);

                        // min over i of d[i]/w[i] where w[i]>0
                        double minr = double.PositiveInfinity;
                        int mini = -1;
                        for (i = 0; i < rows; i++)
                        {
                            if (w[i] > eps)
                            {
                                double r = d[i] / w[i];
                                if (r < minr)
                                {
                                    minr = r;
                                    mini = i;
                                }
                            }
                        }

                        if (mini == -1)
                        {
                            // unbounded
                            throw new ArgumentException();
                        }

                        //System.Diagnostics.Debug.WriteLine(round + ": " + V[maxj] + " <=> " + B[mini]);
                        // swap base
                        int k = B[mini];
                        B[mini] = V[maxj];
                        V[maxj] = k;

                        round++;
                        if (round == 5000) round++;
                    } while (round < 5000);
                }
            }
            // just in case
            return new double[cols + 1];
        }
    }
}
