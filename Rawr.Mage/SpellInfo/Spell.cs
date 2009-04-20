using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace Rawr.Mage
{
    public enum MagicSchool
    {
        Holy = 1,
        Fire = 2,
        Nature,
        Frost,
        Shadow,
        Arcane,
        FrostFire
    }

    public struct SpellData
    {
        public float MinDamage;
        public float MaxDamage;
        public float PeriodicDamage;
        public int Cost;
        public float SpellDamageCoefficient;
        public float DotDamageCoefficient;
    }

    public class SpellContribution : IComparable<SpellContribution>
    {
        public string Name;
        public float Hits;
        public float Damage;
        public float Range;

        public int CompareTo(SpellContribution other)
        {
            return other.Damage.CompareTo(this.Damage);
        }
    }

    public class AoeSpell : Spell
    {
        public AoeSpell(SpellTemplate template) : base(template) { }

        public override void Calculate(CastingState castingState)
        {
            base.Calculate(castingState);
            // do not count debuffs for aoe effects, can't assume it will be up on all
            // do not include molten fury (molten fury relates to boss), instead amplify all by average
            if (castingState.MoltenFury)
            {
                SpellModifier /= (1 + 0.06f * castingState.MageTalents.MoltenFury);
            }
            if (castingState.MageTalents.MoltenFury > 0)
            {
                SpellModifier *= (1 + 0.06f * castingState.MageTalents.MoltenFury * castingState.CalculationOptions.MoltenFuryPercentage);
            }
        }

        public override void CalculateDerivedStats(CastingState castingState, bool outOfFiveSecondRule, bool pom, bool spammedDot, bool round, bool forceHit, bool forceMiss)
        {
            base.CalculateDerivedStats(castingState, outOfFiveSecondRule, pom, spammedDot, round, forceHit, forceMiss);
            TargetProcs *= castingState.CalculationOptions.AoeTargets;
        }

        public override float CalculateAverageDamage(Stats baseStats, CalculationOptionsMage calculationOptions, float spellPower, bool spammedDot, bool forceHit, out float damagePerSpellPower)
        {
            damagePerSpellPower = 0; // do we really need this for aoe?
            float baseAverage = (BaseMinDamage + BaseMaxDamage) / 2f + spellPower * SpellDamageCoefficient;
            float critMultiplier = 1 + (CritBonus - 1) * Math.Max(0, CritRate/* - castingState.ResilienceCritRateReduction*/);
            float resistMultiplier = (forceHit ? 1.0f : HitRate) * PartialResistFactor;
            int targets = calculationOptions.AoeTargets;
            float averageDamage = baseAverage * SpellModifier * DirectDamageModifier * targets * (forceHit ? 1.0f : HitRate);
            if (averageDamage > AoeDamageCap) averageDamage = AoeDamageCap;
            averageDamage = averageDamage * critMultiplier * PartialResistFactor;
            if (BasePeriodicDamage > 0.0f)
            {
                if (spammedDot)
                {
                    averageDamage += targets * (BasePeriodicDamage + DotDamageCoefficient * spellPower) * SpellModifier * DotDamageModifier * resistMultiplier * CastTime / DotDuration;
                }
                else
                {
                    averageDamage += targets * (BasePeriodicDamage + DotDamageCoefficient * spellPower) * SpellModifier * DotDamageModifier * resistMultiplier;
                }
            }
            return averageDamage;
        }
    }

    public class Spell
    {
        public SpellId SpellId;
        private SpellTemplate template;

        public SpellTemplate SpellTemplate { get { return template; } }

        public string Name { get { return template.Name; } }

        public float DamagePerSecond;
        public float ThreatPerSecond;
        public float CostPerSecond;

        public bool AffectedByFlameCap { get { return template.AffectedByFlameCap; } }
        public bool ProvidesSnare { get { return template.ProvidesSnare; } }
        public bool ProvidesScorch { get { return template.ProvidesScorch; } }
        public bool AreaEffect { get { return template.AreaEffect; } }

        public bool Channeled { get { return template.Channeled; } }
        public float Ticks { get { return template.Ticks; } }
        public float CastProcs { get { return template.CastProcs; } }
        public float NukeProcs { get { return template.NukeProcs; } }

        public bool SpammedDot { get; set; }

        public float HitProcs;
        public float CritProcs;
        public float TargetProcs;

        public float CastTime;

        public bool Instant { get { return template.Instant; } }
        public int BaseCost { get { return template.BaseCost; } }
        public float BaseCastTime;
        public float BaseCooldown { get { return template.BaseCooldown; } }
        public MagicSchool MagicSchool { get { return template.MagicSchool; } }
        public float BaseMinDamage { get { return template.BaseMinDamage; } }
        public float BaseMaxDamage { get { return template.BaseMaxDamage; } }
        public float BasePeriodicDamage { get { return template.BasePeriodicDamage; } }
        public float SpellDamageCoefficient { get { return template.SpellDamageCoefficient; } }
        public float DotDamageCoefficient { get { return template.DotDamageCoefficient; } }
        public float DotDuration { get { return template.DotDuration; } }
        public float AoeDamageCap { get { return template.AoeDamageCap; } }
        public float Range { get { return template.Range; } }

        public float MinHitDamage
        {
            get
            {
                return (BaseMinDamage + RawSpellDamage * SpellDamageCoefficient) * SpellModifier * DirectDamageModifier;
            }
        }

        public float MaxHitDamage
        {
            get
            {
                return (BaseMaxDamage + RawSpellDamage * SpellDamageCoefficient) * SpellModifier * DirectDamageModifier;
            }
        }

        public float MinCritDamage
        {
            get
            {
                return MinHitDamage * CritBonus;
            }
        }

        public float MaxCritDamage
        {
            get
            {
                return MaxHitDamage * CritBonus;
            }
        }

        public float DotDamage
        {
            get
            {
                return (BasePeriodicDamage + DotDamageCoefficient * RawSpellDamage) * SpellModifier * DotDamageModifier;
            }
        }

        public const float GlobalCooldownLimit = 1.0f;
        public const float MaxHitRate = 1.0f;

        private class SpellCycle : Cycle
        {
            private Spell spell;

            public SpellCycle(Spell spell) : base(spell.castingState)
            {
                this.spell = spell;
                Name = spell.Name;
                sequence = spell.Name;
                Ticks = spell.Ticks;
                CastTime = spell.CastTime;
                HitProcs = spell.HitProcs;
                CastProcs = spell.CastProcs;
                NukeProcs = spell.NukeProcs;
                CritProcs = spell.CritProcs;
                TargetProcs = spell.TargetProcs;
                damagePerSecond = spell.DamagePerSecond;
                threatPerSecond = spell.ThreatPerSecond;
                costPerSecond = spell.CostPerSecond;
                AffectedByFlameCap = spell.AffectedByFlameCap;
                OO5SR = spell.OO5SR;
                AreaEffect = spell.AreaEffect;
                DpsPerSpellPower = spell.DpsPerSpellPower;
                if (AreaEffect) AoeSpell = spell;
            }

            public override void AddSpellContribution(Dictionary<string, SpellContribution> dict, float duration)
            {
                spell.AddSpellContribution(dict, spell.CastTime * duration / CastTime);
            }

            public override void AddManaUsageContribution(Dictionary<string, float> dict, float duration)
            {
                spell.AddManaUsageContribution(dict, spell.CastTime * duration / CastTime);
            }
        }

        private Cycle cycle;
        public static implicit operator Cycle(Spell spell)
        {
            if (spell.cycle == null)
            {
                spell.cycle = new SpellCycle(spell);
            }
            return spell.cycle;
        }

        public Spell(SpellTemplate template)
        {
            this.template = template;
        }

        public float CostModifier;
        public float CostAmplifier;
        public float SpellModifier;
        public float DirectDamageModifier;
        public float DotDamageModifier;
        public float RealResistance { get { return template.RealResistance; } }
        public float CritRate;
        public float ThreatMultiplier { get { return template.ThreatMultiplier; } }
        public float CritBonus { get { return template.CritBonus; } }
        public float HitRate { get { return template.HitRate; } }
        public float PartialResistFactor { get { return template.PartialResistFactor; } }
        public float RawSpellDamage;
        public float AverageDamage;
        public float DpsPerSpellPower;

        public float InterruptProtection;

        public float Cooldown { get { return template.Cooldown; } }

        public float Cost
        {
            get
            {
                return (float)Math.Floor(BaseCost * CostAmplifier * CostModifier);
            }
        }

        public float ABCost
        {
            get
            {
                return (float)Math.Floor(Math.Round(BaseCost * CostAmplifier) * CostModifier);
            }
        }

        private CastingState castingState;

        public float OO5SR = 0;

        public virtual void Calculate(CastingState castingState)
        {
            this.castingState = castingState;

            BaseCastTime = template.BaseCastTime;
            CostModifier = template.BaseCostModifier;
            CostAmplifier = template.BaseCostAmplifier;
            DirectDamageModifier = template.BaseDirectDamageModifier;
            DotDamageModifier = template.BaseDotDamageModifier;
            if (castingState.PowerInfusion) CostModifier -= 0.2f; // don't have any information on this, going by best guess
            if (castingState.ArcanePower) CostModifier += 0.2f;
            InterruptProtection = template.BaseInterruptProtection;

            SpellModifier = template.BaseSpellModifier * castingState.StateSpellModifier;
            CritRate = template.BaseCritRate + castingState.StateCritRate;
            if (castingState.Combustion && (MagicSchool == MagicSchool.Fire || MagicSchool == MagicSchool.FrostFire))
            {
                CritRate = 3 / castingState.CombustionDuration;
            }

            switch (MagicSchool)
            {
                case MagicSchool.Arcane:
                    RawSpellDamage = castingState.ArcaneSpellPower;
                    break;
                case MagicSchool.Fire:
                    RawSpellDamage = castingState.FireSpellPower;
                    break;
                case MagicSchool.FrostFire:
                    RawSpellDamage = castingState.FrostFireSpellPower;
                    break;
                case MagicSchool.Frost:
                    RawSpellDamage = castingState.FrostSpellPower;
                    break;
                case MagicSchool.Nature:
                    RawSpellDamage = castingState.NatureSpellPower;
                    break;
                case MagicSchool.Shadow:
                    RawSpellDamage = castingState.ShadowSpellPower;
                    break;
                case MagicSchool.Holy:
                    RawSpellDamage = castingState.HolySpellPower;
                    break;
            }
        }

        public void CalculateManualClearcasting(bool manualClearcasting, bool clearcastingAveraged, bool clearcastingActive)
        {
            if (manualClearcasting && !clearcastingAveraged)
            {
                CritRate -= 0.15f * 0.02f * castingState.MageTalents.ArcaneConcentration * castingState.MageTalents.ArcanePotency; // replace averaged arcane potency with actual % chance
                if (clearcastingActive) CritRate += 0.15f * castingState.MageTalents.ArcanePotency;
            }
        }

        public static float ProcBuffUp(float procChance, float buffDuration, float triggerInterval)
        {
            if (triggerInterval <= 0)
                return 0;
            else
                return 1 - (float)Math.Pow(1 - procChance, buffDuration / triggerInterval);
        }

        public void CalculateDerivedStats(CastingState castingState)
        {
            CalculateDerivedStats(castingState, false, false, true, false, false, false);
        }

        public void CalculateDerivedStats(CastingState castingState, bool outOfFiveSecondRule, bool pom, bool spammedDot)
        {
            CalculateDerivedStats(castingState, outOfFiveSecondRule, pom, spammedDot, false, false, false);
        }

        public virtual void CalculateDerivedStats(CastingState castingState, bool outOfFiveSecondRule, bool pom, bool spammedDot, bool round, bool forceHit, bool forceMiss)
        {
            MageTalents mageTalents = castingState.MageTalents;
            Stats baseStats = castingState.BaseStats;
            CalculationOptionsMage calculationOptions = castingState.CalculationOptions;

            if (CritRate < 0.0f) CritRate = 0.0f;
            if (CritRate > 1.0f) CritRate = 1.0f;

            HitProcs = Ticks * HitRate;
            CritProcs = HitProcs * CritRate;
            TargetProcs = HitProcs;

            if (Instant) InterruptProtection = 1;
            if (castingState.IcyVeins) InterruptProtection = 1;

            float channelReduction;
            CastTime = template.CalculateCastTime(castingState.Calculations.HasteRatingEffects, calculationOptions, castingState.CastingSpeed, castingState.SpellHasteRating, InterruptProtection, CritRate, pom, BaseCastTime, out channelReduction);

            if (Ticks > 0)
            {
                SpammedDot = spammedDot;
                if (!forceMiss)
                {
                    float damagePerSpellPower;
                    AverageDamage = CalculateAverageDamage(baseStats, calculationOptions, RawSpellDamage, spammedDot, forceHit, out damagePerSpellPower);

                    DamagePerSecond = AverageDamage / CastTime;
                    ThreatPerSecond = DamagePerSecond * ThreatMultiplier;
                    DpsPerSpellPower = damagePerSpellPower / CastTime;
                }
            }
            CastTime *= (1 - channelReduction);
            CostPerSecond = CalculateCost(mageTalents, round) / CastTime;

            if (outOfFiveSecondRule)
            {
                OO5SR = 1;
            }
        }

        public virtual float CalculateAverageDamage(Stats baseStats, CalculationOptionsMage calculationOptions, float spellPower, bool spammedDot, bool forceHit, out float damagePerSpellPower)
        {
            float baseAverage = (BaseMinDamage + BaseMaxDamage) / 2f;
            float critMultiplier = 1 + (CritBonus - 1) * Math.Max(0, CritRate/* - castingState.ResilienceCritRateReduction*/);
            float resistMultiplier = (forceHit ? 1.0f : HitRate) * PartialResistFactor;
            float commonMultiplier = SpellModifier * resistMultiplier;
            float nukeMultiplier = commonMultiplier * DirectDamageModifier * critMultiplier;
            float averageDamage = baseAverage * nukeMultiplier;
            damagePerSpellPower = SpellDamageCoefficient * nukeMultiplier;
            if (BasePeriodicDamage > 0.0f)
            {
                float dotFactor = commonMultiplier * DotDamageModifier;
                if (spammedDot)
                {
                    dotFactor *= CastTime / DotDuration;
                }
                averageDamage += BasePeriodicDamage * dotFactor;
                damagePerSpellPower += DotDamageCoefficient * dotFactor;
            }
            return averageDamage + damagePerSpellPower * spellPower;
        }

        private float CalculateCost(MageTalents mageTalents, bool round)
        {
            float cost;
            if (round)
            {
                cost = (float)Math.Floor(Math.Round(BaseCost * CostAmplifier) * CostModifier);
            }
            else
            {
                cost = (float)Math.Floor(BaseCost * CostAmplifier * CostModifier);
            }

            if (MagicSchool == MagicSchool.Fire || MagicSchool == MagicSchool.FrostFire) cost += CritRate * cost * 0.01f * mageTalents.Burnout; // last I read Burnout works on final pre MOE cost

            cost *= (1 - 0.02f * mageTalents.ArcaneConcentration);

            // from what I know MOE works on base cost
            // not tested, but I think if you get MOE proc on a spell while CC is active you still get mana return
            cost -= CritRate * BaseCost * 0.1f * mageTalents.MasterOfElements;
            return cost;
        }

        public void CalculateManualClearcastingCost(MageTalents mageTalents, bool round, bool manualClearcasting, bool clearcastingAveraged, bool clearcastingActive)
        {
            float cost;
            if (round)
            {
                cost = (float)Math.Floor(Math.Round(BaseCost * CostAmplifier) * CostModifier);
            }
            else
            {
                cost = (float)Math.Floor(BaseCost * CostAmplifier * CostModifier);
            }

            if (MagicSchool == MagicSchool.Fire || MagicSchool == MagicSchool.FrostFire) cost += CritRate * cost * 0.01f * mageTalents.Burnout; // last I read Burnout works on final pre MOE cost

            if (!manualClearcasting || clearcastingAveraged)
            {
                cost *= (1 - 0.02f * mageTalents.ArcaneConcentration);
            }
            else if (clearcastingActive)
            {
                cost = 0;
            }

            // from what I know MOE works on base cost
            // not tested, but I think if you get MOE proc on a spell while CC is active you still get mana return
            cost -= CritRate * BaseCost * 0.1f * mageTalents.MasterOfElements;
            CostPerSecond = cost / CastTime;
        }

        public void AddSpellContribution(Dictionary<string, SpellContribution> dict, float duration)
        {
            SpellContribution contrib;
            if (!dict.TryGetValue(Name, out contrib))
            {
                contrib = new SpellContribution() { Name = Name };
                dict[Name] = contrib;
            }
            contrib.Hits += HitProcs * duration / CastTime;
            contrib.Damage += AverageDamage * duration / CastTime;
            contrib.Range = Range;
        }

        public void AddManaUsageContribution(Dictionary<string, float> dict, float duration)
        {
            float contrib;
            dict.TryGetValue(Name, out contrib);
            contrib += CostPerSecond * duration;
            dict[Name] = contrib;
        }
    }
}
