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
        Stats weap;
        Item weapon;
        Character m_char;

        public DKTANK()
        {
            // TODO: Add constructor logic here

            weap = new Stats();
            weap.Strength = 10;
            weapon = new Item("test", ItemQuality.Common, ItemType.OneHandSword, 10101, "icon.bmp", ItemSlot.OneHand, "", false,
                weap, weap, ItemSlot.None, ItemSlot.None, ItemSlot.None, 100, 200, ItemDamageType.Physical, 3.8f, "Death Knight");

            m_char = new Character();
            string szXML = System.IO.File.ReadAllText("..\\..\\..\\..\\..\\Rawr\\Rawr.UnitTests\\testdata\\~Test_Rawr4_Blood2h.xml");
            m_char = Character.LoadFromXml(szXML);
            if (m_char.Class == CharacterClass.Druid)
            {
                // This means it didn't load properly.
                m_char.Class = CharacterClass.DeathKnight;
                // So a weapon, so we have values in weapon specific abilities.
                m_char.MainHand = new ItemInstance(weapon, null, null, null, new Enchant(), new Reforging(), new Tinkering());
                // Some talents.
                // Blood Talents.
                m_char.DeathKnightTalents = new DeathKnightTalents("03322203130022011321000000000000000000000000000000000000000.00000000000000000000000000000");
            }
            m_char.CurrentModel = "TankDK";
            if (m_char.BossOptions == null || m_char.BossOptions.Attacks.Count <= 0)
            {
                BossList list = new BossList();
                BossHandler testboss = new BossHandler();
                testboss = list.GetBossFromName("Pit Lord Argaloth");

                m_char.BossOptions = new BossOptions();
                m_char.BossOptions.CloneThis(testboss);
            }
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

            CalculationOptionsTankDK calcOpts = new CalculationOptionsTankDK();
            m_char.CalculationOptions = calcOpts;
            this.testContextInstance.BeginTimer("GetCalc");
            CharacterCalculationsBase calcs = CalcTankDK.GetCharacterCalculations(m_char);
            calcs.GetCharacterDisplayCalculationValues();
            this.testContextInstance.EndTimer("GetCalc");
        }

        [TestMethod]
        public void TestMethod_TankDK_Rotation()
        {
            Rawr.TankDK.CharacterCalculationsTankDK CalcTankDK = new Rawr.TankDK.CharacterCalculationsTankDK();
            CalculationOptionsTankDK calcOpts = new CalculationOptionsTankDK();
            Rawr.DK.StatsDK TotalStats = new Rawr.DK.StatsDK();

            Rawr.DPSDK.CharacterCalculationsDPSDK DPSCalcs = new Rawr.DPSDK.CharacterCalculationsDPSDK();
            Rawr.DPSDK.CalculationOptionsDPSDK DPSopts = new Rawr.DPSDK.CalculationOptionsDPSDK();

            Rawr.DK.DKCombatTable ct = new Rawr.DK.DKCombatTable(m_char, TotalStats, DPSCalcs, DPSopts);
            Rawr.DK.Rotation rot = new Rawr.DK.Rotation(ct, false);
            rot.PRE_BloodDiseaseless();
            Assert.IsTrue(rot.m_TPS > 0, "rotation BloodDiseaseless produces 0 DPS");
            rot.PRE_BloodDiseased();
            Assert.IsTrue(rot.m_TPS > 0, "rotation BloodDiseased produces 0 DPS");
        }

        [TestMethod]
        public void TestMethod_TankDK_OverallCheck()
        {
            Rawr.TankDK.CalculationsTankDK CalcTankDK = new Rawr.TankDK.CalculationsTankDK();
            CalculationOptionsTankDK calcOpts = new CalculationOptionsTankDK();
            m_char.CalculationOptions = calcOpts;
            Item additionalItem = new Item("TestItem", ItemQuality.Common, ItemType.None, 102010, "", ItemSlot.Back, "", false,
                new Stats(), null,
                ItemSlot.None, ItemSlot.None, ItemSlot.None,
                0, 0, ItemDamageType.Physical, 0, "");

            CharacterCalculationsTankDK calcs = CalcTankDK.GetCharacterCalculations(m_char) as CharacterCalculationsTankDK;
            float OValueBase = calcs.OverallPoints;
            float[] SValueBase = calcs.SubPoints;

            // Setup the stats on what we want.
            additionalItem.Stats.Stamina = 5000;
            calcs = CalcTankDK.GetCharacterCalculations(m_char, additionalItem) as CharacterCalculationsTankDK;
            float OValueStam = calcs.OverallPoints;
            float[] SValueStam = calcs.SubPoints;
            additionalItem.Stats.Stamina = 0;
            Assert.IsTrue(OValueBase < OValueStam, "Stamina");
            
            additionalItem.Stats.DodgeRating = 5000;
            calcs = CalcTankDK.GetCharacterCalculations(m_char, additionalItem) as CharacterCalculationsTankDK;
            float OValueDodge = calcs.OverallPoints;
            float[] SValueDodge = calcs.SubPoints;
            additionalItem.Stats.DodgeRating = 0;

            additionalItem.Stats.DodgeRating = 10000;
            calcs = CalcTankDK.GetCharacterCalculations(m_char, additionalItem) as CharacterCalculationsTankDK;
            float OValueDodge2 = calcs.OverallPoints;
            float[] SValueDodge2 = calcs.SubPoints;
            additionalItem.Stats.DodgeRating = 0;
//            Assert.IsTrue(OValueDodge < OValueDodge2, "Dodge2");
//            Assert.IsTrue(OValueBase < OValueDodge, "Dodge1");

            additionalItem.Stats.ParryRating = 5000;
            calcs = CalcTankDK.GetCharacterCalculations(m_char, additionalItem) as CharacterCalculationsTankDK;
            float OValueParry = calcs.OverallPoints;
            float[] SValueParry = calcs.SubPoints;
            additionalItem.Stats.ParryRating = 0;
//            Assert.IsTrue(OValueBase < OValueParry, "Parry");
            
            additionalItem.Stats.Agility = 5000;
            calcs = CalcTankDK.GetCharacterCalculations(m_char, additionalItem) as CharacterCalculationsTankDK;
            float OValueAgility = calcs.OverallPoints;
            float[] SValueAgility = calcs.SubPoints;
            additionalItem.Stats.Agility = 0;
            Assert.IsTrue(OValueBase < OValueAgility, "Agility");

            additionalItem.Stats.MasteryRating = 5000;
            calcs = CalcTankDK.GetCharacterCalculations(m_char, additionalItem) as CharacterCalculationsTankDK;
            float OValueMastery = calcs.OverallPoints;
            float[] SValueMastery = calcs.SubPoints;
            additionalItem.Stats.MasteryRating = 0;
            Assert.IsTrue(OValueBase < OValueMastery, "Mastery");
            
        }

    }
}
