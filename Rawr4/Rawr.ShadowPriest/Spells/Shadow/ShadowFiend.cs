using System;

namespace Rawr.ShadowPriest.Spells
{
    public class ShadowFiend : Spell
    {
        protected override void SetBaseValues()
        {
            base.SetBaseValues();

            shortName = "SF";
            name = "Shadow Fiend";
            cooldown = 180f;
        }
    }
}
