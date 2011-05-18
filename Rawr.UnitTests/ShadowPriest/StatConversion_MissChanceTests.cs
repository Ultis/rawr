using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rawr;

namespace Rawr.UnitTests.ShadowPriest
{
    [TestClass]
    public class StatConversion_MissChanceTests
    {
        private const float TOLERANCE = .00005f;

        [TestMethod]
        public void SpellMiss_PVE_AboveTargetBy4()
        {
            float expected = 0.00f;

            var levelsAboveTarget = 4;

            var miss = StatConversion.GetSpellMiss(levelsAboveTarget, false);

            Assert.AreEqual(expected, miss, TOLERANCE);
        }

        [TestMethod]
        public void SpellMiss_PVE_AboveTargetBy3()
        {
            float expected = 0.01f;

            var levelsAboveTarget = 3;

            var miss = StatConversion.GetSpellMiss(levelsAboveTarget, false);

            Assert.AreEqual(expected, miss, TOLERANCE);
        }

        [TestMethod]
        public void SpellMiss_PVE_AboveTargetBy2()
        {
            float expected = 0.02f;

            var levelsAboveTarget = 2;

            var miss = StatConversion.GetSpellMiss(levelsAboveTarget, false);

            Assert.AreEqual(expected, miss, TOLERANCE);
        }

        [TestMethod]
        public void SpellMiss_PVE_AboveTargetBy1()
        {
            float expected = 0.03f;

            var levelsAboveTarget = 1;

            var miss = StatConversion.GetSpellMiss(levelsAboveTarget, false);

            Assert.AreEqual(expected, miss, TOLERANCE);
        }

        [TestMethod]
        public void SpellMiss_PVE_SameLevel()
        {
            float expected = 0.04f;

            var levelsBelowTarget = 0;

            var miss = StatConversion.GetSpellMiss(levelsBelowTarget, false);

            Assert.AreEqual(expected, miss, TOLERANCE);
        }

        [TestMethod]
        public void SpellMiss_PVE_BelowTargetBy1()
        {
            float expected = 0.05f;

            var levelsBelowTarget = 1;

            var miss = StatConversion.GetSpellMiss(-levelsBelowTarget, false);

            Assert.AreEqual(expected, miss, TOLERANCE);
        }

        [TestMethod]
        public void SpellMiss_PVE_BelowTargetBy2()
        {
            float expected = 0.06f;

            var levelsBelowTarget = 2;

            var miss = StatConversion.GetSpellMiss(-levelsBelowTarget, false);

            Assert.AreEqual(expected, miss, TOLERANCE);
        }

        [TestMethod]
        public void SpellMiss_PVE_BelowTargetBy3()
        {
            float expected = 0.17f;

            int levelsBelowTarget = 3;

            var miss = StatConversion.GetSpellMiss(-levelsBelowTarget, false);

            Assert.AreEqual(expected, miss, TOLERANCE);
        }

        [TestMethod]
        public void SpellMiss_PVE_BelowTargetBy4()
        {
            float expected = 0.28f;

            int levelsBelowTarget = 4;

            var miss = StatConversion.GetSpellMiss(-levelsBelowTarget, false);

            Assert.AreEqual(expected, miss, TOLERANCE);
        }

        [TestMethod]
        public void SpellMiss_PVE_BelowTargetBy5()
        {
            float expected = 0.39f;

            int levelsBelowTarget = 5;

            var miss = StatConversion.GetSpellMiss(-levelsBelowTarget, false);

            Assert.AreEqual(expected, miss, TOLERANCE);
        }
    }
}
