using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Mage
{
    public class LP
    {
        private double[,] data;
        private int[] disabled;
        private int rows;
        private int cols;

        int[] _B;
        int[] _V;
        double[,] _LU;
        LU lu;
        double[] _d;
        double[] _x;
        double[] _w;
        double[] _wd;
        double[] _u;
        double[] _c;

        public LP Clone()
        {
            LP clone = (LP)MemberwiseClone();
            clone._B = (int[])_B.Clone();
            clone._V = (int[])_V.Clone();
            clone._d = (double[])_d.Clone();
            clone._u = (double[])_u.Clone();
            clone._c = (double[])_c.Clone();
            clone.disabled = (int[])disabled.Clone();
            return clone;
        }

        public void DisableColumn(int col)
        {
            disabled[col] = 1;
        }

        public LP(double[,] data, int rows, int cols)
        {
            rows++; // add extra row for disabled
            this.data = data;
            this.rows = rows;
            this.cols = cols;

            _B = new int[rows];
            _V = new int[cols];
            _LU = new double[rows, rows];
            lu = new LU(_LU, rows);
            _d = new double[rows];
            _x = new double[rows];
            _w = new double[rows];
            _wd = new double[cols];
            _u = new double[rows];
            _c = new double[cols];
            disabled = new int[cols];

            for (int i = 0; i < rows; i++)
            {
                _B[i] = cols + i;
            }
            for (int j = 0; j < cols; j++)
            {
                _V[j] = j;
            }
        }

        public unsafe double[] SolvePrimal()
        {
            // c = data[rows,:]
            // A = data[0:rows-1,0:cols-1][I(rows)]
            // b = data[:,cols]
            // B = [cols:cols+rows-1]

            // http://books.google.com/books?id=-afIqQSZE5AC&pg=PA313&dq=revised+simplex+FTRAN&source=gbs_toc_s&cad=1&sig=dPDCfoi0CqPh14mIfnQr-wM2AKg#PPA159,M1
            // LU = A_B
            // d = x_B <- A_B^-1*b ... primal solution
            // u = u <- c_B*A_B^-1 ... dual solution
            // w_N <- c_N - u*A_N  ... dual solution

            //  eps1 = 10-5, eps2 = 10-8, and eps3 = 10-6
            double eps = 0.00001;

            int i, j;
            bool feasible = false;
            int round = 0;
            fixed (double* a = data, LU = _LU, d = _d, x = _x, w = _w, c = _c, u = _u)
            {
                fixed (int* B = _B, V = _V, D = disabled)
                {
                    do
                    {
                        // [L U] = lu(A(:,B_indices));
                        for (j = 0; j < rows; j++)
                        {
                            int col = B[j];
                            if (col < cols)
                            {
                                for (i = 0; i < rows - 1; i++)
                                {
                                    LU[i * rows + j] = a[i * (cols + 1) + col];
                                }
                                LU[i * rows + j] = D[col];
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
                        if (lu.Singular)
                        {
                            //System.Diagnostics.Debug.WriteLine("Basis singular");
                            // TODO probably good to investigate what causes basis to become singular
                            // at least from some preliminary testing doesn't seem to have side effects
                        }

                        if (!feasible)
                        {
                            // d = U \ (L \ b);
                            for (i = 0; i < rows - 1; i++)
                            {
                                d[i] = a[i * (cols + 1) + cols];
                            }
                            d[i] = 0;
                            lu.Solve(d, 1);

                            feasible = true;
                            for (i = 0; i < rows; i++)
                            {
                                if (d[i] < -eps)
                                {
                                    feasible = false;
                                    break;
                                }
                            }
                            if (feasible)
                            {
                                // we have a feasible solution, initialize phase 2
                                for (i = 0; i < rows; i++)
                                {
                                    if (B[i] < cols) u[i] = a[(rows - 1) * (cols + 1) + B[i]];
                                    else u[i] = 0;
                                }
                                lu.SolveForward(u);
                                for (j = 0; j < cols; j++)
                                {
                                    int col = V[j];

                                    double cost = ((col < cols) ? a[(rows - 1) * (cols + 1) + col] : 0);
                                    if (col < cols)
                                    {
                                        for (i = 0; i < rows - 1; i++)
                                        {
                                            cost -= a[i * (cols + 1) + col] * u[i];
                                        }
                                        cost -= D[col] * u[i];
                                    }
                                    else
                                    {
                                        cost -= u[col - cols];
                                    }
                                    c[j] = cost;
                                }
                            }
                        }

                        // c_tilde(:,V_indices) = c(:,V_indices) - ((c(:,B_indices) \ U) \ L) *  A(:,V_indices);
                        // compute max c~ = cV - cB * AV
                        if (!feasible)
                        {
                            double infis = 0;
                            for (i = 0; i < rows; i++)
                            {
                                if (d[i] < -eps)
                                {
                                    x[i] = 1;
                                    infis -= d[i];
                                }
                                else x[i] = 0;
                            }
                            lu.SolveForward(x);
                            for (j = 0; j < cols; j++)
                            {
                                int col = V[j];

                                double cost = 0;
                                if (col < cols)
                                {
                                    for (i = 0; i < rows - 1; i++)
                                    {
                                        cost -= a[i * (cols + 1) + col] * x[i];
                                    }
                                    cost -= D[col] * x[i];
                                }
                                else
                                {
                                    cost -= x[col - cols];
                                }
                                c[j] = cost;
                            }
                        }

                        double maxc = eps;
                        int maxj = -1;
                        for (j = 0; j < cols; j++)
                        {
                            double cost = c[j];
                            if (cost > maxc)
                            {
                                maxc = cost;
                                maxj = j;
                            }
                        }

                        if (maxj == -1)
                        {
                            // optimum, return solution (or could be no feasible solution)
                            // solution(B_indices,:) = d;
                            double[] ret = new double[cols + 1];
                            for (i = 0; i < rows; i++)
                            {
                                if (B[i] < cols && d[i] > eps) ret[B[i]] = d[i];
                            }
                            double value = 0;
                            for (i = 0; i < rows; i++)
                            {
                                if (B[i] < cols) value += a[(rows - 1) * (cols + 1) + B[i]] * d[i];
                            }
                            ret[cols] = value;
                            //System.Diagnostics.Debug.WriteLine("Primal solved in " + round);
                            return ret;
                        }

                        // w = U \ (L \ A(:,j));
                        int maxcol = V[maxj];
                        if (maxcol < cols)
                        {
                            for (i = 0; i < rows - 1; i++)
                            {
                                w[i] = a[i * (cols + 1) + maxcol];
                            }
                            w[i] = D[maxcol];
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
                            throw new InvalidOperationException();
                        }

                        if (feasible)
                        {
                            // minr = primal step
                            // rd = dual step

                            double rd = c[maxj] / w[mini];

                            // x = z <- e_mini*A_B^-1
                            for (i = 0; i < rows; i++)
                            {
                                x[i] = ((i == mini) ? 1 : 0);
                            }
                            lu.SolveForward(x); // TODO exploit nature of x

                            // update primal and dual
                            for (i = 0; i < rows; i++)
                            {
                                d[i] -= minr * w[i];
                                //u[i] -= rd * x[i];
                            }
                            d[mini] = minr;
                            for (j = 0; j < cols; j++)
                            {
                                int col = V[j];
                                if (col < cols)
                                {
                                    for (i = 0; i < rows - 1; i++)
                                    {
                                        c[j] -= rd * a[i * (cols + 1) + col] * x[i];
                                    }
                                    c[j] -= rd * D[col] * x[i];
                                }
                                else
                                {
                                    c[j] -= rd * x[col - cols];
                                }
                                c[maxj] = -rd;
                            }
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

        // assumes an existing feasible dual solution (use after restricting constraints, i.e. disabling columns)
        public unsafe double[] SolveDual()
        {
            // c = data[rows,:]
            // A = data[0:rows-1,0:cols-1][I(rows)]
            // b = data[:,cols]
            // B = [cols:cols+rows-1]

            // http://books.google.com/books?id=-afIqQSZE5AC&pg=PA313&dq=revised+simplex+FTRAN&source=gbs_toc_s&cad=1&sig=dPDCfoi0CqPh14mIfnQr-wM2AKg#PPA159,M1
            // LU = A_B
            // d = x_B <- A_B^-1*b ... primal solution
            // u = u <- c_B*A_B^-1 ... dual solution
            // w_N <- c_N - u*A_N  ... dual solution

            //  eps1 = 10-5, eps2 = 10-8, and eps3 = 10-6
            double eps = 0.00001;

            int i, j;
            int round = 0;
            bool needsRecalc = true;
            fixed (double* a = data, LU = _LU, d = _d, x = _x, w = _w, c = _c, u = _u, wd = _wd)
            {
                fixed (int* B = _B, V = _V, D = disabled)
                {
                    do
                    {
                        // [L U] = lu(A(:,B_indices));
                        for (j = 0; j < rows; j++)
                        {
                            int col = B[j];
                            if (col < cols)
                            {
                                for (i = 0; i < rows - 1; i++)
                                {
                                    LU[i * rows + j] = a[i * (cols + 1) + col];
                                }
                                LU[i * rows + j] = D[col];
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
                        if (lu.Singular)
                        {
                            //System.Diagnostics.Debug.WriteLine("Basis singular");
                            // when restricting constraints sometimes the basis becomes singular
                            // restart normal primal problem
                            for (i = 0; i < rows; i++)
                            {
                                B[i] = cols + i;
                            }
                            for (j = 0; j < cols; j++)
                            {
                                V[j] = j;
                            }
                            return SolvePrimal();
                        }

                        if (needsRecalc)
                        {
                            // d = U \ (L \ b);
                            for (i = 0; i < rows - 1; i++)
                            {
                                d[i] = a[i * (cols + 1) + cols];
                            }
                            d[i] = 0;
                            lu.Solve(d, 1);

                            for (i = 0; i < rows; i++)
                            {
                                if (B[i] < cols) u[i] = a[(rows - 1) * (cols + 1) + B[i]];
                                else u[i] = 0;
                            }
                            lu.SolveForward(u);
                            for (j = 0; j < cols; j++)
                            {
                                int col = V[j];

                                double cost = ((col < cols) ? a[(rows - 1) * (cols + 1) + col] : 0);
                                if (col < cols)
                                {
                                    for (i = 0; i < rows - 1; i++)
                                    {
                                        cost -= a[i * (cols + 1) + col] * u[i];
                                    }
                                    cost -= D[col] * u[i];
                                }
                                else
                                {
                                    cost -= u[col - cols];
                                }
                                if (cost > eps)
                                {
                                    // this should only happen if dual is not feasible
                                    // should not come here unless LU is singular
                                    // ...
                                    // maybe it can
                                    // at least we don't have to start from scratch
                                    return SolvePrimal();
                                }
                                c[j] = cost;
                            }

                            needsRecalc = false;
                        }

                        double mind = -eps;
                        int mini = -1;
                        for (i = 0; i < rows; i++)
                        {
                            double cost = d[i];
                            if (cost < mind)
                            {
                                mind = cost;
                                mini = i;
                            }
                        }

                        if (mini == -1)
                        {
                            // optimum, return solution
                            // solution(B_indices,:) = d;
                            double[] ret = new double[cols + 1];
                            for (i = 0; i < rows; i++)
                            {
                                if (B[i] < cols && d[i] > eps) ret[B[i]] = d[i];
                            }
                            double value = 0;
                            for (i = 0; i < rows; i++)
                            {
                                if (B[i] < cols) value += a[(rows - 1) * (cols + 1) + B[i]] * d[i];
                            }
                            ret[cols] = value;
                            //System.Diagnostics.Debug.WriteLine("Dual solved in " + round);
                            return ret;
                        }

                        // x = z <- e_mini*A_B^-1
                        for (i = 0; i < rows; i++)
                        {
                            x[i] = ((i == mini) ? 1 : 0);
                        }
                        lu.SolveForward(x); // TODO exploit nature of x

                        // min over i of d[i]/w[i] where w[i]>0
                        double minr = double.PositiveInfinity;
                        int minj = -1;
                        for (j = 0; j < cols; j++)
                        {
                            wd[j] = 0;
                            int col = V[j];
                            if (col < cols)
                            {
                                for (i = 0; i < rows - 1; i++)
                                {
                                    wd[j] += a[i * (cols + 1) + col] * x[i];
                                }
                                wd[j] += D[col] * x[i];
                            }
                            else
                            {
                                wd[j] += x[col - cols];
                            }
                            if (wd[j] < -eps)
                            {
                                double r = c[j] / wd[j];
                                if (r < minr)
                                {
                                    minr = r;
                                    minj = j;
                                }
                            }
                        }

                        if (minj == -1)
                        {
                            // unfeasible
                            throw new InvalidOperationException();
                        }

                        // w = U \ (L \ A(:,j));
                        int mincol = V[minj];
                        if (mincol < cols)
                        {
                            for (i = 0; i < rows - 1; i++)
                            {
                                w[i] = a[i * (cols + 1) + mincol];
                            }
                            w[i] = D[mincol];
                        }
                        else
                        {
                            for (i = 0; i < rows; i++)
                            {
                                w[i] = ((i == mincol - cols) ? 1 : 0);
                            }
                        }
                        lu.Solve(w, 1);

                        // -minr = dual step
                        // rp = primal step

                        double rp = d[mini] / wd[minj];

                        // update primal and dual
                        for (i = 0; i < rows; i++)
                        {
                            d[i] -= rp * w[i];
                            //u[i] -= minr * x[i];
                        }
                        d[mini] = rp;
                        for (j = 0; j < cols; j++)
                        {
                            c[j] -= minr * wd[j];
                        }
                        c[minj] = -minr;

                        //System.Diagnostics.Debug.WriteLine(round + ": " + V[maxj] + " <=> " + B[mini]);
                        // swap base
                        int k = B[mini];
                        B[mini] = V[minj];
                        V[minj] = k;

                        round++;
                        if (round == 5000) round++;
                    } while (round < 5000);
                }
            }
            // just in case
            return new double[cols + 1];
        }

        private unsafe double[] SolvePrimalWithoutDisabledRow()
        {
            // c = data[rows,:]
            // A = data[0:rows-1,0:cols-1][I(rows)]
            // b = data[:,cols]
            // B = [cols:cols+rows-1]

            // http://books.google.com/books?id=-afIqQSZE5AC&pg=PA313&dq=revised+simplex+FTRAN&source=gbs_toc_s&cad=1&sig=dPDCfoi0CqPh14mIfnQr-wM2AKg#PPA159,M1
            // LU = A_B
            // d = x_B <- A_B^-1*b ... primal solution
            // u = u <- c_B*A_B^-1 ... dual solution
            // w_N <- c_N - u*A_N

            //  eps1 = 10-5, eps2 = 10-8, and eps3 = 10-6
            double eps = 0.00001;

            int i, j;
            bool feasible = false;
            int round = 0;
            fixed (double* a = data, LU = _LU, d = _d, x = _x, w = _w, c = _c, u = _u)
            {
                fixed (int* B = _B, V = _V)
                {
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

                        if (!feasible)
                        {
                            // d = U \ (L \ b);
                            for (i = 0; i < rows; i++)
                            {
                                d[i] = a[i * (cols + 1) + cols];
                            }
                            lu.Solve(d, 1);

                            feasible = true;
                            for (i = 0; i < rows; i++)
                            {
                                if (d[i] < -eps)
                                {
                                    feasible = false;
                                    break;
                                }
                            }
                            if (feasible)
                            {
                                // we have a feasible solution, initialize phase 2
                                for (i = 0; i < rows; i++)
                                {
                                    if (B[i] < cols) u[i] = a[rows * (cols + 1) + B[i]];
                                    else u[i] = 0;
                                }
                                lu.SolveForward(u);
                                for (j = 0; j < cols; j++)
                                {
                                    int col = V[j];

                                    double cost = ((col < cols) ? a[rows * (cols + 1) + col] : 0);
                                    if (col < cols)
                                    {
                                        for (i = 0; i < rows; i++)
                                        {
                                            cost -= a[i * (cols + 1) + col] * u[i];
                                        }
                                    }
                                    else
                                    {
                                        cost -= u[col - cols];
                                    }
                                    c[j] = cost;
                                }
                            }
                        }

                        // c_tilde(:,V_indices) = c(:,V_indices) - ((c(:,B_indices) \ U) \ L) *  A(:,V_indices);
                        // compute max c~ = cV - cB * AV
                        if (!feasible)
                        {
                            for (i = 0; i < rows; i++)
                            {
                                if (d[B[i]] < -eps) x[i] = -1;
                                else x[i] = 0;
                            }
                            lu.SolveForward(x);
                            for (j = 0; j < cols; j++)
                            {
                                int col = V[j];

                                double cost = ((feasible && col < cols) ? a[rows * (cols + 1) + col] : 0);
                                if (col < cols)
                                {
                                    for (i = 0; i < rows; i++)
                                    {
                                        cost -= a[i * (cols + 1) + col] * x[i];
                                    }
                                }
                                else
                                {
                                    cost -= x[col - cols];
                                }
                                c[j] = cost;
                            }
                        }

                        double maxc = eps;
                        int maxj = -1;
                        for (j = 0; j < cols; j++)
                        {
                            double cost = c[j];
                            if (cost > maxc)
                            {
                                maxc = cost;
                                maxj = j;
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

                        if (feasible)
                        {
                            // minr = primal step
                            // rd = dual step

                            double rd = c[maxj] / w[mini];

                            // x = z <- e_mini*A_B^-1
                            for (i = 0; i < rows; i++)
                            {
                                x[i] = ((i == mini) ? 1 : 0);
                            }
                            lu.SolveForward(x); // TODO exploit nature of x

                            // update primal and dual
                            for (i = 0; i < rows; i++)
                            {
                                d[i] -= minr * w[i];
                                u[i] -= rd * x[i];
                            }
                            d[mini] = minr;
                            for (j = 0; j < cols; j++)
                            {
                                int col = V[j];
                                if (col < cols)
                                {
                                    for (i = 0; i < rows; i++)
                                    {
                                        c[j] -= rd * a[i * (cols + 1) + col] * x[i];
                                    }
                                }
                                else
                                {
                                    c[j] -= rd * x[col - cols];
                                }
                                c[maxj] = -rd;
                            }
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
                                if (d[i] < -eps) x[i] = 1;
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
