using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Mage
{
    public class LP
    {
        private SparseMatrix A;
        private int[] disabled;
        private int rows;
        private int cols;

        private int[] _B;
        private int[] _V;
        LU2 lu;

        private static double[] _LU;
        private static double[] _d;
        private static double[] _x;
        private static double[] _w;
        private static double[] _ww;
        private static double[] _wd;
        private static double[] _u;
        private static double[] _c;
        private static double[] _b;
        private static double[] _cost;

        private static int maxRows = 0;
        private static int maxCols = 0;

        static LP()
        {
            maxRows = 200;
            maxCols = 5000;
            RecreateArrays();
        }

        private static void RecreateArrays()
        {
            _LU = new double[maxRows * maxRows];
            _d = new double[maxRows];
            _x = new double[maxRows];
            _w = new double[maxRows];
            _ww = new double[maxRows];
            _wd = new double[maxCols];
            _u = new double[maxRows];
            _c = new double[maxCols];
            _cost = new double[maxCols];
            _b = new double[maxRows];
        }


        public LP Clone()
        {
            LP clone = (LP)MemberwiseClone();
            clone._B = (int[])_B.Clone();
            clone._V = (int[])_V.Clone();
            clone.disabled = (int[])disabled.Clone();
            return clone;
        }

        public void DisableColumn(int col)
        {
            disabled[col] = 1;
        }

        public double this[int row, int col]
        {
            get
            {
                if (row == rows - 1) return _cost[col];
                else if (col == cols) return _b[row];
                else return A[row, col];
            }
            set
            {
                if (row == rows - 1) _cost[col] = value;
                else if (col == cols) _b[row] = value;
                else A[row, col] = value;
            }
        }

        private bool constructed;
        public void EndConstruction()
        {
            if (constructed) return;
            for (int i = 0; i < rows - 1; i++)
            {
                A[i, cols + i] = 1.0;
            }
            A.EndConstruction();
            constructed = true;
        }

        public LP(int rows, int cols)
        {
            rows++; // add extra row for disabled
            if (rows > maxRows || cols > maxCols)
            {
                maxRows = Math.Max(rows, maxRows);
                maxCols = Math.Max(cols, maxCols);
                RecreateArrays();
            }
            this.rows = rows;
            this.cols = cols;

            A = new SparseMatrix(rows - 1, cols + rows);
            _B = new int[rows];
            _V = new int[cols];
            lu = new LU2(_LU, rows);
            disabled = new int[cols + rows];
            disabled[cols + rows - 1] = 1;
            Array.Clear(_cost, 0, cols + rows);
            Array.Clear(_b, 0, rows);

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
            double* cc, sValue;
            int* sRow;
            int sCol1, sCol2;
            int redecompose = 0;
            fixed (double* a = SparseMatrix.data, LU = _LU, d = _d, x = _x, w = _w, ww = _ww, c = _c, u = _u, b = _b, cost = _cost, sparseValue = SparseMatrix.value)
            {
                fixed (int* B = _B, V = _V, D = disabled, sparseRow = SparseMatrix.row, sparseCol = SparseMatrix.col)
                {
                    do
                    {
                        // [L U] = lu(A(:,B_indices));
                        if (redecompose <= 0)
                        {
                            for (j = 0; j < rows; j++)
                            {
                                int col = B[j];
                                for (i = 0; i < rows - 1; i++)
                                {
                                    LU[i * rows + j] = a[i * (cols + rows) + col];
                                }
                                LU[i * rows + j] = D[col];
                            }
                            lu.Decompose();
                            redecompose = 50; // decompose every 50 iterations to maintain stability
                        }
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
                                d[i] = b[i];
                            }
                            d[i] = 0;
                            lu.FSolve(d);

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
                                    if (B[i] < cols) u[i] = cost[B[i]];
                                    else u[i] = 0;
                                }
                                lu.BSolve(u);
                                for (j = 0; j < cols; j++)
                                {
                                    int col = V[j];

                                    double costcol = ((col < cols) ? cost[col] : 0);
                                    sCol1 = sparseCol[col];
                                    sCol2 = sparseCol[col + 1];
                                    sRow = sparseRow + sCol1;
                                    sValue = sparseValue + sCol1;
                                    for (i = sCol1; i < sCol2; i++, sRow++, sValue++)
                                    {
                                        costcol -= *sValue * u[*sRow];
                                    }
                                    costcol -= D[col] * u[rows - 1];
                                    c[j] = costcol;
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
                            lu.BSolve(x);
                            for (j = 0; j < cols; j++)
                            {
                                int col = V[j];

                                double costcol = 0;
                                sCol1 = sparseCol[col];
                                sCol2 = sparseCol[col + 1];
                                sRow = sparseRow + sCol1;
                                sValue = sparseValue + sCol1;
                                for (i = sCol1; i < sCol2; i++, sRow++, sValue++)
                                {
                                    costcol -= *sValue * x[*sRow];
                                }
                                costcol -= D[col] * x[rows - 1];
                                c[j] = costcol;
                            }
                        }

                        double maxc = eps;
                        int maxj = -1;
                        for (j = 0; j < cols; j++)
                        {
                            double costj = c[j];
                            if (costj > maxc)
                            {
                                maxc = costj;
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
                                if (B[i] < cols) value += cost[B[i]] * d[i];
                            }
                            ret[cols] = value;
                            return ret;
                        }

                        // w = U \ (L \ A(:,j));
                        int maxcol = V[maxj];
                        for (i = 0; i < rows - 1; i++)
                        {
                            w[i] = a[i * (cols + rows) + maxcol];
                        }
                        w[i] = D[maxcol];
                        //lu.FSolve(w);
                        lu.FSolveL(w, ww);
                        lu.FSolveU(ww, w);

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
                            lu.BSolve(x); // TODO exploit nature of x

                            // update primal and dual
                            for (i = 0; i < rows; i++)
                            {
                                d[i] -= minr * w[i];
                                //u[i] -= rd * x[i];
                            }
                            d[mini] = minr;
                            cc = c;
                            for (j = 0; j < cols; j++, cc++)
                            {
                                int col = V[j];
                                sCol1 = sparseCol[col];
                                sCol2 = sparseCol[col + 1];
                                sRow = sparseRow + sCol1;
                                sValue = sparseValue + sCol1;
                                for (i = sCol1; i < sCol2; i++, sRow++, sValue++)
                                {
                                    *cc -= rd * *sValue * x[*sRow];
                                }
                                *cc -= rd * D[col] * x[rows - 1];
                                c[maxj] = -rd;
                            }
                        }

                        //System.Diagnostics.Debug.WriteLine(round + ": " + V[maxj] + " <=> " + B[mini]);
                        // swap base

                        redecompose--;
                        if (redecompose > 0)
                        {
                            lu.Update(ww, mini);
                        }

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
            double* sValue;
            int* sRow;
            int sCol1, sCol2;
            int redecompose = 0;
            fixed (double* a = SparseMatrix.data, LU = _LU, d = _d, x = _x, w = _w, ww = _ww, wd = _wd, c = _c, u = _u, b = _b, cost = _cost, sparseValue = SparseMatrix.value)
            {
                fixed (int* B = _B, V = _V, D = disabled, sparseRow = SparseMatrix.row, sparseCol = SparseMatrix.col)
                {
                    do
                    {
                        if (redecompose <= 0)
                        {
                            // [L U] = lu(A(:,B_indices));
                            for (j = 0; j < rows; j++)
                            {
                                int col = B[j];
                                for (i = 0; i < rows - 1; i++)
                                {
                                    LU[i * rows + j] = a[i * (cols + rows) + col];
                                }
                                LU[i * rows + j] = D[col];
                            }
                            lu.Decompose();
                            redecompose = 50;
                        }
                        if (lu.Singular)
                        {
                            //System.Diagnostics.Debug.WriteLine("Basis singular");
                            // when restricting constraints sometimes the basis becomes singular
                            // restart normal primal problem from scratch
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
                            for (i = 0; i < rows; i++)
                            {
                                d[i] = b[i]; // TODO block copy?
                            }
                            lu.FSolve(d);

                            for (i = 0; i < rows; i++)
                            {
                                if (B[i] < cols) u[i] = cost[B[i]];
                                else u[i] = 0;
                            }
                            lu.BSolve(u);
                            for (j = 0; j < cols; j++)
                            {
                                int col = V[j];

                                double costcol = ((col < cols) ? cost[col] : 0);
                                sCol1 = sparseCol[col];
                                sCol2 = sparseCol[col + 1];
                                sRow = sparseRow + sCol1;
                                sValue = sparseValue + sCol1;
                                for (i = sCol1; i < sCol2; i++, sRow++, sValue++)
                                {
                                    costcol -= *sValue * u[*sRow];
                                }
                                costcol -= D[col] * u[rows - 1];
                                if (costcol > eps)
                                {
                                    // this should only happen if dual is not feasible
                                    // should not come here unless LU is singular
                                    // ...
                                    // maybe it can
                                    // at least we don't have to start from scratch
                                    return SolvePrimal();
                                }
                                c[j] = costcol;
                            }

                            needsRecalc = false;
                        }

                        double mind = -eps;
                        int mini = -1;
                        for (i = 0; i < rows; i++)
                        {
                            double costi = d[i];
                            if (costi < mind)
                            {
                                mind = costi;
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
                                if (B[i] < cols) value += cost[B[i]] * d[i];
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
                        lu.BSolve(x); // TODO exploit nature of x

                        // min over i of d[i]/w[i] where w[i]>0
                        double minr = double.PositiveInfinity;
                        int minj = -1;
                        for (j = 0; j < cols; j++)
                        {
                            wd[j] = 0;
                            int col = V[j];
                            sCol1 = sparseCol[col];
                            sCol2 = sparseCol[col + 1];
                            sRow = sparseRow + sCol1;
                            sValue = sparseValue + sCol1;
                            for (i = sCol1; i < sCol2; i++, sRow++, sValue++)
                            {
                                wd[j] += *sValue * x[*sRow];
                            }
                            wd[j] += D[col] * x[rows - 1];
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
                        for (i = 0; i < rows - 1; i++)
                        {
                            w[i] = a[i * (cols + rows) + mincol];
                        }
                        w[i] = D[mincol];
                        //lu.FSolve(w);
                        lu.FSolveL(w, ww);
                        lu.FSolveU(ww, w);

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

                        redecompose--;
                        if (redecompose > 0)
                        {
                            lu.Update(ww, mini);
                        }

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
    }
}
