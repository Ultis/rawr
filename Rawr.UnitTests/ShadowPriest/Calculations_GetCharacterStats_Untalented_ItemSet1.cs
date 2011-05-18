using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rawr;

namespace Rawr.UnitTests.ShadowPriest
{
    [TestClass]
    public class Calculations_GetCharacterStats_Untalented_ItemSet1 : Calculations_GetCharacterStatsBase
    {
        #region Setup/Teardown

        protected override void SetupCharacter(Character character)
        {
            TestItemSets.LoadItemSet1(character);
        }

        #endregion

        [TestMethod]
        public void BloodElf_Health()
        {
            int expected = 96617;

            AssertStatCorrect(s => s.Health, expected, CharacterRace.BloodElf);
        }

        [TestMethod]
        public void BloodElf_Intellect()
        {
            int expected = 3187;

            AssertStatCorrect(s => s.Intellect, expected, CharacterRace.BloodElf);
        }

        [TestMethod]
        public void BloodElf_Mana()
        {
            int expected = 68115;

            AssertStatCorrect(s => s.Mana, expected, CharacterRace.BloodElf);
        }

        [TestMethod]
        public void BloodElf_SpellCrit()
        {
            float expected = 0.0995f;

            AssertStatCorrect(s => s.SpellCrit, expected, CharacterRace.BloodElf);
        }

        [TestMethod]
        public void BloodElf_CritRating()
        {
            int expected = 681;

            AssertStatCorrect(s => s.CritRating, expected, CharacterRace.BloodElf);
        }

        [TestMethod]
        public void BloodElf_HasteRating()
        {
            int expected = 923;

            AssertStatCorrect(s => s.HasteRating, expected, CharacterRace.BloodElf);
        }

        [TestMethod]
        public void BloodElf_HitRating()
        {
            int expected = 411;

            AssertStatCorrect(s => s.HitRating, expected, CharacterRace.BloodElf);
        }

        [TestMethod]
        public void BloodElf_SpellHaste()
        {
            float expected = 0.0721f;

            AssertStatCorrect(s => s.SpellHaste, expected, CharacterRace.BloodElf);
        }

        [TestMethod]
        public void BloodElf_SpellHit()
        {
            float expected = 0.0401f;

            AssertStatCorrect(s => s.SpellHit, expected, CharacterRace.BloodElf);
        }

        [TestMethod]
        public void BloodElf_SpellPower()
        {
            int expected = 4710;

            AssertStatCorrect(s => s.SpellPower, expected, CharacterRace.BloodElf);
        }

        [TestMethod]
        public void BloodElf_Spirit()
        {
            int expected = 1073;

            AssertStatCorrect(s => s.Spirit, expected, CharacterRace.BloodElf);
        }

        [TestMethod]
        public void BloodElf_Stamina()
        {
            int expected = 3828;

            AssertStatCorrect(s => s.Stamina, expected, CharacterRace.BloodElf);
        }

        [TestMethod]
        public void Dwarf_Health()
        {
            int expected = 96631;

            AssertStatCorrect(s => s.Health, expected, CharacterRace.Dwarf);
        }

        [TestMethod]
        public void Dwarf_Intellect()
        {
            int expected = 3183;

            AssertStatCorrect(s => s.Intellect, expected, CharacterRace.Dwarf);
        }

        [TestMethod]
        public void Dwarf_Mana()
        {
            int expected = 68055;

            AssertStatCorrect(s => s.Mana, expected, CharacterRace.Dwarf);
        }

        [TestMethod]
        public void Dwarf_SpellCrit()
        {
            float expected = 0.0994f;

            AssertStatCorrect(s => s.SpellCrit, expected, CharacterRace.Dwarf);
        }

        [TestMethod]
        public void Dwarf_CritRating()
        {
            int expected = 681;

            AssertStatCorrect(s => s.CritRating, expected, CharacterRace.Dwarf);
        }

        [TestMethod]
        public void Dwarf_HasteRating()
        {
            int expected = 923;

            AssertStatCorrect(s => s.HasteRating, expected, CharacterRace.Dwarf);
        }

        [TestMethod]
        public void Dwarf_HitRating()
        {
            int expected = 411;

            AssertStatCorrect(s => s.HitRating, expected, CharacterRace.Dwarf);
        }

        [TestMethod]
        public void Dwarf_SpellHaste()
        {
            float expected = 0.0721f;

            AssertStatCorrect(s => s.SpellHaste, expected, CharacterRace.Dwarf);
        }

        [TestMethod]
        public void Dwarf_SpellHit()
        {
            float expected = 0.0401f;

            AssertStatCorrect(s => s.SpellHit, expected, CharacterRace.Dwarf);
        }

        [TestMethod]
        public void Dwarf_SpellPower()
        {
            int expected = 4706;

            AssertStatCorrect(s => s.SpellPower, expected, CharacterRace.Dwarf);
        }

        [TestMethod]
        public void Dwarf_Spirit()
        {
            int expected = 1074;

            AssertStatCorrect(s => s.Spirit, expected, CharacterRace.Dwarf);
        }

        [TestMethod]
        public void Dwarf_Stamina()
        {
            int expected = 3829;

            AssertStatCorrect(s => s.Stamina, expected, CharacterRace.Dwarf);
        }

        [TestMethod]
        public void Goblin_Health()
        {
            int expected = 96617;

            AssertStatCorrect(s => s.Health, expected, CharacterRace.Goblin);
        }

        [TestMethod]
        public void Goblin_Intellect()
        {
            int expected = 3187;

            AssertStatCorrect(s => s.Intellect, expected, CharacterRace.Goblin);
        }

        [TestMethod]
        public void Goblin_Mana()
        {
            int expected = 68115;

            AssertStatCorrect(s => s.Mana, expected, CharacterRace.Goblin);
        }

        [TestMethod]
        public void Goblin_SpellCrit()
        {
            float expected = 0.0995f;

            AssertStatCorrect(s => s.SpellCrit, expected, CharacterRace.Goblin);
        }

        [TestMethod]
        public void Goblin_SpellHaste()
        {
            float expected = 0.0828f;

            AssertStatCorrect(s => s.SpellHaste, expected, CharacterRace.Goblin);
        }

        [TestMethod]
        public void Goblin_SpellHit()
        {
            float expected = 0.0401f;

            AssertStatCorrect(s => s.SpellHit, expected, CharacterRace.Goblin);
        }

        [TestMethod]
        public void Goblin_SpellPower()
        {
            int expected = 4710;

            AssertStatCorrect(s => s.SpellPower, expected, CharacterRace.Goblin);
        }

        [TestMethod]
        public void Goblin_Spirit()
        {
            int expected = 1073;

            AssertStatCorrect(s => s.Spirit, expected, CharacterRace.Goblin);
        }

        [TestMethod]
        public void Goblin_Stamina()
        {
            int expected = 3828;

            AssertStatCorrect(s => s.Stamina, expected, CharacterRace.Goblin);
        }

        [TestMethod]
        public void Goblin_CritRating()
        {
            int expected = 681;

            AssertStatCorrect(s => s.CritRating, expected, CharacterRace.Goblin);
        }

        [TestMethod]
        public void Goblin_HasteRating()
        {
            int expected = 923;

            AssertStatCorrect(s => s.HasteRating, expected, CharacterRace.Goblin);
        }

        [TestMethod]
        public void Goblin_HitRating()
        {
            int expected = 411;

            AssertStatCorrect(s => s.HitRating, expected, CharacterRace.Goblin);
        }

        [TestMethod]
        public void Human_Health()
        {
            int expected = 96617;

            AssertStatCorrect(s => s.Health, expected, CharacterRace.Human);
        }

        [TestMethod]
        public void Human_Intellect()
        {
            int expected = 3184;

            AssertStatCorrect(s => s.Intellect, expected, CharacterRace.Human);
        }

        [TestMethod]
        public void Human_Mana()
        {
            int expected = 68070;

            AssertStatCorrect(s => s.Mana, expected, CharacterRace.Human);
        }

        [TestMethod]
        public void Human_SpellCrit()
        {
            float expected = 0.0994f;

            AssertStatCorrect(s => s.SpellCrit, expected, CharacterRace.Human);
        }

        [TestMethod]
        public void Human_SpellHaste()
        {
            float expected = 0.0721f;

            AssertStatCorrect(s => s.SpellHaste, expected, CharacterRace.Human);
        }

        [TestMethod]
        public void Human_SpellHit()
        {
            float expected = 0.0401f;

            AssertStatCorrect(s => s.SpellHit, expected, CharacterRace.Human);
        }

        [TestMethod]
        public void Human_SpellPower()
        {
            int expected = 4707;

            AssertStatCorrect(s => s.SpellPower, expected, CharacterRace.Human);
        }

        [TestMethod]
        public void Human_Spirit()
        {
            int expected = 1106;

            AssertStatCorrect(s => s.Spirit, expected, CharacterRace.Human);
        }

        [TestMethod]
        public void Human_Stamina()
        {
            int expected = 3828;

            AssertStatCorrect(s => s.Stamina, expected, CharacterRace.Human);
        }

        [TestMethod]
        public void Human_CritRating()
        {
            int expected = 681;

            AssertStatCorrect(s => s.CritRating, expected, CharacterRace.Human);
        }

        [TestMethod]
        public void Human_HasteRating()
        {
            int expected = 923;

            AssertStatCorrect(s => s.HasteRating, expected, CharacterRace.Human);
        }

        [TestMethod]
        public void Human_HitRating()
        {
            int expected = 411;

            AssertStatCorrect(s => s.HitRating, expected, CharacterRace.Human);
        }


        [TestMethod]
        public void Tauren_Health()
        {
            int expected = 98795;

            AssertStatCorrect(s => s.Health, expected, CharacterRace.Tauren);
        }

        [TestMethod]
        public void Tauren_Intellect()
        {
            int expected = 3180;

            AssertStatCorrect(s => s.Intellect, expected, CharacterRace.Tauren);
        }

        [TestMethod]
        public void Tauren_Mana()
        {
            int expected = 68010;

            AssertStatCorrect(s => s.Mana, expected, CharacterRace.Tauren);
        }

        [TestMethod]
        public void Tauren_SpellCrit()
        {
            float expected = 0.0994f;

            AssertStatCorrect(s => s.SpellCrit, expected, CharacterRace.Tauren);
        }

        [TestMethod]
        public void Tauren_CritRating()
        {
            int expected = 681;

            AssertStatCorrect(s => s.CritRating, expected, CharacterRace.Tauren);
        }

        [TestMethod]
        public void Tauren_HasteRating()
        {
            int expected = 923;

            AssertStatCorrect(s => s.HasteRating, expected, CharacterRace.Tauren);
        }

        [TestMethod]
        public void Tauren_HitRating()
        {
            int expected = 411;

            AssertStatCorrect(s => s.HitRating, expected, CharacterRace.Tauren);
        }

        [TestMethod]
        public void Tauren_SpellHaste()
        {
            float expected = 0.0721f;

            AssertStatCorrect(s => s.SpellHaste, expected, CharacterRace.Tauren);
        }

        [TestMethod]
        public void Tauren_SpellHit()
        {
            float expected = 0.0401f;

            AssertStatCorrect(s => s.SpellHit, expected, CharacterRace.Tauren);
        }

        [TestMethod]
        public void Tauren_SpellPower()
        {
            int expected = 4703;

            AssertStatCorrect(s => s.SpellPower, expected, CharacterRace.Tauren);
        }

        [TestMethod]
        public void Tauren_Spirit()
        {
            int expected = 1077;

            AssertStatCorrect(s => s.Spirit, expected, CharacterRace.Tauren);
        }

        [TestMethod]
        public void Tauren_Stamina()
        {
            int expected = 3829;

            AssertStatCorrect(s => s.Stamina, expected, CharacterRace.Tauren);
        }


        [TestMethod]
        public void Troll_Health()
        {
            int expected = 96617;

            AssertStatCorrect(s => s.Health, expected, CharacterRace.Troll);
        }

        [TestMethod]
        public void Troll_Intellect()
        {
            int expected = 3180;

            AssertStatCorrect(s => s.Intellect, expected, CharacterRace.Troll);
        }

        [TestMethod]
        public void Troll_Mana()
        {
            int expected = 68010;

            AssertStatCorrect(s => s.Mana, expected, CharacterRace.Troll);
        }

        [TestMethod]
        public void Troll_SpellCrit()
        {
            float expected = 0.0994f;

            AssertStatCorrect(s => s.SpellCrit, expected, CharacterRace.Troll);
        }

        [TestMethod]
        public void Troll_CritRating()
        {
            int expected = 681;

            AssertStatCorrect(s => s.CritRating, expected, CharacterRace.Troll);
        }

        [TestMethod]
        public void Troll_HasteRating()
        {
            int expected = 923;

            AssertStatCorrect(s => s.HasteRating, expected, CharacterRace.Troll);
        }

        [TestMethod]
        public void Troll_HitRating()
        {
            int expected = 411;

            AssertStatCorrect(s => s.HitRating, expected, CharacterRace.Troll);
        }

        [TestMethod]
        public void Troll_SpellHaste()
        {
            float expected = 0.0721f;

            AssertStatCorrect(s => s.SpellHaste, expected, CharacterRace.Troll);
        }

        [TestMethod]
        public void Troll_SpellHit()
        {
            float expected = 0.0401f;

            AssertStatCorrect(s => s.SpellHit, expected, CharacterRace.Troll);
        }

        [TestMethod]
        public void Troll_SpellPower()
        {
            int expected = 4703;

            AssertStatCorrect(s => s.SpellPower, expected, CharacterRace.Troll);
        }

        [TestMethod]
        public void Troll_Spirit()
        {
            int expected = 1076;

            AssertStatCorrect(s => s.Spirit, expected, CharacterRace.Troll);
        }

        [TestMethod]
        public void Troll_Stamina()
        {
            int expected = 3828;

            AssertStatCorrect(s => s.Stamina, expected, CharacterRace.Troll);
        }
    }
}