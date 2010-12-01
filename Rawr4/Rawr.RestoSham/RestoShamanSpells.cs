using System;

namespace Rawr.RestoSham
{
    /// <summary>
    /// Base class that implements the least common denominator of functionality common to all relevant spells.
    /// </summary>
    public abstract class Spell
    {
        #region Fields
        public const float HealingPowerMultiplier = 1.88f;
        #endregion

        // Base stats
        public int SpellId { get; set; }
        public string SpellName { get; set; }
        public int BaseManaCost { get; set; }
        public float BaseCoefficient { get; set; }
        public float Cooldown { get; set; }
        public float BaseEffect { get; set; }
        public float BaseCastTime { get; set; }
        public bool Instant { get; set; }
        public bool HasCooldown { get; set; }
        public bool CanCrit { get; set; }

        // Network
        public float Latency { get; set; }
        public float GcdLatency { get; set; }

        // Modifiers
        public float CooldownReduction { get; set; }
        public float CostScale { get; set; }
        public float EffectModifier { get; set; }
        public float BonusSpellPower { get; set; }
        public float CastTimeReduction { get; set; }
        public float CritRateModifier { get; set; }

        // Scaling
        public float CriticalScale { get; set; }
        public float HasteScale { get; set; }

        // Incremental stats
        public float SpellPower { get; set; }
        public float CritRate { get; set; }

        // Derived stats
        public float EffectiveCrit { get { return CritRate + CritRateModifier; } }
        public float EffectiveCritModifier { get { return (Math.Min(CriticalScale - 1, 1f) * EffectiveCrit) + 1; } }

        // Calculation Results
        public float CastTime { get; protected set; }
        public int ManaCost { get; protected set; }
        public float Effect { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Spell"/> class.
        /// </summary>
        public Spell()
        {
            Instant = false;
            HasCooldown = false;
            CooldownReduction = 0f;
            CostScale = 0f;
            EffectModifier = 0f;
            BonusSpellPower = 0f;
            CastTimeReduction = 0f;
            CriticalScale = 1.5f;
            HasteScale = 0f;
            Latency = 0f;
            GcdLatency = 0f;
            CastTime = 0;
            CritRate = 0f;
            CritRateModifier = 0f;
            SpellPower = 0f;
            CanCrit = true;
        }

        public void Calculate()
        {
            ManaCost = (int)Math.Round(BaseManaCost * CostScale, 0);
            CalculateCastTime();
            CalculateEffect();
        }

        protected void CalculateCastTime()
        {
            if (Instant)
                CastTime = Math.Max(1.5f * HasteScale + Latency, 1f + GcdLatency);
            else
                CastTime = Math.Max((BaseCastTime - CastTimeReduction) * HasteScale + Latency, 1f + GcdLatency);
        }

        protected virtual void CalculateEffect()
        {
            Effect = 0f;
            if (BaseEffect <= 1f)
                return;

            float spellPower = GetEffectiveSpellPower(BaseCoefficient);
            float nonCrit = (BaseEffect + spellPower) * EffectModifier;
            if (!CanCrit)
            {
                Effect = nonCrit;
                return;
            }
            Effect = EffectiveCritModifier * nonCrit;
        }

        protected virtual float GetEffectiveSpellPower(float coeff)
        {
            return (SpellPower + BonusSpellPower) * coeff;
        }

        public virtual float EPS { get { return Effect / CastTime; } }
        public virtual float MPS { get { return ManaCost / CastTime; } }
    }

    public class HealingSpell : Spell
    {
        protected override float GetEffectiveSpellPower(float coeff)
        {
            return (SpellPower + BonusSpellPower) * coeff * Spell.HealingPowerMultiplier;
        }
    }
    public class Hot : HealingSpell
    {
        // Base stats
        public float HotDuration { get; set; }
        public float BaseHotTickFrequency { get; set; }
        public float BaseHotCoefficient { get; set; }
        public float BaseHotEffect { get; set; }
        protected int BaseHotTickCount { get; set; }

        // Calculation results
        public float TotalHotEffect { get; private set; }

        // Derived stats
        public float TickEffect
        {
            get
            {
                return TotalHotEffect / BaseHotTickCount;
            }
        }

        public override float EPS
        {
            get
            {
                if (Instant)
                    return base.EPS + (TotalHotEffect / HotDuration);
                else
                    return base.EPS + (TotalHotEffect / (HotDuration + CastTime));
            }
        }
        public override float MPS
        {
            get
            {
                if (Instant)
                    return (ManaCost / (HotDuration));
                else
                    return (ManaCost / (HotDuration + CastTime));
            }
        }

        protected override void CalculateEffect()
        {
            base.CalculateEffect();

            float spellPower = GetEffectiveSpellPower(BaseCoefficient);
            float nonCrit = (BaseHotEffect + spellPower) * EffectModifier;
            if (!CanCrit)
            {
                Effect = nonCrit;
                return;
            }
            TotalHotEffect = EffectiveCritModifier * nonCrit;
        }
    }
    
    public sealed class HealingRain : Hot
    {
        public HealingRain()
        {
            SpellName = "Healing Rain";
            Cooldown = 10f;
            BaseCastTime = 2f;
            BaseHotCoefficient = 0.5f;
            BaseHotTickFrequency = 2f;
            BaseEffect = 0f;
            BaseHotEffect = 3775f;
            BaseHotTickCount = 5;
            HotDuration = 10f;
            HasCooldown = true;
        }
    }
    public sealed class Riptide : Hot
    {
        public Riptide()
        {
            SpellName = "Riptide";
            Cooldown = 6f;
            BaseHotTickFrequency = 3f;
            BaseCoefficient = 1.5f / 3.5f;
            BaseHotCoefficient = 0.5f;
            BaseEffect = 2363f;
            BaseHotEffect = 3725f;
            BaseHotTickCount = 5;
            Instant = true;
            HasCooldown = true;
        }
    }
    public sealed class ChainHeal : HealingSpell
    {
        public bool ChainedHeal { get; set; }

        public ChainHeal()
        {
            SpellName = "ChainHeal";
            Cooldown = 0f;
            BaseCoefficient = 2.5f / 3.5f;
            BaseCastTime = 2.5f;
            BaseEffect = 3178f;
        }
    }
    public sealed class EarthShield : Hot
    {
        private const int _BaseChargeHeal = 1770;

        public int Charges
        {
            get { return BaseHotTickCount; }
            set
            {
                BaseHotTickCount = value;
                HotDuration = BaseHotTickFrequency * BaseHotTickCount;
                BaseHotEffect = _BaseChargeHeal * BaseHotTickCount;
            }
        }

        public EarthShield()
        {
            SpellId = 974;
            SpellName = "Earth Shield";
            BaseHotTickFrequency = 4f;              // Start modeling at a charge procing every 4s
            BaseHotCoefficient = 1f / 3.5f;
            Charges = 9;
            Instant = true;
        }
    }
}
