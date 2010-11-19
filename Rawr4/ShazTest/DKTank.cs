using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rawr;
using Rawr.TankDK;

namespace ShazTest
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class DKTANK
    {
        public DKTANK()
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
        public void TestMethod_TankDK_BuildAcceptance()
        {
            Rawr.TankDK.CalculationsTankDK CalcTankDK = new Rawr.TankDK.CalculationsTankDK();
            Character c = new Character();
            string szXML = System.IO.File.ReadAllText("C:\\Users\\Shazear\\Documents\\Rawr\\Rawr.UnitTests\\testdata\\~Test_Rawr4_Blood2h.xml");
            c = Character.LoadFromXml(szXML);
            if (c.Class == CharacterClass.Druid)
            {
                c.Class = CharacterClass.DeathKnight;
                // TODO: Get some talents in here all proper.
            }
            c.CurrentModel = "TankDK";
            if (c.BossOptions == null || c.BossOptions.Attacks.Count <= 0)
            {
                BossList list = new BossList();
                BossHandler testboss = new BossHandler();
                testboss = list.GetBossFromName("Pit Lord Argaloth");
                
                c.BossOptions = new BossOptions();
                c.BossOptions.CloneThis(testboss);
            }
            CalculationOptionsTankDK calcOpts = new CalculationOptionsTankDK();
            c.CalculationOptions = calcOpts;
            this.testContextInstance.BeginTimer("GetCalc");
            CharacterCalculationsBase calcs = CalcTankDK.GetCharacterCalculations(c);
            calcs.GetCharacterDisplayCalculationValues();
            this.testContextInstance.EndTimer("GetCalc");
        }
    }
}
