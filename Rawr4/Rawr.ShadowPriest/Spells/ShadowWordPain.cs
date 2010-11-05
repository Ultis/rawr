using System;


namespace Rawr.ShadowPriest.Spells
{
    public class ShadowWordPain :Spell
    {
        protected override void SetBaseValues()
        {
            base.SetBaseValues();

            manaCost = 0.22f * Constants.BaseMana;
            shortName = "SW:P";
        }

    }
}
