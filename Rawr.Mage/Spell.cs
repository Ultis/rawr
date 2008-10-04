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
        Arcane,
        FrostFire
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
        [Description("MBAM")]
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
        [Description("Frostfire Bolt")]
        FrostfireBolt,
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
        ArcaneBlastSpam,
        ArcaneBlast33,
        ArcaneBlast33NoCC,
        [Description("Arcane Blast(0)")]
        ArcaneBlast00,
        ArcaneBlast00NoCC,
        ArcaneBlast0POM,
        ArcaneBlast10,
        ArcaneBlast01,
        [Description("Arcane Blast(1)")]
        ArcaneBlast11,
        ArcaneBlast11NoCC,
        [Description("Arcane Blast(2)")]
        ArcaneBlast22,
        ArcaneBlast22NoCC,
        ArcaneBlast12,
        ArcaneBlast23,
        ArcaneBlast30,
        ArcaneBlast0Hit,
        ArcaneBlast1Hit,
        ArcaneBlast2Hit,
        ArcaneBlast3Hit,
        ArcaneBlast0Miss,
        ArcaneBlast1Miss,
        ArcaneBlast2Miss,
        ArcaneBlast3Miss,
        ABarAM,
        ABP,
        ABAM,
        ABMBAM,
        ABABar,
        FBABar,
        FrBABar,
        FFBABar,
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
        public float DotDamageCoefficient;
    }

    public class SpellContribution : IComparable<SpellContribution>
    {
        public string Name;
        public float Hits;
        public float Damage;

        public int CompareTo(SpellContribution other)
        {
            return other.Damage.CompareTo(this.Damage);
        }
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

        public abstract void AddSpellContribution(Dictionary<string, SpellContribution> dict, float duration);
    }

    public class SpellCustomMix : Spell
    {
        float weightTotal;
        CastingState castingState;

        public SpellCustomMix(CastingState castingState)
        {
            this.castingState = castingState;
            sequence = "Custom Mix";
            Name = "Custom Mix";
            if (castingState.CalculationOptions.CustomSpellMix == null) return;
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

        public override void AddSpellContribution(Dictionary<string, SpellContribution> dict, float duration)
        {
            foreach (SpellWeight spellWeight in castingState.CalculationOptions.CustomSpellMix)
            {
                Spell spell = castingState.GetSpell(spellWeight.Spell);
                spell.AddSpellContribution(dict, duration * spellWeight.Weight / weightTotal);
            }
        }
    }

    public abstract class BaseSpell : Spell
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

        public static Dictionary<int, int> BaseMana = new Dictionary<int, int>();
        static BaseSpell()
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

        public BaseSpell(string name, bool channeled, bool binary, bool instant, bool areaEffect, int range, float castTime, float cooldown, MagicSchool magicSchool, SpellData spellData) : this(name, channeled, binary, instant, areaEffect, spellData.Cost, range, castTime, cooldown, magicSchool, spellData.MinDamage, spellData.MaxDamage, spellData.PeriodicDamage, 1, 1, spellData.SpellDamageCoefficient, spellData.DotDamageCoefficient, 0, false) { }
        public BaseSpell(string name, bool channeled, bool binary, bool instant, bool areaEffect, int range, float castTime, float cooldown, MagicSchool magicSchool, SpellData spellData, float hitProcs, float castProcs) : this(name, channeled, binary, instant, areaEffect, spellData.Cost, range, castTime, cooldown, magicSchool, spellData.MinDamage, spellData.MaxDamage, spellData.PeriodicDamage, hitProcs, castProcs, spellData.SpellDamageCoefficient, spellData.DotDamageCoefficient, 0, false) { }
        public BaseSpell(string name, bool channeled, bool binary, bool instant, bool areaEffect, int range, float castTime, float cooldown, MagicSchool magicSchool, SpellData spellData, float hitProcs, float castProcs, float dotDuration, bool spammedDot) : this(name, channeled, binary, instant, areaEffect, spellData.Cost, range, castTime, cooldown, magicSchool, spellData.MinDamage, spellData.MaxDamage, spellData.PeriodicDamage, hitProcs, castProcs, spellData.SpellDamageCoefficient, spellData.DotDamageCoefficient, dotDuration, spammedDot) { }
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
        public float CostAmplifier;
        public float SpellModifier;
        public float DirectDamageModifier;
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
        public bool ForceHit;
        public bool ForceMiss;
        public float InterruptProtection;
        public bool EffectProc;

        public float Cooldown;
        public float Cost;

        private CastingState castingState;
        private Waterbolt waterbolt;

        public virtual void Calculate(CastingState castingState)
        {
            this.castingState = castingState;
            if (AreaEffect) TargetProcs *= castingState.CalculationOptions.AoeTargets;
            Cooldown = BaseCooldown;

            CostModifier = 1;
            CostAmplifier = 1;
            DirectDamageModifier = 1;
            if (MagicSchool == MagicSchool.Fire || MagicSchool == MagicSchool.FrostFire) CostAmplifier *= (1.0f - 0.01f * castingState.MageTalents.Pyromaniac);
            if (MagicSchool == MagicSchool.Fire || MagicSchool == MagicSchool.Frost || MagicSchool == MagicSchool.FrostFire) CostAmplifier *= (1.0f - 0.01f * castingState.MageTalents.ElementalPrecision);
            if (castingState.MageTalents.FrostChanneling > 0) CostAmplifier *= (1.0f - 0.01f - 0.03f * castingState.MageTalents.FrostChanneling);
            if (MagicSchool == MagicSchool.Arcane) CostAmplifier *= (1.0f - 0.01f * castingState.MageTalents.ArcaneFocus);
            if (castingState.ArcanePower) CostModifier += 0.3f;
            if (MagicSchool == MagicSchool.Fire || MagicSchool == MagicSchool.FrostFire) AffectedByFlameCap = true;
            if (MagicSchool == MagicSchool.Fire || MagicSchool == MagicSchool.FrostFire) InterruptProtection += 0.35f * castingState.MageTalents.BurningSoul;
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
                case MagicSchool.FrostFire:
                    SpellModifier = castingState.FrostFireSpellModifier;
                    CritRate = castingState.FrostFireCritRate;
                    CritBonus = castingState.FrostFireCritBonus;
                    RawSpellDamage = castingState.FrostFireDamage;
                    HitRate = castingState.FrostFireHitRate;
                    RealResistance = Math.Min(castingState.CalculationOptions.FireResist, castingState.CalculationOptions.FrostResist);
                    ThreatMultiplier = castingState.FrostFireThreatMultiplier;
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
                SpellModifier = (1 + 0.01f * castingState.MageTalents.ArcaneInstability) * (1 + 0.01f * castingState.MageTalents.PlayingWithFire);
                if (castingState.ArcanePower)
                {
                    SpellModifier *= 1.3f;
                }
                if (castingState.MageTalents.MoltenFury > 0)
                {
                    SpellModifier *= (1 + 0.06f * castingState.MageTalents.MoltenFury * castingState.CalculationOptions.MoltenFuryPercentage);
                }
                if (MagicSchool == MagicSchool.Fire) SpellModifier *= (1 + 0.02f * castingState.MageTalents.FirePower);
                if (MagicSchool == MagicSchool.Frost) SpellModifier *= (1 + 0.02f * castingState.MageTalents.PiercingIce);

                float maxHitRate = 1.00f;
                int playerLevel = castingState.CalculationOptions.PlayerLevel;
                if (MagicSchool == MagicSchool.Arcane) HitRate = Math.Min(maxHitRate, ((targetLevel <= playerLevel + 2) ? (0.96f - (targetLevel - playerLevel) * 0.01f) : (0.94f - (targetLevel - playerLevel - 2) * 0.11f)) + castingState.SpellHit + 0.01f * castingState.MageTalents.ArcaneFocus);
                if (MagicSchool == MagicSchool.Fire) HitRate = Math.Min(maxHitRate, ((targetLevel <= playerLevel + 2) ? (0.96f - (targetLevel - playerLevel) * 0.01f) : (0.94f - (targetLevel - playerLevel - 2) * 0.11f)) + castingState.SpellHit + 0.01f * castingState.MageTalents.ElementalPrecision);
                if (MagicSchool == MagicSchool.Frost) HitRate = Math.Min(maxHitRate, ((targetLevel <= playerLevel + 2) ? (0.96f - (targetLevel - playerLevel) * 0.01f) : (0.94f - (targetLevel - playerLevel - 2) * 0.11f)) + castingState.SpellHit + 0.01f * castingState.MageTalents.ElementalPrecision);
                if (MagicSchool == MagicSchool.Nature) HitRate = Math.Min(maxHitRate, ((targetLevel <= playerLevel + 2) ? (0.96f - (targetLevel - playerLevel) * 0.01f) : (0.94f - (targetLevel - playerLevel - 2) * 0.11f)) + castingState.SpellHit);
                if (MagicSchool == MagicSchool.Shadow) HitRate = Math.Min(maxHitRate, ((targetLevel <= playerLevel + 2) ? (0.96f - (targetLevel - playerLevel) * 0.01f) : (0.94f - (targetLevel - playerLevel - 2) * 0.11f)) + castingState.SpellHit);
            }

            if (ManualClearcasting && !ClearcastingAveraged)
            {
                CritRate -= 0.015f * castingState.MageTalents.ArcanePotency; // replace averaged arcane potency with actual % chance
                if (ClearcastingActive) CritRate += 0.15f * castingState.MageTalents.ArcanePotency;
            }

            PartialResistFactor = (RealResistance == 1) ? 0 : (1 - Math.Max(0f, RealResistance - castingState.BaseStats.SpellPenetration / 350f * 0.75f) - ((targetLevel > castingState.CalculationOptions.PlayerLevel) ? ((targetLevel - castingState.CalculationOptions.PlayerLevel) * 0.02f) : 0f));
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
            float levelScalingFactor;
            levelScalingFactor = (float)((52f / 82f) * Math.Pow(63f / 131f, (castingState.CalculationOptions.PlayerLevel - 70) / 10f));

            // TODO consider converting to discrete model for procs
            float maxPushback = 0.5f;
            GlobalCooldown = Math.Max(castingState.GlobalCooldownLimit, 1.5f / CastingSpeed);
            CastTime = BaseCastTime / CastingSpeed + castingState.Latency;
            CastTime = CastTime * (1 + InterruptFactor * maxPushback) - (maxPushback * 0.5f + castingState.Latency) * maxPushback * InterruptFactor;
            if (CastTime < GlobalCooldown + castingState.Latency) CastTime = GlobalCooldown + castingState.Latency;

            // Quagmirran
            if (castingState.BaseStats.SpellHasteFor6SecOnHit_10_45 > 0 && HitProcs > 0)
            {
                // hasted casttime
                float speed = CastingSpeed / (1 + Haste / 995f * levelScalingFactor) * (1 + (Haste + castingState.BaseStats.SpellHasteFor6SecOnHit_10_45) / 995f * levelScalingFactor);
                float gcd = Math.Max(castingState.GlobalCooldownLimit, 1.5f / speed);
                float cast = BaseCastTime / speed + castingState.Latency;
                cast = cast * (1 + InterruptFactor * maxPushback) - (maxPushback * 0.5f + castingState.Latency) * maxPushback * InterruptFactor;
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
                CastTime = CastTime * (1 + InterruptFactor * maxPushback) - (maxPushback * 0.5f + castingState.Latency) * maxPushback * InterruptFactor;
                if (CastTime < GlobalCooldown + castingState.Latency) CastTime = GlobalCooldown + castingState.Latency;
            }

            // MSD
            if (castingState.BaseStats.SpellHasteFor6SecOnCast_15_45 > 0 && CastProcs > 0)
            {
                // hasted casttime
                float speed = CastingSpeed / (1 + Haste / 995f * levelScalingFactor) * (1 + (Haste + castingState.BaseStats.SpellHasteFor6SecOnCast_15_45) / 995f * levelScalingFactor);
                float gcd = Math.Max(castingState.GlobalCooldownLimit, 1.5f / speed);
                float cast = BaseCastTime / speed + castingState.Latency;
                cast = cast * (1 + InterruptFactor * maxPushback) - (maxPushback * 0.5f + castingState.Latency) * maxPushback * InterruptFactor;
                if (cast < gcd + castingState.Latency) cast = gcd + castingState.Latency;

                CastingSpeed /= (1 + Haste / 995f * levelScalingFactor);
                float castsAffected = 0;
                for (int c = 0; c < CastProcs; c++) castsAffected += (float)Math.Ceiling((6f - c * CastTime / CastProcs) / cast) / CastProcs;
                Haste += castingState.BaseStats.SpellHasteFor6SecOnCast_15_45 * castsAffected * cast / (45f + CastTime / CastProcs / 0.15f);
                //Haste += castingState.BasicStats.SpellHasteFor6SecOnCast_15_45 * 6f / (45f + CastTime / CastProcs / 0.15f);
                CastingSpeed *= (1 + Haste / 995f * levelScalingFactor);

                GlobalCooldown = Math.Max(castingState.GlobalCooldownLimit, 1.5f / CastingSpeed);
                CastTime = BaseCastTime / CastingSpeed + castingState.Latency;
                CastTime = CastTime * (1 + InterruptFactor * maxPushback) - (maxPushback * 0.5f + castingState.Latency) * maxPushback * InterruptFactor;
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
                proccedCastTime = proccedCastTime * (1 + InterruptFactor * maxPushback) - (maxPushback * 0.5f + castingState.Latency) * maxPushback * InterruptFactor;
                if (proccedCastTime < proccedGcd + castingState.Latency) proccedCastTime = proccedGcd + castingState.Latency;
                int chancesToProc = (int)(((int)Math.Floor(5f / proccedCastTime) + 1) * HitProcs);
                if (!Instant) chancesToProc -= 1;
                chancesToProc *= (int)(TargetProcs / HitProcs);
                Haste = rawHaste + castingState.BaseStats.SpellHasteFor5SecOnCrit_50 * (1 - (float)Math.Pow(1 - 0.5f * CritRate, chancesToProc));
                //Haste = rawHaste + castingState.BasicStats.SpellHasteFor5SecOnCrit_50 * ProcBuffUp(1 - (float)Math.Pow(1 - 0.5f * CritRate, HitProcs), 5, CastTime);
                CastingSpeed *= (1 + Haste / 995f * levelScalingFactor);
                GlobalCooldown = Math.Max(castingState.GlobalCooldownLimit, 1.5f / CastingSpeed);
                CastTime = BaseCastTime / CastingSpeed + castingState.Latency;
                CastTime = CastTime * (1 + InterruptFactor * maxPushback) - (maxPushback * 0.5f + castingState.Latency) * maxPushback * InterruptFactor;
                if (CastTime < GlobalCooldown + castingState.Latency) CastTime = GlobalCooldown + castingState.Latency;
            }

            Cost = (float)Math.Floor(Math.Floor(BaseCost * CostAmplifier) * CostModifier); // glyph and talent amplifiers are rounded down

            CritRate = Math.Min(1, CritRate);
            //Cost *= (1f - CritRate * 0.1f * castingState.MageTalents.MasterOfElements);
            if (!Channeled) Cost -= CritRate * BaseCost * 0.1f * castingState.MageTalents.MasterOfElements; // from what I know MOE works on base cost
            if (!Channeled && (MagicSchool == MagicSchool.Fire || MagicSchool == MagicSchool.FrostFire)) Cost += CritRate * BaseCost * 0.01f * castingState.MageTalents.Burnout;

            CostPerSecond = Cost / CastTime;

            if (!ManualClearcasting || ClearcastingAveraged)
            {
                CostPerSecond *= (1 - 0.02f * castingState.MageTalents.ArcaneConcentration);
            }
            else if (ClearcastingActive)
            {
                CostPerSecond = 0;
            }

            // hit & crit ranges, do it before passive spell damage effects for cleaner comparison in game
            MinHitDamage = (BaseMinDamage + RawSpellDamage * SpellDamageCoefficient) * SpellModifier * DirectDamageModifier;
            MaxHitDamage = (BaseMaxDamage + RawSpellDamage * SpellDamageCoefficient) * SpellModifier * DirectDamageModifier;
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

            if (!ForceMiss)
            {
                SpellDamage = RawSpellDamage * SpellDamageCoefficient;
                float baseAverage = (BaseMinDamage + BaseMaxDamage) / 2f + SpellDamage;
                float critMultiplier = 1 + (CritBonus - 1) * Math.Max(0, CritRate - castingState.ResilienceCritRateReduction);
                float resistMultiplier = (ForceHit ? 1.0f : HitRate) * PartialResistFactor;
                int targets = 1;
                if (AreaEffect) targets = castingState.CalculationOptions.AoeTargets;
                AverageDamage = baseAverage * SpellModifier * DirectDamageModifier * targets * (ForceHit ? 1.0f : HitRate);
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
            }

            if (castingState.WaterElemental)
            {
                waterbolt = new Waterbolt(castingState, RawSpellDamage); // TODO should be frost damage
                DamagePerSecond += waterbolt.DamagePerSecond;
            }
            if (!ForceMiss && !EffectProc && castingState.BaseStats.LightningCapacitorProc > 0)
            {
                BaseSpell LightningBolt = castingState.LightningBolt;
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
            if (!ForceMiss && !EffectProc && castingState.BaseStats.ShatteredSunAcumenProc > 0 && !castingState.CalculationOptions.Aldor)
            {
                BaseSpell ArcaneBolt = castingState.ArcaneBolt;
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

        public override void AddSpellContribution(Dictionary<string, SpellContribution> dict, float duration)
        {
            SpellContribution contrib;
            if (!dict.TryGetValue(Name, out contrib))
            {
                contrib = new SpellContribution() { Name = Name };
                dict[Name] = contrib;
            }
            contrib.Hits += HitProcs * duration / CastTime;
            contrib.Damage += AverageDamage * duration / CastTime;
            if (waterbolt != null)
            {
                if (!dict.TryGetValue(waterbolt.Name, out contrib))
                {
                    contrib = new SpellContribution() { Name = waterbolt.Name };
                    dict[waterbolt.Name] = contrib;
                }
                contrib.Hits += duration / waterbolt.CastTime;
                contrib.Damage += waterbolt.DamagePerSecond * duration;
            }
            if (!EffectProc && castingState.BaseStats.LightningCapacitorProc > 0)
            {
                BaseSpell LightningBolt = castingState.LightningBolt;
                if (!dict.TryGetValue(LightningBolt.Name, out contrib))
                {
                    contrib = new SpellContribution() { Name = LightningBolt.Name };
                    dict[LightningBolt.Name] = contrib;
                }
                //discrete model
                int hitsInsideCooldown = (int)(2.5f / (CastTime / HitProcs));
                float avgCritsPerHit = CritRate * TargetProcs / HitProcs;
                float avgHitsToDischarge = 3f / avgCritsPerHit;
                if (avgHitsToDischarge < 1) avgHitsToDischarge = 1;
                float boltDps = LightningBolt.AverageDamage / ((CastTime / HitProcs) * (hitsInsideCooldown + avgHitsToDischarge));
                contrib.Hits += duration / ((CastTime / HitProcs) * (hitsInsideCooldown + avgHitsToDischarge));
                contrib.Damage += boltDps * duration;
            }
            if (!EffectProc && castingState.BaseStats.ShatteredSunAcumenProc > 0 && !castingState.CalculationOptions.Aldor)
            {
                BaseSpell ArcaneBolt = castingState.ArcaneBolt;
                if (!dict.TryGetValue(ArcaneBolt.Name, out contrib))
                {
                    contrib = new SpellContribution() { Name = ArcaneBolt.Name };
                    dict[ArcaneBolt.Name] = contrib;
                }
                float boltDps = ArcaneBolt.AverageDamage / (45f + CastTime / HitProcs / 0.1f);
                contrib.Hits += duration / (45f + CastTime / HitProcs / 0.1f);
                contrib.Damage += boltDps * duration;
            }
        }
    }

    #region Base Spells
    public class Waterbolt : Spell
    {
        public Waterbolt(CastingState castingState, float playerSpellDamage)
        {
            Name = "Waterbolt";
            Character character = castingState.CalculationOptions.Character;
            CalculationOptionsMage calculationOptions = castingState.CalculationOptions;
            int playerLevel = calculationOptions.PlayerLevel;
            int targetLevel = calculationOptions.TargetLevel;
            // 45 sec, 3 min cooldown + cold snap
            // 2.5 sec Waterbolt, affected by heroism, totems, 0.4x frost damage from character
            // TODO recheck all buffs that apply
            float spellHit = 0;
            float spellCrit = 0.05f;
            if (character.ActiveBuffs.Contains(Buff.GetBuffByName("Totem of Wrath"))) spellCrit += 0.03f;
            if (character.ActiveBuffs.Contains(Buff.GetBuffByName("Inspiring Presence"))) spellHit += 0.01f;
            if (character.ActiveBuffs.Contains(Buff.GetBuffByName("Misery"))) spellHit += 0.03f;
            float hitRate = Math.Min(1.00f, ((targetLevel <= playerLevel + 2) ? (0.96f - (targetLevel - playerLevel) * 0.01f) : (0.94f - (targetLevel - playerLevel - 2) * 0.11f)) + spellHit);
            if (character.ActiveBuffs.Contains(Buff.GetBuffByName("Winter's Chill")) || character.MageTalents.WintersChill > 0 || character.ActiveBuffs.Contains(Buff.GetBuffByName("Improved Scorch")) || character.MageTalents.ImprovedScorch > 0) spellCrit += 0.1f;
            float multiplier = hitRate;
            float haste = 1.0f;
            if (character.ActiveBuffs.Contains(Buff.GetBuffByName("Curse of the Elements"))) multiplier *= 1.1f;
            if (character.ActiveBuffs.Contains(Buff.GetBuffByName("Improved Curse of the Elements"))) multiplier *= 1.13f / 1.1f;
            if (character.ActiveBuffs.Contains(Buff.GetBuffByName("Misery"))) haste *= 1.05f;
            if (castingState.Heroism) haste *= 1.3f;
            float realResistance = calculationOptions.FrostResist;
            float partialResistFactor = (realResistance == 1) ? 0 : (1 - realResistance - ((targetLevel > playerLevel) ? ((targetLevel - playerLevel) * 0.02f) : 0f));
            multiplier *= partialResistFactor;

            CastTime = 2.5f / haste;
            CostPerSecond = 0.0f;
            DamagePerSecond = (521.5f + (0.4f * playerSpellDamage + (character.ActiveBuffs.Contains(Buff.GetBuffByName("Wrath of Air")) ? 101 : 0)) * 2.5f / 3.5f) * multiplier * (1 + 0.5f * spellCrit) / 2.5f * haste;
            ThreatPerSecond = 0.0f;
            ManaRegenPerSecond = 0.0f;
        }

        public override void AddSpellContribution(Dictionary<string, SpellContribution> dict, float duration)
        {
            SpellContribution contrib;
            if (!dict.TryGetValue(Name, out contrib))
            {
                contrib = new SpellContribution() { Name = Name };
                dict[Name] = contrib;
            }
            contrib.Hits += duration / CastTime;
            contrib.Damage += DamagePerSecond * duration;
        }
    }

    public class Wand : BaseSpell
    {
        public Wand(CastingState castingState, MagicSchool school, int minDamage, int maxDamage, float speed)
            : base("Wand", false, false, false, false, 0, 30, 0, 0, school, minDamage, maxDamage, 0, 1, 0, 0, 0, 0, false)
        {
            // Tested: affected by Arcane Instability, affected by Chaotic meta, not affected by Arcane Power
            Calculate(castingState);
            CastTime = speed;
            CritRate = castingState.SpellCrit;
            CritBonus = (1 + (1.5f * (1 + castingState.BaseStats.BonusSpellCritMultiplier) - 1)) * castingState.ResilienceCritDamageReduction;
            SpellModifier = (1 + 0.01f * castingState.MageTalents.ArcaneInstability) * (1 + 0.01f * castingState.MageTalents.PlayingWithFire) * (1 + castingState.BaseStats.BonusSpellPowerMultiplier);
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
            CalculateDerivedStats(castingState);
        }
    }

    public class FireBlast : BaseSpell
    {
        public static SpellData[] SpellData = new SpellData[11];
        static FireBlast()
        {
            SpellData[0] = new SpellData() { Cost = (int)(0.21 * BaseMana[71]), MinDamage = 664, MaxDamage = 786, SpellDamageCoefficient = 1.5f / 3.5f };
            SpellData[1] = new SpellData() { Cost = (int)(0.21 * BaseMana[71]), MinDamage = 667, MaxDamage = 790, SpellDamageCoefficient = 1.5f / 3.5f };
            SpellData[2] = new SpellData() { Cost = (int)(0.21 * BaseMana[72]), MinDamage = 671, MaxDamage = 794, SpellDamageCoefficient = 1.5f / 3.5f };
            SpellData[3] = new SpellData() { Cost = (int)(0.21 * BaseMana[73]), MinDamage = 675, MaxDamage = 798, SpellDamageCoefficient = 1.5f / 3.5f };
            SpellData[4] = new SpellData() { Cost = (int)(0.21 * BaseMana[74]), MinDamage = 760, MaxDamage = 900, SpellDamageCoefficient = 1.5f / 3.5f };
            SpellData[5] = new SpellData() { Cost = (int)(0.21 * BaseMana[75]), MinDamage = 764, MaxDamage = 904, SpellDamageCoefficient = 1.5f / 3.5f };
            SpellData[6] = new SpellData() { Cost = (int)(0.21 * BaseMana[76]), MinDamage = 768, MaxDamage = 908, SpellDamageCoefficient = 1.5f / 3.5f };
            SpellData[7] = new SpellData() { Cost = (int)(0.21 * BaseMana[77]), MinDamage = 772, MaxDamage = 912, SpellDamageCoefficient = 1.5f / 3.5f };
            SpellData[8] = new SpellData() { Cost = (int)(0.21 * BaseMana[78]), MinDamage = 776, MaxDamage = 916, SpellDamageCoefficient = 1.5f / 3.5f };
            SpellData[9] = new SpellData() { Cost = (int)(0.21 * BaseMana[79]), MinDamage = 780, MaxDamage = 920, SpellDamageCoefficient = 1.5f / 3.5f };
            SpellData[10] = new SpellData() { Cost = (int)(0.21 * BaseMana[80]), MinDamage = 925, MaxDamage = 1095, SpellDamageCoefficient = 1.5f / 3.5f };
        }
        private static SpellData GetMaxRankSpellData(CalculationOptionsMage options)
        {
            return SpellData[options.PlayerLevel - 70];
        }

        public FireBlast(CastingState castingState)
            : base("Fire Blast", false, false, true, false, 20, 0, 8, MagicSchool.Fire, GetMaxRankSpellData(castingState.CalculationOptions))
        {
            Calculate(castingState);
        }

        public override void Calculate(CastingState castingState)
        {
            base.Calculate(castingState);
            Cooldown -= 1.0f * castingState.MageTalents.ImprovedFireBlast;
            CritRate += 0.02f * castingState.MageTalents.Incineration;
            SpellModifier *= (1 + 0.02f * castingState.MageTalents.SpellImpact);
            CalculateDerivedStats(castingState);
        }
    }

    public class Scorch : BaseSpell
    {
        public static SpellData[] SpellData = new SpellData[11];
        static Scorch()
        {
            SpellData[0] = new SpellData() { Cost = (int)(0.08 * BaseMana[70]), MinDamage = 305, MaxDamage = 361, SpellDamageCoefficient = 1.5f / 3.5f };
            SpellData[1] = new SpellData() { Cost = (int)(0.08 * BaseMana[71]), MinDamage = 307, MaxDamage = 364, SpellDamageCoefficient = 1.5f / 3.5f };
            SpellData[2] = new SpellData() { Cost = (int)(0.08 * BaseMana[72]), MinDamage = 310, MaxDamage = 366, SpellDamageCoefficient = 1.5f / 3.5f };
            SpellData[3] = new SpellData() { Cost = (int)(0.08 * BaseMana[73]), MinDamage = 321, MaxDamage = 379, SpellDamageCoefficient = 1.5f / 3.5f };
            SpellData[4] = new SpellData() { Cost = (int)(0.08 * BaseMana[74]), MinDamage = 323, MaxDamage = 382, SpellDamageCoefficient = 1.5f / 3.5f };
            SpellData[5] = new SpellData() { Cost = (int)(0.08 * BaseMana[75]), MinDamage = 326, MaxDamage = 385, SpellDamageCoefficient = 1.5f / 3.5f };
            SpellData[6] = new SpellData() { Cost = (int)(0.08 * BaseMana[76]), MinDamage = 328, MaxDamage = 387, SpellDamageCoefficient = 1.5f / 3.5f };
            SpellData[7] = new SpellData() { Cost = (int)(0.08 * BaseMana[77]), MinDamage = 331, MaxDamage = 390, SpellDamageCoefficient = 1.5f / 3.5f };
            SpellData[8] = new SpellData() { Cost = (int)(0.08 * BaseMana[78]), MinDamage = 376, MaxDamage = 444, SpellDamageCoefficient = 1.5f / 3.5f };
            SpellData[9] = new SpellData() { Cost = (int)(0.08 * BaseMana[79]), MinDamage = 379, MaxDamage = 448, SpellDamageCoefficient = 1.5f / 3.5f };
            SpellData[10] = new SpellData() { Cost = (int)(0.08 * BaseMana[80]), MinDamage = 382, MaxDamage = 451, SpellDamageCoefficient = 1.5f / 3.5f };
        }
        private static SpellData GetMaxRankSpellData(CalculationOptionsMage options)
        {
            return SpellData[options.PlayerLevel - 70];
        }

        public Scorch(CastingState castingState, bool clearcastingActive)
            : base("Scorch", false, false, false, false, 30, 1.5f, 0, MagicSchool.Fire, GetMaxRankSpellData(castingState.CalculationOptions))
        {
            ManualClearcasting = true;
            ClearcastingActive = clearcastingActive;
            ClearcastingAveraged = false;
            Calculate(castingState);
        }

        public Scorch(CastingState castingState)
            : base("Scorch", false, false, false, false, 30, 1.5f, 0, MagicSchool.Fire, GetMaxRankSpellData(castingState.CalculationOptions))
        {
            Calculate(castingState);
        }

        public override void Calculate(CastingState castingState)
        {
            base.Calculate(castingState);
            CritRate += 0.02f * castingState.MageTalents.Incineration;
            SpellModifier *= (1 + 0.02f * castingState.MageTalents.SpellImpact);
            CalculateDerivedStats(castingState);
        }
    }

    public class Flamestrike : BaseSpell
    {
        public static SpellData[] SpellData = new SpellData[11];
        static Flamestrike()
        {
            SpellData[0] = new SpellData() { Cost = (int)(0.53 * BaseMana[70]), MinDamage = 480, MaxDamage = 585, PeriodicDamage = 424, SpellDamageCoefficient = 0.2363f, DotDamageCoefficient = 0.12f };
            SpellData[1] = new SpellData() { Cost = (int)(0.53 * BaseMana[71]), MinDamage = 480, MaxDamage = 585, PeriodicDamage = 424, SpellDamageCoefficient = 0.2363f, DotDamageCoefficient = 0.12f };
            SpellData[2] = new SpellData() { Cost = (int)(0.53 * BaseMana[72]), MinDamage = 688, MaxDamage = 842, PeriodicDamage = 620, SpellDamageCoefficient = 0.2363f, DotDamageCoefficient = 0.12f };
            SpellData[3] = new SpellData() { Cost = (int)(0.53 * BaseMana[73]), MinDamage = 690, MaxDamage = 845, PeriodicDamage = 620, SpellDamageCoefficient = 0.2363f, DotDamageCoefficient = 0.12f };
            SpellData[4] = new SpellData() { Cost = (int)(0.53 * BaseMana[74]), MinDamage = 693, MaxDamage = 848, PeriodicDamage = 620, SpellDamageCoefficient = 0.2363f, DotDamageCoefficient = 0.12f };
            SpellData[5] = new SpellData() { Cost = (int)(0.53 * BaseMana[75]), MinDamage = 696, MaxDamage = 851, PeriodicDamage = 620, SpellDamageCoefficient = 0.2363f, DotDamageCoefficient = 0.12f };
            SpellData[6] = new SpellData() { Cost = (int)(0.53 * BaseMana[76]), MinDamage = 699, MaxDamage = 854, PeriodicDamage = 620, SpellDamageCoefficient = 0.2363f, DotDamageCoefficient = 0.12f };
            SpellData[7] = new SpellData() { Cost = (int)(0.53 * BaseMana[77]), MinDamage = 699, MaxDamage = 854, PeriodicDamage = 620, SpellDamageCoefficient = 0.2363f, DotDamageCoefficient = 0.12f };
            SpellData[8] = new SpellData() { Cost = (int)(0.53 * BaseMana[78]), MinDamage = 699, MaxDamage = 854, PeriodicDamage = 620, SpellDamageCoefficient = 0.2363f, DotDamageCoefficient = 0.12f };
            SpellData[9] = new SpellData() { Cost = (int)(0.53 * BaseMana[79]), MinDamage = 873, MaxDamage = 1067, PeriodicDamage = 780, SpellDamageCoefficient = 0.2363f, DotDamageCoefficient = 0.12f };
            SpellData[10] = new SpellData() { Cost = (int)(0.53 * BaseMana[80]), MinDamage = 876, MaxDamage = 1071, PeriodicDamage = 780, SpellDamageCoefficient = 0.2363f, DotDamageCoefficient = 0.12f };
        }
        private static SpellData GetMaxRankSpellData(CalculationOptionsMage options)
        {
            return SpellData[options.PlayerLevel - 70];
        }

        public Flamestrike(CastingState castingState, bool spammedDot)
            : base("Flamestrike", false, false, false, true, 30, 3, 0, MagicSchool.Fire, GetMaxRankSpellData(castingState.CalculationOptions), 1, 1, 8f, spammedDot)
        {
            base.Calculate(castingState);
            AoeDamageCap = 7830;
            CritRate += 0.02f * castingState.MageTalents.WorldInFlames;
            CalculateDerivedStats(castingState);
        }
    }

    public class FrostNova : BaseSpell
    {
        public static SpellData[] SpellData = new SpellData[11];
        static FrostNova()
        {
            SpellData[0] = new SpellData() { Cost = (int)(0.08 * BaseMana[70]), MinDamage = 100, MaxDamage = 113, SpellDamageCoefficient = 1.5f / 3.5f * 0.5f * 0.13f }; // TODO need level 70 WotLK data
            SpellData[1] = new SpellData() { Cost = (int)(0.08 * BaseMana[71]), MinDamage = 232, MaxDamage = 262, SpellDamageCoefficient = 1.5f / 3.5f * 0.5f * 0.13f };
            SpellData[2] = new SpellData() { Cost = (int)(0.08 * BaseMana[72]), MinDamage = 232, MaxDamage = 263, SpellDamageCoefficient = 1.5f / 3.5f * 0.5f * 0.13f };
            SpellData[3] = new SpellData() { Cost = (int)(0.08 * BaseMana[73]), MinDamage = 233, MaxDamage = 263, SpellDamageCoefficient = 1.5f / 3.5f * 0.5f * 0.13f };
            SpellData[4] = new SpellData() { Cost = (int)(0.08 * BaseMana[74]), MinDamage = 233, MaxDamage = 264, SpellDamageCoefficient = 1.5f / 3.5f * 0.5f * 0.13f };
            SpellData[5] = new SpellData() { Cost = (int)(0.08 * BaseMana[75]), MinDamage = 365, MaxDamage = 415, SpellDamageCoefficient = 1.5f / 3.5f * 0.5f * 0.13f };
            SpellData[6] = new SpellData() { Cost = (int)(0.08 * BaseMana[76]), MinDamage = 365, MaxDamage = 416, SpellDamageCoefficient = 1.5f / 3.5f * 0.5f * 0.13f };
            SpellData[7] = new SpellData() { Cost = (int)(0.08 * BaseMana[77]), MinDamage = 366, MaxDamage = 417, SpellDamageCoefficient = 1.5f / 3.5f * 0.5f * 0.13f };
            SpellData[8] = new SpellData() { Cost = (int)(0.08 * BaseMana[78]), MinDamage = 367, MaxDamage = 418, SpellDamageCoefficient = 1.5f / 3.5f * 0.5f * 0.13f };
            SpellData[9] = new SpellData() { Cost = (int)(0.08 * BaseMana[79]), MinDamage = 368, MaxDamage = 419, SpellDamageCoefficient = 1.5f / 3.5f * 0.5f * 0.13f };
            SpellData[10] = new SpellData() { Cost = (int)(0.08 * BaseMana[80]), MinDamage = 368, MaxDamage = 419, SpellDamageCoefficient = 1.5f / 3.5f * 0.5f * 0.13f };
        }
        private static SpellData GetMaxRankSpellData(CalculationOptionsMage options)
        {
            return SpellData[options.PlayerLevel - 70];
        }

        public FrostNova(CastingState castingState)
            : base("Frost Nova", false, true, true, true, 0, 0, 25, MagicSchool.Frost, GetMaxRankSpellData(castingState.CalculationOptions))
        {
            Calculate(castingState);
        }


        public override void Calculate(CastingState castingState)
        {
            base.Calculate(castingState);
            //Cooldown -= 2 * castingState.CalculationOptions.ImprovedFrostNova;
            CalculateDerivedStats(castingState);
        }
    }

    public class Frostbolt : BaseSpell
    {
        public static SpellData[] SpellData = new SpellData[11];
        static Frostbolt()
        {
            SpellData[0] = new SpellData() { Cost = (int)(0.15 * BaseMana[70]), MinDamage = 630, MaxDamage = 680, SpellDamageCoefficient = 0.95f * 3.0f / 3.5f };
            SpellData[1] = new SpellData() { Cost = (int)(0.15 * BaseMana[71]), MinDamage = 633, MaxDamage = 684, SpellDamageCoefficient = 0.95f * 3.0f / 3.5f };
            SpellData[2] = new SpellData() { Cost = (int)(0.15 * BaseMana[72]), MinDamage = 637, MaxDamage = 688, SpellDamageCoefficient = 0.95f * 3.0f / 3.5f };
            SpellData[3] = new SpellData() { Cost = (int)(0.15 * BaseMana[73]), MinDamage = 641, MaxDamage = 692, SpellDamageCoefficient = 0.95f * 3.0f / 3.5f };
            SpellData[4] = new SpellData() { Cost = (int)(0.15 * BaseMana[74]), MinDamage = 645, MaxDamage = 696, SpellDamageCoefficient = 0.95f * 3.0f / 3.5f };
            SpellData[5] = new SpellData() { Cost = (int)(0.15 * BaseMana[75]), MinDamage = 702, MaxDamage = 758, SpellDamageCoefficient = 0.95f * 3.0f / 3.5f };
            SpellData[6] = new SpellData() { Cost = (int)(0.15 * BaseMana[76]), MinDamage = 706, MaxDamage = 763, SpellDamageCoefficient = 0.95f * 3.0f / 3.5f };
            SpellData[7] = new SpellData() { Cost = (int)(0.15 * BaseMana[77]), MinDamage = 710, MaxDamage = 767, SpellDamageCoefficient = 0.95f * 3.0f / 3.5f };
            SpellData[8] = new SpellData() { Cost = (int)(0.15 * BaseMana[78]), MinDamage = 714, MaxDamage = 771, SpellDamageCoefficient = 0.95f * 3.0f / 3.5f };
            SpellData[9] = new SpellData() { Cost = (int)(0.15 * BaseMana[79]), MinDamage = 799, MaxDamage = 861, SpellDamageCoefficient = 0.95f * 3.0f / 3.5f };
            SpellData[10] = new SpellData() { Cost = (int)(0.15 * BaseMana[80]), MinDamage = 803, MaxDamage = 866, SpellDamageCoefficient = 0.95f * 3.0f / 3.5f };
        }
        private static SpellData GetMaxRankSpellData(CalculationOptionsMage options)
        {
            return SpellData[options.PlayerLevel - 70];
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
            : base("Frostbolt", false, false, false, false, 30, 3, 0, MagicSchool.Frost, GetMaxRankSpellData(castingState.CalculationOptions))
        {
            Calculate(castingState);
        }


        public override void Calculate(CastingState castingState)
        {
            base.Calculate(castingState);
            if (castingState.CalculationOptions.GlyphOfFrostbolt)
            {
                DirectDamageModifier *= 1.05f;
            }
            BaseCastTime -= 0.1f * castingState.MageTalents.ImprovedFrostbolt;
            CritRate += 0.02f * castingState.MageTalents.EmpoweredFrostbolt;
            InterruptProtection += castingState.BaseStats.AldorRegaliaInterruptProtection;
            SpellDamageCoefficient += 0.05f * castingState.MageTalents.EmpoweredFrostbolt;
            SpellModifier *= (1 + castingState.BaseStats.BonusMageNukeMultiplier) * (1 + 0.04f * castingState.MageTalents.TormentTheWeak * castingState.CalculationOptions.SlowedTime);
            CalculateDerivedStats(castingState);
        }
    }

    public class Fireball : BaseSpell
    {
        public static SpellData[] SpellData = new SpellData[11];
        static Fireball()
        {
            SpellData[0] = new SpellData() { Cost = (int)(0.21 * BaseMana[70]), MinDamage = 717, MaxDamage = 913, PeriodicDamage = 92, SpellDamageCoefficient = 3.5f / 3.5f };
            SpellData[1] = new SpellData() { Cost = (int)(0.21 * BaseMana[71]), MinDamage = 721, MaxDamage = 918, PeriodicDamage = 92, SpellDamageCoefficient = 3.5f / 3.5f };
            SpellData[2] = new SpellData() { Cost = (int)(0.21 * BaseMana[72]), MinDamage = 725, MaxDamage = 922, PeriodicDamage = 92, SpellDamageCoefficient = 3.5f / 3.5f };
            SpellData[3] = new SpellData() { Cost = (int)(0.21 * BaseMana[73]), MinDamage = 729, MaxDamage = 926, PeriodicDamage = 92, SpellDamageCoefficient = 3.5f / 3.5f };
            SpellData[4] = new SpellData() { Cost = (int)(0.21 * BaseMana[74]), MinDamage = 783, MaxDamage = 997, PeriodicDamage = 100, SpellDamageCoefficient = 3.5f / 3.5f };
            SpellData[5] = new SpellData() { Cost = (int)(0.21 * BaseMana[75]), MinDamage = 787, MaxDamage = 1002, PeriodicDamage = 100, SpellDamageCoefficient = 3.5f / 3.5f };
            SpellData[6] = new SpellData() { Cost = (int)(0.21 * BaseMana[76]), MinDamage = 792, MaxDamage = 1007, PeriodicDamage = 100, SpellDamageCoefficient = 3.5f / 3.5f };
            SpellData[7] = new SpellData() { Cost = (int)(0.21 * BaseMana[77]), MinDamage = 796, MaxDamage = 1011, PeriodicDamage = 100, SpellDamageCoefficient = 3.5f / 3.5f };
            SpellData[8] = new SpellData() { Cost = (int)(0.21 * BaseMana[78]), MinDamage = 888, MaxDamage = 1132, PeriodicDamage = 116, SpellDamageCoefficient = 3.5f / 3.5f };
            SpellData[9] = new SpellData() { Cost = (int)(0.21 * BaseMana[79]), MinDamage = 893, MaxDamage = 1138, PeriodicDamage = 116, SpellDamageCoefficient = 3.5f / 3.5f };
            SpellData[10] = new SpellData() { Cost = (int)(0.21 * BaseMana[80]), MinDamage = 898, MaxDamage = 1143, PeriodicDamage = 116, SpellDamageCoefficient = 3.5f / 3.5f };
        }
        private static SpellData GetMaxRankSpellData(CalculationOptionsMage options)
        {
            return SpellData[options.PlayerLevel - 70];
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
            if (castingState.CalculationOptions.GlyphOfFireball)
            {
                BasePeriodicDamage = 0.0f;
                DirectDamageModifier *= 1.05f;
            }
            SpammedDot = true;
            DotDuration = 8;
            InterruptProtection += castingState.BaseStats.AldorRegaliaInterruptProtection;
            BaseCastTime -= 0.1f * castingState.MageTalents.ImprovedFireball;
            SpellDamageCoefficient += 0.05f * castingState.MageTalents.EmpoweredFireball;
            SpellModifier *= (1 + castingState.BaseStats.BonusMageNukeMultiplier);
            SpellModifier *= (1 + 0.02f * castingState.MageTalents.SpellImpact) * (1 + 0.04f * castingState.MageTalents.TormentTheWeak * castingState.CalculationOptions.SlowedTime);
            CalculateDerivedStats(castingState);
        }
    }

    public class FrostfireBolt : BaseSpell
    {
        public static SpellData[] SpellData = new SpellData[11];
        static FrostfireBolt()
        {
            SpellData[0] = new SpellData() { Cost = (int)(0.16 * BaseMana[70]), MinDamage = 0, MaxDamage = 0, PeriodicDamage = 0, SpellDamageCoefficient = 0f };
            SpellData[1] = new SpellData() { Cost = (int)(0.16 * BaseMana[71]), MinDamage = 0, MaxDamage = 0, PeriodicDamage = 0, SpellDamageCoefficient = 0f };
            SpellData[2] = new SpellData() { Cost = (int)(0.16 * BaseMana[72]), MinDamage = 0, MaxDamage = 0, PeriodicDamage = 0, SpellDamageCoefficient = 0f };
            SpellData[3] = new SpellData() { Cost = (int)(0.16 * BaseMana[73]), MinDamage = 0, MaxDamage = 0, PeriodicDamage = 0, SpellDamageCoefficient = 0f };
            SpellData[4] = new SpellData() { Cost = (int)(0.16 * BaseMana[74]), MinDamage = 0, MaxDamage = 0, PeriodicDamage = 0, SpellDamageCoefficient = 0f };
            SpellData[5] = new SpellData() { Cost = (int)(0.16 * BaseMana[75]), MinDamage = 629, MaxDamage = 731, PeriodicDamage = 60, SpellDamageCoefficient = 3.0f / 3.5f };
            SpellData[6] = new SpellData() { Cost = (int)(0.16 * BaseMana[76]), MinDamage = 632, MaxDamage = 735, PeriodicDamage = 60, SpellDamageCoefficient = 3.0f / 3.5f };
            SpellData[7] = new SpellData() { Cost = (int)(0.16 * BaseMana[77]), MinDamage = 636, MaxDamage = 739, PeriodicDamage = 60, SpellDamageCoefficient = 3.0f / 3.5f };
            SpellData[8] = new SpellData() { Cost = (int)(0.16 * BaseMana[78]), MinDamage = 640, MaxDamage = 743, PeriodicDamage = 60, SpellDamageCoefficient = 3.0f / 3.5f };
            SpellData[9] = new SpellData() { Cost = (int)(0.16 * BaseMana[79]), MinDamage = 644, MaxDamage = 747, PeriodicDamage = 60, SpellDamageCoefficient = 3.0f / 3.5f };
            SpellData[10] = new SpellData() { Cost = (int)(0.16 * BaseMana[80]), MinDamage = 722, MaxDamage = 838, PeriodicDamage = 90, SpellDamageCoefficient = 3.0f / 3.5f };
        }
        private static SpellData GetMaxRankSpellData(CalculationOptionsMage options)
        {
            return SpellData[options.PlayerLevel - 70];
        }

        public FrostfireBolt(CastingState castingState, bool pom)
            : base("Frostfire Bolt", false, false, false, false, 40, 3.0f, 0, MagicSchool.FrostFire, GetMaxRankSpellData(castingState.CalculationOptions))
        {
            if (pom)
            {
                this.Instant = true;
                this.BaseCastTime = 0.0f;
            }
            Calculate(castingState);
            SpellModifier *= (1 + 0.04f * castingState.MageTalents.TormentTheWeak * castingState.CalculationOptions.SlowedTime);
            SpammedDot = true;
            DotDuration = 9;
            CalculateDerivedStats(castingState);
        }
    }

    public class Pyroblast : BaseSpell
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
            CritRate += 0.02f * castingState.MageTalents.WorldInFlames;
            CalculateDerivedStats(castingState);
        }
    }

    //922-983
    //
    //709 + k*992<=922
    //776 + k*992>=983
    //
    //0.20866935483870967741935483870968 <= k <= 0.21471774193548387096774193548387
    public class ConeOfCold : BaseSpell
    {
        public static SpellData[] SpellData = new SpellData[11];
        static ConeOfCold()
        {
            SpellData[0] = new SpellData() { Cost = (int)(0.29 * BaseMana[70]), MinDamage = 418, MaxDamage = 457, SpellDamageCoefficient = 0.2142f };
            SpellData[1] = new SpellData() { Cost = (int)(0.29 * BaseMana[71]), MinDamage = 418, MaxDamage = 457, SpellDamageCoefficient = 0.2142f };
            SpellData[2] = new SpellData() { Cost = (int)(0.29 * BaseMana[72]), MinDamage = 559, MaxDamage = 611, SpellDamageCoefficient = 0.2142f };
            SpellData[3] = new SpellData() { Cost = (int)(0.29 * BaseMana[73]), MinDamage = 561, MaxDamage = 614, SpellDamageCoefficient = 0.2142f };
            SpellData[4] = new SpellData() { Cost = (int)(0.29 * BaseMana[74]), MinDamage = 563, MaxDamage = 616, SpellDamageCoefficient = 0.2142f };
            SpellData[5] = new SpellData() { Cost = (int)(0.29 * BaseMana[75]), MinDamage = 565, MaxDamage = 618, SpellDamageCoefficient = 0.2142f };
            SpellData[6] = new SpellData() { Cost = (int)(0.29 * BaseMana[76]), MinDamage = 568, MaxDamage = 621, SpellDamageCoefficient = 0.2142f };
            SpellData[7] = new SpellData() { Cost = (int)(0.29 * BaseMana[77]), MinDamage = 568, MaxDamage = 621, SpellDamageCoefficient = 0.2142f };
            SpellData[8] = new SpellData() { Cost = (int)(0.29 * BaseMana[78]), MinDamage = 568, MaxDamage = 621, SpellDamageCoefficient = 0.2142f };
            SpellData[9] = new SpellData() { Cost = (int)(0.29 * BaseMana[79]), MinDamage = 707, MaxDamage = 773, SpellDamageCoefficient = 0.2142f };
            SpellData[10] = new SpellData() { Cost = (int)(0.29 * BaseMana[80]), MinDamage = 709, MaxDamage = 776, SpellDamageCoefficient = 0.2142f };
        }
        private static SpellData GetMaxRankSpellData(CalculationOptionsMage options)
        {
            return SpellData[options.PlayerLevel - 70];
        }

        public ConeOfCold(CastingState castingState)
            : base("Cone of Cold", false, true, true, true, 0, 0, 10, MagicSchool.Frost, GetMaxRankSpellData(castingState.CalculationOptions))
        {
            base.Calculate(castingState);
            AoeDamageCap = 6500;
            int ImprovedConeOfCold = castingState.MageTalents.ImprovedConeOfCold;
            SpellModifier *= (1 + ((ImprovedConeOfCold > 0) ? (0.05f + 0.1f * ImprovedConeOfCold) : 0) + 0.02f * castingState.MageTalents.SpellImpact);
            CritRate += 0.02f * castingState.MageTalents.Incineration;
            CalculateDerivedStats(castingState);
        }
    }

    public class ArcaneBarrage : BaseSpell
    {
        public static SpellData[] SpellData = new SpellData[11];
        static ArcaneBarrage()
        {
            SpellData[0] = new SpellData() { Cost = (int)(0.18 * BaseMana[70]), MinDamage = 709, MaxDamage = 865, SpellDamageCoefficient = 3.0f / 3.5f };
            SpellData[1] = new SpellData() { Cost = (int)(0.18 * BaseMana[71]), MinDamage = 724, MaxDamage = 881, SpellDamageCoefficient = 3.0f / 3.5f };
            SpellData[2] = new SpellData() { Cost = (int)(0.18 * BaseMana[72]), MinDamage = 740, MaxDamage = 896, SpellDamageCoefficient = 3.0f / 3.5f };
            SpellData[3] = new SpellData() { Cost = (int)(0.18 * BaseMana[73]), MinDamage = 755, MaxDamage = 912, SpellDamageCoefficient = 3.0f / 3.5f };
            SpellData[4] = new SpellData() { Cost = (int)(0.18 * BaseMana[74]), MinDamage = 771, MaxDamage = 927, SpellDamageCoefficient = 3.0f / 3.5f };
            SpellData[5] = new SpellData() { Cost = (int)(0.18 * BaseMana[75]), MinDamage = 786, MaxDamage = 943, SpellDamageCoefficient = 3.0f / 3.5f };
            SpellData[6] = new SpellData() { Cost = (int)(0.18 * BaseMana[76]), MinDamage = 802, MaxDamage = 958, SpellDamageCoefficient = 3.0f / 3.5f };
            SpellData[7] = new SpellData() { Cost = (int)(0.18 * BaseMana[77]), MinDamage = 802, MaxDamage = 958, SpellDamageCoefficient = 3.0f / 3.5f };
            SpellData[8] = new SpellData() { Cost = (int)(0.18 * BaseMana[78]), MinDamage = 802, MaxDamage = 958, SpellDamageCoefficient = 3.0f / 3.5f };
            SpellData[9] = new SpellData() { Cost = (int)(0.18 * BaseMana[79]), MinDamage = 802, MaxDamage = 958, SpellDamageCoefficient = 0.95f * 3.0f / 3.5f }; // downranking penalty
            SpellData[10] = new SpellData() { Cost = (int)(0.18 * BaseMana[80]), MinDamage = 936, MaxDamage = 1144, SpellDamageCoefficient = 3.0f / 3.5f };
        }
        private static SpellData GetMaxRankSpellData(CalculationOptionsMage options)
        {
            return SpellData[options.PlayerLevel - 70];
        }

        public ArcaneBarrage(CastingState castingState)
            : base("Arcane Barrage", false, false, true, false, 30, 0, 3, MagicSchool.Arcane, GetMaxRankSpellData(castingState.CalculationOptions))
        {
            Calculate(castingState);
            SpellModifier *= (1 + 0.04f * castingState.MageTalents.TormentTheWeak * castingState.CalculationOptions.SlowedTime);
            CalculateDerivedStats(castingState);
        }
    }

    public class ArcaneBlast : BaseSpell
    {
        public static SpellData[] SpellData = new SpellData[11];
        static ArcaneBlast()
        {
            SpellData[0] = new SpellData() { Cost = (int)(0.09 * BaseMana[70]), MinDamage = 668, MaxDamage = 772, SpellDamageCoefficient = 2.5f / 3.5f };
            SpellData[1] = new SpellData() { Cost = (int)(0.09 * BaseMana[71]), MinDamage = 690, MaxDamage = 800, SpellDamageCoefficient = 2.5f / 3.5f };
            SpellData[2] = new SpellData() { Cost = (int)(0.09 * BaseMana[72]), MinDamage = 695, MaxDamage = 806, SpellDamageCoefficient = 2.5f / 3.5f };
            SpellData[3] = new SpellData() { Cost = (int)(0.09 * BaseMana[73]), MinDamage = 700, MaxDamage = 811, SpellDamageCoefficient = 2.5f / 3.5f };
            SpellData[4] = new SpellData() { Cost = (int)(0.09 * BaseMana[74]), MinDamage = 705, MaxDamage = 816, SpellDamageCoefficient = 2.5f / 3.5f };
            SpellData[5] = new SpellData() { Cost = (int)(0.09 * BaseMana[75]), MinDamage = 711, MaxDamage = 822, SpellDamageCoefficient = 2.5f / 3.5f };
            SpellData[6] = new SpellData() { Cost = (int)(0.09 * BaseMana[76]), MinDamage = 805, MaxDamage = 935, SpellDamageCoefficient = 2.5f / 3.5f };
            SpellData[7] = new SpellData() { Cost = (int)(0.09 * BaseMana[77]), MinDamage = 811, MaxDamage = 942, SpellDamageCoefficient = 2.5f / 3.5f };
            SpellData[8] = new SpellData() { Cost = (int)(0.09 * BaseMana[78]), MinDamage = 817, MaxDamage = 948, SpellDamageCoefficient = 2.5f / 3.5f };
            SpellData[9] = new SpellData() { Cost = (int)(0.09 * BaseMana[79]), MinDamage = 823, MaxDamage = 954, SpellDamageCoefficient = 2.5f / 3.5f };
            SpellData[10] = new SpellData() { Cost = (int)(0.09 * BaseMana[80]), MinDamage = 912, MaxDamage = 1058, SpellDamageCoefficient = 2.5f / 3.5f };
        }
        private static SpellData GetMaxRankSpellData(CalculationOptionsMage options)
        {
            return SpellData[options.PlayerLevel - 70];
        }

        public ArcaneBlast(CastingState castingState, int timeDebuff, int costDebuff, bool manualClearcasting, bool clearcastingActive, bool pom)
            : base("Arcane Blast", false, false, false, false, 30, 2.5f, 0, MagicSchool.Arcane, GetMaxRankSpellData(castingState.CalculationOptions))
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

        public ArcaneBlast(CastingState castingState, int timeDebuff, int costDebuff, bool forceHit)
            : base("Arcane Blast", false, false, false, false, 30, 2.5f, 0, MagicSchool.Arcane, GetMaxRankSpellData(castingState.CalculationOptions))
        {
            if (forceHit)
            {
                ForceHit = true;
            }
            else
            {
                ForceMiss = true;
            }
            this.timeDebuff = timeDebuff;
            this.costDebuff = costDebuff;
            Calculate(castingState);
        }


        public ArcaneBlast(CastingState castingState, int timeDebuff, int costDebuff) : base("Arcane Blast", false, false, false, false, 30, 2.5f, 0, MagicSchool.Arcane, GetMaxRankSpellData(castingState.CalculationOptions))
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
            InterruptProtection += 0.2f * castingState.MageTalents.ArcaneStability;
            CostModifier += 2.00f * costDebuff + castingState.BaseStats.ArcaneBlastBonus * 0.25f;
            SpellModifier *= (1 + castingState.BaseStats.ArcaneBlastBonus * 0.25f + 0.15f * timeDebuff + 0.02f * castingState.MageTalents.SpellImpact);
            SpellDamageCoefficient += 0.03f * castingState.MageTalents.ArcaneEmpowerment;
            CritRate += 0.02f * castingState.MageTalents.Incineration;
            //CritRate += 0.02f * castingState.CalculationOptions.ArcaneImpact;
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
    //
    // rank 11: 71 (with k = kbase + 0.45/5
    //792/(1.03*1.03*1.05) <= x + k*1182 <= 793/(1.03*1.03*1.05)
    //665/(1.03*1.03*1.05) <= x + k*864 <= 666/(1.03*1.03*1.05)
    //476/(1.03*1.03*1.05) <= x + k*389 <= 477/(1.03*1.03*1.05)
    //319/(1.03*1.03) <= x + k*35 <= 320/(1.03*1.03)
    //289 <= x <= 290
    //297/1.03 <= x <= 298/1.03
    //306/(1.03*1.03) <= x <= 307/(1.03*1.03)

    //710.98662860374614545601443518307 <= x + k*1182 <= 711.88433899339734008411546351032
    //596.97740911804442768718383762214 <= x + k*864 <= 597.8751195076956223152848659494
    //427.31014547396864297608948377164 <= x + k*389 <= 428.20785586361983760419051209889
    //300.68809501366764068243943821284 <= x + k*35 <= 301.63069092280139504194551795645

    //409.3559376809447504140689172266 <= k*1147 <= 411.1962439797296994016760252975
    //295.3467181952430326452383196657 <= k*829 <= 297.1870244940279816328454277366
    //125.6794545511672479341439658152 <= k*354 <= 127.519760849952196921751073886

    //0.3568927093992543595589092565184 <= k <= 0.35849716127265013025429470383391
    //0.3562686588603655399821933892228 <= k <= 0.35848856995660793924348061246876
    //0.3550267077716588924693332367661 <= k <= 0.36022531313545818339477704487571

    //0.3568927093992543595589092565184 <= k <= 0.35848856995660793924348061246876
    //0.266892709399254359558909256518 <= kraw <= 0.268488569956607939243480612468
    //0.934124482897390258456182397813 <= kbase <= 0.939709994848127787352182143638

    //4.670622414486951292280911989065 <= kbase <= 4.69854997424063893676091071819

    //k := 0.35692857142857142857142857142857

    //289.0970571751747168845858637551 <= x <= 289.9947675648259115126868920824
    //288.59112340375871340146955190831 <= x <= 289.48883379340990802957058023561
    //288.46493118825435726180376948611 <= x <= 289.36264157790555188990479781331
    //288.19559501366764068243943821282 <= x <= 289.13819092280139504194551795642

    //289.0970571751747168845858637551 <= x <= 289.13819092280139504194551795642

    // x := 289.1

    // level 72
    // 290 <= x <= 291
    // 299/1.03 <= x <= 300/1.03

    // 290.31954001319634272787256103309 <= x <= 291

    // 308/(1.03*1.03) <= x <= 309/(1.03*1.03)
    // 321/(1.03*1.03) <= x + k*36 <= 322/(1.03*1.03) (very heavily slanted towards 321)
    // 476/(1.03*1.03) <= x + k*468 <= 477/(1.03*1.03)
    // 769/(1.03*1.03*1.05) <= x + k*1182 <= 770/(1.03*1.03*1.05)

    // 0.32148018977597637226254438055556 <= k <= 0.36656507577423780647458656696111
    // 0.33691378792236554513866230333376 <= k <= 0.34038185607607796315497324074957
    // 0.3378504988509041192975387340577 <= k <= 0.33918566837413157437387415478046

    // 0.3378504988509041192975387340577 <= k <= 0.33918566837413157437387415478046

    // k := 0.33814285714285714285714285714286

    // 290.40014397479229225859445484289 <= x <= 291.34273988392604661810053458649
    // 290.42479560480993226775110081774 <= x <= 291.36739151394368662725718056134
    // 290.65443249891152615254792651436 <= x <= 291.55214288856272078064895484166

    // 290.65443249891152615254792651436 <= x <= 291

    // x := 290.8

    // level 73
    // 291 <= x <= 292
    // 300/1.03 <= x <= 301/1.03
    // 309/(1.03*1.03) <= x <= 310/(1.03*1.03)

    // 291.2621359223300970873786407767 <= x <= 292

    // k := 0.85*(5/3.5+0.45)/5 = 0.31935714285714285714285714285714

    // 612/(1.03*1.03*1.05) <= 257.67142857142857142857142857143 + x <= 613/(1.03*1.03*1.05)

    // => 291.72732989510254096925790770642 <= x <= 292

    // 702/(1.03*1.03*1.05) <= 338.83792857142857142857142857128 + x <= 703/(1.03*1.03*1.05)

    // x := 291.9
    public class ArcaneMissiles : BaseSpell
    {
        private bool barrage;

        public static SpellData[] SpellData = new SpellData[11];
        static ArcaneMissiles()
        {
            SpellData[0] = new SpellData() { Cost = (int)(0.34 * BaseMana[70]), MinDamage = 287.9f * 5, MaxDamage = 287.9f * 5, SpellDamageCoefficient = 5f / 3.5f }; // there's some indication that coefficient might be slightly different
            SpellData[1] = new SpellData() { Cost = (int)(0.34 * BaseMana[71]), MinDamage = 289.1f * 5, MaxDamage = 289.1f * 5, SpellDamageCoefficient = 4.67125f / 3.5f }; // some huge downraking style penalty for some reason (seems to be 0.95 * (5/3.5 + 0.45)), for now don't place the coeff on 0.45, just use 4.67125 instead of 4.75
            SpellData[2] = new SpellData() { Cost = (int)(0.34 * BaseMana[72]), MinDamage = 290.8f * 5, MaxDamage = 290.8f * 5, SpellDamageCoefficient = 4.3425f / 3.5f }; // some huge downraking style penalty for some reason (hypothesis 0.9 * (5/3.5 + 0.45), confirmed)
            SpellData[3] = new SpellData() { Cost = (int)(0.34 * BaseMana[73]), MinDamage = 291.9f * 5, MaxDamage = 291.9f * 5, SpellDamageCoefficient = 4.01375f / 3.5f }; // some huge downraking style penalty for some reason (hypothesis 0.85 * (5/3.5 + 0.45), confirmed)
            SpellData[4] = new SpellData() { Cost = (int)(0.34 * BaseMana[74]), MinDamage = 293.0f * 5, MaxDamage = 293.0f * 5, SpellDamageCoefficient = 3.685f / 3.5f }; // some huge downraking style penalty for some reason (hypothesis 0.8 * (5/3.5 + 0.45), confirmed)
            SpellData[5] = new SpellData() { Cost = (int)(0.34 * BaseMana[75]), MinDamage = 320.0f * 5, MaxDamage = 320.0f * 5, SpellDamageCoefficient = 5f / 3.5f };
            SpellData[6] = new SpellData() { Cost = (int)(0.34 * BaseMana[76]), MinDamage = 321.7f * 5, MaxDamage = 321.7f * 5, SpellDamageCoefficient = 5f / 3.5f };
            SpellData[7] = new SpellData() { Cost = (int)(0.34 * BaseMana[77]), MinDamage = 323.0f * 5, MaxDamage = 323.0f * 5, SpellDamageCoefficient = 5f / 3.5f };
            SpellData[8] = new SpellData() { Cost = (int)(0.34 * BaseMana[78]), MinDamage = 324.8f * 5, MaxDamage = 324.8f * 5, SpellDamageCoefficient = 5f / 3.5f };
            SpellData[9] = new SpellData() { Cost = (int)(0.34 * BaseMana[79]), MinDamage = 360.0f * 5, MaxDamage = 360.0f * 5, SpellDamageCoefficient = 5f / 3.5f };
            SpellData[10] = new SpellData() { Cost = (int)(0.34 * BaseMana[80]), MinDamage = 361.9f * 5, MaxDamage = 361.9f * 5, SpellDamageCoefficient = 5f / 3.5f };
        }
        private static SpellData GetMaxRankSpellData(CalculationOptionsMage options)
        {
            return SpellData[options.PlayerLevel - 70];
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
            SpellDamageCoefficient += 0.15f * castingState.MageTalents.ArcaneEmpowerment;
            SpellModifier *= (1 + castingState.BaseStats.BonusMageNukeMultiplier) * (1 + 0.04f * castingState.MageTalents.TormentTheWeak * castingState.CalculationOptions.SlowedTime);
            InterruptProtection += 0.2f * castingState.MageTalents.ArcaneStability;
            CalculateDerivedStats(castingState);
        }
    }

    public class ArcaneExplosion : BaseSpell
    {
        public static SpellData[] SpellData = new SpellData[11];
        static ArcaneExplosion()
        {
            SpellData[0] = new SpellData() { Cost = (int)(0.25 * BaseMana[70]), MinDamage = 377, MaxDamage = 407, SpellDamageCoefficient = 1.5f / 3.5f * 0.5f };
            SpellData[1] = new SpellData() { Cost = (int)(0.25 * BaseMana[71]), MinDamage = 378, MaxDamage = 409, SpellDamageCoefficient = 1.5f / 3.5f * 0.5f };
            SpellData[2] = new SpellData() { Cost = (int)(0.25 * BaseMana[72]), MinDamage = 380, MaxDamage = 411, SpellDamageCoefficient = 1.5f / 3.5f * 0.5f };
            SpellData[3] = new SpellData() { Cost = (int)(0.25 * BaseMana[73]), MinDamage = 381, MaxDamage = 412, SpellDamageCoefficient = 1.5f / 3.5f * 0.5f };
            SpellData[4] = new SpellData() { Cost = (int)(0.25 * BaseMana[74]), MinDamage = 383, MaxDamage = 414, SpellDamageCoefficient = 1.5f / 3.5f * 0.5f };
            SpellData[5] = new SpellData() { Cost = (int)(0.25 * BaseMana[75]), MinDamage = 385, MaxDamage = 415, SpellDamageCoefficient = 1.5f / 3.5f * 0.5f };
            SpellData[6] = new SpellData() { Cost = (int)(0.25 * BaseMana[76]), MinDamage = 481, MaxDamage = 519, SpellDamageCoefficient = 1.5f / 3.5f * 0.5f };
            SpellData[7] = new SpellData() { Cost = (int)(0.25 * BaseMana[77]), MinDamage = 483, MaxDamage = 521, SpellDamageCoefficient = 1.5f / 3.5f * 0.5f };
            SpellData[8] = new SpellData() { Cost = (int)(0.25 * BaseMana[78]), MinDamage = 485, MaxDamage = 523, SpellDamageCoefficient = 1.5f / 3.5f * 0.5f };
            SpellData[9] = new SpellData() { Cost = (int)(0.25 * BaseMana[79]), MinDamage = 487, MaxDamage = 525, SpellDamageCoefficient = 1.5f / 3.5f * 0.5f };
            SpellData[10] = new SpellData() { Cost = (int)(0.25 * BaseMana[80]), MinDamage = 538, MaxDamage = 582, SpellDamageCoefficient = 1.5f / 3.5f * 0.5f };
        }
        private static SpellData GetMaxRankSpellData(CalculationOptionsMage options)
        {
            return SpellData[options.PlayerLevel - 70];
        }

        public ArcaneExplosion(CastingState castingState)
            : base("Arcane Explosion", false, false, true, true, 0, 0, 0, MagicSchool.Arcane, GetMaxRankSpellData(castingState.CalculationOptions))
        {
            base.Calculate(castingState);
            if (castingState.CalculationOptions.GlyphOfArcaneExplosion) CostAmplifier *= 0.9f;
            CritRate += 0.02f * castingState.MageTalents.WorldInFlames;
            SpellModifier *= (1 + 0.02f * castingState.MageTalents.SpellImpact); // bugged currently
            AoeDamageCap = 10180;
            CalculateDerivedStats(castingState);
        }
    }

    public class BlastWave : BaseSpell
    {
        public BlastWave(CastingState castingState)
            : base("Blast Wave", false, false, true, true, 645, 0, 0, 30, MagicSchool.Fire, 616, 724, 0, 0.1357f)
        {
            base.Calculate(castingState);
            AoeDamageCap = 9440;
            SpellModifier *= (1 + 0.02f * castingState.MageTalents.SpellImpact);
            CritRate += 0.02f * castingState.MageTalents.WorldInFlames;
            CalculateDerivedStats(castingState);
        }
    }

    public class DragonsBreath : BaseSpell
    {
        public DragonsBreath(CastingState castingState)
            : base("Dragon's Breath", false, false, true, true, 700, 0, 0, 20, MagicSchool.Fire, 680, 790, 0, 0.1357f)
        {
            base.Calculate(castingState);
            AoeDamageCap = 10100;
            CritRate += 0.02f * castingState.MageTalents.WorldInFlames;
            CalculateDerivedStats(castingState);
        }
    }

    public class Blizzard : BaseSpell
    {
        public static SpellData[] SpellData = new SpellData[11];
        static Blizzard()
        {
            SpellData[0] = new SpellData() { Cost = (int)(0.74 * BaseMana[70]), MinDamage = 1476, MaxDamage = 1476, SpellDamageCoefficient = 1.1429f }; // TODO verify level 70 WotLK data
            SpellData[1] = new SpellData() { Cost = (int)(0.74 * BaseMana[71]), MinDamage = 2192, MaxDamage = 2192, SpellDamageCoefficient = 1.1429f };
            SpellData[2] = new SpellData() { Cost = (int)(0.74 * BaseMana[72]), MinDamage = 2192, MaxDamage = 2192, SpellDamageCoefficient = 1.1429f };
            SpellData[3] = new SpellData() { Cost = (int)(0.74 * BaseMana[73]), MinDamage = 2200, MaxDamage = 2200, SpellDamageCoefficient = 1.1429f };
            SpellData[4] = new SpellData() { Cost = (int)(0.74 * BaseMana[74]), MinDamage = 2800, MaxDamage = 2800, SpellDamageCoefficient = 1.1429f };
            SpellData[5] = new SpellData() { Cost = (int)(0.74 * BaseMana[75]), MinDamage = 2800, MaxDamage = 2800, SpellDamageCoefficient = 1.1429f };
            SpellData[6] = new SpellData() { Cost = (int)(0.74 * BaseMana[76]), MinDamage = 2808, MaxDamage = 2808, SpellDamageCoefficient = 1.1429f };
            SpellData[7] = new SpellData() { Cost = (int)(0.74 * BaseMana[77]), MinDamage = 2808, MaxDamage = 2808, SpellDamageCoefficient = 1.1429f };
            SpellData[8] = new SpellData() { Cost = (int)(0.74 * BaseMana[78]), MinDamage = 2816, MaxDamage = 2816, SpellDamageCoefficient = 1.1429f };
            SpellData[9] = new SpellData() { Cost = (int)(0.74 * BaseMana[79]), MinDamage = 2816, MaxDamage = 2816, SpellDamageCoefficient = 1.1429f };
            SpellData[10] = new SpellData() { Cost = (int)(0.74 * BaseMana[80]), MinDamage = 3408, MaxDamage = 3408, SpellDamageCoefficient = 1.1429f };
        }
        private static SpellData GetMaxRankSpellData(CalculationOptionsMage options)
        {
            return SpellData[options.PlayerLevel - 70];
        }

        public Blizzard(CastingState castingState)
            : base("Blizzard", true, false, false, true, 0, 8, 0, MagicSchool.Frost, GetMaxRankSpellData(castingState.CalculationOptions))
        {
            base.Calculate(castingState);
            AoeDamageCap = 28950;
            CritRate += 0.02f * castingState.MageTalents.WorldInFlames;
            CalculateDerivedStats(castingState);
        }
    }

    // lightning capacitor
    public class LightningBolt : BaseSpell
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
    public class ArcaneBolt : BaseSpell
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
                if (sequence == null) sequence = string.Join("-", spellList.ConvertAll<string>(spell => (spell != null) ? spell.Name : "Pause").ToArray());
                return sequence;
            }
        }

        public float AverageDamage;
        public float AverageThreat;
        public float Cost;
        public float SpellCount = 0;

        private List<Spell> spellList;
        private FSRCalc fsr;

        public SpellCycle()
        {
            spellList = new List<Spell>();
            fsr = new FSRCalc();
        }

        public SpellCycle(int capacity)
        {
            spellList = new List<Spell>(capacity);
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
            spellList.Add(spell);
            SpellCount++;
        }

        public void AddPause(float duration)
        {
            fsr.AddPause(duration);
            spellList.Add(null);
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

        public override void AddSpellContribution(Dictionary<string, SpellContribution> dict, float duration)
        {
            foreach (Spell spell in spellList)
            {
                if (spell != null)
                {
                    spell.AddSpellContribution(dict, spell.CastTime * duration / CastTime);
                }
            }
        }
    }

    #region Spell Cycles

    class ArcaneMissilesCC : Spell
    {
        Spell AMc1;
        Spell AM10;
        Spell AM11;
        float CC;

        public ArcaneMissilesCC(CastingState castingState)
        {
            Name = "Arcane Missiles CC";

            //AM?1-AM11-AM11-...=0.9*0.1*...
            //...
            //AM?1-AM10=0.9

            //TIME = T * [1 + 1/0.9]
            //DAMAGE = AM?1 + AM10 + 0.1/0.9*AM11

            CC = 0.02f * castingState.MageTalents.ArcaneConcentration;

            AMc1 = new ArcaneMissiles(castingState, false, true, false, true);
            AM10 = new ArcaneMissiles(castingState, false, false, true, false);
            AM11 = new ArcaneMissiles(castingState, false, false, true, true);

            CastProcs = AMc1.CastProcs * (1 + 1 / (1 - CC));
            CastTime = AMc1.CastTime * (1 + 1 / (1 - CC));
            HitProcs = AMc1.HitProcs * (1 + 1 / (1 - CC));
            Channeled = true;
            CostPerSecond = (AMc1.CostPerSecond + AM10.CostPerSecond + CC / (1 - CC) * AM11.CostPerSecond) / (1 + 1 / (1 - CC));
            DamagePerSecond = (AMc1.DamagePerSecond + AM10.DamagePerSecond + CC / (1 - CC) * AM11.DamagePerSecond) / (1 + 1 / (1 - CC));
            ThreatPerSecond = (AMc1.ThreatPerSecond + AM10.ThreatPerSecond + CC / (1 - CC) * AM11.ThreatPerSecond) / (1 + 1 / (1 - CC));
            //ManaRegenPerSecond = (AMc1.ManaRegenPerSecond + AM10.ManaRegenPerSecond + CC / (1 - CC) * AM11.ManaRegenPerSecond) / (1 + 1 / (1 - CC)); // we only use it indirectly in spell cycles that recompute oo5sr
        }

        public override void AddSpellContribution(Dictionary<string, SpellContribution> dict, float duration)
        {
            AMc1.AddSpellContribution(dict, duration * 1 / (1 + 1 / (1 - CC)));
            AM10.AddSpellContribution(dict, duration * 1 / (1 + 1 / (1 - CC)));
            AM11.AddSpellContribution(dict, duration * CC / (1 - CC) / (1 + 1 / (1 - CC)));
        }
    }

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

    class ABP : SpellCycle
    {
        public ABP(CastingState castingState)
            : base(3)
        {
            Name = "ABP";
            ABCycle = true;

            Spell AB = castingState.GetSpell(SpellId.ArcaneBlast00);

            AddSpell(AB, castingState);
            AddPause(3 - AB.CastTime + castingState.Latency);

            Calculate(castingState);
        }
    }

    class ABAM : Spell
    {
        SpellCycle chain1;
        SpellCycle chain2;
        float MB;

        public ABAM(CastingState castingState)
        {
            Name = "ABAM";
            ABCycle = true;

            Spell AB = castingState.GetSpell(SpellId.ArcaneBlast00);
            Spell AM = castingState.GetSpell(SpellId.ArcaneMissiles);
            Spell MBAM = castingState.GetSpell(SpellId.ArcaneMissilesMB);

            MB = 0.04f * castingState.MageTalents.MissileBarrage;

            if (MB == 0.0)
            {
                // if we don't have barrage then this degenerates to AB-AM
                chain1 = new SpellCycle(2);
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
                chain1 = new SpellCycle(2);
                chain1.AddSpell(AB, castingState);
                chain1.AddSpell(AM, castingState);
                chain1.Calculate(castingState);

                //AB-MBAM 0.15
                chain2 = new SpellCycle(2);
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

        public override void AddSpellContribution(Dictionary<string, SpellContribution> dict, float duration)
        {
            if (chain2 == null)
            {
                chain1.AddSpellContribution(dict, duration);
            }
            else
            {
                chain1.AddSpellContribution(dict, (1 - MB) * chain1.CastTime / CastTime * duration);
                chain2.AddSpellContribution(dict, MB * chain2.CastTime / CastTime * duration);
            }
        }
    }

    class ABarAM : Spell
    {
        SpellCycle chain1;
        SpellCycle chain2;
        float MB;

        public ABarAM(CastingState castingState)
        {
            Name = "ABarAM";
            ABCycle = true;

            Spell ABar = castingState.GetSpell(SpellId.ArcaneBarrage);
            Spell AM = castingState.GetSpell(SpellId.ArcaneMissiles);
            Spell MBAM = castingState.GetSpell(SpellId.ArcaneMissilesMB);

            MB = 0.04f * castingState.MageTalents.MissileBarrage;

            if (MB == 0.0)
            {
                // if we don't have barrage then this degenerates to ABar-AM
                chain1 = new SpellCycle(2);
                chain1.AddSpell(ABar, castingState);
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
                chain1 = new SpellCycle(2);
                chain1.AddSpell(ABar, castingState);
                chain1.AddSpell(AM, castingState);
                chain1.Calculate(castingState);

                //AB-MBAM 0.15
                chain2 = new SpellCycle(2);
                chain2.AddSpell(ABar, castingState);
                chain2.AddSpell(MBAM, castingState);
                chain2.Calculate(castingState);

                CastTime = (1 - MB) * chain1.CastTime + MB * chain2.CastTime;
                CostPerSecond = ((1 - MB) * chain1.CastTime * chain1.CostPerSecond + MB * chain2.CastTime * chain2.CostPerSecond) / CastTime;
                DamagePerSecond = ((1 - MB) * chain1.CastTime * chain1.DamagePerSecond + MB * chain2.CastTime * chain2.DamagePerSecond) / CastTime;
                ThreatPerSecond = ((1 - MB) * chain1.CastTime * chain1.ThreatPerSecond + MB * chain2.CastTime * chain2.ThreatPerSecond) / CastTime;
                ManaRegenPerSecond = ((1 - MB) * chain1.CastTime * chain1.ManaRegenPerSecond + MB * chain2.CastTime * chain2.ManaRegenPerSecond) / CastTime;
            }
        }

        public override void AddSpellContribution(Dictionary<string, SpellContribution> dict, float duration)
        {
            if (chain2 == null)
            {
                chain1.AddSpellContribution(dict, duration);
            }
            else
            {
                chain1.AddSpellContribution(dict, (1 - MB) * chain1.CastTime / CastTime * duration);
                chain2.AddSpellContribution(dict, MB * chain2.CastTime / CastTime * duration);
            }
        }
    }

    class ABABar : Spell
    {
        SpellCycle chain1;
        SpellCycle chain2;
        float MB;

        public ABABar(CastingState castingState)
        {
            Name = "ABABar";
            ABCycle = true;

            Spell AB = castingState.GetSpell(SpellId.ArcaneBlast00);
            Spell ABar = castingState.GetSpell(SpellId.ArcaneBarrage);
            Spell MBAM = castingState.GetSpell(SpellId.ArcaneMissilesMB);

            MB = 0.04f * castingState.MageTalents.MissileBarrage;

            if (MB == 0.0)
            {
                // if we don't have barrage then this degenerates to AB-ABar
                chain1 = new SpellCycle(2);
                chain1.AddSpell(AB, castingState);
                chain1.AddSpell(ABar, castingState);
                chain1.Calculate(castingState);

                commonChain = chain1;

                CastTime = chain1.CastTime;
                CostPerSecond = chain1.CostPerSecond;
                DamagePerSecond = chain1.DamagePerSecond;
                ThreatPerSecond = chain1.ThreatPerSecond;
                ManaRegenPerSecond = chain1.ManaRegenPerSecond;
            }
            else
            {
                // ABar-MBAM         0.2
                // ABar-AB           0.8*0.8
                // ABar-AB-ABar-MBAM 0.8*0.2

                // ABar-MBAM         (2-MB)*MB
                // ABar-AB           (1-MB)
                
                //AB-ABar 0.8 * 0.8
                chain1 = new SpellCycle(2);
                chain1.AddSpell(ABar, castingState);
                chain1.AddSpell(AB, castingState);
                chain1.Calculate(castingState);

                //ABar-MBAM
                chain2 = new SpellCycle(2);
                chain2.AddSpell(ABar, castingState);
                chain2.AddSpell(MBAM, castingState);
                chain2.Calculate(castingState);

                commonChain = chain1;

                CastTime = (1 - MB) * chain1.CastTime + (2 - MB) * MB * chain2.CastTime;
                CostPerSecond = ((1 - MB) * chain1.CastTime * chain1.CostPerSecond + (2 - MB) * MB * chain2.CastTime * chain2.CostPerSecond) / CastTime;
                DamagePerSecond = ((1 - MB) * chain1.CastTime * chain1.DamagePerSecond + (2 - MB) * MB * chain2.CastTime * chain2.DamagePerSecond) / CastTime;
                ThreatPerSecond = ((1 - MB) * chain1.CastTime * chain1.ThreatPerSecond + (2 - MB) * MB * chain2.CastTime * chain2.ThreatPerSecond) / CastTime;
                ManaRegenPerSecond = ((1 - MB) * chain1.CastTime * chain1.ManaRegenPerSecond + (2 - MB) * MB * chain2.CastTime * chain2.ManaRegenPerSecond) / CastTime;
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

        public override void AddSpellContribution(Dictionary<string, SpellContribution> dict, float duration)
        {
            if (chain2 == null)
            {
                chain1.AddSpellContribution(dict, duration);
            }
            else
            {
                chain1.AddSpellContribution(dict, (1 - MB) * chain1.CastTime / CastTime * duration);
                chain2.AddSpellContribution(dict, (2 - MB) * MB * chain2.CastTime / CastTime * duration);
            }
        }
    }

    class FBABar : Spell
    {
        SpellCycle chain1;
        SpellCycle chain2;
        float MB;

        public FBABar(CastingState castingState)
        {
            Name = "FBABar";
            ABCycle = true;

            Spell FB = castingState.GetSpell(SpellId.Fireball);
            Spell ABar = castingState.GetSpell(SpellId.ArcaneBarrage);
            Spell MBAM = castingState.GetSpell(SpellId.ArcaneMissilesMB);

            MB = 0.04f * castingState.MageTalents.MissileBarrage;

            if (MB == 0.0)
            {
                // if we don't have barrage then this degenerates to FB-ABar
                chain1 = new SpellCycle(2);
                chain1.AddSpell(FB, castingState);
                chain1.AddSpell(ABar, castingState);
                chain1.Calculate(castingState);

                commonChain = chain1;

                CastTime = chain1.CastTime;
                CostPerSecond = chain1.CostPerSecond;
                DamagePerSecond = chain1.DamagePerSecond;
                ThreatPerSecond = chain1.ThreatPerSecond;
                ManaRegenPerSecond = chain1.ManaRegenPerSecond;
            }
            else
            {
                chain1 = new SpellCycle(2);
                chain1.AddSpell(ABar, castingState);
                chain1.AddSpell(FB, castingState);
                chain1.Calculate(castingState);

                chain2 = new SpellCycle(2);
                chain2.AddSpell(ABar, castingState);
                chain2.AddSpell(MBAM, castingState);
                chain2.Calculate(castingState);

                commonChain = chain2;

                CastTime = (1 - MB) * chain1.CastTime + (2 - MB) * MB * chain2.CastTime;
                CostPerSecond = ((1 - MB) * chain1.CastTime * chain1.CostPerSecond + (2 - MB) * MB * chain2.CastTime * chain2.CostPerSecond) / CastTime;
                DamagePerSecond = ((1 - MB) * chain1.CastTime * chain1.DamagePerSecond + (2 - MB) * MB * chain2.CastTime * chain2.DamagePerSecond) / CastTime;
                ThreatPerSecond = ((1 - MB) * chain1.CastTime * chain1.ThreatPerSecond + (2 - MB) * MB * chain2.CastTime * chain2.ThreatPerSecond) / CastTime;
                ManaRegenPerSecond = ((1 - MB) * chain1.CastTime * chain1.ManaRegenPerSecond + (2 - MB) * MB * chain2.CastTime * chain2.ManaRegenPerSecond) / CastTime;
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

        public override void AddSpellContribution(Dictionary<string, SpellContribution> dict, float duration)
        {
            if (chain2 == null)
            {
                chain1.AddSpellContribution(dict, duration);
            }
            else
            {
                chain1.AddSpellContribution(dict, (1 - MB) * chain1.CastTime / CastTime * duration);
                chain2.AddSpellContribution(dict, (2 - MB) * MB * chain2.CastTime / CastTime * duration);
            }
        }
    }

    class FrBABar : Spell
    {
        SpellCycle chain1;
        SpellCycle chain2;
        float MB;

        public FrBABar(CastingState castingState)
        {
            Name = "FrBABar";
            ABCycle = true;

            Spell FrB = castingState.GetSpell(SpellId.Frostbolt);
            Spell ABar = castingState.GetSpell(SpellId.ArcaneBarrage);
            Spell MBAM = castingState.GetSpell(SpellId.ArcaneMissilesMB);

            MB = 0.04f * castingState.MageTalents.MissileBarrage;

            if (MB == 0.0)
            {
                // if we don't have barrage then this degenerates to FrB-ABar
                chain1 = new SpellCycle(2);
                chain1.AddSpell(FrB, castingState);
                chain1.AddSpell(ABar, castingState);
                chain1.Calculate(castingState);

                commonChain = chain1;

                CastTime = chain1.CastTime;
                CostPerSecond = chain1.CostPerSecond;
                DamagePerSecond = chain1.DamagePerSecond;
                ThreatPerSecond = chain1.ThreatPerSecond;
                ManaRegenPerSecond = chain1.ManaRegenPerSecond;
            }
            else
            {
                chain1 = new SpellCycle(2);
                chain1.AddSpell(ABar, castingState);
                chain1.AddSpell(FrB, castingState);
                chain1.Calculate(castingState);

                chain2 = new SpellCycle(2);
                chain2.AddSpell(ABar, castingState);
                chain2.AddSpell(MBAM, castingState);
                chain2.Calculate(castingState);

                commonChain = chain2;

                CastTime = (1 - MB) * chain1.CastTime + (2 - MB) * MB * chain2.CastTime;
                CostPerSecond = ((1 - MB) * chain1.CastTime * chain1.CostPerSecond + (2 - MB) * MB * chain2.CastTime * chain2.CostPerSecond) / CastTime;
                DamagePerSecond = ((1 - MB) * chain1.CastTime * chain1.DamagePerSecond + (2 - MB) * MB * chain2.CastTime * chain2.DamagePerSecond) / CastTime;
                ThreatPerSecond = ((1 - MB) * chain1.CastTime * chain1.ThreatPerSecond + (2 - MB) * MB * chain2.CastTime * chain2.ThreatPerSecond) / CastTime;
                ManaRegenPerSecond = ((1 - MB) * chain1.CastTime * chain1.ManaRegenPerSecond + (2 - MB) * MB * chain2.CastTime * chain2.ManaRegenPerSecond) / CastTime;
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

        public override void AddSpellContribution(Dictionary<string, SpellContribution> dict, float duration)
        {
            if (chain2 == null)
            {
                chain1.AddSpellContribution(dict, duration);
            }
            else
            {
                chain1.AddSpellContribution(dict, (1 - MB) * chain1.CastTime / CastTime * duration);
                chain2.AddSpellContribution(dict, (2 - MB) * MB * chain2.CastTime / CastTime * duration);
            }
        }
    }

    class FFBABar : Spell
    {
        SpellCycle chain1;
        SpellCycle chain2;
        float MB;

        public FFBABar(CastingState castingState)
        {
            Name = "FFBABar";
            ABCycle = true;

            Spell FFB = castingState.GetSpell(SpellId.FrostfireBolt);
            Spell ABar = castingState.GetSpell(SpellId.ArcaneBarrage);
            Spell MBAM = castingState.GetSpell(SpellId.ArcaneMissilesMB);

            MB = 0.04f * castingState.MageTalents.MissileBarrage;

            if (MB == 0.0)
            {
                // if we don't have barrage then this degenerates to FB-ABar
                chain1 = new SpellCycle(2);
                chain1.AddSpell(FFB, castingState);
                chain1.AddSpell(ABar, castingState);
                chain1.Calculate(castingState);

                commonChain = chain1;

                CastTime = chain1.CastTime;
                CostPerSecond = chain1.CostPerSecond;
                DamagePerSecond = chain1.DamagePerSecond;
                ThreatPerSecond = chain1.ThreatPerSecond;
                ManaRegenPerSecond = chain1.ManaRegenPerSecond;
            }
            else
            {
                chain1 = new SpellCycle(2);
                chain1.AddSpell(ABar, castingState);
                chain1.AddSpell(FFB, castingState);
                chain1.Calculate(castingState);

                chain2 = new SpellCycle(4);
                chain2.AddSpell(ABar, castingState);
                chain2.AddSpell(MBAM, castingState);
                chain2.Calculate(castingState);

                commonChain = chain2;

                CastTime = (1 - MB) * chain1.CastTime + (2 - MB) * MB * chain2.CastTime;
                CostPerSecond = ((1 - MB) * chain1.CastTime * chain1.CostPerSecond + (2 - MB) * MB * chain2.CastTime * chain2.CostPerSecond) / CastTime;
                DamagePerSecond = ((1 - MB) * chain1.CastTime * chain1.DamagePerSecond + (2 - MB) * MB * chain2.CastTime * chain2.DamagePerSecond) / CastTime;
                ThreatPerSecond = ((1 - MB) * chain1.CastTime * chain1.ThreatPerSecond + (2 - MB) * MB * chain2.CastTime * chain2.ThreatPerSecond) / CastTime;
                ManaRegenPerSecond = ((1 - MB) * chain1.CastTime * chain1.ManaRegenPerSecond + (2 - MB) * MB * chain2.CastTime * chain2.ManaRegenPerSecond) / CastTime;
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

        public override void AddSpellContribution(Dictionary<string, SpellContribution> dict, float duration)
        {
            if (chain2 == null)
            {
                chain1.AddSpellContribution(dict, duration);
            }
            else
            {
                chain1.AddSpellContribution(dict, (1 - MB) * chain1.CastTime / CastTime * duration);
                chain2.AddSpellContribution(dict, (2 - MB) * MB * chain2.CastTime / CastTime * duration);
            }
        }
    }

    class AB : Spell
    {
        BaseSpell AB3;
        SpellCycle chain1;
        SpellCycle chain3;
        SpellCycle chain4;
        Spell AB0M;
        float hit, k21, k31, k41;

        public AB(CastingState castingState)
        {
            Name = "Arcane Blast";
            ABCycle = true;

            // main cycle is AB3 spam
            // spell miss causes debuff reset

            // RAMP =
            // AB0-AB1-AB2           hit*hit*hit = k1
            // AB0-AB1-miss-RAMP hit*hit*(1-hit) = k2
            // AB0-miss-RAMP     hit*(1-hit)     = k3
            // miss-RAMP         (1-hit)         = k4

            // RAMP = k1 * (AB0+AB1+AB2) + k2 * (AB0+AB1+AB2M + RAMP) + k3 * (AB0+AB1M + RAMP) + k4 * (AB0M + RAMP)
            // RAMP * (1 - k2 - k3 - k4) = k1 * (AB0+AB1+AB2) + k2 * (AB0+AB1+AB2M) + k3 * (AB0+AB1M) + k4 * (AB0M)
            // RAMP = (AB0+AB1+AB2) + k2 / k1 * (AB0+AB1+AB2M) + k3 / k1 * (AB0+AB1M) + k4 / k1 * (AB0M)

            // AB3           hit
            // AB3M-RAMP     (1-hit)

            AB3 = (BaseSpell)castingState.GetSpell(SpellId.ArcaneBlast33);
            hit = AB3.HitRate;

            if (AB3.HitRate >= 1.0f || 2 * AB3.CastTime < 3.0)
            {
                // if we have enough hit this is just AB3
                // if we have enough haste to get 2 ABs in 3 sec then assume we get to chain cast, can refine this if desired
                CastTime = AB3.CastTime;
                CostPerSecond = AB3.CostPerSecond;
                DamagePerSecond = AB3.DamagePerSecond;
                ThreatPerSecond = AB3.ThreatPerSecond;
                ManaRegenPerSecond = AB3.ManaRegenPerSecond;
            }
            else
            {
                Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast0Hit);
                Spell AB1 = castingState.GetSpell(SpellId.ArcaneBlast1Hit);
                Spell AB2 = castingState.GetSpell(SpellId.ArcaneBlast2Hit);
                AB3 = (BaseSpell)castingState.GetSpell(SpellId.ArcaneBlast3Hit);
                AB0M = castingState.GetSpell(SpellId.ArcaneBlast0Miss);
                Spell AB1M = castingState.GetSpell(SpellId.ArcaneBlast1Miss);
                Spell AB2M = castingState.GetSpell(SpellId.ArcaneBlast2Miss);
                Spell AB3M = castingState.GetSpell(SpellId.ArcaneBlast3Miss);

                // AB3 hit

                // AB3M-RAMP (1-hit)
                chain1 = new SpellCycle(4);
                chain1.AddSpell(AB3M, castingState);
                chain1.AddSpell(AB0, castingState);
                chain1.AddSpell(AB1, castingState);
                chain1.AddSpell(AB2, castingState);
                chain1.Calculate(castingState);

                chain3 = new SpellCycle(3);
                chain3.AddSpell(AB0, castingState);
                chain3.AddSpell(AB1, castingState);
                chain3.AddSpell(AB2M, castingState);
                chain3.Calculate(castingState);

                chain4 = new SpellCycle(2);
                chain4.AddSpell(AB0, castingState);
                chain4.AddSpell(AB1M, castingState);
                chain4.Calculate(castingState);

                k21 = (1 - hit) / hit;
                k31 = (1 - hit) / hit / hit;
                k41 = (1 - hit) / hit / hit / hit;

                CastTime = hit * AB3.CastTime + (1 - hit) * (chain1.CastTime + k21 * chain3.CastTime + k31 * chain4.CastTime + k41 * AB0M.CastTime);
                CostPerSecond = (hit * AB3.CostPerSecond * AB3.CastTime + (1 - hit) * (chain1.CostPerSecond * chain1.CastTime + k21 * chain3.CostPerSecond * chain3.CastTime + k31 * chain4.CostPerSecond * chain4.CastTime + k41 * AB0M.CostPerSecond * AB0M.CastTime)) / CastTime;
                DamagePerSecond = (hit * AB3.DamagePerSecond * AB3.CastTime + (1 - hit) * (chain1.DamagePerSecond * chain1.CastTime + k21 * chain3.DamagePerSecond * chain3.CastTime + k31 * chain4.DamagePerSecond * chain4.CastTime + k41 * AB0M.DamagePerSecond * AB0M.CastTime)) / CastTime;
                ThreatPerSecond = (hit * AB3.ThreatPerSecond * AB3.CastTime + (1 - hit) * (chain1.ThreatPerSecond * chain1.CastTime + k21 * chain3.ThreatPerSecond * chain3.CastTime + k31 * chain4.ThreatPerSecond * chain4.CastTime + k41 * AB0M.ThreatPerSecond * AB0M.CastTime)) / CastTime;
                ManaRegenPerSecond = (hit * AB3.ManaRegenPerSecond * AB3.CastTime + (1 - hit) * (chain1.ManaRegenPerSecond * chain1.CastTime + k21 * chain3.ManaRegenPerSecond * chain3.CastTime + k31 * chain4.ManaRegenPerSecond * chain4.CastTime + k41 * AB0M.ManaRegenPerSecond * AB0M.CastTime)) / CastTime;
            }
        }

        public override void AddSpellContribution(Dictionary<string, SpellContribution> dict, float duration)
        {
            if (AB3.HitRate >= 1.0f || 2 * AB3.CastTime < 3.0)
            {
                AB3.AddSpellContribution(dict, duration);
            }
            else
            {
                AB3.AddSpellContribution(dict, duration * hit * AB3.CastTime / CastTime);
                chain1.AddSpellContribution(dict, duration * (1 - hit) * chain1.CastTime / CastTime);
                chain3.AddSpellContribution(dict, duration * (1 - hit) * k21 * chain3.CastTime / CastTime);
                chain4.AddSpellContribution(dict, duration * (1 - hit) * k31 * chain4.CastTime / CastTime);
                AB0M.AddSpellContribution(dict, duration * (1 - hit) * k41 * AB0M.CastTime / CastTime);
            }
        }
    }

    class ABMBAM : Spell
    {
        BaseSpell AB3;
        SpellCycle chain1;
        SpellCycle chain2;
        SpellCycle chain3;
        SpellCycle chain4;
        SpellCycle chain5;
        SpellCycle chain6;
        SpellCycle chain7;
        Spell AB0M;
        float MB, MB3, MB4, MB5, MB6, MB7, MB8, hit, miss;

        public ABMBAM(CastingState castingState)
        {
            Name = "ABMBAM";
            ABCycle = true;

            // main cycle is AB3 spam
            // on MB we change into ramp up mode

            // RAMP =
            // AB0-AB1-AB2           0.85*0.85*0.85 = k1
            // AB0-AB1-AB2-(AB3-)MBAM-RAMP 0.85*0.85*0.15 = k2
            // AB0-AB1-(AB2-)MBAM-RAMP     0.85*0.15      = k3
            // AB0-(AB1-)MBAM-RAMP         0.15           = k4

            // RAMP = k1 * (AB0+AB1+AB2) + k2 * (AB0+AB1+AB2+MBAM + RAMP) + k3 * (AB0+AB1+MBAM + RAMP) + k4 * (AB0+MBAM + RAMP)
            // RAMP * (1 - k2 - k3 - k4) = k1 * (AB0+AB1+AB2) + k2 * (AB0+AB1+AB2+MBAM) + k3 * (AB0+AB1+MBAM) + k4 * (AB0+MBAM)
            // RAMP = (AB0+AB1+AB2) + k2 / k1 * (AB0+AB1+AB2+MBAM) + k3 / k1 * (AB0+AB1+MBAM) + k4 / k1 * (AB0+MBAM)

            // RAMP =
            // AB0H-AB1H-AB2H                 0.85*hit*0.85*hit*0.85*hit = k1
            // AB0H-AB1H-AB2H-(AB3-)MBAM-RAMP 0.85*hit*0.85*hit*0.15*hit = k2
            // AB0H-AB1H-(AB2-)MBAM-RAMP      0.85*hit*0.15*hit          = k3
            // AB0H-(AB1-)MBAM-RAMP           0.15*hit                   = k4
            // AB0H-AB1H-AB2M-RAMP            0.85*hit*0.85*hit*miss     = k5
            // AB0H-AB1M-RAMP                 0.85*hit*miss              = k6
            // AB0M-RAMP                      miss                       = k7

            // RAMP = k1 * (AB0H+AB1H+AB2H) + k2 * (AB0H+AB1H+AB2H+AB3+MBAM + RAMP) + k3 * (AB0H+AB1H+AB2+MBAM + RAMP) + k4 * (AB0H+AB1+MBAM + RAMP) + k5 * (AB0H+AB1H+AB2M + RAMP) + k6 * (AB0H+AB1M + RAMP) + k7 * (AB0M + RAMP)
            // RAMP = (AB0H+AB1H+AB2H) + k2 / k1 * (AB0H+AB1H+AB2H+AB3+MBAM) + k3 / k1 * (AB0H+AB1H+AB2+MBAM) + k4 / k1 * (AB0H+AB1+MBAM) + k5 / k1 * (AB0H+AB1H+AB2M) + k6 / k1 * (AB0H+AB1M) + k7 / k1 * (AB0M)

            // AB3H                 0.85*hit
            // AB3H-(AB3-)MBAM-RAMP 0.15*hit
            // AB3M-RAMP            (1-hit)

            Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast00);
            Spell AB1 = castingState.GetSpell(SpellId.ArcaneBlast11);
            Spell AB2 = castingState.GetSpell(SpellId.ArcaneBlast22);
            AB3 = (BaseSpell)castingState.GetSpell(SpellId.ArcaneBlast33);
            Spell MBAM = castingState.GetSpell(SpellId.ArcaneMissilesMB);

            MB = 0.04f * castingState.MageTalents.MissileBarrage;
            hit = AB3.HitRate;
            miss = 1 - hit;

            if (MB == 0.0)
            {
                // TODO take hit rate into account
                // if we don't have barrage then this degenerates to AB
                CastTime = AB3.CastTime;
                CostPerSecond = AB3.CostPerSecond;
                DamagePerSecond = AB3.DamagePerSecond;
                ThreatPerSecond = AB3.ThreatPerSecond;
                ManaRegenPerSecond = AB3.ManaRegenPerSecond;
            }
            else if (AB3.CastTime + MBAM.CastTime < 3.0)
            {
                // TODO take hit rate into account
                //AB3 0.85

                //AB3-MBAM 0.15
                chain1 = new SpellCycle(3);
                chain1.AddSpell(AB3, castingState);
                chain1.AddSpell(AB3, castingState); // account for latency
                chain1.AddSpell(MBAM, castingState);

                CastTime = (1 - MB) * AB3.CastTime + MB * chain1.CastTime;
                CostPerSecond = ((1 - MB) * AB3.CostPerSecond * AB3.CastTime + MB * chain1.CostPerSecond * chain1.CastTime) / CastTime;
                DamagePerSecond = ((1 - MB) * AB3.DamagePerSecond * AB3.CastTime + MB * chain1.DamagePerSecond * chain1.CastTime) / CastTime;
                ThreatPerSecond = ((1 - MB) * AB3.ThreatPerSecond * AB3.CastTime + MB * chain1.ThreatPerSecond * chain1.CastTime) / CastTime;
                ManaRegenPerSecond = ((1 - MB) * AB3.ManaRegenPerSecond * AB3.CastTime + MB * chain1.ManaRegenPerSecond * chain1.CastTime) / CastTime;
            }
            else if (AB3.HitRate >= 1.0f)
            {
                //AB3 0.85

                //AB3-MBAM-RAMP 0.15
                chain1 = new SpellCycle(6);
                chain1.AddSpell(AB3, castingState);
                chain1.AddSpell(AB3, castingState); // account for latency
                chain1.AddSpell(MBAM, castingState);
                chain1.AddSpell(AB0, castingState);
                chain1.AddSpell(AB1, castingState);
                chain1.AddSpell(AB2, castingState);
                chain1.Calculate(castingState);

                chain3 = new SpellCycle(5);
                chain3.AddSpell(AB0, castingState);
                chain3.AddSpell(AB1, castingState);
                chain3.AddSpell(AB2, castingState);
                chain3.AddSpell(AB3, castingState); // account for latency
                chain3.AddSpell(MBAM, castingState);
                chain3.Calculate(castingState);

                chain4 = new SpellCycle(4);
                chain4.AddSpell(AB0, castingState);
                chain4.AddSpell(AB1, castingState);
                chain4.AddSpell(AB2, castingState); // account for latency
                chain4.AddSpell(MBAM, castingState);
                chain4.Calculate(castingState);

                chain5 = new SpellCycle(3);
                chain5.AddSpell(AB0, castingState);
                chain5.AddSpell(AB1, castingState); // account for latency
                chain5.AddSpell(MBAM, castingState);
                chain5.Calculate(castingState);

                MB3 = MB / (1 - MB);
                MB4 = MB / (1 - MB) / (1 - MB);
                MB5 = MB / (1 - MB) / (1 - MB) / (1 - MB);

                CastTime = (1 - MB) * AB3.CastTime + MB * (chain1.CastTime + MB3 * chain3.CastTime + MB4 * chain4.CastTime + MB5 * chain5.CastTime);
                CostPerSecond = ((1 - MB) * AB3.CostPerSecond * AB3.CastTime + MB * (chain1.CostPerSecond * chain1.CastTime + MB3 * chain3.CostPerSecond * chain3.CastTime + MB4 * chain4.CostPerSecond * chain4.CastTime + MB5 * chain5.CostPerSecond * chain5.CastTime)) / CastTime;
                DamagePerSecond = ((1 - MB) * AB3.DamagePerSecond * AB3.CastTime + MB * (chain1.DamagePerSecond * chain1.CastTime + MB3 * chain3.DamagePerSecond * chain3.CastTime + MB4 * chain4.DamagePerSecond * chain4.CastTime + MB5 * chain5.DamagePerSecond * chain5.CastTime)) / CastTime;
                ThreatPerSecond = ((1 - MB) * AB3.ThreatPerSecond * AB3.CastTime + MB * (chain1.ThreatPerSecond * chain1.CastTime + MB3 * chain3.ThreatPerSecond * chain3.CastTime + MB4 * chain4.ThreatPerSecond * chain4.CastTime + MB5 * chain5.ThreatPerSecond * chain5.CastTime)) / CastTime;
                ManaRegenPerSecond = ((1 - MB) * AB3.ManaRegenPerSecond * AB3.CastTime + MB * (chain1.ManaRegenPerSecond * chain1.CastTime + MB3 * chain3.ManaRegenPerSecond * chain3.CastTime + MB4 * chain4.ManaRegenPerSecond * chain4.CastTime + MB5 * chain5.ManaRegenPerSecond * chain5.CastTime)) / CastTime;
            }
            else
            {
                Spell AB0H = castingState.GetSpell(SpellId.ArcaneBlast0Hit);
                Spell AB1H = castingState.GetSpell(SpellId.ArcaneBlast1Hit);
                Spell AB2H = castingState.GetSpell(SpellId.ArcaneBlast2Hit);
                Spell AB3H = castingState.GetSpell(SpellId.ArcaneBlast3Hit);
                AB0M = castingState.GetSpell(SpellId.ArcaneBlast0Miss);
                Spell AB1M = castingState.GetSpell(SpellId.ArcaneBlast1Miss);
                Spell AB2M = castingState.GetSpell(SpellId.ArcaneBlast2Miss);
                Spell AB3M = castingState.GetSpell(SpellId.ArcaneBlast3Miss);
                //AB3H 0.85*hit

                //AB3H-AB3-MBAM-RAMP 0.15*hit
                chain1 = new SpellCycle(6);
                chain1.AddSpell(AB3H, castingState);
                chain1.AddSpell(AB3, castingState); // account for latency
                chain1.AddSpell(MBAM, castingState);
                chain1.AddSpell(AB0H, castingState);
                chain1.AddSpell(AB1H, castingState);
                chain1.AddSpell(AB2H, castingState);
                chain1.Calculate(castingState);

                //AB3M-RAMP miss
                chain2 = new SpellCycle(4);
                chain2.AddSpell(AB3M, castingState);
                chain2.AddSpell(AB0H, castingState);
                chain2.AddSpell(AB1H, castingState);
                chain2.AddSpell(AB2H, castingState);
                chain2.Calculate(castingState);

                chain3 = new SpellCycle(5);
                chain3.AddSpell(AB0H, castingState);
                chain3.AddSpell(AB1H, castingState);
                chain3.AddSpell(AB2H, castingState);
                chain3.AddSpell(AB3, castingState); // account for latency
                chain3.AddSpell(MBAM, castingState);
                chain3.Calculate(castingState);

                chain4 = new SpellCycle(4);
                chain4.AddSpell(AB0H, castingState);
                chain4.AddSpell(AB1H, castingState);
                chain4.AddSpell(AB2, castingState); // account for latency
                chain4.AddSpell(MBAM, castingState);
                chain4.Calculate(castingState);

                chain5 = new SpellCycle(3);
                chain5.AddSpell(AB0H, castingState);
                chain5.AddSpell(AB1, castingState); // account for latency
                chain5.AddSpell(MBAM, castingState);
                chain5.Calculate(castingState);

                chain6 = new SpellCycle(3);
                chain6.AddSpell(AB0H, castingState);
                chain6.AddSpell(AB1H, castingState);
                chain6.AddSpell(AB2M, castingState);
                chain6.Calculate(castingState);

                chain7 = new SpellCycle(2);
                chain7.AddSpell(AB0H, castingState);
                chain7.AddSpell(AB1M, castingState);
                chain7.Calculate(castingState);

                MB3 = MB / (1 - MB);
                MB4 = MB / (1 - MB) / (1 - MB) / hit;
                MB5 = MB / (1 - MB) / (1 - MB) / (1 - MB) / hit / hit;
                MB6 = miss / (1 - MB) / hit;
                MB7 = miss / (1 - MB) / hit / (1 - MB) / hit;
                MB8 = miss / (1 - MB) / hit / (1 - MB) / hit / (1 - MB) / hit;

                CastTime = (1 - MB) * hit * AB3.CastTime + MB * hit * chain1.CastTime + miss * chain2.CastTime + (1 - (1 - MB) * hit) * (MB3 * chain3.CastTime + MB4 * chain4.CastTime + MB5 * chain5.CastTime + MB6 * chain6.CastTime + MB7 * chain7.CastTime + MB8 * AB0M.CastTime);
                CostPerSecond = ((1 - MB) * hit * AB3.CostPerSecond * AB3.CastTime + MB * hit * chain1.CostPerSecond * chain1.CastTime + miss * chain2.CostPerSecond * chain2.CastTime + (1 - (1 - MB) * hit) * (MB3 * chain3.CostPerSecond * chain3.CastTime + MB4 * chain4.CostPerSecond * chain4.CastTime + MB5 * chain5.CostPerSecond * chain5.CastTime + MB6 * chain6.CostPerSecond * chain6.CastTime + MB7 * chain7.CostPerSecond * chain7.CastTime + MB8 * AB0M.CostPerSecond * AB0M.CastTime)) / CastTime;
                DamagePerSecond = ((1 - MB) * hit * AB3.DamagePerSecond * AB3.CastTime + MB * hit * chain1.DamagePerSecond * chain1.CastTime + miss * chain2.DamagePerSecond * chain2.CastTime + (1 - (1 - MB) * hit) * (MB3 * chain3.DamagePerSecond * chain3.CastTime + MB4 * chain4.DamagePerSecond * chain4.CastTime + MB5 * chain5.DamagePerSecond * chain5.CastTime + MB6 * chain6.DamagePerSecond * chain6.CastTime + MB7 * chain7.DamagePerSecond * chain7.CastTime + MB8 * AB0M.DamagePerSecond * AB0M.CastTime)) / CastTime;
                ThreatPerSecond = ((1 - MB) * hit * AB3.ThreatPerSecond * AB3.CastTime + MB * hit * chain1.ThreatPerSecond * chain1.CastTime + miss * chain2.ThreatPerSecond * chain2.CastTime + (1 - (1 - MB) * hit) * (MB3 * chain3.ThreatPerSecond * chain3.CastTime + MB4 * chain4.ThreatPerSecond * chain4.CastTime + MB5 * chain5.ThreatPerSecond * chain5.CastTime + MB6 * chain6.ThreatPerSecond * chain6.CastTime + MB7 * chain7.ThreatPerSecond * chain7.CastTime + MB8 * AB0M.ThreatPerSecond * AB0M.CastTime)) / CastTime;
                ManaRegenPerSecond = ((1 - MB) * hit * AB3.ManaRegenPerSecond * AB3.CastTime + MB * hit * chain1.ManaRegenPerSecond * chain1.CastTime + miss * chain2.ManaRegenPerSecond * chain2.CastTime + (1 - (1 - MB) * hit) * (MB3 * chain3.ManaRegenPerSecond * chain3.CastTime + MB4 * chain4.ManaRegenPerSecond * chain4.CastTime + MB5 * chain5.ManaRegenPerSecond * chain5.CastTime + MB6 * chain6.ManaRegenPerSecond * chain6.CastTime + MB7 * chain7.ManaRegenPerSecond * chain7.CastTime + MB8 * AB0M.ManaRegenPerSecond * AB0M.CastTime)) / CastTime;
            }
        }

        public override void AddSpellContribution(Dictionary<string, SpellContribution> dict, float duration)
        {
            if (chain1 == null)
            {
                AB3.AddSpellContribution(dict, duration);
            }
            else if (chain3 == null)
            {
                AB3.AddSpellContribution(dict, duration * (1 - MB) * AB3.CastTime / CastTime);
                chain1.AddSpellContribution(dict, duration * MB * chain1.CastTime / CastTime);
            }
            else if (AB3.HitRate >= 1.0f)
            {
                AB3.AddSpellContribution(dict, duration * (1 - MB) * AB3.CastTime / CastTime);
                chain1.AddSpellContribution(dict, duration * MB * chain1.CastTime / CastTime);
                chain3.AddSpellContribution(dict, duration * MB * MB3 * chain3.CastTime / CastTime);
                chain4.AddSpellContribution(dict, duration * MB * MB4 * chain4.CastTime / CastTime);
                chain5.AddSpellContribution(dict, duration * MB * MB5 * chain5.CastTime / CastTime);
            }
            else
            {
                AB3.AddSpellContribution(dict, duration * (1 - MB) * hit * AB3.CastTime / CastTime);
                chain1.AddSpellContribution(dict, duration * MB * hit * chain1.CastTime / CastTime);
                chain2.AddSpellContribution(dict, duration * miss * chain1.CastTime / CastTime);
                chain3.AddSpellContribution(dict, duration * (1 - (1 - MB) * hit) * MB3 * chain3.CastTime / CastTime);
                chain4.AddSpellContribution(dict, duration * (1 - (1 - MB) * hit) * MB4 * chain4.CastTime / CastTime);
                chain5.AddSpellContribution(dict, duration * (1 - (1 - MB) * hit) * MB5 * chain5.CastTime / CastTime);
                chain6.AddSpellContribution(dict, duration * (1 - (1 - MB) * hit) * MB6 * chain6.CastTime / CastTime);
                chain7.AddSpellContribution(dict, duration * (1 - (1 - MB) * hit) * MB7 * chain7.CastTime / CastTime);
                AB0M.AddSpellContribution(dict, duration * (1 - (1 - MB) * hit) * MB8 * AB0M.CastTime / CastTime);
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

            if (castingState.MageTalents.ImprovedScorch == 0)
            {
                // in this case just Fireball, scorch debuff won't be applied
                AddSpell(FB, castingState);
                Calculate(castingState);
            }
            else
            {
                int averageScorchesNeeded = (int)Math.Ceiling(3f / (float)castingState.MageTalents.ImprovedScorch);
                int extraScorches = 1;
                if (castingState.CalculationOptions.GlyphOfImprovedScorch)
                {
                    averageScorchesNeeded = 1;
                    extraScorches = 0;
                }
                double timeOnScorch = 30;
                int fbCount = 0;

                while (timeOnScorch > FB.CastTime + (averageScorchesNeeded + extraScorches) * Sc.CastTime) // one extra scorch gap to account for possible resist
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

            if (castingState.MageTalents.ImprovedScorch == 0)
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
                int averageScorchesNeeded = (int)Math.Ceiling(3f / (float)castingState.MageTalents.ImprovedScorch);
                int extraScorches = 1;
                if (castingState.CalculationOptions.GlyphOfImprovedScorch)
                {
                    averageScorchesNeeded = 1;
                    extraScorches = 0;
                }
                double timeOnScorch = 30;
                int fbCount = 0;
                float blastCooldown = 0;

                do
                {
                    float expectedTimeWithBlast = Blast.CastTime + (int)((timeOnScorch - (blastCooldown > 0f ? blastCooldown : 0f) - Blast.CastTime - (averageScorchesNeeded + extraScorches) * Sc.CastTime) / FB.CastTime) * FB.CastTime + averageScorchesNeeded * Sc.CastTime;
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
                    else if (timeOnScorch > FB.CastTime + (averageScorchesNeeded + extraScorches) * Sc.CastTime) // one extra scorch gap to account for possible resist
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
        SpellCycle chain1;
        SpellCycle chain2;
        SpellCycle chain3;
        SpellCycle chain4;
        float CC;

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

            CC = 0.02f * castingState.MageTalents.ArcaneConcentration;

            //AMCC-AB0                       0.1
            chain1 = new SpellCycle(2);
            chain1.AddSpell(AMCC, castingState);
            chain1.AddSpell(AB0, castingState);
            chain1.Calculate(castingState);

            //AM?0-AB1-AMCC-AB0              0.9*0.1
            chain2 = new SpellCycle(4);
            chain2.AddSpell(AMc0, castingState);
            chain2.AddSpell(AB1, castingState);
            chain2.AddSpell(AMCC, castingState);
            chain2.AddSpell(AB0, castingState);
            chain2.Calculate(castingState);

            //AM?0-AB1-AM?0-AB2-AMCC-AB0     0.9*0.9*0.1
            chain3 = new SpellCycle(6);
            chain3.AddSpell(AMc0, castingState);
            chain3.AddSpell(AB1, castingState);
            chain3.AddSpell(AMc0, castingState);
            chain3.AddSpell(AB2, castingState);
            chain3.AddSpell(AMCC, castingState);
            chain3.AddSpell(AB0, castingState);
            chain3.Calculate(castingState);

            //AM?0-AB1-AM?0-AB2-AM?0-S-AB3?  0.9*0.9*0.9
            chain4 = new SpellCycle(13);
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

        public override void AddSpellContribution(Dictionary<string, SpellContribution> dict, float duration)
        {
            chain1.AddSpellContribution(dict, duration * CC * chain1.CastTime / CastTime);
            chain2.AddSpellContribution(dict, duration * CC * (1 - CC) * chain2.CastTime / CastTime);
            chain3.AddSpellContribution(dict, duration * CC * (1 - CC) * (1 - CC) * chain3.CastTime / CastTime);
            chain4.AddSpellContribution(dict, duration * (1 - CC) * (1 - CC) * (1 - CC) * chain4.CastTime / CastTime);
        }
    }

    class ABAM3Sc2CCAM : Spell
    {
        SpellCycle chain1;
        SpellCycle chain2;
        SpellCycle chain3;
        SpellCycle chain4;
        float CC;

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

            CC = 0.02f * castingState.MageTalents.ArcaneConcentration;

            //AMCC-AB0                       0.1
            chain1 = new SpellCycle(2);
            chain1.AddSpell(AMCC, castingState);
            chain1.AddSpell(AB0, castingState);
            chain1.Calculate(castingState);

            //AM?0-AB1-AMCC-AB0              0.9*0.1
            chain2 = new SpellCycle(4);
            chain2.AddSpell(AMc0, castingState);
            chain2.AddSpell(AB1, castingState);
            chain2.AddSpell(AMCC, castingState);
            chain2.AddSpell(AB0, castingState);
            chain2.Calculate(castingState);

            //AM?0-AB1-AM?0-AB2-AMCC-AB0     0.9*0.9*0.1
            chain3 = new SpellCycle(13);
            chain3.AddSpell(AMc0, castingState);
            chain3.AddSpell(AB1, castingState);
            chain3.AddSpell(AMc0, castingState);
            chain3.AddSpell(AB2, castingState);
            chain3.AddSpell(AMCC, castingState);
            chain3.AddSpell(AB0, castingState);
            chain3.Calculate(castingState);

            //AM?0-AB1-AM?0-AB2-AM?0-S-AB3?  0.9*0.9*0.9
            chain4 = new SpellCycle();
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

        public override void AddSpellContribution(Dictionary<string, SpellContribution> dict, float duration)
        {
            chain1.AddSpellContribution(dict, duration * CC * chain1.CastTime / CastTime);
            chain2.AddSpellContribution(dict, duration * CC * (1 - CC) * chain2.CastTime / CastTime);
            chain3.AddSpellContribution(dict, duration * CC * (1 - CC) * (1 - CC) * chain3.CastTime / CastTime);
            chain4.AddSpellContribution(dict, duration * (1 - CC) * (1 - CC) * (1 - CC) * chain4.CastTime / CastTime);
        }
    }

    class ABAM3FrBCCAM : Spell
    {
        SpellCycle chain1;
        SpellCycle chain2;
        SpellCycle chain3;
        SpellCycle chain4;
        float CC;

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

            CC = 0.02f * castingState.MageTalents.ArcaneConcentration;

            //AMCC-AB0                       0.1
            chain1 = new SpellCycle(2);
            chain1.AddSpell(AMCC, castingState);
            chain1.AddSpell(AB0, castingState);
            chain1.Calculate(castingState);

            //AM?0-AB1-AMCC-AB0              0.9*0.1
            chain2 = new SpellCycle(4);
            chain2.AddSpell(AMc0, castingState);
            chain2.AddSpell(AB1, castingState);
            chain2.AddSpell(AMCC, castingState);
            chain2.AddSpell(AB0, castingState);
            chain2.Calculate(castingState);

            //AM?0-AB1-AM?0-AB2-AMCC-AB0     0.9*0.9*0.1
            chain3 = new SpellCycle(6);
            chain3.AddSpell(AMc0, castingState);
            chain3.AddSpell(AB1, castingState);
            chain3.AddSpell(AMc0, castingState);
            chain3.AddSpell(AB2, castingState);
            chain3.AddSpell(AMCC, castingState);
            chain3.AddSpell(AB0, castingState);
            chain3.Calculate(castingState);

            //AM?0-AB1-AM?0-AB2-AM?0-S-AB3?  0.9*0.9*0.9
            chain4 = new SpellCycle(13);
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

        public override void AddSpellContribution(Dictionary<string, SpellContribution> dict, float duration)
        {
            chain1.AddSpellContribution(dict, duration * CC * chain1.CastTime / CastTime);
            chain2.AddSpellContribution(dict, duration * CC * (1 - CC) * chain2.CastTime / CastTime);
            chain3.AddSpellContribution(dict, duration * CC * (1 - CC) * (1 - CC) * chain3.CastTime / CastTime);
            chain4.AddSpellContribution(dict, duration * (1 - CC) * (1 - CC) * (1 - CC) * chain4.CastTime / CastTime);
        }
    }

    class ABAM3FrBCCAMFail : Spell
    {
        SpellCycle chain1;
        SpellCycle chain2;
        SpellCycle chain3;
        SpellCycle chain4;
        float CC;

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

            CC = 0.02f * castingState.MageTalents.ArcaneConcentration;

            //AMCC-AB0                       0.1
            chain1 = new SpellCycle(2);
            chain1.AddSpell(AMCC, castingState);
            chain1.AddSpell(AB0, castingState);
            chain1.Calculate(castingState);

            //AM?0-AB1-AMCC-AB0              0.9*0.1
            chain2 = new SpellCycle(4);
            chain2.AddSpell(AMc0, castingState);
            chain2.AddSpell(AB1, castingState);
            chain2.AddSpell(AMCC, castingState);
            chain2.AddSpell(AB0, castingState);
            chain2.Calculate(castingState);

            //AM?0-AB1-AM?0-AB2-AMCC-AB0     0.9*0.9*0.1
            chain3 = new SpellCycle(6);
            chain3.AddSpell(AMc0, castingState);
            chain3.AddSpell(AB1, castingState);
            chain3.AddSpell(AMc0, castingState);
            chain3.AddSpell(AB2, castingState);
            chain3.AddSpell(AMCC, castingState);
            chain3.AddSpell(AB0, castingState);
            chain3.Calculate(castingState);

            //AM?0-AB1-AM?0-AB2-AM?0-S-AB3?  0.9*0.9*0.9
            chain4 = new SpellCycle(13);
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

        public override void AddSpellContribution(Dictionary<string, SpellContribution> dict, float duration)
        {
            chain1.AddSpellContribution(dict, duration * CC * chain1.CastTime / CastTime);
            chain2.AddSpellContribution(dict, duration * CC * (1 - CC) * chain2.CastTime / CastTime);
            chain3.AddSpellContribution(dict, duration * CC * (1 - CC) * (1 - CC) * chain3.CastTime / CastTime);
            chain4.AddSpellContribution(dict, duration * (1 - CC) * (1 - CC) * (1 - CC) * chain4.CastTime / CastTime);
        }
    }

    class ABAM3FrBScCCAM : Spell
    {
        SpellCycle chain1;
        SpellCycle chain2;
        SpellCycle chain3;
        SpellCycle chain4;
        float CC;

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

            CC = 0.02f * castingState.MageTalents.ArcaneConcentration;

            //AMCC-AB0                       0.1
            chain1 = new SpellCycle(2);
            chain1.AddSpell(AMCC, castingState);
            chain1.AddSpell(AB0, castingState);
            chain1.Calculate(castingState);

            //AM?0-AB1-AMCC-AB0              0.9*0.1
            chain2 = new SpellCycle(4);
            chain2.AddSpell(AMc0, castingState);
            chain2.AddSpell(AB1, castingState);
            chain2.AddSpell(AMCC, castingState);
            chain2.AddSpell(AB0, castingState);
            chain2.Calculate(castingState);

            //AM?0-AB1-AM?0-AB2-AMCC-AB0     0.9*0.9*0.1
            chain3 = new SpellCycle(6);
            chain3.AddSpell(AMc0, castingState);
            chain3.AddSpell(AB1, castingState);
            chain3.AddSpell(AMc0, castingState);
            chain3.AddSpell(AB2, castingState);
            chain3.AddSpell(AMCC, castingState);
            chain3.AddSpell(AB0, castingState);
            chain3.Calculate(castingState);

            //AM?0-AB1-AM?0-AB2-AM?0-S-AB3?  0.9*0.9*0.9
            chain4 = new SpellCycle(13);
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

        public override void AddSpellContribution(Dictionary<string, SpellContribution> dict, float duration)
        {
            chain1.AddSpellContribution(dict, duration * CC * chain1.CastTime / CastTime);
            chain2.AddSpellContribution(dict, duration * CC * (1 - CC) * chain2.CastTime / CastTime);
            chain3.AddSpellContribution(dict, duration * CC * (1 - CC) * (1 - CC) * chain3.CastTime / CastTime);
            chain4.AddSpellContribution(dict, duration * (1 - CC) * (1 - CC) * (1 - CC) * chain4.CastTime / CastTime);
        }
    }

    class ABAMCCAM : Spell
    {
        SpellCycle chain1;
        SpellCycle chain2;
        float CC;

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

            CC = 0.02f * castingState.MageTalents.ArcaneConcentration;

            if (CC == 0)
            {
                // if we don't have clearcasting then this degenerates to AMc0-AB33
                chain1 = new SpellCycle(2);
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
                chain1 = new SpellCycle(5);
                chain1.AddSpell(AMCC, castingState);
                chain1.AddSpell(AB00, castingState);
                chain1.AddSpell(AB01, castingState);
                chain1.AddSpell(AB12, castingState);
                chain1.AddSpell(AB23, castingState);
                chain1.Calculate(castingState);

                //AM?0-AB33                      0.9
                chain2 = new SpellCycle(2);
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

        public override void AddSpellContribution(Dictionary<string, SpellContribution> dict, float duration)
        {
            if (CC == 0)
            {
                chain1.AddSpellContribution(dict, duration);
            }
            else
            {
                chain1.AddSpellContribution(dict, duration * CC * chain1.CastTime / CastTime);
                chain2.AddSpellContribution(dict, duration * (1 - CC) * chain2.CastTime / CastTime);
            }
        }
    }

    class ABAM3CCAM : Spell
    {
        SpellCycle chain1;
        SpellCycle chain2;
        SpellCycle chain3;
        float CC;

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

            CC = 0.02f * castingState.MageTalents.ArcaneConcentration;

            if (CC == 0)
            {
                // if we don't have clearcasting then this degenerates to AMc0-AB33
                chain1 = new SpellCycle(2);
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
                chain1 = new SpellCycle(24);
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
                chain2 = new SpellCycle(4);
                chain2.AddSpell(AB0, castingState);
                chain2.AddSpell(AMc0, castingState);
                chain2.AddSpell(AB1, castingState);
                chain2.AddSpell(AMCC, castingState);
                chain2.Calculate(castingState);

                //AB00-AMCC                                           0.1
                chain3 = new SpellCycle(2);
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

        public override void AddSpellContribution(Dictionary<string, SpellContribution> dict, float duration)
        {
            if (CC == 0)
            {
                chain1.AddSpellContribution(dict, duration);
            }
            else
            {
                chain1.AddSpellContribution(dict, duration * (1 - CC) * (1 - CC) * chain1.CastTime / CastTime);
                chain2.AddSpellContribution(dict, duration * CC * (1 - CC) * chain2.CastTime / CastTime);
                chain3.AddSpellContribution(dict, duration * CC * chain3.CastTime / CastTime);
            }
        }
    }
    #endregion
}
