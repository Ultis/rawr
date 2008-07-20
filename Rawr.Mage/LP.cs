using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Mage
{
    public unsafe class LP
    {
        internal SparseMatrix A;
        private double[] extraConstraints;
        private List<bool> extraConstraintEditable;
        private int numExtraConstraints;
        private int baseRows;
        private int rows;
        internal int cols;

        private int[] _B;
        private int[] _V;
        LU lu;

        private static double[] _d;
        private static double[] _x;
        private static double[] _w;
        private static double[] _ww;
        private static double[] _wd;
        private static double[] _u;
        private static double[] _c;
        internal static double[] _b;
        internal static double[] _cost;
        private static bool[] _blacklist;

        private double* a;
        private double* U;
        private double* d;
        private double* x;
        private double* w;
        private double* ww;
        private double* wd;
        private double* c;
        private double* u;
        private double* b;
        private double* cost;
        private double* sparseValue;
        private double* D;
        private int* B;
        private int* V;
        private int* sparseRow;
        private int* sparseCol;
        private bool* blacklist;

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
            _d = new double[maxRows];
            _x = new double[maxRows];
            _w = new double[maxRows];
            _ww = new double[maxRows];
            _wd = new double[maxCols];
            _u = new double[maxRows];
            _c = new double[maxCols];
            _cost = new double[maxCols];
            _b = new double[maxRows];
            _blacklist = new bool[maxRows + maxCols];
        }


        public LP Clone()
        {
            LP clone = (LP)MemberwiseClone();
            clone._B = (int[])_B.Clone();
            clone._V = (int[])_V.Clone();
            if (numExtraConstraints > 0)
            {
                clone.extraConstraints = (double[])extraConstraints.Clone();
                clone.extraConstraintEditable = new List<bool>(extraConstraintEditable);
            }
            return clone;
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
                A[row, col] = value;
            }
        }

        public void SetCost(int col, double value)
        {
            _cost[col] = value;
        }

        public void SetRHS(int row, double value)
        {
            _b[row] = value;
        }

        public double GetConstraintElement(int index, int col)
        {
            return extraConstraints[index * (cols + rows + 1) + col];
        }

        public void SetConstraintElement(int index, int col, double value)
        {
            extraConstraints[index * (cols + rows + 1) + col] = value;
        }

        public double GetConstraintRHS(int index)
        {
            return extraConstraints[index * (cols + rows + 1) + cols + rows];
        }

        public void SetConstraintRHS(int index, double value)
        {
            extraConstraints[index * (cols + rows + 1) + cols + rows] = value;
        }

        public int ExtraConstraintCount
        {
            get
            {
                return numExtraConstraints;
            }
        }

        public int AddColumn()
        {
            _cost[cols] = 0.0;
            cols++;
            return A.AddColumn();
        }

        private int disablingConstraint = -1;

        public int AddDisablingConstraint()
        {
            if (disablingConstraint != -1) return disablingConstraint;
            disablingConstraint = AddConstraint(true);
            return disablingConstraint;
        }

        // should only add extra constraints that are tight when primal feasible
        public int AddConstraint(bool editable)
        {
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
            if (extraConstraintEditable == null) extraConstraintEditable = new List<bool>();
            extraConstraintEditable.Add(editable);
            A.AddColumn();
            A.EndConstruction();
            int[] newB = new int[rows];
            Array.Copy(_B, newB, rows - 1);
            _B = newB;
            //_B[rows - 1] = cols + rows - 1;
            // extra constraints should be at start so they are not eliminated when patching singular basis
            _B[rows - 1] = _B[numExtraConstraints - 1];
            _B[numExtraConstraints - 1] = cols + rows - 1;
            lu = new LU(rows);
            return numExtraConstraints - 1;
        }

        private bool constructed;
        public void EndConstruction()
        {
            if (constructed) return;
            for (int i = 0; i < baseRows; i++)
            {
                A.AddColumn();
                A[i, cols + i] = 1.0;
            }
            A.EndConstruction();
            _B = new int[rows];
            _V = new int[cols];
            for (int i = 0; i < rows; i++)
            {
                _B[i] = cols + i;
            }
            for (int j = 0; j < cols; j++)
            {
                _V[j] = j;
            }
            constructed = true;
        }

        public LP(int baseRows, int maxCols)
        {
            this.baseRows = baseRows;
            if (baseRows + 10 > maxRows || maxCols + 10 > LP.maxCols)
            {
                maxRows = Math.Max(baseRows + 10, maxRows);
                LP.maxCols = Math.Max(maxCols + 10, LP.maxCols);
                RecreateArrays();
            }
            this.rows = baseRows;
            this.cols = 0;

            A = new SparseMatrix(baseRows, maxCols + baseRows);
            lu = new LU(rows);
            //extraConstraints = new double[cols + rows + 1];
            //extraConstraints[cols + rows - 1] = 1;
            //numExtraConstraints = 1;
            Array.Clear(_b, 0, baseRows);

            ColdStart = true;
        }

        private bool ColdStart;

        public unsafe double[] SolvePrimal()
        {
            double[] ret = null;

            fixed (double* a = SparseMatrix.data, U = LU._U, d = _d, x = _x, w = _w, ww = _ww, wd = _wd, c = _c, u = _u, b = _b, cost = _cost, sparseValue = SparseMatrix.value, D = extraConstraints)
            fixed (int* B = _B, V = _V, sparseRow = SparseMatrix.row, sparseCol = SparseMatrix.col)
            fixed (bool* blacklist = _blacklist)
            {
                this.a = a;
                this.U = U;
                this.d = d;
                this.x = x;
                this.w = w;
                this.ww = ww;
                this.wd = wd;
                this.c = c;
                this.u = u;
                this.b = b;
                this.cost = cost;
                this.sparseValue = sparseValue;
                this.D = D;
                this.B = B;
                this.V = V;
                this.sparseRow = sparseRow;
                this.sparseCol = sparseCol;
                this.blacklist = blacklist;

                ret = SolvePrimalUnsafe(false);

                this.a = null;
                this.U = null;
                this.d = null;
                this.x = null;
                this.w = null;
                this.ww = null;
                this.wd = null;
                this.c = null;
                this.u = null;
                this.b = null;
                this.cost = null;
                this.sparseValue = null;
                this.D = null;
                this.B = null;
                this.V = null;
                this.sparseRow = null;
                this.sparseCol = null;
                this.blacklist = null;
            }

            return ret;
        }

        private unsafe double[] SolvePrimalUnsafe(bool dualFeasibilityOnly)
        {
            // c = data[rows,:]
            // A = data[0:rows-1,0:cols-1][I(rows)]
            // b = data[:,cols]
            // B = [cols:cols+rows-1]

            // LU = A_B
            // d = x_B <- A_B^-1*b ... primal solution
            // u = u <- c_B*A_B^-1 ... dual solution
            // w_N <- c_N - u*A_N  ... dual solution

            //  eps1 = 10-5, eps2 = 10-8, and eps3 = 10-6
            const double eps = 0.00001;

            int i, j, k;
            bool feasible = false;
            int round = 0;
            double* cc, sValue;
            int* sRow;
            int sCol1, sCol2;
            int redecompose = 0;
            //double lastInfis = double.PositiveInfinity;
            const int maxRedecompose = 50;
            Array.Clear(_blacklist, 0, rows + cols);
            {
                if (b == null) throw new InvalidOperationException();
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
                                U[i * rows + j] = a[i + col * baseRows];
                            }
                            for (k = 0; k < numExtraConstraints; k++, i++)
                            {
                                U[i * rows + j] = D[k * (cols + rows + 1) + col];
                            }
                        }
                        lu.Decompose();
                        redecompose = maxRedecompose; // decompose every 50 iterations to maintain stability
                        //feasible = false; // when refactoring basis recompute the solution
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
                        if (dualFeasibilityOnly) feasible = true; // if we don't care for primal feasibility then just fake it
                        if (feasible)
                        {
                            //System.Diagnostics.Debug.WriteLine("Primal feasible in " + round);
                            // we have a feasible solution, initialize phase 2
                            for (i = 0; i < rows; i++)
                            {
                                if (B[i] < cols) u[i] = cost[B[i]];
                                else u[i] = 0.0;
                            }
                            lu.BSolve(u);
                            for (j = 0; j < cols; j++)
                            {
                                int col = V[j];

                                double costcol = ((col < cols) ? cost[col] : 0.0);
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
                        double infis = 0.0;
                        for (i = 0; i < rows; i++)
                        {
                            if (d[i] < -eps)
                            {
                                x[i] = 1.0;
                                infis -= d[i];
                            }
                            else x[i] = 0.0;
                        }
                        if (infis > 100.0 && !ColdStart)
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
                            ColdStart = true;
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

                    double maxc = 0.0;
                    int maxj = -1;
                    for (j = 0; j < cols; j++)
                    {
                        double costj = c[j];
                        // just ignore the disabled columns, because they can't be in the true solution
                        // and if we allowed it in it would force the extra constraint out
                        if (costj > maxc + eps && !blacklist[V[j]] && (disablingConstraint == -1 || V[j] >= cols || D == null || D[disablingConstraint * (cols + rows + 1) + V[j]] == 0.0)) // add eps barriers so that we stabilize pivot order
                        {
                            maxc = costj;
                            maxj = j;
                        }
                    }

                    if (maxj == -1)
                    {
                        if (dualFeasibilityOnly) return null;
                        // rebuild solution so it's stable
                        /*for (i = 0; i < baseRows; i++)
                        {
                            d[i] = b[i];
                        }
                        for (k = 0; k < numExtraConstraints; k++, i++)
                        {
                            d[i] = D[k * (cols + rows + 1) + cols + rows];
                        }
                        lu.FSolve(d);*/

                        // optimum, return solution (or could be no feasible solution)
                        // solution(B_indices,:) = d;
                        double[] ret = new double[cols + 1];
                        if (!feasible) return ret; // if it's not feasible then return null solution
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
                        ColdStart = false;
                        //System.Diagnostics.Trace.WriteLine("Primal solved in " + round);
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
                    double maxnorm = 0.0;
                    int mini = -1;
                    for (i = 0; i < rows; i++)
                    {
                        double v = Math.Abs(w[i]);
                        if (v > maxnorm) maxnorm = v;
                        if (w[i] > eps && d[i] > -eps) // !!! if variable is currently unfeasible you should let it go more unfeasible as a compromise to get some others into feasible range, if you block on it you actually force entering variable to be negative which means total infeasibilities will increase
                        {
                            double r = d[i] / w[i];
                            if (r < minr - eps) // add eps barriers so that we stabilize pivot order
                            {
                                minr = r;
                                mini = i;
                            }
                        }
                    }

                    if (mini == -1)
                    {
                        if (dualFeasibilityOnly) return null; // we need primal feasibility to continue
                        // unbounded
                        if (!ColdStart)
                        {
                            // something went really horrible wrong
                            // when you have code that runs normally when debugger is attached and throws exceptions otherwise you have to employ extreme measures
                            // so somehow there is something very horribly unstable with this basis
                            // because by design the formulated LP can't be primary unbounded
                            // so take a deep breath and try from scratch
                            for (i = 0; i < rows; i++)
                            {
                                B[i] = cols + i;
                            }
                            for (j = 0; j < cols; j++)
                            {
                                V[j] = j;
                            }
                            redecompose = 0;
                            ColdStart = true;
                            goto RESTART;
                        }
                        // ok, so going from scratch didn't help ah
                        // of course you knew this was going to happen
                        // oh well, give up and collect bug reports
                        //throw new InvalidOperationException();

                        // then again, we don't like those bug reports
                        // this is a very strange situation, so just say that solution is crap
                        // it'll find something that works hopefully
                        double[] ret = new double[cols + 1];
                        return ret;
                    }

                    // bah probably not that bad, just restore extra constraints into basis after primal returns
                    /*if (B[mini] >= cols + baseRows)
                    {
                        System.Diagnostics.Debug.WriteLine("Trying to remove extra constraint from basis!!! Investigate and prevent this from happening!!!");
                    }*/

                    // pivot stability
                    double pivotStability = Math.Abs(w[mini]) / maxnorm;
                    //System.Diagnostics.Trace.WriteLine("Pivot stability " + pivotStability);
                    if (pivotStability < 0.0001)
                    {
                        if (redecompose == maxRedecompose && pivotStability < 0.000001)
                        {
                            // basis is stable so it must be something with the pivot
                            // blacklist it
                            blacklist[V[maxj]] = true;
                            continue;
                        }
                        else if (redecompose == maxRedecompose)
                        {
                            // we just refactored and it's probably not horrible bad
                        }
                        else
                        {
                            // try recalculating basis and variables
                            redecompose = 0;
                            feasible = false;
                            continue;
                        }
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
                        if (lu.Singular)
                        {
                            if (redecompose == maxRedecompose - 1)
                            {
                                redecompose = 0;
                                feasible = false; // forces recalc
                                blacklist[V[maxj]] = true;
                                continue;
                            }
                            else
                            {
                                redecompose = 0;
                                feasible = false;
                                continue;
                            }
                        }
                    }

                    k = B[mini];
                    B[mini] = V[maxj];
                    V[maxj] = k;

                    round++;
                    if (round == 5000) round++;
                } while (round < 5000);
            }                            
            // just in case
            return new double[cols + 1];
        }

        public unsafe double[] SolveDual()
        {
            double[] ret = null;

            fixed (double* a = SparseMatrix.data, U = LU._U, d = _d, x = _x, w = _w, ww = _ww, wd = _wd, c = _c, u = _u, b = _b, cost = _cost, sparseValue = SparseMatrix.value, D = extraConstraints)
            fixed (int* B = _B, V = _V, sparseRow = SparseMatrix.row, sparseCol = SparseMatrix.col)
            fixed (bool* blacklist = _blacklist)
            {
                this.a = a;
                this.U = U;
                this.d = d;
                this.x = x;
                this.w = w;
                this.ww = ww;
                this.wd = wd;
                this.c = c;
                this.u = u;
                this.b = b;
                this.cost = cost;
                this.sparseValue = sparseValue;
                this.D = D;
                this.B = B;
                this.V = V;
                this.sparseRow = sparseRow;
                this.sparseCol = sparseCol;
                this.blacklist = blacklist;

                ret = SolveDualUnsafe();

                this.a = null;
                this.U = null;
                this.d = null;
                this.x = null;
                this.w = null;
                this.ww = null;
                this.wd = null;
                this.c = null;
                this.u = null;
                this.b = null;
                this.cost = null;
                this.sparseValue = null;
                this.D = null;
                this.B = null;
                this.V = null;
                this.sparseRow = null;
                this.sparseCol = null;
                this.blacklist = null;
            }

            return ret;
        }

        public unsafe double[] SolveDualUnsafe()
        {
            // c = data[rows,:]
            // A = data[0:rows-1,0:cols-1][I(rows)]
            // b = data[:,cols]
            // B = [cols:cols+rows-1]

            // LU = A_B
            // d = x_B <- A_B^-1*b ... primal solution
            // u = u <- c_B*A_B^-1 ... dual solution
            // w_N <- c_N - u*A_N  ... dual solution

            //  eps1 = 10-5, eps2 = 10-8, and eps3 = 10-6
            const double eps = 0.00001;

            int i, j, k;
            int round = 0;
            bool needsRecalc = true;
            double* sValue;
            int* sRow, sRow2;
            int sCol1, sCol2;
            double* cj, ccols, wdj;
            int* Vj;
            int redecompose = 0;
            const int maxRedecompose = 50;
            Array.Clear(_blacklist, 0, rows + cols);

            bool primalPatched = false;
            bool blacklistAllowed = true;

            {
                ccols = c + cols;
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
                                U[i * rows + j] = a[i + col * baseRows];
                            }
                            for (k = 0; k < numExtraConstraints; k++, i++)
                            {
                                U[i * rows + j] = D[k * (cols + rows + 1) + col];
                            }
                        }
                        lu.Decompose();
                        redecompose = maxRedecompose; // decompose each time to see where the instability comes from
                        needsRecalc = true; // for long dual phases refresh the reduced costs
                    }

                    if (lu.Singular)
                    {
                        // try to patch the basis by replacing singular columns with corresponding slacks of singular rows
                        redecompose = 0;
                        for (j = lu.Rank; j < rows; j++)
                        {
                            int singularColumn = LU._Q[j];
                            int singularRow = LU._P[j];
                            int slackColumn = cols + singularRow;
                            int vindex = Array.IndexOf(_V, slackColumn);
                            V[vindex] = B[singularColumn];
                            B[singularColumn] = slackColumn;
                        }
                        goto DECOMPOSE;
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
                            if (costcol > eps && (disablingConstraint == -1 || V[j] >= cols || D == null || D[disablingConstraint * (cols + rows + 1) + V[j]] == 0.0))
                            {
                                if (primalPatched)
                                {
                                    // this can happen as result of swapping the basis which while retains primal feasibility and optimality does not necessarily retain dual feasibility
                                    ColdStart = false;
                                    double[] ret = SolvePrimalUnsafe(false);
                                    // after we have result of primal make sure we return extra constraints to basis
                                    for (k = 0; k < numExtraConstraints; k++)
                                    {
                                        if (extraConstraintEditable[k])
                                        {
                                            int vindex = Array.IndexOf(_V, cols + baseRows + k);
                                            if (vindex >= 0)
                                            {
                                                for (i = 0; i < rows; i++)
                                                {
                                                    x[i] = ((i == baseRows + k) ? 1 : 0);
                                                }
                                                lu.FSolveL(x, w);
                                                lu.FSolveU(w, x);
                                                //double cvindex = 0.0;
                                                //for (i = 0; i < rows; i++)
                                                //{
                                                //    if (B[i] < cols) cvindex -= cost[B[i]] * x[i];
                                                //}
                                                for (j = 0; j < rows; j++)
                                                {
                                                    if (Math.Abs(x[j]) > eps && d[j] <= eps && B[j] < cols + baseRows)
                                                    {
                                                        // do the swap
                                                        V[vindex] = B[j];
                                                        B[j] = cols + baseRows + k;
                                                        // if we'll do more swaps then update the basis
                                                        if (k < numExtraConstraints - 1)
                                                        {
                                                            lu.Update(w, j);
                                                        }
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    return ret;
                                }
                                else
                                {
                                    ColdStart = false;
                                    SolvePrimalUnsafe(true);
                                    redecompose = 0;
                                    primalPatched = true;
                                    goto DECOMPOSE;
                                }
                            }
                            c[j] = costcol;
                        }

                        needsRecalc = false;
                    }

                    double mind = 0.0;
                    int mini = -1;
                    for (i = 0; i < rows; i++)
                    {
                        double costi = d[i];
                        if (costi < mind - eps) // add eps barriers so that we stabilize pivot order
                        {
                            mind = costi;
                            mini = i;
                        }
                    }

                    /*double vvv = 0;
                    for (i = 0; i < rows; i++)
                    {
                        if (B[i] < cols) vvv += cost[B[i]] * d[i];
                    }*/

                    if (mini == -1)
                    {
                        // refine solution
                        for (i = 0; i < baseRows; i++)
                        {
                            d[i] = b[i]; // TODO block copy?
                        }
                        for (k = 0; k < numExtraConstraints; k++, i++)
                        {
                            d[i] = D[k * (cols + rows + 1) + cols + rows];
                        }
                        lu.FSolve(d);
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
                        ColdStart = false;

                        //System.Diagnostics.Debug.WriteLine("Selecting a non-extended subbasis that is non-singular.");
                        for (k = 0; k < numExtraConstraints; k++)
                        {
                            if (extraConstraintEditable[k])
                            {
                                // we have to compute this anyway and makes a nice test to be 100% sure we're not already in basis
                                int vindex = Array.IndexOf(_V, cols + baseRows + k);
                                if (vindex >= 0)
                                {
                                    //B = [b1 b2 b3 ... bn]
                                    //B x = bx

                                    //now replace column j of B with bx
                                    //B~ = B + (bx - B * ej) * ej'

                                    //let's assume that B~ is linearly dependent
                                    //then there exists w != 0 such that B~ w = 0

                                    //B~ w = (B + (bx - B * ej) * ej') w = B w + (bx - B * ej) * (ej' * w) =
                                    //     = B w + (bx - B * ej) * w_j = 0
                                    //B (w + (x - ej) * w_j) = 0

                                    //because B is linearly independent all components have to be 0

                                    //w_i + x_i * w_j = 0  for all i != j
                                    //w_j + (x_j - 1) * w_j = 0
                                    //w_j + (x_j - 1) * w_j = w_j + x_j * w_j - w_j = x_j * w_j = 0

                                    //if w_j = 0 then w_i = 0 for all i != j -><-
                                    //so w_j must be != 0
                                    //this means that x_j = 0

                                    //if x_j != 0 then replacing column j in B with bx creates a linearly independent system

                                    // so using this brilliant math lets compute x

                                    for (i = 0; i < rows; i++)
                                    {
                                        x[i] = ((i == baseRows + k) ? 1 : 0);
                                    }
                                    lu.FSolveL(x, w);
                                    lu.FSolveU(w, x);

                                    /*System.Diagnostics.Trace.WriteLine("Pivot options:");
                                    for (j = 0; j < rows; j++)
                                    {
                                        if (Math.Abs(x[j]) > eps)
                                        {
                                            System.Diagnostics.Trace.WriteLine("B[" + j + "]=" + B[j] + ", d=" + d[j] + " orientation=" + x[j]);
                                        }
                                    }
                                    System.Diagnostics.Trace.WriteLine("Entering dual variable " + c[vindex]);*/

                                    for (j = 0; j < rows; j++)
                                    {
                                        // we want to leave the dual solution unchanged
                                        // right now j is primal basic, therefore dual non-basic
                                        // so the value of dual solution associated with it is 0
                                        if (Math.Abs(x[j]) > eps && d[j] <= eps && B[j] < cols + baseRows)
                                        {
                                            // check numerical stability
                                            /*for (i = 0; i < rows; i++)
                                            {
                                                x[i] = ((i == j) ? 1 : 0);
                                            }
                                            lu.BSolve(x); // TODO exploit nature of x

                                            // min over i of d[i]/w[i] where w[i]>0
                                            double maxnorm2 = 0.0;
                                            for (int jj = 0; jj < cols; jj++)
                                            {
                                                wd[jj] = 0;
                                                int col = V[jj];
                                                sCol1 = sparseCol[col];
                                                sCol2 = sparseCol[col + 1];
                                                sRow = sparseRow + sCol1;
                                                sValue = sparseValue + sCol1;
                                                for (i = sCol1; i < sCol2; i++, sRow++, sValue++)
                                                {
                                                    wd[jj] += *sValue * x[*sRow];
                                                }
                                                for (int kk = 0; kk < numExtraConstraints; kk++, i++)
                                                {
                                                    wd[jj] += D[kk * (cols + rows + 1) + col] * x[baseRows + kk];
                                                }
                                                double v = Math.Abs(wd[jj]);
                                                if (v > maxnorm2) maxnorm2 = v;
                                            }

                                            // pivot stability
                                            System.Diagnostics.Trace.WriteLine("Pivot stability " + Math.Abs(wd[vindex]) / maxnorm2);*/


                                            // do the swap
                                            //System.Diagnostics.Trace.WriteLine("Pivoting on B[" + j + "]=" + B[j] + " x[j]=" + x[j]);
                                            V[vindex] = B[j];
                                            B[j] = cols + baseRows + k;
                                            // d[j] doesn't change 0 => 0
                                            // if we'll do more swaps then update the basis
                                            if (k < numExtraConstraints - 1)
                                            {
                                                lu.Update(w, j);
                                            }
                                            break;
                                        }
                                    }
                                    if (j == rows)
                                    {
                                        // now this is critical, I think it can happen, but not sure exactly under what conditions
                                        // I think the nature of extra constraints that we're using should prevent this from happening
                                        // but you never know unless you prove it
                                        redecompose = 0;
                                    }
                                    else
                                    {
                                        // verify we're still dual feasible
                                        // testing so far shows that if we're not dual feasible it is because
                                        // of problems in factorization
                                        // refactoring the basis produced the correct result
                                        // but in some cases even refactoring doesn't help
                                        // investigate if basis close to singular or unstable pivot
                                        /*for (j = 0; j < rows; j++)
                                        {
                                            int col = B[j];
                                            for (i = 0; i < baseRows; i++)
                                            {
                                                LU[i * rows + j] = a[i + col * baseRows];
                                            }
                                            for (int kk = 0; kk < numExtraConstraints; kk++, i++)
                                            {
                                                LU[i * rows + j] = D[kk * (cols + rows + 1) + col];
                                            }
                                        }
                                        lu.Decompose();
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
                                            for (int kk = 0; kk < numExtraConstraints; kk++, i++)
                                            {
                                                costcol -= D[kk * (cols + rows + 1) + col] * u[baseRows + kk];
                                            }
                                            if (costcol > eps)
                                            {
                                                ColdStart = false;
                                            }
                                        }*/
                                    }
                                }
                            }
                        }

                        //System.Diagnostics.Trace.WriteLine("Dual solved in " + round);
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
                    //double altminr = double.NegativeInfinity;
                    int minj = -1;
                    //int altminj = -1;
                    double maxnorm = 0.0;
                    for (cj = c, wdj = wd, Vj = V, j = 0; j < cols; j++, cj++, wdj++, Vj++)
                    {
                        int col = *Vj;
                        if (col < cols)
                        {
                            *wdj = 0;
                            sCol1 = sparseCol[col];
                            sCol2 = sparseCol[col + 1];
                            sRow = sparseRow + sCol1;
                            sRow2 = sparseRow + sCol2;
                            sValue = sparseValue + sCol1;
                            for (; sRow < sRow2; sRow++, sValue++)
                            {
                                *wdj += *sValue * x[*sRow];
                            }
                            for (k = 0; k < numExtraConstraints; k++)
                            {
                                *wdj += D[k * (cols + rows + 1) + col] * x[baseRows + k];
                            }
                        }
                        else
                        {
                            *wdj = x[col - cols];
                        }
                        double v = Math.Abs(*wdj);
                        if (v > maxnorm) maxnorm = v;
                        if (*wdj < -eps && *cj < eps && !blacklist[col])
                        {
                            // verify the new c[j] will not go above eps
                            // c[j] - minr * wd[j] > eps
                            if (*cj - minr * *wdj > 0.000001) // add eps barriers so that we stabilize pivot order??? what when c gets positive
                            {
                                minr = *cj / *wdj;
                                minj = j;
                            }
                        }
                        /*else if (wd[j] > eps)
                        {
                            if (c[j] - altminr * wd[j] > eps)
                            {
                                altminr = c[j] / wd[j];
                                altminj = j;
                            }
                        }*/
                    }
                    /*if (minr < altminr)
                    {
                        minr = altminr;
                        minj = altminj;
                    }*/

                    if (minj == -1)
                    {
                        // if we blacklisted some variables give it another try
                        if (blacklistAllowed)
                        {
                            bool retry = false;
                            for (i = 0; i < cols + rows; i++)
                            {
                                if (blacklist[i])
                                {
                                    blacklist[i] = false;
                                    retry = true;
                                }
                            }
                            if (retry)
                            {
                                blacklistAllowed = false;
                                redecompose = 0;
                                needsRecalc = true;
                                continue;
                            }
                        }
                        // unfeasible, return null solution, don't pursue this branch because no solution exists here
                        double[] ret = new double[cols + 1];
                        //System.Diagnostics.Debug.WriteLine("Dual unfeasible after " + round);
                        return ret;
                    }

                    // pivot stability
                    double pivotStability = Math.Abs(wd[minj]) / maxnorm;
                    //System.Diagnostics.Trace.WriteLine("Pivot stability " + Math.Abs(wd[minj]) / maxnorm);
                    if (pivotStability < 0.0001)
                    {
                        if (redecompose == maxRedecompose)
                        {
                            // we just refactored, so not much else we can do right now
                            // if pivot actually fails we'll blacklist it
                        }
                        else
                        {
                            // try recalculating basis and variables
                            redecompose = 0;
                            needsRecalc = true;
                            continue;
                        }
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

                    //System.Diagnostics.Trace.WriteLine(w[mini] + " = " + wd[minj]);

                    double rp = d[mini] / wd[minj];
                    if (rp > 1000.0) needsRecalc = true; // if we're making so big jumps in primal space then we have to be a lot more careful with stability

                    // update primal and dual
                    for (i = 0; i < rows; i++)
                    {
                        d[i] -= rp * w[i];
                        //u[i] -= minr * x[i];
                    }
                    d[mini] = rp;
                    for (cj = c, wdj = wd; cj < ccols; cj++, wdj++)
                    {
                        *cj -= minr * *wdj;
                        //if (c[j] > eps && (disablingConstraint == -1 || D[disablingConstraint * (cols + rows + 1) + V[j]] == 0.0)) c[j] += 0.0;
                    }
                    c[minj] = -minr;

                    redecompose--;
                    if (redecompose > 0)
                    {
                        lu.Update(ww, mini);
                        if (lu.Singular)
                        {
                            if (redecompose == maxRedecompose - 1)
                            {
                                // we just refactored so the factorization is the best we can get
                                // and shows that previous basis was not singular (could be close to it though)
                                // the fact that we got a singular basis shows that
                                // e_mini A_B^-1 A_N_minj == 0
                                // this however is equivalent to wd[minj] == 0
                                // this means that in the ratio test we picked an element that looked like != 0, but wasn't
                                // due to ill conditioning
                                // therefore we can blacklist it
                                if (blacklistAllowed) // otherwise we'll make the swap and patch singularity in next phase, beware of oscillation
                                {
                                    blacklist[V[minj]] = true;
                                    redecompose = 0;
                                    needsRecalc = true;
                                    continue;
                                }
                            }
                            else
                            {
                                redecompose = 0;
                                needsRecalc = true;
                                continue;
                            }
                        }
                    }

                    // swap base
                    k = B[mini];
                    B[mini] = V[minj];
                    V[minj] = k;

                    round++;
                    if (round == 5000) round++;
                } while (round < 5000);
            }            
            // just in case
            return new double[cols + 1];
        }
    }
}
