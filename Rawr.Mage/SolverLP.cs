using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Mage
{
    public unsafe sealed class SolverLP : IComparable<SolverLP>
    {
        private int cRows;
        private int cCols;
        private CharacterCalculationsMage calculations;
        private LP lp;
        private double[] compactSolution = null;
        private bool needsDual;
        private int segments;
        public StringBuilder Log = new StringBuilder();
        //public int[] disabledHex;

        private double* pRowScale;
        private double* pColumnScale;
        private double* pCost;
        private SparseMatrix sparseMatrix;

        private static int maxRows = 0;
        private static int maxCols = 0;
        internal static double[] rowScale;
        internal static double[] columnScale;

        static SolverLP()
        {
            maxRows = 200;
            maxCols = 5000;
            RecreateArrays();
        }

        private static void RecreateArrays()
        {
            rowScale = new double[maxRows];
            columnScale = new double[maxCols];
        }

        public void BeginUnsafe(double* pRowScale, double* pColumnScale, double* pCost, double* pData, double* pValue, int* pRow, int* pCol)
        {
            this.pRowScale = pRowScale;
            this.pColumnScale = pColumnScale;
            this.pCost = pCost;
            sparseMatrix = lp.A;
            sparseMatrix.BeginUnsafe(pData, pValue, pRow, pCol);
        }

        public void EndUnsafe()
        {
            sparseMatrix.EndUnsafe();
            sparseMatrix = null;
            pRowScale = null;
            pCost = null;
        }

        public void EndColumnConstruction()
        {
            lp.EndColumnConstruction();
        }

        public void ForceRecalculation(bool dual)
        {
            compactSolution = null;
            needsDual = dual;
        }

        public void ForceRecalculation()
        {
            compactSolution = null;
            needsDual = false;
        }

        public SolverLP Clone()
        {
            //if (compactSolution != null && !allowReuse) throw new InvalidOperationException();
            SolverLP clone = (SolverLP)this.MemberwiseClone();
            clone.compactSolution = null;
            clone.lp = lp.Clone();
            if (Log != null) clone.Log = new StringBuilder(Log.ToString());
            //if (disabledHex != null) clone.disabledHex = (int[])disabledHex.Clone();
            return clone;
        }

        public SolverLP(int baseRows, int maximumColumns, CharacterCalculationsMage calculations, int segments)
        {
            if (baseRows > maxRows || maximumColumns > maxCols)
            {
                maxRows = Math.Max(baseRows, maxRows);
                maxCols = Math.Max(maximumColumns, maxCols);
                RecreateArrays();
            }

            this.calculations = calculations;
            this.segments = segments;
            cRows = baseRows;
            cCols = 0;

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

        public void SetColumnScale(int col, double value)
        {
            columnScale[col] = value;
        }

        public void SetColumnScaleUnsafe(int col, double value)
        {
            pColumnScale[col] = value;
        }

        public int AddColumn()
        {
            columnScale[cCols] = 1.0;
            cCols++;
            return lp.AddColumn();
        }

        public int AddColumnUnsafe()
        {
            pColumnScale[cCols] = 1.0;
            cCols++;
            return lp.AddColumn();
        }

        public double this[int row, int col]
        {
            get
            {
                if (row == -1 || col == -1) return 0;
                return lp[row, col] / rowScale[row] / columnScale[col];
            }
            set
            {
                if (row == -1 || col == -1) return;
                lp[row, col] = value * rowScale[row] * columnScale[col];
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
            sparseMatrix.SetElementUnsafe(row, col, value * pRowScale[row] * pColumnScale[col]);
        }

        public void SetRHS(int row, double value)
        {
            if (row == -1) return;
            lp.SetRHS(row, value * rowScale[row]);
        }

        public void SetLHS(int row, double value)
        {
            if (row == -1) return;
            lp.SetLHS(row, value * rowScale[row]);
        }

        public void SetRHSUnsafe(int row, double value)
        {
            if (row == -1) return;
            lp.SetRHS(row, value * pRowScale[row]);
        }

        public void SetLHSUnsafe(int row, double value)
        {
            if (row == -1) return;
            lp.SetLHS(row, value * pRowScale[row]);
        }

        public void SetCost(int col, double value)
        {
            if (col == -1) return;
            lp.SetCost(col, value * rowScale[cRows] * columnScale[col]);
        }

        public void SetCostUnsafe(int col, double value)
        {
            pCost[col] = value * pRowScale[cRows] * pColumnScale[col];
        }

        public void SetColumnLowerBound(int col, double value)
        {
            lp.SetLowerBound(col, value / columnScale[col]);
        }

        public void SetColumnUpperBound(int col, double value)
        {
            lp.SetUpperBound(col, value / columnScale[col]);
        }

        private int rowMaximizeSegment = -1;
        private int rowColdsnap = -1;

        public void EraseColumn(int col)
        {
            if (col == -1) return;
            lp.SetUpperBound(col, 0.0);
            compactSolution = null;
            needsDual = true;
        }

        public void UpdateMaximizeSegmentColumn(int col)
        {
            if (col == -1) return;
            if (rowMaximizeSegment == -1) rowMaximizeSegment = lp.AddConstraint(true);
            lp.SetConstraintElement(rowMaximizeSegment, col, lp.GetConstraintElement(rowMaximizeSegment, col) - 1.0 * columnScale[col]);
            compactSolution = null;
            needsDual = true;
        }

        public void UpdateMaximizeSegmentDuration(double value)
        {
            if (rowMaximizeSegment == -1) rowMaximizeSegment = lp.AddConstraint(true);
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

        public int AddConstraint(bool editable)
        {
            return lp.AddConstraint(editable);
        }

        public void SetConstraintRHS(int index, double value)
        {
            lp.SetConstraintRHS(index, value);
        }

        public void SetConstraintLHS(int index, double value)
        {
            lp.SetConstraintLHS(index, value);
        }

        public void SetConstraintElement(int index, int col, double value)
        {
            lp.SetConstraintElement(index, col, columnScale[col] * value);
        }

        public void AddColdsnapConstraints(int segments)
        {
            if (rowColdsnap == -1)
            {
                rowColdsnap = lp.AddConstraint(true);
                for (int i = 1; i < segments; i++) lp.AddConstraint(true);
            }
        }

        public void UpdateColdsnapColumn(int index, int col)
        {
            compactSolution = null;
            needsDual = true;
            if (col == -1) return;
            lp.SetConstraintElement(rowColdsnap + index, col, 1.0 * columnScale[col]);
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
                if (needsDual)
                {
                    //System.Diagnostics.Trace.WriteLine("Solving " + Log.ToString());
                    //System.Diagnostics.Debug.WriteLine("Solving H=" + HeroismHash.ToString("X") + ", AP=" + APHash.ToString("X") + ", IV=" + IVHash.ToString("X"));
                    compactSolution = lp.SolveDual(false);
                }
                else
                {
                    compactSolution = lp.SolvePrimal(false);
                    //compactSolution = lp.SolveDual(true);
                }
                UnscaleSolution();
            }
        }

        private void UnscaleSolution()
        {
            compactSolution[compactSolution.Length - 1] /= rowScale[cRows];
            for (int i = 0; i < compactSolution.Length - 1; i++)
            {
                compactSolution[i] *= columnScale[i];
            }
        }

        public double[] Solve()
        {
            SolveInternal();
            return compactSolution;
        }

        public void SolvePrimalDual()
        {
            lp.SolvePrimal(true);
            //lp.SolveDual(true);
            compactSolution = lp.SolveDual(true);
            UnscaleSolution();
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

        public override string ToString()
        {
            if (compactSolution == null) return base.ToString();
            return "LP: " + compactSolution[compactSolution.Length - 1].ToString();
        }

        public string Listing
        {
            get
            {
                if (compactSolution == null) return "";
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < calculations.SolutionVariable.Count; i++)
                {
                    if (compactSolution[i] > 0.01)
                    {
                        if (calculations.SolutionVariable[i].Type == VariableType.Spell)
                        {
                            sb.AppendLine(String.Format("{2} {0}: {1:F}", calculations.SolutionVariable[i].State.BuffLabel + "+" + calculations.SolutionVariable[i].Spell.Name, compactSolution[i], calculations.SolutionVariable[i].Segment));
                        }
                        else
                        {
                            sb.AppendLine(String.Format("{2} {0}: {1:F}", calculations.SolutionVariable[i].Type, compactSolution[i], calculations.SolutionVariable[i].Segment));
                        }
                    }
                }
                return sb.ToString();
            }
        }
    }
}
