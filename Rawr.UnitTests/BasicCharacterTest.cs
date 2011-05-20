using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rawr;

namespace Rawr.UnitTests
{

    enum Models
    {
        Bear,
        Cat,
        DPSDK,
        DPSWarr,
        Elemental,
        Enhance,
        Healadin,
        HealPriest,
        Hunter,
        Mage,
        Moonkin,
        ProtPaladin,
        ProtWarr,
        RestoSham,
        Retribution,
        Rogue,
        ShadowPriest,
        TankDK,
        Tree,
        Warlock
    }

    /// <summary>
    /// Basic Character tests.
    /// Load and run a calculation on Empty & Loaded Characters for each model.
    /// </summary>
    [TestClass]
    public class BasicCharacterTest
    {
        Character m_char;
        static string szBasePath = "..\\..\\..\\..\\Rawr\\Rawr.UnitTests\\testdata\\";

        public BasicCharacterTest()
        {
            m_char = new Character();

            Calculations.RegisterModel(typeof(Rawr.Bear.CalculationsBear));
            Calculations.RegisterModel(typeof(Rawr.Cat.CalculationsCat));
            Calculations.RegisterModel(typeof(Rawr.DPSDK.CalculationsDPSDK));
            Calculations.RegisterModel(typeof(Rawr.DPSWarr.CalculationsDPSWarr));
            Calculations.RegisterModel(typeof(Rawr.Elemental.CalculationsElemental));
            Calculations.RegisterModel(typeof(Rawr.Enhance.CalculationsEnhance));
            Calculations.RegisterModel(typeof(Rawr.Healadin.CalculationsHealadin));
            Calculations.RegisterModel(typeof(Rawr.HealPriest.CalculationsHealPriest));
            Calculations.RegisterModel(typeof(Rawr.Hunter.CalculationsHunter));
            Calculations.RegisterModel(typeof(Rawr.Mage.CalculationsMage));
            Calculations.RegisterModel(typeof(Rawr.Moonkin.CalculationsMoonkin));
            Calculations.RegisterModel(typeof(Rawr.ProtPaladin.CalculationsProtPaladin));
            Calculations.RegisterModel(typeof(Rawr.ProtWarr.CalculationsProtWarr));
            Calculations.RegisterModel(typeof(Rawr.RestoSham.CalculationsRestoSham));
            Calculations.RegisterModel(typeof(Rawr.Retribution.CalculationsRetribution));
            Calculations.RegisterModel(typeof(Rawr.Rogue.CalculationsRogue));
            Calculations.RegisterModel(typeof(Rawr.ShadowPriest.CalculationsShadowPriest));
            Calculations.RegisterModel(typeof(Rawr.TankDK.CalculationsTankDK));
            Calculations.RegisterModel(typeof(Rawr.Tree.CalculationsTree));
            Calculations.RegisterModel(typeof(Rawr.Warlock.CalculationsWarlock));
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
        public void Test_EmptyCharacterXML()
        {
            string szFullPath;
            int iCount = 0;
            foreach (string szModel in EnumHelper.GetNames(typeof(Models)))
            {
                szFullPath = szBasePath + "~Test_Empty" + szModel + ".xml";
                string szCharXML = System.IO.File.ReadAllText(szFullPath);
                Character c = Character.LoadFromXml(szCharXML);
                Assert.AreEqual(szModel, c.CurrentModel, true);
                CharacterCalculationsBase CharCalcs = c.CurrentCalculations.GetCharacterCalculations(c);
                Assert.IsNotNull(CharCalcs, szModel);
                CharCalcs.GetCharacterDisplayCalculationValues();
                Assert.IsTrue(CharCalcs.OverallPoints >= 0, szModel);
                iCount++;
            }
            // Test to ensure all models run.
            Assert.AreEqual(EnumHelper.GetCount(typeof(Models)), iCount, "ModelCount");
        }
    }
}
