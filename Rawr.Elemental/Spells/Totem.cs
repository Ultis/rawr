using System;

namespace Rawr.Elemental.Spells
{
    public class Totem : Spell
    {
        public Totem() : base()
        {
        }

        protected override void SetBaseValues()
        {
            base.SetBaseValues();
            shortName = "Totem";
            castTime = 0f;
            gcd = 1f;
        }

        public override float CCCritChance
        {
            get
            {
                return 0f;
            }
        }
    }
}