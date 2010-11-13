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
        /// How long the dot stays on the target before haste
        /// </summary>
        protected float debuffDurationBase = 0f;
        /// <summary>
        /// Time Between ticks after haste
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
        { get { return tickPeriodBase; } } //-time lost from haste and reset if has extra dot!

        /// <summary>
        /// Number of ticks with 0 haste
        /// </summary>
        public float TickNumberBase
        { get { return debuffDurationBase / TickPeriod; } }

        /// <summary>
        /// How long the dot stays on the target after haste
        /// </summary>
        public float DebuffDuration
        { get { return debuffDurationBase; } } //-time lost from haste and reset if has extra dot!

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
        { get { return TickNumberBase + tickExtra; } }

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

            tickExtra = (float)Math.Round(StatConversion.GetHasteFromRating(args.Stats.HasteRating, CharacterClass.Priest) / tickHasteCoEf,0);
            
        }

    }
}
