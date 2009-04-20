using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Mage
{
    public abstract class SpellTemplate
    {
        public string Name;

        public bool AffectedByFlameCap;
        public bool ProvidesSnare;
        public bool ProvidesScorch;
        public bool AreaEffect;
        public bool Channeled;
        public float Ticks;

        public bool Instant;
        public int BaseCost;
        public float Range;
        public float BaseCastTime;
        public float BaseCooldown;
        public MagicSchool MagicSchool;
        public float BaseMinDamage;
        public float BaseMaxDamage;
        public float BasePeriodicDamage;
        public float SpellDamageCoefficient;
        public float DotDamageCoefficient;
        public float DotDuration;
        public float CastProcs;
        public float NukeProcs;
        public float HitRate;
        public float AoeDamageCap;

        public float RealResistance;
        public float PartialResistFactor;
        public float ThreatMultiplier;
        public float CritBonus;

        public float Cooldown;
        public float BaseCostModifier;
        public float BaseCostAmplifier;
        public float BaseInterruptProtection;
        public float BaseSpellModifier;
        public float BaseCritRate;
        public float BaseDirectDamageModifier;
        public float BaseDotDamageModifier;

        public static Dictionary<int, int> BaseMana = new Dictionary<int, int>();
        static SpellTemplate()
        {
            BaseMana[70] = 2241;
            BaseMana[71] = 2343;
            BaseMana[72] = 2446;
            BaseMana[73] = 2549;
            BaseMana[74] = 2652;
            BaseMana[75] = 2754;
            BaseMana[76] = 2857;
            BaseMana[77] = 2960;
            BaseMana[78] = 3063;
            BaseMana[79] = 3166;
            BaseMana[80] = 3268;
        }

        public virtual Spell GetSpell(CastingState castingState)
        {
            if (AreaEffect)
            {
                AoeSpell spell = new AoeSpell(this);
                spell.Calculate(castingState);
                spell.CalculateDerivedStats(castingState);
                return spell;
            }
            else
            {
                Spell spell = new Spell(this);
                spell.Calculate(castingState);
                spell.CalculateDerivedStats(castingState);
                return spell;
            }
        }

        public virtual float GetEffectAverageDamage(CastingState castingState)
        {
            Spell spell = new Spell(this);
            spell.Calculate(castingState);
            float damagePerSpellPower;
            return spell.CalculateAverageDamage(castingState.BaseStats, castingState.CalculationOptions, 0, false, false, out damagePerSpellPower);
        }

        protected SpellTemplate() { }
        public SpellTemplate(string name, bool channeled, bool instant, bool areaEffect, int range, float castTime, float cooldown, MagicSchool magicSchool, SpellData spellData) : this(name, channeled, instant, areaEffect, spellData.Cost, range, castTime, cooldown, magicSchool, spellData.MinDamage, spellData.MaxDamage, spellData.PeriodicDamage, 1, 1, spellData.SpellDamageCoefficient, spellData.DotDamageCoefficient, 0) { }
        public SpellTemplate(string name, bool channeled, bool instant, bool areaEffect, int range, float castTime, float cooldown, MagicSchool magicSchool, SpellData spellData, float hitProcs, float castProcs) : this(name, channeled, instant, areaEffect, spellData.Cost, range, castTime, cooldown, magicSchool, spellData.MinDamage, spellData.MaxDamage, spellData.PeriodicDamage, hitProcs, castProcs, spellData.SpellDamageCoefficient, spellData.DotDamageCoefficient, 0) { }
        public SpellTemplate(string name, bool channeled, bool instant, bool areaEffect, int range, float castTime, float cooldown, MagicSchool magicSchool, SpellData spellData, float hitProcs, float castProcs, float dotDuration) : this(name, channeled, instant, areaEffect, spellData.Cost, range, castTime, cooldown, magicSchool, spellData.MinDamage, spellData.MaxDamage, spellData.PeriodicDamage, hitProcs, castProcs, spellData.SpellDamageCoefficient, spellData.DotDamageCoefficient, dotDuration) { }
        public SpellTemplate(string name, bool channeled, bool instant, bool areaEffect, int cost, int range, float castTime, float cooldown, MagicSchool magicSchool, float minDamage, float maxDamage, float periodicDamage, float spellDamageCoefficient) : this(name, channeled, instant, areaEffect, cost, range, castTime, cooldown, magicSchool, minDamage, maxDamage, periodicDamage, 1, 1, spellDamageCoefficient, 0, 0) { }
        public SpellTemplate(string name, bool channeled, bool instant, bool areaEffect, int cost, int range, float castTime, float cooldown, MagicSchool magicSchool, float minDamage, float maxDamage, float periodicDamage) : this(name, channeled, instant, areaEffect, cost, range, castTime, cooldown, magicSchool, minDamage, maxDamage, periodicDamage, 1, 1, instant ? (1.5f / 3.5f) : (castTime / 3.5f), 0, 0) { }
        public SpellTemplate(string name, bool channeled, bool instant, bool areaEffect, int cost, int range, float castTime, float cooldown, MagicSchool magicSchool, float minDamage, float maxDamage, float periodicDamage, float hitProcs, float castProcs) : this(name, channeled, instant, areaEffect, cost, range, castTime, cooldown, magicSchool, minDamage, maxDamage, periodicDamage, hitProcs, castProcs, instant ? (1.5f / 3.5f) : (castTime / 3.5f), 0, 0) { }
        public SpellTemplate(string name, bool channeled, bool instant, bool areaEffect, int cost, int range, float castTime, float cooldown, MagicSchool magicSchool, float minDamage, float maxDamage, float periodicDamage, float hitProcs, float castProcs, float spellDamageCoefficient, float dotDamageCoefficient, float dotDuration)
        {
            Name = name;
            Channeled = channeled;
            Instant = instant;
            AreaEffect = areaEffect;
            BaseCost = cost;
            BaseCastTime = castTime;
            BaseCooldown = cooldown;
            Range = range;
            MagicSchool = magicSchool;
            BaseMinDamage = minDamage;
            BaseMaxDamage = maxDamage;
            BasePeriodicDamage = periodicDamage;
            SpellDamageCoefficient = spellDamageCoefficient;
            Ticks = hitProcs;
            CastProcs = castProcs;
            DotDamageCoefficient = dotDamageCoefficient;
            DotDuration = dotDuration;
        }

        public virtual void Calculate(CharacterCalculationsMage calculations)
        {
            Stats baseStats = calculations.BaseStats;
            MageTalents mageTalents = calculations.MageTalents;
            CalculationOptionsMage calculationOptions = calculations.CalculationOptions;

            Cooldown = BaseCooldown;
            BaseCost -= (int)calculations.BaseStats.SpellsManaReduction;

            BaseDirectDamageModifier = 1.0f;
            BaseDotDamageModifier = 1.0f;
            BaseCostModifier = 1.0f;
            BaseCostAmplifier = 1.0f;
            BaseCostAmplifier *= (1.0f - 0.01f * mageTalents.Precision);
            if (mageTalents.FrostChanneling > 0) BaseCostAmplifier *= (1.0f - 0.01f - 0.03f * mageTalents.FrostChanneling);
            if (MagicSchool == MagicSchool.Arcane) BaseCostAmplifier *= (1.0f - 0.01f * mageTalents.ArcaneFocus);
            if (MagicSchool == MagicSchool.Fire || MagicSchool == MagicSchool.FrostFire) AffectedByFlameCap = true;
            if (MagicSchool == MagicSchool.Fire || MagicSchool == MagicSchool.FrostFire) BaseInterruptProtection += 0.35f * mageTalents.BurningSoul;
            if (Range != 0)
            {
                if (MagicSchool == MagicSchool.Arcane) Range += mageTalents.MagicAttunement * 3;
                if (MagicSchool == MagicSchool.Fire) Range += mageTalents.FlameThrowing * 3;
                if (MagicSchool == MagicSchool.Frost) Range *= (1 + mageTalents.ArcticReach * 0.1f);
            }
            BaseInterruptProtection += baseStats.InterruptProtection;

            int playerLevel = calculationOptions.PlayerLevel;
            int targetLevel = AreaEffect ? calculationOptions.AoeTargetLevel : calculationOptions.TargetLevel;

            switch (MagicSchool)
            {
                case MagicSchool.Arcane:
                    BaseSpellModifier = calculations.BaseArcaneSpellModifier;
                    BaseCritRate = calculations.BaseArcaneCritRate;
                    CritBonus = calculations.BaseArcaneCritBonus;
                    HitRate = calculations.BaseArcaneHitRate;
                    ThreatMultiplier = calculations.ArcaneThreatMultiplier;
                    RealResistance = calculationOptions.ArcaneResist;
                    break;
                case MagicSchool.Fire:
                    BaseSpellModifier = calculations.BaseFireSpellModifier;
                    BaseCritRate = calculations.BaseFireCritRate;
                    CritBonus = calculations.BaseFireCritBonus;
                    HitRate = calculations.BaseFireHitRate;
                    ThreatMultiplier = calculations.FireThreatMultiplier;
                    RealResistance = calculationOptions.FireResist;
                    break;
                case MagicSchool.FrostFire:
                    BaseSpellModifier = calculations.BaseFrostFireSpellModifier;
                    BaseCritRate = calculations.BaseFrostFireCritRate;
                    CritBonus = calculations.BaseFrostFireCritBonus;
                    HitRate = calculations.BaseFrostFireHitRate;
                    ThreatMultiplier = calculations.FrostFireThreatMultiplier;
                    if (calculationOptions.FireResist == -1)
                    {
                        RealResistance = calculationOptions.FrostResist;
                    }
                    else if (calculationOptions.FrostResist == -1)
                    {
                        RealResistance = calculationOptions.FireResist;
                    }
                    else
                    {
                        RealResistance = Math.Min(calculationOptions.FireResist, calculationOptions.FrostResist);
                    }
                    break;
                case MagicSchool.Frost:
                    BaseSpellModifier = calculations.BaseFrostSpellModifier;
                    BaseCritRate = calculations.BaseFrostCritRate;
                    CritBonus = calculations.BaseFrostCritBonus;
                    HitRate = calculations.BaseFrostHitRate;
                    ThreatMultiplier = calculations.FrostThreatMultiplier;
                    RealResistance = calculationOptions.FrostResist;
                    break;
                case MagicSchool.Nature:
                    BaseSpellModifier = calculations.BaseNatureSpellModifier;
                    BaseCritRate = calculations.BaseNatureCritRate;
                    CritBonus = calculations.BaseNatureCritBonus;
                    HitRate = calculations.BaseNatureHitRate;
                    ThreatMultiplier = calculations.NatureThreatMultiplier;
                    RealResistance = calculationOptions.NatureResist;
                    break;
                case MagicSchool.Shadow:
                    BaseSpellModifier = calculations.BaseShadowSpellModifier;
                    BaseCritRate = calculations.BaseShadowCritRate;
                    CritBonus = calculations.BaseShadowCritBonus;
                    HitRate = calculations.BaseShadowHitRate;
                    ThreatMultiplier = calculations.ShadowThreatMultiplier;
                    RealResistance = calculationOptions.ShadowResist;
                    break;
                case MagicSchool.Holy:
                    BaseSpellModifier = calculations.BaseHolySpellModifier;
                    BaseCritRate = calculations.BaseHolyCritRate;
                    CritBonus = calculations.BaseHolyCritBonus;
                    HitRate = calculations.BaseHolyHitRate;
                    ThreatMultiplier = calculations.HolyThreatMultiplier;
                    RealResistance = calculationOptions.HolyResist;
                    break;
            }

            if (AreaEffect)
            {
                HitRate = ((targetLevel <= playerLevel + 2) ? (0.96f - (targetLevel - playerLevel) * 0.01f) : (0.94f - (targetLevel - playerLevel - 2) * 0.11f)) + calculations.BaseSpellHit;
                if (MagicSchool == MagicSchool.Arcane) HitRate += 0.01f * mageTalents.ArcaneFocus;
                if (HitRate > Spell.MaxHitRate) HitRate = Spell.MaxHitRate;
            }

            PartialResistFactor = (RealResistance == -1) ? 0 : (1 - StatConversion.GetAverageResistance(playerLevel, targetLevel, RealResistance, baseStats.SpellPenetration));
        }

        public static float ProcBuffUp(float procChance, float buffDuration, float triggerInterval)
        {
            if (triggerInterval <= 0)
                return 0;
            else
                return 1 - (float)Math.Pow(1 - procChance, buffDuration / triggerInterval);
        }

        public float CalculateCastTime(SpecialEffect[] hasteEffects, CalculationOptionsMage calculationOptions, float castingSpeed, float spellHasteRating, float interruptProtection, float critRate, bool pom, float baseCastTime, out float channelReduction)
        {
            // interrupt factors of more than once per spell are not supported, so put a limit on it (up to twice is probably approximately correct)
            float InterruptFactor = Math.Min(calculationOptions.InterruptFrequency, 2 * castingSpeed / baseCastTime);

            float levelScalingFactor = calculationOptions.LevelScalingFactor;
            if (pom) baseCastTime = 0.0f;

            float maxPushback = 0.5f * Math.Max(0, 1 - interruptProtection);
            if (Channeled) maxPushback = 0.0f;
            float globalCooldown = Math.Max(Spell.GlobalCooldownLimit, 1.5f / castingSpeed);
            float castTime = baseCastTime / castingSpeed + calculationOptions.Latency;
            castTime = castTime * (1 + InterruptFactor * maxPushback) - (maxPushback * 0.5f + calculationOptions.Latency) * maxPushback * InterruptFactor;
            if (castTime < globalCooldown + calculationOptions.Latency) castTime = globalCooldown + calculationOptions.Latency;
            channelReduction = 0.0f;

            foreach (SpecialEffect effect in hasteEffects)
            {
                float procs = 0.0f;
                switch (effect.Trigger)
                {
                    case Trigger.DamageSpellCast:
                    case Trigger.SpellCast:
                        procs = CastProcs;
                        break;
                    case Trigger.DamageSpellCrit:
                    case Trigger.SpellCrit:
                        procs = critRate * Ticks;
                        break;
                    case Trigger.DamageSpellHit:
                    case Trigger.SpellHit:
                        procs = HitRate * Ticks;
                        break;
                }
                if (procs == 0.0f) continue;
                if (effect.Cooldown >= effect.Duration)
                {
                    // hasted casttime
                    float speed = castingSpeed / (1 + spellHasteRating / 995f * levelScalingFactor) * (1 + (spellHasteRating + effect.Stats.HasteRating) / 995f * levelScalingFactor);
                    float gcd = Math.Max(Spell.GlobalCooldownLimit, 1.5f / speed);
                    float cast = baseCastTime / speed + calculationOptions.Latency;
                    cast = cast * (1 + InterruptFactor * maxPushback) - (maxPushback * 0.5f + calculationOptions.Latency) * maxPushback * InterruptFactor;
                    if (cast < gcd + calculationOptions.Latency) cast = gcd + calculationOptions.Latency;

                    castingSpeed /= (1 + spellHasteRating / 995f * levelScalingFactor);
                    float castsAffected = 0;
                    for (int c = 0; c < procs; c++) castsAffected += (float)Math.Ceiling((effect.Duration - c * castTime / procs) / cast) / procs;
                    spellHasteRating += effect.Stats.HasteRating * castsAffected * cast / (effect.Cooldown + castTime / procs / effect.Chance);
                    //Haste += castingState.BasicStats.SpellHasteFor6SecOnCast_15_45 * 6f / (45f + CastTime / CastProcs / 0.15f);
                    castingSpeed *= (1 + spellHasteRating / 995f * levelScalingFactor);

                    globalCooldown = Math.Max(Spell.GlobalCooldownLimit, 1.5f / castingSpeed);
                    castTime = baseCastTime / castingSpeed + calculationOptions.Latency;
                    castTime = castTime * (1 + InterruptFactor * maxPushback) - (maxPushback * 0.5f + calculationOptions.Latency) * maxPushback * InterruptFactor;
                    if (castTime < globalCooldown + calculationOptions.Latency) castTime = globalCooldown + calculationOptions.Latency;
                }
                else if (effect.Cooldown == 0 && (effect.Trigger == Trigger.SpellCrit || effect.Trigger == Trigger.DamageSpellCrit))
                {
                    float rawHaste = spellHasteRating;

                    castingSpeed /= (1 + spellHasteRating / 995f * levelScalingFactor);
                    float proccedSpeed = castingSpeed * (1 + (rawHaste + effect.Stats.HasteRating) / 995f * levelScalingFactor);
                    float proccedGcd = Math.Max(Spell.GlobalCooldownLimit, 1.5f / proccedSpeed);
                    float proccedCastTime = baseCastTime / proccedSpeed + calculationOptions.Latency;
                    proccedCastTime = proccedCastTime * (1 + InterruptFactor * maxPushback) - (maxPushback * 0.5f + calculationOptions.Latency) * maxPushback * InterruptFactor;
                    if (proccedCastTime < proccedGcd + calculationOptions.Latency) proccedCastTime = proccedGcd + calculationOptions.Latency;
                    int chancesToProc = (int)(((int)Math.Floor(effect.Duration / proccedCastTime) + 1) * Ticks);
                    if (!(Instant || pom)) chancesToProc -= 1;
                    if (AreaEffect) chancesToProc *= calculationOptions.AoeTargets;
                    spellHasteRating = rawHaste + effect.Stats.HasteRating * (1 - (float)Math.Pow(1 - effect.Chance * critRate, chancesToProc));
                    //Haste = rawHaste + castingState.BasicStats.SpellHasteFor5SecOnCrit_50 * ProcBuffUp(1 - (float)Math.Pow(1 - 0.5f * CritRate, HitProcs), 5, CastTime);
                    castingSpeed *= (1 + spellHasteRating / 995f * levelScalingFactor);
                    globalCooldown = Math.Max(Spell.GlobalCooldownLimit, 1.5f / castingSpeed);
                    castTime = baseCastTime / castingSpeed + calculationOptions.Latency;
                    castTime = castTime * (1 + InterruptFactor * maxPushback) - (maxPushback * 0.5f + calculationOptions.Latency) * maxPushback * InterruptFactor;
                    if (castTime < globalCooldown + calculationOptions.Latency) castTime = globalCooldown + calculationOptions.Latency;
                }
            }

            // channeled pushback
            if (Channeled && InterruptFactor > 0)
            {
                int maxLostTicks = (int)Math.Ceiling(Ticks * 0.25f * Math.Max(0, 1 - interruptProtection));
                // pushbacks that happen up to pushbackCastTime cut the cast time to pushbackCastTime
                // pushbacks that happen after just terminate the channel
                // [---|---X---|---|---]
                float tickFactor = 0;
                for (int i = 0; i < maxLostTicks; i++)
                {
                    tickFactor += InterruptFactor * castTime / Ticks * (i + 1) / Ticks;
                }
                tickFactor += InterruptFactor * (Ticks - maxLostTicks) * castTime / Ticks * maxLostTicks / Ticks;
                channelReduction = tickFactor;
            }

            return castTime;
        }
    }
}
