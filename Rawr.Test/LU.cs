using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Rawr.Test
{
    /// <summary>
    /// Summary description for LU
    /// </summary>
    [TestClass]
    public class LU
    {
        public LU()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void TestDecompose()
        {
            for (int size = 1; size < 100; size++)
            {
                for (int run = 0; run < 10; run++)
                {
                    TestDecomposeSize(size);
                }
            }
        }

        [TestMethod]
        public void TestFSolve()
        {
            for (int size = 1; size < 100; size++)
            {
                for (int run = 0; run < 10; run++)
                {
                    TestFSolveSize(size);
                }
            }
        }

        [TestMethod]
        public void TestBSolve()
        {
            for (int size = 1; size < 100; size++)
            {
                for (int run = 0; run < 10; run++)
                {
                    TestBSolveSize(size);
                }
            }
        }

        private void TestDecomposeSize(int size)
        {
            Random rnd = new Random();
            double[,] B = new double[size, size];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    B[i, j] = rnd.NextDouble();
                }
            }
            double[,] B0 = (double[,])B.Clone();
            Mage.LU2 lu = new Rawr.Mage.LU2(B, size);
            lu.Decompose();
            for (int j = 0; j < size; j++)
            {
                // Ln...L0 B = P U Q
                // left hand side
                double[] L = new double[size];
                for (int i = 0; i < size; i++)
                {
                    L[i] = B0[i, j];
                }
                for (int k = 0; k < lu.etaSize; k++)
                {
                    int row = lu._LI[k]; // we're updating using row, if element is zero we can skip
                    // b~ = b + eta (erow' b)
                    double f = L[row];
                    if (Math.Abs(f) >= 0.000001)
                    {
                        for (int i = 0; i < size; i++)
                        {
                            L[i] += f * lu._L[k * size + i];
                        }
                    }
                }
                // right hand side
                double[] R = new double[size];
                int jj = j;
                for (jj = 0; jj < size; jj++)
                {
                    if (lu._Q[jj] == j) break;
                }
                for (int i = 0; i < size; i++)
                {
                    R[lu._P[i]] = lu._U[i, jj]; // DON'T CHANGE THIS!!! THIS IS HOW IT REALLY SHOULD BE!!!!1!!
                }
                for (int i = 0; i < size; i++)
                {
                    Assert.AreEqual(L[i], R[i], 0.00001, "Size = " + size);
                }
            }
        }

        private void TestFSolveSize(int size)
        {
            Random rnd = new Random();
            double[,] B = new double[size, size];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    B[i, j] = rnd.NextDouble();
                }
            }
            double[] x = new double[size];
            for (int i = 0; i < size; i++)
            {
                x[i] = rnd.NextDouble();
            }
            double[] b = new double[size];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    b[i] += B[i, j] * x[j];
                }
            }
            Mage.LU2 lu = new Rawr.Mage.LU2(B, size);
            lu.Decompose();
            unsafe
            {
                fixed (double* _b = b)
                {
                    lu.FSolve(_b);
                }
            }
            for (int i = 0; i < size; i++)
            {
                Assert.AreEqual(x[i], b[i], 0.000001, "Size = " + size);
            }
        }

        private void TestBSolveSize(int size)
        {
            Random rnd = new Random();
            double[,] B = new double[size, size];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    B[i, j] = rnd.NextDouble();
                }
            }
            double[] x = new double[size];
            for (int i = 0; i < size; i++)
            {
                x[i] = rnd.NextDouble();
            }
            double[] b = new double[size];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    b[i] += B[j, i] * x[j];
                }
            }
            Mage.LU2 lu = new Rawr.Mage.LU2(B, size);
            lu.Decompose();
            unsafe
            {
                fixed (double* _b = b)
                {
                    lu.BSolve(_b);
                }
            }
            for (int i = 0; i < size; i++)
            {
                Assert.AreEqual(x[i], b[i], 0.000001, "Size = " + size);
            }
        }
    }
}
