using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rawr.Rogue;
using Rawr.Rogue.ClassAbilities;

namespace Rawr.UnitTests.Rawr.Rogue.Tests
{
    /// <summary>
    /// Tests the Properties and Methods in the Rawr.Rogue.CombatFactors class
    /// </summary>
    [TestClass]
    public class CombatFactorsTests
    {
        #region Crap required by VS Test class
        public CombatFactorsTests(){ }
        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }
        #endregion

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
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void BaseEnergyRegen()
        {
            var combatFactors = CreateCombatFactors(new RogueTalents());
            AssertRoundedValues(10f, combatFactors.BaseEnergyRegen);
        }

        [TestMethod]
        public void RegenWithAdrenalineRush()
        {
            var combatFactors = CreateCombatFactors(new RogueTalents { AdrenalineRush = 1});
            AssertRoundedValues(10.83333f, combatFactors.BaseEnergyRegen);
        }

        [TestMethod]
        public void RegenWithAdrenalineRushGlyph()
        {
            var combatFactors = CreateCombatFactors(new RogueTalents {AdrenalineRush = 1, GlyphOfAdrenalineRush = true});
            AssertRoundedValues(11.111111f, combatFactors.BaseEnergyRegen);
        }

        [TestMethod]
        public void RegenWithVitality()
        {
            var combatFactors = CreateCombatFactors(new RogueTalents { Vitality = 3 });
            AssertRoundedValues(12.5f, combatFactors.BaseEnergyRegen);
        }

        [TestMethod]
        public void RegenWithBladeFlurry()
        {
            var combatFactors = CreateCombatFactors(new RogueTalents { BladeFlurry = 1}); 
            AssertRoundedValues(9.7916667f, combatFactors.BaseEnergyRegen);
        }

        [TestMethod]
        public void RegenWithBladeFlurryGlyph()
        {
            var combatFactors = CreateCombatFactors(new RogueTalents { BladeFlurry = 1, GlyphOfBladeFlurry = true });
            AssertRoundedValues(10f, combatFactors.BaseEnergyRegen);
        }

        public void AssertRoundedValues(float expected, float actual)
        {
            Assert.AreEqual(Math.Round(expected, 4), Math.Round(actual, 4));
        }

        public CombatFactors CreateCombatFactors( RogueTalents talents )
        {
            var character = new RogueTestCharacter(talents);
            TalentsAndGlyphs.Initialize(character.RogueTalents);
            return new CombatFactors(character, new Stats());
        }
    }

    internal class RogueTestCharacter : Character
    {
        public RogueTestCharacter():this(new RogueTalents()){}
        public RogueTestCharacter(RogueTalents talents)
        {
            Class = CharacterClass.Rogue;
            RogueTalents = talents;
            CurrentModel = "Rogue";
            CalculationOptions = new CalculationOptionsRogue();
        }
    }
}
