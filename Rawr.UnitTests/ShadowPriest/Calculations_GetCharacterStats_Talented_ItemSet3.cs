using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rawr;

namespace Rawr.UnitTests.ShadowPriest
{
    /// <summary>
    /// Naked Dwarf Testing
    /// </summary>
    [TestClass]
    public class Calculations_GetCharacterStats_Talented_ItemSet3 : Calculations_GetCharacterStatsBase
    {
        protected override void SetupCharacter(Character character)
        {
            TestItemSets.LoadItemSet3(character);
            TestTalentSets.LoadSuggested(character.PriestTalents);
        }

        [TestMethod]
        public void Draenei_Health()
        {
            int expected = 107845;

            AssertStatCorrect(s => s.Health, expected, CharacterRace.Draenei);
        }

        [TestMethod]
        public void Draenei_Intellect()
        {
            int expected = 3158;

            AssertStatCorrect(s => s.Intellect, expected, CharacterRace.Draenei);
        }

        [TestMethod]
        public void Draenei_Mana()
        {
            int expected = 67680;

            AssertStatCorrect(s => s.Mana, expected, CharacterRace.Draenei);
        }

        [TestMethod]
        public void Draenei_SpellCrit()
        {
            float expected = 0.1476f;

            AssertStatCorrect(s => s.SpellCrit, expected, CharacterRace.Draenei);
        }

        [TestMethod]
        public void Draenei_SpellHaste()
        {
            float expected = 0.1303f;

            AssertStatCorrect(s => s.SpellHaste, expected, CharacterRace.Draenei);
        }

        [TestMethod]
        public void Draenei_SpellHit()
        {
            float expected = 0.0495f;

            AssertStatCorrect(s => s.SpellHit, expected, CharacterRace.Draenei);
        }

        [TestMethod]
        public void Draenei_SpellPower()
        {
            int expected = 4877;

            AssertStatCorrect(s => s.SpellPower, expected, CharacterRace.Draenei);
        }

        [TestMethod]
        public void Draenei_Spirit()
        {
            int expected = 320;

            AssertStatCorrect(s => s.Spirit, expected, CharacterRace.Draenei);
        }

        [TestMethod]
        public void Draenei_Stamina()
        {
            int expected = 4630;

            AssertStatCorrect(s => s.Stamina, expected, CharacterRace.Draenei);
        }

        [TestMethod]
        public void Gnome_Health()
        {
            int expected = 107845;

            AssertStatCorrect(s => s.Health, expected, CharacterRace.Gnome);
        }

        [TestMethod]
        public void Gnome_Intellect()
        {
            int expected = 3161;

            AssertStatCorrect(s => s.Intellect, expected, CharacterRace.Gnome);
        }

        [TestMethod]
        public void Gnome_Mana()
        {
            int expected = 71111;

            AssertStatCorrect(s => s.Mana, expected, CharacterRace.Gnome);
        }

        [TestMethod]
        public void Gnome_SpellCrit()
        {
            float expected = 0.1476f;

            AssertStatCorrect(s => s.SpellCrit, expected, CharacterRace.Gnome);
        }

        [TestMethod]
        public void Gnome_SpellHaste()
        {
            float expected = 0.1303f;

            AssertStatCorrect(s => s.SpellHaste, expected, CharacterRace.Gnome);
        }

        [TestMethod]
        public void Gnome_SpellHit()
        {
            float expected = 0.0395f;

            AssertStatCorrect(s => s.SpellHit, expected, CharacterRace.Gnome);
        }

        [TestMethod]
        public void Gnome_SpellPower()
        {
            int expected = 4880;

            AssertStatCorrect(s => s.SpellPower, expected, CharacterRace.Gnome);
        }

        [TestMethod]
        public void Gnome_Spirit()
        {
            int expected = 318;

            AssertStatCorrect(s => s.Spirit, expected, CharacterRace.Gnome);
        }

        [TestMethod]
        public void Gnome_Stamina()
        {
            int expected = 4630;

            AssertStatCorrect(s => s.Stamina, expected, CharacterRace.Gnome);
        }

        [TestMethod]
        public void NightElf_Health()
        {
            int expected = 107845;

            AssertStatCorrect(s => s.Health, expected, CharacterRace.NightElf);
        }

        [TestMethod]
        public void NightElf_Intellect()
        {
            int expected = 3158;

            AssertStatCorrect(s => s.Intellect, expected, CharacterRace.NightElf);
        }

        [TestMethod]
        public void NightElf_Mana()
        {
            int expected = 67680;

            AssertStatCorrect(s => s.Mana, expected, CharacterRace.NightElf);
        }

        [TestMethod]
        public void NightElf_SpellCrit()
        {
            float expected = 0.1476f;

            AssertStatCorrect(s => s.SpellCrit, expected, CharacterRace.NightElf);
        }

        [TestMethod]
        public void NightElf_SpellHaste()
        {
            float expected = 0.1303f;

            AssertStatCorrect(s => s.SpellHaste, expected, CharacterRace.NightElf);
        }

        [TestMethod]
        public void NightElf_SpellHit()
        {
            float expected = 0.0395f;

            AssertStatCorrect(s => s.SpellHit, expected, CharacterRace.NightElf);
        }

        [TestMethod]
        public void NightElf_SpellPower()
        {
            int expected = 4877;

            AssertStatCorrect(s => s.SpellPower, expected, CharacterRace.NightElf);
        }

        [TestMethod]
        public void NightElf_Spirit()
        {
            int expected = 318;

            AssertStatCorrect(s => s.Spirit, expected, CharacterRace.NightElf);
        }

        [TestMethod]
        public void NightElf_Stamina()
        {
            int expected = 4630;

            AssertStatCorrect(s => s.Stamina, expected, CharacterRace.NightElf);
        }


        [TestMethod]
        public void Forsaken_Health()
        {
            int expected = 107845;

            AssertStatCorrect(s => s.Health, expected, CharacterRace.Undead);
        }

        [TestMethod]
        public void Forsaken_Intellect()
        {
            int expected = 3156;

            AssertStatCorrect(s => s.Intellect, expected, CharacterRace.Undead);
        }

        [TestMethod]
        public void Forsaken_Mana()
        {
            int expected = 67650;

            AssertStatCorrect(s => s.Mana, expected, CharacterRace.Undead);
        }

        [TestMethod]
        public void Forsaken_SpellCrit()
        {
            float expected = 0.1475f;

            AssertStatCorrect(s => s.SpellCrit, expected, CharacterRace.Undead);
        }

        [TestMethod]
        public void Forsaken_SpellHaste()
        {
            float expected = 0.1303f;

            AssertStatCorrect(s => s.SpellHaste, expected, CharacterRace.Undead);
        }

        [TestMethod]
        public void Forsaken_SpellHit()
        {
            float expected = 0.0395f;

            AssertStatCorrect(s => s.SpellHit, expected, CharacterRace.Undead);
        }

        [TestMethod]
        public void Forsaken_SpellPower()
        {
            int expected = 4875;

            AssertStatCorrect(s => s.SpellPower, expected, CharacterRace.Undead);
        }

        [TestMethod]
        public void Forsaken_Spirit()
        {
            int expected = 323;

            AssertStatCorrect(s => s.Spirit, expected, CharacterRace.Undead);
        }

        [TestMethod]
        public void Forsaken_Stamina()
        {
            int expected = 4630;

            AssertStatCorrect(s => s.Stamina, expected, CharacterRace.Undead);
        }


        [TestMethod]
        public void Worgen_Health()
        {
            int expected = 107845;

            AssertStatCorrect(s => s.Health, expected, CharacterRace.Worgen);
        }

        [TestMethod]
        public void Worgen_Intellect()
        {
            int expected = 3154;

            AssertStatCorrect(s => s.Intellect, expected, CharacterRace.Worgen);
        }

        [TestMethod]
        public void Worgen_Mana()
        {
            int expected = 67620;

            AssertStatCorrect(s => s.Mana, expected, CharacterRace.Worgen);
        }

        [TestMethod]
        public void Worgen_SpellCrit()
        {
            float expected = 0.1575f;

            AssertStatCorrect(s => s.SpellCrit, expected, CharacterRace.Worgen);
        }

        [TestMethod]
        public void Worgen_SpellHaste()
        {
            float expected = 0.1303f;

            AssertStatCorrect(s => s.SpellHaste, expected, CharacterRace.Worgen);
        }

        [TestMethod]
        public void Worgen_SpellHit()
        {
            float expected = 0.0395f;

            AssertStatCorrect(s => s.SpellHit, expected, CharacterRace.Worgen);
        }

        [TestMethod]
        public void Worgen_SpellPower()
        {
            int expected = 4873;

            AssertStatCorrect(s => s.SpellPower, expected, CharacterRace.Worgen);
        }

        [TestMethod]
        public void Worgen_Spirit()
        {
            int expected = 317;

            AssertStatCorrect(s => s.Spirit, expected, CharacterRace.Worgen);
        }

        [TestMethod]
        public void Worgen_Stamina()
        {
            int expected = 4630;

            AssertStatCorrect(s => s.Stamina, expected, CharacterRace.Worgen);
        }
    }
}