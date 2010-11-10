using System;

namespace Rawr.ShadowPriest.Spells
{
    public class MindSpike : DD
    {
        protected override void SetBaseValues()
        {
            base.SetBaseValues();

            manaCost = 0.12f * Constants.BaseMana;
            shortName = "Spike";
            name = "Mind Spike";
        }
    }
}
