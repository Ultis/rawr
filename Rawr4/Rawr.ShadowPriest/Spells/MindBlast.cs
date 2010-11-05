using System;

namespace Rawr.ShadowPriest.Spells
{
    public class MindBlast : Spell
    {
        protected override void SetBaseValues()
        {
            base.SetBaseValues();

            baseMinDamage = 1028;
            baseMaxDamage = 1086;
            baseCastTime = 1.5f;
            manaCost = 0.17f * Constants.BaseMana;
            shortName = "MB";
        }

    }
}
