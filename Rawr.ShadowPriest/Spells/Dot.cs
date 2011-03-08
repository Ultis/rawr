using System;

namespace Rawr.ShadowPriest.Spells
{
    /// <summary>
    /// Dot Spell
    /// </summary>
    public class DoTSpell : Spell
    {
        /// <summary>
        /// Coef for dot ticks
        /// </summary>
        protected float tickHasteCoEf = 0f;
        /// <summary>
        /// How long the dot stays on the target
        /// </summary>
        protected float debuffDuration = 0f;
        /// <summary>
        /// How long the dot stays on the target before haste
        /// </summary>
        protected float debuffDurationBase = 0f;
        /// <summary>
        /// Time Between ticks after haste
        /// </summary>
        protected float tickPeriod = 3f;
        /// <summary>
        /// Time Between ticks
        /// </summary>
        protected float tickPeriodBase = 3f;
        /// <summary>
        /// Extra ticks from haste
        /// </summary>
        protected float tickExtra =0f;


        public DoTSpell()
        {
            SetDotValues();
        }

        protected virtual void SetDotValues()
        {
            tickHasteCoEf = 0f;
            debuffDurationBase = 0f;
            tickPeriodBase = 3f;
            tickExtra = 0f;
        }

        /// <summary>
        /// Time per DoT tick
        /// </summary>
        public float TickPeriod
        { get { return tickPeriod; } }

        /// <summary>
        /// Number of ticks with 0 haste
        /// </summary>
        public float TickNumberBase
        { get { return debuffDurationBase / tickPeriodBase; } }

        /// <summary>
        /// How long the dot stays on the target
        /// </summary>
        public float DebuffDuration
        { 
            get 
            {
                float addedTicks = (float)Math.Round(tickExtra, 0);
                return (debuffDurationBase) + (addedTicks * tickPeriod) ;
            }
        }

        /// <summary>
        /// Damage per Tick
        /// </summary>
        public float TickDamage
        { get { return (BaseDamage + SpellPowerCoef * spellPower) / TickNumber; } }

        /// <summary>
        /// CritDamage per Tick
        /// </summary>
        public float TickCritDamage
        { get { return TickDamage * critModifier; } }

        /// <summary>
        /// Number of ticks after haste
        /// </summary>
        public float TickNumber
        { get { return TickNumberBase + (float)Math.Round(tickExtra,0); } }

        /// <summary>
        /// General rule for SP co-ef for dots is duration / 15
        /// </summary>
        public override float SpellPowerCoef
        { get { return debuffDurationBase / 15f; } }

        /// <summary>
        /// Average Damage for spell
        /// </summary>
        public override float AverageDamage
        { get { return ((1f - CritChance) * TickDamage + CritChance * TickCritDamage) * TickNumber; } }

        public override void Initialize(ISpellArgs args)
        {
            base.Initialize(args);

            float hasteRating = StatConversion.GetHasteFromRating(args.Stats.HasteRating, CharacterClass.Priest);

            if (tickHasteCoEf != 0f)
                tickExtra = hasteRating / tickHasteCoEf;

            float remander = (float)Math.IEEERemainder(tickExtra, 1);
            
            tickPeriod = tickPeriodBase / (1 + hasteRating);
            debuffDuration = debuffDurationBase / (1 + hasteRating);
        }

    }
}
