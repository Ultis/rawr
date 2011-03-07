using NUnit.Framework;
using Rawr;

namespace Tests.Rawr.ShadowPriest
{
    /// <summary>
    /// Naked Testing
    /// </summary>
    [TestFixture]
    public class Calculations_GetCharacterStats_Naked : Calculations_GetCharacterStatsBase
    {
        protected override void SetupCharacter(Character character)
        {
            
        }

        [Test]
        public void BloodElf_Health()
        {
            int expected = 44019;

            AssertStatCorrect(s => s.Health, expected, CharacterRace.BloodElf);
        }

        [Test]
        public void BloodElf_Intellect()
        {
            int expected = 202;

            AssertStatCorrect(s => s.Intellect, expected, CharacterRace.BloodElf);
        }

        [Test]
        public void BloodElf_Mana()
        {
            int expected = 23340;

            AssertStatCorrect(s => s.Mana, expected, CharacterRace.BloodElf);
        }

        [Test]
        public void BloodElf_SpellCrit()
        {
            float expected = 0.0155f;

            AssertStatCorrect(s => s.SpellCrit, expected, CharacterRace.BloodElf);
        }

        [Test]
        public void BloodElf_SpellHaste()
        {
            float expected = 0.00f;

            AssertStatCorrect(s => s.SpellHaste, expected, CharacterRace.BloodElf);
        }

        [Test]
        public void BloodElf_SpellHit()
        {
            float expected = 0.0f;

            AssertStatCorrect(s => s.SpellHit, expected, CharacterRace.BloodElf);
        }

        [Test]
        public void BloodElf_SpellPower()
        {
            int expected = 192;

            AssertStatCorrect(s => s.SpellPower, expected, CharacterRace.BloodElf);
        }

        [Test]
        public void BloodElf_Spirit()
        {
            int expected = 196;

            AssertStatCorrect(s => s.Spirit, expected, CharacterRace.BloodElf);
        }

        [Test]
        public void BloodElf_Stamina()
        {
            int expected = 71;

            AssertStatCorrect(s => s.Stamina, expected, CharacterRace.BloodElf);
        }

        [Test]
        public void Draenei_Health()
        {
            int expected = 44019;

            AssertStatCorrect(s => s.Health, expected, CharacterRace.Draenei);
        }

        [Test]
        public void Draenei_Intellect()
        {
            int expected = 199;

            AssertStatCorrect(s => s.Intellect, expected, CharacterRace.Draenei);
        }

        [Test]
        public void Draenei_Mana()
        {
            int expected = 23295;

            AssertStatCorrect(s => s.Mana, expected, CharacterRace.Draenei);
        }

        [Test]
        public void Draenei_SpellCrit()
        {
            float expected = 0.0154f;

            AssertStatCorrect(s => s.SpellCrit, expected, CharacterRace.Draenei);
        }

        [Test]
        public void Draenei_SpellHaste()
        {
            float expected = 0.0f;

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
            int expected = 189;

            AssertStatCorrect(s => s.SpellPower, expected, CharacterRace.Draenei);
        }

        [Test]
        public void Draenei_Spirit()
        {
            int expected = 200;

            AssertStatCorrect(s => s.Spirit, expected, CharacterRace.Draenei);
        }

        [Test]
        public void Draenei_Stamina()
        {
            int expected = 71;

            AssertStatCorrect(s => s.Stamina, expected, CharacterRace.Draenei);
        }

        [Test]
        public void Dwarf_Health()
        {
            int expected = 44033;

            AssertStatCorrect(s => s.Health, expected, CharacterRace.Dwarf);
        }

        [Test]
        public void Dwarf_Intellect()
        {
            int expected = 198;

            AssertStatCorrect(s => s.Intellect, expected, CharacterRace.Dwarf);
        }

        [Test]
        public void Dwarf_Mana()
        {
            int expected = 23280;

            AssertStatCorrect(s => s.Mana, expected, CharacterRace.Dwarf);
        }

        [Test]
        public void Dwarf_SpellCrit()
        {
            float expected = 0.0154f;

            AssertStatCorrect(s => s.SpellCrit, expected, CharacterRace.Dwarf);
        }

        [Test]
        public void Dwarf_SpellHaste()
        {
            float expected = 0.0f;

            AssertStatCorrect(s => s.SpellHaste, expected, CharacterRace.Dwarf);
        }

        [Test]
        public void Dwarf_SpellHit()
        {
            float expected = 0.0f;

            AssertStatCorrect(s => s.SpellHit, expected, CharacterRace.Dwarf);
        }

        [Test]
        public void Dwarf_SpellPower()
        {
            int expected = 188;

            AssertStatCorrect(s => s.SpellPower, expected, CharacterRace.Dwarf);
        }

        [Test]
        public void Dwarf_Spirit()
        {
            int expected = 197;

            AssertStatCorrect(s => s.Spirit, expected, CharacterRace.Dwarf);
        }

        [Test]
        public void Dwarf_Stamina()
        {
            int expected = 72;

            AssertStatCorrect(s => s.Stamina, expected, CharacterRace.Dwarf);
        }

        [Test]
        public void Gnome_Health()
        {
            int expected = 44019;

            AssertStatCorrect(s => s.Health, expected);
        }

        [Test]
        public void Gnome_Intellect()
        {
            int expected = 202;

            AssertStatCorrect(s => s.Intellect, expected, CharacterRace.Gnome);
        }

        [Test]
        public void Gnome_Mana()
        {
            int expected = 24507;

            AssertStatCorrect(s => s.Mana, expected, CharacterRace.Gnome);
        }

        [Test]
        public void Gnome_SpellCrit()
        {
            float expected = 0.0155f;

            AssertStatCorrect(s => s.SpellCrit, expected, CharacterRace.Gnome);
        }

        [Test]
        public void Gnome_SpellHaste()
        {
            float expected = 0.0f;

            AssertStatCorrect(s => s.SpellHaste, expected, CharacterRace.Gnome);
        }

        [Test]
        public void Gnome_SpellHit()
        {
            float expected = 0.0f;

            AssertStatCorrect(s => s.SpellHit, expected, CharacterRace.Gnome);
        }

        [Test]
        public void Gnome_SpellPower()
        {
            int expected = 192;

            AssertStatCorrect(s => s.SpellPower, expected, CharacterRace.Gnome);
        }

        [Test]
        public void Gnome_Spirit()
        {
            int expected = 198;

            AssertStatCorrect(s => s.Spirit, expected, CharacterRace.Gnome);
        }

        [Test]
        public void Gnome_Stamina()
        {
            int expected = 71;

            AssertStatCorrect(s => s.Stamina, expected, CharacterRace.Gnome);
        }

        [Test]
        public void Goblin_Health()
        {
            int expected = 44019;

            AssertStatCorrect(s => s.Health, expected, CharacterRace.Goblin);
        }

        [Test]
        public void Goblin_Intellect()
        {
            int expected = 202;

            AssertStatCorrect(s => s.Intellect, expected, CharacterRace.Goblin);
        }

        [Test]
        public void Goblin_Mana()
        {
            int expected = 23340;

            AssertStatCorrect(s => s.Mana, expected, CharacterRace.Goblin);
        }

        [Test]
        public void Goblin_SpellCrit()
        {
            float expected = 0.0155f;

            AssertStatCorrect(s => s.SpellCrit, expected, CharacterRace.Goblin);
        }

        [Test]
        public void Goblin_SpellHaste()
        {
            float expected = 0.01f;

            AssertStatCorrect(s => s.SpellHaste, expected, CharacterRace.Goblin);
        }

        [Test]
        public void Goblin_SpellHit()
        {
            float expected = 0.0f;

            AssertStatCorrect(s => s.SpellHit, expected, CharacterRace.Goblin);
        }

        [Test]
        public void Goblin_SpellPower()
        {
            int expected = 192;

            AssertStatCorrect(s => s.SpellPower, expected, CharacterRace.Goblin);
        }

        [Test]
        public void Goblin_Spirit()
        {
            int expected = 196;

            AssertStatCorrect(s => s.Spirit, expected, CharacterRace.Goblin);
        }

        [Test]
        public void Goblin_Stamina()
        {
            int expected = 71;

            AssertStatCorrect(s => s.Stamina, expected, CharacterRace.Goblin);
        }

        [Test]
        public void Human_Health()
        {
            int expected = 44019;

            AssertStatCorrect(s => s.Health, expected, CharacterRace.Human);
        }

        [Test]
        public void Human_Intellect()
        {
            int expected = 199;

            AssertStatCorrect(s => s.Intellect, expected, CharacterRace.Human);
        }

        [Test]
        public void Human_Mana()
        {
            int expected = 23295;

            AssertStatCorrect(s => s.Mana, expected, CharacterRace.Human);
        }

        [Test]
        public void Human_SpellCrit()
        {
            float expected = 0.0154f;

            AssertStatCorrect(s => s.SpellCrit, expected, CharacterRace.Human);
        }

        [Test]
        public void Human_SpellHaste()
        {
            float expected = 0.0f;

            AssertStatCorrect(s => s.SpellHaste, expected, CharacterRace.Human);
        }

        [Test]
        public void Human_SpellHit()
        {
            float expected = 0.0f;

            AssertStatCorrect(s => s.SpellHit, expected, CharacterRace.Human);
        }

        [Test]
        public void Human_SpellPower()
        {
            int expected = 189;

            AssertStatCorrect(s => s.SpellPower, expected, CharacterRace.Human);
        }

        [Test]
        public void Human_Spirit()
        {
            int expected = 203;

            AssertStatCorrect(s => s.Spirit, expected, CharacterRace.Human);
        }

        [Test]
        public void Human_Stamina()
        {
            int expected = 71;

            AssertStatCorrect(s => s.Stamina, expected, CharacterRace.Human);
        }

        [Test]
        public void NightElf_Health()
        {
            int expected = 44019;

            AssertStatCorrect(s => s.Health, expected, CharacterRace.NightElf);
        }

        [Test]
        public void NightElf_Intellect()
        {
            int expected = 199;

            AssertStatCorrect(s => s.Intellect, expected, CharacterRace.NightElf);
        }

        [Test]
        public void NightElf_Mana()
        {
            int expected = 23295;

            AssertStatCorrect(s => s.Mana, expected, CharacterRace.NightElf);
        }

        [Test]
        public void NightElf_SpellCrit()
        {
            float expected = 0.0154f;

            AssertStatCorrect(s => s.SpellCrit, expected, CharacterRace.NightElf);
        }

        [Test]
        public void NightElf_SpellHaste()
        {
            float expected = 0.0f;

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
            int expected = 189;

            AssertStatCorrect(s => s.SpellPower, expected, CharacterRace.NightElf);
        }

        [Test]
        public void NightElf_Spirit()
        {
            int expected = 198;

            AssertStatCorrect(s => s.Spirit, expected, CharacterRace.NightElf);
        }

        [Test]
        public void NightElf_Stamina()
        {
            int expected = 71;

            AssertStatCorrect(s => s.Stamina, expected, CharacterRace.NightElf);
        }

        [Test]
        public void Tauren_Health()
        {
            int expected = 46197;

            AssertStatCorrect(s => s.Health, expected, CharacterRace.Tauren);
        }

        [Test]
        public void Tauren_Intellect()
        {
            int expected = 195;

            AssertStatCorrect(s => s.Intellect, expected, CharacterRace.Tauren);
        }

        [Test]
        public void Tauren_Mana()
        {
            int expected = 23235;

            AssertStatCorrect(s => s.Mana, expected, CharacterRace.Tauren);
        }

        [Test]
        public void Tauren_SpellCrit()
        {
            float expected = 0.0154f;

            AssertStatCorrect(s => s.SpellCrit, expected, CharacterRace.Tauren);
        }

        [Test]
        public void Tauren_SpellHaste()
        {
            float expected = 0.00f;

            AssertStatCorrect(s => s.SpellHaste, expected, CharacterRace.Tauren);
        }

        [Test]
        public void Tauren_SpellHit()
        {
            float expected = 0.0f;

            AssertStatCorrect(s => s.SpellHit, expected, CharacterRace.Tauren);
        }

        [Test]
        public void Tauren_SpellPower()
        {
            int expected = 185;

            AssertStatCorrect(s => s.SpellPower, expected, CharacterRace.Tauren);
        }

        [Test]
        public void Tauren_Spirit()
        {
            int expected = 200;

            AssertStatCorrect(s => s.Spirit, expected, CharacterRace.Tauren);
        }

        [Test]
        public void Tauren_Stamina()
        {
            int expected = 72;

            AssertStatCorrect(s => s.Stamina, expected, CharacterRace.Tauren);
        }

        [Test]
        public void Troll_Health()
        {
            int expected = 44019;

            AssertStatCorrect(s => s.Health, expected, CharacterRace.Troll);
        }

        [Test]
        public void Troll_Intellect()
        {
            int expected = 195;

            AssertStatCorrect(s => s.Intellect, expected, CharacterRace.Troll);
        }

        [Test]
        public void Troll_Mana()
        {
            int expected = 23235;

            AssertStatCorrect(s => s.Mana, expected, CharacterRace.Troll);
        }

        [Test]
        public void Troll_SpellCrit()
        {
            float expected = 0.0154f;

            AssertStatCorrect(s => s.SpellCrit, expected, CharacterRace.Troll);
        }

        [Test]
        public void Troll_SpellHaste()
        {
            float expected = 0.0f;

            AssertStatCorrect(s => s.SpellHaste, expected, CharacterRace.Troll);
        }

        [Test]
        public void Troll_SpellHit()
        {
            float expected = 0.0f;

            AssertStatCorrect(s => s.SpellHit, expected, CharacterRace.Troll);
        }

        [Test]
        public void Troll_SpellPower()
        {
            int expected = 185;

            AssertStatCorrect(s => s.SpellPower, expected, CharacterRace.Troll);
        }

        [Test]
        public void Troll_Spirit()
        {
            int expected = 199;

            AssertStatCorrect(s => s.Spirit, expected, CharacterRace.Troll);
        }

        [Test]
        public void Troll_Stamina()
        {
            int expected = 71;

            AssertStatCorrect(s => s.Stamina, expected, CharacterRace.Troll);
        }


        [Test]
        public void Forsaken_Health()
        {
            int expected = 44019;

            AssertStatCorrect(s => s.Health, expected, CharacterRace.Undead);
        }

        [Test]
        public void Forsaken_Intellect()
        {
            int expected = 197;

            AssertStatCorrect(s => s.Intellect, expected, CharacterRace.Undead);
        }

        [Test]
        public void Forsaken_Mana()
        {
            int expected = 23265;

            AssertStatCorrect(s => s.Mana, expected, CharacterRace.Undead);
        }

        [Test]
        public void Forsaken_SpellCrit()
        {
            float expected = 0.0154f;

            AssertStatCorrect(s => s.SpellCrit, expected, CharacterRace.Undead);
        }

        [Test]
        public void Forsaken_SpellHaste()
        {
            float expected = 0.0f;

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
            int expected = 187;

            AssertStatCorrect(s => s.SpellPower, expected, CharacterRace.Undead);
        }

        [Test]
        public void Forsaken_Spirit()
        {
            int expected = 203;

            AssertStatCorrect(s => s.Spirit, expected, CharacterRace.Undead);
        }

        [Test]
        public void Forsaken_Stamina()
        {
            int expected = 71;

            AssertStatCorrect(s => s.Stamina, expected, CharacterRace.Undead);
        }

        [Test]
        public void Worgen_Health()
        {
            int expected = 44019;

            AssertStatCorrect(s => s.Health, expected, CharacterRace.Worgen);
        }

        [Test]
        public void Worgen_Intellect()
        {
            int expected = 195;

            AssertStatCorrect(s => s.Intellect, expected, CharacterRace.Worgen);
        }

        [Test]
        public void Worgen_Mana()
        {
            int expected = 23235;

            AssertStatCorrect(s => s.Mana, expected, CharacterRace.Worgen);
        }

        [Test]
        public void Worgen_SpellCrit()
        {
            float expected = 0.0254f;

            AssertStatCorrect(s => s.SpellCrit, expected, CharacterRace.Worgen);
        }

        [Test]
        public void Worgen_SpellHaste()
        {
            float expected = 0.0f;

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
            int expected = 185;

            AssertStatCorrect(s => s.SpellPower, expected, CharacterRace.Worgen);
        }

        [Test]
        public void Worgen_Spirit()
        {
            int expected = 197;

            AssertStatCorrect(s => s.Spirit, expected, CharacterRace.Worgen);
        }

        [Test]
        public void Worgen_Stamina()
        {
            int expected = 71;

            AssertStatCorrect(s => s.Stamina, expected, CharacterRace.Worgen);
        }
    }
}