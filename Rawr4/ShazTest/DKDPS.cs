using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rawr;
using Rawr.DPSDK;

namespace ShazTest
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class DKDPS
    {
        Stats weap;
        Item weapon;

        public DKDPS()
        {
            // TODO: Add constructor logic here

            weap = new Stats();
            weap.Strength = 10;
            weapon = new Item("test", ItemQuality.Common, ItemType.OneHandSword, 10101, "icon.bmp", ItemSlot.OneHand, "", false,
                weap, weap, ItemSlot.None, ItemSlot.None, ItemSlot.None, 100, 200, ItemDamageType.Physical, 3.8f, "Death Knight");

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
        public void TestMethod_DPSDK_BuildAcceptance()
        {
            Rawr.DPSDK.CalculationsDPSDK CalcDPSDK = new Rawr.DPSDK.CalculationsDPSDK();
            Character c = new Character();
            string szXML = System.IO.File.ReadAllText("..\\..\\..\\..\\..\\Rawr\\Rawr.UnitTests\\testdata\\~Test_Rawr4_Unholy2h.xml");
            c = Character.LoadFromXml(szXML);
            if (c.Class == CharacterClass.Druid)
            {
                // This means it didn't load properly.
                c.Class = CharacterClass.DeathKnight;
                // So a weapon, so we have values in weapon specific abilities.
                c.MainHand = new ItemInstance(weapon, null, null, null, new Enchant(), new Reforging());
                // Some talents.
                c.DeathKnightTalents = new DeathKnightTalents("0000000000000000000320302000000000000000332032123031011231.00000000000000000000000000000");
            }
            c.CurrentModel = "DPSDK";
            CalculationOptionsDPSDK calcOpts = new CalculationOptionsDPSDK();
            calcOpts.presence = Rawr.DK.Presence.Frost;
            c.CalculationOptions = calcOpts;
            this.testContextInstance.BeginTimer("GetCalc");
            CharacterCalculationsBase calcs = CalcDPSDK.GetCharacterCalculations(c);
            calcs.GetCharacterDisplayCalculationValues();
            this.testContextInstance.EndTimer("GetCalc");
        }


        [TestMethod]
        public void TestMethod_DPSDK_DPSMisMatch()
        {
            Rawr.DPSDK.CalculationsDPSDK CalcDPSDK = new Rawr.DPSDK.CalculationsDPSDK();
            Character c = new Character();
            string szXML = System.IO.File.ReadAllText("..\\..\\..\\..\\..\\Rawr\\Rawr.UnitTests\\testdata\\~Test_Rawr4_Unholy2h.xml");
            c = Character.LoadFromXml(szXML);
            if (c.Class == CharacterClass.Druid)
            {
                // This means it didn't load properly.
                c.Class = CharacterClass.DeathKnight;
                // So a weapon, so we have values in weapon specific abilities.
                c.MainHand = new ItemInstance(weapon, null, null, null, new Enchant(), new Reforging());
                // Some talents.
                c.DeathKnightTalents = new DeathKnightTalents("0000000000000000000320302000000000000000332032123031011231.00000000000000000000000000000");
            }
            c.CurrentModel = "DPSDK";
            CalculationOptionsDPSDK calcOpts = new CalculationOptionsDPSDK();
            calcOpts.presence = Rawr.DK.Presence.Frost;
            c.CalculationOptions = calcOpts;
            CharacterCalculationsDPSDK calcs = CalcDPSDK.GetCharacterCalculations(c) as CharacterCalculationsDPSDK;
            calcs.GetCharacterDisplayCalculationValues();
            for (int i = 0; i < EnumHelper.GetCount(typeof(Rawr.DK.DKability)); i++)
            {
                Assert.IsTrue(calcs.dpsSub[i] <= calcs.damSub[i], string.Format("{0} Dam: {1} DPS: {2}",
                    ((Rawr.DK.DKability)i).ToString(), calcs.damSub[i], calcs.dpsSub[i]));
                Assert.IsTrue(calcs.tpsSub[i] <= calcs.threatSub[i], string.Format("{0} Threat: {1} TPS: {2}",
                    ((Rawr.DK.DKability)i).ToString(), calcs.threatSub[i], calcs.tpsSub[i]));
            }
        }
    }
}
