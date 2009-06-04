namespace Rawr.Rogue.ClassAbilities
{
    public class TalentsAndGlyphs
    {
        public static void Initialize( RogueTalents talents, CalculationOptionsRogue calcOpts )
        {
            Glyphs.Initialize(talents);
            Talents.Initialize(talents, calcOpts);
        }
    }
}