using System;

namespace Rawr.ShadowPriest.Spells
{
    public class MindSear : Spell
    {
        /// <summary>
        /// Mind Sear is a Cahnnelled AOE.
        /// It Benifits from:
        /// Talents:
        /// Twin Disciplines, Darkness (to be handeled in Char stats), Shadowform, Twisted Faith
        //TODO: ALL
        /// </summary>
        public MindSear() : base()
        { 
        }
        protected override void SetBaseValues()
        {
            base.SetBaseValues();

            baseMinDamage = 91;
            baseMaxDamage = 97;
            manaCost = 0.28f * Constants.BaseMana;
            shortName = "Sear";
        }
    }
}
