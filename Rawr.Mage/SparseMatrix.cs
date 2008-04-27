using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Mage
{
    public class SparseMatrix
    {
        internal double[] value;
        internal int[] row;
        internal int[] col;
        private int cols;
        private int rows;
        private int sparseSize;
        private int sparseIndex;
        private int lastCol = 0;

        internal double[,] data; // still store the dense version, memory is cheap and it speeds some stuff up

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

        public SparseMatrix(int rows, int cols)
        {
            this.rows = rows;
            this.cols = cols;
            sparseSize = (int)(rows * cols * 0.4);
            value = new double[sparseSize];
            row = new int[sparseSize];
            col = new int[cols + 1];
            data = new double[rows, cols];
        }

        public double this[int row, int col]
        {
            get
            {
                return data[row, col];
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
                        this.col[c] = sparseIndex;
                    }
                }
                if (sparseIndex >= sparseSize)
                {
                    sparseSize += (int)(rows * cols * 0.1);
                    int[] newRow = new int[sparseSize];
                    Array.Copy(this.row, newRow, this.row.Length);
                    this.row = newRow;
                    double[] newValue = new double[sparseSize];
                    Array.Copy(this.value, newValue, this.value.Length);
                    this.value = newValue;
                }
                this.row[sparseIndex] = row;
                this.value[sparseIndex] = value;
                data[row, col] = value;
                lastCol = col;
                sparseIndex++;
            }
        }

        public void EndConstruction()
        {
            for (int c = lastCol + 1; c <= cols; c++)
            {
                this.col[c] = sparseIndex;
            }            
            finalized = true;
        }
    }
}
