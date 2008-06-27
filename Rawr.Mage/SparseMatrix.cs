using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Mage
{
    // only one should exist at a time, otherwise behavior unspecified
    public unsafe class SparseMatrix
    {
        internal static double[] value;
        internal static int[] row;
        internal static int[] col;
        internal static double[] data; // still store the dense version, memory is cheap and it speeds some stuff up
        private static int maxRows = 0;
        private static int maxCols = 0;
        private static int sparseSize;

        private double* pData;
        internal double* pValue;
        private int* pRow;
        private int* pCol;

        public void BeginUnsafe(double* pData, double* pValue, int* pRow, int* pCol)
        {
            this.pData = pData;
            this.pValue = pValue;
            this.pRow = pRow;
            this.pCol = pCol;
        }

        public void EndUnsafe()
        {
            pData = null;
            pValue = null;
            pRow = null;
            pCol = null;
        }

        static SparseMatrix()
        {
            maxRows = 200;
            maxCols = 5000;
            RecreateArrays();
        }

        private static void RecreateArrays()
        {
            sparseSize = Math.Max(sparseSize, (int)(maxRows * maxCols * 0.4));
            value = new double[sparseSize];
            row = new int[sparseSize];
            col = new int[maxCols + 1];
            data = new double[maxRows * maxCols];
        }

        private int cols;
        private int rows;
        private int sparseIndex = 0;
        private int lastCol = 0;

        private bool finalized;

        public bool ReadOnly
        {
            get
            {
                return finalized;
            }
        }

        public int Columns
        {
            get
            {
                return cols;
            }
        }

        public int Rows
        {
            get
            {
                return rows;
            }
        }

        public SparseMatrix(int rows, int maxCols)
        {
            if (rows > maxRows || maxCols + 20 > SparseMatrix.maxCols)
            {
                maxRows = Math.Max(rows, maxRows);
                SparseMatrix.maxCols = Math.Max(maxCols + 20, SparseMatrix.maxCols); // give some room for AddColumn
                RecreateArrays();
            }
            this.rows = rows;
            this.cols = 0;
            //Array.Clear(data, 0, rows * cols); // only need to clear what will be used
        }

        public double this[int row, int col]
        {
            get
            {
                return data[col * rows + row];
            }
            set
            {
                if (finalized) throw new InvalidOperationException();
                if (col < lastCol) throw new ArgumentException();
                if (value == 0.0) return;
                if (col > lastCol)
                {
                    for (int c = lastCol + 1; c <= col; c++)
                    {
                        SparseMatrix.col[c] = sparseIndex;
                    }
                }
                if (sparseIndex >= sparseSize)
                {
                    sparseSize += (int)(rows * cols * 0.1);
                    int[] newRow = new int[sparseSize];
                    Array.Copy(SparseMatrix.row, newRow, SparseMatrix.row.Length);
                    SparseMatrix.row = newRow;
                    double[] newValue = new double[sparseSize];
                    Array.Copy(SparseMatrix.value, newValue, SparseMatrix.value.Length);
                    SparseMatrix.value = newValue;
                }
                SparseMatrix.row[sparseIndex] = row;
                SparseMatrix.value[sparseIndex] = value;
                data[col * rows + row] = value; // store data by columns, we always access by column so it will have better locality, we also never increase number of rows, only columns, that way we don't have to reposition data
                lastCol = col;
                sparseIndex++;
            }
        }

        public void SetElementUnsafe(int row, int col, double value)
        {
            if (pValue == null)
            {
                this[row, col] = value;
                return;
            }
            if (value == 0.0) return;
            if (col > lastCol)
            {
                for (int c = lastCol + 1; c <= col; c++)
                {
                    pCol[c] = sparseIndex;
                }
            }
            if (sparseIndex >= sparseSize)
            {
                // C# does not allow to change a pinned reference, if we have to recreate arrays we have to move to safe code
                EndUnsafe();
                sparseSize += (int)(rows * cols * 0.1);
                int[] newRow = new int[sparseSize];
                Array.Copy(SparseMatrix.row, newRow, SparseMatrix.row.Length);
                SparseMatrix.row = newRow;
                double[] newValue = new double[sparseSize];
                Array.Copy(SparseMatrix.value, newValue, SparseMatrix.value.Length);
                SparseMatrix.value = newValue;
                SparseMatrix.row[sparseIndex] = row;
                SparseMatrix.value[sparseIndex] = value;
                data[col * rows + row] = value; // store data by columns, we always access by column so it will have better locality, we also never increase number of rows, only columns, that way we don't have to reposition data
                lastCol = col;
                sparseIndex++;
                return;
            }
            pRow[sparseIndex] = row;
            pValue[sparseIndex] = value;
            pData[col * rows + row] = value; // store data by columns, we always access by column so it will have better locality, we also never increase number of rows, only columns, that way we don't have to reposition data
            lastCol = col;
            sparseIndex++;
        }

        public int AddColumn()
        {
            finalized = false;
            cols++;
            if (cols > maxCols)
            {
                throw new InvalidOperationException();
            }
            Array.Clear(data, rows * (cols - 1), rows); // only need to clear what will be used
            return cols - 1;
        }

        public void EndConstruction()
        {
            for (int c = lastCol + 1; c <= cols; c++)
            {
                SparseMatrix.col[c] = sparseIndex;
            }            
            finalized = true;
        }
    }
}
