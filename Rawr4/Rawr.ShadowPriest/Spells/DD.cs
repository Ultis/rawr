using System;

namespace Rawr.ShadowPriest.Spells
{
    /// <summary>
    /// Direct Damge Spell
    /// </summary>
    public class DD : Spell
    {
        protected float baseSpread = 0f;

        protected virtual void SetDDValues()
        {
            baseSpread = 0f;
        }

        public float Spread
        { get { return baseSpread /2 * BaseDamage ; } }

        public float MinDamage
        { get { return BaseDamage + spellPower * SpellPowerCoef - Spread; ; } }
        public float MaxDamage
        { get { return BaseDamage + spellPower * SpellPowerCoef + Spread; } }

        public float MinCritDamage
        { get { return MinDamage * critModifier; } }

        public float MaxCritDamage
        { get { return MaxDamage * critModifier; } }

        public override float AverageDamage
        { get { return (1f - CritChance) * ((MinDamage + MaxDamage) / 2) + CritChance * ((MinCritDamage + MaxCritDamage) / 2); } }

        /// <summary>
        /// General rule for SP co-ef for dots is duration / 15
        /// </summary>
        public override float SpellPowerCoef
        { get { return castTimeBase / 3.5f; } }

    }
}
