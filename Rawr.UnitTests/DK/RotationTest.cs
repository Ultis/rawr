using Rawr.DK;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Rawr.UnitTests.DK
{
    
    
    /// <summary>
    ///This is a test class for DKRotationTest and is intended
    ///to contain all DKRotationTest Unit Tests
    ///</summary>
    [TestClass()]
    public class DKRotationTest
    {
        int[] m_CurrentAbilityStatus = new int[EnumHelper.GetCount(typeof(DKCostTypes))];
        int[] m_Update = new int[EnumHelper.GetCount(typeof(DKCostTypes))];
        int[] m_ExpectedAbilityStatus = new int[EnumHelper.GetCount(typeof(DKCostTypes))];

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
        [TestInitialize()]
        public void MyTestInitialize()
        {
            // Reset each structure to 0
            foreach (int i in EnumHelper.GetValues(typeof(DKCostTypes)))
            {
                m_CurrentAbilityStatus[i] = 0;
                m_Update[i] = 0;
                m_ExpectedAbilityStatus[i] = 0;
            }
        }
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for ProcessRunningRunes
        ///</summary>
        [TestMethod()]
        public void ProcessRunningRunesTest_zero()
        {
            bool r = Rotation.ProcessRunningRunes(m_CurrentAbilityStatus, m_Update);
            Assert.IsTrue(r);
        }

        /// <summary>
        ///A test for ProcessRunningRunes
        ///</summary>
        [TestMethod()]
        public void ProcessRunningRunesTest_NothingAvailable()
        {
            foreach (int i in EnumHelper.GetValues(typeof(DKCostTypes)))
                m_Update[i] = 1;
            bool r = Rotation.ProcessRunningRunes(m_CurrentAbilityStatus, m_Update);
            Assert.IsFalse(r);
        }

        /// <summary>
        ///A test for ProcessRunningRunes
        ///</summary>
        [TestMethod()]
        public void ProcessRunningRunesTest_1RuneEach()
        {
            m_CurrentAbilityStatus[(int)DKCostTypes.Blood] = 2;
            m_CurrentAbilityStatus[(int)DKCostTypes.Frost] = 2;
            m_CurrentAbilityStatus[(int)DKCostTypes.UnHoly] = 2;

            m_ExpectedAbilityStatus[(int)DKCostTypes.Blood] = 2;
            m_ExpectedAbilityStatus[(int)DKCostTypes.Frost] = 2;
            m_ExpectedAbilityStatus[(int)DKCostTypes.UnHoly] = 2;

            m_Update[(int)DKCostTypes.Blood] = 1;
            m_Update[(int)DKCostTypes.RunicPower] = -10;

            m_ExpectedAbilityStatus[(int)DKCostTypes.Blood] = 1;
            m_ExpectedAbilityStatus[(int)DKCostTypes.RunicPower] = 10;

            bool r = Rotation.ProcessRunningRunes(m_CurrentAbilityStatus, m_Update);
            string szTest = "Blood Rune Test";
            Assert.IsTrue(r, szTest);
            foreach (int i in EnumHelper.GetValues(typeof(DKCostTypes)))
            {
                Assert.AreEqual(m_ExpectedAbilityStatus[i], m_CurrentAbilityStatus[i], szTest + ": " + Enum.GetName(typeof(DKCostTypes), i));
            }

            m_Update[(int)DKCostTypes.Blood] = 0;
            m_Update[(int)DKCostTypes.Frost] = 1;
            m_Update[(int)DKCostTypes.RunicPower] = -10;

            m_ExpectedAbilityStatus[(int)DKCostTypes.Frost] = 1;
            m_ExpectedAbilityStatus[(int)DKCostTypes.RunicPower] = 20;

            r = Rotation.ProcessRunningRunes(m_CurrentAbilityStatus, m_Update);
            szTest = "Frost Rune Test";
            Assert.IsTrue(r, szTest);
            foreach (int i in EnumHelper.GetValues(typeof(DKCostTypes)))
            {
                Assert.AreEqual(m_ExpectedAbilityStatus[i], m_CurrentAbilityStatus[i], szTest + ": " + Enum.GetName(typeof(DKCostTypes), i));
            }
            m_Update[(int)DKCostTypes.Frost] = 0;
            m_Update[(int)DKCostTypes.UnHoly] = 1;
            m_Update[(int)DKCostTypes.RunicPower] = -10;

            m_ExpectedAbilityStatus[(int)DKCostTypes.UnHoly] = 1;
            m_ExpectedAbilityStatus[(int)DKCostTypes.RunicPower] = 30;

            r = Rotation.ProcessRunningRunes(m_CurrentAbilityStatus, m_Update);
            szTest = "Unholy Rune Test";
            Assert.IsTrue(r, szTest);
            foreach (int i in EnumHelper.GetValues(typeof(DKCostTypes)))
            {
                Assert.AreEqual(m_ExpectedAbilityStatus[i], m_CurrentAbilityStatus[i], szTest + ": " + Enum.GetName(typeof(DKCostTypes), i));
            }

            m_Update[(int)DKCostTypes.UnHoly] = 0;
            m_Update[(int)DKCostTypes.RunicPower] = 25;

            m_ExpectedAbilityStatus[(int)DKCostTypes.RunicPower] = 5;

            r = Rotation.ProcessRunningRunes(m_CurrentAbilityStatus, m_Update);
            szTest = "RP Test";
            Assert.IsTrue(r, szTest);
            foreach (int i in EnumHelper.GetValues(typeof(DKCostTypes)))
            {
                Assert.AreEqual(m_ExpectedAbilityStatus[i], m_CurrentAbilityStatus[i], szTest + ": " + Enum.GetName(typeof(DKCostTypes), i));
            }
        }

        /// <summary>
        ///A test for ProcessRunningRunes
        ///</summary>
        [TestMethod()]
        public void ProcessRunningRunesTest_2Runes()
        {
            m_CurrentAbilityStatus[(int)DKCostTypes.Blood] = 2;
            m_CurrentAbilityStatus[(int)DKCostTypes.Frost] = 2;
            m_CurrentAbilityStatus[(int)DKCostTypes.UnHoly] = 2;

            m_ExpectedAbilityStatus[(int)DKCostTypes.Blood] = 2;
            m_ExpectedAbilityStatus[(int)DKCostTypes.Frost] = 2;
            m_ExpectedAbilityStatus[(int)DKCostTypes.UnHoly] = 2;

            m_Update[(int)DKCostTypes.Blood] = 1;
            m_Update[(int)DKCostTypes.Frost] = 1;
            m_Update[(int)DKCostTypes.RunicPower] = -20;

            m_ExpectedAbilityStatus[(int)DKCostTypes.Blood] = 1;
            m_ExpectedAbilityStatus[(int)DKCostTypes.Frost] = 1;
            m_ExpectedAbilityStatus[(int)DKCostTypes.RunicPower] = 20;

            bool r = Rotation.ProcessRunningRunes(m_CurrentAbilityStatus, m_Update);
            string szTest = "BF Rune Test";
            Assert.IsTrue(r, szTest);
            foreach (int i in EnumHelper.GetValues(typeof(DKCostTypes)))
            {
                Assert.AreEqual(m_ExpectedAbilityStatus[i], m_CurrentAbilityStatus[i], szTest + ": " + Enum.GetName(typeof(DKCostTypes), i));
            }

            m_Update[(int)DKCostTypes.Blood] = 0;
            m_Update[(int)DKCostTypes.Frost] = 1;
            m_Update[(int)DKCostTypes.UnHoly] = 1;
            m_Update[(int)DKCostTypes.RunicPower] = -20;

            m_ExpectedAbilityStatus[(int)DKCostTypes.Frost] = 0;
            m_ExpectedAbilityStatus[(int)DKCostTypes.UnHoly] = 1;
            m_ExpectedAbilityStatus[(int)DKCostTypes.RunicPower] = 40;

            r = Rotation.ProcessRunningRunes(m_CurrentAbilityStatus, m_Update);
            szTest = "FU Rune Test";
            Assert.IsTrue(r, szTest);
            foreach (int i in EnumHelper.GetValues(typeof(DKCostTypes)))
            {
                Assert.AreEqual(m_ExpectedAbilityStatus[i], m_CurrentAbilityStatus[i], szTest + ": " + Enum.GetName(typeof(DKCostTypes), i));
            }

            m_Update[(int)DKCostTypes.Blood] = 1;
            m_Update[(int)DKCostTypes.Frost] = 1;
            m_Update[(int)DKCostTypes.UnHoly] = 0;
            m_Update[(int)DKCostTypes.RunicPower] = -20;

            r = Rotation.ProcessRunningRunes(m_CurrentAbilityStatus, m_Update);
            szTest = "InSufficient BF Rune Test";
            Assert.IsFalse(r, szTest);
            foreach (int i in EnumHelper.GetValues(typeof(DKCostTypes)))
            {
                Assert.AreEqual(m_ExpectedAbilityStatus[i], m_CurrentAbilityStatus[i], szTest + ": " + Enum.GetName(typeof(DKCostTypes), i));
            }

        }

        /// <summary>
        ///A test for ProcessRunningRunes
        ///</summary>
        [TestMethod()]
        public void ProcessRunningRunesTest_DeathRunes()
        {
            m_CurrentAbilityStatus[(int)DKCostTypes.Death] = 4;

            m_Update[(int)DKCostTypes.Blood] = 1;
            m_Update[(int)DKCostTypes.RunicPower] = -10;

            m_ExpectedAbilityStatus[(int)DKCostTypes.Death] = 3;
            m_ExpectedAbilityStatus[(int)DKCostTypes.RunicPower] = 10;

            bool r = Rotation.ProcessRunningRunes(m_CurrentAbilityStatus, m_Update);
            string szTest = "Blood Rune Test";
            Assert.IsTrue(r, szTest);
            foreach (int i in EnumHelper.GetValues(typeof(DKCostTypes)))
            {
                Assert.AreEqual(m_ExpectedAbilityStatus[i], m_CurrentAbilityStatus[i], szTest + ": " + Enum.GetName(typeof(DKCostTypes), i));
            }

            m_Update[(int)DKCostTypes.Blood] = 0;
            m_Update[(int)DKCostTypes.Frost] = 1;
            m_Update[(int)DKCostTypes.RunicPower] = -10;

            m_ExpectedAbilityStatus[(int)DKCostTypes.Death] = 2;
            m_ExpectedAbilityStatus[(int)DKCostTypes.RunicPower] = 20;

            r = Rotation.ProcessRunningRunes(m_CurrentAbilityStatus, m_Update);
            szTest = "Frost Rune Test";
            Assert.IsTrue(r, szTest);
            foreach (int i in EnumHelper.GetValues(typeof(DKCostTypes)))
            {
                Assert.AreEqual(m_ExpectedAbilityStatus[i], m_CurrentAbilityStatus[i], szTest + ": " + Enum.GetName(typeof(DKCostTypes), i));
            }
            m_Update[(int)DKCostTypes.Frost] = 0;
            m_Update[(int)DKCostTypes.UnHoly] = 1;
            m_Update[(int)DKCostTypes.RunicPower] = -10;

            m_ExpectedAbilityStatus[(int)DKCostTypes.Death] = 1;
            m_ExpectedAbilityStatus[(int)DKCostTypes.RunicPower] = 30;

            r = Rotation.ProcessRunningRunes(m_CurrentAbilityStatus, m_Update);
            szTest = "Unholy Rune Test";
            Assert.IsTrue(r, szTest);
            foreach (int i in EnumHelper.GetValues(typeof(DKCostTypes)))
            {
                Assert.AreEqual(m_ExpectedAbilityStatus[i], m_CurrentAbilityStatus[i], szTest + ": " + Enum.GetName(typeof(DKCostTypes), i));
            }

            m_Update[(int)DKCostTypes.Blood] = 1;
            m_Update[(int)DKCostTypes.Frost] = 1;
            m_Update[(int)DKCostTypes.RunicPower] = -20;

            r = Rotation.ProcessRunningRunes(m_CurrentAbilityStatus, m_Update);
            szTest = "Insufficient Runes";
            Assert.IsFalse(r, szTest);
            // And Current Ability Status Should go unchanged.
            foreach (int i in EnumHelper.GetValues(typeof(DKCostTypes)))
            {
                Assert.AreEqual(m_ExpectedAbilityStatus[i], m_CurrentAbilityStatus[i], szTest + ": " + Enum.GetName(typeof(DKCostTypes), i));
            }
        }
    }
}
