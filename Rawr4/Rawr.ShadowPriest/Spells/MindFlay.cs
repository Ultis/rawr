using System;

namespace Rawr.ShadowPriest.Spells
{
    public class MindFlay : Spell
    {
        protected override void SetBaseValues()
        {
            base.SetBaseValues();

            baseCastTime = 3f;
            manaCost = 0.22f * Constants.BaseMana;
            shortName = "MF";
        }
    }
}
