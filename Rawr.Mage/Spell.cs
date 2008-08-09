using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace Rawr.Mage
{
    public enum MagicSchool
    {
        Fire = 2,
        Nature,
        Frost,
        Shadow,
        Arcane
    }

    public enum SpellId
    {
        None,
        Wand,
        LightningBolt,
        [Description("Arcane Barrage")]
        ArcaneBarrage,
        ArcaneBolt,
        [Description("Arcane Missiles")]
        ArcaneMissiles,
        ArcaneMissilesMB,
        ArcaneMissilesCC,
        ArcaneMissilesNoProc,
        //ArcaneMissilesFTF,
        //ArcaneMissilesFTT,
        [Description("Frostbolt")]
        Frostbolt,
        [Description("POM+Frostbolt")]
        FrostboltPOM,
        FrostboltNoCC,
        [Description("Fireball")]
        Fireball,
        [Description("POM+Fireball")]
        FireballPOM,
        [Description("Pyroblast")]
        Pyroblast,
        [Description("POM+Pyroblast")]
        PyroblastPOM,
        [Description("Fire Blast")]
        FireBlast,
        [Description("Scorch")]
        Scorch,
        ScorchNoCC,
        [Description("Arcane Blast")]
        ArcaneBlast33,
        ArcaneBlast33NoCC,
        ArcaneBlast00,
        ArcaneBlast00NoCC,
        ArcaneBlast0POM,
        ArcaneBlast10,
        ArcaneBlast01,
        ArcaneBlast11,
        ArcaneBlast11NoCC,
        ArcaneBlast22,
        ArcaneBlast22NoCC,
        ArcaneBlast12,
        ArcaneBlast23,
        ArcaneBlast30,
        ABAM,
        ABMBAM,
        ABAMP,
        AB3AMSc,
        ABAM3Sc,
        ABAM3Sc2,
        ABAM3FrB,
        ABAM3FrB2,
        ABFrB,
        AB3FrB,
        ABFrB3FrB,
        ABFrB3FrB2,
        ABFrB3FrBSc,
        ABFB3FBSc,
        AB3Sc,
        FireballScorch,
        FireballFireBlast,
        ABAM3ScCCAM,
        ABAM3Sc2CCAM,
        ABAM3FrBCCAM,
        ABAM3FrBCCAMFail,
        ABAM3FrBScCCAM,
        ABAMCCAM,
        ABAM3CCAM,
        [Description("Arcane Explosion")]
        ArcaneExplosion,
        FlamestrikeSpammed,
        [Description("Flamestrike")]
        FlamestrikeSingle,
        [Description("Blizzard")]
        Blizzard,
        [Description("Blast Wave")]
        BlastWave,
        [Description("Dragon's Breath")]
        DragonsBreath,
        [Description("Cone of Cold")]
        ConeOfCold,
        CustomSpellMix
    }

    public struct SpellData
    {
        public float MinDamage;
        public float MaxDamage;
        public float PeriodicDamage;
        public int Cost;
        public float SpellDamageCoefficient;
    }

    public abstract class Spell
    {
        public string Name;
        public SpellId SpellId;

        public float DamagePerSecond;
        public float ThreatPerSecond;
        public float CostPerSecond;
        public float ManaRegenPerSecond;

        public float ManaPerSecond
        {
            get
            {
                return CostPerSecond - ManaRegenPerSecond;
            }
        }

        public bool AffectedByFlameCap;
        public bool ABCycle;

        public bool AreaEffect;

        protected string sequence = null;
        public virtual string Sequence
        {
            get
            {
                return sequence;
            }
        }

        public bool Channeled;
        public float HitProcs;
        public float CastProcs;
        public float CastTime;
    }

    public class SpellCustomMix : Spell
    {
        public SpellCustomMix(CastingState castingState)
        {
            sequence = "Custom Mix";
            Name = "Custom Mix";
            if (castingState.CalculationOptions.CustomSpellMix == null) return;
            float weightTotal = 0f;
            foreach (SpellWeight spellWeight in castingState.CalculationOptions.CustomSpellMix)
            {
                Spell spell = castingState.GetSpell(spellWeight.Spell);
                weightTotal += spellWeight.Weight;
                CastTime += spellWeight.Weight * spell.CastTime;
                CostPerSecond += spellWeight.Weight * spell.CostPerSecond;
                DamagePerSecond += spellWeight.Weight * spell.DamagePerSecond;
                ThreatPerSecond += spellWeight.Weight * spell.ThreatPerSecond;
                ManaRegenPerSecond += spellWeight.Weight * spell.ManaRegenPerSecond;
            }
            CastTime /= weightTotal;
            CostPerSecond /= weightTotal;
            DamagePerSecond /= weightTotal;
            ThreatPerSecond /= weightTotal;
            ManaRegenPerSecond /= weightTotal;
        }
    }

    abstract class BaseSpell : Spell
    {
        public bool Instant;
        public bool Binary;
        public int BaseCost;
        public int BaseRange;
        public float BaseCastTime;
        public float BaseCooldown;
        public MagicSchool MagicSchool;
        public float BaseMinDamage;
        public float BaseMaxDamage;
        public float BasePeriodicDamage;
        public float SpellDamageCoefficient;
        public float DotDamageCoefficient;
        public float DotDuration;
        public bool SpammedDot;
        public float TargetProcs;
        public float AoeDamageCap;
        public float MinHitDamage;
        public float MaxHitDamage;
        public float MinCritDamage;
        public float MaxCritDamage;
        public float DotDamage;

        protected static int RankLevelIndex(int rank, int level)
        {
            return rank * 100 + level;
        }

        public BaseSpell(string name, bool channeled, bool binary, bool instant, bool areaEffect, int range, float castTime, float cooldown, MagicSchool magicSchool, SpellData spellData) : this(name, channeled, binary, instant, areaEffect, spellData.Cost, range, castTime, cooldown, magicSchool, spellData.MinDamage, spellData.MaxDamage, spellData.PeriodicDamage, 1, 1, spellData.SpellDamageCoefficient, 0, 0, false) { }
        public BaseSpell(string name, bool channeled, bool binary, bool instant, bool areaEffect, int range, float castTime, float cooldown, MagicSchool magicSchool, SpellData spellData, float hitProcs, float castProcs) : this(name, channeled, binary, instant, areaEffect, spellData.Cost, range, castTime, cooldown, magicSchool, spellData.MinDamage, spellData.MaxDamage, spellData.PeriodicDamage, hitProcs, castProcs, spellData.SpellDamageCoefficient, 0, 0, false) { }
        public BaseSpell(string name, bool channeled, bool binary, bool instant, bool areaEffect, int cost, int range, float castTime, float cooldown, MagicSchool magicSchool, float minDamage, float maxDamage, float periodicDamage, float spellDamageCoefficient) : this(name, channeled, binary, instant, areaEffect, cost, range, castTime, cooldown, magicSchool, minDamage, maxDamage, periodicDamage, 1, 1, spellDamageCoefficient, 0, 0, false) { }
        public BaseSpell(string name, bool channeled, bool binary, bool instant, bool areaEffect, int cost, int range, float castTime, float cooldown, MagicSchool magicSchool, float minDamage, float maxDamage, float periodicDamage) : this(name, channeled, binary, instant, areaEffect, cost, range, castTime, cooldown, magicSchool, minDamage, maxDamage, periodicDamage, 1, 1, instant ? (1.5f / 3.5f) : (castTime / 3.5f), 0, 0, false) { }
        public BaseSpell(string name, bool channeled, bool binary, bool instant, bool areaEffect, int cost, int range, float castTime, float cooldown, MagicSchool magicSchool, float minDamage, float maxDamage, float periodicDamage, float hitProcs, float castProcs) : this(name, channeled, binary, instant, areaEffect, cost, range, castTime, cooldown, magicSchool, minDamage, maxDamage, periodicDamage, hitProcs, castProcs, instant ? (1.5f / 3.5f) : (castTime / 3.5f), 0, 0, false) { }
        public BaseSpell(string name, bool channeled, bool binary, bool instant, bool areaEffect, int cost, int range, float castTime, float cooldown, MagicSchool magicSchool, float minDamage, float maxDamage, float periodicDamage, float hitProcs, float castProcs, float spellDamageCoefficient, float dotDamageCoefficient, float dotDuration, bool spammedDot)
        {
            Name = name;
            Channeled = channeled;
            Binary = binary;
            Instant = instant;
            AreaEffect = areaEffect;
            BaseCost = cost;
            BaseRange = range;
            BaseCastTime = castTime;
            BaseCooldown = cooldown;
            MagicSchool = magicSchool;
            BaseMinDamage = minDamage;
            BaseMaxDamage = maxDamage;
            BasePeriodicDamage = periodicDamage;
            SpellDamageCoefficient = spellDamageCoefficient;
            HitProcs = hitProcs;
            CastProcs = castProcs;
            TargetProcs = hitProcs;
            DotDamageCoefficient = dotDamageCoefficient;
            DotDuration = dotDuration;
            SpammedDot = spammedDot;
        }

        public float CostModifier;
        public float SpellModifier;
        public float RealResistance;
        public float CritRate;
        public float ThreatMultiplier;
        public float CritBonus;
        public float HitRate;
        public float CastingSpeed;
        public float GlobalCooldown;
        public float PartialResistFactor;
        public float RawSpellDamage;
        public float SpellDamage;
        public float AverageDamage;
        public bool ManualClearcasting = false;
        public bool ClearcastingAveraged;
        public bool ClearcastingActive;
        public bool ClearcastingProccing;
        public float InterruptProtection;
        public bool EffectProc;

        public float Cooldown;
        public float Cost;

        public virtual void Calculate(CastingState castingState)
        {
            if (AreaEffect) TargetProcs *= castingState.CalculationOptions.AoeTargets;
            Cooldown = BaseCooldown;

            CostModifier = 1;
            if (MagicSchool == MagicSchool.Fire) CostModifier -= 0.01f * castingState.CalculationOptions.Pyromaniac;
            if (MagicSchool == MagicSchool.Fire || MagicSchool == MagicSchool.Frost) CostModifier -= 0.01f * castingState.CalculationOptions.ElementalPrecision;
            if (MagicSchool == MagicSchool.Frost) CostModifier -= 0.05f * castingState.CalculationOptions.FrostChanneling;
            if (castingState.CalculationOptions.WotLK && MagicSchool == MagicSchool.Arcane) CostModifier -= 0.01f * castingState.CalculationOptions.ArcaneFocus;
            if (castingState.ArcanePower) CostModifier += 0.3f;
            if (MagicSchool == MagicSchool.Fire) AffectedByFlameCap = true;
            if (MagicSchool == MagicSchool.Fire) InterruptProtection += 0.35f * castingState.CalculationOptions.BurningSoul;
            InterruptProtection += castingState.BaseStats.InterruptProtection;

            switch (MagicSchool)
            {
                case MagicSchool.Arcane:
                    SpellModifier = castingState.ArcaneSpellModifier;
                    CritRate = castingState.ArcaneCritRate;
                    CritBonus = castingState.ArcaneCritBonus;
                    RawSpellDamage = castingState.ArcaneDamage;
                    HitRate = castingState.ArcaneHitRate;
                    RealResistance = castingState.CalculationOptions.ArcaneResist;
                    ThreatMultiplier = castingState.ArcaneThreatMultiplier;
                    break;
                case MagicSchool.Fire:
                    SpellModifier = castingState.FireSpellModifier;
                    CritRate = castingState.FireCritRate;
                    CritBonus = castingState.FireCritBonus;
                    RawSpellDamage = castingState.FireDamage;
                    HitRate = castingState.FireHitRate;
                    RealResistance = castingState.CalculationOptions.FireResist;
                    ThreatMultiplier = castingState.FireThreatMultiplier;
                    break;
                case MagicSchool.Frost:
                    SpellModifier = castingState.FrostSpellModifier;
                    CritRate = castingState.FrostCritRate;
                    CritBonus = castingState.FrostCritBonus;
                    RawSpellDamage = castingState.FrostDamage;
                    HitRate = castingState.FrostHitRate;
                    RealResistance = castingState.CalculationOptions.FrostResist;
                    ThreatMultiplier = castingState.FrostThreatMultiplier;
                    break;
                case MagicSchool.Nature:
                    SpellModifier = castingState.NatureSpellModifier;
                    CritRate = castingState.NatureCritRate;
                    CritBonus = castingState.NatureCritBonus;
                    RawSpellDamage = castingState.NatureDamage;
                    HitRate = castingState.NatureHitRate;
                    RealResistance = castingState.CalculationOptions.NatureResist;
                    ThreatMultiplier = castingState.NatureThreatMultiplier;
                    break;
                case MagicSchool.Shadow:
                    SpellModifier = castingState.ShadowSpellModifier;
                    CritRate = castingState.ShadowCritRate;
                    CritBonus = castingState.ShadowCritBonus;
                    RawSpellDamage = castingState.ShadowDamage;
                    HitRate = castingState.ShadowHitRate;
                    RealResistance = castingState.CalculationOptions.ShadowResist;
                    ThreatMultiplier = castingState.ShadowThreatMultiplier;
                    break;
            }

            int targetLevel = AreaEffect ? castingState.CalculationOptions.AoeTargetLevel : castingState.CalculationOptions.TargetLevel;

            // do not count debuffs for aoe effects, can't assume it will be up on all
            // do not include molten fury (molten fury relates to boss), instead amplify all by average
            if (AreaEffect)
            {
                SpellModifier = (1 + 0.01f * castingState.CalculationOptions.ArcaneInstability) * (1 + 0.01f * castingState.CalculationOptions.PlayingWithFire);
                if (castingState.ArcanePower)
                {
                    SpellModifier *= 1.3f;
                }
                if (castingState.CalculationOptions.MoltenFury > 0)
                {
                    SpellModifier *= (1 + 0.1f * castingState.CalculationOptions.MoltenFury * castingState.CalculationOptions.MoltenFuryPercentage);
                }
                if (MagicSchool == MagicSchool.Fire) SpellModifier *= (1 + 0.02f * castingState.CalculationOptions.FirePower);
                if (MagicSchool == MagicSchool.Frost) SpellModifier *= (1 + 0.02f * castingState.CalculationOptions.PiercingIce);

                if (MagicSchool == MagicSchool.Arcane) HitRate = Math.Min(0.99f, ((targetLevel <= 72) ? (0.96f - (targetLevel - 70) * 0.01f) : (0.94f - (targetLevel - 72) * 0.11f)) + castingState.SpellHit + (castingState.CalculationOptions.WotLK ? 0.01f : 0.02f) * castingState.CalculationOptions.ArcaneFocus);
                if (MagicSchool == MagicSchool.Fire) HitRate = Math.Min(0.99f, ((targetLevel <= 72) ? (0.96f - (targetLevel - 70) * 0.01f) : (0.94f - (targetLevel - 72) * 0.11f)) + castingState.SpellHit + 0.01f * castingState.CalculationOptions.ElementalPrecision);
                if (MagicSchool == MagicSchool.Frost) HitRate = Math.Min(0.99f, ((targetLevel <= 72) ? (0.96f - (targetLevel - 70) * 0.01f) : (0.94f - (targetLevel - 72) * 0.11f)) + castingState.SpellHit + 0.01f * castingState.CalculationOptions.ElementalPrecision);
                if (MagicSchool == MagicSchool.Nature) HitRate = Math.Min(0.99f, ((targetLevel <= 72) ? (0.96f - (targetLevel - 70) * 0.01f) : (0.94f - (targetLevel - 72) * 0.11f)) + castingState.SpellHit);
                if (MagicSchool == MagicSchool.Shadow) HitRate = Math.Min(0.99f, ((targetLevel <= 72) ? (0.96f - (targetLevel - 70) * 0.01f) : (0.94f - (targetLevel - 72) * 0.11f)) + castingState.SpellHit);
            }

            if (ManualClearcasting && !ClearcastingAveraged)
            {
                CritRate -= (castingState.CalculationOptions.WotLK ? 0.015f : 0.01f) * castingState.CalculationOptions.ArcanePotency; // replace averaged arcane potency with actual % chance
                if (ClearcastingActive) CritRate += (castingState.CalculationOptions.WotLK ? 0.15f : 0.1f) * castingState.CalculationOptions.ArcanePotency;
            }

            if (castingState.CalculationOptions.WotLK)
            {
                PartialResistFactor = (RealResistance == 1) ? 0 : (1 - Math.Max(0f, RealResistance - castingState.BaseStats.SpellPenetration / 350f * 0.75f) - ((targetLevel > 70) ? ((targetLevel - 70) * 0.02f) : 0f));
            }
            else
            {
                PartialResistFactor = (RealResistance == 1) ? 0 : (1 - Math.Max(0f, RealResistance - castingState.BaseStats.SpellPenetration / 350f * 0.75f) - ((targetLevel > 70 && !Binary) ? ((targetLevel - 70) * 0.02f) : 0f));
            }
        }

        private float ProcBuffUp(float procChance, float buffDuration, float triggerInterval)
        {
            if (triggerInterval <= 0)
                return 0;
            else
                return 1 - (float)Math.Pow(1 - procChance, buffDuration / triggerInterval);
        }

        protected void CalculateDerivedStats(CastingState castingState)
        {
            CastingSpeed = castingState.CastingSpeed;

            if (Instant) InterruptProtection = 1;
            if (castingState.IcyVeins) InterruptProtection = 1;
            float InterruptFactor = castingState.CalculationOptions.InterruptFrequency * Math.Max(0, 1 - InterruptProtection);

            float Haste = castingState.SpellHasteRating;
            float levelScalingFactor = (1 - (70 - 60) / 82f * 3);

            // TODO consider converting to discrete model for procs

            GlobalCooldown = Math.Max(castingState.GlobalCooldownLimit, 1.5f / CastingSpeed);
            CastTime = BaseCastTime / CastingSpeed + castingState.Latency;
            CastTime = CastTime * (1 + InterruptFactor) - (0.5f + castingState.Latency) * InterruptFactor;
            if (CastTime < GlobalCooldown + castingState.Latency) CastTime = GlobalCooldown + castingState.Latency;

            // Quagmirran
            if (castingState.BaseStats.SpellHasteFor6SecOnHit_10_45 > 0 && HitProcs > 0)
            {
                // hasted casttime
                float speed = CastingSpeed / (1 + Haste / 995f * levelScalingFactor) * (1 + (Haste + castingState.BaseStats.SpellHasteFor6SecOnHit_10_45) / 995f * levelScalingFactor);
                float gcd = Math.Max(castingState.GlobalCooldownLimit, 1.5f / speed);
                float cast = BaseCastTime / speed + castingState.Latency;
                cast = cast * (1 + InterruptFactor) - (0.5f + castingState.Latency) * InterruptFactor;
                if (cast < gcd + castingState.Latency) cast = gcd + castingState.Latency;

                CastingSpeed /= (1 + Haste / 995f * levelScalingFactor);
                //discrete model
                float castsAffected = 0;
                for (int c = 0; c < HitProcs; c++) castsAffected += (float)Math.Ceiling((6f - c * CastTime / HitProcs) / cast) / HitProcs;
                Haste += castingState.BaseStats.SpellHasteFor6SecOnHit_10_45 * castsAffected * cast / (45f + CastTime / HitProcs / 0.1f);
                //continuous model
                //Haste += castingState.BasicStats.SpellHasteFor6SecOnHit_10_45 * 6f / (45f + CastTime / HitProcs / 0.1f);
                CastingSpeed *= (1 + Haste / 995f * levelScalingFactor);

                GlobalCooldown = Math.Max(castingState.GlobalCooldownLimit, 1.5f / CastingSpeed);
                CastTime = BaseCastTime / CastingSpeed + castingState.Latency;
                CastTime = CastTime * (1 + InterruptFactor) - (0.5f + castingState.Latency) * InterruptFactor;
                if (CastTime < GlobalCooldown + castingState.Latency) CastTime = GlobalCooldown + castingState.Latency;
            }

            // MSD
            if (castingState.BaseStats.SpellHasteFor6SecOnCast_15_45 > 0 && CastProcs > 0)
            {
                // hasted casttime
                float speed = CastingSpeed / (1 + Haste / 995f * levelScalingFactor) * (1 + (Haste + castingState.BaseStats.SpellHasteFor6SecOnCast_15_45) / 995f * levelScalingFactor);
                float gcd = Math.Max(castingState.GlobalCooldownLimit, 1.5f / speed);
                float cast = BaseCastTime / speed + castingState.Latency;
                cast = cast * (1 + InterruptFactor) - (0.5f + castingState.Latency) * InterruptFactor;
                if (cast < gcd + castingState.Latency) cast = gcd + castingState.Latency;

                CastingSpeed /= (1 + Haste / 995f * levelScalingFactor);
                float castsAffected = 0;
                for (int c = 0; c < CastProcs; c++) castsAffected += (float)Math.Ceiling((6f - c * CastTime / CastProcs) / cast) / CastProcs;
                Haste += castingState.BaseStats.SpellHasteFor6SecOnCast_15_45 * castsAffected * cast / (45f + CastTime / CastProcs / 0.15f);
                //Haste += castingState.BasicStats.SpellHasteFor6SecOnCast_15_45 * 6f / (45f + CastTime / CastProcs / 0.15f);
                CastingSpeed *= (1 + Haste / 995f * levelScalingFactor);

                GlobalCooldown = Math.Max(castingState.GlobalCooldownLimit, 1.5f / CastingSpeed);
                CastTime = BaseCastTime / CastingSpeed + castingState.Latency;
                CastTime = CastTime * (1 + InterruptFactor) - (0.5f + castingState.Latency) * InterruptFactor;
                if (CastTime < GlobalCooldown + castingState.Latency) CastTime = GlobalCooldown + castingState.Latency;
            }


            // AToI, first cast after proc is not affected for non-instant
            if (castingState.BaseStats.SpellHasteFor5SecOnCrit_50 > 0)
            {
                float rawHaste = Haste;

                CastingSpeed /= (1 + Haste / 995f * levelScalingFactor);
                float proccedSpeed = CastingSpeed * (1 + (rawHaste + castingState.BaseStats.SpellHasteFor5SecOnCrit_50) / 995f * levelScalingFactor);
                float proccedGcd = Math.Max(castingState.GlobalCooldownLimit, 1.5f / proccedSpeed);
                float proccedCastTime = BaseCastTime / proccedSpeed + castingState.Latency;
                proccedCastTime = proccedCastTime * (1 + InterruptFactor) - (0.5f + castingState.Latency) * InterruptFactor;
                if (proccedCastTime < proccedGcd + castingState.Latency) proccedCastTime = proccedGcd + castingState.Latency;
                int chancesToProc = (int)(((int)Math.Floor(5f / proccedCastTime) + 1) * HitProcs);
                if (!Instant) chancesToProc -= 1;
                chancesToProc *= (int)(TargetProcs / HitProcs);
                Haste = rawHaste + castingState.BaseStats.SpellHasteFor5SecOnCrit_50 * (1 - (float)Math.Pow(1 - 0.5f * CritRate, chancesToProc));
                //Haste = rawHaste + castingState.BasicStats.SpellHasteFor5SecOnCrit_50 * ProcBuffUp(1 - (float)Math.Pow(1 - 0.5f * CritRate, HitProcs), 5, CastTime);
                CastingSpeed *= (1 + Haste / 995f * levelScalingFactor);
                GlobalCooldown = Math.Max(castingState.GlobalCooldownLimit, 1.5f / CastingSpeed);
                CastTime = BaseCastTime / CastingSpeed + castingState.Latency;
                CastTime = CastTime * (1 + InterruptFactor) - (0.5f + castingState.Latency) * InterruptFactor;
                if (CastTime < GlobalCooldown + castingState.Latency) CastTime = GlobalCooldown + castingState.Latency;
            }

            Cost = BaseCost * CostModifier;

            CritRate = Math.Min(1, CritRate);
            if (MagicSchool == MagicSchool.Fire || MagicSchool == MagicSchool.Frost) Cost *= (1f - CritRate * 0.1f * castingState.CalculationOptions.MasterOfElements);

            CostPerSecond = Cost / CastTime;

            if (!ManualClearcasting || ClearcastingAveraged)
            {
                CostPerSecond *= (1 - 0.02f * castingState.CalculationOptions.ArcaneConcentration);
            }
            else if (ClearcastingActive)
            {
                CostPerSecond = 0;
            }

            // hit & crit ranges, do it before passive spell damage effects for cleaner comparison in game
            MinHitDamage = (BaseMinDamage + RawSpellDamage * SpellDamageCoefficient) * SpellModifier;
            MaxHitDamage = (BaseMaxDamage + RawSpellDamage * SpellDamageCoefficient) * SpellModifier;
            MinCritDamage = MinHitDamage * CritBonus;
            MaxCritDamage = MaxHitDamage * CritBonus;
            DotDamage = (BasePeriodicDamage + DotDamageCoefficient * RawSpellDamage) * SpellModifier;

            if (castingState.BaseStats.SpellDamageFor10SecOnHit_5 > 0) RawSpellDamage += castingState.BaseStats.SpellDamageFor10SecOnHit_5 * ProcBuffUp(1 - (float)Math.Pow(0.95, TargetProcs), 10, CastTime);
            if (castingState.BaseStats.SpellDamageFor6SecOnCrit > 0) RawSpellDamage += castingState.BaseStats.SpellDamageFor6SecOnCrit * ProcBuffUp(1 - (float)Math.Pow(1 - CritRate, HitProcs), 6, CastTime);
            if (castingState.BaseStats.SpellDamageFor10SecOnHit_10_45 > 0) RawSpellDamage += castingState.BaseStats.SpellDamageFor10SecOnHit_10_45 * 10f / (45f + CastTime / HitProcs / 0.1f);
            if (castingState.BaseStats.SpellDamageFor10SecOnResist > 0) RawSpellDamage += castingState.BaseStats.SpellDamageFor10SecOnResist * ProcBuffUp(1 - (float)Math.Pow(HitRate, HitProcs), 10, CastTime);
            if (castingState.BaseStats.SpellDamageFor15SecOnCrit_20_45 > 0) RawSpellDamage += castingState.BaseStats.SpellDamageFor15SecOnCrit_20_45 * 15f / (45f + CastTime / HitProcs / 0.2f / CritRate);
            if (castingState.BaseStats.SpellDamageFor10SecOnCrit_20_45 > 0) RawSpellDamage += castingState.BaseStats.SpellDamageFor10SecOnCrit_20_45 * 10f / (45f + CastTime / HitProcs / 0.2f / CritRate);
            if (castingState.BaseStats.ShatteredSunAcumenProc > 0 && castingState.CalculationOptions.Aldor) RawSpellDamage += 120 * 10f / (45f + CastTime / HitProcs / 0.1f);

            SpellDamage = RawSpellDamage * SpellDamageCoefficient;
            float baseAverage = (BaseMinDamage + BaseMaxDamage) / 2f + SpellDamage;
            float critMultiplier = 1 + (CritBonus - 1) * Math.Max(0, CritRate - castingState.ResilienceCritRateReduction);
            float resistMultiplier = HitRate * PartialResistFactor;
            int targets = 1;
            if (AreaEffect) targets = castingState.CalculationOptions.AoeTargets;
            AverageDamage = baseAverage * SpellModifier * targets * HitRate;
            if (AreaEffect && AverageDamage > AoeDamageCap) AverageDamage = AoeDamageCap;
            AverageDamage = AverageDamage * critMultiplier * PartialResistFactor;
            if (SpammedDot)
            {
                AverageDamage += targets * (BasePeriodicDamage + DotDamageCoefficient * RawSpellDamage) * SpellModifier * resistMultiplier * CastTime / DotDuration;
            }
            else
            {
                AverageDamage += targets * (BasePeriodicDamage + DotDamageCoefficient * RawSpellDamage) * SpellModifier * resistMultiplier;
            }
            DamagePerSecond = AverageDamage / CastTime;
            ThreatPerSecond = DamagePerSecond * ThreatMultiplier;

            if (!EffectProc && castingState.BaseStats.LightningCapacitorProc > 0)
            {
                BaseSpell LightningBolt = (BaseSpell)castingState.GetSpell(SpellId.LightningBolt);
                //discrete model
                int hitsInsideCooldown = (int)(2.5f / (CastTime / HitProcs));
                float avgCritsPerHit = CritRate * TargetProcs / HitProcs;
                float avgHitsToDischarge = 3f / avgCritsPerHit;
                if (avgHitsToDischarge < 1) avgHitsToDischarge = 1;
                float boltDps = LightningBolt.AverageDamage / ((CastTime / HitProcs) * (hitsInsideCooldown + avgHitsToDischarge));
                DamagePerSecond += boltDps;
                ThreatPerSecond += boltDps * castingState.NatureThreatMultiplier;
                //continuous model
                //DamagePerSecond += LightningBolt.AverageDamage / (2.5f + 3f * CastTime / (CritRate * TargetProcs));
            }
            if (!EffectProc && castingState.BaseStats.ShatteredSunAcumenProc > 0 && !castingState.CalculationOptions.Aldor)
            {
                BaseSpell ArcaneBolt = (BaseSpell)castingState.GetSpell(SpellId.ArcaneBolt);
                float boltDps = ArcaneBolt.AverageDamage / (45f + CastTime / HitProcs / 0.1f);
                DamagePerSecond += boltDps;
                ThreatPerSecond += boltDps * castingState.ArcaneThreatMultiplier;
            }

            /*float casttimeHash = castingState.ClearcastingChance * 100 + CastTime;
            float OO5SR = 0;
            if (!FSRCalc.TryGetCachedOO5SR(Name, casttimeHash, out OO5SR))
            {
                FSRCalc fsr = new FSRCalc();
                fsr.AddSpell(CastTime - castingState.Latency, castingState.Latency, Channeled);
                OO5SR = fsr.CalculateOO5SR(castingState.ClearcastingChance, Name, casttimeHash);
            }*/

            float OO5SR;

            if (Cost > 0)
            {
                OO5SR = FSRCalc.CalculateSimpleOO5SR(castingState.ClearcastingChance, CastTime - castingState.Latency, castingState.Latency, Channeled);
            }
            else
            {
                OO5SR = 1;
            }

            ManaRegenPerSecond = castingState.ManaRegen5SR + OO5SR * (castingState.ManaRegen - castingState.ManaRegen5SR) + castingState.BaseStats.ManaRestorePerHit * HitProcs / CastTime + castingState.BaseStats.ManaRestorePerCast * CastProcs / CastTime + castingState.BaseStats.ManaRestorePerCast_5_15 / (15f + CastTime / CastProcs / 0.05f);
            ThreatPerSecond += (castingState.BaseStats.ManaRestorePerHit * HitProcs / CastTime + castingState.BaseStats.ManaRestorePerCast * CastProcs / CastTime) * 0.5f * (1 + castingState.BaseStats.ThreatIncreaseMultiplier) * (1 - castingState.BaseStats.ThreatReductionMultiplier);

            if (castingState.Mp5OnCastFor20Sec > 0 && CastProcs > 0)
            {
                float totalMana = castingState.Mp5OnCastFor20Sec / 5f / CastTime * 0.5f * (20 - CastTime / HitProcs / 2f) * (20 - CastTime / HitProcs / 2f);
                ManaRegenPerSecond += totalMana / 20f;
                ThreatPerSecond += totalMana / 20f * 0.5f * (1 + castingState.BaseStats.ThreatIncreaseMultiplier) * (1 - castingState.BaseStats.ThreatReductionMultiplier);
            }
        }
    }

    #region Base Spells
    class Wand : BaseSpell
    {
        public Wand(CastingState castingState, MagicSchool school, int minDamage, int maxDamage, float speed)
            : base("Wand", false, false, false, false, 0, 30, 0, 0, school, minDamage, maxDamage, 0, 1, 0, 0, 0, 0, false)
        {
            // Tested: affected by Arcane Instability, affected by Chaotic meta, not affected by Arcane Power
            Calculate(castingState);
            CastTime = speed;
            CritRate = castingState.SpellCrit;
            CritBonus = (1 + (1.5f * (1 + castingState.BaseStats.BonusSpellCritMultiplier) - 1)) * castingState.ResilienceCritDamageReduction;
            SpellModifier = (1 + 0.01f * castingState.CalculationOptions.ArcaneInstability) * (1 + 0.01f * castingState.CalculationOptions.PlayingWithFire) * (1 + castingState.BaseStats.BonusSpellPowerMultiplier);
            switch (school)
            {
                case MagicSchool.Arcane:
                    SpellModifier *= (1 + castingState.BaseStats.BonusArcaneSpellPowerMultiplier);
                    break;
                case MagicSchool.Fire:
                    SpellModifier *= (1 + castingState.BaseStats.BonusFireSpellPowerMultiplier);
                    break;
                case MagicSchool.Frost:
                    SpellModifier *= (1 + castingState.BaseStats.BonusFrostSpellPowerMultiplier);
                    break;
                case MagicSchool.Nature:
                    SpellModifier *= (1 + castingState.BaseStats.BonusNatureSpellPowerMultiplier);
                    break;
                case MagicSchool.Shadow:
                    SpellModifier *= (1 + castingState.BaseStats.BonusShadowSpellPowerMultiplier);
                    break;
            }
            if (castingState.CalculationOptions.WandSpecialization > 0)
            {
                SpellModifier *= 1 + 0.01f + 0.12f * castingState.CalculationOptions.WandSpecialization;
            }
            CalculateDerivedStats(castingState);
        }
    }

    class FireBlast : BaseSpell
    {
        public FireBlast(CastingState castingState)
            : base("Fire Blast", false, false, true, false, 465, 20, 0, 8, MagicSchool.Fire, 664, 786, 0)
        {
            Calculate(castingState);
        }

        public override void Calculate(CastingState castingState)
        {
            base.Calculate(castingState);
            Cooldown -= 0.5f * castingState.CalculationOptions.ImprovedFireBlast;
            CritRate += 0.02f * castingState.CalculationOptions.Incinerate;
            if (castingState.CalculationOptions.WotLK) SpellModifier *= (1 + 0.002f * castingState.CalculationOptions.SpellImpact);
            CalculateDerivedStats(castingState);
        }
    }

    class Scorch : BaseSpell
    {

        public Scorch(CastingState castingState, bool clearcastingActive)
            : base("Scorch", false, false, false, false, 180, 30, 1.5f, 0, MagicSchool.Fire, 305, 361, 0)
        {
            ManualClearcasting = true;
            ClearcastingActive = clearcastingActive;
            ClearcastingAveraged = false;
            Calculate(castingState);
        }

        public Scorch(CastingState castingState)
            : base("Scorch", false, false, false, false, 180, 30, 1.5f, 0, MagicSchool.Fire, 305, 361, 0)
        {
            Calculate(castingState);
        }

        public override void Calculate(CastingState castingState)
        {
            base.Calculate(castingState);
            CritRate += 0.02f * castingState.CalculationOptions.Incinerate;
            CalculateDerivedStats(castingState);
        }
    }

    class Flamestrike : BaseSpell
    {
        public Flamestrike(CastingState castingState, bool spammedDot)
            : base("Flamestrike", false, false, false, true, 1175, 30, 3, 0, MagicSchool.Fire, 480, 585, 424, 1, 1, 0.2363f, 0.12f, 8f, spammedDot)
        {
            base.Calculate(castingState);
            AoeDamageCap = 7830;
            CritRate += 0.05f * castingState.CalculationOptions.ImprovedFlamestrike;
            CalculateDerivedStats(castingState);
        }
    }

    class FrostNova : BaseSpell
    {
        public FrostNova(CastingState castingState)
            : base("Frost Nova", false, true, true, true, 185, 0, 0, 25, MagicSchool.Frost, 100, 113, 0, 1.5f / 3.5f * 0.5f * 0.13f)
        {
            Calculate(castingState);
        }


        public override void Calculate(CastingState castingState)
        {
            base.Calculate(castingState);
            Cooldown -= 2 * castingState.CalculationOptions.ImprovedFrostNova;
            CalculateDerivedStats(castingState);
        }
    }

    class Frostbolt : BaseSpell
    {
        public static Dictionary<int, SpellData> SpellData = new Dictionary<int, SpellData>();
        public static Dictionary<int, int> MaxRank = new Dictionary<int, int>();
        static Frostbolt()
        {
            MaxRank[70] = 14;
            SpellData[RankLevelIndex(13, 70)] = new SpellData() { Cost = 330, MinDamage = 600, MaxDamage = 647, SpellDamageCoefficient = 0.95f * 3.0f / 3.5f };
            SpellData[RankLevelIndex(14, 70)] = new SpellData() { Cost = 345, MinDamage = 630, MaxDamage = 680, SpellDamageCoefficient = 0.95f * 3.0f / 3.5f };
        }
        private static SpellData GetMaxRankSpellData(CalculationOptionsMage options)
        {
            return SpellData[RankLevelIndex(options.WotLK ? MaxRank[options.PlayerLevel] : 13, options.PlayerLevel)];
        }

        public Frostbolt(CastingState castingState, bool manualClearcasting, bool clearcastingActive, bool pom)
            : base("Frostbolt", false, true, false, false, 30, 3, 0, MagicSchool.Frost, GetMaxRankSpellData(castingState.CalculationOptions))
        {
            if (pom)
            {
                this.Instant = true;
                this.BaseCastTime = 0.0f;
            }
            if (manualClearcasting)
            {
                ManualClearcasting = true;
                ClearcastingActive = clearcastingActive;
                ClearcastingAveraged = false;
            }
            Calculate(castingState);
        }

        public Frostbolt(CastingState castingState)
            : base("Frostbolt", false, true, false, false, 30, 3, 0, MagicSchool.Frost, GetMaxRankSpellData(castingState.CalculationOptions))
        {
            Calculate(castingState);
        }


        public override void Calculate(CastingState castingState)
        {
            base.Calculate(castingState);
            BaseCastTime -= 0.1f * castingState.CalculationOptions.ImprovedFrostbolt;
            CritRate += 0.01f * castingState.CalculationOptions.EmpoweredFrostbolt;
            InterruptProtection += castingState.BaseStats.AldorRegaliaInterruptProtection;
            SpellDamageCoefficient += 0.02f * castingState.CalculationOptions.EmpoweredFrostbolt;
            int targetLevel = castingState.CalculationOptions.TargetLevel;
            HitRate = Math.Min(0.99f, ((targetLevel <= 72) ? (0.96f - (targetLevel - 70) * 0.01f) : (0.94f - (targetLevel - 72) * 0.11f)) + castingState.SpellHit + 0.02f * castingState.CalculationOptions.ElementalPrecision); // bugged Elemental Precision
            SpellModifier *= (1 + castingState.BaseStats.BonusMageNukeMultiplier);
            CalculateDerivedStats(castingState);
        }
    }

    class Fireball : BaseSpell
    {
        public static Dictionary<int, SpellData> SpellData = new Dictionary<int, SpellData>();
        public static Dictionary<int, int> MaxRank = new Dictionary<int, int>();
        static Fireball()
        {
            MaxRank[70] = 14;
            SpellData[RankLevelIndex(13, 70)] = new SpellData() { Cost = 425, MinDamage = 649, MaxDamage = 821, PeriodicDamage = 84, SpellDamageCoefficient = 3.5f / 3.5f };
            SpellData[RankLevelIndex(14, 70)] = new SpellData() { Cost = 465, MinDamage = 717, MaxDamage = 913, PeriodicDamage = 92, SpellDamageCoefficient = 3.5f / 3.5f };
        }
        private static SpellData GetMaxRankSpellData(CalculationOptionsMage options)
        {
            return SpellData[RankLevelIndex(options.WotLK ? MaxRank[options.PlayerLevel] : 13, options.PlayerLevel)];
        }

        public Fireball(CastingState castingState, bool pom)
            : base("Fireball", false, false, false, false, 35, 3.5f, 0, MagicSchool.Fire, GetMaxRankSpellData(castingState.CalculationOptions))
        {
            if (pom)
            {
                this.Instant = true;
                this.BaseCastTime = 0.0f;
            }
            Calculate(castingState);
            SpammedDot = true;
            DotDuration = 8;
            InterruptProtection += castingState.BaseStats.AldorRegaliaInterruptProtection;
            BaseCastTime -= 0.1f * castingState.CalculationOptions.ImprovedFireball;
            SpellDamageCoefficient += 0.03f * castingState.CalculationOptions.EmpoweredFireball;
            SpellModifier *= (1 + castingState.BaseStats.BonusMageNukeMultiplier);
            CalculateDerivedStats(castingState);
        }
    }

    class Pyroblast : BaseSpell
    {
        public Pyroblast(CastingState castingState, bool pom)
            : base("Pyroblast", false, false, false, false, 500, 35, 6f, 0, MagicSchool.Fire, 939, 1191, 356)
        {
            if (pom)
            {
                this.Instant = true;
                this.BaseCastTime = 0.0f;
            }
            Calculate(castingState);
            SpammedDot = false;
            DotDuration = 12;
            SpellDamageCoefficient = 1.15f;
            DotDamageCoefficient = 0.2f;
            CalculateDerivedStats(castingState);
        }
    }

    class ConeOfCold : BaseSpell
    {
        public ConeOfCold(CastingState castingState)
            : base("Cone of Cold", false, true, true, true, 645, 0, 0, 10, MagicSchool.Frost, 418, 457, 0, 0.1357f)
        {
            base.Calculate(castingState);
            AoeDamageCap = 6500;
            int ImprovedConeOfCold = castingState.CalculationOptions.ImprovedConeOfCold;
            if (castingState.CalculationOptions.WotLK)
            {
                SpellModifier *= (1 + ((ImprovedConeOfCold > 0) ? (0.05f + 0.1f * ImprovedConeOfCold) : 0) + 0.002f * castingState.CalculationOptions.SpellImpact);
                CritRate += 0.02f * castingState.CalculationOptions.Incinerate;
            }
            else
            {
                SpellModifier *= (1 + ((ImprovedConeOfCold > 0) ? (0.05f + 0.1f * ImprovedConeOfCold) : 0));
            }
            CalculateDerivedStats(castingState);
        }
    }

    class ArcaneBarrage : BaseSpell
    {
        public ArcaneBarrage(CastingState castingState)
            : base("Arcane Barrage", false, false, true, false, 405, 30, 0, 3, MagicSchool.Arcane, 709, 865, 0, 3.0f / 3.5f)
        {
            Calculate(castingState);
            CalculateDerivedStats(castingState);
        }
    }

    class ArcaneBlast : BaseSpell
    {
        public ArcaneBlast(CastingState castingState, int timeDebuff, int costDebuff, bool manualClearcasting, bool clearcastingActive, bool pom)
            : base("Arcane Blast", false, false, false, false, 195, 30, 2.5f, 0, MagicSchool.Arcane, 668, 772, 0)
        {
            if (manualClearcasting)
            {
                ManualClearcasting = true;
                ClearcastingActive = clearcastingActive;
                ClearcastingAveraged = false;
            }
            if (pom)
            {
                this.Instant = true;
                this.BaseCastTime = 0.0f;
            }
            this.timeDebuff = timeDebuff;
            this.costDebuff = costDebuff;
            Calculate(castingState);
        }

        public ArcaneBlast(CastingState castingState, int timeDebuff, int costDebuff) : base("Arcane Blast", false, false, false, false, 195, 30, 2.5f, 0, MagicSchool.Arcane, 668, 772, 0)
        {
            this.timeDebuff = timeDebuff;
            this.costDebuff = costDebuff;
            Calculate(castingState);
        }

        private int timeDebuff;
        private int costDebuff;

        public override void Calculate(CastingState castingState)
        {
            base.Calculate(castingState);
            CostModifier += 0.75f * costDebuff + castingState.BaseStats.ArcaneBlastBonus;
            if (castingState.CalculationOptions.WotLK)
            {
                SpellModifier *= (1 + castingState.BaseStats.ArcaneBlastBonus + 0.25f * timeDebuff + 0.002f * castingState.CalculationOptions.SpellImpact);
                SpellDamageCoefficient += 0.03f * castingState.CalculationOptions.EmpoweredArcaneMissiles; // TODO change talent name
                CritRate += 0.02f * castingState.CalculationOptions.Incinerate;
            }
            else
            {
                SpellModifier *= (1 + castingState.BaseStats.ArcaneBlastBonus);
                BaseCastTime -= timeDebuff / 3f;
            }
            CritRate += 0.02f * castingState.CalculationOptions.ArcaneImpact;
            CalculateDerivedStats(castingState);
        }
    }

    // 582 arcane, 500-501, 1.03 amp
    // 982 arcane, 655-656 , 1.03 amp
    // 1274 arcane, 768-769, 1.03 amp
    // 1269 arcane, 805-806, 1.03 * 1.05 amp
    // 1275 arcane, 807-808 , 1.03 * 1.05 amp
    //
    // 267.55514100785945446139620896945 <= base 10 <= 267.7420527045769764216366158113
    // 287.7142857142857142857142857144 <= base 11 <= 288.7142857142857142857142857144
    class ArcaneMissiles : BaseSpell
    {
        private bool barrage;

        public static Dictionary<int, SpellData> SpellData = new Dictionary<int, SpellData>();
        public static Dictionary<int, int> MaxRank = new Dictionary<int, int>();
        static ArcaneMissiles()
        {
            MaxRank[70] = 11;
            SpellData[RankLevelIndex(10, 70)] = new SpellData() { Cost = 740, MinDamage = 267.6f * 5, MaxDamage = 267.6f * 5, SpellDamageCoefficient = 5f / 3.5f };
            SpellData[RankLevelIndex(11, 70)] = new SpellData() { Cost = 761, MinDamage = 287.9f * 5, MaxDamage = 287.9f * 5, SpellDamageCoefficient = 5f / 3.5f }; // there's some indication that coefficient might be slightly different
        }
        private static SpellData GetMaxRankSpellData(CalculationOptionsMage options)
        {
            return SpellData[RankLevelIndex(options.WotLK ? MaxRank[options.PlayerLevel] : 10, options.PlayerLevel)];
        }

        public ArcaneMissiles(CastingState castingState, bool barrage, bool clearcastingAveraged, bool clearcastingActive, bool clearcastingProccing)
            : base("Arcane Missiles", true, false, false, false, 30, 5, 0, MagicSchool.Arcane, GetMaxRankSpellData(castingState.CalculationOptions), 5, 6)
        {
            ManualClearcasting = true;
            ClearcastingActive = clearcastingActive;
            ClearcastingAveraged = clearcastingAveraged;
            ClearcastingProccing = clearcastingProccing;
            this.barrage = barrage;
            Calculate(castingState);
        }

        public ArcaneMissiles(CastingState castingState, bool barrage)
            : base("Arcane Missiles", true, false, false, false, 30, 5, 0, MagicSchool.Arcane, GetMaxRankSpellData(castingState.CalculationOptions), 5, 6)
        {
            this.barrage = barrage;
            Calculate(castingState);
        }

        public override void Calculate(CastingState castingState)
        {
            base.Calculate(castingState);
            if (barrage) BaseCastTime -= 2.5f;
            if (!castingState.CalculationOptions.WotLK) CostModifier += 0.02f * castingState.CalculationOptions.EmpoweredArcaneMissiles;

            // CC double dipping
            if (!ManualClearcasting) CritRate += (castingState.CalculationOptions.WotLK ? 0.015f : 0.01f) * castingState.CalculationOptions.ArcanePotency;
            else if (ClearcastingProccing) CritRate += (castingState.CalculationOptions.WotLK ? 0.15f : 0.1f) * castingState.CalculationOptions.ArcanePotency;

            SpellDamageCoefficient += 0.15f * castingState.CalculationOptions.EmpoweredArcaneMissiles;
            SpellModifier *= (1 + castingState.BaseStats.BonusMageNukeMultiplier);
            InterruptProtection += 0.2f * castingState.CalculationOptions.ImprovedArcaneMissiles;
            CalculateDerivedStats(castingState);
        }
    }

    class ArcaneMissilesCC : Spell
    {
        public ArcaneMissilesCC(CastingState castingState)
        {
            Name= "Arcane Missiles CC";

            //AM?1-AM11-AM11-...=0.9*0.1*...
            //...
            //AM?1-AM10=0.9

            //TIME = T * [1 + 1/0.9]
            //DAMAGE = AM?1 + AM10 + 0.1/0.9*AM11

            float CC = 0.02f * castingState.CalculationOptions.ArcaneConcentration;

            Spell AMc1 = new ArcaneMissiles(castingState, false, true, false, true);
            Spell AM10 = new ArcaneMissiles(castingState, false, false, true, false);
            Spell AM11 = new ArcaneMissiles(castingState, false, false, true, true);

            CastProcs = AMc1.CastProcs * (1 + 1 / (1 - CC));
            CastTime = AMc1.CastTime * (1 + 1 / (1 - CC));
            HitProcs = AMc1.HitProcs * (1 + 1 / (1 - CC));
            Channeled = true;
            CostPerSecond = (AMc1.CostPerSecond + AM10.CostPerSecond + CC / (1 - CC) * AM11.CostPerSecond) / (1 + 1 / (1 - CC));
            DamagePerSecond = (AMc1.DamagePerSecond + AM10.DamagePerSecond + CC / (1 - CC) * AM11.DamagePerSecond) / (1 + 1 / (1 - CC));
            ThreatPerSecond = (AMc1.ThreatPerSecond + AM10.ThreatPerSecond + CC / (1 - CC) * AM11.ThreatPerSecond) / (1 + 1 / (1 - CC));
            //ManaRegenPerSecond = (AMc1.ManaRegenPerSecond + AM10.ManaRegenPerSecond + CC / (1 - CC) * AM11.ManaRegenPerSecond) / (1 + 1 / (1 - CC)); // we only use it indirectly in spell cycles that recompute oo5sr
        }
    }

    class ArcaneExplosion : BaseSpell
    {
        public ArcaneExplosion(CastingState castingState)
            : base("Arcane Explosion", false, false, true, true, 545, 0, 0, 0, MagicSchool.Arcane, 377, 407, 0, 1.5f / 3.5f * 0.5f)
        {
            base.Calculate(castingState);
            CritRate += 0.02f * castingState.CalculationOptions.ArcaneImpact;
            if (castingState.CalculationOptions.WotLK) SpellModifier *= (1 + 0.002f * castingState.CalculationOptions.SpellImpact);
            AoeDamageCap = 10180;
            CalculateDerivedStats(castingState);
        }
    }

    class BlastWave : BaseSpell
    {
        public BlastWave(CastingState castingState)
            : base("Blast Wave", false, false, true, true, 645, 0, 0, 30, MagicSchool.Fire, 616, 724, 0, 0.1357f)
        {
            base.Calculate(castingState);
            AoeDamageCap = 9440;
            if (castingState.CalculationOptions.WotLK) SpellModifier *= (1 + 0.002f * castingState.CalculationOptions.SpellImpact);
            CalculateDerivedStats(castingState);
        }
    }

    class DragonsBreath : BaseSpell
    {
        public DragonsBreath(CastingState castingState)
            : base("Dragon's Breath", false, false, true, true, 700, 0, 0, 20, MagicSchool.Fire, 680, 790, 0, 0.1357f)
        {
            base.Calculate(castingState);
            AoeDamageCap = 10100;
            CalculateDerivedStats(castingState);
        }
    }

    class Blizzard : BaseSpell
    {
        public Blizzard(CastingState castingState)
            : base("Blizzard", true, false, false, true, 1645, 0, 8, 0, MagicSchool.Frost, 1476, 1476, 0, 1.1429f)
        {
            base.Calculate(castingState);
            CritBonus = 1;
            CritRate = 0;
            HitRate = 1;
            AoeDamageCap = 28950;
            CalculateDerivedStats(castingState);
        }
    }

    // lightning capacitor
    class LightningBolt : BaseSpell
    {
        public LightningBolt(CastingState castingState)
            : base("Lightning Bolt", false, false, true, false, 0, 30, 0, 0, MagicSchool.Nature, 694, 806, 0, 0, 0, 0, 0, 0, false)
        {
            EffectProc = true;
            Calculate(castingState);
        }

        public override void Calculate(CastingState castingState)
        {
            base.Calculate(castingState);
            CritBonus = (1 + (1.5f * (1 + castingState.BaseStats.BonusSpellCritMultiplier) - 1)) * castingState.ResilienceCritDamageReduction;
            CalculateDerivedStats(castingState);
        }
    }

    // Shattered Sun Pendant of Acumen
    class ArcaneBolt : BaseSpell
    {
        public ArcaneBolt(CastingState castingState)
            : base("Arcane Bolt", false, false, true, false, 0, 50, 0, 0, MagicSchool.Arcane, 333, 367, 0, 0, 0, 0, 0, 0, false)
        {
            EffectProc = true;
            Calculate(castingState);
        }

        public override void Calculate(CastingState castingState)
        {
            base.Calculate(castingState);
            CritBonus = (1 + (1.5f * (1 + castingState.BaseStats.BonusSpellCritMultiplier) - 1)) * castingState.ResilienceCritDamageReduction;
            CalculateDerivedStats(castingState);
        }
    }
    #endregion

    class SpellCycle : Spell
    {
        public override string Sequence
        {
            get
            {
                if (sequence == null) sequence = string.Join("-", spellList.ToArray());
                return sequence;
            }
        }

        public float AverageDamage;
        public float AverageThreat;
        public float Cost;
        public float SpellCount = 0;

        private List<string> spellList = new List<string>();
        private FSRCalc fsr = new FSRCalc();

        public SpellCycle()
        {
            spellList = new List<string>();
            fsr = new FSRCalc();
        }

        public SpellCycle(int capacity)
        {
            spellList = new List<string>(capacity);
            fsr = new FSRCalc(capacity);
        }

        public void AddSpell(Spell spell, CastingState castingState)
        {
            fsr.AddSpell(spell.CastTime - castingState.Latency, castingState.Latency, spell.Channeled);
            HitProcs += spell.HitProcs;
            CastProcs += spell.CastProcs;
            AverageDamage += spell.DamagePerSecond * spell.CastTime;
            AverageThreat += spell.ThreatPerSecond * spell.CastTime;
            Cost += spell.CostPerSecond * spell.CastTime;
            AffectedByFlameCap = AffectedByFlameCap || spell.AffectedByFlameCap;
            spellList.Add(spell.Name);
            SpellCount++;
        }

        public void AddPause(float duration)
        {
            fsr.AddPause(duration);
            spellList.Add("Pause");
        }

        public void Calculate(CastingState castingState)
        {
            CastTime = fsr.Duration;

            CostPerSecond = Cost / CastTime;
            DamagePerSecond = AverageDamage / CastTime;
            ThreatPerSecond = AverageThreat / CastTime;

            float OO5SR = fsr.CalculateOO5SR(castingState.ClearcastingChance);

            ManaRegenPerSecond = castingState.ManaRegen5SR + OO5SR * (castingState.ManaRegen - castingState.ManaRegen5SR) + castingState.BaseStats.ManaRestorePerHit * HitProcs / CastTime + castingState.BaseStats.ManaRestorePerCast * CastProcs / CastTime;

            if (castingState.Mp5OnCastFor20Sec > 0)
            {
                float averageCastTime = fsr.Duration / SpellCount;
                float totalMana = castingState.Mp5OnCastFor20Sec / 5f / averageCastTime * 0.5f * (20 - averageCastTime / HitProcs / 2f) * (20 - averageCastTime / HitProcs / 2f);
                ManaRegenPerSecond += totalMana / 20f;
            }
        }
    }

    #region Spell Cycles

    class ABAMP : SpellCycle
    {
        public ABAMP(CastingState castingState) : base(3)
        {
            Name = "ABAMP";
            ABCycle = true;

            Spell AB = castingState.GetSpell(SpellId.ArcaneBlast10);
            Spell AM = castingState.GetSpell(SpellId.ArcaneMissiles);

            AddSpell(AB, castingState);
            AddSpell(AM, castingState);
            AddPause(8 - AM.CastTime - AB.CastTime + castingState.Latency);

            Calculate(castingState);
        }
    }

    class ABAM : Spell
    {
        public ABAM(CastingState castingState)
        {
            Name = "ABAM";
            ABCycle = true;

            Spell AB = castingState.GetSpell(castingState.CalculationOptions.WotLK ? SpellId.ArcaneBlast00 : SpellId.ArcaneBlast33);
            Spell AM = castingState.GetSpell(SpellId.ArcaneMissiles);
            Spell MBAM = castingState.GetSpell(SpellId.ArcaneMissilesMB);

            float MB = 0.03f * castingState.CalculationOptions.MissileBarrage;

            if (MB == 0.0 || !castingState.CalculationOptions.WotLK)
            {
                // if we don't have barrage then this degenerates to AB-AM
                SpellCycle chain1 = new SpellCycle(2);
                chain1.AddSpell(AB, castingState);
                chain1.AddSpell(AM, castingState);
                chain1.Calculate(castingState);

                CastTime = chain1.CastTime;
                CostPerSecond = chain1.CostPerSecond;
                DamagePerSecond = chain1.DamagePerSecond;
                ThreatPerSecond = chain1.ThreatPerSecond;
                ManaRegenPerSecond = chain1.ManaRegenPerSecond;
            }
            else
            {
                //AB-AM 0.85
                SpellCycle chain1 = new SpellCycle(2);
                chain1.AddSpell(AB, castingState);
                chain1.AddSpell(AM, castingState);
                chain1.Calculate(castingState);

                //AB-MBAM 0.15
                SpellCycle chain2 = new SpellCycle(2);
                chain2.AddSpell(AB, castingState);
                chain2.AddSpell(MBAM, castingState);
                chain2.Calculate(castingState);

                CastTime = (1 - MB) * chain1.CastTime + MB * chain2.CastTime;
                CostPerSecond = ((1 - MB) * chain1.CastTime * chain1.CostPerSecond + MB * chain2.CastTime * chain2.CostPerSecond) / CastTime;
                DamagePerSecond = ((1 - MB) * chain1.CastTime * chain1.DamagePerSecond + MB * chain2.CastTime * chain2.DamagePerSecond) / CastTime;
                ThreatPerSecond = ((1 - MB) * chain1.CastTime * chain1.ThreatPerSecond + MB * chain2.CastTime * chain2.ThreatPerSecond) / CastTime;
                ManaRegenPerSecond = ((1 - MB) * chain1.CastTime * chain1.ManaRegenPerSecond + MB * chain2.CastTime * chain2.ManaRegenPerSecond) / CastTime;
            }
        }
    }

    class ABMBAM : Spell
    {
        public ABMBAM(CastingState castingState)
        {
            Name = "ABMBAM";
            ABCycle = true;

            // main cycle is AB3 spam
            // on MB we change into ramp up mode

            // RAMP =
            // AB0-AB1-AB2           0.85*0.85*0.85 = k1
            // AB0-AB1-AB2-MBAM-RAMP 0.85*0.85*0.15 = k2
            // AB0-AB1-MBAM-RAMP     0.85*0.15      = k3
            // AB0-MBAM-RAMP         0.15           = k4

            // RAMP = k1 * (AB0+AB1+AB2) + k2 * (AB0+AB1+AB2+MBAM + RAMP) + k3 * (AB0+AB1+MBAM + RAMP) + k4 * (AB0+MBAM + RAMP)
            // RAMP * (1 - k2 - k3 - k4) = k1 * (AB0+AB1+AB2) + k2 * (AB0+AB1+AB2+MBAM) + k3 * (AB0+AB1+MBAM) + k4 * (AB0+MBAM)
            // RAMP = (AB0+AB1+AB2) + k2 / k1 * (AB0+AB1+AB2+MBAM) + k3 / k1 * (AB0+AB1+MBAM) + k4 / k1 * (AB0+MBAM)

            // AB3           0.85
            // AB3-MBAM-RAMP 0.15

            Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast00);
            Spell AB1 = castingState.GetSpell(SpellId.ArcaneBlast11);
            Spell AB2 = castingState.GetSpell(SpellId.ArcaneBlast22);
            Spell AB3 = castingState.GetSpell(SpellId.ArcaneBlast33);
            Spell MBAM = castingState.GetSpell(SpellId.ArcaneMissilesMB);

            float MB = 0.03f * castingState.CalculationOptions.MissileBarrage;

            if (MB == 0.0 || !castingState.CalculationOptions.WotLK)
            {
                // if we don't have barrage then this degenerates to AB
                CastTime = AB3.CastTime;
                CostPerSecond = AB3.CostPerSecond;
                DamagePerSecond = AB3.DamagePerSecond;
                ThreatPerSecond = AB3.ThreatPerSecond;
                ManaRegenPerSecond = AB3.ManaRegenPerSecond;
            }
            else
            {
                //AB3 0.85

                //AB3-MBAM-RAMP 0.15
                SpellCycle chain1 = new SpellCycle(2);
                chain1.AddSpell(AB3, castingState);
                chain1.AddSpell(MBAM, castingState);
                chain1.AddSpell(AB0, castingState);
                chain1.AddSpell(AB1, castingState);
                chain1.AddSpell(AB2, castingState);
                chain1.Calculate(castingState);

                SpellCycle chain3 = new SpellCycle(4);
                chain3.AddSpell(AB0, castingState);
                chain3.AddSpell(AB1, castingState);
                chain3.AddSpell(AB2, castingState);
                chain3.AddSpell(MBAM, castingState);
                chain3.Calculate(castingState);

                SpellCycle chain4 = new SpellCycle(3);
                chain4.AddSpell(AB0, castingState);
                chain4.AddSpell(AB1, castingState);
                chain4.AddSpell(MBAM, castingState);
                chain4.Calculate(castingState);

                SpellCycle chain5 = new SpellCycle(2);
                chain5.AddSpell(AB0, castingState);
                chain5.AddSpell(MBAM, castingState);
                chain5.Calculate(castingState);

                float MB3 = MB / (1 - MB);
                float MB4 = MB / (1 - MB) / (1 - MB);
                float MB5 = MB / (1 - MB) / (1 - MB) / (1 - MB);

                CastTime = (1 - MB) * AB3.CastTime + MB * (chain1.CastTime + MB3 * chain3.CastTime + MB4 * chain4.CastTime + MB5 * chain5.CastTime);
                CostPerSecond = ((1 - MB) * AB3.CostPerSecond * AB3.CastTime + MB * (chain1.CostPerSecond * chain1.CastTime + MB3 * chain3.CostPerSecond * chain3.CastTime + MB4 * chain4.CostPerSecond * chain4.CastTime + MB5 * chain5.CostPerSecond * chain5.CastTime)) / CastTime;
                DamagePerSecond = ((1 - MB) * AB3.DamagePerSecond * AB3.CastTime + MB * (chain1.DamagePerSecond * chain1.CastTime + MB3 * chain3.DamagePerSecond * chain3.CastTime + MB4 * chain4.DamagePerSecond * chain4.CastTime + MB5 * chain5.DamagePerSecond * chain5.CastTime)) / CastTime;
                ThreatPerSecond = ((1 - MB) * AB3.ThreatPerSecond * AB3.CastTime + MB * (chain1.ThreatPerSecond * chain1.CastTime + MB3 * chain3.ThreatPerSecond * chain3.CastTime + MB4 * chain4.ThreatPerSecond * chain4.CastTime + MB5 * chain5.ThreatPerSecond * chain5.CastTime)) / CastTime;
                ManaRegenPerSecond = ((1 - MB) * AB3.ManaRegenPerSecond * AB3.CastTime + MB * (chain1.ManaRegenPerSecond * chain1.CastTime + MB3 * chain3.ManaRegenPerSecond * chain3.CastTime + MB4 * chain4.ManaRegenPerSecond * chain4.CastTime + MB5 * chain5.ManaRegenPerSecond * chain5.CastTime)) / CastTime;
            }
        }
    }

    class AB3AMSc : SpellCycle
    {
        public AB3AMSc(CastingState castingState) : base(12)
        {
            Name = "AB3AMSc";
            ABCycle = true;

            Spell AB30 = castingState.GetSpell(SpellId.ArcaneBlast30);
            Spell AB01 = castingState.GetSpell(SpellId.ArcaneBlast01);
            Spell AB12 = castingState.GetSpell(SpellId.ArcaneBlast12);
            Spell AM = castingState.GetSpell(SpellId.ArcaneMissiles);
            Spell Sc = castingState.GetSpell(SpellId.Scorch);

            AddSpell(AB30, castingState);
            AddSpell(AB01, castingState);
            AddSpell(AB12, castingState);
            AddSpell(AM, castingState);
            float gap = 8 - AM.CastTime;
            while (gap - AB30.CastTime >= Sc.CastTime)
            {
                AddSpell(Sc, castingState);
                gap -= Sc.CastTime;
            }
            if (AB30.CastTime < gap) AddPause(gap - AB30.CastTime + castingState.Latency);

            Calculate(castingState);
        }
    }

    class ABAM3Sc : SpellCycle
    {
        public ABAM3Sc(CastingState castingState) : base(14)
        {
            Name = "ABAM3Sc";
            ABCycle = true;

            Spell AB30 = castingState.GetSpell(SpellId.ArcaneBlast30);
            Spell AB11 = castingState.GetSpell(SpellId.ArcaneBlast11);
            Spell AB22 = castingState.GetSpell(SpellId.ArcaneBlast22);
            Spell AM = castingState.GetSpell(SpellId.ArcaneMissiles);
            Spell Sc = castingState.GetSpell(SpellId.Scorch);

            AddSpell(AB30, castingState);
            AddSpell(AM, castingState);
            AddSpell(AB11, castingState);
            AddSpell(AM, castingState);
            AddSpell(AB22, castingState);
            AddSpell(AM, castingState);
            float gap = 8 - AM.CastTime;
            while (gap - AB30.CastTime >= Sc.CastTime)
            {
                AddSpell(Sc, castingState);
                gap -= Sc.CastTime;
            }
            if (AB30.CastTime < gap) AddPause(gap - AB30.CastTime + castingState.Latency);

            Calculate(castingState);
        }
    }

    class ABAM3Sc2 : SpellCycle
    {
        public ABAM3Sc2(CastingState castingState) : base(14)
        {
            Name = "ABAM3Sc2";
            ABCycle = true;

            Spell AB30 = castingState.GetSpell(SpellId.ArcaneBlast30);
            Spell AB11 = castingState.GetSpell(SpellId.ArcaneBlast11);
            Spell AB22 = castingState.GetSpell(SpellId.ArcaneBlast22);
            Spell AM = castingState.GetSpell(SpellId.ArcaneMissiles);
            Spell Sc = castingState.GetSpell(SpellId.Scorch);

            AddSpell(AB30, castingState);
            AddSpell(AM, castingState);
            AddSpell(AB11, castingState);
            AddSpell(AM, castingState);
            AddSpell(AB22, castingState);
            AddSpell(AM, castingState);
            float gap = 8 - AM.CastTime;
            while (gap >= AB30.CastTime)
            {
                AddSpell(Sc, castingState);
                gap -= Sc.CastTime;
            }
            if (AB30.CastTime < gap) AddPause(gap - AB30.CastTime + castingState.Latency);

            Calculate(castingState);
        }
    }

    class ABAM3FrB : SpellCycle
    {
        public ABAM3FrB(CastingState castingState) : base(14)
        {
            Name = "ABAM3FrB";
            ABCycle = true;

            Spell AB30 = castingState.GetSpell(SpellId.ArcaneBlast30);
            Spell AB11 = castingState.GetSpell(SpellId.ArcaneBlast11);
            Spell AB22 = castingState.GetSpell(SpellId.ArcaneBlast22);
            Spell AM = castingState.GetSpell(SpellId.ArcaneMissiles);
            Spell FrB = castingState.GetSpell(SpellId.Frostbolt);

            AddSpell(AB30, castingState);
            AddSpell(AM, castingState);
            AddSpell(AB11, castingState);
            AddSpell(AM, castingState);
            AddSpell(AB22, castingState);
            AddSpell(AM, castingState);
            float gap = 8 - AM.CastTime;
            while (gap - AB30.CastTime >= FrB.CastTime)
            {
                AddSpell(FrB, castingState);
                gap -= FrB.CastTime;
            }
            if (AB30.CastTime < gap) AddPause(gap - AB30.CastTime + castingState.Latency);

            Calculate(castingState);
        }
    }

    class ABAM3FrB2 : SpellCycle
    {
        public ABAM3FrB2(CastingState castingState) : base(14)
        {
            Name = "ABAM3FrB2";
            ABCycle = true;

            Spell AB30 = castingState.GetSpell(SpellId.ArcaneBlast30);
            Spell AB11 = castingState.GetSpell(SpellId.ArcaneBlast11);
            Spell AB22 = castingState.GetSpell(SpellId.ArcaneBlast22);
            Spell AM = castingState.GetSpell(SpellId.ArcaneMissiles);
            Spell FrB = castingState.GetSpell(SpellId.Frostbolt);

            AddSpell(AB30, castingState);
            AddSpell(AM, castingState);
            AddSpell(AB11, castingState);
            AddSpell(AM, castingState);
            AddSpell(AB22, castingState);
            AddSpell(AM, castingState);
            float gap = 8 - AM.CastTime;
            while (gap >= FrB.CastTime)
            {
                AddSpell(FrB, castingState);
                gap -= FrB.CastTime;
            }
            if (AB30.CastTime < gap) AddPause(gap - AB30.CastTime + castingState.Latency);

            Calculate(castingState);
        }
    }

    class AB3FrB : SpellCycle
    {
        public AB3FrB(CastingState castingState) : base(11)
        {
            Name = "AB3FrB";
            ABCycle = true;

            Spell AB30 = castingState.GetSpell(SpellId.ArcaneBlast30);
            Spell AB01 = castingState.GetSpell(SpellId.ArcaneBlast01);
            Spell AB12 = castingState.GetSpell(SpellId.ArcaneBlast12);
            Spell FrB = castingState.GetSpell(SpellId.Frostbolt);

            AddSpell(AB30, castingState);
            AddSpell(AB01, castingState);
            AddSpell(AB12, castingState);
            float gap = 8;
            while (gap - AB30.CastTime >= FrB.CastTime)
            {
                AddSpell(FrB, castingState);
                gap -= FrB.CastTime;
            }
            if (AB30.CastTime < gap) AddPause(gap - AB30.CastTime + castingState.Latency);

            Calculate(castingState);
        }
    }

    class ABFrB : SpellCycle
    {
        public ABFrB(CastingState castingState)
            : base(13)
        {
            Name = "ABFrB";
            ABCycle = true;

            Spell AB10 = castingState.GetSpell(SpellId.ArcaneBlast10);
            Spell FrB = castingState.GetSpell(SpellId.Frostbolt);

            AddSpell(AB10, castingState);
            float gap = 8;
            while (gap - AB10.CastTime >= FrB.CastTime)
            {
                AddSpell(FrB, castingState);
                gap -= FrB.CastTime;
            }
            if (AB10.CastTime < gap) AddPause(gap - AB10.CastTime + castingState.Latency);

            Calculate(castingState);
        }
    }

    class ABFrB3FrB : SpellCycle
    {
        public ABFrB3FrB(CastingState castingState) : base(13)
        {
            Name = "ABFrB3FrB";
            ABCycle = true;

            Spell AB30 = castingState.GetSpell(SpellId.ArcaneBlast30);
            Spell AB11 = castingState.GetSpell(SpellId.ArcaneBlast11);
            Spell AB22 = castingState.GetSpell(SpellId.ArcaneBlast22);
            Spell FrB = castingState.GetSpell(SpellId.Frostbolt);

            AddSpell(AB30, castingState);
            AddSpell(FrB, castingState);
            AddSpell(AB11, castingState);
            AddSpell(FrB, castingState);
            AddSpell(AB22, castingState);
            float gap = 8;
            while (gap - AB30.CastTime >= FrB.CastTime)
            {
                AddSpell(FrB, castingState);
                gap -= FrB.CastTime;
            }
            if (AB30.CastTime < gap) AddPause(gap - AB30.CastTime + castingState.Latency);

            Calculate(castingState);
        }
    }

    class ABFrB3FrB2 : SpellCycle
    {
        public ABFrB3FrB2(CastingState castingState) : base(13)
        {
            Name = "ABFrB3FrB2";
            ABCycle = true;

            Spell AB30 = castingState.GetSpell(SpellId.ArcaneBlast30);
            Spell AB11 = castingState.GetSpell(SpellId.ArcaneBlast11);
            Spell AB22 = castingState.GetSpell(SpellId.ArcaneBlast22);
            Spell FrB = castingState.GetSpell(SpellId.Frostbolt);

            AddSpell(AB30, castingState);
            AddSpell(FrB, castingState);
            AddSpell(AB11, castingState);
            AddSpell(FrB, castingState);
            AddSpell(AB22, castingState);
            float gap = 8;
            while (gap >= FrB.CastTime)
            {
                AddSpell(FrB, castingState);
                gap -= FrB.CastTime;
            }
            if (AB30.CastTime < gap) AddPause(gap - AB30.CastTime + castingState.Latency);

            Calculate(castingState);
        }
    }

    class ABFrB3FrBSc : SpellCycle
    {
        public ABFrB3FrBSc(CastingState castingState) : base(13)
        {
            Name = "ABFrB3FrBSc";
            ABCycle = true;

            Spell AB30 = castingState.GetSpell(SpellId.ArcaneBlast30);
            Spell AB11 = castingState.GetSpell(SpellId.ArcaneBlast11);
            Spell AB22 = castingState.GetSpell(SpellId.ArcaneBlast22);
            Spell FrB = castingState.GetSpell(SpellId.Frostbolt);
            Spell Sc = castingState.GetSpell(SpellId.Scorch);

            AddSpell(AB30, castingState);
            AddSpell(FrB, castingState);
            AddSpell(AB11, castingState);
            AddSpell(FrB, castingState);
            AddSpell(AB22, castingState);
            float gap = 8;
            while (gap >= FrB.CastTime)
            {
                AddSpell(FrB, castingState);
                gap -= FrB.CastTime;
            }
            while (gap >= Sc.CastTime)
            {
                AddSpell(Sc, castingState);
                gap -= Sc.CastTime;
            }
            if (AB30.CastTime < gap) AddPause(gap - AB30.CastTime + castingState.Latency);

            Calculate(castingState);
        }
    }

    class ABFB3FBSc : SpellCycle
    {
        public ABFB3FBSc(CastingState castingState) : base(13)
        {
            Name = "ABFB3FBSc";
            ABCycle = true;

            Spell AB30 = castingState.GetSpell(SpellId.ArcaneBlast30);
            Spell AB11 = castingState.GetSpell(SpellId.ArcaneBlast11);
            Spell AB22 = castingState.GetSpell(SpellId.ArcaneBlast22);
            Spell FB = castingState.GetSpell(SpellId.Fireball);
            Spell Sc = castingState.GetSpell(SpellId.Scorch);

            AddSpell(AB30, castingState);
            AddSpell(FB, castingState);
            AddSpell(AB11, castingState);
            AddSpell(FB, castingState);
            AddSpell(AB22, castingState);
            float gap = 8;
            while (gap >= FB.CastTime)
            {
                AddSpell(FB, castingState);
                gap -= FB.CastTime;
            }
            while (gap >= Sc.CastTime)
            {
                AddSpell(Sc, castingState);
                gap -= Sc.CastTime;
            }
            if (AB30.CastTime < gap) AddPause(gap - AB30.CastTime + castingState.Latency);

            Calculate(castingState);
        }
    }

    class AB3Sc : SpellCycle
    {
        public AB3Sc(CastingState castingState) : base(11)
        {
            Name = "AB3Sc";
            ABCycle = true;

            Spell AB30 = castingState.GetSpell(SpellId.ArcaneBlast30);
            Spell AB01 = castingState.GetSpell(SpellId.ArcaneBlast01);
            Spell AB12 = castingState.GetSpell(SpellId.ArcaneBlast12);
            Spell Sc = castingState.GetSpell(SpellId.Scorch);

            AddSpell(AB30, castingState);
            AddSpell(AB01, castingState);
            AddSpell(AB12, castingState);
            float gap = 8;
            while (gap >= Sc.CastTime)
            {
                AddSpell(Sc, castingState);
                gap -= Sc.CastTime;
            }
            if (AB30.CastTime < gap) AddPause(gap - AB30.CastTime + castingState.Latency);

            Calculate(castingState);
        }
    }

    class FireballScorch : SpellCycle
    {
        public FireballScorch(CastingState castingState) : base(33)
        {
            Name = "FireballScorch";

            Spell FB = castingState.GetSpell(SpellId.Fireball);
            Spell Sc = castingState.GetSpell(SpellId.Scorch);

            if (castingState.CalculationOptions.ImprovedScorch == 0)
            {
                // in this case just Fireball, scorch debuff won't be applied
                AddSpell(FB, castingState);
                Calculate(castingState);
            }
            else
            {
                int averageScorchesNeeded = (int)Math.Ceiling(3f / (float)castingState.CalculationOptions.ImprovedScorch);
                double timeOnScorch = 30;
                int fbCount = 0;

                while (timeOnScorch > FB.CastTime + (averageScorchesNeeded + 1) * Sc.CastTime) // one extra scorch gap to account for possible resist
                {
                    AddSpell(FB, castingState);
                    fbCount++;
                    timeOnScorch -= FB.CastTime;
                }
                for (int i = 0; i < averageScorchesNeeded; i++)
                {
                    AddSpell(Sc, castingState);
                }

                Calculate(castingState);

                sequence = string.Format("{0}x Fireball : {1}x Scorch", fbCount, averageScorchesNeeded);
            }
        }
    }

    class FireballFireBlast : SpellCycle
    {
        public FireballFireBlast(CastingState castingState)
            : base(33)
        {
            Name = "FireballFireBlast";

            Spell FB = castingState.GetSpell(SpellId.Fireball);
            BaseSpell Blast = (BaseSpell)castingState.GetSpell(SpellId.FireBlast);
            Spell Sc = castingState.GetSpell(SpellId.Scorch);

            if (castingState.CalculationOptions.ImprovedScorch == 0)
            {
                // in this case just Fireball/Fire Blast, scorch debuff won't be applied
                float blastCooldown = Blast.Cooldown - Blast.CastTime;
                AddSpell(Blast, castingState);
                while (blastCooldown > 0)
                {
                    AddSpell(FB, castingState);
                    blastCooldown -= FB.CastTime;
                }
                Calculate(castingState);
            }
            else
            {
                int averageScorchesNeeded = (int)Math.Ceiling(3f / (float)castingState.CalculationOptions.ImprovedScorch);
                double timeOnScorch = 30;
                int fbCount = 0;
                float blastCooldown = 0;

                do
                {
                    float expectedTimeWithBlast = Blast.CastTime + (int)((timeOnScorch - (blastCooldown > 0f ? blastCooldown : 0f) - Blast.CastTime - (averageScorchesNeeded + 1) * Sc.CastTime) / FB.CastTime) * FB.CastTime + averageScorchesNeeded * Sc.CastTime;
                    if (expectedTimeWithBlast > Blast.Cooldown && (blastCooldown <= 0 || (Blast.DamagePerSecond * Blast.CastTime / (Blast.CastTime + blastCooldown) > FB.DamagePerSecond)))
                    {
                        if (blastCooldown > 0)
                        {
                            AddPause(blastCooldown);
                            timeOnScorch -= blastCooldown;
                        }
                        AddSpell(Blast, castingState);
                        fbCount++;
                        timeOnScorch -= Blast.CastTime;
                        blastCooldown = Blast.Cooldown - Blast.CastTime;
                    }
                    else if (timeOnScorch > FB.CastTime + (averageScorchesNeeded + 1) * Sc.CastTime) // one extra scorch gap to account for possible resist
                    {
                        AddSpell(FB, castingState);
                        fbCount++;
                        timeOnScorch -= FB.CastTime;
                        blastCooldown -= FB.CastTime;
                    }
                    else
                    {
                        break;
                    }
                } while (true);
                for (int i = 0; i < averageScorchesNeeded; i++)
                {
                    AddSpell(Sc, castingState);
                    blastCooldown -= Sc.CastTime;
                }
                if (blastCooldown > 0) AddPause(blastCooldown);

                Calculate(castingState);
            }
        }
    }

    class ABAM3ScCCAM : Spell
    {
        public ABAM3ScCCAM(CastingState castingState)
        {
            Name = "ABAM3ScCC";
            ABCycle = true;

            //AMCC-AB0                       0.1
            //AM?0-AB1-AMCC-AB0              0.9*0.1
            //AM?0-AB1-AM?0-AB2-AMCC-AB0     0.9*0.9*0.1
            //AM?0-AB1-AM?0-AB2-AM?0-S-AB3?  0.9*0.9*0.9

            //TIME = 0.1*[AMCC+AB0] + 0.9*0.1*[AM+AMCC+AB0+AB1] + 0.9*0.9*0.1*[2*AM+AMCC+AB0+AB1+AB2] + 0.9*0.9*0.9*[3*AM+AB1+AB2+AB3?]
            //     = [0.1 + 0.9*0.1 + 0.9*0.9*0.1]*[AMCC+AB0] + [0.9*0.1 + 2*0.9*0.9*0.1 + 3*0.9*0.9*0.9]*AM + 0.9*AB1 + 0.9*0.9*AB2 + 0.9*0.9*0.9*[S+AB3?]
            //     = 0.271*[AMCC+AB0] + 2.439*AM + 0.9*AB1 + 0.81*AB2 + 0.729*[S+AB3?]
            //DAMAGE = 0.271*[AMCC+AB0] + 2.439*AM + 0.9*AB1 + 0.81*AB2 + 0.729*[S+AB3?]

            Spell AMc0 = castingState.GetSpell(SpellId.ArcaneMissilesNoProc);
            Spell AMCC = castingState.GetSpell(SpellId.ArcaneMissilesCC);
            Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast00NoCC);
            Spell AB1 = castingState.GetSpell(SpellId.ArcaneBlast11NoCC);
            Spell AB2 = castingState.GetSpell(SpellId.ArcaneBlast22NoCC);
            Spell Sc0 = castingState.GetSpell(SpellId.ScorchNoCC);

            Spell AB3 = castingState.GetSpell(SpellId.ArcaneBlast30);
            Spell Sc = castingState.GetSpell(SpellId.Scorch);

            float CC = 0.02f * castingState.CalculationOptions.ArcaneConcentration;

            //AMCC-AB0                       0.1
            SpellCycle chain1 = new SpellCycle(2);
            chain1.AddSpell(AMCC, castingState);
            chain1.AddSpell(AB0, castingState);
            chain1.Calculate(castingState);

            //AM?0-AB1-AMCC-AB0              0.9*0.1
            SpellCycle chain2 = new SpellCycle(4);
            chain2.AddSpell(AMc0, castingState);
            chain2.AddSpell(AB1, castingState);
            chain2.AddSpell(AMCC, castingState);
            chain2.AddSpell(AB0, castingState);
            chain2.Calculate(castingState);

            //AM?0-AB1-AM?0-AB2-AMCC-AB0     0.9*0.9*0.1
            SpellCycle chain3 = new SpellCycle(6);
            chain3.AddSpell(AMc0, castingState);
            chain3.AddSpell(AB1, castingState);
            chain3.AddSpell(AMc0, castingState);
            chain3.AddSpell(AB2, castingState);
            chain3.AddSpell(AMCC, castingState);
            chain3.AddSpell(AB0, castingState);
            chain3.Calculate(castingState);

            //AM?0-AB1-AM?0-AB2-AM?0-S-AB3?  0.9*0.9*0.9
            SpellCycle chain4 = new SpellCycle(13);
            chain4.AddSpell(AMc0, castingState);
            chain4.AddSpell(AB1, castingState);
            chain4.AddSpell(AMc0, castingState);
            chain4.AddSpell(AB2, castingState);
            chain4.AddSpell(AMc0, castingState);
            chain4.AddSpell(Sc0, castingState);
            float gap = 8 - AMc0.CastTime - Sc0.CastTime;
            while (gap - AB3.CastTime >= Sc.CastTime)
            {
                chain4.AddSpell(Sc, castingState);
                gap -= Sc.CastTime;
            }
            if (AB3.CastTime < gap) chain4.AddPause(gap - AB3.CastTime + castingState.Latency);
            chain4.AddSpell(AB3, castingState);
            chain4.Calculate(castingState);

            CastTime = CC * chain1.CastTime + CC * (1 - CC) * chain2.CastTime + CC * (1 - CC) * (1 - CC) * chain3.CastTime + (1 - CC) * (1 - CC) * (1 - CC) * chain4.CastTime;
            CostPerSecond = (CC * chain1.CastTime * chain1.CostPerSecond + CC * (1 - CC) * chain2.CastTime * chain2.CostPerSecond + CC * (1 - CC) * (1 - CC) * chain3.CastTime * chain3.CostPerSecond + (1 - CC) * (1 - CC) * (1 - CC) * chain4.CastTime * chain4.CostPerSecond) / CastTime;
            DamagePerSecond = (CC * chain1.CastTime * chain1.DamagePerSecond + CC * (1 - CC) * chain2.CastTime * chain2.DamagePerSecond + CC * (1 - CC) * (1 - CC) * chain3.CastTime * chain3.DamagePerSecond + (1 - CC) * (1 - CC) * (1 - CC) * chain4.CastTime * chain4.DamagePerSecond) / CastTime;
            ThreatPerSecond = (CC * chain1.CastTime * chain1.ThreatPerSecond + CC * (1 - CC) * chain2.CastTime * chain2.ThreatPerSecond + CC * (1 - CC) * (1 - CC) * chain3.CastTime * chain3.ThreatPerSecond + (1 - CC) * (1 - CC) * (1 - CC) * chain4.CastTime * chain4.ThreatPerSecond) / CastTime;
            ManaRegenPerSecond = (CC * chain1.CastTime * chain1.ManaRegenPerSecond + CC * (1 - CC) * chain2.CastTime * chain2.ManaRegenPerSecond + CC * (1 - CC) * (1 - CC) * chain3.CastTime * chain3.ManaRegenPerSecond + (1 - CC) * (1 - CC) * (1 - CC) * chain4.CastTime * chain4.ManaRegenPerSecond) / CastTime;

            commonChain = chain4;
        }

        private SpellCycle commonChain;

        public override string Sequence
        {
            get
            {
                return commonChain.Sequence;
            }
        }
    }

    class ABAM3Sc2CCAM : Spell
    {
        public ABAM3Sc2CCAM(CastingState castingState)
        {
            Name = "ABAM3Sc2CC";
            ABCycle = true;

            //AMCC-AB0                       0.1
            //AM?0-AB1-AMCC-AB0              0.9*0.1
            //AM?0-AB1-AM?0-AB2-AMCC-AB0     0.9*0.9*0.1
            //AM?0-AB1-AM?0-AB2-AM?0-S-AB3?  0.9*0.9*0.9

            //TIME = 0.1*[AMCC+AB0] + 0.9*0.1*[AM+AMCC+AB0+AB1] + 0.9*0.9*0.1*[2*AM+AMCC+AB0+AB1+AB2] + 0.9*0.9*0.9*[3*AM+AB1+AB2+AB3?]
            //     = [0.1 + 0.9*0.1 + 0.9*0.9*0.1]*[AMCC+AB0] + [0.9*0.1 + 2*0.9*0.9*0.1 + 3*0.9*0.9*0.9]*AM + 0.9*AB1 + 0.9*0.9*AB2 + 0.9*0.9*0.9*[S+AB3?]
            //     = 0.271*[AMCC+AB0] + 2.439*AM + 0.9*AB1 + 0.81*AB2 + 0.729*[S+AB3?]
            //DAMAGE = 0.271*[AMCC+AB0] + 2.439*AM + 0.9*AB1 + 0.81*AB2 + 0.729*[S+AB3?]

            Spell AMc0 = castingState.GetSpell(SpellId.ArcaneMissilesNoProc);
            Spell AMCC = castingState.GetSpell(SpellId.ArcaneMissilesCC);
            Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast00NoCC);
            Spell AB1 = castingState.GetSpell(SpellId.ArcaneBlast11NoCC);
            Spell AB2 = castingState.GetSpell(SpellId.ArcaneBlast22NoCC);
            Spell Sc0 = castingState.GetSpell(SpellId.ScorchNoCC);

            Spell AB3 = castingState.GetSpell(SpellId.ArcaneBlast30);
            Spell Sc = castingState.GetSpell(SpellId.Scorch);

            float CC = 0.02f * castingState.CalculationOptions.ArcaneConcentration;

            //AMCC-AB0                       0.1
            SpellCycle chain1 = new SpellCycle(2);
            chain1.AddSpell(AMCC, castingState);
            chain1.AddSpell(AB0, castingState);
            chain1.Calculate(castingState);

            //AM?0-AB1-AMCC-AB0              0.9*0.1
            SpellCycle chain2 = new SpellCycle(4);
            chain2.AddSpell(AMc0, castingState);
            chain2.AddSpell(AB1, castingState);
            chain2.AddSpell(AMCC, castingState);
            chain2.AddSpell(AB0, castingState);
            chain2.Calculate(castingState);

            //AM?0-AB1-AM?0-AB2-AMCC-AB0     0.9*0.9*0.1
            SpellCycle chain3 = new SpellCycle(13);
            chain3.AddSpell(AMc0, castingState);
            chain3.AddSpell(AB1, castingState);
            chain3.AddSpell(AMc0, castingState);
            chain3.AddSpell(AB2, castingState);
            chain3.AddSpell(AMCC, castingState);
            chain3.AddSpell(AB0, castingState);
            chain3.Calculate(castingState);

            //AM?0-AB1-AM?0-AB2-AM?0-S-AB3?  0.9*0.9*0.9
            SpellCycle chain4 = new SpellCycle();
            chain4.AddSpell(AMc0, castingState);
            chain4.AddSpell(AB1, castingState);
            chain4.AddSpell(AMc0, castingState);
            chain4.AddSpell(AB2, castingState);
            chain4.AddSpell(AMc0, castingState);
            chain4.AddSpell(Sc0, castingState);
            float gap = 8 - AMc0.CastTime - Sc0.CastTime;
            while (gap >= Sc.CastTime)
            {
                chain4.AddSpell(Sc, castingState);
                gap -= Sc.CastTime;
            }
            if (AB3.CastTime < gap) chain4.AddPause(gap - AB3.CastTime + castingState.Latency);
            chain4.AddSpell(AB3, castingState);
            chain4.Calculate(castingState);

            CastTime = CC * chain1.CastTime + CC * (1 - CC) * chain2.CastTime + CC * (1 - CC) * (1 - CC) * chain3.CastTime + (1 - CC) * (1 - CC) * (1 - CC) * chain4.CastTime;
            CostPerSecond = (CC * chain1.CastTime * chain1.CostPerSecond + CC * (1 - CC) * chain2.CastTime * chain2.CostPerSecond + CC * (1 - CC) * (1 - CC) * chain3.CastTime * chain3.CostPerSecond + (1 - CC) * (1 - CC) * (1 - CC) * chain4.CastTime * chain4.CostPerSecond) / CastTime;
            DamagePerSecond = (CC * chain1.CastTime * chain1.DamagePerSecond + CC * (1 - CC) * chain2.CastTime * chain2.DamagePerSecond + CC * (1 - CC) * (1 - CC) * chain3.CastTime * chain3.DamagePerSecond + (1 - CC) * (1 - CC) * (1 - CC) * chain4.CastTime * chain4.DamagePerSecond) / CastTime;
            ThreatPerSecond = (CC * chain1.CastTime * chain1.ThreatPerSecond + CC * (1 - CC) * chain2.CastTime * chain2.ThreatPerSecond + CC * (1 - CC) * (1 - CC) * chain3.CastTime * chain3.ThreatPerSecond + (1 - CC) * (1 - CC) * (1 - CC) * chain4.CastTime * chain4.ThreatPerSecond) / CastTime;
            ManaRegenPerSecond = (CC * chain1.CastTime * chain1.ManaRegenPerSecond + CC * (1 - CC) * chain2.CastTime * chain2.ManaRegenPerSecond + CC * (1 - CC) * (1 - CC) * chain3.CastTime * chain3.ManaRegenPerSecond + (1 - CC) * (1 - CC) * (1 - CC) * chain4.CastTime * chain4.ManaRegenPerSecond) / CastTime;

            commonChain = chain4;
        }

        private SpellCycle commonChain;

        public override string Sequence
        {
            get
            {
                return commonChain.Sequence;
            }
        }
    }

    class ABAM3FrBCCAM : Spell
    {
        public ABAM3FrBCCAM(CastingState castingState)
        {
            Name = "ABAM3FrBCC";
            ABCycle = true;

            //AMCC-AB0                       0.1
            //AM?0-AB1-AMCC-AB0              0.9*0.1
            //AM?0-AB1-AM?0-AB2-AMCC-AB0     0.9*0.9*0.1
            //AM?0-AB1-AM?0-AB2-AM?0-S-AB3?  0.9*0.9*0.9

            //TIME = 0.1*[AMCC+AB0] + 0.9*0.1*[AM+AMCC+AB0+AB1] + 0.9*0.9*0.1*[2*AM+AMCC+AB0+AB1+AB2] + 0.9*0.9*0.9*[3*AM+AB1+AB2+AB3?]
            //     = [0.1 + 0.9*0.1 + 0.9*0.9*0.1]*[AMCC+AB0] + [0.9*0.1 + 2*0.9*0.9*0.1 + 3*0.9*0.9*0.9]*AM + 0.9*AB1 + 0.9*0.9*AB2 + 0.9*0.9*0.9*[S+AB3?]
            //     = 0.271*[AMCC+AB0] + 2.439*AM + 0.9*AB1 + 0.81*AB2 + 0.729*[S+AB3?]
            //DAMAGE = 0.271*[AMCC+AB0] + 2.439*AM + 0.9*AB1 + 0.81*AB2 + 0.729*[S+AB3?]

            Spell AMc0 = castingState.GetSpell(SpellId.ArcaneMissilesNoProc);
            Spell AMCC = castingState.GetSpell(SpellId.ArcaneMissilesCC);
            Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast00NoCC);
            Spell AB1 = castingState.GetSpell(SpellId.ArcaneBlast11NoCC);
            Spell AB2 = castingState.GetSpell(SpellId.ArcaneBlast22NoCC);
            Spell FrB0 = castingState.GetSpell(SpellId.FrostboltNoCC);

            Spell AB3 = castingState.GetSpell(SpellId.ArcaneBlast30);
            Spell FrB = castingState.GetSpell(SpellId.Frostbolt);

            float CC = 0.02f * castingState.CalculationOptions.ArcaneConcentration;

            //AMCC-AB0                       0.1
            SpellCycle chain1 = new SpellCycle(2);
            chain1.AddSpell(AMCC, castingState);
            chain1.AddSpell(AB0, castingState);
            chain1.Calculate(castingState);

            //AM?0-AB1-AMCC-AB0              0.9*0.1
            SpellCycle chain2 = new SpellCycle(4);
            chain2.AddSpell(AMc0, castingState);
            chain2.AddSpell(AB1, castingState);
            chain2.AddSpell(AMCC, castingState);
            chain2.AddSpell(AB0, castingState);
            chain2.Calculate(castingState);

            //AM?0-AB1-AM?0-AB2-AMCC-AB0     0.9*0.9*0.1
            SpellCycle chain3 = new SpellCycle(6);
            chain3.AddSpell(AMc0, castingState);
            chain3.AddSpell(AB1, castingState);
            chain3.AddSpell(AMc0, castingState);
            chain3.AddSpell(AB2, castingState);
            chain3.AddSpell(AMCC, castingState);
            chain3.AddSpell(AB0, castingState);
            chain3.Calculate(castingState);

            //AM?0-AB1-AM?0-AB2-AM?0-S-AB3?  0.9*0.9*0.9
            SpellCycle chain4 = new SpellCycle(13);
            chain4.AddSpell(AMc0, castingState);
            chain4.AddSpell(AB1, castingState);
            chain4.AddSpell(AMc0, castingState);
            chain4.AddSpell(AB2, castingState);
            chain4.AddSpell(AMc0, castingState);
            chain4.AddSpell(FrB0, castingState);
            float gap = 8 - AMc0.CastTime - FrB0.CastTime;
            while (gap - AB3.CastTime >= FrB.CastTime)
            {
                chain4.AddSpell(FrB, castingState);
                gap -= FrB.CastTime;
            }
            if (AB3.CastTime < gap) chain4.AddPause(gap - AB3.CastTime + castingState.Latency);
            chain4.AddSpell(AB3, castingState);
            chain4.Calculate(castingState);

            CastTime = CC * chain1.CastTime + CC * (1 - CC) * chain2.CastTime + CC * (1 - CC) * (1 - CC) * chain3.CastTime + (1 - CC) * (1 - CC) * (1 - CC) * chain4.CastTime;
            CostPerSecond = (CC * chain1.CastTime * chain1.CostPerSecond + CC * (1 - CC) * chain2.CastTime * chain2.CostPerSecond + CC * (1 - CC) * (1 - CC) * chain3.CastTime * chain3.CostPerSecond + (1 - CC) * (1 - CC) * (1 - CC) * chain4.CastTime * chain4.CostPerSecond) / CastTime;
            DamagePerSecond = (CC * chain1.CastTime * chain1.DamagePerSecond + CC * (1 - CC) * chain2.CastTime * chain2.DamagePerSecond + CC * (1 - CC) * (1 - CC) * chain3.CastTime * chain3.DamagePerSecond + (1 - CC) * (1 - CC) * (1 - CC) * chain4.CastTime * chain4.DamagePerSecond) / CastTime;
            ThreatPerSecond = (CC * chain1.CastTime * chain1.ThreatPerSecond + CC * (1 - CC) * chain2.CastTime * chain2.ThreatPerSecond + CC * (1 - CC) * (1 - CC) * chain3.CastTime * chain3.ThreatPerSecond + (1 - CC) * (1 - CC) * (1 - CC) * chain4.CastTime * chain4.ThreatPerSecond) / CastTime;
            ManaRegenPerSecond = (CC * chain1.CastTime * chain1.ManaRegenPerSecond + CC * (1 - CC) * chain2.CastTime * chain2.ManaRegenPerSecond + CC * (1 - CC) * (1 - CC) * chain3.CastTime * chain3.ManaRegenPerSecond + (1 - CC) * (1 - CC) * (1 - CC) * chain4.CastTime * chain4.ManaRegenPerSecond) / CastTime;

            commonChain = chain4;
        }

        private SpellCycle commonChain;

        public override string Sequence
        {
            get
            {
                return commonChain.Sequence;
            }
        }
    }

    class ABAM3FrBCCAMFail : Spell
    {
        public ABAM3FrBCCAMFail(CastingState castingState)
        {
            Name = "ABAM3FrBCCFail";
            ABCycle = true;

            //AMCC-AB0                       0.1
            //AM?0-AB1-AMCC-AB0              0.9*0.1
            //AM?0-AB1-AM?0-AB2-AMCC-AB0     0.9*0.9*0.1
            //AM?0-AB1-AM?0-AB2-AM?0-S-AB3?  0.9*0.9*0.9

            //TIME = 0.1*[AMCC+AB0] + 0.9*0.1*[AM+AMCC+AB0+AB1] + 0.9*0.9*0.1*[2*AM+AMCC+AB0+AB1+AB2] + 0.9*0.9*0.9*[3*AM+AB1+AB2+AB3?]
            //     = [0.1 + 0.9*0.1 + 0.9*0.9*0.1]*[AMCC+AB0] + [0.9*0.1 + 2*0.9*0.9*0.1 + 3*0.9*0.9*0.9]*AM + 0.9*AB1 + 0.9*0.9*AB2 + 0.9*0.9*0.9*[S+AB3?]
            //     = 0.271*[AMCC+AB0] + 2.439*AM + 0.9*AB1 + 0.81*AB2 + 0.729*[S+AB3?]
            //DAMAGE = 0.271*[AMCC+AB0] + 2.439*AM + 0.9*AB1 + 0.81*AB2 + 0.729*[S+AB3?]

            Spell AMc0 = castingState.GetSpell(SpellId.ArcaneMissilesNoProc);
            Spell AMCC = castingState.GetSpell(SpellId.ArcaneMissilesCC);
            Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast00NoCC);
            Spell AB1 = castingState.GetSpell(SpellId.ArcaneBlast11NoCC);
            Spell AB2 = castingState.GetSpell(SpellId.ArcaneBlast22NoCC);
            Spell FrB0 = castingState.GetSpell(SpellId.FrostboltNoCC);

            Spell AB3 = castingState.GetSpell(SpellId.ArcaneBlast00);
            Spell FrB = castingState.GetSpell(SpellId.Frostbolt);

            float CC = 0.02f * castingState.CalculationOptions.ArcaneConcentration;

            //AMCC-AB0                       0.1
            SpellCycle chain1 = new SpellCycle(2);
            chain1.AddSpell(AMCC, castingState);
            chain1.AddSpell(AB0, castingState);
            chain1.Calculate(castingState);

            //AM?0-AB1-AMCC-AB0              0.9*0.1
            SpellCycle chain2 = new SpellCycle(4);
            chain2.AddSpell(AMc0, castingState);
            chain2.AddSpell(AB1, castingState);
            chain2.AddSpell(AMCC, castingState);
            chain2.AddSpell(AB0, castingState);
            chain2.Calculate(castingState);

            //AM?0-AB1-AM?0-AB2-AMCC-AB0     0.9*0.9*0.1
            SpellCycle chain3 = new SpellCycle(6);
            chain3.AddSpell(AMc0, castingState);
            chain3.AddSpell(AB1, castingState);
            chain3.AddSpell(AMc0, castingState);
            chain3.AddSpell(AB2, castingState);
            chain3.AddSpell(AMCC, castingState);
            chain3.AddSpell(AB0, castingState);
            chain3.Calculate(castingState);

            //AM?0-AB1-AM?0-AB2-AM?0-S-AB3?  0.9*0.9*0.9
            SpellCycle chain4 = new SpellCycle(13);
            chain4.AddSpell(AMc0, castingState);
            chain4.AddSpell(AB1, castingState);
            chain4.AddSpell(AMc0, castingState);
            chain4.AddSpell(AB2, castingState);
            chain4.AddSpell(AMc0, castingState);
            chain4.AddSpell(FrB0, castingState);
            float gap = 8 - AMc0.CastTime - FrB0.CastTime;
            while (gap - AB3.CastTime >= FrB.CastTime)
            {
                chain4.AddSpell(FrB, castingState);
                gap -= FrB.CastTime;
            }
            if (AB3.CastTime < gap) chain4.AddPause(gap - AB3.CastTime + castingState.Latency);
            chain4.AddSpell(AB3, castingState);
            chain4.Calculate(castingState);

            CastTime = CC * chain1.CastTime + CC * (1 - CC) * chain2.CastTime + CC * (1 - CC) * (1 - CC) * chain3.CastTime + (1 - CC) * (1 - CC) * (1 - CC) * chain4.CastTime;
            CostPerSecond = (CC * chain1.CastTime * chain1.CostPerSecond + CC * (1 - CC) * chain2.CastTime * chain2.CostPerSecond + CC * (1 - CC) * (1 - CC) * chain3.CastTime * chain3.CostPerSecond + (1 - CC) * (1 - CC) * (1 - CC) * chain4.CastTime * chain4.CostPerSecond) / CastTime;
            DamagePerSecond = (CC * chain1.CastTime * chain1.DamagePerSecond + CC * (1 - CC) * chain2.CastTime * chain2.DamagePerSecond + CC * (1 - CC) * (1 - CC) * chain3.CastTime * chain3.DamagePerSecond + (1 - CC) * (1 - CC) * (1 - CC) * chain4.CastTime * chain4.DamagePerSecond) / CastTime;
            ThreatPerSecond = (CC * chain1.CastTime * chain1.ThreatPerSecond + CC * (1 - CC) * chain2.CastTime * chain2.ThreatPerSecond + CC * (1 - CC) * (1 - CC) * chain3.CastTime * chain3.ThreatPerSecond + (1 - CC) * (1 - CC) * (1 - CC) * chain4.CastTime * chain4.ThreatPerSecond) / CastTime;
            ManaRegenPerSecond = (CC * chain1.CastTime * chain1.ManaRegenPerSecond + CC * (1 - CC) * chain2.CastTime * chain2.ManaRegenPerSecond + CC * (1 - CC) * (1 - CC) * chain3.CastTime * chain3.ManaRegenPerSecond + (1 - CC) * (1 - CC) * (1 - CC) * chain4.CastTime * chain4.ManaRegenPerSecond) / CastTime;

            commonChain = chain4;
        }

        private SpellCycle commonChain;

        public override string Sequence
        {
            get
            {
                return commonChain.Sequence;
            }
        }
    }

    class ABAM3FrBScCCAM : Spell
    {
        public ABAM3FrBScCCAM(CastingState castingState)
        {
            Name = "ABAM3FrBScCC";
            ABCycle = true;

            //AMCC-AB0                       0.1
            //AM?0-AB1-AMCC-AB0              0.9*0.1
            //AM?0-AB1-AM?0-AB2-AMCC-AB0     0.9*0.9*0.1
            //AM?0-AB1-AM?0-AB2-AM?0-S-AB3?  0.9*0.9*0.9

            //TIME = 0.1*[AMCC+AB0] + 0.9*0.1*[AM+AMCC+AB0+AB1] + 0.9*0.9*0.1*[2*AM+AMCC+AB0+AB1+AB2] + 0.9*0.9*0.9*[3*AM+AB1+AB2+AB3?]
            //     = [0.1 + 0.9*0.1 + 0.9*0.9*0.1]*[AMCC+AB0] + [0.9*0.1 + 2*0.9*0.9*0.1 + 3*0.9*0.9*0.9]*AM + 0.9*AB1 + 0.9*0.9*AB2 + 0.9*0.9*0.9*[S+AB3?]
            //     = 0.271*[AMCC+AB0] + 2.439*AM + 0.9*AB1 + 0.81*AB2 + 0.729*[S+AB3?]
            //DAMAGE = 0.271*[AMCC+AB0] + 2.439*AM + 0.9*AB1 + 0.81*AB2 + 0.729*[S+AB3?]

            Spell AMc0 = castingState.GetSpell(SpellId.ArcaneMissilesNoProc);
            Spell AMCC = castingState.GetSpell(SpellId.ArcaneMissilesCC);
            Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast00NoCC);
            Spell AB1 = castingState.GetSpell(SpellId.ArcaneBlast11NoCC);
            Spell AB2 = castingState.GetSpell(SpellId.ArcaneBlast22NoCC);
            Spell FrB0 = castingState.GetSpell(SpellId.FrostboltNoCC);

            Spell AM = castingState.GetSpell(SpellId.ArcaneMissiles);
            Spell AB3 = castingState.GetSpell(SpellId.ArcaneBlast30);
            Spell FrB = castingState.GetSpell(SpellId.Frostbolt);
            Spell Sc = castingState.GetSpell(SpellId.Scorch);

            float CC = 0.02f * castingState.CalculationOptions.ArcaneConcentration;

            //AMCC-AB0                       0.1
            SpellCycle chain1 = new SpellCycle(2);
            chain1.AddSpell(AMCC, castingState);
            chain1.AddSpell(AB0, castingState);
            chain1.Calculate(castingState);

            //AM?0-AB1-AMCC-AB0              0.9*0.1
            SpellCycle chain2 = new SpellCycle(4);
            chain2.AddSpell(AMc0, castingState);
            chain2.AddSpell(AB1, castingState);
            chain2.AddSpell(AMCC, castingState);
            chain2.AddSpell(AB0, castingState);
            chain2.Calculate(castingState);

            //AM?0-AB1-AM?0-AB2-AMCC-AB0     0.9*0.9*0.1
            SpellCycle chain3 = new SpellCycle(6);
            chain3.AddSpell(AMc0, castingState);
            chain3.AddSpell(AB1, castingState);
            chain3.AddSpell(AMc0, castingState);
            chain3.AddSpell(AB2, castingState);
            chain3.AddSpell(AMCC, castingState);
            chain3.AddSpell(AB0, castingState);
            chain3.Calculate(castingState);

            //AM?0-AB1-AM?0-AB2-AM?0-S-AB3?  0.9*0.9*0.9
            SpellCycle chain4 = new SpellCycle(13);
            chain4.AddSpell(AMc0, castingState);
            chain4.AddSpell(AB1, castingState);
            chain4.AddSpell(AMc0, castingState);
            chain4.AddSpell(AB2, castingState);
            chain4.AddSpell(AMc0, castingState);
            float gap = 8 - AMc0.CastTime;
            bool extraAM = false;
            while (gap >= AM.CastTime)
            {
                chain4.AddSpell(AM, castingState);
                gap -= AM.CastTime;
                extraAM = true;
            }
            if (!extraAM)
            {
                chain4.AddSpell(FrB0, castingState);
                gap -= FrB0.CastTime;
            }
            while (gap >= FrB.CastTime)
            {
                chain4.AddSpell(FrB, castingState);
                gap -= FrB.CastTime;
            }
            while (gap >= Sc.CastTime)
            {
                chain4.AddSpell(Sc, castingState);
                gap -= Sc.CastTime;
            }
            if (AB3.CastTime < gap) chain4.AddPause(gap - AB3.CastTime + castingState.Latency);
            chain4.AddSpell(AB3, castingState);
            chain4.Calculate(castingState);

            CastTime = CC * chain1.CastTime + CC * (1 - CC) * chain2.CastTime + CC * (1 - CC) * (1 - CC) * chain3.CastTime + (1 - CC) * (1 - CC) * (1 - CC) * chain4.CastTime;
            CostPerSecond = (CC * chain1.CastTime * chain1.CostPerSecond + CC * (1 - CC) * chain2.CastTime * chain2.CostPerSecond + CC * (1 - CC) * (1 - CC) * chain3.CastTime * chain3.CostPerSecond + (1 - CC) * (1 - CC) * (1 - CC) * chain4.CastTime * chain4.CostPerSecond) / CastTime;
            DamagePerSecond = (CC * chain1.CastTime * chain1.DamagePerSecond + CC * (1 - CC) * chain2.CastTime * chain2.DamagePerSecond + CC * (1 - CC) * (1 - CC) * chain3.CastTime * chain3.DamagePerSecond + (1 - CC) * (1 - CC) * (1 - CC) * chain4.CastTime * chain4.DamagePerSecond) / CastTime;
            ThreatPerSecond = (CC * chain1.CastTime * chain1.ThreatPerSecond + CC * (1 - CC) * chain2.CastTime * chain2.ThreatPerSecond + CC * (1 - CC) * (1 - CC) * chain3.CastTime * chain3.ThreatPerSecond + (1 - CC) * (1 - CC) * (1 - CC) * chain4.CastTime * chain4.ThreatPerSecond) / CastTime;
            ManaRegenPerSecond = (CC * chain1.CastTime * chain1.ManaRegenPerSecond + CC * (1 - CC) * chain2.CastTime * chain2.ManaRegenPerSecond + CC * (1 - CC) * (1 - CC) * chain3.CastTime * chain3.ManaRegenPerSecond + (1 - CC) * (1 - CC) * (1 - CC) * chain4.CastTime * chain4.ManaRegenPerSecond) / CastTime;

            commonChain = chain4;
        }

        private SpellCycle commonChain;

        public override string Sequence
        {
            get
            {
                return commonChain.Sequence;
            }
        }
    }

    class ABAMCCAM : Spell
    {
        public ABAMCCAM(CastingState castingState)
        {
            Name = "ABAMCC";
            ABCycle = true;

            //AMCC-AB00-AB01-AB12-AB23       0.1
            //AM?0-AB33                      0.9

            Spell AMc0 = castingState.GetSpell(SpellId.ArcaneMissilesNoProc);
            Spell AMCC = castingState.GetSpell(SpellId.ArcaneMissilesCC);
            Spell AB00 = castingState.GetSpell(SpellId.ArcaneBlast00NoCC);
            Spell AB01 = castingState.GetSpell(SpellId.ArcaneBlast01);
            Spell AB12 = castingState.GetSpell(SpellId.ArcaneBlast12);
            Spell AB23 = castingState.GetSpell(SpellId.ArcaneBlast23);
            Spell AB33 = castingState.GetSpell(SpellId.ArcaneBlast33NoCC);

            float CC = 0.02f * castingState.CalculationOptions.ArcaneConcentration;

            if (CC == 0)
            {
                // if we don't have clearcasting then this degenerates to AMc0-AB33
                SpellCycle chain1 = new SpellCycle(2);
                chain1.AddSpell(AMc0, castingState);
                chain1.AddSpell(AB33, castingState);
                chain1.Calculate(castingState);

                CastTime = chain1.CastTime;
                CostPerSecond = chain1.CostPerSecond;
                DamagePerSecond = chain1.DamagePerSecond;
                ThreatPerSecond = chain1.ThreatPerSecond;
                ManaRegenPerSecond = chain1.ManaRegenPerSecond;

                commonChain = chain1;
            }
            else
            {

                //AMCC-AB00-AB01-AB12-AB23       0.1
                SpellCycle chain1 = new SpellCycle(5);
                chain1.AddSpell(AMCC, castingState);
                chain1.AddSpell(AB00, castingState);
                chain1.AddSpell(AB01, castingState);
                chain1.AddSpell(AB12, castingState);
                chain1.AddSpell(AB23, castingState);
                chain1.Calculate(castingState);

                //AM?0-AB33                      0.9
                SpellCycle chain2 = new SpellCycle(2);
                chain2.AddSpell(AMc0, castingState);
                chain2.AddSpell(AB33, castingState);
                chain2.Calculate(castingState);

                CastTime = CC * chain1.CastTime + (1 - CC) * chain2.CastTime;
                CostPerSecond = (CC * chain1.CastTime * chain1.CostPerSecond + (1 - CC) * chain2.CastTime * chain2.CostPerSecond) / CastTime;
                DamagePerSecond = (CC * chain1.CastTime * chain1.DamagePerSecond + (1 - CC) * chain2.CastTime * chain2.DamagePerSecond) / CastTime;
                ThreatPerSecond = (CC * chain1.CastTime * chain1.ThreatPerSecond + (1 - CC) * chain2.CastTime * chain2.ThreatPerSecond) / CastTime;
                ManaRegenPerSecond = (CC * chain1.CastTime * chain1.ManaRegenPerSecond + (1 - CC) * chain2.CastTime * chain2.ManaRegenPerSecond) / CastTime;

                commonChain = chain2;
            }
        }

        private SpellCycle commonChain;

        public override string Sequence
        {
            get
            {
                return commonChain.Sequence;
            }
        }
    }

    class ABAM3CCAM : Spell
    {
        public ABAM3CCAM(CastingState castingState)
        {
            Name = "ABAM3CC";
            ABCycle = true;

            //AM?0-AB33-AMCC subcycle
            //starts with 3 AB debuffs, alternate AM-AB until AM procs CC, then AM chain and stop

            //AM?0-AB33-AM?0-AB33-...=0.1*0.9*...
            //...
            //AM?0-AB33-AM?0-AB33-AMCC=0.1*0.9*0.9
            //AM?0-AB33-AMCC=0.1*0.9
            //AMCC=0.1

            //V = AMCC + 0.1*0.9*AM?0AB33 + 0.1*0.9*0.9*2*AM?0AB33 + ... + 0.1*0.9^n*n*AM?0AB33 + ...
            //  = AMCC + 0.1*AM?0AB33 * sum_1_inf n*0.9^n
            //  = AMCC + 9*AM?0AB33

            // it is on average equivalent to (AM?0-AB33)x9+AMCC cycle


            //AB00-AM?0-AB11-AM?0-AB22-[(AM?0-AB33)x9+AMCC]       0.9*0.9
            //AB00-AM?0-AB11-AMCC                                 0.9*0.1
            //AB00-AMCC                                           0.1

            Spell AMc0 = castingState.GetSpell(SpellId.ArcaneMissilesNoProc);
            Spell AMCC = castingState.GetSpell(SpellId.ArcaneMissilesCC);
            Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast00NoCC);
            Spell AB1 = castingState.GetSpell(SpellId.ArcaneBlast11NoCC);
            Spell AB2 = castingState.GetSpell(SpellId.ArcaneBlast22NoCC);
            Spell AB3 = castingState.GetSpell(SpellId.ArcaneBlast33NoCC);

            float CC = 0.02f * castingState.CalculationOptions.ArcaneConcentration;

            if (CC == 0)
            {
                // if we don't have clearcasting then this degenerates to AMc0-AB33
                SpellCycle chain1 = new SpellCycle(2);
                chain1.AddSpell(AMc0, castingState);
                chain1.AddSpell(AB3, castingState);
                chain1.Calculate(castingState);

                CastTime = chain1.CastTime;
                CostPerSecond = chain1.CostPerSecond;
                DamagePerSecond = chain1.DamagePerSecond;
                ThreatPerSecond = chain1.ThreatPerSecond;
                ManaRegenPerSecond = chain1.ManaRegenPerSecond;

                commonChain = chain1;
            }
            else
            {
                //AB00-AM?0-AB11-AM?0-AB22-[(AM?0-AB33)x9+AMCC]       0.9*0.9
                SpellCycle chain1 = new SpellCycle(24);
                chain1.AddSpell(AB0, castingState);
                chain1.AddSpell(AMc0, castingState);
                chain1.AddSpell(AB1, castingState);
                chain1.AddSpell(AMc0, castingState);
                chain1.AddSpell(AB2, castingState);
                for (int i = 0; i < (int)((1 - CC) / CC); i++)
                {
                    chain1.AddSpell(AMc0, castingState);
                    chain1.AddSpell(AB3, castingState);
                }
                chain1.AddSpell(AMCC, castingState);
                chain1.Calculate(castingState);

                //AB00-AM?0-AB11-AMCC                                 0.9*0.1
                SpellCycle chain2 = new SpellCycle(4);
                chain2.AddSpell(AB0, castingState);
                chain2.AddSpell(AMc0, castingState);
                chain2.AddSpell(AB1, castingState);
                chain2.AddSpell(AMCC, castingState);
                chain2.Calculate(castingState);

                //AB00-AMCC                                           0.1
                SpellCycle chain3 = new SpellCycle(2);
                chain3.AddSpell(AB0, castingState);
                chain3.AddSpell(AMCC, castingState);
                chain3.Calculate(castingState);


                CastTime = (1 - CC) * (1 - CC) * chain1.CastTime + CC * (1 - CC) * chain2.CastTime + CC * chain3.CastTime;
                CostPerSecond = ((1 - CC) * (1 - CC) * chain1.CastTime * chain1.CostPerSecond + CC * (1 - CC) * chain2.CastTime * chain2.CostPerSecond + CC * chain3.CastTime * chain3.CostPerSecond) / CastTime;
                DamagePerSecond = ((1 - CC) * (1 - CC) * chain1.CastTime * chain1.DamagePerSecond + CC * (1 - CC) * chain2.CastTime * chain2.DamagePerSecond + CC * chain3.CastTime * chain3.DamagePerSecond) / CastTime;
                ThreatPerSecond = ((1 - CC) * (1 - CC) * chain1.CastTime * chain1.ThreatPerSecond + CC * (1 - CC) * chain2.CastTime * chain2.ThreatPerSecond + CC * chain3.CastTime * chain3.ThreatPerSecond) / CastTime;
                ManaRegenPerSecond = ((1 - CC) * (1 - CC) * chain1.CastTime * chain1.ManaRegenPerSecond + CC * (1 - CC) * chain2.CastTime * chain2.ManaRegenPerSecond + CC * chain3.CastTime * chain3.ManaRegenPerSecond) / CastTime;

                commonChain = chain3;
            }
        }

        private SpellCycle commonChain;

        public override string Sequence
        {
            get
            {
                return commonChain.Sequence;
            }
        }
    }
    #endregion
}
