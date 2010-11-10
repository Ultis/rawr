using System;

namespace Rawr.ShadowPriest.Spells
{
    /// <summary>
    /// Direct Damge Spell
    /// </summary>
    public class DD : Spell
    {
        protected float dd = 0f;

        public DD()
        {
            SetDDValues();
        }
        protected virtual void SetDDValues()
        {
            
        }
        public override float AverageDamage
        { get { return averageDamage; } }
    }
}
