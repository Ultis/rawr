using Rawr;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Rawr.UnitTests
{
    
    
    /// <summary>
    ///This is a test class for AttackTest and is intended
    ///to contain all AttackTest Unit Tests
    ///</summary>
    [TestClass()]
    public class AttackTest
    {


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
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for AvoidableBy
        ///</summary>
        [TestMethod()]
        public void AvoidableByTest()
        {
            Attack target = new Attack(); // Default is it avoidable by all.
            bool expected = true; 
            bool actual;
            foreach (AVOIDANCE_TYPES at in Enum.GetValues(typeof(AVOIDANCE_TYPES)))
            {
                foreach (AVOIDANCE_TYPES at2 in Enum.GetValues(typeof(AVOIDANCE_TYPES)))
                {
                    if ((at | at2) == AVOIDANCE_TYPES.None) expected = false;
                    else expected = true;
                    actual = target.AvoidableBy(at | at2);
                    Assert.AreEqual(expected, actual, Enum.GetName(typeof(AVOIDANCE_TYPES), at | at2));
                }
            }
            target.SetUnavoidable();
            foreach (AVOIDANCE_TYPES at in Enum.GetValues(typeof(AVOIDANCE_TYPES)))
            {
                foreach (AVOIDANCE_TYPES at2 in Enum.GetValues(typeof(AVOIDANCE_TYPES)))
                {
                    if ((at | at2) == AVOIDANCE_TYPES.None) expected = true;
                    else expected = false;
                    actual = target.AvoidableBy(at | at2);
                    Assert.AreEqual(expected, actual, Enum.GetName(typeof(AVOIDANCE_TYPES), at | at2));
                }
            }
            // Set up individual flag values.
            target.Missable = true;
            foreach (AVOIDANCE_TYPES at in Enum.GetValues(typeof(AVOIDANCE_TYPES)))
            {
                foreach (AVOIDANCE_TYPES at2 in Enum.GetValues(typeof(AVOIDANCE_TYPES)))
                {
                    if ((at | at2) == AVOIDANCE_TYPES.Miss) expected = true;
                    else expected = false;
                    actual = target.AvoidableBy(at | at2);
                    Assert.AreEqual(expected, actual, Enum.GetName(typeof(AVOIDANCE_TYPES), at | at2));
                }
            }
            target.SetUnavoidable();

            target.Dodgable = true;
            foreach (AVOIDANCE_TYPES at in Enum.GetValues(typeof(AVOIDANCE_TYPES)))
            {
                foreach (AVOIDANCE_TYPES at2 in Enum.GetValues(typeof(AVOIDANCE_TYPES)))
                {
                    if ((at | at2) == AVOIDANCE_TYPES.Dodge) expected = true;
                    else expected = false;
                    actual = target.AvoidableBy(at | at2);
                    Assert.AreEqual(expected, actual, Enum.GetName(typeof(AVOIDANCE_TYPES), at | at2));
                }
            }
            target.SetUnavoidable();

            target.Parryable = true;
            foreach (AVOIDANCE_TYPES at in Enum.GetValues(typeof(AVOIDANCE_TYPES)))
            {
                foreach (AVOIDANCE_TYPES at2 in Enum.GetValues(typeof(AVOIDANCE_TYPES)))
                {
                    if ((at | at2) == AVOIDANCE_TYPES.Parry) expected = true;
                    else expected = false;
                    actual = target.AvoidableBy(at | at2);
                    Assert.AreEqual(expected, actual, Enum.GetName(typeof(AVOIDANCE_TYPES), at | at2));
                }
            }
            target.SetUnavoidable();

            target.Blockable = true;
            foreach (AVOIDANCE_TYPES at in Enum.GetValues(typeof(AVOIDANCE_TYPES)))
            {
                foreach (AVOIDANCE_TYPES at2 in Enum.GetValues(typeof(AVOIDANCE_TYPES)))
                {
                    if ((at | at2) == AVOIDANCE_TYPES.Block) expected = true;
                    else expected = false;
                    actual = target.AvoidableBy(at | at2);
                    Assert.AreEqual(expected, actual, Enum.GetName(typeof(AVOIDANCE_TYPES), at | at2));
                }
            }
            target.SetUnavoidable();
        }

        /// <summary>
        ///A test for Avoidable
        ///</summary>
        [TestMethod()]
        public void AvoidableTest()
        {
            Attack target = new Attack();
            bool actual;
            actual = target.Avoidable;
            Assert.IsTrue(actual);
            target.SetUnavoidable();
            actual = target.Avoidable;
            Assert.IsFalse(actual);
        }

        /// <summary>
        ///A test for Blockable
        ///</summary>
        [TestMethod()]
        public void BlockableTest()
        {
            Attack target = new Attack();
            bool expected = true;
            bool actual;
            actual = target.Blockable;
            Assert.AreEqual(expected, actual, "Block default");
            expected = false;
            target.Blockable = expected;
            actual = target.Blockable;
            Assert.AreEqual(expected, actual, "Block false");
        }

        /// <summary>
        ///A test for Dodgable
        ///</summary>
        [TestMethod()]
        public void DodgableTest()
        {
            Attack target = new Attack();
            bool expected = true;
            bool actual;
            actual = target.Dodgable;
            Assert.AreEqual(expected, actual, "Dodge default");
            expected = false;
            target.Dodgable = expected;
            actual = target.Dodgable;
            Assert.AreEqual(expected, actual, "Dodge false");
        }

        /// <summary>
        ///A test for Missable
        ///</summary>
        [TestMethod()]
        public void MissableTest()
        {
            Attack target = new Attack();
            bool expected = true;
            bool actual;
            actual = target.Missable;
            Assert.AreEqual(expected, actual, "Miss default");
            expected = false;
            target.Missable = expected;
            actual = target.Missable;
            Assert.AreEqual(expected, actual, "Misss false");
        }

        /// <summary>
        ///A test for Parryable
        ///</summary>
        [TestMethod()]
        public void ParryableTest()
        {
            Attack target = new Attack();
            bool expected = true;
            bool actual;
            actual = target.Parryable;
            Assert.AreEqual(expected, actual, "Parry default");
            expected = false;
            target.Parryable = expected;
            actual = target.Parryable;
            Assert.AreEqual(expected, actual, "Parry false");
        }
    }
}
