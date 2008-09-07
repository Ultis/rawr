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
        //internal static double[] _b;
        internal static double[] _cost;
        private static double[] _costWorking;
        //private static double[] _beta;
        //private static double[] _betaBackup;

        private bool costWorkingDirty;
        private bool shifted;

        private int[] _flags;
        internal double[] _lb;
        internal double[] _ub;

        private const int flagNLB = 0x1; // variable nonbasic at lower bound
        private const int flagNUB = 0x2; // variable nonbasic at upper bound
        private const int flagN = 0x3; // variable nonbasic
        private const int flagB = 0x4; // variable basic
        private const int flagDis = 0x8; // variable blacklisted
        private const int flagFix = 0x10; // variable fixed
        private const int flagLB = 0x20; // variable has finite lower bound
        private const int flagUB = 0x40; // variable has finite upper bound
        private const int flagPivot = 0x80; // variable is eligible for pivot
        private const int flagFlip = 0x100; // variable bounds must be flipped
        private const int flagPivot2 = 0x200; // variable is in reduced pivot candidate set

        private bool disabledDirty;

        private double* a;
        private double* U;
        private double* d;
        private double* x;
        private double* w;
        private double* ww;
        private double* wd;
        private double* c;
        private double* u;
        //private double* b;
        private double* cost;
        private double* sparseValue;
        private double* D;
        private int* B;
        private int* V;
        private int* sparseRow;
        private int* sparseCol;
        private int* flags;
        private double* lb;
        private double* ub;
        private double* beta;
        private double* betaBackup;

        private static int maxRows = 0;
        private static int maxCols = 0;

        private const double epsPrimal = 1.0e-7;
        private const double epsPrimalLow = 1.0e-6;
        private const double epsPrimalRel = 1.0e-9;
        private const double epsDual = 1.0e-7; 
        private const double epsDualI = 1.0e-8;
        private const double epsPivot = 1.0e-5;
        private const double epsPivot2 = 1.0e-7;
        private const double epsZero = 1.0e-12;
        private const double epsDrop = 1.0e-14;

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
            _cost = new double[maxCols + maxRows];
            _costWorking = new double[maxCols + maxRows];
            //_b = new double[maxRows];
            //_beta = new double[maxRows];
            //_betaBackup = new double[maxRows];
        }


        public LP Clone()
        {
            LP clone = (LP)MemberwiseClone();
            clone._B = (int[])_B.Clone();
            clone._V = (int[])_V.Clone();
            clone._flags = (int[])_flags.Clone();
            clone._lb = (double[])_lb.Clone();
            clone._ub = (double[])_ub.Clone();
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
                else if (col == cols) return -_lb[cols + row];
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

        private bool hardInfeasibility = false;

        public bool UpdateLowerBound(int col, double value)
        {
            if (value > _lb[col] + epsZero)
            {
                SetLowerBound(col, value);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool UpdateUpperBound(int col, double value)
        {
            if (value < _ub[col] - epsZero)
            {
                SetUpperBound(col, value);
                return true;
            }
            else
            {
                return false;
            }
        }

        public void SetLowerBound(int col, double value)
        {
            _lb[col] = value;
            if (double.IsNegativeInfinity(value))
            {
                _flags[col] &= ~flagLB;
            }
            else
            {
                _flags[col] |= flagLB;
            }
            if (_ub[col] - _lb[col] < epsZero)
            {
                _flags[col] |= flagFix;
            }
            else
            {
                _flags[col] &= ~flagFix;
            }
            if (_ub[col] < _lb[col] - epsZero) hardInfeasibility = true;
        }

        public void SetUpperBound(int col, double value)
        {
            _ub[col] = value;
            if (double.IsPositiveInfinity(value))
            {
                _flags[col] &= ~flagUB;
            }
            else
            {
                _flags[col] |= flagUB;
            }
            if (_ub[col] - _lb[col] < epsZero)
            {
                _flags[col] |= flagFix;
            }
            else
            {
                _flags[col] &= ~flagFix;
            }
            if (_ub[col] < _lb[col] - epsZero) hardInfeasibility = true;
        }

        public void SetRHS(int row, double value)
        {
            //_b[row] = value;
            SetLowerBound(cols + row, -value);
        }

        public void SetLHS(int row, double value)
        {
            SetUpperBound(cols + row, -value);
        }

        public double GetConstraintElement(int index, int col)
        {
            return extraConstraints[index * cols + col];
        }

        public void SetConstraintElement(int index, int col, double value)
        {
            extraConstraints[index * cols + col] = value;
        }

        public double GetConstraintRHS(int index)
        {
            return -_lb[cols + baseRows + index];
        }

        public void SetConstraintRHS(int index, double value)
        {
            SetLowerBound(cols + baseRows + index, -value);
        }

        public double GetConstraintLHS(int index)
        {
            return -_ub[cols + baseRows + index];
        }

        public void SetConstraintLHS(int index, double value)
        {
            SetUpperBound(cols + baseRows + index, -value);
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
            if (cols + rows + 100 >= maxSize)
            {
                maxSize = 2 * maxSize;
                ExtendInstanceArrays();
            }
            _cost[cols] = 0.0;
            _ub[cols] = double.PositiveInfinity;
            _flags[cols] = flagNLB | flagLB;
            cols++;            
            return A.AddColumn();
        }

        private int maxSize;

        // should only add extra constraints that are tight when primal feasible
        public int AddConstraint(bool editable)
        {
            if (cols + rows >= maxSize)
            {
                maxSize += 100;
                ExtendInstanceArrays();
            }
            numExtraConstraints++;
            rows++;
            double[] newArray = new double[(cols + 1) * numExtraConstraints];
            if (extraConstraints != null) Array.Copy(extraConstraints, newArray, cols * (numExtraConstraints - 1));
            extraConstraints = newArray;
            if (extraConstraintEditable == null) extraConstraintEditable = new List<bool>();
            extraConstraintEditable.Add(editable);
            _ub[cols + rows - 1] = double.PositiveInfinity;
            _flags[cols + rows - 1] = flagLB | flagB;
            _cost[cols + rows - 1] = 0.0;
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
        public void EndColumnConstruction()
        {
            if (constructed) return;
            if (cols + rows > maxSize + 100)
            {
                maxSize = cols + rows + 100;
                ExtendInstanceArrays();
            }
            A.EndConstruction();
            _B = new int[rows];
            _V = new int[cols];
            for (int i = 0; i < rows; i++)
            {
                _B[i] = cols + i;
                _ub[cols + i] = double.PositiveInfinity;
                _flags[cols + i] = flagB | flagLB;
                _cost[cols + i] = 0.0;
            }
            for (int j = 0; j < cols; j++)
            {
                _V[j] = j;
            }
            constructed = true;
        }

        private void ExtendInstanceArrays()
        {
            if (_lb == null)
            {
                _lb = new double[maxSize];
                _ub = new double[maxSize];
                _flags = new int[maxSize];
            }
            else
            {
                double[] newlb = new double[maxSize];
                Array.Copy(_lb, newlb, _lb.Length);
                _lb = newlb;
                double[] newub = new double[maxSize];
                Array.Copy(_ub, newub, _ub.Length);
                _ub = newub;
                int[] newflags = new int[maxSize];
                Array.Copy(_flags, newflags, _flags.Length);
                _flags = newflags;
            }
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
            maxSize = rows + 500;
            ExtendInstanceArrays();

            A = new SparseMatrix(baseRows, maxCols + baseRows);
            lu = new LU(rows);
            //extraConstraints = new double[cols + rows + 1];
            //extraConstraints[cols + rows - 1] = 1;
            //numExtraConstraints = 1;
            //Array.Clear(_b, 0, baseRows);

            ColdStart = true;
        }

        private bool ColdStart;

        public unsafe double[] SolvePrimal(bool prepareForDual)
        {
            if (hardInfeasibility) return new double[cols + 1];
            double[] ret = null;

            fixed (double* a = SparseMatrix.data, U = LU._U, sL = LU.sparseL, column = LU.column, column2 = LU.column2, d = _d, x = _x, w = _w, ww = _ww, wd = _wd, c = _c, u = _u, cost = _cost, costw = _costWorking, sparseValue = SparseMatrix.value, D = extraConstraints, lb = _lb, ub = _ub)
            fixed (int* B = _B, V = _V, sparseRow = SparseMatrix.row, sparseCol = SparseMatrix.col, P = LU._P, Q = LU._Q, LJ = LU._LJ, sLI = LU.sparseLI, sLstart = LU.sparseLstart, flags = _flags)
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
                this.cost = cost;
                this.sparseValue = sparseValue;
                this.D = D;
                this.B = B;
                this.V = V;
                this.sparseRow = sparseRow;
                this.sparseCol = sparseCol;
                this.flags = flags;
                this.lb = lb;
                this.ub = ub;

                lu.BeginUnsafe(U, sL, P, Q, LJ, sLI, sLstart, column, column2);
                if (prepareForDual)
                {
                    this.cost = costw;
                    LoadCost();
                    PerturbCost();
                }
                ret = SolvePrimalUnsafe(prepareForDual, prepareForDual, false, true);
                lu.EndUnsafe();

                this.a = null;
                this.U = null;
                this.d = null;
                this.x = null;
                this.w = null;
                this.ww = null;
                this.wd = null;
                this.c = null;
                this.u = null;
                this.cost = null;
                this.sparseValue = null;
                this.D = null;
                this.B = null;
                this.V = null;
                this.sparseRow = null;
                this.sparseCol = null;
                this.flags = null;
                this.lb = null;
                this.ub = null;
            }

            return ret;
        }

        private unsafe void Decompose()
        {
            for (int j = 0; j < rows; j++)
            {
                int col = B[j];
                if (col < cols)
                {
                    int i = 0;
                    for (; i < baseRows; i++)
                    {
                        U[i * rows + j] = a[i + col * baseRows];
                    }
                    for (int k = 0; k < numExtraConstraints; k++, i++)
                    {
                        U[i * rows + j] = D[k * cols + col];
                    }
                }
                else
                {
                    for (int i = 0; i < rows; i++)
                    {
                        U[i * rows + j] = (i == col - cols) ? 1.0 : 0.0;
                    }
                }
            }
            lu.Decompose();
        }

        private unsafe void PatchSingularBasis()
        {
            for (int j = lu.Rank; j < rows; j++)
            {
                int singularColumn = LU._Q[j];
                int singularRow = LU._P[j];
                int slackColumn = cols + singularRow;
                int vindex = Array.IndexOf(_V, slackColumn);
                V[vindex] = B[singularColumn];
                B[singularColumn] = slackColumn;
            }
        }

        private double DualPhaseILowerBound(int col)
        {
            //if (col < cols && (flags[col] & flagLB) == 0) return -1.0;
            if ((flags[col] & flagLB) == 0) return -1.0;
            return 0.0;
        }

        private double DualPhaseIUpperBound(int col)
        {
            //if (col < cols && (flags[col] & flagUB) == 0) return 1.0;
            if ((flags[col] & flagUB) == 0) return 1.0;
            return 0.0;
        }

        private unsafe void ComputePrimalSolution(bool dualPhaseI)
        {
            Array.Clear(_d, 0, rows);
            //int j = 0;
            //for (; j < baseRows; j++)
            //{
            //    d[j] = b[j];
            //}
            //for (int k = 0; k < numExtraConstraints; k++, j++)
            //{
            //    d[j] = D[k * (cols + 1) + cols];
            //}
            for (int col = 0; col < cols + rows; col++)
            {
                double v = 0.0;
                if (dualPhaseI)
                {
                    if ((flags[col] & flagNLB) != 0)
                    {
                        v = DualPhaseILowerBound(col);
                    }
                    else if ((flags[col] & flagNUB) != 0)
                    {
                        v = DualPhaseIUpperBound(col);
                    }
                }
                else
                {
                    if ((flags[col] & flagNLB) != 0)
                    {
                        v = lb[col];
                    }
                    else if ((flags[col] & flagNUB) != 0)
                    {
                        v = ub[col];
                    }
                }
                if (Math.Abs(v) >= epsZero)
                {
                    if (col < cols)
                    {
                        int sCol1 = sparseCol[col];
                        int sCol2 = sparseCol[col + 1];
                        int* sRow = sparseRow + sCol1;
                        double* sValue = sparseValue + sCol1;
                        for (int i = sCol1; i < sCol2; i++, sRow++, sValue++)
                        {
                            d[*sRow] -= *sValue * v;
                        }
                        for (int k = 0; k < numExtraConstraints; k++)
                        {
                            d[baseRows + k] -= D[k * cols + col] * v;
                        }
                    }
                    else
                    {
                        d[col - cols] -= v;
                    }
                }
            }
            lu.FSolve(d);
        }

        private unsafe bool IsPrimalFeasible(double eps)
        {
            for (int i = 0; i < rows; i++)
            {
                int col = B[i];
                if (d[i] < lb[col] - Math.Abs(lb[col]) * epsPrimalRel - eps || d[i] > ub[col] + Math.Abs(ub[col]) * epsPrimalRel + eps)
                {
                    return false;
                }
            }
            return true;
        }

        private unsafe void ComputeReducedCosts()
        {
            for (int i = 0; i < rows; i++)
            {
                //if (B[i] < cols) u[i] = cost[B[i]];
                //else u[i] = 0.0;
                u[i] = cost[B[i]];
            }
            lu.BSolve(u);
            for (int j = 0; j < cols; j++)
            {
                int col = V[j];

                if (col < cols)
                {
                    double costcol = cost[col];
                    int sCol1 = sparseCol[col];
                    int sCol2 = sparseCol[col + 1];
                    int* sRow = sparseRow + sCol1;
                    double* sValue = sparseValue + sCol1;
                    for (int i = sCol1; i < sCol2; i++, sRow++, sValue++)
                    {
                        costcol -= *sValue * u[*sRow];
                    }
                    for (int k = 0; k < numExtraConstraints; k++)
                    {
                        costcol -= D[k * cols + col] * u[baseRows + k];
                    }
                    c[j] = costcol;
                }
                else
                {
                    c[j] = cost[col] - u[col - cols];
                }
            }
        }

        private unsafe void ComputePhaseIReducedCosts(out double infeasibility, double eps)
        {
            infeasibility = 0.0;
            for (int i = 0; i < rows; i++)
            {
                int col = B[i];
                if (d[i] < lb[col] - Math.Abs(lb[col]) * epsPrimalRel - eps)
                {
                    x[i] = 1.0;
                    infeasibility += lb[col] - d[i];
                }
                else if (d[i] > ub[col] + Math.Abs(ub[col]) * epsPrimalRel + eps)
                {
                    x[i] = -1.0;
                    infeasibility += d[i] - ub[col];
                }
                else x[i] = 0.0;
            }
            lu.BSolve(x);
            for (int j = 0; j < cols; j++)
            {
                int col = V[j];

                if (col < cols)
                {
                    double costcol = 0;
                    int sCol1 = sparseCol[col];
                    int sCol2 = sparseCol[col + 1];
                    int* sRow = sparseRow + sCol1;
                    double* sValue = sparseValue + sCol1;
                    for (int i = sCol1; i < sCol2; i++, sRow++, sValue++)
                    {
                        costcol -= *sValue * x[*sRow];
                    }
                    for (int k = 0; k < numExtraConstraints; k++)
                    {
                        costcol -= D[k * cols + col] * x[baseRows + k];
                    }
                    c[j] = costcol;
                }
                else
                {
                    c[j] = -x[col - cols];
                }
            }
        }

        private unsafe double[] ComputeReturnSolution()
        {
            double[] ret = new double[cols + 1];
            double value = 0.0;
            for (int i = 0; i < rows; i++)
            {
                if (B[i] < cols)
                {
                    double di = d[i];
                    if (Math.Abs(di - lb[i]) < Math.Abs(lb[i]) * epsPrimalRel + epsPrimalLow) di = lb[i];
                    else if (Math.Abs(di - ub[i]) < Math.Abs(ub[i]) * epsPrimalRel + epsPrimalLow) di = ub[i];
                    ret[B[i]] = di;
                    value += cost[B[i]] * di;
                }
            }
            for (int i = 0; i < cols; i++)
            {
                if ((flags[i] & flagNLB) != 0)
                {
                    ret[i] = lb[i];
                    value += cost[i] * lb[i];
                }
                else if ((flags[i] & flagNUB) != 0)
                {
                    ret[i] = ub[i];
                    value += cost[i] * ub[i];
                }
            }
            ret[cols] = value;
            return ret;
        }

        private unsafe double ComputeValue()
        {
            double value = 0.0;
            for (int i = 0; i < rows; i++)
            {
                if (B[i] < cols)
                {
                    value += cost[B[i]] * d[i];
                }
            }
            for (int i = 0; i < cols; i++)
            {
                if ((flags[i] & flagNLB) != 0)
                {
                    value += cost[i] * lb[i];
                }
                else if ((flags[i] & flagNUB) != 0)
                {
                    value += cost[i] * ub[i];
                }
            }
            return value;
        }

        private unsafe double ComputeDualIValue()
        {
            double value = 0.0;
            for (int i = 0; i < rows; i++)
            {
                if (B[i] < cols)
                {
                    value += cost[B[i]] * d[i];
                }
            }
            for (int i = 0; i < cols; i++)
            {
                if ((flags[i] & flagNLB) != 0)
                {
                    value += cost[i] * DualPhaseILowerBound(i);
                }
                else if ((flags[i] & flagNUB) != 0)
                {
                    value += cost[i] * DualPhaseIUpperBound(i);
                }
            }
            return value;
        }
        private unsafe int SelectPrimalIncoming(out double direction, bool prepareForDual)
        {
            double maxc = (prepareForDual) ? epsDualI : epsDual;
            int maxj = -1;
            direction = 0.0;
            double dir = 0.0;
            for (int j = 0; j < cols; j++)
            {
                int col = V[j];
                double costj = 0.0;
                if ((flags[col] & flagNLB) != 0)
                {
                    costj = c[j];
                    dir = 1.0;
                }
                else if ((flags[col] & flagNUB) != 0)
                {
                    costj = -c[j];
                    dir = -1.0;
                }
                if (costj > maxc && (flags[col] & flagDis) == 0 && (flags[col] & flagFix) == 0)
                {
                    maxc = costj;
                    maxj = j;
                    direction = dir;
                }
            }
            return maxj;
        }

        private unsafe bool SelectPrimalOutgoing(int incoming, double direction, bool feasible, out int mini, out double minr, out int bound, double eps)
        {
            // w = U \ (L \ A(:,j));
            int maxcol = V[incoming];
            if (maxcol < cols)
            {
                int i = 0;
                for (; i < baseRows; i++)
                {
                    w[i] = a[i + maxcol * baseRows];
                }
                for (int k = 0; k < numExtraConstraints; k++, i++)
                {
                    w[i] = D[k * cols + maxcol];
                }
            }
            else
            {
                //Array.Clear(_w, 0, rows);
                //w[maxcol - cols] = 1.0;
                for (int i = 0; i < rows; i++)
                {
                    w[i] = (i == maxcol - cols) ? 1.0 : 0.0;
                }
            }
            //lu.FSolve(w);
            lu.FSolveL(w, ww);
            lu.FSolveU(ww, w);

            // min over i of d[i]/w[i] where w[i]>0
            double minrr = double.PositiveInfinity;
            minr = double.PositiveInfinity;
            mini = -1;
            bound = 0;
            double minv = 0.0;
            //double reducedcost = c[incoming];
            for (int i = 0; i < rows; i++)
            {
                double v = Math.Abs(w[i]);
                double wi = direction * w[i];
                int col = B[i];
                bool ifeasiblelb = (d[i] >= lb[col] - Math.Abs(lb[col]) * epsPrimalRel - eps);
                bool ifeasibleub = (d[i] <= ub[col] + Math.Abs(ub[col]) * epsPrimalRel + eps);
                bool ifeasible = ifeasiblelb && ifeasibleub;
                if (feasible && !ifeasible)
                {
                    // we lost primal feasibility
                    // fall back to phase I
                    return false;
                }
                if (feasible || ifeasible)
                {
                    if (wi > epsPivot && (flags[col] & flagLB) != 0)
                    {
                        double r = (d[i] - lb[col]) / wi;
                        if (r < minrr + epsZero && (r < minrr || v > minv))
                        {
                            minrr = r;
                            minr = (d[i] - lb[col]) / w[i];
                            mini = i;
                            minv = v;
                            bound = flagNLB;
                        }
                    }
                    else if (wi < -epsPivot && (flags[col] & flagUB) != 0)
                    {
                        double r = (d[i] - ub[col]) / wi;
                        if (r < minrr + epsZero && (r < minrr || v > minv))
                        {
                            minrr = r;
                            minr = (d[i] - ub[col]) / w[i];
                            mini = i;
                            minv = v;
                            bound = flagNUB;
                        }
                    }
                }
                else
                {
                    if (!ifeasibleub && wi > epsPivot)
                    {
                        double r = (d[i] - ub[col]) / wi;
                        if (r < minrr + epsZero && (r < minrr || v > minv))
                        {
                            minrr = r;
                            minr = (d[i] - ub[col]) / w[i];
                            mini = i;
                            minv = v;
                            bound = flagNUB;
                        }
                    }
                    else if (!ifeasiblelb && wi < -epsPivot)
                    {
                        double r = (d[i] - lb[col]) / wi;
                        if (r < minrr + epsZero && (r < minrr || v > minv))
                        {
                            minrr = r;
                            minr = (d[i] - lb[col]) / w[i];
                            mini = i;
                            minv = v;
                            bound = flagNLB;
                        }
                    }
                }
            }
            return true;
        }

        private unsafe int SelectDualOutgoing(bool phaseI, out double delta, out int bound)
        {
            double mind = 0.0;
            int mini = -1;
            delta = 0.0;
            bound = 0;
            for (int i = 0; i < rows; i++)
            {
                int col = B[i];
                double lbcol, ubcol;
                if (phaseI)
                {
                    lbcol = DualPhaseILowerBound(col);
                    ubcol = DualPhaseIUpperBound(col);
                }
                else
                {
                    lbcol = lb[col];
                    ubcol = ub[col];
                }
                if ((flags[col] & flagDis) == 0)
                {
                    if ((phaseI || (flags[col] & flagLB) != 0) && d[i] < lbcol - Math.Abs(lbcol) * epsPrimalRel - epsPrimal)
                    {
                        double dist = d[i] - lbcol;
                        if (dist < mind)
                        {
                            mind = dist;
                            mini = i;
                            delta = dist;
                            bound = flagNLB;
                        }
                    }
                    else if ((phaseI || (flags[col] & flagUB) != 0) && d[i] > ubcol + Math.Abs(ubcol) * epsPrimalRel + epsPrimal)
                    {
                        double dist = ubcol - d[i];
                        if (dist < mind)
                        {
                            mind = dist;
                            mini = i;
                            delta = -dist;
                            bound = flagNUB;
                        }
                    }
                }
            }
            return mini;
        }

        private unsafe bool ComputeDualPivotRow(bool phaseI, int outgoing)
        {
            // x = z <- e_mini*A_B^-1
            for (int i = 0; i < rows; i++)
            {
                x[i] = ((i == outgoing) ? 1 : 0);
            }
            lu.BSolve(x); // TODO exploit nature of x

            double* cj = c;
            double* wdj = wd;
            int* Vj = V;
            double eps = phaseI ? epsDualI : epsDual;
            for (int j = 0; j < cols; j++, cj++, wdj++, Vj++)
            {
                int col = *Vj;
                if (phaseI)
                {
                    if ((flags[col] & flagUB) != 0 && (flags[col] & flagLB) != 0) continue;
                }
                else
                {
                    if ((flags[col] & flagFix) != 0) continue;
                }
                if ((flags[col] & flagNLB) != 0 && *cj > eps)
                {
                    // we lost dual feasibility
                    // creative use of shifting
                    double shift = eps - *cj;
                    if (Math.Abs(shift) < 1.0e-5)
                    {
                        c[j] += shift;
                        cost[col] += shift; // requires working copy for cost
                        costWorkingDirty = true;
                        shifted = true;
                    }
                    else
                    {
                        // fall back to dual phase I
                        return false;
                    }
                }
                else if ((flags[col] & flagNUB) != 0 && *cj < -eps)
                {
                    // we lost dual feasibility
                    // creative use of shifting
                    double shift = -eps - *cj;
                    if (Math.Abs(shift) < 1.0e-5)
                    {
                        c[j] += shift;
                        cost[col] += shift; // requires working copy for cost
                        costWorkingDirty = true;
                        shifted = true;
                    }
                    else
                    {
                        // fall back to dual phase I
                        return false;
                    }
                }
                if (col < cols)
                {
                    *wdj = 0;
                    int sCol1 = sparseCol[col];
                    int sCol2 = sparseCol[col + 1];
                    int* sRow = sparseRow + sCol1;
                    int* sRow2 = sparseRow + sCol2;
                    double* sValue = sparseValue + sCol1;
                    for (; sRow < sRow2; sRow++, sValue++)
                    {
                        *wdj += *sValue * x[*sRow];
                    }
                    for (int k = 0; k < numExtraConstraints; k++)
                    {
                        *wdj += D[k * cols + col] * x[baseRows + k];
                    }
                }
                else
                {
                    *wdj = x[col - cols];
                }
            }
            return true;
        }

        private unsafe void SelectDualIncoming(bool phaseI, int outgoing, out int minj, out double minr)
        {
            double minrr = double.PositiveInfinity;
            minr = double.PositiveInfinity;
            minj = -1;
            double minv = 0.0;
            double* cj = c;
            double* wdj = wd;
            int* Vj = V;
            double eps = phaseI ? epsDualI : epsDual;
            double direction = 1.0;
            if (phaseI)
            {
                if (d[outgoing] > DualPhaseIUpperBound(B[outgoing]))
                {
                    direction = -1.0;
                }
            }
            else
            {
                if (d[outgoing] > ub[B[outgoing]])
                {
                    direction = -1.0;
                }
            }
            for (int j = 0; j < cols; j++, cj++, wdj++, Vj++)
            {
                int col = *Vj;
                if (phaseI)
                {
                    if ((flags[col] & flagUB) != 0 && (flags[col] & flagLB) != 0) continue;
                }
                else
                {
                    if ((flags[col] & flagFix) != 0) continue;
                }
                double v = Math.Abs(*wdj);
                double wdir = direction * *wdj;
                if (((flags[col] & flagNLB) != 0 && wdir < -epsPivot) || ((flags[col] & flagNUB) != 0 && wdir > epsPivot))
                {
                    double r = *cj / wdir;
                    if (r < minrr + epsZero && (r < minrr || v > minv))
                    {
                        minr = *cj / *wdj;
                        minrr = r;
                        minj = j;
                        minv = v;
                    }
                }
            }
        }

        private unsafe void SelectDualIncomingWithBoundFlips(bool phaseI, double delta, out int minj, out double minr)
        {
            minj = -1;
            minr = double.PositiveInfinity;
            //double minv = 0.0;
            double eps = phaseI ? epsDualI : epsDual;
            double epsp = epsPivot;
            bool positive = (delta > 0);
            if (!positive) delta = -delta;
            int available = 0;
            int allavailable = 0;
            bool retried = false;

            RETRY:
            // mark eligible pivots and compute max step
            double maxstep = double.PositiveInfinity;
            double* cj = c;
            double* wdj = wd;
            int* Vj = V;
            for (int j = 0; j < cols; j++, cj++, wdj++, Vj++)
            {
                int col = *Vj;
                flags[col] &= ~flagPivot2;
                if (phaseI)
                {
                    if ((flags[col] & flagUB) != 0 && (flags[col] & flagLB) != 0)
                    {
                        flags[col] &= ~flagPivot;
                        continue;
                    }
                }
                else
                {
                    if ((flags[col] & flagFix) != 0)
                    {
                        flags[col] &= ~flagPivot;
                        continue;
                    }
                }
                double wdir;
                if (positive)
                    wdir = -*wdj;
                else
                    wdir = *wdj;
                if (((flags[col] & flagNLB) != 0 && wdir < -epsp) || ((flags[col] & flagNUB) != 0 && wdir > epsp))
                {
                    double r;
                    if (wdir > 0)
                    {
                        r = (*cj + eps) / wdir;
                    }
                    else
                    {
                        r = (*cj - eps) / wdir;
                    }
                    if (r < maxstep) maxstep = r;
                    flags[col] |= flagPivot;
                    available++;
                    allavailable++;
                }
                else
                {
                    flags[col] &= ~flagPivot;
                }
            }
            if (available == 0 && Math.Abs(delta) < epsPivot && !retried)
            {
                retried = true;
                epsp = 1.0e-7;
                goto RETRY;
            }

            double deltamin = 0.0;
            double step = Math.Max(maxstep * 10.0, eps);
            while (delta - deltamin > epsZero && allavailable > 0)
            {
                available = 0;
                delta -= deltamin;
                maxstep = double.PositiveInfinity;
                cj = c;
                wdj = wd;
                Vj = V;
                deltamin = 0;
                for (int j = 0; j < cols; j++, cj++, wdj++, Vj++)
                {
                    int col = *Vj;
                    flags[col] &= ~flagPivot2;
                    if ((flags[col] & flagPivot) != 0)
                    {
                        double wdir;
                        if (positive)
                            wdir = -*wdj;
                        else
                            wdir = *wdj;
                        double r;
                        double v = Math.Abs(*wdj);
                        if (wdir > 0) // upper bound
                        {
                            r = (*cj + eps) / wdir;
                            if (*cj - step * wdir < -eps)
                            {
                                flags[col] &= ~flagPivot;
                                flags[col] |= flagPivot2;
                                available++;
                                allavailable--;
                                if (phaseI)
                                {
                                    deltamin += (DualPhaseIUpperBound(col) - DualPhaseILowerBound(col)) * v;
                                }
                                else
                                {
                                    deltamin += (ub[col] - lb[col]) * v;
                                }
                            }
                            else if (r < maxstep) maxstep = r;
                        }
                        else // lower bound
                        {
                            r = (*cj - eps) / wdir;
                            if (*cj - step * wdir > eps)
                            {
                                flags[col] &= ~flagPivot;
                                flags[col] |= flagPivot2;
                                available++;
                                allavailable--;
                                if (phaseI)
                                {
                                    deltamin += (DualPhaseIUpperBound(col) - DualPhaseILowerBound(col)) * v;
                                }
                                else
                                {
                                    deltamin += (ub[col] - lb[col]) * v;
                                }
                            }
                            else if (r < maxstep) maxstep = r;
                        }
                    }
                }
                step = 2.0 * maxstep;
            }

            while (delta >= 0 && (available > 0 || delta > epsZero))
            {
                maxstep = double.PositiveInfinity;
                cj = c;
                wdj = wd;
                Vj = V;
                for (int j = 0; j < cols; j++, cj++, wdj++, Vj++)
                {
                    int col = *Vj;
                    if ((flags[col] & flagPivot2) != 0)
                    {
                        double wdir;
                        if (positive)
                            wdir = -*wdj;
                        else
                            wdir = *wdj;
                        double r;
                        if (wdir > 0)
                        {
                            r = (*cj + eps) / wdir;
                        }
                        else
                        {
                            r = (*cj - eps) / wdir;
                        }
                        if (r < maxstep) maxstep = r;
                    }
                } 

                minj = -1;
                double minv = 0.0;
                cj = c;
                wdj = wd;
                Vj = V;
                for (int j = 0; j < cols; j++, cj++, wdj++, Vj++)
                {
                    int col = *Vj;
                    if ((flags[col] & flagPivot2) != 0)
                    {
                        double wdir;
                        if (positive)
                            wdir = -*wdj;
                        else
                            wdir = *wdj;
                        double r = *cj / wdir;
                        double v = Math.Abs(*wdj);
                        if (r <= maxstep)
                        {
                            if (phaseI)
                            {
                                delta -= (DualPhaseIUpperBound(col) - DualPhaseILowerBound(col)) * v;
                            }
                            else
                            {
                                delta -= (ub[col] - lb[col]) * v;
                            }
                            flags[col] &= ~flagPivot2;
                            available--;
                            if (v > minv)
                            {
                                minv = v;
                                minj = j;
                            }
                        }
                    }
                }
                if (minj == -1) return;
            }

            if (minj != -1)
            {
                minr = c[minj] / wd[minj];
            }
        }

        private unsafe void UpdatePrimal(double minr, int mini, int maxj)
        {
            // minr = primal step
            // rd = dual step

            double rd = c[maxj] / w[mini];

            // x = z <- e_mini*A_B^-1
            for (int i = 0; i < rows; i++)
            {
                x[i] = ((i == mini) ? 1.0 : 0.0);
            }
            lu.BSolve(x); // TODO exploit nature of x

            // update primal and dual
            if (Math.Abs(minr) > epsZero)
            {
                for (int i = 0; i < rows; i++)
                {
                    d[i] -= minr * w[i];
                }
            }
            if ((flags[V[maxj]] & flagNLB) != 0)
            {
                d[mini] = lb[V[maxj]] + minr;
            }
            else if ((flags[V[maxj]] & flagNUB) != 0)
            {
                d[mini] = ub[V[maxj]] + minr;
            }
            double* cc = c;
            for (int j = 0; j < cols; j++, cc++)
            {
                int col = V[j];
                if (col < cols)
                {
                    //double v = 0.0;
                    int sCol1 = sparseCol[col];
                    int sCol2 = sparseCol[col + 1];
                    int* sRow = sparseRow + sCol1;
                    int* sRow2 = sparseRow + sCol2;
                    double* sValue = sparseValue + sCol1;
                    // TODO reinvestigate moving rd out of the loop once dual is more stable
                    for (; sRow < sRow2; sRow++, sValue++)
                    {
                        *cc -= rd * *sValue * x[*sRow];
                    }
                    for (int k = 0; k < numExtraConstraints; k++)
                    {
                        *cc -= rd * D[k * cols + col] * x[baseRows + k];
                    }
                }
                else
                {
                    *cc -= rd * x[col - cols];
                }
            }
            c[maxj] = -rd;
        }

        private unsafe void UpdateDual(bool phaseI, int minj, int mini, double minr, double delta, bool updatec)
        {
            // w = U \ (L \ A(:,j));
            int mincol = V[minj];
            if (mincol < cols)
            {
                int i = 0;
                for (; i < baseRows; i++)
                {
                    w[i] = a[i + mincol * baseRows];
                }
                for (int k = 0; k < numExtraConstraints; k++, i++)
                {
                    w[i] = D[k * cols + mincol];
                }
            }
            else
            {
                //Array.Clear(_w, 0, rows);
                //w[mincol - cols] = 1.0;
                for (int i = 0; i < rows; i++)
                {
                    w[i] = (i == mincol - cols) ? 1.0 : 0.0;
                }
            }
            //lu.FSolve(w);
            lu.FSolveL(w, ww);
            lu.FSolveU(ww, w);

            // -minr = dual step
            // rp = primal step

            //System.Diagnostics.Trace.WriteLine(w[mini] + " = " + wd[minj]);

            double l, u;
            if (phaseI)
            {
                u = DualPhaseIUpperBound(B[mini]);
                l = DualPhaseILowerBound(B[mini]);
            }
            else
            {
                u = ub[B[mini]];
                l = lb[B[mini]];
            }
            if (delta > 0)
            {
                delta = d[mini] - u;
            }
            else
            {
                delta = d[mini] - l;
            }
            double rp = delta / w[mini];

            // update primal and dual
            for (int i = 0; i < rows; i++)
            {
                d[i] -= rp * w[i];
            }
            if ((flags[mincol] & flagNLB) != 0)
            {
                d[mini] = (phaseI ? DualPhaseILowerBound(mincol) : lb[mincol]) + rp;
            }
            else if ((flags[mincol] & flagNUB) != 0)
            {
                d[mini] = (phaseI ? DualPhaseIUpperBound(mincol) : ub[mincol]) + rp;
            }
            if (updatec)
            {
                double* ccols = c + cols;
                for (double* cj = c, wdj = wd; cj < ccols; cj++, wdj++)
                {
                    *cj -= minr * *wdj;
                }
                c[minj] = -minr;
            }
        }

        private unsafe void PerturbCost()
        {
            Random rnd = new Random(0);
            for (int i = 0; i < cols; i++)
            {
                // usually fixed positions would not be perturbed, but in the context of MIP branching it's necessary to use the same perturbation
                // this wouldn't be an issue if the variable was nonbasic, but especially in MIP the fixed positions will
                // most often be on basic positions (for example when disabling a variable in a branch) and thus it's cost influences
                // reduced costs of all nonbasic variable which could lead to loss of feasibility
                //if ((flags[i] & flagFix) == 0)
                //{
                    cost[i] -= 0.5 * (100.0 * epsDual + Math.Abs(cost[i]) * 1.0e-5) * (1 + rnd.NextDouble()); // perturb in negative direction since all variables/slacks have lower bound
                //}
            }
            costWorkingDirty = true;
        }

        private unsafe void LoadCost()
        {
            Array.Copy(_cost, _costWorking, cols + rows);
            costWorkingDirty = false;
        }

        private unsafe bool BoundFlip(bool phaseI, double dualStep, double eps, int incoming, bool flipBounds)
        {
            bool needDualI = false;
            bool flipsDone = false;
            Array.Clear(_x, 0, rows);
            bool modc = (Math.Abs(dualStep) > epsZero && incoming != -1);
            for (int j = 0; j < cols; j++)
            {
                int col = V[j];
                if (!flipBounds) flags[col] &= ~flagFlip;
                if (j != incoming)
                {
                    if ((flags[col] & flagFix) != 0) continue;
                    if (modc) c[j] -= dualStep * wd[j];
                    double v = 0.0;
                    if ((flags[col] & flagNLB) != 0 && c[j] > eps)
                    {
                        if (phaseI)
                        {
                            // phase I flip
                            v = DualPhaseIUpperBound(col) - DualPhaseILowerBound(col);
                            if (flipBounds) flags[col] = (flags[col] | flagNUB) & ~flagNLB;
                            else flags[col] |= flagFlip;
                        }
                        else if ((flags[col] & flagUB) != 0)
                        {
                            // phase II flip
                            v = ub[col] - lb[col];
                            if (flipBounds) flags[col] = (flags[col] | flagNUB) & ~flagNLB;
                            else flags[col] |= flagFlip;
                        }
                        else
                        {
                            needDualI = true;
                        }
                    }
                    else if ((flags[col] & flagNUB) != 0 && c[j] < -eps)
                    {
                        if (phaseI)
                        {
                            // phase I flip
                            v = DualPhaseILowerBound(col) - DualPhaseIUpperBound(col);
                            if (flipBounds) flags[col] = (flags[col] | flagNLB) & ~flagNUB;
                            else flags[col] |= flagFlip;
                        }
                        else if ((flags[col] & flagLB) != 0)
                        {
                            // phase II flip
                            v = lb[col] - ub[col];
                            if (flipBounds) flags[col] = (flags[col] | flagNLB) & ~flagNUB;
                            else flags[col] |= flagFlip;
                        }
                        else
                        {
                            needDualI = true;
                        }
                    }
                    if (Math.Abs(v) > epsZero)
                    {
                        if (col < cols)
                        {
                            int sCol1 = sparseCol[col];
                            int sCol2 = sparseCol[col + 1];
                            int* sRow = sparseRow + sCol1;
                            int* sRow2 = sparseRow + sCol2;
                            double* sValue = sparseValue + sCol1;
                            for (; sRow < sRow2; sRow++, sValue++)
                            {
                                x[*sRow] += v * *sValue;
                            }
                            for (int k = 0; k < numExtraConstraints; k++)
                            {
                                x[baseRows + k] += v * D[k * cols + col];
                            }
                        }
                        else
                        {
                            x[col - cols] += v;
                        }
                        flipsDone = true;
                    }
                }
            }
            if (incoming != -1) c[incoming] = -dualStep;
            if (flipsDone)
            {
                lu.FSolve(x);
                for (int i = 0; i < rows; i++)
                {
                    d[i] -= x[i];
                }
            }
            return needDualI;
        }

        private unsafe void UpdateBounds()
        {
            for (int j = 0; j < cols; j++)
            {
                int col = V[j];
                if ((flags[col] & flagFlip) != 0)
                {
                    //System.Diagnostics.Debug.WriteLine("Flipped " + col);
                    if ((flags[col] & flagNLB) != 0)
                    {
                        flags[col] = (flags[col] | flagNUB) & ~flagNLB;
                    }
                    else
                    {
                        flags[col] = (flags[col] | flagNLB) & ~flagNUB;
                    }
                }
            }
        }

        private unsafe bool PreprocessingSingletonConstraint()
        {
            bool updated = false;
            // singleton row            
            for (int i = 0; i < rows; i++)
            {
                //a[i + col * baseRows];
                double rhs = 0.0;
                int singlecol = -1;
                bool issingleton = true;
                for (int col = 0; col < cols; col++)
                {
                    double v;
                    if (i < baseRows)
                    {
                        v = a[i + col * baseRows];
                    }
                    else
                    {
                        v = D[(i - baseRows) * cols + col];
                    }
                    if (Math.Abs(v) > epsZero)
                    {
                        if ((flags[col] & flagFix) != 0)
                        {
                            rhs -= v * lb[col];
                        }
                        else
                        {
                            if (singlecol != -1)
                            {
                                issingleton = false;
                                break;
                            }
                            else
                            {
                                singlecol = col;
                            }
                        }
                    }
                }
                if (issingleton)
                {
                    if (singlecol == -1)
                    {
                        // all variables are fixed => row is fixed
                        updated |= UpdateLowerBound(cols + i, -rhs);
                        updated |= UpdateUpperBound(cols + i, -rhs);
                    }
                    else
                    {
                        // update col bounds
                        // v * x + s = rhs
                        // lb <= s <= ub
                        // -ub <= -s <= -lb
                        // x = (rhs - s) / v
                        double v;
                        if (i < baseRows)
                        {
                            v = a[i + singlecol * baseRows];
                        }
                        else
                        {
                            v = D[(i - baseRows) * cols + singlecol];
                        }
                        if (v > 0)
                        {
                            // (rhs - ub) / v <= x = (rhs - s) / v <= (rhs - lb) / v
                            updated |= UpdateLowerBound(singlecol, (rhs - ub[cols + i]) / v);
                            updated |= UpdateUpperBound(singlecol, (rhs - lb[cols + i]) / v);
                        }
                        else
                        {
                            // (rhs - ub) / v >= x = (rhs - s) / v >= (rhs - lb) / v
                            updated |= UpdateUpperBound(singlecol, (rhs - ub[cols + i]) / v);
                            updated |= UpdateLowerBound(singlecol, (rhs - lb[cols + i]) / v);
                        }
                    }
                }
            }
            return updated;
        }

        private unsafe bool PreprocessingTightenBounds()
        {
            bool updated = false;
            // singleton row            
            for (int i = 0; i < rows; i++)
            {
                //a[i + col * baseRows];
                double lbi = 0.0;
                double ubi = 0.0;
                for (int col = 0; col < cols; col++)
                {
                    double v;
                    if (i < baseRows)
                    {
                        v = a[i + col * baseRows];
                    }
                    else
                    {
                        v = D[(i - baseRows) * cols + col];
                    }
                    if (Math.Abs(v) > epsZero)
                    {
                        if (v > 0)
                        {
                            lbi += v * lb[col];
                            ubi += v * ub[col];
                        }
                        else
                        {
                            lbi += v * ub[col];
                            ubi += v * lb[col];
                        }
                    }
                }
                updated |= UpdateLowerBound(cols + i, -ubi);
                updated |= UpdateUpperBound(cols + i, -lbi);
                for (int col = 0; col < cols; col++)
                {
                    double v;
                    if (i < baseRows)
                    {
                        v = a[i + col * baseRows];
                    }
                    else
                    {
                        v = D[(i - baseRows) * cols + col];
                    }
                    if (Math.Abs(v) > epsZero)
                    {
                        if (v > 0)
                        {
                            updated |= UpdateUpperBound(col, lb[col] - (lbi + lb[cols + i]) / v);
                            updated |= UpdateLowerBound(col, ub[col] - (ub[cols + i] + ubi) / v);
                        }
                        else
                        {
                            updated |= UpdateLowerBound(col, ub[col] - (lbi + lb[cols + i]) / v);
                            updated |= UpdateUpperBound(col, lb[col] - (ub[cols + i] + ubi) / v);
                        }
                    }
                }
            }
            return updated;
        }

        private unsafe void Preprocessing()
        {
            if (hardInfeasibility) return;
            bool updated;
            do
            {
                updated = false;
                //updated |= PreprocessingSingletonConstraint();
                updated |= PreprocessingTightenBounds();
            } while (updated && !hardInfeasibility);
        }

        private unsafe void InitializeDSEWeights(bool identity)
        {
            if (identity)
            {
                for (int i = 0; i < rows; i++)
                {
                    beta[i] = 1.0;
                }
            }
            else
            {
                for (int j = 0; j < rows; j++)
                {
                    for (int i = 0; i < rows; i++)
                    {
                        x[i] = ((i == j) ? 1 : 0);
                    }
                    lu.BSolve(x); // TODO exploit nature of x
                    beta[j] = 0.0;
                    for (int i = 0; i < rows; i++)
                    {
                        beta[j] += x[i] * x[i];
                    }
                }
            }
        }

        private unsafe double[] SolvePrimalUnsafe(bool prepareForDual, bool verifyRefactoredOptimality, bool shortLimit, bool highPrecision)
        {
            // LU = A_B
            // d = x_B <- A_B^-1*b ... primal solution
            // u = u <- c_B*A_B^-1 ... dual solution
            // w_N <- c_N - u*A_N  ... dual solution

            int i, j, k;
            bool feasible = false;
            int round = 0;
            int redecompose = 0;
            const int maxRedecompose = 50;
            int verificationAttempts = 0;
            double eps = highPrecision ? epsPrimal : epsPrimalLow;
            double lowestInfeasibility = double.PositiveInfinity;

            if (disabledDirty)
            {
                for (i = 0; i < cols + rows; i++)
                {
                    flags[i] &= ~flagDis;
                }
                disabledDirty = false;
            }

            do
            {
            DECOMPOSE:
                if (redecompose <= 0)
                {
                    Decompose();
                    redecompose = maxRedecompose; // decompose every 50 iterations to maintain stability
                    feasible = false; // when refactoring basis recompute the solution
                }
                if (lu.Singular)
                {
                    redecompose = 0;
                    PatchSingularBasis();
                    continue;
                }

                if (!feasible)
                {
                    ComputePrimalSolution(false);
                    feasible = IsPrimalFeasible(eps);

                    if (feasible)
                    {
                        // we have a feasible solution, initialize phase 2
                        ComputeReducedCosts();
                    }
                    else if (shortLimit)
                    {
                        // we're retuning dual optimality on unshifted/redecomposed basis
                        // if redecomposition caused loss of primal optimality then we probably
                        // won't be able to fix the optimality and doing so might just cause us problems
                        // in the dual
                        // abort and let dual handle the problems if we're also dual unfeasible
                        return null;
                    }
                }

                if (!feasible)
                {
                    double infeasibility;
                    ComputePhaseIReducedCosts(out infeasibility, eps);
                    if (infeasibility < lowestInfeasibility) lowestInfeasibility = infeasibility;
                    else if (infeasibility > lowestInfeasibility + eps)
                    {
                        // we're not using a shifting strategy for primal so the only way to combat
                        // numerical cycling is to lower the tolerances
                        if (eps < 1.0)
                        {
                            eps *= 10.0;
                            lowestInfeasibility = double.PositiveInfinity;
                        }
                        if (redecompose < maxRedecompose)
                        {
                            redecompose = 0;
                            goto DECOMPOSE;
                        }
                    }
                    if (infeasibility > 100.0 && !ColdStart)
                    {
                        // we're so far out of feasible region we're better off starting from scratch
                        for (i = 0; i < rows; i++)
                        {
                            B[i] = cols + i;
                            flags[cols + i] = (flags[cols + i] | flagB) & ~flagN & ~flagDis;
                        }
                        for (j = 0; j < cols; j++)
                        {
                            V[j] = j;
                            flags[j] = (flags[j] | flagNLB) & ~flagB & ~flagDis;
                        }
                        disabledDirty = false;
                        redecompose = 0;
                        ColdStart = true;
                        continue;
                    }
                }

            MINISTEP:
                double direction;
                int maxj = SelectPrimalIncoming(out direction, prepareForDual);

                if (maxj == -1)
                {
                    if (feasible && verifyRefactoredOptimality && redecompose < maxRedecompose && verificationAttempts < 5)
                    {
                        redecompose = 0;
                        feasible = false;
                        verificationAttempts++;
                        continue;
                    }
                    /*if (blacklistAllowed)
                    {
                        bool retry = false;
                        if (disabledDirty)
                        {
                            for (i = 0; i < cols + rows; i++)
                            {
                                if ((flags[i] & flagDis) != 0)
                                {
                                    flags[i] &= ~flagDis;
                                    retry = true;
                                }
                            }
                            disabledDirty = false;
                        }
                        if (retry)
                        {
                            blacklistAllowed = false;
                            redecompose = 0;
                            needsRecalc = true;
                            continue;
                        }
                    }*/

                    // rebuild solution so it's stable
                    //ComputePrimalSolution();

                    // optimum, return solution (or could be no feasible solution)
                    if (!feasible) return new double[cols + 1]; // if it's not feasible then return null solution
                    ColdStart = false;
                    //System.Diagnostics.Trace.WriteLine("Primal optimality in round " + round);
                    return ComputeReturnSolution();
                }

                int mini;
                double minr;
                int bound;

                if (!SelectPrimalOutgoing(maxj, direction, feasible, out mini, out minr, out bound, eps))
                {
                    feasible = false;
                    lowestInfeasibility = double.PositiveInfinity;
                    redecompose = 0;
                    goto DECOMPOSE;
                }

                if (mini == -1)
                {
                    // unbounded
                    if (!ColdStart)
                    {
                        // something went really horribly wrong, try from scratch
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
                        continue;
                    }
                    // completely unstable, investigate
                    double[] ret = new double[cols + 1];
                    return ret;
                }

                // determine if we do a basis change or just a bound swap
                int col = V[maxj];
                if ((flags[col] & (flagLB | flagUB)) == (flagLB | flagUB) && Math.Abs(minr) >= ub[col] - lb[col] - epsZero)
                {
                    // do bound swap
                    if (direction > 0.0)
                    {
                        flags[col] = (flags[col] | flagNUB) & ~flagNLB;
                    }
                    else
                    {
                        flags[col] = (flags[col] | flagNLB) & ~flagNUB;
                    }

                    if (!feasible) continue;

                    minr = direction * (ub[col] - lb[col]);
                    for (i = 0; i < rows; i++)
                    {
                        d[i] -= minr * w[i];
                    }

                    goto MINISTEP;
                }

                if (feasible)
                {
                    UpdatePrimal(minr, mini, maxj);
                }

                // swap base

                redecompose--;
                if (redecompose > 0)
                {
                    double pivot;
                    lu.Update(ww, mini, out pivot);
                    if (lu.Singular || Math.Abs(w[mini] - pivot) > 1.0e-6 * Math.Abs(w[mini]))
                    {
                        if (redecompose == maxRedecompose - 1)
                        {
                            redecompose = 0;
                            feasible = false; // forces recalc
                            flags[V[maxj]] |= flagDis;
                            disabledDirty = true;
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
                flags[B[mini]] = (flags[B[mini]] | flagB) & ~flagN;
                flags[V[maxj]] = (flags[V[maxj]] | bound) & ~flagB;

                round++;
            } while ((round < 5000 && !shortLimit) || round < 100); // limit computation so we don't dead loop, if everything works it shouldn't take more than this
            // when tuning dual feasibility limit to 100 rounds, we don't want to spend too much time on it

            // just in case
            // if feasible return the best we got
            if (feasible) return ComputeReturnSolution();
            return new double[cols + 1];
        }

        public unsafe double[] SolveDual(bool startWithPhaseI)
        {
            if (hardInfeasibility) return new double[cols + 1];
            double[] ret = null;

            fixed (double* a = SparseMatrix.data, U = LU._U, sL = LU.sparseL, column = LU.column, column2 = LU.column2, d = _d, x = _x, w = _w, ww = _ww, wd = _wd, c = _c, u = _u, cost = _costWorking, sparseValue = SparseMatrix.value, D = extraConstraints, lb = _lb, ub = _ub/*, beta = _beta, betaBackup = _betaBackup*/)
            fixed (int* B = _B, V = _V, sparseRow = SparseMatrix.row, sparseCol = SparseMatrix.col, P = LU._P, Q = LU._Q, LJ = LU._LJ, sLI = LU.sparseLI, sLstart = LU.sparseLstart, flags = _flags)
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
                this.cost = cost;
                this.sparseValue = sparseValue;
                this.D = D;
                this.B = B;
                this.V = V;
                this.sparseRow = sparseRow;
                this.sparseCol = sparseCol;
                this.flags = flags;
                this.lb = lb;
                this.ub = ub;
                //this.beta = beta;
                //this.betaBackup = betaBackup;

                lu.BeginUnsafe(U, sL, P, Q, LJ, sLI, sLstart, column, column2);
                LoadCost();
                PerturbCost();
                bool dualUnbounded;
                shifted = false;
                //Preprocessing();
                ret = SolveDualUnsafe(startWithPhaseI, out dualUnbounded);
                if (!dualUnbounded)
                {
                    // unshifted might no longer be dual feasible for our tolerances
                    // reoptimize it with primal, dual phase II tolerances are ok
                    if (shifted)
                    {
                        LoadCost();
                        PerturbCost();
                    }
                    ret = SolvePrimalUnsafe(false, true, true, true);
                }
                if ((costWorkingDirty && !dualUnbounded) || ret == null)
                {
                    if (costWorkingDirty) LoadCost();
                    int[] _Bcopy = null;
                    int[] _Vcopy = null;
                    int[] _flagscopy = null;
                    if (!dualUnbounded)
                    {
                        // we're just reoptimizing for changes in cost vector
                        // since we'll be reusing this basis for dual later
                        // we should store the basis, because the basis on unperturbed problem
                        // very likely is not dual feasible on perturbed system
                        // since we're using deterministic perturbation the likelihood of remaining
                        // dual feasible is much higher since changes due to shifting are very small
                        _Bcopy = (int[])_B.Clone();
                        _Vcopy = (int[])_V.Clone();
                        _flagscopy = (int[])_flags.Clone();
                    }
                    ret = SolvePrimalUnsafe(false, false, false, false);
                    if (!dualUnbounded)
                    {
                        _B = _Bcopy;
                        _V = _Vcopy;
                        _flags = _flagscopy;
                    }
                }
                lu.EndUnsafe();

                this.a = null;
                this.U = null;
                this.d = null;
                this.x = null;
                this.w = null;
                this.ww = null;
                this.wd = null;
                this.c = null;
                this.u = null;
                this.cost = null;
                this.sparseValue = null;
                this.D = null;
                this.B = null;
                this.V = null;
                this.sparseRow = null;
                this.sparseCol = null;
                this.flags = null;
                this.lb = null;
                this.ub = null;
                this.beta = null;
                this.betaBackup = null;
            }

            return ret;
        }

        public unsafe double[] SolveDualUnsafe(bool startWithPhaseI, out bool dualUnbounded)
        {
            // LU = A_B
            // d = x_B <- A_B^-1*b ... primal solution
            // u = u <- c_B*A_B^-1 ... dual solution
            // w_N <- c_N - u*A_N  ... dual solution

            int i, k;
            int round = 0;
            bool needsRecalc = true;
            int redecompose = 0;
            const int maxRedecompose = 50;
            dualUnbounded = false;

            if (disabledDirty)
            {
                for (i = 0; i < cols + rows; i++)
                {
                    flags[i] &= ~flagDis;
                }
                disabledDirty = false;
            }

            bool blacklistAllowed = true;

            bool phaseI = false;
            if (startWithPhaseI)
            {
                Decompose();
                if (lu.Singular)
                {
                    PatchSingularBasis();
                    Decompose();
                }
                redecompose = maxRedecompose;
                ComputePrimalSolution(phaseI);
                ComputeReducedCosts();
                phaseI = BoundFlip(phaseI, 0.0, epsDualI, -1, true);
                if (phaseI)
                {
                    ComputePrimalSolution(phaseI);
                    BoundFlip(phaseI, 0.0, epsDualI, -1, true);
                }
                needsRecalc = false;
            }

            do
            {
            DECOMPOSE:
                if (redecompose <= 0)
                {
                    Decompose();
                    redecompose = maxRedecompose; // decompose each time to see where the instability comes from
                    needsRecalc = true; // for long dual phases refresh the reduced costs
                }

                if (lu.Singular)
                {
                    //System.Diagnostics.Trace.WriteLine("Basis singular in " + round);
                    // try to patch the basis by replacing singular columns with corresponding slacks of singular rows
                    redecompose = 0;
                    PatchSingularBasis();
                    goto DECOMPOSE;
                }

                if (needsRecalc)
                {
                    ComputePrimalSolution(phaseI);
                    ComputeReducedCosts();
                    needsRecalc = false;
                }

                double delta;
                int bound;

                int mini = SelectDualOutgoing(phaseI, out delta, out bound);

                if (mini == -1)
                {
                    // if we blacklisted some variables give it another try
                    if (disabledDirty)
                    {
                        for (i = 0; i < cols + rows; i++)
                        {
                            flags[i] &= ~flagDis;
                        }
                        disabledDirty = false;
                        blacklistAllowed = false;
                        redecompose = 0;
                        needsRecalc = true;
                        continue;
                    }
                    if (phaseI)
                    {
                        // if a variable is on a nonexisting bound then we must have an unrecoverable dual unfeasibility, shouldn't happen
                        for (int col = 0; col < cols + rows; col++)
                        {
                            if ((flags[col] & flagNLB) != 0 && (flags[col] & flagLB) == 0) return new double[cols + 1];
                            else if ((flags[col] & flagNUB) != 0 && (flags[col] & flagUB) == 0) return new double[cols + 1];
                        }
                        phaseI = false;
                        ComputePrimalSolution(phaseI);
                        goto DECOMPOSE;
                    }
                    // refine solution
                    //ComputePrimalSolution();
                    ColdStart = false;
                    //System.Diagnostics.Trace.WriteLine("Dual optimal in " + round);
                    return ComputeReturnSolution();
                }

                int minj;
                double minr;

                if (!ComputeDualPivotRow(phaseI, mini))
                {
                    Decompose();
                    //System.Diagnostics.Trace.WriteLine("Feasibility lost in " + round + ", conditioning " + U[rows * rows - 1]);
                    if (lu.Singular)
                    {
                        PatchSingularBasis();
                        Decompose();
                    }
                    redecompose = maxRedecompose;
                    ComputeReducedCosts();
                    phaseI = BoundFlip(phaseI, 0.0, epsDualI, -1, true);
                    if (phaseI)
                    {
                        ComputePrimalSolution(phaseI);
                        BoundFlip(phaseI, 0.0, epsDualI, -1, true);
                    }
                    goto DECOMPOSE;
                }

                //SelectDualIncoming(phaseI, mini, out minj, out minr);
                SelectDualIncomingWithBoundFlips(phaseI, delta, out minj, out minr);

                if (minj == -1)
                {
                    if (redecompose == maxRedecompose)
                    {
                        // unfeasible, return null solution, don't pursue this branch because no solution exists here
                        double[] ret = new double[cols + 1];
                        dualUnbounded = true;
                        // debug measure, verify primal infeasibility by solving the primal
                        //ret = SolvePrimalUnsafe(true, false, false);
                        //System.Diagnostics.Trace.WriteLine("Dual unfeasible after " + round);
                        return ret;
                    }
                    else
                    {
                        // make a clean factorization to make sure we're really unbounded
                        redecompose = 0;
                        needsRecalc = true;
                        continue;
                    }
                }

                // shifting to prevent numerical cycling
                if (minr * delta > 0)
                {
                    //System.Diagnostics.Debug.WriteLine("Value shifted in round " + round);
                    if (delta > 0)
                    {
                        minr = -1.0e-12;
                    }
                    else
                    {
                        minr = 1.0e-12;
                    }
                    double shift = minr * wd[minj] - c[minj];
                    c[minj] += shift;
                    cost[V[minj]] += shift; // requires working copy for cost
                    costWorkingDirty = true;
                    shifted = true;
                    // NLB: *cj < eps, NUB: *cj > -eps
                    // update goes *cj -= minr * *wdj
                    for (int j = 0; j < cols; j++)
                    {
                        if (j != minj)
                        {
                            int col = V[j];
                            if ((flags[col] & flagNLB) != 0)
                            {
                                if (c[j] - minr * wd[j] > epsDual)
                                {
                                    shift = epsDual - c[j] + minr * wd[j];
                                    c[j] += shift;
                                    cost[col] += shift; // requires working copy for cost
                                }
                            }
                            else if ((flags[col] & flagNUB) != 0)
                            {
                                if (c[j] - minr * wd[j] < -epsDual)
                                {
                                    shift = -epsDual - c[j] + minr * wd[j];
                                    c[j] += shift;
                                    cost[col] += shift; // requires working copy for cost
                                }
                            }
                        }
                    }
                }

                BoundFlip(phaseI, minr, phaseI ? epsDualI : epsDual, minj, false);
                UpdateDual(phaseI, minj, mini, minr, delta, false); // use true if using simple ratio test

                redecompose--;
                if (redecompose > 0)
                {
                    double pivot;
                    lu.Update(ww, mini, out pivot);
                    if (lu.Singular || Math.Abs(wd[minj] - pivot) > 1.0e-6 * Math.Abs(wd[minj]))
                    {
                        if (redecompose == maxRedecompose - 1)
                        {
                            // if we don't allow blacklisting we'll either go on if not singular or patch the basis
                            if (blacklistAllowed)
                            {
                                flags[B[mini]] |= flagDis;
                                disabledDirty = true;
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

                //System.Diagnostics.Debug.WriteLine("Round " + round + ", swap " + B[mini] + " out, " + V[minj] + " in, value change estimate " + delta * minr);

                // swap base
                UpdateBounds();
                k = B[mini];
                B[mini] = V[minj];
                V[minj] = k;
                flags[B[mini]] = (flags[B[mini]] | flagB) & ~flagN;
                flags[V[minj]] = (flags[V[minj]] | bound) & ~flagB;

                round++;
            } while (round < 5000);
            // just in case
            return new double[cols + 1];
        }
    }
}
