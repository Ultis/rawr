using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rawr;
using Rawr.DPSDK;

namespace Rawr.UnitTests.DK
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class DKDPS
    {
        Stats weap;
        
        Item weapon;
        Character m_char;

        public DKDPS()
        {
            // TODO: Add constructor logic here

            weap = new Stats();
            weap.Strength = 10;
            weap.CritRating = 500;
            weapon = new Item("test", ItemQuality.Common, ItemType.OneHandSword, 10101, "icon.bmp", ItemSlot.OneHand, "", false,
                weap, weap, ItemSlot.None, ItemSlot.None, ItemSlot.None, 100, 200, ItemDamageType.Physical, 3.8f, "Death Knight");

            m_char = new Character();
            string szXML = System.IO.File.ReadAllText("..\\..\\..\\..\\Rawr\\Rawr.UnitTests\\testdata\\~Test_Rawr4_Unholy2h.xml");
            m_char = Character.LoadFromXml(szXML);
            if (m_char.Class == CharacterClass.Druid)
            {
                // This means it didn't load properly.
                m_char.Class = CharacterClass.DeathKnight;
                // So a weapon, so we have values in weapon specific abilities.
                m_char.MainHand = new ItemInstance(weapon, 0, null, null, null, new Enchant(), new Reforging(), new Tinkering());
                // Unholy DK
                m_char.DeathKnightTalents = new DeathKnightTalents("203200000000000000002000000000000000000003310321231031021231.010000001010100000010010000111");
            }
            m_char.CurrentModel = "DPSDK";

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
        public void DPSDK_BuildAcceptance()
        {
            Rawr.DPSDK.CalculationsDPSDK CalcDPSDK = new Rawr.DPSDK.CalculationsDPSDK();

            CalculationOptionsDPSDK calcOpts = new CalculationOptionsDPSDK();
            calcOpts.presence = Rawr.DK.Presence.Frost;
            m_char.CalculationOptions = calcOpts;
            this.testContextInstance.BeginTimer("GetCalc");
            CharacterCalculationsBase calcs = CalcDPSDK.GetCharacterCalculations(m_char);
            calcs.GetCharacterDisplayCalculationValues();
            this.testContextInstance.EndTimer("GetCalc");
        }


        [TestMethod]
        public void DPSDK_DPSMisMatch()
        {
            Rawr.DPSDK.CalculationsDPSDK CalcDPSDK = new Rawr.DPSDK.CalculationsDPSDK();

            CalculationOptionsDPSDK calcOpts = new CalculationOptionsDPSDK();
            calcOpts.presence = Rawr.DK.Presence.Frost;
            m_char.CalculationOptions = calcOpts;
            CharacterCalculationsDPSDK calcs = CalcDPSDK.GetCharacterCalculations(m_char) as CharacterCalculationsDPSDK;
            calcs.GetCharacterDisplayCalculationValues();
            for (int i = 0; i < EnumHelper.GetCount(typeof(Rawr.DK.DKability)); i++)
            {
                Assert.IsTrue(calcs.dpsSub[i] <= calcs.damSub[i], string.Format("{0} Dam: {1} DPS: {2}",
                    ((Rawr.DK.DKability)i).ToString(), calcs.damSub[i], calcs.dpsSub[i]));
                Assert.IsTrue(calcs.tpsSub[i] <= calcs.threatSub[i], string.Format("{0} Threat: {1} TPS: {2}",
                    ((Rawr.DK.DKability)i).ToString(), calcs.threatSub[i], calcs.tpsSub[i]));
            }
        }

        [TestMethod]
        public void DPSDK_Rotation()
        {
            Rawr.DPSDK.CharacterCalculationsDPSDK CalcDPSDK = new Rawr.DPSDK.CharacterCalculationsDPSDK();
            CalculationOptionsDPSDK calcOpts = new CalculationOptionsDPSDK();
            Rawr.DK.StatsDK TotalStats = new Rawr.DK.StatsDK();

            Rawr.DK.DKCombatTable ct = new Rawr.DK.DKCombatTable(m_char, TotalStats, CalcDPSDK, calcOpts, m_char.BossOptions);
            Rawr.DK.Rotation rot = new Rawr.DK.Rotation(ct, false);
            rot.PRE_OneEachRot();
            rot.ReportRotation();
            Assert.IsTrue(rot.m_DPS > 0, "rotation OneEach produces 0 DPS");
            rot.PRE_Frost();
            rot.ReportRotation();
            Assert.IsTrue(rot.m_DPS > 0, "rotation Frost produces 0 DPS");
            rot.PRE_Unholy();
            rot.ReportRotation();
            Assert.IsTrue(rot.m_DPS > 0, "rotation Unholy produces 0 DPS");
            rot.PRE_BloodDiseased();
            rot.ReportRotation();
            Assert.IsTrue(rot.m_DPS > 0, "rotation BloodDiseased produces 0 DPS");
            rot.Solver();
            rot.ReportRotation();
            Assert.IsTrue(rot.m_DPS > 0, "rotation solver produces 0 DPS");
            
        }

        [TestMethod]
        public void DPSDK_TrinketHang()
        {
            Stats StatTrink = new Stats();
            StatTrink.AddSpecialEffect(new SpecialEffect(Trigger.MainHandHit, new Stats() { Strength = 300 }, 10, 0, .1f, 5));
            StatTrink.MasteryRating = 500;
            Item Trinket = new Item("testTrink", ItemQuality.Epic, ItemType.None, 10102, "icon.bmp", ItemSlot.Trinket, "", false,
                StatTrink, StatTrink, ItemSlot.None, ItemSlot.None, ItemSlot.None, 0, 0, ItemDamageType.Physical, 0, "");
            m_char.Trinket1 = new ItemInstance(Trinket, 0, null, null, null, new Enchant(), new Reforging(), new Tinkering());

            // This bug was due to non-valid swing times.
            m_char.MainHand = null;

            Rawr.DPSDK.CalculationsDPSDK CalcDPSDK = new Rawr.DPSDK.CalculationsDPSDK();

            CalculationOptionsDPSDK calcOpts = new CalculationOptionsDPSDK();
            calcOpts.presence = Rawr.DK.Presence.Frost;
            m_char.CalculationOptions = calcOpts;
            this.testContextInstance.BeginTimer("GetCalc");
            CharacterCalculationsBase calcs = CalcDPSDK.GetCharacterCalculations(m_char);
            calcs.GetCharacterDisplayCalculationValues();
            this.testContextInstance.EndTimer("GetCalc");
        }
    }
}
