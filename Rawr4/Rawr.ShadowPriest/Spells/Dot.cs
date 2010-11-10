using System;

namespace Rawr.ShadowPriest.Spells
{
    public class Dot : Spell
    {
        protected float tickNumber;
        protected float tickPeriod;
        protected float debuffDuration;

        public Dot()
        {
            SetDotValues();
        }
        protected virtual void SetDotValues()
        {
            tickNumber = 2f;
            tickPeriod = 1f;
            debuffDuration = 5f;
        }

        /// <summary>
        /// Time Between ticks after haste
        /// </summary>
        public float TickPeriod
        { get { return tickPeriod; } }

        /// <summary>
        /// Time until experation of dot
        /// </summary>
        public float DebuffDuration
        { get { return debuffDuration; } }
    }
}
