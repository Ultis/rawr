using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Mage
{
    public class LP
    {
        private SparseMatrix A;
        private double[] extraConstraints;
        private int numExtraConstraints;
        private int baseRows;
        private int rows;
        private int cols;
        private bool disableRowAdded;

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
            if (numExtraConstraints > 0) clone.extraConstraints = (double[])extraConstraints.Clone();
            return clone;
        }

        public void DisableColumn(int col)
        {
            if (!disableRowAdded)
            {
                disableRowAdded = true;
                AddConstraint();
            }
            extraConstraints[col] = 1;
        }

        public double this[int row, int col]
        {
            get
            {
                if (row > baseRows) throw new ArgumentException();
                else if (row == baseRows) return _cost[col];
                else if (col == cols) return _b[row];
                else return A[row, col];
            }
            set
            {
                if (row > baseRows) throw new ArgumentException();
                else if (row == baseRows) _cost[col] = value;
                else if (col == cols) _b[row] = value;
                else A[row, col] = value;
            }
        }

        public double GetConstraintElement(int col)
        {
            return extraConstraints[(numExtraConstraints - 1) * (cols + rows + 1) + col];
        }

        public void SetConstraintElement(int col, double value)
        {
            extraConstraints[(numExtraConstraints - 1) * (cols + rows + 1) + col] = value;
        }

        public double GetConstraintRHS()
        {
            return extraConstraints[(numExtraConstraints - 1) * (cols + rows + 1) + cols + rows];
        }

        public void SetConstraintRHS(double value)
        {
            extraConstraints[(numExtraConstraints - 1) * (cols + rows + 1) + cols + rows] = value;
        }

        public void AddConstraint()
        {
            if (!disableRowAdded)
            {
                disableRowAdded = true;
                AddConstraint();
            }
            numExtraConstraints++;
            rows++;
            double[] newArray = new double[(cols + rows + 1) * numExtraConstraints];
            for (int i = 0; i < numExtraConstraints - 1; i++)
            {
                Array.Copy(extraConstraints, (cols + rows) * i, newArray, (cols + rows + 1) * i, cols + rows - 1);
                newArray[(cols + rows + 1) * i + cols + rows] = extraConstraints[(cols + rows) * i + cols + rows - 1];
            }
            extraConstraints = newArray;
            extraConstraints[(numExtraConstraints - 1) * (cols + rows + 1) + cols + rows - 1] = 1;
            A.AddColumn();
            A.EndConstruction();
            _cost[cols + rows - 1] = 0;
            int[] newB = new int[rows];
            Array.Copy(_B, newB, rows - 1);
            _B = newB;
            _B[rows - 1] = cols + rows - 1;
            lu = new LU2(_LU, rows);
        }

        private bool constructed;
        public void EndConstruction()
        {
            if (constructed) return;
            for (int i = 0; i < baseRows; i++)
            {
                A[i, cols + i] = 1.0;
            }
            A.EndConstruction();
            constructed = true;
        }

        public LP(int rows, int cols)
        {
            baseRows = rows;
            //rows++; // add extra row for disabled
            if (rows > maxRows || cols > maxCols)
            {
                maxRows = Math.Max(rows, maxRows);
                maxCols = Math.Max(cols, maxCols);
                RecreateArrays();
            }
            this.rows = rows;
            this.cols = cols;

            A = new SparseMatrix(baseRows, cols + rows);
            _B = new int[rows];
            _V = new int[cols];
            lu = new LU2(_LU, rows);
            //extraConstraints = new double[cols + rows + 1];
            //extraConstraints[cols + rows - 1] = 1;
            //numExtraConstraints = 1;
            Array.Clear(_cost, 0, cols + rows);
            Array.Clear(_b, 0, baseRows);

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

            int i, j, k;
            bool feasible = false;
            int round = 0;
            double* cc, sValue;
            int* sRow;
            int sCol1, sCol2;
            int redecompose = 0;
            double lastInfis = double.PositiveInfinity;
            fixed (double* a = SparseMatrix.data, LU = _LU, d = _d, x = _x, w = _w, ww = _ww, c = _c, u = _u, b = _b, cost = _cost, sparseValue = SparseMatrix.value, D = extraConstraints)
            {
                fixed (int* B = _B, V = _V, sparseRow = SparseMatrix.row, sparseCol = SparseMatrix.col)
                {
                    do
                    {
                    RESTART:
                        // [L U] = lu(A(:,B_indices));
                        if (redecompose <= 0)
                        {
                            for (j = 0; j < rows; j++)
                            {
                                int col = B[j];
                                for (i = 0; i < baseRows; i++)
                                {
                                    LU[i * rows + j] = a[i + col * baseRows];
                                }
                                for (k = 0; k < numExtraConstraints; k++, i++)
                                {
                                    LU[i * rows + j] = D[k * (cols + rows + 1) + col];
                                }
                            }
                            lu.Decompose();
                            redecompose = 50; // decompose every 50 iterations to maintain stability
                        }
                        if (lu.Singular)
                        {
                            //System.Diagnostics.Debug.WriteLine("Basis singular");
                            // TODO deal with it
                        }

                        if (!feasible)
                        {
                            // d = U \ (L \ b);
                            for (i = 0; i < baseRows; i++)
                            {
                                d[i] = b[i];
                            }
                            for (k = 0; k < numExtraConstraints; k++, i++)
                            {
                                d[i] = D[k * (cols + rows + 1) + cols + rows];
                            }
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
                                //System.Diagnostics.Debug.WriteLine("Primal feasible in " + round);
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
                                    for (k = 0; k < numExtraConstraints; k++, i++)
                                    {
                                        costcol -= D[k * (cols + rows + 1) + col] * u[baseRows + k];
                                    }
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
                            if (infis > 100.0)
                            {
                                // we're so far out of feasible region we're better off starting from scratch
                                for (i = 0; i < rows; i++)
                                {
                                    B[i] = cols + i;
                                }
                                for (j = 0; j < cols; j++)
                                {
                                    V[j] = j;
                                }
                                redecompose = 0;
                                goto RESTART;
                            }
                            //lastInfis = infis;
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
                                for (k = 0; k < numExtraConstraints; k++, i++)
                                {
                                    costcol -= D[k * (cols + rows + 1) + col] * x[baseRows + k];
                                }
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
                            //System.Diagnostics.Debug.WriteLine("Primal solved in " + round);
                            return ret;
                        }

                        // w = U \ (L \ A(:,j));
                        int maxcol = V[maxj];
                        for (i = 0; i < baseRows; i++)
                        {
                            w[i] = a[i + maxcol * baseRows];
                        }
                        for (k = 0; k < numExtraConstraints; k++, i++)
                        {
                            w[i] = D[k * (cols + rows + 1) + maxcol];
                        }
                        //lu.FSolve(w);
                        lu.FSolveL(w, ww);
                        lu.FSolveU(ww, w);

                        // min over i of d[i]/w[i] where w[i]>0
                        double minr = double.PositiveInfinity;
                        int mini = -1;
                        for (i = 0; i < rows; i++)
                        {
                            if (w[i] > eps && d[i] > -eps) // !!! if variable is currently unfeasible you should let it go more unfeasible as a compromise to get some others into feasible range, if you block on it you actually force entering variable to be negative which means total infeasibilities will increase
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
                                for (k = 0; k < numExtraConstraints; k++, i++)
                                {
                                    *cc -= rd * D[k * (cols + rows + 1) + col] * x[baseRows + k];
                                }
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

                        k = B[mini];
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

            int i, j, k;
            int round = 0;
            bool needsRecalc = true;
            double* sValue;
            int* sRow;
            int sCol1, sCol2;
            int redecompose = 0;
            fixed (double* a = SparseMatrix.data, LU = _LU, d = _d, x = _x, w = _w, ww = _ww, wd = _wd, c = _c, u = _u, b = _b, cost = _cost, sparseValue = SparseMatrix.value, D = extraConstraints)
            {
                fixed (int* B = _B, V = _V, sparseRow = SparseMatrix.row, sparseCol = SparseMatrix.col)
                {
                    do
                    {
                    DECOMPOSE:
                        if (redecompose <= 0)
                        {
                            // [L U] = lu(A(:,B_indices));
                            for (j = 0; j < rows; j++)
                            {
                                int col = B[j];
                                for (i = 0; i < baseRows; i++)
                                {
                                    LU[i * rows + j] = a[i + col * baseRows];
                                }
                                for (k = 0; k < numExtraConstraints; k++, i++)
                                {
                                    LU[i * rows + j] = D[k * (cols + rows + 1) + col];
                                }
                            }
                            lu.Decompose();
                            redecompose = 50;
                        }
                        if (lu.Singular)
                        {
                            //System.Diagnostics.Debug.WriteLine("Basis singular");
                            // when restricting constraints sometimes the basis becomes singular
                            // try to patch the basis by replacing singular columns with corresponding slacks of singular rows
                            redecompose = 0;
                            for (j = lu.Rank; j < rows; j++)
                            {
                                int singularColumn = LU2._Q[j];
                                int singularRow = LU2._P[j];
                                int slackColumn = cols + singularRow;
                                int vindex = Array.IndexOf(_V, slackColumn);
                                V[vindex] = B[singularColumn];
                                B[singularColumn] = slackColumn;
                            }
                            goto DECOMPOSE;
                            /*for (i = 0; i < rows; i++)
                            {
                                B[i] = cols + i;
                            }
                            for (j = 0; j < cols; j++)
                            {
                                V[j] = j;
                            }
                            return SolvePrimal();*/
                        }

                        if (needsRecalc)
                        {
                            // d = U \ (L \ b);
                            for (i = 0; i < baseRows; i++)
                            {
                                d[i] = b[i]; // TODO block copy?
                            }
                            for (k = 0; k < numExtraConstraints; k++, i++)
                            {
                                d[i] = D[k * (cols + rows + 1) + cols + rows];
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
                                for (k = 0; k < numExtraConstraints; k++, i++)
                                {
                                    costcol -= D[k * (cols + rows + 1) + col] * u[baseRows + k];
                                }
                                if (costcol > eps)
                                {
                                    // this should only happen if dual is not feasible
                                    // should come here only if LU is singular
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
                            for (k = 0; k < numExtraConstraints; k++, i++)
                            {
                                wd[j] += D[k * (cols + rows + 1) + col] * x[baseRows + k];
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
                            // unfeasible, return null solution, don't pursue this branch because no solution exists here
                            double[] ret = new double[cols + 1];
                            //System.Diagnostics.Debug.WriteLine("Dual unfeasible after " + round);
                            return ret;
                        }

                        // w = U \ (L \ A(:,j));
                        int mincol = V[minj];
                        for (i = 0; i < baseRows; i++)
                        {
                            w[i] = a[i + mincol * baseRows];
                        }
                        for (k = 0; k < numExtraConstraints; k++, i++)
                        {
                            w[i] = D[k * (cols + rows + 1) + mincol];
                        }
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
                        k = B[mini];
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

        private static double[] LPSolve(double[,] data, int rows, int cols)
        {
            double[,] a = data;
            int[] XN;
            int[] XB;

            bool feasible;
            int i, j, r, c, t;
            double v, bestv;

            bestv = 0;
            c = 0;
            r = 0;

            XN = new int[cols];
            XB = new int[rows];

            for (i = 0; i < rows; i++)
                XB[i] = cols + i;
            for (j = 0; j < cols; j++)
                XN[j] = j;

            int round = 0;

            do
            {
                feasible = true;
                // check feasibility
                for (i = 0; i < rows; i++)
                {
                    if (a[i, cols] < 0)
                    {
                        feasible = false;
                        bestv = 0;
                        for (j = 0; j < cols; j++)
                        {
                            if (a[i, j] < bestv)
                            {
                                bestv = a[i, j];
                                c = j;
                            }
                        }
                        break;
                    }
                }
                if (feasible)
                {
                    // standard problem
                    bestv = 0;
                    for (j = 0; j < cols; j++)
                    {
                        if (a[rows, j] > bestv)
                        {
                            bestv = a[rows, j];
                            c = j;
                        }
                    }
                }
                if (bestv == 0) break;
                bestv = -1;
                for (i = 0; i < rows; i++)
                {
                    if (a[i, c] > 0)
                    {
                        v = a[i, cols] / a[i, c];
                        if (bestv == -1 || v < bestv)
                        {
                            bestv = v;
                            r = i;
                        }
                    }
                }
                if (bestv == -1) break;
                v = a[r, c];
                a[r, c] = 1;
                for (j = 0; j <= cols; j++)
                {
                    a[r, j] = a[r, j] / v;
                }
                for (i = 0; i <= rows; i++)
                {
                    if (i != r)
                    {
                        v = a[i, c];
                        a[i, c] = 0;
                        for (j = 0; j <= cols; j++)
                        {
                            a[i, j] = a[i, j] - a[r, j] * v;
                            if (a[i, j] < 0.00000000001 && a[i, j] > -0.00000000001) a[i, j] = 0; // compensate for floating point errors
                        }
                    }
                }
                t = XN[c];
                XN[c] = XB[r];
                XB[r] = t;
                round++;
            } while (round < 5000); // fail safe for infinite loops caused by floating point instability

            double[] ret = new double[cols + 1];
            for (i = 0; i < rows; i++)
            {
                if (XB[i] < cols) ret[XB[i]] = a[i, cols];
            }
            ret[cols] = -a[rows, cols];

            return ret;
        }

        private static unsafe double[] LPSolveUnsafe(double[,] data, int rows, int cols)
        {
            double[] ret = new double[cols + 1];
            int[] xn = new int[cols + 1];
            int[] xb = new int[rows + 1];
            if (cols > 30000) return ret; // prevent unstable solutions

            double* ai, aij, arows;

            fixed (double* a = data)
            {
                fixed (int* XN = xn, XB = xb)
                {
                    arows = a + rows * (cols + 1);

                    bool feasible;
                    int i, j, r, c, t;
                    double v, bestv;

                    bestv = 0;
                    c = 0;
                    r = 0;

                    for (i = 0; i < rows; i++)
                        XB[i] = cols + i;
                    for (j = 0; j < cols; j++)
                        XN[j] = j;

                    int round = 0;

                    do
                    {
                        feasible = true;
                        // check feasibility
                        for (i = 0, ai = a; i < rows; i++, ai += (cols + 1))
                        {
                            if (ai[cols] < 0)
                            {
                                feasible = false;
                                bestv = 0;
                                for (j = 0, aij = ai; j < cols; j++, aij++)
                                {
                                    if (*aij < bestv)
                                    {
                                        bestv = *aij;
                                        c = j;
                                    }
                                }
                                break;
                            }
                        }
                        if (feasible)
                        {
                            // standard problem
                            bestv = 0;
                            for (j = 0, aij = arows; j < cols; j++, aij++)
                            {
                                if (*aij > bestv)
                                {
                                    bestv = *aij;
                                    c = j;
                                }
                            }
                        }
                        if (bestv == 0) break;
                        bestv = -1;
                        for (i = 0, ai = a; i < rows; i++, ai += (cols + 1))
                        {
                            if (ai[c] > 0)
                            {
                                v = ai[cols] / ai[c];
                                if (bestv == -1 || v < bestv)
                                {
                                    if (ai[c] > 0.0000000001)
                                    {
                                        bestv = v;
                                        r = i;
                                    }
                                    else
                                    {
                                        ai[c] = 0;
                                    }
                                }
                            }
                        }
                        if (bestv == -1) break;
                        aij = a + r * (cols + 1) + c;
                        v = *aij;
                        *aij = 1;
                        ai = a + r * (cols + 1);
                        for (j = 0, aij = ai; j <= cols; j++, aij++)
                        {
                            *aij /= v;
                        }
                        for (i = 0, ai = a; i <= rows; i++, ai += (cols + 1))
                        {
                            if (i != r)
                            {
                                v = ai[c];
                                ai[c] = 0;
                                for (j = 0, aij = ai; j <= cols; j++, aij++)
                                {
                                    *aij -= a[r * (cols + 1) + j] * v;
                                    if (*aij < 0.00000000001 && *aij > -0.00000000001) *aij = 0; // compensate for floating point errors
                                }
                            }
                        }
                        //System.Diagnostics.Debug.WriteLine(round + ": " + XN[c] + " <=> " + XB[r]);
                        t = XN[c];
                        XN[c] = XB[r];
                        XB[r] = t;
                        round++;
                        if (round == 5000) round++;
                    } while (round < 5000); // fail safe for infinite loops caused by floating point instability

                    for (i = 0; i < rows; i++)
                    {
                        if (XB[i] < cols) ret[XB[i]] = a[i * (cols + 1) + cols];
                    }
                    ret[cols] = -a[rows * (cols + 1) + cols];
                }
            }
            return ret;
        }
    }
}
