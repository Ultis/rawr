using System;

namespace Rawr.ShadowPriest.Spells
{
    public class DevouringPlauge : Spell
    {
        protected override void SetBaseValues()
        {
            base.SetBaseValues();

            manaCost = 0.25f * Constants.BaseMana;
            shortName = "DP";
        }
    }
}
