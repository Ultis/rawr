using Rawr;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Rawr.UnitTests
{
    
    
    /// <summary>
    ///This is a test class for StatConversionTest and is intended
    ///to contain all StatConversionTest Unit Tests
    ///</summary>
    [TestClass()]
    public class StatConversionTest
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
        ///A test for GetDRAvoidanceChance
        ///</summary>
        [TestMethod()]
        public void GetDRAvoidanceChanceTest_Warrior_NoGear()
        {
            ItemInstance[] IIArray = new ItemInstance[1];
            Character toon = new Character("TestWarrior", "Malygos", Character.CharacterRegion.US, Character.CharacterRace.Human, IIArray, new System.Collections.Generic.List<Buff>(), "ProtWar"); // TODO: Initialize to an appropriate value
            Assert.IsNotNull(toon);
            //toon.Level = 80;  //Asumption here.
            toon.Class = Character.CharacterClass.Warrior;

            Stats stats = new Stats();
            uint TargetLevel = 83;
            float[] expected = new float[(int)HitResult.NUM_HitResult];
            expected[(int)HitResult.Miss] = -0.006f;
            expected[(int)HitResult.Dodge] = -0.006f;
            expected[(int)HitResult.Parry] = -0.006f;
            expected[(int)HitResult.Block] = 0.044f;
            expected[(int)HitResult.Crit] = -0f;

            // Iterate through the hit result types.
            for (HitResult i = 0; i < HitResult.NUM_HitResult; i++)
            {
                float actual = StatConversion.GetDRAvoidanceChance(toon, stats, i, TargetLevel);
                Assert.AreEqual(expected[(int)i], actual, i.ToString());
            }
        }

        /// <summary>
        ///A test for GetDefenseFromRating
        ///</summary>
        [TestMethod()]
        public void GetDefenseFromRatingTest_zero()
        {
            float Rating = 0F; 
            float expected = 0F; 
            float actual;
            actual = StatConversion.GetDefenseFromRating(Rating);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for GetDefenseFromRating
        ///</summary>
        [TestMethod()]
        public void GetDefenseFromRatingTest_CritCap()
        {
            float Rating = 689F;
            float expected = 140F;
            float actual;
            actual = StatConversion.GetDefenseFromRating(Rating);
            Assert.AreEqual(expected, actual);
        }

        /* Commenting out the tests that aren't implemented yet.
         * 
         * 
        /// <summary>
        ///A test for GetCombatRatingModifier
        ///</summary>
        [TestMethod()]
        public void GetCombatRatingModifierTest()
        {
            // TODO:  (Shazear)
            // Setup an array that compares real values w/ expected values.
            uint iLevel = 0; // TODO: Initialize to an appropriate value
            float expected = 0F; // TODO: Initialize to an appropriate value
            float actual;
            actual = StatConversion.GetCombatRatingModifier(iLevel);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetSpiritRegenSec
        ///</summary>
        [TestMethod()]
        public void GetSpiritRegenSecTest1()
        {
            float Spirit = 0F; // TODO: Initialize to an appropriate value
            float Intellect = 0F; // TODO: Initialize to an appropriate value
            Character.CharacterClass Class = new Character.CharacterClass(); // TODO: Initialize to an appropriate value
            float expected = 0F; // TODO: Initialize to an appropriate value
            float actual;
            actual = StatConversion.GetSpiritRegenSec(Spirit, Intellect, Class);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetSpiritRegenSec
        ///</summary>
        [TestMethod()]
        public void GetSpiritRegenSecTest()
        {
            float Spirit = 0F; // TODO: Initialize to an appropriate value
            float Intellect = 0F; // TODO: Initialize to an appropriate value
            float expected = 0F; // TODO: Initialize to an appropriate value
            float actual;
            actual = StatConversion.GetSpiritRegenSec(Spirit, Intellect);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetSpellMiss
        ///</summary>
        [TestMethod()]
        public void GetSpellMissTest()
        {
            int LevelDelta = 0; // TODO: Initialize to an appropriate value
            bool bPvP = false; // TODO: Initialize to an appropriate value
            float expected = 0F; // TODO: Initialize to an appropriate value
            float actual;
            actual = StatConversion.GetSpellMiss(LevelDelta, bPvP);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetSpellHitFromRating
        ///</summary>
        [TestMethod()]
        public void GetSpellHitFromRatingTest1()
        {
            float Rating = 0F; // TODO: Initialize to an appropriate value
            float expected = 0F; // TODO: Initialize to an appropriate value
            float actual;
            actual = StatConversion.GetSpellHitFromRating(Rating);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetSpellHitFromRating
        ///</summary>
        [TestMethod()]
        public void GetSpellHitFromRatingTest()
        {
            float Rating = 0F; // TODO: Initialize to an appropriate value
            Character.CharacterClass Class = new Character.CharacterClass(); // TODO: Initialize to an appropriate value
            float expected = 0F; // TODO: Initialize to an appropriate value
            float actual;
            actual = StatConversion.GetSpellHitFromRating(Rating, Class);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetSpellHasteFromRating
        ///</summary>
        [TestMethod()]
        public void GetSpellHasteFromRatingTest1()
        {
            float Rating = 0F; // TODO: Initialize to an appropriate value
            Character.CharacterClass Class = new Character.CharacterClass(); // TODO: Initialize to an appropriate value
            float expected = 0F; // TODO: Initialize to an appropriate value
            float actual;
            actual = StatConversion.GetSpellHasteFromRating(Rating, Class);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetSpellHasteFromRating
        ///</summary>
        [TestMethod()]
        public void GetSpellHasteFromRatingTest()
        {
            float Rating = 0F; // TODO: Initialize to an appropriate value
            float expected = 0F; // TODO: Initialize to an appropriate value
            float actual;
            actual = StatConversion.GetSpellHasteFromRating(Rating);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetSpellCritFromRating
        ///</summary>
        [TestMethod()]
        public void GetSpellCritFromRatingTest1()
        {
            float Rating = 0F; // TODO: Initialize to an appropriate value
            Character.CharacterClass Class = new Character.CharacterClass(); // TODO: Initialize to an appropriate value
            float expected = 0F; // TODO: Initialize to an appropriate value
            float actual;
            actual = StatConversion.GetSpellCritFromRating(Rating, Class);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetSpellCritFromRating
        ///</summary>
        [TestMethod()]
        public void GetSpellCritFromRatingTest()
        {
            float Rating = 0F; // TODO: Initialize to an appropriate value
            float expected = 0F; // TODO: Initialize to an appropriate value
            float actual;
            actual = StatConversion.GetSpellCritFromRating(Rating);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetSpellCritFromIntellect
        ///</summary>
        [TestMethod()]
        public void GetSpellCritFromIntellectTest1()
        {
            float Intellect = 0F; // TODO: Initialize to an appropriate value
            float expected = 0F; // TODO: Initialize to an appropriate value
            float actual;
            actual = StatConversion.GetSpellCritFromIntellect(Intellect);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetSpellCritFromIntellect
        ///</summary>
        [TestMethod()]
        public void GetSpellCritFromIntellectTest()
        {
            float Intellect = 0F; // TODO: Initialize to an appropriate value
            Character.CharacterClass Class = new Character.CharacterClass(); // TODO: Initialize to an appropriate value
            float expected = 0F; // TODO: Initialize to an appropriate value
            float actual;
            actual = StatConversion.GetSpellCritFromIntellect(Intellect, Class);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetResistanceTableString
        ///</summary>
        [TestMethod()]
        public void GetResistanceTableStringTest()
        {
            int AttackerLevel = 0; // TODO: Initialize to an appropriate value
            int TargetLevel = 0; // TODO: Initialize to an appropriate value
            float TargetResistance = 0F; // TODO: Initialize to an appropriate value
            float AttackerSpellPenetration = 0F; // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            actual = StatConversion.GetResistanceTableString(AttackerLevel, TargetLevel, TargetResistance, AttackerSpellPenetration);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetResistanceTable
        ///</summary>
        [TestMethod()]
        public void GetResistanceTableTest()
        {
            int AttackerLevel = 0; // TODO: Initialize to an appropriate value
            int TargetLevel = 0; // TODO: Initialize to an appropriate value
            float TargetResistance = 0F; // TODO: Initialize to an appropriate value
            float AttackerSpellPenetration = 0F; // TODO: Initialize to an appropriate value
            float[] expected = null; // TODO: Initialize to an appropriate value
            float[] actual;
            actual = StatConversion.GetResistanceTable(AttackerLevel, TargetLevel, TargetResistance, AttackerSpellPenetration);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetResilienceFromRating
        ///</summary>
        [TestMethod()]
        public void GetResilienceFromRatingTest1()
        {
            float Rating = 0F; // TODO: Initialize to an appropriate value
            Character.CharacterClass Class = new Character.CharacterClass(); // TODO: Initialize to an appropriate value
            float expected = 0F; // TODO: Initialize to an appropriate value
            float actual;
            actual = StatConversion.GetResilienceFromRating(Rating, Class);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetResilienceFromRating
        ///</summary>
        [TestMethod()]
        public void GetResilienceFromRatingTest()
        {
            float Rating = 0F; // TODO: Initialize to an appropriate value
            float expected = 0F; // TODO: Initialize to an appropriate value
            float actual;
            actual = StatConversion.GetResilienceFromRating(Rating);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetPhysicalHitFromRating
        ///</summary>
        [TestMethod()]
        public void GetPhysicalHitFromRatingTest1()
        {
            float Rating = 0F; // TODO: Initialize to an appropriate value
            float expected = 0F; // TODO: Initialize to an appropriate value
            float actual;
            actual = StatConversion.GetPhysicalHitFromRating(Rating);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetPhysicalHitFromRating
        ///</summary>
        [TestMethod()]
        public void GetPhysicalHitFromRatingTest()
        {
            float Rating = 0F; // TODO: Initialize to an appropriate value
            Character.CharacterClass Class = new Character.CharacterClass(); // TODO: Initialize to an appropriate value
            float expected = 0F; // TODO: Initialize to an appropriate value
            float actual;
            actual = StatConversion.GetPhysicalHitFromRating(Rating, Class);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetPhysicalHasteFromRating
        ///</summary>
        [TestMethod()]
        public void GetPhysicalHasteFromRatingTest()
        {
            float Rating = 0F; // TODO: Initialize to an appropriate value
            Character.CharacterClass Class = new Character.CharacterClass(); // TODO: Initialize to an appropriate value
            float expected = 0F; // TODO: Initialize to an appropriate value
            float actual;
            actual = StatConversion.GetPhysicalHasteFromRating(Rating, Class);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetPhysicalCritFromRating
        ///</summary>
        [TestMethod()]
        public void GetPhysicalCritFromRatingTest1()
        {
            float Rating = 0F; // TODO: Initialize to an appropriate value
            float expected = 0F; // TODO: Initialize to an appropriate value
            float actual;
            actual = StatConversion.GetPhysicalCritFromRating(Rating);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetPhysicalCritFromRating
        ///</summary>
        [TestMethod()]
        public void GetPhysicalCritFromRatingTest()
        {
            float Rating = 0F; // TODO: Initialize to an appropriate value
            Character.CharacterClass Class = new Character.CharacterClass(); // TODO: Initialize to an appropriate value
            float expected = 0F; // TODO: Initialize to an appropriate value
            float actual;
            actual = StatConversion.GetPhysicalCritFromRating(Rating, Class);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetPhysicalCritFromAgility
        ///</summary>
        [TestMethod()]
        public void GetPhysicalCritFromAgilityTest()
        {
            float Agility = 0F; // TODO: Initialize to an appropriate value
            Character.CharacterClass Class = new Character.CharacterClass(); // TODO: Initialize to an appropriate value
            float expected = 0F; // TODO: Initialize to an appropriate value
            float actual;
            actual = StatConversion.GetPhysicalCritFromAgility(Agility, Class);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetParryFromRating
        ///</summary>
        [TestMethod()]
        public void GetParryFromRatingTest1()
        {
            float Rating = 0F; // TODO: Initialize to an appropriate value
            float expected = 0F; // TODO: Initialize to an appropriate value
            float actual;
            actual = StatConversion.GetParryFromRating(Rating);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetParryFromRating
        ///</summary>
        [TestMethod()]
        public void GetParryFromRatingTest()
        {
            float Rating = 0F; // TODO: Initialize to an appropriate value
            Character.CharacterClass Class = new Character.CharacterClass(); // TODO: Initialize to an appropriate value
            float expected = 0F; // TODO: Initialize to an appropriate value
            float actual;
            actual = StatConversion.GetParryFromRating(Rating, Class);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetMinimumResistance
        ///</summary>
        [TestMethod()]
        public void GetMinimumResistanceTest()
        {
            int AttackerLevel = 0; // TODO: Initialize to an appropriate value
            int TargetLevel = 0; // TODO: Initialize to an appropriate value
            float TargetResistance = 0F; // TODO: Initialize to an appropriate value
            float AttackerSpellPenetration = 0F; // TODO: Initialize to an appropriate value
            float expected = 0F; // TODO: Initialize to an appropriate value
            float actual;
            actual = StatConversion.GetMinimumResistance(AttackerLevel, TargetLevel, TargetResistance, AttackerSpellPenetration);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetHitFromRating
        ///</summary>
        [TestMethod()]
        public void GetHitFromRatingTest1()
        {
            float Rating = 0F; // TODO: Initialize to an appropriate value
            Character.CharacterClass Class = new Character.CharacterClass(); // TODO: Initialize to an appropriate value
            float expected = 0F; // TODO: Initialize to an appropriate value
            float actual;
            actual = StatConversion.GetHitFromRating(Rating, Class);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetHitFromRating
        ///</summary>
        [TestMethod()]
        public void GetHitFromRatingTest()
        {
            float Rating = 0F; // TODO: Initialize to an appropriate value
            float expected = 0F; // TODO: Initialize to an appropriate value
            float actual;
            actual = StatConversion.GetHitFromRating(Rating);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetHasteFromRating
        ///</summary>
        [TestMethod()]
        public void GetHasteFromRatingTest()
        {
            float Rating = 0F; // TODO: Initialize to an appropriate value
            Character.CharacterClass Class = new Character.CharacterClass(); // TODO: Initialize to an appropriate value
            float expected = 0F; // TODO: Initialize to an appropriate value
            float actual;
            actual = StatConversion.GetHasteFromRating(Rating, Class);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetExpertiseFromRating
        ///</summary>
        [TestMethod()]
        public void GetExpertiseFromRatingTest1()
        {
            float Rating = 0F; // TODO: Initialize to an appropriate value
            float expected = 0F; // TODO: Initialize to an appropriate value
            float actual;
            actual = StatConversion.GetExpertiseFromRating(Rating);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetExpertiseFromRating
        ///</summary>
        [TestMethod()]
        public void GetExpertiseFromRatingTest()
        {
            float Rating = 0F; // TODO: Initialize to an appropriate value
            Character.CharacterClass Class = new Character.CharacterClass(); // TODO: Initialize to an appropriate value
            float expected = 0F; // TODO: Initialize to an appropriate value
            float actual;
            actual = StatConversion.GetExpertiseFromRating(Rating, Class);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetDodgeFromRating
        ///</summary>
        [TestMethod()]
        public void GetDodgeFromRatingTest1()
        {
            float Rating = 0F; // TODO: Initialize to an appropriate value
            float expected = 0F; // TODO: Initialize to an appropriate value
            float actual;
            actual = StatConversion.GetDodgeFromRating(Rating);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetDodgeFromRating
        ///</summary>
        [TestMethod()]
        public void GetDodgeFromRatingTest()
        {
            float Rating = 0F; // TODO: Initialize to an appropriate value
            Character.CharacterClass Class = new Character.CharacterClass(); // TODO: Initialize to an appropriate value
            float expected = 0F; // TODO: Initialize to an appropriate value
            float actual;
            actual = StatConversion.GetDodgeFromRating(Rating, Class);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetDodgeFromAgility
        ///</summary>
        [TestMethod()]
        public void GetDodgeFromAgilityTest()
        {
            float Agility = 0F; // TODO: Initialize to an appropriate value
            Character.CharacterClass Class = new Character.CharacterClass(); // TODO: Initialize to an appropriate value
            float expected = 0F; // TODO: Initialize to an appropriate value
            float actual;
            actual = StatConversion.GetDodgeFromAgility(Agility, Class);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetCritFromRating
        ///</summary>
        [TestMethod()]
        public void GetCritFromRatingTest1()
        {
            float Rating = 0F; // TODO: Initialize to an appropriate value
            float expected = 0F; // TODO: Initialize to an appropriate value
            float actual;
            actual = StatConversion.GetCritFromRating(Rating);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetCritFromRating
        ///</summary>
        [TestMethod()]
        public void GetCritFromRatingTest()
        {
            float Rating = 0F; // TODO: Initialize to an appropriate value
            Character.CharacterClass Class = new Character.CharacterClass(); // TODO: Initialize to an appropriate value
            float expected = 0F; // TODO: Initialize to an appropriate value
            float actual;
            actual = StatConversion.GetCritFromRating(Rating, Class);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetCritFromAgility
        ///</summary>
        [TestMethod()]
        public void GetCritFromAgilityTest()
        {
            float Agility = 0F; // TODO: Initialize to an appropriate value
            Character.CharacterClass Class = new Character.CharacterClass(); // TODO: Initialize to an appropriate value
            float expected = 0F; // TODO: Initialize to an appropriate value
            float actual;
            actual = StatConversion.GetCritFromAgility(Agility, Class);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetCombatRatingModifier
        ///</summary>
        [TestMethod()]
        public void GetCombatRatingModifierTest1()
        {
            uint iLevel = 0; // TODO: Initialize to an appropriate value
            float expected = 0F; // TODO: Initialize to an appropriate value
            float actual;
            actual = StatConversion.GetCombatRatingModifier(iLevel);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetBlockFromRating
        ///</summary>
        [TestMethod()]
        public void GetBlockFromRatingTest1()
        {
            float Rating = 0F; // TODO: Initialize to an appropriate value
            float expected = 0F; // TODO: Initialize to an appropriate value
            float actual;
            actual = StatConversion.GetBlockFromRating(Rating);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetBlockFromRating
        ///</summary>
        [TestMethod()]
        public void GetBlockFromRatingTest()
        {
            float Rating = 0F; // TODO: Initialize to an appropriate value
            Character.CharacterClass Class = new Character.CharacterClass(); // TODO: Initialize to an appropriate value
            float expected = 0F; // TODO: Initialize to an appropriate value
            float actual;
            actual = StatConversion.GetBlockFromRating(Rating, Class);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetAverageResistance
        ///</summary>
        [TestMethod()]
        public void GetAverageResistanceTest()
        {
            int AttackerLevel = 0; // TODO: Initialize to an appropriate value
            int TargetLevel = 0; // TODO: Initialize to an appropriate value
            float TargetResistance = 0F; // TODO: Initialize to an appropriate value
            float AttackerSpellPenetration = 0F; // TODO: Initialize to an appropriate value
            float expected = 0F; // TODO: Initialize to an appropriate value
            float actual;
            actual = StatConversion.GetAverageResistance(AttackerLevel, TargetLevel, TargetResistance, AttackerSpellPenetration);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetArmorPenetrationFromRating
        ///</summary>
        [TestMethod()]
        public void GetArmorPenetrationFromRatingTest1()
        {
            float Rating = 0F; // TODO: Initialize to an appropriate value
            float expected = 0F; // TODO: Initialize to an appropriate value
            float actual;
            actual = StatConversion.GetArmorPenetrationFromRating(Rating);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetArmorPenetrationFromRating
        ///</summary>
        [TestMethod()]
        public void GetArmorPenetrationFromRatingTest()
        {
            float Rating = 0F; // TODO: Initialize to an appropriate value
            Character.CharacterClass Class = new Character.CharacterClass(); // TODO: Initialize to an appropriate value
            float expected = 0F; // TODO: Initialize to an appropriate value
            float actual;
            actual = StatConversion.GetArmorPenetrationFromRating(Rating, Class);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetArmorDamageReduction
        ///</summary>
        [TestMethod()]
        public void GetArmorDamageReductionTest()
        {
            int AttackerLevel = 0; // TODO: Initialize to an appropriate value
            float TargetArmor = 0F; // TODO: Initialize to an appropriate value
            float ArmorIgnoreDebuffs = 0F; // TODO: Initialize to an appropriate value
            float ArmorIgnoreBuffs = 0F; // TODO: Initialize to an appropriate value
            float ArmorPenetrationRating = 0F; // TODO: Initialize to an appropriate value
            float expected = 0F; // TODO: Initialize to an appropriate value
            float actual;
            actual = StatConversion.GetArmorDamageReduction(AttackerLevel, TargetArmor, ArmorIgnoreDebuffs, ArmorIgnoreBuffs, ArmorPenetrationRating);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for AttackerResistancePenalty
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Rawr.Base.dll")]
        public void AttackerResistancePenaltyTest()
        {
            int LevelDelta = 0; // TODO: Initialize to an appropriate value
            float expected = 0F; // TODO: Initialize to an appropriate value
            float actual;
            actual = StatConversion_Accessor.AttackerResistancePenalty(LevelDelta);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
         * 
         */

    }
}

