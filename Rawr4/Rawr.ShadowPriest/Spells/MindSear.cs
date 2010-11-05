using System;

namespace Rawr.ShadowPriest.Spells
{
    public class MindSear : Spell
    {
        protected override void SetBaseValues()
        {
            base.SetBaseValues();

            baseMinDamage = 91;
            baseMaxDamage = 97;
            manaCost = 0.28f * Constants.BaseMana;
            shortName = "Sear";
        }
    }
}
