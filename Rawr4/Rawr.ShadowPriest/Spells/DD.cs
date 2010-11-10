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

        /// <summary>
        /// Average Damage
        /// </summary>
        public float AverageDamage
        { get { return dd; } }

    }
}
