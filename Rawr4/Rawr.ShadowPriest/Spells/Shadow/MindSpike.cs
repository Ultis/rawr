using System;

namespace Rawr.ShadowPriest.Spells
{
    public class MindSpike : Spell
    {
        protected override void SetBaseValues()
        {
            base.SetBaseValues();

            baseMinDamage = 1083;
            baseMaxDamage = 1143;
            baseCastTime = 1.5f;
            manaCost = 0.12f * Constants.BaseMana;
            shortName = "Spike";
            name = "Mind Spike";
        }
    }
}
