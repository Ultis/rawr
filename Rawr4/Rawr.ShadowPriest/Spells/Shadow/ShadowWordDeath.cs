using System;

namespace Rawr.ShadowPriest.Spells
{
    public class ShadowWordDeath : Spell
    {
        protected override void SetBaseValues()
        {
            base.SetBaseValues();

            manaCost = 0.12f * Constants.BaseMana;
            cooldown = 10f;
            shortName = "SW:D";
        }
    }
}
