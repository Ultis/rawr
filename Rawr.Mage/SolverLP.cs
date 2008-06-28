using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Mage
{
    public unsafe sealed class SolverLP : IComparable<SolverLP>
    {
        private int cRows;
        private int cCols;
        public LP lp;
        internal double[] rowScale;
        double[] compactSolution = null;
        bool needsDual;
        //public string Log = string.Empty;
        //public int[] disabledHex;

        double* pRowScale;
        double* pCost;
        double* pB;
        SparseMatrix sparseMatrix;

        public void BeginUnsafe(double* pRowScale, double* pCost, double* pB, double* pData, double* pValue, int* pRow, int* pCol)
        {
            this.pRowScale = pRowScale;
            this.pCost = pCost;
            this.pB = pB;
            sparseMatrix = lp.A;
            sparseMatrix.BeginUnsafe(pData, pValue, pRow, pCol);
        }

        public void EndUnsafe()
        {
            sparseMatrix.EndUnsafe();
            sparseMatrix = null;
            pRowScale = null;
            pCost = null;
            pB = null;           
        }

        public void ForceRecalculation()
        {
            compactSolution = null;
        }

        public SolverLP Clone()
        {
            //if (compactSolution != null && !allowReuse) throw new InvalidOperationException();
            SolverLP clone = (SolverLP)this.MemberwiseClone();
            clone.compactSolution = null;
            //clone.lp = (double[,])clone.lp.Clone();
            clone.lp = lp.Clone();
            //if (disabledHex != null) clone.disabledHex = (int[])disabledHex.Clone();
            return clone;
        }

        public SolverLP(int commonRows, int maximumColumns)
        {
            cRows = commonRows;
            cCols = 0;

            rowScale = new double[cRows + 1];
            for (int i = 0; i <= cRows; i++)
            {
                rowScale[i] = 1.0;
            }

            lp = new LP(cRows, maximumColumns);
        }

        public void SetRowScale(int row, double value)
        {
            if (row == -1) return;
            rowScale[row] = value;
        }

        public void SetRowScaleUnsafe(int row, double value)
        {
            if (row == -1) return;
            pRowScale[row] = value;
        }

        public int AddColumn()
        {
            cCols++;
            return lp.AddColumn();
        }

        public int AddColumnUnsafe()
        {
            pCost[cCols++] = 0.0;
            lp.cols++;
            return sparseMatrix.AddColumn();
        }

        public double this[int row, int col]
        {
            get
            {
                if (row == -1 || col == -1) return 0;
                return lp[row, col] / rowScale[row];
            }
            set
            {
                if (row == -1 || col == -1) return;
                lp[row, col] = value * rowScale[row];
                compactSolution = null;
            }
        }

        public void SetElement(int row, int col, double value)
        {
            this[row, col] = value;
        }

        public void SetElementUnsafe(int row, int col, double value)
        {
            if (row == -1) return;
            sparseMatrix.SetElementUnsafe(row, col, value * pRowScale[row]);
        }

        public void SetRHS(int row, double value)
        {
            if (row == -1) return;
            lp.SetRHS(row, value * rowScale[row]);
        }

        public void SetRHSUnsafe(int row, double value)
        {
            if (row == -1) return;
            pB[row] = value * pRowScale[row];
        }

        public void SetCost(int col, double value)
        {
            if (col == -1) return;
            lp.SetCost(col, value * rowScale[cRows]);
        }

        public void SetCostUnsafe(int col, double value)
        {
            pCost[col] = value * pRowScale[cRows];
        }

        private int rowDisableColumn = -1;
        private int rowMaximizeSegment = -1;
        private int rowColdsnap = -1;

        public void EraseColumn(int col)
        {
            if (col == -1) return;
            if (rowDisableColumn == -1) rowDisableColumn = lp.AddConstraint();
            lp.SetConstraintElement(rowDisableColumn, col, 1.0);
            compactSolution = null;
            needsDual = true;
        }

        public void UpdateMaximizeSegmentColumn(int col)
        {
            if (col == -1) return;
            if (rowMaximizeSegment == -1) rowMaximizeSegment = lp.AddConstraint();
            lp.SetConstraintElement(rowMaximizeSegment, col, lp.GetConstraintElement(rowMaximizeSegment, col) - 1.0);
            compactSolution = null;
            needsDual = true;
        }

        public void UpdateMaximizeSegmentDuration(double value)
        {
            if (rowMaximizeSegment == -1) rowMaximizeSegment = lp.AddConstraint();
            lp.SetConstraintRHS(rowMaximizeSegment, lp.GetConstraintRHS(rowMaximizeSegment) - value);
            compactSolution = null;
            needsDual = true;
        }

        public bool HasColdsnapConstraints
        {
            get
            {
                return rowColdsnap != -1;
            }
        }

        public void AddColdsnapConstraints(int segments)
        {
            if (rowColdsnap == -1)
            {
                rowColdsnap = lp.AddConstraint();
                for (int i = 1; i < segments; i++) lp.AddConstraint();
            }
        }

        public void UpdateColdsnapColumn(int index, int col)
        {
            compactSolution = null;
            needsDual = true;
            if (col == -1) return;
            lp.SetConstraintElement(rowColdsnap + index, col, 1.0);
        }

        public void UpdateColdsnapDuration(int index, double value)
        {
            compactSolution = null;
            needsDual = true;
            lp.SetConstraintRHS(rowColdsnap + index, value);
        }

        private void SolveInternal()
        {
            if (compactSolution == null)
            {
                lp.EndConstruction();
                if (needsDual)
                {
                    //System.Diagnostics.Debug.WriteLine("Solving H=" + HeroismHash.ToString("X") + ", AP=" + APHash.ToString("X") + ", IV=" + IVHash.ToString("X"));
                    compactSolution = lp.SolveDual();
                }
                else
                {
                    compactSolution = lp.SolvePrimal();
                }
                compactSolution[cCols] /= rowScale[cRows];
            }
        }

        public double[] Solve()
        {
            SolveInternal();
            return compactSolution;
        }

        public void SolvePrimalDual()
        {
            lp.EndConstruction();
            lp.SolvePrimal();
            compactSolution = lp.SolveDual();
        }

        public double Value
        {
            get
            {
                SolveInternal();
                return compactSolution[compactSolution.Length - 1];
            }
        }

        int IComparable<SolverLP>.CompareTo(SolverLP other)
        {
            return this.Value.CompareTo(other.Value);
        }
    }
}
