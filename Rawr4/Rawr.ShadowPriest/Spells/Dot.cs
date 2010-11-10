using System;

namespace Rawr.ShadowPriest.Spells
{
    /// <summary>
    /// Dot Spell
    /// </summary>
    public class Dot : Spell
    {
        /// <summary>
        /// Coef for dot ticks
        /// </summary>
        protected float tickHasteCoEf;
        /// <summary>
        /// Numbers of ticks spell has with 0 haste
        /// </summary>
        protected float tickNumberBase;
        /// <summary>
        /// Number of ticks after haste
        /// </summary>
        protected float tickNumber;
        /// <summary>
        /// Time between ticks
        /// </summary>
        protected float tickPeriod;
        /// <summary>
        /// How long the dot stays on the target before haste
        /// </summary>
        protected float debuffDurationBase;
        /// <summary>
        /// How long the dot stays on the target after haste
        /// </summary>
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
            averageDamage = 5000f;
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

        public override float AverageDamage
        { get { return averageDamage; } }

        public override void Initialize(ISpellArgs args)
        {
            base.Initialize(args);
            tickNumber += tickNumberBase + (float)Math.Round(StatConversion.GetHasteFromRating(args.Stats.HasteRating, CharacterClass.Priest) / tickHasteCoEf);
            
        }

    }
}
