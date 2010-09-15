using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Rawr.Moonkin
{
    // A class designed specifically to perform matrix operations for solving a simple system of linear equations (2 variables)
    public class SimpleFastMatrix
    {
        private float[] matrix;
        private int rows, columns;

        public int Rows { get { return rows; } }
        public int Columns { get { return columns; } }

        public SimpleFastMatrix(float[] inputData, int numCols)
        {
            matrix = inputData;
            columns = numCols;
            rows = inputData.Length / numCols;
        }

        public SimpleFastMatrix Invert()
        {
            if (rows == 2 && columns == 2)
            {
                float determinant = matrix[0] * matrix[3] - matrix[1] * matrix[2];
                float scalarFactor = 1 / determinant;

                return new SimpleFastMatrix(new float[] { scalarFactor * matrix[3], scalarFactor * -matrix[1], scalarFactor * -matrix[2], scalarFactor * matrix[0] }, 2);
            }
            throw new NotImplementedException("Matrices other than 2x2 are not supported in this implementation");
        }

        public SimpleFastMatrix MatrixMultiply(SimpleFastMatrix rightSide)
        {
            if (this.Columns != rightSide.Rows)
                throw new InvalidOperationException("Matrix dimensions are incompatible for multiplication");

            float[] returnData = new float[this.Rows * rightSide.Columns];

            for (int pos = 0; pos < returnData.Length; ++pos)
            {
                int row = pos / rightSide.Columns;
                int col = pos % rightSide.Columns;

                for (int i = 0; i < this.Columns; ++i)
                {
                    returnData[pos] += matrix[row * this.Columns + i] * rightSide.matrix[i * this.Columns + col];
                }
            }

            return new SimpleFastMatrix(returnData, rightSide.Columns);
        }
    }
}
