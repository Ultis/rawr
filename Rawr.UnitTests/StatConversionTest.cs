using Rawr;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Rawr.UnitTests
{
    
    
    /// <summary>
    ///This is a test class for StatConversionTest and is intended
    ///to contain all StatConversionTest Unit Tests
    ///</summary>
    [TestClass()]
    public class StatConversionTest
    {
        public static int HitResultCount = EnumHelper.GetCount(typeof(HitResult));

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
        ///A test for GetDodgeFromAgility
        ///</summary>
        [TestMethod()]
        public void GetDodgeFromAgilityTest()
        {
            float Agility = 1000F;
            float expected = 0F;
            float actual = 0f;
            for (int c = 1; c <= EnumHelper.GetCount(typeof(CharacterClass)); c++)
            {
                if (c != 10)
                {
                    actual = StatConversion.GetDodgeFromAgility(Agility, (CharacterClass)c);
                    if ((CharacterClass)c == CharacterClass.DeathKnight
                        || (CharacterClass)c == CharacterClass.Paladin
                        || (CharacterClass)c == CharacterClass.Warrior)
                    {
                        Assert.AreEqual(expected, actual);
                    }
                    else
                    {
                        // TODO: Update this to be more accurate for the non-plate classes.
                        Assert.AreNotEqual(expected, actual);
                    }
                }
            }
        }
    }
}

