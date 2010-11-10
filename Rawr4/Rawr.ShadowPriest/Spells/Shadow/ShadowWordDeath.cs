using System;

namespace Rawr.ShadowPriest.Spells
{
    public class ShadowWordDeath : DD
    {
        protected override void SetBaseValues()
        {
            base.SetBaseValues();

            manaCost = 0.12f * Constants.BaseMana;
            cooldown = 10f;
            shortName = "SW:D";
            name = "Shadow Word: Death";
        }
    }
}
