using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rawr.RestoSham;

namespace Rawr.UnitTests.RestoSham
{
    /// <summary>
    /// Summary description for RestoShamStatsTest
    /// </summary>
    [TestClass]
    public class RestoShamStatsTest
    {
        [TestMethod]
        public void HasRelevantStatsTest()
        {
            CalculationsRestoSham calc = new CalculationsRestoSham();
            {
                Stats s = new Stats()
                {
                    Hp5 = 1
                };
                Assert.IsTrue(calc.HasRelevantStats(s));
            }
            {
                Stats s = new Stats()
                {
                    Mage2T10 = 1
                };
                Assert.IsFalse(calc.HasRelevantStats(s));
            }
            {
                Stats s = new Stats()
                {
                    Mage2T10 = 1,
                    Mana = 1
                };
                Assert.IsTrue(calc.HasRelevantStats(s));
            }
            {
                Stats s = new Stats()
                {
                    Mage2T10 = 1,
                    Mana = 1,
                    AttackPower = 10000,
                    Mp5 = 100,
                    TotemCHBaseCost = 1,
                    ThreatIncreaseMultiplier = 0
                };
                Assert.IsTrue(calc.HasRelevantStats(s));
            }
        }

        [TestMethod]
        public void GetRelevantStatsTest()
        {
            CalculationsRestoSham calc = new CalculationsRestoSham();
            {
                Stats s = new Stats()
                {
                    Hp5 = 1
                };
                Assert.AreEqual<Stats>(s, calc.GetRelevantStats(s));
            }
            {
                Stats s = new Stats()
                {
                    Mage2T10 = 1
                };
                Assert.AreNotEqual<Stats>(s, calc.GetRelevantStats(s));
                Stats correct = new Stats();
                Assert.AreEqual<Stats>(correct, calc.GetRelevantStats(s));
            }
            {
                Stats full = new Stats()
                {
                    Mage2T10 = 1,
                    Mana = 1
                };
                Stats wanted = new Stats()
                {
                    Mana = 1
                };
                Assert.AreNotEqual<Stats>(full, calc.GetRelevantStats(full));
                Assert.AreEqual<Stats>(wanted, calc.GetRelevantStats(full));
            }
            {
                Stats full = new Stats()
                {
                    Mage2T10 = 1,
                    Mana = 1,
                    AttackPower = 10000,
                    Mp5 = 100,
                    TotemCHBaseCost = 1,
                    ThreatIncreaseMultiplier = 0
                };
                Stats wanted = new Stats()
                {
                    Mana = 1,
                    Mp5 = 100,
                    TotemCHBaseCost = 1
                };
                Assert.AreNotEqual<Stats>(full, calc.GetRelevantStats(full));
                Assert.AreEqual<Stats>(wanted, calc.GetRelevantStats(full));
            }
        }
    }
}
