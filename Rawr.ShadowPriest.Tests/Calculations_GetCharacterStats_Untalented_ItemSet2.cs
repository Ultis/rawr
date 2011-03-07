using NUnit.Framework;
using Rawr;

namespace Tests.Rawr.ShadowPriest
{
    /// <summary>
    /// Naked Dwarf Testing
    /// </summary>
    [TestFixture]
    public class Calculations_GetCharacterStats_Untalented_ItemSet2 : Calculations_GetCharacterStatsBase
    {
        protected override void SetupCharacter(Character character)
        {
            TestItemSets.LoadItemSet2(character);
        }

        [Test]
        public void Draenei_Health()
        {
            int expected = 107845;

            AssertStatCorrect(s => s.Health, expected, CharacterRace.Draenei);
        }

        [Test]
        public void Draenei_Intellect()
        {
            int expected = 3458;

            AssertStatCorrect(s => s.Intellect, expected, CharacterRace.Draenei);
        }

        [Test]
        public void Draenei_Mana()
        {
            int expected = 72180;

            AssertStatCorrect(s => s.Mana, expected, CharacterRace.Draenei);
        }

        [Test]
        public void Draenei_SpellCrit()
        {
            float expected = 0.1317f;

            AssertStatCorrect(s => s.SpellCrit, expected, CharacterRace.Draenei);
        }

        [Test]
        public void Draenei_SpellHaste()
        {
            float expected = 0.0533f;

            AssertStatCorrect(s => s.SpellHaste, expected, CharacterRace.Draenei);
        }

        [Test]
        public void Draenei_SpellHit()
        {
            float expected = 0.01f;

            AssertStatCorrect(s => s.SpellHit, expected, CharacterRace.Draenei);
        }

        [Test]
        public void Draenei_SpellPower()
        {
            int expected = 5177;

            AssertStatCorrect(s => s.SpellPower, expected, CharacterRace.Draenei);
        }

        [Test]
        public void Draenei_Spirit()
        {
            int expected = 2016;

            AssertStatCorrect(s => s.Spirit, expected, CharacterRace.Draenei);
        }

        [Test]
        public void Draenei_Stamina()
        {
            int expected = 4630;

            AssertStatCorrect(s => s.Stamina, expected, CharacterRace.Draenei);
        }

        [Test]
        public void Gnome_Health()
        {
            int expected = 107845;

            AssertStatCorrect(s => s.Health, expected, CharacterRace.Gnome);
        }

        [Test]
        public void Gnome_Intellect()
        {
            int expected = 3461;

            AssertStatCorrect(s => s.Intellect, expected, CharacterRace.Gnome);
        }

        [Test]
        public void Gnome_Mana()
        {
            int expected = 75836;

            AssertStatCorrect(s => s.Mana, expected, CharacterRace.Gnome);
        }

        [Test]
        public void Gnome_SpellCrit()
        {
            float expected = 0.1318f;

            AssertStatCorrect(s => s.SpellCrit, expected, CharacterRace.Gnome);
        }

        [Test]
        public void Gnome_SpellHaste()
        {
            float expected = 0.0533f;

            AssertStatCorrect(s => s.SpellHaste, expected, CharacterRace.Gnome);
        }

        [Test]
        public void Gnome_SpellHit()
        {
            float expected = 0.00f;

            AssertStatCorrect(s => s.SpellHit, expected, CharacterRace.Gnome);
        }

        [Test]
        public void Gnome_SpellPower()
        {
            int expected = 5180;

            AssertStatCorrect(s => s.SpellPower, expected, CharacterRace.Gnome);
        }

        [Test]
        public void Gnome_Spirit()
        {
            int expected = 2014;

            AssertStatCorrect(s => s.Spirit, expected, CharacterRace.Gnome);
        }

        [Test]
        public void Gnome_Stamina()
        {
            int expected = 4630;

            AssertStatCorrect(s => s.Stamina, expected, CharacterRace.Gnome);
        }

        [Test]
        public void NightElf_Health()
        {
            int expected = 107845;

            AssertStatCorrect(s => s.Health, expected, CharacterRace.NightElf);
        }

        [Test]
        public void NightElf_Intellect()
        {
            int expected = 3458;

            AssertStatCorrect(s => s.Intellect, expected, CharacterRace.NightElf);
        }

        [Test]
        public void NightElf_Mana()
        {
            int expected = 72180;

            AssertStatCorrect(s => s.Mana, expected, CharacterRace.NightElf);
        }

        [Test]
        public void NightElf_SpellCrit()
        {
            float expected = 0.1317f;

            AssertStatCorrect(s => s.SpellCrit, expected, CharacterRace.NightElf);
        }

        [Test]
        public void NightElf_SpellHaste()
        {
            float expected = 0.0533f;

            AssertStatCorrect(s => s.SpellHaste, expected, CharacterRace.NightElf);
        }

        [Test]
        public void NightElf_SpellHit()
        {
            float expected = 0.0f;

            AssertStatCorrect(s => s.SpellHit, expected, CharacterRace.NightElf);
        }

        [Test]
        public void NightElf_SpellPower()
        {
            int expected = 5177;

            AssertStatCorrect(s => s.SpellPower, expected, CharacterRace.NightElf);
        }

        [Test]
        public void NightElf_Spirit()
        {
            int expected = 2014;

            AssertStatCorrect(s => s.Spirit, expected, CharacterRace.NightElf);
        }

        [Test]
        public void NightElf_Stamina()
        {
            int expected = 4630;

            AssertStatCorrect(s => s.Stamina, expected, CharacterRace.NightElf);
        }

        [Test]
        public void Forsaken_Health()
        {
            int expected = 107845;

            AssertStatCorrect(s => s.Health, expected, CharacterRace.Undead);
        }

        [Test]
        public void Forsaken_Intellect()
        {
            int expected = 3456;

            AssertStatCorrect(s => s.Intellect, expected, CharacterRace.Undead);
        }

        [Test]
        public void Forsaken_Mana()
        {
            int expected = 72150;

            AssertStatCorrect(s => s.Mana, expected, CharacterRace.Undead);
        }

        [Test]
        public void Forsaken_SpellCrit()
        {
            float expected = 0.1317f;

            AssertStatCorrect(s => s.SpellCrit, expected, CharacterRace.Undead);
        }

        [Test]
        public void Forsaken_SpellHaste()
        {
            float expected = 0.0533f;

            AssertStatCorrect(s => s.SpellHaste, expected, CharacterRace.Undead);
        }

        [Test]
        public void Forsaken_SpellHit()
        {
            float expected = 0.0f;

            AssertStatCorrect(s => s.SpellHit, expected, CharacterRace.Undead);
        }

        [Test]
        public void Forsaken_SpellPower()
        {
            int expected = 5175;

            AssertStatCorrect(s => s.SpellPower, expected, CharacterRace.Undead);
        }

        [Test]
        public void Forsaken_Spirit()
        {
            int expected = 2019;

            AssertStatCorrect(s => s.Spirit, expected, CharacterRace.Undead);
        }

        [Test]
        public void Forsaken_Stamina()
        {
            int expected = 4630;

            AssertStatCorrect(s => s.Stamina, expected, CharacterRace.Undead);
        }

        [Test]
        public void Worgen_Health()
        {
            int expected = 107845;

            AssertStatCorrect(s => s.Health, expected, CharacterRace.Worgen);
        }

        [Test]
        public void Worgen_Intellect()
        {
            int expected = 3454;

            AssertStatCorrect(s => s.Intellect, expected, CharacterRace.Worgen);
        }

        [Test]
        public void Worgen_Mana()
        {
            int expected = 72120;

            AssertStatCorrect(s => s.Mana, expected, CharacterRace.Worgen);
        }

        [Test]
        public void Worgen_SpellCrit()
        {
            float expected = 0.1416f;

            AssertStatCorrect(s => s.SpellCrit, expected, CharacterRace.Worgen);
        }

        [Test]
        public void Worgen_SpellHaste()
        {
            float expected = 0.0533f;

            AssertStatCorrect(s => s.SpellHaste, expected, CharacterRace.Worgen);
        }

        [Test]
        public void Worgen_SpellHit()
        {
            float expected = 0.0f;

            AssertStatCorrect(s => s.SpellHit, expected, CharacterRace.Worgen);
        }

        [Test]
        public void Worgen_SpellPower()
        {
            int expected = 5173;

            AssertStatCorrect(s => s.SpellPower, expected, CharacterRace.Worgen);
        }

        [Test]
        public void Worgen_Spirit()
        {
            int expected = 2013;

            AssertStatCorrect(s => s.Spirit, expected, CharacterRace.Worgen);
        }

        [Test]
        public void Worgen_Stamina()
        {
            int expected = 4630;

            AssertStatCorrect(s => s.Stamina, expected, CharacterRace.Worgen);
        }
    }
}